<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IPDeviceTemplate.aspx.cs" Inherits="website2016V2.IPDeviceTemplate" %>
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

    
    <%-- Display Role Part--%>
    <div class="card" style="font-size: 13px">
        <div class="card-header">
            <h3 class="card-title">Add IP Device Template
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
                             <asp:DropDownList ID="ddltype" runat="server" required="true" CssClass="form-control" OnSelectedIndexChanged="ddltype_SelectedIndexChanged1" >
                           
                        </asp:DropDownList>
                            </div>
                        </div>
               <div class="col-md-4">
                        <div class="form-group">
                            TCP Port
                            <asp:TextBox ID="txtPort" runat="server" PlaceHolder="Please enter port" required="true" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
               <div class="col-md-4">
                        <div class="form-group">
                            IP Address
                            <asp:TextBox ID="txtIPaddress" runat="server" PlaceHolder="Please enter  IP Address" required="true" CssClass="form-control"></asp:TextBox> 
                            </div>
                        </div>
               </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label ID="lblExtraData1" runat="server" Text="ExtraData 1:"></asp:Label>
                            <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter  extradata"  CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label ID="lblExtraData2" runat="server" Text="ExtraData 2:"></asp:Label>
                            <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter extradata "   CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <asp:Label ID="lblExtraData3" runat="server" Text="ExtraData 3:"></asp:Label>
                            <asp:TextBox ID="txtExtraData3" runat="server" PlaceHolder="Please enter  extradata" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>                
                 
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            Image Normal
                            <div class="input-group">
                             <div class="custom-file">
                                 <asp:FileUpload ID="fuImageNormal" runat="server"  CssClass="custom-file-input"></asp:FileUpload>
                                 <label class="custom-file-label" for="fuImageNormal">Choose file</label>
                             </div>
                                <asp:Image ID="imgNormal" runat="server" Height="38px" />
                            </div> 
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            Image Error
                            <div class="input-group">
                             <div class="custom-file">
                                 <asp:FileUpload ID="fuImageError" runat="server" CssClass="custom-file-input"></asp:FileUpload>
                                 <label class="custom-file-label" for="fuImageError">Choose file</label>
                             </div>
                                &nbsp;<asp:Image ID="imgError" runat="server" Height="38px" />
                            </div>
                        </div>                        
                    </div> 
                    <div class="col-md-4">
                        <div class="form-group">
                            No Response Image
                            <div class="input-group">
                             <div class="custom-file">
                                 <asp:FileUpload ID="fuNoResponse" runat="server" CssClass="custom-file-input"></asp:FileUpload>
                                 <label class="custom-file-label" for="fuNoResponse">Choose file</label>
                             </div>
                            &nbsp;<asp:Image ID="imgResponse" runat="server" Height="38px" />
                            </div> 
                        </div>
                    </div>
                </div>
                
                   <div class="row">                    
                    <div class="col-md-4">
                        <div class="form-group">
                            Caption
                            <asp:TextBox ID="txtCaption" runat="server" PlaceHolder="Please enter caption" required="true" CssClass="form-control"></asp:TextBox>
                        </div>
                        
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            Template Name
                            <asp:TextBox ID="txtTemplateName" runat="server" PlaceHolder="Please enter template name" required="true" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" Width="250px" Height="40px"  class="btn bg-gray form-control" OnClick="btnAdd_Click" />
                    </div> 
                    <div class="col-md-4">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="250px" Height="40px" class="btn bg-gray form-control" OnClick="btnAdd_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>
                </div>
        </div>

    </div>
 
</asp:Content>

                