<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MeteringEventLog.aspx.cs" Inherits="website2016V2.Metering.MeteringEventLog" %>

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

    <h3>Metering Event Log</h3>
    
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
                        <asp:Label ID="lblAdd" runat="server" Text="Metering Event Log"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Enable Crosshair: </div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="chkCrosshair" runat="server" AutoPostBack="True" Checked="True" Enabled="False" />
                    </div>

                    <div class="col-md-2">Selected Meters:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMeters" runat="server" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-md-2">Start Date:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" runat="server" PlaceHolder="Please enter start date" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">End Date:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" runat="server" PlaceHolder="Please enter end date" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>                   
                   
                </div>
                
                <div class="row">
                <div class="col-md-2"></div>
                    <div class="col-md-2" style="vertical-align: middle;text-align:center;">
                        <asp:Button ID="Button1" runat="server"  Text="Export" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                </div>    
              </div>

                <div class="row">
                    <div class="col-md-2">Data Set:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDataSet" runat="server" Width="250px" Height="34px">
                            <asp:ListItem Selected="True" Value="0">All Data</asp:ListItem>
                            <asp:ListItem Value="2">Every 2 Rows</asp:ListItem>
                            <asp:ListItem Value="3">Every 3 Rows</asp:ListItem>
                            <asp:ListItem Value="4">Every 4 Rows</asp:ListItem>
                            <asp:ListItem Value="5">Every 5 Rows</asp:ListItem>
                            <asp:ListItem Value="10">Every 10 Rows</asp:ListItem>
                            <asp:ListItem Value="20">Every 20 Rows</asp:ListItem>
                            <asp:ListItem Value="40">Every 40 Rows</asp:ListItem>
                            <asp:ListItem Value="60">Every 60 Rows</asp:ListItem>
                            <asp:ListItem Value="100">Every 100 Rows</asp:ListItem>
                            <asp:ListItem Value="200">Every 200 Rows</asp:ListItem>
                            <asp:ListItem Value="250">Every 250 Rows</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">Default Rangers:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDefaultRangers" runat="server" Width="250px" Height="34px">
                            <asp:ListItem Value="0">Last 30 Mins</asp:ListItem>
                            <asp:ListItem Value="1">Last Hour</asp:ListItem>
                            <asp:ListItem Value="2">Last 2 Hours</asp:ListItem>
                            <asp:ListItem Value="3">Last 3 Hours</asp:ListItem>
                            <asp:ListItem Value="4">Last 5 Hours</asp:ListItem>
                            <asp:ListItem Value="5">Last 10 Hours</asp:ListItem>
                            <asp:ListItem Value="6">Last 12 Hours</asp:ListItem>
                            <asp:ListItem Value="7">Last 24 Hours</asp:ListItem>
                            <asp:ListItem Value="8">Last 2 Days</asp:ListItem>
                            <asp:ListItem Value="9">Last 4 Days</asp:ListItem>
                            <asp:ListItem Value="10">Last Week</asp:ListItem>
                            <asp:ListItem Value="11">Last Month</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                
                 <div class="row">
                    <div class="col-md-2">Email Graphs:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmailGraphs" runat="server" PlaceHolder="Please enter email graphs" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>

                     </div>
                <br />

            </div>
        </div>
    </div>

</asp:Content>
