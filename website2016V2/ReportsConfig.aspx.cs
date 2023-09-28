using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace website2016V2
{
    partial class ReportsConfig : System.Web.UI.Page
    {
        int intSelectedReportId;
        LiveMonitoring.DataAccess myDA = new LiveMonitoring.DataAccess();
        DataTable myDT;

        DataSet myDS;

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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                if (!IsPostBack)
                {
                    setVisibilties();
                    getAllReports();
                    getReportTypes();
                    getAllSensors();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (divAddFunctionality.Visible == true)
            {
                btnAdd.Text = "Show Add";
                divAddFunctionality.Visible = false;
            }
            else
            {
                btnAdd.Text = "Hide Add";
                divAddFunctionality.Visible = true;
            }
        }

        /// <summary>
        /// set the visibility of the controls.
        /// </summary>
        /// <remarks></remarks>
        public void setVisibilties()
        {
            btnAdd.Visible = true;
            btnRemove.Visible = false;
            divAddFunctionality.Visible = false;
            divEditFunction.Visible = false;
            divControls.Visible = true;
            btnShowScheduling.Visible = false;
            btnShowEdit.Visible = false;
        }

        protected void gvReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            intSelectedReportId = Convert.ToInt32(gvReports.SelectedRow.Cells[1].Text.ToString());
            btnRemove.Visible = true;
            //Initialize the report Id variable.
            btnShowScheduling.Visible = true;
            btnShowEdit.Visible = true;

            //Get the scheduling information for the selected report.
            getReportSchedulingInformation();
            //Get the edit information for the selected report.
            txtEditReportName.Text = gvReports.SelectedRow.Cells[2].Text;
            txtEditDescription.Text = gvReports.SelectedRow.Cells[3].Text.ToUpper();   
            //ddlEditReportType.SelectedItem.Text = gvReports.SelectedRow.Cells[5].Text.ToUpper();
            //getReportInformation();
        }

        /// <summary>
        /// Get all reports and set the datasource of the gvReports gridview.
        /// </summary>
        /// <remarks></remarks>
        private void getAllReports()
        {
            myDT = new DataTable();
            myDS = new DataSet();
            try
            {
                myDS = myDA.ExecCmdQueryNoParamsDS("ReportsSelectAll");
                myDT = myDS.Tables[0];
                gvReports.DataSource = myDT;
                gvReports.DataBind();
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.getAllReports: Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Get all the report types and populate the lstReportTypes
        /// </summary>
        /// <remarks></remarks>
        private void getReportTypes()
        {
            string[] reportTypeNames = null;
            int[] reportTypeValues = null;
            try
            {
                reportTypeNames = (string[])Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.IPMonReportType));
                reportTypeValues = (int[])Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.IPMonReportType));
                for (int intx = 0; intx <= reportTypeNames.Length - 1; intx++)
                {
                    ListItem newListItem = new ListItem();
                    newListItem.Value = Convert.ToString(reportTypeValues[intx]);
                    newListItem.Text = reportTypeNames[intx];
                    ddlReportType.Items.Add(newListItem);
                    ddlEditReportType.Items.Add(newListItem);
                }
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.getReportTypes: Error: " + ex.Message);
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            removeAReport();
            getAllReports();
        }

        protected void btnAddReport_Click(object sender, EventArgs e)
        {
            addAReport();
            getAllReports();
        }

        /// <summary>
        /// Add a report.
        /// </summary>
        /// <remarks></remarks>
        public void addAReport()
        {
            System.Data.SqlClient.SqlParameter[] myParams = new System.Data.SqlClient.SqlParameter[3];
            try
            {
                myParams[0] = new System.Data.SqlClient.SqlParameter();
                myParams[0].ParameterName = "@NAME";
                myParams[0].Value = txtReportName.Text;

                myParams[1] = new System.Data.SqlClient.SqlParameter();
                myParams[1].ParameterName = "@DESCRIPTION";
                myParams[1].Value = txtDescription.Text;

                myParams[2] = new System.Data.SqlClient.SqlParameter();
                myParams[2].ParameterName = "@TYPE";
                myParams[2].Value = Convert.ToInt32(ddlReportType.SelectedValue);

                myDA.ExecCmdQueryParams("spReportsInsert", myParams);
                getAllReports();
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.addAReport: Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Remove a report.
        /// </summary>
        /// <remarks></remarks>
        private void removeAReport()
        {
            System.Data.SqlClient.SqlParameter[] myParams = new System.Data.SqlClient.SqlParameter[1];
            try
            {
                myParams[0] = new System.Data.SqlClient.SqlParameter();
                myParams[0].ParameterName = "@ID";
                myParams[0].Value = Convert.ToInt32(gvReports.SelectedRow.Cells[1].Text);
                myDA.ExecCmdQueryParams("spReportsDelete", myParams);
                getAllReports();
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.addAReport: Error: " + ex.Message);
            }
        }

        protected void btnShowEdit_Click(object sender, EventArgs e)
        {
            if (divEditFunction.Visible)
            {
                btnShowEdit.Text = "Show Edit";
                divEditFunction.Visible = false;
            }
            else
            {
                btnShowEdit.Text = "Hide Edit";
                divEditFunction.Visible = true;
            }
        }

        protected void btnShowScheduling_Click(object sender, EventArgs e)
        {
            if (divScheduling.Visible)
            {
                btnShowScheduling.Text = "Show Scheduling";
                divScheduling.Visible = false;
            }
            else
            {
                btnShowScheduling.Text = "Hide Scheduling";
                divScheduling.Visible = true;
            }
        }

        protected void btnEditReport_Click(object sender, EventArgs e)
        {
            // Save the changes made to the selected report information.
            System.Data.SqlClient.SqlParameter[] myParams = new System.Data.SqlClient.SqlParameter[4];
            int intReportId = 0;
            intReportId = Convert.ToInt32(gvReports.SelectedRow.Cells[1].Text);
            try
            {
                myParams[0] = new System.Data.SqlClient.SqlParameter();
                myParams[0].ParameterName = "@NAME";
                myParams[0].Value = txtEditReportName.Text;

                myParams[1] = new System.Data.SqlClient.SqlParameter();
                myParams[1].ParameterName = "@DESCRIPTION";
                myParams[1].Value = txtEditDescription.Text;

                myParams[2] = new System.Data.SqlClient.SqlParameter();
                myParams[2].ParameterName = "@TYPE";
                myParams[2].Value = Convert.ToInt32(ddlEditReportType.SelectedValue);

                myParams[3] = new System.Data.SqlClient.SqlParameter();
                myParams[3].ParameterName = "@REPORTID";
                myParams[3].Value = intReportId;

                myDA.ExecCmdQueryParams("[spReportsEdit]", myParams);
                getAllReports();
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.addAReport: Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Get the information of the selected report.
        /// 
        /// </summary>
        /// <remarks></remarks>

        private void getReportInformation()
        {
            int intReportId = 0;
            intReportId = Convert.ToInt32(gvReports.SelectedRow.Cells[1].Text);
            try
            {
                myDT = new DataTable();
                myDS = new DataSet();
                System.Data.SqlClient.SqlParameter[] sqlParams = new System.Data.SqlClient.SqlParameter[1];
                sqlParams[0] = new System.Data.SqlClient.SqlParameter();
                sqlParams[0].Value = intReportId;
                sqlParams[0].ParameterName = "@REPORTID";
                try
                {
                    myDS = myDA.ExecCmdQueryParamsDS("ReportsSelectByReportId", sqlParams);
                    myDT = myDS.Tables[0];

                    foreach (DataRow myrow in myDT.Rows)
                    {
                        txtEditDescription.Text = myrow["ReportDescription"].ToString();
                        txtEditReportName.Text = myrow["ReportName"].ToString();
                        ddlEditReportType.SelectedValue = myrow["ReportType"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Trace.Write("ReportsConfig.getReportInformation: Error: " + ex.Message);
                }

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Get the scheduling information for the selected report.
        /// 
        /// </summary>
        /// <remarks></remarks>
        private void getReportSchedulingInformation()
        {
            try
            {
                //Dim intReportId As Integer = 0
                //intReportId = CInt(gvReports.SelectedRow.Cells(1).Text)
                //intSelectedReportId = intReportId
                myDT = new DataTable();
                myDS = new DataSet();
                System.Data.SqlClient.SqlParameter[] sqlParams = new System.Data.SqlClient.SqlParameter[1];
                sqlParams[0] = new System.Data.SqlClient.SqlParameter();
                sqlParams[0].Value = intSelectedReportId;
                sqlParams[0].ParameterName = "@ReportID";
                myDS = myDA.ExecCmdQueryParamsDS("ReportsSelectScheduleByID", sqlParams);

                if (myDS.Tables.Count != 0)
                {
                    //Data was returned.
                    myDT = myDS.Tables[0];                    
                    gvReportSchedule.DataSource = myDT;                    
                    gvReportSchedule.DataBind();
                }
                else
                {
                    gvReportSchedule.DataSource = null;
                    gvReportSchedule.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.getReportSchedulingInformation: error: " + ex.Message.ToString());
            }
        }

        protected void btnShowAddSchedule_Click(object sender, EventArgs e)
        {
            if (divSchedulingAdd.Visible)
            {
                divSchedulingAdd.Visible = false;
                btnShowAddSchedule.Text = "Show Add Scheduling";
            }
            else
            {
                divSchedulingAdd.Visible = true;
                btnShowAddSchedule.Text = "Hide Add Scheduling";
            }
        }

        protected void btnSchedulingAddNew_Click(object sender, EventArgs e)
        {
            addReportSchedule();
        }

        /// <summary>
        /// Create a new report schedule.
        /// </summary>
        /// <remarks></remarks>
        public void addReportSchedule()
        {
            try
            {
                myDA = new LiveMonitoring.DataAccess();
                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[8];
                int intID = Convert.ToInt32(intSelectedReportId);
                //The selected report Id.

                //@REPORTID,
                MyParams[0] = new System.Data.SqlClient.SqlParameter();
                MyParams[0].Value = intID;
                MyParams[0].ParameterName = "@REPORTID";

                //@HOURSDATABACK
                MyParams[1] = new System.Data.SqlClient.SqlParameter();
                MyParams[1].Value = txtHoursDataBack.Text;
                MyParams[1].ParameterName = "@HOURSDATABACK";

                //@REPORTRUNDAYS
                MyParams[2] = new System.Data.SqlClient.SqlParameter();
                MyParams[2].Value = txtReportRunDays.Text;
                MyParams[2].ParameterName = "@REPORTRUNDAYS";

                //@REPORTRUNMONTHLY
                MyParams[3] = new System.Data.SqlClient.SqlParameter();
                MyParams[3].Value = txtReportRunMonthly.Text;
                MyParams[3].ParameterName = "@REPORTRUNMONTHLY";

                //@TRIGGERHOUR
                MyParams[4] = new System.Data.SqlClient.SqlParameter();
                MyParams[4].Value = txtTriggerHour.Text;
                MyParams[4].ParameterName = "@TRIGGERHOUR";

                //@REPORTDATAROWS
                MyParams[5] = new System.Data.SqlClient.SqlParameter();
                MyParams[5].Value = txtReportDataRows.Text;
                MyParams[5].ParameterName = "@REPORTDATAROWS";

                //@REPORTDATAPERIOD
                MyParams[6] = new System.Data.SqlClient.SqlParameter();
                MyParams[6].Value = txtReportDataPeriod.Text;
                MyParams[6].ParameterName = "@REPORTDATAPERIOD";

                //@REPORTTARRIF
                MyParams[7] = new System.Data.SqlClient.SqlParameter();
                MyParams[7].Value = txtTarrif.Text;
                MyParams[7].ParameterName = "@REPORTTARRIF";

                //@MONTHSDATABACK
                MyParams[8] = new System.Data.SqlClient.SqlParameter();
                MyParams[8].Value = txtMonthsDataBack.Text;
                MyParams[8].ParameterName = "@MONTHSDATABACK";

                //@REPORTNOSENSORSPERREPORT,
                MyParams[9] = new System.Data.SqlClient.SqlParameter();
                MyParams[9].Value = txtReportNoSensorsPerReport.Text;
                MyParams[9].ParameterName = "@REPORTNOSENSORSPERREPORT";

                //@REPORTSUMARYHOURS
                MyParams[10] = new System.Data.SqlClient.SqlParameter();
                MyParams[10].Value = txtReportSummaryHours.Text;
                MyParams[10].ParameterName = "@REPORTSUMARYHOURS";

                //@REPORTTRENDINGHOURS
                MyParams[11] = new System.Data.SqlClient.SqlParameter();
                MyParams[11].Value = txtReportTrendingHours.Text;
                MyParams[11].ParameterName = "@REPORTTRENDINGHOURS";

                //@REPORTAVERAGINGDAYS
                MyParams[12] = new System.Data.SqlClient.SqlParameter();
                MyParams[12].Value = txtReportAveragingDays.Text;
                MyParams[12].ParameterName = "@REPORTAVERAGINGDAYS";

                //@REPORTEXTRADATA
                MyParams[13] = new System.Data.SqlClient.SqlParameter();
                MyParams[13].Value = txtReportExtraData.Text;
                MyParams[13].ParameterName = "@REPORTEXTRADATA";

                //@REPORTEXTRADATA1
                MyParams[14] = new System.Data.SqlClient.SqlParameter();
                MyParams[14].Value = txtReportExtraData1.Text;
                MyParams[14].ParameterName = "@REPORTEXTRADATA1";

                //@REPORTEXTRAVAL
                MyParams[15] = new System.Data.SqlClient.SqlParameter();
                MyParams[15].Value = txtReportExtraVal.Text;
                MyParams[15].ParameterName = "@REPORTEXTRAVAL";

                //@REPORTEXTRAVAL1
                MyParams[16] = new System.Data.SqlClient.SqlParameter();
                MyParams[16].Value = txtReportExtraVal1.Text;
                MyParams[16].ParameterName = "@REPORTEXTRAVAL1";
                myDA.ExecCmdQueryParamsDS("spReportScheduleInsert", MyParams);

            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.addReportSchedule: error: " + ex.Message.ToString());
            }
        }

        protected void btnShowAddSensor_Click(object sender, EventArgs e)
        {
            if (divSensorFunction.Visible)
            {
                divSensorFunction.Visible = false;
                btnShowAddSensor.Text = "Show Add Sensor";
            }
            else
            {
                divSensorFunction.Visible = true;
                btnShowAddSensor.Text = "Hide Add Sensor";
            }
        }

        protected void btnAddSensor_Click(object sender, EventArgs e)
        {
            addSensorToSchedule();

        }

        /// <summary>
        /// Add a new row of information to the ReportScheduleSensors table. 
        /// </summary>
        /// <remarks></remarks>
        private void addSensorToSchedule()
        {
            int intSensorID = 0;
            int intReportScheduleID = 0;
            try
            {
                intSensorID = Convert.ToInt32(ddlSensors.SelectedValue);
                intReportScheduleID = Convert.ToInt32(gvReportSchedule.SelectedRow.Cells[1].Text.ToString());

                System.Data.SqlClient.SqlParameter[] myParams = new System.Data.SqlClient.SqlParameter[2];
                myParams[0] = new System.Data.SqlClient.SqlParameter();
                myParams[0].ParameterName = "@REPORTSCHEDULEID";
                myParams[0].Value = intReportScheduleID;

                myParams[1] = new System.Data.SqlClient.SqlParameter();
                myParams[1].ParameterName = "@SENSORID";
                myParams[1].Value = intSensorID;

                myDA.ExecCmdQueryParams("REPORTSCHEDULESENSORLINK.spInsertNew", myParams);

            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.addSensorToSchedule: Error: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get all the sensors for the report scheduling.
        /// 
        /// </summary>
        /// <remarks></remarks>
        private void getAllSensors()
        {
            myDT = new DataTable();
            myDS = new DataSet();
            try
            {
                myDS = myDA.ExecCmdQueryNoParamsDS("sensor_select_all");
                if ((myDS != null))
                {
                    if (myDS.Tables.Count > 0)
                    {
                        myDT = myDS.Tables[0];
                        if (myDT.Rows.Count > 0)
                        {
                            foreach (DataRow myRow in myDT.Rows)
                            {
                                ListItem newItem = new ListItem();
                                if ((myRow["ID"].ToString() != null))
                                {
                                    newItem.Value = myRow["ID"].ToString();
                                }
                                if ((myRow["Caption"].ToString() != null))
                                {
                                    newItem.Text = myRow["Caption"].ToString();
                                }
                                if ((newItem.Value != null) & (newItem.Text != null))
                                {
                                    ddlSensors.Items.Add(newItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.getAllSensors: Error: " + ex.Message.ToString());
            }
        }

        protected void btnShowSensorsForSchedule_Click(object sender, EventArgs e)
        {
            if (divSensorsForSchedule.Visible)
            {
                divSensorsForSchedule.Visible = false;
                btnShowSensorsForSchedule.Text = "Show Sensors for Schedule";
            }
            else
            {
                getSensorsForSchedule();
                divSensorsForSchedule.Visible = true;
                btnShowSensorsForSchedule.Text = "Hide Sensors for Schedule";
            }
        }

        protected void gvSensorSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            getSensorsForSchedule();
        }

        /// <summary>
        /// Get the sensor information for the selected report schedules.
        /// </summary>
        /// <remarks></remarks>
        private void getSensorsForSchedule()
        {
            int intReportScheduleId = 0;
            intReportScheduleId = Convert.ToInt32(gvReportSchedule.SelectedRow.Cells[1].Text);
            myDT = new DataTable();
            
            myDS = new DataSet();
            try
            {
                System.Data.SqlClient.SqlParameter[] sqlparams = new System.Data.SqlClient.SqlParameter[1];
                sqlparams[0] = new System.Data.SqlClient.SqlParameter();
                sqlparams[0].ParameterName = "@REPORTSCHEDULEID";
                sqlparams[0].Value = intReportScheduleId;
                LiveMonitoring.IRemoteLib.ReportsSchedulesDef sd = new LiveMonitoring.IRemoteLib.ReportsSchedulesDef();

                myDS = myDA.ExecCmdQueryParamsDS("[REPORTSCHEDULESENSORLINK].[spSelectSensorsByReportScheduleID]", sqlparams);

               
                if (myDS.Tables.Count > 0)
                {

                    myDT = myDS.Tables[0];

                    gvSensorSchedule.DataSource = myDT;
                    gvSensorSchedule.DataBind();
                    
                }
            }
            catch (Exception ex)
            {
                Trace.Write("ReportsConfig.getSensorsForSchedule: Error: " + ex.Message);
            }
        }

        public ReportsConfig()
        {
            Load += Page_Load;
        }        
    }
}