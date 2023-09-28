Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
Imports SLServerDisplay.LiveMonitoring

Partial Public Class ChildEditContentPage
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
        Sitesproxy.GetDisplayGroupsAsync(CInt(_parameters("DisplayID")))
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
        Sitesproxy.EditDisplayGroupPageAsync(CInt(txtGroupID.Text), CInt(_parameters("DisplayID")), Me.txtDisplayName.Text, CInt(Me.cmbType.Items(Me.cmbType.SelectedIndex).tag), "",
                                        txtWidth.Value, txtHeight.Value, txtScreen.Value, CInt(Me.cmbPanelPos.Items(Me.cmbPanelPos.SelectedIndex).tag),
                                        CInt(Me.cmbPanel.Items(Me.cmbPanel.SelectedIndex).tag), txtMediaFile.Text, "", 0, txtTimer.Value)

        'close
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private Sub btnFiles_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnFiles.Click
        lstDisplayFiles.Visibility = Windows.Visibility.Visible
        Sitesproxy.ReturnDisplayFilesAsync()
    End Sub
    Private Sub SetSelected(CmbBox As Windows.Controls.ComboBox, TagVal As Integer)
        For Each MyItem As ComboBoxItem In CmbBox.Items
            If CInt(MyItem.Tag) = TagVal Then
                MyItem.IsSelected = True
                CmbBox.SelectedItem = MyItem
            Else
                MyItem.IsSelected = False
            End If
        Next
    End Sub
    Private Sub Sitesproxy_GetDisplayGroupCompleted(sender As Object, e As LiveMonitoring.GetDisplayGroupCompletedEventArgs) Handles Sitesproxy.GetDisplayGroupCompleted
        Dim PageNames As New Collection
        Try

            If IsNothing(e.Result) = False Then
                Dim MySite As New ServerDisplay(_parameters)
                Dim MyString() = e.Result.Split("|")


                txtDisplayName.Text = MyString(0)
                SetSelected(cmbType, CInt(MyString(5)))
                ' cmbType.SelectedIndex = CInt(MyString(5))
                txtWidth.Value = CInt(MyString(8))
                txtHeight.Value = CInt(MyString(7))
                txtScreen.Value = CInt(MyString(4))
                SetSelected(cmbPanel, CInt(MyString(2)))
                ' cmbPanel.SelectedIndex = CInt(MyString(2))
                SetSelected(cmbPanelPos, CInt(MyString(3)))
                'cmbPanelPos.SelectedIndex = CInt(MyString(3))
                txtMediaFile.Text = CStr(MyString(9))
                txtTimer.Value = CDbl(MyString(11))


                'MyString = Mysqlreader.Item("GroupName").ToString + "|" + Mysqlreader.Item("ID").ToString + "|" + Mysqlreader.Item("PanelNo").ToString + "|" + Mysqlreader.Item("PanelPos").ToString + "|"
                'MyString += Mysqlreader.Item("Screen").ToString + "|"
                'MyString += Mysqlreader.Item("DisplayType").ToString + "|"
                'If IsDBNull(Mysqlreader.Item("DisplayImage")) = False Then
                '    MyString += CStr(If(Mysqlreader.Item("DisplayImage"), "")) + "|"
                'Else
                '    MyString += "|"
                'MySite.Name = MyString(0)

                'MySite.PageNo = CInt(MyString(2))

                'MySite.ScreenSetting = CInt(MyString(4))
                'Try
                '    MySite.DisplayType = CInt(MyString(5))
                '    MySite.DisplayImage = CStr(MyString(6))
                '    If MyString(7) <> "" Then
                '        MySite.DisplayHeight = CInt(MyString(7))
                '    Else
                '        MySite.DisplayHeight = 0
                '    End If
                '    If MyString(8) <> "" Then
                '        MySite.DisplayWidth = CInt(MyString(8))
                '    Else
                '        MySite.DisplayWidth = 0
                '    End If
                '    MySite.ExtraData = CStr(MyString(9))
                '    If MyString(10) <> "" Then
                '        MySite.Extravalue = CDbl(MyString(10))
                '    Else
                '        MySite.Extravalue = 0
                '    End If
                '    Try
                '        MySite.ExtraData1 = CStr(MyString(11))
                '        If MyString(12) <> "" Then
                '            MySite.Extravalue1 = CDbl(MyString(12))
                '        Else
                '            MySite.Extravalue1 = 0
                '        End If
                '    Catch ex As Exception

                '    End Try



                'Catch ex As Exception

                'End Try
                ''siteid starts loading details
                'MySite.SiteID = CInt(MyString(1))

            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Sitesproxy_GetDisplayGroupsCompleted(sender As Object, e As LiveMonitoring.GetDisplayGroupsCompletedEventArgs) Handles Sitesproxy.GetDisplayGroupsCompleted
        Dim PageNames As New Collection
        Try

            If IsNothing(e.Result) = False Then
                If e.Result.Count() > 0 Then


                    For mycnt As Integer = 0 To e.Result.Count - 1
                        Try
                            Dim MyString() = e.Result.Item(mycnt).Split("|")
                            Dim MyListitem As New ListBoxItem
                            'MyListitem.
                            MyListitem.BorderBrush = New System.Windows.Media.SolidColorBrush(Colors.Black)
                            MyListitem.BorderThickness = New Thickness(2, 2, 2, 2)
                            'BorderThickness="2" BorderBrush="Blue"

                            MyListitem.Content += "Name:" + MyString(0)
                            MyListitem.Content += "  Page:" + MyString(4)
                            MyListitem.Content += "  ID:" + MyString(1)
                            MyListitem.Tag = CInt(MyString(1))
                            'MyListitem.Content += " ID:" + MyString(0)
                            'MyListitem.Content += " AlertType:" + MyString(1)

                            lstGroups.Items.Add(MyListitem)
                        Catch ex As Exception

                        End Try


                    Next

                Else
                    'ServerDisplay.SiteName.Text += "Nothing"
                End If
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Sitesproxy_ReturnDisplayFilesCompleted(sender As Object, e As LiveMonitoring.ReturnDisplayFilesCompletedEventArgs) Handles Sitesproxy.ReturnDisplayFilesCompleted
        If IsNothing(e.Result) = False Then
            lstDisplayFiles.Items.Clear()
            For Each MyString In e.Result
                lstDisplayFiles.Items.Add(MyString)
            Next
        End If
    End Sub

    Private Sub lstDisplayFiles_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles lstDisplayFiles.KeyDown
        If e.Key = Key.Escape Then
            lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub

    Private Sub lstDisplayFiles_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles lstDisplayFiles.KeyUp
        If e.Key = Key.Escape Then
            lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub

    Private Sub lstDisplayFiles_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstDisplayFiles.SelectionChanged
        If UBound(e.AddedItems) > -1 Then
            If IsNothing(e.AddedItems.Item(0)) = False Then
                txtMediaFile.Text = Me.GetParameters("ServiceRef").Replace("SLIPmonInterfaceSVC.svc", "DisplayDocuments/" + CStr(e.AddedItems.Item(0)))

                lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
            End If
        End If


    End Sub

    Private Sub lstGroups_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstGroups.SelectionChanged
        If UBound(e.AddedItems) > -1 Then
            If IsNothing(e.AddedItems.Item(0)) = False Then
                Dim MyListitem As ListBoxItem = CType(e.AddedItems.Item(0), ListBoxItem)
                Controls.Visibility = Windows.Visibility.Visible
                txtGroupID.Text = MyListitem.Tag
                Sitesproxy.GetDisplayGroupAsync(CInt(MyListitem.Tag))
                ' txtMediaFile.Text = Me.GetParameters("ServiceRef").Replace("SLIPmonInterfaceSVC.svc", "DisplayDocuments/" + CStr(e.AddedItems.Item(0)))

                'lstDisplayFiles.Visibility = Windows.Visibility.Collapsed
            End If
        End If
    End Sub

    Private Sub DelButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles DelButton.Click
        If Controls.Visibility = Windows.Visibility.Collapsed Then
            MessageBox.Show("Select Page to Delete!", "Delete", MessageBoxButton.OK)
            Exit Sub
        Else
            If MessageBox.Show("Are you sure to delete this page ?", "Confirmation", MessageBoxButton.OKCancel) = MessageBoxResult.OK Then
                Sitesproxy.DelDisplayGroupAsync(CInt(txtGroupID.Text))
                Me.DialogResult = True
            End If

        End If
    End Sub
End Class
