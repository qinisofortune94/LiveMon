using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;

using System.Runtime.Remoting.Channels;
using System.Drawing;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

namespace website2016V2
{

    partial class NewVideo : System.Web.UI.Page
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

                if (IsPostBack == false)
                {
                    Response.Expires = 0;
                    Response.CacheControl = "no-cache";
                    Page.MaintainScrollPositionOnPostBack = true;
                    Load_Cameras();
                    Collection MyCol = null;
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    MyCol = MyRem.LiveMonServer.GetAllVideoByDate(DateAndTime.DateAdd(DateInterval.Hour, -24, DateAndTime.Now), DateAndTime.DateAdd(DateInterval.Hour, 1, DateAndTime.Now));
                    MyRem.VideoFiles = MyCol;
                    FillVideoFiles(MyCol);
                    FillSchedule(MyCol);
                    //WebScheduleInfo1.ActiveDayUtc = New Infragistics.WebUI.Shared.SmartDate(Now)
                }
                else
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    Control c = MyRem.GetPostBackControl(this.Page);
                    if ((c == null) == false)
                    {
                        if (c.ID != "btnPlay" & !string.IsNullOrEmpty(CurDate.Value))
                        {
                            Collection MyCol = null;
                            //DateTime.Parse("",system.Globalization.u
                            DateTime NewDate = System.Convert.ToDateTime(CurDate.Value);
                            //NewDate()
                            MyCol = MyRem.LiveMonServer.GetAllVideoByDate(NewDate, DateAndTime.DateAdd(DateInterval.Day, 1, NewDate));
                            FillVideoFiles(MyCol);
                            FillSchedule(MyRem.VideoFiles);
                        }
                        else
                        {
                            if (c.ID == "btnPlay" & !string.IsNullOrEmpty(CurDate.Value))
                            {
                                Collection MyCol = null;
                                //DateTime.Parse("",system.Globalization.u
                                DateTime NewDate = System.Convert.ToDateTime(CurDate.Value);
                                //NewDate()
                                MyCol = MyRem.LiveMonServer.GetAllVideoByDate(NewDate, DateAndTime.DateAdd(DateInterval.Day, 1, NewDate));
                                FillSchedule(MyRem.VideoFiles);
                            }
                        }
                    }
                    else
                    {
                        Collection MyCol = null;
                        //DateTime.Parse("",system.Globalization.u
                        DateTime NewDate = System.Convert.ToDateTime(CurDate.Value);
                        //NewDate()
                        MyCol = MyRem.LiveMonServer.GetAllVideoByDate(DateAndTime.DateAdd(DateInterval.Hour, -24, NewDate), DateAndTime.DateAdd(DateInterval.Hour, 24, NewDate));
                        FillVideoFiles(MyCol);
                        if ((MyRem.VideoFiles == null) == false)
                        {
                            FillSchedule(MyRem.VideoFiles);
                        }
                    }

                }

            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            
        }
        public void Load_Cameras()
        {
            //CameraMenu
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            this.ddlCamera.Items.Clear();
            System.Web.UI.WebControls.ListItem WebMenuItem = new System.Web.UI.WebControls.ListItem();
            WebMenuItem.Text = "All Cameras";
            WebMenuItem.Value = "All Cameras";
            this.ddlCamera.Items.Add(WebMenuItem);
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyWebMenuItem = new System.Web.UI.WebControls.ListItem();
                    MyWebMenuItem.Text = MyCamera.Caption + " ID=Camera" + MyCamera.ID.ToString();
                    MyWebMenuItem.Value = MyCamera.ID.ToString();
                    this.ddlCamera.Items.Add(MyWebMenuItem);
                }
            }
        }



        protected void ddlCamera_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }
        public void FillVideoFiles(Collection MyCollect)
        {
            System.Configuration.Configuration rootWebConfig1 = default(System.Configuration.Configuration);
            System.Configuration.KeyValueConfigurationElement customSetting = default(System.Configuration.KeyValueConfigurationElement);
            rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);
            customSetting = rootWebConfig1.AppSettings.Settings["Web.HomeDir"];
            string RemoteFilePath = customSetting.Value;
            ddlFiles.Items.Clear();
            //LiveMonitoring.IRemoteLib.VideoPlayBackFiles MyFileInfo = default(LiveMonitoring.IRemoteLib.VideoPlayBackFiles);
            foreach (LiveMonitoring.IRemoteLib.VideoPlayBackFiles MyFileInfo in MyCollect)
            {
                System.Web.UI.WebControls.ListItem WebMenuItem = new System.Web.UI.WebControls.ListItem();
                //Dim MyPathtmp As String = "~/" & MyFileInfo.Path.Replace(RemoteFilePath, "")
                string MyPathtmp = "http://" + this.Page.Request.Url.Host + ":" + this.Page.Request.Url.Port + MyFileInfo.Path.Replace(RemoteFilePath, "");
                //Dim MyPathtmp As String = "http://" & Me.Page.Request.Url.Host & MyFileInfo.Path.Replace(RemoteFilePath, "")
                WebMenuItem.Text = MyFileInfo.FileName + " Date:" + MyFileInfo.FileDate.ToString();
                //TODO:Change back for deploy
                WebMenuItem.Value = MyPathtmp.Replace("\\", "/");
                //MyFileInfo.Path '
                ddlFiles.Items.Add(WebMenuItem);
            }
        }

        public void FillSchedule(Collection MyCollect)
        {
            if ((MyCollect == null) == false)
            {
                //LiveMonitoring.IRemoteLib.VideoPlayBackFiles MyFileInfo = default(LiveMonitoring.IRemoteLib.VideoPlayBackFiles);
                foreach (LiveMonitoring.IRemoteLib.VideoPlayBackFiles MyFileInfo in MyCollect)
                {
                    //'Dim appt As New Infragistics.WebUI.WebSchedule.Appointment(WebScheduleInfo1)
                    //appt.StartDateTime = New Infragistics.WebUI.Shared.SmartDate(MyFileInfo.FileDate)
                    //appt.EndDateTime = New Infragistics.WebUI.Shared.SmartDate(DateTime.Now.AddHours(1))
                    //appt.Subject = MyFileInfo.FileName
                    //appt.EnableReminder = False
                    //appt.Duration = New TimeSpan(0, 0, 120)
                    //appt.Description = "This is the Description of the appointment"
                    //appt.AllDayEvent = False
                    //appt.Status = 0
                    //appt.DataKey = MyFileInfo.FileName
                    //appt.ResourceKey = -999
                    //appt.EnableReminder = False
                    //'app.ReminderInterval. = 900
                    //'app.ShowTimeAs = 3
                    //appt.Importance = 0
                    // WebScheduleInfo1.Activities.Add(appt)
                    //VideoFilesView.WebScheduleInfo.Activities.Add(appt)
                }
            }

        }

        protected void btnPlay_Click(object sender, System.EventArgs e)
        {
            string cntrlstr = "";
            cntrlstr += "<OBJECT ID='MediaPlayBack2' ";
            switch (ddlSize.SelectedValue)
            {
                case "0":
                    cntrlstr += " WIDTH=100 HEIGHT=100 ";
                    break;
                case "1":
                    cntrlstr += " WIDTH=200 HEIGHT=200 ";
                    break;
                case "2":
                    cntrlstr += " WIDTH=300 HEIGHT=300 ";
                    break;
                case "3":
                    cntrlstr += " WIDTH=400 HEIGHT=400 ";
                    break;
                case "4":
                    cntrlstr += " WIDTH=500 HEIGHT=500 ";
                    break;
                case "5":
                    cntrlstr += " WIDTH=600 HEIGHT=600 ";
                    break;
                default:
                    cntrlstr += " WIDTH=250 HEIGHT=250 ";
                    break;
            }
            cntrlstr += "CLASSID=CLSID:5B32067A-121B-49DE-8182-91EB13DDF8D6 ";
            cntrlstr += " CODEBASE='MediaPb.cab#version=2,2,0,8'> ";
            cntrlstr += "<PARAM NAME='SliderVisible' VALUE='true'> ";
            switch (ddlSpeed.SelectedValue)
            {
                case "0":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='1'> ";
                    break;
                case "1":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='2'> ";
                    break;
                case "2":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='3'> ";
                    break;
                case "3":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='4'> ";
                    break;
                case "4":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='5'> ";
                    break;
                case "5":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='6'> ";
                    break;
                case "6":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='-2'> ";
                    break;
                case "7":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='-3'> ";
                    break;
                case "8":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='-4'> ";
                    break;
                case "9":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='-5'> ";
                    break;
                case "10":
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='-6'> ";
                    break;
                default:
                    cntrlstr += " <PARAM NAME='PlaybackSpeed' VALUE='1'> ";
                    break;
            }
            cntrlstr += "<PARAM NAME='MediaMapVisible' VALUE='false'> ";
            cntrlstr += "<PARAM NAME='AutoPlay' VALUE='true'> ";
            cntrlstr += "<PARAM NAME='PlayFileName' VALUE='" + ddlFiles.SelectedValue + "'> ";
            //cntrlstr += "<PARAM NAME='PlayFileName' VALUE='http://192.168.1.88/hgd3/test_200000029.hgd'> "
            cntrlstr += "</OBJECT>";           
            this.div2.InnerHtml = "";
            this.div2.InnerHtml += cntrlstr;
            //C:\Rats\IPMonDev\IPMonDev\bin\Debug\VideoData
            try
            {
                System.Configuration.Configuration rootWebConfig1 = default(System.Configuration.Configuration);
                System.Configuration.KeyValueConfigurationElement customSetting = default(System.Configuration.KeyValueConfigurationElement);
                rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);
                customSetting = rootWebConfig1.AppSettings.Settings["Web.HomeDir"];
                string RemoteFilePath = customSetting.Value;
                //cmbFiles.Items.Clear()
                //Dim MyFileInfo As LiveMonitoring.IRemoteLib.VideoPlayBackFiles
                //For Each MyFileInfo In MyCollect
                //    Dim WebMenuItem As New Web.UI.WebControls.ListItem()
                //    'Dim MyPathtmp As String = "~/" & MyFileInfo.Path.Replace(RemoteFilePath, "")
                string MyPathtmp = "http://" + this.Page.Request.Url.Host + ":" + this.Page.Request.Url.Port + "/PlayBackFile.aspx?PlayFileURL=";
                string tmpFIle = ddlFiles.SelectedValue.Replace("http://" + this.Page.Request.Url.Host + ":" + this.Page.Request.Url.Port, RemoteFilePath).Replace("/", "\\");
                //    'Dim MyPathtmp As String = "http://" & Me.Page.Request.Url.Host & MyFileInfo.Path.Replace(RemoteFilePath, "")
                //    WebMenuItem.Text = MyFileInfo.FileName & " Date:" & MyFileInfo.FileDate.ToString
                //    'TODO:Change back for deploy
                //    WebMenuItem.Value = MyPathtmp.Replace("\", "/") 'MyFileInfo.Path '
                //    cmbFiles.Items.Add(WebMenuItem)
                //Next

                DirectLink.Visible = true;
                DirectLink.NavigateUrl = MyPathtmp + tmpFIle;

            }
            catch (Exception ex)
            {
            }
        }



        protected void btnGetFiles_Click(object sender, System.EventArgs e)
        {
            Collection MyCollection = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            if (ddlCamera.SelectedValue == "All Cameras")
            {
                MyCollection = MyRem.LiveMonServer.GetAllVideo();
            }
            else
            {

                if (string.IsNullOrEmpty(txtEndDate.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please select end date.";

                    return;
                }
                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please select start date.";
                    return;
                }
                if (ddlCamera.SelectedValue == "-1")
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please select a camera.";
                    return;
                }
                MyCollection = MyRem.LiveMonServer.GetCameraVideoByDate(Convert.ToInt32(ddlCamera.SelectedValue), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
            }
            FillVideoFiles(MyCollection);
            FillSchedule(MyCollection);
        }
        public NewVideo()
        {
            Load += Page_Load;
        }
    }
}