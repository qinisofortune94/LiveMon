using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Drawing;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources;
using Infragistics.UltraChart.Data.Series;
using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.ColorModel;
using Infragistics.UltraChart.Data;
using Infragistics.UltraChart.Core.Primitives;
using Infragistics.UltraChart.Shared.Events;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI.WebControls;

namespace website2016V2
{
    partial class CustomGraphs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
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

                //ok logged on level ?

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                //If IsNothing(Session["SearchSensor"]) = False And txtSensName.Text = "" Then
                //    txtSensName.Text = CStr(Session["SearchSensor"])
                //End If
                if (IsPostBack == true)
                {
                }
                else
                {
                    Response.Expires = 5;
                    Page.MaintainScrollPositionOnPostBack = true;
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;

                    int MySensorNum = 0;
                    MySensorNum = Convert.ToInt32(Request.QueryString["SensorFieldNum"]);
                    Load_Sensors(MySensorNum);
                    Session["SensorsField"] = "";
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    if (MySensorNum == 0)
                    {
                        //all cameras
                        //Dim MyCollection As New Collection
                        //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session["SelectedSite"]), Nothing, Session["SelectedSite"]))'GetServerObjects 'server1.GetAll()
                        //Dim MyObject1 As Object
                        //Dim MyDiv As Integer = 1
                        //Dim added As Boolean = False
                        //For Each MyObject1 In MyCollection
                        //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                        //        If added = False Then 'only add 1st one
                        //            AddLayer(MyObject1)
                        //            added = True
                        //            Session["SensorsField"]+= MyObject1.ID.ToString + ","
                        //        End If
                        //    End If
                        //Next
                    }
                    else
                    {
                        //specific
                        //Dim MyCollection As New Collection
                        //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session["SelectedSite"]), Nothing, Session["SelectedSite"]))'GetServerObjects 'server1.GetAll()
                        //Dim MyObject1 As Object
                        //Dim MyCnt As Integer = 0
                        //For Each MyObject1 In MyCollection
                        //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                        //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
                        //        If MySensorNum = MySensor.ID Then
                        //            Session["SensorsField"] += MySensor.ID.ToString + ","
                        //            AddLayer(MySensor)
                        //            Exit For
                        //        Else
                        //            MyCnt += 1
                        //        End If
                        //    End If
                        //Next

                    }
                }
                //End If

            }
            else
            {
                Response.Redirect("Index.aspx");
            }

        }
        public void AddPageBreak()
        {
            HtmlGenericControl MyHtml = new HtmlGenericControl();
            MyHtml.InnerHtml = "<div style=\"height:1px\">&nbsp;</div><div style=\"page-break-before: always; height:1px;\">&nbsp;</div>";

            // MyHtml.InnerHtml = "<tr style=""page-break-before: always;"">"
            //this.Charts.Controls.Add(MyHtml);

        }

        public void Load_Sensors(int SelectedID)
        {
            //CameraMenu
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            bool added = false;
            Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.SensorDetails.SensorType));
            Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.SensorDetails.SensorType));
            tvSensors.Nodes.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                try
                {
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        bool AddSens = true;
                        if ((Session["SearchSensor"] == null) == false)
                        {
                            if (MySensor.Caption.ToUpper().Contains(Convert.ToString(Session["SearchSensor"]).ToUpper()) == false)
                            {
                                AddSens = false;
                            }
                        }
                        if (AddSens)
                        {
                            TreeNode node = FindNode(MySensor.SensGroup.SensorGroupName);
                            if ((node == null))
                            {
                                TreeNode node1 = new TreeNode();
                                node1.ShowCheckBox = false;
                                node1.Text = MySensor.SensGroup.SensorGroupName;
                                //Item(CInt(MySensor.Type))
                                node1.Value = MySensor.SensGroup.SensorGroupID.ToString();
                                //CInt(MySensor.Type)
                                node1.Expanded = false;
                                tvSensors.Nodes.Add(node1);
                                node = FindNode(MySensor.SensGroup.SensorGroupName);
                            }

                            TreeNode subnode = new TreeNode();
                            subnode.ShowCheckBox = false;
                            subnode.Text = MySensor.Caption;
                            subnode.Value = MySensor.ID.ToString();
                            subnode.Expanded = false;
                            node.ChildNodes.Add(subnode);
                            //node = FindNode(MySensor.SensGroup.SensorGroupName)
                            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                            {
                                try
                                {
                                    TreeNode subnode1 = new TreeNode();
                                    subnode1.ShowCheckBox = true;
                                    subnode1.Text = MyField.FieldName;
                                    subnode1.Value = MyField.FieldNumber.ToString();
                                    subnode.ChildNodes.Add(subnode1);

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                }

            }

        }
        private TreeNode FindNode(string nodeName)
        {
            try
            {
                for (int mycnt = 0; mycnt <= tvSensors.Nodes.Count - 1; mycnt++)
                {
                    if (tvSensors.Nodes[mycnt].Text == nodeName)
                    {
                        return tvSensors.Nodes[mycnt];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void ultraChart1_ChartDrawItem(object sender, ChartDrawItemEventArgs e)
        {
            try
            {
                if ((e.Primitive == null) == false)
                {
                    if ((e.Primitive.Path == null) == false)
                    {
                        if (e.Primitive.Path.IndexOf("Legend") != -1)
                        {
                            if (e.Primitive.Path.IndexOf("Border") == -1)
                            {
                                e.Primitive.PE.StrokeWidth = 4;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;

                Trace.Write("err" + ex.Message);
            }

        }



        private void SetAxisTypes(ChartLayerAppearance layer)
        {
            switch (layer.ChartType)
            {
                case ChartType.ColumnChart:
                case ChartType.StackColumnChart:
                    layer.AxisX.DataType = AxisDataType.String;
                    layer.AxisX.SetLabelAxisType = SetLabelAxisType.GroupBySeries;
                    layer.AxisY.DataType = AxisDataType.Numeric;
                    layer.AxisY.TickmarkStyle = AxisTickStyle.Smart;
                    break;
                case ChartType.BarChart:
                case ChartType.StackBarChart:
                    layer.AxisY.DataType = AxisDataType.String;
                    layer.AxisY.SetLabelAxisType = SetLabelAxisType.GroupBySeries;
                    layer.AxisX.DataType = AxisDataType.Numeric;
                    layer.AxisX.TickmarkStyle = AxisTickStyle.Smart;
                    layer.AxisX.SetLabelAxisType = SetLabelAxisType.ContinuousData;
                    layer.AxisX.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
                    break;
                case ChartType.LineChart:
                case ChartType.SplineChart:
                case ChartType.AreaChart:
                case ChartType.SplineAreaChart:
                case ChartType.StackLineChart:
                case ChartType.StackSplineChart:
                case ChartType.StackAreaChart:
                case ChartType.StackSplineAreaChart:
                    layer.AxisX.DataType = AxisDataType.String;
                    layer.AxisX.SetLabelAxisType = SetLabelAxisType.ContinuousData;
                    layer.AxisY.DataType = AxisDataType.Numeric;
                    layer.AxisY.TickmarkStyle = AxisTickStyle.Smart;
                    break;
                case ChartType.BubbleChart:
                case ChartType.ScatterChart:
                    layer.AxisX.DataType = AxisDataType.Numeric;
                    layer.AxisY.DataType = AxisDataType.Numeric;
                    layer.AxisX.TickmarkStyle = AxisTickStyle.Smart;
                    layer.AxisY.TickmarkStyle = AxisTickStyle.Smart;
                    break;
                case ChartType.RadarChart:
                case ChartType.PieChart:
                case ChartType.DoughnutChart:
                case ChartType.PolarChart:
                    break;
                case ChartType.CandleChart:
                    layer.AxisX.DataType = AxisDataType.String;
                    layer.AxisX.SetLabelAxisType = SetLabelAxisType.DateData;
                    layer.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;
                    layer.AxisY.DataType = AxisDataType.Numeric;
                    layer.AxisY2.DataType = AxisDataType.Numeric;
                    layer.AxisY.TickmarkStyle = AxisTickStyle.Smart;
                    layer.AxisY2.TickmarkStyle = AxisTickStyle.Smart;
                    break;
                case ChartType.ParetoChart:
                    // Create an Y2 axis
                    AxisItem yAxis2 = new AxisItem();
                    yAxis2.axisNumber = AxisNumber.Y2_Axis;
                    yAxis2.Labels.ItemFormatString = "<DATA_VALUE:#0.0#>";
                    yAxis2.RangeType = AxisRangeType.Custom;
                    yAxis2.RangeMax = 1000;

                    // Add the second Y axes to the ChartArea
                    layer.ChartArea.Axes.Add(yAxis2);

                    // Set the second Y axes
                    layer.AxisY2 = yAxis2;

                    layer.AxisX.DataType = AxisDataType.String;
                    layer.AxisX.SetLabelAxisType = SetLabelAxisType.ContinuousData;
                    layer.AxisY.DataType = AxisDataType.Numeric;
                    layer.AxisY.TickmarkStyle = AxisTickStyle.Smart;
                    layer.AxisY2.DataType = AxisDataType.Numeric;
                    layer.AxisY2.TickmarkStyle = AxisTickStyle.Smart;
                    break;
                case ChartType.StepLineChart:
                case ChartType.StepAreaChart:
                    layer.AxisX.DataType = AxisDataType.Time;
                    layer.AxisY.DataType = AxisDataType.Numeric;
                    break;
                case ChartType.GanttChart:
                    layer.AxisX.DataType = AxisDataType.Time;
                    layer.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;
                    layer.AxisY.DataType = AxisDataType.String;
                    break;
            }

        }

        protected void btnGenerateSetnRangers_Click(object sender, System.EventArgs e)
        {
            lblError.Visible = false;
            switch (this.ddlDefaultRangers.SelectedValue)
            {
                case "0":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "1":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -1, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "2":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -2, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "3":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -3, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "4":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -5, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "5":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -10, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "6":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -12, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "7":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -24, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "8":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Day, -2, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "9":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Day, -4, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "10":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Day, -7, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case "11":
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Month, -1, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
            }
            RegenerateCallbackGraphs();

        }
        protected void btnGenerateDateRange_Click(object sender, System.EventArgs e)
        {
            lblError.Visible = false;

            if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select start/end date.";

                return;
            }
            Session["StartDate"] = txtStartDate.Text;
            Session["EndDate"] = txtEndDate.Text;
            RegenerateCallbackGraphs();
        }
        public void FillSessionSensors()
        {
            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
            Collection MyCollection = new Collection();
            //clear session var
            Session["SensorsField"] = "";
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            foreach (TreeNode Mynode in tvSensors.Nodes)
            {
                foreach (TreeNode subnode in Mynode.ChildNodes)
                {
                    foreach (TreeNode subSubnode in subnode.ChildNodes)
                    {
                        if (subSubnode.Checked)
                        {
                            Session["SensorsField"] += subnode.Value.ToString() + ":" + subSubnode.Value.ToString() + ",";
                        }
                    }

                }
            }


        }

        public LiveMonitoring.IRemoteLib.SensorDetails FindSensor(int sensorID)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects();
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (sensorID == MySensor.ID)
                        {
                            return MySensor;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return null;

        }
        //
        public void RegenerateCallbackGraphs()
        {
            this.Charts.Controls.Clear();
            FillSessionSensors();
            string[] mySensorsFields = Strings.Split((string)Session["SensorsField"], ",");
            int Acnt = 0;

            //Depending on the graphtype selected
            switch (this.ddlDropDownChartType.SelectedValue)
            {
                case "0":
                    //Line Chart Overlay
                    Infragistics.WebUI.UltraWebChart.UltraChart SensorChart = CreateSimpleLineGraph();
                    foreach (string MySensorField in mySensorsFields)
                    {
                        string[] SensField = MySensorField.Split(':');
                        int CurSens = 0;
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        Collection MyData = null;
                        if ((SensField == null) == false)
                        {
                            if (!string.IsNullOrEmpty(SensField[0]))
                            {
                                //reload datastream for each sensor
                                if (CurSens != Convert.ToInt32(SensField[0]))
                                {
                                    MySensor = FindSensor(Convert.ToInt32(SensField[0]));

                                    if ((MySensor == null) == false)
                                    {
                                        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                        try
                                        {
                                            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true)
                                            {
                                                if (ddlDataSet.SelectedValue == "0")
                                                {
                                                    MyData = MyRem.LiveMonServer.GetSensorHistory(Convert.ToInt32(SensField[0]), Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                                                }
                                                else
                                                {
                                                    MyData = MyRem.LiveMonServer.GetFilteredSensorHistory(Convert.ToInt32(ddlDataSet.SelectedValue), Convert.ToInt32(SensField[0]), Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                                                }
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    CurSens = Convert.ToInt32(SensField[0]);
                                }
                                AddNewLineToGraph(ref SensorChart, MyData, Convert.ToInt32(SensField[0]), Convert.ToInt32(SensField[1]), MySensor);
                            }
                        }

                    }

                    this.Charts.Controls.Add(SensorChart);
                    break;
                case "1":
                    //Line Chart by Sensor
                    Infragistics.WebUI.UltraWebChart.UltraChart SensorCharts = CreateSimpleLineGraph();
                    bool FirstScan = true;
                    foreach (string MySensorField in mySensorsFields)
                    {
                        string[] SensField = MySensorField.Split(':');
                        int CurSens = 0;
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        Collection MyData = null;

                        if ((SensField == null) == false)
                        {
                            if (!string.IsNullOrEmpty(SensField[0]))
                            {
                                //reload datastream for each sensor
                                if (CurSens != Convert.ToInt32(SensField[0]))
                                {
                                    if (FirstScan == false)
                                    {
                                        this.Charts.Controls.Add(SensorCharts = CreateSimpleLineGraph(MySensor.Caption));
                                        //create new Chart
                                        SensorCharts = CreateSimpleLineGraph(MySensor.Caption);
                                    }
                                    MySensor = FindSensor(Convert.ToInt32(SensField[0]));

                                    if ((MySensor == null) == false)
                                    {
                                        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                        try
                                        {
                                            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true)
                                            {
                                                if (ddlDataSet.SelectedValue == "0")
                                                {
                                                    MyData = MyRem.LiveMonServer.GetSensorHistory(Convert.ToInt32(SensField[0]), Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                                                }
                                                else
                                                {
                                                    MyData = MyRem.LiveMonServer.GetFilteredSensorHistory(Convert.ToInt32(ddlDataSet.SelectedValue), Convert.ToInt32(SensField[0]), Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                                                }
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    CurSens = Convert.ToInt32(SensField[0]);
                                    AddNewLineToGraph(ref SensorCharts, MyData, Convert.ToInt32(SensField[0]), Convert.ToInt32(SensField[1]), MySensor);
                                    FirstScan = false;
                                }
                                else
                                {
                                    AddNewLineToGraph(ref SensorCharts, MyData, Convert.ToInt32(SensField[0]), Convert.ToInt32(SensField[1]), MySensor);
                                }

                            }
                        }

                    }

                    //add last data
                    this.Charts.Controls.Add(SensorCharts);
                    break;
                case "2":
                    //>ON/OFF Bar
                    //DrawDryContactOnOffGraphs(SensorDet, MyData)
                    Infragistics.WebUI.UltraWebChart.UltraChart SensorChartss = CreateSimpleBarGraph();
                    bool FirstScans = true;
                    foreach (string MySensorField in mySensorsFields)
                    {
                        string[] SensField = MySensorField.Split(':');
                        int CurSens = 0;
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        Collection MyData = null;

                        if ((SensField == null) == false)
                        {
                            if (!string.IsNullOrEmpty(SensField[0]))
                            {
                                //reload datastream for each sensor
                                if (CurSens != Convert.ToInt32(SensField[0]))
                                {
                                    if (FirstScans == false)
                                    {
                                        this.Charts.Controls.Add(SensorChartss);
                                        //create new Chart
                                        SensorChart = CreateSimpleBarGraph(MySensor.Caption);
                                    }
                                    MySensor = FindSensor(Convert.ToInt32(SensField[0]));

                                    if ((MySensor == null) == false)
                                    {
                                        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                        try
                                        {
                                            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true)
                                            {
                                                if (ddlDataSet.SelectedValue == "0")
                                                {
                                                    MyData = MyRem.LiveMonServer.GetSensorHistory(Convert.ToInt32(SensField[0]), Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                                                }
                                                else
                                                {
                                                    MyData = MyRem.LiveMonServer.GetFilteredSensorHistory(Convert.ToInt32(ddlDataSet.SelectedValue), Convert.ToInt32(SensField[0]), Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                                                }
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    CurSens = Convert.ToInt32(SensField[0]);
                                    AddNewBarOnOffToGraph(ref SensorChartss, MyData, Convert.ToInt32(SensField[0]), Convert.ToInt32(SensField[1]), MySensor);
                                    FirstScan = false;
                                }
                                else
                                {
                                    AddNewBarOnOffToGraph(ref SensorChartss, MyData, Convert.ToInt32(SensField[0]), Convert.ToInt32(SensField[1]), MySensor);
                                }

                            }
                        }

                    }

                    //add last data
                    this.Charts.Controls.Add(SensorChartss);
                    break;
                case "3":
                    //Bar Per Sensor
                    break;

                case "4":
                    //Pie Sliced Per Sensor Total
                    break;

                case "5":
                    //Pie Sliced Per Sensor Counter
                    break;
            }
        }
        //
        public void DrawDiscreteOnOffGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            Infragistics.WebUI.UltraWebChart.UltraChart Mychart = new Infragistics.WebUI.UltraWebChart.UltraChart();
            Mychart.ID = "DiscOnOff" + SensorDet.ID.ToString();
            Mychart.TitleTop.Extent = 45;
            //sets the font color
            Mychart.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            Mychart.TitleTop.FontSizeBestFit = false;
            //sets the horizontal alignment of the text
            Mychart.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            Mychart.TitleTop.Margins.Bottom = 2;
            Mychart.TitleTop.Margins.Top = 2;
            Mychart.TitleTop.Margins.Left = 2;
            Mychart.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            Mychart.TitleTop.Text = SensorDet.Caption;
            //sets the vertical alignment of the title
            Mychart.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            Mychart.TitleTop.Visible = true;
            //wrap/don't wrap the text
            Mychart.TitleTop.WrapText = true;
            // Set composite charts
            Mychart.ChartType = ChartType.Composite;
            // Create the ChartArea
            ChartArea myChartArea = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            Mychart.CompositeChart.ChartAreas.Add(myChartArea);
            // Create the ChartLayer
            ChartLayerAppearance chartLayer = new ChartLayerAppearance();
            // Set the ChartType
            chartLayer.ChartType = ChartType.GanttChart;
            // Create an X axis
            AxisItem xAxis = new AxisItem();
            xAxis.axisNumber = AxisNumber.X_Axis;
            xAxis.DataType = AxisDataType.String;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.Labels.Font = new Font("Tahoma", 7);
            xAxis.LineThickness = 1;
            // Create an Y axis
            AxisItem yAxis = new AxisItem();
            yAxis.axisNumber = AxisNumber.Y_Axis;
            yAxis.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            yAxis.Labels.Font = new Font("Tahoma", 7);
            yAxis.LineThickness = 1;
            myChartArea.Axes.Add(xAxis);
            myChartArea.Axes.Add(yAxis);
            // Set the axes
            chartLayer.AxisX = xAxis;
            chartLayer.AxisY = yAxis;
            Mychart.Tooltips.Font.Name = "Arial";
            Mychart.Tooltips.Font.Size = 8;
            Mychart.Tooltips.Overflow = TooltipOverflow.ChartArea;
            Mychart.Tooltips.Display = TooltipDisplay.MouseMove;
            Mychart.Tooltips.Format = TooltipStyle.Custom;
            Mychart.Tooltips.FormatString = "<START_TIME:yyyy-MM-dd hh:mm:ss> - <END_TIME:yyyy-MM-dd hh:mm:ss>";
            Mychart.Tooltips.Font.Name = "Arial";
            Mychart.Tooltips.Font.Size = 8;
            Mychart.Tooltips.Overflow = TooltipOverflow.ChartArea;
            Mychart.Tooltips.Display = TooltipDisplay.MouseMove;
            Mychart.Tooltips.Format = TooltipStyle.Custom;
            Mychart.Tooltips.FormatString = "<START_TIME:yyyy-MM-dd hh:mm:ss> - <END_TIME:yyyy-MM-dd hh:mm:ss>";

            // Set the ChartArea
            chartLayer.ChartArea = myChartArea;
            // Add the ChartLayer to the ChartLayers collection
            Mychart.CompositeChart.ChartLayers.Add(chartLayer);
            // Create Series
            ISeries series = null;
            GanttDataSource ganttData = new GanttDataSource();
            GanttSeries ganttSeries = ganttData.Series.Add("Series A");
            ganttSeries.Label = "";

            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];

            for (tmpcntwe1 = 1; tmpcntwe1 <= SensorDet.Fields.Count; tmpcntwe1++)
            {
                tmpcntwe = tmpcntwe1;
                ChartColors[tmpcntwe1 - 1] = Color.Green;
                string[] Dates = GenerateDiscreteONOffStartEndData(tmpcntwe, MyData);
                GanttItem task1a = default(GanttItem);
                if (SensorDet.Fields.Contains(tmpcntwe1.ToString()) == true)
                {
                    task1a = ganttSeries.Items.Add(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName);
                }
                else
                {
                    task1a = ganttSeries.Items.Add("Unk");
                }
                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1)
                {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++)
                    {
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt)
                        {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt]))
                            {
                                endtime = EndArray[loopcnt];
                            }
                            else
                            {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime))
                                {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        }
                        else
                        {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt]))
                        {
                            task1a.Times.Add(DateTime.Parse(StartArray[loopcnt]), DateTime.Parse(endtime));
                            task1a.Times[0].ID = 0;
                            task1a.Times[0].LinkToID = 1;
                            task1a.Times[0].PercentComplete = 1;
                            task1a.Times[0].Owner = "Worker A";
                        }
                    }
                }
            }
            //roger data end
            series = ganttSeries;
            // Set X axis
            xAxis.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            // Set Y axis
            yAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            // Set the ChartType
            chartLayer.ChartType = ChartType.GanttChart;
            // Set Axis Type
            SetAxisTypes(chartLayer);
            chartLayer.AxisY.Extent = 130;
            // Add the series to the ChartLayer's Series collection.
            chartLayer.Series.Add(series);
            Mychart.Series.Add(series);
            Mychart.Width = 700;
            Mychart.Height = 150;
            Mychart.ColorModel.ModelStyle = ColorModels.CustomLinear;
            Mychart.ColorModel.CustomPalette = ChartColors;
            this.Charts.Controls.Add(Mychart);

        }
        public string[] GenerateDiscreteONOffStartEndData(int tmpcntwe, Collection MyData)
        {
            int mytmpcnt = 0;
            string[] Dates = new string[2];
            //0=Start 1=End seperate with ,
            int tmp1cntwe = 0;
            var AMFoutputStateSwitch = false;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
            {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyDataHistory.Field == tmpcntwe)
                {
                    long Pattern = 1;
                    long myrestmp = 0;
                    //= 1 << mytmpcnt
                    myrestmp = Pattern << mytmpcnt;
                    //this bit is off so on disply
                    if (((long)MyDataHistory.Value & myrestmp) == 0)
                    {
                        //changed add startdate
                        if (AMFoutputStateSwitch == false)
                        {
                            Dates[0] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = true;
                        }
                    }
                    else
                    {
                        //changed add enddate
                        if (AMFoutputStateSwitch == true)
                        {
                            Dates[1] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = false;
                        }
                    }
                }
            }
            return Dates;
        }
        public Infragistics.WebUI.UltraWebChart.UltraChart CreateSimpleBarGraph(string ChartID = "")
        {
            // Dim SensorChart As New Infragistics.WebUI.UltraWebChart.UltraChart

            //roger Newline
            // SensorChart = New Infragistics.WebUI.UltraWebChart.UltraChart
            //attach event for legent width
            // AddHandler SensorChart.ChartDrawItem, AddressOf ultraChart1_ChartDrawItem
            // SensorChart.ID = "LineChart" + ChartID

            Infragistics.WebUI.UltraWebChart.UltraChart Mychart = new Infragistics.WebUI.UltraWebChart.UltraChart();
            Mychart.ID = "DiscOnOff" + ChartID;
            Mychart.TitleTop.Extent = 45;
            //sets the font color
            Mychart.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            Mychart.TitleTop.FontSizeBestFit = false;
            //sets the horizontal alignment of the text
            Mychart.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            Mychart.TitleTop.Margins.Bottom = 2;
            Mychart.TitleTop.Margins.Top = 2;
            Mychart.TitleTop.Margins.Left = 2;
            Mychart.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            Mychart.TitleTop.Text = "BarGraph:" + ChartID;
            //sets the vertical alignment of the title
            Mychart.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            Mychart.TitleTop.Visible = true;
            //wrap/don't wrap the text
            Mychart.TitleTop.WrapText = true;
            // Set composite charts
            Mychart.ChartType = ChartType.Composite;
            // Create the ChartArea

            // Create Series
            //Dim series As ISeries = Nothing
            //Dim ganttData As New GanttDataSource()
            //Dim ganttSeries As GanttSeries = ganttData.Series.Add("Series A")
            //ganttSeries.Label = ""
            Color[] ChartColors = new Color[41];
            ChartColors[0] = Color.Green;
            ChartColors[1] = Color.Green;
            //' ''roger data
            // ''Dim tmpcntwe As Integer = 0
            // ''Dim tmpcntwe1 As Integer = 0
            // ''Dim ChartColors(40) As Color

            // ''For tmpcntwe1 = 1 To SensorDet.Fields.Count
            // ''    tmpcntwe = tmpcntwe1
            // ''    ChartColors(tmpcntwe1 - 1) = Color.Green
            // ''    Dim Dates() As String = GenerateDiscreteONOffStartEndData(tmpcntwe, MyData)
            // ''    Dim task1a As GanttItem
            // ''    If SensorDet.Fields.Contains(tmpcntwe1.ToString) = True Then
            // ''        task1a = ganttSeries.Items.Add(CType(SensorDet.Fields(tmpcntwe), LiveMonitoring.IRemoteLib.SensorFieldsDef).FieldName)
            // ''    Else
            // ''        task1a = ganttSeries.Items.Add("Unk")
            // ''    End If
            // ''    Dim LastEnd As Integer = 0
            // ''    Dim StartArray() As String = Split(Dates(0), ",")
            // ''    Dim EndArray() As String = Split(Dates(1), ",")
            // ''    Dim loopcnt As Integer
            // ''    If UBound(StartArray) > -1 Then
            // ''        For loopcnt = 0 To UBound(StartArray)
            // ''            Dim endtime As String
            // ''            If UBound(EndArray) >= loopcnt Then
            // ''                If EndArray(loopcnt) <> "" Then
            // ''                    endtime = EndArray(loopcnt)
            // ''                Else
            // ''                    endtime = Me.endrange.Text
            // ''                    If endtime = "" Then
            // ''                        endtime = Now.ToString
            // ''                    End If
            // ''                End If
            // ''            Else
            // ''                endtime = Me.endrange.Text
            // ''            End If
            // ''            If StartArray(loopcnt) <> "" Then
            // ''                task1a.Times.Add(DateTime.Parse(StartArray(loopcnt)), DateTime.Parse(endtime))
            // ''                task1a.Times[0].ID = 0
            // ''                task1a.Times[0].LinkToID = 1
            // ''                task1a.Times[0].PercentComplete = 1
            // ''                task1a.Times[0].Owner = "Worker A"
            // ''            End If
            // ''        Next
            // ''    End If
            // ''Next
            //' ''roger data end
            //   series = ganttSeries
            // Set X axis

            // Add the series to the ChartLayer's Series collection.
            // chartLayer.Series.Add(series)
            //  Mychart.Series.Add(series)
            Mychart.Width = 700;
            Mychart.Height = 150;
            Mychart.ColorModel.ModelStyle = ColorModels.CustomLinear;
            Mychart.ColorModel.CustomPalette = ChartColors;
            return Mychart;

        }
        public void AddNewBarOnOffToGraph(ref Infragistics.WebUI.UltraWebChart.UltraChart SensorChart, Collection MyData, int SensorID, int SensorField, LiveMonitoring.IRemoteLib.SensorDetails MySensor)
        {
            ChartArea SensorChartArea = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            SensorChart.CompositeChart.ChartAreas.Add(SensorChartArea);
            // Create the ChartLayer
            ChartLayerAppearance chartLayer = new ChartLayerAppearance();
            // Set the ChartType
            chartLayer.ChartType = ChartType.GanttChart;
            // Create an X axis
            AxisItem xAxis = new AxisItem();
            xAxis.axisNumber = AxisNumber.X_Axis;
            xAxis.DataType = AxisDataType.String;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            xAxis.Labels.Font = new Font("Tahoma", 7);
            xAxis.LineThickness = 1;
            // Create an Y axis
            AxisItem yAxis = new AxisItem();
            yAxis.axisNumber = AxisNumber.Y_Axis;
            yAxis.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            yAxis.Labels.Font = new Font("Tahoma", 7);
            yAxis.LineThickness = 1;
            SensorChartArea.Axes.Add(xAxis);
            SensorChartArea.Axes.Add(yAxis);
            // Set the axes
            chartLayer.AxisX = xAxis;
            chartLayer.AxisY = yAxis;
            SensorChart.Tooltips.Font.Name = "Arial";
            SensorChart.Tooltips.Font.Size = 8;
            SensorChart.Tooltips.Overflow = TooltipOverflow.ChartArea;
            SensorChart.Tooltips.Display = TooltipDisplay.MouseMove;
            SensorChart.Tooltips.Format = TooltipStyle.Custom;
            SensorChart.Tooltips.FormatString = "<START_TIME:yyyy-MM-dd hh:mm:ss> - <END_TIME:yyyy-MM-dd hh:mm:ss>";
            SensorChart.Tooltips.Font.Name = "Arial";
            SensorChart.Tooltips.Font.Size = 8;
            SensorChart.Tooltips.Overflow = TooltipOverflow.ChartArea;
            SensorChart.Tooltips.Display = TooltipDisplay.MouseMove;
            SensorChart.Tooltips.Format = TooltipStyle.Custom;
            SensorChart.Tooltips.FormatString = "<START_TIME:yyyy-MM-dd hh:mm:ss> - <END_TIME:yyyy-MM-dd hh:mm:ss>";

            // Set the ChartArea
            chartLayer.ChartArea = SensorChartArea;
            // Add the ChartLayer to the ChartLayers collection
            SensorChart.CompositeChart.ChartLayers.Add(chartLayer);
            // Create Series
            ISeries series = null;
            GanttDataSource ganttData = new GanttDataSource();
            GanttSeries ganttSeries = ganttData.Series.Add("Series A");
            ganttSeries.Label = "";
            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            //Dim ChartColors(40) As Color

            for (tmpcntwe1 = 1; tmpcntwe1 <= MySensor.Fields.Count; tmpcntwe1++)
            {
                if (SensorField == tmpcntwe1)
                {
                    tmpcntwe = tmpcntwe1;
                    //ChartColors(tmpcntwe1 - 1) = Color.Green
                    string[] Dates = GenerateDiscreteONOffStartEndData(tmpcntwe, MyData);
                    GanttItem task1a = default(GanttItem);
                    if (MySensor.Fields.Contains(tmpcntwe1.ToString()) == true)
                    {
                        task1a = ganttSeries.Items.Add(((LiveMonitoring.IRemoteLib.SensorFieldsDef)MySensor.Fields[tmpcntwe]).FieldName);
                    }
                    else
                    {
                        task1a = ganttSeries.Items.Add("Unk");
                    }
                    int LastEnd = 0;
                    string[] StartArray = Strings.Split(Dates[0], ",");
                    string[] EndArray = Strings.Split(Dates[1], ",");
                    int loopcnt = 0;
                    if (Information.UBound(StartArray) > -1)
                    {
                        for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++)
                        {
                            string endtime = null;
                            if (Information.UBound(EndArray) >= loopcnt)
                            {
                                if (!string.IsNullOrEmpty(EndArray[loopcnt]))
                                {
                                    endtime = EndArray[loopcnt];
                                }
                                else
                                {
                                    endtime = this.txtEndDate.Text;
                                    if (string.IsNullOrEmpty(endtime))
                                    {
                                        endtime = DateAndTime.Now.ToString();
                                    }
                                }
                            }
                            else
                            {
                                endtime = this.txtEndDate.Text;
                            }
                            if (!string.IsNullOrEmpty(StartArray[loopcnt]))
                            {
                                task1a.Times.Add(DateTime.Parse(StartArray[loopcnt]), DateTime.Parse(endtime));
                                task1a.Times[0].ID = 0;
                                task1a.Times[0].LinkToID = 1;
                                task1a.Times[0].PercentComplete = 1;
                                task1a.Times[0].Owner = "Worker A";
                            }
                        }
                    }
                }

            }
            //roger data end

            series = ganttSeries;
            // Set X axis
            // Set Y axis
            // Set the ChartType
            // Set Axis Type
            // Add the series to the ChartLayer's Series collection.
            xAxis.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            xAxis.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            // Set Y axis
            yAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            // Set the ChartType
            chartLayer.ChartType = ChartType.GanttChart;
            // Set Axis Type
            SetAxisTypes(chartLayer);
            chartLayer.AxisY.Extent = 130;
            // Add the series to the ChartLayer's Series collection.
            chartLayer.Series.Add(series);

            //ChartLayer.Series.Add(Series) '???????????????????????
            SensorChart.Series.Add(series);

        }

        public Infragistics.WebUI.UltraWebChart.UltraChart CreateSimpleLineGraph(string ChartID = "")
        {
            Infragistics.WebUI.UltraWebChart.UltraChart SensorChart = new Infragistics.WebUI.UltraWebChart.UltraChart();

            //roger Newline
            SensorChart = new Infragistics.WebUI.UltraWebChart.UltraChart();
            //attach event for legent width
            SensorChart.ChartDrawItem += ultraChart1_ChartDrawItem;
            SensorChart.ID = "LineChart" + ChartID;
            //The following appleis to all titles, titletop is simply used for the example
            //sets the height or width of space for the title
            SensorChart.TitleTop.Extent = 45;
            //sets the font color
            SensorChart.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            SensorChart.TitleTop.FontSizeBestFit = false;
            //sets the horizontal alignment of the text
            SensorChart.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            SensorChart.TitleTop.Margins.Bottom = 2;
            SensorChart.TitleTop.Margins.Top = 2;
            SensorChart.TitleTop.Margins.Left = 2;
            SensorChart.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            SensorChart.TitleTop.Text = "LineChart";
            //sets the vertical alignment of the title
            SensorChart.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            SensorChart.TitleTop.Visible = true;
            //wrap/don't wrap the text
            SensorChart.TitleTop.WrapText = true;
            // Set composite charts
            SensorChart.ChartType = ChartType.Composite;
            //SensorChart.Tooltips.Format = TooltipStyle.Custom
            //SensorChart.Tooltips.Display = TooltipDisplay.MouseMove
            //SensorChart.Tooltips.FormatString = "My Data: <DATA_VALUE:$#0.00>"

            // Create the ChartArea
            ChartArea CTmyDLChartArea = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            SensorChart.CompositeChart.ChartAreas.Add(CTmyDLChartArea);
            // Create the ChartLayerSensorChart

            // Create the ChartLayer
            ChartLayerAppearance CTDLchartLayer = new ChartLayerAppearance();
            // Set the ChartType depending on sensor type
            CTDLchartLayer.ChartType = ChartType.LineChart;
            // Create an X axis
            AxisItem CTDLxAxis = new AxisItem();
            CTDLxAxis.axisNumber = AxisNumber.X_Axis;
            CTDLxAxis.DataType = AxisDataType.Time;
            CTDLxAxis.Labels.ItemFormatString = "<ITEM_LABEL>";
            CTDLxAxis.Labels.Font = new Font("Tahoma", 7);
            CTDLxAxis.LineThickness = 1;
            // Create an Y axis
            AxisItem CTDLyAxis = new AxisItem();
            CTDLyAxis.axisNumber = AxisNumber.Y_Axis;
            CTDLyAxis.DataType = AxisDataType.Numeric;
            CTDLyAxis.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            CTDLyAxis.Labels.Font = new Font("Tahoma", 7);
            CTDLyAxis.LineThickness = 1;
            CTmyDLChartArea.Axes.Add(CTDLxAxis);
            CTmyDLChartArea.Axes.Add(CTDLyAxis);
            // Set the axes
            CTDLchartLayer.AxisX = CTDLxAxis;
            CTDLchartLayer.AxisY = CTDLyAxis;
            // Set the ChartArea
            CTDLchartLayer.ChartArea = CTmyDLChartArea;
            // Add the ChartLayer to the ChartLayers collection
            SensorChart.CompositeChart.ChartLayers.Add(CTDLchartLayer);

            //' '' '' Create Series
            // '' ''Dim ctDLseries As ISeries = Nothing
            // '' ''Dim ctnumericTimeSeries(15) As NumericTimeSeries
            // '' ''Dim Bcnt As Integer
            // '' ''For Bcnt = 0 To 14
            // '' ''    ctnumericTimeSeries(Bcnt) = New NumericTimeSeries
            // '' ''    If SensorDet.Fields.Contains((Bcnt + 1).ToString) = True Then
            // '' ''        ctnumericTimeSeries(Bcnt).Label = CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).FieldName
            // '' ''    Else
            // '' ''        ctnumericTimeSeries(Bcnt).Label = "Unknown"
            // '' ''    End If
            // '' ''Next
            // '' ''Dim tmp1cntwe As Integer = 0
            //' '' ''mycnt1= fields ?
            // '' ''Dim MyDataHistory As LiveMonitoring.IRemoteLib.DataHistory
            // '' ''Dim MaxFieldCnt As Integer = 0
            // '' ''For Each MyDataHistory In MyData
            // '' ''    Dim MyField As Integer = MyDataHistory.Field - 1 'field always starts at 1
            // '' ''    If MyField > MaxFieldCnt Then
            // '' ''        MaxFieldCnt = MyField
            // '' ''    End If
            // '' ''    Dim myret As Integer
            // '' ''    myret = ctnumericTimeSeries(MyField).Points.Add(New NumericTimeDataPoint())
            // '' ''    ctnumericTimeSeries(MyField).Points(myret).TimeValue = MyDataHistory.DT
            // '' ''    ctnumericTimeSeries(MyField).Points(myret).NumericValue = MyDataHistory.Value
            // '' ''Next
            // '' ''Dim Acnt As Integer
            // '' ''For Acnt = 0 To MaxFieldCnt
            // '' ''    CTDLchartLayer.Series.Add(ctnumericTimeSeries(Acnt))
            // '' ''    ctDLseries = ctnumericTimeSeries(Acnt)
            // '' ''    SensorChart.Series.Add(ctDLseries)
            // '' ''Next
            // Set X axis
            CTDLxAxis.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            CTDLxAxis.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            // Set Y axis
            // Set the ChartType
            CTDLchartLayer.ChartType = ChartType.LineChart;
            // Set Axis Type
            SetAxisTypes(CTDLchartLayer);
            CTDLchartLayer.AxisX.DataType = AxisDataType.Time;
            CTDLchartLayer.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            CTDLchartLayer.AxisY.DataType = AxisDataType.Numeric;
            // Add the series to the ChartLayer's Series collection.
            SensorChart.Width = 700;
            SensorChart.Height = 500;

            CompositeLegend legend = new CompositeLegend();
            legend.LabelStyle.Font = new Font("Times New Roman", 10);
            SensorChart.CompositeChart.Legends.Add(legend);
            legend.ChartLayers.Add(SensorChart.CompositeChart.ChartLayers[0]);
            legend.BoundsMeasureType = MeasureType.Percentage;
            legend.Bounds = new Rectangle(30, 5, 56, 14);
            SensorChart.CompositeChart.ChartAreas[0].BoundsMeasureType = MeasureType.Percentage;
            SensorChart.CompositeChart.ChartAreas[0].Bounds = new Rectangle(0, 20, 100, 80);
            SensorChart.ColorModel.ModelStyle = ColorModels.CustomLinear;
            Color[] ChartColors4 = null;
            ChartColors4 = new Color[] {
                Color.Orange,
                Color.Yellow,
                Color.Blue,
                Color.Green,
                Color.Red,
                Color.Black,
                Color.Blue,
                Color.Blue,
                Color.Blue,
                Color.Blue,
                Color.Blue
            };
            SensorChart.ColorModel.CustomPalette = ChartColors4;
            return SensorChart;
        }

        public void AddNewLineToGraph(ref Infragistics.WebUI.UltraWebChart.UltraChart SensorChart, Collection MyData, int SensorID, int SensorField, LiveMonitoring.IRemoteLib.SensorDetails MySensor)
        {

            // Create Series
            ISeries ctDLseries = null;
            NumericTimeSeries[] ctnumericTimeSeries = new NumericTimeSeries[1];

            ctnumericTimeSeries[0] = new NumericTimeSeries();
            if (MySensor.Fields.Contains((SensorField).ToString()) == true)
            {
                ctnumericTimeSeries[0].Label = MySensor.Caption + ":" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)MySensor.Fields[(SensorField).ToString()]).FieldName;
            }
            else
            {
                ctnumericTimeSeries[0].Label = MySensor.Caption + ":Unknown";
            }
            // Next
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            int TabField = 0;

            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
            {
                if (MyDataHistory.Field == (SensorField))
                {
                    if (MyDataHistory.tabRowCnt > TabField)
                    {
                        TabField = MyDataHistory.tabRowCnt;
                        //+ 1
                        Array.Resize(ref ctnumericTimeSeries, TabField + 1);
                    }

                }
            }
            for (var TabFieldCnt = 0; TabFieldCnt <= TabField; TabFieldCnt++)
            {
                foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
                {
                    int MyField = MyDataHistory.Field;
                    //- 1 'field always starts at 1

                    if (MyField == (SensorField))
                    {

                        if ((ctnumericTimeSeries[TabFieldCnt] == null) == true)
                        {
                            ctnumericTimeSeries[TabFieldCnt] = new NumericTimeSeries();
                            //TabFieldCnt.ToString + FieldNo.ToString
                            if (MySensor.Fields.Contains(("T" + TabFieldCnt.ToString() + SensorField.ToString()).ToString()) == true)
                            {
                                ctnumericTimeSeries[TabFieldCnt].Label = MySensor.Caption + ":" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)MySensor.Fields["T" + TabFieldCnt.ToString() + SensorField.ToString()]).FieldName;
                            }
                            else
                            {
                                ctnumericTimeSeries[TabFieldCnt].Label = MySensor.Caption + ":Unknown";
                            }

                        }

                        if (MyDataHistory.tabRowCnt == TabFieldCnt)
                        {
                            int myret = 0;
                            myret = ctnumericTimeSeries[TabFieldCnt].Points.Add(new NumericTimeDataPoint());
                            ctnumericTimeSeries[TabFieldCnt].Points[myret].TimeValue = MyDataHistory.DT;
                            if (chkUseOtherData.Checked)
                            {
                                if (Information.IsNumeric(MyDataHistory.OtherData))
                                {
                                    ctnumericTimeSeries[TabFieldCnt].Points[myret].NumericValue = Convert.ToDouble(MyDataHistory.OtherData);
                                }
                                else
                                {
                                    ctnumericTimeSeries[TabFieldCnt].Points[myret].NumericValue = 0;
                                }
                            }
                            else
                            {
                                ctnumericTimeSeries[TabFieldCnt].Points[myret].NumericValue = MyDataHistory.Value;
                            }
                        }


                    }
                }
                //TabFieldCnt = TabFieldCnt + 1
            }


            //try zero as only adding one layeer
            ChartLayerAppearance CTDLchartLayer = SensorChart.CompositeChart.ChartLayers[0];
            for (var TabFieldCnt = 0; TabFieldCnt <= TabField; TabFieldCnt++)
            {
                //For Acnt = 0 To MaxFieldCnt
                if ((ctnumericTimeSeries[TabFieldCnt] == null) == false)
                {
                    CTDLchartLayer.Series.Add(ctnumericTimeSeries[TabFieldCnt]);
                    ctDLseries = ctnumericTimeSeries[TabFieldCnt];
                    SensorChart.Series.Add(ctDLseries);
                }
                // Next
            }


        }

        protected void btnFilterSensorName_Click(object sender, EventArgs e)
        {
            try
            {
                //Session["SearchSensor"] = txtFilterBySensor.Text;
                Load_Sensors(0);

            }
            catch (Exception ex)
            {
            }
        }
        public CustomGraphs()
        {
            Load += Page_Load;
        }
    }

    public class LabelRenderers : IRenderLabel
    {
        Infragistics.WebUI.UltraWebChart.UltraChart MyChartRef = new Infragistics.WebUI.UltraWebChart.UltraChart();
        public string ToString(Hashtable context)
        {

            DateTime startTime = Convert.ToDateTime(context["START_TIME"]);
            DateTime endTime = Convert.ToDateTime(context["END_TIME"]);

            int seriesIndex = Convert.ToInt32(context["DATA_ROW"]);
            int itemIndex = Convert.ToInt32(context["DATA_COLUMN"]);
            int timeEntryIndex = Convert.ToInt32(context["TIME_ENTRY_INDEX"]);
            if ((Convert.ToString(context["ITEM_LABEL"]) == null) == false)
            {
                string itemlabel = Convert.ToString(context["ITEM_LABEL"]);
            }
            else
            {
                string itemlabel = "";
            }
            GanttSeries series = (Infragistics.UltraChart.Data.GanttSeries)MyChartRef.Series[seriesIndex];
            //as GanttSeries
            GanttItem item = series.Items[itemIndex];
            GanttTimeEntry timeEntry = item.Times[timeEntryIndex];

            return "Entry :" + startTime.ToString() + " Details:" + timeEntry.Owner;

        }

        public LabelRenderers(Infragistics.WebUI.UltraWebChart.UltraChart ChartRef)
        {
            MyChartRef = ChartRef;
        }
    }
}