using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class Reports : System.Web.UI.Page
    {
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        string conStrReport = WebConfigurationManager.ConnectionStrings["IPMonConnectionStringReport"].ToString();
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

                if (Page.IsPostBack == false)
                {
                    getReports();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        public void getReports()
        {
            string Url = "";
            //CONNECT TO THE LOCAL SERVER AND GET SERVER URL   
            string sqlQuerylocal = "reports_get_report";
            SqlConnection connectionlocal = new SqlConnection(conStr);
            SqlCommand cmdlocal = new SqlCommand(sqlQuerylocal, connectionlocal);
            cmdlocal.CommandType = CommandType.StoredProcedure;
            try
            {
                if (connectionlocal.State == ConnectionState.Closed)
                {
                    connectionlocal.Open();
                }
                SqlDataReader readerlocal = cmdlocal.ExecuteReader();

                while (readerlocal.Read())
                {
                    Url = readerlocal["Url"].ToString();
                }

                readerlocal.Close();
                if (connectionlocal.State == ConnectionState.Open)
                {
                    connectionlocal.Close();
                }

            }
            catch (Exception ex)
            {
            }

            string sqlQuery = "reports_get_report";
            SqlConnection connection = new SqlConnection(conStrReport);
            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                //fill sql reports
                while (reader.Read())
                {
                    ListItem mynewItem = new ListItem();
                    mynewItem.Value = (Url + "/Pages/ReportViewer.aspx?" + reader["Url"].ToString() + "&rs:Command=Render");
                    mynewItem.Text = (reader["ReportName"].ToString());
                    lstReprts.Items.Add(mynewItem);
                }

                reader.Close();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lstReprts_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(lstReprts.SelectedValue.ToString());
        }
    }
}