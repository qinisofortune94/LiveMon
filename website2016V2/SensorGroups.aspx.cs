using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using LiveMonitoring;
using System.Drawing;
using System.Drawing.Imaging;
using Infragistics.WebUI.UltraWebGrid;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using Microsoft.VisualBasic;

namespace website2016V2
{
    public partial class SensorGroups : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!Page.IsPostBack)
                {
                    if (((string)Session["LoggedIn"] == "True"))
                    {
                        LiveMonitoring.IRemoteLib.UserDetails MyUser1 = new LiveMonitoring.IRemoteLib.UserDetails();
                        MyUser1 = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                        Label user = this.Master.FindControl("lblUser") as Label;
                        Label loginUser = this.Master.FindControl("lblUser2") as Label;
                        Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                        loginUser.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                        user.Text = ((MyUser1.FirstName + (" " + MyUser1.SurName)));
                        LastLogin.Text = " LL:" + MyUser1.LoginDT.ToString();
                        successMessage.Visible = false;
                        warningMessage.Visible = false;
                        errorMessage.Visible = false;

                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                    //ok logged on level ?
                    string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                    LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                    LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                    int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                    MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                    if (MyIPMonPageSecure > MyUser.UserLevel)
                    {
                        Response.Redirect("~/NotAuthorisedView.aspx");
                    }

                    divEdit.Visible = false;
                    DivNew.Visible = true;
                    LoadGroups();

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            LoadGroups();
            if (gridSensorGroups.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gridSensorGroups.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gridSensorGroups.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gridSensorGroups.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        public bool SaveSensorGroup()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.SensorGroup NewSensorGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
            NewSensorGroup.SensorGroupName = txtGroup.Text.ToUpper();
            NewSensorGroup.SensorGroupDisc = txtDescription.Text.ToUpper();
            NewSensorGroup.SensorGroupOrder = 1;
            int Myresp = MyRem.LiveMonServer.AddNewSensorGroup(NewSensorGroup);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        public bool EditSensorGroup()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            LiveMonitoring.IRemoteLib.SensorGroup NewSensorGroup = new LiveMonitoring.IRemoteLib.SensorGroup();
            NewSensorGroup.SensorGroupName = txtGroup.Text.ToUpper();
            NewSensorGroup.SensorGroupDisc = txtDescription.Text.ToUpper();
            NewSensorGroup.SensorGroupOrder = 1;
            NewSensorGroup.SensorGroupID = Convert.ToInt32(ViewState["Id"]);

            int Myresp = MyRem.LiveMonServer.EditSensorGroup(NewSensorGroup);
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                divEdit.Visible = false;
                DivNew.Visible = true;

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string temp = btnSave.Text;
            try
            {
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedEdit.aspx");
                }

                if (CanSave() == true)
                {

                    if (temp.Contains("Add"))
                    {
                        if (SaveSensorGroup() == true)
                        {
                            try
                            {
                                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                MyRem.WriteLog("Sensor group Saved", "User:" + MyUser.ID.ToString() + "|" + txtGroup.Text + "|" + txtDescription.Text);

                            }
                            catch (Exception ex)
                            {
                            }
                            successMessage.Visible = true;
                            lblSuccess.Text = "Sensor group was successfully saved.";
                            txtDescription.Text = "";
                            txtGroup.Text = "";
                            LoadGroups();

                        }
                        else
                        {
                            errorMessage.Visible = true;
                            lblError.Text = "An error occured while trying to save group, please try again.";
                        }
                    }
                    else
                    if(temp.Contains("Update"))
                    {           
                        if (EditSensorGroup() == true)
                        {
                            successMessage.Visible = true;
                            lblSuccess.Text = "Sensor group was successfully saved.";
                            
                            try
                            {
                                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                                MyRem.WriteLog("Sensor group Edited", "User:" + MyUser.ID.ToString() + "|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                            }
                            catch (Exception ex)
                            {
                            }
                            txtEditDescription.Text = "";
                            txtEditGroup.Text = "";
                            LoadGroups();
                            divEdit.Visible = false;
                        }
                        else
                        {
                            errorMessage.Visible = true;
                           lblError.Text = "An error occured while trying to save group, please try again.";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = ex.Message;
            }
        }
        public bool CanSave()
        {

            if (txtGroup.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please enter grouping name.";
                txtGroup.Focus();
                return false;
            }
            if (txtDescription.Text.Trim().Length == 0)
            {
                warningMessage.Visible = true;
               
                lblWarning.Text = "Please enter grouping description.";
                txtDescription.Focus();
                return false;
            }
            return true;
        }
        public bool CanEdit()
        {

            if (txtEditGroup.Text.Trim().Length == 0)
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Please enter grouping name.";
                txtGroup.Focus();
                return false;
            }
            if (txtEditDescription.Text.Trim().Length == 0)
            {
                lblWarning.Visible = true;
                lblWarning.Text = "Please enter grouping description.";
                txtDescription.Focus();
                return false;
            }
            return true;
        }

        protected void gvSample_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gridSensorGroups.DataKeys[myRow.RowIndex].Value.ToString();
           

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string GroupID = gridSensorGroups.Rows[myRow.RowIndex].Cells[2].Text;
                string Group = gridSensorGroups.Rows[myRow.RowIndex].Cells[3].Text;
                string Description = gridSensorGroups.Rows[myRow.RowIndex].Cells[4].Text;



                ViewState["Id"] = Id;

                lblID.Visible = true;
                lblID.Text = GroupID;
                txtGroup.Text = Group;
                txtDescription.Text = Description;


                lblAdd.Text = "Update";
                btnSave.Text = "Update";
               
            }

            else if (commandName == "DeleteItem")
            {

                ViewState["Id"] = Id;


                if (CanDelete() == true)
                {
                    if (DeleteSensorGroup() == true)
                    {
                        try
                        {
                            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                            MyRem.WriteLog("Sensor group Deleted", "User:|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                        }
                        catch (Exception ex)
                        {
                        }
                        successMessage.Visible = true;
                        lblSuccess.Text = "Sensor group was Deleted.";
                        txtDescription.Text = "";
                        txtGroup.Text = "";
                        LoadGroups();
                        divEdit.Visible = false;
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Visible = true;
                       lblError.Text = "An error occured while trying to Delete group, please try again.";
                    }
                }

                //SampleLogic business = new SampleLogic();

                //int RecordId = Convert.ToInt16(Id);
                //business.Delete(RecordId);

                ////Refresh Grid
                //LoadData();
            }
        }
        public void LoadGroups()
        {
            gridSensorGroups.DataSource = getGroups();
            gridSensorGroups.DataBind();

        }
        private DataTable getGroups()
        {

            string sqlQuery = "sensor_groups_getgroups";
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedEdit.aspx");
                }
                if (CanEdit() == true)
                {
                    if (EditSensorGroup() == true)
                    {
                        lblSuccess.Text = "Sensor group was successfully saved.";
                        try
                        {
                            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                            MyRem.WriteLog("Sensor group Edited", "User:" + MyUser.ID.ToString() + "|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                        }
                        catch (Exception ex)
                        {
                        }
                        txtEditDescription.Text = "";
                        txtEditGroup.Text = "";
                        LoadGroups();
                        divEdit.Visible = false;
                    }
                    else
                    {
                        lblSuccess.Text = "An error occured while trying to save group, please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        protected void gridSensorGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DivNew.Visible = false;
                divEdit.Visible = true;
                lblAdd.Text = "Update";
                btnSave.Text = "Update";
                txtEditGroup.Text = gridSensorGroups.SelectedRow.Cells[2].Text;
                txtEditDescription.Text = gridSensorGroups.SelectedRow.Cells[3].Text;
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        protected void gridSensorGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gridSensorGroups.PageIndex = e.NewPageIndex;
                gridSensorGroups.DataSource = getGroups();
                gridSensorGroups.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();

            int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedDelete.aspx");
            }
            try
            {
                if (CanDelete() == true)
                {
                    if (DeleteSensorGroup() == true)
                    {
                        try
                        {
                            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                            MyRem.WriteLog("Sensor group Deleted", "User:" + MyUser.ID.ToString() + "|" + txtEditGroup.Text + "|" + txtEditDescription.Text);

                        }
                        catch (Exception ex)
                        {
                        }
                        lblSuccess.Text = "Sensor group was Deleted.";
                        txtEditDescription.Text = "";
                        txtEditGroup.Text = "";
                        LoadGroups();
                        divEdit.Visible = false;
                    }
                    else
                    {
                        lblSuccess.Text = "An error occured while trying to Delete group, please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }
        public bool CanDelete()
        {
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                int MyDiv = 1;
                bool added = false;
                if ((MyCollection == null))
                    return true;
                bool Refrenced = false;
                string RefrencedName = "";

                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    try
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails Mysensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            try
                            {
                                if (gridSensorGroups.SelectedRow.Cells[1].Text == Mysensor.SensGroup.SensorGroupID.ToString())
                                {
                                    Refrenced = true;
                                    RefrencedName += Mysensor.Caption + ",";
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.CameraDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.CameraDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.CameraDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.IPDevicesDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.IPDevicesDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.IPDevicesDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.OtherDevicesDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.OtherDevicesDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SNMPManagerDetails Then
                        // Dim Mysensor As LiveMonitoring.IRemoteLib.SNMPManagerDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                        //End If
                        //cmbSensGroup
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorGroup Then
                        // Dim MysensorGroup As LiveMonitoring.IRemoteLib.SensorGroup = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorGroup)
                        //End If
                        //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.LocationDetails Then
                        // Dim MyLocation As LiveMonitoring.IRemoteLib.LocationDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.LocationDetails)
                        //End If



                    }
                    catch (Exception ex)
                    {
                    }

                }
                if (Refrenced)
                {
                    lblError.Visible = true;
                    lblError.Text = "Cannot delete ,Group is in use for " + RefrencedName;
                    txtEditGroup.Focus();
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return true;
        }
        public bool DeleteSensorGroup()
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
            bool response = true;
            int Myresp =Convert.ToInt32(MyRem.LiveMonServer.DeleteSensorGroup(Convert.ToInt32(ViewState["Id"])));
            if (Myresp == 0)
            {
                response = false;
            }
            return response;
        }
        public void SensorGroups_SensorGroups()
        {
            Load += Page_Load;
        }


       

        protected void btnSave_Click1(object sender, EventArgs e)
        {

        }

        protected void btnClearNewSensorGroup_Click(object sender, EventArgs e)
        {

        }

        protected void btnEdit_Click1(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click1(object sender, EventArgs e)
        {

        }

        protected void gridSensorGroups_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {

        }
    }
}