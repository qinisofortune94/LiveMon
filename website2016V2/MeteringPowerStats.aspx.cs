using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;

using System.Runtime.Remoting.Channels;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using website2016V2.Usercontrols;

namespace website2016V2
{
    public partial class MeteringPowerStats : System.Web.UI.Page
    {
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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                //MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                if (IsPostBack == true)
                {
                    RegenerateCallbackGraphs();

                }
                else
                {
                    Response.Expires = 5;
                    Page.MaintainScrollPositionOnPostBack = true;
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateAndTime.Now);
                    Session["EndDate"] = DateAndTime.Now;

                    int MySensorNum = 0;
                    MySensorNum = Convert.ToInt32(Request.QueryString["SensorNum"]);
                    Load_Sensors(MySensorNum);
                    // Load_Tarrifs()
                    Session["Sensors"] = "";
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
                        // If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                        // If added = False Then 'only add 1st one
                        // AddLayer(MyObject1)
                        // added = True
                        // Session["Sensors"] += MyObject1.ID.ToString + ","
                        // End If
                        // End If
                        //Next
                    }
                    else
                    {
                        //specific
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
        public void AddPageBreak()
        {
            HtmlGenericControl MyHtml = new HtmlGenericControl();
            MyHtml.InnerHtml = "<div style=\"height:1px\">&nbsp;</div><div style=\"page-break-before: always; height:1px;\">&nbsp;</div>";



            // MyHtml.InnerHtml = "<tr style=""page-break-before: always;"">"
            this.Charts.Controls.Add(MyHtml);
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
                    // If MySensor.SiteID = CInt(Session["Site"]) Then
                    if (MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile | 
                        MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile |
                        MySensor.Type == LiveMonitoring.IRemoteLib.SensorDetails.SensorType.LandisGyrE650Profile)
                    {
                        //Dim MyIttem As New System.Web.UI.WebControls.ListItem
                        //MyIttem.Text = MySensor.Caption
                        //MyIttem.Value = MySensor.ID
                        //SensorsList.Items.Add(MyIttem)
                        //If Firstone Then
                        // MyIttem.Selected = True
                        // Firstone = False
                        //End If
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
                        subnode.ShowCheckBox = true;
                        subnode.Text = MySensor.Caption;
                        subnode.Value = MySensor.ID.ToString();
                        node.ChildNodes.Add(subnode);
                        //Dim node As TreeNode = FindNode(MySensor.SensGroup.SensorGroupName)
                        //If IsNothing(node) Then
                        // Dim node1 As New TreeNode
                        // node1.ShowCheckBox = False
                        // node1.Text = MySensor.SensGroup.SensorGroupName 'Item(CInt(MySensor.Type))
                        // node1.Value = MySensor.SensGroup.SensorGroupID 'CInt(MySensor.Type)
                        // node1.Expanded = False
                        // tvSensors.Nodes.Add(node1)
                        // node = FindNode(MySensor.SensGroup.SensorGroupName)
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

        public void AddLayer(LiveMonitoring.IRemoteLib.SensorDetails SensorDet)
        {
            //MyCollection

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();


            if ((SensorDet == null) == false)
            {
                if (this.SelectDispType.SelectedValue == "0")
                {
                    //// Create instance of the UserControl SimpleControl
                    dynamic myPowerStatusUserControl = (PowerStatusUserControl)LoadControl("~/UserControls/PowerStatusUserControl.ascx");

                    //// Set the Public Properties
                    myPowerStatusUserControl.MeterID = SensorDet.ID;
                    myPowerStatusUserControl.DisplayInterval = Convert.ToInt32(DefaultDateRange.SelectedValue);
                    myPowerStatusUserControl.UpdateInterval = 60000;
                    try
                    {
                        myPowerStatusUserControl.ReductionTarget = double.Parse(SensorDet.ExtraData3);

                    }
                    catch (Exception ex)
                    {
                    }
                    myPowerStatusUserControl.StartScanner();
                    //myPowerStatusUserControl.
                    PowerUsage.Controls.Add(myPowerStatusUserControl);
                }
                else
                {
                    //// Create instance of the UserControl SimpleControl
                    dynamic myPowerTargetUserControl = (PowerTargetUserControl)LoadControl("~/UserControls/PowerTargetUserControl.ascx");

                    //// Set the Public Properties
                    myPowerTargetUserControl.MeterID = SensorDet.ID;
                    myPowerTargetUserControl.DisplayInterval = Convert.ToInt32(DefaultDateRange.SelectedValue);
                    myPowerTargetUserControl.UpdateInterval = 60000;
                    try
                    {
                        myPowerTargetUserControl.ReductionTarget = double.Parse(SensorDet.ExtraData3);

                    }
                    catch (Exception ex)
                    {
                    }
                    myPowerTargetUserControl.StartScanner();
                    //myPowerStatusUserControl.
                    PowerUsage.Controls.Add(myPowerTargetUserControl);
                }
                //Select Case SensorDet.Type
                // Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterReadProfile

                // Case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterReadProfile

                //End Select

            }


        }

        public void DrawHtmlLine()
        {
            StringBuilder MyHtmlStr = new StringBuilder();
            MyHtmlStr.Append("<br/><hr/><br/>");
        }
        protected void CmdQuickGenerate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
         
            FillSessionSensors();
            RegenerateCallbackGraphs();
        }

        public void FillSessionSensors()
        {
            //Dim MyItem As New Web.UI.WebControls.ListItem()
            //Dim MyCollection As New Collection
            //'clear session var
            //Session["Sensors"] = ""
            //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
            //Session["Sensors"] += SensorsList.SelectedValue
            Session["Sensors"] = "";
            foreach (TreeNode Mynode in tvSensors.Nodes)
            {
                foreach (TreeNode subnode in Mynode.ChildNodes)
                {
                    if (subnode.Checked)
                    {
                        Session["Sensors"] += subnode.Value.ToString() + ",";
                    }
                }
            }
            //MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session["SelectedSite"]), Nothing, Session["SelectedSite"]))'GetServerObjects 'server1.GetAll()
            //For Each MyItem In chkSensors.Items
            // If MyItem.Selected = True Then 'clear old check
            // 'add to cameras
            // Dim MyObject1 As Object
            // Dim MyCnt As Integer = 0
            // For Each MyObject1 In MyCollection
            // If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            // Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            // If MySensor.ID = MyItem.Value Then
            // Session["Sensors"] += MySensor.ID.ToString + ","
            // Exit For
            // Else
            // MyCnt += 1
            // End If
            // End If
            // Next
            // End If
            //Next

        }

        public void RegenerateCallbackGraphs()
        {
            this.PowerUsage.Controls.Clear();
            //FillSessionSensors()
            if ((Session["Sensors"] == null))
                return;
            string[] mySensors = Strings.Split(Session["Sensors"].ToString(), ",");
            int Acnt = 0;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //If WebPanel1.Expanded Then
            // WebPanel1.Expanded = False
            //End If
            Collection MyCollection = new Collection();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            //- 1 'last one is empty
            for (Acnt = 0; Acnt <= Information.UBound(mySensors); Acnt++)
            {

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

        protected void RadioButtonList1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }
        public MeteringPowerStats()
        {
            Load += Page_Load;
        }

        protected void tvSensors_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
           
            FillSessionSensors();
            RegenerateCallbackGraphs();

        }

        protected void SelectDispType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}