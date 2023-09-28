<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeviceInfo.aspx.cs" Inherits="website2016V2.DeviceInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


    <%--<link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

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



    <h3>Device Information</h3>
   

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 70%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Device Info"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <table style="width: 536px">
                            <tr>
                                <td style="width: 149px">
                                    <strong>Working Set</strong></td>
                                <td>
                                    <strong>Version</strong></td>
                                <td>
                                    <strong>User Name</strong></td>
                            </tr>
                            <tr>
                                <td style="width: 149px">
                                    <asp:Label ID="Label1" runat="server" Text="Label" Width="144px"></asp:Label></td>
                                <td style="font-weight: bold">
                                    <asp:Label ID="Label2" runat="server" Text="Label" Width="216px"></asp:Label></td>
                                <td style="font-weight: bold">
                                    <asp:Label ID="Label3" runat="server" Text="Label" Width="176px"></asp:Label></td>
                            </tr>
                            <tr style="font-weight: bold">
                                <td style="width: 149px">
                                    Domain Name</td>
                                <td>
                                    <strong>Tick Count</strong></td>
                                <td>
                                    <strong>System Directory</strong></td>
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
                                    <strong>OS Version</strong></td>
                                <td>
                                    <strong>Machine Name</strong></td>
                                <td>
                                    <strong>Current Directory</strong></td>
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
                                    <strong>Command Line</strong></td>
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
