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

        private List<LiveMonitoring.IRemoteLib.SensorDetails> getAllSensors()
        {
            DataSet myDS = new DataSet();
            DataTable myDT = new DataTable();
            List<LiveMonitoring.IRemoteLib.SensorDetails> SensorList = new List<LiveMonitoring.IRemoteLib.SensorDetails>();
            try
            {
                myDS = myDA.ExecCmdQueryNoParamsDS("sensor_select_all");
                myDT = myDS.Tables[0];
                //is there any rows returned.
                if (myDT.Rows.Count > 0)
                {

                    for (int intCount = 0; intCount <= myDT.Rows.Count - 1; intCount++)
                    {
                        //New sensor to be added to the list of sensors
                        LiveMonitoring.IRemoteLib.SensorDetails newSensorDetail = new LiveMonitoring.IRemoteLib.SensorDetails();

                        //Initialize new sensor
                        var _with1 = newSensorDetail;
                        if ((myDT.Rows[intCount]["ID"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ID"]))
                        {
                            _with1.ID = (int)myDT.Rows[intCount]["ID"];
                        }

                        if ((myDT.Rows[intCount]["Type"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["Type"]))
                        {
                            _with1.Type = (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)(int)myDT.Rows[intCount]["Type"];
                        }

                        if ((myDT.Rows[intCount]["IPDeviceID"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["IPDeviceID"]))
                        {
                            _with1.IPDeviceID = (int)myDT.Rows[intCount]["IPDeviceID"];
                        }
                        if ((myDT.Rows[intCount]["Module"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["Module"]))
                        {
                            _with1.ModuleNo = (int)myDT.Rows[intCount]["Module"];
                        }
                        if ((myDT.Rows[intCount]["Register"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["Register"]))
                        {
                            _with1.Register = (int)myDT.Rows[intCount]["Register"];
                        }
                        if ((myDT.Rows[intCount]["SerialNumber"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["SerialNumber"]))
                        {
                            _with1.SerialNumber = (string)myDT.Rows[intCount]["SerialNumber"];
                        }
                        if ((myDT.Rows[intCount]["LastValue"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["LastValue"]))
                        {
                            _with1.LastValue = (double)myDT.Rows[intCount]["LastValue"];
                        }
                        if ((myDT.Rows[intCount]["LastValueDt"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["LastValueDt"]))
                        {
                            _with1.LastValueDt = (DateTime)myDT.Rows[intCount]["LastValueDt"];
                        }
                        if ((myDT.Rows[intCount]["Caption"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["Caption"]))
                        {
                            _with1.Caption = (string)myDT.Rows[intCount]["Caption"];
                        }
                        if ((myDT.Rows[intCount]["ImageNormal"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ImageNormal"]))
                        {
                            _with1.ImageNormalByte = (byte[])myDT.Rows[intCount]["ImageNormal"];
                        }
                        if ((myDT.Rows[intCount]["ImageError"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ImageError"]))
                        {
                            _with1.ImageErrorByte = (byte[])myDT.Rows[intCount]["ImageError"];
                        }

                        if ((myDT.Rows[intCount]["ImageNoResponse"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ImageNoResponse"]))
                        {
                            _with1.ImageNoResponseByte = (byte[])myDT.Rows[intCount]["ImageNoResponse"];
                        }
                        if ((myDT.Rows[intCount]["MinValue"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["MinValue"]))
                        {
                            _with1.MinValue = (double)myDT.Rows[intCount]["MinValue"];
                        }

                        if ((myDT.Rows[intCount]["MaxValue"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["MaxValue"]))
                        {
                            _with1.MaxValue = (double)myDT.Rows[intCount]["MaxValue"];
                        }
                        if ((myDT.Rows[intCount]["Multiplier"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["Multiplier"]))
                        {
                            _with1.Multiplier = (double)myDT.Rows[intCount]["Multiplier"];
                        }
                        if ((myDT.Rows[intCount]["Divisor"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["Divisor"]))
                        {
                            _with1.Divisor = (double)myDT.Rows[intCount]["Divisor"];
                        }
                        if ((myDT.Rows[intCount]["ScanRate"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ScanRate"]))
                        {
                            _with1.ScanRate = (double)myDT.Rows[intCount]["ScanRate"];
                        }
                        if ((myDT.Rows[intCount]["OutputType"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["OutputType"]))
                        {
                            _with1.OutputType = (LiveMonitoring.IRemoteLib.SensorDetails.OutputTypeDef)myDT.Rows[intCount]["OutputType"];
                        }
                        if ((myDT.Rows[intCount]["SiteID"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["SiteID"]))
                        {
                            _with1.SiteID = (int)myDT.Rows[intCount]["SiteID"];
                        }
                        if ((myDT.Rows[intCount]["SiteCritical"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["SiteCritical"]))
                        {
                            _with1.SiteCritical = (int)myDT.Rows[intCount]["SiteCritical"];
                        }
                        if ((myDT.Rows[intCount]["ExtraData"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ExtraData"]))
                        {
                            _with1.ExtraData = (string)myDT.Rows[intCount]["ExtraData"];
                        }
                        if ((myDT.Rows[intCount]["ExtraData1"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ExtraData1"]))
                        {
                            _with1.ExtraData1 = (string)myDT.Rows[intCount]["ExtraData1"];
                        }
                        if ((myDT.Rows[intCount]["ExtraData2"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ExtraData2"]))
                        {
                            _with1.ExtraData2 = (string)myDT.Rows[intCount]["ExtraData2"];
                        }

                        if ((myDT.Rows[intCount]["ExtraData3"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ExtraData3"]))
                        {
                            _with1.ExtraData3 = (string)myDT.Rows[intCount]["ExtraData3"];
                        }
                        if ((myDT.Rows[intCount]["ExtraValue"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ExtraValue"]))
                        {
                            _with1.ExtraValue = (double)myDT.Rows[intCount]["ExtraValue"];
                        }
                        if ((myDT.Rows[intCount]["ExtraValue1"] != null) & !Information.IsDBNull(myDT.Rows[intCount]["ExtraValue1"]))
                        {
                            _with1.ExtraValue1 = (double)myDT.Rows[intCount]["ExtraValue1"];
                        }
                        SensorList.Add(newSensorDetail);
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error occured while getting all sensors.";

                Trace.Write("Website.SitesSensorSetup.GetAllSensors: Error: " + ex.Message.ToString());
            }
            return SensorList;
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
                //roger removed as this is not implemented
                //sensorList = getAllSensors();
                
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