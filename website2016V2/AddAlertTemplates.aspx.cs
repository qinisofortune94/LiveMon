using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class AddAlertTemplates : System.Web.UI.Page
    {
        private LiveMonitoring.IRemoteLib.AlertDetails AlertDetails = new LiveMonitoring.IRemoteLib.AlertDetails();
        private List<LiveMonitoring.IRemoteLib.AlertContactDef> AlertContacts = new List<LiveMonitoring.IRemoteLib.AlertContactDef>();
        private List<LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef> AlertThreasholds = new List<LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef>();
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        private int MyAlertId;
        protected void Page_Load(object sender, EventArgs e)
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                LoadTempObjects();

                if (IsPostBack == false)
                {
                    try
                    {
                        //clear temp data
                        DeleteContactLinkTemp();
                        DeleteThreshholdsTemp();
                    }
                    catch
                    {
                    }
                    PageLoadThresholds();
                    LoadContactPage();

                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.loadAlert(AlertType, chkMessageType);

                    Collection MyCollection = new Collection();
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                    object MyObject1 = null;
                    AlertCameraImages.Items.Clear();
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MyCamera.Caption;
                            MyItem.Value = MyCamera.ID.ToString();
                            MyItem.Selected = false;
                            AlertCameraImages.Items.Add(MyItem);
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MySensor.Caption;
                            MyItem.Value = MySensor.ID.ToString();
                            MyItem.Selected = false;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = GridContacts.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;
                try
                {
                    DeleteSpecificContactLinkTemp(Convert.ToInt32(ViewState["Id"] = Id));
                    LoadTempContacts();
                }
                catch (Exception ex)
                {
                }
            }

            else if (commandName == "DeleteItem")
            {
                ViewState["Id"] = Id;
            }

        }

        protected void gvSample_Commands1(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = GridThreashholds.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;
                try
                {
                    DeleteSpecificThreshholdTemp(Convert.ToInt32(ViewState["Id"] = Id));
                    LoadTempThreshholds();
                }
                catch (Exception ex)
                {
                }
            }

            else if (commandName == "DeleteItem")
            {
                ViewState["Id"] = Id;
            }

        }

        private void LoadTempObjects()
        {
            try
            {
                if ((Session["AlertDetails"] == null) == false)
                {
                    AlertDetails = (LiveMonitoring.IRemoteLib.AlertDetails)Session["AlertDetails"];
                }
                if ((Session["AlertContacts"] == null) == false)
                {
                    AlertContacts = (List<LiveMonitoring.IRemoteLib.AlertContactDef>)Session["AlertContacts"];
                }
                if ((Session["AlertDetails"] == null) == false)
                {
                    AlertThreasholds = (List<LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef>)Session["AlertThreasholds"];
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadContactPage()
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedView.aspx");
            }
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.IRemoteLib.AlertDetails MyAlert = default(LiveMonitoring.IRemoteLib.AlertDetails);

            LoadallContacts();
        }

        public void LoadallContacts()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllContacts();
            cmbContacts.Items.Clear();
            if ((MyCollectionAlerts == null) == true)
            {
                return;
            }
            bool firstRow = true;
            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyContact in MyCollectionAlerts)
            {
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = MyContact.ContactName;
                MyItem.Value = MyContact.ID.ToString();
                cmbContacts.Items.Add(MyItem);
                if (firstRow)
                {
                    MyItem.Selected = true;
                    firstRow = false;
                }
            }
        }

        public void AddContactRows(string[] RowVals)
        {
            DataRow Row = null;
            DataTable dt = new DataTable();
            if (Session["myContacttable"] == null == false)
            {
                dt = (DataTable)Session["myContacttable"];
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Cell", typeof(string));
                dt.Columns.Add("Pager", typeof(string));
                dt.Columns.Add("Other", typeof(string));
                dt.Columns.Add("Outputs", typeof(string));
                dt.Columns.Add("ResendDelay", typeof(string));
                dt.Columns.Add("ID", typeof(int));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];
            Row[5] = RowVals[5];
            Row[6] = RowVals[6];
            Row[7] = RowVals[7];
            Row[8] = RowVals[8];

            dt.Rows.Add(Row);
            Session["myContacttable"] = dt;
            GridContactBind(dt);
        }

        public void ClearContactRows()
        {
            DataTable dt = new DataTable();
            Session["myContacttable"] = dt;
            GridContactBind(dt);
        }

        public void GridContactBind(DataTable dt)
        {
            GridContacts.DataSource = dt;
            GridContacts.DataKeyNames = (new string[] { "ID" });
            GridContacts.DataBind();

            GridContactsVerify.DataSource = dt;
            GridContactsVerify.DataKeyNames = (new string[] { "ID" });
            GridContactsVerify.DataBind();
        }

        public void LoadContactGrid(int AlertID)
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsContacts(AlertID);
            ClearContactRows();
            if ((MyCollectionAlerts == null) == true)
            {
                return;
            }
            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyAlert in MyCollectionAlerts)
            {
                AddContactRows((new string[] {
                    MyAlert.ContactName,
                    MyAlert.Type.ToString(),
                    MyAlert.OutputParam.ToString(),
                    MyAlert.OutputParam1.ToString(),
                    MyAlert.OutputParam2.ToString(),
                    MyAlert.OutputParam3.ToString(),
                    MyAlert.OutputParam4.ToString() + "," + MyAlert.OutputParam5.ToString() + "," + MyAlert.OutputParam6.ToString() + "," + MyAlert.OutputParam7.ToString(),
                    MyAlert.ResendDelay.ToString(),
                    MyAlert.LinkID.ToString()
                }));
            }
        }

        public void AddRows(string[] RowVals)
        {
            DataRow Row = null;
            DataTable dt = new DataTable();

            if (Session["mytable"] == null == false)
            {
                dt = (DataTable)Session["mytable"];
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Message", typeof(string));
                dt.Columns.Add("Include Image", typeof(bool));
                dt.Columns.Add("Camera 1 ID", typeof(int));
                dt.Columns.Add("Camera 2 ID", typeof(int));
                dt.Columns.Add("Sensor 1 ID", typeof(int));
                dt.Columns.Add("Sensor 2 ID", typeof(int));
                dt.Columns.Add("Sensor 3 ID", typeof(int));
                dt.Columns.Add("Sensor 4 ID", typeof(int));
                dt.Columns.Add("Enabled", typeof(bool));
                dt.Columns.Add("SendNormal", typeof(bool));
                dt.Columns.Add("Delay1", typeof(int));
                dt.Columns.Add("Delay2", typeof(int));
                dt.Columns.Add("ID", typeof(int));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = Convert.ToBoolean(RowVals[2]);
            Row[3] = Convert.ToInt32(RowVals[3]);
            Row[4] = Convert.ToInt32(RowVals[4]);
            Row[5] = Convert.ToInt32(RowVals[5]);
            Row[6] = Convert.ToInt32(RowVals[6]);
            Row[7] = Convert.ToInt32(RowVals[7]);
            Row[8] = Convert.ToInt32(RowVals[8]);
            Row[9] = Convert.ToBoolean(RowVals[9]);
            Row[10] = Convert.ToBoolean(RowVals[10]);
            Row[11] = Convert.ToInt32(RowVals[11]);
            Row[12] = Convert.ToInt32(RowVals[12]);
            Row[13] = Convert.ToInt32(RowVals[13]);
            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mytable"] = dt;
            GridBind(dt);
        }

        public void GridBind(DataTable dt)
        {
            GridContacts.DataSource = dt;
            GridContacts.DataKeyNames = (new string[] { "ID" });
            GridContacts.DataBind();
            GridContactsVerify.DataSource = dt;
            GridContactsVerify.DataKeyNames = (new string[] { "ID" });
            GridContactsVerify.DataBind();
        }

        public void ClearRows()
        {
            DataTable dt = new DataTable();
            Session["mytable"] = dt;
            GridBind(dt);
        }

        public void LoadTempContacts()
        {
            GridContacts.DataSource = getTempContacts();
            GridContacts.DataBind();

            GridContactsVerify.DataSource = getTempContacts();
            GridContactsVerify.DataBind();

        }

        public void LoadTempThreshholds()
        {
            GridThreashholds.DataSource = getTempThreshholds();
            GridThreashholds.DataBind();

            GridThreashholdsVerify.DataSource = getTempThreshholds();
            GridThreashholdsVerify.DataBind();

        }

        private DataTable getTempContacts()
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];


            string sqlQuery = "[People].[contacts_select_specifictTemp]";
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

        private DataTable getTempThreshholds()
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];


            string sqlQuery = "alerts_threshholds_select_specificTemp";
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

        public bool SaveContactLinkTemp()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.Int);
                SqlParameter paramContactID = new SqlParameter("@ContactID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertcontactlinkTemp_add_new";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramUserID).Value = MyUser.ID;
                cmd.Parameters.Add(paramContactID).Value = cmbContacts.SelectedValue;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;

            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = false;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        public bool SaveThreshholdTemp(LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef Threshhold)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.DataAccess da = new LiveMonitoring.DataAccess();
            SqlDataReader MySQLReader = null;
            System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[21];
            MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[0].ParameterName = "SensorID";
            MySQLParam[0].Value = Threshhold.SensorID;
            MySQLParam[1] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[1].ParameterName = "DeviceID";
            MySQLParam[1].Value = Threshhold.DeviceID;
            MySQLParam[2] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[2].ParameterName = "TestType";
            MySQLParam[2].Value = Threshhold.TestType;
            MySQLParam[3] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[3].ParameterName = "CheckValue";
            MySQLParam[3].Value = Threshhold.CheckValue;
            MySQLParam[4] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[4].ParameterName = "HoldPeriod";
            MySQLParam[4].Value = Threshhold.HoldPeriod;
            MySQLParam[5] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[5].ParameterName = "AlertID";
            MySQLParam[5].Value = Threshhold.AlertID;
            MySQLParam[6] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[6].ParameterName = "Comparison";
            MySQLParam[6].Value = Threshhold.Comparison;
            MySQLParam[7] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[7].ParameterName = "Order";
            MySQLParam[7].Value = Threshhold.Order;
            MySQLParam[8] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[8].ParameterName = "Extra";
            MySQLParam[8].Value = Threshhold.Extra;
            MySQLParam[9] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[9].ParameterName = "Extra1";
            MySQLParam[9].Value = Threshhold.Extra1;
            MySQLParam[10] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[10].ParameterName = "Extra2";
            MySQLParam[10].Value = Threshhold.Extra2;
            MySQLParam[11] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[11].ParameterName = "Extra3";
            MySQLParam[11].Value = Threshhold.Extra3;
            MySQLParam[12] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[12].ParameterName = "Field";
            MySQLParam[12].Value = Threshhold.Field;
            MySQLParam[13] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[13].ParameterName = "Name";
            MySQLParam[13].Value = Threshhold.Name;
            MySQLParam[14] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[14].ParameterName = "TabularRowCnt";
            MySQLParam[14].Value = Threshhold.TabularCnt;
            MySQLParam[15] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[15].ParameterName = "Field1";
            MySQLParam[15].Value = Threshhold.Field1;
            MySQLParam[16] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[16].ParameterName = "Field2";
            MySQLParam[16].Value = Threshhold.Field2;
            MySQLParam[17] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[17].ParameterName = "GroupID";
            MySQLParam[17].Value = Threshhold.GroupID;
            MySQLParam[18] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[18].ParameterName = "LocatonID";
            MySQLParam[18].Value = Threshhold.LocatonID;
            MySQLParam[19] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[19].ParameterName = "IsTemplate";
            MySQLParam[19].Value = Threshhold.IsTemplate;

            MySQLParam[20] = new System.Data.SqlClient.SqlParameter();
            MySQLParam[20].ParameterName = "UserID";
            MySQLParam[20].Value = MyUser.ID;

            MySQLReader = da.ExecCmdQueryParams("threshholds_add_newTemp", MySQLParam);
            int MyNewID = 0;
            if ((MySQLReader == null) == false)
            {
                while (MySQLReader.Read())
                {
                    //if details then
                    if (Information.IsDBNull(MySQLReader.GetValue(0)) == false)
                    {
                        MyNewID = Convert.ToInt32(MySQLReader.GetValue(0));
                        if (MyNewID > 0)
                        {
                            Saved = true;
                        }
                    }
                }
            }
            else
            {
                //not inserted
                MyNewID = 0;
                Saved = false;
            }
            MySQLReader = null;
            return Saved;
        }

        public bool DeleteContactLinkTemp()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertcontactlinkTemp_deleteAll";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramUserID).Value = MyUser.ID;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        public bool DeleteSpecificContactLink(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertcontactlink_deleteSpecific";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        public bool DeleteThreshholdsTemp()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alert_threshholdstemp_deleteAll";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramUserID).Value = MyUser.ID;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        public bool DeleteSpecificThreshhold(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alert_threshholdstemplate_deleteSpecific";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        protected void cmdLinkContacts_Click(object sender, EventArgs e)
        {
            //And Me.txtID.Value <> "" Then
            if (cmbContacts.SelectedIndex != -1)
            {
                try
                {
                    if ((SaveContactLinkTemp() == true))
                    {
                        LoadTempContacts();
                        lblValidatesContacts.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    //if err
                    errorMessage.Visible = true;
                    lblError.Text = "Error adding link:" + ex.Message;
                }
            }
            else
            {
                lblErr.Visible = true;
                if (cmbContacts.SelectedIndex != -1)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Error Please select contact !";
                }
                else
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Error Please select alert !";
                }
            }
        }

        private void PageLoadThresholds()
        {
            int reqAlertID = 0;
            reqAlertID = Convert.ToInt32(Request.QueryString["AlertID"]);

            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.testType(TestType);
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));

            object MyObject1 = null;
            cmbDeviceID.Items.Clear();
            cmbSensorID.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyCamera.Caption;
                    MyItem.Value = MyCamera.ID.ToString();
                    MyItem.Selected = false;
                    cmbDeviceID.Items.Add(MyItem);
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevices = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyIPDevices.Caption;
                    MyItem.Value = MyIPDevices.ID.ToString();
                    MyItem.Selected = false;
                    cmbDeviceID.Items.Add(MyItem);
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevices = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyOtherDevices.Caption;
                    MyItem.Value = MyOtherDevices.ID.ToString();
                    MyItem.Selected = false;
                    cmbDeviceID.Items.Add(MyItem);
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPManager = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MySNMPManager.Caption;
                    MyItem.Value = MySNMPManager.ID.ToString();
                    MyItem.Selected = false;
                    cmbDeviceID.Items.Add(MyItem);
                }

                LoadSensorTypes();
            }
        }

        public void LoadGrid()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsThreashholds(Convert.ToInt32(this.lblMyID.Text));
            //LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = default(LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef);
            ClearThreasholdRows();

            foreach (LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert in MyCollectionAlerts)
            {
                try
                {
                    AddThreasholdRows((new string[] {
                        MyAlert.Order.ToString(),
                        MyAlert.Name,
                        MyAlert.SensorID.ToString(),
                        MyAlert.DeviceID.ToString(),
                        MyAlert.Field.ToString(),
                        MyAlert.TestType.ToString(),
                        MyAlert.CheckValue.ToString(),
                        MyAlert.HoldPeriod.ToString(),
                        MyAlert.Comparison.ToString(),
                        MyAlert.Extra,
                        MyAlert.Extra1,
                        MyAlert.Extra2.ToString(),
                        MyAlert.Extra3.ToString(),
                        MyAlert.ID.ToString(),
                        MyAlert.TabularCnt.ToString(),
                        MyAlert.Field1.ToString(),
                        MyAlert.Field2.ToString(),
                        MyAlert.GroupID.ToString(),
                        MyAlert.LocatonID.ToString(),
                        MyAlert.IsTemplate.ToString()
                    }));

                }
                catch (Exception ex)
                {
                }
            }
        }

        public void AddThreasholdRows(string[] RowVals)
        {
            DataRow Row = null;
            DataTable dt = new DataTable();

            if (Session["myThreshtable"] == null == false)
            {
                dt = (DataTable)Session["myThreshtable"];
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Order", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("SensorID", typeof(int));
                dt.Columns.Add("DeviceID", typeof(int));
                dt.Columns.Add("Field", typeof(int));
                dt.Columns.Add("TestType", typeof(int));
                dt.Columns.Add("CheckValue", typeof(double));
                dt.Columns.Add("HoldPeriod", typeof(int));
                dt.Columns.Add("Comparison", typeof(int));
                dt.Columns.Add("Extra Data", typeof(string));
                dt.Columns.Add("Extra Data1", typeof(string));
                dt.Columns.Add("Extra Data2", typeof(string));
                dt.Columns.Add("Extra Data3", typeof(string));
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("TabularCnt", typeof(string));
                dt.Columns.Add("Field1", typeof(string));
                dt.Columns.Add("Field2", typeof(string));
                dt.Columns.Add("GroupID", typeof(string));
                dt.Columns.Add("LocatonID", typeof(string));
                dt.Columns.Add("IsTemplate", typeof(string));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];

            try
            {
                Row[2] = RowVals[2];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[3] = RowVals[3];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[4] = RowVals[4];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[5] = RowVals[5];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[6] = RowVals[6];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[7] = RowVals[7];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[8] = RowVals[8];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[9] = RowVals[9];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[10] = RowVals[10];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[11] = RowVals[11];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[12] = RowVals[12];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[13] = RowVals[13];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[14] = RowVals[14];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[15] = RowVals[15];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[16] = RowVals[16];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[17] = RowVals[17];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[18] = RowVals[18];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[19] = RowVals[19];

            }
            catch (Exception ex)
            {
            }
            dt.Rows.Add(Row);
            Session["myThreshtable"] = dt;
            GridThreasholdBind(dt);

        }

        public void ClearThreasholdRows()
        {
            DataTable dt = new DataTable();
            Session["myThreshtable"] = dt;
            GridThreasholdBind(dt);
        }

        public void GridThreasholdBind(DataTable dt)
        {
            GridThreashholds.DataSource = dt;
            GridThreashholds.DataKeyNames = (new string[] { "ID" });
            GridThreashholds.DataBind();

            GridThreashholdsVerify.DataSource = dt;
            GridThreashholdsVerify.DataKeyNames = (new string[] { "ID" });
            GridThreashholdsVerify.DataBind();
        }

        protected void cmbSensorID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            cmbField.Items.Clear();
            cmbFieldComp.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;

                    if (MySensor.Type == (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(cmbSensorID.SelectedValue))
                    {
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                        {
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MyField.FieldName;
                            MyItem.Value = MyField.FieldNumber.ToString();
                            MyItem.Selected = false;
                            cmbField.Items.Add(MyItem);
                            cmbFieldComp.Items.Add(MyItem);
                        }
                    }
                }
            }
        }

        public void LoadSensorTypes()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.LoadfieldNames(cmbSensorID);
        }

        protected void TestType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (TestType.SelectedIndex)
            {

                case (int)LiveMonitoring.IRemoteLib.TestType.ConsumptionAlert - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.OutOfAvergeBand - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Averge band Percentage D=10";
                    int ten = 10;
                    TxtExtra3.Text = ten.ToString();
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.AboveAveragePercent - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.BellowAveragePercent - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.ProjectedHighConsumption - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Projected units (val)";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.ProjectedLowConsumption - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Projected units (val)";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldEquals - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldGreater - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldSmaller - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                default:
                    TxtExtra2.Visible = true;
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Must Occure (Hours)";
                    lblExtra3.Text = "Extra3 Val";
                    int zero = 0;
                    TxtExtra3.Text = zero.ToString();
                    break;
            }
        }

        protected void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSensorFieldsValue();
        }

        public void LoadSensorFieldsValue()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            curVals.InnerText = "Current Values:";
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (MySensor.ID == Convert.ToInt32(cmbSensorID.SelectedValue))
                    {
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                        {
                            if (MyField.FieldNumber == Convert.ToInt32(cmbField.SelectedValue))
                            {
                                curVals.InnerText += " Val:" + MyField.LastValue.ToString() + ": Other:" + MyField.LastOtherValue + ": Tab:" + MyField.TabularRowNo.ToString() + " ";
                            }
                        }
                    }
                }
            }
        }

        protected void cmdSend_Click_SaveThreashold(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            lblThreadSuccess.Text = "";
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please Supply a Name!";
                return;
            }
            if (string.IsNullOrEmpty(this.cmbSensorID.SelectedValue) & string.IsNullOrEmpty(this.cmbDeviceID.SelectedValue))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please Select Sensor type!";
                return;
            }
            if (string.IsNullOrEmpty(this.cmbField.SelectedValue) & string.IsNullOrEmpty(this.cmbField.SelectedValue))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please Select Field!";
                return;
            }
            if (string.IsNullOrEmpty(this.txtCheckValue.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter a Value!";
                return;
            }
            if (string.IsNullOrEmpty(this.txtOrder.Text))
            {
                this.txtOrder.Text = "0";
            }
            if (Convert.ToInt32(this.TestType.SelectedValue) == 11 | Convert.ToInt32(this.TestType.SelectedValue) == 12 | Convert.ToInt32(this.TestType.SelectedValue) == 13)
            {
                if (string.IsNullOrEmpty(this.TxtExtra.Text))
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Please enter a String Value to check!";
                    return;
                }
            }
            LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef();
            MyAlert.Name = this.txtName.Text;
            MyAlert.SensorID = Convert.ToInt32(this.cmbSensorID.SelectedValue);
            MyAlert.DeviceID = Convert.ToInt32(this.cmbDeviceID.SelectedValue);
            MyAlert.TestType = (LiveMonitoring.IRemoteLib.TestType)Convert.ToInt32(this.TestType.SelectedValue);
            MyAlert.Field = Convert.ToInt32(this.cmbField.SelectedValue);
            MyAlert.CheckValue = Convert.ToInt32(this.txtCheckValue.Text);
            MyAlert.HoldPeriod = Convert.ToInt32(this.txtHoldPeriod.Text);
            //???
            MyAlert.AlertID = 0;
            MyAlert.Comparison = this.Comparison.Items[0].Selected;
            MyAlert.Order = Convert.ToInt32(this.txtOrder.Text);
            MyAlert.Extra = this.TxtExtra.Text;
            MyAlert.Extra1 = this.TxtExtra1.Text;
            if (!string.IsNullOrEmpty(this.TxtExtra2.Text))
            {
                MyAlert.Extra2 = Convert.ToDouble(this.TxtExtra2.Text);
                MyAlert.Extra3 = Convert.ToDouble(this.TxtExtra3.Text);
            }
            MyAlert.TabularCnt = Convert.ToInt32(this.txtTabularCnt.Text);
            if (cmbFieldComp.Visible == true)
            {
                MyAlert.Field1 = Convert.ToInt32(cmbFieldComp.SelectedValue);
            }
            else
            {
                MyAlert.Field1 = 0;
            }
            MyAlert.IsTemplate = this.chkSensAlertTemplate.Checked;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
         
            if (SaveThreshholdTemp(MyAlert) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error!";
                try
                {
                    MyRem.WriteLog("Add ThreshHold Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    successMessage.Visible = true;
                    lblSuccess.Text = "ThreshHold " + txtName.Text + " was successfully saved!";
                    MyRem.WriteLog("Add ThreshHold Succeeded", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                }
                catch (Exception ex)
                {
                }
                lblErr.Visible = false;
                this.txtName.Text = "";
                //Me.TxtExtra.Text = ""
                this.TxtExtra1.Text = "";
                this.TxtExtra2.Text = "";
                this.TxtExtra3.Text = "";
                this.txtOrder.Text = "0";

                LoadTempThreshholds();

                this.txtCheckValue.Text = "";
                this.txtName.Text = "";
            }
        }

        protected void Submit_Click_SaveAlert(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            LiveMonitoring.IRemoteLib.AlertDetails MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails();
            MyAlert.AlertMessage = this.AlertMessage.Text;
            int i = 0;
            int result = 0;
            for (i = 0; i < this.AlertType.Items.Count; i++)
            {
                if (this.AlertType.Items[i].Selected == true)
                {
                    result += Convert.ToInt32(this.AlertType.Items[i].Value);
                }
            }
            MyAlert.AlertType = (LiveMonitoring.IRemoteLib.AlertDetails.AlertsType)result;

            if (MyAlert.AlertType == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select Alert Type!";
                return;
            }
            if (string.IsNullOrEmpty(MyAlert.AlertMessage))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please Enter a Message!";
                return;
            }
            int aCnt = 0;
            for (i = 0; i <= this.AlertCameraImages.Items.Count - 1; i++)
            {
                if (this.AlertCameraImages.Items[i].Selected)
                {
                    if (aCnt == 0)
                    {
                        MyAlert.CameraID1 = Convert.ToInt32(this.AlertCameraImages.Items[i].Value);
                    }
                    else
                    {
                        MyAlert.CameraID2 = Convert.ToInt32(this.AlertCameraImages.Items[i].Value);

                    }
                    aCnt += 1;
                    if (aCnt > 1)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            aCnt = 0;
            MyAlert.SensorValueID1 = 0;
            MyAlert.SensorValueID3 = 0;
            MyAlert.SensorValueID1 = 0;
            MyAlert.SensorValueID3 = 0;
            MyAlert.SensorValueID2 = 0;
            MyAlert.SensorValueID4 = 0;
            MyAlert.SensorValueID2 = 0;
            MyAlert.SensorValueID4 = 0;
            MyAlert.IncludeImage = this.AlertIncludeImage.Items[0].Selected;
            MyAlert.Enabled = this.AlertEnabled.Items[0].Selected;
            MyAlert.SendNormal = this.AlertSendRTN.Items[0].Selected;
            MyAlert.Camera1Delay = Convert.ToDouble(this.txtDelay1.Text);
            MyAlert.Camera2Delay = Convert.ToDouble(this.txtDelay2.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            int MyALertID = MyRem.LiveMonServer.AddNewAlert(MyAlert);

            if (MyALertID < 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error!";
                try
                {
                    MyRem.WriteLog("Add Alert Failed", "User:" + MyUser.ID.ToString() + "|" + this.AlertMessage.Text);

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                try
                {
                    MyRem.WriteLog("Add Alert Succeeded", "User:" + MyUser.ID.ToString() + "|" + MyALertID.ToString());

                }
                catch (Exception ex)
                {
                }
                this.AlertMessage.Text = "";
                Response.Redirect("LinkAlertcontact.aspx?AlertID=" + MyALertID.ToString());
            }
        }

        protected void btnDevice_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Device>";
        }

        protected void btnAlertMins_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<AlertMins>";

        }

        protected void btnAlertStart_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<AlertStart>";

        }

        protected void btnCrLf_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<CRLF>";

        }

        protected void btnFields_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Fields>";

        }

        protected void btnName_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Name>";

        }

        protected void btnRTNSE_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<RTN Start>Your RTN Message HERE<RTN End>";

        }

        protected void btnValues_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Values>";
        }

        protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
        {
            Wizard1.HeaderText = "You are currently on " + Wizard1.ActiveStep.Title;
            switch (Wizard1.ActiveStep.ID)
            {
                case "Wiz0":
                    //Create message
                    if (txtTemplateName.Text.Trim().Length == 0)
                    {
                        Wizard1.ActiveStepIndex = 0;
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter template name before you can proceed";
                    }
                    else
                    {
                        lblValitesMessage.Text = "";
                    }
                    if (AlertMessage.Text.Trim().Length == 0)
                    {
                        Wizard1.ActiveStepIndex = 0;
                        AlertMessage.Focus();
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter alert message before you can proceed";
                    }
                    else
                    {
                        lblValitesMessage.Text = "";
                    }
                    break;
                case "Wiz1":
                    //options
                    if (AlertType.SelectedIndex != -1)
                    {
                        lblAlert.Text = "";
                    }
                    else
                    {
                        Wizard1.ActiveStepIndex = 1;
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select Alert Type";
                    }
                    break;
                case "Wiz2":
                    //contacts
                    break;

                case "Wiz3":
                    //threasholds
                    if (GridContacts.Rows.Count == 0)
                    {
                        Wizard1.ActiveStepIndex = 3;
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please Link Contact before you can proceed!";
                    }
                    else
                    {
                        lblValidatesContacts.Text = "";
                    }
                    break;
                case "Wiz4":
                    //Verify
                    if (GridThreashholds.Rows.Count == 0)
                    {
                        Wizard1.ActiveStepIndex = 4;
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please add atleast one threshhold before you can proceed!";
                    }
                    else
                    {
                        lblValidatesThreshholds.Text = "";
                    }
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Wiz.ID = "Wiz";
            //Wiz0.ID = "Wiz0";
            //Wiz1.ID = "Wiz1";
            Wiz2.ID = "Wiz2";
            Wiz3.ID = "Wiz3";
            Wiz4.ID = "Wiz4";

            Wiz.Title = "Create message";
            Wiz.StepType = WizardStepType.Start;
            //Wiz0.Title = "Choose Message Type";
            //Wiz0.StepType = WizardStepType.Auto;
            //Wiz1.Title = "Configure Options";
            // Wiz1.StepType = WizardStepType.Auto;
            Wiz2.Title = "Choose Contacts";
            Wiz2.StepType = WizardStepType.Auto;
            Wiz3.Title = "Add Threshholds";
            Wiz3.StepType = WizardStepType.Auto;

            Wiz4.Title = "Verify Details";
            Wiz4.StepType = WizardStepType.Finish;

            DataList dl = (DataList)Wizard1.FindControl("SideBarContainer").FindControl("SideBarList");
            dl.ItemDataBound += w_ItemDataBound;
        }

        private void w_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lb = e.Item.FindControl("SideBarButton") as LinkButton;
            if (lb != null)
            {
                lb.Enabled = true;
            }
        }

        protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
                Wizard1.BorderWidth = Unit.Pixel(Convert.ToInt32(Wizard1.BorderWidth.Value + 1));
                int I = Wizard1.ActiveStepIndex;
                //validate first step
                if (I == 0)
                {
                    if (AlertMessage.Text.Trim().Length == 0)
                    {
                        Wizard1.MoveTo(Wizard1.ActiveStep);
                        AlertMessage.Focus();
                        lblValitesMessage.Text = "Please enter alert message before you can proceed";
                        return;
                    }
                }

                if (I == 2)
                {
                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    if (MyIPMonPageSecure > MyUser.UserLevel)
                    {
                        Response.Redirect("NotAuthorisedEdit.aspx");
                    }
                    //int j = 0;
                    //for (j = 0; j <= this.AlertType.Items.Count - 1; j++)
                    //{
                    //    if (this.AlertType.Items[j].Selected)
                    //    {
                    //        //display verify summary
                    //        chkMessageType.Items[j].Selected = Convert.ToBoolean(AlertType.Items[j].Value);
                    //    }
                    //}
                    ////verify summary starts here
                    //for (int f = 0; f <= this.AlertIncludeImage.Items.Count - 1; f++)
                    //{
                    //    if (this.AlertIncludeImage.Items[f].Selected)
                    //    {
                    //        this.AlertIncludeImageVerify.Items[f].Selected = true;
                    //    }
                    //    else
                    //    {
                    //        this.AlertIncludeImageVerify.Items[f].Selected = false;
                    //    }
                    //}
                    //txtDelay1Verify.Text = txtDelay1.Text;
                    //txtDelay2Verify.Text = txtDelay2.Text;
                    //lblSensor1.Text = "Template";
                    //lblSensor2.Text = "Template";
                    //lblField1.Text = "Template";
                    //lblField2.Text = "Template";
                    //for (int f = 0; f <= this.AlertEnabled.Items.Count - 1; f++)
                    //{
                    //    if (this.AlertEnabled.Items[f].Selected)
                    //    {
                    //        this.AlertEnabledVerify.Items[f].Selected = true;
                    //    }
                    //    else
                    //    {
                    //        this.AlertEnabledVerify.Items[f].Selected = false;
                    //    }
                    //}
                    //for (int f = 0; f <= this.AlertSendRTN.Items.Count - 1; f++)
                    //{
                    //    if (this.AlertSendRTN.Items[f].Selected)
                    //    {
                    //        this.AlertSendRTNVerify.Items[f].Selected = true;
                    //    }
                    //    else
                    //    {
                    //        this.AlertSendRTNVerify.Items[f].Selected = false;
                    //    }
                    //}
                }

                if (I == 3)
                {
                    if (GridContacts.Rows.Count == 0)
                    {
                        lblValidatesContacts.Text = "Please Link Contact before you can proceed!";
                        Wizard1.ActiveStepIndex = 3;
                        return;
                    }
                }
                if (I == 4)
                {
                    lblMessage.Text = AlertMessage.Text;

                    txtDelay1Verify.Text = txtDelay1.Text;
                    txtDelay2Verify.Text = txtDelay2.Text;
                    lblSensor1.Text = "Template";
                    lblSensor2.Text = "Template";
                    lblField1.Text = "Template";
                    lblField2.Text = "Template";
                    for (int f = 0; f <= this.AlertEnabled.Items.Count - 1; f++)
                    {
                        if (this.AlertEnabled.Items[f].Selected)
                        {
                            this.AlertEnabledVerify.Items[f].Selected = true;
                        }
                        else
                        {
                            this.AlertEnabledVerify.Items[f].Selected = false;
                        }
                    }
                    for (int f = 0; f <= this.AlertSendRTN.Items.Count - 1; f++)
                    {
                        if (this.AlertSendRTN.Items[f].Selected)
                        {
                            this.AlertSendRTNVerify.Items[f].Selected = true;
                        }
                        else
                        {
                            this.AlertSendRTNVerify.Items[f].Selected = false;
                        }
                    }
                    int j = 0;
                    for (j = 0; j <= this.AlertType.Items.Count - 1; j++)
                    {
                        if (this.AlertType.Items[j].Selected)
                        {
                            //display verify summary
                            chkMessageType.Items[j].Selected = Convert.ToBoolean(AlertType.Items[j].Value);
                        }
                    }
                    //verify summary starts here
                    for (int f = 0; f <= this.AlertIncludeImage.Items.Count - 1; f++)
                    {
                        if (this.AlertIncludeImage.Items[f].Selected)
                        {
                            this.AlertIncludeImageVerify.Items[f].Selected = true;
                        }
                        else
                        {
                            this.AlertIncludeImageVerify.Items[f].Selected = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
                Wizard1.BorderWidth = Unit.Pixel(Convert.ToInt32(Wizard1.BorderWidth.Value - 1));
            }
            catch
            {
            }
        }

        protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {

            try
            {
                //create new alert
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedEdit.aspx");
                }
                LiveMonitoring.IRemoteLib.AlertTemplateDetails MyAlert = new LiveMonitoring.IRemoteLib.AlertTemplateDetails();
                MyAlert.AlertMessage = this.AlertMessage.Text;
                // j = 0;
                int result = 0;
                for (int i = 0; i < this.AlertType.Items.Count; i++)
                {
                    if (this.AlertType.Items[i].Selected == true)
                    {
                        result += Convert.ToInt32(this.AlertType.Items[i].Value);
                    }
                }
                MyAlert.AlertType = (LiveMonitoring.IRemoteLib.AlertTemplateDetails.AlertsTypeTemplate)result;

                if (MyAlert.AlertType == 0)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Message Type:Please select Alert Type!";
                    return;
                }
                if (string.IsNullOrEmpty(MyAlert.AlertMessage))
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Message:Please Enter a Message!";
                    return;
                }
                int aCnt = 0;
                for (int j = 0; j <= this.AlertCameraImages.Items.Count - 1; j++)
                {
                    if (this.AlertCameraImages.Items[j].Selected)
                    {
                        if (aCnt == 0)
                        {
                            MyAlert.CameraID1 = Convert.ToInt32(this.AlertCameraImages.Items[j].Value);
                        }
                        else
                        {
                            MyAlert.CameraID2 = Convert.ToInt32(this.AlertCameraImages.Items[j].Value);

                        }
                        aCnt += 1;
                        if (aCnt > 1)
                        {
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
                aCnt = 0;
                MyAlert.SensorValueID1 = 0;
                MyAlert.SensorValueID3 = 0;
                MyAlert.SensorValueID1 = 0;
                MyAlert.SensorValueID3 = 0;
                MyAlert.SensorValueID2 = 0;
                MyAlert.SensorValueID4 = 0;
                MyAlert.SensorValueID2 = 0;
                MyAlert.SensorValueID4 = 0;
                // End If
                int SiteId = 1;
                try
                {
                    if ((Convert.ToInt32(Session["SelectedSite"]) > 0))
                    {
                        SiteId = (int)Session["SelectedSite"];
                    }
                }
                catch (Exception ex)
                {
                }
                MyAlert.IncludeImage = this.AlertIncludeImage.Items[0].Selected;
                MyAlert.Enabled = this.AlertEnabled.Items[0].Selected;
                MyAlert.SendNormal = this.AlertSendRTN.Items[0].Selected;
                MyAlert.Camera1Delay = Convert.ToDouble(this.txtDelay1.Text);
                MyAlert.Camera2Delay = Convert.ToDouble(this.txtDelay2.Text);
                MyAlert.AlertTemplateName = txtTemplateName.Text.ToUpper();
                MyAlert.SiteId = SiteId;
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyAlertId = MyRem.LiveMonServer.AddNewAlertTemplate(MyAlert);

                lblMyID.Text = MyAlertId.ToString();

                if (MyAlertId < 0)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Message, Message Type & Options:Not saved Error!";
                    try
                    {
                        MyRem.WriteLog("Add Alert Template Failed", "User:" + MyUser.ID.ToString() + "|" + this.AlertMessage.Text);

                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    //Link Contacts
                    try
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Message, Message Type & Options: Successfully Saved!";
                        for (int j = 0; j <= GridContacts.Rows.Count - 1; j++)
                        {
                            MyRem.LiveMonServer.AddNewAlertTemplateContactLink(Convert.ToInt32(lblMyID.Text), Convert.ToInt32(GridContacts.Rows[j].Cells[10].Text));
                        }
                        lblContactLinkSuccess.Text = "Contacts: Successfully saved!";
                        DeleteContactLinkTemp();
                        LoadTempContacts();
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Contacts: Error occured: " + ex.Message;
                    }

                    //Link Threshholds
                    try
                    {
                        lblThreadSuccess.Text = "";
                        for (int j = 0; j <= GridThreashholds.Rows.Count - 1; j++)
                        {
                            LiveMonitoring.IRemoteLib.AlertTemplateDetails.AlertThreshholdsTemplateDef MyAlerts = new LiveMonitoring.IRemoteLib.AlertTemplateDetails.AlertThreshholdsTemplateDef();
                            MyAlerts.Name = GridThreashholds.Rows[j].Cells[2].Text;
                            MyAlerts.SensorID = Convert.ToInt32(GridThreashholds.Rows[j].Cells[3].Text);
                            MyAlerts.DeviceID = Convert.ToInt32(GridThreashholds.Rows[j].Cells[4].Text);
                            MyAlerts.TestType = (LiveMonitoring.IRemoteLib.TestType)Convert.ToInt32(GridThreashholds.Rows[j].Cells[6].Text);
                            MyAlerts.Field = Convert.ToInt32(GridThreashholds.Rows[j].Cells[5].Text);
                            MyAlerts.CheckValue = Convert.ToDouble(GridThreashholds.Rows[j].Cells[7].Text);
                            MyAlerts.HoldPeriod = Convert.ToInt32(GridThreashholds.Rows[j].Cells[8].Text);
                            //???
                            MyAlerts.AlertID = Convert.ToInt32(lblMyID.Text);
                            //System.Web.UI.WebControls.CheckBox cellComparison = (CheckBox)GridThreashholds.Rows(j).Cells(10).Controls(0);

                            MyAlerts.Comparison = bool.Parse(GridThreashholds.Rows[j].Cells[10].Text);
                            MyAlerts.Extra = GridThreashholds.Rows[j].Cells[12].Text;
                            MyAlerts.Extra1 = GridThreashholds.Rows[j].Cells[13].Text;

                            MyAlerts.IsTemplate = bool.Parse(GridThreashholds.Rows[j].Cells[21].Text);


                            if (MyRem.LiveMonServer.AddNewAlertThreshholdTemplate(MyAlerts) == false)
                            {
                                errorMessage.Visible = true;
                                lblError.Text = "Not saved Error!";
                                try
                                {
                                    MyRem.WriteLog("Add ThreshHold Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                                }
                                catch (Exception ex)
                                {
                                }

                            }
                            else
                            {
                                successMessage.Visible = true;
                                lblSuccess.Text = "Threshhold: was successfully saved!";
                                lblErr.Visible = false;
                                this.txtName.Text = "";
                                //Me.TxtExtra.Text = ""
                                this.TxtExtra1.Text = "";
                                this.TxtExtra2.Text = "";
                                this.TxtExtra3.Text = "";
                                this.txtOrder.Text = "0";

                                this.txtCheckValue.Text = "";
                                this.txtName.Text = "";
                                Wizard1.Visible = false;
                                Response.Redirect("AddAlertTemplates.aspx");
                            }
                        }

                        
                        try
                        {
                            MyRem.WriteLog("Add ThreshHold Succeeded", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                        }
                        catch
                        {
                        }
                        DeleteThreshholdsTemp();
                        LoadTempThreshholds();
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Threshhold: error occured while saving: " + ex.Message;
                    }

                    try
                    {
                        MyRem.WriteLog("Add Alert Succeeded", "User:" + MyUser.ID.ToString() + "|" + MyAlertId.ToString());
                        
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        public bool DeleteSpecificThreshholdTemp(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alert_threshholdstemp_deleteSpecific";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        public bool DeleteSpecificContactLinkTemp(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertcontactlinkTemp_deleteSpecific";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = "Error adding link:" + ex.Message;
                return Saved;
            }

            return Saved;
        }

        protected void GridContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DeleteSpecificContactLinkTemp(Convert.ToInt32(GridContacts.SelectedRow.Cells[9].Text));
                LoadTempContacts();
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridThreashholds_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DeleteSpecificThreshholdTemp(Convert.ToInt32(GridThreashholds.SelectedRow.Cells[1].Text));
                LoadTempThreshholds();
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridThreashholds_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridThreashholds.PageIndex = e.NewPageIndex;
                GridThreashholds.DataSource = getTempThreshholds();
                GridThreashholds.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        protected void GridContacts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridContacts.PageIndex = e.NewPageIndex;
                GridContacts.DataSource = getTempContacts();
                GridContacts.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        public void AlertTemplates()
        {
            Load += Page_Load;
        }
    }
}