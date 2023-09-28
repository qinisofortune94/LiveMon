<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertGroupLinkContacts.aspx.cs" Inherits="website2016V2.AlertGroupLinkContacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <%--  <script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap.min.js"></script>
    <script src="DataTable/dataTables.buttons.min.js"></script>--%>
    <script src="DataTable/jszip.min.js"></script>
    <script src="DataTable/pdfmake.min.js"></script>
    <script src="DataTable/vfs_fonts.js"></script>
    <script src="DataTable/buttons.html5.min.js"></script>



    <%--  <link href="DataTable/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />--%>




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
    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Alert Group Link Conatcts</h3>
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


            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                        <asp:Label ID="lblAdd" runat="server" Text="ContactLink"></asp:Label></h3>
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
                            <div class="col-sm-12">
                                <div class="form-group">
                                    View By Group
                                      <asp:DropDownList ID="cmbGroupMain" CssClass="form-control" runat="server" Width="630px" OnSelectedIndexChanged="cmbGroupMain_SelectedIndexChanged" ToolTip="Select group to view" Height="41px" AutoPostBack="true" AppendDataBoundItems="true">
                                          <asp:ListItem Text="--View All--" Value="0" Selected="True" />
                                      </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cmbGroupMain"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row card-body table-responsive p-0">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:GridView ID="GridContactLink" CssClass="gvdatatable table table-hover text-nowrap" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="RefNo" OnRowCommand="gvSample_Commands">

                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="25px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandName="RemoveItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="25px" />
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="RefNo" HeaderText="Ref No" SortExpression="RefNo" InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                            <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" />
                                            <asp:BoundField DataField="GroupName" HeaderText="Group Name" SortExpression="GroupName" />

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                        <asp:Label ID="Label1" runat="server" Text="Create New Contact Link"></asp:Label></h3>
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
                            <div id="DivNew" runat="server">

                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            Groups
                                            <asp:DropDownList ID="cmbGroups" runat="server" PlaceHolder="please select Group" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            Contact
                                             <asp:DropDownList ID="cmbContacts" runat="server" PlaceHolder="Please select Contact" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                        </div>

                                    </div>

                                    <div class="col-sm-4">
                                        <div class="row text-center">
                                            <div class="form-group">
                                                <label></label>
                                                <div class="col-sm-6">
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnAdd_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label></label>
                                                <div class="col-sm-6">
                                                    <asp:Button ID="BtnLinkContact" runat="server" Visible="false" Text="Link Contact" Width="100px" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnLinkContact_Click" />
                                                    <br />
                                                    <asp:Label ID="lblSuccessAdd" runat="server"></asp:Label>

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


            
    <%-- Display Role Part--%>
     <div class="card" >
              <div class="card-header">
                <h3 class="card-title"> <asp:Label ID="lblAddb" runat="server" Text="Contact Link Temp"></asp:Label></h3>
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
                    <asp:Label ID="ContactName" runat="server" Visible="false" Text="">Contact Name</asp:Label>
                    <div class="col-md-12">
                        <asp:GridView ID="GridContactLinkTemp" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender2" AutoGenerateColumns="false" DataKeyNames="ID" OnRowCommand="gvSample_Commands2">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Link Contacts" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandName="RemoveItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="GroupName" HeaderText="Group Name" SortExpression="ContactName" />
                                <asp:BoundField DataField="Name" HeaderText="Contact Name" SortExpression="GroupName" />
                                <asp:BoundField DataField="ContactName" HeaderText="Contact ID" SortExpression="GroupName" />
                                <asp:BoundField DataField="GroupContactId" HeaderText="Group Contact ID" SortExpression="GroupName" />
                                <asp:BoundField DataField="ContactId" HeaderText="Contact ID" SortExpression="GroupName" />
                            </Columns>
                        </asp:GridView>

                        <asp:GridView runat="server" CssClass="gvdatatable table table-striped table-bordered" CellPadding="4" GridLines="None" ForeColor="#333333" Width="100%"
                            ID="GridContactLinkTemp1" PageSize="100" OnRowDeleting="GridContactLinkTemp_RowDeleting">
                            <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                            <Columns>
                                <asp:CommandField SelectText="Remove" ShowSelectButton="True"></asp:CommandField>
                            </Columns>

                        </asp:GridView>

                        <br />
                        <asp:Label ID="lblSuccess" runat="server"></asp:Label>
                    </div>
                </div>
                </div>
             </div>


        </div>
    </div>





  

</asp:Content>
