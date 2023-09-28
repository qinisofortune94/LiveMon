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
    public partial class EditAlertthreshholds : System.Web.UI.Page
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                //MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session("UserDetails");
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    int reqAlertID = 0;
                    if (!string.IsNullOrEmpty(Request.QueryString["AlertID"]))
                    {
                        reqAlertID = Convert.ToInt32(Request.QueryString["AlertID"]);
                    }
                    else
                    {
                    }

                    this.txtAlertID.Text = reqAlertID.ToString();
                    try
                    {
                        LiveMonitoring.testing test = new LiveMonitoring.testing();
                        test.getValues(TestType);
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
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
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        LoadGrid();

                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void cmdSend_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please Name the threashold !";
                return;
            }
            if (string.IsNullOrEmpty(txtID.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select the threashold to edit!";
                return;
            }
            if (Convert.ToInt32(this.TestType.SelectedValue) == 11 | Convert.ToInt32(this.TestType.SelectedValue) == 12 | Convert.ToInt32(this.TestType.SelectedValue) == 13)
            {
                if (string.IsNullOrEmpty(this.TxtExtra.Text))
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Please enter a String Value to check!";
                    return;
                }
            }
            LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef();
            MyAlert.ID = Convert.ToInt32(txtID.Text);
            MyAlert.Name = this.txtName.Text;
            MyAlert.SensorID = Convert.ToInt32(this.cmbSensorID.SelectedValue);
            MyAlert.DeviceID = Convert.ToInt32(this.cmbDeviceID.SelectedValue);
            MyAlert.TestType = (LiveMonitoring.IRemoteLib.TestType)Convert.ToInt32(this.TestType.SelectedValue);
            MyAlert.CheckValue = Convert.ToDouble(this.txtCheckValue.Text);
            MyAlert.HoldPeriod = Convert.ToInt32(this.txtHoldPeriod.Text);
            if (!string.IsNullOrEmpty(this.cmbField.SelectedValue))
            {
                MyAlert.Field = Convert.ToInt32(this.cmbField.SelectedValue);
            }
            else
            {
                MyAlert.Field = 1;
                //defalt to 1 for first field
            }
            MyAlert.AlertID = Convert.ToInt32(this.txtAlertID.Text);
            MyAlert.Comparison = this.Comparison.Items[0].Selected;
            MyAlert.Order = Convert.ToInt32(this.txtOrder.Text);
            MyAlert.Extra = this.TxtExtra.Text;
            MyAlert.Extra1 = this.TxtExtra1.Text;

            if (!string.IsNullOrEmpty(this.TxtExtra2.Text))
                MyAlert.Extra2 = Convert.ToInt32(this.TxtExtra2.Text);
            if (!string.IsNullOrEmpty(this.TxtExtra3.Text))
                MyAlert.Extra3 = Convert.ToInt32(this.TxtExtra3.Text);
            MyAlert.TabularCnt = Convert.ToInt32(this.txtTabularCnt.Text);
            if (cmbFieldComp.Visible == true)
            {
                MyAlert.Field1 = Convert.ToInt32(cmbFieldComp.SelectedValue);
            }
            else
            {
                MyAlert.Field1 = 0;
            }
            //MyAlert.ID = Me.GridThreashholds.s
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs
            MyAlert.IsTemplate = this.chkSensAlertTemplate.Checked;
            if (MyRem.LiveMonServer.EditAlertThreshhold(MyAlert) == false)
            {
                try
                {
                    MyRem.WriteLog("Edit Alert ThreshHold Failed", "User:" + MyUser.ID.ToString() + "|" + MyAlert.ID.ToString());

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error!";
            }
            else
            {
                try
                {
                    MyRem.WriteLog("Edit Alert ThreshHold Succeed", "User:" + MyUser.ID.ToString() + "|" + MyAlert.ID.ToString() + "|" + MyAlert.ToString());

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = false;
                this.txtName.Text = "";
                this.TxtExtra.Text = "";
                this.TxtExtra1.Text = "";
                this.TxtExtra2.Text = "";
                this.TxtExtra3.Text = "";
                this.txtOrder.Text = "";
                //Me.txtAlertID.Text = ""
                this.txtCheckValue.Text = "";
                this.txtName.Text = "";
                LoadGrid();
            }
        }

        protected void cmdFinnished_Click(object sender, EventArgs e)
        {
            txtName.Text = "Fin";
            Response.Redirect("EditAlerts.aspx?AlertID=" + txtAlertID.Text);
        }

        public void LoadGrid()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            if (string.IsNullOrEmpty(this.txtAlertID.Text))
            {
                return;
            }
            MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsThreashholds(Convert.ToInt32(this.txtAlertID.Text));
            //LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = default(LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef);
            //GridThreashholdsOld.Clear()
            ClearThreasholdRows();
            if ((MyCollectionAlerts == null) == true)
            {
                return;
            }
            foreach (LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert in MyCollectionAlerts)
            {
                try
                {
                    int r = Convert.ToInt32(MyAlert.TestType);
                    AddThreasholdRows((new string[] {
                        MyAlert.Order.ToString(),
                        MyAlert.Name,
                        MyAlert.SensorID.ToString(),
                        MyAlert.DeviceID.ToString(),
                        MyAlert.Field.ToString(),
                        r.ToString(),
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
            DataRow Row = null;
            DataTable dt = new DataTable();
            //= CType(Session("mytable"), DataTable)
            //ListFiles()

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
            try
            {
                GridThreashholds.DataSource = dt;
                GridThreashholds.DataKeyNames = (new string[] { "ID" });
                GridThreashholds.DataBind();

            }
            catch (Exception ex)
            {
            }

        }

        protected void GridThreashholds_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridThreashholds.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["myThreshtable"];
            GridThreasholdBind(dt);
        }

        protected void GridThreashholds_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            for (int j = 0; j <= GridThreashholds.Rows.Count - 1; j++)
            {
                //Me.txtOrder.Text = myrow.Item(9)
                this.txtOrder.Text = GridThreashholds.Rows[0].Cells[1].Text;
                this.TxtExtra.Text = GridThreashholds.Rows[0].Cells[10].Text;
                this.TxtExtra1.Text = GridThreashholds.Rows[0].Cells[11].Text;
                this.TxtExtra2.Text = GridThreashholds.Rows[0].Cells[12].Text;
                this.TxtExtra3.Text = GridThreashholds.Rows[0].Cells[13].Text;
                this.txtTabularCnt.Text = GridThreashholds.Rows[0].Cells[15].Text;
                this.txtCheckValue.Text = GridThreashholds.Rows[0].Cells[7].Text;
                this.txtHoldPeriod.Text = GridThreashholds.Rows[0].Cells[8].Text;
                this.cmbField.SelectedValue = GridThreashholds.Rows[0].Cells[5].Text;
                this.TestType.SelectedValue = GridThreashholds.Rows[0].Cells[6].Text;
                this.chkSensAlertTemplate.Checked = bool.Parse(GridThreashholds.Rows[0].Cells[20].Text);


                if (Convert.ToInt32(GridThreashholds.Rows[0].Cells[3].Text) > 0)
                {
                    try
                    {
                        this.cmbSensorID.SelectedValue = GridThreashholds.Rows[0].Cells[3].Text;
                        LoadSensorFields();


                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (Convert.ToInt32(GridThreashholds.Rows[0].Cells[4].Text) > 0)
                {
                    try
                    {
                        this.cmbDeviceID.SelectedValue = GridThreashholds.Rows[0].Cells[4].Text;


                    }
                    catch (Exception ex)
                    {
                    }
                    //LoadFields()
                }


                try
                {
                    SetLabels(Convert.ToInt32(this.TestType.SelectedValue) - 1);

                }
                catch (Exception ex)
                {
                }

                try
                {
                    if (Information.IsDBNull(GridThreashholds.Rows[0].Cells[9].Text) == false)
                    {
                        if (bool.Parse(GridThreashholds.Rows[0].Cells[9].Text) == true)
                        {
                            this.Comparison.Items[0].Selected = true;
                        }
                        else
                        {
                            this.Comparison.Items[0].Selected = false;
                        }
                    }
                    else
                    {
                        this.Comparison.Items[0].Selected = true;
                    }

                }
                catch (Exception ex)
                {
                    this.Comparison.Items[0].Selected = true;
                }

                try
                {
                    this.cmbFieldComp.SelectedValue = GridThreashholds.Rows[0].Cells[16].Text;

                }
                catch (Exception ex)
                {
                }

                Session["myThreshtableKey"] = GridThreashholds.SelectedDataKey.Value;
                LoadGridRow(Convert.ToInt32(GridThreashholds.SelectedDataKey.Value));
            }
        }

        private void LoadGridRow(int RowID)
        {
            //Dim myrow = Alertsgrid.SelectedRow
            //dt.Rows(row.DataItemIndex)("PaintName") = (CType((row.Item(1).Controls(0)), TextBox)).Text
            txtID.Text = RowID.ToString();
            DataTable dt = new DataTable();
            dt = (DataTable)Session["myThreshtable"];
            DataRow[] MyRows = null;
            DataRow myrow = null;
            //MyRow = dt.Rows.Find(RowID)
            MyRows = dt.Select("ID =" + RowID.ToString());
            //For Each MyRow In dt.Rows

            //Next
            try
            {
                if ((MyRows == null) == false)
                {
                    myrow = MyRows[0];

                    txtID.Text = RowID.ToString();
                    //this.txtOrder.Text = GridThreashholds.Rows[0].Cells[1].Text;
                    this.txtName.Text = (string)myrow[1];
                    //if ((int)myrow[2] > 0)
                    //{
                    //    try
                    //    {
                    //        this.cmbSensorID.SelectedValue = (string)myrow[2];
                    //        LoadSensorFields();


                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //}
                    //if ((int)myrow[3] > 0)
                    //{
                    //    try
                    //    {
                    //        this.cmbDeviceID.SelectedValue = (string)myrow[3];


                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //    //LoadFields()
                    //}
                    //this.cmbField.SelectedValue = (string)myrow[4];
                    //this.TestType.SelectedValue = (string)myrow[5];
                    //try
                    //{
                    //    SetLabels(Convert.ToInt32(this.TestType.SelectedValue) - 1);

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //RaiseEvent TestType()
                    //this.txtCheckValue.Text = (string)myrow[6];
                    //this.txtHoldPeriod.Text = (string)myrow[7];
                    //try
                    //{
                    //    if (Information.IsDBNull(myrow[8]) == false)
                    //    {
                    //        if ((bool)myrow[8] == true)
                    //        {
                    //            this.Comparison.Items[0].Selected = true;
                    //        }
                    //        else
                    //        {
                    //            this.Comparison.Items[0].Selected = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        this.Comparison.Items[0].Selected = true;
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    this.Comparison.Items[0].Selected = true;
                    //}


                    
                    //try
                    //{
                    //    //this.txtTabularCnt.Text = (string)myrow[14];

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //try
                    //{
                    //    this.cmbFieldComp.SelectedValue = (string)myrow[15];

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //try
                    //{
                    //    //this.chkSensAlertTemplate.Checked = (bool)myrow[19];

                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }
            }
            catch (Exception ex)
            {

            }
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
                    if (MyCamera.ID == Convert.ToUInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "Camera";
                        int zero = 0;
                        MyItem.Value = zero.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevices = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    if (MyIPDevices.ID == Convert.ToUInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "IPDevice";
                        int zero = 0;
                        MyItem.Value = zero.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevices = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                    if (MyOtherDevices.ID == Convert.ToUInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "Other";
                        int zero = 0;
                        MyItem.Value = zero.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }

                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPManager = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                    if (MySNMPManager.ID == Convert.ToUInt32(cmbDeviceID.SelectedValue))
                    {
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = "SNMP";
                        int zero = 0;
                        MyItem.Value = zero.ToString();
                        MyItem.Selected = false;
                        cmbField.Items.Add(MyItem);
                    }
                }
            }
        }

        protected void cmbSensorID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadSensorFields();
        }

        public void LoadSensorFields()
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
                    if (MySensor.ID == Convert.ToUInt32(cmbSensorID.SelectedValue))
                    {
                        //LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
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
                            //no fields try create them
                        }
                        else
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

        protected void cmdSaveNew_Click(object sender, EventArgs e)
        {
            //If txtID.Text <> "" Then
            //    If MsgBox("Are you sure to save edit as new threashhold ?", MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then
            //        Exit Sub
            //    End If
            //End If
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please Name the threashold !";
                return;
            }
            if (string.IsNullOrEmpty(this.txtAlertID.Text))
            {
                errorMessage.Visible = true;
                this.lblError.Text = "Please select alert!";
                return;
            }
            if (string.IsNullOrEmpty(this.txtHoldPeriod.Text))
            {
                errorMessage.Visible = true;
                this.lblError.Text = "Please enter holdperiod!";
                return;
            }
            if (string.IsNullOrEmpty(this.txtOrder.Text))
            {
                errorMessage.Visible = true;
                this.lblError.Text = "Please enter Order!";
                return;
            }
            if (Convert.ToInt32(this.TestType.SelectedValue) == 11 | Convert.ToInt32(this.TestType.SelectedValue) == 12 | Convert.ToInt32(this.TestType.SelectedValue) == 13)
            {
                if (string.IsNullOrEmpty(this.TxtExtra.Text))
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Please enter a String Value to check!";
                    return;
                }
            }

            LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef();
            MyAlert.Name = this.txtName.Text;
            MyAlert.SensorID = Convert.ToInt32(this.cmbSensorID.SelectedValue);
            MyAlert.DeviceID = Convert.ToInt32(this.cmbDeviceID.SelectedValue);
            MyAlert.TestType = (LiveMonitoring.IRemoteLib.TestType)Convert.ToInt32(this.TestType.SelectedValue);
            MyAlert.CheckValue = Convert.ToInt32(this.txtCheckValue.Text);
            if (!string.IsNullOrEmpty(this.cmbField.SelectedValue))
            {
                MyAlert.Field = Convert.ToInt32(this.cmbField.SelectedValue);
            }
            else
            {
                MyAlert.Field = 1;
            }
            MyAlert.HoldPeriod = Convert.ToInt32(this.txtHoldPeriod.Text);
            MyAlert.AlertID = Convert.ToInt32(this.txtAlertID.Text);
            MyAlert.Comparison = this.Comparison.Items[0].Selected;
            MyAlert.Order = Convert.ToInt32(this.txtOrder.Text);
            MyAlert.Extra = this.TxtExtra.Text;
            MyAlert.Extra1 = this.TxtExtra1.Text;

            if (!string.IsNullOrEmpty(this.TxtExtra2.Text))
                MyAlert.Extra2 = Convert.ToInt32(this.TxtExtra2.Text);
            if (!string.IsNullOrEmpty(this.TxtExtra3.Text))
                MyAlert.Extra3 = Convert.ToInt32(this.TxtExtra3.Text);
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
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyRem.LiveMonServer.AddNewAlertThreshhold(MyAlert) == false)
            {
                try
                {
                    MyRem.WriteLog("Add NewE Alert ThreshHold Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text);

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error!";
            }
            else
            {
                try
                {
                    MyRem.WriteLog("Add NewE Alert ThreshHold Succeed", "User:" + MyUser.ID.ToString() + "|" + this.txtName.Text + "|" + MyAlert.ToString());

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = false;
                this.txtName.Text = "";
                this.TxtExtra.Text = "";
                this.TxtExtra1.Text = "";
                this.TxtExtra2.Text = "";
                this.TxtExtra3.Text = "";
                this.txtOrder.Text = "0";
                //Me.txtAlertID.Text = ""
                this.txtCheckValue.Text = "0";
                this.txtName.Text = "";
                LoadGrid();
            }

        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;
            this.txtName.Text = "";
            this.TxtExtra.Text = "";
            this.TxtExtra1.Text = "";
            this.TxtExtra2.Text = "";
            this.TxtExtra3.Text = "";
            this.txtOrder.Text = "0";
            //Me.txtAlertID.Text = ""
            this.txtCheckValue.Text = "0";
            this.txtName.Text = "";
            LoadGrid();
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetDeleteLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedDelete.aspx");
            }
            if (string.IsNullOrEmpty(this.txtID.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select a Threshhold to delete first!";

                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs
            try
            {
                MyRem.WriteLog("Delete Alert ThreshHold", "User:" + MyUser.ID.ToString() + "|" + this.txtID.Text);

            }
            catch (Exception ex)
            {
            }
            MyRem.LiveMonServer.DeleteAlertThreshhold(Convert.ToInt32(this.txtID.Text));
            LoadGrid();

        }

        protected void TestType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SetLabels(TestType.SelectedIndex);
        }

        private void SetLabels(int SelectedIndex)
        {
            switch (SelectedIndex)
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
                    int ten = 10;
                    TxtExtra3.Text = ten.ToString();
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
                    // lblExtra2.Text = "Field 2"
                    cmbFieldComp.Visible = true;
                    // TxtExtra2.Visible = False
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldGreater - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    // lblExtra2.Text = "Field 2"
                    cmbFieldComp.Visible = true;
                    //  TxtExtra2.Visible = False
                    lblExtra3.Text = "Extra3 Val";
                    break;
                case (int)LiveMonitoring.IRemoteLib.TestType.PercentFieldSmaller - 1:
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    //  lblExtra2.Text = "Field 2"
                    cmbFieldComp.Visible = true;
                    //  TxtExtra2.Visible = False
                    lblExtra3.Text = "Extra3 Val";
                    break;
                default:
                    cmbFieldComp.Visible = false;
                    TxtExtra2.Visible = true;
                    lblExtra.Text = "Extra String";
                    lblExtra1.Text = "Extra String1";
                    lblExtra2.Text = "Must Occure (Hours)";
                    lblExtra3.Text = "Extra3 Val";
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
                        //LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                        {
                            if (MyField.FieldNumber == Convert.ToInt32(cmbField.SelectedValue))
                            {
                                curVals.InnerText += "val:" + MyField.LastValue.ToString() + "|" + MyField.LastOtherValue + ":Tab:" + MyField.TabularRowNo.ToString();
                            }
                        }
                    }
                }
            }
        }

        public void EditAlertThreshHolds()
        {
            Load += Page_Load;
        }
    }
}