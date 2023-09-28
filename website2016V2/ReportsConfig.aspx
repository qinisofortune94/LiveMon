<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportsConfig.aspx.cs" Inherits="website2016V2.ReportsConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <script>

        function ValidateFax() {
            //var regex = new RegExp("^\\+[0-9]{1,3}-[0-9]{3}-[0-9]{7}$");
            var regex = new RegExp("[0](\d{9})|([0](\d{2})( |-)((\d{3}))( |-)(\d{4}))|[0](\d{2})( |-)(\d{7})");
            var fax = document.getElementById("txtFaxNumber").value;
            if (fax != '') {
                if (regex.test(fax)) {
                    alert("Fax Number Is Valid");
                } else {
                    alert("Fax Number Is Invalid");
                }
            } else {
                alert("Enter Fax Number.");
            }
        }

    </script>

    <h3>Reports Config</h3>

    <%--<div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width: 1080px">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width: 1080px">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width: 1080px">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>--%>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Reports Config"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div id="divMainContent">
                    <div id="divDisplay" runat="server">
                        <h2>Display</h2>
                        <asp:GridView ID="gvReports" runat="server" CssClass="gvdatatable table table-striped table-bordered" width="100%"  EmptyDataText="No Reports Found" CellPadding="4" ForeColor="#333333" Height="80px" GridLines="None" OnSelectedIndexChanged="gvReports_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div id="divControls" runat="server">
                        <h2>Controls </h2>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnRemove_Click" /><br />
                            </div>
                            <div class="col-md-1"></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnAdd" runat="server" Text="Show Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAdd_Click" /><br />
                            </div>
                            <div class="col-md-1"></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnShowEdit" runat="server" Text="Show Edit" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnShowEdit_Click" /><br />
                           </div>
                            <div class="col-md-1"></div>
                           <div class="col-md-2">
                                <asp:Button ID="btnShowScheduling" runat="server" Text="Show Scheduling" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnShowScheduling_Click" />
                            </div>
                        </div>
                    </div>

                    <div id="divAddFunctionality" runat="server">
                        <div class="row">
                            <div class="col-md-2">Name:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportName" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Description:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDescription" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Type:</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReportType" runat="server" required="true" Width="250px" Height="34px" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <asp:Button ID="btnAddReport" runat="server" Text="Add" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAddReport_Click" />
                    </div>
                    
                    <div id="divScheduling" runat="server" visible="false">
                        <h2>Scheduling</h2>
                        <asp:GridView
                            ID="gvReportSchedule"
                            CssClass="Grid"
                            runat="server"
                            width="100%"
                            EmptyDataText="No Scheduling found for selected report." CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle CssClass="gridSelectedRowClass" BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle CssClass="GridHeader" BackColor="green" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>

                        <div class="row">
                            <div class="col-md-2">
                                <asp:Button ID="btnShowAddSensor" runat="server" Text="Show Add Sensor" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnShowAddSensor_Click" />
                                </div>
                            <div class="col-md-1"></div>
                            <div class="col-md-2">
                                <asp:Button ID="btnShowAddSchedule" runat="server" Text="Show Add Schedule" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnShowAddSchedule_Click" />
                            </div>
                        </div>

                        <div id="divSensorFunction" runat="server" visible="false">
                            <h2>Sensor Section.</h2>
                            <div class="row">
                                <div class="col-md-2">Choose Sensor:</div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSensors" runat="server" required="true" Width="250px" Height="34px" AutoPostBack="true"></asp:DropDownList>
                                </div>

                                <asp:Button ID="btnAddSensor" runat="server" Text="Add Sensor" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnAddSensor_Click" />
                                <asp:Button ID="btnShowSensorsForSchedule" runat="server" Text="Show Sensors for Schedule" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnShowSensorsForSchedule_Click" />
                            </div>
                        </div>

                        <div id="divSensorsForSchedule" runat="server" visible="False">
                            <asp:GridView ID="gvSensorSchedule" width="100%" CssClass="Grid" runat="server" EmptyDataText="No Sensors for the selected schedule found." CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvSensorSchedule_SelectedIndexChanged">
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle CssClass="gridSelectedRowClass" BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle CssClass="GridHeader" BackColor="green" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </div>
                    </div>

                    <div id="divSchedulingAdd" runat="server" visible="false">
                        <h2>Add Scheduling</h2>
                        <div class="row">
                            <div class="col-md-2">Report Run Days:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportRunDays" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Run monthly:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportRunMonthly" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Trigger Hour:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTriggerHour" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Data Rows:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportDataRows" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Data Period:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportDataPeriod" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Months Data Back:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMonthsDataBack" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Number of Sensors Per Report</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportNoSensorsPerReport" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Summary Hours:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportSummaryHours" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Averaging Days:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportAveragingDays" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Extra Data:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportExtraData" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Extra Data1:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportExtraData1" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Extra Value:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportExtraVal" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Report Extra Value 1:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportExtraVal1" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Hours data back:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHoursDataBack" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Tarrif:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTarrif" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Trending Hours:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReportTrendingHours" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>
                         <div class="row">
                        <div class="col-md-2">
                            <asp:Button ID="btnSchedulingAddNew" runat="server" Text="Add New Schedule" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnSchedulingAddNew_Click" />
                        </div>
                             </div>
                    </div>

                    <div id="divEditFunction" runat="server">
                        <h2>Editing</h2>
                        <div class="row">
                            <div class="col-md-2">Name:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEditReportName" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Description:</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEditDescription" runat="server"    CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">Type:</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEditReportType" runat="server" required="true"   CssClass="form-control" Width="250px" Height="34px" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>                        
                            <asp:Button ID="btnEditReport" runat="server" Text="Save Changes" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnEditReport_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div><br />
</asp:Content>
