<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SensorDetails.aspx.cs" Inherits="website2016V2.SensorDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <%--<meta http-equiv="refresh" content="20" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

   
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
        h5,p {
  font-style: oblique;
    font-weight:bold;
    
    
    }

     </style>

    <script type="text/javascript">
        function scrollTo()
        {
            return;
        }
    </script> 

    <script src="Scripts2/highcharts.js" type="text/javascript"></script>
    <script src="Scripts2/drilldown.js" type="text/javascript"></script>
    <script src="Scripts2/exporting.js"></script>
    <script src="Scripts2/highcharts-more.js"></script>


    <div class="">
                        <div class="page-title">
                            <div class="title_left">
                                <h3>Sensor Dashboard Display</h3>
                                <asp:Button ID="editTest" CssClass="btn btn-info" runat="server" Text="Edit Sensor/Test Sensor Readings" ToolTip="Edit Sensor/Test Sensor readings" OnClick="editTest_Click"/>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <asp:Panel ID="sensorDetails" runat="server">
                        <div class="row">
                            <div class="col-md-3 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Settings</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor ID <span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtSensorID" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor Name<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtSensorName" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor Group<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtSensorGroup" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Sensor Scan Rate<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtScanRate" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Maximum Value<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtMaxValue" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />

                                            <div class="form-group">
                                                <div class="col-md-12 col-sm-6 col-xs-6">
                                                 <label class="control-label" for="first-name">Minimum Value<span class="required"></span>
                                                    </label>
                                                    <asp:TextBox ID="txtMinValue" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div><br />
                                            </div><br /><br />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-9 col-sm-12 col-xs-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Graph</h2>
                                        <asp:DropDownList ID="dropRange" runat="server" CssClass="marg" Width="168px" OnSelectedIndexChanged="dropRange_SelectedIndexChanged" AutoPostBack="true">
                                             <asp:ListItem Value="0">Last 30 Mins</asp:ListItem>
                                             <asp:ListItem Value="1">Last Hour</asp:ListItem>
                                             <asp:ListItem Value="2">Last 5 Hours</asp:ListItem>
                                        </asp:DropDownList>
                                        <ul class="nav navbar-right panel_toolbox"> 
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="col-md-12">
                                              <asp:Literal id="chrtMyChart" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="x_panel">
                                    <div class="x_title">
                                        <h2>Sensor Field</h2>
                                        <ul class="nav navbar-right panel_toolbox">
                                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                            </li>
                                        </ul>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="x_content">
                                        <div class="form-group">
                                            <div class="col-md-3 col-sm-6 col-xs-6">
                                                <label class="control-label" for="first-name">Type <span class="required"></span>
                                                </label>
                                                <asp:DropDownList ID="cmbType2" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div><br />
                                        </div><br /><br />

                                        <div class="form-group">
                                            <div class="col-md-12 col-sm-6 col-xs-6">
                                                <asp:GridView ID="cmbFields2" runat="server" CssClass="gvdatatable table table-striped table-bordered"
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnPageIndexChanging="cmbFields_PageIndexChanging" OnDataBinding="cmbFields_DataBinding"
                                                    AllowPaging="True" AutoGenerateEditButton="True" PageSize="5" OnRowCancelingEdit="cmbFields_RowCancelingEdit" OnRowUpdating="cmbFields_RowUpdating" OnRowEditing="cmbFields_RowEditing" 
                                                    ViewStateMode="Enabled" OnSelectedIndexChanged="cmbType_SelectedIndexChanged">
                                                   
                                                </asp:GridView>
                                            </div><br />
                                        </div><br />
                                    </div>
                                </div>
                            </div>
                        </div>
                        </asp:Panel>
                    </div>
                    <%--<asp:HiddenField ID="txtID2" runat="server" />--%>
                    <asp:TextBox ID="alert" Visible="false" runat="server" Height="97px" Width="100%"></asp:TextBox>
</asp:Content>