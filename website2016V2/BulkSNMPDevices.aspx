<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BulkSNMPDevices.aspx.cs" Inherits="website2016V2.BulkSNMPDevices" %>
<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

     
     
    <h3>Bulk SNMP</h3>

    <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>

        <div id="accordionBulk" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTreee">
                <h4 class="panel-title">
                    <a id="Treee" data-toggle="collapse" data-parent="#accordionBulk" href="#collapseOne" aria-expanded="false" aria-controls="collapseTree"><strong>
                        <asp:Label ID="Label3" runat="server" Text="Bulk SNMP"></asp:Label>
                    </strong>
                    </a>
                </h4>

            </div>


    <asp:GridView ID="gdvSNMPTemplates" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false" DataKeyNames="TemplateName">

                            <Columns>
                                  
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="TemplateName" HeaderText="Template Name" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption" />
                           
                            </Columns>
                        </asp:GridView>
            </div>
            </div>
    

      <div id="accordionBulk" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTreee">
                <h4 class="panel-title">
                    <a id="Treee" data-toggle="collapse" data-parent="#accordionBulk" href="#collapseOne" aria-expanded="false" aria-controls="collapseTree"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Bulk SNMP"></asp:Label>
                    </strong>
                    </a>
                </h4>

            </div>
          
          
             <div id="collapseTree" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTreee" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">Number of Other Devices To Create</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtBulkDevices" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                         <asp:CheckBox Id="ChkImport" Visible="false" runat="server" Text="Show Import?" OnCheckedChanged="ChkImport_CheckedChanged"/>
                    </div>
                    <div class="col-md-2"> Select File</div>
                        
                    <div class="col-md-4" id="tbrImportRow" runat="server">
                           <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                            <asp:FileUpload ID="FileUpload1" runat="server" /> 
                        <%-- <asp:Button ID="BtnLoad" runat="server" Text="Load" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnLoad_Click" />--%>

                    </div>
                    </div>

            
                
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="BtnCreatBulk" runat="server" Text="Create" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnSubmit" runat="server" Text="Submit" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnSubmit_Click" />
                    </div>
                </div>

                 <div>
                      <asp:Label ID="lblMessage" runat="server"></asp:Label>
                 </div>
            </div>
            </div>
         </div>


    <div id="accordionBulk" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTreee">
                <h4 class="panel-title">
                    <a id="Treee" data-toggle="collapse" data-parent="#accordionBulk" href="#collapseOne" aria-expanded="false" aria-controls="collapseTree"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Bulk SNMP"></asp:Label>
                    </strong>
                    </a>
                </h4>

            </div>


            <%-- <asp:GridView ID="gdvBulk" runat="server" AllowPaging="True" AutoGenerateEditButton="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="RemoteHost" HeaderText="RemoteHost" />
                                <asp:BoundField DataField="RemotePort" HeaderText="RemotePort" Visible="true" />
                                <asp:BoundField DataField="Authentication" HeaderText="Authentication" Visible="true" />
                                <asp:BoundField DataField="Community" HeaderText="Community" Visible="true" />
                                <asp:BoundField DataField="LocalEngineId" HeaderText="LocalEngineId" Visible="true" />
                                <asp:BoundField DataField="LocalHost" HeaderText="LocalHost" Visible="true" />
                                <asp:BoundField DataField="LocalPort" HeaderText="LocalPort" Visible="true" />
                                <asp:BoundField DataField="RequestId" HeaderText="RequestId" Visible="true" />
                                <asp:BoundField DataField="SNMPVersion" HeaderText="SNMPVersion" Visible="true" />
                                <asp:BoundField DataField="Timeout" HeaderText="Timeout" Visible="true" />
                                <asp:BoundField DataField="User" HeaderText="User" Visible="true" />
                                <asp:BoundField DataField="Password" HeaderText="Password" Visible="true" />
                                <asp:BoundField DataField="Data1" HeaderText="Data1" Visible="true" />
                                <asp:BoundField DataField="Data2" HeaderText="Data2" Visible="true" />
                                <asp:BoundField DataField="Data3" HeaderText="Data3" Visible="true" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>--%>

             <asp:GridView ID="gdvBulk" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false"  >

                            <Columns>
                                <asp:BoundField DataField="RemoteHost" HeaderText="RemoteHost" visible="true"/>
                                <asp:BoundField DataField="RemotePort" HeaderText="RemotePort" Visible="true" />
                                <asp:BoundField DataField="Authentication" HeaderText="Authentication" Visible="true" />
                                <asp:BoundField DataField="Community" HeaderText="Community" Visible="true" />
                                <asp:BoundField DataField="LocalEngineId" HeaderText="LocalEngineId" Visible="true" />
                                <asp:BoundField DataField="LocalHost" HeaderText="LocalHost" Visible="true" />
                                <asp:BoundField DataField="LocalPort" HeaderText="LocalPort" Visible="true" />
                                <asp:BoundField DataField="RequestId" HeaderText="RequestId" Visible="true" />
                                <asp:BoundField DataField="SNMPVersion" HeaderText="SNMPVersion" Visible="true" />
                                <asp:BoundField DataField="Timeout" HeaderText="Timeout" Visible="true" />
                                <asp:BoundField DataField="User" HeaderText="User" Visible="true" />
                                <asp:BoundField DataField="Password" HeaderText="Password" Visible="false" />
                                <asp:BoundField DataField="Data1" HeaderText="Data1" Visible="false" />
                                <asp:BoundField DataField="Data2" HeaderText="Data2" Visible="false" />
                                <asp:BoundField DataField="Data3" HeaderText="Data3" Visible="false" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="false" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="false" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="false" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />

                            </Columns>
                        </asp:GridView>



<%--              <asp:GridView ID="gdvSNMPTemplates" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="TemplateName" HeaderText="Template Name" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption" />
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>--%>


               


              


            </div>
        </div>



</asp:Content>
