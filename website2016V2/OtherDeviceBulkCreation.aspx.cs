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
    public partial class OtherDeviceBulkCreation : System.Web.UI.Page
    {
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
                if (!IsPostBack)
                {
                    GetOtherDeviceTemplates();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetOtherDeviceTemplates();
            }
            if (gdvOtherTemplates.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gdvOtherTemplates.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gdvOtherTemplates.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gdvOtherTemplates.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            createOtherDevice();
        }

        private void GetOtherDeviceTemplates()
        {
            List<LiveMonitoring.IRemoteLib.OtherDeviceTemplate> STList = new List<LiveMonitoring.IRemoteLib.OtherDeviceTemplate>();
            LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();

            STList = myrem.LiveMonServer.GetAllOtherDeviceTemplates();
            DataTable DT = new DataTable();

            // Create the columns/headers
            DT.Columns.Add("TemplateName");
            DT.Columns.Add("Type", typeof(LiveMonitoring.IRemoteLib.OtherDeviceTemplate.DeviceType));
            DT.Columns.Add("IPAdress");
            DT.Columns.Add("Port");
            DT.Columns.Add("SerialPort");
            DT.Columns.Add("SerialSettings");
            DT.Columns.Add("LastReadDT");
            DT.Columns.Add("ExtraData");
            DT.Columns.Add("ExtraData1");
            DT.Columns.Add("ExtraData2");
            DT.Columns.Add("ExtraData3");
            DT.Columns.Add("ExtraData4");
            DT.Columns.Add("ExtraData5");
            DT.Columns.Add("ImageNormal");
            DT.Columns.Add("ImageError");
            DT.Columns.Add("ImageNoResponse");
            DT.Columns.Add("Caption");

            foreach (LiveMonitoring.IRemoteLib.OtherDeviceTemplate stItem in STList)
            {
                var rowToAdd = DT.NewRow();
                //TODO: check if there are more fields

                rowToAdd["TemplateName"] = stItem.templateName;
                rowToAdd["Type"] = stItem.Type;
                rowToAdd["IPAdress"] = stItem.IPAdress;
                rowToAdd["Port"] = stItem.Port;
                rowToAdd["SerialPort"] = stItem.SerialPort;
                rowToAdd["SerialSettings"] = stItem.SerialSettings;
                rowToAdd["LastReadDT"] = stItem.LastReadDT;
                rowToAdd["ExtraData"] = stItem.ExtraData;
                rowToAdd["ExtraData1"] = stItem.ExtraData1;
                rowToAdd["ExtraData2"] = stItem.ExtraData2;
                rowToAdd["ExtraData3"] = stItem.ExtraData3;
                rowToAdd["ExtraData4"] = stItem.ExtraData4;
                rowToAdd["ExtraData5"] = stItem.ExtraData5;
                rowToAdd["ImageNormal"] = stItem.ImageNormalByte;
                rowToAdd["ImageError"] = stItem.ImageErrorByte;
                rowToAdd["ImageNoResponse"] = stItem.ImageNoResponseByte;
                rowToAdd["Caption"] = stItem.Caption;

                DT.Rows.Add(rowToAdd);

            }
            gdvOtherTemplates.DataSource = DT;
            gdvOtherTemplates.DataBind();
            Session["CTDT"] = DT;
        }

        private void createOtherDevice()
        {
            int numberToCreate = 0;
            try
            {
                if ((txtNumberOtherDevices.Text.Length > 0 & Information.IsNumeric(txtNumberOtherDevices.Text)))
                {
                    numberToCreate = Convert.ToInt32(txtNumberOtherDevices.Text);
                }
                //create the datatable for the camera sensors to be added.
                DataTable dt = new DataTable();
                DataTable dtCT = (DataTable)Session["CTDT"];
                int intSel = gdvOtherTemplates.SelectedIndex;

                dt.Columns.Add("Type", typeof(LiveMonitoring.IRemoteLib.OtherDeviceTemplate.DeviceType));
                dt.Columns.Add("IPAdress");
                dt.Columns.Add("Port");
                dt.Columns.Add("SerialPort");
                dt.Columns.Add("SerialSettings");
                dt.Columns.Add("LastReadDT");
                dt.Columns.Add("ExtraData");
                dt.Columns.Add("ExtraData1");
                dt.Columns.Add("ExtraData2");
                dt.Columns.Add("ExtraData3");
                dt.Columns.Add("ExtraData4");
                dt.Columns.Add("ExtraData5");
                dt.Columns.Add("ImageNormal");
                dt.Columns.Add("ImageError");
                dt.Columns.Add("ImageNoResponse");
                dt.Columns.Add("Caption");
                if (cboShowImport.Checked)
                {
                    Readfile();
                }

                LiveMonitoring.IRemoteLib.OtherDeviceTemplate.DeviceType myenum = new LiveMonitoring.IRemoteLib.OtherDeviceTemplate.DeviceType();
                for (int intX = 0; intX <= (numberToCreate - 1); intX++)
                {
                    var rowToAdd = dt.NewRow();
                    rowToAdd["Type"] = Enum.Parse(myenum.GetType(), dtCT.Rows[intSel]["Type"].ToString());
                    rowToAdd["IPAdress"] = dtCT.Rows[intSel]["IPAdress"];
                    rowToAdd["Port"] = dtCT.Rows[intSel]["Port"];
                    rowToAdd["SerialPort"] = dtCT.Rows[intSel]["SerialPort"];
                    rowToAdd["SerialSettings"] = dtCT.Rows[intSel]["SerialSettings"];
                    rowToAdd["LastReadDT"] = dtCT.Rows[intSel]["LastReadDT"];
                    rowToAdd["ExtraData"] = dtCT.Rows[intSel]["ExtraData"];
                    rowToAdd["ExtraData1"] = dtCT.Rows[intSel]["ExtraData1"];
                    rowToAdd["ExtraData2"] = dtCT.Rows[intSel]["ExtraData2"];
                    rowToAdd["ExtraData3"] = dtCT.Rows[intSel]["ExtraData3"];
                    rowToAdd["ExtraData4"] = dtCT.Rows[intSel]["ExtraData4"];
                    rowToAdd["ExtraData5"] = dtCT.Rows[intSel]["ExtraData5"];
                    rowToAdd["ImageNormal"] = dtCT.Rows[intSel]["ImageNormal"];
                    rowToAdd["ImageError"] = dtCT.Rows[intSel]["ImageError"];
                    rowToAdd["ImageNoResponse"] = dtCT.Rows[intSel]["ImageNoResponse"];


                    if (cboShowImport.Checked)
                    {
                        rowToAdd["Caption"] = SensorNameList[intX];
                    }
                    else
                    {
                        rowToAdd["Caption"] = dtCT.Rows[intSel]["Caption"];
                    }

                    dt.Rows.Add(rowToAdd);
                }
                gdvBulk.DataSource = dt;
                gdvBulk.DataBind();
                Session["DTBulk"] = dt;
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error: " + ex.Message;
                Trace.Write("CameraBulkCreation.createCameras.error: " + ex.Message);
            }
        }

        private void Bind()
        {
            gdvBulk.DataSource = (DataTable)Session["DTBulk"];
            gdvBulk.DataBind();
        }

        protected void gdvBulk_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvBulk.PageIndex = e.NewPageIndex;
            Bind();
        }

        protected void gdvBulk_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdvBulk.EditIndex = -1;
            Bind();
        }

        protected void gdvBulk_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdvBulk.EditIndex = e.NewEditIndex;
            Bind();
        }

        protected void gdvBulk_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable myDTNew = null;
            myDTNew = (DataTable)Session["DTBulk"];

            // Update the values.
            GridViewRow row = gdvBulk.Rows[e.RowIndex];

            myDTNew.Rows[row.DataItemIndex]["Type"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["IPAdress"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Port"] = ((TextBox)(row.Cells[3].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["SerialPort"] = ((TextBox)(row.Cells[4].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["SerialSettings"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["LastReadDT"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData1"] = ((TextBox)(row.Cells[8].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData2"] = ((TextBox)(row.Cells[9].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData3"] = ((TextBox)(row.Cells[10].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData4"] = ((TextBox)(row.Cells[11].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ExtraData5"] = ((TextBox)(row.Cells[12].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNormal"] = ((TextBox)(row.Cells[13].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageError"] = ((TextBox)(row.Cells[14].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNoResponse"] = ((TextBox)(row.Cells[15].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Caption"] = ((TextBox)(row.Cells[16].Controls[0])).Text;

            gdvBulk.EditIndex = -1;
            Session["DTBulk"] = myDTNew;
            Bind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<LiveMonitoring.IRemoteLib.OtherDevicesDetails> OtherdeviceDetailsList = new List<LiveMonitoring.IRemoteLib.OtherDevicesDetails>();
            try
            {
                foreach (GridViewRow row in gdvBulk.Rows)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails sd = new LiveMonitoring.IRemoteLib.OtherDevicesDetails();
                    sd.Type = (LiveMonitoring.IRemoteLib.OtherDevicesDetails.DeviceType)Convert.ToInt32(row.Cells[1].Text);
                    sd.IPAdress = row.Cells[2].Text;
                    sd.Port = Convert.ToInt32(row.Cells[3].Text);
                    sd.SerialPort = Convert.ToInt32(row.Cells[4].Text);
                    sd.SerialSettings = row.Cells[5].Text;
                    sd.LastReadDT = Convert.ToDateTime(row.Cells[6].Text);
                    sd.ExtraData = row.Cells[7].Text;
                    //sd.ExtraData1 = row.Cells[8].Text;
                    //sd.ExtraData2 = row.Cells[9].Text;
                    //sd.ExtraData3 = Convert.ToInt32(row.Cells[10].Text);
                    //sd.ExtraData4 = Convert.ToInt32(row.Cells[11].Text);
                    //.ExtraData5 = Convert.ToInt32(row.Cells[12].Text);
                    sd.ImageNormalByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[13].Text);
                    sd.ImageErrorByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[14].Text);
                    sd.ImageNoResponseByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[15].Text);
                    sd.Caption = row.Cells[16].Text;
                    // Add to list
                    OtherdeviceDetailsList.Add(sd);
                }

                //save the list of cameras
                LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();
                bool didSave = false;

                didSave = myrem.LiveMonServer.CreateBulkOtherDevice(OtherdeviceDetailsList);
                if (didSave)
                {
                    successMessage.Visible = true;
                    lblSuccess.Text = "Save was successful.";
                }
                else
                {
                    throw new Exception("Failed to save. Please try again.");
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        protected void cboShowImport_CheckedChanged(object sender, EventArgs e)
        {
            if (cboShowImport.Checked)
            {
                divImportRow.Visible = true;
            }
            else
            {
                divImportRow.Visible = false;
            }
        }

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

            try
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
                int rowCount = 11;
                string sensorName = "";
                //Get the rows.
                //0 index row is the headings
                for (int r = 1; r <= rowCount; r++)
                {
                    //Read the columns
                    Exceldoc1.Cell = convertToA1(r, 11);
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
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error: " + ex.Message;
            }
        }

        public void BulkCreation_OtherDeviceBulkCreation()
        {
            Load += Page_Load;
        }
    }
}