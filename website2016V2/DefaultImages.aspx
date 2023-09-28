<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefaultImages.aspx.cs" Inherits="website2016V2.DefaultImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">


    <div class="card" style="box-shadow:none !important">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 20px !important;font-weight: 600;">Default Images</h3>
        </div>
        <div class="card-body" style="padding: 10px !important">
            <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="Label4" runat="server"></asp:Label>
            </div>
            <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblWarning" runat="server"></asp:Label>
            </div>
            <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </div>

            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">
                        <asp:Label ID="lblAdd" runat="server" Text="Add" data-card-widget="collapse" data-toggle="tooltip" title="Collapse" style="cursor:pointer"></asp:Label></h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                    </div>
                </div>
                <div class="card-body" style="padding: 10px !important">
                    <div id="AddNew" runat="server">
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" Text="Image Normal"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:FileUpload ID="filImageNormal"  CssClass="form-control" runat="server" />
                                <asp:Image ID="imgNormal" runat="server" Height="40px" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" Text="Image Error">  </asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:FileUpload ID="filImageError"  CssClass="form-control" runat="server" />
                                <asp:Image ID="imgError" runat="server" Height="40px" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" Text="Image No Response "></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:FileUpload ID="filImageNoResponse"  CssClass="form-control" runat="server" />
                                <asp:Image ID="imgResponse" runat="server" Height="40px" />
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-4">
                            </div>
                        </div>

                        <br />
                        <div class="row">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnSave" runat="server" Text="Add"  class="btn form-control" BackColor="#ced4da" OnClick="btnSave_Click" />
                                <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-md-2">
                            </div>

                            <%--<div class="col-md-4">
                        <asp:Button ID="BtnClear" runat="server" Text="Clear" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF" />
                    </div>--%>
                        </div>

                    </div>
                </div>
            </div>

             <div class="card" >
              <div class="card-header">
                <h3 class="card-title"> <asp:Label ID="lblAddb" runat="server" Text="Settings" data-card-widget="collapse" data-toggle="tooltip" title="Collapse" style="cursor:pointer"></asp:Label></h3>
                <div class="card-tools">
                    <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                        <i class="fas fa-minus"></i>
                    </button>
                </div>
            </div>
            <div class="card-body" style="padding: 10px !important">
               <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="gridDefaultImages" runat="server" CssClass="gvdatatable table table-striped table-bordered"
                            CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%" AllowPaging="True" PageSize="5">
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
                    </div>
                </div>
                </div>
             </div>
        </div>
    </div>






    


</asp:Content>
