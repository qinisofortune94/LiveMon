using Microsoft.VisualBasic;
using System;
using System.Collections;
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
    public partial class AddOtherDeviceTemplate : System.Web.UI.Page
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
                    //LoadDevices();
                    //LoadSites();

                }
                catch (Exception ex)
                {
                }
                if (IsPostBack == false)
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                   // test.ipDevice(ddltype);
                    LoadPage();
                    //LoadSites();
                    //LoadDevices();
                    // LoadPeople();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        //public void LoadDevices()
        //{

        //    Collection MyCollection = new Collection();
        //    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
        //    MyCollection = MyRem.get_GetServerObjects((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]);
        //    //GetServerObjects 'server1.GetAll()
        //    object MyObject1 = null;
        //    int MyDiv = 1;
        //    bool added = false;

        //    foreach (object MyObject1_loopVariable in MyCollection)
        //    {
        //        MyObject1 = MyObject1_loopVariable;
        //        if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
        //        {
        //            LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
        //            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
        //            MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
        //            MyItem.Value = Convert.ToString(MyLocation.Id);
        //            MyItem.Selected = false;
        //            ddlDeviceLocation.Items.Add(MyItem);
        //        }
        //    }
        //}
        //public void LoadSites()
        //{
        //    try
        //    {
        //        List<LiveMonitoring.IRemoteLib.SiteDetails> MyCollection = new List<LiveMonitoring.IRemoteLib.SiteDetails>();
        //        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
        //        MyCollection = MyRem.GetServerAllSites;
        //        //GetServerObjects 'server1.GetAll()
        //        object MyObject1 = null;
        //        int MyDiv = 1;
        //        bool added = false;
        //        if ((MyCollection == null))
        //            return;
        //        foreach (object MyObject1_loopVariable in MyCollection)
        //        {
        //            MyObject1 = MyObject1_loopVariable;
        //            try
        //            {
        //                //cmbSensGroup
        //                if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
        //                {
        //                    LiveMonitoring.IRemoteLib.SiteDetails MySite = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
        //                    //not orphans
        //                    if (MySite.ID > 0)
        //                    {
        //                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
        //                        MyItem.Text = MySite.SiteName;
        //                        MyItem.Value = Convert.ToString(MySite.ID);
        //                        MyItem.Selected = false;
        //                        try
        //                        {
        //                            if (Convert.ToInt32(Session["SelectedSite"]) == MySite.ID)
        //                            {
        //                                MyItem.Selected = true;
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                        }
        //                        ddlDevice.Items.Add(MyItem);
        //                    }

        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //        }
        //        try
        //        {
        //            LiveMonitoring.testing test = new LiveMonitoring.testing();
        //          //  test.SortDropDown(ddlDevice);

        //        }
        //        catch (Exception ex)
        //        {
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //    }


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

        public void LoadPage()
        {

            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.OtherDevice(ddltype);
            //Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType));
            //Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType));
            //string x = null;
            //int MyVal = 0;
            //ddltype.Items.Clear();
            //foreach (string x_loopVariable in Item)
            //{
            //    x = x_loopVariable;
            //   System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
            //    MyItem.Text = x;
            //    MyItem.Value = ItemValue(MyVal);
            //    MyItem.Selected = false;
            //    ddltype.Items.Add(MyItem);
            //    MyVal += 1;
            //}
            ddlSerialPort.Items.Clear();
            System.Web.UI.WebControls.ListItem NewItems = new System.Web.UI.WebControls.ListItem();
            NewItems.Text = "None";
            int item = 0;
            NewItems.Value = item.ToString();
            ddlSerialPort.Items.Add(NewItems);

            int MyInt = 0;
            for (MyInt = 1; MyInt <= 20; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                NewItem.Text = "Com" + MyInt.ToString();
                NewItem.Value = MyInt.ToString();
                ddlSerialPort.Items.Add(NewItem);
            }
            for (MyInt = 0; MyInt <= 17; MyInt++)
            {
                System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                switch (MyInt)
                {
                    case 0:
                        int one = 75;
                        NewItem.Text = "75";
                        NewItem.Value = one.ToString();
                        break;
                    case 1:
                        NewItem.Text = "110";
                        NewItem.Value = "110";
                        break;
                    case 2:
                        NewItem.Text = "134";
                        int r = 134;
                        NewItem.Value = r.ToString();
                        break;
                    case 3:
                        int three = 150;
                        NewItem.Text = "150";
                        NewItem.Value = three.ToString();
                        break;
                    case 4:
                        int four = 300;
                        NewItem.Text = "300";
                        NewItem.Value = four.ToString();
                        break;
                    case 5:
                        int five = 600;
                        NewItem.Text = "600";
                        NewItem.Value = five.ToString();
                        break;
                    case 6:
                        int six = 1200;
                        NewItem.Text = "1200";
                        NewItem.Value = six.ToString();
                        break;
                    case 7:
                        int seven = 1800;
                        NewItem.Text = "1800";
                        NewItem.Value = seven.ToString();
                        break;
                    case 8:
                        int eight = 2400;
                        NewItem.Text = "2400";
                        NewItem.Value = eight.ToString();

                        break;
                    case 9:
                        int nine = 4800;
                        NewItem.Text = "4800";
                        NewItem.Value = nine.ToString();
                        break;
                    case 10:
                        int ten = 7200;
                        NewItem.Text = "7200";
                        NewItem.Value = ten.ToString(); ;
                        break;
                    case 11:
                        int eleven = 9600;
                        NewItem.Text = "9600";
                        NewItem.Value = eleven.ToString();

                        NewItem.Selected = true;
                        break;
                    case 12:
                        int twelve = 14400;
                        NewItem.Text = "14400";
                        NewItem.Value = twelve.ToString();
                        break;
                    case 13:
                        int thirteen = 19200;
                        NewItem.Text = "19200";
                        NewItem.Value = thirteen.ToString();
                        break;
                    case 14:
                        int fourteen = 38400;
                        NewItem.Text = "38400";
                        NewItem.Value = fourteen.ToString();
                        break;
                    case 15:
                        int fifteen = 57600;
                        NewItem.Text = "57600";
                        NewItem.Value = fifteen.ToString();
                        break;
                    case 16:
                        int sixteen = 115200;
                        NewItem.Text = "115200";
                        NewItem.Value = sixteen.ToString();
                        break;
                    case 17:
                        int seventeen = 128000;
                        NewItem.Text = "128000";
                        NewItem.Value = seventeen.ToString();
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
                        int BitsZero = 5;
                        NewItem.Text = "5";
                        NewItem.Value = BitsZero.ToString();
                        break;
                    case 1:
                        int BitsOne = 6;
                        NewItem.Text = "6";
                        NewItem.Value = BitsOne.ToString();
                        break;
                    case 2:
                        int BitsTwo = 7;
                        NewItem.Text = "7";
                        NewItem.Value = BitsTwo.ToString();
                        break;
                    case 3:
                        int BitsThree = 8;
                        NewItem.Text = "8";
                        NewItem.Value = BitsThree.ToString();
                        NewItem.Selected = true;
                        break;
                    case 4:
                        int BitsFour = 9;
                        NewItem.Text = "9";
                        NewItem.Value = BitsFour.ToString();
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
                        int StopOne = 1;
                        NewItem.Text = "1";
                        NewItem.Value = StopOne.ToString(); ;
                        NewItem.Selected = true;
                        break;
                    case 1:
                        double StopTwo = 1.5;
                        NewItem.Text = "1.5";
                        NewItem.Value = StopTwo.ToString(); ;
                        break;
                    case 2:
                        int StopThree = 2;
                        NewItem.Text = "2";
                        NewItem.Value = StopThree.ToString();
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
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                lblError.Visible = true;
                lblError.Text = "Please supply a caption!";
                return;
            }
            LiveMonitoring.IRemoteLib.OtherDeviceTemplate NewIPDevice = new LiveMonitoring.IRemoteLib.OtherDeviceTemplate();

            NewIPDevice.Caption = this.txtCaption.Text;
            NewIPDevice.ExtraData = this.txtExtraData.Text;
            NewIPDevice.ExtraData1 = this.txtExtraData1.Text;
            NewIPDevice.ExtraData2 = this.txtExtraData2.Text;
            if (!string.IsNullOrEmpty(this.txtExtraData3.Text))
                NewIPDevice.ExtraData3 = Convert.ToInt32(this.txtExtraData3.Text);
            if (!string.IsNullOrEmpty(this.txtExtraData4.Text))
                NewIPDevice.ExtraData4 = Convert.ToInt32(this.txtExtraData4.Text);
            if (!string.IsNullOrEmpty(this.txtExtraData5.Text))
                NewIPDevice.ExtraData5 = Convert.ToInt32(this.txtExtraData5.Text);
            NewIPDevice.IPAdress = txtIPaddress.Text;
            //   if (double.IsNaN(this.txtPort.Text) == false)
            NewIPDevice.Port = Convert.ToInt32(txtPort.Text);
            NewIPDevice.SerialPort = Convert.ToInt32(this.ddlSerialPort.SelectedValue);
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
            NewIPDevice.SerialSettings = this.ddlBaudRate.SelectedValue + "," + this.ddlDataBits.SelectedValue + "," + this.radStopBits.SelectedValue + "," + this.radHandShaking.SelectedValue;

            NewIPDevice.templateName = txtTemplateName.Text;
            // Save the template.
            bool Myresp = MyRem.LiveMonServer.AddNewOtherDeviceTemplate(NewIPDevice);

            if (Myresp)
            {
                //save fields
                successMessage.Visible = true;
                lblSuccess.Text = "Add OtherDevice Succeeded.";
                ClearVals();
            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again!";
            }
        }
        public void ClearVals()
        {
            int zero = 0;
            //lblErr.Visible = false;
           this.txtExtraData.Text = "";
            this.txtExtraData1.Text = "";
            this.txtExtraData2.Text = "";
            this.txtExtraData3.Text = "";
            this.txtExtraData4.Text = "";
            this.txtExtraData5.Text = "";
            this.txtCaption.Text = "";
            this.txtPort.Text = zero.ToString();
            //this.txtIP1.Value = 0;
            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;
        }

        protected void ddltype_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            lblSerialPort.Text = "Serial Port";
            lblExtraData.Text = "ExtraData";
            lblExtraData1.Text = "ExtraData1";
            lblExtraData2.Text = "ExtraData2";
            lblExtraData3.Text = "ExtraData3";
            lblExtraData4.Text = "ExtraData4";
            lblExtraData5.Text = "ExtraData5";
            int items = 0;
            switch (Convert.ToInt32(ddltype.SelectedValue))
            {
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.OneWireSerial:
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItems = new System.Web.UI.WebControls.ListItem();
                    NewItems.Text = "None";
                    NewItems.Text = items.ToString();
                    ddlSerialPort.Items.Add(NewItems);

                    int MyInt = 0;
                    for (MyInt = 1; MyInt <= 20; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.ModbusSerial:
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItemss = new System.Web.UI.WebControls.ListItem();
                    NewItemss.Text = "None";
                    NewItemss.Value = items.ToString();
                    ddlSerialPort.Items.Add(NewItemss);

                    //int MyInt = 0;
                    for (MyInt = 1; MyInt <= 20; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.OneWireUSB:
                    lblSerialPort.Text = "USB Port";
                    lblExtraData.Text = "Adapter Name";
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItemsss = new System.Web.UI.WebControls.ListItem();
                    NewItemsss.Text = "None";
                    NewItemsss.Value = items.ToString();
                    ddlSerialPort.Items.Add(NewItemsss);
                    // int MyInt = 0;
                    for (MyInt = 1; MyInt <= 10; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "USB" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.VotronicUSB:
                    lblSerialPort.Text = "USB Port";
                    lblExtraData.Text = "Adapter Name";
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItemssss = new System.Web.UI.WebControls.ListItem();
                    NewItemssss.Text = "None";
                    NewItemssss.Value = items.ToString();
                    ddlSerialPort.Items.Add(NewItemssss);
                    //int MyInt = 0;
                    for (MyInt = 1; MyInt <= 10; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "USB" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType.GPSReciever:
                    ddlSerialPort.Items.Clear();
                    System.Web.UI.WebControls.ListItem NewItemsssss = new System.Web.UI.WebControls.ListItem();
                    NewItemsssss.Text = "None";
                    NewItemsssss.Value = items.ToString();
                    ddlSerialPort.Items.Add(NewItemsssss);

                    // int MyInt = 0;
                    for (MyInt = 1; MyInt <= 20; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
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
                    System.Web.UI.WebControls.ListItem NewItemssssss = new System.Web.UI.WebControls.ListItem();
                    NewItemssssss.Text = "None";
                    NewItemssssss.Value = items.ToString();
                    ddlSerialPort.Items.Add(NewItemssssss);

                    //int MyInt = 0;
                    for (MyInt = 1; MyInt <= 20; MyInt++)
                    {
                        System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                        NewItem.Text = "Com" + MyInt.ToString();
                        NewItem.Value = MyInt.ToString();
                        ddlSerialPort.Items.Add(NewItem);
                    }

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
        public void Add_IPDevices()
        {
            Load += Page_Load;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            lblError.Visible = false;
            txtIPaddress.Text = "";
            txtExtraData.Text = "";
            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraData3.Text = "";
            txtCaption.Text = "";
            txtPort.Text = "";
            ddltype.SelectedIndex = -1;
            //ddlDeviceLocation.SelectedIndex = -1;
            //ddlDevice.SelectedIndex = -1;
            //this.txtIP1.Value = 0;

            //this.txtIP2.Value = 0;
            //this.txtIP3.Value = 0;
            //this.txtIP4.Value = 0;

        }

      

   
    }
}