<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOtherDevices.aspx.cs" Inherits="website2016V2.AddOtherDevices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">


    <h3>Devices</h3>
 <div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>

    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">Add Other devices
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
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       Type
                       <asp:DropDownList ID="ddltype" runat="server" AutoPostBack="true" required="true" CssClass="form-control"  OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
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
               <div class="col-md-4">
                   <div class="form-group">
                       BaudRate
                       <asp:DropDownList ID="ddlBaudRate" runat="server" AutoPostBack="true" PlaceHolder="Please select rate" required="true" CssClass="form-control" ></asp:DropDownList> 
                   </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblSerialPort" runat="server" Text="SerialPort" Width="120px"></asp:Label>
                       <asp:DropDownList ID="ddlSerialPort" runat="server" AutoPostBack="true" PlaceHolder="Please select serial port" required="true" CssClass="form-control" >
                            </asp:DropDownList> 
                   </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        IP Address
                        <asp:TextBox ID="txtIPaddress" runat="server" PlaceHolder="Please enter  IP Address" required="true" CssClass="form-control" ></asp:TextBox> 
                    </div>
                </div>
                <div class="col-md-4" style="height:70px!important">
                   <div class="form-group">
                       Stop Bits
                       <asp:RadioButtonList RepeatDirection="Horizontal" CellPadding="15" CellSpacing="15" ID="radStopBits" runat="server"></asp:RadioButtonList>
                   </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       Data Bits
                       <asp:DropDownList ID="ddlDataBits" runat="server" AutoPostBack="true" PlaceHolder="Please select rate" required="true" CssClass="form-control" ></asp:DropDownList> 
                   </div>
                </div>
                <div class="col-md-4" style="height:70px!important">
                    <div class="form-group">
                       Hand Shaking
                         <asp:RadioButtonList ID="radHandShaking" runat="server" RepeatDirection="Horizontal" CellPadding="15" CellSpacing="15">
                    </asp:RadioButtonList>
                    </div>
                </div>
                <div class="col-md-4" style="height:70px!important">
                   <div class="form-group">
                       Error
                       <asp:RadioButtonList ID="radErrCheck" RepeatDirection="Horizontal" CellPadding="10" CellSpacing="15" runat="server">
                    </asp:RadioButtonList>
                   </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData1" runat="server" Text="ExtraData 1"></asp:Label>
                       <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                   </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                       <asp:Label ID="lblExtraData2" runat="server" Text="ExtraData 2"></asp:Label>
                        <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extradata "  CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
               <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData3" runat="server" Text="ExtraData 3"></asp:Label>
                       <asp:RegularExpressionValidator ID="rgeExtraData3" runat="server"  Display="Dynamic" ControlToValidate="txtExtraData3"
                ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="^\d+$">        </asp:RegularExpressionValidator>
                        <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                   </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData4" runat="server" Text="ExtraData 4"></asp:Label>
                       <asp:RegularExpressionValidator ID="rgeExtraData4" runat="server"  Display="Dynamic" ControlToValidate="txtExtraData4"
                ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="^\d+$">        </asp:RegularExpressionValidator>
                      <asp:TextBox ID="txtExtraData4" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                   </div>
                </div>                
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData5" runat="server" Text="ExtraData 5"></asp:Label>
                       <asp:RegularExpressionValidator ID="rgeExtraData5" runat="server" Display="Dynamic" ControlToValidate="txtExtraData5"
                ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="^\d+$">        </asp:RegularExpressionValidator>
                        <asp:TextBox ID="txtExtraData5" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control" ></asp:TextBox>
                   </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                       <asp:Label ID="lblExtraData" runat="server" Text="ExtraData "></asp:Label>
                       <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter  extradata" CssClass="form-control" ></asp:TextBox>
                   </div>
                </div>
            </div>
            <div class="row">
                
                <div class="col-md-4">
                    <div class="form-group">
                        Image Normal
                        <div class="input-group">
                            <div class="custom-file" style="border:1px solid #ccc;padding-left:5px!important">
                                <asp:FileUpload ID="fuImageNormal" runat="server" ></asp:FileUpload>
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
                                <asp:FileUpload ID="fuImageError" runat="server"></asp:FileUpload>
                            </div>
                            <asp:Image ID="imgError" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                   <div class="form-group">
                        No Response Image
                        <div class="input-group">
                            <div class="custom-file" style="border:1px solid #ccc;padding-left:5px!important">
                                <asp:FileUpload ID="fuNoResponse" runat="server"></asp:FileUpload>
                            </div>
                            <asp:Image ID="imgResponse" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                
                <div class="col-md-4">
                    <div class="form-group">
                        Caption
                        <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter caption" required="true" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
               <div class="col-md-4">
                   <div class="form-group">
                       Device Location
                       <div class="input-group">
                            <div class="custom-file">
                                <asp:DropDownList ID="ddlDeviceLocation" runat="server" AutoPostBack="true" required="true" CssClass="form-control" >
                               
                        </asp:DropDownList>
                            </div>
                            <asp:Button ID="btnChange" runat="server" Text="Change Location" class="btn bg-gray" />
                        </div>
                   </div>
                   
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        Device
                       <asp:DropDownList ID="ddlDevice" runat="server" AutoPostBack="true" required="true" CssClass="form-control" >
                               
                        </asp:DropDownList>
                   </div>
                </div>                
            </div>           
            <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8 text-center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" class="btn bg-gray" Width="100" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn bg-gray" Width="100" OnClick="btnClear_Click" />
                    </div>
                    <div class="col-md-2">
                        
                    </div>
                </div>           
        </div>
    </div>

</asp:Content>