using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

namespace website2016.Services
{
    /// <summary>
    /// Summary description for WebServiceColumnChart
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceColumnChart : System.Web.Services.WebService
    {
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        public class RevenueEntity
        {
            public string date { get; set; }
            public int value { get; set; }
            public Boolean drilldown { get; set; }
        }

        [WebMethod]
        public List<RevenueEntity> GetRevenueByYear()
        {
            string conc = MyDataAccess.GetAppSetting("DataBaseCon");
            string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
            List<RevenueEntity> YearRevenues = new List<RevenueEntity>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select TOP 10 FieldName,FieldNumber from SensorFields group by FieldName,FieldNumber";
                    cmd.Connection = con;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds, "GetRevenueByYear");
                    }
                }
            }
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables["GetRevenueByYear"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables["GetRevenueByYear"].Rows)
                        {
                            YearRevenues.Add(new RevenueEntity
                            {
                                date = dr["FieldName"].ToString(),
                                value = Convert.ToInt32(dr["FieldNumber"]),
                                drilldown = true
                            });
                        }
                    }
                }
            }
            return YearRevenues;
        }

        [WebMethod]
        public List<RevenueEntity> GetRevenueByQuarter(string date)
        {
            string conc = MyDataAccess.GetAppSetting("DataBaseCon");
            string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
            List<RevenueEntity> QuarterRevenues = new List<RevenueEntity>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select SensorID,FieldNumber from SensorFields where FieldName='" + date + "' group by SensorID,FieldNumber";
                    cmd.Connection = con;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds, "dsQuarter");
                    }
                }
            }
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables["dsQuarter"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables["dsQuarter"].Rows)
                        {
                            QuarterRevenues.Add(new RevenueEntity
                            {
                                date = dr["SensorID"].ToString(),
                                value = Convert.ToInt32(dr["FieldNumber"])

                            });
                        }
                    }
                }
            }
            return QuarterRevenues;
        }
    }
}
