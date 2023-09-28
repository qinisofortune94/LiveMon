using LiveMonitoring;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SensorStatus : System.Web.UI.Page, ICallbackEventHandler
    {

        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();

        private string _callbackArg;
        public string MyTable;
        public string MyCMD = "";
        private Collection MyCollection = new Collection();
        private Collection MyCameraCollection = new Collection();
        private Collection MySensorCollection = new Collection();
        private Collection MySensorGroupCollection = new Collection();
        private Collection MyIPDevicesCollection = new Collection();
        private Collection MyOtherDevicesCollection = new Collection();
        private Collection MySNMPDevicesCollection = new Collection();

        private Collection MySensorCollection2 = new Collection();

        private Collection MyCollection2 = new Collection();
        private LiveMonitoring.GlobalRemoteVars MyRemObj = new LiveMonitoring.GlobalRemoteVars();


        private static Collection MySensorsCol = new Collection();
        private LiveMonitoring.GlobalRemoteVars MyRemm = new LiveMonitoring.GlobalRemoteVars();
        public DateTime LastRefresh;

        List<string> oListXAxis = new List<string>();
        List<object> oListYAxis = new List<object>();

        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        string conStrReport = WebConfigurationManager.ConnectionStrings["IPMonConnectionStringReport"].ToString();
        LiveMonitoring.testing test = new LiveMonitoring.testing();
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
                DropDownList sites = this.Master.FindControl("cmbCurrentSite") as DropDownList;
                sites.Visible = false;
                
                if (IsPostBack == false)
                {
                    try
                    {
                        if (Convert.ToBoolean(MyDataAccess.GetAppSetting("BySite")) == true)
                        {
                            if (MyUser.UserSites.Count > 1)
                            {
                                cmbCurrentSite.Visible = true;
                                cmbCurrentSite.BorderColor = System.Drawing.Color.SeaGreen;
                            }
                            cmbCurrentSite.Items.Clear();
                            Sites RetSites = new Sites(MyUser.ID);

                            List<MySite> MySitesList = new List<MySite>();
                            bool firstitem = true;
                            foreach (Sites.Site MySiteID in RetSites.SitesList)
                            {
                                try
                                {
                                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                    MyItem.Text = MySiteID.SiteObj.SiteName;
                                    //MySiteID.SiteName
                                    MyItem.Value = MySiteID.SiteObj.SiteID.ToString();

                                    if ((Session["SelectedSite"] == null) == false)
                                    {
                                        if (Convert.ToInt32(Session["SelectedSite"]) == MySiteID.SiteObj.SiteID)
                                        {
                                            MyItem.Selected = true;
                                        }
                                    }
                                    else
                                    {
                                        if (firstitem)
                                        {
                                            Session["SelectedSite"] = MySiteID.SiteObj.SiteID;
                                            MyItem.Selected = true;
                                            firstitem = false;
                                        }
                                    }
                                    cmbCurrentSite.Items.Add(MyItem);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            try
                            {
                                test.SortDropDown(cmbCurrentSite);
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

                //////Display Sensor Status/////////////////////////
                displayStatusOfSensor();
            }
            else
            {
                Response.Redirect("Index.aspx");
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

        public class MySite
        {
            public int siteID { get; set; }
            public string siteName { get; set; }
        }

        protected void cmbCurrentSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SelectedSite"] = cmbCurrentSite.SelectedValue;
        }

        public void displayStatusOfSensor()
        {
            this.Response.Expires = 1;
            System.Web.UI.ClientScriptManager cm = Page.ClientScript;
            string cbReference = null;
            cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            string callbackScript = null;
            callbackScript = "function CallServer(arg, context)" + "{" + cbReference + "; }";
            cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            if (IsCallback == false)
            {
                if (IsPostBack == false)
                {
                    Fill_Grid();
                    LastRefresh = DateAndTime.Now;
                    Session["LastRefresh"] = LastRefresh;
                    Session["MyGrid"] = MyTable;
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    ReFill_Grid();
                    LastRefresh = DateAndTime.Now;
                    Session["LastRefresh"] = LastRefresh;
                    Session["MyGrid"] = MyTable;
                }

                LastRefresh = DateAndTime.Now;
            }
        }

        public void Fill_Grid()
        {
            try
            {
                //Hold all the sensor ID's
                System.Collections.Generic.List<int> selectSensors = new System.Collections.Generic.List<int>();
                try
                {
                    // Are there any sensors?
                    if ((Session["SelectSensors"] == null) == false)
                    {
                        //Yes.
                        selectSensors = (System.Collections.Generic.List<int>)Session["SelectSensors"];
                    }
                }
                catch (Exception ex)
                {
                }

                //GetServerObjects 'server1.GetAll()
                MyCollection = MyRemm.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                object MyObject1 = null;

                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                MyCameraCollection.Add(MyCamera);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                bool isAdded = false;
                                //Ok
                                if (chkFilter_OK.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.ok)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        isAdded = true;
                                        //count2 = count2 + 1;
                                    }
                                }
                                //error
                                if (chkFilter_Error.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.criticalerror)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        isAdded = true;
                                        //count = count + 1;
                                    }
                                }
                                //No response
                                if (chkFilter_NoResponse.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.noresponse)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        isAdded = true;
                                        //count++;
                                    }
                                }
                                //alert
                                if (chkFilter_Alert.Checked & isAdded == false)
                                {
                                    if (CheckFieldAlert(MySensor))
                                    {
                                        MySensorCollection.Add(MySensor);
                                        isAdded = true;
                                        //count = count + 1;
                                    }
                                }
                                //warning
                                if (chkFilter_Warning.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.statuserror)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        isAdded = true;
                                    }
                                }

                                if (chkFilter_Unknown.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.disabled)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        isAdded = true;
                                    }
                                }

                                if (chkFilter_SensorWarning.Checked & isAdded == false)
                                {
                                    //LiveMonitoring.IRemoteLib.FieldStatusDef myFields = new LiveMonitoring.IRemoteLib.FieldStatusDef();
                                    foreach (LiveMonitoring.IRemoteLib.FieldStatusDef myFields in MySensor.Fields)
                                    {
                                        if (myFields == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                            }

                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                            {
                                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                                MySensorGroupCollection.Add(MySensorGroup, MySensorGroup.SensorGroupID.ToString());
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                                MyIPDevicesCollection.Add(MyIPDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                                MyOtherDevicesCollection.Add(MyOtherDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                            {
                                LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                                MySNMPDevicesCollection.Add(MySNMPDevicesDetails);
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                Collection MyFilteredSensors = new Collection();//= MySensorCollection

                for (int MyCntSensor = 1; MyCntSensor <= MySensorCollection.Count; MyCntSensor++)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensObj1 = (LiveMonitoring.IRemoteLib.SensorDetails)MySensorCollection[MyCntSensor];
                    MyFilteredSensors.Add(MySensObj1);
                }

                try
                {
                    if (selectSensors.Count > 0)
                    {
                        MyFilteredSensors.Clear();
                        //LiveMonitoring.IRemoteLib.SensorDetails MySensObj = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensObj in MySensorCollection)
                        {
                            try
                            {
                                if (selectSensors.Contains(MySensObj.ID))
                                {
                                    MyFilteredSensors.Add(MySensObj);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        MyFilteredSensors = MySensorCollection;
                    }

                }
                catch (Exception ex)
                {
                }


                LiveMonitoring.IRemoteLib.SensorDetails MyCurSensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                string MyDeviceName = "";
                //BackColor="#C1D2EE" BorderColor="#316AC5"
                // MyTable = "<table BorderColor=""#316AC5""><tr bgcolor=""#C1D2EE""><td>Icon</td><td>Device</td><td>Sensor</td><td>Field</td><td>Alert</td><td>Value</td><td>Extra Value</td></tr>"
                MyTable = "<table id=\"myTable\" runat=\"server\" class=\"table table-hover text-nowrap\" BorderColor=\"#316AC5\"><thead><tr class=\"headings\"><td><font size=4>Icon</font></td><td><font size=4>Device</font></td><td><font size=4>Sensor</font></td><td><font size=4>Field</font></td><td><font size=4>Status</font></td><td><font size=4>Value</font></td><td><font size=4></font></td></tr></thead><tbody>";
                MaxNo.Value = MyFilteredSensors.Count.ToString();

                //MySensorCollection.Count
                for (int MyCntSensor = 1; MyCntSensor <= MyFilteredSensors.Count; MyCntSensor++)
                {
                    try
                    {
                        if (MyCntSensor >= Convert.ToInt32(StartNo.Value) & MyCntSensor <= Convert.ToInt32(EndNo.Value))
                        {
                            MyCurSensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyFilteredSensors[MyCntSensor];
                            MyDeviceName = "";
                            //LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
                            foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails in MyIPDevicesCollection)
                            {
                                if (MyCurSensor.IPDeviceID == MyIPDevicesDetails.ID)
                                {
                                    MyDeviceName = MyIPDevicesDetails.Caption;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
                                foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails in MyOtherDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyOtherDevicesDetails.ID)
                                    {
                                        MyDeviceName = MyOtherDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
                                foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails in MySNMPDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MySNMPDevicesDetails.ID)
                                    {
                                        MyDeviceName = MySNMPDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.CameraDetails MyCamera = default(LiveMonitoring.IRemoteLib.CameraDetails);
                                foreach (LiveMonitoring.IRemoteLib.CameraDetails MyCamera in MyCameraCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyCamera.ID)
                                    {
                                        MyDeviceName = MyCamera.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MyCurSensor.Fields)
                            {
                                try
                                {
                                    if (MyFields.DisplayValue)
                                    {
                                        if (MyCurSensor.LastErrors.Count > 0)
                                        {
                                            AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus, MyCurSensor.LastErrors.Peek());
                                        }
                                        else
                                        {
                                            AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblerr.Visible = true;
                                    lblerr.Text = ex.Message;
                                    AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }
                MyTable += "</table>";
            }
            catch (Exception ex)
            {
                lblerr.Visible = true;
                lblerr.Text = ex.Message;
            }
        }

        private bool CheckFieldAlert(LiveMonitoring.IRemoteLib.SensorDetails MySensor)
        {
            try
            {
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef Myfield in MySensor.Fields)
                {
                    try
                    {
                        if (Myfield.FieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                        {
                            return true;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool CheckFieldWarning(LiveMonitoring.IRemoteLib.SensorDetails MySensor)
        {
            try
            {
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef Myfield in MySensor.Fields)
                {
                    try
                    {
                        if (Myfield.FieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                        {
                            return true;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool doesSearchTermMatchCaption(string SearchTerm, string Caption)
        {
            if (Caption.ToUpper().Contains(SearchTerm.ToUpper()))
            {
                return true;
            }
            return false;
        }

        public void AddValues(string MySensorID, string MyDevice, string MyField, string MySensor, LiveMonitoring.IRemoteLib.StatusDef MyStatus, string MyValue, string MyOtherValue, LiveMonitoring.IRemoteLib.FieldStatusDef MyFieldStatus, string tooltip = "")
        {
            try
            {
                //Me.GridStatus.Rows.Add()
                string MyAlert = null;
                string MyColor = null;
                //Black Gray 		Silver 		White
                //Yellow 		Lime 		Aqua 		Fuchsia
                //Red 		Green 		Blue 		Purple
                //Maroon 		Olive 		Navy 		Teal
                switch (MyStatus)
                {
                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                        MyAlert = "Alert";
                        MyColor = "Red";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.criticalerror:
                        MyAlert = "Critical Error";
                        MyColor = "#8B0000";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.devicefailure:
                        MyAlert = "Device failure";
                        MyColor = "Fuchsia";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.disabled:
                        MyAlert = "Disabled";
                        MyColor = "Gray";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                        MyAlert = "No Response";
                        MyColor = "Purple";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.notify:
                        MyAlert = "Notify";
                        MyColor = "Navy";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                        MyAlert = "OK";
                        MyColor = "Green";

                        if (MyFieldStatus != LiveMonitoring.IRemoteLib.FieldStatusDef.ok)
                        {
                            if (MyFieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                            {
                                MyAlert = "Sensor Warning";
                                MyColor = "Orange";
                            }
                            else if (MyFieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                            {
                                MyAlert = "Sensor Alert";
                                MyColor = "Red";
                            }
                        }
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.statuserror:
                        MyColor = "#DC143C";
                        MyAlert = "Error";
                        break;
                    default:
                        MyAlert = "Unknown";
                        MyColor = "White";
                        break;
                }

                if (!string.IsNullOrEmpty(tooltip))
                {
                    MyTable += "<tr>";
                    //<a href=""DisplayGraphs.aspx?SensorNum=" & MyImage & """ target='main' title=""" + Server.HtmlEncode(tooltip) + """>"
                }
                else
                {
                    MyTable += "<tr>";
                    //<a href=""DisplayGraphs.aspx?SensorNum=" & MyImage & """ target='main'>"
                }
                MyTable += "<td>";
                //<img src='ReturnThumbnailImage.aspx?Sensor=" & MyImage & "' height=12 width=12>"
                MyTable += "</td><td><font size=2>" + Server.HtmlEncode(MyDevice);
                if (!string.IsNullOrEmpty(tooltip))
                {
                    MyTable += "</font></td><td><font size=2><span title=\"Last Err:" + Server.HtmlEncode(tooltip) + "\">" + Server.HtmlEncode(MySensor);
                }
                else
                {
                    //MyTable += "</font></td><td><font size=2 color=\"#000000\"><span>" + Server.HtmlEncode(MySensor);
                    MyTable += "</font></td><td><font size=2><span>" + Server.HtmlEncode(MySensor);
                }
                MyTable += "</span></font></td><td><font size=2>" + Server.HtmlEncode(MyField);
                MyTable += "</font></td><td><font size=2><mark style=\"background-color: " + MyColor + ";color: white;border-radius: 36px 50px 50px 36px / 12px 30px 30px 12px\">" + Server.HtmlEncode(MyAlert);
                MyTable += "</mark></font></td><td><font size=2>" + Server.HtmlEncode(MyValue);
                //MyTable += "</font></td><td><font size=2>" + Server.HtmlEncode(MyOtherValue);
                MyTable += "</font></td><td><font size=2>" + "<a href=\"SensorDetails.aspx?SensorID=" + MySensorID + "\" style=\"color:#00BFFF\"> Details </a>";
                MyTable += "</font></td>";
                MyTable += "</tr>";
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnNext20_Click(object sender, System.EventArgs e)
        {
            if (Convert.ToInt32(EndNo.Value) < Convert.ToInt32(MaxNo.Value))
            {
                StartNo.Value = (Convert.ToInt32(StartNo.Value) + 20).ToString();
                EndNo.Value = (Convert.ToInt32(EndNo.Value) + 20).ToString();
            }
            else
            {
                StartNo.Value = Convert.ToInt32(0).ToString();
                EndNo.Value = Convert.ToInt32(20).ToString();
            }
            Fill_Grid();
            Session["MyGrid"] = MyTable;
        }

        protected void btnPrev20_Click(object sender, System.EventArgs e)
        {
            if (Convert.ToInt32(StartNo.Value) > 0)
            {
                StartNo.Value = (Convert.ToInt32(StartNo.Value) - 20).ToString();
                EndNo.Value = (Convert.ToInt32(EndNo.Value) - 20).ToString();
            }
            else
            {
                StartNo.Value = Convert.ToInt32(0).ToString();
                EndNo.Value = Convert.ToInt32(20).ToString();
            }
            Fill_Grid();
            Session["MyGrid"] = MyTable;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            MyTable = "";
            ReFill_Grid();
            Session["MyGrid"] = MyTable;
        }

        public void ReFill_Grid()
        {
            try
            {
                //Hold all the sensor ID's
                System.Collections.Generic.List<int> selectSensors = new System.Collections.Generic.List<int>();
                try
                {
                    // Are there any sensors?
                    if ((Session["SelectSensors"] == null) == false)
                    {
                        //Yes.
                        selectSensors = (System.Collections.Generic.List<int>)Session["SelectSensors"];
                    }

                }
                catch (Exception ex)
                {
                }

                //GetServerObjects 'server1.GetAll()
                MyCollection = MyRemm.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                object MyObject1 = null;
                bool HasSearchTerm = false;
                if ((!(txtSearch.Text == null)) | (txtSearch.Text.Length > 0))
                {
                    HasSearchTerm = true;
                }
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                if (HasSearchTerm)
                                {
                                    if (doesSearchTermMatchCaption(txtSearch.Text, MyCamera.Caption))
                                    {
                                        MyCameraCollection.Add(MyCamera);
                                    }
                                }
                                else
                                {
                                    MyCameraCollection.Add(MyCamera);
                                }
                            }

                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                bool isAdded = false;
                                //Ok
                                if (chkFilter_OK.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.ok)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //error
                                if (chkFilter_Error.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.criticalerror)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //No response
                                if (chkFilter_NoResponse.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.noresponse)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //alert
                                if (chkFilter_Alert.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.alert)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //warning
                                if (chkFilter_Warning.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.statuserror)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }

                                if (chkFilter_Unknown.Checked & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.disabled)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }

                                // Sensor Warning.
                                if (chkFilter_SensorWarning.Checked & isAdded == false)
                                {
                                    //LiveMonitoring.IRemoteLib.FieldStatusDef myFields = new LiveMonitoring.IRemoteLib.FieldStatusDef();
                                    foreach (LiveMonitoring.IRemoteLib.FieldStatusDef myFields in MySensor.Fields)
                                    {
                                        if (myFields == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                                        {
                                            if (HasSearchTerm)
                                            {
                                                if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                                {
                                                    MySensorCollection.Add(MySensor);
                                                    isAdded = true;
                                                }
                                            }
                                            else
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                    }
                                }

                                if (chkFilter_SensorAlert.Checked & isAdded == false)
                                {
                                    //LiveMonitoring.IRemoteLib.FieldStatusDef myFields = new LiveMonitoring.IRemoteLib.FieldStatusDef();
                                    foreach (LiveMonitoring.IRemoteLib.FieldStatusDef myFields in MySensor.Fields)
                                    {
                                        if (myFields == LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                                        {
                                            if (HasSearchTerm)
                                            {
                                                if (doesSearchTermMatchCaption(txtSearch.Text, MySensor.Caption))
                                                {
                                                    MySensorCollection.Add(MySensor);
                                                    isAdded = true;
                                                }
                                            }
                                            else
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                    }
                                }

                            }

                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                            {
                                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;

                                MySensorGroupCollection.Add(MySensorGroup, MySensorGroup.SensorGroupID.ToString());
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                                MyIPDevicesCollection.Add(MyIPDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                                MyOtherDevicesCollection.Add(MyOtherDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                            {
                                LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                                MySNMPDevicesCollection.Add(MySNMPDevicesDetails);
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }

                Collection MyFilteredSensors = new Collection();

                try
                {
                    if (selectSensors.Count > 0)
                    {
                        MyFilteredSensors.Clear();
                        //LiveMonitoring.IRemoteLib.SensorDetails MySensObj = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensObj in MySensorCollection)
                        {
                            try
                            {
                                if (selectSensors.Contains(MySensObj.ID))
                                {
                                    MyFilteredSensors.Add(MySensObj);
                                }

                            }
                            catch (Exception ex)
                            {
                            }

                        }
                    }
                    else
                    {
                        MyFilteredSensors = MySensorCollection;
                    }

                }
                catch (Exception ex)
                {
                }


                LiveMonitoring.IRemoteLib.SensorDetails MyCurSensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                string MyDeviceName = "";
                //BackColor="#C1D2EE" BorderColor="#316AC5"
                MyTable = "<table id=\"myTable\" runat=\"server\" class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\"><thead><tr class=\"headings\"><td><font size=4>Icon</font></td><td><font size=4>Device</font></td><td><font size=4>Sensor</font></td><td><font size=4>Field</font></td><td><font size=4>Status</font></td><td><font size=4>Value</font></td><td><font size=4></font></td></tr></thead><tbody>";
                MaxNo.Value = MyFilteredSensors.Count.ToString();
                if (MyFilteredSensors.Count > 0)
                {

                    //MySensorCollection.Count
                    for (int MyCntSensor = 1; MyCntSensor <= MyFilteredSensors.Count; MyCntSensor++)
                    {
                        try
                        {
                            if (MyCntSensor >= Convert.ToInt32(StartNo.Value) & MyCntSensor <= Convert.ToInt32(EndNo.Value))
                            {
                                MyCurSensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyFilteredSensors[MyCntSensor];
                                MyDeviceName = "";
                                //LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
                                foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails in MyIPDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyIPDevicesDetails.ID)
                                    {
                                        MyDeviceName = MyIPDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                                if (string.IsNullOrEmpty(MyDeviceName))
                                {
                                    //LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
                                    foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails in MyOtherDevicesCollection)
                                    {
                                        if (MyCurSensor.IPDeviceID == MyOtherDevicesDetails.ID)
                                        {
                                            MyDeviceName = MyOtherDevicesDetails.Caption;
                                            break; // TODO: might not be correct. Was : Exit For
                                        }
                                    }
                                }
                                if (string.IsNullOrEmpty(MyDeviceName))
                                {
                                    //LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
                                    foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails in MySNMPDevicesCollection)
                                    {
                                        if (MyCurSensor.IPDeviceID == MySNMPDevicesDetails.ID)
                                        {
                                            MyDeviceName = MySNMPDevicesDetails.Caption;
                                            break; // TODO: might not be correct. Was : Exit For
                                        }
                                    }
                                }
                                if (string.IsNullOrEmpty(MyDeviceName))
                                {
                                    //LiveMonitoring.IRemoteLib.CameraDetails MyCamera = default(LiveMonitoring.IRemoteLib.CameraDetails);
                                    foreach (LiveMonitoring.IRemoteLib.CameraDetails MyCamera in MyCameraCollection)
                                    {
                                        if (MyCurSensor.IPDeviceID == MyCamera.ID)
                                        {
                                            MyDeviceName = MyCamera.Caption;
                                            break; // TODO: might not be correct. Was : Exit For
                                        }
                                    }
                                }
                                //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MyCurSensor.Fields)
                                {
                                    try
                                    {
                                        if (MyFields.DisplayValue)
                                        {
                                            if (MyCurSensor.LastErrors.Count > 0)
                                            {
                                                AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus, MyCurSensor.LastErrors.Peek());

                                            }
                                            else
                                            {
                                                AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lblerr.Visible = true;
                                        lblerr.Text = ex.Message;
                                        AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    //If there are no sensors found
                    MyTable += "<tr><td colspan=\"5\" style=\"color:red\">No Sensors found.</td></tr>";
                }
                MyTable += "</tbody></table>";
            }
            catch (Exception ex)
            {
                lblerr.Visible = true;
                lblerr.Text = ex.Message;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ReFill_Grid();
                Session["MyGrid"] = MyTable;
            }
            catch (Exception ex)
            {
            }
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            _callbackArg = eventArgument;
        }

        public string GetCallbackResult()
        {
            LastRefresh = Convert.ToDateTime(Session["LastRefresh"]);
            switch (_callbackArg)
            {
                case "time":
                    return tool.My.MyProject.Computer.Clock.LocalTime.ToString();
                case "mem":
                    return (tool.My.MyProject.Computer.Info.AvailablePhysicalMemory / 1024).ToString() + "K";
                case "Fill_Grid":
                    if (DateAndTime.DateDiff(DateInterval.Second, LastRefresh, DateAndTime.Now) >= 20)
                    {
                        // Fill_Grid()
                        ReFill_Grid();
                        LastRefresh = DateAndTime.Now;
                        Session["LastRefresh"] = LastRefresh;
                        Session["MyGrid"] = MyTable;
                    }
                    else
                    {
                        return Session["MyGrid"].ToString();
                    }
                    //Me.r
                    return MyTable;
                default:
                    return _callbackArg;
            }

        }

        /////////////////////Export data to excel when this button is clicked/////////////////////
        protected void export_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String selectedsite = "Sensor Details For ";
            if (cmbCurrentSite.SelectedItem != null)
            {
                selectedsite += cmbCurrentSite.SelectedItem.ToString();
            }
            else
            {
                selectedsite += "thissite";
            }

            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename= " + selectedsite + " " + now + ".xls");
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Sensor Data</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");
            //Form form1 = this.Master.FindControl("form1") as Form;
            //form1.InnerHtml = MyTable; // give ur html string here
            string style = @"<style> TABLE { border: 1px solid gray; } TD { border: 1px solid gray; } </style> ";
            Response.Write(style);
            Response.Write("</head>");
            Response.Flush();
        }
    }
}