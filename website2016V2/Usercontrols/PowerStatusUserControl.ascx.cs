using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI;

namespace website2016V2.Usercontrols
{
    public partial class PowerStatusUserControl : System.Web.UI.UserControl
    {
        private static LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private int _MeterID;
        public int DisplayInterval { get; set; }
        public double ReductionTarget { get; set; }
        public int MeterID
        {
            //GetMeterCaption

            get { return _MeterID; }
            set
            {
                _MeterID = value;
                this.txtCaption.Text = (GetMeterCaption());
            }
        }


        private double HistKwhMeter
        {

            get { return HistKwhMeter; }
            set
            {
                txtHistKwh.Text = value.ToString("0.##");
                txtTargetKwh.Text = (value - (value * ReductionTarget)).ToString();
                if (Convert.ToDouble(txtTargetKwh.Text) > 0)
                {
                    if (Convert.ToDouble(txtTargetKwh.Text) < Convert.ToDouble(txtKwh.Text))
                    {
                        txtKwh.BackColor = System.Drawing.Color.MediumVioletRed;
                    }
                    else
                    {
                        txtKwh.BackColor = System.Drawing.Color.LightGreen;
                    }
                }
            }
        }
        private double HistKVAMeter
        {

            get { return HistKVAMeter; }
            set { txtHistKVA.Text = value.ToString("0.##"); }
        }
        private double HistKVarMeter
        {

            get { return HistKVarMeter; }
            set { txtHistKVar.Text = value.ToString("0.##"); }
        }
        private double HistpfMeter
        {

            get { return HistpfMeter; }
            set { txtHistpf.Text = value.ToString("0.##"); }
        }
        private double AvgKwhMeter
        {

            get { return AvgKwhMeter; }
            set { txtAvgKwh.Text = value.ToString("0.##"); }
        }
        private double AvgKVAMeter
        {

            get { return AvgKVAMeter; }
            set { txtAvgKVA.Text = value.ToString("0.##"); }
        }
        private double AvgKVarMeter
        {

            get { return AvgKVarMeter; }
            set { txtAvgKVar.Text = value.ToString("0.##"); }
        }
        private double AvgpfMeter
        {

            get { return AvgpfMeter; }
            set { txtAvgpf.Text = value.ToString("0.##"); }
        }
        private double KwhMeter
        {

            get { return KwhMeter; }
            set { txtKwh.Text = value.ToString("0.##"); }
        }
        private double KVAMeter
        {

            get { return KVAMeter; }
            set { txtKVA.Text = value.ToString("0.##"); }
        }
        private double KVarMeter
        {

            get { return KVarMeter; }
            set { txtKVar.Text = value.ToString("0.##"); }
        }
        private double pfMeter
        {

            get { return pfMeter; }
            set { txtpf.Text = value.ToString("0.##"); }
        }
        public double UpdateInterval
        {
            get { return MeterRefreshTimer.Interval; }
            //MeterRefreshTimer.Enabled = True
            set {MeterRefreshTimer.Interval = (int)value; }
        }


        public UpdatePanelUpdateMode UpdateMode
        {
            get { return this.MeterInfoUpdatePanel.UpdateMode; }
            set { this.MeterInfoUpdatePanel.UpdateMode = value; }
        }

        public void Update()
        {
            this.MeterInfoUpdatePanel.Update();
        }

        protected void MeterRefreshTimer_Tick(object sender, System.EventArgs e)
        {
            MeterRefreshTimer.Enabled = false;
            try
            {
                this.Update();

            }
            catch (Exception ex)
            {
            }
            MeterRefreshTimer.Enabled = true;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }



        public void StartScanner()
        {
            GetMeterData();
            MeterRefreshTimer.Enabled = true;
        }
        private string GetMeterCaption()
        {
            string MyRet = "";
            try
            {
                string MySql = "";
             
                MySql += "SELECT [Caption]";
                MySql += " FROM sensors ";
                MySql += "where id=" + MeterID.ToString();
                System.Data.SqlClient.SqlDataReader MySQLReader = default(System.Data.SqlClient.SqlDataReader);
                MySQLReader = MyDataAccess.QueryDBSQLStr(MySql);
                if ((MySQLReader == null) == false)
                {
                    while (MySQLReader.Read())
                    {
                        //if details then
                        try
                        {
                            if (Information.IsDBNull(MySQLReader["Caption"]) == false)
                            {
                                MyRet = MySQLReader["Caption"].ToString();
                                //0=id
                            }


                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    MySQLReader.Close();
                }
                MySQLReader = null;
                return MyRet;

            }
            catch (Exception ex)
            {
            }
            return MyRet;
        }
        private void GetMeterData()
        {
            try
            {
                string MySql = "";
                MySql += "SELECT SUM([KVA]) AS SumKVA , SUM([KVar]) AS SumKVar, avg([KVar]) AS avgKVar, avg([KVA]) AS avgKVA";
                MySql += ", SUM([Kwh]) AS SumKwh , avg([Kwh]) AS avgKwh , max([Kwh]) AS maxKwh, min([Kwh]) AS minKwh";
                MySql += ",max([pf]) AS maxpf, min([pf]) AS minpf, avg([pf]) AS avgpf";
                MySql += " FROM [MeteringBillingData] ";
                MySql += "where (TimeStamp>='@StartPlaceholder' and timestamp <='@EndPlaceholder') and meterid=" + MeterID.ToString();
                switch (DisplayInterval)
                {
                    case 0:
                        //hourly
                        this.txtPeriod.Text = "Last Hour";
                        MySql = MySql.Replace("@EndPlaceholder",Convert.ToDateTime(DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd( DateInterval.Hour, -1,Convert.ToDateTime(DateTime.Now)).ToString());
                        break;
                    case 1:
                        //daily
                        this.txtPeriod.Text = "Last Day";
                        MySql = MySql.Replace("@EndPlaceholder", Convert.ToDateTime(DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder", DateAndTime.DateAdd(DateInterval.Day, -1,Convert.ToDateTime(DateAndTime.Now)).ToString());
                        
                        break;
                    case 2:
                        //weekly
                        this.txtPeriod.Text = "Last 7 Days";
                        MySql = MySql.Replace("@EndPlaceholder", Convert.ToDateTime(DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Day, -7, Convert.ToDateTime(DateAndTime.Now)).ToString());
                        break;
                    case 3:
                        //monthly
                        this.txtPeriod.Text = "Last Month";
                        MySql = MySql.Replace("@EndPlaceholder", Convert.ToDateTime(DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Month, -1, Convert.ToDateTime(DateAndTime.Now)).ToString());
                        break;
                    case 4:
                        //yearly
                        this.txtPeriod.Text = "Last Year";
                        MySql = MySql.Replace("@EndPlaceholder", Convert.ToDateTime(DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1, Convert.ToDateTime(DateAndTime.Now)).ToString());
                        break;
                    default:
                        //hour default
                        this.txtPeriod.Text = "Last Hour";
                        MySql = MySql.Replace("@EndPlaceholder", Convert.ToDateTime(DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Hour, -1, Convert.ToDateTime(DateAndTime.Now)).ToString());
                        break;
                }
                System.Data.SqlClient.SqlDataReader MySQLReader = default(System.Data.SqlClient.SqlDataReader);

                MySQLReader = MyDataAccess.QueryDBSQLStr(MySql);
                if ((MySQLReader == null) == false)
                {
                    while (MySQLReader.Read())
                    {
                        //if details then
                        try
                        {
                            if (Information.IsDBNull(MySQLReader["SumKwh"]) == false)
                            {
                                this.KwhMeter =Convert.ToDouble( MySQLReader["SumKwh"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["SumKVar"]) == false)
                            {
                                this.KVarMeter =Convert.ToDouble( MySQLReader["SumKVar"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["SumKVA"]) == false)
                            {
                                this.KVAMeter =Convert.ToDouble( MySQLReader["SumKVA"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["maxpf"]) == false)
                            {
                                this.pfMeter = Convert.ToInt32(MySQLReader["maxpf"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["AvgKwh"]) == false)
                            {
                                this.AvgKwhMeter =Convert.ToDouble(MySQLReader["AvgKwh"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["AvgKVar"]) == false)
                            {
                                this.AvgKVarMeter =Convert.ToDouble( MySQLReader["AvgKVar"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["AvgKVA"]) == false)
                            {
                                this.AvgKVAMeter =Convert.ToDouble( MySQLReader["AvgKVA"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["Avgpf"]) == false)
                            {
                                this.AvgpfMeter = Convert.ToInt32(MySQLReader["Avgpf"]);
                                //0=id
                            }
                            FindHistoricData();



                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    MySQLReader.Close();
                }
                MySQLReader = null;

            }
            catch (Exception ex)
            {
            }

        }
        private void FindHistoricData()
        {
            try
            {
                string MySql = "";
                MySql += "SELECT SUM([KVA]) AS SumKVA , SUM([KVar]) AS SumKVar, avg([KVar]) AS avgKVar, avg([KVA]) AS avgKVA";
                MySql += ", SUM([Kwh]) AS SumKwh , avg([Kwh]) AS avgKwh , max([Kwh]) AS maxKwh, min([Kwh]) AS minKwh";
                MySql += ",max([pf]) AS maxpf, min([pf]) AS minpf, avg([pf]) AS avgpf";
                MySql += " FROM [MeteringBillingData] ";
                MySql += "where (TimeStamp>='@StartPlaceholder' and timestamp <='@EndPlaceholder') and meterid=" + MeterID.ToString();
                switch (DisplayInterval)
                {
                    case 0:
                        //hourly
                        MySql = MySql.Replace("@EndPlaceholder", DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Hour, -1,DateAndTime.DateAdd(DateInterval.Year, -1,DateAndTime.Now)).ToString());
                        break;
                    case 1:
                        //daily
                        MySql = MySql.Replace("@EndPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Day, -1,DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now)).ToString());
                        break;
                    case 2:
                        //weekly
                        MySql = MySql.Replace("@EndPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Day, -7,DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now)).ToString());
                        break;
                    case 3:
                        //monthly
                        MySql = MySql.Replace("@EndPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Month, -1,DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now)).ToString());
                        break;
                    case 4:
                        //yearly
                        MySql = MySql.Replace("@EndPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1,DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now)).ToString());
                        break;
                    default:
                        //hour default
                        MySql = MySql.Replace("@EndPlaceholder",DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now).ToString());
                        MySql = MySql.Replace("@StartPlaceholder",DateAndTime.DateAdd(DateInterval.Hour, -1,DateAndTime.DateAdd(DateInterval.Year, -1, DateAndTime.Now)).ToString());
                        break;
                }
                System.Data.SqlClient.SqlDataReader MySQLReader = default(System.Data.SqlClient.SqlDataReader);

                MySQLReader = MyDataAccess.QueryDBSQLStr(MySql);
                if ((MySQLReader == null) == false)
                {
                    while (MySQLReader.Read())
                    {
                        //if details then
                        try
                        {
                            //History 
                            if (Information.IsDBNull(MySQLReader["SumKwh"]) == false)
                            {
                                this.HistKwhMeter =Convert.ToDouble( MySQLReader["SumKwh"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["SumKVar"]) == false)
                            {
                                this.HistKVarMeter =Convert.ToDouble( MySQLReader["SumKVar"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["SumKVA"]) == false)
                            {
                                this.HistKVAMeter =Convert.ToDouble( MySQLReader["SumKVA"]);
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["maxpf"]) == false)
                            {
                                this.HistpfMeter = Convert.ToInt32(MySQLReader["maxpf"]);
                                //0=id
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    MySQLReader.Close();
                }
                MySQLReader = null;

            }
            catch (Exception ex)
            {
            }


        }
        public PowerStatusUserControl()
        {
            Load += Page_Load;
        }
    }
}