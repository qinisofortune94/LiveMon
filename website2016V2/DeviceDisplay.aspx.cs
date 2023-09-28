using LiveMonitoring;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class DeviceDisplay : System.Web.UI.Page
    {
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private string _callbackArg;
        public string MyTable;
        public string MyCMD = "";
        private Collection MyCollection = new Collection();
        private List<LiveMonitoring.IRemoteLib.SiteDetails> RetSites = new List<LiveMonitoring.IRemoteLib.SiteDetails>();
        private List<LiveMonitoring.IRemoteLib.CameraDetails> MyCameraCollection = new List<LiveMonitoring.IRemoteLib.CameraDetails>();
        private List<LiveMonitoring.IRemoteLib.SensorDetails> MySensorCollection = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
        private List<LiveMonitoring.IRemoteLib.SensorGroup> MySensorGroupCollection = new List<LiveMonitoring.IRemoteLib.SensorGroup>();
        private List<LiveMonitoring.IRemoteLib.IPDevicesDetails> MyIPDevicesCollection = new List<LiveMonitoring.IRemoteLib.IPDevicesDetails>();
        private List<LiveMonitoring.IRemoteLib.OtherDevicesDetails> MyOtherDevicesCollection = new List<LiveMonitoring.IRemoteLib.OtherDevicesDetails>();
        private List<LiveMonitoring.IRemoteLib.SNMPManagerDetails> MySNMPDevicesCollection = new List<LiveMonitoring.IRemoteLib.SNMPManagerDetails>();
        private LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
        public DateTime LastRefresh;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
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


                    if ((Request.UserAgent.ToLower().Contains("konqueror") == false))
                    {
                        if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("gzip")))
                        {
                            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress, true);
                            Response.AppendHeader("Content-encoding", "gzip");

                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("deflate")))
                            {
                                Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress, true);
                                Response.AppendHeader("Content-encoding", "deflate");
                            }
                        }
                    }

                    this.Response.Expires = 1;
                    if (IsCallback == false)
                    {
                        if (IsPostBack == false)
                        {
                            Fill_List();
                        }
                        else
                        {
                            string CtrlID = string.Empty;
                            if (Request.Form["__EVENTTARGET"] != null & Request.Form["__EVENTTARGET"] != string.Empty)
                            {
                                CtrlID = Request.Form["__EVENTTARGET"];
                            }
                            else
                            {
                                if (Request.Form[hidSourceID.UniqueID] != null & Request.Form[hidSourceID.UniqueID] != string.Empty)
                                {
                                    CtrlID = Request.Form[hidSourceID.UniqueID];
                                }
                            }
                            Fill_FilteredList();
                        }

                        LastRefresh = DateAndTime.Now;
                    }
                    else
                    {
                    }
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
        }

        //public class MySite
        //{
        //    public int siteID { get; set; }
        //    public string siteName { get; set; }
        //}

        //protected void cmbCurrentSite_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Session["SelectedSite"] = cmbCurrentSite.SelectedValue;
        //}

        public void ClearLists()
        {
            try
            {
                MyCollection.Clear();
                RetSites.Clear();
                MyCameraCollection.Clear();
                MySensorCollection.Clear();
                MySensorGroupCollection.Clear();
                MyIPDevicesCollection.Clear();
                MyOtherDevicesCollection.Clear();
                MySNMPDevicesCollection.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        public void Fill_List()
        {
            try
            {
                ClearLists();
                //Hold all the sensor ID's
                System.Collections.Generic.List<int> selectSensors = new System.Collections.Generic.List<int>();

                try
                {
                    // Are there any sensors?
                    if ((Session["SelectSensors"] == null) == false)
                    {
                        selectSensors = (System.Collections.Generic.List<int>)Session["SelectSensors"];
                    }

                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message;
                }

                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                object MyObject1 = null;
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
                            {
                                LiveMonitoring.IRemoteLib.SiteDetails MyObject6 = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
                                try
                                {
                                    RetSites.Add(MyObject6);
                                }
                                catch (Exception ex)
                                {
                                    //Trace.Write("Get All err  MySitesList:" + ex.Message)
                                }
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                MyCameraCollection.Add(MyCamera);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                MySensorCollection.Add(MySensor);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                            {
                                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                                MySensorGroupCollection.Add(MySensorGroup);
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
                            errorMessage.Visible = true;
                            lblError.Text = ex.Message;
                        }

                    }

                    try
                    {
                        FillSites(MyCollection);
                        Session["SelectObjects"] = MyCollection;
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = ex.Message;
                    }
                }
               
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        public void FillSites(Collection ReturnedObjects)
        {
            Collection addedSites = new Collection();
            Collection HiddenSites = new Collection();
            if ((Session["HiddenSites"] == null) == false)
            {
                HiddenSites = (Collection)Session["HiddenSites"];
            }

            foreach (LiveMonitoring.IRemoteLib.SiteDetails MySiteObj_loopVariable in RetSites)
            {
                //MySiteObj = MySiteObj_loopVariable;
                try
                {
                    if (addedSites.Contains(MySiteObj_loopVariable.ID.ToString()) == false)
                    {
                        Control oCtrlDemo = new Control();
                        oCtrlDemo = LoadControl("SiteContainer.ascx"); 
                        oCtrlDemo.ID = "SiteID" + MySiteObj_loopVariable.ID.ToString();
                        Type ucType = oCtrlDemo.GetType(); 
                        PropertyInfo ucsetID = ucType.GetProperty("LoadSiteDetails");

                        EventInfo ucsetEventD = ucType.GetEvent("RefreshPage");
                        Type hType = ucsetEventD.EventHandlerType;
                        Delegate d = Delegate.CreateDelegate(hType, this, "Fill_FilteredList");
                        ucsetEventD.AddEventHandler(oCtrlDemo, d);
                        ucsetID.SetValue(oCtrlDemo, MySiteObj_loopVariable, null);
                        displayGroups.Controls.Add(oCtrlDemo);
                        addedSites.Add(MySiteObj_loopVariable.ID, MySiteObj_loopVariable.ID.ToString());
                        //now add devices and sensors in this site
                        try
                        {
                            PropertyInfo ucsetSensor = ucType.GetProperty("setID");
                            ucsetSensor.SetValue(oCtrlDemo, MySiteObj_loopVariable.ID, null);
                        }
                        catch (Exception ex)
                        {
                        }
                        if (HiddenSites.Contains(oCtrlDemo.ID) == false)
                        {
                            try
                            {
                                foreach (object MyObject1_loopVariable in ReturnedObjects)
                                {
                                   // MySiteObj_loopVariable = MyObject1_loopVariable;
                                    try
                                    {
                                        if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.SensorDetails)
                                        {
                                            if (MySiteObj_loopVariable.SiteSensors.Contains(((LiveMonitoring.IRemoteLib.SensorDetails)MyObject1_loopVariable).ID))
                                            {
                                                PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                                                ucsetSensor.SetValue(oCtrlDemo, (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1_loopVariable, null);
                                            }
                                        }
                                        else if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                                        {
                                            if (MySiteObj_loopVariable.SiteIPDevices.Contains(((LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1_loopVariable).ID))
                                            {
                                                PropertyInfo ucsetSensor = ucType.GetProperty("LoadIPDeviceDetails");
                                                ucsetSensor.SetValue(oCtrlDemo, (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1_loopVariable, null);
                                            }
                                        }
                                        else if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                                        {
                                            if (MySiteObj_loopVariable.SiteOtherDevices.Contains(((LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1_loopVariable).ID))
                                            {
                                                PropertyInfo ucsetSensor = ucType.GetProperty("LoadOtherDeviceDetails");
                                                ucsetSensor.SetValue(oCtrlDemo, (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1_loopVariable, null);
                                            }
                                        }
                                        else if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                                        {
                                            if (MySiteObj_loopVariable.SiteSNMPDevices.Contains(((LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1_loopVariable).ID))
                                            {
                                                PropertyInfo ucsetSensor = ucType.GetProperty("LoadSNMPDeviceDetails");
                                                ucsetSensor.SetValue(oCtrlDemo, (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1_loopVariable, null);
                                            }
                                        }
                                        else if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.CameraDetails)
                                        {
                                            if (MySiteObj_loopVariable.SiteCameras.Contains(((LiveMonitoring.IRemoteLib.CameraDetails)MyObject1_loopVariable).ID))
                                            {
                                                PropertyInfo ucsetSensor = ucType.GetProperty("LoadCameraDetails");
                                                ucsetSensor.SetValue(oCtrlDemo, (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1_loopVariable, null);
                                            }
                                            //add all
                                        }
                                        else if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.SensorGroup)
                                        {
                                            PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorGroupDetails");
                                            ucsetSensor.SetValue(oCtrlDemo, (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1_loopVariable, null);
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
                                MethodInfo UCDrawGroups = ucType.GetMethod("DrawGroups");
                                // Invoke the method
                                UCDrawGroups.Invoke(oCtrlDemo, null);
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void AddValues(string MyImage, string MyDevice, string MyField, string MySensor, LiveMonitoring.IRemoteLib.StatusDef MyStatus, string MyValue, string MyOtherValue)
        {
            try
            {
                //Me.GridStatus.Rows.Add()
                string MyAlert = null;
                string MyColor = null;
                //       Black 		Gray 		Silver 		White
                //Yellow 		Lime 		Aqua 		Fuchsia
                //Red 		Green 		Blue 		Purple
                //Maroon 		Olive 		Navy 		Teal
                switch (MyStatus)
                {
                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                        MyAlert = "Alert";
                        MyColor = "Maroon";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.criticalerror:
                        MyAlert = "Critical";
                        MyColor = "Red";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.devicefailure:
                        MyAlert = "Device failure";
                        MyColor = "Purple";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.disabled:
                        MyAlert = "Disabled";
                        MyColor = "Gray";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                        MyAlert = "No Response";
                        MyColor = "Fuchsia";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.notify:
                        MyAlert = "Notify";
                        MyColor = "Navy";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                        MyAlert = "OK";
                        MyColor = "Green";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.statuserror:
                        MyColor = "Maroon";
                        MyAlert = "Yellow";
                        break;
                    default:
                        MyAlert = "Unknown";
                        MyColor = "White";
                        break;
                }

                MyTable += "<tr><a href=DisplayGraphs.aspx?SensorNum=" + MyImage + " target='main'>";
                MyTable += "<td>";
                //<img src='ReturnThumbnailImage.aspx?Sensor=" & MyImage & "' height=12 width=12>"
                MyTable += "</td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyDevice);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MySensor);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyField);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyAlert);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyValue);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyOtherValue);
                MyTable += "</font></td>";
                MyTable += "</a><tr>";

            }
            catch (Exception ex)
            {
            }
        }

        protected void chkFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_FilteredList();
        }

        public void Fill_FilteredList()
        {
            try
            {
                bool MyFilterOK = false;
                bool MyFilterError = false;
                bool MyFilterNoResponse = false;
                bool MyFilterSTDAlert = false;
                bool MyFilterSensorAlert = false;
                bool MyFilterSensorWarning = false;
                bool MyFilterUnknown = false;

                ClearLists();
                displayGroups.Controls.Clear();
                //Hold all the sensor ID's
                foreach (ListItem Item_loopVariable in this.chkFilter.Items)
                {
                    //Item = Item_loopVariable;
                    switch (Convert.ToInt32(Item_loopVariable.Value))
                    {
                        case 0:
                            if (Item_loopVariable.Selected)
                                MyFilterOK = true;
                            break;
                        case 1:
                            if (Item_loopVariable.Selected)
                                MyFilterError = true;
                            break;
                        case 2:
                            if (Item_loopVariable.Selected)
                                MyFilterNoResponse = true;
                            break;
                        case 3:
                            if (Item_loopVariable.Selected)
                                MyFilterSTDAlert = true;
                            break;
                        case 4:
                            if (Item_loopVariable.Selected)
                                MyFilterSensorAlert = true;
                            break;
                        case 5:
                            if (Item_loopVariable.Selected)
                                MyFilterSensorWarning = true;
                            break;
                        case 6:
                            if (Item_loopVariable.Selected)
                                MyFilterUnknown = true;

                            break;
                    }

                }

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

                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                Collection MyFilteredCollection = new Collection();
                object MyObject1 = null;
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
                            {
                                LiveMonitoring.IRemoteLib.SiteDetails MyObject6 = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
                                try
                                {
                                    RetSites.Add(MyObject6);
                                    MyFilteredCollection.Add(MyObject6);
                                }
                                catch (Exception ex)
                                {
                                    //Trace.Write("Get All err  MySitesList:" + ex.Message)
                                }
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                MyCameraCollection.Add(MyCamera);
                                MyFilteredCollection.Add(MyCamera);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                bool TestFilters = false;
                                try
                                {
                                    if (!string.IsNullOrEmpty(txtFilterName.Text) & MySensor.Caption.ToUpper().Contains(txtFilterName.Text.ToUpper()))
                                    {
                                        TestFilters = true;
                                    }
                                    else if (string.IsNullOrEmpty(txtFilterName.Text))
                                    {
                                        TestFilters = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TestFilters = true;
                                }

                                if (TestFilters)
                                {
                                    if (MyFilterOK & MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.ok)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                    else if (MyFilterError & (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.statuserror | MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.criticalerror))
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                    else if (MyFilterNoResponse & MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.noresponse)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                    else if (MyFilterSTDAlert & MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.alert)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                    else if (MyFilterSensorAlert & CheckFieldAlert(MySensor))
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                    else if (MyFilterSensorWarning & CheckFieldWarning(MySensor))
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                    else if (MyFilterUnknown & MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.disabled)
                                    {
                                        MySensorCollection.Add(MySensor);
                                        MyFilteredCollection.Add(MySensor);
                                    }
                                }

                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                            {
                                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                                MySensorGroupCollection.Add(MySensorGroup);
                                MyFilteredCollection.Add(MySensorGroup);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                                MyIPDevicesCollection.Add(MyIPDevicesDetails);
                                MyFilteredCollection.Add(MyIPDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                                MyOtherDevicesCollection.Add(MyOtherDevicesDetails);
                                MyFilteredCollection.Add(MyOtherDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                            {
                                LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                                MySNMPDevicesCollection.Add(MySNMPDevicesDetails);
                                MyFilteredCollection.Add(MySNMPDevicesDetails);
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    try
                    {
                        //add a site container to display for each site
                        FillSites(MyFilteredCollection);
                        Session["SelectObjects"] = MyFilteredCollection;
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                //lblerr.Visible = True
                //lblerr.Text = ex.Message
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                Fill_FilteredList();
            }
            catch (Exception ex)
            {
            }
        }

        public void DeviceDisplay_DeviceDisplay()
        {
            Load += Page_Load;
        }
    }
}