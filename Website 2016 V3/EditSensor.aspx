<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditSensor.aspx.cs" Inherits="website2016V2.EditSensor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <div class="">
                        <div class="page-title">
                            <div class="title_left">
                                <h3>Sensor Dashboard Display</h3><br />
                            </div>
                            <div class="title_right">
                                <div class="col-md-4 col-sm-5 col-xs-12 form-group pull-right top_search">
                                    <div class="input-group">
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>

                        <asp:Panel ID="editTestSensor" runat="server">
                            <div class="row">
                                <div class="alert alert-success" id="successMessage" runat="server">
                                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                    <asp:Label ID="lblSucces" runat="server" Width="200px"></asp:Label>
                                </div>
                                <div class="alert alert-warning" id="warningMessage" runat="server">
                                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                    <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
                                </div>
                                <div class="alert alert-danger" id="errorMessage" runat="server">
                                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                    <asp:Label ID="lblErr" runat="server"  Width="200px"></asp:Label>
                                </div>
                                <div class="col-md-6 col-sm-12 col-xs-12">
                                    <div class="x_panel">
                                        <div class="x_title">
                                            <h2>Edit Sensor</h2>
                                            <ul class="nav navbar-right panel_toolbox">
                                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                                </li>
                                            </ul>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="x_content">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="col-md-12 col-sm-5 col-xs-12 form-group top_search">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtSensName" placeholder="Search for sensors..." CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox><span class="input-group-btn"><asp:Button ID="btnnSearchSens" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnnSearchSens_Click"/></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Select Sensor <span class="required"></span>
                                                        </label>
                                                        <asp:DropDownList ID="cmbSensors" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbSensors_SelectedIndexChanged" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Type <span class="required"></span>
                                                        </label>
                                                        <asp:DropDownList ID="cmbType" OnSelectedIndexChanged="cmbType2_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label7" Font-Bold="true" runat="server" CssClass="control-label" Text="Module"></asp:Label>
                                                        <asp:TextBox ID="txtModule" runat="server" Text="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label8" Font-Bold="true" runat="server" CssClass="control-label" Text="Register"></asp:Label>
                                                        <asp:TextBox ID="txtRegister" runat="server" Text="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label11" Font-Bold="true" runat="server" CssClass="control-label" Text="Maximum Value"></asp:Label>
                                                        <asp:TextBox ID="txtMaxValue" runat="server" ValueText="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Scan Rate <span class="required"></span>
                                                        </label>
                                                        <asp:TextBox ID="txtScanRate" runat="server" ValueText="5000" MinValue="0" ToolTip="How often to scan this sensors value. Min 0=disabled min rate = 5000" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblExtraValue" Font-Bold="true" runat="server" Text="Extra Value" CssClass="control-label"></asp:Label>
                                                        <asp:TextBox ID="txtExtraValue" runat="server" ValueText="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblExtraValue1" Font-Bold="true" runat="server" Text="Extra Value 1" CssClass="control-label"></asp:Label>
                                                        <asp:TextBox ID="txtExtraValue1" runat="server" ValueText="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br /><br />

                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="col-md-12 col-sm-5 col-xs-12 form-group top_search">
                                                            <div class="input-group">
                                                                <%--<label class="control-label" for="first-name">Sensor Location <span class="required"></span>
                                                                </label>--%>
                                                                <asp:DropdownList ID="cmbLocations" placeholder="Search for..." CssClass="form-control" runat="server" ToolTip="The Location of the Sensor." AutoPostBack="True"></asp:DropdownList><span class="input-group-btn"><asp:Button ID="btnnChangeLocation" runat="server" CssClass="btn btn-primary" ToolTip="Set the sensor location." Text="Change Location" OnClick="btnnChangeLocation_Click"/></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="col-md-12 col-sm-5 col-xs-12 form-group top_search">
                                                            <div class="input-group">
                                                                <%--<label class="control-label" for="first-name">Sensor Location <span class="required"></span>
                                                                </label>--%>
                                                                <asp:DropdownList ID="cmbSites" placeholder="Search for..." CssClass="form-control" runat="server" ToolTip="The site the sensor is linked to." AutoPostBack="True"></asp:DropdownList><span class="input-group-btn"><asp:Button ID="btnnChangeSite" runat="server" CssClass="btn btn-primary" ToolTip="Set the sensor site." Text="Change Site" OnClick="btnnChangeSite_Click"/></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-md-4 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Image Normal <span class="required"></span>
                                                        </label>
                                                        <asp:FileUpload ID="filImageNormal" runat="server" ToolTip="Normal image to use as Icon of sensor" />
                                                        <asp:Image ID="imgNormal" runat="server" Height="50px" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Image No Response <span class="required"></span>
                                                        </label>
                                                        <asp:FileUpload ID="filImageNoResponse" runat="server" ToolTip="NoResponse image to use as Icon of sensor" />
                                                        <asp:Image ID="imgResponse" runat="server" Height="50px" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Image error <span class="required"></span>
                                                        </label>
                                                        <asp:FileUpload ID="filImageError" runat="server" ToolTip="Error image to use as Icon of sensor" />
                                                        <asp:Image ID="imgError" runat="server" Height="50px" />
                                                    </div><br /><br /><br />
                                                </div><br /><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label5" Font-Bold="true" runat="server" CssClass="control-label" Text="SerialNumber"></asp:Label>
                                                        <asp:TextBox ID="txtSerialNumber" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />
                                            </div>
                                      </div>
                                   </div>
                                </div>
                                <div class="col-md-6 col-sm-12 col-xs-12">
                                    <div class="x_panel">
                                        <div class="x_title">
                                            <h2></h2>
                                            <ul class="nav navbar-right panel_toolbox">
                                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                                </li>
                                            </ul>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="x_content">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="col-md-12 col-sm-5 col-xs-12 form-group top_search">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtDeviceName" placeholder="Search for devices..." CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox><span class="input-group-btn"><asp:Button ID="btnnSearchDevice" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnnSearchDevice_Click"/></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Devices <span class="required"></span>
                                                        </label>
                                                        <asp:DropDownList ID="cmbDevice" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Caption <span class="required"></span>
                                                        </label>
                                                        <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control" MaxLength="50">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Zero Value" CssClass="control-label" ToolTip="The value when the sensor is at zero!"></asp:Label>
                                                        <asp:TextBox ID="txtMinValue2" runat="server" Text="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label12" Font-Bold="true" runat="server" CssClass="control-label" Text="Multiplier"></asp:Label>
                                                        <asp:TextBox ID="txtMultiplier" runat="server" Text="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblExtraData" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra Data"></asp:Label>
                                                        <asp:TextBox ID="txtExtraData" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblExtraData1" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra Data 1"></asp:Label>
                                                        <asp:TextBox ID="txtExtraData1" runat="server" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblExtraData2" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra Data 2"></asp:Label>
                                                        <asp:TextBox ID="txtExtraData2" runat="server" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblExtraData3" Font-Bold="true" runat="server" Text="Extra Data 3" CssClass="control-label"></asp:Label>
                                                        <asp:TextBox ID="txtExtraData3" runat="server" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div><br />
                                                </div><br /><br />

                                                <div id="sensOutput" runat="server" visible="false" class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <asp:Label ID="lblOutPut" Font-Bold="true" runat="server" Text="Sensor Output" CssClass="control-label" Visible="False"></asp:Label>
                                                        <asp:DropDownList ID="cmbSensOutput" runat="server" CssClass="form-control" Visible="False">
                                                        </asp:DropDownList>
                                                    </div><br />
                                                </div><br /><br />

                                                <div class="form-group">
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Divisor <span class="required"></span>
                                                        </label>
                                                        <asp:TextBox ID="txtDivisor" runat="server" ValueText="0" CssClass="form-control">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                        <label class="control-label" for="first-name">Sensor Group <span class="required"></span>
                                                        </label>
                                                        <asp:DropDownList ID="cmbSensGroup" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div><br />
                                                </div><br /><br /><br />

                                                <div class="form-group">
                                                    <div class="input-group">
                                                        <div class="col-md-12 col-sm-5 col-xs-12 form-group top_search">
                                                            <div class="input-group">
                                                                <%--<label class="control-label" for="first-name">Sensor Location <span class="required"></span>
                                                                </label>--%>
                                                                <asp:DropdownList ID="DropDownAlertGroup" placeholder="Search for..." CssClass="form-control" runat="server" ToolTip="Select the default group of contacts to notify for Standard alerts." AutoPostBack="True"></asp:DropdownList><span class="input-group-btn"><asp:Button ID="btnTestSensor0" runat="server" CssClass="btn btn-primary" ToolTip="Set the notification group." Text="Link Alert Group" OnClick="btnLinkAlertGroup_Click"/></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="models" runat="server" visible="false" class="form-group">
                                                    <div class="col-md-12 col-sm-6 col-xs-6">
                                                        <asp:Label ID="Label20" Font-Bold="true" runat="server" Text="Models" CssClass="control-label" Visible="False"></asp:Label>
                                                         <asp:DropDownList ID="cmbModels" runat="server" Width="350px" Visible="False" AutoPostBack="True" OnSelectedIndexChanged="cmbModels_SelectedIndexChanged">
                                                            <asp:ListItem>Nothing</asp:ListItem>
                                                            <asp:ListItem Value="C1002">Stultz-C1002</asp:ListItem>
                                                            <asp:ListItem Value="C1010/C2020">Stulz-SNMPC1010/C2020</asp:ListItem>
                                                            <asp:ListItem Value="C2020FCB">Stulz-C2020FCB</asp:ListItem>
                                                            <asp:ListItem Value="C4000">Stulz-C4000</asp:ListItem>
                                                            <asp:ListItem Value="C5000">Stultz-C5000</asp:ListItem>
                                                            <asp:ListItem Value="C6000">Stultz-C6000</asp:ListItem>
                                                            <asp:ListItem Value="C6000CH">Stultz-C6000CH</asp:ListItem>
                                                            <asp:ListItem Value="C7000IOC">*Stultz-C7000IOC</asp:ListItem>
                                                            <asp:ListItem Value="C7000CH">Stultz-C7000CH</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div><br />
                                                </div><br /><br />
                                                <div class="ln_solid"></div>

                                                <div class="form-group">
                                                    <div class="col-md-9 col-md-offset-2">
                                                        <asp:Button ID="btnEditSesor" runat="server" Text="Edit Sensor" Width="148px" CssClass="btn btn-success" OnClick="btnEditSensor_Click" ToolTip="Save changes made to the sensor configuration." />
                                                        <asp:Button ID="btnTestSensor" runat="server" CssClass="btn btn-success" Text="Test Sensor Readings" ToolTip="Test the sensor configuration." OnClick="btnTestSensor_Click" />                                                        
                                                    </div>
                                                </div>
                                               

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="x_panel">
                                        <div class="x_title">
                                            <h2>Sensor Readings</h2>
                                            <ul class="nav navbar-right panel_toolbox">
                                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                                </li>
                                            </ul>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="x_content">
                                            <div id="results" runat ="server"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <%--<asp:HiddenField ID="txtID2" runat="server" />--%>
                  <asp:TextBox ID="alert" Visible="false" runat="server" Height="97px" Width="100%"></asp:TextBox>
</asp:Content>

