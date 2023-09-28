<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SiteCamerasSetup.aspx.cs" Inherits="website2016V2.SiteCamerasSetup" %>
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



    <h3>Site Cameras Setup</h3>
    <div class="alert alert-success" id="successMessage" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
    </div>
    
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Sites"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:ListBox ID="lstSites" runat="server" Height="100px" Width="240px" AutoPostBack="True" OnSelectedIndexChanged="lstSites_SelectedIndexChanged"></asp:ListBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Select Camera">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:ListBox ID="lstCameras" runat="server" Height="100px" Width="240px"></asp:ListBox>
                    </div>
                </div>
              
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="btnAddCamera" runat="server" Text="Add Camera" Height="40px" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnAddCamera_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Delete"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gdvUsersToSite" CssClass="gvdatatable table table-striped table-bordered" runat="server" DataKeyNames="Camera ID" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" OnRowCommand="gvSample_Commands">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Camera ID" HeaderText="Camera ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                <asp:BoundField DataField="Site ID" HeaderText="Site ID" SortExpression="Site ID"/>
                                <asp:BoundField DataField="Caption" HeaderText="Caption" SortExpression="Caption"/>
                                <asp:BoundField DataField="IP Address" HeaderText="IP Address" />
                             
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
