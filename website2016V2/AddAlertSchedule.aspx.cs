using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;

namespace website2016V2
{
    public partial class AddAlertSchedule : System.Web.UI.Page
    {


        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
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
               // LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    // MyRem.LiveMonServer.GetSpecificAlerts()
                    LoadAlertsGrid(MyRem);
                }

            }
            else
            {
                Response.Redirect("Index.aspx");
            }
           


        }

        protected void gvSample_Commands2(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = AlertsSchedgrid.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "DeleteItem")
            {
                ViewState["Idd"] = Id;

                int intScheduleId = 0;
                SqlParameter[] myParams = new SqlParameter[1];

                try
                {
                    intScheduleId = Convert.ToInt32(ViewState["Idd"]);

                    //@SITEUSERID
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@ID";
                    myParams[0].Value = intScheduleId;

                    MyDataAccess.ExecCmdQueryParams("sp_deleteSchedule", myParams);

                    LoadScheduleGrid(Convert.ToInt32(Session["myAlertScheduleID"]));
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
                //Refresh Grid
                //LoadPeople();

                //LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

                //if (MyRem.LiveMonServer.DeleteAlertSchedule(Convert.ToInt32(ViewState["Idd"])) == false)
                //{
                //    errorMessage.Visible = true;
                //    lblError.Text = "Alert schedule not deleted.";
                //    Alertsgrid.Focus();
                //}
                //else
                //{
                //    errorMessage.Visible = true;
                //    lblError.Text = "Alert schedule deleted.";
                //    //Me.ContactID.Text = ""
                //    // errLbl.Visible = False
                //    // Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value
                //    //LoadGridRow(CInt(Session["myAlertScheduleID"]))
                //    LoadScheduleGrid(Convert.ToInt32(Session["myAlertScheduleID"]));
                //}
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = Alertsgrid.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                //string GroupID = gridSensorGroups.Rows[myRow.RowIndex].Cells[2].Text;
                //string Group = gridSensorGroups.Rows[myRow.RowIndex].Cells[3].Text;
                //string Description = gridSensorGroups.Rows[myRow.RowIndex].Cells[4].Text;



                //ViewState["Id"] = Id;

                //lblID.Visible = true;
                //lblID.Text = GroupID;
                //txtGroup.Text = Group;
                //txtDescription.Text = Description;


                //lblAdd.Text = "Update";
                //btnSave.Text = "Update";

            }

            else if (commandName == "SelectItem")
            {

                ContactID.Text = Id;

                Session["myAlertScheduleID"] = Alertsgrid.DataKeys[myRow.RowIndex].Value.ToString();
                LoadGridRow(Convert.ToInt32(Alertsgrid.DataKeys[myRow.RowIndex].Value));
                LoadScheduleGrid(Convert.ToInt32(Alertsgrid.DataKeys[myRow.RowIndex].Value));

                //Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value;
                //LoadGridRow(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));
                //LoadScheduleGrid(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));

                //SampleLogic business = new SampleLogic();

                //int RecordId = Convert.ToInt16(Id);
                //business.Delete(RecordId);

                ////Refresh Grid
                //LoadData();
            }
            

        }
        private void LoadGridRow(int RowID)
        {
            ContactID.Text = RowID.ToString();

        }
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            //LoadPeople();
            if (Alertsgrid.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                Alertsgrid.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                Alertsgrid.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                Alertsgrid.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        

        private void LoadAlertsGrid(LiveMonitoring.GlobalRemoteVars MyRem)
        {
            Collection MyCollectionAlerts = new Collection();
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllAlerts();

           // LiveMonitoring.IRemoteLib.AlertDetails MyAlert = default(LiveMonitoring.IRemoteLib.AlertDetails);
            // Alertsgrid.Clear()
            ClearRows();

            foreach (LiveMonitoring.IRemoteLib.AlertDetails MyAlert in MyCollectionAlerts)
            {

                try
                {
                    AddRows((new string[] {
                    MyAlert.AlertType.ToString(),
                    MyAlert.AlertMessage,
                    MyAlert.IncludeImage.ToString(),
                    MyAlert.CameraID1.ToString(),
                    MyAlert.CameraID2.ToString(),
                    MyAlert.SensorValueID1.ToString(),
                    MyAlert.SensorValueID2.ToString(),
                    MyAlert.SensorValueID3.ToString(),
                    MyAlert.SensorValueID4.ToString(),
                    MyAlert.Enabled.ToString(),
                    MyAlert.SendNormal.ToString(),
                    MyAlert.Camera1Delay.ToString(),
                    MyAlert.Camera2Delay.ToString(),
                    MyAlert.AlertId.ToString()
                }));


                }
                catch (Exception ex)
                {
                }

            }
        }
        public void AddRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytable"], DataTable)
            //ListFiles()

            if (Session["mytable"] == null == false)
            {
                dt = (DataTable)Session["mytable"];
                //For Each row1 As DataRow In dt.Rows
                // dt.ImportRow(row1)
                //Next

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

        public void ClearRows()
        {
            DataTable dt = new DataTable();
            Session["mytable"] = dt;
            GridBind(dt);
        }

        public void GridBind(DataTable dt)
        {
            Alertsgrid.DataSource = dt;
            Alertsgrid.DataKeyNames = (new string[] { "ID" });
            Alertsgrid.DataBind();
        }
        protected void Alertsgrid_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Alertsgrid.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }
        protected void Alertsgrid_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value;
            LoadGridRow(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));
            LoadScheduleGrid(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));


        }
        private void LoadScheduleGrid(int MyAlertID)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            Collection MyCollectionAlertsSched = new Collection();
            MyCollectionAlertsSched = MyRem.LiveMonServer.GetSpecificAlertsSchedule(MyAlertID);
            // MyCollectionAlertsSched = MyRem.LiveMonServer.GetAllAlerts

           // LiveMonitoring.IRemoteLib.AlertDetails.AlertScheduleDef MyAlertSched = default(LiveMonitoring.IRemoteLib.AlertDetails.AlertScheduleDef);

            SchedClearRows();

            foreach (LiveMonitoring.IRemoteLib.AlertDetails.AlertScheduleDef MyAlertSched in MyCollectionAlertsSched)
            {
                try
                {
                    AddSchedRows((new string[] {
                    MyAlertSched.Day.ToString(),
                    MyAlertSched.StartTime.ToString(),
                    MyAlertSched.EndTime.ToString(),
                    MyAlertSched.ID.ToString()
                }));


                }
                catch (Exception ex)
                {
                }

            }
        }
        public void AddSchedRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytable"], DataTable)
            //ListFiles()

            if (Session["mySchedtable"] == null == false)
            {
                dt = (DataTable)Session["mySchedtable"];
                //For Each row1 As DataRow In dt.Rows
                // dt.ImportRow(row1)
                //Next

            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Day", typeof(int));
                dt.Columns.Add("StartTime", typeof(DateTime));
                dt.Columns.Add("EndTime", typeof(DateTime));
                dt.Columns.Add("ID", typeof(int));
            }
            Row = dt.NewRow();
            Row[0] = Convert.ToInt32(RowVals[0]);
            Row[1] = Convert.ToDateTime(RowVals[1]);
            Row[2] = Convert.ToDateTime(RowVals[2]);
            Row[3] = Convert.ToInt32(RowVals[3]);

            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mySchedtable"] = dt;
            SchedGridBind(dt);

        }

        public void SchedClearRows()
        {
            DataTable dt = new DataTable();
            Session["mySchedtable"] = dt;
            SchedGridBind(dt);
        }

        public void SchedGridBind(DataTable dt)
        {
            AlertsSchedgrid.DataSource = dt;
            AlertsSchedgrid.DataKeyNames = (new string[] { "ID" });
            AlertsSchedgrid.DataBind();
        }
        protected void AlertsSchedgrid_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            AlertsSchedgrid.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mySchedtable"];
            SchedGridBind(dt);
        }

        

        protected void AlertsSchedgrid_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void AlertsSchedgrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //'
            dynamic MyRowID = e.Keys[0];
            //Dim myrowsel As UltraGridRow = GridContacts.SelectedDataKey.Value ' e.e.Cell.Row
            //delete cmd
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            if (MyRem.LiveMonServer.DeleteAlertSchedule(Convert.ToInt32(e.Keys[0])) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Alert schedule not deleted.";
                Alertsgrid.Focus();
            }
            else
            {
                //Me.ContactID.Text = ""
                // errLbl.Visible = False
                // Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value
                //LoadGridRow(CInt(Session["myAlertScheduleID"]))
                LoadScheduleGrid(Convert.ToInt32(Session["myAlertScheduleID"]));
            }

            //Try
            // Dim MyRem As New LiveMonitoring.GlobalRemoteVars
            // MyRem.LiveMonServer.DeleteAlertContactLink(e.Keys(0))
            // LoadPage()
            // LoadContactGrid(CInt(Me.txtID.Text))
            //Catch ex As Exception
            // 'if err
            // lblErr.Visible = True
            // lblErr.Text = "Error deleting link:" & ex.Message

            //End Try
        }
        public AddAlertSchedule()
        {
            Load += Page_Load;
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ContactID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select Alert.";

                Alertsgrid.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.Day.SelectedValue))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select day.";

                Alertsgrid.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.SchedStartTime.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select Start Time.";

                Alertsgrid.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.SchedEndTime.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select End Time.";

                Alertsgrid.Focus();

                return;
            }
            this.errLbl.Visible = false;
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            LiveMonitoring.IRemoteLib.AlertDetails.AlertScheduleDef MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails.AlertScheduleDef();
            MyAlert.ID = Convert.ToInt32(this.ContactID.Text);
            MyAlert.Day = Convert.ToInt32(Day.SelectedValue);
            //(Convert.ToDateTime(this.SchedStartTime.Text)
            MyAlert.StartTime = (Convert.ToDateTime(this.SchedStartTime.Text));
            MyAlert.EndTime = (Convert.ToDateTime(this.SchedEndTime.Text));
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            if (MyRem.LiveMonServer.AddNewAlertSchedule(MyAlert) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Alert schedule not saved Error.";

                Alertsgrid.Focus();

            }
            else
            {
                successMessage.Visible = true;
                lblSucces.Text = "Alert schedule added.";

                Alertsgrid.Focus();

            }
            LoadScheduleGrid(Convert.ToInt32(Session["myAlertScheduleID"]));
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {

        }

        protected void btnClearScheduleFields_Click(object sender, EventArgs e)
        {

        }
    }
}