Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

Partial Public Class Server
    Inherits UserControl
    Implements IComparable(Of Server)
    Private mySensorID As Integer
    Private myStatus As Integer
    Private myType As Integer
    Private Shared SLSettings As New SLSettings
    Public _sensor As New SensorDetails
    Public Event ClickedMe(ByVal SensorID As Integer)
    Public Event ClickedMeAlert(ByVal SensorID As Integer)
    Function CompareTo(ByVal other As SLServerDisplay.Server) As Integer Implements IComparable(Of SLServerDisplay.Server).CompareTo
        Return Me.ServerText.Text.CompareTo(other.ServerText.Text)
    End Function
    Public Property SensorType() As Integer
        Get
            Return myType
        End Get
        Set(ByVal value As Integer)
            myType = value
        End Set
    End Property
    
    Public Property Status() As Integer
        Get
            Return myStatus
        End Get
        Set(ByVal value As Integer)
            If value >= 2 And myStatus < 2 And value < 7 Then
                If value <> myStatus Then
                    'play sound if status changes to alert
                    'Dim m As New MediaElement ' = New MediaElement())
                    'LayoutRoot.Children.Add(m)
                    media.Source = New Uri(Application.Current.Host.Source.ToString + "/../../Sounds/sound33.mp3")
                    media.Visibility = Visibility.Visible
                    media.Position = New TimeSpan(0)
                    media.Play()
                    If SLSettings.RepeatSound = True Then
                        AddHandler media.MediaEnded, AddressOf RepeatMedia
                    End If

                    'Dim myDuration = New Duration(TimeSpan.FromSeconds(35))
                    'StatusError.Duration = myDuration
                    StatusError.RepeatBehavior = RepeatBehavior.Forever
                    StatusError.Begin()
                    'StatusError.Duration
                
                End If
            End If
            If value >= 2 And value < 7 Then
                If value = myStatus Then
                    StatusError.Stop()
                End If
            End If

            myStatus = value
            If myStatus = 7 Then
                Dim MyColorCol As New GradientStopCollection
                Dim MyStop As New GradientStop '( { Color = Color.FromArgb(0xCC, 0xF5, 0x5F, 0x0F), Offset = 0 })
                MyStop.Color = Colors.LightGray
                MyColorCol.Add(MyStop)
                Dim MyStop1 As New GradientStop '( { Color = Color.FromArgb(0xCC, 0xF5, 0x5F, 0x0F), Offset = 0 })
                MyStop1.Color = Colors.DarkGray
                MyStop1.Offset = 1
                MyColorCol.Add(MyStop1)
                Dim MyColorBrush As New LinearGradientBrush '(MyColorCol)
                MyColorBrush.GradientStops = MyColorCol
                ServerBlock.Background = MyColorBrush
            Else
                If myStatus < 2 Then
                    Dim MyColorCol As New GradientStopCollection
                    '<GradientStop Color="#FF8CFC59"/>
                    '<GradientStop Color="#FF1FA202" Offset="1"/>
                    Dim MyStop As New GradientStop '( { Color = Color.FromArgb(0xCC, 0xF5, 0x5F, 0x0F), Offset = 0 })
                    MyStop.Color = Color.FromArgb(CByte(&HFF), CByte(&H8C), CByte(&HFC), CByte(&H59))
                    MyColorCol.Add(MyStop)
                    Dim MyStop1 As New GradientStop '( { Color = Color.FromArgb(0xCC, 0xF5, 0x5F, 0x0F), Offset = 0 })
                    MyStop1.Color = Color.FromArgb(CByte(&HFF), CByte(&H1F), CByte(&HA2), CByte(&H2))
                    MyStop1.Offset = 1
                    MyColorCol.Add(MyStop1)
                    Dim MyColorBrush As New LinearGradientBrush '(MyColorCol)
                    MyColorBrush.GradientStops = MyColorCol
                    ServerBlock.Background = MyColorBrush
                End If
            End If
            If myStatus = 8 Then
                Dim MyColorCol As New GradientStopCollection
                Dim MyStop As New GradientStop '( { Color = Color.FromArgb(0xCC, 0xF5, 0x5F, 0x0F), Offset = 0 })
                MyStop.Color = Colors.Yellow
                MyColorCol.Add(MyStop)
                Dim MyStop1 As New GradientStop '( { Color = Color.FromArgb(0xCC, 0xF5, 0x5F, 0x0F), Offset = 0 })
                MyStop1.Color = Colors.Orange
                MyStop1.Offset = 1
                MyColorCol.Add(MyStop1)
                Dim MyColorBrush As New LinearGradientBrush '(MyColorCol)
                MyColorBrush.GradientStops = MyColorCol
                ServerBlock.Background = MyColorBrush
            End If
            If myStatus >= 2 And myStatus < 7 Then
                NotAvailable.Begin()
            End If
            If myStatus = 0 Then
                NotAvailable.Stop()
            End If

            FlashMe(Me)

        End Set
    End Property
    Public Sub RepeatMedia(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        media.Stop()
        media.Position = TimeSpan.FromSeconds(0)
        media.Play()
    End Sub
    Public Sub FlashMe(ByVal ctlToAnimate As UIElement)
        ctlToAnimate.Opacity = 0
        Dim sb As New Storyboard()
        Dim daY As New DoubleAnimationUsingKeyFrames()
        daY.BeginTime = TimeSpan.FromSeconds(0)
        Dim sDKF1 As New SplineDoubleKeyFrame()
        sDKF1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))
        sDKF1.Value = 0
        Dim sDKF2 As New SplineDoubleKeyFrame()

        sDKF2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500))
        sDKF2.Value = 1

        Dim sDKF3 As New SplineDoubleKeyFrame()
        sDKF3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
        sDKF3.Value = 0

        daY.KeyFrames.Add(sDKF1)

        daY.KeyFrames.Add(sDKF2)

        daY.KeyFrames.Add(sDKF3)

        Storyboard.SetTargetProperty(daY, New PropertyPath("(UIElement.Opacity)"))
        Storyboard.SetTarget(daY, ctlToAnimate)
        sb.Children.Add(daY)
        'Dim objectAnimationUsingKeyFrames As New ObjectAnimationUsingKeyFrames()
        'objectAnimationUsingKeyFrames.BeginTime = TimeSpan.FromMilliseconds(500)


        'Dim dOKF1 As New DiscreteObjectKeyFrame()

        'dOKF1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))
        'dOKF1.Value = Visibility.Visible

        'Dim dOKF2 As New DiscreteObjectKeyFrame()
        'dOKF2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))

        'dOKF2.Value = Visibility.Collapsed


        'objectAnimationUsingKeyFrames.KeyFrames.Add(dOKF1)

        'objectAnimationUsingKeyFrames.KeyFrames.Add(dOKF2)

        'Storyboard.SetTargetProperty(objectAnimationUsingKeyFrames, New PropertyPath("(UIElement.Visibility)"))
        'Storyboard.SetTarget(objectAnimationUsingKeyFrames, ctlToAnimate)
        'sb.Children.Add(objectAnimationUsingKeyFrames)
        sb.Begin()
    End Sub
    Public Property SensorID() As Integer
        Get
            Return mySensorID
        End Get
        Set(ByVal value As Integer)
            mySensorID = value
        End Set
    End Property
    Public Sub New()
        ' Required to initialize variables
        InitializeComponent()
    End Sub

    Private Sub Server_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        media.Stop()
        'write log
        Try
            If myStatus < 2 Then
                RaiseEvent ClickedMeAlert(mySensorID)
            End If
        Catch ex As Exception

        End Try
        'ok now a normal click event
        Try
            If SLSettings.ShowDetail Then
                RaiseEvent ClickedMe(mySensorID)
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class
