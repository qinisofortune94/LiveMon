<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSensorNote.aspx.cs" Inherits="website2016V2.AddSensorNote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    
    <link href="plugins/jquery.datetimepicker.css" rel="stylesheet" />
    <h3>New Sensor Note</h3>

    
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
        <asp:Label ID="lblDisplay" Visible="false" runat="server"></asp:Label>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Type of sensor"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                
                <div   id="AddNotes">
                
                <div class="row">
                    <div class="col-md-2">Sensor:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="drpSensor" runat="server" AutoPostBack="true" PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="drpSensor_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">Sensor Field:</div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="drpSensorField" runat="server" PlaceHolder="Please select sensor Field" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropdownList>
                    </div>
                </div>
               

                <div class="row" >
                    <div class="col-md-2">Sensor Note:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNotes" runat="server"  Rows="6"  TextMode="MultiLine"  PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>

                    <div class="col-md-2">Employee:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="drpUser" runat="server" PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    
                </div>

                <div class="row">
                    <div class="col-md-2">Event Date:</div>
                    <div class="col-md-4">
                        <input type="text" value="<%= this.InputValue %>" id="EventDate" placeholder="select date" name="EventDate" class="form-control" style="width: 250px; height: 34px"  />
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label ID="lblDelete" runat="server" Text="Delete?"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="chkDelete" runat="server" Visible="false" Checked="false" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                          
                    </div>
                </div>
                    <br />
                 <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSave" runat="server" Text="Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSave_Click"/>
                        <asp:Label ID="lblSucces" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnClearNewSensorNote" runat="server" Text="Clear" Width="250px" Height="40px" class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="BtnClearNewSensorNote_Click"/>
                    </div>
                </div>
               </div>

              <div   id="EditNotes1" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-2">Sensor:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="drpEditSensor" runat="server" AutoPostBack="true" PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">Sensor Field:</div>
                    <div class="col-md-4">
                        <asp:DropdownList ID="drpEditField" runat="server" PlaceHolder="Please select sensor Field" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropdownList>
                    </div>
                </div>
               

                <div class="row">
                    <div class="col-md-2">Sensor Note:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEditNotes" runat="server"  Rows="6"  TextMode="MultiLine"  PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>

                    <div class="col-md-2">Employee:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="drpEditUsers"  runat="server" PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                    
                </div>

                <div class="row">
                    <div class="col-md-2">Event Date:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="EditEventDate" TextMode="DateTimeLocal" runat="server" PlaceHolder="Please enter event date" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                    </div>
                </div>

                 <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnEdit" runat="server" Text="Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAddNewSensorNote_Click"/>
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                            <asp:CheckBox ID="s" runat="server" Checked="false" />
                    </div>
                </div>
               </div>
            </div>
        </div>
    </div>

    <%-- Display Role Part--%>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <div class="row">
                <div class="col-md-2">
                        <h4 class="panel-title">
                            <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                                <asp:Label ID="lblAddb" runat="server" Text="Display"></asp:Label>
                            </strong>
                            </a>
                        </h4>
                </div>
                <div class="col-md-4 right">
                    <asp:DropDownList ID="drpViewSensor" OnSelectedIndexChanged="drpViewSensor_SelectedIndexChanged" runat="server" AutoPostBack="true" PlaceHolder="Select sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                </div>
                    </div>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
                         <div class="row">
                              <asp:GridView ID="gridNotes" runat="server" CssClass="gvdatatable table table-striped table-bordered" OnPageIndexChanging="gridDashboards_PageIndexChanging"
                                    CellPadding="4" ForeColor="#333333" GridLines="Both" Width="100%" AutoGenerateSelectButton="True" AllowPaging="True" OnSelectedIndexChanged="gridNotes_SelectedIndexChanged">
                                </asp:GridView>

                            </div>
     
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="plugins/jquery.js"></script>
    <script src="plugins/jquery.datetimepicker.js"></script>
    <script>
        $('#EventDate').datetimepicker({
            //mask: '9999/19/39 29:59'
        });

        var logic = function (currentDateTime) {
            if (currentDateTime.getDay() == 6) {
                this.setOptions({
                    minTime: '11:00'
                });
            } else
                this.setOptions({
                    minTime: '8:00'
                });
        };
    </script>
</asp:Content>
