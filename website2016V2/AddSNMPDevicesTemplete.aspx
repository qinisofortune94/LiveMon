<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSNMPDevicesTemplete.aspx.cs" Inherits="website2016V2.AddSNMPDevicesTemplete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">


    <h3>Devices</h3>

    
    <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSucces" runat="server"></asp:Label>
    </div>
    <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
    </div>
    <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

           


            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">Caption</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtCaption" runat="server" PlaceHolder="Please enter caption" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Templete Name</div>
                       
                    <div class="col-md-4">
                         <asp:TextBox ID="TxtTempletName" runat="server" PlaceHolder="Please enter template name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    </div>

              
                 
                
                
                 <div class="row">
                    <div class="col-md-2">RemoteHost Name or IP</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRemoteHostName" runat="server" PlaceHolder="RemoteHost Name or IP" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">RemotePort</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRemotePort" runat="server" PlaceHolder="Please enter RemotePort" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">Community</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCommunity" runat="server" PlaceHolder="Please enter Community" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">LocalEngineId</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLocalEngineId" runat="server" PlaceHolder="Please enter LocalEngineId" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>

                  <div class="row">
                    <div class="col-md-2">Authentication Protocol</div>
                    <div class="col-md-4">
                         <asp:Dropdownlist ID="dllAuthenticationProtocol"  runat="server"  CssClass="form-control" Width="250px" Height="34px"></asp:Dropdownlist>
                    </div>
                    <div class="col-md-2">SNMP Version</div>
                    <div class="col-md-4">
                        <asp:Dropdownlist ID="ddlSNMPVersion"  runat="server"  CssClass="form-control" Width="250px" Height="34px" ></asp:Dropdownlist>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">RequestId</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="TxtRequestId" runat="server" PlaceHolder="Please enter RequestId" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">IPAddress-Local</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIPAddressLocal" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2">Timeout</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="TxtTimeout" runat="server" PlaceHolder="Please enter Timeout" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">User</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtUser" runat="server" PlaceHolder="Please enter User" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>

                 <div class="row">
                    <div class="col-md-2">Password</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="TxtPassword" runat="server" PlaceHolder="Please enter Password" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Confirm Password</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtConfirmPassword" runat="server" PlaceHolder="Please Confirm Password" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>

                 <div class="row">
                    <div class="col-md-2">Local Port</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="TxtLocalPort" runat="server" PlaceHolder="Please enter Local Port" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Data 1</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtData1" runat="server" PlaceHolder="Please enter Data 1" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                

                 <div class="row">
                    <div class="col-md-2">Data 2</div>
                    <div class="col-md-4">
                         <asp:TextBox ID="TxtData2" runat="server" PlaceHolder="Please enter Data 2" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">Data 3</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtData3" runat="server" PlaceHolder="Please enter Data 3" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>

            
            <h3>File Uploads</h3>
                  <div class="row">
                      <div class="col-md-2">Image No Response</div>
                    <div class="col-md-2">
                        <asp:FileUpload ID="filImageError" runat="server" />
                        <asp:Image ID="imgError" runat="server" Height="50px" />
                    </div>
                  </div>
                <div class="row">
                    <div class="col-md-2">Image Normal</div>
                    <div class="col-md-4">
                         <asp:FileUpload ID="filImageNormal" runat="server" />
                        <asp:Image ID="imgNormal" runat="server" Height="50px" />
                    </div>
                  </div>
                 
                <div class="row">
                     <div class="col-md-2">Image Error</div>
                    <div class="col-md-4">
       
                         <asp:FileUpload ID="filImageNoResponse" runat="server" />    
                         <asp:Image ID="imgResponse" runat="server" Height="50px" />
                    </div>
                  </div>

                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnAdd" runat="server" Text="Save" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                  >
                </div>
            </div>
        </div>
    </div>

     <%-- Display Role Part--%>


</asp:Content>
