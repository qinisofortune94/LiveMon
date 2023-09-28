using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.IO.Compression;
using Infragistics;
using Microsoft.VisualBasic.Devices;

namespace website2016V2
{
    public partial class StatusByType : System.Web.UI.Page, ICallbackEventHandler
    {
        private string _callbackArg;
        public string MyTable;
        public string MyCMD = "";
        private Collection MyCollection = new Collection();
        private Collection MyCameraCollection = new Collection();
        private Collection MySensorCollection = new Collection();
        private Collection MySensorGroupCollection = new Collection();
        private Collection MyIPDevicesCollection = new Collection();
        private Collection MyOtherDevicesCollection = new Collection();
        private Collection MySNMPDevicesCollection = new Collection();
        private LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

        public DateTime LastRefresh;

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

                
            }
            else
            {
                Response.Redirect("Index.aspx");
            }

            if ((Request.UserAgent.ToLower().Contains("konqueror") == false))
            {
                
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

                this.Response.Expires = 1;

                if (IsCallback == false)
                {
                    ClientScriptManager cm = Page.ClientScript;
                    string cbReference = null;
                    cbReference = cm.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                    string callbackScript = null;
                    callbackScript = "function CallServer(arg, context)" + "{" + cbReference + "; }";
                    cm.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

                    if (IsPostBack == false)
                        Fill_Grid();
                    LastRefresh = DateAndTime.Now;
                }
            }
            #region "Roger mini tree"
            //'roger mini tree start


            /// <summary>
            /// Add the different sensors to the different sections in
            /// the nodes of the treeview.
            /// </summary>
            /// <remarks></remarks>

        }

        public void FillSites()
        {
            //Dim MySQLreader As SqlDataReader
            //Dim MySQLParam(0) As SqlClient.SqlParameter
            //MySQLParam(0) = New SqlClient.SqlParameter
            //MySQLParam(0).ParameterName = "@UserID"
            //MySQLParam(0).Value = UserID
            //MySQLreader = ExecCmdQueryParams("GetUniqueDevicesByUser", MySQLParam)
            //Dim MySensorCollection As New Collection
            //Dim MySensorGroupCollection As New Collection
            //Dim MyIPDevicesCollection As New Collection
            //Dim MyOtherDevicesCollection As New Collection
            //Dim MySNMPDevicesCollection As New Collection
            ClearSite();
            Collection addedGroups = new Collection();

            //Check each sensor groupd add them to the correct location.

            foreach (object MySensorGroupObj_loopVariable in MySensorGroupCollection)
            {
               object MySensorGroupObj = MySensorGroupObj_loopVariable;
                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MySensorGroupObj;
                //Does the sensor group already exist in the addedGroups collection.

                if (addedGroups.Contains(MySensorGroup.SensorGroupID.ToString()) == false)
                {
                    AddSite(MySensorGroup.SensorGroupName, MySensorGroup.SensorGroupID.ToString());
                    AddAlertingSite(MySensorGroup.SensorGroupName, MySensorGroup.SensorGroupID.ToString());
                    addedGroups.Add(MySensorGroup, MySensorGroup.SensorGroupID.ToString());

                }

            }

            if (MySensorCollection.Count > 0)
            {
                //Dim MySite As String = ""

                // For each sensor found, add it to the sensor section.
                foreach (object MySensorObj_loopVariable in MySensorCollection)
                {
                   object MySensorObj = MySensorObj_loopVariable;
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MySensorObj;
                    //Add to 'All' section and addedGroups collection.
                    if (addedGroups.Contains(MySensor.SensGroup.SensorGroupID.ToString()) == false)
                    {
                        AddSite(MySensor.SensGroup.SensorGroupName, MySensor.SensGroup.SensorGroupID.ToString());
                        addedGroups.Add(MySensor.SensGroup, MySensor.SensGroup.SensorGroupID.ToString());
                    }

                    //Add to 'Alert' section.
                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.alert)
                    {
                        AddAlertingSiteDevice(MySensor.SensGroup.SensorGroupID.ToString(), MySensor.Caption, MySensor.ID.ToString(), true);
                        AddSiteDevice(MySensor.SensGroup.SensorGroupID.ToString(), MySensor.Caption, MySensor.ID.ToString(), true);
                    }
                    else
                    {
                        AddSiteDevice(MySensor.SensGroup.SensorGroupID.ToString(), MySensor.Caption, MySensor.ID.ToString(), false);
                    }
                    // Add to location.
                    if (MySensor.SiteID != 0)
                    {
                        // Add to Status section.
                        LiveMonitoring.Sites.Site mysite = new LiveMonitoring.Sites.Site(MySensor.SiteID);
                        AddToLocationSection(mysite.SiteObj.SiteName, mysite.SiteObj.SiteName, MySensor.Caption, MySensor.ID.ToString());
                    }
                    // Add to the status section.
                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.ok)
                    {
                        addToStatusSection("Ok", MySensor.Caption, MySensor.ID.ToString());
                    }
                    else if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.noresponse)
                    {
                        addToStatusSection("No Response", MySensor.Caption, MySensor.ID.ToString());
                    }
                }
            }
            // MySQLreader.Close()
            //MySQLreader = Nothing
        }

        /// <summary>
        /// Clear all the nodes.
        /// </summary>
        /// <remarks></remarks>
        public void ClearSite()
        {
            //TODO: Clear the locations nodes.
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = ReturnRoot();
            MyNodeRoot.Nodes.Clear();
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeAlert = ReturnAlertsRoot();
            MyNodeAlert.Nodes.Clear();
        }

        /// <summary>
        /// Add the site node
        /// </summary>
        /// <param name="NodeText"></param>
        /// <param name="NodeTag"></param>
        /// <remarks></remarks>
        public void AddSite(string NodeText, string NodeTag)
        {
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = ReturnRoot();
            Infragistics.WebUI.UltraWebNavigator.Node MyNode = MyNodeRoot.Nodes.Add(NodeText, NodeTag);
            MyNode.ShowExpand = true;
            MyNode.ImageUrl = "~/images/performance.ico";
            MyNode.SelectedImageUrl = "~/images/performance.ico";
        }

        /// <summary>
        /// Add the alerting node
        /// </summary>
        /// <param name="NodeText"></param>
        /// <param name="NodeTag"></param>
        /// <remarks></remarks>
        public void AddAlertingSite(string NodeText, string NodeTag)
        {
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = ReturnAlertsRoot();
            Infragistics.WebUI.UltraWebNavigator.Node MyNode = MyNodeRoot.Nodes.Add(NodeText, NodeTag);
            MyNode.ShowExpand = true;
            MyNode.ImageUrl = "~/images/performance.ico";
            MyNode.SelectedImageUrl = "~/images/performance.ico";
        }




        /// <summary>
        /// Add a device to the alerting site node.
        /// </summary>
        /// <param name="SiteTag">Node name</param>
        /// <param name="DeviceText">Device name</param>
        /// <param name="DeviceTag">Device tag</param>
        /// <param name="InAlert">optional boolean. Is the device in an alert status?</param>
        /// <remarks></remarks>
        public void AddAlertingSiteDevice(string SiteTag, string DeviceText, string DeviceTag, bool InAlert = false)
        {
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = ReturnAlertsRoot();
           // Infragistics.WebUI.UltraWebNavigator.Node MyNode = default(Infragistics.WebUI.UltraWebNavigator.Node);
            foreach (Infragistics.WebUI.UltraWebNavigator.Node MyNode in MyNodeRoot.Nodes)
            {
                if (MyNode.Tag == SiteTag)
                {
                    Infragistics.WebUI.UltraWebNavigator.Node MyNodeAdd = MyNode.Nodes.Add(DeviceText, DeviceTag);
                    if (InAlert)
                    {
                        MyNodeAdd.SelectedImageUrl = "~/images/eventlog.ico";
                        MyNodeAdd.ImageUrl = "~/images/eventlog.ico";
                        // MyNodeAdd.TargetFrame = "main"
                        //MyNodeAdd.TargetUrl = "Device.aspx?Device=" & DeviceTag

                        MyNode.ImageUrl = "~/images/eventlogError.ico";
                        MyNode.SelectedImageUrl = "~/images/eventlogError.ico";
                    }
                    else
                    {
                        MyNodeAdd.SelectedImageUrl = "~/images/performance.ico";
                        MyNodeAdd.ImageUrl = "~/images/performance.ico";
                        //MyNodeAdd.TargetFrame = "main"
                        //MyNodeAdd.TargetUrl = "Device.aspx?Device=" & DeviceTag
                    }
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

        }

        /// <summary>
        /// Add a device to the site section
        /// </summary>
        /// <param name="SiteTag">Site tag name</param>
        /// <param name="DeviceText">Device name</param>
        /// <param name="DeviceTag">Device tag</param>
        /// <param name="InAlert">Is the device in an alert status?</param>
        /// <remarks></remarks>
        public void AddSiteDevice(string SiteTag, string DeviceText, string DeviceTag, bool InAlert = false)
        {
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = ReturnRoot();
            //Infragistics.WebUI.UltraWebNavigator.Node MyNode = default(Infragistics.WebUI.UltraWebNavigator.Node);
            foreach (Infragistics.WebUI.UltraWebNavigator.Node MyNode in MyNodeRoot.Nodes)
            {
                if (MyNode.Tag == SiteTag)
                {
                    Infragistics.WebUI.UltraWebNavigator.Node MyNodeAdd = MyNode.Nodes.Add(DeviceText, DeviceTag);
                    if (InAlert)
                    {
                        MyNodeAdd.SelectedImageUrl = "~/images/eventlog.ico";
                        MyNodeAdd.ImageUrl = "~/images/eventlog.ico";
                        //MyNodeAdd.TargetFrame = "main"
                        //MyNodeAdd.TargetUrl = "Device.aspx?Device=" & DeviceTag

                        MyNode.ImageUrl = "~/images/eventlog.ico";
                        MyNode.SelectedImageUrl = "~/images/eventlog.ico";
                    }
                    else
                    {
                        MyNodeAdd.SelectedImageUrl = "~/images/performance.ico";
                        MyNodeAdd.ImageUrl = "~/images/performance.ico";
                        // MyNodeAdd.TargetFrame = "main"
                        // MyNodeAdd.TargetUrl = "Device.aspx?Device=" & DeviceTag
                    }
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

        }

        /// <summary>
        /// Add a device to the location section.
        /// </summary>
        /// <param name="LocationTag">Location tag</param>
        /// <param name="LocationName">The actual location name to add</param>
        /// <param name="DeviceText">The device name</param>
        /// <param name="DeviceTag">The device tag</param>
        /// <remarks></remarks>
        private void AddToLocationSection(string LocationTag, string LocationName, string DeviceText, string DeviceTag)
        {
            Infragistics.WebUI.UltraWebNavigator.Node actualLocationNode = new Infragistics.WebUI.UltraWebNavigator.Node();
            Infragistics.WebUI.UltraWebNavigator.Node ParentNode = ReturnNode("Location");
            bool hasLocationAlready = false;
            try
            {
                //Check if location Name already exists.
                if (ParentNode.Nodes.Count > 0)
                {
                    foreach (Infragistics.WebUI.UltraWebNavigator.Node childnode in ParentNode.Nodes)
                    {
                        if (childnode.Text.Contains(LocationName))
                        {
                            hasLocationAlready = true;
                            actualLocationNode = childnode;
                        }
                    }
                }


                // The location does not exist.
                if (hasLocationAlready == false)
                {
                    actualLocationNode.Text = LocationName;
                    ParentNode.Nodes.Add(actualLocationNode);
                }

                // Add device node to the location node.
                Infragistics.WebUI.UltraWebNavigator.Node MyNode = actualLocationNode.Nodes.Add(DeviceText, DeviceTag);
                MyNode.ShowExpand = true;
                MyNode.ImageUrl = "~/images/performance.ico";
                MyNode.SelectedImageUrl = "~/images/performance.ico";
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message;
            }
        }

        /// <summary>
        /// Add a device to the status section.
        /// </summary>
        /// <param name="StatusType">The status type.</param>
        /// <param name="DeviceName">The name of the device.</param>
        /// <param name="deviceTag">The tag of the device.</param>
        /// <remarks></remarks>
        private void addToStatusSection(string StatusType, string DeviceName, string deviceTag)
        {
            Infragistics.WebUI.UltraWebNavigator.Node statusNode = new Infragistics.WebUI.UltraWebNavigator.Node();
            Infragistics.WebUI.UltraWebNavigator.Node ParentNode = ReturnNode("Status");
            bool hasStatusAlready = false;
            try
            {
                //Check if status type already exists.
                foreach (Infragistics.WebUI.UltraWebNavigator.Node childnode in ParentNode.Nodes)
                {
                    if (childnode.Text.Contains(StatusType))
                    {
                        hasStatusAlready = true;
                        statusNode = childnode;
                    }
                }

                // The status type does not exist.
                // Add the status to the parentnode.
                if (hasStatusAlready == false)
                {
                    statusNode.Text = StatusType;
                    ParentNode.Nodes.Add(statusNode);
                }

                // Add device node to the status type node.
                Infragistics.WebUI.UltraWebNavigator.Node MyNode = statusNode.Nodes.Add(DeviceName, deviceTag);
                MyNode.ShowExpand = true;
                MyNode.ImageUrl = "~/images/performance.ico";
                MyNode.SelectedImageUrl = "~/images/performance.ico";
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message;
            }
        }




        public bool Contains(string[] arr, object value)
        {
            foreach (object o_loopVariable in arr)
            {
                object o = o_loopVariable;
                if (o == value)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Returns the node that has the tag value of "root".
        /// 
        /// </summary>
        /// <returns>Infragistics.WebUI.UltraWebNavigator.Node</returns>
        /// <remarks></remarks>
        public Infragistics.WebUI.UltraWebNavigator.Node ReturnRoot()
        {
            Infragistics.WebUI.UltraWebNavigator.Node functionReturnValue = default(Infragistics.WebUI.UltraWebNavigator.Node);
            functionReturnValue = null;
            //Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = default(Infragistics.WebUI.UltraWebNavigator.Node);
            foreach (Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot in this.SiteTree.Nodes)
            {
                if (MyNodeRoot.Tag == "root")
                {
                    return MyNodeRoot;
                }
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Returns the node that has the "Alerts" tag value.
        /// </summary>
        /// <returns>Infragistics.WebUI.UltraWebNavigator.Node</returns>
        /// <remarks></remarks>
        public Infragistics.WebUI.UltraWebNavigator.Node ReturnAlertsRoot()
        {
            Infragistics.WebUI.UltraWebNavigator.Node functionReturnValue = default(Infragistics.WebUI.UltraWebNavigator.Node);
            functionReturnValue = null;
            //Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = default(Infragistics.WebUI.UltraWebNavigator.Node);
            foreach (Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot in this.SiteTree.Nodes)
            {
                if (MyNodeRoot.Tag == "Alerts")
                {
                    return MyNodeRoot;
                }
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Return the node that has a tag value that matches the TagName provided.
        /// </summary>
        /// <param name="TagName">The tag value.</param>
        /// <returns>Infragistics.WebUI.UltraWebNavigator.Node</returns>
        /// <remarks></remarks>
        private Infragistics.WebUI.UltraWebNavigator.Node ReturnNode(string TagName)
        {
            Infragistics.WebUI.UltraWebNavigator.Node node = new Infragistics.WebUI.UltraWebNavigator.Node();
            try
            {
                //Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot = default(Infragistics.WebUI.UltraWebNavigator.Node);
                foreach (Infragistics.WebUI.UltraWebNavigator.Node MyNodeRoot in this.SiteTree.Nodes)
                {
                    if (MyNodeRoot.Tag == TagName)
                    {
                        node = MyNodeRoot;
                    }
                }
            }
            catch (Exception ex)
            {
                node = null;
            }


            return node;
        }

        //'roger mini tree end
        #endregion

        public void RaiseCallbackEvent(string eventArgument)
        {
            _callbackArg = eventArgument;
        }

        public string GetCallbackResult()
        {
            Computer com = new Computer();
            LastRefresh = Convert.ToDateTime(Session["LastRefresh"]);
            switch (_callbackArg)
            {
                case "time":
                    return DateAndTime.Now.ToString();
                case "mem":
                    return (com.Info.AvailablePhysicalMemory / 1024).ToString() + "K";
                case "Fill_Grid":
                    TimeSpan diff = LastRefresh - DateTime.Now;
                    

                    if (Convert.ToInt32(diff) >= 20)
                    {
                        Fill_Grid();
                        LastRefresh = DateAndTime.Now;
                        Session["LastRefresh"] = LastRefresh;
                        Session["MyGrid"] = MyTable;
                    }
                    else
                    {
                        
                        return Session["MyGrid"].ToString();
                    }
                    //Me.r
                    return MyTable;
                default:
                    return _callbackArg;
            }

        }

        public void Fill_Grid()
        {
            try
            {
                //Hold all the sensor ID's
                System.Collections.Generic.List<int> selectSensors = new System.Collections.Generic.List<int>();
                try
                {
                    // Are there any sensors?
                    if ((Session["SelectSensors"] == null) == false)
                    {
                        //Yes.
                        selectSensors = (System.Collections.Generic.List<int>)Session["SelectSensors"];
                    }

                }
                catch (Exception ex)
                {
                }

                //GetServerObjects 'server1.GetAll()
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                object MyObject1 = null;
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            MyCameraCollection.Add(MyCamera);
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            MySensorCollection.Add(MySensor);
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                        {
                            LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;
                            MySensorGroupCollection.Add(MySensorGroup, MySensorGroup.SensorGroupID.ToString());
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                            MyIPDevicesCollection.Add(MyIPDevicesDetails);
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                            MyOtherDevicesCollection.Add(MyOtherDevicesDetails);
                        }
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        {
                            LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                            MySNMPDevicesCollection.Add(MySNMPDevicesDetails);
                        }
                    }
                }
                try
                {
                    FillSites();

                }
                catch (Exception ex)
                {
                }
                Collection MyFilteredSensors = new Collection();
                //= MySensorCollection

                for (int MyCntSensor = 1; MyCntSensor <= MySensorCollection.Count; MyCntSensor++)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensObj1 = (LiveMonitoring.IRemoteLib.SensorDetails)MySensorCollection[MyCntSensor];
                    MyFilteredSensors.Add(MySensObj1);
                }

                try
                {
                    if (selectSensors.Count > 0)
                    {
                        MyFilteredSensors.Clear();
                        //LiveMonitoring.IRemoteLib.SensorDetails MySensObj = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensObj in MySensorCollection)
                        {
                            if (selectSensors.Contains(MySensObj.ID))
                            {
                                MyFilteredSensors.Add(MySensObj);
                            }
                        }
                    }
                    else
                    {
                        MyFilteredSensors = MySensorCollection;
                    }

                }
                catch (Exception ex)
                {
                }


                LiveMonitoring.IRemoteLib.SensorDetails MyCurSensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                string MyDeviceName = "";
                //BackColor="#C1D2EE" BorderColor="#316AC5"
                MyTable = "<table id=\"myTable\" runat=\"server\" class=\"table table-striped responsive-utilities jambo_table bulk_action table-bordered panel-success col-md-12 col-sm-6 col-xs-6\" BorderColor=\"#316AC5\"><tr bgcolor=\"#C1D2EE\"><td>Icon</td><td>Device</td><td>Sensor</td><td>Field</td><td>Alert</td><td>Value</td><td>Extra Value</td></tr>";
                MaxNo.Value = MyFilteredSensors.Count.ToString();

                //MySensorCollection.Count
                for (int MyCntSensor = 1; MyCntSensor <= MyFilteredSensors.Count; MyCntSensor++)
                {
                    if (MyCntSensor >= Convert.ToInt32(StartNo.Value) & MyCntSensor <= Convert.ToInt32(EndNo.Value))
                    {
                        MyCurSensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyFilteredSensors[MyCntSensor];
                        MyDeviceName = "";
                        //LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
                        foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails in MyIPDevicesCollection)
                        {
                            if (MyCurSensor.IPDeviceID == MyIPDevicesDetails.ID)
                            {
                                MyDeviceName = MyIPDevicesDetails.Caption;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        if (string.IsNullOrEmpty(MyDeviceName))
                        {
                            //LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
                            foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails in MyOtherDevicesCollection)
                            {
                                if (MyCurSensor.IPDeviceID == MyOtherDevicesDetails.ID)
                                {
                                    MyDeviceName = MyOtherDevicesDetails.Caption;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(MyDeviceName))
                        {
                            //LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
                            foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails in MySNMPDevicesCollection)
                            {
                                if (MyCurSensor.IPDeviceID == MySNMPDevicesDetails.ID)
                                {
                                    MyDeviceName = MySNMPDevicesDetails.Caption;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(MyDeviceName))
                        {
                           // LiveMonitoring.IRemoteLib.CameraDetails MyCamera = default(LiveMonitoring.IRemoteLib.CameraDetails);
                            foreach (LiveMonitoring.IRemoteLib.CameraDetails MyCamera in MyCameraCollection)
                            {
                                if (MyCurSensor.IPDeviceID == MyCamera.ID)
                                {
                                    MyDeviceName = MyCamera.Caption;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }
                        //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                        foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MyCurSensor.Fields)
                        {
                            try
                            {
                                if (MyFields.DisplayValue)
                                {
                                    AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblerr.Visible = true;
                                lblerr.Text = ex.Message;
                                AddValues(MyCurSensor.ID.ToString(), MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status, MyFields.LastValue.ToString() + MyFields.Caption, MyFields.LastOtherValue);
                            }
                        }
                    }
                }
                MyTable += "</table>";
            }
            catch (Exception ex)
            {
                lblerr.Visible = true;
                lblerr.Text = ex.Message;
            }
            ////GridStatus.DataBind()
        }

        public void AddValues(string MyImage, string MyDevice, string MyField, string MySensor, LiveMonitoring.IRemoteLib.StatusDef MyStatus, string MyValue, string MyOtherValue)
        {
            try
            {
                //Me.GridStatus.Rows.Add()
                string MyAlert = null;
                string MyColor = null;
                // Black         Gray         Silver         White
                //Yellow         Lime         Aqua         Fuchsia
                //Red         Green         Blue         Purple
                //Maroon         Olive         Navy         Teal
                switch (MyStatus)
                {
                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                        MyAlert = "Alert";
                        MyColor = "Red";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.criticalerror:
                        MyAlert = "Critical";
                        MyColor = "#8B0000";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.devicefailure:
                        MyAlert = "Device failure";
                        MyColor = "Fuchsia";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.disabled:
                        MyAlert = "Disabled";
                        MyColor = "Gray";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                        MyAlert = "No Response";
                        MyColor = "Purple";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.notify:
                        MyAlert = "Notify";
                        MyColor = "Navy";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                        MyAlert = "OK";
                        MyColor = "Green";
                        break;
                    case LiveMonitoring.IRemoteLib.StatusDef.statuserror:
                        MyColor = "Maroon";
                        MyAlert = "Yellow";
                        break;
                    default:
                        MyAlert = "Unknown";
                        MyColor = "White";
                        break;
                }

                MyTable += "<tr><a href=DisplayGraphs.aspx?SensorNum=" + MyImage + " target='main'>";
                MyTable += "<td>";
                //<img src='ReturnThumbnailImage.aspx?Sensor=" & MyImage & "' height=12 width=12>"
                MyTable += "</td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyDevice);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MySensor);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyField);
                MyTable += "</font></td><td><font size=2 color='" + MyColor +";color: white;border-radius: 36px 50px 50px 36px / 12px 30px 30px 12px\">" + "'>" + Server.HtmlEncode(MyAlert);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyValue);
                MyTable += "</font></td><td><font size=2 color='" + MyColor + "'>" + Server.HtmlEncode(MyOtherValue);
                MyTable += "</font></td>";
                MyTable += "</a><tr>";

            }
            catch (Exception ex)
            {
            }


        }

        protected void btnNext20_Click(object sender, System.EventArgs e)
        {
            //StartNo
            //EndNo
            //MaxNo
            if (Convert.ToInt32(EndNo.Value) < Convert.ToInt32(MaxNo.Value))
            {
                StartNo.Value = (Convert.ToInt32(StartNo.Value) + 20).ToString();
                EndNo.Value = (Convert.ToInt32(EndNo.Value) + 20).ToString();
            }
            else
            {
                StartNo.Value = Convert.ToInt32(0).ToString();
                EndNo.Value = Convert.ToInt32(20).ToString();
            }
            Fill_Grid();
            Session["MyGrid"] = MyTable;
        }

        protected void btnPrev20_Click(object sender, System.EventArgs e)
        {
            if (Convert.ToInt32(StartNo.Value) > 0)
            {
                StartNo.Value = (Convert.ToInt32(StartNo.Value) - 20).ToString();
                EndNo.Value = (Convert.ToInt32(EndNo.Value) - 20).ToString();
            }
            else
            {
                StartNo.Value = Convert.ToInt32(0).ToString();
                EndNo.Value = Convert.ToInt32(20).ToString();
            }
            Fill_Grid();
            Session["MyGrid"] = MyTable;
        }

        protected void SiteTree_NodeClicked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
        {
            Infragistics.WebUI.UltraWebNavigator.Node MyNodeAdd = e.Node;
            // ListBox1.Items.Clear()
            System.Collections.Generic.List<int> MyRet = new System.Collections.Generic.List<int>();
            // ReturnItems(MyNodeAdd)
            if (MyNodeAdd.Nodes.Count > 0)
            {
                //children
                //Infragistics.WebUI.UltraWebNavigator.Node MyChild = default(Infragistics.WebUI.UltraWebNavigator.Node);
                foreach (Infragistics.WebUI.UltraWebNavigator.Node MyChild in MyNodeAdd.Nodes)
                {
                    if (MyChild.Nodes.Count > 0)
                    {
                        //children1
                        //Infragistics.WebUI.UltraWebNavigator.Node MyChild1 = default(Infragistics.WebUI.UltraWebNavigator.Node);
                        foreach (Infragistics.WebUI.UltraWebNavigator.Node MyChild1 in MyChild.Nodes)
                        {
                            // ListBox1.Items.Add(MyChild1.Text)
                            if (Information.IsNumeric(MyChild1.Tag))
                            {
                                MyRet.Add(Convert.ToInt32(MyChild1.Tag));
                            }

                        }
                    }
                    else
                    {
                        //top node
                        // ListBox1.Items.Add(MyChild.Text)
                        if (Information.IsNumeric(MyChild.Tag))
                        {
                            MyRet.Add(Convert.ToInt32(MyChild.Tag));
                        }
                    }
                }
            }
            else
            {
                //top node
                //ListBox1.Items.Add(MyNodeAdd.Text)
                if (Information.IsNumeric(MyNodeAdd.Tag))
                {
                    MyRet.Add(Convert.ToInt32(MyNodeAdd.Tag));
                }
            }
            if (MyRet.Count > 0)
            {
                //Dim MyString As String = ""
                //For Each myInteger In MyRet
                // MyString += myInteger.ToString + "|"
                // 'ListBox1.Items.Add(MyChild.Text)
                //Next
                Session["SelectSensors"] = MyRet;
                Fill_Grid();
                StartNo.Value = Convert.ToInt32(0).ToString();
                EndNo.Value = Convert.ToInt32(20).ToString();
                Session["MyGrid"] = MyTable;
            }
        }

        public System.Collections.Generic.List<int> ReturnItems(Infragistics.WebUI.UltraWebNavigator.Node MyNodeAdd)
        {
            System.Collections.Generic.List<int> MyRet = new System.Collections.Generic.List<int>();
            if (MyNodeAdd.Nodes.Count > 0)
            {
                //has children

                //Infragistics.WebUI.UltraWebNavigator.Node MyChild = default(Infragistics.WebUI.UltraWebNavigator.Node);
                foreach (Infragistics.WebUI.UltraWebNavigator.Node MyChild in MyNodeAdd.Nodes)
                {
                    System.Collections.Generic.List<int> MyRet1 = ReturnItems(MyChild);
                    foreach (int myInteger in MyRet1)
                    {
                       //  myInteger = myInteger_loopVariable;
                        MyRet.Add(myInteger);
                        //ListBox1.Items.Add(MyChild.Text)
                    }
                }
            }
            else
            {
                //top node
                //ListBox1.Items.Add(MyNodeAdd.Text)
                MyRet.Add(Convert.ToInt32(MyNodeAdd.Tag));
            }
            return MyRet;
        }
        public void topbar()
        {
            Load += Page_Load;
        }

        protected void SiteTree_NodeClicked1(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
        {

        }
    }

}