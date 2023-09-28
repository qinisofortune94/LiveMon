Imports System.IO
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Collections
Imports System.Windows.Media.Imaging
Imports System.IO.IsolatedStorage
Imports SLServerDisplay.LiveMonitoring

'Imports IPMonInterface
Partial Public Class Page
    Inherits UserControl
    'Private proxy As New IPMonInterface.SLIPmonInterfaceSoapClient
    Private WithEvents Sitesproxy As SLIPmonInterfaceSVCClient
    Private MaxPage As Integer = 0
    Private LastWebDisplayed = 0
    Private LastMediaDisplayed = 0
    Private MyPages As New List(Of ServerDisplay)

    Private CurVisiblePage As Integer = 1
    Private Shared SLSettings As New SLSettings
    Private WithEvents MyPageTimer As New System.Windows.Threading.DispatcherTimer
    Public _parameters As IDictionary(Of String, String)
    'Public WithEvents SQLBackupTimer As New System.Timers.Timer()
    'Private ServiceURI As String = "http://localhost:2029/website/svc"
    'Public ReadOnly Property GetServiceURI() As String
    '    Get
    '        Return ServiceURI
    '    End Get
    'End Property
    Public InitWidth As Integer
    Public InitHeight As Integer
    Private _snowFlakes As New List(Of Snowflake)()
    Private _snowflakeTimer As New Storyboard()
    Private _rand As New Random(DateTime.Now.Millisecond)
    Private _newFlakeCount As Integer = 3
    Private _wind As Integer = 20
    Private OldX As Double
    Private OldY As Double
    Public Function LoadGrid(ByVal PageNo As Integer) As System.Windows.Controls.Grid
        Dim ControlName As String = ""
        'If PageNo > 1 Then
        '    ControlName = "LayoutRoot" + (PageNo).ToString
        'Else
        '    ControlName = "LayoutRoot"
        'End If
        ControlName = "LayoutRoot" + (PageNo).ToString

        If IsNothing(PageRoot.FindName(ControlName)) = True Then
            Dim LayoutRoot As New System.Windows.Controls.Grid
            Dim Mycol As New System.Windows.Controls.ColumnDefinition
            LayoutRoot.Name = ControlName
            Mycol.Width = System.Windows.GridLength.Auto
            ' Dim mySensor As SilverlightIRemotelib.IRemoteLib.SensorDetails = CType(IPUnSerialise(, GetType(SilverlightIRemotelib.IRemoteLib.SensorDetails)), SilverlightIRemotelib.IRemoteLib.SensorDetails)
            LayoutRoot.ColumnDefinitions.Add(Mycol)
            'Dim myScaletransform As New ScaleTransform
            'myScaletransform.
            'LayoutRoot.RenderTransform.SetValue(ScaleTransform.
            'img.RenderTransform.SetValue(ScaleTransform.CenterXProperty, img.Width / 2);

            'add ssecond col
            Dim Mycol1 As New System.Windows.Controls.ColumnDefinition
            Mycol1.Width = System.Windows.GridLength.Auto
            LayoutRoot.ColumnDefinitions.Add(Mycol1)
            'TODO Change visible
            PageRoot.Children.Add(LayoutRoot)
            If PageNo = CurVisiblePage Then
                LayoutRoot.Visibility = Windows.Visibility.Visible
            Else
                LayoutRoot.Visibility = Windows.Visibility.Collapsed
            End If
            MaxPage += 1
            Return LayoutRoot
        Else
            Return CType(PageRoot.FindName(ControlName), System.Windows.Controls.Grid)
        End If

    End Function
    Private Sub SetupTimers()
        Try
            MyPageTimer.Interval = New TimeSpan(0, 0, SLSettings.PageTimer)
            AddHandler MyPageTimer.Tick, AddressOf OnTimer
            MyPageTimer.Start()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ResetPageTimer(ByVal NextPage As Double)
        Try
            MyPageTimer.Stop()
            MyPageTimer.Interval = New TimeSpan(0, 0, NextPage)
            MyPageTimer.Start()
        Catch ex As Exception
            MyPageTimer.Interval = New TimeSpan(0, 0, SLSettings.PageTimer)
            If MyPageTimer.IsEnabled = False Then MyPageTimer.Start()

        End Try
    End Sub
    Private Sub ShowMediaPage(ByVal URL As String, ByVal PageNo As Integer)
        Try
            'Dim wb As System.Windows.Browser.HtmlElement = System.Windows.Browser.HtmlPage.Document.GetElementById("iframeReport")
            Dim refreshURL As Boolean = False
            Dim ControlName As String = "BrowserRoot" + (PageNo).ToString
            If PageNo <> LastMediaDisplayed Then
                HideMediaPage(LastMediaDisplayed)
            Else
                refreshURL = True
            End If
            LastMediaDisplayed = PageNo
            If IsNothing(PageRoot.FindName(ControlName)) = True Then
                Dim wb As New MediaPlaya 'System.Windows.Controls.MediaElement
                wb.Name = ControlName
                If wb IsNot Nothing Then
                    Try
                        Dim MyPage As ServerDisplay = FindDisplay(PageNo)
                        Try
                            wb.VideoPlaya.Source = (New Uri(URL, UriKind.Absolute))
                            ' wb.Navigate(New Uri(URL, UriKind.Absolute))
                            wb.Height = 450
                            wb.Width = 620
                            ' wb.CanSeek
                            'wb.CanPause
                            'If MyPage.DisplayHeight > 0 Then
                            '    wb.Height = 900
                            '    Me.scaleTransform.ScaleY = 900 / MyPage.DisplayHeight
                            'Else
                            '    wb.Height = 900
                            '    Me.scaleTransform.ScaleY = InitHeight / 625
                            'End If
                            'If MyPage.DisplayWidth > 0 Then
                            '    wb.Width = 1240 'MyPage.DisplayWidth
                            '    Me.scaleTransform.ScaleX = 1240 / MyPage.DisplayWidth
                            'Else
                            '    wb.Width = 1240
                            '    Me.scaleTransform.ScaleX = InitWidth / 1242
                            'End If
                            Dim value As Duration
                            value = wb.VideoPlaya.NaturalDuration
                            If value.TimeSpan.Seconds > 0 Then
                                ResetPageTimer(value.TimeSpan.Seconds)
                            End If
                            'value.TimeSpan.Seconds
                            wb.Visibility = Windows.Visibility.Visible
                            PageRoot.Children.Add(wb)
                        Catch ex As Exception

                        End Try

                    Catch ex As Exception

                    End Try

                    'If wb.GetAttribute("src") <> URL Then
                    '    wb.SetAttribute("src", URL)
                    'End If
                    'wb.SetStyleAttribute("visibility", "visible")

                End If
            Else
                Dim wb As MediaPlaya = CType(PageRoot.FindName(ControlName), MediaPlaya)
                Try
                    Dim MyPage As ServerDisplay = FindDisplay(PageNo)
                    'Try
                    '    If MyPage.DisplayHeight > 0 Then
                    '        Me.scaleTransform.ScaleY = 900 / MyPage.DisplayHeight
                    '    Else
                    '        Me.scaleTransform.ScaleY = InitHeight / 625
                    '    End If
                    '    If MyPage.DisplayWidth > 0 Then
                    '        Me.scaleTransform.ScaleX = 1240 / MyPage.DisplayWidth
                    '    Else
                    '        Me.scaleTransform.ScaleX = InitWidth / 1242
                    '    End If
                    'Catch ex As Exception

                    'End Try

                Catch ex As Exception

                End Try
                If refreshURL Then
                    wb.VideoPlaya.Source = (New Uri(URL, UriKind.Absolute))
                    'wb.re
                End If
                wb.Visibility = Windows.Visibility.Visible
                Dim value As Duration
                value = wb.VideoPlaya.NaturalDuration
                If value.TimeSpan.Seconds > 0 Then
                    ResetPageTimer(value.TimeSpan.Seconds)
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub HideMediaPage(ByVal PageNo As Integer)
        Try
            Dim ControlName As String = "BrowserRoot" + (PageNo).ToString
            Dim wb As MediaPlaya 'System.Windows.Controls.MediaElement
            wb = PageRoot.FindName(ControlName)
            'Dim wb As System.Windows.Browser.HtmlElement = System.Windows.Browser.HtmlPage.Document.GetElementById("iframeReport")
            If wb IsNot Nothing Then
                'wb.SetAttribute("src", "")
                ' wb.SetStyleAttribute("visibility", "hidden")
                wb.Visibility = Windows.Visibility.Collapsed
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ShowWebPage(ByVal URL As String, ByVal PageNo As Integer)
        Try
            'Dim wb As System.Windows.Browser.HtmlElement = System.Windows.Browser.HtmlPage.Document.GetElementById("iframeReport")
            Dim refreshURL As Boolean = False
            Dim ControlName As String = "BrowserRoot" + (PageNo).ToString
            If PageNo <> LastWebDisplayed Then
                HideWebPage(LastWebDisplayed)
            Else
                refreshURL = True
            End If
            LastWebDisplayed = PageNo
            If IsNothing(PageRoot.FindName(ControlName)) = True Then
                Dim wb As New System.Windows.Controls.WebBrowser
                wb.Name = ControlName
                If wb IsNot Nothing Then
                    Try
                        Dim MyPage As ServerDisplay = FindDisplay(PageNo)
                        Try
                            wb.Navigate(New Uri(URL, UriKind.Absolute))
                            wb.Height = 900
                            wb.Width = 1240
                            'If MyPage.DisplayHeight > 0 Then
                            '    wb.Height = 900
                            '    Me.scaleTransform.ScaleY = 900 / MyPage.DisplayHeight
                            'Else
                            '    wb.Height = 900
                            '    Me.scaleTransform.ScaleY = InitHeight / 625
                            'End If
                            'If MyPage.DisplayWidth > 0 Then
                            '    wb.Width = 1240 'MyPage.DisplayWidth
                            '    Me.scaleTransform.ScaleX = 1240 / MyPage.DisplayWidth
                            'Else
                            '    wb.Width = 1240
                            '    Me.scaleTransform.ScaleX = InitWidth / 1242
                            'End If

                            wb.Visibility = Windows.Visibility.Visible
                            PageRoot.Children.Add(wb)
                        Catch ex As Exception

                        End Try

                    Catch ex As Exception

                    End Try

                    'If wb.GetAttribute("src") <> URL Then
                    '    wb.SetAttribute("src", URL)
                    'End If
                    'wb.SetStyleAttribute("visibility", "visible")

                End If
            Else
                Dim wb As System.Windows.Controls.WebBrowser = CType(PageRoot.FindName(ControlName), System.Windows.Controls.WebBrowser)
                Try
                    Dim MyPage As ServerDisplay = FindDisplay(PageNo)
                    'Try
                    '    If MyPage.DisplayHeight > 0 Then
                    '        Me.scaleTransform.ScaleY = 900 / MyPage.DisplayHeight
                    '    Else
                    '        Me.scaleTransform.ScaleY = InitHeight / 625
                    '    End If
                    '    If MyPage.DisplayWidth > 0 Then
                    '        Me.scaleTransform.ScaleX = 1240 / MyPage.DisplayWidth
                    '    Else
                    '        Me.scaleTransform.ScaleX = InitWidth / 1242
                    '    End If
                    'Catch ex As Exception

                    'End Try

                Catch ex As Exception

                End Try
                If refreshURL Then
                    wb.Navigate(New Uri(URL, UriKind.Absolute))
                    'wb.re
                End If
                wb.Visibility = Windows.Visibility.Visible
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub HideWebPage(ByVal PageNo As Integer)
        Try
            Dim ControlName As String = "BrowserRoot" + (PageNo).ToString
            Dim wb As System.Windows.Controls.WebBrowser
            wb = PageRoot.FindName(ControlName)
            'Dim wb As System.Windows.Browser.HtmlElement = System.Windows.Browser.HtmlPage.Document.GetElementById("iframeReport")
            If wb IsNot Nothing Then
                'wb.SetAttribute("src", "")
                ' wb.SetStyleAttribute("visibility", "hidden")
                wb.Visibility = Windows.Visibility.Collapsed
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function FindDisplay(ByVal MyPageno As Integer) As ServerDisplay
        Try
            For Each MyServer As ServerDisplay In MyPages
                If MyServer.ScreenSetting = MyPageno Then
                    Return MyServer
                    Exit Function
                End If
            Next

        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function
    Private Sub OnTimer(ByVal sender As Object, ByVal e As EventArgs)
        'MyTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
        'DoWork()
        'proxy.OpenAsync()

        Try
            'MaxPage
            Dim NextPageName As String
            Dim CurPageName As String
            If MaxPage = 1 Then Exit Sub
            If (CurVisiblePage + 1) <= MaxPage Then
                CurPageName = "LayoutRoot" + CurVisiblePage.ToString
                CurVisiblePage += 1
                NextPageName = "LayoutRoot" + CurVisiblePage.ToString
            Else
                CurPageName = "LayoutRoot" + CurVisiblePage.ToString
                CurVisiblePage = 1
                NextPageName = "LayoutRoot" + CurVisiblePage.ToString
            End If
            Dim MyDisplay As ServerDisplay = FindDisplay(CurVisiblePage)
            If IsNothing(MyDisplay) = False Then
                'resize
                Try
                    If MyDisplay.DisplayHeight > 0 Then
                        Me.scaleTransform.ScaleY = 900 / MyDisplay.DisplayHeight
                    Else
                        Me.scaleTransform.ScaleY = InitHeight / 625
                    End If
                    If MyDisplay.DisplayWidth > 0 Then
                        Me.scaleTransform.ScaleX = 1240 / MyDisplay.DisplayWidth
                    Else
                        Me.scaleTransform.ScaleX = InitWidth / 1242
                    End If
                Catch ex As Exception

                End Try

                Try
                    'set the page timer
                    If MyDisplay.Extravalue1 > 0 Then
                        ResetPageTimer(MyDisplay.Extravalue1)
                    Else 'use default
                        ResetPageTimer(SLSettings.PageTimer)
                    End If
                Catch ex As Exception

                End Try
                If MyDisplay.DisplayType = 3 Then
                    'LastWebDisplayed = CurVisiblePage
                    ShowWebPage(MyDisplay.ExtraData, CurVisiblePage)
                Else
                    HideWebPage(LastWebDisplayed)
                End If
                If MyDisplay.DisplayType = 4 Then
                    'LastWebDisplayed = CurVisiblePage
                    ShowMediaPage(MyDisplay.ExtraData, CurVisiblePage)
                Else
                    HideMediaPage(LastMediaDisplayed)
                End If
            Else
                HideWebPage(LastWebDisplayed)
                HideMediaPage(LastMediaDisplayed)
            End If
            If IsNothing(PageRoot.FindName(CurPageName)) = False Then
                'CType(PageRoot.FindName(CurPageName), Windows.Controls.Grid).Visibility = Windows.Visibility.Collapsed
                'GridHide.SetValue(Storyboard.TargetNameProperty, CurPageName)
                Try
                    AnimateCollapsed(CType(PageRoot.FindName(CurPageName), Windows.Controls.Grid))
                    'GridHide.Stop()
                    'Storyboard.SetTargetName(GridHide, CurPageName)
                    'GridHide.Begin()
                Catch ex As Exception

                End Try
            End If
            If IsNothing(PageRoot.FindName(NextPageName)) = False Then
                'CType(PageRoot.FindName(NextPageName), Windows.Controls.Grid).Visibility = Windows.Visibility.Visible
                ' GridShow.SetValue(Storyboard.TargetNameProperty, NextPageName)
                Try
                    AnimateVisible(CType(PageRoot.FindName(NextPageName), Windows.Controls.Grid))
                    'GridShow.Stop()
                    'Storyboard.SetTargetName(GridShow, NextPageName)
                    'GridShow.Begin()
                Catch ex As Exception

                End Try
            End If

        Catch ex As Exception

        End Try
        'Me.FindName("1").SiteName.Text = DateTime.Now.ToLongTimeString()
    End Sub
    Public Shared Sub AnimateVisible(ByVal ctlToAnimate As UIElement)

        ctlToAnimate.Visibility = Visibility.Visible
        ctlToAnimate.Opacity = 0

        Dim sb As New Storyboard()
        Dim daY As New DoubleAnimationUsingKeyFrames()

        daY.BeginTime = TimeSpan.FromSeconds(0)
        Dim sDKF1 As New SplineDoubleKeyFrame()

        sDKF1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))
        sDKF1.Value = 0

        Dim sDKF2 As New SplineDoubleKeyFrame()
        sDKF2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2))
        sDKF2.Value = 1

        daY.KeyFrames.Add(sDKF1)

        daY.KeyFrames.Add(sDKF2)

        Storyboard.SetTargetProperty(daY, New PropertyPath("(UIElement.Opacity)"))
        Storyboard.SetTarget(daY, ctlToAnimate)
        sb.Children.Add(daY)

        sb.Begin()

    End Sub
    Public Shared Sub AnimateCollapsed(ByVal ctlToAnimate As UIElement)
        ctlToAnimate.Opacity = 1
        Dim sb As New Storyboard()
        Dim daY As New DoubleAnimationUsingKeyFrames()
        daY.BeginTime = TimeSpan.FromSeconds(0)
        Dim sDKF1 As New SplineDoubleKeyFrame()
        sDKF1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))
        sDKF1.Value = 1
        Dim sDKF2 As New SplineDoubleKeyFrame()

        sDKF2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500))
        sDKF2.Value = 0

        Dim sDKF3 As New SplineDoubleKeyFrame()
        sDKF3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
        sDKF3.Value = 1

        daY.KeyFrames.Add(sDKF1)

        daY.KeyFrames.Add(sDKF2)

        daY.KeyFrames.Add(sDKF3)

        Storyboard.SetTargetProperty(daY, New PropertyPath("(UIElement.Opacity)"))
        Storyboard.SetTarget(daY, ctlToAnimate)
        sb.Children.Add(daY)
        Dim objectAnimationUsingKeyFrames As New ObjectAnimationUsingKeyFrames()
        objectAnimationUsingKeyFrames.BeginTime = TimeSpan.FromMilliseconds(500)


        Dim dOKF1 As New DiscreteObjectKeyFrame()

        dOKF1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))
        dOKF1.Value = Visibility.Visible

        Dim dOKF2 As New DiscreteObjectKeyFrame()
        dOKF2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))

        dOKF2.Value = Visibility.Collapsed


        objectAnimationUsingKeyFrames.KeyFrames.Add(dOKF1)

        objectAnimationUsingKeyFrames.KeyFrames.Add(dOKF2)

        Storyboard.SetTargetProperty(objectAnimationUsingKeyFrames, New PropertyPath("(UIElement.Visibility)"))
        Storyboard.SetTarget(objectAnimationUsingKeyFrames, ctlToAnimate)
        sb.Children.Add(objectAnimationUsingKeyFrames)
        sb.Begin()
    End Sub
    'Public Sub FadeGrid(ByVal GridName As String)
    '    Try
    '        If Resources.Contains(GridName + "Story") = False Then
    '            Dim Storyboard As New Storyboard()

    '            Dim Animation As New DoubleAnimation()
    '            Storyboard.SetTargetName(Animation, GridName)
    '            Storyboard.SetTargetProperty(Animation, New PropertyPath(Grid.OpacityProperty))
    '            Animation.To = 0
    '            Animation.Duration = TimeSpan.FromSeconds(2)
    '            Storyboard.Children.Add(Animation)

    '            Dim Animation1 As New DoubleAnimation()
    '            Storyboard.SetTargetName(Animation1, GridName)
    '            Storyboard.SetTargetProperty(Animation, New PropertyPath(Grid.VisibilityProperty))
    '            Animation.To = 0
    '            Animation.BeginTime = TimeSpan.FromSeconds(2)
    '            Animation.Duration = TimeSpan.FromSeconds(2)
    '            Storyboard.Children.Add(Animation1)

    '            Resources.Add(GridName + "FadeStory", Storyboard)
    '            Storyboard.Begin()
    '        Else 'it exists
    '            Dim Storyboard As Storyboard = Resources.Item(GridName + "Story")
    '            Storyboard.Begin()

    '        End If

    '        'Note that this second animation starts after the first animation (because of BeginTime). It references the same image as the first animation!
    '        'Animation = New DoubleAnimation()
    '        'Storyboard.SetTargetName(Animation, "Image1")
    '        'Storyboard.SetTargetProperty(Animation, "Opacity")
    '        'Animation.To = 0
    '        'Animation.Duration = TimeSpan.FromSeconds(2)
    '        'Animation.BeginTime = TimeSpan.FromSeconds(2)
    '        'Storyboard.Children.Add(Animation)

    '    Catch ex As Exception

    '    End Try




    'End Sub
    'Public Sub LoadSettings()
    '    'Dim output As StringBuilder = New StringBuilder()

    '    ' Create an XmlReader
    '    Using reader As XmlReader = XmlReader.Create("SLIPMonSettings.xml")
    '        'Dim ws As XmlWriterSettings = New XmlWriterSettings()
    '        'ws.Indent = True
    '        'Using writer As XmlWriter = XmlWriter.Create(output, ws)

    '        ' Parse the file and display each of the nodes.
    '        While reader.Read()
    '            Select Case reader.NodeType
    '                Case XmlNodeType.Element
    '                    'writer.WriteStartElement(reader.Name)
    '                Case XmlNodeType.Text
    '                    'writer.WriteString(reader.Value)
    '                    ServiceURI = reader.Value
    '                Case XmlNodeType.XmlDeclaration
    '                Case XmlNodeType.ProcessingInstruction
    '                    'writer.WriteProcessingInstruction(reader.Name, reader.Value)
    '                Case XmlNodeType.Comment
    '                    'writer.WriteComment(reader.Value)
    '                Case XmlNodeType.EndElement
    '                    'writer.WriteFullEndElement()
    '            End Select
    '        End While
    '        'End Using
    '    End Using
    '    'OutputTextBlock.Text = output.ToString()
    'End Sub
    Private Function GetParameters(ByVal ParameterID As String) As String
        Return _parameters(ParameterID)
    End Function
    'MyMenuButtClicked
    Private Sub MyMenuButtClicked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        ' App.Current.RootVisual = New MainMenu(_parameters)
        MyPageTimer.Stop()
        Me.SpinLogo.Stop()
        Try
            HideWebPage(LastWebDisplayed)
            HideMediaPage(LastMediaDisplayed)
        Catch ex As Exception

        End Try


        Dim MyMenuPage As ChildMenu = New ChildMenu(_parameters)
        AddHandler MyMenuPage.Closed, AddressOf MymenuPage_Closed
        MyMenuPage.Show()
    End Sub
    Private Sub MyButtClicked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim MyButton As Windows.Controls.Button = CType(e.OriginalSource, Windows.Controls.Button)
        Dim initParams As IDictionary(Of String, String) = Me._parameters
        Try
            'If initParams.Contains("ServiceRef") Then
            initParams.Remove("ServiceRef")
            ' End If
            ' If initParams.Contains("DisplayID") Then
            initParams.Remove("DisplayID")
            'End If
        Catch ex As Exception

        End Try
        Dim Mybutt As Windows.Controls.Button = CType(sender, Windows.Controls.Button)
        Dim Mybutt1 As Windows.Controls.Button = imageroot.FindName("txtMyButtCancel")
        Dim MyText1 As Windows.Controls.TextBox = imageroot.FindName("txtServiceRef")
        Dim MyText2 As Windows.Controls.TextBox = imageroot.FindName("txtDisplayID")
        Dim MyRet As Integer
        If MyText1.Text = "" Or Integer.TryParse(MyText2.Text, MyRet) = False Then
            MessageBox.Show("Invalid Input", "Dashboard", MessageBoxButton.OK)
            Exit Sub
        End If

        initParams.Add("ServiceRef", MyText1.Text)
        initParams.Add("DisplayID", CInt(MyText2.Text))
        Try
            Using file As IsolatedStorageFile =
            IsolatedStorageFile.GetUserStoreForApplication()
                Using stream As New IsolatedStorageFileStream("initParams.txt", System.IO.FileMode.Create, file)
                    Dim serializer As New DataContractSerializer(GetType(Dictionary(Of String, String)))
                    serializer.WriteObject(stream, initParams)
                End Using
            End Using
        Catch ex As Exception

        End Try
        MyText1.Visibility = Windows.Visibility.Collapsed
        MyText2.Visibility = Windows.Visibility.Collapsed
        Mybutt.Visibility = Windows.Visibility.Collapsed
        Mybutt1.Visibility = Windows.Visibility.Collapsed
        Dim MyMenuButt As Windows.Controls.Button = imageroot.FindName("txtMyMenuButt")
        MyMenuButt.Visibility = Windows.Visibility.Collapsed
    End Sub

    Private Sub MyButtCancel(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim Mybutt As Windows.Controls.Button = imageroot.FindName("txtMyButt")
        Dim Mybutt1 As Windows.Controls.Button = imageroot.FindName("txtMyButtCancel")
        Dim MyText1 As Windows.Controls.TextBox = imageroot.FindName("txtServiceRef")
        Dim MyText2 As Windows.Controls.TextBox = imageroot.FindName("txtDisplayID")
        MyText1.Visibility = Windows.Visibility.Collapsed
        MyText2.Visibility = Windows.Visibility.Collapsed
        Mybutt.Visibility = Windows.Visibility.Collapsed
        Mybutt1.Visibility = Windows.Visibility.Collapsed
        Dim MyMenuButt As Windows.Controls.Button = imageroot.FindName("txtMyMenuButt")
        MyMenuButt.Visibility = Windows.Visibility.Collapsed
    End Sub
    Public Sub New(ByVal parameters As IDictionary(Of String, String))
        Me._parameters = parameters
        Try
            If Me._parameters.ContainsKey("ServiceRef") = False Then
                Me._parameters.Add("ServiceRef", SLSettings.ServiceURL)
                Me._parameters.Add("DisplayID", SLSettings.DisplayID)
                'initParams = e.InitParams
                Try
                    Using file As IsolatedStorageFile =
                    IsolatedStorageFile.GetUserStoreForApplication()
                        Using stream As New IsolatedStorageFileStream("initParams.txt", System.IO.FileMode.Create, file)
                            Dim serializer As New DataContractSerializer(GetType(Dictionary(Of String, String)))
                            serializer.WriteObject(stream, Me._parameters)
                        End Using
                    End Using
                Catch ex As Exception

                End Try

            End If
        Catch ex As Exception

        End Try



        InitializeComponent()

        'BUTTON GENRATION

        Try
            Dim MyText1 As New Windows.Controls.TextBox
            imageroot.Children.Add(MyText1)
            MyText1.Name = "txtServiceRef"
            MyText1.Text = Me.GetParameters("ServiceRef")
            MyText1.Visibility = Windows.Visibility.Collapsed

            Dim MyText2 As New Windows.Controls.TextBox
            imageroot.Children.Add(MyText2)
            MyText2.Name = "txtDisplayID"
            MyText2.Text = Me.GetParameters("DisplayID")
            MyText2.Visibility = Windows.Visibility.Collapsed
            Dim MyMenuButt As New Windows.Controls.Button
            imageroot.Children.Add(MyMenuButt)

            'MENU BUTTON
            MyMenuButt.Name = "txtMyMenuButt"
            MyMenuButt.Content = "Menu"
            MyMenuButt.Visibility = Windows.Visibility.Collapsed
            AddHandler MyMenuButt.Click, AddressOf MyMenuButtClicked

            'SAVE BUTTON
            Dim MyButt As New Windows.Controls.Button
            imageroot.Children.Add(MyButt)
            MyButt.Name = "txtMyButt"
            MyButt.Content = "Save"
            MyButt.Visibility = Windows.Visibility.Collapsed
            AddHandler MyButt.Click, AddressOf MyButtClicked

            'CANCEL BUTTON
            Dim MyButt1 As New Windows.Controls.Button
            imageroot.Children.Add(MyButt1)
            MyButt1.Name = "txtMyButtCancel"
            MyButt1.Content = "Cancel"
            MyButt1.Visibility = Windows.Visibility.Collapsed
            AddHandler MyButt1.Click, AddressOf MyButtCancel
        Catch ex As Exception

        End Try
        'create grid page 1
        'set visible
        'TODO START PAGE TIMER
        'LoadSettings()
        'snow
        '_snowflakeTimer.Duration = TimeSpan.FromMilliseconds(10)
        'AddHandler _snowflakeTimer.Completed, AddressOf SnowFlakeTimer
        '_snowflakeTimer.Begin()
        ''
        Dim binding As New System.ServiceModel.BasicHttpBinding '= new BasicHttpBinding(); 
        binding.SendTimeout = TimeSpan.FromSeconds(150)
        binding.CloseTimeout = TimeSpan.FromSeconds(120)
        binding.OpenTimeout = TimeSpan.FromSeconds(120)
        binding.ReceiveTimeout = TimeSpan.FromSeconds(150)
        Me.ComsEvent.RepeatBehavior = RepeatBehavior.Forever
        Me.ComsEvent.Begin()
        If _parameters.ContainsKey("ServiceRef") Then
            Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(Me.GetParameters("ServiceRef")))
        Else
            Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
        End If
        'If _parameters.ContainsKey("DisplayID") Then
        'Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
        If _parameters.ContainsKey("DisplayID") Then
            Sitesproxy.GetDisplayGroupsAsync(CInt(_parameters("DisplayID")))
        Else
            Sitesproxy.GetSitesAsync()
        End If
        SetupTimers()
        Me.SpinLogo.RepeatBehavior = RepeatBehavior.Forever
        Me.SpinLogo.Begin()
    End Sub
    Private Sub SnowFlakeTimer(ByVal sender As Object, ByVal e As EventArgs)
        MoveSnowFlakes()
        CreateSnowFlakes()
    End Sub
    Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Application.Current.Host.Content.IsFullScreen = True
    End Sub
    Private Sub MoveSnowFlakes()
        Dim _flakesToRemove As New List(Of Snowflake)()
        For Each flake As Snowflake In _snowFlakes
            flake.Fall(_wind)
            If True = flake.Completed Then
                _flakesToRemove.Add(flake)
            End If
        Next
        For Each flake As Snowflake In _flakesToRemove
            _snowFlakes.Remove(flake)
            SnowCanvas.Children.Remove(flake)
        Next
        _snowflakeTimer.Begin()
    End Sub
    Private Sub CreateSnowFlakes()
        TotalCount.Text = "Total Snowflakes = " & _snowFlakes.Count
        Dim count As Integer = _rand.[Next](0, _newFlakeCount)
        For i As Integer = 0 To count - 1
            Dim flake As New Snowflake(_rand.[Next](0, 1020), 0.0R)
            _snowFlakes.Add(flake)
            SnowCanvas.Children.Add(flake)
        Next
        If _wind < 0 Then
            For i As Integer = 0 To count - 1
                Dim flake As New Snowflake(1010, _rand.[Next](0, 700))
                _snowFlakes.Add(flake)
                SnowCanvas.Children.Add(flake)
            Next
        ElseIf _wind > 0 Then
            For i As Integer = 0 To count - 1
                Dim flake As New Snowflake(0, _rand.[Next](0, 700))
                _snowFlakes.Add(flake)
                SnowCanvas.Children.Add(flake)
            Next
        End If
    End Sub
    Private Sub Slider_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
        If Volume IsNot Nothing Then
            _newFlakeCount = CInt(Volume.Value)
            VolumeValue.Text = _newFlakeCount.ToString()
        End If
    End Sub
    Private Sub Wind_ValueChanged(ByVal sender As Object, ByVal e As RoutedPropertyChangedEventArgs(Of Double))
        If TheWind IsNot Nothing Then
            _wind = CInt(TheWind.Value)
            WindValue.Text = _wind.ToString()
        End If
    End Sub
    Private Sub Sitesproxy_GetDisplayGroupsCompleted(ByVal sender As Object, ByVal e As GetDisplayGroupsCompletedEventArgs) Handles Sitesproxy.GetDisplayGroupsCompleted
        Dim PageNames As New Collection
        Try

            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()
                    'For mycnt1 As Integer = 0 To e.Result.Count - 1
                    '    'TODO Check PAGE
                    '    'add rows
                    '    Dim Myrow As New System.Windows.Controls.RowDefinition
                    '    Myrow.Height = System.Windows.GridLength.Auto
                    '    ' Dim mySensor As SilverlightIRemotelib.IRemoteLib.SensorDetails = CType(IPUnSerialise(, GetType(SilverlightIRemotelib.IRemoteLib.SensorDetails)), SilverlightIRemotelib.IRemoteLib.SensorDetails)
                    '    LayoutRoot.RowDefinitions.Add(Myrow)
                    'Next

                    For mycnt As Integer = 0 To e.Result.Count - 1
                        'items.Add(MySiteDetail.SiteName + "|" + MySiteDetail.ID.ToString + "|" + MySiteDetail.ExtraData + "|" + MySiteDetail.ExtraValue.ToString)

                        '<SLServerDisplay:ServerDisplay x:Name="ServerDisplay1" Visibility="Visible"  Grid.Row="0" Grid.Column="1"/>
                        Dim MySite As New ServerDisplay(_parameters)
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        MySite.Name = MyString(0)

                        MySite.PageNo = CInt(MyString(2))

                        MySite.ScreenSetting = CInt(MyString(4))
                        Try
                            MySite.DisplayType = CInt(MyString(5))
                            MySite.DisplayImage = CStr(MyString(6))
                            If MyString(7) <> "" Then
                                MySite.DisplayHeight = CInt(MyString(7))
                            Else
                                MySite.DisplayHeight = 0
                            End If
                            If MyString(8) <> "" Then
                                MySite.DisplayWidth = CInt(MyString(8))
                            Else
                                MySite.DisplayWidth = 0
                            End If
                            MySite.ExtraData = CStr(MyString(9))
                            If MyString(10) <> "" Then
                                MySite.Extravalue = CDbl(MyString(10))
                            Else
                                MySite.Extravalue = 0
                            End If
                            Try
                                MySite.ExtraData1 = CStr(MyString(11))
                                If MyString(12) <> "" Then
                                    MySite.Extravalue1 = CDbl(MyString(12))
                                Else
                                    MySite.Extravalue1 = 0
                                End If
                            Catch ex As Exception

                            End Try

                            'items.Add(MyGroup.GroupName + 
                            '"|" + MyGroup.ID.ToString + "|" 
                            '+ MyGroup.PanelNo.ToString + "|" 
                            '+ MyGroup.PanelPos.ToString + "|" 
                            '+ MyGroup.Screen.ToString + "|" 
                            '+ MyGroup.DisplayType.ToString + "|" 
                            '+ If(MyGroup.DisplayImage, "") + "|" 
                            '+ CStr(If(MyGroup.DisplayHeight, 0)) + "|" 
                            '+ CStr(If(MyGroup.DisplayWidth, 0)) 
                            '+ If(MyGroup.ExtraData1, "") + "|" 
                            '+ CStr(If(MyGroup.ExtraValue1, 0)) 
                            '+ If(MyGroup.ExtraData2, "") + "|" 
                            '+ CStr(If(MyGroup.ExtraValue2, 0)))

                        Catch ex As Exception

                        End Try
                        'siteid starts loading details
                        MySite.SiteID = CInt(MyString(1))
                        'TODO Check PAGE grid exists/created
                        Dim LayoutRoot As System.Windows.Controls.Grid = LoadGrid(MySite.ScreenSetting)

                        'items.Add(MySiteDetail.SiteName + "|" + 0
                        'MySiteDetail.ID.ToString + "|" + 1
                        'MySiteDetail.PanelNo.ToString + "|" + 2
                        'MySiteDetail.PanelPos.ToString + "|" + 3
                        'MySiteDetail.Screen.ToString + "|" + 4
                        'MySiteDetail.DisplayType.ToString + "|" + 5
                        'MySiteDetail.DisplayImage.ToString + "|" + 6
                        'MySiteDetail.DisplayHeight.ToString + "|" + 7
                        'MySiteDetail.DisplayWidth.ToString) 8


                        LayoutRoot.Children.Add(MySite)
                        'MyPages
                        If LayoutRoot.RowDefinitions.Count < (MySite.PageNo + 1) Then
                            While LayoutRoot.RowDefinitions.Count <= MySite.PageNo
                                Dim MyGridrow As New System.Windows.Controls.RowDefinition
                                MyGridrow.Height = System.Windows.GridLength.Auto
                                ' Dim mySensor As SilverlightIRemotelib.IRemoteLib.SensorDetails = CType(IPUnSerialise(, GetType(SilverlightIRemotelib.IRemoteLib.SensorDetails)), SilverlightIRemotelib.IRemoteLib.SensorDetails)
                                LayoutRoot.RowDefinitions.Add(MyGridrow)
                            End While
                        End If
                        MySite.SiteName.Text = MyString(0)
                        Try
                            If PageNames.Contains(MySite.ScreenSetting.ToString) = False Then
                                PageNames.Add(MySite.Name, MySite.ScreenSetting.ToString)
                            End If
                        Catch ex As Exception

                        End Try

                        MySite.SetValue(Grid.RowProperty, MySite.PageNo)
                        If CInt(MyString(3)) = 1 Then 'mission critical
                            MySite.SetValue(Grid.ColumnProperty, 0)
                        End If
                        If CInt(MyString(3)) = 2 Then 'Business critical
                            MySite.SetValue(Grid.ColumnProperty, 1)
                        End If
                        If CInt(MyString(3)) > 2 Then 'other
                            MySite.SetValue(Grid.ColumnProperty, 0)
                            MySite.SetValue(Grid.ColumnSpanProperty, 2)
                            MySite.BackPlaneBorder.Width = 1200
                            MySite.ServersInARow = 18
                        End If
                        MyPages.Add(MySite)

                    Next

                Else
                    'ServerDisplay.SiteName.Text += "Nothing"
                End If
            End If
        Catch ex As Exception

        End Try
        Try
            If SLSettings.ShowPageButton Then
                Dim MyPagecnt As Integer
                If MaxPage > 1 Then
                    For MyPagecnt = 1 To MaxPage
                        Dim MyButton As New Windows.Controls.Button
                        imageroot.Children.Add(MyButton)
                        Try
                            If PageNames.Contains(MyPagecnt.ToString) = True Then
                                MyButton.Content = CStr(PageNames.Item(MyPagecnt.ToString))
                            Else
                                MyButton.Content = "Page:" + MyPagecnt.ToString
                            End If
                        Catch ex As Exception
                            MyButton.Content = "Page:" + MyPagecnt.ToString
                        End Try
                        'MyButton.Content = "Page:" + MyPagecnt.ToString
                        MyButton.Tag = MyPagecnt
                        MyButton.Name = "Page:" + MyPagecnt.ToString
                        AddHandler MyButton.Click, AddressOf PageClicked
                    Next
                End If
            End If


        Catch ex As Exception

        End Try
        Try
            Dim MyButton1 As New Windows.Controls.Button
            imageroot.Children.Add(MyButton1)
            MyButton1.Content = "Pause"
            MyButton1.Tag = "Pause"
            MyButton1.Name = "Pause"
            AddHandler MyButton1.Click, AddressOf PauseTimer
        Catch ex As Exception

        End Try
        Try
            Dim MyButton2 As New Windows.Controls.Button
            imageroot.Children.Add(MyButton2)
            MyButton2.Content = "Resume"
            MyButton2.Tag = "Resume"
            MyButton2.Name = "Resume"
            AddHandler MyButton2.Click, AddressOf ResumeTimer
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ResumeTimer(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        MyPageTimer.Start()
        Me.SpinLogo.Begin()
    End Sub
    Private Sub PauseTimer(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        MyPageTimer.Stop()
        Me.SpinLogo.Stop()
    End Sub
    Private Sub PageClicked(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim MyButton As Windows.Controls.Button = CType(e.OriginalSource, Windows.Controls.Button)

        Dim NextPageName As String
        Dim CurPageName As String

        CurPageName = "LayoutRoot" + CurVisiblePage.ToString
        CurVisiblePage = CInt(MyButton.Tag)
        NextPageName = "LayoutRoot" + MyButton.Tag.ToString

        Dim MyDisplay As ServerDisplay = FindDisplay(CurVisiblePage)
        If IsNothing(MyDisplay) = False Then
            'resize
            Try
                If MyDisplay.DisplayHeight > 0 Then
                    Me.scaleTransform.ScaleY = 900 / MyDisplay.DisplayHeight
                Else
                    Me.scaleTransform.ScaleY = InitHeight / 625
                End If
                If MyDisplay.DisplayWidth > 0 Then
                    Me.scaleTransform.ScaleX = 1240 / MyDisplay.DisplayWidth
                Else
                    Me.scaleTransform.ScaleX = InitWidth / 1242
                End If
            Catch ex As Exception

            End Try
            Try
                'set the page timer
                If MyDisplay.Extravalue1 > 0 Then
                    ResetPageTimer(MyDisplay.Extravalue1)
                Else 'use default
                    ResetPageTimer(SLSettings.PageTimer)
                End If
            Catch ex As Exception

            End Try
            If MyDisplay.DisplayType = 3 Then
                'LastWebDisplayed = CurVisiblePage
                ShowWebPage(MyDisplay.ExtraData, CurVisiblePage)
            Else
                HideWebPage(LastWebDisplayed)
            End If
            If MyDisplay.DisplayType = 4 Then
                'LastWebDisplayed = CurVisiblePage
                ShowMediaPage(MyDisplay.ExtraData, CurVisiblePage)
            Else
                HideMediaPage(LastMediaDisplayed)
            End If
        Else
            HideWebPage(LastWebDisplayed)
            HideMediaPage(LastMediaDisplayed)
        End If
        If CurPageName <> NextPageName Then
            If IsNothing(PageRoot.FindName(CurPageName)) = False Then
                'CType(PageRoot.FindName(CurPageName), Windows.Controls.Grid).Visibility = Windows.Visibility.Collapsed
                'GridHide.SetValue(Storyboard.TargetNameProperty, CurPageName)
                Try
                    AnimateCollapsed(CType(PageRoot.FindName(CurPageName), Windows.Controls.Grid))
                    'GridHide.Stop()
                    'Storyboard.SetTargetName(GridHide, CurPageName)
                    'GridHide.Begin()
                Catch ex As Exception

                End Try
            End If
        End If

        If IsNothing(PageRoot.FindName(NextPageName)) = False Then
            'CType(PageRoot.FindName(NextPageName), Windows.Controls.Grid).Visibility = Windows.Visibility.Visible
            ' GridShow.SetValue(Storyboard.TargetNameProperty, NextPageName)
            Try
                AnimateVisible(CType(PageRoot.FindName(NextPageName), Windows.Controls.Grid))
                'GridShow.Stop()
                'Storyboard.SetTargetName(GridShow, NextPageName)
                'GridShow.Begin()
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub Sitesproxy_GetSitesCompleted(ByVal sender As Object, ByVal e As LiveMonitoring.GetSitesCompletedEventArgs) Handles Sitesproxy.GetSitesCompleted
        Try
            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then
                    Me.ComsEvent.Stop()
                    'For mycnt1 As Integer = 0 To e.Result.Count - 1
                    '    'TODO Check PAGE
                    '    'add rows
                    '    Dim Myrow As New System.Windows.Controls.RowDefinition
                    '    Myrow.Height = System.Windows.GridLength.Auto
                    '    ' Dim mySensor As SilverlightIRemotelib.IRemoteLib.SensorDetails = CType(IPUnSerialise(, GetType(SilverlightIRemotelib.IRemoteLib.SensorDetails)), SilverlightIRemotelib.IRemoteLib.SensorDetails)
                    '    LayoutRoot.RowDefinitions.Add(Myrow)
                    'Next

                    For mycnt As Integer = 0 To e.Result.Count - 1
                        'items.Add(MySiteDetail.SiteName + "|" + MySiteDetail.ID.ToString + "|" + MySiteDetail.ExtraData + "|" + MySiteDetail.ExtraValue.ToString)

                        '<SLServerDisplay:ServerDisplay x:Name="ServerDisplay1" Visibility="Visible"  Grid.Row="0" Grid.Column="1"/>
                        Dim MySite As New ServerDisplay(_parameters)
                        Dim MyString() = e.Result.Item(mycnt).Split("|")
                        MySite.Name = MyString(0)

                        MySite.PageNo = CInt(MyString(2))
                        MySite.ScreenSetting = CInt(MyString(4))
                        Try
                            MySite.DisplayType = CInt(MyString(5))
                            MySite.DisplayImage = CStr(MyString(6))
                            MySite.DisplayHeight = CInt(MyString(7))
                            MySite.DisplayWidth = CInt(MyString(8))
                            MySite.ExtraData = CStr(MyString(9))
                            MySite.Extravalue = CDbl(MyString(10))
                        Catch ex As Exception

                        End Try
                        'siteid starts loading details
                        MySite.SiteID = CInt(MyString(1))
                        'TODO Check PAGE grid exists/created
                        Dim LayoutRoot As System.Windows.Controls.Grid = LoadGrid(MySite.ScreenSetting)

                        'items.Add(MySiteDetail.SiteName + "|" + 0
                        'MySiteDetail.ID.ToString + "|" + 1
                        'MySiteDetail.PanelNo.ToString + "|" + 2
                        'MySiteDetail.PanelPos.ToString + "|" + 3
                        'MySiteDetail.Screen.ToString + "|" + 4
                        'MySiteDetail.DisplayType.ToString + "|" + 5
                        'MySiteDetail.DisplayImage.ToString + "|" + 6
                        'MySiteDetail.DisplayHeight.ToString + "|" + 7
                        'MySiteDetail.DisplayWidth.ToString) 8


                        LayoutRoot.Children.Add(MySite)
                        'MyPages
                        If LayoutRoot.RowDefinitions.Count < (MySite.PageNo + 1) Then
                            While LayoutRoot.RowDefinitions.Count <= MySite.PageNo
                                Dim MyGridrow As New System.Windows.Controls.RowDefinition
                                MyGridrow.Height = System.Windows.GridLength.Auto
                                ' Dim mySensor As SilverlightIRemotelib.IRemoteLib.SensorDetails = CType(IPUnSerialise(, GetType(SilverlightIRemotelib.IRemoteLib.SensorDetails)), SilverlightIRemotelib.IRemoteLib.SensorDetails)
                                LayoutRoot.RowDefinitions.Add(MyGridrow)
                            End While
                        End If
                        MySite.SiteName.Text = MyString(0)
                        MySite.SetValue(Grid.RowProperty, MySite.PageNo)
                        If CInt(MyString(3)) = 1 Then 'mission critical
                            MySite.SetValue(Grid.ColumnProperty, 0)
                        End If
                        If CInt(MyString(3)) = 2 Then 'Business critical
                            MySite.SetValue(Grid.ColumnProperty, 1)
                        End If
                        If CInt(MyString(3)) > 2 Then 'other
                            MySite.SetValue(Grid.ColumnProperty, 0)
                            MySite.SetValue(Grid.ColumnSpanProperty, 2)
                            MySite.BackPlaneBorder.Width = 1200
                            MySite.ServersInARow = 18
                        End If
                        MyPages.Add(MySite)

                    Next

                Else
                    'ServerDisplay.SiteName.Text += "Nothing"
                End If
            End If
        Catch ex As Exception

        End Try


    End Sub



    Private Sub Page_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        '** Note That BrowserHost returns (sender as Object, e As System.EventArgs)
        'Application.Current.Host.Content.IsFullScreen = True
        ' AddHandler System.Windows.Interop.SilverlightHost, AddressOf Me.BrowserHost_FullScreenChange
    End Sub
    '** This handler is attached on Page_Load


    Private Sub Page_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles Me.MouseMove
        Dim MyPoint As System.Windows.Point = e.GetPosition(Nothing)
        If MyPoint.X < OldX Then
            If _wind > -50 Then _wind -= 1
        End If
        If MyPoint.X > OldX Then
            If _wind < 50 Then _wind += 1
        End If
        If MyPoint.Y < OldY Then
            If _newFlakeCount > 2 Then _newFlakeCount -= 1
        End If
        If MyPoint.Y > OldY Then
            If _newFlakeCount < 10 Then _newFlakeCount += 1
        End If
        OldY = MyPoint.Y
        OldX = MyPoint.X
    End Sub

    Private Sub Logo_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Logo.MouseLeftButtonDown
        Dim Mybutt As Windows.Controls.Button = imageroot.FindName("txtMyButt")
        Dim Mybutt1 As Windows.Controls.Button = imageroot.FindName("txtMyButtCancel")
        'txtMyMenuButt

        Dim MyText1 As Windows.Controls.TextBox = imageroot.FindName("txtServiceRef")
        Dim MyText2 As Windows.Controls.TextBox = imageroot.FindName("txtDisplayID")
        Mybutt.Visibility = Windows.Visibility.Visible
        Mybutt1.Visibility = Windows.Visibility.Visible
        MyText1.Visibility = Windows.Visibility.Visible
        MyText2.Visibility = Windows.Visibility.Visible
        Dim MyMenuButt As Windows.Controls.Button = imageroot.FindName("txtMyMenuButt")
        MyMenuButt.Visibility = Windows.Visibility.Visible
    End Sub

    Private Sub MymenuPage_Closed(sender As Object, e As EventArgs)
        'refresh & reload
        MyPageTimer.Start()
        Me.SpinLogo.Begin()

    End Sub

End Class