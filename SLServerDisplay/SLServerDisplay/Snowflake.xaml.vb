Imports System.Windows.Media.Imaging
Partial Public Class Snowflake
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub
    Private _posLeft As Double = 0.0R
    Private _posTop As Double = 0.0R
    Private _floorValue As Double = 764
    Private seed As Integer = DateTime.Now.Millisecond
    Private _rand As New Random(DateTime.Now.Millisecond)
    Private _completed As Boolean = False
    Private _horzInc As Double = 0.0R

    Public ReadOnly Property Completed() As Boolean
        Get
            Return _completed
        End Get
    End Property

    Public Sub New(ByVal left As Double, ByVal top As Double)
        InitializeComponent()
        _posLeft = left
        _posTop = top
        Me.SetValue(Canvas.LeftProperty, _posLeft)
        Me.SetValue(Canvas.TopProperty, _posTop)
        Dim rndSize As New Random(DateTime.Now.Millisecond)
        'SnowScale.ScaleX = InlineAssignHelper(SnowScale.ScaleY, 0.05 * rndSize.[Next](1, 6))

        SnowScale.ScaleX = 0.01 * rndSize.Next(1, 6)
        SnowScale.ScaleY = 0.01 * rndSize.Next(1, 6)



    End Sub
    Public Sub explode()
        Dim rndSize As New Random(DateTime.Now.Millisecond)
        Dim myval As Integer = CInt(rndSize.Next(1, 320))
        If myval >= 16 And myval < 22 Then
            Dim rndSize1 As New Random(DateTime.Now.Millisecond)
            If rndSize1.Next(1, 20) > 10 Then
                Me.Flake.Source = New BitmapImage(New Uri("smallexplos.png", UriKind.Relative))
            Else
                Me.Flake.Source = New BitmapImage(New Uri("smallexplos1.png", UriKind.Relative))
            End If
            explosion.Begin()
        End If

        
    End Sub
    Public Sub Fall(ByVal windValue As Double)
        Dim value As Integer = _rand.[Next](50)
        If value = 25 Then
            ' 1 in 50 change to change direction 
            If _horzInc >= 0.2 Then
                _horzInc = 0
            ElseIf _horzInc <= -0.2 Then
                _horzInc = 0
            Else
                value = _rand.[Next](3)
                If value = 1 Then
                    _horzInc = 0.2
                ElseIf value = 2 Then
                    _horzInc = -0.2
                ElseIf value > 0 Then
                    _horzInc = 0.0R
                End If
            End If
        End If
        Dim horzValue As Double = _horzInc + (windValue * 0.1)
        _posLeft += horzValue
        Me.SetValue(Canvas.LeftProperty, _posLeft)
        _posTop += 1
        Me.SetValue(Canvas.TopProperty, _posTop)
        'Me.SetValue(Canvas.LeftProperty, _posLeft)
        'Me.SetValue(Canvas.TopProperty, System.Math.Max(System.Threading.Interlocked.Increment((_posTop)), _posTop - 1))
        _completed = (_posLeft >= 1019 OrElse _posLeft <= 0 OrElse _posTop >= _floorValue)
        Dim rndSize As New Random(DateTime.Now.Millisecond)
        Dim myval As Integer = CInt(rndSize.Next(1, 320))
        If myval >= 16 And myval < 22 Then
            ' explode()
        End If

    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

    
End Class
