using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using LiveMonitoring;
using System.Drawing;
using System.Drawing.Imaging;
//using Infragistics.WebUI.UltraWebGrid;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;

namespace website2016V2
{
    public partial class AddNewSensor : System.Web.UI.Page
    {

        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        byte[] pstrNormal = null;
        byte[] pstrError = null;
        byte[] pstrNoresponse = null;
        private Collection MySensorCollection = new Collection();

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
                //lblSucces.Visible = false;
                //lblwarning.Visible = false;
                //lblError.Visible = false;
                gridNewSensors.Visible = true;
                LoadSensorsGrid();
                // GridBind1(dt);


                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                //MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");

                }
                try
                {
                    LoadDefaultImages();

                }
                catch
                {
                }
                if (IsPostBack == false)
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.LoadValues(cmbSensOutput);
                    test.LoadfieldNames(ddlSensorType);
                    // test.LoadfieldNames2(ddlDevice);
                    //test.LoadfieldNames2(cmbSensOutput);

                    LoadSites();
                    LoadDevices();
                    LoadModels();
                    LoadAlertGroups();
                    LoadDefaultImages();
                    cmbFields.Visible = true;
                    ClearRows();
                    AddRow((new string[] {
                            "Temp",
                            "deg C",
                            "1",
                            "true"
                        }));
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            //ok logged on level ?


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
            //LoadPeople();
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

        public void AddRow(string[] RowVals)
        {
            try
            {
                DataRow Row = default(DataRow);
                DataTable dt = new DataTable();
                //= CType(Session["mytable"], DataTable)
                //ListFiles()

                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                    //For Each row1 As DataRow In dt.Rows
                    // dt.ImportRow(row1)
                    //Next

                }
                //"InputStatus", "", "1", "true"
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field Name", typeof(string));
                    dt.Columns.Add("Field Suffix", typeof(string));
                    dt.Columns.Add("Field", typeof(int));
                    dt.Columns.Add("Display Field", typeof(bool));
                    dt.Columns.Add("Field Max Val", typeof(double));
                    dt.Columns.Add("Field Min Val", typeof(double));
                    dt.Columns.Add("Field Notes", typeof(string));
                    dt.Columns.Add("Field Max Warn Val", typeof(double));
                    dt.Columns.Add("Field Min Warn Val", typeof(double));
                    dt.Columns.Add("Field Percentage Test", typeof(double));
                }
                Row = dt.NewRow();
                Row[0] = RowVals[0];
                Row[1] = RowVals[1];
                Row[2] = Convert.ToInt32(RowVals[2]);
                Row[3] = Convert.ToBoolean(RowVals[3]);
                try
                {
                    if (Information.UBound(RowVals) > 4)
                    {
                        Row[4] = Convert.ToDouble(RowVals[4]);
                        Row[5] = Convert.ToDouble(RowVals[5]);
                        Row[6] = RowVals[6];
                        if (!string.IsNullOrEmpty(RowVals[7]))
                            Row[7] = Convert.ToDouble(RowVals[7]);
                        else
                            Row[7] = 0;
                        if (!string.IsNullOrEmpty(RowVals[8]))
                            Row[8] = Convert.ToDouble(RowVals[8]);
                        else
                            Row[8] = 0;
                        if (!string.IsNullOrEmpty(RowVals[9]))
                            Row[9] = Convert.ToInt32(RowVals[9]);
                        else
                            Row[9] = 0;
                    }
                    else
                    {
                        Row[4] = 0;
                        Row[5] = 0;
                        Row[6] = "";
                        //RowVals(7) = 0
                        // 'RowVals(8) = 0
                        // RowVals(9) = 0
                    }
                    //If RowVals(4) <> "" Then Row.Item(4) = CDbl(RowVals(4)) Else Row.Item(4) = 0
                    //If RowVals(5) <> "" Then Row.Item(5) = CDbl(RowVals(5)) Else Row.Item(5) = 0
                    //If RowVals(6) <> "" Then Row.Item(6) = CStr(RowVals(6)) Else Row.Item(6) = ""

                }
                catch (Exception ex)
                {
                }
                //If RowVals(4) <> "" Then Row.Item(4) = CDbl(RowVals(4)) Else Row.Item(4) = 0
                //If RowVals(5) <> "" Then Row.Item(5) = CDbl(RowVals(5)) Else Row.Item(5) = 0
                //If RowVals(6) <> "" Then Row.Item(6) = CStr(RowVals(6)) Else Row.Item(6) = ""
                //If RowVals(7) <> "" Then Row.Item(7) = CDbl(RowVals(7)) Else Row.Item(7) = 0
                //If RowVals(8) <> "" Then Row.Item(8) = CDbl(RowVals(8)) Else Row.Item(8) = 0
                //Row.Item(5) = 0
                //Row.Item(6) = ""
                //Row.Item(7) = 0
                //Row.Item(8) = 0
                //DTable.Rows.Add(Row)
                dt.Rows.Add(Row);
                Session["mytable"] = dt;
                GridBind(dt);

            }
            catch (Exception ex)
            {
            }


        }
        public void AddRows(List<LiveMonitoring.IRemoteLib.SensorFieldsDef> RowVals)
        {
            try
            {
                DataTable dt = new DataTable();
                //= CType(Session["mytable"], DataTable)
                //ListFiles()

                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                    //For Each row1 As DataRow In dt.Rows
                    // dt.ImportRow(row1)
                    //Next

                }
                //"InputStatus", "", "1", "true"
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field Name", typeof(string));
                    dt.Columns.Add("Field Suffix", typeof(string));
                    dt.Columns.Add("Field", typeof(int));
                    dt.Columns.Add("Display Field", typeof(bool));
                    dt.Columns.Add("Field Max Val", typeof(double));
                    dt.Columns.Add("Field Min Val", typeof(double));
                    dt.Columns.Add("Field Notes", typeof(string));
                    dt.Columns.Add("Field Max Warn Val", typeof(double));
                    dt.Columns.Add("Field Min Warn Val", typeof(double));
                    dt.Columns.Add("Field Percentage Test", typeof(double));
                }
                DataRow Row = default(DataRow);
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in RowVals)
                {
                    Row = dt.NewRow();
                    try
                    {
                        Row[0] = MyField.FieldName;
                        Row[1] = MyField.Caption;
                        Row[2] = MyField.FieldNumber;
                        Row[3] = MyField.DisplayValue;
                        if (!string.IsNullOrEmpty(MyField.FieldMaxValue.ToString()))
                            Row[4] = MyField.FieldMaxValue;
                        else
                            Row[4] = 0;
                        if (!string.IsNullOrEmpty(MyField.FieldMinValue.ToString()))
                            Row[5] = MyField.FieldMinValue;
                        else
                            Row[5] = 0;
                        if (!string.IsNullOrEmpty(MyField.FieldNotes.ToString()))
                            Row[6] = MyField.FieldNotes;
                        else
                            Row[6] = "";
                        if (!string.IsNullOrEmpty(MyField.FieldMaxWarnValue.ToString()))
                            Row[7] = MyField.FieldMaxWarnValue;
                        else
                            Row[7] = 0;
                        if (!string.IsNullOrEmpty(MyField.FieldMinWarnValue.ToString()))
                            Row[8] = MyField.FieldMinWarnValue;
                        else
                            Row[8] = 0;
                        if (!string.IsNullOrEmpty(MyField.FieldPercentOfTest.ToString()))
                            Row[9] = MyField.FieldPercentOfTest;
                        else
                            Row[9] = 0;

                    }
                    catch (Exception ex)
                    {
                    }
                    dt.Rows.Add(Row);


                }
                Session["mytable"] = dt;
                GridBind(dt);

            }
            catch (Exception ex)
            {
            }


        }
        public void ClearRows()
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Field Name", typeof(string));
                dt.Columns.Add("Field Suffix", typeof(string));
                dt.Columns.Add("Field", typeof(int));
                dt.Columns.Add("Display Field", typeof(bool));
                dt.Columns.Add("Field Max Val", typeof(double));
                dt.Columns.Add("Field Min Val", typeof(double));
                dt.Columns.Add("Field Notes", typeof(string));
                dt.Columns.Add("Field Max Warn Val", typeof(double));
                dt.Columns.Add("Field Min Warn Val", typeof(double));
                dt.Columns.Add("Field Percentage Test", typeof(double));
            }
            Session["mytable"] = dt;
            GridBind(dt);
        }

        public void GridBind(DataTable dt)
        {
            this.cmbFields.DataSource = dt;
            cmbFields.DataKeyNames = (new string[] { "Field" });
            cmbFields.DataBind();
        }
        public void GridBind1(DataTable dt)
        {
            this.gridNewSensors.DataSource = dt;
            gridNewSensors.DataKeyNames = (new string[] { "ID" });
            gridNewSensors.DataBind();
        }
        private void LoadSensorsGrid()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;
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
                            AddRows((new string[] {
                                Mysensor.ID.ToString(),
                                Mysensor.Caption,
                                Mysensor.Type.ToString(),
                                Mysensor.SiteID.ToString(),
                                Mysensor.IPDeviceID.ToString(),
                                Mysensor.Register.ToString(),
                                Mysensor.ScanRate.ToString()

                            }));
                        }

                       // foreach (LiveMonitoring.IRemoteLib.SensorDetails Mysensor in MySensorCollection)
                       // {
                            //LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;

                            //AddRows((new string[] {
                            //    Mysensor.ID.ToString(),
                            //    Mysensor.Caption,
                            //    Mysensor.Type.ToString(),
                            //    Mysensor.SiteID.ToString(),
                            //    Mysensor.IPDeviceID.ToString(),
                            //    Mysensor.Register.ToString(),
                            //    Mysensor.ScanRate.ToString()

                            //}));
                       // }


                        ////if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        ////{
                        ////    //foreach (LiveMonitoring.IRemoteLib.SensorDetails Mysensor in MyCollection)
                        ////    //{
                        ////        LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;

                        ////        AddRows((new string[] {
                        ////    Mysensor.ID.ToString(),
                        ////    Mysensor.Caption,
                        ////    Mysensor.Type.ToString(),
                        ////    Mysensor.SiteID.ToString(),
                        ////    Mysensor.IPDeviceID.ToString(),
                        ////    Mysensor.Register.ToString(),
                        ////    Mysensor.ScanRate.ToString()

                        ////}));
                        ////   // }

                        ////}

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

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
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("Caption", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("SiteID", typeof(string));
                dt.Columns.Add("IPDeviceID", typeof(string));
                dt.Columns.Add("Register", typeof(string));
                dt.Columns.Add("ScanRate", typeof(double));

            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];
            Row[5] = RowVals[5];
            Row[6] = Convert.ToDouble(RowVals[6]);
            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mytableSens"] = dt;
            GridBind1(dt);

        }

        public void LoadSites()
        {
            try
            {
                List<LiveMonitoring.IRemoteLib.SiteDetails> MyCollection = new List<LiveMonitoring.IRemoteLib.SiteDetails>();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.GetServerAllSites;
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;
                if ((MyCollection == null))
                    return;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        //cmbSensGroup
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
                        {
                            LiveMonitoring.IRemoteLib.SiteDetails MySite = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
                            //not orphans
                            if (MySite.ID > -1)
                            {
                                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                MyItem.Text = MySite.SiteName;
                                MyItem.Value = MySite.ID.ToString();
                                MyItem.Selected = false;
                                try
                                {
                                    if (Convert.ToInt32(Session["SelectedSite"]) == MySite.ID)
                                    {
                                        MyItem.Selected = true;
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                                ddlSensorSite.Items.Add(MyItem);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                try
                {
                    LiveMonitoring.testing test = new LiveMonitoring.testing();
                    test.SortDropDown(ddlSensorSite);

                }
                catch (Exception ex)
                {
                }


            }
            catch (Exception ex)
            {
            }


        }
        public void LoadAlertGroups()
        {
            List<LiveMonitoring.IRemoteLib.GroupContacts> MyCollection = default(List<LiveMonitoring.IRemoteLib.GroupContacts>);
            ddlSensorDefaultAlertsGroup.Items.Clear();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.LiveMonServer.GetAllAlertGroups();
            if ((MyCollection == null))
                return;
            foreach (LiveMonitoring.IRemoteLib.GroupContacts MyObject1 in MyCollection)
            {
                try
                {
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyObject1.Name;
                    MyItem.Value = MyObject1.ID.ToString();
                    MyItem.Selected = false;
                    ddlSensorDefaultAlertsGroup.Items.Add(MyItem);

                }
                catch (Exception ex)
                {
                }
            }
        }
        public void LoadDevices(SharedFuncs.DeviceFilter Filter = null)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            ddlDevice.Items.Clear();
            ddlSensorGroup.Items.Clear();
            ddlSensorLocation.Items.Clear();
            int MyDiv = 1;
            bool added = false;
            if ((MyCollection == null))
                return;
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
                        if ((Filter == null))
                        {
                            ddlDevice.Items.Add(MyItem);
                        }
                        else
                        {
                            if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.Camera, 0))
                            {
                                ddlDevice.Items.Add(MyItem);
                            }
                        }
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.IPDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        if ((Filter == null))
                        {
                            ddlDevice.Items.Add(MyItem);
                        }
                        else
                        {
                            if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.IPDevice, Convert.ToInt32(Mysensor.Type)))
                            {
                                ddlDevice.Items.Add(MyItem);
                            }
                        }
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.OtherDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        if ((Filter == null))
                        {
                            ddlDevice.Items.Add(MyItem);
                        }
                        else
                        {
                            if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.OtherDevice, (int)Mysensor.Type))
                            {
                                ddlDevice.Items.Add(MyItem);
                            }
                        }
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                    {
                        LiveMonitoring.IRemoteLib.SNMPManagerDetails Mysensor = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = Mysensor.Caption;
                        MyItem.Value = Mysensor.ID.ToString();
                        MyItem.Selected = false;
                        if ((Filter == null))
                        {
                            ddlDevice.Items.Add(MyItem);
                        }
                        else
                        {
                            if (Filter.DeviceInFilter(LiveMonitoring.SharedFuncs.DeviceTypeItem.DeviceTypes.SNMPDevice, 0))
                            {
                                ddlDevice.Items.Add(MyItem);
                            }
                        }
                    }
                    //cmbSensGroup
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                    {
                        LiveMonitoring.IRemoteLib.SensorGroup MysensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MysensorGroup.SensorGroupName;
                        MyItem.Value = MysensorGroup.SensorGroupID.ToString();
                        MyItem.Selected = false;
                        ddlSensorGroup.Items.Add(MyItem);
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
                    {
                        LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
                        MyItem.Value = MyLocation.Id.ToString();
                        MyItem.Selected = false;
                        ddlSensorLocation.Items.Add(MyItem);
                    }

                }
                catch (Exception ex)
                {
                }

            }
            if ((Filter == null) == false)
            {
                if (ddlDevice.Items.Count == 0)
                {
                    //no Device message
                    warningMessage.Visible = true;
                    lblwarning.Text = "No suitable Devices for selected sensor type .Please add device first !";
                    ddlSensorType.Focus();
                    //Try
                    // System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ShownotyBehindScript", "ShownotyBehind('No Device for this sensor type .Please add device first !" + Now.ToString + "','warning','centerRight','defaultTheme');", True)
                    //Catch ex As Exception

                    //End Try

                    return;
                }
            }
            try
            {
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.SortDropDown(ddlSensorGroup);

            }
            catch (Exception ex)
            {
            }
            try
            {
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.SortDropDown(ddlDevice);

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
        //private void SortDropDown(DropDownList dd)
        //{
        //    try
        //    {
        //        ListItem[] ar = null;
        //        long i = 0;
        //        foreach (ListItem li in dd.Items)
        //        {
        //            Array.Resize(ref ar, i + 1);
        //            ar[i] = li;
        //            i += 1;
        //        }
        //        Array ar1 = ar;

        //        ar1.Sort(ar1, new ListItemComparer());
        //        dd.Items.Clear();
        //        dd.Items.AddRange(ar1);

        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}
        //private class ListItemComparer : IComparer
        //{
        //    public int Compare(object x, object y)
        //    {
        //        ListItem a = x;
        //        ListItem b = y;
        //        CaseInsensitiveComparer c = new CaseInsensitiveComparer();
        //        return c.Compare(a.Text, b.Text);
        //    }
        //}
        
        protected void cmdSend_Click(object sender, System.EventArgs e)
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
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            Collection MyCollection = default(Collection);
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()

            switch (Convert.ToInt32(this.ddlSensorType.SelectedValue))
            {
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AspenTechADOQuery:
                    if (string.IsNullOrEmpty(Convert.ToString(this.txtSerialNumber.Text)))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter SQL Query";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AspenTechODBCQuery:
                    if (string.IsNullOrEmpty(Convert.ToString(this.txtSerialNumber.Text)))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter SQL Query";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
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
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Output number.";
                        ddlSensorType.Focus();
                        return;
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Min Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Max Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBAllPoints:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field!";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBAllSoftware:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field!";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBHardware:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBOperatingSystem:
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Days Between Scans in Extra Value Field > 0.";
                        ddlSensorType.Focus();
                        return;
                    }
                    //check if device is snmp device

                    //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                    object MyObject1 = null;
                    //LiveMonitoring.IRemoteLib.SNMPManagerDetails
                    bool IsSnmp = false;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            if (string.IsNullOrEmpty(this.ddlSensorType.SelectedValue /*== MyObject1.ID*/))
                            {
                                IsSnmp = true;
                            }

                        }
                    }

                    if (IsSnmp == false)
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please Select a SNMP device.";
                        ddlSensorType.Focus();
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
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Min Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Max Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    if (string.IsNullOrEmpty(this.txtExtraData.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter WMI Namespace.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData1.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter WMI Property.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                    if (string.IsNullOrEmpty(this.cmbModels.SelectedValue))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please Select a valid model.";
                        ddlSensorType.Focus();

                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue.Text) | Information.IsNumeric(this.txtExtraValue.Text) == false)
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter a valid Bus no.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraValue1.Text) | Information.IsNumeric(this.txtExtraValue1.Text) == false)
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter a valid Module no.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please supply a Caption.";
                ddlSensorType.Focus();
                return;
            }
            else
            {
                lblwarning.Text = string.Empty;
            }
            //ok got a name is it unique
            try
            {
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    if (MyObject1_loopVariable is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        if ((string)this.txtCaption.Text == (string)MyObject1_loopVariable)
                        {
                            warningMessage.Visible = true;
                            lblwarning.Text = "Duplicated capation. Caption must be unique.";
                            ddlSensorType.Focus();
                            try
                            {
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ShownotyBehindScript", "ShownotyBehind('Sensor Not Added ! Duplicate Sensor Name supplied , must be unique !" + DateTime.Now.ToString() + " Caption:" + this.txtCaption.Text + "','warning','centerRight','defaultTheme');", true);

                            }
                            catch (Exception ex)
                            {
                            }

                            return;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
            }

            if (string.IsNullOrEmpty(this.ddlDevice.SelectedValue))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please select IP Device.";
                ddlSensorType.Focus();
                return;
            }
            LiveMonitoring.IRemoteLib.SensorDetails NewSensor = new LiveMonitoring.IRemoteLib.SensorDetails();

            object sensorType = this.ddlSensorType.SelectedValue;
            NewSensor.Type = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(ddlSensorType.SelectedValue);
            NewSensor.IPDeviceID = Convert.ToInt32(this.ddlDevice.SelectedValue);
            NewSensor.ModuleNo = Convert.ToInt32(this.txtModule.Text);
            NewSensor.Register = Convert.ToInt32(this.txtRegister.Text);
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.ddlSensorType.SelectedValue);

            if (MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint | MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AdroitSQLHistorian | MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WonderWareSQLHistorian | MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AspenTechADOQuery | MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AspenTechODBCQuery)
            {
                NewSensor.SerialNumber = this.txtSerialNumber.Text.ToString();
            }
            else
            {
                NewSensor.SerialNumber = this.txtSerialNumber.Text.ToString();
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

            NewSensor.Caption = this.txtCaption.Text;
            if ((byteNormal == null))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please set Image normal and try again.";
                ddlSensorType.Focus();
                return;
            }
            else
            {
                NewSensor.ImageNormal = imgNormal;
                NewSensor.ImageNormalByte = byteNormal;
            }

            if ((byteNoresponse == null))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please set Image No Response and try again.";
                ddlSensorType.Focus();
                return;
            }
            else
            {
                NewSensor.ImageNoResponse = imgNoresponse;
                NewSensor.ImageNoResponseByte = byteNoresponse;
            }

            if ((byteError == null))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please set Image Error and try again.";
                ddlSensorType.Focus();
                return;
            }
            else
            {
                NewSensor.ImageError = imgError;
                NewSensor.ImageErrorByte = byteError;
            }


            NewSensor.MinValue = Convert.ToDouble(txtZeroValue.Text);
            NewSensor.MaxValue = Convert.ToDouble(txtMaxValue.Text);
            NewSensor.Multiplier = Convert.ToDouble(this.txtMultiplier.Text);
            NewSensor.Divisor = Convert.ToDouble(this.txtDivisor.Text);
            if (Convert.ToDouble(this.txtScanRate.Text) > 0 & Convert.ToDouble(this.txtScanRate.Text) < 5000)
            {
                NewSensor.ScanRate = 5000;
            }
            else
            {
                NewSensor.ScanRate = Convert.ToDouble(this.txtScanRate.Text);
            }

            NewSensor.ExtraData = (this.txtExtraData.Text);

            NewSensor.ExtraData1 = (this.txtExtraData1.Text);
            NewSensor.ExtraData2 = (this.txtExtraData2.Text);
            NewSensor.ExtraData3 = (this.txtExtraData3.Text);
            NewSensor.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
            NewSensor.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
            //NewSensor.ScanRate = CDbl(Me.txtScanRate.Value)
            try
            {
                NewSensor.SensGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
                NewSensor.SensGroup.SensorGroupID = Convert.ToInt32(ddlSensorGroup.SelectedValue);
                NewSensor.SensGroup.SensorGroupName = ddlSensorGroup.Items[ddlSensorGroup.SelectedIndex].Text.Trim();

            }
            catch (Exception ex)
            {
            }
            try
            {
                if ((Session["SelectedSite"] == null) == false)
                {
                    NewSensor.Add2Site = Convert.ToInt32(ddlSensorSite.SelectedValue);
                }

            }
            catch (Exception ex)
            {
            }

            if (cmbSensOutput.Visible)
            {
                try
                {
                    NewSensor.OutputType = (LiveMonitoring.IRemoteLib.SensorDetails.OutputTypeDef)Convert.ToInt32(cmbSensOutput.SelectedValue);

                }
                catch (Exception ex)
                {
                }
            }

            int Myresp = MyRem.LiveMonServer.AddNewSensor(NewSensor);
            if (Myresp > 0)
            {
                try
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ShownotyBehindScript", "ShownotyBehind('Sensor Added" + DateTime.Now.ToString() + " Caption:" + NewSensor.Caption + " ID:" + Myresp.ToString() + "','success','centerRight','defaultTheme');", true);


                }
                catch (Exception ex)
                {
                }
                try
                {
                    if ((ddlSensorLocation.SelectedValue == null) == false)
                    {
                        MyRem.LiveMonServer.AddEditLocationLink(NewSensor.ID, Convert.ToInt32(ddlSensorLocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.Sensor, -99);
                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if ((ddlSensorDefaultAlertsGroup.SelectedValue == null) == false)
                    {
                        if (Convert.ToInt32(ddlSensorDefaultAlertsGroup.SelectedValue) > 0)
                        {
                            MyRem.LiveMonServer.LinkAlertGroups(NewSensor.ID, Convert.ToInt32(ddlSensorDefaultAlertsGroup.SelectedValue), MyUser.ID);
                        }
                    }

                }
                catch (Exception ex)
                {
                }

                DataTable dt = new DataTable();
                //= CType(Session["mytable"], DataTable)
                //'ListFiles()

                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                }
                try
                {
                    //DataRow MyRow = default(DataRow);
                    foreach (DataRow MyRow in dt.Rows)
                    {
                        try
                        {
                            LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                            MyField.FieldName = ("Field Name");
                            MyField.Caption = ("Field Suffix");
                            MyField.FieldNumber = Convert.ToInt32("Field");
                            MyField.DisplayValue = Convert.ToBoolean("Display Field");
                            MyField.FieldMaxValue = Convert.ToInt32("Field Max Val");
                            MyField.FieldMinValue = Convert.ToInt32("Field Min Val");
                            MyField.FieldNotes = ("Field Notes");
                            MyField.SensorID = Myresp;
                            //dt.Columns.Add("Field Max Warn Val", GetType(Double))
                            //dt.Columns.Add("Field Min Warn Val", GetType(Double))
                            MyField.FieldMaxWarnValue = Convert.ToInt32("Field Max Warn Val");
                            MyField.FieldMinWarnValue = Convert.ToInt32("Field Percentage Test");
                            MyField.FieldPercentOfTest = Convert.ToInt32("Field Percentage Test");
                            // dt.Columns.Add("Field Percentage Test", GetType(Double))
                            try
                            {
                                MyRem.LiveMonServer.EditSensorField(MyField);

                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }


                }
                catch (Exception ex)
                {
                }

                successMessage.Visible = true;
                lblSucces.Text = "Sensor Successfully added!";
                ddlSensorType.Focus();

                try
                {
                    MyRem.WriteLog("Add Sensor Succeed", "User:" + MyUser.ID.ToString() + "|" + Myresp.ToString());

                }
                catch (Exception ex)
                {
                }
                ClearVals();
            }
            else
            {
                try
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ShownotyBehindScript", "ShownotyBehind('Sensor Not Added Error !" + DateTime.Now.ToString() + " Caption:" + NewSensor.Caption + "','error','centerRight','defaultTheme');", true);

                }
                catch (Exception ex)
                {
                }
                lblErr.Visible = true;
                lblErr.Text = "An error has occured during save, Please try again!";
                ddlSensorType.Focus();
                try
                {
                    MyRem.WriteLog("Add Sensor Failed", "User:" + MyUser.ID.ToString() + "|" + this.txtCaption.Text);

                }
                catch (Exception ex)
                {
                }

            }

        }

        public void ClearVals()
        {
            txtSerialNumber.Visible = false;
            txtSerialNumber1.Visible = true;
            //lblErr.Visible = False
            txtModule.Text = Convert.ToString(0);
            this.txtRegister.Text = Convert.ToString(0);
            this.txtSerialNumber1.Text = "";
            this.txtCaption.Text = "";
            this.txtZeroValue.Text = Convert.ToString(0);
            this.txtMaxValue.Text = Convert.ToString(0);
            this.txtMultiplier.Text = Convert.ToString(0);
            this.txtDivisor.Text = Convert.ToString(0);
            this.txtScanRate.Text = Convert.ToString(5000);
        }

        protected void cmbType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToUInt32(this.ddlSensorType.SelectedValue);
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            SharedFuncs.DeviceFilter Filter = new SharedFuncs.DeviceFilter(MyEnum);
            LoadDevices(Filter);
            gridNewSensors.Visible = true;
            ClearRows();
            LiveMonitoring.IRemoteLib.SensorFieldsDefault MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDefault(MyEnum);
            if ((MyFields == null) == false)
            {
                AddRows(MyFields.FieldsList);
            }
            txtSerialNumber.Visible = false;
            txtSerialNumber1.Visible = true;
            lblExtraData.Text = "Extra Data";
            lblExtraData1.Text = "Extra Data 1";
            lblExtraData2.Text = "Extra Data 2";
            lblExtraData3.Text = "Extra Data 3";
            lblExtraValue.Text = "Extra Value";
            txtExtraValue.Text = Convert.ToString(0);
            cmbModels.Visible = false;
            txtExtraData.Visible = true;
            lblOutPut.Visible = false;
            cmbSensOutput.Visible = false;
            switch (MyEnum)
            {
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBAllPoints:
                    lblExtraValue.Text = "CMDB Days between scan";
                    txtExtraValue.Text = Convert.ToString(7);
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBAllSoftware:
                    lblExtraValue.Text = "CMDB Days between scan";
                    txtExtraValue.Text = Convert.ToString(7);
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBHardware:
                    lblExtraValue.Text = "CMDB Days between scan";
                    txtExtraValue.Text = Convert.ToString(7);
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMICMDBOperatingSystem:
                    lblExtraValue.Text = "CMDB Days between scan";
                    txtExtraValue.Text = Convert.ToString(7);
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RemoteIPMonPing:
                    // AddRow((New String() {"ResponseSpeed", "s", "1", "true"}))
                    // AddRow((New String() {"ResponseData", "", "2", "true"}))
                    this.lblExtraData.Text = "Remote Server";
                    this.txtExtraData.Text = "tcp://123.123.123.123:8000/IPMon";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:
                    break;
                // AddRow((New String() {"InputStatus", "", "1", "true"}))
                // AddRow((New String() {"OutputStatus", "", "2", "true"}))
                // AddRow((New String() {"MainsPhase1Volts", "v", "3", "true"}))
                // AddRow((New String() {"MainsPhase2Volts", "v", "4", "true"}))
                // AddRow((New String() {"MainsPhase3Volts", "v", "5", "true"}))
                // AddRow((New String() {"AlternatorPhase1Volts", "v", "6", "true"}))
                // AddRow((New String() {"AlternatorPhase2Volts", "v", "7", "true"}))
                // AddRow((New String() {"AlternatorPhase3Volts", "v", "8", "true"}))
                // AddRow((New String() {"DCInputVolts", "v", "9", "true"}))
                // AddRow((New String() {"SpeedInputRPM", "rpm", "10", "true"}))
                // AddRow((New String() {"PhaseRotationInput", "", "11", "true"}))
                // AddRow((New String() {"FunctionSwitchStatus", "", "12", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BaseAudio:
                    break;

                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReader247DB:
                    Label7.Text = "Reader Number";
                    this.txtModule.Text = Convert.ToString(0);
                    break;
                // AddRow((New String() {"Access Log", "Access", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReaderSagem:
                    Label7.Text = "Reader Number";
                    this.txtModule.Text = Convert.ToString(0);
                    break;
                // AddRow((New String() {"Access Log", "Access", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraAudio:
                    break;
                // AddRow((New String() {"Audio Level", "db", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDInput:
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                    Label8.Text = "Output Number";
                    txtRegister.Text = Convert.ToString(1);
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                    break;
                // AddRow((New String() {"Motion", "Cm", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraItemDetector:
                    break;
                // AddRow((New String() {"Detection", "Items", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Out", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"AIn", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                // AddRow((New String() {"Humidity", "rh", "2", "true"}))
                // AddRow((New String() {"Dew Point", "deg C", "3", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegTemperature:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegDryContact:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Status", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegFlood:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Status", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegPowerDetector:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Status", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegHumidity:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Humidity", "rh", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                    break;
                // AddRow((New String() {"Ping", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                    Label5.Text = "URL to Open";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"IS", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                    Label5.Text = "SNMP OID";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"SNMP", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                    Label5.Text = "SQL Query";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber1.Visible = false;
                    break;
                // AddRow((New String() {"SQL", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AspenTechADOQuery:
                    Label5.Text = "SQL Query";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber1.Visible = false;

                    break;

                // AddRow((New String() {"SQL", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AspenTechODBCQuery:
                    Label5.Text = "SQL Query";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber1.Visible = false;

                    break;
                // AddRow((New String() {"SQL", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAInput:
                    break;
                // AddRow((New String() {"AIn", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                    break;
                // AddRow((New String() {"Cnt", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    Label5.Text = "Bit To Check";
                    this.txtSerialNumber1.Text = "CC";
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    Label5.Text = "Bit To Check";
                    this.txtSerialNumber1.Text = "CC";
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUDOutput:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUForceMultipleCoils:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUreadWriteRegisters:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUwriteMultipleRegisters:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTUwriteSingleRegister:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbuswriteMultipleRegisters:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbuswriteSingleRegister:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMBusOffOnScheduledPoint:
                    break;
                // AddRow((New String() {"Out", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMBusOnOffScheduledPoint:
                    break;
                // AddRow((New String() {"Out", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusForceMultipleCoils:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusreadWriteRegisters:
                    // AddRow((New String() {"Out", "", "1", "true"}))
                    lblOutPut.Visible = true;
                    cmbSensOutput.Visible = true;
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                    break;
                // AddRow((New String() {"Float", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPRTUMbusRTD:
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OPCItem:
                    Label5.Text = "OPC Item Name";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Value", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GPSSensor:
                    break;
                // AddRow((New String() {"Latitude", "", "1", "true"}))
                // AddRow((New String() {"Longitude", "", "2", "true"}))
                // AddRow((New String() {"Speed", "km/h", "3", "true"}))
                // AddRow((New String() {"Bearing", "", "4", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireDigiPotenType2C:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Value", "val", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireLioBatMonitorType30:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Val", "value", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireDualAdressableSwitchC:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;

                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireQuadADType20:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;

                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireTHDSmartMonitorType26:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                // AddRow((New String() {"Humidity", "rh", "2", "true"}))
                // AddRow((New String() {"Dew Point", "deg C", "3", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireTempType10:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireTempType28:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OneWireThermocronType21:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                // AddRow((New String() {"Humidity", "rh", "2", "true"}))
                // AddRow((New String() {"Dew Point", "deg C", "3", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusAInput:
                    break;
                // AddRow((New String() {"AIn", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusAOutput:
                    break;
                // AddRow((New String() {"Out", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusCounter:
                    break;
                // AddRow((New String() {"Cnt", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusDInput:
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusDiscrete:
                    Label5.Text = "Bit To Check";
                    this.txtSerialNumber1.Text = "CC";
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusDOutput:
                    break;
                // AddRow((New String() {"Out", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusFloat:
                    break;
                // AddRow((New String() {"Float", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusRTD:
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SerialMbusADiscrete:
                    break;
                // AddRow((New String() {"Input", "on/off", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecUPS:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"IP Volt", "vac", "1", "true"}))
                // AddRow((New String() {"IP FaultVolt", "vac", "2", "true"}))
                // AddRow((New String() {"OP Volt", "vac", "3", "true"}))
                // AddRow((New String() {"OP Current", "amp", "4", "true"}))
                // AddRow((New String() {"IP Frequency", "hz", "5", "true"}))
                // AddRow((New String() {"BatVolt", "vdc", "6", "true"}))
                // AddRow((New String() {"Temperature", "deg C", "7", "true"}))
                // AddRow((New String() {"UPS Status", "bit", "8", "true"}))
                // AddRow((New String() {"Rating Voltage", "V", "9", "true"}))
                // AddRow((New String() {"Rating Frequency", "hz", "10", "true"}))
                // AddRow((New String() {"Rating Current", "A", "11", "true"}))
                // AddRow((New String() {"Rating Battery Voltage", "V", "12", "true"}))
                // AddRow((New String() {"Company Name", "", "13", "true"}))
                // AddRow((New String() {"UPS Model", "", "14", "true"}))
                // AddRow((New String() {"UPS Version", "", "15", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.Megatec3Phase:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"BatteryVoltage", "V", "1", "true"}))
                // AddRow((New String() {"BatteryCapacity", "%", "2", "true"}))
                // AddRow((New String() {"BatteryTimeRemaining", "min", "3", "true"}))
                // AddRow((New String() {"BatteryCurrent", "amp", "4", "true"}))
                // AddRow((New String() {"Temperature3Phase", "deg c", "5", "true"}))
                // AddRow((New String() {"IPFrequency3Phase", "hz", "6", "true"}))
                // AddRow((New String() {"BypassFrequency", "hz", "7", "true"}))
                // AddRow((New String() {"OPFrequency", "hz", "8", "true"}))
                // AddRow((New String() {"RectifierDCStatus", "", "9", "true"}))
                // AddRow((New String() {"UPSStatus3Phase", "", "10", "true"}))
                // AddRow((New String() {"InvertorFaultStatus", "", "11", "true"}))
                // AddRow((New String() {"IPVoltageRPhase", "V", "12", "true"}))
                // AddRow((New String() {"IPVoltageYPhase", "V", "13", "true"}))
                // AddRow((New String() {"IPVoltageBPhase", "V", "14", "true"}))
                // AddRow((New String() {"BypassVoltageRPhase", "V", "15", "true"}))
                // AddRow((New String() {"BypassVoltageYPhase", "V", "16", "true"}))
                // AddRow((New String() {"BypassVoltageBPhase", "V", "17", "true"}))
                // AddRow((New String() {"OPVoltageRPhase", "V", "18", "true"}))
                // AddRow((New String() {"OPVoltageYPhase", "V", "19", "true"}))
                // AddRow((New String() {"OPVoltageBPhase", "V", "20", "true"}))
                // AddRow((New String() {"LoadPercentRPhase", "%", "21", "true"}))
                // AddRow((New String() {"LoadPercentYPhase", "%", "22", "true"}))
                // AddRow((New String() {"LoadPercentBPhase", "%", "23", "true"}))
                // AddRow((New String() {"Rect_VoltRating", "V", "24", "true"}))
                // AddRow((New String() {"RectifierFrequencyRating", "hz", "25", "true"}))
                // AddRow((New String() {"Bypass_VoltRating", "V", "26", "true"}))
                // AddRow((New String() {"BypassSourceFrequencyRating", "hz", "27", "true"}))
                // AddRow((New String() {"OP_VoltRating", "", "28", "true"}))
                // AddRow((New String() {"OPFrequencyRating", "hz", "29", "true"}))
                // AddRow((New String() {"BatteryVoltageRating", "V", "30", "true"}))
                // AddRow((New String() {"Power_Rating", "", "31", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ShutUPS:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"IP Volt", "vac", "1", "true"}))
                // AddRow((New String() {"OP Volt", "vac", "2", "true"}))
                // AddRow((New String() {"OP Current", "amp", "3", "true"}))
                // AddRow((New String() {"Bat RemainCapacity", "%", "4", "true"}))
                // AddRow((New String() {"Bat RemainTime", "min", "5", "true"}))
                // AddRow((New String() {"UPS Status", "bit", "6", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReaderZKSoft:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"FPR", "User", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PowerWareSNMP:
                    Label5.Text = "SerialNumber";

                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.PowerWareSNMP MyPowerDevice = new LiveMonitoring.IRemoteLib.PowerWareSNMP();
                    LiveMonitoring.IRemoteLib.PowerWareSNMP.OIDObj MyPowerDeviceOID = default(LiveMonitoring.IRemoteLib.PowerWareSNMP.OIDObj);
                    MyPowerDevice.LoadModels("POWERWARE 9390");
                    int OIDCNT = 0;
                    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyPowerDevice.PowerWareOIDS) - 1; OIDCNT++)
                    {
                        try
                        {
                            MyPowerDeviceOID = MyPowerDevice.PowerWareOIDS[OIDCNT];
                            // AddRow((New String() {MyPowerDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RTSysTM3SNMP:
                    Label5.Text = "SerialNumber";

                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.RTSystemsSNMP MyPowerDevice1 = new LiveMonitoring.IRemoteLib.RTSystemsSNMP("TM3");
                    LiveMonitoring.IRemoteLib.RTSystemsSNMP.OIDObj MyPowerDeviceOID1 = default(LiveMonitoring.IRemoteLib.RTSystemsSNMP.OIDObj);

                    int OIDCNT1 = 0;
                    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyPowerDevice1.RTSystemsOIDS) - 1; OIDCNT1++)
                    {
                        try
                        {
                            MyPowerDeviceOID1 = MyPowerDevice1.RTSystemsOIDS[OIDCNT1];
                            // AddRow((New String() {MyPowerDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DeltaSNMP:
                    Label5.Text = "SerialNumber";

                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.DeltaSNMP MyPowerDevice2 = new LiveMonitoring.IRemoteLib.DeltaSNMP();
                    LiveMonitoring.IRemoteLib.DeltaSNMP.OIDObj MyPowerDeviceOID2 = default(LiveMonitoring.IRemoteLib.DeltaSNMP.OIDObj);
                    MyPowerDevice2.LoadModels("Delta");
                    int OIDCNT2 = 0;
                    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyPowerDevice2.DeltaOIDS) - 1; OIDCNT2++)
                    {
                        try
                        {
                            MyPowerDeviceOID2 = MyPowerDevice2.DeltaOIDS[OIDCNT2];
                            // AddRow((New String() {MyPowerDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.VoltronicUSBUPS:
                    Label5.Text = "SerialNumber";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"ACVoltage", "vac", "1", "true"}))
                // AddRow((New String() {"BatteryVoltage", "vdc", "2", "true"}))
                // AddRow((New String() {"Load", "%", "3", "true"}))
                // AddRow((New String() {"LoadFrequency", "hz", "4", "true"}))
                // AddRow((New String() {"LoadPower", "W", "5", "true"}))
                // AddRow((New String() {"LoadVoltage", "vac", "6", "true"}))
                // AddRow((New String() {"Temperature", "deg C", "7", "true"}))
                // AddRow((New String() {"AudibleAlarm", "on/off", "8", "true"}))
                // AddRow((New String() {"BackupOperation", "on/off", "9", "true"}))
                // AddRow((New String() {"BatteryCritical", "on/off", "10", "true"}))
                // AddRow((New String() {"BatteryLow", "on/off", "11", "true"}))
                // AddRow((New String() {"TestRunning", "on/off", "12", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegMonPDU8SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M8PDU";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice3 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID3 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice3.LoadModels(txtExtraData.Text);
                    int OIDCNT3 = 0;
                    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyContegDevice3.ContegOIDS) - 1; OIDCNT3++)
                    {
                        try
                        {
                            MyContegDeviceOID3 = MyContegDevice3.ContegOIDS[OIDCNT3];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegMonPDU24SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M24PDU";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice4 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID4 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice4.LoadModels(txtExtraData.Text);
                    int OIDCNT4 = 0;
                    for (OIDCNT4 = 0; OIDCNT4 <= Information.UBound(MyContegDevice4.ContegOIDS) - 1; OIDCNT4++)
                    {
                        try
                        {
                            MyContegDeviceOID4 = MyContegDevice4.ContegOIDS[OIDCNT4];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegMonPDU16SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M16PDU";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice5 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID5 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice5.LoadModels(txtExtraData.Text);
                    int OIDCNT5 = 0;
                    for (OIDCNT5 = 0; OIDCNT5 <= Information.UBound(MyContegDevice5.ContegOIDS) - 1; OIDCNT5++)
                    {
                        try
                        {
                            MyContegDeviceOID5 = MyContegDevice5.ContegOIDS[OIDCNT5];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegIntPDU008C3SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M8PDU";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice6 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID6 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice6.LoadModels(txtExtraData.Text);
                    int OIDCNT6 = 0;
                    for (OIDCNT6 = 0; OIDCNT6 <= Information.UBound(MyContegDevice6.ContegOIDS) - 1; OIDCNT6++)
                    {
                        try
                        {
                            MyContegDeviceOID6 = MyContegDevice6.ContegOIDS[OIDCNT6];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegIntPDU24SNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "M24PDU";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP MyContegDevice7 = new LiveMonitoring.IRemoteLib.ContegPDUSNMP();
                    LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj MyContegDeviceOID7 = default(LiveMonitoring.IRemoteLib.ContegPDUSNMP.OIDObj);
                    MyContegDevice7.LoadModels(txtExtraData.Text);
                    int OIDCNT7 = 0;
                    for (OIDCNT7 = 0; OIDCNT7 <= Information.UBound(MyContegDevice7.ContegOIDS) - 1; OIDCNT7++)
                    {
                        try
                        {
                            MyContegDeviceOID7 = MyContegDevice7.ContegOIDS[OIDCNT7];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegRamosCSNMP:
                    cmbModels.Visible = true;
                    txtExtraData.Visible = false;
                    LoadModels();
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "RamosMiniC";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.ContegRamosSNMP MyContegDevice8 = new LiveMonitoring.IRemoteLib.ContegRamosSNMP();
                    LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj MyContegDeviceOID8 = default(LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj);
                    MyContegDevice8.LoadModels(txtExtraData.Text);
                    int OIDCNT8 = 0;
                    for (OIDCNT8 = 0; OIDCNT8 <= Information.UBound(MyContegDevice8.ContegOIDS) - 1; OIDCNT8++)
                    {
                        try
                        {
                            MyContegDeviceOID8 = MyContegDevice8.ContegOIDS[OIDCNT8];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;

                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonSNMP:
                    cmbModels.Visible = true;
                    txtExtraData.Visible = false;
                    LoadModels();
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    txtExtraData.Text = "3268";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP MyContegDevice9 = new LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP();
                    LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj MyContegDeviceOID9 = default(LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj);
                    MyContegDevice9.LoadModels(txtExtraData.Text);
                    int OIDCNT9 = 0;
                    for (OIDCNT9 = 0; OIDCNT9 <= Information.UBound(MyContegDevice9.hwgroupOIDS) - 1; OIDCNT9++)
                    {
                        try
                        {
                            MyContegDeviceOID9 = MyContegDevice9.hwgroupOIDS[OIDCNT9];
                            // AddRow((New String() {MyContegDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLTemperature:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Temp", "deg C", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLDryContact:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Status", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLFlood:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Status", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLPowerDetector:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Status", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonXMLHumidity:
                    Label5.Text = "Sens ID";
                    break;
                // AddRow((New String() {"Humidity", "rh", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GamatronicsSNMP:
                    Label5.Text = "SerialNumber";

                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.GamaTronicsSNMP MyGamaDevice10 = new LiveMonitoring.IRemoteLib.GamaTronicsSNMP();
                    LiveMonitoring.IRemoteLib.GamaTronicsSNMP.OIDObj MyGamaDeviceOID10 = default(LiveMonitoring.IRemoteLib.GamaTronicsSNMP.OIDObj);
                    MyGamaDevice10.LoadModels("Default");
                    int OIDCNT10 = 0;
                    for (OIDCNT10 = 0; OIDCNT10 <= Information.UBound(MyGamaDevice10.GamaTronicsOIDS) - 1; OIDCNT10++)
                    {
                        try
                        {
                            MyGamaDeviceOID10 = MyGamaDevice10.GamaTronicsOIDS[OIDCNT10];
                            // AddRow((New String() {MyGamaDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecSNMP:
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "UPS Type-1Phase/3Phase";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.MegaTecSNMP MyPowerDevice11 = new LiveMonitoring.IRemoteLib.MegaTecSNMP();
                    LiveMonitoring.IRemoteLib.MegaTecSNMP.OIDObj MyPowerDeviceOID11 = default(LiveMonitoring.IRemoteLib.MegaTecSNMP.OIDObj);
                    MyPowerDevice11.LoadModels(this.txtExtraData.Text);
                    int OIDCNT11 = 0;
                    for (OIDCNT11 = 0; OIDCNT11 <= Information.UBound(MyPowerDevice11.MegaTecOIDS) - 1; OIDCNT11++)
                    {
                        try
                        {
                            MyPowerDeviceOID11 = MyPowerDevice11.MegaTecOIDS[OIDCNT11];
                            // AddRow((New String() {MyPowerDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetTCP:
                    LiveMonitoring.IRemoteLib.LovatoGenset Mygenset = new LiveMonitoring.IRemoteLib.LovatoGenset();
                    LiveMonitoring.IRemoteLib.LovatoGenset.GensetRow Myrow = default(LiveMonitoring.IRemoteLib.LovatoGenset.GensetRow);
                    int mycnt = 0;
                    for (mycnt = 0; mycnt <= Mygenset.GensetTable.GetUpperBound(0) - 1; mycnt++)
                    {
                        Myrow = Mygenset.GensetTable[mycnt];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DeepSeaGensetStatus:
                    LiveMonitoring.IRemoteLib.DeepSeaGenset Mygenset1 = new LiveMonitoring.IRemoteLib.DeepSeaGenset();
                    LiveMonitoring.IRemoteLib.DeepSeaGenset.GensetRow Myrow1 = default(LiveMonitoring.IRemoteLib.DeepSeaGenset.GensetRow);
                    int mycnt1 = 0;
                    for (mycnt1 = 0; mycnt1 <= Mygenset1.GensetTable.GetUpperBound(0) - 1; mycnt1++)
                    {
                        Myrow1 = Mygenset1.GensetTable[mycnt1];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DeepSeaGensetMonitor:
                    LiveMonitoring.IRemoteLib.DeepSeaGenset Mygenset2 = new LiveMonitoring.IRemoteLib.DeepSeaGenset();
                    LiveMonitoring.IRemoteLib.DeepSeaGenset.GensetRow Myrow2 = default(LiveMonitoring.IRemoteLib.DeepSeaGenset.GensetRow);
                    int mycnt2 = 0;
                    for (mycnt2 = 0; mycnt2 <= Mygenset2.GensetTable.GetUpperBound(0) - 1; mycnt2++)
                    {
                        Myrow2 = Mygenset2.GensetTable[mycnt2];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF120Mk4Status:
                    LiveMonitoring.IRemoteLib.AMFMK4Genset Mygenset3 = new LiveMonitoring.IRemoteLib.AMFMK4Genset();
                    LiveMonitoring.IRemoteLib.AMFMK4Genset.GensetRow Myrow3 = default(LiveMonitoring.IRemoteLib.AMFMK4Genset.GensetRow);
                    int mycnt3 = 0;
                    for (mycnt3 = 0; mycnt3 <= Mygenset3.GensetTable.GetUpperBound(0) - 1; mycnt3++)
                    {
                        Myrow3 = Mygenset3.GensetTable[mycnt3];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF120Mk4Monitor:
                    LiveMonitoring.IRemoteLib.AMFMK4Genset Mygenset4 = new LiveMonitoring.IRemoteLib.AMFMK4Genset();
                    LiveMonitoring.IRemoteLib.AMFMK4Genset.GensetRow Myrow4 = default(LiveMonitoring.IRemoteLib.AMFMK4Genset.GensetRow);
                    int mycnt4 = 0;
                    for (mycnt4 = 0; mycnt4 <= Mygenset4.GensetTable.GetUpperBound(0) - 1; mycnt4++)
                    {
                        Myrow4 = Mygenset4.GensetTable[mycnt4];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                //case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PowerWareSNMP:

                //    LiveMonitoring.IRemoteLib.PowerWareSNMP MyPowerDevice5 = new LiveMonitoring.IRemoteLib.PowerWareSNMP();
                //    LiveMonitoring.IRemoteLib.PowerWareSNMP.OIDObj MyPowerDeviceOID5 = default(LiveMonitoring.IRemoteLib.PowerWareSNMP.OIDObj);
                //    //for now just defult to 9390
                //    MyPowerDevice5.LoadModels("POWERWARE 9390");
                //    int OIDCNT5 = 0;
                //    for (OIDCNT5 = 0; OIDCNT5 <= Information.UBound(MyPowerDevice5.PowerWareOIDS) - 1; OIDCNT5++)
                //    {
                //        MyPowerDeviceOID5 = MyPowerDevice5.PowerWareOIDS[OIDCNT5];
                //        // AddRow((New String() {MyPowerDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))
                //    }

                //    break;
                //case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DeltaSNMP:
                //    LiveMonitoring.IRemoteLib.DeltaSNMP MyPowerDevice = new LiveMonitoring.IRemoteLib.DeltaSNMP();
                //    LiveMonitoring.IRemoteLib.DeltaSNMP.OIDObj MyPowerDeviceOID = default(LiveMonitoring.IRemoteLib.DeltaSNMP.OIDObj);
                //    //2000
                //    MyPowerDevice.LoadModels("Delta");
                //    int OIDCNT = 0;
                //    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyPowerDevice.DeltaOIDS) - 1; OIDCNT++)
                //    {
                //        MyPowerDeviceOID = MyPowerDevice.DeltaOIDS(OIDCNT);
                //        // AddRow((New String() {MyPowerDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))
                //    }

                //    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SMTPCheckSensor:
                    Label5.Text = "Email From";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Response", "ms", "1", "true"}))

                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPCheckSensor | LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPLastDateSensor | LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPCountSensor:
                    Label5.Text = "Email From";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"Response", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    Label5.Text = "WMI Class";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    lblExtraData.Text = "WMI Namespace";
                    lblExtraData1.Text = "WMI Property";
                    lblExtraData2.Text = "WMI Value";
                    break;
                // AddRow((New String() {"Value", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIMemorySensor:
                    Label5.Text = "Serial No";
                    break;
                // AddRow((New String() {"totalRAM", "mb", "1", "true"}))
                // AddRow((New String() {"freeRAM", "mb", "2", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIPCInfoSensor:
                    Label5.Text = "Serial No";
                    break;
                // AddRow((New String() {"Caption", "", "1", "true"}))
                // AddRow((New String() {"Version", "", "2", "true"}))
                // AddRow((New String() {"Manufacturer", "", "3", "true"}))
                // AddRow((New String() {"csname", "", "4", "true"}))
                // AddRow((New String() {"WindowsDirectory", "", "5", "true"}))
                // AddRow((New String() {"totalRAM", "mb", "6", "true"}))
                // AddRow((New String() {"freeRAM", "mb", "7", "true"}))
                // AddRow((New String() {"FreePhysicalMemory", "mb", "8", "true"}))
                // AddRow((New String() {"TotalVisibleMemorySize", "mb", "9", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIPrintersSensor:
                    Label5.Text = "Serial No";
                    break;
                // AddRow((New String() {"Name", "", "1", "true"}))
                // AddRow((New String() {"Name", "", "2", "true"}))
                // AddRow((New String() {"Name", "", "3", "true"}))
                // AddRow((New String() {"Name", "", "4", "true"}))
                // AddRow((New String() {"Name", "", "5", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIProcessorLoadSensor:
                    Label5.Text = "Serial No";
                    break;
                // AddRow((New String() {"CPU1", "%", "1", "true"}))
                // AddRow((New String() {"CPU2", "%", "2", "true"}))
                // AddRow((New String() {"CPU3", "%", "3", "true"}))
                // AddRow((New String() {"CPU4", "%", "4", "true"}))
                // AddRow((New String() {"CPU5", "%", "5", "true"}))
                // AddRow((New String() {"CPU6", "%", "6", "true"}))
                // AddRow((New String() {"CPU7", "%", "7", "true"}))
                // AddRow((New String() {"CPU8", "%", "8", "true"}))
                // AddRow((New String() {"CPU9", "%", "9", "true"}))
                // AddRow((New String() {"CPU10", "%", "10", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIProcessRunningSensor:
                    Label5.Text = "Process Name";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"ProcessLoaded", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIProcessThreadsSensor:
                    Label5.Text = "Process Name";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"ProcessThreads", "cnt", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIProcessMemorySensor:
                    Label5.Text = "Process name";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"WorkingSetSize", "K", "1", "true"}))
                // AddRow((New String() {"PeakWorkingSetSize", "mb", "2", "true"}))
                // AddRow((New String() {"PageFileUsage", "mb", "3", "true"}))
                // AddRow((New String() {"PeakPageFileUsage", "mb", "4", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIServicesSensor:
                    Label5.Text = "Serial No";
                    break;
                // AddRow((New String() {"Description", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIDrivesSensor:
                    Label5.Text = "Serial No";
                    break;
                // AddRow((New String() {"Device", "", "1", "true"}))
                // AddRow((New String() {"Used", "gb", "2", "true"}))
                // AddRow((New String() {"Total", "gb", "3", "true"}))
                // AddRow((New String() {"Device", "", "4", "true"}))
                // AddRow((New String() {"Used", "gb", "5", "true"}))
                // AddRow((New String() {"Total", "gb", "6", "true"}))
                // AddRow((New String() {"Device", "", "7", "true"}))
                // AddRow((New String() {"Used", "gb", "8", "true"}))
                // AddRow((New String() {"Total", "gb", "9", "true"}))
                // AddRow((New String() {"Device", "", "10", "true"}))
                // AddRow((New String() {"Used", "gb", "11", "true"}))
                // AddRow((New String() {"Total", "gb", "12", "true"}))
                // AddRow((New String() {"Device", "", "13", "true"}))
                // AddRow((New String() {"Used", "gb", "14", "true"}))
                // AddRow((New String() {"Total", "gb", "15", "true"}))
                // AddRow((New String() {"Device", "", "16", "true"}))
                // AddRow((New String() {"Used", "gb", "17", "true"}))
                // AddRow((New String() {"Total", "gb", "18", "true"}))
                // AddRow((New String() {"Device", "", "19", "true"}))
                // AddRow((New String() {"Used", "gb", "20", "true"}))
                // AddRow((New String() {"Total", "gb", "21", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPSite:
                    // AddRow((New String() {"Response", "ms", "1", "true"}))
                    lblExtraData.Text = "IP Adress/Host";
                    lblExtraValue.Text = "Slow Response";
                    lblExtraValue1.Text = "Report Threshhold";
                    txtExtraValue1.Text = "999999";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PSExch07Specific | LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PSExchX64Specific:
                    // AddRow((New String() {"ItemCount", "val", "1", "true"}))
                    // AddRow((New String() {"TotalItemSize", "byte", "2", "true"}))
                    // AddRow((New String() {"DatabaseName", "", "3", "true"}))
                    // AddRow((New String() {"DisplayName", "", "4", "true"}))
                    // AddRow((New String() {"LastLogonTime", "", "5", "true"}))
                    lblExtraData.Text = "";
                    lblExtraData2.Text = "Mailbox";
                    lblExtraValue.Text = "Slow Response";
                    lblExtraValue1.Text = "Count Threshhold";
                    txtExtraValue1.Text = "0";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PSExch07All:
                    // AddRow((New String() {"ItemCount", "val", "1", "true"}))
                    // AddRow((New String() {"TotalItemSize", "byte", "2", "true"}))
                    // AddRow((New String() {"DatabaseName", "", "3", "true"}))
                    // AddRow((New String() {"DisplayName", "", "4", "true"}))
                    // AddRow((New String() {"LastLogonTime", "", "5", "true"}))
                    lblExtraData.Text = "";
                    lblExtraData2.Text = "Mailbox";
                    lblExtraValue.Text = "Slow Response";
                    lblExtraValue1.Text = "Count Threshhold";
                    txtExtraValue1.Text = "0";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM20TCP:
                    LiveMonitoring.IRemoteLib.LovatoGensetRGAM20 myset = new LiveMonitoring.IRemoteLib.LovatoGensetRGAM20();
                    for (mycnt = 0; mycnt <= Information.UBound(myset.GensetTable) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.GensetTable(mycnt).SettingName, myset.GensetTable(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM60TCP:
                    LiveMonitoring.IRemoteLib.LovatoGensetRGAM60 myset1 = new LiveMonitoring.IRemoteLib.LovatoGensetRGAM60();
                    for (mycnt1 = 0; mycnt1 <= Information.UBound(myset1.GensetTable) - 1; mycnt1++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.GensetTable(mycnt).SettingName, myset.GensetTable(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIServiceRunningSensor:
                    // AddRow((New String() {"ServiceRunning", "on/off", "1", "true"}))
                    lblExtraData.Text = "Service Display Name";
                    break;
                //lblExtraData2.Text = "Mailbox"

                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIRegistrySensor:
                    // AddRow((New String() {"Registry Value", "val", "1", "true"}))
                    lblExtraData.Text = "HdefKey";
                    lblExtraData1.Text = "Sub Key Name";
                    lblExtraData2.Text = "Value Name";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCurrentValues:
                    // AddRow((New String() {"ActivePowerPhaseSys", "W", "1", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseA", "W", "2", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseB", "W", "3", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseC", "W", "4", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseSys", "var", "5", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseA", "var", "6", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseB", "var", "7", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseC", "var", "8", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseSys", "VA", "9", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseA", "VA", "10", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseB", "VA", "11", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseC", "VA", "12", "true"}))
                    // AddRow((New String() {"RMSCurrentPhaseA", "Amps", "13", "true"}))
                    // AddRow((New String() {"RMSCurrentPhaseB", "Amps", "14", "true"}))
                    // AddRow((New String() {"RMSCurrentPhaseC", "Amps", "15", "true"}))
                    // AddRow((New String() {"LineFrequency", "Hz", "16", "true"}))
                    // AddRow((New String() {"RMSVoltagePhaseA", "Volts", "17", "true"}))
                    // AddRow((New String() {"RMSVoltagePhaseB", "Volts", "18", "true"}))
                    // AddRow((New String() {"RMSVoltagePhaseC", "Volts", "19", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseSys", "pf", "20", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseA", "pf", "21", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseB", "pf", "22", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseC", "pf", "23", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseA", "degs", "24", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseB", "degs", "25", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseC", "degs", "26", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseL1L2", "degs", "27", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseL1L3", "degs", "28", "true"}))
                    // AddRow((New String() {"PhaseRotation", "", "29", "true"}))
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    //lblExtraValue1.Text = "Report Threshhold"
                    txtExtraValue.Text = "1";
                    lblExtraValue1.Text = "Tarrif ID";
                    txtExtraValue1.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCurrentValues:
                    // AddRow((New String() {"RMSCurrentPhaseA", "W", "1", "true"}))
                    // AddRow((New String() {"RMSCurrentPhaseB", "W", "2", "true"}))
                    // AddRow((New String() {"RMSCurrentPhaseC", "W", "3", "true"}))
                    // AddRow((New String() {"RMSVoltagePhaseA", "V", "4", "true"}))
                    // AddRow((New String() {"RMSVoltagePhaseB", "V", "5", "true"}))
                    // AddRow((New String() {"RMSVoltagePhaseC", "V", "6", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseSys", "pf", "7", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseA", "pf", "8", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseB", "pf", "9", "true"}))
                    // AddRow((New String() {"PowerFactorPhaseC", "pf", "10", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseSys", "W", "11", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseA", "W", "12", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseB", "W", "13", "true"}))
                    // AddRow((New String() {"ActivePowerPhaseC", "W", "14", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseSys", "var", "15", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseA", "var", "16", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseB", "var", "17", "true"}))
                    // AddRow((New String() {"RectivePowerPhaseC", "var", "18", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseSys", "VA", "19", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseA", "VA", "20", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseB", "VA", "21", "true"}))
                    // AddRow((New String() {"ApparentPowerPhaseC", "VA", "22", "true"}))
                    // AddRow((New String() {"PhaseRotation", "", "23", "true"}))
                    // AddRow((New String() {"LineFrequencyPhaseA", "Hz", "24", "true"}))
                    // AddRow((New String() {"LineFrequencyPhaseB", "Hz", "25", "true"}))
                    // AddRow((New String() {"LineFrequencyPhaseC", "Hz", "26", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseA", "degs", "27", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseB", "degs", "28", "true"}))
                    // AddRow((New String() {"PhaseAnglePhaseC", "degs", "29", "true"}))
                    // AddRow((New String() {"RMSCurrentScaledPhaseA", "", "30", "true"}))
                    // AddRow((New String() {"RMSCurrentScaledPhaseB", "", "31", "true"}))
                    // AddRow((New String() {"RMSCurrentScaledPhaseC", "", "32", "true"}))
                    // AddRow((New String() {"RMSVoltageScaledPhaseA", "", "33", "true"}))
                    // AddRow((New String() {"RMSVoltageScaledPhaseB", "", "34", "true"}))
                    // AddRow((New String() {"RMSVoltageScaledPhaseC", "", "35", "true"}))
                    // AddRow((New String() {"ActivePowerScaledPhaseSys", "", "36", "true"}))
                    // AddRow((New String() {"ActivePowerScaledPhaseA", "", "37", "true"}))
                    // AddRow((New String() {"ActivePowerScaledPhaseB", "", "38", "true"}))
                    // AddRow((New String() {"ActivePowerScaledPhaseC", "", "39", "true"}))
                    // AddRow((New String() {"RectivePowerScaledPhaseSys", "", "40", "true"}))
                    // AddRow((New String() {"RectivePowerScaledPhaseA", "", "41", "true"}))
                    // AddRow((New String() {"RectivePowerScaledPhaseB", "", "42", "true"}))
                    // AddRow((New String() {"RectivePowerScaledPhaseC", "", "43", "true"}))
                    // AddRow((New String() {"ApparentPowerScaledPhaseSys", "", "44", "true"}))
                    // AddRow((New String() {"ApparentPowerScaledPhaseA", "", "45", "true"}))
                    // AddRow((New String() {"ApparentPowerScaledPhaseB", "", "46", "true"}))
                    // AddRow((New String() {"ApparentPowerScaledPhaseC", "", "47", "true"}))

                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    //lblExtraValue1.Text = "Report Threshhold"
                    txtExtraValue.Text = "1";
                    lblExtraValue1.Text = "Tarrif ID";
                    txtExtraValue1.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCUMULATIVEREGISTERS:
                    // AddRow((New String() {"Import_Wh", "Wh", "1", "true"}))
                    // AddRow((New String() {"Export_Wh", "Wh", "2", "true"}))
                    // AddRow((New String() {"Q1_varh", "varh", "3", "true"}))
                    // AddRow((New String() {"Q2_varh", "varh", "4", "true"}))
                    // AddRow((New String() {"Q3_varh", "varh", "5", "true"}))
                    // AddRow((New String() {"Q4_varh", "varh", "6", "true"}))
                    // AddRow((New String() {"VAh_1", "VAh", "7", "true"}))
                    // AddRow((New String() {"VAh_2", "VAh", "8", "true"}))
                    // AddRow((New String() {"Reserved_1", "r", "9", "true"}))
                    // AddRow((New String() {"Reserved_2", "r", "10", "true"}))
                    // AddRow((New String() {"Reserved_3", "r", "11", "true"}))
                    // AddRow((New String() {"Reserved_4", "r", "12", "true"}))
                    // AddRow((New String() {"Reserved_5", "r", "13", "true"}))
                    // AddRow((New String() {"Reserved_6", "r", "14", "true"}))
                    // AddRow((New String() {"Customer_Defined_1", "", "15", "true"}))
                    // AddRow((New String() {"Customer_Defined_2", "", "16", "true"}))
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    //lblExtraValue1.Text = "Report Threshhold"
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCUMULATIVEREGISTERS:
                    // AddRow((New String() {"Import_Wh", "Wh", "1", "true"}))
                    // AddRow((New String() {"Export_Wh", "Wh", "2", "true"}))
                    // AddRow((New String() {"Q1_varh", "varh", "3", "true"}))
                    // AddRow((New String() {"Q2_varh", "varh", "4", "true"}))
                    // AddRow((New String() {"Q3_varh", "varh", "5", "true"}))
                    // AddRow((New String() {"Q4_varh", "varh", "6", "true"}))
                    // AddRow((New String() {"VAh", "VAh", "7", "true"}))
                    // AddRow((New String() {"VAh_2", "VAh", "8", "true"}))
                    // AddRow((New String() {"Customer_Defined_1", "", "9", "true"}))
                    // AddRow((New String() {"Customer_Defined_2", "", "10", "true"}))
                    // AddRow((New String() {"Customer_Defined_3Vah", "VAh", "11", "true"}))


                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    //lblExtraValue1.Text = "Report Threshhold"
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCumulativeMaxDemandRegisters:
                    // AddRow((New String() {"Cumulative_Register", "", "1", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source", "", "2", "true"}))
                    // AddRow((New String() {"Cumulative_Register1", "", "3", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source1", "", "4", "true"}))
                    // AddRow((New String() {"Cumulative_Register2", "", "5", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source2", "", "6", "true"}))
                    // AddRow((New String() {"Cumulative_Register3", "", "7", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source3", "", "8", "true"}))
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    //lblExtraValue1.Text = "Report Threshhold"
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCumulativeMaxDemandRegisters:
                    // AddRow((New String() {"Cumulative_Register", "", "1", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source", "", "2", "true"}))
                    // AddRow((New String() {"Cumulative_Register1", "", "3", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source1", "", "4", "true"}))
                    // AddRow((New String() {"Cumulative_Register2", "", "5", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source2", "", "6", "true"}))
                    // AddRow((New String() {"Cumulative_Register3", "", "7", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source3", "", "8", "true"}))
                    // AddRow((New String() {"Cumulative_Register4", "", "9", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source4", "", "10", "true"}))
                    // AddRow((New String() {"Cumulative_Register5", "", "11", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source5", "", "12", "true"}))
                    // AddRow((New String() {"Cumulative_Register6", "", "13", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source6", "", "14", "true"}))
                    // AddRow((New String() {"Cumulative_Register7", "", "15", "true"}))
                    // AddRow((New String() {"Cumulative_Register_Source7", "", "16", "true"}))
                    Label4.Text = "Warning Level";
                    Label11.Text = "Alert Level";
                    Label12.Text = "Critical Level";
                    lblExtraValue.Text = "Device ID";
                    //lblExtraValue1.Text = "Report Threshhold"
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile:
                    // AddRow((New String() {"Kw", "Kw", "1", "true"}))
                    // AddRow((New String() {"Kwh", "Kwh", "2", "true"}))
                    // AddRow((New String() {"pf", "pf", "3", "true"}))
                    // AddRow((New String() {"KVar", "KVar", "4", "true"}))
                    // AddRow((New String() {"KVA", "KVA", "5", "true"}))
                    // AddRow((New String() {"Period", "Period", "6", "true"}))
                    lblExtraData3.Text = "NotifiedMaxDemand";
                    lblExtraValue1.Text = "Tarrif ID";
                    txtExtraValue1.Text = "1";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile:
                    // AddRow((New String() {"Kw", "Kw", "1", "true"}))
                    // AddRow((New String() {"Kwh", "Kwh", "2", "true"}))
                    // AddRow((New String() {"pf", "pf", "3", "true"}))
                    // AddRow((New String() {"KVar", "KVar", "4", "true"}))
                    // AddRow((New String() {"KVA", "KVA", "5", "true"}))
                    // AddRow((New String() {"Period", "Period", "6", "true"}))
                    lblExtraData3.Text = "NotifiedMaxDemand";
                    lblExtraValue1.Text = "Tarrif ID";
                    txtExtraValue1.Text = "1";
                    lblExtraValue.Text = "Device ID";
                    txtExtraValue.Text = "1";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG800:
                    LiveMonitoring.IRemoteLib.LovatoDMG800 myset2 = new LiveMonitoring.IRemoteLib.LovatoDMG800();
                    for (mycnt2 = 0; mycnt2 <= Information.UBound(myset2.DMG800Table) - 1; mycnt2++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DMG800Table(mycnt).SettingName, myset.DMG800Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG700:
                    LiveMonitoring.IRemoteLib.LovatoDMG700 myset3 = new LiveMonitoring.IRemoteLib.LovatoDMG700();
                    for (mycnt3 = 0; mycnt3 <= Information.UBound(myset3.DMG700Table) - 1; mycnt3++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DMG700Table(mycnt).SettingName, myset.DMG700Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG300:
                    LiveMonitoring.IRemoteLib.LovatoDMG300 myset4 = new LiveMonitoring.IRemoteLib.LovatoDMG300();
                    for (mycnt4 = 0; mycnt4 <= Information.UBound(myset4.DMG300Table) - 1; mycnt4++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DMG300Table(mycnt).SettingName, myset.DMG300Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG210:
                    LiveMonitoring.IRemoteLib.LovatoDMG210 myset16 = new LiveMonitoring.IRemoteLib.LovatoDMG210();
                    for (mycnt = 0; mycnt <= Information.UBound(myset16.DMG210Table) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DMG210Table(mycnt).SettingName, myset.DMG210Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDCRK:
                    LiveMonitoring.IRemoteLib.LovatoDCRK mysetf = new LiveMonitoring.IRemoteLib.LovatoDCRK();
                    for (mycnt = 0; mycnt <= Information.UBound(mysetf.DCRKTable) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DCRKTable(mycnt).SettingName, myset.DCRKTable(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDCRJ:
                    LiveMonitoring.IRemoteLib.LovatoDCRJ myset17 = new LiveMonitoring.IRemoteLib.LovatoDCRJ();
                    for (mycnt = 0; mycnt <= Information.UBound(myset17.DCRJTable) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DCRJTable(mycnt).SettingName, myset.DCRJTable(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DirisA4041New:
                    LiveMonitoring.IRemoteLib.DirisA4041New myset18 = new LiveMonitoring.IRemoteLib.DirisA4041New();
                    for (mycnt = 0; mycnt <= Information.UBound(myset18.DirisA4041Table) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DirisA4041Table(mycnt).SettingName, myset.DirisA4041Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DirisA4041Old:
                    LiveMonitoring.IRemoteLib.DirisA4041Old myset20 = new LiveMonitoring.IRemoteLib.DirisA4041Old();
                    for (mycnt = 0; mycnt <= Information.UBound(myset20.DirisA4041Table) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.DirisA4041Table(mycnt).SettingName, myset.DirisA4041Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.YasKawaA1000:
                    LiveMonitoring.IRemoteLib.YasKawaA1000 myset11 = new LiveMonitoring.IRemoteLib.YasKawaA1000();
                    for (mycnt = 0; mycnt <= Information.UBound(myset11.YasKawaA1000Table) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.YasKawaA1000Table(mycnt).SettingName, myset.YasKawaA1000Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.YasKawaV1000:
                    LiveMonitoring.IRemoteLib.YasKawaV1000 myset12 = new LiveMonitoring.IRemoteLib.YasKawaV1000();
                    for (mycnt = 0; mycnt <= Information.UBound(myset12.YasKawaV1000Table) - 1; mycnt++)
                    {
                        //GensetTable(0).SettingName
                        //GensetTable(0).SettingUnits 
                        // AddRow((New String() {myset.YasKawaV1000Table(mycnt).SettingName, myset.YasKawaV1000Table(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzMBusC6000:
                    LiveMonitoring.IRemoteLib.StulzC6000ModBus Mygenset8 = new LiveMonitoring.IRemoteLib.StulzC6000ModBus();
                    LiveMonitoring.IRemoteLib.StulzC6000ModBus.StulzRow Myrow8 = default(LiveMonitoring.IRemoteLib.StulzC6000ModBus.StulzRow);
                    int mycnt8 = 0;
                    for (mycnt8 = 0; mycnt8 <= Mygenset8.StulzTable.GetUpperBound(0) - 1; mycnt8++)
                    {
                        Myrow8 = Mygenset8.StulzTable[mycnt8];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzMBusC5000:
                    LiveMonitoring.IRemoteLib.StulzC5000ModBus Mygenset0 = new LiveMonitoring.IRemoteLib.StulzC5000ModBus();
                    LiveMonitoring.IRemoteLib.StulzC5000ModBus.StulzRow Myrow0 = default(LiveMonitoring.IRemoteLib.StulzC5000ModBus.StulzRow);
                    int mycnt0 = 0;
                    for (mycnt0 = 0; mycnt0 <= Mygenset0.StulzTable.GetUpperBound(0) - 1; mycnt0++)
                    {
                        Myrow0 = Mygenset0.StulzTable[mycnt0];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzMBusC1010:
                    LiveMonitoring.IRemoteLib.StulzC1010ModBus Mygenset01 = new LiveMonitoring.IRemoteLib.StulzC1010ModBus();
                    LiveMonitoring.IRemoteLib.StulzC1010ModBus.StulzRow Myrow01 = default(LiveMonitoring.IRemoteLib.StulzC1010ModBus.StulzRow);
                    int mycnt01 = 0;
                    for (mycnt01 = 0; mycnt01 <= Mygenset01.StulzTable.GetUpperBound(0) - 1; mycnt01++)
                    {
                        Myrow01 = Mygenset01.StulzTable[mycnt01];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzMBusC1010Rack:
                    LiveMonitoring.IRemoteLib.StulzC1010RackModBus Mygenset02 = new LiveMonitoring.IRemoteLib.StulzC1010RackModBus();
                    LiveMonitoring.IRemoteLib.StulzC1010RackModBus.StulzRow Myrow02 = default(LiveMonitoring.IRemoteLib.StulzC1010RackModBus.StulzRow);
                    int mycnt02 = 0;
                    for (mycnt02 = 0; mycnt02 <= Mygenset02.StulzTable.GetUpperBound(0) - 1; mycnt02++)
                    {
                        Myrow02 = Mygenset02.StulzTable[mycnt02];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzMBusC1010STULSR:
                    LiveMonitoring.IRemoteLib.StulzC1010StulSRModBus Mygenset03 = new LiveMonitoring.IRemoteLib.StulzC1010StulSRModBus();
                    LiveMonitoring.IRemoteLib.StulzC1010StulSRModBus.StulzRow Myrow03 = default(LiveMonitoring.IRemoteLib.StulzC1010StulSRModBus.StulzRow);
                    int mycnt03 = 0;
                    for (mycnt03 = 0; mycnt03 <= Mygenset03.StulzTable.GetUpperBound(0) - 1; mycnt03++)
                    {
                        Myrow03 = Mygenset03.StulzTable[mycnt03];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzMBusC1002:
                    LiveMonitoring.IRemoteLib.StulzC1002ModBus Mygenset04 = new LiveMonitoring.IRemoteLib.StulzC1002ModBus();
                    LiveMonitoring.IRemoteLib.StulzC1002ModBus.StulzRow Myrow04 = default(LiveMonitoring.IRemoteLib.StulzC1002ModBus.StulzRow);
                    int mycnt04 = 0;
                    for (mycnt04 = 0; mycnt04 <= Mygenset04.StulzTable.GetUpperBound(0) - 1; mycnt04++)
                    {
                        Myrow04 = Mygenset04.StulzTable[mycnt04];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))

                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                    cmbModels.Visible = true;
                    LoadModels();
                    txtExtraData.Visible = false;
                    Label5.Text = "SerialNumber";
                    lblExtraData.Text = "Model";
                    lblExtraValue.Text = "Bus no.";
                    txtExtraValue.Text = Convert.ToString(1);
                    lblExtraValue1.Text = "Module no.";
                    txtExtraValue1.Text = Convert.ToString(1);
                    lblExtraData1.Text = "Global Adress";
                    txtExtraData1.Text = Convert.ToString(1);
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCC";
                    LiveMonitoring.IRemoteLib.StulzWIB8000SNMP MyStulzWIB8000SNMPDevice = new LiveMonitoring.IRemoteLib.StulzWIB8000SNMP(Convert.ToInt32(txtExtraValue.Text), Convert.ToInt32(txtExtraValue1.Text), Convert.ToInt32(txtExtraData1.Text));
                    LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj MyStulzDeviceOID = default(LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj);
                    MyStulzWIB8000SNMPDevice.LoadModels(cmbModels.SelectedValue);
                    int OIDCNT01 = 0;
                    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyStulzWIB8000SNMPDevice.StulzOIDS) - 1; OIDCNT++)
                    {
                        try
                        {
                            MyStulzDeviceOID = MyStulzWIB8000SNMPDevice.StulzOIDS[OIDCNT01];
                            // AddRow((New String() {MyStulzDeviceOID.OIDName, "val", (OIDCNT + 1).ToString, "true"}))

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                //Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEHydranM2
                // MyGEModbus = New LiveMonitoring.IRemoteLib.GEModbus("HydranM2")
                //Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEMiniTrans
                // MyGEModbus = New LiveMonitoring.IRemoteLib.GEModbus("MiniTrans")
                //Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEMultiTrans
                // MyGEModbus = New LiveMonitoring.IRemoteLib.GEModbus("MultiTrans")
                //Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GETapTrans
                // MyGEModbus = New LiveMonitoring.IRemoteLib.GEModbus("TapTrans")
                //Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GETransFix
                // MyGEModbus = New LiveMonitoring.IRemoteLib.GEModbus("TransFix")
                //Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEDualTrans
                // MyGEModbus = New LiveMonitoring.IRemoteLib.GEModbus("DualTrans")
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEDualTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus = new LiveMonitoring.IRemoteLib.GEModbus("DualTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow06 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt06 = 0;
                    for (mycnt06 = 0; mycnt06 <= MYGEMBus.GensetTable.GetUpperBound(0) - 1; mycnt06++)
                    {
                        Myrow06 = MYGEMBus.GensetTable[mycnt06];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GETransFix:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus1 = new LiveMonitoring.IRemoteLib.GEModbus("TransFix");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow07 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt07 = 0;
                    for (mycnt07 = 0; mycnt07 <= MYGEMBus1.GensetTable.GetUpperBound(0) - 1; mycnt07++)
                    {
                        Myrow07 = MYGEMBus1.GensetTable[mycnt07];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GETapTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus2 = new LiveMonitoring.IRemoteLib.GEModbus("TapTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow10 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt09 = 0;
                    for (mycnt09 = 0; mycnt09 <= MYGEMBus2.GensetTable.GetUpperBound(0) - 1; mycnt09++)
                    {
                        Myrow10 = MYGEMBus2.GensetTable[mycnt09];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEMultiTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus3 = new LiveMonitoring.IRemoteLib.GEModbus("MultiTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow11 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt12 = 0;
                    for (mycnt12 = 0; mycnt12 <= MYGEMBus3.GensetTable.GetUpperBound(0) - 1; mycnt12++)
                    {
                        Myrow11 = MYGEMBus3.GensetTable[mycnt12];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEMiniTrans:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus4 = new LiveMonitoring.IRemoteLib.GEModbus("MiniTrans");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow15 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt15 = 0;
                    for (mycnt15 = 0; mycnt15 <= MYGEMBus4.GensetTable.GetUpperBound(0) - 1; mycnt15++)
                    {
                        Myrow15 = MYGEMBus4.GensetTable[mycnt15];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.GEHydranM2:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.GEModbus MYGEMBus5 = new LiveMonitoring.IRemoteLib.GEModbus("HydranM2");
                    LiveMonitoring.IRemoteLib.GEModbus.DataRowValue Myrow16 = default(LiveMonitoring.IRemoteLib.GEModbus.DataRowValue);
                    int mycnt16 = 0;
                    for (mycnt16 = 0; mycnt16 <= MYGEMBus5.GensetTable.GetUpperBound(0) - 1; mycnt16++)
                    {
                        Myrow16 = MYGEMBus5.GensetTable[mycnt16];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.JanitzaUMG604MbusReadings:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.JanitzaModbus Mygenset17 = new LiveMonitoring.IRemoteLib.JanitzaModbus("UMG604R");
                    LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue Myrow17 = default(LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue);
                    int mycnt17 = 0;
                    for (mycnt17 = 0; mycnt17 <= Mygenset17.GensetTable.GetUpperBound(0) - 1; mycnt17++)
                    {
                        Myrow17 = Mygenset17.GensetTable[mycnt17];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.JanitzaUMG604MbusHarmonics:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.JanitzaModbus Mygenset18 = new LiveMonitoring.IRemoteLib.JanitzaModbus("UMG604Har");
                    LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue Myrow18 = default(LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue);
                    int mycnt18 = 0;
                    for (mycnt18 = 0; mycnt18 <= Mygenset18.GensetTable.GetUpperBound(0) - 1; mycnt18++)
                    {
                        Myrow18 = Mygenset18.GensetTable[mycnt18];
                        //Note: If not present, please add < InstrumentationKey > Your Key </ InstrumentationKey > to the top of this file.
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.JanitzaUMG604MbusAll:
                    lblExtraValue.Text = "TCP Conn Type Dir=0 Serial=1";
                    txtExtraValue.Text = Convert.ToString(0);
                    LiveMonitoring.IRemoteLib.JanitzaModbus Mygenset19 = new LiveMonitoring.IRemoteLib.JanitzaModbus("UMG604");
                    LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue Myrow19 = default(LiveMonitoring.IRemoteLib.JanitzaModbus.DataRowValue);
                    int mycnt19 = 0;
                    for (mycnt19 = 0; mycnt19 <= Mygenset19.GensetTable.GetUpperBound(0) - 1; mycnt19++)
                    {
                        Myrow19 = Mygenset19.GensetTable[mycnt19];
                        // AddRow((New String() {Myrow.SettingName, Myrow.SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TraceRouteSensor:
                    break;
                // AddRow((New String() {"TraceRoute", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000DemandResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000DemandResults myset21 = new LiveMonitoring.IRemoteLib.RockwellPM1000DemandResults();
                    for (mycnt = 0; mycnt <= Information.UBound(myset21.PM1000DemandResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000DemandResult(mycnt).SettingName, myset.PM1000DemandResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000EnergyLogResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000EnergyLog myset22 = new LiveMonitoring.IRemoteLib.RockwellPM1000EnergyLog();
                    for (mycnt = 0; mycnt <= Information.UBound(myset22.PM1000EnergyLogResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000EnergyLogResult(mycnt).SettingName, myset.PM1000EnergyLogResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000EnergyResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000EnergyResults myset23 = new LiveMonitoring.IRemoteLib.RockwellPM1000EnergyResults();
                    for (mycnt = 0; mycnt <= Information.UBound(myset23.PM1000EnergyResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000EnergyResult(mycnt).SettingName, myset.PM1000EnergyResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000LoadFactorlogResults:
                    break;
                //not Implemented
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000PowerResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000PowerResults myset24 = new LiveMonitoring.IRemoteLib.RockwellPM1000PowerResults();
                    for (mycnt = 0; mycnt <= Information.UBound(myset24.PM1000PowerResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000PowerResult(mycnt).SettingName, myset.PM1000PowerResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000TimeOfuseLogkVAResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000TOULogApparentEnergyDemand myset25 = new LiveMonitoring.IRemoteLib.RockwellPM1000TOULogApparentEnergyDemand();
                    for (mycnt = 0; mycnt <= Information.UBound(myset25.PM1000TOULogApparentEnergyDemandResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000TOULogApparentEnergyDemandResult(mycnt).SettingName, myset.PM1000TOULogApparentEnergyDemandResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000TimeOfuseLogkVARResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000TOULogReactiveEnergyDemand myset26 = new LiveMonitoring.IRemoteLib.RockwellPM1000TOULogReactiveEnergyDemand();
                    for (mycnt = 0; mycnt <= Information.UBound(myset26.PM1000TOULogReactiveEnergyDemandResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000TOULogReactiveEnergyDemandResult(mycnt).SettingName, myset.PM1000TOULogReactiveEnergyDemandResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000TimeOfuseLogKwhResults:
                    LiveMonitoring.IRemoteLib.RockwellPM1000TOULogRealEnergyDemand myset27 = new LiveMonitoring.IRemoteLib.RockwellPM1000TOULogRealEnergyDemand();
                    for (mycnt = 0; mycnt <= Information.UBound(myset27.PM1000TOULogRealEnergyDemandResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000TOULogRealEnergyDemandResult(mycnt).SettingName, myset.PM1000TOULogRealEnergyDemandResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000UnitStatusLog:
                    LiveMonitoring.IRemoteLib.RockwellPM1000StatusLog myset28 = new LiveMonitoring.IRemoteLib.RockwellPM1000StatusLog();
                    for (mycnt = 0; mycnt <= Information.UBound(myset28.PM1000StatusLogResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000StatusLogResult(mycnt).SettingName, myset.PM1000StatusLogResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000VoltsAmpsFrequency:
                    LiveMonitoring.IRemoteLib.RockwellPM1000VAFResults myset29 = new LiveMonitoring.IRemoteLib.RockwellPM1000VAFResults();
                    for (mycnt = 0; mycnt <= Information.UBound(myset29.PM1000VAFResult) - 1; mycnt++)
                    {
                        // AddRow((New String() {myset.PM1000VAFResult(mycnt).SettingName, myset.PM1000VAFResult(mycnt).SettingUnits, (mycnt + 1).ToString, "true"}))
                    }

                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FilesDIRAgeMonitor:
                    lblExtraData.Text = "Directory to watch";
                    break;
                // AddRow((New String() {"Oldest", "dt", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileAgeChangeMonitor:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "File to watch";
                    break;
                // AddRow((New String() {"Date", "dt", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileAgeChangeMonitorText:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "File to watch";
                    lblExtraData2.Text = "Keywords coma delimit";
                    break;
                // AddRow((New String() {"TextStatus", "st", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileMonitorTextCnt:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "File to watch";
                    lblExtraData2.Text = "Keywords coma delimit";
                    break;
                // AddRow((New String() {"TextStatus", "st", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileWatcher:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "File to watch";
                    break;
                // AddRow((New String() {"Status", "st", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileCounter:
                    lblExtraData.Text = "Directory to watch";
                    break;
                //lblExtraData1.Text = "File to watch"
                // AddRow((New String() {"Files", "cnt", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileFilteredCounter:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "Filter";
                    break;
                // AddRow((New String() {"Files", "cnt", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileCSVMonitorText:
                    lblExtraData.Text = "CSV File to check";
                    lblExtraData1.Text = "Keyword to find";
                    break;
                // AddRow((New String() {"Keyword", "cnt", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileWatcherText:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "File to watch";
                    lblExtraData2.Text = "Keywords coma delimit";
                    break;
                // AddRow((New String() {"TextStatus", "st", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.FileFilterWatcherText:
                    lblExtraData.Text = "Directory to watch";
                    lblExtraData1.Text = "File filter to watch script-&lt;CCYY&gt; &lt;MM&gt; &lt;DD&gt;";
                    lblExtraData2.Text = "Keywords coma delimit";
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WebServiceCheck:
                    lblExtraData.Text = "Argument 1";
                    lblExtraData1.Text = "Argument 2";
                    lblExtraData2.Text = "Argument 3";
                    break;
                // AddRow((New String() {"Response", "vals", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WebServiceWCF:
                    lblExtraData.Text = "Argument 1";
                    lblExtraData1.Text = "Argument 2";
                    lblExtraData2.Text = "Argument 3";
                    break;
                // AddRow((New String() {"Response", "vals", "1", "true"}))
                //'Added for EOH
                //AdroitService = 550
                //AdroitSQLHistorian = 555
                //WonderWareService = 560
                //WonderWareSQLHistorian = 565
                //OracleService = 570
                //OracleConnection = 575
                //OracleUptime = 580
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AdroitService:
                    Label5.Text = "Process Name";
                    this.txtSerialNumber1.Text = "Agent Server";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"ProcessLoaded", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AdroitSQLHistorian:
                    Label5.Text = "SQL Connection Str";
                    this.txtSerialNumber.Text = "Select top 1 from dblog";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber1.Visible = false;
                    break;
                // AddRow((New String() {"SQL", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WonderWareService:
                    Label5.Text = "Process Name";

                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    this.txtSerialNumber1.Text = "Wonderware Service";
                    break;
                // AddRow((New String() {"ProcessLoaded", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WonderWareSQLHistorian:
                    Label5.Text = "SQL Connection Str";
                    this.txtSerialNumber.Text = "Select top 1 from Wonderwarelog";
                    txtSerialNumber1.Visible = false;
                    txtSerialNumber.Visible = true;
                    break;
                // AddRow((New String() {"SQL", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OracleService:
                    Label5.Text = "Process Name";
                    this.txtSerialNumber1.Text = "OracleServiceXE";
                    this.txtSerialNumber1.Text = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";
                    break;
                // AddRow((New String() {"ProcessLoaded", "", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OracleConnection:
                    txtSerialNumber1.Visible = true;
                    break;
                //txtSerialNumber.Visible = False
                // AddRow((New String() {"Connection", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.OracleUptime:
                    Label5.Text = "Process Name";
                    this.txtSerialNumber1.Text = "OracleServiceXE";
                    //lblExtraData.Text = "Directory to watch"
                    lblExtraData.Text = "SQL Connection Str";
                    this.txtExtraData.Text = "select 'Hostname : ' || host_name,'Instance Name : ' || instance_name,'Started At : ' || to_char(startup_time,'DD-MON-YYYY HH24:MI:SS') stime,'Uptime : ' || floor(sysdate - startup_time) || ' days(s) ' ||trunc( 24*((sysdate-startup_time) - trunc(sysdate-startup_time))) || ' hour(s) ' ||mod(trunc(1440*((sysdate-startup_time) - trunc(sysdate-startup_time))), 60) ||' minute(s) ' || mod(trunc(86400*((sysdate-startup_time) - trunc(sysdate-startup_time))), 60) ||' seconds' uptime from sys.v_$instance";
                    txtSerialNumber1.Visible = true;
                    this.txtExtraData.Visible = true;
                    break;
                //txtSerialNumber.Visible = False
                // AddRow((New String() {"SQL", "ms", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLDBMemory:
                    Label5.Text = "";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                // AddRow((New String() {"SQL Memory", "MB", "1", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLDBLocks:
                    Label5.Text = "";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                // AddRow((New String() {"DatabaseName", "val", "1", "true"}))
                // AddRow((New String() {"Program_name", "val", "2", "true"}))
                // AddRow((New String() {"Status", "val", "3", "true"}))
                // AddRow((New String() {"Last_request", "val", "4", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLConnections:
                    Label5.Text = "";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                // AddRow((New String() {"DBName", "val", "1", "true"}))
                // AddRow((New String() {"No Connections", "val", "2", "true"}))
                // AddRow((New String() {"LoginName", "val", "3", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLBackupHist:
                    Label5.Text = "";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                // AddRow((New String() {"DBName", "val", "1", "true"}))
                // AddRow((New String() {"Backup_start_date", "dt", "2", "true"}))
                // AddRow((New String() {"Backup_finish_date", "dt", "3", "true"}))
                // AddRow((New String() {"Backup_size", "bytes", "4", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLDBStatus:
                    Label5.Text = "";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                // AddRow((New String() {"DBName", "val", "1", "true"}))
                // AddRow((New String() {"State", "val", "2", "true"}))
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLMirrorStatus:
                    Label5.Text = "";
                    txtSerialNumber.Visible = true;
                    txtSerialNumber.Visible = false;
                    break;
                    // AddRow((New String() {"DBName", "val", "1", "true"}))
                    // AddRow((New String() {"IsMirrorOn", "dt", "2", "true"}))
                    // AddRow((New String() {"Mirroring_state_desc", "dt", "3", "true"}))
                    // AddRow((New String() {"MirrorSafety", "bytes", "4", "true"}))
                    // AddRow((New String() {"Mirroring_role_desc", "bytes", "5", "true"}))
                    // AddRow((New String() {"MirrorServer", "bytes", "6", "true"}))
            }

        }
        public void LoadModels()
        {
            try
            {
                cmbModels.Items.Clear();
                LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.ddlSensorType.SelectedValue);
                switch (MyEnum)
                {
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                        cmbModels.Items.Add(new ListItem("Nothing", ""));
                        cmbModels.Items.Add(new ListItem("Stultz-C1002", "C1002"));
                        cmbModels.Items.Add(new ListItem("Stulz-C1010/C2020", "C1010/C2020"));
                        cmbModels.Items.Add(new ListItem("Stultz-C2020FCB", "C2020FCB"));
                        cmbModels.Items.Add(new ListItem("Stultz-C4000", "C4000"));
                        cmbModels.Items.Add(new ListItem("Stultz-C5000", "C5000"));
                        cmbModels.Items.Add(new ListItem("Stultz-C6000", "C6000"));
                        cmbModels.Items.Add(new ListItem("Stultz-C6000CH", "C6000CH"));
                        cmbModels.Items.Add(new ListItem("*Stultz-C7000IOC", "C7000IOC"));
                        cmbModels.Items.Add(new ListItem("Stultz-C7000CH", "C7000CH"));
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegRamosCSNMP:
                        cmbModels.Items.Add(new ListItem("Nothing", ""));
                        cmbModels.Items.Add(new ListItem("RamosMiniC", "RamosMiniC"));

                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonSNMP:
                        cmbModels.Items.Add(new ListItem("Nothing", ""));
                        cmbModels.Items.Add(new ListItem("Poseidon-3262", "3262"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3265", "3265"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3266", "3266"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3268", "3268"));
                        cmbModels.Items.Add(new ListItem("Poseidon-1250", "1250"));
                        cmbModels.Items.Add(new ListItem("Poseidon-2250", "2250"));
                        cmbModels.Items.Add(new ListItem("Poseidon-2261", "2261"));
                        cmbModels.Items.Add(new ListItem("Poseidon-3468", "3468"));
                        cmbModels.Items.Add(new ListItem("Poseidon-Sitemon", "Sitemon"));
                        cmbModels.Items.Add(new ListItem("Poseidon-4001", "4001"));
                        cmbModels.Items.Add(new ListItem("Poseidon-4002", "4002"));
                        cmbModels.Items.Add(new ListItem("STE2", "STE2"));
                        break;

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void cmbModels_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.ddlSensorType.SelectedValue);
            switch (MyEnum)
            {
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.StulzSNMPWIB8000:
                    try
                    {
                        this.txtExtraData.Text = cmbModels.SelectedValue;
                        ClearRows();
                        if (!string.IsNullOrEmpty(txtExtraValue.Text) & !string.IsNullOrEmpty(txtExtraValue1.Text) & !string.IsNullOrEmpty(txtExtraData1.Text))
                        {
                            LiveMonitoring.IRemoteLib.StulzWIB8000SNMP MyStulzWIB8000SNMPDevice = new LiveMonitoring.IRemoteLib.StulzWIB8000SNMP(Convert.ToInt32(txtExtraValue.Text), Convert.ToInt32(txtExtraValue1.Text), Convert.ToInt32(txtExtraData1.Text));
                            LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj MyStulzDeviceOID = default(LiveMonitoring.IRemoteLib.StulzWIB8000SNMP.OIDObj);
                            MyStulzWIB8000SNMPDevice.LoadModels(cmbModels.SelectedValue);
                            int OIDCNT1 = 0;
                            for (OIDCNT1 = 0; OIDCNT1 <= Information.UBound(MyStulzWIB8000SNMPDevice.StulzOIDS) - 1; OIDCNT1++)
                            {
                                try
                                {
                                    MyStulzDeviceOID = MyStulzWIB8000SNMPDevice.StulzOIDS[OIDCNT1];
                                    AddRow((new string[] {
                                                MyStulzDeviceOID.OIDName,
                                                "val",
                                                (OIDCNT1 + 1).ToString(),
                                                "true"
                                            }));

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        else
                        {
                            lblErr.Visible = true;
                            lblErr.Text = "Please enter Global ID/Address/Bus no.!";
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ContegRamosCSNMP:
                    try
                    {
                        this.txtExtraData.Text = cmbModels.SelectedValue;
                        ClearRows();
                        LiveMonitoring.IRemoteLib.ContegRamosSNMP MyContegSNMPDevice = new LiveMonitoring.IRemoteLib.ContegRamosSNMP(txtExtraData.Text);
                        LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj MyContegDeviceOID1 = default(LiveMonitoring.IRemoteLib.ContegRamosSNMP.OIDObj);
                        MyContegSNMPDevice.LoadModels(cmbModels.SelectedValue);
                        int OIDCNT2 = 0;
                        for (OIDCNT2 = 0; OIDCNT2 <= Information.UBound(MyContegSNMPDevice.ContegOIDS) - 1; OIDCNT2++)
                        {
                            try
                            {
                                MyContegDeviceOID1 = MyContegSNMPDevice.ContegOIDS[OIDCNT2];
                                AddRow((new string[] {
                                            MyContegDeviceOID1.OIDName,
                                            "val",
                                            (OIDCNT2 + 1).ToString(),
                                            "true"
                                        }));

                            }
                            catch (Exception ex)
                            {
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                //case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.APCSmartUPSSNMP:
                //    this.txtExtraData.Text = cmbModels.SelectedValue;
                //    ClearRows();
                //    txtExtraData.Visible = false;
                //    //LoadModels()
                //    // Label5.Text = "SerialNumber"
                //    //lblExtraData.Text = "Model"
                //    // txtExtraData.Text = "SRT 5000"
                //    //Me.txtSerialNumber1.InputMask = "CCCCCCCCCCCCCCCC"
                //    LiveMonitoring.IRemoteLib.APCSNMPUPS MyContegDevice = new LiveMonitoring.IRemoteLib.APCSNMPUPS();
                //    LiveMonitoring.IRemoteLib.APCSNMPUPS.OIDObj MyContegDeviceOID = default(LiveMonitoring.IRemoteLib.APCSNMPUPS.OIDObj);
                //    MyContegDevice.LoadModels(txtExtraData.Text);
                //    int OIDCNT = 0;
                //    for (OIDCNT = 0; OIDCNT <= Information.UBound(MyContegDevice.APCOIDS) - 1; OIDCNT++)
                //    {
                //        try
                //        {
                //            MyContegDeviceOID = MyContegDevice.APCOIDS(OIDCNT);
                //            AddRow((new string[] {
                //                        MyContegDeviceOID.OIDName,
                //                        "val",
                //                        (OIDCNT + 1).ToString,
                //                        "true"
                //                    }));

                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //    }

                //    break;
                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HW_GroupPoseidonSNMP:
                    try
                    {
                        this.txtExtraData.Text = cmbModels.SelectedValue;
                        ClearRows();
                        LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP MyHWGroupSNMPDevice = new LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP(txtExtraData.Text);
                        LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj MyHWGroupDeviceOID = default(LiveMonitoring.IRemoteLib.HW_GroupPoseidonSNMP.OIDObj);
                        MyHWGroupSNMPDevice.LoadModels(cmbModels.SelectedValue);
                        int OIDCNT = 0;
                        for (OIDCNT = 0; OIDCNT <= Information.UBound(MyHWGroupSNMPDevice.hwgroupOIDS) - 1; OIDCNT++)
                        {
                            try
                            {
                                MyHWGroupDeviceOID = MyHWGroupSNMPDevice.hwgroupOIDS[OIDCNT];
                                AddRow((new string[] {
                                            MyHWGroupDeviceOID.OIDName,
                                            "val",
                                            (OIDCNT + 1).ToString(),
                                            "true"
                                        }));

                            }
                            catch (Exception ex)
                            {
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                    break;
            }

        }

        protected void cmbFields_DataBinding(object sender, System.EventArgs e)
        {
            cmbFields.PageSize = 5;
            cmbFields.AllowPaging = true;
        }

        protected void cmbFields_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            cmbFields.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //Reset the edit index.
            cmbFields.EditIndex = -1;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            dynamic dt = (DataTable)Session["mytable"];
            dynamic row = cmbFields.Rows[e.RowIndex];
            dt.Rows[row.DataItemIndex]["Field Name"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Suffix"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Display Field"] = ((CheckBox)(row.Cells[4].Controls[0])).Checked;
            dt.Rows[row.DataItemIndex]["Field Max Val"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Min Val"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Notes"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Max Warn Val"] = ((TextBox)(row.Cells[8].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Min Warn Val"] = ((TextBox)(row.Cells[9].Controls[0])).Text;

            if (!string.IsNullOrEmpty(((TextBox)(row.Cells[10].Controls[0])).Text))
                dt.Rows[row.DataItemIndex]["Field Percentage Test"] = ((TextBox)(row.Cells[10].Controls[0])).Text;
            cmbFields.EditIndex = -1;
            Session["mytable"] = dt;
            GridBind(dt);
        }

        protected void cmbFields_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            cmbFields.EditIndex = e.NewEditIndex;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //find sensor types suported

            //Fill list with only those types of sensors
            try
            {
                txtCaption.Text = ddlDevice.SelectedItem.Text.ToUpper();

            }
            catch (Exception ex)
            {
            }


        }
        protected void btnTestSensor_Click(object sender, EventArgs e)
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
            LiveMonitoring.IRemoteLib.SensorDetails NewSensor = new LiveMonitoring.IRemoteLib.SensorDetails();
            //Dim Cursensor As New LiveMonitoring.IRemoteLib.SensorDetails
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();


            switch (Convert.ToInt32(this.ddlSensorType.SelectedValue))
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
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Output number.";
                        ddlSensorType.Focus();
                        return;
                    }

                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Min Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Max Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                    if (string.IsNullOrEmpty(this.txtSerialNumber.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Serial number.";
                        ddlSensorType.Focus();
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
                    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                    //GetServerObjects 'server1.GetAll()
                    object MyObject1 = null;
                    //LiveMonitoring.IRemoteLib.SNMPManagerDetails
                    bool IsSnmp = false;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            if (Convert.ToInt32(this.ddlDevice.SelectedValue) == (int)MyObject1)
                            {
                                IsSnmp = true;
                            }

                        }
                    }

                    if (IsSnmp == false)
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please Select a SNMP device.";
                        ddlSensorType.Focus();
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
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Min Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtZeroValue.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Max Value.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                    if (string.IsNullOrEmpty(this.txtModule.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Module number.";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtRegister.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter Register number.";
                        ddlSensorType.Focus();
                        return;
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                    if (string.IsNullOrEmpty(this.txtExtraData.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter WMI Namespace.";
                        ddlSensorType.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.txtExtraData1.Text))
                    {
                        warningMessage.Visible = true;
                        lblwarning.Text = "Please enter WMI Property.";
                        ddlSensorType.Focus();
                        return;
                    }

                    break;
            }
            if (string.IsNullOrEmpty(this.txtCaption.Text))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please supply a Caption.";
                ddlSensorType.Focus();
                return;
            }
            else
            {
                lblErr.Text = string.Empty;
            }
            if (string.IsNullOrEmpty(this.ddlDevice.SelectedValue))
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please select IP Device.";
                ddlSensorType.Focus();
                return;
            }
            //asign cur vals then replace
            //NewSensor = Cursensor
            bool ChangeType = false;
            if (NewSensor.Type != (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.ddlSensorType.SelectedValue))
            {
                ChangeType = true;
            }
            NewSensor.Type = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.ddlSensorType.SelectedValue);
            NewSensor.IPDeviceID = Convert.ToInt32(this.ddlDevice.SelectedValue);
            if (Information.IsNumeric(this.txtModule.Text))
            {
                NewSensor.ModuleNo = Convert.ToInt32(this.txtModule.Text);
            }
            else
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please supply a correct Module.";
                ddlSensorType.Focus();
                return;
            }

            if (Information.IsNumeric(this.txtRegister.Text))
            {
                NewSensor.Register = Convert.ToInt32(this.txtRegister.Text);
            }
            else
            {
                warningMessage.Visible = true;
                lblwarning.Text = "Please supply a correct Register.";
                ddlSensorType.Focus();
                return;
            }

            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)Convert.ToInt32(this.ddlSensorType.SelectedValue);
            if (MyEnum == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint)
            {
                NewSensor.SerialNumber = this.txtSerialNumber.Text;
            }
            else
            {
                NewSensor.SerialNumber = this.txtSerialNumber1.Text.Trim();
            }
            NewSensor.Caption = this.txtCaption.Text;
            if (this.filImageNormal.HasFile)
            {
                NewSensor.ImageNormal = Myfunc.Strip_Image(this.filImageNormal);
                NewSensor.ImageNormalByte = MyRem.ImagetoByte(NewSensor.ImageNormal, ImageFormat.Bmp);
            }
            if (this.filImageNoResponse.HasFile)
            {
                NewSensor.ImageNoResponse = Myfunc.Strip_Image(this.filImageNoResponse);
                NewSensor.ImageNoResponseByte = MyRem.ImagetoByte(NewSensor.ImageNoResponse, ImageFormat.Bmp);
            }
            if (this.filImageError.HasFile)
            {
                NewSensor.ImageError = Myfunc.Strip_Image(this.filImageError);
                NewSensor.ImageErrorByte = MyRem.ImagetoByte(NewSensor.ImageError, ImageFormat.Bmp);
            }
            NewSensor.MinValue = Convert.ToDouble(this.txtZeroValue.Text);
            NewSensor.MaxValue = Convert.ToDouble(this.txtMaxValue.Text);
            NewSensor.Multiplier = Convert.ToDouble(this.txtMultiplier.Text);
            NewSensor.Divisor = Convert.ToDouble(this.txtDivisor.Text);
            if (Convert.ToDouble(this.txtScanRate.Text) > 0 & Convert.ToDouble(this.txtScanRate.Text) < 5000)
            {
                NewSensor.ScanRate = 5000;
            }
            else
            {
                NewSensor.ScanRate = Convert.ToDouble(this.txtScanRate.Text);
            }
            NewSensor.ExtraData = (this.txtExtraData.Text);
            NewSensor.ExtraData1 = (this.txtExtraData1.Text);
            NewSensor.ExtraData2 = (this.txtExtraData2.Text);
            NewSensor.ExtraData3 = (this.txtExtraData3.Text);
            NewSensor.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
            NewSensor.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
            try
            {
                NewSensor.SensGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
                NewSensor.SensGroup.SensorGroupID = Convert.ToInt32(ddlSensorGroup.SelectedValue);
                NewSensor.SensGroup.SensorGroupName = ddlSensorGroup.Items[ddlSensorGroup.SelectedIndex].Text.Trim();

            }
            catch (Exception ex)
            {
            }
            if (cmbSensOutput.Visible)
            {
                try
                {
                    NewSensor.OutputType = (LiveMonitoring.IRemoteLib.SensorDetails.OutputTypeDef)Convert.ToInt32(cmbSensOutput.SelectedValue);
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "An error occured while getting the Output type!";
                    ddlSensorType.Focus();
                }
            }
            //Try
            // If IsNothing(Session["SelectedSite"]) = False Then
            // NewSensor.Add2Site = CInt(Session["SelectedSite"])
            // End If
            //Catch ex As Exception

            //End Try
            DataTable dt = new DataTable();
            //= CType(Session["mytable"], DataTable)

            if (Session["mytable"] == null == false)
            {
                dt = (DataTable)Session["mytable"];
            }

            try
            {
                // DataRow MyRow = default(DataRow);
                foreach (DataRow MyRow in dt.Rows)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.SensorFieldsDef MyField = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                        MyField.FieldName = ("Field Name");
                        MyField.Caption = ("Field Suffix");
                        MyField.FieldNumber = Convert.ToInt32(("Field"));
                        MyField.DisplayValue = Convert.ToBoolean("Display Field");
                        MyField.FieldMaxValue = Convert.ToInt32("Field Max Val");
                        MyField.FieldMinValue = Convert.ToInt32("Field Min Val");
                        MyField.FieldNotes = ("Field Notes");
                        MyField.SensorID = -1;
                        try
                        {
                            NewSensor.Fields.Add(MyField, MyField.FieldNumber.ToString());

                        }
                        catch (Exception ex)
                        {
                        }

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

        protected void txtSerialNumber_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnAddNewsensor_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClearNewSensor_Click(object sender, EventArgs e)
        {
            ClearVals();
        }

        protected void btnTestSensor_Click1(object sender, EventArgs e)
        {

        }
    }
}