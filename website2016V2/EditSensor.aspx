<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditSensor.aspx.cs" Inherits="website2016V2.EditSensor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <style>
        .control-label {
            font-weight: normal !important
        }
        input,select{
            font-size: 14px !important;
        }

    </style>
    <div>
        <div class="card">

            <div class="alert alert-dark" id="successMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblSucces" runat="server"></asp:Label>
            </div>
            <div class="alert alert-default-dark" id="warningMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblWarning" runat="server"></asp:Label>
            </div>
            <div class="alert alert-default-light" id="errorMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblErr" runat="server"></asp:Label>
            </div>

            <div class="card-header">
                <h3 class="card-title">Edit Sensor</h3>
                <div class="card-tools">
                    <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                        <i class="fas fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="card-body" style="padding: 10px !important">

                <div class="row" style="font-size: 13px">
                    <div class="col-md-6">
                        <div class="card">
                            <div style="margin: 7px !important; height: 490px;">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="input-group ">

                                                <asp:TextBox ID="txtSensName" placeholder="Search for sensors..." CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox>

                                                <div class="input-group-append">
                                                    <asp:Button ID="btnnSearchSens" runat="server" class="btn input-group-text" Text="Filter" OnClick="btnnSearchSens_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Select Sensor <span class="required"></span>
                                            <asp:DropDownList ID="cmbSensors" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbSensors_SelectedIndexChanged" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Type <span class="required"></span>
                                            <asp:DropDownList ID="cmbType" OnSelectedIndexChanged="cmbType2_SelectedIndexChanged" runat="server" CssClass="form-control" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label7" Font-Bold="true" runat="server" CssClass="control-label" Text="Module"></asp:Label>
                                            <asp:TextBox ID="txtModule" runat="server" Text="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label8" Font-Bold="true" runat="server" CssClass="control-label" Text="Register"></asp:Label>
                                            <asp:TextBox ID="txtRegister" runat="server" Text="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label11" Font-Bold="true" runat="server" CssClass="control-label" Text="Maximum Value"></asp:Label>
                                            <asp:TextBox ID="txtMaxValue" runat="server" ValueText="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Scan Rate <span class="required"></span>
                                            <asp:TextBox ID="txtScanRate" runat="server" ValueText="5000" MinValue="0" ToolTip="How often to scan this sensors value. Min 0=disabled min rate = 5000" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>




                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExtraValue" Font-Bold="true" runat="server" Text="Extra Value" CssClass="control-label"></asp:Label>
                                            <asp:TextBox ID="txtExtraValue" runat="server" ValueText="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExtraValue1" Font-Bold="true" runat="server" Text="Extra Value 1" CssClass="control-label"></asp:Label>
                                            <asp:TextBox ID="txtExtraValue1" runat="server" ValueText="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="input-group ">
                                                <asp:DropDownList ID="cmbLocations" placeholder="Search for..." CssClass="form-control" runat="server" ToolTip="The Location of the Sensor." AutoPostBack="True"></asp:DropDownList>
                                                <div class="input-group-append">
                                                    <asp:Button ID="btnnChangeLocation" runat="server" class="btn input-group-text" ToolTip="Set the sensor location." Text="Change Location" OnClick="btnnChangeLocation_Click" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="input-group ">
                                                <asp:DropDownList ID="cmbSites" placeholder="Search for..." CssClass="form-control" runat="server" ToolTip="The site the sensor is linked to." AutoPostBack="True"></asp:DropDownList>
                                                <div class="input-group-append">
                                                    <asp:Button ID="btnnChangeSite" runat="server" class="btn input-group-text" ToolTip="Set the sensor site." Text="Change Site" OnClick="btnnChangeSite_Click" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>




                            </div>




                        </div>
                    </div>



                    <div class="col-md-6">
                        <div class="card">
                            <div style="margin: 7px !important">

                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="input-group ">
                                                <asp:TextBox ID="txtDeviceName" placeholder="Search for devices..." CssClass="form-control" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <asp:Button ID="btnnSearchDevice" runat="server" class="btn input-group-text" Text="Filter" OnClick="btnnSearchDevice_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Devices <span class="required"></span>
                                            <asp:DropDownList ID="cmbDevice" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Caption <span class="required"></span>
                                            <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control" MaxLength="50">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Zero Value" CssClass="control-label" ToolTip="The value when the sensor is at zero!"></asp:Label>
                                            <asp:TextBox ID="txtMinValue2" runat="server" Text="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label12" Font-Bold="true" runat="server" CssClass="control-label" Text="Multiplier"></asp:Label>
                                            <asp:TextBox ID="txtMultiplier" runat="server" Text="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExtraData" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra Data"></asp:Label>
                                            <asp:TextBox ID="txtExtraData" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExtraData1" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra Data 1"></asp:Label>
                                            <asp:TextBox ID="txtExtraData1" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExtraData2" Font-Bold="true" CssClass="control-label" runat="server" Text="Extra Data 2"></asp:Label>
                                            <asp:TextBox ID="txtExtraData2" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label ID="lblExtraData3" Font-Bold="true" runat="server" Text="Extra Data 3" CssClass="control-label"></asp:Label>
                                            <asp:TextBox ID="txtExtraData3" runat="server" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Divisor <span class="required"></span>
                                            <asp:TextBox ID="txtDivisor" runat="server" ValueText="0" CssClass="form-control">
                                            </asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            Sensor Group <span class="required"></span>
                                            <asp:DropDownList ID="cmbSensGroup" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Label ID="lblOutPut" Font-Bold="true" runat="server" Text="Sensor Output" CssClass="control-label" Visible="False"></asp:Label>
                                            <asp:DropDownList ID="cmbSensOutput" runat="server" CssClass="form-control" Visible="False">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="input-group ">
                                                <asp:DropDownList ID="DropDownAlertGroup" placeholder="Search for..." CssClass="form-control" runat="server" ToolTip="Select the default group of contacts to notify for Standard alerts." AutoPostBack="True"></asp:DropDownList>
                                                <div class="input-group-append">
                                                    <asp:Button ID="btnTestSensor0" runat="server" class="btn input-group-text" ToolTip="Set the notification group." Text="Link Alert Group" OnClick="btnLinkAlertGroup_Click" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>



                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">

                        <div class="row">
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
                                    </div>
                                    <br />
                                </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    Image Normal <span class="required"></span>
                                    <div class="form-control">
                                        <asp:FileUpload ID="filImageNormal" runat="server" ToolTip="Normal image to use as Icon of sensor" Style="margin: -4px" />
                                    </div>
                                    <asp:Image ID="imgNormal" runat="server" Height="50px" />
                                    <!-- text input -->

                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    Image No Response <span class="required"></span>
                                    <div class="form-control">
                                        <asp:FileUpload ID="filImageNoResponse" runat="server" ToolTip="NoResponse image to use as Icon of sensor" Style="margin: -4px" />
                                    </div>
                                    <asp:Image ID="imgResponse" runat="server" Height="50px" />
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    Image error <span class="required"></span>
                                    <div class="form-control">
                                        <asp:FileUpload ID="filImageError" runat="server" ToolTip="Error image to use as Icon of sensor" Style="margin: -4px" />
                                    </div>
                                    <asp:Image ID="imgError" runat="server" Height="50px" />
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <asp:Label ID="Label5" Font-Bold="true" runat="server" CssClass="control-label" Text="SerialNumber"></asp:Label>
                                    <asp:TextBox ID="txtSerialNumber" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <!-- text input -->
                                <div class="form-group">
                                    <asp:Button ID="btnEditSesor" runat="server" Text="Edit Sensor" class="btn btn-block" Style="background-color: #ced4da" OnClick="btnEditSensor_Click" ToolTip="Save changes made to the sensor configuration." />


                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <asp:Button ID="btnDeleteSensor" runat="server" Text="Delete Sensor" class="btn btn-block" Style="background-color: #ced4da" ToolTip="Delete Sensor" OnClick="btnDeleteSensor_Click" />
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <asp:Button ID="btnTestSensor" runat="server" class="btn btn-block" Style="background-color: #ced4da" Text="Test Sensor Readings" ToolTip="Test the sensor configuration." OnClick="btnTestSensor_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div style="font-size:13px">
        <div class="card">
              <div class="card-header">
                <h3 class="card-title">Sensor Field Grid</h3>
                <div class="card-tools">
                    <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                        <i class="fas fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="card-body table-responsive p-0" style="padding: 10px !important">
                <asp:GridView ID="cmbFields2" runat="server" CssClass="table table-hover text-nowrap"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnPageIndexChanging="cmbFields_PageIndexChanging" OnDataBinding="cmbFields_DataBinding"
                                AllowPaging="True" AutoGenerateEditButton="True" PageSize="5" OnRowCancelingEdit="cmbFields_RowCancelingEdit" OnRowUpdating="cmbFields_RowUpdating" OnRowEditing="cmbFields_RowEditing"
                                ViewStateMode="Enabled" OnSelectedIndexChanged="cmbType2_SelectedIndexChanged">
                            </asp:GridView>
                </div>
            </div>

         <div class="card" >
              <div class="card-header">
                <h3 class="card-title">Sensor Readings</h3>
                <div class="card-tools">
                    <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                        <i class="fas fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="card-body" style="padding: 10px !important">
                <div id="results" runat="server"></div>
                </div>
             </div>

        
    </div>

    <%--<asp:HiddenField ID="txtID2" runat="server" />--%>
    <asp:TextBox ID="alert" Visible="false" runat="server" Height="97px" Width="100%"></asp:TextBox>
</asp:Content>

