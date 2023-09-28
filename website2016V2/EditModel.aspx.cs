using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class EditModel : System.Web.UI.Page , ICallbackEventHandler
    {
        private string _callbackArg;
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
               // LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                ClientScriptManager cm = Page.ClientScript;
                string cbReference = null;
                cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                string callbackScript = null;
                callbackScript = "function CallServer(arg, context)" + "{" + cbReference + "; }";
                cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                if (IsCallback == false)
                {

                    if (IsPostBack)
                    {
                    }
                    else
                    {
                        //fill table with devices/sensors remember hidden values
                        LoadMaps();
                        LoadDevices();
                    }
                }
                else
                {
                   
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

        }



        public void LoadMaps()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyCollection = MyRem.LiveMonServer.GetAllModels();

            }
            catch (Exception ex)
            {
            }

            //LiveMonitoring.IRemoteLib.ModelDetails MyObject1 = default(LiveMonitoring.IRemoteLib.ModelDetails);
            int myCnt = 0;
            for (myCnt = 1; myCnt <= UltraWebTab1.Tabs.Count - 1; myCnt++)
            {
                try
                {
                    UltraWebTab1.Tabs.RemoveAt(1);

                }
                catch (Exception ex)
                {
                }

            }
            //UltraWebTab1.Tabs.Clear()
            //Dim MyTab As New Infragistics.WebUI.UltraWebTab.Tab
            //MyTab.Text = "Add New"
            //UltraWebTab1.Tabs.Add(MyTab)
            foreach (LiveMonitoring.IRemoteLib.ModelDetails MyObject1 in MyCollection)
            {
                try
                {
                    Infragistics.WebUI.UltraWebTab.Tab MynewTab = new Infragistics.WebUI.UltraWebTab.Tab();
                    MynewTab.Text = MyObject1.LayerName;
                    MynewTab.Key = MyObject1.ID.ToString();
                    MynewTab.Tag = MyObject1.ID;
                    UltraWebTab1.Tabs.Add(MynewTab);

                }
                catch (Exception ex)
                {
                }

            }
        }
        public void LoadDevices()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;
            StringBuilder MyStringBuilder = new StringBuilder();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                try
                {
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        //Me.Tophandle.InnerHtml += "<div id='Object" + Mysensor.ID.ToString + "' "
                        MyStringBuilder.Append("<div id='Object" + Mysensor.ID.ToString() + "' ");
                        MyStringBuilder.Append(" TITLE='" + LoadSensorTitle(Mysensor.ID) + "' style='border: solid 1px black;height: 20px; width: 22px;");
                        MyStringBuilder.Append("overflow: auto;background-color: transparent; background-image: url(ReturnnormalImage.aspx?Sensor=" + Mysensor.ID.ToString() + ");");
                        MyStringBuilder.Append(" background-repeat: no-repeat;' onmousedown='fnonmousedown();' ondragstart='fnGetSource();' ondragover='cancelevent();'>");
                        MyStringBuilder.Append("<input type='hidden' id='ID" + Mysensor.ID.ToString() + "' value='Sensor'>");
                        MyStringBuilder.Append("</div>");
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                    {
                        LiveMonitoring.IRemoteLib.CameraDetails Mysensor = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                        MyStringBuilder.Append("<div id='Object" + Mysensor.ID.ToString() + "' ");
                        MyStringBuilder.Append(" TITLE='" + LoadDeviceTitle(Mysensor.ID) + "' style='border: solid 1px black;height: 20px; width: 22px;");
                        MyStringBuilder.Append("overflow: auto;background-color: transparent; background-image: url(ReturnnormalImage.aspx?Device=" + Mysensor.ID.ToString() + ");");
                        MyStringBuilder.Append(" background-repeat: no-repeat;' onmousedown='fnonmousedown();' ondragstart='fnGetSource();' ondragover='cancelevent();'>");
                        MyStringBuilder.Append("<input type='hidden' id='ID" + Mysensor.ID.ToString() + "' value='Device'>");
                        MyStringBuilder.Append("</div>");
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.IPDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                        MyStringBuilder.Append("<div id='Object" + Mysensor.ID.ToString() + "' ");
                        MyStringBuilder.Append(" TITLE='" + LoadDeviceTitle(Mysensor.ID) + "' style='border: solid 1px black;height: 20px; width: 22px;");
                        MyStringBuilder.Append("overflow: auto;background-color: transparent; background-image: url(ReturnnormalImage.aspx?Device=" + Mysensor.ID.ToString() + ");");
                        MyStringBuilder.Append(" background-repeat: no-repeat;' onmousedown='fnonmousedown();' ondragstart='fnGetSource();' ondragover='cancelevent();'>");
                        MyStringBuilder.Append("<input type='hidden' id='ID" + Mysensor.ID.ToString() + "' value='Device'>");
                        MyStringBuilder.Append("</div>");
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.OtherDevicesDetails Mysensor = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                        MyStringBuilder.Append("<div id='Object" + Mysensor.ID.ToString() + "' ");
                        MyStringBuilder.Append(" TITLE='" + LoadDeviceTitle(Mysensor.ID) + "' style='border: solid 1px black;height: 20px; width: 22px;");
                        MyStringBuilder.Append("overflow: auto;background-color: transparent; background-image: url(ReturnnormalImage.aspx?Device=" + Mysensor.ID.ToString() + ");");
                        MyStringBuilder.Append(" background-repeat: no-repeat;' onmousedown='fnonmousedown();' ondragstart='fnGetSource();' ondragover='cancelevent();'>");
                        MyStringBuilder.Append("<input type='hidden' id='ID" + Mysensor.ID.ToString() + "' value='Device'>");
                        MyStringBuilder.Append("</div>");
                    }

                }
                catch (Exception ex)
                {
                }

            }
            this.Tophandle.InnerHtml = MyStringBuilder.ToString();
            //Dim MyCollection As New Collection
            //MyCollection = server1.GetAllModels
            //Dim MyObject1 As LiveMonitoring.IRemoteLib.ModelDetails
            //For Each MyObject1 In MyCollection
            //Next

        }



        public void RaiseCallbackEvent(string eventArgument)
        {
            _callbackArg = eventArgument;
        }
        public string GetCallbackResult()
        {

            string[] MyVars = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyVars = _callbackArg.Split(',');
            switch (MyVars[0].Trim())
            {
                case "Added":
                    //add to db
                    ////CallServer('Added ,ID:'+srcIdcnt +',x:'+event.x +',y:'+ event.y+',image:'+ srcDiv.style.backgroundImage+','+src,'Add');
                    LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails MyObject1 = new LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails();
                    if (MyVars[5].IndexOf("Sensor") > 0)
                    {
                        MyObject1.SensorID = Convert.ToInt32(MyVars[1]);
                    }
                    else
                    {
                        MyObject1.DeviceID = Convert.ToInt32(MyVars[1]);
                    }
                    MyObject1.PositionX = Convert.ToInt32(MyVars[2]);
                    MyObject1.PositionY = Convert.ToInt32(MyVars[3]);
                    MyObject1.ModelID = Convert.ToInt32(this.UltraWebTab1.Tabs[this.UltraWebTab1.SelectedTab].Tag);
                    int MyID = MyRem.LiveMonServer.AddNewModelLocation(MyObject1);
                    //set ID
                    MyVars[1] = MyID.ToString();
                    _callbackArg = string.Join(",", MyVars);
                    return _callbackArg;
                case "Removed":
                    //delete this ID
                    //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                    //Dim Myfunc As New LiveMonitoring.SharedFuncs

                    MyRem.LiveMonServer.DeleteModelLocation(Convert.ToInt32(MyVars[1]));
                    return _callbackArg;
                case "Changed":
                    //edit this ID
                    ////CallServer('Changed ,ID:'+srcIdcnt +',x:'+event.x +',y:'+ event.y,'Change');
                    LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails MyObject2 = new LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails();
                    MyObject2.ID = Convert.ToInt32(MyVars[1]);
                    MyObject2.PositionX = Convert.ToInt32(MyVars[2]);
                    MyObject2.PositionY = Convert.ToInt32(MyVars[3]);
                    MyObject2.ModelID = Convert.ToInt32(this.UltraWebTab1.Tabs[this.UltraWebTab1.SelectedTab].Tag);
                    int MyID1 = Convert.ToInt32(MyRem.LiveMonServer.EditModelLocation(MyObject2));
                    return _callbackArg;
                default:
                    return _callbackArg;
            }

            //End Select

        }


        public void LoadSpecificDevices(int ModelNo)
        {
            try
            {
                Collection MyCollection = new Collection();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.LiveMonServer.GetSpecificModelDeviceLocations(ModelNo);
                //LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails MyObject1 = default(LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails);
                foreach (LiveMonitoring.IRemoteLib.ModelDeviceLocationDetails MyObject1 in MyCollection)
                {
                    try
                    {
                        this.Bottom.InnerHtml += "<div id='Image" + MyObject1.ID.ToString() + "' ";
                        if (Information.IsDBNull(MyObject1.DeviceID) == true | MyObject1.DeviceID == 0)
                        {
                            this.Bottom.InnerHtml += " TITLE='" + LoadSensorTitle(MyObject1.DeviceID) + "' style='border: solid 1px black;height: 20px; width: 22px;";
                            this.Bottom.InnerHtml += "overflow: auto;background-color: transparent; background-image: url(ReturnnormalImage.aspx?Sensor=" + MyObject1.SensorID.ToString() + ");";
                        }
                        else
                        {
                            this.Bottom.InnerHtml += " TITLE='" + LoadDeviceTitle(MyObject1.DeviceID) + "' style='border: solid 1px black;height: 20px; width: 22px;";
                            this.Bottom.InnerHtml += "overflow: auto;background-color: transparent; background-image: url(ReturnnormalImage.aspx?Device=" + MyObject1.DeviceID.ToString() + ");";
                        }
                        this.Bottom.InnerHtml += "POSITION:absolute;z-index:5;";
                        this.Bottom.InnerHtml += "left:" + MyObject1.PositionX.ToString() + "px;top:" + MyObject1.PositionY.ToString() + "px;";
                        this.Bottom.InnerHtml += " background-repeat: no-repeat;' onmousedown='fnonmousedown();' ondragstart='fnGetSource();' ondragover='cancelevent();'>";
                        if (Information.IsDBNull(MyObject1.DeviceID) == true)
                        {
                            this.Bottom.InnerHtml += "<input type='hidden' id='ID" + MyObject1.SensorID.ToString() + "' value='Sensor'>";
                        }
                        else
                        {
                            this.Bottom.InnerHtml += "<input type='hidden' id='ID" + MyObject1.DeviceID.ToString() + "' value='Device'>";
                        }
                        this.Bottom.InnerHtml += "</div>";

                    }
                    catch (Exception ex)
                    {
                    }

                }

            }
            catch (Exception ex)
            {
            }
        }



        public string LoadDeviceTitle(int DeviceID)
        {
            string functionReturnValue = null;
            Collection MyCollection = new Collection();
            functionReturnValue = "";
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                try
                {
                    if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                    {
                        LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                        if (MyCamera.ID == DeviceID)
                        {
                            functionReturnValue += MyCamera.Caption + "&#10;";
                            functionReturnValue += "LR:" + MyCamera.DTLastRead.ToString() + "&#10;";
                            functionReturnValue += "LMD:" + MyCamera.LastMotionDate.ToString() + "&#10;";
                            functionReturnValue += "LS:" + MyCamera.Status.ToString();
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevices = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                        if (MyIPDevices.ID == DeviceID)
                        {
                            functionReturnValue += MyIPDevices.Caption + "&#10;";
                            functionReturnValue += "LR:" + MyIPDevices.DTLastRead.ToString() + "&#10;";
                            functionReturnValue += "Type:" + MyIPDevices.Type.ToString() + "&#10;";
                            functionReturnValue += "LS:" + MyIPDevices.Status.ToString();
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                    {
                        LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevice = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                        if (MyOtherDevice.ID == DeviceID)
                        {
                            functionReturnValue += MyOtherDevice.Caption + "&#10;";
                            functionReturnValue += "LR:" + MyOtherDevice.LastReadDT.ToString() + "&#10;";
                            functionReturnValue += "Type:" + MyOtherDevice.Type.ToString() + "&#10;";
                            functionReturnValue += "LS:" + MyOtherDevice.Status.ToString();
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }

                }
                catch (Exception ex)
                {
                }

            }
            return functionReturnValue;
        }

        public string LoadSensorTitle(int SensorID)
        {
            string functionReturnValue = null;
            Collection MyCollection = new Collection();
            functionReturnValue = "";
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                try
                {
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (MySensor.ID == SensorID)
                        {
                            functionReturnValue += MySensor.Caption + "&#10;";
                            //MyField = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyField in MySensor.Fields)
                            {
                                functionReturnValue += MyField.FieldName + ":" + MyField.LastValue.ToString() + "&#10;";
                                functionReturnValue += "LFR:" + MyField.LastDTRead.ToString() + "&#10;";
                            }
                            functionReturnValue += "LS:" + MySensor.Status.ToString();
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }

                }
                catch (Exception ex)
                {
                }

            }
            return functionReturnValue;
        }



        protected void GridDisplayGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {

        }

        protected void UltraWebTab1_TabClick(object sender, Infragistics.WebUI.UltraWebTab.WebTabEvent e)
        {
            //now load this model image to bottom div
            if ((e.Tab.Tag == null) == false)
            {
                //Me.Bottom.Style.Item("background-image") = "url(ReturnModelImage.aspx?Model=" + e.Tab.Tag.ToString + ") 100%"
                //Me.Backimage.Src = "ReturnModelImage.aspx?Model=" + e.Tab.Tag.ToString
                //Me.Bottom.Style.Item("background-size") = "100%"
                //: 100%;
                this.Bottom.InnerHtml = "<img id=Backimage src=ReturnModelImage.aspx?Model=" + e.Tab.Tag.ToString() + " style='height: 100%; width: 100%;z-index:-1;'></img>";
                this.ModelEdit.Visible = true;
                //load specific models
                LoadSpecificDevices(Convert.ToInt32(e.Tab.Tag));

            }
            else
            {
                this.ModelEdit.Visible = false;
            }
        }

        protected void cmdLoadNewModel_Click(object sender, EventArgs e)
        {
            LiveMonitoring.IRemoteLib.ModelDetails Mymodel = new LiveMonitoring.IRemoteLib.ModelDetails();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();

            Mymodel.ModelImage = Myfunc.Strip_Image(this.FileUpload);

            if ((Mymodel.ModelImage == null))
            {
                errorMessage.Visible = true;
                lblError.Text = "Image not found. Please try again.";

                return;

            }

            Mymodel.ModelImageByte = MyRem.ImagetoByte(Mymodel.ModelImage, ImageFormat.Bmp);

            if (txtModelName.Text.Length < 1)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply the Model name.";

                return;
            }
            else
            {
                Mymodel.LayerName = txtModelName.Text;

            }


            if (MyRem.LiveMonServer.AddNewModel(Mymodel))
            {
                successMessage.Visible = true;
                lblSucces.Text = "Load Model Succeeded.";

                LoadMaps();

            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "Load Model failed.";
            }
        }

        protected void cmdEditModel_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            LiveMonitoring.IRemoteLib.ModelDetails Mymodel = new LiveMonitoring.IRemoteLib.ModelDetails();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();

            Mymodel.ModelImage = Myfunc.Strip_Image(this.FileUploadEdit);
            Mymodel.ModelImageByte = MyRem.ImagetoByte(Mymodel.ModelImage, ImageFormat.Bmp);
            Mymodel.LayerName = this.UltraWebTab1.Tabs[this.UltraWebTab1.SelectedTab].Text;
            Mymodel.ID = Convert.ToInt32(this.UltraWebTab1.Tabs[this.UltraWebTab1.SelectedTab].Tag);

            if (MyRem.LiveMonServer.EditModel(Mymodel))
            {
                try
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "EditModel Succeeded.";

                    MyRem.WriteLog("EditModel Succeeded", "User:" + MyUser.ID.ToString() + "|" + Mymodel.ToString());

                }
                catch (Exception ex)
                {
                }
                LoadMaps();
            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "Edit Model failed.";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
    }
}
