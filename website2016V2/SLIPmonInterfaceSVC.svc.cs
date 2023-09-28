using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Serialization;

namespace website2016V2
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SLIPmonInterfaceSVC
    {
        private LiveMonitoring.IRemoteLib ServerInterface;
        private static Collection MySensorsCol = new Collection();
        //(GetConSetting)
        private static LiveMonitoring.DataAccess MyDataAccess;
        public SLIPmonInterfaceSVC()
        {

            try
            {
                string appSettings = ConfigurationManager.AppSettings.Get("Remote.Settings");
                ServerInterface = (LiveMonitoring.IRemoteLib)Activator.GetObject(typeof(LiveMonitoring.IRemoteLib), appSettings);
                //Dim MyFunc As New LocalDataFunctions
                try
                {
                    string mystring = ConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
                    //Dim mystring As String = ConfigurationManager.ConnectionStrings.Item("IPMonConnectionString")
                    MyDataAccess = new LiveMonitoring.DataAccess(mystring);

                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("SlipmonInterface ServerInterface" + ex.Message);
            }
        }

        [OperationContract()]
        public string GetConfigSetting(string SettingName)
        {
            try
            {
                string item = null;
                item = MyDataAccess.GetAppSetting(SettingName);
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public System.Collections.Generic.List<string> GetRemoteAlertHistory(System.DateTime StartDate, System.DateTime EndDate)
        {
            try
            {
                //Dim items As New System.Collections.Generic.List(Of String) '= New List(Of Object)()

                System.Collections.Generic.List<string> RetCollection = new System.Collections.Generic.List<string>();
                Collection MyCollection = ServerInterface.GetAllAlertHistoryByDate(StartDate, EndDate);
                int MyMaxMessages = 0;
                if ((MyCollection == null) == false)
                {
                    foreach (LiveMonitoring.IRemoteLib.AlertHistory myHist in MyCollection)
                    {
                        try
                        {
                            string MyString = "";
                            MyString += myHist.ID.ToString() + "|";
                            MyString += myHist.AlertType.ToString() + "|";
                            MyString += myHist.Dest.ToString() + "|";
                            MyString += myHist.AlertMessage.ToString() + "|";
                            MyString += myHist.Sent.ToString();
                            if (MyMaxMessages + MyString.Length < 44535)
                            {
                                RetCollection.Add(MyString);
                                MyMaxMessages += MyString.Length;
                            }
                            else
                            {
                                //too big messages
                                break; // TODO: might not be correct. Was : Exit For
                            }


                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                return RetCollection;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Collection GetRemoteSensors()
        {
            try
            {
                Collection RetCollection = new Collection();
                Collection MyCollection = ServerInterface.GetSensors();
                if ((MyCollection == null) == false)
                {
                    foreach (string mystring in MyCollection)
                    {
                        if (MySensorsCol.Contains(mystring))
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MySensorsCol[mystring];
                            if ((MySensor == null) == false)
                            {
                                RetCollection.Add(MySensor, MySensor.ID.ToString());
                            }
                        }
                        else
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = ServerInterface.GetSpecificSensor(Convert.ToInt32(mystring));
                            if ((MySensor == null) == false)
                            {
                                RetCollection.Add(MySensor, MySensor.ID.ToString());
                                MySensorsCol.Add(MySensor, MySensor.ID.ToString());
                            }
                        }
                    }
                }

                return RetCollection;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Collection GetSpecificRemoteSensors(string Sensors)
        {
            try
            {
                Collection RetCollection = new Collection();
                //Dim MyCollection As Collection = ServerInterface.GetSensors()
                //If IsNothing(MyCollection) = False Then
                string[] MySensors = Sensors.Split('|');
                foreach (string mystring in MySensors)
                {
                    if (!string.IsNullOrEmpty(mystring))
                    {
                        try
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = ServerInterface.GetSpecificSensor(Convert.ToInt32(mystring));
                            if ((MySensor == null) == false)
                            {
                                try
                                {
                                    RetCollection.Add(MySensor, MySensor.ID.ToString());

                                }
                                catch (Exception ex)
                                {
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                //End If

                return RetCollection;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [OperationContract()]
        public void LogIt(string LogType, string LogEntry)
        {
            // Add your operation implementation here
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyRem.WriteLog(LogType, LogEntry);

            }
            catch (Exception ex)
            {
            }
        }

        [OperationContract()]
        public string GetSetting(string KeyName)
        {
            // Add your operation implementation here
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            return MyRem.GetAppSetting(KeyName);
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetAll()
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> MyList = new System.Collections.Generic.List<string>();
            try
            {
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.get_GetServerObjects();
                //server1.GetAll()
                //Dim MyObject
                foreach (object MyObject in MyCollection)
                {
                    MyList.Add((string)MyObject);
                }

            }
            catch (Exception ex)
            {
            }
            return MyList;
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetSites()
        {
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                Collection MyCollection = new Collection();
                ArrayList MySiteDetails = new ArrayList();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.get_GetServerObjects();
                //server1.GetAll()
                object MyObject1 = null;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SiteDetails)
                    {
                        LiveMonitoring.IRemoteLib.SiteDetails MySiteDetail = (LiveMonitoring.IRemoteLib.SiteDetails)MyObject1;
                        items.Add(MySiteDetail.SiteName + "|" + MySiteDetail.ID.ToString() + "|" + MySiteDetail.PanelNo.ToString() + "|" + MySiteDetail.PanelPos.ToString() + "|" + MySiteDetail.Screen.ToString() + "|" + MySiteDetail.DisplayType.ToString() + "|" + MySiteDetail.DisplayImage.ToString() + "|" + MySiteDetail.DisplayHeight.ToString() + "|" + MySiteDetail.DisplayWidth.ToString() + "|" + MySiteDetail.ExtraData.ToString() + "|" + MySiteDetail.ExtraValue.ToString());
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetDisplays()
        {
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                Collection MyCollection = new Collection();
                ArrayList MySiteDetails = new ArrayList();
                //DisplayName MyGroup = default(DisplayName);
                IPMnLinqDataContext sites = new IPMnLinqDataContext();
                var DisplayGroupsQuery = from DispGrp in sites.DisplayNames
                                         orderby DispGrp.DefaultOrderByColumn, DispGrp.ID
                                         select DispGrp;
                foreach (DisplayName MyGroup in DisplayGroupsQuery)
                {
                    items.Add(MyGroup.DisplayName1 + "|" + MyGroup.ID.ToString() + "|" + MyGroup.DisplayType.ToString() + "|" + MyGroup.DefaultOrderByColumn.ToString() + "|" + MyGroup.ExtraData.ToString() + "|" + MyGroup.ExtraValue.ToString());
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [OperationContract()]
        public void DelDisplayGroup(int ID)
        {
            try
            {
                string MyString = "";
                Collection MyCollection = new Collection();
                ArrayList MySiteDetails = new ArrayList();

                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@ID";
                MySQLParam[0].Value = ID;

                MyDataAccess.ExecCmdNonQueryParams("displayGroup_Delete", MySQLParam);



            }
            catch (Exception ex)
            {
            }
        }

        [OperationContract()]
        public string GetDisplayGroup(int ID)
        {
            try
            {
                string MyString = "";
                Collection MyCollection = new Collection();
                ArrayList MySiteDetails = new ArrayList();
                //Dim MyGroup As DisplayGroup
                //Dim sites As IPMonLinqDataContext = New IPMonLinqDataContext()

                //Dim DisplayGroupsQuery = From DispGrp In sites.DisplayGroups _
                //                         Where DispGrp.DisplayID = DisplayID _
                //                         Order By DispGrp.Screen, DispGrp.PanelNo _
                //                        Select DispGrp
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@ID";
                MySQLParam[0].Value = ID;

                System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryParams("displayGroup_select_specific", MySQLParam);
                //displayGroups_select_specific
                if ((Mysqlreader == null) == false)
                {
                    while (Mysqlreader.Read())
                    {
                        //If IsDBNull(Mysqlreader.Item("StoreID")) = False And IsDBNull(Mysqlreader.Item("FileName")) = False And IsDBNull(Mysqlreader.Item("FileData")) = False Then
                        //End If

                        try
                        {
                            MyString = Mysqlreader["GroupName"].ToString() + "|" + Mysqlreader["ID"].ToString() + "|" + Mysqlreader["PanelNo"].ToString() + "|" + Mysqlreader["PanelPos"].ToString() + "|";
                            MyString += Mysqlreader["Screen"].ToString() + "|";
                            MyString += Mysqlreader["DisplayType"].ToString() + "|";
                            if (Information.IsDBNull(Mysqlreader["DisplayImage"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DisplayImage"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DisplayHeight"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DisplayHeight"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DisplayWidth"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DisplayWidth"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0);
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue2"] ?? 0);
                            }
                            else
                            {
                                MyString += "|";
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }

                //For Each MyGroup In DisplayGroupsQuery
                //    Try
                //        items.Add(MyGroup.GroupName + "|" + MyGroup.ID.ToString + "|" + MyGroup.PanelNo.ToString + "|" + MyGroup.PanelPos.ToString + "|" + MyGroup.Screen.ToString + "|" + MyGroup.DisplayType.ToString + "|" + If(MyGroup.DisplayImage, "") + "|" + CStr(If(MyGroup.DisplayHeight, 0)) + "|" + CStr(If(MyGroup.DisplayWidth, 0)) + "|" + CStr(If(MyGroup.ExtraData1, "")) + "|" + CStr(If(MyGroup.ExtraValue1, 0)))

                //    Catch ex As Exception

                //    End Try
                //Next
                return MyString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetDisplayGroups(int DisplayID)
        {
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                Collection MyCollection = new Collection();
                ArrayList MySiteDetails = new ArrayList();
                //Dim MyGroup As DisplayGroup
                //Dim sites As IPMonLinqDataContext = New IPMonLinqDataContext()

                //Dim DisplayGroupsQuery = From DispGrp In sites.DisplayGroups _
                //                         Where DispGrp.DisplayID = DisplayID _
                //                         Order By DispGrp.Screen, DispGrp.PanelNo _
                //                        Select DispGrp
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@DisplayID";
                MySQLParam[0].Value = DisplayID;

                System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryParams("displayGroups_select_specific", MySQLParam);
                //displayGroups_select_specific
                if ((Mysqlreader == null) == false)
                {
                    while (Mysqlreader.Read())
                    {
                        //If IsDBNull(Mysqlreader.Item("StoreID")) = False And IsDBNull(Mysqlreader.Item("FileName")) = False And IsDBNull(Mysqlreader.Item("FileData")) = False Then
                        //End If
                        string MyString = "";
                        try
                        {
                            MyString = Mysqlreader["GroupName"].ToString() + "|" + Mysqlreader["ID"].ToString() + "|" + Mysqlreader["PanelNo"].ToString() + "|" + Mysqlreader["PanelPos"].ToString() + "|";
                            MyString += Mysqlreader["Screen"].ToString() + "|";
                            MyString += Mysqlreader["DisplayType"].ToString() + "|";
                            if (Information.IsDBNull(Mysqlreader["DisplayImage"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DisplayImage"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DisplayHeight"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DisplayHeight"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DisplayWidth"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DisplayWidth"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue2"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        items.Add(MyString);
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [OperationContract()]
        public List<string> ReturnDisplayFiles()
        {
            //As System.Collections.Generic.List(Of String)
            List<string> mretlist = new List<string>();
            try
            {
                var DI = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/DisplayDocuments/"));
                if ((DI == null) == false)
                {
                    System.IO.FileInfo[] aryFi = DI.GetFiles();
                    foreach (System.IO.FileInfo fi in aryFi)
                    {
                        mretlist.Add(fi.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return mretlist;
        }
        [OperationContract()]
        public List<string> DeleteDisplayFiles(string FileName)
        {
            //As System.Collections.Generic.List(Of String)

            try
            {
                var FS = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/DisplayDocuments/") + FileName);
                if ((FS == null) == false)
                {
                    FS.Delete();
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("denied"))
                {
                    try
                    {
                        System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/DisplayDocuments/") + FileName);

                    }
                    catch (Exception ex1)
                    {
                        return null;
                    }
                }
            }
            return ReturnDisplayFiles();
        }
        [OperationContract()]
        //As System.Collections.Generic.List(Of String)
        public void UploadDisplayFileBin(string _SerialFile, string FileName)
        {
            try
            {
                byte[] File = Convert.FromBase64String(_SerialFile);
                MemoryStream MS = new MemoryStream(File);
                var FS = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/DisplayDocuments/") + FileName, FileMode.Create);
                MS.WriteTo(FS);
                MS.Close();
                FS.Close();
                FS.Dispose();

            }
            catch (Exception ex)
            {
            }
        }

        [OperationContract()]
        public bool AddDisplay(string DisplayName, int DisplayType)
        {
            bool Myert = false;
            bool functionReturnValue = false;
            //As System.Collections.Generic.List(Of String)
            try
            {
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[5];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@DisplayName";
                MySQLParam[0].Value = DisplayName;
                MySQLParam[1] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[1].ParameterName = "@ExtraData";
                MySQLParam[1].Value = "";
                MySQLParam[2] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[2].ParameterName = "@ExtraValue";
                MySQLParam[2].Value = 0;
                MySQLParam[3] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[3].ParameterName = "@DisplayType";
                MySQLParam[3].Value = DisplayType;
                MySQLParam[4] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[4].ParameterName = "@DefaultOrderByColumn";
                MySQLParam[4].Value = 1;

                MyDataAccess.ExecCmdNonQueryParams("display_add_new", MySQLParam);
                Myert = true;
                return Myert;
                return functionReturnValue;

            }
            catch (Exception ex)
            {
                return Myert;
            }
            return functionReturnValue;
        }
        [OperationContract()]
        public bool AddDisplayGroupPage(int DisplayID, string GroupName, int DisplayType, string DisplayImage, double DisplayWidth, double DisplayHeight, int Screen, int PanelPos, int PanelNo, string Extra1 = "",
        string Extra2 = "", double ExtraVal1 = 0, double ExtraVal2 = 0)
        {
            bool Myert = false;
            bool functionReturnValue = false;
            //As System.Collections.Generic.List(Of String)
            try
            {
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[13];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@DisplayID";
                MySQLParam[0].Value = DisplayID;
                MySQLParam[1] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[1].ParameterName = "@GroupName";
                MySQLParam[1].Value = GroupName;
                MySQLParam[2] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[2].ParameterName = "@DisplayType";
                MySQLParam[2].Value = DisplayType;
                MySQLParam[3] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[3].ParameterName = "@DisplayImage";
                MySQLParam[3].Value = DisplayImage;
                MySQLParam[4] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[4].ParameterName = "@DisplayWidth";
                MySQLParam[4].Value = DisplayWidth;
                MySQLParam[5] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[5].ParameterName = "@DisplayHeight";
                MySQLParam[5].Value = DisplayHeight;
                MySQLParam[6] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[6].ParameterName = "@Screen";
                MySQLParam[6].Value = Screen;
                MySQLParam[7] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[7].ParameterName = "@PanelNo";
                MySQLParam[7].Value = PanelNo;
                MySQLParam[8] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[8].ParameterName = "@PanelPos";
                MySQLParam[8].Value = PanelPos;
                MySQLParam[9] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[9].ParameterName = "@ExtraData1";
                MySQLParam[9].Value = Extra1;
                MySQLParam[10] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[10].ParameterName = "@ExtraData2";
                MySQLParam[10].Value = Extra2;
                MySQLParam[11] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[11].ParameterName = "@ExtraValue1";
                MySQLParam[11].Value = ExtraVal1;
                MySQLParam[12] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[12].ParameterName = "@ExtraValue2";
                MySQLParam[12].Value = ExtraVal2;

                MyDataAccess.ExecCmdNonQueryParams("displayGroups_add_new", MySQLParam);

                Myert = true;
                return Myert;
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                return Myert;
            }
            return functionReturnValue;

        }
        [OperationContract()]
        public bool EditDisplayGroupPage(int ID, int DisplayID, string GroupName, int DisplayType, string DisplayImage, double DisplayWidth, double DisplayHeight, int Screen, int PanelPos, int PanelNo,
        string Extra1 = "", string Extra2 = "", double ExtraVal1 = 0, double ExtraVal2 = 0)
        {
            bool Myert = false;
            bool functionReturnValue = false;
            //As System.Collections.Generic.List(Of String)
            try
            {
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[14];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@DisplayID";
                MySQLParam[0].Value = DisplayID;
                MySQLParam[1] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[1].ParameterName = "@GroupName";
                MySQLParam[1].Value = GroupName;
                MySQLParam[2] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[2].ParameterName = "@DisplayType";
                MySQLParam[2].Value = DisplayType;
                MySQLParam[3] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[3].ParameterName = "@DisplayImage";
                MySQLParam[3].Value = DisplayImage;
                MySQLParam[4] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[4].ParameterName = "@DisplayWidth";
                MySQLParam[4].Value = DisplayWidth;
                MySQLParam[5] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[5].ParameterName = "@DisplayHeight";
                MySQLParam[5].Value = DisplayHeight;
                MySQLParam[6] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[6].ParameterName = "@Screen";
                MySQLParam[6].Value = Screen;
                MySQLParam[7] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[7].ParameterName = "@PanelNo";
                MySQLParam[7].Value = PanelNo;
                MySQLParam[8] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[8].ParameterName = "@PanelPos";
                MySQLParam[8].Value = PanelPos;
                MySQLParam[9] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[9].ParameterName = "@ExtraData1";
                MySQLParam[9].Value = Extra1;
                MySQLParam[10] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[10].ParameterName = "@ExtraData2";
                MySQLParam[10].Value = Extra2;
                MySQLParam[11] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[11].ParameterName = "@ExtraValue1";
                MySQLParam[11].Value = ExtraVal1;
                MySQLParam[12] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[12].ParameterName = "@ExtraValue2";
                MySQLParam[12].Value = ExtraVal2;
                MySQLParam[13] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[13].ParameterName = "@ID";
                MySQLParam[13].Value = ID;
                MyDataAccess.ExecCmdNonQueryParams("displayGroups_Update", MySQLParam);

                Myert = true;
                return Myert;
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                return Myert;
            }
            return functionReturnValue;
        }

        [OperationContract()]
        public bool ChangeDisplayGroupPage(int ID, int NewScreen, int OldScreen, int DisplayGroup)
        {
            bool Myert = false;
            bool functionReturnValue = false;
            //As System.Collections.Generic.List(Of String)
            try
            {
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[4];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@NewScreen";
                MySQLParam[0].Value = NewScreen;
                MySQLParam[1] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[1].ParameterName = "@ID";
                MySQLParam[1].Value = ID;
                MySQLParam[2] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[2].ParameterName = "@OldScreen";
                MySQLParam[2].Value = OldScreen;
                MySQLParam[3] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[3].ParameterName = "@DisplayGroup";
                MySQLParam[3].Value = DisplayGroup;
                MyDataAccess.ExecCmdNonQueryParams("displayGroups_ReOrder", MySQLParam);
                Myert = true;
                return Myert;
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                return Myert;
            }
            return functionReturnValue;
        }

        [OperationContract()]
        //As System.Collections.Generic.List(Of String)
        public void SetDisplaySensorPosition(int DisplayID, int SensorID, double SensorTop, double SensorLeft)
        {
            try
            {
                string MySql = "";
                MySql += "Update DisplaySensorLink set ExtraValue1 =" + SensorLeft.ToString();
                MySql += " , ExtraValue2= " + SensorTop.ToString();
                MySql += " where DisplayGroupID=" + DisplayID.ToString();
                MySql += " and SensorID=" + SensorID.ToString();
                MyDataAccess.UpdateDBSQLStr(MySql);

            }
            catch (Exception ex)
            {
            }
        }
        [OperationContract()]
        //As System.Collections.Generic.List(Of String)
        public void SetDisplayBackImage(int DisplayID, string BackImage)
        {
            try
            {
                string MySql = "";
                MySql += "Update DisplayGroups set DisplayImage =" + BackImage.ToString();
                MySql += " where DisplayID=" + DisplayID.ToString();
                MyDataAccess.UpdateDBSQLStr(MySql);

            }
            catch (Exception ex)
            {
            }
        }

        [OperationContract()]
        public bool RemoveSensor(int DisplayID, int SensorID)
        {
            bool Myert = false;
            bool functionReturnValue = false;
            //As System.Collections.Generic.List(Of String)
            try
            {
                string MySql = "";
                MySql += "Delete from DisplaySensorLink ";
                MySql += " where DisplayGroupID=" + DisplayID.ToString();
                MySql += " and SensorID=" + SensorID.ToString();
                MyDataAccess.UpdateDBSQLStr(MySql);
                Myert = true;
                return Myert;
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                return Myert;
            }
            return functionReturnValue;
        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetAllSensors()
        {
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                try
                {
                    System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryNoParams("sensor_select_all");
                    //displayGroups_select_specific
                    if ((Mysqlreader == null) == false)
                    {
                        while (Mysqlreader.Read())
                        {
                            int myIntType = Convert.ToInt32(Mysqlreader["Type"]);
                            //MyNewSensor.Type
                            //items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString)
                            //Try
                            //    items.Add(MyNewSensor.Caption + "|" + MyNewSensor.ID.ToString + "|" + MyNewSensor.SiteID.ToString + "|" + myInt.ToString + "|" + myIntType.ToString + "|" + CStr(If(MyNewSensor.ExtraData, "")) + "|" + CStr(If(MyNewSensor.ExtraData1, "")) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)))
                            //Catch ex As Exception

                            //End Try
                            string MyString = "";
                            try
                            {
                                MyString = Convert.ToString(Mysqlreader["Caption"]) + "|";
                                //1
                                MyString += Convert.ToString(Mysqlreader["ID"]) + "|";
                                //2
                                MyString += Convert.ToString(Mysqlreader["SiteID"]) + "|";
                                //3
                                MyString += "0|";
                                //4
                                MyString += myIntType.ToString() + "|";
                                //5
                                //6
                                if (Information.IsDBNull(Mysqlreader["ExtraData"]) == false)
                                {
                                    MyString += Convert.ToString(Mysqlreader["ExtraData"] ?? "") + "|";
                                }
                                else
                                {
                                    MyString += "|";
                                }
                                //7
                                if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                                {
                                    MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                                }
                                else
                                {
                                    MyString += "|";
                                }
                                //8
                                if (Information.IsDBNull(Mysqlreader["ExtraValue"]) == false)
                                {
                                    MyString += Convert.ToString(Mysqlreader["ExtraValue"] ?? 0) + "|";
                                }
                                else
                                {
                                    MyString += "|";
                                }
                                //9
                                if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                                {
                                    MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0) + "|";
                                }
                                else
                                {
                                    MyString += "|";
                                }
                                //10
                                if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                                {
                                    MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                                }
                                else
                                {
                                    MyString += "|";
                                }
                                //11
                                if (Information.IsDBNull(Mysqlreader["ExtraData3"]) == false)
                                {
                                    MyString += Convert.ToString(Mysqlreader["ExtraData3"] ?? "") + "|";
                                }
                                else
                                {
                                    MyString += "|";
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                            items.Add(MyString);
                        }
                    }



                }
                catch (Exception ex)
                {
                }


                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetSpecificSensorDisplay(int SensorId, int DisplayID, int SensorScale = 100, string ExtraData = "", string ExtraData1 = "")
        {
            try
            {
                //add to display sensor link 
                // @DisplayGroupID int,
                //@SensorID int,
                //@DisplayOrder int
                System.Data.SqlClient.SqlParameter[] MyUpdSQLParam = new System.Data.SqlClient.SqlParameter[6];
                MyUpdSQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MyUpdSQLParam[0].ParameterName = "@DisplayGroupID";
                MyUpdSQLParam[0].Value = DisplayID;
                MyUpdSQLParam[1] = new System.Data.SqlClient.SqlParameter();
                MyUpdSQLParam[1].ParameterName = "@SensorID";
                MyUpdSQLParam[1].Value = SensorId;
                MyUpdSQLParam[2] = new System.Data.SqlClient.SqlParameter();
                MyUpdSQLParam[2].ParameterName = "@DisplayOrder";
                MyUpdSQLParam[2].Value = 1;
                MyUpdSQLParam[3] = new System.Data.SqlClient.SqlParameter();
                MyUpdSQLParam[3].ParameterName = "@SensorScale";
                MyUpdSQLParam[3].Value = SensorScale;
                MyUpdSQLParam[4] = new System.Data.SqlClient.SqlParameter();
                MyUpdSQLParam[4].ParameterName = "@ExtraData1";
                MyUpdSQLParam[4].Value = ExtraData;
                MyUpdSQLParam[5] = new System.Data.SqlClient.SqlParameter();
                MyUpdSQLParam[5].ParameterName = "@ExtraData2";
                MyUpdSQLParam[5].Value = ExtraData1;


                MyDataAccess.ExecCmdNonQueryParams("displaySensorLink_add_new", MyUpdSQLParam);


            }
            catch (Exception ex)
            {
            }
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                //Dim MySensorCollection As New ArrayList
                //Dim MySensor As Sensor
                //Dim sites As IPMonLinqDataContext = New IPMonLinqDataContext()


                //Dim DisplaySensorsQuery = From DispGrp In sites.DisplaySensorLinks _
                //                          Where DispGrp.DisplayGroupID = GroupId _
                //                          Order By DispGrp.DisplayOrder, DispGrp.ID _
                //                         Select DispGrp.SensorID
                //Dim DisplaySensorsQuery = From DispGrp In sites.SensorGroupViews _
                //                        Where DispGrp.DisplayGroupID = GroupId _
                //                       Select DispGrp
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@SensorID";
                MySQLParam[0].Value = SensorId;

                System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryParams("displayGroups_select_specific_sensor", MySQLParam);
                //displayGroups_select_specific
                if ((Mysqlreader == null) == false)
                {
                    while (Mysqlreader.Read())
                    {
                        int myIntType = Convert.ToInt32(Mysqlreader["Type"]);
                        //MyNewSensor.Type
                        //items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString)
                        //Try
                        //    items.Add(MyNewSensor.Caption + "|" + MyNewSensor.ID.ToString + "|" + MyNewSensor.SiteID.ToString + "|" + myInt.ToString + "|" + myIntType.ToString + "|" + CStr(If(MyNewSensor.ExtraData, "")) + "|" + CStr(If(MyNewSensor.ExtraData1, "")) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)))
                        //Catch ex As Exception

                        //End Try
                        //Try
                        //    items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString + "|" + myIntType.ToString + "|" + MySensor.ExtraData + "|" + MySensor.ExtraData1 + "|" + MySensor.ExtraValue.ToString + "|" + MySensor.ExtraValue1.ToString)
                        //Catch ex As Exception

                        //End Try
                        string MyString = "";
                        try
                        {
                            MyString = Convert.ToString(Mysqlreader["Caption"]) + "|";
                            MyString += Convert.ToString(Mysqlreader["ID"]) + "|";
                            MyString += Convert.ToString(Mysqlreader["SiteID"]) + "|";
                            MyString += "0|";
                            MyString += myIntType.ToString() + "|";
                            if (Information.IsDBNull(Mysqlreader["ExtraData"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData3"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData3"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraData1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraData2"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraValue1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraValue2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraValue2"] ?? 0);
                            }
                            else
                            {
                                MyString += "|";
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        items.Add(MyString);
                    }
                }
                //Try
                //    items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString + "|" + myIntType.ToString + "|" + MySensor.ExtraData + "|" + MySensor.ExtraData1 + "|" + MySensor.ExtraValue.ToString + "|" + MySensor.ExtraValue1.ToString)
                //Catch ex As Exception

                //End Try

                //For Each MyNewSensor As SensorGroupView In DisplaySensorsQuery
                //    Try
                //        'Dim MySensorid As Integer = Mysensid
                //        'Try
                //        'Dim SensorsQuery = From sens In sites.Sensors _
                //        'Where sens.ID = MySensorid _
                //        '     Select sens
                //        'For Each MyNewSensor In SensorsQuery
                //        'If MySensor.ID = Mysensid Then
                //        Dim myInt As Integer = 0 'MySensor.Status
                //        'Dim myInt As Integer = MySensor.Status


                //        'items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + GroupId.ToString + "|" + myInt.ToString)
                //        'End If
                //        'Next
                //        'Catch ex As Exception

                //        'End Try

                //    Catch ex As Exception

                //    End Try

                //Next

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetSpecificSensorOnDisplay(int SensorId)
        {

            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@ID";
                MySQLParam[0].Value = SensorId;

                System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryParams("sensor_select_specific", MySQLParam);
                //displayGroups_select_specific_sensor
                //displayGroups_select_specific
                if ((Mysqlreader == null) == false)
                {
                    while (Mysqlreader.Read())
                    {
                        int myIntType = Convert.ToInt32(Mysqlreader["Type"]);
                        //MyNewSensor.Type
                        string MyString = "";
                        try
                        {
                            MyString = Convert.ToString(Mysqlreader["Caption"]) + "|";
                            MyString += Convert.ToString(Mysqlreader["ID"]) + "|";
                            MyString += Convert.ToString(Mysqlreader["SiteID"]) + "|";
                            MyString += "0|";
                            MyString += myIntType.ToString() + "|";
                            if (Information.IsDBNull(Mysqlreader["ExtraData"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData3"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData3"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            MyString += "|";
                            MyString += "|";
                            MyString += "|";
                            MyString += "|";

                            //If IsDBNull(Mysqlreader.Item("DispExtraData1")) = False Then
                            //    MyString += CStr(If(Mysqlreader.Item("DispExtraData1"), 0)) + "|"
                            //Else
                            //    MyString += "|"
                            //End If
                            //If IsDBNull(Mysqlreader.Item("DispExtraData2")) = False Then
                            //    MyString += CStr(If(Mysqlreader.Item("DispExtraData2"), 0)) + "|"
                            //Else
                            //    MyString += "|"
                            //End If
                            //If IsDBNull(Mysqlreader.Item("DispExtraValue1")) = False Then
                            //    MyString += CStr(If(Mysqlreader.Item("DispExtraValue1"), 0)) + "|"
                            //Else
                            //    MyString += "|"
                            //End If
                            //If IsDBNull(Mysqlreader.Item("DispExtraValue2")) = False Then
                            //    MyString += CStr(If(Mysqlreader.Item("DispExtraValue2"), 0))
                            //Else
                            //    MyString += "|"
                            //End If

                        }
                        catch (Exception ex)
                        {
                        }
                        items.Add(MyString);
                    }
                }


                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetListSensors(string SensorIds)
        {
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()

                string[] Sensors = SensorIds.Split(',');
                foreach (string MySensorIDstr in Sensors)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(MySensorIDstr))
                        {
                            System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                            MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                            MySQLParam[0].ParameterName = "@ID";
                            MySQLParam[0].Value = Convert.ToInt32(MySensorIDstr);

                            System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryParams("sensor_select_specific", MySQLParam);
                            //displayGroups_select_specific
                            if ((Mysqlreader == null) == false)
                            {
                                while (Mysqlreader.Read())
                                {
                                    int myIntType = Convert.ToInt32(Mysqlreader["Type"]);
                                    //MyNewSensor.Type
                                    //items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString)
                                    //Try
                                    //    items.Add(MyNewSensor.Caption + "|" + MyNewSensor.ID.ToString + "|" + MyNewSensor.SiteID.ToString + "|" + myInt.ToString + "|" + myIntType.ToString + "|" + CStr(If(MyNewSensor.ExtraData, "")) + "|" + CStr(If(MyNewSensor.ExtraData1, "")) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)))
                                    //Catch ex As Exception

                                    //End Try
                                    string MyString = "";
                                    try
                                    {
                                        MyString = Convert.ToString(Mysqlreader["Caption"]) + "|";
                                        MyString += Convert.ToString(Mysqlreader["ID"]) + "|";
                                        MyString += Convert.ToString(Mysqlreader["SiteID"]) + "|";
                                        MyString += "0|";
                                        MyString += myIntType.ToString() + "|";
                                        if (Information.IsDBNull(Mysqlreader["ExtraData"]) == false)
                                        {
                                            MyString += Convert.ToString(Mysqlreader["ExtraData"] ?? "") + "|";
                                        }
                                        else
                                        {
                                            MyString += "|";
                                        }
                                        if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                                        {
                                            MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                                        }
                                        else
                                        {
                                            MyString += "|";
                                        }
                                        if (Information.IsDBNull(Mysqlreader["ExtraValue"]) == false)
                                        {
                                            MyString += Convert.ToString(Mysqlreader["ExtraValue"] ?? 0) + "|";
                                        }
                                        else
                                        {
                                            MyString += "|";
                                        }
                                        if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                                        {
                                            MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0) + "|";
                                        }
                                        else
                                        {
                                            MyString += "|";
                                        }
                                        if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                                        {
                                            MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                                        }
                                        else
                                        {
                                            MyString += "|";
                                        }
                                        if (Information.IsDBNull(Mysqlreader["ExtraData3"]) == false)
                                        {
                                            MyString += Convert.ToString(Mysqlreader["ExtraData3"] ?? "") + "|";
                                        }
                                        else
                                        {
                                            MyString += "|";
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    items.Add(MyString);
                                }
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                }

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetGroupSensors(int GroupId)
        {
            try
            {
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                //Dim MySensorCollection As New ArrayList
                //Dim MySensor As Sensor
                //Dim sites As IPMonLinqDataContext = New IPMonLinqDataContext()


                //Dim DisplaySensorsQuery = From DispGrp In sites.DisplaySensorLinks _
                //                          Where DispGrp.DisplayGroupID = GroupId _
                //                          Order By DispGrp.DisplayOrder, DispGrp.ID _
                //                         Select DispGrp.SensorID
                //Dim DisplaySensorsQuery = From DispGrp In sites.SensorGroupViews _
                //                        Where DispGrp.DisplayGroupID = GroupId _
                //                       Select DispGrp
                System.Data.SqlClient.SqlParameter[] MySQLParam = new System.Data.SqlClient.SqlParameter[1];
                MySQLParam[0] = new System.Data.SqlClient.SqlParameter();
                MySQLParam[0].ParameterName = "@DisplayID";
                MySQLParam[0].Value = GroupId;

                System.Data.SqlClient.SqlDataReader Mysqlreader = MyDataAccess.ExecCmdQueryParams("displayGroups_select_sensors", MySQLParam);
                //displayGroups_select_specific
                if ((Mysqlreader == null) == false)
                {
                    while (Mysqlreader.Read())
                    {
                        int myIntType = Convert.ToInt32(Mysqlreader["Type"]);
                        //MyNewSensor.Type
                        //items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString)
                        //Try
                        //    items.Add(MyNewSensor.Caption + "|" + MyNewSensor.ID.ToString + "|" + MyNewSensor.SiteID.ToString + "|" + myInt.ToString + "|" + myIntType.ToString + "|" + CStr(If(MyNewSensor.ExtraData, "")) + "|" + CStr(If(MyNewSensor.ExtraData1, "")) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)) + "|" + CStr(If(MyNewSensor.ExtraValue, 0)))
                        //Catch ex As Exception

                        //End Try
                        string MyString = "";
                        try
                        {
                            MyString = Convert.ToString(Mysqlreader["Caption"]) + "|";
                            MyString += Convert.ToString(Mysqlreader["ID"]) + "|";
                            if (Information.IsDBNull(Mysqlreader["SiteID"]))
                            {
                                MyString += "0|";
                            }
                            else
                            {
                                MyString += Convert.ToString(Mysqlreader["SiteID"]) + "|";
                            }

                            MyString += "0|";
                            MyString += myIntType.ToString() + "|";
                            if (Information.IsDBNull(Mysqlreader["ExtraData"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData1"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraValue1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData2"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["ExtraData3"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["ExtraData3"] ?? "") + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraData1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraData1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraData2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraData2"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraValue1"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraValue1"] ?? 0) + "|";
                            }
                            else
                            {
                                MyString += "|";
                            }
                            if (Information.IsDBNull(Mysqlreader["DispExtraValue2"]) == false)
                            {
                                MyString += Convert.ToString(Mysqlreader["DispExtraValue2"] ?? 0);
                            }
                            else
                            {
                                MyString += "|";
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        items.Add(MyString);
                    }
                }
                //For Each MyNewSensor As SensorGroupView In DisplaySensorsQuery
                //    Try
                //        'Dim MySensorid As Integer = Mysensid
                //        'Try
                //        'Dim SensorsQuery = From sens In sites.Sensors _
                //        'Where sens.ID = MySensorid _
                //        '     Select sens
                //        'For Each MyNewSensor In SensorsQuery
                //        'If MySensor.ID = Mysensid Then
                //        Dim myInt As Integer = 0 'MySensor.Status
                //        'Dim myInt As Integer = MySensor.Status


                //        'items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + GroupId.ToString + "|" + myInt.ToString)
                //        'End If
                //        'Next
                //        'Catch ex As Exception

                //        'End Try

                //    Catch ex As Exception

                //    End Try

                //Next

                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetGroupSensorsStatus(int GroupID, string Sensors)
        {
            try
            {
                Collection MyCollection = new Collection();
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                ArrayList MySensorCollection = new ArrayList();
                MyCollection = GetSpecificRemoteSensors(Sensors);
                //MyRem.GetServerObjects 'server1.GetAll()
                if ((MyCollection == null) == false)
                {
                    object MyObject1 = null;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                            string RetString = "|";
                            try
                            {
                                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MySensor.Fields)
                                {
                                    RetString += MyFields.FieldName + "," + MyFields.LastValue.ToString() + "," + MyFields.LastOtherValue + ",";
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            int myInt = (int)MySensor.Status;
                            items.Add(MySensor.ID.ToString() + "|" + myInt.ToString() + RetString);
                        }
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetSiteSensors(int SiteId)
        {
            try
            {
                Collection MyCollection = new Collection();
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                ArrayList MySensorCollection = new ArrayList();
                // Dim MyFunc As New LiveMonitoring.SharedFuncs
                //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                MyCollection = GetRemoteSensors();
                //MyRem.GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        if (MySensor.SiteID == SiteId)
                        {
                            //MySensorCollection.Add(MyFunc.IPSerialise(MySensor))
                            int myInt = (int)MySensor.Status;
                            int myIntType = (int)MySensor.Type;
                            //items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString)
                            try
                            {
                                items.Add(MySensor.Caption + "|" + MySensor.ID.ToString() + "|" + MySensor.SiteID.ToString() + "|" + myInt.ToString() + "|" + myIntType.ToString() + "|" + MySensor.ExtraData + "|" + MySensor.ExtraData1 + "|" + MySensor.ExtraValue.ToString() + "|" + MySensor.ExtraValue1.ToString());

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetSiteSensorsStatus(int SiteId, string Sensors)
        {
            try
            {
                Collection MyCollection = new Collection();
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                ArrayList MySensorCollection = new ArrayList();
                //Dim MyFunc As New LiveMonitoring.SharedFuncs
                //Dim MyRem As New LiveMonitoring.GlobalRemoteVars
                MyCollection = GetSpecificRemoteSensors(Sensors);
                //MyRem.GetServerObjects 'server1.GetAll()
                if ((MyCollection == null) == false)
                {
                    object MyObject1 = null;
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                        {
                            LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                            //If MySensor.SiteID = SiteId Then
                            //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                            string RetString = "|";
                            try
                            {
                                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MySensor.Fields)
                                {
                                    RetString += MyFields.FieldName + "," + MyFields.LastValue.ToString() + "," + MyFields.LastOtherValue + ",";
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                            int myInt = (int)MySensor.Status;
                            items.Add(MySensor.ID.ToString() + "|" + myInt.ToString() + RetString);
                            //End If
                        }
                    }
                }
                return items;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetSensors()
        {
            try
            {
                Collection MyCollection = new Collection();
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                ArrayList MySensorCollection = new ArrayList();
                MyCollection = GetRemoteSensors();
                //MyRem.GetServerObjects 'server1.GetAll()
                object MyObject1 = null;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                    {
                        LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                        int myInt = (int)MySensor.Status;
                        int myIntType = (int)MySensor.Type;
                        items.Add(MySensor.Caption + "|" + MySensor.ID.ToString() + "|" + MySensor.SiteID.ToString() + "|" + myInt.ToString() + "|" + myIntType.ToString() + "|" + MySensor.ExtraData + "|" + MySensor.ExtraData1 + "|" + MySensor.ExtraValue.ToString() + "|" + MySensor.ExtraValue1.ToString() + "|" + MySensor.ExtraData2.ToString() + "|" + MySensor.ExtraData3.ToString());
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [OperationContract()]
        public string CheckLogin(string UserName, string Password)
        {
            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            string MyRet = "";
            try
            {
                MyUser = MyRem.LiveMonServer.TryLogin(UserName, Password, "", "", "");
            }
            catch (Exception ex)
            {
                return "";
            }
            if ((MyUser == null) == false)
            {
                MyRet = MyUser.FirstName + "|" + MyUser.SurName + "|" + MyUser.ID.ToString() + "|" + MyUser.UserLevel.ToString();
            }
            return MyRet;
        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetSensorValues(int SensorID)
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //MyCollection = MyRem.GetServerObjects 'server1.GetAll()
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = ServerInterface.GetSpecificSensor(SensorID);
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObjSLect1, LiveMonitoring.IRemoteLib.SensorDetails)
            if (MySensor.ID == SensorID)
            {
                //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                string RetString = "";
                //MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MySensor.Fields)
                {
                    try
                    {
                        string MyOtherValue = "";
                        try
                        {
                            MyOtherValue = MyFields.LastOtherValue.Replace("|", "[");

                        }
                        catch (Exception ex)
                        {
                        }
                        string MyFieldName = "";
                        try
                        {
                            MyFieldName = MyFields.FieldName.Replace("|", "[");

                        }
                        catch (Exception ex)
                        {
                        }
                        RetString = MyFieldName + "|" + MyFields.LastValue.ToString() + "|" + MyOtherValue + "|" + MyFields.LastDTRead.ToString() + "|" + SensorID.ToString() + "|" + MyFields.Caption + "|" + MyFields.DisplayValue.ToString() + "|" + MyFields.FieldNumber.ToString() + "|" + MyFields.TabularRowNo.ToString() + "|" + MyFields.FieldMaxValue.ToString() + "|" + MyFields.FieldMaxWarnValue.ToString() + "|" + MyFields.FieldMinValue.ToString() + "|" + MyFields.FieldMinWarnValue.ToString() + "|" + MyFields.FieldPercentOfTest.ToString();


                    }
                    catch (Exception ex)
                    {
                    }
                    items.Add(RetString);
                }
            }
            //    End If
            //Next
            return items;
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetSensorFieldValues(int SensorID, int SensorFieldno)
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //MyCollection = MyRem.GetServerObjects 'server1.GetAll()
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = ServerInterface.GetSpecificSensor(SensorID);
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObjSLect1, LiveMonitoring.IRemoteLib.SensorDetails)
            if (MySensor.ID == SensorID)
            {
                //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                string RetString = "";
                //MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MySensor.Fields)
                {
                    try
                    {
                        if (SensorFieldno == MyFields.FieldNumber)
                        {
                            string MyOtherValue = "";
                            try
                            {
                                MyOtherValue = MyFields.LastOtherValue.Replace("|", "[");

                            }
                            catch (Exception ex)
                            {
                            }
                            string MyFieldName = "";
                            try
                            {
                                MyFieldName = MyFields.FieldName.Replace("|", "[");

                            }
                            catch (Exception ex)
                            {
                            }
                            RetString = MyFieldName + "|" + MyFields.LastValue.ToString() + "|" + MyOtherValue + "|" + MyFields.LastDTRead.ToString() + "|" + SensorID.ToString() + "|" + MyFields.Caption + "|" + MyFields.DisplayValue.ToString() + "|" + MyFields.FieldNumber.ToString() + "|" + MyFields.TabularRowNo.ToString() + "|" + MyFields.FieldMaxValue.ToString() + "|" + MyFields.FieldMaxWarnValue.ToString() + "|" + MyFields.FieldMinValue.ToString() + "|" + MyFields.FieldMinWarnValue.ToString() + "|" + MyFields.FieldPercentOfTest.ToString();
                            items.Add(RetString);
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            //    End If
            //Next
            return items;
        }
        [OperationContract()]
        public string GetSensorsStatus(int SensorID)
        {
            string functionReturnValue = null;
            //System.Collections.Generic.List(Of Integer)
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = ServerInterface.GetSpecificSensor(SensorID);
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            if (MySensor.ID == SensorID)
            {
                return Convert.ToString(MySensor.Status) + "|" + SensorID.ToString();
                return functionReturnValue;
                //Dim MyFields As LiveMonitoring.IRemoteLib.SensorFieldsDef
                //Dim RetString As String = "" 'MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                //For Each MyFields In MySensor.Fields
                //    RetString = MyFields.FieldName + "|" + MyFields.LastValue.ToString + "|" + MyFields.LastOtherValue + "|" + MyFields.LastDTRead.ToString
                //    items.Add(RetString)
                //Next
            }

            //MyCollection = MyRem.GetServerObjects 'server1.GetAll()
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            //        If MySensor.ID = SensorID Then
            //            Return MySensor.Status
            //        End If
            //    End If
            //Next
            return LiveMonitoring.IRemoteLib.StatusDef.noresponse.ToString();
            return functionReturnValue;
        }
        [OperationContract()]
        public int GetSensorStatus(int SensorID)
        {
            int functionReturnValue = 0;
            //System.Collections.Generic.List(Of Integer)
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = ServerInterface.GetSpecificSensor(SensorID);
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            if (MySensor.ID == SensorID)
            {
                return (int)MySensor.Status;
                return functionReturnValue;
                //Dim MyFields As LiveMonitoring.IRemoteLib.SensorFieldsDef
                //Dim RetString As String = "" 'MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                //For Each MyFields In MySensor.Fields
                //    RetString = MyFields.FieldName + "|" + MyFields.LastValue.ToString + "|" + MyFields.LastOtherValue + "|" + MyFields.LastDTRead.ToString
                //    items.Add(RetString)
                //Next
            }

            //MyCollection = MyRem.GetServerObjects 'server1.GetAll()
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
            //        Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
            //        If MySensor.ID = SensorID Then
            //            Return MySensor.Status
            //        End If
            //    End If
            //Next
            return (int)LiveMonitoring.IRemoteLib.StatusDef.noresponse;
            return functionReturnValue;
        }

        [OperationContract()]
        public Collection GetSensorHistory(int SensorID, DateTime StartDate, DateTime EndDate)
        {
            //System.Collections.Generic.List(Of Integer)
            Collection MyCollection = new Collection();
            Collection MyRetCollection = new Collection();
            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = ServerInterface.GetSensorHistory(SensorID, StartDate, EndDate);
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.DataHistory)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.DataHistory MyHistory = (LiveMonitoring.IRemoteLib.DataHistory)MyObject1;
                        string RetString = "";
                        //MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                        if ((MyHistory.OtherData == null))
                        {
                            RetString = MyHistory.Field.ToString() + "|" + MyHistory.Value.ToString() + "|''|" + MyHistory.DT.ToString() + "|" + MyHistory.Status.ToString();
                        }
                        else
                        {
                            RetString = MyHistory.Field.ToString() + "|" + MyHistory.Value.ToString() + "|" + MyHistory.OtherData + "|" + MyHistory.DT.ToString() + "|" + MyHistory.Status.ToString();
                        }
                        //RetString = MyHistory.Field + "|" + MyHistory.Value.ToString + "|" + MyHistory.OtherData + "|" + MyHistory.DT.ToString + "|" + MyHistory.Status.ToString
                        MyRetCollection.Add(RetString);


                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return MyRetCollection;
        }
        private LiveMonitoring.IRemoteLib.SensorDetails GetSensorByID(int ID)
        {
            try
            {
                LiveMonitoring.IRemoteLib.SensorDetails MySensor = new LiveMonitoring.IRemoteLib.SensorDetails();
                System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();
                //= New List(Of Object)()
                ArrayList MySensorCollection = new ArrayList();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MySensor = MyRem.LiveMonServer.GetSpecificSensor(ID);
                //GetRemoteSensors() 'MyRem.GetServerObjects 'server1.GetAll()
                //Dim MyObject1 As Object
                return MySensor;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [OperationContract()]
        public System.Collections.Generic.List<object> GetSensorHistorySummary(int SensorID, int Fieldno, double Hours, int Operation)
        {
            //    Private Enum GaugeType
            //    Sensor = 1
            //    Sum = 2
            //    Power = 3
            //    Percentage = 4
            //    Average = 5
            //End Enum
            Collection MyCollection = new Collection();
            List<object> MyRetCollection = new List<object>();
            MyRetCollection.Add(SensorID);
            MyRetCollection.Add(Fieldno);

            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            System.DateTime EndDate = DateAndTime.Now;
            System.DateTime StartDate = DateAndTime.DateAdd(DateInterval.Hour, -Hours, DateAndTime.Now);
            MyCollection = ServerInterface.GetSensorHistory(SensorID, StartDate, EndDate);
            object MyObject1 = null;
            double total = 0;
            int rowcnt = 0;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.DataHistory)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.DataHistory MyHistory = (LiveMonitoring.IRemoteLib.DataHistory)MyObject1;
                        if (MyHistory.Field == Fieldno)
                        {
                            rowcnt += 1;
                            total += MyHistory.Value;
                            //Dim RetString As String = "" 'MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                            //If IsNothing(MyHistory.OtherData) Then
                            //    RetString = MyHistory.Field.ToString + "|" + MyHistory.Value.ToString + "|''|" + MyHistory.DT.ToString + "|" + MyHistory.Status.ToString
                            //Else
                            //    RetString = MyHistory.Field.ToString + "|" + MyHistory.Value.ToString + "|" + MyHistory.OtherData + "|" + MyHistory.DT.ToString + "|" + MyHistory.Status.ToString
                            //End If
                            //'RetString = MyHistory.Field + "|" + MyHistory.Value.ToString + "|" + MyHistory.OtherData + "|" + MyHistory.DT.ToString + "|" + MyHistory.Status.ToString
                            //MyRetCollection.Add(RetString)
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            switch (Operation)
            {
                case 2:
                    MyRetCollection.Add(total);
                    break;
                // Exit Function
                case 4:
                    MyRetCollection.Add(total / rowcnt);
                    break;
                //Exit Function
                case 5:
                    MyRetCollection.Add(total / rowcnt);
                    break;
                //Exit Function
                default:
                    MyRetCollection.Add(0);
                    break;
            }
            //add field name
            try
            {
                LiveMonitoring.IRemoteLib.SensorDetails MySensor = GetSensorByID(SensorID);
                if ((MySensor == null) == false)
                {
                    if (MySensor.Fields.Contains(Fieldno.ToString()))
                    {
                        MyRetCollection.Add(((LiveMonitoring.IRemoteLib.SensorFieldsDef)MySensor.Fields[Fieldno.ToString()]).FieldName);
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return MyRetCollection;
        }

        [OperationContract()]
        public System.Collections.Generic.List<object> GetMeteringKWhSummary(int MeterID, double Hours, int Operation)
        {
            List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData = null;
            LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif = default(LiveMonitoring.IRemoteLib.MeteringTarrifDetails);
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<object> MyRetCollection = new List<object>();
            MyRetCollection.Add(MeterID);

            System.DateTime EndDate = DateAndTime.Now;
            System.DateTime StartDate = DateAndTime.DateAdd(DateInterval.Hour, -Hours, DateAndTime.Now);
            try
            {
                MyData = MyRem.LiveMonServer.GetMeteringProfileRecord(MeterID, StartDate, EndDate);

            }
            catch (Exception ex)
            {
            }
            try
            {
                MyDataMarkers = MyRem.LiveMonServer.GetMeteringProfileMarkers(MeterID, StartDate, EndDate);

            }
            catch (Exception ex)
            {
            }

            //Else
            //MyData = MyRem.server1.GetFilteredSensorHistory(cmbDataSet.SelectedValue, SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
            // End If
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = MyRem.LiveMonServer.GetSpecificSensor(MeterID);
            try
            {
                //GetMeteringTarrifEvent
                if ((MySensor == null) == false)
                {
                    MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(Convert.ToInt32(MySensor.ExtraValue1));
                }
                else
                {
                    MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(1);
                }


            }
            catch (Exception ex)
            {
            }



            int Bcnt = 0;
            int MyPeriod = 30;
            //default to 30 minutes
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                try
                {
                    if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                    {
                        myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                        MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                    }

                }
                catch (Exception ex)
                {
                }

            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next

            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            int findMW = 1;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                try
                {
                    if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                    {
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelName == "KVA")
                        {
                            findMVA = Bcnt + 1;
                        }
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelName == "Import KW")
                        {
                            findMW = Bcnt + 1;
                        }
                        ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelDivisorUnits;
                        //End If
                    }
                    else
                    {
                        ConversionFactor[Bcnt] = 0;
                    }

                }
                catch (Exception ex)
                {
                }

            }
            //pf possible
            if (MyDataChannels.Import_mW & MyDataChannels.mVA)
            {
                ConversionFactor[8] = 0;
            }

            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            double TotKvarCnt = 0;
            double PeakKWhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKWhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKWhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0

            double MaxKvaDemand = 0;
            System.DateTime MaxKvaDemandDate = default(System.DateTime);
            double MaxKwhDemand = 0;
            System.DateTime MaxKwhDemandDate = default(System.DateTime);
            double periodCnt = 0;
            double KVAVal = 0;
            double kwhVal = 0;

            int MaxFieldCnt = 0;
            bool firstrec = true;

            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {

                try
                {
                    // MyDataHistory.TimeStamp
                    //MyTarrif
                    //MyTarrif.ActiveEnergyCharges()
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = 0;
                    //= FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, TypePeriod)
                    try
                    {
                        //Dim TypePeriod As New LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges
                        TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);


                    }
                    catch (Exception ex)
                    {
                    }
                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == null) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                PeakKWhCost = TypePeriod.CostcPerKWh;
                                PeakKWhLabel = TypePeriod.ChargeName;
                                break;
                            case 2:
                                //standard
                                StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                break;
                        }
                        //standard kwh by default
                    }
                    else
                    {
                        StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                    }


                    //KWH findMW
                    switch (findMW)
                    {
                        case 1:
                            //always 1 import w
                            TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                            TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                            break;
                    }
                    kwhVal = (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);


                    periodCnt += 1;
                    if (kwhVal > MaxKwhDemand)
                    {
                        MaxKwhDemand = kwhVal;
                        MaxKwhDemandDate = MyDataHistory.TimeStamp;
                    }
                    if (KVAVal > MaxKvaDemand)
                    {
                        MaxKvaDemand = KVAVal;
                        MaxKvaDemandDate = MyDataHistory.TimeStamp;
                    }

                }
                catch (Exception ex)
                {
                }

                // End If
            }
            // MyRetCollection.Add(StartDate)
            // MyRetCollection.Add(EndDate)
            MyRetCollection.Add(TotKWhCnt);
            MyRetCollection.Add("Kwh");

            return MyRetCollection;

        }
        [OperationContract()]
        public System.Collections.Generic.List<double> GetMeteringKWhLastVal(int MeterID, double Hours, int Operation)
        {
            List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData = null;
            LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif = default(LiveMonitoring.IRemoteLib.MeteringTarrifDetails);
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<double> MyRetCollection = new List<double>();
            MyRetCollection.Add(MeterID);
            System.DateTime EndDate = DateAndTime.Now;
            System.DateTime StartDate = DateAndTime.DateAdd(DateInterval.Hour, -Hours, DateAndTime.Now);
            try
            {
                MyData = MyRem.LiveMonServer.GetMeteringProfileRecord(MeterID, StartDate, EndDate);

            }
            catch (Exception ex)
            {
            }
            try
            {
                MyDataMarkers = MyRem.LiveMonServer.GetMeteringProfileMarkers(MeterID, StartDate, EndDate);

            }
            catch (Exception ex)
            {
            }

            //Else
            //MyData = MyRem.server1.GetFilteredSensorHistory(cmbDataSet.SelectedValue, SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
            // End If
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = MyRem.LiveMonServer.GetSpecificSensor(MeterID);
            try
            {
                //GetMeteringTarrifEvent
                if ((MySensor == null) == false)
                {
                    MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(Convert.ToInt32(MySensor.ExtraValue1));
                }
                else
                {
                    MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(1);
                }


            }
            catch (Exception ex)
            {
            }



            int Bcnt = 0;
            int MyPeriod = 30;
            //default to 30 minutes
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                try
                {
                    if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                    {
                        myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                        MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                    }

                }
                catch (Exception ex)
                {
                }

            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next

            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            int findMW = 1;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                try
                {
                    if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                    {
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelName == "KVA")
                        {
                            findMVA = Bcnt + 1;
                        }
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelName == "Import KW")
                        {
                            findMW = Bcnt + 1;
                        }
                        ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelDivisorUnits;
                        //End If
                    }
                    else
                    {
                        ConversionFactor[Bcnt] = 0;
                    }

                }
                catch (Exception ex)
                {
                }

            }
            //pf possible
            if (MyDataChannels.Import_mW & MyDataChannels.mVA)
            {
                ConversionFactor[8] = 0;
            }

            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            double TotKvarCnt = 0;
            double PeakKWhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKWhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKWhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0

            double MaxKvaDemand = 0;
            System.DateTime MaxKvaDemandDate = default(System.DateTime);
            double MaxKwhDemand = 0;
            System.DateTime MaxKwhDemandDate = default(System.DateTime);
            double periodCnt = 0;
            double KVAVal = 0;
            double kwhVal = 0;

            int MaxFieldCnt = 0;
            bool firstrec = true;

            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {

                try
                {
                    // MyDataHistory.TimeStamp
                    //MyTarrif
                    //MyTarrif.ActiveEnergyCharges()
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = 0;
                    //= FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, TypePeriod)
                    try
                    {
                        //Dim TypePeriod As New LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges
                        TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);


                    }
                    catch (Exception ex)
                    {
                    }
                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == null) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                PeakKWhCost = TypePeriod.CostcPerKWh;
                                PeakKWhLabel = TypePeriod.ChargeName;
                                break;
                            case 2:
                                //standard
                                StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                break;
                        }
                        //standard kwh by default
                    }
                    else
                    {
                        StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                    }


                    //KWH findMW
                    switch (findMW)
                    {
                        case 1:
                            //always 1 import w
                            TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                            TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                            break;
                    }
                    kwhVal = (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);

                    periodCnt += 1;
                    if (kwhVal > MaxKwhDemand)
                    {
                        MaxKwhDemand = kwhVal;
                        MaxKwhDemandDate = MyDataHistory.TimeStamp;
                    }
                    if (KVAVal > MaxKvaDemand)
                    {
                        MaxKvaDemand = KVAVal;
                        MaxKvaDemandDate = MyDataHistory.TimeStamp;
                    }

                }
                catch (Exception ex)
                {
                }

                // End If
            }
            // MyRetCollection.Add(StartDate)
            // MyRetCollection.Add(EndDate)
            MyRetCollection.Add(kwhVal);
            return MyRetCollection;

        }
        [OperationContract()]
        public List<object> GetMeteringKWh(int MeterID, DateTime StartDate, DateTime EndDate)
        {
            //System.Collections.Generic.List(Of Integer)
            List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyData = null;
            LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyTarrif = default(LiveMonitoring.IRemoteLib.MeteringTarrifDetails);
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyDataMarkers = null;
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            List<object> MyRetCollection = new List<object>();
            try
            {
                MyData = MyRem.LiveMonServer.GetMeteringProfileRecord(MeterID, StartDate, EndDate);

            }
            catch (Exception ex)
            {
            }
            try
            {
                MyDataMarkers = MyRem.LiveMonServer.GetMeteringProfileMarkers(MeterID, StartDate, EndDate);

            }
            catch (Exception ex)
            {
            }

            //Else
            //MyData = MyRem.server1.GetFilteredSensorHistory(cmbDataSet.SelectedValue, SensorDet.ID, CDate(Session("StartDate")), CDate(Session("EndDate")))
            // End If
            LiveMonitoring.IRemoteLib.SensorDetails MySensor = MyRem.LiveMonServer.GetSpecificSensor(MeterID);
            try
            {
                //GetMeteringTarrifEvent
                if ((MySensor == null) == false)
                {
                    MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(Convert.ToInt32(MySensor.ExtraValue1));
                }
                else
                {
                    MyTarrif = MyRem.LiveMonServer.GetMeteringTarrif(1);
                }


            }
            catch (Exception ex)
            {
            }



            int Bcnt = 0;
            int MyPeriod = 30;
            //default to 30 minutes
            //check if we have a channel config else use default 69
            int myChannelConfig = 69;
            //LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileMarker);
            foreach (LiveMonitoring.IRemoteLib.MeteringProfileMarker MyDataMarkerHistory in MyDataMarkers)
            {
                try
                {
                    if (MyDataMarkerHistory.ProfileMarkerType == 1 | MyDataMarkerHistory.ProfileMarkerType == 4)
                    {
                        myChannelConfig = MyDataMarkerHistory.TRegisterConfigValue;
                        MyPeriod = returnMeteringPeriod(Convert.ToInt32(Conversion.Hex(MyDataMarkerHistory.Period).Substring(0, 1)));
                    }

                }
                catch (Exception ex)
                {
                }

            }
            //ok now we have channels set the names
            LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig MyDataChannels = new LiveMonitoring.IRemoteLib.ElsterA1700Data.TChannelConfig(myChannelConfig);
            //Dim mychanel As LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData

            //For Each mychanel In MyDataChannels.ChannelNames

            //Next

            double[] ConversionFactor = new double[10];
            int findMVA = 0;
            int findMW = 0;
            for (Bcnt = 0; Bcnt <= 7; Bcnt++)
            {
                try
                {
                    if (MyDataChannels.ChannelNames.Contains((Bcnt).ToString()) == true)
                    {
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelName == "KVA")
                        {
                            findMVA = Bcnt + 1;
                        }
                        if (((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelName == "Import KW")
                        {
                            findMW = Bcnt + 1;
                        }
                        ConversionFactor[Bcnt] = ((LiveMonitoring.IRemoteLib.ElsterA1700Data.ChanelData)MyDataChannels.ChannelNames[Bcnt]).ChannelDivisorUnits;
                        //End If
                    }
                    else
                    {
                        ConversionFactor[Bcnt] = 0;
                    }

                }
                catch (Exception ex)
                {
                }

            }
            //pf possible
            if (MyDataChannels.Import_mW & MyDataChannels.mVA)
            {
                ConversionFactor[8] = 0;
            }

            int tmp1cntwe = 0;
            //mycnt1= fields ?
            //LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory = default(LiveMonitoring.IRemoteLib.MeteringProfileRecord);
            double TotKWhCnt = 0;
            double TotKWCnt = 0;
            double TotKvarCnt = 0;
            double PeakKWhCnt = 0;
            double StandKWhCnt = 0;
            double OffPeakKWhCnt = 0;
            double PeakKWhCost = 0;
            double StandKWhCost = 0;
            double OffPeakKWhCost = 0;
            double PeakKWhTotCost = 0;
            double StandKWhTotCost = 0;
            double OffPeakKWhTotCost = 0;
            string PeakKWhLabel = "";
            // = 0
            string StandKWhLabel = "";
            // = 0
            string OffPeakKWhLabel = "";
            // = 0

            double MaxKvaDemand = 0;
            System.DateTime MaxKvaDemandDate = default(System.DateTime);
            double MaxKwhDemand = 0;
            System.DateTime MaxKwhDemandDate = default(System.DateTime);
            double periodCnt = 0;
            double KVAVal = 0;
            double kwhVal = 0;

            int MaxFieldCnt = 0;
            bool firstrec = true;

            foreach (LiveMonitoring.IRemoteLib.MeteringProfileRecord MyDataHistory in MyData)
            {

                try
                {
                    // MyDataHistory.TimeStamp
                    //MyTarrif
                    //MyTarrif.ActiveEnergyCharges()
                    LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges TypePeriod = new LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges();
                    int TmpChargePeriod = 0;
                    //= FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, TypePeriod)
                    try
                    {
                        //Dim TypePeriod As New LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges
                        TmpChargePeriod = FindPeriod(MyDataHistory.TimeStamp, MyTarrif.ActiveEnergyCharges, ref TypePeriod);


                    }
                    catch (Exception ex)
                    {
                    }
                    if ((TypePeriod.ChargeName == null) == false & (TmpChargePeriod == null) == false)
                    {
                        switch (TypePeriod.ChargePeriods[TmpChargePeriod].MeteringChargeType.ID)
                        {
                            //cant mix peak and offp eak so first one should be correct
                            case 1:
                                //peak
                                PeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                PeakKWhCost = TypePeriod.CostcPerKWh;
                                PeakKWhLabel = TypePeriod.ChargeName;
                                break;
                            case 2:
                                //standard
                                StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                StandKWhCost = TypePeriod.CostcPerKWh;
                                StandKWhLabel = TypePeriod.ChargeName;
                                break;
                            case 3:
                                //off peak
                                OffPeakKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                                OffPeakKWhCost = TypePeriod.CostcPerKWh;
                                OffPeakKWhLabel = TypePeriod.ChargeName;
                                break;
                        }
                        //standard kwh by default
                    }
                    else
                    {
                        StandKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                        StandKWhCost = TypePeriod.CostcPerKWh;
                        StandKWhLabel = TypePeriod.ChargeName;
                    }


                    //KWH findMW
                    switch (findMW)
                    {
                        case 1:
                            //always 1 import w
                            TotKWhCnt += (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);
                            TotKWCnt += (MyDataHistory.Channel1 / ConversionFactor[0]);
                            break;
                    }
                    kwhVal = (MyDataHistory.Channel1 / ConversionFactor[0]) * (MyPeriod / 60);


                    periodCnt += 1;
                    if (kwhVal > MaxKwhDemand)
                    {
                        MaxKwhDemand = kwhVal;
                        MaxKwhDemandDate = MyDataHistory.TimeStamp;
                    }
                    if (KVAVal > MaxKvaDemand)
                    {
                        MaxKvaDemand = KVAVal;
                        MaxKvaDemandDate = MyDataHistory.TimeStamp;
                    }

                }
                catch (Exception ex)
                {
                }

                // End If
            }
            MyRetCollection.Add(StartDate);
            MyRetCollection.Add(EndDate);
            MyRetCollection.Add(TotKWhCnt);


            return MyRetCollection;
        }


        private int returnMeteringPeriod(int PEriodCode)
        {
            int retval = 30;
            try
            {
                switch (PEriodCode)
                {
                    case 0:
                        retval = 1;
                        break;
                    case 1:
                        retval = 2;
                        break;
                    case 2:
                        retval = 3;
                        break;
                    case 3:
                        retval = 4;
                        break;
                    case 4:
                        retval = 5;
                        break;
                    case 5:
                        retval = 6;
                        break;
                    case 6:
                        retval = 10;
                        break;
                    case 7:
                        retval = 15;
                        break;
                    case 8:
                        retval = 20;
                        break;
                    case 9:
                        retval = 30;
                        break;
                    case 10:
                        retval = 60;

                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return retval;

        }
        private int FindPeriod(DateTime CheckTime, LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges[] ActivePeriods, ref LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges MyActiveCahrge)
        {
            int y = 0;
            //Dim myretval As LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges 'off peak by default
            try
            {
                foreach (LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges myActiveEnergyPeriod in ActivePeriods)
                {
                    if ((myActiveEnergyPeriod == null) == false)
                    {
                        if (myActiveEnergyPeriod.FindPeriod(CheckTime.Month))
                        {
                            //myActiveEnergyPeriod.ChargePeriods()
                            for (int MyPeriodcnt = 0; MyPeriodcnt <= Information.UBound(myActiveEnergyPeriod.ChargePeriods) - 1; MyPeriodcnt++)
                            {
                                LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharges = myActiveEnergyPeriod.ChargePeriods[MyPeriodcnt];
                                if (CheckPeriodDay(CheckTime, MyCharges))
                                {
                                    //day match now time match
                                    if (CheckPeriodTime(CheckTime, MyCharges))
                                    {
                                        //day match now time match
                                        //MyCharges.MeteringChargeType()
                                        MyActiveCahrge = myActiveEnergyPeriod;
                                        return MyPeriodcnt;
                                    }
                                }
                            }
                        }
                    }

                }


            }
            catch (Exception ex)
            {
            }
            return y;
        }
        private bool CheckPeriodTime(DateTime CheckTime, LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharges)
        {

            try
            {
                //Weekday so must match
                if (MyCharges.StartTime.TimeOfDay <= CheckTime.TimeOfDay & MyCharges.EndTime.TimeOfDay > CheckTime.TimeOfDay)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
            }
            return false;
        }

        private bool CheckPeriodDay(DateTime CheckTime, LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharges)
        {
            try
            {
                //everyday so must match
                if (MyCharges.Days == 7)
                {
                    return true;
                }
                //Weekday so must match
                if (MyCharges.Days == 8 & (CheckTime.DayOfWeek > 0 & (int)CheckTime.DayOfWeek < 6))
                {
                    return true;
                }
                //actual day must match
                if (MyCharges.Days < 7 & ((int)CheckTime.DayOfWeek == MyCharges.Days))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
            }
            return false;
        }

        [OperationContract()]
        public Collection GetMeteringProfileRecordHistory(int MeterID, DateTime StartDate, DateTime EndDate)
        {
            //System.Collections.Generic.List(Of Integer)
            List<LiveMonitoring.IRemoteLib.MeteringProfileRecord> MyCollection = null;
            Collection MyRetCollection = new Collection();
            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = ServerInterface.GetMeteringProfileRecord(MeterID, StartDate, EndDate);
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.MeteringProfileRecord)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.MeteringProfileRecord MyHistory = (LiveMonitoring.IRemoteLib.MeteringProfileRecord)MyObject1;
                        string RetString = "";
                        //MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                        RetString = MyHistory.TimeStamp.ToString() + "|" + MyHistory.Channel1.ToString() + "|";
                        RetString += MyHistory.Channel2.ToString() + "|";
                        RetString += MyHistory.Channel3.ToString() + "|";
                        RetString += MyHistory.Channel4.ToString() + "|";
                        RetString += MyHistory.Channel5.ToString() + "|";
                        RetString += MyHistory.Channel6.ToString() + "|";
                        RetString += MyHistory.Channel7.ToString() + "|";
                        RetString += MyHistory.Channel8.ToString() + "|";
                        RetString += MyHistory.MeterID.ToString() + "|";
                        RetString += MyHistory.Status.ToString();
                        MyRetCollection.Add(RetString);


                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return MyRetCollection;
        }
        [OperationContract()]
        public Collection GetMeteringProfileMarkerHistory(int MeterID, DateTime StartDate, DateTime EndDate)
        {
            //System.Collections.Generic.List(Of Integer)
            List<LiveMonitoring.IRemoteLib.MeteringProfileMarker> MyCollection = null;
            Collection MyRetCollection = new Collection();
            System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
            //= New List(Of Object)()
            ArrayList MySensorCollection = new ArrayList();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = ServerInterface.GetMeteringProfileMarkers(MeterID, StartDate, EndDate);
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.MeteringProfileMarker)
                {
                    try
                    {
                        LiveMonitoring.IRemoteLib.MeteringProfileMarker MyHistory = (LiveMonitoring.IRemoteLib.MeteringProfileMarker)MyObject1;
                        string RetString = "";
                        //MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                        RetString = MyHistory.TimeStamp.ToString() + "|" + MyHistory.ProfileMarkerType.ToString() + "|";
                        RetString += MyHistory.TRegisterConfigValue.ToString() + "|";
                        RetString += MyHistory.Period.ToString();
                        MyRetCollection.Add(RetString);


                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return MyRetCollection;
        }


        private List<MeteringObjects.MeteringSeasonMonths> FillChargePeriodMonths(List<LiveMonitoring.IRemoteLib.MeteringSeasonMonths> MyChargesMonths)
        {
            List<MeteringObjects.MeteringSeasonMonths> retList = new List<MeteringObjects.MeteringSeasonMonths>();
            try
            {
                foreach (LiveMonitoring.IRemoteLib.MeteringSeasonMonths MyMonth_loopVariable in MyChargesMonths)
                {
                    //MyMonth = MyMonth_loopVariable;
                    if ((MyMonth_loopVariable == null) == false)
                    {
                        MeteringObjects.MeteringSeasonMonths NewMyMonth = new MeteringObjects.MeteringSeasonMonths();
                        NewMyMonth.ID = MyMonth_loopVariable.ID;
                        NewMyMonth.MeteringSeason = new MeteringObjects.MeteringSeasonNames();
                        NewMyMonth.MeteringSeason.ID = MyMonth_loopVariable.SeasonID;
                        //NewMyMonth.MeteringSeason.SeasonNames = MyMonth.MeteringSeason.SeasonNames
                        NewMyMonth.MonthNo = MyMonth_loopVariable.MonthNo;
                        NewMyMonth.ActiveChargeID = MyMonth_loopVariable.ActiveChargeID;
                        retList.Add(NewMyMonth);
                    }

                }

            }
            catch (Exception ex)
            {
            }

            return retList;
        }
        private MeteringObjects.MeteringChargePeriods[] FillChargePeriods(LiveMonitoring.IRemoteLib.MeteringChargePeriods[] MyCharges)
        {
            MeteringObjects.MeteringChargePeriods[] retList = new MeteringObjects.MeteringChargePeriods[1];
            try
            {
                foreach (LiveMonitoring.IRemoteLib.MeteringChargePeriods MyCharge_loopVariable in MyCharges)
                {
                    //MyCharge = MyCharge_loopVariable;
                    if ((MyCharge_loopVariable == null) == false)
                    {
                        MeteringObjects.MeteringChargePeriods NewMyMonth = new MeteringObjects.MeteringChargePeriods();
                        NewMyMonth.ID = MyCharge_loopVariable.ID;
                        NewMyMonth.ActiveChargeID = MyCharge_loopVariable.ActiveChargeID;
                        NewMyMonth.Days = MyCharge_loopVariable.Days;
                        NewMyMonth.EndTime = MyCharge_loopVariable.EndTime;
                        NewMyMonth.MeteringChargeType = new MeteringObjects.MeteringChargeTypes();
                        NewMyMonth.MeteringChargeType.ChargeTypeName = MyCharge_loopVariable.MeteringChargeType.ChargeTypeName;
                        NewMyMonth.MeteringChargeType.ID = MyCharge_loopVariable.MeteringChargeType.ID;
                        NewMyMonth.StartTime = MyCharge_loopVariable.StartTime;
                        retList[retList.GetUpperBound(0)] = NewMyMonth;
                        Array.Resize(ref retList, retList.GetUpperBound(0) + 2);
                    }


                }

            }
            catch (Exception ex)
            {
            }

            return retList;
        }
        [OperationContract()]
        public string GetMeteringTarrif(int TarrifID)
        {
            //System.Collections.Generic.List(Of Integer)
            LiveMonitoring.IRemoteLib.MeteringTarrifDetails MyCollection = default(LiveMonitoring.IRemoteLib.MeteringTarrifDetails);
            //MyCollection.
            string MyRetCollection = "";
            // Dim items As New System.Collections.Generic.List(Of Integer) '= New List(Of Object)()
            // Dim MySensorCollection As New ArrayList
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = ServerInterface.GetMeteringTarrif(TarrifID);
            MeteringObjects.MeteringTarrifDetails MyObject = new MeteringObjects.MeteringTarrifDetails();
            // ERROR: Not supported in C#: ReDimStatement

            if ((MyCollection == null) == false)
            {
                if ((MyCollection.ActiveEnergyCharges == null) == false)
                {
                    foreach (LiveMonitoring.IRemoteLib.MeteringActiveEnergyCharges MyCharge_loopVariable in MyCollection.ActiveEnergyCharges)
                    {
                        //MyCharge = MyCharge_loopVariable;
                        if ((MyCharge_loopVariable == null) == false)
                        {
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)] = new MeteringObjects.MeteringActiveEnergyCharges();
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)].ID = MyCharge_loopVariable.ID;
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)].ChargeName = MyCharge_loopVariable.ChargeName;
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)].CostcPerKWh = MyCharge_loopVariable.CostcPerKWh;
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)].TariffID = MyCharge_loopVariable.TariffID;
                            // MyObject.ActiveEnergyCharges(MyObject.ActiveEnergyCharges.GetUpperBound(0)).CostcPerKWh = MyCharge.CostcPerKWh
                            //For Each MySeasonMonth In
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)].ChargePeriodMonths = FillChargePeriodMonths(MyCharge_loopVariable.ChargePeriodMonths);
                            //Next
                            MyObject.ActiveEnergyCharges[MyObject.ActiveEnergyCharges.GetUpperBound(0)].ChargePeriods = FillChargePeriods(MyCharge_loopVariable.ChargePeriods);
                            // MyObject.ActiveEnergyCharges(MyObject.ActiveEnergyCharges.GetUpperBound(0)) = NewMyMonth
                            Array.Resize(ref MyObject.ActiveEnergyCharges, MyObject.ActiveEnergyCharges.GetUpperBound(0) + 2);
                        }

                    }
                }

                // MyObject.ActiveEnergyCharges = MyCollection.ActiveEnergyCharges.Clone
                // MyObject.MeteringNetworkCharge = MyCollection.MeteringNetworkCharge.Clone
                // ERROR: Not supported in C#: ReDimStatement

                if ((MyCollection.MeteringNetworkCharge == null) == false)
                {
                    foreach (LiveMonitoring.IRemoteLib.MeteringNetworkCharges MyNetCharge_loopVariable in MyCollection.MeteringNetworkCharge)
                    {
                        //MyNetCharge = MyNetCharge_loopVariable;
                        if ((MyNetCharge_loopVariable == null) == false)
                        {
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)] = new MeteringObjects.MeteringNetworkCharges();
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].ID = MyNetCharge_loopVariable.ID;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].ChargeName = MyNetCharge_loopVariable.ChargeName;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].Columns = MyNetCharge_loopVariable.Columns;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].CostRperkVA = MyNetCharge_loopVariable.CostRperkVA;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].FixedCost = MyNetCharge_loopVariable.FixedCost;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].MaximumDemand = MyNetCharge_loopVariable.MaximumDemand;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].PenaltyCharge = MyNetCharge_loopVariable.PenaltyCharge;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].Percentage = MyNetCharge_loopVariable.Percentage;
                            MyObject.MeteringNetworkCharge[MyObject.MeteringNetworkCharge.GetUpperBound(0)].TariffID = MyNetCharge_loopVariable.TariffID;

                            // MyObject.ActiveEnergyCharges(MyObject.ActiveEnergyCharges.GetUpperBound(0)) = NewMyMonth
                            Array.Resize(ref MyObject.MeteringNetworkCharge, MyObject.MeteringNetworkCharge.GetUpperBound(0) + 2);
                        }

                    }
                }

                //MyObject.MeteringVoltageSurcharge = MyCollection.MeteringVoltageSurcharge.Clone
                // ERROR: Not supported in C#: ReDimStatement

                if ((MyCollection.MeteringVoltageSurcharge == null) == false)
                {
                    foreach (LiveMonitoring.IRemoteLib.MeteringVoltageSurcharges MyVoltCharge_loopVariable in MyCollection.MeteringVoltageSurcharge)
                    {
                        //MyVoltCharge = MyVoltCharge_loopVariable;
                        if ((MyVoltCharge_loopVariable == null) == false)
                        {
                            MyObject.MeteringVoltageSurcharge[MyObject.MeteringVoltageSurcharge.GetUpperBound(0)] = new MeteringObjects.MeteringVoltageSurcharges();
                            MyObject.MeteringVoltageSurcharge[MyObject.MeteringVoltageSurcharge.GetUpperBound(0)].ID = MyVoltCharge_loopVariable.ID;
                            MyObject.MeteringVoltageSurcharge[MyObject.MeteringVoltageSurcharge.GetUpperBound(0)].SurchargePercentage = MyVoltCharge_loopVariable.SurchargePercentage;
                            MyObject.MeteringVoltageSurcharge[MyObject.MeteringVoltageSurcharge.GetUpperBound(0)].TariffID = MyVoltCharge_loopVariable.TariffID;
                            MyObject.MeteringVoltageSurcharge[MyObject.MeteringVoltageSurcharge.GetUpperBound(0)].Voltage = MyVoltCharge_loopVariable.Voltage;
                            Array.Resize(ref MyObject.MeteringNetworkCharge, MyObject.MeteringNetworkCharge.GetUpperBound(0) + 2);
                        }

                    }
                }

                if ((MyCollection.TarrifDetails == null) == false)
                {
                    MyObject.TarrifDetails.ID = MyCollection.TarrifDetails.ID;
                    MyObject.TarrifDetails.TarriffName = MyCollection.TarrifDetails.TarriffName;
                }


                //MyRetCollection.Add(MyCollection)
                //Dim MyObject1 As Object
                //For Each MyObject1 In MyCollection
                //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.MeteringProfileMarker Then
                //        Try
                //            Dim MyHistory As LiveMonitoring.IRemoteLib.MeteringProfileMarker = CType(MyObject1, LiveMonitoring.IRemoteLib.MeteringProfileMarker)
                //            Dim RetString As String = "" 'MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
                //            RetString = MyHistory.TimeStamp.ToString + "|" + MyHistory.ProfileMarkerType.ToString + "|"
                //            RetString += MyHistory.TRegisterConfigValue.ToString + "|"
                //            RetString += MyHistory.Period.ToString


                //        Catch ex As Exception

                //        End Try
                //    End If
                //Next
            }

            XmlSerializer mySerializer = new XmlSerializer(typeof(MeteringObjects.MeteringTarrifDetails));
            MemoryStream myFile = new MemoryStream();
            // To write to a file, create a StreamWriter object.
            //Dim myWriter As StreamWriter = New StreamWriter("myFileName.xml")
            mySerializer.Serialize(myFile, MyObject);
            //Dim pos = memStream.Position
            myFile.Position = 0;
            StreamReader reader = new StreamReader(myFile);
            string MyStr = reader.ReadToEnd();
            MyRetCollection = (MyStr);
            return MyRetCollection;
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetMeteringTarrifNames()
        {
            List<LiveMonitoring.IRemoteLib.MeteringTariff> MyCollection = new List<LiveMonitoring.IRemoteLib.MeteringTariff>();
            System.Collections.Generic.List<string> MyRetCollection = new System.Collections.Generic.List<string>();
            // Dim items As New System.Collections.Generic.List(Of Integer) '= New List(Of Object)()
            // Dim MySensorCollection As New ArrayList
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = ServerInterface.GetMeteringTarrifNames();
            // Dim myObject As MySerializableClass = New MySerializableClass()
            // Insert code to set properties and fields of the object.
            foreach (LiveMonitoring.IRemoteLib.MeteringTariff MyItem in MyCollection)
            {
                MeteringObjects.MeteringTariff MyObject = new MeteringObjects.MeteringTariff();
                MyObject.ID = MyItem.ID;
                MyObject.TarriffName = MyItem.TarriffName;
                //= CType(MyItem, MeteringObjects.MeteringTariff)
                XmlSerializer mySerializer = new XmlSerializer(typeof(MeteringObjects.MeteringTariff));
                MemoryStream myFile = new MemoryStream();
                // To write to a file, create a StreamWriter object.
                //Dim myWriter As StreamWriter = New StreamWriter("myFileName.xml")
                mySerializer.Serialize(myFile, MyObject);
                //Dim pos = memStream.Position
                myFile.Position = 0;
                StreamReader reader = new StreamReader(myFile);
                string MyStr = reader.ReadToEnd();
                MyRetCollection.Add(MyStr);
                // Reset the position so that subsequent writes are correct.    memStream.Position = pos
                // myFile()
            }


            //MyRetCollection.Add(MyCollection)
            //Dim MyObject1 As Object
            //For Each MyObject1 In MyCollection
            //    If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.MeteringProfileMarker Then
            //        Try
            //            Dim MyHistory As LiveMonitoring.IRemoteLib.MeteringProfileMarker = CType(MyObject1, LiveMonitoring.IRemoteLib.MeteringProfileMarker)
            //            Dim RetString As String = "" 'MySensor.Caption + "|" + MySensor.ID.ToString + "|"""
            //            RetString = MyHistory.TimeStamp.ToString + "|" + MyHistory.ProfileMarkerType.ToString + "|"
            //            RetString += MyHistory.TRegisterConfigValue.ToString + "|"
            //            RetString += MyHistory.Period.ToString


            //        Catch ex As Exception

            //        End Try
            //    End If
            //Next

            return MyRetCollection;
        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetCameras()
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> MyCameraCollection = new System.Collections.Generic.List<string>();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                {
                    LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                    MyCameraCollection.Add(MyCamera.Caption + "|" + MyCamera.ID);
                }
            }
            return MyCameraCollection;
        }
        [OperationContract()]
        public byte[] GetCameraImage(int CameraID)
        {
            LiveMonitoring.GlobalRemoteVars MyAppRem = new LiveMonitoring.GlobalRemoteVars();
            byte[] myImageBytes = null;
            myImageBytes = MyAppRem.LiveMonServer.GetCameraImage(CameraID);
            return myImageBytes;
        }

        [OperationContract()]
        public System.Collections.Generic.List<string> GetIPDevicesDetails()
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> MyIPDevicesCollection = new System.Collections.Generic.List<string>();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                    MyIPDevicesCollection.Add(MyIPDevicesDetails.Caption + "|" + MyIPDevicesDetails.ID);
                }
            }
            return MyIPDevicesCollection;
        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetOtherDevicesDetails()
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> MyOtherDevicesCollection = new System.Collections.Generic.List<string>();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;
            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                {
                    LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                    MyOtherDevicesCollection.Add(MyOtherDevicesDetails.Caption + "|" + MyOtherDevicesDetails.ID);
                }
            }
            return MyOtherDevicesCollection;
        }
        [OperationContract()]
        public System.Collections.Generic.List<string> GetSNMPManagerDetails()
        {
            Collection MyCollection = new Collection();
            System.Collections.Generic.List<string> MySNMPDevicesCollection = new System.Collections.Generic.List<string>();
            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            MyCollection = MyRem.get_GetServerObjects();
            //server1.GetAll()
            object MyObject1 = null;

            foreach (object MyObject1_loopVariable in MyCollection)
            {
                MyObject1 = MyObject1_loopVariable;
                if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                {
                    LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                    MySNMPDevicesCollection.Add(MySNMPDevicesDetails.Caption + "|" + MySNMPDevicesDetails.ID);
                }
            }
            return MySNMPDevicesCollection;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
