using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Mvc;
using LiveMonitoring;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;

namespace website2016V2
{
    public partial class KVAGraphDisplay : System.Web.UI.Page
    {
       public static int count = 0;
        //public class KVAValues
        //{
        //    public string [] KVA { get; set; }

        //}
        private static DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack == false)
            {
                String DashboardName = "Dash1";
                Session["DashName"] = DashboardName;
               // getDashboards();
              //  Dashboards.SelectedValue = DashboardName;
               // lbl1.Text = DateTime.Now.ToLongTimeString();
                //ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(string), "CallMyFunction", "Draw1stChart();", true);
            }
            else
            {

            }
        }

       

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string[]> Getvalues()
        {
            count = 0;      
            SqlDataReader Sensors;
            List<string[]> listValues = new List<string[]>();
            //find logged in users not logged off in 24 hours [FindLoggedInoperator24hrs]
            if (Convert.ToString(HttpContext.Current.Session["DashName"]) != "")
            {
                Sensors = SensorData(HttpContext.Current.Session["DashName"].ToString());
            }
            else
            {             
                Sensors = FindMachineData();
            }
            try
            {               
                while (Sensors.Read()) // for each loogged in operator
                {
                    try
                    {                   
                        {
                            int MaxReachedNMDSensorID = 0;
                            int CurrentID = 0;
                            int SensorID = Convert.ToInt32(Sensors["SensorID"]);
                            string Caption = Convert.ToString(Sensors["Caption"]);

                            try
                            {
                                CurrentID = Convert.ToInt32(Sensors["CurrentID"]);
                            }
                            catch (Exception)
                            {
                                CurrentID = 0;
                            }
                            try
                            {
                                MaxReachedNMDSensorID = Convert.ToInt32(Sensors["NMDID"]);
                            }
                            catch (Exception)
                            {
                                MaxReachedNMDSensorID = 0;
                            }
                            DateTime today = DateTime.Now;
                               DateTime EDate = Convert.ToDateTime(today);
                               DateTime SDate = Convert.ToDateTime(DateTime.Now.AddDays(-60));
                         //   DateTime EDate = Convert.ToDateTime("2017-06-08 14:39:22.440");
                          //  DateTime SDate = Convert.ToDateTime("2017-04-18 14:01:21.000");
                            //  int CompanyId = Convert.ToInt16(Session["CompanyId"]);
                            //  var containers = db.tb_Container.ToList().FindAll(x => x.IsDeleted == false && x.IsActive == true);                                                   
                            double MaximumDemandReached = 0;
                            string MaximumDemandDate = "";
                            string KVAV = "";
                            string current = "";
                            try
                            {
                                SqlDataReader MaxDemandKVA = FindMDReached(MaxReachedNMDSensorID); 
                                try
                                {
                                     
                                    while (MaxDemandKVA.Read())
                                    {
                                        MaximumDemandReached = Convert.ToDouble(MaxDemandKVA["Value"]);
                                        MaximumDemandDate = Convert.ToString(MaxDemandKVA["OtherData"]);
                                    }
                                }
                                catch (Exception)
                                {

                                    MaximumDemandReached = 0;
                                }
                                finally
                                {
                                    if (MaxDemandKVA.IsClosed == false) MaxDemandKVA.Close();
                                }                                
                            }
                            catch (Exception ex)
                            {
                               
                            }



                            SqlDataReader KVA1 = SensorDetails(SensorID, SDate, EDate);
                            try
                            {
                                while (KVA1.Read())
                                {
                                    KVAV = Convert.ToString(KVA1["Value"]);

                                }
                            }
                            catch (Exception)
                            {

                                //throw;
                            }
                            finally
                            {
                                if (KVA1.IsClosed == false) KVA1.Close();
                            }
                            //KVA1.Close();

                            SqlDataReader Current1 = FindCurrentReading(CurrentID, SDate, EDate);
                            try
                            {
                                while (Current1.Read())
                                {
                                    current = Convert.ToString(Current1["Value"]);
                                    //  caption = Convert.ToString(Current1["Caption"]);
                                }
                            }
                            catch (Exception)
                            {

                                //throw;
                            }
                            finally
                            {
                                if (Current1.IsClosed == false) Current1.Close();
                            }

                            string[] KValues = new string[3];
                            KValues[0] = current;
                            KValues[1] = Convert.ToString(MaximumDemandReached);
                            KValues[2] = Caption;

                            int i = 0;
                            listValues.Add(KValues);

                           
                            // var json =    new JavaScriptSerializer().Serialize(listValues.ToArray());

                                  //  return Json(listValues.ToArray(), JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {

                    }     
                }
               
                //Sensors.Close();
            }
            catch (Exception)
            {
               // Getvalues();
            }
            finally
            {
                if (Sensors.IsClosed == false) Sensors.Close();
               
            }

             return listValues; 
        }





        private static SqlDataReader SensorDetails(int SensorID, DateTime sDate, DateTime eDate)
        {

            SqlParameter[] parameters =

                           {

                                    new SqlParameter("@SensorID",SensorID),
                                   new SqlParameter("@SDate", sDate),
                                    new SqlParameter("@EDate", eDate)


                                };


            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDBySensorDate]", parameters);




        }

        private static SqlDataReader FindCurrentReading(int CurrentID, DateTime sDate, DateTime eDate)
        {




            SqlParameter[] parameters =

                           {

                                    new SqlParameter("@SensorID",CurrentID),
                                     new SqlParameter("@SDate",sDate),
                                      new SqlParameter("@EDate",eDate)
                                };


            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDActiveBySensorDate1]", parameters);


        }

        private static SqlDataReader SensorData(string DispTableName)
        {




            SqlParameter[] parameters =

                           {

                                    new SqlParameter("@DashboardName",DispTableName)

                                };

            return MyDataAccess.ExecCmdQueryParams("[dbo].[FindSensorData11]", parameters);



        }

        private static SqlDataReader FindMachineData()
        {
            return MyDataAccess.ExecCmdQueryNoParams("[dbo].[FindSensorData1]");
        }

        private static SqlDataReader FindMDReached(int SensorID)
        {
            SqlParameter[] parameters =
                           {
                                    new SqlParameter("@SensorID",SensorID)
                                };
            return MyDataAccess.ExecCmdQueryParams("[dbo].[MeteringGetMDReachedSensor]", parameters);
        }
        //public void getDashboards()
        //{
        //    SqlDataReader dataReader;
        //    //SqlDataReader functionReturnValue = default(SqlDataReader);
        //    try
        //    {
        //        dataReader = MyDataAccess.ExecCmdQueryNoParams("[dbo].[GetDashboards]");
        //        String Dash = "";
        //        Dashboards.Items.Clear();
        //        Dashboards.Items.Add("");
        //        Dashboards.Items.Add("---Add Dashboard---");
        //        try
        //        {
        //            while (dataReader.Read())
        //            {
        //                Dash = Convert.ToString(dataReader["DashboardName"]);
        //                Dashboards.Items.Add(Dash);
        //            }

        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    catch (Exception)
        //    {

        //       // throw;
        //    }
        //    finally
        //    {

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

        //    }
        //}

    }
}