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
    public partial class EditOtherDevice : System.Web.UI.Page
    {

private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
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
                    //LiveMonitoring.testing test = new LiveMonitoring.testing();
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

            }
            else
            {
                Response.Redirect("Index.aspx");
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
                    //  test.ipDevice(ddlDevice);
                    //  LoadPeople();

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
            test.ipDevice(ddltype);
            ddlSerialPort.Items.Clear();
            System.Web.UI.WebControls.ListItem NewItems = new System.Web.UI.WebControls.ListItem();
            NewItems.Text = "None";
            int item = 0;
            NewItems.Text = item.ToString();
            ddlSerialPort.Items.Add(NewItems);

            int MyInt = 0;
            for (MyInt = 1; MyInt <= 20; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                NewItem.Text = "Com" + MyInt.ToString();
                NewItem.Value = MyInt.ToString();
                ddlSerialPort.Items.Add(NewItem);
            }
            ddlBaudRate.Items.Clear();
            for (MyInt = 0; MyInt <= 17; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                switch (MyInt)
                {
                    case 0:
                        int item0 = 75;
                        NewItem.Text = "75";
                        NewItem.Text = item0.ToString();
                        break;
                    case 1:
                        int item1 = 110;
                        NewItem.Text = "110";
                        NewItem.Text=item1.ToString();

                        break;
                    case 2:
                        int item2 = 134;
                        NewItem.Text = "134";
                        NewItem.Text = item2.ToString();
                        break;
                    case 3:
                        int item3 = 150;
                        NewItem.Text = "150";
                        NewItem.Text = item3.ToString();
                        break;
                    case 4:
                        int item4 = 300;
                        NewItem.Text = "300";
                        NewItem.Text = item4.ToString();
                        break;
                    case 5:
                        int item5 = 600;
                        NewItem.Text = "600";
                        NewItem.Text = item5.ToString();
                        break;
                    case 6:
                        int item6 = 1200;
                        NewItem.Text = "1200";
                        NewItem.Text = item6.ToString();
                        break;
                    case 7:
                        int item7 = 1800;
                        NewItem.Text = "1800";
                        NewItem.Text = item7.ToString();
                        break;
                    case 8:
                        int item8 = 2400;
                        NewItem.Text = "2400";
                        NewItem.Text = item8.ToString();
                        break;
                    case 9:
                        int item9 = 4800;
                        NewItem.Text = "4800";
                        NewItem.Text = item9.ToString();
                        break;
                    case 10:
                        int item10 = 7200;
                        NewItem.Text = "7200";
                        NewItem.Text = item10.ToString();
                        break;
                    case 11:
                        int item11 = 9600;
                        NewItem.Text = "9600";
                        NewItem.Text = item11.ToString();
                        NewItem.Selected = true;
                        break;
                    case 12:
                        int item12 = 14400;
                        NewItem.Text = "14400";
                        NewItem.Text = item12.ToString();
                        break;
                    case 13:
                        int item13 = 19200;
                        NewItem.Text = "19200";
                        NewItem.Text = item13.ToString();
                        break;
                    case 14:
                        int item14 = 38400;
                        NewItem.Text = "38400";
                        NewItem.Text = item14.ToString();
                        break;
                    case 15:
                        int item15 = 57600;
                        NewItem.Text = "57600";
                        NewItem.Text = item15.ToString();
                        break;
                    case 16:
                        int item16 = 115200;
                        NewItem.Text = "115200";
                        NewItem.Text = item16.ToString();
                        break;
                    case 17:
                        int item17 = 128000;
                        NewItem.Text = "128000";
                        NewItem.Text = item17.ToString();
                        break;
                }
                ddlBaudRate.Items.Add(NewItem);
            }
            ddlDataBits.Items.Clear();
            for (MyInt = 0; MyInt <= 4; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                switch (MyInt)
                {
                    case 0:
                        int item0 = 5;
                        NewItem.Text = "5";
                        NewItem.Text = item0.ToString();
                        break;
                    case 1:
                        int item1 = 6;
                        NewItem.Text = "6";
                        NewItem.Text = item1.ToString();
                        break;
                    case 2:
                        int item2 = 7;
                        NewItem.Text = "7";
                        NewItem.Text = item2.ToString();
                        break;
                    case 3:
                        int item3 = 8;
                        NewItem.Text = "8";
                        NewItem.Text = item3.ToString();
                        NewItem.Selected = true;
                        break;
                    case 4:
                        int item4 = 9;
                        NewItem.Text = "9";
                        NewItem.Text = item4.ToString();
                        break;
                }
                ddlDataBits.Items.Add(NewItem);
            }
            radStopBits.Items.Clear();
            for (MyInt = 0; MyInt <= 2; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                switch (MyInt)
                {
                    case 0:
                        int item0 = 1;
                        NewItem.Text = "1";
                        NewItem.Text = item0.ToString();
                        NewItem.Selected = true;
                        break;
                    case 1:
                        double item1 = 1.5;
                        NewItem.Text = "1.5";
                        NewItem.Value = item1.ToString();
                        break;
                    case 2:
                        int item2 = 2;
                        NewItem.Text = "2";
                        NewItem.Text = item2.ToString();
                        break;
                }
                radStopBits.Items.Add(NewItem);
            }
            radErrCheck.Items.Clear();
            for (MyInt = 0; MyInt <= 4; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                switch (MyInt)
                {
                    case 0:
                        NewItem.Text = "Even";
                        NewItem.Value = "Even";
                        break;
                    case 1:
                        NewItem.Text = "Odd";
                        NewItem.Value = "Odd";
                        break;
                    case 2:
                        NewItem.Text = "None";
                        NewItem.Value = "None";
                        NewItem.Selected = true;
                        break;
                    case 3:
                        NewItem.Text = "Mark";
                        NewItem.Value = "Mark";
                        break;
                    case 4:
                        NewItem.Text = "Space";
                        NewItem.Value = "Space";
                        break;
                }
                radErrCheck.Items.Add(NewItem);
            }
            radHandShaking.Items.Clear();
            for (MyInt = 0; MyInt <= 2; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                switch (MyInt)
                {
                    case 0:
                        NewItem.Text = "Xon/Xoff";
                        NewItem.Value = "Xon/Xoff";
                        break;
                    case 1:
                        NewItem.Text = "None";
                        NewItem.Value = "None";
                        NewItem.Selected = true;
                        break;
                    case 2:
                        NewItem.Text = "Hardware";
                        NewItem.Value = "Hardware";
                        break;
                }
                radHandShaking.Items.Add(NewItem);
            }
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
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
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
                       // this.TblDet.Visible = true;
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
        }



        protected void btnAdd_Click(object sender, System.EventArgs e)
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
            LiveMonitoring.IRemoteLib.OtherDevicesDetails NewIPDevice = new LiveMonitoring.IRemoteLib.OtherDevicesDetails();
            LiveMonitoring.IRemoteLib.OtherDevicesDetails CurDevice = new LiveMonitoring.IRemoteLib.OtherDevicesDetails();
            try
            {
                CurDevice = ReturnSpecificDevice(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Can not find current sensor.";
                return;
                //error cannot find current sensor
            }
            if ((CurDevice == null) == true)
            {
                errorMessage.Visible = true;
                lblError.Text = "Can not find current sensor.";
                return;
                //error cannot find current sensor
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs MyFunc = new LiveMonitoring.SharedFuncs();
            NewIPDevice = CurDevice;
            NewIPDevice.Type = (LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType)Convert.ToInt32( ddltype.SelectedValue);
            NewIPDevice.Caption = txtCaption.Text;
            NewIPDevice.ExtraData = this.txtExtraData.Text;
            NewIPDevice.ExtraData1 = this.txtExtraData1.Text;
            NewIPDevice.ExtraData2 = this.txtExtraData2.Text;
            NewIPDevice.ExtraData3 =Convert.ToInt32( this.txtExtraData3.Text);
            NewIPDevice.ExtraData4 = Convert.ToInt32(this.txtExtraData4.Text);
            NewIPDevice.ExtraData5 = Convert.ToInt32(this.txtExtraData5.Text);
            //   NewIPDevice.IPAdress = this.txtIP1.Value.ToString + "." + this.txtIP2.Value.ToString + "." + this.txtIP3.Value.ToString + "." + this.txtIP4.Value.ToString;
            NewIPDevice.Port = Convert.ToInt32(txtPort.Text);
       
            NewIPDevice.SerialSettings = this.ddlBaudRate.SelectedValue + "," + this.ddlDataBits.SelectedValue + "," + this.radStopBits.SelectedValue + "," + this.radHandShaking.SelectedValue;
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
                imgNormal = MyFunc.Strip_Image(this.fuImageNormal);
                byteNormal = MyRem.ImagetoByte(imgNormal, ImageFormat.Bmp);
            }

            //NO RESPONSE IMAGE
            if (fuNoResponse.FileName.Trim().Length == 0)
            {
                byteNoresponse = pstrNoresponse;
            }
            else
            {
                imgNoresponse = MyFunc.Strip_Image(this.fuNoResponse);
                byteNoresponse = MyRem.ImagetoByte(imgNoresponse, ImageFormat.Bmp);
            }

            //ERROR MAGE
            if (fuImageError.FileName.Trim().Length == 0)
            {
                byteError = pstrError;
            }
            else
            {
                imgError = MyFunc.Strip_Image(this.fuImageError);
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
                if ((Session["SelectedSite"] == null) == false)
                {
                    NewIPDevice.Add2Site = Convert.ToInt32(Session["SelectedSite"]);
                }

            }
            catch (Exception ex)
            {
            }
            bool Myresp = MyRem.LiveMonServer.EditOtherDevice(NewIPDevice);
            if (Myresp)
            {
                //save fields
                //whoopeee
                try
                {
                    successMessage.Visible = true;
                    lblSuccess.Text = "EditOtherDevice Succeeded.";
                    MyRem.WriteLog("Edit OtherDevice Succeeded", "User:" + MyUser.ID.ToString ()+ "|" + NewIPDevice.ToString());

                }
                catch (Exception ex)
                {
                }
                ClearVals();
            }
            else
            {
                try
                {
                    errorMessage.Visible = true;
                    lblError.Text = "EditOtherDevice Failed.";
                    MyRem.WriteLog("Edit Other Device Failed", "User:" + MyUser.ID.ToString() + "|" + NewIPDevice.ToString());

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again.";
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
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails MyDevice = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                    if (ID == MyDevice.ID)
                    {
                       // this.ddltype.SelectedIndex = Convert.ToInt32(MyDevice.Type);
                        ddltype.SelectedValue = ((int)MyDevice.Type).ToString();
                        ddltype_SelectedIndexChanged(this, new EventArgs());
                        this.txtCaption.Text = MyDevice.Caption;
                        this.txtExtraData.Text = MyDevice.ExtraData;
                        this.txtExtraData1.Text = MyDevice.ExtraData1;
                        this.txtExtraData2.Text = MyDevice.ExtraData2;
                        this.txtExtraData3.Text = MyDevice.ExtraData3.ToString();
                        this.txtExtraData4.Text = MyDevice.ExtraData4.ToString();
                        this.txtExtraData5.Text = MyDevice.ExtraData5.ToString();
                        if ((MyDevice.IPAdress == null) == false)
                        {
                            this.txtIPaddress.Text = MyDevice.IPAdress.ToString();
                        }
                        this.txtPort.Text = MyDevice.Port.ToString();

                        txtPort.Text = Convert.ToString(MyDevice.Port);
                        //imgNormal.ImageUrl = "ReturnErrorImage.aspx?Device=" + MyDevice.ID.ToString();
                        //imgResponse.ImageUrl = "ReturnNoResponseImage.aspx?Device=" + MyDevice.ID.ToString();
                        //imgError.ImageUrl = "ReturnNormalImage.aspx?Device=" + MyDevice.ID.ToString();
                        this.ddlSerialPort.SelectedValue = MyDevice.SerialPort.ToString();
                        if ((MyDevice.SerialSettings == null) == false)
                        {
                            string[] MySerial = MyDevice.SerialSettings.Split(',');
                            this.ddlBaudRate.SelectedValue = MySerial[0];
                            this.ddlDataBits.SelectedValue = MySerial[1];
                            this.radStopBits.SelectedValue = MySerial[2];
                            this.radHandShaking.SelectedValue = MySerial[3];
                        }
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }


    

        public LiveMonitoring.IRemoteLib.OtherDevicesDetails ReturnSpecificDevice(int ID)
        {
            LiveMonitoring.IRemoteLib.OtherDevicesDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
            functionReturnValue = null;
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                    if (ID == Mysensor.ID)
                    {
                        return Mysensor;
                    }
                }
            }
            return functionReturnValue;
        }
        public void ClearVals()
        {
            this.txtExtraData.Text = "";
            this.txtExtraData1.Text = "";
            this.txtExtraData2.Text = "";
            this.txtExtraData3.Text = "";
            this.txtExtraData4.Text = "";
            this.txtExtraData5.Text = "";
            this.txtCaption.Text = "";
            //this.txtPort.Value = 0;
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
            lblError.Visible = false;
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
            LiveMonitoring.IRemoteLib.OtherDevicesDetails Cursensor = new LiveMonitoring.IRemoteLib.OtherDevicesDetails();
            try
            {
                Cursensor = ReturnSpecificDevice(Convert.ToInt32 (this.ddlSelectedDevice.SelectedValue));
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
                    if (MyRem.LiveMonServer.DeleteOtherDevice(Cursensor.ID))
                    {
                        //what now refesh
                        try
                        {
                            successMessage.Visible = true;
                            lblSuccess.Text = "Delete Other Device Succeeded.";
                            MyRem.WriteLog("Delete Other Device Succeeded", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ID.ToString());

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
                            lblError.Text = "Delete Other Device failed.";
                            MyRem.WriteLog("Delete Other Device failed", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ID.ToString());

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
        protected void cbntConfigure_Click(object sender, System.EventArgs e)
        {
            //Response.Redirect("Http://" + txtIP1.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text)
            if (string.IsNullOrEmpty(this.txtIPaddress.Text))
            {
                Response.Redirect("proxy.aspx?url=" + txtIPaddress.Text);
            }
          
        }


        //protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType MyEnum = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(this.ddltype.SelectedValue);
        //    //txtExtraData2.PasswordMode = false;
        //    //txtExtraData2.PasswordMode = false;
        //    int item0 = 0;
        //    this.txtPort.Text = item0.ToString();
        //    txtExtraData3.TextMode = TextBoxMode.Password;
        //    switch (MyEnum)
        //    {
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SQLServer:
        //            lblExtraData.Text = "Database Name";
        //            lblExtraData1.Text = "Username";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebServer:
        //            int case1 = 80;
        //            lblExtraData.Text = "UserName";
        //            lblExtraData1.Text = "Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = case1.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.Biometric247Server:
        //            lblExtraData.Text = "Database Con String";
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.SMTPServer:
        //            int case2 = 25;
        //            lblExtraData.Text = "Mail Send To";
        //            lblExtraData1.Text = "Username";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = case2.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.POPServer:
        //            int case3 = 110;
        //            lblExtraData.Text = "Not Used";
        //            lblExtraData1.Text = "Username";
        //            lblExtraData2.Text = "ExtraDAta2";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            this.txtPort.Text = case3.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.ICMP:
        //            int case4 = 8;
        //            lblExtraData.Text = "Extra Data";
        //            lblExtraData1.Text = "Extra Data1";
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = case4.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WMIServer:
        //            int case5 = 0;
        //            lblExtraData.Text = "WMI Username";
        //            lblExtraData1.Text = "WMI Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text=case5.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitWMIServer:
        //            int case6 = 0;
        //            lblExtraData.Text = "WMI Username";
        //            lblExtraData1.Text = "WMI Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = case6.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareWMIServer:
        //            int case7 = 0;
        //            lblExtraData.Text = "WMI Username";
        //            lblExtraData1.Text = "WMI Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Extra Data2";
        //            this.txtPort.Text = case7.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleWMIServer:
        //            int case8 = 0;
        //            lblExtraData.Text = "WMI Username";
        //            lblExtraData1.Text = "WMI Password";
        //            txtExtraData1.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = case8.ToString();
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.AdroitSQLServer:
        //            lblExtraData.Text = "Database Name";
        //            lblExtraData1.Text = "Username";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            lblExtraData2.Text = "Password";
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WonderWareSQLServer:
        //            lblExtraData.Text = "Database Name";
        //            lblExtraData1.Text = "Username";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password;
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.OracleDBServer:
        //            // lblExtraData.Text = "Database Name"
        //            lblExtraData1.Text = "Username";
        //            lblExtraData2.Text = "Password";
        //            txtExtraData2.TextMode = TextBoxMode.Password; 
        //            break;
        //        case LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType.WebService:
        //            int case9 = 80;
        //            lblExtraData.Text = "ServiceName";
        //            lblExtraData1.Text = "MethodName";
        //            this.txtPort.Text = case9.ToString();


        //            break;
        //        default:
        //            lblExtraData.Text = "Extra Data";
        //            lblExtraData1.Text = "Extra Data1";
        //            lblExtraData2.Text = "Extra Data2";
        //            break;
        //    }
        //}

        protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int zero = 0;
            lblSerialPort.Text = "Serial Port";
            lblExtraData.Text = "ExtraData";
            lblExtraData1.Text = "ExtraData1";
            lblExtraData2.Text = "ExtraData2";
            lblExtraData3.Text = "ExtraData3";
            lblExtraData4.Text = "ExtraData4";
            lblExtraData5.Text = "ExtraData5";
            switch (Convert.ToInt32(ddltype.SelectedValue))
            {
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.OneWireSerial:
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItems = new System.Web.UI.WebControls.ListItem();
                    NewItems.Text = "None";
                    NewItems.Value = zero.ToString();
                    ddlSerialPort.Items.Clear();

                    int MyInt = 0;
                    for (MyInt = 1; MyInt <= 20; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case(int) LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.ModbusSerial:
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItems1 = new System.Web.UI.WebControls.ListItem();
                    NewItems1.Text = "None";
                    NewItems1.Value = zero.ToString();
                    ddlSerialPort.Items.Add(NewItems1);

                    int MyInt1 = 0;
                    for (MyInt1 = 1; MyInt1 <= 20; MyInt1++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt1.ToString();
                        NewItem.Value = MyInt1.ToString();
                       ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.OneWireUSB:
                    lblSerialPort.Text = "USB Port";
                    lblExtraData.Text = "Adapter Name";
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItems2 = new System.Web.UI.WebControls.ListItem();
                    NewItems2.Text = "None";
                    NewItems2.Value = zero.ToString();
                    ddlSerialPort.Items.Add(NewItems2);
                    int MyInt2 = 0;
                    for (MyInt2 = 1; MyInt2 <= 10; MyInt2++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "USB" + MyInt2.ToString();
                        NewItem.Value = MyInt2.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.VotronicUSB:
                    lblSerialPort.Text = "USB Port";
                    lblExtraData.Text = "Adapter Name";
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItems3 = new System.Web.UI.WebControls.ListItem();
                    NewItems3.Text = "None";
                    NewItems3.Value = zero.ToString();
                    ddlSerialPort.Items.Add(NewItems3);
                    int MyInt3 = 0;
                    for (MyInt3 = 1; MyInt3 <= 10; MyInt3++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "USB" + MyInt3.ToString();
                        NewItem.Value = MyInt3.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.GPSReciever:
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItems4 = new System.Web.UI.WebControls.ListItem();
                    NewItems4.Text = "None";
                    NewItems4.Value = zero.ToString();
                    ddlSerialPort.Items.Add(NewItems4);

                    int MyInt4 = 0;
                    for (MyInt4 = 1; MyInt4 <= 20; MyInt4++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt4.ToString();
                        NewItem.Value = MyInt4.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.AMF:
                    ddlSerialPort.Items.Clear();
                    lblExtraData.Text = "ServiceDate";
                    lblExtraData1.Text = "FillDate";
                    lblExtraData2.Text = "Consumption";
                    lblExtraData3.Text = "ServiceHours";
                    lblExtraData4.Text = "TankSize";
                    lblExtraData5.Text = "ExtraData5";
                    System.Web.UI.WebControls.ListItem NewItems5 = new System.Web.UI.WebControls.ListItem();
                    NewItems5.Text = "None";
                    NewItems5.Value = zero.ToString();
                    ddlSerialPort.Items.Add(NewItems5);

                    int MyInt5 = 0;
                    for (MyInt5 = 1; MyInt5 <= 20; MyInt5++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt5.ToString();
                        NewItem.Value = MyInt5.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
            }

        }

        protected void ddlSelectedDevice_SelectedIndexChanged1(object sender, System.EventArgs e)
        {
            //this.TblDet.Visible = true;
            //find details

            LoadSpecificDevice(Convert.ToInt32((this.ddlSelectedDevice.SelectedValue)));
        }

        protected void cmbDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //this.TblDet.Visible = true;
            //find details
            LoadSpecificDevice(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue));
        }



        protected void btnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if ((ddlDeviceLocation.SelectedValue == null) == false)
                {
                    MyRem.LiveMonServer.AddEditLocationLink(Convert.ToInt32(this.ddlSelectedDevice.SelectedValue),Convert.ToInt32( ddlDeviceLocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.Other_Device, 99);
                }

            }
            catch (Exception ex)
            {
            }
        }



        protected void btnChangeSite_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if (Convert.ToString(Session["SelectedSite"]) != ddlDevice.SelectedValue)
                {
                    bool Myresp = MyRem.LiveMonServer.EditSensorDeviceSite(Convert.ToInt32(this.ddlSelectedDevice), Convert.ToInt32(ddlDevice.SelectedValue), LiveMonitoring.IRemoteLib.SDUpdateType.IPDevice);
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtExtraData.Text = "";
            this.txtExtraData1.Text = "";
            this.txtExtraData2.Text = "";
            this.txtExtraData3.Text = "";
            this.txtExtraData4.Text = "";
            this.txtExtraData5.Text = "";
            this.txtCaption.Text = "";
            //this.txtPort.Value = 0;
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
            lblError.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
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

      
    }
}