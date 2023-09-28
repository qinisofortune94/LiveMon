using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Data.Series;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Events;
using Infragistics.UltraChart.Shared.Styles;
using LiveMonitoring;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class CumulativeDisplay : System.Web.UI.Page
    {
        private Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart1 = new Infragistics.WebUI.UltraWebChart.UltraChart();
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart2 = new Infragistics.WebUI.UltraWebChart.UltraChart();
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
                if (IsPostBack == true)
                {
                }
                else
                {
                    try
                    {
                        if (Convert.ToBoolean(MyDataAccess.GetAppSetting("BySite")) == true)
                        {
                            if (MyUser.UserSites.Count > 1)
                            {
                                ddlCurrentSite.Visible = true;
                                ddlCurrentSite.BorderColor = System.Drawing.Color.SeaGreen;
                            }
                            ddlCurrentSite.Items.Clear();
                            Sites RetSites = new Sites(MyUser.ID);

                            List<MySite> MySitesList = new List<MySite>();
                            bool firstitem = true;
                            foreach (Sites.Site MySiteID in RetSites.SitesList)
                            {
                                try
                                {
                                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                    MyItem.Text = MySiteID.SiteObj.SiteName;
                                    //MySiteID.SiteName
                                    MyItem.Value = MySiteID.SiteObj.SiteID.ToString();

                                    if ((Session["SelectedSite"] == null) == false)
                                    {
                                        if (Convert.ToInt32(Session["SelectedSite"]) == MySiteID.SiteObj.SiteID)
                                        {
                                            MyItem.Selected = true;
                                        }
                                    }
                                    else
                                    {
                                        if (firstitem)
                                        {
                                            Session["SelectedSite"] = MySiteID.SiteObj.SiteID;
                                            MyItem.Selected = true;
                                            firstitem = false;
                                        }
                                    }
                                    ddlCurrentSite.Items.Add(MyItem);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            //try
                            //{
                            //   // test.SortDropDown(ddlCurrentSite);
                            //}
                            //catch (Exception ex)
                            //{
                            //}
                        }
                    }
                    finally
                    {

                    }
                }
                Response.Expires = 5;
                Page.MaintainScrollPositionOnPostBack = true;
                //Session["StartDate"] = DateTimeOffset.Now;
                Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateTime.Now);
                Session["EndDate"] = DateTime.Now;

                int MySensorNum = 0;
                MySensorNum = Convert.ToInt32(Request.QueryString["SensorNum"]);
                Load_Sensors(MySensorNum);
                //Load_Tarrifs();
                Session["Sensors"] = "";
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

                if (MySensorNum == 0)
                {

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
        public void AddPageBreak()
        {
            HtmlGenericControl MyHtml = new HtmlGenericControl();
            MyHtml.InnerHtml = "<div style=\"height:1px\">&nbsp;</div><div style=\"page-break-before: always; height:1px;\">&nbsp;</div>";



            // MyHtml.InnerHtml = "<tr style=""page-break-before: always;"">"
            this.Charts.Controls.Add(MyHtml);
        }
        public class MySite
        {
            public int siteID { get; set; }
            public string siteName { get; set; }
        }
        public void Load_Sensors(int SelectedID)
        {//
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
                    if (MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCUMULATIVEREGISTERS | MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCUMULATIVEREGISTERS)
                    {
                        Session["Sensors"] += MySensor.ID.ToString() + ",";
                        System.Web.UI.WebControls.ListItem MyIttem = new System.Web.UI.WebControls.ListItem();
                        MyIttem.Text = MySensor.Caption;
                        MyIttem.Value = MySensor.ID.ToString();
                        //AddLayer(MySensor);
                       // ddlMeter.Items.Add(MyIttem);
                        if (Firstone)
                        {
                            MyIttem.Selected = true;
                            Firstone = false;
                        }
                    }
                    //End If
                }
            }

        }

        public void DrawElsterA1140TarrifReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            try
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1><tr><td>");
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
                        //MyTarrif.ActiveEnergyCharges()
                        LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                        int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                        if ((TypePeriod == null) == false & (TmpChargePeriod == null) == false)
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
                    MyHtmlStr.Append("<table border=1 width=450 ><tr><td>");
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
                    MyHtmlStr.Append("Billing :");
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    if (PeakKWhCnt > 0)
                    {
                        MyHtmlStr.Append(PeakKWhLabel + "  " + PeakKWhCnt.ToString("#.00") + " KWh @ " + (PeakKWhCost / 100).ToString() + " R/KWh  </td><td>  R" + ((PeakKWhCost * PeakKWhCnt) / 100).ToString("#.00"));
                        PeakKWhTotCost = (PeakKWhCost * PeakKWhCnt) / 100;
                    }
                    else
                    {
                        MyHtmlStr.Append(PeakKWhLabel + "  " + PeakKWhCnt.ToString("#.00") + " KWh @ " + (PeakKWhCost / 100).ToString() + " R/KWh  </td><td>  R0");
                        PeakKWhTotCost = 0;
                        //(PeakKWhCost * PeakKWhCnt) / 100
                    }
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    if (StandKWhCnt > 0)
                    {
                        MyHtmlStr.Append(StandKWhLabel + "  " + StandKWhCnt.ToString("#.00") + " KWh @ " + (StandKWhCost / 100).ToString() + " R/KWh </td><td>   R" + ((StandKWhCost * StandKWhCnt) / 100).ToString("#.00"));
                        StandKWhTotCost = (StandKWhCost * StandKWhCnt) / 100;
                    }
                    else
                    {
                        MyHtmlStr.Append(StandKWhLabel + "  " + StandKWhCnt.ToString("#.00") + " KWh @ " + (StandKWhCost / 100).ToString() + " R/KWh  </td><td>  R0");
                        StandKWhTotCost = 0;
                    }
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    if (OffPeakKWhCnt > 0)
                    {
                        MyHtmlStr.Append(OffPeakKWhLabel + "  " + OffPeakKWhCnt.ToString("#.00") + " KWh @ " + (OffPeakKWhCost / 100).ToString() + " R/KWh  </td><td>  R" + ((OffPeakKWhCost * OffPeakKWhCnt) / 100).ToString("#.00"));
                        OffPeakKWhTotCost = (OffPeakKWhCost * OffPeakKWhCnt) / 100;
                    }
                    else
                    {
                        MyHtmlStr.Append(OffPeakKWhLabel + "  " + OffPeakKWhCnt.ToString("#.00") + " KWh @ " + (OffPeakKWhCost / 100).ToString() + " R/KWh   </td><td> R0");
                        OffPeakKWhTotCost = 0;
                    }
                    double totCharge = 0;
                    try
                    {
                        double[] ChargeVals = new double[Information.UBound(MyTarrif.MeteringNetworkCharge)];
                        LiveMonitoring.IRemoteLib.MeteringNetworkCharges MyNetCharge = default(LiveMonitoring.IRemoteLib.MeteringNetworkCharges);
                        for (int myCnt = 0; myCnt <= Information.UBound(MyTarrif.MeteringNetworkCharge) - 1; myCnt++)
                        {
                            MyNetCharge = MyTarrif.MeteringNetworkCharge[myCnt];
                            if ((MyNetCharge == null) == false)
                            {
                                if (MyNetCharge.Percentage > 0)
                                {
                                    //calculate percentage
                                    double TotColVal = 0;
                                    string Cols = "";
                                    try
                                    {
                                        foreach (int MyColumns in MyNetCharge.Columns)
                                        {
                                            TotColVal += (ChargeVals[MyColumns] * (MyNetCharge.Percentage / 100));
                                            Cols += MyColumns.ToString() + ",";
                                        }
                                        Cols = Cols.Remove(Cols.LastIndexOf(","), 1);

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    ChargeVals[myCnt] = TotColVal;
                                    MyHtmlStr.Append("</td></tr><tr><td>");
                                    MyHtmlStr.Append(MyNetCharge.ChargeName + "  (" + Cols + ")  @ " + (MyNetCharge.Percentage).ToString() + " %  </td><td>  R" + (ChargeVals[myCnt]).ToString("#.00"));

                                }
                                if (MyNetCharge.CostRperkVA > 0)
                                {
                                    //calculate CostRperkVA
                                    double TotColVal = 0;
                                    try
                                    {
                                        TotColVal = TotKvarCnt * MyNetCharge.CostRperkVA;

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    ChargeVals[myCnt] = TotColVal;
                                    MyHtmlStr.Append("</td></tr><tr><td>");
                                    MyHtmlStr.Append(MyNetCharge.ChargeName + "  " + TotKvarCnt.ToString("#.00") + "kVA  @ " + (MyNetCharge.CostRperkVA).ToString() + " R/kVA  </td><td>  R" + (ChargeVals[myCnt]).ToString("#.00"));
                                }
                                if (MyNetCharge.FixedCost > 0)
                                {
                                    //calculate FixedCost
                                    double TotColVal = MyNetCharge.FixedCost;
                                    ChargeVals[myCnt] = TotColVal;
                                    MyHtmlStr.Append("</td></tr><tr><td>");
                                    MyHtmlStr.Append(MyNetCharge.ChargeName + "  @ R" + (MyNetCharge.FixedCost).ToString() + "  </td><td>  R" + (ChargeVals[myCnt]).ToString("#.00"));
                                }
                            }
                            totCharge += ChargeVals[myCnt];
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    MyHtmlStr.Append("Subtotal  " + "  </td><td>  R" + (totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost).ToString("#.00"));
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    MyHtmlStr.Append("Vat @ 15%  " + "  </td><td>  R" + ((totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost) * 0.15).ToString("#.00"));
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    MyHtmlStr.Append("Total  " + "  </td><td>  R" + (totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost + ((totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost) * 0.14)).ToString("#.00"));

                    MyHtmlStr.Append("</td></tr></table>");

                }
                catch (Exception ex)
                {
                }


                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }


        }

        public void DrawElsterA1700TarrifReport(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData, List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers, LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif)
        {
            try
            {
                StringBuilder MyHtmlStr = new StringBuilder();
                MyHtmlStr.Append("<table border=1><tr><td>");
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
                        //MyTarrif.ActiveEnergyCharges()
                        LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                        int TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);
                        if ((TypePeriod == null) == false & (TmpChargePeriod == null) == false)
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
                    MyHtmlStr.Append("<table border=1 width=450 ><tr><td>");
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
                    MyHtmlStr.Append("Billing :");
                    MyHtmlStr.Append("</td><td></td></tr><tr><td>");
                    if (PeakKWhCnt > 0)
                    {
                        MyHtmlStr.Append(PeakKWhLabel + "  " + PeakKWhCnt.ToString("#.00") + " KWh @ " + (PeakKWhCost / 100).ToString() + " R/KWh  </td><td>  R" + ((PeakKWhCost * PeakKWhCnt) / 100).ToString("#.00"));
                        PeakKWhTotCost = (PeakKWhCost * PeakKWhCnt) / 100;
                    }
                    else
                    {
                        MyHtmlStr.Append(PeakKWhLabel + "  " + PeakKWhCnt.ToString("#.00") + " KWh @ " + (PeakKWhCost / 100).ToString() + " R/KWh  </td><td>  R0");
                        PeakKWhTotCost = 0;
                        //(PeakKWhCost * PeakKWhCnt) / 100
                    }
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    if (StandKWhCnt > 0)
                    {
                        MyHtmlStr.Append(StandKWhLabel + "  " + StandKWhCnt.ToString("#.00") + " KWh @ " + (StandKWhCost / 100).ToString() + " R/KWh </td><td>   R" + ((StandKWhCost * StandKWhCnt) / 100).ToString("#.00"));
                        StandKWhTotCost = (StandKWhCost * StandKWhCnt) / 100;
                    }
                    else
                    {
                        MyHtmlStr.Append(StandKWhLabel + "  " + StandKWhCnt.ToString("#.00") + " KWh @ " + (StandKWhCost / 100).ToString() + " R/KWh  </td><td>  R0");
                        StandKWhTotCost = 0;
                    }
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    if (OffPeakKWhCnt > 0)
                    {
                        MyHtmlStr.Append(OffPeakKWhLabel + "  " + OffPeakKWhCnt.ToString("#.00") + " KWh @ " + (OffPeakKWhCost / 100).ToString() + " R/KWh  </td><td>  R" + ((OffPeakKWhCost * OffPeakKWhCnt) / 100).ToString("#.00"));
                        OffPeakKWhTotCost = (OffPeakKWhCost * OffPeakKWhCnt) / 100;
                    }
                    else
                    {
                        MyHtmlStr.Append(OffPeakKWhLabel + "  " + OffPeakKWhCnt.ToString("#.00") + " KWh @ " + (OffPeakKWhCost / 100).ToString() + " R/KWh   </td><td> R0");
                        OffPeakKWhTotCost = 0;
                    }
                    double totCharge = 0;
                    try
                    {
                        double[] ChargeVals = new double[Information.UBound(MyTarrif.MeteringNetworkCharge)];
                        LiveMonitoring.IRemoteLib.MeteringNetworkCharges MyNetCharge = default(LiveMonitoring.IRemoteLib.MeteringNetworkCharges);
                        for (int myCnt = 0; myCnt <= Information.UBound(MyTarrif.MeteringNetworkCharge) - 1; myCnt++)
                        {
                            MyNetCharge = MyTarrif.MeteringNetworkCharge[myCnt];
                            if ((MyNetCharge == null) == false)
                            {
                                if (MyNetCharge.Percentage > 0)
                                {
                                    //calculate percentage
                                    double TotColVal = 0;
                                    string Cols = "";
                                    try
                                    {
                                        foreach (int MyColumns in MyNetCharge.Columns)
                                        {
                                            TotColVal += (ChargeVals[MyColumns] * (MyNetCharge.Percentage / 100));
                                            Cols += MyColumns.ToString() + ",";
                                        }
                                        Cols = Cols.Remove(Cols.LastIndexOf(","), 1);

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    ChargeVals[myCnt] = TotColVal;
                                    MyHtmlStr.Append("</td></tr><tr><td>");
                                    MyHtmlStr.Append(MyNetCharge.ChargeName + "  (" + Cols + ")  @ " + (MyNetCharge.Percentage).ToString() + " %  </td><td>  R" + (ChargeVals[myCnt]).ToString("#.00"));

                                }
                                if (MyNetCharge.CostRperkVA > 0)
                                {
                                    //calculate CostRperkVA
                                    double TotColVal = 0;
                                    try
                                    {
                                        TotColVal = TotKvarCnt * MyNetCharge.CostRperkVA;

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    ChargeVals[myCnt] = TotColVal;
                                    MyHtmlStr.Append("</td></tr><tr><td>");
                                    MyHtmlStr.Append(MyNetCharge.ChargeName + "  " + TotKvarCnt.ToString("#.00") + "kVA  @ " + (MyNetCharge.CostRperkVA).ToString() + " R/kVA  </td><td>  R" + (ChargeVals[myCnt]).ToString("#.00"));
                                }
                                if (MyNetCharge.FixedCost > 0)
                                {
                                    //calculate FixedCost
                                    double TotColVal = MyNetCharge.FixedCost;
                                    ChargeVals[myCnt] = TotColVal;
                                    MyHtmlStr.Append("</td></tr><tr><td>");
                                    MyHtmlStr.Append(MyNetCharge.ChargeName + "  @ R" + (MyNetCharge.FixedCost).ToString() + "  </td><td>  R" + (ChargeVals[myCnt]).ToString("#.00"));
                                }
                            }
                            totCharge += ChargeVals[myCnt];
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    MyHtmlStr.Append("Subtotal  " + "  </td><td>  R" + (totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost).ToString("#.00"));
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    MyHtmlStr.Append("Vat @ 15%  " + "  </td><td>  R" + ((totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost) * 0.15).ToString("#.00"));
                    MyHtmlStr.Append("</td></tr><tr><td>");
                    MyHtmlStr.Append("Total  " + "  </td><td>  R" + (totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost + ((totCharge + PeakKWhTotCost + StandKWhTotCost + OffPeakKWhTotCost) * 0.14)).ToString("#.00"));

                    MyHtmlStr.Append("</td></tr></table>");

                }
                catch (Exception ex)
                {
                }


                this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();
                AddPageBreak();

            }
            catch (Exception ex)
            {
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
            Collection MyData = null;
            //List(Of LiveMonitoring.IRemoteLib.MeteringProfileRecord)
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                if (Information.IsDate(Session["StartDate"]) == true & Information.IsDate(Session["EndDate"]) == true)
                {
                    //If cmbDataSet.SelectedValue = 0 Then
                    //Try
                    //    MyData = MyRem.server1.GetMeteringProfileRecord(SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
                    //Catch ex As Exception

                    //End Try
                    //Try
                    //    MyDataMarkers = MyRem.server1.GetMeteringProfileMarkers(SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
                    //Catch ex As Exception

                    //End Try

                    //Else
                    MyData = MyRem.LiveMonServer.GetFilteredSensorHistory(1, SensorDet.ID, Convert.ToDateTime(Session["StartDate"]), Convert.ToDateTime(Session["EndDate"]));
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
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCUMULATIVEREGISTERS:
                        DrawElsterA1140CUMULATIVEREGISTERSGraphs(SensorDet, MyData);
                        break;
                    case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCUMULATIVEREGISTERS:
                        DrawElsterA1700CUMULATIVEREGISTERSGraphs(SensorDet, MyData);
                        break;
                }
            }


        }

        public void DrawHtmlLine()
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<br/><hr/><br/>");
        }

        public void DrawElsterA1700CUMULATIVEREGISTERSGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            try
            {
                DrawElsterA1700CUMULATIVEREGISTERSGraphs2(SensorDet, MyData);
                //Active Power
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }


        }

        public void DrawElsterA1700CUMULATIVEREGISTERSGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            bool FirstOne = true;
            System.DateTime StartDate = default(System.DateTime);
            System.DateTime EndDate = default(System.DateTime);
            double StartVal = 0;
            double EndVal = 0;
            //  LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
            {
                int MyField = MyDataHistory.Field;
                //- 1 'field always starts at 1
                //ignore status
                if (MyField == 1)
                {
                    if (FirstOne)
                    {
                        StartDate = MyDataHistory.DT;
                        StartVal = MyDataHistory.Value;
                        FirstOne = false;
                    }
                    EndDate = MyDataHistory.DT;
                    EndVal = MyDataHistory.Value;
                    //If MyField > MaxFieldCnt Then
                    //    MaxFieldCnt = MyField
                    //End If
                    //Dim myret As Integer
                    //myret = numericTimeSeries1(MyField - 1).Points.Add(New NumericTimeDataPoint())
                    // numericTimeSeries1(MyField - 1).Points(myret).TimeValue = MyDataHistory.DT
                    //numericTimeSeries1(MyField - 1).Points(myret).NumericValue = MyDataHistory.Value
                }

            }

            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Cumulative KWh Report: " + SensorDet.Caption);
            MyHtmlStr.Append("</td><td></td></tr>");
            MyHtmlStr.Append("<tr><td>");
            MyHtmlStr.Append("StartDate:" + StartDate.ToString());
            MyHtmlStr.Append("</td><td>");
            MyHtmlStr.Append("StartValue:" + StartVal.ToString());
            MyHtmlStr.Append("</td></tr>");
            MyHtmlStr.Append("<tr><td>");
            MyHtmlStr.Append("EndDate:" + EndDate.ToString());
            MyHtmlStr.Append("</td><td>");
            MyHtmlStr.Append("EndValue:" + EndVal.ToString());
            MyHtmlStr.Append("</td></tr>");
            MyHtmlStr.Append("<tr><td>");
            MyHtmlStr.Append("Minutes Difference:" + DateAndTime.DateDiff(DateInterval.Minute, StartDate, EndDate).ToString());
            MyHtmlStr.Append("</td><td>");
            MyHtmlStr.Append("Value Difference :" + (EndVal - StartVal).ToString());
            MyHtmlStr.Append("</td></tr>");

            //MyHtmlStr.Append("Meter :" & SensorDet.Caption)
            //ExcelStrData += "Sensor:" & SensorDet.Caption + vbCrLf
            MyHtmlStr.Append("</td></tr></table>");
            this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();



        }

        public void DrawElsterA1700CUMULATIVEREGISTERSGraphs3(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart1 = new Infragistics.WebUI.UltraWebChart.UltraChart();
            //attach event for legent width
            MyDateLinechart1.ChartDrawItem += ultraChart1_ChartDrawItem;
            MyDateLinechart1.ID = "ElsterA1700Wh" + SensorDet.ID.ToString();
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
            MyDateLinechart1.TitleTop.Text = "Elster Wh :" + SensorDet.Caption;
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
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea1);
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            // Create an X axis
            AxisItem DLxAxis1 = new AxisItem();
            DLxAxis1.axisNumber = AxisNumber.X_Axis;
            DLxAxis1.DataType = AxisDataType.String;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>";
            DLxAxis1.Labels.Font = new Font("Tahoma", 7);
            DLxAxis1.LineThickness = 1;
            // Create an Y axis
            AxisItem DLyAxis1 = new AxisItem();
            DLyAxis1.axisNumber = AxisNumber.Y_Axis;
            DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis1.Labels.Font = new Font("Tahoma", 7);
            DLyAxis1.LineThickness = 1;
            myDLChartArea1.Axes.Add(DLxAxis1);
            myDLChartArea1.Axes.Add(DLyAxis1);
            // Set the axes
            DLchartLayer1.AxisX = DLxAxis1;
            DLchartLayer1.AxisY = DLyAxis1;
            // Set the ChartArea
            DLchartLayer1.ChartArea = myDLChartArea1;
            // Add the ChartLayer to the ChartLayers collection
            MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer1);
            ///''''''''''''''''''
            ISeries DLseries1 = null;
            NumericTimeSeries[] numericTimeSeries1 = new NumericTimeSeries[5];
            int Bcnt = 0;
            LiveMonitoring.IRemoteLib.LovatoGenset MyGenset = new LiveMonitoring.IRemoteLib.LovatoGenset();

            for (Bcnt = 0; Bcnt <= 1; Bcnt++)
            {
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                if (SensorDet.Fields.Contains((Bcnt + 1).ToString()) == true)
                {
                    if (((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(Bcnt + 1).ToString()]).DisplayValue)
                    {
                        numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                        numericTimeSeries1[Bcnt].Label = ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(Bcnt + 1).ToString()]).FieldName;
                    }
                }
                else
                {
                    numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                    numericTimeSeries1[Bcnt].Label = "Unknown";
                }
            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            // LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
            {
                int MyField = MyDataHistory.Field;
                //- 1 'field always starts at 1
                //ignore status
                if (MyField >= 1 & MyField <= 2)
                {
                    if (MyField > MaxFieldCnt)
                    {
                        MaxFieldCnt = MyField;
                    }
                    int myret = 0;
                    myret = numericTimeSeries1[MyField - 1].Points.Add(new NumericTimeDataPoint());
                    numericTimeSeries1[MyField - 1].Points[myret].TimeValue = MyDataHistory.DT;
                    numericTimeSeries1[MyField - 1].Points[myret].NumericValue = MyDataHistory.Value;
                }

            }
            int Acnt = 0;
            for (Acnt = 0; Acnt <= MaxFieldCnt - 1; Acnt++)
            {
                DLchartLayer1.Series.Add(numericTimeSeries1[Acnt]);
                DLseries1 = numericTimeSeries1[Acnt];
                MyDateLinechart1.Series.Add(DLseries1);
            }
            ///''''''''''''''''''
            // Set X axis
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
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
            // Add the series to the ChartLayer's Series collection.
            MyDateLinechart1.Width = 700;
            MyDateLinechart1.Height = 500;
            CompositeLegend legend1 = new CompositeLegend();
            legend1.LabelStyle.Font = new Font("Times New Roman", 10);
            MyDateLinechart1.CompositeChart.Legends.Add(legend1);
            legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers[0]);
            legend1.BoundsMeasureType = MeasureType.Percentage;
            legend1.Bounds = new Rectangle(5, 5, 105, 14);
            MyDateLinechart1.CompositeChart.ChartAreas[0].BoundsMeasureType = MeasureType.Percentage;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Bounds = new Rectangle(0, 20, 100, 80);
            this.Charts.Controls.Add(MyDateLinechart1);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)

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
        public void DrawElsterA1140CUMULATIVEREGISTERSGraphs(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            try
            {
                DrawElsterA1140CUMULATIVEREGISTERSGraphs2(SensorDet, MyData);
                //Active Power
                AddPageBreak();

            }
            catch (Exception ex)
            {
            }
        }

        public void DrawElsterA1140CUMULATIVEREGISTERSGraphs2(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            bool FirstOne = true;
            System.DateTime StartDate = default(System.DateTime);
            System.DateTime EndDate = default(System.DateTime);
            double StartVal = 0;
            double EndVal = 0;
            //LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
            {
                int MyField = MyDataHistory.Field;
                //- 1 'field always starts at 1
                //ignore status
                if (MyField == 1)
                {
                    if (FirstOne)
                    {
                        StartDate = MyDataHistory.DT;
                        StartVal = MyDataHistory.Value;
                        FirstOne = false;
                    }
                    EndDate = MyDataHistory.DT;
                    EndVal = MyDataHistory.Value;
                    //If MyField > MaxFieldCnt Then
                    //    MaxFieldCnt = MyField
                    //End If
                    //Dim myret As Integer
                    //myret = numericTimeSeries1(MyField - 1).Points.Add(New NumericTimeDataPoint())
                    // numericTimeSeries1(MyField - 1).Points(myret).TimeValue = MyDataHistory.DT
                    //numericTimeSeries1(MyField - 1).Points(myret).NumericValue = MyDataHistory.Value
                }

            }

            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<table border=1><tr><td>");
            MyHtmlStr.Append("Cumulative KWh Report: " + SensorDet.Caption);
            MyHtmlStr.Append("</td><td></td></tr>");
            MyHtmlStr.Append("<tr><td>");
            MyHtmlStr.Append("StartDate:" + StartDate.ToString());
            MyHtmlStr.Append("</td><td>");
            MyHtmlStr.Append("StartValue:" + StartVal.ToString());
            MyHtmlStr.Append("</td></tr>");
            MyHtmlStr.Append("<tr><td>");
            MyHtmlStr.Append("EndDate:" + EndDate.ToString());
            MyHtmlStr.Append("</td><td>");
            MyHtmlStr.Append("EndValue:" + EndVal.ToString());
            MyHtmlStr.Append("</td></tr>");
            MyHtmlStr.Append("<tr><td>");
            MyHtmlStr.Append("Minutes Difference:" + DateAndTime.DateDiff(DateInterval.Minute, StartDate, EndDate).ToString());
            MyHtmlStr.Append("</td><td>");
            MyHtmlStr.Append("Value Difference :" + (EndVal - StartVal).ToString());
            MyHtmlStr.Append("</td></tr>");

            //MyHtmlStr.Append("Meter :" & SensorDet.Caption)
            //ExcelStrData += "Sensor:" & SensorDet.Caption + vbCrLf
            MyHtmlStr.Append("</td></tr></table>");
            this.DivTarrifReport.InnerHtml = MyHtmlStr.ToString();

        }

        public void DrawElsterA1140CUMULATIVEREGISTERSGraphs3(LiveMonitoring.IRemoteLib.SensorDetails SensorDet, Collection MyData)
        {
            Infragistics.WebUI.UltraWebChart.UltraChart MyDateLinechart1 = new Infragistics.WebUI.UltraWebChart.UltraChart();
            //attach event for legent width
            MyDateLinechart1.ChartDrawItem += ultraChart1_ChartDrawItem;
            MyDateLinechart1.ID = "ElsterA1140Wh" + SensorDet.ID.ToString();
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
            MyDateLinechart1.TitleTop.Text = "Elster Wh :" + SensorDet.Caption;
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
            // Add the Chart Area to the ChartAreas collection
            MyDateLinechart1.CompositeChart.ChartAreas.Add(myDLChartArea1);
            // Create the ChartLayer
            ChartLayerAppearance DLchartLayer1 = new ChartLayerAppearance();
            // Set the ChartType
            DLchartLayer1.ChartType = ChartType.LineChart;
            // Create an X axis
            AxisItem DLxAxis1 = new AxisItem();
            DLxAxis1.axisNumber = AxisNumber.X_Axis;
            DLxAxis1.DataType = AxisDataType.String;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL>";
            DLxAxis1.Labels.Font = new Font("Tahoma", 7);
            DLxAxis1.LineThickness = 1;
            // Create an Y axis
            AxisItem DLyAxis1 = new AxisItem();
            DLyAxis1.axisNumber = AxisNumber.Y_Axis;
            DLyAxis1.Labels.ItemFormatString = "<DATA_VALUE:0.00#>";
            DLyAxis1.Labels.Font = new Font("Tahoma", 7);
            DLyAxis1.LineThickness = 1;
            myDLChartArea1.Axes.Add(DLxAxis1);
            myDLChartArea1.Axes.Add(DLyAxis1);
            // Set the axes
            DLchartLayer1.AxisX = DLxAxis1;
            DLchartLayer1.AxisY = DLyAxis1;
            // Set the ChartArea
            DLchartLayer1.ChartArea = myDLChartArea1;
            // Add the ChartLayer to the ChartLayers collection
            MyDateLinechart1.CompositeChart.ChartLayers.Add(DLchartLayer1);
            ///''''''''''''''''''
            ISeries DLseries1 = null;
            NumericTimeSeries[] numericTimeSeries1 = new NumericTimeSeries[5];
            int Bcnt = 0;
            LiveMonitoring.IRemoteLib.LovatoGenset MyGenset = new LiveMonitoring.IRemoteLib.LovatoGenset();

            for (Bcnt = 0; Bcnt <= 1; Bcnt++)
            {
                numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                if (SensorDet.Fields.Contains((Bcnt + 1).ToString()) == true)
                {
                    if (((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(Bcnt + 1).ToString()]).DisplayValue)
                    {
                        numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                        numericTimeSeries1[Bcnt].Label = ((LiveMonitoring.IRemoteLib.SensorFieldsDef)SensorDet.Fields[(Bcnt + 1).ToString()]).FieldName;
                    }
                }
                else
                {
                    numericTimeSeries1[Bcnt] = new NumericTimeSeries();
                    numericTimeSeries1[Bcnt].Label = "Unknown";
                }
            }
            int tmp1cntwe = 0;
            //mycnt1= fields ?
            // LiveMonitoring.IRemoteLib.DataHistory MyDataHistory = default(LiveMonitoring.IRemoteLib.DataHistory);
            int MaxFieldCnt = 0;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDataHistory in MyData)
            {
                int MyField = MyDataHistory.Field;
                //- 1 'field always starts at 1
                //ignore status
                if (MyField >= 1 & MyField <= 2)
                {
                    if (MyField > MaxFieldCnt)
                    {
                        MaxFieldCnt = MyField;
                    }
                    int myret = 0;
                    myret = numericTimeSeries1[MyField - 1].Points.Add(new NumericTimeDataPoint());
                    numericTimeSeries1[MyField - 1].Points[myret].TimeValue = MyDataHistory.DT;
                    numericTimeSeries1[MyField - 1].Points[myret].NumericValue = MyDataHistory.Value;
                }

            }
            int Acnt = 0;
            for (Acnt = 0; Acnt <= MaxFieldCnt - 1; Acnt++)
            {
                DLchartLayer1.Series.Add(numericTimeSeries1[Acnt]);
                DLseries1 = numericTimeSeries1[Acnt];
                MyDateLinechart1.Series.Add(DLseries1);
            }
            ///''''''''''''''''''
            // Set X axis
            DLxAxis1.Labels.Orientation = TextOrientation.VerticalLeftFacing;
            DLxAxis1.Labels.ItemFormatString = "<ITEM_LABEL:MM/dd/yyyy HH:mm>";
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
            // Add the series to the ChartLayer's Series collection.
            MyDateLinechart1.Width = 700;
            MyDateLinechart1.Height = 500;
            CompositeLegend legend1 = new CompositeLegend();
            legend1.LabelStyle.Font = new Font("Times New Roman", 10);
            MyDateLinechart1.CompositeChart.Legends.Add(legend1);
            legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers[0]);
            legend1.BoundsMeasureType = MeasureType.Percentage;
            legend1.Bounds = new Rectangle(5, 5, 105, 14);
            MyDateLinechart1.CompositeChart.ChartAreas[0].BoundsMeasureType = MeasureType.Percentage;
            MyDateLinechart1.CompositeChart.ChartAreas[0].Bounds = new Rectangle(0, 20, 100, 80);
            this.Charts.Controls.Add(MyDateLinechart1);
            //Dim legend1 As New CompositeLegend()
            //legend1.LabelStyle.Font = New Font("Times New Roman", 10)
            //MyDateLinechart1.CompositeChart.Legends.Add(legend1)
            //legend1.ChartLayers.Add(MyDateLinechart1.CompositeChart.ChartLayers(0))
            //legend1.BoundsMeasureType = MeasureType.Percentage
            // legend1.Bounds = New Rectangle(5, 5, 105, 14)

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
            //lblErr.Visible = false;
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
      
            try
            {
                txtStart.Text = Session["StartDate"].ToString();
                txtEnd.Text = Session["EndDate"].ToString();
                RegenerateCallbackGraphs();
            }
            catch (Exception ex)
            {
            }

        }
        protected void  btnGenerate1_Click(object sender, EventArgs e)
        {
             lblErr.Visible = false;
            if (string.IsNullOrEmpty(txtStart.Text) | string.IsNullOrEmpty(txtEnd.Text))
            {
                lblErr.Visible = true;
                lblErr.Text = "Error please select start/end date!";
                return;
            }
            Session["StartDate"] = txtStart.Text;
            Session["EndDate"] = txtEnd.Text;
            RegenerateCallbackGraphs();
        }
        public void FillSessionSensors()
        {
            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
            Collection MyCollection = new Collection();
            //clear session var
            //Session("Sensors") = ""
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Session("Sensors") += SensorsList.SelectedValue
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
              //this.Charts.Controls.Clear();
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
                        if (!string.IsNullOrEmpty(mySensors[Acnt]))
                        {
                            if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID)
                            {
                                AddLayer(MySensor);
                                AddPageBreak();
                               //AddTarrifReport(MySensor)
                                break; // TODO: might not be correct. Was : Exit For

                            }
                            else
                            {
                                MyCnt += 1;
                            }
                        }

                    }
                }
                //AddImages(Acnt + 1, CInt(myCameras(Acnt)), 200, 200)
            }
        }
        public void MeteringCumulativeKWReport()
        {
            Load += Page_Load;
        }

        protected void cmbCurrentSite_SelectedIndexChanged(object sender, EventArgs  e)
        {
            Session["SelectedSite"] = ddlCurrentSite.SelectedValue;
        }


        //protected void btnGenerate1_Click(object sender, EventArgs e)
        //    {

        //    }

        //protected void btnGenerate2_Click(object sender, EventArgs e)
        //{

        //}
    }
}