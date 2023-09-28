using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class AddAlert : System.Web.UI.Page
    {
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
        private LiveMonitoring.GlobalRemoteVars MyRemm = new LiveMonitoring.GlobalRemoteVars();
        public DateTime LastRefresh;
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

                addAlertLoad();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Response.Redirect("Index.aspx");
        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
        }

        protected void editAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx");
        }

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }

        public void addAlertLoad()
        {
            successMessage.Visible = false;
            warningMessage.Visible = false;
            errorMessage.Visible = false;

            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
           
            if (IsPostBack == false)
            {
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.getItemValue(this.AlertType, this.cmbSensor1ID, this.cmbSensor2ID);
                //AlertCameraImages
                Collection MyCollection = new Collection();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                AlertCameraImages.Items.Clear();
                //AlertSensorValues.Items.Clear()

                if ((MyCollection == null))
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Camera details not found. Please try again.";
                    return;
                }

                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                    {
                        LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MyCamera.Caption;
                        MyItem.Value = MyCamera.ID.ToString();
                        MyItem.Selected = false;
                        AlertCameraImages.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MySensor.Caption;
                        MyItem.Value = MySensor.ID.ToString();
                        MyItem.Selected = false;
                        //AlertSensorValues.Items.Add(MyItem)
                        cmbSensor1ID.Items.Add(MyItem);
                        cmbSensor2ID.Items.Add(MyItem);
                    }
                }
            }
        }

        protected void cmbSensor1ID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            cmbField1.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    int cbosensor = Convert.ToInt32(cmbSensor1ID.SelectedValue);
                    if (MySensor.ID == cbosensor)
                    {
                        //LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                        {
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MyField.FieldName;
                            MyItem.Value = MyField.FieldNumber.ToString();
                            MyItem.Selected = false;
                            cmbField1.Items.Add(MyItem);
                        }
                    }
                }
            }
            int cbosensor2 = Convert.ToInt32(cmbSensor1ID.SelectedValue);
            if (cmbField1.Items.Count == 0 & cbosensor2 > 0)
            {
                int n = 1;
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = "Field1";
                MyItem.Value = n.ToString();
                MyItem.Selected = false;
                cmbField1.Items.Add(MyItem);
            }

        }

        protected void cmbSensor2ID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            cmbField2.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    int cbosensor = Convert.ToInt32(cmbSensor1ID.SelectedValue);
                    if (MySensor.ID == cbosensor)
                    {
                        //LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                        {
                            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                            MyItem.Text = MyField.FieldName;
                            MyItem.Value = MyField.FieldNumber.ToString();
                            MyItem.Selected = false;
                            cmbField2.Items.Add(MyItem);
                        }
                    }
                }
            }
            int cbosensor2 = Convert.ToInt32(cmbSensor1ID.SelectedValue);
            if (cmbField2.Items.Count == 0 & cbosensor2 > 0)
            {
                int n = 1;
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = "Field1";
                MyItem.Value = n.ToString();
                MyItem.Selected = false;
                cmbField1.Items.Add(MyItem);
            }
        }

        protected void btnDevice_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Device>";
        }

        protected void btnAlertMins_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<AlertMins>";

        }

        protected void btnAlertStart_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<AlertStart>";

        }

        protected void btnCrLf_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<CRLF>";

        }

        protected void btnFields_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Fields>";

        }

        protected void btnName_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Name>";

        }

        protected void btnRTNSE_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<RTN Start>Your RTN Message HERE<RTN End>";

        }

        protected void btnValues_Click(object sender, EventArgs e)
        {
            AlertMessage.Text += "<Values>";

        }

        protected void SubmitAlert_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            LiveMonitoring.IRemoteLib.AlertDetails MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails();
            MyAlert.AlertMessage = this.AlertMessage.Text;
            int i = 0;
            //int result = 0;
            for (i = 0; i < this.AlertType.Items.Count; i++)
            {
                if (this.AlertType.Items[i].Selected == true)
                {
                    MyAlert.AlertType = MyAlert.AlertType | (LiveMonitoring.IRemoteLib.AlertDetails.AlertsType)Convert.ToInt32(AlertType.Items[i].Value);
                    //result += Convert.ToInt32(this.AlertType.Items[i].Value);
                }
            }
           // MyAlert.AlertType = (LiveMonitoring.IRemoteLib.AlertDetails.AlertsType)result;

            if (MyAlert.AlertType == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select Alert Type.";
                AlertType.Focus();
                return;
            }
            if (string.IsNullOrEmpty(MyAlert.AlertMessage))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please Enter a Message." + MyAlert.AlertType;
                AlertType.Focus();
                return;
            }
            int aCnt = 0;
            for (i = 0; i <= this.AlertCameraImages.Items.Count - 1; i++)
            {
                if (this.AlertCameraImages.Items[i].Selected)
                {
                    if (aCnt == 0)
                    {
                        MyAlert.CameraID1 = Convert.ToInt32(this.AlertCameraImages.Items[i].Value);
                    }
                    else
                    {
                        MyAlert.CameraID2 = Convert.ToInt32(this.AlertCameraImages.Items[i].Value);

                    }
                    aCnt += 1;
                    if (aCnt > 1)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            aCnt = 0;
            if (!string.IsNullOrEmpty(this.cmbSensor1ID.SelectedValue))
            {
                if (this.cmbSensor1ID.SelectedValue == "0")
                {
                    MyAlert.SensorValueID1 = 0;
                    MyAlert.SensorValueID3 = 0;
                }
                else
                {
                    if (string.IsNullOrEmpty(this.cmbField1.SelectedValue))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please Select Field for 1.";

                        AlertType.Focus();
                        return;
                    }
                    else
                    {
                        MyAlert.SensorValueID1 = Convert.ToInt32(this.cmbSensor1ID.SelectedValue);
                        MyAlert.SensorValueID3 = Convert.ToInt32(this.cmbField1.SelectedValue);
                    }
                }
            }
            else
            {
                MyAlert.SensorValueID1 = 0;
                MyAlert.SensorValueID3 = 0;
            }
            if (!string.IsNullOrEmpty(this.cmbSensor2ID.SelectedValue))
            {
                if (this.cmbSensor2ID.SelectedValue == "0")
                {
                    MyAlert.SensorValueID2 = 0;
                    MyAlert.SensorValueID4 = 0;
                }
                else
                {
                    if (string.IsNullOrEmpty(this.cmbField2.SelectedValue))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please Select Field for 2.";

                        AlertType.Focus();
                        return;
                    }
                    else
                    {
                        MyAlert.SensorValueID2 = Convert.ToInt32(this.cmbSensor2ID.SelectedValue);
                        MyAlert.SensorValueID4 = Convert.ToInt32(this.cmbField2.SelectedValue);
                    }
                }
            }
            else
            {
                MyAlert.SensorValueID2 = 0;
                MyAlert.SensorValueID4 = 0;
            }

            MyAlert.IncludeImage = this.AlertIncludeImage.Items[0].Selected;
            MyAlert.Enabled = this.AlertEnabled.Items[0].Selected;
            MyAlert.SendNormal = this.AlertSendRTN.Items[0].Selected;
            MyAlert.Camera1Delay = Convert.ToDouble(this.txtDelay1.Text);
            MyAlert.Camera2Delay = Convert.ToDouble(this.txtDelay2.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            int MyALertID = MyRem.LiveMonServer.AddNewAlert(MyAlert);

            if (MyALertID < 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Add Alert Failed.";

                AlertType.Focus();
                try
                {
                    MyRem.WriteLog("Add Alert Failed", "User:" + MyUser.ID.ToString() + "|" + this.AlertMessage.Text);

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                successMessage.Visible = true;
                lblSucces.Text = "Add Alert Succeeded.";

                AlertType.Focus();
                try
                {
                    MyRem.WriteLog("Add Alert Succeeded", "User:" + MyUser.ID.ToString() + "|" + MyALertID.ToString());

                }
                catch (Exception ex)
                {
                }
                Response.Redirect("LinkAlertcontact.aspx?AlertID=" + MyALertID.ToString());
            }
        }
    }
}