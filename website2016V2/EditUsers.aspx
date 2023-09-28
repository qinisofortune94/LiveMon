<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditUsers.aspx.cs" Inherits="website2016V2.EditUsers" %>
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

        function ValidateFax() {
            //var regex = new RegExp("^\\+[0-9]{1,3}-[0-9]{3}-[0-9]{7}$");
            var regex = new RegExp("[0](\d{9})|([0](\d{2})( |-)((\d{3}))( |-)(\d{4}))|[0](\d{2})( |-)(\d{7})");
            var fax = document.getElementById("txtFaxNumber").value;
            if (fax != '') {
                if (regex.test(fax)) {
                    alert("Fax Number Is Valid");
                } else {
                    alert("Fax Number Is Invalid");
                }
            } else {
                alert("Enter Fax Number.");
            }
        }

    </script>

    <h3>Edit Users</h3>

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
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Exsisting users"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                     
                <div class="row">
                    <div class="col-md-2">
                        <asp:GridView ID="UserGrid" runat="server"
                    CellPadding="4" ForeColor="#333333" CssClass="gvdatatable table table-striped table-bordered" AllowPaging="True" PageSize="5" AutoGenerateColumns="False" Width="97%" AutoGenerateSelectButton="True"  OnSelectedIndexChanged="gridusers_SelectedIndexChanged"   OnPageIndexChanging="Usergrid_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="FirstName" HeaderText="First Name" Visible="true" />
                        <asp:BoundField DataField="Surname" HeaderText="Surname" />
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                        <asp:BoundField DataField="UserLevel" HeaderText="User Level" />
                       <asp:BoundField DataField="Phone" HeaderText="Phone" Visible="false"/>
                         <asp:BoundField DataField="Fax" HeaderText="Fax" Visible="false"/>
                       <asp:BoundField DataField="Cell" HeaderText="Cell" Visible="false"/>
                         <asp:BoundField DataField="ID" HeaderText="ID" Visible="false"/>
                        <asp:BoundField DataField="Address" HeaderText="Address" Visible="false"/>
                       <asp:BoundField DataField="Email" HeaderText="Email" Visible="false"/>
                         <asp:BoundField DataField="Pager" HeaderText="Pager" Visible="false"/>
                      <asp:BoundField DataField="Password" HeaderText="Password" Visible="false"/>
                        
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#EFF3FB" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#EFF3FB" Font-Bold="True" ForeColor="black" />
                    <PagerStyle BackColor="#EFF3FB" ForeColor="black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
                    
                        
                    </div>
                    <div class="col-md-4">
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                       
                    </div>
                </div>
                  </div>
            </div>
        </div>

     <br />     
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label2" runat="server" Text="Edit User"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
        <div class="row">
            <div class="col-md-2">ID:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtID" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
            </div>
            <div class="col-md-2"></div>
            <div class ="col-md-4"></div>
        </div>

        <div class="row">
            <div class="col-md-2">First Name:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtFirstName" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

            </div>
            <div class="col-md-2">SurName:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtSurName" runat="server"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col-md-2">UserLevel:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtUserLevel" runat="server" TextMode="Number"  CssClass="form-control" Width="250px" Height="34px" maxvalue="99" minvalue="1" valuetext="5"></asp:TextBox>

            </div>
            <div class="col-md-2">Phone Number:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" Height="34px" server="" Width="250px"></asp:TextBox>
               
                
            </div>
        </div>

        <div class="row">
            <div class="col-md-2">Fax Number:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtFaxNumber" runat="server"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

            </div>
            <div class="col-md-2">Mobile Number:</div>
            <div class="col-md-4">
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"   Display="Dynamic" 
                                ControlToValidate="txtMobileNumber" ErrorMessage="Not a Valid Phone Number#."   
                                ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$"></asp:RegularExpressionValidator>  
                <asp:TextBox ID="txtMobileNumber" runat="server"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
             
            </div>
        </div>

        <div class="row">
            <div class="col-md-2">Pager Number:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtPagerNumber" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

            </div>
            <div class="col-md-2">Address:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtAddress" runat="server" required="true" CssClass="form-control" Width="250px" Height="120px" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col-md-2">Email:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtEmail" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                     ErrorMessage="Invalid Email address" ControlToValidate="txtEmail"
                                     SetFocusOnError="True"
                                     ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
            </div>

            <div class="col-md-2">Password:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col-md-2">User Name:</div>
            <div class="col-md-4">
                <asp:TextBox ID="txtUserName" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
            </div>

            <div class="col-md-2">Confirm Password:</div>
            <div class="col-md-4">
                 <asp:CompareValidator runat=server Display="Dynamic"
            controltovalidate=txtConfirmPassword
            controltocompare=txtPassword 
            errormessage="Passwords do not match." />
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
            </div>
        </div>

        <input type="hidden" id="PeopleID" runat="server" />
<br />
        <div class="row">
            <div class="col-md-2"></div>
            <div class="col-md-4">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnUpdate_Click"/>
            </div>

            <div class="col-md-2"></div>
            <div class="col-md-4">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClientClick="return confirm('Are you sure you want to delete this User?');" OnClick="btnDelete_Click" />
            </div>
        </div>

    </div>
     </div>
        </div>  

      
</asp:Content>
