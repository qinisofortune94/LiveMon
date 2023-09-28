using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using Microsoft.VisualBasic.ApplicationServices;

namespace website2016V2
{
    public class DataManagerr
    {
        #region"Helper methods"
        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["IPMonConnectionString"].ConnectionString; }
        }


        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            return connection;
        }

        private SqlCommand CreateCommand(string text)
        {
            SqlCommand command = null;
            command.CommandText = text;
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = CreateConnection();

            return command;
        }

        private SqlCommand CreateCommand(string text, SqlConnection connection)
        {
            SqlCommand command = null;
            command.CommandText = text;
            command.Connection = connection;

            return command;
        }



        private SqlDataAdapter CreateDataAdapter(string @select)
        {
            SqlDataAdapter da = null;
            da.SelectCommand = CreateCommand(@select);
            return da;
        }

        #endregion


        public DataRow GetPeopleDetails(int id)
        {
            string sqlQuery = "[People].[spGetPeopleDetails]";
            SqlConnection connection = new SqlConnection(ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(sqlQuery, connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("PeopleId", id);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                return dt.Rows[0];
            }
            return null;
        }

        public void LoadRootLayers(DropDownList rootLayers, object p)
        {
            string sqlQuery = "[dbo].[EquipmentLayout_select_allrootdetails]";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                rootLayers.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    rootLayers.Items.Add(new ListItem(reader["SensorName"].ToString(), reader["ID"].ToString()));
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
            //return null;
        }

        public string LoadEmployees(DropDownList DropEmployeeList, string pstrSite)
        {

            string sqlQuery = "[People].[spGetPeopleList]";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();
                DropEmployeeList.Items.Clear();
                //fill the dropdown with both the text and the primary key/value
                while (reader.Read())
                {
                    DropEmployeeList.Items.Add(new ListItem(reader["Employee"].ToString(), reader["ID"].ToString()));
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
            return null;
        }
        public string GetEncrypted(string password)
        {
            HashAlgorithm hash = new SHA256Managed();
            string salt = "livemon.co.za";

            // compute hash of the password prefixing password with the salt
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(salt + password);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            return Convert.ToBase64String(hashBytes);
        }

        internal List<User> GetPeopleDetails()
        {
            throw new NotImplementedException();
        }

        internal List<AddUsers> LoadEmployees()
        {
            throw new NotImplementedException();
        }

        internal void LoadEmployees(int peopleId)
        {
            throw new NotImplementedException();
        }
    }
}