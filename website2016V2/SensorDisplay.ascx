<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SensorDisplay.ascx.cs" Inherits="website2016V2.SensorDisplay" %>

<asp:label ID="SensorName" runat="server" Text="Sensor:" BackColor="#3366FF"></asp:label>
<div style="display:none; "   id="contextMenu<%# _SensorID.ToString()%>" >
        <table  border="0" cellpadding="0" cellspacing="0" style="border: thin solid #808080; cursor: default;" width="100px" bgcolor="White">
            <tr> <td >
                    <a id="EditSensor" href="" runat ="server">Edit Sensor</a>
                </td> </tr>
            <tr> <td >
                    <a id="ShowGraph" href="" runat ="server">Show Graph</a>
                </td> </tr>
        </table>
</div>
<div id="Div1"  onmousedown="HideMenu('contextMenu<%# _SensorID.ToString()%>');" onmouseup="HideMenu('contextMenu<%# _SensorID.ToString()%>');"
          oncontextmenu="ShowMenu('contextMenu<%# _SensorID.ToString()%>',event);" class="detailItem">
<div id="SensorContainerDetails" runat="server" >
    </div>
</div>
<div id="sub-close" style="clear: both"></div>