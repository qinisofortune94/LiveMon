using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace website2016V2
{
    public partial class CaptureImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] myImageBytes = null;
            Image lastimage = default(Image);
            int CameraNo = 0;

            if (Request.QueryString["Camera"] != "undefined")
            {
                CameraNo = Convert.ToInt32(Request.QueryString["Camera"]);
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                myImageBytes = MyRem.LiveMonServer.GetCameraImage(CameraNo);
                if ((myImageBytes == null) == false)
                {
                    MemoryStream ms = new MemoryStream(myImageBytes, 0, myImageBytes.Length);
                    lastimage = Image.FromStream(ms);
                    //lastimage = ScaleByPercent(lastimage, resolution)
                    Response.ContentType = "application/octet-stream";
                    Response.ContentType = "application/pdf";
                    string FileName = "Camera" + CameraNo.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".jpg";
                    Response.AddHeader("Content-disposition", "attachment; filename=" + FileName);
                    lastimage.Save(Response.OutputStream, ImageFormat.Jpeg);
                    //Response.OutputStream.Write(ms, 0, ms.Length)
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();

                    //lastimage.Save(Response.OutputStream, ImageFormat.Jpeg)
                }
            }
        }
    }
}