using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace website2016V2
{
    partial class SQLReportsConfig : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
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
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("~/NotAuthorisedView.aspx");
                }
                divEdit.Visible = false;
                DivNew.Visible = false;
                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;
                LoadSettings();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadSettings();
            if (gvReports.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvReports.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvReports.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvReports.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


        public void LoadSettings()
        {
            gvReports.DataSource = getSettings();
            gvReports.DataBind();

        }

        private DataTable getSettings()
        {

            string sqlQuery = "reports_get_report";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                divEdit.Visible = false;
                DivNew.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        protected void gvReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DivNew.Visible = false;
                divEdit.Visible = true;
                txtEditName.Text = gvReports.SelectedRow.Cells[2].Text;
                txtEditDescription.Text = gvReports.SelectedRow.Cells[3].Text;
                txtEditUrl.Text = gvReports.SelectedRow.Cells[4].Text;
                chkisDeleted.Checked = false;
                lblSuccess.Text = "";
                lblEditSuccess.Text = "";
            }
            catch (Exception ex)
            {
                lblEditSuccess.Text = ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanSave() == true)
                {
                    if (SaveSettings() == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Report settings were successfully saved.";
                        Clear();
                        LoadSettings();
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while trying to save settings, please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }
     
        public void Clear()
        {
            txtDescription.Text = "";
            txtName.Text = "";
            txtUrl.Text = "";
        }
        public bool CanSave()
        {

            if (txtName.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter report name.";
                txtName.Focus();
                return false;
            }
            if (txtDescription.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter report description.";
                txtDescription.Focus();
                return false;
            }
            if (txtUrl.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter report url.";
                txtUrl.Focus();
                return false;
            }
            return true;
        }
        public bool SaveSettings()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.ReportSettings NewSettings = new LiveMonitoring.IRemoteLib.ReportSettings();
            NewSettings.ReportName = txtName.Text.ToUpper();
            NewSettings.ReportDiscription = txtDescription.Text.ToUpper();
            NewSettings.ReportUrl = txtUrl.Text;
            NewSettings.SiteId = 1;
            NewSettings.CapturedBy = MyUser.ID;
            int Myresp = MyRem.LiveMonServer.AddNewReportSettings(NewSettings);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        
        public void ClearEdit()
        {
            txtEditDescription.Text = "";
            txtEditName.Text = "";
            txtEditUrl.Text = "";
        }

        public bool CanSaveEdit()
        {

            if (txtEditName.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter report name.";
                txtEditName.Focus();
                return false;
            }
            if (txtEditDescription.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter report description.";
                txtEditDescription.Focus();
                return false;
            }
            if (txtEditUrl.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter report url.";
                txtEditUrl.Focus();
                return false;
            }
            try
            {
                if (Convert.ToInt32(gvReports.SelectedRow.Cells[1].Text) == 0)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please select settings to update! if this persist reload/refresh this page and try again.";
                    return false;
                }
            }
            catch
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please select settings to update! if this persist reload/refresh this page and try again.";
                return false;
            }
            return true;
        }

        public bool UpdateSettings()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.ReportSettings NewSettings = new LiveMonitoring.IRemoteLib.ReportSettings();
            NewSettings.ReportName = txtEditName.Text.ToUpper();
            NewSettings.ReportDiscription = txtEditDescription.Text.ToUpper();
            NewSettings.ReportUrl = txtEditUrl.Text;
            NewSettings.SiteId = 1;
            NewSettings.CapturedBy = MyUser.ID;
            NewSettings.ReportId = Convert.ToInt32(gvReports.SelectedRow.Cells[1].Text);
            NewSettings.isDeleted = chkisDeleted.Checked;
            int Myresp = MyRem.LiveMonServer.EditReportSettings(NewSettings);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanSaveEdit() == true)
                {
                    if (UpdateSettings() == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Report settings were successfully updated.";
                        ClearEdit();
                        divEdit.Visible = false;
                        LoadSettings();
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while saving settings, please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvReports.PageIndex = e.NewPageIndex;
                gvReports.DataSource = getSettings();
                gvReports.DataBind();
                lblSuccess.Text = "";
                lblEditSuccess.Text = "";

            }
            catch (Exception ex)
            {
            }
        }

        public SQLReportsConfig()
        {
            Load += Page_Load;
        }
    }
}