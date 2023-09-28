using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class EditCameras : System.Web.UI.Page
    {
        public void LoadDevices()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = false;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails Mysensor = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = Mysensor.Caption;
                    MyItem.Value = Mysensor.ID.ToString();
                    if (added == false)
                    {
                        MyItem.Selected = true;
                        added = true;
                        LoadSpecificDevice(Mysensor.ID);

                        //accordion.Visible = true;
                       //.Visible = true;
                    }
                    else
                    {
                        MyItem.Selected = false;
                    }

                    cmbDevices.Items.Add(MyItem);

                    //Dim myrow As New UltraGridRow(True)
                    //myrow.Cells.Add()
                    //myrow.Cells(0).Value = Mysensor.Caption
                    //myrow.Cells.Add()
                    //myrow.Cells(1).Value = "" '"<img src=ReturnnormalImage.aspx?Camera=" + Mysensor.ID.ToString + ">"
                    //myrow.Tag = Mysensor.ID
                    //cmbDevices.Rows.Add(myrow)
                }
                if (MyObject1 is LiveMonitoring.IRemoteLib.LocationDetails)
                {
                    LiveMonitoring.IRemoteLib.LocationDetails MyLocation = (LiveMonitoring.IRemoteLib.LocationDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                    MyItem.Text = MyLocation.Building + "|" + MyLocation.Room + "|" + MyLocation.Cabinet + "|" + MyLocation.Floor + "|" + MyLocation.Street;
                    MyItem.Value = MyLocation.Id.ToString();
                    MyItem.Selected = false;
                    DdlDevicelocation.Items.Add(MyItem);
                }
            }
        }
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
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                // = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (Page.IsPostBack == false)
                {
                    LoadDevices();
                    LoadSites();
                }
                else
                {
                    //refresh the main page
                    StringBuilder TheScript = new StringBuilder();
                    // Holds the injected script.

                    // Create the script.
                    TheScript.Append("<script type='text/javascript'>" + Constants.vbCrLf);
                    TheScript.Append("parent.RefreshChildFrame(1);");
                    TheScript.Append(Constants.vbCrLf);
                    TheScript.Append("</script>");

                    this.ClientScript.RegisterStartupScript(typeof(string), "RefreshMain", TheScript.ToString());
                }

            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if ((DdlDevicelocation.SelectedValue == null) == false)
                {
                    MyRem.LiveMonServer.AddEditLocationLink(Convert.ToInt32(cmbDevices.SelectedValue),Convert.ToInt32(DdlDevicelocation.SelectedValue), LiveMonitoring.IRemoteLib.LocationDetails.DeviceSensorType.Camera, 99);
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnChangeSite_Click(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                if (Convert.ToInt32(Session["SelectedSite"]) != Convert.ToInt32(DdlDeviceSite.SelectedValue))
                {
                    int Myresp = Convert.ToInt32(MyRem.LiveMonServer.EditSensorDeviceSite(Convert.ToInt32(cmbDevices.SelectedValue), Convert.ToInt32(DdlDeviceSite.SelectedValue), LiveMonitoring.IRemoteLib.SDUpdateType.Camera));
                }

            }
            catch (Exception ex)
            {
            }
        }


        private void SortDropDown(DropDownList dd)
        {
            try
            {
                ListItem[] ar = null;
                int i = 0;
                foreach (ListItem li in dd.Items)
                {
                    Array.Resize(ref ar, i + 1);
                    ar[i] = li;
                    i += 1;
                }
                Array ar1 = ar;

                //ar1.Sort(ar1, new ListItemComparer());
                dd.Items.Clear();
                dd.Items.AddRange(ar);

            }
            catch (Exception ex)
            {
            }

        }

        private class ListItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListItem a = (System.Web.UI.WebControls.ListItem)x;
                ListItem b = (System.Web.UI.WebControls.ListItem)y;
                CaseInsensitiveComparer c = new CaseInsensitiveComparer();
                return c.Compare(a.Text, b.Text);
            }
        }


        public void LoadSites()
        {
            try
            {
                List<LiveMonitoring.IRemoteLib.SiteDetails> MyCollection = new List<LiveMonitoring.IRemoteLib.SiteDetails>();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.GetServerAllSites;
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;
                if ((MyCollection == null))
                    return;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        //cmbSensGroup
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
                        {
                            LiveMonitoring.IRemoteLib.SiteDetails MySite = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
                            //not orphans
                            if (MySite.ID > 0)
                            {
                                System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
                                MyItem.Text = MySite.SiteName;
                                MyItem.Value = MySite.ID.ToString();
                                MyItem.Selected = false;
                                try
                                {
                                    if (Convert.ToInt32(Session["SelectedSite"]) == MySite.ID)
                                    {
                                        MyItem.Selected = true;
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                                DdlDeviceSite.Items.Add(MyItem);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
                try
                {
                    SortDropDown(DdlDeviceSite);

                }
                catch (Exception ex)
                {
                }


            }
            catch (Exception ex)
            {
            }


        }

        protected void btnCreate_Click(object sender, EventArgs e)
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
            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = new LiveMonitoring.IRemoteLib.CameraDetails();
            LiveMonitoring.IRemoteLib.CameraDetails CurDevice = new LiveMonitoring.IRemoteLib.CameraDetails();
            try
            {
                CurDevice = ReturnSpecificDevice(Convert.ToInt32(cmbDevices.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Cannot find current sensor.";

                cmbDevices.Focus();
                return;

            }
            if ((CurDevice == null) == true)
            {
                errorMessage.Visible = true;
                lblError.Text = "Cannot find current sensor.";

                cmbDevices.Focus();
                return;

            }
            MyCamera = CurDevice;
            MyCamera.Caption = this.TxtCapiton.Text;
            MyCamera.EventRecord = this.chkEventEnabled.Checked;
            int MyEventRecord = 0;
            int i = 0;
            for (i = 0; i <= chkEvents.Items.Count - 1; i++)
            {
                if (this.chkEvents.Items[i].Selected)
                {
                    MyEventRecord = MyEventRecord | Convert.ToInt32(chkEvents.Items[i].Value);
                }
            }
            MyCamera.Events = MyEventRecord;
            MyCamera.Type = Convert.ToInt32(this.DdlType.SelectedValue);
            MyCamera.IPAdress = txtIpAdrres.Text;
            MyCamera.Port =Convert.ToInt32(txtPort.Text);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            if (this.filImageNormal.HasFile)
            {
                MyCamera.ImageNormal = Myfunc.Strip_Image(this.filImageNormal);
                MyCamera.ImageNormalByte = MyRem.ImagetoByte(MyCamera.ImageNormal, ImageFormat.Bmp);
            }
            if (this.filImageNoResponse.HasFile)
            {
                MyCamera.ImageNoResponse = Myfunc.Strip_Image(this.filImageNoResponse);
                MyCamera.ImageNoResponseByte = MyRem.ImagetoByte(MyCamera.ImageNoResponse, ImageFormat.Bmp);
            }
            if (this.filImageError.HasFile)
            {
                MyCamera.ImageError = Myfunc.Strip_Image(this.filImageError);
                MyCamera.ImageErrorByte = MyRem.ImagetoByte(MyCamera.ImageError, ImageFormat.Bmp);
            }
            MyCamera.Password = this.txtPassword.Text;
            MyCamera.User = this.txtUserName.Text;
            MyCamera.PostEventTime =Convert.ToInt32(TxtPostEventRecording.Text);
            MyCamera.PreEventTime = Convert.ToInt32(TxtPreEventRecording.Text);

            try
            {
                if ((Session["SelectedSite"] == null) == false)
                {
                    MyCamera.Add2Site = Convert.ToInt32(Session["SelectedSite"]);
                }

            }
            catch (Exception ex)
            {
            }

            bool Myresp = MyRem.LiveMonServer.EditCamera(MyCamera);
            if (Myresp)
            {
                //save fields
                //whoopeee
                successMessage.Visible = true;
                lblSucces.Text = "Edit Camera Succeed.";

                cmbDevices.Focus();

                try
                {
                    MyRem.WriteLog("Edit Camera Succeed", "User:" + MyUser.ID.ToString() + "|" + MyCamera.ID.ToString());

                }
                catch (Exception ex)
                {
                }

                ClearVals();
            }
            else
            {
                errorMessage.Visible = true;
                lblError.Text = "Edit Camera Failed.";

                cmbDevices.Focus();

                try
                {
                    MyRem.WriteLog("Edit Camera Failed", "User:" + MyUser.ID.ToString() + "|" + MyCamera.ID.ToString());

                }
                catch (Exception ex)
                {
                }
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during save, Please try again.";

                cmbDevices.Focus();
            }
        }


        public void ClearVals()
        {
            //lblErr.Visible = false;
            //lblErr.Text = "";

            this.txtPassword.Text = "";
            this.txtUserName.Text = "";
         
            TxtPostEventRecording.Text = "";
            TxtPreEventRecording.Text = "";
        
            this.txtPort.Text = "100";
      
            TxtCapiton.Text = "";
            txtIpAdrres.Text = "";
        }


        public LiveMonitoring.IRemoteLib.CameraDetails ReturnSpecificDevice(int ID)
        {

            LiveMonitoring.IRemoteLib.CameraDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.CameraDetails);
            functionReturnValue = null;
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails Mysensor = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    if (ID == Mysensor.ID)
                    {
                        return Mysensor;
                    }
                }
            }
            return functionReturnValue;
        }


        public void LoadSpecificDevice(int ID)
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails MyDevice = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    if (ID == MyDevice.ID)
                    {
                        this.DdlType.SelectedIndex = Convert.ToInt32(MyDevice.Type);
                        this.DdlType.SelectedValue = MyDevice.Type.ToString();
                       
                        this.TxtCapiton.Text = MyDevice.Caption;
                        string[] MyIp = MyDevice.IPAdress.Split('.');
                        txtIpAdrres.Text = MyDevice.IPAdress;
                        this.txtPort.Text = Convert.ToString(MyDevice.Port);
                        this.txtPassword.Text = MyDevice.Password;
                        this.txtUserName.Text = MyDevice.User;
                        this.imgError.ImageUrl = "ReturnErrorImage.aspx?Device=" + MyDevice.ID.ToString();
                        this.imgResponse.ImageUrl = "ReturnNoResponseImage.aspx?Device=" + MyDevice.ID.ToString();
                        this.imgNormal.ImageUrl = "ReturnNormalImage.aspx?Device=" + MyDevice.ID.ToString();
                        this.chkEventEnabled.Checked = MyDevice.EventRecord;
                        int MyEventRecord = MyDevice.Events;
                        int i = 0;
                        for (i = 0; i <= chkEvents.Items.Count - 1; i++)
                        {
                            int MyVal = 1;
                            MyVal = MyVal << i;
                            if ((MyVal & MyEventRecord) > 0)
                            {
                                chkEvents.Items[i].Selected = true;
                            }
                        }
                        this.TxtPostEventRecording.Text =Convert.ToString(MyDevice.PostEventTime);
                        this.TxtPreEventRecording.Text =Convert.ToString(MyDevice.PreEventTime);

                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
        }

        protected void cmdConfigure_Click(object sender, System.EventArgs e)
        {
            //Response.Redirect("Http://" + txtIP1.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text)
            Response.Redirect("proxy.aspx?url=" + txtIpAdrres.Text);

        }

        protected void cmbDevices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //this.accordion.Visible = true;
            //find details
            LoadSpecificDevice(Convert.ToInt32(cmbDevices.SelectedValue));
        }

        public EditCameras()
        {
            Load += Page_Load;
        }

        protected void btnDeleteCamera_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetDeleteLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedDelete.aspx");
            }
            LiveMonitoring.IRemoteLib.CameraDetails Cursensor = new LiveMonitoring.IRemoteLib.CameraDetails();
            try
            {
                Cursensor = ReturnSpecificDevice(Convert.ToInt32(this.cmbDevices.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error has occured during Delete, Please try again.";

                cmbDevices.Focus();
                return;
                //error cannot find current sensor
            }
            if ((Cursensor == null) == false)
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                //Dim Myfunc As New LiveMonitoring.SharedFuncs

                if (MyRem.LiveMonServer.DeleteCamera(Cursensor.ID))
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "Camera Successfully Deleted.";
                    //what now refesh
                    LoadDevices();
                }
                else
                {
                    errorMessage.Visible = true;
                    lblError.Text = "An error has occured during Delete, Please try again.";

                    cmbDevices.Focus();
                }
            }
        }
    }
}