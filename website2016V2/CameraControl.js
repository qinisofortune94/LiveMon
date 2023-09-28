// JScript File

function ShowUserCover(CntrlID){
var i ;
var Command;
var myElementID;
//MyID1_MyImageHolder
myElementID="MyID"+CntrlID+"_MyImageHolder";
//for(i = 1; i <= 12; i++)
//if (document.getElementById(myElementID).value>0)
//{
   Command = "ReturnCameraImage.aspx?Camera=" + CntrlID +"&Resolution=" + document.getElementById('cmbResolution').value
   tmp = new Date()
   tmp = "&"+tmp.getTime()
   if (document.images.item(myElementID)!=null)
   {
        document.images.item(myElementID).src=Command+tmp
    }
    
//}
   setTimeout("ShowUserCover("+CntrlID+")", document.getElementById('Refresh').value);
}


