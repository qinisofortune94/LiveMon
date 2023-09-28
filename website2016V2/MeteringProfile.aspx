<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeteringProfile.aspx.cs" Inherits="website2016V2.Metering.MeteringProfile" %>
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

     <script defer src="../Scripts/bootstrap-datepicker.js"></script>
    <script defer src="../Scripts/bootstrap-datepicker.min.js"></script>

    <link href="Content/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />

    <h3>Metering</h3>

    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Equipment List"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
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
                                <asp:BoundField DataField="MeterID" HeaderText="MeterID" SortExpression="MeterID" />
                                <asp:BoundField DataField="MarkerID" HeaderText="MarkerID" SortExpression="MarkerID" />
                                <asp:BoundField DataField="TimeStamp" HeaderText="TimeStamp" SortExpression="TimeStamp" />
                                <asp:BoundField DataField="Status" HeaderText="Status"  SortExpression="Status" />
                                <asp:BoundField DataField="Channel1" HeaderText="Channel1" SortExpression="Channel1" />
                                <asp:BoundField DataField="Channel2" HeaderText="Channel2" SortExpression="Channel2"  ></asp:BoundField>
                                <asp:BoundField DataField="Channel3" HeaderText="Channel3" SortExpression="Channel3" />
                                <asp:BoundField DataField="Channel4" HeaderText="Channel4" SortExpression="Channel4" />
                              
                            </Columns>
                        </asp:GridView>  
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
