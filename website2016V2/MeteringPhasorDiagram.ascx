<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeteringPhasorDiagram.ascx.cs" Inherits="website2016V2.MeteringPhasorDiagram" %>

<%@ Register Assembly="Infragistics2.WebUI.UltraWebChart.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.UltraWebChart" TagPrefix="igchart" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebChart.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.UltraChart.Resources.Appearance" TagPrefix="igchartprop" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebChart.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.UltraChart.Data" TagPrefix="igchartdata" %>

<style type="text/css">
    .style1 {
        width: 100%;
        border: 2px solid #000000;
    }

    .style2 {
        height: 23px;
    }

    .style3 {
        width: 22px;
    }

    .style4 {
        height: 23px;
        width: 22px;
    }

    .style5 {
        height: 26px;
    }

    .style6 {
        height: 25px;
    }

    .style7 {
        height: 24px;
    }

    .style8 {
        width: 22px;
        height: 24px;
    }
</style>

<table class="style1">
    <tr>
        <td colspan="6">Phasor Diagram:</td>
        <td colspan="4">Current Refrence:</td>
        <td colspan="8">To individual phase</td>
    </tr>
    <tr>
        <td colspan="4">Voltage Red phase value (V):</td>
        <td>
            <asp:label id="lblRedPhaseVolts" runat="server" text="Label"></asp:label>
        </td>
        <td class="style3">&nbsp;</td>
        <td colspan="5">Current Red phase value (A):</td>
        <td>
            <asp:label id="lblRedPhaseAmps" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">Voltage scaled values
        </td>
        <td colspan="4">Original a+jb values</td>
    </tr>
    <tr>
        <td colspan="4">Voltage Red phase angle(0deg)</td>
        <td>
            <asp:label id="lblRedPhaseAngle" runat="server" text="Label"></asp:label>
        </td>
        <td class="style3">&nbsp;</td>
        <td colspan="5">Current Red phase angle (deg):</td>
        <td>
            <asp:label id="lblRedPhaseCurAngle" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblRedScaledVolts" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblRedScaledVoltsSine" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbRedScaledVolts" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbRedScaledVoltsS" runat="server" text="Label"></asp:label>
        </td>
    </tr>
    <tr>
        <td colspan="4">Voltage White phase value (V):</td>
        <td>
            <asp:label id="lblWhitePhaseVolts" runat="server" text="Label"></asp:label>
        </td>
        <td class="style3">&nbsp;</td>
        <td colspan="5">Current White phase value (A):</td>
        <td>
            <asp:label id="lblWhitePhaseAmps" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblWhiteScaledVolts" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblWhiteScaledVoltsSine" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbWhiteScaledVolts" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbWhiteScaledVoltsS" runat="server" text="Label"></asp:label>
        </td>
    </tr>
    <tr>
        <td class="style2" colspan="4">Voltage White phase angle(240deg)</td>
        <td class="style2">
            <asp:label id="lblWhitePhaseAngle" runat="server" text="Label"></asp:label>
        </td>
        <td class="style4"></td>
        <td class="style2" colspan="5">Current White phase angle (deg):</td>
        <td class="style2">
            <asp:label id="lblWhitePhaseCurAngle" runat="server" text="Label"></asp:label>
        </td>
        <td class="style2">
            <asp:label id="lblBlueScaledVolts" runat="server" text="Label"></asp:label>
        </td>
        <td class="style2">
            <asp:label id="lblBlueScaledVoltsSine" runat="server" text="Label"></asp:label>
        </td>
        <td class="style2" colspan="2">
            <asp:label id="lblajbBlueScaledVolts" runat="server" text="Label"></asp:label>
        </td>
        <td class="style2" colspan="2">
            <asp:label id="lblajbBlueScaledVoltsS" runat="server" text="Label"></asp:label>
        </td>
    </tr>
    <tr>
        <td colspan="4">Voltage Blue phase value (V):</td>
        <td>
            <asp:label id="lblBluePhaseVolts" runat="server" text="Label"></asp:label>
        </td>
        <td class="style3">&nbsp;</td>
        <td colspan="5">Current Blue phase value (A):</td>
        <td>
            <asp:label id="lblBluePhaseAmps" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="style7" colspan="4">Voltage Blue phase angle(120deg)</td>
        <td class="style7">
            <asp:label id="lblBluePhaseAngle" runat="server" text="Label"></asp:label>
        </td>
        <td class="style8"></td>
        <td class="style7" colspan="5">Current Blue phase angle (deg):</td>
        <td class="style7">
            <asp:label id="lblBluePhaseCurAngle" runat="server" text="Label"></asp:label>
        </td>
        <td class="style7" colspan="2">Current scaled values
        </td>
        <td class="style7" colspan="3">Original a+jb values</td>
        <td class="style7">Normalised angles</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="style3">&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>
            <asp:label id="lblRedScaledCurrent" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblRedScaledCurrentS" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblajbRedScaledCur" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblajbRedScaledCurS" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbRedCurNorAngle" runat="server" text="Label"></asp:label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="style3">&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>
            <asp:label id="lblWhiteScaledCurrent" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblWhiteScaledCurrentS" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblajbWhiteScaledCur" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblajbWhiteScaledCurS" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbWhiteCurNorAngle" runat="server" text="Label"></asp:label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="style3">&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>
            <asp:label id="lblBlueScaledCurrent" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblBlueScaledCurrentS" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblajbBlueScaledCur" runat="server" text="Label"></asp:label>
        </td>
        <td>
            <asp:label id="lblajbBlueScaledCurS" runat="server" text="Label"></asp:label>
        </td>
        <td colspan="2">
            <asp:label id="lblajbBlueCurNorAngle" runat="server" text="Label"></asp:label>
        </td>
    </tr>
    <tr>
        <td colspan="2">Power Factor Red Phase:</td>
        <td colspan="2">
            <asp:label id="lblRedPowerFactor" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td colspan="11" rowspan="13">
            <igchart:UltraChart ID="PhasorDiagram" runat="server"
                BackgroundImageFileName="" BorderWidth="0px" ChartType="ScatterChart"
                EmptyChartText="Data Not Available. Please call UltraChart.Data.DataBind() after setting valid Data.DataSource"
                Height="329px" Version="7.1" Width="725px">
                <effects><Effects>
<igchartprop:GradientEffect></igchartprop:GradientEffect>
</Effects>
</effects>

                <data maxvalue="100" minvalue="-100">
                </data>

                <titletop text="Phasor Diagram"></titletop>

                <titleleft text="jb"></titleleft>

                <titlebottom text="a"></titlebottom>

                <colormodel modelstyle="CustomLinear" colorbegin="Pink" colorend="DarkRed" alphalevel="150"></colormodel>

                <axis>
<PE ElementType="None" Fill="Cornsilk"></PE>

<X Visible="True" LineThickness="1" TickmarkStyle="Smart" TickmarkInterval="10">
<MajorGridLines Visible="True" DrawStyle="Dot" Color="Gainsboro" Thickness="1" AlphaLevel="255"></MajorGridLines>

<MinorGridLines Visible="False" DrawStyle="Dot" Color="LightGray" Thickness="1" AlphaLevel="255"></MinorGridLines>

<Labels ItemFormatString="&lt;DATA_VALUE:00.##&gt;" Font="Verdana, 7pt" FontColor="DimGray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="VerticalLeftFacing">
<SeriesLabels FormatString="" Font="Verdana, 7pt" FontColor="DimGray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="VerticalLeftFacing">
<Layout Behavior="Auto"></Layout>
</SeriesLabels>

<Layout Behavior="Auto"></Layout>
</Labels>
</X>

<Y Visible="True" LineThickness="1" TickmarkStyle="Smart" TickmarkInterval="40">
<MajorGridLines Visible="True" DrawStyle="Dot" Color="Gainsboro" Thickness="1" AlphaLevel="255"></MajorGridLines>

<MinorGridLines Visible="False" DrawStyle="Dot" Color="LightGray" Thickness="1" AlphaLevel="255"></MinorGridLines>

<Labels ItemFormatString="&lt;DATA_VALUE:00.##&gt;" Font="Verdana, 7pt" FontColor="DimGray" HorizontalAlign="Far" VerticalAlign="Center" Orientation="Horizontal">
<SeriesLabels FormatString="" Font="Verdana, 7pt" FontColor="DimGray" HorizontalAlign="Far" VerticalAlign="Center" Orientation="Horizontal">
<Layout Behavior="Auto"></Layout>
</SeriesLabels>

<Layout Behavior="Auto"></Layout>
</Labels>
</Y>

<Y2 Visible="False" LineThickness="1" TickmarkStyle="Smart" TickmarkInterval="40">
<MajorGridLines Visible="True" DrawStyle="Dot" Color="Gainsboro" Thickness="1" AlphaLevel="255"></MajorGridLines>

<MinorGridLines Visible="False" DrawStyle="Dot" Color="LightGray" Thickness="1" AlphaLevel="255"></MinorGridLines>

<Labels ItemFormatString="&lt;DATA_VALUE:00.##&gt;" Visible="False" Font="Verdana, 7pt" FontColor="Gray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="Horizontal">
<SeriesLabels FormatString="" Font="Verdana, 7pt" FontColor="Gray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="Horizontal">
<Layout Behavior="Auto"></Layout>
</SeriesLabels>

<Layout Behavior="Auto"></Layout>
</Labels>
</Y2>

<X2 Visible="False" LineThickness="1" TickmarkStyle="Smart" TickmarkInterval="10">
<MajorGridLines Visible="True" DrawStyle="Dot" Color="Gainsboro" Thickness="1" AlphaLevel="255"></MajorGridLines>

<MinorGridLines Visible="False" DrawStyle="Dot" Color="LightGray" Thickness="1" AlphaLevel="255"></MinorGridLines>

<Labels ItemFormatString="&lt;DATA_VALUE:00.##&gt;" Visible="False" Font="Verdana, 7pt" FontColor="Gray" HorizontalAlign="Far" VerticalAlign="Center" Orientation="VerticalLeftFacing">
<SeriesLabels FormatString="" Font="Verdana, 7pt" FontColor="Gray" HorizontalAlign="Far" VerticalAlign="Center" Orientation="VerticalLeftFacing">
<Layout Behavior="Auto"></Layout>
</SeriesLabels>

<Layout Behavior="Auto"></Layout>
</Labels>
</X2>

<Z Visible="False" LineThickness="1" TickmarkStyle="Smart" TickmarkInterval="0">
<MajorGridLines Visible="True" DrawStyle="Dot" Color="Gainsboro" Thickness="1" AlphaLevel="255"></MajorGridLines>

<MinorGridLines Visible="False" DrawStyle="Dot" Color="LightGray" Thickness="1" AlphaLevel="255"></MinorGridLines>

<Labels ItemFormatString="" Visible="False" Font="Verdana, 7pt" FontColor="DimGray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="Horizontal">
<SeriesLabels Font="Verdana, 7pt" FontColor="DimGray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="Horizontal">
<Layout Behavior="Auto"></Layout>
</SeriesLabels>

<Layout Behavior="Auto"></Layout>
</Labels>
</Z>

<Z2 Visible="False" LineThickness="1" TickmarkStyle="Smart" TickmarkInterval="0">
<MajorGridLines Visible="True" DrawStyle="Dot" Color="Gainsboro" Thickness="1" AlphaLevel="255"></MajorGridLines>

<MinorGridLines Visible="False" DrawStyle="Dot" Color="LightGray" Thickness="1" AlphaLevel="255"></MinorGridLines>

<Labels ItemFormatString="" Visible="False" Font="Verdana, 7pt" FontColor="Gray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="Horizontal">
<SeriesLabels Font="Verdana, 7pt" FontColor="Gray" HorizontalAlign="Near" VerticalAlign="Center" Orientation="Horizontal">
<Layout Behavior="Auto"></Layout>
</SeriesLabels>

<Layout Behavior="Auto"></Layout>
</Labels>
</Z2>
</axis>

                <tooltips font-bold="False" font-italic="False" font-overline="False" font-strikeout="False" font-underline="False"></tooltips>
            </igchart:UltraChart>
        </td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="style5" colspan="2">Power Factor White Phase</td>
        <td class="style5" colspan="2">
            <asp:label id="lblWhitePowerFactor" runat="server" text="Label"></asp:label>
        </td>
        <td class="style5"></td>
        <td class="style5" colspan="2"></td>
    </tr>
    <tr>
        <td colspan="2">Power Factor Blue Phase</td>
        <td colspan="2">
            <asp:label id="lblBluePowerFactor" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">System Power Factor</td>
        <td colspan="2">
            <asp:label id="lblSysPowerFactor" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="style6" colspan="2">Voltage nominal</td>
        <td class="style6" colspan="2">230</td>
        <td class="style6"></td>
        <td class="style6" colspan="2"></td>
    </tr>
    <tr>
        <td colspan="2">Red phase % variance</td>
        <td colspan="2">
            <asp:label id="lblRedPhaseVariance" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="style7" colspan="2">White phase % variance</td>
        <td class="style7" colspan="2">
            <asp:label id="lblWhitePhaseVariance" runat="server" text="Label"></asp:label>
        </td>
        <td class="style7"></td>
        <td class="style7" colspan="2"></td>
    </tr>
    <tr>
        <td colspan="2">Blue phase % variance</td>
        <td colspan="2">
            <asp:label id="lblBluePhaseVariance" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">Current Verification</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">Neutral current magnitude (A)</td>
        <td colspan="2">
            <asp:label id="lblCurrent2Neutral" runat="server" text="Label"></asp:label>
        </td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="3">Data Date</td>
        <td colspan="5">
            <asp:dropdownlist id="ddlDataSelection" runat="server" height="17px"
                width="195px" autopostback="True" causesvalidation="True">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem></asp:ListItem>
            </asp:dropdownlist>
        </td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="style3">&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td colspan="2">&nbsp;</td>
        <td colspan="2">&nbsp;</td>
    </tr>
</table>