<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SensorStatus.aspx.cs" Inherits="website2016V2.SensorStatus" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    
    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="dashboard_graph">
                                <div class="row x_title">
                                    <div class="col-md-12 pull-right">
                                        <div class="col-md-2 pull-right">
                                            <div class="form-group">
                                                <asp:DropDownList ID="cmbCurrentSite" runat="server" CssClass="pull-right form-control btn-round" AutoPostBack="true" Visible="false" ToolTip="Select Site to view ." OnSelectedIndexChanged="cmbCurrentSite_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>    
                                        </div>
                                        <div class="col-md-10 pull-right">
                                            <h3 class="pull-right">Selected Site : <small></small></h3>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </div><br />

                    <div class="">
                        <div class="clearfix"></div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Status Display</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                            <div class="panel-success col-md-12 col-sm-6 col-xs-6">
                                                <div class="panel panel-heading">
                                                            <asp:CheckBox
                                                                ID="chkFilter_OK"
                                                                value="0"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="Ok"  AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                            <asp:CheckBox
                                                                ID="chkFilter_Error"
                                                                value="1"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="Critical Error"  AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                            <asp:CheckBox
                                                                ID="chkFilter_NoResponse"
                                                                value="2"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="No Response"  AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                            <asp:CheckBox
                                                                ID="chkFilter_Alert"
                                                                value="3"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="Alert"  AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                            <asp:CheckBox
                                                                ID="chkFilter_Warning"
                                                                value="4"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="Error" AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                            <asp:CheckBox
                                                                ID="chkFilter_Unknown"
                                                                value="5"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="Disabled"  AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                             <asp:CheckBox
                                                                ID="chkFilter_SensorWarning"
                                                                value="6"
                                                                Checked="true"
                                                                runat="server"
                                                                Text="Sensor Warning"  AutoPostBack="True" ToolTip="Select Status to filter list."/>
                                                             <asp:CheckBox
                                                                ID="chkFilter_SensorAlert"
                                                                value="7"
                                                                Checked="false"
                                                                runat="server"
                                                                Text="Sensor Alert"  AutoPostBack="True" ToolTip="Select Status to filter list."/>

                                                            <asp:Button ID="export" runat="server" CssClass="btncls1 btn btn-sm btn-success" Text="Export to excel" OnClick="export_Click"/>
                                                            <asp:Button ID="btnFilter" runat="server" CssClass="btncls btn btn-sm btn-success" Text="Filter" />
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="txtbox"></asp:TextBox>
                        
                                                            <br />
                                                            <asp:Button
                                                                ID="btnUpdate"
                                                                runat="server"
                                                                Text="Update" Visible="False" />
                                                     </div>
                                                </div>
                                            <div class="panel-success col-md-12 col-sm-6 col-xs-6">
                                                 <%=MyTable %> 
                                                <%--<div id="MyDiv" runat="server" class="panel-success col-md-12 col-sm-6 col-xs-6">
                                                    
                                                </div>--%>
                                            </div>
                                            
                                            <div id="divFilter" class="panel-success col-md-12 col-sm-6 col-xs-6">
                                                    <div class="panel-heading">
                                                        <asp:Button ID="btnNext20" runat="server" CssClass="next20 btn btn-sm btn-success" Text="Next 20 >>>" />
                                                        <asp:Button ID="btnPrev20" runat="server" CssClass="Prev20 btn btn-sm btn-success" Text="<<< Prev 20" />
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
                        </div>
                    </div>
</asp:Content>