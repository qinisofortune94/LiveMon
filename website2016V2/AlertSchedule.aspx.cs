using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class AlertSchedule : System.Web.UI.Page
    {


        protected void btnSend_Click(object sender, EventArgs e)
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

            LiveMonitoring.IRemoteLib.AlertDetails.AlertContactScheduleDef MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails.AlertContactScheduleDef();
            MyAlert.ID = Convert.ToInt32(ContactID.Text);

            MyAlert.Day = Convert.ToInt32(lstDay.SelectedValue);
            MyAlert.StartTime = Convert.ToDateTime(txtStartTime.Text);
            MyAlert.EndTime = Convert.ToDateTime(txtEndTime.Text);

            if (MyAlert.EndTime < MyAlert.StartTime)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Not saved. Start time after end time";
                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            if (MyRem.LiveMonServer.AddNewAlertContactSchedule(MyAlert) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "The saving failed. Please try again.";
            }
            else
            {

                LoadGrid();
            }
        }

        //protected void btnFinish_Click(object sender,EventArgs e)
        //{
        //    Response.Redirect("AlertContact.aspx?AlertID=" +this.AlertID.ToString());


        ////}


        protected void Page_Load(object sender, System.EventArgs e)
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
                //ok logged on level ?

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
                    int reqAlertID = 0;
                    reqAlertID = Convert.ToInt32(Request.QueryString["AlertID"]);
                    txtAlertID.Text = reqAlertID.ToString();
                    int reqContactID = 0;
                    if (!string.IsNullOrEmpty(Request.QueryString["ContactID"]))
                    {
                        reqContactID = Convert.ToInt32(Request.QueryString["ContactID"]);
                    }
                    ContactID.Text = reqContactID.ToString();
                }
                LoadGrid();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void AddContactRows(string[] RowVals)
        {
            System.Data.DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytable"], DataTable)
            //ListFiles()

            if (Session["myContacttable"] == null == false)
            {
                dt = (DataTable)Session["myContacttable"];
                //For Each row1 As DataRow In dt.Rows
                //    dt.ImportRow(row1)
                //Next

            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Day", typeof(string));
                dt.Columns.Add("StartTime", typeof(DateTime));
                dt.Columns.Add("EndTime", typeof(DateTime));

            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];


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
            AlertsSchedgrid.DataSource = dt;
            AlertsSchedgrid.DataKeyNames = (new string[] { "Day" });
            AlertsSchedgrid.DataBind();
        }


        public void LoadGrid()
        {

            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            if (string.IsNullOrEmpty(this.ContactID.Text))
            {
                return;
            }
            MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsContactsSchedule(Convert.ToInt32(this.txtAlertID.Text), Convert.ToInt32(this.ContactID.Text));
            //LiveMonitoring.IRemoteLib.AlertDetails.AlertContactScheduleDef MyAlert = default(LiveMonitoring.IRemoteLib.AlertDetails.AlertContactScheduleDef);



            ClearContactRows();
            foreach (LiveMonitoring.IRemoteLib.AlertDetails.AlertContactScheduleDef MyAlert in MyCollectionAlerts)
            {
                AddContactRows((new string[] {
                MyAlert.Day.ToString(),
                MyAlert.StartTime.ToString(),
                MyAlert.EndTime.ToString(),

            }));
                //Dim myrow As New UltraGridRow(True)
                //myrow.Cells.Add()
                //myrow.Cells(0).Value = MyAlert.ContactName
                //myrow.Cells.Add()
                //myrow.Cells(1).Value = MyAlert.Type
                //myrow.Cells.Add()
                //myrow.Cells(2).Value = MyAlert.OutputParam
                //myrow.Cells.Add()
                //myrow.Cells(3).Value = MyAlert.OutputParam1
                //myrow.Cells.Add()
                //myrow.Cells(4).Value = MyAlert.OutputParam2
                //myrow.Cells.Add()
                //myrow.Cells(5).Value = MyAlert.OutputParam3
                //myrow.Cells.Add()
                //myrow.Cells(6).Value = MyAlert.OutputParam4
                //GridContactsOld.Rows.Add(myrow)
            }
        }



    }
}