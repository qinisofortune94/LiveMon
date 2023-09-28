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
    public partial class SystemLogs : System.Web.UI.Page
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

                //if (MyIPMonPageSecure > MyUser.UserLevel)
                //{
                //    Response.Redirect("NotAuthorisedView.aspx");
                //}



                if (IsPostBack == false)
                {
                    LoadPage(StartDT,EndDT);
                }
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
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("ID", typeof(Int32));
                dt.Columns.Add("Data", typeof(DateTime));

            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];


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
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllLogHistoryByDate(StartDT, EndDT);
            Array AlertTypeItems = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));

            //LiveMonitoring.IRemoteLib.LogHistory MyAlert = default(LiveMonitoring.IRemoteLib.LogHistory);
            ClearHistoryRows();
            foreach (LiveMonitoring.IRemoteLib.LogHistory MyAlert in MyCollectionAlerts)
            {
                AddContactRows((new string[] {
           
                    MyAlert.Type.ToString(),
                    MyAlert.ID.ToString(),
                    MyAlert.Dt.ToString(),
           
           
                }));
            }
        }

        protected void btnGenerate_Click(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtStart.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select a StartDate.";

                return;
            }
            if (string.IsNullOrEmpty(this.txtEnd.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select an EndDate.";

                return;
            }
            LoadPage(Convert.ToDateTime(this.txtStart.Text), Convert.ToDateTime(this.txtEnd.Text));
        }     

    }
}