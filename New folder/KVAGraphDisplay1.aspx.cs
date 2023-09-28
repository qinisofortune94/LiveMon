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
            String DashboardName = Request.QueryString["DashboardName"];
            Session["DashName"] = "Dash1";

            //String SensorID = Request.QueryString["SensorID"];
            //Session["SensorID"] = "2043";
            //Session["SensorID2"] = "2044";
        }
        //Jason method
        //[HttpPost]
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
                            DateTime EDate = Convert.ToDateTime("2017-06-08 14:39:22.440");
                            DateTime SDate = Convert.ToDateTime("2017-04-18 14:01:21.000");
                            //  int CompanyId = Convert.ToInt16(Session["CompanyId"]);
                            //  var containers = db.tb_Container.ToList().FindAll(x => x.IsDeleted == false && x.IsActive == true);
                            SqlDataReader KVA1 = SensorDetails(SensorID, SDate, EDate);

                            SqlDataReader Current1 = FindCurrentReading(CurrentID, SDate, EDate);

                            string KVAV = "";
                            string current = "";
                            // string caption = "";
                            while (KVA1.Read())
                            {
                                KVAV = Convert.ToString(KVA1["Value"]);

                            }
                            while (Current1.Read())
                            {
                                current = Convert.ToString(Current1["Value"]);
                                //  caption = Convert.ToString(Current1["Caption"]);
                            }

                            string[] KValues = new string[3];
                            KValues[0] = KVAV;
                            KValues[1] = current;
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
            }
            catch (Exception)
            {

            }
            return listValues; 
        }




        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string[]> Getvalues1()
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


                        if ((count == 1))
                        {
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
                            DateTime EDate = Convert.ToDateTime("2017-06-08 14:39:22.440");
                            DateTime SDate = Convert.ToDateTime("2017-04-18 14:01:21.000");
                            //  int CompanyId = Convert.ToInt16(Session["CompanyId"]);
                            //  var containers = db.tb_Container.ToList().FindAll(x => x.IsDeleted == false && x.IsActive == true);
                            SqlDataReader KVA1 = SensorDetails(SensorID, SDate, EDate);

                            SqlDataReader Current1 = FindCurrentReading(CurrentID, SDate, EDate);

                            string KVAV = "";
                            string current = "";
                            // string caption = "";
                            while (KVA1.Read())
                            {
                                KVAV = Convert.ToString(KVA1["Value"]);

                            }
                            while (Current1.Read())
                            {
                                current = Convert.ToString(Current1["Value"]);
                                //  caption = Convert.ToString(Current1["Caption"]);
                            }

                            string[] KValues = new string[3];
                            KValues[0] = KVAV;
                            KValues[1] = current;
                            KValues[2] = Caption;

                            int i = 0;
                            listValues.Add(KValues);

                            i++;
                            // var json =    new JavaScriptSerializer().Serialize(listValues.ToArray());

                                   //  return Json(listValues.ToArray(), JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    count++;
                }
            }
            catch (Exception)
            {

            }
            return listValues;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string[]> Getvalues2()
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


                        if ((count == 2))
                        {
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
                            DateTime EDate = Convert.ToDateTime("2017-06-08 14:39:22.440");
                            DateTime SDate = Convert.ToDateTime("2017-04-18 14:01:21.000");
                            //  int CompanyId = Convert.ToInt16(Session["CompanyId"]);
                            //  var containers = db.tb_Container.ToList().FindAll(x => x.IsDeleted == false && x.IsActive == true);
                            SqlDataReader KVA1 = SensorDetails(SensorID, SDate, EDate);

                            SqlDataReader Current1 = FindCurrentReading(CurrentID, SDate, EDate);

                            string KVAV = "";
                            string current = "";
                            // string caption = "";
                            while (KVA1.Read())
                            {
                                KVAV = Convert.ToString(KVA1["Value"]);

                            }
                            while (Current1.Read())
                            {
                                current = Convert.ToString(Current1["Value"]);
                                //  caption = Convert.ToString(Current1["Caption"]);
                            }

                            string[] KValues = new string[3];
                            KValues[0] = KVAV;
                            KValues[1] = current;
                            KValues[2] = Caption;

                            int i = 0;
                            listValues.Add(KValues);

                            i++;
                            // var json =    new JavaScriptSerializer().Serialize(listValues.ToArray());

                                   //  return Json(listValues.ToArray(), JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    count++;
                }
            }
            catch (Exception)
            {

            }
            return listValues;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string[]> Getvalues3()
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


                        if ((count == 3))
                        {
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
                            DateTime EDate = Convert.ToDateTime("2017-06-08 14:39:22.440");
                            DateTime SDate = Convert.ToDateTime("2017-04-18 14:01:21.000");
                            //  int CompanyId = Convert.ToInt16(Session["CompanyId"]);
                            //  var containers = db.tb_Container.ToList().FindAll(x => x.IsDeleted == false && x.IsActive == true);
                            SqlDataReader KVA1 = SensorDetails(SensorID, SDate, EDate);

                            SqlDataReader Current1 = FindCurrentReading(CurrentID, SDate, EDate);

                            string KVAV = "";
                            string current = "";
                            // string caption = "";
                            while (KVA1.Read())
                            {
                                KVAV = Convert.ToString(KVA1["Value"]);

                            }
                            while (Current1.Read())
                            {
                                current = Convert.ToString(Current1["Value"]);
                                //  caption = Convert.ToString(Current1["Caption"]);
                            }

                            string[] KValues = new string[3];
                            KValues[0] = KVAV;
                            KValues[1] = current;
                            KValues[2] = Caption;

                            int i = 0;
                            listValues.Add(KValues);

                            i++;
                            // var json =    new JavaScriptSerializer().Serialize(listValues.ToArray());

                                 //  return Json(listValues.ToArray(), JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    count++;
                }
            }
            catch (Exception)
            {

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

            return MyDataAccess.ExecCmdQueryParams("[dbo].[FindSensorData]", parameters);



        }

        private static SqlDataReader FindMachineData()
        {
            return MyDataAccess.ExecCmdQueryNoParams("[dbo].[FindSensorData1]");
        }



    }
}