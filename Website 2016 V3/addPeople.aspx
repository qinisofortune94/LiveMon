<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="addPeople.aspx.cs" Inherits="website2016V2.addPeople" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


    <%--<link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

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



    <h3>People Details</h3>
    <asp:Label ID="lblEditSuccess" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 70%">
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
                        <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFirstName" runat="server" PlaceHolder="Please enter  name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Last Name">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLastName" runat="server" PlaceHolder="Please enter surname" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div><br /><br />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" Text="Telephone">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTelephone" runat="server" PlaceHolder="Please enter telephone number" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" Text="Cell"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCell" runat="server" PlaceHolder="Please enter cell number" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"   
                                ControlToValidate="txtCell" ErrorMessage="Not a Valid Phone Number#."   
                                ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$"></asp:RegularExpressionValidator>  
                    </div>
                </div><br />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" Text="E-Mail">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" runat="server" PlaceHolder="Please enter email" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                     ErrorMessage="Invalid Email address" ControlToValidate="txtEmail"
                                     SetFocusOnError="True"
                                     ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="Label13" runat="server" Text="Fax">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFax" runat="server" PlaceHolder="Please enter fax" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>                  
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" Text="Address">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAddress" runat="server" PlaceHolder="Please enter address" required="true" CssClass="form-control" Width="250px" Height="57px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSave" runat="server" Text="Add" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSave_Click"/>
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
        <div class="panel panel-default" style="width: 70%">
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
                                <asp:BoundField DataField="FirstName" HeaderText="Name" SortExpression="Name"/>
                                <asp:BoundField DataField="LastName" HeaderText="Surname" SortExpression="Surname"/>
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Cell" HeaderText="Cell" />
                                <asp:BoundField DataField="Telephone" HeaderText="Telephone" />
                                <asp:BoundField DataField="Fax" HeaderText="Fax" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
