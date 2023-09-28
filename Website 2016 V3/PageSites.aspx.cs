using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class PageSites : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.AppSettings["DataBaseCon"].ToString();
        byte[] pstrIcon = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack == false)
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

                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }

                try
                {
                    LoadDefaultIcons();

                }
                catch (Exception ex)
                {
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadSites();
            if (gridSites.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridSites.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridSites.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridSites.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridSites.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;


                try
                {
                    lblDelete.Visible = true;
                    chkDelete.Visible = true;
                    txtSite.Text = gridSites.Rows[myRow.RowIndex].Cells[2].Text; ;
                    try
                    {
                        LoadDefaultIconsEdit();

                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                    lblSuccess.Text = ex.Message;
                }

                lblAdd.Text = "Update";
                btnSave.Text = "Update";

            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                //Refresh Grid
                LoadSites();
            }

        }

        public void LoadSites()
        {
            gridSites.DataSource = getSites();
            gridSites.DataBind();
        }

        private DataTable getSites()
        {
            try
            {
                string sqlQuery = "[Sites].[spGetSites]";
                SqlConnection connection = new SqlConnection(conStr);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("GroupId", "LiveMon");
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return dt;
            }
            catch(Exception ex)
            {
                //errorMessage.Visible = true;
                lblSuccess.Text = ex.Message;
                return null;
            }
        }

        public void LoadDefaultIcons()
        {
            string sqlQuery = "select top(1) SiteIcon from DefaultIcons";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pstrIcon = (byte[])reader["SiteIcon"];
            }

            reader.Close();
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            //ASSIGN DEFAULT ICONS
            string base64StringpstrNormal = Convert.ToBase64String(pstrIcon, 0, pstrIcon.Length);
            imgIcon.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNormal;

        }

        public void LoadDefaultIconsEdit()
        {

            string sqlQuery = "select top(1) SiteIcon from DefaultIcons";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pstrIcon = (byte[])reader["SiteIcon"];
            }

            reader.Close();
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            //ASSIGN DEFAULT ICONS
            string base64StringpstrNormal = Convert.ToBase64String(pstrIcon, 0, pstrIcon.Length);
            //EditimgIcon.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNormal;

        }

        public bool CanSave()
        {
            if (txtSite.Text.Trim().Length == 0)
            {
                lblSuccess.Text = "Please enter site name.";
                txtSite.Focus();
                return false;
            }

            return true;
        }

        public bool SaveSite()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.SiteDetails NewSite = new LiveMonitoring.IRemoteLib.SiteDetails();
            NewSite.SiteName = txtSite.Text.ToUpper();
            byte[] byteIcon = null;
            System.Drawing.Image imgicon = null;

            //Icon
            try
            {
                if (filImageIcon.FileName.Trim().Length == 0)
                {
                    byteIcon = pstrIcon;
                }
                else
                {
                    imgicon = Myfunc.Strip_Image(this.filImageIcon);
                    byteIcon = MyRem.ImagetoByte(imgicon, ImageFormat.Bmp);
                }

            }
            catch
            {
            }
            NewSite.SiteIcon = byteIcon;
            bool Myresp = MyRem.LiveMonServer.AddNewSite(NewSite);

            return Myresp;
        }

        public bool EditSite(int id)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.SiteDetails NewSite = new LiveMonitoring.IRemoteLib.SiteDetails();
            NewSite.SiteName = txtSite.Text.ToUpper();
            NewSite.ID = id;
            byte[] byteIcon = null;
            System.Drawing.Image imgicon = null;

            //Icon
            try
            {
                if (filImageIcon.FileName.Trim().Length == 0)
                {
                    byteIcon = pstrIcon;
                }
                else
                {
                    imgicon = Myfunc.Strip_Image(this.filImageIcon);
                    byteIcon = MyRem.ImagetoByte(imgicon, ImageFormat.Bmp);
                }

            }
            catch
            {
            }
            NewSite.SiteIcon = byteIcon;
            bool Myresp = MyRem.LiveMonServer.EditSite(NewSite);

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
                    if (SaveSite() == true)
                    {
                        lblSuccess.Text = "Site was successfully saved.";
                        txtSite.Text = "";
                        LoadSites();
                    }
                    else
                    {
                        lblSuccess.Text = "An error occured while trying to save site, please try again.";
                    }
                }
                else if (temp.Contains("Update"))
                {

                    int Id = int.Parse(ViewState["Id"].ToString());

                    EditSite(Id);
                    btnSave.Text = "Add";
                }

                //Refresh Grid
                LoadSites();
            }

        }

        public void Site_Site_Site()
        {
            Load += Page_Load;
        }
    }
}