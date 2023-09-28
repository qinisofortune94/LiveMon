<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddModels.aspx.cs" Inherits="website2016V2.AddModels" %>

<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="iglbar" Namespace="Infragistics.WebUI.UltraWebListbar" Assembly="Infragistics2.WebUI.UltraWebListbar.v7.1, Version=7.1.20071.1071, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">




    <script type="text/javascript">
        $(document).ready(function ($) {
            $('#accordion2').smartmenus({
                mainMenuSubOffsetX: -1,
                mainMenuSubOffsetY: 4,
                subMenusSubOffsetX: 6,
                subMenusSubOffsetY: -6
            });
            //$('#accordion2').dcAccordion({
            //    eventType: 'click',
            //    autoClose: false,
            //    saveState: true,
            //    disableLink: true,
            //    speed: 'fast',
            //    classActive: 'test',
            //    showCount: true
            //});
            $(".slider-arrow, .panel").animate({
                left: "+=800"
            }, 1000, function () {
                // Animation complete.
            });
            $(".slider-arrow, .panel").animate({
                left: "-=800"
            }, 1000, function () {
                // Animation complete.
            });
        });
        $(function () {
            $('.slider-arrow').click(function () {
                if ($(this).hasClass('show')) {
                    $(".slider-arrow, .panel").animate({
                        left: "+=800"
                    }, 900, function () {
                        // Animation complete.
                    });
                    $(this).html('&laquo;').removeClass('show').addClass('hide');
                }
                else {
                    $(".slider-arrow, .panel").animate({
                        left: "-=800"
                    }, 900, function () {
                        // Animation complete.
                    });
                    $(this).html('&raquo;').removeClass('hide').addClass('show');
                }
            });

        });
    </script>


<script type="text/javascript">
    function RefreshChildFrame(childFrame)
    {
        //document.frames[childFrame].RefreshMe();
    }
    function resize_iframe()
    {

        var height=window.innerWidth;//Firefox
        if (document.body.clientHeight)
        {
            height=document.body.clientHeight;//IE
        }
        //resize the iframe according to the size of the
        //window (all these should be on the same line)
        document.getElementById("main").style.height=parseInt(height-
        document.getElementById("main").offsetTop-8)+"px";
        document.getElementById("contents").style.height=parseInt(height-
        document.getElementById("contents").offsetTop-8)+"px";
    }
    function autoIframe(frameId){
        try{
            frame = document.getElementById(frameId);
            innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;
            objToResize = (frame.style) ? frame.style : frame;
            objToResize.height = innerDoc.body.scrollHeight + 240;
        }
        catch(err){
            window.status = err.message;
        }
    }


</script>

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
        <div class="panel panel-default" style="width: 1080px">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong>
                        <asp:Label ID="lblAdd" runat="server" Text="Add/Edit Models"></asp:Label>
                    </strong>
                    </a>
                </h4>
            </div>

             


            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
               

              <div class="row" >
                  <div class="col-md-4">


                    Models<igtab:UltraWebTab ID="UltraWebTab1" runat="server" BorderColor="#CCCCCC"
                            BorderStyle="Solid" BorderWidth="1px" Height="120px" ThreeDEffect="False" Width="850px" AutoPostBack="True" OnTabClick="UltraWebTab1_TabClick">
                            <DefaultTabStyle  Height="20px">
                            </DefaultTabStyle>
                            <Tabs>
                                <igtab:Tab Text="Add New" Tooltip="Adds a new graphic layout." Key="0">
                                    <ContentTemplate>

                                        <br />
                                        
                 <div class="row">
                    <div class="col-md-2">File</div>
                    <div class="col-md-4">
                            <asp:FileUpload ID="FileUpload" runat="server" />
                    </div>
                    <div class="col-md-2">Model Name</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtModelName" runat="server" PlaceHolder="Please enter Model Name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                </div>
                       <div class="row">
                  
                    <div class="col-md-2"></div>
                    <div class="col-md-4">
                           <asp:Button ID="cmdLoadNewModel" runat="server" Text="Load Model" Width="250px" Height="40px"  class="btn btn-primary form-control" BorderColor="#0099FF" OnClick="cmdLoadNewModel_Click" />
                         
                    </div>
                </div>



                                       
                                        
                                    </ContentTemplate>
                                </igtab:Tab>
                                <igtab:Tab Key="1" Text="Test">
                                </igtab:Tab>
                            </Tabs>
                            <RoundedImage FillStyle="LeftMergedWithCenter"
                                NormalImage="ig_tab_blueb1.gif" SelectedImage="ig_tab_blueb2.gif" />
                        </igtab:UltraWebTab>
                        <br />
                        <br />
                  
                  </div>
              </div>
             <div id=ModelEdit runat = server visible=false >

                  <table >
                <tr>
                    <td style="width: 3px; height: 20px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Available Objects" Width="112px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Tophandle" runat ="server" style="border: solid 1px black; height: 120px; width: 112px; overflow: auto;
                                            background-color: transparent;" >
<%--                                        <div id="Top" style="border: solid 1px black; height: 120px; width: 112px; overflow: auto;
                                            background-color: transparent; background-image: url(images/mycomputer.gif);
                                            background-repeat: no-repeat;" ondragstart='fnGetSource();' ondragover='cancelevent();'>
                                            <input type="hidden" id="DevID" value="D12">
                                        </div>
--%>                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Trash Bin" Width="112px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="Trash" style="border: solid 1px black; height: 120px; width: 112px; overflow: auto;
                                        background-color: transparent; background-repeat: no-repeat;" ondragenter='cancelevent();'
                                        ondragover='cancelevent();' ondrop='fnGetDestination();' onmousedown='fnPlaceit();'>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="height: 20px">
                        <%--background-image: url(images/iplogo.jpg);--%>
                        <div id="Bottom" style="height: 480px; width: 640px ;border: solid 1px black; background-color: white;" ondragenter='cancelevent();'
                            ondragover='cancelevent();' ondrop='fnGetDestination();'  ondblclick='fnPlaceit();' runat =server >
                        </div>
                        &nbsp;<br />
                        <div id='MyDiv'>
                        </div>
                        <asp:Button ID="Button3" runat="server" Text="Delete Model" OnClientClick="return confirm('Are you sure you want to delete this model?');" />
                        <asp:Button ID="cmdEditModel" runat="server" Text="Edit Model" OnClick="cmdEditModel_Click" />
                        <asp:FileUpload ID="FileUploadEdit" runat="server" /></td>
                    <td style="height: 20px">
                    </td>
                </tr>
            </table>
            </div>

             </div>



              <%--  <div class="row">
                     <div class="col-md-2">File</div>
                    <div class="col-md-4">
       
                         <asp:FileUpload ID="filImageNoResponse" runat="server" />    
                         <asp:Image ID="imgResponse" runat="server" Height="50px" />
                    </div>
                  </div>
                


                 <div class="row">
                    <div class="col-md-2">Model Name</div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtModelName" runat="server" PlaceHolder="Please enter Model Name" required="true" CssClass="form-control" Width="250px" Height="34px"></asp:TextBox>
                    </div>
                   
                    </div>

                <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnLoad" runat="server" Text="Load Model" Width="250px" Height="40px"   class="btn btn-success form-control" BorderColor="#0099FF" OnClick="btnCreate_Click" />
                    </div>
                    


                </div>--%>

              
                
            </div>
        
</asp:Content>
