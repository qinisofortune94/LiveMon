'** Needed to see BrowserHost
Imports System.Windows.Interop
Partial Public Class App
    Inherits Application
    Private WithEvents rootPage As Page '= New Page()
    Private WithEvents htmlContent As Content
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Application_Startup(ByVal o As Object, ByVal e As StartupEventArgs) Handles Me.Startup
        rootPage = New Page(e.InitParams)
        Me.RootVisual = rootPage 'New Page()
        htmlContent = Me.Host.Content
        'Me.Host.Content.IsFullScreen = True

    End Sub
    Private Sub ToggleFullScreen(ByVal sender As Object, ByVal e As MouseButtonEventArgs) Handles rootPage.MouseLeftButtonDown
        '  Me.Host.Content.IsFullScreen = Not Me.Host.Content.IsFullScreen
    End Sub

    Private Sub DisplaySizeInformation( _
        ByVal sender As Object, ByVal e As EventArgs) _
        Handles htmlContent.FullScreenChanged, htmlContent.Resized

        'Dim message As String = String.Format( _
        '    "ActualWidth={0}, ActualHeight={1}", _
        '    Me.Host.Content.ActualWidth, _
        '    Me.Host.Content.ActualHeight)

        'rootPage.LayoutRoot.Children.Clear()
        'Dim t As New TextBlock()
        ' t.Text = message
        'rootPage.LayoutRoot.Children.Add(t)
        rootPage.scaleTransform.ScaleX = Me.Host.Content.ActualWidth / 1242
        rootPage.scaleTransform.ScaleY = Me.Host.Content.ActualHeight / 625

    End Sub

    Private Sub Application_Exit(ByVal o As Object, ByVal e As EventArgs) Handles Me.Exit

    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException

        ' If the app is running outside of the debugger then report the exception using
        ' the browser's exception mechanism. On IE this will display it a yellow alert 
        ' icon in the status bar and Firefox will display a script error.
        If Not System.Diagnostics.Debugger.IsAttached Then

            ' NOTE: This will allow the application to continue running after an exception has been thrown
            ' but not handled. 
            ' For production applications this error handling should be replaced with something that will 
            ' report the error to the website and stop the application.
            e.Handled = True
            Deployment.Current.Dispatcher.BeginInvoke(New Action(Of ApplicationUnhandledExceptionEventArgs)(AddressOf ReportErrorToDOM), e)
        End If
    End Sub

    Private Sub ReportErrorToDOM(ByVal e As ApplicationUnhandledExceptionEventArgs)

        Try
            Dim errorMsg As String = e.ExceptionObject.Message + e.ExceptionObject.StackTrace
            errorMsg = errorMsg.Replace(""""c, "'"c).Replace(ChrW(13) & ChrW(10), "\n")

            System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(""Unhandled Error in Silverlight 2 Application " + errorMsg + """);")
        Catch

        End Try
    End Sub

End Class
