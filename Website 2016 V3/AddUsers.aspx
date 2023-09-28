<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"  CodeBehind="AddUsers.aspx.cs" Inherits="website2016V2.AddUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


   <%-- <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

    <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <script>
        $(function () {
            $("[id$=txtStart], [id$=txtEnd]").datepicker({
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

    <h3>Add Users</h3>
    
               <div class="col-md-3">
                   <div class="success" id="successMessage"  runat="server">
                            <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
                    </div>
                    <div class="warning" id="warningMessage"  runat="server">
                            <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
                    </div>
                        <div class="error" id="errorMessage"  runat="server">
                            <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
                    </div>
               </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Add Users"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                
                 <div class="row">
                    <div class="col-md-2">Person:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPerson" runat="server" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">First Name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFirstName" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                    <div class="col-md-2">SurName:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSurName" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
          
                <div class="row">
                    <div class="col-md-2">UserLevel:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUserLevel" runat="server" TextMode="Number" required="true" CssClass="form-control" Width="250px" Height="34px" maxvalue="99" minvalue="1" valuetext="5"></asp:TextBox>

                    </div>
                    <div class="col-md-2">Phone Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPhoneNumber" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
               
                <div class="row">
                    <div class="col-md-2">Fax Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFaxNumber" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                    <div class="col-md-2">Mobile Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtMobileNumber" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
             
                <div class="row">
                    <div class="col-md-2">Pager Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPagerNumber" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                    <div class="col-md-2">Address:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAddress" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
          
                <div class="row">
                    <div class="col-md-2">Email:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                    <div class="col-md-2">Password:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPassword" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">User Name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUserName" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                    </div>
                    <div class="col-md-2">Confirm Password:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
               
            </div>

            <div class="row">
                <div class="col-md-2">                 
                    <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" Width="480px"></asp:Label>            
                </div>               
            </div>

            <div class="row">
                <div class="col-md-2"></div>
                    <div class="col-md-2" style="vertical-align: middle;text-align:center;">
                        <asp:Button ID="btnSave" runat="server"  Text="Save" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                </div>    
              </div>
          
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Users"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <%--  <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="GridView1" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="ID">
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
                                <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" SortExpression="PeopleId" />
                                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
                                <asp:BoundField DataField="SurName" HeaderText="SurName" SortExpression="SurName" />
                                <asp:BoundField DataField="UserLevel" HeaderText="UserLevel" SortExpression="UserLevel" />
                                <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" SortExpression="PhoneNumber" />
                                <asp:BoundField DataField="FaxNumber" HeaderText="FaxNumber" SortExpression="FaxNumber" />
                                <asp:BoundField DataField="MobileNumber" HeaderText="MobileNumber" SortExpression="MobileNumber" />
                                <asp:BoundField DataField="PageNumber" HeaderText="PageNumber" SortExpression="PageNumber"/>
                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />                                
                                <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
                               
                                </Columns>
                        </asp:GridView>  
                    </div>
                </div>
            </div>--%>
        </div>
    </div>

</asp:Content>
