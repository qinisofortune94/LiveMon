<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PageSecuritySetup.aspx.cs" Inherits="website2016V2.PageSecuritySetup" %>
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

    <h3>Page Security Setup</h3>

    <div class="success" id="successMessage"  runat="server">
        <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
    </div>
    <div class="warning" id="warningMessage"  runat="server">
          <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
    </div>
    <div class="error" id="errorMessage"  runat="server">
          <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Setup"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <asp:GridView ID="gridSecurity" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="False" DataKeyNames="AutoN" OnRowCommand="gvSample_Commands" DataSourceID="SqlDataSource1">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSave" runat="server" Text="Save" CommandName="SaveItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                               
                                <asp:BoundField DataField="AutoN" HeaderText="AutoN" InsertVisible="False" ReadOnly="True" SortExpression="AutoN" />
                                <asp:BoundField DataField="PageName" HeaderText="PageName" SortExpression="PageName" />
                                <asp:BoundField DataField="PageDisplayName" HeaderText="PageDisplayName" SortExpression="PageDisplayName" />
                                <asp:BoundField DataField="ViewLevel" HeaderText="ViewLevel" SortExpression="ViewLevel" />
                                <asp:BoundField DataField="EditLevel" HeaderText="EditLevel" SortExpression="EditLevel" />
                                <asp:BoundField DataField="DeleteLevel" HeaderText="DeleteLevel" SortExpression="DeleteLevel" />


                            </Columns>
                </asp:GridView>

                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=197.81.207.71;Initial Catalog=liveMon;User ID=LivemonDev;Password=LivemonDev1234" ProviderName="System.Data.SqlClient" SelectCommand="SELECT [AutoN], [PageName], [PageDisplayName], [ViewLevel], [EditLevel], [DeleteLevel] FROM [PageSecurity]"></asp:SqlDataSource>

            </div>
        </div>
    </div>
</asp:Content>
