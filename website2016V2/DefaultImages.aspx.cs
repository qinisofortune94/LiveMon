using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace website2016V2
{
    public partial class DefaultImages : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanSave() == true)
                {
                    if (SaveDefaultImages() == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Default images settings was successfully saved.";

                        gridDefaultImages.DataSource = getDefaultImages();
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
        public bool SaveDefaultImages()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.DefaultSensorImages NewDefaultSensorImages = new LiveMonitoring.IRemoteLib.DefaultSensorImages();
            NewDefaultSensorImages.ImageNormal = Myfunc.Strip_Image(this.filImageNormal);
            NewDefaultSensorImages.ImageNormalByte = MyRem.ImagetoByte(NewDefaultSensorImages.ImageNormal, ImageFormat.Bmp);
            NewDefaultSensorImages.ImageNoResponse = Myfunc.Strip_Image(this.filImageNoResponse);
            NewDefaultSensorImages.ImageNoResponseByte = MyRem.ImagetoByte(NewDefaultSensorImages.ImageNoResponse, ImageFormat.Bmp);
            NewDefaultSensorImages.ImageError = Myfunc.Strip_Image(this.filImageError);
            NewDefaultSensorImages.ImageErrorByte = MyRem.ImagetoByte(NewDefaultSensorImages.ImageError, ImageFormat.Bmp);
            NewDefaultSensorImages.CapturedBy = MyUser.ID;
            int Myresp = MyRem.LiveMonServer.AddNewDefaultSensorImages(NewDefaultSensorImages);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }

        public bool CanSave()
        {
            HttpPostedFile file = filImageNormal.PostedFile;
            int iFileSize = file.ContentLength;
            if (iFileSize > 15000)
            {
                errorMessage.Visible = true;
                lblError.Text = "Normal Image too large!";
                filImageNormal.Focus();
                return false;
            }

            HttpPostedFile fileerror = filImageError.PostedFile;
            int iFileSizeError = fileerror.ContentLength;
            if (iFileSizeError > 15000)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error Image too large!";
                filImageNormal.Focus();
                return false;
            }

            HttpPostedFile fileNoResponse = filImageNoResponse.PostedFile;
            int iFileSizeNoResp = fileNoResponse.ContentLength;
            if (iFileSizeNoResp > 15000)
            {
                errorMessage.Visible = true;
                lblError.Text = "No Response Image too large!";
                filImageNormal.Focus();
                return false;
            }

            if (filImageNormal.FileName.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please upload Normal Image!";
                filImageNormal.Focus();
                return false;
            }
            if (filImageError.FileName.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please upload Error Image!";
                filImageError.Focus();
                return false;
            }
            if (filImageNoResponse.FileName.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please upload No Response Image!";
                filImageNoResponse.Focus();
                return false;
            }
            return true;
        }
        private DataTable getDefaultImages()
        {


            string sqlQuery = "sensors_get_DefaultImages";
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

                errorMessage.Visible = false;
                successMessage.Visible = false;
                warningMessage.Visible = false;

                try
                {
                    gridDefaultImages.DataSource = getDefaultImages();
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

        protected void gridDefaultImages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridDefaultImages.PageIndex = e.NewPageIndex;
                gridDefaultImages.DataSource = getDefaultImages();
                gridDefaultImages.DataBind();

            }
            catch (Exception ex)
            {
            }
        }
        public void DefaultImages_DefaultImages()
        {
            Load += Page_Load;
        }
    }
}