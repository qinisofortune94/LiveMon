using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class ActiveDirectoryUnits : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.AppSettings["DataBaseCon"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Page.IsPostBack == false)
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

                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                    successMessage.Visible = false;
                    warningMessage.Visible = false;
                    errorMessage.Visible = false;
                    if (!IsPostBack)
                    {
                        drpSites.Items.Clear();
                        FillSites(drpSites);
                        drpMainSites.Items.Clear();
                        FillSites(drpMainSites);
                        chkDelete.Checked = false;
                    }
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
            
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            try
            {
                LoadUnits(Convert.ToInt32(drpMainSites.SelectedValue));
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error Loading Sites";
            }
            if (gridUnits.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridUnits.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridUnits.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridUnits.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridUnits.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;

                txtUnit.Text = gridUnits.Rows[myRow.RowIndex].Cells[2].Text;
                drpSites.SelectedItem.Text = gridUnits.Rows[myRow.RowIndex].Cells[3].Text;
                chkDelete.Checked = false;
                lblDelete.Visible = true;
                chkDelete.Visible = true;

                lblAdd.Text = "Update";
                btnSave.Text = "Update";
                successMessage.Visible = false;
            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                //Refresh Grid
            }

        }

        public void LoadUnits(int pintSiteId)
        {
            gridUnits.DataSource = getSites(pintSiteId);
            gridUnits.DataBind();
        }

        public void FillSites(DropDownList DropDownListSites)
        {
            string sqlQuery = "sitenames_select_all";
            SqlConnection connection = new SqlConnection(conStr);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                DropDownListSites.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownListSites.Items.Add(new ListItem(reader["SiteName"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error Connecting to the database.";
            }

        }

        private DataTable getSites(int pintSiteId)
        {
            try
            {
                string sqlQuery = "units_getUnit_BySiteID";
                SqlConnection connection = new SqlConnection(conStr);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("SiteId", pintSiteId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Error Connecting to the database.";
                return null;
            }
        }

        public bool CanSave()
        {
            if (txtUnit.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter unit name.";
                txtUnit.Focus();
                return false;
            }

            if (drpSites.SelectedItem == null)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select a valid site.";
                drpSites.Focus();
                return false;
            }

            return true;
        }

        public bool SaveUnit()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.UnitDetails NewUnit = new LiveMonitoring.IRemoteLib.UnitDetails();
            NewUnit.UnitName = txtUnit.Text.ToUpper();
            NewUnit.SiteId = Convert.ToInt32(drpSites.SelectedValue);
            bool Myresp = MyRem.LiveMonServer.AddNewUnit(NewUnit);

            return Myresp;
        }
        public bool EditUnit(int id)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.UnitDetails NewUnit = new LiveMonitoring.IRemoteLib.UnitDetails();
            NewUnit.UnitName = txtUnit.Text.ToUpper();
            NewUnit.ID = id;
            NewUnit.SiteId = Convert.ToInt32(drpSites.SelectedValue);
            NewUnit.isActive = chkDelete.Checked;

            bool Myresp = MyRem.LiveMonServer.EditUnit(NewUnit);

            return Myresp;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CanSave())
            {
                //Check button text to add or update
                string temp = btnSave.Text;

                if (temp.Contains("Add"))
                {
                    if (SaveUnit() == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Unit was successfully saved.";
                        txtUnit.Text = "";
                        drpSites.SelectedIndex = -1;

                        LoadUnits(Convert.ToInt32(drpMainSites.SelectedValue));
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while trying to save unit, please try again.";
                    }
                }
                else if (temp.Contains("Update"))
                {

                    int Id = int.Parse(ViewState["Id"].ToString());

                    if (EditUnit(Id) == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Unit was successfully saved.";
                        txtUnit.Text = "";
                        drpSites.SelectedIndex = -1;
                        LoadUnits(Convert.ToInt32(drpMainSites.SelectedValue));
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while trying to save Unit, please try again.";
                    }


                    btnSave.Text = "Add";
                    lblDelete.Visible = false;
                    chkDelete.Visible = false;
                }

                //Refresh Grid
                LoadUnits(Convert.ToInt32(drpMainSites.SelectedValue));
            }
        }

        protected void drpMainSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(Page.IsPostBack == false)
                {
                    LoadUnits(Convert.ToInt32(drpMainSites.SelectedValue));
                }   
            }
            catch (Exception ex)
            {
            }
        }

        public void Site_Units()
        {
            Load += Page_Load;
        }
    }
}