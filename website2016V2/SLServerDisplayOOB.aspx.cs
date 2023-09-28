using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SLServerDisplayOOB : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess Mydataaccess = new LiveMonitoring.DataAccess();
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

                this.Xaml1.Source = "~/ClientBin/SLServerDisplayOOB.xap";
                string MyRef = Mydataaccess.GetAppSetting("SLServiceRef");
                if (!string.IsNullOrEmpty(Request.QueryString["DisplayID"]))
                {
                    int MyDisplay = 0;
                    MyDisplay = Convert.ToInt32(Request.QueryString["DisplayID"]);
                    this.Xaml1.InitParameters = "DisplayID=" + Convert.ToString(Request.QueryString["DisplayID"]) + ",ServiceRef=" + MyRef;
                }
                else
                {
                    string MyDisplay = Mydataaccess.GetAppSetting("DefaultServerDisplay");
                    if (string.IsNullOrEmpty(MyDisplay))
                    {
                        MyDisplay = "1";
                    }
                    this.Xaml1.InitParameters = "DisplayID=" + MyDisplay + ",ServiceRef=" + MyRef;
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public SLServerDisplayOOB()
        {
            Load += Page_Load;
        }
    }
}