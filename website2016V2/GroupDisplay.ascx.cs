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
    public partial class GroupDisplay : System.Web.UI.UserControl
    {
        private int _GroupID;
        private List<LiveMonitoring.IRemoteLib.CameraDetails> _MyCameraCollection = new List<LiveMonitoring.IRemoteLib.CameraDetails>();
        private List<LiveMonitoring.IRemoteLib.SensorDetails> _MySensorCollection = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
        private List<LiveMonitoring.IRemoteLib.IPDevicesDetails> _MyIPDevicesCollection = new List<LiveMonitoring.IRemoteLib.IPDevicesDetails>();
        private List<LiveMonitoring.IRemoteLib.OtherDevicesDetails> _MyOtherDevicesCollection = new List<LiveMonitoring.IRemoteLib.OtherDevicesDetails>();
        private List<LiveMonitoring.IRemoteLib.SNMPManagerDetails> _MySNMPDevicesCollection = new List<LiveMonitoring.IRemoteLib.SNMPManagerDetails>();
        private LiveMonitoring.IRemoteLib.SensorGroup _MySensorGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
        private Collection addedDeviceGroup = new Collection();
        public int setID
        {
            //load group details
            //fill Devices
            set { _GroupID = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public LiveMonitoring.IRemoteLib.SensorGroup LoadSensorGroupDetails
        {
            set
            {
                _MySensorGroup = (value);
                GroupName.Text = value.SensorGroupName;
                //load Site details
                //fill Sensors
            }
        }
        public LiveMonitoring.IRemoteLib.SensorDetails LoadSensorDetails
        {
            set
            {
                _MySensorCollection.Add(value);
                try
                {
                    //TestSens.Text += value.ID.ToString + ":"

                }
                catch (Exception ex)
                {
                }
                //load Site details
                //fill Sensors
            }
        }
        public LiveMonitoring.IRemoteLib.OtherDevicesDetails LoadOtherDeviceDetails
        {
            //load Site details
            //fill Sensors
            set { _MyOtherDevicesCollection.Add(value); }
        }

        public LiveMonitoring.IRemoteLib.SNMPManagerDetails LoadSNMPDeviceDetails
        {
            //load Site details
            //fill Sensors
            set { _MySNMPDevicesCollection.Add(value); }
        }
        public LiveMonitoring.IRemoteLib.IPDevicesDetails LoadIPDeviceDetails
        {
            //load Site details
            //fill Sensors
            set { _MyIPDevicesCollection.Add(value); }
        }
        public LiveMonitoring.IRemoteLib.CameraDetails LoadCameraDetails
        {
            //load Site details
            //fill Sensors
            set { _MyCameraCollection.Add(value); }
        }

        public void DrawDevices()
        {
            //draw Devices then add sensors to each
            foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIpDevice_loopVariable in _MyIPDevicesCollection)
            {
                //MyIpDevice = MyIpDevice_loopVariable;
                try
                {
                    Control oCtrlDemo = new Control();
                    if (addedDeviceGroup.Contains(MyIpDevice_loopVariable.ID.ToString()))
                    {
                        oCtrlDemo = GroupContainerDetails.FindControl("Device" + MyIpDevice_loopVariable.ID.ToString());
                    }
                    else
                    {
                        oCtrlDemo = LoadControl("DeviceContainer.ascx");
                        oCtrlDemo.ID = "Device" + MyIpDevice_loopVariable.ID.ToString();
                        GroupContainerDetails.Controls.Add(oCtrlDemo);

                        addedDeviceGroup.Add(MyIpDevice_loopVariable.ID, MyIpDevice_loopVariable.ID.ToString());
                    }
                    Type ucType = oCtrlDemo.GetType();
                    PropertyInfo ucsetIPD = ucType.GetProperty("LoadIPDeviceDetails");
                    ucsetIPD.SetValue(oCtrlDemo, MyIpDevice_loopVariable, null);
                    foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensor_loopVariable in _MySensorCollection)
                    {
                        //MySensor = MySensor_loopVariable;

                        try
                        {
                            if (MyIpDevice_loopVariable.ID == MySensor_loopVariable.IPDeviceID)
                            {
                                //add this sensor to device display
                                try
                                {
                                    PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                                    ucsetSensor.SetValue(oCtrlDemo, MySensor_loopVariable, null);

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

                }
                catch (Exception ex)
                {
                }
            }
            foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyIpDevice_loopVariable in _MyOtherDevicesCollection)
            {
                //MyIpDevice = MyIpDevice_loopVariable;
                try
                {
                    Control oCtrlDemo = new Control();
                    if (addedDeviceGroup.Contains(MyIpDevice_loopVariable.ID.ToString()))
                    {
                        oCtrlDemo = GroupContainerDetails.FindControl("Device" + MyIpDevice_loopVariable.ID.ToString());
                    }
                    else
                    {
                        oCtrlDemo = LoadControl("DeviceContainer.ascx");
                        oCtrlDemo.ID = "Device" + MyIpDevice_loopVariable.ID.ToString();
                        GroupContainerDetails.Controls.Add(oCtrlDemo);

                        addedDeviceGroup.Add(MyIpDevice_loopVariable.ID, MyIpDevice_loopVariable.ID.ToString());
                    }
                    Type ucType = oCtrlDemo.GetType();
                    PropertyInfo ucsetIPD = ucType.GetProperty("LoadOtherDeviceDetails");
                    ucsetIPD.SetValue(oCtrlDemo, MyIpDevice_loopVariable, null);
                    foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensor_loopVariable in _MySensorCollection)
                    {
                        //MySensor = MySensor_loopVariable;

                        try
                        {
                            if (MyIpDevice_loopVariable.ID == MySensor_loopVariable.IPDeviceID)
                            {
                                //add this sensor to device display
                                try
                                {
                                    PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                                    ucsetSensor.SetValue(oCtrlDemo, MySensor_loopVariable, null);
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

                }
                catch (Exception ex)
                {
                }
            }
            foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MyIpDevice_loopVariable in _MySNMPDevicesCollection)
            {
                //MyIpDevice = MyIpDevice_loopVariable;
                try
                {
                    Control oCtrlDemo = new Control();
                    if (addedDeviceGroup.Contains(MyIpDevice_loopVariable.ID.ToString()))
                    {
                        oCtrlDemo = GroupContainerDetails.FindControl("Device" + MyIpDevice_loopVariable.ID.ToString());
                    }
                    else
                    {
                        oCtrlDemo = LoadControl("DeviceContainer.ascx");
                        oCtrlDemo.ID = "Device" + MyIpDevice_loopVariable.ID.ToString();
                        GroupContainerDetails.Controls.Add(oCtrlDemo);

                        addedDeviceGroup.Add(MyIpDevice_loopVariable.ID, MyIpDevice_loopVariable.ID.ToString());
                    }
                    Type ucType = oCtrlDemo.GetType();
                    PropertyInfo ucsetIPD = ucType.GetProperty("LoadSNMPDeviceDetails");
                    ucsetIPD.SetValue(oCtrlDemo, MyIpDevice_loopVariable, null);
                    foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensor_loopVariable in _MySensorCollection)
                    {
                        //MySensor = MySensor_loopVariable;

                        try
                        {
                            if (MyIpDevice_loopVariable.ID == MySensor_loopVariable.IPDeviceID)
                            {
                                //add this sensor to device display
                                try
                                {
                                    PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                                    ucsetSensor.SetValue(oCtrlDemo, MySensor_loopVariable, null);

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

                }
                catch (Exception ex)
                {
                }
            }
            foreach (LiveMonitoring.IRemoteLib.CameraDetails MyIpDevice_loopVariable in _MyCameraCollection)
            {
                //MyIpDevice = MyIpDevice_loopVariable;
                try
                {
                    Control oCtrlDemo = new Control();
                    if (addedDeviceGroup.Contains(MyIpDevice_loopVariable.ID.ToString()))
                    {
                        oCtrlDemo = GroupContainerDetails.FindControl("Device" + MyIpDevice_loopVariable.ID.ToString());
                    }
                    else
                    {
                        oCtrlDemo = LoadControl("DeviceContainer.ascx");
                        oCtrlDemo.ID = "Device" + MyIpDevice_loopVariable.ID.ToString();
                        GroupContainerDetails.Controls.Add(oCtrlDemo);

                        addedDeviceGroup.Add(MyIpDevice_loopVariable.ID, MyIpDevice_loopVariable.ID.ToString());
                    }
                    Type ucType = oCtrlDemo.GetType();
                    PropertyInfo ucsetIPD = ucType.GetProperty("LoadCameraDetails");
                    ucsetIPD.SetValue(oCtrlDemo, MyIpDevice_loopVariable, null);
                    foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensor_loopVariable in _MySensorCollection)
                    {
                        //MySensor = MySensor_loopVariable;

                        try
                        {
                            if (MyIpDevice_loopVariable.ID == MySensor_loopVariable.IPDeviceID)
                            {
                                //add this sensor to device display
                                try
                                {
                                    PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                                    ucsetSensor.SetValue(oCtrlDemo, MySensor_loopVariable, null);

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

                }
                catch (Exception ex)
                {
                }
            }

        }
    }
}