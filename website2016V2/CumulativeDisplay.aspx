<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CumulativeDisplay.aspx.cs" Inherits="website2016V2.CumulativeDisplay" %>
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
                    <%--<div class="col-md-2"></div>--%>
                    <div class="col-md-4">
                          <div  ID="Charts" runat="server" enableviewstate="true">
                    </div>
                    </div>
                   
                   <%-- <div class="col-md-2"></div>--%>
                    <div class="col-md-4">
                       <div  ID="DivTarrifReport" runat="server" enableviewstate="true">
                    </div>
                    </div>
             <div class="col-md-2"></div>
                    <div class="col-md-4">
                          <div ID="Graph" runat="server" enableviewstate="true"></div>
                    </div>
             </div>
            </div>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Cumulative KW Display : Select Range By Date or Defalut "></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
                        <asp:Label ID="lblErr" runat="server"></asp:Label>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <asp:Panel ID="Panel1" runat="server" Border="1"  BorderStyle="outset">
                <div class="row">
                     <%--   <div class="col-md-2"></div>
                          <div class="col-md-4">
                        <asp:DropDownList ID="ddlMeter" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>--%>
                      <div class="col-md-2"></div>
                          <div class="col-md-4">
                        <asp:DropDownList ID="ddlCurrentSite" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="cmbCurrentSite_SelectedIndexChanged"></asp:DropDownList>
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
                  <div class="col-md-2">End Day:</div>
                    <div class="col-md-4">
                        <asp:Button ID="btnGenerate1" runat="server" Text="Generate" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" height="40px" OnClick="btnGenerate1_Click"/>
                    </div>
                </div>
               </asp:Panel>
                  <br />
                <asp:Panel ID="Panel2" runat="server" Border="1"  BorderStyle="outset">
                <div class="row">
                    
                    <div class="col-md-2">Default Ranges:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRanges" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px">
                            <asp:ListItem>-----Select----</asp:ListItem>
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
               
                 <div class="col-md-4">
                        <asp:Button ID="btnGenerate2" runat="server" Text="Generate" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" height="40px" OnClick="btnGenerate2_Click"/>
                    </div>
               </div>
                    </asp:Panel>
                  <br />     
                        
              <%--  <div class="row">
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnClear" runat="server" Text="Clear" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF" />
                    </div>
                </div>--%>
            </div>
        </div>  
</asp:Content>
