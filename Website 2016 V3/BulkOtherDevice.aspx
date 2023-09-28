<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BulkOtherDevice.aspx.cs" Inherits="website2016V2.BulkOtherDevice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">


    <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


<%--    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

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

    <h3> Other Devices</h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add Other Bulk Devices"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">Number of Devices:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="Please enter  number of devices" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <br />
                    <div class="col-md-2">Show Import?:</div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="chkDeviceNumber" runat="server" PlaceHolde="" required="true"></asp:CheckBox>
                    </div>
                </div>

                 <div class="row">
                    <div class="col-md-2">Import File:</div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="fuImport" runat="server" PlaceHolder="Please enter  name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:FileUpload>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                </div>
                     </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="120px"  Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="120px"  Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Display Role Part--%>

   <%-- <div id="accordion" role="tablist" aria-multiselectable="true">
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
</asp:Content>