using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
	public partial class AlertHistory : System.Web.UI.Page
	{
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
        //Private myvar As New LiveMonitoring.SimpleEnc
        DateTime StartDT, EndDT;
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

                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);

                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }



                if (IsPostBack == false)
                {
                    LoadPage(StartDT, EndDT);
                      

                }
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
                dt.Columns.Add("ID", typeof(Int32));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Message Alert", typeof(string));
                dt.Columns.Add("Via", typeof(string));
                dt.Columns.Add("Date Sent", typeof(DateTime));
                
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];


            dt.Rows.Add(Row);
            Session["myContacttable"] = dt;
            GridContactBind(dt);

        }
        public void ClearHistoryRows()
        {
            DataTable dt = new DataTable();
            Session["myContacttable"] = dt;
            GridContactBind(dt);
        }
        public void GridContactBind(DataTable dt)
        {
            GridAlertHistory.DataSource = dt;
            GridAlertHistory.DataKeyNames = (new string[] { "ID" });
            GridAlertHistory.DataBind();
        }
        protected void Usergrid_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridAlertHistory.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytableSensSched"];
            GridContactBind(dt);
        }

        public void LoadPage(DateTime StartDT, DateTime EndDT)
{
	this.txtStart.Text = StartDT.ToString();
	this.txtEnd.Text = EndDT.ToString();
	Collection MyCollectionAlerts = new Collection();
	LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
	MyCollectionAlerts = MyRem.LiveMonServer.GetAllAlertHistoryByDate(StartDT, EndDT);
	Array AlertTypeItems = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
	Array AlertTypeVals = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));

	//LiveMonitoring.IRemoteLib.AlertHistory MyAlert = default(LiveMonitoring.IRemoteLib.AlertHistory);
    ClearHistoryRows();
    foreach (LiveMonitoring.IRemoteLib.AlertHistory MyAlert in MyCollectionAlerts)
    {

        AddContactRows((new string[] {
                    
                    MyAlert.ID.ToString(),
                    MyAlert.AlertType.ToString(),
                   MyAlert.AlertMessage.ToString(),
                    MyAlert.Dest.ToString(),
                    MyAlert.Sent.ToString()
           
           
                }));
	}
}
        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridAlertHistory.PageIndex = e.NewPageIndex;
            LoadPage(StartDT, EndDT);
        }
        protected void btnGenerate_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtStart.Text))
            {
                warningMessage.Visible = true;
                lblError.Text = "Please select a StartDate!";

                return;
            }
            if (string.IsNullOrEmpty(this.txtEnd.Text))
            {

                warningMessage.Visible = true;
                lblError.Text = "Please select a EndDate!";

                return;
            }
            LoadPage(Convert.ToDateTime(this.txtStart.Text), Convert.ToDateTime(this.txtEnd.Text));
        }

       
       


	}
}