using Microsoft.VisualBasic;
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
    public partial class SiteSensors : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess myDA = new LiveMonitoring.DataAccess();
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
            if (gdvSensorsToSite.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gdvSensorsToSite.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gdvSensorsToSite.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gdvSensorsToSite.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gdvSensorsToSite.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;
            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                SqlParameter[] myParams = new SqlParameter[1];

                //@SITESENSORID
                myParams[0] = new System.Data.SqlClient.SqlParameter();
                myParams[0].ParameterName = "@SITESENSORID";
                myParams[0].Value = ViewState["Id"];
                try
                {
                    myDA.ExecCmdQueryParams("site_SensorRemoveBySiteSensorID", myParams);

                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "An error occured while removing sensor to site.";

                    Trace.Write("Website.SitesSensorSetup.removeSensorToSiteRelationship: Error: " + ex.Message.ToString());
                }

                //Refresh Grid
                ChangeSelectedListIndex();
            }

        }

        protected void lstSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectedListIndex();
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            string temp = btnAddSensor.Text;

            if (temp.Contains("Add Sensor"))
            {
                int sensorId = Convert.ToInt32(lstSensors.SelectedValue);
                int SiteId = Convert.ToInt32(lstSites.SelectedValue);
                SqlParameter[] myParams = new SqlParameter[2];
                try
                {
                    //@SITEID 
                    myParams[0] = new System.Data.SqlClient.SqlParameter();
                    myParams[0].ParameterName = "@SITEID";
                    myParams[0].Value = SiteId;

                    //@SENSORID
                    myParams[1] = new System.Data.SqlClient.SqlParameter();
                    myParams[1].ParameterName = "@SENSORID";
                    myParams[1].Value = sensorId;
                    myDA.ExecCmdQueryParams("site_SensorAddBySiteIDAndSensorID", myParams);
                    ChangeSelectedListIndex();
                    successMessage.Visible = true;
                    lblSucces.Text = "Sensor Successfuly added..";
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "An error occured while adding sensor to site.";

                    Trace.Write("Website.SitesSensorSetup.addSensorToSite: Error: " + ex.Message.ToString());
                }
            }   
        }

        public void ChangeSelectedListIndex()
        {
            try
            {
                var intSelectedSiteID = lstSites.SelectedValue;
                DataTable siteDT = new DataTable();
                siteDT = GetSitesSensorBySiteId(Convert.ToInt32(intSelectedSiteID));
                if ((siteDT != null))
                {
                    gdvSensorsToSite.DataSource = siteDT;
                    gdvSensorsToSite.DataBind();
                }
                else
                {
                    gdvSensorsToSite.DataSource = null;
                    gdvSensorsToSite.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Write("Website.SiteSensors.lstSites_SelectedIndexChanged: Error: " + ex.Message.ToString());
            }
        }

        public void populateListBox()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            List<LiveMonitoring.Sites.Site> sitesList = new List<LiveMonitoring.Sites.Site>();
            List<LiveMonitoring.IRemoteLib.SensorDetails> sensorList = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
            lstSites.Items.Clear();
            lstSensors.Items.Clear();
            try
            {
                sensorList = test.getAllSensors(lblError);
                foreach (LiveMonitoring.IRemoteLib.SensorDetails mySensor in sensorList)
                {
                    ListItem mynewItem = new ListItem();
                    mynewItem.Value = mySensor.ID.ToString();
                    mynewItem.Text = mySensor.Caption.ToString();
                    lstSensors.Items.Add(mynewItem);
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
                lblError.Text = "An error occured while populating ListBox..";

                Trace.Write("Website.SitesUsers.PopulateListBox: Error: " + ex.Message.ToString());
            }
        }

        public DataTable GetSitesSensorBySiteId(int intSiteId)
        {
            DataTable myDT = new DataTable();
            DataSet myDS = new DataSet();
            System.Data.SqlClient.SqlParameter[] myParams = new System.Data.SqlClient.SqlParameter[1];
            try
            {
                //@SITEID
                myParams[0] = new System.Data.SqlClient.SqlParameter();
                myParams[0].ParameterName = "@SITEID";
                myParams[0].Value = intSiteId;
                myDS = myDA.ExecCmdQueryParamsDS("site_SensorsBySiteID", myParams);
                myDT = myDS.Tables[0];
                //Ensure that there is information returned
                if (!(myDT.Rows.Count > 0))
                {
                    myDT = null;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error occured while getting sensors by site it.";
                Trace.Write("Website.SitesSensorSetup.GetSitesSensorBySiteId: Error: " + ex.Message.ToString());
                myDT = null;
            }
            return myDT;
        }

        public void SitesUserSetup()
        {
            Load += Page_Load;
        }
    }
}