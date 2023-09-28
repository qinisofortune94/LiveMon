<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BackupData.aspx.cs" Inherits="website2016V2.BackupData" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

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

    <h3>Backup Data</h3>
    <div class="col-md">
        <div class="alert alert-success" id="successMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblSuccess" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-warning" id="warningMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblWarning" runat="server" ></asp:Label>
        </div>
        <div class="alert alert-danger" id="errorMessage" runat="server" style="width:100%">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <asp:Label ID="lblError" runat="server" ></asp:Label>
        </div>
    </div>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Backup Data"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">

                <asp:Panel ID="panel1" runat="server" BorderStyle="Ridge" ><br />
                    <div class="col-md-2">SQL DB Backup Path:</div>
                    <div class="row">
                        <div class="col-md-2">
                            <igtxt:WebTextEdit ID="txtSQLBackupPath" runat="server" Width="250px" Height="40px"></igtxt:WebTextEdit>
                            <asp:Button ID="btnSQLBrowse" runat="server" Text="Browse" OnClick="btnSQLBrowse_Click" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">

                            <fieldset style="width: 500px">
                                <legend>Days</legend>
                                <asp:CheckBoxList ID="cbxlSQLDays" runat="server" RepeatColumns="7" RepeatDirection="Horizontal">
                                    <asp:ListItem>Monday</asp:ListItem>
                                    <asp:ListItem>Tuesday</asp:ListItem>
                                    <asp:ListItem>Wednesday</asp:ListItem>
                                    <asp:ListItem>Thursday</asp:ListItem>
                                    <asp:ListItem>Friday</asp:ListItem>
                                    <asp:ListItem>Saturday</asp:ListItem>
                                    <asp:ListItem>Sunday</asp:ListItem>
                                </asp:CheckBoxList>
                            </fieldset>

                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <igtxt:WebDateTimeEdit ID="txtBackupSQLTime" runat="server" DisplayModeFormat="H:mm:ss" EditModeFormat="H:mm:ss" Width="152px">
                                <SpinButtons Display="OnRight" />
                            </igtxt:WebDateTimeEdit>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="btnSQLDBBackupPath" runat="server" Text="Set SQL DB Backup" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" />
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <asp:Panel ID="panel2" runat="server" BorderStyle="Ridge" ><br />
                    <div class="col-md-2">Video DB Backup Path:</div>

                    <div class="row">
                        <div class="col-md-2">
                            <igtxt:WebTextEdit ID="txtVideoDBBackupPath" runat="server" Width="250px" Height="40px"></igtxt:WebTextEdit>
                            <asp:Button ID="btnVideoBrowse" runat="server" Text="Browse" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnVideoBrowse_Click" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <fieldset style="width: 500px">
                                <legend>Days</legend>
                                <asp:CheckBoxList ID="cbxlVideoDays" runat="server" RepeatColumns="7" RepeatDirection="Horizontal">
                                    <asp:ListItem>Monday</asp:ListItem>
                                    <asp:ListItem>Tuesday</asp:ListItem>
                                    <asp:ListItem>Wednesday</asp:ListItem>
                                    <asp:ListItem>Thursday</asp:ListItem>
                                    <asp:ListItem>Friday</asp:ListItem>
                                    <asp:ListItem>Saturday</asp:ListItem>
                                    <asp:ListItem>Sunday</asp:ListItem>
                                </asp:CheckBoxList>
                            </fieldset>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <igtxt:WebDateTimeEdit ID="txtBackupVideoTime" runat="server" DisplayModeFormat="H:mm:ss" EditModeFormat="H:mm:ss" Width="152px">
                                <SpinButtons Display="OnRight" />
                            </igtxt:WebDateTimeEdit>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-2"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnVideoDBBackupPath" runat="server" Text="Set Video DB Backup" Width="250px" Height="40px" class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnVideoDBBackupPath_Click" />
                        </div>
                    </div>
                </asp:Panel>

                <div>
                    <asp:Label ID="lblEdit" runat="server" Text="Label" Font-Bold="True" Visible="False" Width="192px"></asp:Label></div>

                <div class="row">
                    <div class="col-md-2">
                        <ignav:UltraWebTree ID="FolderBrowser" runat="server" OnNodeClicked="FolderBrowser_NodeClicked"
                            DefaultImage="ig_treeFolder.gif" DefaultSelectedImage="ig_treeFolderOpen.gif" Height="297px" Width="608px" BackImageUrl="" Cursor="Default" FileUrl="" Font-Names="Microsoft Sans Serif" Font-Size="9pt" ImageDirectory="/ig_common/WebNavigator31/" Indentation="20" JavaScriptFilename="" JavaScriptFileNameCommon="" LeafNodeImageUrl="" ParentNodeImageUrl="" RootNodeImageUrl="" TargetFrame="" TargetUrl="" Visible="False" WebTreeTarget="ClassicTree">
                            <NodeStyle>
                                <Padding Bottom="2px" Left="2px" Right="2px" Top="2px" />
                            </NodeStyle>
                            <SelectedNodeStyle BackColor="Navy" ForeColor="White" BorderStyle="Solid" BorderWidth="1px" Cursor="Default">
                                <Padding Bottom="1px" Left="2px" Right="2px" Top="1px" />
                            </SelectedNodeStyle>
                            <NodeEditStyle Font-Names="Microsoft Sans Serif" Font-Size="9pt">
                            </NodeEditStyle>
                        </ignav:UltraWebTree>
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
