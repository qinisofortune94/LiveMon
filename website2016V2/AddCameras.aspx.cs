using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class AddCameras : System.Web.UI.Page
    {
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
               // MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                try
                {
                    LoadDefaultImages();

                }
                catch (Exception ex)
                {
                }
                if (IsPostBack == false)
                {
                    LoadSites();
                    LoadDevices();
                }

            }
            else
            {
                Response.Redirect("Index.aspx");
            }        
        }


        private void SortDropDown(DropDownList dd)
        {
            try
            {
                ListItem[] ar = null;
                int i = 0;
                foreach (ListItem li in dd.Items)
                {
                    Array.Resize(ref ar, i + 1);
                    ar[i] = li;
                    i += 1;
                }
                Array ar1 = ar;

                //ar1.Sort(ar1, new ListItemComparer());
                dd.Items.Clear();
                dd.Items.AddRange(ar);

            }
            catch (Exception ex)
            {
            }

        }


        private class ListItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListItem a = (System.Web.UI.WebControls.ListItem)x;
                ListItem b = (System.Web.UI.WebControls.ListItem)y;
                CaseInsensitiveComparer c = new CaseInsensitiveComparer();
                return c.Compare(a.Text, b.Text);
            }
        }



        public void LoadSites()
        {
            try
            {
                List<LiveMonitoring.IRemoteLib.SiteDetails> MyCollection = new List<LiveMonitoring.IRemoteLib.SiteDetails>();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.GetServerAllSites;
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;
                if ((MyCollection == null))
                    return;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        //cmbSensGroup
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
                        {
                            LiveMonitoring.IRemoteLib.SiteDetails MySite = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
                            //not orphans
                            if (MySite.ID > 0)
                            {
                                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                MyItem.Text = MySite.SiteName;
                                MyItem.Value = MySite.ID.ToString();
                                MyItem.Selected = false;
                                try
                                {
                                    if (Convert.ToInt32(Session["SelectedSite"]) == MySite.ID)
                                    {
                                        MyItem.Selected = true;
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                                DdlDeviceSite.Items.Add(MyItem);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                try
                {
                    SortDropDown(DdlDeviceSite);

                }
                catch (Exception ex)
                {
                }


            }
            catch (Exception ex)
            {
            }


        }

        public void LoadDevices()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;

            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
                {
                    LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
                    MyItem.Value =Convert.ToString(MyLocation.Id);
                    MyItem.Selected = false;
                    DdlDevicelocation.Items.Add(MyItem);
                }
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            if (string.IsNullOrEmpty(TxtCapiton.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply a Caption.";

                txtIpAdrres.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtUserName.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply an Username.";

                txtIpAdrres.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply a Password.";

                txtIpAdrres.Focus();
                return;
            }



            if (!Regex.IsMatch(txtPort.Text,
                       @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "RemotePort must be Number";
                txtPort.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;
            }

            if (!Regex.IsMatch(TxtPostEventRecording.Text,
                       @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Incorrect PostEventRecording";
                TxtPostEventRecording.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;

            }

            if (!Regex.IsMatch(TxtPreEventRecording.Text,
                     @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Incorrect PreEventRecording";
                TxtPreEventRecording.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;

            }


            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = new LiveMonitoring.IRemoteLib.CameraDetails();
            MyCamera.Caption = TxtCapiton.Text;
            MyCamera.EventRecord = this.chkEventEnabled.Checked;
            int MyEventRecord = 0;
            int i = 0;
            for (i = 0; i <= chkEvents.Items.Count - 1; i++)
            {
                if (this.chkEvents.Items[i].Selected)
                {
                    MyEventRecord = MyEventRecord | Convert.ToInt32(chkEvents.Items[i].Value);
                }
            }
            MyCamera.Events = MyEventRecord;
            MyCamera.IPAdress = txtIpAdrres.Text;
            MyCamera.Port = Convert.ToInt32(txtPort.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            byte[] byteNormal = null;
            byte[] byteError = null;
            byte[] byteNoresponse = null;

            System.Drawing.Image imgNormal = null;
            System.Drawing.Image imgError = null;
            System.Drawing.Image imgNoresponse = null;

            //NORMAL IMAGE
            if (filImageNormal.FileName.Trim().Length == 0)
            {
                byteNormal = pstrNormal;
            }
            else
            {
                imgNormal = Myfunc.Strip_Image(this.filImageNormal);
                byteNormal = MyRem.ImagetoByte(imgNormal, ImageFormat.Bmp);
            }

            //NO RESPONSE IMAGE
            if (filImageNoResponse.FileName.Trim().Length == 0)
            {
                byteNoresponse = pstrNoresponse;
            }
            else
            {
                imgNoresponse = Myfunc.Strip_Image(this.filImageNoResponse);
                byteNoresponse = MyRem.ImagetoByte(imgNoresponse, ImageFormat.Bmp);
            }

            //ERROR MAGE
            if (filImageError.FileName.Trim().Length == 0)
            {
                byteError = pstrError;
            }
            else
            {
                imgError = Myfunc.Strip_Image(this.filImageError);
                byteError = MyRem.ImagetoByte(imgError, ImageFormat.Bmp);
            }
            MyCamera.ImageError = imgError;
            MyCamera.ImageNoResponse = imgNoresponse;
            MyCamera.ImageNormal = imgNormal;
            MyCamera.ImageNormalByte = byteNormal;
            MyCamera.ImageNoResponseByte = byteNoresponse;
            MyCamera.ImageErrorByte = byteError;
            MyCamera.Password = this.txtPassword.Text;
            MyCamera.User = this.txtUserName.Text;
            MyCamera.PostEventTime = Convert.ToInt32(TxtPostEventRecording.Text);
            MyCamera.PreEventTime = Convert.ToInt32(TxtPreEventRecording.Text);
            MyCamera.Type = Convert.ToInt32(this.DdlType.SelectedValue);
            try
            {
                //If IsNothing(Session["SelectedSite"]) = False Then
                MyCamera.Add2Site = Convert.ToInt32(DdlDeviceSite.SelectedValue);
                //End If

            }
            catch (Exception ex)
            {
            }

            if (MyRem.LiveMonServer.AddNewCamera(MyCamera) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error.";

                txtIpAdrres.Focus();
                try
                {
                    MyRem.WriteLog("Add Camera Failed", "User:" + MyUser.ID.ToString() + "|" + TxtCapiton.Text);

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                successMessage.Visible = true;
                lblSucces.Text = "Add Camera Succeed.";

                txtIpAdrres.Focus();
                try
                {
                    MyRem.WriteLog("Add Camera Succeed", "User:" + MyUser.ID.ToString() + "|" + TxtCapiton);

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if ((DdlDevicelocation.SelectedValue == null) == false)
                    {
                        MyRem.LiveMonServer.AddEditLocationLink(MyCamera.ID, Convert.ToInt32(DdlDevicelocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.Camera, -99);
                    }

                }
                catch (Exception ex)
                {
                }
                this.txtPassword.Text = "";
                this.txtUserName.Text = "";
                this.TxtPostEventRecording.Text = "";
                this.TxtPreEventRecording.Text = "";
                this.txtPort.Text = "100";
            }

        }

        public void LoadDefaultImages()
        {
            string sqlQuery = "select * from SensorDefaultImages";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pstrNormal = (byte[])reader["ImageNormal"];
                pstrError = (byte[])reader["ImageError"];
                pstrNoresponse = (byte[])reader["ImageNoResponse"];

            }

            reader.Close();
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            //ASSIGN DEFAULT IMAGES
            string base64StringpstrNormal = Convert.ToBase64String(pstrNormal, 0, pstrNormal.Length);
            imgNormal.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNormal;

            string base64StringpstrError = Convert.ToBase64String(pstrError, 0, pstrError.Length);
            imgError.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrError;

            string base64StringpstrNoresponse = Convert.ToBase64String(pstrNoresponse, 0, pstrNoresponse.Length);
            imgResponse.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNoresponse;

        }
        public AddCameras()
        {
            Load += Page_Load;
        }
    }
}