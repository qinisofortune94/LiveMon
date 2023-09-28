using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Web.UI.WebControls;

namespace website2016V2
{
    partial class ExportData : System.Web.UI.Page
    {
        public void Load_Sensors()
        {
            //CameraMenu
            Collection MyCollection = new Collection();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;
            ddlSensors.Items.Clear();
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    System.Web.UI.WebControls.ListItem MyNewWebMenuItem = new System.Web.UI.WebControls.ListItem();
                    MyNewWebMenuItem.Text = MySensor.Caption;
                    MyNewWebMenuItem.Value = MySensor.ID.ToString();
                    ddlSensors.Items.Add(MyNewWebMenuItem);
                }
            }
        }

        public void Load_DataFormats()
        {
            ddlDataFormat.Items.Clear();

            ddlDataFormat.Items.Add("CSV");
            ddlDataFormat.Items.Add("XML");

        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (((string)Session["LoggedIn"] == "True"))
            {
                LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                Label user = this.Master.FindControl("lblUser") as Label;
                Label loginUser = this.Master.FindControl("lblUser2") as Label;
                Label LastLogin = this.Master.FindControl("LastLogin") as Label;
                loginUser.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                user.Text = ((MyUser.FirstName + (" " + MyUser.SurName)));
                LastLogin.Text = " LL:" + MyUser.LoginDT.ToString();

                //ok logged on level ?

                successMessage.Visible = false;
                warningMessage.Visible = false;
                errorMessage.Visible = false;

                string name = System.IO.Path.GetFileName(Request.ServerVariables["SCRIPT_NAME"]).Replace(".aspx", "");
                //LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
                LiveMonitoring.PageSecurityClass IPMonPageSecure = new LiveMonitoring.PageSecurityClass();
                int MyIPMonPageSecure = IPMonPageSecure.get_GetViewLevelByName(name);
                MyUser = (LiveMonitoring.IRemoteLib.UserDetails)Session["UserDetails"];
                if (MyIPMonPageSecure > MyUser.UserLevel)
                {
                    Response.Redirect("NotAuthorisedView.aspx");
                }
                if (IsPostBack == false)
                {
                    Load_Sensors();
                    Load_DataFormats();
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
            
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            string strData = null;
            //LiveMonitoring.IRemoteLib.DataHistory MyDatahistory = new LiveMonitoring.IRemoteLib.DataHistory();
            Collection MyData = default(Collection);
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();

            if (Information.IsDate(txtStartDate.Text) == false)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply correct start date.";

                return;

            }

            if (Information.IsDate(txtEndDate.Text) == false)
            {
                warningMessage.Visible = true;
                lblWarning.Text = "Please supply correct end date.";

                return;
            }

            MyData = MyRem.LiveMonServer.GetSensorHistory(Convert.ToInt32(ddlSensors.SelectedValue), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));

            string selectedFormat = ddlDataFormat.SelectedValue;
            LiveMonitoring.IRemoteLib.SensorDetails MySensorDet = ReturnSensor(Convert.ToInt32(ddlSensors.SelectedValue));

            switch (selectedFormat)
            {
                case "CSV":
                    strData = "sep=," + Constants.vbCrLf + "ID,Sensor,Field,DT,Status,Value,OtherData" + Constants.vbCrLf;
                    if ((MyData == null) == false)
                    {
                        //Dim MySensorDet As LiveMonitoring.IRemoteLib.SensorDetails = ReturnSensor(CInt(cmbSensors.SelectedValue))
                        if ((MySensorDet == null) == false)
                        {
                            foreach (LiveMonitoring.IRemoteLib.DataHistory MyDatahistory in MyData)
                            {
                                strData += MyDatahistory.ID.ToString() + ",";
                                strData += MySensorDet.Caption + ",";
                                if (MySensorDet.Fields.Count >= MyDatahistory.Field)
                                {
                                    strData += ((LiveMonitoring.IRemoteLib.SensorFieldsDef)MySensorDet.Fields[(MyDatahistory.Field).ToString()]).FieldName;
                                }
                                else
                                {
                                    strData += (MyDatahistory.Field).ToString();
                                }
                                strData += MyDatahistory.Field.ToString() + ",";
                                strData += MyDatahistory.DT.ToString() + ",";
                                strData += MyDatahistory.Status.ToString() + ",";
                                strData += MyDatahistory.Value.ToString() + ",";
                                if ((MyDatahistory.OtherData == null))
                                {
                                    strData += "" + Constants.vbCrLf;
                                }
                                else
                                {
                                    strData += MyDatahistory.OtherData.ToString() + Constants.vbCrLf;
                                }

                            }
                        }

                    }
                    byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(strData);
                    Response.Clear();
                    Response.AddHeader("Content-Type", "application/Excel");
                    Response.AddHeader("Content-Disposition", "inline;filename=IPMonExp_" + ddlSensors.SelectedItem.Text + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".csv");
                    Response.BinaryWrite(data);
                    Response.End();
                    break;
                case "XML":
                    Response.Clear();
                    Response.AddHeader("Content-Type", "text/xml");
                    Response.AddHeader("Content-Disposition", "attachment;filename=IPMonExp_" + ddlSensors.SelectedItem.Text + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".xml");
                    exportToXMLHDD(MyData, MySensorDet);
                    //Response.Write(Ms.ToString)
                    Response.End();
                    break;
            }
            // System.Diagnostics.Process.Start("notepad.exe", "C:\IPMonExp_" + cmbSensors.SelectedItem.Text & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ".xml")


        }


        protected void exportToXMLHDD(Collection MyData, LiveMonitoring.IRemoteLib.SensorDetails MySensorDet)
        {
            // Dim Myretstream As New MemoryStream
            // Create XmlWriterSettings.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            //"C:\IPMonExp_" + cmbSensors.SelectedItem.Text & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & ".xml"
            // Create XmlWriter.
            using (XmlWriter writer = XmlWriter.Create(Response.OutputStream, settings))
            {
                //Bigin writing.
                writer.WriteStartDocument();
                writer.WriteStartElement("Sensor");
                //Root          
                writer.WriteElementString("ID", MySensorDet.ID.ToString());
                writer.WriteElementString("Caption", MySensorDet.Caption.ToString());
                //Loop over sensors in array.
                //ID,Sensor,Field,DT,Status,Value,OtherData
                foreach (LiveMonitoring.IRemoteLib.DataHistory MyDatahistory in MyData)
                {
                    writer.WriteElementString("ID", (MyDatahistory.ID).ToString());
                    writer.WriteElementString("Field", (MyDatahistory.Field).ToString());
                    writer.WriteElementString("DT", MyDatahistory.DT.ToString());
                    writer.WriteElementString("Status", (MyDatahistory.Status).ToString());
                    writer.WriteElementString("Value", MyDatahistory.Value.ToString());
                    if ((MyDatahistory.OtherData == null) == false)
                    {
                        writer.WriteElementString("OtherValue", MyDatahistory.OtherData.ToString());
                    }
                    else
                    {
                        writer.WriteElementString("OtherValue", "");
                    }

                    // writer.WriteEndElement()
                }

                //For Each sensor In sensors
                //    writer.WriteStartElement("Sensor")
                //    writer.WriteElementString("ID", sensor.ID.ToString)
                //    writer.WriteElementString("Caption", MySensorDet.Caption)

                //    If MySensorDet.Fields.Count >= sensor.Field Then
                //        writer.WriteElementString("Field", CType(MySensorDet.Fields((sensor.Field).ToString), LiveMonitoring.IRemoteLib.SensorFieldsDef).FieldName)
                //    Else
                //        writer.WriteElementString("Field", (sensor.Field).ToString)
                //    End If
                //Next
                // End document.
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            //Return Myretstream
        }


        public class Sensor
        {
            public Sensor(int id, string firstName, string lastName, int salary)
            {
                // Set fields.
                this._id = id;
                this._firstName = firstName;
                this._lastName = lastName;
                this._salary = salary;
            }

            // Storage of employee data.
            public string _firstName;
            public int _id;
            public string _lastName;
            public int _salary;
        }


        private LiveMonitoring.IRemoteLib.SensorDetails ReturnSensor(int SensorID)
        {
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            Collection MyCollection = new Collection();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;
            int MyCnt = 0;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                {
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                    if (SensorID == MySensor.ID)
                    {
                        return MySensor;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                    else
                    {
                        MyCnt += 1;
                    }
                }
            }
            return null;
        }
        public ExportData()
        {
            Load += Page_Load;
        }
    }
}