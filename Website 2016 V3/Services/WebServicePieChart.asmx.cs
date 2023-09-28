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
    /// Summary description for WebServicePieChart
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServicePieChart : System.Web.Services.WebService
    {
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        public class WorkTimeDetails
        {
            public string Activity { get; set; }
            public int MachineNumber { get; set; }
        }
        [WebMethod]
        public List<WorkTimeDetails> WorkTime()
        {
            string conc = MyDataAccess.GetAppSetting("DataBaseCon");
            string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
            List<WorkTimeDetails> worktimeinfo = new List<WorkTimeDetails>();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection("Data Source = LIVEMON2012R2; Initial Catalog = dynatech; Integrated Security = True"))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select Activity,MachineNumber from tblWorkTime";
                    cmd.Connection = con;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds, "WorkTime");
                    }
                }
            }
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables["WorkTime"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables["WorkTime"].Rows)
                        {
                            worktimeinfo.Add(new WorkTimeDetails { Activity = dr["Activity"].ToString(), MachineNumber = Convert.ToInt32(dr["MachineNumber"]) });
                        }
                    }
                }
            }
            return worktimeinfo;
        }
    }
}
