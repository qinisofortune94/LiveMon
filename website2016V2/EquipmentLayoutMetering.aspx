<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EquipmentLayoutMetering.aspx.cs" Inherits="website2016V2.EquipmentLayoutMetering" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <link href="css/main.css" rel="stylesheet" />
 
    <script type="text/javascript">
        //Load the Heirachy with Ajax
        //document.body.onload = function () { callAjax_GetAllEquipmentLayout(); }
        function ddl_changed(ddl) {
           // alert(ddl.value);
            callAjax_GetspecificEquipmentLayout(ddl.value);
            //callAjax_GetAllEquipmentLayout();
        }
</script>
      
<div id="header">
    Family Name
</div>
<div id="control_panel">
    <div class="section">
        Control Panel
    </div>
    <%--<button id="add_child">
        Add Child
    </button>
    <button id="remove_node">
        Remove Node
    </button>--%>
    <p>Root Layouts</p>
    <asp:DropDownList id="RootLayers" class="dropdownctrl" runat="server" onchange="ddl_changed(this)">

    </asp:DropDownList>
    <p>Sensor Types</p>
    <asp:DropDownList id="TypeFilter" class="dropdownctrl" runat="server">

    </asp:DropDownList>
    <button id="zoom_in">
        Zoom In</button>
    <button id="zoom_out">
        Zoom Out
    </button>
    <div class="section">
        Information Panel
    </div>
    <div id="information_panel">

    </div>
</div>
<div id="divider">

</div>
<div id="main">
    <canvas id="canvas"></canvas>
</div>


<script src="Scripts/tree.js"></script>
<script src="Scripts/main.js"></script>

</asp:Content>
