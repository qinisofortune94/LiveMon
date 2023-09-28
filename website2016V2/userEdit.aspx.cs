using Infragistics.WebUI.UltraWebGrid;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class userEdit : System.Web.UI.Page
    {
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();


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
            if (IsPostBack == false)
            {
                LoadPage();
            }
        }




        protected void cmdUpdate_Click(object sender, EventArgs e)
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

            if (this.txtPassword.Text != this.txtPasswordConfirm.Text)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please confirm Password.";

                this.txtPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtUserName.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please confirm Username.";
                this.txtUserName.Focus();
                return;
            }
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser.ID =Convert.ToInt32(this.txtID.Text);
            MyUser.Address = this.txtAddress.Text;
            MyUser.Cell = this.txtCellNumber.Text;
            MyUser.Email = this.txtEmail.Text;
            MyUser.Fax = this.txtFaxnumber.Text;
            MyUser.FirstName = this.txtFirstname.Text;
            MyUser.Pager = this.txtPager.Text;
            MyUser.Password = da.GetEncrypted(this.txtPassword.Text);
            MyUser.Phone = this.txtPhoneNumber.Text;
            MyUser.SurName = this.txtSurname.Text;
            MyUser.UserLevel =Convert.ToInt32(this.UserLevell.Text);
            MyUser.UserName = this.txtUserName.Text;
            MyUser.PeopleID =Convert.ToInt32(this.PeopleID.Value);

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs

            if (MyRem.LiveMonServer.EditUser(MyUser) == false)
            {
                errorMessage.Visible = true;
                lblError.Text = "Saving updated user failed.";

            }
            else
            {
                successMessage.Visible = true;
                lblSucces.Text = "User updated successfuly.";

                LoadPage();
            }
        }


        private void ClearVals()
        {
            this.txtID.Text = "";
            this.txtAddress.Text = "";
            this.txtCellNumber.Text = "";
            this.txtEmail.Text = "";
            this.txtFaxnumber.Text = "";
            this.txtFirstname.Text = "";
            this.txtPager.Text = "";
            this.txtPassword.Text = "";
            this.txtPasswordConfirm.Text = "";
            this.txtPhoneNumber.Text = "";
            this.txtSurname.Text = "";
            this.UserLevell.Text = "";
            this.txtUserName.Text = "";
            this.PeopleID.Value = "";
        }



        public void LoadPage()
        {
            ClearVals();
            Collection MyCollectionUsers = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            try
            {
                MyCollectionUsers = MyRem.LiveMonServer.GetAllUsers();
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Failed to get all users.";
            }

           // LiveMonitoring.IRemoteLib.UserDetails MyUser = default(LiveMonitoring.IRemoteLib.UserDetails);
            gridUsers.Clear();
            foreach (LiveMonitoring.IRemoteLib.UserDetails MyUser in MyCollectionUsers)
            {
                //Alertsgrid.Rows.Add()
                UltraGridRow myrow = new UltraGridRow(true);
                myrow.Cells.Add();
                myrow.Cells[0].Value = MyUser.FirstName;
                myrow.Cells.Add();
                myrow.Cells[1].Value = MyUser.SurName;
                myrow.Cells.Add();
                myrow.Cells[2].Value = MyUser.UserName;
                myrow.Cells.Add();
                myrow.Cells[3].Value = MyUser.UserLevel;
                myrow.Tag = MyUser.ID;
                gridUsers.Rows.Add(myrow);
            }
            //LoadPage()
        }


        protected void gridUsers_ActiveRowChange(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            UltraGridRow myrow = e.Row;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = default(LiveMonitoring.IRemoteLib.UserDetails);
            txtID.Text =Convert.ToString(myrow.Tag);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyUser = MyRem.LiveMonServer.GetSpecificUser(Convert.ToInt32(myrow.Tag));
            this.txtAddress.Text = MyUser.Address;
            this.txtCellNumber.Text = MyUser.Cell;
            this.txtEmail.Text = MyUser.Email;
            this.txtFaxnumber.Text = MyUser.Fax;
            this.txtFirstname.Text = MyUser.FirstName;
            this.txtPager.Text = MyUser.Pager;
            this.txtPassword.Text = da.GetEncrypted(MyUser.Password);
            this.txtPhoneNumber.Text = MyUser.Phone;
            this.txtSurname.Text = MyUser.SurName;
            this.UserLevell.Text = MyUser.UserLevel.ToString();
            this.txtUserName.Text = MyUser.UserName;
            this.PeopleID.Value = MyUser.PeopleID.ToString();
        }


        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetDeleteLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedDelete.aspx");
            }
            //If MsgBox("Are You Sure ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim Myfunc As New LiveMonitoring.SharedFuncs
            MyRem.LiveMonServer.DeleteUser(Convert.ToInt32(this.txtID.Text));
            LoadPage();
            //End If
        }

        public userEdit()
        {
            Load += Page_Load;
        }
    }
}