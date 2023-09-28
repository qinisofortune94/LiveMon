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
using System.Web.Configuration;
using System.Data.SqlClient;


namespace website2016V2
{
    
    public partial class AlertGroupLinkContacts : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        private int MyAlertId;
        LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
        int SiteId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                if (((string)Session["LoggedIn"] == "True"))
                {
                    LiveMonitoring.IRemoteLib.UserDetails MyUser1 = new LiveMonitoring.IRemoteLib.UserDetails();
                    MyUser1 = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    Label user = this.Master.FindControl("lblUser") as Label;
                    Label loginUser = this.Master.FindControl("lblUser2") as Label;
                    Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                    loginUser.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                    user.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                    LastLogin.Text = " LL:" + MyUser1.LoginDT.ToString();

                    //ok logged on level ?
                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    if (MyIPMonPageSecure > MyUser.UserLevel)
                    {
                        Response.Redirect("NotAuthorisedView.aspx");
                    }
                    SiteId = Convert.ToInt32(Session["SelectedSite"]);
                    LoadContactLink();
                    cmbGroups.Items.Clear();
                    Myfunc.FillContactGroups(cmbGroups, SiteId, conStr);
                    cmbGroupMain.Items.Clear();
                    Myfunc.FillContactGroups(cmbGroupMain, SiteId, conStr);
                    cmbGroupMain.SelectedIndex = -1;
                    //DivNew.Visible = false;
                    cmbContacts.Items.Clear();
                    Myfunc.FillAlertContacts(cmbContacts, SiteId, conStr);
                    //cmbContacts.SelectedIndex = -1
                    DeleteContactGroupLinkTemp();
                    //GridContactLink.Visible = true;
                    //GridContactLink.Visible = true;
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
                
            }
            
                
        }

        protected void GridView1_PreRender2(object sender, EventArgs e)
        {
            LoadTempContactLink();
            if (GridContactLinkTemp.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                GridContactLinkTemp.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                GridContactLinkTemp.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                GridContactLinkTemp.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadContactLink();
            if (GridContactLink.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                GridContactLink.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                GridContactLink.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                GridContactLink.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = GridContactLink.DataKeys[myRow.RowIndex].Value.ToString();
            //string Id = GridContactLinkTemp.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                //string GroupID = gridSensorGroups.Rows[myRow.RowIndex].Cells[2].Text;
                //string Group = gridSensorGroups.Rows[myRow.RowIndex].Cells[3].Text;
                //string Description = gridSensorGroups.Rows[myRow.RowIndex].Cells[4].Text;



                //ViewState["Id"] = Id;

               
                //txtGroup.Text = Group;
                //txtDescription.Text = Description;


                //lblAdd.Text = "Update";
                //btnSave.Text = "Update";

            }

            else if (commandName == "RemoveItem")
            {

                ViewState["Id"] = Id;

                //SampleLogic business = new SampleLogic();

                //int RecordId = Convert.ToInt16(Id);
                //business.Delete(RecordId);

                ////Refresh Grid
                //LoadData();
            }
        }

        protected void gvSample_Commands2(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = GridContactLinkTemp.DataKeys[myRow.RowIndex].Value.ToString();
            //string Id = GridContactLinkTemp.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                try
                {
                    lblSuccess.Text = "";
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    if (GridContactLinkTemp.Rows.Count > 0)
                    {
                        int i = 0;

                        for (i = 0; i <= GridContactLinkTemp.Rows.Count - 1; i++)
                        {

                            if (Myfunc.AddContactLink(Convert.ToInt32(GridContactLinkTemp.Rows[myRow.RowIndex].Cells[6].Text), Convert.ToInt32(GridContactLinkTemp.Rows[myRow.RowIndex].Cells[2].Text), MyUser.ID, conStr) == true)
                            {
                            }
                            else
                            {
                                lblSuccess.Text = "System failed to connect to the remote database, please check your connection and try again!";
                            }

                        }
                        lblSuccess.Text = GridContactLinkTemp.Rows.Count.ToString() + " records were successfully linked.";
                        DeleteContactGroupLinkTemp();
                        LoadTempContactLink();
                        LoadContactLink();
                    }

                }
                catch (Exception ex)
                {
                    lblSuccess.Text = ex.Message;
                }

            }

            else if (commandName == "RemoveItem")
            {

                ViewState["Id"] = Id;

                int intUserSiteId = 0;
                SqlParameter[] myParams = new SqlParameter[1];

                try
                {
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    intUserSiteId = MyUser.ID;

                    //@SITEUSERID
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@UserId";
                    myParams[0].Value = intUserSiteId;

                    MyDataAccess.ExecCmdQueryParams("Alerts_contactgrouplinktemp_DeleteAll", myParams);
                    LoadTempContactLink();
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
            }
        }

        public bool DeleteContactGroupLinkTemp()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.Int);


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "Alerts_contactgrouplinktemp_DeleteAll";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramUserID).Value = MyUser.ID;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
            }

            return Saved;
        }
        protected void GridContactLinkTemp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteContactGroupLinkTemp();
        }

        public bool DeleteSpecificContactLink(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "Alerts_contactgrouplink_DeleteSpecific";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
                return Saved;
            }

            return Saved;
        }
        public bool DeleteSpecificContactLinkTemp(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "Alerts_contactgrouplinktem_DeleteSpecific";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
                return Saved;
            }

            return Saved;
        }
        public void LoadTempContactLink()
        {
            GridContactLinkTemp.DataSource = getTempContactLink();
            GridContactLinkTemp.DataBind();

        }
        public void LoadContactLink()
        {
            int pintGroupId = 0;

            if (cmbGroupMain.SelectedItem != null)
            {
                GridContactLink.DataSource = getContactLink(Convert.ToInt32(cmbGroupMain.SelectedValue));
                GridContactLink.DataBind();
            }
            else
            {
                GridContactLink.DataSource = getContactLink(pintGroupId);
                GridContactLink.DataBind();
            }

        }
        private DataTable getTempContactLink()
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];


            string sqlQuery = "Alerts_contactgrouplinktem_GetAll";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("UserID", MyUser.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }
        private DataTable getContactLink(int pintGroupId)
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];


            string sqlQuery = "Alerts_contactgrouplink_GetAll";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("GroupId", pintGroupId);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblSuccessAdd.Text = "";
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (cmbContacts.SelectedItem != null | cmbGroups.SelectedItem != null)
            {
                if (Myfunc.AddContactLinkTemp(Convert.ToInt32(cmbGroups.SelectedValue),Convert.ToInt32(cmbContacts.SelectedValue), MyUser.ID, conStr, cmbGroups.SelectedItem.Text, cmbContacts.SelectedItem.Text) == true)
                {
                    ContactName.Text = cmbContacts.SelectedItem.Text;
                    LoadTempContactLink();
                }
                else
                {
                    lblSuccessAdd.Text = "System failed to connect to the remote database, please check your connection and try again!";
                }
            }
            else
            {
                lblSuccessAdd.Text = "please select a valid group and contact!";
            }
        }

        
        protected void GridContactLinkTemp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(DeleteSpecificContactLinkTemp(Convert.ToInt32(GridContactLinkTemp.SelectedRow.Cells[3].Text)));
                LoadTempContactLink();
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        protected void btnLinkContact_Click(object sender, EventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (GridContactLinkTemp.Rows.Count > 0)
                {
                    int i = 0;

                    for (i = 0; i <= GridContactLinkTemp.Rows.Count - 1; i++)
                    {

                        if (Myfunc.AddContactLink(Convert.ToInt32(GridContactLinkTemp.Rows[i].Cells[5].Text),Convert.ToInt32(GridContactLinkTemp.Rows[i].Cells[1].Text), MyUser.ID, conStr) == true)
                        {
                        }
                        else
                        {
                            lblSuccess.Text = "System failed to connect to the remote database, please check your connection and try again!";
                        }

                    }
                    lblSuccess.Text = GridContactLinkTemp.Rows.Count.ToString() + " records were successfully linked.";
                    DeleteContactGroupLinkTemp();
                    LoadTempContactLink();
                    LoadContactLink();
                }

            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }


        protected void cmbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ContactName.Visible = false;
            DivNew.Visible = true;
            GridContactLink.Visible = false;
        }

        protected void cmbGroupMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DivNew.Visible = false;
                GridContactLink.Visible = true;
                LoadContactLink();

            }
            catch (Exception ex)
            {
            }
        }

        protected void GridContactLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DeleteSpecificContactLink(Convert.ToInt32(GridContactLink.SelectedRow.Cells[1].Text));
                LoadContactLink();
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        protected void GridContactLink_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                int pintGroupId = 0;
                if (cmbGroupMain.SelectedItem != null)
                {
                    GridContactLink.PageIndex = e.NewPageIndex;
                    GridContactLink.DataSource = getContactLink(Convert.ToInt32(cmbGroupMain.SelectedValue));
                    GridContactLink.DataBind();
                }
                else
                {
                    GridContactLink.PageIndex = e.NewPageIndex;
                    GridContactLink.DataSource = getContactLink(pintGroupId);
                    GridContactLink.DataBind();
                }


            }
            catch (Exception ex)
            {
            }
        }
        public void Alerts_ContactGroups_BulkLinkContacts()
        {
            Load += Page_Load;
        }

        protected void BtnLinkContact_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {

        }
    }
}