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
using Infragistics.WebUI;
using System.Text;

namespace website2016V2
{
    public partial class SensorScanSchadule : System.Web.UI.Page
    {

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
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            successMessage.Visible = false;
            warningMessage.Visible = false;
            errorMessage.Visible = false;

            //ok logged on level ?
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedView.aspx");
            }


        }
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                // Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                // MyRem.LiveMonServer.GetSpecificAlerts()
                Session["mytableSensSched"] = null;
                LoadSensorsGrid();
                //MyRem)
            }
            if (Alertsgrid.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                Alertsgrid.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                Alertsgrid.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                Alertsgrid.FooterRow.TableSection = TableRowSection.TableFooter;
            }
            //else if(AlertsSchedgrid.Rows.Count > 0)
            //{
            //    //Replace the <td> with <th> and adds the scope attribute
            //    AlertsSchedgrid.UseAccessibleHeader = true;

            //    //Adds the <thead> and <tbody> elements required for DataTables to work
            //    AlertsSchedgrid.HeaderRow.TableSection = TableRowSection.TableHeader;

            //    //Adds the <tfoot> element required for DataTables to work
            //    AlertsSchedgrid.FooterRow.TableSection = TableRowSection.TableFooter;
            //}
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

            else
            {
                if (commandName == "SelectItem")
                {
                    ContactID.Text = Id;
                    Session["mySensorScheduleID"] = Alertsgrid.DataKeys[myRow.RowIndex].Value.ToString();
                    LoadGridRow(Convert.ToInt32(Alertsgrid.DataKeys[myRow.RowIndex].Value));
                    LoadScheduleGrid(Convert.ToInt32(Alertsgrid.DataKeys[myRow.RowIndex].Value));

                    //ViewState["Id"] = Id;

                    //SampleLogic business = new SampleLogic();

                    //int RecordId = Convert.ToInt16(Id);
                    //business.Delete(RecordId);

                    ////Refresh Grid
                    //LoadData();
                }

                if (commandName == "DeleteItem")
                {
                    ViewState["Id"] = Id;

                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    if (MyRem.LiveMonServer.DeleteSensorScanSchedule(Convert.ToInt32(ViewState["Id"])) == false)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Sensor Schedule was not deleted successfully.";
                    }
                    else
                    {
                        successMessage.Visible = true;
                        lblSucces.Text = "Sensor Schedule was deleted successfully.";
                        //Me.ContactID.Text = ""
                        // errLbl.Visible = False
                        // Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value
                        //LoadGridRow(CInt(Session["myAlertScheduleID"]))
                        LoadScheduleGrid(Convert.ToInt32(Session["mySensorScheduleID"]));
                    }
                }


            }
        }
        private void LoadSensorsGrid()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;
            if ((MyCollection == null) == false)
            {
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;

                            AddRows((new string[] {
                            Mysensor.ID.ToString(),
                            Mysensor.Caption,
                            Mysensor.ScanRate.ToString()
                        }));
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

        }

        public void AddRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytableSensSched"], DataTable)
            //ListFiles()

            if (Session["mytableSensSched"] == null == false)
            {
                dt = (DataTable)Session["mytableSensSched"];
                //For Each row1 As DataRow In dt.Rows
                // dt.ImportRow(row1)
                //Next

            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("Caption", typeof(string));
                dt.Columns.Add("ScanRate", typeof(double));

            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = Convert.ToDouble(RowVals[2]);
            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mytableSensSched"] = dt;
            GridBind(dt);

        }

        public void ClearRows()
        {
            DataTable dt = new DataTable();
            Session["mytableSensSched"] = dt;
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
            dt = (DataTable)Session["mytableSensSched"];
            GridBind(dt);
        }
        protected void Alertsgrid_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                Session["mySensorScheduleID"] = Alertsgrid.SelectedDataKey.Value;
                LoadGridRow(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));
                LoadScheduleGrid(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));


            }
            catch (Exception ex)
            {
            }
        }
        private void LoadScheduleGrid(int MyAlertID)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                LiveMonitoring.IRemoteLib.SensorDetails MyCollectionAlertsSched = default(LiveMonitoring.IRemoteLib.SensorDetails);
                MyCollectionAlertsSched = MyRem.LiveMonServer.GetSpecificSensor(MyAlertID);
                // MyCollectionAlertsSched = MyRem.LiveMonServer.GetAllAlerts

                //LiveMonitoring.IRemoteLib.SensorScheduleDef MyAlertSched = default(LiveMonitoring.IRemoteLib.SensorScheduleDef);
                try
                {
                    if (MyCollectionAlertsSched.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMBusOffOnScheduledPoint || MyCollectionAlertsSched.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMBusOnOffScheduledPoint)
                    {
                        SensorValue.Visible = true;
                        SensorLabel.Visible = true;
                        SensorName.Visible = true;
                        btnToggleOn.Visible = true;
                        btnToggleOff.Visible = true;
                        SensorName.Text = MyCollectionAlertsSched.Caption + " : ";
                        LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = (LiveMonitoring.IRemoteLib.SensorFieldsDef)MyCollectionAlertsSched.Fields[1];
                        SensorValue.Text = MyField.Caption + ":" + MyField.LastValue.ToString();
                    }
                    else
                    {
                        SensorValue.Visible = false;
                        SensorLabel.Visible = false;
                        SensorName.Visible = false;
                        btnToggleOn.Visible = false;
                        btnToggleOff.Visible = false;
                        SensorName.Text = "";
                        SensorValue.Text = "";
                    }
                }
                catch (Exception)
                {

                    throw;
                }


                SchedClearRows();
                if ((MyCollectionAlertsSched.Schedule == null) == false)
                {

                    foreach (LiveMonitoring.IRemoteLib.SensorScheduleDef MyAlertSched in MyCollectionAlertsSched.Schedule)
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

            }
            catch (Exception ex)
            {
            }
        }
        public void AddSchedRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytableSensSched"], DataTable)
            //ListFiles()

            if (Session["mySensSchedtable"] == null == false)
            {
                dt = (DataTable)Session["mySensSchedtable"];
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
            Session["mySensSchedtable"] = dt;
            SchedGridBind(dt);

        }

        public void SchedClearRows()
        {
            DataTable dt = new DataTable();
            Session["mySensSchedtable"] = dt;
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
            dt = (DataTable)Session["mySensSchedtable"];
            SchedGridBind(dt);
        }

        private void LoadGridRow(int RowID)
        {
            ContactID.Text = RowID.ToString();
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
            if (MyRem.LiveMonServer.DeleteSensorScanSchedule(Convert.ToInt32(e.Keys[0])) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Sensor Schedule was not deleted successfully.";
            }
            else
            {
                successMessage.Visible = true;
                lblSucces.Text = "Sensor Schedule was deleted successfully.";
                //Me.ContactID.Text = ""
                // errLbl.Visible = False
                // Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value
                //LoadGridRow(CInt(Session["myAlertScheduleID"]))
                LoadScheduleGrid(Convert.ToInt32(Session["mySensorScheduleID"]));
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

        //protected void Day_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(Day.SelectedValue) == 10)
        //    {
        //        SchedStartTime.EditModeFormat = "MM/dd/yyyy h:mm:ss";
        //        SchedStartTime.DisplayModeFormat = "MM/dd/yyyy h:mm:ss";
        //        SchedEndTime.EditModeFormat = "MM/dd/yyyy h:mm:ss";
        //        SchedEndTime.DisplayModeFormat = "MM/dd/yyyy h:mm:ss";
        //    }
        //    else
        //    {
        //        SchedStartTime.EditModeFormat = "H:mm:ss";
        //        SchedStartTime.DisplayModeFormat = "H:mm:ss";
        //        SchedEndTime.EditModeFormat = "H:mm:ss";
        //        SchedEndTime.DisplayModeFormat = "H:mm:ss";

        //    }
        //}
        public void AddSensorSchedule()
        {
            Load += Page_Load;
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ContactID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select Alert.";
                return;
            }
            if (string.IsNullOrEmpty(this.Day.SelectedValue))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select day.";
                return;
            }
            if (string.IsNullOrEmpty(this.SchedStartTime.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select Start Time.";
                return;
            }
            if (string.IsNullOrEmpty(this.SchedEndTime.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select End Time.";
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
            LiveMonitoring.IRemoteLib.SensorScheduleDef MyScanSchedule = new LiveMonitoring.IRemoteLib.SensorScheduleDef();
            MyScanSchedule.ID = Convert.ToInt32(this.ContactID.Text);

            MyScanSchedule.Day = Convert.ToInt32(this.Day.SelectedValue);
            MyScanSchedule.StartTime = (Convert.ToDateTime(this.SchedStartTime.Text));
            //MyScanSchedule.EndTime = DateTime.Parse(this.SchedEndTime.Text);

            MyScanSchedule.EndTime = (Convert.ToDateTime(this.SchedEndTime.Text));

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            if (MyRem.LiveMonServer.AddNewSensorScanSchedule(MyScanSchedule) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Sensor Schedule was not saved successfully.";
                try
                {
                    MyRem.WriteLog("Add NewSensorScanSchedule Failed", "User:" + MyUser.ID.ToString() + "|" + this.ContactID.ToString());

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                successMessage.Visible = true;
                lblSucces.Text = "Sensor Schedule was saved successfully.";
                try
                {
                    MyRem.WriteLog("Add NewSensorScanSchedule Succeeded", "User:" + MyUser.ID.ToString() + "|" + this.ContactID.ToString());

                }
                catch (Exception ex)
                {
                }
            }
            LoadScheduleGrid(Convert.ToInt32(Session["mySensorScheduleID"]));
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {


        }

        protected void btnClearScheduleFields_Click(object sender, EventArgs e)
        {
            ContactID.Text = "";
            SchedStartTime.Text = "";
            SchedEndTime.Text = "";
        }

        protected void cmdToggleOn(object sender, EventArgs e)
        {

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<string> Myresp = MyRem.LiveMonServer.ToggleONOffSensor(Convert.ToInt32(ContactID.Text), 1);
            if (Myresp != null)
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1><tr><td>");
                MyHtmlStr.Append("Sent On:" + DateTime.Now.ToString());
                MyHtmlStr.Append("</tr></td>");
                foreach (string MyString in Myresp)
                {
                    try
                    {
                        MyHtmlStr.Append("<tr><td>");
                        MyHtmlStr.Append(MyString);
                        MyHtmlStr.Append("</td></tr>");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                MyHtmlStr.Append("</td></table>");
                results.InnerHtml = "";
                results.InnerHtml = MyHtmlStr.ToString();

            }
            else
            {
                try
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Toggle On Sensor Failed!";
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected void cmdToggleOff(object sender, EventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<string> Myresp = MyRem.LiveMonServer.ToggleONOffSensor(Convert.ToInt32(ContactID.Text), 0);
            if (Myresp != null)
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1><tr><td>");
                MyHtmlStr.Append("Sent On:" + DateTime.Now.ToString());
                MyHtmlStr.Append("</tr></td>");
                foreach (string MyString in Myresp)
                {
                    try
                    {
                        MyHtmlStr.Append("<tr><td>");
                        MyHtmlStr.Append(MyString);
                        MyHtmlStr.Append("</td></tr>");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                MyHtmlStr.Append("</td></table>");
                results.InnerHtml = "";
                results.InnerHtml = MyHtmlStr.ToString();

            }
            else
            {
                try
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Toggle On Sensor Failed!";
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}