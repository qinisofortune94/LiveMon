<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewSensors.aspx.cs" Inherits="website2016V2.NewSensors" %>
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


   
    <%--    Create role part--%>

    <h3>Sensors</h3>

                    <asp:Label ID="lblErr" runat="server" Visible="false"  Width="200px"></asp:Label>

                     <div class="success" id="successMessage"  runat="server">
                           <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
                        </div>

                    <div class="error" id="errorMessage"  runat="server">
                               <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
                        </div>

                    <div class="warning" id="warningMessage"  runat="server">
                          <asp:Label ID="lblwarning" runat="server"  Width="400px"></asp:Label>
                        </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add Sensor"></asp:Label>
                    </strong>
                    </a>
                </h4>

            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                 <div class="row">
                    <div class="col-md-2">Sensor Type:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSensorType" runat="server" PlaceHolder="Select sensor type" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">Device:</div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="ddlDevice" runat="server" PlaceHolder="Please select device" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropdownList>
                    </div>
                </div>
             
                <div>
                         <asp:GridView ID="cmbFields" runat="server" CssClass="gvdatatable table table-striped table-bordered"  AutoGenerateColumns="false" >

                            <Columns>
                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>--%>

                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>--%>

                            <%--    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id"  InsertVisible="False" ReadOnly="True"></asp:BoundField>--%>
                                <asp:BoundField DataField="Field Name" HeaderText="Field Name" SortExpression="FieldName" />
                                <asp:BoundField DataField="Field Suffix" HeaderText="Field Suffix" SortExpression="FieldSuffix" />
                                <asp:BoundField DataField="Field" HeaderText="Field" SortExpression="Field" />
                                <asp:BoundField DataField="Display Field" HeaderText="Display Field" SortExpression="DisplayField" />
                                <asp:BoundField DataField="Field MaxVal" HeaderText="Field Max Val" SortExpression="FieldMaxVal" />
                                <asp:BoundField DataField="Field MinVal" HeaderText="Field Min Val" SortExpression="FieldMinVal" />
                                <asp:BoundField DataField="Field Notes" HeaderText="Field Notes" SortExpression="FieldNotes" />
                                <asp:BoundField DataField="Field Max Warn Val" HeaderText="Field Max Warn Val" SortExpression="FieldMaxWarnVal" />
                                <asp:BoundField DataField="Field Min Warn Val" HeaderText="Field Min Warn Val" SortExpression="FieldMinWarnVal" />
                                <asp:BoundField DataField="Field Percentage Test" HeaderText="Field Percentage Test" SortExpression="FieldPercentageTest" />

                           </Columns>
                        </asp:GridView>

                </div>

                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label6" runat="server" Text="Caption" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter  sensor caption" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"><asp:Label ID="Label7" runat="server" Text="Module" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtModule" runat="server" PlaceHolder="Please enter Module" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label8" runat="server" Text="Register" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRegister" runat="server" PlaceHolder="Please enter Register" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"><asp:Label ID="Label11" runat="server" Text="MaxValue" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtMaxValue" runat="server" PlaceHolder="Please enter Maximum Value" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label4" runat="server" Text="Zero Value" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtZeroValue" runat="server" PlaceHolder="Please enter zero value" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"><asp:Label ID="Label13" runat="server" Text="Divisor" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDivisor" runat="server" PlaceHolder="Please enter divisor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label12" runat="server" Text="Multiplier" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtMultiplier" runat="server" PlaceHolder="Please enter multiplier" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"><asp:Label ID="lblExtraData" runat="server" Text="Extra Data" Width="350px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter extra data" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblExtraData1" runat="server" Text="Extra Data 1" Width="350px"></asp:Label>

                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter Extra Data 1" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"><asp:Label ID="lblExtraData2" runat="server" Text="Extra Data 2" Width="350px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extra data 2" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="lblExtraData3" runat="server" Text="Extra Data 3" Width="350px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter Extra Data 3" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"><asp:Label ID="lblExtraValue" runat="server" Text="Extra Value" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraValue" runat="server" PlaceHolder="Please enter extra value" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="lblExtraValue1" runat="server" Text="Extra Value 1" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExtraValue1" runat="server" PlaceHolder="Please enter Extra value 1" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Sensor Group:</div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="ddlSensorGroup" runat="server" PlaceHolder="Please select sensor group" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropdownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label17" runat="server" Text="Sensor Location" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSensorLocation" runat="server" PlaceHolder="Select sensor location" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-md-2"><asp:Label ID="Label18" runat="server" Text="Sensor Site" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="ddlSensorSite" runat="server" PlaceHolder="Please select sensor site" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropdownList>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label5" runat="server" Text="Serial Number:" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSerialNumber" runat="server" PlaceHolder="Please enter serial number" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                   </div>

                    <div class="col-md-2"><asp:Label ID="Label16" runat="server" Text="Sensor Default alertd Group" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="ddlSensorDefaultAlertsGroup" runat="server" PlaceHolder="Please select Sensor Default Alerts Group" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropdownList>
                    </div>
                </div>

                 <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label19" runat="server" Text="Serial Number 1:" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                     <asp:Textbox ID="txtSerialNumber1" runat="server" PromptChar=" " HideEnterKey="True" CssClass="form-control" Width="250px" Height="34px"
                            InputMask="CCCCCCCCCCCCCCCC">
                        </asp:Textbox>
                    </div>
                     <div class="col-md-2"><asp:Label ID="Label20" runat="server" Text="Scan Rate:" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                    
                            <asp:Textbox ID="txtScanRate" runat="server" ValueText="5000" MinValue="0" CssClass="form-control" Width="250px" Height="34px"
                            ToolTip="how often to scan this sensor in milli seconds.Min 0=disabled min rate = 5000" >
                        </asp:Textbox>
                        </div>
                    </div>

                <div class="row">
                    <asp:DropDownList ID="cmbModels" runat="server" CssClass="form-control" Width="250px" Height="34px" Visible="False" AutoPostBack="True">
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


               
                <div class="row">
                        <asp:Label ID="lblOutPut" runat="server" Text="Sensor Output" Width="125px"
                            Visible="False"></asp:Label>
                    </div>
                <div class="row">
                    <asp:DropDownList ID="cmbSensOutput" runat="server" CssClass="form-control" Width="250px" Height="34px" Visible="False">
                    </asp:DropDownList>
                </div>


                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label1" runat="server" Text="Image Normal" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="filImageNormal" runat="server" />
                        <%--<%--<input type="file" name="FileUploadImageNormal" id="FileUpLoad1" />--%>
                        <asp:Image ID="imgNormal" runat="server" Height="50px" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label2" runat="server" Text="Image Error" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="filImageError" runat="server" />
                        <%--<input type="file" name="FileUploadImageError" id="FileUpLoad2" />--%>
                        <asp:Image ID="imgError" runat="server" Height="50px" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2"><asp:Label ID="Label3" runat="server" Text="Image Nor Response" Width="125px"></asp:Label></div>
                    <div class="col-md-4">
                        <asp:FileUpload ID="filImageNoResponse" runat="server" />
                        <%--<input type="file" name="FileUploadImageNoResponse" id="FileUpLoad3" />--%>
                        <asp:Image ID="imgResponse" runat="server" Height="50px" />
                    </div>
                </div>

                              
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="cmdSend" runat="server" ToolTip="Save the Sensor configuration."  Text="Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                    </div>

                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnTestSensor" runat="server" ToolTip="Test the sensor configuration and show the results."  Text="Test Sensor" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF"/>
                    </div>

                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnClearNewSensor" runat="server" Text="Clear" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" />
                    </div>
                </div>
            </div>
        </div></div>
    <%-- Display Role Part--%>

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
                     
                        <asp:GridView ID="gridNewSensors" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">

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
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                <asp:BoundField DataField="IPDeviceID" HeaderText="IPDeviceID" SortExpression="IPDeviceID" />
                                <asp:BoundField DataField="Module" HeaderText="Module" SortExpression="Module" />
                                <asp:BoundField DataField="Register" HeaderText="Register" SortExpression="Register" />
                                <asp:BoundField DataField="ScanRate" HeaderText="ScanRate" SortExpression="ScanRate" />
                                <asp:BoundField DataField="SiteID" HeaderText="SiteID" SortExpression="SiteID" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
