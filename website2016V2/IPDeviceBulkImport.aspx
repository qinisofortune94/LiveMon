<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IPDeviceBulkImport.aspx.cs" Inherits="website2016V2.IPDeviceBulkImport" %>

<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
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

    <h3>Import IP Devices</h3>
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
            <h3 class="card-title">Import
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
                    Number of rows
                        <asp:TextBox ID="txtNumberofRows" runat="server" PlaceHolder="Please enter  number of devices" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Import File
                        <div class="input-group">
                            <div class="custom-file">
                                <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                                <br />
                                <asp:FileUpload ID="FileUpload1" runat="server" PlaceHolder="Please enter name" CssClass="custom-file-input" Width="250px" Height="34px"></asp:FileUpload>
                                <label class="custom-file-label" for="FileUpload1">Choose file</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8 text-center">
                    <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Text="Load" Width="100px" Height="40px" class="btn bg-gray" />
                    <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" Visible="false" runat="server" Text="Submit" Height="40px" class="btn bg-gray" />
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
                    <asp:GridView ID="gdvBulk" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Type">
                        <Columns>
                            <asp:BoundField DataField="Type" HeaderText="Type" />
                            <asp:BoundField DataField="IPAdress" HeaderText="IPAdress" Visible="true" />
                            <asp:BoundField DataField="Port" HeaderText="Port" Visible="true" />
                            <asp:BoundField DataField="Data1" HeaderText="Data1" Visible="true" />
                            <asp:BoundField DataField="Data2" HeaderText="Data2" Visible="true" />
                            <asp:BoundField DataField="Data3" HeaderText="Data3" Visible="true" />
                            <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
