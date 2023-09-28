<%@ Page Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="AddAlert.aspx.cs" Inherits="website2016V2.AddAlert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">Add Alert Form</h3>

    <%--<div class="title_right">
                                    <div class="col-sm-5 col-sm-5 col-xs-12 form-group pull-right top_search">
                                        <div class="input-group">
                                        </div>
                                    </div>
                                </div>--%>

     <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>

</div>
       
    <br />
<div class="card-body">
<div class="row">
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
                        <!-- form input mask -->
                        <div class="col-sm-4 col-sm-12 col-xs-12">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h3>Alert Type</h3>
                                    
                                    <div class="clearfix"></div>
                                </div>
                                <div class="x_content">
                                    <br />
                                        <div style="margin-left:10px">
                                            <asp:CheckBoxList ID="AlertType" runat="server" RepeatColumns="2" Width="488px">
                                            </asp:CheckBoxList>
                                        </div>
                                       
                                        <div class="ln_solid"></div>

                                        <div class="form-group">
                                            <div class="col-sm-9 col-sm-offset-3">
                                            </div>
                                      </div>
                                </div>
                              </div>
                            </div>
                            <!-- form color picker -->
                        <div class="col-sm-8 col-sm-12 col-xs-12">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h2>Message</h2>
                                    <ul class="nav navbar-right panel_toolbox">
                                        <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                        </li>
                                    </ul>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="x_content">
                                    <br />
                                    <div style="width:100%">
                                        <asp:TextBox ID="AlertMessage" runat="server" Height="127px" Width="100%"></asp:TextBox>
                                    </div><br />
                                    <div class="ln_solid"></div>
                                    <div class="form-group">
                                            <div class="btn-group" style="margin-left:25px">
                                                 <asp:Button ID="btnDevice" CssClass="btn btn-gray" runat="server" Text="Device" ToolTip="Device/s that caused the alert trigger" OnClick="btnDevice_Click" />
                                                 <asp:Button ID="btnFields" CssClass="btn btn-gray" runat="server" Text="Fields" ToolTip="Fields that caused the trigger" OnClick="btnFields_Click" />
                                                 <asp:Button ID="btnName" CssClass="btn btn-gray" runat="server" Text="Name" ToolTip="Contact name" OnClick="btnName_Click" />
                                                 <asp:Button ID="btnValues" CssClass="btn btn-gray" runat="server" Text="Values" ToolTip="Values of the fields" OnClick="btnValues_Click" />
                                                 <asp:Button ID="btnAlertStart" CssClass="btn btn-gray" runat="server" Text="AlertStart" ToolTip="Alert start date" OnClick="btnAlertStart_Click" />
                                                 <asp:Button ID="btnAlertMins" CssClass="btn btn-gray" runat="server" Text="AlertMins" ToolTip="Alert running for x mins" OnClick="btnAlertMins_Click" />
                                                 <asp:Button ID="btnCrLf" CssClass="btn btn-gray" runat="server" Text="CRLF" OnClick="btnCrLf_Click" ToolTip="Alert running for x mins" />
                                                 <asp:Button ID="btnRTNSE" CssClass="btn btn-gray" runat="server" Text="Return Message" OnClick="btnRTNSE_Click" ToolTip="Return to Normal custom message" />
                                            </div>
                                     </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-sm-12">
                            <div class="x_panel">
                                <div class="x_title">
                                    <h2>Add Alert Fields</h2>
                                    <ul class="nav navbar-right panel_toolbox">
                                        <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                        </li>
                                    </ul>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="x_content">
                                    <div class="col-sm-10">
                                        <p class="pSetting">Include Images</p>
                                        <asp:RadioButtonList ID="AlertIncludeImage" runat="server" RepeatColumns="2" Width="176px">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:CheckBoxList ID="AlertCameraImages" runat="server" Height="64px" RepeatColumns="2" Width="496px">
                                        </asp:CheckBoxList><br /><br />

                                        <p class="pSetting">Image Delayed Send</p>

                                        <div class="form-group">
                                            <div class="col-sm-6 col-sm-6 col-xs-6">
                                                <label class="control-label" for="first-name">Delay 1 <span class="required">*</span>
                                                </label>
                                                <asp:TextBox ID="txtDelay1" runat="server" CssClass="form-control" TextMode="Number" Text="0"></asp:TextBox>
                                            </div><br />
                                        </div><br /><br />

                                        <div class="form-group">
                                            <div class="col-sm-6 col-sm-6 col-xs-6">
                                                <label class="control-label" for="first-name">Delay 2 <span class="required">*</span>
                                                </label>
                                                <asp:TextBox ID="txtDelay2" runat="server" CssClass="form-control" TextMode="Number" Text="0"></asp:TextBox>
                                            </div><br />
                                        </div><br /><br />

                                        <p class="pSetting">Include Sensor Values</p>
                                         <div class="form-group">
                                           <div class="col-sm-3 col-sm-6 col-xs-6">
                                                <label class="control-label" for="first-name">Sensor 1 <span class="required">*</span>
                                                </label>
                                                <asp:DropDownList ID="cmbSensor1ID" runat="server" CssClass="form-control" AutoPostBack="true" Width="178px" OnSelectedIndexChanged="cmbSensor1ID_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                             <div class="col-sm-3 col-sm-6 col-xs-6">
                                                <label class="control-label" style="margin-left:0px" for="first-name">Field <span class="required">*</span>
                                                </label>
                                                <asp:DropDownList ID="cmbField1" runat="server" CssClass="form-control leaveSpace" Width="178px"></asp:DropDownList>
                                             </div><br />
                                        </div><br /><br /><br />

                                        <div class="form-group">
                                            <div class="col-sm-3 col-sm-6 col-xs-6">
                                                <label class="control-label" for="first-name">Sensor 2 <span class="required">*</span>
                                                </label>
                                                <asp:DropDownList ID="cmbSensor2ID" runat="server" CssClass="form-control" AutoPostBack="True" Width="178px"  OnSelectedIndexChanged="cmbSensor2ID_SelectedIndexChanged"></asp:DropDownList>
                                            </div>                                          
                                            <div class="col-sm-3 col-sm-6 col-xs-6">
                                                <label class="control-label" style="margin-left:0px" for="first-name">Field <span class="required">*</span>
                                                </label>
                                                <asp:DropDownList ID="cmbField2" runat="server" CssClass="form-control leaveSpace" Width="178px"></asp:DropDownList>
                                            </div><br />
                                        </div><br /><br />

                                        <p class="pSetting">Enabled</p>
                                         <asp:RadioButtonList ID="AlertEnabled" runat="server" RepeatColumns="2" Width="176px">
                                            <asp:ListItem Selected="True">True</asp:ListItem>
                                            <asp:ListItem>False</asp:ListItem>
                                        </asp:RadioButtonList><br /><br />

                                        <p class="pSetting">Send Return to Normal Alert</p>
                                         <asp:RadioButtonList ID="AlertSendRTN" runat="server" RepeatColumns="2" Width="176px">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList><br /><br />
                                        <div class="ln_solid"></div>

                                         <div class="form-group">
                                            <div class="col-sm-9 col-sm-offset-4">
                                                <asp:Button ID="SubmitAlert" runat="server" CssClass="btn btn-gray" Text="Save & Link Contacts" Width="160px" OnClick="SubmitAlert_Click"/>
                                            </div>
                                        </div>

                                    </div><br />
                                    
                                </div>
                            </div>
                        </div>

                        </div>
</div>
</div>
           
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

