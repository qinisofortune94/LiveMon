using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Linq;
using System.Web.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace website2016V2
{
    public partial class AddSensorNote : System.Web.UI.Page
    {
        
        string conStr = WebConfigurationManager.AppSettings["DataBaseCon"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser1 = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser1 = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                user.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                LastLogin.Text = " LL:" + MyUser1.LoginDT.ToString();

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;
                lblDelete.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("~/NotAuthorisedView.aspx");
                    EditNotes1.Visible = false;
                }
                //Edit.Visible = false;
                //AddNew.Visible = false;
                if(Page.IsPostBack == false)
                {
                    drpViewSensor.Items.Clear();
                    FillSensors(drpViewSensor, MyUser.ID.ToString());
                    drpSensor.Items.Clear();
                    drpViewSensor.SelectedIndex = -1;
                    FillSensors(drpSensor, MyUser.ID.ToString());
                    drpUser.Items.Clear();
                    drpSensor.SelectedIndex = -1;
                    FillEmployees(drpUser, MyUser.ID.ToString());
                    drpEditSensor.Items.Clear();
                    FillSensors(drpEditSensor, MyUser.ID.ToString());
                    drpEditUsers.Items.Clear();
                    FillEmployees(drpEditUsers, MyUser.ID.ToString());
                    drpViewSensor.SelectedIndex = -1;

                    LoadSensors(Convert.ToInt32(drpViewSensor.SelectedValue));
                }    
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void FillSensors(DropDownList DropDownListSensors, string pintGroupID)
        {
            string sqlQuery = "[Dashboards].[spGetAllSensors]";
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

        public void FillEmployees(DropDownList DropDownListUsers, string pintGroupID)
        {
            string sqlQuery = "[Dashboards].[spGetUsers]";
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
                DropDownListUsers.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownListUsers.Items.Add(new ListItem(reader["Employee"].ToString(), reader["ID"].ToString()));
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

        public void LoadSensors(int Sensorid)
        {
            gridNotes.DataSource = getSensors(Sensorid);
            gridNotes.DataBind();

        }
        private DataTable getSensors(int sensorId)
        {


            string sqlQuery = "[Sensors].[spGetAllSensorNotes]";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("SensorId", sensorId);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        protected void gridDashboards_SelectedIndexChanged(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                //AddNew.Visible = false;
                //Edit.Visible = true;
                drpEditSensor.SelectedItem.Text = Convert.ToString(-1);
                drpEditUsers.SelectedItem.Text = Convert.ToString(-1);
                drpEditField.SelectedIndex = -1;
                txtEditNotes.Text = "";
                //lblEditSuccess.Text = "";

                drpEditSensor.SelectedItem.Text = gridNotes.SelectedRow.Cells[2].Text.ToUpper();
                FillFields(drpEditField, drpEditSensor.SelectedValue);
                drpEditField.SelectedItem.Text = gridNotes.SelectedRow.Cells[3].Text.ToUpper();
                txtEditNotes.Text = gridNotes.SelectedRow.Cells[4].Text.ToUpper();
                drpEditUsers.SelectedItem.Text = gridNotes.SelectedRow.Cells[6].Text.ToUpper();
                //add eventdate
                this.EditEventDate.Text = gridNotes.SelectedRow.Cells[5].Text;
                successMessage.Visible = true;
                lblSuccess.Text = "yeee";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        //protected void gridDashboards_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage.Visible = true;
        //        lblError.Text = ex.Message;
        //    }
        //}

        public void FillSites(DropDownList DropDownListSites, string pintGroupID)
        {
            string sqlQuery = "[Sites].[spGetSites]";
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
                DropDownListSites.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownListSites.Items.Add(new ListItem(reader["Site"].ToString(), reader["ID"].ToString()));
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

        public void FillOtherDevices(DropDownList DropDownLisDevices)
        {
            string sqlQuery = "[Sites].[spGetSNMPDevices]";
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
                DropDownLisDevices.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownLisDevices.Items.Add(new ListItem(reader["Device"].ToString(), reader["ID"].ToString()));
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
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
           // AddNew.Visible = true;
            //Edit.Visible = false;
        }


        public bool canSave()
        {

            if (drpSensor.SelectedItem == null)
            {
               // lblSuccess.BackColor = Drawing.Color.Red;
                lblSuccess.Text = "Please select a valid sensor!";
                drpSensor.Focus();
                return false;
            }

            if (drpSensorField.SelectedItem == null)
            {
                //lblSuccess.BackColor = Drawing.Color.Red;
                lblSuccess.Text = "Please select a valid Sensor Field!";
                drpSensorField.Focus();
                return false;
            }

            if (drpUser.SelectedItem == null)
            {
                //lblSuccess.BackColor = Drawing().Color.Red;
                lblSuccess.Text = "Please select a valid Employee!";
                drpUser.Focus();
                return false;
            }

            if (txtNotes.Text.Trim().Length == 0)
            {
                //lblSuccess.BackColor = Drawing.Color.Red;
                lblSuccess.Text = "Please enter the notes!";
                txtNotes.Focus();
                return false;
            }
            
            DateTime eventdate = (Convert.ToDateTime(this.Request.Form["EventDate"]));

            if (eventdate.ToShortDateString().Trim().Length == 0)
            {
                //lblSuccess.BackColor = Drawing.Color.Red;
                lblSuccess.Text = "Please enter valid event date!";
                return false;
            }
            return true;
        }
        public bool EditcanSave()
        {
            if (drpEditSensor.SelectedItem == null)
            {
                //lblEditSuccess.BackColor = Drawing.Color.Red;
                //lblEditSuccess.Text = "Please select a valid sensor!";
                drpEditSensor.Focus();
                return false;
            }

            if (drpEditField.SelectedItem == null)
            {
                //lblEditSuccess.BackColor = Drawing.Color.Red;
                //lblEditSuccess.Text = "Please select a valid Sensor Field!";
                drpEditField.Focus();
                return false;
            }

            if (drpEditUsers.SelectedItem == null)
            {
               // lblEditSuccess.BackColor = Drawing.Color.Red;
               // lblEditSuccess.Text = "Please select a valid Employee!";
                drpEditUsers.Focus();
                return false;
            }

            if (txtEditNotes.Text.Trim().Length == 0)
            {
                //lblEditSuccess.BackColor = Drawing.Color.Red;
               // lblEditSuccess.Text = "Please enter the notes!";
                txtEditNotes.Focus();
                return false;
            }
            DateTime eventdate = DateTime.Parse(Request.Form["EditEventDate"]);

            if (eventdate.ToShortDateString().Trim().Length == 0)
            {
                //lblEditSuccess.BackColor = Drawing.Color.Red;
                //lblEditSuccess.Text = "Please enter valid event date!";
                return false;
            }
            return true;
        }
        #region "LOCAL CODE"
        public bool SaveNotes()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramSensorId = new SqlParameter("@SensorId", SqlDbType.Int);
                SqlParameter paramFieldId = new SqlParameter("@FieldId", SqlDbType.Int);
                SqlParameter paramUserId = new SqlParameter("@UserId", SqlDbType.Int);
                SqlParameter paramNotes = new SqlParameter("@Notes", SqlDbType.NVarChar);
                SqlParameter paramEventDate = new SqlParameter("@EventDate", SqlDbType.DateTime);
                SqlParameter paramCapturedBy = new SqlParameter("@CapturedBy", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                DateTime eventdate = Convert.ToDateTime(this.Request.Form["EventDate"]);


                cmd.CommandText = "[Sensors].[spAddSensorNotes]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(paramSensorId).Value = drpSensor.SelectedValue;
                cmd.Parameters.Add(paramFieldId).Value = drpSensorField.SelectedValue;
                cmd.Parameters.Add(paramUserId).Value = drpUser.SelectedValue;
                cmd.Parameters.Add(paramNotes).Value = txtNotes.Text;
                cmd.Parameters.Add(paramEventDate).Value = eventdate;
                cmd.Parameters.Add(paramCapturedBy).Value = MyUser.ID;
                cmd.ExecuteNonQuery();
                Saved = true;

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                Saved = false;
                //lblSuccess.BackColor = Drawing.Color.Red;
                lblSuccess.Text = ex.Message.ToString();
            }
            return Saved;
        }

        public bool EditNotes()
        {
            bool Saved = false;

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);
                SqlParameter paramisActive = new SqlParameter("@isActive", SqlDbType.Bit);
                SqlParameter paramSensorId = new SqlParameter("@SensorId", SqlDbType.Int);
                SqlParameter paramFieldId = new SqlParameter("@FieldId", SqlDbType.Int);
                SqlParameter paramUserId = new SqlParameter("@UserId", SqlDbType.Int);
                SqlParameter paramNotes = new SqlParameter("@Notes", SqlDbType.NVarChar);
                SqlParameter paramEventDate = new SqlParameter("@EventDate", SqlDbType.DateTime);
                SqlParameter paramCapturedBy = new SqlParameter("@CapturedBy", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                DateTime eventdate = (Convert.ToDateTime(this.Request.Form["EventDate"]));
                lblDisplay.Text = eventdate.ToString();

                cmd.CommandText = "[Sensors].[spEditSensorNotes]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(paramID).Value = gridNotes.SelectedRow.Cells[1].Text;
                cmd.Parameters.Add(paramSensorId).Value = drpSensor.SelectedValue;
                cmd.Parameters.Add(paramFieldId).Value = drpSensorField.SelectedValue;
                cmd.Parameters.Add(paramUserId).Value = drpUser.SelectedValue;
                cmd.Parameters.Add(paramNotes).Value = txtNotes.Text;
                cmd.Parameters.Add(paramEventDate).Value = lblDisplay.Text;
                cmd.Parameters.Add(paramCapturedBy).Value = MyUser.ID;
                cmd.Parameters.Add(paramisActive).Value = chkDelete.Checked;
                cmd.ExecuteNonQuery();
                Saved = true;

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                Saved = false;
                //lblEditSuccess.BackColor = Drawing.Color.Red;
                //lblEditSuccess.Text = ex.Message.ToString();
            }
            return Saved;
        }
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string temp = btnSave.Text;

            if (temp.Contains("Add"))
            {
                try
                {
                    lblSuccess.Text = "";
                    if (canSave() == true)
                    {
                        if (SaveNotes() == true)
                        {
                            successMessage.Visible = true;
                            lblSuccess.Text = "Sensor Notes successfully saved!";
                            // AddNew.Visible = False
                            drpSensor.SelectedIndex = -1;
                            drpSensorField.SelectedIndex = -1;
                            drpUser.SelectedIndex = -1;
                            txtNotes.Text = "";
                            LoadSensors(Convert.ToInt32(drpViewSensor.SelectedValue));
                        }
                        else
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while saving notes please try again later.";
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message.ToString();
                }
            }
            else if (temp.Contains("Update"))
            {
                try
                {
                    //lblEditSuccess.Text = "";
                    if (canSave() == true)
                    {
                        if (EditNotes() == true)
                        {
                            // lblEditSuccess.BackColor = Drawing.Color.White;
                            // lblEditSuccess.Text = "Sensor Notes successfully saved!";
                            //Edit.Visible = false;
                            drpSensor.SelectedIndex = -1;
                            drpSensorField.SelectedIndex = -1;
                            drpUser.SelectedIndex = -1;
                            txtNotes.Text = "";
                            //Request.Form["EventDate"] = "";
                            LoadSensors(Convert.ToInt32(drpViewSensor.SelectedValue));
                            successMessage.Visible = true;
                            lblSuccess.Text = "Succesfully updated.";
                            btnSave.Text = "Add";
                            chkDelete.Visible = false;
                            lblDelete.Visible = false;
                        }
                        else
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while saving notes.";
                          
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message;
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //lblEditSuccess.Text = "";
                if (EditcanSave() == true)
                {
                    if (EditNotes() == true)
                    {
                       // lblEditSuccess.BackColor = Drawing.Color.White;
                       // lblEditSuccess.Text = "Sensor Notes successfully saved!";
                        //Edit.Visible = false;
                        drpEditSensor.SelectedIndex = -1;
                        drpEditField.SelectedIndex = -1;
                        drpEditUsers.SelectedIndex = -1;
                        txtEditNotes.Text = "";
                        LoadSensors(Convert.ToInt32(drpViewSensor.SelectedValue));
                    }
                    else
                    {
                        //lblEditSuccess.BackColor = Drawing.Color.Red;
                       // lblEditSuccess.Text = "An error occured while saving notes please try again later.";
                    }
                }
            }
            catch (Exception ex)
            {
               // lblEditSuccess.BackColor = Drawing.Color.Red;
               // lblEditSuccess.Text = ex.Message.ToString();
            }
        }


        protected void gridDashboards_PageIndexChanged(object sender, EventArgs e)
        {
        }

        protected void gridDashboards_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridNotes.PageIndex = e.NewPageIndex;
                gridNotes.DataSource = getSensors(Convert.ToInt32(drpViewSensor.SelectedValue));
                gridNotes.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        protected void drpSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillFields(drpSensorField, drpSensor.SelectedValue);

            }
            catch (Exception ex)
            {
            }
        }

        protected void drpEditSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillFields(drpEditSensor, drpEditSensor.SelectedValue);

            }
            catch (Exception ex)
            {
            }
        }

        protected void drpViewSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadSensors(Convert.ToInt32(drpViewSensor.SelectedValue));

            }
            catch (Exception ex)
            {
            }
        }
        public void Sensors_Notes_SensorNotes()
        {
            Load += Page_Load;
        }


        protected void btnAddNewSensorNote_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClearNewSensorNote_Click(object sender, EventArgs e)
        {
            drpSensor.SelectedIndex = (0);
            drpSensor.SelectedIndex = (0);
            drpSensorField.SelectedIndex = (0);
            drpSensorField.SelectedIndex = (0);
            txtNotes.Text = "";
            Request.Form["EventDate"] = "";
            
        }

        protected void gridNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //AddNew.Visible = false;
                //Edit.Visible = true;
                //drpEditSensor.SelectedItem.Text = Convert.ToString(-1);
                //drpEditUsers.SelectedItem.Text = Convert.ToString(-1);
                //drpEditField.SelectedIndex = -1;
                //txtEditNotes.Text = "";
                //lblEditSuccess.Text = "";

                drpSensor.SelectedItem.Text = gridNotes.SelectedRow.Cells[2].Text.ToUpper();
                FillFields(drpSensorField, drpEditSensor.SelectedValue);
                drpSensorField.SelectedItem.Text = gridNotes.SelectedRow.Cells[3].Text.ToUpper();
                txtNotes.Text = gridNotes.SelectedRow.Cells[4].Text.ToUpper();
                this.InputValue = gridNotes.SelectedRow.Cells[5].Text.ToUpper();
                drpUser.SelectedItem.Text = gridNotes.SelectedRow.Cells[6].Text.ToUpper();
                btnSave.Text = "Update";
                chkDelete.Visible = true;
                lblDelete.Visible = true;
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        protected string InputValue { get; set; }
    }
}