<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSnmp.aspx.cs" Inherits="website2016V2.AddSnmp" %>

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
    <%--  <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>

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
                    }]

            });
        });
    </script>


    <h3>Devices</h3>

    <%-- <div class="success" id="successMessage"  runat="server">
     <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
    </div>
                         
                            <div class="warning" id="warningMessage"  runat="server">
                                    <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
                            </div>
                                <div class="error" id="errorMessage"  runat="server">
                                    <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
                            </div>--%>

    <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <div>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Not a valid IP address" ForeColor="Red"
            ControlToValidate="txtIPAddressLocal" SetFocusOnError="True" ValidationExpression="\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"></asp:RegularExpressionValidator>
    </div>


    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title"><asp:Label ID="lblAdd" runat="server" Text="Add"></asp:Label></h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Caption
                        <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter Caption" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        RemoteHost Name or IP
                        <asp:TextBox ID="txtRemoteHostName" runat="server" PlaceHolder="RemoteHost Name or IP" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Remote Port
                        <asp:TextBox ID="txtRemotePort" runat="server" Text="161" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Community
                        <asp:TextBox ID="txtCommunity" runat="server" Text="public" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        LocalEngineId
                        <asp:TextBox ID="txtLocalEngineId" runat="server" Text="1" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Authentication Protocol
                        <asp:DropDownList ID="dllAuthenticationProtocol" runat="server" CssClass="form-control" ></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        SNMP Version
                        <asp:DropDownList ID="ddlSNMPVersion" runat="server" CssClass="form-control" ></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        RequestId
                        <asp:TextBox ID="txtRequestId" runat="server" Text="0" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        IPAddress-Local
                        <asp:TextBox ID="txtIP1" runat="server" max="255" min="0" Text="192" TextMode="Number"
                            Width="47px">
                        </asp:TextBox>
                        <asp:TextBox ID="txtIP2" runat="server" max="255" min="0" Text="168" TextMode="Number"
                            Width="47px">
                        </asp:TextBox>
                        <asp:TextBox ID="txtIP3" runat="server" max="255" min="0" Text="0" TextMode="Number"
                            Width="47px">
                                    
                        </asp:TextBox>

                        <asp:TextBox ID="txtIP4" runat="server" max="255" min="0" Text="100" TextMode="Number"
                            Width="47px">
                            
                        </asp:TextBox>
                        <asp:TextBox ID="txtIPAddressLocal" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Visible="false" Height="34px"></asp:TextBox>

                    </div>
                </div>
            </div>

            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Timeout
                        <asp:TextBox ID="txtTimeout" runat="server" TextMode="Number" max="999999" min="1" Text="2000" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        User
                        <asp:TextBox ID="TxtUser" runat="server" Text="RogerFraser" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Password
                        <asp:TextBox ID="txtPassword" runat="server" PasswordMode="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Confirm Password
                        <asp:TextBox ID="txtConfirmPassword" runat="server" PlaceHolder="Please Confirm Password" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Local Port
                        <asp:TextBox ID="txtLocalPort" runat="server" max="9999999" TextMode="Number"
                            min="0" Text="0" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Data 1
                        <asp:TextBox ID="txtData1" runat="server" PlaceHolder="Please enter Data 1" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Data 2
                        <asp:TextBox ID="txtData2" runat="server" PlaceHolder="Please enter Data 2" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Data 3
                        <asp:TextBox ID="txtData3" runat="server" PlaceHolder="Please enter Data 3" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Device Location
                        <asp:DropDownList ID="DdlDevicelocation" runat="server" CssClass="form-control" ></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Device Site
                        <asp:DropDownList ID="DdlDeviceSite" runat="server" CssClass="form-control" ></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">

                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">

                    </div>
                </div>
            </div>
            <h3>File Uploads</h3>
            <div class="row">                
                <div class="col-md-4">
                    <div class="form-group">
                        Image No Response
                        <div class="input-group">
                            <div class="custom-file" style="border: 1px solid #ccc; padding-left: 5px!important">
                               <asp:FileUpload ID="filImageError" runat="server" />                        
                            </div>
                            <asp:Image ID="imgError" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Image Normal
                        <div class="input-group">
                            <div class="custom-file" style="border: 1px solid #ccc; padding-left: 5px!important">
                               <asp:FileUpload ID="filImageNormal" runat="server" />
                            </div>
                            <asp:Image ID="imgNormal" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Image Error
                        <div class="input-group">
                            <div class="custom-file" style="border: 1px solid #ccc; padding-left: 5px!important">
                               <asp:FileUpload ID="filImageNoResponse" runat="server" />
                            </div>                            
                        <asp:Image ID="imgResponse" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">                
                <div class="col-md-2">                    
                </div>
                <div class="col-md-8 text-center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100" class="btn bg-gray" OnClick="btnCreate_Click" />
                    <asp:Button ID="BtnClear" runat="server" Text="Clear" Width="100" class="btn bg-gray" OnClick="btnClear_Click" />
                </div>
                    <div class="col-md-2">
                    </div>
            </div>
        </div>
    </div>
</asp:Content>
