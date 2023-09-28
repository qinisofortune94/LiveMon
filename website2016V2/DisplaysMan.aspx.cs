using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2
{
    public partial class DisplaysMan : System.Web.UI.Page
    {
        private int selectedDisplayID;
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
                //ok logged on level ?
                if (!IsPostBack)
                {
                    LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();


                    successMessage.Visible = false;
                    warningMessage.Visible = false;
                    errorMessage.Visible = false;

                    List<LiveMonitoring.IRemoteLib.DisplayDetails> displayItems = remoteProc.LiveMonServer.GetAllDisplayNames();

                    List<object> displayObjectItems = new List<object>();

                    foreach (LiveMonitoring.IRemoteLib.DisplayDetails display in displayItems)
                    {
                        displayObjectItems.Add(new
                        {
                            ID = display.ID,
                            DisplayName = display.DisplayName,
                            DisplayType = display.DisplayType,
                            ExtraData = display.ExtraData,
                            ExtraValue = display.ExtraValue,
                            DefaultOrderByColumn = display.DefaultOrderByColumn
                        });

                    }
                    // Initialize the DataTable and store it in ViewState.

                    LiveMonitoring.IRemoteLib.DisplayDetails displayObject = new LiveMonitoring.IRemoteLib.DisplayDetails();
                    InitializeDataSource(displayObject.GetType(), "DisplayNames", dataList: displayItems);

                    // Enable the GridView paging option and specify the page size.
                    gvDisplayNames.AllowPaging = true;
                    gvDisplayNames.PageSize = 5;

                    gvDisplayGroups.AllowPaging = true;
                    gvDisplayGroups.PageSize = 5;

                    gvDisplayGroupsLinks.AllowPaging = true;
                    gvDisplayGroupsLinks.PageSize = 5;

                    gvDisplaySensorLink.AllowPaging = true;
                    gvDisplaySensorLink.PageSize = 5;

                    // Enable the GridView sorting option.
                    gvDisplayNames.AllowSorting = true;
                    gvDisplayGroups.AllowSorting = true;
                    gvDisplayGroupsLinks.AllowSorting = true;
                    gvDisplaySensorLink.AllowSorting = true;

                    // Initialize the sorting expression.
                    // ViewState("SortExpression") = "ID ASC"
                    ViewState["SortExpression"] = "ID DESC";

                    // Populate the GridView.
                    BindGridView("dtDisplayNames", gvDisplayNames);

                    TabDisplaySensorLink.CssClass = "Clicked";
                    MainView.ActiveViewIndex = 0;
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            
        }

        private void InitializeDataSource(Type type, string tableName, List<LiveMonitoring.IRemoteLib.DisplayDetails> dataList = null,
              List<LiveMonitoring.IRemoteLib.DisplayGroup> dataList1 = null,
               List<LiveMonitoring.IRemoteLib.DisplayGroupLink> dataList2 = null, List<LiveMonitoring.IRemoteLib.DisplaySensorLink> dataList3 = null)
        {
            // Create a DataTable object named dtMeteringTariff.
            DataTable dataTable = new DataTable();

            setDataTable(ref dataTable, type, tableName, dataList, dataList1, dataList2, dataList3);

        }


        private void setDataTable(ref DataTable dataTable, Type type, string tableName, List<LiveMonitoring.IRemoteLib.DisplayDetails> dataList = null, List<LiveMonitoring.IRemoteLib.DisplayGroup> dataList1 = null,
             List<LiveMonitoring.IRemoteLib.DisplayGroupLink> dataList2 = null, List<LiveMonitoring.IRemoteLib.DisplaySensorLink> dataList3 = null)
        {
            FieldInfo[] fieldList = type.GetFields();

            foreach (FieldInfo field in fieldList)
            {
                dataTable.Columns.Add(field.Name);

                if (field.Name == "ID")
                {
                    // Set ID column as the primary key.

                    DataColumn[] dcKeys = new DataColumn[1];
                    dcKeys[0] = dataTable.Columns[field.Name];
                    dataTable.PrimaryKey = dcKeys;

                }
            }

            //LiveMonitoring.IRemoteLib.MeteringTariff item2 = (LiveMonitoring.IRemoteLib.MeteringTariff)item;


            switch (tableName)
            {
                case "DisplayNames":
                    foreach (LiveMonitoring.IRemoteLib.DisplayDetails item in dataList)
                    {
                        dataTable.Rows.Add(item.ID, item.DisplayName, item.DisplayType, item.ExtraData, item.ExtraValue, item.DefaultOrderByColumn);
                    }

                    break;
                case "DisplayGroup":

                    foreach (LiveMonitoring.IRemoteLib.DisplayGroup item1 in dataList1)
                    {
                        dataTable.Rows.Add(item1.ID, item1.DisplayID, item1.GroupName, item1.DisplayType, item1.DisplayImage, item1.DisplayWidth, item1.DisplayHeight, item1.Screen, item1.PanelNo, item1.PanelPos,
                        item1.ExtraData1, item1.ExtraData2, item1.ExtraValue1, item1.ExtraValue2);
                    }

                    break;
                case "DisplayGroupLink":

                    foreach (LiveMonitoring.IRemoteLib.DisplayGroupLink item2 in dataList2)
                    {
                        dataTable.Rows.Add(item2.ID, item2.DisplayID, item2.GroupID, item2.Screen, item2.PanelNo, item2.PanelPos);
                    }

                    break;
                case "DisplaySensorLink":

                    foreach (LiveMonitoring.IRemoteLib.DisplaySensorLink item3 in dataList3)
                    {
                        dataTable.Rows.Add(item3.ID, item3.DisplayGroupID, item3.SensorID, item3.DisplayOrder, item3.ExtraData1, item3.ExtraData2, item3.ExtraValue1, item3.ExtraValue2);
                    }


                    break;
                default:
                    break; // TODO: might not be correct. Was : Exit Select


            }



            ViewState["dt" + tableName] = dataTable;
        }


        private void BindGridView(string vState, GridView gvData)
        {
            if (ViewState[vState] != null)
            {
                // Get the DataTable from ViewState.
                DataTable dataTable = (DataTable)ViewState[vState];

                // Convert the DataTable to DataView.
                DataView dataView = new DataView(dataTable);

                // Set the sort column and sort order.
                dataView.Sort = ViewState["SortExpression"].ToString();

                gvData.DataSource = dataView;
                gvData.DataBind();

            }
        }


        protected void gvDisplayNames_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page. 
            gvDisplayNames.PageIndex = e.NewPageIndex;

            // Rebind the GridView control to 
            // show data in the new page.
            BindGridView("dtDisplayNames", gvDisplayNames);
        }

        protected void gvDisplayGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page. 
            gvDisplayGroups.PageIndex = e.NewPageIndex;

            // Rebind the GridView control to 
            // show data in the new page.
            BindGridView("dtDisplayGroup", gvDisplayGroups);
        }
        protected void gvDisplayGroupLinks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page. 
            gvDisplayGroupsLinks.PageIndex = e.NewPageIndex;

            // Rebind the GridView control to 
            // show data in the new page.
            BindGridView("dtDisplayGroupLink", gvDisplayGroupsLinks);
        }

        // GridView.RowEditing Event
        protected void gvDisplayNames_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Make the GridView control into edit mode 
            // for the selected row. 
            gvDisplayNames.EditIndex = e.NewEditIndex;

            // Rebind the GridView control to show data in edit mode.
            BindGridView("dtMeteringTariff", gvDisplayNames);

            // Hide the Add button.
            lbtnEditDisplay.Visible = false;
        }

        protected void gvDisplayNames_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Exit edit mode.
            gvDisplayNames.EditIndex = -1;

            // Rebind the GridView control to show data in view mode.
            BindGridView("dtMeteringTariff", gvDisplayNames);

            // Show the Add button.
            lbtnEditDisplay.Visible = true;
        }
        // '===================================================
        // '======================UPDATE=======================
        // '===================================================
        // GridView.RowUpdating Event
        protected void gvDisplayNames_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ViewState["dtDisplayNames"] != null)
            {
                // Get the DataTable from ViewState.
                DataTable dtDisplayName = (DataTable)ViewState["dtDisplayNames"];

                // Get the TarrifID of the selected row.
                string strDisplayNameID = gvDisplayNames.Rows[e.RowIndex].Cells[2].Text;

                // Find the row in DateTable.
                DataRow drDisplayName = dtDisplayName.Rows.Find(strDisplayNameID);

                // Retrieve edited values and updating respective items.
                drDisplayName["DisplayName"] = ((TextBox)gvDisplayNames.Rows[e.RowIndex].FindControl("TextBox1")).Text;
                drDisplayName["DisplayType"] = ((TextBox)gvDisplayNames.Rows[e.RowIndex].FindControl("TextBox2")).Text;

                // Exit edit mode.
                gvDisplayNames.EditIndex = -1;

                // Rebind the GridView control to show data after updating.
                BindGridView("dtMeteringTariff", gvDisplayNames);

                // Show the Add button.
                lbtnEditDisplay.Visible = true;
            }
        }



        protected void gvDisplayGroups_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ViewState["dtMeteringActiveEnergyCharges"] != null)
            {
                // Get the DataTable from ViewState.
                //Dim dtMeteringTariff As DataTable = DirectCast(ViewState("dtMeteringActiveEnergyCharges"), DataTable)

                //' Get the TarrifID of the selected row.
                //Dim strTarrifID As String = gvTarrif.Rows(e.RowIndex).Cells(2).Text

                //' Find the row in DateTable.
                //Dim drTarrif As DataRow = dtMeteringTariff.Rows.Find(strTarrifID)

                //' Retrieve edited values and updating respective items.
                //drTarrif("LastName") = DirectCast(gvTarrif.Rows(e.RowIndex).FindControl("TextBox1"), TextBox).Text
                //drTarrif("FirstName") = DirectCast(gvTarrif.Rows(e.RowIndex).FindControl("TextBox2"), TextBox).Text

                //' Exit edit mode.
                //gvTarrif.EditIndex = -1

                //' Rebind the GridView control to show data after updating.
                //BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy)

                //' Show the Add button.
                //lbtnEdit.Visible = True
            }
        }

        // GridView.SelectedIndexChanging Event


        protected void gvDisplaySensorLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvDisplaySensorLink.SelectedRow;

            dynamic selectedDisplaySensorLinkID = Convert.ToInt32(row.Cells[2].Text);

            Session["selectedDisplaySensorLinkID"] = selectedDisplaySensorLinkID;

            lbtnDisplaySensorLinkEdit.Visible = true;

        }


        protected void gvDisplayNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvDisplayNames.SelectedRow;
            selectedDisplayID = Convert.ToInt32(row.Cells[2].Text);
            Session["selectedDisplayID"] = selectedDisplayID;

            pDetails.Visible = false;
            //Dim selectedDisplayName As String = row.Cells(3).Text

            LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();

            //' =======LOADING DISPLAY GROUPS =========

            dynamic displayGroupItems = from a in remoteProc.LiveMonServer.GetAllDisplayGroups() where a.DisplayID == selectedDisplayID select a;

            LiveMonitoring.IRemoteLib.DisplayGroup tempObject = new LiveMonitoring.IRemoteLib.DisplayGroup();
            loadItems(displayGroupItems, gvDisplayGroups, tempObject.GetType());
            pDisplayGroups.Visible = true;

            lbtnEditDisplay.Visible = true;

        }



        protected void loadItems(IEnumerable<object> itemList, GridView gridView, Type type)
        {

            List<LiveMonitoring.IRemoteLib.DisplayGroup> resultlist = new List<LiveMonitoring.IRemoteLib.DisplayGroup>();
            List<LiveMonitoring.IRemoteLib.DisplayGroupLink> resultlist1 = new List<LiveMonitoring.IRemoteLib.DisplayGroupLink>();
            List<LiveMonitoring.IRemoteLib.DisplaySensorLink> resultlist2 = new List<LiveMonitoring.IRemoteLib.DisplaySensorLink>();




            foreach (var item_loopVariable in itemList)
            {
                //System.Reflection.PropertyInfo pi = item.GetType().GetProperty("ID");
                //int ID = (int)(pi.GetValue(item, null));

                //pi = item.GetType().GetProperty("TarriffName");
                //String name = (String)(pi.GetValue(item, null));

                //item2.ID = ID;
                //item2.TarriffName = name;

                //dataTable.Rows.Add(item2.ID, item2.TarriffName);

                switch (type.Name)
                {
                    case "DisplayGroup":
                        LiveMonitoring.IRemoteLib.DisplayGroup item1 = (LiveMonitoring.IRemoteLib.DisplayGroup)item_loopVariable;
                        resultlist.Add(item1);
                        break;
                    case "DisplayGroupLink":
                        LiveMonitoring.IRemoteLib.DisplayGroupLink item2 = (LiveMonitoring.IRemoteLib.DisplayGroupLink)item_loopVariable;
                        resultlist1.Add(item2);
                        break;
                    case "DisplaySensorLink":
                        LiveMonitoring.IRemoteLib.DisplaySensorLink item3 = (LiveMonitoring.IRemoteLib.DisplaySensorLink)item_loopVariable;
                        resultlist2.Add(item3);
                        break;

                }

            }

            switch (type.Name)
            {
                case "DisplayGroup":
                    InitializeDataSource(type, type.Name, dataList1: resultlist);
                    break;
                case "DisplayGroupLink":
                    InitializeDataSource(type, type.Name, dataList2: resultlist1);
                    break;
                case "DisplaySensorLink":
                    InitializeDataSource(type, type.Name, dataList3: resultlist2);

                    break;
            }



            BindGridView("dt" + type.Name, gridView);
        }



        protected void gvDisplayGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow rowf = gvDisplayGroups.SelectedRow;

            int selectedDisplayGroupID = Convert.ToInt32(rowf.Cells[2].Text);
            Session["selectedDisplayGroupID"] = selectedDisplayGroupID;

            lbtnDisplayGroupEdit.Visible = true;
            LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();

            //As List(Of LiveMonitoring.IRemoteLib.DisplaySensorLink)
            dynamic sensorLinkItems = from a in remoteProc.LiveMonServer.GetAllDisplaySensorLinks() where a.DisplayGroupID == selectedDisplayGroupID select a;

            LiveMonitoring.IRemoteLib.DisplaySensorLink displaySensorLinkObject = new LiveMonitoring.IRemoteLib.DisplaySensorLink();

            loadItems(sensorLinkItems, gvDisplaySensorLink, displaySensorLinkObject.GetType());

            //Dim sensorLinkObjectItems As List(Of Object) = New List(Of Object)()
            //For Each sensorLink As LiveMonitoring.IRemoteLib.DisplaySensorLink In sensorLinkItems

            // sensorLinkObjectItems.Add(New With {.ID = sensorLink.ID, .DisplayGroupID = sensorLink.DisplayGroupID, .SensorID = sensorLink.SensorID, .DisplayOrder = sensorLink.DisplayOrder, .ExtraData1 = sensorLink.ExtraData1, .ExtraData2 = sensorLink.ExtraData2, .ExtraValue1 = sensorLink.ExtraValue1, .ExtraValue2 = sensorLink.ExtraValue2})

            //Next

            //Dim sensorLinkObject As LiveMonitoring.IRemoteLib.DisplaySensorLink = New LiveMonitoring.IRemoteLib.DisplaySensorLink()
            //InitializeDataSource(sensorLinkObject.GetType(), sensorLinkObjectItems, "DisplaySensorLink")

            // Enable the GridView paging option and specify the page size.
            //gvDisplaySensorLink.AllowPaging = True
            //gvDisplaySensorLink.PageSize = 5


            // Initialize the sorting expression.
            //ViewState("SortExpression") = "ID ASC"
            //ViewState("SortExpression") = "ID DESC"

            // Populate the GridView.
            //BindGridView("dtDisplaySensorLink", gvDisplaySensorLink)


            // =======LOADING DISPLAY GROUPS LINK =========

            int selectedDisplayID = Convert.ToInt32(Session["selectedDisplayID"]);

            dynamic displayGroupLinkItems = from n in remoteProc.LiveMonServer.GetAllDisplayGroupLinks() where n.DisplayID == selectedDisplayID & n.GroupID == selectedDisplayGroupID select n;

            LiveMonitoring.IRemoteLib.DisplayGroupLink displayGroupLinkObject = new LiveMonitoring.IRemoteLib.DisplayGroupLink();

            loadItems(displayGroupLinkItems, gvDisplayGroupsLinks, displayGroupLinkObject.GetType());

            pDetails.Visible = true;


        }

        protected void gvDisplayGroupsLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow rowf = gvDisplayGroupsLinks.SelectedRow;

            int selectedDisplayGroupLinkID = Convert.ToInt32(rowf.Cells[2].Text);
            Session["selectedDisplayGroupLinkID"] = selectedDisplayGroupLinkID;

            lbtnDisplayGroupsLinksEdit.Visible = true;


        }

        // Protected Sub gvVoltage_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        // Dim rowf As GridViewRow = gvVoltage.SelectedRow

        // Dim selectedVoltageID As Integer = CType(rowf.Cells(2).Text, Integer)
        // Session["selectedVoltageID"] = selectedVoltageID

        // lbtnVoltageEdit.Visible = True

        // txtVoltageVoltage.Text = rowf.Cells(3).Text
        // txtVoltageSurchargePercentage.Text = rowf.Cells(4).Text

        // End Sub

        // ' GridView.RowDeleting Event

        protected void gvDisplayNames_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState["dtDisplayNames"] != null)
            {
                // Get the DisplayID of the selected row.
                string strDisplayID = gvDisplayNames.Rows[e.RowIndex].Cells[2].Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isDisplayNameDeleted = remoteProc.LiveMonServer.DeleteDisplay(Convert.ToInt32(strDisplayID));

                if (isDisplayNameDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtDisplayNames = (DataTable)ViewState["dtDisplayNames"];

                    // Find the row in DateTable.
                    DataRow drDisplayName = dtDisplayNames.Rows.Find(strDisplayID);

                    // Remove the row from the DataTable.
                    dtDisplayNames.Rows.Remove(drDisplayName);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtDisplayNames", gvDisplayNames);

                }


            }
        }
        // GridView.RowDeleting Event

        protected void gvDisplayGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState["dtDisplayGroup"] != null)
            {
                // Get the DisplayGroupID of the selected row.
                string strDisplayGroupID = gvDisplayGroups.Rows[e.RowIndex].Cells[2].Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isDisplayGroupDeleted = remoteProc.LiveMonServer.DeleteDisplayGroup(Convert.ToInt32(strDisplayGroupID));

                if (isDisplayGroupDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtDisplayGroup = (DataTable)ViewState["dtDisplayGroup"];


                    // Find the row in DateTable.
                    DataRow drDisplayGroup = dtDisplayGroup.Rows.Find(strDisplayGroupID);

                    // Remove the row from the DataTable.
                    dtDisplayGroup.Rows.Remove(drDisplayGroup);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtDisplayGroup", gvDisplayGroups);
                }

            }
        }

        protected void gvDisplayGroupsLinks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState["dtDisplayGroupLink"] != null)
            {
                // Get the DisplayGroupsLinkID of the selected row.
                string strDisplayGroupsLinkID = gvDisplayGroupsLinks.Rows[e.RowIndex].Cells[2].Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isDisplayGroupsLinkDeleted = remoteProc.LiveMonServer.DeleteDisplayGroupLink(Convert.ToInt32(strDisplayGroupsLinkID));

                if (isDisplayGroupsLinkDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtDisplayGroupLink = (DataTable)ViewState["dtDisplayGroupLink"];

                    // Find the row in DateTable.
                    DataRow drDisplayGroupsLink = dtDisplayGroupLink.Rows.Find(strDisplayGroupsLinkID);

                    // Remove the row from the DataTable.
                    dtDisplayGroupLink.Rows.Remove(drDisplayGroupsLink);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtDisplayGroupLink", gvDisplayGroupsLinks);
                }

            }
        }


        protected void gvDisplaySensorLink_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (ViewState["dtDisplaySensorLink"] != null)
            {
                // Get the DisplaySensorLinkID of the selected row.
                string strDisplaySensorLinkID = gvDisplaySensorLink.Rows[e.RowIndex].Cells[2].Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isDisplaySensorLinkDeleted = remoteProc.LiveMonServer.DeleteDisplaySensorLink(Convert.ToInt32(strDisplaySensorLinkID));

                if (isDisplaySensorLinkDeleted == true)
                {
                    // Get the DataTable from ViewState.
                    DataTable dtDisplaySensorLink = (DataTable)ViewState["dtDisplaySensorLink"];

                    // Find the row in DateTable.
                    DataRow drdtDisplaySensorLink = dtDisplaySensorLink.Rows.Find(strDisplaySensorLinkID);

                    // Remove the row from the DataTable.
                    dtDisplaySensorLink.Rows.Remove(drdtDisplaySensorLink);

                    // Rebind the GridView control to show data after deleting.
                    BindGridView("dtDisplaySensorLink", gvDisplaySensorLink);
                }

            }
        }

        // GridView.Sorting Event
        protected void gvDisplayNames_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Dim strSortExpression As String() = ViewState("SortExpression").ToString().Split(" "c)

            // ' If the sorting column is the same as the previous one, 
            // ' then change the sort order.
            // If strSortExpression(0) = e.SortExpression Then
            // If strSortExpression(1) = "ASC" Then
            // ViewState("SortExpression") = Convert.ToString(e.SortExpression) & " " & "DESC"
            // Else
            // ViewState("SortExpression") = Convert.ToString(e.SortExpression) & " " & "ASC"
            // End If
            // Else
            // ' If sorting column is another column, 
            // ' then specify the sort order to "Ascending".
            // ViewState("SortExpression") = Convert.ToString(e.SortExpression) & " " & "ASC"
            // End If

            // ' Rebind the GridView control to show sorted data.
            // BindGridView("dtMeteringTariff", gvTarrif)
            // End Sub
            // ' GridView.Sorting Event
            // Protected Sub gvActiveEnergy_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
            // Dim strSortExpression As String() = ViewState("SortExpression").ToString().Split(" "c)

            // ' If the sorting column is the same as the previous one, 
            // ' then change the sort order.
            // If strSortExpression(0) = e.SortExpression Then
            // If strSortExpression(1) = "ASC" Then
            // ViewState("SortExpression") = Convert.ToString(e.SortExpression) & " " & "DESC"
            // Else
            // ViewState("SortExpression") = Convert.ToString(e.SortExpression) & " " & "ASC"
            // End If
            // Else
            // ' If sorting column is another column, 
            // ' then specify the sort order to "Ascending".
            // ViewState("SortExpression") = Convert.ToString(e.SortExpression) & " " & "ASC"
            // End If

            // ' Rebind the GridView control to show sorted data.
            // BindGridView("dtMeteringActiveEnergyCharges", gvActiveEnergy)
        }


        protected void lbtnEditDisplay_Click(object sender, EventArgs e)
        {
            GridViewRow row = gvDisplayNames.SelectedRow;

            txtDisplayName.Text = row.Cells[3].Text;
            txtDisplayType.Text = row.Cells[4].Text;
            txtExtraData.Text = row.Cells[5].Text;
            txtExtraValue.Text = row.Cells[6].Text;
            txtDefaultOrderByColumn.Text = row.Cells[7].Text;

            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            pnlEditDisplay.Visible = false;
            pnlEditDisplay.Visible = true;
        }

        protected void lbtnAddDisplay_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // ' Hide the Add button and showing Add panel.
            lbtnEditDisplay.Visible = false;
            pnlEditDisplay.Visible = true;
        }

        // Protected Sub lbtnVoltageEdit_Click(ByVal sender As Object, ByVal e As EventArgs)

        // Session["operation"] = Operations.update

        // ' Hide the Add button and showing Add panel.

        // lbtnVoltageEdit.Visible = False
        // pnlVoltageEdit.Visible = True
        // End Sub
        // Protected Sub lbtnVoltageAdd_Click(ByVal sender As Object, ByVal e As EventArgs)

        // Session["operation"] = Operations.add

        // ' Hide the Add button and showing Add panel.

        // lbtnVoltageEdit.Visible = False
        // pnlVoltageEdit.Visible = True
        // End Sub


        protected void lbtnDisplayGroupsLinksEdit_Click(object sender, EventArgs e)
        {
            GridViewRow rowf = gvDisplayGroupsLinks.SelectedRow;

            //txtDisplayGroupsDisplayID.Text = CType(rowf.Cells(3).Text, Integer)
            //txtDisplayGroupsGroupID.Text = CType(rowf.Cells(4).Text, Integer)
            txtDisplayGroupsScreen.Text = Convert.ToInt32(rowf.Cells[5].Text).ToString();
            txtDisplayGroupsPanelNo.Text = Convert.ToInt32(rowf.Cells[6].Text).ToString();
            txtDisplayGroupsPanelPos.Text = Convert.ToInt32(rowf.Cells[7].Text).ToString();

            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            lbtnDisplayGroupsLinksEdit.Visible = false;
            pnlDisplayGroupsLinksEdit.Visible = true;
        }

        protected void lbtnDisplayGroupsLinksAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.

            lbtnDisplayGroupsLinksEdit.Visible = false;
            pnlDisplayGroupsLinksEdit.Visible = true;
        }


        




        protected void lbtnDisplayGroupEdit_Click(object sender, EventArgs e)
        {
            GridViewRow rowf = gvDisplayGroups.SelectedRow;

            txtDisplayGroupName.Text = rowf.Cells[4].Text;
            txtDisplayGroupDisplayType.Text = rowf.Cells[5].Text;
            txtDisplayGroupDisplayImage.Text = rowf.Cells[6].Text;

            txtDisplayGroupDisplayWidth.Text = rowf.Cells[7].Text;
            txtDisplayGroupDisplayHeight.Text = rowf.Cells[8].Text;
            txtDisplayGroupScreen.Text = rowf.Cells[9].Text;

            txtDisplayGroupPanelNo.Text = rowf.Cells[10].Text;
            txtDisplayGroupPanelPos.Text = rowf.Cells[11].Text;
            txtDisplayGroupExtraData1.Text = rowf.Cells[12].Text;

            txtDisplayGroupExtraData2.Text = rowf.Cells[13].Text;
            txtDisplayGroupExtraValue1.Text = rowf.Cells[14].Text;
            txtDisplayGroupExtraValue2.Text = rowf.Cells[15].Text;

            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            lbtnDisplayGroupEdit.Visible = false;
            pnlDisplayGroupEdit.Visible = true;
        }

        protected void gvDisplayNames_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Make sure the current GridViewRow is a data row.
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Make sure the current GridViewRow is either 
                // in the normal state or an alternate row.
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    // Add client-side confirmation when deleting.
                    ((LinkButton)e.Row.Cells[1].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you certain you want to delete this item ?')) return false;";
                }
            }
        }
        protected void lbtnDisplaySensorLinkAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.

            lbtnDisplaySensorLinkEdit.Visible = false;
            pnlDisplaySensorLinkEdit.Visible = true;
        }

        protected void lbtnDisplaySensorLinkEdit_Click(object sender, EventArgs e)
        {
            GridViewRow rowf = gvDisplaySensorLink.SelectedRow;

            // txtDisplayGroupID.Text = rowf.Cells(3).Text
            txtSensorID.Text = rowf.Cells[4].Text;
            txtDisplayOrder.Text = rowf.Cells[5].Text;

            txtExtraData1.Text = rowf.Cells[6].Text;
            txtExtraData2.Text = rowf.Cells[7].Text;
            txtExtraValue1.Text = rowf.Cells[8].Text;

            txtExtraValue2.Text = rowf.Cells[9].Text;

            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.update;

            // Hide the Add button and showing Add panel.
            lbtnDisplaySensorLinkEdit.Visible = false;
            pnlDisplaySensorLinkEdit.Visible = true;

        }

        protected void lbtnDisplayGroupAdd_Click(object sender, EventArgs e)
        {
            Session["operation"] = LiveMonitoring.IRemoteLib.Operations.@add;

            // Hide the Add button and showing Add panel.

            lbtnDisplayGroupEdit.Visible = false;
            pnlDisplayGroupEdit.Visible = true;
        }

        protected void lbtnDisplaySensorLinkSubmit_Click(object sender, EventArgs e)
        {

            CheckSecurity();


            if (ViewState["dtDisplaySensorLink"] != null)
            {


                if (!Regex.IsMatch(txtExtraValue1.Text,
                 @"(^([0-9]*|\d*\d{1}?\d*)$)"))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Extra Value 1 must be Number";
                    txtExtraValue1.Focus();
                    return;
                }
                else
                {
                    warningMessage.Visible = false;
                }

                if (!Regex.IsMatch(txtExtraValue2.Text,
                    @"(^([0-9]*|\d*\d{1}?\d*)$)"))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Extra Value 2 must be Number";
                    txtExtraValue2.Focus();
                    return;
                }
                else
                {
                    warningMessage.Visible = false;
                }

                if (!Regex.IsMatch(txtDisplayOrder.Text,
                    @"(^([0-9]*|\d*\d{1}?\d*)$)"))
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Display Order Must be a Number";
                    txtDisplayOrder.Focus();
                    return;
                }
                else
                {
                    warningMessage.Visible = false;
                }

                LiveMonitoring.IRemoteLib.DisplaySensorLink displaySensorLink = new LiveMonitoring.IRemoteLib.DisplaySensorLink();

                displaySensorLink.DisplayGroupID = Convert.ToInt32(Session["selectedDisplayGroupID"]);
                displaySensorLink.SensorID = Convert.ToInt32(txtSensorID.Text);
                displaySensorLink.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                displaySensorLink.ExtraData1 = txtExtraData1.Text;
                displaySensorLink.ExtraData2 = txtExtraData2.Text;
                displaySensorLink.ExtraValue1 = Convert.ToDouble(txtExtraValue1.Text);
                displaySensorLink.ExtraValue2 = Convert.ToDouble(txtExtraValue2.Text);


            
                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isDisplaySensorLinkSaved = false;


                if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    displaySensorLink.ID = Convert.ToInt32(Session["selectedDisplaySensorLinkID"]);

                    isDisplaySensorLinkSaved = remoteProc.LiveMonServer.EditDisplaySensorLink(displaySensorLink);

                    if (isDisplaySensorLinkSaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtDisplaySensorLink = (DataTable)ViewState["dtDisplaySensorLink"];

                        // Find the row in DateTable.
                        DataRow drDisplaySensorLink = dtDisplaySensorLink.Rows.Find(Session["selectedDisplaySensorLinkID"]);

                        // Retrieve edited values and updating respective items.

                        drDisplaySensorLink["ID"] = Session["selectedDisplaySensorLinkID"];
                        drDisplaySensorLink["DisplayGroupID"] = Session["selectedDisplayGroupID"];
                        drDisplaySensorLink["SensorID"] = txtSensorID.Text;
                        drDisplaySensorLink["DisplayOrder"] = txtDisplayOrder.Text;
                        drDisplaySensorLink["ExtraData1"] = txtExtraData1.Text;
                        drDisplaySensorLink["ExtraData2"] = txtExtraData2.Text;
                        drDisplaySensorLink["ExtraValue1"] = txtExtraValue1.Text;
                        drDisplaySensorLink["ExtraValue2"] = txtExtraValue2.Text;

                        // Exit edit mode.
                        gvDisplaySensorLink.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtDisplaySensorLink", gvDisplaySensorLink);

                    }

                }
                else if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    int displaySensorLinkID = 0;
                    try
                    {
                        displaySensorLinkID = remoteProc.LiveMonServer.AddNewDisplaySensorLink(displaySensorLink);


                    }
                    catch (Exception ex)
                    {
                    }


                    if (displaySensorLinkID != -99 & displaySensorLinkID != 0)
                    {
                        DataTable dt = (DataTable)ViewState["dtDisplaySensorLink"];
                        DataRow dr = dt.NewRow();

                        dr["ID"] = displaySensorLinkID;
                        dr["DisplayGroupID"] = Session["selectedDisplayGroupID"];
                        dr["SensorID"] = txtSensorID.Text;
                        dr["DisplayOrder"] = txtDisplayOrder.Text;
                        dr["ExtraData1"] = txtExtraData1.Text;
                        dr["ExtraData2"] = txtExtraData2.Text;
                        dr["ExtraValue1"] = txtExtraValue1.Text;
                        dr["ExtraValue2"] = txtExtraValue2.Text;

                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtDisplaySensorLink", gvDisplaySensorLink);

                    }

                }
            }

            // ' Empty the TextBox controls.

            txtSensorID.Text = "";
            txtDisplayOrder.Text = "";
            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraValue1.Text = "";
            txtExtraValue2.Text = "";

            // Show the Add button and hiding the Add panel.

            lbtnDisplaySensorLinkEdit.Visible = true;
            pnlDisplaySensorLinkEdit.Visible = false;



        }

        protected void lbtnDisplayGroupsLinksSubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState["dtDisplayGroupLink"] != null)
            {
                LiveMonitoring.IRemoteLib.DisplayGroupLink displayGroupLink = new LiveMonitoring.IRemoteLib.DisplayGroupLink();





                displayGroupLink.DisplayID = Convert.ToInt32(Session["selectedDisplayID"]);
                displayGroupLink.GroupID = Convert.ToInt32(Session["selectedDisplayGroupID"]);
                displayGroupLink.PanelNo = Convert.ToInt32(txtDisplayGroupsPanelNo.Text);
                displayGroupLink.PanelPos = Convert.ToInt32(txtDisplayGroupsPanelPos.Text);
                displayGroupLink.Screen = Convert.ToInt32(txtDisplayGroupsScreen.Text);



                if (ViewState["dtDisplaySensorLink"] != null)
                {


                    if (!Regex.IsMatch(txtDisplayGroupPanelNo.Text,
                     @"(^([0-9]*|\d*\d{1}?\d*)$)"))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Extra Value 1 must be Number";
                        txtDisplayGroupPanelNo.Focus();
                        return;
                    }
                    else
                    {
                        warningMessage.Visible = false;
                    }

                    if (!Regex.IsMatch(txtDisplayGroupPanelPos.Text,
                        @"(^([0-9]*|\d*\d{1}?\d*)$)"))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Extra Value 2 must be Number";
                        txtDisplayGroupPanelPos.Focus();
                        return;
                    }
                    else
                    {
                        warningMessage.Visible = false;
                    }

                    if (!Regex.IsMatch(txtDisplayGroupScreen.Text,
                        @"(^([0-9]*|\d*\d{1}?\d*)$)"))
                    {
                        warningMessage.Visible = true;
                        lblWarning.Text = "Display Order Must be a Number";
                        txtDisplayGroupScreen.Focus();
                        return;
                    }
                    else
                    {
                        warningMessage.Visible = false;
                    }

                    LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                    bool isDisplayGroupLinkSaved = false;


                    if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                    {
                        displayGroupLink.ID = Convert.ToInt32(Session["selectedDisplayGroupLinkID"]);

                        isDisplayGroupLinkSaved = remoteProc.LiveMonServer.EditDisplayGroupLink(displayGroupLink);

                        if (isDisplayGroupLinkSaved == true)
                        {
                            // Get the DataTable from ViewState.

                            DataTable dtDisplayGroupLink = (DataTable)ViewState["dtDisplayGroupLink"];

                            // Find the row in DateTable.
                            DataRow drDisplayGroupLink = dtDisplayGroupLink.Rows.Find(Session["selectedDisplayGroupLinkID"]);

                            // Retrieve edited values and updating respective items.

                            drDisplayGroupLink["DisplayID"] = Session["selectedDisplayID"];
                            drDisplayGroupLink["GroupID"] = Session["selectedDisplayGroupID"];

                            drDisplayGroupLink["PanelNo"] = txtDisplayGroupsPanelNo.Text;
                            drDisplayGroupLink["PanelPos"] = txtDisplayGroupsPanelPos.Text;
                            drDisplayGroupLink["Screen"] = txtDisplayGroupsScreen.Text;

                            // Exit edit mode.
                            gvDisplayGroupsLinks.EditIndex = -1;

                            // Rebind the GridView control to show data after updating.
                            BindGridView("dtDisplayGroupLink", gvDisplayGroupsLinks);

                        }

                    }
                    else if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                    {
                        int displayGroupLinkID = 0;
                        try
                        {
                            displayGroupLinkID = remoteProc.LiveMonServer.AddNewDisplayGroupLink(displayGroupLink);


                        }
                        catch (Exception ex)
                        {
                        }


                        if (displayGroupLinkID != -99 & displayGroupLinkID != 0)
                        {
                            DataTable dt = (DataTable)ViewState["dtDisplayGroupLink"];
                            DataRow dr = dt.NewRow();

                            dr["ID"] = displayGroupLinkID;
                            dr["DisplayID"] = Session["selectedDisplayID"];
                            dr["GroupID"] = Session["selectedDisplayGroupID"];

                            dr["PanelNo"] = txtDisplayGroupsPanelNo.Text;
                            dr["PanelPos"] = txtDisplayGroupsPanelPos.Text;
                            dr["Screen"] = txtDisplayGroupsScreen.Text;

                            dt.Rows.InsertAt(dr, 0);

                            BindGridView("dtDisplayGroupLink", gvDisplayGroupsLinks);

                        }

                    }
                }

                // ' Empty the TextBox controls.
                txtDisplayGroupsPanelNo.Text = "";
                txtDisplayGroupsPanelPos.Text = "";
                txtDisplayGroupsScreen.Text = "";
                // Show the Add button and hiding the Add panel.
                lbtnDisplayGroupsLinksEdit.Visible = true;
                pnlDisplayGroupsLinksEdit.Visible = false;
            }
        }



        protected void lbtnDisplaySubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState["dtDisplayNames"] != null)
            {
                LiveMonitoring.IRemoteLib.DisplayDetails displayName = new LiveMonitoring.IRemoteLib.DisplayDetails();


                if (txtDisplayName.Text.Length < 1)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please supply display name.";

                    return;
                }

                if (txtDisplayType.Text.Length < 1)
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please supply display type.";

                    return;
                }

                if (Information.IsNumeric(txtExtraValue.Text))
                {
                    displayName.ExtraValue = Convert.ToDouble(txtExtraValue.Text);

                }
                else if (txtExtraValue.Text == string.Empty)
                {
                    displayName.ExtraValue = 0;
                }
                else
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please use a numeric value for ExtraValue field.";
                }

                if (Information.IsNumeric(txtDefaultOrderByColumn.Text))
                {
                    displayName.ExtraValue = Convert.ToInt32(txtDefaultOrderByColumn.Text);

                }
                else if (txtDefaultOrderByColumn.Text == string.Empty)
                {
                    displayName.DefaultOrderByColumn = 0;
                }
                else
                {
                    warningMessage.Visible = true;
                    lblWarning.Text = "Please use a numeric value for DefaultOrderByColumn field.";
                }

                displayName.DisplayName = txtDisplayName.Text;
                displayName.DisplayType = txtDisplayType.Text;
                displayName.ExtraData = txtExtraData.Text;

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();
                bool isDisplaySaved = false;


                if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    displayName.ID = Convert.ToInt32(Session["selectedDisplayID"]);
                    isDisplaySaved = remoteProc.LiveMonServer.EditDisplay(displayName);
                    if (isDisplaySaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtDisplayNames = (DataTable)ViewState["dtDisplayNames"];

                        // Find the row in DateTable.
                        DataRow drDisplay = dtDisplayNames.Rows.Find(Session["selectedDisplayID"]);

                        // Retrieve edited values and updating respective items.
                        drDisplay["DisplayName"] = txtDisplayName.Text;
                        drDisplay["DisplayType"] = txtDisplayType.Text;
                        drDisplay["ExtraData"] = txtExtraData.Text;
                        drDisplay["ExtraValue"] = txtExtraValue.Text;
                        drDisplay["DefaultOrderByColumn"] = txtDefaultOrderByColumn.Text;

                        // Exit edit mode.
                        gvDisplayNames.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtDisplayNames", gvDisplayNames);

                    }

                }
                else if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    object displayID = null;

                    try
                    {
                        displayID = remoteProc.LiveMonServer.AddNewDisplay(displayName);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (Convert.ToInt32(displayID) != -99 | Convert.ToInt32(displayID) != 0)
                    {
                        successMessage.Visible = true;
                        lblSucces.Text = "Display added successfully.";

                        DataTable dt = (DataTable)ViewState["dtDisplayNames"];
                        DataRow dr = dt.NewRow();

                        dr["ID"] = displayID;
                        dr["DisplayName"] = txtDisplayName.Text;
                        dr["DisplayType"] = txtDisplayType.Text;
                        dr["ExtraData"] = txtExtraData.Text;
                        dr["ExtraValue"] = txtExtraValue.Text;
                        dr["DefaultOrderByColumn"] = txtDefaultOrderByColumn.Text;

                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtDisplayNames", gvDisplayNames);

                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Add display failed.";
                    }

                }


            }

            // Empty the TextBox controls.

            txtDisplayName.Text = "";
            txtDisplayType.Text = "";

            txtExtraData.Text = "";
            txtExtraValue.Text = "";

            // Show the Add button and hiding the Add panel.

            lbtnEditDisplay.Visible = true;
            pnlEditDisplay.Visible = false;

        }

        protected void lbtnDisplayGroupSubmit_Click(object sender, EventArgs e)
        {
            CheckSecurity();


            if (ViewState["dtDisplayGroup"] != null)
            {
                LiveMonitoring.IRemoteLib.DisplayGroup displayGroupNew = new LiveMonitoring.IRemoteLib.DisplayGroup();

                displayGroupNew.GroupName = txtDisplayGroupName.Text;
                displayGroupNew.DisplayType = Convert.ToInt32(txtDisplayGroupDisplayType.Text);
                displayGroupNew.DisplayImage = txtDisplayGroupDisplayImage.Text;

                displayGroupNew.DisplayWidth = Convert.ToInt32(txtDisplayGroupDisplayWidth.Text);
                displayGroupNew.DisplayHeight = Convert.ToInt32(txtDisplayGroupDisplayHeight.Text);
                displayGroupNew.Screen = Convert.ToInt32(txtDisplayGroupScreen.Text);

                displayGroupNew.PanelNo = Convert.ToInt32(txtDisplayGroupPanelNo.Text);
                displayGroupNew.PanelPos = Convert.ToInt32(txtDisplayGroupPanelPos.Text);
                displayGroupNew.ExtraData1 = txtDisplayGroupExtraData1.Text;

                displayGroupNew.ExtraData2 = txtDisplayGroupExtraData2.Text;
                displayGroupNew.ExtraValue1 = Convert.ToInt32(txtDisplayGroupExtraValue1.Text);
                displayGroupNew.ExtraValue2 = Convert.ToInt32(txtDisplayGroupExtraValue2.Text);

                LiveMonitoring.GlobalRemoteVars remoteProc = new LiveMonitoring.GlobalRemoteVars();


                if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.update)
                {
                    displayGroupNew.ID = Convert.ToInt32(Session["selectedDisplayGroupID"]);
                    displayGroupNew.DisplayID = Convert.ToInt32(Session["selectedDisplayID"]);
                    bool isDisplayGroupSaved = remoteProc.LiveMonServer.EditDisplayGroup(displayGroupNew);

                    if (isDisplayGroupSaved == true)
                    {
                        // Get the DataTable from ViewState.

                        DataTable dtDisplayGroup = (DataTable)ViewState["dtDisplayGroup"];

                        //Find the row in DateTable.
                        DataRow drDisplayGroup = dtDisplayGroup.Rows.Find(Session["selectedDisplayGroupID"]);

                        // Retrieve edited values and updating respective items.
                        drDisplayGroup["GroupName"] = txtDisplayGroupName.Text;
                        drDisplayGroup["DisplayType"] = txtDisplayGroupDisplayType.Text;
                        drDisplayGroup["DisplayImage"] = txtDisplayGroupDisplayImage.Text;
                        drDisplayGroup["DisplayWidth"] = txtDisplayGroupDisplayWidth.Text;

                        drDisplayGroup["DisplayHeight"] = txtDisplayGroupDisplayHeight.Text;
                        drDisplayGroup["Screen"] = txtDisplayGroupScreen.Text;
                        drDisplayGroup["PanelNo"] = txtDisplayGroupPanelNo.Text;
                        drDisplayGroup["PanelPos"] = txtDisplayGroupPanelPos.Text;

                        drDisplayGroup["ExtraData1"] = txtDisplayGroupExtraData1.Text;
                        drDisplayGroup["ExtraData2"] = txtDisplayGroupExtraData2.Text;
                        drDisplayGroup["ExtraValue1"] = txtDisplayGroupExtraValue1.Text;
                        drDisplayGroup["ExtraValue2"] = txtDisplayGroupExtraValue2.Text;

                        // Exit edit mode.
                        gvDisplayGroups.EditIndex = -1;

                        // Rebind the GridView control to show data after updating.
                        BindGridView("dtDisplayGroup", gvDisplayGroups);

                    }


                }
                else if ((LiveMonitoring.IRemoteLib.Operations)Session["operation"] == LiveMonitoring.IRemoteLib.Operations.@add)
                {
                    int displayGroupID = 0;
                    LiveMonitoring.IRemoteLib.DisplayGroup displayGroup = new LiveMonitoring.IRemoteLib.DisplayGroup();

                    displayGroup.DisplayID = Convert.ToInt32(Session["selectedDisplayID"]);
                    displayGroup.GroupName = txtDisplayGroupName.Text;
                    displayGroup.DisplayType = Convert.ToInt32(txtDisplayGroupDisplayType.Text);
                    displayGroup.DisplayImage = txtDisplayGroupDisplayImage.Text;
                    displayGroup.DisplayWidth = Convert.ToInt32(txtDisplayGroupDisplayWidth.Text);

                    displayGroup.DisplayHeight = Convert.ToInt32(txtDisplayGroupDisplayHeight.Text);
                    displayGroup.Screen = Convert.ToInt32(txtDisplayGroupScreen.Text);
                    displayGroup.PanelNo = Convert.ToInt32(txtDisplayGroupPanelNo.Text);
                    displayGroup.PanelPos = Convert.ToInt32(txtDisplayGroupPanelPos.Text);

                    displayGroup.ExtraData1 = txtDisplayGroupExtraData1.Text;
                    displayGroup.ExtraData2 = txtDisplayGroupExtraData2.Text;
                    displayGroup.ExtraValue1 = Convert.ToInt32(txtDisplayGroupExtraValue1.Text);
                    displayGroup.ExtraValue2 = Convert.ToInt32(txtDisplayGroupExtraValue2.Text);


                    try
                    {
                        displayGroupID = remoteProc.LiveMonServer.AddNewDisplayGroup(displayGroup);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (displayGroupID != -99 | displayGroupID != 0)
                    {
                        successMessage.Visible = true;
                        lblSucces.Text = "Display added succesfully.";

                        DataTable dt = (DataTable)ViewState["dtDisplayGroup"];
                        DataRow dr = dt.NewRow();

                        dr["ID"] = displayGroupID;
                        dr["GroupName"] = txtDisplayGroupName.Text;
                        dr["DisplayType"] = txtDisplayGroupDisplayType.Text;
                        dr["DisplayImage"] = txtDisplayGroupDisplayImage.Text;
                        dr["DisplayWidth"] = txtDisplayGroupDisplayWidth.Text;

                        dr["DisplayHeight"] = txtDisplayGroupDisplayHeight.Text;
                        dr["Screen"] = txtDisplayGroupScreen.Text;
                        dr["PanelNo"] = txtDisplayGroupPanelNo.Text;
                        dr["PanelPos"] = txtDisplayGroupPanelPos.Text;

                        dr["ExtraData1"] = txtDisplayGroupExtraData1.Text;
                        dr["ExtraData2"] = txtDisplayGroupExtraData2.Text;
                        dr["ExtraValue1"] = txtDisplayGroupExtraValue1.Text;
                        dr["ExtraValue2"] = txtDisplayGroupExtraValue2.Text;

                        dt.Rows.InsertAt(dr, 0);

                        BindGridView("dtDisplayGroup", gvDisplayGroups);

                    }
                    else
                    {
                        errorMessage.Visible = true;
                        lblError.Text = "Add display failed.";

                    }
                }


            }

            // Empty the TextBox controls.
            txtDisplayGroupName.Text = "";
            txtDisplayGroupDisplayType.Text = "";
            txtDisplayGroupDisplayImage.Text = "";

            txtDisplayGroupDisplayWidth.Text = "";
            txtDisplayGroupDisplayHeight.Text = "";
            txtDisplayGroupScreen.Text = "";

            txtDisplayGroupPanelNo.Text = "";
            txtDisplayGroupPanelPos.Text = "";
            txtDisplayGroupExtraData1.Text = "";

            txtDisplayGroupExtraData2.Text = "";
            txtDisplayGroupExtraValue1.Text = "";
            txtDisplayGroupExtraValue2.Text = "";

            // Show the Add button and hiding the Add panel.

            lbtnDisplayGroupEdit.Visible = true;
            pnlDisplayGroupEdit.Visible = false;
        }
        protected void lbtnDisplayGroupsLinksCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.

            //txtDisplayGroupsDisplayID.Text = ""
            //txtDisplayGroupsGroupID.Text = ""
            txtDisplayGroupsScreen.Text = "";
            txtDisplayGroupsPanelNo.Text = "";
            txtDisplayGroupsPanelPos.Text = "";


            if (gvDisplayGroupsLinks.SelectedIndex != -1)
            {
                gvDisplayGroupsLinks.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlDisplayGroupsLinksEdit.Visible = false;

            lbtnDisplayGroupsLinksEdit.Visible = false;
        }


        protected void lbtnDisplaySensorLinkCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.

            //txtDisplayGroupID.Text = ""
            txtSensorID.Text = "";
            txtDisplayOrder.Text = "";

            txtExtraData1.Text = "";
            txtExtraData2.Text = "";
            txtExtraValue1.Text = "";

            txtExtraValue2.Text = "";


            if (gvDisplaySensorLink.SelectedIndex != -1)
            {
                gvDisplaySensorLink.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlDisplaySensorLinkEdit.Visible = false;

            lbtnDisplaySensorLinkEdit.Visible = false;
        }


        protected void lbtnDisplayGroupCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.

            txtDisplayGroupName.Text = "";
            txtDisplayGroupDisplayType.Text = "";
            txtDisplayGroupDisplayImage.Text = "";

            txtDisplayGroupDisplayWidth.Text = "";
            txtDisplayGroupDisplayHeight.Text = "";
            txtDisplayGroupScreen.Text = "";

            txtDisplayGroupPanelNo.Text = "";
            txtDisplayGroupPanelPos.Text = "";
            txtDisplayGroupExtraData1.Text = "";

            txtDisplayGroupExtraData2.Text = "";
            txtDisplayGroupExtraValue1.Text = "";
            txtDisplayGroupExtraValue2.Text = "";


            if (gvDisplayGroups.SelectedIndex != -1)
            {
                gvDisplayGroups.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlDisplayGroupEdit.Visible = false;
            lbtnDisplayGroupEdit.Visible = false;

            pDisplayGroups.Visible = false;
        }


        protected void lbtnDisplayCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.

            txtDisplayName.Text = "";
            txtDisplayType.Text = "";

            txtExtraData.Text = "";
            txtExtraValue.Text = "";

            txtDefaultOrderByColumn.Text = "";

            if (gvDisplayNames.SelectedIndex != -1)
            {
                gvDisplayNames.SelectedIndex = -1;
            }

            // Show the Add button and hiding the Add panel.

            pnlEditDisplay.Visible = false;

            lbtnEditDisplay.Visible = false;
            pDetails.Visible = false;
            pDisplayGroups.Visible = false;
        }




        protected void TabDisplaySensorLink_Click(object sender, EventArgs e)
        {
            TabDisplaySensorLink.CssClass = "Clicked";
            TabDisplayGroupsLinks.CssClass = "Initial";

            MainView.ActiveViewIndex = 0;

        }

        protected void TabDisplayGroupsLinks_Click(object sender, EventArgs e)
        {
            TabDisplaySensorLink.CssClass = "Initial";
            TabDisplayGroupsLinks.CssClass = "Clicked";

            MainView.ActiveViewIndex = 1;


        }


        // Protected Sub TabVoltageSurcharge_Click(sender As Object, e As EventArgs)
        // TabActiveEnergy.CssClass = "Initial"
        // TabNetworkCharge.CssClass = "Initial"
        // TabVoltageSurcharge.CssClass = "Clicked"

        // MainView.ActiveViewIndex = 2
        // End Sub
        protected void CheckSecurity()
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
        }
        public DisplaysMan()
        {
            Load += Page_Load;
        }
    }







}
