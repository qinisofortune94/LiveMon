<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientInfo.aspx.cs" Inherits="website2016V2.ClientInfo" %>
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



    <h3>Client Information</h3>
   

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Client Info"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <table style="width: 536px">
                            <tr><td></td><td></td><td>
                               </td></tr>
                                <tr>
                                    <td style="width: 149px">
                                        <strong>Remote IP</strong></td>
                                    <td>
                                        <strong>Remote Host</strong></td>
                                    <td>
                                        <strong>Remote User</strong></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                        <asp:Label ID="Label1" runat="server" Text="Label" Width="144px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Label" Width="216px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Label" Width="176px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                        <strong>Request Method</strong></td>
                                    <td>
                                        <strong>Script Name</strong></td>
                                    <td>
                                        <strong>Server Name</strong></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                        <asp:Label ID="Label4" runat="server" Text="Label" Width="144px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Label" Width="216px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Label" Width="176px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                        <strong>Server Port</strong></td>
                                    <td>
                                        <strong>Server Protocol</strong></td>
                                    <td>
                                        <strong>Server Software</strong></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                        <asp:Label ID="Label7" runat="server" Text="Label" Width="136px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Label" Width="216px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Label" Width="176px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                        <strong>Browser</strong></td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="Label10" runat="server" Text="Label" Width="544px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 149px">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
