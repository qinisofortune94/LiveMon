<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEquipmentLayout.aspx.cs" Inherits="website2016V2.AddEquipmentLayout" %>
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
        <h3>Metering Equipment Layout</h3>

                    <asp:Label ID="lblErr" runat="server" Visible="false"  Width="200px"></asp:Label>

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
                        <asp:Label ID="lblAdd" runat="server" Text="Add Equipment Layout"></asp:Label>
                        
                        <asp:HiddenField ID="SensorsID" runat="server" />
                        <asp:HiddenField ID="ParentsID" runat="server" />
                    </strong>
                    </a>
                </h4>

            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label6" runat="server" Text="Sensor Name " Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSensor" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="ddlSensor_SelectedIndexChanged1"   >
                            <asp:ListItem Selected="True" Value="0" Text="Default"></asp:ListItem>
                        </asp:DropDownList>
                        
                    </div>
                    <div class="col-md-2"><asp:Label ID="Label7" runat="server" Text="Parent Name" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                         <asp:DropDownList ID="ddlSensorParent" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="ddlSensorParent_SelectedIndexChanged"  > 
                             <asp:ListItem Selected="True" Value="0" Text="Default"></asp:ListItem>
                         </asp:DropDownList>
                       
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label12" runat="server" Text="ExtraBool" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlExtraBool" runat="server" AutoPostBack="true" required="true" CssClass="form-control" Width="250px" Height="34px"  >
                            <asp:ListItem>True</asp:ListItem>
                            <asp:ListItem>False</asp:ListItem>
                        </asp:DropDownList>
                       
                    </div>
                      <div class="col-md-2"><asp:Label ID="Label1" runat="server" Text="ExtraBool" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddExtraBool1" runat="server" AutoPostBack="true" required="true" CssClass="form-control" Width="250px" Height="34px"  >
                            <asp:ListItem>True</asp:ListItem>
                            <asp:ListItem>False</asp:ListItem>
                        </asp:DropDownList>
                       
                    </div>
                                       
                </div>
                 <div class="row">                 
                    <div class="col-md-2"><asp:Label ID="lblExtraValue" runat="server" Text="Extra Value" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraValue" runat="server"  Text="0"   CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                       <div class="col-md-2"><asp:Label ID="lblExtraValue1" runat="server" Text="Extra Value 1" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraValue1" runat="server" Text="0" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                       <div class="col-md-2"><asp:Label ID="lblExtraData" runat="server" Text="Extra Data" Width="350px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter extra data"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblExtraData1" runat="server" Text="Extra Data 1" Width="350px"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter Extra Data 1"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>                 
                </div>
                              
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="cmdSend" runat="server" ToolTip="Save the Sensor configuration."  Text="Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdSend_Click" AutoPostBack="true" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnClearNewSensor" runat="server" Text="Clear" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="BtnClearNewSensor_Click" AutoPostBack="true" />
                    </div>
                </div>
            </div>
        </div>
        </div><br />
    <%-- Display Role Part--%>

      <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Edit/Delete"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gridNewSensors" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">

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

                                <asp:BoundField DataField="ID" HeaderText="Id" SortExpression="Id"  InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="Sensor" HeaderText="Sensor" SortExpression="Sensor" />
                                <asp:BoundField DataField="Parent" HeaderText="Parent" SortExpression="Parent" />
                                <asp:BoundField DataField="ExtraBool" HeaderText="ExtraBool" SortExpression="ExtraBool" />
                                <asp:BoundField DataField="ExtraBool1" HeaderText="ExtraBool1" SortExpression="ExtraBool1" />
                                <asp:BoundField DataField="ExtraValue" HeaderText="ExtraValue" SortExpression="ExtraValue" />
                                <asp:BoundField DataField="ExtraValue1" HeaderText="ExtraValue1" SortExpression="ExtraValue1" />
                                <asp:BoundField DataField="ExtraData" HeaderText="ExtraData" SortExpression="ExtraData" />
                                <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" SortExpression="ExtraData1" />
                            </Columns>
                        </asp:GridView>  
                    </div>
                </div>
            </div>
        </div>
    </div>      
</asp:Content>
