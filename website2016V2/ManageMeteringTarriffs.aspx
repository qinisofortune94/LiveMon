<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageMeteringTarriffs.aspx.cs" Inherits="website2016V2.ManageMeteringTarriffs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap.min.js"></script>
    <script src="DataTable/dataTables.buttons.min.js"></script>
    <script src="DataTable/jszip.min.js"></script>
    <script src="DataTable/pdfmake.min.js"></script>
    <script src="DataTable/vfs_fonts.js"></script>
    <script src="DataTable/buttons.html5.min.js"></script>

    <link href="DataTable/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            $('.gvdatatable').dataTable({
                dom: 'Bfrtip',
                buttons: [
            'excelHtml5',
            'pdfHtml5'
                ],

                "order": [[2, "desc"]],
                buttons: [
                     {
                         extend: 'pdf',
                         text: 'PDF',
                         exportOptions: {
                             columns: [2, 3, 4, 5],
                         }
                     },
                      {
                          extend: 'excel',
                          text: 'Excel',
                          exportOptions: {
                              columns: [2, 3, 4, 5],
                          }
                      }

                ],
                columnDefs: [
          {
              "targets": [0],
              //"visible": false,
              "orderable": false,
              "searchable": false

          },
          {
              "targets": [1],
              "orderable": false,
              "searchable": false
          }]

            });
        });
    </script>

    <h3>Manage Metering Tarrif</h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <div class="row">
                    <div class="col-md-2">
                        <h4 class="panel-title">
                            <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                                <asp:Label ID="Label2" runat="server" Text="Edit/Delete"></asp:Label>
                            </strong>
                            </a>
                        </h4>
                    </div>
                </div>
            
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvTarrif" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Select" CommandName="SelectItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                <asp:BoundField DataField="TarriffName" HeaderText="Tarriff Name" SortExpression="TarriffName"/>
                               

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <div class="row">
                    <div class="col-md-2">
                        <h4 class="panel-title">
                            <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                                <asp:Label ID="Label3" runat="server" Text="Metering Manage Terrif"></asp:Label>
                            </strong>
                            </a>
                        </h4>
                    </div>
                </div>
            
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="lbtnAdd" runat="server" Text="Add" Width="250px" Height="40px"  class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnAdd_Click" />
                            </div>
                            <div class="col-md-2">
                            </div>

                            <div class="col-md-4">
                                <asp:Button ID="lbtnEdit" runat="server" Text="Edit" Width="250px" Height="40px" visible="false" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnEdit_Click" />
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnlEdit" runat="server" Visible="False">
         
                     <div class="row">
                            <div class="col-md-2">Tarrif name:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTarriffName" runat="server" PlaceHolder="CostRperMaxkVA" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                     </div>
            <br />
            <br />


                 <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                          
                      <asp:Button ID="lbtnSubmit" runat="server" Text="Submit" Width="250px" Height="40px"  class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnSubmit_Click" />

                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                      <asp:Button ID="lbtnCancel" runat="server" Text="Cancel" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnCancel_Click" />

                       
                    </div>
                </div>
        </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <div id="accordionPeriod" runat="server" visible="false" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwot">
                <h4 class="panel-title">
                    <a id="Secondr" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Charge Period"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwod" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gridPeriod" AllowPaging="true" OnPageIndexChanging="gridPeriod_PageIndexChanging" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false">

                            <Columns>
                                <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" SortExpression="ChargeName"/>
                                <asp:BoundField DataField="StartTime" HeaderText="Start Time" SortExpression="StartTime"/>
                                <asp:BoundField DataField="EndTime" HeaderText="End Time" />
                                <asp:BoundField DataField="Day" HeaderText="Day" />
                                <asp:BoundField DataField="ChargeTypeName" HeaderText="Charge Type Name" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    
  <div id="accordionTT" runat="server" visible="false" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Manage Terrif"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
    <asp:Panel ID="pDetails" runat="server" Visible="false">

            <table width="100%" align="center" >
        
            <tr>
                <td>
                    <asp:Button Text="Active Energy" BorderStyle="None" ID="TabActiveEnergy"  Width="250px" Height="40px" CssClass="Initial" runat="server" class="btn btn-success form-control"
               BorderColor="#0099FF" OnClick="TabChargeType_Click" />
                    <asp:Button Text="Network charges" BorderStyle="None" ID="TabNetworkCharge" Width="250px" Height="40px" CssClass="Initial" runat="server" class="btn btn-success form-control"
                       BorderColor="#0099FF"  OnClick="TabNetworkCharge_Click" />
                    <asp:Button Text="Voltage Surcharge" BorderStyle="None" ID="TabVoltageSurcharge" Width="250px" Height="40px" CssClass="Initial" runat="server"  class="btn btn-success form-control"
                        BorderColor="#0099FF" OnClick="TabVoltageSurcharge_Click" />
                    <asp:MultiView ID="MainView" runat="server">
                        <asp:View ID="vActiveEnergy" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td class="auto-style1">
                                            <%-- <asp:GridView  width="100%" ID="gvActiveEnergy" runat="server" AutoGenerateColumns="False" BackColor="White" 
                                                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                                 onselectedindexchanged="gvActiveEnergy_SelectedIndexChanged" 
                                                    onpageindexchanging="gvTarrif_PageIndexChanging" 
                                                    onrowcancelingedit="gvTarrif_RowCancelingEdit" 
                                                    onrowdatabound="gvTarrif_RowDataBound" onrowdeleting="gvActiveEnergy_RowDeleting" 
                                                    onrowediting="gvTarrif_RowEditing" onrowupdating="gvTarrif_RowUpdating" 
                                                    onsorting="gvTarrif_Sorting">
                                                <RowStyle BackColor="White" ForeColor="#003399" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostcPerKWh" HeaderText="CostcPerKWh" ReadOnly="True" SortExpression="ID" />

                                                    </Columns>
                                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                                </asp:GridView>
                                            --%>

                                         <asp:GridView ID="gvActiveEnergy" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id"  
             
                                                      onselectedindexchanged="gvActiveEnergy_SelectedIndexChanged" 
                                                    onpageindexchanging="gvTarrif_PageIndexChanging" 
                                                    onrowcancelingedit="gvTarrif_RowCancelingEdit" 
                                                    onrowdatabound="gvTarrif_RowDataBound" onrowdeleting="gvActiveEnergy_RowDeleting" 
                                                    onrowediting="gvTarrif_RowEditing" onrowupdating="gvTarrif_RowUpdating" 
                                                    onsorting="gvTarrif_Sorting"> 

                            <Columns>
                               
                                                     <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostcPerKWh" HeaderText="CostcPerKWh" ReadOnly="True" SortExpression="ID" />

                            </Columns>
                        </asp:GridView>
                                                 <br />


                                                 <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="lbtnActiveEnergyAdd" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnActiveEnergyAdd_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="lbtnActiveEnergyEdit" runat="server" Text="Edit" Width="250px" Height="40px" Visible="false" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnActiveEnergyEdit_Click" />
                    </div>
                </div>        


                                               <%-- <asp:LinkButton ID="lbtnActiveEnergyAdd" runat="server" onclick="lbtnActiveEnergyAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnActiveEnergyEdit" runat="server" Visible="false" onclick="lbtnActiveEnergyEdit_Click">Edit</asp:LinkButton>
                                    --%>
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlActiveEnergyEdit" runat="server" Visible="False">
                                                  
                                                   <%-- <asp:TextBox ID="txtActiveEnergyChargeName" runat="server" Columns="50" ></asp:TextBox>
                                                    <asp:TextBox ID="txtActiveEnergyCostcPerKWh" runat="server" Columns="50" ></asp:TextBox>--%>

                                                     <div class="row">
                                                     <div class="col-md-2">Charge Name:</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="txtActiveEnergyChargeName" runat="server" PlaceHolder="CostRperMaxkVA"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">Energy CostcPerKW:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="txtActiveEnergyCostcPerKWh" runat="server" PlaceHolder="FixedCost"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>
                                                    <br />
                                                    <br />


                                                            <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnActiveEnergySubmit" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnActiveEnergySubmit_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="btnActiveEnergyCancel_Click" runat="server" Text="Clear" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnActiveEnergyCancel_Click" />
                    </div>
                </div>

                                                 <%--   <asp:LinkButton ID="LinkButton2"  runat="server" onclick="lbtnActiveEnergySubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton3" runat="server" onclick="lbtnActiveEnergyCancel_Click">Cancel</asp:LinkButton>--%>
            
                                                </asp:Panel>

                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vNetworkCharges" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>
                                             <%--<asp:GridView  width="100%" ID="gvNetworkCharges" runat="server" AutoGenerateColumns="False" BackColor="White" 
                                                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                                  onselectedindexchanged="gvNetworkCharges_SelectedIndexChanged" 
                                                    onpageindexchanging="gvTarrif_PageIndexChanging" 
                                                    onrowcancelingedit="gvTarrif_RowCancelingEdit" 
                                                    onrowdatabound="gvTarrif_RowDataBound" onrowdeleting="gvNetworkCharges_RowDeleting" 
                                                    onrowediting="gvTarrif_RowEditing" onrowupdating="gvTarrif_RowUpdating" 
                                                    onsorting="gvTarrif_Sorting">
                                                <RowStyle BackColor="White" ForeColor="#003399" />
                                                    <Columns>

                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperkWh" HeaderText="CostRperkWh" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperday" HeaderText="CostR/day" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperkVA" HeaderText="CostR/kVA" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperMaxkVA" HeaderText="CostR/MaxkVA" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="FixedCost" HeaderText="FixedCost" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="MaximumDemand" HeaderText="MaxDemand" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PenaltyCharge" HeaderText="PenaltyCharge" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="Percentage" HeaderText="Percentage" ReadOnly="True" SortExpression="ID" />
                                                    
                                                    </Columns>
                                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                                </asp:GridView>--%>


                                             <asp:GridView ID="gvNetworkCharges" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id"  
                                                        onselectedindexchanged="gvNetworkCharges_SelectedIndexChanged" 
                                                    onpageindexchanging="gvTarrif_PageIndexChanging" 
                                                    onrowcancelingedit="gvTarrif_RowCancelingEdit" 
                                                    onrowdatabound="gvTarrif_RowDataBound" onrowdeleting="gvNetworkCharges_RowDeleting" 
                                                    onrowediting="gvTarrif_RowEditing" onrowupdating="gvTarrif_RowUpdating" 
                                                    onsorting="gvTarrif_Sorting"> 

                            <Columns>
                               
                                                   <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ChargeName" HeaderText="ChargeName" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperkWh" HeaderText="CostRperkWh" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperday" HeaderText="CostR/day" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperkVA" HeaderText="CostR/kVA" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="CostRperMaxkVA" HeaderText="CostR/MaxkVA" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="FixedCost" HeaderText="FixedCost" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="MaximumDemand" HeaderText="MaxDemand" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PenaltyCharge" HeaderText="PenaltyCharge" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="Percentage" HeaderText="Percentage" ReadOnly="True" SortExpression="ID" />

                            </Columns>
                        </asp:GridView>
                                            
                                                <br />
                                                                                    <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="lbtnNetworkChargesAdd" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnNetworkChargesAdd_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="lbtnNetworkChargesEdit" runat="server" Text="Clear" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnNetworkChargesEdit_Click" />
                    </div>
                </div>
                                              <%--  <asp:LinkButton ID="lbtnNetworkChargesAdd" runat="server" onclick="lbtnNetworkChargesAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnNetworkChargesEdit" runat="server" Visible="false" onclick="lbtnNetworkChargesEdit_Click">Edit</asp:LinkButton>
                                    --%>
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlNetworkChargesEdit" runat="server" Visible="False">

                                          <div class="row">
                                                     <div class="col-md-2">Charge Name:</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="txtNetworkChargeName" runat="server" PlaceHolder="Charge Name:"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">CostRperkWh:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="txtNetworkCostRperkWh" runat="server" PlaceHolder="CostRperkWh"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>


                                           <div class="row">
                                                     <div class="col-md-2">CostRperday:</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="txtNetworkCostRperday" runat="server" PlaceHolder="CostRperday"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">CostRperkVA:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="txtNetworkCostRperkVA" runat="server"  PlaceHolder="CostRperkVA"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>

                                           <div class="row">
                                                     <div class="col-md-2">CostRperday:</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="TextBox1" runat="server" PlaceHolder="CostRperday"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">CostRperkVA:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="TextBox2" runat="server"  PlaceHolder="CostRperkVA"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>


                                       <div class="row">
                                                     <div class="col-md-2">CostRperMaxkVA:</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="txtNetworkCostRperMaxkVA" runat="server" PlaceHolder="CostRperMaxkVA"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">FixedCost:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="txtNetworkFixedCost" runat="server" PlaceHolder="FixedCost"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>

                                             <div class="row">
                                                     <div class="col-md-2">MaximumDemand::</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="txtNetworkMaximumDemand" runat="server" PlaceHolder="MaximumDemand"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">PenaltyCharge:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="txtNetworkPenaltyCharge" runat="server"  PlaceHolder="PenaltyCharge"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>

                                                      <div class="row">
                                                <div class="col-md-2">Percentage:</div>
                                                <div class="col-md-4">
                                         <asp:TextBox ID="txtNetworkPercentage" runat="server" PlaceHolder="Percentage:" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                    </div>
                                                <div class="col-md-2"></div>
                                                <div class="col-md-4">
                                                    </div>
                                                    </div>

                                                   <%--Charge Name:
                                                    <asp:TextBox ID="txtNetworkChargeName" runat="server" Columns="50" ></asp:TextBox>
                                                    CostRperkWh:
                                                    <asp:TextBox ID="txtNetworkCostRperkWh" runat="server" Columns="50" ></asp:TextBox>--%>
                                                  <%--  CostRperday:
                                                    <asp:TextBox ID="txtNetworkCostRperday" runat="server" Columns="50" ></asp:TextBox>
                                                    CostRperkVA:
                                                    <asp:TextBox ID="txtNetworkCostRperkVA" runat="server" Columns="50" ></asp:TextBox>--%>
                                                   <%-- CostRperMaxkVA:
                                                    <asp:TextBox ID="txtNetworkCostRperMaxkVA" runat="server" Columns="50" ></asp:TextBox>
                                                    FixedCost:
                                                    <asp:TextBox ID="txtNetworkFixedCost" runat="server" Columns="50" ></asp:TextBox>--%>
                                                  <%--  MaximumDemand:
                                                    <asp:TextBox ID="txtNetworkMaximumDemand" runat="server" Columns="50" ></asp:TextBox>
                                                    PenaltyCharge:
                                                    <asp:TextBox ID="txtNetworkPenaltyCharge" runat="server" Columns="50" ></asp:TextBox>--%>
                                                 <%--   Percentage:
                                                    <asp:TextBox ID="txtNetworkPercentage" runat="server" Columns="50" ></asp:TextBox>--%>
                                                    <br />
                                                    <br />


                                                      <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSub" runat="server" Text="Submit" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnNetworkChargesSubmit_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnNetworkChargesCancel_Click" />
                    </div>
                </div>
                                                 <%--   <asp:LinkButton ID="LinkButton4"  runat="server" onclick="lbtnNetworkChargesSubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton5" runat="server" onclick="lbtnNetworkChargesCancel_Click">Cancel</asp:LinkButton>--%>
            
                                                </asp:Panel>

                                                <br />
                                                <br />
                                    
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vVoltage" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>
                                        <br />
                                        <%--<asp:GridView ID="gvVoltage" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC"
                                             BorderStyle="None" BorderWidth="1px" CellPadding="4" onpageindexchanging="gvTarrif_PageIndexChanging" 
                                             onselectedindexchanged="gvVoltage_SelectedIndexChanged" 
                                            onrowcancelingedit="gvTarrif_RowCancelingEdit" onrowdatabound="gvTarrif_RowDataBound" 
                                            onrowdeleting="gvVoltage_RowDeleting" onrowediting="gvTarrif_RowEditing" onrowupdating="gvTarrif_RowUpdating" 
                                            onsorting="gvTarrif_Sorting" width="100%">
                                            <RowStyle BackColor="White" ForeColor="#003399" />
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" />
                                                <asp:CommandField ShowDeleteButton="True" />
                                                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                <asp:BoundField DataField="TariffID" HeaderText="TariffID" ReadOnly="True" SortExpression="ID" />
                                                <asp:BoundField DataField="Voltage" HeaderText="Voltage" ReadOnly="True" SortExpression="ID" />
                                                <asp:BoundField DataField="SurchargePercentage" HeaderText="SurchargePercentage" ReadOnly="True" SortExpression="ID" />
                                            
                                            </Columns>
                                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                        </asp:GridView>--%>


                                         <asp:GridView ID="gvVoltage" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id"  
                                                        onselectedindexchanged="gvNetworkCharges_SelectedIndexChanged" 
                                                    onpageindexchanging="gvTarrif_PageIndexChanging" 
                                                    onrowcancelingedit="gvTarrif_RowCancelingEdit" 
                                                    onrowdatabound="gvTarrif_RowDataBound" onrowdeleting="gvNetworkCharges_RowDeleting" 
                                                    onrowediting="gvTarrif_RowEditing" onrowupdating="gvTarrif_RowUpdating" 
                                                    onsorting="gvTarrif_Sorting"> 

                            <Columns>
                               
                                                   <asp:CommandField ShowSelectButton="True" />
                                                <asp:CommandField ShowDeleteButton="True" />
                                                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                <asp:BoundField DataField="TariffID" HeaderText="TariffID" ReadOnly="True" SortExpression="ID" />
                                                <asp:BoundField DataField="Voltage" HeaderText="Voltage" ReadOnly="True" SortExpression="ID" />
                                                <asp:BoundField DataField="SurchargePercentage" HeaderText="SurchargePercentage" ReadOnly="True" SortExpression="ID" />

                            </Columns>
                        </asp:GridView>


                                       
                                          <br />


                                                                                      <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="lbtnVoltageAdd" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnVoltageAdd_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="lbtnVoltageEdit" runat="server" Text="Edit" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnVoltageEdit_Click" />
                    </div>
                </div>
                                                
                                              <%--  <asp:LinkButton ID="lbtnVoltageAdd" runat="server" onclick="lbtnVoltageAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnVoltageEdit" runat="server" Visible="false" onclick="lbtnVoltageEdit_Click">Edit</asp:LinkButton>--%>
                                    
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlVoltageEdit" runat="server" Visible="False">

                                                      <div class="row">
                                                     <div class="col-md-2">Voltage:</div>
                                                    <div class="col-md-4">
                                                <asp:TextBox ID="txtVoltageVoltage" runat="server" PlaceHolder="MaximumDemand" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                 </div>
                                             <div class="col-md-2">Surcharge Percentage:</div>
                                         <div class="col-md-4">
                                     <asp:TextBox ID="txtVoltageSurchargePercentage" runat="server"  PlaceHolder="PenaltyCharge" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                        </div>
                                            </div>


                                                 <%--   Voltage:
                                                    <asp:TextBox ID="txtVoltageVoltage" runat="server" Columns="50" ></asp:TextBox>
                                                    Surcharge Percentage:
                                                    <asp:TextBox ID="txtVoltageSurchargePercentage" runat="server" Columns="50" ></asp:TextBox>--%>


                                                    
                                                                                      <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="LinkButton6" runat="server" Text="Submit" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnVoltageSubmit_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="LinkButton7" runat="server" Text="Cancel" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnVoltagesCancel_Click" />
                    </div>
                </div>


                                                
                                                    <br />
                                                    <br />
                                                  <%--  <asp:LinkButton ID="LinkButton6"  runat="server" onclick="lbtnVoltageSubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton7" runat="server" onclick="lbtnVoltagesCancel_Click">Cancel</asp:LinkButton>
            --%>
                                                </asp:Panel>

                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>

    </asp:Panel>
            </div>
      </div>
    <br />
</asp:Content>
