<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatusByType.aspx.cs" Inherits="website2016V2.StatusByType" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<%@ Register Assembly="Infragistics2.WebUI.Misc.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>


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

    <%--<script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script>
        $(function () {
            $("[id$=txtDateOfBirth]").datepicker({
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
    --%>
    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Status By Type</h3>
            <%-- <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                    <i class="fas fa-minus"></i>
                </button>
                <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                    <i class="fas fa-times"></i>
                </button>
            </div>--%>
        </div>
        <div class="card-body" style="padding: 10px !important">

            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">Type of sensor</h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                </div>
                <div class="card-body" style="padding: 10px !important">
                    <div runat="server" id="form1" style="height: 273px; width: 100%">
                        <a href="../website/helpfiles/sensorstatus.htm" target="_help" title="Show help for this page!">
                            <%--<img src="~/images/root.gif">--%></a><igmisc:WebPanel
                                ID="WebPanel1" runat="server" Width="100%">
                                <Header Text="Filter">
                                </Header>
                                <Template>
                                    <ignav:UltraWebTree ID="SiteTree" runat="server"
                                        DefaultImage="ig_treeOfficeFolder.gif"
                                        DefaultSelectedImage="ig_treeOfficeFolder.gif"
                                        Font-Names="Microsoft Sans Serif" Font-Size="8pt" Height="220px" HoverClass=""
                                        Indentation="20" Width="100%"
                                        ScrollTopPos="1" ImageDirectory="~/ig_common/Images/" OnNodeClicked="SiteTree_NodeClicked" Style="margin-bottom: 8px">
                                        <Images>
                                            <DefaultImage Url="~/images/folder.gif"></DefaultImage>
                                            <SelectedImage Url="~/images/folderopen.gif"></SelectedImage>
                                            <ExpandImage Url="~/images/plus.gif"></ExpandImage>
                                            <CollapseImage Url="~/images/minus.gif"></CollapseImage>
                                        </Images>
                                        <Nodes>
                                            <ignav:Node
                                                Expanded="True"
                                                ImageUrl="~/Images/otheroptions.ico"
                                                SelectedImageUrl="~/Images/otheroptions.ico"
                                                TagString="root"
                                                TargetFrame=""
                                                TargetUrl=""
                                                Text="All Sensors">
                                                <Images>
                                                    <DefaultImage Url="~/Images/otheroptions.ico" />
                                                    <SelectedImage Url="~/Images/otheroptions.ico" />
                                                </Images>
                                            </ignav:Node>
                                            <ignav:Node
                                                ImageUrl="~/images/eventlog.ico"
                                                TagString="Alerts"
                                                TargetFrame=""
                                                TargetUrl=""
                                                Text="Alert Sensors">
                                                <Images>
                                                    <DefaultImage Url="~/images/eventlog.ico" />
                                                </Images>
                                            </ignav:Node>
                                            <%--  <ignav:Node 
                                Text="Groups"
                                TagString="Groups">
                            </ignav:Node>--%>
                                            <ignav:Node
                                                Text="Location"
                                                TagString="Location">
                                            </ignav:Node>
                                            <ignav:Node
                                                Text="Status"
                                                TagString="Status">
                                            </ignav:Node>

                                        </Nodes>
                                        <Levels>
                                            <ignav:Level Index="0" />
                                        </Levels>
                                        <NodeStyle>
                                            <Padding Bottom="2px" Left="2px" Right="2px" Top="2px" />
                                        </NodeStyle>
                                        <SelectedNodeStyle BackColor="#316AC5" ForeColor="White">
                                            <Padding Bottom="2px" Left="2px" Right="2px" Top="2px" />
                                        </SelectedNodeStyle>
                                    </ignav:UltraWebTree>
                                    <%--<asp:Timer ID="Timer1" runat="server">
            </asp:Timer>--%>
                                </Template>
                            </igmisc:WebPanel>
                        <br />

                        <%--  </form>--%>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    <h3 class="card-title">Status By Type</h3>
                    <div class="card-tools">
                        <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
                            <i class="fas fa-minus"></i>
                        </button>
                        <button type="button" class="btn btn-tool" data-card-widget="remove" data-toggle="tooltip" title="Remove">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                </div>
                <div class="card-body" style="padding: 10px !important">
                    <div id="MyDiv" runat="server">

                        <%=MyTable %>

                        <asp:Button ID="btnPrev20" runat="server" Text="<<< Prev 20" CssClass="Prev20 btn btn-sm" Style="background-color: #ced4da"  OnClick="btnPrev20_Click" />
                        <asp:Button ID="btnNext20" runat="server" Text="Next 20 >>>" CssClass="next20  btn btn-sm" Style="background-color: #ced4da" OnClick="btnNext20_Click" />

                        <asp:Label ID="lblerr" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                        <input type="hidden" id="StartNo" value="0" runat="server" />
                        <input type="hidden" id="EndNo" value="20" runat="server" />
                        <input type="hidden" id="MaxNo" value="0" runat="server" />

                        <%--<div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="GridView1" CssClass="gvdatatable table table-striped table-bordered" runat="server" OnPreRender="GridView1_PreRender" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvSample_Commands">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="EditItem">
                                       
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DeleteItem" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id"  InsertVisible="False" ReadOnly="True"></asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
                                <asp:BoundField DataField="IdNumber" HeaderText="Id Number" SortExpression="IdNumber" />
                                <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" DataFormatString="{0:dd MMM yyyy}" SortExpression="DateOfBirth" />
                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                             


                            </Columns>
                        </asp:GridView>  
                    </div>
                </div>--%>
                    </div>

                </div>
            </div>

        </div>
    </div>

    <%--    Create role part--%>





    <%--<div id="accordion" role="tablist" aria-multiselectable="true">
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
              
          
    </div>
   </div>
        </div>--%>

    <%-- Display Role Part--%>






    <%--<div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Status By Type"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                &nbsp;
            </div>
        </div>
    </div>--%>
    <%--<br />--%>
</asp:Content>
