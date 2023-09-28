<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IPDeviceEdit.aspx.cs" Inherits="website2016V2.IPDeviceEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
  <script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap.min.js"></script>
    <script src="DataTable/dataTables.buttons.min.js"></script>
    <script src="DataTable/jszip.min.js"></script>
    <script src="DataTable/pdfmake.min.js"></script>
    <script src="DataTable/vfs_fonts.js"></script>
    <script src="DataTable/buttons.html5.min.js"></script>
    <link href="DataTable/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />

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
            <h3 class="card-title">Edit IP Device
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
           <div class="row" runat="server" visible="false">
                    <div class="col-md-2">Device name:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDeviceName" runat="server" PlaceHolder="Please enter device"  CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                            <asp:Button ID="btnSearchDevice" runat="server" Text="Filter" Height="20px" Width="63px" OnClick="btnSearchDevice_Click" />
                    </div>
                </div>

           <div class="row">
                <div class="col-sm-4">
                    <!-- text input -->
                    <div class="form-group">
                        Device name
                         <asp:DropDownList ID="ddlSelectedDevice" runat="server" required="true" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cmbDevices_SelectedIndexChanged" >
                              
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        Type
                       <asp:DropDownList ID="ddltype" runat="server" required="true" CssClass="form-control" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                         </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        Port
                        <asp:RegularExpressionValidator ID="rgePort" runat="server" Display="Dynamic" ControlToValidate="txtPort"
                 ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="^\d+$">        </asp:RegularExpressionValidator>
                        <asp:TextBox ID="txtPort" runat="server" PlaceHolder="Please enter port" required="true" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4">
                    <!-- text input -->
                    <div class="form-group">
                        Device Location
                        <asp:DropDownList ID="ddlDeviceLocation" runat="server"  required="true" CssClass="form-control" AutoPostBack="true">
                       
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group" style="padding-top:20px!important">
                      <asp:Button ID="btnChangeLocation" runat="server" Text="Change Location" class="btn bg-gray form-control" OnClick="btnChangeLocation_Click" />
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4">
                    <!-- text input -->
                    <div class="form-group">
                        Sites
                        <asp:DropDownList ID="ddlDevice" runat="server" required="true" AutoPostBack="true" CssClass="form-control">
                     
                          </asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group" style="padding-top:20px!important">
                       <asp:Button ID="btnChangeSite" runat="server" Text="Change Site" class="btn bg-gray form-control" OnClick="btnChangeSite_Click" />
                       
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4">
                    <!-- text input -->
                    <div class="form-group">
                       <asp:Label ID="Label1" runat="server" Text="Image Normal"></asp:Label>
                        <div class="input-group">
                            <div class="custom-file">
                                <asp:FileUpload ID="fuImageNormal" CssClass="custom-file-input" runat="server" />
                                <label class="custom-file-label" for="fuImageNormal">Choose file</label>
                            </div>
                            &nbsp;<asp:Image ID="imgNormal" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Image Error"></asp:Label>
                        <div class="input-group">
                            <div class="custom-file">
                                <asp:FileUpload ID="fuImageError" CssClass="custom-file-input" runat="server" />
                                <label class="custom-file-label" for="fuImageError">Choose file</label>
                                <%--<input type="file" name="FileUploadImageError" id="FileUpLoad2" />--%>
                            </div>
                            &nbsp;<asp:Image ID="imgError" runat="server" Height="38px" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                         <asp:Label ID="Label3" runat="server" Text="Image Nor Response"></asp:Label>
                        <div class="input-group">
                            <div class="custom-file">
                                <asp:FileUpload ID="fuNoResponse" CssClass="custom-file-input" runat="server" />
                                <label class="custom-file-label" for="fuNoResponse">Choose file</label>
                                <%--<input type="file" name="FileUploadImageNoResponse" id="FileUpLoad3" />--%>
                            </div>

                            &nbsp;<asp:Image ID="imgResponse" runat="server" Height="38px" syle="border-radius:5px !important" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4">
                    <!-- text input -->
                    <div class="form-group">
                        IP Address
                         <div class="input-group">
                            <div class="custom-file">
                                <asp:TextBox ID="txtIPaddress" runat="server" PlaceHolder="Please enter  IP Address" required="true" CssClass="form-control"></asp:TextBox> 
                            </div>

                            &nbsp;<asp:Button ID="cmdConfigure" class="btn bg-gray" style="width:100px!important" runat="server" Text="Configure" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        Caption
                       <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter device caption" required="true" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                       <asp:Label ID="lblExtraData1" runat="server" Text="ExtraData 1"></asp:Label>
                        <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4">
                    <!-- text input -->
                    <div class="form-group">
                        <asp:Label ID="lblExtraData2" runat="server" Text="ExtraData 2"></asp:Label>
                        <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extradata "   CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        <asp:Label ID="lblExtraData3" runat="server" Text="ExtraData 3"></asp:Label>
                       <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter  extradata" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="form-group">
                        Device Location
                        
                    </div>
                </div>
            </div>

                <br />
                <div class="row">
                    <div class="col-sm-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Edit" class="btn bg-gray form-control" OnClick="btnAdd_Click" />
                    </div>
                    <div class="col-sm-4">
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" class="btn bg-gray form-control" OnClick="btnDelete_Click" />
                    </div>

                    <div class="col-sm-4">
                        <asp:Button ID="BtnClear" runat="server" Text="Clear" class="btn bg-gray form-control" OnClick="BtnClear_Click" />
                    </div>
                </div>
        </div>
    </div>    
</asp:Content>
