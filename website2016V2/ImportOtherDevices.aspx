<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportOtherDevices.aspx.cs" Inherits="website2016V2.ImportOtherDevices" %>

<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <h3>Import Other Device </h3>
    <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSuccess" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>

    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">
                <asp:Label ID="lblAdd" runat="server" Text="Import"></asp:Label>
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                <div class="col-md-2"></div>
                <div class="col-md-4">
                    <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                </div>
                <div class="col-md-2"></div>
                <div class="col-md-4">
                </div>
            </div>
            <div class="row">                
                <div class="col-md-6">
                    <div class="form-group">
                        Import
                        <div class="input-group">
                            <div class="custom-file" style="border: 1px solid #ccc; padding-left: 5px!important"> 
                                <asp:FileUpload ID="fuFile" runat="server"></asp:FileUpload>
                            </div>
                            <asp:Button ID="btnLoad" runat="server" Text="Load" class="btn bg-gray" OnClick="btnLoad_Click" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        Number of rows
                        <asp:TextBox ID="txtNumberORows" runat="server" PlaceHolder="Please enter number of rows" required="true" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>                
            </div><br />
            <div class="row">
                <div class="col-md-12 text-center"><asp:Button ID="btnsubmit" runat="server" Text="Submit" Width="120px" Height="40px" class="btn bg-gray" OnClick="btnSubmit_Click" /></div>                
            </div>
        </div>
    </div>
    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title"><asp:Label ID="lblAddb" runat="server" Text="Information"></asp:Label></h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gdvBulk" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Type">
                            <Columns>

                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="IPAdress" HeaderText="IPAdress" Visible="true" />
                                <asp:BoundField DataField="Port" HeaderText="Port" Visible="true" />
                                <asp:BoundField DataField="SerialPort" HeaderText="SerialPort" Visible="true" />
                                <asp:BoundField DataField="SerialSettings" HeaderText="SerialSettings" Visible="true" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData1" Visible="true" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData" Visible="true" />
                                <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="true" />
                                <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="true" />
                                <asp:BoundField DataField="ExtraData4" HeaderText="ExtraData4" Visible="true" />
                                <asp:BoundField DataField="ExtraData5" HeaderText="ExtraData5" Visible="true" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                                <%-- <asp:BoundField DataField="LastReadDT" HeaderText="LastReadDT" Visible="true" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />--%>
                            </Columns>
                        </asp:GridView>

                        <asp:GridView ID="gdvBulk1" runat="server" AllowPaging="true" AutoGenerateEditButton="false" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Width="95%" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="IPAdress" HeaderText="IPAdress" Visible="true" />
                                <asp:BoundField DataField="Port" HeaderText="Port" Visible="true" />
                                <asp:BoundField DataField="SerialPort" HeaderText="SerialPort" Visible="true" />
                                <asp:BoundField DataField="SerialSettings" HeaderText="SerialSettings" Visible="true" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData1" Visible="true" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData" Visible="false" />
                                <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="false" />
                                <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="false" />
                                <asp:BoundField DataField="ExtraData4" HeaderText="ExtraData4" Visible="false" />
                                <asp:BoundField DataField="ExtraData5" HeaderText="ExtraData5" Visible="false" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                                <%-- <asp:BoundField DataField="LastReadDT" HeaderText="LastReadDT" Visible="true" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />--%>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                    </div>
                </div>
        </div>
    </div>    
</asp:Content>
