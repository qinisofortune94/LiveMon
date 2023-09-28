<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeteringPowerStats.aspx.cs" Inherits="website2016V2.MeteringPowerStats" %>
<%@ Register assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebDataInput" tagprefix="igtxt" %>
<%@ Register assembly="Infragistics2.WebUI.Misc.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.Misc" tagprefix="igmisc" %>
<%@ Register src="~/usercontrols/PowerStatusUserControl.ascx" tagname="PowerStatusUserControl" tagprefix="PowerStatusUserControl" %>
<%@ Register src="~/usercontrols/PowerTargetUserControl.ascx" tagname="PowerTargetUserControl" tagprefix="PowerTargetUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
  


    <div>
 <h3>Metering Stats</h3>
       
 <div id="accordion" role="tablist" aria-multiselectable="true">
    <div class="panel panel-default" style="width: 100%">
        <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Select Sensors"></asp:Label>
                    </strong>
                    </a>
                </h4>
        </div>
        <asp:Panel ID="Panel1" runat="server" height="160px" ScrollBars="Horizontal" style="overflow:scroll" width="100%">
            <asp:TreeView ID="tvSensors" runat="server">
            </asp:TreeView>
        </asp:Panel>
            
             
    </div>
 </div>



            
            
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                

     
            
   

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Metering States"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

           


            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
            
                <div class="row">
                        
                    <div class="col-md-2">  <asp:RadioButtonList ID="SelectDispType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectDispType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">Show All Values</asp:ListItem>
                        <asp:ListItem Value="1">Show Target KWh</asp:ListItem>
                    </asp:RadioButtonList></div>
                  
                    </div>
                   
            
                
                <div class="row">
                    <div class="col-md-2"> Date Set</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DefaultDateRange" runat="server"  CssClass="form-control" Width="250px" Height="34px" AutoPostBack="True">
                        <asp:ListItem Value="0">Per Hour</asp:ListItem>
                        <asp:ListItem Value="1">Per Day</asp:ListItem>
                        <asp:ListItem Value="2">Per 7 Day</asp:ListItem>
                        <asp:ListItem Value="3">Per Month</asp:ListItem>
                        <asp:ListItem Value="4">Per Year</asp:ListItem>
                    </asp:DropDownList>
                    </div>
                    <div class="col-md-2"></div>

                    <div class="col-md-4">
                    </div>
                    </div>

              
            

                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="CmdQuickGenerate" runat="server" Text="Generate" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    
                </div>
            </div>
        </div>
    </div>
             <tr>
                <td class="style1">
                    Data Set</td>
                <td style="width: 259px">
                 <%--   <asp:RadioButtonList ID="SelectDispType" runat="server" AutoPostBack="True">
                        <asp:ListItem Selected="True" Value="0">Show All Values</asp:ListItem>
                        <asp:ListItem Value="1">Show Target KWh</asp:ListItem>
                    </asp:RadioButtonList>--%>
          <%--          </td>
                <td style="width: 78px">
                </td>
            </tr>
            <tr>
            <td style="height: 24px; width: 259px;">
                    <asp:DropDownList ID="DefaultDateRange" runat="server" Width="168px">
                        <asp:ListItem Value="0">Per Hour</asp:ListItem>
                        <asp:ListItem Value="1">Per Day</asp:ListItem>
                        <asp:ListItem Value="2">Per 7 Day</asp:ListItem>
                        <asp:ListItem Value="3">Per Month</asp:ListItem>
                        <asp:ListItem Value="4">Per Year</asp:ListItem>
                    </asp:DropDownList>
                    <igtxt:webimagebutton ID="CmdQuickGenerate" runat="server" Text="Generate" 
                        Height="1px">
                    </igtxt:webimagebutton>
                </td>
                <td class="style2">
                   
                </td>--%>
                
           <%--     <td style="height: 24px; width: 78px;">
                    &nbsp;</td>
            </tr>
           --%>


  <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Power Usage"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
       <asp:PlaceHolder runat="server" 
               ID="PowerUsage" ></asp:PlaceHolder>
       <div id="PowerUsage1" runat="server" enableviewstate="true"></div>
    <div id="Charts" runat="server" enableviewstate="false" ></div>
       
        <br />
            </div>

      <div class="loading" align="center">
    Loading. Please wait.<br />
    <br />
    <img src="loader.gif" alt="" />
</div>
      </div>
    
    </div>
   
    		
    <div class="modal"><!-- Place at bottom of page --></div>

    
</asp:Content>
