<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUsers.aspx.cs" Inherits="website2016V2.AddUsers" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

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

     <h3>Add Users</h3>

    <div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
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
                        <asp:DropDownList ID="ddlPerson" runat="server" CssClass="form-control" Width="250px" Height="34px" required="True" AutoPostBack="true" OnSelectedIndexChanged="ddlPerson_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">First Name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        <%--<igtxt:webtextedit id="txtFirstName" runat="server" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>--%>
                    </div>
                    <div class="col-md-2">Surname:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSurName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                       <%-- <igtxt:webtextedit id="txtSurName" runat="server" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>--%>
                    </div>
                </div>
          
                <div class="row">
                    <div class="col-md-2">User Level:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUserLevel" runat="server" CssClass="form-control" Width="100px" Height="34px" TextMode="Number" min="1" max="99" step="1" Text="5"></asp:TextBox>
                       <%-- <igtxt:webnumericedit id="txtUserLevel" runat="server" maxvalue="99" minvalue="1" valuetext="5">
                            <SpinButtons Display="OnRight"></SpinButtons>
                        </igtxt:webnumericedit>--%>
                    </div>
                    <div class="col-md-2">Phone Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPhoneNumber" runat="server" displaymode="Mask" placeholder="(###) ###-####" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                     <%--   <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"   
                                ControlToValidate="txtPhoneNumber" ErrorMessage="Not a Valid Phone Number#." ForeColor="Red"
                                ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$"></asp:RegularExpressionValidator>--%> 
                        <%--<igtxt:webmaskedit id="txtPhoneNumber" runat="server" displaymode="Mask" inputmask="(###) ###-####" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>--%>
                    </div>
                </div>
               
                <div class="row">
                    <div class="col-md-2">Fax Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFaxNumber" runat="server" displaymode="Mask" placeholder="###-###-####" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"   
                                ControlToValidate="txtFaxNumber" ErrorMessage="Not a Valid Fax Number#." ForeColor="Red"   
                                ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$"></asp:RegularExpressionValidator>--%>
                        <%-- <igtxt:webmaskedit id="txtFaxNumber" runat="server" displaymode="Mask" inputmask="(###) ###-####" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>--%>
                    </div>
                    <div class="col-md-2">Mobile Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtMobileNumber" runat="server" displaymode="Mask" placeholder="###-###-####" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"   
                                ControlToValidate="txtMobileNumber" ErrorMessage="Not a Valid Cell Number#." ForeColor="Red"   
                                ValidationExpression="^(\(?\s*\d{3}\s*[\)\-\.]?\s*)?[2-9]\d{2}\s*[\-\.]\s*\d{4}$"></asp:RegularExpressionValidator> --%>
                        <%--<igtxt:webmaskedit id="txtMobileNumber" runat="server" displaymode="Mask" inputmask="(###) ###-####" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>--%>
                    </div>
                </div>
             
                <div class="row">
                    <div class="col-md-2">Pager Number:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPagerNumber" runat="server" displaymode="Mask" placeholder="###-###-####" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        <%--<igtxt:webmaskedit id="txtPagerNumber" runat="server" displaymode="Mask" inputmask="(###) ###-####" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>--%>
                    </div>
                    <div class="col-md-2">Address:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="form-control" Width="250px" Height="70px"></asp:TextBox>
                       <%-- <igtxt:webtextedit id="txtAddress" runat="server" CssClass="form-control" Width="250px" Height="68px" TextMode="MultiLine"></igtxt:webtextedit>--%>
                    </div>
                </div>
          
                <div class="row">
                    <div class="col-md-2">Email:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" Width="300px" Height="34px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                     ErrorMessage="Invalid Email address" ForeColor="Red" ControlToValidate="txtEmail"
                                     SetFocusOnError="True"
                                     ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        </asp:RegularExpressionValidator>
                       <%-- <igtxt:webtextedit id="txtEmail" runat="server" CssClass="form-control" Width="300px" Height="34px"></igtxt:webtextedit>--%>
                    </div>
                  
                    <div class="col-md-2">Password:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" passwordmode="True" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        <%--<igtxt:webtextedit id="txtPassword" runat="server" passwordmode="True" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>--%>
                    </div>
                </div>
              
                <div class="row">
                    <div class="col-md-2">User Name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        <%--<igtxt:webtextedit id="txtUserName" runat="server" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>--%>
                    </div>

                    <div class="col-md-2">Confirm Password:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" passwordmode="True" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        <%--<igtxt:webtextedit id="txtConfirmPassword" runat="server" passwordmode="True" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>--%>
                    </div>
                </div>

                <div class="row">
                <div class="col-md-2"></div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server"  Text="Save" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClientClick="if (!ValidateFax()) { return false;};" OnClick="btnSave_Click" />
                </div>    

                <div class="col-md-2"></div>
                <div class="col-md-4">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnClear_Click"/>
                    </div>
               </div>
                                
          
        </div>
    </div>    
            
    <%--<div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Users"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

              <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gvUsers" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="ID">
                         
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
            </div>
        </div>--%>
    </div>

</asp:Content>
