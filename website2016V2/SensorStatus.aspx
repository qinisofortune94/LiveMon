<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SensorStatus.aspx.cs" Inherits="website2016V2.SensorStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <style>
        .table-bordered {
            border: 1px solid #dee2e6 !important;
            height: 40px !important;
            font-size: 13px !important;
        }

        label:not(.form-check-label):not(.custom-file-label) {
            font-weight: normal !important;
        }

        .custom-control-label {
            position: relative;
            margin-bottom: 0;
            vertical-align: top;
        }
    </style>
    <meta http-equiv="refresh" content="20" />

    <div class="card" style="font-size: 13px">
        <div class="card-body" style="display: block;">
            <div class="row" style="font-size: 13px">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="dashboard_graph">
                        <div class="row">
                            <div class="col-md-12 pull-right">
                                <div class="col-md-2 pull-right">
                                    <div class="form-group">
                                        <asp:DropDownList ID="cmbCurrentSite" runat="server" CssClass="pull-right form-control btn-round" AutoPostBack="true" Visible="false" ToolTip="Select Site to view ." OnSelectedIndexChanged="cmbCurrentSite_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-10 pull-right">
                                    <h6 class="pull-right">Selected Site : <small></small></h6>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">Sensor Status Display</h3>

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
            <div class="" style="font-size: 14px">
                <div class="col-md-12">
                    <div class="panel-success col-md-12 col-sm-6 col-xs-6">
                        <div class="panel panel-heading">
                            <div class="row">
                                <asp:CheckBox
                                    ID="chkFilter_OK"
                                    value="0"
                                    Checked="true"
                                    runat="server"
                                     AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">Ok</span>

                                <asp:CheckBox
                                    Style="margin-left: 10px !important; font-weight: normal !important"
                                    ID="chkFilter_Error"
                                    value="1"
                                    Checked="true"
                                    runat="server"
                                    AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">Critical Error</span>

                                <asp:CheckBox
                                    style="margin-left: 10px !important; font-weight: normal !important"
                                    ID="chkFilter_NoResponse"
                                    value="2"
                                    Checked="true"
                                    runat="server" AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">No Response</span>


                                <asp:CheckBox
                                    Style="margin-left: 10px !important; font-weight: normal !important"
                                    ID="chkFilter_Alert"
                                    value="3"
                                    Checked="true"
                                    runat="server"
                                     AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">Alert</span>


                                <asp:CheckBox
                                    Style="margin-left: 10px !important"
                                    ID="chkFilter_Warning"
                                    value="4"
                                    Checked="true"
                                    runat="server"
                                    AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">Error</span>


                                <asp:CheckBox
                                    Style="margin-left: 10px !important"
                                    ID="chkFilter_Unknown"
                                    value="5"
                                    Checked="true"
                                    runat="server"
                                    AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">Disabled</span>

                                <asp:CheckBox
                                    Style="margin-left: 10px !important"
                                    ID="chkFilter_SensorWarning"
                                    value="6"
                                    Checked="true"
                                    runat="server"
                                     AutoPostBack="True" ToolTip="Select Status to filter list." />
                                <span style="margin-left: 3px !important; font-weight: normal !important">Sensor Warning</span>

                                <asp:CheckBox
                                    Style="margin-left: 10px !important"
                                    ID="chkFilter_SensorAlert"
                                    value="7"
                                    Checked="false"
                                    runat="server"
                                    AutoPostBack="True" ToolTip="Select Status to filter list." />
                                 <span style="margin-left: 3px !important; font-weight: normal !important">Sensor Alert</span>

                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="export" runat="server" CssClass="btncls1 btn btn-sm" Style="background-color: #ced4da" Text="Export to excel" OnClick="export_Click" />
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btncls btn btn-sm" Style="background-color: #ced4da" Text="Filter" />
                                    <asp:Button
                                        ID="btnUpdate"
                                        runat="server"
                                        Text="Update" Visible="False" />

                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="txtbox form-control" Style="margin-right: 15px !important"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px">
                        <div class="panel-success col-md-12 col-sm-6 col-xs-6">
                            <div class="card-body table-responsive p-0" style="font-size: 13px">
                                <%=MyTable %>
                                <%--<div id="MyDiv" runat="server" class="panel-success col-md-12 col-sm-6 col-xs-6">
                                                    
                                                </div>--%>
                            </div>
                        </div>
                    </div>


                    <div id="divFilter" class="panel-success col-md-12 col-sm-6 col-xs-6">
                        <div class="panel-heading">
                            <asp:Button ID="btnNext20" runat="server" CssClass="next20 btn btn-sm" Style="background-color: #ced4da" Text="Next 20 >>>" />
                            <asp:Button ID="btnPrev20" runat="server" CssClass="Prev20 btn btn-sm" Style="background-color: #ced4da" Text="<<< Prev 20" />
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            <input type="hidden" id="StartNo" value="0" runat="server" />
                            <input type="hidden" id="EndNo" value="20" runat="server" />
                            <input type="hidden" id="MaxNo" value="0" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
