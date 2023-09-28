using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Web.Configuration;
using System.Data.SqlClient;

namespace website2016V2
{
    public partial class AddSensorsTemplate : System.Web.UI.Page
    {
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;

        byte[] pstrNoresponse = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser1 = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser1 = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                user.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                LastLogin.Text = " LL:" + MyUser1.LoginDT.ToString();

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;
                gridNewSensors.Visible = true;
                //ok logged on level ?
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];


                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                try
                {
                    LoadDefaultImages();

                }
                catch (Exception ex)
                {
                }
                if (IsPostBack == false)
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.LoadfieldNames(cmbType);
                    test.LoadfieldNames2(cmbSensOutput);

                    LoadDevices();

                }


                
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            

        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridNewSensors.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                //string GroupID = gridSensorGroups.Rows[myRow.RowIndex].Cells[2].Text;
                //string Group = gridSensorGroups.Rows[myRow.RowIndex].Cells[3].Text;
                //string Description = gridSensorGroups.Rows[myRow.RowIndex].Cells[4].Text;



                //ViewState["Id"] = Id;

                //lblID.Visible = true;
                //lblID.Text = GroupID;
                //txtGroup.Text = Group;
                //txtDescription.Text = Description;


                //lblAdd.Text = "Update";
                //btnSave.Text = "Update";

            }

            else if (commandName == "SelectItem")
            {

                //ContactID.Text = Id;

                //Session["myAlertScheduleID"] = gridNewSensors.DataKeys[myRow.RowIndex].Value.ToString();
                //LoadGridRow(Convert.ToInt32(gridNewSensors.DataKeys[myRow.RowIndex].Value));
                //LoadScheduleGrid(Convert.ToInt32(gridNewSensors.DataKeys[myRow.RowIndex].Value));

                //Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value;
                //LoadGridRow(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));
                //LoadScheduleGrid(Convert.ToInt32(Alertsgrid.SelectedDataKey.Value));

                //SampleLogic business = new SampleLogic();

                //int RecordId = Convert.ToInt16(Id);
                //business.Delete(RecordId);

                ////Refresh Grid
                //LoadData();
            }
            if (commandName == "DeleteItem")
            {
                ViewState["Id"] = Id;
                //Dim myrowsel As UltraGridRow = GridContacts.SelectedDataKey.Value ' e.e.Cell.Row
                //delete cmd
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

                if (MyRem.LiveMonServer.DeleteAlertSchedule(Convert.ToInt32(ViewState["Id"])) == false)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Alert schedule not deleted.";
                    gridNewSensors.Focus();
                }
                else
                {
                    //Me.ContactID.Text = ""
                    // errLbl.Visible = False
                    // Session["myAlertScheduleID"] = Alertsgrid.SelectedDataKey.Value
                    //LoadGridRow(CInt(Session["myAlertScheduleID"]))
                    //LoadScheduleGrid(Convert.ToInt32(Session["myAlertScheduleID"]));
                }
            }

        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadPage();
            if (gridNewSensors.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridNewSensors.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridNewSensors.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridNewSensors.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        //public void GridBind1(DataTable dt)
        //{
        //    this.gridNewSensors.DataSource = dt;
        //    gridNewSensors.DataKeyNames = (new string[] { "ID" });
        //    gridNewSensors.DataBind();
        //}
        //private void LoadSensorsGrid()
        //{
        //    Collection MyCollection = new Collection();
        //    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
        //    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
        //    //GetServerObjects 'server1.GetAll()
        //    object MyObject1 = null;
        //    int MyDiv = 1;
        //    bool added = false;
        //    if ((MyCollection == null) == false)
        //    {
        //        foreach (object MyObject1_loopVariable in MyCollection)
        //        {
        //            MyObject1 = MyObject1_loopVariable;
        //            try
        //            {
        //                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorTemplate)
        //                {
        //                    LiveMonitoring.IRemoteLib.SensorTemplate Mysensor = (LiveMonitoring.IRemoteLib.SensorTemplate)MyObject1;

        //                    AddRows((new string[] {
        //                    Mysensor.SensorTemplateID.ToString(),
        //                    Mysensor.Caption,
        //                    Mysensor.Type.ToString(),
        //                    Mysensor.SiteID.ToString(),
        //                    Mysensor.IPDeviceID.ToString(),
        //                    Mysensor.Register.ToString(),
        //                    Mysensor.templateName,
        //                    Mysensor.ScanRate.ToString()


        //                }));
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //        }
        //    }

        //}


        //}

        public void GridUser()
        {
            DataTable dt = new DataTable();
            Session["mytableSensSched"] = dt;
            GridBind(dt);
        }

        public void GridBind(DataTable dt)
        {
            gridNewSensors.DataSource = dt;
            gridNewSensors.DataKeyNames = (new string[] { "SensorTemplateID" });
            gridNewSensors.DataBind();
        }
        public void ClearUserGridRows()
        {
            DataTable dt = new DataTable();
            Session["myContacttable"] = dt;
            GridBind(dt);
        }
        public void LoadPage()
        {

            List<LiveMonitoring.IRemoteLib.SensorTemplate> MyCollectionUsers = new List<LiveMonitoring.IRemoteLib.SensorTemplate>();
           
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyCollectionUsers = MyRem.LiveMonServer.GetAllSensorTemplates();
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Failed to get all users.";
            }

            //  LiveMonitoring.IRemoteLib.UserDetails MyUser = default(LiveMonitoring.IRemoteLib.UserDetails);
            ClearUserGridRows();
            foreach (LiveMonitoring.IRemoteLib.SensorTemplate Mysensor in MyCollectionUsers)
            {

                AddRows((new string[] {
                            Mysensor.SensorTemplateID.ToString(),
                            Mysensor.Caption,
                            Mysensor.Type.ToString(),
                            Mysensor.SiteID.ToString(),
                            Mysensor.IPDeviceID.ToString(),
                            Mysensor.Register.ToString(),
                            Mysensor.templateName,
                            Mysensor.ScanRate.ToString()
                             
                         

                        }));
            }
            //LoadPage()
        }



        public void AddRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytableSensSched"], DataTable)
            //ListFiles()

            if (Session["mytableSens"] == null == false)
            {
                dt = (DataTable)Session["mytableSens"];
                //For Each row1 As DataRow In dt.Rows
                // dt.ImportRow(row1)
                //Next

            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("SensorTemplateID", typeof(string));
                dt.Columns.Add("Caption", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("SiteID", typeof(string));
                dt.Columns.Add("IPDeviceID", typeof(string));
                dt.Columns.Add("Register", typeof(string));
                dt.Columns.Add("templateName", typeof(string));
                dt.Columns.Add("ScanRate", typeof(double));

            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];
            Row[5] = RowVals[5];
            Row[6] = RowVals[6];
            Row[7] = Convert.ToDouble(RowVals[7]);
            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mytableSens"] = dt;
            GridBind(dt);

        }


        public void LoadDevices()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                try
                {
                    //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                    // Dim Mysensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
                    //End If
                    if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                    {
                        LiveMonitoring.IRemoteLib.CameraDetails Mysensor = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        cmbDevice.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.IPDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        cmbDevice.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.OtherDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        cmbDevice.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                    {
                        LiveMonitoring.IRemoteLib.SNMPManagerDetails Mysensor = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        cmbDevice.Items.Add(MyItem);
                    }
                    //cmbSensGroup
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                    {
                        LiveMonitoring.IRemoteLib.SensorGroup MysensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MysensorGroup.SensorGroupName;
                        MyItem.Value = MysensorGroup.SensorGroupID.ToString();
                        MyItem.Selected = false;
                        cmbSensGroup.Items.Add(MyItem);
                    }

                }
                catch (Exception ex)
                {
                }

            }
            try
            {
                SortDropDown(cmbSensGroup);

            }
            catch (Exception ex)
            {
            }
            try
            {
                SortDropDown(cmbDevice);

            }
            catch (Exception ex)
            {
            }
            try
            {
                // SortDropDown(cmbSensors)

            }
            catch (Exception ex)
            {
            }
        }
        private void SortDropDown(DropDownList dd)
        {
            try
            {
                ListItem[] ar = null;
               int i = 0;
                foreach (ListItem li in dd.Items)
                {
                    Array.Resize(ref ar, i + 1);
                    ar[i] = li;
                    i += 1;
                }
                Array ar1 = ar;

                //ar1.Sort(ar1, new ListItemComparer());
                dd.Items.Clear();
                dd.Items.AddRange(ar);

            }
            catch (Exception ex)
            {
            }

        }

        private class ListItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListItem a =(System.Web.UI.WebControls.ListItem) x;
                ListItem b = (System.Web.UI.WebControls.ListItem) y;
                CaseInsensitiveComparer c = new CaseInsensitiveComparer();
                return c.Compare(a.Text, b.Text);
            }
        }


        protected void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            // Insert a new sensor template.
            // sp: [SENSORTEMPLATE].spInsert


            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();

            switch (Convert.ToInt32(this.cmbType.SelectedValue))
            {
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BaseAudio:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraAudio:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDInput:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Output number!";
                        return;
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Serial number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Serial number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    if (string.IsNullOrEmpty(this.txtSerialNumber1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Serial number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    if (string.IsNullOrEmpty(this.txtSerialNumber1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Serial number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    if (string.IsNullOrEmpty(this.txtSerialNumber1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Serial number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Min Value!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Max Value!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    if (string.IsNullOrEmpty(this.txtSerialNumber1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Serial number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                    //check if device is snmp device

                    //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                    Collection MyCollection = default(Collection);
                    MyCollection = MyRem.get_GetServerObjects();
                    //server1.GetAll()
                    object MyObject1 = null;
                    //LiveMonitoring.IRemoteLib.SNMPManagerDetails
                    bool IsSnmp = false;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            if (string.IsNullOrEmpty(this.cmbDevice.SelectedValue))
                            {
                                IsSnmp = true;
                            }

                        }
                    }

                    if (IsSnmp == false)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please Select a SNMP device!";
                        return;

                    }
                    break;
                // If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.CameraDetails Then
                // Dim MyCamera As LiveMonitoring.IRemoteLib.CameraDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.CameraDetails)
                // MyCameraCollection.Add(MyCamera)
                // End If
                // If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then

                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Min Value!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Max Value!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Module number!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter Register number!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    if (string.IsNullOrEmpty(this.txtExtraData.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter WMI Namespace!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData1.Text))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter WMI Property!";
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                    if (string.IsNullOrEmpty(this.cmbModels.SelectedValue))
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please Select a valid model!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text) | Information.IsNumeric(this.txtExtraValue.Text) == false)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter a valid Bus no .!";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue1.Text) | Information.IsNumeric(this.txtExtraValue1.Text) == false)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Please enter a valid Module no .!";
                        return;
                    }
                    break;
            }
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please supply a Caption!";
                return;
            }
            if (string.IsNullOrEmpty(this.cmbDevice.SelectedValue))
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select IP Device!";
                return;
            }
            LiveMonitoring.IRemoteLib.SensorTemplate NewSensor = new LiveMonitoring.IRemoteLib.SensorTemplate();
            NewSensor.templateName = this.txtTemplateName.Text;
            NewSensor.Type =(LiveMonitoring.IRemoteLib.SensorTemplate.SensorType) Convert.ToInt32(this.cmbType.SelectedValue);
            NewSensor.IPDeviceID = Convert.ToInt32(this.cmbDevice.SelectedValue);
            NewSensor.ModuleNo = Convert.ToInt32(this.txtModule.Text);
            NewSensor.Register = Convert.ToInt32(this.txtRegister.Text);
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum =(LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.cmbType.SelectedValue);

            if (MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint)
            {
                NewSensor.SerialNumber = this.txtSerialNumber.Text;
            }
            else
            {
                NewSensor.SerialNumber = this.txtSerialNumber1.Text.Trim();
            }
            byte[] byteNormal = null;
            byte[] byteError = null;
            byte[] byteNoresponse = null;

            System.Drawing.Image imgNormal = null;
            System.Drawing.Image imgError = null;
            System.Drawing.Image imgNoresponse = null;

            //NORMAL IMAGE
            if (filImageNormal.FileName.Trim().Length == 0)
            {
                byteNormal = pstrNormal;
            }
            else
            {
                imgNormal = Myfunc.Strip_Image(this.filImageNormal);
                byteNormal = MyRem.ImagetoByte(imgNormal, ImageFormat.Bmp);
            }

            //NO RESPONSE IMAGE
            if (filImageNoResponse.FileName.Trim().Length == 0)
            {
                byteNoresponse = pstrNoresponse;
            }
            else
            {
                imgNoresponse = Myfunc.Strip_Image(this.filImageNoResponse);
                byteNoresponse = MyRem.ImagetoByte(imgNoresponse, ImageFormat.Bmp);
            }

            //ERROR MAGE
            if (filImageError.FileName.Trim().Length == 0)
            {
                byteError = pstrError;
            }
            else
            {
                imgError = Myfunc.Strip_Image(this.filImageError);
                byteError = MyRem.ImagetoByte(imgError, ImageFormat.Bmp);
            }
            NewSensor.ImageNormal = imgNormal;
            NewSensor.ImageNormalByte = byteNormal;
            NewSensor.ImageNoResponse = imgNoresponse;
            NewSensor.ImageNoResponseByte = byteNoresponse;
            NewSensor.ImageError = imgError;
            NewSensor.ImageErrorByte = byteError;
            NewSensor.Caption = this.txtCaption.Text;
            NewSensor.MinValue = Convert.ToDouble(txtZeroValue.Text);
            NewSensor.MaxValue = Convert.ToDouble(this.txtMaxValue.Text);
            NewSensor.Multiplier = Convert.ToDouble(this.txtMultiplier.Text);
            NewSensor.Divisor = Convert.ToDouble(this.txtDivisor.Text);
            NewSensor.ScanRate = Convert.ToDouble(this.txtScanRate.Text);
            NewSensor.ExtraData = (this.txtExtraData.Text);
            NewSensor.ExtraData1 = (this.txtExtraData1.Text);
            NewSensor.ExtraData2 = (this.txtExtraData2.Text);
            NewSensor.ExtraData3 = (this.txtExtraData3.Text);
            NewSensor.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
            NewSensor.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
            NewSensor.ScanRate = Convert.ToDouble(this.txtScanRate.Text);
            try
            {
                NewSensor.SensGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
                NewSensor.SensGroup.SensorGroupID =Convert.ToInt32(cmbSensGroup.SelectedValue);
                NewSensor.SensGroup.SensorGroupName = cmbSensGroup.Items[cmbSensGroup.SelectedIndex].Text.Trim();

            }
            catch (Exception ex)
            {
            }

            if (cmbSensOutput.Visible)
            {
                try
                {
                    NewSensor.OutputType =(LiveMonitoring.IRemoteLib.SensorTemplate.OutputTypeDef)Convert.ToInt32(cmbSensOutput.SelectedValue);

                }
                catch (Exception ex)
                {
                }
            }


            try
            {
                MyRem.LiveMonServer.AddNewSensorTemplate(NewSensor);
                successMessage.Visible = true;
                lblSuccess.Text = "Template Saved Successfully";
                txtCaption.Text = "";
                txtDivisor.Text = "";
                txtExtraData.Text = "";
                txtExtraData1.Text = "";
                txtExtraData2.Text = "";
                txtExtraData3.Text = "";
                txtExtraValue.Text = "";
                txtExtraValue1.Text = "";
                txtModule.Text = "";
                txtMultiplier.Text = "";
                txtRegister.Text = "";
                txtScanRate.Text = "";
                txtSerialNumber.Text = "";
                txtSerialNumber1.Text = "";
                txtTemplateName.Text = "";
                txtZeroValue.Text = "";
               
            }
            catch (Exception ex)
            {
                lblErr.Visible = true;
                lblErr.Text = "An error has occured during save, Please try again! ERROR MESSAGE: " + ex.Message;

            }



            //lblErr.Visible = True
            //lblErr.Text = "An error has occured during save, Please try again!"


        }


        public void LoadDefaultImages()
        {
            try
            {
                string sqlQuery = "select * from SensorDefaultImages";
                SqlConnection connection = new SqlConnection(conStr);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlQuery;
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pstrNormal = (byte[])reader["ImageNormal"];
                    pstrError = (byte[])reader["ImageError"];
                    pstrNoresponse = (byte[])reader["ImageNoResponse"];

                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                //ASSIGN DEFAULT IMAGES
                string base64StringpstrNormal = Convert.ToBase64String(pstrNormal, 0, pstrNormal.Length);
                imgNormal.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNormal;

                string base64StringpstrError = Convert.ToBase64String(pstrError, 0, pstrError.Length);
                imgError.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrError;

                string base64StringpstrNoresponse = Convert.ToBase64String(pstrNoresponse, 0, pstrNoresponse.Length);
                imgResponse.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNoresponse;

            }
            catch (Exception ex)
            {
            }



        }
        public void Sensors_SensorTemplateAdd()
        {
            Load += Page_Load;
        }

        protected void btnAddNewsensorsTemplate_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClearNewSensorTemplate_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClearNewSensor_Click(object sender, EventArgs e)
        {
            txtCaption.Text = "";
            txtDivisor.Text = "";
            txtExtraData.Text = "";
            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraData3.Text = "";
            txtExtraValue.Text = "";
            txtExtraValue1.Text = "";
            txtModule.Text = "";
            txtMultiplier.Text = "";
            txtRegister.Text = "";
            txtScanRate.Text = "";
            txtSerialNumber.Text = "";
            txtSerialNumber1.Text = "";
            txtTemplateName.Text = "";
            txtZeroValue.Text = "";
        }
    }
}