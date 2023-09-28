<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewVideo.aspx.cs" Inherits="website2016V2.NewVideo" %>

<%--<%@ Register TagPrefix="ig_sched" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebSchedule.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>--%>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    
    <link href="css/messages.css" rel="stylesheet" />

    
    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui.css" rel="stylesheet" />

    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Scripts/keyboard.css" rel="stylesheet" />
    <script src="Scripts/jquery.keyboard.js" type="text/javascript"></script>
    <script src="Scripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="Scripts/jquery.keyboard.extension-typing.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-timepicker-addon.js" type="text/javascript"></script>

    <script src="Scripts/jsloadKeyBoard.js" type="text/javascript"></script>
    <script src="Scripts/jsloadTimePicker.js" type="text/javascript"></script>

     <script language="javascript">

        function CmdExport_onclick() {
            if (document.getElementById('MediaPlayBack2')) {
                document.MediaPlayBack2.AVIFileName = document.getElementById('File1').value;
                document.MediaPlayBack2.StartAVIConversion();
            }
        }
        function CmdStop_onclick() {
            if (document.getElementById('MediaPlayBack2')) {
                document.MediaPlayBack2.Pause();
            } 
        }
        function CmdPlay_onclick() {
            if (document.getElementById('MediaPlayBack2')) {
                document.MediaPlayBack2.Resume();
            } 
        }
        function CmdCapture_onclick() {
            if (document.getElementById('MediaPlayBack2')) {
                document.MediaPlayBack2.SaveSnapshot(1, document.getElementById('FileExp').value);
            } 
        }
        function CmdChooseAVI_onclick() {
            if (document.getElementById('MediaPlayBack2')) {
                document.MediaPlayBack2.ChooseAVIVideoCompressor('IPMon Choose Compression');
            } 
        }
        function CmdNextFrame_onclick() {
            if (document.getElementById('MediaPlayBack2')) {
                document.MediaPlayBack2.NextFrame();
            } 
        }
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

        .ui-timepicker-div .ui-widget-header {
            margin-bottom: 8px;
        }

        .ui-timepicker-div dl {
            text-align: left;
        }

            .ui-timepicker-div dl dt {
                height: 25px;
                margin-bottom: -25px;
            }

            .ui-timepicker-div dl dd {
                margin: 0 10px 10px 65px;
            }

        .ui-timepicker-div td {
            font-size: 90%;
        }
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

    <h3>New video</h3>

    <input type="hidden" id="CurDate" value='' runat="server" />

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
                        <asp:Label ID="lblAdd" runat="server" Text="New video"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Select Camera:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCamera" runat="server" required="true" Width="250px" Height="34px" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>

                <asp:Panel ID="panel1" runat="server" BorderStyle="Ridge">
                    <br />
                    <div class="row">
                        <div class="col-md-2">Start Date:</div>
                        <div class="col-md-4">
                            <span id="SpanTimePicker">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox></span>
                        </div>
                        <div class="col-md-2">End Date:</div>
                        <div class="col-md-4">
                            <span id="SpanTimePicker">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox></span>

                            <asp:Button ID="btnGetFiles" runat="server" Text="Get Files" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnGetFiles_Click" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="panel2" runat="server" BorderStyle="Ridge">
                    <br />
                    <div class="row">
                        <div class="col-md-2">Available Video Files:</div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFiles" runat="server" Width="250px" Height="34px" AutoPostBack="true"></asp:DropDownList>
                        </div>

                        <div class="col-md-2">Normal:</div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSpeed" runat="server" Width="120px" AutoPostBack="true">
                                <asp:ListItem Selected="True" Value="0">Normal</asp:ListItem>
                                <asp:ListItem Value="1">2X</asp:ListItem>
                                <asp:ListItem Value="2">3X</asp:ListItem>
                                <asp:ListItem Value="3">4X</asp:ListItem>
                                <asp:ListItem Value="4">5X</asp:ListItem>
                                <asp:ListItem Value="5">6X</asp:ListItem>
                                <asp:ListItem Value="6">1/2</asp:ListItem>
                                <asp:ListItem Value="7">1/3</asp:ListItem>
                                <asp:ListItem Value="8">1/4</asp:ListItem>
                                <asp:ListItem Value="9">1/5</asp:ListItem>
                                <asp:ListItem Value="10">1/6</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2">Size:</div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSize" runat="server" Width="120px" AutoPostBack="true">
                                <asp:ListItem Value="0">100X100</asp:ListItem>
                                <asp:ListItem Value="1">200X200</asp:ListItem>
                                <asp:ListItem Value="2">300X300</asp:ListItem>
                                <asp:ListItem Value="3" Selected="True">400X400</asp:ListItem>
                                <asp:ListItem Value="4">500X500</asp:ListItem>
                                <asp:ListItem Value="5">600X600</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnPlaySelection" runat="server" Text="Play Selection" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div id="div2" runat="server"></div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnPause" runat="server" Text="Pause" Size="Small" />
                            <asp:Button ID="btnPlay" runat="server" Text="Play" Size="Small"/>
                            <asp:Button ID="btnNext" runat="server" Text=">" Size="Small"/>
                        </div>
                        <div class="col-md-2">Frame Export:</div>
                        <div class="col-md-4">
                            <asp:FileUpload ID="fuFrameExport" runat="server" Width="250px" Height="34px"></asp:FileUpload>
                        </div>

                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:HyperLink ID="DirectLink" runat="server" Visible="False">DirectLink</asp:HyperLink>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnCapture" runat="server" Text="Capture Frame" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF"/>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">Video Export:</div>
                        <div class="col-md-4">
                            <asp:FileUpload ID="fuVideoExport" runat="server" Width="250px" Height="34px" alue='c:\NewVideo.avi'></asp:FileUpload>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-2">Video Compression:</div>
                        <div class="col-md-4">
                    <asp:Button ID="btnVideoCompression" runat="server" Text="Video Compression"  Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF"/>
                      </div>

                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnChooseAVI" runat="server" Text="Export Video" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF"/>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>
