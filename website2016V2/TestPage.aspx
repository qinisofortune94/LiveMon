<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="website2016V2.TestPage" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
   
    <div>
        <asp:GridView ID="gvTarrif" 
            runat="server" 
            Width="100%"
            AutoGenerateColumns="False" 
            BackColor="White" 
            HorizontalAlign="Center" 
            BorderColor="#3366CC" 
            BorderStyle="None" 
            BorderWidth="1px" 
            CellPadding="4" 
            onpageindexchanging="gvTarrif_PageIndexChanging" 
            onselectedindexchanged="gvTarrif_SelectedIndexChanged" 
            onrowcancelingedit="gvTarrif_RowCancelingEdit" 
            onrowdatabound="gvTarrif_RowDataBound" 
            onrowdeleting="gvTarrif_RowDeleting" 
            onrowediting="gvTarrif_RowEditing" 
            onrowupdating="gvTarrif_RowUpdating" 
            onsorting="gvTarrif_Sorting">
        <RowStyle BackColor="White" ForeColor="#003399" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:CommandField ShowDeleteButton="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                    SortExpression="ID" />
                <asp:BoundField DataField="TarriffName" HeaderText="Tarriff Name" ReadOnly="True" 
                    SortExpression="ID" />
                                       
            </Columns>
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        </asp:GridView>
    
        <br /> 
        <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click">Add</asp:LinkButton>
        <asp:LinkButton ID="lbtnEdit" runat="server"  Visible="false" onclick="lbtnEdit_Click">Edit</asp:LinkButton>
       
        <br />
        <br />
        <asp:Panel ID="pnlEdit" runat="server" Visible="False">
            Tarrif name:
            <asp:TextBox ID="txtTarriffName" runat="server" Columns="50" ></asp:TextBox>
            <br />
            <br />
            <asp:LinkButton ID="lbtnSubmit" runat="server" onclick="lbtnSubmit_Click">Submit</asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lbtnCancel" runat="server" onclick="lbtnCancel_Click">Cancel</asp:LinkButton>
            
        </asp:Panel>
    </div>
  
    <asp:Panel ID="pDetails" runat="server" Visible="false">

            <table width="100%" align="center" >
        
            <tr>
                <td>
                    <asp:Button Text="Active Energy" BorderStyle="None" ID="TabActiveEnergy" CssClass="Initial" runat="server"
                        OnClick="TabChargeType_Click" />
                    <asp:Button Text="Network charges" BorderStyle="None" ID="TabNetworkCharge" CssClass="Initial" runat="server"
                        OnClick="TabNetworkCharge_Click" />
                    <asp:Button Text="Voltage Surcharge" BorderStyle="None" ID="TabVoltageSurcharge" CssClass="Initial" runat="server"
                        OnClick="TabVoltageSurcharge_Click" />
                    <asp:MultiView ID="MainView" runat="server">
                        <asp:View ID="vActiveEnergy" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td class="auto-style1">
                                             <asp:GridView  width="100%" ID="gvActiveEnergy" runat="server" AutoGenerateColumns="False" BackColor="White" 
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
                                            
                                                 <br />
                                                <asp:LinkButton ID="lbtnActiveEnergyAdd" runat="server" onclick="lbtnActiveEnergyAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnActiveEnergyEdit" runat="server" Visible="false" onclick="lbtnActiveEnergyEdit_Click">Edit</asp:LinkButton>
                                    
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlActiveEnergyEdit" runat="server" Visible="False">
                                                   Charge Name:
                                                    <asp:TextBox ID="txtActiveEnergyChargeName" runat="server" Columns="50" ></asp:TextBox>
                                                    <asp:TextBox ID="txtActiveEnergyCostcPerKWh" runat="server" Columns="50" ></asp:TextBox>
                                                    <br />
                                                    <br />
                                                    <asp:LinkButton ID="LinkButton2"  runat="server" onclick="lbtnActiveEnergySubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton3" runat="server" onclick="lbtnActiveEnergyCancel_Click">Cancel</asp:LinkButton>
            
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
                                             <asp:GridView  width="100%" ID="gvNetworkCharges" runat="server" AutoGenerateColumns="False" BackColor="White" 
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
                                                </asp:GridView>
                                            
                                                <br />
                                                <asp:LinkButton ID="lbtnNetworkChargesAdd" runat="server" onclick="lbtnNetworkChargesAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnNetworkChargesEdit" runat="server" Visible="false" onclick="lbtnNetworkChargesEdit_Click">Edit</asp:LinkButton>
                                    
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlNetworkChargesEdit" runat="server" Visible="False">
                                                   Charge Name:
                                                    <asp:TextBox ID="txtNetworkChargeName" runat="server" Columns="50" ></asp:TextBox>
                                                    CostRperkWh:
                                                    <asp:TextBox ID="txtNetworkCostRperkWh" runat="server" Columns="50" ></asp:TextBox>
                                                    CostRperday:
                                                    <asp:TextBox ID="txtNetworkCostRperday" runat="server" Columns="50" ></asp:TextBox>
                                                    CostRperkVA:
                                                    <asp:TextBox ID="txtNetworkCostRperkVA" runat="server" Columns="50" ></asp:TextBox>
                                                    CostRperMaxkVA:
                                                    <asp:TextBox ID="txtNetworkCostRperMaxkVA" runat="server" Columns="50" ></asp:TextBox>
                                                    FixedCost:
                                                    <asp:TextBox ID="txtNetworkFixedCost" runat="server" Columns="50" ></asp:TextBox>
                                                    MaximumDemand:
                                                    <asp:TextBox ID="txtNetworkMaximumDemand" runat="server" Columns="50" ></asp:TextBox>
                                                    PenaltyCharge:
                                                    <asp:TextBox ID="txtNetworkPenaltyCharge" runat="server" Columns="50" ></asp:TextBox>
                                                    Percentage:
                                                    <asp:TextBox ID="txtNetworkPercentage" runat="server" Columns="50" ></asp:TextBox>
                                                    <br />
                                                    <br />
                                                    <asp:LinkButton ID="LinkButton4"  runat="server" onclick="lbtnNetworkChargesSubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton5" runat="server" onclick="lbtnNetworkChargesCancel_Click">Cancel</asp:LinkButton>
            
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
                                        <asp:GridView ID="gvVoltage" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC"
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
                                        </asp:GridView>

                                          <br />
                                                <asp:LinkButton ID="lbtnVoltageAdd" runat="server" onclick="lbtnVoltageAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnVoltageEdit" runat="server" Visible="false" onclick="lbtnVoltageEdit_Click">Edit</asp:LinkButton>
                                    
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlVoltageEdit" runat="server" Visible="False">

                                                    Voltage:
                                                    <asp:TextBox ID="txtVoltageVoltage" runat="server" Columns="50" ></asp:TextBox>
                                                    Surcharge Percentage:
                                                    <asp:TextBox ID="txtVoltageSurchargePercentage" runat="server" Columns="50" ></asp:TextBox>
                                                
                                                    <br />
                                                    <br />
                                                    <asp:LinkButton ID="LinkButton6"  runat="server" onclick="lbtnVoltageSubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="LinkButton7" runat="server" onclick="lbtnVoltagesCancel_Click">Cancel</asp:LinkButton>
            
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
    

</asp:Content>
