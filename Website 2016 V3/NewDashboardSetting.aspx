<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewDashboardSetting.aspx.cs" Inherits="website2016V2.NewDashboardSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <div class="">
                         <div id="DivSuccess"></div>
                        <div class="alert alert-info" id="successMessage" runat="server">
                            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="page-title">
                            <div class="title_left">
                                <h3>New Dashboard Settings</h3><br />
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
                                        <h2>Add New Settings</h2>
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
                                                    <asp:DropDownList ID="drpSensor" runat="server" OnSelectedIndexChanged="drpSensor_SelectedIndexChanged" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                                </div><br /><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="form-group top_search">
                                                            <div class="input-group">
                                                                <asp:DropDownList ID="drpField" runat="server" CssClass="form-control" ></asp:DropDownList><span class="input-group-btn"><asp:Button ID="btnnAdd" runat="server" Text="Add Field" CssClass="btn btn-primary" OnClick="btnnAdd_Click" /></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                             </div>

                                            <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:Label ID="Label7" runat="server" CssClass="control-label" Text="Data Hours"></asp:Label>
                                                    <asp:TextBox ID="txtDataHours" runat="server" TextMode="Number" min="1" max="168" step="1" Text="0" CssClass="form-control">
                                                    </asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div id="gr" runat="server" class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <asp:GridView ID="gridDashboards" runat="server"
                                                        CellPadding="4" ForeColor="#333333" OnSelectedIndexChanged="gridDashboards_SelectedIndexChanged" Width="100%" AllowPaging="True" PageSize="3" AutoGenerateColumns="False">
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:CommandField SelectText="Remove" ShowSelectButton="True" />
                                                            <asp:BoundField DataField="FieldId" HeaderText="No" />
                                                            <asp:BoundField DataField="Field" HeaderText="Field" />
                                                        </Columns>
                                                        <EditRowStyle BackColor="#2461BF" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#EFF3FB" />
                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                    </asp:GridView>
                                                </div><br /><br /><br /><br />
                                            </div><br /><br />

                                            
                                            <hr />
                                            <div class="form-group">
                                                <div class="col-md-8 col-md-offset-1">
                                                    <asp:Button ID="btnnSave" CssClass="btn btn-success" Width="120px" runat="server" Text="Save Settings" OnClick="btnnSave_Click" />                                                      
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
</asp:Content>
