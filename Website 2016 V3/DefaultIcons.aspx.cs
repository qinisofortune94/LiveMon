using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class DefaultIcons : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                try
                {
                    gridDefaultImages.DataSource = getDefaultIcons();
                    gridDefaultImages.DataBind();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanSave() == true)
                {

                    if (SaveDefaultIcons() == true)
                    {
                        successMessage.Visible = true;
                        lblSucces.Text = "Default icons settings was successfully saved.";

                        gridDefaultImages.DataSource = getDefaultIcons();
                        gridDefaultImages.DataBind();
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Saving failed please try again.";
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        public bool SaveDefaultIcons()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.DefaultIcons NewDefaultIcons = new LiveMonitoring.IRemoteLib.DefaultIcons();
            NewDefaultIcons.LocationIcon = Myfunc.Strip_Image(this.filLocationIcon);
            NewDefaultIcons.LocationIconByte = MyRem.ImagetoByte(NewDefaultIcons.LocationIcon, ImageFormat.Bmp);
            NewDefaultIcons.SiteIcon = Myfunc.Strip_Image(this.filSiteIcon);
            NewDefaultIcons.SiteIconByte = MyRem.ImagetoByte(NewDefaultIcons.SiteIcon, ImageFormat.Bmp);
            NewDefaultIcons.CapturedBy = MyUser.ID;
            int Myresp = MyRem.LiveMonServer.AddNewDefaultIcons(NewDefaultIcons);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }

        public bool CanSave()
        {

            if (filLocationIcon.FileName.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please upload location icon.";

                filLocationIcon.Focus();
                return false;
            }

            if (filSiteIcon.FileName.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please upload site icon.";

                filSiteIcon.Focus();
                return false;
            }

            return true;
        }

        private DataTable getDefaultIcons()
        {
            string sqlQuery = "DefaultIcons_get_DefaultIcons";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        public void DefaultImages_DefaultImages()
        {
            Load += Page_Load;
        }
    }
}