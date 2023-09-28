<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetLocation.aspx.cs" Inherits="website2016V2.SetLocation" %>
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



    <h3>Set Locations</h3>
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
                        <asp:Label ID="lblAdd" runat="server" Text="Add"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Cabinet"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCabinet" runat="server" PlaceHolder="Please enter cabinet" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Floor">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFloor" runat="server" PlaceHolder="floor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>          
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label13" runat="server" Text="Surburb">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSuburb" runat="server" PlaceHolder="Please enter surburb" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" Text="Building">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtBuilding" runat="server" PlaceHolder="Please enter building" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" Text="Unit"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUnit" runat="server" PlaceHolder="Please enter unit" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" Text="Street">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStreet" runat="server" PlaceHolder="Please enter street" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" Text="City">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCity" runat="server" PlaceHolder="Please enter city" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" Text="Town">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTown" runat="server" PlaceHolder="Please enter town" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label16" runat="server" Text="Room">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRoom" runat="server" PlaceHolder="Please enter room" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label10" runat="server" Text="Planet">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPlanet" runat="server" PlaceHolder="Please enter planet" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>          
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Text="Province">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtProvince" runat="server" PlaceHolder="Please enter province" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label9" runat="server" Text="Country">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCountry" runat="server" PlaceHolder="Please enter country" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label11" runat="server" Text="GPS Latitude">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGPSLat" runat="server" PlaceHolder="Please enter gps latitude" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ID="RegularExpressionValidator3" runat="server"   
                        ControlToValidate="txtGPSLat" ErrorMessage="Not a Valid Latitude#."   
                        ValidationExpression="^-?([1-9]?[0-9])\.{1}\d{1,12}"></asp:RegularExpressionValidator>    
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label12" runat="server" Text="GPS Longitude">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGPSLong" runat="server" PlaceHolder="Please enter gps longitude" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ID="RegularExpressionValidator1" runat="server"   
                            ControlToValidate="txtGPSLong" ErrorMessage="Not a Valid Longitude#."   
                            ValidationExpression="^-{1,3}\d*\.{0,1}\d+$"></asp:RegularExpressionValidator>   
                    </div>              
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label14" runat="server" Text="Icon">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="filImageIcon" runat="server" Width="184px" CssClass="form-control"/>
                    </div>
                    <div class="col-md-2">
                        
                    </div>
                    <div class="col-md-4">
                        <asp:Image ID="imgIcon" runat="server" Height="45px" Width="50px" />
                    </div>
                </div><br /><br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAddLocation" runat="server" Text="Add" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAddLocation_Click"/>
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnClear" runat="server" Text="Clear" Height="40px" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="BtnClear_Click" />
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
                    <div class="col-sm-11 col-md-11 col-lg-12">
                        <asp:GridView ID="gdvLocations" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">

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
                                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false"/>
                                <asp:BoundField DataField="TypeDS" HeaderText="TypeDS" SortExpression="TypeDS" Visible="false"/>
                                <asp:BoundField DataField="Cabinet" HeaderText="Cabinet" />
                                <asp:BoundField DataField="Room" HeaderText="Room" />
                                <asp:BoundField DataField="Floor" HeaderText="Floor" />
                                <asp:BoundField DataField="Building" HeaderText="Building" />
                                <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street"/>
                                <asp:BoundField DataField="Suburb" HeaderText="Surburb" SortExpression="Surburb"/>
                                <asp:BoundField DataField="Town" HeaderText="Town" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="Province" HeaderText="Province" />
                                <asp:BoundField DataField="Country" HeaderText="Country" />
                                <asp:BoundField DataField="Planet" HeaderText="Planet" />
                                <asp:BoundField DataField="GPSLat" HeaderText="GPSLat" />
                                <asp:BoundField DataField="GPSLong" HeaderText="GPSLong" />
                                <asp:BoundField DataField="defaultlocation" HeaderText="defaultlocation" Visible="false" />
                             
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
