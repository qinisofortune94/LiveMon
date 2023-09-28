using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static LiveMonitoring.Sites;

namespace website2016V2
{
    public partial class SiteCamerasSetup : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        List<Site> ListOfSites = new List<Site>();
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

                if (!IsPostBack)
                {
                    populateListBox();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (gdvUsersToSite.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gdvUsersToSite.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gdvUsersToSite.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gdvUsersToSite.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gdvUsersToSite.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;

            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                //Remove the selected row from the usersites table.
                int intCameraSiteId = 0;
                SqlParameter[] myParams = new SqlParameter[1];

                try
                {
                    //Get the selected SiteUserId from the datagrid.
                    intCameraSiteId = Convert.ToInt32(ViewState["Id"]);

                    //@SITECAMERAID
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@ID";
                    myParams[0].Value = intCameraSiteId;

                    //Set the SP requirements and values.
                    MyDataAccess.ExecCmdQueryParams("spSitesCameraRemoveCameraFromSite", myParams);

                    //Call the gdvSiteUsers method
                    ChangeSelectedListIndex();
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesCameraSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }

                //Refresh Grid
                //LoadPeople();
            }
        }

        protected void lstSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedListIndex();
        }

        protected void btnAddCamera_Click(object sender, EventArgs e)
        {
            string temp = btnAddCamera.Text;
            if (temp.Contains("Add Camera"))
            {
                //Add the selected Camera to the site.
                int intCameraId = 0;
                // Camera id
                int intSiteId = 0;
                // site id
                SqlParameter[] myParams = new SqlParameter[2];

                try
                {
                    //Set values for the vars.
                    intCameraId = Convert.ToInt32(lstCameras.SelectedValue);
                    intSiteId = Convert.ToInt32(lstSites.SelectedValue);

                    //@SITEID
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@SITEID";
                    myParams[0].Value = intSiteId;

                    //@CAMERAID
                    myParams[1] = new SqlParameter();
                    myParams[1].ParameterName = "@CAMERAID";
                    myParams[1].Value = intCameraId;

                    //Set the SP requirements and values.
                    MyDataAccess.ExecCmdQueryParams("spSitesCamerasAddCameraToSite", myParams);

                    //Call the gdvSiteCameras method
                    ChangeSelectedListIndex();

                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesCamerasSetup.btnAddCamera_Click: Error: " + ex.Message.ToString());
                }
            } 
        }

        public LiveMonitoring.IRemoteLib.UserDetails getUser()
        {
            LiveMonitoring.IRemoteLib.UserDetails loggedInUser = new LiveMonitoring.IRemoteLib.UserDetails();
            loggedInUser = null;
            if ((Session["LoggedIn"] != null))
            {
                if ((string)Session["LoggedIn"] == "True")
                {
                    loggedInUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["LoggedIn"];
                }
            }
            return loggedInUser;
        }

        public void ChangeSelectedListIndex()
        {
            try
            {
                int intSelectedSiteID = Convert.ToInt32(lstSites.SelectedValue);
                DataTable siteDT = new DataTable();
                siteDT = GetSiteCamerasBySiteId(intSelectedSiteID);
                //if ((siteDT != null))
                //{
                    gdvUsersToSite.DataSource = siteDT;
                    gdvUsersToSite.DataBind();
                //}
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message.ToString();

                Trace.Write("Website.SitesCameraSetup.lstSites_SelectedIndexChanged: Error: " + ex.Message.ToString());
            }
        }

        public void populateListBox()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            List<LiveMonitoring.Sites.Site> sitesList = new List<LiveMonitoring.Sites.Site>();
            List<Camera> CamerasList = new List<Camera>();
            lstSites.Items.Clear();
            lstCameras.Items.Clear();
            try
            {
                //Get Cameras and populate the Camera listbox.
                CamerasList = getAllCameras();
                foreach (Camera myCamera in CamerasList)
                {
                    ListItem mynewItem = new ListItem();
                    mynewItem.Value = myCamera.ID.ToString();
                    mynewItem.Text = myCamera.Caption;
                    lstCameras.Items.Add(mynewItem);
                }
                //Get sites and populate the site listbox.
                sitesList = test.getAllSites(lblError);
                foreach (LiveMonitoring.Sites.Site mysite in sitesList)
                {
                    ListItem mynewitem = new ListItem();
                    mynewitem.Value = mysite.SiteObj.SiteID.ToString();
                    mynewitem.Text = mysite.SiteObj.SiteName;
                    lstSites.Items.Add(mynewitem);
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message.ToString();

                Trace.Write("Website.SitesCameraSetup.PopulateListBox: Error: " + ex.Message.ToString());
            }
        }

        public DataTable GetSiteCamerasBySiteId(int intSiteId)
        {
            DataSet myDS = new DataSet();
            DataTable myDt = new DataTable();
            SqlParameter[] myParams = new SqlParameter[1];
            myParams[0] = new SqlParameter();
            myParams[0].ParameterName = "@SITEID";
            myParams[0].Value = intSiteId;
            try
            {
                myDS = MyDataAccess.ExecCmdQueryParamsDS("spSitesCamerasBySiteId", myParams);
                //sp
                myDt = myDS.Tables[0];
                //Get the first table of information.
                if (myDt.Rows.Count > 0 == true)
                {
                    return myDt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message.ToString();

                Trace.Write("Website.SitesCameraSetup.GetAllSitesCameras: Error: " + ex.Message.ToString());
                return null;
            }
        }

        public List<Camera> getAllCameras()
        {
            List<Camera> ListOfAllCameras = new List<Camera>();
            DataTable myDT = new DataTable();
            DataSet myDS = new DataSet();
            myDS = MyDataAccess.ExecCmdQueryNoParamsDS("spCamerasGetAll");
            myDT = myDS.Tables[0];
            if (myDT.Rows.Count > 0)
            {
                for (int counter = 0; counter <= myDT.Rows.Count - 1; counter++)
                {
                    Camera newCameraDetails = new Camera();
                    var _with1 = newCameraDetails;
                    _with1.Caption = myDT.Rows[counter]["Caption"].ToString();
                    _with1.IPAdress = myDT.Rows[counter]["IPAdress"].ToString();
                    _with1.ID = (int)myDT.Rows[counter]["ID"];
                    ListOfAllCameras.Add(newCameraDetails);
                }
            }
            return ListOfAllCameras;
        }

        public void SitesCamerasSetup()
        {
            Load += Page_Load;
        }
    }
}