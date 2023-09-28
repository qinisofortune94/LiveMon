using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2.Metering
{
    public partial class MeteringEventLog : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (((string)Session["LoggedIn"] == "True"))
        //    {

        //        DataManager datamanager = new DataManager();

        //        LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
        //        MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
        //        Label user = this.Master.FindControl("lblUser") as Label;
        //        Label loginUser = this.Master.FindControl("lblUser2") as Label;
        //        Label LastLogin = this.Master.FindControl("LastLogin") as Label;
        //        loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
        //        user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
        //        LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

        //        successMessage.Visible = false;
        //        warningMessage.Visible = false;
        //        errorMessage.Visible = false;

        //        string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
        //        LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

        //        int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
        //        MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
        //        if (MyIPMonPageSecure > MyUser.UserLevel)
        //        {
        //            Response.Redirect("NotAuthorisedView.aspx");
        //        }

        //        else
        //        {
        //            Response.Expires = 5;
        //            Page.MaintainScrollPositionOnPostBack = true;
        //            Session("StartDate") = DateAdd(DateInterval.Minute, -30, Now);
        //            Session("EndDate") = Now;

        //            int MySensorNum;
        //            MySensorNum = (int)Request.QueryString("SensorNum");
        //            Load_Sensors(MySensorNum);
        //            Load_Tarrifs();
        //            Session("Sensors") = "";
        //            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
        //            if (MySensorNum == 0)
        //            {
        //            }

        //            else
        //            {
        //                //specific
        //                Collection MyCollection = new Collection();
        //                MyCollection = MyRem.GetServerObjects(IIf(IsNothing(Session("SelectedSite")), null, Session("SelectedSite")));
        //                //GetServerObjects 'server1.GetAll()
        //                object MyObject1;
        //                int MyCnt = 0;
        //                foreach (MyObject1 in MyCollection)
        //                {
        //                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
        //                    {
        //                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
        //                        if (MySensorNum == MySensor.ID)
        //                        {
        //                            Session("Sensors") += MySensor.ID.ToString + ",";
        //                            AddLayer(MySensor);
        //                            break; // TODO: might not be correct. Was : Exit For
        //                        }
        //                        else
        //                        {
        //                            MyCnt += 1;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
 