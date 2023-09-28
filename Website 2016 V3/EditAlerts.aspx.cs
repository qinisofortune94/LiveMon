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
    public partial class EditAlerts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                EditLoadPage();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }
        ///Method to go to form load
        public void EditLoadPage()
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
           
            if (IsPostBack == false)
            {
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.getItemValue(this.AlertType, this.cmbSensor1ID, this.cmbSensor2ID);
                
                Collection MyCollection = new Collection();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                
                object MyObject1 = null;
                AlertCameraImages.Items.Clear();
              
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
                        cmbSensor1ID.Items.Add(MyItem);
                        cmbSensor2ID.Items.Add(MyItem);
                    }
                }
                LoadAlertsGrid(MyRem);
                LoadallContacts();
            }
        }

        ///Load alerts to a gridview
        private void LoadAlertsGrid(LiveMonitoring.GlobalRemoteVars MyRem)
        {
            Collection MyCollectionAlerts = new Collection();
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllAlerts();
            ClearRows();
            bool AddRow = true;

            foreach (LiveMonitoring.IRemoteLib.AlertDetails MyAlert in MyCollectionAlerts)
            {
                if (((((string)Session["AlertFilterText"] == "") && ((string)Session["AlertFilterText"] != "")) || (Session["AlertFilterText"] == null)))
                {
                    AddRow = true;
                }
                else
                {
                    AddRow = false;
                    switch (Session["AlertFilterSelect"].ToString())
                    {
                        case "0":
                            if (MyAlert.AlertMessage.ToUpper().Contains(Session["AlertFilterText"].ToString().ToUpper()))
                            {
                                AddRow = true;
                            }
                            break;
                        case "1":
                            if (ContainsSensorName(MyAlert.SensorValueID1, MyAlert.SensorValueID2, MyRem, Session["AlertFilterText"].ToString()))
                            {
                                AddRow = true;
                            }
                            break;
                        case "2":
                            if (ContainsCameraName(MyAlert.SensorValueID1, MyAlert.SensorValueID2, MyRem, Session["AlertFilterText"].ToString()))
                            {
                                AddRow = true;
                            }
                            break;
                        case "3":
                            Collection MyCollectionAlertsThres = MyRem.LiveMonServer.GetSpecificAlertsThreashholds(Convert.ToInt32(MyAlert.AlertId));
                            if (ContainsThreshName(MyCollectionAlertsThres, MyRem, Session["AlertFilterText"].ToString()))
                            {
                                AddRow = true;
                            }

                            break;
                    }
                }
                if (AddRow)
                {
                    try
                    {
                        AddRows((new string[] {
                            MyAlert.AlertType.ToString(),
                            MyAlert.AlertMessage,
                            MyAlert.IncludeImage.ToString(),
                            MyAlert.CameraID1.ToString(),
                            MyAlert.CameraID2.ToString(),
                            MyAlert.SensorValueID1.ToString(),
                            MyAlert.SensorValueID2.ToString(),
                            MyAlert.SensorValueID3.ToString(),
                            MyAlert.SensorValueID4.ToString(),
                            MyAlert.Enabled.ToString(),
                            MyAlert.SendNormal.ToString(),
                            MyAlert.Camera1Delay.ToString(),
                            MyAlert.Camera2Delay.ToString(),
                            MyAlert.AlertId.ToString()
                        }));
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private bool ContainsThreshName(Collection MyThreshs, LiveMonitoring.GlobalRemoteVars MyRem, string SearchString)
        {
            bool functionReturnValue = false;
            try
            {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;

                if ((MyCollection == null) == false)
                {
                    foreach (LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef MyThres in MyThreshs)
                    {
                        foreach (object MyObject1_loopVariable in MyCollection)
                        {
                            MyObject1 = MyObject1_loopVariable;
                            try
                            {
                                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                                {
                                    LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                    if (MyThres.SensorID == Mysensor.ID)
                                    {
                                        bool AddSens = true;
                                        if (Mysensor.Caption.ToUpper().Contains(SearchString.ToUpper()) == true)
                                        {
                                            return true;
                                            return functionReturnValue;
                                        }
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
            catch (Exception ex)
            {
            }
            return false;
            return functionReturnValue;
        }

        private bool ContainsSensorName(int SensorValueID1, int SensorValueID2, LiveMonitoring.GlobalRemoteVars MyRem, string SearchString)
        {
            bool functionReturnValue = false;
            try
            {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            
                object MyObject1 = null;

                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;

                                bool AddSens = true;
                                if (Mysensor.Caption.ToUpper().Contains(SearchString.ToUpper()) == true)
                                {
                                    return true;
                                    return functionReturnValue;
                                }
                            }
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
            return false;
            return functionReturnValue;
        }

        private bool ContainsCameraName(int SensorValueID1, int SensorValueID2, LiveMonitoring.GlobalRemoteVars MyRem, string SearchString)
        {
            bool functionReturnValue = false;
            try
            {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
               
                object MyObject1 = null;

                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails Mysensor = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                if (Mysensor.Caption.ToUpper().Contains(SearchString.ToUpper()) == true)
                                {
                                    return true;
                                    return functionReturnValue;
                                }
                            }
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
            return false;
            return functionReturnValue;
        }

        public void AddRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();

            if (Session["mytable"] == null == false)
            {
                dt = (DataTable)Session["mytable"];
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Message", typeof(string));
                dt.Columns.Add("Include Image", typeof(bool));
                dt.Columns.Add("Camera 1 ID", typeof(int));
                dt.Columns.Add("Camera 2 ID", typeof(int));
                dt.Columns.Add("Sensor 1 ID", typeof(int));
                dt.Columns.Add("Sensor 2 ID", typeof(int));
                dt.Columns.Add("Sensor 3 ID", typeof(int));
                dt.Columns.Add("Sensor 4 ID", typeof(int));
                dt.Columns.Add("Enabled", typeof(bool));
                dt.Columns.Add("SendNormal", typeof(bool));
                dt.Columns.Add("Delay1", typeof(int));
                dt.Columns.Add("Delay2", typeof(int));
                dt.Columns.Add("ID", typeof(int));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = Convert.ToBoolean(RowVals[2]);
            Row[3] = Convert.ToInt32(RowVals[3]);
            Row[4] = Convert.ToInt32(RowVals[4]);
            Row[5] = Convert.ToInt32(RowVals[5]);
            Row[6] = Convert.ToInt32(RowVals[6]);
            Row[7] = Convert.ToInt32(RowVals[7]);
            Row[8] = Convert.ToInt32(RowVals[8]);
            Row[9] = Convert.ToBoolean(RowVals[9]);
            Row[10] = Convert.ToBoolean(RowVals[10]);
            Row[11] = Convert.ToInt32(RowVals[11]);
            Row[12] = Convert.ToInt32(RowVals[12]);
            Row[13] = Convert.ToInt32(RowVals[13]);
            dt.Rows.Add(Row);
            Session["mytable"] = dt;
            GridBind(dt);

        }

        public void ClearRows()
        {
            DataTable dt = new DataTable();
            Session["mytable"] = dt;
            GridBind(dt);
        }

        public void GridBind(DataTable dt)
        {
            Alertsgrid.DataSource = dt;
            Alertsgrid.DataKeyNames = (new string[] { "ID" });
            Alertsgrid.DataBind();
        }

        private string returnalertstring(int alerttype)
        {
            string[] returnValue = null;
            returnValue = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
            Array items = default(Array);
            items = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
            string item = "";

            for (int i = 0; i <= items.Length - 1; i++)
            {
                int myint = 1;
                myint = myint << i;
                if ((alerttype & myint) > 0)
                {
                    item += (returnValue[i]) + " ";
                }
            }
            return item;
        }

        public void LoadallContacts()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllContacts();
                     
            cmbContacts.Items.Clear();
            if ((MyCollectionAlerts == null) == true)
            {
                return;
            }
            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyContact in MyCollectionAlerts)
            {
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = MyContact.ContactName;
                MyItem.Value = MyContact.ID.ToString();
                if (cmbContacts.Items.Count == 0)
                    MyItem.Selected = true;
                cmbContacts.Items.Add(MyItem);
            }
        }

        public void LoadContactGrid(int AlertID)
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsContacts(AlertID);
            ClearContactRows();
            if ((MyCollectionAlerts == null) == true)
            {
                return;
            }

            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyAlert in MyCollectionAlerts)
            {
                try
                {
                    AddContactRows((new string[] {
                        MyAlert.ContactName,
                        MyAlert.Type.ToString(),
                        MyAlert.OutputParam.ToString(),
                        MyAlert.OutputParam1.ToString(),
                        MyAlert.OutputParam2.ToString(),
                        MyAlert.OutputParam3.ToString(),
                        MyAlert.OutputParam4.ToString() + "," + MyAlert.OutputParam5.ToString() + "," + MyAlert.OutputParam6.ToString() + "," + MyAlert.OutputParam7.ToString(),
                        MyAlert.ResendDelay.ToString(),
                        MyAlert.LinkID.ToString()
                    }));
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void AddContactRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();

            if (Session["myContacttable"] == null == false)
            {
                dt = (DataTable)Session["myContacttable"];
            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Cell", typeof(string));
                dt.Columns.Add("Pager", typeof(string));
                dt.Columns.Add("Other", typeof(string));
                dt.Columns.Add("Outputs", typeof(string));
                dt.Columns.Add("ResendDelay", typeof(string));
                dt.Columns.Add("ID", typeof(int));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];
            Row[5] = RowVals[5];
            Row[6] = RowVals[6];
            Row[7] = RowVals[7];
            Row[8] = RowVals[8];

            dt.Rows.Add(Row);
            Session["myContacttable"] = dt;
            GridContactBind(dt);
        }

        public void ClearContactRows()
        {
            DataTable dt = new DataTable();
            Session["myContacttable"] = dt;
            GridContactBind(dt);
        }

        public void GridContactBind(DataTable dt)
        {
            GridContacts.DataSource = dt;
            GridContacts.DataKeyNames = (new string[] { "ID" });
            GridContacts.DataBind();
        }

        protected void btnSubmitEdit_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            LiveMonitoring.IRemoteLib.AlertDetails MyAlert = new LiveMonitoring.IRemoteLib.AlertDetails();

            try
            {
                MyAlert.AlertId = Convert.ToDouble(this.txtID.Text);

            }
            catch (Exception ex)
            {
            }

            MyAlert.AlertMessage = this.AlertMessage.Text;
            int i = 0;
            //for (i = 0; i <= this.AlertType.Items.Count - 1; i++)
            //{
            //    if (this.AlertType.Items[i].Selected)
            //    {
            //        MyAlert.AlertType = MyAlert.AlertType | AlertType.Items[i].Value;
            //    }
            //}
            int result = 0;
            for (i = 0; i < this.AlertType.Items.Count; i++)
            {
                if (this.AlertType.Items[i].Selected == true)
                {
                    result += Convert.ToInt32(this.AlertType.Items[i].Value);
                }
            }
            MyAlert.AlertType = (LiveMonitoring.IRemoteLib.AlertDetails.AlertsType)result;

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

                        Alertsgrid.Focus();
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
                        Alertsgrid.Focus();
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
            bool MyALertID = MyRem.LiveMonServer.EditAlert(MyAlert);
            if (MyALertID == true)
            {
                try
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "Edit Alert Succeeded.";
                    Alertsgrid.Focus();
                    MyRem.WriteLog("Edit Alert Succeed", "User:" + MyUser.ID.ToString() + "|" + MyAlert.AlertId.ToString() + "|" + MyAlert.ToString());

                    divAlertType.Visible = false;
                    divMessages.Visible = false;
                    divAddAlertFields.Visible = false;
                    divLinkedContacts.Visible = false;
                    divSensorGrid.Visible = false;
                }
                catch (Exception ex)
                {
                }

                EditLoadPage();
            }
            else
            {
                try
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Edit Alert Failed.";
                    Alertsgrid.Focus();
                    MyRem.WriteLog("Edit Alert Failed", "User:" + MyUser.ID.ToString() + "|" + MyAlert.AlertId.ToString());

                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void cmdUpdateThreashholds_Click(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select an Alert to edit first.";
                Alertsgrid.Focus();
                return;
            }
            Response.Redirect("editalertthreshholds.aspx?AlertID=" + this.txtID.Text.ToString());

        }

        protected void cmdUpdateContacts_Click(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select an Alert to edit first.";
                Alertsgrid.Focus();
                return;
            }

            Response.Redirect("editalertcontact.aspx?AlertID=" + this.txtID.Text.ToString());

        }

        protected void btnDeleteEdit_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetDeleteLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            if (string.IsNullOrEmpty(this.txtID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select an Alert to delete first.";
                Alertsgrid.Focus();
                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                successMessage.Visible = true;
                lblSucces.Text = "Delete Alert Succeeded.";
                Alertsgrid.Focus();
                MyRem.WriteLog("Delete Alert Succeed", "User:" + MyUser.ID.ToString() + "|" + this.txtID.Text);

                divAlertType.Visible = false;
                divMessages.Visible = false;
                divAddAlertFields.Visible = false;
                divLinkedContacts.Visible = false;
                divSensorGrid.Visible = false;
            }
            catch (Exception ex)
            {
            }
            MyRem.LiveMonServer.DeleteAlert(Convert.ToInt32(this.txtID.Text));
            EditLoadPage();
        }

        protected void btnLinkContactEdit_Click(object sender, System.EventArgs e)
        {
            if (cmbContacts.SelectedIndex != -1 & !string.IsNullOrEmpty(this.txtID.Text))
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                try
                {
                    lblErr.Text = "";
                    lblErr.Visible = false;
                    MyRem.LiveMonServer.AddNewAlertContactLink(Convert.ToInt32(this.txtID.Text), Convert.ToInt32(cmbContacts.SelectedValue));
                    try
                    {
                        successMessage.Visible = true;
                        lblSucces.Text = "AddNewAlertContactLink Succeed.";
                        Alertsgrid.Focus();
                        MyRem.WriteLog("AddNewAlertContactLink Succeed", "User:" + MyUser.ID.ToString() + "|" + this.txtID.Text + "|" + Convert.ToString(cmbContacts.SelectedValue));

                    }
                    catch (Exception ex)
                    {
                    }
                    EditLoadPage();
                    LoadContactGrid(Convert.ToInt32(this.txtID.Text));
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "AddNewAlertContactLink Failed.";
                    Alertsgrid.Focus();
                    try
                    {
                        MyRem.WriteLog("AddNewAlertContactLink Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtID.Text + "|" + Convert.ToString(cmbContacts.SelectedValue));

                    }
                    catch (Exception ex1)
                    {
                    }
                }
            }
            else
            {
                lblErr.Visible = true;
                if (cmbContacts.SelectedIndex != -1)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Error Please select contact.";

                    Alertsgrid.Focus();
                }
                else
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Error Please select alert.";

                    Alertsgrid.Focus();
                }
            }
        }

        protected void cmbSensor1ID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadField1();
        }

        private void LoadField1()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            
            object MyObject1 = null;
            cmbField1.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (MySensor.ID == Convert.ToInt32(cmbSensor1ID.SelectedValue))
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
            if (cmbField1.Items.Count == 0 & Convert.ToInt32(cmbSensor1ID.SelectedValue) > 0)
            {
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = "Field1";
                int y = 1;
                MyItem.Value = y.ToString();
                MyItem.Selected = false;
                cmbField1.Items.Add(MyItem);
            }
        }

        protected void cmbSensor2ID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            loadfield2();

        }

        private void loadfield2()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            
            object MyObject1 = null;
            cmbField2.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (MySensor.ID == Convert.ToInt32(cmbSensor2ID.SelectedValue))
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
            if (cmbField2.Items.Count == 0 & Convert.ToInt32(cmbSensor2ID.SelectedValue) > 0)
            {
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = "Field1";
                int y = 1;
                MyItem.Value = y.ToString();
                MyItem.Selected = false;
                cmbField1.Items.Add(MyItem);
            }
        }

        protected void Alertsgrid_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Alertsgrid.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        private void LoadGridRow(int RowID)
        {
            txtID.Text = RowID.ToString();
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            DataRow[] MyRows = null;
            DataRow myrow = default(DataRow);
            //MyRow = dt.Rows.Find(RowID)
            MyRows = dt.Select("ID =" + RowID.ToString());
        
            try
            {
                if ((MyRows == null) == false)
                {
                    myrow = MyRows[0];
                    int i = 0;
                    for (i = 0; i <= this.AlertType.Items.Count - 1; i++)
                    {
                        int myint = 1;
                        myint = myint << i;
                        if ((Convert.ToInt32(myrow[0]) & myint) > 0)
                        {
                            this.AlertType.Items[i].Selected = true;
                        }
                        else
                        {
                            this.AlertType.Items[i].Selected = false;
                        }
                    }
                    this.AlertMessage.Text = myrow[1].ToString();
                    //LiveMonitoring.testing test = new LiveMonitoring.testing();
                    //test.GridSelected(this.AlertIncludeImage, myrow);
                    this.AlertIncludeImage.Items[0].Value = Convert.ToString(myrow[2]);
                    for (i = 0; i <= this.AlertCameraImages.Items.Count - 1; i++)
                    {
                        int s = Convert.ToInt32(this.AlertCameraImages.Items[i].Value);
                        if (s == Convert.ToInt32(myrow[3]) | s == Convert.ToInt32(myrow[4]))
                        {
                            this.AlertCameraImages.Items[i].Selected = true;
                        }
                    }
                    this.txtDelay1.Text = (myrow[11]).ToString();
                    this.txtDelay2.Text = (myrow[12]).ToString();
                    try
                    {
                        this.cmbSensor1ID.SelectedValue = (myrow[5]).ToString();
                        LoadField1();
                        this.cmbField1.SelectedValue = (myrow[7]).ToString();
                    }
                    catch (Exception ex1)
                    {
                    }
                    try
                    {
                        this.cmbSensor2ID.SelectedValue = (myrow[6]).ToString();
                        loadfield2();
                        this.cmbField2.SelectedValue = (myrow[8]).ToString();
                    }
                    catch (Exception ex2)
                    {
                    }
                    
                    if (Convert.ToBoolean(myrow[9]))
                    {
                        this.AlertEnabled.Items[0].Selected = true;
                        this.AlertEnabled.Items[1].Selected = false;
                    }
                    else
                    {
                        this.AlertEnabled.Items[1].Selected = true;
                        this.AlertEnabled.Items[0].Selected = false;
                    }
                    if (Convert.ToBoolean(myrow[10]))
                    {
                        this.AlertSendRTN.Items[0].Selected = true;
                        this.AlertSendRTN.Items[1].Selected = false;
                    }
                    else
                    {
                        this.AlertSendRTN.Items[1].Selected = true;
                        this.AlertSendRTN.Items[0].Selected = false;
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void Alertsgrid_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Session["myMaterialReciepeID"] = Alertsgrid.SelectedDataKey.Value;
            LoadGridRow(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));
            LoadContactGrid(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));
            // Get sensors for selected alert.
            getSensors();

            divAlertType.Visible = true;
            divMessages.Visible = true;
            divAddAlertFields.Visible = true;
            divLinkedContacts.Visible = true;
        }

        protected void GridContacts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            dynamic MyRowID = e.Keys[0];
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyRem.LiveMonServer.DeleteAlertContactLink(Convert.ToInt32(e.Keys[0]));
                EditLoadPage();
                LoadContactGrid(Convert.ToInt32(this.txtID.Text));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error deleting link:" + ex.Message;

                Alertsgrid.Focus();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Session["AlertFilterText"] = txtFilterName.Text;
                Session["AlertFilterSelect"] = cmbFilterSelect.SelectedValue;
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

                LoadAlertsGrid(MyRem);

            }
            catch (Exception ex)
            {
            }
        }

        private void getSensors()
        {
            divSensorGrid.Visible = false;

            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                DataSet myDS = new DataSet();
                myDS = (DataSet)MyRem.LiveMonServer.GetSensorsByAlert(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value.ToString()));
                gdvSensors.DataSource = myDS.Tables[0];
                gdvSensors.DataBind();
                divSensorGrid.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
        }

        protected void editAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx");
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Response.Redirect("Index.aspx");
        }

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }
    }
}