<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAlertthreshholds.aspx.cs" Inherits="website2016V2.EditAlertthreshholds" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <h3>Edit Alert Threshholds</h3>
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
            <div class="panel-heading" role="tab" id="headingTwo">
                <div class="row">
                    <div class="col-md-2">
                        <h4 class="panel-title">
                            <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                                <asp:Label ID="lblAddb" runat="server" Text="Alert Threshholds"></asp:Label>
                            </strong>
                            </a>
                        </h4>
                    </div>
                </div>
            
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="GridThreashholds" runat="server" OnSelectedIndexChanged="GridThreashholds_SelectedIndexChanged" OnPageIndexChanging="GridThreashholds_PageIndexChanging"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" CssClass="gvdatatable table table-striped table-bordered"
                                AutoGenerateSelectButton="True">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
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
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Alert ID"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:TextBox id="txtID" runat="server" ReadOnly="True" Visible="false">
                        </asp:TextBox>
                        <asp:TextBox ID="txtAlertID" runat="server" CssClass="form-control" ReadOnly="true" Width="250px" Height="34px"></asp:TextBox>                        
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" Text="Thresh Hold Name"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>                        
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" Text="Sensor ID"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:DropDownList ID="cmbSensorID" OnSelectedIndexChanged="cmbSensorID_SelectedIndexChanged" CssClass="form-control" Width="250px" Height="34px" runat="server" AutoPostBack="True">
                        </asp:DropDownList>                        
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="cmbDeviceID" OnSelectedIndexChanged="cmbDeviceID_SelectedIndexChanged" runat="server" 
                        AutoPostBack="True" CssClass="form-control" Width="250px" Height="34px" Visible="False">
                    </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" Text="Field"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:DropDownList ID="cmbField" OnSelectedIndexChanged="cmbField_SelectedIndexChanged" runat="server" CssClass="form-control" Width="250px" Height="34px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:DropDownList ID="cmbFieldComp" runat="server" CssClass="form-control" Width="250px" Height="34px" Visible="False">
                        </asp:DropDownList>                        
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" Text="Tabular"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:TextBox id="txtTabularCnt" runat="server" Text="0" CssClass="form-control" Width="250px" Height="34px" ToolTip="The row of tabular values to check negative checks all rows of this field"></asp:TextBox>                                               
                     </div>
                </div>
                <div class="row">
                    <div id="curVals" runat="server" style="width: 1188px"> </div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" Text="Test Type"></asp:Label>
                     </div>
                </div>
                
                <div class="row">
                    <div class="col-md-12">
                        <asp:RadioButtonList ID="TestType" OnSelectedIndexChanged="TestType_SelectedIndexChanged" runat="server" RepeatColumns="3" 
                            Width="100%" AutoPostBack="True">
                        </asp:RadioButtonList>                                               
                     </div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label9" runat="server" Text="Check Value"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:TextBox id="txtCheckValue" runat="server" CssClass="form-control" Width="250px" Height="34px" Text="0"></asp:TextBox>                                                                      
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label10" runat="server" Text="Hold Period"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:TextBox id="txtHoldPeriod" runat="server" CssClass="form-control" Width="250px" Height="34px" TextMode="Number" max="999" min="0" step="1" Text="0"></asp:TextBox>                                                                                              
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label11" runat="server" Text="Comparison to other thresh holds"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                          <asp:RadioButtonList ID="Comparison" runat="server" RepeatColumns="2" 
                            Width="290px" Height="27px">
                            <asp:ListItem Selected="True" Value="0">And</asp:ListItem>
                            <asp:ListItem Value="1">Or</asp:ListItem>
                           </asp:RadioButtonList>                                                                                            
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="Label12" runat="server" Text="Order of Comparison"></asp:Label>
                     </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                    <asp:TextBox id="txtOrder" runat="server" CssClass="form-control" Width="250px" Height="34px" TextMode="Number" max="999" min="0" Text="0"></asp:TextBox>                                                                                                                    
                     </div>
                </div>
                
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblExtra" runat="server" Text="Extra String">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtExtra" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblExtra1" runat="server" Text="Extra String">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtExtra1" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>                  
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblExtra2" runat="server" Text="Must Occure(Hours)">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtExtra2" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblExtra3" runat="server" Text="Extra 3">  </asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtExtra3" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>                  
                </div><br />
                <div class="row">
                    <div class="col-md-2">
                       <asp:CheckBox ID="chkSensAlertTemplate" runat="server" Text="Alert Template" />
                    </div>
                </div>
                
                <br />
                <div class="row">
                    <div class="col-md-2">
                        <asp:Button ID="cmdSaveNew" runat="server" Text="Save New" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdSaveNew_Click"/>
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="cmdSend" runat="server" Text="Edit" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdSend_Click"/>                        
                        <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="40px" Width="250px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="cmdDelete_Click"/>
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="cmdFinnished" runat="server" Text="Finished" Height="40px" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="cmdFinnished_Click"/>
                        <asp:Button ID="cmdClear" runat="server" Text="Clear All" Height="40px" Width="250px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="cmdClear_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div><br />
</asp:Content>
