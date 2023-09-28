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

using System.Text;
using System.Web.UI.WebControls;

namespace website2016V2
{

    partial class Reportz : System.Web.UI.Page
    {
        private string ExcelStrData = "";
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

            foreach (object MyObject1_loopVariable in MyCollection) {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    //Dim MyItem As New Web.UI.WebControls.ListItem()
                    //MyItem.Text = MySensor.Caption
                    //MyItem.Value = MySensor.ID
                    //If (SelectedID = 0 Or SelectedID = MySensor.ID) And IsPostBack = True Then
                    //    MyItem.Selected = True
                    //Else
                    //    If chkSensors.Items.Count = 0 Then
                    //        MyItem.Selected = True
                    //    Else
                    //        MyItem.Selected = False
                    //    End If
                    //End If
                    //'MyItem.Tag = "Sensor"
                    //chkSensors.Items.Add(MyItem)
                    TreeNode node = FindNode(MySensor.SensGroup.SensorGroupName);
                    if ((node == null)) {
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
                    subnode.ShowCheckBox = true;
                    subnode.Text = MySensor.Caption;
                    subnode.Value = MySensor.ID.ToString();
                    node.ChildNodes.Add(subnode);
                }
            }
        }
        private TreeNode FindNode(string nodeName)
        {
            try {
                for (int mycnt = 0; mycnt <= tvSensors.Nodes.Count - 1; mycnt++) {
                    if (tvSensors.Nodes[mycnt].Text == nodeName) {
                        return tvSensors.Nodes[mycnt];
                    }
                }
                return null;
            } catch (Exception ex) {
                return null;
            }
        }

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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == true)
                {
                    //Response.Write("viewstate size:" + CStr(Request("__VIEWSTATE").Length.ToString()()))
                }
                else
                {
                    //Response.Expires = 5
                    Page.MaintainScrollPositionOnPostBack = true;
                    Session["RepStartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateAndTime.Now);
                    Session["RepEndDate"] = DateAndTime.Now;

                    int MySensorNum = 0;
                    MySensorNum = Convert.ToInt32(Request.QueryString["SensorNum"]);
                    Load_Sensors(MySensorNum);
                    Session["Sensors"] = "";
                    if (MySensorNum == 0)
                    {
                        //all cameras
                        Collection MyCollection = new Collection();
                        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                        MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                        //GetServerObjects 'server1.GetAll()
                        object MyObject1 = null;
                        int MyDiv = 1;
                        bool added = false;
                        foreach (object MyObject1_loopVariable in MyCollection)
                        {
                            MyObject1 = MyObject1_loopVariable;
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                //only add 1st one
                                if (added == false)
                                {
                                    AddLayer((LiveMonitoring.IRemoteLib.SensorDetails)MyObject1);
                                    added = true;
                                    Session["Sensors"] += MyObject1.ToString() + ",";
                                }
                            }
                        }
                    }
                    else
                    {
                        //specific
                        Collection MyCollection = new Collection();
                        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
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
                                if (MySensorNum == MySensor.ID)
                                {
                                    Session["Sensors"] += MySensor.ID.ToString() + ",";
                                    AddLayer(MySensor);
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
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void AddPowerSupplyLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            Collection MyData = null;
            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true) {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyData = MyRem.LiveMonServer.GetSensorHistory(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));

                if ((MyData == null) == false) {
                    StringBuilder MyHtmlStr = new StringBuilder();
                    MyHtmlStr.Append("<table border=1><tr><td>");
                    MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
                    ExcelStrData += "Sensor:" + SensorDet.Caption + Constants.vbCrLf;
                    MyHtmlStr.Append("</td></table>");
                    MyHtmlStr.Append("<table border=1><tr><td>Total Time Recorded</td><td>Power ON seconds</td><td>Power Off seconds</td><tr>");

                    //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
                    int MaxFieldCnt = 0;
                    int OldField = -1;
                    int UptimeCnt = 0;
                    int DowntimeCnt = 0;
                    bool Firstrec = true;
                    DateTime StartDate = default(DateTime);

                    foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                        switch (SensorDet.Type) {

                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.RockwellPM1000DemandResults:
                                if (MyDataHistory.Field == 3) {
                                    if (MyDataHistory.Value <= 0) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCurrentValues:
                                //rms volt 1
                                if (MyDataHistory.Field == 17) {
                                    if (MyDataHistory.Value <= 10) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCurrentValues:
                                //rms volt 1
                                if (MyDataHistory.Field == 4) {
                                    if (MyDataHistory.Value <= 10) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM20TCP:
                                //Mainss volt 1
                                if (MyDataHistory.Field == 1) {
                                    if (MyDataHistory.Value <= 10) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM60TCP:
                                //Mainss volt 1
                                if (MyDataHistory.Field == 1) {
                                    if (MyDataHistory.Value <= 10) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.Megatec3Phase:
                                //Mainss volt 1
                                if (MyDataHistory.Field == 12) {
                                    if (MyDataHistory.Value <= 10) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.PowerWareSNMP:
                                //In volt 1
                                if (MyDataHistory.Field == 17) {
                                    if (MyDataHistory.Value <= 10) {
                                        DowntimeCnt += 1;
                                    } else {
                                        UptimeCnt += 1;
                                    }
                                }
                                break;
                            default:
                                //use date difference
                                if (Firstrec) {
                                    StartDate = MyDataHistory.DT;
                                    Firstrec = false;
                                } else {
                                    if (DateAndTime.DateDiff(DateInterval.Second, StartDate, MyDataHistory.DT) <= ((SensorDet.ScanRate / 1000) * 1.1)) {
                                        UptimeCnt += 1;
                                    } else {
                                        DowntimeCnt += 1;
                                    }
                                    StartDate = MyDataHistory.DT;
                                }
                                break;
                        }
                    }
                    MyHtmlStr.Append("<tr><td>");
                    MyHtmlStr.Append(((UptimeCnt + DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00")));
                    ExcelStrData += ",Total time:" + ((UptimeCnt + DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00") + Constants.vbCrLf);
                    MyHtmlStr.Append("</td><td>");
                    MyHtmlStr.Append(((UptimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00")));
                    ExcelStrData += ",Power On:" + ((UptimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00") + Constants.vbCrLf);
                    MyHtmlStr.Append("</td><td>");
                    MyHtmlStr.Append(((DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00")));
                    ExcelStrData += ",Power Off:" + ((DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00") + Constants.vbCrLf);
                    MyHtmlStr.Append("</td><td>");
                    MyHtmlStr.Append("</tr></table>");

                    this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
                }
            }

        }

        public void AddUptimeLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            Collection MyData = null;
            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true) {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyData = MyRem.LiveMonServer.GetSensorHistory(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));

                if ((MyData == null) == false) {
                    StringBuilder MyHtmlStr = new StringBuilder();
                    MyHtmlStr.Append("<table border=1><tr><td>");
                    MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
                    ExcelStrData += "Sensor:" + SensorDet.Caption + Constants.vbCrLf;
                    MyHtmlStr.Append("</td></table>");
                    MyHtmlStr.Append("<table border=1><tr><td>Total Time seconds</td><td>Uptime seconds</td><td>Downtime seconds</td><tr>");

                    //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
                    int MaxFieldCnt = 0;
                    int OldField = -1;
                    int UptimeCnt = 0;
                    int DowntimeCnt = 0;
                    bool Firstrec = true;
                    DateTime StartDate = default(DateTime);

                    foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                        switch (SensorDet.Type) {

                            case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                                if (MyDataHistory.Value >= 99999) {
                                    DowntimeCnt += 1;
                                } else {
                                    UptimeCnt += 1;
                                }
                                break;
                            default:
                                if (Firstrec) {
                                    StartDate = MyDataHistory.DT;
                                    Firstrec = false;
                                } else {
                                    if (DateAndTime.DateDiff(DateInterval.Second, StartDate, MyDataHistory.DT) <= ((SensorDet.ScanRate / 1000) * 1.1)) {
                                        UptimeCnt += 1;
                                    } else {
                                        DowntimeCnt += 1;
                                    }
                                    StartDate = MyDataHistory.DT;
                                }
                                break;
                        }

                    }


                    MyHtmlStr.Append("<tr><td>");
                    MyHtmlStr.Append(((UptimeCnt + DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00")));
                    ExcelStrData += ",Total time:" + ((UptimeCnt + DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00") + Constants.vbCrLf);
                    MyHtmlStr.Append("</td><td>");
                    MyHtmlStr.Append(((UptimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00")));
                    ExcelStrData += ",Uptime:" + ((UptimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00") + Constants.vbCrLf);
                    MyHtmlStr.Append("</td><td>");
                    MyHtmlStr.Append(((DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00")));
                    ExcelStrData += ",Downtime:" + ((DowntimeCnt) * (SensorDet.ScanRate / 1000)).ToString(("#.00") + Constants.vbCrLf);
                    MyHtmlStr.Append("</td><td>");
                    MyHtmlStr.Append("</tr></table>");

                    this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
                }
            }

        }
        public void AddLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection
            Collection MyData = null;
            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true) {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyData = MyRem.LiveMonServer.GetSensorHistory(SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                if ((MyData == null) == false) {
                    switch (SensorDet.Type) {
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:
                            DrawAMFGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BaseAudio:
                            break;
                        //TODO:Audio Graphs
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraAudio:
                            break;
                        //TODO:Audio Graphs
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDInput:
                            DrawDryContactOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                            break;
                        //output
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                            DrawDryContactOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                            break;
                        //output
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAInput:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                            break;
                        //output
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                            DrawDryContactOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                            DrawDiscreteOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                            break;
                        //output
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReader247DB:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawBiometricOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReaderSagem:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawBiometricOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BiometricReaderZKSoft:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawBiometricOnOffGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecUPS:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawMegatecUPSGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ShutUPS:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawShutUPSGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ServermonAgentSensor:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ServerLogSensor:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawServerLogGraphs(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SMTPCheckSensor:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.POPCheckSensor:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.WMIGeneralSensor:
                            //MyBiochart = New Infragistics.WebUI.UltraWebChart.UltraChart
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.DeepSeaGensetMonitor:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCUMULATIVEREGISTERS:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCurrentValues:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterTOURegister:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCUMULATIVEREGISTERS:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCurrentValues:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterTOURegister:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetRGAM20TCP:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoGensetTCP:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG210:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG300:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG700:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LovatoDMG800:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.Megatec3Phase:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecSNMP:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.YasKawaA1000:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.YasKawaV1000:
                            DrawSimpleLineGraph(SensorDet, MyData);
                            break;
                        default:
                            try {
                                DrawSimpleLineGraph(SensorDet, MyData);

                            } catch (Exception ex) {
                            }
                            warningMessage.Visible = true;
                            lblWarning.Text = "Report unknown sensor.";

                            Trace.Write("Report unknown sensor" + SensorDet.Type.ToString());
                            break;
                    }
                    if (SensorDet.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput) {
                        DrawSimpleLineGraph(SensorDet, MyData);
                    }
                    DrawHtmlLine();

                }
            }



        }
        public void AddMaxMinAvgLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection
            //MyCollection.Add(MyRetVal(0), "Max")
            //MyCollection.Add(MyMaxDate, "MaxDate")
            //MyCollection.Add(MyRetVal(1), "Min")
            //MyCollection.Add(MyMinDate, "MinDate")
            //MyCollection.Add(MyRetVal(2), "Avg")
            //MyCollection.Add(MyRetVal(3), "CheckValue")

            Collection MyData = null;
            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true) {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1><tr><td>");
                MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
                ExcelStrData += "Sensor:" + SensorDet.Caption + Constants.vbCrLf;
                MyHtmlStr.Append("</td></table>");
                MyHtmlStr.Append("<table border=1>");
                ExcelStrData += "Field,Max Value,Max Date,Min Value,Min Date,Avg Value" + Constants.vbCrLf;

                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in SensorDet.Fields) {
                    MyData = MyRem.LiveMonServer.GetSensorMaxMinAvgHistory(SensorDet.ID, MyFields.FieldNumber, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
                    if ((MyData == null) == false) {
                        if (Convert.ToDouble(MyData["CheckValue"]) == 99) {
                            MyHtmlStr.Append("<tr><td>Field</td><td>Max Value</td><td>Max Date</td><td>Min Value</td><td>Min Date</td><td>Avg Value</td></tr>");
                            MyHtmlStr.Append("<tr><td>" + MyFields.FieldName + "</td><td>" + Convert.ToDouble(MyData["Max"]).ToString() + "</td><td>" + Convert.ToString((MyData["MaxDate"]) + "</td><td>" + Convert.ToDouble(MyData["Min"]).ToString() + "</td><td>" + Convert.ToString((MyData["MinDate"]) + "</td><td>" + Convert.ToDouble(MyData["Avg"]).ToString() + "</td></tr>")));
                            ExcelStrData += MyFields.FieldName + "," + Convert.ToDouble(MyData["Max"]).ToString() + "," + Convert.ToString((MyData["MaxDate"]) + "," + Convert.ToDouble(MyData["Min"]).ToString() + "," + Convert.ToString((MyData["MinDate"]) + "," + Convert.ToDouble(MyData["Avg"]).ToString() + Constants.vbCrLf));
                        }
                    }
                }
                MyHtmlStr.Append("</table>");
                this.ReportzSection.InnerHtml += MyHtmlStr.ToString();

            }



        }
        public void AddMaxMinAvgTrendTextLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection
            //MyCollection.Add(MyRetVal(0), "Max")
            //MyCollection.Add(MyMaxDate, "MaxDate")
            //MyCollection.Add(MyRetVal(1), "Min")
            //MyCollection.Add(MyMinDate, "MinDate")
            //MyCollection.Add(MyRetVal(2), "Avg")
            //MyCollection.Add(MyRetVal(3), "CheckValue")

            Collection MyData = null;
            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true) {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1><tr><td>");
                MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
                ExcelStrData += "Sensor:" + SensorDet.Caption + Constants.vbCrLf;
                MyHtmlStr.Append("</td></table>");
                MyHtmlStr.Append("<table border=1>");

                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in SensorDet.Fields) {
                    System.DateTime StartDate = Convert.ToDateTime(Session["StartDate"]);
                    System.DateTime EndDate = Convert.ToDateTime(Session["EndDate"]);
                    int Periods = 1;
                    int AddOn = 0;
                    //select type of date
                    switch (ddlDailySetting.SelectedValue) {
                        case "0":
                            //Daily 6-6
                            StartDate = Convert.ToDateTime(DateAndTime.DateValue(StartDate.ToString()));
                            EndDate = Convert.ToDateTime(DateAndTime.DateValue(EndDate.ToString()));
                            AddOn = 24;
                            Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                            break;
                        case "1":
                            //Daily 12-12
                            StartDate = Convert.ToDateTime(DateAndTime.DateValue(StartDate.ToString()));
                            EndDate = Convert.ToDateTime(DateAndTime.DateValue(EndDate.ToString()));
                            AddOn = 24;
                            Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                            break;
                        case "2":
                            //12 Hourly
                            AddOn = 12;
                            Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                            break;
                        case "3":
                            //Hourly
                            AddOn = 1;
                            Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                            break;
                        case "4":
                            //Weekly
                            AddOn = 168;
                            Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                            break;
                    }
                    if (Periods < 1) {
                        Periods = 1;
                    }
                    int MyPeriodCnt = 0;
                    ExcelStrData += "Field,Max Value,Max Date,Min Value,Min Date,Avg Value" + Constants.vbCrLf;

                    for (MyPeriodCnt = 1; MyPeriodCnt <= Periods; MyPeriodCnt++) {
                        MyData = MyRem.LiveMonServer.GetSensorMaxMinAvgHistory(SensorDet.ID, MyFields.FieldNumber, StartDate, DateAndTime.DateAdd(DateInterval.Hour, AddOn, StartDate));
                        StartDate = DateAndTime.DateAdd(DateInterval.Hour, AddOn, StartDate);
                        if ((MyData == null) == false) {
                            if (Convert.ToDateTime(MyData["MaxDate"]) != Convert.ToDateTime("1/1/1990")) {
                                if (Convert.ToDouble(MyData["CheckValue"]) == 99) {
                                    MyHtmlStr.Append("<tr><td>Field</td><td>Max Value</td><td>Max Date</td><td>Min Value</td><td>Min Date</td><td>Avg Value</td></tr>");
                                    MyHtmlStr.Append("<tr><td>" + MyFields.FieldName + "</td><td>" + Convert.ToDouble(MyData["Max"]).ToString() + "</td><td>" + Convert.ToString((MyData["MaxDate"]) + "</td><td>" + Convert.ToDouble(MyData["Min"]).ToString() + "</td><td>" + Convert.ToString((MyData["MinDate"]) + "</td><td>" + Convert.ToDouble(MyData["Avg"]).ToString() + "</td></tr>")));
                                    ExcelStrData += MyFields.FieldName + "," + Convert.ToDouble(MyData["Max"]).ToString() + "," + Convert.ToString(MyData["MaxDate"]) + "," + Convert.ToDouble(MyData["Min"]).ToString() + "," + Convert.ToString(MyData["MinDate"]) + "," + Convert.ToDouble(MyData["Avg"]).ToString() + Constants.vbCrLf;
                                }
                            }
                        }

                    }
                }
                MyHtmlStr.Append("</table>");
                this.ReportzSection.InnerHtml += MyHtmlStr.ToString();

            }



        }
        public void AddMaxMinAvgTrendGraphLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection
            //MyCollection.Add(MyRetVal(0), "Max")
            //MyCollection.Add(MyMaxDate, "MaxDate")
            //MyCollection.Add(MyRetVal(1), "Min")
            //MyCollection.Add(MyMinDate, "MinDate")
            //MyCollection.Add(MyRetVal(2), "Avg")
            //MyCollection.Add(MyRetVal(3), "CheckValue")

            Collection MyData = null;
            if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true) {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = new LiveMonitoring.IRemoteLib.SensorFieldsDef();
                // Dim MyHtmlStr As New StringBuilder
                //MyHtmlStr.Append("<table border=1><tr><td>")
                // MyHtmlStr.Append("Sensor:" & SensorDet.Caption)
                // MyHtmlStr.Append("</td></table>")
                // MyHtmlStr.Append("<table border=1>")

                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in SensorDet.Fields) {
                    if ((SensorDet.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.MegatecUPS & (MyFields.FieldNumber == 7 | MyFields.FieldNumber == 8))) {
                    } else {
                        System.DateTime StartDate = Convert.ToDateTime(Session["StartDate"]);
                        System.DateTime EndDate = Convert.ToDateTime(Session["EndDate"]);
                        int Periods = 1;
                        int AddOn = 0;
                        Collection AllData = new Collection();
                        //select type of date
                        switch (ddlDailySetting.SelectedValue) {
                            case "0":
                                //Daily 6-6
                                StartDate = Convert.ToDateTime(DateAndTime.DateValue(StartDate.ToString()));
                                EndDate = Convert.ToDateTime(DateAndTime.DateValue(EndDate.ToString()));
                                AddOn = 24;
                                Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                                break;
                            case "1":
                                //Daily 12-12
                                StartDate = Convert.ToDateTime(DateAndTime.DateValue(StartDate.ToString()));
                                EndDate = Convert.ToDateTime(DateAndTime.DateValue(EndDate.ToString()));
                                AddOn = 24;
                                Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                                break;
                            case "2":
                                //12 Hourly
                                AddOn = 12;
                                Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                                break;
                            case "3":
                                //Hourly
                                AddOn = 1;
                                Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                                break;
                            case "4":
                                //Weekly
                                AddOn = 168;
                                Periods = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Hour, StartDate, EndDate) / AddOn);
                                break;
                        }
                        if (Periods < 1) {
                            Periods = 1;
                        }
                        int MyPeriodCnt = 0;

                        for (MyPeriodCnt = 1; MyPeriodCnt <= Periods; MyPeriodCnt++) {
                            MyData = MyRem.LiveMonServer.GetSensorMaxMinAvgHistory(SensorDet.ID, MyFields.FieldNumber, StartDate, DateAndTime.DateAdd(DateInterval.Hour, AddOn, StartDate));

                            if ((MyData == null) == false) {
                                if (Convert.ToDateTime(MyData["MaxDate"]) != Convert.ToDateTime("1/1/1990")) {
                                    MyData.Add(StartDate, "StartDate");
                                    AllData.Add(MyData, MyPeriodCnt.ToString());
                                }
                                //If CDbl(MyData("CheckValue")) = 99 Then
                                //    MyHtmlStr.Append("<tr><td>Field</td><td>Max Value</td><td>Max Date</td><td>Min Value</td><td>Min Date</td><td>Avg Value</td></tr>")
                                //    MyHtmlStr.Append("<tr><td>" + MyFields.FieldName + "</td><td>" + CDbl(MyData("Max")).ToString() + "</td><td>" + CStr(MyData("MaxDate")) + "</td><td>" + CDbl(MyData("Min")).ToString() + "</td><td>" + CStr(MyData("MinDate")) + "</td><td>" + CDbl(MyData("Avg")).ToString() + "</td></tr>")
                                //End If
                            }
                            StartDate = DateAndTime.DateAdd(DateInterval.Hour, AddOn, StartDate);

                        }
                        try {
                            DrawMaxMinAvgGraph(SensorDet.Caption, MyFields.FieldName, AllData, MyFields.FieldNumber, SensorDet.ID);
                        } catch (Exception ex) {
                            errorMessage.Visible = true;
                            lblError.Text = "MinMaxAvg graph trend err:" + ex.Message;

                            Trace.Write("MinMaxAvg graph trend err:" + ex.Message);
                        }
                        //create graph
                    }


                }
                //MyHtmlStr.Append("</table>")
                //Me.ReportzSection.InnerHtml += MyHtmlStr.ToString()

            }

        }
        private void ultraChart1_ChartDrawItem(object sender, ChartDrawItemEventArgs e)
        {
            try {
                if ((e.Primitive == null) == false) {
                    if ((e.Primitive.Path == null) == false) {
                        if (e.Primitive.Path.IndexOf("Legend") != -1) {
                            if (e.Primitive.Path.IndexOf("Border") == -1) {
                                e.Primitive.PE.StrokeWidth = 4;
                            }
                        }
                    }
                }

            } catch (Exception ex) {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;

                Trace.Write("err" + ex.Message);
            }

        }

        public void DrawMaxMinAvgGraph(string SensorTitle, string FieldTitle, Collection graphData, int FieldNumber, int SensorNumber)
        {
            try {
                Infragistics.WebUI.UltraWebChart.UltraChart SensorChart = default(Infragistics.WebUI.UltraWebChart.UltraChart);

                //roger Newline
                SensorChart = new Infragistics.WebUI.UltraWebChart.UltraChart();
                //attach event for legent width
                SensorChart.ChartDrawItem += ultraChart1_ChartDrawItem;
                SensorChart.ID = "Sensor" + SensorNumber.ToString() + "Field" + FieldNumber.ToString();
                //The following appleis to all titles, titletop is simply used for the example
                //sets the height or width of space for the title
                SensorChart.TitleTop.Extent = 45;
                //sets the font color
                SensorChart.TitleTop.FontColor = Color.Red;
                //sets whether the chart auto-sizes the font for the title
                SensorChart.TitleTop.FontSizeBestFit = true;
                //sets the horizontal alignment of the text
                SensorChart.TitleTop.HorizontalAlign = StringAlignment.Center;
                //sets the margins for the Top, Bottom, Left and right
                SensorChart.TitleTop.Margins.Bottom = 2;
                SensorChart.TitleTop.Margins.Top = 2;
                SensorChart.TitleTop.Margins.Left = 2;
                SensorChart.TitleTop.Margins.Right = 2;
                //sets the text to display for the chart in the title
                SensorChart.TitleTop.Text = "Min Max Avg Sensor:" + SensorTitle + " Field:" + FieldTitle;
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
                // Create Series
                ISeries ctDLseries = null;
                NumericTimeSeries[] ctnumericTimeSeries = new NumericTimeSeries[16];
                ctnumericTimeSeries[0] = new NumericTimeSeries();
                ctnumericTimeSeries[0].Label = "Max";
                ctnumericTimeSeries[1] = new NumericTimeSeries();
                ctnumericTimeSeries[1].Label = "Min";
                ctnumericTimeSeries[2] = new NumericTimeSeries();
                ctnumericTimeSeries[2].Label = "Avg";
                int tmp1cntwe = 0;
                //mycnt1= fields ?
                ExcelStrData += "Field,Max Value,Max Date,Min Value,Min Date,Avg Value" + Constants.vbCrLf;

                Collection MyDataHistory = null;
                int MaxFieldCnt = 2;
                foreach (Collection MyDataHistory_loopVariable in graphData) {
                    MyDataHistory = MyDataHistory_loopVariable;
                    int MyField = 0;
                    //= MyDataHistory.Field - 1 'field always starts at 1
                    //If MyField > MaxFieldCnt Then
                    //    MaxFieldCnt = MyField
                    //End If
                    int myret = 0;
                    ExcelStrData += (FieldTitle + "," + Convert.ToDouble(MyDataHistory["Max"]).ToString() + "," + Convert.ToString(MyDataHistory["MaxDate"]) + "," + Convert.ToDouble(MyDataHistory["Min"]).ToString() + "," + Convert.ToString(MyDataHistory["MinDate"]) + "," + Convert.ToDouble(MyDataHistory["Avg"]).ToString() + Constants.vbCrLf);

                    MyField = 0;
                    //max
                    myret = ctnumericTimeSeries[MyField].Points.Add(new NumericTimeDataPoint());
                    ctnumericTimeSeries[MyField].Points[myret].TimeValue = Convert.ToDateTime(MyDataHistory["MaxDate"]);
                    ctnumericTimeSeries[MyField].Points[myret].NumericValue = Convert.ToDouble(MyDataHistory["Max"]);
                    MyField = 1;
                    //min
                    myret = ctnumericTimeSeries[MyField].Points.Add(new NumericTimeDataPoint());
                    ctnumericTimeSeries[MyField].Points[myret].TimeValue = Convert.ToDateTime(MyDataHistory["MinDate"]);
                    ctnumericTimeSeries[MyField].Points[myret].NumericValue = Convert.ToDouble(MyDataHistory["Min"]);
                    MyField = 2;
                    //min
                    myret = ctnumericTimeSeries[MyField].Points.Add(new NumericTimeDataPoint());
                    ctnumericTimeSeries[MyField].Points[myret].TimeValue = Convert.ToDateTime(MyDataHistory["StartDate"]);
                    ctnumericTimeSeries[MyField].Points[myret].NumericValue = Convert.ToDouble(MyDataHistory["Avg"]);
                }
                int Acnt = 0;
                for (Acnt = 0; Acnt <= MaxFieldCnt; Acnt++) {
                    CTDLchartLayer.Series.Add(ctnumericTimeSeries[Acnt]);
                    ctDLseries = ctnumericTimeSeries[Acnt];
                    SensorChart.Series.Add(ctDLseries);
                }
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

                this.ReportzSection.Controls.Add(SensorChart);
                //Me.Charts.Controls.Add(SensorChart)

            } catch (Exception ex) {
                errorMessage.Visible = true;
                lblError.Text = "Draw MaxMinAvgGraph err:" + ex.Message;

                Trace.Write("Draw MaxMinAvgGraph err:" + ex.Message);
            }

        }
        private void SetAxisTypes(ChartLayerAppearance layer)
        {
            switch (layer.ChartType) {
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

        public void DrawHtmlLine()
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<br/><hr/><br/>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
        }
        public int SelectAmfFields(int mycnt, ref Color[] DrawColor)
        {
            int functionReturnValue = 0;
            //analouge
            switch (mycnt) {
                case 1:
                    functionReturnValue = 33;
                    DrawColor[mycnt - 1] = Color.Green;
                    break;
                case 2:
                    DrawColor[mycnt - 1] = Color.Green;
                    functionReturnValue = 32;
                    break;
                case 3:
                    functionReturnValue = 67;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 4:
                    functionReturnValue = 68;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 5:
                    functionReturnValue = 69;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 6:
                    functionReturnValue = 18;
                    DrawColor[mycnt - 1] = Color.Green;
                    break;
                case 7:
                    functionReturnValue = 16;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 8:
                    functionReturnValue = 23;
                    DrawColor[mycnt - 1] = Color.Green;
                    break;
                case 9:
                    functionReturnValue = 31;
                    DrawColor[mycnt - 1] = Color.Green;
                    break;
                case 10:
                    functionReturnValue = 30;
                    DrawColor[mycnt - 1] = Color.Green;
                    break;
                case 11:
                    functionReturnValue = 38;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 12:
                    functionReturnValue = 47;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 13:
                    functionReturnValue = 46;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 14:
                    functionReturnValue = 65;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 15:
                    functionReturnValue = 66;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 16:
                    functionReturnValue = 45;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 17:
                    functionReturnValue = 39;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 18:
                    functionReturnValue = 42;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 19:
                    functionReturnValue = 40;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 20:
                    functionReturnValue = 51;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 21:
                    functionReturnValue = 41;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 22:
                    functionReturnValue = 48;
                    DrawColor[mycnt - 1] = Color.Red;
                    break;
                case 23:
                    functionReturnValue = 49;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 24:
                    functionReturnValue = 53;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 25:
                    functionReturnValue = 52;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 26:
                    functionReturnValue = 50;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
                case 27:
                    functionReturnValue = 19;
                    DrawColor[mycnt - 1] = Color.Green;
                    break;
                default:
                    functionReturnValue = 2;
                    DrawColor[mycnt - 1] = Color.Yellow;
                    break;
            }
            return functionReturnValue;
        }
        public string FindAmfFields(int mycnt)
        {
            //analouge
            if (mycnt == 3)
                return "Mains Phase 1 Volts";
            if (mycnt == 4)
                return "Mains Phase 2 Volts";
            if (mycnt == 5)
                return "Mains Phase 3 Volts";
            if (mycnt == 6)
                return "Alternator Phase 1 Volts";
            if (mycnt == 7)
                return "Alternator Phase 2 Volts";
            if (mycnt == 8)
                return "Alternator Phase 3 Volts";
            if (mycnt == 9)
                return "DC input volts";
            if (mycnt == 10)
                return "Speed input in RPM";
            if (mycnt == 11)
                return "Phase Rotation Input";
            //input status
            if (mycnt == 12)
                return "Alternator Charge Input";
            //13
            if (mycnt == 13)
                return "No Fuel Input";
            if (mycnt == 14)
                return "Spare 2 Input";
            if (mycnt == 15)
                return "Test Switch Input";
            if (mycnt == 16)
                return "Load Input";
            if (mycnt == 17)
                return "Auto/Manual Input";
            if (mycnt == 18)
                return "Alarm Mute Input";
            if (mycnt == 19)
                return "Lamp Test Switch Input";
            if (mycnt == 20)
                return "Heater Fault Input";
            if (mycnt == 21)
                return "Low Fuel Input";
            if (mycnt == 22)
                return "Remote Start Input";
            if (mycnt == 23)
                return "Spare 1 Input";
            if (mycnt == 24)
                return "Auxiliary Shutdown Input";
            if (mycnt == 25)
                return "Low Oil Pressure Input";
            if (mycnt == 26)
                return "High Temperature Input";
            //Output status LEDS
            if (mycnt == 27)
                return "Alternator On Load";
            //30
            if (mycnt == 28)
                return "Alternator Available";
            if (mycnt == 29)
                return "Mains On Load";
            if (mycnt == 30)
                return "Mains Available";
            if (mycnt == 31)
                return "Keyboard Scan Line 3";
            if (mycnt == 32)
                return "Keyboard Scan Line 2";
            if (mycnt == 33)
                return "Keyboard Scan Line 1";
            if (mycnt == 34)
                return "Keyboard Scan Line 0";
            if (mycnt == 35)
                return "Fail to Start";
            if (mycnt == 36)
                return "High temperature";
            if (mycnt == 37)
                return "Low Water";
            //spare 1 
            if (mycnt == 38)
                return "Overspeed";
            if (mycnt == 39)
                return "Low Oil Pressure";
            if (mycnt == 40)
                return "Auxilary Shutdown";
            if (mycnt == 41)
                return "Not Auto";
            if (mycnt == 42)
                return "emergency Stop";
            if (mycnt == 43)
                return "Low Alternator Supply";
            if (mycnt == 44)
                return "High Alternator Supply";
            if (mycnt == 45)
                return "Underspeed";
            if (mycnt == 46)
                return "Mains Charge Failed";
            if (mycnt == 47)
                return "Spare 2";
            if (mycnt == 48)
                return "No Fuel";
            if (mycnt == 49)
                return "Heater Fault";
            if (mycnt == 50)
                return "Low Fuel ";
            if (mycnt == 51)
                return "LED Port 1/2";
            if (mycnt == 52)
                return "Auxiliary Start";
            if (mycnt == 53)
                return "Run Relay";
            if (mycnt == 54)
                return "Start Relay";
            if (mycnt == 55)
                return "Pre Heat Relay";
            if (mycnt == 56)
                return "Alarm Relay";
            if (mycnt == 57)
                return "Mains On Load Relay";
            if (mycnt == 58)
                return "Alternator on Load Relay";
            if (mycnt == 59)
                return "I2C TX";
            if (mycnt == 60)
                return "I2C Clock Out";
            if (mycnt == 61)
                return "Remote Start";
            if (mycnt == 62)
                return "Alternator Phase Fault";
            if (mycnt == 63)
                return "Alternator Charging";
            if (mycnt == 64)
                return "High Mains Supply";
            if (mycnt == 65)
                return "Low Mains Supply";
            if (mycnt == 66)
                return "Mains Phase Fault";
            return "Unk";
        }
        public void DrawSimpleLineGraph(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
            ExcelStrData += "Sensor:" + SensorDet.Caption + Constants.vbCrLf;
            MyHtmlStr.Append("</td></table>");
            MyHtmlStr.Append("<table border=1><tr><td>");

            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            int OldField = -1;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyField > MaxFieldCnt) {
                    MaxFieldCnt = MyField;
                }
                //headings
                if (OldField != MyField) {
                    if (OldField == -1) {
                        MyHtmlStr.Append("<tr>");
                    } else {
                        MyHtmlStr.Append("</tr><tr>");
                    }
                    MyHtmlStr.Append("<td>");
                    if (SensorDet.Fields.Count >= MyField + 1) {
                        MyHtmlStr.Append(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 1).ToString()]).FieldName + "</td>");
                        ExcelStrData += ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 1).ToString()]).FieldName;
                    } else {
                        MyHtmlStr.Append("Unk</td>");
                        ExcelStrData += "Unk";
                    }
                }
                MyHtmlStr.Append("<td>Date:" + MyDataHistory.DT.ToString() + "</td>");
                MyHtmlStr.Append("<td>V:" + MyDataHistory.Value.ToString() + "</td>");
                ExcelStrData += ",Date:" + MyDataHistory.DT.ToString() + "," + MyDataHistory.Value.ToString() + Constants.vbCrLf;

            }
            MyHtmlStr.Append("</tr></table>");

            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
        }
        public void DrawAMFGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            DrawAMFGraphs1(SensorDet, MyData);
            DrawAMFGraphs2(SensorDet, MyData);
            DrawAMFGraphs3(SensorDet, MyData);

        }
        public void DrawAMFGraphs1(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("AMF Inputs:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");
            MyHtmlStr.Append("<table border=1>");

            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];

            for (tmpcntwe1 = 1; tmpcntwe1 <= 27; tmpcntwe1++) {
                tmpcntwe = SelectAmfFields(tmpcntwe1, ref ChartColors);
                int Namesint = 0;
                if (tmpcntwe >= 13 & tmpcntwe <= 27) {
                    Namesint = tmpcntwe - 1;
                }
                if (tmpcntwe >= 30 & tmpcntwe <= 69) {
                    Namesint = tmpcntwe - 3;
                }
                if (tmpcntwe1 == 1) {
                    MyHtmlStr.Append("<tr><td>" + FindAmfFields(Namesint) + "</td>");
                } else {
                    MyHtmlStr.Append("</tr><tr><td>" + FindAmfFields(Namesint) + "</td>");
                }
                string[] Dates = GenerateAMFStartEndData(tmpcntwe, MyData);
                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1) {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++) {
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt) {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt])) {
                                endtime = EndArray[loopcnt];
                            } else {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime)) {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        } else {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt])) {
                            MyHtmlStr.Append("<td>Start:" + StartArray[loopcnt] + "</td><td>End:" + endtime + "</td>");
                        }
                    }
                }
            }
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();


        }
        public void DrawServerLogGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Server Log:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");

            MyHtmlStr.Append("<table border=1><tr><td>");

            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            int OldField = -1;
            MyHtmlStr.Append(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(1).ToString()]).FieldName + "</td>");
            MyHtmlStr.Append("</td><td></td><td></td></tr>");
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                //If MyDataHistory.Field > 2 And MyDataHistory.Field <= 8 Then
                int MyField = MyDataHistory.Field;
                //- 3 'field always starts at 1
                MyHtmlStr.Append("<tr>");
                MyHtmlStr.Append("<td>Date:" + MyDataHistory.DT.ToString() + "</td>");
                MyHtmlStr.Append("<td>Val:" + MyDataHistory.Value.ToString() + "</td>");
                MyHtmlStr.Append("<td>Data:" + MyDataHistory.OtherData + "</td>");
                MyHtmlStr.Append("</tr>");
                //End If
            }
            MyHtmlStr.Append("</table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
        }

        public void DrawAMFGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("AMF Mains & Alternator:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");

            MyHtmlStr.Append("<table border=1><tr><td>");

            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            int OldField = -1;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                if (MyDataHistory.Field > 2 & MyDataHistory.Field <= 8) {
                    int MyField = MyDataHistory.Field - 3;
                    //field always starts at 1
                    if (MyField > MaxFieldCnt) {
                        MaxFieldCnt = MyField;
                    }
                    //headings
                    if (OldField != MyField) {
                        if (OldField == -1) {
                            MyHtmlStr.Append("<tr>");
                        } else {
                            MyHtmlStr.Append("</tr><tr>");
                        }
                        MyHtmlStr.Append("<td>");
                        MyHtmlStr.Append(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 3).ToString()]).FieldName + "</td>");
                    }
                    MyHtmlStr.Append("<td>Date:" + MyDataHistory.DT.ToString() + "</td>");
                    MyHtmlStr.Append("<td>V:" + MyDataHistory.Value.ToString() + "</td>");
                }
            }
            MyHtmlStr.Append("</tr></table>");

            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
        }
        public void DrawAMFGraphs3(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("AMF DC & Speed:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");

            MyHtmlStr.Append("<table border=1><tr><td>");

            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            int OldField = -1;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                if (MyDataHistory.Field >= 9 & MyDataHistory.Field <= 11) {
                    int MyField = MyDataHistory.Field - 9;
                    //field always starts at 1
                    if (MyField > MaxFieldCnt) {
                        MaxFieldCnt = MyField;
                    }
                    //headings
                    if (OldField != MyField) {
                        if (OldField == -1) {
                            MyHtmlStr.Append("<tr>");
                        } else {
                            MyHtmlStr.Append("</tr><tr>");
                        }
                        MyHtmlStr.Append("<td>");
                        MyHtmlStr.Append(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 9).ToString()]).FieldName + "</td>");
                    }
                    MyHtmlStr.Append("<td>Date:" + MyDataHistory.DT.ToString() + "</td>");
                    MyHtmlStr.Append("<td>V:" + MyDataHistory.Value.ToString() + "</td>");
                }
            }
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
        }
        public void DrawMegatecUPSGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            DrawMegatecUPSGraphs2(SensorDet, MyData);
            DrawMegatecUPSGraphs1(SensorDet, MyData);
        }
        public void DrawMegatecUPSGraphs1(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("UPS STATUS:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");
            MyHtmlStr.Append("<table border=1>");

            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];
            string NameField = null;
            for (tmpcntwe1 = 1; tmpcntwe1 <= 8; tmpcntwe1++) {
                switch (tmpcntwe1) {
                    case 8:
                        NameField = "Utility Fail";
                        break;
                    case 7:
                        NameField = "Batery low";
                        break;
                    case 6:
                        NameField = "Boost";
                        break;
                    case 5:
                        NameField = "Failed";
                        break;
                    case 4:
                        NameField = "Standby";
                        break;
                    case 3:
                        NameField = "Testing";
                        break;
                    case 2:
                        NameField = "Shutdown";
                        break;
                    case 1:
                        NameField = "Beeper On";
                        break;
                }
                string[] Dates = GenerateMegatecUPSGantStartEndData(8, tmpcntwe1, MyData);
                if (!string.IsNullOrEmpty(NameField)) {
                    MyHtmlStr.Append("<tr><td>" + NameField + "</td>");
                } else {
                    MyHtmlStr.Append("</tr><tr><td>" + NameField + "</td>");
                }

                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1) {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++) {
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt) {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt])) {
                                endtime = EndArray[loopcnt];
                            } else {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime)) {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        } else {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt])) {
                            MyHtmlStr.Append("<td>Start:" + StartArray[loopcnt] + "</td><td>End:" + endtime + "</td>");
                        }
                    }
                }
            }
            //roger data end
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();



        }
        public void DrawMegatecUPSGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></table>");
            MyHtmlStr.Append("<table border=1><tr><td>");


            int Bcnt = 0;
            int OldField = -1;

            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                if (MyDataHistory.Field <= 7) {
                    int MyField = MyDataHistory.Field - 1;
                    //- 3 'field always starts at 1
                    if (MyField > MaxFieldCnt) {
                        MaxFieldCnt = MyField;
                    }
                    if (MyField == 0) {
                        MyHtmlStr.Append("<tr>");
                    } else {
                        MyHtmlStr.Append("</tr><tr>");
                    }
                    MyHtmlStr.Append("<td>");
                    if (!string.IsNullOrEmpty(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 1).ToString()]).FieldName)) {
                        MyHtmlStr.Append(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 1).ToString()]).FieldName + "</td>");
                    } else {
                        MyHtmlStr.Append("Unk</td>");
                    }
                    MyHtmlStr.Append("<td>Date:" + MyDataHistory.DT.ToString() + "</td>");
                    MyHtmlStr.Append("<td>V:" + MyDataHistory.Value.ToString() + "</td>");
                }
            }
            MyHtmlStr.Append("</tr></table>");

            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();


        }
        public string[] GenerateMegatecUPSGantStartEndData(int FieldNum, int BitNo, Collection MyData)
        {
            ///'''''''''''''''''''''''''''''''''''''''''
            //field num 12-27 = Field1
            //Field num 28>=Field 2
            int ValueField = FieldNum;
            int mytmpcnt = BitNo - 1;
            string[] Dates = new string[2];
            //0=Start 1=End seperate with ,
            int tmp1cntwe = 0;
            var AMFoutputStateSwitch = false;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyDataHistory.Field == ValueField) {
                    long Pattern = 1;
                    long myrestmp = 0;
                    //= 1 << mytmpcnt
                    myrestmp = Pattern << mytmpcnt;
                    //this bit is off so on disply
                    if (((long)MyDataHistory.Value & myrestmp) >= 1) {
                        //changed add startdate
                        if (AMFoutputStateSwitch == false) {
                            Dates[0] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = true;
                        }
                    } else {
                        //changed add enddate
                        if (AMFoutputStateSwitch == true) {
                            Dates[1] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = false;
                        }
                    }
                }
            }
            return Dates;
            ///'''''''''''''''''''''''''''''''''''''''''
        }
        public void DrawShutUPSGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            DrawShutUPSGraphs2(SensorDet, MyData);
            DrawShutUPSGraphs1(SensorDet, MyData);
        }
        public void DrawShutUPSGraphs1(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("UPS STATUS:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");
            MyHtmlStr.Append("<table border=1>");

            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];
            string NameField = null;
            for (tmpcntwe1 = 1; tmpcntwe1 <= 8; tmpcntwe1++) {
                switch (tmpcntwe1) {
                    case 8:
                        NameField = "Power Overload";
                        break;
                    case 7:
                        NameField = "UPS Shutdown";
                        break;
                    case 6:
                        NameField = "Output Status";
                        break;
                    case 5:
                        NameField = "Battery Need Replacing";
                        break;
                    case 4:
                        NameField = "Bellow Remain Capacity";
                        break;
                    case 3:
                        NameField = "Discharging Battery";
                        break;
                    case 2:
                        NameField = "Charging Battery";
                        break;
                    case 1:
                        NameField = "Main AC Status";
                        break;
                }
                string[] Dates = GenerateMegatecUPSGantStartEndData(6, tmpcntwe1, MyData);
                if (!string.IsNullOrEmpty(NameField)) {
                    MyHtmlStr.Append("<tr><td>" + NameField + "</td>");
                } else {
                    MyHtmlStr.Append("</tr><tr><td>" + NameField + "</td>");
                }
                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1) {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++) {
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt) {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt])) {
                                endtime = EndArray[loopcnt];
                            } else {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime)) {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        } else {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt])) {
                            MyHtmlStr.Append("<td>Start:" + StartArray[loopcnt] + "</td><td>End:" + endtime + "</td>");
                        }
                    }
                }
            }
            //roger data end
            //roger data end
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();


        }
        public void DrawShutUPSGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Sensor:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></table>");
            MyHtmlStr.Append("<table border=1><tr><td>");


            int Bcnt = 0;
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                if (MyDataHistory.Field <= 8) {
                    int MyField = MyDataHistory.Field - 1;
                    //- 3 'field always starts at 1
                    if (MyField > MaxFieldCnt) {
                        MaxFieldCnt = MyField;
                    }
                    if (MyField == 0) {
                        MyHtmlStr.Append("<tr>");
                    } else {
                        MyHtmlStr.Append("</tr><tr>");
                    }
                    MyHtmlStr.Append("<td>");
                    if (!string.IsNullOrEmpty(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 1).ToString()]).FieldName)) {
                        MyHtmlStr.Append(((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(MyField + 1).ToString()]).FieldName + "</td>");
                    } else {
                        MyHtmlStr.Append("Unk</td>");
                    }
                    MyHtmlStr.Append("<td>Date:" + MyDataHistory.DT.ToString() + "</td>");
                    MyHtmlStr.Append("<td>V:" + MyDataHistory.Value.ToString() + "</td>");
                }
            }
            MyHtmlStr.Append("</tr></table>");

            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();


        }
        public void DrawDiscreteOnOffGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Discrete On/Off:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");
            MyHtmlStr.Append("<table border=1>");

            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];

            for (tmpcntwe1 = 1; tmpcntwe1 <= SensorDet.Fields.Count; tmpcntwe1++) {
                tmpcntwe = tmpcntwe1;
                ChartColors[tmpcntwe1 - 1] = Color.Green;
                if (tmpcntwe1 == 1) {
                    MyHtmlStr.Append("<tr><td>" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName + "</td>");
                } else {
                    MyHtmlStr.Append("</tr><tr><td>" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName + "</td>");
                }
                string[] Dates = GenerateDiscreteONOffStartEndData(tmpcntwe, MyData);
                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1) {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++) {
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt) {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt])) {
                                endtime = EndArray[loopcnt];
                            } else {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime)) {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        } else {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt])) {
                            MyHtmlStr.Append("<td>Start:" + StartArray[loopcnt] + "</td><td>End:" + endtime + "</td>");
                        }
                    }
                }
            }
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();

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
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyDataHistory.Field == tmpcntwe) {
                    long Pattern = 1;
                    long myrestmp = 0;
                    //= 1 << mytmpcnt
                    myrestmp = Pattern << mytmpcnt;
                    //this bit is off so on disply
                    if (((long)MyDataHistory.Value & myrestmp) == 0) {
                        //changed add startdate
                        if (AMFoutputStateSwitch == false) {
                            Dates[0] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = true;
                        }
                    } else {
                        //changed add enddate
                        if (AMFoutputStateSwitch == true) {
                            Dates[1] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = false;
                        }
                    }
                }
            }
            return Dates;
        }
        public void DrawDryContactOnOffGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Drycontact On/Off:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");
            MyHtmlStr.Append("<table border=1>");

            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];

            for (tmpcntwe1 = 1; tmpcntwe1 <= SensorDet.Fields.Count; tmpcntwe1++) {
                tmpcntwe = tmpcntwe1;
                ChartColors[tmpcntwe1 - 1] = Color.Green;
                if (tmpcntwe1 == 1) {
                    MyHtmlStr.Append("<tr><td>" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName + "</td>");
                } else {
                    MyHtmlStr.Append("</tr><tr><td>" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName + "</td>");
                }
                string[] Dates = GenerateDryContactONOffStartEndData(tmpcntwe, MyData);
                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1) {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++) {
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt) {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt])) {
                                endtime = EndArray[loopcnt];
                            } else {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime)) {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        } else {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt])) {
                            MyHtmlStr.Append("<td>Start:" + StartArray[loopcnt] + "</td><td>End:" + endtime + "</td>");
                        }
                    }
                }
            }
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();
        }
        public string[] GenerateDryContactONOffStartEndData(int tmpcntwe, Collection MyData)
        {
            string[] Dates = new string[2];
            //0=Start 1=End seperate with ,
            int tmp1cntwe = 0;
            var AMFoutputStateSwitch = false;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyDataHistory.Field == tmpcntwe) {
                    //this bit is off so on disply
                    if (MyDataHistory.Value == 1) {
                        //changed add startdate
                        if (AMFoutputStateSwitch == false) {
                            Dates[0] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = true;
                        }
                    } else {
                        //changed add enddate
                        if (AMFoutputStateSwitch == true) {
                            Dates[1] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = false;
                        }
                    }
                }
            }
            return Dates;
        }
        public string[] GenerateAMFStartEndData(int FieldNum, Collection MyData)
        {
            ///'''''''''''''''''''''''''''''''''''''''''
            //field num 12-27 = Field1
            //Field num 28>=Field 2
            int ValueField = 0;
            int mytmpcnt = 0;
            if (FieldNum >= 12 & FieldNum <= 27) {
                ValueField = 1;
                mytmpcnt = FieldNum - 12;
            } else {
                ValueField = 2;
                mytmpcnt = FieldNum - 28;
            }
            string[] Dates = new string[2];
            //0=Start 1=End seperate with ,
            int tmp1cntwe = 0;
            var AMFoutputStateSwitch = false;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyDataHistory.Field == ValueField) {
                    long Pattern = 1;
                    long myrestmp = 0;
                    //= 1 << mytmpcnt
                    myrestmp = Pattern << mytmpcnt;
                    //this bit is off so on disply
                    if (((long)MyDataHistory.Value & myrestmp) == 0) {
                        //changed add startdate
                        if (AMFoutputStateSwitch == false) {
                            Dates[0] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = true;
                        }
                    } else {
                        //changed add enddate
                        if (AMFoutputStateSwitch == true) {
                            Dates[1] += Convert.ToString(MyDataHistory.DT) + ",";
                            AMFoutputStateSwitch = false;
                        }
                    }
                }
            }
            return Dates;
            ///'''''''''''''''''''''''''''''''''''''''''
        }
        public string[] GenerateBiometricONOffStartEndData(int tmpcntwe, Collection MyData)
        {
            int mytmpcnt = 0;
            string[] Dates = new string[3];
            //0=Start 1=End seperate with ,
            int tmp1cntwe = 0;
            var AMFoutputStateSwitch = false;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData) {
                int MyField = MyDataHistory.Field - 1;
                //field always starts at 1
                if (MyDataHistory.Field == tmpcntwe) {
                    Dates[0] += Convert.ToString(MyDataHistory.DT) + ",";
                    Dates[1] += Convert.ToString(DateAndTime.DateAdd(DateInterval.Minute, 1, MyDataHistory.DT)) + ",";
                    Dates[2] += Convert.ToString(MyDataHistory.OtherData) + ",";
                    //Dim Pattern As Long = 1
                    //Dim myrestmp As Long '= 1 << mytmpcnt
                    //myrestmp = Pattern << mytmpcnt
                    //If (MyDataHistory.Value And myrestmp) = 0 Then 'this bit is off so on disply
                    //    If AMFoutputStateSwitch = False Then 'changed add startdate
                    //        Dates(0) += CStr(MyDataHistory.DT) & ","
                    //        AMFoutputStateSwitch = True
                    //    End If
                    //Else
                    //    If AMFoutputStateSwitch = True Then  'changed add enddate
                    //        Dates(1) += CStr(MyDataHistory.DT) & ","
                    //        AMFoutputStateSwitch = False
                    //    End If
                    //End If
                }
            }
            return Dates;
        }

        public void DrawBiometricOnOffGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Bio Metric Access:" + SensorDet.Caption);
            MyHtmlStr.Append("</td></tr></table>");

            MyHtmlStr.Append("<table border=1><tr><td>");



            //roger data
            int tmpcntwe = 0;
            int tmpcntwe1 = 0;
            Color[] ChartColors = new Color[41];

            for (tmpcntwe1 = 1; tmpcntwe1 <= SensorDet.Fields.Count; tmpcntwe1++) {
                tmpcntwe = tmpcntwe1;
                if (tmpcntwe1 == 1) {
                    MyHtmlStr.Append("<tr><td>" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName + "</td>");
                } else {
                    MyHtmlStr.Append("</tr><tr><td>" + ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[tmpcntwe]).FieldName + "</td>");
                }
                MyHtmlStr.Append("</tr><tr><td>Date</td><td>Data</td></tr>");

                string[] Dates = GenerateBiometricONOffStartEndData(tmpcntwe, MyData);
                int LastEnd = 0;
                string[] StartArray = Strings.Split(Dates[0], ",");
                string[] EndArray = Strings.Split(Dates[1], ",");
                string[] DataArray = Strings.Split(Dates[2], ",");
                int loopcnt = 0;
                if (Information.UBound(StartArray) > -1) {
                    for (loopcnt = 0; loopcnt <= Information.UBound(StartArray); loopcnt++) {
                        MyHtmlStr.Append("<tr>");
                        string endtime = null;
                        if (Information.UBound(EndArray) >= loopcnt) {
                            if (!string.IsNullOrEmpty(EndArray[loopcnt])) {
                                endtime = EndArray[loopcnt];
                            } else {
                                endtime = this.txtEndDate.Text;
                                if (string.IsNullOrEmpty(endtime)) {
                                    endtime = DateAndTime.Now.ToString();
                                }
                            }
                        } else {
                            endtime = this.txtEndDate.Text;
                        }
                        if (!string.IsNullOrEmpty(StartArray[loopcnt])) {
                            MyHtmlStr.Append("<td>Access:" + StartArray[loopcnt] + "</td><td>Data:" + DataArray[loopcnt] + "</td>");
                        }
                        MyHtmlStr.Append("</tr>");
                    }
                }
            }
            MyHtmlStr.Append("</tr></table>");
            this.ReportzSection.InnerHtml += MyHtmlStr.ToString();


        }
        public void FillSessionSensors()
        {
            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
            Collection MyCollection = new Collection();
            //clear session var
            Session["Sensors"] = "";
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            foreach (TreeNode Mynode in tvSensors.Nodes) {
                foreach (TreeNode subnode in Mynode.ChildNodes) {
                    if (subnode.Checked) {
                        Session["Sensors"] += subnode.Value.ToString() + ",";
                    }
                }
            }
            //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session["SelectedSite"]), Nothing, Session["SelectedSite"]))'GetServerObjects 'server1.GetAll()
            //For Each MyItem In chkSensors.Items
            //    If MyItem.Selected = True Then 'clear old check
            //        'add to cameras
            //        Dim MyObject1 As Object
            //        Dim MyCnt As Integer = 0
            //        For Each MyObject1 In MyCollection
            //            If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //                Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            //                If MySensor.ID = MyItem.Value Then
            //                    Session["Sensors"] += MySensor.ID.ToString() + ","
            //                    Exit For
            //                Else
            //                    MyCnt += 1
            //                End If
            //            End If
            //        Next
            //    End If
            //Next

        }
        public void FilloldSessionSensors()
        {
            //Dim MyItem As New Web.UI.WebControls.ListItem()
            //Dim MyCollection As New Collection
            //'clear session var
            //Session["Sensors"] = ""
            //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
            //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session["SelectedSite"]), Nothing, Session["SelectedSite"]))'GetServerObjects 'server1.GetAll()
            //For Each MyItem In chkSensors.Items
            //    If MyItem.Selected = True Then 'clear old check
            //        'add to cameras
            //        Dim MyObject1 As Object
            //        Dim MyCnt As Integer = 0
            //        For Each MyObject1 In MyCollection
            //            If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //                Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            //                If MySensor.ID = MyItem.Value Then
            //                    Session["Sensors"] += MySensor.ID.ToString() + ","
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
            this.ReportzSection.InnerHtml = "";
            FillSessionSensors();
            string[] mySensors = Strings.Split((string)Session["Sensors"], ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++) {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection) {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID) {
                            AddLayer(MySensor);
                            break; // TODO: might not be correct. Was : Exit For
                        } else {
                            MyCnt += 1;
                        }
                    }
                }
            }
        }

        public void RegeneratePowerSupplyGraphs()
        {
            this.ReportzSection.InnerHtml = "";
            FillSessionSensors();
            string[] mySensors = Strings.Split((string)Session["Sensors"], ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++) {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection) {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID) {
                            AddPowerSupplyLayer(MySensor);
                            break; // TODO: might not be correct. Was : Exit For
                        } else {
                            MyCnt += 1;
                        }
                    }
                }
            }
        }
        public void RegenerateUptimeGraphs()
        {
            this.ReportzSection.InnerHtml = "";
            FillSessionSensors();
            string[] mySensors = Strings.Split((string)Session["Sensors"], ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++) {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection) {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID) {
                            AddUptimeLayer(MySensor);
                            break; // TODO: might not be correct. Was : Exit For
                        } else {
                            MyCnt += 1;
                        }
                    }
                }
            }
        }
        public void RegenerateMaxMinAvg()
        {
            this.ReportzSection.InnerHtml = "";
            FillSessionSensors();
            string[] mySensors = Strings.Split((string)Session["Sensors"], ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++) {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection) {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID) {
                            AddMaxMinAvgLayer(MySensor);
                            break; // TODO: might not be correct. Was : Exit For
                        } else {
                            MyCnt += 1;
                        }
                    }
                }
            }
        }
        public void RegenerateMaxMinAvgTrendText()
        {
            this.ReportzSection.InnerHtml = "";
            FillSessionSensors();
            string[] mySensors = Strings.Split((string)Session["Sensors"], ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++) {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection) {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID) {
                            AddMaxMinAvgTrendTextLayer(MySensor);
                            break; // TODO: might not be correct. Was : Exit For
                        } else {
                            MyCnt += 1;
                        }
                    }
                }
            }
        }
        public void RegenerateMaxMinAvgTrendGraph()
        {
            this.ReportzSection.InnerHtml = "";
            FillSessionSensors();
            string[] mySensors = Strings.Split((string)Session["Sensors"], ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++) {
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                foreach (object MyObject1_loopVariable in MyCollection) {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails) {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID) {
                            AddMaxMinAvgTrendGraphLayer(MySensor);
                            break; // TODO: might not be correct. Was : Exit For
                        } else {
                            MyCnt += 1;
                        }
                    }
                }
            }
        }
        protected void btnGenerate_Click(object sender, System.EventArgs e)
        {
            switch (ddlReportType.SelectedValue) {
                case "0":

                    if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text)) {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select start/end date.";

                        return;
                    }

                    Session["StartDate"] = txtStartDate.Text;
                    Session["EndDate"] = txtEndDate.Text;
                    RegenerateCallbackGraphs();
                    break;
                case "1":
                    if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text)) {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select start/end date.";
                        return;
                    }
                    lblError.Visible = false;
                    Session["StartDate"] = txtStartDate.Text;
                    Session["EndDate"] = txtEndDate.Text;
                    RegenerateMaxMinAvg();
                    break;
                case "2":
                    if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text)) {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select start/end date.";
                        return;
                    }
                    lblError.Visible = false;
                    Session["StartDate"] = txtStartDate.Text;
                    Session["EndDate"] = txtEndDate.Text;
                    RegenerateMaxMinAvgTrendText();
                    break;
                case "3":
                    if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text)) {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select start/end date.";
                        return;
                    }
                    lblError.Visible = false;
                    Session["StartDate"] = txtStartDate.Text;
                    Session["EndDate"] = txtEndDate.Text;
                    RegenerateMaxMinAvgTrendGraph();
                    break;
                case "4":
                    if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text)) {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select start/end date.";
                        return;
                    }
                    Session["StartDate"] = txtStartDate.Text;
                    Session["EndDate"] = txtEndDate.Text;
                    RegenerateUptimeGraphs();
                    break;
                case "5":
                    if (string.IsNullOrEmpty(txtStartDate.Text) | string.IsNullOrEmpty(txtEndDate.Text)) {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Please select start/end date.";
                        return;
                    }
                    Session["StartDate"] = txtStartDate.Text;
                    Session["EndDate"] = txtEndDate.Text;
                    RegeneratePowerSupplyGraphs();
                    break;
            }
            if (chkRawData.Checked) {
                //insert seperator character
                ExcelStrData = "sep=," + Constants.vbCrLf + ExcelStrData;
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(ExcelStrData);
                Response.Clear();
                Response.AddHeader("Content-Type", "application/Excel");
                Response.AddHeader("Content-Disposition", "inline;filename=IPMonExp_" + DateAndTime.Now.Day.ToString() + DateAndTime.Now.Month.ToString() + DateAndTime.Now.Year.ToString() + ".csv");
                Response.BinaryWrite(data);
                Response.End();
            }

        }

        protected void ddlReportType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlReportType.SelectedIndex >= 2) {
                ddlDailySetting.Visible = true;
            } else {
                ddlDailySetting.Visible = false;
            }
        }
        public Reportz()
        {
            Load += Page_Load;
        }
    }
 }
