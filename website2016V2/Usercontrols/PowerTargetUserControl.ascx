<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PowerTargetUserControl.ascx.cs" Inherits="website2016V2.Usercontrols.PowerTargetUserControl" %>

<asp:Panel ID="Panel1" runat="server" >
<asp:UpdatePanel ID="MeterInfoUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Table ID="Table1" runat="server" Height="98px" Width="593px">
            <asp:TableRow>
                <asp:TableCell>Caption:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control" Width="250px" Height="34px">ABC Meter</asp:TextBox></asp:TableCell><asp:TableCell>Period</asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtPeriod" runat="server" CssClass="form-control" Width="250px" Height="34px">1 Hour</asp:TextBox></asp:TableCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableHeaderCell> </asp:TableHeaderCell><asp:TableHeaderCell>Total</asp:TableHeaderCell><asp:TableHeaderCell>Avg</asp:TableHeaderCell><asp:TableHeaderCell>Historic</asp:TableHeaderCell><asp:TableHeaderCell>Target</asp:TableHeaderCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>KWh:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="txtKwh" runat="server" CssClass="form-control" Width="250px" Height="34px">0 Kwh</asp:TextBox></asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtAvgKwh" runat="server" CssClass="form-control" Width="250px" Height="34px">0 Kwh</asp:TextBox></asp:TableCell><asp:TableCell>
                            <asp:TextBox ID="txtHistKwh" runat="server" CssClass="form-control" Width="250px" Height="34px">0 Kwh</asp:TextBox></asp:TableCell><asp:TableCell>
                            <asp:TextBox ID="txtTargetKwh" runat="server" CssClass="form-control" Width="250px" Height="34px">0 Kwh</asp:TextBox></asp:TableCell></asp:TableRow>
        </asp:Table>




        <%--<asp:Label ID="Label6" runat="server" Text="Total:" style="top: 28px; left: 61px; position: absolute; height: 19px; width: 51px"></asp:Label>
      <asp:Label ID="Label7" runat="server" Text="Avg:" style="top: 28px; left: 221px; position: absolute; height: 19px; width: 51px"></asp:Label>
      <asp:Label ID="Label8" runat="server" Text="Historic:" style="top: 28px; left: 381px; position: absolute; height: 19px; width: 51px"></asp:Label>

    <asp:Label runat="server" Text="Caption:"          style="top: 48px; left: 10px; position: absolute; height: 19px; width: 51px"></asp:Label>
    <asp:TextBox ID="txtCaption" runat="server"         style="top: 48px; left: 61px; position: absolute; height: 22px; width: 128px">ABC Meter</asp:TextBox>--%>
        <%-- <asp:Label ID="Label1" runat="server" Text="Period:" ></asp:Label>
 <asp:TextBox ID="txtPeriod" runat="server" >1 Hour</asp:TextBox>--%>
        <%--<asp:Label ID="Label2" runat="server" Text="KWh:" ></asp:Label>
 


 <asp:Label ID="Label3" runat="server" Text=":" ></asp:Label>
 
 
 
 <asp:Label ID="Label4" runat="server" Text=":" ></asp:Label>
 
 
 
 <asp:Label ID="Label5" runat="server" Text=":" ></asp:Label>
 
        --%>
        <asp:Timer ID="MeterRefreshTimer" runat="server">
        </asp:Timer>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Panel>

