<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ReportsConfig.aspx.cs" Inherits="website2016V2.ReportsConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


   <%-- <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

    <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <script>
        $(function () {
            $("[id$=txtStart], [id$=txtEnd]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                //startDate: new Date(),
                format: 'dd-mm-yyyy',
                buttonImage: '/Images/calender.png',
                autoclose: true,
                orientation: 'auto bottom',

            });

        });
    </script>


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

    <h3>Reports Config</h3>
    
                <div class="col-md">
                    <div class="success" id="successMessage" runat="server">
                        <asp:Label ID="lblSucces" runat="server" Width="200px"></asp:Label>
                    </div>
                    <div class="warning" id="warningMessage" runat="server">
                        <asp:Label ID="lblWarning" runat="server" Width="400px"></asp:Label>
                    </div>
                    <div class="error" id="errorMessage" runat="server">
                        <asp:Label ID="lblError" runat="server" Width="200px"></asp:Label>
                    </div>
                </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Reports Config"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">

                        <asp:GridView ID="GridView1" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">

                            <Columns>

                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                <asp:BoundField DataField="ReportName" HeaderText="ReportName" SortExpression="ReportName" />
                                <asp:BoundField DataField="ReportDescription" HeaderText="ReportDescription" SortExpression="ReportDescription" />
                                <asp:BoundField DataField="ReportType" HeaderText="ReportType" SortExpression="ReportType" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <h3>Controls</h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Button ID="Button1" runat="server" Text="Show Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                        <%-- type="button" data-toggle="collapse" data-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">--%>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="collapse" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2"></div>
                </div>
                <br />

                <div class="row">
                    <div class="col-md-2">Name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">Description:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">Type:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList1" runat="server" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
               
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                    </div>
                </div>
               
            </div>

        </div>
    </div>

</asp:Content>
