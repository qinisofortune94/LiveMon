Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
Imports SLServerDisplay.LiveMonitoring

Partial Public Class ChildAddNewDisplay
    Inherits ChildWindow
    Private WithEvents Sitesproxy As SLIPmonInterfaceSVCClient
    Private Shared SLSettings As New SLSettings
    Public _parameters As IDictionary(Of String, String)
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

        Dim binding As New System.ServiceModel.BasicHttpBinding '= new BasicHttpBinding(); 
        binding.SendTimeout = TimeSpan.FromSeconds(150)
        binding.CloseTimeout = TimeSpan.FromSeconds(120)
        binding.OpenTimeout = TimeSpan.FromSeconds(120)
        binding.ReceiveTimeout = TimeSpan.FromSeconds(150)
        If _parameters.ContainsKey("ServiceRef") Then
            Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(Me.GetParameters("ServiceRef")))
        Else
            Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
        End If
        'If _parameters.ContainsKey("DisplayID") Then
        'Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
        'If _parameters.ContainsKey("DisplayID") Then
        '    Sitesproxy.GetDisplayGroupsAsync(CInt(_parameters("DisplayID")))
        'Else
        '    Sitesproxy.GetSitesAsync()
        'End If
    End Sub
    Private Function GetParameters(ByVal ParameterID As String) As String
        Return _parameters(ParameterID)
    End Function

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        'save it
        Sitesproxy.AddDisplayAsync(Me.txtDisplayName.Text, CInt(Me.cmbType.SelectedItem.tag))

        'close
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

End Class
