<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SensorFieldDisplay.ascx.cs" Inherits="website2016V2.SensorFieldDisplay" %>
<div id="SensorFieldContainer" runat="server" style="float: left; overflow: hidden;" >
    <asp:label ID="FieldName" runat="server" BackColor="#FFF"  BorderWidth="1px"></asp:label>
    <asp:label ID="FieldValue" runat="server" BackColor="#FFF" Visible="False" BorderWidth="1px"></asp:label>
   <asp:label ID="FieldOtherValue" runat="server" BackColor="#FFF" Text="Val:" Visible="False" BorderWidth="1px"></asp:label>
</div>