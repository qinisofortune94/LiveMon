using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using Infragistics.WebUI.UltraWebGrid;

namespace website2016V2
{
    public partial class AlertHistory1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["LoggedIn"] != "True")
            {
                Response.Redirect("NotAuthorisedLogon.aspx");
            }
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
            if (IsPostBack == false)
            {
                LoadPage(DateAndTime.DateAdd(DateInterval.Day, -7,DateTime.Now),DateTime.Now);
            }
        }
        public void LoadPage(DateTime StartDT, DateTime EndDT)
        {
            this.txtStart.Text = Convert.ToDateTime(StartDT).ToString();
            this.txtEnd.Text = Convert.ToDateTime(EndDT).ToString();
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllAlertHistoryByDate(StartDT, EndDT);
            Array AlertTypeItems = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
            Array AlertTypeVals = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));

            LiveMonitoring.IRemoteLib.AlertHistory MyAlert;
            gvAlertHistory.Clear();
            foreach (MyAlert in MyCollectionAlerts)
            {
                UltraGridRow myrow = new UltraGridRow(true);
                myrow.Cells.Add();
                int mycnt;
                for (mycnt = 0; mycnt <= AlertTypeVals.Length - 1; mycnt++)
                {
                    if (AlertTypeVals(mycnt) == MyAlert.AlertType)
                    {
                        myrow.Cells(0).Value = AlertTypeItems(mycnt);
                        break; 
                    }
                }
                myrow.Cells.Add();
                myrow.Cells(1).Value = MyAlert.AlertMessage;
                myrow.Cells.Add();
                myrow.Cells(2).Value = MyAlert.Dest;
                myrow.Cells.Add();
                myrow.Cells(3).Value = MyAlert.Sent;
                myrow.Tag = MyAlert.ID;
                GridAlertHistory.Rows.Add(myrow);
            }
        }

        protected void btnGenerate_Click(object sender, System.EventArgs e)
        {
            if (this.txtStart.Text == "")
            {
                lblErr.Visible = true;
                lblErr.Text = "Please select a StartDate!";

                return;
            }
            if (this.txtEnd.Text == "")
            {
                lblErr.Visible = true;
                lblErr.Text = "Please select a EndDate!";

                return;
            }
            LoadPage((System.DateTime)this.txtStart.Text, (System.DateTime)this.txtEnd.Text);
        }
    }
}
