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
    public partial class EditSensor : System.Web.UI.Page
    {
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
        int sensorIDD = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                LoadToEditSensor(Convert.ToInt32(Request.QueryString["SensorID"]));
            }
        }

        public void LoadToEditSensor(int getID)
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
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                try
                {
                    LoadDefaultImages();
                }
                catch
                {
                }
                if ((Request.QueryString["SensorID"] == null) == false)
                {
                    sensorIDD = Convert.ToInt32(Request.QueryString["SensorID"]);
                }

                if (IsPostBack == false)
                {
                    LoadPage();
                    LoadSites();
                }
                else
                {
                    StringBuilder TheScript = new StringBuilder();
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

        public void LoadPage()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.LoadValues(cmbSensOutput);
            test.LoadfieldNames(cmbType);

            LoadAlertGroups();
            //LoadSensors()
            //cmbFields.Visible = true;
            ClearRows();
            AddRow((new string[] {
                "Temp",
                "deg C",
                "1",
                "true"
            }));
            LoadDevices();
        }

        public void AddRow(string[] RowVals)
        {
            try
            {
                DataRow Row = default(DataRow);
                DataTable dt = new DataTable();
                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                }
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field Name", typeof(string));
                    dt.Columns.Add("Field Suffix", typeof(string));
                    dt.Columns.Add("Field", typeof(int));
                    dt.Columns.Add("Display Field", typeof(bool));
                    dt.Columns.Add("Field Max Val", typeof(double));
                    dt.Columns.Add("Field Min Val", typeof(double));
                    dt.Columns.Add("Field Notes", typeof(string));
                    dt.Columns.Add("Field Max Warn Val", typeof(double));
                    dt.Columns.Add("Field Min Warn Val", typeof(double));
                    dt.Columns.Add("Field Percentage Test", typeof(double));
                }
                Row = dt.NewRow();
                Row[0] = RowVals[0];
                Row[1] = RowVals[1];
                Row[2] = Convert.ToInt32(RowVals[2]);
                Row[3] = Convert.ToBoolean(RowVals[3]);
                try
                {
                    if (Information.UBound(RowVals) > 4)
                    {
                        Row[4] = Convert.ToDouble(RowVals[4]);
                        Row[5] = Convert.ToDouble(RowVals[5]);
                        Row[6] = RowVals[6];
                        if (!string.IsNullOrEmpty(RowVals[7]))
                            Row[7] = Convert.ToDouble(RowVals[7]);
                        else
                            Row[7] = 0;
                        if (!string.IsNullOrEmpty(RowVals[8]))
                            Row[8] = Convert.ToDouble(RowVals[8]);
                        else
                            Row[8] = 0;
                        if (!string.IsNullOrEmpty(RowVals[9]))
                            Row[9] = Convert.ToInt32(RowVals[9]);
                        else
                            Row[8] = 0;
                    }
                    else
                    {
                        Row[4] = 0;
                        Row[5] = 0;
                        Row[6] = "";
                    }
                }
                catch (Exception ex)
                {
                }
                dt.Rows.Add(Row);
                Session["mytable"] = dt;
                GridBind(dt);
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
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.SortDropDown(cmbSites);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadSpecificSensor(int ID)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (ID == Mysensor.ID)
                    {
                        LiveMonitoring.testing test = new LiveMonitoring.testing();
                        test.droType(cmbType, MyObject1);
                        //this.cmbType.SelectedValue = Convert.ToString(Mysensor.Type);
                        //int drop = Convert.ToInt32(this.cmbType.SelectedValue);
                        //drop = (int)(LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(Mysensor.Type);
                        cmbType2_SelectedIndexChanged(this, new EventArgs());
                        load_Rows(Mysensor);
                        if (Mysensor.Type != LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ServerLogSensor)
                        {
                            try
                            {
                                this.cmbDevice.SelectedValue = Mysensor.IPDeviceID.ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        
                        this.txtCaption.Text = Mysensor.Caption;
                        this.txtDivisor.Text = Mysensor.Divisor.ToString();
                        this.txtMaxValue.Text = Mysensor.MaxValue.ToString();
                        this.txtMinValue2.Text = Mysensor.MinValue.ToString();
                        this.txtModule.Text = Mysensor.ModuleNo.ToString();
                        this.txtMultiplier.Text = Mysensor.Multiplier.ToString();
                        this.txtRegister.Text = Mysensor.Register.ToString();
                        this.txtScanRate.Text = Mysensor.ScanRate.ToString();
                        if ((Mysensor.SerialNumber == null) == false)
                        {
                            this.txtSerialNumber.Text = Mysensor.SerialNumber.ToString();
                        }

                        this.txtExtraData.Text = Mysensor.ExtraData;
                        this.txtExtraData1.Text = Mysensor.ExtraData1;
                        this.txtExtraData2.Text = Mysensor.ExtraData2;
                        this.txtExtraData3.Text = Mysensor.ExtraData3;
                        this.txtExtraValue.Text = Mysensor.ExtraValue.ToString();
                        this.txtExtraValue1.Text = Mysensor.ExtraValue1.ToString();
                        try
                        {
                            if ((Mysensor.SensGroup == null) == false)
                            {
                                cmbSensGroup.SelectedValue = Mysensor.SensGroup.SensorGroupID.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            cmbSensOutput.SelectedValue = Mysensor.OutputType.ToString();
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            if ((Mysensor.SensorGroupContacts == null) == false)
                            {
                                DropDownAlertGroup.SelectedValue = Mysensor.SensorGroupContacts.ID.ToString();
                            }
                            else
                            {
                                int y = -1;
                                DropDownAlertGroup.SelectedValue = y.ToString();
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }

        public void LoadDevices(LiveMonitoring.SharedFuncs.DeviceFilter Filter = null)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;
            cmbSensors.Visible = true;
            cmbSensors.Items.Clear();
            cmbDevice.Items.Clear();
            cmbSensGroup.Items.Clear();
            cmbLocations.Items.Clear();
            if ((MyCollection == null) == false)
            {
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            bool AddSens = true;
                            if ((Session["SearchSensor"] == null) == false)
                            {
                                if (Mysensor.Caption.ToUpper().Contains(Convert.ToString(Session["SearchSensor"]).ToUpper()) == false)
                                {
                                    AddSens = false;
                                }
                            }
                            if (AddSens)
                            {
                                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                MyItem.Text = Mysensor.Caption;
                                MyItem.Value = Mysensor.ID.ToString();
                                cmbSensors.Items.Add(MyItem);
                                if (cmbSensors.Items.Count == 1)
                                {
                                    MyItem.Selected = true;
                                    //this.tbldet.Visible = true;
                                    LoadSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
                                }
                            }
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails Mysensor = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
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
                                if ((Filter == null))
                                {
                                    cmbDevice.Items.Add(MyItem);
                                }
                                else
                                {
                                    if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.Camera, 0))
                                    {
                                        cmbDevice.Items.Add(MyItem);
                                    }
                                }
                            }

                        }
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
                                if ((Filter == null))
                                {
                                    cmbDevice.Items.Add(MyItem);
                                }
                                else
                                {
                                    if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.IPDevice, Convert.ToInt32(Mysensor.Type)))
                                    {
                                        cmbDevice.Items.Add(MyItem);
                                    }
                                }
                            }
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.OtherDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
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
                                if ((Filter == null))
                                {
                                    cmbDevice.Items.Add(MyItem);
                                }
                                else
                                {
                                    if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.OtherDevice, Convert.ToInt32(Mysensor.Type)))
                                    {
                                        cmbDevice.Items.Add(MyItem);
                                    }
                                }
                            }

                        }
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
                                if ((Filter == null))
                                {
                                    cmbDevice.Items.Add(MyItem);
                                }
                                else
                                {
                                    if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.SNMPDevice, 0))
                                    {
                                        cmbDevice.Items.Add(MyItem);
                                    }
                                }
                            }

                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                        {
                            LiveMonitoring.IRemoteLib.SensorGroup MysensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MysensorGroup.SensorGroupName;
                            MyItem.Value = MysensorGroup.SensorGroupID.ToString();
                            MyItem.Selected = false;
                            cmbSensGroup.Items.Add(MyItem);
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
                    catch (Exception ex)
                    {
                    }
                }
                if ((Filter == null) == false)
                {
                    if (cmbDevice.Items.Count == 0)
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "No suitable Devices for selected sensor type .Please add device first !";
                        cmbType.Focus();
                        return;
                    }
                }
                try
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.SortDropDown(cmbSensGroup);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.SortDropDown(cmbDevice);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.SortDropDown(cmbSensors);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    if (sensorIDD != 0)
                    {
                        cmbSensors.SelectedValue = sensorIDD.ToString();
                        //this.tbldet.Visible = true;
                        LoadSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }

        public void load_Rows(LiveMonitoring.IRemoteLib.SensorDetails Mysensor)
        {
            ClearRows();
            //cmbFields.Visible = true;

            txtSerialNumber.Visible = true;
            lblExtraData.Text = "Extra Data";
            lblExtraData1.Text = "Extra Data 1";
            lblExtraData2.Text = "Extra Data 2";
            lblExtraData3.Text = "Extra Data 3";

            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in Mysensor.Fields)
            {
                try
                {
                    AddRow((new string[] {
                        MyField.FieldName,
                        MyField.Caption,
                        MyField.FieldNumber.ToString(),
                        MyField.DisplayValue.ToString(),
                        MyField.FieldMaxValue.ToString(),
                        MyField.FieldMinValue.ToString(),
                        MyField.FieldNotes,
                        MyField.FieldMaxWarnValue.ToString(),
                        MyField.FieldMinWarnValue.ToString(),
                        MyField.FieldPercentOfTest.ToString()
                    }));
                }
                catch (Exception ex)
                {
                }
            }

            switch (Mysensor.Type)
            {
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReader247DB:
                    Label7.Text = "Reader Number";
                    int m = 0;
                    this.txtModule.Text = m.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReaderSagem:
                    Label7.Text = "Reader Number";
                    int m2 = 0;
                    this.txtModule.Text = m2.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                    Label8.Text = "Output Number";
                    int r = 1;
                    txtRegister.Text = r.ToString();
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    Label5.Text = "SerialNumber";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    Label5.Text = "SerialNumber";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    Label5.Text = "SerialNumber";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    Label5.Text = "SerialNumber";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    Label5.Text = "SerialNumber";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegTemperature:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegDryContact:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegFlood:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegPowerDetector:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegHumidity:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    Label5.Text = "SerialNumber";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                    Label5.Text = "URL to Open";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                    Label5.Text = "SNMP OID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                    Label5.Text = "SQL Connection Str";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    Label5.Text = "Bit To Check";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    Label5.Text = "Bit To Check";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OPCItem:
                    Label5.Text = "OPC Item Name";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireDigiPotenType2C:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireLioBatMonitorType30:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireDualAdressableSwitchC:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireQuadADType20:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireTHDSmartMonitorType26:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireTempType10:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireTempType28:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireThermocronType21:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusDiscrete:
                    Label5.Text = "Bit To Check";
                    break;
                //Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecUPS:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.Megatec3Phase:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ShutUPS:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReaderZKSoft:
                    Label5.Text = "SerialNumber";
                    break;
                //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PowerWareSNMP:
                    Label5.Text = "SerialNumber";

                    //'Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCC"
                    LiveMonitoring.IRemoteLib.PowerWareSNMP MyPowerDevice = new LiveMonitoring.IRemoteLib.PowerWareSNMP();
                    LiveMonitoring.IRemoteLib.PowerWareSNMP.OIDObj MyPowerDeviceOID = default(LiveMonitoring.IRemoteLib.PowerWareSNMP.OIDObj);
                    //for now just defult to 9390
                    MyPowerDevice.LoadModels("POWERWARE 9390");
                    int OIDCNT = 0;
                    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyPowerDevice.PowerWareOIDS) - 1; OIDCNT++)
                    {
                        try
                        {
                            MyPowerDeviceOID = MyPowerDevice.PowerWareOIDS[OIDCNT];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DeltaSNMP:
                    Label5.Text = "SerialNumber";
                    LiveMonitoring.IRemoteLib.DeltaSNMP MyPowerDevice1 = new LiveMonitoring.IRemoteLib.DeltaSNMP();
                    LiveMonitoring.IRemoteLib.DeltaSNMP.OIDObj MyPowerDeviceOID1 = default(LiveMonitoring.IRemoteLib.DeltaSNMP.OIDObj);
                    MyPowerDevice1.LoadModels("Delta");
                    int OIDCNT1 = 0;
                    for (OIDCNT1 = 0; OIDCNT1 <= Information.UBound(MyPowerDevice1.DeltaOIDS) - 1; OIDCNT1++)
                    {
                        try
                        {
                            MyPowerDeviceOID1 = MyPowerDevice1.DeltaOIDS[OIDCNT1];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegMonPDU8SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M8PDU";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice.LoadModels(txtExtraData.Text);
                    int OIDCNT2 = 0;
                    for (OIDCNT2 = 0; OIDCNT2 <= Information.UBound(MyContegDevice.ContegOIDS) - 1; OIDCNT2++)
                    {
                        try
                        {
                            MyContegDeviceOID = MyContegDevice.ContegOIDS[OIDCNT2];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegMonPDU24SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M24PDU";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice1 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID1 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice1.LoadModels(txtExtraData.Text);
                    int OIDCNT3 = 0;
                    for (OIDCNT3 = 0; OIDCNT3 <= Information.UBound(MyContegDevice1.ContegOIDS) - 1; OIDCNT3++)
                    {
                        try
                        {
                            MyContegDeviceOID1 = MyContegDevice1.ContegOIDS[OIDCNT3];

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegMonPDU16SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M16PDU";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice2 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID2 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice2.LoadModels(txtExtraData.Text);
                    int OIDCNT4 = 0;
                    for (OIDCNT4 = 0; OIDCNT4 <= Information.UBound(MyContegDevice2.ContegOIDS) - 1; OIDCNT4++)
                    {
                        try
                        {
                            MyContegDeviceOID2 = MyContegDevice2.ContegOIDS[OIDCNT4];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegIntPDU008C3SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M8PDU";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice3 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID3 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice3.LoadModels(txtExtraData.Text);
                    int OIDCNT5 = 0;
                    for (OIDCNT5 = 0; OIDCNT5 <= Information.UBound(MyContegDevice3.ContegOIDS) - 1; OIDCNT5++)
                    {
                        try
                        {
                            MyContegDeviceOID3 = MyContegDevice3.ContegOIDS[OIDCNT5];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegIntPDU24SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M24PDU";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice4 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID4 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice4.LoadModels(txtExtraData.Text);
                    int OIDCNT6 = 0;
                    for (OIDCNT6 = 0; OIDCNT6 <= Information.UBound(MyContegDevice4.ContegOIDS) - 1; OIDCNT6++)
                    {
                        try
                        {
                            MyContegDeviceOID4 = MyContegDevice4.ContegOIDS[OIDCNT6];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegRamosCSNMP:
                    cmbModels.Visible = true;
                    txtExtraData.Visible = false;
                    LoadModels();
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "RamosMiniC";
                    LiveMonitoring.IRemoteLib.ContegRamosSNMP MyContegDevice6 = new LiveMonitoring.IRemoteLib.ContegRamosSNMP();
                    LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj MyContegDeviceOID6 = default(LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj);
                    MyContegDevice6.LoadModels(txtExtraData.Text);
                    int OIDCNT7 = 0;
                    for (OIDCNT7 = 0; OIDCNT7 <= Information.UBound(MyContegDevice6.ContegOIDS) - 1; OIDCNT7++)
                    {
                        try
                        {
                            MyContegDeviceOID6 = MyContegDevice6.ContegOIDS[OIDCNT7];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonSNMP:
                    cmbModels.Visible = true;
                    txtExtraData.Visible = false;
                    LoadModels();
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "3268";
                    LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP MyContegDevice8 = new LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP();
                    LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj MyContegDeviceOID8 = default(LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj);
                    MyContegDevice8.LoadModels(txtExtraData.Text);
                    int OIDCNT9 = 0;
                    for (OIDCNT9 = 0; OIDCNT9 <= Information.UBound(MyContegDevice8.hwgroupOIDS) - 1; OIDCNT9++)
                    {
                        try
                        {
                            MyContegDeviceOID8 = MyContegDevice8.hwgroupOIDS[OIDCNT9];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLTemperature:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLDryContact:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLFlood:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLPowerDetector:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLHumidity:
                    Label5.Text = "Sens ID";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GamatronicsSNMP:
                    Label5.Text = "SerialNumber";
                    LiveMonitoring.IRemoteLib.GamaTronicsSNMP MyGamaDevice = new LiveMonitoring.IRemoteLib.GamaTronicsSNMP();
                    LiveMonitoring.IRemoteLib.GamaTronicsSNMP.OIDObj MyGamaDeviceOID = default(LiveMonitoring.IRemoteLib.GamaTronicsSNMP.OIDObj);
                    MyGamaDevice.LoadModels("Default");
                    int OIDCNT10 = 0;
                    for (OIDCNT10 = 0; OIDCNT10 <= Information.UBound(MyGamaDevice.GamaTronicsOIDS) - 1; OIDCNT10++)
                    {
                        try
                        {
                            MyGamaDeviceOID = MyGamaDevice.GamaTronicsOIDS[OIDCNT10];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecSNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "UPS Type-1Phase/3Phase";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SMTPCheckSensor:
                    Label5.Text = "Email From";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPCheckSensor | LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPLastDateSensor | LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPCountSensor:
                    Label5.Text = "Email From";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    Label5.Text = "WMI Class";
                    lblExtraData.Text = "WMI Namespace";
                    lblExtraData1.Text = "WMI Property";
                    lblExtraData2.Text = "WMI Value";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIMemorySensor:
                    Label5.Text = "Serial No";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIPCInfoSensor:
                    Label5.Text = "Serial No";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIPrintersSensor:
                    Label5.Text = "Serial No";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIProcessorLoadSensor:
                    Label5.Text = "Serial No";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIProcessRunningSensor:
                    Label5.Text = "Process Name";
                    break;
                //Me.txtSerialNumber.InputMask = "CCCCCCCCCCCCCCCC" = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC"
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIServicesSensor:
                    Label5.Text = "Serial No";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIDrivesSensor:
                    Label5.Text = "Serial No";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPSite:
                    AddRow((new string[] {
                        "Response",
                        "ms",
                        "1",
                        "true"
                    }));
                    lblExtraData.Text = "IP Adress/Host";
                    lblExtraValue.Text = "Slow Response";
                    lblExtraValue1.Text = "Report Threshhold";
                    txtExtraValue1.Text = "999999";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PSExch07Specific | LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PSExchX64Specific:
                    lblExtraData.Text = "";
                    lblExtraData2.Text = "Mailbox";
                    lblExtraValue.Text = "Slow Response";
                    lblExtraValue1.Text = "Count Threshhold";
                    txtExtraValue1.Text = "0";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PSExch07All:
                    lblExtraData.Text = "";
                    lblExtraData2.Text = "Mailbox";
                    lblExtraValue.Text = "Slow Response";
                    lblExtraValue1.Text = "Count Threshhold";
                    txtExtraValue1.Text = "0";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM20TCP:
                    LiveMonitoring.IRemoteLib.LovatoGensetRGAM20 myset = new LiveMonitoring.IRemoteLib.LovatoGensetRGAM20();
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM60TCP:
                    LiveMonitoring.IRemoteLib.LovatoGensetRGAM60 myset1 = new LiveMonitoring.IRemoteLib.LovatoGensetRGAM60();
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIServiceRunningSensor:
                    lblExtraData.Text = "Service Display Name";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIRegistrySensor:
                    lblExtraData.Text = "HdefKey Name";
                    lblExtraData1.Text = "Sub Key Name";
                    lblExtraData2.Text = "Value Name";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCurrentValues:
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCurrentValues:

                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCUMULATIVEREGISTERS:
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCUMULATIVEREGISTERS:

                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCumulativeMaxDemandRegisters:
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCumulativeMaxDemandRegisters:
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile:
                    lblExtraValue1.Text = "NotifiedMaxDemand";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile:
                    lblExtraValue1.Text = "NotifiedMaxDemand";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                    cmbModels.Visible = true;
                    txtExtraData.Visible = false;
                    LoadModels();
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    lblExtraValue.Text = "Bus no.";
                    int e = 1;
                    txtExtraValue.Text = e.ToString();
                    lblExtraValue1.Text = "Module no.";
                    txtExtraValue1.Text = e.ToString();
                    lblExtraData1.Text = "Global Adress";
                    txtExtraData1.Text = e.ToString();
                    LiveMonitoring.IRemoteLib.StulzWIB8000SNMP MyStulzWIB8000SNMPDevice = new LiveMonitoring.IRemoteLib.StulzWIB8000SNMP(Convert.ToInt32(txtExtraValue.Text), Convert.ToInt32(txtExtraValue1.Text), Convert.ToInt32(txtExtraData1.Text));
                    LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj MyStulzDeviceOID = default(LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj);
                    MyStulzWIB8000SNMPDevice.LoadModels(cmbModels.SelectedValue);
                    int OIDCNT11 = 0;
                    for (OIDCNT11 = 0; OIDCNT11 <= Information.UBound(MyStulzWIB8000SNMPDevice.StulzOIDS) - 1; OIDCNT11++)
                    {
                        try
                        {
                            MyStulzDeviceOID = MyStulzWIB8000SNMPDevice.StulzOIDS[OIDCNT11];
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEDualTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    int exx = 0;
                    txtExtraValue.Text = exx.ToString();
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus = new LiveMonitoring.IRemoteLib.GEModbus("DualTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt = 0;
                    for (mycnt = 0; mycnt <= MYGEMBus.GensetTable.GetUpperBound(0) - 1; mycnt++)
                    {
                        Myrow = MYGEMBus.GensetTable[mycnt];
                        AddRow((new string[] {
                            Myrow.SettingName,
                            Myrow.SettingUnits,
                            (mycnt + 1).ToString(),
                            "true"
                        }));
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GETransFix:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    int ev = 0;
                    txtExtraValue.Text = ev.ToString();
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus1 = new LiveMonitoring.IRemoteLib.GEModbus("TransFix");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow1 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt1 = 0;
                    for (mycnt1 = 0; mycnt1 <= MYGEMBus1.GensetTable.GetUpperBound(0) - 1; mycnt1++)
                    {
                        Myrow1 = MYGEMBus1.GensetTable[mycnt1];
                        AddRow((new string[] {
                            Myrow1.SettingName,
                            Myrow1.SettingUnits,
                            (mycnt1 + 1).ToString(),
                            "true"
                        }));
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GETapTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    int t = 0;
                    txtExtraValue.Text = t.ToString();
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus2 = new LiveMonitoring.IRemoteLib.GEModbus("TapTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow2 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt2 = 0;
                    for (mycnt2 = 0; mycnt2 <= MYGEMBus2.GensetTable.GetUpperBound(0) - 1; mycnt2++)
                    {
                        Myrow2 = MYGEMBus2.GensetTable[mycnt2];
                        AddRow((new string[] {
                            Myrow2.SettingName,
                            Myrow2.SettingUnits,
                            (mycnt2 + 1).ToString(),
                            "true"
                        }));
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEMultiTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    int y = 0;
                    txtExtraValue.Text = y.ToString();
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus3 = new LiveMonitoring.IRemoteLib.GEModbus("MultiTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow3 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt3 = 0;
                    for (mycnt3 = 0; mycnt3 <= MYGEMBus3.GensetTable.GetUpperBound(0) - 1; mycnt3++)
                    {
                        Myrow3 = MYGEMBus3.GensetTable[mycnt3];
                        AddRow((new string[] {
                            Myrow3.SettingName,
                            Myrow3.SettingUnits,
                            (mycnt3 + 1).ToString(),
                            "true"
                        }));
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEMiniTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    int rr = 0;
                    txtExtraValue.Text = rr.ToString();
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus4 = new LiveMonitoring.IRemoteLib.GEModbus("MiniTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow4 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt4 = 0;
                    for (mycnt4 = 0; mycnt4 <= MYGEMBus4.GensetTable.GetUpperBound(0) - 1; mycnt4++)
                    {
                        Myrow4 = MYGEMBus4.GensetTable[mycnt4];
                        AddRow((new string[] {
                            Myrow4.SettingName,
                            Myrow4.SettingUnits,
                            (mycnt4 + 1).ToString(),
                            "true"
                        }));
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEHydranM2:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    int xx = 0;
                    txtExtraValue.Text = xx.ToString();
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus5 = new LiveMonitoring.IRemoteLib.GEModbus("HydranM2");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow5 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt5 = 0;
                    for (mycnt5 = 0; mycnt5 <= MYGEMBus5.GensetTable.GetUpperBound(0) - 1; mycnt5++)
                    {
                        Myrow5 = MYGEMBus5.GensetTable[mycnt5];
                        AddRow((new string[] {
                            Myrow5.SettingName,
                            Myrow5.SettingUnits,
                            (mycnt5 + 1).ToString(),
                            "true"
                        }));
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.JanitzaUMG604MbusReadings:

                    LiveMonitoring.IRemoteLib.JanitzaModbus Mygenset = new LiveMonitoring.IRemoteLib.JanitzaModbus("UMG604R");
                    LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue Myrow6 = default(LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue);
                    int mycnt6 = 0;
                    for (mycnt6 = 0; mycnt6 <= Mygenset.GensetTable.GetUpperBound(0) - 1; mycnt6++)
                    {
                        Myrow6 = Mygenset.GensetTable[mycnt6];
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.JanitzaUMG604MbusHarmonics:

                    LiveMonitoring.IRemoteLib.JanitzaModbus Mygenset2 = new LiveMonitoring.IRemoteLib.JanitzaModbus("UMG604Har");
                    LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue Myrow8 = default(LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue);
                    int mycnt9 = 0;
                    for (mycnt9 = 0; mycnt9 <= Mygenset2.GensetTable.GetUpperBound(0) - 1; mycnt9++)
                    {
                        Myrow8 = Mygenset2.GensetTable[mycnt9];
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.JanitzaUMG604MbusAll:

                    LiveMonitoring.IRemoteLib.JanitzaModbus Mygenset3 = new LiveMonitoring.IRemoteLib.JanitzaModbus("UMG604");
                    LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue Myrow11 = default(LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue);
                    int mycnt11 = 0;
                    for (mycnt11 = 0; mycnt11 <= Mygenset3.GensetTable.GetUpperBound(0) - 1; mycnt11++)
                    {
                        Myrow11 = Mygenset3.GensetTable[mycnt11];
                    }

                    break;
            }

        }

        public void LoadModels()
        {
            try
            {
                cmbModels.Items.Clear();
                LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue);
                switch (MyEnum)
                {
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                        cmbModels.Items.Add(new ListItem("Nothing", ""));
                        cmbModels.Items.Add(new ListItem("Stultz-C1002", "C1002"));
                        cmbModels.Items.Add(new ListItem("Stulz-C1010/C2020", "C1010/C2020"));
                        cmbModels.Items.Add(new ListItem("Stultz-C2020FCB", "C2020FCB"));
                        cmbModels.Items.Add(new ListItem("Stultz-C4000", "C4000"));
                        cmbModels.Items.Add(new ListItem("Stultz-C5000", "C5000"));
                        cmbModels.Items.Add(new ListItem("Stultz-C6000", "C6000"));
                        cmbModels.Items.Add(new ListItem("Stultz-C6000CH", "C6000CH"));
                        cmbModels.Items.Add(new ListItem("*Stultz-C7000IOC", "C7000IOC"));
                        cmbModels.Items.Add(new ListItem("Stultz-C7000CH", "C7000CH"));
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegRamosCSNMP:
                        cmbModels.Items.Add(new ListItem("Nothing", ""));
                        cmbModels.Items.Add(new ListItem("RamosMiniC", "RamosMiniC"));

                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonSNMP:
                        cmbModels.Items.Add(new ListItem("Nothing", ""));
                        cmbModels.Items.Add(new ListItem("Poseidon-3262", "3262"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3265", "3265"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3266", "3266"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3268", "3268"));
                        cmbModels.Items.Add(new ListItem("Poseidon-1250", "1250"));
                        cmbModels.Items.Add(new ListItem("Poseidon-2250", "2250"));
                        cmbModels.Items.Add(new ListItem("Poseidon-2261", "2261"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3468", "3468"));
                        cmbModels.Items.Add(new ListItem("Poseidon-Sitemon", "Sitemon"));
                        cmbModels.Items.Add(new ListItem("Poseidon-4001", "4001"));
                        cmbModels.Items.Add(new ListItem("Poseidon-4002", "4002"));

                        break;
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void LoadAlertGroups()
        {
            try
            {
                List<LiveMonitoring.IRemoteLib.GroupContacts> MyCollection = default(List<LiveMonitoring.IRemoteLib.GroupContacts>);
                DropDownAlertGroup.Items.Clear();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.LiveMonServer.GetAllAlertGroups();
                if ((MyCollection == null))
                    return;
                foreach (LiveMonitoring.IRemoteLib.GroupContacts MyObject1 in MyCollection)
                {
                    try
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MyObject1.Name;
                        MyItem.Value = MyObject1.ID.ToString();
                        MyItem.Selected = false;
                        DropDownAlertGroup.Items.Add(MyItem);

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadDefaultImages()
        {
            try
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
                string base64StringpstrNormal = Convert.ToBase64String(pstrNormal, 0, pstrNormal.Length);
                imgNormal.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNormal;

                string base64StringpstrError = Convert.ToBase64String(pstrError, 0, pstrError.Length);
                imgError.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrError;

                string base64StringpstrNoresponse = Convert.ToBase64String(pstrNoresponse, 0, pstrNoresponse.Length);
                imgResponse.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNoresponse;
            }
            catch (Exception ex)
            {
            }
        }

        public void add_Rows(int MyType)
        {
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)MyType;
            cmbFields2.Visible = true;
            //cmbFields.Visible = true;
            ClearRows();
            LiveMonitoring.IRemoteLib.SensorFieldsDefault MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDefault(MyEnum);
            if ((MyFields == null) == false)
            {
                AddRows(MyFields.FieldsList);
            }
        }

        public void AddRows(List<LiveMonitoring.IRemoteLib.SensorFieldsDef> RowVals)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                }

                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field Name", typeof(string));
                    dt.Columns.Add("Field Suffix", typeof(string));
                    dt.Columns.Add("Field", typeof(int));
                    dt.Columns.Add("Display Field", typeof(bool));
                    dt.Columns.Add("Field Max Val", typeof(double));
                    dt.Columns.Add("Field Min Val", typeof(double));
                    dt.Columns.Add("Field Notes", typeof(string));
                    dt.Columns.Add("Field Max Warn Val", typeof(double));
                    dt.Columns.Add("Field Min Warn Val", typeof(double));
                    dt.Columns.Add("Field Percentage Test", typeof(double));
                }
                DataRow Row = default(DataRow);
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in RowVals)
                {
                    Row = dt.NewRow();
                    try
                    {
                        Row[0] = MyField.FieldName;
                        Row[1] = MyField.Caption;
                        Row[2] = MyField.FieldNumber;
                        Row[3] = MyField.DisplayValue;
                        if (!string.IsNullOrEmpty(MyField.FieldMaxValue.ToString()))
                        {
                            Row[4] = MyField.FieldMaxValue;
                        }
                        else
                        {
                            Row[4] = 0;
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldMinValue.ToString()))
                        {
                            Row[5] = MyField.FieldMinValue;
                        }
                        else
                        {
                            Row[5] = 0;
                        }
                        if (MyField.FieldNotes != null)
                        {
                            Row[6] = MyField.FieldNotes;
                        }
                        else
                        {
                            Row[6] = "";
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldMaxWarnValue.ToString()))
                        {
                            Row[7] = MyField.FieldMaxWarnValue;
                        }
                        else
                        {
                            Row[7] = 0;
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldMinWarnValue.ToString()))
                        {
                            Row[8] = MyField.FieldMinWarnValue;
                        }
                        else
                        {
                            Row[8] = 0;
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldPercentOfTest.ToString()))
                        {
                            Row[9] = MyField.FieldPercentOfTest;
                        }
                        else
                        {
                            Row[9] = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    dt.Rows.Add(Row);
                }
                Session["mytable"] = dt;
                GridBind(dt);
            }
            catch (Exception ex)
            {
            }
        }

        public void GridBind(DataTable dt)
        {
            this.cmbFields2.DataSource = dt;
            cmbFields2.DataKeyNames = (new string[] { "Field" });
            cmbFields2.DataBind();
        }

        public void ClearRows()
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Field Name", typeof(string));
                dt.Columns.Add("Field Suffix", typeof(string));
                dt.Columns.Add("Field", typeof(int));
                dt.Columns.Add("Display Field", typeof(bool));
                dt.Columns.Add("Field Max Val", typeof(double));
                dt.Columns.Add("Field Min Val", typeof(double));
                dt.Columns.Add("Field Notes", typeof(string));
                dt.Columns.Add("Field Max Warn Val", typeof(double));
                dt.Columns.Add("Field Min Warn Val", typeof(double));
                dt.Columns.Add("Field Percentage Test", typeof(double));
            }
            Session["mytable"] = dt;
            GridBind(dt);
        }

        protected void btnnSearchSens_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchSensor"] = txtSensName.Text;
                LoadDevices();

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnnSearchDevice_Click(object sender, EventArgs e)
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

        protected void cmbFields_DataBinding(object sender, System.EventArgs e)
        {
            cmbFields2.PageSize = 5;
            cmbFields2.AllowPaging = true;
        }

        protected void cmbFields_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            cmbFields2.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            cmbFields2.EditIndex = -1;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            cmbFields2.EditIndex = e.NewEditIndex;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            dynamic dt = (DataTable)Session["mytable"];
            dynamic row = cmbFields2.Rows[e.RowIndex];
            dt.Rows[row.DataItemIndex]["Field Name"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Suffix"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Display Field"] = ((CheckBox)(row.Cells[4].Controls[0])).Checked;
            dt.Rows[row.DataItemIndex]["Field Max Val"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Min Val"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Notes"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Max Warn Val"] = ((TextBox)(row.Cells[8].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Min Warn Val"] = ((TextBox)(row.Cells[9].Controls[0])).Text;

            if (!string.IsNullOrEmpty(((TextBox)(row.Cells[10].Controls[0])).Text))
                dt.Rows[row.DataItemIndex]["Field Percentage Test"] = ((TextBox)(row.Cells[10].Controls[0])).Text;
            cmbFields2.EditIndex = -1;
            Session["mytable"] = dt;
            GridBind(dt);
        }

        protected void cmbSensors_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //this.tbldet.Visible = true;
            LoadSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
        }

        protected void cmbType2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                add_Rows(Convert.ToInt32(this.cmbType.SelectedValue));

            }
            catch (Exception ex)
            {
            }

        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
        }

        protected void editAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx");
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Response.Redirect("Index.aspx");
        }

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }

        public LiveMonitoring.IRemoteLib.SensorDetails ReturnSpecificSensor(int ID)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            LiveMonitoring.IRemoteLib.SensorDetails Mysensor1 = new LiveMonitoring.IRemoteLib.SensorDetails();
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (ID == Mysensor.ID)
                    {
                        return Mysensor;
                    }
                }
            }

            return Mysensor1;
        }

        protected void cmbModels_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue);
            switch (MyEnum)
            {
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                    try
                    {
                        this.txtExtraData.Text = cmbModels.SelectedValue;
                        ClearRows();
                        if (!string.IsNullOrEmpty(txtExtraValue.Text) & !string.IsNullOrEmpty(txtExtraValue1.Text) & !string.IsNullOrEmpty(txtExtraData1.Text))
                        {
                            LiveMonitoring.IRemoteLib.StulzWIB8000SNMP MyStulzWIB8000SNMPDevice = new LiveMonitoring.IRemoteLib.StulzWIB8000SNMP(Convert.ToInt32(txtExtraValue.Text), Convert.ToInt32(txtExtraValue1.Text), Convert.ToInt32(txtExtraData1.Text));
                            LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj MyStulzDeviceOID = default(LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj);
                            MyStulzWIB8000SNMPDevice.LoadModels(cmbModels.SelectedValue);
                            int OIDCNT = 0;
                            for (OIDCNT = 0; OIDCNT <= Information.UBound(MyStulzWIB8000SNMPDevice.StulzOIDS) - 1; OIDCNT++)
                            {
                                try
                                {
                                    MyStulzDeviceOID = MyStulzWIB8000SNMPDevice.StulzOIDS[OIDCNT];
                                    AddRow((new string[] {
                                        MyStulzDeviceOID.OIDName,
                                        "val",
                                        (OIDCNT + 1).ToString(),
                                        "true"
                                    }));
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else
                        {
                            warningMessage.Visible = true;
                            lblWarning.Text = "Please enter Global ID/Address/Bus no.";
                            cmbSensors.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegRamosCSNMP:
                    try
                    {
                        this.txtExtraData.Text = cmbModels.SelectedValue;
                        ClearRows();
                        LiveMonitoring.IRemoteLib.ContegRamosSNMP MyContegSNMPDevice = new LiveMonitoring.IRemoteLib.ContegRamosSNMP(txtExtraData.Text);
                        LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj MyContegDeviceOID = default(LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj);
                        MyContegSNMPDevice.LoadModels(cmbModels.SelectedValue);
                        int OIDCNT = 0;
                        for (OIDCNT = 0; OIDCNT <= Information.UBound(MyContegSNMPDevice.ContegOIDS) - 1; OIDCNT++)
                        {
                            try
                            {
                                MyContegDeviceOID = MyContegSNMPDevice.ContegOIDS[OIDCNT];
                                AddRow((new string[] {
                                    MyContegDeviceOID.OIDName,
                                    "val",
                                    (OIDCNT + 1).ToString(),
                                    "true"
                                }));
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonSNMP:
                    try
                    {
                        this.txtExtraData.Text = cmbModels.SelectedValue;
                        ClearRows();
                        LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP MyHWGroupSNMPDevice = new LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP(txtExtraData.Text);
                        LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj MyHWGroupDeviceOID = default(LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj);
                        MyHWGroupSNMPDevice.LoadModels(cmbModels.SelectedValue);
                        int OIDCNT = 0;
                        for (OIDCNT = 0; OIDCNT <= Information.UBound(MyHWGroupSNMPDevice.hwgroupOIDS) - 1; OIDCNT++)
                        {
                            try
                            {
                                MyHWGroupDeviceOID = MyHWGroupSNMPDevice.hwgroupOIDS[OIDCNT];
                                AddRow((new string[] {
                                    MyHWGroupDeviceOID.OIDName,
                                    "val",
                                    (OIDCNT + 1).ToString(),
                                    "true"
                                }));
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
            }
        }

        protected void btnTestSensor_Click(object sender, EventArgs e)
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
            LiveMonitoring.IRemoteLib.SensorDetails NewSensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            LiveMonitoring.IRemoteLib.SensorDetails Cursensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();

            try
            {
                Cursensor = ReturnSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblErr.Text = "error cannot find current sensor";

                cmbSensors.Focus();
                return;
            }
            if ((Cursensor == null) == true)
            {
                return;
                //error cannot find current sensor
            }
            switch (Convert.ToInt32(this.cmbType.SelectedValue))
            {
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BaseAudio:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraAudio:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDInput:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Output number.";

                        cmbSensors.Focus();
                        return;
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Min Value.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Max Value.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                    //check if device is snmp device

                    //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                    Collection MyCollection = default(Collection);
                    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                    //GetServerObjects 'server1.GetAll()
                    object MyObject1 = null;
                    //LiveMonitoring.IRemoteLib.SNMPManagerDetails
                    bool IsSnmp = false;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            if (this.cmbDevice.SelectedValue == (string)MyObject1)
                            {
                                IsSnmp = true;
                            }

                        }
                    }

                    if (IsSnmp == false)
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please Select a SNMP device.";

                        cmbSensors.Focus();
                        return;

                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Min Value.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Max Value.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();

                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";

                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    if (string.IsNullOrEmpty(this.txtExtraData.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter WMI Namespace.";

                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData1.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter WMI Property";

                        cmbSensors.Focus();
                        return;
                    }

                    break;
            }
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply a Caption.";

                cmbSensors.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.cmbDevice.SelectedValue))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select IP Device.";

                cmbSensors.Focus();

                return;
            }
            //asign cur vals then replace
            NewSensor = Cursensor;
            bool ChangeType = false;
            if (NewSensor.Type != (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue))
            {
                ChangeType = true;
            }
            NewSensor.Type = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue);
            NewSensor.IPDeviceID = Convert.ToInt32(this.cmbDevice.SelectedValue);
            NewSensor.ModuleNo = Convert.ToInt32(this.txtModule.Text);
            NewSensor.Register = Convert.ToInt32(this.txtRegister.Text);
            NewSensor.SerialNumber = this.txtSerialNumber.Text.Trim();
            NewSensor.Caption = this.txtCaption.Text;
            if (this.filImageNormal.HasFile)
            {
                NewSensor.ImageNormal = Myfunc.Strip_Image(this.filImageNormal);
                NewSensor.ImageNormalByte = MyRem.ImagetoByte(NewSensor.ImageNormal, ImageFormat.Bmp);
            }
            if (this.filImageNoResponse.HasFile)
            {
                NewSensor.ImageNoResponse = Myfunc.Strip_Image(this.filImageNoResponse);
                NewSensor.ImageNoResponseByte = MyRem.ImagetoByte(NewSensor.ImageNoResponse, ImageFormat.Bmp);
            }
            if (this.filImageError.HasFile)
            {
                NewSensor.ImageError = Myfunc.Strip_Image(this.filImageError);
                NewSensor.ImageErrorByte = MyRem.ImagetoByte(NewSensor.ImageError, ImageFormat.Bmp);
            }
            NewSensor.MinValue = Convert.ToDouble(this.txtMinValue2.Text);
            NewSensor.MaxValue = Convert.ToDouble(this.txtMaxValue.Text);
            NewSensor.Multiplier = Convert.ToDouble(this.txtMultiplier.Text);
            NewSensor.Divisor = Convert.ToDouble(this.txtDivisor.Text);
            if (Convert.ToDouble(this.txtScanRate.Text) > 0 & Convert.ToDouble(this.txtScanRate.Text) < 5000)
            {
                NewSensor.ScanRate = 5000;
            }
            else
            {
                NewSensor.ScanRate = Convert.ToDouble(this.txtScanRate.Text);
            }
            NewSensor.ExtraData = (this.txtExtraData.Text);
            NewSensor.ExtraData1 = (this.txtExtraData1.Text);
            NewSensor.ExtraData2 = (this.txtExtraData2.Text);
            NewSensor.ExtraData3 = (this.txtExtraData3.Text);
            NewSensor.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
            NewSensor.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
            try
            {
                NewSensor.SensGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
                NewSensor.SensGroup.SensorGroupID = Convert.ToInt32(cmbSensGroup.SelectedValue);
                NewSensor.SensGroup.SensorGroupName = cmbSensGroup.Items[cmbSensGroup.SelectedIndex].Text.Trim();

            }
            catch (Exception ex)
            {
            }
            if (cmbSensOutput.Visible)
            {
                try
                {
                    NewSensor.OutputType = (LiveMonitoring.IRemoteLib.SensorDetails.OutputTypeDef)Convert.ToInt32(cmbSensOutput.SelectedValue);

                }
                catch (Exception ex)
                {
                }
            }
            DataTable dt = new DataTable();

            if (Session["mytable"] == null == false)
            {
                dt = (DataTable)Session["mytable"];
            }

            try
            {
                //DataRow MyRow = default(DataRow);
                foreach (DataRow MyRow in dt.Rows)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                        MyField.FieldName = (string)MyRow["Field Name"];
                        MyField.Caption = (string)MyRow["Field Suffix"];
                        MyField.FieldNumber = (int)MyRow["Field"];
                        MyField.DisplayValue = (bool)MyRow["Display Field"];
                        MyField.FieldMaxValue = (int)MyRow["Field Max Val"];
                        MyField.FieldMinValue = (int)MyRow["Field Min Val"];
                        MyField.FieldNotes = (string)MyRow["Field Notes"];
                        MyField.FieldMaxWarnValue = (int)MyRow["Field Max Warn Val"];
                        MyField.FieldMinWarnValue = (int)MyRow["Field Min Warn Val"];
                        if (Information.IsDBNull((string)MyRow["Field Percentage Test"]) == false)
                            MyField.FieldPercentOfTest = (int)MyRow["Field Percentage Test"];
                        //dt.Columns.Add("Field Percentage Test", GetType(Double))
                        MyField.SensorID = Convert.ToInt32(this.cmbSensors.SelectedValue);
                        try
                        {
                            NewSensor.Fields.Add(MyField, MyField.FieldNumber.ToString());

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }


            }
            catch (Exception ex)
            {
            }

            List<string> Myresp = MyRem.LiveMonServer.TestSensor(NewSensor);
            // results
            if ((Myresp == null) == false)
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1 class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\"><tr><td>");
                MyHtmlStr.Append("Sensor:" + NewSensor.Caption + ":" + DateTime.Now.ToString());
                MyHtmlStr.Append("</tr></td>");
                foreach (string MyString in Myresp)
                {
                    try
                    {
                        MyHtmlStr.Append("<tr><td>");
                        MyHtmlStr.Append(MyString);
                        MyHtmlStr.Append("</td></tr>");

                    }
                    catch (Exception ex)
                    {
                    }
                }
                MyHtmlStr.Append("</td></table>");
                this.results.InnerHtml = MyHtmlStr.ToString();

            }
            else
            {
                try
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "Test Sensor Failed!";

                    cmbSensors.Focus();

                    MyRem.WriteLog("Test Sensor Failed", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = true;
                lblErr.Text = "An error has occured during save, Please try again.";

                cmbSensors.Focus();
            }
        }

        protected void btnnChangeSite_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if (Convert.ToInt32(Session["SelectedSite"]) != Convert.ToInt32(cmbSites.SelectedValue))
                {
                    int Myresp = Convert.ToInt32(MyRem.LiveMonServer.EditSensorDeviceSite(Convert.ToInt32(this.cmbSensors.SelectedValue), Convert.ToInt32(cmbSites.SelectedValue), LiveMonitoring.IRemoteLib.SDUpdateType.Sensor));
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if ((cmbLocations.SelectedValue == null) == false)
                {
                    MyRem.LiveMonServer.AddEditLocationLink(Convert.ToInt32(this.cmbSensors.SelectedValue), Convert.ToInt32(cmbLocations.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.Sensor, 99);
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnLinkAlertGroup_Click(object sender, EventArgs e)
        {
            LiveMonitoring.IRemoteLib.SensorDetails Cursensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            try
            {
                Cursensor = ReturnSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
            }
            catch (Exception ex)
            {
                return;
            }
            if ((Cursensor == null) == true)
            {
                return;
            }
            try
            {
                if ((DropDownAlertGroup.SelectedValue == null) == false)
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                    if (Convert.ToInt32(DropDownAlertGroup.SelectedValue) > 0)
                    {
                        MyRem.LiveMonServer.LinkAlertGroups(Cursensor.ID, Convert.ToInt32(DropDownAlertGroup.SelectedValue), MyUser.ID);
                    }
                    else
                    {
                        if (Cursensor.SensorGroupContacts.LinkID > 0)
                            MyRem.LiveMonServer.RemoveAlertGroupsLink(Cursensor.SensorGroupContacts.LinkID);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnEditSensor_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            
            LiveMonitoring.IRemoteLib.SensorDetails NewSensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            LiveMonitoring.IRemoteLib.SensorDetails Cursensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            Collection MyCollection = default(Collection);
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()

            try
            {
                Cursensor = ReturnSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblErr.Text = "Cannot find current sensor!";
                return;
            }
            if ((Cursensor == null) == true)
            {
                errorMessage.Visible = true;
                lblErr.Text = "Cannot find current sensor!";
                return;
            }
            switch (Convert.ToInt32(this.cmbType.SelectedValue))
            {
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BaseAudio:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraAudio:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDInput:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Output number";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Min Value.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Max Value.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Serial number";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBAllPoints:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field";
                        cmbSensors.Focus();
                        return;
                    }
                    int tev = 0;
                    if (this.txtExtraValue.Text == tev.ToString())
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBAllSoftware:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field.";
                        cmbSensors.Focus();
                        return;
                    }
                    int tev2 = 0;
                    if (this.txtExtraValue.Text == tev2.ToString())
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBHardware:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field.";
                        cmbSensors.Focus();
                        return;
                    }
                    int txv = 0;
                    if (this.txtExtraValue.Text == txv.ToString())
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBOperatingSystem:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field.";
                        cmbSensors.Focus();
                        return;
                    }
                    int txv2 = 0;
                    if (this.txtExtraValue.Text == txv2.ToString())
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        cmbSensors.Focus();
                        return;
                    }
                    object MyObject1 = null;
                    bool IsSnmp = false;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            if (this.cmbDevice.SelectedValue == (string)MyObject1)
                            {
                                IsSnmp = true;
                            }
                        }
                    }

                    if (IsSnmp == false)
                    {
                        lblErr.Visible = true;
                        lblErr.Text = "Please Select a SNMP device!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Min Value";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtMinValue2.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Max Value.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Module number.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter Register number.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    if (string.IsNullOrEmpty(this.txtExtraData.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter WMI Namespace.";
                        cmbSensors.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData1.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter WMI Property.";
                        cmbSensors.Focus();
                        return;
                    }
                    break;
            }
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply a Caption.";
                cmbSensors.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.cmbDevice.SelectedValue))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select IP Device.";
                cmbSensors.Focus();
                return;
            }
            NewSensor = Cursensor;
            bool ChangeType = false;
            if (NewSensor.Type != (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue))
            {
                ChangeType = true;
            }
            NewSensor.Type = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue);
            NewSensor.IPDeviceID = Convert.ToInt32(this.cmbDevice.SelectedValue);
            NewSensor.ModuleNo = Convert.ToInt32(this.txtModule.Text);
            NewSensor.Register = Convert.ToInt32(this.txtRegister.Text);
            NewSensor.SerialNumber = this.txtSerialNumber.Text.ToString();
            byte[] byteNormal = null;
            byte[] byteError = null;
            byte[] byteNoresponse = null;

            System.Drawing.Image imgNormal = null;
            System.Drawing.Image imgError = null;
            System.Drawing.Image imgNoresponse = null;
            if (filImageNormal.FileName.Trim().Length == 0)
            {
                byteNormal = pstrNormal;
            }
            else
            {
                imgNormal = Myfunc.Strip_Image(this.filImageNormal);
                byteNormal = MyRem.ImagetoByte(imgNormal, ImageFormat.Bmp);
            }
            if (filImageNoResponse.FileName.Trim().Length == 0)
            {
                byteNoresponse = pstrNoresponse;
            }
            else
            {
                imgNoresponse = Myfunc.Strip_Image(this.filImageNoResponse);
                byteNoresponse = MyRem.ImagetoByte(imgNoresponse, ImageFormat.Bmp);
            }
            if (filImageError.FileName.Trim().Length == 0)
            {
                byteError = pstrError;
            }
            else
            {
                imgError = Myfunc.Strip_Image(this.filImageError);
                byteError = MyRem.ImagetoByte(imgError, ImageFormat.Bmp);
            }

            NewSensor.Caption = this.txtCaption.Text;
            NewSensor.ImageNormal = imgNormal;
            NewSensor.ImageNormalByte = byteNormal;
            NewSensor.ImageNoResponse = imgNoresponse;
            NewSensor.ImageNoResponseByte = byteNoresponse;
            NewSensor.ImageError = imgError;
            NewSensor.ImageErrorByte = byteError;
            NewSensor.MinValue = Convert.ToDouble(this.txtMinValue2.Text);
            NewSensor.MaxValue = Convert.ToDouble(this.txtMaxValue.Text);
            NewSensor.Multiplier = Convert.ToDouble(this.txtMultiplier.Text);
            NewSensor.Divisor = Convert.ToDouble(this.txtDivisor.Text);
            if (Convert.ToDouble(this.txtScanRate.Text) > 0 & Convert.ToDouble(this.txtScanRate.Text) < 5000)
            {
                NewSensor.ScanRate = 5000;
            }
            else
            {
                NewSensor.ScanRate = Convert.ToDouble(this.txtScanRate.Text);
            }
            NewSensor.ExtraData = (this.txtExtraData.Text);
            NewSensor.ExtraData1 = (this.txtExtraData1.Text);
            NewSensor.ExtraData2 = (this.txtExtraData2.Text);
            NewSensor.ExtraData3 = (this.txtExtraData3.Text);
            NewSensor.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
            NewSensor.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
            try
            {
                NewSensor.SensGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
                NewSensor.SensGroup.SensorGroupID = Convert.ToInt32(cmbSensGroup.SelectedValue);
                NewSensor.SensGroup.SensorGroupName = cmbSensGroup.Items[cmbSensGroup.SelectedIndex].Text.Trim();

            }
            catch (Exception ex)
            {
            }
            if (cmbSensOutput.Visible)
            {
                try
                {
                    NewSensor.OutputType = (LiveMonitoring.IRemoteLib.SensorDetails.OutputTypeDef)Convert.ToInt32(cmbSensOutput.SelectedValue);
                }
                catch (Exception ex)
                {
                }
            }
            try
            {
                if ((Session["SelectedSite"] == null) == false)
                {
                    NewSensor.Add2Site = Convert.ToInt32(cmbSites.SelectedValue);
                }
            }
            catch (Exception ex)
            {
            }
            bool Myresp = MyRem.LiveMonServer.EditSensor(NewSensor);
            if (Myresp)
            {
                try
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ShownotyBehindScript", "ShownotyBehind('Sensor Edited" + DateTime.Now.ToString() + " Caption:" + NewSensor.Caption + " ID:" + Myresp.ToString() + "','success','centerRight','defaultTheme');", true);
                }
                catch (Exception ex)
                {
                }
                DataTable dt = new DataTable();
                
                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                }
                try
                {
                    foreach (DataRow MyRow in dt.Rows)
                    {
                        try
                        {
                            LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                            MyField.FieldName = (string)MyRow["Field Name"];
                            MyField.Caption = (string)MyRow["Field Suffix"];
                            MyField.FieldNumber = (int)MyRow["Field"];
                            MyField.DisplayValue = (bool)MyRow["Display Field"];
                            MyField.FieldMaxValue = (int)MyRow["Field Max Val"];
                            MyField.FieldMinValue = (int)MyRow["Field Min Val"];
                            MyField.FieldNotes = (string)MyRow["Field Notes"];
                            MyField.FieldMaxWarnValue = (int)MyRow["Field Max Warn Val"];
                            MyField.FieldMinWarnValue = (int)MyRow["Field Min Warn Val"];
                            if (Information.IsDBNull(MyRow["Field Percentage Test"]) == false)
                                MyField.FieldPercentOfTest = (int)MyRow["Field Percentage Test"];

                            MyField.SensorID = Convert.ToInt32(this.cmbSensors.SelectedValue);
                            try
                            {
                                MyRem.LiveMonServer.EditSensorField(MyField);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                try
                {
                    if ((DropDownAlertGroup.SelectedValue == null) == false)
                    {
                        if (Convert.ToInt32(DropDownAlertGroup.SelectedValue) > 0)
                        {
                            MyRem.LiveMonServer.LinkAlertGroups(NewSensor.ID, Convert.ToInt32(DropDownAlertGroup.SelectedValue), MyUser.ID);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                
                ///Edit Succeeded
                try
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "EditSensor Succeeded!";
                    cmbSensors.Focus();
                    MyRem.WriteLog("EditSensor Succeeded", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());
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
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ShownotyBehindScript", "ShownotyBehind('Sensor Not Edited Error !" + DateTime.Now.ToString() + " Caption:" + NewSensor.Caption + "','error','centerRight','defaultTheme');", true);
                }
                catch (Exception ex)
                {
                }

                ///Edit Sensor Failed
                try
                {
                    errorMessage.Visible = true;
                    lblErr.Text = "EditSensor Failed!";
                    cmbSensors.Focus();
                    MyRem.WriteLog("EditSensor Failed", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());
                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = true;
                lblErr.Text = "An error has occured during save, Please try again!";
                cmbSensors.Focus();
            }

        }


        public void ClearVals()
        {
            int zero = 0;
            int fivethousand = 5000;

            this.txtModule.Text = zero.ToString();
            this.txtRegister.Text = zero.ToString();
            this.txtSerialNumber.Text = "";
            this.txtCaption.Text = "";
            this.txtMinValue2.Text = zero.ToString();
            this.txtMaxValue.Text = zero.ToString();
            this.txtMultiplier.Text = zero.ToString();
            this.txtDivisor.Text = zero.ToString();
            this.txtScanRate.Text = fivethousand.ToString();
        }

        private class ListItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListItem a = (ListItem)x;
                ListItem b = (ListItem)y;
                CaseInsensitiveComparer c = new CaseInsensitiveComparer();
                return c.Compare(a.Text, b.Text);
            }
        }

        protected void btnDeleteSensor_Click(object sender, EventArgs e)
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
            LiveMonitoring.IRemoteLib.SensorDetails Cursensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            try
            {
                Cursensor = ReturnSpecificSensor(Convert.ToInt32(this.cmbSensors.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblErr.Text = "An error has occured during Delete, Please try again!";

                cmbSensors.Focus();


                return;
                //error cannot find current sensor
            }
            if ((Cursensor == null) == false)
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                //Dim Myfunc As New LiveMonitoring.SharedFuncs

                LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                int MyCnt = 0;
                for (MyCnt = 1; MyCnt <= Cursensor.Fields.Count - 1; MyCnt++)
                {
                    MyFields = (LiveMonitoring.IRemoteLib.SensorFieldsDef)Cursensor.Fields[1];
                    MyRem.LiveMonServer.DeleteSensorField(MyFields.ID);
                }
                if (MyRem.LiveMonServer.DeleteSensor(Cursensor.ID))
                {
                    //what now

                    try
                    {
                        successMessage.Visible = true;
                        lblSucces.Text = "DeleteSensor Succeeded!";

                        cmbSensors.Focus();
                        MyRem.WriteLog("DeleteSensor Succeeded", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());

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
                        lblErr.Text = "DeleteSensor Failed!";

                        cmbSensors.Focus();

                        MyRem.WriteLog("DeleteSensor Failed", "User:" + MyUser.ID.ToString() + "|" + Cursensor.ToString());

                    }
                    catch (Exception ex)
                    {
                    }

                    errorMessage.Visible = true;
                    lblErr.Text = "An error has occured during Delete, Please try again!";

                    cmbSensors.Focus();

                }
            }
        }
    }
}
