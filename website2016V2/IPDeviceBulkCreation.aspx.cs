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
    public partial class IPDeviceBulkCreation : System.Web.UI.Page
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
                    GetIPDeviceTemplates();
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
                GetIPDeviceTemplates();
            }
            if (gdvIPDevicesTemplates.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gdvIPDevicesTemplates.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gdvIPDevicesTemplates.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gdvIPDevicesTemplates.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gdvIPDevicesTemplates.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;

            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;
            }

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            createIPDevice();
        }

        private void GetIPDeviceTemplates()
        {
            List<LiveMonitoring.IRemoteLib.IPDeviceTemplate> STList = new List<LiveMonitoring.IRemoteLib.IPDeviceTemplate>();
            LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();

            STList = myrem.LiveMonServer.GetAllIPDeviceTemplates();
            DataTable DT = new DataTable();

            // Create the columns/headers
            DT.Columns.Add("IPDeviceTemplateID");
            DT.Columns.Add("TemplateName");
            DT.Columns.Add("Type", typeof(LiveMonitoring.IRemoteLib.IPDeviceTemplate.DeviceType));
            DT.Columns.Add("IPAdress");
            DT.Columns.Add("Port");
            DT.Columns.Add("Data1");
            DT.Columns.Add("Data2");
            DT.Columns.Add("Data3");
            DT.Columns.Add("ImageNormal");
            DT.Columns.Add("ImageError");
            DT.Columns.Add("ImageNoResponse");
            DT.Columns.Add("DTLastRead");
            DT.Columns.Add("Caption");

            foreach (LiveMonitoring.IRemoteLib.IPDeviceTemplate stItem in STList)
            {
                var rowToAdd = DT.NewRow();
                //TODO: check if there are more fields

                rowToAdd["IPDeviceTemplateID"] = stItem.IPDeviceTemplateID;
                rowToAdd["TemplateName"] = stItem.templateName;
                rowToAdd["Type"] = stItem.Type;
                rowToAdd["IPAdress"] = stItem.IPAdress;
                rowToAdd["Port"] = stItem.Port;
                rowToAdd["Data1"] = stItem.Data1;
                rowToAdd["Data2"] = stItem.Data2;
                rowToAdd["Data3"] = stItem.Data3;
                rowToAdd["ImageNormal"] = stItem.ImageNormalByte;
                rowToAdd["ImageError"] = stItem.ImageErrorByte;
                rowToAdd["ImageNoResponse"] = stItem.ImageNoResponseByte;
                rowToAdd["DTLastRead"] = stItem.DTLastRead;
                rowToAdd["Caption"] = stItem.Caption;

                //Add
                DT.Rows.Add(rowToAdd);

            }

            gdvIPDevicesTemplates.DataSource = DT;
            gdvIPDevicesTemplates.DataBind();
            Session["CTDT"] = DT;
        }

        private void createIPDevice()
        {
            int numberToCreate = 0;
           
            try
            {
                if ((txtNumberIPDevices.Text.Length > 0 & Information.IsNumeric(txtNumberIPDevices.Text)))
                {
                    numberToCreate = Convert.ToInt32(txtNumberIPDevices.Text);
                }

                DataTable dt = new DataTable();
                DataTable dtCT = (DataTable)Session["CTDT"];
                int intSel = gdvIPDevicesTemplates.SelectedIndex;
                dt.Columns.Add("Type", typeof(LiveMonitoring.IRemoteLib.IPDeviceTemplate.DeviceType));
                dt.Columns.Add("IPAdress");
                dt.Columns.Add("Port");
                dt.Columns.Add("Data1");
                dt.Columns.Add("Data2");
                dt.Columns.Add("Data3");
                dt.Columns.Add("ImageNormal");
                dt.Columns.Add("ImageError");
                dt.Columns.Add("ImageNoResponse");
                dt.Columns.Add("DTLastRead");
                dt.Columns.Add("Caption");
                LiveMonitoring.IRemoteLib.IPDeviceTemplate.DeviceType myenum = new LiveMonitoring.IRemoteLib.IPDeviceTemplate.DeviceType();
                if (cboShowImport.Checked)
                {
                    Readfile();
                }
                for (int intX = 0; intX <= (numberToCreate - 1); intX++)
                {
                    var rowToAdd = dt.NewRow();
                    rowToAdd["Type"] = Enum.Parse(myenum.GetType(), dtCT.Rows[intSel]["Type"].ToString());
                    rowToAdd["IPAdress"] = dtCT.Rows[intSel]["IPAdress"];
                    rowToAdd["Port"] = dtCT.Rows[intSel]["Port"];
                    rowToAdd["Data1"] = dtCT.Rows[intSel]["Data1"];
                    rowToAdd["Data2"] = dtCT.Rows[intSel]["Data2"];
                    rowToAdd["Data3"] = dtCT.Rows[intSel]["Data3"];
                    rowToAdd["ImageNormal"] = dtCT.Rows[intSel]["ImageNormal"];
                    rowToAdd["ImageError"] = dtCT.Rows[intSel]["ImageError"];
                    rowToAdd["ImageNoResponse"] = dtCT.Rows[intSel]["ImageNoResponse"];
                    rowToAdd["DTLastRead"] = dtCT.Rows[intSel]["DTLastRead"];

                    //If IsNothing(SensorNameList.IndexOf(intX)) = False Then
                    if (cboShowImport.Checked == true)
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

            GridViewRow row = gdvBulk.Rows[e.RowIndex];


            myDTNew.Rows[row.DataItemIndex]["Type"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["IPAdress"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Port"] = ((TextBox)(row.Cells[3].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Data1"] = ((TextBox)(row.Cells[4].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Data2"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Data3"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNormal"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageError"] = ((TextBox)(row.Cells[8].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNoResponse"] = ((TextBox)(row.Cells[9].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["DTLastRead"] = ((TextBox)(row.Cells[10].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Caption"] = ((TextBox)(row.Cells[11].Controls[0])).Text;

            gdvBulk.EditIndex = -1;
            Session["DTBulk"] = myDTNew;
            Bind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //create list of IP Device details.
            List<LiveMonitoring.IRemoteLib.IPDevicesDetails> IpdeviceDetailsList = new List<LiveMonitoring.IRemoteLib.IPDevicesDetails>();

            try
            {
                foreach (GridViewRow row in gdvBulk.Rows)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails sd = new LiveMonitoring.IRemoteLib.IPDevicesDetails();
                    sd.Type = (LiveMonitoring.IRemoteLib.IPDevicesDetails.DeviceType)Convert.ToInt32(row.Cells[1].Text);
                    sd.IPAdress = row.Cells[2].Text;
                    sd.Port = Convert.ToInt32(row.Cells[3].Text);
                    sd.Data1 = row.Cells[4].Text;
                    sd.Data2 = row.Cells[5].Text;
                    sd.Data3 = row.Cells[6].Text;
                    sd.ImageNormalByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[7].Text);
                    sd.ImageErrorByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[8].Text);
                    sd.ImageNoResponseByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[9].Text);
                    sd.DTLastRead = Convert.ToDateTime(row.Cells[10].Text);
                    sd.Caption = row.Cells[11].Text;

                    // Add to list
                    IpdeviceDetailsList.Add(sd);
                }

                //save the list of cameras
                LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();
                bool didSave = false;
                didSave = myrem.LiveMonServer.CreateBulkIPDevice(IpdeviceDetailsList);
                if (didSave)
                {
                    successMessage.Visible = true;
                    lblSuccess.Text = "Creation was successful.";
                }
                else
                {
                    throw new Exception("Creation failed. Please try again.");
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
            if ((column / 7) == 0)
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

        public void BulkCreation_IPDeviceBulkCreation()
        {
            Load += Page_Load;
        }
    }
}