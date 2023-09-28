<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCameraTemplete.aspx.cs" Inherits="website2016V2.AddCameraTemplete" %>
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
    <%--    Create role part--%>

<div class="card" style="font-size:13px">
    <div class="card-header">
          <h3 class="card-title">Add Camera Template
          </h3>
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
     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Display="Dynamic" runat="server" ErrorMessage="Not a valid IP address"
                                ControlToValidate="txtIpAdrres" SetFocusOnError="True" ValidationExpression="\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"></asp:RegularExpressionValidator>
       <%--      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Not a valid IP address"
      ControlToValidate="txtIpAdrres" SetFocusOnError="True" ValidationExpression="\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"></asp:RegularExpressionValidator></div>--%>
    <br />
        </div>

    <div class="card-body">
<div class="card" style="font-size:13px" id="accordion" role="tablist" aria-multiselectable="true">

    <div class="card-header">
          <h3 class="card-title">Add Camera Template
          </h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
            
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        IP Address
                        <asp:TextBox ID="txtIpAdrres" runat="server" PlaceHolder="Please enter IP Address" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                         Port
                        <asp:TextBox ID="txtPort" runat="server" PlaceHolder="Please enter Port" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                       Username
                        <asp:TextBox ID="txtUserName" runat="server" PlaceHolder="Please enter UserName" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>
                  </div>

            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        Password
                        <asp:TextBox ID="txtPassword" runat="server" PlaceHolder="Please enter Password" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        Caption
                           <asp:TextBox ID="TxtCapiton" runat="server" PlaceHolder="Please enter Caption" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        Type
                      <asp:DropDownList ID="DdlType" runat="server" CssClass="form-control" Width="250px" Height="34px">
                            <asp:ListItem Value="0" Selected="True">Auto</asp:ListItem>
                            <asp:ListItem Value="1">2K Model 1 Chanel</</asp:ListItem>
                            <asp:ListItem Value="2">3K Model</asp:ListItem>
                            <asp:ListItem Value="3">4/5/6 K Series</asp:ListItem>
                            <asp:ListItem Value="4">RTSP model</asp:ListItem>
                            <asp:ListItem Value="5">2K with 4 channels</asp:ListItem>
                            <asp:ListItem Value="6">7K with dual streams</asp:ListItem>
                            <asp:ListItem Value="7">Dual Stream Model</asp:ListItem>
                            <asp:ListItem Value="8">Multi Stream Model</asp:ListItem>
                        </asp:DropDownList>                      

                      </div>
                    </div>
                  </div>
        
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        PreEvent Recording
                           <asp:TextBox ID="TxtPreEventRecording" runat="server" PlaceHolder="Please enter PreEvent Recording" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        PostEvent Recording
                        <asp:TextBox ID="TxtPostEventRecording" runat="server" PlaceHolder="Please enter PostEvent Recording" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                      Device Location
                         <asp:Dropdownlist ID="DdlDevicelocation" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:Dropdownlist>
                      </div>
                    </div>
                  </div>
               
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        Device Site
                         <asp:Dropdownlist ID="DdlDeviceSite" runat="server" CssClass="form-control" Width="250px" Height="34px" ></asp:Dropdownlist>
                      </div>
                    </div>
                <div class="col-sm-4">
                      <div class="form-group">
                        Camera Templete
                        <asp:TextBox ID="TxtCamerTempleteName" runat="server" PlaceHolder="Please enter Camera Templete" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                      </div>
                    </div>
           </div> 
            
             <br /><hr />

            <div class="row">
                    <div class="col-sm-12">
                      <!-- text input -->
                      <div class="form-group">
                       <asp:CheckBoxList ID="chkEvents" runat="server" RepeatColumns="3" Width="100%">
                        <asp:ListItem Value="1"><span style="margin-left:10px"></span>Motion Detect Alert Window 1</asp:ListItem>
                        <asp:ListItem Value="2"><span style="margin-left:10px"></span>Motion Detect Alert Window 2</asp:ListItem>
                        <asp:ListItem Value="4"><span style="margin-left:10px"></span>Motion Detect Alert Window 3</asp:ListItem>
                        <asp:ListItem Value="256"><span style="margin-left:10px"></span>Digital Input Low 1</asp:ListItem>
                        <asp:ListItem Value="512"><span style="margin-left:10px"></span>Digital Input Low 2</asp:ListItem>
                        <asp:ListItem Value="1024"><span style="margin-left:10px"></span>Digital Input Low 3</asp:ListItem>
                        <asp:ListItem Value="2048"><span style="margin-left:10px"></span>Digital Input Low 4</asp:ListItem>
                        <asp:ListItem Value="65536"><span style="margin-left:10px"></span>Digital Input High 1</asp:ListItem>
                        <asp:ListItem Value="131072"><span style="margin-left:10px"></span>Digital Input High 2</asp:ListItem>
                        <asp:ListItem Value="262144"><span style="margin-left:10px"></span>Digital Input High 3</asp:ListItem>
                        <asp:ListItem Value="524288"><span style="margin-left:10px"></span>Digital Input High 4</asp:ListItem>
                        <asp:ListItem Value="2097152"><span style="margin-left:10px"></span>Digital Input Rising 1</asp:ListItem>
                        <asp:ListItem Value="4194304"><span style="margin-left:10px"></span>Digital Input Rising 2</asp:ListItem>
                        <asp:ListItem Value="8388608"><span style="margin-left:10px"></span>Digital Input Rising 3</asp:ListItem>
                        <asp:ListItem Value="16777216"><span style="margin-left:10px"></span>Digital Input Rising 4</asp:ListItem>
                        <asp:ListItem Value="33554432"><span style="margin-left:10px"></span>Digital Input Falling 1</asp:ListItem>
                        <asp:ListItem Value="67108864"><span style="margin-left:10px"></span>Digital Input Falling 2</asp:ListItem>
                        <asp:ListItem Value="134217728"><span style="margin-left:10px"></span>Digital Input Falling 3</asp:ListItem>
                        <asp:ListItem Value="268435456"><span style="margin-left:10px"></span>Digital Input Falling 4</asp:ListItem>
                    </asp:CheckBoxList>
                      </div>
                    </div>
                  
            </div>
              <br /><hr />
    <div class="row">
                    
                    <div class="col-md-4">
                        <div class="form-group">Event Recording Enabled</div>
                     </div>
                    <div class="col-md-4">
                         <div class="form-group">
                        <asp:CheckBox ID="chkEventEnabled" runat="server"/>
                        </div>
                    </div>

                    </div>
            <div class="row">
                    
                    <div class="col-md-12">
                        <div class="form-group">
                        
                        </div>
                     </div>
                    

                    </div>
            <br />
            <div class="row">
                <div class="col-sm-4">
                       <h3 class="card-title">File Uploads</h3>
                </div>
            </div>
            
            <br />
                <div class="row">
                      <div class="col-md-4">
                          <div class="form-group">Image No Response</div>
                      </div>
                    <div class="col-md-8">
                        <div class="form-group">
                             <asp:FileUpload ID="filImageError" runat="server" />
                        <asp:Image ID="imgError" runat="server" Height="50px" />
                            </div>
                    </div>
                  </div>
                       <br />
                <div class="row">
                    <div class="col-md-4"><div class="form-group">Image Normal</div></div>
                    <div class="col-md-8">
                        <div class="form-group">
                         <asp:FileUpload ID="filImageNormal" runat="server" />
                        <asp:Image ID="imgNormal" runat="server" Height="50px" />
                            </div>
                    </div>
                  </div>
                  <br />
                <div class="row">
                     <div class="col-md-4"><div class="form-group">Image Error</div></div>
                    <div class="col-md-8">
                    <div class="form-group">
                          <asp:FileUpload ID="filImageNoResponse" runat="server" />    
                         <asp:Image ID="imgResponse" runat="server" Height="50px" />
                    </div>
                    </div>
                  </div>
                    <br />
                
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        <asp:Button ID="btnSaveCamera" runat="server" Text="Save" Width="250px" Height="40px" class="btn bg-gray form-control" OnClick="btnCreate_Click" />
                      </div>
                    </div>
                  </div>
                   
    </div>
   
    </div>
</div>
    </div>
</asp:Content>
