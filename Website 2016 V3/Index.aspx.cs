using System;
using System.Web.UI.WebControls;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Web.Configuration;
using Microsoft.VisualBasic;
using System.IO.Compression;
using System.Collections.Generic;
using LiveMonitoring;
using System.Web.UI;
using System.Text;
using System.IO;
using System.Collections;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using website2016V2.Dashboard;

namespace website2016
{
    public partial class Index : System.Web.UI.Page
    {
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private string _path = "";
        private string _filterAttribute;
        private string _firstname;
        private string _lastname;
        private string _email;
        private string _site;

        public string MyTable;
        public string MyCMD = "";
        private Collection MyCollection = new Collection();
        private Collection MyCameraCollection = new Collection();
        private Collection MySensorCollection = new Collection();
        private Collection MySensorGroupCollection = new Collection();
        private Collection MyIPDevicesCollection = new Collection();
        private Collection MyOtherDevicesCollection = new Collection();
        private Collection MySNMPDevicesCollection = new Collection();

        private Collection MySensorCollection2 = new Collection();

        private Collection MyCollection2 = new Collection();
        private LiveMonitoring.GlobalRemoteVars MyRemObj = new LiveMonitoring.GlobalRemoteVars();


        private static Collection MySensorsCol = new Collection();
        private LiveMonitoring.GlobalRemoteVars MyRemm = new LiveMonitoring.GlobalRemoteVars();
        public DateTime LastRefresh;

        List<string> oListXAxis = new List<string>();
        List<object> oListYAxis = new List<object>();

        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        string conStrReport = WebConfigurationManager.ConnectionStrings["IPMonConnectionStringReport"].ToString();
        LiveMonitoring.testing test = new LiveMonitoring.testing();
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Request.UserAgent.ToLower().Contains("konqueror") == false))
            {
                if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("gzip")))
                {
                    Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress, true);
                    Response.AppendHeader("Content-encoding", "gzip");
                }
                else
                {
                    if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("deflate")))
                    {
                        Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress, true);
                        Response.AppendHeader("Content-encoding", "deflate");
                    }
                }
            }

            if (IsPostBack == false)
            {
                if (Session["LoggedIn"] == null)
                {
                    try
                    {
                        Session["LoggedIn"] = "False";
                        string d = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().Name;
                        //TextBox tb = (TextBox)Login1.FindControl("UserName");
                        //tb.Text = d + "\\" + test.GetUser();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            if (((string)Session["LoggedIn"] == "True"))
            {
                dashboardpanel.Visible = true;
                PanelLogin.Visible = false;
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                lblUser2.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                lblUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                graphs();
            }
            else if ((IsPostBack == false))
            {
                try
                {
                    Login1.Visible = true;
                    string s = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().Name;
                    TextBox tb = (TextBox)Login1.FindControl("UserName");
                    tb.Text = (s + ("\\" + test.GetUser()));
                }
                catch (Exception ex)
                {
                }
            }
            
            getSensorcounter();
            
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
          
            try
            {
                if (Login1.UserName.Contains("\\"))
                {
                    if ((IsAuthenticated(Domain.GetCurrentDomain().Name, test.GetUser(), Login1.Password) == true))
                    {
                        MyUser = MyRem.LiveMonServer.TryLogin(Login1.UserName, MyRem.GetEncrypted(Login1.Password), _firstname, _lastname, _email, _site);
                    }
                    else
                    {
                        MyUser = MyRem.LiveMonServer.TryLogin(Login1.UserName, MyRem.GetEncrypted(Login1.Password), "", "", "", "");
                    }
                }
                else
                {
                    MyUser = MyRem.LiveMonServer.TryLogin(Login1.UserName, MyRem.GetEncrypted(Login1.Password), "", "", "", "");
                }
            }
            catch (Exception ex)
            {
            }
            if ((MyUser == null) == false)
            {
                if (MyUser.ID != 0)
                {
                    e.Authenticated = true;
                    //Session.SessionID = SessionStateMode.InProc
                    Session["LoggedIn"] = "True";
                    Session["UserDetails"] = MyUser;
                    Session.Timeout = 60;//1 minute for the session
                    lblUser.Text = "User:" + MyUser.UserName + " LL:" + MyUser.LoginDT.ToString();
                    lblUser2.Text = ("User:" + (MyUser.FirstName + (" LL:" + MyUser.SurName)));
                }
                else
                {
                    e.Authenticated = false;
                    Session["LoggedIn"] = "";
                    Session["UserDetails"] = "";
                }
            }
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + "\\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.			
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                //full names
                search.PropertiesToLoad.Add("givenname");
                // firstname
                search.PropertiesToLoad.Add("sn");
                // lastname
                search.PropertiesToLoad.Add("mail");
                // mail
                search.PropertiesToLoad.Add("ou");

                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
                _firstname = (string)result.Properties["givenname"][0];
                _lastname = (string)result.Properties["sn"][0];
                try
                {
                    _email = (string)result.Properties["mail"][0];
                }
                catch
                {
                }
                try
                {
                    _site = (string)result.Properties["ou"][0];
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Login1.Visible = true;
            PanelLogin.Visible = true;
            dashboardpanel.Visible = false;
            //string d = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().Name;
            TextBox tb = (TextBox)Login1.FindControl("UserName");
            //tb.Text = d + "\\" + test.GetUser();
        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
        }

        protected void editAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx");
        }

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }

        protected void addNewSensor_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddSensor.aspx");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            getSensorcounter();
        }

        public class SensorCounterStatus
        {
            public int totalSensors = 0;
            public int alertSensors = 0;
            public int errorSensors = 0;
            public int errorSensors2 = 0;
            public int notRespondingSensors = 0;
            public int okSensors = 0;
            public int warningFieldSensors = 0;
            public int alertFieldSensors = 0;
            public int disabledSensor = 0;
            public int df = 0;
        }

        public SensorCounterStatus getSensorcounter()
        {
            MyCollection2.Clear();
            //GetServerObjects 'server1.GetAll()
            MyCollection2 = MyRemObj.get_GetServerObjects((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]);
            object MyObject1 = null;
            SensorCounterStatus retSensorCounterStatus = new SensorCounterStatus();
            MySensorCollection2.Clear();
            if ((MyCollection2 == null) == false)
            {
                foreach (object MyObject1_loopVariable in MyCollection2)
                {
                    MyObject1 = MyObject1_loopVariable;

                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            MySensorCollection2.Add(MySensor);
                            retSensorCounterStatus.totalSensors += 1;
                            switch (MySensor.Status)
                            {
                                case LiveMonitoring.IRemoteLib.StatusDef.ok:
                                    retSensorCounterStatus.okSensors += 1;
                                    break;
                                case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                                    retSensorCounterStatus.notRespondingSensors += 1;
                                    break;
                                case LiveMonitoring.IRemoteLib.StatusDef.criticalerror:
                                    retSensorCounterStatus.errorSensors += 1;
                                    break;
                                case LiveMonitoring.IRemoteLib.StatusDef.alert:
                                    retSensorCounterStatus.alertFieldSensors += 1;
                                    break;
                                case LiveMonitoring.IRemoteLib.StatusDef.statuserror:
                                    retSensorCounterStatus.errorSensors2 += 1;
                                    break;
                                case LiveMonitoring.IRemoteLib.StatusDef.disabled:
                                    retSensorCounterStatus.disabledSensor += 1;
                                    break;
                                case LiveMonitoring.IRemoteLib.StatusDef.devicefailure:
                                    retSensorCounterStatus.df += 1;
                                    break;
                                case (LiveMonitoring.IRemoteLib.StatusDef)LiveMonitoring.IRemoteLib.FieldStatusDef.warning:
                                    retSensorCounterStatus.warningFieldSensors += 1;
                                    break;

                            }

                            lblNoresponse.Text = Convert.ToString(retSensorCounterStatus.notRespondingSensors + " "+ "out of " + retSensorCounterStatus.totalSensors);
                            lblOkay.Text = Convert.ToString(retSensorCounterStatus.okSensors + " " + "out of " + retSensorCounterStatus.totalSensors);
                            lblError.Text = Convert.ToString(retSensorCounterStatus.errorSensors + " " + "out of " + retSensorCounterStatus.totalSensors);
                            lblAlert.Text = Convert.ToString(retSensorCounterStatus.alertFieldSensors + " " + "out of " + retSensorCounterStatus.totalSensors);
                            lblError2.Text = Convert.ToString(retSensorCounterStatus.errorSensors2 + " " + "out of " + retSensorCounterStatus.totalSensors);
                            lblWarning.Text = Convert.ToString(retSensorCounterStatus.warningFieldSensors + " " + "out of " + retSensorCounterStatus.totalSensors);
                            lblDisable.Text = Convert.ToString(retSensorCounterStatus.disabledSensor + " " + "out of " + retSensorCounterStatus.totalSensors);
                            lblDF.Text = Convert.ToString(retSensorCounterStatus.df + " " + "out of " + retSensorCounterStatus.totalSensors);
                            //if (CheckFieldAlert2(MySensor))
                            //{
                            //    retSensorCounterStatus.alertFieldSensors += 1;
                            //    lblAlert.Text = Convert.ToString(retSensorCounterStatus.alertFieldSensors + " " + "out of " + retSensorCounterStatus.totalSensors);
                            //}
                            //else
                            //{
                            //    lblAlert.Text = Convert.ToString("0" + " " + "out of " + retSensorCounterStatus.totalSensors);
                            //}


                            //if (CheckFieldWarning2(MySensor))
                            //{
                            //    retSensorCounterStatus.warningFieldSensors += 1;
                            //    lblWarning.Text = Convert.ToString(retSensorCounterStatus.warningFieldSensors + " " + "out of " + retSensorCounterStatus.totalSensors);
                            //}
                            //else
                            //{
                            //    lblWarning.Text = Convert.ToString("0" + " " + "out of " + retSensorCounterStatus.totalSensors);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return retSensorCounterStatus;
        }
        private bool CheckFieldAlert2(LiveMonitoring.IRemoteLib.SensorDetails MySensor)
        {
            try
            {
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef Myfield in MySensor.Fields)
                {
                    try
                    {
                        if (Myfield.FieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                        {
                            return true;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool CheckFieldWarning2(LiveMonitoring.IRemoteLib.SensorDetails MySensor)
        {
            try
            {
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef Myfield in MySensor.Fields)
                {
                    try
                    {
                        if (Myfield.FieldStatus == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void TestCountSensors()
        {
            Load += Page_Load;
        }

        public void graphs()
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<LiveMonitoring.IRemoteLib.UserDashBoards> FilteredMyDashboardList = new List<LiveMonitoring.IRemoteLib.UserDashBoards>();
            FilteredMyDashboardList = MyRem.LiveMonServer.GetUserDashBoards(MyUser.ID);
            int ChartType = 0;
            int Row = 0;
            int Pos = 0;

            if ((FilteredMyDashboardList == null) == false)
            {
                for (int i = 0; i <= FilteredMyDashboardList.Count - 1; i++)
                {
                    try
                    {
                        ChartType = (int)FilteredMyDashboardList[i].ChartType;
                        Row = FilteredMyDashboardList[i].RowPos;
                        Pos = FilteredMyDashboardList[i].ColPos;
                        switch (ChartType)
                        {
                            case (int)LiveMonitoring.IRemoteLib.liveMonChartType.PIEChart:
                                liveMonPIE objPieChart = (liveMonPIE)LoadControl("~\\Dashboard\\liveMonPIE.ascx");
                                objPieChart.setDashBoard = FilteredMyDashboardList[i];
                                AddControl(objPieChart, FilteredMyDashboardList[i].RowPos, FilteredMyDashboardList[i].ColPos);
                                break;
                            case (int)LiveMonitoring.IRemoteLib.liveMonChartType.BarChart:
                                liveMonOnOff objBarChart = (liveMonOnOff)LoadControl("~\\Dashboard\\liveMonOnOff.ascx");
                                objBarChart.setDashBoard = FilteredMyDashboardList[i];
                                AddControl(objBarChart, FilteredMyDashboardList[i].RowPos, FilteredMyDashboardList[i].ColPos);
                                break;
                            case (int)LiveMonitoring.IRemoteLib.liveMonChartType.Columnchart:
                                liveMonColumn objColumnChart = (liveMonColumn)LoadControl("~\\Dashboard\\liveMonColumn.ascx");
                                objColumnChart.setDashBoard = FilteredMyDashboardList[i];
                                AddControl(objColumnChart, FilteredMyDashboardList[i].RowPos, FilteredMyDashboardList[i].ColPos);
                                break;
                            case (int)LiveMonitoring.IRemoteLib.liveMonChartType.LineChart:
                                liveMonLine objLineChart = (liveMonLine)LoadControl("~\\Dashboard\\liveMonLine.ascx");
                                objLineChart.setDashBoard = FilteredMyDashboardList[i];
                                AddControl(objLineChart, FilteredMyDashboardList[i].RowPos, FilteredMyDashboardList[i].ColPos);
                                break;
                            case (int)LiveMonitoring.IRemoteLib.liveMonChartType.OnOffChart:
                                liveMonOnOffReal objOnOffChart = (liveMonOnOffReal)LoadControl("~\\Dashboard\\liveMonOnOffReal.ascx");
                                objOnOffChart.setDashBoard = FilteredMyDashboardList[i];
                                AddControl(objOnOffChart, FilteredMyDashboardList[i].RowPos, FilteredMyDashboardList[i].ColPos);
                                break;
                            case (int)LiveMonitoring.IRemoteLib.liveMonChartType.Scatterchart:
                                liveMonGauge objScatterChart = (liveMonGauge)LoadControl("~\\Dashboard\\liveMonGauge.ascx");
                                objScatterChart.setDashBoard = FilteredMyDashboardList[i];
                                AddControl(objScatterChart, FilteredMyDashboardList[i].RowPos, FilteredMyDashboardList[i].ColPos);
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

        }

        private void AddControl(object Myobject, int RowPos, int ColPos)
        {
            try
            {
                if (RowPos == 1)
                {
                    if (ColPos == 1)
                    {
                        phRow1Pos1.Controls.Add((Control)Myobject);
                    }
                    else if (ColPos == 2)
                    {
                        phRow1Pos2.Controls.Add((Control)Myobject);
                    }
                    else if (ColPos == 3)
                    {
                        phRow1Pos3.Controls.Add((Control)Myobject);
                    }
                }
                else if (RowPos == 2)
                {
                    if (ColPos == 1)
                    {
                        phRow2Pos1.Controls.Add((Control)Myobject);
                    }
                    else if (ColPos == 2)
                    {
                        phRow2Pos2.Controls.Add((Control)Myobject);
                    }
                    else if (ColPos == 3)
                    {
                        phRow2Pos3.Controls.Add((Control)Myobject);
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        public class SensorFields
        {
            public string id;
            public string text;
        }

        protected void btnnEditSettings_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("UserEditDashboard.aspx");

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnnNewSettings_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("NewDashboardSetting.aspx");

            }
            catch (Exception ex)
            {
            }
        }

        public void Dashboard_DashBoardDisplay()
        {
            Load += Page_Load;
        }

        protected void okayLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=Ok");
        }

        protected void warnLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=Warning");
        }

        protected void NoResponseLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=NoResponse");
        }

        protected void alertLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=Alert");
        }

        protected void errLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=CriticalError");
        }

        protected void LinkError_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=Error");
        }

        protected void LinkDisable_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=Disabled");
        }

        protected void LinkDF_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sensor_readings_by_status?Status=DeviceFailure");
        }

        protected void linkPeople_Click(object sender, EventArgs e)
        {
            Response.Redirect("addPeople.aspx");
        }
    }
}