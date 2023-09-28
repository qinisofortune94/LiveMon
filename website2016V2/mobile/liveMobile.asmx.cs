using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Web.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using LiveMonitoring;

namespace website2016V2.mobile
{
    /// <summary>
    /// Summary description for liveMobile
    /// </summary>
    [WebService(Namespace = "http://contoso.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class liveMobile : System.Web.Services.WebService
    {
        private static Collection LoggedInAccounts = new Collection();
        private string _path = "";
        private string _filterAttribute;
        private string _firstname;
        private string _lastname;
        private string _telephone;
        private string _email;
        private string _address;
        private string _cell;
        private string _site;
        private string _group;
        [DllImport("advapi32.dll", EntryPoint = "GetUserNameA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int GetUserName(string lpBuffer, ref int nSize);
        string conStr = WebConfigurationManager.ConnectionStrings["IPMonConnectionString"].ToString();
        string conStrReport = WebConfigurationManager.ConnectionStrings["IPMonConnectionStringReport"].ToString();
        private class UserLoggins
        {
            public LiveMonitoring.IRemoteLib.UserDetails User;
            public string UserGUID;
            public DateTime LogonDate;
        }
        public class SensorStatusItems
        {
            public int SensorID;
            public string DeviceName;
            public string Fieldname;
            public string Caption;
            public int SensorStatus;
            public double SensorFieldLastValue;
            public string SensorFieldCaption;
            public string SensorFieldsLastOtherValue;
            public int SensorFieldStatus;
            public string SensorLastError;
            public string StatusColor;
            public string StatusAlert;
            //AddValues(MyTable, MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
            // MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
            //  MyCurSensor.LastErrors.Peek)
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string HelloWorld(string MachineId)
        {
            string MyJSString = "Hello World From Roger :" + DateAndTime.Now.ToString() + " :ID:" + MachineId;
            return MyJSString;
            //Dim js As New JavaScriptSerializer()
            //Context.Response.Clear()
            //Context.Response.ContentType = "application/json"
            //'Dim data As New HelloWorldData()
            //' data.Message = "HelloWorld"
            //Context.Response.Write(js.Serialize(MyJSString))
        }

        private byte[] HexStrtoByte(string hexString)
        {
            try
            {
                //Dim hexString As String = "01050001FFFF8FFB"
                int length = hexString.Length;
                int upperBound = length - 1;
                //\ 2
                //If length Mod 2 = 0 Then
                //    upperBound -= 1
                //Else
                //    hexString = "0" & hexString
                //End If
                byte[] bytes = new byte[upperBound + 1];
                for (int i = 0; i <= upperBound - 1; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i, 1), 16);
                }
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private string DecryptStr(string cipherText)
        {
            try
            {
                if (cipherText == null || cipherText.Length <= 0)
                {
                    throw new ArgumentNullException("cipherText");
                }
                //If Key Is Nothing OrElse Key.Length <= 0 Then
                //    Throw New ArgumentNullException("Key")
                //End If
                //If IV Is Nothing OrElse IV.Length <= 0 Then
                //    Throw New ArgumentNullException("Key")
                //End If
                // Declare the string used to hold
                // the decrypted text.
                // //var key = CryptoJS.enc.Hex.parse('1023546798abcdef');
                ////var iv = CryptoJS.enc.Hex.parse('efdcaba9876543201');
                byte[] Key = HexStrtoByte("1023546798abcdef");
                byte[] IV = HexStrtoByte("efdcaba9876543201");
                string plaintext = null;

                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.ToInt32(cipherText)))
                    {

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {


                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private string DecryptStringAES(string StringtoDecrypt)
        {
            //Dim Key() As Byte = HexStrtoByte("1023546798abcdef")
            // Dim IV() As Byte = HexStrtoByte("efdcaba9876543201")
            if (string.IsNullOrEmpty(StringtoDecrypt))
            {
                return null;
            }
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");
            //Dim Keybytes() As Byte = Convert.FromBase64String("4536538a2f826d756115ceed14569e9eefe97a4c83dadc5b176b5af535a73148")
            // Dim ivb() As Byte = Convert.FromBase64String("6352a4c2cff80677")
            //c# encrrption
            // Dim encryptStringToBytes__1 = EncryptStringToBytes("It works", keybytes, iv)

            // Decrypt the bytes to a string.
            //  Dim roundtrip = DecryptStringFromBytes(encryptStringToBytes__1, keybytes, iv)

            //DECRYPT FROM CRIPTOJS
            var encrypted = Convert.FromBase64String(StringtoDecrypt);
            //"+Ijpt1GDVgM4MqMAQUwf0Q==")
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            //Dim decriptedFromJavascript = DecryptStringFromBytes(encrypted, Key, IV)
            return decriptedFromJavascript;
        }

        private string EncryptStringAES(string StringtoEncrypt)
        {
            //Dim Key() As Byte = HexStrtoByte("1023546798abcdef")
            // Dim IV() As Byte = HexStrtoByte("efdcaba9876543201")
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");
            //Dim Keybytes() As Byte = Convert.FromBase64String("4536538a2f826d756115ceed14569e9eefe97a4c83dadc5b176b5af535a73148")
            // Dim ivb() As Byte = Convert.FromBase64String("6352a4c2cff80677")
            //c# encrrption
            var encryptStringToBytes__1 = EncryptStringToBytes(StringtoEncrypt, keybytes, iv);

            // Decrypt the bytes to a string.
            //  Dim roundtrip = DecryptStringFromBytes(encryptStringToBytes__1, keybytes, iv)

            //DECRYPT FROM CRIPTOJS
            var encrypted = Convert.ToBase64String(encryptStringToBytes__1);
            //"+Ijpt1GDVgM4MqMAQUwf0Q==")
            // Dim decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv)
            //Dim decriptedFromJavascript = DecryptStringFromBytes(encrypted, Key, IV)
            return encrypted;
        }
        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {

                try
                {
                    //                keySize: 128 / 8,
                    //iv:             iv,
                    //            mode : CryptoJS.mode.CBC,
                    //            padding : CryptoJS.pad.Pkcs7()
                    //Settings
                    rijAlg.Mode = CipherMode.CBC;
                    rijAlg.Padding = PaddingMode.PKCS7;
                    rijAlg.FeedbackSize = 128;

                    rijAlg.Key = key;
                    rijAlg.IV = iv;

                    // Create a decrytor to perform the stream transform.
                    var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                }

            }

            return plaintext;
        }
        private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted = null;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private string GenerateGUID()
        {


            //Dim sGUID As String
            return System.Guid.NewGuid().ToString();
            //MessageBox.Show(sGUID)


        }

        
        //generate a token pass back
        //all susequent calls must have the matching token else login again
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Login2Service(string usernamein, string passwordin)
        {
            //Decrypt User & Password
            string UserName = DecryptStringAES(usernamein);
            string Password = DecryptStringAES(passwordin);
            //test valid


            LiveMonitoring.IRemoteLib.UserDetails MyUser = new LiveMonitoring.IRemoteLib.UserDetails();

            LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
            //Dim mgr As LiveMonitoring.IRemoteLib
            //Dim server1 As LiveMonitoring.IRemoteLib
            //server1 = CType(Activator.GetObject(GetType(LiveMonitoring.IRemoteLib), MyRem.GetAppSetting("Remote.Settings")), LiveMonitoring.IRemoteLib)

            LiveMonitoring.testing test = new LiveMonitoring.testing();
            try
            {
                if (UserName.Contains("\\"))
                {
                    if ((IsAuthenticated(Domain.GetCurrentDomain().Name, test.GetUser(), Password) == true))
                    {
                        MyUser = MyRem.LiveMonServer.TryLogin(UserName, MyRem.GetEncrypted(Password), _firstname, _lastname, _email, _site);
                    }
                    else
                    {
                        MyUser = MyRem.LiveMonServer.TryLogin(UserName, MyRem.GetEncrypted(Password), "", "", "", "");
                    }
                }
                else
                {
                    MyUser = MyRem.LiveMonServer.TryLogin(UserName, MyRem.GetEncrypted(Password), "", "", "", "");
                }


            }
            catch (Exception ex)
            {
            }
            if ((MyUser == null) == false)
            {
                if (MyUser.ID != 0)
                {
                    UserLoggins MyLogin = new UserLoggins();
                    MyLogin.User = MyUser;

                    MyLogin.UserGUID = System.Guid.NewGuid().ToString();
                    MyLogin.LogonDate = DateAndTime.Now;
                    CleanupAccounts();

                    if (LoggedInAccounts.Contains(MyLogin.UserGUID) == false)
                    {
                        LoggedInAccounts.Add(MyLogin, MyLogin.UserGUID);
                    }
                    return (EncryptStringAES(MyLogin.UserGUID));
                }
                else
                {
                    return (EncryptStringAES("Logon FAILED :" + DateAndTime.Now.ToString()));
                }
            }

            return "Hey you said :" + UserName + ":" + Password + DateTime.Now.ToString();

        }
        //authenticate user using active directory.
        private bool IsAuthenticated(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + "\\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.			
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                //full names
                search.PropertiesToLoad.Add("givenname");
                // firstname
                search.PropertiesToLoad.Add("sn");
                // lastname
                search.PropertiesToLoad.Add("mail");
                // mail
                search.PropertiesToLoad.Add("ou");

                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return false;
                }
                string nd = "";
                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
                _firstname = (string)result.Properties["givenname"][0];
                _lastname = (string)result.Properties["sn"][0];
                try
                {
                    _email = (string)result.Properties["mail"][0];
                }
                catch
                {
                }
                try
                {
                    _site = (string)result.Properties["ou"][0];
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }

        private void CleanupAccounts()
        {
            try
            {
                Collection removelist = new Collection();

                foreach (UserLoggins MyLogin in LoggedInAccounts)
                {
                    if (DateAndTime.DateDiff(DateInterval.Hour, DateAndTime.Now, MyLogin.LogonDate) > 2)
                    {
                        removelist.Add(MyLogin.UserGUID);
                    }
                }
                foreach (object Item_loopVariable in removelist)
                {
                    //Item = Item_loopVariable;
                    LoggedInAccounts.Remove(Convert.ToString(Item_loopVariable));
                }

            }
            catch (Exception ex)
            {
            }
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Sites.Site.SiteTypeObj> GetUserSites(string tokensent)
        {
            //Decrypt User & Password
            if (string.IsNullOrEmpty(tokensent))
                return null;
            string Token = DecryptStringAES(tokensent);
            if (LoggedInAccounts.Contains(Token) == true)
            {
                UserLoggins MyLogin = (UserLoggins)LoggedInAccounts[Token];
                Sites RetSites = new Sites(MyLogin.User.ID);
                List<Sites.Site.SiteTypeObj> retlist = new List<Sites.Site.SiteTypeObj>();
                foreach (Sites.Site retSite in RetSites.SitesList)
                {
                    retlist.Add(retSite.SiteObj);
                }
                // Dim retStatusstr As List(Of SensorStatusItems) = BuildStatusObjects(Filters, SiteID, FilterText)
                return retlist;
                //Return GetWorksOrderList(CType(LoggedInAccounts(Token), UserLoggins))
            }
            else
            {
                return null;
            }
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<SensorStatusItems> GetAllStatus(string tokensent)
        {
            //Decrypt User & Password
            if (string.IsNullOrEmpty(tokensent))
                return null;
            string Token = DecryptStringAES(tokensent);
            if (LoggedInAccounts.Contains(Token) == true)
            {
                List<SensorStatusItems> retStatusstr = BuildAllStatusObjects();
                return retStatusstr;
                //Return GetWorksOrderList(CType(LoggedInAccounts(Token), UserLoggins))
            }
            else
            {
                return null;
            }
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<SensorStatusItems> GetFilteredStatus(string tokensent, object[] Filters, int SiteID, string FilterText)
        {
            //Decrypt User & Password
            if (string.IsNullOrEmpty(tokensent))
                return null;
            string Token = DecryptStringAES(tokensent);
            if (LoggedInAccounts.Contains(Token) == true)
            {
                List<SensorStatusItems> retStatusstr = BuildStatusObjects(Filters, SiteID, FilterText);
                return retStatusstr;
                //Return GetWorksOrderList(CType(LoggedInAccounts(Token), UserLoggins))
            }
            else
            {
                return null;
            }
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<LiveMonitoring.IRemoteLib.AlertHistory> GetAlertHistory(string tokensent, DateTime StartDate, DateTime EndDate)
        {
            //Decrypt User & Password
            if (string.IsNullOrEmpty(tokensent))
                return null;
            string Token = DecryptStringAES(tokensent);
            if (LoggedInAccounts.Contains(Token) == true)
            {
                Collection MyCollection = new Collection();
                List<LiveMonitoring.IRemoteLib.AlertHistory> MyRetCollection = new List<LiveMonitoring.IRemoteLib.AlertHistory>();
                ArrayList MySensorCollection = new ArrayList();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.LiveMonServer.GetAllAlertHistoryByDate(StartDate, EndDate);
                object MyObject1 = null;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.AlertHistory)
                    {
                        try
                        {
                            LiveMonitoring.IRemoteLib.AlertHistory MyHistory = (LiveMonitoring.IRemoteLib.AlertHistory)MyObject1;
                            MyRetCollection.Add(MyHistory);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                return MyRetCollection;
            }
            else
            {
                return null;
            }
        }

        public class ReturnDataHistory
        {
            public int Fieldno;
            public double Value;
            public string OtherData;
            public System.DateTime DataDate;
            public int Status;
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<LiveMonitoring.IRemoteLib.DataHistory> GetSensorHistory(string tokensent, int SensorID, DateTime StartDate, DateTime EndDate)
        {
            //System.Collections.Generic.List(Of Integer)
            //Decrypt User & Password
            if (string.IsNullOrEmpty(tokensent))
                return null;
            string Token = DecryptStringAES(tokensent);
            if (LoggedInAccounts.Contains(Token) == true)
            {
                Collection MyCollection = new Collection();
                List<LiveMonitoring.IRemoteLib.DataHistory> MyRetCollection = new List<LiveMonitoring.IRemoteLib.DataHistory>();
                System.Collections.Generic.List<int> items = new System.Collections.Generic.List<int>();
                //= New List(Of Object)()
                ArrayList MySensorCollection = new ArrayList();
                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                MyCollection = MyRem.LiveMonServer.GetSensorHistory(SensorID, StartDate, EndDate);
                object MyObject1 = null;
                foreach (object MyObject1_loopVariable in MyCollection)
                {
                    MyObject1 = MyObject1_loopVariable;
                    if (MyObject1 is LiveMonitoring.IRemoteLib.DataHistory)
                    {
                        try
                        {
                            LiveMonitoring.IRemoteLib.DataHistory MyHistory = (LiveMonitoring.IRemoteLib.DataHistory)MyObject1;
                            MyRetCollection.Add(MyHistory);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                return MyRetCollection;
            }
            else
            {
                return null;
            }

        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public LiveMonitoring.IRemoteLib.SensorDetails GetSensor(string tokensent, int SensorID)
        {
            LiveMonitoring.IRemoteLib.SensorDetails functionReturnValue = default(LiveMonitoring.IRemoteLib.SensorDetails);
            string Token = DecryptStringAES(tokensent);
            if (LoggedInAccounts.Contains(Token) == true)
            {
                try
                {
                    LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                    LiveMonitoring.IRemoteLib.SensorDetails MySensor = MyRem.LiveMonServer.GetSpecificSensor(SensorID);
                    if ((MySensor == null) == false)
                    {
                        //kill images for jason
                        MySensor.ImageError = null;
                        MySensor.ImageErrorByte = null;
                        MySensor.ImageNoResponse = null;
                        MySensor.ImageNoResponseByte = null;
                        MySensor.ImageNormal = null;
                        MySensor.ImageNormalByte = null;
                        return MySensor;
                        return functionReturnValue;
                    }

                }
                catch (Exception ex)
                {
                }
                return null;
            }
            else
            {
                return null;
            }
            return functionReturnValue;

        }

        private bool doesSearchTermMatchCaption(string SearchTerm, string Caption)
        {
            if (Caption.ToUpper().Contains(SearchTerm.ToUpper()))
            {
                return true;
            }
            return false;
        }
        private bool CheckFilter(object[] filters, string CheckedFilter)
        {
            bool functionReturnValue = false;
            foreach (object myFilter in filters)
            {
                if (myFilter.ToString().Contains(CheckedFilter))
                {
                    return true;
                    return functionReturnValue;
                }
            }
            return false;
            return functionReturnValue;
        }
        private void SetStatusColorAlert(ref SensorStatusItems thisStatus)
        {
            switch (thisStatus.SensorStatus)
            {
                //MyStatus

                case (int)LiveMonitoring.IRemoteLib.StatusDef.alert:
                    thisStatus.StatusAlert = "Alert";
                    thisStatus.StatusColor = "Maroon";
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.criticalerror:
                    thisStatus.StatusAlert = "Critical";
                    thisStatus.StatusColor = "Red";
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.devicefailure:
                    thisStatus.StatusAlert = "Device failure";
                    thisStatus.StatusColor = "Purple";
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.disabled:
                    thisStatus.StatusAlert = "Disabled";
                    thisStatus.StatusColor = "Gray";
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.noresponse:
                    thisStatus.StatusAlert = "No Response";
                    thisStatus.StatusColor = "Fuchsia";
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.notify:
                    thisStatus.StatusAlert = "Notify";
                    thisStatus.StatusColor = "Navy";
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.ok:
                    thisStatus.StatusAlert = "OK";
                    thisStatus.StatusColor = "Green";
                    if (thisStatus.SensorFieldStatus != (int)LiveMonitoring.IRemoteLib.FieldStatusDef.ok)
                    {
                        if (thisStatus.SensorFieldStatus == (int)LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                        {
                            thisStatus.StatusAlert = "Sensor Warning";
                            thisStatus.StatusColor = "Orange";
                        }
                        else if (thisStatus.SensorFieldStatus == (int)LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                        {
                            thisStatus.StatusAlert = "Sensor Alert";
                            thisStatus.StatusColor = "Red";
                        }
                    }
                    break;
                case (int)LiveMonitoring.IRemoteLib.StatusDef.statuserror:
                    thisStatus.StatusColor = "Maroon";
                    thisStatus.StatusAlert = "Yellow";
                    break;
                default:
                    thisStatus.StatusAlert = "Unknown";
                    thisStatus.StatusColor = "White";
                    break;
            }
        }
        private List<SensorStatusItems> BuildAllStatusObjects()
        {
            try
            {
                List<SensorStatusItems> MyTable = new List<SensorStatusItems>();
                //Hold all the sensor ID's
                System.Collections.Generic.List<int> selectSensors = new System.Collections.Generic.List<int>();

                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                Collection MyCameraCollection = new Collection();
                Collection MySensorCollection = new Collection();
                Collection MySensorGroupCollection = new Collection();
                Collection MyIPDevicesCollection = new Collection();
                Collection MyOtherDevicesCollection = new Collection();
                Collection MySNMPDevicesCollection = new Collection();

                //GetServerObjects 'server1.GetAll()
                MyCollection = MyRem.get_GetServerObjects(null);
                object MyObject1 = null;
                bool HasSearchTerm = false;

                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                MyCameraCollection.Add(MyCamera);
                            }
                            //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                            //    Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
                            //    MySensorCollection.Add(MySensor)
                            //End If
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                bool isAdded = false;
                                //Ok

                                MySensorCollection.Add(MySensor);

                            }



                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                            {
                                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;

                                MySensorGroupCollection.Add(MySensorGroup, MySensorGroup.SensorGroupID.ToString());
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                                MyIPDevicesCollection.Add(MyIPDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                                MyOtherDevicesCollection.Add(MyOtherDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                            {
                                LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                                MySNMPDevicesCollection.Add(MySNMPDevicesDetails);
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }

                //Try
                //    FillSites()
                //Catch ex As Exception

                //End Try

                Collection MyFilteredSensors = new Collection();
                //= MySensorCollection

                //For MyCntSensor As Integer = 1 To MySensorCollection.Count
                //    Dim MySensObj1 As LiveMonitoring.IRemoteLib.SensorDetails = MySensorCollection.Item(MyCntSensor)
                //    MyFilteredSensors.Add(MySensObj1)
                //Next

                try
                {
                    if (selectSensors.Count > 0)
                    {
                        MyFilteredSensors.Clear();
                        //LiveMonitoring.IRemoteLib.SensorDetails MySensObj = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensObj in MySensorCollection)
                        {
                            try
                            {
                                if (selectSensors.Contains(MySensObj.ID))
                                {
                                    MyFilteredSensors.Add(MySensObj);
                                }

                            }
                            catch (Exception ex)
                            {
                            }

                        }
                    }
                    else
                    {
                        MyFilteredSensors = MySensorCollection;
                    }

                }
                catch (Exception ex)
                {
                }


                LiveMonitoring.IRemoteLib.SensorDetails MyCurSensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                string MyDeviceName = "";
                //BackColor="#C1D2EE" BorderColor="#316AC5"
                // MyTable = "<table id=""myTable"" BorderColor=""#316AC5""><thead><tr bgcolor=""#C1D2EE""><td>Icon</td><td>Device</td><td>Sensor</td><td>Field</td><td>Alert</td><td>Value</td><td>Extra Value</td></tr></thead><tbody>"
                // MaxNo.Value = MyFilteredSensors.Count.ToString

                if (MyFilteredSensors.Count > 0)
                {
                    //MySensorCollection.Count
                    for (int MyCntSensor = 1; MyCntSensor <= MyFilteredSensors.Count; MyCntSensor++)
                    {
                        try
                        {
                            //If MyCntSensor >= CInt(StartNo.Value) And MyCntSensor <= CInt(EndNo.Value) Then
                            MyCurSensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyFilteredSensors[MyCntSensor];
                            MyDeviceName = "";
                            //LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
                            foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails in MyIPDevicesCollection)
                            {
                                if (MyCurSensor.IPDeviceID == MyIPDevicesDetails.ID)
                                {
                                    MyDeviceName = MyIPDevicesDetails.Caption;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
                                foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails in MyOtherDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyOtherDevicesDetails.ID)
                                    {
                                        MyDeviceName = MyOtherDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
                                foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails in MySNMPDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MySNMPDevicesDetails.ID)
                                    {
                                        MyDeviceName = MySNMPDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.CameraDetails MyCamera = default(LiveMonitoring.IRemoteLib.CameraDetails);
                                foreach (LiveMonitoring.IRemoteLib.CameraDetails MyCamera in MyCameraCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyCamera.ID)
                                    {
                                        MyDeviceName = MyCamera.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MyCurSensor.Fields)
                            {
                                SensorStatusItems MyaddStatus = new SensorStatusItems();
                                try
                                {
                                    if (MyFields.DisplayValue)
                                    {
                                        if (MyCurSensor.LastErrors.Count > 0)
                                        {
                                            MyaddStatus.SensorID = MyCurSensor.ID;
                                            MyaddStatus.DeviceName = MyDeviceName;
                                            MyaddStatus.Fieldname = MyFields.FieldName;
                                            MyaddStatus.Caption = MyCurSensor.Caption;
                                            MyaddStatus.SensorStatus = (int)MyCurSensor.Status;
                                            MyaddStatus.SensorFieldLastValue = MyFields.LastValue;
                                            MyaddStatus.SensorFieldCaption = MyFields.Caption;
                                            MyaddStatus.SensorFieldsLastOtherValue = MyFields.LastOtherValue;
                                            MyaddStatus.SensorFieldStatus = (int)MyFields.FieldStatus;
                                            MyaddStatus.SensorLastError = MyCurSensor.LastErrors.Peek();
                                            //AddValues(MyTable, .ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                            //      MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                            //      MyCurSensor.LastErrors.Peek)

                                        }
                                        else
                                        {
                                            MyaddStatus.SensorID = MyCurSensor.ID;
                                            MyaddStatus.DeviceName = MyDeviceName;
                                            MyaddStatus.Fieldname = MyFields.FieldName;
                                            MyaddStatus.Caption = MyCurSensor.Caption;
                                            MyaddStatus.SensorStatus = (int)MyCurSensor.Status;
                                            MyaddStatus.SensorFieldLastValue = MyFields.LastValue;
                                            MyaddStatus.SensorFieldCaption = MyFields.Caption;
                                            MyaddStatus.SensorFieldsLastOtherValue = MyFields.LastOtherValue;
                                            MyaddStatus.SensorFieldStatus = (int)MyFields.FieldStatus;
                                            MyaddStatus.SensorLastError = "";
                                            //                                        AddValues(MyTable, MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                            //                                            MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                            //)
                                        }
                                        //AddValues(MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                        //          MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                        //          IIf(MyCurSensor.LastErrors.Count > 0, MyCurSensor.LastErrors.Peek, ""))
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //lblerr.Visible = True
                                    //lblerr.Text = ex.Message
                                    MyaddStatus.SensorID = MyCurSensor.ID;
                                    MyaddStatus.DeviceName = MyDeviceName;
                                    MyaddStatus.Fieldname = MyFields.FieldName;
                                    MyaddStatus.Caption = MyCurSensor.Caption;
                                    MyaddStatus.SensorStatus = (int)MyCurSensor.Status;
                                    MyaddStatus.SensorFieldLastValue = MyFields.LastValue;
                                    MyaddStatus.SensorFieldCaption = MyFields.Caption;
                                    MyaddStatus.SensorFieldsLastOtherValue = MyFields.LastOtherValue;
                                    MyaddStatus.SensorFieldStatus = (int)MyFields.FieldStatus;
                                    //                                AddValues(MyTable, MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                    //                                          MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                    //)
                                }
                                SetStatusColorAlert(ref MyaddStatus);
                                MyTable.Add(MyaddStatus);
                            }

                            // End If
                        }
                        catch (Exception ex)
                        {
                            //TODO: Add a message to the table maybe if an error has occured?
                        }

                    }
                }
                else
                {
                    //myFilterSensor.Count is less than 0
                    // MyTable += "<tr><td colspan=""5"" style=""color:red"">No Sensors found.</td></tr>"
                    return null;
                }
                // MyTable += "</tbody></table>"
                return MyTable;
            }
            catch (Exception ex)
            {
                //lblerr.Visible = True
                //lblerr.Text = ex.Message
                return null;
            }
        }

        private List<SensorStatusItems> BuildStatusObjects(object[] filters, int SiteID, string FilterText)
        {
            try
            {
                List<SensorStatusItems> MyTable = new List<SensorStatusItems>();
                //Hold all the sensor ID's
                System.Collections.Generic.List<int> selectSensors = new System.Collections.Generic.List<int>();

                LiveMonitoring.GlobalRemoteVars MyRem = new LiveMonitoring.GlobalRemoteVars();
                Collection MyCollection = new Collection();
                Collection MyCameraCollection = new Collection();
                Collection MySensorCollection = new Collection();
                Collection MySensorGroupCollection = new Collection();
                Collection MyIPDevicesCollection = new Collection();
                Collection MyOtherDevicesCollection = new Collection();
                Collection MySNMPDevicesCollection = new Collection();
                int zero = 0;
                //GetServerObjects 'server1.GetAll()
                MyCollection = MyRem.get_GetServerObjects((SiteID == 0 ? (int?)null : SiteID));
                object MyObject1 = null;
                bool HasSearchTerm = false;
                if ((!(FilterText == null)) | (FilterText.Trim().Length > 0))
                {
                    HasSearchTerm = true;
                }
                if ((MyCollection == null) == false)
                {
                    foreach (object MyObject1_loopVariable in MyCollection)
                    {
                        MyObject1 = MyObject1_loopVariable;
                        try
                        {
                            if (MyObject1 is LiveMonitoring.IRemoteLib.CameraDetails)
                            {
                                LiveMonitoring.IRemoteLib.CameraDetails MyCamera = (LiveMonitoring.IRemoteLib.CameraDetails)MyObject1;
                                if (HasSearchTerm)
                                {
                                    if (doesSearchTermMatchCaption(FilterText, MyCamera.Caption))
                                    {
                                        MyCameraCollection.Add(MyCamera);
                                    }
                                }
                                else
                                {
                                    MyCameraCollection.Add(MyCamera);
                                }
                            }
                            //If TypeOf MyObject1 Is LiveMonitoring.IRemoteLib.SensorDetails Then
                            //    Dim MySensor As LiveMonitoring.IRemoteLib.SensorDetails = CType(MyObject1, LiveMonitoring.IRemoteLib.SensorDetails)
                            //    MySensorCollection.Add(MySensor)
                            //End If
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorDetails)
                            {
                                LiveMonitoring.IRemoteLib.SensorDetails MySensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyObject1;
                                bool isAdded = false;
                                //Ok
                                //filters = { 0: "filter.ok", 1: "filter.error", 2: "filter.noresponse", 3: "filter.alert", 4: "filter.warning", 5: "filter.unknown", 6: "filter.sensorwarning", 7: "filter.sensoralert" };

                                if (CheckFilter(filters, "filter.ok") & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.ok)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //error
                                if (CheckFilter(filters, "filter.error") & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.criticalerror)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //No response
                                if (CheckFilter(filters, "filter.noresponse") & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.noresponse)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //alert
                                if (CheckFilter(filters, "filter.alert") & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.alert)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //warning
                                if (CheckFilter(filters, "filter.warning") & isAdded == false)
                                {
                                    if (MySensor.Status == LiveMonitoring.IRemoteLib.StatusDef.statuserror)
                                    {
                                        if (HasSearchTerm)
                                        {
                                            if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                        else
                                        {
                                            MySensorCollection.Add(MySensor);
                                            isAdded = true;
                                        }
                                    }
                                }
                                //'unknown
                                //"filter.unknown"
                                //If chkFilter_OK.Checked And isAdded = False Then
                                //    If MySensor.Status = LiveMonitoring.IRemoteLib.StatusDef.ok Then
                                //        MySensorCollection.Add(MySensor)
                                //        isAdded = True
                                //    End If
                                //End If

                                // Sensor Warning.
                                if (CheckFilter(filters, "filter.sensorwarning") & isAdded == false)
                                {
                                    //LiveMonitoring.IRemoteLib.FieldStatusDef myFields = new LiveMonitoring.IRemoteLib.FieldStatusDef();
                                    foreach (LiveMonitoring.IRemoteLib.FieldStatusDef myFields in MySensor.Fields)
                                    {
                                        if (myFields == LiveMonitoring.IRemoteLib.FieldStatusDef.warning)
                                        {
                                            if (HasSearchTerm)
                                            {
                                                if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                                {
                                                    MySensorCollection.Add(MySensor);
                                                    isAdded = true;
                                                }
                                            }
                                            else
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                    }
                                }
                                //If chkFilter_SensorAlert.Checked And isAdded = False Then
                                //    Dim myFields As New LiveMonitoring.IRemoteLib.FieldStatusDef
                                //    For Each myFields In MySensor.Fields
                                //        If myFields = LiveMonitoring.IRemoteLib.FieldStatusDef.alert Then
                                //            MySensorCollection.Add(MySensor)
                                //            isAdded = True
                                //        End If
                                //    Next
                                //End If
                                // Sensor Alert.
                                if (CheckFilter(filters, "filter.sensoralert") & isAdded == false)
                                {
                                    //LiveMonitoring.IRemoteLib.FieldStatusDef myFields = new LiveMonitoring.IRemoteLib.FieldStatusDef();
                                    foreach (LiveMonitoring.IRemoteLib.FieldStatusDef myFields in MySensor.Fields)
                                    {
                                        if (myFields == LiveMonitoring.IRemoteLib.FieldStatusDef.alert)
                                        {
                                            if (HasSearchTerm)
                                            {
                                                if (doesSearchTermMatchCaption(FilterText, MySensor.Caption))
                                                {
                                                    MySensorCollection.Add(MySensor);
                                                    isAdded = true;
                                                }
                                            }
                                            else
                                            {
                                                MySensorCollection.Add(MySensor);
                                                isAdded = true;
                                            }
                                        }
                                    }
                                }

                            }



                            if (MyObject1 is LiveMonitoring.IRemoteLib.SensorGroup)
                            {
                                LiveMonitoring.IRemoteLib.SensorGroup MySensorGroup = (LiveMonitoring.IRemoteLib.SensorGroup)MyObject1;

                                MySensorGroupCollection.Add(MySensorGroup, MySensorGroup.SensorGroupID.ToString());
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.IPDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = (LiveMonitoring.IRemoteLib.IPDevicesDetails)MyObject1;
                                MyIPDevicesCollection.Add(MyIPDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.OtherDevicesDetails)
                            {
                                LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = (LiveMonitoring.IRemoteLib.OtherDevicesDetails)MyObject1;
                                MyOtherDevicesCollection.Add(MyOtherDevicesDetails);
                            }
                            if (MyObject1 is LiveMonitoring.IRemoteLib.SNMPManagerDetails)
                            {
                                LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = (LiveMonitoring.IRemoteLib.SNMPManagerDetails)MyObject1;
                                MySNMPDevicesCollection.Add(MySNMPDevicesDetails);
                            }

                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }

                //Try
                //    FillSites()
                //Catch ex As Exception

                //End Try

                Collection MyFilteredSensors = new Collection();
                //= MySensorCollection

                //For MyCntSensor As Integer = 1 To MySensorCollection.Count
                //    Dim MySensObj1 As LiveMonitoring.IRemoteLib.SensorDetails = MySensorCollection.Item(MyCntSensor)
                //    MyFilteredSensors.Add(MySensObj1)
                //Next

                try
                {
                    if (selectSensors.Count > 0)
                    {
                        MyFilteredSensors.Clear();
                        //LiveMonitoring.IRemoteLib.SensorDetails MySensObj = default(LiveMonitoring.IRemoteLib.SensorDetails);
                        foreach (LiveMonitoring.IRemoteLib.SensorDetails MySensObj in MySensorCollection)
                        {
                            try
                            {
                                if (selectSensors.Contains(MySensObj.ID))
                                {
                                    MyFilteredSensors.Add(MySensObj);
                                }

                            }
                            catch (Exception ex)
                            {
                            }

                        }
                    }
                    else
                    {
                        MyFilteredSensors = MySensorCollection;
                    }

                }
                catch (Exception ex)
                {
                }


                LiveMonitoring.IRemoteLib.SensorDetails MyCurSensor = default(LiveMonitoring.IRemoteLib.SensorDetails);
                string MyDeviceName = "";
                //BackColor="#C1D2EE" BorderColor="#316AC5"
                // MyTable = "<table id=""myTable"" BorderColor=""#316AC5""><thead><tr bgcolor=""#C1D2EE""><td>Icon</td><td>Device</td><td>Sensor</td><td>Field</td><td>Alert</td><td>Value</td><td>Extra Value</td></tr></thead><tbody>"
                // MaxNo.Value = MyFilteredSensors.Count.ToString

                if (MyFilteredSensors.Count > 0)
                {
                    //MySensorCollection.Count
                    for (int MyCntSensor = 1; MyCntSensor <= MyFilteredSensors.Count; MyCntSensor++)
                    {
                        try
                        {
                            //If MyCntSensor >= CInt(StartNo.Value) And MyCntSensor <= CInt(EndNo.Value) Then
                            MyCurSensor = (LiveMonitoring.IRemoteLib.SensorDetails)MyFilteredSensors[MyCntSensor];
                            MyDeviceName = "";
                            //LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails = default(LiveMonitoring.IRemoteLib.IPDevicesDetails);
                            foreach (LiveMonitoring.IRemoteLib.IPDevicesDetails MyIPDevicesDetails in MyIPDevicesCollection)
                            {
                                if (MyCurSensor.IPDeviceID == MyIPDevicesDetails.ID)
                                {
                                    MyDeviceName = MyIPDevicesDetails.Caption;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails = default(LiveMonitoring.IRemoteLib.OtherDevicesDetails);
                                foreach (LiveMonitoring.IRemoteLib.OtherDevicesDetails MyOtherDevicesDetails in MyOtherDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyOtherDevicesDetails.ID)
                                    {
                                        MyDeviceName = MyOtherDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails = default(LiveMonitoring.IRemoteLib.SNMPManagerDetails);
                                foreach (LiveMonitoring.IRemoteLib.SNMPManagerDetails MySNMPDevicesDetails in MySNMPDevicesCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MySNMPDevicesDetails.ID)
                                    {
                                        MyDeviceName = MySNMPDevicesDetails.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(MyDeviceName))
                            {
                                //LiveMonitoring.IRemoteLib.CameraDetails MyCamera = default(LiveMonitoring.IRemoteLib.CameraDetails);
                                foreach (LiveMonitoring.IRemoteLib.CameraDetails MyCamera in MyCameraCollection)
                                {
                                    if (MyCurSensor.IPDeviceID == MyCamera.ID)
                                    {
                                        MyDeviceName = MyCamera.Caption;
                                        break; // TODO: might not be correct. Was : Exit For
                                    }
                                }
                            }
                            //LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields = default(LiveMonitoring.IRemoteLib.SensorFieldsDef);
                            foreach (LiveMonitoring.IRemoteLib.SensorFieldsDef MyFields in MyCurSensor.Fields)
                            {
                                SensorStatusItems MyaddStatus = new SensorStatusItems();
                                try
                                {
                                    if (MyFields.DisplayValue)
                                    {
                                        if (MyCurSensor.LastErrors.Count > 0)
                                        {
                                            MyaddStatus.SensorID = MyCurSensor.ID;
                                            MyaddStatus.DeviceName = MyDeviceName;
                                            MyaddStatus.Fieldname = MyFields.FieldName;
                                            MyaddStatus.Caption = MyCurSensor.Caption;
                                            MyaddStatus.SensorStatus = (int)MyCurSensor.Status;
                                            MyaddStatus.SensorFieldLastValue = MyFields.LastValue;
                                            MyaddStatus.SensorFieldCaption = MyFields.Caption;
                                            MyaddStatus.SensorFieldsLastOtherValue = MyFields.LastOtherValue;
                                            MyaddStatus.SensorFieldStatus = (int)MyFields.FieldStatus;
                                            MyaddStatus.SensorLastError = MyCurSensor.LastErrors.Peek();
                                            //AddValues(MyTable, .ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                            //      MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                            //      MyCurSensor.LastErrors.Peek)

                                        }
                                        else
                                        {
                                            MyaddStatus.SensorID = MyCurSensor.ID;
                                            MyaddStatus.DeviceName = MyDeviceName;
                                            MyaddStatus.Fieldname = MyFields.FieldName;
                                            MyaddStatus.Caption = MyCurSensor.Caption;
                                            MyaddStatus.SensorStatus = (int)MyCurSensor.Status;
                                            MyaddStatus.SensorFieldLastValue = MyFields.LastValue;
                                            MyaddStatus.SensorFieldCaption = MyFields.Caption;
                                            MyaddStatus.SensorFieldsLastOtherValue = MyFields.LastOtherValue;
                                            MyaddStatus.SensorFieldStatus = (int)MyFields.FieldStatus;
                                            MyaddStatus.SensorLastError = "";
                                            //                                        AddValues(MyTable, MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                            //                                            MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                            //)
                                        }
                                        //AddValues(MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                        //          MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                        //          IIf(MyCurSensor.LastErrors.Count > 0, MyCurSensor.LastErrors.Peek, ""))
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //lblerr.Visible = True
                                    //lblerr.Text = ex.Message
                                    MyaddStatus.SensorID = MyCurSensor.ID;
                                    MyaddStatus.DeviceName = MyDeviceName;
                                    MyaddStatus.Fieldname = MyFields.FieldName;
                                    MyaddStatus.Caption = MyCurSensor.Caption;
                                    MyaddStatus.SensorStatus = (int)MyCurSensor.Status;
                                    MyaddStatus.SensorFieldLastValue = MyFields.LastValue;
                                    MyaddStatus.SensorFieldCaption = MyFields.Caption;
                                    MyaddStatus.SensorFieldsLastOtherValue = MyFields.LastOtherValue;
                                    MyaddStatus.SensorFieldStatus = (int)MyFields.FieldStatus;
                                    //                                AddValues(MyTable, MyCurSensor.ID.ToString, MyDeviceName, MyFields.FieldName, MyCurSensor.Caption, MyCurSensor.Status,
                                    //                                          MyFields.LastValue.ToString + MyFields.Caption, MyFields.LastOtherValue, MyFields.FieldStatus,
                                    //)
                                }
                                SetStatusColorAlert(ref MyaddStatus);
                                MyTable.Add(MyaddStatus);
                            }

                            // End If
                        }
                        catch (Exception ex)
                        {
                            //TODO: Add a message to the table maybe if an error has occured?
                        }

                    }
                }
                else
                {
                    //myFilterSensor.Count is less than 0
                    // MyTable += "<tr><td colspan=""5"" style=""color:red"">No Sensors found.</td></tr>"
                    return null;
                }
                // MyTable += "</tbody></table>"
                return MyTable;
            }
            catch (Exception ex)
            {
                //lblerr.Visible = True
                //lblerr.Text = ex.Message
                return null;
            }
        }
    }
}
