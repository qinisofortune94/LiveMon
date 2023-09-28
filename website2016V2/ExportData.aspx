<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExportData.aspx.cs" Inherits="website2016V2.ExportData" %>
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

    <h3>Export Data</h3>

    <div class="col-md">
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
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Export Data"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                 <asp:Panel ID="panel1" runat="server" BorderStyle="Ridge" ><br />
                <div class="row">
                    <div class="col-md-2">Sensor:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSensors" runat="server" Width="250px" Height="34px" AutoPostBack="True"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">Data Format:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDataFormat" runat="server" Width="250px" Height="34px" AutoPostBack="True"></asp:DropDownList>
                    </div>
                </div>
                     </asp:Panel>

                <asp:Panel ID="panel2" runat="server" BorderStyle="Ridge" ><br />
                <div class="row">
                    <div class="col-md-2">Start Date:</div>
                    <div class="col-md-4">
                        <span ID="SpanTimePicker">
                        <asp:TextBox ID="txtStartDate" runat="server" PlaceHolder="Please enter start date" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </span>
                    </div>
                    <div class="col-md-2">End Date:</div>
                    <div class="col-md-4">
                        <span ID="SpanTimePicker">
                        <asp:TextBox ID="txtEndDate" runat="server"  PlaceHolder="Please enter end date" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </span>
                        <asp:Button ID="btnExport" runat="server" Text="Export" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnExport_Click"/>
                    
                    </div>    
              </div>
            </asp:Panel>
               
            </div>
        </div>
    </div>

</asp:Content>
