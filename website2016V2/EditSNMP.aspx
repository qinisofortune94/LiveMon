<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditSNMP.aspx.cs" Inherits="website2016V2.EditSNMP" %>

<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <div>
            <div>
             <h3>Edit SNMP Device</h3>
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
                        <asp:Label ID="lblAdd" runat="server" Text="Edit Snmp Device"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

           


            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div id="TblDet" runat="server" visible="false">
                 
                    <div class="row">
                    <div class="col-md-2">Select Device</div>
                    <div class="col-md-4">
                           <asp:DropDownList ID="cmbDevices" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="cmbDevices_SelectedIndexChanged"  CssClass="form-control" Width="250px" Height="34px">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                     
                </div>
                      

                    </div>
                        
                 <div class="row">
                    <div class="col-md-2">RemoteHost</div>
                    <div class="col-md-4">
                          <igtxt:WebTextEdit ID="txtRemoteHost" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                    </div>
                    <div class="col-md-2">RemotePort</div>
                    <div class="col-md-4">
                </div>
                     <igtxt:WebNumericEdit ID="txtRemotePort" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebNumericEdit>

                    </div>
                

                      <%--  <tr>
                            <td style="width: 263px">
                                RemoteHost</td>
                            <td>
                                RemotePort</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebTextEdit ID="txtRemoteHost" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                                <igtxt:WebNumericEdit ID="txtRemotePort" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebNumericEdit>
                            </td>
                            <td>
                            </td>
                        </tr>--%>



                     <div class="row">
                    <div class="col-md-2">Community</div>
                    <div class="col-md-4">
                          <igtxt:WebTextEdit ID="txtCommunity" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                    </div>
                    <div class="col-md-2">LocalEngineId</div>
                    <div class="col-md-4">
                </div>
                     <igtxt:WebTextEdit ID="txtLocalEngineId" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>

                    </div>
                </div>



                     <%--   <tr>
                            <td style="width: 263px">
                                Community</td>
                            <td>
                                LocalEngineId</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebTextEdit ID="txtCommunity" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                                <igtxt:WebTextEdit ID="txtLocalEngineId" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                            </td>
                        </tr>--%>




                    <div class="row">
                    <div class="col-md-2">AuthenticationProtocol</div>
                    <div class="col-md-4">
                         <asp:DropDownList ID="cmbAuthentication" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </asp:DropDownList>
                    </div>
                    <div class="col-md-2">SNMPVersion</div>
                    <div class="col-md-4">
                </div>
                      <asp:DropDownList ID="cmbSNMPVersion" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </asp:DropDownList>

                    </div>
             


                     <%--   <tr>
                            <td style="width: 263px">
                                AuthenticationProtocol</td>
                            <td>
                                SNMPVersion</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <asp:DropDownList ID="cmbAuthentication" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="cmbSNMPVersion" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </asp:DropDownList></td>
                            <td>
                            </td>
                        </tr>--%>



                         <div class="row">
                    <div class="col-md-2">RequestId</div>
                    <div class="col-md-4">
                           <igtxt:WebTextEdit ID="txtRequestID" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                    </div>
                    <div class="col-md-2">Local Interface
                                IPAddress</div>
                    <div class="col-md-4">
                </div>
                     <igtxt:WebTextEdit ID="txtIpAddress" runat="server" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>

                    </div>
             


                        
                        <%--    <td style="width: 263px">
                                RequestId</td>
                            <td>
                                Local Interface
                                IPAddress</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebTextEdit ID="txtRequestID" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                            <Table><tr>
                            <td>
                                <igtxt:WebTextEdit ID="txtIpAddress" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                               
                            </td>
                                </tr></Table>
                            
                            </td>
                        </tr>--%>



                        
                                  <div class="row">
                    <div class="col-md-2">Timeout</div>
                    <div class="col-md-4">
                             <igtxt:WebNumericEdit ID="txtTimeout" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebNumericEdit>
                    </div>
                    <div class="col-md-2">User</div>
                    <div class="col-md-4">
                </div>
                     <igtxt:WebTextEdit ID="txtUsername" runat="server" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>

                    </div>
                        
                       <%-- <tr>
                            <td style="width: 263px">
                                Timeout</td>
                            <td>
                                User</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebNumericEdit ID="txtTimeout" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control">
                                </igtxt:WebNumericEdit>
                            </td>
                            <td>
                                <igtxt:WebTextEdit ID="txtUsername" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                            </td>
                        </tr>--%>




                                        <div class="row">
                    <div class="col-md-2">Password</div>
                    <div class="col-md-4">
                             <igtxt:WebTextEdit ID="txtPassword" runat="server" PasswordMode="True" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                    </div>
                    <div class="col-md-2">Confirm Password</div>
                    <div class="col-md-4">
                </div>
                     <igtxt:WebTextEdit ID="txtPassword1" runat="server" PasswordMode="True"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>

                    </div>                    

                      <%--  <tr>
                            <td style="width: 263px">
                                Password</td>
                            <td>
                                Confirm Password</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebTextEdit ID="txtPassword" runat="server" PasswordMode="True" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                                <igtxt:WebTextEdit ID="txtPassword1" runat="server" PasswordMode="True" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                            </td>
                        </tr>--%>






                      <div class="row">
                    <div class="col-md-2">Port</div>
                    <div class="col-md-4">
                           <igtxt:WebNumericEdit ID="txtPort" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebNumericEdit>
                    </div>
                    <div class="col-md-2">Caption</div>
                    <div class="col-md-4">
                </div>
                       <igtxt:WebTextEdit ID="txtCaption" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>

                    </div>
                        
                      <%--  <tr>
                            <td style="width: 263px">
                                Port</td>
                            <td>
                                Caption</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebNumericEdit ID="txtPort" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebNumericEdit>
                            </td>
                            <td>
                                <igtxt:WebTextEdit ID="txtCaption" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                            </td>
                        </tr>--%>


                             <div class="row">
                    <div class="col-md-2">Data1</div>
                    <div class="col-md-4">
                         <igtxt:WebTextEdit ID="txtData1" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                    </div>
                    <div class="col-md-2">Data2</div>
                    <div class="col-md-4">
                </div>
                         <igtxt:WebTextEdit ID="txtData2" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>

                    </div>
    

                        
                       <%-- <tr>
                            <td style="width: 263px">
                                Data1</td>
                            <td>
                                Data2</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <igtxt:WebTextEdit ID="txtData1" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                                <igtxt:WebTextEdit ID="txtData2" runat="server" PlaceHolder="Please enter IPAddress-Local" required="true" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td>
                            </td>
                        </tr>--%>


                      <div class="row">
                    <div class="col-md-2">Data3</div>
                    <div class="col-md-4">
                         <igtxt:WebTextEdit ID="txtData3" runat="server" CssClass="form-control" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                </div>
                       

                    </div>
                       <%-- <tr>
                            <td style="width: 263px; height: 22px;">
                                Data3</td>
                            <td style="height: 22px">
                            </td>
                            <td style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px; height: 26px;">
                                <igtxt:WebTextEdit ID="txtData3" runat="server" Width="350px">
                                </igtxt:WebTextEdit>
                            </td>
                            <td style="height: 26px">
                                &nbsp;
                            </td>
                            <td style="height: 26px">
                            </td>
                        </tr>--%>
                            

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


                        
                            <%--<td style="width: 263px">
                                ImageNormal<asp:Image ID="imgNormal" runat="server" Height="16px" /></td>
                            <td>
                                ImageError<asp:Image ID="imgError" runat="server" /></td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <asp:FileUpload ID="filImageNormal" runat="server" /></td>
                            <td>
                                <asp:FileUpload ID="filImageError" runat="server" /></td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                ImageNoResponse<asp:Image ID="imgResponse" runat="server" /></td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 263px">
                                <asp:FileUpload ID="filImageNoResponse" runat="server" /></td>
                            <td>
                                &nbsp;</td>
                            <td>
                            </td>
                        </tr>
                         <tr>--%>
                 <%--   <td style="width: 5px">
                        <asp:Label ID="Label17" runat="server" Text="Device Location" Width="125px"></asp:Label>
                    </td>
                    <td style="width: 358px">
                        <asp:Label ID="Label18" runat="server" Text="Device Site" Width="125px"
                            Visible="False"></asp:Label>
                    </td>
                </tr>--%>
              <%--  <tr>--%>
                   <%-- <td style="width: 5px">
                        <asp:DropDownList ID="cmbLocations" runat="server"  CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="cmbLocations_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button runat="server" ID="btnChangeLocation" Text="Change Location" Visible ="true" Width="115px" />
                    </td>
                    <td style="width: 358px">
                        <asp:DropDownList ID="cmbSites" runat="server"  CssClass="form-control" Width="250px" Height="34px">
                        </asp:DropDownList><asp:Button ID="btnChangeSite" runat="server" Text="Change Site" Width="88px" Visible="true" />
                    </td>--%>



                <%--</tr>--%>



                 <div class="row">
                    <div class="col-md-2">Device Location</div>
                    <div class="col-md-4">
                         <asp:Dropdownlist ID="cmbLocations" runat="server" CssClass="form-control" Width="250px" Height="34px" ></asp:Dropdownlist>
                    </div>
                    <div class="col-md-2">Device Site</div>
                    <div class="col-md-4">
                         <asp:Dropdownlist ID="cmbSites" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:Dropdownlist>
                    </div>
                </div>
                       <%-- <tr>
                            <td colspan="3">
                                <asp:Label ID="lblErr" runat="server" ForeColor="Red" Width="512px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 263px; height: 21px">
                            </td>
                            <td style="height: 21px">
                                &nbsp;
                            </td>
                            <td style="height: 21px">
                            </td>
                        </tr>--%>



                     <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="cmdSave" runat="server" Text="Save" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdSave_Click"  />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="cmdDelete" runat="server" Text="Delete" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF"  OnClientClick="return confirm('Are you sure you want to delete this SNMP device?')" OnClick="cmdDelete_Click" />
                    </div>
                </div>
                <div>
                    <asp:Label ID="lblErr" runat="server"></asp:Label>

                </div>
                        
                      <%--  <tr>
                            <td style="width: 263px">
                                <asp:Button ID="cmdSave" runat="server" Text="Save" /></td>
                            <td><asp:Button ID="cmdDelete" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this SNMP device?');" /></td>
                            <td>
                            </td>
                        </tr>--%>
                   
                </div>
            </div>
        </div>
    </div>
        </div>
   



</asp:Content>
