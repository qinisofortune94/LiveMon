<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OtherDeviceBulkCreation.aspx.cs" Inherits="website2016V2.OtherDeviceBulkCreation" %>

<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <h3>Bulk Other Device Creation</h3>
    <br />
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
                <asp:Label ID="Label1" runat="server" Text="Display"></asp:Label>
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gdvOtherTemplates" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="TemplateName">
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
            <h3 class="card-title">
                <asp:Label ID="lblAdd" runat="server" Text="Add Bulk Other Devices"></asp:Label>
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        Number of Devices
                        <asp:TextBox ID="txtNumberOtherDevices" runat="server" PlaceHolder="Please enter  number of devices" required="true" CssClass="form-control" ></asp:TextBox>
                    <asp:CheckBox ID="cboShowImport" Visible="false" OnCheckedChanged="cboShowImport_CheckedChanged" runat="server" Text="Show Import?" AutoPostBack="True" />
                    </div>
                </div>
                <div class="col-md-6" id="divImportRow" runat="server" visible="true">
                    <div class="form-group">
                        Import File
                        <div class="input-group">
                            <div class="custom-file" style="border: 1px solid #ccc; padding-left: 5px!important">
                                <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                    <br />
                    <asp:FileUpload ID="FileUpload1" runat="server" PlaceHolder="Please enter  name" ></asp:FileUpload>
                            </div>                            
                        </div>
                    </div>                    
                </div>
                <br />
            </div>           
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8 text-center">
                    <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="Create" class="btn bg-gray" />
                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit" class="btn bg-gray" />
                </div>
                <div class="col-md-2">
                </div>
            </div>
        </div>
    </div>
    <%-- Display Role Part--%>
    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">
                <asp:Label ID="lblAddb" runat="server" Text="Display"></asp:Label>
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
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
                            <asp:BoundField DataField="SerialPort" HeaderText="SerialPort" Visible="true" />
                            <asp:BoundField DataField="SerialSettings" HeaderText="SerialSettings" Visible="true" />
                            <asp:BoundField DataField="LastReadDT" HeaderText="LastReadDT" Visible="true" />
                            <asp:BoundField DataField="ExtraData" HeaderText="ExtraData1" Visible="false" />
                            <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData" Visible="true" />
                            <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="false" />
                            <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="false" />
                            <asp:BoundField DataField="ExtraData4" HeaderText="ExtraData4" Visible="false" />
                            <asp:BoundField DataField="ExtraData5" HeaderText="ExtraData5" Visible="false" />
                            <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="false" />
                            <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="false" />
                            <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="false" />
                            <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
