using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SensorDetails : System.Web.UI.Page
    {
        private Collection MyIPDevicesCollection = new Collection();
        private Collection MyOtherDevicesCollection = new Collection();
        private Collection MySNMPDevicesCollection = new Collection();
        private Collection MyCameraCollection = new Collection();

        // LiveMonitoring.IRemoteLib ServerInterface;
        List<string> oListXAxis = new List<string>();
        List<object> oListYAxis = new List<object>();
        int zero = 0;
        int one = 1;
        int two = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                LoadSensorPage(Convert.ToInt32(Request.QueryString["SensorID"]));
                GetSensorHistory(Convert.ToInt32(Request.QueryString["SensorID"]), DateAndTime.DateAdd(DateInterval.Minute, -20, DateAndTime.Now), DateTime.Now);
            }
        }

        public void LoadSensorPage(int SensorID)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                this.alert.Text = SensorID.ToString();

                /////Display Sensor Details
                GetSensorByID(SensorID);
                LoadFields();
                LoadSpecificSensor();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void AddRow(string[] RowVals)
        {
            try
            {
                DataRow Row = default(DataRow);
                DataTable dt = new DataTable();
                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                }
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field Name", typeof(string));
                    dt.Columns.Add("Field Suffix", typeof(string));
                    dt.Columns.Add("Field", typeof(int));
                    dt.Columns.Add("Display Field", typeof(bool));
                    dt.Columns.Add("Field Max Val", typeof(double));
                    dt.Columns.Add("Field Min Val", typeof(double));
                    dt.Columns.Add("Field Notes", typeof(string));
                    dt.Columns.Add("Field Max Warn Val", typeof(double));
                    dt.Columns.Add("Field Min Warn Val", typeof(double));
                    dt.Columns.Add("Field Percentage Test", typeof(double));
                }
                Row = dt.NewRow();
                Row[0] = RowVals[0];
                Row[1] = RowVals[1];
                Row[2] = Convert.ToInt32(RowVals[2]);
                Row[3] = Convert.ToBoolean(RowVals[3]);
                try
                {
                    if (Information.UBound(RowVals) > 4)
                    {
                        Row[4] = Convert.ToDouble(RowVals[4]);
                        Row[5] = Convert.ToDouble(RowVals[5]);
                        Row[6] = RowVals[6];
                        if (!string.IsNullOrEmpty(RowVals[7]))
                            Row[7] = Convert.ToDouble(RowVals[7]);
                        else
                            Row[7] = 0;
                        if (!string.IsNullOrEmpty(RowVals[8]))
                            Row[8] = Convert.ToDouble(RowVals[8]);
                        else
                            Row[8] = 0;
                        if (!string.IsNullOrEmpty(RowVals[9]))
                            Row[9] = Convert.ToInt32(RowVals[9]);
                        else
                            Row[8] = 0;
                    }
                    else
                    {
                        Row[4] = 0;
                        Row[5] = 0;
                        Row[6] = "";
                    }
                }
                catch (Exception ex)
                {
                }
                dt.Rows.Add(Row);
                Session["mytable"] = dt;
                GridBind(dt);
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadFields()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            test.LoadfieldNames(cmbType2);
        }

        public void LoadSpecificSensor()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (Convert.ToInt32(Request.QueryString["SensorID"]) == Mysensor.ID)
                    {
                        LiveMonitoring.testing test = new LiveMonitoring.testing();
                        test.droType(cmbType2, MyObject1);

                        cmbType_SelectedIndexChanged(this, new EventArgs());
                    }
                }
            }
        }

        private LiveMonitoring.IRemoteLib.SensorDetails GetSensorByID(int ID)
        {
            try
            {
                LiveMonitoring.IRemoteLib.SensorDetails MySensor = new LiveMonitoring.IRemoteLib.SensorDetails();
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                ArrayList MySensorCollection = new ArrayList();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MySensor = MyRem.LiveMonServer.GetSpecificSensor(ID);
                
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MySensor.Fields)
                {
                    try
                    {
                        if (MyFields.DisplayValue)
                        {
                            //this.cmbType2.Items.FindByText(MyFields.FieldName).Selected = true;
                            this.cmbType2.Text = MyFields.FieldName;
                            //this.txtFieldNumber.Text = MyFields.FieldNumber.ToString();
                            //this.txtFieldPercentOfTest.Text = MyFields.FieldPercentOfTest.ToString();
                            //this.txtFieldLastPercent.Text = MyFields.FieldLastPercent.ToString();
                            //this.txtLastTestRes.Text = MyFields.LastTestRes.ToString();
                            //this.txtCaption.Text = MyFields.Caption.ToString();
                            //this.txtValue.Text = MyFields.LastValue.ToString() + MyFields.Caption;
                            //this.txtExtraValue.Text = MyFields.LastOtherValue.ToString();
                            //this.txtLDRead.Text = MyFields.LastDTRead.ToString();
                            //this.txtSensorStatus.Text = MyFields.FieldStatus.ToString();

                            this.txtSensorID.Text = ID.ToString();
                            this.txtSensorName.Text = MySensor.Caption.ToString();
                            this.txtSensorGroup.Text = MySensor.SensGroup.SensorGroupName.ToString();
                            this.txtScanRate.Text = MySensor.ScanRate.ToString();
                            this.txtMaxValue.Text = MySensor.MaxValue.ToString();
                            this.txtMinValue.Text = MySensor.MinValue.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }         
                }
      
                return MySensor;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void addAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("AddAlert.aspx");
        }

        protected void editAlert_Click(Object sender, EventArgs e)
        {
            Response.Redirect("EditAlerts.aspx");
        }

        protected void LogoutButton_Click(Object sender, EventArgs e)
        {
            Session["LoggedIn"] = "";
            Session["UserDetails"] = "";
            Session.Abandon();
            Response.Redirect("Index.aspx");
        }

        protected void sensorStatus_Click(Object sender, EventArgs e)
        {
            Response.Redirect("SensorStatus.aspx");
        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                add_Rows(Convert.ToInt32(this.cmbType2.SelectedValue));
            }
            catch (Exception ex)
            {
            }
        }

        public void add_Rows(int MyType)
        {
            LiveMonitoring.IRemoteLib.SensorDetails.SensorType MyEnum = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)MyType;
            cmbFields2.Visible = true;
            //cmbFields.Visible = true;
            ClearRows();
            LiveMonitoring.IRemoteLib.SensorFieldsDefault MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDefault(MyEnum);
            if ((MyFields == null) == false)
            {
                AddRows(MyFields.FieldsList);
            }
        }

        public void ClearRows()
        {
            DataTable dt = new DataTable();
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("Field Name", typeof(string));
                dt.Columns.Add("Field Suffix", typeof(string));
                dt.Columns.Add("Field", typeof(int));
                dt.Columns.Add("Display Field", typeof(bool));
                dt.Columns.Add("Field Max Val", typeof(double));
                dt.Columns.Add("Field Min Val", typeof(double));
                dt.Columns.Add("Field Notes", typeof(string));
                dt.Columns.Add("Field Max Warn Val", typeof(double));
                dt.Columns.Add("Field Min Warn Val", typeof(double));
                dt.Columns.Add("Field Percentage Test", typeof(double));
            }
            Session["mytable"] = dt;
            GridBind(dt);
        }

        public void AddRows(List<LiveMonitoring.IRemoteLib.SensorFieldsDef> RowVals)
        {
            try
            {
                DataTable dt = new DataTable();
                if (Session["mytable"] == null == false)
                {
                    dt = (DataTable)Session["mytable"];
                }
             
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field Name", typeof(string));
                    dt.Columns.Add("Field Suffix", typeof(string));
                    dt.Columns.Add("Field", typeof(int));
                    dt.Columns.Add("Display Field", typeof(bool));
                    dt.Columns.Add("Field Max Val", typeof(double));
                    dt.Columns.Add("Field Min Val", typeof(double));
                    dt.Columns.Add("Field Notes", typeof(string));
                    dt.Columns.Add("Field Max Warn Val", typeof(double));
                    dt.Columns.Add("Field Min Warn Val", typeof(double));
                    dt.Columns.Add("Field Percentage Test", typeof(double));
                }
                DataRow Row = default(DataRow);
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in RowVals)
                {
                    Row = dt.NewRow();
                    try
                    {
                        Row[0] = MyField.FieldName;
                        Row[1] = MyField.Caption;
                        Row[2] = MyField.FieldNumber;
                        Row[3] = MyField.DisplayValue;
                        if (!string.IsNullOrEmpty(MyField.FieldMaxValue.ToString()))
                        {
                            Row[4] = MyField.FieldMaxValue;
                        }
                        else
                        {
                            Row[4] = 0;
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldMinValue.ToString()))
                        {
                            Row[5] = MyField.FieldMinValue;
                        }
                        else
                        {
                            Row[5] = 0;
                        }
                        if (MyField.FieldNotes != null)
                        {
                            Row[6] = MyField.FieldNotes;
                        }
                        else
                        {
                            Row[6] = "";
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldMaxWarnValue.ToString()))
                        {
                            Row[7] = MyField.FieldMaxWarnValue;
                        }
                        else
                        {
                            Row[7] = 0;
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldMinWarnValue.ToString()))
                        {
                            Row[8] = MyField.FieldMinWarnValue;
                        }
                        else
                        {
                            Row[8] = 0;
                        }
                        if (!string.IsNullOrEmpty(MyField.FieldPercentOfTest.ToString()))
                        {
                            Row[9] = MyField.FieldPercentOfTest;
                        }
                        else
                        {
                            Row[9] = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    dt.Rows.Add(Row);
                }
                Session["mytable"] = dt;
                GridBind(dt);
            }
            catch (Exception ex)
            {
            }
        }

        public void GridBind(DataTable dt)
        {
            this.cmbFields2.DataSource = dt;
            cmbFields2.DataKeyNames = (new string[] { "Field" });
            cmbFields2.DataBind();
        }

        protected void cmbFields_DataBinding(object sender, System.EventArgs e)
        {
            cmbFields2.PageSize = 5;
            cmbFields2.AllowPaging = true;
        }

        protected void cmbFields_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            cmbFields2.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            cmbFields2.EditIndex = -1;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            cmbFields2.EditIndex = e.NewEditIndex;
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytable"];
            GridBind(dt);
        }

        protected void cmbFields_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            dynamic dt = (DataTable)Session["mytable"];
            dynamic row = cmbFields2.Rows[e.RowIndex];
            dt.Rows[row.DataItemIndex]["Field Name"] = ((TextBox)(row.Cells[1].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Suffix"] = ((TextBox)(row.Cells[2].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Display Field"] = ((CheckBox)(row.Cells[4].Controls[0])).Checked;
            dt.Rows[row.DataItemIndex]["Field Max Val"] = ((TextBox)(row.Cells[5].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Min Val"] = ((TextBox)(row.Cells[6].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Notes"] = ((TextBox)(row.Cells[7].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Max Warn Val"] = ((TextBox)(row.Cells[8].Controls[0])).Text;
            dt.Rows[row.DataItemIndex]["Field Min Warn Val"] = ((TextBox)(row.Cells[9].Controls[0])).Text;

            if (!string.IsNullOrEmpty(((TextBox)(row.Cells[10].Controls[0])).Text))
                dt.Rows[row.DataItemIndex]["Field Percentage Test"] = ((TextBox)(row.Cells[10].Controls[0])).Text;
            cmbFields2.EditIndex = -1;
            Session["mytable"] = dt;
            GridBind(dt);
        }
        protected void editTest_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditSensor.aspx?SensorID="+ Convert.ToInt32(Request.QueryString["SensorID"]));
        }

        public Collection GetSensorHistory(int SensorID, DateTime StartDate, DateTime EndDate)
        {
            Collection MyCollection = new Collection();
            Collection MyRetCollection = new Collection();
            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.LiveMonServer.GetSensorHistory(SensorID, StartDate, EndDate);
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.DataHistory)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.DataHistory MyHistory = (LiveMonitoring.IRemoteLib.DataHistory)MyObject1;
                        string RetString = "";
                        if ((MyHistory.OtherData == null))
                        {
                            RetString = MyHistory.Field.ToString() + "|" + MyHistory.Value.ToString() + "|''|" + MyHistory.DT.ToString() + "|" + MyHistory.Status.ToString();
                            oListXAxis.Add(MyHistory.DT.ToString());
                            oListYAxis.Add(MyHistory.Value.ToString());
                        }
                        else
                        {
                            RetString = MyHistory.Field.ToString() + "|" + MyHistory.Value.ToString() + "|" + MyHistory.OtherData + "|" + MyHistory.DT.ToString() + "|" + MyHistory.Status.ToString();
                            oListXAxis.Add(MyHistory.DT.ToString());
                            oListYAxis.Add(MyHistory.Value.ToString());
                        }
                        MyRetCollection.Add(RetString);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            var color = System.Drawing.ColorTranslator.FromHtml("#CCE6FF");
            var bgColor = new DotNet.Highcharts.Helpers.BackColorOrGradient(color);
            var borderColor = System.Drawing.ColorTranslator.FromHtml("#6495ED");
            var pbcolor = System.Drawing.ColorTranslator.FromHtml("#F0FFF0");
            var pbcolor2 = new DotNet.Highcharts.Helpers.BackColorOrGradient(pbcolor);
            var pbordercolor = System.Drawing.ColorTranslator.FromHtml("#6495ED");

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
              .InitChart(new Chart
              {
                  DefaultSeriesType = ChartTypes.Column,
                  BackgroundColor = bgColor,
                  BorderColor = borderColor,
                  BorderWidth = 2,
                  ClassName = "dark-container",
                  PlotBackgroundColor = pbcolor2,
                  PlotBorderColor = pbordercolor,
                  PlotBorderWidth = 1
              })
              .SetCredits(new Credits
              {
                  Href = "http://www.livemonitoring.co.za",
                  Text = "Live Monitoring",
              })
              .SetTitle(new Title
              {
                  Text = "Sensor History Graph",
                  X = -20
              })
             .SetSubtitle(new Subtitle
             {
                 Text = "For the last 30min/60min/5hrs of data",
                 X = -20
             })
             .SetYAxis(new YAxis
             {

             })
            .SetXAxis(new XAxis
            {
                Categories = oListXAxis.ToArray(),
                Labels = new XAxisLabels
                {
                    Rotation = -45,
                    Align = HorizontalAligns.Right,
                    Style = "font: 'normal 10px Verdana, sans-serif'"
                },
            })
           .SetSeries(new[]
           {
                new Series { Name = txtSensorName.Text, 
                Data = new Data(oListYAxis.ToArray())},
            });
            
            chrtMyChart.Text = chart.ToHtmlString();

            return MyRetCollection;
        }

        protected void dropRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropRange.SelectedValue == zero.ToString())
            {
                GetSensorHistory(Convert.ToInt32(Request.QueryString["SensorID"]), DateAndTime.DateAdd(DateInterval.Minute, -20, DateAndTime.Now), DateTime.Now);
            }
            else if (dropRange.SelectedValue == one.ToString())
            {
                GetSensorHistory(Convert.ToInt32(Request.QueryString["SensorID"]), DateAndTime.DateAdd(DateInterval.Minute, -50, DateAndTime.Now), DateTime.Now);
            }
            else if (dropRange.SelectedValue == two.ToString())
            {
                GetSensorHistory(Convert.ToInt32(Request.QueryString["SensorID"]), DateAndTime.DateAdd(DateInterval.Minute, -100, DateAndTime.Now), DateTime.Now);
            }
        }
    }
}