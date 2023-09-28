Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
'Imports SLServerDisplayOOB.LiveMonitoring
Imports System.Collections.ObjectModel
Imports SLServerDisplay.LiveMonitoring

'Imports SLServerDisplay.LiveMonitoring

Partial Public Class ChildListDisplays
    Inherits ChildWindow
    Private SelectedListitem As ListBoxItem
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
        Sitesproxy.GetDisplaysAsync() 'CInt(_parameters("DisplayID")))
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
        '  Sitesproxy.AddDisplayAsync(Me.txtDisplayName.Text, CInt(Me.cmbType.SelectedValue))

        'close
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub Sitesproxy_ChangeDisplayGroupPageCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles Sitesproxy.ChangeDisplayGroupPageCompleted
        Sitesproxy.GetDisplayGroupsAsync(CInt(_parameters("DisplayID")))
    End Sub

    Private Sub Sitesproxy_GetDisplayGroupsCompleted(sender As Object, e As GetDisplayGroupsCompletedEventArgs) Handles Sitesproxy.GetDisplayGroupsCompleted

    End Sub

    Private Sub LstPages_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles LstPages.SelectionChanged
        If UBound(e.AddedItems) > -1 Then
            If IsNothing(e.AddedItems.Item(0)) = False Then
                SelectedListitem = CType(e.AddedItems.Item(0), ListBoxItem)
            End If
        End If
    End Sub

    Private Sub Sitesproxy_GetDisplaysCompleted(sender As Object, e As GetDisplaysCompletedEventArgs) Handles Sitesproxy.GetDisplaysCompleted
        Dim DisplayNames As New Collection
        Try

            If IsNothing(e.Result) = False Then
                LstPages.Items.Clear()
                If e.Result.Count() > 0 Then
                    'Dim list As New ObservableCollection(Of String)

                    'list.Add(1);
                    'list.Add(2);
                    'list.Add(3);

                    'listBox1.ItemsSource = list;

                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Try
                            Dim MyString() = e.Result.Item(mycnt).Split("|")
                            Dim MyListitem As New ListBoxItem
                            'MyListitem.
                            MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                            MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                            'BorderThickness="2" BorderBrush="Blue"
                            'items.Add(MyGroup.DisplayName + "|" + MyGroup.ID.ToString + "|" + MyGroup.DisplayType.ToString + "|" + MyGroup.DefaultOrderByColumn.ToString + "|" + MyGroup.ExtraData.ToString + "|" + MyGroup.ExtraValue.ToString)

                            MyListitem.Content += "Name:" + MyString(0)
                            MyListitem.Content += "  Type:" + MyString(3)
                            MyListitem.Content += "  ID:" + MyString(1)
                            MyListitem.Tag = e.Result(mycnt) 'CInt(MyString(1))
                            'MyListitem.Content += " ID:" + MyString(0)
                            'MyListitem.Content += " AlertType:" + MyString(1)
                            ' list.Add(MyListitem.Content)
                            LstPages.Items.Add(MyListitem)
                        Catch ex As Exception

                        End Try


                    Next
                    '  lstPages.ItemsSource = list
                Else
                    'ServerDisplay.SiteName.Text += "Nothing"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
