<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="website2016.Index" %>

<%--<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.v7.1, Version=7.1.20071.40, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>--%>

<%@ Register Src="~/Dashboard/liveMonColumn.ascx" TagPrefix="uc2" TagName="liveMonColumn" %>
<%@ Register Src="~/Dashboard/liveMonPIE.ascx" TagPrefix="uc2" TagName="liveMonPIE" %>
<%@ Register Src="~/Dashboard/liveMonLine.ascx" TagPrefix="uc2" TagName="liveMonLine" %>
<%@ Register Src="~/Dashboard/liveMonOnOff.ascx" TagPrefix="uc2" TagName="liveMonOnOff" %>
<%@ Register Src="~/Dashboard/liveMonGauge.ascx" TagPrefix="uc2" TagName="liveMonGauge" %>

<%@ Register Src="~/Dashboard/liveMonOnOffReal.ascx" TagPrefix="uc2" TagName="liveMonOnOffReal" %>
<!DOCTYPE html>
<html lang="en">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>LiveMon Monitoring | </title>
    <link runat="server" rel="shortcut icon" href="images/Eye.png" type="image/x-icon"/>
    <!-- Bootstrap core CSS -->

   <%-- <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="fonts/css/font-awesome.min.css" rel="stylesheet">
    <link href="css/animate.min.css" rel="stylesheet" />--%>
<%-- 
    <link href="css/custom.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="css/maps/jquery-jvectormap-2.0.3.css" />
    <link href="css/icheck/flat/green.css" rel="stylesheet" />
    <link href="css/floatexamples.css" rel="stylesheet" type="text/css" />
    <link href="css/navigation.css" rel="stylesheet" />
    <link href="css/mystyle.css" rel="stylesheet" />--%>
    <%--<script src="Scripts/jquery-2.1.0.min.js"></script>--%>
    <%--<script src="js/jquery.min.js"></script>--%>
    
    


<!--  stuf-->
            
<!-- jQuery -->
<script src="../Content/plugins/jquery/jquery.min.js"></script>
    <link rel="stylesheet" href="../Content/plugins/fontawesome-free/css/all.min.css">
  <!-- Ionicons -->
  <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
  <!-- Tempusdominus Bbootstrap 4 -->
  <link rel="stylesheet" href="../Content/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
  <!-- iCheck -->
  <link rel="stylesheet" href="../Content/plugins/icheck-bootstrap/icheck-bootstrap.min.css">
  <!-- JQVMap -->
  <link rel="stylesheet" href="../Content/plugins/jqvmap/jqvmap.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="../Content/dist/css/adminlte.min.css">
  <!-- overlayScrollbars -->
  <link rel="stylesheet" href="../Content/plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
  <!-- Daterange picker -->
  <link rel="stylesheet" href="../Content/plugins/daterangepicker/daterangepicker.css">
  <!-- summernote -->
  <link rel="stylesheet" href="../Content/plugins/summernote/summernote-bs4.css">
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">

    <!--  Stuf end -->
    
    
    
    <script src="js/nprogress.js"></script>
   
    <style type="text/css">
         .auto-style1 {
             height: 37px;
         }
         .goRight{
             float:right;
         }
         .txtbox{
            display: block;
            float:right;
            height: 29px;
            width: 150px;
            margin-top:1px;
        }
        .btncls{
            display: block;
            float:right;
       
            margin-top:1px;
            margin-right:10px;
            width: 60px;
        }
        .btncls1{
            display: block;
            float:right;
       
            margin-top:1px;
            margin-right:10px;
            width: 100px;
        }
       .next20{
           float:right;
           width:120px;
           margin-top:5px;
       }
       .Prev20{
           width:120px;
           margin-top:5px;
           margin-left:2px;
       }
       .leaveSpace{
           margin-left:0px;
       }
       .pSetting{
           font-family:'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;
           font-size:17px;
       }
     </style>
    <style>
        .sidebar-dark-primary .nav-sidebar>.nav-item>.nav-link.active, .sidebar-light-primary .nav-sidebar>.nav-item>.nav-link.active {
    background-color:  rgba(255,255,255,.1) !important;
    color: #fff !important;
}

        [class*=sidebar-dark-] .nav-treeview>.nav-item>.nav-link.active, [class*=sidebar-dark-] .nav-treeview>.nav-item>.nav-link.active:focus, [class*=sidebar-dark-] .nav-treeview>.nav-item>.nav-link.active {
    background-color: #7a8187 !important;
    color: #343a40 !important;
}
        .nav-sidebar>.nav-item .nav-icon.fa, .nav-sidebar>.nav-item .nav-icon.fab, .nav-sidebar>.nav-item .nav-icon.far, .nav-sidebar>.nav-item .nav-icon.fas, .nav-sidebar>.nav-item .nav-icon.glyphicon, .nav-sidebar>.nav-item .nav-icon.ion {
    font-size: 1.1rem;
    /*color: lightgoldenrodyellow;*/
}
    </style>
    <script type="text/javascript">
    //$(window).load(function () {
    //    // Animate loader off screen
    //    $(".se-pre-con").fadeOut("slow");;
    //});
    </script>
    <style type="text/css">
        /* Paste this css to your style sheet file or under head tag */
/* This only works with JavaScript, 
if it's not present, don't show loader */
.no-js #loader { display: none;  }
.js #loader { display: block; position: absolute; left: 100px; top: 0; }
.se-pre-con {
	position: fixed;
	left: 0px;
	top: 0px;
	width: 100%;
	height: 100%;
	z-index: 9999;
	background: url(images/Preloader_8.gif) center no-repeat #fff;
}
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $("body").tooltip({ selector: '[data-toggle=tooltip]' });
        });
        $(document).ready(function () {
            var navs = $('nav > ul.nav');

            // Add class .active to current link
            navs.find('a').each(function (s) {

                var cUrl = String(window.location).split('?')[0];

                if (cUrl.substr(cUrl.length - 1) === '#') {
                    cUrl = cUrl.slice(0, -1);
                }

                if ($($(this))[0].href === cUrl) {
                    //$(this).addClass('active');
                    $(this).children('i').css({ 'color': 'rgb(130 164 23)' });
                    $(this).parents('ul').add(this).each(function () {
                        //console.log($(this).parent())
                        $(this).parent().addClass('menu-open');
                        //$(this).parent().children('li').css({ 'color': 'lightgoldenrodyellow' });
                        //$(this).parent().children('.nav-item .nav-icon.ion').addClass('active');

                    });
                }
            });
        });
    </script>
    <!-- SlimScroll 1.3.0 -->
    <script src="../Content/plugins/slimScroll/jquery.slimscroll.min.js"></script>
     <link href="../Scripts/ScrollBar/style.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            var scroll_top_duration = 10;
            $('body,html').animate({
                scrollTop: 0,
            }, scroll_top_duration
            );
        });
    </script>
</head>


<body class="hold-transition sidebar-mini layout-fixed layout-navbar-fixed layout-footer-fixed" style="margin-bottom:100px">
      <%--<div class="se-pre-con"></div>--%>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1">
            </asp:Timer>
        </div>
        <div class="wrapper">
         <%--Login Panel --%>
        <asp:Panel ID="PanelLogin" runat="server">

            <%--<section class="content">
      <div class="container-fluid"  style="text-align:center">
          <div class="row">
         
              <div class="col-12 col-sm-6 col-md-4"></div>
              <div class="col-12 col-sm-6 col-md-5">
                   <div  style="text-align:center;margin-top:50px">
                                <img src="images/login.png"/>
                            </div>
                            <br /><br />
 
            <div class="card card-info">
              <div class="card-header" style="background-color:#94c348">
                <h3 class="card-title" style="text-align:center;float:none;font-size:25px">Log In</h3>
              </div>
              <!-- /.card-header -->
              <!-- form start -->
              <form class="form-horizontal">
                <div class="card-body">
                 <div class="form-group row">
                    <label for="inputEmail3" class="col-sm-3 col-form-label">User Name</label>
                    <div class="col-sm-7">
                      <input type="email" class="form-control" id="inputEmail3" placeholder="Email">
                    </div>
                  </div>
                  <div class="form-group row">
                    <label for="inputPassword3" class="col-sm-3 col-form-label">Password</label>
                    <div class="col-sm-7">
                      <input type="password" class="form-control" id="inputPassword3" placeholder="Password">
                    </div>
                  </div>
                </div>
                <!-- /.card-body -->
                <div class="card-footer">
                  <button type="submit" class="btn btn-info float-right" style="background-color:#94c348">Log In</button>
                </div>
                <!-- /.card-footer -->
              </form>
            </div>
              
              </div>
              <div class="col-12 col-sm-6 col-md-4"></div>
              </div>
           <div class="row">
         
              <div class="col-12 col-sm-6 col-md-4"></div>
              <div class="col-12 col-sm-6 col-md-5">
                   <br /><br />
                <div class="clearfix"></div>
                            <div class="separator" >
                                <p class="change_link" style="text-align:center">
                                    <label style="color:#ffffff">Our website!</label>
                                    <a href="http://www.livemonitoring.co.za" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"> www.livemonitoring.co.za </a>
                                </p>
                                <div class="clearfix"></div>
                                <br />
                                <div  style="text-align:center">
                                    <h1 style="color:#94c348"><img src="images/Eye.png" /> Live Monitoring!</h1>
                                    <p style="color:#94c348">&copy; <%: DateTime.Now.Year %> All Rights Reserved. Live Monitoring! Profit Through Innovation.</p>
                                </div>
                            </div>
              </div>
              <div class="col-12 col-sm-6 col-md-4"></div>
              </div>
              </div>
      
                
                </section>--%>
      <section class="content" id="login">
      <div class="container-fluid"  style="text-align:center">
          <div class="row">
         
              <div class="col-12 col-sm-6 col-md-4"></div>
              <div class="col-12 col-sm-6 col-md-5">
                   <div  style="text-align:center;margin-top:50px">
                                <img src="images/login.png"/>
                            </div>
                            <br /><br />
 
            <div class="card card-info">
            <div class="Login">
                               <asp:Login ID="Login1" class="loginform" runat="server" DisplayRememberMe="false" BackColor="#EFF3FB"
                                   BorderColor="#B5C7DE" BorderStyle="Solid" BorderWidth="1px"  ForeColor="#333333" 
                                   Font-Names="Verdana" Font-Size="1.0em" Width="550px" Style="position: static;border-radius:10px" Height="206px" OnAuthenticate="Login1_Authenticate">
                                  <TitleTextStyle BackColor="#94c348" Font-Bold="True" Font-Size="1.5em" ForeColor="White" />
                                  <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                  <TextBoxStyle Font-Size="1.0em" Width="300px" />
                                   
                                  <LoginButtonStyle BackColor="#94c348" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="1.2em" ForeColor="white" Height="40px" />
                               </asp:Login>
                            </div>
            </div>
              
              </div>
              <div class="col-12 col-sm-6 col-md-4"></div>
              </div>
           <div class="row">
         
              <div class="col-12 col-sm-6 col-md-4"></div>
              <div class="col-12 col-sm-6 col-md-5">
                   <br /><br />
                <div class="clearfix"></div>
                            <div class="separator" >
                                <p class="change_link" style="text-align:center">
                                    <label style="color:#ffffff">Our website!</label>
                                    <a href="http://www.livemonitoring.co.za" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"> www.livemonitoring.co.za </a>
                                </p>
                                <div class="clearfix"></div>
                                <br />
                                <div  style="text-align:center">
                                    <h1 style="color:#94c348"><img src="images/Eye.png" /> Live Monitoring!</h1>
                                    <p style="color:#94c348">&copy; <%: DateTime.Now.Year %> All Rights Reserved. Live Monitoring! Profit Through Innovation.</p>
                                </div>
                            </div>
              </div>
              <div class="col-12 col-sm-6 col-md-4"></div>
              </div>

              </div>
      
                
                </section>
            
            <!-- /.card -->
            <%--<div class="wrapper">
                <div id="wrapper">
                    <div id="login" class="animate form">
                        <section class="login_content" style="width:474px;margin-left:500px">
                            <div style="margin-left:100px;margin-top:20px">
                                <img src="images/login.png"/>
                            </div>
                            <br /><br />
                            <br /><br />
                            <div class="clearfix"></div>
                            <div class="separator" style="margin-left:100px">
                                <p class="change_link">
                                    <label style="color:#ffffff">Our website!</label>
                                    <a href="http://www.livemonitoring.co.za" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"> www.livemonitoring.co.za </a>
                                </p>
                                <div class="clearfix"></div>
                                <br />
                                <div>
                                    <h1 style="color:#94c348;margin-left:20px"><img src="images/Eye.png" /> Live Monitoring!</h1>
                                    <p style="color:#94c348">&copy; <%: DateTime.Now.Year %> All Rights Reserved. Live Monitoring! Profit Through Innovation.</p>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
            </div>--%>
        </asp:Panel>
         <%--Dashboard Panel --%>
        <asp:Panel ID="dashboardpanel" runat="server" Visible="false">
        <!-- Navbar -->
  <nav class="main-header navbar navbar-expand navbar-white navbar-light">
    <!-- Left navbar links -->
    <ul class="navbar-nav">
      <li class="nav-item">
        <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
      </li>
      <li class="nav-item d-none d-sm-inline-block">
        <a href="index.aspx" class="nav-link">Dashboard</a>
      </li>
    </ul>

    <!-- SEARCH FORM -->
    <div class="form-inline ml-3">
      <div class="input-group input-group-sm" >
           <asp:DropDownList ID="cmbCurrentSite" Width="300px" runat="server" CssClass="form-control form-control-navbar" AutoPostBack="true" ToolTip="Select Site to view ." OnSelectedIndexChanged="cmbCurrentSite_SelectedIndexChanged">
                    </asp:DropDownList>
           <asp:Label ID="lblUser" style="color:ghostwhite" runat="server"></asp:Label>
                                      

             <asp:LinkButton id="logout" CssClass="float-right" Text="Log out" runat="server" OnClick="LogoutButton_Click"/>
        
        </div>
    </div>

    <!-- Right navbar links -->
    <ul class="navbar-nav ml-auto">
      <!-- Messages Dropdown Menu -->
      <li class="nav-item dropdown">
        <a class="nav-link" data-toggle="dropdown" href="#">
          <i class="far fa-comments"></i>
          <span class="badge badge-danger navbar-badge">3</span>
        </a>
        <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
          <a href="#" class="dropdown-item">
            <!-- Message Start -->
            <div class="media">
              <img src="dist/img/user1-128x128.jpg" alt="User Avatar" class="img-size-50 mr-3 img-circle">
              <div class="media-body">
                <h3 class="dropdown-item-title">
                  Brad Diesel
                  <span class="float-right text-sm text-danger"><i class="fas fa-star"></i></span>
                </h3>
                <p class="text-sm">Call me whenever you can...</p>
                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
              </div>
            </div>
            <!-- Message End -->
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <!-- Message Start -->
            <div class="media">
              <img src="dist/img/user8-128x128.jpg" alt="User Avatar" class="img-size-50 img-circle mr-3">
              <div class="media-body">
                <h3 class="dropdown-item-title">
                  John Pierce
                  <span class="float-right text-sm text-muted"><i class="fas fa-star"></i></span>
                </h3>
                <p class="text-sm">I got your message bro</p>
                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
              </div>
            </div>
            <!-- Message End -->
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <!-- Message Start -->
            <div class="media">
              <img src="dist/img/user3-128x128.jpg" alt="User Avatar" class="img-size-50 img-circle mr-3">
              <div class="media-body">
                <h3 class="dropdown-item-title">
                  Nora Silvester
                  <span class="float-right text-sm text-warning"><i class="fas fa-star"></i></span>
                </h3>
                <p class="text-sm">The subject goes here</p>
                <p class="text-sm text-muted"><i class="far fa-clock mr-1"></i> 4 Hours Ago</p>
              </div>
            </div>
            <!-- Message End -->
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item dropdown-footer">See All Messages</a>
        </div>
      </li>
      <!-- Notifications Dropdown Menu -->
      <li class="nav-item dropdown">
        <a class="nav-link" data-toggle="dropdown" href="#">
          <i class="far fa-bell"></i>
          <span class="badge badge-warning navbar-badge">15</span>
        </a>
        <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
          <span class="dropdown-item dropdown-header">15 Notifications</span>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <i class="fas fa-envelope mr-2"></i> 4 new messages
            <span class="float-right text-muted text-sm">3 mins</span>
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <i class="fas fa-users mr-2"></i> 8 friend requests
            <span class="float-right text-muted text-sm">12 hours</span>
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <i class="fas fa-file mr-2"></i> 3 new reports
            <span class="float-right text-muted text-sm">2 days</span>
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item dropdown-footer">See All Notifications</a>
        </div>
      </li>
      <li class="nav-item">
        <a class="nav-link" data-widget="control-sidebar" data-slide="true" href="#" role="button">
          <i class="fas fa-th-large"></i>
        </a>
      </li>
    </ul>
  </nav>
  <!-- /.navbar -->

  <!-- Main Sidebar Container -->
  <aside class="main-sidebar sidebar-dark-primary elevation-4">
    <!-- Brand Logo -->
    <a href="index3.html" class="brand-link">
      <img src="images/Eye.png" alt="AdminLTE Logo" class="brand-image img-circle elevation-3"
           style="opacity: .8">
      <span class="brand-text font-weight-light">Live Monitoring</span>login
    </a>

    <!-- Sidebar -->
    <div class="sidebar">
      <!-- Sidebar user panel (optional) -->
      <div class="user-panel mt-3 pb-3 mb-3 d-flex">
        <div class="image">
          <%--<img src="images/Eye.png" class="img-circle elevation-2" alt="User Image">--%>
            <i class="fa fa-user img-circle elevation-2" style="color: white;padding: 10px;"></i>
        </div>
        <div class="info">
          <a href="#" class="d-block"><asp:Label ID="lblUser2" runat="server"></asp:Label></a>
             <div class="text-white">
         <asp:Label ID="LastLogin" runat="server"></asp:Label>
        </div>
        </div>
      </div>

      <!-- Sidebar Menu -->
      <nav class="mt-2">
        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
          <!-- Add icons to the links using the .nav-icon class
               with font-awesome or any other icon font library -->
          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-th"></i>
              <p>
                Dashboard
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
              <li class="nav-item">
                <a href="index.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Dashboard</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="KVAGraphDisplay.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>KVA Gauge Dashboard</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="DashboardKVA.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>KVA Gridview Dashboard</p>
                </a>
              </li>
            </ul>
          </li>
            <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
                <i class="nav-icon fas fa-microchip"></i>
              <p>
                Sensors
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview">
              <li class="nav-item">
                <a href="AddNewSensor.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Add New Sensor</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="EditSensor.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Edit Sensor</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="SensorStatus.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Sensor Status</p>
                </a>
              </li>
                <li class="nav-item">
                <a href="StatusByType.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Status By Type</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="AddSensorNote.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Sensor Add Notes</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="SensorGroups.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Sensor Groups</p>
                </a>
              </li>
                <li class="nav-item">
                <a href="SensorAlertGroups.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Sensor Alert Groups</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="AlertGroupLinkContacts.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Alert Group Link Contacts</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="DefaultImages.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Default Images</p>
                </a>
              </li>
                 <li class="nav-item">
                <a href="AddSensorsTemplate.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Add Sensors Template</p>
                </a>
              </li>
                <li class="nav-item">
                <a href="BulkSensors.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Bulk Sensors</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="ImportSensors.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Import Sensors</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="SensorScanSchadule.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Sensors Scan Schedule</p>
                </a>
              </li>
                <li class="nav-item">
                <a href="NMDChange.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Edit NMD</p>
                </a>
              </li>
            </ul>
          </li>
           
           <li class="nav-item has-treeview"><a href="#" class="nav-link"><i class="nav-icon fas fa-cubes"></i><p>Devices<i class="right fas fa-angle-left"></i></p></a>
            <ul class="nav nav-treeview" >
                <li class="nav-item"><a href="DeviceDisplay.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Device Status</p></a></li>
                <li class="nav-item"><a href="AddIPDevice.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add IP Device</p></a></li>
                <li class="nav-item"><a href="IPDeviceEdit.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit IP Device</p></a></li>
                <li class="nav-item"><a href="IPDeviceTemplate.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add IP Devices Template</p></a></li>
                <li class="nav-item"><a href="IPDeviceBulkCreation.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Bulk IP Devices</p></a></li>
                <li class="nav-item"><a href="IPDeviceBulkImport.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Import IP Devices</p></a></li>
                <li class="nav-item"><a href="AddOtherDevices.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Other Devices</p></a></li>
                <li class="nav-item"><a href="EditOtherDevice.aspx"><i class="far fa-circle nav-icon"></i><p>Edit Other Devices</p></a></li>
                <li class="nav-item"><a href="AddOtherDeviceTemplate.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Other Devices Template</p></a></li>
                <li class="nav-item"><a href="OtherDeviceBulkCreation.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Bulk Other Devices</p></a></li>
                <li class="nav-item"><a href="ImportOtherDevices.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Import Other Devices</p></a></li>
                <li class="nav-item"><a href="AddSnmp.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add SNMP Devices</p></a></li>
                <li class="nav-item"><a href="EditSNMP.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit SNMP Devices</p></a></li>
                <li class="nav-item"><a href="AddSNMPDevicesTemplete.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add SNMP Devices Template</p></a></li>
                <li class="nav-item"><a href="BulkSNMPDevices.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Bulk SNMP Devices</p></a></li>
                <li class="nav-item"><a href="ImportSnmpDevices.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Import SNMP Devices</p></a></li>
           </ul>
          </li>
         
          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-camera"></i>
              <p>
               Cameras
                <i class="fas fa-angle-left right"></i>
                <span class="badge badge-info right">6</span>
              </p>
            </a>
            <ul class="nav nav-treeview" >
                                        <li class="nav-item"> <a href="CameraDynamic.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>All Cameras</p></a> </li>
                                        <li class="nav-item"> <a href="AddCameras.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Cameras</p></a> </li>
                                        <li class="nav-item"> <a href="EditCameras.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit Cameras</p></a> </li>
                                        <li class="nav-item"> <a href="AddCameraTemplete.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Cameras Template</p></a> </li>
                                        <li class="nav-item"> <a href="BulkCameras.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Bulk Cameras</p></a> </li>
                                        <li class="nav-item"> <a href="ImportCameras.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Import Cameras</p></a> </li>
            </ul>
          </li>
           
          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-exclamation-triangle"></i>
              <p>
                Alerts
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
                                             <li class="nav-item"> <a href="AddAlertWizard.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Alert Wizard</p></a> </li>
                                             <li class="nav-item"> <a href="AddAlert.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Alert</p></a> </li>
                                             <li class="nav-item"> <a href="EditAlerts.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit Alert</p></a> </li>
                                             <li class="nav-item"> <a href="AddAlertSchedule.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Alert Schedule</p></a> </li>
                                             <li class="nav-item"> <a href="AddAlertTemplates.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Alert Template</p></a> </li>
                                             <li class="nav-item"> <a href="LinkTemplates.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Bulk Configure Alerts</p></a> </li>
                                             <li class="nav-item"> <a href="EditAlertTemplates.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit Alert Template</p></a> </li>
                                             <li class="nav-item"> <a href="AlertContact.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Alert Contact</p></a> </li>
                                             <li class="nav-item"> <a href="EditAlertContact.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit Alert Contact</p></a> </li>
            </ul>
          </li>

          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-desktop"></i>
              <p>
                Displays
                <i class="fas fa-angle-left right"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
                                                 <li class="nav-item"> <a href="EditModel.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add/Edit Models</p></a> </li>
                                                 <li class="nav-item"> <a href="SLServerDisplay.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Server Display</p></a> </li>
                                                 <li class="nav-item"> <a href="SLServerDisplayPage.aspx" class="nav-link" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"><i class="far fa-circle nav-icon"></i><p>NW Server Display</p></a></li>
                                                 <li class="nav-item"> <a href="SLDeviceDisplay.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Device Display</p></a> </li>
                                                 <li class="nav-item"> <a href="SLDeviceDisplayPage.aspx" class="nav-link" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"><i class="far fa-circle nav-icon"></i><p>NW Device Display</p></a></li>
                                                 <li class="nav-item"> <a href="SLDeviceMap.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Map Display</p></a> </li>  
                                                 <li class="nav-item"> <a href="SLDeviceMapPage.aspx" class="nav-link" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"><i class="far fa-circle nav-icon"></i><p>NW Map Displays</p></a></li>
                                                 <li class="nav-item"> <a href="SLServerDisplayOOB.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>OOB Server Display</p></a> </li>
                                                 <li class="nav-item"> <a href="DisplaysMan.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Manage Display</p></a> </li>
            </ul>
          </li>

            <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-tachometer-alt"></i>
              <p>
                Metering
                <i class="fas fa-angle-left right"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
                                                     <li class="nav-item"> <a href="ManageMeteringTarriffs.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Manage Metering Tarrif</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringPowerStats.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering Stats</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringBilling.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering Billing</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringBillingCompare.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering Billing Compare</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringEnergy.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering Energy</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringTotal.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering TOU Total</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringTOUMonthly.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering TOU Monthly</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringPower.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering Power</p></a> </li>
                                                     <li class="nav-item"> <a href="ProfileDisplay.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Profile Display</p></a> </li>
                                                     <li class="nav-item"><a href="CumulativeDisplayGraph.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Cumulative KW Display</p></a> </li>
                                                     <li class="nav-item"> <a href="AddTarrif.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Tarrif</p></a> </li>
                                                     <li class="nav-item"> <a href="EditTarrif.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit Tarrif</p></a> </li>
                                                     <li class="nav-item"> <a href="MeteringEventLogs.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Metering Event Log</p></a> </li>
            </ul>
          </li>
           
          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-history"></i>
              <p>
                History
                <i class="fas fa-angle-left right"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
                                                         <li class="nav-item"> <a href="AlertHistory.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Alert History</p></a> </li>
                                                         <li class="nav-item"> <a href="Reportz.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Reports</p></a> </li>
                                                         <li class="nav-item"> <a href="Graphs.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Graphs</p></a> </li>
                                                         <li class="nav-item"> <a href="CustomGraphs.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Custom Graphs</p></a> </li>
                                                         <li class="nav-item"> <a href="ExportData.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Export Data</p></a> </li>
                                                         <li class="nav-item"> <a href="NewVideo.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>New Video</p></a> </li>
                                                         <li class="nav-item"> <a href="SystemLogs.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>System Log</p></a> </li>  
             
            </ul>
          </li>
         
          <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-cog"></i>
              <p>
                Configuration
                <i class="fas fa-angle-left right"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
                                        <li class="nav-item"> <a href="ReportsConfig.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Reports Config</p></a> </li>
                                        <li class="nav-item"> <a href="SQLReportsConfig.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>SQL Reports Config</p></a> </li>
                                        <li class="nav-item"> <a href="BackupData.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Backup Data</p></a> </li>
                                        <li class="nav-item"> <a href="ServerConfiguration.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Server Configuration</p></a> </li>
                                        <li class="nav-item"> <a href="AddUsers.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Add Users</p></a> </li>
                                        <li class="nav-item"> <a href="userEdit.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Edit Users</p></a> </li>
                                        <li class="nav-item"> <a href="addPeople.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>People</p></a> </li>
                                        <li class="nav-item"> <a href="PageSecuritySetup.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Page Security</p></a> </li>
                                        <li class="nav-item"> <a href="PageSites.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites</p></a> </li>
                                        <li class="nav-item"> <a href="SiteUsers.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites Users</p></a> </li>
                                        <li class="nav-item"> <a href="SiteSensors.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites Sensors</p></a> </li>
                                        <li class="nav-item"> <a href="SiteIPDevice.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites IP Devices</p></a> </li>
                                        <li class="nav-item"> <a href="SiteOtherDevice.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites Other Devices</p></a> </li>
                                        <li class="nav-item"> <a href="SiteSNMPDevice.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites SNMP Devices</p></a> </li>
                                        <li class="nav-item"> <a href="SiteCamerasSetup.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Sites Cameras</p></a> </li>
                                        <li class="nav-item"> <a href="SetLocation.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Set Locations</p></a> </li>
                                        <li class="nav-item"> <a href="DefaultIcons.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Default Icons</p></a> </li>
                                        <li class="nav-item"> <a href="ActiveDirectoryUnits.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Active Directory Units</p></a> </li>
                </ul>
         </li>

            <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-copy"></i>
              <p>
                Reports
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
             <li class="nav-item"><a href="Reports.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Reports</p></a></li>
            </ul>
          </li>
            <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-hourglass-start"></i>
              <p>
                Status
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
             <li class="nav-item"><a href="DeviceInfo.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Device Info</p></a></li>
             <li class="nav-item"><a href="ClientInfo.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Client Info</p></a></li>

                </ul>
          </li>
            <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-handshake"></i>
              <p>
                Help
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
             <li class="nav-item"><a href="http://www.ip-mon.com/HelpFiles/index.htm" onclick="window.open('#','_blank');window.open(this.href,'_self');"><i class="far fa-circle nav-icon"></i><p>Launch Online Help</p></a></li>
            </ul>
          </li>
            <li class="nav-item has-treeview">
            <a href="#" class="nav-link">
              <i class="nav-icon fas fa-copy"></i>
              <p>
                Dashboard
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview" >
             <li class="nav-item"><a href="index.aspx" class="nav-link"><i class="far fa-circle nav-icon"></i><p>Dashboard</p></a></li>
            </ul>
          </li>
        </ul>
      </nav>
      <!-- /.sidebar-menu -->
    </div>
    <!-- /.sidebar -->
  </aside>
            <img src="images/Eye.png" /> <span>Live Monitoring!</span>
  <!-- Content Wrapper. Contains page content -->
  <div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
          <!-- top navigation -->
                <%--<div class="top_nav" >

                    <div class="nav_menu" >
                        <nav class="" role="navigation">
                            <div class="nav toggle">
                                <a id="menu_toggle"><i class="fa fa-bars"></i></a>
                            </div>
                            <div style="position:relative" class="nav_li">
                                <ul class="nav active navbar-nav navbar-right nav-tabs" style="align-content:inherit">
                                    <li>
                                        <a href="javascript:;" class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                            
                                            <span style="color:ghostwhite">Welcome,</span>
                                            <asp:Label ID="Label1" style="color:ghostwhite" runat="server"></asp:Label>
                                        <span class=" fa fa-angle-down"></span>
                                        </a>
                                       <ul class="dropdown-menu dropdown-usermenu animated fadeInDown pull-right">
                                            
                                            <li>
                                                <a href="http://www.ip-mon.com/HelpFiles/index.htm" onclick="window.open('#','_blank');window.open(this.href,'_self');">Help</a>
                                            </li>
                                            <li>
                                                <asp:LinkButton id="LinkButton1" Text="Log out" OnClick="LogoutButton_Click" runat="server"/>
                                            </li>
                                        </ul>
                                    </li>
                                    <li><a href="index.aspx"><asp:Label ID="Label2" style="color:ghostwhite" runat="server">Dashboard</asp:Label></a></li>
                                </ul>

                            </div>
                    </nav>
                </div>
                </div>--%>
                <!-- /top navigation -->
        <%--<div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">Dashboard v2</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="#">Home</a></li>
              <li class="breadcrumb-item active">Dashboard</li>
            </ol>
          </div><!-- /.col -->
        </div>--%><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->

    <!-- Main content -->
   
        <!-- Info boxes -->
          <div class="row">
          <div class="col-md-12">
            <div class="card">
              <div class="card-header">
                  <div class="col-md-12 pull-right">
                                        <div class="col-md-2 pull-right">
                                            <div class="form-group">
                                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="pull-right form-control btn-round" AutoPostBack="true" Visible="false" ToolTip="Select Site to view ." OnSelectedIndexChanged="cmbCurrentSite_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>    
                                        </div>
                                       <%-- <div class="col-md-10 pull-right">
                                            <h3 class="pull-right">Selected Site : <small></small></h3>
                                        </div>--%>
                                    </div>
                <%--<h5 class="card-title">Selected Site:</h5>--%>
                <div class="card-tools">
                   <h5 class="card-title">Selected Site:</h5>
                </div>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
                <div class="row">
                 
                  <!-- /.col -->
                </div>
                <!-- /.row -->
              </div>
              <!-- ./card-body -->
              
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>

        <!-- /.row -->
          <div class="row">
          <div class="col-md-12">
            <div class="card">
              <div class="card-header">
                  
                <h5 class="card-title">Status Counter:</h5>
                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse">
                    <i class="fas fa-minus"></i>
                  </button>
                  <div class="btn-group">
                    <button type="button" class="btn btn-tool" data-toggle="dropdown">
                      <i class="fas fa-wrench"></i>
                    </button>
                    <div class="dropdown-menu dropdown-menu-right" role="menu">
                      <a href="#" class="dropdown-item">Action</a>
                      <a href="#" class="dropdown-item">Another action</a>
                      <a href="#" class="dropdown-item">Something else here</a>
                      <a class="dropdown-divider"></a>
                      <a href="#" class="dropdown-item">Separated link</a>
                    </div>
                  </div>
                  <%--<button type="button" class="btn btn-tool" data-card-widget="remove">
                    <i class="fas fa-times"></i>
                  </button>--%>
                </div>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
           <div class="row">

           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                  <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblOkay" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/Ok.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="okayLink" CssClass="create"  runat="server" Text="Sensors with ok status"  OnClick="okayLink_Click"/></p>
              </div>
            </div>
          </div>
           
           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                  <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblWarning" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/Warning-1.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="warnLink" CssClass="create" Text="Sensors with warning status"  runat="server" OnClick="warnLink_Click"/></p>
              </div>
            </div>
          </div>

           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                  <div class="count" style="color:white">
                                                      <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblNoresponse" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                       
                                                  </div>
                <i> <img src="Content/icons/NoResponse.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="NoResponseLink" CssClass="create" Text="Sensors with no response status"  runat="server" OnClick="NoResponseLink_Click"/></p>
              </div>
            </div>
          </div>

           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                   <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblAlert" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/Alert1.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="alertLink" CssClass="create" Text="Sensors with alert status"  runat="server" OnClick="alertLink_Click"/></p>
              </div>
            </div>
          </div>

          </div>

                  <div class="row">

           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                   <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblError" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/CriticalError.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="errLink" CssClass="create" Text="Sensors with critical error status"  runat="server" OnClick="errLink_Click"/></p>
              </div>
            </div>
          </div>
           
           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                  <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblError2" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/errorstatus.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="LinkError" CssClass="create" Text="Sensors with error status"  runat="server" OnClick="LinkError_Click"/></p>
              </div>
            </div>
          </div>

           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                  <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblDF" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/failurestatus.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="LinkDF" CssClass="create" Text="Sensors with device failure status"  runat="server" OnClick="LinkDF_Click"/></p>
              </div>
            </div>
          </div>

           <div class="col-lg-3 col-6">
            <!-- small box -->
            <div class="small-box text-center">
              <div class="inner">
                  <div class="count" style="color:white">
                                                        <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Conditional" runat="server">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblDisable" runat="server"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </div>
                <i> <img src="Content/icons/Disabled.png" style="height:50px;width:50px" /></i>
                <p><asp:LinkButton id="LinkDisable" CssClass="create" Text="Sensors with disabled status"  runat="server" OnClick="LinkDisable_Click"/></p>
              </div>
            </div>
          </div>

          </div>

                  
          <%--<div class="row">
          <div class="col-12 col-sm-6 col-md-3">
              
            <div class="info-box" style="background-color:#007bff">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/Ok.png"/></span>

              <div class="info-box-content">
                  
                <p style="color:white">Sensors with ok status</p>
                   <p style="color:ghostwhite"></p>
                
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3" style="background-color:#3f97f4">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/Warning-1.png" style="height:60px;width:53px" /></span>

              <div class="info-box-content">
                  
                <p  style="color:white">Sensors with warning status</p>
                  <p style="color:ghostwhite"></p>
             
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->

          <!-- fix for small devices only -->
          <div class="clearfix hidden-md-up"></div>

          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3" style="background-color:#4899f0">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/NoResponse.png" style="height:60px;width:53px" />/span>

              <div class="info-box-content">
                  
                                                  
                <p style="color:white">Sensors with no response status</p>
                  <p style="color:ghostwhite"></p>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3" style="background-color:#3e9afb;">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/Alert1.png"  style="height:60px;width:53px"/></span>

              <div class="info-box-content">
                 
                                                    
                <p style="color:white">Sensors with alert status</p>
                  <p style="color:ghostwhite"></p>
               
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
        </div>

          <div class="row">
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box" style="background-color:#0e79ec;">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/CriticalError.png" style="height:60px;width:53px" /></span>

              <div class="info-box-content">
                 
                                                    
                <p style="color:white">Sensors with critical error status</p>
                  <p style="color:ghostwhite"></p>
               
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3" style="background-color:#3b8ce3">
              <span class="info-box-icon elevation-1"><img src="Content/icons/errorstatus.png" style="height:60px;width:53px" /></span>

              <div class="info-box-content">
                  
                                                    
                <p style="color:white">Sensors with error status</span>
                  <p style="color:ghostwhite"></p>
               
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->

          <!-- fix for small devices only -->
          <div class="clearfix hidden-md-up"></div>

          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3" style="background-color:#549fee">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/failurestatus.png" style="height:60px;width:53px" /></span>

              <div class="info-box-content">
                  
                                                    
                <p style="color:white">Sensors with device failure status</span>
                  <p style="color:ghostwhite"></p>
                
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3" style="background-color:#568fcc">
              <span class="info-box-icon elevation-1">
                  <img src="Content/icons/Disabled.png" style="height:60px;width:53px" /></span>

              <div class="info-box-content">
                  
                <p style="color:white">Sensors with disabled status</span>
                  <p style="color:ghostwhite"></p>
               
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
        </div>--%>
          
              <!-- ./card-body -->
              
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>

        <div class="row">
          <div class="col-md-12">
            <div class="card">
              <div class="card-header">
                <h5 class="card-title">Dashboard</h5>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse">
                    <i class="fas fa-minus"></i>
                  </button>
                  <div class="btn-group">
                    <button type="button" class="btn btn-tool" data-toggle="dropdown">
                      <i class="fas fa-wrench"></i>
                    </button>
                    <div class="dropdown-menu dropdown-menu-right" role="menu">
                      <a href="#" class="dropdown-item">Action</a>
                      <a href="#" class="dropdown-item">Another action</a>
                      <a href="#" class="dropdown-item">Something else here</a>
                      <a class="dropdown-divider"></a>
                      <a href="#" class="dropdown-item">Separated link</a>
                    </div>
                  </div>
                  <%--<button type="button" class="btn btn-tool" data-card-widget="remove">
                    <i class="fas fa-times"></i>
                  </button>--%>
                </div>
              </div>
              <!-- /.card-header -->
              <div class="card-body">

                <div class="row">
                 <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <%--<h2>Dashboard</h2>--%>
                                        <ul class="nav navbar-right panel_toolbox">
                                             <li class="dropdown">
                                                <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false"><i class="fa fa-wrench"></i></a>
                                                <ul class="dropdown-menu" role="menu">
                                                    <li>
                                                        <asp:LinkButton id="btnnNewSettingsLink" Text="Add New Dashboard Settings" OnClick="btnnNewSettings_Click" runat="server"/>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton id="btnnEditSettingsLink" Text="Edit Dashboard Settings" OnClick="btnnEditSettings_Click" runat="server"/>
                                                    </li>
                                                </ul>
                                            </li>
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <asp:HiddenField ID="hdValue" runat="server" />
                                        <asp:HiddenField ID="hdFieldValue" runat="server" />

                                        <div class="form-group">
                                            <div class="col-md-12 col-sm-6 col-xs-6">
                                                <div class="col-md-6 col-sm-6 col-xs-6 box1">
                                                    <asp:PlaceHolder ID="phRow1Pos1" runat="server"></asp:PlaceHolder>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-6 box1">
                                                    <asp:PlaceHolder ID="phRow1Pos2" runat="server"></asp:PlaceHolder>
                                                </div>
                                            </div><br />
                                        </div><br /><br />

                                        <div class="form-group">
                                            <div class="col-md-12 col-sm-6 col-xs-6">
                                                <div class="col-md-6 col-sm-6 col-xs-6 box2">                                      
                                                    <asp:PlaceHolder ID="phRow1Pos3" runat="server"></asp:PlaceHolder>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-6 box2">
                                                    <asp:PlaceHolder ID="phRow2Pos1" runat="server"></asp:PlaceHolder>
                                                </div>
                                            </div><br />
                                        </div><br /><br />

                                        <div class="form-group">
                                            <div class="col-md-12 col-sm-6 col-xs-6">
                                                <div class="col-md-6 col-sm-6 col-xs-6 box2">
                                                    <asp:PlaceHolder ID="phRow2Pos2" runat="server"></asp:PlaceHolder>
                                                </div>
                                                <div class="col-md-6 col-sm-6 col-xs-6 box2">
                                                    <asp:PlaceHolder ID="phRow2Pos3" runat="server"></asp:PlaceHolder>
                                                </div>
                                            </div><br />
                                        </div><br /><br />
                                    </div>
                                </div>
                            </div>
                </div>
                <!-- /.row -->
              </div>
              <!-- ./card-body -->
              
              <!-- /.card-footer -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->

        <!-- Main row -->
        
        <!-- /.row -->
      </div><!--/. container-fluid -->
          
    <!-- /.content -->
       <footer class="main-footer">
    <strong>Copyright &copy;  <%: DateTime.Now.Year %> - Live Monitoring.| <span class="lead"> <img src="images/Eye.png" class="auto-style1" /> Live Monitoring!</span></strong>
    All rights reserved.
    <div class="float-right d-none d-sm-inline-block">
      <b>Version</b> 3.0.5
    </div>
  </footer>
  </div>
  <!-- /.content-wrapper -->
        </asp:Panel>

  <!-- Control Sidebar -->
  <aside class="control-sidebar control-sidebar-dark">
    <!-- Control sidebar content goes here -->
  </aside>
  <!-- /.control-sidebar -->
</div>
<!-- ./wrapper -->


      



<!-- jQuery UI 1.11.4 -->
<script src="../Content/plugins/jquery-ui/jquery-ui.min.js"></script>
<!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
<script>
    $.widget.bridge('uibutton', $.ui.button)
</script>
<!-- Bootstrap 4 -->
<script src="../Content/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- ChartJS -->
<script src="../Content/plugins/chart.js/Chart.min.js"></script>
<!-- Sparkline -->
<script src="../Content/plugins/sparklines/sparkline.js"></script>
<!-- JQVMap -->
<script src="../Content/plugins/jqvmap/jquery.vmap.min.js"></script>
<script src="../Content/plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
<!-- jQuery Knob Chart -->
<script src="../Content/plugins/jquery-knob/jquery.knob.min.js"></script>
<!-- daterangepicker -->
<script src="../Content/plugins/moment/moment.min.js"></script>
<script src="../Content/plugins/daterangepicker/daterangepicker.js"></script>
<!-- Tempusdominus Bootstrap 4 -->
<script src="../Content/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<!-- Summernote -->
<script src="../Content/plugins/summernote/summernote-bs4.min.js"></script>
<!-- overlayScrollbars -->
<script src="../Content/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="../Content/dist/js/adminlte.js"></script>
<!-- AdminLTE dashboard demo (This is only for demo purposes) -->
<script src="../Content/dist/js/pages/dashboard.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="../Content/dist/js/demo.js"></script>

        <%--<script src="js/bootstrap.min.js"></script>--%>
            
  
        <!-- chart js -->
         <%--<script src="js/moment.min.js"></script>--%>
        <%--<script src="js/chartjs/chart.min.js"></script>--%>
        <!-- bootstrap progress js -->
        <%--<script src="js/progressbar/bootstrap-progressbar.min.js"></script>
        <script src="js/nicescroll/jquery.nicescroll.min.js"></script>--%>
        <!-- icheck -->
       <%-- <script src="js/icheck/icheck.min.js"></script>--%>
        <!-- daterangepicker -->
        <%--<script type="text/javascript" src="js/moment.min.js"></script>
        <script type="text/javascript" src="js/datepicker/daterangepicker.js"></script>

        <script src="js/custom.js"></script>--%>

        <!-- flot js -->
        <!--[if lte IE 8]><script type="text/javascript" src="js/excanvas.min.js"></script><![endif]-->
        <%--<script type="text/javascript" src="js/flot/jquery.flot.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.pie.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.orderBars.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.time.min.js"></script>
        <script type="text/javascript" src="js/flot/date.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.spline.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.stack.js"></script>
        <script type="text/javascript" src="js/flot/curvedLines.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.resize.js"></script>--%>
       
     
        <!-- pace -->
        <script src="js/pace/pace.min.js"></script>
      
        <script>
            NProgress.done();
        </script>
    </form>
 <a href="#0" class="cd-top">Top</a>
 <script src="~/Scripts/ScrollBar/main.js"></script>

</body>

</html>