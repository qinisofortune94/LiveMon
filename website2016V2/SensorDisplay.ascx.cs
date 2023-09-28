using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SensorDisplay : System.Web.UI.UserControl
    {
        private string _callbackArg;
        public int _SensorID;
        private LiveMonitoring.IRemoteLib.SensorDetails _MySensor;
        public DateTime LastRefresh;
        public int setID
        {
            set { _SensorID = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind();
        }

        public LiveMonitoring.IRemoteLib.SensorDetails LoadSensorDetails
        {
            set
            {
                _MySensor = (value);
                EditSensor.HRef = "../../EditSensors.aspx?SensorID=" + _MySensor.ID.ToString();
                ShowGraph.HRef = "../../DisplayGraphs.aspx?SensorNum=" + _MySensor.ID.ToString();
                _SensorID = _MySensor.ID;
                SensorName.Text = value.Caption;
                //+ ":" + value.ID.ToString
                if (value.LastErrors.Count > 0)
                    SensorName.ToolTip = value.LastErrors.Peek();
                //load fields
                try
                {
                    foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef Myfield in _MySensor.Fields)
                    {
                        try
                        {
                            Control oCtrlDemo = new Control();
                            oCtrlDemo = LoadControl("SensorFieldDisplay.ascx");
                            oCtrlDemo.ID = "SensorField" + Myfield.FieldNumber.ToString();
                            SensorContainerDetails.Controls.Add(oCtrlDemo);
                            Type ucType = oCtrlDemo.GetType();
                            try
                            {
                                PropertyInfo ucsetSensor = ucType.GetProperty("setSensorStatus");
                                ucsetSensor.SetValue(oCtrlDemo, _MySensor.Status, null);

                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                PropertyInfo ucsetSensor = ucType.GetProperty("setFieldStatus");
                                ucsetSensor.SetValue(oCtrlDemo, Myfield.FieldStatus, null);

                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                PropertyInfo ucsetSensor = ucType.GetProperty("setFieldCaption");
                                ucsetSensor.SetValue(oCtrlDemo, Myfield.FieldName, null);

                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                PropertyInfo ucsetSensor = ucType.GetProperty("setFieldValue");
                                ucsetSensor.SetValue(oCtrlDemo, Myfield.LastValue.ToString(), null);

                            }
                            catch (Exception ex)
                            {
                            }
                            if (!string.IsNullOrEmpty(Myfield.LastOtherValue))
                            {
                                try
                                {
                                    PropertyInfo ucsetSensor = ucType.GetProperty("setFieldOtherValue");
                                    ucsetSensor.SetValue(oCtrlDemo, Myfield.LastOtherValue.ToString(), null);

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
                //load Site details
                //fill Sensors
            }
        }

        public void DeviceDisplay_SensorDisplay()
        {
            Load += Page_Load;
        }
    }
}