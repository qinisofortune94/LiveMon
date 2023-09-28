// JScript File

    var srcIdcnt=0; //cnt for object
    var srcElementId; //Id of dragged object
    var destElementId; //Id of valid drop target object   
function ReceiveServerData(arg, context)
{
    var myArgs=arg.split(",");
    if (context=='Add')
    {
//CallServer('Added ,ID:'+srcIdcnt +',x:'+event.x +',y:'+ event.y+',image:'+ srcDiv.style.backgroundImage,'Add');
        var newdiv = document.createElement('div');
        newdiv.setAttribute('id','Image'+myArgs[1]);
        newdiv.style.width = 20;
        newdiv.style.height = 20;
        newdiv.style.left = myArgs[2];
        newdiv.style.top = myArgs[3];
        newdiv.style.position = "absolute";
        newdiv.style.border="solid 1px black";
        newdiv.style.backgroundImage = myArgs[4];
        newdiv.style.backgroundRepeat='no-repeat';
        newdiv.attachEvent('ondragstart',fnGetSource);   
        //newdiv.innerHTML = src ;
        document.getElementById("Bottom").appendChild(newdiv);
    }
    if (context=='Remove')
    {
        var srcDiv = document.getElementById(('Image'+myArgs[1])) ;
        document.getElementById("Bottom").removeChild(srcDiv);
    }
    if (context=='Change')
    {
        var srcDiv = document.getElementById('Image'+myArgs[1]) ;
        srcDiv.style.left = myArgs[2];
        srcDiv.style.top = myArgs[3];
    }
   document.getElementById('MyDiv').innerHTML = arg;
   //what 2 do
}
function correctPosition(oElement,oPos,oWhich) {
  while( oElement.offsetParent ) {
    oPos -= oElement['offset'+oWhich];
    oElement = oElement.offsetParent;
  }
  oPos += document.documentElement['scroll'+oWhich] ? document.documentElement['scroll'+oWhich] : document.body['scroll'+oWhich];
  return oPos;
}

function findPos(obj) {
	var curleft = curtop = 0;
	if (obj.offsetParent) {
		curleft = obj.offsetLeft
		curtop = obj.offsetTop
		while (obj = obj.offsetParent) {
			curleft += obj.offsetLeft
			curtop += obj.offsetTop
		}
	}
	return [curleft,curtop];
}

    function fnGetSource()
    {
        srcElementId = event.srcElement.id ;
        document.getElementById('MyDiv').innerHTML = srcElementId;
    }    
    function fnonmousedown()
    {
        srcElementId = event.srcElement.id ;
        document.getElementById('MyDiv').innerHTML = srcElementId;
    }    
    
    function fnGetDestination()
    {
        if (srcElementId!="" )
        {
        
            if (srcElementId.indexOf("Object")>=0 )
            {
                srcIdcnt=srcElementId.substring(srcElementId.indexOf("Object")+6,10);
                //srcIdcnt++;
                destElementId = event.srcElement.id;          
                var dest = document.getElementById(destElementId).innerHTML ;
                var src = document.getElementById(srcElementId).innerHTML ;
                var srcDiv = document.getElementById(srcElementId) ;
                var iebody=(document.compatMode && document.compatMode != "BackCompat")? document.documentElement : document.body

                var dsocleft=document.all? iebody.scrollLeft : pageXOffset
                //define universal dsoc top point
                var dsoctop=document.all? iebody.scrollTop : pageYOffset
                 CallServer('Added,'+srcIdcnt +','+(event.x+dsocleft) +','+ (event.y+dsoctop)+','+ srcDiv.style.backgroundImage+','+src,'Add');
             }
             else
             {
              //trash
                 srcIdcnt=srcElementId.substring(srcElementId.indexOf("Image")+5,10);
                  if (event.srcElement.id=="Trash" )
                 {
                  var truthBeTold = window.confirm("Click OK to Delete this object. Click Cancel to keep.");
                    if (truthBeTold) 
                    {
                         CallServer('Removed,'+srcIdcnt,'Remove');
                         //CallServer('Removed ,ID:'+srcIdcnt,'Remove');
                    } ;//else  window.alert("Bye for now!");
                 }
                 else
                  //Bottom
                 {
                     //listener(event);
                     //define universal dsoc left point
                     //define reference to the body object in IE
                    var iebody=(document.compatMode && document.compatMode != "BackCompat")? document.documentElement : document.body

                    var dsocleft=document.all? iebody.scrollLeft : pageXOffset
                    //define universal dsoc top point
                    var dsoctop=document.all? iebody.scrollTop : pageYOffset
                     CallServer('Changed ,'+srcIdcnt +','+(event.x+dsocleft) +','+ (event.y+dsoctop),'Change');
                 }
             }
        }
    }   
    function fnPlaceit()
    {
        if (srcElementId!="" )
        {
        
            if (srcElementId.indexOf("Object")>=0 )
            {
                srcIdcnt=srcElementId.substring(srcElementId.indexOf("Object")+6,10);
                //srcIdcnt++;
                destElementId = event.srcElement.id;          
                var dest = document.getElementById(destElementId).innerHTML ;
                var src = document.getElementById(srcElementId).innerHTML ;
                var srcDiv = document.getElementById(srcElementId) ;
                var iebody=(document.compatMode && document.compatMode != "BackCompat")? document.documentElement : document.body

                var dsocleft=document.all? iebody.scrollLeft : pageXOffset
                //define universal dsoc top point
                var dsoctop=document.all? iebody.scrollTop : pageYOffset
                 CallServer('Added,'+srcIdcnt +','+(event.x+dsocleft) +','+ (event.y+dsoctop)+','+ srcDiv.style.backgroundImage+','+src,'Add');
             }
             else
             {
              //trash
                 srcIdcnt=srcElementId.substring(srcElementId.indexOf("Image")+5,10);
                  if (event.srcElement.id=="Trash" )
                 {
                  var truthBeTold = window.confirm("Click OK to Delete this object. Click Cancel to keep.");
                    if (truthBeTold) 
                    {
                         CallServer('Removed,'+srcIdcnt,'Remove');
                         //CallServer('Removed ,ID:'+srcIdcnt,'Remove');
                    } ;//else  window.alert("Bye for now!");
                 }
                 else
                  //Bottom
                 {
                     //listener(event);
                     //define universal dsoc left point
                     //define reference to the body object in IE
                    var iebody=(document.compatMode && document.compatMode != "BackCompat")? document.documentElement : document.body

                    var dsocleft=document.all? iebody.scrollLeft : pageXOffset
                    //define universal dsoc top point
                    var dsoctop=document.all? iebody.scrollTop : pageYOffset
                     CallServer('Changed ,'+srcIdcnt +','+(event.x+dsocleft) +','+ (event.y+dsoctop),'Change');
                 }
             }
        }
    }  
    function cancelevent()
    {
        window.event.returnValue = false;
    }
    
function listener( e )
{
   var docX, docY;
   if( e )
   {
      if( typeof( e.pageX ) == 'number' )
      {
         docX = e.pageX;
         docY = e.pageY;
      }
      else
      {
         docX = e.clientX;
         docY = e.clientY;
      }
   }
   else
   {
      e = window.event;
      docX = e.clientX;
      docY = e.clientY;
      if( document.documentElement
        && ( document.documentElement.scrollTop
            || document.documentElement.scrollLeft ) )
      {
         docX += document.documentElement.scrollLeft;
         docY += document.documentElement.scrollTop;
      } 
      else if( document.body
         && ( document.body.scrollTop
             || document.body.scrollLeft ) )
      {
         docX += document.body.scrollLeft;
         docY += document.body.scrollTop;
      }
   }
}