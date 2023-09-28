using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SensorFieldDisplay : System.Web.UI.UserControl
    {
        private LiveMonitoring.IRemoteLib.FieldStatusDef _SensorFieldStatus;
        private LiveMonitoring.IRemoteLib.StatusDef _SensorStatus;
        public LiveMonitoring.IRemoteLib.FieldStatusDef setFieldStatus
        {
            set { _SensorFieldStatus = value; }
        }
        public LiveMonitoring.IRemoteLib.StatusDef setSensorStatus
        {
            set { _SensorStatus = value; }
        }
        public string setFieldCaption
        {
            set
            {
                this.FieldName.Text = value;
                DrawStatus(ref this.FieldName);
            }
        }
        public string setFieldValue
        {
            set
            {
                this.FieldValue.Text = value;
                this.FieldValue.Visible = true;
                DrawStatus(ref this.FieldValue);
            }
        }
        public string setFieldOtherValue
        {
            set
            {
                this.FieldOtherValue.Text = value;
                this.FieldOtherValue.Visible = true;
                if (this.FieldValue.Text == "0")
                {
                    this.FieldValue.Visible = false;
                }
                DrawStatus(ref this.FieldOtherValue);
            }
        }
        public string setFieldTooltip
        {
            set
            {
                this.FieldOtherValue.ToolTip = value;
                this.FieldValue.ToolTip = value;
                this.FieldName.ToolTip = value;

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void DrawStatus(ref System.Web.UI.WebControls.Label MyLabel)
        {
            string MyAlert = null;
            System.Drawing.Color MyColor = default(System.Drawing.Color);
            //       Black 		Gray 		Silver 		White
            //Yellow 		Lime 		Aqua 		Fuchsia
            //Red 		Green 		Blue 		Purple
            //Maroon 		Olive 		Navy 		Teal
            switch (_SensorStatus)
            {
                case LiveMonitoring.IRemoteLib.StatusDef.alert:
                    MyAlert = "Alert";
                    MyColor = System.Drawing.Color.Red;
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.criticalerror:
                    MyAlert = "Critical";
                    MyColor = System.Drawing.Color.Maroon;
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.devicefailure:
                    MyAlert = "Device failure";
                    MyColor = System.Drawing.Color.Fuchsia;
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.disabled:
                    MyAlert = "Disabled";
                    MyColor = System.Drawing.Color.DarkGray;
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                    MyAlert = "No Response";
                    MyColor = System.Drawing.Color.Purple;
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.notify:
                    MyAlert = "Notify";
                    MyColor = System.Drawing.Color.Navy;
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.ok:
                    MyAlert = "OK";
                    MyColor = System.Drawing.Color.Green;
                    if (_SensorFieldStatus != LiveMonitoring.IRemoteLib.FieldStatusDef.ok)
                    {
                        if (_SensorFieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                        {
                            MyAlert = "Sensor Warning";
                            MyColor = System.Drawing.Color.Orange;
                        }
                        else if (_SensorFieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                        {
                            MyAlert = "Sensor Alert";
                            MyColor = System.Drawing.Color.Red;
                        }
                    }
                    break;
                case LiveMonitoring.IRemoteLib.StatusDef.statuserror:
                    MyColor = System.Drawing.Color.Maroon;
                    MyAlert = "Yellow";
                    break;
                default:
                    MyAlert = "Unknown";
                    MyColor = System.Drawing.Color.White;
                    break;
            }

            MyLabel.ToolTip = MyAlert;
            MyLabel.BackColor = MyColor;
        }
    }
}