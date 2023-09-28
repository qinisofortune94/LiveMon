using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.IO;
using Infragistics.WebUI.UltraWebGrid;
using Microsoft.VisualBasic;
using System.Web.UI.WebControls;

namespace website2016V2
{
    partial class ServerConfiguration : System.Web.UI.Page
    {
        public void LoadPage()
        {
            Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.SMSDevices));
            Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.SMSDevices));
            int MyVal = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                SMSGrid.Rows.Clear();
                for (MyVal = 0; MyVal <= 99; MyVal++)
                {
                    if (!string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("SMS." + MyVal.ToString() + ".DeviceType")))
                    {
                        UltraGridRow myrow = new UltraGridRow(true);
                        myrow.Cells.Add();
                        myrow.Cells[0].Text = (MyRem.LiveMonServer.GetConfigSetting("SMS." + MyVal.ToString() + ".DeviceType"));
                        myrow.Cells.Add();
                        myrow.Cells[1].Value = MyVal.ToString();
                        myrow.Tag = MyVal;
                        SMSGrid.Rows.Add(myrow);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            try
            {
                EmailGrid.Rows.Clear();
                for (MyVal = 0; MyVal <= 99; MyVal++)
                {
                    if (!string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("Email." + MyVal.ToString() + ".SMTPServer")))
                    {
                        UltraGridRow myrow = new UltraGridRow(true);
                        myrow.Cells.Add();
                        myrow.Cells[0].Value = MyRem.LiveMonServer.GetConfigSetting("Email." + MyVal.ToString() + ".SMTPServer");
                        myrow.Cells.Add();
                        myrow.Cells[1].Value = MyVal.ToString();
                        myrow.Tag = MyVal;
                        EmailGrid.Rows.Add(myrow);
                    }
                }


            }
            catch (Exception ex)
            {
            }
            try
            {
                SMSTimer.Text = MyRem.LiveMonServer.GetConfigSetting("SMS.Timer");
                EmailTimer.Text = MyRem.LiveMonServer.GetConfigSetting("Email.Timer");
                MMSTimer.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.Timer");
                //load mms
                this.txtMMSURL.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.MyMMSURL");
                this.txtMMSSMTPAPIId.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.api_id");
                this.txtMMSSMTPUser.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.user");
                this.txtMMSSMTPPassword.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.password");
                this.txtMMSFTPServer.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.FTPServer");
                this.txtMMSFTPUser.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.FTPUserName");
                this.txtMMSFTPPassword.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.FTPPassWord");
                this.txtMMSFTPRemotePath.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.RemotePath");
                this.txtMMSImageURL.Text = MyRem.LiveMonServer.GetConfigSetting("MMS.ImageURL");

            }
            catch (Exception ex)
            {
            }

            try
            {
                //load serial settings
                //Array ItemHandshake = System.Enum.GetValues(typeof(Ports.Handshake));
                //Array HandshakeItem = System.Enum.GetNames(typeof(Ports.Handshake));
                //string x = null;
                //MyVal = 0;
                //this.ddlHandShake.Items.Clear();
                //foreach (string x_loopVariable in HandshakeItem)
                //{
                //    x = x_loopVariable;
                //    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                //    MyItem.Text = x;
                //    MyItem.Value = ItemHandshake(MyVal);
                //    MyItem.Selected = false;
                //    this.ddlHandShake.Items.Add(MyItem);
                //    MyVal += 1;
                //}
                //Array ItemParity = System.Enum.GetValues(typeof(Ports.Parity));
                //Array ParityItem = System.Enum.GetNames(typeof(Ports.Parity));
                //MyVal = 0;
                //this.ddlComParity.Items.Clear();
                //foreach (string x_loopVariable in ParityItem)
                //{
                //    x = x_loopVariable;
                //    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                //    MyItem.Text = x;
                //    MyItem.Value = ItemParity(MyVal);
                //    MyItem.Selected = false;
                //    this.ddlComParity.Items.Add(MyItem);
                //    MyVal += 1;
                //}
                //Array ItemStopBits = System.Enum.GetValues(typeof(Ports.StopBits));
                //Array StopBitsItem = System.Enum.GetNames(typeof(Ports.StopBits));
                //MyVal = 0;
                //this.ddlStopBits.Items.Clear();
                //foreach (string x_loopVariable in StopBitsItem)
                //{
                //    x = x_loopVariable;
                //    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                //    MyItem.Text = x;
                //    MyItem.Value = ItemStopBits(MyVal);
                //    MyItem.Selected = false;
                //    this.ddlStopBits.Items.Add(MyItem);
                //    MyVal += 1;
                //}
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.ServerConfig(this.ddlHandShake, this.ddlComParity, this.ddlStopBits);
                ddlComSpeed.Items.Clear();
                int MyInt = 0;
                for (MyInt = 0; MyInt <= 17; MyInt++)
                {
                    System.Web.UI.WebControls.ListItem NewItem = new System.Web.UI.WebControls.ListItem();
                    switch (MyInt)
                    {
                        case 0:
                            NewItem.Text = "75";
                            NewItem.Value = Convert.ToString(75);
                            break;
                        case 1:
                            NewItem.Text = "110";
                            NewItem.Value = Convert.ToString(110);
                            break;
                        case 2:
                            NewItem.Text = "134";
                            NewItem.Value = Convert.ToString(134);
                            break;
                        case 3:
                            NewItem.Text = "150";
                            NewItem.Value = Convert.ToString(150);
                            break;
                        case 4:
                            NewItem.Text = "300";
                            NewItem.Value = Convert.ToString(300);
                            break;
                        case 5:
                            NewItem.Text = "600";
                            NewItem.Value = Convert.ToString(600);
                            break;
                        case 6:
                            NewItem.Text = "1200";
                            NewItem.Value = Convert.ToString(1200);
                            break;
                        case 7:
                            NewItem.Text = "1800";
                            NewItem.Value = Convert.ToString(1800);
                            break;
                        case 8:
                            NewItem.Text = "2400";
                            NewItem.Value = Convert.ToString(2400);
                            break;
                        case 9:
                            NewItem.Text = "4800";
                            NewItem.Value = Convert.ToString(4800);
                            break;
                        case 10:
                            NewItem.Text = "7200";
                            NewItem.Value = Convert.ToString(7200);
                            break;
                        case 11:
                            NewItem.Text = "9600";
                            NewItem.Value = Convert.ToString(9600);

                            break;
                        case 12:
                            NewItem.Text = "14400";
                            NewItem.Value = Convert.ToString(14400);
                            break;
                        case 13:
                            NewItem.Text = "19200";
                            NewItem.Value = Convert.ToString(19200);
                            break;
                        case 14:
                            NewItem.Text = "38400";
                            NewItem.Value = Convert.ToString(38400);
                            break;
                        case 15:
                            NewItem.Text = "57600";
                            NewItem.Value = Convert.ToString(57600);
                            break;
                        case 16:
                            NewItem.Text = "115200";
                            NewItem.Value = Convert.ToString(115200);
                            NewItem.Selected = true;
                            break;
                        case 17:
                            NewItem.Text = "128000";
                            NewItem.Value = Convert.ToString(128000);
                            break;
                    }
                    ddlComSpeed.Items.Add(NewItem);
                }

            }
            catch (Exception ex)
            {
            }
            try
            {
                //Me.txtReportName.Text = MyRem.LiveMonServer.GetConfigSetting("Report1.Name")
                //Me.txtReportHours.Text = MyRem.LiveMonServer.GetConfigSetting("Report1.Scheduling.Hours")
                //Dim myint1 As Integer
                //Dim MyDays As String = MyRem.LiveMonServer.GetConfigSetting("Report1.Scheduling.Days")
                //For myint1 = 0 To 6
                //    If MyDays.IndexOf(myint1.ToString) > -1 Then
                //        Me.chkDays.Items(myint1 - 1).Selected = True
                //    End If
                //Next
                //Me.txtReportRecipients.Text = MyRem.LiveMonServer.GetConfigSetting("Report1.Recipients")

                //Me.txtReport2Name.Text = MyRem.LiveMonServer.GetConfigSetting("Report2.Name")
                //Me.txtReport2Hours.Text = MyRem.LiveMonServer.GetConfigSetting("Report2.Scheduling.Hours")
                //MyDays = MyRem.LiveMonServer.GetConfigSetting("Report2.Scheduling.Days")
                //For myint1 = 0 To 6
                //    If MyDays.IndexOf(myint1.ToString) > -1 Then
                //        Me.chk2Days.Items(myint1 - 1).Selected = True
                //    End If
                //Next
                //Me.txtReport2Recipients.Text = MyRem.LiveMonServer.GetConfigSetting("Report2.Recipients")

                //Me.txtReport3Name.Text = MyRem.LiveMonServer.GetConfigSetting("Report3.Name")
                //Me.txtReport3Hours.Text = MyRem.LiveMonServer.GetConfigSetting("Report3.Scheduling.Hours")
                //MyDays = MyRem.LiveMonServer.GetConfigSetting("Report3.Scheduling.Days")
                //For myint1 = 0 To 6
                //    If MyDays.IndexOf(myint1.ToString) > -1 Then
                //        Me.chk3Days.Items(myint1 - 1).Selected = True
                //    End If
                //Next
                //Me.txtReport3Recipients.Text = MyRem.LiveMonServer.GetConfigSetting("Report3.Recipients")
                //Me.cmbrep3DailySetting.SelectedValue = MyRem.LiveMonServer.GetConfigSetting("Report3.DataPeriod")
                //Me.txtReport3SensorField.Text = MyRem.LiveMonServer.GetConfigSetting("Report3.SensorFields")


            }
            catch (Exception ex)
            {
            }
            try
            {
                this.txtVideoDays.Text = MyRem.LiveMonServer.GetConfigSetting("History.Config.Video.FileSize");
                this.txtSensorDays.Text = MyRem.LiveMonServer.GetConfigSetting("History.Config.Sensor.Days");
                this.txtDialUp.Text = MyRem.LiveMonServer.GetConfigSetting("DataSend.BackupModem");

                this.chkServerPing.Checked = Convert.ToBoolean(MyRem.LiveMonServer.GetConfigSetting("Ping.Server.Up"));
                this.txtPingEmail.Text = MyRem.LiveMonServer.GetConfigSetting("Ping.Server.Email");

                this.txtResetServer.Text = MyRem.LiveMonServer.GetConfigSetting("Reset.Timer");
                this.chkIncludeStart.Checked = Convert.ToBoolean(MyRem.LiveMonServer.GetConfigSetting("Alert.IncludeDateStart"));


            }
            catch (Exception ex)
            {
            }
        }


        protected void Page_Init(object sender, System.EventArgs e)
        {
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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    LoadPage();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void EmailGrid_ActiveRowChange(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            UltraGridRow myrow = e.Row;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            EmailEdit.Visible = true;
            txtDialUp.Text = myrow.Tag.ToString();
            this.txtEmailServer.Text = MyRem.LiveMonServer.GetConfigSetting("Email." + myrow.Tag.ToString() + ".SMTPServer");
            txtAdress.Text = MyRem.LiveMonServer.GetConfigSetting("Email." + myrow.Tag.ToString() + ".From");

        }

        protected void SMSGrid_ActiveRowChange(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            UltraGridRow myrow = e.Row;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.SMSDevices));
            Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.SMSDevices));
            int MyType = Convert.ToInt32(MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".DeviceType"));
            switch (MyType)
            {
                case 0:
                    WebPanel1.Text = myrow.Tag.ToString();
                    WebPanel1.Visible = true;
                    WebPanel2.Visible = false;
                    WebPanel3.Visible = false;
                    WebPanel4.Visible = false;
                    txtComport.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".Comport");
                    ddlComSpeed.SelectedValue = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".ComSpeed");
                    txtComDataBits.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".ComDataBits");
                    ddlComParity.SelectedValue = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".ComParity");
                    ddlStopBits.SelectedValue = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".ComStopBits");
                    txtBufferSize.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".ComBuffSize");
                    ddlHandShake.SelectedValue = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".Handshaking");
                    break;
                case 1:
                    WebPanel1.Visible = false;
                    WebPanel2.Visible = true;
                    WebPanel2.Text = myrow.Tag.ToString();
                    WebPanel3.Visible = false;
                    WebPanel4.Visible = false;
                    txtSMTPServerName.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".MySMTPserver");
                    txtSMTPFrom.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".FromAdd");
                    txtSMTPTo.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".ToAdd");
                    txtSMTPSubject.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".Subject");
                    txtSMTPAPIId.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".api_id");
                    txtSMTPUser.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".user");
                    txtSMTPPassword.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".password");
                    txtSMTPReply.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".reply");

                    break;
                case 2:
                    WebPanel1.Visible = false;
                    WebPanel2.Visible = false;
                    WebPanel3.Visible = true;
                    WebPanel3.Text = myrow.Tag.ToString();
                    WebPanel4.Visible = false;
                    txtFTPServer.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".FTPServer");
                    txtFTPUser.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".UserName");
                    txtFTPPassword.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".PassWord");
                    txtFTPRemotePath.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".RemotePath");
                    txtFTPOutputString.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".OutPutStr");
                    break;
                case 3:
                    WebPanel1.Visible = false;
                    WebPanel2.Visible = false;
                    WebPanel3.Visible = false;
                    WebPanel4.Visible = true;
                    WebPanel4.Text = myrow.Tag.ToString();
                    txtHttpURL.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".URL");
                    txtHttpUser.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".reply");
                    txtHttpPassword.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".api_id");
                    txtHttpReplyURL.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".user");
                    txtHttpAPIId.Text = MyRem.LiveMonServer.GetConfigSetting("SMS." + myrow.Tag.ToString() + ".password");

                    break;
                default:
                    WebPanel1.Visible = false;
                    WebPanel2.Visible = false;
                    WebPanel3.Visible = false;
                    WebPanel4.Visible = false;
                    break;
            }
            EmailEdit.Visible = true;
            txtDialUp.Text = myrow.Tag.ToString();
            this.txtEmailServer.Text = MyRem.LiveMonServer.GetConfigSetting("Email." + myrow.Tag.ToString() + ".SMTPServer");
            txtAdress.Text = MyRem.LiveMonServer.GetConfigSetting("Email." + myrow.Tag.ToString() + ".From");

        }

        protected void btnUpdateSMSGSMDevice_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            int MyRow = Convert.ToInt32(WebPanel1.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".Comport", txtComport.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComSpeed", ddlComSpeed.SelectedValue);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComDataBits", txtComDataBits.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComParity", ddlComParity.SelectedValue);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComStopBits", ddlStopBits.SelectedValue);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComBuffSize", txtBufferSize.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".Handshaking", ddlHandShake.SelectedValue);
                WebPanel1.Visible = false;
                LoadPage();

                successMessage.Visible = true;
                lblSuccess.Text = "SMS GSM saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "SMS GSM saving failed. Please try again.";
            }
        }

        protected void btnUpdateFTP_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            int MyRow = Convert.ToInt32(WebPanel3.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".FTPServer", this.txtFTPServer.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".UserName", this.txtFTPUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".PassWord", this.txtFTPPassword.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".RemotePath", this.txtFTPRemotePath.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".OutPutStr", this.txtFTPOutputString.Text);
                WebPanel3.Visible = false;
                LoadPage();
                successMessage.Visible = true;
                lblSuccess.Text = "FTP saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "FTP saving failed. Please try again.";
            }


        }

        protected void btnHttpUpdate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            int MyRow = Convert.ToInt32(WebPanel4.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".URL", this.txtHttpURL.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".reply", this.txtHttpReplyURL.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".api_id", this.txtHttpAPIId.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".user", this.txtHttpUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".password", this.txtHttpPassword.Text);
                WebPanel4.Visible = false;
                LoadPage();

                successMessage.Visible = true;
                lblSuccess.Text = "Http saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Http saving failed. Please try again.";
            }

        }

        protected void btnAddGSM_Click(object sender, System.EventArgs e)
        {
            int MyVal = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            for (MyVal = 0; MyVal <= 99; MyVal++)
            {
                if (string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("SMS." + MyVal.ToString() + ".DeviceType")))
                {
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            int MyRow = MyVal;

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".Comport", txtComport.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComSpeed", ddlComSpeed.SelectedValue);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComDataBits", txtComDataBits.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComParity", ddlComParity.SelectedValue);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComStopBits", ddlStopBits.SelectedValue);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ComBuffSize", txtBufferSize.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".Handshaking", ddlHandShake.SelectedValue);
                LoadPage();
                WebPanel1.Visible = true;
                WebPanel1.Text = MyRow.ToString();

                successMessage.Visible = true;
                lblSuccess.Text = "GSM saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "GSM saving failed. Please try again.";
            }


        }

        protected void btnAddSMTP_Click(object sender, System.EventArgs e)
        {
            int MyVal = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            for (MyVal = 0; MyVal <= 99; MyVal++)
            {
                if (string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("SMS." + MyVal.ToString() + ".DeviceType")))
                {
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            int MyRow = MyVal;

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".MySMTPserver", this.txtSMTPServerName.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".FromAdd", this.txtSMTPFrom.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ToAdd", this.txtSMTPTo.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".Subject", this.txtSMTPSubject.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".api_id", this.txtSMTPAPIId.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".user", this.txtSMTPUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".password", this.txtSMTPPassword.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".reply", this.txtSMTPReply.Text);
                LoadPage();
                WebPanel2.Visible = true;
                WebPanel2.Text = MyRow.ToString();

                successMessage.Visible = true;
                lblSuccess.Text = "SMTP saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "SMTP saving failed. Please try again.";
            }


        }

        protected void btnAddFTP_Click(object sender, System.EventArgs e)
        {
            int MyVal = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            for (MyVal = 0; MyVal <= 99; MyVal++)
            {
                if (string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("SMS." + MyVal.ToString() + ".DeviceType")))
                {
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            int MyRow = MyVal;

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".FTPServer", this.txtFTPServer.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".UserName", this.txtFTPUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".PassWord", this.txtFTPPassword.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".RemotePath", this.txtFTPRemotePath.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".OutPutStr", this.txtFTPOutputString.Text);
                LoadPage();
                WebPanel3.Visible = true;
                WebPanel3.Text = MyRow.ToString();

                successMessage.Visible = true;
                lblSuccess.Text = "FTP saved succesfully.";
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "FTP saving failed. Please try again.";
            }


        }

        protected void btnAddHttp_Click(object sender, System.EventArgs e)
        {
            int MyVal = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                for (MyVal = 0; MyVal <= 99; MyVal++)
                {
                    if (string.IsNullOrEmpty(MyRem.LiveMonServer.GetConfigSetting("SMS." + MyVal.ToString() + ".DeviceType")))
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                int MyRow = MyVal;
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".URL", this.txtHttpURL.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".reply", this.txtHttpReplyURL.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".api_id", this.txtHttpAPIId.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".user", this.txtHttpUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".password", this.txtHttpPassword.Text);
                LoadPage();
                WebPanel4.Visible = true;
                WebPanel4.Text = MyRow.ToString();

                successMessage.Visible = true;
                lblSuccess.Text = "Http saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "http saving failed. Please try again.";
            }
        }

        protected void UpdateSMSTimer_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS.Timer", this.SMSTimer.Text);

                successMessage.Visible = true;
                lblSuccess.Text = "SMS timer saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "SMS timer saving failed. Please try again.";
            }


        }

        protected void btnUpdateEmailTimer_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("Email.Timer", this.EmailTimer.Text);

                successMessage.Visible = true;
                lblSuccess.Text = "Email timer saved succesfully.";

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Email timer saving failed. Please try again.";
            }
        }

        protected void btnTestReport_Click(object sender, System.EventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                MyRem.LiveMonServer.RunReportTest();
                // lblerr.Visible = False
            }
            catch (Exception ex)
            {
                //  lblerr.Visible = True
                //   lblerr.Text = ex.Message

            }
        }

        //Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        //    Dim MyRem As New LiveMonitoring.GlobalRemoteVars
        //    Try
        //        MyRem.LiveMonServer.SetConfigSetting("Report1.Name", Me.txtReportName.Text)
        //        MyRem.LiveMonServer.SetConfigSetting("Report1.Scheduling.Hours", Me.txtReportHours.Text)
        //        Dim myint As Integer
        //        Dim MyDays As String = ""
        //        For myint = 0 To 6
        //            If Me.chkDays.Items(myint).Selected Then
        //                MyDays += myint.ToString + ","
        //            End If
        //        Next
        //        MyDays = MyDays.Remove(MyDays.Length - 2, 1)
        //        MyRem.LiveMonServer.SetConfigSetting("Report1.Scheduling.Days", MyDays)
        //        MyRem.LiveMonServer.SetConfigSetting("Report1.Recipients", Me.txtReportRecipients.Text)
        //        lblerr.Visible = False
        //    Catch ex As Exception
        //        lblerr.Visible = True
        //        lblerr.Text = ex.Message
        //    End Try

        //End Sub

        protected void BtnSaveDays_Click(object sender, System.EventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                MyRem.LiveMonServer.SetConfigSetting("History.Config.Video.FileSize", this.txtVideoDays.Text);
                MyRem.LiveMonServer.SetConfigSetting("History.Config.Sensor.Days", this.txtSensorDays.Text);

                successMessage.Visible = true;
                lblSuccess.Text = "Days saved succesfully.";

                // lblerr.Visible = False

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Days saving failed. Please try again.";
                //  lblerr.Visible = True
                //  lblerr.Text = ex.Message
            }

        }


        protected void btnUpdateSMSEmail_Click1(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            int MyRow = Convert.ToInt32(WebPanel2.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".MySMTPserver", this.txtSMTPServerName.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".FromAdd", this.txtSMTPFrom.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".ToAdd", this.txtSMTPTo.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".Subject", this.txtSMTPSubject.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".api_id", this.txtSMTPAPIId.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".user", this.txtSMTPUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".password", this.txtSMTPPassword.Text);
                MyRem.LiveMonServer.SetConfigSetting("SMS." + MyRow.ToString() + ".reply", this.txtSMTPReply.Text);
                WebPanel2.Visible = false;
                LoadPage();
                successMessage.Visible = true;
                lblSuccess.Text = " MMS Email saved sucessfuly.";

                LoadPage();
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "MMS timer saving failed. Please try again.";
            }

        }

        protected void btnUpdateMMSEmail_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("MMS.MyMMSURL", this.txtMMSURL.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.api_id", this.txtMMSSMTPAPIId.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.user", this.txtMMSSMTPUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.password", this.txtMMSSMTPPassword.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.FTPServer", this.txtMMSFTPServer.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.FTPUserName", this.txtMMSFTPUser.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.FTPPassWord", this.txtMMSFTPPassword.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.RemotePath", this.txtMMSFTPRemotePath.Text);
                MyRem.LiveMonServer.SetConfigSetting("MMS.ImageURL", this.txtMMSImageURL.Text);
                WebPanel2.Visible = false;

                successMessage.Visible = true;
                lblSuccess.Text = " MMS Email saved sucessfuly.";

                LoadPage();
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "MMS timer saving failed. Please try again.";
            }
        }

        protected void UpdateMMSTimer_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            try
            {
                MyRem.LiveMonServer.SetConfigSetting("MMS.Timer", this.MMSTimer.Text);

                successMessage.Visible = true;
                lblSuccess.Text = " MMS timer saved sucessfuly.";
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "MMS timer saving failed. Please try again.";
            }
        }

        protected void btnUpdateDialUp_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();


            try
            {
                MyRem.LiveMonServer.SetConfigSetting("DataSend.BackupModem", this.txtDialUp.Text);

                successMessage.Visible = true;
                lblSuccess.Text = "Dialup saved sucessfuly.";
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Dialup saving failed. Please try again.";
            }
        }


        protected void btnUpdatePingServer_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("Ping.Server.Up", this.chkServerPing.Checked.ToString());
                MyRem.LiveMonServer.SetConfigSetting("Ping.Server.Email", this.txtPingEmail.Text);


                successMessage.Visible = true;
                lblSuccess.Text = "Ping server saved sucessfuly.";
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Ping server saving failed. Please try again.";
            }
        }

        //Protected Sub WebNumericEdit1_ValueChange(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebDataInput.ValueChangeEventArgs) Handles txtReport2Hours.ValueChange

        //End Sub

        //Protected Sub BtnSaveRep2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveRep2.Click
        //    Dim MyRem As New LiveMonitoring.GlobalRemoteVars
        //    Try
        //        MyRem.LiveMonServer.SetConfigSetting("Report2.Name", Me.txtReport2Name.Text)
        //        MyRem.LiveMonServer.SetConfigSetting("Report2.Scheduling.Hours", Me.txtReport2Hours.Text)
        //        Dim myint As Integer
        //        Dim MyDays As String = ""
        //        For myint = 0 To 6
        //            If Me.chk2Days.Items(myint).Selected Then
        //                MyDays += myint.ToString + ","
        //            End If
        //        Next
        //        MyDays = MyDays.Remove(MyDays.Length - 2, 1)
        //        MyRem.LiveMonServer.SetConfigSetting("Report2.Scheduling.Days", MyDays)
        //        MyRem.LiveMonServer.SetConfigSetting("Report2.Recipients", Me.txtReport2Recipients.Text)
        //        lblerr.Visible = False
        //    Catch ex As Exception
        //        lblerr.Visible = True
        //        lblerr.Text = ex.Message
        //    End Try
        //End Sub

        //Protected Sub btnSaveRep3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRep3.Click
        //    Dim MyRem As New LiveMonitoring.GlobalRemoteVars
        //    Try
        //        MyRem.LiveMonServer.SetConfigSetting("Report3.Name", Me.txtReport3Name.Text)
        //        MyRem.LiveMonServer.SetConfigSetting("Report3.Scheduling.Hours", Me.txtReport3Hours.Text)
        //        Dim myint As Integer
        //        Dim MyDays As String = ""
        //        For myint = 0 To 6
        //            If Me.chk3Days.Items(myint).Selected Then
        //                MyDays += myint.ToString + ","
        //            End If
        //        Next
        //        MyDays = MyDays.Remove(MyDays.Length - 2, 1)
        //        MyRem.LiveMonServer.SetConfigSetting("Report3.Scheduling.Days", MyDays)
        //        MyRem.LiveMonServer.SetConfigSetting("Report3.Recipients", Me.txtReport3Recipients.Text)
        //        MyRem.LiveMonServer.SetConfigSetting("Report3.DataPeriod", Me.cmbrep3DailySetting.SelectedValue)
        //        MyRem.LiveMonServer.SetConfigSetting("Report3.SensorFields", Me.txtReport3SensorField.Text)
        //        lblerr.Visible = False
        //    Catch ex As Exception
        //        lblerr.Visible = True
        //        lblerr.Text = ex.Message
        //    End Try
        //End Sub

        //Protected Sub TestRep3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TestRep3.Click
        //    Dim MyRem As New LiveMonitoring.GlobalRemoteVars
        //    Try
        //        MyRem.LiveMonServer.RunReportTest()
        //        'lblerr.Visible = False
        //    Catch ex As Exception
        //        'lblerr.Visible = True
        //        ' lblerr.Text = ex.Message

        //    End Try

        //End Sub

        protected void btnIncludeStart_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("Alert.IncludeDateStart", this.chkIncludeStart.Checked.ToString());

                successMessage.Visible = true;
                lblSuccess.Text = "Date start saved sucessfuly.";
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Date start saving failed. Please try again.";
            }

        }

        protected void btnResetServerUpdate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyRem.LiveMonServer.SetConfigSetting("Reset.Timer", this.txtResetServer.Text);

                successMessage.Visible = true;
                lblSuccess.Text = "Reset timer saved sucessfuly.";
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Reset timer saving failed. Please try again.";
            }
        }

        public ServerConfiguration()
        {
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}