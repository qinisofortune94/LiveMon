
using LiveMonitoring;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class DashboardConfig : System.Web.UI.Page
    {
        private static DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {

                getDashboards();
                btnSave.Text = "Show";

            }
            else
            {

            }
        }

        public void getDashboards()
        {

            SqlDataReader functionReturnValue = default(SqlDataReader);

            SqlDataReader dataReader = MyDataAccess.ExecCmdQueryNoParams("[dbo].[GetDashboards]");
            String Dash = "";
            Dashboards.Items.Clear();
            Dashboards.Items.Add("");
            Dashboards.Items.Add("---Add Dashboard---");
            try
            {
                while (dataReader.Read())
                {
                    Dash = Convert.ToString(dataReader["DashboardName"]);
                    Dashboards.Items.Add(Dash);
                }

            }
            catch (Exception)
            {
                
            }
        }

        protected void Dashboards_SelectedIndexChanged(object sender, EventArgs e)
        {
            
         
            if (Dashboards.SelectedIndex == 1 )
            {
              //  txtDash.Text = "";
                OtherDIV.Visible = true;
                getSensors();
                btnSave.Text = "Save";
                txtDash.ReadOnly = false;
             
            }
            else
            {
             
                OtherDIV.Visible = false;
                Session["DashName"] = Dashboards.SelectedValue;
                btnSave.Visible = true;
                btnDelete.Visible = true;
                btnSave.Text = "Show";
                btnDelete.Text = "Edit/Delete";
              
                //   Response.Redirect("DashboardKVA.aspx?DashboardName="+ Dashboards.SelectedValue,true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();

        }

        public void getSensors()
        {


            SqlDataReader dataReader = MyDataAccess.ExecCmdQueryNoParams("[dbo].[MeteringGetProfBySensorDateNMD]");
            String Dash;
            Sens.Items.Clear();
            while (dataReader.Read())
            {
                Dash = Convert.ToString(dataReader["Caption"]);
                ListItem item = new ListItem(Dash, Convert.ToString(dataReader["ID"]));
                Sens.Items.Add(item);
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Edit();

        }

        public void Edit()
        {

            if (btnDelete.Text == "Edit/Delete")
            {
                txtDash.ReadOnly = true;
                OtherDIV.Visible = true;
                btnSave.Text = "Save";
                SqlParameter[] parameters =
                                 {
                                    new SqlParameter("@DashboardName",Dashboards.SelectedValue),

                                 };

                SqlDataReader dataReader = MyDataAccess.ExecCmdQueryParams("[dbo].[GetSelectedSensors]", parameters);

                txtDash.Text = Dashboards.SelectedValue;
                Dashboards.SelectedIndex = 0;
                getSensors();

                while (dataReader.Read())

                {
                    String Cap = Convert.ToString(dataReader["Caption"]);
                    for (int i = 0; i <= Sens.Items.Count - 1; i++)
                    {

                        if (Sens.Items[i].Text == Cap)
                        {


                            Sens.Items[i].Selected = true;

                        }

                    }

                }

            }

            if (btnDelete.Text == "Delete")
            {
                SqlParameter[] parameters =
                                  {
                                    new SqlParameter("@Dashboard",txtDash.Text),

                                 };
                MyDataAccess.ExecCmdQueryParams("[dbo].[DeleteDashboard]", parameters);
                btnSave.Text = "Show";
                btnDelete.Text = "Edit/Delete";
                OtherDIV.Visible = false;
                getDashboards();
              
            }
            else
            {
                btnSave.Text = "Save";
                btnDelete.Text = "Delete";
            }
        }

        public void Save()
        {

          
            if (btnSave.Text == "Save")


            {
             
                ListItem item = Dashboards.Items.FindByText(txtDash.Text);
                
                if (item != null)
                {
                    String str1 = "";





                    for (int i = 0; i <= Sens.Items.Count - 1; i++)
                    {

                        str1 = Sens.Items[i].Value;

                        if (Sens.Items[i].Selected)
                        {
                            SqlParameter[] parameters1 =
                                    {
                                    new SqlParameter("@DashboardName",txtDash.Text),
                                    new SqlParameter("@SensorID",Convert.ToDouble(str1))

                                };
                            MyDataAccess.ExecCmdQueryParams("[dbo].[updateDash]", parameters1);


                           
                        }

                        else
                        {

                            SqlParameter[] parameters1 =
                                   {
                                    new SqlParameter("@DashboardName",txtDash.Text),
                                    new SqlParameter("@SensorID",Convert.ToDouble(str1))

                                };
                           MyDataAccess.ExecCmdQueryParams("[dbo].[updateDash1]", parameters1);
                        //    int T = 0;
                        //    while (reader.Read())
                        //    {
                        //        T = Convert.ToInt16(reader["Value"]);
                        //        if (T == 1)
                        //            Message.Visible = true;
                        //    }
                        }


                        

                    }
                    if(Type.SelectedIndex == 1)

                    Response.Redirect("DashboardKVA.aspx?DashboardName=" + txtDash.Text, true);
                    else if (Type.SelectedIndex == 2)
                    {
                        Response.Redirect("KVAGraphDisplay.aspx?DashboardName=" + txtDash.Text, true);
                    }
                    else
                    {
                        Message.Visible = true;
                        btnSave.Text = "Save";
                    }


                }

                else
                {
                    String str = "";
                    for (int i = 0; i <= Sens.Items.Count - 1; i++)
                    {
                  

                        str = Sens.Items[i].Value;
                        if (Sens.Items[i].Selected == true)
                        {


                            


                            SqlParameter[] parameters =
                                      {
                                    new SqlParameter("@DashboardName",txtDash.Text),
                                    new SqlParameter("@Sensor",Convert.ToDouble(str))
                                 };
                            MyDataAccess.ExecCmdQueryParams("[dbo].[InsertDashboardConfig]", parameters);
                        }


                    }
                    OtherDIV.Visible = false;
                    Dashboards.SelectedIndex = 0;

                    if (Type.SelectedIndex == 1)

                        Response.Redirect("DashboardKVA.aspx?DashboardName=" + txtDash.Text, true);
                    else if (Type.SelectedIndex == 2)
                    {
                        Response.Redirect("KVAGraphDisplay.aspx?DashboardName=" + txtDash.Text, true);
                    }
                    else
                    {
                        Message.Visible = true;
                        btnSave.Text = "Save";
                    }

                }
                }


               else if (btnSave.Text == "Show")
                {
                if (Type.SelectedIndex == 1)

                    Response.Redirect("DashboardKVA.aspx?DashboardName=" + Dashboards.SelectedValue, true);
                else if (Type.SelectedIndex == 2)
                {
                    Response.Redirect("KVAGraphDisplay.aspx?DashboardName=" + Dashboards.SelectedValue, true);
                }
                else
                {
                    Message.Visible = true;
                }
            }


            }

        protected void Type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    }
