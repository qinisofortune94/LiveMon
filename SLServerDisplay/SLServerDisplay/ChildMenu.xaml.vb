Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
Imports System.Windows.Controls

Partial Public Class ChildMenu
    Inherits ChildWindow
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
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click

        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub btnAddNewDisplay_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnAddNewDisplay.Click
        Dim MyAddNewDisplay As New ChildAddNewDisplay(_parameters)
        MyAddNewDisplay.Show()
    End Sub

    Private Sub btnEditExsistingPage_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEditExsistingPage.Click
        Dim MyAddNewDisplay As New ChildEditContentPage(_parameters)
        MyAddNewDisplay.Show()
    End Sub

    Private Sub AddNewPage_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles AddNewPage.Click
        Dim MyAddNewDisplay As New ChildAddNewContent(_parameters)
        MyAddNewDisplay.Show()
    End Sub

    Private Sub btnChangeSequence_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnChangeSequence.Click
        Dim MyAddNewDisplay As New ChildChangeSequence(_parameters)
        MyAddNewDisplay.Show()
    End Sub

    Private Sub btnUploadMedia_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnUploadMedia.Click
        Dim MyAddNewDisplay As New ChildUploadMedia(_parameters)
        MyAddNewDisplay.Show()
    End Sub

    Private Sub btnAddNewDisplay_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnAddNewDisplay_Copy.Click
        Dim MyAddNewDisplay As New ChildListDisplays(_parameters)
        MyAddNewDisplay.Show()
    End Sub
End Class
