<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CameraControl.ascx.cs" Inherits="website2016V2.CameraControl" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%--<script type="text/javascript" src="CameraControl.js"></script>
--%><table border="1">
    <tr>
        <td>
            <table border="1" onload="ShowUserCover()">
                <tr>
                    <td>
                        <strong>Camera Control</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Zoom" Width="88px"></asp:Label><br />
                        <igtxt:WebNumericEdit ID="ZoomLevel" runat="server" MaxValue="500" MinValue="10"
                            ValueText="100" Width="56px">
                            <SpinButtons Display="OnRight" Delta="10"></SpinButtons>
                        </igtxt:WebNumericEdit>
                        <br />
                        <asp:HyperLink ID="btnCapture" runat="server" Text="Capture" Target="_NewCapture" /><br />
                        <asp:HyperLink ID="btnConfig" runat="server" Text="Configure" Width="96px" Target="main" /><br />
                        <asp:HyperLink ID="btnConfigProxy" runat="server" Text="Proxy Config" Width="96px"
                            Target="main" /><br />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HdnCameraID" runat="server" Value="0" />
            <asp:HiddenField ID="Refresh" runat="server" Value="2000" />
            <asp:HiddenField ID="HiddenName" runat="server" Value="" />
        </td>
        <td>
            <div id="ImageHolder">
                <asp:Image ID="MyImageHolder" runat="server" />
           </div>
        </td>
        <td>
            <table border="1">
                <tr>
                    <td>
                        <asp:HyperLink ID="btnConfigSensor" runat="server" Text="Configure" Width="96px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>IO Pannel</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div runat="server" id="InPannel">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div runat="server" id="OutPannel">
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
