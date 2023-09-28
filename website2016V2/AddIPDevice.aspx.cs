using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using LiveMonitoring;

namespace website2016V2
{
    public partial class AddIPDevice : System.Web.UI.Page
    {
        
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
        //Private myvar As New LiveMonitoring.SimpleEnc
        protected void Page_Load(object sender, System.EventArgs e)
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");

                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);

                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }

                try
                {
                    if(Page.IsPostBack == false)
                    {
                       
                        LoadDevices();
                        LoadSites();
                    }

                    LoadDefaultImages();
                }
                catch (Exception ex)
                {
                }
                if (IsPostBack == false)
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.ipDevice(ddltype);
                 
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
            MyCollection = MyRem.get_GetServerObjects((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]);
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
                    MyItem.Value = Convert.ToString(MyLocation.Id);
                    ddlDeviceLocation.Items.Add(MyItem);
                    MyItem.Selected = false;
                  
                }
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
                                MyItem.Value =Convert.ToString( MySite.ID);
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
                                ddlDevice.Items.Add(MyItem);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                try
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.SortDropDown(ddlDevice);

                }
                catch (Exception ex)
                {
                }


            }
            catch (Exception ex)
            {
            }


        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            LiveMonitoring.SimpleEnc MyEnc = new LiveMonitoring.SimpleEnc();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply a Caption.";
                return;
            }
            if (string.IsNullOrEmpty(this.txtPort.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply a port.";
                return;
            }
            LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
            switch (MyEnum)
            {
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
                    if (string.IsNullOrEmpty(this.txtExtraData1.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please supply a Database Name.";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please supply an Username.";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData3.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please supply a Password.";
                        return;
                    }
                    break;
            }
            LiveMonitoring.IRemoteLib.IPDevicesDetails NewIPDevice = new LiveMonitoring.IRemoteLib.IPDevicesDetails();
            NewIPDevice.Type = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
            NewIPDevice.Caption = this.txtCaption.Text;
            if (txtExtraData1.TextMode == TextBoxMode.Password)
                {
                NewIPDevice.Data1 = MyEnc.EncryptString(this.txtExtraData1.Text);
            }
            else
            {
                NewIPDevice.Data1 = this.txtExtraData1.Text;
            }
            if (txtExtraData2.TextMode == TextBoxMode.Password)
            {
                NewIPDevice.Data2 = MyEnc.EncryptString(this.txtExtraData1.Text);
            }
            else
            {
                NewIPDevice.Data2 = this.txtExtraData2.Text;
            }
            if(txtExtraData3.TextMode == TextBoxMode.Password)
            {
                NewIPDevice.Data3 = MyEnc.EncryptString(this.txtExtraData3.Text);
            }
            else
            {
                NewIPDevice.Data3 = this.txtExtraData2.Text;
            }


            if (string.IsNullOrEmpty(this.txtIPaddress.Text))
            {
                NewIPDevice.IPAdress = this.txtIPaddress.Text.ToString();
            }
            else
            {
                NewIPDevice.IPAdress = this.txtIPaddress.Text;
            }
            NewIPDevice.Port = Convert.ToInt32(txtPort.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();

            byte[] byteNormal = null;
            byte[] byteError = null;
            byte[] byteNoresponse = null;

            System.Drawing.Image imgNormal = null;
            System.Drawing.Image imgError = null;
            System.Drawing.Image imgNoresponse = null;

            //NORMAL IMAGE
            if (fuImageNormal.FileName.Trim().Length == 0)
            {
                byteNormal = pstrNormal;
            }
            else
            {
                imgNormal = Myfunc.Strip_Image(this.fuImageNormal);
                byteNormal = MyRem.ImagetoByte(imgNormal, ImageFormat.Bmp);
            }

            //NO RESPONSE IMAGE
            if (fuNoResponse.FileName.Trim().Length == 0)
            {
                byteNoresponse = pstrNoresponse;
            }
            else
            {
                imgNoresponse = Myfunc.Strip_Image(this.fuNoResponse);
                byteNoresponse = MyRem.ImagetoByte(imgNoresponse, ImageFormat.Bmp);
            }

            //ERROR MAGE
            if (fuImageError.FileName.Trim().Length == 0)
            {
                byteError = pstrError;
            }
            else
            {
                imgError = Myfunc.Strip_Image(this.fuImageError);
                byteError = MyRem.ImagetoByte(imgError, ImageFormat.Bmp);
            }
            NewIPDevice.ImageNormal = imgNormal;
            NewIPDevice.ImageNormalByte = byteNormal;
            NewIPDevice.ImageNoResponse = imgNoresponse;
            NewIPDevice.ImageNoResponseByte = byteNoresponse;
            NewIPDevice.ImageError = imgError;
            NewIPDevice.ImageErrorByte = byteError;
            try
            {
                //If IsNothing(Session("SelectedSite")) = False Then
                NewIPDevice.Add2Site = Convert.ToInt32(ddlDeviceLocation.SelectedValue);
                //End If

            }
            catch (Exception ex)
            {
            }
            int Myresp = MyRem.LiveMonServer.AddNewIPDevice(NewIPDevice);
            if (Myresp > 0)
            {
                //save fields
                //whoopeee
                successMessage.Visible = true;
                lblSuccess.Text = "IPDevice Successfully added!";
                try
                {

                    MyRem.WriteLog("Add IPDevice Succeed", "User.:" + MyUser.ID.ToString()+ "|" + Myresp.ToString());

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if ((ddlDeviceLocation.SelectedValue == null) == false)
                    {
                        MyRem.LiveMonServer.AddEditLocationLink(Myresp, Convert.ToInt32(ddlDeviceLocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.IPdevice, -99);
                    }

                }
                catch (Exception ex)
                {
                }
                ClearVals();
            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again!";
                try
                {
                    MyRem.WriteLog("Add IPDevice Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtCaption.Text);

                }
                catch (Exception ex)
                {
                }
            }
        }
        public void ClearVals()
        {
            int value = 0;
            lblError.Visible = false;
            txtIPaddress.Text = "";
            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraData3.Text = "";
            txtCaption.Text = "";
            txtPort.Text=value.ToString();
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
        }

        //protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt16(this.ddltype.SelectedValue);
        //    txtExtraData1.PasswordMode = false;
        //    txtExtraData2.PasswordMode = false;
        //    int zero = 0;
        //    this.txtPort.Text = zero.ToString();
        //    txtExtraData3.PasswordMode = True
        //    switch (MyEnum)
        //    {
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebServer:

        //            lblExtraData1.Text = "UserName";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            txtPort.Text = Convert.ToString(80);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
        //            lblExtraData1.Text = "Database Con String";
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
        //            lblExtraData1.Text = "Mail Send To";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            txtPort.Text = Convert.ToString(25);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
        //            lblExtraData1.Text = "Not Used";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = Convert.ToString(110);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
        //            lblExtraData1.Text = "Extra Data";
        //            lblExtraData2.Text = "Extra Data1";
        //            lblExtraData3.Text = "Extra Data2";
        //            this.txtPort.Text = Convert.ToString(8);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            txtExtraData1.PasswordMode = true;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = Convert.ToString(0);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitWMIServer:
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";

        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = Convert.ToString(0);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareWMIServer:
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData3.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = Convert.ToString(0);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleWMIServer:
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = Convert.ToString(0);
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitSQLServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            txtExtraData2.PasswordMode = true;
        //            lblExtraData2.Text = "Password";
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareSQLServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;

        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleDBServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebService:
        //            lblExtraData1.Text = "ServiceName";
        //            lblExtraData2.Text = "MethodName";
        //            this.txtPort.Text = Convert.ToString(80);
        //            break;
        //        default:
        //            lblExtraData1.Text = "Extra Data";
        //            lblExtraData2.Text = "Extra Data1";
        //            lblExtraData3.Text = "Extra Data2";
        //            break;
        //    }
        //    AdroitWMIServer = 14
        //    AdroitSQLServer = 15
        //    WonderWareWMIServer = 16
        //    WonderWareSQLServer = 17
        //    OracleWMIServer = 18
        //    OracleDBServer = 19
        //}



        //protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
        //   // txtExtraData2.TextMode = TextBoxMode.Password(false);
        //   // txtExtraData3.PasswordMode = false;
        //    int zero = 0;
        //    this.txtPort.Text = zero.ToString();
        //    txtExtraData3.TextMode = TextBoxMode.Password;
        //    switch (MyEnum)
        //    {
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebServer:
        //            int value1 = 80;
        //            lblExtraData1.Text = "UserName";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = value1.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
        //            lblExtraData1.Text = "Database Con String";
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
        //            int value2 = 25;
        //            lblExtraData1.Text = "Mail Send To";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = value2.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
        //            int value3 = 110;
        //            lblExtraData1.Text = "Not Used";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = value3.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
        //            int value4 = 8;
        //            lblExtraData1.Text = "Extra Data";
        //            lblExtraData2.Text = "Extra Data2";
        //            lblExtraData3.Text = "Extra Data3";
        //            this.txtPort.Text = value4.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
        //            int value5 = 0;
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData3.TextMode = TextBoxMode.Password;
                   
        //            lblExtraData3.Text = "Extra Data3";
        //            this.txtPort.Text = value5.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitWMIServer:
        //            int value6 = 0;
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            lblExtraData3.Text = "Extra Data3";
        //            this.txtPort.Text = value6.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareWMIServer:
        //            int value7 = 0;
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = value7.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleWMIServer:
        //            int value8 = 0;
        //            lblExtraData1.Text = "WMI Username";
        //            lblExtraData2.Text = "WMI Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            lblExtraData3.Text = "Extra Data3";
        //            this.txtPort.Text = value8.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitSQLServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            txtExtraData3.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Password";
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareSQLServer:
        //            lblExtraData1.Text = "Database Name";
        //            lblExtraData2.Text = "Username";
        //            lblExtraData3.Text = "Password";
        //            txtExtraData3.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleDBServer:
        //            // lblExtraData.Text = "Database Name"
        //            lblExtraData1.Text = "Username";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebService:
        //            int value9 = 80;
        //            lblExtraData1.Text = "ServiceName";
        //            lblExtraData2.Text = "MethodName";
        //            this.txtPort.Text = value9.ToString();
        //            break;
        //        default:
        //            lblExtraData1.Text = "Extra Data 1";
        //            lblExtraData2.Text = "Extra Data 2";
        //            lblExtraData3.Text = "Extra Data 3";
        //            break;
        //    }
        //    //AdroitWMIServer = 14
        //    //AdroitSQLServer = 15
        //    //WonderWareWMIServer = 16
        //    //WonderWareSQLServer = 17
        //    //OracleWMIServer = 18
        //    //OracleDBServer = 19
        //}

      
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
        public void Add_IPDevices()
        {
            Load += Page_Load;
        }

        protected void BtnClear_Click(object sender, EventArgs e)
            {

                lblError.Visible = false;
                txtIPaddress.Text = "";
                txtExtraData1.Text = "";
                txtExtraData2.Text = "";
                txtExtraData3.Text = "";
                txtCaption.Text = "";
                txtPort.Text ="";
                ddltype.SelectedIndex=-1;
                ddlDeviceLocation.SelectedIndex = -1;
                ddlDevice.SelectedIndex = -1;
                //this.txtIP1.Value = 0;

               //this.txtIP2.Value = 0;
               //this.txtIP3.Value = 0;
              //this.txtIP4.Value = 0;
       
            }

        protected void ddltype_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
            // txtExtraData2.TextMode = TextBoxMode.Password(false);
            // txtExtraData3.PasswordMode = false;
            int zero = 0;
            this.txtPort.Text = zero.ToString();
            txtExtraData3.TextMode = TextBoxMode.Password;
            switch (MyEnum)
            {
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebServer:
                    int value1 = 80;
                    lblExtraData1.Text = "UserName";
                    lblExtraData2.Text = "Password";
                    txtExtraData1.TextMode = TextBoxMode.Password;
                    this.txtPort.Text = value1.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
                    lblExtraData1.Text = "Database Con String";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
                    int value2 = 25;
                    lblExtraData1.Text = "Mail Send To";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    this.txtPort.Text = value2.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
                    int value3 = 110;
                    lblExtraData1.Text = "Not Used";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    this.txtPort.Text = value3.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
                    int value4 = 8;
                    lblExtraData1.Text = "Extra Data";
                    lblExtraData2.Text = "Extra Data2";
                    lblExtraData3.Text = "Extra Data3";
                    this.txtPort.Text = value4.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
                    int value5 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData3.TextMode = TextBoxMode.Password;

                    lblExtraData3.Text = "Extra Data3";
                    this.txtPort.Text = value5.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitWMIServer:
                    int value6 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData3.Text = "Extra Data3";
                    this.txtPort.Text = value6.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareWMIServer:
                    int value7 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData2.Text = "Extra Data2";
                    this.txtPort.Text = value7.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleWMIServer:
                    int value8 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData3.Text = "Extra Data3";
                    this.txtPort.Text = value8.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitSQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    lblExtraData2.Text = "Password";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareSQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleDBServer:
                    // lblExtraData.Text = "Database Name"
                    lblExtraData1.Text = "Username";
                    lblExtraData2.Text = "Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebService:
                    int value9 = 80;
                    lblExtraData1.Text = "ServiceName";
                    lblExtraData2.Text = "MethodName";
                    this.txtPort.Text = value9.ToString();
                    break;
                default:
                    lblExtraData1.Text = "Extra Data 1";
                    lblExtraData2.Text = "Extra Data 2";
                    lblExtraData3.Text = "Extra Data 3";
                    break;
            }
            //AdroitWMIServer = 14
            //AdroitSQLServer = 15
            //WonderWareWMIServer = 16
            //WonderWareSQLServer = 17
            //OracleWMIServer = 18
            //OracleDBServer = 19
        }
    }
    }