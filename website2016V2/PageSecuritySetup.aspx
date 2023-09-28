<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PageSecuritySetup.aspx.cs" Inherits="website2016V2.PageSecuritySetup" %>
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

    <h3>Page Security Setup</h3>

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

    <div id="accordion1" role="tablist" aria-multiselectable="true" runat="server" visible="false">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne1">
                <h4 class="panel-title">
                    <a id="first1" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Update"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne1" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="View Level"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtViewLevel" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" Text="Edit Level">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEditLevel" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Text="Delete Level">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDeleteLevel" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        
                        <asp:Button ID="btnSave" runat="server" Text="Update" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSave_Click"/>
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        
                    </div>
                </div>
            </div>
        </div>
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
            <div id="collapseOne" style="height: 100%" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <asp:GridView ID="gridSecurity" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="False" DataKeyNames="AutoN" OnRowCommand="gvSample_Commands">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSave" runat="server" Text="Save" CommandName="SaveItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnk" runat="server" Text="Edit" CommandName="EditItem">
                                       
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
            </div>
        </div>
    </div>
</asp:Content>
