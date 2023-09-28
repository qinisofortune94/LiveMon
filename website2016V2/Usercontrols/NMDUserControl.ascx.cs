using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace website2016V2.Usercontrols
{
    public partial class NMDUserControl : System.Web.UI.UserControl
    {
        private static LiveMonitoring.DataAccess MyDataAccess = new LiveMonitoring.DataAccess();
        private int _MeterID;
        private string _RawNMD;
        #region "Properties"
        public int MeterID
        {
            //GetMeterCaption

            get { return _MeterID; }
            set
            {
                _MeterID = value;
                this.txtCaption.Text = (GetMeterCaption());
            }
        }
        private void ProcessRawNMD()
        {
            string[] myNMD = _RawNMD.Split(';');
            if (myNMD.Length>1)//got to find it in the monthly values
            {
                for(int myarraycnt=0; myarraycnt<= 22; myarraycnt+=2) // 0 2 4 6 8 10 12 14 16 18 20 22
                {
                    int tmpMonth = Convert.ToInt32(myNMD[myarraycnt]);
                    double tmpNMDKVA = Convert.ToDouble(myNMD[myarraycnt+1]);
                    if (DateTime.Now.Month == tmpMonth)
                    {
                        this.NMDMeter = tmpNMDKVA;
                        break;
                    }
                }
            }
            else //single value indicates single NMD
            {
                this.NMDMeter = Convert.ToDouble(myNMD[0]);
            }
        }
        private double NMDMeter
        {

            get { return NMDMeter; }
            set
            {
                txtNMD.Text = value.ToString("0.##");
               
                if (Convert.ToDouble(txtLASTKVA.Text) > 0)
                {
                    if (Convert.ToDouble(txtLASTKVA.Text) < (Convert.ToDouble(txtNMD.Text)*0.9))//90% of NMD OK
                    {
                        txtLASTKVA.BackColor = System.Drawing.Color.LightGreen;
                        txtNMD.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if ((Convert.ToDouble(txtLASTKVA.Text) >= (Convert.ToDouble(txtNMD.Text) * 0.9)) && (Convert.ToDouble(txtLASTKVA.Text) <= Convert.ToDouble(txtNMD.Text) )) //within 10% of NMD
                    {
                        txtLASTKVA.BackColor = System.Drawing.Color.MediumVioletRed;
                        txtNMD.BackColor = System.Drawing.Color.MediumVioletRed;                      
                    }
                    else  //OVER NMD OVER NMD 
                    {
                        txtLASTKVA.BackColor = System.Drawing.Color.Red;
                        txtNMD.BackColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        #endregion
        public void Update()
        {
            this.MeterInfoUpdatePanel.Update();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string GetMeterCaption()
        {
            string MyRet = "";
            try
            {
                string MySql = "";

                MySql += "SELECT [Caption] ,ExtraData";
                MySql += " FROM sensors ";
                MySql += "where id=" + MeterID.ToString();
                System.Data.SqlClient.SqlDataReader MySQLReader = default(System.Data.SqlClient.SqlDataReader);
                MySQLReader = MyDataAccess.QueryDBSQLStr(MySql);
                if ((MySQLReader == null) == false)
                {
                    while (MySQLReader.Read())
                    {
                        //if details then
                        try
                        {
                            if (Information.IsDBNull(MySQLReader["Caption"]) == false)
                            {
                                MyRet = MySQLReader["Caption"].ToString();
                                //0=id
                            }
                            if (Information.IsDBNull(MySQLReader["ExtraData"]) == false)
                            {
                                _RawNMD = MySQLReader["ExtraData"].ToString();
                                //0=id
                            }                          

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    MySQLReader.Close();
                }
                MySQLReader = null;
                return MyRet;

            }
            catch (Exception ex)
            {
            }
            return MyRet;
        }
    }
}