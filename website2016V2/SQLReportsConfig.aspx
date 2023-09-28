<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SQLReportsConfig.aspx.cs" Inherits="website2016V2.SQLReportsConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
   <%-- <script src="DataTable/jquery.dataTables.min.js"></script>
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
    </script>--%>

   <%-- <script>
        function ValidateFax() {
            //var regex = new RegExp("^\\+[0-9]{1,3}-[0-9]{3}-[0-9]{7}$");
            var regex = new RegExp("[0](\d{9})|([0](\d{2})( |-)((\d{3}))( |-)(\d{4}))|[0](\d{2})( |-)(\d{7})");
            var fax = document.getElementById("txtFaxNumber").value;
            if (fax != '') {
                if (regex.test(fax)) {
                    alert("Fax Number Is Valid");
                } else {
                    alert("Fax Number Is Invalid");
                }
            } else {
                alert("Enter Fax Number.");
            }
        }

    </script>--%>

    <h3>SQL Reports Config</h3>

    <div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Server Settings"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">  
            <div class="row">
                <div class="col-md-2">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAddNew_Click" />
                </div>
                </div>

            <div class="row">
                <div class="col-md-12">
                    <asp:GridView ID="gvReports" runat="server"  CssClass="gvdatatable table table-striped table-bordered"
                      OnSelectedIndexChanged="gvReports_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="ReportID" HeaderText="RefNo" Visible="true" />
                            <asp:BoundField DataField="ReportName" HeaderText="Server Name" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Url" HeaderText="Url" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="green" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
            </div>

            <div id="DivNew" runat="server">
                <h3>New Settings</h3>

                <div id="AddGroups">
                    <div class="row">
                        <div class="col-md-2">Server Name:</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">Description:</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">Url:</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtUrl" runat="server" Height="34px" Width="450px"></asp:TextBox>
                        </div>
                    </div>

                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSave_Click" />

                    <%--<asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>--%>
                </div>
            </div>

            <div id="divEdit" runat="server">
                <h3>Edit Settings </h3>

                <div id="EditGroups">
                    <div class="row">
                        <div class="col-md-2">Server Name</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-2">Description</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEditDescription" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-2">Url</div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEditUrl" runat="server" Height="34px" Width="450px"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">Delete(tick to delete)</div>
                        <div class="col-md-4">
                            <asp:CheckBox ID="chkisDeleted" runat="server" Checked="false" /><br />

                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnEdit_Click" />
                        </div>
                    </div>

                    <asp:Label ID="lblEditSuccess" runat="server" Text=""></asp:Label>

                </div>

            </div>
                </div>
        </div>
    </div>
      
</asp:Content>
