using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class DeviceInfo : System.Web.UI.Page
    {
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

                info();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void info()
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
           
            Label1.Text = Environment.WorkingSet.ToString();
            Label2.Text = Environment.Version.ToString();
            Label3.Text = Environment.UserName;
            Label4.Text = Environment.UserDomainName;
            Label5.Text = Environment.TickCount.ToString();
            Label6.Text = Environment.SystemDirectory;
            Label7.Text = Environment.OSVersion.ToString();
            Label8.Text = Environment.MachineName;
            Label9.Text = Environment.CurrentDirectory;
            Label10.Text = Environment.CommandLine;

        }

        public DeviceInfo()
        {
            Load += Page_Load;
        }
    }
}