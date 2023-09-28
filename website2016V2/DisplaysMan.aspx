<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplaysMan.aspx.cs" Inherits="website2016V2.DisplaysMan" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

    <h3>Manage Displays</h3>
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
  
       <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="lblAddb" runat="server" Text="Display Names"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">
      
        <%--<asp:GridView ID="gvDisplayNames" 
            runat="server" 
            Width="100%"
            AutoGenerateColumns="False" 
            BackColor="White" 
            HorizontalAlign="Center" 
            BorderColor="#3366CC" 
            BorderStyle="None" 
            BorderWidth="1px" 
            CellPadding="4" 

            onpageindexchanging="gvDisplayNames_PageIndexChanging" 
            onselectedindexchanged="gvDisplayNames_SelectedIndexChanged" 
            onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
            onrowdatabound="gvDisplayNames_RowDataBound" 
            onrowdeleting="gvDisplayNames_RowDeleting" 
            onrowediting="gvDisplayNames_RowEditing" 
            onrowupdating="gvDisplayNames_RowUpdating" 
            onsorting="gvDisplayNames_Sorting"

           >
        <RowStyle BackColor="White" ForeColor="#003399" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:CommandField ShowDeleteButton="True" />

                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="DisplayType" HeaderText="Display Type" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ExtraData" HeaderText="Extra Data" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="ExtraValue" HeaderText="Extra Value" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DefaultOrderByColumn" HeaderText="Default Order By Column" ReadOnly="True" SortExpression="ID" />
                
            </Columns>
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        </asp:GridView>
    --%>



          <asp:GridView ID="gvDisplayNames" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"      onselectedindexchanged="gvDisplayNames_SelectedIndexChanged" 

                                                onpageindexchanging="gvDisplayNames_PageIndexChanging" 
        
            onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
            onrowdatabound="gvDisplayNames_RowDataBound" 
            onrowdeleting="gvDisplayNames_RowDeleting" 
            onrowediting="gvDisplayNames_RowEditing" 
            onrowupdating="gvDisplayNames_RowUpdating" 
            onsorting="gvDisplayNames_Sorting">

                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                <asp:CommandField ShowDeleteButton="True" />

                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="DisplayType" HeaderText="Display Type" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ExtraData" HeaderText="Extra Data" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="ExtraValue" HeaderText="Extra Value" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DefaultOrderByColumn" HeaderText="Default Order By Column" ReadOnly="True" SortExpression="ID" />


                            </Columns>
                        </asp:GridView>

        <br /> 

         <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
        <asp:Button ID="lbtnAddDisplay" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnAddDisplay_Click" />                    
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
 
                        <asp:Button ID="lbtnEditDisplay" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnEditDisplay_Click" />  
                       
                    </div>
                </div>
      <%--  <asp:LinkButton ID="lbtnAddDisplay" runat="server" onclick="lbtnAddDisplay_Click">Add</asp:LinkButton>--%>
       <%-- <asp:LinkButton ID="lbtnEditDisplay" runat="server"  Visible="false" onclick="lbtnEditDisplay_Click">Edit</asp:LinkButton>--%>
       
        <br />
        <br />
        <asp:Panel ID="pnlEditDisplay" runat="server" Visible="False">
           <%-- Display name:
            <asp:TextBox ID="txtDisplayName" runat="server" Columns="50" ></asp:TextBox>
            <br />
             Display type:
             <asp:TextBox ID="txtDisplayType" runat="server" Columns="50" ></asp:TextBox>
            <br />
             Extra data:
            <asp:TextBox ID="txtExtraData" runat="server" Columns="50" ></asp:TextBox>
            <br />
             Extra value:
             <asp:TextBox ID="txtExtraValue" runat="server" Columns="50" ></asp:TextBox>
            <br />
             Default Order By Column:
             <asp:TextBox ID="txtDefaultOrderByColumn" runat="server" Columns="50" ></asp:TextBox>
            <br />
            <br />--%>

                <div class="row">
                                                       <div class="col-md-2">Display name:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayName" runat="server" PlaceHolder="Please enter Display Name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Display Type:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayType" runat="server" PlaceHolder="Please enter Display Type" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>

                                            <div class="row">
                                                       <div class="col-md-2">Extra data:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtExtraData" runat="server" PlaceHolder="Please enter Extra Data" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Extra value:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtExtraValue" runat="server" PlaceHolder="Please enter Extra Value" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>


                                         <div class="row">
                                                       <div class="col-md-2">Default Order By Column:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDefaultOrderByColumn" runat="server" PlaceHolder="Please enter Default Order by Column" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                              
                                                    </div>




            
             <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                            <asp:Button ID="lbtnDisplaySubmit" runat="server" Text="Submit" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplaySubmit_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                            <asp:Button ID="lbtnDisplayCancel" runat="server" Text="Cancel" Width="250px" Height="40px"   class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplayCancel_Click" />
                       
                    </div>
                </div>

         
           <%-- <asp:LinkButton ID="lbtnDisplaySubmit" runat="server" onclick="lbtnDisplaySubmit_Click">Submit</asp:LinkButton>--%>
            &nbsp;&nbsp;&nbsp;

        

          <%--  <asp:LinkButton ID="lbtnDisplayCancel" runat="server" onclick="lbtnDisplayCancel_Click">Cancel</asp:LinkButton>--%>
            
        </asp:Panel>
    </div>
</div>
                </div>
            </div>
           </div>




 <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a id="Second" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo"><strong>
                        <asp:Label ID="Label1" runat="server" Text="Display Groups"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo" style="padding: 1%">
                <div class="row">
                    <div class="col-md-12">

  <asp:Panel ID="pDisplayGroups" runat="server" Visible="false">
<div>
   



     <asp:GridView ID="gvDisplayGroups" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"    onselectedindexchanged="gvDisplayGroups_SelectedIndexChanged" 

                                                 onpageindexchanging="gvDisplayGroups_PageIndexChanging" 
                                                    onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
                                                    onrowdatabound="gvDisplayNames_RowDataBound" onrowdeleting="gvDisplayGroups_RowDeleting" 
                                                    onrowediting="gvDisplayNames_RowEditing" onrowupdating="gvDisplayNames_RowUpdating" 
                                                    onsorting="gvDisplayNames_Sorting">

                            <Columns>
                                 <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />

                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayID" HeaderText="Display ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="DisplayType" HeaderText="Display Type" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayImage" HeaderText="Display Image" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayWidth" HeaderText="Display Width" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="DisplayHeight" HeaderText="Display Height" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="Screen" HeaderText="Screen" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PanelNo" HeaderText="Panel No" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="PanelPos" HeaderText="Panel Pos" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="ExtraValue1" HeaderText="Panel Pos" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ExtraValue2" HeaderText="ExtraData1" ReadOnly="True" SortExpression="ID" />


                            </Columns>
                        </asp:GridView>




<%--        <asp:GridView  width="100%" ID="gvDisplayGroups" runat="server" AutoGenerateColumns="False" BackColor="White" 
                                                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                                 onselectedindexchanged="gvDisplayGroups_SelectedIndexChanged" 

                                                 onpageindexchanging="gvDisplayGroups_PageIndexChanging" 
                                                    onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
                                                    onrowdatabound="gvDisplayNames_RowDataBound" onrowdeleting="gvDisplayGroups_RowDeleting" 
                                                    onrowediting="gvDisplayNames_RowEditing" onrowupdating="gvDisplayNames_RowUpdating" 
                                                    onsorting="gvDisplayNames_Sorting">

                                                <RowStyle BackColor="White" ForeColor="#003399" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />

                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayID" HeaderText="Display ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="DisplayType" HeaderText="Display Type" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayImage" HeaderText="Display Image" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayWidth" HeaderText="Display Width" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="DisplayHeight" HeaderText="Display Height" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="Screen" HeaderText="Screen" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PanelNo" HeaderText="Panel No" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="PanelPos" HeaderText="Panel Pos" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ExtraData1" HeaderText="ExtraData1" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ExtraData2" HeaderText="ExtraData2" ReadOnly="True" SortExpression="ID" />

                                                        <asp:BoundField DataField="ExtraValue1" HeaderText="Panel Pos" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="ExtraValue2" HeaderText="ExtraData1" ReadOnly="True" SortExpression="ID" />

                                                    </Columns>
                                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                                </asp:GridView>--%>
                                            




                                                 <br />


    
           

        <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
         <asp:Button ID="lbtnDisplayGroupAdd" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupAdd_Click" />
                         
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
       
         <asp:Button ID="lbtnDisplayGroupEdit" runat="server" Text="Edit" Width="250px" Height="40px"    class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupEdit_Click" />
                       
                    </div>
                </div>
                                               <%-- <asp:LinkButton ID="lbtnDisplayGroupAdd" runat="server" onclick="lbtnDisplayGroupAdd_Click">Add</asp:LinkButton>--%>

                                              <%--  <asp:LinkButton ID="lbtnDisplayGroupEdit" runat="server" Visible="false" onclick="lbtnDisplayGroupEdit_Click">Edit</asp:LinkButton>--%>
                                    
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlDisplayGroupEdit" runat="server" Visible="False">

                                                  <%--  Group Name:
                                                    <asp:TextBox ID="txtDisplayGroupName" runat="server" Columns="50" ></asp:TextBox>
                                                    Display Type:
                                                    <asp:TextBox ID="txtDisplayGroupDisplayType" runat="server" Columns="50" ></asp:TextBox>

                                                    Display Image:
                                                    <asp:TextBox ID="txtDisplayGroupDisplayImage" runat="server" Columns="50" ></asp:TextBox>
                                                    Display Width:
                                                    <asp:TextBox ID="txtDisplayGroupDisplayWidth" runat="server" Columns="50" ></asp:TextBox>

                                                    Display Height:
                                                    <asp:TextBox ID="txtDisplayGroupDisplayHeight" runat="server" Columns="50" ></asp:TextBox>
                                                    Screen:
                                                    <asp:TextBox ID="txtDisplayGroupScreen" runat="server" Columns="50" ></asp:TextBox>

                                                    Panel No:
                                                    <asp:TextBox ID="txtDisplayGroupPanelNo" runat="server" Columns="50" ></asp:TextBox>
                                                    Panel Pos:
                                                    <asp:TextBox ID="txtDisplayGroupPanelPos" runat="server" Columns="50" ></asp:TextBox>

                                                    Extra Data1:
                                                    <asp:TextBox ID="txtDisplayGroupExtraData1" runat="server" Columns="50" ></asp:TextBox>
                                                    Extra Data2:
                                                    <asp:TextBox ID="txtDisplayGroupExtraData2" runat="server" Columns="50" ></asp:TextBox>

                                                    Extra Value1:
                                                    <asp:TextBox ID="txtDisplayGroupExtraValue1" runat="server" Columns="50" ></asp:TextBox>
                                                    Extra Value2:
                                                    <asp:TextBox ID="txtDisplayGroupExtraValue2" runat="server" Columns="50" ></asp:TextBox>--%>


                                                    <div class="row">
                                                       <div class="col-md-2">Group Name:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupName" runat="server" PlaceHolder="Please enter Display Group Name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Display Type:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupDisplayType" runat="server" PlaceHolder="Please enter Display GroupDisplay Type" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>


                                                        <div class="row">
                                                       <div class="col-md-2">Display Image:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupDisplayImage" runat="server" PlaceHolder="Please enter Display Group Display Image" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Display Width:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupDisplayWidth" runat="server" PlaceHolder="Please enter Display Group Display Width" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>


                                                    
                                                        <div class="row">
                                                       <div class="col-md-2">Display Height:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupDisplayHeight" runat="server" PlaceHolder="Please enter Display Group Display Height" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Screen:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupScreen" runat="server" PlaceHolder="Please enter Display Group Screen" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>


                                                        <div class="row">
                                                       <div class="col-md-2">Panel No:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupPanelNo" runat="server" PlaceHolder="Please enter Display GroupPanelNo" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Panel Pos:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupPanelPos" runat="server" PlaceHolder="Please enter Display GroupPanelPos" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>

                                                          <div class="row">
                                                       <div class="col-md-2">Extra Data1:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupExtraData1" runat="server" PlaceHolder="Please enter Display GroupExtraData1" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2">  Extra Data2:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupExtraData2" runat="server" PlaceHolder="Please enter Display GroupExtraData2" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>

                                                            <div class="row">
                                                       <div class="col-md-2">Extra Value1:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupExtraValue1" runat="server" PlaceHolder="Please enter Display GroupExtraValue1" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Extra Value2:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupExtraValue2" runat="server" PlaceHolder="Please enter Display GroupExtraValue2" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>

                                                    <br />
                                                    <br />


            
              <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                                  <asp:Button ID="lbtnDisplayGroupSubmit" runat="server" Text="Submit" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupSubmit_Click" />

                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
         <asp:Button ID="lbtnDisplayGroupCancel" runat="server" Text="Cancel" Width="250px" Height="40px"   class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupCancel_Click" />
                        
                       
                    </div>
                </div>
          
                                                 <%--   <asp:LinkButton ID="lbtnDisplayGroupSubmit"  runat="server" onclick="lbtnDisplayGroupSubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lbtnDisplayGroupCancel" runat="server" onclick="lbtnDisplayGroupCancel_Click">Cancel</asp:LinkButton>--%>
            
                                                </asp:Panel>

                                        <br />
                                        <br />
    </div>
</asp:Panel>
                        </div>
                    </div>

                </div>
            </div>
     </div>







  
    <asp:Panel ID="pDetails" runat="server" Visible="false">

            <table width="100%" align="center" >
        
            <tr>
                <td>
                    <asp:Button Text="Display groups links" BorderStyle="None" ID="TabDisplaySensorLink" CssClass="Initial" runat="server"
                        OnClick="TabDisplaySensorLink_Click" />
                    <asp:Button Text="Display Sensor Links" BorderStyle="None" ID="TabDisplayGroupsLinks" CssClass="Initial" runat="server"
                        OnClick="TabDisplayGroupsLinks_Click" />
                    
                    <asp:MultiView ID="MainView" runat="server">
                   
                        <asp:View ID="vDisplayGroupsLinks" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td>
                                             <%--<asp:GridView  width="100%" ID="gvDisplayGroupsLinks" runat="server" AutoGenerateColumns="False" BackColor="White" 
                                                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                                  onselectedindexchanged="gvDisplayGroupsLinks_SelectedIndexChanged" 
                                                    onpageindexchanging="gvDisplayGroupLinks_PageIndexChanging" 
                                                    onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
                                                    onrowdatabound="gvDisplayNames_RowDataBound" onrowdeleting="gvDisplayGroupsLinks_RowDeleting" 
                                                    onrowediting="gvDisplayNames_RowEditing" onrowupdating="gvDisplayNames_RowUpdating" 
                                                    onsorting="gvDisplayNames_Sorting">
                                                <RowStyle BackColor="White" ForeColor="#003399" />
                                                    <Columns>

                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />

                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayID" HeaderText="Display ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="GroupID" HeaderText="Group ID" ReadOnly="True" SortExpression="ID" />
                                                        
                                                        <asp:BoundField DataField="Screen" HeaderText="Screen" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PanelNo" HeaderText="Panel No" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PanelPos" HeaderText="Panel Pos" ReadOnly="True" SortExpression="ID" />
                                                       
                                                    </Columns>
                                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                                </asp:GridView>
--%>


                                         <asp:GridView ID="gvDisplayGroupsLinks" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"        onselectedindexchanged="gvDisplayGroupsLinks_SelectedIndexChanged" 
                                                    onpageindexchanging="gvDisplayGroupLinks_PageIndexChanging" 
                                                    onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
                                                    onrowdatabound="gvDisplayNames_RowDataBound" onrowdeleting="gvDisplayGroupsLinks_RowDeleting" 
                                                    onrowediting="gvDisplayNames_RowEditing" onrowupdating="gvDisplayNames_RowUpdating" 
                                                    onsorting="gvDisplayNames_Sorting">

                            <Columns>
                  <asp:CommandField ShowSelectButton="True" />
                                                        <asp:CommandField ShowDeleteButton="True" />

                                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="DisplayID" HeaderText="Display ID" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="GroupID" HeaderText="Group ID" ReadOnly="True" SortExpression="ID" />
                                                        
                                                        <asp:BoundField DataField="Screen" HeaderText="Screen" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PanelNo" HeaderText="Panel No" ReadOnly="True" SortExpression="ID" />
                                                        <asp:BoundField DataField="PanelPos" HeaderText="Panel Pos" ReadOnly="True" SortExpression="ID" />

                            </Columns>
                        </asp:GridView>
                                            
                                                <br />

                                   
                                        
                         <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
         <asp:Button ID="lbtnDisplayGroupsLinksAdd" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupsLinksAdd_Click" />
                         
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
         <asp:Button ID="lbtnDisplayGroupsLinksEdit" runat="server" Text="Edit" Width="250px" Height="40px" Visible="false"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupsLinksEdit_Click" />
       
                       
                    </div>
                </div>                  
                                            <%--    <asp:LinkButton ID="lbtnDisplayGroupsLinksAdd" runat="server" onclick="lbtnDisplayGroupsLinksAdd_Click">Add</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnDisplayGroupsLinksEdit" runat="server" Visible="false" onclick="lbtnDisplayGroupsLinksEdit_Click">Edit</asp:LinkButton>
                                    --%>
                                                <br />
                                                <br />
                                                <asp:Panel ID="pnlDisplayGroupsLinksEdit" runat="server" Visible="False">


                                                      <div class="row">
                                      <div class="col-md-2">Screen:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupsScreen" runat="server" PlaceHolder="Please enter Display Group Display Height" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Panel No:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupsPanelNo" runat="server" PlaceHolder="Please enter Display Group Screen" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>
                                                    
                                                <div class="row">
                                      <div class="col-md-2">Panel Pos:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayGroupsPanelPos"  runat="server" PlaceHolder="Please enter Display Group Display Height" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Panel No:</div>
                                            <div class="col-md-4">
                                                
                                                             </div>
                                                   
                                                    <br />


                                                      <asp:Button ID="lbtnDisplayGroupsLinksSubmit" runat="server" Text="Submit" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupsLinksSubmit_Click" />
         <asp:Button ID="lbtnDisplayGroupsLinksCancel" runat="server" Text="Cancel" Width="250px" Height="40px" Visible="false"   class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplayGroupsLinksCancel_Click" />
                                                   <%-- <asp:LinkButton ID="lbtnDisplayGroupsLinksSubmit"  runat="server" onclick="lbtnDisplayGroupsLinksSubmit_Click">Submit</asp:LinkButton>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lbtnDisplayGroupsLinksCancel" runat="server" onclick="lbtnDisplayGroupsLinksCancel_Click">Cancel</asp:LinkButton>
            --%>
                                                </asp:Panel>

                                                <br />
                                                <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>

                             <asp:View ID="vDisplay" runat="server">
                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                <tr>
                                    <td class="auto-style1">
           <%--                             <asp:GridView ID="gvDisplaySensorLink" 
            runat="server" 
            Width="100%"
            AutoGenerateColumns="False" 
            BackColor="White" 
            HorizontalAlign="Center" 
            BorderColor="#3366CC" 
            BorderStyle="None" 
            BorderWidth="1px" 
            CellPadding="4" 

            onpageindexchanging="gvDisplayNames_PageIndexChanging" 
            onselectedindexchanged="gvDisplaySensorLink_SelectedIndexChanged" 
            onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
            onrowdatabound="gvDisplayNames_RowDataBound" 
            onrowdeleting="gvDisplaySensorLink_RowDeleting" 
            onrowediting="gvDisplayNames_RowEditing" 
            onrowupdating="gvDisplayNames_RowUpdating" 
            onsorting="gvDisplayNames_Sorting"
           >
        <RowStyle BackColor="White" ForeColor="#003399" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:CommandField ShowDeleteButton="True" />

                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DisplayGroupID" HeaderText="DisplayGroup ID" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="SensorID" HeaderText="Sensor ID" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="ExtraData1" HeaderText="Extra Data1" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ExtraData2" HeaderText="Extra Data2" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="ExtraValue1" HeaderText="Extra Value1" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ExtraValue2" HeaderText="Extra Value2" ReadOnly="True" SortExpression="ID" />
                
            </Columns>
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        </asp:GridView>--%>




                                               <asp:GridView ID="gvDisplaySensorLink" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"        onpageindexchanging="gvDisplayNames_PageIndexChanging" 
            onselectedindexchanged="gvDisplaySensorLink_SelectedIndexChanged" 
            onrowcancelingedit="gvDisplayNames_RowCancelingEdit" 
            onrowdatabound="gvDisplayNames_RowDataBound" 
            onrowdeleting="gvDisplaySensorLink_RowDeleting" 
            onrowediting="gvDisplayNames_RowEditing" 
            onrowupdating="gvDisplayNames_RowUpdating" 
            onsorting="gvDisplayNames_Sorting">

                            <Columns>
                   <asp:CommandField ShowSelectButton="True" />
                <asp:CommandField ShowDeleteButton="True" />

                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DisplayGroupID" HeaderText="DisplayGroup ID" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="SensorID" HeaderText="Sensor ID" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="ExtraData1" HeaderText="Extra Data1" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ExtraData2" HeaderText="Extra Data2" ReadOnly="True" SortExpression="ID" />

                <asp:BoundField DataField="ExtraValue1" HeaderText="Extra Value1" ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ExtraValue2" HeaderText="Extra Value2" ReadOnly="True" SortExpression="ID" />

                            </Columns>
                        </asp:GridView>
    
        <br /> 


                                                      <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
         <asp:Button ID="lbtnDisplaySensorLinkAdd" runat="server" Text="Add" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplaySensorLinkAdd_Click" />
                         
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
       
         <asp:Button ID="lbtnDisplaySensorLinkEdit" runat="server" Text="Edit" Width="250px" Height="40px" Visible="false"   class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplaySensorLinkEdit_Click" />
                       
                    </div>
                </div>
                     
                     
                    
        <%--<asp:LinkButton ID="lbtnDisplaySensorLinkAdd" runat="server" onclick="lbtnDisplaySensorLinkAdd_Click">Add</asp:LinkButton>
        <asp:LinkButton ID="lbtnDisplaySensorLinkEdit" runat="server"  Visible="false" onclick="lbtnDisplaySensorLinkEdit_Click">Edit</asp:LinkButton>--%>
       
        <br />
        <br />
        <asp:Panel ID="pnlDisplaySensorLinkEdit" runat="server" Visible="False">

                       <div class="row">
                                      <div class="col-md-2">Sensor ID:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtSensorID" runat="server" PlaceHolder="Please enter Display Group Display Height" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Display Order:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtDisplayOrder" runat="server" PlaceHolder="Please enter Display Group Screen" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>


                <div class="row">
                                      <div class="col-md-2">Extra Data1:</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtExtraData1" runat="server" PlaceHolder="Please enter Display Group Display Height" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Default Order By Column:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtExtraData2" runat="server" PlaceHolder="Please enter Display Group Screen" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>

            
                <div class="row">
                                      <div class="col-md-2">Extra value1::</div>
                                                    <div class="col-md-4">
                                                     <asp:TextBox ID="txtExtraValue1" runat="server" PlaceHolder="Please enter Display Group Display Height" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                     </div>
                                                         <div class="col-md-2"> Extra Value2:</div>
                                            <div class="col-md-4">
                                                     <asp:TextBox ID="txtExtraValue2" runat="server" PlaceHolder="Please enter Display Group Screen" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                                                             </div>
                                                    </div>


          
          
           
           
            
          
            
                                                      <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
               <asp:Button ID="lbtnDisplaySensorLinkSubmit" runat="server" Text="Submit" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="lbtnDisplaySensorLinkSubmit_Click" />
                         
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
       
         <asp:Button ID="lbtnDisplaySensorLinkCancel" runat="server" Text="Cancel" Width="250px" Height="40px" Visible="false"   class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="lbtnDisplaySensorLinkCancel_Click" />
                       
                    </div>
                </div>
         <%--   <asp:LinkButton ID="lbtnDisplaySensorLinkSubmit" runat="server" onclick="lbtnDisplaySensorLinkSubmit_Click">Submit</asp:LinkButton>
            &nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lbtnDisplaySensorLinkCancel" runat="server" onclick="lbtnDisplaySensorLinkCancel_Click">Cancel</asp:LinkButton>
            --%>
        </asp:Panel>


                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    
                    </asp:MultiView>
                </td>
            </tr>
        </table>

    </asp:Panel>












</asp:Content>
