<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="website2016.Index" %>

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
    <%--<meta http-equiv="refresh" content="20" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>LiveMon Monitoring | </title>
    <link runat="server" rel="shortcut icon" href="images/Eye.png" type="image/x-icon"/>
    <!-- Bootstrap core CSS -->

    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="fonts/css/font-awesome.min.css" rel="stylesheet">
    <link href="css/animate.min.css" rel="stylesheet">

    <!-- Custom styling plus plugins -->
    <link href="css/custom.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="css/maps/jquery-jvectormap-2.0.3.css" />
    <link href="css/icheck/flat/green.css" rel="stylesheet" />
    <link href="css/floatexamples.css" rel="stylesheet" type="text/css" />
    <link href="css/navigation.css" rel="stylesheet" />
    <link href="css/mystyle.css" rel="stylesheet" />

    <script src="js/jquery.min.js"></script>
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
       .create{
           color:white;
       }
       .marg{
           margin-left:16px;
           margin-top:5px;
       }
       .box1 {
            display: block;
            padding: 10px;
            margin-bottom: 10px;
            text-align: justify;
        }

        .box2 {
            display: block;
            padding: 10px;
            text-align: justify;
            margin-top: 10px;
        }

     </style>

    <script src="Scripts2/highcharts.js" type="text/javascript"></script>
    <script src="Scripts2/drilldown.js" type="text/javascript"></script>
    <script src="Scripts2/exporting.js"></script>
    <script src="Scripts2/highcharts-more.js"></script>
    
</head>


<body class="nav-md">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="10000">
            </asp:Timer>
        </div>
        <%--Login Panel --%>
        <asp:Panel ID="PanelLogin" runat="server">
            <div class="">
                <div id="wrapper">
                    <div id="login" class="animate form">
                        <section class="login_content" style="width:474px;" >
                            <div>
                                <img src="images/login.png"/>
                            </div>
                            <br /><br />
                            <div class="Login">
                               <asp:Login ID="Login1" class="loginform" runat="server" DisplayRememberMe="false" BackColor="#EFF3FB"
                                   BorderColor="#B5C7DE" BorderStyle="Solid" BorderWidth="1px" BorderPadding="4" ForeColor="#333333"
                                   Font-Names="Verdana" Font-Size="1.2em" Width="474px" Style="position: static;" Height="206px" OnAuthenticate="Login1_Authenticate">
                                  <TitleTextStyle BackColor="#94c348" Font-Bold="True" Font-Size="1.6em" ForeColor="White" />
                                  <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                  <TextBoxStyle Font-Size="1.6em" />
                                  <LoginButtonStyle BackColor="#94c348" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="1.6em" ForeColor="white" />
                               </asp:Login>
                            </div><br /><br />
                            <div class="clearfix"></div>
                            <div class="separator">
                                <p class="change_link">
                                    <label style="color:#ffffff">Our website!</label>
                                    <a href="http://www.livemonitoring.co.za/" onclick="window.open('#','_blank');window.open(this.href,'_self');" class="to_register" style="color:#94c348"> www.livemonitoring.co.za/ </a>
                                </p>
                                <div class="clearfix"></div>
                                <br />
                                <div>
                                    <h1 style="color:#94c348"><img src="images/Eye.png" /> Live Monitoring!</h1>
                                    <p style="color:#94c348">&copy; <%: DateTime.Now.Year %> All Rights Reserved. Live Monitoring! Profit Through Innovation.</p>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </asp:Panel>
        
       <%--Dashboard Panel --%>
        <asp:Panel ID="dashboardpanel" runat="server" Visible="false">
        <div class="container body">
            <div class="main_container">
                <div class="col-md-3 left_col">
                    <div class="left_col scroll-view">
                        <div class="navbar nav_title" style="border: 0;">
                            <a href="index.aspx" class="site_title"><img src="images/Eye.png" /> <span>Live Monitoring!</span></a>
                        </div>
                        <div class="clearfix"></div>

                        <!-- menu prile quick info -->
                        <div class="profile">
                            <div class="profile_pic">
                                <img src="images/img.jpg" alt="..." class="img-circle profile_img">
                            </div>
                            <div class="profile_info">
                                <span>Logged in user,</span>
                                <h2><asp:Label ID="lblUser2" runat="server"></asp:Label></h2>
                            </div>
                        </div>
                        <!-- /menu prile quick info -->

                        <br />

                        <!-- sidebar menu -->
                        <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">

                            <div class="menu_section">
                                <h3><asp:Label ID="LastLogin" runat="server"></asp:Label></h3>
                                 <ul class="nav side-menu"> 
                                     <li> 
                                         <a><i class="fa fa-home"></i> Dashboard <span class="fa fa-chevron-down"></span></a>
                                          <ul class="nav child_menu" style="display: none">
                                               <li> <a href="#">Dashboard</a> </li>

                                          </ul> 

                                     </li> 
                                     <li> 
                                         <a><i class="fa fa-cogs"></i> Sensors <span class="fa fa-chevron-down"></span></a>
                                          <ul class="nav child_menu" style="display: none"> 
                                              <li> 
                                                  <asp:LinkButton id="addNewSensor" Text="Add New Sensors" OnClick="addNewSensor_Click" runat="server"/>
                                              </li>
                                              </li> 
                                     <li> <asp:LinkButton id="sensorStatus" Text="Sensor Status" OnClick="sensorStatus_Click" runat="server"/> 

                                     </li> 
                                     <li> <a href="#">Status By Type</a> </li>
                                      <li> <a href="#">Sensor Add Notes</a> </li> 
                                     <li> <a href="#">Sensor Groups</a> </li> 
                                     <li> <a href="#">Sensor Alert Groups</a> </li>
                                     <li> <a href="#">Alert Group Link Contacts</a> </li> 
                                     <li> <a href="#">Default Images</a> </li> 
                                     <li> <a href="#">Add Sensors Template</a> </li> 
                                     <li> <a href="#">Bulk Sensors</a> </li> 
                                     <li> <a href="#">Import Sensors</a> </li> 
                                     <li> <a href="#">Sensors Scan Schedule</a> </li> 

                                 </ul> </li> 
                                <li> <a><i class="fa fa-desktop"></i> Devices <span class="fa fa-chevron-down"></span></a> 
                                    <ul class="nav child_menu" style="display: none"> 
                                        <li> <a href="#">Device Status</a> </li> 
                                        <li> <a href="#">Add IP Devices</a> </li> 
                                        <li> <a href="#">Edit IP Devices</a> </li> 
                                        <li> <a href="#">Add IP Devices Template</a> </li> 
                                        <li> <a href="#">Bulk IP Devices</a> </li> 
                                        <li> <a href="#">Import IP Devices</a> </li> 
                                        <li> <a href="#">Add Other Devices</a> </li> 
                                        <li> <a href="#">Edit Other Devices</a> </li> 
                                        <li> <a href="#">Add Other Devices Template</a> </li> 
                                        <li> <a href="#">Bulk Other Devices</a> </li> 
                                        <li> <a href="#">Import Other Devices</a> </li> 
                                        <li> <a href="#">Add SNMP Devices</a> </li> 
                                        <li> <a href="#">Edit SNMP Devices</a> </li> 
                                        <li> <a href="#">Add SNMP Devices Template</a> </li> 
                                        <li> <a href="#">Bulk SNMP Devices</a> </li> 
                                        <li> <a href="#">Import SNMP Devices</a> </li> 

                                    </ul> </li> 
                                <li> <a><i class="fa fa-camera"></i> Cameras <span class="fa fa-chevron-down"></span></a> 
                                    <ul class="nav child_menu" style="display: none"> 
                                        <li> <a href="#">All Cameras</a> </li> 
                                        <li> <a href="#">Add Cameras</a> </li> 
                                        <li> <a href="#">Edit Cameras</a> </li> 
                                        <li> <a href="#">Add Cameras Template</a> </li> 
                                        <li> <a href="#">Bulk Cameras</a> </li> 
                                        <li> <a href="#">Import Cameras</a> </li> 

                                    </ul> </li> 
                                     <li> <a><i class="fa fa-arrows"></i> Alerts <span class="fa fa-chevron-down"></span></a> 
                                         <ul class="nav child_menu" style="display: none"> 
                                             <li> <a href="#">Add Alert Wizard</a> </li> 
                                             <li> <asp:LinkButton id="linkbuttonalert" Text="Add Alert" OnClick="addAlert_Click" runat="server"/> </li> 
                                             <li> <asp:LinkButton id="linkbutton1Edit" Text="Edit Alert" OnClick="editAlert_Click" runat="server"/> </li> 
                                             <li> <a href="#">Add Alert Schedule</a> </li> 
                                             <li> <a href="#">Add Alert Template</a> </li> 
                                             <li> <a href="#">Bulk Configure Alerts</a> </li> 
                                             <li> <a href="#">Edit Alert Template</a> </li> 
                                             <li> <a href="#">Add Alert Contact</a> </li> 
                                             <li> <a href="#">Edit Alert Contact</a> </li> 

                                         </ul> </li> <li> <a><i class="fa fa-bar-chart-o"></i> Displays <span class="fa fa-chevron-down"></span></a> 
                                             <ul class="nav child_menu" style="display: none"> 
                                                 <li> <a href="#">Add/Edit Models</a> </li> 
                                                 <li> <a href="#">View Models</a> </li> 
                                                 <li> <a href="#">Server Display</a> </li> 
                                                 <li> <a href="#">NW Server Display</a> </li> 
                                                 <li> <a href="#">Device Display</a> </li> 
                                                 <li> <a href="#">NW Device Display</a> </li> 
                                                 <li> <a href="#">NW Map Displays </a> </li> 
                                                 <li> <a href="#">Map Display</a> </li> 
                                                 <li> <a href="#">OOB Server Display</a> </li> 
                                                 <li> <a href="#">Manage Display</a> </li> 

                                             </ul> </li> <li> <a><i class="fa fa-mercury"></i> Metering <span class="fa fa-chevron-down"></span></a> 
                                                 <ul class="nav child_menu" style="display: none"> 
                                                     <li> <a href="#">Manage Metering Tarrif</a> </li> 
                                                     <li> <a href="#">Metering Stats</a> </li> 
                                                     <li> <a href="#">Metering Billing</a> </li> 
                                                     <li> <a href="#">Metering Billing Compare</a> </li> 
                                                     <li> <a href="#">Metering Energy</a> </li> 
                                                     <li> <a href="#">Metering TOU Total</a> </li> 
                                                     <li> <a href="#">Metering TOU Monthly</a> </li> 
                                                     <li> <a href="#">Metering Power</a> </li> 
                                                     <li> <a href="#">Profile Display</a> </li> 
                                                     <li> <a href="#">Billing Display</a> </li> 
                                                     <li> <a href="#">Cumulative KW Display</a> </li> 
                                                     <li> <a href="#">Add Tarrif</a> </li> 
                                                     <li> <a href="#">Edit Tarrif</a> </li> 
                                                     <li> <a href="#">Metering Equipment List</a> </li> 
                                                     <li> <a href="#">Metering Equipment Layout</a> </li> 
                                                     <li> <a href="#">Metering Equipment Sensors</a> </li> 
                                                     <li> <a href="MeteringEventLog.aspx">Metering Event Log</a> </li> 
                                                     <li> <a href="MeteringRescanProfile.aspx">Metering Rescan Profile</a> </li> 

                                                 </ul> </li> <li> <a><i class="fa fa-history"></i> History <span class="fa fa-chevron-down"></span></a> 
                                                     <ul class="nav child_menu" style="display: none"> 
                                                         <li> <a href="AlertHistory.aspx">Alert History</a> </li> 
                                                         <li> <a href="Reports.aspx">Reports</a> </li> 
                                                         <li> <a href="Graphs.aspx">Graphs</a> </li> 
                                                         <li> <a href="CustomGraphs.aspx">Custom Graphs</a> </li> 
                                                         <li> <a href="ExportData.aspx">Export Data</a> </li> 
                                                         <li> <a href="NewVideo.aspx">New Video</a> </li> 
                                                         <li> <a href="SystemLog.aspx">System Log</a> </li> 

                                                     </ul> </li> 
                                                <li> <a><i class="fa fa-cog"></i> Configuration <span class="fa fa-chevron-down"></span></a> 
                                                    <ul class="nav child_menu" style="display: none"> 
                                                        <li> <a href="ReportsConfig.aspx">Reports Config</a> </li> 
                                                        <li> <a href="SQLReportsConfig.aspx">SQL Reports Config</a> </li> 
                                                        <li> <a href="BackupData.aspx">Backup Data</a> </li> 
                                                        <li> <a href="ServerConfiguration.aspx">Server Configuration</a> </li> 
                                                        <li> <a href="AddUsers.aspx">Add Users</a> </li> 
                                                        <li> <a href="#">Edit Users</a> </li> 
                                                        <li> <asp:LinkButton id="linkPeople" Text="People" OnClick="linkPeople_Click" runat="server"/> </li> 
                                                        <li> <a href="PageSecuritySetup.aspx">Page Security</a> </li> 
                                                        <li> <a href="PageSites.aspx">Sites</a> </li> 
                                                        <li> <a href="SiteUsers.aspx">Sites Users</a> </li> 
                                                        <li> <a href="SiteSensors.aspx">Sites Sensors</a> </li> 
                                                        <li> <a href="SiteIPDDevices.aspx">Sites IP Devices</a> </li> 
                                                        <li> <a href="SiteOtherDevices.aspx">Sites Other Devices</a> </li> 
                                                        <li> <a href="SiteSNPDevices.aspx">Sites SNMP Devices</a> </li> 
                                                        <li> <a href="SiteCamerasSetup.aspx">Sites Cameras</a> </li> 
                                                        <li> <a href="SetLocation.aspx">Set Locations</a> </li> 
                                                        <li> <a href="DefaultIcons.aspx">Default Icons</a> </li> 
                                                        <li> <a href="ActiveDirectoryUnits.aspx">Active Directory Units</a> </li> 
                                                    </ul> </li> </ul> 
                            </div>

                            <div class="menu_section">
                                <h3>Live Monitoring</h3>
                                <ul class="nav side-menu">
                                    <li><a><i class="fa fa-tasks"></i> Reports <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu" style="display: none">
                                            <li>
                                                <a href="#">Reports</a>
                                            </li>
                                        </ul>
                                    </li>
                                     <li>
                                        <a><i class="fa fa-stack-overflow"></i> Status <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu" style="display: none">
                                            <li>
                                                <a href="DeviceInfo.aspx">Device Info</a>
                                            </li>
                                            <li>
                                                <a href="ClientInfo.aspx">Client Info</a>
                                            </li>
                                        </ul>
                                    </li>
                                     <li>
                                        <a><i class="fa fa-question"></i> Help <span class="fa fa-chevron-down"></span></a>
                                        <ul class="nav child_menu" style="display: none">
                                            <li>
                                                <a href="http://www.ip-mon.com/HelpFiles/index.htm" onclick="window.open('#','_blank');window.open(this.href,'_self');">Launch Online Help</a>
                                            </li>
                                        </ul>
                                    </li>
                                    <li><a><i class="fa fa-laptop"></i> Landing Page <span class="label label-success pull-right">Coming Soon</span></a>
                                    </li>
                                </ul>
                            </div>

                        </div>
                        <!-- /sidebar menu -->

                        <!-- /menu footer buttons -->
                        <div class="sidebar-footer hidden-small">
                            <a data-toggle="tooltip" data-placement="top" title="Settings">
                                <span class="glyphicon glyphicon-cog" aria-hidden="true"></span>
                            </a>
                            <a data-toggle="tooltip" data-placement="top" title="FullScreen">
                                <span class="glyphicon glyphicon-fullscreen" aria-hidden="true"></span>
                            </a>
                            <a data-toggle="tooltip" data-placement="top" title="Lock">
                                <span class="glyphicon glyphicon-eye-close" aria-hidden="true"></span>
                            </a>
                            <a data-toggle="tooltip" data-placement="top" title="Logout">
                                <span class="glyphicon glyphicon-off" aria-hidden="true"></span>
                            </a>
                        </div>
                        <!-- /menu footer buttons -->
                    </div>
                </div>

                <!-- top navigation -->
                <div class="top_nav" >

                    <div class="nav_menu" >
                        <nav class="" role="navigation">
                            <div class="nav toggle">
                                <a id="menu_toggle"><i class="fa fa-bars"></i></a>
                            </div>
                            <div style="position:relative" class="nav_li">
                                <ul class="nav active navbar-nav navbar-right nav-tabs" style="align-content:inherit">
                                    <li>
                                        <a href="javascript:;" class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                            <img src="images/img.jpg" alt="">
                                            <span>Welcome,</span>
                                            <asp:Label ID="lblUser" runat="server"></asp:Label>
                                        <span class=" fa fa-angle-down"></span>
                                        </a>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<ul class="dropdown-menu dropdown-usermenu animated fadeInDown pull-right">
                                            <li>
                                                <a href="javascript:;">  Profile</a>
                                            </li>
                                            <li>
                                                <a href="javascript:;">
                                                    <span class="badge bg-red pull-right">50%</span>
                                                    <span>Settings</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="http://www.ip-mon.com/HelpFiles/index.htm" onclick="window.open('#','_blank');window.open(this.href,'_self');">Help</a>
                                            </li>
                                            <li>
                                                <asp:LinkButton id="logout" Text="Log out" OnClick="LogoutButton_Click" runat="server"/>
                                            </li>
                                        </ul>
                                    </li>

                                    <li role="presentation" class="dropdown">
                                        <a href="javascript:;" class="dropdown-toggle info-number" data-toggle="dropdown" aria-expanded="false">
                                            <i class="fa fa-envelope-o"></i>
                                            <span class="badge bg-green">6</span>
                                        </a>
                                        <ul id="menu1" class="dropdown-menu list-unstyled msg_list animated fadeInDown" role="menu">
                                            <li>
                                                <a>
                                                    <span class="image">
                                                        <img src="images/img.jpg" alt="Profile Image" />
                                                    </span>
                                                    <span>
                                                        <span>John Smith</span>
                                                        <span class="time">3 mins ago</span>
                                                    </span>
                                                    <span class="message">
                                                        Film festivals used to be do-or-die moments for movie makers. They were where...
                                                    </span>
                                                </a>
                                            </li>
                                            <li>
                                                <a>
                                                    <span class="image">
                                                        <img src="images/img.jpg" alt="Profile Image" />
                                                    </span>
                                                    <span>
                                                        <span>John Smith</span>
                                                        <span class="time">3 mins ago</span>
                                                    </span>
                                                    <span class="message">
                                                        Film festivals used to be do-or-die moments for movie makers. They were where...
                                                    </span>
                                                </a>
                                            </li>
                                            <li>
                                                <a>
                                                    <span class="image">
                                                        <img src="images/img.jpg" alt="Profile Image" />
                                                    </span>
                                                    <span>
                                                        <span>John Smith</span>
                                                        <span class="time">3 mins ago</span>
                                                    </span>
                                                    <span class="message">
                                                        Film festivals used to be do-or-die moments for movie makers. They were where...
                                                    </span>
                                                </a>
                                            </li>
                                            <li>
                                                <a>
                                                    <span class="image">
                                                        <img src="images/img.jpg" alt="Profile Image" />
                                                    </span>
                                                    <span>
                                                        <span>John Smith</span>
                                                        <span class="time">3 mins ago</span>
                                                    </span>
                                                    <span class="message">
                                                        Film festivals used to be do-or-die moments for movie makers. They were where...
                                                    </span>
                                                </a>
                                            </li>
                                            <li>
                                                <div class="text-center">
                                                    <a href="inbox.html">
                                                        <strong>See All Alerts</strong>
                                                        <i class="fa fa-angle-right"></i>
                                                    </a>
                                                </div>
                                            </li>
                                        </ul>
                                    </li>
                                    <li class="">
                                        <a href="javascript:;" class="user-profile dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                            Updates
                                            <span class=" fa fa-angle-down"></span>
                                        </a>
                                        <ul class="dropdown-menu dropdown-usermenu animated fadeInDown pull-right">
                                            <li>
                                                <a href="javascript:;">  Sensor</a>
                                            </li>
                                            <li>
                                                <a href="javascript:;">
                                                    <span class="badge bg-red pull-right"></span>
                                                    <span>Mchine</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="javascript:;">
                                                    <span class="badge bg-red pull-right">60</span>
                                                    <span>Alerts</span>
                                                </a>
                                            </li>
                                        </ul>
                                    </li>
                                    <li><a href="index.aspx">Dashboard</a></li>
                                </ul>

                            </div>
                    </nav>
                </div>
                </div>
                <!-- /top navigation -->


                 <!-- page content -->
                <div class="right_col" role="main">
                    <!-- top tiles -->
                    <div class="">
                        <div class="clearfix"></div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Status Counter</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="row top_tiles">
                                            <div id="result" runat="server" class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:green">
                                                    <%--<div class="icon">
                                                        <i class="fa fa-check-circle"></i>
                                                    </div>--%>
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Ok status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="okayLink" CssClass="create" Text="View Details" runat="server" OnClick="okayLink_Click"/></p>
                                                </div>
                                            </div>
                                            <div id="result2" runat="server" class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:darkorange">
                                                    <%--<div class="icon">
                                                        <i class="fa fa-warning"></i>
                                                    </div>--%>
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Warning status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="warnLink" CssClass="create" Text="View Details" runat="server" OnClick="warnLink_Click"/></p>
                                                </div>
                                            </div>
                                            <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                               <div class="tile-stats" style="background-color:purple">
                                                  <%--<div class="icon">
                                                       <i class="fa fa-reply"></i>
                                                  </div>--%>
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

                                                  <h5 style="color:white;margin-left:12px">Sensors with No Response status</h5>
                                                   <hr />
                                                  <p style="color:white"><asp:LinkButton id="NoResponseLink" CssClass="create" Text="View Details" runat="server" OnClick="NoResponseLink_Click"/></p>
                                              </div>
                                            </div>
                                            <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:red">
                                                    <%--<div class="icon" style="color:white">
                                                        <i class="fa fa-asterisk"></i>
                                                    </div>--%>
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Alert status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="alertLink" CssClass="create" Text="View Details" runat="server" OnClick="alertLink_Click"/></p>
                                              </div>
                                            </div>
                                            <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:#8B0000">
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Critical Error status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="errLink" CssClass="create" Text="View Details" runat="server" OnClick="errLink_Click"/></p>
                                              </div>
                                          </div>
                                          <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:#DC143C">
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Error status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="LinkError" CssClass="create" Text="View Details" runat="server" OnClick="LinkError_Click"/></p>
                                              </div>
                                          </div>
                                          <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:#008b8b">
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Device Failure status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="LinkDF" CssClass="create" Text="View Details" runat="server" OnClick="LinkDF_Click"/></p>
                                              </div>
                                          </div>
                                          <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                                <div class="tile-stats" style="background-color:#a9a9a9 ">
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

                                                    <h5 style="color:white;margin-left:12px">Sensors with Disabled status</h5>
                                                    <hr />
                                                    <p style="color:white"><asp:LinkButton id="LinkDisable" CssClass="create" Text="View Details" runat="server" OnClick="LinkDisable_Click"/></p>
                                              </div>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Dashboard</h2>
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
                    </div>
                    
                    <!-- /top tiles -->
                    
              </div>
            </div>
            <!-- /page content -->

            <!-- footer content -->
            <hr />
            <footer class="dash-box-footer">
                <p class="pull-right">&copy; <%: DateTime.Now.Year %> - Live Monitoring.|
              <span class="lead"> <img src="images/Eye.png" class="auto-style1" /> Live Monitoring!</span>
            </p>
            </footer>
            </asp:Panel>
            <!-- /footer content -->
        </div>

    

        <div id="custom_notifications" class="custom-notifications dsp_none">
            <ul class="list-unstyled notifications clearfix" data-tabbed_notifications="notif-group">
            </ul>
            <div class="clearfix"></div>
            <div id="notif-group" class="tabbed_notifications"></div>
        </div>

        <script src="js/bootstrap.min.js"></script>

        <!-- gauge js -->
        <script type="text/javascript" src="js/gauge/gauge.min.js"></script>
        <script type="text/javascript" src="js/gauge/gauge_demo.js"></script>
        <!-- chart js -->
        <script src="js/chartjs/chart.min.js"></script>
        <!-- bootstrap progress js -->
        <script src="js/progressbar/bootstrap-progressbar.min.js"></script>
        <script src="js/nicescroll/jquery.nicescroll.min.js"></script>
        <!-- icheck -->
        <script src="js/icheck/icheck.min.js"></script>
        <!-- daterangepicker -->
        <script type="text/javascript" src="js/moment.min.js"></script>
        <script type="text/javascript" src="js/datepicker/daterangepicker.js"></script>

        <script src="js/custom.js"></script>

        <!-- flot js -->
        <!--[if lte IE 8]><script type="text/javascript" src="js/excanvas.min.js"></script><![endif]-->
        <script type="text/javascript" src="js/flot/jquery.flot.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.pie.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.orderBars.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.time.min.js"></script>
        <script type="text/javascript" src="js/flot/date.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.spline.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.stack.js"></script>
        <script type="text/javascript" src="js/flot/curvedLines.js"></script>
        <script type="text/javascript" src="js/flot/jquery.flot.resize.js"></script>
        <script>
            $(document).ready(function () {
                // [17, 74, 6, 39, 20, 85, 7]
                //[82, 23, 66, 9, 99, 6, 2]
                var data1 = [[gd(2012, 1, 1), 17], [gd(2012, 1, 2), 74], [gd(2012, 1, 3), 6], [gd(2012, 1, 4), 39], [gd(2012, 1, 5), 20], [gd(2012, 1, 6), 85], [gd(2012, 1, 7), 7]];

                var data2 = [[gd(2012, 1, 1), 82], [gd(2012, 1, 2), 23], [gd(2012, 1, 3), 66], [gd(2012, 1, 4), 9], [gd(2012, 1, 5), 119], [gd(2012, 1, 6), 6], [gd(2012, 1, 7), 9]];
                $("#canvas_dahs").length && $.plot($("#canvas_dahs"), [
                    data1, data2
                ], {
                    series: {
                        lines: {
                            show: false,
                            fill: true
                        },
                        splines: {
                            show: true,
                            tension: 0.4,
                            lineWidth: 1,
                            fill: 0.4
                        },
                        points: {
                            radius: 0,
                            show: true
                        },
                        shadowSize: 2
                    },
                    grid: {
                        verticalLines: true,
                        hoverable: true,
                        clickable: true,
                        tickColor: "#d5d5d5",
                        borderWidth: 1,
                        color: '#fff'
                    },
                    colors: ["rgba(38, 185, 154, 0.38)", "rgba(3, 88, 106, 0.38)"],
                    xaxis: {
                        tickColor: "rgba(51, 51, 51, 0.06)",
                        mode: "time",
                        tickSize: [1, "day"],
                        //tickLength: 10,
                        axisLabel: "Date",
                        axisLabelUseCanvas: true,
                        axisLabelFontSizePixels: 12,
                        axisLabelFontFamily: 'Verdana, Arial',
                        axisLabelPadding: 10
                            //mode: "time", timeformat: "%m/%d/%y", minTickSize: [1, "day"]
                    },
                    yaxis: {
                        ticks: 8,
                        tickColor: "rgba(51, 51, 51, 0.06)",
                    },
                    tooltip: false
                });

                function gd(year, month, day) {
                    return new Date(year, month - 1, day).getTime();
                }
            });
        </script>

        <!-- worldmap -->
        <script type="text/javascript" src="js/maps/jquery-jvectormap-2.0.3.min.js"></script>
        <script type="text/javascript" src="js/maps/gdp-data.js"></script>
        <script type="text/javascript" src="js/maps/jquery-jvectormap-world-mill-en.js"></script>
        <script type="text/javascript" src="js/maps/jquery-jvectormap-us-aea-en.js"></script>
        <!-- pace -->
        <script src="js/pace/pace.min.js"></script>
        <script>
            $(function () {
                $('#world-map-gdp').vectorMap({
                    map: 'world_mill_en',
                    backgroundColor: 'transparent',
                    zoomOnScroll: false,
                    series: {
                        regions: [{
                            values: gdpData,
                            scale: ['#E6F2F0', '#149B7E'],
                            normalizeFunction: 'polynomial'
                        }]
                    },
                    onRegionTipShow: function (e, el, code) {
                        el.html(el.html() + ' (GDP - ' + gdpData[code] + ')');
                    }
                });
            });
        </script>
        <!-- skycons -->
        <script src="js/skycons/skycons.min.js"></script>
        <script>
            var icons = new Skycons({
                    "color": "#73879C"
                }),
                list = [
                    "clear-day", "clear-night", "partly-cloudy-day",
                    "partly-cloudy-night", "cloudy", "rain", "sleet", "snow", "wind",
                    "fog"
                ],
                i;

            for (i = list.length; i--;)
                icons.set(list[i], list[i]);

            icons.play();
        </script>

        <!-- dashbord linegraph -->
        <script>
            var doughnutData = [
                {
                    value: 30,
                    color: "#455C73"
                },
                {
                    value: 30,
                    color: "#9B59B6"
                },
                {
                    value: 60,
                    color: "#BDC3C7"
                },
                {
                    value: 100,
                    color: "#26B99A"
                },
                {
                    value: 120,
                    color: "#3498DB"
                }
        ];
            var myDoughnut = new Chart(document.getElementById("canvas1").getContext("2d")).Doughnut(doughnutData);
        </script>
        <!-- /dashbord linegraph -->
        <!-- datepicker -->
        <script type="text/javascript">
            $(document).ready(function () {

                var cb = function (start, end, label) {
                    console.log(start.toISOString(), end.toISOString(), label);
                    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                    //alert("Callback has fired: [" + start.format('MMMM D, YYYY') + " to " + end.format('MMMM D, YYYY') + ", label = " + label + "]");
                }

                var optionSet1 = {
                    startDate: moment().subtract(29, 'days'),
                    endDate: moment(),
                    minDate: '01/01/2012',
                    maxDate: '12/31/2015',
                    dateLimit: {
                        days: 60
                    },
                    showDropdowns: true,
                    showWeekNumbers: true,
                    timePicker: false,
                    timePickerIncrement: 1,
                    timePicker12Hour: true,
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    },
                    opens: 'left',
                    buttonClasses: ['btn btn-default'],
                    applyClass: 'btn-small btn-primary',
                    cancelClass: 'btn-small',
                    format: 'MM/DD/YYYY',
                    separator: ' to ',
                    locale: {
                        applyLabel: 'Submit',
                        cancelLabel: 'Clear',
                        fromLabel: 'From',
                        toLabel: 'To',
                        customRangeLabel: 'Custom',
                        daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                        monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                        firstDay: 1
                    }
                };
                $('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
                $('#reportrange').daterangepicker(optionSet1, cb);
                $('#reportrange').on('show.daterangepicker', function () {
                    console.log("show event fired");
                });
                $('#reportrange').on('hide.daterangepicker', function () {
                    console.log("hide event fired");
                });
                $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
                    console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
                });
                $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
                    console.log("cancel event fired");
                });
                $('#options1').click(function () {
                    $('#reportrange').data('daterangepicker').setOptions(optionSet1, cb);
                });
                $('#options2').click(function () {
                    $('#reportrange').data('daterangepicker').setOptions(optionSet2, cb);
                });
                $('#destroy').click(function () {
                    $('#reportrange').data('daterangepicker').remove();
                });
            });
        </script>
        <script>
            NProgress.done();
        </script>
        <!-- /datepicker -->
        <!-- /footer content -->
    </form>
</body>

</html>


