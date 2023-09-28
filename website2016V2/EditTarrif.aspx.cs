using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2.Metering
{
    //IPMonConnectionString
    public partial class EditTarrif : System.Web.UI.Page
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
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
           
            //calling the Load method to populate the gridview 
            LoadData();

            if (GridView1.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                GridView1.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //btnClear -- we are calling the load method to remove all the data from the textbox
            Clear();
        }
        public void Clear()
        {
            txtTarrif.Text = "";

        }
        public int SaveTarrif()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            int response = 0;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.MeteringTariff NewTarrif = new LiveMonitoring.IRemoteLib.MeteringTariff();
            NewTarrif.TarriffName = txtTarrif.Text;
          
            response = MyRem.LiveMonServer.AddNewMeteringTarrif(NewTarrif);

            return 0;
        }
        public bool CanSave()
        {
            if (txtTarrif.Text.Trim().Length == 0)
            {
                lblSuccess.Text = "Please enter first name.";
                txtTarrif.Focus();
                return false;
            }
            return true;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (CanSave())
            {
                //Check button text to add or update
                string temp = btnAdd.Text;

                if (temp.Contains("Add"))
                {
                    if (SaveTarrif() == 0)
                    {
                        lblSuccess.Text = "Details were successfully saved.";
                        Clear();
                    }
                    else
                    {
                        lblSuccess.Text = "An error occured while trying to save details, please try again.";
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
                        LiveMonitoring.IRemoteLib.MeteringTariff NewTarrif = new LiveMonitoring.IRemoteLib.MeteringTariff();
                        NewTarrif.TarriffName = txtTarrif.Text;



                        NewTarrif.ID = Id;
                        response = MyRem.LiveMonServer.EditMeteringTarrif(NewTarrif);
                        //lblSuccess.Text = "Details were successfully saved.";
                        return;
                    }
                    catch (Exception ex)
                    {
                        //return false;
                    }



                    btnAdd.Text = "Add";
                }

                //Refresh Grid
                

                Clear();
            }
        }

        public void LoadData()
        {
            GridView1.DataSource = getTarrifs();
            GridView1.DataBind();


        }
        private DataTable getTarrifs()
        {
            try
            {
                string sqlQuery = "MeteringGetAllTarrifDetails";
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
                //lblEditSuccess.Text = ex.Message;
                return null;
            }
        }


        private bool ValidateRole()
        {
            bool valid = true;

            if ((txtTarrif.Text == string.Empty))
            {
                txtTarrif.Style.Add("border", "1px solid red");

                valid = false;
            }
            else
            {
                txtTarrif.Style.Add("border", "");

            }

            return valid;
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = GridView1.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                ViewState["Id"] = Id;
                //Accessing BoundField Column
             
                txtTarrif.Text = GridView1.Rows[myRow.RowIndex].Cells[3].Text;
               
                lblAdd.Text = "Update";
                btnAdd.Text = "Update";

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

                    MyDataAccess.ExecCmdQueryParams("MeteringDeleteMeteringTariff", myParams);
                    LoadData();
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
                ViewState["Id"] = Id;
                LoadData();
               
             
            }

        }

        
    }
}