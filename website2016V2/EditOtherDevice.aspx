<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditOtherDevice.aspx.cs" Inherits="website2016V2.EditOtherDevice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">


    <h3>Devices</h3>
    <div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>


    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">Edit Other devices
            </h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>                
            </div>
        </div>
        <div class="card-body" style="display: block;">
            <div class="row" runat="server" visible="false">
                    <div class="col-md-2">Device name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDeviceName" runat="server" PlaceHolder="Please enter device type"  CssClass="form-control" ></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                            <asp:Button ID="btnSearch" runat="server" Text="Filter" Height="20px" Width="63px" OnClick="btnSearch_Click" />
                    </div>
                </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       Device name
                       <asp:DropDownList ID="ddlSelectedDevice" runat="server" required="true" AutoPostBack="true" CssClass="form-control"  OnSelectedIndexChanged="ddlSelectedDevice_SelectedIndexChanged1">
                               
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       Type
                       <asp:DropDownList ID="ddltype" runat="server" OnSelectedIndexChanged="ddltype_SelectedIndexChanged" CssClass="form-control" >
                            
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       TCP Port
                       <asp:RegularExpressionValidator ID="rgePort" runat="server" Display="Dynamic" ControlToValidate="txtPort"
                 ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="^\d+$">        </asp:RegularExpressionValidator>
                        <asp:TextBox ID="txtPort" runat="server" PlaceHolder="Please enter port" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       BaudRate
                       <asp:DropDownList ID="ddlBaudRate" runat="server" AutoPostBack="true" PlaceHolder="Please select rate" required="true" CssClass="form-control" ></asp:DropDownList> 
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblSerialPort" runat="server" Text="SerialPort" Width="120px"></asp:Label>                    
                        <asp:DropDownList ID="ddlSerialPort" runat="server" AutoPostBack="true" PlaceHolder="Please select serial port" required="true" CssClass="form-control" ></asp:DropDownList> 
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       IP Address
                       <asp:TextBox ID="txtIPaddress" runat="server" PlaceHolder="Please enter  IP Address"  CssClass="form-control" ></asp:TextBox> 
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4" style="height:70px!important">
                   <div class="form-group">
                       Stop Bits
                       <asp:RadioButtonList ID="radStopBits" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="15" runat="server"></asp:RadioButtonList>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       Data Bits
                       <asp:DropDownList ID="ddlDataBits" runat="server" AutoPostBack="true" PlaceHolder="Please select rate" required="true" CssClass="form-control" ></asp:DropDownList> 
                    </div>
                </div>
                <div class="col-md-4" style="height:70px!important">
                   <div class="form-group">
                       Hand Shaking
                       <asp:RadioButtonList ID="radHandShaking" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="15" runat="server">
                    </asp:RadioButtonList>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4" style="height:70px!important">
                   <div class="form-group">
                       Error
                       <asp:RadioButtonList ID="radErrCheck" runat="server" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="15">
                    </asp:RadioButtonList>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData1" runat="server" Text="ExtraData 1:"></asp:Label>
                       <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                      
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData2" runat="server" Text="ExtraData 2:"></asp:Label>
                       <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extradata" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData3" runat="server" Text="ExtraData 3"></asp:Label>
                       <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter extradata" CssClass="form-control" ></asp:TextBox>
                       
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData4" runat="server" Text="ExtraData 4"></asp:Label>
                       <asp:TextBox ID="txtExtraData4" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData5" runat="server" Text="ExtraData 5"></asp:Label>
                       <asp:TextBox ID="txtExtraData5" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData" runat="server" Text="ExtraData "></asp:Label>
                       <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter  extradata" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       Image Normal
                        <div class="input-group">
                            <div class="custom-file" style="border:1px solid #ccc;padding-left:5px!important">
                                <asp:FileUpload ID="fuImageNormal" runat="server"></asp:FileUpload>
                            </div>
                            <asp:Image ID="imgNormal" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       Image Error
                        <div class="input-group">
                            <div class="custom-file" style="border:1px solid #ccc;padding-left:5px!important">
                                <asp:FileUpload ID="fuImageError" runat="server" ></asp:FileUpload>
                            </div>
                            <asp:Image ID="imgError" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       No Response Image
                        <div class="input-group">
                            <div class="custom-file" style="border:1px solid #ccc;padding-left:5px!important">
                                <asp:FileUpload ID="fuNoResponse" runat="server" ></asp:FileUpload>
                            </div>
                            <asp:Image ID="imgResponse" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       Caption
                       <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter caption"  CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       Device Location
                       <div class="custom-file">
                            <asp:DropDownList ID="ddlDeviceLocation" runat="server" AutoPostBack="true" required="true" CssClass="form-control" >
                               
                        </asp:DropDownList>
                       </div>                      
                       <asp:Button ID="btnChange" runat="server" Text="Change Location"  class="btn bg-gray" Onclick="btnChangeLocation_Click"/>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">                       
                       <div class="custom-file">
                           <asp:DropDownList ID="ddlDevice" runat="server" AutoPostBack="true" required="true" CssClass="form-control" >
                             
                        </asp:DropDownList>
                       </div>
                       <asp:Button ID="btnChangeSite" runat="server" Text="Change Site" class="btn bg-gray" OnClick="btnChangeSite_Click" />
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">

                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                   <div class="form-group">

                    </div>
                </div>
                <div class="col-md-8 text-center">
                   <div class="form-group">
                       <asp:Button ID="btnAdd" runat="server" Text="Edit" class="btn bg-gray" OnClick="btnAdd_Click" />
                       <asp:Button ID="btnDelete" runat="server" Text="Delete" class="btn bg-gray" OnClick="btnDelete_Click" />
                       <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn bg-gray" OnClick="btnClear_Click" />

                    </div>
                </div>
                <div class="col-md-2">
                   <div class="form-group">

                    </div>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>