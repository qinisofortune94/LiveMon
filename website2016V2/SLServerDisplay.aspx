<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SLServerDisplay.aspx.cs" Inherits="website2016V2.SLServerDisplay" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <meta http-equiv="refresh" content="3600"/>
    <script>
        //document.getElementById('silverlightObjDiv').oncontextmenu = disableRightClick;
        //function disableRightClick(e) {
        //    if (!e) e = window.event;
        //    if (e.preventDefault) {
        //        e.preventDefault();
        //    } else {
        //        e.returnValue = false;
        //    }
        //}
    </script>

    <h3>Server Display</h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Display"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <div  id="silverlightObjDiv" ClientIDMode="Static" style="width: 100%;height:800px;"> 
                        <asp:Silverlight ID="Xaml1" runat="server" MinimumVersion="2.0.31005.0" Width="100%" Height="100%" />
                    </div>
                    <iframe id="iframeReport" style="position:absolute;top:0px;left:0px;width:100%;height:100%;visibility:hidden;margin-left:15px;" src=""></iframe>
                </div>
            </div>
        </div>
    </div><br />       
</asp:Content>
