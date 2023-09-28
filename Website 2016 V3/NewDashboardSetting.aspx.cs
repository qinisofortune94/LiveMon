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
    public partial class NewDashboardSetting : System.Web.UI.Page
    {
        private string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
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

                    drpSensor.Items.Clear();
                    drpChartTypes.Items.Clear();
                    FillSensors(drpSensor, User.Identity.Name.ToString());
                    FillChartTypes(drpChartTypes);
                    drpSensor.SelectedIndex = -1;
                    DeleteAllTempDashboardField();
                    LoadTempDashBoardField();

                    drpField.Items.Clear();
                    FillFields(drpField, drpSensor.SelectedValue);
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
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
            if (gridDashboards.Rows.Count == 0)
            {
                //lblSuccess.BackColor = System.Drawing.Color.Red;
                lblSuccess.Text = "Please add atleast one field!";
                drpField.Focus();
                return false;
            }
            return true;
        }

        protected void btnnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                LiveMonitoring.IRemoteLib.UserDashBoards MyDashboard = new LiveMonitoring.IRemoteLib.UserDashBoards();
                LiveMonitoring.IRemoteLib.UserDashBoardParameters MyParameters = new LiveMonitoring.IRemoteLib.UserDashBoardParameters();

                if (canSave())
                {
                    MyDashboard.UserID = MyUser.ID;
                    MyDashboard.GraphName = txtGraphName.Text;
                    MyDashboard.RowPos = Convert.ToInt32(txtRowsPos.Text);
                    MyDashboard.ColPos = Convert.ToInt32(txtColsPos.Text);
                    MyDashboard.ChartType = (LiveMonitoring.IRemoteLib.liveMonChartType)Convert.ToInt32(drpChartTypes.SelectedValue);

                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    int DashboardId = 0;
                    int ey = 1;
                    DashboardId = MyRem.LiveMonServer.AddUserDashBoards(MyUser.ID.ToString(), MyDashboard, drpSensor.SelectedValue, ey.ToString(), txtDataHours.Text);
                    MyDashboard.ID = DashboardId;
                    int i = 0;
                    for (i = 0; i <= gridDashboards.Rows.Count - 1; i++)
                    {
                        if (DashboardId > 0)
                        {
                            MyRem.LiveMonServer.AddUserDashboardParameters(MyUser.ID.ToString(), MyDashboard, drpSensor.SelectedValue, gridDashboards.Rows[i].Cells[1].Text, txtDataHours.Text);

                        }
                        else
                        {
                            //lblSuccess.ForeColor = System.Drawing.Color.Red;
                            lblSuccess.Text = "An error occured while saving settings, please try again or contact your system administrator.";
                        }
                    }
                    //lblSuccess.ForeColor = System.Drawing.Color.Black;
                    lblSuccess.Text = "Dashboard settings were successfully saved!";
                    try
                    {
                        DeleteAllTempDashboardField();
                        LoadTempDashBoardField();
                    }
                    catch (Exception ex)
                    {
                    }
                    txtGraphName.Text = "";
                    drpChartTypes.SelectedIndex = -1;
                    drpSensor.SelectedIndex = -1;
                    drpField.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                //lblSuccess.ForeColor = System.Drawing.Color.Red;
                lblSuccess.Text = ex.Message;
            }
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

        protected void btnnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpField.SelectedItem != null)
                {
                    if (SaveTempDashboardField() == true)
                    {
                        lblSuccess.Text = "Field was successfully added.";
                        LoadTempDashBoardField();
                    }
                    else
                    {
                        lblSuccess.Text = "A server related error occured, make sure you connected to the database server. If this error persist contact your system administrator.";
                    }
                }
                else
                {
                    lblSuccess.Text = "Please select a field to add!";
                }
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        private bool SaveTempDashboardField()
        {
            string sqlQuery = "[Dashboards].[spSaveTempDashboardField]";
            SqlConnection connection = new SqlConnection(conStr);

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserId", MyUser.ID);
            cmd.Parameters.AddWithValue("FieldId", drpField.SelectedValue);
            cmd.Parameters.AddWithValue("Field", drpField.SelectedItem.Text);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                cmd.ExecuteNonQuery();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private bool DeleteTempDashboardField()
        {
            string sqlQuery = "[Dashboards].[spDeleteTempDashboardsField]";
            SqlConnection connection = new SqlConnection(conStr);

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserId", MyUser.ID);
            cmd.Parameters.AddWithValue("FieldId", gridDashboards.SelectedRow.Cells[1].Text);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                cmd.ExecuteNonQuery();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private bool DeleteAllTempDashboardField()
        {
            string sqlQuery = "[Dashboards].[DeleteAllTempDashboardField]";
            SqlConnection connection = new SqlConnection(conStr);

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserId", MyUser.ID);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                cmd.ExecuteNonQuery();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private DataTable getTempDashboardFields()
        {

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            string sqlQuery = "[Dashboards].[spGetTempDashboardsField]";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("UserID", MyUser.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        public void LoadTempDashBoardField()
        {
            gridDashboards.DataSource = getTempDashboardFields();
            gridDashboards.DataBind();
        }

        protected void gridDashboards_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DeleteTempDashboardField() == true)
                {
                    lblSuccess.Text = "Field successfully deleted!";
                    LoadTempDashBoardField();
                }
                else
                {
                    lblSuccess.Text = "An error occured while trying to delete field, make sure you connected to the database server. If this error persist please contact your system administrator.";
                }
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        public void Dashboard_UserAddDashboard()
        {
            Load += Page_Load;
        }
    }
}