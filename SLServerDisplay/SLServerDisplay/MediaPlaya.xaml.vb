Imports System.Windows.Controls.MediaElement

Partial Public Class MediaPlaya
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnPlay_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnPlay.Click
        VideoPlaya.Play()
    End Sub

    Private Sub BtnPause_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles BtnPause.Click
        VideoPlaya.Pause()
    End Sub

    Private Sub Stop_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles [Stop].Click
        VideoPlaya.Stop()
    End Sub

    Private Sub VideoPlaya_MediaOpened(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles VideoPlaya.MediaOpened

    End Sub

    Private Sub VideoPlaya_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles VideoPlaya.MouseLeftButtonDown
        'MessageBox.Show("You clicked me dude 1234" & Now.ToString)
        btnPlay.Visibility = Windows.Visibility.Visible
    End Sub
End Class
