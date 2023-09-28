using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class AddAlertThreshHolds : System.Web.UI.Page
    {
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

                AddAlertThreshHoldOnLoad();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void AddAlertThreshHoldOnLoad()
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
                int reqAlertID = 0;
                reqAlertID = Convert.ToInt32(Request.QueryString["AlertID"]);
                this.txtAlertID.Text = reqAlertID.ToString();
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.getValues(this.TestType);
                //AlertCameraImages
                Collection MyCollection = new Collection();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                cmbDeviceID.Items.Clear();
                cmbSensorID.Items.Clear();
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
                        cmbDeviceID.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevices = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MyIPDevices.Caption;
                        MyItem.Value = MyIPDevices.ID.ToString();
                        MyItem.Selected = false;
                        cmbDeviceID.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevices = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MyOtherDevices.Caption;
                        MyItem.Value = MyOtherDevices.ID.ToString();
                        MyItem.Selected = false;
                        cmbDeviceID.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                    {
                        LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPManager = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MySNMPManager.Caption;
                        MyItem.Value = MySNMPManager.ID.ToString();
                        MyItem.Selected = false;
                        cmbDeviceID.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MySensor.Caption;
                        MyItem.Value = MySensor.ID.ToString();
                        MyItem.Selected = false;
                        cmbSensorID.Items.Add(MyItem);
                    }
                }
            }
        }

        protected void btnnSend_Click(object sender, EventArgs e)
            {
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                //if (MyIPMonPageSecure > MyUser.UserLevel)
                //{
                //    Response.Redirect("NotAuthorisedEdit.aspx");
                //}

                if (string.IsNullOrEmpty(this.txtName.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please Supply a Name.";

                    txtAlertID.Focus();

                    return;
                }

                if (string.IsNullOrEmpty(this.cmbSensorID.SelectedValue) & string.IsNullOrEmpty(this.cmbDeviceID.SelectedValue))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please Select Sensor or Device.";
                    txtAlertID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.cmbField.SelectedValue) & string.IsNullOrEmpty(this.cmbField.SelectedValue))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please Select Field.";
                    txtAlertID.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.txtCheckValue.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please enter a Value.";
                    txtAlertID.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtOrder.Text))
                {
                    this.txtOrder.Text = "0";
                }
                int n11 = 11;
                int n12 = 12;
                int n13 = 13;
                if ((this.TestType.SelectedValue == n11.ToString()) | (this.TestType.SelectedValue == n12.ToString()) | (this.TestType.SelectedValue == n13.ToString()))
                {
                    if (string.IsNullOrEmpty(this.TxtExtra.Text))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please enter a String Value to check.";
                        txtAlertID.Focus();
                        return;
                    }
                }
                LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef();
                MyAlert.Name = this.txtName.Text;
                MyAlert.SensorID = Convert.ToInt32(this.cmbSensorID.SelectedValue);
                MyAlert.DeviceID = Convert.ToInt32(this.cmbDeviceID.SelectedValue);
                MyAlert.TestType = (LiveMonitoring.IRemoteLib.TestType)Convert.ToInt32(this.TestType.SelectedValue);
                MyAlert.Field = Convert.ToInt32(this.cmbField.SelectedValue);
                MyAlert.CheckValue = Convert.ToDouble(this.txtCheckValue.Text);
                MyAlert.HoldPeriod = Convert.ToInt32(this.txtHoldPeriod.Text);
                MyAlert.AlertID = Convert.ToInt32(this.txtAlertID.Text);
                MyAlert.Comparison = this.Comparison.Items[0].Selected;
                MyAlert.Order = Convert.ToInt32(this.txtOrder.Text);
                MyAlert.Extra = this.TxtExtra.Text;
                MyAlert.Extra1 = this.TxtExtra1.Text;
                if (!string.IsNullOrEmpty(this.TxtExtra2.Text))
                {
                    MyAlert.Extra2 = Convert.ToDouble(this.TxtExtra2.Text);
                    MyAlert.Extra3 = Convert.ToDouble(this.TxtExtra3.Text);
                }
                MyAlert.TabularCnt = Convert.ToInt32(this.txtTabularCnt.Text);
                if (cmbFieldComp.Visible == true)
                {
                    MyAlert.Field1 = Convert.ToInt32(cmbFieldComp.SelectedValue);
                }
                else
                {
                    MyAlert.Field1 = 0;
                }
                MyAlert.IsTemplate = this.chkSensAlertTemplate.Checked;
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                //Dim Myfunc As New LiveMonitoring.SharedFuncs


                if (MyRem.LiveMonServer.AddNewAlertThreshhold(MyAlert) == false)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Add ThreshHold Failed.";
                    txtAlertID.Focus();
                    try
                    {
                        MyRem.WriteLog("Add ThreshHold Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "Add ThreshHold Succeeded.";
                    txtAlertID.Focus();
                    try
                    {
                        MyRem.WriteLog("Add ThreshHold Succeeded", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                    }
                    catch (Exception ex)
                    {
                    }
                    lblErr.Visible = false;
                    this.txtName.Text = "";
                    this.TxtExtra.Text = "";
                    this.TxtExtra1.Text = "";
                    this.TxtExtra2.Text = "";
                    this.TxtExtra3.Text = "";
                    this.txtOrder.Text = "0";
                    //Me.txtAlertID.Text = ""
                    LoadGrid();
                    this.txtCheckValue.Text = "";
                    this.txtName.Text = "";
                }
            }

        protected void btnnFinnished_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx?AlertID=" + txtAlertID.Text);
        }

        public void LoadGrid()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsThreashholds(Convert.ToInt32(this.txtAlertID.Text));
            //LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = default(LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef);
            ClearThreasholdRows();

            foreach (LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert in MyCollectionAlerts)
            {
                try
                {
                    AddThreasholdRows((new string[] {
                        MyAlert.Order.ToString(),
                        MyAlert.Name,
                        MyAlert.SensorID.ToString(),
                        MyAlert.DeviceID.ToString(),
                        MyAlert.Field.ToString(),
                        MyAlert.TestType.ToString(),
                        MyAlert.CheckValue.ToString(),
                        MyAlert.HoldPeriod.ToString(),
                        MyAlert.Comparison.ToString(),
                        MyAlert.Extra,
                        MyAlert.Extra1,
                        MyAlert.Extra2.ToString(),
                        MyAlert.Extra3.ToString(),
                        MyAlert.ID.ToString(),
                        MyAlert.TabularCnt.ToString(),
                        MyAlert.Field1.ToString(),
                        MyAlert.Field2.ToString(),
                        MyAlert.GroupID.ToString(),
                        MyAlert.LocatonID.ToString(),
                        MyAlert.IsTemplate.ToString()
                    }));
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void AddThreasholdRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();

            if (Session["myThreshtable"] == null == false)
            {
                dt = (DataTable)Session["myThreshtable"];
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Order", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("SensorID", typeof(int));
                dt.Columns.Add("DeviceID", typeof(int));
                dt.Columns.Add("Field", typeof(int));
                dt.Columns.Add("TestType", typeof(int));
                dt.Columns.Add("CheckValue", typeof(double));
                dt.Columns.Add("HoldPeriod", typeof(int));
                dt.Columns.Add("Comparison", typeof(int));
                dt.Columns.Add("Extra Data", typeof(string));
                dt.Columns.Add("Extra Data1", typeof(string));
                dt.Columns.Add("Extra Data2", typeof(string));
                dt.Columns.Add("Extra Data3", typeof(string));
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("TabularCnt", typeof(string));
                dt.Columns.Add("Field1", typeof(string));
                dt.Columns.Add("Field2", typeof(string));
                dt.Columns.Add("GroupID", typeof(string));
                dt.Columns.Add("LocatonID", typeof(string));
                dt.Columns.Add("IsTemplate", typeof(string));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];

            try
            {
                Row[2] = RowVals[2];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[3] = RowVals[3];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[4] = RowVals[4];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[5] = RowVals[5];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[6] = RowVals[6];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[7] = RowVals[7];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[8] = RowVals[8];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[9] = RowVals[9];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[10] = RowVals[10];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[11] = RowVals[11];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[12] = RowVals[12];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[13] = RowVals[13];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[14] = RowVals[14];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[15] = RowVals[15];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[16] = RowVals[16];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[17] = RowVals[17];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[18] = RowVals[18];

            }
            catch (Exception ex)
            {
            }
            try
            {
                Row[19] = RowVals[19];

            }
            catch (Exception ex)
            {
            }
            dt.Rows.Add(Row);
            Session["myThreshtable"] = dt;
            GridThreasholdBind(dt);
        }

        public void ClearThreasholdRows()
        {
            DataTable dt = new DataTable();
            Session["myThreshtable"] = dt;
            GridThreasholdBind(dt);
        }

        public void GridThreasholdBind(DataTable dt)
        {
            GridThreashholds.DataSource = dt;
            GridThreashholds.DataKeyNames = (new string[] { "ID" });
            GridThreashholds.DataBind();
        }

        protected void cmbDeviceID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadFields();
        }

        public void LoadFields()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            cmbField.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    if (MyCamera.ID == Convert.ToInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "Camera";
                        int v = 0;
                        MyItem.Value = v.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevices = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    if (MyIPDevices.ID == Convert.ToInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "IPDevice";
                        int v = 0;
                        MyItem.Value = v.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevices = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                    if (MyOtherDevices.ID == Convert.ToInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "OtherDevice";
                        int v = 0;
                        MyItem.Value = v.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }

                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPManager = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                    if (MySNMPManager.ID == Convert.ToInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "SNMP";
                        int v = 0;
                        MyItem.Value = v.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }
                }
            }
        }

        protected void cmbSensorID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            cmbField.Items.Clear();
            cmbFieldComp.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (MySensor.ID == Convert.ToInt32(cmbSensorID.SelectedValue))
                    {
                        if (MySensor.Fields.Count > 0)
                        {
                            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                            {
                                try
                                {
                                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                    MyItem.Text = MyField.FieldName;
                                    MyItem.Value = MyField.FieldNumber.ToString();
                                    MyItem.Selected = false;
                                    cmbField.Items.Add(MyItem);
                                    cmbFieldComp.Items.Add(MyItem);

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else //no fields try create them
                        {
                            try
                            {
                                LiveMonitoring.IRemoteLib.SensorFieldsDefault MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDefault(MySensor.Type);
                                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MyFields.FieldsList)
                                {
                                    MyField.SensorID = MySensor.ID;
                                    try
                                    {
                                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                        MyItem.Text = MyField.FieldName;
                                        MyItem.Value = MyField.FieldNumber.ToString();
                                        MyItem.Selected = false;
                                        cmbField.Items.Add(MyItem);
                                        cmbFieldComp.Items.Add(MyItem);

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    //and save it
                                    try
                                    {
                                        MyRem.LiveMonServer.EditSensorField(MyField);
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
        }

        protected void TestType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (TestType.SelectedIndex)
            {
                case (int)LiveMonitoring.IRemoteLib.TestType.ConsumptionAlert - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.OutOfAvergeBand - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Averge band Percentage D=10";
                    int val = 10;
                    TxtExtra3.Text = val.ToString();
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.AboveAveragePercent - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.BellowAveragePercent - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.ProjectedHighConsumption - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Projected units (val)";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.ProjectedLowConsumption - 1:
                    lblExtra.Text = "Period Setting units(n-minute[d],H-Hour,D-Day,M-month,Y-Year)";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Period units back(val)";
                    lblExtra3.Text = "Projected units (val)";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldEquals - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    cmbFieldComp.Visible = true;
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldGreater - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    cmbFieldComp.Visible = true;
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldSmaller - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    cmbFieldComp.Visible = true;
                    lblExtra3.Text = "Extra3 Val";
                    break;
                default:
                    cmbFieldComp.Visible = false;
                    TxtExtra2.Visible = true;
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Must Occure (Hours)";
                    lblExtra3.Text = "Extra3 Val";
                    int vall = 10;
                    TxtExtra3.Text = vall.ToString();
                    break;
            }
        }

        protected void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSensorFieldsValue();
        }

        public void LoadSensorFieldsValue()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            curVals.InnerText = "Current Values:";
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (MySensor.ID == Convert.ToInt32(cmbSensorID.SelectedValue))
                    {
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                        {
                            if (MyField.FieldNumber == Convert.ToInt32(cmbField.SelectedValue))
                            {
                                curVals.InnerText += " Val:" + MyField.LastValue.ToString() + ": Other:" + MyField.LastOtherValue + ": Tab:" + MyField.TabularRowNo.ToString() + " ";
                            }
                        }
                    }
                }
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

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }
    }
}