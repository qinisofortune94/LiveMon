
using LiveMonitoring;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class DashboardKVA : System.Web.UI.Page
    {
        private static DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Refresh", "120");
            if (IsPostBack == false)
            {
               //Response.AppendHeader("Refresh", "300");
                String DashboardName = "Dash1";
                Session["DashName"] = DashboardName;
             //   getDashboards();
             //   Dashboards.SelectedValue = DashboardName;
                Shifttbl.InnerHtml = BuildShiftDisplay();
                lbl1.Text = DateTime.Now.ToLongTimeString();
            }
            else
            {
                
            }
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lbl1.Text = DateTime.Now.ToLongTimeString();
            Shifttbl.InnerHtml = BuildShiftDisplay();
        }

        private string BuildShiftDisplay()
        {
            int rowcount = 0;
            int colcount = 1;

            string DashDisplay = "<table id=\"NMDDash\" style=\"width:100%\" border=\"1\" >";
            if (!string.IsNullOrEmpty(Session["DashName"] as string))
            {
                DashDisplay += "<tr><th colspan=\"5\" style=\"align-content:center;text-align:center;\">" + Session["DashName"] + " </th></tr>";
            }
            else
            {
                DashDisplay += "<tr><th colspan=\"5\" style=\"align-content:center;text-align:center;\"> </th></tr>";
            }
            DashDisplay += "<tr>";
            try
            {

                SqlDataReader Sensors;
                //find logged in users not logged off in 24 hours [FindLoggedInoperator24hrs]
                if (Convert.ToString(Session["DashName"]) != "")
                {
                    Sensors = SensorData(Session["DashName"].ToString());
                }
                else
                {
                    Sensors = FindMachineData();
                }
                //SqlDataReader 
                while (Sensors.Read()) // for each loogged in operator
                {
                    try
                    {
                        DateTime today = DateTime.Now;

                        int CurrentKVASensorID = 0;
                        int MaxReachedNMDSensorID = 0;
                        int SensorID = Convert.ToInt32(Sensors["SensorID"]);
                        string Caption = Convert.ToString(Sensors["Caption"]);

                        try
                        {
                            CurrentKVASensorID = Convert.ToInt32(Sensors["CurrentID"]);

                        }
                        catch (Exception)
                        {
                            CurrentKVASensorID = 0;

                        }
                        try
                        {
                            MaxReachedNMDSensorID = Convert.ToInt32(Sensors["NMDID"]);

                        }
                        catch (Exception)
                        {
                            MaxReachedNMDSensorID = 0;

                        }
                        DateTime eDate = Convert.ToDateTime(today);
                        DateTime sDate = Convert.ToDateTime(DateTime.Now.AddDays(-60));


                       //  DateTime eDate = Convert.ToDateTime("2017-06-08 14:39:22.440");
                       //   DateTime sDate = Convert.ToDateTime("2017-04-18 14:01:21.000");
                        SqlDataReader machinedata = FindSensorDetails(SensorID, sDate, eDate);


                        double CurrentCalcKVA = 0;
                        decimal CalculatedAVGKVA = 0;
                        double MaximumDemandReached = 0;
                        string MaximumDemandDate="";
                        try
                        {

                            SqlDataReader AvgKVA = FindAvgKVA(SensorID, sDate, eDate);
                            while (AvgKVA.Read())
                            {
                                CalculatedAVGKVA = Convert.ToDecimal(AvgKVA["Average"]);


                            }

                        }
                        catch (Exception ex)
                        {
                            CalculatedAVGKVA = 0;
                        }
                        try
                        {

                            SqlDataReader MaxDemandKVA = FindMDReached(MaxReachedNMDSensorID);
                            while (MaxDemandKVA.Read())
                            {
                                MaximumDemandReached = Convert.ToDouble(MaxDemandKVA["Value"]);
                                MaximumDemandDate = Convert.ToString(MaxDemandKVA["OtherData"]);
                            }

                        }
                        catch (Exception ex)
                        {
                            CalculatedAVGKVA = 0;
                        }

                        try
                        {

                            SqlDataReader CurrentV = FindCurrentReading(CurrentKVASensorID, sDate, eDate);
                            while (CurrentV.Read())
                            {
                                CurrentCalcKVA = Convert.ToDouble(CurrentV["Value"]);
                            }

                        }
                        catch (Exception ex)
                        {
                            CurrentCalcKVA = 0;
                        }
                        TimeSpan span = eDate.Subtract(sDate);
                        TimeSpan spent = today.Subtract(sDate);
                        string MachineDatatbl = "";

                        //////////////////////////////////////////////////////////////////////////////
                        double ProfileAverageKVA = 0;
                        double MonthlyNotifiedNMD = 0;
                        while (machinedata.Read()) // for each machine record (Should only be one??)
                        {


                            //calculate using time since shift start
                            ProfileAverageKVA = Convert.ToDouble(machinedata["Value"]);
                            MonthlyNotifiedNMD = Convert.ToDouble(machinedata["NMD"]);

                            //build a machine Table
                            string tmpStylecolor = "forestgreen";
                            double percentageeff = 0;
                            // if (TotalPlannedMake > 0)
                            percentageeff = (CurrentCalcKVA / MonthlyNotifiedNMD) * 100;
                            Decimal AVG1 = decimal.Round(CalculatedAVGKVA, 2, MidpointRounding.AwayFromZero);

                            if ((percentageeff >= 80.00)) tmpStylecolor = "red";
                            if ((percentageeff < 79.999) && (percentageeff >= 51.00)) tmpStylecolor = "yellow";
                            if (percentageeff <= 50.00) tmpStylecolor = "forestgreen";

                            MachineDatatbl = "<td><table style=\"width:100%\" border=\"1\">";
                            MachineDatatbl += "<tr title=\"Sensor Caption !\" style=\"background-color:lawngreen\"><th colspan=\"3\">" + Caption + "</th></tr>";
                            MachineDatatbl += "<tr style=\"background-color:lawngreen\"><th title=\"30 Min profile Average Reading !\" >AVG KVA :" + ProfileAverageKVA.ToString("0.00") + "</th><th title=\"Planed Maximum !\">NMD :" + MonthlyNotifiedNMD.ToString("0.00") + "</th><th title=\"Date reached:" + MaximumDemandDate + " !\">MaxReached :" + MaximumDemandReached.ToString("0.00") + "</th></tr>";
                            // MachineDatatbl += "<tr style=\"background-color:lawngreen\"><td title=\"Total planned since start of shift to now !\">Planned:" + TotalPlannedMakeNow.ToString("0.00") + "</td><td title=\"Total planned since start of shift to end of shift !\">Planned4Shift:" + TotalPlannedMakeShift.ToString("0.00") + "</td></tr>";
                            MachineDatatbl += "<tr><td colspan=\"3\" title=\"Current KVA Calculated !\" style=\"background-color:" + tmpStylecolor + "\">Current Reading:" + CurrentCalcKVA.ToString("0.00") + "</td></tr>";
                            MachineDatatbl += "</table>";
                            MachineDatatbl += "</td>";

                            DashDisplay += MachineDatatbl;

                            if (colcount % 5 == 0)
                            {
                                DashDisplay += "</tr><tr>";
                                //and reset colums
                                colcount = 1;
                            }
                            else
                            {
                                colcount += 1;
                            }

                        }
                        //close the reader
                        if (machinedata.IsClosed == false) machinedata.Close();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    rowcount += 1;
                }
                //ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(string), "CallMyFunction", "DrawChart();", true);
                if (Sensors.IsClosed == false) Sensors.Close();
            }
            catch (Exception)
            {
                //redraw the page 
                Response.AppendHeader("Refresh", "0");
                //throw; //removed timeout 
            }
            
            DashDisplay += "</table>";
            return DashDisplay;
        }
        private void CommandBtn_Click(Object sender, CommandEventArgs e)
        {

            switch (e.CommandName)
            {

                case "Show Chart":
                    if ((String)e.CommandArgument != "")
                    {
                        string myControlName = "ChartRow" + (String)e.CommandArgument;
                        ContentPlaceHolder myPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
                        HtmlTableRow ct = (myPlaceHolder.FindControl(myControlName)) as HtmlTableRow;
                        //divDriverName1.Attributes.Add("style", "display:none");
                        ct.Visible = true;
                    }

                    break;

                case "Hide Chart":

                    // Test whether the command argument is an empty string ("").
                    if ((String)e.CommandArgument == "")
                    {
                        // End the message.
                        //Message.Text += ".";
                    }
                    else
                    {
                        // Display an error message for the command argument. 
                        //Message.Text += ", however the command argument is not recogized.";
                    }
                    break;
                default:
                    // The command name is not recognized. Display an error message.           
                    break;
            }

        }
        private SqlDataReader FindSensorDetails(int SensorID, DateTime sDate, DateTime eDate)
        {

            SqlParameter[] parameters =
                           {
                                    new SqlParameter("@SensorID",SensorID),
                                   new SqlParameter("@SDate", sDate),
                                    new SqlParameter("@EDate", eDate)
                                };


            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDBySensorDate]", parameters);




        }

        private SqlDataReader FindMachineData()
        {
            return MyDataAccess.ExecCmdQueryNoParams("[dbo].[FindSensorData1]");
        }

        private SqlDataReader FindMDReached(int SensorID)
        {
            SqlParameter[] parameters =
                           {
                                    new SqlParameter("@SensorID",SensorID)
                                };
            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDReachedSensor]", parameters);
        }
        private SqlDataReader SensorData(string DispTableName)
        {




            SqlParameter[] parameters =

                           {

                                    new SqlParameter("@DashboardName",DispTableName)

                                };

            return MyDataAccess.ExecCmdQueryParams("[dbo].[FindSensorData]", parameters);



        }

        private SqlDataReader FindAvgKVA(int SensorID, DateTime sDate, DateTime eDate)
        {




            SqlParameter[] parameters =

                           {

                                    new SqlParameter("@SensorID",SensorID),
                                    new SqlParameter("@sDate",sDate),
                                    new SqlParameter("@eDate",eDate)
                                };

            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDBySensorDateAVG]", parameters);





        }

        private SqlDataReader FindCurrentReading(int CurrentID, DateTime sDate, DateTime eDate)
        {

            SqlParameter[] parameters =

                           {

                                    new SqlParameter("@SensorID",CurrentID),
                                     new SqlParameter("@SDate",sDate),
                                      new SqlParameter("@EDate",eDate)
                                };

            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDActiveBySensorDate1]", parameters);
        }

        //public void getDashboards()
        //{

        //    SqlDataReader functionReturnValue = default(SqlDataReader);

        //    SqlDataReader dataReader = MyDataAccess.ExecCmdQueryNoParams("[dbo].[GetDashboards]");
        //    String Dash = "";
        //    try
        //    {
        //        while (dataReader.Read())
        //        {
        //            Dash = Convert.ToString(dataReader["DashboardName"]);
        //            Dashboards.Items.Add(Dash);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Dashboards.Items.Add(Dash);
        //    }
        //}

        //protected void Dashboards_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    if (Dashboards.SelectedIndex == 1)
        //    {

        //        Response.Redirect("DashboardConfig.aspx", true);

        //    }
        //    else
        //    {

        //        Session["DashName"] = Dashboards.SelectedValue;
        //        Shifttbl.InnerHtml = BuildShiftDisplay();
        //    }
        //}



    }
}
