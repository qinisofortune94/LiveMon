<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddAlertThreshHolds.aspx.cs" Inherits="website2016V2.AddAlertThreshHolds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <div class="">
                        <div class="page-title">
                                <div class="title_left">
                                    <h3>Add Alert ThreshHolds Form</h3><br />
                                </div><br />

                                <div class="title_right">
                                    <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
                                        <div class="input-group">
                                        </div>
                                    </div>
                                </div>
                         </div>
                         <div class="clearfix"></div>
                         <div class="row">
                            <div class="alert alert-success" id="successMessage"  runat="server">
                                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
                            </div>
                            <div class="alert alert-warning" id="warningMessage"  runat="server">
                                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
                            </div>
                            <div class="alert alert-danger" id="errorMessage"  runat="server">
                                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
                                <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" Width="776px"></asp:Label>
                            </div>
                             <div class="col-md-6 col-sm-12 col-xs-12">
                                  <div class="x_panel">
                                     <div class="x_title">
                                        <h2>Add ThreshHold</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                     </div>
                                     <div class="x_content">
                                         <div class="col-md-12">
                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Alert ID <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtAlertID" CssClass="form-control col-md-7 col-xs-12" runat="server" ReadOnly="True"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">TreshHold Name <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtName" CssClass="form-control col-md-7 col-xs-12" runat="server"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <label class="control-label" for="first-name">Sensor ID <span class="required">*</span>
                                                    </label>
                                                    <asp:DropDownList ID="cmbSensorID" CssClass="form-control col-md-7 col-xs-12" runat="server" Width="225px" AutoPostBack="True" Height="35px" OnSelectedIndexChanged="cmbSensorID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <label class="control-label" style="margin-left:70px" for="first-name"> <span class="required"></span>
                                                    </label>
                                                    <asp:DropDownList ID="cmbDeviceID" Visible="false" CssClass="form-control col-md-7 col-xs-12 leaveSpace" runat="server" Width="247px" AutoPostBack="True" Height="35px">
                                                    </asp:DropDownList>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-4 col-sm-6 col-xs-6">
                                                    <label class="control-label" for="first-name">Field <span class="required">*</span>
                                                    </label>
                                                    <asp:DropDownList ID="cmbField" CssClass="form-control col-md-7 col-xs-12" runat="server" Width="222px" AutoPostBack="True" Height="35px">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="cmbFieldComp" CssClass="form-control col-md-7 col-xs-12" runat="server" Height="27px" Visible="False" Width="247px">
                                                    </asp:DropDownList>
                                                </div><br />
                                             </div><br /><br />

                                              <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Tabular Row <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtTabularCnt0" CssClass="form-control col-md-3 col-xs-12" TextMode="Number" runat="server" Text="0" ToolTip="The row of tabular values to check negative checks all rows of this field"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                              <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Check Value <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtCheckValue" CssClass="form-control col-md-7 col-xs-12" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Tab Count Value <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtTabularCnt" CssClass="form-control col-md-7 col-xs-12" runat="server" TextMode="Number" Text="0"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                              <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Hold Period Before Triggering <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtHoldPeriod" runat="server" CssClass="form-control col-md-7 col-xs-12" MaxValue="999" MinValue="0" TextMode="Number" Text="0"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Comparison to other thresh holds <span class="required">*</span>
                                                    </label>
                                                     <asp:RadioButtonList CssClass="col-md-7 col-xs-12" ID="Comparison" runat="server" RepeatColumns="2" Width="384px">
                                                        <asp:ListItem Selected="True" Value="0">And</asp:ListItem>
                                                        <asp:ListItem Value="1">Or</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div><br />
                                            </div><br /><br />

                                              <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <label class="control-label" for="first-name">Order of Comparison <span class="required">*</span>
                                                    </label>
                                                    <asp:TextBox id="txtOrder" runat="server" CssClass="form-control col-md-7 col-xs-12" MaxValue="9999" MinValue="0" TextMode="Number" Text="0"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br /><br />

                                            <div class="ln_solid"></div>

                                             <div class="form-group">
                                                <div class="col-md-8 col-md-offset-3">
                                                    <asp:Button id="btnnSend" runat="server" CssClass="btn btn-success" text="Save" width="90px" OnClick="btnnSend_Click">
                                                    </asp:Button>
                                                    <asp:Button id="btnnFinnished" runat="server" CssClass="btn btn-success" text="Finished" width="90px" OnClick="btnnFinnished_Click">
                                                    </asp:Button>
                                                </div>
                                            </div>
                                         </div>
                                     </div>
                                  </div>
                             </div>
                             <div class="col-md-6 col-sm-12 col-xs-12">
                                  <div class="x_panel">
                                     <div class="x_title">
                                        <h2>Test Type</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                     </div>
                                     <div class="x_content">
                                         <asp:RadioButtonList ID="TestType" runat="server" RepeatColumns="2" Width="100%" AutoPostBack="True">
                                         </asp:RadioButtonList><br /><br />
                                          <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                     <asp:Label ID="lblExtra" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra String"></asp:Label>
                                                    <asp:TextBox id="TxtExtra" CssClass="form-control col-md-7 col-xs-12" runat="server"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                   <asp:Label ID="lblExtra1" CssClass="control-label" Font-Bold="true" runat="server" Text="Extra String 1"></asp:Label>
                                                    <asp:TextBox id="TxtExtra1" CssClass="form-control col-md-7 col-xs-12" runat="server"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                     <asp:Label ID="lblExtra2" CssClass="control-label" Font-Bold="true" runat="server" Text="Must Occure (Hours)"></asp:Label>
                                                    <asp:TextBox id="TxtExtra2" CssClass="form-control col-md-7 col-xs-12" Text="0" TextMode="Number" runat="server"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <asp:Label ID="lblExtra3" CssClass="control-label" Font-Bold="true" runat="server" Text="Extra String 3"></asp:Label>
                                                    <asp:TextBox id="TxtExtra3" CssClass="form-control col-md-7 col-xs-12" Text="0" TextMode="Number" runat="server"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                             <div class="form-group">
                                                <div class="col-md-8 col-sm-6 col-xs-12">
                                                    <asp:CheckBox ID="chkSensAlertTemplate" runat="server" Text="Alert Template" />
                                                </div><br />
                                            </div>
                                         
                                     </div>
                                  </div>
                             </div>

                             <div class="col-md-12">
                                 <div class="x_panel">
                                     <div class="x_title">
                                        <h2>ThreshHolds</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                     <div class="x_content">
                                         <div class="col-md-12">
                                                <asp:GridView ID="GridThreashholds" runat="server" 
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
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
                                               </asp:GridView><br />

                                             <div id="curVals" runat="server" style="width:100%"> </div>
                                         </div>
                                     </div>
                                 </div>
                             </div>
                         </div>
                   </div>
</asp:Content>
