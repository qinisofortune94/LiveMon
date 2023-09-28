<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertSchedule.aspx.cs" Inherits="website2016V2.AlertSchedule" %>
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
   <%-- <script>
        $(function () {
            $("[id$=txtStartTime]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                //startDate: new Date(),
                format: 'dd-mm-yyyy hh:mm A',
                buttonImage: '/Images/calender.png',
                autoclose: true,
                orientation: 'auto bottom',

            });

        });
    </script>--%>

   <%--  <script>
         $(function () {
             $("[id$=txtEndTime]").datepicker({
                 showOn: 'button',
                 buttonImageOnly: true,
                 //startDate: new Date(),
                 format: 'dd-mm-yyyy hh:mm A',
                 buttonImage: '/Images/calender.png',
                 autoclose: true,
                 orientation: 'auto bottom',

             });

         });
    </script>--%>
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
           },
                 {
                     "targets": [2],
                     "visible": false,
                     "orderable": false,
                     "searchable": false
                 }]

            });
        });
    </script>
    <%--    Create role part--%>

    <h3>Alert Contact</h3>
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

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add Contact"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                    <div class="row">
                       <div class="col-md-2">Alert ID:</div>
                    <div class="col-md-4">
                            <asp:TextBox ID="txtAlertID" runat="server" ReadOnly="True"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">ontact ID</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="ContactID" runat="server" ReadOnly="True"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
              <div class="row">
                    <div class="col-md-2" >Day:</div>
                    <div class="col-md-4">
                        <asp:ListBox ID="lstDay" runat="server" Rows="1"  required="true" CssClass="form-control" Width="250px" Height="50">
                        <asp:ListItem Value="1">Monday</asp:ListItem>
                        <asp:ListItem Value="2">Tuesday</asp:ListItem>
                        <asp:ListItem Value="3">Wednesday</asp:ListItem>
                        <asp:ListItem Value="4">Thursday</asp:ListItem>
                        <asp:ListItem Value="5">Friday</asp:ListItem>
                        <asp:ListItem Value="6">Saturday</asp:ListItem>
                        <asp:ListItem Value="0">Sunday</asp:ListItem>
                        <asp:ListItem Value="7">EveryDay</asp:ListItem>
                        <asp:ListItem Value="8">WeekDays</asp:ListItem>
                         <asp:ListItem Value="9">WeekEnds</asp:ListItem>
                    </asp:ListBox>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                      
                    </div>
                </div>
              <div class="row">
                    <div class="col-md-2">Start Time:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartTime" TextMode="DateTimeLocal" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">End Time:</div>
                    <div class="col-md-4">
                     <asp:TextBox ID="txtEndTime" runat="server" TextMode="DateTimeLocal" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                      
                       
                </div>
            
                 
                     
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                       
                    <asp:Button ID="btnSend" runat="server" Text="Save" Width="250px" Height="50px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSend_Click" />
                    </div>
                    
                    <div class="col-md-2">
                        
                    </div>

                    <div class="col-md-4">
                       <%--<asp:Button ID="btnFinish" runat="server" Text="Finish" Width="250px" Height="50px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnFinish_Click"/>--%>
                    </div>
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
                        <asp:Label ID="lblAddb" runat="server" Text="Exsisting Alert Contact"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">   
                <div class="row">
                    <div class="col-md-12">
                         <asp:GridView ID="AlertsSchedgrid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateDeleteButton="True" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
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
               </div>
            </div>
            </div>
</asp:Content>
