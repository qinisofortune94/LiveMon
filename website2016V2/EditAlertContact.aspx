<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditAlertContact.aspx.cs" Inherits="website2016V2.EditAlertContact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
   
<div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">Edit Alert Contact</h3>
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

<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Alert Contacts</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
           <div class="row">
                    <div class="col-sm-12">
                        <asp:GridView ID="GridContacts" AllowPaging="true" OnPageIndexChanging="GridContacts_PageIndexChanging" OnSelectedIndexChanged="GridContacts_SelectedIndexChanged" AutoGenerateSelectButton="true" CssClass="gvdatatable table table-striped table-bordered" runat="server" AutoGenerateColumns="false">

                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                <asp:BoundField DataField="Type" HeaderText="Type" Visible="true" />
                                <asp:BoundField DataField="Email" HeaderText="Email" Visible="true" />
                                <asp:BoundField DataField="Cell" HeaderText="Cell" Visible="true" />
                                <asp:BoundField DataField="Pager" HeaderText="Pager" Visible="true" />
                                <asp:BoundField DataField="Other" HeaderText="Other" Visible="true" />
                                <asp:BoundField DataField="Outputs" HeaderText="Outputs" Visible="true" />
                                <asp:BoundField DataField="ResendDelay" HeaderText="ResendDelay" Visible="true" />
                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="true" />
                                <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" Visible="true" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
    </div>
    </div>

<div class="card" style="font-size:13px" id="accordion1">
    <div class="card-header">
          <h3 class="card-title">Add</h3>
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
    
<div class="card-body">
            <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label1" runat="server" Text="People"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="drpEmployee" AutoPostBack="true" OnSelectedIndexChanged="drpEmployee_SelectedIndexChanged" runat="server" PlaceHolder="Please enter cell number" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label5" runat="server" Text="Name">  </asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="AlertContactName" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox ID="AlertID" runat="server" ReadOnly="True" CssClass="form-control" Width="250px" Height="34px" Visible="false">0</asp:TextBox>
                        <asp:TextBox ID="txtID" runat="server" ReadOnly="True" CssClass="form-control" Width="250px" Height="34px" Visible="False"></asp:TextBox>                        
                    </div>
                    <div class="col-sm-4">
                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label7" runat="server" Text="Type">  </asp:Label>
                    </div>                  
                </div>
                
                <div class="row">
                    <div class="col-sm-12">
                        <hr />
                        <asp:CheckBoxList ID="AlertContactType" runat="server" Height="64px" Width="344px" RepeatColumns="3">
                        </asp:CheckBoxList>
                        <hr />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="AlertContactEmail" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label3" runat="server" Text="Cell Number">  </asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="AlertContactCell" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label4" runat="server" Text="IM Contact"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="AlertContactIM" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label6" runat="server" Text="Other">  </asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="AlertContactOther" runat="server" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label8" runat="server" Text="Outputs">  </asp:Label>
                    </div>                  
                </div>
                
                <div class="row">
                    <div class="col-sm-2">
                        <asp:CheckBoxList ID="AlertContactOutput" runat="server" Height="56px" Width="344px" RepeatColumns="3">
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Label ID="Label9" runat="server" Text="Resend Delay"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="AlertResendDelay" runat="server" max="10080" min="0" step="1" TextMode="Number" ToolTip="How often to resent this alert." Text="60" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        
                    </div>
                    <div class="col-sm-4">
                        <asp:CheckBox ID="chkSingleSend" runat="server" Text="Single Send" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-4">           
                        <asp:Button ID="CmdSend" runat="server" OnClick="CmdSend_Click" Text="Save" Height="40px" Width="250px" class="btn bg-gray form-control"/>
                    </div>
                    <div class="col-sm-2">
                    </div>

                    <div class="col-sm-4">
                        <asp:Button ID="cmdClear" runat="server" OnClick="cmdClear_Click" Text="Clear" Height="40px" Width="250px" class="btn bg-gray form-control" />
                    </div>
                </div>
                <br /><hr />
                <div class="row">
                    <div class="col-sm-2">
                        <asp:Button ID="cmdSaveNew" OnClick="cmdSaveNew_Click" runat="server" Text="Save New" Height="40px" Width="250px" class="btn bg-gray form-control" />
                    </div>
                    <div class="col-sm-4">           
                        <asp:Button ID="cmdDelete" runat="server" OnClick="cmdDelete_Click" Text="Delete" Height="40px" Width="250px" class="btn bg-gray form-control" />
                        <asp:Button ID="cmdFinnished" runat="server" OnClick="cmdFinnished_Click" Text="Finished" Height="40px" Width="250px" class="btn bg-gray form-control" />      
                    </div>
                    <div class="col-sm-2">
                        
                        <asp:Button ID="cmdEditSchedule" Visible="false" runat="server" OnClick="cmdEditSchedule_Click" Text="Edit Schedule" Height="40px" Width="250px" class="btn bg-gray form-control" />
                    </div>

                    <div class="col-sm-4">
                        
                    </div>
                </div>
    </div>
    </div>

</div>
</div>
</asp:Content>
