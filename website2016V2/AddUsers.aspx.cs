using System;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;

namespace website2016V2
{
    partial class AddUsers : System.Web.UI.Page
    {
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();

        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (((string)Session["LoggedIn"] == "True"))
            {

                DataManagerr datamanager = new DataManagerr();
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
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    datamanager.LoadEmployees(ddlPerson, null);
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        //[System.Web.Services.WebMethod]
        //public static string CheckEmail(string email)
        //{
        //    string retval = "";
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString());
        //    SqlCommand cmd = new SqlCommand("select Email from Users where Email=@Email", con);

        //    cmd.Parameters.AddWithValue("@Email", email);
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    if (dr.HasRows)
        //    {
        //        retval = "true";
        //    }
        //    else
        //    {
        //        retval = "false";
        //    }

        //    return retval;
        //}

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUserC = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUserC = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

            if (MyIPMonPageSecure > MyUserC.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
            var existingUser = MyUserC.Email.Equals(txtEmail.Text);
            if (existingUser != true)
            {
                lblError.Text = "User Email Exist";
                txtEmail.Focus();
            }

            if (this.txtFirstName.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply first name.";
                this.txtFirstName.Focus();
                return;
            }
            if (this.txtSurName.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply surname.";
                this.txtSurName.Focus();
                return;
            }
            if (this.txtUserLevel.Text == Convert.ToString(0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply user level.";
                this.txtUserLevel.Focus();
                return;
            }
            if (this.txtPhoneNumber.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply phone number.";
                this.txtPhoneNumber.Focus();
                return;
            }
            if (this.txtFaxNumber.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply fax number.";
                txtFaxNumber.Focus();
                return;
            }
            if (this.txtMobileNumber.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply mobile number.";
                this.txtMobileNumber.Focus();
                return;
            }
            if (this.txtPagerNumber.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply page number.";
                txtPagerNumber.Focus();
                return;
            }
            if (this.txtAddress.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply address.";
                this.txtAddress.Focus();
                return;
            }
            if (this.txtEmail.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply email.";
                this.txtEmail.Focus();
                return;
            }

            if (this.txtPassword.Text == Convert.ToString(0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply password.";
                this.txtPassword.Focus();
                return;
            }
            if (this.txtPassword.Text != this.txtConfirmPassword.Text)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please confirm password.";
                this.txtPassword.Focus();
                return;
            }
            if (this.txtUserName.Text == Convert.ToString(0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply user name.";
                this.txtUserName.Focus();
                return;
            }

            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser.Password = da.GetEncrypted(this.txtPassword.Text);
            MyUser.FirstName = this.txtFirstName.Text;
            MyUser.SurName = this.txtSurName.Text;
            MyUser.UserLevel = Convert.ToInt16(this.txtUserLevel.Text);
            MyUser.Phone = this.txtPhoneNumber.Text;
            MyUser.Fax = this.txtFaxNumber.Text;
            MyUser.Cell = this.txtMobileNumber.Text;
            MyUser.Pager = this.txtPagerNumber.Text;
            MyUser.Address = this.txtAddress.Text;
            MyUser.Email = this.txtEmail.Text;
            MyUser.UserName = this.txtUserName.Text;
                        
            if (ddlPerson.SelectedIndex > 0)
            {
                MyUser.PeopleID = Convert.ToInt16(ddlPerson.SelectedItem.Value);
            }
            else
            {
                MyUser.PeopleID = -1;
            }

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs

            if (MyRem.LiveMonServer.AddNewUser(MyUser) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Adding user failed.";

            }
            else
            {
                successMessage.Visible = true;
                lblSuccess.Text = "User added successfuly.";
                //Clear();
                //BtnClear_Click();
            }
        }
        private void Clear()
        {
            this.txtAddress.Text = "";
            this.txtMobileNumber.Text = "";
            this.txtEmail.Text = "";
            this.txtFaxNumber.Text = "";
            this.txtFirstName.Text = "";
            this.txtPagerNumber.Text = "";
            this.txtPassword.Text = "";
            this.txtConfirmPassword.Text = "";
            this.txtPhoneNumber.Text = "";
            this.txtSurName.Text = "";
            this.txtUserLevel.Text = "5";
            this.txtUserName.Text = "";
            lblError.Visible = false;
        }

        // ERROR: Handles clauses are not supported in C#
        protected void ddlPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataManagerr datamanager = new DataManagerr();
                DataRow dr = datamanager.GetPeopleDetails(Convert.ToInt32(ddlPerson.SelectedItem.Value));
                txtFirstName.Text = (string)dr["FirstName"];
                txtSurName.Text = (string)dr["SurName"];
                txtMobileNumber.Text = (string)dr["Cell"];
                txtPhoneNumber.Text = (string)dr["Phone"];
                txtEmail.Text = (string)dr["Email"];
                txtFaxNumber.Text = (string)dr["Fax"];
                txtAddress.Text = (string)dr["Address"];                
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error occured. Please try again.";
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        //protected void gvUsers_Commands(object sender, GridViewCommandEventArgs e)
        //{
        //    string commandName = e.CommandName;

        //    LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
        //    GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
        //    GridView myGrid = (GridView)sender; // the gridview 
        //    string Id = gvUsers.DataKeys[myRow.RowIndex].Value.ToString();

        //    if (commandName == "EditItem")
        //    {
        //        //Accessing BoundField Column
        //        string PeopleId = gvUsers.Rows[myRow.RowIndex].Cells[3].Text;
        //        string FirstName = gvUsers.Rows[myRow.RowIndex].Cells[4].Text;
        //        string SurName = gvUsers.Rows[myRow.RowIndex].Cells[5].Text;
        //        string UserLevel = gvUsers.Rows[myRow.RowIndex].Cells[6].Text;
        //        string PhoneNumber = gvUsers.Rows[myRow.RowIndex].Cells[7].Text;
        //        string FaxNumber = gvUsers.Rows[myRow.RowIndex].Cells[8].Text;
        //        string MobileNumber = gvUsers.Rows[myRow.RowIndex].Cells[9].Text;
        //        string PageNumber = gvUsers.Rows[myRow.RowIndex].Cells[10].Text;
        //        string Address = gvUsers.Rows[myRow.RowIndex].Cells[11].Text;
        //        string Email = gvUsers.Rows[myRow.RowIndex].Cells[12].Text;
        //        string UserName = gvUsers.Rows[myRow.RowIndex].Cells[13].Text;

        //        ViewState["Id"] = PeopleId;

        //        txtFirstName.Text = FirstName;
        //        txtSurName.Text = SurName;
        //        txtUserLevel.Text = UserLevel;
        //        txtPhoneNumber.Text = PhoneNumber;
        //        txtFaxNumber.Text = FaxNumber;
        //        txtMobileNumber.Text = MobileNumber;
        //        txtPagerNumber.Text = PageNumber;
        //        txtAddress.Text = Address;
        //        txtEmail.Text = Email;
        //        txtUserName.Text = UserName;
        //        //txtPassword.Text = Password;
        //        //txtConfirmPassword.Text = ConfirmPassword;

        //        lblAdd.Text = "Update";
        //        btnAdd.Text = "Update";

        //    }

        //    else if (commandName == "DeleteItem")
        //    {

        //        ViewState["Id"] = Id;

        //        DataManagerr datamanager = new DataManagerr();

        //        int PeopleId = Convert.ToInt16(Id);
        //        datamanager.LoadEmployees(PeopleId);

        //        //Refresh Grid
        //        LoadEmployees();
        //    }
        //}

        //public void LoadEmployees()
        //{
        //    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();

        //    List<UserDetails> list = MyUser.LoadEmployees();
        //    gvUsers.DataSource = list;
        //    gvUsers.DataBind();

        //}

        //protected void gvUsers_PreRender(object sender, EventArgs e)
        //{
        //    //calling the Load method to populate the gridview 
        //    LoadEmployees();

        //    if (gvUsers.Rows.Count > 0)
        //    {
        //        //Replace the <td> with <th> and adds the scope attribute
        //        gvUsers.UseAccessibleHeader = true;

        //        //Adds the <thead> and <tbody> elements required for DataTables to work
        //        gvUsers.HeaderRow.TableSection = TableRowSection.TableHeader;

        //        //Adds the <tfoot> element required for DataTables to work
        //        gvUsers.FooterRow.TableSection = TableRowSection.TableFooter;
        //    }
        //}
    }
}
