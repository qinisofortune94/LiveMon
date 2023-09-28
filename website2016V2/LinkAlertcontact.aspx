<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkAlertcontact.aspx.cs" Inherits="website2016V2.LinkAlertcontact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
     <div class="">
                         <div class="alert alert-danger" id="errorMessageLink" visible="false" runat="server">
                            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                            <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                         </div>
                         <div class="page-title">
                                <div class="title_left">
                                    <h3>Link Alert Contact Form</h3><br />
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
                             <div class="col-md-12">
                                 <div class="x_panel">
                                     <div class="x_title">
                                        <h2>Add Alert Fields</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                            <asp:GridView ID="Alertsgrid" runat="server" AllowPaging="True" 
                                                 AllowSorting="True" AutoGenerateSelectButton="True" CellPadding="4" 
                                                 ForeColor="#333333" GridLines="None" Width="100%">
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
                                             </asp:GridView><br /><br />
                                            <asp:HiddenField ID="txtID" runat="server" />

                                            <p class="pSetting">Contacts</p>
                                            <div class="col-md-6 col-sm-6 col-xs-6">
                                                <asp:DropDownList ID="cmbContacts" CssClass="form-control" runat="server" Height="32px">
                                                </asp:DropDownList>
                                                <br /><br />
                                                <asp:Button id="btnLinkContacts" CssClass="btn btn-success" runat="server" Text="Link Contact" Width="152px" OnClick="btnLinkContacts_Click">
                                                </asp:Button>
                                                <asp:Button id="btnAddContact" CssClass="btn btn-success" runat="server" Text="Add Contacts" Width="152px">
                                                </asp:Button>
                                                <asp:Button ID="btnSetThreashhold" CssClass="btn btn-success" runat="server" Text="Set Threashholds" Width="144px" OnClick="btnSetThreashhold_Click">
                                                </asp:Button>
                                                <asp:HiddenField ID="txtContactID" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                 </div>
                             </div>

                             <div class="col-md-12">
                                <div class="x_panel">
                                     <div class="x_title">
                                        <h2>Linked Contacts</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                      <div class="x_content">
                                           <div class="col-md-12">
                                               <asp:GridView ID="GridContacts" runat="server" 
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
                                               </asp:GridView>
                                           </div>
                                      </div>
                                 </div>
                             </div>
                         </div>
                     </div>
</asp:Content>