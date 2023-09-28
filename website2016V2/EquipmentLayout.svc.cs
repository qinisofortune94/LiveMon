using LiveMonitoring;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

/// <summary>
/// This is the web service for Equipment Layout.returns Json data to the Javascript Ajax Call
/// </summary>
namespace website2016V2
{
    /// <summary>
    /// Equipment Layout Class
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EquipmentLayout
    {

        /// <summary>
        /// My data access is shared data access routines
        /// </summary>
        private static DataAccess MyDataAccess = new DataAccess();
        //FindEquipmentLayoutTreeByID
        /// <summary>
        /// Finds the equipment layout tree by identifier.
        /// </summary>
        /// <param name="sensorid">The sensorid.</param>
        /// <returns></returns>
        /// //http://www.cleancode.co.nz/blog/1041/complete-example-wcf-ajax-ssl-http
        [OperationContract]
        [WebInvoke(Method = "POST",
     BodyStyle = WebMessageBodyStyle.WrappedRequest,RequestFormat = WebMessageFormat.Json,
     ResponseFormat = WebMessageFormat.Json)]
        List<AssetHierarchy> FindEquipmentLayoutTreeByID(int sensorid)
        {
            List<AssetHierarchy> returnRoot;
            AssetHierarchy myassettree = new AssetHierarchy();
            // TreeItem[] retItem = new TreeItem[1];
            returnRoot = myassettree.LoadRoot(Convert.ToInt32(sensorid));
            return returnRoot;
        }
        /// <summary>
        /// Finds the equipment layout tree.
        /// </summary>
        /// <returns>List of Asset Heirachy objects in Json</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
     BodyStyle = WebMessageBodyStyle.WrappedRequest,
     ResponseFormat = WebMessageFormat.Json)]
        List<AssetHierarchy> FindEquipmentLayoutTree()
        {
            List<AssetHierarchy> returnRoot;
            AssetHierarchy myassettree = new AssetHierarchy();
            // TreeItem[] retItem = new TreeItem[1];
            returnRoot = myassettree.LoadRoot();
            return returnRoot;
        }
        /// <summary>
        /// Base class of Aset Heirachy data to be passed back to JavaScript
        /// </summary>
        public class AssetHierarchy
        {
            private static DataAccess MyDataAccess = new DataAccess();
            public int SensorID { get; set; }

            public int ParentID { get; set; }

            public bool? HasChildren { get; set; }

            public String SensorName { get; set; }

            public List<AssetHierarchy> Children;

            /// <summary>
            /// Loads the root data of the heirachy
            /// </summary>
            /// <returns>List of AssetHeirachy with a List of children embedded</returns>
            public List<AssetHierarchy> LoadRoot()
            {
                //load parent or root details
                //[mobile].[GetRootAssetHierachy]
                List<AssetHierarchy> returnRoots = new List<AssetHierarchy>();
                SqlDataReader myreader = MyDataAccess.ExecCmdQueryNoParams("[dbo].[EquipmentLayout_select_allrootdetails]");
                try
                {
                    while (myreader.Read())
                    {
                        if (!myreader.IsDBNull(myreader.GetOrdinal("SensorID"))) this.SensorID = myreader.GetInt32(myreader.GetOrdinal("SensorID"));
                        if (!myreader.IsDBNull(myreader.GetOrdinal("ParentID"))) this.ParentID = myreader.GetInt32(myreader.GetOrdinal("ParentID"));
                        if (!myreader.IsDBNull(myreader.GetOrdinal("HasChildren"))) this.HasChildren = myreader.GetBoolean(myreader.GetOrdinal("HasChildren"));
                        if (!myreader.IsDBNull(myreader.GetOrdinal("SensorName"))) this.SensorName = myreader.GetString(myreader.GetOrdinal("SensorName"));
                        if (this.HasChildren == true)
                        {
                            LoadChildren(this.SensorID);
                        }
                        returnRoots.Add(this);
                    }
                    myreader.Close();
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }

                return returnRoots;
            }
            /// <summary>
            /// Loads the children.
            /// </summary>
            /// <param name="ParentID">The parent identifier.</param>
            /// <returns>The list of children for the parent Heirachy asset</returns>
            public bool LoadChildren(int ParentID)
            {
                //load children 
                Children = new List<AssetHierarchy>();
                //[mobile].[GetChildAssetHierachy]  @ParentPK
                try
                {
                    SqlParameter[] parameters =
                     {
                        new SqlParameter("@ParentID", ParentID)
                    };
                    //var parameters = new SqlParameter[0];
                    // parameters[0] = new SqlParameter("@UserGUID", UserGUID);
                    SqlDataReader myreader = MyDataAccess.ExecCmdQueryParams("[dbo].[EquipmentLayout_select_allChilddetails]", parameters);
                    try
                    {
                        while (myreader.Read())
                        {
                            AssetHierarchy child = new AssetHierarchy();
                            if (!myreader.IsDBNull(myreader.GetOrdinal("SensorID"))) child.SensorID = myreader.GetInt32(myreader.GetOrdinal("SensorID"));
                            if (!myreader.IsDBNull(myreader.GetOrdinal("ParentID"))) child.ParentID = myreader.GetInt32(myreader.GetOrdinal("ParentID"));
                            if (!myreader.IsDBNull(myreader.GetOrdinal("HasChildren"))) child.HasChildren = myreader.GetBoolean(myreader.GetOrdinal("HasChildren"));
                            if (!myreader.IsDBNull(myreader.GetOrdinal("SensorName"))) child.SensorName = myreader.GetString(myreader.GetOrdinal("SensorName"));
                            Children.Add(child);
                            if (this.HasChildren == true)
                            {
                                child.LoadChildren(child.SensorID);
                            }
                            //find children where parentpk==AssetPK and load
                        }
                        myreader.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.Message);
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    Debug.Write(ex.Message);
                    return false;
                }
            }

            public List<AssetHierarchy> LoadRoot(int id)
            {
                List<AssetHierarchy> returnRoots = new List<AssetHierarchy>();
                SqlParameter[] parameters =
                    {
                        new SqlParameter("@RootID", id)
                    };
                
                SqlDataReader myreader = MyDataAccess.ExecCmdQueryParams("[dbo].[EquipmentLayout_select_rootdetailsbyid]", parameters);

                try
                {
                    while (myreader.Read())
                    {
                        if (!myreader.IsDBNull(myreader.GetOrdinal("SensorID"))) this.SensorID = myreader.GetInt32(myreader.GetOrdinal("SensorID"));
                        if (!myreader.IsDBNull(myreader.GetOrdinal("ParentID"))) this.ParentID = myreader.GetInt32(myreader.GetOrdinal("ParentID"));
                        if (!myreader.IsDBNull(myreader.GetOrdinal("HasChildren"))) this.HasChildren = myreader.GetBoolean(myreader.GetOrdinal("HasChildren"));
                        if (!myreader.IsDBNull(myreader.GetOrdinal("SensorName"))) this.SensorName = myreader.GetString(myreader.GetOrdinal("SensorName"));
                        if (this.HasChildren == true)
                        {
                            LoadChildren(this.SensorID);
                        }
                        returnRoots.Add(this);
                    }
                    myreader.Close();
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }

                return returnRoots;
            }
        }
    }
}
