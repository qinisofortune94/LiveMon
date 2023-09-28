using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


namespace website2016V2
{

    partial class MeteringPhasorDiagram : System.Web.UI.UserControl
    {
        private double _RedPhaseValue;
        private double _RedPhaseAngle;
        private double _WhitePhaseValue;
        private double _WhitePhaseAngle;
        private double _BluePhaseValue;
        private double _BluePhaseAngle;
        private double _RedPhaseAmps;
        private double _RedPhaseCurAngle;
        private double _WhitePhaseAmps;
        private double _WhitePhaseCurAngle;
        private double _BluePhaseAmps;
        private double _BluePhaseCurAngle;
        private double _RedPowerFactor;
        private double _WhitePowerFactor;
        private double _BluePowerFactor;
        private double _VoltageNominal = 230;
        private double _RedCurNorAngle;
        private double _WhiteCurNorAngle;
        private double _BlueCurNorAngle;
        private Collection _MyDataHistoryCol = new Collection();
        private List<LiveMonitoring.IRemoteLib.DataHistory> _MyDataHistory = new List<LiveMonitoring.IRemoteLib.DataHistory>();
        private LiveMonitoring.IRemoteLib.SensorDetails _MySensor = new LiveMonitoring.IRemoteLib.SensorDetails();
        public LiveMonitoring.IRemoteLib.SensorDetails SensorDetails
        {
            get { return _MySensor; }
            set { _MySensor = value; }
        }
        public int FindDatePosition(System.DateTime SeekDataDate, int SeekFieldNo)
        {
            try
            {
                if (_MyDataHistory.Count > 0)
                {
                    int Mycnt = 1;
                    //find position based on date
                    foreach (var MyData_loopVariable in _MyDataHistory)
                    {
                        if (SeekDataDate >= MyData_loopVariable.DT)
                        {
                            return Mycnt;
                        }
                        Mycnt += 1;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
            }
            return 0;
        }
        private static int CompareDates(LiveMonitoring.IRemoteLib.DataHistory x, LiveMonitoring.IRemoteLib.DataHistory y)
        {

            if (x.DT == y.DT)
            {
                return 0;
            }
            if (x.DT < y.DT)
            {
                return -1;
            }
            if (x.DT > y.DT)
            {
                return 1;
            }
            return 0;

        }
        public Collection DataHistory
        {
            get { return _MyDataHistoryCol; }
            set
            {
                _MyDataHistoryCol = value;
                _MyDataHistory.Clear();
                foreach (LiveMonitoring.IRemoteLib.DataHistory MyData in _MyDataHistoryCol)
                {
                    try
                    {
                        //_MyDataHistory.Insert(FindDatePosition(MyData.DT, MyData.Field), MyData)
                        _MyDataHistory.Add(MyData);

                    }
                    catch (Exception ex)
                    {
                    }

                }

                LoadList();
            }
        }
        private void LoadList()
        {
            try
            {
                if ((ddlDataSelection == null) == false)
                {
                    this.ddlDataSelection.Items.Clear();
                    //Else
                    //    cmbDatadate.i()
                }


            }
            catch (Exception ex)
            {
            }

            Collection MyCheckList = new Collection();
            bool FirstOne = true;
            foreach (LiveMonitoring.IRemoteLib.DataHistory MyData in _MyDataHistory)
            {
                try
                {
                    if (MyCheckList.Contains(MyData.DT.ToString()) == false & MyData.Field == 1)
                    {
                        MyCheckList.Add(MyData.DT.ToString());
                        ListItem MyListItem = new ListItem();
                        MyListItem.Text = MyData.DT.ToString();
                        MyListItem.Value = MyData.DT.ToString();
                        this.ddlDataSelection.Items.Add(MyListItem);
                        if (FirstOne)
                        {
                            MyListItem.Selected = true;
                            FirstOne = false;
                        }

                    }
                    else
                    {
                    }

                }
                catch (Exception ex)
                {
                    Trace.Write("Err phasor diagram LoadList" + ex.Message);
                }
            }
            LoadValues();
        }
        private void LoadValues()
        {
            try
            {
                double SetScanRate = _MySensor.ScanRate;
                if (SetScanRate == 0)
                {
                    SetScanRate = 60000;
                }
                bool StartReading = false;
                //_MyDataHistory .Sort(m
                // Dim dc As New MyDataComparer
                _MyDataHistory.Sort(CompareDates);
                foreach (LiveMonitoring.IRemoteLib.DataHistory MyData in _MyDataHistory)
                {
                    try
                    {
                        if (StartReading)
                        {
                            if (MyData.DT >= DateAndTime.DateAdd(DateInterval.Second, SetScanRate / 1000 + ((SetScanRate / 1000) / 2), Convert.ToDateTime(ddlDataSelection.SelectedValue)))
                            {
                                StartReading = false;
                                //stop reading
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        //ok start point
                        if (MyData.DT.ToString() == ddlDataSelection.SelectedValue | StartReading == true)
                        {
                            StartReading = true;
                            //RedPowerFactor
                            //WhitePowerFactor
                            //BluePowerFactor
                            //BluePhaseAmps
                            //   BluePhaseCurAngle
                            //WhitePhaseAmps
                            //   WhitePhaseCurAngle
                            //RedPhaseAmps
                            //   RedPhaseCurAngle
                            //BluePhaseAngle
                            //BluePhaseValue
                            //WhitePhaseAngle
                            //WhitePhaseValue
                            //   RedPhaseAngle
                            //RedPhaseValue
                            switch (_MySensor.Type)
                            {
                                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1140MeterCurrentValues:
                                    //29 fields
                                    switch (MyData.Field)
                                    {

                                        case 21:
                                            RedPowerFactor = MyData.Value;
                                            break;
                                        case 22:
                                            WhitePowerFactor = MyData.Value;
                                            break;
                                        case 23:
                                            BluePowerFactor = MyData.Value;
                                            break;
                                        case 13:
                                            BluePhaseAmps = MyData.Value;
                                            break;
                                        case 24:
                                            BluePhaseCurAngle = MyData.Value;
                                            break;
                                        case 14:
                                            WhitePhaseAmps = MyData.Value;
                                            break;
                                        case 25:
                                            WhitePhaseCurAngle = MyData.Value;
                                            break;
                                        case 15:
                                            RedPhaseAmps = MyData.Value;
                                            break;
                                        case 26:
                                            RedPhaseCurAngle = MyData.Value;
                                            break;
                                        case 17:
                                            BluePhaseValue = MyData.Value;
                                            break;
                                        case 18:
                                            WhitePhaseValue = MyData.Value;
                                            break;
                                        case 19:
                                            RedPhaseValue = MyData.Value;
                                            break;
                                    }
                                    break;
                                case LiveMonitoring.IRemoteLib.SensorDetails.SensorType.ElsterA1700MeterCurrentValues:
                                    //47 fields
                                    switch (MyData.Field)
                                    {

                                        case 8:
                                            RedPowerFactor = MyData.Value;
                                            break;
                                        case 9:
                                            WhitePowerFactor = MyData.Value;
                                            break;
                                        case 10:
                                            BluePowerFactor = MyData.Value;
                                            break;
                                        case 1:
                                            BluePhaseAmps = MyData.Value;
                                            break;
                                        case 27:
                                            BluePhaseCurAngle = MyData.Value;
                                            break;
                                        case 2:
                                            WhitePhaseAmps = MyData.Value;
                                            break;
                                        case 28:
                                            WhitePhaseCurAngle = MyData.Value;
                                            break;
                                        case 3:
                                            RedPhaseAmps = MyData.Value;
                                            break;
                                        case 29:
                                            RedPhaseCurAngle = MyData.Value;
                                            break;
                                        case 4:
                                            BluePhaseValue = MyData.Value;
                                            break;
                                        case 5:
                                            WhitePhaseValue = MyData.Value;
                                            break;
                                        case 6:
                                            RedPhaseValue = MyData.Value;
                                            break;
                                    }
                                    break;
                            }
                            //ok right record field 1 to
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                DrawGraph();
            }
            catch (Exception ex)
            {
                Trace.Write("Err phasor diagram LoadValues" + ex.Message);
            }
        }
        public double RedPowerFactor
        {
            get { return _RedPowerFactor; }
            set
            {
                _RedPowerFactor = value;
                this.lblRedPowerFactor.Text = value.ToString("0.0");
            }
        }
        public double WhitePowerFactor
        {
            get { return _WhitePowerFactor; }
            set
            {
                _WhitePowerFactor = value;
                this.lblWhitePowerFactor.Text = value.ToString("0.0");
            }
        }
        public double BluePowerFactor
        {
            get { return _BluePowerFactor; }
            set
            {
                _BluePowerFactor = value;
                this.lblBluePowerFactor.Text = value.ToString("0.0");
            }
        }
        public double BluePhaseAmps
        {
            get { return _BluePhaseAmps; }
            set
            {
                _BluePhaseAmps = value;
                this.lblBluePhaseAmps.Text = value.ToString("0.0");
            }
        }
        public double BluePhaseCurAngle
        {
            get { return _BluePhaseCurAngle; }
            set
            {
                _BluePhaseCurAngle = value;
                this.lblBluePhaseCurAngle.Text = value.ToString("0.0");
            }
        }

        public double WhitePhaseAmps
        {
            get { return _WhitePhaseAmps; }
            set
            {
                _WhitePhaseAmps = value;
                this.lblWhitePhaseAmps.Text = value.ToString("0.0");
            }
        }
        public double WhitePhaseCurAngle
        {
            get { return _WhitePhaseCurAngle; }
            set
            {
                _WhitePhaseCurAngle = value;
                this.lblWhitePhaseCurAngle.Text = value.ToString("0.0");
            }
        }

        public double RedPhaseAmps
        {
            get { return _RedPhaseAmps; }
            set
            {
                _RedPhaseAmps = value;
                this.lblRedPhaseAmps.Text = value.ToString("0.0");
            }
        }
        public double RedPhaseCurAngle
        {
            get { return _RedPhaseCurAngle; }
            set
            {
                _RedPhaseCurAngle = value;
                this.lblRedPhaseCurAngle.Text = value.ToString("0.0");
            }
        }
        public double BluePhaseAngle
        {
            get { return _BluePhaseAngle; }
            set
            {
                _BluePhaseAngle = value;
                this.lblBluePhaseAngle.Text = value.ToString("0.0");
            }
        }
        public double BluePhaseValue
        {
            get { return _BluePhaseValue; }
            set
            {
                _BluePhaseValue = value;
                this.lblBluePhaseVolts.Text = value.ToString("0.0");
            }
        }

        public double WhitePhaseAngle
        {
            get { return _WhitePhaseAngle; }
            set
            {
                _WhitePhaseAngle = value;
                this.lblWhitePhaseAngle.Text = value.ToString("0.0");
            }
        }
        public double WhitePhaseValue
        {
            get { return _WhitePhaseValue; }
            set
            {
                _WhitePhaseValue = value;
                this.lblWhitePhaseVolts.Text = value.ToString("0.0");
            }
        }

        public double RedPhaseAngle
        {
            get { return _RedPhaseAngle; }
            set
            {
                _RedPhaseAngle = value;
                this.lblRedPhaseAngle.Text = value.ToString("0.0");
            }
        }
        public double RedPhaseValue
        {
            get { return _RedPhaseValue; }
            set
            {
                _RedPhaseValue = value;
                this.lblRedPhaseVolts.Text = value.ToString("0.0");
            }
        }
        public void DrawGraph()
        {
            try
            {
                //voltage scaled
                lblRedScaledVolts.Text = ((_RedPhaseValue / _VoltageNominal) * 100 * Math.Cos(_RedPhaseAngle * Math.PI / 180)).ToString("0.00");
                lblWhiteScaledVolts.Text = ((_WhitePhaseValue / _VoltageNominal) * 100 * Math.Cos(_WhitePhaseAngle * Math.PI / 180)).ToString("0.00");
                lblBlueScaledVolts.Text = ((_BluePhaseValue / _VoltageNominal) * 100 * Math.Cos(_BluePhaseAngle * Math.PI / 180)).ToString("0.00");

                lblRedScaledVoltsSine.Text = ((_RedPhaseValue / _VoltageNominal) * 100 * Math.Sin(_RedPhaseAngle * Math.PI / 180)).ToString("0.00");
                lblWhiteScaledVoltsSine.Text = ((_WhitePhaseValue / _VoltageNominal) * 100 * Math.Sin(_WhitePhaseAngle * Math.PI / 180)).ToString("0.00");
                lblBlueScaledVoltsSine.Text = ((_BluePhaseValue / _VoltageNominal) * 100 * Math.Sin(_BluePhaseAngle * Math.PI / 180)).ToString("0.00");
                //original a+jb values
                lblajbRedScaledVolts.Text = (_RedPhaseValue * Math.Cos(_RedPhaseAngle * Math.PI / 180)).ToString("0.00");
                lblajbWhiteScaledVolts.Text = (_WhitePhaseValue * Math.Cos(_WhitePhaseAngle * Math.PI / 180)).ToString("0.00");
                lblajbBlueScaledVolts.Text = (_BluePhaseValue * Math.Cos(_BluePhaseAngle * Math.PI / 180)).ToString("0.00");
                //sine
                lblajbRedScaledVoltsS.Text = (_RedPhaseValue * Math.Sin(_RedPhaseAngle * Math.PI / 180)).ToString("0.00");
                lblajbWhiteScaledVoltsS.Text = (_WhitePhaseValue * Math.Sin(_WhitePhaseAngle * Math.PI / 180)).ToString("0.00");
                lblajbBlueScaledVoltsS.Text = (_BluePhaseValue * Math.Sin(_BluePhaseAngle * Math.PI / 180)).ToString("0.00");

                //
                _RedCurNorAngle = _RedPhaseCurAngle;
                _WhiteCurNorAngle = _WhitePhaseCurAngle + 240;
                _BlueCurNorAngle = _BluePhaseCurAngle + 120;
                //current scaled values
                lblRedScaledCurrent.Text = (_RedPhaseAmps / (Math.Max(Math.Max(_RedPhaseAmps, _BluePhaseAmps), _WhitePhaseAmps)) * 100 * Math.Cos(_RedCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblWhiteScaledCurrent.Text = (_WhitePhaseAmps / (Math.Max(Math.Max(_RedPhaseAmps, _BluePhaseAmps), _WhitePhaseAmps)) * 100 * Math.Cos(_WhiteCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblBlueScaledCurrent.Text = (_BluePhaseAmps / (Math.Max(Math.Max(_RedPhaseAmps, _BluePhaseAmps), _WhitePhaseAmps)) * 100 * Math.Cos(_BlueCurNorAngle * Math.PI / 180)).ToString("0.00");
                //sine
                lblRedScaledCurrentS.Text = (_RedPhaseAmps / (Math.Max(Math.Max(_RedPhaseAmps, _BluePhaseAmps), _WhitePhaseAmps)) * 100 * Math.Sin(_RedCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblWhiteScaledCurrentS.Text = (_WhitePhaseAmps / (Math.Max(Math.Max(_RedPhaseAmps, _BluePhaseAmps), _WhitePhaseAmps)) * 100 * Math.Sin(_WhiteCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblBlueScaledCurrentS.Text = (_BluePhaseAmps / (Math.Max(Math.Max(_RedPhaseAmps, _BluePhaseAmps), _WhitePhaseAmps)) * 100 * Math.Sin(_BlueCurNorAngle * Math.PI / 180)).ToString("0.00");

                //original a+jb values current
                lblajbRedScaledCur.Text = (_RedPhaseAmps * Math.Cos(_RedCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblajbWhiteScaledCur.Text = (_WhitePhaseAmps * Math.Cos(_WhiteCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblajbBlueScaledCur.Text = (_BluePhaseAmps * Math.Cos(_BlueCurNorAngle * Math.PI / 180)).ToString("0.00");

                //sine
                lblajbRedScaledCurS.Text = (_RedPhaseAmps * Math.Sin(_RedCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblajbWhiteScaledCurS.Text = (_WhitePhaseAmps * Math.Sin(_WhiteCurNorAngle * Math.PI / 180)).ToString("0.00");
                lblajbBlueScaledCurS.Text = (_BluePhaseAmps * Math.Sin(_BlueCurNorAngle * Math.PI / 180)).ToString("0.00");
                //normalised angles
                lblajbRedCurNorAngle.Text = _RedCurNorAngle.ToString("0.00");
                lblajbWhiteCurNorAngle.Text = _WhiteCurNorAngle.ToString("0.00");
                lblajbBlueCurNorAngle.Text = _BlueCurNorAngle.ToString("0.00");
            }
            catch (Exception ex)
            {
                Trace.Write("Err phasor diagram DrawGraph" + ex.Message);
            }

        }



        protected void cmbDatadate_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadValues();
        }

        public MeteringPhasorDiagram()
        {
            if ((ddlDataSelection == null) == false)
            {
                this.ddlDataSelection.Items.Clear();
            }
            else
            {
                ddlDataSelection = new DropDownList();
            }
        }
    }

}