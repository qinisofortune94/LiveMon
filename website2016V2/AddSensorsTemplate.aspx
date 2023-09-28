<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSensorsTemplate.aspx.cs" Inherits="website2016V2.AddSensorsTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <%--  <script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap.min.js"></script>
    <script src="DataTable/dataTables.buttons.min.js"></script>--%>



    <!-- DataTables -->
    <script src="../Content/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="../Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
    <script src="../Content/plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
    <script src="../Content/plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>

    <script src="DataTable/dataTables.buttons.min.js"></script>
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />

    <!-- DataTables -->
    <link rel="stylesheet" href="../Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css">
    <link rel="stylesheet" href="../Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css">



    <script src="DataTable/jszip.min.js"></script>
    <script src="DataTable/pdfmake.min.js"></script>
    <script src="DataTable/vfs_fonts.js"></script>
    <script src="DataTable/buttons.html5.min.js"></script>

    <%-- <link href="DataTable/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />--%>

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


    <div class="card" style="box-shadow: none !important">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 20px !important; font-weight: 600;">Sensor Template</h3>
        </div>
        <div class="card-body" style="padding: 10px !important">


            <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblSuccess" runat="server"></asp:Label>
            </div>
            <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblWarning" runat="server"></asp:Label>
            </div>
            <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>

            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                       
                            <asp:Label ID="lblAdd" runat="server" data-card-widget="collapse" data-toggle="tooltip" title="Collapse" Style="cursor: pointer;" Text="Add Sensor Template"></asp:Label></h3>
                   
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="card-body" style="padding: 10px !important">
                    <asp:Label ID="lblErr" runat="server" Visible="false" Width="200px"></asp:Label>

                    <div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblType" runat="server" Text="Type"></asp:Label>
                                    <asp:DropDownList ID="cmbType" runat="server" PlaceHolder="Select sensor type" required="true" CssClass="form-control" Height="34px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblDevice" runat="server" Text="Device "></asp:Label>
                                    <asp:DropDownList ID="cmbDevice" runat="server" PlaceHolder="Please select device" required="true" CssClass="form-control" Height="34px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Caption"></asp:Label>
                                    <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter  sensor caption" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="Module"></asp:Label>
                                    <asp:TextBox ID="txtModule" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator11"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtModule"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label8" runat="server" Text="Register"></asp:Label>
                                    <asp:TextBox ID="txtRegister" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator6"
                                        runat="server"
                                        ForeColor="Red"
                                        Display="Dynamic"
                                        ControlToValidate="txtRegister"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label11" runat="server" Text="MaxValue"></asp:Label>
                                    <asp:TextBox ID="txtMaxValue" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator1"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtMaxValue"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label4" runat="server" Text="Zero Value"></asp:Label>
                                    <asp:TextBox ID="txtZeroValue" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator2"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtZeroValue"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label13" runat="server" Text="Divisor"></asp:Label>
                                    <asp:TextBox ID="txtDivisor" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator3"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtDivisor"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label12" runat="server" Text="Multiplier"></asp:Label>
                                    <asp:TextBox ID="txtMultiplier" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator4"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtMultiplier"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraData" runat="server" Text="Extra Data" Width="350px"></asp:Label>
                                    <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter extra data" CssClass="form-control" Height="34px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraData1" runat="server" Text="Extra Data 1" Width="350px"></asp:Label>
                                    <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter Extra Data 1" CssClass="form-control" Height="34px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraData2" runat="server" Text="Extra Data 2" Width="350px"></asp:Label>
                                    <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extra data 2" CssClass="form-control" Height="34px"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraData3" runat="server" Text="Extra Data 3" Width="350px"></asp:Label>
                                    <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter Extra Data 3" CssClass="form-control" Height="34px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraValue" runat="server" Text="Extra Value"></asp:Label>
                                    <asp:TextBox ID="txtExtraValue" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator5"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtExtraValue"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraValue1" runat="server" Text="Extra Value 1"></asp:Label>
                                    <asp:TextBox ID="txtExtraValue1" runat="server" Text="0" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator7"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtExtraValue1"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblExtraValue2" runat="server" Text="Sensor Group"></asp:Label>
                                    <asp:DropDownList ID="cmbSensGroup" runat="server" PlaceHolder="Please select sensor group" required="true" CssClass="form-control" Height="34px"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Serial Number:"></asp:Label>
                                    <asp:TextBox ID="txtSerialNumber" runat="server" PlaceHolder="Please enter serial number" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator8"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtSerialNumber"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label20" runat="server" Text="Scan Rate:"></asp:Label>
                                    <asp:TextBox ID="txtScanRate" runat="server" Text="0" MinValue="0" CssClass="form-control" Height="34px"
                                        ToolTip="how often to scan this sensor in milli seconds.Min 0=disabled min rate = 5000">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator9"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtScanRate"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>







                        <div class="row">


                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="Label19" runat="server" Text="Serial Number 1:"></asp:Label>
                                    <asp:TextBox ID="txtSerialNumber1" runat="server" PromptChar=" " HideEnterKey="True" CssClass="form-control" Height="34px"
                                        InputMask="CCCCCCCCCCCCCCCC">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator10"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        ControlToValidate="txtSerialNumber1"
                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*"
                                        ErrorMessage="Invalid Entry (must be a number)">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Label ID="lblOutPut" runat="server" Text="Sensor Output"
                                        Visible="true"></asp:Label>
                                    <asp:DropDownList ID="cmbSensOutput" runat="server" CssClass="form-control" Height="34px" Visible="TRUE">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label></label>
                                    <asp:DropDownList ID="cmbModels" runat="server" CssClass="form-control" Height="34px" Visible="False" AutoPostBack="True">
                                        <asp:ListItem>Nothing</asp:ListItem>
                                        <asp:ListItem Value="C1002">Stultz-C1002</asp:ListItem>
                                        <asp:ListItem Value="C1010/C2020">Stulz-SNMPC1010/C2020</asp:ListItem>
                                        <asp:ListItem Value="C2020FCB">Stulz-C2020FCB</asp:ListItem>
                                        <asp:ListItem Value="C4000">Stulz-C4000</asp:ListItem>
                                        <asp:ListItem Value="C5000">Stultz-C5000</asp:ListItem>
                                        <asp:ListItem Value="C6000">Stultz-C6000</asp:ListItem>
                                        <asp:ListItem Value="C6000CH">Stultz-C6000CH</asp:ListItem>
                                        <asp:ListItem Value="C7000IOC">*Stultz-C7000IOC</asp:ListItem>
                                        <asp:ListItem Value="C7000CH">Stultz-C7000CH</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>








                        <div class="row">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Image Normal"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:FileUpload ID="filImageNormal" CssClass="form-control" runat="server" />
                                    <%--<%--<input type="file" name="FileUploadImageNormal" id="FileUpLoad1" />--%>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Image ID="imgNormal" runat="server" Height="40px" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Image Error"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:FileUpload ID="filImageError" CssClass="form-control" runat="server" />
                                    <%--<input type="file" name="FileUploadImageError" id="FileUpLoad2" />--%>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Image ID="imgError" runat="server" Height="40px" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <div class="form-group">
                                    <asp:Label ID="Label3" runat="server" Text="Image Nor Response"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:FileUpload ID="filImageNoResponse" CssClass="form-control" runat="server" />
                                    <%--<input type="file" name="FileUploadImageNoResponse" id="FileUpLoad3" />--%>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Image ID="imgResponse" runat="server" Height="40px" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <div class="form-group">
                                    Template Name:
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:TextBox ID="txtTemplateName" runat="server" required="true" CssClass="form-control" Height="34px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Button ID="btnSaveTemplate" runat="server" ToolTip="Save the Template." Text="Save Template" class="btn form-control" BackColor="#ced4da" OnClick="btnSaveTemplate_Click" />
                                </div>
                            </div>
                            <div class="col-md-2">
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <asp:Button ID="BtnClearNewSensor" runat="server" Text="Clear" class="btn  form-control" BackColor="#ced4da" OnClick="BtnClearNewSensor_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                        <asp:Label ID="lblAddb" runat="server" data-card-widget="collapse" data-toggle="tooltip" title="Collapse" Style="cursor: pointer" Text="Sensor Template"></asp:Label></h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="card-body" style="padding: 10px !important">
                    <div class="row">
                        <div class="col-md-12">
                            <%-- <asp:GridView ID="GridView1" runat="server" CellPadding="4"
                            ForeColor="#333333" GridLines="None" Width="936px"
                            PageSize="5" AllowPaging="True" ViewStateMode="Enabled">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>--%>

                            <asp:GridView ID="gridNewSensors" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="GroupID" OnRowCommand="gvSample_Commands">

                                <Columns>
                                    <%--<asp:TemplateField>
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
                                </asp:TemplateField>--%>

                                    <asp:BoundField DataField="SensorTemplateID" HeaderText="Id" SortExpression="Id" InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                    <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" SortExpression="IPDeviceID" />
                                    <asp:BoundField DataField="Module" HeaderText="Module" SortExpression="Module" />
                                    <asp:BoundField DataField="Register" HeaderText="Register" SortExpression="Register" />
                                    <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" SortExpression="ScanRate" />
                                    <asp:BoundField DataField="templateName" HeaderText="templateName" SortExpression="templateName" />


                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>






</asp:Content>
