<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="userEdit.aspx.cs" Inherits="website2016V2.userEdit" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <h3>Edit Users</h3>

     <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Exsisting users"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                     

    <div>
        <table style="width: 100%">

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

            <tr>
                <td colspan="12">
                    <igtbl:UltraWebGrid ID="gridUsers" OnActiveRowChange="gridUsers_ActiveRowChange" runat="server" Width="100%">
                        <Bands>
                            <igtbl:UltraGridBand>
                                <AddNewRow View="NotSet" Visible="NotSet">
                                </AddNewRow>
                                <Columns>
                                    <igtbl:UltraGridColumn HeaderText="Firstname">
                                        <Header Caption="Firstname">
                                        </Header>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Surname">
                                        <Header Caption="Surname">
                                            <RowLayoutColumnInfo OriginX="1" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="1" />
                                        </Footer>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Username">
                                        <Header Caption="Username">
                                            <RowLayoutColumnInfo OriginX="2" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="2" />
                                        </Footer>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Level">
                                        <Header Caption="Level">
                                            <RowLayoutColumnInfo OriginX="3" />
                                        </Header>
                                        <Footer>
                                            <RowLayoutColumnInfo OriginX="3" />
                                        </Footer>
                                    </igtbl:UltraGridColumn>
                                </Columns>
                            </igtbl:UltraGridBand>
                        </Bands>
                        <DisplayLayout BorderCollapseDefault="Separate" Name="UltraWebGrid1" RowHeightDefault="20px" 
                            SelectTypeRowDefault="Single" Version="3.00" >
                            <GroupByBox>
                                <Style BackColor="ActiveBorder" BorderColor="Window" ></Style>
                            </GroupByBox>
                            <ActivationObject BorderColor="1, 68, 208">
                            </ActivationObject>
                            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                            </FooterStyleDefault>
                            <RowStyleDefault BackColor="Window" BorderColor="#0144D0" BorderStyle="Solid" BorderWidth="1px">
                                <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                <Padding Left="3px" />
                            </RowStyleDefault>
                            <SelectedRowStyleDefault BackColor="#9FBEEB">
                            </SelectedRowStyleDefault>
                            <HeaderStyleDefault BackColor="Green" BorderStyle="Solid" ForeColor="White" HorizontalAlign="Left">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                            </HeaderStyleDefault>
                            <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                            </EditCellStyleDefault>
                            <FrameStyle BackColor="#E9F3FF" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
                                Font-Size="8pt" Width="544px">
                            </FrameStyle>
                            <Pager>
                                <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
                            </Pager>
                            <AddNewBox>
                                <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
                            </AddNewBox>
                        </DisplayLayout>
                    </igtbl:UltraWebGrid></td>
            </tr>
 </table>

     <br />
      <br />
           



              <div class="row">
                    <div class="col-md-2"> ID</div>
                    <div class="col-md-4">
            <igtxt:webtextedit id="txtID" runat="server"  PlaceHolder="Please enter ID" required="true" CssClass="form-control" Width="250px" Height="34px">
                    </igtxt:WebTextEdit>                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                    </div>
                </div>



             <div class="row">
                    <div class="col-md-2"> FirstName</div>
                    <div class="col-md-4">
                      <igtxt:webtextedit id="txtFirstname" runat="server" PlaceHolder="Please enter FirstName" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>
                  </div>
                    <div class="col-md-2">Surname</div>
                    <div class="col-md-4">
                         <igtxt:webtextedit id="txtSurname" runat="server"  PlaceHolder="Please enter Surname" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>
                    </div>
                </div>


            <div class="row">
                    <div class="col-md-2"> Level</div>
                    <div class="col-md-4">
                <asp:Textbox id="UserLevell" runat="server" max="99" min="1" Text="5" step="1" TextMode="Number"  required="true" CssClass="form-control" Width="250px" Height="34px">

             </asp:Textbox>      </div>
                    <div class="col-md-2">Phone Number</div>
                    <div class="col-md-4">
                   <igtxt:webmaskedit id="txtPhoneNumber" runat="server" displaymode="Mask" inputmask="(###) ###-####"   required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>

                    </div>
                </div>
   
               <div class="row">
                    <div class="col-md-2"> Fax Number</div>
                    <div class="col-md-4">
                                  <igtxt:webmaskedit id="txtFaxnumber" runat="server" displaymode="Mask" inputmask="(###) ###-####"  required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>
    </div>
                    <div class="col-md-2">Mobile Numberr</div>
                    <div class="col-md-4">
                    <igtxt:webmaskedit id="txtCellNumber" runat="server" displaymode="Mask" inputmask="(###) ###-####"   required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>

                    </div>
                </div>


                 <div class="row">
                    <div class="col-md-2">  Pager Number</div>
                    <div class="col-md-4">
                    <igtxt:webmaskedit id="txtPager" runat="server" displaymode="Mask" inputmask="(###) ###-####"   required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webmaskedit>
    </div>
                    <div class="col-md-2">Address</div>
                    <div class="col-md-4">
                    <igtxt:webtextedit id="txtAddress" runat="server"  PlaceHolder="Please enter Address" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>

                    </div>
                </div>



                    <div class="row">
                    <div class="col-md-2">  Email</div>
                    <div class="col-md-4">
                    <igtxt:webtextedit id="txtEmail" runat="server"  PlaceHolder="Please enter Email" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>
    </div>
                    <div class="col-md-2">Username</div>
                    <div class="col-md-4">
                    <igtxt:webtextedit id="txtUserName" runat="server"  PlaceHolder="Please enter username" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>

                    </div>
                </div>


        
                    <div class="row">
                    <div class="col-md-2">   Password</div>
                    <div class="col-md-4">
                    <igtxt:webtextedit id="txtPassword" runat="server" passwordmode="True"  PlaceHolder="Please enter Password" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>
    </div>
                    <div class="col-md-2">Confirm</div>
                    <div class="col-md-4">
                    <igtxt:webtextedit id="txtPasswordConfirm" runat="server" passwordmode="True"  PlaceHolder="Please Confirm password" required="true" CssClass="form-control" Width="250px" Height="34px"></igtxt:webtextedit>
                        <input type="hidden" id="PeopleID" runat="server"/>
                    </div>
                </div>
   
         
            
           
           
                    <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" Width="480px"></asp:Label></td>
            <br />

        <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                    <asp:Button ID="cmdUpdate" runat="server" Text="Update"  Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdUpdate_Click" />                    </div>
                    <div class="col-md-2">
                       
                    </div>
             <div class="col-md-4">
                                                <asp:Button ID="cmdDelete" runat="server" Text="Delete"  Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClientClick="return confirm('Are you sure you want to delete this User?');" OnClick="cmdDelete_Click" />

                        </div>
                    
                </div>

              
              
    
    </div>
</div>
</div>
         </div>
    <br />


</asp:Content>
