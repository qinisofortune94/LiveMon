using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class AlertContact : System.Web.UI.Page
    {
        int MyContactID = 0;

        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
        protected void Page_Load(object sender, System.EventArgs e)
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
                //MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
            

                if (IsPostBack == false)
                {
                    int reqAlertID = 0;
                    reqAlertID = Convert.ToInt32(Request.QueryString["AlertID"]);
                    LiveMonitoring.DataManager datamanager = new LiveMonitoring.DataManager();
                    txtAlertID.Text = reqAlertID.ToString();

                    Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
                    //Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.AlertDetails.AlertsType));
                    //string x = null;
                    //int MyVal = 0;
                    //this.AlertContactType.Items.Clear();
                    // datamanager.LoadEmployees(ddlPerson, null);
                    //foreach (string x_loopVariable in Item)
                    //{
                    //    x = x_loopVariable;
                    //    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    //    MyItem.Text = x;
                    //    MyItem.Value = ItemValue(MyVal);
                    //    MyItem.Selected = false;
                    //    this.AlertContactType.Items.Add(MyItem);
                    //    MyVal += 1;
                    //}
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.AlertContactType(AlertContactType, ddlPerson);

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


        protected void AddPerson_Click1(object sender, EventArgs e)
        {
            Response.Redirect("addPeople.aspx");
        }



        protected void btnSend_Click1(object sender, EventArgs e)
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
            LiveMonitoring.IRemoteLib.AlertContactDef MyAlert = new LiveMonitoring.IRemoteLib.AlertContactDef();
            MyAlert.ID =Convert.ToInt32(this.txtAlertID.Text);
            int i = 0;
            for (i = 0; i <= this.AlertContactType.Items.Count - 1; i++)
            {
                if (this.AlertContactType.Items[i].Selected)
                {
                    MyAlert.Type = MyAlert.Type | Convert.ToInt32(AlertContactType.Items[i].Value);
                }
            }

           

            if (ddlPerson.SelectedItem == null & ((MyAlert.Type &(int) LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Email) > 0 | (MyAlert.Type &(int) LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.SMS) > 0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter select employee.";

               ddlPerson.Focus();
            }
            if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Email) > 0)
            {

                if (string.IsNullOrEmpty(this.txtEmail.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please enter Email Adress.";
                    return;
                }
            }
            if ((MyAlert.Type &(int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.SMS) > 0)
            {
                if (string.IsNullOrEmpty(txtCellNumber.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please enter Cell Number.";
                    return;
                }
            }

            int aCnt = 0;
            for (i = 0; i <= this.AlertContactOutput.Items.Count - 1; i++)
            {
                if (this.AlertContactOutput.Items[i].Selected)
                {
                    if (aCnt == 0)
                    {
                        MyAlert.OutputParam4 =Convert.ToDouble (this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 1)
                    {
                        MyAlert.OutputParam5 = Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
                    }
                    if (aCnt == 2)
                    {
                        MyAlert.OutputParam6 =Convert.ToDouble(this.AlertContactOutput.Items[i].Value);
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
            if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Output) > 0)
            {
                if ((MyAlert.OutputParam4) == 0)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please select an Output.";
                    return;
                }
            }
            //if ((MyAlert.Type &(int) LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMGoogleTalk) > 0)
            //{
            //    if (string.IsNullOrEmpty((txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter a Google Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMJabbber) > 0)
            //{
            //    if (string.IsNullOrEmpty((this.txtCellNumber.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an Jabber Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type &(int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMMSNLiveMessenger) > 0)
            //{
            //    if (string.IsNullOrEmpty((txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an MSN Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type &(int) LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMSkypeCall) > 0)
            //{
            //    if (string.IsNullOrEmpty((this.txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an Skype Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type &(int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMSkypeMessage) > 0)
            //{
            //    if (string.IsNullOrEmpty((this.txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an Skype Contact.";
            //        return;
            //    }
            //}
            MyAlert.OutputParam = txtEmail.Text;
            MyAlert.OutputParam1 = txtCellNumber.Text;
            MyAlert.OutputParam2 = txtIMName.Text;
            MyAlert.OutputParam3 = txtOther.Text;
            MyAlert.ResendDelay = Convert.ToInt32(this.txtResend.Value);
            MyAlert.ContactName = txtName.Text;
            MyAlert.SingleSend =chksingle.Checked;
            MyAlert.PeopleId = Convert.ToInt32(ddlPerson.SelectedValue);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //MyRem.server1.
            MyContactID = MyRem.LiveMonServer.AddNewAlertContact(MyAlert);

            if (MyContactID < 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error.";
            }
            else
            {
                txtEmail.Text = "";
                txtCellNumber.Text = "";
                txtIMName.Text = "";
                txtOther.Text = "";
                txtName.Text = "";
                successMessage.Visible = true;
                lblSucces.Text = "Successfully saved";
                btnSchedule.Visible = true;
                Response.Redirect("AlertSchedule.aspx?AlertID=" + this.txtAlertID.Text + "&ContactID=" + MyContactID.ToString());
            }
        }

        public void btnSchedule_Click(object sender, EventArgs e)
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
            LiveMonitoring.IRemoteLib.AlertContactDef MyAlert = new LiveMonitoring.IRemoteLib.AlertContactDef();
            MyAlert.ID = Convert.ToInt32(this.txtAlertID.Text);
            int i = 0;
            for (i = 0; i <= this.AlertContactType.Items.Count - 1; i++)
            {
                if (this.AlertContactType.Items[i].Selected)
                {
                    MyAlert.Type = MyAlert.Type | Convert.ToInt32(AlertContactType.Items[i].Value);
                }
            }



            if (ddlPerson.SelectedItem == null & ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Email) > 0 | (MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.SMS) > 0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter select employee.";

                ddlPerson.Focus();
            }
            if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Email) > 0)
            {

                if (string.IsNullOrEmpty(this.txtEmail.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please enter Email Adress.";
                    return;
                }
            }
            if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.SMS) > 0)
            {
                if (string.IsNullOrEmpty(txtCellNumber.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please enter Cell Number.";
                    return;
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
            if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.Output) > 0)
            {
                if ((MyAlert.OutputParam4) == 0)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please select an Output.";
                    return;
                }
            }
            //if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMGoogleTalk) > 0)
            //{
            //    if (string.IsNullOrEmpty((txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter a Google Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMJabbber) > 0)
            //{
            //    if (string.IsNullOrEmpty((this.txtCellNumber.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an Jabber Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMMSNLiveMessenger) > 0)
            //{
            //    if (string.IsNullOrEmpty((txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an MSN Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMSkypeCall) > 0)
            //{
            //    if (string.IsNullOrEmpty((this.txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an Skype Contact.";
            //        return;
            //    }
            //}
            //if ((MyAlert.Type & (int)LiveMonitoring.IRemoteLib.AlertDetails.AlertsType.IMSkypeMessage) > 0)
            //{
            //    if (string.IsNullOrEmpty((this.txtIMName.Text).Trim()))
            //    {
            //        warningMessage.Visible = true;
            //        lblWarning.Text = "Please enter an Skype Contact.";
            //        return;
            //    }
            //}
            MyAlert.OutputParam = txtEmail.Text;
            MyAlert.OutputParam1 = txtCellNumber.Text;
            MyAlert.OutputParam2 = txtIMName.Text;
            MyAlert.OutputParam3 = txtOther.Text;
            MyAlert.ResendDelay = Convert.ToInt32(this.txtResend.Value);
            MyAlert.ContactName = txtName.Text;
            MyAlert.SingleSend = chksingle.Checked;
            MyAlert.PeopleId = Convert.ToInt32(ddlPerson.SelectedValue);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //MyRem.server1.
            MyContactID = MyRem.LiveMonServer.AddNewAlertContact(MyAlert);

            if (MyContactID < 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Not saved Error.";
            }
            else
            {
                txtEmail.Text = "";
                txtCellNumber.Text = "";
                txtIMName.Text = "";
                txtOther.Text = "";
                txtName.Text = "";
                successMessage.Visible = true;
                lblSucces.Text = "Successfully saved";
                
                //Response.Redirect("AlertSchedule.aspx?AlertID=" + this.txtAlertID.Text + "&ContactID=" + MyContactID.ToString());
            }
            
        }


        public void AddContactRows(string[] RowVals)
        {
            System.Data.DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytable"], DataTable)
            //ListFiles()

            if (Session["myContacttable"] == null == false)
            {
                dt = (DataTable)Session["myContacttable"];
                //For Each row1 As DataRow In dt.Rows
                //    dt.ImportRow(row1)
                //Next

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
            Row[2]= RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];
            Row[5]= RowVals[5];
            Row[6]= RowVals[6];
            Row[7]= RowVals[7];
            Row[8]= RowVals[8];

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

        protected void btnThreshold_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddAlertThreshHolds.aspx?AlertID=" + this.txtAlertID.ToString());
            //Response.Redirect("addalertthreashholds.aspx?AlertID=" + Me.AlertID.Text)
            //AddAlertThreshHolds.aspx
        }

        public void LoadGrid()
        {
            Collection MyCollectionAlerts = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            if (!string.IsNullOrEmpty(this.txtAlertID.Text))
            {
                MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlertsContacts(Convert.ToInt32(this.txtAlertID.Text));
            }
            else
            {
                MyCollectionAlerts = MyRem.LiveMonServer.GetAllContacts();
                //CInt(Me.AlertID.Text))
            }


            //LiveMonitoring.IRemoteLib.AlertContactDef MyAlert = default(LiveMonitoring.IRemoteLib.AlertContactDef);
            // GridContactsOld.Clear()
            ClearContactRows();
            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyAlert in MyCollectionAlerts)
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
                //Dim myrow As New UltraGridRow(True)
                //myrow.Cells.Add()
                //myrow.Cells(0).Value = MyAlert.ContactName
                //myrow.Cells.Add()
                //myrow.Cells(1).Value = MyAlert.Type
                //myrow.Cells.Add()
                //myrow.Cells(2).Value = MyAlert.OutputParam
                //myrow.Cells.Add()
                //myrow.Cells(3).Value = MyAlert.OutputParam1
                //myrow.Cells.Add()
                //myrow.Cells(4).Value = MyAlert.OutputParam2
                //myrow.Cells.Add()
                //myrow.Cells(5).Value = MyAlert.OutputParam3
                //myrow.Cells.Add()
                //myrow.Cells(6).Value = MyAlert.OutputParam4
                //GridContactsOld.Rows.Add(myrow)
            }
        }



        protected void ddlPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.DataManager datamanager = new LiveMonitoring.DataManager();
                DataRow dr = datamanager.GetPeopleDetails(ddlPerson.SelectedIndex);
                if (Information.IsDBNull(dr["FullNames"]) == false)
                    txtName.Text = (string)dr["FullNames"];
                if (Information.IsDBNull(dr["Cell"]) == false)
                    txtCellNumber.Text =(string) dr["Cell"];
                if (Information.IsDBNull(dr["Email"]) == false)
                    txtEmail.Text =(string) dr["Email"];

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        //protected void btnSend_Click1(object sender, EventArgs e)
        //{

        //}
    }
}