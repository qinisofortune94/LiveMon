<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Graphs.aspx.cs" Inherits="website2016V2.Graphs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <script src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.12/js/dataTables.bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>


    <%--<link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" rel="stylesheet" />

    <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <script>
        $(function () {
            $("[id$=txtStart], [id$=txtEnd]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                //startDate: new Date(),
                format: 'dd-mm-yyyy',
                buttonImage: '/Images/calender.png',
                autoclose: true,
                orientation: 'auto bottom',

            });

        });
    </script>


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
                         title: 'Users',
                         exportOptions: {
                             columns: [3, 4, 5, 6, 7],
                         }
                     },
                      {
                          extend: 'excel',
                          text: 'Excel',
                          title: 'Users',
                          exportOptions: {
                              columns: [3, 4, 5, 6, 7],
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

    <h3>Graphs</h3>

               <div class="col-md">
                   <div class="success" id="successMessage"  runat="server">
                            <asp:Label ID="lblSucces" runat="server"  Width="200px"></asp:Label>
                    </div>
                    <div class="warning" id="warningMessage"  runat="server">
                            <asp:Label ID="lblWarning" runat="server"  Width="400px"></asp:Label>
                    </div>
                        <div class="error" id="errorMessage"  runat="server">
                            <asp:Label ID="lblError" runat="server"  Width="200px"></asp:Label>
                    </div>
               </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Graphs"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <div class="row">
                    <div class="col-md-2">Selected Sensor:</div>
                    <div class="col-md-4">
                        <asp:Panel ID="pnlTreeView" runat="server"  BorderStyle="Solid" Width="250px">
                            <asp:TreeView ID="TreeView1" runat="server">
                                <Nodes>
                                    <asp:TreeNode Expanded="false" Text="Temperature">
                                        <asp:TreeNode Text="THD"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Expanded="false" Text="HW-Group">
                                        <asp:TreeNode Text="Posedion 2250"></asp:TreeNode>
                                        <asp:TreeNode Text="Posedion 3268"></asp:TreeNode>
                                        <asp:TreeNode Text="Posedion 3265"></asp:TreeNode>
                                        <asp:TreeNode Text="Pos XML Temp"></asp:TreeNode>
                                        <asp:TreeNode Text="Pos XML Humid"></asp:TreeNode>
                                        <asp:TreeNode Text="Conteg PDU"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Expanded="false" Text="BMS">
                                        <asp:TreeNode Text="PINH GOOGLE"></asp:TreeNode>
                                        <asp:TreeNode Text="PING LIVE MONITORING"></asp:TreeNode>
                                        <asp:TreeNode Text="PING CNN"></asp:TreeNode>
                                        <asp:TreeNode Text="Africa Wild Ping"></asp:TreeNode>
                                    </asp:TreeNode>
                                </Nodes>
                            </asp:TreeView>
                        </asp:Panel>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-md-2">Filter By Sensor:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtName" runat="server" PlaceHolder="Filter by sensor" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>

                        <asp:Button ID="Button1" runat="server" Text="Filter" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                    </div>

                    <div class="col-md-2">Data Set:</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList1" runat="server" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
                               
                <div class="row">
                    <div class="col-md-2">Default Range</div>
                </div>
               
                <div class="row">
                    <div class="col-md-2">Start Day:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStart" runat="server" PlaceHolder="Please enter start day" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">End Day:</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEnd" runat="server" PlaceHolder="Please enter end day" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>                 
                </div>                             
                
                <div class="row">
                    <div class="col-md-2">Default Ranges</div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownList2" runat="server" Width="250px" Height="40px"></asp:DropDownList>
                    </div>
                </div>

                 <div class="row">
                <div class="col-md-2"></div>
                    <div class="col-md-2" style="vertical-align: middle;text-align:center;">
                        <asp:Button ID="Button2" runat="server"  Text="Generate" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                </div>    
              </div>

            </div>
          
        </div>
    </div>

</asp:Content>
