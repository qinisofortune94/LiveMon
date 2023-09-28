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
using System.IO.Compression;
using System.Web;

namespace website2016V2
{
    public partial class ReturnThumbnailImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] lastimage = null;
            Image retimage = default(Image);
            int CameraNo = 7;
            int SensorNo = 7;
            int IPDeviceNo = 7;
            int OtherDeviceNo = 7;
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
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects(((Session["SelectedSite"] == null) ? null : Session["SelectedSite"]));
            //GetServerObjects 'server1.GetAll()
            if ((MyCollection == null) == false)
            {
                object MyObject1 = null;
                CameraNo = Convert.ToInt32(Request.QueryString["Camera"]);
                SensorNo = Convert.ToInt32(Request.QueryString["Sensor"]);
                IPDeviceNo = Convert.ToInt32(Request.QueryString["IPDevice"]);
                OtherDeviceNo = Convert.ToInt32(Request.QueryString["OtherDevice"]);
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (Request.QueryString["Camera"] != "undefined")
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            if (MyCamera.ID == CameraNo)
                            {
                                switch (MyCamera.Status)
                                {
                                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                                        lastimage = MyCamera.ImageErrorByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                                        lastimage = MyCamera.ImageNoResponseByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    default:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                }
                            }
                        }
                    }
                    if (Request.QueryString["Sensor"] != "undefined")
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MyCamera = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            if (MyCamera.ID == SensorNo)
                            {
                                switch (MyCamera.Status)
                                {
                                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                                        lastimage = MyCamera.ImageErrorByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                                        lastimage = MyCamera.ImageNoResponseByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    default:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                }
                            }
                        }
                    }
                    if (Request.QueryString["IPDevice"] != "undefined")
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.IPDevicesDetails MyCamera = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                            if (MyCamera.ID == IPDeviceNo)
                            {
                                switch (MyCamera.Status)
                                {
                                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                                        lastimage = MyCamera.ImageErrorByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                                        lastimage = MyCamera.ImageNoResponseByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    default:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                }
                            }
                        }
                    }
                    if (Request.QueryString["OtherDevice"] != "undefined")
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.OtherDevicesDetails MyCamera = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                            if (MyCamera.ID == OtherDeviceNo)
                            {
                                switch (MyCamera.Status)
                                {
                                    case LiveMonitoring.IRemoteLib.StatusDef.alert:
                                        lastimage = MyCamera.ImageErrorByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.ok:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    case LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                                        lastimage = MyCamera.ImageNoResponseByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                    default:
                                        lastimage = MyCamera.ImageNormalByte;
                                        break; // TODO: might not be correct. Was : Exit For

                                        break;
                                }
                            }
                        }
                    }
                }
                //End If
                if ((lastimage == null) == false)
                {
                    MemoryStream ms = new MemoryStream(lastimage, 0, lastimage.Length);
                    retimage = Image.FromStream(ms);
                    if ((retimage.Size.Height * retimage.Size.Width) > 300)
                    {
                        int MyResPer = Convert.ToInt32(100 * (300 / (retimage.Size.Height * retimage.Size.Width)));
                        retimage = MyRem.ScaleByPercent(retimage, MyResPer);
                    }
                    Response.ContentType = "image/jpg";
                    Response.Cache.SetExpires(DateTime.Now.AddMinutes(2));
                    Response.Cache.SetCacheability(HttpCacheability.Public);
                    Response.Cache.VaryByParams["Sensor"] = true;
                    //Response.Cache.VaryByParams("image") = True

                    retimage.Save(Response.OutputStream, ImageFormat.Jpeg);
                }
            }


            //If IsNothing(lastimage) = False Then
            // Response.ContentType = "image/jpg"
            // lastimage.Save(Response.OutputStream, ImageFormat.Jpeg)
            //End If

        }
    }
}