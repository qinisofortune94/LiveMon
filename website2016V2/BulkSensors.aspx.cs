using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Data;

namespace website2016V2
{
    public partial class BulkSensors : System.Web.UI.Page
    {
        DataTable myDT = new DataTable();
        DataTable stDT = new DataTable();

        List<string> SensorNameList = new List<string>();
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
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            if (!(IsPostBack))
            {
                btnSubmit.Visible = true;
                GetSensorTemplates();
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateSensors();

        }
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            //LoadPeople();
            if (gdvSensorTemplates.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gdvSensorTemplates.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gdvSensorTemplates.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gdvSensorTemplates.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gdvSensorTemplates.DataKeys[myRow.RowIndex].Value.ToString();

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
                ViewState["Id"] = gdvSensorTemplates.SelectedIndex;

                //ContactID.Text = Id;

                //Session["myAlertScheduleID"] = gdvSensorTemplates.DataKeys[myRow.RowIndex].Value.ToString();
                //LoadGridRow(Convert.ToInt32(gdvSensorTemplates.DataKeys[myRow.RowIndex].Value));
                //LoadScheduleGrid(Convert.ToInt32(gdvSensorTemplates.DataKeys[myRow.RowIndex].Value));

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
                    //errorMessage.Visible = true;
                    //lblError.Text = "Alert schedule not deleted.";
                    //Alertsgrid.Focus();
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

        /// <summary>
        /// Get all the sensor templates and populate the gridview.
        /// </summary>
        /// <remarks></remarks>
        private void GetSensorTemplates()
        {
            List<LiveMonitoring.IRemoteLib.SensorTemplate> STList = new List<LiveMonitoring.IRemoteLib.SensorTemplate>();
            LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();

            STList = myrem.LiveMonServer.GetAllSensorTemplates();

            // Convert the list to a dataset/datatable to use as the datasource for the gridview.
            // LiveMonitoring.IRemoteLib.SensorTemplate stItem = new LiveMonitoring.IRemoteLib.SensorTemplate();
            DataTable DT = new DataTable();

            // Create the columns/headers
            DT.Columns.Add("TemplateID");
            DT.Columns.Add("TemplateName");
            DT.Columns.Add("Caption");
            DT.Columns.Add("IPDeviceID");
            DT.Columns.Add("Module");
            DT.Columns.Add("Registration");
            DT.Columns.Add("SerialNumber");
            DT.Columns.Add("LastValue");
            DT.Columns.Add("LastValueDT");
            DT.Columns.Add("ImageNormal");
            DT.Columns.Add("ImageError");
            DT.Columns.Add("ImageNoResponse");
            DT.Columns.Add("MinValue");
            DT.Columns.Add("MaxValue");
            DT.Columns.Add("Multiplier");
            DT.Columns.Add("Divisor");
            DT.Columns.Add("OffsetStart");
            DT.Columns.Add("ScanRate");
            DT.Columns.Add("OutputType");
            DT.Columns.Add("SiteID");
            DT.Columns.Add("SiteCritical");
            DT.Columns.Add("ExtraData");
            DT.Columns.Add("ExtraData1");
            DT.Columns.Add("ExtraData2");
            DT.Columns.Add("ExtraData3");
            DT.Columns.Add("ExtraValue");
            DT.Columns.Add("ExtraValue1");
            DT.Columns.Add("SensorGroup");
            DT.Columns.Add("ProxySensID");

            foreach (LiveMonitoring.IRemoteLib.SensorTemplate stItem in STList)
            {
                dynamic rowToAdd = DT.NewRow();
                rowToAdd["TemplateID"] = stItem.SensorTemplateID;
                rowToAdd["TemplateName"] = stItem.templateName;
                rowToAdd["Caption"] = stItem.ModuleNo;
                rowToAdd["IPDeviceID"] = stItem.IPDeviceID;
                rowToAdd["Module"] = stItem.ModuleNo;
                rowToAdd["Registration"] = stItem.Register;
                rowToAdd["SerialNumber"] = stItem.SerialNumber;
                rowToAdd["LastValue"] = stItem.LastValue;
                rowToAdd["LastValueDT"] = stItem.LastValueDt;
                rowToAdd["ImageNormal"] = stItem.ImageNormalByte;
                rowToAdd["ImageError"] = stItem.ImageErrorByte;
                rowToAdd["ImageNoResponse"] = stItem.ImageNoResponseByte;
                rowToAdd["MinValue"] = stItem.MinValue;
                rowToAdd["MaxValue"] = stItem.MaxValue;
                rowToAdd["Multiplier"] = stItem.Multiplier;
                rowToAdd["Divisor"] = stItem.Divisor;
                rowToAdd["OffsetStart"] = stItem.OffSetStart;
                rowToAdd["ScanRate"] = stItem.ScanRate;
                rowToAdd["OutputType"] = stItem.OutputType;
                rowToAdd["SiteID"] = stItem.SiteID;
                rowToAdd["SiteCritical"] = stItem.SiteCritical;
                rowToAdd["ExtraData"] = stItem.ExtraData;
                rowToAdd["ExtraData1"] = stItem.ExtraData1;
                rowToAdd["ExtraData2"] = stItem.ExtraData2;
                rowToAdd["ExtraData3"] = stItem.ExtraData3;
                rowToAdd["ExtraValue"] = stItem.ExtraValue;
                rowToAdd["ExtraValue1"] = stItem.ExtraValue1;
                rowToAdd["SensorGroup"] = stItem.SensorGroup;
                rowToAdd["ProxySensID"] = stItem.ProxySensorID;



                DT.Rows.Add(rowToAdd);
            }
            gdvSensorTemplates.DataSource = DT;
            gdvSensorTemplates.DataBind();
            Session["DTSensorTemplates"] = DT;
        }

        /// <summary>
        /// Create the sensors.
        /// </summary>
        /// <remarks></remarks>
        private void CreateSensors()
        {
            string prefix = "";
            int numberToCreate = 0;
            int stID = 0;
            bool isComplete = false;
            LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();

            try
            {

                if ((txtNumberSensors.Text.Length > 0 & Information.IsNumeric(txtNumberSensors.Text)))
                {
                    numberToCreate = Convert.ToInt32(txtNumberSensors.Text);

                }

                //stID = CInt(gdvSensorTemplates.SelectedRow.Cells(1).Text)

                //Create the sensors from the template.


                List<LiveMonitoring.IRemoteLib.SensorDetails> listSensor = new List<LiveMonitoring.IRemoteLib.SensorDetails>();

                DataTable DT = new DataTable();

                // Create the columns/headers
                DT.Columns.Add("Caption", typeof(string));
                DT.Columns.Add("IPDeviceID", typeof(int));
                DT.Columns.Add("Module", typeof(int));
                DT.Columns.Add("Registration", typeof(int));
                DT.Columns.Add("SerialNumber", typeof(string));
                DT.Columns.Add("LastValue", typeof(double));
                DT.Columns.Add("LastValueDT", typeof(System.DateTime));
                DT.Columns.Add("ImageNormal", typeof(byte[]));
                DT.Columns.Add("ImageError", typeof(byte[]));
                DT.Columns.Add("ImageNoResponse", typeof(byte[]));
                DT.Columns.Add("MinValue", typeof(double));
                DT.Columns.Add("MaxValue", typeof(double));
                DT.Columns.Add("Multiplier", typeof(double));
                DT.Columns.Add("Divisor", typeof(double));
                DT.Columns.Add("OffsetStart", typeof(double));
                DT.Columns.Add("ScanRate", typeof(int));
                DT.Columns.Add("OutputType", typeof(LiveMonitoring.IRemoteLib.SensorTemplate.OutputTypeDef));
                DT.Columns.Add("SiteID", typeof(int));
                DT.Columns.Add("SiteCritical", typeof(int));
                DT.Columns.Add("ExtraData", typeof(string));
                DT.Columns.Add("ExtraData1", typeof(string));
                DT.Columns.Add("ExtraData2", typeof(string));
                DT.Columns.Add("ExtraData3", typeof(string));
                DT.Columns.Add("ExtraValue", typeof(double));
                DT.Columns.Add("ExtraValue1", typeof(double));
                DT.Columns.Add("SensorGroup", typeof(int));
                DT.Columns.Add("ProxySensID", typeof(int));

                // Dim dtSensorTemplate As DataTable = gdvSensorTemplates.DataSource
                DataTable dtSensorTemplate = (DataTable)Session["DTSensorTemplates"];
                int intSel = gdvSensorTemplates.SelectedIndex;

                //if (cboShowImport.Checked)
                //{
                    //read file for caption names
                    Readfile();
               // }


                for (int X = 0; X <= (numberToCreate - 1); X++)
                {
                    dynamic rowToAdd = DT.NewRow();

                    LiveMonitoring.IRemoteLib.SensorTemplate.OutputTypeDef myEnum = new LiveMonitoring.IRemoteLib.SensorTemplate.OutputTypeDef();

                    if ((SensorNameList.ElementAt(X) == null) == false & cboShowImport.Checked == true)
                    {
                        rowToAdd["Caption"] = SensorNameList[X];

                    }
                    else
                    {
                        rowToAdd["Caption"] = dtSensorTemplate.Rows[intSel]["Caption"];
                    }

                    rowToAdd["IPDeviceID"] = dtSensorTemplate.Rows[intSel]["IPDeviceID"].ToString();
                    rowToAdd["Module"] = dtSensorTemplate.Rows[intSel]["Module"];
                    rowToAdd["Registration"] = dtSensorTemplate.Rows[intSel]["Registration"];
                    rowToAdd["SerialNumber"] = dtSensorTemplate.Rows[intSel]["SerialNumber"];
                    rowToAdd["LastValue"] = dtSensorTemplate.Rows[intSel]["LastValue"];
                    rowToAdd["LastValueDT"] = dtSensorTemplate.Rows[intSel]["LastValueDT"];
                    rowToAdd["ImageNormal"] = System.Text.Encoding.UTF8.GetBytes((String)dtSensorTemplate.Rows[intSel]["ImageNormal"]);
                    rowToAdd["ImageError"] = System.Text.Encoding.UTF8.GetBytes((String)dtSensorTemplate.Rows[intSel]["ImageError"]);
                    rowToAdd["ImageNoResponse"] = System.Text.Encoding.UTF8.GetBytes((String)dtSensorTemplate.Rows[intSel]["ImageNoResponse"]);
                    rowToAdd["MinValue"] = dtSensorTemplate.Rows[intSel]["MinValue"];
                    rowToAdd["MaxValue"] = dtSensorTemplate.Rows[intSel]["MaxValue"];
                    rowToAdd["Multiplier"] = dtSensorTemplate.Rows[intSel]["Multiplier"];
                    rowToAdd["Divisor"] = dtSensorTemplate.Rows[intSel]["Divisor"];
                    rowToAdd["OffsetStart"] = dtSensorTemplate.Rows[intSel]["OffsetStart"];
                    rowToAdd["ScanRate"] = dtSensorTemplate.Rows[intSel]["ScanRate"];
                    rowToAdd["OutputType"] = Enum.Parse(myEnum.GetType(), dtSensorTemplate.Rows[intSel]["OutputType"].ToString());
                    rowToAdd["SiteID"] = dtSensorTemplate.Rows[intSel]["SiteID"];
                    rowToAdd["SiteCritical"] = dtSensorTemplate.Rows[intSel]["SiteCritical"];
                    rowToAdd["ExtraData"] = dtSensorTemplate.Rows[intSel]["ExtraData"];
                    rowToAdd["ExtraData1"] = dtSensorTemplate.Rows[intSel]["ExtraData2"];
                    rowToAdd["ExtraData3"] = dtSensorTemplate.Rows[intSel]["ExtraData3"];
                    rowToAdd["ExtraValue"] = dtSensorTemplate.Rows[intSel]["ExtraValue"];
                    rowToAdd["ExtraValue1"] = dtSensorTemplate.Rows[intSel]["ExtraValue1"];
                    rowToAdd["ProxySensID"] = dtSensorTemplate.Rows[intSel]["ProxySensID"];
                    rowToAdd["SensorGroup"] = dtSensorTemplate.Rows[intSel]["SensorGroup"];

                    DT.Rows.Add(rowToAdd);
                }

                Session["DTBulk"] = DT;
                gdvBulk.DataSource = DT;
                gdvBulk.DataBind();


                //' isComplete = myrem.LiveMonServer.CreateBulkSensors(stID, prefix, numberToCreate)
                //lblMessage.Text = String.Format("SAVING COMPLETE: " + vbCrLf + " prefix is {0}. {1} sensors to create with the template with {2} id.", prefix, numberToCreate, stID)
                //lblMessage.ForeColor = Drawing.Color.Green

                btnSubmit.Visible = true;

            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message;
                Trace.Write("SensorBulkCreation.createSensors.error: " + ex.Message);
            }



        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //create list of sensor details.
            List<LiveMonitoring.IRemoteLib.SensorDetails> SensorDetailsList = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
            try
            {
                foreach (GridViewRow row in gdvBulk.Rows)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails sd = new LiveMonitoring.IRemoteLib.SensorDetails();
                    sd.Caption = row.Cells[1].Text;
                    sd.IPDeviceID = Convert.ToInt32(row.Cells[2].Text);
                    sd.ModuleNo = Convert.ToInt32(row.Cells[3].Text);
                    sd.Register = Convert.ToInt32(row.Cells[4].Text);
                    sd.SerialNumber = row.Cells[5].Text;
                    sd.LastValue = Convert.ToDouble(row.Cells[6].Text);
                    sd.LastValueDt = Convert.ToDateTime(row.Cells[7].Text);
                    sd.ImageNormalByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[8].Text);
                    sd.ImageErrorByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[9].Text);
                    sd.ImageNoResponseByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[10].Text);
                    sd.MinValue = Convert.ToDouble(row.Cells[11].Text);
                    sd.MaxValue = Convert.ToDouble(row.Cells[12].Text);
                    sd.Multiplier = Convert.ToDouble(row.Cells[13].Text);
                    sd.Divisor = Convert.ToDouble(row.Cells[14].Text);
                    sd.OffSetStart = Convert.ToDouble(row.Cells[15].Text);
                    sd.ScanRate = Convert.ToDouble(row.Cells[16].Text);
                    sd.OutputType = (LiveMonitoring.IRemoteLib.SensorDetails.OutputTypeDef)Convert.ToInt32(row.Cells[17].Text);
                    sd.SiteID = Convert.ToInt32(row.Cells[18].Text);
                    sd.SiteCritical = Convert.ToInt32(row.Cells[19].Text);
                    sd.ExtraData = row.Cells[20].Text;
                    sd.ExtraData1 = row.Cells[21].Text;
                    sd.ExtraData2 = row.Cells[22].Text;
                    sd.ExtraData3 = row.Cells[23].Text;
                    sd.ExtraValue = Convert.ToDouble(row.Cells[24].Text);
                    sd.ExtraValue1 = Convert.ToDouble(row.Cells[25].Text);
                    LiveMonitoring.IRemoteLib.SensorGroup mySensgroup = new LiveMonitoring.IRemoteLib.SensorGroup();
                    mySensgroup.SensorGroupID = Convert.ToInt32(row.Cells[26].Text);
                    sd.SensGroup = mySensgroup;
                    sd.ProxySensorID = Convert.ToInt32(row.Cells[27].Text);
                    SensorDetailsList.Add(sd);
                }

                //save the list of sensors
                LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();
                bool didSave = false;
                didSave = myrem.LiveMonServer.CreateBulkSensors(SensorDetailsList);
                if (didSave)
                {
                    successMessage.Visible = true;
                    lblSuccess.Text = "Save was successful.";
                }
                else
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Failed to save.";
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error found: " + ex.Message;
            }

        }

        private void bind()
        {
            gdvBulk.DataSource = (DataTable)Session["DTBulk"];
            gdvBulk.DataBind();
        }


        protected void gdvBulk_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvBulk.PageIndex = e.NewPageIndex;
            bind();
        }

        protected void gdvBulk_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdvBulk.EditIndex = -1;
            bind();
        }

        protected void gdvBulk_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdvBulk.EditIndex = e.NewEditIndex;
            bind();
        }

        protected void gdvBulk_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            gdvBulk.EditIndex = -1;
            bind();
        }

        protected void gdvBulk_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable myDTNew = default(DataTable);
            myDTNew = (DataTable)Session["DTBulk"];

            // Update the values.
            GridViewRow row = gdvBulk.Rows[e.RowIndex];

            myDTNew.Rows[row.DataItemIndex]["Caption"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["IPDeviceID"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Module"] = ((TextBox)(row.Cells[3].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Registration"] = ((TextBox)(row.Cells[4].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["SerialNumber"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["LastValue"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["LastValueDt"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNormal"] = System.Text.Encoding.UTF8.GetBytes(((TextBox)(row.Cells[8].Controls[0])).Text);
            myDTNew.Rows[row.DataItemIndex]["ImageError"] = System.Text.Encoding.UTF8.GetBytes(((TextBox)(row.Cells[9].Controls[0])).Text);
            myDTNew.Rows[row.DataItemIndex]["ImageNoResponse"] = System.Text.Encoding.UTF8.GetBytes(((TextBox)(row.Cells[10].Controls[0])).Text);
            myDTNew.Rows[row.DataItemIndex]["MinValue"] = ((TextBox)(row.Cells[11].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["MaxValue"] = ((TextBox)(row.Cells[12].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Multiplier"] = ((TextBox)(row.Cells[13].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Divisor"] = ((TextBox)(row.Cells[14].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["OffSetStart"] = ((TextBox)(row.Cells[15].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ScanRate"] = ((TextBox)(row.Cells[16].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["OutputType"] = ((TextBox)(row.Cells[17].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["SiteID"] = ((TextBox)(row.Cells[18].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["SiteCritical"] = ((TextBox)(row.Cells[19].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData"] = ((TextBox)(row.Cells[20].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData1"] = ((TextBox)(row.Cells[21].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData2"] = ((TextBox)(row.Cells[22].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData3"] = ((TextBox)(row.Cells[23].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraValue"] = ((TextBox)(row.Cells[24].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraValue1"] = ((TextBox)(row.Cells[25].Controls[0])).Text;
            LiveMonitoring.IRemoteLib.SensorGroup mySensgroup = new LiveMonitoring.IRemoteLib.SensorGroup();

            myDTNew.Rows[row.DataItemIndex]["SensorGroup"] = ((TextBox)(row.Cells[26].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ProxySensID"] = ((TextBox)(row.Cells[27].Controls[0])).Text;

            gdvBulk.EditIndex = -1;
            Session["DTBulk"] = myDTNew;
            bind();
        }


        protected void cboShowImport_CheckedChanged(object sender, EventArgs e)
        {
            if (cboShowImport.Checked)
            {
                tbrImportRow.Visible = true;
            }
            else
            {
                tbrImportRow.Visible = false;
            }
        }

        //#region "File Handeling"



        private string getColumnLetter(int column)
        {
            string result = "";
            if ((column / 26) == 0)
            {
                result += Strings.ChrW(column + 65);
            }
            else
            {
                result += (Strings.ChrW((column / 26) + 64)).ToString() + (Strings.ChrW((column % 26) + 65)).ToString();
            }

            return result;
        }

        private string convertToA1(int row, int column)
        {
            return getColumnLetter(column) + (row + 1).ToString();
        }


        private void Readfile()
        {
            if (FileUpload1.HasFile)
            {
                // Set the file.
                string filename = FileUpload1.FileName;
                string fileLocationToSaveTo = "~\\Uploads\\";
                string trueLocation = Server.MapPath(fileLocationToSaveTo + filename);

                // Save the file on the server to avoid permissions issues.
                FileUpload1.SaveAs(trueLocation);

                // Load the file.
                Exceldoc1.Load(trueLocation);

                // Use only the first sheet.
                Exceldoc1.SheetIndex = 0;




            }
            else
            {
                throw new Exception("Error: No file found.");
            }

            int rowCount = Convert.ToInt32(txtNumberSensors.Text);
            string sensorName = "";
            //Get the rows.
            //0 index row is the headings
            for (int r = 1; r <= rowCount; r++)
            {
                //Read the columns
                Exceldoc1.Cell = convertToA1(r, 1);
                if (Exceldoc1.CellText.Trim().Length > 0)
                {
                    if ((Exceldoc1.CellText.Contains("NULL") == false))
                    {
                        sensorName = Exceldoc1.CellValue;
                    }
                    else
                    {
                        sensorName = null;
                    }
                }
                if ((sensorName == null) == false)
                {
                    SensorNameList.Add(sensorName);
                }
            }
        }


        public void BulkCreation_SensorBulkCreation()
        {
            Load += Page_Load;
        }

        protected void BtnImportSensor_Click(object sender, EventArgs e)
        {

        }

        protected void ckbShowImport_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
  }
