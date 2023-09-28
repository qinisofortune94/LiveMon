<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CamSensLink.aspx.cs" Inherits="website2016V2.CamSensLink" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
     <h3>Selected Camera </h3>
    <br />

        <div id="form1" runat="server">
            <asp:Label ID="Label5" runat="server" Text="Selected Camera"></asp:Label>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <a href="../website/helpfiles/CamSensLink.htm"
            target="_help" title="Show help for this page!"><img id="IMG1" src="images/helpSystem/root.gif" /></a> <br />
            <asp:Label ID="lblCamera" runat="server" Text="Label" Width="104px"></asp:Label><br />
            <igtbl:UltraWebGrid ID="grdCamSensLink" runat="server" Height="200px" Width="804px" OnInitializeLayout="grdCamSensLink_InitializeLayout">
                <Bands>
                    <igtbl:UltraGridBand>
                        <AddNewRow View="NotSet" Visible="NotSet">
                        </AddNewRow>
                        <Columns>
                            <igtbl:UltraGridColumn HeaderText="Camera">
                                <Header Caption="Camera">
                                </Header>
                            </igtbl:UltraGridColumn>
                            <igtbl:UltraGridColumn HeaderText="Sensor">
                                <Header Caption="Sensor">
                                    <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                </Header>
                                <Footer>
                                    <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                                </Footer>
                            </igtbl:UltraGridColumn>
                            <igtbl:UltraGridColumn HeaderText="View Level">
                                <Header Caption="View Level">
                                    <RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                </Header>
                                <Footer>
                                    <RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
                                </Footer>
                            </igtbl:UltraGridColumn>
                            <igtbl:UltraGridColumn HeaderText="Execute Level">
                                <Header Caption="Execute Level">
                                    <RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
                                </Header>
                                <Footer>
                                    <RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
                                </Footer>
                            </igtbl:UltraGridColumn>
                            <igtbl:UltraGridColumn HeaderText="Delete" Type="Button" CellButtonDisplay="Always">
                                <Header Caption="Delete">
                                    <RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
                                </Header>
                                <Footer>
                                    <RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
                                </Footer>
                            </igtbl:UltraGridColumn>
                        </Columns>
                    </igtbl:UltraGridBand>
                </Bands>
                <DisplayLayout Version="3.00" Name="UltraWebGrid1"
                    BorderCollapseDefault="Separate"
                    RowHeightDefault="20px" SelectTypeRowDefault="Single">
                    <GroupByBox>
                        <Style BorderColor="Window" BackColor="ActiveBorder"></Style>
<BoxStyle BackColor="ActiveBorder" BorderColor="Window"></BoxStyle>
                    </GroupByBox>
                    <FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
                        <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                    </FooterStyleDefault>
                    <RowStyleDefault BorderWidth="1px" BorderColor="#0144D0" BorderStyle="Solid" BackColor="Window">
                        <BorderDetails ColorTop="Window" ColorLeft="Window"></BorderDetails>
                        <Padding Left="3px"></Padding>
                    </RowStyleDefault>
                    <HeaderStyleDefault HorizontalAlign="Left" BorderStyle="Solid" BackColor="Green" ForeColor="White">
                        <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                    </HeaderStyleDefault>
                    <EditCellStyleDefault BorderWidth="0px" BorderStyle="None">
                    </EditCellStyleDefault>
                    <FrameStyle BorderWidth="1px" BorderStyle="Solid" Font-Size="8pt"
                        Font-Names="Verdana" BackColor="#E9F3FF" Width="804px" Height="200px">
                    </FrameStyle>
                    <Pager>
                        <Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
<PagerStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid">
<BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px"></BorderDetails>
</PagerStyle>
                    </Pager>
                    <AddNewBox>
                        <Style BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" BackColor="Window">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
<BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid">
<BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px"></BorderDetails>
</BoxStyle>
                    </AddNewBox>
                    <ActivationObject BorderColor="1, 68, 208">
                    </ActivationObject>
                    <SelectedRowStyleDefault BackColor="#9FBEEB">
                    </SelectedRowStyleDefault>
                </DisplayLayout>
            </igtbl:UltraWebGrid>
            <br />
            <table style="width: 568px" >
              
            <tr><td>
                <asp:Label ID="Label1" runat="server" Text="Camera" Width="144px"></asp:Label></td></tr>
            <tr><td>
                <asp:DropDownList ID="cmbCamera" runat="server"  CssClass="form-control" Width="250px" Height="34px"    >
                </asp:DropDownList></td></tr>
            <tr><td style="height: 10px">
                <asp:Label ID="Label2" runat="server" Text="Sensor" Width="144px"></asp:Label></td></tr>
            <tr><td>
                <asp:DropDownList ID="cmbSensor" runat="server" CssClass="form-control" Width="250px" Height="34px" >
                </asp:DropDownList></td></tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="View Level" Width="144px"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtViewLevel" runat="server" max="99" min="1" Text="5" step="1" TextMode="Number"   CssClass="form-control" Width="250px" Height="34px" >
                            
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Execute Level" Width="144px"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtExecuteLevel"  runat="server" max="99" min="1" Text="5" step="1" TextMode="Number"   CssClass="form-control" Width="250px" Height="34px" >
                        
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="height: 21px">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" class="btn btn-success form-control" BorderColor="#0099FF" Width="250px" Height="40px"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" Width="144px"></asp:Label></td>
                </tr>
            
            
            </table>
        </div>
</asp:Content>
