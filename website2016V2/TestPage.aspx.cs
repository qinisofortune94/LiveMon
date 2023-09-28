using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Dynamic;


namespace website2016V2
{
    public partial class TestPage : System.Web.UI.Page
    {
        private int selectedTarrifID;

        private LiveMonitoring.IRemoteLib.Operations operation;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // The Page is accessed for the first time.

            if (!IsPostBack)
            {
                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                List<LiveMonitoring.IRemoteLib.MeteringTariff> tarrifItems = remoteProc.LiveMonServer.GetMeteringTarrifNames();

                List<object> tarrifObjectItems = new List<object>();
                foreach (LiveMonitoring.IRemoteLib.MeteringTariff tarrif in tarrifItems)
                {

                    tarrifObjectItems.Add(new
                    {
                        ID = tarrif.ID,
                        TarriffName = tarrif.TarriffName
                    });

                }
                // Initialize the DataTable and store it in ViewState.

                LiveMonitoring.IRemoteLib.MeteringTariff tarrifObject = new LiveMonitoring.IRemoteLib.MeteringTariff();
                InitializeDataSource(tarrifObject.GetType(), tarrifObjectItems, "MeteringTariff");

                // Enable the GridView paging option and specify the page size.
                gvTarrif.AllowPaging = true;
                gvTarrif.PageSize = 5;

                gvActiveEnergy.AllowPaging = true;
                gvActiveEnergy.PageSize = 5;

                // Enable the GridView sorting option.
                gvTarrif.AllowSorting = true;
                gvActiveEnergy.AllowSorting = true;

                // Initialize the sorting expression.
                ViewState("SortExpression") = "ID ASC";
                //ViewState("SortExpression") = "ID DESC"

                // Populate the GridView.
                BindGridView("dtMeteringTariff", gvTarrif);

                TabActiveEnergy.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }

        }

        // Initialize the DataTable.

        private void InitializeDataSource(Type type, List<object> dataList, string tableName)
        {
            // Create a DataTable object named dtMeteringTariff.
            DataTable dataTable = new DataTable();

            setDataTable(ref dataTable, type, dataList, tableName);

        }

        private void setDataTable(ref DataTable dataTable, Type type, List<object> dataList, string tableName)
        {
            FieldInfo[] fieldList = type.GetFields();

            foreach (FieldInfo field in fieldList)
            {
                dataTable.Columns.Add(field.Name);

                if (field.Name == "ID")
                {
                    // Set ID column as the primary key.

                    DataColumn[] dcKeys = new DataColumn[1];
                    dcKeys(0) = dataTable.Columns(field.Name);
                    dataTable.PrimaryKey = dcKeys;

                }
            }


            foreach (object item in dataList)
            {
                switch (tableName)
                {
                    case "MeteringTariff":
                        dataTable.Rows.Add(item.ID, item.TarriffName);

                        break;
                    case "MeteringActiveEnergyCharges":
                        dataTable.Rows.Add(item.ID, item.TariffID, item.ChargeName, item.CostcPerKWh);
                        break;
                    case "MeteringNetworkCharges":
                        dataTable.Rows.Add(item.ID, item.TariffID, item.ChargeName, item.CostRperkWh, item.CostRperday, item.CostRperkVA, item.CostRperMaxkVA, item.FixedCost, item.MaximumDemand, item.MaximumDemand,
                        item.PenaltyCharge, item.Percentage);
                        break;
                    case "MeteringVoltageSurcharges":
                        dataTable.Rows.Add(item.ID, item.TariffID, item.Voltage, item.SurchargePercentage);

                        break;
                    default:
                        break; // TODO: might not be correct. Was : Exit Select

                        break;
                }

            }

            ViewState("dt" + tableName) = dataTable;
        }
        private void BindGridView(string vState, GridView gvData)
        {
            if (ViewState(vState) != null)
            {
                // Get the DataTable from ViewState.
                DataTable dataTable = (DataTable)ViewState(vState);

                // Convert the DataTable to DataView.
                DataView dataView = new DataView(dataTable);

                // Set the sort column and sort order.
                dataView.Sort = ViewState("SortExpression").ToString();

                gvData.DataSource = dataView;
                gvData.DataBind();

            }
        }

        // GridView.RowDataBound Event
        protected void gvTarrif_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Make sure the current GridViewRow is a data row.
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Make sure the current GridViewRow is either 
                // in the normal state or an alternate row.
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    // Add client-side confirmation when deleting.
                    ((LinkButton)e.Row.Cells(1).Controls(0)).Attributes("onclick") = "if(!confirm('Are you certain you want to delete this item ?')) return false;";
                }
            }
        }

        /// <summary>
        /// GridView.PageIndexChanging Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected void gvTarrif_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page.  
            gvTarrif.PageIndex = e.NewPageIndex;

            // Rebind the GridView control to 
            // show data in the new page.
            BindGridView("dtMeteringTariff", gvTarrif);
        }
        protected void gvActiveEnergy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page.  
            gvActiveEnergy.PageIndex = e.NewPageIndex;

            // Rebind the GridView control to 
            // show data in the new page.
            BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy);
        }

        // GridView.RowEditing Event
        protected void gvTarrif_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Make the GridView control into edit mode 
            // for the selected row. 
            gvTarrif.EditIndex = e.NewEditIndex;

            // Rebind the GridView control to show data in edit mode.
            BindGridView("dtMeteringTariff", gvTarrif);

            // Hide the Add button.
            lbtnEdit.Visible = false;
        }

        // GridView.RowCancelingEdit Event
        protected void gvTarrif_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Exit edit mode.
            gvTarrif.EditIndex = -1;

            // Rebind the GridView control to show data in view mode.
            BindGridView("dtMeteringTariff", gvTarrif);

            // Show the Add button.
            lbtnEdit.Visible = true;
        }
        //===================================================
        //======================UPDATE=======================
        //===================================================
        // GridView.RowUpdating Event
        protected void gvTarrif_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ViewState("dtMeteringTariff") != null)
            {
                // Get the DataTable from ViewState.
                DataTable dtMeteringTariff = (DataTable)ViewState("dtMeteringTariff");

                // Get the TarrifID of the selected row.
                string strTarrifID = gvTarrif.Rows(e.RowIndex).Cells(2).Text;

                // Find the row in DateTable.
                DataRow drTarrif = dtMeteringTariff.Rows.Find(strTarrifID);

                // Retrieve edited values and updating respective items.
                drTarrif("LastName") = ((TextBox)gvTarrif.Rows(e.RowIndex).FindControl("TextBox1")).Text;
                drTarrif("FirstName") = ((TextBox)gvTarrif.Rows(e.RowIndex).FindControl("TextBox2")).Text;

                // Exit edit mode.
                gvTarrif.EditIndex = -1;

                // Rebind the GridView control to show data after updating.
                BindGridView("dtMeteringTariff", gvTarrif);

                // Show the Add button.
                lbtnEdit.Visible = true;
            }
        }
        // GridView.RowUpdating Event
        protected void gvActiveEnergy_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ViewState("dtMeteringActiveEnergyCharges") != null)
            {
                // Get the DataTable from ViewState.
                DataTable dtMeteringTariff = (DataTable)ViewState("dtMeteringActiveEnergyCharges");

                // Get the TarrifID of the selected row.
                string strTarrifID = gvTarrif.Rows(e.RowIndex).Cells(2).Text;

                // Find the row in DateTable.
                DataRow drTarrif = dtMeteringTariff.Rows.Find(strTarrifID);

                // Retrieve edited values and updating respective items.
                drTarrif("LastName") = ((TextBox)gvTarrif.Rows(e.RowIndex).FindControl("TextBox1")).Text;
                drTarrif("FirstName") = ((TextBox)gvTarrif.Rows(e.RowIndex).FindControl("TextBox2")).Text;

                // Exit edit mode.
                gvTarrif.EditIndex = -1;

                // Rebind the GridView control to show data after updating.
                BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy);

                // Show the Add button.
                lbtnEdit.Visible = true;
            }
        }

        // GridView.SelectedIndexChanging Event

        protected void gvTarrif_SelectedIndexChanged(object sender, EventArgs e)
        {
            pDetails.Visible = true;

            GridViewRow row = gvTarrif.SelectedRow;

            selectedTarrifID = Convert.ToInt32(row.Cells(2).Text);

            Session["selectedTarrifID"] = selectedTarrifID;


            string selectedTarrifName = row.Cells(3).Text;

            //txtTarriffName.Text = selectedTarrifName

            LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();

            // =======LOADING ACTIVE ENERGY CHARGE =========

            dynamic activeEnergyChargeItems = from a in remoteProc.LiveMonServer.GetAllMeteringActiveEnergyChargeswhere a.TariffID == selectedTarrifIDa;

            LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges tempObject = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();

            loadItems(activeEnergyChargeItems, gvActiveEnergy, tempObject.GetType());


            lbtnEdit.Visible = true;

            // =======LOADING NETWORK CHARGES =========

            dynamic netWorkChargeItems = from n in remoteProc.LiveMonServer.GetAllMeteringNetworkChargeswhere n.TariffID == selectedTarrifIDn;

            LiveMonitoring.IRemoteLib.MeteringNetworkCharges networkObject = new LiveMonitoring.IRemoteLib.MeteringNetworkCharges();
            loadItems(netWorkChargeItems, gvNetworkCharges, networkObject.GetType());


            // =======LOADING VOLTAGE SURCHARGES =========

            dynamic voltageSurchargeItems = from v in remoteProc.LiveMonServer.GetAllMeteringVoltageSurchargeswhere v.TariffID == selectedTarrifIDv;

            LiveMonitoring.IRemoteLib.MeteringVoltageSurcharges voltageObject = new LiveMonitoring.IRemoteLib.MeteringVoltageSurcharges();
            loadItems(voltageSurchargeItems, gvVoltage, voltageObject.GetType());


        }


        protected void loadItems(IEnumerable<object> itemList, GridView gridView, Type type)
        {
            List<object> resultList = new List<object>();

            foreach (void item_loopVariable in itemList)
            {
                item = item_loopVariable;
                switch (type.Name)
                {
                    case "MeteringActiveEnergyCharges":
                        resultList.Add(new
                        {
                            ID = item.ID,
                            TariffID = item.TariffID,
                            ChargeName = item.ChargeName,
                            CostcPerKWh = item.CostcPerKWh
                        });
                        break;
                    case "MeteringNetworkCharges":
                        resultList.Add(new
                        {
                            ID = item.ID,
                            TariffID = item.TariffID,
                            ChargeName = item.ChargeName,
                            CostRperkWh = item.CostRperkWh,
                            CostRperday = item.CostRperday,
                            CostRperkVA = item.CostRperkVA,
                            CostRperMaxkVA = item.CostRperMaxkVA,
                            FixedCost = item.FixedCost,
                            MaximumDemand = item.MaximumDemand,
                            PenaltyCharge = item.PenaltyCharge,
                            Percentage = item.Percentage
                        });
                        break;
                    case "MeteringVoltageSurcharges":
                        resultList.Add(new
                        {
                            ID = item.ID,
                            TariffID = item.TariffID,
                            Voltage = item.Voltage,
                            SurchargePercentage = item.SurchargePercentage
                        });
                        break;
                }

            }

            InitializeDataSource(type, resultList, type.Name);

            BindGridView("dt" + type.Name, gridView);
        }
        protected void gvActiveEnergy_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow rowf = gvActiveEnergy.SelectedRow;

            int selectedActiveEnergyID = Convert.ToInt32(rowf.Cells(2).Text);
            Session["selectedActiveEnergyID"] = selectedActiveEnergyID;
            string chargeName = rowf.Cells(3).Text;
            string costcPerKWh = rowf.Cells(4).Text;

            lbtnActiveEnergyEdit.Visible = true;

            txtActiveEnergyChargeName.Text = chargeName;
            txtActiveEnergyCostcPerKWh.Text = costcPerKWh;

        }

        protected void gvNetworkCharges_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow rowf = gvNetworkCharges.SelectedRow;

            int selectedgvNetworkChargeID = Convert.ToInt32(rowf.Cells(2).Text);
            Session["selectedgvNetworkChargeID"] = selectedgvNetworkChargeID;

            lbtnNetworkChargesEdit.Visible = true;

            txtNetworkChargeName.Text = rowf.Cells(2).Text;
            txtNetworkCostRperkWh.Text = rowf.Cells(3).Text;
            txtNetworkCostRperday.Text = rowf.Cells(4).Text;
            txtNetworkCostRperkVA.Text = rowf.Cells(5).Text;
            txtNetworkCostRperMaxkVA.Text = rowf.Cells(6).Text;
            txtNetworkFixedCost.Text = rowf.Cells(7).Text;
            txtNetworkMaximumDemand.Text = rowf.Cells(8).Text;
            txtNetworkPenaltyCharge.Text = rowf.Cells(9).Text;
            txtNetworkPercentage.Text = rowf.Cells(10).Text;

        }

        protected void gvVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow rowf = gvVoltage.SelectedRow;

            int selectedVoltageID = Convert.ToInt32(rowf.Cells(2).Text);
            Session["selectedVoltageID"] = selectedVoltageID;

            lbtnVoltageEdit.Visible = true;

            txtVoltageVoltage.Text = rowf.Cells(3).Text;
            txtVoltageSurchargePercentage.Text = rowf.Cells(4).Text;

        }

        // GridView.RowDeleting Event

        protected void gvTarrif_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState("dtMeteringTariff") != null)
            {
                // Get the TarrifID of the selected row.
                string strTarrifID = gvTarrif.Rows(e.RowIndex).Cells(2).Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isTarrifDeleted = remoteProc.LiveMonServer.DeleteMeteringTarrif(Convert.ToInt32(strTarrifID));

                if (isTarrifDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtMeteringTariff = (DataTable)ViewState("dtMeteringTariff");



                    // Find the row in DateTable.
                    DataRow drTarrif = dtMeteringTariff.Rows.Find(strTarrifID);

                    // Remove the row from the DataTable.
                    dtMeteringTariff.Rows.Remove(drTarrif);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtMeteringTariff", gvTarrif);

                }


            }
        }
        // GridView.RowDeleting Event
        protected void gvActiveEnergy_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState("dtMeteringActiveEnergyCharges") != null)
            {
                // Get the TarrifID of the selected row.
                string strActiveEnergyID = gvActiveEnergy.Rows(e.RowIndex).Cells(2).Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isActiveEnergyDeleted = remoteProc.LiveMonServer.DeleteMeteringActiveEnergyCharges(Convert.ToInt32(strActiveEnergyID));

                if (isActiveEnergyDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtMeteringActiveEnergyCharges = (DataTable)ViewState("dtMeteringActiveEnergyCharges");


                    // Find the row in DateTable.
                    DataRow drActiveEnergy = dtMeteringActiveEnergyCharges.Rows.Find(strActiveEnergyID);

                    // Remove the row from the DataTable.
                    dtMeteringActiveEnergyCharges.Rows.Remove(drActiveEnergy);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy);
                }

            }
        }

        protected void gvNetworkCharges_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState("dtMeteringNetworkCharges") != null)
            {
                // Get the TarrifID of the selected row.
                string strNetworkChargeID = gvNetworkCharges.Rows(e.RowIndex).Cells(2).Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isNetworkChargeDeleted = remoteProc.LiveMonServer.DeleteMeteringNetworkCharges(Convert.ToInt32(strNetworkChargeID));

                if (isNetworkChargeDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtMeteringNetworkCharges = (DataTable)ViewState("dtMeteringNetworkCharges");

                    // Find the row in DateTable.
                    DataRow drNetworkCharge = dtMeteringNetworkCharges.Rows.Find(strNetworkChargeID);

                    // Remove the row from the DataTable.
                    dtMeteringNetworkCharges.Rows.Remove(drNetworkCharge);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtMeteringNetworkCharges", gvNetworkCharges);
                }

            }
        }

        protected void gvVoltage_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState("dtMeteringVoltageSurcharges") != null)
            {
                // Get the TarrifID of the selected row.
                string strVoltageID = gvVoltage.Rows(e.RowIndex).Cells(2).Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isVoltageDeleted = remoteProc.LiveMonServer.DeleteMeteringVoltageSurcharges(Convert.ToInt32(strVoltageID));

                if (isVoltageDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtMeteringVoltageSurcharges = (DataTable)ViewState("dtMeteringVoltageSurcharges");

                    // Find the row in DateTable.
                    DataRow drVoltage = dtMeteringVoltageSurcharges.Rows.Find(strVoltageID);

                    // Remove the row from the DataTable.
                    dtMeteringVoltageSurcharges.Rows.Remove(drVoltage);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtMeteringVoltageSurcharges", gvVoltage);
                }

            }
        }

        // GridView.Sorting Event
        protected void gvTarrif_Sorting(object sender, GridViewSortEventArgs e)
        {
            string[] strSortExpression = ViewState("SortExpression").ToString().Split(' ');

            // If the sorting column is the same as the previous one, 
            // then change the sort order.
            if (strSortExpression(0) == e.SortExpression)
            {
                if (strSortExpression(1) == "ASC")
                {
                    ViewState("SortExpression") = Convert.ToString(e.SortExpression) + " " + "DESC";
                }
                else
                {
                    ViewState("SortExpression") = Convert.ToString(e.SortExpression) + " " + "ASC";
                }
            }
            else
            {
                // If sorting column is another column, 
                // then specify the sort order to "Ascending".
                ViewState("SortExpression") = Convert.ToString(e.SortExpression) + " " + "ASC";
            }

            // Rebind the GridView control to show sorted data.
            BindGridView("dtMeteringTariff", gvTarrif);
        }
        // GridView.Sorting Event
        protected void gvActiveEnergy_Sorting(object sender, GridViewSortEventArgs e)
        {
            string[] strSortExpression = ViewState("SortExpression").ToString().Split(' ');

            // If the sorting column is the same as the previous one, 
            // then change the sort order.
            if (strSortExpression(0) == e.SortExpression)
            {
                if (strSortExpression(1) == "ASC")
                {
                    ViewState("SortExpression") = Convert.ToString(e.SortExpression) + " " + "DESC";
                }
                else
                {
                    ViewState("SortExpression") = Convert.ToString(e.SortExpression) + " " + "ASC";
                }
            }
            else
            {
                // If sorting column is another column, 
                // then specify the sort order to "Ascending".
                ViewState("SortExpression") = Convert.ToString(e.SortExpression) + " " + "ASC";
            }

            // Rebind the GridView control to show sorted data.
            BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy);
        }


        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            GridViewRow row = gvTarrif.SelectedRow;

            selectedTarrifID = Convert.ToInt32(row.Cells(2).Text);

            Session["selectedTarrifID"] = selectedTarrifID;


            // Dim selectedTarrifName As String = row.Cells(3).Text

            txtTarriffName.Text = row.Cells(3).Text;

            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            lbtnEdit.Visible = false;
            pnlEdit.Visible = true;
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.
            lbtnEdit.Visible = false;
            pnlEdit.Visible = true;
        }


        protected void lbtnVoltageEdit_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.

            lbtnVoltageEdit.Visible = false;
            pnlVoltageEdit.Visible = true;
        }

        protected void lbtnVoltageAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.

            lbtnVoltageEdit.Visible = false;
            pnlVoltageEdit.Visible = true;
        }


        protected void lbtnNetworkChargesEdit_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            lbtnNetworkChargesEdit.Visible = false;
            pnlNetworkChargesEdit.Visible = true;
        }

        protected void lbtnNetworkChargesAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.
            lbtnNetworkChargesEdit.Visible = false;
            pnlNetworkChargesEdit.Visible = true;
        }


        protected void lbtnActiveEnergyEdit_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            lbtnActiveEnergyEdit.Visible = false;
            pnlActiveEnergyEdit.Visible = true;
        }

        protected void lbtnActiveEnergyAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.
            lbtnActiveEnergyEdit.Visible = false;
            pnlActiveEnergyEdit.Visible = true;
        }


        protected void lbtnNetworkChargesSubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState("dtMeteringNetworkCharges") != null)
            {
                LiveMonitoring.IRemoteLib.MeteringNetworkCharges networkCharge = new LiveMonitoring.IRemoteLib.MeteringNetworkCharges();

                networkCharge.TariffID = Session["selectedTarrifID"];
                networkCharge.ChargeName = txtNetworkChargeName.Text == string.Empty ? null : txtNetworkChargeName.Text;
                networkCharge.CostRperkWh = txtNetworkCostRperkWh.Text == string.Empty ? null : txtNetworkCostRperkWh.Text;
                //networkCharge.CostRperkWh = CType(If(txtNetworkCostRperkWh.Text = String.Empty, Nothing, txtNetworkCostRperkWh.Text), Double)

                networkCharge.CostRperday = txtNetworkCostRperday.Text == string.Empty ? null : txtNetworkCostRperday.Text;

                networkCharge.CostRperkVA = txtNetworkCostRperkVA.Text == string.Empty ? null : txtNetworkCostRperkVA.Text;

                networkCharge.CostRperMaxkVA = txtNetworkCostRperMaxkVA.Text == string.Empty ? null : txtNetworkCostRperMaxkVA.Text;
                networkCharge.FixedCost = txtNetworkFixedCost.Text == string.Empty ? null : txtNetworkFixedCost.Text;

                networkCharge.MaximumDemand = txtNetworkMaximumDemand.Text == string.Empty ? null : txtNetworkMaximumDemand.Text;
                networkCharge.PenaltyCharge = txtNetworkPenaltyCharge.Text == string.Empty ? null : txtNetworkPenaltyCharge.Text;
                networkCharge.Percentage = txtNetworkPercentage.Text == string.Empty ? null : txtNetworkPercentage.Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isNetworkChargeSaved = false;


                if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    networkCharge.ID = Session["selectedgvNetworkChargeID"];

                    isNetworkChargeSaved = remoteProc.LiveMonServer.EditMeteringNetworkCharges(networkCharge);

                    if (isNetworkChargeSaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtNetworkCharge = (DataTable)ViewState("dtMeteringNetworkCharges");

                        // Find the row in DateTable.
                        DataRow drNetworkCharge = dtNetworkCharge.Rows.Find(Session["selectedgvNetworkChargeID"]);

                        // Retrieve edited values and updating respective items.
                        drNetworkCharge("TariffID") = Session["selectedTarrifID"];
                        drNetworkCharge("ChargeName") = txtNetworkChargeName.Text;
                        drNetworkCharge("CostRperday") = txtNetworkCostRperday.Text;
                        drNetworkCharge("CostRperkVA") = txtNetworkCostRperkVA.Text;
                        drNetworkCharge("CostRperkWh") = txtNetworkCostRperkWh.Text;
                        drNetworkCharge("CostRperMaxkVA") = txtNetworkCostRperMaxkVA.Text;
                        drNetworkCharge("MaximumDemand") = txtNetworkMaximumDemand.Text;
                        drNetworkCharge("PenaltyCharge") = txtNetworkPenaltyCharge.Text;
                        drNetworkCharge("Percentage") = txtNetworkPercentage.Text;

                        // Exit edit mode.
                        gvNetworkCharges.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtMeteringNetworkCharges", gvNetworkCharges);

                    }

                }
                else if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    int networkChargeID = 0;
                    try
                    {
                        networkChargeID = remoteProc.LiveMonServer.AddMeteringNetworkCharges(networkCharge);


                    }
                    catch (Exception ex)
                    {
                    }


                    if (networkChargeID != -99 & networkChargeID != 0)
                    {
                        DataTable dt = (DataTable)ViewState("dtMeteringNetworkCharges");
                        DataRow dr = dt.NewRow();

                        dr("ID") = networkChargeID;
                        dr("ChargeName") = txtNetworkChargeName.Text;
                        dr("CostRperkWh") = txtNetworkCostRperkWh.Text;
                        dr("CostRperday") = txtNetworkCostRperday.Text;

                        dr("CostRperkVA") = txtNetworkCostRperkVA.Text;
                        dr("CostRperMaxkVA") = txtNetworkCostRperMaxkVA.Text;
                        dr("FixedCost") = txtNetworkFixedCost.Text;
                        dr("MaximumDemand") = txtNetworkMaximumDemand.Text;

                        dr("PenaltyCharge") = txtNetworkPenaltyCharge.Text;
                        dr("Percentage") = txtNetworkPercentage.Text;

                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtMeteringNetworkCharges", gvNetworkCharges);

                    }

                }
            }

            // Empty the TextBox controls.
            txtTarriffName.Text = "";

            // Show the Add button and hiding the Add panel.
            lbtnEdit.Visible = true;
            pnlEdit.Visible = false;
        }


        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState("dtMeteringTariff") != null)
            {
                LiveMonitoring.IRemoteLib.MeteringTariff meteringTarrif = new LiveMonitoring.IRemoteLib.MeteringTariff();

                meteringTarrif.TarriffName = txtTarriffName.Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isTarrifSaved = false;


                if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    meteringTarrif.ID = Session["selectedTarrifID"];
                    isTarrifSaved = remoteProc.LiveMonServer.EditMeteringTarrif(meteringTarrif);
                    if (isTarrifSaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtMeteringTariff = (DataTable)ViewState("dtMeteringTariff");

                        // Find the row in DateTable.
                        DataRow drTarrif = dtMeteringTariff.Rows.Find(Session["selectedTarrifID"]);

                        // Retrieve edited values and updating respective items.
                        drTarrif("TarriffName") = txtTarriffName.Text;

                        // Exit edit mode.
                        gvTarrif.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtMeteringTariff", gvTarrif);

                    }

                }
                else if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    int tarrifID = 0;

                    try
                    {
                        tarrifID = remoteProc.LiveMonServer.AddNewMeteringTarrif(meteringTarrif);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (tarrifID != -99 | tarrifID != 0)
                    {
                        DataTable dt = (DataTable)ViewState("dtMeteringTariff");
                        DataRow dr = dt.NewRow();

                        dr("ID") = tarrifID;
                        dr("TarriffName") = txtTarriffName.Text;

                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtMeteringTariff", gvTarrif);

                    }

                }


            }

            // Empty the TextBox controls.
            txtTarriffName.Text = "";

            // Show the Add button and hiding the Add panel.
            lbtnEdit.Visible = true;
            pnlEdit.Visible = false;
        }


        protected void lbtnVoltageSubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState("dtMeteringVoltageSurcharges") != null)
            {
                LiveMonitoring.IRemoteLib.MeteringVoltageSurcharges voltageSurcharge = new LiveMonitoring.IRemoteLib.MeteringVoltageSurcharges();

                voltageSurcharge.TariffID = Session["selectedTarrifID"];
                voltageSurcharge.Voltage = txtVoltageVoltage.Text;
                voltageSurcharge.SurchargePercentage = txtVoltageSurchargePercentage.Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isVoltageSaved = false;


                if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    voltageSurcharge.ID = Session["selectedVoltageID"];

                    isVoltageSaved = remoteProc.LiveMonServer.EditMeteringVoltageSurcharges(voltageSurcharge);

                    if (isVoltageSaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtMeteringVoltageSurcharges = (DataTable)ViewState("dtMeteringVoltageSurcharges");

                        // Find the row in DateTable.
                        DataRow drVoltage = dtMeteringVoltageSurcharges.Rows.Find(Session["selectedVoltageID"]);

                        // Retrieve edited values and updating respective items.
                        drVoltage("Voltage") = txtVoltageVoltage.Text;
                        drVoltage("SurchargePercentage") = txtVoltageSurchargePercentage.Text;

                        // Exit edit mode.
                        gvVoltage.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtMeteringVoltageSurcharges", gvVoltage);

                    }

                }
                else if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    int voltageID = 0;

                    try
                    {
                        voltageID = remoteProc.LiveMonServer.AddMeteringVoltageSurcharges(voltageSurcharge);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (voltageID != -99 & voltageID != 0)
                    {
                        DataTable dt = (DataTable)ViewState("dtMeteringVoltageSurcharges");
                        DataRow dr = dt.NewRow();

                        dr("ID") = voltageID;
                        dr("TariffID") = Session["selectedTarrifID"];
                        dr("Voltage") = txtVoltageVoltage.Text;
                        dr("SurchargePercentage") = txtVoltageSurchargePercentage.Text;


                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtMeteringVoltageSurcharges", gvVoltage);

                    }

                }

            }

            // Empty the TextBox controls.
            txtVoltageVoltage.Text = "";
            txtVoltageSurchargePercentage.Text = "";

            // Show the Add button and hiding the Add panel.
            lbtnVoltageEdit.Visible = true;
            pnlVoltageEdit.Visible = false;
        }


        protected void lbtnActiveEnergySubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState("dtMeteringActiveEnergyCharges") != null)
            {
                LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges activeEnergyCharge = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                activeEnergyCharge.ID = Session["selectedActiveEnergyID"];
                activeEnergyCharge.ChargeName = txtActiveEnergyChargeName.Text;
                activeEnergyCharge.CostcPerKWh = txtActiveEnergyCostcPerKWh.Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();


                if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    bool isActiveEnergySaved = remoteProc.LiveMonServer.EditMeteringActiveEnergyCharges(activeEnergyCharge);

                    if (isActiveEnergySaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtMeteringActiveEnergyCharges = (DataTable)ViewState("dtMeteringActiveEnergyCharges");

                        // Find the row in DateTable.
                        DataRow drActiveEnergy = dtMeteringActiveEnergyCharges.Rows.Find(Session["selectedActiveEnergyID"]);

                        // Retrieve edited values and updating respective items.
                        drActiveEnergy("ChargeName") = txtActiveEnergyChargeName.Text;
                        drActiveEnergy("CostcPerKWh") = txtActiveEnergyCostcPerKWh.Text;

                        // Exit edit mode.
                        gvActiveEnergy.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy);

                    }


                }
                else if (Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    int activeChargeID = 0;
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges activeCharge = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    activeCharge.TariffID = Convert.ToInt32(Session["selectedTarrifID"]);
                    activeCharge.ChargeName = txtActiveEnergyChargeName.Text;
                    activeCharge.CostcPerKWh = Convert.ToDouble(txtActiveEnergyCostcPerKWh.Text);

                    try
                    {
                        activeChargeID = remoteProc.LiveMonServer.AddMeteringActiveEnergyCharges(activeCharge);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (activeChargeID != -99 | activeChargeID != 0)
                    {
                        DataTable dt = (DataTable)ViewState("dtMeteringActiveEnergyCharges");
                        DataRow dr = dt.NewRow();

                        dr("ID") = activeChargeID;
                        dr("ChargeName") = txtActiveEnergyChargeName.Text;
                        dr("CostcPerKWh") = Convert.ToDouble(txtActiveEnergyCostcPerKWh.Text);

                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy);

                    }
                }


            }

            // Empty the TextBox controls.
            txtActiveEnergyChargeName.Text = "";
            txtActiveEnergyCostcPerKWh.Text = "";

            // Show the Add button and hiding the Add panel.
            lbtnActiveEnergyEdit.Visible = true;
            pnlActiveEnergyEdit.Visible = false;
        }
        protected void lbtnNetworkChargesCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.
            txtNetworkChargeName.Text = "";

            if (gvNetworkCharges.SelectedIndex != -1)
            {
                gvNetworkCharges.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlNetworkChargesEdit.Visible = false;

            lbtnNetworkChargesEdit.Visible = false;
            lbtnNetworkChargesEdit.Visible = false;
        }

        protected void lbtnVoltagesCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.

            txtVoltageVoltage.Text = "";
            txtVoltageSurchargePercentage.Text = "";

            if (gvVoltage.SelectedIndex != -1)
            {
                gvVoltage.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlVoltageEdit.Visible = false;

            lbtnVoltageEdit.Visible = false;
            lbtnVoltageEdit.Visible = false;
        }


        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.
            txtTarriffName.Text = "";

            if (gvTarrif.SelectedIndex != -1)
            {
                gvTarrif.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlEdit.Visible = false;

            lbtnEdit.Visible = false;
            pDetails.Visible = false;
        }

        protected void lbtnActiveEnergyCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.
            txtActiveEnergyChargeName.Text = "";
            txtActiveEnergyCostcPerKWh.Text = "";

            if (gvActiveEnergy.SelectedIndex != -1)
            {
                gvActiveEnergy.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlActiveEnergyEdit.Visible = false;

            lbtnActiveEnergyEdit.Visible = false;
            lbtnActiveEnergyEdit.Visible = false;
        }

        protected void TabChargeType_Click(object sender, EventArgs e)
        {
            TabActiveEnergy.CssClass = "Clicked";
            TabNetworkCharge.CssClass = "Initial";
            TabVoltageSurcharge.CssClass = "Initial";

            MainView.ActiveViewIndex = 0;

        }

        protected void TabNetworkCharge_Click(object sender, EventArgs e)
        {
            TabActiveEnergy.CssClass = "Initial";
            TabNetworkCharge.CssClass = "Clicked";
            TabVoltageSurcharge.CssClass = "Initial";

            MainView.ActiveViewIndex = 1;

        }


        protected void TabVoltageSurcharge_Click(object sender, EventArgs e)
        {
            TabActiveEnergy.CssClass = "Initial";
            TabNetworkCharge.CssClass = "Initial";
            TabVoltageSurcharge.CssClass = "Clicked";

            MainView.ActiveViewIndex = 2;
        }
        protected void CheckSecurity()
        {
            string name = System.IO.Path.GetFileName(Request.ServerVariables("SCRIPT_NAME")).Replace(".aspx", "");
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
            int MyIPMonPageSecure = IPMonPageSecure.GetEditLevelByName(name);
            MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
            if (MyIPMonPageSecure > MyUser.UserLevel)
            {
                Response.Redirect("NotAuthorisedEdit.aspx");
            }
        }
        public MeteringEditTarrif()
        {
            Load += Page_Load;
        }

    }
}