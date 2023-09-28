Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
Imports System.Windows.Controls
Imports SLServerDisplay.LiveMonitoring
Imports System.Security.Cryptography
Imports System.Text

Partial Public Class ChildLogon
    Inherits ChildWindow
    Private Shared SLSettings As New SLSettings
    Public _parameters As IDictionary(Of String, String)
    Private WithEvents Sitesproxy As SLIPmonInterfaceSVCClient
    Public Event LoginSucceeded(ByVal UserDetail As UserDetails)
    Public pstrUserName As String
    Public pstrPassword As String
    Public Sub New(ByVal parameters As IDictionary(Of String, String))
        Me._parameters = parameters
        Try
            If Me._parameters.ContainsKey("ServiceRef") = False Then
                Me._parameters.Add("ServiceRef", SLSettings.ServiceURL)
                Me._parameters.Add("DisplayID", SLSettings.DisplayID)
                Me._parameters.Add("UserName", pstrUserName)
                Me._parameters.Add("Password", pstrPassword)
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
        'Try
        '    Try
        '        pstrPassword = Me.GetParameters("Password")
        '        pstrUserName = Me.GetParameters("UserName")
        '    Catch ex As Exception

        '    End Try
        '    Sitesproxy.CheckLoginAsync(pstrUserName, pstrPassword)
        'Catch ex As Exception

        'End Try
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

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub btnLogIn_Click(sender As Object, e As RoutedEventArgs) Handles btnLogIn.Click
        Sitesproxy.CheckLoginAsync(txtUserName.Text, GetEncrypted(txtPassword.Password.ToString))
    End Sub
    Public Function GetEncrypted(password As String) As String
        Dim hash As HashAlgorithm = New SHA256Managed()
        Dim salt As String = "livemon.co.za"

        ' compute hash of the password prefixing password with the salt
        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(salt & password)
        Dim hashBytes As Byte() = hash.ComputeHash(plainTextBytes)

        Return Convert.ToBase64String(hashBytes)
    End Function

    Private Sub Sitesproxy_CheckLoginCompleted(sender As Object, e As CheckLoginCompletedEventArgs) Handles Sitesproxy.CheckLoginCompleted
        Try
            If e.Result <> "" Then
                'logged in 
                ' MyUser.FirstName + "|" + MyUser.SurName + "|" + MyUser.ID.ToString + "|" + MyUser.UserLevel.ToString
                Dim MyUser() As String = e.Result.Split("|")
                Dim MyUserDet As New UserDetails
                MyUserDet.FirstName = MyUser(0)
                MyUserDet.SurName = MyUser(1)
                MyUserDet.ID = MyUser(2)
                MyUserDet.UserLevel = MyUser(3)
                Me.DialogResult = True
                RaiseEvent LoginSucceeded(MyUserDet)
                Me.Close()
            Else
                lblMessage.Visibility = Windows.Visibility.Visible
                lblMessage.Content = "Invalid login .Please try again !"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPassword_TextInputStart(sender As Object, e As TextCompositionEventArgs) Handles txtPassword.TextInputStart
        lblMessage.Visibility = Windows.Visibility.Collapsed
    End Sub

    Private Sub txtUserName_TextInputStart(sender As Object, e As TextCompositionEventArgs) Handles txtUserName.TextInputStart
        lblMessage.Visibility = Windows.Visibility.Collapsed
    End Sub
End Class
