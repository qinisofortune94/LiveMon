<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SensorGroups.aspx.cs" Inherits="website2016V2.SensorGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
  <%--  <script src="DataTable/jquery.dataTables.min.js"></script>
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
     <style>
        .control-label {
            font-weight: normal !important
        }
        input,select{
            font-size: 14px !important;
        }

    </style>
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


    <%-- <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script>
        $(function () {
            $("[id$=txtEventDate2]").datepicker({
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

    <div class="card" style="font-size:13px">
        <div class="card-header">
            <h3 class="card-title">Sensor Groups</h3>
        </div>
        <div class="card-body" style="padding: 10px !important">
            <div class="alert alert-success" id="successMessage" runat="server">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
            </div>
            <div class="alert alert-warning" id="warningMessage" runat="server">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblWarning" runat="server"></asp:Label>
            </div>
            <div class="alert alert-danger" id="errorMessage" runat="server">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>


            <div class="card" style="font-size:13px">
                <div class="card-header">
                    <h3 class="card-title">
                        <asp:Label ID="lblAdd" runat="server" Text="Add Sensor Group"></asp:Label>
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
                <div class="card-body" style="padding: 10px !important">
                    <div id="AddGroups" runat="server">
                        <div id="DivNew" runat="server">
                            <div class="row">
                                <asp:Label ID="lblID" runat="server" Text="Label" Visible="false"></asp:Label>

                                <div class="col-sm-4">
                                    <div class="form-group">
                                        Groups
                            <asp:TextBox ID="txtGroup" runat="server" PlaceHolder="Add Group" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <div class="form-group">
                                        Discription
                            <asp:TextBox ID="txtDescription" runat="server" PlaceHolder="Please enter discription" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-sm-4">
                                    <div class="row text-center">
                                        <div class="form-group">
                                            <label></label>
                                            <div class="col-sm-6">
                                                <asp:Button ID="btnSave" runat="server" Text="Add" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label></label>
                                            <div class="col-sm-6">
                                                <asp:Button ID="btnClearNewSensorGroup" runat="server" Text="Clear" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnClearNewSensorGroup_Click" />
                                            </div>
                                        </div>

                                    </div>
                                </div>


                            </div>


                        </div>

                    </div>


                    <div id="divEdit" runat="server">
                        <div id="EditGroup" runat="server">

                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        Groups
                                 <asp:TextBox ID="txtEditGroup" runat="server" PlaceHolder="add Group" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <div class="form-group">
                                        Discription
                            <asp:TextBox ID="txtEditDescription" runat="server" PlaceHolder="Please enter discription" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:Button ID="btnEdit" runat="server" Text="Add" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnEdit_Click" />

                                            </div>
                                            <div class="col-md-6">
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnDelete_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>



                </div>


            </div>


            <div class="card" style="font-size:13px">
              <div class="card-header">
                <h3 class="card-title">
                      <asp:Label ID="lblAddb" runat="server" Text="Edit/Delete"></asp:Label>
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
            <div class="card-body" style="padding: 10px !important">
                 <div class="card-body table-responsive p-0" style="padding: 10px !important">
                <div class="row">
                    <div class="col-md-12" style="font-weight:normal">
                        <asp:GridView ID="gridSensorGroups" CssClass="gvdatatable table table-striped table-bordered table-condensed" 
                            runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="GroupId" OnRowCommand="gvSample_Commands" OnPageIndexChanged="gridSensorGroups_SelectedIndexChanged" OnPageIndexChanging="gridSensorGroups_PageIndexChanging">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>--%>
                                <asp:BoundField DataField="GroupId" HeaderText="Ref No" SortExpression="Id" InsertVisible="False" ReadOnly="True" />
                                <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Name" />
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Surname" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                </div>
                </div>
             </div>
        </div>




    </div>



    <%-- <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add Sensor Group"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
            </div>
        </div>
    </div>--%>


    <%-- Display Role Part--%>

     

  <%--  <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                      
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                
            </div>
        </div>
    </div>--%>

</asp:Content>