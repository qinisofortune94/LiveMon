using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace website2016V2
{
    public partial class IPDeviceTemplate : System.Web.UI.Page
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

                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);

                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }

                try
                {
                   LoadDefaultImages();
                //    LoadDevices();
                //    LoadSites();

                }
               catch (Exception ex)
                {
                }
                if (IsPostBack == false)
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.ipDevice(ddltype);
                    // LoadPeople();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

            try {
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel) {
                    Response.Redirect("NotAuthorisedEdit.aspx");
                }
                if (string.IsNullOrEmpty(this.txtCaption.Text)) {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please supply a Caption!";
                    return;
                }
                if (string.IsNullOrEmpty(this.txtPort.Text)) {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please supply a port!";
                    return;
                }
                LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
                switch (MyEnum) {
                    case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
                        if (string.IsNullOrEmpty(this.txtExtraData1.Text)) {
                            warningMessage.Visible = true;
                            lblWarning.Text = "Please supply a Database Name!";
                            return;
                        }
                        if (string.IsNullOrEmpty(this.txtExtraData2.Text)) {
                            warningMessage.Visible = true;
                            lblWarning.Text = "Please supply an Username!";
                            return;
                        }
                        if (string.IsNullOrEmpty(this.txtExtraData2.Text)) {
                            warningMessage.Visible = true;
                            lblWarning.Text = "Please supply a Password!";
                            return;
                        }
                        break;
                }
                LiveMonitoring.IRemoteLib.IPDeviceTemplate NewIPDevice = new LiveMonitoring.IRemoteLib.IPDeviceTemplate();
                //NewIPDevice.templateName = Me.txtTemplateName.Text
                NewIPDevice.Type = (LiveMonitoring.IRemoteLib.IPDeviceTemplate.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
                NewIPDevice.Caption = this.txtCaption.Text;
                NewIPDevice.Data1 = this.txtExtraData1.Text;
                NewIPDevice.Data2 = this.txtExtraData2.Text;
                NewIPDevice.Data3 = this.txtExtraData3.Text;
         
                if (string.IsNullOrEmpty(this.txtIPaddress.Text)) {
                   // NewIPDevice.IPAdress = this.txtIP1.Value.ToString + "." + this.txtIP2.Value.ToString + "." + this.txtIP3.Value.ToString + "." + this.txtIP4.Value.ToString;
                } else {
                    NewIPDevice.IPAdress = this.txtIPaddress.Text;
                }
                NewIPDevice.Port = Convert.ToInt32(this.txtPort.Text);
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
                NewIPDevice.ImageNormal = Myfunc.Strip_Image(this.fuImageNormal);
                NewIPDevice.ImageNormalByte = MyRem.ImagetoByte(NewIPDevice.ImageNormal, ImageFormat.Bmp);
                NewIPDevice.ImageNoResponse = Myfunc.Strip_Image(this.fuNoResponse);
                NewIPDevice.ImageNoResponseByte = MyRem.ImagetoByte(NewIPDevice.ImageNoResponse, ImageFormat.Bmp);
                NewIPDevice.ImageError = Myfunc.Strip_Image(this.fuImageError);
                NewIPDevice.ImageErrorByte = MyRem.ImagetoByte(NewIPDevice.ImageError, ImageFormat.Bmp);
                NewIPDevice.templateName = txtTemplateName.Text;
                bool didSave = MyRem.LiveMonServer.AddNewIPDeviceTemplate(NewIPDevice);
                if (didSave == true) {
                    ClearVals();
                    successMessage.Visible = true;
                    lblSuccess.Text = "Template Saved Successfully.";
                    
                } else {
                    errorMessage.Visible = true;
                    lblError.Text = "Failed to save the template. Please try again.";
                    
                }
            } catch (Exception ex) {
                Trace.Write("IPDeviceTemplate.aspx:CmdSaveClick: Error: " + ex.Message);
                errorMessage.Visible = true;
                lblError.Text = "Error: " + ex.Message;
                
            }
        }

        public void ClearVals()
        {
            int zero = 0;
            lblError.Visible = false;
            this.txtIPaddress.Text = "";
            this.txtExtraData1.Text = "";
            this.txtExtraData2.Text = "";
            this.txtExtraData3.Text = "";
            this.txtCaption.Text = "";
            this.txtPort.Text = zero.ToString();
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
        }

        protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt16(this.ddltype.SelectedValue);
            switch (MyEnum) {
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebServer:
                    int webServer = 80;
                    lblExtraData1.Text = "UserName";
                    lblExtraData2.Text = "Password";
                    this.txtPort.Text = webServer.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
                    lblExtraData1.Text = "Database Con String";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
                    int smtServer = 25;
                    lblExtraData1.Text = "Mail Send To";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    this.txtPort.Text = smtServer.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
                    int popServer = 110;
                    lblExtraData1.Text = "Not Used";
                    lblExtraData2.Text = "Password";
                    this.txtPort.Text = popServer.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
                    int ICMP = 8;
                    lblExtraData1.Text = "Extra Data";
                    lblExtraData2.Text = "Extra Data1";
                    lblExtraData3.Text = "Extra Data2";
                    this.txtPort.Text = ICMP.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
                    int WMIServer = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    lblExtraData3.Text = "Extra Data2";
                    this.txtPort.Text = WMIServer.ToString();
                    break;
                default:
                    lblExtraData1.Text = "Extra Data1";
                    lblExtraData2.Text = "Extra Data2";
                    lblExtraData3.Text = "Extra Data3";
                    break;
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
        public void Add_IPDevicesTemplate()
        {
            Load += Page_Load;
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {

            int zero = 0;
            lblError.Visible = false;
            this.txtIPaddress.Text = "";
            this.txtExtraData1.Text = "";
            this.txtExtraData2.Text = "";
            this.txtExtraData3.Text = "";
            this.txtCaption.Text = "";
            this.txtPort.Text = zero.ToString();
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
        }

        protected void ddltype_SelectedIndexChanged1(object sender, EventArgs e)
        {
            LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt16(this.ddltype.SelectedValue);
            switch (MyEnum)
            {
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebServer:
                    int webServer = 80;
                    lblExtraData1.Text = "UserName";
                    lblExtraData2.Text = "Password";
                    this.txtPort.Text = webServer.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
                    lblExtraData1.Text = "Database Con String";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
                    int smtServer = 25;
                    lblExtraData1.Text = "Mail Send To";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    this.txtPort.Text = smtServer.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
                    int popServer = 110;
                    lblExtraData1.Text = "Not Used";
                    lblExtraData2.Text = "Password";
                    this.txtPort.Text = popServer.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
                    int ICMP = 8;
                    lblExtraData1.Text = "Extra Data";
                    lblExtraData2.Text = "Extra Data1";
                    lblExtraData3.Text = "Extra Data2";
                    this.txtPort.Text = ICMP.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
                    int WMIServer = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    lblExtraData3.Text = "Extra Data2";
                    this.txtPort.Text = WMIServer.ToString();
                    break;
                default:
                    lblExtraData1.Text = "Extra Data1";
                    lblExtraData2.Text = "Extra Data2";
                    lblExtraData3.Text = "Extra Data3";
                    break;
            }
        }
    }
   
}