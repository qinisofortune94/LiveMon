using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class ClientInfo : System.Web.UI.Page
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
            
            Label1.Text = (Request.ServerVariables["REMOTE_ADDR"]);
            //%> Returns the clients IP address. 
            Label2.Text = (Request.ServerVariables["REMOTE_HOST"]);
            //%> Returns the client hostname 
            Label3.Text = (Request.ServerVariables["REMOTE_USER"]);
            //%> Returns the authenticated client name. 
            Label4.Text = (Request.ServerVariables["REQUEST_METHOD"]);
            // Returns the HTTP request method 
            Label5.Text = (Request.ServerVariables["SCRIPT_NAME"]);
            //%> Returns the name of the script program being executed 
            Label6.Text = (Request.ServerVariables["SERVER_NAME"]);
            //%> Returns the servers hostname or IP address. 
            Label7.Text = (Request.ServerVariables["SERVER_PORT"]);
            //%> Returns the TCP/IP port used. 
            Label8.Text = (Request.ServerVariables["SERVER_PROTOCOL"]);
            //%> Returns name and version of information retrieving protocol 
            Label9.Text = (Request.ServerVariables["SERVER_SOFTWARE"]);
            //%> Returns the name and version of the Web Server Software 
            Label10.Text = (Request.ServerVariables["HTTP_USER_AGENT"]);
            //%> Returns the name of the browser. 
        }

        public ClientInfo()
        {
            Load += Page_Load;
        }
    }
}