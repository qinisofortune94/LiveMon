using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class CameraControl : System.Web.UI.UserControl
    {
        private int _myID;
        public string setImageHolderID
        {
            get { return this.MyImageHolder.ID; }
            set { this.MyImageHolder.ID = value; }
        }
        public int setID
        {
            set
            {
                this.HdnCameraID.Value = value.ToString();
                _myID = value;
            }
        }
        public int setRefresh
        {
            set { this.Refresh.Value = value.ToString(); }
        }
        public string setProxyConfigURL
        {
            set { this.btnConfigProxy.NavigateUrl = value; }
        }
        public string setConfigURL
        {
            set { this.btnConfig.NavigateUrl = value; }
        }
        public string setConfigCameraLink
        {
            set { this.btnConfigSensor.NavigateUrl = value; }
        }
        public string setCaptureURL
        {
            set { this.btnCapture.NavigateUrl = value; }
        }
        public System.Web.UI.Control setAddIOInput
        {
            set
            {
                this.InPannel.Controls.Add(value);
                System.Web.UI.HtmlControls.HtmlGenericControl MyBR = new System.Web.UI.HtmlControls.HtmlGenericControl();
                MyBR.InnerHtml = "<br/>";
                this.InPannel.Controls.Add(MyBR);
            }
        }

        public System.Web.UI.Control setAddIOOutput
        {
            set
            {
                this.OutPannel.Controls.Add(value);
                System.Web.UI.HtmlControls.HtmlGenericControl MyBR = new System.Web.UI.HtmlControls.HtmlGenericControl();
                MyBR.InnerHtml = "<br/>";
                this.OutPannel.Controls.Add(MyBR);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public CameraControl()
        {
            Load += Page_Load;
        }
    }
}