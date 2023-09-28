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
    public partial class SiteSNPDevices : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.AppSettings["DataBaseCon"].ToString();
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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;
                if (Page.IsPostBack == false)
                {
                    drpViewSite.Items.Clear();
                    FillSites(drpViewSite, MyUser.ID.ToString());
                    drpSite.Items.Clear();
                    FillSites(drpSite, MyUser.ID.ToString());
                    drpDevices.Items.Clear();
                    FillOtherDevices(drpDevices);
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadDevices(Convert.ToInt32(drpViewSite.SelectedValue)); ;
            if (gridDevices.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridDevices.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridDevices.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridDevices.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridDevices.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;

                int one = 1;
                drpDevices.SelectedItem.Text = one.ToString();
                drpSite.SelectedItem.Text = one.ToString();
                //lblEditSuccess.Text = "";
                drpSite.SelectedItem.Text = gridDevices.Rows[myRow.RowIndex].Cells[2].Text;
                drpDevices.SelectedItem.Text = gridDevices.Rows[myRow.RowIndex].Cells[3].Text;


                lblAdd.Text = "Update";
                btnSave.Text = "Update";
                chkDelete.Visible = true;
                lblDelete.Visible = true;

            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                //Refresh Grid
                LoadDevices(Convert.ToInt32(drpViewSite.SelectedValue));
            }
        }

        public void LoadDevices(int siteid)
        {
            gridDevices.DataSource = getDevices(siteid);
            gridDevices.DataBind();

        }
        private DataTable getDevices(int siteId)
        {
            string sqlQuery = "[Sites].[spGetAllSNMPDevices]";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("SiteId", siteId);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        public void FillSites(DropDownList DropDownListSites, string pintGroupID)
        {
            string sqlQuery = "[Sites].[spGetSites]";
            SqlConnection connection = new SqlConnection(conStr);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("GroupId", pintGroupID);
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
                    DropDownListSites.Items.Add(new ListItem(reader["Site"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }

        }

        public void FillOtherDevices(DropDownList DropDownLisDevices)
        {
            string sqlQuery = "[Sites].[spGetSNMPDevices]";
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
                DropDownLisDevices.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropDownLisDevices.Items.Add(new ListItem(reader["Device"].ToString(), reader["ID"].ToString()));
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void drpViewSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDevices(Convert.ToInt32(drpViewSite.SelectedValue));
            }
            catch (Exception ex)
            {
            }
        }

        public bool canSave()
        {
            if (drpSite.SelectedItem == null)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select a valid site!";
                drpSite.Focus();
                return false;
            }

            if (drpDevices.SelectedItem == null)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please select a valid device!";
                drpDevices.Focus();
                return false;
            }
            return true;
        }

        public bool SaveSiteDevices()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramSiteId = new SqlParameter("@SiteId", SqlDbType.Int);
                SqlParameter paramDeviceId = new SqlParameter("@DeviceId", SqlDbType.Int);
                SqlParameter paramCapturedBy = new SqlParameter("@CapturedBy", SqlDbType.NVarChar);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "[Sites].[spAddSNMPDevices]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(paramSiteId).Value = drpSite.SelectedValue;
                cmd.Parameters.Add(paramDeviceId).Value = drpDevices.SelectedValue;
                cmd.Parameters.Add(paramCapturedBy).Value = MyUser.ID;
                cmd.ExecuteNonQuery();
                Saved = true;

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
            return Saved;
        }

        public bool EditSiteDevices(int id)
        {
            bool Saved = false;

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                SqlParameter paramSiteId = new SqlParameter("@SiteId", SqlDbType.Int);
                SqlParameter paramDeviceId = new SqlParameter("@DeviceId", SqlDbType.Int);
                SqlParameter paramisActive = new SqlParameter("@isActive", SqlDbType.Bit);
                SqlParameter paramCapturedBy = new SqlParameter("@CapturedBy", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "[Sites].[spEditSNMPDevices]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(paramID).Value = id;
                cmd.Parameters.Add(paramSiteId).Value = drpSite.SelectedValue;
                cmd.Parameters.Add(paramDeviceId).Value = drpDevices.SelectedValue;
                cmd.Parameters.Add(paramCapturedBy).Value = MyUser.ID;
                cmd.Parameters.Add(paramisActive).Value = chkDelete.Checked;
                cmd.ExecuteNonQuery();
                Saved = true;

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                Saved = false;
                errorMessage.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
            return Saved;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string temp = btnSave.Text;

            if (temp.Contains("Add"))
            {
                lblSuccess.Text = "";
                if (canSave() == true)
                {
                    if (SaveSiteDevices() == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Site SNMP Device successfully saved!";
                        //AddNew.Visible = false;
                        drpSite.SelectedIndex = -1;
                        drpDevices.SelectedIndex = -1;
                        LoadDevices(Convert.ToInt32(drpViewSite.SelectedValue));
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while saving device please try again later.";
                    }
                }
            }
            else if (temp.Contains("Update"))
            {
                int Id = int.Parse(ViewState["Id"].ToString());
                try
                {
                    lblSuccess.Text = "";
                    if (canSave() == true)
                    {
                        if (EditSiteDevices(Id) == true)
                        {
                            successMessage.Visible = true;
                            lblSuccess.Text = "Site SNMP Device successfully updated!";
                            //Edit.Visible = false;
                            drpSite.SelectedIndex = -1;
                            drpDevices.SelectedIndex = -1;
                            LoadDevices(Convert.ToInt32(drpViewSite.SelectedValue));
                        }
                        else
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while saving device please try again later.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message.ToString();
                }
                btnSave.Text = "Add";
                lblDelete.Visible = false;
                chkDelete.Visible = false;
            }

            //Refresh Grid
            LoadDevices(Convert.ToInt32(drpViewSite.SelectedValue));
        }

        public void Site_SNMPDevices_SitesSNMPDevicessetup()
        {
            Load += Page_Load;
        }
    }
}