<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemLog.aspx.cs" Inherits="website2016V2.SystemLog" %>
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

    <h3>System Log</h3>
    
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
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="System Log"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Filter By Time</div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">Start Day:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStart" runat="server" PlaceHolder="Please enter start date" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                    <div class="col-md-2">End Day:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEnd" runat="server" PlaceHolder="Please enter end date" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>                        

            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                </div>
            </div>
            </div>
        </div>
    </div>

    <%-- Display Role Part--%>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
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

                        <asp:GridView ID="GridView1" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">

                            <Columns>

                                <asp:BoundField DataField="AlertType" HeaderText="Alert Type" SortExpression="AlertType" />
                                <asp:BoundField DataField="AlertMess" HeaderText="Alert Message" SortExpression="AlertMess" />
                                <asp:BoundField DataField="AlertDest" HeaderText="Alert Dest" SortExpression="AlertDest" />
                                <asp:BoundField DataField="Sent" HeaderText="Date Sent" SortExpression="Sent" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
