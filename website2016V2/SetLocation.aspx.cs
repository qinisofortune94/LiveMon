using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class SetLocation : System.Web.UI.Page
    {
        LiveMonitoring.DataAccess myDA = new LiveMonitoring.DataAccess();
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        DataTable myDT;
        DataSet myDS;
        private static string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();

        byte[] pstrIcon = null;
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

                try
                {
                    LoadDefaultIcons();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            populateGridView();
            if (gdvLocations.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gdvLocations.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gdvLocations.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gdvLocations.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gdvLocations.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                ViewState["Id"] = Id;
                txtCabinet.Text = gdvLocations.Rows[myRow.RowIndex].Cells[5].Text;
                txtRoom.Text = gdvLocations.Rows[myRow.RowIndex].Cells[6].Text;
                txtFloor.Text = gdvLocations.Rows[myRow.RowIndex].Cells[7].Text;
                txtBuilding.Text = gdvLocations.Rows[myRow.RowIndex].Cells[8].Text;
                txtUnit.Text = gdvLocations.Rows[myRow.RowIndex].Cells[9].Text;
                txtStreet.Text = gdvLocations.Rows[myRow.RowIndex].Cells[10].Text;
                txtSuburb.Text = gdvLocations.Rows[myRow.RowIndex].Cells[11].Text;
                txtTown.Text = gdvLocations.Rows[myRow.RowIndex].Cells[12].Text;
                txtCity.Text = gdvLocations.Rows[myRow.RowIndex].Cells[13].Text;
                txtProvince.Text = gdvLocations.Rows[myRow.RowIndex].Cells[14].Text;
                txtCountry.Text = gdvLocations.Rows[myRow.RowIndex].Cells[15].Text;
                txtPlanet.Text = gdvLocations.Rows[myRow.RowIndex].Cells[16].Text;
                txtGPSLat.Text = gdvLocations.Rows[myRow.RowIndex].Cells[17].Text;
                txtGPSLong.Text = gdvLocations.Rows[myRow.RowIndex].Cells[18].Text;            

                lblAdd.Text = "Update";
                btnAddLocation.Text = "Update";
            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;
                removeLocation(Convert.ToInt32(ViewState["Id"]));
                //Refresh Grid
                populateGridView();
            }

        }

        public void LoadDefaultIcons()
        {
            string sqlQuery = "select top(1) LocationIcon from DefaultIcons";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlQuery;
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pstrIcon = (byte[])reader["LocationIcon"];
            }

            reader.Close();
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            //ASSIGN DEFAULT ICONS
            string base64StringpstrNormal = Convert.ToBase64String(pstrIcon, 0, pstrIcon.Length);
            imgIcon.ImageUrl = Convert.ToString("data:image/png;base64,") + base64StringpstrNormal;
        }

        public void Clear()
        {
            txtBuilding.Text = "";
            txtCabinet.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtFloor.Text = "";
            txtGPSLat.Text = "";
            txtGPSLong.Text = "";
            txtPlanet.Text = "";
            txtProvince.Text = "";
            txtRoom.Text = "";
            txtStreet.Text = "";
            txtSuburb.Text = "";
            txtTown.Text = "";
            txtUnit.Text = "";
        }

        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            string temp = btnAddLocation.Text;

            if (temp.Contains("Add"))
            {
                addLocation();
                populateGridView();
                Clear();
            }
            else if (temp.Contains("Update"))
            {
                int Id = int.Parse(ViewState["Id"].ToString());
                SqlParameter[] myParams = new SqlParameter[15];
                try
                {
                    myParams[0] = new SqlParameter();
                    myParams[0].ParameterName = "@ID";
                    myParams[0].Value = Id;
                    myParams[1] = new SqlParameter();
                    myParams[1].ParameterName = "@Cabinet";
                    myParams[1].Value = txtCabinet.Text;
                    myParams[2] = new SqlParameter();
                    myParams[2].ParameterName = "@Room";
                    myParams[2].Value = txtRoom.Text;
                    myParams[3] = new SqlParameter();
                    myParams[3].ParameterName = "@Floor";
                    myParams[3].Value = txtFloor.Text;
                    myParams[4] = new SqlParameter();
                    myParams[4].ParameterName = "@Building";
                    myParams[4].Value = txtBuilding.Text;
                    myParams[5] = new SqlParameter();
                    myParams[5].ParameterName = "@Unit";
                    myParams[5].Value = txtUnit.Text;
                    myParams[6] = new SqlParameter();
                    myParams[6].ParameterName = "@Street";
                    myParams[6].Value = txtStreet.Text;
                    myParams[7] = new SqlParameter();
                    myParams[7].ParameterName = "@Suburb";
                    myParams[7].Value = txtSuburb.Text;
                    myParams[8] = new SqlParameter();
                    myParams[8].ParameterName = "@Town";
                    myParams[8].Value = txtTown.Text;
                    myParams[9] = new SqlParameter();
                    myParams[9].ParameterName = "@City";
                    myParams[9].Value = txtCity.Text;
                    myParams[10] = new SqlParameter();
                    myParams[10].ParameterName = "@Province";
                    myParams[10].Value = txtProvince.Text;
                    myParams[11] = new SqlParameter();
                    myParams[11].ParameterName = "@Country";
                    myParams[11].Value = txtCountry.Text;
                    myParams[12] = new SqlParameter();
                    myParams[12].ParameterName = "@Planet";
                    myParams[12].Value = txtPlanet.Text;
                    myParams[13] = new SqlParameter();
                    myParams[13].ParameterName = "@GPSLat";
                    myParams[13].Value = txtGPSLat.Text;
                    myParams[14] = new SqlParameter();
                    myParams[14].ParameterName = "@GPSLong";
                    myParams[14].Value = txtGPSLong.Text;

                    MyDataAccess.ExecCmdQueryParams("location_update", myParams);
                    populateGridView();
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message;
                    Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }

                btnAddLocation.Text = "Add";
                successMessage.Visible = false;
                Clear();
            }
        }

        private void populateGridView()
        {
            try
            {
                myDS = myDA.ExecCmdQueryNoParamsDS("location_select_all");
                gdvLocations.DataSource = myDS.Tables[0];
                gdvLocations.DataBind();
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = ex.Message;

                Trace.Write("LocationsSetup.populateGridView: Error: " + ex.Message);
            }
        }

        private void addLocation()
        {
            System.Data.SqlClient.SqlParameter[] myparams = new System.Data.SqlClient.SqlParameter[17];
            try
            {
                myparams[0] = new System.Data.SqlClient.SqlParameter();
                myparams[0].ParameterName = "@DefaultLocation";
                myparams[0].Value = false;
                //cboDevices.SelectedValue

                myparams[1] = new System.Data.SqlClient.SqlParameter();
                myparams[1].ParameterName = "@TypeDS";
                myparams[1].Value = 0;
                //cboType.SelectedValue

                myparams[2] = new System.Data.SqlClient.SqlParameter();
                myparams[2].ParameterName = "@Cabinet";
                myparams[2].Value = txtCabinet.Text;

                myparams[3] = new System.Data.SqlClient.SqlParameter();
                myparams[3].ParameterName = "@Room";
                myparams[3].Value = txtRoom.Text;

                myparams[4] = new System.Data.SqlClient.SqlParameter();
                myparams[4].ParameterName = "@Floor";
                myparams[4].Value = txtFloor.Text;

                myparams[5] = new System.Data.SqlClient.SqlParameter();
                myparams[5].ParameterName = "@Building";
                myparams[5].Value = txtBuilding.Text;

                myparams[6] = new System.Data.SqlClient.SqlParameter();
                myparams[6].ParameterName = "@Unit";
                myparams[6].Value = txtUnit.Text;

                myparams[7] = new System.Data.SqlClient.SqlParameter();
                myparams[7].ParameterName = "@Street";
                myparams[7].Value = txtStreet.Text;

                myparams[8] = new System.Data.SqlClient.SqlParameter();
                myparams[8].ParameterName = "@Suburb";
                myparams[8].Value = txtSuburb.Text;

                myparams[9] = new System.Data.SqlClient.SqlParameter();
                myparams[9].ParameterName = "@Town";
                myparams[9].Value = txtTown.Text;

                myparams[10] = new System.Data.SqlClient.SqlParameter();
                myparams[10].ParameterName = "@City";
                myparams[10].Value = txtCity.Text;

                myparams[11] = new System.Data.SqlClient.SqlParameter();
                myparams[11].ParameterName = "@Province";
                myparams[11].Value = txtProvince.Text;

                myparams[12] = new System.Data.SqlClient.SqlParameter();
                myparams[12].ParameterName = "@Country";
                myparams[12].Value = txtCountry.Text;

                myparams[13] = new System.Data.SqlClient.SqlParameter();
                myparams[13].ParameterName = "@Planet";
                myparams[13].Value = txtPlanet.Text;

                myparams[14] = new System.Data.SqlClient.SqlParameter();
                myparams[14].ParameterName = "@GPSLat";
                myparams[14].Value = txtGPSLat.Text;

                myparams[15] = new System.Data.SqlClient.SqlParameter();
                myparams[15].ParameterName = "@GPSLong";
                myparams[15].Value = txtGPSLong.Text;
                byte[] byteIcon = null;
                System.Drawing.Image imgicon = null;
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
                //Icon
                try
                {
                    if (filImageIcon.FileName.Trim().Length == 0)
                    {
                        byteIcon = pstrIcon;
                    }
                    else
                    {
                        imgicon = Myfunc.Strip_Image(this.filImageIcon);
                        byteIcon = MyRem.ImagetoByte(imgicon, ImageFormat.Bmp);
                    }
                }
                catch
                {
                }
                myparams[16] = new System.Data.SqlClient.SqlParameter();
                myparams[16].ParameterName = "@LocationIcon";
                myparams[16].Value = byteIcon;

                myDA.ExecCmdQueryParams("location_add_new", myparams);
                try
                {
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();

                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];

                    successMessage.Visible = true;
                    lblSucces.Text = "Location add new Succeeded.";

                    MyRem.WriteLog("location_add_new Succeeded", "User:" + MyUser.ID.ToString());

                }
                catch (Exception ex)
                {
                }

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Add Location Setup failed.";

                Trace.Write("LocationsSetup.addLocation: Error: " + ex.Message);
            }
        }

        private void removeLocation(int id)
        {
            System.Data.SqlClient.SqlParameter[] myparams = new System.Data.SqlClient.SqlParameter[1];
            try
            {
                myparams[0] = new System.Data.SqlClient.SqlParameter();
                myparams[0].ParameterName = "@ID";
                myparams[0].Value = Convert.ToInt32(id);
                myDA.ExecCmdQueryParams("location_delete", myparams);

            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                lblError.Text = "Remove Location Setup failed.";

                Trace.Write("LocationsSetup.removeLocation: Error: " + ex.Message);
            }
        }
        public void LocationsSetup()
        {
            Load += Page_Load;
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}