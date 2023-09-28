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
    public partial class DeviceContainer : System.Web.UI.UserControl
    {
        private List<LiveMonitoring.IRemoteLib.SensorDetails> _MySensorCollection = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
        private Collection _MySensorIDCollection = new Collection();
        private LiveMonitoring.IRemoteLib.IPDevicesDetails _MyIPDevice = new LiveMonitoring.IRemoteLib.IPDevicesDetails();
        private LiveMonitoring.IRemoteLib.OtherDevicesDetails _MyOtherDevice = new LiveMonitoring.IRemoteLib.OtherDevicesDetails();
        private LiveMonitoring.IRemoteLib.SNMPManagerDetails _MySNMPDevice = new LiveMonitoring.IRemoteLib.SNMPManagerDetails();
        private LiveMonitoring.IRemoteLib.CameraDetails _MyCamera = new LiveMonitoring.IRemoteLib.CameraDetails();
        private int _DeviceID;
        public int setID
        {

            //load Device details
            //fill Sensors
            set { _DeviceID = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public LiveMonitoring.IRemoteLib.SensorDetails LoadSensorDetails
        {
            set
            {
                if (_MySensorIDCollection.Contains(value.ID.ToString()) == false)
                {
                    _MySensorCollection.Add(value);
                    _MySensorIDCollection.Add(value.ID, value.ID.ToString());
                    //load fields
                    try
                    {
                        Control oCtrlDemo = new Control();
                        oCtrlDemo = LoadControl("SensorDisplay.ascx");
                        oCtrlDemo.ID = "Sensor" + value.ID.ToString();
                        DeviceContainerDetails.Controls.Add(oCtrlDemo);
                        Type ucType = oCtrlDemo.GetType();
                        try
                        {
                            PropertyInfo ucsetSensor = ucType.GetProperty("LoadSensorDetails");
                            ucsetSensor.SetValue(oCtrlDemo, value, null);
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }

                //load Site details
                //fill Sensors
            }
        }

        public LiveMonitoring.IRemoteLib.OtherDevicesDetails LoadOtherDeviceDetails
        {
            set
            {
                _MyOtherDevice = (value);
                DeviceName.Text = value.Caption;
                //load Site details
                //fill Sensors
            }
        }

        public LiveMonitoring.IRemoteLib.SNMPManagerDetails LoadSNMPDeviceDetails
        {
            set
            {
                _MySNMPDevice = (value);
                DeviceName.Text = value.Caption;
                //load Site details
                //fill Sensors
            }
        }
        public LiveMonitoring.IRemoteLib.IPDevicesDetails LoadIPDeviceDetails
        {
            set
            {
                _MyIPDevice = (value);
                DeviceName.Text = value.Caption;
                //load Site details
                //fill Sensors
            }
        }
        public LiveMonitoring.IRemoteLib.CameraDetails LoadCameraDetails
        {
            set
            {
                _MyCamera = (value);
                DeviceName.Text = value.Caption;
                //load Site details
                //fill Sensors
            }
        }
    }
}