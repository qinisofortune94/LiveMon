using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class UserEditDashboard : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (((string)Session["LoggedIn"] == "True"))
                {
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    Label user = this.Master.FindControl("lblUser") as Label;
                    Label loginUser = this.Master.FindControl("lblUser2") as Label;
                    Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                    loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                    user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                    LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);

                    if (!Page.IsPostBack)
                    {
                        drpChartTypes.Items.Clear();
                        FillChartTypes(drpChartTypes);
                        drpSensor.Items.Clear();
                        FillSensors(drpSensor, User.Identity.Name.ToString());
                        divEditDashboard.Visible = false;
                        //gridDashboards.DataSource = getDashboards(MyUser.ID);
                        //gridDashboards.DataBind();

                        LoadUserDash.Items.Clear();
                        LoadDropForDash(LoadUserDash,MyUser.ID.ToString());
                        gridDashboards.DataSource = getDashboardsFromDropDown(Convert.ToInt32(LoadUserDash.SelectedValue));
                        gridDashboards.DataBind();     
                    }
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
        }

        public void FillChartTypes(DropDownList DropDownLisChartTypes)
        {
            string sqlQuery = "[Dashboards].[spGetChartTypes]";
            SqlConnection connection = new SqlConnection(conStr);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                DropDownLisChartTypes.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownLisChartTypes.Items.Add(new ListItem(reader["ChartType"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private DataTable getDashboards(int pintUserID)
        {
            string pstrGroupId = "";
            string sqlQuery = "[Dashboards].[spGetUserDashboards]";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("UserID", pintUserID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        private DataTable getDashboardsFromDropDown(int pintUserID)
        {
            string pstrGroupId = "";
            string sqlQuery = "[Dashboards].[spGetUserDashboards2]";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("UserID", pintUserID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        public void FillSensors(DropDownList DropDownListSensors, string pintGroupID)
        {
            string sqlQuery = "[Dashboards].[spGetSensors]";
            SqlConnection connection = new SqlConnection(conStr);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("GroupId", pintGroupID);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                DropDownListSensors.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownListSensors.Items.Add(new ListItem(reader["Sensor"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void drpSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                drpField.Items.Clear();
                FillFields(drpField, drpSensor.SelectedValue);
            }
            catch (Exception ex)
            {
            }

        }

        public void FillFields(DropDownList DropDownListFields, string pintSensorID)
        {
            string sqlQuery = "[Dashboards].[spGetFields]";
            SqlConnection connection = new SqlConnection(conStr);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("SensorId", pintSensorID);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                DropDownListFields.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownListFields.Items.Add(new ListItem(reader["Field"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }

        }

        public void LoadDropForDash(DropDownList DropDownListFields, string pintSensorID)
        {
            string sqlQuery = "[Dashboards].[spGetUserDashToDropdown]";
            SqlConnection connection = new SqlConnection(conStr);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("userId", pintSensorID);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                LoadUserDash.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    LoadUserDash.Items.Add(new ListItem(reader["Field"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }

        }

        public bool canSave()
        {
            int tR = Convert.ToInt32(txtRowsPos.Text);
            int tC = Convert.ToInt32(txtColsPos.Text);

            if (txtGraphName.Text.Trim().Length == 0)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please enter a valid graph name!";
                txtGraphName.Focus();
                return false;
            }

            if (txtRowsPos.Text.Trim().Length == 0)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please enter a valid row!";
                txtRowsPos.Focus();
                return false;
            }

            if (txtColsPos.Text.Trim().Length == 0)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please enter a valid row position!";
                txtColsPos.Focus();
                return false;
            }
            if (Information.IsNumeric(txtRowsPos.Text) == false)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please enter only numbers on the row field!";
                txtRowsPos.Focus();
            }
            if (Information.IsNumeric(txtColsPos.Text) == false)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please enter only numbers on the row position field!";
                txtColsPos.Focus();
            }

            if (tR > 2 | tR <= 0)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Row can either be 1 or 2!";
                txtRowsPos.Focus();
            }
            if (tC > 3 | tC <= 0)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Row position can either be 1,2 or 3!";
                txtColsPos.Focus();
            }
            if (drpChartTypes.SelectedItem == null)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please select a valid chart type!";
                drpChartTypes.Focus();
                return false;
            }
            if (drpSensor.SelectedItem == null)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please select Sensor!";
                drpSensor.Focus();
                return false;
            }
            if (drpField.SelectedItem == null)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please select a valid field!";
                drpField.Focus();
                return false;
            }
            return true;
        }

        protected void gridDashboards_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                divEditDashboard.Visible = true;
                txtGraphName.Text = "";
                txtRowsPos.Text = "";
                txtColsPos.Text = "";
                int minus = -1;
                //drpChartTypes.SelectedItem.Text = minus.ToString();
                //drpSensor.SelectedItem.Text = minus.ToString();

                txtDataHours.Text = "";
                lblSuccess.Text = "";
                txtGraphName.Text = gridDashboards.SelectedRow.Cells[2].Text.ToUpper();
                txtRowsPos.Text = gridDashboards.SelectedRow.Cells[3].Text;
                txtColsPos.Text = gridDashboards.SelectedRow.Cells[4].Text;
                drpChartTypes.SelectedItem.Text = gridDashboards.SelectedRow.Cells[5].Text.ToUpper();
                drpSensor.SelectedItem.Text = gridDashboards.SelectedRow.Cells[6].Text.ToUpper();
                drpField.Items.Clear();
                FillFields(drpField, drpSensor.SelectedValue);
                drpField.SelectedItem.Text = gridDashboards.SelectedRow.Cells[7].Text.ToUpper();
                txtDataHours.Text = gridDashboards.SelectedRow.Cells[8].Text;
                //Session["hh"] = drpSensor.SelectedItem.Value;
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        public bool EditDashBoards()
        {
            bool Saved = false;

            try
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                LiveMonitoring.IRemoteLib.UserDashBoards MyDashboard = new LiveMonitoring.IRemoteLib.UserDashBoards();
                MyDashboard.ID = Convert.ToInt32(gridDashboards.SelectedRow.Cells[1].Text);
                MyDashboard.UserID = MyUser.ID;
                MyDashboard.GraphName = txtGraphName.Text;
                MyDashboard.RowPos = Convert.ToInt32(txtRowsPos.Text);
                MyDashboard.ColPos = Convert.ToInt32(txtColsPos.Text);
                MyDashboard.ChartType = (LiveMonitoring.IRemoteLib.liveMonChartType)Convert.ToInt32(drpChartTypes.SelectedValue);

                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if (MyRem.LiveMonServer.EditUserDashBoards(MyUser.UserName, false, MyDashboard) == true)
                {
                    Saved = true;
                    //save parameters
                    LiveMonitoring.IRemoteLib.UserDashBoardParameters MyParameters = new LiveMonitoring.IRemoteLib.UserDashBoardParameters();
                    MyParameters.ID = Convert.ToInt32(gridDashboards.SelectedRow.Cells[9].Text);
                    MyParameters.UserDashID = Convert.ToInt32(gridDashboards.SelectedRow.Cells[1].Text);
                    MyParameters.SensorID = Convert.ToInt32(drpSensor.SelectedItem.Value);
                    MyParameters.FieldNo = Convert.ToInt32(drpField.SelectedValue);
                    MyParameters.DataHours = Convert.ToInt32(txtDataHours.Text);
                    MyParameters.DataGroupBy = true;
                    MyParameters.GroupBy = "none";
                    MyParameters.UseCaptionLabel = true;
                    MyParameters.AlternateCaption = "none";
                    MyParameters.CapturedBy = MyUser.UserName;
                    MyParameters.Color = 1;
                    MyParameters.isActive = chkDelete.Checked;
                    if (MyRem.LiveMonServer.EditDashBoardParameters(MyUser.UserName, chkDelete.Checked, MyParameters) == true)
                    {
                        Saved = true;
                    }
                    else
                    {
                        Saved = false;
                    }
                }
                else
                {
                    Saved = false;
                }
            }
            catch (Exception ex)
            {
                Saved = false;
                lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = ex.Message.ToString();
            }
            return Saved;
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                lblSuccess.Text = "";
                if (canSave() == true)
                {
                    if (EditDashBoards() == true)
                    {
                        lblSuccess.Text = "Dashboard settings were successfully updated!";
                        //gridDashboards.DataSource = getDashboards(MyUser.ID);
                        //gridDashboards.DataBind();
                        gridDashboards.DataSource = getDashboardsFromDropDown(Convert.ToInt32(LoadUserDash.SelectedValue));
                        gridDashboards.DataBind();
                        txtColsPos.Text = "";
                        txtRowsPos.Text = "";
                        txtGraphName.Text = "";
                        drpChartTypes.SelectedIndex = 3;
                        //drpSensor.SelectedIndex = -1;
                        //drpField.SelectedIndex = -1;
                    }
                    else
                    {
                        //lblSuccess.BackColor = System.Drawing.Color.Red;
                        lblSuccess.Text = "An error occured while saving dashboard setting please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = ex.Message.ToString();
            }
        }

        protected void gridDashboards_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                gridDashboards.PageIndex = e.NewPageIndex;
                //gridDashboards.DataSource = getDashboards(MyUser.ID);
                //gridDashboards.DataBind();
                gridDashboards.DataSource = getDashboardsFromDropDown(Convert.ToInt32(LoadUserDash.SelectedValue));
                gridDashboards.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        public void Dashboard_UserEditDashboard()
        {
            Load += Page_Load;
        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
        }

        protected void editAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx");
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Response.Redirect("Index.aspx");
        }

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }

        protected void LoadUserDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridDashboards.DataSource = getDashboardsFromDropDown(Convert.ToInt32(LoadUserDash.SelectedValue));
            gridDashboards.DataBind();
        }
    }
}