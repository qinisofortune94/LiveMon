
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
    public partial class BulkSNMPDevices : System.Web.UI.Page
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

                if (!IsPostBack)
                {
                    GetSNMPDeviceTemplates();

                    successMessage.Visible = false;
                    warningMessage.Visible = false;
                    errorMessage.Visible = false;

                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            createSNMPDevice();
        }


     

    /// <summary>
    /// Get all the SNMP device templates and populate the gridview.
    /// </summary>
    /// <remarks></remarks>
    private void GetSNMPDeviceTemplates()
        {
            List<LiveMonitoring.IRemoteLib.SNMPDeviceTemplate> STList = new List<LiveMonitoring.IRemoteLib.SNMPDeviceTemplate>();
            LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();

            STList = myrem.LiveMonServer.GetAllSNMPDeviceTemplates();

            // Convert the list to a dataset/datatable to use as the datasource for the gridview.
           // LiveMonitoring.IRemoteLib.SNMPDeviceTemplate stItem = new LiveMonitoring.IRemoteLib.SNMPDeviceTemplate();
            DataTable DT = new DataTable();

            // Create the columns/headers
            DT.Columns.Add("TemplateName");
            DT.Columns.Add("RemoteHost");
            DT.Columns.Add("RemotePort");
            DT.Columns.Add("Authentication", typeof(LiveMonitoring.IRemoteLib.SNMPDeviceTemplate.AuthProtocol));
            DT.Columns.Add("Community");
            DT.Columns.Add("LocalEngineId");
            DT.Columns.Add("LocalHost");
            DT.Columns.Add("LocalPort");
            DT.Columns.Add("RequestId");
            DT.Columns.Add("SNMPVersion", typeof(LiveMonitoring.IRemoteLib.SNMPDeviceTemplate.SNMPVer));
            DT.Columns.Add("Timeout");
            DT.Columns.Add("User");
            DT.Columns.Add("Password");
            DT.Columns.Add("Data1");
            DT.Columns.Add("Data2");
            DT.Columns.Add("Data3");
            DT.Columns.Add("ImageNormal");
            DT.Columns.Add("ImageError");
            DT.Columns.Add("ImageNoResponse");
            DT.Columns.Add("Caption");

            foreach (LiveMonitoring.IRemoteLib.SNMPDeviceTemplate stItem in STList)
            {
                dynamic rowToAdd = DT.NewRow();
                //TODO: check if there are more fields


                rowToAdd["TemplateName"] = stItem.templateName;
                rowToAdd["RemoteHost"] = stItem.RemoteHost;
                rowToAdd["RemotePort"] = stItem.RemotePort;
                rowToAdd["Authentication"] = stItem.AuthenticationProtocol;
                rowToAdd["Community"] = stItem.Community;
                rowToAdd["LocalEngineId"] = stItem.LocalEngineId;
                rowToAdd["LocalHost"] = stItem.LocalHost;
                rowToAdd["LocalPort"] = stItem.LocalPort;
                rowToAdd["RequestId"] = stItem.RequestId;
                rowToAdd["SNMPVersion"] = stItem.SNMPVersion;
                rowToAdd["Timeout"] = stItem.Timeout;
                rowToAdd["User"] = stItem.User;
                rowToAdd["Password"] = stItem.Password;
                rowToAdd["Data1"] = stItem.Data1;
                rowToAdd["Data2"] = stItem.Data2;
                rowToAdd["Data3"] = stItem.Data3;
                rowToAdd["ImageNormal"] = stItem.ImageNormalByte;
                rowToAdd["ImageError"] = stItem.ImageErrorByte;
                rowToAdd["ImageNoResponse"] = stItem.ImageNoResponseByte;
                rowToAdd["Caption"] = stItem.Caption;

                //Add
                DT.Rows.Add(rowToAdd);

            }


            gdvSNMPTemplates.DataSource = DT;
            gdvSNMPTemplates.DataBind();
            Session["CTDT"] = DT;
        }





        private void createSNMPDevice()
        {
            string prefix = "";
            int numberToCreate = 0;
            int ctID = 0;
            bool isComplete = false;
            try
            {
                if ((TxtBulkDevices.Text.Length > 0 & Information.IsNumeric(TxtBulkDevices.Text)))
                {
                    numberToCreate = Convert.ToInt32(TxtBulkDevices.Text);

                }
                //create the datatable for the camera sensors to be added.
                DataTable dt = new DataTable();
                DataTable dtCT = (DataTable)Session["CTDT"];
                int intSel = gdvSNMPTemplates.SelectedIndex;

                dt.Columns.Add("RemoteHost");
                dt.Columns.Add("RemotePort");
                dt.Columns.Add("Authentication", typeof(LiveMonitoring.IRemoteLib.SNMPDeviceTemplate.AuthProtocol));
                dt.Columns.Add("Community");
                dt.Columns.Add("LocalEngineId");
                dt.Columns.Add("LocalHost");
                dt.Columns.Add("LocalPort");
                dt.Columns.Add("RequestId");
                dt.Columns.Add("SNMPVersion", typeof(LiveMonitoring.IRemoteLib.SNMPDeviceTemplate.SNMPVer));
                dt.Columns.Add("Timeout");
                dt.Columns.Add("User");
                dt.Columns.Add("Password");
                dt.Columns.Add("Data1");
                dt.Columns.Add("Data2");
                dt.Columns.Add("Data3");
                dt.Columns.Add("ImageNormal");
                dt.Columns.Add("ImageError");
                dt.Columns.Add("ImageNoResponse");
                dt.Columns.Add("Caption");
                //if (ChkImport.Checked)
                //{
                    Readfile();

               // }

                for (int intX = 0; intX <= (numberToCreate - 1); intX++)
                {
                    dynamic rowToAdd = dt.NewRow();
                    rowToAdd["RemoteHost"] = dtCT.Rows[intSel]["RemoteHost"];
                    rowToAdd["RemotePort"] = dtCT.Rows[intSel]["RemotePort"];
                    rowToAdd["Authentication"] = dtCT.Rows[intSel]["Authentication"];
                    rowToAdd["Community"] = dtCT.Rows[intSel]["Community"];
                    rowToAdd["LocalEngineId"] = dtCT.Rows[intSel]["LocalEngineId"];
                    rowToAdd["LocalHost"] = dtCT.Rows[intSel]["LocalHost"];
                    rowToAdd["LocalPort"] = dtCT.Rows[intSel]["LocalPort"];
                    rowToAdd["RequestId"] = dtCT.Rows[intSel]["RequestId"];
                    rowToAdd["SNMPVersion"] = dtCT.Rows[intSel]["SNMPVersion"];
                    rowToAdd["Timeout"] = dtCT.Rows[intSel]["Timeout"];
                    rowToAdd["User"] = dtCT.Rows[intSel]["User"];
                    rowToAdd["Password"] = dtCT.Rows[intSel]["Password"];
                    rowToAdd["Data1"] = dtCT.Rows[intSel]["Data1"];
                    rowToAdd["Data2"] = dtCT.Rows[intSel]["Data2"];
                    rowToAdd["Data3"] = dtCT.Rows[intSel]["Data3"];
                    rowToAdd["ImageNormal"] = dtCT.Rows[intSel]["ImageNormal"];
                    rowToAdd["ImageError"] = dtCT.Rows[intSel]["ImageError"];
                    rowToAdd["ImageNoResponse"] = dtCT.Rows[intSel]["ImageNoResponse"];

                    if (ChkImport.Checked)
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
                //Create the sensors from the template.
                //Dim myrem As New LiveMonitoring.GlobalRemoteVars
                // isComplete = myrem.LiveMonServer.CreateBulkCamera(ctID, prefix, numberToCreate)

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error: " + ex.Message;

                //lblMessage.ForeColor = System.Drawing.Color.Red;
                //lblMessage.Text = "Error: " + ex.Message;
                //Trace.Write("CameraBulkCreation.createCameras.error: " + ex.Message);
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
            DataTable myDTNew = default(DataTable);
            myDTNew = (DataTable)Session["DTBulk"];

            // Update the values.
            GridViewRow row = gdvBulk.Rows[e.RowIndex];


            myDTNew.Rows[row.DataItemIndex]["RemoteHost"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["RemotePort"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Authentication"] = ((TextBox)(row.Cells[3].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Community"] = ((TextBox)(row.Cells[4].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["LocalEngineId"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["LocalHost"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["LocalPort"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["RequestId"] = ((TextBox)(row.Cells[8].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["SNMPVersion"] = ((TextBox)(row.Cells[9].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Timeout"] = ((TextBox)(row.Cells[10].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["User"] = ((TextBox)(row.Cells[11].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Password"] = ((TextBox)(row.Cells[12].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Data1"] = ((TextBox)(row.Cells[13].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Data2"] = ((TextBox)(row.Cells[14].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Data3"] = ((TextBox)(row.Cells[15].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNormal"] = ((TextBox)(row.Cells[16].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageError"] = ((TextBox)(row.Cells[17].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["ImageNoResponse"] = ((TextBox)(row.Cells[18].Controls[0])).Text;
            myDTNew.Rows[row.DataItemIndex]["Caption"] = ((TextBox)(row.Cells[19].Controls[0])).Text;

            //myDTNew.Rows(row.DataItemIndex)("RemoteHost") = e.NewValues(0)
            //myDTNew.Rows(row.DataItemIndex)("RemotePort") = e.NewValues(1)
            //myDTNew.Rows(row.DataItemIndex)("Authentication") = e.NewValues(2)
            //myDTNew.Rows(row.DataItemIndex)("Community") = e.NewValues(3)
            //myDTNew.Rows(row.DataItemIndex)("LocalEngineId") = e.NewValues(4)
            //myDTNew.Rows(row.DataItemIndex)("LocalHost") = e.NewValues(5)
            //myDTNew.Rows(row.DataItemIndex)("LocalPort") = e.NewValues(6)
            //myDTNew.Rows(row.DataItemIndex)("RequestId") = e.NewValues(7)
            //myDTNew.Rows(row.DataItemIndex)("SNMPVersion") = e.NewValues(8)
            //myDTNew.Rows(row.DataItemIndex)("Timeout") = e.NewValues(9)
            //myDTNew.Rows(row.DataItemIndex)("User") = e.NewValues(10)
            //myDTNew.Rows(row.DataItemIndex)("Password") = e.NewValues(11)
            //myDTNew.Rows(row.DataItemIndex)("Data1") = e.NewValues(12)
            //myDTNew.Rows(row.DataItemIndex)("Data2") = e.NewValues(13)
            //myDTNew.Rows(row.DataItemIndex)("Data3") = e.NewValues(14)
            //myDTNew.Rows(row.DataItemIndex)("ImageNormal") = System.Text.Encoding.UTF8.GetBytes(e.NewValues(15))
            //myDTNew.Rows(row.DataItemIndex)("ImageError") = System.Text.Encoding.UTF8.GetBytes(e.NewValues(16))
            //myDTNew.Rows(row.DataItemIndex)("ImageNoResponse") = System.Text.Encoding.UTF8.GetBytes(e.NewValues(17))
            //myDTNew.Rows(row.DataItemIndex)("Caption") = e.NewValues(18)

            gdvBulk.EditIndex = -1;
            Session["DTBulk"] = myDTNew;
            Bind();
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //create list of IP Device details.
            List<LiveMonitoring.IRemoteLib.SNMPManagerDetails> SNMPdeviceDetailsList = new List<LiveMonitoring.IRemoteLib.SNMPManagerDetails>();

            try
            {
                foreach (GridViewRow row in gdvBulk.Rows)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails sd = new LiveMonitoring.IRemoteLib.SNMPManagerDetails();
                    sd.RemoteHost = row.Cells[0].Text;
                    sd.RemotePort = Convert.ToInt32(row.Cells[1].Text);
                    sd.AuthenticationProtocol = (LiveMonitoring.IRemoteLib.SNMPManagerDetails.AuthProtocol)Convert.ToInt32((row.Cells[2].Text));
                    sd.Community = row.Cells[3].Text;
                    sd.LocalEngineId = Convert.ToInt32(row.Cells[4].Text);
                    sd.LocalHost = row.Cells[5].Text;
                    sd.LocalPort = Convert.ToInt32(row.Cells[6].Text);
                    sd.RequestId = Convert.ToInt32(row.Cells[7].Text);
                    sd.SNMPVersion = (LiveMonitoring.IRemoteLib.SNMPManagerDetails.SNMPVer)Convert.ToInt32(row.Cells[8].Text);
                    sd.Timeout = Convert.ToInt32(row.Cells[9].Text);
                    sd.User = row.Cells[10].Text;
                    sd.Password = row.Cells[11].Text;
                    sd.Data1 = row.Cells[12].Text;
                    sd.Data2 = row.Cells[13].Text;
                    sd.Data3 = row.Cells[14].Text;
                    sd.ImageNormalByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[15].Text);
                    sd.ImageErrorByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[16].Text);
                    sd.ImageNoResponseByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[17].Text);
                    sd.Caption = row.Cells[18].Text;
                    // Add to list
                    SNMPdeviceDetailsList.Add(sd);
                }

                //save the list of cameras
                LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();
                bool didSave = false;
                didSave = myrem.LiveMonServer.CreateBulkSNMPDevice(SNMPdeviceDetailsList);
                if (didSave)
                {

                    successMessage.Visible = true;
                    lblSucces.Text = "Creation was successful.";
                    //lblMessage.Text = "Creation was successful.";
                    //lblMessage.ForeColor = System.Drawing.Color.Green;
                    //lblMessage.Visible = true;


                }
                else
                {
                    throw new Exception("Creation Failed to save. Please try again.");
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
                //lblMessage.Visible = true;
                //lblMessage.Text = ex.Message;
          
                
            }
        }

        protected void ChkImport_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkImport.Checked)
            {
                tbrImportRow.Visible = true;
            }
            else
            {
                tbrImportRow.Visible = false;
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
                int rowCount = 15;
                string sensorName = "";
                //Get the rows.
                //0 index row is the headings
                for (int r = 1; r <= rowCount; r++)
                {
                    //Read the columns
                    Exceldoc1.Cell = convertToA1(r, 15);
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
                lblMessage.Text = "Error: " + ex.Message;

              
               
               
            }
        }
        public BulkSNMPDevices()
        {
            Load += Page_Load;
        }




    }

}

       
