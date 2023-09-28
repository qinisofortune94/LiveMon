<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PageSites.aspx.cs" Inherits="website2016V2.PageSites" %>
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


    <h3>Sites</h3>
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
                        <asp:Label ID="Label1" runat="server" Text="Site"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSite" runat="server" PlaceHolder="Please enter  name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div><br />
                    <div class="col-md-2">
                        
                    </div>
                    <div class="col-md-4">
                        
                    </div>
                </div>
                <div class="row" style="margin-bottom:8px">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Icon">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="filImageIcon" runat="server" Width="266px" />
                    </div><br />
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" Text="Image Icon">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Image ID="imgIcon" runat="server" Height="45px" Width="50px" />
                    </div><br />
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblDelete" Visible="false" runat="server" Text="Delete?">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="chkDelete" Visible="false" runat="server" Checked="false" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSave" runat="server" Text="Add" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSave_Click"/>
                        <%--<asp:Button ID="btnEdit" runat="server" Visible="false" Text="edit" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF"/>--%>
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
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Edit/Delete"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gridSites" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                <asp:BoundField DataField="Site" HeaderText="Name" SortExpression="Site"/>
                             
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
