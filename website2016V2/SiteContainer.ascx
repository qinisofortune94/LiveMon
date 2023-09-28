<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteContainer.ascx.cs" Inherits="website2016V2.SiteContainer" %>
<style type="text/css">
         .auto-style1 {
             color:white;
         }

     </style>
<asp:ImageButton ID="ShowSite" OnClick="ShowSite_Click" runat="server" ImageUrl="~/images/plus.gif" OnClientClick = "SetSource(this.id)" />
<asp:ImageButton ID="HideSite" OnClick="HideSite_Click" runat="server" ImageUrl="~/images/minus.gif" OnClientClick = "SetSource(this.id)"/>
<asp:Label ID="SiteName" runat="server" Text="Site:" BackColor="#FFFF66"></asp:Label>
<div id="SiteContainerDetails" runat="server" CssClass="SiteContainerclass">
</div>