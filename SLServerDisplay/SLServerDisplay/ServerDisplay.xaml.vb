Imports System.Windows.Media
Imports System.Collections
Imports System.Windows.Controls
Partial Public Class ServerDisplay
    Inherits UserControl
    Private MyId As Integer = 0
    Private MyColumn As Integer = 0
    Private MyRow As Integer = 0
    Private ServerHeight As Double = 50 '38 '38
    Private ServerWidth As Double = 58 '52 '52
    Private LastSensorWidth As Double
    Private ServerRow As Integer = 9
    Private MySiteID As Integer
    Private MySitePageNo As Integer
    Private MySiteExtraData As String
    Private MySiteExtraValue As Double
    Private MySiteExtraData1 As String
    Private MySiteExtraValue1 As Double
    Private MyUndisplayed As Integer = 0
    Private MyDisplayType As Integer = 0
    Private MyDisplayWidth As Integer = 0
    Private MyDisplayHeight As Integer = 0
    Private SensorsCol As New Collection
    Private MyDisplayImage As String
    Private MyScreen As Integer = 1
    Private Sensors As String
    Private Clickpos As Point
    Public isInEditInEffect As Boolean = False
    Private myServerList As New List(Of Server)  '= new List<int>()
    Private WithEvents proxy As LiveMonitoring.SLIPmonInterfaceSVCClient
    Private WithEvents MyTimer As New System.Windows.Threading.DispatcherTimer
    Private Shared SLSettings As New SLSettings
    Public _parameters As IDictionary(Of String, String)
    Public Property PageNo() As Integer
        Get
            Return MySitePageNo
        End Get
        Set(ByVal value As Integer)
            MySitePageNo = value
        End Set
    End Property
    Public Property Extravalue() As Double
        Get
            Return MySiteExtraValue
        End Get
        Set(ByVal value As Double)
            MySiteExtraValue = value
        End Set
    End Property
    Public Property ExtraData() As String
        Get
            Return MySiteExtraData
        End Get
        Set(ByVal value As String)
            MySiteExtraData = value
        End Set
    End Property
    Public Property Extravalue1() As Double
        Get
            Return MySiteExtraValue1
        End Get
        Set(ByVal value As Double)
            MySiteExtraValue1 = value
        End Set
    End Property
    Public Property ExtraData1() As String
        Get
            Return MySiteExtraData1
        End Get
        Set(ByVal value As String)
            MySiteExtraData1 = value
        End Set
    End Property
    Public Property DisplayType() As Integer
        Get
            Return MyDisplayType
        End Get
        Set(ByVal value As Integer)
            MyDisplayType = value
        End Set
    End Property
    Public Property DisplayWidth() As Integer
        Get
            Return MyDisplayWidth
        End Get
        Set(ByVal value As Integer)
            MyDisplayWidth = value
        End Set
    End Property
    Public Property DisplayHeight() As Integer
        Get
            Return MyDisplayHeight
        End Get
        Set(ByVal value As Integer)
            MyDisplayHeight = value
        End Set
    End Property
    Public Property DisplayImage() As String
        Get
            Return MyDisplayImage
        End Get
        Set(ByVal value As String)
            MyDisplayImage = value
        End Set
    End Property
    
    Public Property ScreenSetting() As Integer
        Get
            Return MyScreen
        End Get
        Set(ByVal value As Integer)
            MyScreen = value
        End Set
    End Property
    Public Property ServerWidthSetting() As Double
        Get
            Return ServerWidth
        End Get
        Set(ByVal value As Double)
            ServerWidth = value
        End Set
    End Property
    Public Property ServerHeightSetting() As Double
        Get
            Return ServerHeight
        End Get
        Set(ByVal value As Double)
            ServerHeight = value
        End Set
    End Property
    Public Property ServersInARow() As Integer
        Get
            Return ServerRow
        End Get
        Set(ByVal value As Integer)
            ServerRow = value
        End Set
    End Property
    Public Property SiteID() As Integer
        Get
            Return MySiteID
        End Get
        Set(ByVal value As Integer)
            MySiteID = value
            If MyDisplayType = 2 Then
                SetupAlertTimers()
                'proxy.GetRemoteAlertHistoryAsync(DateAdd(DateInterval.Day, -1, Now), Now)
                Me.ComsEvent.RepeatBehavior = RepeatBehavior.Forever
                Me.ComsEvent.Begin()
            Else
                'If _parameters.Count > 0 Then
                If _parameters.ContainsKey("DisplayID") Then
                    proxy.GetGroupSensorsAsync(MySiteID)
                    'proxy.GetGroupSensorsRemoveAsync(MySiteID)
                Else
                    proxy.GetSiteSensorsAsync(MySiteID)
                End If
                Me.ComsEvent.RepeatBehavior = RepeatBehavior.Forever
                Me.ComsEvent.Begin()
            End If
            Try
                proxy.GetAllSensorsAsync() 'load the add form
            Catch ex As Exception

            End Try
        End Set
    End Property

    Private Property e As Object

    Private Sub TheGrid_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles TheGrid.MouseLeftButtonDown
        If isInEditInEffect Then
            Try
                Clickpos = e.GetPosition(Nothing)
                PopupAddSensor.IsOpen = True
                'txtBackImage.Text = Me.DisplayImage
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub TheGrid_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles TheGrid.MouseRightButtonDown

        If isInEditInEffect Then
            Try
                Clickpos = e.GetPosition(Nothing)
                PopupRemoveSensor.IsOpen = True
                ' txtBackImage.Text = Me.DisplayImage
            Catch ex As Exception

            End Try
        End If
    End Sub


    Private Function GetParameters(ByVal ParameterID As String) As String
        Return _parameters(ParameterID)
    End Function
    Public Sub New(ByVal parameters As IDictionary(Of String, String))
        Me._parameters = parameters

        InitializeComponent()
        'LoadSettings()
        Dim binding As New System.ServiceModel.BasicHttpBinding '= new BasicHttpBinding(); 
        binding.SendTimeout = TimeSpan.FromSeconds(150)
        binding.CloseTimeout = TimeSpan.FromSeconds(120)
        binding.OpenTimeout = TimeSpan.FromSeconds(120)
        binding.ReceiveTimeout = TimeSpan.FromSeconds(150)
        'binding.MaxReceivedMessageSize = Int.MaxValue
        ''IPMonInterface.
        ''myServiceClient myService = new myServiceClient(binding, new EndpointAddress(http://localhost/MyService/MyService.svc));
        '        ServiceHostWS = new ServiceHost(typeof(TheService))
        '        WSHttpBinding(binding)
        '        binding = New WSHttpBinding()
        'ServiceHostWS.AddServiceEndpoint(typeof(ITestServiceContract), binding, "http://localhost:2104/ServiceHostWS")

        '        ServiceHostWS.Open()
        'proxy = New LiveMonitoring.SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
        If _parameters.ContainsKey("ServiceRef") Then
            proxy = New LiveMonitoring.SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(Me.GetParameters("ServiceRef")))
        Else
            proxy = New LiveMonitoring.SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
        End If
        'If _parameters.ContainsKey("DisplayID") Then

    End Sub
    Private Sub SetupAlertTimers()
        Try
            MyTimer.Interval = New TimeSpan(0, 0, SLSettings.ServerDisplayTimer)
            AddHandler MyTimer.Tick, AddressOf OnAlertTimer
            MyTimer.Start()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub OnAlertTimer(ByVal sender As Object, ByVal e As EventArgs)
        'MyTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        'DoWork()
        'proxy.OpenAsync()
        Try
            proxy.GetRemoteAlertHistoryAsync(DateAdd(DateInterval.Day, -1, Now), Now)
            Me.ComsEvent.RepeatBehavior = RepeatBehavior.Forever
            Me.ComsEvent.Begin()

        Catch ex As Exception

        End Try
        'Me.FindName("1").SiteName.Text = DateTime.Now.ToLongTimeString()
    End Sub
    Private Sub SetupTimers()
        Try
            MyTimer.Interval = New TimeSpan(0, 0, SLSettings.ServerDisplayTimer)
            AddHandler MyTimer.Tick, AddressOf OnTimer
            MyTimer.Start()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub OnTimer(ByVal sender As Object, ByVal e As EventArgs)
        'MyTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        'DoWork()
        'proxy.OpenAsync()
        Try
            If IsNothing(Sensors) = False Then
                proxy.GetSiteSensorsStatusAsync(MySiteID, Sensors)
                Me.ComsEvent.RepeatBehavior = RepeatBehavior.Forever
                Me.ComsEvent.Begin()
            End If

        Catch ex As Exception

        End Try
        'Me.FindName("1").SiteName.Text = DateTime.Now.ToLongTimeString()
    End Sub

    Private Sub proxy_GetAllSensorsCompleted(sender As Object, e As LiveMonitoring.GetAllSensorsCompletedEventArgs) Handles proxy.GetAllSensorsCompleted
        Me.SensorsList.Items.Clear()

        ' Me.SensorsListRemove.Items.Clear()

        Dim GreenServers As String = ""
        Try
            'load the sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then

                    Me.ComsEvent.Stop()
                    LastSensorWidth = 0
                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Dim MySensor As New SensorDetails
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        Try
                            'ServerDisplay

                            MySensor.Caption = MyString(0)
                            MySensor.ID = CInt(MyString(1))
                            MySensor.SiteID = CInt(MyString(2))
                            MySensor.Status = CInt(MyString(3))
                            MySensor.Type = CInt(MyString(4))
                            MySensor.ExtraData = CStr(MyString(5))
                            MySensor.ExtraData1 = CStr(MyString(6))
                            MySensor.ExtraValue = CDbl(MyString(7))
                            MySensor.ExtraValue1 = CDbl(MyString(8))
                            Try
                                MySensor.ExtraData2 = CStr(MyString(9))
                                MySensor.ExtraData3 = CStr(MyString(10))
                            Catch ex As Exception

                            End Try
                            Try
                                MySensor.DispExtraData1 = CStr(MyString(11))
                                MySensor.DispExtraData2 = CStr(MyString(12))
                            Catch ex As Exception

                            End Try
                            Try
                                MySensor.DispExtraValue = CDbl(MyString(13))
                                MySensor.DispExtraValue1 = CDbl(MyString(14))
                            Catch ex As Exception

                            End Try
                            SensorsCol.Add(MySensor, MySensor.ID.ToString)
                        Catch ex As Exception

                        End Try
                        Try
                            Dim MyListitem As New ListBoxItem
                            'MyListitem.
                            MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                            MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                            'BorderThickness="2" BorderBrush="Blue"
                            MyListitem.Content += " Sensor:" + MySensor.Caption
                            MyListitem.Content += " Type:" + MySensor.Type.ToString
                            MyListitem.Name = MySensor.ID.ToString
                            'MyListitem.Content += " ID:" + MyString(0)
                            'MyListitem.Content += " AlertType:" + MyString(1)
                            'myServerList.Add(MyNew)
                            Me.SensorsList.Items.Add(MyListitem)
                            ' Me.SensorsListRemove.Items.Add(MyListitem)
                        Catch ex As Exception

                        End Try
                        ''add
                        'Dim MyString() = e.Result.Item(mycnt).Split("|")
                        'Dim MyNew As New Server

                        'MyNew.ServerText.Text = MyString(0)
                        'Sensors += (MyString(1)) + "|"
                        'MyNew.SensorID = CInt(MyString(1))
                        'MyNew.Name = MyNew.SensorID.ToString
                        'MyNew.Status = CInt(MyString(3))


                        ' ''''new'''''
                        'CurSensor = CInt(MyString(1))
                        ''Dim MySensor As SensorDetails = SensorsCol(CurSensor.ToString)
                        ''DisplayRoot.Children.Clear()
                        'MySensor.Status = CInt(MyString(3))



                        ''get values move to sensor
                        ''move to sensor->proxy.GetSensorValuesAsync(CInt(MyItem.Name))


                        ''''''''''''
                    Next
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub proxy_GetGroupSensorsRemoveCompleted() '(ByVal sender As Object, ByVal e As LiveMonitoring.GetGroupSensorsRemoveCompletedEventArgs) Handles proxy.GetGroupSensorsRemoveCompleted
        '  Me.SensorsList.Items.Clear()

        Me.SensorsListRemove.Items.Clear()

        Dim GreenServers As String = ""
        Try
            'load group sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then

                    Me.ComsEvent.Stop()
                    LastSensorWidth = 0
                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Dim MySensor As New SensorDetails
                        Dim MyString() ' = e.Result.Item(mycnt).Split("|")
                        Try
                            'ServerDisplay

                            MySensor.Caption = MyString(0)
                            MySensor.ID = CInt(MyString(1))
                            MySensor.SiteID = CInt(MyString(2))
                            MySensor.ExtraData = CStr(MyString(5))
                            MySensor.ExtraData1 = CStr(MyString(6))
                            MySensor.ExtraValue = CDbl(MyString(7))
                            MySensor.ExtraValue1 = CDbl(MyString(8))
                            Try
                                MySensor.ExtraData2 = CStr(MyString(9))
                                MySensor.ExtraData3 = CStr(MyString(10))
                            Catch ex As Exception

                            End Try
                            Try
                                MySensor.DispExtraData1 = CStr(MyString(11))
                                MySensor.DispExtraData2 = CStr(MyString(12))
                            Catch ex As Exception

                            End Try
                            Try
                                MySensor.DispExtraValue = CDbl(MyString(13))
                                MySensor.DispExtraValue1 = CDbl(MyString(14))
                            Catch ex As Exception

                            End Try
                            SensorsCol.Add(MySensor, MySensor.ID.ToString)
                        Catch ex As Exception

                        End Try
                        Try
                            Dim MyListitem As New ListBoxItem

                            MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                            MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)

                            MyListitem.Content += " Sensor:" + MySensor.Caption
                            MyListitem.Content += " Type:" + MySensor.Type.ToString
                            MyListitem.Name = MySensor.ID.ToString
                            Me.SensorsListRemove.Items.Add(MyListitem)
                        Catch ex As Exception

                        End Try

                    Next
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub


    Private Sub proxy_GetGroupSensorsCompleted(ByVal sender As Object, ByVal e As LiveMonitoring.GetGroupSensorsCompletedEventArgs) Handles proxy.GetGroupSensorsCompleted
        Dim GreenServers As String = ""
        Try
            'load the sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()
                    'myServerList = New List(Of ServerDetails)
                    If MyDisplayType = 1 Then 'add total green
                        MyUndisplayed = 0
                        Dim MyNew As New Server
                        MyNew.ServerBlock.Height = ServerHeight
                        MyNew.ServerBlock.Width = ServerWidth
                        MyNew.ServerText.Text = "Total OK"
                        MyNew.SensorID = -1
                        MyNew.Name = "TotalGreen"
                        MyNew.Status = 0
                        If MyId = 0 And MyId Mod ServerRow = 0 Then
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                                If Double.IsNaN(Me.ActualHeight) = False Then
                                    Me.BackPlaneBorder.Height = ActualHeight
                                End If
                            End If
                            If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                                Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                            End If
                        End If
                        DrawingObjects.Children.Add(MyNew)
                        myServerList.Add(MyNew)
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                        MyId += 1
                        MyRow += 1
                    Else
                        If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                            Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                            If Double.IsNaN(Me.ActualHeight) = False Then
                                Me.BackPlaneBorder.Height = ActualHeight
                            End If
                        End If
                        If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                            Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                        End If
                    End If

                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Try
                            ''add
                            ''tems.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + MySensor.Status.ToString)
                            Dim MyString() = e.Result.Item(mycnt).Split("|")
                            'Dim MySens As New ServerDetails
                            Dim MyNew As New Server
                            MyNew.ServerBlock.Height = ServerHeight
                            MyNew.ServerBlock.Width = ServerWidth

                            MyNew.ServerText.Text = MyString(0)
                            'MySens.SensorCaption = MyString(0)
                            'MySens.SensorID = CInt(MyString(1))
                            ' ToolTipService.SetToolTip(MyNew.ServerText, MyString(0))

                            Sensors += (MyString(1)) + "|"
                            MyNew.SensorID = CInt(MyString(1))
                            MyNew.Name = MySiteID.ToString + ":" + MyNew.SensorID.ToString
                            MyNew.Status = CInt(MyString(3))
                            AddHandler MyNew.ClickedMe, AddressOf SensorClickedEvent
                            AddHandler MyNew.ClickedMeAlert, AddressOf SensorClickedAlertEvent
                            If MyDisplayType = 1 And MyNew.Status = 0 Then 'add total green
                                MyUndisplayed += 1
                            End If
                            'MySens.Status = CInt(MyString(3))
                            'TheGrid
                            If MyId <> 0 And MyId Mod ServerRow = 0 Then
                                MyColumn += 1
                                MyRow = 0
                                If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                    Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 10
                                End If
                                If Me.BackPlaneBorder.Height < ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) Then
                                    Me.BackPlaneBorder.Height = ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) 'ActualHeight + ServerHeight + 10
                                Else
                                    '
                                End If
                            End If
                            If MyId = 0 And MyId Mod ServerRow = 0 Then
                                If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                    Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                                    If Double.IsNaN(Me.ActualHeight) = False Then
                                        Me.BackPlaneBorder.Height = ActualHeight
                                    End If
                                End If
                                If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                                    Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                                End If
                            End If
                            myServerList.Add(MyNew)
                            DrawingObjects.Children.Add(MyNew)
                            MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                            MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                            If MyDisplayType = 1 And MyNew.Status <> 0 Then 'add total green
                                MyId += 1
                                MyRow += 1
                            ElseIf MyDisplayType = 1 And MyNew.Status = 0 Then
                                GreenServers += MyNew.ServerText.Text + "|"
                                MyNew.Visibility = Windows.Visibility.Collapsed
                                MyNew.SetValue(Canvas.TopProperty, CDbl(0 * (ServerHeight + 10)))
                                MyNew.SetValue(Canvas.LeftProperty, CDbl((0 * (ServerWidth + 5))))
                                'MyNew.SetValue(
                            ElseIf MyDisplayType <> 1 Then
                                MyId += 1
                                MyRow += 1
                            End If
                        Catch ex As Exception

                        End Try

                    Next
                    Try
                        If MyDisplayType = 1 Then 'add total green
                            Dim MyNew As Server = CType(TheGrid.FindName("TotalGreen"), Server)
                            MyNew.ServerCnt.Text = MyUndisplayed.ToString
                            ToolTipService.SetToolTip(MyNew.ServerBlock, GreenServers)
                            ToolTipService.SetToolTip(MyNew.ServerText, GreenServers)
                        End If
                    Catch ex As Exception

                    End Try


                End If
            End If

        Catch ex As Exception

        End Try
        SetupTimers()
        Try
            proxy.GetSiteSensorsStatusAsync(MySiteID, Sensors)
        Catch ex As Exception

        End Try




        Me.SensorsListRemove.Items.Clear()

        ' Dim GreenServers As String = ""
        Try
            'load group sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then

                    Me.ComsEvent.Stop()
                    LastSensorWidth = 0
                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Dim MySensor As New SensorDetails
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        Try
                            'ServerDisplay

                            MySensor.Caption = MyString(0)
                            MySensor.ID = CInt(MyString(1))
                            MySensor.SiteID = CInt(MyString(2))
                            MySensor.ExtraData = CStr(MyString(5))
                            MySensor.ExtraData1 = CStr(MyString(6))
                            MySensor.ExtraValue = CDbl(MyString(7))
                            MySensor.ExtraValue1 = CDbl(MyString(8))
                            Try
                                MySensor.ExtraData2 = CStr(MyString(9))
                                MySensor.ExtraData3 = CStr(MyString(10))
                            Catch ex As Exception

                            End Try
                            Try
                                MySensor.DispExtraData1 = CStr(MyString(11))
                                MySensor.DispExtraData2 = CStr(MyString(12))
                            Catch ex As Exception

                            End Try
                            Try
                                MySensor.DispExtraValue = CDbl(MyString(13))
                                MySensor.DispExtraValue1 = CDbl(MyString(14))
                            Catch ex As Exception

                            End Try
                            SensorsCol.Add(MySensor, MySensor.ID.ToString)
                        Catch ex As Exception

                        End Try
                        Try
                            Dim MyListitem As New ListBoxItem

                            MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                            MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)

                            MyListitem.Content += " Sensor:" + MySensor.Caption
                            MyListitem.Content += " Type:" + MySensor.Type.ToString
                            MyListitem.Name = MySensor.ID.ToString
                            Me.SensorsListRemove.Items.Add(MyListitem)
                        Catch ex As Exception

                        End Try

                    Next
                End If
            End If

        Catch ex As Exception

        End Try





    End Sub





    Private Sub proxy_GetRemoteAlertHistoryCompleted(ByVal sender As Object, ByVal e As LiveMonitoring.GetRemoteAlertHistoryCompletedEventArgs) Handles proxy.GetRemoteAlertHistoryCompleted
        'Dim GreenServers As String = ""
        Try
            'load the sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()
                    'myServerList = New List(Of ServerDetails)

                    Me.BackPlaneBorder.Height = 350
                    Dim MyNew As New AlertList
                    MyNew.AlertsList.Items.Clear()
                    MyNew.Height = 340
                    MyNew.Width = 1200
                    DrawingObjects.Children.Clear()
                    DrawingObjects.Children.Add(MyNew)
                    For mycnt As Integer = 0 To e.Result.Count - 1
                        ''add
                        ''tems.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + MySensor.Status.ToString)
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        'Dim MySens As New ServerDetails
                        'MyString += myHist.ID.ToString + "|"
                        'MyString += myHist.AlertType.ToString + "|"
                        'MyString += myHist.Dest.ToString + "|"
                        'MyString += myHist.AlertMessage.ToString + "|"
                        'MyString += myHist.Sent.ToString
                        Dim MyListitem As New ListBoxItem
                        'MyListitem.
                        MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                        MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                        'BorderThickness="2" BorderBrush="Blue"
                        MyListitem.Content += " Dest:" + MyString(2)
                        MyListitem.Content += " AlertMessage:" + MyString(3)
                        MyListitem.Content += " Sent:" + MyString(4)
                        MyListitem.Content += " ID:" + MyString(0)
                        MyListitem.Content += " AlertType:" + MyString(1)
                        'myServerList.Add(MyNew)
                        MyNew.AlertsList.Items.Add(MyListitem)
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                    Next


                End If
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Sub proxy_GetSiteSensorsCompleted(ByVal sender As Object, ByVal e As LiveMonitoring.GetSiteSensorsCompletedEventArgs) Handles proxy.GetSiteSensorsCompleted
        Dim GreenServers As String = ""
        Try
            'load the sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()
                    'myServerList = New List(Of ServerDetails)
                    If MyDisplayType = 1 Then 'add total green
                        MyUndisplayed = 0
                        Dim MyNew As New Server
                        MyNew.ServerBlock.Height = ServerHeight
                        MyNew.ServerBlock.Width = ServerWidth
                        MyNew.ServerText.Text = "Total OK"
                        MyNew.SensorID = -1
                        MyNew.Name = "TotalGreen"
                        MyNew.Status = 0
                        If MyId = 0 And MyId Mod ServerRow = 0 Then
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                                If Double.IsNaN(Me.ActualHeight) = False Then
                                    Me.BackPlaneBorder.Height = ActualHeight
                                End If
                            End If
                            If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                                Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                            End If
                        End If
                        DrawingObjects.Children.Add(MyNew)
                        myServerList.Add(MyNew)
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                        MyId += 1
                        MyRow += 1
                    Else
                        If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                            Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                            If Double.IsNaN(Me.ActualHeight) = False Then
                                Me.BackPlaneBorder.Height = ActualHeight
                            End If
                        End If
                        If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                            Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                        End If
                    End If

                    For mycnt As Integer = 0 To e.Result.Count - 1
                        ''add
                        ''tems.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + MySensor.Status.ToString)
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        'Dim MySens As New ServerDetails
                        Dim MyNew As New Server
                        MyNew.ServerBlock.Height = ServerHeight
                        MyNew.ServerBlock.Width = ServerWidth

                        MyNew.ServerText.Text = MyString(0)
                        'MySens.SensorCaption = MyString(0)
                        'MySens.SensorID = CInt(MyString(1))
                        ' ToolTipService.SetToolTip(MyNew.ServerText, MyString(0))
                        Sensors += (MyString(1)) + "|"
                        MyNew.SensorID = CInt(MyString(1))
                        MyNew.Name = MyNew.SensorID.ToString
                        MyNew.Status = CInt(MyString(3))
                        AddHandler MyNew.ClickedMe, AddressOf SensorClickedEvent
                        AddHandler MyNew.ClickedMeAlert, AddressOf SensorClickedAlertEvent
                        If MyDisplayType = 1 And MyNew.Status = 0 Then 'add total green
                            MyUndisplayed += 1
                        End If
                        'MySens.Status = CInt(MyString(3))
                        'TheGrid
                        If MyId <> 0 And MyId Mod ServerRow = 0 Then
                            MyColumn += 1
                            MyRow = 0
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 10
                            End If
                            If Me.BackPlaneBorder.Height < ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) Then
                                Me.BackPlaneBorder.Height = ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) 'ActualHeight + ServerHeight + 10
                            Else
                                '
                            End If
                        End If
                        If MyId = 0 And MyId Mod ServerRow = 0 Then
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                                If Double.IsNaN(Me.ActualHeight) = False Then
                                    Me.BackPlaneBorder.Height = ActualHeight
                                End If
                            End If
                            If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                                Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                            End If
                        End If
                        myServerList.Add(MyNew)
                        DrawingObjects.Children.Add(MyNew)
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                        If MyDisplayType = 1 And MyNew.Status <> 0 Then 'add total green
                            MyId += 1
                            MyRow += 1
                        ElseIf MyDisplayType = 1 And MyNew.Status = 0 Then
                            GreenServers += MyNew.ServerText.Text + "|"
                            MyNew.Visibility = Windows.Visibility.Collapsed
                            MyNew.SetValue(Canvas.TopProperty, CDbl(0 * (ServerHeight + 10)))
                            MyNew.SetValue(Canvas.LeftProperty, CDbl((0 * (ServerWidth + 5))))
                            'MyNew.SetValue(
                        ElseIf MyDisplayType <> 1 Then
                            MyId += 1
                            MyRow += 1
                        End If
                    Next
                    If MyDisplayType = 1 Then 'add total green
                        Dim MyNew As Server = CType(TheGrid.FindName("TotalGreen"), Server)
                        MyNew.ServerCnt.Text = MyUndisplayed.ToString
                        ToolTipService.SetToolTip(MyNew.ServerBlock, GreenServers)
                        ToolTipService.SetToolTip(MyNew.ServerText, GreenServers)
                    End If

                End If
            End If

        Catch ex As Exception

        End Try
        SetupTimers()
        Try
            proxy.GetSiteSensorsStatusAsync(MySiteID, Sensors)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SensorClickedEvent(ByVal SensorId As Integer)
        'show detail ?
        '    Dim result As MessageBoxResult = _
        'MessageBox.Show("Details Sensor clicked " + SensorId.ToString, _
        '"MessageBox Sign", MessageBoxButton.OKCancel)
        proxy.LogItAsync("Silverlight", "Clicked Sensor:" + SensorId.ToString)
    End Sub
    Private Sub SensorClickedAlertEvent(ByVal SensorId As Integer)
        'log alert details
        proxy.LogItAsync("Silverlight", "Cleared alerting Sensor:" + SensorId.ToString)
        'Dim result As MessageBoxResult = _
        ' MessageBox.Show("Sensor Alert clicked " + SensorId.ToString, _
        '"MessageBox Sign", MessageBoxButton.OKCancel)
    End Sub
    Private Sub proxy_GetSiteSensorsStatusCompleted(ByVal sender As Object, ByVal e As LiveMonitoring.GetSiteSensorsStatusCompletedEventArgs) Handles proxy.GetSiteSensorsStatusCompleted
        Try
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()

                    If MyDisplayType = 1 Then
                        MyUndisplayed = 0
                    End If
                    For mycnt As Integer = 0 To e.Result.Count - 1

                        ''add
                        'items.Add(MySensor.ID.ToString + "|" + myInt.ToString + RetString)
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        'MySiteID.ToString + ":" +CInt(MyString(0)).ToString

                        Dim MyNew As Server = CType(TheGrid.FindName(MySiteID.ToString + ":" + CInt(MyString(0)).ToString), Server)
                        If IsNothing(MyNew) = False Then
                            Try
                                ToolTipService.SetToolTip(MyNew.ServerBlock, DateTime.Now.ToLongTimeString() + Environment.NewLine + "Res:" + MyString(2) + Environment.NewLine + "Status:" + MyString(1))
                                ToolTipService.SetToolTip(MyNew.ServerText, DateTime.Now.ToLongTimeString() + Environment.NewLine + "Res:" + MyString(2) + Environment.NewLine + "Status:" + MyString(1))
                                MyNew.Status = CInt(MyString(1))
                                If MyDisplayType = 1 And MyNew.Status = 0 Then 'add total green
                                    MyUndisplayed += 1
                                End If
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                    If MyDisplayType = 1 Then
                        ReDrawIcons()
                    End If
                End If
            End If

        Catch ex As Exception

        End Try

        'proxy.CloseAsync()
    End Sub
    Private Sub ReDrawIcons()
        'remove green
        'check/add a total
        'Dim MySens As New ServerDetails
        'Dim myObjects As Object
        Dim MyNew As Server
        Dim GreenServers As String = ""
        MyId = 1
        MyColumn = 0
        MyRow = 1
        Me.BackPlaneBorder.Height = ServerHeight + 50
        myServerList.Sort()
        For Each myObjects As Server In myServerList
            If TypeOf myObjects Is Server Then
                MyNew = CType(myObjects, Server)
                If MyNew.SensorID <> -1 Then
                    If MyId <> 0 And MyId Mod ServerRow = 0 Then
                        MyColumn += 1
                        MyRow = 0
                        If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                            Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 10
                        End If
                        If Me.BackPlaneBorder.Height < ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) Then
                            Me.BackPlaneBorder.Height = ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) 'ActualHeight + ServerHeight + 10
                        Else
                            '
                        End If
                    End If
                    If MyId = 0 And MyId Mod ServerRow = 0 Then
                        If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                            Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                            If Double.IsNaN(Me.ActualHeight) = False Then
                                Me.BackPlaneBorder.Height = ActualHeight
                            End If
                        End If
                        If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                            Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                        End If
                    End If
                    If MyNew.Status = 0 Then
                        GreenServers += MyNew.ServerText.Text + Environment.NewLine
                        MyNew.Visibility = Windows.Visibility.Collapsed
                        MyNew.SetValue(Canvas.TopProperty, CDbl(0 * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((0 * (ServerWidth + 5))))
                    Else
                        MyNew.Visibility = Windows.Visibility.Visible
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                    End If


                    If MyDisplayType = 1 And MyNew.Status <> 0 Then 'add total green
                        MyId += 1
                        MyRow += 1
                    ElseIf MyDisplayType = 1 And MyNew.Status = 0 Then
                        'MyNew.Visibility = Windows.Visibility.Collapsed
                    ElseIf MyDisplayType <> 1 Then
                        MyId += 1
                        MyRow += 1
                    End If
                End If
            End If
        Next
        Dim MyNew11 As Server = CType(TheGrid.FindName("TotalGreen"), Server)
        MyNew11.ServerCnt.Text = MyUndisplayed.ToString
        ToolTipService.SetToolTip(MyNew11.ServerBlock, GreenServers)
        ToolTipService.SetToolTip(MyNew11.ServerText, GreenServers)

        'Dim MyNew As New Server
        'If IsNothing(MyNew) = True Then
        '    MyNew = New Server
        '    MyNew.ServerText.Text = "TotalGreen"
        '    MySens.SensorCaption = MyNew.ServerText.Text
        '    MySens.SensorID = -1
        '    MySens.Status = MyNew.Status
        '    myServerList.Add(MySens)
        'End If
        'MyId = 0
        'MyColumn = 0
        'MyRow = 0
        'For MyCnt As Integer = 0 To myServerList.Count
        '    If myServerList.
        'Next

    End Sub


    Private Sub AddSens_Click(sender As Object, e As RoutedEventArgs) Handles AddSens.Click
        Try
            If Me.SensorsList.SelectedItems.Count = 0 Then
                Dim result As MessageBoxResult = MessageBox.Show("Please select a sensor ! ", "liveMon Alert", MessageBoxButton.OK)
            Else
                proxy.GetSpecificSensorDisplayAsync(CType(Me.SensorsList.SelectedItem, ListBoxItem).Name, Me.MySiteID, CInt(txtSensorScale.Value))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub proxy_GetSpecificSensorDisplayCompleted(sender As Object, e As LiveMonitoring.GetSpecificSensorDisplayCompletedEventArgs) Handles proxy.GetSpecificSensorDisplayCompleted
        Dim GreenServers As String = ""
        Try
            'load the sensors
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()
                    'myServerList = New List(Of ServerDetails)
                    If MyDisplayType = 1 Then 'add total green
                        MyUndisplayed = 0
                        Dim MyNew As New Server
                        MyNew.ServerBlock.Height = ServerHeight
                        MyNew.ServerBlock.Width = ServerWidth
                        MyNew.ServerText.Text = "Total OK"
                        MyNew.SensorID = -1
                        MyNew.Name = "TotalGreen"
                        MyNew.Status = 0
                        If MyId = 0 And MyId Mod ServerRow = 0 Then
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                                If Double.IsNaN(Me.ActualHeight) = False Then
                                    Me.BackPlaneBorder.Height = ActualHeight
                                End If
                            End If
                            If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                                Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                            End If
                        End If
                        DrawingObjects.Children.Add(MyNew)
                        myServerList.Add(MyNew)
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                        MyId += 1
                        MyRow += 1
                    Else
                        If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                            Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                            If Double.IsNaN(Me.ActualHeight) = False Then
                                Me.BackPlaneBorder.Height = ActualHeight
                            End If
                        End If
                        If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                            Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                        End If
                    End If

                    For mycnt As Integer = 0 To e.Result.Count - 1
                        ''add
                        ''tems.Add(MySensor.Caption + "|" + MySensor.ID.ToString + "|" + MySensor.SiteID.ToString + "|" + MySensor.Status.ToString)
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        'Dim MySens As New ServerDetails
                        Dim MyNew As New Server
                        MyNew.ServerBlock.Height = ServerHeight
                        MyNew.ServerBlock.Width = ServerWidth

                        MyNew.ServerText.Text = MyString(0)
                        'MySens.SensorCaption = MyString(0)
                        'MySens.SensorID = CInt(MyString(1))
                        ' ToolTipService.SetToolTip(MyNew.ServerText, MyString(0))
                        Sensors += (MyString(1)) + "|"
                        MyNew.SensorID = CInt(MyString(1))
                        'MyNew.Name = MyNew.SensorID.ToString
                        ' MyNew.SensorID = CInt(MyString(1))
                        MyNew.Name = MySiteID.ToString + ":" + MyNew.SensorID.ToString
                        MyNew.Status = CInt(MyString(3))
                        AddHandler MyNew.ClickedMe, AddressOf SensorClickedEvent
                        AddHandler MyNew.ClickedMeAlert, AddressOf SensorClickedAlertEvent
                        If MyDisplayType = 1 And MyNew.Status = 0 Then 'add total green
                            MyUndisplayed += 1
                        End If
                        'MySens.Status = CInt(MyString(3))
                        'TheGrid
                        If MyId <> 0 And MyId Mod ServerRow = 0 Then
                            MyColumn += 1
                            MyRow = 0
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 10
                            End If
                            If Me.BackPlaneBorder.Height < ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) Then
                                Me.BackPlaneBorder.Height = ((ServerHeight * (MyColumn + 1)) + ((MyColumn + 1) * 20)) 'ActualHeight + ServerHeight + 10
                            Else
                                '
                            End If
                        End If
                        If MyId = 0 And MyId Mod ServerRow = 0 Then
                            If Double.IsNaN(Me.BackPlaneBorder.Height) Then
                                Me.BackPlaneBorder.Height = 0 'ActualHeight + ServerHeight + 40
                                If Double.IsNaN(Me.ActualHeight) = False Then
                                    Me.BackPlaneBorder.Height = ActualHeight
                                End If
                            End If
                            If Me.BackPlaneBorder.Height < (ServerHeight * (MyColumn + 1)) Then
                                Me.BackPlaneBorder.Height = ActualHeight + ServerHeight + 20
                            End If
                        End If
                        myServerList.Add(MyNew)
                        DrawingObjects.Children.Add(MyNew)
                        MyNew.SetValue(Canvas.TopProperty, CDbl(MyColumn * (ServerHeight + 10)))
                        MyNew.SetValue(Canvas.LeftProperty, CDbl((MyRow * (ServerWidth + 5))))
                        If MyDisplayType = 1 And MyNew.Status <> 0 Then 'add total green
                            MyId += 1
                            MyRow += 1
                        ElseIf MyDisplayType = 1 And MyNew.Status = 0 Then
                            GreenServers += MyNew.ServerText.Text + "|"
                            MyNew.Visibility = Windows.Visibility.Collapsed
                            MyNew.SetValue(Canvas.TopProperty, CDbl(0 * (ServerHeight + 10)))
                            MyNew.SetValue(Canvas.LeftProperty, CDbl((0 * (ServerWidth + 5))))
                            'MyNew.SetValue(
                        ElseIf MyDisplayType <> 1 Then
                            MyId += 1
                            MyRow += 1
                        End If
                    Next
                    If MyDisplayType = 1 Then 'add total green
                        Dim MyNew As Server = CType(TheGrid.FindName("TotalGreen"), Server)
                        MyNew.ServerCnt.Text = MyUndisplayed.ToString
                        ToolTipService.SetToolTip(MyNew.ServerBlock, GreenServers)
                        ToolTipService.SetToolTip(MyNew.ServerText, GreenServers)
                    End If

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Header_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Header.MouseLeftButtonDown
        If isInEditInEffect Then
            Try
                Clickpos = e.GetPosition(Nothing)
                PopupAddSensor.IsOpen = True
                ' txtBackImage.Text = Me.DisplayImage
                
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub Header_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Header.MouseRightButtonDown

    End Sub

    Private Sub cancelAddSens_Click(sender As Object, e As RoutedEventArgs) Handles cancelAddSens.Click
        Try
            PopupAddSensor.IsOpen = False
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BtnRemoveSens_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles BtnRemoveSens.Click

        Try
            If Me.SensorsListRemove.SelectedItems.Count = 0 Then
                Dim result As MessageBoxResult = MessageBox.Show("Please select a sensor ! ", "liveMon Alert", MessageBoxButton.OK)
                Exit Sub
            Else
                ' proxy.GetSpecificSensorDisplayAsync(CType(Me.SensorsList.SelectedItem, ListBoxItem).Name, Me.MySiteID, CInt(txtSensorScale.Value))

                'TryCast(sender._sensor, SensorDetails)
                'Dim sensor As SensorDetails = CType(Me.SensorsListRemove.SelectedItem, SensorDetails)

                If MessageBox.Show("Are you sure to remove this sensor: " + CType(Me.SensorsListRemove.SelectedItem, ListBoxItem).Content + " ? ", "liveMon Alert", MessageBoxButton.OK) = MessageBoxResult.OK Then
                    Dim sensorID As Integer = CType(CType(Me.SensorsListRemove.SelectedItem, ListBoxItem).Name, Integer)

                    proxy.RemoveSensorAsync(Me.MySiteID, sensorID)

                    PopupRemoveSensor.IsOpen = False
                    ' MyNew.Name = MySiteID.ToString + ":" + MyNew.SensorID.ToString
                    'myServerList.Add(MyNew)
                    'DrawingObjects.Children.Add(MyNew)
                    For MyInt As Integer = 0 To myServerList.Count - 1
                        If myServerList.Item(MyInt).SensorID = sensorID Then
                            Try
                                DrawingObjects.Children.Remove(myServerList.Item(MyInt))
                                myServerList.RemoveAt(MyInt)
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
               
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub btnCancelRemSens_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancelRemSens.Click
        Try
            PopupRemoveSensor.IsOpen = False
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub Element_MouseRightButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
    '    If isInEditInEffect Then
    '        Dim MySens As SensorDetails = TryCast(sender._sensor, SensorDetails)
    '        Try
    '            PopupRemoveSensor.IsOpen = True
    '            LblSensorID.Text = MySens.Caption
    '            LblSensorID.Tag = MySens.ID
    '        Catch ex As Exception

    '        End Try
    '    End If

    'End Sub

    Private Sub btnFilterSensors_Click(sender As Object, e As RoutedEventArgs) Handles btnFilterSensors.Click
        If Me.txtFilterSensors.Text.Trim = "" Then
            Try
                Dim MySensor As New SensorDetails
                For Each MySensor In SensorsCol
                    'If MySensor.Caption.ToUpper.Contains(Me.txtFilterSensors.Text.ToUpper.Trim) Then
                    Try
                        Dim MyListitem As New ListBoxItem
                        'MyListitem.
                        MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                        MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                        'BorderThickness="2" BorderBrush="Blue"
                        MyListitem.Content += " Sensor:" + MySensor.Caption
                        MyListitem.Content += " Type:" + MySensor.Type.ToString
                        MyListitem.Name = MySensor.ID.ToString
                        'MyListitem.Content += " ID:" + MyString(0)
                        'MyListitem.Content += " AlertType:" + MyString(1)
                        'myServerList.Add(MyNew)
                        Me.SensorsList.Items.Add(MyListitem)
                        ' Me.SensorsListRemove.Items.Add(MyListitem)
                    Catch ex As Exception

                    End Try
                    'End If
                Next
            Catch ex As Exception

            End Try
        Else
            Me.SensorsList.Items.Clear()
            Dim MySensor As New SensorDetails
            For Each MySensor In SensorsCol
                If MySensor.Caption.ToUpper.Contains(Me.txtFilterSensors.Text.ToUpper.Trim) Then
                    Try
                        Dim MyListitem As New ListBoxItem
                        'MyListitem.
                        MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                        MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                        'BorderThickness="2" BorderBrush="Blue"
                        MyListitem.Content += " Sensor:" + MySensor.Caption
                        MyListitem.Content += " Type:" + MySensor.Type.ToString
                        MyListitem.Name = MySensor.ID.ToString
                        'MyListitem.Content += " ID:" + MyString(0)
                        'MyListitem.Content += " AlertType:" + MyString(1)
                        'myServerList.Add(MyNew)
                        Me.SensorsList.Items.Add(MyListitem)
                        ' Me.SensorsListRemove.Items.Add(MyListitem)
                    Catch ex As Exception

                    End Try
                End If
            Next
        End If
    End Sub
End Class
