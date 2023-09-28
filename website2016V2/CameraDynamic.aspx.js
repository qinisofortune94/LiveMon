// JScript File
    var srcIdcnt=0; //cnt for object
    var srcElementId; //Id of dragged object
    var destElementId; //Id of valid drop target object   


function ShowCover(){
var i ;
var Command;
//for(i = 1; i <= 12; i++)
try 
   {
   //fnRefreshInputs();
    for (i = 1; i < 22; i++)
    {
        if (document.getElementById('Hidden'+i).value>0)
        {
            var myElementID;
            var myZoomID;
            myElementID="MyID"+i+"_MyImageHolder";
            myZoomID="MyID"+i+"_ZoomLevel";
           Command = "ReturnCameraImage.aspx?Camera=" + document.getElementById('Hidden'+i).value +"&Resolution=" + document.getElementById(myZoomID).value;
           tmp = new Date();
           tmp = "&"+tmp.getTime();
           //bgsound1.src = "ReturnCameraAudio.aspx?Camera=" + document.getElementById('Hidden'+i).value+tmp ;
           //player.src = "ReturnCameraAudio.aspx?Camera=" + document.getElementById('Hidden'+i).value+tmp ;

           if (document.getElementById(myElementID) != null)
           {
           //     document.images.item("Image"+i).src=Command+tmp;
           // }
           //if (navigator.appName == "Microsoft Internet Explorer") {
           //    if (document.images.item(myElementID) != null) {
           //        document.images.item(myElementID).src = Command + tmp;
           //    }
           //} else 
           //{
               //if FF then use the document.getElementById() method.
                document.getElementById(myElementID).src = Command + tmp;
           }

           //if (document.images.item(myElementID)!=null)
           //{
           //     document.images.item(myElementID).src=Command+tmp;
           // }
            //CallServer('Update','Update'); for each camera to update its contents
        }
    }
   setTimeout("ShowCover()", document.getElementById('Refresh').value);
   }
catch (e)
      {
      // Log error
      //Log.Write(e.description, apgSeverityError, e.number);
      }

}

function fnRefreshInputs(){
    var i ;
    var srcDiv = document.getElementById('Inputs') ;
    try
    {
        if (srcDiv)
            {
                fnCallServer(srcDiv.value);
//               var myArgs=srcDiv.value.split(",");
//                for (i = 0; i<myArgs.length ; i++)
//                {
//                        //setup the timer
//                        CallServer(myArgs[i],'Update');
//                }

            }
            setTimeout( "fnRefreshInputs()", 25000 );
    }
    catch(e)
    {

    }
    
}

function fnCallServer(oToRefresh){
   window.setTimeout("CallServer('" + oToRefresh + "','Update')", document.getElementById('Refresh').value);
}
function fnOnLoad(){
    var i ;
    var srcDiv = document.getElementById('Inputs') ;
    if (srcDiv)
    {
    fnCallServer(srcDiv.value);
//    var myArgs=srcDiv.value.split(",");
//    for (i = 0; i<myArgs.length ; i++)
//    {
//            //setup the timer
//            fnCallServer(myArgs[i]);
//    }

    }

}

function ReceiveServerData(arg, context)
{
    var myArgs=arg.split(",");
    try
    {
        for (ivar = 0; ivar<myArgs.length ; ivar=ivar+2)
        {

            var srcDiv = document.getElementById(myArgs[ivar]) ;
            if (srcDiv)
            {
            srcDiv.innerHTML =myArgs[ivar+1];
            }
        }
     }
    catch(e)
    {
      Log.Write(e.description, apgSeverityError, e.number);
    }


//    if (context=='Update')
//    {
//        var srcDiv = document.getElementById('Bottom') ;
//        srcDiv.innerHTML=arg;
        //document.getElementById("Bottom").removeChild(srcDiv);
//    }
}
function ReceiveOlderServerData(arg, context)
{
    var myArgs=arg.split(",");
    var srcDiv = document.getElementById(myArgs[0]) ;
    if (srcDiv)
    {
    srcDiv.innerHTML =myArgs[1];
    }
//    if (context=='Update')
//    {
//        var srcDiv = document.getElementById('Bottom') ;
//        srcDiv.innerHTML=arg;
        //document.getElementById("Bottom").removeChild(srcDiv);
//    }
}

    function fnGetSource()
    {
        srcElementId = event.srcElement.id ;
    }    
    
    function fnGetDestination()
    {

    }    
    function cancelevent()
    {
        window.event.returnValue = false;
    }


function refresh()
{
   // CallServer('Update','Update');
   ShowCover();
   setTimeout( "fnRefreshInputs()", 15000 );
}