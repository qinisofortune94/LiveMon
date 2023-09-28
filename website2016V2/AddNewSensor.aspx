<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewSensor.aspx.cs" Inherits="website2016V2.AddNewSensor" %>
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
     <style>
        .control-label {
            font-weight: normal !important
        }
        input,select{
            font-size: 14px !important;
        }

    </style>
    <%--<script type="text/javascript">
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
    </script>--%>




    <%--Create role part--%>
    
     <!-- /.card-body -->
        <div class="card"  style="display: block;font-size:13px">
             <div class="card-body" style="display: block;">
                <asp:Label ID="lblErr" runat="server" Visible="false" Width="200px"></asp:Label>
            
   <div class="alert alert-dark" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"></asp:Label>
    </div>
    <div class="alert alert-default-dark" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblwarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-default-light" id="errorMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
            <div class="row">
                    <div class="col-sm-6">
                      <!-- text input -->
                      <div class="form-group">
                       Sensor Type
                        <asp:DropDownList ID="ddlSensorType" runat="server" PlaceHolder="Select sensor type" required="true" CssClass="form-control"  OnSelectedIndexChanged="cmbType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                      </div>
                    </div>
                    <div class="col-sm-6">
                      <div class="form-group">
                        Device
                        <asp:DropdownList ID="ddlDevice" runat="server" PlaceHolder="Please select device" required="false" CssClass="form-control"  OnSelectedIndexChanged="cmbDevice_SelectedIndexChanged" AutoPostBack="true"></asp:DropdownList>
                      </div>
                    </div>
                  </div>
          <div class="row" style="margin-bottom:20px">
                    <div class="card-body table-responsive p-0">                    
                    <asp:GridView ID="cmbFields" runat="server" CellPadding="4" OnPageIndexChanging="cmbFields_PageIndexChanging" CssClass="table table-hover text-nowrap" 
                            ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateEditButton="true" OnRowCancelingEdit="cmbFields_RowCancelingEdit" OnRowEditing="cmbFields_RowEditing" OnRowUpdating="cmbFields_RowUpdating"
                            PageSize="5" AllowPaging="True" ViewStateMode="Enabled">
                            
                        </asp:GridView>
                         </div>
                </div>

                <div class="row" style="margin-bottom:20px">
                    <div class="card-body table-responsive p-0"> 
                        <asp:GridView ID="gridNewSensors" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="ID" OnRowCommand="gvSample_Commands">

                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
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
      <!-- Default box -->
      <div class="card" style="font-size:13px">
        <div class="card-header">
          <h3 class="card-title">Add New Sensor</h3>

          <div class="card-tools">
            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
              <i class="fas fa-times"></i></button>
          </div>
        </div>
        <div class="card-body" style="display: block;">

            

            <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                        <asp:Label ID="Label6" runat="server" Text="Caption" ></asp:Label>
                        <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter  sensor caption"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="Label7" runat="server" Text="Module" ></asp:Label>
                         <asp:TextBox ID="txtModule" runat="server" PlaceHolder="Please enter Module" required="true" CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Register" ></asp:Label>
                       <asp:TextBox ID="txtRegister" runat="server" PlaceHolder="Please enter Register"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>
                  </div>



            <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                        <asp:Label ID="Label11" runat="server" Text="MaxValue" ></asp:Label>
                           <asp:TextBox ID="txtMaxValue" runat="server" Text="0" required="true" CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Zero Value" ></asp:Label>
                        <asp:TextBox ID="txtZeroValue" runat="server" Text="0"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="Label13" runat="server" Text="Divisor" ></asp:Label>
                       <asp:TextBox ID="txtDivisor" runat="server" PlaceHolder="Please enter divisor"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>
                  </div>
        
             <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                        <asp:Label ID="Label12" runat="server" Text="Multiplier" ></asp:Label>
                             <asp:TextBox ID="txtMultiplier" runat="server" Text="0"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="lblExtraData" runat="server" Text="Extra Data" Width="350px"></asp:Label>
                        <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter extra data"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="lblExtraData1" runat="server" Text="Extra Data 1" Width="350px"></asp:Label>
                      <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter Extra Data 1"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>
                  </div>
               
            <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                        <asp:Label ID="lblExtraData2" runat="server" Text="Extra Data 2" Width="350px"></asp:Label>
                           <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extra data 2"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="lblExtraData3" runat="server" Text="Extra Data 3" Width="350px"></asp:Label>
                      
                            <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter Extra Data 3"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                       <asp:Label ID="lblExtraValue" runat="server" Text="Extra Value" ></asp:Label>
                        <asp:TextBox ID="txtExtraValue" runat="server" PlaceHolder="Please enter extra value"  CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>
                  </div>  

            <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                       <asp:Label ID="lblExtraValue1" runat="server" Text="Extra Value 1" ></asp:Label>
                          <asp:TextBox ID="txtExtraValue1" runat="server" Text="0" CssClass="form-control" ></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                       Sensor Group:
                         <asp:DropdownList ID="ddlSensorGroup" runat="server" PlaceHolder="Please select sensor group" required="true" CssClass="form-control" ></asp:DropdownList>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                       <asp:Label ID="Label17" runat="server" Text="Sensor Location" ></asp:Label>
                       <asp:DropDownList ID="ddlSensorLocation" runat="server" PlaceHolder="Select sensor location" required="true" CssClass="form-control" ></asp:DropDownList>
                      </div>
                    </div>
                  </div>

              
               <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                       <asp:Label ID="Label18" runat="server" Text="Sensor Site" ></asp:Label>
                           <asp:DropdownList ID="ddlSensorSite" runat="server" PlaceHolder="Please select sensor site" required="true" CssClass="form-control" ></asp:DropdownList>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                       <asp:Label ID="Label5" runat="server" Text="Serial Number:" ></asp:Label>
                           <asp:TextBox ID="txtSerialNumber" runat="server" PlaceHolder="Please enter serial number"  CssClass="form-control"  OnTextChanged="txtSerialNumber_TextChanged"></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                     <asp:Label ID="Label16" runat="server" Text="Sensor Default alerts Group" ></asp:Label>
                              <asp:DropdownList ID="ddlSensorDefaultAlertsGroup" runat="server" PlaceHolder="Please select Sensor Default Alerts Group" CssClass="form-control" ></asp:DropdownList>
                      </div>
                    </div>
                  </div>


              <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                       <asp:Label ID="Label19" runat="server" Text="Serial Number 1:" ></asp:Label>
                        <asp:Textbox ID="txtSerialNumber1" runat="server" PromptChar=" " HideEnterKey="True" CssClass="form-control" 
                            InputMask="CCCCCCCCCCCCCCCC">
                        </asp:Textbox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="Label20" runat="server" Text="Scan Rate:" ></asp:Label>
                        <asp:Textbox ID="txtScanRate" runat="server" Value="5000" MinValue="5000"  CssClass="form-control" 
                            ToolTip="how often to scan this sensor in milli seconds.Min 0=disabled min rate = 5000" >
                        </asp:Textbox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                          <label></label>
                       <asp:DropDownList ID="cmbModels" runat="server" CssClass="form-control"  Visible="true" AutoPostBack="True" OnSelectedIndexChanged="cmbModels_SelectedIndexChanged">
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
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Image Normal" ></asp:Label>
                        <div class="input-group">
                      <div class="custom-file">
                             <asp:FileUpload ID="filImageNormal" CssClass="custom-file-input" runat="server" />
                         <label class="custom-file-label" for="filImageNormal">Choose file</label>
                      </div>
                           
                    </div>
                          <asp:Image ID="imgNormal" runat="server" Height="50px" />
                      </div>
                    </div>
                    <div class="col-sm-4">
                      <div class="form-group">
                         <asp:Label ID="Label2" runat="server" Text="Image Error" ></asp:Label>
                          <div class="input-group">
                                <div class="custom-file">
                        <asp:FileUpload ID="filImageError" CssClass="custom-file-input" runat="server" />
                                     <label class="custom-file-label" for="filImageError">Choose file</label>
                        <%--<input type="file" name="FileUploadImageError" id="FileUpLoad2" />--%>
                      
                    </div>
                                <asp:Image ID="imgError" runat="server" Height="50px" />
                    </div>
                      </div>
                    </div>
                <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Image Nor Response" ></asp:Label>
                          <div class="input-group">
                                <div class="custom-file">
                        <asp:FileUpload ID="filImageNoResponse"  CssClass="custom-file-input" runat="server" />
                                      <label class="custom-file-label" for="filImageNoResponse">Choose file</label>
                        <%--<input type="file" name="FileUploadImageNoResponse" id="FileUpLoad3" />--%>
                    </div>
                              
                        <asp:Image ID="imgResponse" runat="server" Height="50px" />
                    </div>
                      </div>
                    </div>
                  </div>
               
                <div class="row form-group">
                        <asp:Label ID="lblOutPut" runat="server" Text="Sensor Output" 
                            Visible="False"></asp:Label>
                    </div>
                <div class="row form-group">
                    <asp:DropDownList ID="cmbSensOutput" runat="server" CssClass="form-control"  Visible="False">
                    </asp:DropDownList>
                </div>

         
            <div class="row">
                    <div class="col-sm-4">
                      <!-- text input -->
                      <div class="form-group">
                        <asp:Button ID="cmdSend" runat="server" ToolTip="Save the Sensor configuration."  Text="Add" Width="250px" Height="40px" class="btn btn-block" style="background-color: #ced4da" OnClick="cmdSend_Click" AutoPostBack="true" />
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                          <asp:Button ID="btnTestSensor" runat="server" ToolTip="Test the sensor configuration and show the results."  Text="Test Sensor" Width="250px" Height="40px" class="btn btn-block" style="background-color: #ced4da" OnClick="btnTestSensor_Click" AutoPostBack="true" />
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Button ID="BtnClearNewSensor" runat="server" Text="Clear" Width="250px" Height="40px" class="btn btn-block" style="background-color: #ced4da"  OnClick="BtnClearNewSensor_Click" AutoPostBack="true" />
                      </div>
                    </div>
                  </div>

               
               
          
        </div>
       
        <!-- /.card-footer-->
      </div>
      <!-- /.card -->

    
   

</asp:Content>
