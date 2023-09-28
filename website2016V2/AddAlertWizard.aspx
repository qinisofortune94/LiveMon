<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddAlertWizard.aspx.cs" Inherits="website2016V2.AddAlertWizard" %>
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
<div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">Add Alert Wizard</h3>
</div>
   
<div class="card-body">
<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Add</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
        <div class="row">
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
                    <asp:Wizard ID="Wizard1" runat="server" OnNextButtonClick="Wizard1_NextButtonClick" OnFinishButtonClick="Wizard1_FinishButtonClick" OnPreviousButtonClick="Wizard1_PreviousButtonClick" OnActiveStepChanged="Wizard1_ActiveStepChanged" ActiveStepIndex="0" Height="468px" Width="100%">
                        <WizardSteps>
                            <asp:WizardStep runat="server" Title="Create Message" ID="Wiz" StepType="Start">
                                
                                <div class="row">
                                    <div class="col-sm-6">
                                      <div class="form-group">
                                        <asp:TextBox ID="AlertMessage" runat="server" Height="127px" TextMode="MultiLine" ToolTip="This is the message that will be sent out on the alert triggering .Use the Scripts buttons bellow to add aditional values to the message." CssClass="form-control"></asp:TextBox>
                                    </div>            
                                </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="btn-group" style="margin-left:20px">
                                            <asp:Label ID="lblMyID" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Button ID="btnDevice" CssClass="bg-gray" runat="server" Text="Device" ToolTip="Device/s that caused the alert trigger" OnClick="btnDevice_Click" />
                                            <asp:Button ID="btnFields" CssClass="bg-gray" runat="server" Text="Fields" ToolTip="Fields that caused the trigger" OnClick="btnFields_Click" />
                                            <asp:Button ID="btnName" CssClass="bg-gray" runat="server" Text="Name" ToolTip="Contact name" OnClick="btnName_Click" />
                                            <asp:Button ID="btnValues" CssClass="bg-gray" runat="server" Text="Values" ToolTip="Values of the fields" OnClick="btnValues_Click" />
                                            <asp:Button ID="btnAlertStart" CssClass="bg-gray" runat="server" Text="AlertStart" ToolTip="Alert start date" OnClick="btnAlertStart_Click" />
                                            <asp:Button ID="btnAlertMins" CssClass="bg-gray" runat="server" Text="AlertMins" ToolTip="Alert running for x mins" OnClick="btnAlertMins_Click" />
                                            <asp:Button ID="btnCrLf" CssClass="bg-gray" runat="server" Text="CRLF" OnClick="btnCrLf_Click" ToolTip="Alert running for x mins" />
                                            <asp:Button ID="btnRTNSE" CssClass="bg-gray" runat="server" Text="Return Message" OnClick="btnRTNSE_Click" ToolTip="Return to Normal custom message" />
                                        </div>
                                    </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblValitesMessage" runat="server" Text="" CssClass="form-control"></asp:Label>
                                          </div>
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Choose Message Type" ID="Wiz0">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:CheckBoxList ID="AlertType" Width="488px" runat="server" RepeatColumns="3" ToolTip="Choose the type of alert message pathway to use.Remember that some may have limits ie number of characters in a SMS is 160." CssClass="form-control"></asp:CheckBoxList>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblAlert" runat="server" Text="" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Configure Options" ID="Wiz1">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label ID="Label1" runat="server" Text="Include Images" CssClass="form-control"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="AlertIncludeImage" runat="server" RepeatColumns="2" Width="176px" ToolTip="Should the message include images .Only available for rich types ie Emails and MMS types." CssClass="form-control">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                            </div>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:CheckBoxList ID="AlertCameraImages" runat="server" RepeatColumns="2" ToolTip="Which camera to link images from." CssClass="form-control">
                                        </asp:CheckBoxList>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label ID="Label2" runat="server" Text="Image Delayed Send" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">  
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtDelay1" runat="server" TextMode="Number" min="0" max="15" step="1" Text="0" ToolTip="The delay before capturing the image from the camera feed." required="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtDelay2" runat="server" TextMode="Number" min="0" max="15" step="1" Text="0" ToolTip="The delay before capturing the image from the camera feed." required="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label ID="Label32" runat="server" Text="Include Sensor Values" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label44" runat="server" Text="Sensor 1" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>  
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbSensor1ID" CssClass="form-control" OnSelectedIndexChanged="cmbSensor1ID_SelectedIndexChanged" runat="server" AutoPostBack="True" Width="144px" ToolTip="Add data from a different sensor to the message stream.">
                                        </asp:DropDownList>
                                            </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label45" runat="server" Text="Field" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbField1" CssClass="form-control" runat="server" Width="160px" ToolTip="Add data from a different sensor to the message stream.">
                                        </asp:DropDownList>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label46" runat="server" Text="Sensor 2" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>  
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbSensor2ID" CssClass="form-control" OnSelectedIndexChanged="cmbSensor2ID_SelectedIndexChanged" runat="server" AutoPostBack="True" Width="144px" ToolTip="Add data from a different sensor to the message stream.">
                                        </asp:DropDownList>
                                            </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label47" runat="server" Text="Field" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbField2" CssClass="form-control" runat="server" Width="160px" ToolTip="Add data from a different sensor to the message stream.">
                                        </asp:DropDownList>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label3" runat="server" Text="Enabled" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="AlertEnabled" runat="server" RepeatColumns="2" Width="176px" ToolTip="Is the Alert enabled or disabled.If disabled it will not be sent or tested." CssClass="form-control">
                                            <asp:ListItem Selected="True">True</asp:ListItem>
                                            <asp:ListItem>False</asp:ListItem>
                                        </asp:RadioButtonList>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label ID="Label4" runat="server" Text="Send Return to Normal Alert" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="AlertSendRTN" runat="server" RepeatColumns="2" Width="176px" ToolTip="Sets if an extra message be sent when the status returns to the correct state." CssClass="form-control">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                            </div>
                                    </div>
                                </div> 
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Choose Contacts" ID="Wiz2">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbContacts" runat="server" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px" ToolTip="Select an exsisting contact to link to this alert message.">
                                        </asp:DropDownList>
                                            </div>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Button ID="cmdLinkContacts" OnClick="cmdLinkContacts_Click" ToolTip="Link the Selected contact to the alert." runat="server" Text="Link Contact" Height="40px" Width="250px" class="bg-gray form-control" BorderColor="#0099FF"/>
                                            </div>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:GridView ID="GridContacts" CssClass="gvdatatable table table-striped table-bordered" Width="90%" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="contactRemove">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Remove" CommandName="Remove" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                       
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
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label ID="lblValidatesContacts" runat="server" Text="" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                            </asp:WizardStep>
                            <asp:WizardStep runat="server" Title="Add Thresholds" ID="Wiz3">
                                 <div class="row" style="margin-top:20px">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label5" runat="server" Text="Thresh Hold Name" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="The threashold name for refrence" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                         <div id="curVals" runat="server" style="width: 693px">
                                        </div>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label6" runat="server" Text="Sensor ID" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbSensorID" OnSelectedIndexChanged="cmbSensorID_SelectedIndexChanged" runat="server" ToolTip="The sensor that will cause the alert to trigger" AutoPostBack="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                            </div>
                                    </div>
                                    <div class="col-sm-2">
                                        
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbDeviceID" runat="server" AutoPostBack="true" Visible="false" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label7" runat="server" Text="Field" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbField" OnSelectedIndexChanged="cmbField_SelectedIndexChanged" runat="server" ToolTip="The field of the selected sensor to test." AutoPostBack="true" Class="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:DropDownList ID="cmbFieldComp" ToolTip="The field of the sensor to compare to." runat="server" AutoPostBack="true" Visible="false" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label8" runat="server" Text="Tabular Row" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtTabularCnt0" runat="server" ToolTip="The row of tabular values to check negative checks all rows of this field" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label9" runat="server" Text="Test Type" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-10">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="TestType" OnSelectedIndexChanged="TestType_SelectedIndexChanged" runat="server" AutoPostBack="True" RepeatColumns="4" Width="100%" ToolTip="The type of test to perform on the sensor field value/other value ." CssClass="form-control">
                                        </asp:RadioButtonList>
                                            </div>
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label10" runat="server" Text="Check Value" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtCheckValue" runat="server" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label11" runat="server" Text="Tab Count Value" CssClass="form-control">  </asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtTabularCnt" runat="server" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label12" runat="server" Text="Hold period before triggering" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtHoldPeriod" runat="server" MaxValue="999" MinValue="0" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label13" runat="server" Text="Comparison to other thresh holds" CssClass="form-control">  </asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="Comparison" runat="server" RepeatColumns="2" Width="384px" ToolTip="How to join multiple threashold tst together .Should all be true before alerting or any true before alerting." CssClass="form-control">
                                            <asp:ListItem Selected="True" Value="0">And</asp:ListItem>
                                            <asp:ListItem Value="1">Or</asp:ListItem>
                                        </asp:RadioButtonList>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label14" runat="server" Text="Order of comparison" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtOrder" runat="server" MaxValue="9999" MinValue="0" Text="0" TextMode="Number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblExtra" runat="server" Text="Extra String" CssClass="form-control">  </asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="TxtExtra" runat="server" ToolTip="The value to test in other value f ield." CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblExtra1" runat="server" Text="Extra1 String" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="TxtExtra1" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblExtra2" runat="server" Text="Must Occure (Hours)" CssClass="form-control">  </asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="TxtExtra2" runat="server" ToolTip="This threashold must occure on a period basis" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblExtra3" runat="server" Text="Extra3" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="TxtExtra3" runat="server" ToolTip="Extra information field" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:CheckBox ID="chkSensAlertTemplate" runat="server" Text="Alert Template" ToolTip="Create an alert template that wil be applied to all sensors of the same type ."  CssClass="form-control"/>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblErr" runat="server" ForeColor="Red" Visible="False" Width="476px" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Button ID="cmdSend" OnClick="cmdSend_Click_SaveThreashold" runat="server" Text="Add Threshhold" Height="40px" Width="250px" class="bg-gray form-control" BorderColor="#0099FF"/>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label ID="Label15" runat="server" Text="" CssClass="form-control"></asp:Label>
                                            </div> 
                                    </div>
                                </div><br />
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblTresh" runat="server" Text="Threashholds" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
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
                                                    <asp:BoundField DataField="AlertID" HeaderText="AlertID" />
                                                    <asp:BoundField DataField="Comparison" HeaderText="Comparison" />
                                                    <asp:BoundField DataField="Order" HeaderText="Order" />
                                                    <asp:BoundField DataField="Extra" HeaderText="Extra" Visible="false" />
                                                    <asp:BoundField DataField="Extra1" HeaderText="Extra1" Visible="false"/>
                                                    <asp:BoundField DataField="Extra2" HeaderText="Extra2" Visible="false"/>
                                                    <asp:BoundField DataField="Extra3" HeaderText="Extra3" Visible="false"/>
                                                    <asp:BoundField DataField="TabularRowCnt" HeaderText="TabularRowCnt" Visible="false"/>
                                                    <asp:BoundField DataField="Field1" HeaderText="Field1" Visible="false"/>
                                                    <asp:BoundField DataField="Field2" HeaderText="Field2" Visible="false"/>
                                                    <asp:BoundField DataField="GroupID" HeaderText="GroupID" Visible="false"/>
                                                    <asp:BoundField DataField="LocatonID" HeaderText="LocationID" Visible="false"/>
                                                    <asp:BoundField DataField="IsTemplate" HeaderText="IsTemplate" />
                                                </Columns>
                                            </asp:GridView>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblValidatesThreshholds" runat="server" Text="" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>

                            </asp:WizardStep>
                            <asp:WizardStep runat="server" StepType="Finish" Title="Verify Details" ID="Wiz4">
                                <div class="row" style="margin-top:35px">
                                    <div class="col-sm-2">
                                        <div class="form-group"> 
                                        <asp:Label ID="Label18" Font-Bold="true" runat="server" Text="Alert Message" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label20" Font-Bold="true" runat="server" Text="Message Type" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <hr />
                                        <asp:CheckBoxList ID="chkMessageType" runat="server" RepeatColumns="3" Width="488px" Enabled="false" CssClass="form-control">
                                        </asp:CheckBoxList>
                                        <hr />
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label21" Font-Bold="true" runat="server" Text="Configure Options" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label22" Font-Bold="true" runat="server" Text="Include Images" CssClass="form-control"></asp:Label>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="AlertIncludeImageVerify" runat="server" RepeatColumns="2" Width="176px" Enabled="False" CssClass="form-control">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                            </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:CheckBoxList ID="AlertCameraImagesVerify" runat="server" Height="64px" RepeatColumns="2" Width="496px" Enabled="False" CssClass="form-control">
                                        </asp:CheckBoxList>
                                            </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group"> 
                                        <asp:Label ID="Label23" Font-Bold="true" runat="server" Text="Image Delayed Send" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label24" runat="server" Text="Delay 1" CssClass="form-control"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtDelay1Verify" runat="server" MaxValue="15" MinValue="0" ValueText="0" Enabled="False" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label25" runat="server" Text="Delay 2" CssClass="form-control">  </asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:TextBox ID="txtDelay2Verify" runat="server" MaxValue="15" MinValue="0" ValueText="0" Enabled="False" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                    </div>
                                        </div>
                               
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label26" Font-Bold="true" runat="server" Text="Include Sensor Values" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label27" runat="server" Text="Sensor1:" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label ID="lblSensor1" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label29" runat="server" Text="Field:" CssClass="form-control">  </asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label ID="lblField1" runat="server" Text="" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label31" runat="server" Text="Sensor2:" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label ID="lblSensor2" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label34" runat="server" Text="Field:" CssClass="form-control">  </asp:Label>
                                    </div>
                                        </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label ID="lblField2" runat="server" Text="" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label36" Font-Bold="true" runat="server" Text="Enabled" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                                    </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="AlertEnabledVerify" runat="server" RepeatColumns="2" Width="176px" Enabled="False" CssClass="form-control">
                                            <asp:ListItem Selected="True">True</asp:ListItem>
                                            <asp:ListItem>False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                    </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label37" Font-Bold="true" runat="server" Text="Send Return to Normal Alert" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                                    </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:RadioButtonList ID="AlertSendRTNVerify" runat="server" RepeatColumns="2" Width="176px" Enabled="False" CssClass="form-control">
                                            <asp:ListItem>True</asp:ListItem>
                                            <asp:ListItem Selected="True">False</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label38" Font-Bold="true" runat="server" Text="Contacts" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                                    </div>
                                <div class="row">
                                    <div class="col-sm-8">
                                        <div class="form-group">
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
                                    </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label39" Font-Bold="true" runat="server" Text="Threshholds" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:GridView ID="GridThreashholdsVerify" CssClass="gvdatatable table table-striped table-bordered" Width="85%" runat="server" AutoGenerateColumns="false" DataKeyNames="Id">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="Id" InsertVisible="False" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"/>
                                                    <asp:BoundField DataField="SensorID" HeaderText="SensorID" SortExpression="Surname"/>
                                                    <asp:BoundField DataField="DeviceID" HeaderText="DeviceID" />
                                                    <asp:BoundField DataField="Field" HeaderText="Field" />
                                                    <asp:BoundField DataField="TestType" HeaderText="TestType" />
                                                    <asp:BoundField DataField="CheckValue" HeaderText="CheckValue" />
                                                    <asp:BoundField DataField="HoldPeriod" HeaderText="HoldPeriod" />
                                                    <asp:BoundField DataField="AlertID" HeaderText="AlertID" />
                                                    <asp:BoundField DataField="Comparison" HeaderText="Comparison" />
                                                    <asp:BoundField DataField="Order" HeaderText="Order" />
                                                    <asp:BoundField DataField="Extra" HeaderText="Extra" Visible="false"/>
                                                    <asp:BoundField DataField="Extra1" HeaderText="Extra1" Visible="false"/>
                                                    <asp:BoundField DataField="Extra2" HeaderText="Extra2" Visible="false"/>
                                                    <asp:BoundField DataField="Extra3" HeaderText="Extra3" Visible="false"/>
                                                    <asp:BoundField DataField="TabularRowCnt" HeaderText="TabularRowCnt" />
                                                    <asp:BoundField DataField="Field1" HeaderText="Field1" Visible="false"/>
                                                    <asp:BoundField DataField="Field2" HeaderText="Field2" Visible="false"/>
                                                    <asp:BoundField DataField="GroupID" HeaderText="GroupID" Visible="false"/>
                                                    <asp:BoundField DataField="LocatonID" HeaderText="LocationID" Visible="false"/>
                                                    <asp:BoundField DataField="IsTemplate" HeaderText="IsTemplate" />
                                                </Columns>
                                            </asp:GridView>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label40" runat="server" Text="Saving Success Status" CssClass="form-control" ></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label41" runat="server" Text="" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label42" runat="server" Text="" CssClass="form-control"></asp:Label>
                                    </div>
                                        </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label ID="Label1as" runat="server" Text="" CssClass="form-control">Verify details and click finish to save .</asp:Label>
                                    </div>
                                        </div>
                                </div>
                                
                            </asp:WizardStep>

                        </WizardSteps>
                    </asp:Wizard>
                </div> 
    </div>
    </div>
     <asp:Button ID="btnAddAnother" runat="server" Text="Add Another Alert" Visible="False" />
</div> 
    </div>


</asp:Content>
