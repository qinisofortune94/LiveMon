<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportz.aspx.cs" Inherits="website2016V2.Reportz" %>

<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igchart" Namespace="Infragistics.WebUI.UltraWebChart" Assembly="Infragistics2.WebUI.UltraWebChart.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igchartprop" Namespace="Infragistics.UltraChart.Resources.Appearance" Assembly="Infragistics2.WebUI.UltraWebChart.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igchartdata" Namespace="Infragistics.UltraChart.Data" Assembly="Infragistics2.WebUI.UltraWebChart.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
      <link href="Scripts/messages.css" rel="stylesheet" />
     
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
     <style type="text/css" media="screen">
    <!--
       
        
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
    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui.css" rel="stylesheet" />
    
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Scripts/keyboard.css" rel="stylesheet" />
    <script src="Scripts/jquery.keyboard.js" type="text/javascript"></script>
    <script src="Scripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="Scripts/jquery.keyboard.extension-typing.js" type="text/javascript"></script>
     <script src="Scripts/jquery-ui-1.8.20.custom.min.js"  type="text/javascript"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js"  type="text/javascript"></script>

    <script src="Scripts/jsloadKeyBoard.js" type="text/javascript"></script>
    <script src="Scripts/jsloadTimePicker.js"  type="text/javascript"></script>
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

    <h3>Reports</h3>

    <div class="col-md">
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
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Reports"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <asp:Panel ID="panel1" runat="server" BorderStyle="Ridge">
                    <br />
                    <div class="row">
                        <div class="col-md-2">Selected Sensor:</div>
                        <div class="col-md-4">
                            <asp:Panel ID="pnlTreeView" runat="server" BorderStyle="Ridge" Width="300px">
                                <asp:TreeView ID="tvSensors" runat="server" AutoPostBack="True" required="true">                                
                                </asp:TreeView>
                            </asp:Panel>
                        </div>

                        <div class="col-md-2">Select Report:</div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlReportType" runat="server" required="true" Width="250px" Height="34px" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="0">Plain HTML Tables</asp:ListItem>
                                <asp:ListItem Value="1">Max Min Avg</asp:ListItem>
                                <asp:ListItem Value="2">Max Min Avg Trend Text</asp:ListItem>
                                <asp:ListItem Value="3">Max Min Avg Trend Graph</asp:ListItem>
                                <asp:ListItem Value="4">UP-Time Graph</asp:ListItem>
                                <asp:ListItem Value="5">Power Supply Graph</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDailySetting" runat="server" required="true" Width="250px" Height="34" Visible="false" AutoPostBack="true">
                                <asp:ListItem Selected="True" Value="0">Daily 6-6</asp:ListItem>
                                <asp:ListItem Value="1">Daily 12-12</asp:ListItem>
                                <asp:ListItem Value="2">12 Hourly</asp:ListItem>
                                <asp:ListItem Value="3">Hourly</asp:ListItem>
                                <asp:ListItem Value="4">Weekly</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                </asp:Panel>

                <asp:Panel ID="panel2" runat="server" BorderStyle="Ridge">
                    <br />
                    <div class="row">
                        <div class="col-md-2">Start Day:</div>
                        <div class="col-md-4">
                             <span ID="SpanTimePicker">  
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                 </span>
                        </div>
                        <div class="col-md-2">End Day:</div>
                        <div class="col-md-4">
                             <span ID="SpanTimePicker">  
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                </span>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnGenerate_Click" />
                        </div>
                        <div class="col-md-2">Export Raw Data: </div>
                        <div class="col-md-4">
                            <asp:CheckBox ID="chkRawData" runat="server" AutoPostBack="True" ToolTip="This option will prompt to save a raw csv file rather than showing on the page." />
                        </div>

                    </div>
                </asp:Panel>

            </div>
        </div>

        <div id="ReportzSection" runat="server" enableviewstate="false" style="width: 100%"></div>
        <div id="Charts" runat="server" enableviewstate="false" style="width: 100%"></div>

    </div>

    <script type="text/javascript">

        Calendar.setup(
            {
                inputField: "txtStartDate", // ID of the input field
                ifFormat: "%m/%d/%Y %l:%M:%S %p", // the date format setDateFormat("%Y.%m.%d %H:%M");
                button: "trigger", // ID of the button
                showsTime: true

            }
        );
        Calendar.setup(
            {
                inputField: "txtEndDate", // ID of the input field
                ifFormat: "%m/%d/%Y %l:%M:%S %p", // the date format
                button: "trigger1", // ID of the button
                showsTime: true
            }
        );
    </script>

</asp:Content>
