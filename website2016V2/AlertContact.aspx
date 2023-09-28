<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertContact.aspx.cs" Inherits="website2016V2.AlertContact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">

 <div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">Alert Contact</h3>
</div>
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
       
    <br />
<div class="card-body">
<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Add Contact</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
        <div class="row">
                    <div class="col-sm-2">Person:</div>
                    <div class="col-sm-4">
                  <asp:DropDownList ID="ddlPerson" AutoPostBack="true" runat="server"  required="true" CssClass="form-control" Width="250px" Height="34px" OnSelectedIndexChanged="ddlPerson_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Button ID="btnAddPerson" runat="server" Text="Add Person" Width="250px" Height="50px" class="btn bg-gray form-control" OnClick="AddPerson_Click1"/>
                    </div>
                    <div class="col-sm-2">Name:</div>
                    <div class="col-sm-4">
                           <asp:TextBox ID="txtName" runat="server" PlaceHolder="Please enter name" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>

                </div>

                 <div class="row">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-4">
                    </div>
                    <div class="col-sm-2"></div>
                     <asp:TextBox ID="txtAlertID" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
                     
                    <div class="col-sm-4">
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-4">
                        <hr />
                       <asp:CheckBoxList ID="AlertContactType" runat="server" Height="64px" Width="344px" RepeatColumns="3">
                    </asp:CheckBoxList>
                        <hr />
                    </div>
                    <div class="col-sm-2"></div>
                    <div class="col-sm-4">
                     
                    </div>
                </div>
         
                   <div class="row">
                    <div class="col-sm-2">Email:</div>
                    <div class="col-sm-4">
                
                        <asp:TextBox ID="txtEmail" runat="server" PlaceHolder="Please enter  email" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">Cell Number:</div>
                    <div class="col-sm-4">
                  
              
                        <asp:TextBox ID="txtCellNumber" runat="server" PlaceHolder="Please enter cell number" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
            
                 <div class="row">
                    <div class="col-sm-2">IM Name:</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtIMName" runat="server" PlaceHolder="Please enter  IM name" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">	Other:</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtOther" runat="server" PlaceHolder="Please enter other" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2">Out Puts:</div>
                    <div class="col-sm-4">
                        
                    </div> 
                    
                    <asp:CheckBoxList ID="AlertContactOutput" runat="server" Height="56px" Width="344px" RepeatColumns="3">
                    </asp:CheckBoxList>
                    
                      <div class="row">
                   <div class="col-sm-2"></div>
                    <div class="col-sm-4">
                        
                    </div>
                    </div>
                    </div>

                 <div class="row">
                    <div class="col-sm-2">Resend Delay:</div>
                    <div class="col-sm-4">
                        <input type="number" id="txtResend" runat="server" placeholder="Please enter  Resend" Width="250px" Height="34px" value="60">
                        <%--<asp:TextBox ID="txtResend" runat="server" PlaceHolder="Please enter  Resend" CssClass="form-control" Width="250px" Height="34px" Text="0" ></asp:TextBox>--%>
                    </div>
                      <div class="row">
                   <div class="col-sm-2"></div>
                    <div class="col-sm-4">
                        <asp:CheckBox ID="chksingle" runat="server" text="Single Send"></asp:CheckBox>
                    </div>
                    </div>
                     </div>

                <br />
                <div class="row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
                        <asp:Button ID="btnSend" runat="server" Text="Save & Schedule" Width="250px" Height="50px" class="btn bg-gray form-control" OnClick="btnSend_Click1"/>
                         <asp:Button ID="btnSchedule" runat="server" Text="Save Only" Width="250px" Height="50px" class="btn bg-gray form-control" OnClick="btnSchedule_Click"/>
                    </div>
                    <div class="col-sm-2">
                    </div>

                    <div class="col-sm-4">
                        <asp:Button ID="btnThreshold" runat="server" Text="Finished Set Threashold" Width="250px" Height="50px" class="btn bg-gray form-control" OnClick="btnThreshold_Click" />
                    </div>
                </div>
                   
    </div>
    </div>
    <div class="card" style="font-size:13px" id="accordion1">
    <div class="card-header">
          <h3 class="card-title">Exsisting Alert Contact</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
       <div class="row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">
             
                            <asp:GridView ID="GridContacts" runat="server" 
                                CellPadding="4" ForeColor="#333333" GridLines="None" Width="936px">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                            <br />
                
                    </div>
                    <div class="col-sm-2">
                    </div>

                    <div class="col-sm-4">
                       
                    </div>
         
        </div>
    </div>
    </div>
</div>
</div>
</asp:Content>