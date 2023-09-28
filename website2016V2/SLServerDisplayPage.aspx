<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLServerDisplayPage.aspx.cs" Inherits="website2016V2.SLServerDisplayPage" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="refresh" content="3600"/>
</head>
<body>
    <form id="form1" runat="server">

    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <div  id="silverlightObjDiv" ClientIDMode="Static" style="width: 100%;height:800px;"> 
                        <asp:Silverlight ID="Xaml1" runat="server" MinimumVersion="2.0.31005.0" Width="100%" Height="100%" />
                    </div>
                    <iframe id="iframeReport" style="position:absolute;top:0px;left:0px;width:100%;height:100%;visibility:hidden;margin-left:15px;" src=""></iframe>
    </div>
    </form>
</body>
</html>
