<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCameras.aspx.cs" Inherits="website2016V2.EditCameras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
<div class="card" style="font-size:13px">
<div class="card-header">
   <h3 class="card-title">Edit Cameras</h3>
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
<div class="card-body">
<div class="card" style="font-size:13px" id="accordion">
    <div class="card-header">
          <h3 class="card-title">Edit Cameras</h3>
        
          <div class="card-tools">

            <button type="button" class="btn btn-tool" data-card-widget="collapse" data-toggle="tooltip" title="Collapse">
              <i class="fas fa-minus"></i></button>
            
          </div>
        </div>
<div class="card-body">
    <div class="row">
                <div class="col-sm-12">
                     
                        <div class="col-sm-2" style="float:left">
                        <div class="form-group">
                             Select Camera
                            </div>
                              </div>
                    <div class="col-sm-6" style="float:left">
                        <div class="form-group">
                        <asp:DropDownList ID="cmbDevices" runat="server" AutoPostBack="True" class="form-control"
                            OnSelectedIndexChanged="cmbDevices_SelectedIndexChanged">
                        </asp:DropDownList>
                        </div>
                        </div>
                    </div>
                       
                    </div>
           

    <div id="collapseTwo">
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        IP Address
                        <asp:TextBox ID="txtIpAdrres" runat="server" PlaceHolder="Please enter  name" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                         Port
                        <asp:TextBox ID="txtPort" runat="server" PlaceHolder="Please enter surname" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                       Username
                        <asp:TextBox ID="txtUserName" runat="server" PlaceHolder="Please enter id number" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>
                  </div>
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        Password
                        <asp:TextBox ID="txtPassword" runat="server" PlaceHolder="Please enter address" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        Caption
                           <asp:TextBox ID="TxtCapiton" runat="server" PlaceHolder="Please enter date of birth" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>

                 <div class="col-sm-4">
                      <div class="form-group">
                        Type
                      <asp:DropDownList ID="DdlType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0" Selected="True">Auto</asp:ListItem>
                            <asp:ListItem Value="1">2K Model 1 Chanel</</asp:ListItem>
                            <asp:ListItem Value="2">3K Model</asp:ListItem>
                            <asp:ListItem Value="3">4/5/6 K Series</asp:ListItem>
                            <asp:ListItem Value="4">RTSP model</asp:ListItem>
                            <asp:ListItem Value="5">2K with 4 channels</asp:ListItem>
                            <asp:ListItem Value="6">7K with dual streams</asp:ListItem>
                            <asp:ListItem Value="7">Dual Stream Model</asp:ListItem>
                            <asp:ListItem Value="8">Multi Stream Model</asp:ListItem>
                        </asp:DropDownList>                      </div>
                    </div>
                  </div>
        
            <div class="row">
                    <div class="col-sm-4">
                      <div class="form-group">
                        PreEvent Recording
                           <asp:TextBox ID="TxtPreEventRecording" runat="server" PlaceHolder="Please enter PreEvent Recording" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>

                    <div class="col-sm-4">
                      <div class="form-group">
                        PostEvent Recording
                        <asp:TextBox ID="TxtPostEventRecording" runat="server" PlaceHolder="Please enter PostEvent Recording" required="true" CssClass="form-control"></asp:TextBox>
                      </div>
                    </div>

                 
                  </div>
            
             <br /><hr />

            <div class="row">
                    <div class="col-sm-12">
                      <!-- text input -->
                      <div class="form-group">
                       <asp:CheckBoxList ID="chkEvents" runat="server" RepeatColumns="3" Width="100%">
                        <asp:ListItem Value="1"><span style="margin-left:10px"></span>Motion Detect Alert Window 1</asp:ListItem>
                        <asp:ListItem Value="2"><span style="margin-left:10px"></span>Motion Detect Alert Window 2</asp:ListItem>
                        <asp:ListItem Value="4"><span style="margin-left:10px"></span>Motion Detect Alert Window 3</asp:ListItem>
                        <asp:ListItem Value="256"><span style="margin-left:10px"></span>Digital Input Low 1</asp:ListItem>
                        <asp:ListItem Value="512"><span style="margin-left:10px"></span>Digital Input Low 2</asp:ListItem>
                        <asp:ListItem Value="1024"><span style="margin-left:10px"></span>Digital Input Low 3</asp:ListItem>
                        <asp:ListItem Value="2048"><span style="margin-left:10px"></span>Digital Input Low 4</asp:ListItem>
                        <asp:ListItem Value="65536"><span style="margin-left:10px"></span>Digital Input High 1</asp:ListItem>
                        <asp:ListItem Value="131072"><span style="margin-left:10px"></span>Digital Input High 2</asp:ListItem>
                        <asp:ListItem Value="262144"><span style="margin-left:10px"></span>Digital Input High 3</asp:ListItem>
                        <asp:ListItem Value="524288"><span style="margin-left:10px"></span>Digital Input High 4</asp:ListItem>
                        <asp:ListItem Value="2097152"><span style="margin-left:10px"></span>Digital Input Rising 1</asp:ListItem>
                        <asp:ListItem Value="4194304"><span style="margin-left:10px"></span>Digital Input Rising 2</asp:ListItem>
                        <asp:ListItem Value="8388608"><span style="margin-left:10px"></span>Digital Input Rising 3</asp:ListItem>
                        <asp:ListItem Value="16777216"><span style="margin-left:10px"></span>Digital Input Rising 4</asp:ListItem>
                        <asp:ListItem Value="33554432"><span style="margin-left:10px"></span>Digital Input Falling 1</asp:ListItem>
                        <asp:ListItem Value="67108864"><span style="margin-left:10px"></span>Digital Input Falling 2</asp:ListItem>
                        <asp:ListItem Value="134217728"><span style="margin-left:10px"></span>Digital Input Falling 3</asp:ListItem>
                        <asp:ListItem Value="268435456"><span style="margin-left:10px"></span>Digital Input Falling 4</asp:ListItem>
                    </asp:CheckBoxList>
                      </div>
                    </div>
                  
            </div>
              <br /><hr />
            <div class="row">
                    
                    <div class="col-md-4">
                        <div class="form-group">Event Recording Enabled</div>
                     </div>
                    <div class="col-md-4">
                         <div class="form-group">
                         <asp:CheckBox ID="chkEventEnabled" runat="server" />
                        </div>
                    </div>

                    </div>
            <br />
            <div class="row">
                <div class="col-sm-4">
                       <h3 class="card-title">File Uploads</h3>
                </div>
            </div>
            
            <br />
                <div class="row">
                      <div class="col-md-4">
                          <div class="form-group">Image No Response</div>
                      </div>
                    <div class="col-md-8">
                        <div class="form-group">
                        <asp:FileUpload ID="filImageError" runat="server" />
                        <asp:Image ID="imgError" runat="server" Height="50px" />
                            </div>
                    </div>
                  </div>
                       <br />
                <div class="row">
                    <div class="col-md-4"><div class="form-group">Image Normal</div></div>
                    <div class="col-md-8">
                        <div class="form-group">
                         <asp:FileUpload ID="filImageNormal" runat="server" />
                        <asp:Image ID="imgNormal" runat="server" Height="50px" />
                            </div>
                    </div>
                  </div>
                  <br />
                <div class="row">
                     <div class="col-md-4"><div class="form-group">Image Error</div></div>
                    <div class="col-md-8">
                    <div class="form-group">
                         <asp:FileUpload ID="filImageNoResponse" runat="server" />    
                         <asp:Image ID="imgResponse" runat="server" Height="50px" />
                    </div>
                    </div>
                  </div>
                    <br />
                <div class="row">
                <div class="col-sm-4">
                      <div class="form-group">
                          Device Location
                      </div>
                    </div>
                    <div class="col-sm-4">
                      <div class="form-group">
                          <asp:Dropdownlist ID="DdlDevicelocation" runat="server" CssClass="form-control" ></asp:Dropdownlist>
                      </div>
                    </div>
                    <div class="col-sm-4">
                      <div class="form-group">                        
                          <asp:Button ID="btnChangeLocation" runat="server" Text="Change Location" Width="250px" Height="40px" class="btn btn-default form-control" BorderColor="#0099FF" OnClick="btnChangeLocation_Click" />
                      </div>
                    </div>
                    
           </div>
        <div class="row">
            <div class="col-sm-4">
                      <div class="form-group">
                        Device Site
                      </div>
                    </div>
             <div class="col-sm-4">
                      <div class="form-group">
                      <asp:Dropdownlist ID="DdlDeviceSite" runat="server" CssClass="form-control" ></asp:Dropdownlist>
                      </div>
                    </div>
             <div class="col-sm-4">
                      <div class="form-group"> 
                      <asp:Button ID="BtnChangeSite" runat="server" Text="Change Site" Width="250px" Height="40px" class="btn btn-default form-control" BorderColor="#0099FF" OnClick="btnChangeSite_Click" />
                      </div>
                      </div>
             </div>
        <br />
        <div class="row">
                 <div class="col-sm-2">
                      <div class="form-group">
                      </div>
                    </div>
                 <div class="col-sm-4">
                      <div class="form-group">
                       <asp:Button ID="btnSaveCamera" runat="server" Text="Save" Width="250px" Height="40px" class="btn bg-gray form-control" OnClick="btnCreate_Click" />

                      </div>
                    </div>
                 <div class="col-sm-4">
                      <div class="form-group">
 <asp:Button ID="btnDeleteCamera" runat="server" Text="Delete" Width="250px" Height="40px" class="btn bg-gray form-control" OnClick="btnDeleteCamera_Click"/>                      </div>
                    </div>
                 <div class="col-sm-2">
                      <div class="form-group">
                      </div>
                    </div>
            </div>
                   
    </div>
    </div>
    </div>

</div>
</div>
</asp:Content>

