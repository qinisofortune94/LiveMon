<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NMDUserControl.ascx.cs" Inherits="website2016V2.Usercontrols.NMDUserControl" %>
<asp:Panel ID="Panel1" runat="server">
    <asp:UpdatePanel ID="MeterInfoUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Table ID="Table1" runat="server" Height="158px" Width="189px">
                <asp:TableRow>
                    <asp:TableCell>Caption:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control" Width="250px" Height="34px">ABC Meter</asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>NMD :</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtNMD" runat="server" CssClass="form-control" Width="250px" Height="34px">1234 KVA</asp:TextBox>
                    </asp:TableCell>
                     <asp:TableCell>AVG KVA 30 Days:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtAVGKVA" runat="server" CssClass="form-control" Width="250px" Height="34px">400 KVA</asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <%--               <asp:TableRow>
                    <asp:TableHeaderCell> </asp:TableHeaderCell><asp:TableHeaderCell>Total</asp:TableHeaderCell><asp:TableHeaderCell>Avg</asp:TableHeaderCell><asp:TableHeaderCell>Historic</asp:TableHeaderCell><asp:TableHeaderCell>Target</asp:TableHeaderCell>
                </asp:TableRow>--%>
              
                <asp:TableRow>
                    <asp:TableCell>MAX KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtMAXKVA" runat="server" CssClass="form-control" Width="250px" Height="34px">1200 KVA</asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>Date MAX KVA :</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtDTMAXKVA" runat="server" CssClass="form-control" Width="250px" Height="34px">06/26/2017 10:30 AM</asp:TextBox>
                    </asp:TableCell>
               </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>Last KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtLASTKVA" runat="server" CssClass="form-control" Width="250px" Height="34px">600 KVA</asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>Date Last KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtDTLASTKVA" runat="server" CssClass="form-control" Width="250px" Height="34px">06/26/2017 10:30 AM</asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>Last-1 KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtLAST1KVA" runat="server" CssClass="form-control" Width="250px" Height="34px">550 KVA</asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>Date Last-1 KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtDTLAST1KVA" runat="server" CssClass="form-control" Width="250px" Height="34px">06/26/2017 10:30 AM</asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>Last-2 KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtLAST2KVA" runat="server" CssClass="form-control" Width="250px" Height="34px">550 KVA</asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>Date Last-2 KVA:</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtDTLAST2KVA" runat="server" CssClass="form-control" Width="250px" Height="34px">06/26/2017 10:30 AM</asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Timer ID="MeterRefreshTimer" runat="server">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
