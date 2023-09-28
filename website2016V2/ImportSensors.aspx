<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportSensors.aspx.cs" Inherits="website2016V2.ImportSensors" %>
<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
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

    <%-- <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script>
        $(function () {
            $("[id$=txtDateOfBirth]").datepicker({
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

    <h3>Import Sensor</h3>
      <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSuccess" runat="server"></asp:Label>
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
                        <asp:Label ID="lblAdd" runat="server" Text="Import Sensor"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
             
               <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                <br />
               
            </div>

                 <div class="row">
                    <div class="col-md-2">Import:</div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuFile" runat="server" PlaceHolder="" CssClass="form-control" Width="250px" Height="34px"></asp:FileUpload>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                         <asp:Button ID="btnLoad" OnClick="btnLoad_Click" runat="server" Text="Load" Width="120px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                    </div>
                </div>
                    <div class="row">
                    <div class="col-md-2">Number of rows:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNumberORows" runat="server" PlaceHolder="Please enter  number of rows" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                       <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="120px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSubmit_Click"/>
                    </div>
                <br />
                
            </div>
        </div>
    </div>

    <%-- Display Role Part--%>

      <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Import Sensor "></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%"> 
                <div class="row">
                    <div class="col-md-2">
                       Logs
                    </div>
                    <div class="col-md-12">
          <%--             <asp:GridView ID="gdvBulk" runat="server" AllowPaging="True" AutoGenerateEditButton="false" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Caption" HeaderText="Caption" />
                        <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" Visible="true" />
                        <asp:BoundField DataField="Type" HeaderText="Type" />
                        <asp:BoundField DataField="Module" HeaderText="Module" Visible="false" />
                        <asp:BoundField DataField="Registration" HeaderText="Registration" Visible="false" />
                        <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" Visible="false" />--%>
                        <%-- <asp:BoundField DataField="LastValue" HeaderText="LastValue" Visible="true" />
                        <asp:BoundField DataField="LastValueDT" HeaderText="LastValueDT" Visible="true" />
                        <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                        <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                        <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />--%>
                       <%-- <asp:BoundField DataField="MinValue" HeaderText="MinValue" Visible="false" />
                        <asp:BoundField DataField="MaxValue" HeaderText="MaxValue" Visible="false" />
                        <asp:BoundField DataField="Multiplier" HeaderText="Multiplier" Visible="false" />
                        <asp:BoundField DataField="Divisor" HeaderText="Divisor" Visible="false" />
                        <asp:BoundField DataField="OffsetStart" HeaderText="OffsetStart" Visible="false" />
                        <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" Visible="true" />
                        <asp:BoundField DataField="OutputType" HeaderText="OutputType" Visible="false" />
                        <asp:BoundField DataField="SiteID" HeaderText="SiteID" Visible="true" />
                        <asp:BoundField DataField="SiteCritical" HeaderText="SiteCritical" Visible="true" />
                        <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" Visible="false" />
                        <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" Visible="false" />
                        <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="false" />
                        <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="false" />
                        <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" Visible="false" />
                        <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" Visible="false" />
                        <asp:BoundField DataField="SensorGroup" HeaderText="SensorGroup" Visible="true" />
                        <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID" Visible="true" />
                    </Columns>
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
                </asp:GridView>--%>

                        <asp:GridView ID="gdvBulk" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false">

                            <Columns>
                        <asp:BoundField DataField="Caption" HeaderText="Caption" />
                        <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" Visible="true" />
                        <asp:BoundField DataField="Type" HeaderText="Type" />
                        <asp:BoundField DataField="Module" HeaderText="Module" Visible="false" />
                        <asp:BoundField DataField="Registration" HeaderText="Registration" Visible="false" />
                        <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" Visible="false" />
                        <%-- <asp:BoundField DataField="LastValue" HeaderText="LastValue" Visible="true" />
                        <asp:BoundField DataField="LastValueDT" HeaderText="LastValueDT" Visible="true" />
                        <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                        <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                        <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />--%>
                        <asp:BoundField DataField="MinValue" HeaderText="MinValue" Visible="false" />
                        <asp:BoundField DataField="MaxValue" HeaderText="MaxValue" Visible="false" />
                        <asp:BoundField DataField="Multiplier" HeaderText="Multiplier" Visible="false" />
                        <asp:BoundField DataField="Divisor" HeaderText="Divisor" Visible="false" />
                        <asp:BoundField DataField="OffsetStart" HeaderText="OffsetStart" Visible="false" />
                        <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" Visible="true" />
                        <asp:BoundField DataField="OutputType" HeaderText="OutputType" Visible="false" />
                        <asp:BoundField DataField="SiteID" HeaderText="SiteID" Visible="true" />
                        <asp:BoundField DataField="SiteCritical" HeaderText="SiteCritical" Visible="true" />
                        <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" Visible="false" />
                        <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" Visible="false" />
                        <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="false" />
                        <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="false" />
                        <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" Visible="false" />
                        <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" Visible="false" />
                        <asp:BoundField DataField="SensorGroup" HeaderText="SensorGroup" Visible="true" />
                        <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID" Visible="true" />
                    </Columns>
                     </asp:GridView>


                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                       
                    </div>
                </div>
                  </div>
            </div>
            </div>

</asp:Content>
