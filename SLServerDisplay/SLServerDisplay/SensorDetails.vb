Public Class SensorDetails
    'Public Class SensorDetails
    Enum SiteCriticalType
        MissionCritical = 0
        BusinessCritical = 1
        Other = 2
    End Enum
    Enum SensorType

        EDS_DS18B20 = 0
        EDS_DS18S20 = 1
        EDS_DS2406 = 2
        EDS_AnalogOutput = 3
        EDS_AnalogProbe = 4

        TCPMbusDInput = 5
        TCPMbusDOutput = 6
        TCPMbusAOutput = 7
        TCPMbusAInput = 8
        TCPMbusRTD = 9
        AMF = 10
        CameraDInput = 11
        CameraDOutput = 12
        BaseAudio = 13
        CameraAudio = 14
        SNMPPoint = 15
        ICMPPoint = 16
        SQLPoint = 17
        ISPoint = 18
        HMP2001 = 19
        CameraMotion = 20
        TCPMbusDiscrete = 21
        TCPMbusFloat = 22
        TCPMbusCounter = 23
        OPCItem = 24
        GPSSensor = 25
        SerialMbusDInput = 26
        SerialMbusDOutput = 27
        SerialMbusAOutput = 28
        SerialMbusAInput = 29
        SerialMbusRTD = 30
        Undef = 31
        SerialMbusDiscrete = 32
        SerialMbusFloat = 33
        SerialMbusCounter = 34
        SerialMbusADiscrete = 42

        OneWireTempType10 = 35
        OneWireQuadADType20 = 36
        OneWireThermocronType21 = 37
        OneWireTHDSmartMonitorType26 = 38
        OneWireTempType28 = 39
        OneWireDigiPotenType2C = 40
        OneWireLioBatMonitorType30 = 41

        ShutUPS = 43
        MegatecUPS = 44
        BiometricReader247DB = 45
        BiometricReaderSagem = 46
        BiometricReaderZKSoft = 47
        PowerWareSNMP = 48
        LovatoGensetTCP = 49
        ServermonAgentSensor = 50
        ServerLogSensor = 51
        SMTPCheckSensor = 52
        POPCheckSensor = 53
        'LovatoGensetSerial = 50
        WMIGeneralSensor = 54
        WMIPCInfoSensor = 55
        WMIMemorySensor = 56
        WMIDrivesSensor = 57
        WMIProcessorLoadSensor = 58
        WMIServicesSensor = 59
        WMIPrintersSensor = 60
        WMIProcessRunningSensor = 61
        ICMPSite = 62
        POPCountSensor = 63
        POPLastDateSensor = 64
        DeepSeaGensetMonitor = 65
        DeepSeaGensetStatus = 66
        AMF120Mk4Monitor = 67
        AMF120Mk4Status = 68
        FileCounter = 69
        FilesDIRAgeMonitor = 70
        MegatecSNMP = 71
        PSExch07Specific = 72
        PSExch07All = 73
        Megatec3Phase = 74
        LovatoGensetRGAM20TCP = 75
        WMIServiceRunningSensor = 76
        WMIRegistrySensor = 77
        PSExchX64Specific = 78
        ElsterA1140MeterCurrentValues = 79
        ElsterA1140MeterReadProfile = 80
        ElsterA1140MeterReadInstrumentProfile = 81
        ElsterA1140MeterCUMULATIVEREGISTERS = 82
        ElsterA1140MeterCumulativeMaxDemandRegisters = 83
        ElsterA1140MeterTOURegister = 84
        ElsterA1700MeterCurrentValues = 85
        ElsterA1700MeterReadProfile = 86
        ElsterA1700MeterReadInstrumentProfile = 87
        ElsterA1700MeterCUMULATIVEREGISTERS = 88
        ElsterA1700MeterCumulativeMaxDemandRegisters = 89
        ElsterA1700MeterTOURegister = 90
        LovatoDMG210 = 91
        LovatoDMG300 = 92
        LovatoDMG700 = 93
        LovatoDMG800 = 94
        DirisA4041New = 95
        DirisA4041Old = 96

        LovatoDCRJ = 97
        LovatoDCRK = 98

        YasKawaV1000 = 105
        YasKawaA1000 = 110
        TCPRTUMbusRTD = 115
        OneWireDualAdressableSwitchC = 120

        ContegRamosCSNMP = 124 '-TODO
        ContegDryContact = 125
        ContegTemperature = 130
        ContegHumidity = 135
        'pdu monitored toatl
        ContegMonPDU8SNMP = 136 '-TODO
        ContegMonPDU16SNMP = 137 '-TODO
        ContegMonPDU24SNMP = 138 '-TODO

        ContegFlood = 140
        ContegPowerDetector = 141
        ContegIntPDU008C3SNMP = 143 'single phase'-TODO
        '321C33C9 3 phase
        '318C36C9 3 phase
        ContegIntPDU24SNMP = 144 '-TODO
        LovatoGensetRGAM60TCP = 145
        GamatronicsSNMP = 150
        DeltaSNMP = 153

        'Stultz
        StulzSNMPWIB8000 = 155 '-TODO
        'StulzSNMPGE = 160 '-TODO
        'StulzSNMPCW = 165 '-TODO
        'StulzSNMPChiller = 170 '-TODO
        StulzMBusC6000 = 175 '-TODO
        StulzMBusC5000 = 180 '-TODO
        StulzMBusC1002 = 185 '-TODO
        StulzMBusC1010 = 190 '-TODO
        StulzMBusC1010Rack = 195 '-TODO
        StulzMBusC1010STULSR = 200 '-TODO
        JanitzaUMG604MbusReadings = 205
        JanitzaUMG604MbusHarmonics = 210
        JanitzaUMG604MbusAll = 215
        'Janitza Other -245
        HW_GroupPoseidonSNMP = 250 '-TODO
        HW_GroupPoseidonXMLDryContact = 255
        HW_GroupPoseidonXMLTemperature = 260
        HW_GroupPoseidonXMLHumidity = 265
        HW_GroupPoseidonXMLFlood = 270
        HW_GroupPoseidonXMLPowerDetector = 275
        GEHydranM2 = 280
        GETransFix = 285
        GETapTrans = 290
        GEMultiTrans = 295
        GEMiniTrans = 300
        GEDualTrans = 305
        ProduMaxRuntime = 310
        ProduMaxShifttime = 315
        ProdumaxQty = 320
        WebServicePowerReading = 335
        WebServiceRuntimeReading = 340
        WebServiceShifttimeReading = 345

        TraceRouteSensor = 350

        VoltronicUSBUPS = 355

        CameraItemDetector = 360

        RemoteIPMonPing = 370
        'TCP Mbus Output Values
        TCPMbusForceMultipleCoils = 380
        TCPMbusRTUForceMultipleCoils = 381
        TCPMbuswriteMultipleRegisters = 385
        TCPMbusRTUwriteMultipleRegisters = 386
        TCPMbuswriteSingleRegister = 390
        TCPMbusRTUwriteSingleRegister = 391
        TCPMbusreadWriteRegisters = 395
        TCPMbusRTUreadWriteRegisters = 396
        TCPMbusRTUDOutput = 400
        TCPMBusOnOffScheduledPoint = 405
        TCPMBusOffOnScheduledPoint = 406

        'Rockwell PM1000
        RockwellPM1000VoltsAmpsFrequency = 420
        RockwellPM1000PowerResults = 425
        RockwellPM1000EnergyResults = 430
        RockwellPM1000DemandResults = 435

        RockwellPM1000UnitStatusLog = 440
        RockwellPM1000EnergyLogResults = 445
        RockwellPM1000LoadFactorlogResults = 446
        RockwellPM1000TimeOfuseLogkVAResults = 447
        RockwellPM1000TimeOfuseLogkVARResults = 448
        RockwellPM1000TimeOfuseLogKwhResults = 449

        'Custom Sensors
        CustomModbus = 460
        CustomSNMP = 470
        CustomXML = 480

        'RT Systems
        RTSysTM3SNMP = 500

        'File Process 
        FileWatcher = 520
        FileWatcherText = 525
        FileAgeChangeMonitor = 530
        FileAgeChangeMonitorText = 535
        FileFilteredCounter = 536
        FileCSVMonitorText = 537
        FileMonitorTextCnt = 538
        FileFilterWatcherText = 539

        'Added for EOH
        AdroitService = 550
        AdroitSQLHistorian = 555
        WonderWareService = 560
        WonderWareSQLHistorian = 565
        OracleService = 570
        OracleConnection = 575
        OracleUptime = 580

        'kingsley added this as i couldnt find it on the server and i had an error on my code
        FileAgeMonitor = 590

        'Roger Added EOH
        SQLDBMemory = 600
        OracleDBMemory = 601

        SQLDBLocks = 605
        OracleDBLocks = 606

        SQLConnections = 610
        OracleConnections = 611

        SQLBackupHist = 615
        OracleBackupHist = 616

        SQLDBStatus = 620
        ' OracleDBStatus = 621 part of integrity

        SQLMirrorStatus = 625
        OracleMirrorStatus = 626

        SQLAllDBIntegrity = 630
        OracleDBStatusIntegrity = 631

        SQLAllDBIndexFragmentation = 634
        OracleDBIndexFragmentation = 635

        SQLAllDBFileSize = 636
        OracleDBFileSize = 637

        SQLDBBackupFailure = 638

        'Web Services
        WebServiceCheck = 640
        WebServiceWCF = 645

        'SNMP Monitoring
        SNMPStandardPC = 670
        SNMPAllServiceStatus = 671
        SNMPSpecificServiceStatus = 672
        SNMPAllProcesses = 673
        SNMPSpecificProcessStatus = 674

        'ASPENTEc IP21
        AspenTechADOQuery = 680
        AspenTechODBCQuery = 690

        'WMI Threads
        WMIProcessThreadsSensor = 700
        WMIProcessMemorySensor = 705
        WMIStandardPC = 710
        'Modems
        WMIUSBModem = 712
        WMISerialModem = 713

        WMIGetOfficeversion = 720
        WMIGetAVInstalled = 730
        WMIGetStartupPrograms = 740
        WMIGetAllSoftware = 750

        'cmdb
        WMICMDBAllSoftware = 760
        WMICMDBOperatingSystem = 770
        WMICMDBHardware = 780
        WMICMDBAllPoints = 790
    End Enum
    Enum StatusDef
        ok = 0
        notify = 1
        alert = 2
        statuserror = 3
        criticalerror = 4
        devicefailure = 5
        noresponse = 6
        disabled = 7
        slowresponse = 8
    End Enum
    Class SNMPObjectValues
        Public OID As String
        Public InstanceRow As Integer
    End Class
    Class SensorFieldsDef
        Public ID As Integer
        Public SensorID As Integer
        Public FieldNumber As Integer
        Public FieldName As String
        Public LastValue As Double
        Public LastOtherValue As String
        Public LastDTRead As DateTime
        Public EventDate As Date
        Public Caption As String
        Public FieldStatus As New StatusDef
        Public FieldMaxValue As Double
        Public FieldMinValue As Double
        Public FieldNotes As String
        Public FieldTrigger As Boolean
        Public DisplayValue As Boolean = True
        Public TabularRowNo As Integer = 0
    End Class
    Class SensorGroup
        Public SensorGroupName As String
        Public SensorGroupID As Integer
        Public SensorGroupDisc As String
        Public SensorGroupOrder As Integer
    End Class
    Public Enum OutputTypeDef
        Toggel = 0
        SetValue = 1
        CopyValue = 2
    End Enum
    'items.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + myInt.ToString)
    Public ID As Integer
    Public Type As SensorType
    Public SensGroup As SensorGroup
    Public SNMPVals As SNMPObjectValues
    Public IPDeviceID As Integer
    Public ModuleNo As Integer
    Public Register As Integer
    Public SerialNumber As String
    Public LastValue As Double
    Public OutputType As OutputTypeDef
    Public LastValueDt As DateTime
    Public Caption As String
    Public ImageNormal As Image
    Public ImageError As Image
    Public ImageNoResponse As Image
    Public ImageNormalByte() As Byte
    Public ImageErrorByte() As Byte
    Public ImageNoResponseByte() As Byte
    Public MinValue As Double
    Public MaxValue As Double
    Public Multiplier As Double
    Public Divisor As Double
    Public ScanRate As Double
    Public myconvert As Double
    Public OffSetStart As Double
    Public WMIProperty As String
    Public WMIClass As String
    Public WMIValue As String
    Public WMIPC As String
    Public WMIID As Integer
    Public SiteID As Integer
    Public SiteCritical As Integer
    Public ExtraData As String
    Public ExtraData1 As String
    Public ExtraData2 As String
    Public ExtraData3 As String
    Public ExtraValue As Double
    Public ExtraValue1 As Double
    Public IsProxySensor As Boolean = False
    Public ProxySensorID As Integer
    Public Add2Site As Integer = 0
    Public Fields As New Collection
    Public Schedule As Collection
    Public Status As New StatusDef
    Public DispExtraData As String
    Public DispExtraData1 As String
    Public DispExtraData2 As String
    Public DispExtraData3 As String
    Public DispExtraValue As Double
    Public DispExtraValue1 As Double
    Public DispExtraValue2 As Double

    ' End Class

End Class
