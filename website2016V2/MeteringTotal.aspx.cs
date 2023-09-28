using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Data.Series;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Events;
using Infragistics.UltraChart.Shared.Styles;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace website2016V2.Metering
{
    public partial class MeteringTotal : System.Web.UI.Page
    {
        private Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart1 = new Infragistics.WebUI.UltraWebChart.UltraChart();
        private Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart2 = new Infragistics.WebUI.UltraWebChart.UltraChart();
        private Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart3 = new Infragistics.WebUI.UltraWebChart.UltraChart();
        private bool GeneratePDF = false;
        private Collection Rep2Images = new Collection();
        private StringBuilder ExportStr = new StringBuilder();
        private StringBuilder Rep2HTML = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
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

                if (Session["LoggedIn"].ToString() != "True")
                {
                    Response.Redirect("NotAuthorisedLogon.aspx");
                }
                //ok logged on level ?
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                // LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                //If Not Page.IsPostBack Then
                //    Me.CheckBox1.Checked = Me.UltraChart1.EnableCrossHair
                //End If
                if (IsPostBack == true)
                {
                }
                Response.Expires = 5;
                Page.MaintainScrollPositionOnPostBack = true;
                // Session["StartDate"] = DateTimeOffset.Now;
                Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateTime.Now);
                Session["EndDate"] = DateTime.Now;

                int MySensorNum = 0;
                MySensorNum = Convert.ToInt32(Request.QueryString["SensorNum"]);

                if (Page.IsPostBack == false)
                {
                    Load_Sensors(MySensorNum);
                    Load_Tarrifs();
                }


                Session["Sensors"] = "";
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

                if (MySensorNum == 0)
                {
                    //all cameras
                    //Dim MyCollection As New Collection
                    //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session("SelectedSite")), Nothing, Session("SelectedSite")))'GetServerObjects 'server1.GetAll()
                    //Dim MyObject1 As Object
                    //Dim MyDiv As Integer = 1
                    //Dim added As Boolean = False
                    //For Each MyObject1 In MyCollection
                    //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                    //        If added = False Then 'only add 1st one
                    //            AddLayer(MyObject1)
                    //            added = True
                    //            Session("Sensors") += MyObject1.ID.ToString + ","
                    //        End If
                    //    End If
                    //Next
                }
                else
                {
                    //specific

                    Collection MyCollection = new Collection();
                    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                    //MyCollection = MyRem.get_GetServerObjects(); //get_GetServerObjects 'server1.GetAll();
                    object MyObject1 = null;
                    int MyCnt = 0;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            if (MySensorNum == MySensor.ID)
                            {
                                Session["Sensors"] += MySensor.ID.ToString() + ",";
                              // AddLayer(MySensor);
                                break; // TODO: might not be correct. Was : Exit For
                            }
                            else
                            {
                                MyCnt += 1;
                            }
                        }
                    }
                }
            }
        }
        public void AddPageBreak()
        {
            HtmlGenericControl MyHtml = new HtmlGenericControl();
            MyHtml.InnerHtml = "<div style=\"height:1px\">&nbsp;</div><div style=\"page-break-before: always; height:1px;\">&nbsp;</div>";
            // MyHtml.InnerHtml = "<tr style=""page-break-before: always;"">"
            this.Charts.Controls.Add(MyHtml);
        }
        public void Load_Tarrifs()
        {
            //CameraMenu
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<LiveMonitoring.IRemoteLib.MeteringTariff> MyNewList = MyRem.LiveMonServer.GetMeteringTarrifNames();
            bool Firstone = true;
            foreach (LiveMonitoring.IRemoteLib.MeteringTariff MyTarrif in MyNewList)
            {
                System.Web.UI.WebControls.ListItem MyIttem = new System.Web.UI.WebControls.ListItem();
                MyIttem.Text = MyTarrif.TarriffName;
                MyIttem.Value = MyTarrif.ID.ToString();
                ddlTarrif.Items.Add(MyIttem);
                if (Firstone)
                {
                    MyIttem.Selected = true;
                    Firstone = false;
                }
            }

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
            bool Firstone = true;


            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    //only meters for selected Site
                    // If MySensor.SiteID = CInt(Session("Site")) Then
                    if (MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile |
                        MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile |
                         MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LandisGyrE650Profile)
                    {
                        System.Web.UI.WebControls.ListItem MyIttem = new System.Web.UI.WebControls.ListItem();
                        MyIttem.Text = MySensor.Caption;
                        MyIttem.Value = MySensor.ID.ToString();
                        ddlMeters.Items.Add(MyIttem);
                       
                        if (Firstone)
                        {
                            MyIttem.Selected = true;
                            Firstone = false;
                        }
                       
                        //Dim node As TreeNode = FindNode(MySensor.SensGroup.SensorGroupName)
                        //If IsNothing(node) Then
                        //    Dim node1 As New TreeNode
                        //    node1.ShowCheckBox = False
                        //    node1.Text = MySensor.SensGroup.SensorGroupName 'Item(CInt(MySensor.Type))
                        //    node1.Value = MySensor.SensGroup.SensorGroupID 'CInt(MySensor.Type)
                        //    node1.Expanded = False
                        //    tvSensors.Nodes.Add(node1)
                        //    node = FindNode(MySensor.SensGroup.SensorGroupName)
                        //End If

                        //Dim subnode As TreeNode = New TreeNode()
                        //subnode.ShowCheckBox = True
                        //subnode.Text = MySensor.Caption
                        //subnode.Value = MySensor.ID
                        //node.ChildNodes.Add(subnode)
                    }
                    //End If




                }
            }
        }

        public void AddTOUReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection
            List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData = null;
            LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif = default(LiveMonitoring.IRemoteLib.MeteringTarrifDetails);
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                
                if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true)
                {
                    //If cmbDataSet.SelectedValue = 0 Then
                    try
                    {
                        MyData = MyRem.LiveMonServer.GetMeteringProfileRecord(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));

                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        MyDataMarkers = MyRem.LiveMonServer.GetMeteringProfileMarkers(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));

                    }
                    catch (Exception ex)
                    {
                    }

                    //Else
                    //MyData = MyRem.server1.GetFilteredSensorHistory(cmbDataSet.SelectedValue, SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
                    // End If
                    try
                    {
                        //GetMeteringTarrifEvent
                        MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(Convert.ToInt32(ddlTarrif.SelectedValue));

                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
            }
            if ((MyData == null) == false & (MyTarrif == null) == false)
            {
                switch (SensorDet.Type)
                {
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile:
                        DrawElsterA1140TotalStatsReport(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        DrawElsterA1140TotalTOUReport(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        DrawElsterTotalA1140Kvar2(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile:
                        DrawElsterTotalA1700StatsReport(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        DrawElsterTotalA1700TOUReport(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        DrawElsterTotalA1700Kvar2(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LandisGyrE650Profile:
                        DrawLandisTotalStatsReport(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        DrawLandisTotalTOUReport(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        DrawLandisTotalKvar2(SensorDet, MyData, MyDataMarkers, MyTarrif);
                        break;
                }
            }


        }
        public void DrawLandisTotalStatsReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            try
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1><tr><td>");
                MyHtmlStr.Append("Tarrif Report:" + SensorDet.Caption);
                MyHtmlStr.Append("</td></tr><tr><td>");
                MyHtmlStr.Append("Meter :" + SensorDet.Caption);
                //ExcelStrData += "Sensor:" & SensorDet.Caption + vbCrLf
                MyHtmlStr.Append("</td></tr></table>");

                ///''''''''''''''''''
                int Bcnt = 0;
                double MyPeriod = 30;
                //default to 30 minutes
                //check if we have a channel config else use default 69
                int myChannelConfig = 69;
                // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
                foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
                {
                    if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                    {
                        myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                        MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                    }
                }
                //ok now we have channels set the names
                LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig(myChannelConfig);
                //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData

                //For Each mychanel In MyDataChannels.ChannelNames

                //Next

                double[] ConversionFactor = new double[10];
                int findMVA = 0;
                int findMW = 0;
                for (Bcnt = 0; Bcnt <= 7; Bcnt++)
                {
                    if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                    {
                        if (((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "KVA")
                        {
                            findMVA = Bcnt + 1;
                        }
                        if (((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "Import KW")
                        {
                            findMW = Bcnt + 1;
                        }
                        ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                        //End If
                    }
                    else
                    {
                        ConversionFactor[Bcnt] = 0;
                    }
                }
                //pf possible
                if (MyDataChannels.Import_mW & MyDataChannels.mVA)
                {
                    ConversionFactor[8] = 0;
                }
                int tmp1cntwe = 0;
                //mycnt1= fields ?
                //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
                double TotKWhCnt = 0;
                double TotKWCnt = 0;
                double TotKvarCnt = 0;
                double PeakKWhCnt = 0;
                double StandKWhCnt = 0;
                double OffPeakKWhCnt = 0;
                double PeakKWhCost = 0;
                double StandKWhCost = 0;
                double OffPeakKWhCost = 0;
                double PeakKWhTotCost = 0;
                double StandKWhTotCost = 0;
                double OffPeakKWhTotCost = 0;
                string PeakKWhLabel = "";
                // = 0
                string StandKWhLabel = "";
                // = 0
                string OffPeakKWhLabel = "";
                // = 0

                int MaxFieldCnt = 0;
                bool firstrec = true;

                foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
                {

                    try
                    {
                        // MyDataHistory.TimeStamp
                        //MyTarrif
                        MyTarrif.ActiveEnergyCharges.ToString();
                        LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                        int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                        if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                        {
                            switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                            {
                                //cant mix peak and offp eak so first one should be correct
                                case 1:
                                    //peak
                                    PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    PeakKWhCost = TypePeriod.CostcPerKWh;
                                    PeakKWhLabel = TypePeriod.ChargeName;
                                    break;
                                case 2:
                                    //standard
                                    StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    StandKWhCost = TypePeriod.CostcPerKWh;
                                    StandKWhLabel = TypePeriod.ChargeName;
                                    break;
                                case 3:
                                    //off peak
                                    OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                    OffPeakKWhLabel = TypePeriod.ChargeName;
                                    break;
                            }
                        }
                        else
                        {
                            StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                            StandKWhCost = TypePeriod.CostcPerKWh;
                            StandKWhLabel = TypePeriod.ChargeName;
                        }


                        //KWH findMW
                        switch (findMW)
                        {
                            case 1:
                                //always 1 import w
                                TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                                break;
                        }
                        //KVA findMVA
                        switch (findMVA)
                        {
                            case 1:
                                TotKvarCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                                break;
                            case 2:
                                TotKvarCnt += (MyDataHistory.Channel2 / ConversionFactor[1]);
                                break;
                            case 3:
                                TotKvarCnt += (MyDataHistory.Channel3 / ConversionFactor[2]);
                                break;
                            case 4:
                                TotKvarCnt += (MyDataHistory.Channel4 / ConversionFactor[3]);
                                break;
                            case 5:
                                TotKvarCnt += (MyDataHistory.Channel5 / ConversionFactor[4]);
                                break;
                            case 6:
                                TotKvarCnt += (MyDataHistory.Channel6 / ConversionFactor[5]);
                                break;
                            case 7:
                                TotKvarCnt += (MyDataHistory.Channel7 / ConversionFactor[6]);
                                break;
                            case 8:
                                TotKvarCnt += (MyDataHistory.Channel8 / ConversionFactor[7]);
                                break;
                        }


                    }
                    catch (Exception ex)
                    {
                    }

                    // End If
                }
                try
                {
                    MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=300 >");
                    MyHtmlStr.Append("Tarrif  :" + MyTarrif.TarrifDetails.TarriffName);
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Total Kwh:" + TotKWhCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Total KW :" + TotKWCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Total KVA :" + TotKvarCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("</td></tr></table>");

                }
                catch (Exception ex)
                {
                }


                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                if (GeneratePDF)
                {
                    Rep2HTML.Append(MyHtmlStr);
                }
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }


        }
        public void DrawLandisTotalTOUReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            //attach event for legent width
            //MyDateLinechart2.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart2.ChartDrawItem += MyDateLinechart2_ChartDrawItem2;
            MyDateLinechart2.ID = "Landis Gyr TOU" + SensorDet.ID.ToString();
            MyDateLinechart2.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart2.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //sets the horizontal alignment of the text
            MyDateLinechart2.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart2.TitleTop.Margins.Bottom = 2;
            MyDateLinechart2.TitleTop.Margins.Top = 2;
            MyDateLinechart2.TitleTop.Margins.Left = 2;
            MyDateLinechart2.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart2.TitleTop.Text = "Landis TOU Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart2.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart2.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart2.TitleTop.WrapText = true;
            // Set composite charts
            //MyDateLinechart2.ChartType = ChartType.Composite
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart2.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart2.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.ColumnChart;

            MyDateLinechart2.LineChart.TreatDateTimeAsString = false;
            MyDateLinechart2.Axis.X.TickmarkIntervalType = AxisIntervalType.Hours;
            MyDateLinechart2.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart2.Axis.X.TimeAxisStyle.TimeAxisStyle = RulerGenre.Discrete;
            MyDateLinechart2.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            MyDateLinechart2.Axis.X.Labels.Font = new Font("Tahoma", 7);
            MyDateLinechart2.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            MyDateLinechart2.Data.ZeroAligned = true;
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis1.LineThickness = 1
            // ' Create an Y axis
            //Dim DLyAxis1 As New AxisItem()
            //DLyAxis1.axisNumber = AxisNumber.Y_Axis
            //DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis1.LineThickness = 1
            //Dim DLyAxis2 As New AxisItem()
            //DLyAxis2.axisNumber = AxisNumber.Y2_Axis
            //DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis2.LineThickness = 1
            //myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis1)
            //'myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis2)
            //' Set the axes
            //DLchartLayer1.AxisX = DLxAxis1
            //DLchartLayer1.AxisY = DLyAxis1
            //' Set the ChartArea
            //DLchartLayer1.ChartArea = myDLChartArea1
            //' Add the ChartLayer to the ChartLayers collection
            //MyDateLinechart2.CompositeChart.ChartLayers.Add(DLchartLayer1)
            ///''''''''''''''''''
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
            // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New DataPoint
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                // numericTimeSeries1(Bcnt) = New DataPoint
                //numericTimeSeries1(Bcnt).Label = "Kwh" 'CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            double TotKvarCnt = 0;
            double PeakKWhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKWhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKWhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0
            List<Color> ChartColors = new List<Color>();
            DataTable table = new DataTable();

            table.Columns.Add("Type", System.Type.GetType("System.String"));
            //table.Columns.Add("ID", System.Type.GetType("System.Int32"))
            table.Columns.Add("KWh", System.Type.GetType("System.Double"));
            //table.Rows.Add("label", 5)

            ExportStr.Append("Elster TOU Graphs :" + SensorDet.Caption + Constants.vbCrLf);
            ExportStr.Append("Date,KWh" + Constants.vbCrLf);




            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    string typestring = "OffPeak";
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                PeakKWhCost = TypePeriod.CostcPerKWh;
                                PeakKWhLabel = TypePeriod.ChargeName;
                                //  ChartColors.Add(Color.OrangeRed);
                                typestring = "Peak";
                                break;
                            case 2:
                                //standard"
                                StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.Yellow);
                                typestring = "Stand";
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.LawnGreen);
                                typestring = "OffPeak";
                                break;
                        }
                        //default to standard
                    }
                    else
                    {
                        StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                        // ChartColors.Add(Color.Yellow);
                        typestring = "Stand";
                    }



                    //KWH findMW
                    TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);

                    //If ConversionFactor(0) = 0 Then
                    //    table.Rows.Add(MyDataHistory.TimeStamp, MyDataHistory.Channel1 * (MyPeriod / 60)) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                    //    Try
                    //        ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + (MyDataHistory.Channel1 * (MyPeriod / 60)).ToString + vbCrLf)
                    //    Catch ex As Exception

                    //    End Try
                    //Else
                    //    table.Rows.Add(MyDataHistory.TimeStamp, (MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60))
                    //    Try
                    //        ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + ((MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60)).ToString + vbCrLf)
                    //    Catch ex As Exception

                    //    End Try
                    //End If
                    //table.Rows.Add(MyDataHistory.TimeStamp.Day.ToString, (MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60))

                }
                catch (Exception ex)
                {
                }

                // End If

                table.Rows.Add("Off Peak", OffPeakKWhCnt);
                ExportStr.Append("Off Peak" + "," + (OffPeakKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Standard", StandKWhCnt);
                ExportStr.Append("Standard" + "," + (StandKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Peak", PeakKWhCnt);
                ExportStr.Append("Peak" + "," + (PeakKWhCnt).ToString() + Constants.vbCrLf);



                ChartColors.Add(Color.LawnGreen);
                ChartColors.Add(Color.Yellow);
                ChartColors.Add(Color.OrangeRed);
            }
            // Dim Acnt As Integer
            //For Acnt = 0 To 0
            //    If MyDataChannels.ChannelNames.Contains((Acnt).ToString) = True Then
            //        DLchartLayer1.Series.Add(numericTimeSeries1(Acnt))
            //        DLseries1 = numericTimeSeries1(Acnt)
            //        MyDateLinechart2.Series.Add(DLseries1)
            //    End If
            //Next
            MyDateLinechart2.DataSource = table;

            MyDateLinechart2.DataBind();
            ///''''''''''''''''''
            // Set X axis
            //DLxAxis1.Labels.Orientation = TextOrientation.Horizontal
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:dd>"
            //'DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //'DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //' Set Y axis
            //' Set the ChartType
            //DLchartLayer1.ChartType = ChartType.ColumnChart
            //' Set Axis Type
            //SetAxisTypes(DLchartLayer1)
            //'DLchartLayer1.AxisY.Extent = 130
            //'Dim atype As New AxisDataType
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLchartLayer1.AxisY.DataType = AxisDataType.Numeric

            //'DLchartLayer2.AxisX.DataType = AxisDataType.Time
            //' DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //' DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            //' Add the series to the ChartLayer's Series collection.
            MyDateLinechart2.Width = 500;
            MyDateLinechart2.Height = 500;
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart2.CompositeChart.Legends.Add(legend1)
            //'MyDateLinechart2.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            //' MyDateLinechart2.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            //legend1.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            //legend1.Bounds = New Rectangle(1, 1, 10, 10)
            //'Dim legend2 As New CompositeLegend()
            //'legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //'MyDateLinechart2.CompositeChart.Legends.Add(legend2)
            //'legend2.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(1))
            //'legend2.BoundsMeasureType = MeasureType.Percentage
            //'legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            //MyDateLinechart2.CompositeChart.ChartAreas(0).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart2.CompositeChart.ChartAreas(0).Bounds = New Rectangle(5, 0, 90, 100)
            //Dim MyBorder As New Infragistics.UltraChart.Resources.Appearance.BorderAppearance
            //MyBorder.Thickness = 0
            //MyDateLinechart2.CompositeChart.ChartAreas(0).Border = MyBorder
            MyDateLinechart2.ColorModel.ModelStyle = ColorModels.CustomLinear;
            //Color[] ChartColors4 = null;
            //ChartColors4 = new Color[] {




            //    Color.Red,
            //       Color.Yellow,
            //       Color.Green,

            //};
            //// ERROR: Not supported in C#: ReDimStatement

            //foreach (Color MyColor in ChartColors)
            //{
            //    ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //    Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //}
            //Dim ChartColors5() As Color
            //ChartColors5 = New Color() {Color.Green, Color.Yellow, Color.Red, Color.Green, Color.Red, Color.Black, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            MyDateLinechart2.ColorModel.CustomPalette = ChartColors.ToArray();
            //MyDateLinechart2.ColorModel.CustomPalette = ChartColors.ToArray()
            MyDateLinechart2.Data.SwapRowsAndColumns = true;
            MyDateLinechart2.EnableScrollBar = true;
            MyDateLinechart2.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart2.Axis.X.TickmarkInterval = MyData.Count / 15;
            //15.0
            this.MyDateLinechart2.Legend.Location = LegendLocation.Right;
            int MyItemVal = MyDateLinechart2.ColumnChart.ChartText.Add(new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance());
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].ItemFormatString = "<DATA_VALUE:0.###>";
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Row = -2;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Column = -2;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].VerticalAlign = StringAlignment.Far;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].HorizontalAlign = StringAlignment.Center;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Visible = true;
            //MyDateLinechart2.ColumnChart.ChartText(MyItemVal - 1).ChartTextFont.
            MyDateLinechart2.ColumnChart.ColumnSpacing = 1;
            //MyDateLinechart2.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart2.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            this.Charts.Controls.Add(MyDateLinechart2);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart2.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            StringBuilder MyHtmlStr = new StringBuilder();
            try
            {
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=450 ><tr><td>");
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Peak Kwh:" + PeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Std Kwh :" + StandKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Off Peak Kwh :" + OffPeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("</td></tr></table>");
                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }

            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart2.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart2.ID);
            }


        }
        public void DrawElsterA1140TotalStatsReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            try
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1><tr><td>");
                MyHtmlStr.Append("Tarrif Report:" + SensorDet.Caption);
                MyHtmlStr.Append("</td></tr><tr><td>");
                MyHtmlStr.Append("Meter :" + SensorDet.Caption);
                //ExcelStrData += "Sensor:" & SensorDet.Caption + vbCrLf
                MyHtmlStr.Append("</td></tr></table>");

                ///''''''''''''''''''
                int Bcnt = 0;
                double MyPeriod = 30;
                //default to 30 minutes
                //check if we have a channel config else use default 69
                int myChannelConfig = 69;
               // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
                foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
                {
                    if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                    {
                        myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                        MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                    }
                }
                //ok now we have channels set the names
                LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig(myChannelConfig);
                //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData

                //For Each mychanel In MyDataChannels.ChannelNames

                //Next

                double[] ConversionFactor = new double[10];
                int findMVA = 0;
                int findMW = 0;
                for (Bcnt = 0; Bcnt <= 7; Bcnt++)
                {
                    if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                    {
                        if (((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "KVA")
                        {
                            findMVA = Bcnt + 1;
                        }
                        if (((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "Import KW")
                        {
                            findMW = Bcnt + 1;
                        }
                        ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                        //End If
                    }
                    else
                    {
                        ConversionFactor[Bcnt] = 0;
                    }
                }
                //pf possible
                if (MyDataChannels.Import_mW & MyDataChannels.mVA)
                {
                    ConversionFactor[8] = 0;
                }
                int tmp1cntwe = 0;
                //mycnt1= fields ?
                //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
                double TotKWhCnt = 0;
                double TotKWCnt = 0;
                double TotKvarCnt = 0;
                double PeakKWhCnt = 0;
                double StandKWhCnt = 0;
                double OffPeakKWhCnt = 0;
                double PeakKWhCost = 0;
                double StandKWhCost = 0;
                double OffPeakKWhCost = 0;
                double PeakKWhTotCost = 0;
                double StandKWhTotCost = 0;
                double OffPeakKWhTotCost = 0;
                string PeakKWhLabel = "";
                // = 0
                string StandKWhLabel = "";
                // = 0
                string OffPeakKWhLabel = "";
                // = 0

                int MaxFieldCnt = 0;
                bool firstrec = true;

                foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
                {

                    try
                    {
                        // MyDataHistory.TimeStamp
                        //MyTarrif
                        MyTarrif.ActiveEnergyCharges.ToString();
                        LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                        int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                        if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                        {
                            switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                            {
                                //cant mix peak and offp eak so first one should be correct
                                case 1:
                                    //peak
                                    PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    PeakKWhCost = TypePeriod.CostcPerKWh;
                                    PeakKWhLabel = TypePeriod.ChargeName;
                                    break;
                                case 2:
                                    //standard
                                    StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    StandKWhCost = TypePeriod.CostcPerKWh;
                                    StandKWhLabel = TypePeriod.ChargeName;
                                    break;
                                case 3:
                                    //off peak
                                    OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                    OffPeakKWhLabel = TypePeriod.ChargeName;
                                    break;
                            }
                        }
                        else
                        {
                            StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                            StandKWhCost = TypePeriod.CostcPerKWh;
                            StandKWhLabel = TypePeriod.ChargeName;
                        }


                        //KWH findMW
                        switch (findMW)
                        {
                            case 1:
                                //always 1 import w
                                TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                                break;
                        }
                        //KVA findMVA
                        switch (findMVA)
                        {
                            case 1:
                                TotKvarCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                                break;
                            case 2:
                                TotKvarCnt += (MyDataHistory.Channel2 / ConversionFactor[1]);
                                break;
                            case 3:
                                TotKvarCnt += (MyDataHistory.Channel3 / ConversionFactor[2]);
                                break;
                            case 4:
                                TotKvarCnt += (MyDataHistory.Channel4 / ConversionFactor[3]);
                                break;
                            case 5:
                                TotKvarCnt += (MyDataHistory.Channel5 / ConversionFactor[4]);
                                break;
                            case 6:
                                TotKvarCnt += (MyDataHistory.Channel6 / ConversionFactor[5]);
                                break;
                            case 7:
                                TotKvarCnt += (MyDataHistory.Channel7 / ConversionFactor[6]);
                                break;
                            case 8:
                                TotKvarCnt += (MyDataHistory.Channel8 / ConversionFactor[7]);
                                break;
                        }


                    }
                    catch (Exception ex)
                    {
                    }

                    // End If
                }
                try
                {
                    MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=300 >");
                    MyHtmlStr.Append("Tarrif  :" + MyTarrif.TarrifDetails.TarriffName);
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Total Kwh:" + TotKWhCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Total KW :" + TotKWCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("Total KVA :" + TotKvarCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr>");
                    MyHtmlStr.Append("</td></tr></table>");

                }
                catch (Exception ex)
                {
                }


                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                if (GeneratePDF)
                {
                    Rep2HTML.Append(MyHtmlStr);
                }
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }


        }
        public void DrawElsterA1140TotalTOUReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            //attach event for legent width
            //MyDateLinechart2.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart2.ChartDrawItem += MyDateLinechart2_ChartDrawItem2;
            MyDateLinechart2.ID = "ElsterA1140TOU" + SensorDet.ID.ToString();
            MyDateLinechart2.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart2.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //sets the horizontal alignment of the text
            MyDateLinechart2.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart2.TitleTop.Margins.Bottom = 2;
            MyDateLinechart2.TitleTop.Margins.Top = 2;
            MyDateLinechart2.TitleTop.Margins.Left = 2;
            MyDateLinechart2.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart2.TitleTop.Text = "Elster TOU Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart2.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart2.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart2.TitleTop.WrapText = true;
            // Set composite charts
            //MyDateLinechart2.ChartType = ChartType.Composite
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart2.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart2.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.ColumnChart;

            MyDateLinechart2.LineChart.TreatDateTimeAsString = false;
            MyDateLinechart2.Axis.X.TickmarkIntervalType = AxisIntervalType.Hours;
            MyDateLinechart2.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart2.Axis.X.TimeAxisStyle.TimeAxisStyle = RulerGenre.Discrete;
            MyDateLinechart2.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            MyDateLinechart2.Axis.X.Labels.Font = new Font("Tahoma", 7);
            MyDateLinechart2.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            MyDateLinechart2.Data.ZeroAligned = true;
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis1.LineThickness = 1
            // ' Create an Y axis
            //Dim DLyAxis1 As New AxisItem()
            //DLyAxis1.axisNumber = AxisNumber.Y_Axis
            //DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis1.LineThickness = 1
            //Dim DLyAxis2 As New AxisItem()
            //DLyAxis2.axisNumber = AxisNumber.Y2_Axis
            //DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis2.LineThickness = 1
            //myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis1)
            //'myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis2)
            //' Set the axes
            //DLchartLayer1.AxisX = DLxAxis1
            //DLchartLayer1.AxisY = DLyAxis1
            //' Set the ChartArea
            //DLchartLayer1.ChartArea = myDLChartArea1
            //' Add the ChartLayer to the ChartLayers collection
            //MyDateLinechart2.CompositeChart.ChartLayers.Add(DLchartLayer1)
            ///''''''''''''''''''
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
           // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New DataPoint
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                // numericTimeSeries1(Bcnt) = New DataPoint
                //numericTimeSeries1(Bcnt).Label = "Kwh" 'CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            double TotKvarCnt = 0;
            double PeakKWhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKWhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKWhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0
            List<Color> ChartColors = new List<Color>();
            DataTable table = new DataTable();

            table.Columns.Add("Type", System.Type.GetType("System.String"));
            //table.Columns.Add("ID", System.Type.GetType("System.Int32"))
            table.Columns.Add("KWh", System.Type.GetType("System.Double"));
            //table.Rows.Add("label", 5)

            ExportStr.Append("Elster TOU Graphs :" + SensorDet.Caption + Constants.vbCrLf);
            ExportStr.Append("Date,KWh" + Constants.vbCrLf);




            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    string typestring = "OffPeak";
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                PeakKWhCost = TypePeriod.CostcPerKWh;
                                PeakKWhLabel = TypePeriod.ChargeName;
                              //  ChartColors.Add(Color.OrangeRed);
                                typestring = "Peak";
                                break;
                            case 2:
                                //standard"
                                StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                               // ChartColors.Add(Color.Yellow);
                                typestring = "Stand";
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                               // ChartColors.Add(Color.LawnGreen);
                                typestring = "OffPeak";
                                break;
                        }
                        //default to standard
                    }
                    else
                    {
                        StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                       // ChartColors.Add(Color.Yellow);
                        typestring = "Stand";
                    }



                    //KWH findMW
                    TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);

                    //If ConversionFactor(0) = 0 Then
                    //    table.Rows.Add(MyDataHistory.TimeStamp, MyDataHistory.Channel1 * (MyPeriod / 60)) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                    //    Try
                    //        ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + (MyDataHistory.Channel1 * (MyPeriod / 60)).ToString + vbCrLf)
                    //    Catch ex As Exception

                    //    End Try
                    //Else
                    //    table.Rows.Add(MyDataHistory.TimeStamp, (MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60))
                    //    Try
                    //        ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + ((MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60)).ToString + vbCrLf)
                    //    Catch ex As Exception

                    //    End Try
                    //End If
                    //table.Rows.Add(MyDataHistory.TimeStamp.Day.ToString, (MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60))

                }
                catch (Exception ex)
                {
                }

                // End If

                table.Rows.Add("Off Peak", OffPeakKWhCnt);
                ExportStr.Append("Off Peak" + "," + (OffPeakKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Standard", StandKWhCnt);
                ExportStr.Append("Standard" + "," + (StandKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Peak", PeakKWhCnt);
                ExportStr.Append("Peak" + "," + (PeakKWhCnt).ToString() + Constants.vbCrLf);

               

                ChartColors.Add(Color.LawnGreen);
                ChartColors.Add(Color.Yellow);
                ChartColors.Add(Color.OrangeRed);
            }
            // Dim Acnt As Integer
            //For Acnt = 0 To 0
            //    If MyDataChannels.ChannelNames.Contains((Acnt).ToString) = True Then
            //        DLchartLayer1.Series.Add(numericTimeSeries1(Acnt))
            //        DLseries1 = numericTimeSeries1(Acnt)
            //        MyDateLinechart2.Series.Add(DLseries1)
            //    End If
            //Next
            MyDateLinechart2.DataSource = table;

            MyDateLinechart2.DataBind();
            ///''''''''''''''''''
            // Set X axis
            //DLxAxis1.Labels.Orientation = TextOrientation.Horizontal
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:dd>"
            //'DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //'DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //' Set Y axis
            //' Set the ChartType
            //DLchartLayer1.ChartType = ChartType.ColumnChart
            //' Set Axis Type
            //SetAxisTypes(DLchartLayer1)
            //'DLchartLayer1.AxisY.Extent = 130
            //'Dim atype As New AxisDataType
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLchartLayer1.AxisY.DataType = AxisDataType.Numeric

            //'DLchartLayer2.AxisX.DataType = AxisDataType.Time
            //' DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //' DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            //' Add the series to the ChartLayer's Series collection.
            MyDateLinechart2.Width = 500;
            MyDateLinechart2.Height = 500;
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart2.CompositeChart.Legends.Add(legend1)
            //'MyDateLinechart2.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            //' MyDateLinechart2.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            //legend1.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            //legend1.Bounds = New Rectangle(1, 1, 10, 10)
            //'Dim legend2 As New CompositeLegend()
            //'legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //'MyDateLinechart2.CompositeChart.Legends.Add(legend2)
            //'legend2.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(1))
            //'legend2.BoundsMeasureType = MeasureType.Percentage
            //'legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            //MyDateLinechart2.CompositeChart.ChartAreas(0).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart2.CompositeChart.ChartAreas(0).Bounds = New Rectangle(5, 0, 90, 100)
            //Dim MyBorder As New Infragistics.UltraChart.Resources.Appearance.BorderAppearance
            //MyBorder.Thickness = 0
            //MyDateLinechart2.CompositeChart.ChartAreas(0).Border = MyBorder
            MyDateLinechart2.ColorModel.ModelStyle = ColorModels.CustomLinear;
            //Color[] ChartColors4 = null;
            //ChartColors4 = new Color[] {



                
            //    Color.Red,
            //       Color.Yellow,
            //       Color.Green,

            //};
            //// ERROR: Not supported in C#: ReDimStatement

            //foreach (Color MyColor in ChartColors)
            //{
            //    ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //    Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //}
            //Dim ChartColors5() As Color
            //ChartColors5 = New Color() {Color.Green, Color.Yellow, Color.Red, Color.Green, Color.Red, Color.Black, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            MyDateLinechart2.ColorModel.CustomPalette = ChartColors.ToArray();
            //MyDateLinechart2.ColorModel.CustomPalette = ChartColors.ToArray()
            MyDateLinechart2.Data.SwapRowsAndColumns = true;
            MyDateLinechart2.EnableScrollBar = true;
            MyDateLinechart2.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart2.Axis.X.TickmarkInterval = MyData.Count / 15;
            //15.0
            this.MyDateLinechart2.Legend.Location = LegendLocation.Right;
            int MyItemVal = MyDateLinechart2.ColumnChart.ChartText.Add(new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance());
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].ItemFormatString = "<DATA_VALUE:0.###>";
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Row = -2;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Column = -2;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].VerticalAlign = StringAlignment.Far;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].HorizontalAlign = StringAlignment.Center;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Visible = true;
            //MyDateLinechart2.ColumnChart.ChartText(MyItemVal - 1).ChartTextFont.
            MyDateLinechart2.ColumnChart.ColumnSpacing = 1;
            //MyDateLinechart2.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart2.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            this.Charts.Controls.Add(MyDateLinechart2);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart2.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            StringBuilder MyHtmlStr = new StringBuilder();
            try
            {
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=450 ><tr><td>");
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Peak Kwh:" + PeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Std Kwh :" + StandKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Off Peak Kwh :" + OffPeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("</td></tr></table>");
                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }

            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart2.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart2.ID);
            }


        }
        public void DrawElsterTotalA1700StatsReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            try
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1><tr><td>");
                MyHtmlStr.Append("Tarrif Report:" + SensorDet.Caption);
                MyHtmlStr.Append("</td></tr><tr><td>");
                MyHtmlStr.Append("Meter :" + SensorDet.Caption);
                //ExcelStrData += "Sensor:" & SensorDet.Caption + vbCrLf
                MyHtmlStr.Append("</td></tr></table>");

                ///''''''''''''''''''
                int Bcnt = 0;
                double MyPeriod = 30;
                //default to 30 minutes
                //check if we have a channel config else use default 69
                int myChannelConfig = 69;
                //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
                foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker  MyDataMarkerHistory in MyDataMarkers)
                {
                    if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                    {
                        myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                        MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                    }
                }
                //ok now we have channels set the names
                LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
                //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

                //For Each mychanel In MyDataChannels.ChannelNames

                //Next

                double[] ConversionFactor = new double[10];
                int findMVA = 0;
                int findMW = 0;
                for (Bcnt = 0; Bcnt <= 7; Bcnt++)
                {
                    if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                    {
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "KVA")
                        {
                            findMVA = Bcnt + 1;
                        }
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "Import KW")
                        {
                            findMW = Bcnt + 1;
                        }
                        ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                        //End If
                    }
                    else
                    {
                        ConversionFactor[Bcnt] = 0;
                    }
                }
                //pf possible
                if (MyDataChannels.Import_mW & MyDataChannels.mVA)
                {
                    ConversionFactor[8] = 0;
                }
                int tmp1cntwe = 0;
                //mycnt1= fields ?
               // LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
                double TotKWhCnt = 0;
                double TotKWCnt = 0;
                double TotKvarCnt = 0;
                double PeakKWhCnt = 0;
                double StandKWhCnt = 0;
                double OffPeakKWhCnt = 0;
                double PeakKWhCost = 0;
                double StandKWhCost = 0;
                double OffPeakKWhCost = 0;
                double PeakKWhTotCost = 0;
                double StandKWhTotCost = 0;
                double OffPeakKWhTotCost = 0;
                string PeakKWhLabel = "";
                // = 0
                string StandKWhLabel = "";
                // = 0
                string OffPeakKWhLabel = "";
                // = 0

                int MaxFieldCnt = 0;
                bool firstrec = true;

                foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
                {

                    try
                    {
                        // MyDataHistory.TimeStamp
                        //MyTarrif
                        MyTarrif.ActiveEnergyCharges.ToString();
                        LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                        int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                        if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                        {
                            switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                            {
                                //cant mix peak and offp eak so first one should be correct
                                case 1:
                                    //peak
                                    PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    PeakKWhCost = TypePeriod.CostcPerKWh;
                                    PeakKWhLabel = TypePeriod.ChargeName;
                                    break;
                                case 2:
                                    //standard
                                    StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    StandKWhCost = TypePeriod.CostcPerKWh;
                                    StandKWhLabel = TypePeriod.ChargeName;
                                    break;
                                case 3:
                                    //off peak
                                    OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                    OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                    OffPeakKWhLabel = TypePeriod.ChargeName;
                                    break;
                            }
                        }
                        else
                        {
                            StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                            StandKWhCost = TypePeriod.CostcPerKWh;
                            StandKWhLabel = TypePeriod.ChargeName;
                        }


                        //KWH findMW
                        switch (findMW)
                        {
                            case 1:
                                //always 1 import w
                                TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                                break;
                        }
                        //KVA findMVA
                        switch (findMVA)
                        {
                            case 1:
                                TotKvarCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                                break;
                            case 2:
                                TotKvarCnt += (MyDataHistory.Channel2 / ConversionFactor[1]);
                                break;
                            case 3:
                                TotKvarCnt += (MyDataHistory.Channel3 / ConversionFactor[2]);
                                break;
                            case 4:
                                TotKvarCnt += (MyDataHistory.Channel4 / ConversionFactor[3]);
                                break;
                            case 5:
                                TotKvarCnt += (MyDataHistory.Channel5 / ConversionFactor[4]);
                                break;
                            case 6:
                                TotKvarCnt += (MyDataHistory.Channel6 / ConversionFactor[5]);
                                break;
                            case 7:
                                TotKvarCnt += (MyDataHistory.Channel7 / ConversionFactor[6]);
                                break;
                            case 8:
                                TotKvarCnt += (MyDataHistory.Channel8 / ConversionFactor[7]);
                                break;
                        }


                    }
                    catch (Exception ex)
                    {
                    }

                    // End If
                }
                try
                {
                    MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=450 ><tr><td>");
                    MyHtmlStr.Append("Tarrif  :" + MyTarrif.TarrifDetails.TarriffName);
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    MyHtmlStr.Append("Total Kwh:" + TotKWhCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    MyHtmlStr.Append("Total KW :" + TotKWCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    MyHtmlStr.Append("Total KVA :" + TotKvarCnt.ToString());
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    MyHtmlStr.Append("</td></tr></table>");

                }
                catch (Exception ex)
                {
                }


                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                if (GeneratePDF)
                {
                    Rep2HTML.Append(MyHtmlStr);
                }
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }


        }
        public void DrawElsterTotalA1700TOUReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            //attach event for legent width
          //  MyDateLinechart2.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart2.ChartDrawItem += MyDateLinechart2_ChartDrawItem2;
            MyDateLinechart2.ID = "ElsterA1700TotTOU" + SensorDet.ID.ToString();
            MyDateLinechart2.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart2.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //sets the horizontal alignment of the text
            MyDateLinechart2.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart2.TitleTop.Margins.Bottom = 2;
            MyDateLinechart2.TitleTop.Margins.Top = 2;
            MyDateLinechart2.TitleTop.Margins.Left = 2;
            MyDateLinechart2.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart2.TitleTop.Text = "Elster Total TOU Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart2.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart2.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart2.TitleTop.WrapText = true;
            // Set composite charts
            //MyDateLinechart2.ChartType = ChartType.Composite
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart2.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart2.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.ColumnChart;
            MyDateLinechart2.LineChart.TreatDateTimeAsString = false;
            MyDateLinechart2.Axis.X.TickmarkIntervalType = AxisIntervalType.Hours;
            MyDateLinechart2.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart2.Axis.X.TimeAxisStyle.TimeAxisStyle = RulerGenre.Discrete;
            MyDateLinechart2.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            MyDateLinechart2.Axis.X.Labels.Font = new Font("Tahoma", 7);
            MyDateLinechart2.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            MyDateLinechart2.Data.ZeroAligned = true;
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis1.LineThickness = 1
            // ' Create an Y axis
            //Dim DLyAxis1 As New AxisItem()
            //DLyAxis1.axisNumber = AxisNumber.Y_Axis
            //DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis1.LineThickness = 1
            //Dim DLyAxis2 As New AxisItem()
            //DLyAxis2.axisNumber = AxisNumber.Y2_Axis
            //DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis2.LineThickness = 1
            //myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis1)
            //'myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis2)
            //' Set the axes
            //DLchartLayer1.AxisX = DLxAxis1
            //DLchartLayer1.AxisY = DLyAxis1
            //' Set the ChartArea
            //DLchartLayer1.ChartArea = myDLChartArea1
            //' Add the ChartLayer to the ChartLayers collection
            //MyDateLinechart2.CompositeChart.ChartLayers.Add(DLchartLayer1)
            ///''''''''''''''''''
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
           // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            for (Bcnt = 0; Bcnt <= 0; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New DataPoint
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                // numericTimeSeries1(Bcnt) = New DataPoint
                //numericTimeSeries1(Bcnt).Label = "Kwh" 'CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
           // LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            double TotKvarCnt = 0;
            double PeakKWhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKWhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKWhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0
            List<Color> ChartColors = new List<Color>();
            DataTable table = new DataTable();

            table.Columns.Add("Type", System.Type.GetType("System.String"));
            //table.Columns.Add("ID", System.Type.GetType("System.Int32"))
            table.Columns.Add("KWh", System.Type.GetType("System.Double"));
            //table.Rows.Add("label", 5)
            ExportStr.Append("Elster TOU Graphs :" + SensorDet.Caption + Constants.vbCrLf);
            ExportStr.Append("Date,KWh" + Constants.vbCrLf);




            System.DateTime CurBatchDate = default(System.DateTime);
            double CurVal = 0;
            bool LoadBatchDate = true;
            double CalCValue = 0;

            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();
                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if (LoadBatchDate)
                    {
                        CurBatchDate = MyDataHistory.TimeStamp;
                        LoadBatchDate = false;
                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }
                try
                {
                    string typestring = "OffPeak";
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                PeakKWhCost = TypePeriod.CostcPerKWh;
                                PeakKWhLabel = TypePeriod.ChargeName;
                                 // ChartColors.Add(Color.OrangeRed);
                                typestring = "Peak";
                                break;
                            case 2:
                               //standard"
                                StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.Yellow);
                                typestring = "Stand";
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                 //ChartColors.Add(Color.LawnGreen);
                                typestring = "OffPeak";
                                break;
                        }
                        //default to standard
                    }
                    else
                    {
                        StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                       // ChartColors.Add(Color.Yellow);
                        typestring = "Stand";
                    }
                    //KWH findMW
                    TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                    //If ConversionFactor(0) = 0 Then
                    //    CalCValue = MyDataHistory.Channel1 * (MyPeriod / 60)
                    //Else
                    //    CalCValue = (MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60)
                    //End If
                    //'table.Rows.Add(MyDataHistory.TimeStamp, CalCValue) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                    //'Try
                    //'    ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + (CalCValue).ToString + vbCrLf)
                    //'Catch ex As Exception

                    //'End Try

                    //CurVal += CalCValue
                    //Select Case cmbDataSet.SelectedValue
                    //    Case 0 'all data
                    //        table.Rows.Add(CurBatchDate, CurVal)
                    //        ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                    //        CurVal = 0
                    //        LoadBatchDate = True
                    //    Case 1 '12 hour
                    //        If DateDiff(DateInterval.Hour, CurBatchDate, MyDataHistory.TimeStamp) >= 12 Then
                    //            table.Rows.Add(CurBatchDate, CurVal)
                    //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                    //            CurVal = 0
                    //            CurBatchDate = MyDataHistory.TimeStamp
                    //            LoadBatchDate = True
                    //        End If
                    //    Case 2 'daily
                    //        If DateDiff(DateInterval.Day, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                    //            table.Rows.Add(CurBatchDate, CurVal)
                    //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                    //            CurVal = 0
                    //            CurBatchDate = MyDataHistory.TimeStamp
                    //            LoadBatchDate = True
                    //        End If
                    //    Case 3 'weekly
                    //        If DateDiff(DateInterval.WeekOfYear, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                    //            table.Rows.Add(CurBatchDate, CurVal)
                    //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                    //            CurVal = 0
                    //            CurBatchDate = MyDataHistory.TimeStamp
                    //            LoadBatchDate = True
                    //        End If
                    //    Case 4 'monthly
                    //        If DateDiff(DateInterval.Month, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                    //            table.Rows.Add(CurBatchDate, CurVal)
                    //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                    //            CurVal = 0
                    //            CurBatchDate = MyDataHistory.TimeStamp
                    //            LoadBatchDate = True
                    //        End If
                    //End Select

                    // table.Rows.Add(MyDataHistory.TimeStamp.Day.ToString(), (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60));

                }
                catch (Exception ex)
                {
                }

                // End If
            }
                table.Rows.Add("Off Peak", OffPeakKWhCnt);
                ExportStr.Append("Off Peak" + "," + (OffPeakKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Standard", StandKWhCnt);
                ExportStr.Append("Standard" + "," + (StandKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Peak", PeakKWhCnt);
                ExportStr.Append("Peak" + "," + (PeakKWhCnt).ToString() + Constants.vbCrLf);


                ChartColors.Add(Color.LawnGreen);
                ChartColors.Add(Color.Yellow);
                ChartColors.Add(Color.OrangeRed);
           
            // Dim Acnt As Integer
            //For Acnt = 0 To 0
            //    If MyDataChannels.ChannelNames.Contains((Acnt).ToString) = True Then
            //        DLchartLayer1.Series.Add(numericTimeSeries1(Acnt))
            //        DLseries1 = numericTimeSeries1(Acnt)
            //        MyDateLinechart2.Series.Add(DLseries1)
            //    End If
            //Next
            
            MyDateLinechart2.DataSource = table;
            
            MyDateLinechart2.DataBind();
            ///''''''''''''''''''
            // Set X axis
            //DLxAxis1.Labels.Orientation = TextOrientation.Horizontal
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:dd>"
            //'DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //'DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //' Set Y axis
            //' Set the ChartType
            //DLchartLayer1.ChartType = ChartType.ColumnChart
            //' Set Axis Type
            //SetAxisTypes(DLchartLayer1)
            //'DLchartLayer1.AxisY.Extent = 130
            //'Dim atype As New AxisDataType
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLchartLayer1.AxisY.DataType = AxisDataType.Numeric

            //'DLchartLayer2.AxisX.DataType = AxisDataType.Time
            //' DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //' DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            //' Add the series to the ChartLayer's Series collection.
            MyDateLinechart2.Width = 500;
            MyDateLinechart2.Height = 400;
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart2.CompositeChart.Legends.Add(legend1)
            //'MyDateLinechart2.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            //' MyDateLinechart2.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            //legend1.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            //legend1.Bounds = New Rectangle(1, 1, 10, 10)
            //'Dim legend2 As New CompositeLegend()
            //'legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //'MyDateLinechart2.CompositeChart.Legends.Add(legend2)
            //'legend2.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(1))
            //'legend2.BoundsMeasureType = MeasureType.Percentage
            //'legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            //MyDateLinechart2.CompositeChart.ChartAreas(0).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart2.CompositeChart.ChartAreas(0).Bounds = New Rectangle(5, 0, 90, 100)
            //Dim MyBorder As New Infragistics.UltraChart.Resources.Appearance.BorderAppearance
            //MyBorder.Thickness = 0
            //MyDateLinechart2.CompositeChart.ChartAreas(0).Border = MyBorder
            MyDateLinechart2.ColorModel.ModelStyle = ColorModels.CustomLinear;
            //Color[] ChartColors4= new Color[0];
            //// ERROR: Not supported in C#: ReDimStatement

            //foreach (Color MyColor in ChartColors)
            //{
            //    ChartColors4[ChartColors4.Length] = new Color();
            //    ChartColors4[ChartColors4.Length] = MyColor;
            //    Array.Resize(ref ChartColors4, ChartColors4.Length + 1);
            //}
            //Color[] ChartColors4 = null;
            //ChartColors4 = new Color[] {
            //    Color.Red,
            //       Color.Yellow,
            //         Color.Green,

            //};
            //// ERROR: Not supported in C#: ReDimStatement

            //foreach (Color MyColor in ChartColors)
            //{
            //    ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //    Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //}
            //Dim ChartColors5() As Color
            //ChartColors5 = New Color() {Color.Green, Color.Yellow, Color.Red, Color.Green, Color.Red, Color.Black, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            //If cmbDataSet.SelectedValue = 0 Then
            // ERROR: Not supported in C#: ReDimStatement

            //foreach (Color MyColor in ChartColors)
            //{
            //    ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //    Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //}
            //Else
            //ReDim ChartColors4(0)
            //For Each MyColor As Color In ChartColors
            //    ChartColors4(ChartColors4.GetUpperBound(0)) = Color.LawnGreen
            //    ReDim Preserve ChartColors4(ChartColors4.GetUpperBound(0) + 1)
            //Next
            //End If
            MyDateLinechart2.ColorModel.CustomPalette = ChartColors.ToArray();
            //MyDateLinechart2.ColorModel.CustomPalette = ChartColors.ToArray()
            MyDateLinechart2.Data.SwapRowsAndColumns = true;
            // MyDateLinechart2.EnableScrollBar = True
            //MyDateLinechart2.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart2.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            MyDateLinechart2.Axis.X.TickmarkStyle = AxisTickStyle.Smart;

            MyDateLinechart2.Axis.X.TickmarkInterval = MyData.Count / 20;
            //15.0

            this.DivTOUReport.Controls.Add(MyDateLinechart2);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart2.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart2.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            this.MyDateLinechart2.Legend.Location = LegendLocation.Right;
            int MyItemVal = MyDateLinechart2.ColumnChart.ChartText.Add(new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance());
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].ItemFormatString = "<DATA_VALUE:0.###>";
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Row = -2;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Column = -2;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].VerticalAlign = StringAlignment.Far;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].HorizontalAlign = StringAlignment.Center;
            MyDateLinechart2.ColumnChart.ChartText[MyItemVal - 1].Visible = true;
            //MyDateLinechart2.ColumnChart.ChartText(MyItemVal - 1).ChartTextFont.
            MyDateLinechart2.ColumnChart.ColumnSpacing = 1;
            //Me.MyDateLinechart2.ColumnChart.ChartText(0).Visible = True
            StringBuilder MyHtmlStr = new StringBuilder();
            try
            {
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=450 ><tr><td>");
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Peak Kwh:" + PeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Std Kwh :" + StandKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Off Peak Kwh :" + OffPeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("</td></tr></table>");
                this.DivTOUStatsReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }
            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart2.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart2.ID);
            }


        }

        public void DrawElsterTotalA1140Kvar2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            //attach event for legent width
            //MyDateLinechart3.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart3.ChartDrawItem += MyDateLinechart2_ChartDrawItem2;
            MyDateLinechart3.ID = "ElsterA1140Kvarh" + SensorDet.ID.ToString();
            MyDateLinechart3.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart3.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //sets the horizontal alignment of the text
            MyDateLinechart3.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart3.TitleTop.Margins.Bottom = 2;
            MyDateLinechart3.TitleTop.Margins.Top = 2;
            MyDateLinechart3.TitleTop.Margins.Left = 2;
            MyDateLinechart3.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart3.TitleTop.Text = "Elster Kvarh Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart3.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart3.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart3.TitleTop.WrapText = true;
            // Set composite charts
            //MyDateLinechart3.ChartType = ChartType.Composite
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart3.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart3.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.ColumnChart;
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //MyDateLinechart3.Axis.Add(DLxAxis1)
            MyDateLinechart3.LineChart.TreatDateTimeAsString = false;
            MyDateLinechart3.Axis.X.TickmarkIntervalType = AxisIntervalType.Hours;
            MyDateLinechart3.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart3.Axis.X.TimeAxisStyle.TimeAxisStyle = RulerGenre.Discrete;
            MyDateLinechart3.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            MyDateLinechart3.Axis.X.Labels.Font = new Font("Tahoma", 7);
            MyDateLinechart3.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis1.LineThickness = 1
            // ' Create an Y axis
            //Dim DLyAxis1 As New AxisItem()
            //DLyAxis1.axisNumber = AxisNumber.Y_Axis
            //DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis1.LineThickness = 1
            //Dim DLyAxis2 As New AxisItem()
            //DLyAxis2.axisNumber = AxisNumber.Y2_Axis
            //DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis2.LineThickness = 1
            //myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis1)
            //'myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis2)
            //' Set the axes
            //DLchartLayer1.AxisX = DLxAxis1
            //DLchartLayer1.AxisY = DLyAxis1
            //' Set the ChartArea
            //DLchartLayer1.ChartArea = myDLChartArea1
            //' Add the ChartLayer to the ChartLayers collection
            //MyDateLinechart3.CompositeChart.ChartLayers.Add(DLchartLayer1)
            ///''''''''''''''''''
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            int findKvar = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                {
                    //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                    //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                    //numericTimeSeries1(Bcnt).Label = CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                    if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "Q1 Kvar")
                    {
                        findKvar = Bcnt + 1;
                    }
                    ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                    //End If
                }
                else
                {
                    //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                    // numericTimeSeries1(Bcnt).Label = ""
                    ConversionFactor[Bcnt] = 0;
                }
            }
            for (Bcnt = 0; Bcnt <=0; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New DataPoint
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                // numericTimeSeries1(Bcnt) = New DataPoint
                //numericTimeSeries1(Bcnt).Label = "Kwh" 'CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
           // LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKvarhCnt = 0;
            double TotKvarCnt = 0;

            double PeakKVarhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKvarhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKvarhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0
            List<Color> ChartColors = new List<Color>();
            DataTable table = new DataTable();

            table.Columns.Add("Type", System.Type.GetType("System.String"));
            //table.Columns.Add("ID", System.Type.GetType("System.Int32"))
            table.Columns.Add("Kvarh", System.Type.GetType("System.Double"));
            //table.Rows.Add("label", 5)






            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    double ChannelVal = 0;
                    double ConversionVal = 0;
                    switch (findKvar)
                    {
                        case 1:
                            ChannelVal = MyDataHistory.Channel1;
                            ConversionVal = ConversionFactor[0];
                            break;
                        case 2:
                            ChannelVal = MyDataHistory.Channel2;
                            ConversionVal = ConversionFactor[1];
                            break;
                        case 3:
                            ChannelVal = MyDataHistory.Channel3;
                            ConversionVal = ConversionFactor[2];
                            break;
                        case 4:
                            ChannelVal = MyDataHistory.Channel4;
                            ConversionVal = ConversionFactor[3];
                            break;
                        case 5:
                            ChannelVal = MyDataHistory.Channel5;
                            ConversionVal = ConversionFactor[4];
                            break;
                        case 6:
                            ChannelVal = MyDataHistory.Channel7;
                            ConversionVal = ConversionFactor[5];
                            break;
                        case 7:
                            ChannelVal = MyDataHistory.Channel7;
                            ConversionVal = ConversionFactor[6];
                            break;
                        case 8:
                            ChannelVal = MyDataHistory.Channel8;
                            ConversionVal = ConversionFactor[7];
                            break;
                    }
                    string typestring = "OffPeak";
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);

                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKVarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                PeakKvarhCost = TypePeriod.CostcPerKWh;
                                PeakKvarhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.OrangeRed);
                                typestring = "Peak";
                                break;
                            case 2:
                                //standard"
                                StandKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.Yellow);
                                typestring = "Stand";
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                               // ChartColors.Add(Color.LawnGreen);
                                typestring = "OffPeak";
                                break;
                        }
                        TotKvarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        TotKvarCnt += (ChannelVal / ConversionVal);
                        //If ConversionFactor(0) = 0 Then
                        //    table.Rows.Add(MyDataHistory.TimeStamp, ChannelVal * (MyPeriod / 60)) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                        //Else
                        //    table.Rows.Add(MyDataHistory.TimeStamp, (ChannelVal / ConversionVal) * (MyPeriod / 60))
                        //End If
                        //default standard
                    }
                    else
                    {
                        StandKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                       // ChartColors.Add(Color.Yellow);
                        typestring = "Stand";
                        TotKvarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        TotKvarCnt += (ChannelVal / ConversionVal);
                    }



                    //KWH findMW

                    //table.Rows.Add(MyDataHistory.TimeStamp.Day.ToString, (MyDataHistory.Channel1 / ConversionFactor(0)) * (MyPeriod / 60))

                }
                catch (Exception ex)
                {
                }

                // End If
            }
                table.Rows.Add("Off Peak", OffPeakKWhCnt);
                ExportStr.Append("Off Peak" + "," + (OffPeakKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Standard", StandKWhCnt);
                ExportStr.Append("Standard" + "," + (StandKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Peak", PeakKVarhCnt);
                ExportStr.Append("Peak" + "," + (PeakKVarhCnt).ToString() + Constants.vbCrLf);
                ChartColors.Add(Color.LawnGreen);
                ChartColors.Add(Color.Yellow);
                ChartColors.Add(Color.OrangeRed);
           
            // Dim Acnt As Integer
            //For Acnt = 0 To 0
            //    If MyDataChannels.ChannelNames.Contains((Acnt).ToString) = True Then
            //        DLchartLayer1.Series.Add(numericTimeSeries1(Acnt))
            //        DLseries1 = numericTimeSeries1(Acnt)
            //        MyDateLinechart3.Series.Add(DLseries1)
            //    End If
            //Next
            MyDateLinechart3.DataSource = table;

            MyDateLinechart3.DataBind();
            ///''''''''''''''''''
            // Set X axis
            //DLxAxis1.Labels.Orientation = TextOrientation.Horizontal
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:dd>"
            //DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //'DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //'DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //' Set Y axis
            //' Set the ChartType
            //DLchartLayer1.ChartType = ChartType.ColumnChart
            //' Set Axis Type
            //SetAxisTypes(DLchartLayer1)
            //'DLchartLayer1.AxisY.Extent = 130
            //'Dim atype As New AxisDataType
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLchartLayer1.AxisY.DataType = AxisDataType.Numeric

            //'DLchartLayer2.AxisX.DataType = AxisDataType.Time
            //' DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //' DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            //' Add the series to the ChartLayer's Series collection.
            MyDateLinechart3.Width = 500;
            MyDateLinechart3.Height = 400;
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart3.CompositeChart.Legends.Add(legend1)
            //'MyDateLinechart3.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            //' MyDateLinechart3.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            //legend1.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            //legend1.Bounds = New Rectangle(1, 1, 10, 10)
            //'Dim legend2 As New CompositeLegend()
            //'legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //'MyDateLinechart3.CompositeChart.Legends.Add(legend2)
            //'legend2.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(1))
            //'legend2.BoundsMeasureType = MeasureType.Percentage
            //'legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            //MyDateLinechart3.CompositeChart.ChartAreas(0).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart3.CompositeChart.ChartAreas(0).Bounds = New Rectangle(5, 0, 90, 100)
            //Dim MyBorder As New Infragistics.UltraChart.Resources.Appearance.BorderAppearance
            //MyBorder.Thickness = 0
            //MyDateLinechart3.CompositeChart.ChartAreas(0).Border = MyBorder
            MyDateLinechart3.ColorModel.ModelStyle = ColorModels.CustomLinear;
            //Color[] ChartColors4 = null;
            //ChartColors4 = new Color[] {



               
            //    Color.Red,
            //       Color.Yellow,
            //        Color.Green,

            //};
            //// ERROR: Not supported in C#: ReDimStatement

            //foreach (Color MyColor in ChartColors)
            //{
            //    ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //    Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //}
            //Dim ChartColors5() As Color
            //ChartColors5 = New Color() {Color.Green, Color.Yellow, Color.Red, Color.Green, Color.Red, Color.Black, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            MyDateLinechart3.ColorModel.CustomPalette = ChartColors.ToArray();
            //MyDateLinechart3.ColorModel.CustomPalette = ChartColors.ToArray()
            MyDateLinechart3.Data.SwapRowsAndColumns = true;
            MyDateLinechart3.EnableScrollBar = true;
            MyDateLinechart3.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart3.Axis.X.TickmarkInterval = MyData.Count / 15;
            //15.0
            this.MyDateLinechart3.Legend.Location = LegendLocation.Right;
            int MyItemVal = MyDateLinechart3.ColumnChart.ChartText.Add(new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance());
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].ItemFormatString = "<DATA_VALUE:0.###>";
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Row = -2;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Column = -2;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].VerticalAlign = StringAlignment.Far;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].HorizontalAlign = StringAlignment.Center;
            MyDateLinechart3.ColumnChart.ChartText[ MyItemVal - 1].Visible = true;
            //MyDateLinechart3.ColumnChart.ChartText(MyItemVal - 1).ChartTextFont.
            MyDateLinechart3.ColumnChart.ColumnSpacing = 1;
            // MyDateLinechart3.
            //MyDateLinechart3.ColumnChart.Columns(0).Width = Unit.Pixel(100)
            // MyDateLinechart3.Columns(0).Width = Unit.Percentage(33.3)

            //MyDateLinechart3.ColumnChart.ChartComponent.c
            //MyDateLinechart3.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart3.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)

            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart3.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)


             this.DivKvarReport.Controls.Add(MyDateLinechart3);
            StringBuilder MyHtmlStr = new StringBuilder();
            try
            {
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=350 ><tr><td>");
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Peak Kvarh:" + PeakKVarhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Std Kvarh :" + StandKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Off Peak Kvarh :" + OffPeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("</td></tr></table>");
                this.DivKvarStatsReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }
            if (GeneratePDF)
            {
                //Rep2HTML.Append(MyHtmlStr)
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart3.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart3.ID);
            }

        }

        public void DrawElsterTotalA1700Kvar2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            //attach event for legent width
           // MyDateLinechart3.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart3.ChartDrawItem += MyDateLinechart2_ChartDrawItem2;
            MyDateLinechart3.ID = "ElsterA1700TotKvarh" + SensorDet.ID.ToString();
            MyDateLinechart3.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart3.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //sets the horizontal alignment of the text
            MyDateLinechart3.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart3.TitleTop.Margins.Bottom = 2;
            MyDateLinechart3.TitleTop.Margins.Top = 2;
            MyDateLinechart3.TitleTop.Margins.Left = 2;
            MyDateLinechart3.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart3.TitleTop.Text = "Elster Total Kvarh Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart3.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart3.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart3.TitleTop.WrapText = true;
            // Set composite charts
            //MyDateLinechart3.ChartType = ChartType.Composite
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart3.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart3.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.ColumnChart;
            MyDateLinechart3.LineChart.TreatDateTimeAsString = false;
            MyDateLinechart3.Axis.X.TickmarkIntervalType = AxisIntervalType.Hours;
            MyDateLinechart3.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart3.Axis.X.TimeAxisStyle.TimeAxisStyle = RulerGenre.Discrete;
            MyDateLinechart3.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            MyDateLinechart3.Axis.X.Labels.Font = new Font("Tahoma", 7);
            MyDateLinechart3.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            MyDateLinechart3.Data.ZeroAligned = true;
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis1.LineThickness = 1
            // ' Create an Y axis
            //Dim DLyAxis1 As New AxisItem()
            //DLyAxis1.axisNumber = AxisNumber.Y_Axis
            //DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis1.LineThickness = 1
            //Dim DLyAxis2 As New AxisItem()
            //DLyAxis2.axisNumber = AxisNumber.Y2_Axis
            //DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis2.LineThickness = 1
            //myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis1)
            //'myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis2)
            //' Set the axes
            //DLchartLayer1.AxisX = DLxAxis1
            //DLchartLayer1.AxisY = DLyAxis1
            //' Set the ChartArea
            //DLchartLayer1.ChartArea = myDLChartArea1
            //' Add the ChartLayer to the ChartLayers collection
            //MyDateLinechart3.CompositeChart.ChartLayers.Add(DLchartLayer1)
            ///''''''''''''''''''
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
           // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            int findKvar = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                {
                    //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                    //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                    //numericTimeSeries1(Bcnt).Label = CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                    if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "Q1 Kvar")
                    {
                        findKvar = Bcnt + 1;
                    }
                    ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                    //End If
                }
                else
                {
                    //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                    // numericTimeSeries1(Bcnt).Label = ""
                    ConversionFactor[Bcnt] = 0;
                }
            }
            for (Bcnt = 0; Bcnt <= 0; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New DataPoint
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                // numericTimeSeries1(Bcnt) = New DataPoint
                //numericTimeSeries1(Bcnt).Label = "Kwh" 'CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
           // LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKvarhCnt = 0;
            double TotKvarCnt = 0;

            double PeakKVarhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKvarhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKvarhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0
            List<Color> ChartColors = new List<Color>();
            DataTable table = new DataTable();

            table.Columns.Add("Type", System.Type.GetType("System.String"));
            //table.Columns.Add("ID", System.Type.GetType("System.Int32"));
            table.Columns.Add("Kvarh", System.Type.GetType("System.Double"));
            //table.Rows.Add("label", 5)

            System.DateTime CurBatchDate = default(System.DateTime);
            double CurVal = 0;
            bool LoadBatchDate = true;
            double CalCValue = 0;


            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if (LoadBatchDate)
                    {
                        CurBatchDate = MyDataHistory.TimeStamp;
                        LoadBatchDate = false;
                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    double ChannelVal = 0;
                    double ConversionVal = 0;
                    switch (findKvar)
                    {
                        case 1:
                            ChannelVal = MyDataHistory.Channel1;
                            ConversionVal = ConversionFactor[0];
                            break;
                        case 2:
                            ChannelVal = MyDataHistory.Channel2;
                            ConversionVal = ConversionFactor[1];
                            break;
                        case 3:
                            ChannelVal = MyDataHistory.Channel3;
                            ConversionVal = ConversionFactor[2];
                            break;
                        case 4:
                            ChannelVal = MyDataHistory.Channel4;
                            ConversionVal = ConversionFactor[3];
                            break;
                        case 5:
                            ChannelVal = MyDataHistory.Channel5;
                            ConversionVal = ConversionFactor[4];
                            break;
                        case 6:
                            ChannelVal = MyDataHistory.Channel7;
                            ConversionVal = ConversionFactor[5];
                            break;
                        case 7:
                            ChannelVal = MyDataHistory.Channel7;
                            ConversionVal = ConversionFactor[6];
                            break;
                        case 8:
                            ChannelVal = MyDataHistory.Channel8;
                            ConversionVal = ConversionFactor[7];
                            break;
                    }

                    string typestring = "OffPeak";
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);

                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKVarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                PeakKvarhCost = TypePeriod.CostcPerKWh;
                                PeakKvarhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.OrangeRed);
                                typestring = "Peak";
                                break;
                            case 2:
                                //standard"
                                StandKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                               //  ChartColors.Add(Color.Yellow);
                                typestring = "Stand";
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                //   ChartColors.Add(Color.LawnGreen);
                                typestring = "OffPeak";
                                break;
                        }
                        TotKvarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        TotKvarCnt += (ChannelVal / ConversionVal);
                        //If ConversionFactor(0) = 0 Then
                        //    table.Rows.Add(MyDataHistory.TimeStamp, ChannelVal * (MyPeriod / 60)) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                        //Else
                        //    table.Rows.Add(MyDataHistory.TimeStamp, (ChannelVal / ConversionVal) * (MyPeriod / 60))
                        //End If

                        //'
                        //If ConversionFactor(0) = 0 Then
                        //    CalCValue = ChannelVal * (MyPeriod / 60)
                        //Else
                        //    CalCValue = (ChannelVal / ConversionVal) * (MyPeriod / 60)
                        //End If
                        //'table.Rows.Add(MyDataHistory.TimeStamp, CalCValue) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                        //'Try
                        //'    ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + (CalCValue).ToString + vbCrLf)
                        //'Catch ex As Exception

                        //'End Try

                        //CurVal += CalCValue
                        //Select Case cmbDataSet.SelectedValue
                        //    Case 0 'all data
                        //        table.Rows.Add(CurBatchDate, CurVal)
                        //        ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //        CurVal = 0
                        //        LoadBatchDate = True
                        //    Case 1 '12 hour
                        //        If DateDiff(DateInterval.Hour, CurBatchDate, MyDataHistory.TimeStamp) >= 12 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //    Case 2 'daily
                        //        If DateDiff(DateInterval.Day, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //    Case 3 'weekly
                        //        If DateDiff(DateInterval.WeekOfYear, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //    Case 4 'monthly
                        //        If DateDiff(DateInterval.Month, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //End Select


                        //'
                        //default standard
                    }
                    else
                    {
                        StandKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                        //ChartColors.Add(Color.Yellow);
                        typestring = "Stand";
                        TotKvarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        TotKvarCnt += (ChannelVal / ConversionVal);
                    }



                    //KWH findMW

                    // table.Rows.Add(MyDataHistory.TimeStamp.Day.ToString(), (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60));

                }
                catch (Exception ex)
                {
                }

                // End If
            }
                table.Rows.Add("Off Peak", OffPeakKWhCnt);
                ExportStr.Append("Off Peak" + "," + (OffPeakKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Standard", StandKWhCnt);
                ExportStr.Append("Standard" + "," + (StandKWhCnt).ToString() + Constants.vbCrLf);
                table.Rows.Add("Peak", PeakKVarhCnt);
                ExportStr.Append("Peak" + "," + (PeakKVarhCnt).ToString() + Constants.vbCrLf);

         

               ChartColors.Add(Color.LawnGreen);
                ChartColors.Add(Color.Yellow);
                ChartColors.Add(Color.OrangeRed);
            
            // Dim Acnt As Integer
            //For Acnt = 0 To 0
            //    If MyDataChannels.ChannelNames.Contains((Acnt).ToString) = True Then
            //        DLchartLayer1.Series.Add(numericTimeSeries1(Acnt))
            //        DLseries1 = numericTimeSeries1(Acnt)
            //        MyDateLinechart3.Series.Add(DLseries1)
            //    End If
            //Next
            MyDateLinechart3.DataSource = table;

            MyDateLinechart3.DataBind();
            ///''''''''''''''''''
            // Set X axis
            //DLxAxis1.Labels.Orientation = TextOrientation.Horizontal
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:dd>"
            //'DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //'DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //' Set Y axis
            //' Set the ChartType
            //DLchartLayer1.ChartType = ChartType.ColumnChart
            //' Set Axis Type
            //SetAxisTypes(DLchartLayer1)
            //'DLchartLayer1.AxisY.Extent = 130
            //'Dim atype As New AxisDataType
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLchartLayer1.AxisY.DataType = AxisDataType.Numeric

            //'DLchartLayer2.AxisX.DataType = AxisDataType.Time
            //' DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //' DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            //' Add the series to the ChartLayer's Series collection.
            MyDateLinechart3.Width = 500;
            MyDateLinechart3.Height = 500;
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart3.CompositeChart.Legends.Add(legend1)
            //'MyDateLinechart3.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            //' MyDateLinechart3.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            //legend1.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            //legend1.Bounds = New Rectangle(1, 1, 10, 10)
            //'Dim legend2 As New CompositeLegend()
            //'legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //'MyDateLinechart3.CompositeChart.Legends.Add(legend2)
            //'legend2.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(1))
            //'legend2.BoundsMeasureType = MeasureType.Percentage
            //'legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            //MyDateLinechart3.CompositeChart.ChartAreas(0).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart3.CompositeChart.ChartAreas(0).Bounds = New Rectangle(5, 0, 90, 100)
            //Dim MyBorder As New Infragistics.UltraChart.Resources.Appearance.BorderAppearance
            //MyBorder.Thickness = 0
            //MyDateLinechart3.CompositeChart.ChartAreas(0).Border = MyBorder
            MyDateLinechart3.ColorModel.ModelStyle = ColorModels.CustomLinear;
            //Color[] ChartColors4 = null;
            //ChartColors4 = new Color[] {



                
            //        Color.Red,
            //       Color.Yellow,
            //       Color.Green,

            //};
            //ReDim ChartColors4(0)
            //For Each MyColor As Color In ChartColorsd
            //    ChartColors4(ChartColors4.GetUpperBound(0)) = MyColor
            //    ReDim Preserve ChartColors4(ChartColors4.GetUpperBound(0) + 1)
            //Next
            //Dim ChartColors5() As Color
            //ChartColors5 = New Color() {Color.Green, Color.Yellow, Color.Red, Color.Green, Color.Red, Color.Black, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            //MyDateLinechart3.ColorModel.CustomPalette = ChartColors.ToArray()
            //if (Convert.ToInt32(ddlData.SelectedValue) == 0)
            //{
            //    // ERROR: Not supported in C#: ReDimStatement

            //    foreach (Color MyColor in ChartColors)
            //    {
            //        ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //        Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //    }
            //}
            //else
            //{
            //    // ERROR: Not supported in C#: ReDimStatement

            //    foreach (Color MyColor in ChartColors)
            //    {
            //        ChartColors4[ChartColors4.GetUpperBound(0)] = Color.LawnGreen;
            //        Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //    }
            //}
            MyDateLinechart3.ColorModel.CustomPalette = ChartColors.ToArray();
            MyDateLinechart3.Data.SwapRowsAndColumns = true;
           // MyDateLinechart3.EnableScrollBar = True;
            this.MyDateLinechart3.Legend.Location = LegendLocation.Right;
            int MyItemVal = MyDateLinechart3.ColumnChart.ChartText.Add(new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance());
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].ItemFormatString = "<DATA_VALUE:0.###>";
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Row = -2;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Column = -2;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].VerticalAlign = StringAlignment.Far;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].HorizontalAlign = StringAlignment.Center;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Visible = true;
            //MyDateLinechart3.ColumnChart.ChartText(MyItemVal - 1).ChartTextFont.
            MyDateLinechart3.ColumnChart.ColumnSpacing = 1;
            //Me.MyDateLinechart3.ColumnChart.ChartText(0).Visible = True
            //MyDateLinechart3.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart3.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            MyDateLinechart3.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart3.Axis.X.TickmarkInterval = MyData.Count / 15;
            //15.0
            this.DivKvarReport.Controls.Add(MyDateLinechart3);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart3.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            StringBuilder MyHtmlStr = new StringBuilder();
            try
            {
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=450 ><tr><td>");
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Peak Kvarh:" + PeakKVarhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Std Kvarh :" + StandKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Off Peak Kvarh :" + OffPeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("</td></tr></table>");
                this.DivKvarStatsReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }
            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart3.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart3.ID);
            }

        }
        public void DrawLandisTotalKvar2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            //attach event for legent width
            // MyDateLinechart3.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart3.ChartDrawItem += MyDateLinechart2_ChartDrawItem2;
            MyDateLinechart3.ID = "Landis Gyr TotKvarh" + SensorDet.ID.ToString();
            MyDateLinechart3.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart3.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //sets the horizontal alignment of the text
            MyDateLinechart3.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart3.TitleTop.Margins.Bottom = 2;
            MyDateLinechart3.TitleTop.Margins.Top = 2;
            MyDateLinechart3.TitleTop.Margins.Left = 2;
            MyDateLinechart3.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart3.TitleTop.Text = "Landis Total Kvarh Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart3.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart3.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart3.TitleTop.WrapText = true;
            // Set composite charts
            //MyDateLinechart3.ChartType = ChartType.Composite
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart3.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart3.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.ColumnChart;
            MyDateLinechart3.LineChart.TreatDateTimeAsString = false;
            MyDateLinechart3.Axis.X.TickmarkIntervalType = AxisIntervalType.Hours;
            MyDateLinechart3.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart3.Axis.X.TimeAxisStyle.TimeAxisStyle = RulerGenre.Discrete;
            MyDateLinechart3.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            MyDateLinechart3.Axis.X.Labels.Font = new Font("Tahoma", 7);
            MyDateLinechart3.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            MyDateLinechart3.Data.ZeroAligned = true;
            //Dim DLxAxis1 As New AxisItem()
            //DLxAxis1.axisNumber = AxisNumber.X_Axis
            //DLxAxis1.DataType = AxisDataType.String
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis1.LineThickness = 1
            // ' Create an Y axis
            //Dim DLyAxis1 As New AxisItem()
            //DLyAxis1.axisNumber = AxisNumber.Y_Axis
            //DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis1.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis1.LineThickness = 1
            //Dim DLyAxis2 As New AxisItem()
            //DLyAxis2.axisNumber = AxisNumber.Y2_Axis
            //DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>"
            //DLyAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLyAxis2.LineThickness = 1
            //myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis1)
            //'myDLChartArea1.Axes.Add(DLxAxis1)
            //myDLChartArea1.Axes.Add(DLyAxis2)
            //' Set the axes
            //DLchartLayer1.AxisX = DLxAxis1
            //DLchartLayer1.AxisY = DLyAxis1
            //' Set the ChartArea
            //DLchartLayer1.ChartArea = myDLChartArea1
            //' Add the ChartLayer to the ChartLayers collection
            //MyDateLinechart3.CompositeChart.ChartLayers.Add(DLchartLayer1)
            ///''''''''''''''''''
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
            // LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            int findKvar = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                {
                    //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                    //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                    //numericTimeSeries1(Bcnt).Label = CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                    if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelName == "Q1 Kvar")
                    {
                        findKvar = Bcnt + 1;
                    }
                    ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                    //End If
                }
                else
                {
                    //numericTimeSeries1(Bcnt) = New NumericTimeSeries
                    // numericTimeSeries1(Bcnt).Label = ""
                    ConversionFactor[Bcnt] = 0;
                }
            }
            for (Bcnt = 0; Bcnt <= 0; Bcnt++)
            {
                //numericTimeSeries1(Bcnt) = New DataPoint
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                // numericTimeSeries1(Bcnt) = New DataPoint
                //numericTimeSeries1(Bcnt).Label = "Kwh" 'CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            // LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKvarhCnt = 0;
            double TotKvarCnt = 0;

            double PeakKVarhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKvarhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKvarhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0
            List<Color> ChartColors = new List<Color>();
            DataTable table = new DataTable();

            table.Columns.Add("Type", System.Type.GetType("System.String"));
            //table.Columns.Add("ID", System.Type.GetType("System.Int32"));
            table.Columns.Add("Kvarh", System.Type.GetType("System.Double"));
            //table.Rows.Add("label", 5)

            System.DateTime CurBatchDate = default(System.DateTime);
            double CurVal = 0;
            bool LoadBatchDate = true;
            double CalCValue = 0;


            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if (LoadBatchDate)
                    {
                        CurBatchDate = MyDataHistory.TimeStamp;
                        LoadBatchDate = false;
                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    double ChannelVal = 0;
                    double ConversionVal = 0;
                    switch (findKvar)
                    {
                        case 1:
                            ChannelVal = MyDataHistory.Channel1;
                            ConversionVal = ConversionFactor[0];
                            break;
                        case 2:
                            ChannelVal = MyDataHistory.Channel2;
                            ConversionVal = ConversionFactor[1];
                            break;
                        case 3:
                            ChannelVal = MyDataHistory.Channel3;
                            ConversionVal = ConversionFactor[2];
                            break;
                        case 4:
                            ChannelVal = MyDataHistory.Channel4;
                            ConversionVal = ConversionFactor[3];
                            break;
                        case 5:
                            ChannelVal = MyDataHistory.Channel5;
                            ConversionVal = ConversionFactor[4];
                            break;
                        case 6:
                            ChannelVal = MyDataHistory.Channel7;
                            ConversionVal = ConversionFactor[5];
                            break;
                        case 7:
                            ChannelVal = MyDataHistory.Channel7;
                            ConversionVal = ConversionFactor[6];
                            break;
                        case 8:
                            ChannelVal = MyDataHistory.Channel8;
                            ConversionVal = ConversionFactor[7];
                            break;
                    }

                    string typestring = "OffPeak";
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);

                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == 0) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKVarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                PeakKvarhCost = TypePeriod.CostcPerKWh;
                                PeakKvarhLabel = TypePeriod.ChargeName;
                                // ChartColors.Add(Color.OrangeRed);
                                typestring = "Peak";
                                break;
                            case 2:
                                //standard"
                                StandKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                //  ChartColors.Add(Color.Yellow);
                                typestring = "Stand";
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                //   ChartColors.Add(Color.LawnGreen);
                                typestring = "OffPeak";
                                break;
                        }
                        TotKvarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        TotKvarCnt += (ChannelVal / ConversionVal);
                        //If ConversionFactor(0) = 0 Then
                        //    table.Rows.Add(MyDataHistory.TimeStamp, ChannelVal * (MyPeriod / 60)) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                        //Else
                        //    table.Rows.Add(MyDataHistory.TimeStamp, (ChannelVal / ConversionVal) * (MyPeriod / 60))
                        //End If

                        //'
                        //If ConversionFactor(0) = 0 Then
                        //    CalCValue = ChannelVal * (MyPeriod / 60)
                        //Else
                        //    CalCValue = (ChannelVal / ConversionVal) * (MyPeriod / 60)
                        //End If
                        //'table.Rows.Add(MyDataHistory.TimeStamp, CalCValue) ' = MyDataHistory.Channel1 * (MyPeriod / 60)
                        //'Try
                        //'    ExportStr.Append(MyDataHistory.TimeStamp.ToString + "," + (CalCValue).ToString + vbCrLf)
                        //'Catch ex As Exception

                        //'End Try

                        //CurVal += CalCValue
                        //Select Case cmbDataSet.SelectedValue
                        //    Case 0 'all data
                        //        table.Rows.Add(CurBatchDate, CurVal)
                        //        ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //        CurVal = 0
                        //        LoadBatchDate = True
                        //    Case 1 '12 hour
                        //        If DateDiff(DateInterval.Hour, CurBatchDate, MyDataHistory.TimeStamp) >= 12 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //    Case 2 'daily
                        //        If DateDiff(DateInterval.Day, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //    Case 3 'weekly
                        //        If DateDiff(DateInterval.WeekOfYear, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //    Case 4 'monthly
                        //        If DateDiff(DateInterval.Month, CurBatchDate, MyDataHistory.TimeStamp) >= 1 Then
                        //            table.Rows.Add(CurBatchDate, CurVal)
                        //            ExportStr.Append(CurBatchDate.ToString + "," + (CurVal).ToString + vbCrLf)
                        //            CurVal = 0
                        //            CurBatchDate = MyDataHistory.TimeStamp
                        //            LoadBatchDate = True
                        //        End If
                        //End Select


                        //'
                        //default standard
                    }
                    else
                    {
                        StandKWhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                        //ChartColors.Add(Color.Yellow);
                        typestring = "Stand";
                        TotKvarhCnt += (ChannelVal / ConversionVal) * (MyPeriod / 60);
                        TotKvarCnt += (ChannelVal / ConversionVal);
                    }



                    //KWH findMW

                    // table.Rows.Add(MyDataHistory.TimeStamp.Day.ToString(), (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60));

                }
                catch (Exception ex)
                {
                }

                // End If
            }
            table.Rows.Add("Off Peak", OffPeakKWhCnt);
            ExportStr.Append("Off Peak" + "," + (OffPeakKWhCnt).ToString() + Constants.vbCrLf);
            table.Rows.Add("Standard", StandKWhCnt);
            ExportStr.Append("Standard" + "," + (StandKWhCnt).ToString() + Constants.vbCrLf);
            table.Rows.Add("Peak", PeakKVarhCnt);
            ExportStr.Append("Peak" + "," + (PeakKVarhCnt).ToString() + Constants.vbCrLf);



            ChartColors.Add(Color.LawnGreen);
            ChartColors.Add(Color.Yellow);
            ChartColors.Add(Color.OrangeRed);

            // Dim Acnt As Integer
            //For Acnt = 0 To 0
            //    If MyDataChannels.ChannelNames.Contains((Acnt).ToString) = True Then
            //        DLchartLayer1.Series.Add(numericTimeSeries1(Acnt))
            //        DLseries1 = numericTimeSeries1(Acnt)
            //        MyDateLinechart3.Series.Add(DLseries1)
            //    End If
            //Next
            MyDateLinechart3.DataSource = table;

            MyDateLinechart3.DataBind();
            ///''''''''''''''''''
            // Set X axis
            //DLxAxis1.Labels.Orientation = TextOrientation.Horizontal
            //DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:dd>"
            //'DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //'DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            //' Set Y axis
            //' Set the ChartType
            //DLchartLayer1.ChartType = ChartType.ColumnChart
            //' Set Axis Type
            //SetAxisTypes(DLchartLayer1)
            //'DLchartLayer1.AxisY.Extent = 130
            //'Dim atype As New AxisDataType
            //DLchartLayer1.AxisX.DataType = AxisDataType.Time
            //DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLchartLayer1.AxisY.DataType = AxisDataType.Numeric

            //'DLchartLayer2.AxisX.DataType = AxisDataType.Time
            //' DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //' DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            //' Add the series to the ChartLayer's Series collection.
            MyDateLinechart3.Width = 500;
            MyDateLinechart3.Height = 500;
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart3.CompositeChart.Legends.Add(legend1)
            //'MyDateLinechart3.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            //' MyDateLinechart3.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            //legend1.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            //legend1.Bounds = New Rectangle(1, 1, 10, 10)
            //'Dim legend2 As New CompositeLegend()
            //'legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //'MyDateLinechart3.CompositeChart.Legends.Add(legend2)
            //'legend2.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(1))
            //'legend2.BoundsMeasureType = MeasureType.Percentage
            //'legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            //MyDateLinechart3.CompositeChart.ChartAreas(0).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart3.CompositeChart.ChartAreas(0).Bounds = New Rectangle(5, 0, 90, 100)
            //Dim MyBorder As New Infragistics.UltraChart.Resources.Appearance.BorderAppearance
            //MyBorder.Thickness = 0
            //MyDateLinechart3.CompositeChart.ChartAreas(0).Border = MyBorder
            MyDateLinechart3.ColorModel.ModelStyle = ColorModels.CustomLinear;
            //Color[] ChartColors4 = null;
            //ChartColors4 = new Color[] {




            //        Color.Red,
            //       Color.Yellow,
            //       Color.Green,

            //};
            //ReDim ChartColors4(0)
            //For Each MyColor As Color In ChartColorsd
            //    ChartColors4(ChartColors4.GetUpperBound(0)) = MyColor
            //    ReDim Preserve ChartColors4(ChartColors4.GetUpperBound(0) + 1)
            //Next
            //Dim ChartColors5() As Color
            //ChartColors5 = New Color() {Color.Green, Color.Yellow, Color.Red, Color.Green, Color.Red, Color.Black, Color.Blue, Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            //MyDateLinechart3.ColorModel.CustomPalette = ChartColors.ToArray()
            //if (Convert.ToInt32(ddlData.SelectedValue) == 0)
            //{
            //    // ERROR: Not supported in C#: ReDimStatement

            //    foreach (Color MyColor in ChartColors)
            //    {
            //        ChartColors4[ChartColors4.GetUpperBound(0)] = MyColor;
            //        Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //    }
            //}
            //else
            //{
            //    // ERROR: Not supported in C#: ReDimStatement

            //    foreach (Color MyColor in ChartColors)
            //    {
            //        ChartColors4[ChartColors4.GetUpperBound(0)] = Color.LawnGreen;
            //        Array.Resize(ref ChartColors4, ChartColors4.GetUpperBound(0) + 2);
            //    }
            //}
            MyDateLinechart3.ColorModel.CustomPalette = ChartColors.ToArray();
            MyDateLinechart3.Data.SwapRowsAndColumns = true;
            // MyDateLinechart3.EnableScrollBar = True;
            this.MyDateLinechart3.Legend.Location = LegendLocation.Right;
            int MyItemVal = MyDateLinechart3.ColumnChart.ChartText.Add(new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance());
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].ItemFormatString = "<DATA_VALUE:0.###>";
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Row = -2;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Column = -2;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].VerticalAlign = StringAlignment.Far;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].HorizontalAlign = StringAlignment.Center;
            MyDateLinechart3.ColumnChart.ChartText[MyItemVal - 1].Visible = true;
            //MyDateLinechart3.ColumnChart.ChartText(MyItemVal - 1).ChartTextFont.
            MyDateLinechart3.ColumnChart.ColumnSpacing = 1;
            //Me.MyDateLinechart3.ColumnChart.ChartText(0).Visible = True
            //MyDateLinechart3.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart3.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            MyDateLinechart3.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
            MyDateLinechart3.Axis.X.TickmarkInterval = MyData.Count / 15;
            //15.0
            this.DivKvarReport.Controls.Add(MyDateLinechart3);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart3.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart3.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            StringBuilder MyHtmlStr = new StringBuilder();
            try
            {
                MyHtmlStr.Append("<table class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered\" BorderColor=\"#316AC5\" border=1 width=450 ><tr><td>");
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Metering Interval  :" + MyPeriod.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Peak Kvarh:" + PeakKVarhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Std Kvarh :" + StandKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("Off Peak Kvarh :" + OffPeakKWhCnt.ToString());
                MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                MyHtmlStr.Append("</td></tr></table>");
                this.DivKvarStatsReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }
            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart3.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart3.ID);
            }

        }

        public int returnMeteringPeriod(int PEriodCode)
        {
            int retval = 30;
            try
            {
                switch (PEriodCode)
                {
                    case 0:
                        retval = 1;
                        break;
                    case 1:
                        retval = 2;
                        break;
                    case 2:
                        retval = 3;
                        break;
                    case 3:
                        retval = 4;
                        break;
                    case 4:
                        retval = 5;
                        break;
                    case 5:
                        retval = 6;
                        break;
                    case 6:
                        retval = 10;
                        break;
                    case 7:
                        retval = 15;
                        break;
                    case 8:
                        retval = 20;
                        break;
                    case 9:
                        retval = 30;
                        break;
                    case 10:
                        retval = 60;

                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return retval;

        }
        public int FindPeriod(DateTime CheckTime, LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges[] ActivePeriods, ref LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges MyActiveCahrge)
        {
            //Dim myretval As LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges 'off peak by default
            try
            {
                foreach (LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges myActiveEnergyPeriod in ActivePeriods)
                {

                    if ((myActiveEnergyPeriod == null) == false)
                    {
                        if (myActiveEnergyPeriod.FindPeriod(CheckTime.Month))
                        {
                            //myActiveEnergyPeriod.ChargePeriods()
                            for (int MyPeriodcnt = 0; MyPeriodcnt <= Information.UBound(myActiveEnergyPeriod.ChargePeriods) - 1; MyPeriodcnt++)
                            {
                                LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharges = myActiveEnergyPeriod.ChargePeriods[MyPeriodcnt];
                                if (CheckPeriodDay(CheckTime, MyCharges))
                                {
                                    //day match now time match
                                    if (CheckPeriodTime(CheckTime, MyCharges))
                                    {
                                        //day match now time match
                                        //MyCharges.MeteringChargeType()
                                        MyActiveCahrge = myActiveEnergyPeriod;
                                        return MyPeriodcnt;
                                    }
                                }
                            }
                        }
                    }

                }


            }
            catch (Exception ex)
            {
            }
            return 0;
        }
        public bool CheckPeriodTime(DateTime CheckTime, LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharges)
        {

            try
            {
                //Weekday so must match
                if (MyCharges.StartTime.TimeOfDay <= CheckTime.TimeOfDay & MyCharges.EndTime.TimeOfDay > CheckTime.TimeOfDay)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public bool CheckPeriodDay(DateTime CheckTime, LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharges)
        {
            try
            {
                //everyday so must match
                if (MyCharges.Days == 7)
                {
                    return true;
                }
                //Weekday so must match
                if (MyCharges.Days == 8 & (CheckTime.DayOfWeek > 0 & (int)CheckTime.DayOfWeek < 6))
                {
                    return true;
                }
                //actual day must match
                if (MyCharges.Days < 7 & ((int)CheckTime.DayOfWeek == MyCharges.Days))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public void AddLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection
            List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData = null;
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true)
                {
                    //If cmbDataSet.SelectedValue = 0 Then
                    try
                    {
                        MyData = MyRem.LiveMonServer.GetMeteringProfileRecord(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));

                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        MyDataMarkers = MyRem.LiveMonServer.GetMeteringProfileMarkers(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));

                    }
                    catch (Exception ex)
                    {
                    }

                    //Else
                    //MyData = MyRem.server1.GetFilteredSensorHistory(cmbDataSet.SelectedValue, SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
                    // End If
                }

            }
            catch (Exception ex)
            {
            }
            if ((MyData == null) == false)
            {
                switch (SensorDet.Type)
                {
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile:
                        DrawElsterA1140ProfileGraphs(SensorDet, MyData, MyDataMarkers);
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile:
                        DrawElsterA1700ProfileGraphs(SensorDet, MyData, MyDataMarkers);
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LandisGyrE650Profile:
                        DrawLandisProfileGraphs(SensorDet, MyData, MyDataMarkers);
                        break;
                }
            }


        }

        public void DrawHtmlLine()
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<br/><hr/><br/>");
        }

        public void DrawElsterA1700ProfileGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers)
        {
            try
            {
                DrawElsterA1700ProfileGraphs2(SensorDet, MyData, MyDataMarkers);
                //Active Power
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }


        }

        public void DrawElsterA1700ProfileGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers)
        {
            //attach event for legent width
           // MyDateLinechart1.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart1.ChartDrawItem += ultraChart1_ChartDrawItem;
            MyDateLinechart1.ID = "ElsterA1700CKWA" + SensorDet.ID.ToString();
            MyDateLinechart1.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart1.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //MyDateLinechart1.TitleTop.FontSizeBestFit = false
            //sets the horizontal alignment of the text
            MyDateLinechart1.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart1.TitleTop.Margins.Bottom = 2;
            MyDateLinechart1.TitleTop.Margins.Top = 2;
            MyDateLinechart1.TitleTop.Margins.Left = 2;
            MyDateLinechart1.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart1.TitleTop.Text = "Elster Cumulative KWh Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart1.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart1.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart1.TitleTop.WrapText = true;
            // Set composite charts
            MyDateLinechart1.ChartType = ChartType.Composite;
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            //Dim myDLChartArea2 As New ChartArea()
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            //Dim DLchartLayer2 As New ChartLayerAppearance()
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            //DLchartLayer1.Key = "1"
            //DLchartLayer1.ChartLayer.LayerID = "1"
            // DLchartLayer2.ChartType = ChartType.LineChart
            //DLchartLayer2.Key = "2"
            //DLchartLayer2.ChartLayer.LayerID = "2"
            // Create an X axis
            AxisItem DLxAxis1 = new AxisItem();
            DLxAxis1.axisNumber = AxisNumber.X_Axis;
            DLxAxis1.DataType = AxisDataType.String;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>";
            DLxAxis1.Labels.Font = new Font("Tahoma", 7);
            DLxAxis1.LineThickness = 1;
            //Dim DLxAxis2 As New AxisItem()
            //DLxAxis2.axisNumber = AxisNumber.X_Axis
            //DLxAxis2.DataType = AxisDataType.String
            //DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis2.LineThickness = 1
            // Create an Y axis
            AxisItem DLyAxis1 = new AxisItem();
            DLyAxis1.axisNumber = AxisNumber.Y_Axis;
            DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis1.Labels.Font = new Font("Tahoma", 7);
            DLyAxis1.LineThickness = 1;
            AxisItem DLyAxis2 = new AxisItem();
            DLyAxis2.axisNumber = AxisNumber.Y2_Axis;
            DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis2.Labels.Font = new Font("Tahoma", 7);
            DLyAxis2.LineThickness = 1;
            myDLChartArea1.Axes.Add(DLxAxis1);
            myDLChartArea1.Axes.Add(DLyAxis1);
            //myDLChartArea1.Axes.Add(DLxAxis1)
            myDLChartArea1.Axes.Add(DLyAxis2);
            // Set the axes
            DLchartLayer1.AxisX = DLxAxis1;
            DLchartLayer1.AxisY = DLyAxis1;
            // DLchartLayer2.AxisX = DLxAxis1
            //DLchartLayer2.AxisY = DLyAxis2
            // Set the ChartArea
            DLchartLayer1.ChartArea = myDLChartArea1;
            // DLchartLayer2.ChartArea = myDLChartArea1
            // Add the ChartLayer to the ChartLayers collection
            MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer1);
            // MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer2)
            ///''''''''''''''''''
            ISeries DLseries1 = null;
            NumericTimeSeries[] numericTimeSeries1 = new NumericTimeSeries[2];
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            for (Bcnt = 0; Bcnt <= 0; Bcnt++)
            {
                numericTimeSeries1[Bcnt]= new NumericTimeSeries();
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                numericTimeSeries1[Bcnt].Label = "Kwh";
                //CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKWhCnt = 0;
            double TotKWCnt = 0;

            System.DateTime CurBatchDate = default(System.DateTime);
            double CurVal = 0;
            bool LoadBatchDate = true;

            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                int myret = 0;
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    if (LoadBatchDate)
                    {
                        CurBatchDate = MyDataHistory.TimeStamp;
                        LoadBatchDate = false;
                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    //KWH findMW
                    TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);

                    //numericTimeSeries1(0).Points(myret).TimeValue = MyDataHistory.TimeStamp
                    //numericTimeSeries1(0).Points(myret).NumericValue = TotKWhCnt
                    CurVal += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    switch (ddlData.SelectedIndex)
                    {
                        case 0:
                            //all data
                            myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                            numericTimeSeries1[0].Points[myret].TimeValue = MyDataHistory.TimeStamp;
                            numericTimeSeries1[0].Points[myret].NumericValue = CurVal;
                            //CurVal = 0
                            LoadBatchDate = true;
                            break;
                        case 1:
                            //12 hour
                            if (DateAndTime.DateDiff(DateInterval.Hour, CurBatchDate, MyDataHistory.TimeStamp) >= 12)
                            {
                                myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                                numericTimeSeries1[0].Points[myret].TimeValue = CurBatchDate;
                                numericTimeSeries1[0].Points[myret].NumericValue = CurVal;
                                //CurVal = 0
                                CurBatchDate = MyDataHistory.TimeStamp;
                                LoadBatchDate = true;
                            }
                            break;
                        case 2:
                            //daily
                            if (DateAndTime.DateDiff(DateInterval.Day, CurBatchDate, MyDataHistory.TimeStamp) >= 1)
                            {
                                myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                                numericTimeSeries1[0].Points[myret].TimeValue = CurBatchDate;
                                numericTimeSeries1[0].Points[myret].NumericValue = CurVal;
                                //CurVal = 0
                                CurBatchDate = MyDataHistory.TimeStamp;
                                LoadBatchDate = true;
                            }
                            break;
                        case 3:
                            //weekly
                            if (DateAndTime.DateDiff(DateInterval.WeekOfYear, CurBatchDate, MyDataHistory.TimeStamp) >= 1)
                            {
                                myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                                numericTimeSeries1[0].Points[myret].TimeValue = CurBatchDate;
                                numericTimeSeries1[0].Points[myret].NumericValue = CurVal;
                                //CurVal = 0
                                CurBatchDate = MyDataHistory.TimeStamp;
                                LoadBatchDate = true;
                            }
                            break;
                        case 4:
                            //monthly
                            if (DateAndTime.DateDiff(DateInterval.Month, CurBatchDate, MyDataHistory.TimeStamp) >= 1)
                            {
                                myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                                numericTimeSeries1[0].Points[myret].TimeValue = CurBatchDate;
                                numericTimeSeries1[0].Points[myret].NumericValue = CurVal;
                                //CurVal = 0
                                CurBatchDate = MyDataHistory.TimeStamp;
                                LoadBatchDate = true;
                            }
                            break;
                    }

                }
                catch (Exception ex)
                {
                }

                // End If
            }

            int Acnt = 1;
            for (Acnt = 0; Acnt <= 0; Acnt++)
            {
                if (MyDataChannels.ChannelNames.Contains((Acnt).ToString()) == true)
                {
                    DLchartLayer1.Series.Add(numericTimeSeries1[Acnt]);
                    DLseries1 = numericTimeSeries1[Acnt];
                    MyDateLinechart1.Series.Add(DLseries1);
                }
            }

            ///''''''''''''''''''
            // Set X axis
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            //DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            // Set Y axis
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            // Set Axis Type
            SetAxisTypes(DLchartLayer1);
            //DLchartLayer1.AxisY.Extent = 130
            //Dim atype As New AxisDataType
            DLchartLayer1.AxisX.DataType = AxisDataType.Time;
            DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLchartLayer1.AxisY.DataType = AxisDataType.Numeric;

            //DLchartLayer2.AxisX.DataType = AxisDataType.Time
            // DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            // DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            // Add the series to the ChartLayer's Series collection.
            MyDateLinechart1.Width = 500;
            MyDateLinechart1.Height = 400;
            CompositeLegend legend1 = new CompositeLegend();
            legend1.LabelStyle.Font = new Font("Times New Roman", 10);
            MyDateLinechart1.CompositeChart.Legends.Add(legend1);
            //MyDateLinechart1.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            // MyDateLinechart1.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers[0]);
            legend1.BoundsMeasureType = MeasureType.Percentage;
            legend1.Bounds = new Rectangle(1, 1, 10, 10);
            //Dim legend2 As New CompositeLegend()
            //legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend2)
            //legend2.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(1))
            //legend2.BoundsMeasureType = MeasureType.Percentage
            //legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            this.MyDateLinechart1.Legend.Location = LegendLocation.Right;
            MyDateLinechart1.CompositeChart.ChartAreas[0].BoundsMeasureType = MeasureType.Percentage;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Bounds = new Rectangle(0, 20, 100, 80);
            Infragistics.UltraChart.Resources.Appearance.BorderAppearance MyBorder = new Infragistics.UltraChart.Resources.Appearance.BorderAppearance();
            MyBorder.Thickness = 0;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Border = MyBorder;
            //MyDateLinechart1.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart1.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
           // this.Charts.Controls.Add(MyDateLinechart1);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart1.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart1.ID);
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
                //If TypeOf e.Primitive.Layer Is Infragistics.UltraChart.Core.Layers.LineLayer Then
                //    e.Primitive.PE.Fill = System.Drawing.Color.Red

                //End If
                //If IsNothing(e.Primitive.Layer) = False Then
                //    e.Primitive.PE.Fill = System.Drawing.Color.Blue

                //End If
                //If (TypeOf e.Primitive Is Infragistics.UltraChart.Core.Primitives.Line) And TypeOf e.Primitive.Layer Is Infragistics.UltraChart.Core.Layers.LineLayer Then
                //    e.Primitive.PE.Fill = System.Drawing.Color.Pink
                //    'Else
                //    '    Select Case e.Primitive.Column
                //    '        Case 0
                //    '            e.Primitive.PE.Fill = System.Drawing.Color.Yellow
                //    '        Case 1
                //    '            e.Primitive.PE.Fill = System.Drawing.Color.Blue
                //    '        Case 2
                //    '            e.Primitive.PE.Fill = System.Drawing.Color.Red
                //    '    End Select
                //End If
            }
            catch (Exception ex)
            {
                Trace.Write("err" + ex.Message);
            }

        }
        //Handles UltraChart1.ChartDrawItem
        private void MyDateLinechart1_ChartDrawItem1(object sender, Infragistics.UltraChart.Shared.Events.ChartDrawItemEventArgs e)
        {
            if ((object.ReferenceEquals(e.Primitive.GetType(), typeof(Infragistics.UltraChart.Core.Primitives.Polyline))) & object.ReferenceEquals(e.Primitive.Layer, typeof(Infragistics.UltraChart.Core.Layers.LineLayer)))
            {
                e.Primitive.PE.Fill = System.Drawing.Color.Red;
                //Else
                //    Select Case e.Primitive.Column
                //        Case 0
                //            e.Primitive.PE.Fill = System.Drawing.Color.Yellow
                //        Case 1
                //            e.Primitive.PE.Fill = System.Drawing.Color.Blue
                //        Case 2
                //            e.Primitive.PE.Fill = System.Drawing.Color.Red
                //    End Select
            }

        }
        //Handles UltraChart1.ChartDrawItem
        private void MyDateLinechart2_ChartDrawItem2(object sender, Infragistics.UltraChart.Shared.Events.ChartDrawItemEventArgs e)
        {
            if ((object.ReferenceEquals(e.Primitive.GetType(), typeof(Infragistics.UltraChart.Core.Primitives.Box))))
            {
                int columnWidth = 40;
                Infragistics.UltraChart.Core.Primitives.Box box = (Infragistics.UltraChart.Core.Primitives.Box)e.Primitive;
                //as Infragistics.UltraChart.Core.Primitives.Box;
                if ((box == null))
                {
                    return;
                }
                if ((box.DataPoint == null))
                {
                    return;
                }
                //Dim dWidth As Integer = box.rect.Height - columnWidth
                //If (dWidth <= 0) Then
                //    Return
                //End If
                //box.rect.Height = columnWidth
                //box.rect.X += dWidth / 2
                int dWidth = box.rect.Width - columnWidth;
                if ((dWidth <= 0))
                {
                    return;
                }
                box.rect.Width = columnWidth;
                box.rect.X += dWidth / 4;


              //  box.rect.Y += dWidth / 2;
            }



        }
        public void DrawElsterA1140ProfileGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers)
        {
            try
            {
                DrawElsterA1140ProfileGraphs2(SensorDet, MyData, MyDataMarkers);
                //Active Power
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }



        }
        public void DrawElsterA1140ProfileGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers)
        {
            //attach event for legent width
            //MyDateLinechart1.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart1.ChartDrawItem += ultraChart1_ChartDrawItem;
            MyDateLinechart1.ID = "ElsterA1140CKWA" + SensorDet.ID.ToString();
            MyDateLinechart1.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart1.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //MyDateLinechart1.TitleTop.FontSizeBestFit = false
            //sets the horizontal alignment of the text
            MyDateLinechart1.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart1.TitleTop.Margins.Bottom = 2;
            MyDateLinechart1.TitleTop.Margins.Top = 2;
            MyDateLinechart1.TitleTop.Margins.Left = 2;
            MyDateLinechart1.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart1.TitleTop.Text = "Elster Cumulative KWh Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart1.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart1.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart1.TitleTop.WrapText = true;
            // Set composite charts
            MyDateLinechart1.ChartType = ChartType.Composite;
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            //Dim myDLChartArea2 As New ChartArea()
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            //Dim DLchartLayer2 As New ChartLayerAppearance()
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            //DLchartLayer1.Key = "1"
            //DLchartLayer1.ChartLayer.LayerID = "1"
            // DLchartLayer2.ChartType = ChartType.LineChart
            //DLchartLayer2.Key = "2"
            //DLchartLayer2.ChartLayer.LayerID = "2"
            // Create an X axis
            AxisItem DLxAxis1 = new AxisItem();
            DLxAxis1.axisNumber = AxisNumber.X_Axis;
            DLxAxis1.DataType = AxisDataType.String;
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            DLxAxis1.Labels.Font = new Font("Tahoma", 7);
            DLxAxis1.LineThickness = 1;
            myDLChartArea1.Axes.Add(DLxAxis1);
            //Dim DLxAxis2 As New AxisItem()
            //DLxAxis2.axisNumber = AxisNumber.X_Axis
            //DLxAxis2.DataType = AxisDataType.String
            //DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis2.LineThickness = 1
            // Create an Y axis
            AxisItem DLyAxis1 = new AxisItem();
            DLyAxis1.axisNumber = AxisNumber.Y_Axis;
            DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis1.Labels.Font = new Font("Tahoma", 7);
            DLyAxis1.LineThickness = 1;
            AxisItem DLyAxis2 = new AxisItem();
            DLyAxis2.axisNumber = AxisNumber.Y2_Axis;
            DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis2.Labels.Font = new Font("Tahoma", 7);
            DLyAxis2.LineThickness = 1;

            myDLChartArea1.Axes.Add(DLyAxis1);
            //myDLChartArea1.Axes.Add(DLxAxis1)
            myDLChartArea1.Axes.Add(DLyAxis2);
            // Set the axes
            DLchartLayer1.AxisX = DLxAxis1;
            DLchartLayer1.AxisY = DLyAxis1;
            // DLchartLayer2.AxisX = DLxAxis1
            //DLchartLayer2.AxisY = DLyAxis2
            // Set the ChartArea
            DLchartLayer1.ChartArea = myDLChartArea1;
            // DLchartLayer2.ChartArea = myDLChartArea1
            // Add the ChartLayer to the ChartLayers collection
            MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer1);
            // MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer2)
            ///''''''''''''''''''
            ISeries DLseries1 = null;
            NumericTimeSeries[] numericTimeSeries1 = new NumericTimeSeries[2];
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[9];
            int findMVA = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                numericTimeSeries1[Bcnt].Label = "Kwh";
                //CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                int myret = 0;
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                    //KWH findMW
                    TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);

                    numericTimeSeries1[0].Points[myret].TimeValue = MyDataHistory.TimeStamp;
                    numericTimeSeries1[0].Points[myret].NumericValue = TotKWhCnt;

                }
                catch (Exception ex)
                {
                }

                // End If
            }

            int Acnt = 0;
            for (Acnt = 0; Acnt <= 0; Acnt++)
            {
                if (MyDataChannels.ChannelNames.Contains((Acnt).ToString()) == true)
                {
                    DLchartLayer1.Series.Add(numericTimeSeries1[Acnt]);
                    DLseries1 = numericTimeSeries1[Acnt];
                    MyDateLinechart1.Series.Add(DLseries1);
                }
            }

            ///''''''''''''''''''
            // Set X axis
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            //DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            // Set Y axis
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            // Set Axis Type
            SetAxisTypes(DLchartLayer1);
            //DLchartLayer1.AxisY.Extent = 130
            //Dim atype As New AxisDataType
            DLchartLayer1.AxisX.DataType = AxisDataType.Time;
            DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLchartLayer1.AxisY.DataType = AxisDataType.Numeric;

            //DLchartLayer2.AxisX.DataType = AxisDataType.Time
            // DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            // DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            // Add the series to the ChartLayer's Series collection.
            MyDateLinechart1.Width = 500;
            MyDateLinechart1.Height = 500;
            CompositeLegend legend1 = new CompositeLegend();
            legend1.LabelStyle.Font = new Font("Times New Roman", 10);
            MyDateLinechart1.CompositeChart.Legends.Add(legend1);
            //MyDateLinechart1.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            // MyDateLinechart1.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers[0]);
            legend1.BoundsMeasureType = MeasureType.Percentage;
            legend1.Bounds = new Rectangle(1, 1, 10, 10);
            //Dim legend2 As New CompositeLegend()
            //legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend2)
            //legend2.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(1))
            //legend2.BoundsMeasureType = MeasureType.Percentage
            //legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            MyDateLinechart1.CompositeChart.ChartAreas[0].BoundsMeasureType = MeasureType.Percentage;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Bounds = new Rectangle(5, 0, 90, 100);
            Infragistics.UltraChart.Resources.Appearance.BorderAppearance MyBorder = new Infragistics.UltraChart.Resources.Appearance.BorderAppearance();
            MyBorder.Thickness = 0;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Border = MyBorder;
            //MyDateLinechart1.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart1.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            this.Charts.Controls.Add(MyDateLinechart1);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart1.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart1.ID);
            }
        }

        public void DrawLandisProfileGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers)
        {
            try
            {
                DrawLandisProfileGraphs2(SensorDet, MyData, MyDataMarkers);
                //Active Power
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }



        }
        public void DrawLandisProfileGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers)
        {
            //attach event for legent width
            //MyDateLinechart1.EnableCrossHair = this.CheckBox1.Checked;

            MyDateLinechart1.ChartDrawItem += ultraChart1_ChartDrawItem;
            MyDateLinechart1.ID = "ElsterA1140CKWA" + SensorDet.ID.ToString();
            MyDateLinechart1.TitleTop.Extent = 45;
            //sets the font color
            MyDateLinechart1.TitleTop.FontColor = Color.Red;
            //sets whether the chart auto-sizes the font for the title
            //MyDateLinechart1.TitleTop.FontSizeBestFit = false
            //sets the horizontal alignment of the text
            MyDateLinechart1.TitleTop.HorizontalAlign = StringAlignment.Center;
            //sets the margins for the Top, Bottom, Left and right
            MyDateLinechart1.TitleTop.Margins.Bottom = 2;
            MyDateLinechart1.TitleTop.Margins.Top = 2;
            MyDateLinechart1.TitleTop.Margins.Left = 2;
            MyDateLinechart1.TitleTop.Margins.Right = 2;
            //sets the text to display for the chart in the title
            MyDateLinechart1.TitleTop.Text = "Elster Cumulative KWh Graphs :" + SensorDet.Caption;
            //sets the vertical alignment of the title
            MyDateLinechart1.TitleTop.VerticalAlign = StringAlignment.Near;
            //show/hide the referenced title
            MyDateLinechart1.TitleTop.Visible = true;
            //wrap/don't wrap the text
            MyDateLinechart1.TitleTop.WrapText = true;
            // Set composite charts
            MyDateLinechart1.ChartType = ChartType.Composite;
            // Create the ChartArea
            ChartArea myDLChartArea1 = new ChartArea();
            //Dim myDLChartArea2 As New ChartArea()
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea1);
            //MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea2)
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            //Dim DLchartLayer2 As New ChartLayerAppearance()
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            //DLchartLayer1.Key = "1"
            //DLchartLayer1.ChartLayer.LayerID = "1"
            // DLchartLayer2.ChartType = ChartType.LineChart
            //DLchartLayer2.Key = "2"
            //DLchartLayer2.ChartLayer.LayerID = "2"
            // Create an X axis
            AxisItem DLxAxis1 = new AxisItem();
            DLxAxis1.axisNumber = AxisNumber.X_Axis;
            DLxAxis1.DataType = AxisDataType.String;
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            DLxAxis1.Labels.Font = new Font("Tahoma", 7);
            DLxAxis1.LineThickness = 1;
            myDLChartArea1.Axes.Add(DLxAxis1);
            //Dim DLxAxis2 As New AxisItem()
            //DLxAxis2.axisNumber = AxisNumber.X_Axis
            //DLxAxis2.DataType = AxisDataType.String
            //DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL>"
            //DLxAxis2.Labels.Font = New Font("Tahoma", 7)
            //DLxAxis2.LineThickness = 1
            // Create an Y axis
            AxisItem DLyAxis1 = new AxisItem();
            DLyAxis1.axisNumber = AxisNumber.Y_Axis;
            DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis1.Labels.Font = new Font("Tahoma", 7);
            DLyAxis1.LineThickness = 1;
            AxisItem DLyAxis2 = new AxisItem();
            DLyAxis2.axisNumber = AxisNumber.Y2_Axis;
            DLyAxis2.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis2.Labels.Font = new Font("Tahoma", 7);
            DLyAxis2.LineThickness = 1;

            myDLChartArea1.Axes.Add(DLyAxis1);
            //myDLChartArea1.Axes.Add(DLxAxis1)
            myDLChartArea1.Axes.Add(DLyAxis2);
            // Set the axes
            DLchartLayer1.AxisX = DLxAxis1;
            DLchartLayer1.AxisY = DLyAxis1;
            // DLchartLayer2.AxisX = DLxAxis1
            //DLchartLayer2.AxisY = DLyAxis2
            // Set the ChartArea
            DLchartLayer1.ChartArea = myDLChartArea1;
            // DLchartLayer2.ChartArea = myDLChartArea1
            // Add the ChartLayer to the ChartLayers collection
            MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer1);
            // MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer2)
            ///''''''''''''''''''
            ISeries DLseries1 = null;
            NumericTimeSeries[] numericTimeSeries1 = new NumericTimeSeries[2];
            int Bcnt = 0;
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            double MyPeriod = 30;
            //default to 30 minutes
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                {
                    myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                    MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                }
            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1140Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next
            double[] ConversionFactor = new double[9];
            int findMVA = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                //If CType(SensorDet.Fields((Bcnt + 1).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).DisplayValue Then
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                numericTimeSeries1[Bcnt].Label = "Kwh";
                //CType(MyDataChannels.ChannelNames.Item((Bcnt).ToString), LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData).ChannelName
                ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1140Data.ChanelData)MyDataChannels.ChannelNames[Convert.ToString(Bcnt)]).ChannelDivisorUnits;
                //End If

            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            int MaxFieldCnt = 0;
            bool firstrec = true;
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {
                int myret = 0;
                // Dim ConversionFactor As Double = 0
                try
                {
                    if (firstrec)
                    {
                        firstrec = false;
                        txtStart.Text = MyDataHistory.TimeStamp.ToString();

                    }

                }
                catch (Exception ex)
                {
                }
                try
                {
                    txtEnd.Text = MyDataHistory.TimeStamp.ToString();

                }
                catch (Exception ex)
                {
                }

                try
                {
                    myret = numericTimeSeries1[0].Points.Add(new NumericTimeDataPoint());
                    //KWH findMW
                    TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                    TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);

                    numericTimeSeries1[0].Points[myret].TimeValue = MyDataHistory.TimeStamp;
                    numericTimeSeries1[0].Points[myret].NumericValue = TotKWhCnt;

                }
                catch (Exception ex)
                {
                }

                // End If
            }

            int Acnt = 0;
            for (Acnt = 0; Acnt <= 0; Acnt++)
            {
                if (MyDataChannels.ChannelNames.Contains((Acnt).ToString()) == true)
                {
                    DLchartLayer1.Series.Add(numericTimeSeries1[Acnt]);
                    DLseries1 = numericTimeSeries1[Acnt];
                    MyDateLinechart1.Series.Add(DLseries1);
                }
            }

            ///''''''''''''''''''
            // Set X axis
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
            //DLxAxis2.Labels.Orientation = TextOrientation.VerticalLeftFacing
            //DLxAxis2.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>"
            // Set Y axis
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            // Set Axis Type
            SetAxisTypes(DLchartLayer1);
            //DLchartLayer1.AxisY.Extent = 130
            //Dim atype As New AxisDataType
            DLchartLayer1.AxisX.DataType = AxisDataType.Time;
            DLchartLayer1.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLchartLayer1.AxisY.DataType = AxisDataType.Numeric;

            //DLchartLayer2.AxisX.DataType = AxisDataType.Time
            // DLchartLayer2.AxisX.Labels.Orientation = TextOrientation.VerticalLeftFacing
            // DLchartLayer2.AxisY.DataType = AxisDataType.Numeric
            // Add the series to the ChartLayer's Series collection.
            MyDateLinechart1.Width = 500;
            MyDateLinechart1.Height = 500;
            CompositeLegend legend1 = new CompositeLegend();
            legend1.LabelStyle.Font = new Font("Times New Roman", 10);
            MyDateLinechart1.CompositeChart.Legends.Add(legend1);
            //MyDateLinechart1.CompositeChart.ChartLayers(0).ChartLayer.LayerID = "1"
            // MyDateLinechart1.CompositeChart.ChartLayers(1).ChartLayer.LayerID = "2"
            legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers[0]);
            legend1.BoundsMeasureType = MeasureType.Percentage;
            legend1.Bounds = new Rectangle(1, 1, 10, 10);
            //Dim legend2 As New CompositeLegend()
            //legend2.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend2)
            //legend2.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(1))
            //legend2.BoundsMeasureType = MeasureType.Percentage
            //legend2.Bounds = New Rectangle(88, 1, 11, 7) 'right,top,width,height
            MyDateLinechart1.CompositeChart.ChartAreas[0].BoundsMeasureType = MeasureType.Percentage;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Bounds = new Rectangle(5, 0, 90, 100);
            Infragistics.UltraChart.Resources.Appearance.BorderAppearance MyBorder = new Infragistics.UltraChart.Resources.Appearance.BorderAppearance();
            MyBorder.Thickness = 0;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Border = MyBorder;
            //MyDateLinechart1.CompositeChart.ChartAreas(1).BoundsMeasureType = MeasureType.Percentage
            //MyDateLinechart1.CompositeChart.ChartAreas(1).Bounds = New Rectangle(0, 20, 100, 80)
            this.Charts.Controls.Add(MyDateLinechart1);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)
            if (GeneratePDF)
            {
                MemoryStream ms = new MemoryStream();
                //= New MemoryStream(myImageBytes, 0, myImageBytes.Length)
                MyDateLinechart1.SaveTo(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                System.Drawing.Image lastimage = default(System.Drawing.Image);
                lastimage = System.Drawing.Image.FromStream(ms);
                lastimage.Tag = SensorDet.Caption;
                Rep2Images.Add(lastimage, MyDateLinechart1.ID);
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
        //Protected Sub SensorMenu_MenuItemChecked(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebNavigator.WebMenuItemCheckedEventArgs) Handles SensorMenu.MenuItemChecked
        //    If e.Item.Tag = "Sensor" Then
        //        If e.Item.Checked = True Then 'clear old check
        //            'add to cameras
        //            Dim MyCollection As New Collection
        //            MyCollection = GetServerObjects(IIf(IsNothing(Session("SelectedSite")), Nothing, Session("SelectedSite")))'GetServerObjects 'server1.GetAll()
        //            Dim MyObject1 As Object
        //            Dim MyCnt As Integer = 0
        //            For Each MyObject1 In MyCollection
        //                If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
        //                    Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
        //                    If MyCnt = e.Item.Index Then
        //                        Session("Sensors") += MySensor.ID.ToString + ","
        //                        Exit For
        //                    Else
        //                        MyCnt += 1
        //                    End If
        //                End If
        //            Next
        //        Else
        //            'remove from cameras
        //            Dim MyCollection As New Collection
        //            MyCollection = GetServerObjects(IIf(IsNothing(Session("SelectedSite")), Nothing, Session("SelectedSite")))'GetServerObjects 'server1.GetAll()
        //            Dim MyObject1 As Object
        //            Dim MyCnt As Integer = 0
        //            For Each MyObject1 In MyCollection
        //                If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
        //                    Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
        //                    If MyCnt = e.Item.Index Then
        //                        Session("Sensors") = Replace(Session("Sensors"), MySensor.ID.ToString + ",", "")
        //                        Exit For
        //                    Else
        //                        MyCnt += 1
        //                    End If
        //                End If
        //            Next
        //        End If
        //        RegenerateCallbackGraphs()
        //    End If
        //End Sub 'SetAxisTypes
        protected void btnGenerate2_Click(object sender, EventArgs e)
        {
            lblErr.Visible = false;
            if (!string.IsNullOrEmpty(txtGraphs.Text))
            {
                GeneratePDF = true;
            }
            else
            {
                GeneratePDF = false;
            }
            switch (Convert.ToInt32(this.ddlRanges.SelectedValue))
            {
                case 0:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 1:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -1, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 2:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -2, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 3:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -3, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 4:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -5, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 5:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -10, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 6:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -12, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 7:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Hour, -24, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 8:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Day, -2, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 9:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Day, -4, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 10:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Day, -7, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
                case 11:
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Month, -1, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;
                    break;
            }
            RegenerateCallbackGraphs();
            try
            {
                txtStart.Text = Session["StartDate"].ToString();
                txtEnd.Text = Session["EndDate"].ToString();

            }
            catch (Exception ex)
            {
            }
            if (GeneratePDF)
            {
                LiveMonitoring.DataAccess MyFunc = new LiveMonitoring.DataAccess();
                MyFunc.SendEmail(Rep2Images, txtGraphs.Text, MyFunc.GetAppSetting("OnlineReport.0.ReportName"), ExportStr.ToString(), MyFunc.GetAppSetting("OnlineReport.0.From"), Rep2HTML.ToString());
                //AddPDF()
            }
        }
        private void AddPDF()
        {
            string path = Server.MapPath("~/Reports/");
            string Fontpath = Server.MapPath("~/Fonts/");
        }
            // Create a simple PDF file using the mjwPDF class
        //    mjwPDF objPDF = new mjwPDF();

        //    // Set the PDF title and filename
        //    objPDF.PDFTitle = "Metering Energy Report";
        //    objPDF.PDFFileName = path + "\\" + DateAndTime.Now.Year.ToString() + DateAndTime.Now.Month.ToString() + DateAndTime.Now.Day.ToString() + "AMeteringEnergy.pdf";
        //    // My.Application.Info.DirectoryPath & "\test.pdf"

        //    // We must tell the class where the PDF fonts are located
        //    objPDF.PDFLoadAfm = Fontpath;
        //    // My.Application.Info.DirectoryPath & "\Fonts"

        //    // Set the file properties
        //    objPDF.PDFSetLayoutMode = mjwPDF.PDFLayoutMd.LAYOUT_DEFAULT;
        //    objPDF.PDFFormatPage = mjwPDF.PDFFormatPgStr.FORMAT_A4;
        //    objPDF.PDFOrientation = mjwPDF.PDFOrientationStr.ORIENT_PORTRAIT;
        //    objPDF.PDFSetUnit = mjwPDF.PDFUnitStr.UNIT_PT;

        //    // Lets us set see the bookmark pane when we view the PDF
        //    objPDF.PDFUseOutlines = true;

        //    // View the PDF file after we create it
        //    //objPDF.PDFView = True

        //    // Begin our PDF document
        //    objPDF.PDFBeginDoc();
        //    // Lets add a heading
        //    objPDF.PDFSetFont(mjwPDF.PDFFontNme.FONT_ARIAL, 15, mjwPDF.PDFFontStl.FONT_BOLD);
        //    objPDF.PDFSetDrawColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        //    objPDF.PDFSetTextColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
        //    objPDF.PDFSetAlignement = mjwPDF.PDFAlignValue.ALIGN_CENTER;
        //    objPDF.PDFSetBorder = mjwPDF.PDFBorderValue.BORDER_ALL;
        //    objPDF.PDFSetFill = true;
        //    objPDF.PDFCell("Metering Energy Report      " + DateAndTime.Now.ToString(), 15, 15, objPDF.PDFGetPageWidth - 30, 40);

        //    // Lets draw a dashed red square
        //    objPDF.PDFSetLineColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        //    objPDF.PDFSetFill = true;
        //    objPDF.PDFSetLineStyle = mjwPDF.PDFStyleLgn.pPDF_DASHDOT;
        //    objPDF.PDFSetLineWidth = 1;
        //    objPDF.PDFSetDrawMode = mjwPDF.PDFDrawMd.DRAW_NORMAL;
        //    //UPGRADE_WARNING: Array has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        //    objPDF.PDFDrawPolygon(new object[] { new object[] {
        //        300,
        //        150,
        //        400,
        //        150,
        //        400,
        //        250,
        //        300,
        //        250
        //    } });

        //    // Lets draw an elipse
        //    objPDF.PDFSetDrawColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        //    objPDF.PDFSetLineColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        //    objPDF.PDFSetLineStyle = mjwPDF.PDFStyleLgn.pPDF_DASHDOT;
        //    objPDF.PDFSetLineWidth = 1.25;
        //    objPDF.PDFSetDrawMode = mjwPDF.PDFDrawMd.DRAW_DRAWBORDER;
        //    objPDF.PDFDrawEllipse(300, 150, 75, 25);

        //    AddPageNumber(ref objPDF, ref 1);

        //    //Lets add a bookmark to the start of page 1
        //    //objPDF.PDFSetBookmark("A. Page 1", 0, 0)

        //    //Now a bookmark half way down page 1
        //    // objPDF.PDFSetBookmark("A1. Page 1 Halfway down", 1, 300)

        //    //Now one at the end page 1
        //    // objPDF.PDFSetBookmark("A2. End of Page 1", 1, 500)

        //    //Another one a little further down and shows nesting
        //    // objPDF.PDFSetBookmark("A2-Sub1.", 2, 800)

        //    //objPDF.PDFEndPage()

        //    //Start page 2
        //    // objPDF.PDFNewPage()
        //    int MyCnt = 1;
        //    if (Rep2Images.Count > 0)
        //    {
        //        foreach (Image MyImage in Rep2Images)
        //        {
        //            //Dim MyFileName As String = "Image" + Now.Year.ToString + Now.Month.ToString + Now.Day.ToString + MyCnt.ToString + ".jpg"
        //            //lastimage.Tag()
        //            //ImageFiles.Add(MyFileName)
        //            //MyImage.Save(Server.MapPath("~/ReportImages/") + MyImage.Tag + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)

        //            //Lets add an image to page 2
        //            objPDF.PDFImageMS(MyImage, 15, 15 * MyCnt, 50, 50, "http://www.livemonitoring.co.za");
        //        }
        //    }

        //    //Lets add a bookmark to the start of page 2
        //    // objPDF.PDFSetBookmark("Page 2", 0, 0)

        //    //Now a bookmark just below the logo
        //    // objPDF.PDFSetBookmark("Page 2 just below logo", 1, 75)

        //    // AddPageNumber(objPDF, 2)

        //    // End our PDF document (this will save it to the filename)
        //    objPDF.PDFEndDoc();
        //}

        //// Adds the page number to the current page
        ////private void AddPageNumber(ref mjwPDF objPDF, ref short pageNumber)
        ////{
        ////    string sPageInfo = null;
        ////    double fontSize = 0;
        ////    //UPGRADE_NOTE: margin was upgraded to margin_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        ////    double margin_Renamed = 0;

        ////    fontSize = 10;
        ////    //Size of font to use
        ////    margin_Renamed = 40;
        ////    //Size of margin (left, right, bottom)

        ////    // Set what we want to print for page info
        ////    sPageInfo = "Page " + pageNumber;

        ////    // Should save these settings and change them back for more robust code
        ////    objPDF.PDFSetTextColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
        ////    objPDF.PDFSetAlignement = mjwPDF.PDFAlignValue.ALIGN_RIGHT;
        ////    objPDF.PDFSetFont(mjwPDF.PDFFontNme.FONT_ARIAL, Convert.ToInt16(fontSize), mjwPDF.PDFFontStl.FONT_NORMAL);
        ////    objPDF.PDFSetFill = false;

        ////    // Uncomment the below line if you want to see how our formating works
        ////    //objPDF.PDFSetBorder = BORDER_ALL

        //    // Draw the page number at the bottom of the page to the right
        //    objPDF.PDFCell(sPageInfo, margin_Renamed, objPDF.PDFGetPageHeight - margin_Renamed - fontSize, objPDF.PDFGetPageWidth - (margin_Renamed * 2), fontSize);
        //}
        protected void btnGenerate1_Click1(object sender, EventArgs e)
        {
            lblErr.Visible = false;
            if (!string.IsNullOrEmpty(txtGraphs.Text))
            {
                GeneratePDF = true;
            }
            else
            {
                GeneratePDF = false;
            }
            if (string.IsNullOrEmpty(txtStart.Text) | string.IsNullOrEmpty(txtEnd.Text))
            {
                lblErr.Visible = true;
                lblErr.Text = "Error please select start/end date!";
                return;
            }
            Session["StartDate"] = txtStart.Text;
            Session["EndDate"]= txtEnd.Text;
            RegenerateCallbackGraphs();
            if (GeneratePDF)
            {
                LiveMonitoring.DataAccess MyFunc = new LiveMonitoring.DataAccess();
               MyFunc.SendEmail(Rep2Images, txtGraphs.Text, MyFunc.GetAppSetting("OnlineReport.0.ReportName"), ExportStr.ToString(), MyFunc.GetAppSetting("OnlineReport.0.From"), Rep2HTML.ToString());
                // AddPDF()
            }
        }
        public void FillSessionSensors()
        {
            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
            Collection MyCollection = new Collection();
            //clear session var
            Session["Sensors"] = "";
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            Session["Sensors"] += ddlMeters.SelectedValue;
            //For Each Mynode As TreeNode In tvSensors.Nodes
            //    For Each subnode As TreeNode In Mynode.ChildNodes
            //        If subnode.Checked Then
            //            Session("Sensors") += subnode.Value.ToString + ","
            //        End If
            //    Next
            //Next
            //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session("SelectedSite")), Nothing, Session("SelectedSite")))'GetServerObjects 'server1.GetAll()
            //For Each MyItem In chkSensors.Items
            //    If MyItem.Selected = True Then 'clear old check
            //        'add to cameras
            //        Dim MyObject1 As Object
            //        Dim MyCnt As Integer = 0
            //        For Each MyObject1 In MyCollection
            //            If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //                Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            //                If MySensor.ID = MyItem.Value Then
            //                    Session("Sensors") += MySensor.ID.ToString + ","
            //                    Exit For
            //                Else
            //                    MyCnt += 1
            //                End If
            //            End If
            //        Next
            //    End If
            //Next

        }
        public void FillOldSessionSensors()
        {
            //Dim MyItem As New Web.UI.WebControls.ListItem()
            //Dim MyCollection As New Collection
            //'clear session var
            //Session("Sensors") = ""
            //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
            //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session("SelectedSite")), Nothing, Session("SelectedSite")))'GetServerObjects 'server1.GetAll()
            //For Each MyItem In chkSensors.Items
            //    If MyItem.Selected = True Then 'clear old check
            //        'add to cameras
            //        Dim MyObject1 As Object
            //        Dim MyCnt As Integer = 0
            //        For Each MyObject1 In MyCollection
            //            If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //                Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            //                If MySensor.ID = MyItem.Value Then
            //                    Session("Sensors") += MySensor.ID.ToString + ","
            //                    Exit For
            //                Else
            //                    MyCnt += 1
            //                End If
            //            End If
            //        Next
            //    End If
            //Next

        }
        public void RegenerateCallbackGraphs()
        {
            this.Charts.Controls.Clear();
            FillSessionSensors();
            string[] mySensors = Strings.Split(Session["Sensors"].ToString(), ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //If WebPanel1.Expanded Then
            //    WebPanel1.Expanded = False
            //End If
            //- 1 'last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors); Acnt++)
            {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID)
                        {
                            AddLayer(MySensor);
                            AddPageBreak();
                            AddTOUReport(MySensor);
                            break; // TODO: might not be correct. Was : Exit For

                        }
                        else
                        {
                            MyCnt += 1;
                        }
                    }
                }
                //AddImages(Acnt + 1, CInt(myCameras(Acnt)), 200, 200);
            }
        }
        public void MeteringTOUTotalReport()
        {
            Load += Page_Load;
        }



        //protected void btnGenerate1_Click1(object sender, EventArgs e)
        //{

        //}

        //protected void btnGenerate2_Click(object sender, EventArgs e)
        //{

        //}

        protected void BtnClear_Click(object sender, EventArgs e)
        {

        }

        //protected void ddlTarrif_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        //protected void cmbCurrentSite_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}
    }
}