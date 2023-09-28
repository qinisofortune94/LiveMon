<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeviceDisplay.aspx.cs" Inherits="website2016V2.DeviceDisplay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <meta http-equiv="refresh" content="30" />
    <script type="text/javascript">
        function SetSource(SourceID) {
            var hidSourceID =
                document.getElementById("<%=hidSourceID.ClientID%>");
            hidSourceID.value = SourceID;
        }
        function __doPostBack(eventTarget, eventArgument) {
            if (!form1.onsubmit || (form1.onsubmit() != false)) {
                form1.__EVENTTARGET.value = eventTarget;
                form1.__EVENTARGUMENT.value = eventArgument;
                form1.submit();
            }
        }


        function ShowMenu(control, e) {
            var posx = e.clientX + window.pageXOffset + 'px'; //Left Position of Mouse Pointer
            var posy = e.clientY + window.pageYOffset + 'px'; //Top Position of Mouse Pointer
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.display = 'inline';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;
        }
        function HideMenu(control) {

            document.getElementById(control).style.display = 'none';
        }
        function ReceiveServerData(arg, context) {
            //document.getElementById('MyDiv').innerHTML = arg;
            //what 2 do
        }


    </script>
    <style type="text/css">
        .ContextItem {
            background-color: White;
            color: Black;
            font-weight: normal;
        }

            .ContextItem:hover {
                background-color: #0066FF;
                color: White;
                font-weight: bold;
            }

        .detailItem {
            background: transparant;
        }

            .detailItem:hover {
                background-color: #FEE378;
                border: 1px outset #222222;
                font-weight: bold;
                cursor: default;
            }
    </style>

    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title"> 
               Device Status
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
                <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
        <div class="card-body" style="display: block;">
            
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


            <input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
                <input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
                <asp:HiddenField ID="hidSourceID" runat="server" />
                <div>
                    <div class="panel-success col-md-12 col-sm-6 col-xs-6">
                        <div class="panel panel-heading">
                            <asp:CheckBoxList ID="chkFilter" style="font-weight:400" runat="server" RepeatDirection="Horizontal" Width="855px" AutoPostBack="True" OnSelectedIndexChanged="chkFilter_SelectedIndexChanged" ToolTip="Select Status to filter list.">
                                <asp:ListItem Selected="false" Value="0">&nbsp;Ok</asp:ListItem>
                                <asp:ListItem Selected="false" Value="1">&nbsp;Error</asp:ListItem>
                                <asp:ListItem Selected="false" Value="2">&nbsp;No Response</asp:ListItem>
                                <asp:ListItem Selected="false" Value="3">&nbsp;STD Alert</asp:ListItem>
                                <asp:ListItem Selected="false" Value="4">&nbsp;Sensor Alert</asp:ListItem>
                                <asp:ListItem Selected="false" Value="5">&nbsp;Sensor Warning</asp:ListItem>
                                <asp:ListItem Selected="false" Value="6">&nbsp;Disabled</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>
                <div>
                    <div class="panel-success col-md-12 col-sm-6 col-xs-6">
                        <div class="panel panel-heading">
                            <div class="input-group">
                                <div class="col-md-6 col-sm-5 col-xs-6 form-group top_search">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtFilterName" placeholder="Enter sensor name" CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox><span class="input-group-btn"><asp:Button ID="btnFilter" OnClick="btnFilter_Click" runat="server" class="btn bg-gray" Text="Filter" /></span>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <asp:Label ID="Label1" runat="server" Text="Site" Style="background-color: #FFFF66"></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text="Group" Style="background-color: #FF3399; margin-left: 5%!important"></asp:Label>
                                <asp:Label ID="Label3" runat="server" Text="Device" Style="background-color: #CCFF99; margin-left: 5%!important"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="Sensor" Style="background-color: #3366FF; margin-left: 5%!important"></asp:Label>
                                <%--<asp:Timer ID="tmrRefresh" runat="server" Interval="130000">
                                </asp:Timer>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="margin-left: 15px; margin-top: 30px!important">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>

                    <asp:UpdatePanel ID="displayGroupsUpdatePanel" runat="server">
                        <ContentTemplate>
                            <div id="displayGroups" runat="server">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
</asp:Content>
