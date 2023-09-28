using LiveMonitoring;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class CameraDynamic : System.Web.UI.Page , ICallbackEventHandler
    {
        private LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private string _callbackArg;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsCallback == false)
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


                    ClientScriptManager cm = Page.ClientScript;
                    string cbReference = null;
                    cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                    string callbackScript = null;
                    callbackScript = "function CallServer(arg, context)" + "{" + cbReference + "; }";
                    cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

                    successMessage.Visible = false;
                    warningMessage.Visible = false;
                    errorMessage.Visible = false;

                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");

                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    if (MyIPMonPageSecure > MyUser.UserLevel)
                    {
                        Response.Redirect("NotAuthorisedView.aspx");
                    }
                    if ((Request.UserAgent.ToLower().Contains("konqueror") == false))
                    {
                        if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("gzip")))
                        {
                            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress, true);
                            Response.AppendHeader("Content-encoding", "gzip");

                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("deflate")))
                            {
                                Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress, true);
                                Response.AppendHeader("Content-encoding", "deflate");
                            }
                        }
                    }

                }
                else
                {
                    Response.Redirect("Index.aspx");
                }

            }
        }

        protected void Page_Init(object sender, System.EventArgs e)
        {
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Page.MaintainScrollPositionOnPostBack = true;
            int MyCameraNum = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCameraNum = Convert.ToInt32(Request.QueryString["CameraNum"]);
            Load_Cameras(MyCameraNum);
            LoadImageRefresh();
            if (MyCameraNum == 0)
            {
                //all cameras
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                try
                {
                    if ((MyCollection == null) == false)
                    {
                        if ((MyCollection == null) == false)
                        {
                            foreach (object MyObject1_loopVariable in MyCollection)
                            {
                                MyObject1 = MyObject1_loopVariable;
                                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                                {
                                    AddImages(MyDiv, (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1);
                                    MyDiv += 1;
                                }
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                }

            }
            else
            {
                //specific
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyCnt = 0;
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            if (MyCameraNum == MyCamera.ID)
                            {
                                AddImages(MyCnt + 1, MyCamera);
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
            this.Refresh.Value = cmbRefreshRate.SelectedValue;
        }



      


        public void RaiseCallbackEvent(string eventArgument)
        {
            _callbackArg = eventArgument;
        }
        public string GetCallbackResult()
        {

            string[] MyVars = null;
            MyVars = _callbackArg.Split(',');
            //_callbackArg += "," + ReturnInput(MyVars(0).Trim)
            _callbackArg = ReturnInput(_callbackArg);
            //Select Case MyVars(0).Trim
            // Case "Update"
            // '_callbackArg = "<img id=Backimage src=ReturnModelImage.aspx?Model=" + Me.UltraWebTab1.Tabs(Me.UltraWebTab1.SelectedTab).Tag.ToString + " style='height: 480px; width: 640px;z-index:-1;'></img>"
            // '_callbackArg += ReturnSpecificDevices(Me.UltraWebTab1.Tabs(Me.UltraWebTab1.SelectedTab).Tag)
            // 'send back a string can be split then sent to each camera?? needs some work
            // Return _callbackArg
            // Case Else
            return _callbackArg;
            //End Select

            //End Select

        }


        public string ReturnInput(string InputNumbers)
        {
            string functionReturnValue = null;
            //MsgBox(InputNumber)
            string MyTable = "";
            functionReturnValue = "";
            Collection MyCol = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
           // LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            if (InputNumbers.IndexOf("_") < 0)
            {
                return functionReturnValue;
            }
            string InputNumber = null;
            bool FirstOne = true;
            string[] InputsArray = InputNumbers.Split(',');
            foreach (string InputNumber_loopVariable in InputsArray)
            {
                InputNumber = InputNumber_loopVariable;
                try
                {
                    //split the Inputnumber here
                    //Input" + MyLink.CameraID.ToString + "|" + MySensor.ID.ToString
                    int MyCameraid = Convert.ToInt32(Strings.Mid(InputNumber, InputNumber.IndexOf("Input") + 5 + 1, InputNumber.IndexOf("_") - (InputNumber.IndexOf("Input") + 5)));
                    //????????
                    int MySensorid = Convert.ToInt32(Strings.Mid(InputNumber, InputNumber.IndexOf("_") + 1 + 1, InputNumber.Length - (InputNumber.IndexOf("_") + 1)));
                    //?///???
                    MyCol = MyRem.LiveMonServer.GetSpecificCamSensLink(MyCameraid);
                    foreach (LiveMonitoring.IRemoteLib.CameraSensorLink MyLink in MyCol)
                    {
                        if (MyLink.SensorID == MySensorid)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                            MySensor = MyLink.CurSensor;
                            //MyRem.server1.GetSpecificSensor(MyLink.SensorID)
                            if ((MySensor == null) == false)
                            {
                                try
                                {
                                    //input
                                    ///''
                                   // LiveMonitoring.IRemoteLib.SensorFieldsDef myField = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                                    //"TITLE='"
                                    string MyTitle = "";
                                    foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef myField in MySensor.Fields)
                                    {
                                        MyTitle += myField.FieldName + ":" + myField.LastValue.ToString() + "&#10;";
                                        MyTitle += "Last Read:" + myField.LastDTRead.ToString() + "&#10;";
                                    }
                                    MyTitle += "Last Status:" + MySensor.Status.ToString() + "&#10;";
                                    //MyTable += "<div id='Input" + MyLink.CameraID.ToString + "|" + MySensor.ID.ToString + "' runat='server'>"
                                    //MyTable += "<tr>"
                                    if (FirstOne == false)
                                    {
                                        MyTable += ",";
                                    }
                                    MyTable += InputNumber + ",<img src=ReturnThumbnailImage.aspx?Sensor=" + MySensor.ID.ToString() + " height=15 width=15 TITLE='" + MyTitle + "'>";
                                    MyTable += MySensor.Caption;
                                    // MyTable += "</font></td>"
                                    //MyTable += "</tr>" '</div>"
                                    ///'
                                    //Return MyTable
                                    FirstOne = false;
                                    break; // TODO: might not be correct. Was : Exit For

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
            return MyTable;
            return functionReturnValue;

        }


        public string ReturnSingleInput(string InputNumber)
        {
            string functionReturnValue = null;
            //MsgBox(InputNumber)
            Collection MyCol = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
           // LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            if (InputNumber.IndexOf("_") < 0)
            {
                return functionReturnValue;
            }
            //split the Inputnumber here
            //Input" + MyLink.CameraID.ToString + "|" + MySensor.ID.ToString
            int MyCameraid = Convert.ToInt32(Strings.Mid(InputNumber, InputNumber.IndexOf("Input") + 5 + 1, InputNumber.IndexOf("_") - (InputNumber.IndexOf("Input") + 5)));
            //????????
            int MySensorid = Convert.ToInt32(Strings.Mid(InputNumber, InputNumber.IndexOf("_") + 1 + 1, InputNumber.Length - (InputNumber.IndexOf("_") + 1)));
            //?///???
            MyCol = MyRem.LiveMonServer.GetSpecificCamSensLink(MyCameraid);
            foreach (LiveMonitoring.IRemoteLib.CameraSensorLink MyLink in MyCol)
            {
                if (MyLink.SensorID == MySensorid)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                    MySensor = MyLink.CurSensor;
                    //MyRem.server1.GetSpecificSensor(MyLink.SensorID)
                    if ((MySensor == null) == false)
                    {
                        //input
                        ///''
                       // LiveMonitoring.IRemoteLib.SensorFieldsDef myField = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                        //"TITLE='"
                        string MyTitle = "";
                        string MyTable = "";
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef myField in MySensor.Fields)
                        {
                            MyTitle += myField.FieldName + ":" + myField.LastValue.ToString() + "&#10;";
                            MyTitle += "Last Read:" + myField.LastDTRead.ToString() + "&#10;";
                        }
                        MyTitle += "Last Status:" + MySensor.Status.ToString() + "&#10;";
                        //MyTable += "<div id='Input" + MyLink.CameraID.ToString + "|" + MySensor.ID.ToString + "' runat='server'>"
                        //MyTable += "<tr>"
                        MyTable += "<img src=ReturnThumbnailImage.aspx?Sensor=" + MySensor.ID.ToString() + " height=15 width=15 TITLE='" + MyTitle + "'>";
                        MyTable += MySensor.Caption;
                        // MyTable += "</font></td>"
                        //MyTable += "</tr>" '</div>"

                        ///'
                        return MyTable;
                    }
                }

            }
            return functionReturnValue;
        }


        public void Load_Cameras(int SelectedID)
        {
            //CameraMenu
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            chkCameras.Items.Clear();
            if ((MyCollection == null) == false)
            {
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                    {
                        LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                        //Dim MyWebMenuItem As New Infragistics.WebUI.UltraWebNavigator.Item
                        //For Each MyWebMenuItem In CameraMenu
                        // If MyWebMenuItem.Text = "Select Cameras" Then
                        // Exit For
                        // End If
                        //Next
                        //Dim MyNewWebMenuItem As New Infragistics.WebUI.UltraWebNavigator.Item
                        //MyNewWebMenuItem.Text = MyCamera.Caption
                        //MyNewWebMenuItem.Tag = "Camera"
                        //If SelectedID = 0 Or SelectedID = MyCamera.ID Then
                        // MyNewWebMenuItem.Checked = True
                        //End If
                        //MyNewWebMenuItem.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True
                        //'chkbox list
                        //MyWebMenuItem.Items.Add(MyNewWebMenuItem)
                        System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                        MyItem.Text = MyCamera.Caption;
                        MyItem.Value = MyCamera.ID.ToString();
                        if (SelectedID == 0 | SelectedID == MyCamera.ID)
                        {
                            MyItem.Selected = true;
                        }
                        else
                        {
                            MyItem.Selected = false;
                        }
                        chkCameras.Items.Add(MyItem);
                    }
                }
            }


        }

        public void LoadImageRefresh()
        {
            //Me.cmbImageSize.Items.Clear()

             
            this.cmbRefreshRate.Items.Clear();
            //Dim Size As Integer = 0
            //For Size = 0 To 7
            // Dim Height As Integer = 300
            // Dim Width As Integer = 300
            // Select Case Size
            // Case 0
            // Height = 100
            // Width = 100
            // Case 1
            // Height = 200
            // Width = 200
            // Case 2
            // Height = 100
            // Width = 200
            // Case 3
            // Height = 300
            // Width = 300
            // Case 4
            // Height = 400
            // Width = 400
            // Case 5
            // Height = 500
            // Width = 500
            // Case 6
            // Height = 600
            // Width = 600
            // Case 7
            // Height = 700
            // Width = 700
            // End Select
            // Dim MyItem As New Web.UI.WebControls.ListItem()
            // MyItem.Text = Height.ToString + " X " + Width.ToString
            // MyItem.Value = Size
            // If Size <> 3 Then
            // MyItem.Selected = False
            // Else
            // MyItem.Selected = True
            // End If
            // cmbImageSize.Items.Add(MyItem)
            //Next
            int Myrefresh = 0;
            int Myrate = 0;
            string MyStr = "";
            for (Myrefresh = 0; Myrefresh <= 5; Myrefresh++)
            {
                switch (Myrefresh)
                {
                    case 0:
                        Myrate = 1000;
                        MyStr = "1 Sec";
                        break;
                    case 1:
                        Myrate = 2000;
                        MyStr = "2 Sec";
                        break;
                    case 2:
                        Myrate = 3000;
                        MyStr = "3 Sec";
                        break;
                    case 3:
                        Myrate = 5000;
                        MyStr = "5 Sec";
                        break;
                    case 4:
                        Myrate = 10000;
                        MyStr = "10 Sec";
                        break;
                    case 5:
                        Myrate = 20000;
                        MyStr = "20 Sec";
                        break;
                }
                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                MyItem.Text = MyStr;
                MyItem.Value = Myrate.ToString();
                if (Myrefresh != 2)
                {
                    MyItem.Selected = false;
                }
                else
                {
                    MyItem.Selected = true;
                }
                cmbRefreshRate.Items.Add(MyItem);
            }
        }


        public void AddImages(int DivID, LiveMonitoring.IRemoteLib.CameraDetails Camera)
        {
            //Dim myImage As New System.Web.UI.WebControls.Image
            //myImage.ID = "Image" & DivID.ToString
            //myImage.Height = ImageHeight
            //myImage.Width = ImageWidth
            int CameraId = Camera.ID;
            System.Web.UI.HtmlControls.HtmlGenericControl myDiv1 = (System.Web.UI.HtmlControls.HtmlGenericControl)tbl1.FindControl("Div" + DivID.ToString());
            //If IsNothing(Me.tbl1.FindControl("Image" & DivID.ToString)) = True Then
            // myDiv1.Controls.Add(myImage)
            //End If
            System.Web.UI.HtmlControls.HtmlInputHidden MyTxtBox = (System.Web.UI.HtmlControls.HtmlInputHidden)tbl1.FindControl("Hidden" + DivID.ToString());
            if ((MyTxtBox == null) == false)
            {
                MyTxtBox.Value = CameraId.ToString();
            }
            //userr control test
            System.Web.UI.Control oCtrlDemo = new System.Web.UI.Control();
            oCtrlDemo = LoadControl("CameraControl.ascx");
            // Set the Usercontrol Type 
            oCtrlDemo.ID = "MyID" + DivID.ToString();
            Type ucType = oCtrlDemo.GetType();
            // Get access to the property 
            PropertyInfo ucsetID = ucType.GetProperty("setID");
            // Set the property 
            ucsetID.SetValue(oCtrlDemo, DivID, null);
            myDiv1.Controls.Add(oCtrlDemo);
            //oCtrlDemo.setID()
            AddControls(DivID, Camera,oCtrlDemo);
        }



        public void AddControls(int DivID, LiveMonitoring.IRemoteLib.CameraDetails Camera, System.Web.UI.Control oCtrlDemo)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl myDiv1 = (System.Web.UI.HtmlControls.HtmlGenericControl)tbl1.FindControl("Div" + DivID.ToString());
            Type ucType = oCtrlDemo.GetType();
            // Get access to the property 
            PropertyInfo ucsetURL = ucType.GetProperty("setConfigURL");
            //"http://" +
            ucsetURL.SetValue(oCtrlDemo, "http://" + Camera.User + ":" + Camera.Password + "@" + Camera.IPAdress + "/setup/config.html", null);
            PropertyInfo ucsetProxyURL = ucType.GetProperty("setProxyConfigURL");
            ucsetProxyURL.SetValue(oCtrlDemo, "proxy.aspx?url=" + Camera.User + ":" + Camera.Password + "@" + Camera.IPAdress + "/setup/config.html", null);
            PropertyInfo ucsetCapURL = ucType.GetProperty("setCaptureURL");
            ucsetCapURL.SetValue(oCtrlDemo, "CaptureImage.aspx?Camera=" + Camera.ID.ToString(), null);
            PropertyInfo ucsetCamSensURL = ucType.GetProperty("setConfigCameraLink");
            ucsetCamSensURL.SetValue(oCtrlDemo, "CamSensLink.aspx?Camera=" + Camera.ID.ToString(), null);
            //fill control Pannels

            //Dim myDiv2 As System.Web.UI.HtmlControls.HtmlGenericControl = Me.tbl1.FindControl("CntrlPan" & DivID.ToString)
            Collection MyCol = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            MyCol = MyRem.LiveMonServer.GetSpecificCamSensLink(Camera.ID);
            foreach (LiveMonitoring.IRemoteLib.CameraSensorLink MyLink in MyCol)
            {
                LiveMonitoring.IRemoteLib.SensorDetails MySensor = MyLink.CurSensor;
                //MySensor() 'MyRem.server1.GetSpecificSensor(MyLink.SensorID)
                if ((MySensor == null) == false)
                {
                    bool IsInput = true;
                    //only change if its an output
                    switch (MySensor.Type)
                    {
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.AMF:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.BaseAudio:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraAudio:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDInput:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraDOutput:
                            IsInput = false;
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.CameraMotion:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18B20:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS18S20:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_DS2406:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogOutput:
                            IsInput = false;
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.EDS_AnalogProbe:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.HMP2001:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ICMPPoint:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ISPoint:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SNMPPoint:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.SQLPoint:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAInput:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusAOutput:
                            IsInput = false;
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusCounter:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDInput:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDiscrete:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusDOutput:
                            IsInput = false;
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusFloat:
                            break;
                        case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.TCPMbusRTD:
                            break;
                        default:
                            //?? input assumed
                            break;
                    }
                    if (IsInput == false)
                    {
                        //output
                        System.Web.UI.WebControls.Button myButt2 = new System.Web.UI.WebControls.Button();
                        myButt2.CausesValidation = true;
                        //myButt1.CssClass = "btnstandard"
                        myButt2.ID = "Output" + MySensor.ID.ToString();
                        myButt2.Text = MySensor.Caption + ":" + MySensor.LastValue.ToString();
                        //myDiv2.Controls.Add(myButt2)
                        PropertyInfo ucsetOutputs = ucType.GetProperty("setAddIOOutput");
                        ucsetOutputs.SetValue(oCtrlDemo, myButt2, null);
                        myButt2.Click += this.ButtOutputClick;
                    }
                    else
                    {
                        //input
                        ///''
                       // LiveMonitoring.IRemoteLib.SensorFieldsDef myField = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                        //"TITLE='"
                        string MyTitle = "";
                        string MyTable = "";
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef myField in MySensor.Fields)
                        {
                            MyTitle += myField.FieldName + ":" + myField.LastValue.ToString() + "&#10;";
                            MyTitle += "LFR:" + myField.LastDTRead.ToString() + "&#10;";
                        }
                        if (!string.IsNullOrEmpty(this.Inputs.Value))
                        {
                            this.Inputs.Value += ",";
                        }
                        this.Inputs.Value += "Input" + MyLink.CameraID.ToString() + "_" + MySensor.ID.ToString();
                        MyTitle += "LS:" + MySensor.Status.ToString();
                        //MyTable += "<table>"
                        MyTable += "<div id='Input" + MyLink.CameraID.ToString() + "_" + MySensor.ID.ToString() + "' runat='server'>";
                        //MyTable += "<tr>"
                        MyTable += "<img src=ReturnThumbnailImage.aspx?Sensor=" + MySensor.ID.ToString() + " height=11 width=11 TITLE='" + MyTitle + "'>";
                        MyTable += MySensor.Caption;
                        //MyTable += "</font></td>"
                        MyTable += "</div>";
                        //MyTable += "</table>"

                        ///'
                        System.Web.UI.HtmlControls.HtmlGenericControl myButt1 = new System.Web.UI.HtmlControls.HtmlGenericControl();
                        myButt1.InnerHtml = MyTable;
                        PropertyInfo ucsetInputs = ucType.GetProperty("setAddIOInput");
                        ucsetInputs.SetValue(oCtrlDemo, myButt1, null);
                    }
                }
            }


        }


        public void ButtClick(object sender, System.EventArgs e)
        {
            Interaction.MsgBox("In Here");
        }
        public void ButtOutputClick(object sender, System.EventArgs e)
        {
            //MsgBox("In ZOOm Zoom Here")
            System.Web.UI.WebControls.Button Mysender = (System.Web.UI.WebControls.Button)sender;
            int MySensorid = 0;
            MySensorid = Convert.ToInt32(Strings.Mid(Mysender.ClientID, Mysender.ClientID.IndexOf("Output") + 7));
            //trigger sensoroutput???
            //write coils modbus /opc /other??
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyRem.LiveMonServer.TriggerSensor(MySensorid);

        }


        public void RemoveImages(int DivID)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl myDiv1 = (System.Web.UI.HtmlControls.HtmlGenericControl)tbl1.FindControl("Div" + DivID.ToString());
            if ((myDiv1 == null) == false)
            {
                myDiv1.Controls.Clear();
            }
            System.Web.UI.HtmlControls.HtmlInputHidden MyTxtBox = (System.Web.UI.HtmlControls.HtmlInputHidden)tbl1.FindControl("Hidden" + DivID.ToString());
            if ((MyTxtBox == null) == false)
            {
                MyTxtBox.Value = "0";
            }

        }
        public void SetImageSizes(int size)
        {
            //myImage.ID = "Image" & DivID.ToString
            Session["ImageSizes"] = size;

            int Height = 200;
            int Width = 200;
            switch (size)
            {
                case 0:
                    Height = 100;
                    Width = 100;
                    break;
                case 1:
                    Height = 200;
                    Width = 200;
                    break;
                case 2:
                    Height = 100;
                    Width = 200;
                    break;
                case 3:
                    Height = 300;
                    Width = 300;
                    break;
                case 4:
                    Height = 400;
                    Width = 400;
                    break;
                case 5:
                    Height = 500;
                    Width = 500;
                    break;
                case 6:
                    Height = 600;
                    Width = 600;
                    break;
                case 7:
                    Height = 700;
                    Width = 700;
                    break;
            }
            int Acnt = 0;
            for (Acnt = 1; Acnt <= 30; Acnt++)
            {
                System.Web.UI.WebControls.Image myImage = (System.Web.UI.WebControls.Image)tbl1.FindControl("Image" + Acnt.ToString());
                if ((myImage == null) == false)
                {
                    myImage.Height = Height;
                    myImage.Width = Width;
                }
            }
        }


      
        public CameraDynamic()
        {
            Load += Page_Load;
            Init += Page_Init;
        }

        protected void cmbCurrentSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SelectedSite"] = cmbCurrentSite.SelectedValue;
        }


          public class MySite
        {
            public int siteID { get; set; }
            public string siteName { get; set; }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            int aCnt = 0;
            int i = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            for (i = 0; i <= this.chkCameras.Items.Count - 1; i++)
            {
                if (this.chkCameras.Items[i].Selected)
                {
                    Collection MyCollection = new Collection();
                    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                    //GetServerObjects 'server1.GetAll()
                    object MyObject1 = null;
                    int MyCnt = 0;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            if (MyCamera.ID == Convert.ToInt32(chkCameras.Items[i].Value))
                            {
                                RemoveImages(i + 1);
                                AddImages(i + 1, MyCamera);
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                }
                else
                {
                    //remove from cameras
                    Collection MyCollection = new Collection();
                    MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                    //GetServerObjects 'server1.GetAll()
                    object MyObject1 = null;
                    int MyCnt = 0;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            if (MyCamera.ID == Convert.ToInt32(chkCameras.Items[i].Value))
                            {
                                RemoveImages(i + 1);
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                }
            }
            //SetImageSizes(cmbImageSize.SelectedValue)
           
            this.Refresh.Value = cmbRefreshRate.SelectedValue;
        }

        protected void cmbRefreshRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
