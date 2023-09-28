<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeteringEquipmentLayout.aspx.cs" Inherits="website2016V2.MeteringEquipmentLayout" %>
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
                         exportOptions: {
                             columns: [2, 3, 4, 5],
                         }
                     },
                      {
                          extend: 'excel',
                          text: 'Excel',
                          exportOptions: {
                              columns: [2, 3, 4, 5],
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



<h3>Metering Equiqment Layout</h3>
   <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblwarning" runat="server"></asp:Label>
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
                        <asp:Label ID="lblAdd" runat="server" Text="Add Metering Equiqment Layout "></asp:Label>
                    </strong>
                    </a>
                </h4>

            </div>
         <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                 <div class="row">
                    <div class="col-md-2">Select Meter</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMeterID" runat="server" PlaceHolder="Select sensor type" required="true" CssClass="form-control" Width="250px" Height="34px"  AutoPostBack="true"></asp:DropDownList>
                    </div>
                    
                </div>

               
             <br />
             <div class="row">
                 <div class="col-md-2">Select Feeding Meter</div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="ddlFeedingMeter" runat="server" PlaceHolder="Please select device" required="false" CssClass="form-control" Width="250px" Height="34px"  AutoPostBack="true"></asp:DropdownList>
                    </div>

             </div>


               <div class="row">
                  <div class="col-md-2">MainFeeds?</div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="chkMainFeeds"  runat="server" Checked="false" />
                    </div>
                </div><br />
             <br />
                 <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="250px" Height="40px"  class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAdd_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
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
                        <asp:Label ID="lblAddb" runat="server" Text="Edit/Delete"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gridPeople" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">

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

                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                <asp:BoundField DataField="MeterID" HeaderText="MeterID" SortExpression="MeterID" Visible="false"/>
                                 <asp:BoundField DataField="Meter" HeaderText="Meter" SortExpression="Meter"/>
                                <asp:BoundField DataField="FeedingMeterID" HeaderText="FeedingMeterID" SortExpression="FeedingMeterID" Visible="false"/>
                                <asp:BoundField DataField="FeedingMeter" HeaderText="FeedingMeter" SortExpression="FeedingMeter"/>
                                <asp:BoundField DataField="MainsFeed" HeaderText="MainsFeed" />
                                

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div><br />

</asp:Content>
