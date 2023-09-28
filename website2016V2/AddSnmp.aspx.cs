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
using static System.Net.WebRequestMethods;

namespace website2016V2
{
    public partial class AddSnmp : System.Web.UI.Page
    {
       

        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
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

                //ok logged on level ?
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                //MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
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

                    successMessage.Visible = false;
                    errorMessage.Visible = false;
                    warningMessage.Visible = false;
                    LoadPage();
                    LoadSites();
                    LoadDevices();


                }
            }
            else
            {
                Response.Redirect("Index.aspx");
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
            try
            {
                if ((MyCollection == null) == false)
                {

                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
                        {
                            LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
                            MyItem.Value = MyLocation.Id.ToString();
                            MyItem.Selected = false;
                            DdlDevicelocation.Items.Add(MyItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
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


                //ar1.Sort(ar1[dd], new ListItemComparer());
                dd.Items.Clear();
                dd.Items.AddRange(ar);

            }
            catch (Exception ex)
            {
            }

        }


       


        

      

        private class ListItemComparer : IComparer
        {

            //(LiveMonitoring.IRemoteLib.SNMPManagerDetails.AuthProtocol)Convert.ToInt32
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




        private DataTable getSnmp()
        {
            try
            {
                string sqlQuery = "Select * from SNMPDevice";
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
            catch (Exception ex)
            {
                lblSucces.Text = ex.Message;
                return null;
            }
        }

        public void LoadPage()
        {
            successMessage.Visible = false;
            warningMessage.Visible = false;
            errorMessage.Visible = false;

            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.LoadPage(ddlSNMPVersion, dllAuthenticationProtocol);
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



            if (this.txtPassword.Text != this.txtConfirmPassword.Text)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Password is diffrent, Please retype.";
                txtCaption.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply caption.";
                txtCaption.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtCommunity.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply community.";
                txtCaption.Focus();
                return;
            }

            if (!Regex.IsMatch(txtRemotePort.Text,
                         @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "RemotePort must be Number";
                txtCaption.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;
            }

            if (!Regex.IsMatch(txtTimeout.Text,
                       @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Incorrect Time out Please enter the correct time";
                txtCaption.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;

            }

            if (!Regex.IsMatch(txtLocalPort.Text,
                     @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Local Port must be a number";
                txtCaption.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;
            }



            string dat = txtIP1.Text.ToString() + "." + txtIP2.Text.ToString() + "." + txtIP3.Text.ToString() + "." + txtIP4.Text.ToString();
            if (Regex.IsMatch(dat, @"\b\d{ 1,3}\.\d{ 1,3}\.\d{ 1,3}\.\d{ 1,3}\b"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Invalid IP Address";
                //txtIPAddressLocal.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;
               
            }
          

            //If Me.txtPort.Text = "" Then
            // lblErr.Visible = True
            // lblErr.Text = "Please supply port!"
            // Exit Sub
            //End If
            if (string.IsNullOrEmpty(txtRemotePort.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply RemoteHost.";
                txtCaption.Focus();
                return;
            }
            //If Me.txtRemotePort.Text = "" Then
            // lblErr.Visible = True
            // lblErr.Text = "Please supply Remote Port!"
            // Exit Sub
            //End If
            int d = Convert.ToInt32(ddlSNMPVersion.SelectedValue);

            if (d == (int)LiveMonitoring.IRemoteLib.SNMPManagerDetails.SNMPVer.Ver3)
            {
                if (string.IsNullOrEmpty(this.TxtUser.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please supply Username.";
                    txtCaption.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please supply Password.";
                    txtCaption.Focus();
                    return;
                }
            }
            if (string.IsNullOrEmpty(this.txtLocalEngineId.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply Local Engine Id.";
                txtCaption.Focus();
                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            LiveMonitoring.IRemoteLib.SNMPManagerDetails NewSNMPDevice = new LiveMonitoring.IRemoteLib.SNMPManagerDetails();

            

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





            NewSNMPDevice.AuthenticationProtocol = (LiveMonitoring.IRemoteLib.SNMPManagerDetails.AuthProtocol)Convert.ToInt32(dllAuthenticationProtocol.SelectedValue);
            NewSNMPDevice.Caption = this.txtCaption.Text;
            NewSNMPDevice.Community = this.txtCommunity.Text;

            NewSNMPDevice.Data1 = this.txtData1.Text;

            NewSNMPDevice.Data2 = this.txtData2.Text;
            NewSNMPDevice.Data3 = this.txtData3.Text;
            NewSNMPDevice.ImageNormal = imgNormal;
            NewSNMPDevice.ImageNormalByte = byteNormal;
            NewSNMPDevice.ImageNoResponse = imgNoresponse;
            NewSNMPDevice.ImageNoResponseByte = byteNoresponse;
            NewSNMPDevice.ImageError = imgError;
            NewSNMPDevice.ImageErrorByte = byteError;
            NewSNMPDevice.LocalEngineId = Convert.ToInt32(txtLocalEngineId.Text);
            NewSNMPDevice.LocalHost = dat;  // this.txtIP1.Value.ToString + "." + this.tx.Value.ToString + "." + this.txtIP3.Value.ToString + "." + this.txtIP4.Value.ToString;
            NewSNMPDevice.LocalPort = Convert.ToInt32(txtRemotePort.Text);
            LiveMonitoring.SimpleEnc MyEnc = new LiveMonitoring.SimpleEnc();
           
            if (!string.IsNullOrEmpty(this.txtPassword.Text))
            {
                NewSNMPDevice.Password = MyEnc.EncryptString(this.txtPassword.Text);
            }
            else
            {
                NewSNMPDevice.Password = "";
            }

            NewSNMPDevice.RemoteHost = this.txtRemoteHostName.Text;
            NewSNMPDevice.RemotePort = Convert.ToInt32(txtRemotePort.Text);
            NewSNMPDevice.RequestId = Convert.ToInt32(txtRequestId.Text);
            NewSNMPDevice.SNMPVersion = (LiveMonitoring.IRemoteLib.SNMPManagerDetails.SNMPVer)Convert.ToInt32(ddlSNMPVersion.SelectedValue);
            NewSNMPDevice.Timeout = Convert.ToInt32(this.txtTimeout.Text);
            NewSNMPDevice.User = this.TxtUser.Text;
            try
            {
               
                NewSNMPDevice.Add2Site = Convert.ToInt32(DdlDeviceSite.SelectedValue);
                //End If

            }
            catch (Exception ex)
            {
            }



            
            int Myresp = MyRem.LiveMonServer.AddNewSNMPDevice(NewSNMPDevice);
            if (Myresp>  0)
            {
                
                     
                    successMessage.Visible = true;
                    lblSucces.Text = "Add SNMP Device Succeeded.";
                    txtCaption.Focus();

                    MyRem.WriteLog("Add SNMPrDevice Succeeded", "User:" + MyUser.ID.ToString() + "|" + Myresp.ToString());

               
                    if ((DdlDevicelocation.SelectedValue == null) == false)
                    {
                        MyRem.LiveMonServer.AddEditLocationLink(NewSNMPDevice.ID, Convert.ToInt32(DdlDevicelocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.SNMP_Device, -99);
                    }

             
            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again.";
                txtCaption.Focus();

                try
                {
                    MyRem.WriteLog("Add SNMPDevice Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtCaption.Text);

                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            //lblErr.Visible = false;
            this.txtData1.Text = "";
            this.txtData2.Text = "";
            this.txtData3.Text = "";
            this.txtCaption.Text = "";
            this.txtPassword.Text = "";
            this.txtRemoteHostName.Text = "";
            this.txtRequestId.Text = "";
            this.txtTimeout.Text = "";
            this.TxtUser.Text = "";
            this.txtCommunity.Text = "";
            this.txtRemotePort.Text = "";
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;

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
        public AddSnmp()
        {
            Load += Page_Load;
        }
    }
}