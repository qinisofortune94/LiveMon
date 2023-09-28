<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkTemplates.aspx.cs" Inherits="website2016V2.LinkTemplates" %>
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
    <div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">People Details</h3>
</div>
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
    <br />
<div class="card-body">
<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Add</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
         <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="lblTemplate" runat="server" Text="Template">  </asp:Label>
                    </div>
                    <div class="col-sm-4">
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                       
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:DropDownList ID="cmbTemplates" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-sm-4">
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                       
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="lblSelect" runat="server" Text="Select Sensors">  </asp:Label>
                    </div>
                    <div class="col-sm-4">     
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4"> 
                    </div>                  
                </div>
                <div class="row" onscroll="horizontal">
                    <div class="col-sm-2">
                        <asp:TreeView ID="tvSensors" runat="server">
                        </asp:TreeView>
                    </div>
                    <div class="col-sm-4">     
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4"> 
                    </div>                  
                </div>
                
                <br />
                <div class="row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                        
                        <asp:Button ID="btnApplyAlerts" runat="server" Text="Apply Template" Height="40px" Width="250px" class="btn bg-gray form-control" OnClick="btnApplyAlerts_Click"/>
                    </div>
                    <div class="col-sm-2">
                    </div>

                    <div class="col-sm-4">
                        <asp:Button ID="BtnClear" runat="server" Text="Clear" Height="40px" Width="250px" class="btn bg-gray form-control"/>
                    </div>
                </div>   
                   
    </div>
    </div>
<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Edit/Delete</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
        <div class="row">
                    <div class="col-sm-12">
                        <asp:GridView ID="GridSensorTemplates" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">

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
                                <asp:BoundField DataField="FirstName" HeaderText="Name" SortExpression="Name"/>
                                <asp:BoundField DataField="LastName" HeaderText="Surname" SortExpression="Surname"/>
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Cell" HeaderText="Cell" />
                                <asp:BoundField DataField="Telephone" HeaderText="Telephone" />
                                <asp:BoundField DataField="Fax" HeaderText="Fax" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
    </div>
    </div>
</div>
</div>

</asp:Content>
