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
    public partial class LinkTemplates : System.Web.UI.Page
    {
        private LiveMonitoring.IRemoteLib.AlertDetails AlertDetails = new LiveMonitoring.IRemoteLib.AlertDetails();
        private List<LiveMonitoring.IRemoteLib.AlertContactDef> AlertContacts = new List<LiveMonitoring.IRemoteLib.AlertContactDef>();
        private List<LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef> AlertThreasholds = new List<LiveMonitoring.IRemoteLib.AlertDetails.AlertThreshholdsDef>();
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        private int MyAlertId;
        LiveMonitoring.SharedFuncs Myfunc = new LiveMonitoring.SharedFuncs();
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

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }

                if(Page.IsPostBack == false)
                {
                    cmbTemplates.Items.Clear();
                    Myfunc.FillTemplates(cmbTemplates, 0, conStr);
                    DeleteSensorTemplates();
                    // cmbTemplates.SelectedIndex = -1
                    if (cmbTemplates.SelectedItem != null)
                    {
                        Load_Sensors(Convert.ToInt32(cmbTemplates.SelectedValue));
                    }
                    else
                    {
                        Load_Sensors(0);
                    }
                }
               
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public bool DeleteSpecificSensorTemplate(int pintId)
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertstemplates_SensorToTemplate_Delete";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramID).Value = pintId;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
                return Saved;
            }

            return Saved;
        }

        public bool DeleteSensorTemplates()
        {
            bool Saved = false;
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertstemplates_SensorToTemplate_DeleteAll";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramUserID).Value = MyUser.ID;
                cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                Saved = true;
            }
            catch (Exception ex)
            {
            }

            return Saved;
        }

        public int TemplateTypeId(int TemplateId)
        {
            int ID = 0;
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                SqlCommand cmd = new SqlCommand();
                SqlParameter paramTemplateId = new SqlParameter("@TemplateId", SqlDbType.Int);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = "alertstemplates_Get_TemplateType";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                cmd.Parameters.Clear();

                cmd.Parameters.Add(paramTemplateId).Value = TemplateId;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ID = (int)reader["TemplateTypeID"];
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch (Exception ex)
            {
            }

            return ID;
        }

        public void LoadSensorTemplates()
        {
            GridSensorTemplates.DataSource = getTempSensorTempates();
            GridSensorTemplates.DataBind();
        }

        private DataTable getTempSensorTempates()
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];


            string sqlQuery = "alertstemplates_SensorToTemplate_Select";
            SqlConnection connection = new SqlConnection(conStr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("UserID", MyUser.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dt;
        }

        public void Load_Sensors(int TemplateId)
        {
            //CameraMenu
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            object MyObject1 = null;
            bool added = false;
            Array ItemValue = System.Enum.GetValues(typeof(LiveMonitoring.IRemoteLib.SensorDetails.SensorType));
            Array Item = System.Enum.GetNames(typeof(LiveMonitoring.IRemoteLib.SensorDetails.SensorType));

            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (MySensor.Type == (LiveMonitoring.IRemoteLib.SensorDetails.SensorType)TemplateTypeId(TemplateId))
                    {
                        TreeNode node = FindNode(MySensor.SensGroup.SensorGroupName);
                        if ((node == null))
                        {
                            TreeNode node1 = new TreeNode();
                            node1.ShowCheckBox = false;
                            node1.Text = MySensor.SensGroup.SensorGroupName;
                            //Item(CInt(MySensor.Type))
                            node1.Value = MySensor.SensGroup.SensorGroupID.ToString();
                            //CInt(MySensor.Type)
                            node1.Expanded = false;
                            tvSensors.Nodes.Add(node1);
                            node = FindNode(MySensor.SensGroup.SensorGroupName);
                        }

                        TreeNode subnode = new TreeNode();
                        subnode.ShowCheckBox = true;
                        subnode.Text = MySensor.Caption;
                        subnode.Value = MySensor.ID.ToString();
                        node.ChildNodes.Add(subnode);
                    }
                }
            }

        }

        private TreeNode FindNode(string nodeName)
        {
            try
            {
                for (int mycnt = 0; mycnt <= tvSensors.Nodes.Count - 1; mycnt++)
                {
                    if (tvSensors.Nodes[mycnt].Text == nodeName)
                    {
                        return tvSensors.Nodes[mycnt];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void GridSensorTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    DeleteSpecificSensorTemplate(Convert.ToInt32(GridSensorTemplates.SelectedRow.Cells[3].Text));
                    LoadSensorTemplates();
                }
                catch (Exception ex)
                {
                    lblSuccess.Text = ex.Message;
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void FillSessionSensors()
        {
            System.Web.UI.WebControls.ListItem MyItem = new System.Web.UI.WebControls.ListItem();
            Collection MyCollection = new Collection();
            //clear session var
            Session["Sensors"] = "";
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            foreach (TreeNode Mynode in tvSensors.Nodes)
            {
                foreach (TreeNode subnode in Mynode.ChildNodes)
                {
                    if (subnode.Checked)
                    {
                        Session["Sensors"] += subnode.Value.ToString() + ",";
                    }
                }
            }

        }

        protected void btnApplyAlerts_Click(object sender, EventArgs e)
        {
            try
            {
                lblSuccess.Text = "";
                FillSessionSensors();
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                int i = 0;
                string[] mySensors = Strings.Split(Session["Sensors"].ToString(), ",");
                int Acnt = 0;
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
                //GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                //last one is empty  
                for (Acnt = 0; Acnt <= Information.UBound(mySensors) - 1; Acnt++)
                {
                    int MyCnt = 0;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            if (Convert.ToInt32(mySensors[Acnt]) == MySensor.ID)
                            {
                                if (Myfunc.AddTemplateToAlert(Convert.ToInt32(cmbTemplates.SelectedValue), MySensor.ID, MyUser.ID, conStr) == true)
                                {
                                    try
                                    {
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    lblSuccess.Text = "System failed to connect to the remote database, please check your connection and try again!";
                                }
                            }
                        }
                    }
                }
                lblSuccess.Text = "Alerts bulk configuration completed successfully";
                cmbTemplates.SelectedIndex = -1;
                tvSensors.Nodes.Clear();
                DeleteSensorTemplates();
                LoadSensorTemplates();

            }
            catch (Exception ex)
            {
                lblSuccess.Text = ex.Message;
            }
        }

        protected void cmbTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tvSensors.Nodes.Clear();
                Load_Sensors(Convert.ToInt32(cmbTemplates.SelectedValue));
            }
            catch (Exception ex)
            {
            }
        }
        public void Alerts_Templates_LinkTemplates()
        {
            Load += Page_Load;
        }
    }
}