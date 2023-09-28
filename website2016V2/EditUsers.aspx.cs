
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class EditUsers : System.Web.UI.Page
    {
        private LiveMonitoring.SharedFuncs da = new LiveMonitoring.SharedFuncs();
        protected void Page_Load(object sender, System.EventArgs e)
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
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    LoadPage();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnUpdate_Click(object sender, System.EventArgs e)
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
            if (string.IsNullOrEmpty(this.txtUserName.Text))
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please confirm Username.";
                this.txtUserName.Focus();
                return;
            }
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser.ID = Convert.ToInt32(this.txtID.Text);
            MyUser.Address = this.txtAddress.Text;
            MyUser.Cell = this.txtMobileNumber.Text;
            MyUser.Email = this.txtEmail.Text;
            MyUser.Fax = this.txtFaxNumber.Text;
            MyUser.FirstName = this.txtFirstName.Text;
            MyUser.Pager = this.txtPagerNumber.Text;
            MyUser.Password = da.GetEncrypted(this.txtPassword.Text);
            MyUser.Phone = this.txtPhoneNumber.Text;
            MyUser.SurName = this.txtSurName.Text;
            MyUser.UserLevel = Convert.ToInt32(this.txtUserLevel.Text);
            MyUser.UserName = this.txtUserName.Text;
            MyUser.PeopleID = Convert.ToInt32(this.txtID.Text);

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
                lblSuccess.Text = "User updated successfuly.";

               
            }
        
                LoadPage();
           
        }


        private void ClearVals()
        {
            this.txtID.Text = "";
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
            this.txtUserLevel.Text = "";
            this.txtUserName.Text = "";
            this.txtID.Text = "";
        }

        public void AddRows(string[] RowVals)
        {
            DataRow Row = default(DataRow);
            DataTable dt = new DataTable();
            //= CType(Session["mytableSensSched"], DataTable)
            //ListFiles()

            if (Session["mytableSensSched"] == null == false)
            {
                dt = (DataTable)Session["mytableSensSched"];
                //For Each row1 As DataRow In dt.Rows
                // dt.ImportRow(row1)
                //Next

            }
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("SurName", typeof(string));
                dt.Columns.Add("UserName", typeof(string));
                dt.Columns.Add("UserLevel", typeof(string));
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Fax", typeof(string));
                dt.Columns.Add("Cell", typeof(string));
                dt.Columns.Add("Address", typeof(string));
                dt.Columns.Add("Pager", typeof(string));
                dt.Columns.Add("Email", typeof(string));
             dt.Columns.Add("Password", typeof(string));
            }
            Row = dt.NewRow();
            Row[0] = RowVals[0];
            Row[1] = RowVals[1];
            Row[2] = RowVals[2];
            Row[3] = RowVals[3];
            Row[4] = RowVals[4];
             Row[5] = RowVals[5];
            Row[6] = RowVals[6];
            Row[7] = RowVals[7];
            Row[8] = RowVals[8];
            Row[9] = RowVals[9];
            Row[10] = RowVals[10];
            Row[11] = RowVals[11];
            //Row[12] = RowVals[12];
            //DTable.Rows.Add(Row)
            dt.Rows.Add(Row);
            Session["mytableSensSched"] = dt;
            GridBind(dt);

        }

        public void GridUser()
        {
            DataTable dt = new DataTable();
            Session["mytableSensSched"] = dt;
            GridBind(dt);
        }

        public void GridBind(DataTable dt)
        {
            UserGrid.DataSource = dt;
            UserGrid.DataKeyNames = (new string[] { "ID" });
            UserGrid.DataBind();
        }
        public void ClearUserGridRows()
        {
            DataTable dt = new DataTable();
            Session["myContacttable"] = dt;
            GridBind(dt);
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

            //  LiveMonitoring.IRemoteLib.UserDetails MyUser = default(LiveMonitoring.IRemoteLib.UserDetails);
            ClearUserGridRows();
            foreach (LiveMonitoring.IRemoteLib.UserDetails MyUser in MyCollectionUsers)
            {

                AddRows((new string[] {
                            MyUser.FirstName.ToString(),
                            MyUser.SurName.ToString(),
                            MyUser.UserName.ToString(),
                              MyUser.UserLevel.ToString(),
                              MyUser.ID.ToString(),
                              MyUser.Phone.ToString(),
                              MyUser.Fax.ToString(),
                              MyUser.Cell.ToString(),
                              MyUser.Address.ToString(),
                              MyUser.Email.ToString(),
                              MyUser.Pager.ToString(),
                              MyUser.Password.ToString()

                        }));
            }
            //LoadPage()
        }

        private void gridUsers_ActiveRowChange()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.IRemoteLib.UserDetails MyUser = default(LiveMonitoring.IRemoteLib.UserDetails);
             txtID.Text = (UserGrid.SelectedRow.Cells[8].Text);
            int pName =Convert.ToInt32(UserGrid.SelectedRow.Cells[8].Text);
            MyUser = MyRem.LiveMonServer.GetSpecificUser(pName);
            this.txtAddress.Text = MyUser.Address;
            this.txtMobileNumber.Text = MyUser.Cell;
            this.txtEmail.Text = MyUser.Email;
            this.txtFaxNumber.Text = MyUser.Fax;
            this.txtFirstName.Text = MyUser.FirstName;
            this.txtPagerNumber.Text = MyUser.Pager;
            this.txtPassword.Text = da.GetEncrypted(MyUser.Password);
            this.txtPhoneNumber.Text = MyUser.Phone;
            this.txtSurName.Text = MyUser.SurName;
            this.txtUserLevel.Text = MyUser.UserLevel.ToString();
            this.txtUserName.Text = MyUser.UserName;
            this.PeopleID.Value = MyUser.PeopleID.ToString();
        }
        protected void Usergrid_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            UserGrid.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytableSensSched"];
            GridBind(dt);
        }

        protected void gridusers_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                LiveMonitoring.IRemoteLib.UserDetails MyUser = default(LiveMonitoring.IRemoteLib.UserDetails);
                txtID.Text = (UserGrid.SelectedRow.Cells[8].Text);
                int pName = Convert.ToInt32(UserGrid.SelectedRow.Cells[8].Text);
                MyUser = MyRem.LiveMonServer.GetSpecificUser(pName);
                this.txtAddress.Text = MyUser.Address;
                this.txtMobileNumber.Text = MyUser.Cell;
                this.txtEmail.Text = MyUser.Email;
                this.txtFaxNumber.Text = MyUser.Fax;
                this.txtFirstName.Text = MyUser.FirstName;
                this.txtPagerNumber.Text = MyUser.Pager;
                this.txtPassword.Text = da.GetEncrypted(MyUser.Password);
                this.txtPhoneNumber.Text = MyUser.Phone;
                this.txtSurName.Text = MyUser.SurName;
                this.txtUserLevel.Text = MyUser.UserLevel.ToString();
                this.txtUserName.Text = MyUser.UserName;
                this.PeopleID.Value = MyUser.PeopleID.ToString();

                //  txtFirstName.Text = UserGrid.SelectedRow.Cells[1].Text;
                //  txtSurName.Text = UserGrid.SelectedRow.Cells[2].Text;
                //  txtUserName.Text = UserGrid.SelectedRow.Cells[3].Text;
                //  txtUserLevel.Text = UserGrid.SelectedRow.Cells[4].Text;
                //  txtPhoneNumber.Text = UserGrid.SelectedRow.Cells[5].Text;
                //  txtFaxNumber.Text = UserGrid.SelectedRow.Cells[6].Text;
                //  txtMobileNumber.Text = UserGrid.SelectedRow.Cells[7].Text;
                //  txtID.Text = UserGrid.SelectedRow.Cells[8].Text;
                //  txtAddress.Text = UserGrid.SelectedRow.Cells[9].Text;
                //  txtEmail.Text = UserGrid.SelectedRow.Cells[10].Text;
                //  txtPagerNumber.Text = UserGrid.SelectedRow.Cells[11].Text;
                //txtPassword.Text = UserGrid.SelectedRow.Cells[12].Text;





            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }
     
        protected void btnDelete_Click(object sender, System.EventArgs e)
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
           
        }

        protected void UserGrid_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            UserGrid.PageIndex = e.NewPageIndex;
            //Bind data to the GridView control.
            DataTable dt = new DataTable();
            dt = (DataTable)Session["mytableSensSched"];
            GridBind(dt);
        }

        public EditUsers()
        {
            Load += Page_Load;
        }
    }
}