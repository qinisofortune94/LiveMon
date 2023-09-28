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

                //txtFirstName.Text = gridPeople.Rows[myRow.RowIndex].Cells[3].Text;
                //txtLastName.Text = gridPeople.Rows[myRow.RowIndex].Cells[4].Text;
                //txtEmail.Text = gridPeople.Rows[myRow.RowIndex].Cells[5].Text;
                //txtCell.Text = gridPeople.Rows[myRow.RowIndex].Cells[6].Text;
                //txtTelephone.Text = gridPeople.Rows[myRow.RowIndex].Cells[7].Text;
                //txtFax.Text = gridPeople.Rows[myRow.RowIndex].Cells[8].Text;
                //txtAddress.Text = gridPeople.Rows[myRow.RowIndex].Cells[9].Text;

                //lblAdd.Text = "Update";
                //btnSave.Text = "Update";

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