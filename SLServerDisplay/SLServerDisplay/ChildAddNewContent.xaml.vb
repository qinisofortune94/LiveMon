Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
Imports SLServerDisplay.LiveMonitoring


Partial Public Class ChildAddNewContent
    Inherits ChildWindow
    Private WithEvents Sitesproxy As SLIPmonInterfaceSVCClient
    Private Shared SLSettings As New SLSettings
    Public _parameters As IDictionary(Of String, String)
    Private fileStream As System.IO.Stream
    Private numBytes As Long
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
        Try
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
            Sitesproxy.GetDisplayGroupsAsync(CInt(_parameters("DisplayID")))
            'If _parameters.ContainsKey("DisplayID") Then
            'Sitesproxy = New SLIPmonInterfaceSVCClient(binding, New System.ServiceModel.EndpointAddress(SLSettings.ServiceURL))
            'If _parameters.ContainsKey("DisplayID") Then
            '    Sitesproxy.GetDisplayGroupsAsync(CInt(_parameters("DisplayID")))
            'Else
            '    Sitesproxy.GetSitesAsync()
            'End If
        Catch ex As Exception

        End Try
       
    End Sub
    Private Function GetParameters(ByVal ParameterID As String) As String
        Try
            Return _parameters(ParameterID)

        Catch ex As Exception

        End Try
    End Function

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        'save it
        If Me.txtDisplayName.Text = "" Then
            MessageBox.Show("Error during save ,Please provide a Name !", "Add Page", MessageBoxButton.OK)
            Exit Sub
        End If
        Try
            Sitesproxy.AddDisplayGroupPageAsync(CInt(_parameters("DisplayID")), Me.txtDisplayName.Text, CInt(Me.cmbType.Items(Me.cmbType.SelectedIndex).tag), "",
                                txtWidth.Value, txtHeight.Value, txtScreen.Value, CInt(Me.cmbPanelPos.Items(Me.cmbPanelPos.SelectedIndex).tag),
                                CInt(Me.cmbPanel.Items(Me.cmbPanel.SelectedIndex).tag), txtMediaFile.Text, "", 0, txtTimer.Value)
        Catch ex As Exception
            MessageBox.Show("Error during save !", "Add Page", MessageBoxButton.OK)
        End Try

    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub btnFiles_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnFiles.Click
        Try
            lstDisplayFiles.Visibility = Windows.Visibility.Visible
            Sitesproxy.ReturnDisplayFilesAsync()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Sitesproxy_AddDisplayGroupPageCompleted(sender As Object, e As AddDisplayGroupPageCompletedEventArgs) Handles Sitesproxy.AddDisplayGroupPageCompleted
        Try
            If e.Result = True Then
                MessageBox.Show("Page Saved Correctly !", "Add Page", MessageBoxButton.OK)
                ' Close(dialog)
                Me.DialogResult = True
            Else
                MessageBox.Show("Page not Saved ,Please try again !", "Add Page", MessageBoxButton.OK)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Sitesproxy_GetDisplayGroupsCompleted(sender As Object, e As GetDisplayGroupsCompletedEventArgs) Handles Sitesproxy.GetDisplayGroupsCompleted
        Dim PageNames As New Collection
        Try

            If IsNothing(e.Result) = False Then
                'LstPages.Items.Clear()
                If e.Result.Count() > 0 Then
                    'Dim list As New ObservableCollection(Of String)

                    'list.Add(1);
                    'list.Add(2);
                    'list.Add(3);

                    'listBox1.ItemsSource = list;
                    Dim HigestPage As Integer = 0
                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Try
                            Dim MyString() = e.Result.Item(mycnt).Split("|")
                            If CInt(MyString(4)) > HigestPage Then
                                HigestPage = CInt(MyString(4))
                            End If
                            'Dim MyListitem As New ListBoxItem
                            ''MyListitem.
                            'MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                            'MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                            ''BorderThickness="2" BorderBrush="Blue"

                            'MyListitem.Content += "Name:" + MyString(0)
                            'MyListitem.Content += "  Page:" + MyString(4)
                            'MyListitem.Content += "  ID:" + MyString(1)
                            'MyListitem.Tag = e.Result(mycnt) 'CInt(MyString(1))
                            ''MyListitem.Content += " ID:" + MyString(0)
                            ''MyListitem.Content += " AlertType:" + MyString(1)
                            '' list.Add(MyListitem.Content)
                            'LstPages.Items.Add(MyListitem)
                        Catch ex As Exception

                        End Try


                    Next
                    txtScreen.Value = HigestPage + 1
                    '  lstPages.ItemsSource = list
                Else
                    'ServerDisplay.SiteName.Text += "Nothing"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Sitesproxy_ReturnDisplayFilesCompleted(sender As Object, e As LiveMonitoring.ReturnDisplayFilesCompletedEventArgs) Handles Sitesproxy.ReturnDisplayFilesCompleted
        Try
            If IsNothing(e.Result) = False Then
                lstDisplayFiles.Items.Clear()
                For Each MyString In e.Result
                    lstDisplayFiles.Items.Add(MyString)
                Next
            End If
        Catch ex As Exception

        End Try
       
    End Sub

    Private Sub lstDisplayFiles_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles lstDisplayFiles.KeyDown
        Try
            If e.Key = Key.Escape Then
                lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub lstDisplayFiles_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles lstDisplayFiles.KeyUp
        If e.Key = Key.Escape Then
            lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub

    Private Sub lstDisplayFiles_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstDisplayFiles.SelectionChanged
        Try
            If UBound(e.AddedItems) > -1 Then
                If IsNothing(e.AddedItems.Item(0)) = False Then
                    txtMediaFile.Text = Me.GetParameters("ServiceRef").Replace("SLIPmonInterfaceSVC.svc", "DisplayDocuments/" + CStr(e.AddedItems.Item(0)))

                    lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
                End If
            End If
        Catch ex As Exception

        End Try
       


    End Sub
End Class
