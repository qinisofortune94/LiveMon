//'using Infragistics.WebUI.UltraWebGrid;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class PageSecuritySetup : System.Web.UI.Page
    {
        public LiveMonitoring.PageSecurityClass IPMonPageSecure;
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
                MyPage.PageName = gridSecurity.Rows[myRow.RowIndex].Cells[2].Text;
                MyPage.PageDisplayName = gridSecurity.Rows[myRow.RowIndex].Cells[3].Text;
                MyPage.ViewLevel = Convert.ToInt32(gridSecurity.Rows[myRow.RowIndex].Cells[4].Text);
                MyPage.EditLevel = Convert.ToInt32(gridSecurity.Rows[myRow.RowIndex].Cells[5].Text);
                MyPage.DeleteLevel = Convert.ToInt32(gridSecurity.Rows[myRow.RowIndex].Cells[6].Text);
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
                    lblSucces.Text = "Saved successfuly.";
                }

            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

            }

        }

        public PageSecuritySetup()
        {
            Load += Page_Load;
        }
    }
}