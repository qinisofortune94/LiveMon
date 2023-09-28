using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace website2016V2.Dashboard
{
    public partial class liveMonPIE : System.Web.UI.UserControl
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();

        private LiveMonitoring.IRemoteLib.UserDashBoards _myChart;
        public LiveMonitoring.IRemoteLib.UserDashBoards setDashBoard
        {
            set { _myChart = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (((string)Session["LoggedIn"] == "True"))
                {
                    if (Page.IsPostBack == false)
                    {
                        LoadPieData();
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
            }
        }

        public string GetFieldName(int fieldId, int sensorid, int parameterid)
        {
            string fieldname = "";
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldName]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramfieldid).Value = fieldId;
            cmd.Parameters.Add(paramsensorid).Value = sensorid;

            cmd.Parameters.Add(paramuserDashId).Value = parameterid;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                fieldname = (string)reader["Field"];
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return fieldname;
        }

        public string GetSensorName(int fieldId, int sensorid, int parameterid)
        {
            string sensorname = "";
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldName]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramfieldid).Value = fieldId;
            cmd.Parameters.Add(paramsensorid).Value = sensorid;

            cmd.Parameters.Add(paramuserDashId).Value = parameterid;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                sensorname = (string)reader["Sensor"];
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return sensorname;
        }

        public double GetMinValue(int fieldId, int sensorid, int parameterid)
        {
            double minValue = 0;
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldName]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramfieldid).Value = fieldId;
            cmd.Parameters.Add(paramsensorid).Value = sensorid;

            cmd.Parameters.Add(paramuserDashId).Value = parameterid;


            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                minValue = (int)reader["minValue"];
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return minValue;
        }

        public double GetMaxValue(int fieldId, int sensorid, int parameterid)
        {
            double maxValue = 0;
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldName]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramfieldid).Value = fieldId;
            cmd.Parameters.Add(paramsensorid).Value = sensorid;

            cmd.Parameters.Add(paramuserDashId).Value = parameterid;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                maxValue = (int)reader["maxValue"];
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return maxValue;
        }

        public double GetLastValue(int fieldId, int sensorid, int parameterid)
        {
            double lastValue = 0;
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldName]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramfieldid).Value = fieldId;
            cmd.Parameters.Add(paramsensorid).Value = sensorid;

            cmd.Parameters.Add(paramuserDashId).Value = parameterid;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lastValue = (int)reader["lastValue"];
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return lastValue;
        }

        public string GetColor(int colorid)
        {
            string color = "";
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramColorId = new SqlParameter("@ColorId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetColor]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramColorId).Value = colorid;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                color = (string)reader["Color"];
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            return color;
        }

        public System.Drawing.Color GetColorDrawing(int colorid)
        {
            System.Drawing.Color color = new System.Drawing.Color();

            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramColorId = new SqlParameter("@ColorId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetColor]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(paramColorId).Value = colorid;

            SqlDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    color = System.Drawing.ColorTranslator.FromHtml((string)reader["Color"]);
                }
            }
            catch
            {
            }
            reader.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            if (color.IsEmpty)
            {
                color = System.Drawing.Color.Chocolate;
            }
            return color;
        }

        public void LoadGraph(bool isCaption, string CaptionLabel, string Color, string fielname, double minValue, double maxValue, double lastValue, string groupname, string sensorname)
        {
            DotNet.Highcharts.Highcharts Piechart = new DotNet.Highcharts.Highcharts("Piechart").InitChart(new Chart
            {
                Width = 350,
                Height = 300,
                Type = ChartTypes.Pie
            });
            Title MyTitle = new Title();
            MyTitle.Text = sensorname.ToUpper();
            Piechart.SetTitle(MyTitle);
            PlotOptions MyPlotoptions = new PlotOptions();
            PlotOptionsPie MyPlotoptionsPie = new PlotOptionsPie();
            var _with1 = MyPlotoptionsPie;
            _with1.AllowPointSelect = true;
            _with1.Cursor = Cursors.Pointer;
            _with1.DataLabels = new PlotOptionsPieDataLabels { Enabled = true };
            MyPlotoptions.Pie = MyPlotoptionsPie;
            Piechart.SetPlotOptions(MyPlotoptions);
            if (lastValue <= 0)
            {
                lastValue = 2.1;
            }
            if (minValue <= 0)
            {
                minValue = 0.1;
            }
            if (maxValue <= 0)
            {
                maxValue = 1.1;
            }
            Series MySeries = new Series();
            DotNet.Highcharts.Options.Point MyPoint = new DotNet.Highcharts.Options.Point();
            var _with2 = MyPoint;
            _with2.Name = "Min Value";
            _with2.Y = minValue;
            _with2.Sliced = true;
            _with2.Selected = true;
            _with2.Color = System.Drawing.Color.Green;
            DotNet.Highcharts.Options.Point MyPoint1 = new DotNet.Highcharts.Options.Point();
            var _with3 = MyPoint1;
            _with3.Name = "Max Value";
            _with3.Y = maxValue;
            _with3.Sliced = true;
            _with3.Selected = false;
            _with3.Color = System.Drawing.Color.Red;
            DotNet.Highcharts.Options.Point MyPoint2 = new DotNet.Highcharts.Options.Point();
            var _with4 = MyPoint2;
            _with4.Name = "Last Value";
            _with4.Y = lastValue;
            _with4.Sliced = true;
            _with4.Selected = false;
            _with4.Color = System.Drawing.Color.GreenYellow;
            DotNet.Highcharts.Options.Point[] MyData = {
                MyPoint,
                MyPoint1,
                MyPoint2
            };
            MySeries.Data = new DotNet.Highcharts.Helpers.Data(MyData);
            Piechart.SetSeries(MySeries);
            ltrChart.Text = Piechart.ToHtmlString();
        }

        public void LoadChart()
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<LiveMonitoring.IRemoteLib.UserDashBoards> FilteredMyDashboardList = new List<LiveMonitoring.IRemoteLib.UserDashBoards>();
            LiveMonitoring.IRemoteLib.UserDashBoards AllMyDashboardList = default(LiveMonitoring.IRemoteLib.UserDashBoards);
            int one = 1;
            FilteredMyDashboardList = MyRem.LiveMonServer.GetUserDashBoards(MyUser.ID);
            AllMyDashboardList = FilteredMyDashboardList.FirstOrDefault(z => z.ChartType == (LiveMonitoring.IRemoteLib.liveMonChartType)one);

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("PieChart").InitChart(new Chart
            {
                Width = 500,
                Height = 300,
                Type = ChartTypes.Pie
            });

            XAxis MyXaxis = new XAxis();
            MyXaxis.Categories = new string[] {
                "Min Value",
                "Max Value",
                "Last Value"
            };
            chart.SetXAxis(MyXaxis);

            Title MyTitle = new Title();
            MyTitle.Text = (AllMyDashboardList.GraphName);
            MyTitle.X = -10;
            chart.SetTitle(MyTitle);
            string color = "";
            string ActualColor = "";
            if (color.Trim().Length > 0)
            {
                ActualColor = color;
            }
            else
            {
                ActualColor = "#808080";
            }
            Legend myLegend = new Legend();
            myLegend.Layout = Layouts.Vertical;
            myLegend.Align = HorizontalAligns.Right;
            myLegend.VerticalAlign = VerticalAligns.Top;
            myLegend.X = -1;
            myLegend.Y = 40;
            myLegend.BorderWidth = 1;
            chart.SetLegend(myLegend);

            YAxis MyYaxis2 = new YAxis();

            YAxisTitle MyTitleY2 = new YAxisTitle();

            double minvalue = 0;
            double maxvalue = 0;
            double lastvalue = 0;

            Series[] series = new Series[AllMyDashboardList.DashBoardParameters.Count];
            YAxis[] MyYaxis = new YAxis[AllMyDashboardList.DashBoardParameters.Count];
            YAxisTitle MyTitleY = new YAxisTitle();
            string FieldName = "";
            for (int i = 0; i <= AllMyDashboardList.DashBoardParameters.Count - 1; i++)
            {
                MyTitleY.Text = GetSensorName(AllMyDashboardList.DashBoardParameters[i].FieldNo, AllMyDashboardList.DashBoardParameters[i].SensorID, AllMyDashboardList.DashBoardParameters[i].ID);
                minvalue = GetMinValue(AllMyDashboardList.DashBoardParameters[i].FieldNo, AllMyDashboardList.DashBoardParameters[i].SensorID, AllMyDashboardList.DashBoardParameters[i].ID);
                maxvalue = GetMaxValue(AllMyDashboardList.DashBoardParameters[i].FieldNo, AllMyDashboardList.DashBoardParameters[i].SensorID, AllMyDashboardList.DashBoardParameters[i].ID);
                lastvalue = GetLastValue(AllMyDashboardList.DashBoardParameters[i].FieldNo, AllMyDashboardList.DashBoardParameters[i].SensorID, AllMyDashboardList.DashBoardParameters[i].ID);
                FieldName = GetFieldName(AllMyDashboardList.DashBoardParameters[i].FieldNo, AllMyDashboardList.DashBoardParameters[i].SensorID, AllMyDashboardList.DashBoardParameters[i].ID);
                MyYaxis[i] = new YAxis { Title = MyTitleY };

                series[i] = new Series
                {
                    Name = FieldName,
                    Data = new DotNet.Highcharts.Helpers.Data(new object[] {
                        minvalue,
                        maxvalue,
                        lastvalue
                    })
                };
            }

            chart.SetYAxis(MyYaxis);
            chart.SetSeries(series);
            chart.SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.series.name + ': </b>'+ this.y +' %'; }" });

            ltrChart.Text = chart.ToHtmlString();
        }

        public void LoadPieData()
        {
            var colorr = System.Drawing.ColorTranslator.FromHtml("#CCE6FF");
            var bgColor = new DotNet.Highcharts.Helpers.BackColorOrGradient(colorr);
            var borderColor = System.Drawing.ColorTranslator.FromHtml("#6495ED");
            var pbcolor = System.Drawing.ColorTranslator.FromHtml("#F0FFF0");
            var pbcolor2 = new DotNet.Highcharts.Helpers.BackColorOrGradient(pbcolor);
            var pbordercolor = System.Drawing.ColorTranslator.FromHtml("#6495ED");

            DotNet.Highcharts.Highcharts Piechart = new DotNet.Highcharts.Highcharts("Piechart" + _myChart.ID.ToString()).InitChart(new Chart
            {
                //Width = 350,
                Height = 300,
                Type = ChartTypes.Pie,
                BackgroundColor = bgColor,
                BorderColor = borderColor,
                BorderWidth = 2,
                ClassName = "dark-container",
                PlotBackgroundColor = pbcolor2,
                PlotBorderColor = pbordercolor,
                PlotBorderWidth = 1
            });
            Title MyTitle = new Title();
            MyTitle.Text = _myChart.GraphName.ToUpper();
            Piechart.SetTitle(MyTitle);
            Credits MyCredits = new Credits();
            MyCredits.Href = "http://www.livemonitoring.co.za";
            MyCredits.Text = "Live Monitoring";
            Piechart.SetCredits(MyCredits);
            PlotOptions MyPlotoptions = new PlotOptions();
            PlotOptionsPie MyPlotoptionsPie = new PlotOptionsPie();
            var _with5 = MyPlotoptionsPie;
            _with5.AllowPointSelect = true;
            _with5.Cursor = Cursors.Pointer;
            _with5.DataLabels = new PlotOptionsPieDataLabels { Enabled = true };
            MyPlotoptions.Pie = MyPlotoptionsPie;
            Piechart.SetPlotOptions(MyPlotoptions);
            double minvalue = 0;
            double maxvalue = 0;
            double lastvalue = 0;
            Series MySeries = new Series();
            string FieldName = "";
            System.Drawing.Color color = new System.Drawing.Color();
            string Sensor = "";
            DotNet.Highcharts.Options.Point[] MyPoint = new DotNet.Highcharts.Options.Point[_myChart.DashBoardParameters.Count()];

            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldHistoryDataPie]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;

            string CatDate = "";
            int Value = 0;
            int fieldid = 0;
            int sensorid = 0;
            int dashboardid = 0;
            List<Series> allSeries = new List<Series>();

            for (int i = 0; i <= _myChart.DashBoardParameters.Count() - 1; i++)
            {
                FieldName = GetFieldName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                color = GetColorDrawing(_myChart.DashBoardParameters[i].Color);
                Sensor = GetSensorName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                sensorid = _myChart.DashBoardParameters[i].SensorID;
                fieldid = _myChart.DashBoardParameters[i].FieldNo;
                dashboardid = _myChart.DashBoardParameters[i].ID;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(paramfieldid).Value = fieldid;
                cmd.Parameters.Add(paramsensorid).Value = sensorid;
                cmd.Parameters.Add(paramuserDashId).Value = dashboardid;
                cmd.CommandTimeout = int.MaxValue;
                SqlDataReader reader = cmd.ExecuteReader();
                var results = new List<object[]>();
                while (reader.Read())
                {
                    results.Add(new object[] {
                        reader["Value"],
                        reader["CountOf"]
                    });
                }
                reader.Close();
                allSeries.Add(new Series
                {
                    Name = Sensor + "-" + FieldName,
                    Data = new DotNet.Highcharts.Helpers.Data(results.ToArray())
                });
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            Piechart.SetSeries(allSeries.Select(s => new Series
            {
                Name = s.Name,
                Data = s.Data
            }).ToArray());

            ltrChart.Text = Piechart.ToHtmlString();
        }
        public void Dashboard_liveMonPIE()
        {
            Load += Page_Load;
        }
    }
}