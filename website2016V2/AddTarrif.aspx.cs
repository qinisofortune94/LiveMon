using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2.Metering
{
    public partial class AddTarrif : System.Web.UI.Page
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

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
                    Response.Expires = 5;
                    Page.MaintainScrollPositionOnPostBack = true;
                    // Session["StartDate"] = DateTimeOffset.Now;
                    Session["StartDate"] = DateAndTime.DateAdd(DateInterval.Minute, -30, DateTime.Now);
                    Session["EndDate"] = DateTime.Now;

                    int MySensorNum = 0;
                    MySensorNum = Convert.ToInt32(Request.QueryString["SensorNum"]);
                    //Load_Sensors(MySensorNum);
                    Load_Tarrifs();
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
                                    //AddLayer(MySensor);
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
        }
        private void Load_Tarrifs()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<LiveMonitoring.IRemoteLib.MeteringTariff> MyNewList = MyRem.LiveMonServer.GetMeteringTarrifNames();
            bool Firstone = true;
            foreach (LiveMonitoring.IRemoteLib.MeteringTariff MyTarrif in MyNewList)
            {
                System.Web.UI.WebControls.ListItem MyIttem = new System.Web.UI.WebControls.ListItem();
                MyIttem.Text = MyTarrif.TarriffName;
                MyIttem.Value = MyTarrif.ID.ToString();
                //ddlTarrif.Items.Add(MyIttem);
                if (Firstone)
                {
                    MyIttem.Selected = true;
                    Firstone = false;
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (CanSave())
            {
                //Check button text to add or update
                string temp = btnAdd.Text;

                if (temp.Contains("Add"))
                {
                    if (SavePeople() != 0)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Details were successfully saved.";
                        // Clear();
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while trying to save details, please try again.";
                    }
                }
                //Load_Tarrifs();

               // Clear();
            }
        }
        public bool CanSave()
        {
            if (txtTarrif.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter first name.";
                txtTarrif.Focus();
                return false;
            }

            return true;
        }
        public int SavePeople()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            int response = 0;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.MeteringTariff NewTarrif = new LiveMonitoring.IRemoteLib.MeteringTariff();
            NewTarrif.TarriffName = txtTarrif.Text;
            
            response = MyRem.LiveMonServer.AddNewMeteringTarrif(NewTarrif);
            return response;
           // return response;
        }
        public void Clear()
        {
            txtTarrif.Text = "";
           
        }
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public void People_Tarrif()
        {
            Load += Page_Load;
        }
    }
}