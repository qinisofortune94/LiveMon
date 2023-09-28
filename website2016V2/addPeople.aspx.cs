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
    public partial class addPeople : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
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

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadPeople();
            if (gridPeople.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridPeople.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridPeople.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridPeople.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridPeople.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;

                txtFirstName.Text = gridPeople.Rows[myRow.RowIndex].Cells[3].Text;
                txtLastName.Text = gridPeople.Rows[myRow.RowIndex].Cells[4].Text;
                txtEmail.Text = gridPeople.Rows[myRow.RowIndex].Cells[5].Text;
                txtCell.Text = gridPeople.Rows[myRow.RowIndex].Cells[6].Text;
                txtTelephone.Text = gridPeople.Rows[myRow.RowIndex].Cells[7].Text;
                txtFax.Text = gridPeople.Rows[myRow.RowIndex].Cells[8].Text;
                txtAddress.Text = gridPeople.Rows[myRow.RowIndex].Cells[9].Text;

                lblAdd.Text = "Update";
                btnSave.Text = "Update";
                successMessage.Visible = false;
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
                    myParams[0].ParameterName = "@ID";
                    myParams[0].Value = intUserSiteId;

                    MyDataAccess.ExecCmdQueryParams("sp_deletePeople", myParams);
                    LoadPeople();
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
                //Refresh Grid
                LoadPeople();
            }

        }

        public void LoadPeople()
        {
            gridPeople.DataSource = getPeople();
            gridPeople.DataBind();
        }

        private DataTable getPeople()
        {
            try
            {
                string sqlQuery = "people_Get_AllPeople";
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
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;
                return null;
            }
        }

        public void Clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCell.Text = "";
            txtEmail.Text = "";
            txtTelephone.Text = "";
            txtAddress.Text = "";
            txtFax.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CanSave())
            {
                //Check button text to add or update
                string temp = btnSave.Text;

                if (temp.Contains("Add"))
                {
                    if (SavePeople() == true)
                    {
                        successMessage.Visible = true;
                        lblSuccess.Text = "Details were successfully saved.";
                        Clear();      
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "An error occured while trying to save details, please try again.";
                    }
                }
                else if (temp.Contains("Update"))
                {

                    int Id = int.Parse(ViewState["Id"].ToString());
                    try
                    {
                        LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                        LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
                        bool response = true;
                        DataTable mydt = new DataTable();

                        LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                        MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                        LiveMonitoring.IRemoteLib.People NewPeople = new LiveMonitoring.IRemoteLib.People();
                        NewPeople.FirstName = txtFirstName.Text;
                        NewPeople.SurName = txtLastName.Text;
                        NewPeople.Cell = txtCell.Text;
                        NewPeople.Email = txtEmail.Text;
                        NewPeople.Phone = txtTelephone.Text;
                        NewPeople.Fax = txtFax.Text;
                        NewPeople.Address = txtAddress.Text;
                        NewPeople.SurName = txtLastName.Text;

                        NewPeople.ID = Id;
                        response = MyRem.LiveMonServer.EditPeople(NewPeople);
                        //return response;
                        successMessage.Visible = true;
                        lblSuccess.Text = "Successfully updated";
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Visible = true;
                        lblError.Text = ex.Message.ToString();
                    }

                    btnSave.Text = "Add";
                    successMessage.Visible = false;
                    Clear();
                }

                //Refresh Grid
                LoadPeople();

                Clear();
            }
        }
        public bool CanSave()
        {
            if (txtFirstName.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter first name.";
                txtFirstName.Focus();
                return false;
            }
            if (txtLastName.Text.Trim().Length == 0)
            {
                errorMessage.Visible = true;
                lblError.Text = "Please enter last name.";
                txtLastName.Focus();
                return false;
            }
            return true;
        }

        public bool SavePeople()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.People NewPeople = new LiveMonitoring.IRemoteLib.People();
            NewPeople.FirstName = txtFirstName.Text;
            NewPeople.SurName = txtLastName.Text;
            NewPeople.Cell = txtCell.Text;
            NewPeople.Email = txtEmail.Text;
            NewPeople.Phone = txtTelephone.Text;
            NewPeople.Fax = txtFax.Text;
            NewPeople.Address = txtAddress.Text;
            response = MyRem.LiveMonServer.AddNewPeople(NewPeople);

            return response;
        }
       
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void People_People()
        {
            Load += Page_Load;
        }
    }
}