<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MeteringMonthly.aspx.cs" Inherits="website2016V2.Metering.MeteringMonthly" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
   <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


  <%--  <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

     <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

        <script>
        $(function () {
            $("[id$=txtStart]").datepicker({
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
    <h3>Metering</h3>
     <div class="panel panel-default" style="width: 1080px">
    <div class="row">
                    <div class="col-md-2">Charts1</div>
                    <div class="col-md-4">
                          <div  ID="Charts" runat="server" enableviewstate="true">
                    </div>
                    </div>
                   
                    <div class="col-md-2">Charts2</div>
                    <div class="col-md-4">
                       <div  ID="DivTarrifReport" runat="server" enableviewstate="true">
                    </div>
                    </div>
          
             </div>
    <br />
    <div class="row">
           <div class="col-md-2">Charts3</div>
                    <div class="col-md-4">
                          <div ID="DivTOUReport" runat="server" enableviewstate="true"></div>
                    </div>
                    <div class="col-md-2">Charts1</div>
                    <div class="col-md-4">
                          <div  ID="DivTOUStatsReport" runat="server" enableviewstate="true">
                    </div>
                    </div>
                   
                   </div>
        <div class="row">
             <div class="col-md-2">Charts2</div>
                    <div class="col-md-4">
                       <div  ID="DivKvarReport" runat="server" enableviewstate="true">
                    </div>
                    </div>
             <div class="col-md-2">Charts3</div>
                    <div class="col-md-4">
                          <div ID="DivKvarStatsReport" runat="server" enableviewstate="true"></div>
                    </div>
             </div>
               </div>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblErr" runat="server"> </asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
               <asp:Panel ID="Panel3" runat="server" Border="1"  BorderStyle="outset">
                  <div class="row">
                    <div class="col-md-2">Selected Meters:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMeters" runat="server" PlaceHolder="-----Select------" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">Start Day:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStart" runat="server"  TextMode="DateTimeLocal" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
             
                <div class="row">
                    <div class="col-md-2">End Day:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEnd" runat="server"  TextMode="DateTimeLocal" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">TotalBy:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTotal" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                     <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnGenerate" runat="server" Text="Genarate" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" height ="40px" OnClick="btnGenerate_Click"/>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                        
                    </div>
                </div>
              </asp:Panel>    
                <div class="row">
                    <div class="col-md-2">Tarrifs:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTarrif" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>

                    <div class="col-md-2">Email Graphs:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGraphs" runat="server" PlaceHolder="Please enter email" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    
                </div>
                <br />
             
            </div>
        </div>
    </div>
</asp:Content>
