<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SensorDetails.aspx.cs" Inherits="website2016V2.SensorDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <div class="">
                        <div class="page-title">
                            <div class="title_left">
                                <h3>Sensor Dashboard Display</h3><br />
                            </div>
                            <div class="title_right">
                                <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right">
                                    <div class="input-group">
                                        <asp:Button ID="editTest" CssClass="btn btn-info" runat="server" Text="Edit Sensor/Test Sensor Readings" ToolTip="Edit Sensor/Test Sensor readings" OnClick="editTest_Click"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <asp:Panel ID="sensorDetails" runat="server">
                        <div class="row">
                            <div class="col-md-3 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Settings</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor ID <span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtSensorID" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor Name<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtSensorName" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor Group<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtSensorGroup" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor Scan Rate<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtScanRate" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Maximum Value<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtMaxValue" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Minimum Value<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtMinValue" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-9 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Graph</h2>
                                        <asp:DropDownList ID="dropRange" runat="server" CssClass="marg" Width="168px" OnSelectedIndexChanged="dropRange_SelectedIndexChanged" AutoPostBack="true">
                                             <asp:ListItem Value="0">Last 30 Mins</asp:ListItem>
                                             <asp:ListItem Value="1">Last Hour</asp:ListItem>
                                             <asp:ListItem Value="2">Last 5 Hours</asp:ListItem>
                                        </asp:DropDownList>
                                        <ul class="nav navbar-right panel_toolbox"> 
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                              <asp:Literal id="chrtMyChart" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Field</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="form-group">
                                            <div class="col-md-3 col-sm-6 col-xs-6">
                                                <label class="control-label" for="first-name">Type <span class="required"></span>
                                                </label>
                                                <asp:DropDownList ID="cmbType2" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div><br />
                                        </div><br /><br />

                                        <div class="form-group">
                                            <div class="col-md-12 col-sm-6 col-xs-6">
                                                <asp:GridView ID="cmbFields2" runat="server"
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnPageIndexChanging="cmbFields_PageIndexChanging" OnDataBinding="cmbFields_DataBinding"
                                                    AllowPaging="True" AutoGenerateEditButton="True" PageSize="5" OnRowCancelingEdit="cmbFields_RowCancelingEdit" OnRowUpdating="cmbFields_RowUpdating" OnRowEditing="cmbFields_RowEditing" 
                                                    ViewStateMode="Enabled" OnSelectedIndexChanged="cmbType_SelectedIndexChanged">
                                                    <AlternatingRowStyle BackColor="White" />
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
                                            </div><br />
                                        </div><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                        </asp:Panel>
                    </div>
                    <%--<asp:HiddenField ID="txtID2" runat="server" />--%>
                    <asp:TextBox ID="alert" Visible="false" runat="server" Height="97px" Width="100%"></asp:TextBox>
</asp:Content>