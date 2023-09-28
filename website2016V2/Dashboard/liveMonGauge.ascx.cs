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
	public partial class liveMonGauge : System.Web.UI.UserControl
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

        public void LoadGraph(string sensor, bool caption, string groupname, string captionlabel, string color, string fieldName, double lastvalue, double minvalue, double maxvalue)
        {
            DotNet.Highcharts.Highcharts Linechart = new DotNet.Highcharts.Highcharts("Gaugechart").InitChart(new Chart
            {
                Width = 500,
                Height = 300,
                Type = ChartTypes.Gauge
            });
            XAxis MyXaxis = new XAxis();
            MyXaxis.Categories = new string[] {
                "Min Value",
                "Max Value",
                "Last Value"
            };
            Linechart.SetXAxis(MyXaxis);
            Title MyTitle = new Title();
            MyTitle.Text = (groupname);
            MyTitle.X = -10;
            Linechart.SetTitle(MyTitle);
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
            MyTitleY.Text = (sensor);
            MyYaxis.Title = MyTitleY;
            YAxisPlotLines MyPlotlines = new YAxisPlotLines();
            MyPlotlines.Value = 0;
            MyPlotlines.Width = 1;
            MyPlotlines.Color = ColorTranslator.FromHtml(ActualColor);
            //MyYaxis.Title
            Linechart.SetYAxis(MyYaxis);

            Legend myLegend = new Legend();
            myLegend.Layout = Layouts.Vertical;
            myLegend.Align = HorizontalAligns.Right;
            myLegend.VerticalAlign = VerticalAligns.Top;
            myLegend.X = -1;
            myLegend.Y = 40;
            myLegend.BorderWidth = 1;
            Linechart.SetLegend(myLegend);
            if (lastvalue <= 0)
            {
                lastvalue = 2.1;
            }
            if (minvalue <= 0)
            {
                minvalue = 0.1;
            }
            if (maxvalue <= 0)
            {
                maxvalue = 1.1;
            }
            Series series = new Series();
            series.Name = fieldName;
            series.Data = new DotNet.Highcharts.Helpers.Data(new object[] {
                minvalue,
                maxvalue,
                lastvalue
            });

            Linechart.SetSeries(series);
            Linechart.SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.series.name + ': </b>'+ this.y +' %'; }" });
            ltrChart.Text = Linechart.ToHtmlString();
        }

        public void LoadChart1()
        {
            var colorr = System.Drawing.ColorTranslator.FromHtml("#CCE6FF");
            var bgColor = new DotNet.Highcharts.Helpers.BackColorOrGradient(colorr);
            var borderColor = System.Drawing.ColorTranslator.FromHtml("#6495ED");
            var pbcolor = System.Drawing.ColorTranslator.FromHtml("#F0FFF0");
            var pbcolor2 = new DotNet.Highcharts.Helpers.BackColorOrGradient(pbcolor);
            var pbordercolor = System.Drawing.ColorTranslator.FromHtml("#6495ED");

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("ScatterChart" + _myChart.ID.ToString()).InitChart(new Chart
            {
                //Width = 500,
                Height = 300,
                Type = ChartTypes.Scatter,
                BackgroundColor = bgColor,
                BorderColor = borderColor,
                BorderWidth = 2,
                ClassName = "dark-container",
                PlotBackgroundColor = pbcolor2,
                PlotBorderColor = pbordercolor,
                PlotBorderWidth = 1
            });

            XAxis MyXaxis = new XAxis();

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
            myLegend.BorderWidth = 1;
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

            Series[] series = null;
            YAxis[] MyYaxis = null;
            YAxisTitle MyTitleY = new YAxisTitle();
            string FieldName = "";
            string CatDate = "";
            int Value = 0;
            int fieldid = 0;
            int sensorid = 0;
            int dashboardid = 0;
            for (int i = 0; i <= _myChart.DashBoardParameters.Count() - 1; i++)
            {
                MyTitleY.Text = GetSensorName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
                FieldName = GetFieldName(_myChart.DashBoardParameters[i].FieldNo, _myChart.DashBoardParameters[i].SensorID, _myChart.DashBoardParameters[i].ID);
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

                while (reader.Read())
                {
                    j = j + 1;
                    series = new Series[j];
                    MyYaxis = new YAxis[j];
                    CatDate = reader["Dates"].ToString();
                    Value = (int)reader["Value"];
                    MyXaxis.Categories = new string[] { CatDate };
                    series[i] = new Series
                    {
                        Name = FieldName,
                        Data = new DotNet.Highcharts.Helpers.Data(new object[] { Value })
                    };
                    MyYaxis[i] = new YAxis { Title = MyTitleY };
                }
                reader.Close();
            }

            chart.SetXAxis(MyXaxis);
            chart.SetYAxis(MyYaxis);
            chart.SetSeries(series);
            chart.SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.series.name + ': </b>'+ this.y +' %'; }" });
            ltrChart.Text = chart.ToHtmlString();
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
                Type = ChartTypes.Scatter,
                BackgroundColor = bgColor,
                BorderColor = borderColor,
                BorderWidth = 2,
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
            myLegend.BorderWidth = 1;
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

        public void Dashboard_liveMonGauge()
        {
            Load += Page_Load;
        }
    }
}