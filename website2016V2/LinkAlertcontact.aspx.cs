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
    public partial class LinkAlertcontact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                LoadPage(Convert.ToInt32(Request.QueryString["AlertID"]));
            }
        }

        public void LoadPage(int AlertID)
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

                this.txtID.Value = AlertID.ToString();
                //ok logged on level ?
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);

                //AlertCameraImages
                Collection MyCollection = new Collection();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                Collection MyCollectionAlerts = new Collection();
                MyCollectionAlerts = MyRem.LiveMonServer.GetSpecificAlerts(AlertID);

                ClearRows();
                foreach (LiveMonitoring.IRemoteLib.AlertDetails MyAlert in MyCollectionAlerts)
                {
                    //Alertsgrid.Rows.Add()
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
                LoadallContacts();
                LoadContactGrid(AlertID);
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

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
            bool firstRow = true;
            foreach (LiveMonitoring.IRemoteLib.AlertContactDef MyContact in MyCollectionAlerts)
            {
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = MyContact.ContactName;
                MyItem.Value = MyContact.ID.ToString();
                cmbContacts.Items.Add(MyItem);
                if (firstRow)
                {
                    MyItem.Selected = true;
                    firstRow = false;
                    txtContactID.Value = MyContact.ID.ToString();
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
        }

        protected void cmbContacts_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            txtContactID.Value = cmbContacts.SelectedValue;
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
            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mytable"] = dt;
            GridBind(dt);

        }

        protected void btnLinkContacts_Click(object sender, EventArgs e)
        {
            if (cmbContacts.SelectedIndex != -1 & !string.IsNullOrEmpty(this.txtID.Value))
            {
                try
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    MyRem.LiveMonServer.AddNewAlertContactLink(Convert.ToInt32(this.txtID.Value), Convert.ToInt32(txtContactID.Value));
                    LoadPage(Convert.ToInt32(this.txtID.Value));
                }
                catch (Exception ex)
                {
                    //if err
                    errorMessageLink.Visible = true;
                    lblErr.Visible = true;
                    lblErr.Text = "Error adding link:" + ex.Message;
                }
            }
            else
            {
                lblErr.Visible = true;
                if (cmbContacts.SelectedIndex != -1)
                {
                    lblErr.Text = "Error Please select contact !";
                }
                else
                {
                    lblErr.Text = "Error Please select alert !";
                }
            }
        }

        protected void btnSetThreashhold_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddAlertThreshHolds.aspx?AlertID=" + this.txtID.Value);
        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
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