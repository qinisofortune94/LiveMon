<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CameraDynamic.aspx.cs" Inherits="website2016V2.CameraDynamic" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server" onload="refresh()">
 
<script type="text/javascript" src="CameraDynamic.aspx.js"></script>
    <%--    <bgsound id="bgsound1" loop="-1" balance="0" volume="0"> 
    <object classid="clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6" id="Player" height="40">
  <param name="AutoStart" value="True">
  <param name="uiMode" value="mini">
  <param name="URL" value="ReturnCurrentAudio.aspx?Camera=2"> 
</object>
--%>
    <h3>All Cameras</h3>
    <div id="divmenu" runat="server">

        <table style="width: 536px">

                <tr>
                <td colspan="3">
                    <div class="success" id="successMessage"  runat="server">
                            <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
                    </div>
                    <div class="warning" id="warningMessage"  runat="server">
                            <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
                    </div>
                        <div class="error" id="errorMessage"  runat="server">
                            <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
                    </div>

                </td>
            </tr>
            </table>
  <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="All Cameras"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
        <div>

             <asp:DropDownList ID="cmbCurrentSite" runat="server" 
                            Visible="False"  CssClass="form-control" Width="250px" Height="34px"  AutoPostBack="True" ToolTip="Select Site to view ." OnSelectedIndexChanged="cmbCurrentSite_SelectedIndexChanged">
                </asp:DropDownList>

        </div>

        <asp:Label ID="Label3" runat="server" Text="Choose Camera/s"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; <a href="HelpFiles/allcameras.htm" target="_help" title="Show help for this page!">
        </a><asp:CheckBoxList ID="chkCameras"
                runat="server" Height="64px" RepeatColumns="3" Width="368px">
            </asp:CheckBoxList>

            
                 <div class="row">
                    <div class="col-md-2">Refresh Rate</div>
                    <div class="col-md-4">
                          <asp:DropDownList ID="cmbRefreshRate" runat="server" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="cmbRefreshRate_SelectedIndexChanged" ></asp:DropDownList>
                          <asp:Button ID="cmdRefresh" runat="server"  Text="Change" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdRefresh_Click" /></div>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                       
                    </div>
                </div>
       <%-- <asp:Label ID="Label2" runat="server" Text="Refresh Rate"></asp:Label><br />

        <asp:DropDownList ID="cmbRefreshRate" runat="server" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="cmbRefreshRate_SelectedIndexChanged" ></asp:DropDownList>--%>

 <%--   <asp:DropDownList ID="cmbRefreshRate" runat="server" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="cmbRefreshRate_SelectedIndexChange--%>


    <%--    </asp:DropDownList>--%>
        <br />
       
    <table id="tbl1" border="0" runat="server">
        <tr>
            <td style="width: 5px">
                <div id="Div1" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan1" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div2" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan2" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div3" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan3" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 5px">
                <div id="Div4" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan4" runat="server">
                </div>
            </td>
            <td>
                <div id="Div5" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan5" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div6" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan6" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 5px">
                <div id="Div7" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan7" runat="server">
                </div>
            </td>
            <td>
                <div id="Div8" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan8" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div9" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan9" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 5px">
                <div id="Div10" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan10" runat="server">
                </div>
            </td>
            <td>
                <div id="Div11" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan11" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div12" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan12" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 5px">
                <div id="Div13" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan13" runat="server">
                </div>
            </td>
            <td>
                <div id="Div14" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan14" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div15" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan15" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 5px">
                <div id="Div16" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan16" runat="server">
                </div>
            </td>
            <td>
                <div id="Div17" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan17" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div18" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan18" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 5px">
                <div id="Div19" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan19" runat="server">
                </div>
            </td>
            <td>
                <div id="Div20" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan20" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="Div21" runat="server">
                </div>
            </td>
            <td style="width: 5px">
                <div id="CntrlPan21" runat="server">
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" name="Refresh" id="Refresh" value="3000" runat="server" />
    <input type="hidden" name="Text1" id="Hidden1" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden2" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden3" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden4" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden5" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden6" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden7" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden8" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden9" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden10" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden11" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden12" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden13" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden14" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden15" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden16" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden17" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden18" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden19" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden20" value="0" runat="server" />
    <input type="hidden" name="Text1" id="Hidden21" value="0" runat="server" />
    <input type="hidden" name="Inputs" id="Inputs" value="" runat="server" />


    </div>
   </div>

   
</asp:Content>
