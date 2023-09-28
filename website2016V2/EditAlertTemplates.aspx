<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAlertTemplates.aspx.cs" Inherits="website2016V2.EditAlertTemplates" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap.min.js"></script>
    <script src="DataTable/dataTables.buttons.min.js"></script>
    <script src="DataTable/jszip.min.js"></script>
    <script src="DataTable/pdfmake.min.js"></script>
    <script src="DataTable/vfs_fonts.js"></script>
    <script src="DataTable/buttons.html5.min.js"></script>

    <link href="DataTable/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="DataTable/buttons.dataTables.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            $('.gvdatatable').dataTable({
                dom: 'Bfrtip',
                buttons: [
            'excelHtml5',
            'pdfHtml5'
                ],

                "order": [[2, "desc"]],
                buttons: [
                     {
                         extend: 'pdf',
                         text: 'PDF',
                         exportOptions: {
                             columns: [2, 3, 4, 5],
                         }
                     },
                      {
                          extend: 'excel',
                          text: 'Excel',
                          exportOptions: {
                              columns: [2, 3, 4, 5],
                          }
                      }

                ],
                columnDefs: [
          {
              "targets": [0],
              //"visible": false,
              "orderable": false,
              "searchable": false

          },
          {
              "targets": [1],
              "orderable": false,
              "searchable": false
          }]

            });
        });
    </script>



    <h3>Edit Alert Template</h3>
    <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <asp:Label ID="lblSuccess" runat="server"></asp:Label>
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
                        <asp:Label ID="lblAdd" runat="server" Text="Edit"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblTemplate" runat="server" Text="Template">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        
                    </div>
                    <div class="col-md-2"></div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:DropDownList ID="cmbTemplatesMain" OnSelectedIndexChanged="cmbTemplatesMain_SelectedIndexChanged" ToolTip="Select template to edit" runat="server" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        
                    </div>
                    <div class="col-md-2"></div>
                </div><br /><hr />
                <div class="row">
                    <asp:Wizard ID="Wizard1" runat="server" OnActiveStepChanged="Wizard1_ActiveStepChanged" OnNextButtonClick="Wizard1_NextButtonClick" OnFinishButtonClick="Wizard1_FinishButtonClick" OnPreviousButtonClick="Wizard1_PreviousButtonClick" ActiveStepIndex="0" Height="468px" Width="100%">
                        <WizardSteps>
                            <asp:WizardStep runat="server" Title="Create Message" ID="Wiz" StepType="Start">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label32" runat="server" Text="Template Name:"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtTemplateName" runat="server" required="true" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="AlertMessage" runat="server" Height="127px" TextMode="MultiLine" ToolTip="This is the message that will be sent out on the alert triggering .Use the Scripts buttons bellow to add aditional values to the message." CssClass="form-control"></asp:TextBox>
                                    </div>            
                                </div><br />
                                <div class="row">
                                    <div class="form-group">
                                        <div class="btn-group" style="margin-left:30px">
                                            <asp:Label ID="lblMyID" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Button ID="btnDevice" CssClass="btn btn-primary" runat="server" Text="Device" ToolTip="Device/s that caused the alert trigger" OnClick="btnDevice_Click" />
                                            <asp:Button ID="btnFields" CssClass="btn btn-primary" runat="server" Text="Fields" ToolTip="Fields that caused the trigger" OnClick="btnFields_Click" />
                                            <asp:Button ID="btnName" CssClass="btn btn-primary" runat="server" Text="Name" ToolTip="Contact name" OnClick="btnName_Click" />
                                            <asp:Button ID="btnValues" CssClass="btn btn-primary" runat="server" Text="Values" ToolTip="Values of the fields" OnClick="btnValues_Click" />
                                            <asp:Button ID="btnAlertStart" CssClass="btn btn-primary" runat="server" Text="AlertStart" ToolTip="Alert start date" OnClick="btnAlertStart_Click" />
                                            <asp:Button ID="btnAlertMins" CssClass="btn btn-primary" runat="server" Text="AlertMins" ToolTip="Alert running for x mins" OnClick="btnAlertMins_Click" />
                                            <asp:Button ID="btnCrLf" CssClass="btn btn-primary" runat="server" Text="CRLF" OnClick="btnCrLf_Click" ToolTip="Alert running for x mins" />
                                            <asp:Button ID="btnRTNSE" CssClass="btn btn-primary" runat="server" Text="Return Message" OnClick="btnRTNSE_Click" ToolTip="Return to Normal custom message" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblValitesMessage" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Choose Message Type" ID="Wiz0">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:CheckBoxList ID="AlertType" Width="488px" runat="server" RepeatColumns="3" ToolTip="Choose the type of alert message pathway to use.Remember that some may have limits ie number of characters in a SMS is 160."></asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Configure Options" ID="Wiz1">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label1" runat="server" Text="Include Images"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:RadioButtonList ID="AlertIncludeImage" runat="server" RepeatColumns="2" Width="176px" ToolTip="Should the message include images .Only available for rich types ie Emails and MMS types.">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:CheckBoxList ID="AlertCameraImages" runat="server" RepeatColumns="2" ToolTip="Which camera to link images from.">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label2" runat="server" Text="Image Delayed Send"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">  
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtDelay1" runat="server" TextMode="Number" min="0" max="15" step="1" Text="0" ToolTip="The delay before capturing the image from the camera feed." required="true" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtDelay2" runat="server" TextMode="Number" min="0" max="15" step="1" Text="0" ToolTip="The delay before capturing the image from the camera feed." required="true" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label3" runat="server" Text="Enabled"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:RadioButtonList ID="AlertEnabled" runat="server" RepeatColumns="2" Width="176px" ToolTip="Is the Alert enabled or disabled.If disabled it will not be sent or tested.">
                                            <asp:ListItem Selected="True">True</asp:ListItem>
                                            <asp:ListItem>False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label4" runat="server" Text="Send Return to Normal Alert"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:RadioButtonList ID="AlertSendRTN" runat="server" RepeatColumns="2" Width="176px" ToolTip="Sets if an extra message be sent when the status returns to the correct state.">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div> 
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Choose Contacts" ID="Wiz2">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="cmbContacts" runat="server" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px" ToolTip="Select an exsisting contact to link to this alert message.">
                                        </asp:DropDownList>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Button ID="cmdLinkContacts" OnClick="cmdLinkContacts_Click" ToolTip="Link the Selected contact to the alert." runat="server" Text="Link Contact" Height="40px" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF"/>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridContacts" CssClass="gvdatatable table table-striped table-bordered" Width="90%" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Remove" CommandName="EditItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                       
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="25px" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
                                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Surname"/>
                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    <asp:BoundField DataField="Cell" HeaderText="Cell" />
                                                    <asp:BoundField DataField="Pager" HeaderText="Pager" />
                                                    <asp:BoundField DataField="Other" HeaderText="Other" />
                                                    <asp:BoundField DataField="Outputs" HeaderText="Outputs" />
                                                    <asp:BoundField DataField="ResendDelay" HeaderText="ResendDelay" />
                                                    <asp:BoundField DataField="LinkID" HeaderText="LinkID" />

                                                </Columns>
                                            </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblValidatesContacts" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Add Thresholds" ID="Wiz3">
                                 <div class="row" style="margin-top:20px">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label5" runat="server" Text="Thresh Hold Name"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="The threashold name for refrence" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                         <div id="curVals" runat="server" style="width: 693px">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label6" runat="server" Text="Sensor Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="cmbSensorID" OnSelectedIndexChanged="cmbSensorID_SelectedIndexChanged" runat="server" ToolTip="The sensor that will cause the alert to trigger" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="cmbDeviceID" runat="server" AutoPostBack="true" Visible="false" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label7" runat="server" Text="Field"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="cmbField" OnSelectedIndexChanged="cmbField_SelectedIndexChanged" runat="server" ToolTip="The field of the selected sensor to test." AutoPostBack="true" Class="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="cmbFieldComp" ToolTip="The field of the sensor to compare to." runat="server" AutoPostBack="true" Visible="false" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label8" runat="server" Text="Tabular Row"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtTabularCnt0" runat="server" ToolTip="The row of tabular values to check negative checks all rows of this field" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label9" runat="server" Text="Test Type"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:RadioButtonList ID="TestType" OnSelectedIndexChanged="TestType_SelectedIndexChanged" runat="server" AutoPostBack="True" RepeatColumns="4" Width="100%" ToolTip="The type of test to perform on the sensor field value/other value .">
                                        </asp:RadioButtonList>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label10" runat="server" Text="Check Value"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCheckValue" runat="server" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label11" runat="server" Text="Tab Count Value">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtTabularCnt" runat="server" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label12" runat="server" Text="Hold period before triggering"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtHoldPeriod" runat="server" MaxValue="999" MinValue="0" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label13" runat="server" Text="Comparison to other thresh holds">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RadioButtonList ID="Comparison" runat="server" RepeatColumns="2" Width="384px" ToolTip="How to join multiple threashold tst together .Should all be true before alerting or any true before alerting.">
                                            <asp:ListItem Selected="True" Value="0">And</asp:ListItem>
                                            <asp:ListItem Value="1">Or</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label14" runat="server" Text="Order of comparison"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtOrder" runat="server" MaxValue="9999" MinValue="0" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblExtra" runat="server" Text="Extra String">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtExtra" runat="server" ToolTip="The value to test in other value f ield." CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblExtra1" runat="server" Text="Extra1 String"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtExtra1" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblExtra2" runat="server" Text="Must Occure (Hours)">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtExtra2" runat="server" ToolTip="This threashold must occure on a period basis" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblExtra3" runat="server" Text="Extra3"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="TxtExtra3" runat="server" ToolTip="Extra information field" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        
                                    </div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="chkSensAlertTemplate" runat="server" Text="Alert Template" ToolTip="Create an alert template that wil be applied to all sensors of the same type ." />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" Width="476px"></asp:Label>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="cmdSend" OnClick="cmdSend_Click_SaveThreashold" runat="server" Text="Add Threshhold" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF"/>
                                    </div>
                                    <div class="col-md-2">
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Label ID="lblThreadSuccess" runat="server" Text=""></asp:Label>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label15" runat="server" Text="Threashholds"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridThreashholds" CssClass="gvdatatable table table-striped table-bordered" Width="95%" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands1">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblDelete" runat="server" Text="Remove" CommandName="EditItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                       
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="25px" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
                                                    <asp:BoundField DataField="SensorID" HeaderText="SensorID" SortExpression="Surname"/>
                                                    <asp:BoundField DataField="DeviceID" HeaderText="DeviceID" />
                                                    <asp:BoundField DataField="Field" HeaderText="Field" />
                                                    <asp:BoundField DataField="TestType" HeaderText="TestType" />
                                                    <asp:BoundField DataField="CheckValue" HeaderText="CheckValue" />
                                                    <asp:BoundField DataField="HoldPeriod" HeaderText="HoldPeriod" />
                                                    <asp:BoundField DataField="AlertTemplateID" HeaderText="AlertTemplateID" />
                                                    <asp:BoundField DataField="Comparison" HeaderText="Comparison" />
                                                    <asp:BoundField DataField="Order" HeaderText="Order" Visible="false"/>
                                                    <asp:BoundField DataField="Extra" HeaderText="Extra" Visible="false"/>
                                                    <asp:BoundField DataField="Extra1" HeaderText="Extra1" Visible="false"/>
                                                    <asp:BoundField DataField="Extra2" HeaderText="Extra2" Visible="false"/>
                                                    <asp:BoundField DataField="Extra3" HeaderText="Extra3" Visible="false"/>
                                                    <asp:BoundField DataField="TabularRowCnt" HeaderText="TabularRowCnt" />
                                                    <asp:BoundField DataField="Field1" HeaderText="Field1"/>
                                                    <asp:BoundField DataField="Field2" HeaderText="Field2"/>
                                                    <asp:BoundField DataField="GroupID" HeaderText="GroupID" />
                                                    <asp:BoundField DataField="LocatonID" HeaderText="LocationID" />
                                                    <asp:BoundField DataField="IsTemplate" HeaderText="IsTemplate" />
                                                </Columns>
                                            </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblValidatesThreshholds" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>

                            </asp:WizardStep>
                            <asp:WizardStep runat="server" StepType="Finish" Title="Verify Details" ID="Wiz4">
                                <div class="row" style="margin-top:35px">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label16" Font-Bold="true" runat="server" Text="Alert Message"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label17" Font-Bold="true" runat="server" Text="Message Type"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <hr />
                                        <asp:CheckBoxList ID="chkMessageType" runat="server" RepeatColumns="3" Width="488px" Enabled="false">
                                        </asp:CheckBoxList>
                                        <hr />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label18" Font-Bold="true" runat="server" Text="Configure Options"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label19" Font-Bold="true" runat="server" Text="Include Images"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:RadioButtonList ID="AlertIncludeImageVerify" runat="server" RepeatColumns="2" Width="176px" Enabled="False">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:CheckBoxList ID="AlertCameraImagesVerify" runat="server" Height="64px" RepeatColumns="2" Width="496px" Enabled="False">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label20" Font-Bold="true" runat="server" Text="Image Delayed Send"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label21" Font-Bold="true" runat="server" Text="Delay 1"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDelay1Verify" runat="server" MaxValue="15" MinValue="0" ValueText="0" Enabled="False" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label22" Font-Bold="true" runat="server" Text="Delay 2">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDelay2Verify" runat="server" MaxValue="15" MinValue="0" ValueText="0" Enabled="False" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label23" Font-Bold="true" runat="server" Text="Include Sensor Values"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label24" Font-Bold="true" runat="server" Text="Sensor1:"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblSensor1" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label25" Font-Bold="true" runat="server" Text="Field:">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblField1" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label26" Font-Bold="true" runat="server" Text="Sensor2:"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblSensor2" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label27" Font-Bold="true" runat="server" Text="Field:">  </asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblField2" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label28" Font-Bold="true" runat="server" Text="Enabled"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:RadioButtonList ID="AlertEnabledVerify" runat="server" RepeatColumns="2" Width="176px" Enabled="False">
                                            <asp:ListItem Selected="True">True</asp:ListItem>
                                            <asp:ListItem>False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label29" Font-Bold="true" runat="server" Text="Send Return to Normal Alert"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:RadioButtonList ID="AlertSendRTNVerify" runat="server" RepeatColumns="2" Width="176px" Enabled="False">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label30" Font-Bold="true" runat="server" Text="Contacts"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridContactsVerify" CssClass="gvdatatable table table-striped table-bordered" Width="95%" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
                                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Surname"/>
                                                    <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    <asp:BoundField DataField="Cell" HeaderText="Cell" />
                                                    <asp:BoundField DataField="Pager" HeaderText="Pager" />
                                                    <asp:BoundField DataField="Other" HeaderText="Other" />
                                                    <asp:BoundField DataField="Outputs" HeaderText="Outputs" />
                                                    <asp:BoundField DataField="ResendDelay" HeaderText="ResendDelay" />
                                                    <asp:BoundField DataField="LinkID" HeaderText="LinkID" />

                                                </Columns>
                                            </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label31" Font-Bold="true" runat="server" Text="Threshholds"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView ID="GridThreashholdsVerify" CssClass="gvdatatable table table-striped table-bordered" Width="95%" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
                                                    <asp:BoundField DataField="SensorID" HeaderText="SensorID" SortExpression="Surname"/>
                                                    <asp:BoundField DataField="DeviceID" HeaderText="DeviceID" />
                                                    <asp:BoundField DataField="Field" HeaderText="Field" />
                                                    <asp:BoundField DataField="TestType" HeaderText="TestType" />
                                                    <asp:BoundField DataField="CheckValue" HeaderText="CheckValue" />
                                                    <asp:BoundField DataField="HoldPeriod" HeaderText="HoldPeriod" />
                                                    <asp:BoundField DataField="AlertTemplateID" HeaderText="AlertTemplateID" />
                                                    <asp:BoundField DataField="Comparison" HeaderText="Comparison" />
                                                    <asp:BoundField DataField="Order" HeaderText="Order" Visible="false"/>
                                                    <asp:BoundField DataField="Extra" HeaderText="Extra" />
                                                    <asp:BoundField DataField="Extra1" HeaderText="Extra1"/>
                                                    <asp:BoundField DataField="Extra2" HeaderText="Extra2" Visible="false"/>
                                                    <asp:BoundField DataField="Extra3" HeaderText="Extra3" Visible="false"/>
                                                    <asp:BoundField DataField="TabularRowCnt" HeaderText="TabularRowCnt" />
                                                    <asp:BoundField DataField="Field1" HeaderText="Field1" Visible="false"/>
                                                    <asp:BoundField DataField="Field2" HeaderText="Field2" Visible="false"/>
                                                    <asp:BoundField DataField="GroupID" HeaderText="GroupID" />
                                                    <asp:BoundField DataField="LocatonID" HeaderText="LocationID" />
                                                    <asp:BoundField DataField="IsTemplate" HeaderText="IsTemplate" />
                                                </Columns>
                                            </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label33" runat="server" Text="Saving Success Status"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblMessageSucess" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblContactLinkSuccess" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label1as" runat="server" Text="">Verify details and click finish to save .</asp:Label>
                                        <asp:Label ID="lblThreshholdSuccess" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                
                            </asp:WizardStep>

                        </WizardSteps>
                    </asp:Wizard>
                </div>
                </div>
            </div>
        </div>
</asp:Content>
