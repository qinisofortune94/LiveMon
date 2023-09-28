<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SensorAlertGroups.aspx.cs" Inherits="website2016V2.SensorAlertGroups" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <%--<script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap.min.js"></script>
    <script src="DataTable/dataTables.buttons.min.js"></script>--%>

    
    <!-- DataTables -->
<script src="../Content/plugins/datatables/jquery.dataTables.min.js"></script>
<script src="../Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
<script src="../Content/plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script src="../Content/plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>

     <script src="DataTable/dataTables.buttons.min.js"></script>
     <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />

     <!-- DataTables -->
  <link rel="stylesheet" href="../Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
  <link rel="stylesheet" href="../Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css">



    <script src="DataTable/jszip.min.js"></script>
    <script src="DataTable/pdfmake.min.js"></script>
    <script src="DataTable/vfs_fonts.js"></script>
    <script src="DataTable/buttons.html5.min.js"></script>

<%--    <link href="DataTable/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />--%>




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
                        title: 'Users',
                        exportOptions: {
                            columns: [3, 4, 5, 6, 7],
                        }
                    },
                    {
                        extend: 'excel',
                        text: 'Excel',
                        title: 'Users',
                        exportOptions: {
                            columns: [3, 4, 5, 6, 7],
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


<%--     <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script>
        $(function () {
            $("[id$=txtEventDate1]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                //startDate: new Date(),
                format: 'dd-mm-yyyy', 
                buttonImage: '/Images/calender.png',
                autoclose: true,
                orientation: 'auto bottom',

            });

        });
    </script>--%>

    <%--    Create role part--%>



     <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
        <div class="card" >
              <div class="card-header">
                <h3 class="card-title"> <asp:Label ID="Label1" runat="server" Text="Sensor Alert Groups"></asp:Label></h3>
                <div class="card-tools">
                    <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                        <i class="fas fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="card-body" style="padding: 10px !important">
               <div class="row">
                    <div class="col-md-12">
                        <div width:"936px">
                             <asp:Button ID="btnAddNew" Height="40px" Width="250px" runat="server" Text="Add New" CssClass="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAddNew_Click" />
                        </div><br />
                        <asp:GridView ID="gridContactGroups" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="GroupID" OnRowCommand="gvSample_Commands">

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

                                            <asp:BoundField DataField="GroupId" HeaderText="Ref No" Visible="true" SortExpression="GroupId"/>
                                             <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group"/>
                                            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"/>
                            </Columns>
                        </asp:GridView>  
                    </div>
                </div>


                
    <div id="DivNew" runat="server">

        <div runat="server" id="AddGroups">

            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                        <asp:Label ID="lblAdd" runat="server" Text="Add Sensor Alert Group"></asp:Label></h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                </div>
                <div class="card-body" style="padding: 10px !important">

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <asp:Label ID="Label8" runat="server" Text="Group"></asp:Label>
                                <asp:TextBox ID="txtGroup" runat="server" PlaceHolder="Add Group" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-4">
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="Description">  </asp:Label>
                                <asp:TextBox ID="txtDescription" runat="server" PlaceHolder="Please enter discription" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>

                        </div>

                        <div class="col-sm-4">
                            <div class="row text-center">
                                <div class="form-group">
                                    <label></label>
                                    <div class="col-sm-6">
                                        <asp:Button ID="btnSave" runat="server" Text="Add Group" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnSave_Click" />
                                        <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label></label>
                                    <div class="col-sm-6">
                                        <asp:Button ID="BtnClearNewSensorAlertGroup" runat="server" Text="Clear" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="BtnClearNewSensorAlertGroup_Click" />
                                    </div>
                                </div>

                            </div>
                        </div>


                    </div>

                </div>
            </div>

        </div>
    </div>

                
    <div id="divEdit" runat="server">
        <div runat="server" id="EditGroups">

                <div class="card" >
              <div class="card-header">
                <h3 class="card-title"> <asp:Label ID="Label12" runat="server" Text="Contact Group"></asp:Label></h3>
                <div class="card-tools">
                    <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                        <i class="fas fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="card-body" style="padding: 10px !important">
                <div>
                    <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                      <asp:Label ID="Label3" runat="server" Text="Group"></asp:Label> 
                                        <asp:TextBox ID="txtEditGroup" runat="server" PlaceHolder="Add Group" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
              
                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <div class="form-group">
                                       <asp:Label ID="Label4" runat="server" Text="Description">  </asp:Label>
                                        <asp:TextBox ID="txtEditDescription" runat="server" PlaceHolder="Please enter discription" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                  
                                        </div>

                                </div>

                                <div class="col-sm-4">
                                    <div class="row text-center">
                                        <div class="form-group">
                                            <label></label>
                                            <div class="col-sm-6">
                                                 <asp:Button ID="btnDelete" runat="server" Text="AddGroup" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnAddNewSensorAlertgroup_Click"/>
                        <asp:Label ID="lblEditSuccess" runat="server" Text=""></asp:Label>
                                                </div>
                                        </div>
                                        <div class="form-group">
                                            <label></label>
                                            <div class="col-sm-6">
                                               <asp:Button ID="btnEdit" runat="server" Text="Clear" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="BtnClearNewSensorAlertGroup_Click" />
                    
                                                </div>
                                        </div>
                                        </div>

                                    </div>
                                </div>


                            </div>

             
                </div>
                </div>

    </div>
        </div>



                </div>
             </div>








    </div>

</asp:Content>
