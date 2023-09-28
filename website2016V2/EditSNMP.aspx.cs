using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class EditSNMP : System.Web.UI.Page
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
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
                if (Page.IsPostBack == false)
                {

                    LoadPage();
                    LoadSites();

                }
                else
                {
                    //refresh the main page
                    StringBuilder TheScript = new StringBuilder();
                    // Holds the injected script.

                    // Create the script.
                    TheScript.Append("<script type='text/javascript'>" + Constants.vbCrLf);
                    TheScript.Append("parent.RefreshChildFrame(1);");
                    TheScript.Append(Constants.vbCrLf);
                    TheScript.Append("</script>");

                    this.ClientScript.RegisterStartupScript(typeof(string), "RefreshMain", TheScript.ToString());
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

            
        }

        protected void cmbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
             try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if ((cmbLocations.SelectedValue == null) == false)
                {
                    Convert.ToInt32(MyRem.LiveMonServer.AddEditLocationLink(Convert.ToInt32(cmbDevices.SelectedValue), Convert.ToInt32(cmbLocations.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.SNMP_Device, 99));
                }

            }
            catch (Exception ex)
            {
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

        protected void btnChangeSite_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if (Convert.ToInt32(Session["SelectedSite"]) != Convert.ToInt32(cmbSites.SelectedValue))
                {
                    int Myresp = Convert.ToInt32(MyRem.LiveMonServer.EditSensorDeviceSite(Convert.ToInt32(cmbDevices.SelectedValue), Convert.ToInt32(cmbSites.SelectedValue), LiveMonitoring.IRemoteLib.SDUpdateType.SNMPDevice));
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
                            if (MySite.ID > -1)
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
                                cmbSites.Items.Add(MyItem);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                try
                {
                    SortDropDown(cmbSites);

                }
                catch (Exception ex)
                {
                }


            }
            catch (Exception ex)
            {
            }

        }

        public void LoadPage()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.LoadPage(cmbSNMPVersion, cmbAuthentication);
            LoadDevices();
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
            // Dim AddSens As Boolean = False
            cmbDevices.Items.Clear();
            cmbLocations.Items.Clear();
            try
            {
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            LiveMonitoring.IRemoteLib.SNMPManagerDetails Mysensor = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                            bool AddSens = true;
                            if ((Session["SearchDevice"] == null) == false)
                            {
                                if (Mysensor.Caption.ToUpper().Contains(Convert.ToString(Session["SearchDevice"]).ToUpper()) == false)
                                {
                                    AddSens = false;
                                }
                            }
                            if (AddSens)
                            {
                                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                MyItem.Text = Mysensor.Caption;
                                MyItem.Value = Mysensor.ID.ToString();
                                MyItem.Selected = false;
                                cmbDevices.Items.Add(MyItem);
                                if (added == false)
                                {
                                    MyItem.Selected = true;
                                    added = true;
                                    LoadSpecificDevice(Mysensor.ID);
                                    this.TblDet.Visible = true;
                                }
                                else
                                {
                                    MyItem.Selected = false;
                                }
                            }

                            //Dim myrow As New UltraGridRow(True)
                            //myrow.Cells.Add()
                            //myrow.Cells(0).Value = Mysensor.Caption
                            //myrow.Cells.Add()
                            //myrow.Cells(1).Value = "" '"<img src=ReturnnormalImage.aspx?Device=" + Mysensor.ID.ToString + ">"
                            //myrow.Tag = Mysensor.ID
                            //cmbDevices.Rows.Add(myrow)
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
                        {
                            LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
                           System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
                            MyItem.Value = MyLocation.Id.ToString();
                            MyItem.Selected = false;
                            cmbLocations.Items.Add(MyItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }


            try
            {
                SortDropDown(cmbDevices);

            }
            catch (Exception ex)
            {
            }
        }

        protected void cmdSave_Click(object sender, System.EventArgs e)
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
            if (this.txtPassword.Text != this.txtPassword1.Text)
            {
                warningMessage.Visible = false;
                lblWarning.Text = "Password is different, Please retype.";
                cmbDevices.Focus();
                return;
            }
            LiveMonitoring.IRemoteLib.SNMPManagerDetails NewSNMPDevice = new LiveMonitoring.IRemoteLib.SNMPManagerDetails();
            LiveMonitoring.IRemoteLib.SNMPManagerDetails CurDevice = new LiveMonitoring.IRemoteLib.SNMPManagerDetails();
            try
            {
                CurDevice = ReturnSpecificDevice(Convert.ToInt32(cmbDevices.SelectedValue));

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Cannot find current sensor.";
                cmbDevices.Focus();

                return;
                //error cannot find current sensor
            }

            if ((CurDevice == null) == true)
            {
                errorMessage.Visible = true;
                lblError.Text = "Cannot find current sensor.";
                cmbDevices.Focus();

                return;
                //error cannot find current sensor
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

            if (!Regex.IsMatch(txtPort.Text,
                     @"(^([0-9]*|\d*\d{1}?\d*)$)"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Local Port must be a number";
                txtPort.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;
            }




            if (Regex.IsMatch(txtIpAddress.Text, @"\b\d{ 1,3}\.\d{ 1,3}\.\d{ 1,3}\.\d{ 1,3}\b"))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Invalid IP Address";
                txtIpAddress.Focus();
                return;
            }
            else
            {
                warningMessage.Visible = false;

            }


            NewSNMPDevice = CurDevice;
            NewSNMPDevice.AuthenticationProtocol = (LiveMonitoring.IRemoteLib.SNMPManagerDetails.AuthProtocol)Convert.ToInt32(cmbAuthentication.SelectedValue);
            NewSNMPDevice.Caption = this.txtCaption.Text;
            NewSNMPDevice.Community = this.txtCommunity.Text;

            NewSNMPDevice.Data1 = this.txtData1.Text;

            NewSNMPDevice.Data2 = this.txtData2.Text;
            NewSNMPDevice.Data3 = this.txtData3.Text;
            NewSNMPDevice.LocalEngineId =Convert.ToInt32( txtLocalEngineId.Text);
            NewSNMPDevice.LocalHost = txtIpAddress.Text;
            NewSNMPDevice.LocalPort =Convert.ToInt32(txtPort.Value);
            LiveMonitoring.SimpleEnc MyEnc = new LiveMonitoring.SimpleEnc();
            if (!string.IsNullOrEmpty(this.txtPassword.Text))
            {
                NewSNMPDevice.Password = MyEnc.EncryptString(this.txtPassword.Text);
            }
            else
            {
                NewSNMPDevice.Password = "";
            }
            NewSNMPDevice.RemoteHost = this.txtRemoteHost.Text;
            NewSNMPDevice.RemotePort = Convert.ToInt32( txtRemotePort.Value);
            NewSNMPDevice.RequestId =Convert.ToInt32( txtRequestID.Text);
            NewSNMPDevice.SNMPVersion = (LiveMonitoring.IRemoteLib.SNMPManagerDetails.SNMPVer)Convert.ToInt32(cmbSNMPVersion.SelectedValue);
            NewSNMPDevice.Timeout = Convert.ToInt32(txtTimeout.Value);
            NewSNMPDevice.User = this.txtUsername.Text;
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
            if (this.filImageNormal.HasFile)
            {
                NewSNMPDevice.ImageNormal = Myfunc.Strip_Image(this.filImageNormal);
                NewSNMPDevice.ImageNormalByte = MyRem.ImagetoByte(NewSNMPDevice.ImageNormal, ImageFormat.Bmp);
            }
            if (this.filImageNoResponse.HasFile)
            {
                NewSNMPDevice.ImageNoResponse = Myfunc.Strip_Image(this.filImageNoResponse);
                NewSNMPDevice.ImageNoResponseByte = MyRem.ImagetoByte(NewSNMPDevice.ImageNoResponse, ImageFormat.Bmp);
            }
            if (this.filImageError.HasFile)
            {
                NewSNMPDevice.ImageError = Myfunc.Strip_Image(this.filImageError);
                NewSNMPDevice.ImageErrorByte = MyRem.ImagetoByte(NewSNMPDevice.ImageError, ImageFormat.Bmp);
            }
            try
            {
                if ((Session["SelectedSite"] == null) == false)
                {
                    NewSNMPDevice.Add2Site = Convert.ToInt32(Session["SelectedSite"]);
                }

            }
            catch (Exception ex)
            {
            }
            bool Myresp = MyRem.LiveMonServer.EditSNMPDevice(NewSNMPDevice);
            if (Myresp)
            {
                //save fields
                //whoopeee
                successMessage.Visible = true;
                lblSucces.Text = "SNMP Updated";
                ClearVals();

            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again.";
                cmbDevices.Focus();
            }

        }

        public void ClearVals()
        {
            this.txtData1.Text = "";
            this.txtData2.Text = "";
            this.txtData3.Text = "";
            this.txtCaption.Text = "";
            this.txtPassword.Text = "";
            this.txtRemoteHost.Text = "";
            this.txtRequestID.Text = "";
            this.txtTimeout.Text = "";
            this.txtUsername.Text = "";
            this.txtCommunity.Text = "";
            this.txtPort.Value = 0;
            this.txtIpAddress.Text = "";
            lblErr.Visible = false;
        }



        public LiveMonitoring.IRemoteLib.SNMPManagerDetails ReturnSpecificDevice(int ID)
        {
            LiveMonitoring.IRemoteLib.SNMPManagerDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
            functionReturnValue = null;
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails Mysensor = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                    if (ID == Mysensor.ID)
                    {
                        return Mysensor;
                    }
                }
            }
            return functionReturnValue;
        }


        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetDeleteLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedDelete.aspx");
            }
            LiveMonitoring.IRemoteLib.SNMPManagerDetails Cursensor = new LiveMonitoring.IRemoteLib.SNMPManagerDetails();
            try
            {
                Cursensor = ReturnSpecificDevice(Convert.ToInt32(cmbDevices.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during Delete, Please try again.";
                cmbDevices.Focus();
                return;
                //error cannot find current sensor
            }
            if ((Cursensor == null) == false)
            {
                if (CanDelete(Cursensor.ID))
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

                    if (MyRem.LiveMonServer.DeleteSNMPDevice(Cursensor.ID))
                    {
                        //what now refesh
                        try
                        {
                            successMessage.Visible = true;
                            lblSucces.Text = "DeleteSNMPDevice Succeeded.";
                            cmbDevices.Focus();
                            MyRem.WriteLog("DeleteSNMPDevice Succeeded", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ID.ToString());

                        }
                        catch (Exception ex)
                        {
                        }
                        LoadPage();
                    }
                    else
                    {
                        try
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "DeleteSNMPDevice Failed.";
                            cmbDevices.Focus();
                            MyRem.WriteLog("DeleteSNMPDevice Failed", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ID.ToString());

                        }
                        catch (Exception ex)
                        {
                        }
                        errorMessage.Visible = true;
                        lblError.Text = "An error has occured during Delete, Please try again.";
                        cmbDevices.Focus();
                    }
                }

            }
        }


        public bool CanDelete(int DeviceID)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;
                if ((MyCollection == null))
                    return true;
                bool Refrenced = false;
                string RefrencedName = "";

                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            try
                            {
                                if (DeviceID == Mysensor.IPDeviceID)
                                {
                                    Refrenced = true;
                                    RefrencedName += Mysensor.Caption + ",";
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.CameraDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.CameraDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.CameraDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.IPDevicesDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.IPDevicesDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.IPDevicesDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.OtherDevicesDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.OtherDevicesDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SNMPManagerDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.SNMPManagerDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        //End If
                        //cmbSensGroup
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorGroup Then
                        // Dim MysensorGroup As LiveMonitoring.IRemoteLib.SensorGroup = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorGroup)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.LocationDetails Then
                        // Dim MyLocation As LiveMonitoring.IRemoteLib.LocationDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.LocationDetails)
                        //End If



                    }
                    catch (Exception ex)
                    {
                    }

                }
                if (Refrenced)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Cannot delete ,Device is in use for " + RefrencedName;
                    cmbDevices.Focus();

                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return true;
        }



        public void LoadSpecificDevice(int ID)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails MyDevice = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                    if (ID == MyDevice.ID)
                    {
                        //if (MyDevice.AuthenticationProtocol > 0)
                        //{
                        //    this.cmbAuthentication.SelectedValue = MyDevice.AuthenticationProtocol.ToString();
                        //}
                        //else
                        //{
                        //    cmbAuthentication.SelectedValue = "1";
                        //}
                        this.txtCaption.Text = MyDevice.Caption;
                        this.txtCommunity.Text = MyDevice.Community;
                        this.txtData1.Text = MyDevice.Data1;
                        this.txtData2.Text = MyDevice.Data2;
                        this.txtData3.Text = MyDevice.Data3;
                        this.txtLocalEngineId.Text = MyDevice.LocalEngineId.ToString();
                        this.txtIpAddress.Text = MyDevice.LocalHost;


                        this.txtPort.Value = MyDevice.LocalPort;
                        this.txtPassword.Text = MyDevice.Password;
                        this.txtRemoteHost.Text = MyDevice.RemoteHost;
                        this.txtRemotePort.Value = MyDevice.RemotePort;
                        this.txtRequestID.Text = MyDevice.RequestId.ToString();
                        //this.cmbSNMPVersion.SelectedIndex = Convert.ToInt32(MyDevice.SNMPVersion);
                        //this.cmbSNMPVersion.SelectedValue = MyDevice.SNMPVersion.ToString();
                        this.txtTimeout.Value = MyDevice.Timeout;
                        this.txtUsername.Text = MyDevice.User;

                        this.imgError.ImageUrl = "ReturnErrorImage.aspx?Device=" + MyDevice.ID.ToString();
                        this.imgResponse.ImageUrl = "ReturnNoResponseImage.aspx?Device=" + MyDevice.ID.ToString();
                        this.imgNormal.ImageUrl = "ReturnNormalImage.aspx?Device=" + MyDevice.ID.ToString();
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }


        protected void cmbDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.TblDet.Visible = true;
            //find details
            LoadSpecificDevice(Convert.ToInt32(cmbDevices.SelectedValue));
        }

        protected void btnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if ((cmbLocations.SelectedValue == null) == false)
                {
                    MyRem.LiveMonServer.AddEditLocationLink(Convert.ToInt32(cmbDevices.SelectedValue), Convert.ToInt32(cmbLocations.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.Camera, 99);
                    
                }

            }
            catch (Exception ex)
            {
            }
        }


        //protected void btnSearchDevice_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["SearchDevice"] = txtDeviceName.Text;
        //        LoadDevices();

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        public EditSNMP()
        {
            Load += Page_Load;
        }

       
    }
}