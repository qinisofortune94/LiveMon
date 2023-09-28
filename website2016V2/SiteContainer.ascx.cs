using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SiteContainer : System.Web.UI.UserControl
    {
        private int _SiteID;
        private LiveMonitoring.IRemoteLib.SiteDetails _SiteDetails;
        private List<LiveMonitoring.IRemoteLib.CameraDetails> _MyCameraCollection = new List<LiveMonitoring.IRemoteLib.CameraDetails>();
        private Collection _MyCameraIDCollection = new Collection();
        private List<LiveMonitoring.IRemoteLib.SensorDetails> _MySensorCollection = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
        private List<LiveMonitoring.IRemoteLib.SensorGroup> _MySensorGroupCollection = new List<LiveMonitoring.IRemoteLib.SensorGroup>();
        private List<LiveMonitoring.IRemoteLib.IPDevicesDetails> _MyIPDevicesCollection = new List<LiveMonitoring.IRemoteLib.IPDevicesDetails>();
        private Collection _MyIPDevicesIDCollection = new Collection();
        private List<LiveMonitoring.IRemoteLib.OtherDevicesDetails> _MyOtherDevicesCollection = new List<LiveMonitoring.IRemoteLib.OtherDevicesDetails>();
        private Collection _MyOtherDevicesIDCollection = new Collection();
        private List<LiveMonitoring.IRemoteLib.SNMPManagerDetails> _MySNMPDevicesCollection = new List<LiveMonitoring.IRemoteLib.SNMPManagerDetails>();
        private Collection _MySNMPDevicesIDCollection = new Collection();
        private Collection addedSensorGroup = new Collection();
        public event RefreshPageEventHandler RefreshPage;
        public delegate void RefreshPageEventHandler();
        public int setID
        {
            //load Device details
            //fill Sensors
            set { _SiteID = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public LiveMonitoring.IRemoteLib.SiteDetails LoadSiteDetails
        {
            set
            {
                _SiteDetails = value;
                setID = _SiteDetails.ID;
                SiteName.Text = _SiteDetails.SiteName;
            }
        }

        public LiveMonitoring.IRemoteLib.SensorDetails LoadSensorDetails
        {
            set { _MySensorCollection.Add(value); }
        }

        public LiveMonitoring.IRemoteLib.SensorGroup LoadSensorGroupDetails
        {
            set { _MySensorGroupCollection.Add(value); }
        }

        public LiveMonitoring.IRemoteLib.OtherDevicesDetails LoadOtherDeviceDetails
        {
            set
            {
                _MyOtherDevicesCollection.Add(value);
                _MyOtherDevicesIDCollection.Add(value.ID, value.ID.ToString());
                //load Site details
                //fill Sensors
            }
        }

        public LiveMonitoring.IRemoteLib.SNMPManagerDetails LoadSNMPDeviceDetails
        {
            set
            {
                _MySNMPDevicesCollection.Add(value);
                _MySNMPDevicesIDCollection.Add(value.ID, value.ID.ToString());
                //load Site details
                //fill Sensors
            }
        }

        public LiveMonitoring.IRemoteLib.IPDevicesDetails LoadIPDeviceDetails
        {
            set
            {
                _MyIPDevicesCollection.Add(value);
                _MyIPDevicesIDCollection.Add(value.ID, value.ID.ToString());
                //load Site details
                //fill Sensors
            }
        }

        public LiveMonitoring.IRemoteLib.CameraDetails LoadCameraDetails
        {
            set
            {
                _MyCameraCollection.Add(value);
                _MyCameraIDCollection.Add(value.ID, value.ID.ToString());
                //load Site details
                //fill Sensors
            }
        }

        public void DrawGroups()
        {
            //draw groups find each unique group per sensorlist and add the details
            foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensor_loopVariable in _MySensorCollection)
            {
                //MySensor = MySensor_loopVariable;
                try
                {
                    Control oCtrlDemo = new Control();
                    if (addedSensorGroup.Contains(MySensor_loopVariable.SensGroup.SensorGroupID.ToString()))
                    {
                        oCtrlDemo = SiteContainerDetails.FindControl("Group" + MySensor_loopVariable.SensGroup.SensorGroupID.ToString());
                    }
                    else
                    {
                        oCtrlDemo = LoadControl("GroupDisplay.ascx");
                        oCtrlDemo.ID = "Group" + MySensor_loopVariable.SensGroup.SensorGroupID.ToString();
                        SiteContainerDetails.Controls.Add(oCtrlDemo);
                        addedSensorGroup.Add(MySensor_loopVariable.SensGroup.SensorGroupID, MySensor_loopVariable.SensGroup.SensorGroupID.ToString());
                    }
                    Type ucType = oCtrlDemo.GetType();

                    //add group details to group
                    try
                    {
                        LiveMonitoring.IRemoteLib.SensorGroup FoundGroup = findGroupDetails(MySensor_loopVariable.SensGroup.SensorGroupID);
                        if ((FoundGroup == null) == false)
                        {
                            PropertyInfo ucsetGroup = ucType.GetProperty("LoadSensorGroupDetails");
                            ucsetGroup.SetValue(oCtrlDemo, FoundGroup, null);
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    //add Devices to group
                    if (_MyIPDevicesIDCollection.Contains(MySensor_loopVariable.IPDeviceID.ToString()))
                    {
                        LiveMonitoring.IRemoteLib.IPDevicesDetails FoundDevice = findIPDevice(MySensor_loopVariable.IPDeviceID.ToString());
                        if ((FoundDevice == null) == false)
                        {
                            PropertyInfo ucsetSensor = ucType.GetProperty("LoadIPDeviceDetails");
                            ucsetSensor.SetValue(oCtrlDemo, FoundDevice, null);
                        }
                    }
                    else if (_MySNMPDevicesIDCollection.Contains(MySensor_loopVariable.IPDeviceID.ToString()))
                    {
                        LiveMonitoring.IRemoteLib.SNMPManagerDetails FoundDevice = findSNMPDevice(MySensor_loopVariable.IPDeviceID.ToString());
                        if ((FoundDevice == null) == false)
                        {
                            PropertyInfo ucsetSensor = ucType.GetProperty("LoadSNMPDeviceDetails");
                            ucsetSensor.SetValue(oCtrlDemo, FoundDevice, null);
                        }
                    }
                    else if (_MyOtherDevicesIDCollection.Contains(MySensor_loopVariable.IPDeviceID.ToString()))
                    {
                        LiveMonitoring.IRemoteLib.OtherDevicesDetails FoundDevice = findOtherDevice(MySensor_loopVariable.IPDeviceID.ToString());
                        if ((FoundDevice == null) == false)
                        {
                            PropertyInfo ucsetSensor = ucType.GetProperty("LoadOtherDeviceDetails");
                            ucsetSensor.SetValue(oCtrlDemo, FoundDevice, null);
                        }
                    }
                    else if (_MyCameraIDCollection.Contains(MySensor_loopVariable.IPDeviceID.ToString()))
                    {
                        LiveMonitoring.IRemoteLib.CameraDetails FoundDevice = findCameraDevice(MySensor_loopVariable.IPDeviceID.ToString());
                        if ((FoundDevice == null) == false)
                        {
                            PropertyInfo ucsetSensor = ucType.GetProperty("LoadCameraDetails");
                            ucsetSensor.SetValue(oCtrlDemo, FoundDevice, null);
                        }
                    }
                    //Add Sensors to group
                    try
                    {
                        PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                        ucsetSensor.SetValue(oCtrlDemo, MySensor_loopVariable, null);

                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                }

            }
            //draw the loaded controls
            try
            {
                // addedSensorGroup.Add(MySensor.SensGroup.SensorGroupID, MySensor.SensGroup.SensorGroupID.ToString)
                foreach (int MyGroupId in addedSensorGroup)
                {
                    Control oCtrlDemo = new Control();
                    oCtrlDemo = SiteContainerDetails.FindControl("Group" + MyGroupId.ToString());
                    Type ucType = oCtrlDemo.GetType();
                    try
                    {
                        //call to draw group
                        // Get access to the Method 
                        MethodInfo UCDrawDevices = ucType.GetMethod("DrawDevices");
                        // Invoke the method
                        UCDrawDevices.Invoke(oCtrlDemo, null);

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

        private LiveMonitoring.IRemoteLib.IPDevicesDetails findIPDevice(string DeviceID)
        {
            LiveMonitoring.IRemoteLib.IPDevicesDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
            try
            {
                foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevice_loopVariable in _MyIPDevicesCollection)
                {
                    //MyIPDevice = MyIPDevice_loopVariable;
                    if (MyIPDevice_loopVariable.ID == Convert.ToInt32(DeviceID))
                    {
                        return MyIPDevice_loopVariable;
                        return functionReturnValue;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
            return functionReturnValue;
        }

        private LiveMonitoring.IRemoteLib.SNMPManagerDetails findSNMPDevice(string SNMPID)
        {
            LiveMonitoring.IRemoteLib.SNMPManagerDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
            try
            {
                foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MyIPDevice_loopVariable in _MySNMPDevicesCollection)
                {
                    //MyIPDevice = MyIPDevice_loopVariable;
                    if (MyIPDevice_loopVariable.ID == Convert.ToInt32(SNMPID))
                    {
                        return MyIPDevice_loopVariable;
                        return functionReturnValue;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
            return functionReturnValue;
        }

        private LiveMonitoring.IRemoteLib.OtherDevicesDetails findOtherDevice(string OtherID)
        {
            LiveMonitoring.IRemoteLib.OtherDevicesDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
            try
            {
                foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyIPDevice_loopVariable in _MyOtherDevicesCollection)
                {
                    //MyIPDevice = MyIPDevice_loopVariable;
                    if (MyIPDevice_loopVariable.ID == Convert.ToInt32(OtherID))
                    {
                        return MyIPDevice_loopVariable;
                        return functionReturnValue;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
            return functionReturnValue;
        }

        private LiveMonitoring.IRemoteLib.CameraDetails findCameraDevice(string CameraID)
        {
            LiveMonitoring.IRemoteLib.CameraDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.CameraDetails);
            try
            {
                foreach (LiveMonitoring.IRemoteLib.CameraDetails MyIPDevice_loopVariable in _MyCameraCollection)
                {
                    //MyIPDevice = MyIPDevice_loopVariable;
                    if (MyIPDevice_loopVariable.ID == Convert.ToInt32(CameraID))
                    {
                        return MyIPDevice_loopVariable;
                        return functionReturnValue;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
            return functionReturnValue;
        }

        private LiveMonitoring.IRemoteLib.SensorGroup findGroupDetails(int SensorGroupID)
        {
            LiveMonitoring.IRemoteLib.SensorGroup functionReturnValue = default(LiveMonitoring.IRemoteLib.SensorGroup);
            try
            {
                foreach (LiveMonitoring.IRemoteLib.SensorGroup MyGroup_loopVariable in _MySensorGroupCollection)
                {
                    // = MyGroup_loopVariable;
                    if (MyGroup_loopVariable.SensorGroupID == SensorGroupID)
                    {
                        return MyGroup_loopVariable;
                        return functionReturnValue;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
            return functionReturnValue;
        }

        protected void HideSite_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string[] MYPartID = ((System.Web.UI.WebControls.ImageButton)sender).ID.Split('_');
                Control oCtrlDemo = new Control();
                oCtrlDemo = SiteContainerDetails.FindControl(MYPartID[0] + "_SiteContainerDetails");
                if ((Session["HiddenSites"] == null) == false)
                {
                    Collection MyCollection = (Collection)Session["HiddenSites"];
                    MyCollection.Add(this.ID, this.ID.ToString());
                    Session["HiddenSites"] = MyCollection;
                }
                else
                {
                    Collection MyCollection = new Collection();
                    //= Session("HiddenSites")
                    MyCollection.Add(this.ID, this.ID.ToString());
                    Session["HiddenSites"] = MyCollection;
                }
                if (RefreshPage != null)
                {
                    RefreshPage();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowSite_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string[] MYPartID = ((System.Web.UI.WebControls.ImageButton)sender).ID.Split('_');
                Control oCtrlDemo = new Control();
                oCtrlDemo = SiteContainerDetails.FindControl(MYPartID[0] + "_SiteContainerDetails");
                if ((Session["HiddenSites"] == null) == false)
                {
                    Collection MyCollection = (Collection)Session["HiddenSites"];
                    if (MyCollection.Contains(this.ID.ToString()))
                        MyCollection.Remove(this.ID.ToString());
                    Session["HiddenSites"] = MyCollection;
                }
                if (RefreshPage != null)
                {
                    RefreshPage();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}