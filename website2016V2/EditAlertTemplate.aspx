﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAlertTemplate.aspx.cs" Inherits="website2016V2.EditAlertTemplate" %>
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

    <h3>Alert Template </h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Edit"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
             
                 <div class="row">
                    <div class="col-md-2">Template Name:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTemplate" runat="server" PlaceHolder="" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                     <div class="col-md-2">Template Name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="Please enter  template name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <br />
                    <div class="row">
                    <div class="col-md-2">Message:</div>
                    <div class="col-md-4">
                          <asp:TextBox ID="txaMessage" runat="server" TextMode="MultiLine"  PlaceHolder="Please enter Message"  required="true" CssClass="form-control" Width="790px" Height="200px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                    
                    </div>
                        </div>
                        <br />
                <div class="row">
                    
                    <div class="col-md-2">Device to trigger :</div>
                    <div class="col-md-4">
                        <asp:Button ID="btnDevice" runat="server" Text="Device" Width="140px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">Fields to trigger :</div>
                    <div class="col-md-4">
                        <asp:Button ID="btnField" runat="server" Text="Field" Width="140px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>
                     <div class="row">
                    <div class="col-md-2">Contact name:</div>
                    <div class="col-md-4">
                        <asp:Button ID="btnName" runat="server" Text="Name" Width="140px" Height="40px"  class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">Values of the fields:</div>

                    <div class="col-md-4">
                        <asp:Button ID="btnValues" runat="server" Text="Values" Width="140px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>
                        <div class="row">
                    <div class="col-md-2">start date :</div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAlertStart" runat="server" Text="Alert Start" Width="140px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">Alert running for x mins:</div>

                    <div class="col-md-4">
                        <asp:Button ID="btnAlertMinutes" runat="server" Text="Alert Minutes" Width="140px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>
                    <div class="row">
                    <div class="col-md-2"> Alert running mins:</div>
                    <div class="col-md-4">
                        <asp:Button ID="btnCRF" runat="server" Text="CRLF" Width="140px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">Normal message :</div>

                    <div class="col-md-4">
                        <asp:Button ID="btnMessage" runat="server" Text="Return Meassage" Width="140px" Height="40px"  class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>
                   <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                        <asp:Button ID="btnNext" runat="server" Text="Next" Width="50px" Height="40px" class="btn btn-success btn-md form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2"></div>

                    <div class="col-md-4">
                     
                    </div>
                </div>
   
            </div>
        </div>
    </div>

    <%-- Display Role Part--%>

    <%--<div id="accordion" role="tablist" aria-multiselectable="true">
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
                        <asp:GridView ID="GridView1" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="False" DataKeyNames="KinId" OnRowCommand="gvSample_Commands" >

                            <Columns>
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID"  InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                                <asp:BoundField DataField="Data3" HeaderText="Data3" SortExpression="Data3" />
                                  <asp:BoundField DataField="Data1" HeaderText="Data1" SortExpression="Data1" />
                                  <asp:BoundField DataField="Data2" HeaderText="Data2" SortExpression="Data2" />
                                  <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" SortExpression="ImageNormal" />
                                  <asp:BoundField DataField="ImageError" HeaderText="ImageError" SortExpression="ImageError" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" SortExpression="Number" />
                                 <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="Port" />
                                  <asp:BoundField DataField="Caption" HeaderText="Caption" SortExpression="Caption" />
                                  <asp:BoundField DataField="ImageError" HeaderText="Number" SortExpression="Number" />
                                <asp:BoundField DataField="DTLastRead" HeaderText="DTLastRead" SortExpression="DTLastRead" />
                            </Columns>
                        </asp:GridView>  
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
    </div>
</asp:Content>
