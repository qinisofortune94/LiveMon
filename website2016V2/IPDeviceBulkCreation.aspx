<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IPDeviceBulkCreation.aspx.cs" Inherits="website2016V2.IPDeviceBulkCreation" %>

<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
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
            <h3 class="card-title">Display
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
                <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gdvIPDevicesTemplates" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="TemplateName">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="templateName" HeaderText="Template Name" />
                            <asp:BoundField DataField="Caption" HeaderText="Caption" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">Add Bulk IP Devices
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
                <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        Number of IP Devices to create
                        <asp:TextBox ID="txtNumberIPDevices" runat="server" PlaceHolder="Please enter  number of devices" required="true" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <asp:CheckBox ID="cboShowImport" Visible="false" OnCheckedChanged="cboShowImport_CheckedChanged" runat="server" Text="Show Import?" AutoPostBack="True" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">

                    </div>
                </div>
            </div>
            <div class="row" id="divImportRow" runat="server" visible="true">
                 <div class="col-md-4">
                    <div class="form-group">
                        Import File
                        <div class="input-group">
                             <div class="custom-file">
                                 <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                    <br />
                    <asp:FileUpload ID="FileUpload1" runat="server" PlaceHolder="Please enter  name" CssClass="custom-file-input"></asp:FileUpload>
                    <label class="custom-file-label" for="FileUpload1">Choose file</label>
                             </div>
                         </div>
                    </div>
                </div>
                <div class="col-md-4">
                    
                </div>
                 <div class="col-md-4">
                    <div class="form-group">

                    </div>
                </div>
            </div>           
               
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="Create" Height="40px" Width="250px" class="btn bg-gray form-control" />
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit" Height="40px" Width="250px" class="btn bg-gray form-control" />
                </div>
                <div class="col-md-2">
                </div>
            </div>
        </div>
    </div>
    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">Display
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
                <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gdvBulk" AllowPaging="True" OnPageIndexChanging="gdvBulk_PageIndexChanging" OnRowEditing="gdvBulk_RowEditing" OnRowCancelingEdit="gdvBulk_RowCancelingEdit" OnRowUpdating="gdvBulk_RowUpdating" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateEditButton="True" AutoGenerateColumns="false">

                        <Columns>
                            <asp:BoundField DataField="Type" HeaderText="Type" />
                            <asp:BoundField DataField="IPAdress" HeaderText="IPAdress" Visible="true" />
                            <asp:BoundField DataField="Port" HeaderText="Port" Visible="true" />
                            <asp:BoundField DataField="Data1" HeaderText="Data1" Visible="true" />
                            <asp:BoundField DataField="Data2" HeaderText="Data2" Visible="true" />
                            <asp:BoundField DataField="Data3" HeaderText="Data3" Visible="true" />
                            <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="false" />
                            <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="false" />
                            <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="false" />
                            <asp:BoundField DataField="DTLastRead" HeaderText="DTLastRead" Visible="true" />
                            <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
