<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServerConfiguration.aspx.cs" Inherits="website2016V2.ServerConfiguration" %>

<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">    

    <script>

        $(document).ready(function (ev) {
            if (window.igtbl_removeState) {
                __igtbl_removeState = window.igtbl_removeState
                igtbl_removeState = function (stateNode) {
                    if (stateNode && stateNode.childNodes)
                        return __igtbl_removeState(stateNode)
                }
            }
        })

    </script>
    
    <h3>Server Configuration</h3>

    <div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Server Configuration"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

             <%--   <a href="../website/helpfiles/serverconfig.htm" target="_help"
                    title="Show help for this page!">
                    <img src="images/helpSystem/root.gif" /></a><br />--%>
                <igtab:UltraWebTab ID="Devices" runat="server" Width="100%"
                    BorderColor="#0056D7" BorderStyle="Solid" BorderWidth="1px"
                    ThreeDEffect="False" Height="600px" SelectedTab="3">
                    <Tabs>
                        <igtab:Tab Text="SMS Config">
                            <ContentTemplate>
                                &nbsp;<igtbl:UltraWebGrid ID="SMSGrid" OnActiveRowChange="SMSGrid_ActiveRowChange" runat="server" Height="144px"  Width="80%">
                                    <Bands>
                                        <igtbl:UltraGridBand AddButtonCaption="Add" AddButtonToolTipText="Add new device"
                                            AllowAdd="Yes" AllowUpdate="Yes">
                                            <AddNewRow View="NotSet" Visible="NotSet">
                                            </AddNewRow>
                                            <Columns>
                                                <igtbl:UltraGridColumn HeaderText="SMS Device">
                                                    <Header Caption="SMS Device">
                                                    </Header>
                                                </igtbl:UltraGridColumn>
                                                <igtbl:UltraGridColumn HeaderText="Device Order">
                                                    <Header Caption="Device Order">
                                                        <RowLayoutColumnInfo OriginX="1" />
                                                    </Header>
                                                    <Footer>
                                                        <RowLayoutColumnInfo OriginX="1" />
                                                    </Footer>
                                                </igtbl:UltraGridColumn>
                                                <igtbl:UltraGridColumn HeaderText="Add/Edit">
                                                    <Header Caption="Add/Edit">
                                                        <RowLayoutColumnInfo OriginX="2" />
                                                    </Header>
                                                    <Footer>
                                                        <RowLayoutColumnInfo OriginX="2" />
                                                    </Footer>
                                                </igtbl:UltraGridColumn>
                                            </Columns>
                                        </igtbl:UltraGridBand>
                                    </Bands>
                                    <DisplayLayout BorderCollapseDefault="Separate" Name="SMSGrid" RowHeightDefault="20px" SelectTypeRowDefault="Single" Version="3.00">
                                        <GroupByBox>
                                            <style backcolor="ActiveBorder" bordercolor="Window"></style>
                                        </GroupByBox>
                                        <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                        </FooterStyleDefault>
                                        <RowStyleDefault BackColor="Window" BorderColor="#0144D0" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                            <Padding Left="3px" />
                                        </RowStyleDefault>
                                        <HeaderStyleDefault BackColor="#1E6BE7" BorderStyle="Solid" HorizontalAlign="Left" ForeColor="White">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                        </HeaderStyleDefault>
                                        <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                        </EditCellStyleDefault>
                                        <FrameStyle BackColor="#E9F3FF" BorderStyle="Solid"
                                            BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" Height="144px">
                                        </FrameStyle>
                                        <Pager>
                                            <style backcolor="LightGray" borderstyle="Solid" borderwidth="1px"></style>
                                        </Pager>
                                        <AddNewBox>
                                            <style backcolor="Window" bordercolor="InactiveCaption" borderstyle="Solid" borderwidth="1px"></style>
                                        </AddNewBox>
                                        <ActivationObject BorderColor="1, 68, 208">
                                        </ActivationObject>
                                        <SelectedRowStyleDefault BackColor="#9FBEEB">
                                        </SelectedRowStyleDefault>
                                    </DisplayLayout>
                                </igtbl:UltraWebGrid>
                                <asp:Button ID="btnAddGSM" runat="server" Text="Add GSM" OnClick="btnAddGSM_Click"/>
                                <asp:Button ID="btnAddSMTP" runat="server" Text="Add SMTP" OnClick="btnAddSMTP_Click"/>
                                <asp:Button ID="btnAddFTP" runat="server" Text="Add FTP" OnClick="btnAddFTP_Click"/>
                                <asp:Button ID="btnAddHttp" runat="server" Text="Add HTTP" OnClick="btnAddHttp_Click"/>
                                <igmisc:WebPanel ID="WebPanel1" runat="server" Height="380px" Width="400px" Visible="False">
                                    <Template>
                                        <asp:Label ID="Label6" runat="server" Text="Port"></asp:Label>
                                        <br />
                                        <igtxt:WebNumericEdit ID="txtComport" runat="server" Width="250px" Height="34px">
                                            <SpinButtons Display="OnRight" />
                                        </igtxt:WebNumericEdit>
                                        <br />
                                        <asp:Label ID="Label13" runat="server" Width="250px" Height="34px" Text="Speed"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddlComSpeed" runat="server" Width="250px" Height="34px" AutoPostBack="true">
                                        </asp:DropDownList><br />
                                        <asp:Label ID="Label7" runat="server" Text="databits"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddlStopBits" runat="server" Width="250px" Height="34px" AutoPostBack="true">
                                        </asp:DropDownList><br />
                                        <asp:Label ID="Label8" runat="server" Text="Parity"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddlComParity" runat="server" Width="250px" Height="34px" AutoPostBack="true">
                                        </asp:DropDownList><br />
                                        <asp:Label ID="Label9" runat="server" Text="Stop Bits"></asp:Label>
                                        <br />
                                        <igtxt:WebNumericEdit ID="txtComDataBits" runat="server" Width="250px" Height="34px" MaxValue="8" MinDecimalPlaces="None" MinValue="7" ValueText="8">
                                            <SpinButtons Display="OnRight" />
                                        </igtxt:WebNumericEdit>
                                        <br />
                                        <asp:Label ID="Label10" runat="server" Text="Buffer Size"></asp:Label><br />
                                        <igtxt:WebNumericEdit ID="txtBufferSize" runat="server" Width="250px" Height="34px" MaxValue="1000" MinValue="1" ValueText="300">
                                            <SpinButtons Display="OnRight" />
                                        </igtxt:WebNumericEdit>
                                        <br />
                                        <asp:Label ID="Label11" runat="server" Text="HandShaking"></asp:Label>
                                        &nbsp;
                                        <br />
                                        <asp:DropDownList ID="ddlHandShake" runat="server" Width="250px" Height="34px" AutoPostBack="true">                                         
                                        </asp:DropDownList>
                                        <br />
                                        <igtxt:WebImageButton ID="btnUpdateSMSGSMDevice" runat="server" Text="Update" OnClick="btnUpdateSMSGSMDevice_Click">
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="Webimagebutton1" runat="server" Text="Delete">
                                        </igtxt:WebImageButton>
                                    </Template>
                                </igmisc:WebPanel>
                                <igmisc:WebPanel ID="WebPanel2" runat="server" Height="380px" Width="400px" Visible="False">
                                    <Template>
                                        <asp:Label ID="Label12" runat="server" Text="SMTP Server"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtSMTPServerName" Width="250px" Height="34px" runat="server"></asp:TextBox><br />
                                        <asp:Label ID="Label14" runat="server" Text="From"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPFrom" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label15" runat="server" Text="To"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPTo" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label16" runat="server" Text="Subject"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPSubject" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label17" runat="server" Text="API ID"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPAPIId" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label18" runat="server" Text="User"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPUser" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label19" runat="server" Text="Password"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPPassword" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label20" runat="server" Text="Reply"></asp:Label><br />
                                        <asp:TextBox ID="txtSMTPReply" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <igtxt:WebImageButton ID="btnUpdateSMSEmail" runat="server" Text="Update" OnClick="btnUpdateSMSEmail_Click1">
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="btnDeleteSMSEmail" runat="server" Text="Delete">
                                        </igtxt:WebImageButton>
                                    </Template>
                                </igmisc:WebPanel>
                                <igmisc:WebPanel ID="WebPanel3" runat="server" Height="350px" Width="400px" Visible="False">
                                    <Template>
                                        <asp:Label ID="Label21" runat="server" Text="FTP Server"></asp:Label><br />
                                        <asp:TextBox ID="txtFTPServer" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label22" runat="server" Text="User"></asp:Label><br />
                                        <asp:TextBox ID="txtFTPUser" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label23" runat="server" Text="Password"></asp:Label><br />
                                        <asp:TextBox ID="txtFTPPassword" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label24" runat="server" Text="Remote Path"></asp:Label><br />
                                        <asp:TextBox ID="txtFTPRemotePath" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label26" runat="server" Text="Output String"></asp:Label><br />
                                        <asp:TextBox ID="txtFTPOutputString" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <igtxt:WebImageButton ID="btnUpdateFTP" runat="server" Text="Update" OnClick="btnUpdateFTP_Click">
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="btnDeleteFTP" runat="server" Text="Delete">
                                        </igtxt:WebImageButton>
                                    </Template>
                                </igmisc:WebPanel>
                                <igmisc:WebPanel ID="WebPanel4" runat="server" Height="350px" Width="400px" Visible="False">
                                    <Template>
                                        <asp:Label ID="Label25" runat="server" Text="HTTP Get URL"></asp:Label><br />
                                        <asp:TextBox ID="txtHttpURL" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label27" runat="server" Text="User"></asp:Label><br />
                                        <asp:TextBox ID="txtHttpUser" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label28" runat="server" Text="Password"></asp:Label><br />
                                        <asp:TextBox ID="txtHttpPassword" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label29" runat="server" Text="Reply URL"></asp:Label><br />
                                        <asp:TextBox ID="txtHttpReplyURL" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <asp:Label ID="Label30" runat="server" Text="API ID"></asp:Label><br />
                                        <asp:TextBox ID="txtHttpAPIId" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                        <igtxt:WebImageButton ID="btnHttpUpdate" runat="server" Text="Update" OnClick="btnHttpUpdate_Click">
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="btnHttpDelete" runat="server" Text="Delete">
                                        </igtxt:WebImageButton>
                                    </Template>
                                </igmisc:WebPanel>
                            </ContentTemplate>
                        </igtab:Tab>
                        <igtab:Tab Text="Email Config">
                            <ContentTemplate>
                                <igtbl:UltraWebGrid ID="EmailGrid" runat="server" OnActiveRowChange="EmailGrid_ActiveRowChange" Height="160px" Width="80%"  AutoGenerateSelectButton="True" >
                                    <Bands>
                                        <igtbl:UltraGridBand AddButtonCaption="Add" AddButtonToolTipText="Add new device"
                                            AllowAdd="Yes" AllowUpdate="Yes">
                                            <AddNewRow View="NotSet" Visible="NotSet">
                                            </AddNewRow>
                                            <Columns>
                                                <igtbl:UltraGridColumn HeaderText="Email Device">
                                                    <Header Caption="Email Device">
                                                    </Header>
                                                </igtbl:UltraGridColumn>
                                                <igtbl:UltraGridColumn HeaderText="Device Order">
                                                    <Header Caption="Device Order">
                                                        <RowLayoutColumnInfo OriginX="1" />
                                                    </Header>
                                                    <Footer>
                                                        <RowLayoutColumnInfo OriginX="1" />
                                                    </Footer>
                                                </igtbl:UltraGridColumn>
                                                <igtbl:UltraGridColumn HeaderText="Add/Edit">
                                                    <Header Caption="Add/Edit">
                                                        <RowLayoutColumnInfo OriginX="2" />
                                                    </Header>
                                                    <Footer>
                                                        <RowLayoutColumnInfo OriginX="2" />
                                                    </Footer>
                                                </igtbl:UltraGridColumn>
                                            </Columns>
                                        </igtbl:UltraGridBand>
                                    </Bands>
                                    <DisplayLayout BorderCollapseDefault="Separate" Name="EmailGrid" RowHeightDefault="20px" SelectTypeRowDefault="Single" Version="3.00">
                                        <GroupByBox>
                                            <style Backcolor="ActiveBorder" bordercolor="Window"></style>
                                        </GroupByBox>
                                        <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                        </FooterStyleDefault>
                                        <RowStyleDefault BackColor="Window" BorderColor="#0144D0" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                            <Padding Left="3px" />
                                        </RowStyleDefault>
                                        <HeaderStyleDefault BackColor="#1E6BE7" BorderStyle="Solid" HorizontalAlign="Left" ForeColor="White">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                        </HeaderStyleDefault>
                                        <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                        </EditCellStyleDefault>
                                        <FrameStyle BackColor="#E9F3FF" BorderStyle="Solid"
                                            BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" Height="160px">
                                        </FrameStyle>
                                        <Pager>
                                            <style backcolor="LightGray" borderstyle="Solid" borderwidth="1px"></style>
                                        </Pager>
                                        <AddNewBox>
                                            <style backcolor="Window" bordercolor="InactiveCaption" borderstyle="Solid" borderwidth="1px"></style>
                                        </AddNewBox>
                                        <ActivationObject BorderColor="1, 68, 208">
                                        </ActivationObject>
                                        <SelectedRowStyleDefault BackColor="#9FBEEB">
                                        </SelectedRowStyleDefault>
                                    </DisplayLayout>
                                </igtbl:UltraWebGrid>
                                <igmisc:WebPanel ID="EmailEdit" runat="server" Height="200px" Width="400px" Visible="False">
                                    <Template>
                                        <asp:Label ID="Label5" runat="server" Text="ID"></asp:Label><br />
                                        <igtxt:WebTextEdit ID="txtEmailID" runat="server" Enabled="false" Width="250px" Height="34px">
                                        </igtxt:WebTextEdit>
                                        <br />
                                        &nbsp;<asp:Label ID="Label3" runat="server" Text="SMTP Server"></asp:Label><br />
                                        <igtxt:WebTextEdit ID="txtEmailServer" runat="server" Width="250px" Height="34px">
                                        </igtxt:WebTextEdit>
                                        <br />
                                        <asp:Label ID="Label4" runat="server" Text="From Adress"></asp:Label><br />
                                        &nbsp;<igtxt:WebTextEdit ID="txtAdress" runat="server" Width="250px" Height="34px">
                                        </igtxt:WebTextEdit>
                                        <br />
                                        <igtxt:WebImageButton ID="UpdateEmail" runat="server" Text="Update">
                                        </igtxt:WebImageButton>
                                    </Template>
                                </igmisc:WebPanel>
                            </ContentTemplate>
                        </igtab:Tab>
                        <igtab:Tab Text="General">
                            <ContentTemplate>
                                <asp:Label ID="Label1" runat="server" Text="SMS Timer"></asp:Label>
                                <br />
                                <igtxt:WebNumericEdit ID="SMSTimer" runat="server" MaxValue="900000" MinValue="1000">
                                    <SpinButtons Delta="1000" Display="OnRight"></SpinButtons>
                                </igtxt:WebNumericEdit>
                                <br />
                                <igtxt:WebImageButton ID="UpdateSMSTimer" runat="server" Text="Update"></igtxt:WebImageButton>
                                <br />
                                <asp:Label ID="Label2" runat="server" Text="Email Timer"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="EmailTimer" runat="server" MaxValue="900000" MinValue="1000">
                                    <SpinButtons Delta="1000" Display="OnRight"></SpinButtons>
                                </igtxt:WebNumericEdit>
                                <br />
                                <igtxt:WebImageButton ID="UpdateMMSTimer" runat="server" Text="Update">
                                </igtxt:WebImageButton>
                                <br />
                                <asp:Label ID="Label36" runat="server" Text="MMS Timer"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="MMSTimer" runat="server" MaxValue="900000" MinValue="1000">
                                    <SpinButtons Delta="1000" Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                &nbsp;<igtxt:WebImageButton ID="btnUpdateEmailTimer" runat="server" Text="Update" OnClick="btnUpdateEmailTimer_Click">
                                </igtxt:WebImageButton>
                                <br />
                                <asp:Label ID="Label37" runat="server" Text="DialUp Name"></asp:Label><br />
                                <igtxt:WebTextEdit ID="txtDialUp" runat="server" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                                <br />
                                &nbsp;<igtxt:WebImageButton ID="btnUpdateDialUp" runat="server" Text="Update" OnClick="btnUpdateDialUp_Click"></igtxt:WebImageButton>
                                <br />
                                <asp:CheckBox ID="chkServerPing" runat="server" Text="Server Ping Enabled" Width="152px" /><br />
                                <asp:Label ID="Label38" runat="server" Text="Send Ping Emails To" Width="144px"></asp:Label><br />
                                <igtxt:WebTextEdit ID="txtPingEmail" runat="server" Width="250px" Height="34px">
                                </igtxt:WebTextEdit>
                                <br />
                                <igtxt:WebImageButton ID="btnUpdatePingServer" runat="server" Text="Update" OnClick="btnUpdatePingServer_Click">
                                </igtxt:WebImageButton>
                                <br />
                                <asp:Label ID="Label49" runat="server" Text="Alert Include Start"></asp:Label>
                                <br />
                                <asp:CheckBox ID="chkIncludeStart" runat="server" /><br />
                                <igtxt:WebImageButton ID="btnIncludeStart" runat="server" Text="Update" OnClick="btnIncludeStart_Click">
                                </igtxt:WebImageButton>
                                <br />
                                <asp:Label ID="Label50" runat="server" Text="Reset Server(days)"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="txtResetServer" runat="server" Width="250px" Height="34px" MaxValue="365" MinValue="0" Nullable="False">
                                    <SpinButtons Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                <igtxt:WebImageButton ID="btnResetServerUpdate" runat="server" Text="Update" OnClick="btnResetServerUpdate_Click">
                                </igtxt:WebImageButton>
                            </ContentTemplate>
                        </igtab:Tab>

                        <igtab:Tab Text="Scheduled Report" Tooltip="Allows to setup the system scheduled report and test it.">

                            <ContentTemplate>
                                <asp:Button ID="btnTestReport" runat="server" Text="Test Settings" ToolTip="Save Settings before trying test!" OnClick="btnTestReport_Click"/>
                                <%--<igmisc:WebPanel ID="WebPanel5" runat="server" Expanded="False" Height="400px" Width="584px">
                            <Header Text="Text Status Report">
                            </Header>
                            <Template>
                                <asp:Label ID="Label31" runat="server" Text="Report Name" Width="112px"></asp:Label><br />
                                <asp:TextBox ID="txtReportName" runat="server" Width="264px"></asp:TextBox><br />
                                <asp:Label ID="Label32" runat="server" Text="Period of Report(Hours)"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="txtReportHours" runat="server" MaxValue="744" MinValue="1">
                                    <SpinButtons Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                <asp:Label ID="Label33" runat="server" Text="Days" Width="48px"></asp:Label><br />
                                <asp:CheckBoxList ID="chkDays" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Width="360px">
                                    <asp:ListItem Value="0">Monday</asp:ListItem>
                                    <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                    <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                    <asp:ListItem Value="3">Thursday</asp:ListItem>
                                    <asp:ListItem Value="4">Friday</asp:ListItem>
                                    <asp:ListItem Value="5">Saturday</asp:ListItem>
                                    <asp:ListItem Value="6">Sunday</asp:ListItem>
                                </asp:CheckBoxList><asp:Label ID="Label34" runat="server" Text="Recipients" Width="80px"></asp:Label><br />
                                <asp:TextBox ID="txtReportRecipients" runat="server" ToolTip="email adress seperate with,"
                                    Width="264px"></asp:TextBox><br />
                        <asp:Label ID="lblerr" runat="server" ForeColor="Red" Visible="False" Width="536px"></asp:Label><asp:Button ID="BtnSave" runat="server" Text="Save Settings" />
                            </Template>
                        </igmisc:WebPanel>
                        <igmisc:WebPanel ID="WebPanel6" runat="server" Height="400px" Width="584px" 
                            Expanded="False">
                            <Header Text="Graphical&amp;nbsp;Status Report">
                            </Header>
                            <Template>
                                <asp:Label ID="Label39" runat="server" Text="Report Name" Width="112px"></asp:Label><br />
                                <asp:TextBox ID="txtReport2Name" runat="server" Width="264px"></asp:TextBox><br />
                                <asp:Label ID="Label40" runat="server" Text="Period of Report(Hours)"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="txtReport2Hours" runat="server" MaxValue="744" MinValue="1">
                                    <SpinButtons Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                <asp:Label ID="Label41" runat="server" Text="Days" Width="48px"></asp:Label><br />
                                <asp:CheckBoxList ID="chk2Days" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Width="360px">
                                    <asp:ListItem Value="0">Monday</asp:ListItem>
                                    <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                    <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                    <asp:ListItem Value="3">Thursday</asp:ListItem>
                                    <asp:ListItem Value="4">Friday</asp:ListItem>
                                    <asp:ListItem Value="5">Saturday</asp:ListItem>
                                    <asp:ListItem Value="6">Sunday</asp:ListItem>
                                </asp:CheckBoxList><asp:Label ID="Label42" runat="server" Text="Recipients" Width="80px"></asp:Label><br />
                                <asp:TextBox ID="txtReport2Recipients" runat="server" ToolTip="email adress seperate with,"
                                    Width="264px"></asp:TextBox><br />
                                <asp:Button ID="BtnSaveRep2" runat="server" Text="Save Settings" />
                            </Template>
                        </igmisc:WebPanel><igmisc:WebPanel ID="WebPanel7" runat="server" Height="400px" Width="584px" Expanded="False">
                            <Header Text="Min Max Avg&amp;nbsp;Report">
                            </Header>
                            <Template>
                                <asp:Label ID="Label43" runat="server" Text="Report Name" Width="112px"></asp:Label><br />
                                <asp:TextBox ID="txtReport3Name" runat="server" Width="264px"></asp:TextBox><br />
                                <asp:Label ID="Label44" runat="server" Text="Period of Report(Hours)"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="txtReport3Hours" runat="server" MaxValue="744" MinValue="1">
                                    <SpinButtons Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                <asp:Label ID="Label45" runat="server" Text="Days" Width="48px"></asp:Label><br />
                                <asp:CheckBoxList ID="chk3Days" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Width="360px">
                                    <asp:ListItem Value="0">Monday</asp:ListItem>
                                    <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                    <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                    <asp:ListItem Value="3">Thursday</asp:ListItem>
                                    <asp:ListItem Value="4">Friday</asp:ListItem>
                                    <asp:ListItem Value="5">Saturday</asp:ListItem>
                                    <asp:ListItem Value="6">Sunday</asp:ListItem>
                                </asp:CheckBoxList><asp:Label ID="Label46" runat="server" Text="Recipients" Width="80px"></asp:Label><br />
                                <asp:TextBox ID="txtReport3Recipients" runat="server" ToolTip="email adress seperate with,"
                                    Width="264px"></asp:TextBox><br />
                                <asp:Label ID="Label47" runat="server" Text="Avg By" Width="80px"></asp:Label><br />
                                <asp:DropDownList ID="cmbrep3DailySetting" runat="server" Width="200px">
                                    <asp:ListItem Selected="True" Value="0">Daily 6-6</asp:ListItem>
                                    <asp:ListItem Value="1">Daily 12-12</asp:ListItem>
                                    <asp:ListItem Value="2">12 Hourly</asp:ListItem>
                                    <asp:ListItem Value="3">Hourly</asp:ListItem>
                                    <asp:ListItem Value="4">Weekly</asp:ListItem>
                                </asp:DropDownList><br />
                                <asp:Label ID="Label48" runat="server" Text="Sensor#:Field#|" Width="80px"></asp:Label><br />
                                <asp:TextBox ID="txtReport3SensorField" runat="server" ToolTip="email adress seperate with,"
                                    Width="264px"></asp:TextBox><br />
                                <asp:Button ID="btnSaveRep3" runat="server" Text="Save Settings" />
                                <asp:Button ID="TestRep3" runat="server" Text="Test Settings" ToolTip="Save Settings before trying test!" />
                            </Template>
                        </igmisc:WebPanel>--%>
                            </ContentTemplate>
                        </igtab:Tab>
                        <igtab:Tab Text="History" Tooltip="Allows to specify how many days history to store.">
                            <ContentTemplate>
                                <asp:Label ID="Label32a" runat="server" Text="Days Sensor Data to Keep"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="txtSensorDays" runat="server" Width="250px" Height="34px" MaxValue="744" MinValue="1" ValueText="365">
                                    <SpinButtons Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                <asp:Label ID="Label35a" runat="server" Text="Maximum Video Disk Size Mb" Width="216px"></asp:Label><br />
                                <igtxt:WebNumericEdit ID="txtVideoDays" runat="server" Width="250px" Height="34px" MaxValue="30720" MinValue="300" ValueText="1024">
                                    <SpinButtons Display="OnRight" />
                                </igtxt:WebNumericEdit>
                                <br />
                                <br />
                                <asp:Button ID="BtnSaveDays" runat="server" Text="Save Settings" OnClick="BtnSaveDays_Click"/>
                            </ContentTemplate>
                        </igtab:Tab>
                        <igtab:Tab Text="MMS">
                            <ContentTemplate>
                                <asp:Label ID="Label212" runat="server" Text="MMS URL"></asp:Label><br />
                                <asp:TextBox ID="txtMMSURL" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                <asp:Label ID="Label217" runat="server" Text="API ID"></asp:Label><br />
                                <asp:TextBox ID="txtMMSSMTPAPIId" runat="server" Width="250px" Height="34px"></asp:TextBox><br />

                                <asp:Label ID="Label218" runat="server" Text="User"></asp:Label><br />
                                <asp:TextBox ID="txtMMSSMTPUser" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                <asp:Label ID="Label219" runat="server" Text="Password"></asp:Label><br />
                                <asp:TextBox ID="txtMMSSMTPPassword" runat="server" Width="250px" Height="34px"></asp:TextBox><br />

                                <asp:Label ID="Label221" runat="server" Text="FTP Server"></asp:Label><br />
                                <asp:TextBox ID="txtMMSFTPServer" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                <asp:Label ID="Label222" runat="server" Text="User"></asp:Label><br />
                                <asp:TextBox ID="txtMMSFTPUser" runat="server" Width="250px" Height="34px"></asp:TextBox><br />

                                <asp:Label ID="Label223" runat="server" Text="Password"></asp:Label><br />
                                <asp:TextBox ID="txtMMSFTPPassword" runat="server" Width="250px" Height="34px"></asp:TextBox><br />
                                <asp:Label ID="Label224" runat="server" Text="Remote Path"></asp:Label><br />
                                <asp:TextBox ID="txtMMSFTPRemotePath" runat="server" Width="250px" Height="34px"></asp:TextBox><br />

                                <asp:Label ID="Label237" runat="server" Text="MMS ImageURL"></asp:Label><br />
                                <asp:TextBox ID="txtMMSImageURL" runat="server" Width="250px" Height="34px"></asp:TextBox>&nbsp;
                        <br />
                                <igtxt:WebImageButton ID="btnUpdateMMSEmail" runat="server" Text="Update" OnClick="btnUpdateMMSEmail_Click">
                                </igtxt:WebImageButton>
                                <igtxt:WebImageButton ID="btnDeleteMMSEmail" runat="server" Text="Delete">
                                </igtxt:WebImageButton>
                            </ContentTemplate>
                        </igtab:Tab>
                    </Tabs>
                    <DefaultTabStyle BackColor="#E1EDFF" Height="20px">
                    </DefaultTabStyle>
                    <RoundedImage FillStyle="LeftMergedWithCenter"
                        NormalImage="ig_tab_blueb1.gif" SelectedImage="ig_tab_blueb2.gif" />
                </igtab:UltraWebTab>
                &nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </div>
    </div>

</asp:Content>
