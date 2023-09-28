<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BulkSensors.aspx.cs" Inherits="website2016V2.BulkSensors" %>
 <%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <style>
        .form-control {
  vertical-align: middle;

}

    </style>
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
          }]

            });
        });
    </script>

  <%--   <script defer src="../Scripts/bootstrap-datepicker.js"></script>
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

    <%--    C  reate role part--%>

    <h3>Bulk Sensors</h3>
    <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSuccess" runat="server"  Width="200px"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
    </div>
     <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Sensor Template:"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                

                 <asp:GridView ID="gdvSensorTemplates" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="TemplateID">

                            <Columns>
                               <%-- <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>--%>

                                <asp:CommandField ShowSelectButton="True" />

                                <asp:BoundField DataField="TemplateID" HeaderText="Template ID:" Visible="False" />
                                <asp:BoundField DataField="TemplateName" HeaderText="Template Name:" />
                                <asp:BoundField DataField="Caption" HeaderText="Caption:" />
                                <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" Visible="False" />
                                <asp:BoundField DataField="Module" HeaderText="Module" Visible="False" />
                                <asp:BoundField DataField="Registration" HeaderText="Registration" Visible="False" />
                                <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" Visible="False" />
                                <asp:BoundField DataField="LastValue" HeaderText="LastValue" Visible="False" />
                                <asp:BoundField DataField="LastValueDT" HeaderText="LastValueDT" Visible="False" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="False" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="False" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="False" />
                                <asp:BoundField DataField="MinValue" HeaderText="MinValue" Visible="False" />
                                <asp:BoundField DataField="MaxValue" HeaderText="MaxValue" Visible="False" />
                                <asp:BoundField DataField="Multiplier" HeaderText="Multiplier" Visible="False" />
                                <asp:BoundField DataField="Divisor" HeaderText="Divisor" Visible="False" />
                                <asp:BoundField DataField="OffsetStart" HeaderText="OffsetStart" Visible="False" />
                                <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" Visible="False" />
                                <asp:BoundField DataField="OutputType" HeaderText="OutputType" Visible="False" />
                                <asp:BoundField DataField="SiteID" HeaderText="SiteID" Visible="False" />
                                <asp:BoundField DataField="SiteCritical" HeaderText="SiteCritical" Visible="False" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" Visible="False" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" Visible="False" />
                                <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="False" />
                                <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="False" />
                                <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" Visible="False" />
                                <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" Visible="False" />
                                <asp:BoundField DataField="SensorGroup" HeaderText="SensorGroup" Visible="False" />
                                <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID" Visible="False" />


                            </Columns>
                        </asp:GridView>  

            </div>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Bulk Sensor"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                
                 <div class="row">
                    <div class="col-md-2">Number of Sensor to create:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNumberSensors" runat="server" PlaceHolder="Please enter number sensor to create" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="cboShowImport" Visible="false" Text="Show Import" runat="server" required="false" AutoPostBack="True" OnCheckedChanged="ckbShowImport_CheckedChanged"></asp:CheckBox>
                    </div>
                </div>
        <div id="tbrImportRow" runat="server" visible="false"></div>
                 <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                         <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc><br />
                            <asp:FileUpload runat="server" Width="250px" CssClass="form-control" name="SensorImport" id="FileUpload1" /><br />
                    </div>
                </div>
         </div>
                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSubmit" runat="server" Text="Create" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    
                    </div>
                     <div class="col-md-2"></div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSave" runat="server" Text="Submit" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnSubmit_Click" />
                    
                    </div>
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
               </div>
            
        </div>
   

    <%-- Display Role Part--%>

    <div id="accordion" role="tablist" aria-multiselectable="true" >
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Import Sensor:"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseThree" style="overflow-x:scroll ; overflow-y: hidden;" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                
                <asp:GridView ID="gdvBulk" OnRowCancelingEdit="gdvBulk_RowCancelingEdit" OnRowEditing="gdvBulk_RowEditing" OnRowUpdating="gdvBulk_RowUpdating" OnPageIndexChanging="gdvBulk_PageIndexChanging" 
                    runat="server" AllowPaging="True" Width="100%" CssClass="gvdatatable table table-striped table-bordered" AutoGenerateEditButton="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Caption" HeaderText="Caption:" />
                                <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" Visible="true" />
                                <asp:BoundField DataField="Module" HeaderText="Module" Visible="true" />
                                <asp:BoundField DataField="Registration" HeaderText="Registration" Visible="true" />
                                <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" Visible="true" />
                                <asp:BoundField DataField="LastValue" HeaderText="LastValue" Visible="true" />
                                <asp:BoundField DataField="LastValueDT" HeaderText="LastValueDT" Visible="true" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />
                                <asp:BoundField DataField="MinValue" HeaderText="MinValue" Visible="true" />
                                <asp:BoundField DataField="MaxValue" HeaderText="MaxValue" Visible="true" />
                                <asp:BoundField DataField="Multiplier" HeaderText="Multiplier" Visible="true" />
                                <asp:BoundField DataField="Divisor" HeaderText="Divisor" Visible="true" />
                                <asp:BoundField DataField="OffsetStart" HeaderText="OffsetStart" Visible="true" />
                                <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" Visible="true" />
                                <asp:BoundField DataField="OutputType" HeaderText="OutputType" Visible="true" />
                                <asp:BoundField DataField="SiteID" HeaderText="SiteID" Visible="true" />
                                <asp:BoundField DataField="SiteCritical" HeaderText="SiteCritical" Visible="true" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" Visible="true" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" Visible="true" />
                                <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="true" />
                                <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="true" />
                                <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" Visible="true" />
                                <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" Visible="true" />
                                <asp:BoundField DataField="SensorGroup" HeaderText="SensorGroup" Visible="true" />
                                <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID" Visible="true" />
                            </Columns>
                           <%-- <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />--%>
                        </asp:GridView>


                
                 <asp:GridView ID="gdvBulk1" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" OnRowCommand="gvSample_Commands">

                            <Columns>
                               <%-- <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>--%>

                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" CommandName="SelectItem">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Caption" HeaderText="Caption:" />
                                <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" />
                                <asp:BoundField DataField="Module" HeaderText="Module" />
                                <asp:BoundField DataField="Registration" HeaderText="Registration" />
                                <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" />
                                <asp:BoundField DataField="LastValue" HeaderText="LastValue" />
                                <asp:BoundField DataField="LastValueDT" HeaderText="LastValueDT" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" />
                                <asp:BoundField DataField="MinValue" HeaderText="MinValue" />
                                <asp:BoundField DataField="MaxValue" HeaderText="MaxValue"/>
                                <asp:BoundField DataField="Multiplier" HeaderText="Multiplier" />
                                <asp:BoundField DataField="Divisor" HeaderText="Divisor" />
                                <asp:BoundField DataField="OffsetStart" HeaderText="OffsetStart" />
                                <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" />
                                <asp:BoundField DataField="OutputType" HeaderText="OutputType" />
                                <asp:BoundField DataField="SiteID" HeaderText="SiteID" />
                                <asp:BoundField DataField="SiteCritical" HeaderText="SiteCritical" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" />
                                <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" />
                                <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" />
                                <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" />
                                <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" />
                                <asp:BoundField DataField="SensorGroup" HeaderText="SensorGroup"/>
                                <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID"/>

                            </Columns>
                        </asp:GridView>

                <%--<asp:GridView ID="gdvBulk" runat="server" AllowPaging="True" AutoGenerateEditButton="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Caption" HeaderText="Caption:" />
                                <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" Visible="true" />
                                <asp:BoundField DataField="Module" HeaderText="Module" Visible="true" />
                                <asp:BoundField DataField="Registration" HeaderText="Registration" Visible="true" />
                                <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" Visible="true" />
                                <asp:BoundField DataField="LastValue" HeaderText="LastValue" Visible="true" />
                                <asp:BoundField DataField="LastValueDT" HeaderText="LastValueDT" Visible="true" />
                                <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />
                                <asp:BoundField DataField="MinValue" HeaderText="MinValue" Visible="true" />
                                <asp:BoundField DataField="MaxValue" HeaderText="MaxValue" Visible="true" />
                                <asp:BoundField DataField="Multiplier" HeaderText="Multiplier" Visible="true" />
                                <asp:BoundField DataField="Divisor" HeaderText="Divisor" Visible="true" />
                                <asp:BoundField DataField="OffsetStart" HeaderText="OffsetStart" Visible="true" />
                                <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" Visible="true" />
                                <asp:BoundField DataField="OutputType" HeaderText="OutputType" Visible="true" />
                                <asp:BoundField DataField="SiteID" HeaderText="SiteID" Visible="true" />
                                <asp:BoundField DataField="SiteCritical" HeaderText="SiteCritical" Visible="true" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" Visible="true" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" Visible="true" />
                                <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" Visible="true" />
                                <asp:BoundField DataField="ExtraData3" HeaderText="ExtraData3" Visible="true" />
                                <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" Visible="true" />
                                <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" Visible="true" />
                                <asp:BoundField DataField="SensorGroup" HeaderText="SensorGroup" Visible="true" />
                                <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID" Visible="true" />
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>--%>

            </div>
        </div>
    </div>

</asp:Content>
