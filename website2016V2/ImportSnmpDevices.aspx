<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportSnmpDevices.aspx.cs" Inherits="website2016V2.ImportSnmpDevices" %>

<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <h3>Import SNMP Devices</h3>
         <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSuccess" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    

     <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTree">
                <h4 class="panel-title">
                    <a id="Tree" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseTree"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Import SNMP"></asp:Label>
                    </strong>
                    </a>
                </h4>

            </div>
             <div>
                <cc1:Exceldoc ID="Exceldoc2" runat="server"></cc1:Exceldoc>
                <br />
               
            </div>

             <div id="collapseFour" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">Number of rows</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TextBox2" runat="server" PlaceHolder="Please Number of Rows" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                   

                    </div>
                    <div class="col-md-2"> Select File</div>
                        
                     <div class="col-md-4" id="tbrImportRow" runat="server" >
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
                        <asp:Button ID="BtnLoad" runat="server" Text="Load" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnImportSubmit" runat="server" Text="Submit" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>


                  
            </div>
            </div>
         </div>

    <div id="accordion1" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo1">
                <h4 class="panel-title">
                    <a id="Second1" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Display"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gdvBulk" runat="server" CssClass="gvdatatable table table-striped table-bordered" AllowPaging="True" AutoGenerateEditButton="false" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                              <%--  <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />--%>
                                <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                            </Columns>
                            
                        </asp:GridView>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
