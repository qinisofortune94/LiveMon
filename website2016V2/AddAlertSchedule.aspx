<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddAlertSchedule.aspx.cs" Inherits="website2016V2.AddAlertSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    
    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Scripts/keyboard.css" rel="stylesheet" />
    <script src="Scripts/jquery.keyboard.js" type="text/javascript"></script>
    <script src="Scripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="Scripts/jquery.keyboard.extension-typing.js" type="text/javascript"></script>
     <script src="js/jquery-ui-1.8.20.custom.min.js"  type="text/javascript"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js"  type="text/javascript"></script>

    <script src="Scripts/jsloadKeyBoard.js" type="text/javascript"></script>
    <script src="Scripts/jsloadTimePicker.js"  type="text/javascript"></script>


    <link href="Scripts/messages.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="stylesheet" />
    
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


  <%--  date--%>
       <style type="text/css" media="screen">
   
       
        
        /* slider specific CSS */
        .sliderGallery {
            background: url(http://static.jqueryfordesigners.com/demo/images/productbrowser_background_20070622.jpg) no-repeat;
            overflow: hidden;
            position: relative;
            padding: 10px;
            height: 160px;
            width: 560px;
        }
        
        .sliderGallery UL {
            position: absolute;
            list-style: none;
            overflow: none;
            white-space: nowrap;
            padding: 0;
            margin: 0;
        }
        
        .sliderGallery UL LI {
            display: inline;
        }
        
        .slider {
            width: 542px;
            height: 17px;
            margin-top: 140px;
            margin-left: 5px;
            padding: 1px;
            position: relative;
            background: url(http://static.jqueryfordesigners.com/demo/images/productbrowser_scrollbar_20070622.png) no-repeat;
        }
        
        .handle {
            position: absolute;
            cursor: move;
            height: 17px;
            width: 181px;
            top: 0;
            background: url(http://static.jqueryfordesigners.com/demo/images/productbrowser_scroller_20080115.png) no-repeat;
            z-index: 100;
        }
        
        .slider span {
            color: #bbb;
            font-size: 80%;
            cursor: pointer;
            position: absolute;
            z-index: 110;
            top: 3px;
        }
        
        .slider .slider-lbl1 {
            left: 50px;
        }
        
        .slider .slider-lbl2 {
            left: 107px;
        }
        
        .slider .slider-lbl3 {
            left: 156px;
        }

        .slider .slider-lbl4 {
            left: 280px;
        }

        .slider .slider-lbl5 {
            left: 455px;
        }
        .ui-timepicker-div .ui-widget-header { margin-bottom: 8px; }
.ui-timepicker-div dl { text-align: left; }
.ui-timepicker-div dl dt { height: 25px; margin-bottom: -25px; }
.ui-timepicker-div dl dd { margin: 0 10px 10px 65px; }
.ui-timepicker-div td { font-size: 90%; }
    -->
    </style>
    
     <script type="text/javascript" charset="utf-8">
        window.onload = function () {
            var container = $('div.sliderGallery');
            var ul = $('ul', container);

            var itemsWidth = ul.innerWidth() - container.outerWidth();
            //            $('#txtTimeMixed').datetimepicker();
            //            $('#txtExpiryTime').datetimepicker();
            $('.slider', container).slider({
                min: 0,
                max: itemsWidth,
                handle: '.handle',
                stop: function (event, ui) {
                    ul.animate({ 'left': ui.value * -1 }, 500);
                },
                slide: function (event, ui) {
                    ul.css('left', ui.value * -1);
                }
            });
        };
        function CheckSaveClick() {
            return (confirm("Are you sure you want to save?"))
        };
    </script>
    <%--end--%>
    <div class="card" style="font-size:13px">
    <div class="card-header">
   <h3 class="card-title">Alert Schedule</h3>
</div>
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
       
    <br />
<div class="card-body">
<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Add Alert Schedule</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
<div class="card-body">
            <div class="row">
                    <div class="col-sm-12">

                     <asp:GridView ID="Alertsgrid" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false" OnPreRender="GridView1_PreRender" OnRowCommand="gvSample_Commands">
                       <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="select" CommandName="SelectItem">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                   
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>--%>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" InsertVisible="False" ReadOnly="True"/>
                                <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Day"/>
                                <asp:BoundField DataField="Include Image" HeaderText="Include Image" SortExpression="StartTime"/>
                                <asp:BoundField DataField="Camera 1 ID" HeaderText="Camera 1 ID" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Camera 2 ID" HeaderText="Camera 2 ID" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Sensor 1 ID" HeaderText="Sensor 1 ID" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Camera 2 ID" HeaderText="Camera 2 ID" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Sensor 3 ID" HeaderText="Sensor 3 ID" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Sensor 4 ID" HeaderText="Sensor 4 ID" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Enabled" HeaderText="Enabled" SortExpression="EndTime"/>
                                <asp:BoundField DataField="SendNormal" HeaderText="SendNormal" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Delay1" HeaderText="Delay1" SortExpression="EndTime"/>
                                <asp:BoundField DataField="Delay2" HeaderText="Delay2" SortExpression="EndTime"/>
                            </Columns>
                     </asp:GridView>

                   </div>

            </div>
</div>
</div>
<div class="card" style="font-size:13px" id="accordion1">
    <div class="card-header">
          <h3 class="card-title">Selected Alert Schedule</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
            <div class="row">
                    <div class="col-sm-2">Alert:</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="ContactID" runat="server" ReadOnly="True" PlaceHolder="Please select alert Above" required="true" Enabled="false" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">Day:</div>
                    <div class="col-sm-4">
                        <asp:ListBox ID="Day" runat="server" CssClass="form-control" Width="250px" Height="34px" Rows="1">
                        <asp:ListItem Value="1">Monday</asp:ListItem>
                        <asp:ListItem Value="2">Tuesday</asp:ListItem>
                        <asp:ListItem Value="3">Wednesday</asp:ListItem>
                        <asp:ListItem Value="4">Thursday</asp:ListItem>
                        <asp:ListItem Value="5">Friday</asp:ListItem>
                        <asp:ListItem Value="6">Saturday</asp:ListItem>
                        <asp:ListItem Value="0">Sunday</asp:ListItem>
                        <asp:ListItem Value="7">EveryDay</asp:ListItem>
                        <asp:ListItem Value="8">WeekDays</asp:ListItem>
                         <asp:ListItem Value="9">WeekEnds</asp:ListItem>
                    </asp:ListBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2">Start Time:</div>
                    <div class="col-sm-4">
                        <span ID="SpanTimePicker">
                        <asp:TextBox ID="SchedStartTime" Placeholder="Enter Date" runat="server" CssClass="form-control" Width="250px" Height="34px">
                       
                    </asp:TextBox>
                            </span>
                    </div>
                    <div class="col-sm-2">End time:</div>
                    <div class="col-sm-4">
                        <span ID="SpanTimePicker1">  
                        <asp:TextBox ID="SchedEndTime"  runat="server" Placeholder="Enter Date" CssClass="form-control" Width="250px" Height="34px">
                       
                    </asp:TextBox>
                            </span>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                        <asp:Button ID="cmdSend" runat="server" Text="Schedule"  Width="250px" Height="40px" class="btn bg-gray form-control"  OnClick="cmdSend_Click" />
                         <asp:Label ID="errLbl" runat="server" ForeColor="Red" Visible="False" Width="520px"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                    </div>

                    <div class="col-sm-4">
                        <asp:Button ID="btnClearScheduleFields" runat="server" Text="Clear" Width="250px"   Height="40px" class="btn bg-gray form-control" OnClick="btnClearScheduleFields_Click"/>
                    </div>
                </div>
    </div>
</div>
<div class="card" style="font-size:13px" id="accordion2">
    <div class="card-header">
          <h3 class="card-title">Add Alert Schedule</h3>
          <div class="card-tools">
            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
          </div>
     </div>
    
<div class="card-body">
            <div class="row">
                    <div class="col-sm-12">
                     <asp:GridView ID="AlertsSchedgrid" CssClass="gvdatatable table table-striped table-bordered" runat="server"  AutoGenerateColumns="false" OnRowCommand="gvSample_Commands2">

                            <Columns>
                                <asp:TemplateField Visible="false">
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

                                <%--<asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>--%>
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" InsertVisible="False" ReadOnly="True"/>
                                <asp:BoundField DataField="Day" HeaderText="Day" SortExpression="Day"/>
                                <asp:BoundField DataField="StartTime" HeaderText="Start Time" SortExpression="StartTime"/>
                                <asp:BoundField DataField="EndTime" HeaderText="End Time" SortExpression="EndTime"/>

                            </Columns>
                     </asp:GridView>
                  </div>
    </div>
    </div>
</div>
</div>
   </div>
</asp:Content>
