<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KVAGraphDisplay.aspx.cs" Inherits="website2016V2.KVAGraphDisplay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
   
    <%-- <script src="Scripts3/highcharts.js"></script>--%>
   <script src="Scripts/highstock.js"></script>
    <script src="Scripts3/highcharts-more.js"></script>
    
    <script src="Scripts3/Modules/exporting.js"></script>
    <script src="Scripts3/Modules/export-data.js"></script>
    <script type="text/javascript" src="Scripts/jquery-2.1.0.min.js"></script>
        
    <%-- <script src="http://code.jquery.com/jquery-migrate-1.1.0.js"></script>--%>
     <script src="Scripts/LiveGraph.js"></script>
    <script src="Scripts/KVAGraphDisplay.js"></script>
    
    
   
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <h3>KVA Gauge Display </h3>
    <div class="col-md">
    </div>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong></strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <br />

                <div class="row" runat="server">

                    <%--<div class="col-md-2">
                        <asp:DropDownList ID="Dashboards" runat="server" Height="34px" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="Dashboards_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>---Add Dashboard/Edit---</asp:ListItem>
                        </asp:DropDownList>
                    </div>--%>
                    <div class="col-md-2">
                        Last Update:<label id="mylabel"></label>
                    </div>
                </div>
                <br />
                <div id="container"></div>
                <br />
         
                <table id= "NMDDash" border ="0" style="width:100%" runat="server" >
             
             <tr> <td > <div id="container0"></div></td>
               <td > <div id="container1"></div></td>
               <td > <div id="container2"></div></td></tr>
                    </table>
               

                
            </div>
        </div>
    </div>
    
                
</asp:Content>
