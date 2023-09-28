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
    public partial class SensorAlertGroups : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
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

                        successMessage.Visible = false;
                        warningMessage.Visible = false;
                        errorMessage.Visible = false;

                        //ok logged on level ?
                        string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                        LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                        LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                        int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                        MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                        if (MyIPMonPageSecure > MyUser.UserLevel)
                        {
                            Response.Redirect("~/NotAuthorisedView.aspx");
                        }
                        divEdit.Visible = false;
                        DivNew.Visible = false;
                        LoadGroups();
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadGroups();
            if (gridContactGroups.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridContactGroups.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridContactGroups.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridContactGroups.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        public bool SaveSensorGroup()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            int SiteId = Convert.ToInt32(Session["SelectedSite"]);
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.GroupContacts NewContactGroup = new LiveMonitoring.IRemoteLib.GroupContacts();
            NewContactGroup.Name = txtGroup.Text.ToUpper();
            NewContactGroup.Description = txtDescription.Text.ToUpper();
            NewContactGroup.SiteID = SiteId;
            NewContactGroup.CapturedBy = MyUser.ID;
            int Myresp = MyRem.LiveMonServer.AddNewContactGroup(NewContactGroup);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        public bool EditSensorGroup(int id)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            int siteid = Convert.ToInt32(Session["SelectedSite"]);
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.GroupContacts NewContactGroup = new LiveMonitoring.IRemoteLib.GroupContacts();
            NewContactGroup.Name = txtGroup.Text.ToUpper();
            NewContactGroup.Description = txtDescription.Text.ToUpper();
            NewContactGroup.ID =id;

            int Myresp = MyRem.LiveMonServer.EditContactGroup(NewContactGroup);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                divEdit.Visible = false;
                DivNew.Visible = true;
                lblSuccess.Text = "";
                //lblAdd.Text = "";
                lblEditSuccess.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedEdit.aspx");
                }

                if (CanSave() == true)
                {
                    string temp = btnSave.Text;

                    if (temp.Contains("Add Group"))
                    {
                        if (SaveSensorGroup() == true)
                        {
                            try
                            {
                                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                MyRem.WriteLog("Contact group Saved", "User:" + MyUser.ID.ToString() + "|" + txtGroup.Text + "|" + txtDescription.Text);

                            }
                            catch (Exception ex)
                            {
                            }
                            successMessage.Visible = true;
                            lblSucces.Text = "Contact group was successfully saved.";
                            txtDescription.Text = "";
                            txtGroup.Text = "";

                            LoadGroups();
                        }
                        else
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while trying to save group, please try again.";
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        if (EditSensorGroup(Convert.ToInt32(ViewState["Id"])) == true)
                        {
                            successMessage.Visible = true;
                            lblSucces.Text = "Contact group was successfully updated.";
                            try
                            {
                                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                MyRem.WriteLog("Contact group Edited", "User:" + MyUser.ID.ToString() + "|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                            }
                            catch (Exception ex)
                            {
                            }

                            txtDescription.Text = "";
                            txtGroup.Text = "";
                            LoadGroups();
                            divEdit.Visible = false;
                            lblAdd.Text = "Add Sensor Alert Group";
                            btnSave.Text = "Add Group";
                        }
                        else
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while trying to save group, please try again.";
                        }                
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }
        public bool CanSave()
        {

            if (txtGroup.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter grouping name.";
                txtGroup.Focus();
                return false;
            }
            if (txtDescription.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter grouping description.";
                txtDescription.Focus();
                return false;
            }
            return true;
        }
        public bool CanEdit()
        {

            if (txtEditGroup.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter grouping name.";
                txtEditGroup.Focus();
                return false;
            }
            if (txtEditDescription.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter grouping description.";
                txtEditDescription.Focus();
                return false;
            }
            return true;
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridContactGroups.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                string Group = gridContactGroups.Rows[myRow.RowIndex].Cells[3].Text;
                string Description = gridContactGroups.Rows[myRow.RowIndex].Cells[4].Text;


                DivNew.Visible = true;
                AddGroups.Visible = true;

                ViewState["Id"] = Id;

                txtGroup.Text = Group;
                txtDescription.Text = Description;



                lblAdd.Text = "Update";
                btnSave.Text = "Update";

            }
            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedDelete.aspx");
                }
                try
                {
                    if (CanDelete() == true)
                    {
                        int intUserSiteId = 0;
                        SqlParameter[] myParams = new SqlParameter[1];

                        try
                        {
                            intUserSiteId = Convert.ToInt32(ViewState["Id"]);

                            //@SITEUSERID
                            myParams[0] = new SqlParameter();
                            myParams[0].ParameterName = "@ID";
                            myParams[0].Value = intUserSiteId;

                            MyDataAccess.ExecCmdQueryParams("group_contact_delete", myParams);
                            LoadGroups();
                        }
                        catch (Exception ex)
                        {
                            Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                        }
                        //Refresh Grid
                        LoadGroups();
                    }

                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message;
                }     
            }
        }

        public void LoadGroups()
        {
            gridContactGroups.DataSource = getGroups();
            gridContactGroups.DataBind();

        }
        private DataTable getGroups()
        {

            string sqlQuery = "Alerts_getAll_contactgroup";
            int SiteId = Convert.ToInt32(Session["SelectedSite"]);
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("SiteId", SiteId);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedEdit.aspx");
                }
                if (CanEdit() == true)
                {
                    if (EditSensorGroup(Convert.ToInt32(ViewState["Id"])) == true)
                    {
                        lblEditSuccess.Text = "Contact group was successfully saved.";
                        try
                        {
                            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                            MyRem.WriteLog("Contact group Edited", "User:" + MyUser.ID.ToString() + "|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                        }
                        catch (Exception ex)
                        {
                        }
                        txtEditDescription.Text = "";
                        txtEditGroup.Text = "";
                        LoadGroups();
                        divEdit.Visible = false;
                    }
                    else
                    {
                        lblEditSuccess.Text = "An error occured while trying to save group, please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditSuccess.Text = ex.Message;
            }
        }

        protected void gridSensorGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DivNew.Visible =true;
                divEdit.Visible = false;
                txtEditGroup.Text = gridContactGroups.SelectedRow.Cells[2].Text;
                txtEditDescription.Text = gridContactGroups.SelectedRow.Cells[3].Text;
            }
            catch (Exception ex)
            {
                lblEditSuccess.Text = ex.Message;
            }
        }

        protected void gridSensorGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridContactGroups.PageIndex = e.NewPageIndex;
                gridContactGroups.DataSource = getGroups();
                gridContactGroups.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedDelete.aspx");
            }
            try
            {
                if (CanDelete() == true)
                {
                    if (DeleteSensorGroup(Convert.ToInt32(ViewState["Id"])) == true)
                    {
                        try
                        {
                            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                            MyRem.WriteLog("Contact group Deleted", "User:" + MyUser.ID.ToString() + "|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                        }
                        catch (Exception ex)
                        {
                        }
                        lblEditSuccess.Text = "Contact group was Deleted.";
                        txtEditDescription.Text = "";
                        txtEditGroup.Text = "";
                        LoadGroups();
                        divEdit.Visible = false;
                    }
                    else
                    {
                        lblEditSuccess.Text = "An error occured while trying to Delete group, please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblEditSuccess.Text = ex.Message;
            }
        }
        public bool CanDelete()
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;
                if ((MyCollection == null))
                    return true;
                bool Refrenced = false;
                string RefrencedName = "";

                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            try
                            {
                                if (Convert.ToInt32(gridContactGroups.SelectedRow.Cells[1].Text) == Mysensor.SensGroup.SensorGroupID)
                                {
                                    Refrenced = true;
                                    RefrencedName += Mysensor.Caption + ",";
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }
                if (Refrenced)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Cannot delete ,Group is in use for " + RefrencedName;
                    txtEditGroup.Focus();
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return true;
        }
        public bool DeleteSensorGroup(int id)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            int Myresp = Convert.ToInt32(MyRem.LiveMonServer.DeleteSensorGroup(id));
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        public void ContactGroups_ContactGroups()
        {
            Load += Page_Load;
        }

        protected void btnAddNewSensorAlertgroup_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClearNewSensorAlertGroup_Click(object sender, EventArgs e)
        {
            txtDescription.Text = "";
            txtGroup.Text = "";
            lblSuccess.Text = "";
            lblAdd.Text = "";
            lblEditSuccess.Text = "";
        }

        protected void btnAddNew_Click1(object sender, EventArgs e)
        {
            txtDescription.Text = "";
            txtGroup.Text = "";
            lblSuccess.Text = "";
            lblAdd.Text = "";
            lblEditSuccess.Text = "";
        }
    }
}