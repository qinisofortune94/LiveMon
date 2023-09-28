using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using LiveMonitoring;

namespace website2016V2
{
    public partial class SiteMaster : MasterPage
    {
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Response.Redirect("Index.aspx");  
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                try
                {
                    if (Convert.ToBoolean(MyDataAccess.GetAppSetting("BySite")) == true)
                    {
                        LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                        MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                        if (MyUser.UserSites.Count > 1)
                        {
                            //cmbCurrentSite.Visible = true;
                            cmbCurrentSite.BorderColor = System.Drawing.Color.SeaGreen;
                        }
                        cmbCurrentSite.Items.Clear();
                        Sites RetSites = new Sites(MyUser.ID);

                        List<MySite> MySitesList = new List<MySite>();
                        bool firstitem = true;
                        foreach (Sites.Site MySiteID in RetSites.SitesList)
                        {
                            try
                            {
                                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                MyItem.Text = MySiteID.SiteObj.SiteName;
                                //MySiteID.SiteName
                                MyItem.Value = MySiteID.SiteObj.SiteID.ToString();

                                if ((Session["SelectedSite"] == null) == false)
                                {
                                    if (Convert.ToInt32(Session["SelectedSite"]) == MySiteID.SiteObj.SiteID)
                                    {
                                        MyItem.Selected = true;
                                    }
                                }
                                else
                                {
                                    if (firstitem)
                                    {
                                        Session["SelectedSite"] = MySiteID.SiteObj.SiteID;
                                        MyItem.Selected = true;
                                        firstitem = false;
                                    }
                                }
                                cmbCurrentSite.Items.Add(MyItem);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        try
                        {
                            LiveMonitoring.testing test = new LiveMonitoring.testing();
                            test.SortDropDown(cmbCurrentSite);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }

        public class MySite
        {
            public int siteID { get; set; }
            public string siteName { get; set; }
        }

        protected void cmbCurrentSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SelectedSite"] = cmbCurrentSite.SelectedValue;
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }

}