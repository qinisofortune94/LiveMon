using LiveMonitoring;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class EquipmentLayoutMetering : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            DataManagerr datamanager = new DataManagerr();
            if (IsPostBack == false)
            {
                datamanager.LoadRootLayers(RootLayers, null);
                LiveMonitoring.testing test = new LiveMonitoring.testing();
                test.LoadfieldNames(TypeFilter);
               
            }
            //if (Session["RootID"] != null)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(),"script", "<script>callAjax_GetspecificEquipmentLayout("+ Session["RootID"]+");</script>");
            //}
        }

        protected void root_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load DropDownList2
            //Session["RootID"] = RootLayers.SelectedValue;
            //Page.ClientScript.RegisterStartupScript(this.GetType(),"script", "<script>callAjax_GetspecificEquipmentLayout();</script>");
            
        }
    }
}