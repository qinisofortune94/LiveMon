<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashboardConfig.aspx.cs" Inherits="website2016V2.DashboardConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

  

    <h3>KVA Dashboard </h3>
     <div class="col-md">
        
    </div>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
             
                    </strong>
                    </a>
                </h4>
            </div>
                       
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                  <div class="row" runat="server">
                    <div class="col-md-2">Dashboards:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="Dashboards" runat="server" Height="34px" Width="250px" AutoPostBack ="true" OnSelectedIndexChanged="Dashboards_SelectedIndexChanged" >
             <asp:ListItem></asp:ListItem>
             <asp:ListItem>------Add Dashboard------</asp:ListItem>   
        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">Type:</div>
                      <div class="col-md-4">
                          <asp:DropDownList ID="Type" runat="server" Height="34px" Width="250px" OnSelectedIndexChanged="Type_SelectedIndexChanged" >
                        <asp:ListItem></asp:ListItem>
                    <asp:ListItem>Gridview</asp:ListItem>
                   <asp:ListItem>Gauge</asp:ListItem>                    
        </asp:DropDownList>
                               </div>
                </div>
                  
                <br />
                <div class="row" >
                    <div id="OtherDIV" runat="server" visible="false">
                   <div class="col-md-2">Name of Dashboard:</div>
                    <div class="col-md-4">
                      <asp:TextBox ID="txtDash" CssClass="form-control" Height="34px" Width="250px" runat="server"></asp:TextBox>
    
                        
                    </div>
                        
                    <div class="col-md-2">Select Sensors:</div>
                    <div class="col-md-4">
                        <asp:CheckboxList runat="server" id="Sens" Width ="220px" selectionmode="Multiple">
                 
                         
                         </asp:CheckboxList>
                    </div>
           
                         </div>
                        </div>
                </div>
                  
         
                

                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSave" runat="server" Text="" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSave_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="btnDelete" runat="server" Text="Edit/Delete" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnDelete_Click" />
                    </div>
                </div>

               <asp:Label ID="Message" Text ="Please Select Sensor/Type or name dashboard" ForeColor="Red" Visible="false" runat="server"></asp:Label>
            </div>
        </div>
    

</asp:Content>
