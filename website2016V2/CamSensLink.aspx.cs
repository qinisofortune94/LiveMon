using Infragistics.WebUI.UltraWebGrid;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using website2016V2;

namespace website2016V2
{
    public partial class CamSensLink : System.Web.UI.Page
    {
        public PageSecuritySetup IPMOnPageSecure;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != "True")
            {
                Response.Redirect("NotAuthorisedLogon.aspx");
            }
            //ok logged on level ?
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (99 < MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedView.aspx");
            }
            if (IsPostBack == false)
            {
                if (Request.QueryString["Camera"] != "undefined" & (Request.QueryString["Camera"] == null) == false)
                {
                    LoadSpecificPage(Convert.ToInt32(Request.QueryString["Camera"]));
                }
                else
                {
                    LoadAllPage();
                }

            }


        }

        public void LoadCameraSensors()
        {
            this.cmbCamera.Items.Clear();
            this.cmbSensor.Items.Clear();
            Collection MyCol = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCol = MyRem.LiveMonServer.GetAll();
            object MyObj = null;
            foreach (object MyObj_loopVariable in MyCol)
            {
                MyObj = MyObj_loopVariable;
                if (MyObj is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObj;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyCamera.Caption;
                    MyItem.Value = MyCamera.ID.ToString();
                    MyItem.Selected = false;
                    this.cmbCamera.Items.Add(MyItem);
                }
                if (MyObj is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensorDetails = (LiveMonitoring.IRemoteLib.SensorDetails)MyObj;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MySensorDetails.Caption;
                    MyItem.Value = MySensorDetails.ID.ToString();
                    MyItem.Selected = false;
                    this.cmbSensor.Items.Add(MyItem);
                }
            }
        }

        public void LoadAllPage()
        {
            LoadCameraSensors();

            grdCamSensLink.Rows.Clear();
          //  LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            //Dim MyIPMonPageSecure As Integer = IPMonPageSecure.GetViewLevelByName(name)
            Collection MyCol = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            MyCol = MyRem.LiveMonServer.GetAllCamSensLink();
            foreach (LiveMonitoring.IRemoteLib.CameraSensorLink MyLink in MyCol)
            {
                UltraGridRow myrow = new UltraGridRow(true);
                myrow.Cells.Add();
                myrow.Cells[0].Value = MyLink.CameraID;
                myrow.Cells.Add();
                myrow.Cells[1].Value = MyLink.SensorID;
                myrow.Cells.Add();
                myrow.Cells[2].Value = MyLink.ViewLevel;
                myrow.Cells[2].AllowEditing = AllowEditing.Yes;
                myrow.Cells.Add();
                myrow.Cells[3].Value = MyLink.ExecuteLevel;
                myrow.Cells.Add();
                myrow.Cells[5].Value = "Delete";
                myrow.Tag = MyLink.ID;
                grdCamSensLink.Rows.Add(myrow);
            }
        }
        public void LoadSpecificPage(int ID)
        {
            LoadCameraSensors();

            grdCamSensLink.Rows.Clear();
           // LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            //Dim MyIPMonPageSecure As Integer = IPMonPageSecure.GetViewLevelByName(name)
            Collection MyCol = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            MyCol = MyRem.LiveMonServer.GetSpecificCamSensLink(Convert.ToInt32( ID));
            foreach (LiveMonitoring.IRemoteLib.CameraSensorLink MyLink in MyCol)
            {
                UltraGridRow myrow = new UltraGridRow(true);
                myrow.Cells.Add();
                myrow.Cells[0].Value = MyLink.CameraID;
                myrow.Cells.Add();
                myrow.Cells[1].Value = MyLink.SensorID;
                myrow.Cells.Add();
                myrow.Cells[2].Value = MyLink.ViewLevel;
                myrow.Cells[2].AllowEditing = AllowEditing.Yes;
                myrow.Cells.Add();
                myrow.Cells[3].Value = MyLink.ExecuteLevel;
                myrow.Cells.Add();
                myrow.Cells[4].Value = "Delete";
                myrow.Tag = MyLink.ID;
                grdCamSensLink.Rows.Add(myrow);
            }
        }

        protected void grdCamSensLink_ClickCellButton(object sender, Infragistics.WebUI.UltraWebGrid.CellEventArgs e)
        {
            UltraGridRow myrow = e.Cell.Row;
            LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            //Dim MyIPMonPageSecure As Integer = IPMonPageSecure.GetViewLevelByName(name)

            MyLink.CameraID =Convert.ToInt32 (myrow.Cells[0].Value);
            MyLink.SensorID =Convert.ToInt32(myrow.Cells[1].Value);
            //MsgBox(myrow.Cells(2).Text)
            MyLink.ViewLevel =Convert.ToInt32(myrow.Cells[2].Value);
            MyLink.ExecuteLevel =Convert.ToInt32(myrow.Cells[3].Value);
            MyLink.ID =Convert.ToInt32(myrow.Tag);

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            if (MyRem.LiveMonServer.DeleteCamSensLink(MyLink.ID) == false)
            {
                lblErr.Visible = true;
                lblErr.Text = "Not deleted Error!";
                return;
            }
            if (Request.QueryString["Camera"] != "undefined" & (Request.QueryString["Camera"] == null) == false)
            {
                LoadSpecificPage(Convert.ToInt32(Request.QueryString["Camera"]));
            }
            else
            {
                LoadAllPage();
            }
        }

        protected void btnAdd_Click(object sender, System.EventArgs e)
        {
            LiveMonitoring.IRemoteLib.CameraSensorLink MyLink = new LiveMonitoring.IRemoteLib.CameraSensorLink();
            //Dim MyIPMonPageSecure As Integer = IPMonPageSecure.GetViewLevelByName(name)

            MyLink.CameraID =Convert.ToInt32(this.cmbCamera.SelectedValue);
            MyLink.SensorID =Convert.ToInt32(this.cmbSensor.SelectedValue);
            //MsgBox(myrow.Cells(2).Text)
            MyLink.ViewLevel =Convert.ToInt32(this.txtViewLevel.Text);
            MyLink.ExecuteLevel =Convert.ToInt32( this.txtExecuteLevel.Text);
            //MyLink.ID = myrow.Tag

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            if (MyRem.LiveMonServer.AddNewCamSensLink(MyLink) == false)
            {
                lblErr.Visible = true;
                lblErr.Text = "Not deleted Error!";
                return;
            }
            if (Request.QueryString["Camera"] != "undefined" & (Request.QueryString["Camera"] == null) == false)
            {
                LoadSpecificPage(Convert.ToInt32(Request.QueryString["Camera"]));
            }
            else
            {
                LoadAllPage();
            }

        }

        public CamSensLink()
        {
            Load += Page_Load;
        }

        protected void grdCamSensLink_InitializeLayout(object sender, LayoutEventArgs e)
        {

        }
    }
}