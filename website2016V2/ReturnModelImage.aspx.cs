using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;

using System.Runtime.Remoting.Channels;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Web;

namespace website2016V2
{
    public partial class ReturnModelImage : System.Web.UI.Page
    {
        LiveMonitoring.IRemoteLib.ModelDetails MyObject1 = new LiveMonitoring.IRemoteLib.ModelDetails();
        protected void Page_Load(object sender, EventArgs e)
        {
            Image lastimage = default(Image);
            int ModelNo = 0;
            Collection MyCollection = new Collection();
            if ((Request.UserAgent.ToLower().Contains("konqueror") == false))
            {
                if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("gzip")))
                {
                    Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress, true);
                    Response.AppendHeader("Content-encoding", "gzip");

                }
                else
                {
                    if ((!string.IsNullOrEmpty(Request.Headers["Accept-encoding"]) & Request.Headers["Accept-encoding"].Contains("deflate")))
                    {
                        Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress, true);
                        Response.AppendHeader("Content-encoding", "deflate");
                    }
                }
            }
            //If Request.QueryString("Camera") <> "undefined" Then
             
            ModelNo = Convert.ToInt32(Request.QueryString["Model"]);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.LiveMonServer.GetSpecificModel(ModelNo);
          //  LiveMonitoring.IRemoteLib.ModelDetails MyObject1 = new LiveMonitoring.IRemoteLib.ModelDetails();
            foreach (LiveMonitoring.IRemoteLib.ModelDetails MyObject1 in MyCollection)
            {
                lastimage = MyObject1.ModelImage;
               // break; // TODO: might not be correct. Was : Exit For
                if ((MyObject1.ModelImageByte == null) == false)
                {
                    MemoryStream ms = new MemoryStream(MyObject1.ModelImageByte, 0, MyObject1.ModelImageByte.Length);
                    lastimage = Image.FromStream(ms);
                    if ((lastimage.Size.Height * lastimage.Size.Width) > 220000)
                    {
                        int MyResPer = Convert.ToInt32(100 * (220000 / (lastimage.Size.Height * lastimage.Size.Width)));
                        lastimage = MyRem.ScaleByPercent(lastimage, MyResPer);
                    }

                    Response.ContentType = "image/jpg";
                    Response.Cache.SetExpires(DateTime.Now.AddMinutes(5));
                    Response.Cache.SetCacheability(HttpCacheability.Public);
                    Response.Cache.VaryByParams["Model"] = true;

                    lastimage.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
            }
            //End If
         
           

            //If IsNothing(lastimage) = False Then
            // Response.ContentType = "image/jpg"
            // Dim stmMemory As New MemoryStream

            // lastimage.Save(stmMemory, System.Drawing.Imaging.ImageFormat.Png)
            // stmMemory.WriteTo(Response.OutputStream)

            // 'lastimage.Save(Response.OutputStream, ImageFormat.Jpeg)
            //End If
        }
    }
}