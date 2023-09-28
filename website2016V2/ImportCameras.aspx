<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportCameras.aspx.cs" Inherits="website2016V2.ImportCameras" %>
<%@ Register Assembly="nsoftware.InSpreadsheetWeb" Namespace="nsoftware.InSpreadsheet" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">Import Cameras</h3>
</div>
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
       
    <br />
<div class="card-body">
<div class="card" style="font-size:13px" id="accordion" role="tablist" aria-multiselectable="true">
    <div class="card-header">
          <h3 class="card-title">Import Cameras</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
    <div class="row">
                    <div class="col-sm-12">
                         <div>
           <cc1:Exceldoc ID="Exceldoc1" runat="server"></cc1:Exceldoc>
                <br />
               
            </div>

              <div>
             
                <br />
            </div>

             <div id="collapseFour" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <div class="row">
                    <div class="col-md-2">Number of rows</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="TxtRows" runat="server" PlaceHolder="enter number of rows" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                   

                    </div>
                    <div class="col-md-2"> Select File</div>
                        
                    <div class="col-md-4">
                          <asp:FileUpload ID="FileUpload1" runat="server" /><br />
                    </div>
                    </div>



                <br />
                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="BtnLoad" runat="server" Text="Load" Width="250px" Height="40px"  class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    <div class="col-md-2">
                    </div>

                    <div class="col-md-4">
                        <asp:Button ID="BtnImportSubmit" runat="server" Text="Submit" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="btnClear_Click" />
                    </div>
                </div>

                  <%--<asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>--%>
            </div>
                    </div>
                </div>
    </div>
    </div>

    <div class="card" style="font-size:13px" id="accordion1" role="tablist" aria-multiselectable="true">
    <div class="card-header">
          <h3 class="card-title">Display Groups</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
  
  <div class="row">
                   <div class="col-md-12">

       <asp:GridView ID="gdvBulk" runat="server" AllowPaging="True"  CssClass="gvdatatable table table-striped table-bordered" AutoGenerateEditButton="false" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="IPAdress" HeaderText="IPAdress" Visible="true" />
                                <asp:BoundField DataField="Port" HeaderText="Port" Visible="true" />
                                <asp:BoundField DataField="User" HeaderText="User" Visible="true" />
                                <asp:BoundField DataField="Password" HeaderText="Password" Visible="true" />
                               <%-- <asp:BoundField DataField="ImageNormal" HeaderText="ImageNormal" Visible="true" />
                                <asp:BoundField DataField="ImageError" HeaderText="ImageError" Visible="true" />
                                <asp:BoundField DataField="ImageNoResponse" HeaderText="ImageNoResponse" Visible="true" />
                                <asp:BoundField DataField="DTLastRead" HeaderText="DTLastRead" Visible="true" />--%>
                                <asp:BoundField DataField="Caption" HeaderText="Caption" Visible="true" />
                                <asp:BoundField DataField="MotionSensitivity" HeaderText="MotionSensitivity" Visible="true" />
                                <asp:BoundField DataField="Field1" HeaderText="Field1" Visible="false" />
                                <asp:BoundField DataField="Field2" HeaderText="Field2" Visible="false" />
                                <asp:BoundField DataField="Field3" HeaderText="Field3" Visible="false" />
                                <asp:BoundField DataField="Field4" HeaderText="Field4" Visible="false" />
                                <asp:BoundField DataField="Field5" HeaderText="Field5" Visible="false" />
                                <asp:BoundField DataField="Field6" HeaderText="Field6" Visible="false" />
                                <asp:BoundField DataField="Field7" HeaderText="Field7" Visible="false" />
                                <asp:BoundField DataField="Field8" HeaderText="Field8" Visible="false" />
                                <asp:BoundField DataField="Field9" HeaderText="Field9" Visible="false" />
                                <asp:BoundField DataField="Field10" HeaderText="Field10" Visible="false" />
                                <asp:BoundField DataField="PreEventTime" HeaderText="PreEventTime" Visible="true" />
                                <asp:BoundField DataField="PostEventTime" HeaderText="PostEventTime" Visible="true" />
                                <asp:BoundField DataField="Events" HeaderText="Events" Visible="true" />
                                <asp:BoundField DataField="EventRecording" HeaderText="EventRecording" Visible="true" />
                                <asp:BoundField DataField="ItemDetection" HeaderText="ItemDetection" Visible="true" />
                             <%--   <asp:BoundField DataField="ProxySensID" HeaderText="ProxySensID" Visible="true" />--%>
                            </Columns>
                            
                        </asp:GridView>
                        </div>
                    </div>

           
    </div>
    </div>
  
</div>
  </div> 
   
</asp:Content>
