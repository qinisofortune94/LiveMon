using System;
using System.Web.UI.WebControls;
using System.Data;

namespace website2016V2
{
    public partial class AddUsers : System.Web.UI.Page
    {
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
       
        protected void Page_Load(object sender, System.EventArgs e)
        {      

            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.DataManager datamanager = new LiveMonitoring.DataManager();
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
            if (this.txtPassword.Text != this.txtConfirmPassword.Text)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please confirm Password.";
                this.txtPassword.Focus();
                return;
            }
            if (this.txtUserName.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply Username.";
                return;
            }
            if (this.txtFirstName.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply Firstname.";
                return;
            }
            if (this.txtSurName.Text == "")
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply Surname.";
                return;
            }

            if (this.txtUserLevel.Text == Convert.ToString(0))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply user level.";
                return;
            }
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser.Address = this.txtAddress.Text;
            MyUser.Cell = this.txtMobileNumber.Text;
            MyUser.Email = this.txtEmail.Text;
            MyUser.Fax = this.txtFaxNumber.Text;
            MyUser.FirstName = this.txtFirstName.Text;
            MyUser.Pager = this.txtPagerNumber.Text;
            MyUser.Password = da.GetEncrypted(this.txtPassword.Text);
            MyUser.Phone = this.txtPhoneNumber.Text;
            MyUser.SurName = this.txtSurName.Text;
            MyUser.UserLevel =Convert.ToInt16(this.txtUserLevel.Text);
            MyUser.UserName = this.txtUserName.Text;
            if (ddlPerson.SelectedIndex > 0)
            {
                MyUser.PeopleID =Convert.ToInt16(ddlPerson.SelectedItem.Value);
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
                lblSucces.Text = "User added successfuly.";
                clearform();
            }
        }
        private void clearform()
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
            lblErr.Visible = false;
        }                 

        // ERROR: Handles clauses are not supported in C#
        protected void ddlPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LiveMonitoring.DataManager datamanager = new LiveMonitoring.DataManager();
                DataRow dr = datamanager.GetPeopleDetails(Convert.ToInt16(ddlPerson.SelectedItem.Value));
                txtFirstName.Text = (string)dr["FirstName"];
                txtSurName.Text = (string)dr["SurName"];

                //if (Convert.IsDBNull(dr["MobileNumber"])==false)
                //   txtMobileNumber.Value = dr["MobileNumber"];
                //if (Convert.IsDBNull(dr["Email"]) == false)
                //    txtEmail.Text = (string)dr["Email"];
                //if (Convert.IsDBNull(dr["FaxNumber"]) == false)
                //    txtFaxNumber.Value = dr["Fax"];
                //if (Convert.IsDBNull(dr["Address"]) == false)
                //    txtAddress.Text = (string)dr["Address"];

                if (!Convert.IsDBNull(dr["FirstName"]))
                    txtMobileNumber.Text = (string)dr["FirstName"];
                if (!Convert.IsDBNull(dr["Email"]))
                    txtMobileNumber.Text = (string)dr["Email"];
                if (!Convert.IsDBNull(dr["FaxNumber"]))
                    txtMobileNumber.Text = (string)dr["FaxNumber"];
                if (!Convert.IsDBNull(dr["Address"]))
                    txtMobileNumber.Text = (string)dr["Address"];                
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "An error occured. Please try again.";
            }
        }
    }
}
