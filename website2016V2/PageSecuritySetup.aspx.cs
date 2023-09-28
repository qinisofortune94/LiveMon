//'using Infragistics.WebUI.UltraWebGrid;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class PageSecuritySetup : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        public LiveMonitoring.PageSecurityClass IPMonPageSecure;
        LiveMonitoring.DataAccess myDA = new LiveMonitoring.DataAccess();
        DataSet myDS;
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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");

                getALL();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (gridSecurity.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridSecurity.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridSecurity.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridSecurity.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridSecurity.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "SaveItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;

                LiveMonitoring.IRemoteLib.PageDetailsDef MyPage = new LiveMonitoring.IRemoteLib.PageDetailsDef();
                MyPage.PageName = gridSecurity.Rows[myRow.RowIndex].Cells[3].Text;
                MyPage.PageDisplayName = gridSecurity.Rows[myRow.RowIndex].Cells[4].Text;
                MyPage.ViewLevel = Convert.ToInt32(gridSecurity.Rows[myRow.RowIndex].Cells[5].Text);
                MyPage.EditLevel = Convert.ToInt32(gridSecurity.Rows[myRow.RowIndex].Cells[6].Text);
                MyPage.DeleteLevel = Convert.ToInt32(gridSecurity.Rows[myRow.RowIndex].Cells[7].Text);
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();


                if (MyRem.LiveMonServer.EditPageLevel(MyPage) == false)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Not saved Error!";
                    return;
                }
                else
                {
                    successMessage.Visible = true;
                    lblSuccess.Text = "Saved successfuly.";
                    getALL();
                }
            }

            else if (commandName == "EditItem")
            {
                ViewState["Id"] = Id;
                accordion1.Visible = true;
                txtViewLevel.Text = gridSecurity.Rows[myRow.RowIndex].Cells[5].Text;
                txtEditLevel.Text = gridSecurity.Rows[myRow.RowIndex].Cells[6].Text;
                txtDeleteLevel.Text = gridSecurity.Rows[myRow.RowIndex].Cells[7].Text;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string temp = btnSave.Text;

            if (temp.Contains("Update"))
            {
                int Id = int.Parse(ViewState["Id"].ToString());
                SqlParameter[] myParams = new SqlParameter[4];
                try
                {
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@ID";
                    myParams[0].Value = Id;
                    myParams[1] = new SqlParameter();
                    myParams[1].ParameterName = "@ViewLevel";
                    myParams[1].Value = txtViewLevel.Text;
                    myParams[2] = new SqlParameter();
                    myParams[2].ParameterName = "@EditLevel";
                    myParams[2].Value = txtEditLevel.Text;
                    myParams[3] = new SqlParameter();
                    myParams[3].ParameterName = "@DeleteLevel";
                    myParams[3].Value = txtDeleteLevel.Text;

                    MyDataAccess.ExecCmdQueryParams("security_level_update", myParams);
                    successMessage.Visible = true;
                    lblSuccess.Text = "Levels Successfully updated";
                    getALL();
                    accordion1.Visible = false;
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message;
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
            }
        }

        public void getALL()
        {
            try
            {
                myDS = myDA.ExecCmdQueryNoParamsDS("security_select_all");
                gridSecurity.DataSource = myDS.Tables[0];
                gridSecurity.DataBind();
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;

                Trace.Write("LocationsSetup.populateGridView: Error: " + ex.Message);
            }
        }

        public PageSecuritySetup()
        {
            Load += Page_Load;
        }  
    }
}