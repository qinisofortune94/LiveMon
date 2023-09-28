using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SLDeviceMapPage : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess Mydataaccess = new LiveMonitoring.DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                //Label user = this.Master.FindControl("lblUser") as Label;
                //Label loginUser = this.Master.FindControl("lblUser2") as Label;
                //Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                //loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                //user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                //LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                string MyRef = Mydataaccess.GetAppSetting("SLServiceRef");
                string MyPmaxRef = Mydataaccess.GetAppSetting("PmaxServiceRef");

                this.Xaml1.Source = "~/ClientBin/SLDeviceMap.xap";
                if (!string.IsNullOrEmpty(Request.QueryString["DisplayID"]))
                {
                    int MyDisplay = 0;
                    MyDisplay = Convert.ToInt32(Request.QueryString["DisplayID"]);
                    //Me.Xaml1.InitParameters = "DisplayID=" + CStr(Request.QueryString("DisplayID")) + ",ServiceRef=" + MyRef + ",PmaxServiceRef=" + MyPmaxRef + ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password
                    if ((MyUser == null) == false)
                    {
                        this.Xaml1.InitParameters = "DisplayID=" + Convert.ToString(Request.QueryString["DisplayID"]) + ",ServiceRef=" + MyRef + ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password;
                    }
                    else
                    {
                        this.Xaml1.InitParameters = "DisplayID=" + Convert.ToString(Request.QueryString["DisplayID"]) + ",ServiceRef=" + MyRef;
                        //+ ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password
                    }
                }
                else
                {
                    string MyDisplay = Mydataaccess.GetAppSetting("DefaultMapDisplay");
                    if (string.IsNullOrEmpty(MyDisplay))
                    {
                        MyDisplay = "2";
                    }
                    //-99 is old style siteid setting
                    if (MyDisplay != "-99")
                    {
                        if ((MyUser == null) == false)
                        {
                            this.Xaml1.InitParameters = "DisplayID=" + MyDisplay + ",ServiceRef=" + MyRef + ",PmaxServiceRef=" + MyPmaxRef + ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password;
                        }
                        else
                        {
                            this.Xaml1.InitParameters = "DisplayID=" + MyDisplay + ",ServiceRef=" + MyRef;
                            //+ ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password
                        }
                    }
                    else
                    {
                        if ((MyUser == null) == false)
                        {
                            this.Xaml1.InitParameters = "ServiceRef=" + MyRef + ",PmaxServiceRef=" + MyPmaxRef + ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password;
                        }
                        else
                        {
                            this.Xaml1.InitParameters = "ServiceRef=" + MyRef + ",PmaxServiceRef=" + MyPmaxRef;
                            //+ ",UserName=" + MyUser.UserName + ",Password=" + MyUser.Password
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public SLDeviceMapPage()
        {
            Load += Page_Load;
        }
    }
}