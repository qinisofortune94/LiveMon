<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEditDashboard.aspx.cs" Inherits="website2016V2.UserEditDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <div class="">
                         <div id="DivSuccess"></div>
                        <div class="alert alert-info" id="successMessage" runat="server" style="width:100%">
                            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="page-title">
                            <div class="title_left">
                                <h3>Edit Dashboard Settings</h3><br />
                            </div>
                            <div class="title_right">
                                <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right">
                                   
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Settings</h2>
                                         
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label8" CssClass="control-label" runat="server" Text="User Dashboard"></asp:Label>
                                            <asp:DropDownList ID="LoadUserDash" AutoPostBack="true" runat="server" CssClass="form-control col-md-4 col-sm-12 col-xs-12" OnSelectedIndexChanged="LoadUserDash_SelectedIndexChanged"></asp:DropDownList>
                                        </div><br /><br /><br /><br />

                                        <div class="col-md-12">
                                            <asp:GridView ID="gridDashboards" runat="server" CssClass="gvdatatable table table-striped table-bordered" OnSelectedIndexChanged="gridDashboards_SelectedIndexChanged" OnPageIndexChanging="gridDashboards_PageIndexChanging"
                                                CellPadding="4" ForeColor="#333333" Width="100%" AutoGenerateSelectButton="True" AllowPaging="True" PageSize="5" AutoGenerateColumns="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="DashboardId" HeaderText="No" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                    <asp:BoundField DataField="Row" HeaderText="Row" />
                                                    <asp:BoundField DataField="Position" HeaderText="Position" />
                                                    <asp:BoundField DataField="Graph" HeaderText="Graph" />
                                                    <asp:BoundField DataField="Sensor" HeaderText="Sensor" />
                                                    <asp:BoundField DataField="Field" HeaderText="Field" />
                                                    <asp:BoundField DataField="DataHours" HeaderText="Hours" />
                                                    <asp:BoundField DataField="ParameterId" HeaderText="FieldRefNo" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divEditDashboard" runat="server" class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Edit Settings</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label2" CssClass="control-label" runat="server" Text="Description"></asp:Label>
                                                    <asp:TextBox ID="txtGraphName" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label3" CssClass="control-label" runat="server" Text="Row"></asp:Label>
                                                    <asp:TextBox ID="txtRowsPos" runat="server" TextMode="Number" min="1" max="2" step="1" Text="0" CssClass="form-control">
                                                    </asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label4" CssClass="control-label" runat="server" Text="Row Position"></asp:Label>
                                                     <asp:TextBox ID="txtColsPos" runat="server" TextMode="Number" min="1" max="3" step="1" Text="0" CssClass="form-control">
                                                     </asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label5" CssClass="control-label" runat="server" Text="Graph"></asp:Label>
                                                    <asp:DropDownList ID="drpChartTypes" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label1" CssClass="control-label" runat="server" Text="Sensor"></asp:Label>
                                                    <asp:DropDownList ID="drpSensor" runat="server" OnSelectedIndexChanged="drpSensor_SelectedIndexChanged" CssClass="form-control" EnableViewState="true" AutoPostBack="true"></asp:DropDownList>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label6" CssClass="control-label" runat="server" Text="Field"></asp:Label>
                                                    <asp:DropDownList ID="drpField" runat="server" CssClass="form-control" ></asp:DropDownList>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label7" runat="server" CssClass="control-label" Text="Data Hours"></asp:Label>
                                                    <asp:TextBox ID="txtDataHours" runat="server" TextMode="Number" min="1" max="168" step="1" Text="0" CssClass="form-control">
                                                    </asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label11" CssClass="control-label" runat="server" Text="Remove?"></asp:Label>
                                                    <asp:CheckBox ID="chkDelete" runat="server" Checked="false" />
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-8 col-md-offset-1">
                                                    <asp:Button ID="buttonSave" CssClass="btn btn-success" Width="120px" runat="server" Text="Save Settings" OnClick="buttonSave_Click" />                                                      
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
</asp:Content>
