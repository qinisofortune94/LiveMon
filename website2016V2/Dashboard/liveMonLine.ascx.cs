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
using System.Drawing;

namespace website2016V2.Dashboard
{
    public partial class liveMonLine : System.Web.UI.UserControl
    {
       // LiveMonitoring.testing test = new LiveMonitoring.testing();
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
                        LoadChart();
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

        public class MyList
        {
            public string Name;

            public List<double> Data;
        }

        public void LoadGraph()
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<LiveMonitoring.IRemoteLib.UserDashBoards> FilteredMyDashboardList = new List<LiveMonitoring.IRemoteLib.UserDashBoards>();
            LiveMonitoring.IRemoteLib.UserDashBoards _myChart = default(LiveMonitoring.IRemoteLib.UserDashBoards);

            FilteredMyDashboardList = MyRem.LiveMonServer.GetUserDashBoards(MyUser.ID);
            //line chart type id=3
            _myChart = FilteredMyDashboardList.FirstOrDefault(z => z.ChartType == (LiveMonitoring.IRemoteLib.liveMonChartType)3);



            DotNet.Highcharts.Highcharts Linechart = new DotNet.Highcharts.Highcharts("Linechart").InitChart(new Chart
            {
                Width = 500,
                Height = 300,
                Type = ChartTypes.Line
            });
            XAxis MyXaxis = new XAxis();
            MyXaxis.Categories = new string[] {
                "Min Value",
                "Max Value",
                "Last Value"
            };
            Linechart.SetXAxis(MyXaxis);

            Title MyTitle = new Title();
            MyTitle.Text = (_myChart.GraphName);
            MyTitle.X = -10;
            Linechart.SetTitle(MyTitle);
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


            YAxis MyYaxis = new YAxis();

            YAxisTitle MyTitleY = new YAxisTitle();

            YAxis MyYaxis2 = new YAxis();

            YAxisTitle MyTitleY2 = new YAxisTitle();

            //Only restricting to 2 sensors i'm failing to do a dynamic list of type YAxisTitle & YAxis
            int j = 0;
            while (j <= _myChart.DashBoardParameters.Count - 1)
            {
                if (j == 0)
                {
                    MyTitleY.Text = GetSensorName(_myChart.DashBoardParameters[j].FieldNo, _myChart.DashBoardParameters[j].SensorID, _myChart.DashBoardParameters[j].ID);
                    MyYaxis.Title = MyTitleY;
                }

                if (j == 1)
                {
                    MyTitleY2.Text = GetSensorName(_myChart.DashBoardParameters[j].FieldNo, _myChart.DashBoardParameters[j].SensorID, _myChart.DashBoardParameters[j].ID);
                    MyYaxis2.Title = MyTitleY2;
                }
                j = j + 1;
            }



            YAxisPlotLines MyPlotlines = new YAxisPlotLines();
            MyPlotlines.Value = 0;
            MyPlotlines.Width = 1;
            MyPlotlines.Color = ColorTranslator.FromHtml(ActualColor);

            YAxis[] allMyYaxis = null;
            //test.getY(allMyYaxis, MyYaxis2, MyYaxis);

            //look at this one
            Linechart.SetYAxis(allMyYaxis);

            Legend myLegend = new Legend();
            myLegend.Layout = Layouts.Vertical;
            myLegend.Align = HorizontalAligns.Right;
            myLegend.VerticalAlign = VerticalAligns.Top;
            myLegend.X = -1;
            myLegend.Y = 40;
            myLegend.BorderWidth = 1;
            myLegend.Floating = true;
            Linechart.SetLegend(myLegend);
            Series seriesA = new Series();
            Series seriesB = new Series();

            //Only restricting to 2 rows
            int i = 0;
            var minvalue = 0;
            var maxvalue = 0;
            var lastvalue = 0;
            
            while (i <= _myChart.DashBoardParameters.Count)
            {
                if (i == 0)
                {
                    seriesA.Name = GetFieldName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    minvalue = (int)GetMinValue(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    maxvalue = (int)GetMaxValue(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    lastvalue = (int)GetLastValue(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    seriesA.Data = new DotNet.Highcharts.Helpers.Data(new object[] {
                        minvalue,
                        maxvalue,
                        lastvalue
                    });
                }

                if (i == 1)
                {
                    seriesB.Name = GetFieldName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    minvalue = (int)GetMinValue(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    maxvalue = (int)GetMaxValue(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    lastvalue = (int)GetLastValue(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                    seriesB.Data = new DotNet.Highcharts.Helpers.Data(new object[] {
                        minvalue,
                        maxvalue,
                        lastvalue
                    });
                }
                i = i + 1;
            }

           //look at this
            Series[] MySeries = null;
            //test.getSeries(MySeries,seriesA, seriesB);

            Linechart.SetSeries(MySeries);
            Linechart.SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.series.name + ': </b>'+ this.y +' %'; }" });
            ltrChart.Text = Linechart.ToHtmlString();
        }

        public void LoadChart()
        {
            var colorr = System.Drawing.ColorTranslator.FromHtml("#CCE6FF");
            var bgColor = new DotNet.Highcharts.Helpers.BackColorOrGradient(colorr);
            var borderColor = System.Drawing.ColorTranslator.FromHtml("#6495ED");
            var pbcolor = System.Drawing.ColorTranslator.FromHtml("#F0FFF0");
            var pbcolor2 = new DotNet.Highcharts.Helpers.BackColorOrGradient(pbcolor);
            var pbordercolor = System.Drawing.ColorTranslator.FromHtml("#6495ED");

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("Linechart" + _myChart.ID.ToString()).InitChart(new Chart
            {
                //Width = 500,
                Height = 300,
                Type = ChartTypes.Line,
                BackgroundColor = bgColor,
                BorderColor = borderColor,
                BorderWidth = 1,
                ClassName = "dark-container",
                PlotBackgroundColor = pbcolor2,
                PlotBorderColor = pbordercolor,
                PlotBorderWidth = 1
            });
            Credits MyCredits = new Credits();
            MyCredits.Href = "http://www.livemonitoring.co.za";
            MyCredits.Text = "Live Monitoring";
            chart.SetCredits(MyCredits);

            Title MyTitle = new Title();
            MyTitle.Text = (_myChart.GraphName);
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
            myLegend.BorderWidth = 0;
            myLegend.Floating = true;
            chart.SetLegend(myLegend);

            YAxis MyYaxis2 = new YAxis();

            YAxisTitle MyTitleY2 = new YAxisTitle();

            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            SqlParameter paramfieldid = new SqlParameter("@FieldId", SqlDbType.Int);
            SqlParameter paramsensorid = new SqlParameter("@Sensorid", SqlDbType.Int);
            SqlParameter paramuserDashId = new SqlParameter("@ParameterId", SqlDbType.Int);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.CommandText = "[Dashboards].[spGetFieldHistoryData]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;

            double minvalue = 0;
            double maxvalue = 0;
            double lastvalue = 0;
            XAxis MyXaxis = new XAxis();
            Series[] series = null;
            YAxis[] MyYaxis = null;
            YAxisTitle MyTitleY = new YAxisTitle();
            string FieldName = "";
            string sensor = "";
            string CatDate = "";
            int Value = 0;

            int fieldid = 0;
            int sensorid = 0;
            int dashboardid = 0;

            List<Series> allSeries = new List<Series>();
            for (int i = 0; i <= _myChart.DashBoardParameters.Count() - 1; i++)
            {
                MyTitleY.Text = GetSensorName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                FieldName = GetFieldName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                sensor = MyTitleY.Text;
                sensorid = _myChart.DashBoardParameters[i].SensorID;
                fieldid = _myChart.DashBoardParameters[i].FieldNo;
                dashboardid = _myChart.DashBoardParameters[i].ID;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(paramfieldid).Value = fieldid;
                cmd.Parameters.Add(paramsensorid).Value = sensorid;
                cmd.Parameters.Add(paramuserDashId).Value = dashboardid;
                cmd.CommandTimeout = int.MaxValue;
                SqlDataReader reader = cmd.ExecuteReader();

                int j = 0;
                var results = new List<object[]>();
                while (reader.Read())
                {
                    results.Add(new object[] {
                        reader["Dates"],
                        reader["Value"]
                    });
                }
                reader.Close();
                allSeries.Add(new Series
                {
                    Name = sensor + "-" + FieldName,
                    Data = new DotNet.Highcharts.Helpers.Data(results.ToArray())
                });
            }
            //chart.SetXAxis(MyXaxis)
            chart.SetXAxis(new XAxis
            {
                Type = AxisTypes.Datetime,
                Labels = new XAxisLabels
                {
                    Rotation = -45,
                    Align = HorizontalAligns.Right,
                    Style = "font: 'normal 10px Verdana, sans-serif'"
                },
                Title = new XAxisTitle { Text = "Date" }
            });
            chart.SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Value" } });

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            //load data
            chart.SetSeries(allSeries.Select(s => new Series
            {
                Name = s.Name,
                Data = s.Data
            }).ToArray());
            chart.SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.series.name + ': </b>'+ this.y +' value'; }" });

            ltrChart.Text = chart.ToHtmlString();

        }
        public void Dashboard_liveMonLine()
        {
            Load += Page_Load;
        }
    }
}