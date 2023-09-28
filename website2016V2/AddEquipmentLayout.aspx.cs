using Microsoft.VisualBasic;
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
    public partial class AddEquipmentLayout : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        string sensors;
        string parents;

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
            successMessage.Visible = false;
            errorMessage.Visible = false;
            warningMessage.Visible = false;
            if (!IsPostBack)
            {
                ddlSensor.Items.Clear();
               
                ddlSensorParent.Items.Clear();
          
                LoadSensors();
            }
            
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridNewSensors.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                ViewState["Id"] = Id;
                //Accessing BoundField Column
                sensors = gridNewSensors.Rows[myRow.RowIndex].Cells[3].Text;
                this.SensorsID.Value = sensors;
                parents = gridNewSensors.Rows[myRow.RowIndex].Cells[4].Text;
                this.ParentsID.Value = parents;
                ddlSensor.SelectedItem.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[3].Text;
                ddlSensorParent.SelectedItem.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[4].Text;
                ddlExtraBool.SelectedItem.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[5].Text;
                ddExtraBool1.SelectedItem.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[6].Text;
                txtExtraValue.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[7].Text;
                txtExtraValue1.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[8].Text;
                txtExtraData.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[9].Text;
                txtExtraData1.Text = gridNewSensors.Rows[myRow.RowIndex].Cells[10].Text;

                lblAdd.Text = "Update";
                cmdSend.Text = "Update";

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

                    MyDataAccess.ExecCmdQueryParamsDS("EquipmentLayout_delete", myParams);
                    successMessage.Visible = true;
                    lblSucces.Text = "Sensor Succesfully deleted";
                    //LoadData();
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message; //"Unable to delete the record, Delete not poosible!";
                    //Trace.Write("Website.SitesUserSetup.btnRemove_Click: Error: " + ex.Message.ToString());
                }
                ViewState["Id"] = Id;
                LoadData();


            }

        }
        protected void cmdSend_Click(object sender, EventArgs e)
        {
            
            //Check button text to add or update
            string temp = cmdSend.Text;
            if (temp.Contains("Add"))
            {
                if (addEquipment() == true)
                {
                    successMessage.Visible = true;
                    lblSucces.Text = "Details were successfully saved.";
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
                   // LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
                    bool response = true;
                    DataTable mydt = new DataTable();
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    LiveMonitoring.IRemoteLib.EquipmentLayoutDetails equipmentDetails = new LiveMonitoring.IRemoteLib.EquipmentLayoutDetails();
                    //foreach (ListItem ltItem in ddlSensor.Items)
                    //{
                    //    if (ltItem.Text == SensorsID.Value)
                    //    {
                    //        //Interaction.MsgBox(ltItem.Value);
                    //        equipmentDetails.SensorID = Convert.ToInt32(ltItem.Value);
                    //    }

                    //}
                    //foreach (ListItem ltItem2 in ddlSensorParent.Items)
                    //{
                    //    if (ltItem2.Text == ParentsID.Value)
                    //    {
                    //        //Interaction.MsgBox(ltItem2.Value);
                    //        equipmentDetails.ParentID = Convert.ToInt32(ltItem2.Value);
                    //    }

                    //}
                    if (this.SensorsID.Value==ddlSensor.SelectedItem.Text)
                    {
                        Interaction.MsgBox(ddlSensor.SelectedValue);
                    }
                    if(ParentsID.Value== ddlSensorParent.SelectedItem.Text)
                    {
                        Interaction.MsgBox(ddlSensorParent.SelectedValue);
                    }
                    equipmentDetails.SensorID = Convert.ToInt32(ddlSensor.SelectedValue);
                    equipmentDetails.ParentID = Convert.ToInt32(ddlSensorParent.SelectedValue);                
                    equipmentDetails.ExtraBool = (Convert.ToBoolean(this.ddlExtraBool.SelectedValue));
                    equipmentDetails.ExtraBool1 = (Convert.ToBoolean(this.ddExtraBool1.SelectedValue));
                    equipmentDetails.ExtraData = (this.txtExtraData.Text);
                    equipmentDetails.ExtraData1 = (this.txtExtraData1.Text);
                    equipmentDetails.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
                    equipmentDetails.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
                   // equipmentDetails.ID = (Convert.ToInt32(this.txtParentID.Text));
                 

                    equipmentDetails.ID = Id;
                    response = MyRem.LiveMonServer.EditEquipmentLayout(equipmentDetails);
                    //return response;
                    successMessage.Visible = true;
                    lblSucces.Text = "Successfully updated";
                    Clear();
                }
                catch (Exception ex)
                {
                    errorMessage.Visible = true;
                    lblError.Text = ex.Message.ToString();
                }

                cmdSend.Text = "Add";
                successMessage.Visible = false;
                Clear();
            }

            //Refresh Grid
            LoadData();

            
        }

        protected void BtnClearNewSensor_Click(object sender, EventArgs e)
        {
            Clear();
        }
        

        public bool addEquipment()
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.get_GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.IRemoteLib.EquipmentLayoutDetails  equipmentDetails= new LiveMonitoring.IRemoteLib.EquipmentLayoutDetails();

            equipmentDetails.ExtraBool = (Convert.ToBoolean(this.ddlExtraBool.SelectedValue));
            equipmentDetails.ExtraBool1 = (Convert.ToBoolean(this.ddExtraBool1.SelectedValue));     
            equipmentDetails.ExtraData = (this.txtExtraData.Text);
            equipmentDetails.ExtraData1 = (this.txtExtraData1.Text);          
            equipmentDetails.ExtraValue = Convert.ToDouble(this.txtExtraValue.Text);
            equipmentDetails.ExtraValue1 = Convert.ToDouble(this.txtExtraValue1.Text);
           // equipmentDetails.ID = (Convert.ToInt32(this.txtParentID.Text));
            equipmentDetails.SensorID = Convert.ToInt32(this.ddlSensor.SelectedValue);
            equipmentDetails.ParentID = Convert.ToInt32(this.ddlSensorParent.SelectedValue);
            //NewSensor.ScanRate = CDbl(Me.txtScanRate.Value)
            
            bool Myresp = MyRem.LiveMonServer.AddEquipmentLayout(equipmentDetails);


            return Myresp;

        }
     

        public void Clear()
        {
            ddlSensor.SelectedIndex = -1;
            ddlSensorParent.SelectedIndex = -1;
            txtExtraValue.Text = "";
            txtExtraValue1.Text ="";
            txtExtraData.Text = "";
            txtExtraData1.Text = "";

        }
        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadData();
            if (gridNewSensors.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridNewSensors.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridNewSensors.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridNewSensors.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        public void LoadData()
        {
            gridNewSensors.DataSource = getEqupment();
            gridNewSensors.DataBind();


        }
        private DataTable getEqupment()
        {
            try
            {
                string sqlQuery = "Get_All_EquipmentLayout";
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
                lblError.Text = ex.Message;
                errorMessage.Visible = true;
                return null;
            }
        }
        public void LoadSensors()
        {
            ddlSensor.Items.Clear();
            ddlSensorParent.Items.Clear();
           // ddlSensor.SelectedIndex = 0;
           // ddlSensorParent.SelectedIndex = 0;
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

                            ddlSensor.Items.Add(MyIttem);
                            ddlSensorParent.Items.Add(MyIttem2);
                            if(added)
                            {
                                MyIttem.Selected = true;
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


  

        protected void ddlSensorParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            Interaction.MsgBox(ddlSensor.SelectedValue);  
        }

        protected void ddlSensor_SelectedIndexChanged1(object sender, EventArgs e)
        {
            Interaction.MsgBox(ddlSensorParent.SelectedValue);
        }
    }
}