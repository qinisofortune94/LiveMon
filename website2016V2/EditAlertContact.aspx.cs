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
    public partial class EditAlertContact : System.Web.UI.Page
    {
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
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
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                LiveMonitoring.DataManager datamanager = new LiveMonitoring.DataManager();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    int reqAlertID = 0;
                    reqAlertID = Convert.ToInt32(Request.QueryString["AlertID"]);
                    this.AlertID.Text = reqAlertID.ToString();
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.AlertContactType(AlertContactType, drpEmployee);
                    ////Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
                    ////Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
                    ////string x = null;
                    ////int MyVal = 0;
                    ////this.AlertContactType.Items.Clear();
                    ////datamanager.LoadEmployees(drpEmployee, null);
                    ////foreach (string x_loopVariable in Item)
                    ////{
                    ////    x = x_loopVariable;
                    ////    Web.UI.WebControls.ListItem MyItem = new Web.UI.WebControls.ListItem();
                    ////    MyItem.Text = x;
                    ////    MyItem.Value = ItemValue(MyVal);
                    ////    MyItem.Selected = false;
                    ////    this.AlertContactType.Items.Add(MyItem);
                    ////    MyVal += 1;
                    ////}
                    //AlertCameraImages
                    Collection MyCollection = new Collection();
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    MyCollection = MyRem.get_GetServerObjects();
                    //server1.GetAll()
                    object MyObject1 = null;
                    this.AlertContactOutput.Items.Clear();

                    if ((MyCollection == null))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Sensor details not found.";

                        GridContacts.Focus();

                        return;
                    }

                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                //only outputs
                                if (MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusForceMultipleCoils | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusreadWriteRegisters | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbuswriteMultipleRegisters | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbuswriteSingleRegister | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUForceMultipleCoils | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUreadWriteRegisters | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUwriteMultipleRegisters | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUwriteSingleRegister | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUDOutput)
                                {
                                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                    MyItem.Text = MySensor.Caption;
                                    MyItem.Value = MySensor.ID.ToString();
                                    MyItem.Selected = false;
                                    this.AlertContactOutput.Items.Add(MyItem);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    LoadGrid();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void CmdSend_Click(object sender, EventArgs e)
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

            if (string.IsNullOrEmpty(txtID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select the Contact to edit.";

                GridContacts.Focus();

                return;
            }
            LiveMonitoring.IRemoteLib.AlertContactDef MyAlert = new LiveMonitoring.IRemoteLib.AlertContactDef();
            MyAlert.ID = Convert.ToInt32(this.AlertID.Text);
            int i = 0;
            
            for (i = 0; i <= this.AlertContactType.Items.Count - 1; i++)
            {
                if (this.AlertContactType.Items[i].Selected)
                {
                    MyAlert.Type = MyAlert.Type | Convert.ToInt32(AlertContactType.Items[i].Value);
                }
            }
            int aCnt = 0;
            for (i = 0; i <= this.AlertContactOutput.Items.Count - 1; i++)
            {
                if (this.AlertContactOutput.Items[i].Selected)
                {
                    if (aCnt == 0)
                    {
                        MyAlert.OutputParam4 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 1)
                    {
                        MyAlert.OutputParam5 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 2)
                    {
                        MyAlert.OutputParam6 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 3)
                    {
                        MyAlert.OutputParam7 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    aCnt += 1;
                    if (aCnt > 3)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            MyAlert.ContactName = this.AlertContactName.Text;
            MyAlert.OutputParam = this.AlertContactEmail.Text;
            MyAlert.OutputParam1 = this.AlertContactCell.Text;
            MyAlert.OutputParam2 = this.AlertContactIM.Text;
            MyAlert.OutputParam3 = this.AlertContactOther.Text;
            MyAlert.ResendDelay = Convert.ToInt32(this.AlertResendDelay.Text);
            MyAlert.SingleSend = this.chkSingleSend.Checked;
            MyAlert.ID = Convert.ToInt32(this.txtID.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            bool MyContactID = MyRem.LiveMonServer.EditAlertContact(MyAlert);

            if (MyContactID == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not updated Error.";

                GridContacts.Focus();
            }
            else
            {
                //LblErr.Visible = false;
                this.AlertContactEmail.Text = "";
                this.AlertContactCell.Text = "";
                this.AlertContactIM.Text = "";
                this.AlertContactOther.Text = "";
                this.txtID.Text = "";
                successMessage.Visible = true;
                lblSuccess.Text = "Successfully Saved";
                //Response.Redirect("editalertcontactSchedule.aspx?AlertID=" + Me.AlertID.Text + "&ContactID=" + MyContactID.ToString)
            }
        }

        protected void cmdFinnished_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx?AlertID=" + this.AlertID.Text);
        }

        public void LoadGrid()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollectionAlerts = MyRem.LiveMonServer.GetAllContacts();
            //LiveMonitoring.IRemoteLib.AlertContactDef MyAlert = default(LiveMonitoring.IRemoteLib.AlertContactDef);
            ClearContactRows();
            if ((MyCollectionAlerts == null) == true)
            {
                return;
            }
            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyAlert in MyCollectionAlerts)
            {
                try
                {
                    string OutParam = "";
                    string OutParam1 = "";
                    string OutParam2 = "";
                    string OutParam3 = "";
                    try
                    {
                        OutParam = ((MyAlert.OutputParam == null) ? "" : MyAlert.OutputParam.ToString());

                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        OutParam1 = ((MyAlert.OutputParam1 == null) ? "" : MyAlert.OutputParam1.ToString());

                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        OutParam2 = ((MyAlert.OutputParam2 == null) ? "" : MyAlert.OutputParam2.ToString());

                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        OutParam3 = ((MyAlert.OutputParam3 == null) ? "" : MyAlert.OutputParam3.ToString());

                    }
                    catch (Exception ex)
                    {
                    }


                    AddContactRows((new string[] {
                        MyAlert.ContactName,
                        MyAlert.Type.ToString(),
                        OutParam,
                        OutParam1,
                        OutParam2,
                        OutParam3,
                        MyAlert.OutputParam4.ToString() + "," + MyAlert.OutputParam5.ToString() + "," + MyAlert.OutputParam6.ToString() + "," + MyAlert.OutputParam7.ToString(),
                        MyAlert.ResendDelay.ToString(),
                        MyAlert.ID.ToString(),
                        MyAlert.PeopleId.ToString()
                    }));
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void AddContactRows(string[] RowVals)
        {
            DataRow Row = null;
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
                dt.Columns.Add("PeopleId", typeof(int));
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
            Row[9] = RowVals[9];

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

        protected void cmdEditSchedule_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select a contact to schedule.";

                GridContacts.Focus();
                return;
            }
            Response.Redirect("EditAlertContactSchedule.aspx?AlertID=" + this.AlertID.Text.ToString() + "&ContactID=" + txtID.Text);
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            this.AlertContactEmail.Text = "";
            this.AlertContactCell.Text = "";
            this.AlertContactIM.Text = "";
            this.AlertContactOther.Text = "";
            this.txtID.Text = "";
            //Me.AlertID.Text = ""
            //LblErr.Visible = false;
            LoadGrid();
        }

        protected void cmdSaveNew_Click(object sender, EventArgs e)
        {
            LiveMonitoring.IRemoteLib.AlertContactDef MyAlert = new LiveMonitoring.IRemoteLib.AlertContactDef();
            MyAlert.ID = Convert.ToInt32(this.AlertID.Text);
            int i = 0;
            for (i = 0; i <= this.AlertContactType.Items.Count - 1; i++)
            {
                if (this.AlertContactType.Items[i].Selected)
                {
                    MyAlert.Type = MyAlert.Type | Convert.ToInt32(AlertContactType.Items[i].Value);
                }
            }
            if (drpEmployee.SelectedItem == null & ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Email) > 0 | (MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.SMS) > 0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter select employee.";
                GridContacts.Focus();

                drpEmployee.Focus();
            }
            int aCnt = 0;
            for (i = 0; i <= this.AlertContactOutput.Items.Count - 1; i++)
            {
                if (this.AlertContactOutput.Items[i].Selected)
                {
                    if (aCnt == 0)
                    {
                        MyAlert.OutputParam4 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 1)
                    {
                        MyAlert.OutputParam5 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 2)
                    {
                        MyAlert.OutputParam6 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 3)
                    {
                        MyAlert.OutputParam7 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    aCnt += 1;
                    if (aCnt > 3)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

            MyAlert.OutputParam = this.AlertContactEmail.Text;
            MyAlert.OutputParam1 = this.AlertContactCell.Text;
            MyAlert.OutputParam2 = this.AlertContactIM.Text;
            MyAlert.OutputParam3 = this.AlertContactOther.Text;
            MyAlert.ResendDelay = Convert.ToInt32(AlertResendDelay.Text);
            MyAlert.ContactName = this.AlertContactName.Text;
            MyAlert.SingleSend = this.chkSingleSend.Checked;
            MyAlert.PeopleId = Convert.ToInt32(drpEmployee.SelectedValue);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            int MyContactID = MyRem.LiveMonServer.AddNewAlertContact(MyAlert);

            if (MyContactID < 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error.";
                GridContacts.Focus();
            }
            else
            {
                this.AlertContactEmail.Text = "";
                this.AlertContactCell.Text = "";
                this.AlertContactIM.Text = "";
                this.AlertContactOther.Text = "";
                drpEmployee.SelectedIndex = -1;
                //Me.AlertID.Text = ""
                //LblErr.Visible = false;
                Response.Redirect("AlertSchedule.aspx?AlertID=" + this.AlertID.Text + "&ContactID=");
            }
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
                warningMessage.Visible = true;
                lblWarning.Text = "Please select a Contact to delete first.";
                GridContacts.Focus();

                return;
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs

            MyRem.LiveMonServer.DeleteAlertContact(Convert.ToInt32(this.txtID.Text));
            LoadGrid();
        }

        protected void GridContacts_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridContacts.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["myContacttable"];
            GridContactBind(dt);
        }

        protected void GridContacts_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Session["myContacttableKey"] = GridContacts.SelectedDataKey.Value;
            LoadGridRow(Convert.ToInt32(GridContacts.SelectedDataKey.Value));
        }

        private void LoadGridRow(int RowID)
        {
            txtID.Text = RowID.ToString();
            DataTable dt = new DataTable();
            dt = (DataTable)Session["myContacttable"];
            DataRow[] MyRows = null;
            DataRow myrow = null;
            MyRows = dt.Select("ID =" + RowID.ToString());
            
            try
            {
                if ((MyRows == null) == false)
                {
                    myrow = MyRows[0];


                    this.AlertContactName.Text = (string)myrow[0];
                    int i = 0;
                    for (i = 0; i <= this.AlertContactType.Items.Count - 1; i++)
                    {
                        if ((Convert.ToInt32(myrow[1]) & (1 << i)) > 0)
                        {
                            this.AlertContactType.Items[i].Selected = true;
                        }
                    }
                    int aCnt = 0;
                    string[] MyVals = myrow[6].ToString().Split(',');
                    for (i = 0; i <= this.AlertContactOutput.Items.Count - 1; i++)
                    {
                        int MyCnt = 0;
                        for (MyCnt = 0; MyCnt <= MyVals.Length - 1; MyCnt++)
                        {
                            if (Convert.ToInt32(this.AlertContactOutput.Items[i].Value) == Convert.ToInt32(MyVals[MyCnt]))
                            {
                                this.AlertContactOutput.Items[i].Selected = true;
                            }
                        }
                    }

                    this.AlertContactEmail.Text = (string)myrow[2];
                    this.AlertContactCell.Text = (string)myrow[3];
                    this.AlertContactIM.Text = (string)myrow[4];
                    this.AlertContactOther.Text = (string)myrow[5];
                    try
                    {
                        this.AlertResendDelay.Text = (string)myrow[7];
                        drpEmployee.SelectedItem.Value = (string)myrow[9];

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

        protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.DataManager datamanager = new LiveMonitoring.DataManager();
                DataRow dr = datamanager.GetPeopleDetails(Convert.ToInt32(drpEmployee.SelectedValue));
                AlertContactName.Text = (string)dr["FullNames"];
                AlertContactCell.Text = (string)dr["Cell"];
                AlertContactEmail.Text = (string)dr["Email"];
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        public EditAlertContact()
        {
            Load += Page_Load;
        }
    }
}