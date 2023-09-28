<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashboardKVA.aspx.cs" Inherits="website2016V2.DashboardKVA" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <%--<script src="../Scripts/jquery-1.8.2.js"></script>--%>
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/highcharts.js"></script>
    <script src="Scripts/exporting.js"></script>
    <script src="Scripts/drilldown.js"></script>
    <script src="js/jquery.cookie.js"></script>
    <script src="js/jquery.sumoselect.min.js"></script>
    <script src="Scripts/ShiftEffDashboard.js"></script>
     <link href="../Styles/jquery-ui-1.8.20.custom.min.css" rel="stylesheet" />
  <script src="../Scripts/jquery-ui-1.8.20.custom.min.js"></script>
    <style>
        #ShiftDisplay {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

            #ShiftDisplay td, #ShiftDisplay th {
                border: 1px solid #ddd;
                padding: 2px;
            }

            #ShiftDisplay tr:nth-child(even) {
                background-color: #f2f2f2;
            }

            #ShiftDisplay tr:hover {
                background-color: #ddd;
            }

            #ShiftDisplay th {
                padding-top: 2px;
                padding-bottom: 2px;
                text-align: center;
                background-color: #4CAF50;
                color: white;
            }
    </style>
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="left_col" role="main">
        <h3>KVA Dashboard</h3>
        <div id="accordion" style="color: black" role="tablist" aria-multiselectable="true">
            <div class="panel panel-default" style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server"  ViewStateMode="Enabled" >

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="row" runat="server">
                            <div class="col-md-2">
                       <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="5000" />
                        Last Update: <asp:Label ID="lbl1" runat="server" Text=""></asp:Label>
                         <p></p> 
                             </div> 
                           
                           <%-- <div class="col-md-2">
                         <asp:DropDownList ID="Dashboards" runat="server" Height="34px" Width="160px" AutoPostBack ="true" OnSelectedIndexChanged="Dashboards_SelectedIndexChanged"  >
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>---Add Dashboard/Edit---</asp:ListItem>   
                                </asp:DropDownList>
                                </div>--%>
                            <div class="col-md-2">
                                <div id ="green" style="background-color: forestgreen">0% - 50%</div>
                                </div>
                                <div class="col-md-2">
                                    <div id ="yellow" style="background-color: yellow">51% - 79%</div>
                                    </div>
                            <div class="col-md-2">
                                <div id ="red" style="background-color: red">80% - 100%</div>
                                </div>

                             
                        </div>
                            <br />                
                        <div id="Shifttbl"  runat="server" visible ="true">
                            <table id='ShiftDisplay' style='width:100%' border="1">
                                <tr><th colspan="5" style="align-content:center">KVA Dashboard</th></tr>
                                <tr><th colspan="2">Required Production</th><th></th><th colspan="2" style="background-color:#C0C0C0">Actual Production</th></tr>
                                <tr><td colspan="2">0</td><td></td><td colspan="2" style="background-color:#C0C0C0">0</td></tr>
                                <tr>
                                    <td>
                                    <table style='width:100%' border="1">
                                        <tr><th>Machine:11</th><th>WO:20Inch widget</th></tr>
                                        <tr><th colspan="2" title="Tool tip here!">Air bags 765r make</th></tr>
                                        <tr><td colspan="2" title="Tool tip here!">Planned:0</td></tr>                                
                                        <tr><td colspan="2" style="background-color:forestgreen">Made:10</td></tr>
                                    </table>
                                    </td>
                                    <td>
                                    <table>
                                        <tr><th>Machine:12</th><th>WO:10Inch widget</th></tr>
                                        <tr><th colspan="2">Air bags 4tgg make</th></tr>
                                       <tr><td colspan="2">Planned:0</td></tr>                                
                                        <tr><td colspan="2" style="background-color:red">Made:10</td></tr>
                                    </table>
                                    </td>
                                    <td>
                                    <table>
                                        <tr><th>Machine:1</th><th>WO:0Inch widget</th></tr>
                                        <tr><th colspan="2">Air bags 65r77 make</th></tr>
                                        <tr><td colspan="2">Planned:0</td></tr>                                
                                        <tr><td colspan="2" style="background-color:yellow">Made:10</td></tr>
                                    </table>
                                    </td>
                                    <td>
                                    <table>
                                        <tr style="background-color:lawngreen"><th>Machine:2</th><th>WO:2Inch widget</th></tr>
                                        <tr style="background-color:lawngreen"><th colspan="2">Air bags fggd make</th></tr>
                                       <tr style="background-color:lawngreen"><td colspan="2">Planned:0</td></tr>                                
                                        <tr><td colspan="2" style="background-color:gray">Made:10</td></tr>
                                    </table>
                                    </td>
                                    <td>
                                    <table>
                                        <tr><th>Machine:6</th><th>WO:6Inch widget</th></tr>
                                        <tr><th colspan="2">Air bags 433 make</th></tr>
                                       <tr><td colspan="2">Planned:0</td></tr>                                
                                        <tr><td colspan="2" style="background-color:forestgreen">Made:10</td></tr>
                                    </table>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>
