using LiveMonitoring;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class MeteringEquipmentLayout : System.Web.UI.Page
    {
        public DataAccess MyDataAccess = new DataAccess();
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
            successMessage.Visible = false;
            warningMessage.Visible = false;
            errorMessage.Visible = false;
            if (IsPostBack == false)
            {
                LoadSensors();
            }
        }





        public void LoadSensors()
        {
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            object MyObject1 = null;
            int MyDiv = 1;
            bool added = true;
            if ((MyCollection == null) == false)
            {
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor2 = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            System.Web.UI.WebControls.ListItem MyIttem = new System.Web.UI.WebControls.ListItem();
                            System.Web.UI.WebControls.ListItem MyIttem2 = new System.Web.UI.WebControls.ListItem();
                            MyIttem.Text = Mysensor.Caption;
                            MyIttem.Value = Mysensor.ID.ToString();
                            MyIttem2.Text = Mysensor2.Caption;
                            MyIttem2.Value = Mysensor2.ID.ToString();
                            ddlMeterID.Items.Add(MyIttem);
                            ddlFeedingMeter.Items.Add(MyIttem2);
                            if (added)
                            {
                                MyIttem.Selected = false;
                                MyIttem2.Selected = true;
                                added = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadMeteringEquipmentLayout();
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
                ////Accessing BoundField Column
                ViewState["Id"] = Id;

                ddlMeterID.SelectedValue = gridPeople.Rows[myRow.RowIndex].Cells[3].Text;
                ddlFeedingMeter.SelectedValue = gridPeople.Rows[myRow.RowIndex].Cells[5].Text;

                lblAdd.Text = "Update";
                btnAdd.Text = "Update";
                successMessage.Visible = false;
            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;

                int selectedId = 0;
                SqlParameter[] myParams = new SqlParameter[1];

                try
                {
                    selectedId = Convert.ToInt32(ViewState["Id"]);

                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@ID";
                    myParams[0].Value = selectedId;

                    MyDataAccess.ExecCmdQueryParams("MeteringDeleteMeteringEquipmentLayout", myParams);
                    LoadMeteringEquipmentLayout();
                }
                catch (Exception ex)
                {
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
      
                LoadMeteringEquipmentLayout();
            }

        }


        public void LoadMeteringEquipmentLayout()
        {
            gridPeople.DataSource = getMeteringEquipmentLayout();
            gridPeople.DataBind();
        }

        private DataTable getMeteringEquipmentLayout()
        {
            try
            {
                string sqlQuery = "GetAllMeteringEquipmentLayout";
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string temp = btnAdd.Text;


            if (temp.Contains("Add"))
            {

                try
                {
                    SqlParameter[] MySQLParam = new SqlParameter[3];

                    MySQLParam[0] = new SqlParameter();
                    MySQLParam[0].ParameterName = "@MeterID";
                    MySQLParam[0].Value = ddlMeterID.SelectedValue;

                    MySQLParam[1] = new SqlParameter();
                    MySQLParam[1].ParameterName = "@FeedingMeterID";
                    MySQLParam[1].Value = ddlFeedingMeter.SelectedValue;

                    MySQLParam[2] = new SqlParameter();
                    MySQLParam[2].ParameterName = "@MainsFeed";
                    MySQLParam[2].Value = chkMainFeeds.Checked;

                    //execute the stored procedure to add the record
                    MyDataAccess.ExecCmdQueryParamsDS("MeteringAddMeteringEquipmentLayout", MySQLParam);

                    successMessage.Visible = true;
                    lblSucces.Text = "Metering equipment layout has been succussfully Added";

                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = "Err3" + ex.Message;

                }


            }
            else if (temp.Contains("Update"))
            {
                int Id = int.Parse(ViewState["Id"].ToString());
                try
                {
                    System.Data.SqlClient.SqlParameter[] MySQLParam = new SqlParameter[4];

                    MySQLParam[0] = new SqlParameter();
                    MySQLParam[0].ParameterName = "@MeterID";
                    MySQLParam[0].Value = ddlMeterID.SelectedValue;

                    MySQLParam[1] = new SqlParameter();
                    MySQLParam[1].ParameterName = "@FeedingMeterID";
                    MySQLParam[1].Value = ddlFeedingMeter.SelectedValue;

                    MySQLParam[2] = new SqlParameter();
                    MySQLParam[2].ParameterName = "@MainsFeed";
                    MySQLParam[2].Value = chkMainFeeds.Checked;

                    MySQLParam[3] = new SqlParameter();
                    MySQLParam[3].ParameterName = "@ID";
                    MySQLParam[3].Value = Id;

                    //execute the stored procedure to add the record
                    MyDataAccess.ExecCmdQueryParamsDS("MeteringUpdateMeteringEquipmentLayout", MySQLParam);
                    //Response.Redirect("Default.aspx")
                    successMessage.Visible = true;
                    lblSucces.Text = "Metering equipment layout successfully updated!";
                    btnAdd.Text = "Add";
                }
                catch (Exception ex)
                {



                }

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}