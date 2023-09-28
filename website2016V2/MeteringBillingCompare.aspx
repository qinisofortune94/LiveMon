<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeteringBillingCompare.aspx.cs" Inherits="website2016V2.MeteringBillingCompare" %>
<%@ Register assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebDataInput" tagprefix="igtxt" %>

<%@ Register assembly="nsoftware.IPWorksWeb" namespace="nsoftware.IPWorks" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
     <link href="css/messages.css" rel="stylesheet" />
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
    <h3>Metering</h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                         <asp:Label ID="Label1" runat="server" Text="Billing Compare"></asp:Label>
                        <asp:Label ID="lblAdd" runat="server" Text="Billing"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
               <div class="row">
                    <div class="col-md-2">Selected Meters 1:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList1" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px">
                            <asp:ListItem>------Select-----</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                     <div class="col-md-2">Tarrif 1:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList2" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px">
                               <asp:ListItem>------Select-----</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Selected Meters 2:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList3" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                     <div class="col-md-2">Tarrif 2:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList4" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                   <div class="col-md-2">Selected Meters 3:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList5" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                   
                    <div class="col-md-2">Tarrif 3:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTarrif" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
                <br />
            <asp:Panel ID="Panel1" runat="server" Border="1" BorderStyle="outset">
                   <div class="row">
                    <div class="col-md-2">Start Day:</div>
                    <div class="col-md-4" > 
                           <span ID="SpanTimePicker">                       
                      <asp:TextBox ID="txtStart" runat="server" PlaceHolder="Please enter Start Day" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                        </span>
                                </div>
                   <div class="col-md-2">End Day:</div>
                    <div class="col-md-4">
                           <span ID="SpanTimePicker">  
                        <asp:TextBox ID="txtEnd" runat="server"  PlaceHolder="Please enter End Day" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </span>
                               </div>
                </div>
                 <br />
                 <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" height ="40px" OnClick="cmdDateGenerate_Click"/>
                    </div>
                     </div>
                </asp:Panel>
                <br />
                
           
                         <div class="row">
                    <div class="col-md-2">Default Ranges:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRanges" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px">
                         <asp:ListItem>------Select-----</asp:ListItem>
                        <asp:ListItem Value="0">Last 30 Mins</asp:ListItem>
                        <asp:ListItem Value="1">Last Hour</asp:ListItem>
                        <asp:ListItem Value="2">Last 2 Hours</asp:ListItem>
                        <asp:ListItem Value="3">Last 3 Hours</asp:ListItem>
                        <asp:ListItem Value="4">Last 5 Hours</asp:ListItem>
                        <asp:ListItem Value="5">Last 10 Hours</asp:ListItem>
                        <asp:ListItem Value="6">Last 12 Hours</asp:ListItem>
                        <asp:ListItem Value="7">Last 24 Hours</asp:ListItem>
                        <asp:ListItem Value="8">Last 2 Days</asp:ListItem>
                        <asp:ListItem Value="9">Last 4 Days</asp:ListItem>
                        <asp:ListItem Value="10">Last Week</asp:ListItem>
                        <asp:ListItem Value="11">Last Month</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                     <div class="col-md-2"></div>
                    <div class="col-md-4">
                        <asp:Button ID="btnGenerate2" runat="server" Text="Generate" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" height ="40px" OnClick="CmdQuickGenerate_Click"/>
                    </div>
                             </div>
                    
         
                <div class="row">
                    <div class="col-md-2">Email Graphs:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtGraphs" runat="server" PlaceHolder="Please enter email"  CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Data Set:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlData" runat="server" required="true" CssClass="form-control" Width="250px" Height="34px">
                     <asp:ListItem Selected="True" Value="0">All Data</asp:ListItem>
                    <asp:ListItem Value="2">Every 2 Rows</asp:ListItem>
                    <asp:ListItem Value="3">Every 3 Rows</asp:ListItem>
                    <asp:ListItem Value="4">Every 4 Rows</asp:ListItem>
                    <asp:ListItem Value="5">Every 5 Rows</asp:ListItem>
                    <asp:ListItem Value="10">Every 10 Rows</asp:ListItem>
                    <asp:ListItem Value="20">Every 20 Rows</asp:ListItem>
                    <asp:ListItem Value="40">Every 40 Rows</asp:ListItem>
                    <asp:ListItem Value="60">Every 60 Rows</asp:ListItem>
                    <asp:ListItem Value="100">Every 100 Rows</asp:ListItem>
                    <asp:ListItem Value="200">Every 200 Rows</asp:ListItem>
                    <asp:ListItem Value="250">Every 250 Rows</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
             
            </div>
        </div>
    </div>
     <div class="panel panel-default" style="width: 100%">
    
         <div class="row">
                    
             <%--<div class="col-md-2">Charts1</div>--%>
                    <div class="col-md-4">
                         <div  ID="Charts1" runat="server" enableviewstate="true">
                    </div>
                    </div>
                <div class="col-md-1"></div>
                <div class="col-md-4">
                     <div  ID="DivTarrifReport" runat="server" enableviewstate="false">
                    </div>
                       </div>
             </div>
         <br />
         <div class="row">
                 <%--   <div class="col-md-1"></div>--%>
                    <div class="col-md-4">
                       <div  ID="Charts2" runat="server" enableviewstate="true">
                    </div>
                    </div>
              <div class="col-md-1"></div>
                    <div class="col-md-4">
                       <div  ID="DivTarrifReport1" runat="server" enableviewstate="false">
                    </div>
                    </div>
             </div>
         <br />
         <div class="row">
           <%--   <div class="col-md-1"></div>--%>
               <div class="col-md-4">
                         <div  ID="Charts" runat="server" enableviewstate="true">
                    </div>
                    </div>
            
           <div class="col-md-1"></div>
                    <div class="col-md-4">
                         <div  ID="DivTarrifReport2" runat="server" enableviewstate="false">
                    </div>
                    </div>
              </div>   
        </div>
              
        

    
  

</asp:Content>
