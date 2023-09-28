using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class IPDeviceEdit : System.Web.UI.Page
    {
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
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
                    LoadDefaultImages();

                }
                catch (Exception ex)
                {
                }


                if (Page.IsPostBack == false)
                {
                    LoadPage();
                    LoadSites();
                   // LoadDevices();
                   // LiveMonitoring.testing test = new LiveMonitoring.testing();
                    //test.ipDevice(ddltype);

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
                //If IsNothing(Session("SearchDevice")) = False And txtDeviceName.Text = "" Then
                //    txtDeviceName.Text = CStr(Session("SearchDevice"))
                //End If
            }
            else
            {
                Response.Redirect("Index.aspx");
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

           // ASSIGN DEFAULT IMAGES
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
                if (Convert.ToString(Session["SelectedSite"]) != ddlDevice.SelectedValue)
                {
                    bool Myresp = MyRem.LiveMonServer.EditSensorDeviceSite(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue), Convert.ToInt32(ddlDevice.SelectedValue), LiveMonitoring.IRemoteLib.SDUpdateType.IPDevice);
                }

            }
            catch (Exception ex)
            {
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
                                MyItem.Value = Convert.ToString(MySite.ID);
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

        public void LoadPage()
        {
            try
            {
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.ipDevice(ddltype);

            }
            catch (Exception ex)
            {
            }
            //If IsPostBack = False Then
            LoadDevices();
            //End If


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
            ddlSelectedDevice.Items.Clear();
            ddlDeviceLocation.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
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
                        ddlSelectedDevice.Items.Add(MyItem);
                        if (added == false)
                        {
                            MyItem.Selected = true;
                            added = true;
                            LoadSpecificDevice(Mysensor.ID);
                            //this.TblDet.Visible = true;
                        }
                        else
                        {
                            MyItem.Selected = false;
                        }
                        //Dim myrow As New UltraGridRow(True)
                        //myrow.Cells.Add()
                        //myrow.Cells(0).Value = Mysensor.Caption
                        //'ReturnnormalImage.aspx?Sensor=" + MyObject1.SensorID.ToString + ")
                        //myrow.Cells.Add()
                        //myrow.Cells(1).Value = "" '"<img src=ReturnnormalImage.aspx?Device=" + Mysensor.ID.ToString + ">"
                        //myrow.Tag = Mysensor.ID
                        //cmbDevices.Rows.Add(myrow)
                    }

                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
                {
                    LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
                    MyItem.Value = MyLocation.Id.ToString();
                    MyItem.Selected = false;
                    ddlDeviceLocation.Items.Add(MyItem);
                }
            }
            try
            {
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                //test.ipDevice(ddltype);
                test.SortDropDown(ddlSelectedDevice);

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
            LiveMonitoring.IRemoteLib.IPDevicesDetails NewIPDevice = new LiveMonitoring.IRemoteLib.IPDevicesDetails();
            LiveMonitoring.IRemoteLib.IPDevicesDetails CurDevice = new LiveMonitoring.IRemoteLib.IPDevicesDetails();
            try
            {
                CurDevice = ReturnSpecificDevice(Convert.ToInt32(ddlSelectedDevice.SelectedValue));
            }
            catch (Exception ex)
            {
                return;
                //error cannot find current sensor
            }
            if ((CurDevice == null) == true)
            {
                return;
                //error cannot find current sensor
            }


            NewIPDevice = CurDevice;
            NewIPDevice.Type = NewIPDevice.Type = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
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
            if (txtExtraData3.TextMode == TextBoxMode.Password)
            {
                NewIPDevice.Data3 = MyEnc.EncryptString(this.txtExtraData3.Text);
            }
            else
            {
                NewIPDevice.Data3 = this.txtExtraData2.Text;
            }

            //NewIPDevice.Data1 = Me.txtExtraData.Text
            //NewIPDevice.Data2 = Me.txtExtraData1.Text
            //NewIPDevice.Data3 = Me.txtExtraData2.Text

            if (string.IsNullOrEmpty(this.txtIPaddress.Text))
            {
                NewIPDevice.IPAdress = this.txtIPaddress.Text.ToString();
            }
            else
            {
                NewIPDevice.IPAdress = this.txtIPaddress.Text;
            }


            NewIPDevice.Port = Convert.ToInt32(this.txtPort.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            if (this.fuImageNormal.HasFile)
            {
                NewIPDevice.ImageNormal = Myfunc.Strip_Image(this.fuImageNormal);
                NewIPDevice.ImageNormalByte = MyRem.ImagetoByte(NewIPDevice.ImageNormal, ImageFormat.Bmp);
            }
            if (this.fuNoResponse.HasFile)
            {
                NewIPDevice.ImageNoResponse = Myfunc.Strip_Image(this.fuNoResponse);
                NewIPDevice.ImageNoResponseByte = MyRem.ImagetoByte(NewIPDevice.ImageNoResponse, ImageFormat.Bmp);
            }
            if (this.fuImageError.HasFile)
            {
                NewIPDevice.ImageError = Myfunc.Strip_Image(this.fuImageError);
                NewIPDevice.ImageErrorByte = MyRem.ImagetoByte(NewIPDevice.ImageError, ImageFormat.Bmp);
            }
            try
            {
                if ((Session["SelectedSite"] == null) == false)
                {
                    NewIPDevice.Add2Site = Convert.ToInt32(Session["SelectedSite"]);
                }

            }
            catch (Exception ex)
            {
            }
            bool Myresp = MyRem.LiveMonServer.EditIPDevice(NewIPDevice);
            if (Myresp)
            {
                //save fields
                //whoopeee
                successMessage.Visible = true;
              lblSuccess.Text = "IP Device Successfully edited.";
                try
                {
                    MyRem.WriteLog("Edit IP Device Succeeded", "User:" + MyUser.ID.ToString() + "|" + NewIPDevice.ToString());

                }
                catch (Exception ex)
                {
                }
                ClearVals();
                //lblErr.Visible = False

            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again!";
                try
                {
                    MyRem.WriteLog("Edit IP Device Failed", "User:" + MyUser.ID.ToString() + "|" + NewIPDevice.ToString());

                }
                catch (Exception ex)
                {
                }
            }
        }
        public LiveMonitoring.IRemoteLib.IPDevicesDetails ReturnSpecificDevice(int ID)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.IRemoteLib.IPDevicesDetails returnValue = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
            returnValue = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    if (ID == Mysensor.ID)
                    {
                        return Mysensor;
                    }
                }



            }
            return returnValue;
        }
        public void ClearVals()
        {
            int zero = 0;
            txtIPaddress.Text = "";
            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraData3.Text = "";
            txtCaption.Text = "";
            txtPort.Text = zero.ToString();
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
            // lblError.Visible = False;



        }
        public void LoadSpecificDevice1(int ID)
        {

            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails MyDevice = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    if (ID == MyDevice.ID)
                    {
                        this.ddltype.SelectedIndex = Convert.ToInt32(MyDevice.Type);
                        this.ddltype.DataTextField = MyDevice.Type.ToString();
                        ddltype_SelectedIndexChanged(this, new EventArgs());
                        this.txtCaption.Text = MyDevice.Caption;
                        this.txtExtraData1.Text = MyDevice.Data1;
                        this.txtExtraData2.Text = MyDevice.Data2;
                        this.txtExtraData3.Text = MyDevice.Data3;
                        this.txtIPaddress.Text = MyDevice.IPAdress;
                        


                        txtPort.Text = Convert.ToString(MyDevice.Port);
                        //imgNormal.ImageUrl = "ReturnErrorImage.aspx?Device=" + MyDevice.ID.ToString();
                        //imgResponse.ImageUrl = "ReturnNoResponseImage.aspx?Device=" + MyDevice.ID.ToString();
                        //imgError.ImageUrl = "ReturnNormalImage.aspx?Device=" + MyDevice.ID.ToString();
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
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
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails MyDevice = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    if (ID == MyDevice.ID)
                    {
                        this.ddltype.SelectedIndex = Convert.ToInt32(MyDevice.Type);
                        this.ddltype.SelectedValue = ((int)MyDevice.Type).ToString();
                        ddltype_SelectedIndexChanged(this, new EventArgs());
                        this.txtCaption.Text = MyDevice.Caption;
                        this.txtExtraData1.Text = MyDevice.Data1;
                        this.txtExtraData2.Text = MyDevice.Data2;
                        this.txtExtraData3.Text = MyDevice.Data3;
                        this.txtIPaddress.Text = MyDevice.IPAdress;

                        txtPort.Text = Convert.ToString(MyDevice.Port);
                        //this.ImageError.ImageUrl = "ReturnErrorImage.aspx?Device=" + MyDevice.ID.ToString;
                        //this.ImageNoResponse.ImageUrl = "ReturnNoResponseImage.aspx?Device=" + MyDevice.ID.ToString;
                        //this.ImageNormal.ImageUrl = "ReturnNormalImage.aspx?Device=" + MyDevice.ID.ToString;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }

        protected void cmbDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadSpecificDevice(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue));
        }


        protected void btnDelete_Click(object sender, System.EventArgs e)
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
            LiveMonitoring.IRemoteLib.IPDevicesDetails Cursensor = new LiveMonitoring.IRemoteLib.IPDevicesDetails();
            try
            {
                Cursensor = ReturnSpecificDevice(Convert.ToInt32(ddlSelectedDevice.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during Delete, Please try again!";
                return;
                //error cannot find current sensor
            }
            if ((Cursensor == null) == false)
            {
                if (CanDelete(Cursensor.ID))
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    if (MyRem.LiveMonServer.DeleteIPDevice(Cursensor.ID))
                    {
                        //what now refesh
                        try
                        {
                            MyRem.WriteLog("DeleteIPDevice Succeeded", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());

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
                            MyRem.WriteLog("DeleteIPDevice Failed", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());

                        }
                        catch (Exception ex)
                        {
                        }
                        errorMessage.Visible = true;
                        lblError.Text = "An error has occured during Delete, Please try again!";
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
                        //    Dim Mysensor As LiveMonitoring.IRemoteLib.CameraDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.CameraDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.IPDevicesDetails Then
                        //    Dim Mysensor As LiveMonitoring.IRemoteLib.IPDevicesDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.IPDevicesDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.OtherDevicesDetails Then
                        //    Dim Mysensor As LiveMonitoring.IRemoteLib.OtherDevicesDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SNMPManagerDetails Then
                        //    Dim Mysensor As LiveMonitoring.IRemoteLib.SNMPManagerDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        //End If
                        //cmbSensGroup
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorGroup Then
                        //    Dim MysensorGroup As LiveMonitoring.IRemoteLib.SensorGroup = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorGroup)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.LocationDetails Then
                        //    Dim MyLocation As LiveMonitoring.IRemoteLib.LocationDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.LocationDetails)
                        //End If



                    }
                    catch (Exception ex)
                    {
                    }

                }
                if (Refrenced)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Cannot delete ,Device is in use for " + RefrencedName;
                    //txtEditGroup.Focus()
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

        protected void cmdConfigure_Click(object sender, System.EventArgs e)
        {
            //Response.Redirect("Http://" + txtIP1.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text)
            if (string.IsNullOrEmpty(this.txtIPaddress.Text))
            {
                // Response.Redirect("proxy.aspx?url=" + txtIP1.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text);
                Response.Redirect("proxy.aspx?url=" + txtIPaddress.Text);
            }
        }


        protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(ddltype.SelectedValue);
            // txtExtraData1.PasswordMode = false;
            //txtExtraData2.PasswordMode = false;
            int item0 = 0;
            this.txtPort.Text = item0.ToString();
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
                    int value = 80;
                    lblExtraData1.Text = "UserName";
                    lblExtraData2.Text = "Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    this.txtPort.Text = value.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
                    lblExtraData1.Text = "Database Con String";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
                    int value1 = 25;
                    lblExtraData1.Text = "Mail Send To";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    this.txtPort.Text = value1.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
                    int value2 = 110;
                    lblExtraData1.Text = "Not Used";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    this.txtPort.Text = value2.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
                    int value3 = 8;
                    lblExtraData1.Text = "Extra Data 1";
                    lblExtraData2.Text = "Extra Data 2";
                    lblExtraData3.Text = "Extra Data 3";
                    this.txtPort.Text = value3.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
                    int value4 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData3.Text = "Extra Data3";
                    this.txtPort.Text = value4.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitWMIServer:
                    int value5 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData2.Text = "Extra Data2";
                    this.txtPort.Text = value5.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareWMIServer:
                    int value6 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData2.Text = "Extra Data2";
                    this.txtPort.Text = value6.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleWMIServer:
                    int value7 = 0;
                    lblExtraData1.Text = "WMI Username";
                    lblExtraData2.Text = "WMI Password";
                    txtExtraData2.TextMode = TextBoxMode.Password;
                    lblExtraData3.Text = "Extra Data3";
                    this.txtPort.Text = value7.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitSQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    lblExtraData3.Text = "Password";
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareSQLServer:
                    lblExtraData1.Text = "Database Name";
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleDBServer:
                    // lblExtraData.Text = "Database Name"
                    lblExtraData2.Text = "Username";
                    lblExtraData3.Text = "Password";
                    txtExtraData3.TextMode = TextBoxMode.Password;
                    break;
                case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebService:
                    int value8 = 0;
                    lblExtraData1.Text = "ServiceName";
                    lblExtraData2.Text = "MethodName";
                    this.txtPort.Text = value8.ToString();
                    break;
                default:
                    lblExtraData1.Text = "Extra Data";
                    lblExtraData2.Text = "Extra Data1";
                    lblExtraData3.Text = "Extra Data2";
                    break;
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


     
        protected void btnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if ((ddlDeviceLocation.SelectedValue == null) == false)
                {
                    MyRem.LiveMonServer.AddEditLocationLink(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue), Convert.ToInt32(ddlDeviceLocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.IPdevice, 99);
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSearchDevice_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchDevice"] = txtDeviceName.Text;
                LoadDevices();

            }
            catch (Exception ex)
            {
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            int zero = 0;
            txtIPaddress.Text = "";
            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraData3.Text = "";
            txtCaption.Text = "";
            txtPort.Text = zero.ToString();
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
            // lblError.Visible = False;
        }


        protected void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if (Convert.ToInt32(Session["SelectedSite"]) != ddlDevice.SelectedIndex)
                {
                    bool Myresp = MyRem.LiveMonServer.EditSensorDeviceSite(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue), Convert.ToInt32(ddlDevice.SelectedValue), LiveMonitoring.IRemoteLib.SDUpdateType.IPDevice);
                }

            }
            catch (Exception ex)
            {
            }
        }


        protected void ddlSelectedDevice_SelectedIndexChanged1(object sender, EventArgs e)
        {
            LoadSpecificDevice(Convert.ToInt32((this.ddlSelectedDevice.SelectedValue)));
        }

       


    }
}