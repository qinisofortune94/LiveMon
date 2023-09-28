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
    public partial class BulkCameras : System.Web.UI.Page
    {
        List<string> SensorNameList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                    GetCameraTemplates();
                    successMessage.Visible = false;
                    warningMessage.Visible = false;
                    errorMessage.Visible = false;
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
                
            }

         
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            createCameras();
        }


        

        private void GetCameraTemplates()
        {
            List<LiveMonitoring.IRemoteLib.CameraTemplate> STList = new List<LiveMonitoring.IRemoteLib.CameraTemplate>();
            LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();

            STList = myrem.LiveMonServer.GetAllCameraTemplates();

            // Convert the list to a dataset/datatable to use as the datasource for the gridview.
            //LiveMonitoring.IRemoteLib.CameraTemplate stItem = new LiveMonitoring.IRemoteLib.CameraTemplate();
            DataTable DT = new DataTable();

            // Create the columns/headers
            DT.Columns.Add("TemplateName");
            DT.Columns.Add("Type");
            DT.Columns.Add("IPAdress");
            DT.Columns.Add("Port");
            DT.Columns.Add("User");
            DT.Columns.Add("Password");
            DT.Columns.Add("ImageNormal");
            DT.Columns.Add("ImageError");
            DT.Columns.Add("ImageNoResponse");
            DT.Columns.Add("DTLastRead");
            DT.Columns.Add("Caption");
            DT.Columns.Add("MotionSensitivity");
            DT.Columns.Add("Field1");
            DT.Columns.Add("Field2");
            DT.Columns.Add("Field3");
            DT.Columns.Add("Field4");
            DT.Columns.Add("Field5");
            DT.Columns.Add("Field6");
            DT.Columns.Add("Field7");
            DT.Columns.Add("Field8");
            DT.Columns.Add("Field9");
            DT.Columns.Add("Field10");
            DT.Columns.Add("PreEventTime");
            DT.Columns.Add("PostEventTime");
            DT.Columns.Add("Events");
            DT.Columns.Add("EventRecording");
            DT.Columns.Add("ItemDetection");

            if (STList.Count > 0)
            {

                foreach (LiveMonitoring.IRemoteLib.CameraTemplate stItem in STList)
                {
                    dynamic rowToAdd = DT.NewRow();
                    //TODO: check if there are more fields
                    rowToAdd["TemplateName"] = stItem.templateName;
                    rowToAdd["Type"] = stItem.Type;
                    rowToAdd["IPAdress"] = stItem.IPAdress;
                    rowToAdd["Port"] = stItem.Port;
                    rowToAdd["User"] = stItem.User;
                    rowToAdd["Password"] = stItem.Password;
                    rowToAdd["ImageNormal"] = stItem.ImageNormalByte;
                    rowToAdd["ImageError"] = stItem.ImageErrorByte;
                    rowToAdd["ImageNoResponse"] = stItem.ImageNoResponseByte;
                    rowToAdd["DTLastRead"] = stItem.DTLastRead;
                    rowToAdd["Caption"] = stItem.Caption;
                    rowToAdd["MotionSensitivity"] = stItem.MotionSensitivity;
                    rowToAdd["Field1"] = stItem.Field1;
                    rowToAdd["Field2"] = stItem.Field2;
                    rowToAdd["Field3"] = stItem.Field3;
                    rowToAdd["Field4"] = stItem.Field4;
                    rowToAdd["Field5"] = stItem.Field5;
                    rowToAdd["Field6"] = stItem.Field6;
                    rowToAdd["Field7"] = stItem.Field7;
                    rowToAdd["Field8"] = stItem.Field8;
                    rowToAdd["Field9"] = stItem.Field9;
                    rowToAdd["Field10"] = stItem.Field10;
                    rowToAdd["PreEventTime"] = stItem.PreEventTime;
                    rowToAdd["PostEventTime"] = stItem.PostEventTime;
                    rowToAdd["Events"] = stItem.Events;
                    rowToAdd["EventRecording"] = stItem.EventRecord;
                    rowToAdd["ItemDetection"] = stItem.ItemDetection;
                    DT.Rows.Add(rowToAdd);
                }
            }
            gdvCameraTemplates.DataSource = DT;
            gdvCameraTemplates.DataBind();
            Session["CTDT"] = DT;
        }

        private void createCameras()
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
                int intSel =gdvCameraTemplates.SelectedIndex;

                dt.Columns.Add("Type");
                dt.Columns.Add("IPAdress");
                dt.Columns.Add("Port");
                dt.Columns.Add("User");
                dt.Columns.Add("Password");
                dt.Columns.Add("ImageNormal");
                dt.Columns.Add("ImageError");
                dt.Columns.Add("ImageNoResponse");
                dt.Columns.Add("DTLastRead");
                dt.Columns.Add("Caption");
                dt.Columns.Add("MotionSensitivity");
                dt.Columns.Add("Field1");
                dt.Columns.Add("Field2");
                dt.Columns.Add("Field3");
                dt.Columns.Add("Field4");
                dt.Columns.Add("Field5");
                dt.Columns.Add("Field6");
                dt.Columns.Add("Field7");
                dt.Columns.Add("Field8");
                dt.Columns.Add("Field9");
                dt.Columns.Add("Field10");
                dt.Columns.Add("PreEventTime");
                dt.Columns.Add("PostEventTime");
                dt.Columns.Add("Events");
                dt.Columns.Add("EventRecording");
                dt.Columns.Add("ItemDetection");
               // if (ChkImport.Checked)
               // {
                    //read file for caption names
                    Readfile();
               // }
                for (int intX = 0; intX <= (numberToCreate - 1); intX++)
                {
                    dynamic rowToAdd = dt.NewRow();
                    rowToAdd["Type"] = dtCT.Rows[intSel]["Type"];
                    rowToAdd["IPAdress"] = dtCT.Rows[intSel]["IPAdress"];
                    rowToAdd["Port"] = dtCT.Rows[intSel]["Port"];
                    rowToAdd["User"] = dtCT.Rows[intSel]["User"];
                    rowToAdd["Password"] = dtCT.Rows[intSel]["Password"];
                    rowToAdd["ImageNormal"] = dtCT.Rows[intSel]["ImageNormal"];
                    rowToAdd["ImageError"] = dtCT.Rows[intSel]["ImageError"];
                    rowToAdd["ImageNoResponse"] = dtCT.Rows[intSel]["ImageNoResponse"];
                    rowToAdd["DTLastRead"] = dtCT.Rows[intSel]["DTLastRead"];

                    if ((SensorNameList[intX] == null) == false & ChkImport.Checked == true)
                    {
                        rowToAdd["Caption"] = SensorNameList[intX];

                    }
                    else
                    {
                        rowToAdd["Caption"] = dtCT.Rows[intSel]["Caption"];
                    }





                    rowToAdd["MotionSensitivity"] = dtCT.Rows[intSel]["MotionSensitivity"];
                    rowToAdd["Field1"] = dtCT.Rows[intSel]["Field1"];
                    rowToAdd["Field2"] = dtCT.Rows[intSel]["Field2"];
                    rowToAdd["Field3"] = dtCT.Rows[intSel]["Field3"];
                    rowToAdd["Field4"] = dtCT.Rows[intSel]["Field4"];
                    rowToAdd["Field5"] = dtCT.Rows[intSel]["Field5"];
                    rowToAdd["Field6"] = dtCT.Rows[intSel]["Field6"];
                    rowToAdd["Field7"] = dtCT.Rows[intSel]["Field7"];
                    rowToAdd["Field8"] = dtCT.Rows[intSel]["Field8"];
                    rowToAdd["Field9"] = dtCT.Rows[intSel]["Field9"];
                    rowToAdd["Field10"] = dtCT.Rows[intSel]["Field10"];
                    rowToAdd["PreEventTime"] = dtCT.Rows[intSel]["PreEventTime"];
                    rowToAdd["PostEventTime"] = dtCT.Rows[intSel]["PostEventTime"];
                    rowToAdd["Events"] = dtCT.Rows[intSel]["Events"];
                    rowToAdd["EventRecording"] = dtCT.Rows[intSel]["EventRecording"];
                    rowToAdd["ItemDetection"] = dtCT.Rows[intSel]["ItemDetection"];

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
                Trace.Write("CameraBulkCreation.createCameras.error: " + ex.Message);
            }



        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
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


        private void Readfile1()
        {
            int rowCount = 0;
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


        public BulkCameras()
        {
            Load += Page_Load;
        }

      

        protected void Submit_Click(object sender, EventArgs e)
        {

            List<LiveMonitoring.IRemoteLib.CameraDetails> CameraDetailsList = new List<LiveMonitoring.IRemoteLib.CameraDetails>();

            try
            {
                foreach (GridViewRow row in gdvBulk.Rows)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails sd = new LiveMonitoring.IRemoteLib.CameraDetails();
                    sd.Type = Convert.ToInt32(row.Cells[0].Text);
                    sd.IPAdress = row.Cells[1].Text;
                    sd.Port = Convert.ToInt32(row.Cells[2].Text);
                    sd.User = row.Cells[3].Text;
                    sd.Password = row.Cells[4].Text;
                    sd.ImageNormalByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[5].Text);
                    sd.ImageErrorByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[6].Text);
                    sd.ImageNoResponseByte = System.Text.Encoding.UTF8.GetBytes(row.Cells[7].Text);
                    //sd.DTLastRead = Convert.ToDateTime(row.Cells[8].Text);
                    sd.Caption = row.Cells[9].Text;
                    sd.MotionSensitivity = Convert.ToInt32(row.Cells[10].Text);
                    //sd.Field1 = row.Cells[12].Text;
                    //sd.Field2 = row.Cells[13].Text;
                    //sd.Field3 = row.Cells[14].Text;
                    //sd.Field4 = row.Cells[15].Text;
                    //sd.Field5 = row.Cells[16].Text;
                    //sd.Field6 = row.Cells[17].Text;
                    //sd.Field7 = row.Cells[18].Text;
                    //sd.Field8 = row.Cells[19].Text;
                    //sd.Field9 = row.Cells[20].Text;
                    //sd.Field10 = row.Cells[21].Text;
                    sd.PreEventTime = Convert.ToInt32(row.Cells[21].Text);
                    sd.PostEventTime = Convert.ToInt32(row.Cells[22].Text);
                    sd.Events = Convert.ToInt32(row.Cells[23].Text);
                    sd.EventRecord = Convert.ToBoolean(row.Cells[24].Text);
                    sd.ItemDetection = Convert.ToBoolean(row.Cells[25].Text);

                    // Add to list
                    CameraDetailsList.Add(sd);
                }

                //save the list of cameras
                LiveMonitoring.GlobalRemoteVars myrem = new LiveMonitoring.GlobalRemoteVars();
                bool didSave = false;
                didSave = myrem.LiveMonServer.CreateBulkCamera(CameraDetailsList);

                if (didSave)
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "Creation was sucessful";
                }
                else
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Creation failed";
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Creation failed. Error: " + ex.Message;
            }
        }
    }

}