using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
using static LiveMonitoring.Sites;

namespace website2016V2
{
    public partial class SiteUsers : System.Web.UI.Page
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

                int intUserSiteId = 0;
                SqlParameter[] myParams = new SqlParameter[1];

                try
                {
                    intUserSiteId = Convert.ToInt32(ViewState["Id"]);

                    //@SITEUSERID
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@SITEUSERID";
                    myParams[0].Value = intUserSiteId;

                    MyDataAccess.ExecCmdQueryParams("site_usersRemoveUserToSite", myParams);
                    ChangeSelectedListIndex();
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
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
            string temp = btnAdd.Text;

            if (temp.Contains("Add User"))
            {

                int intUserId = 0;
                int intSiteId = 0;
                SqlParameter[] myParams = new SqlParameter[2];

                try
                {
                    intUserId = Convert.ToInt32(listUsers.SelectedValue);
                    intSiteId = Convert.ToInt32(lstSites.SelectedValue);

                    //@SITEID
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@SITEID";
                    myParams[0].Value = intSiteId;

                    //@USERID
                    myParams[1] = new SqlParameter();
                    myParams[1].ParameterName = "@USERID";
                    myParams[1].Value = intUserId;

                    //Set the SP requirements and values.
                    MyDataAccess.ExecCmdQueryParams("site_usersAddUserToSite", myParams);
                    ChangeSelectedListIndex();

                    successMessage.Visible = true;
                    lblSucces.Text = "User Successfuly added..";

                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUsers.btnAddUser_Click: Error: " + ex.Message.ToString());
                }
            }
        }

        public void ChangeSelectedListIndex()
        {
            try
            {
                var intSelectedSiteID = lstSites.SelectedValue;
                DataTable siteDT = new DataTable();
                siteDT = GetSiteUsersBySiteId(Convert.ToInt32(intSelectedSiteID));
                if ((siteDT != null))
                {
                    gdvUsersToSite.DataSource = siteDT;
                    gdvUsersToSite.DataBind();
                }
                else
                {
                    gdvUsersToSite.DataSource = null;
                    gdvUsersToSite.DataBind();
                }
            }
            catch (Exception ex)
            {
                Trace.Write("Website.SitesUserSetup.lstSites_SelectedIndexChanged: Error: " + ex.Message.ToString());
            }
        }

        public void getAllSitesUsers()
        {
            System.Data.SqlClient.SqlDataReader MySQLReader = null;
            MySQLReader = MyDataAccess.ExecCmdQueryNoParams("users_sitesAll");
          
            try
            {
                if ((MySQLReader == null) == false)
                {
                    while (MySQLReader.Read())
                    {
                        try
                        {
                            int Siteid = 0;
                            if (!DBNull.Value.Equals(MySQLReader["SITEID"]))
                            {
                                Siteid = Convert.ToInt32(MySQLReader["SITEID"]);
                            }
                            //if (Information.IsDBNull(MySQLReader["SITEID"]) == false)
                            //{
                            //    Siteid = Convert.ToInt32(MySQLReader["SITEID"]);
                            //}
                            Site MySite = new Site(Siteid);
                            if ((MySite == null) == false)
                            {
                                if (ListOfSites.Contains(MySite) == false)
                                {
                                    ListOfSites.Add(MySite);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while geting all Sites Users.";

                            Trace.Write("Website.SitesUserSetup.GetAllSitesUsers: Error: " + ex.Message.ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error occured while geting all Sites Users.";

                Trace.Write("Website.SitesUserSetup.GetAllSitesUsers: Error: " + ex.Message.ToString());
            }
            finally
            {
                MySQLReader.Close();
                MySQLReader = null;
            }
        }

        public DataTable GetSiteUsersBySiteId(int intSiteId)
        {
            DataSet myDS = new DataSet();
            DataTable myDt = new DataTable();
            SqlParameter[] myParams = new SqlParameter[1];
            myParams[0] = new SqlParameter();
            myParams[0].ParameterName = "@ID";
            myParams[0].Value = intSiteId;
            try
            {
                myDS = MyDataAccess.ExecCmdQueryParamsDS("site_usersBySiteId", myParams);
                //sp
                myDt = myDS.Tables[0];
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
                lblError.Text = "An error occured while geting all Sites Users.";
                Trace.Write("Website.SitesUsers.GetAllSitesUsers: Error: " + ex.Message.ToString());
                return null;
            }
        }

        public void populateListBox()
        {
            LiveMonitoring.testing test = new LiveMonitoring.testing();
            List<LiveMonitoring.Sites.Site> sitesList = new List<LiveMonitoring.Sites.Site>();
            List<LiveMonitoring.IRemoteLib.UserDetails> usersList = new List<LiveMonitoring.IRemoteLib.UserDetails>();
            lstSites.Items.Clear();
            listUsers.Items.Clear();
            try
            {
                //Get users and populate the user listbox.
                usersList = getAllUsers();
                foreach (LiveMonitoring.IRemoteLib.UserDetails myUser in usersList)
                {
                    ListItem mynewItem = new ListItem();
                    mynewItem.Value = myUser.ID.ToString();
                    mynewItem.Text = myUser.FirstName + " " + myUser.SurName;
                    listUsers.Items.Add(mynewItem);
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

        public List<LiveMonitoring.IRemoteLib.UserDetails> getAllUsers()
        {
            List<LiveMonitoring.IRemoteLib.UserDetails> ListOfAllUsers = new List<LiveMonitoring.IRemoteLib.UserDetails>();
            DataTable myDT = new DataTable();
            DataSet myDS = new DataSet();
            myDS = MyDataAccess.ExecCmdQueryNoParamsDS("users_select_all");
            myDT = myDS.Tables[0];
            if (myDT.Rows.Count > 0)
            {
                for (int counter = 0; counter <= myDT.Rows.Count - 1; counter++)
                {
                    LiveMonitoring.IRemoteLib.UserDetails newUserDetails = new LiveMonitoring.IRemoteLib.UserDetails();
                    var _with1 = newUserDetails;
                    _with1.FirstName = myDT.Rows[counter]["FirstName"].ToString();
                    _with1.SurName = myDT.Rows[counter]["SurName"].ToString();
                    _with1.ID = (int)myDT.Rows[counter]["ID"];
                    ListOfAllUsers.Add(newUserDetails);
                }
            }
            return ListOfAllUsers;
        }
        public void SitesUserSetup()
        {
            Load += Page_Load;
        }
    }
}