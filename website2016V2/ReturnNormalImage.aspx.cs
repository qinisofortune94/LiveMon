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
namespace website2016V2
{
    public partial class ReturnNormalImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] lastimage = null;
            Image retimage = default(Image);
            int SensorNo = 7;
            int DeviceNo = 7;
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
            object MyObject1 = null;
            SensorNo = Convert.ToInt32(Request.QueryString["Sensor"]);
            DeviceNo = Convert.ToInt32(Request.QueryString["Device"]);
            if ((MyCollection == null) == false)
            {
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (Request.QueryString["Sensor"] == "undefined" | Request.QueryString["Sensor"] == null)
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                        {
                            LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                            if (MyCamera.ID == DeviceNo)
                            {
                                lastimage = MyCamera.ImageNormalByte;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                    if (Request.QueryString["Sensor"] != "undefined" & Request.QueryString["Sensor"] != null)
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            if (MySensor.ID == SensorNo)
                            {
                                lastimage = MySensor.ImageNormalByte;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                    if (Request.QueryString["Sensor"] == "undefined" | Request.QueryString["Sensor"] == null)
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevices = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                            if (MyIPDevices.ID == DeviceNo)
                            {
                                lastimage = MyIPDevices.ImageNormalByte;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }
                    if (Request.QueryString["Sensor"] == "undefined" | Request.QueryString["Sensor"] == null)
                    {
                        if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                        {
                            LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevice = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                            if (MyOtherDevice.ID == DeviceNo)
                            {
                                lastimage = MyOtherDevice.ImageNormalByte;
                                break; // TODO: might not be correct. Was : Exit For
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
                retimage.Save(Response.OutputStream, ImageFormat.Jpeg);
            }

            //If IsNothing(lastimage) = False Then
            // Response.ContentType = "image/jpg"
            // lastimage.Save(Response.OutputStream, ImageFormat.Jpeg)
            //End If

        }
    }
}