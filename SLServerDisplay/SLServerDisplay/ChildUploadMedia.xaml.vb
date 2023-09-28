Imports System.IO.IsolatedStorage
Imports System.Runtime.Serialization
Imports SLServerDisplay.LiveMonitoring

Partial Public Class ChildUploadMedia
    Inherits ChildWindow
    Private WithEvents Sitesproxy As SLIPmonInterfaceSVCClient
    Private Shared SLSettings As New SLSettings
    Private fileStream As System.IO.Stream
    Private numBytes As Long
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
        'BasicHttpBinding(binding = New BasicHttpBinding())
        'Dim readerQuotas As XmlDictionaryReaderQuotas '= New XmlDictionaryReaderQuotas
        'readerQuotas.MaxArrayLength = 25 * 1024

        'binding.ReaderQuotas = readerQuotas

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
        Sitesproxy.ReturnDisplayFilesAsync()
    End Sub
    Private Function GetParameters(ByVal ParameterID As String) As String
        Return _parameters(ParameterID)
    End Function

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        'save it
        'Sitesproxy.AddDisplayAsync(Me.txtDisplayName.Text, CInt(Me.cmbType.SelectedValue))

        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub



    Private Sub btnOpenFileDLg_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnOpenFileDLg.Click
        ' Create an instance of the open file dialog box.
        Dim openFileDialog1 As OpenFileDialog = New OpenFileDialog

        ' Set filter options and filter index.
        openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        openFileDialog1.FilterIndex = 1

        openFileDialog1.Multiselect = False

        ' Call the ShowDialog method to show the dialogbox.
        Dim UserClickedOK As Boolean = openFileDialog1.ShowDialog

        ' Process input if the user clicked OK.
        If (UserClickedOK = True) Then
            txtMediaFile.Text = openFileDialog1.File.ToString
            numBytes = openFileDialog1.File.Length
            fileStream = openFileDialog1.File.OpenRead
        End If
    End Sub

    Private Sub btnUpload_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnUpload.Click
        Try
            If txtMediaFile.Text = "" Then
                MessageBox.Show("No File Selected.", "File Selection", MessageBoxButton.OK)
                Exit Sub
            End If
        Catch ex As Exception

        End Try
        Try
            'Open the selected file to read.
            ' fileStream.Length
            'Dim fStream As New FileStream("byteArray.dat", FileMode.Open, FileAccess.Read)


            ' // get the length of the file to see if it is possible
            ' // to upload it (with the standard 4 MB limit)
            'Dim numBytes As Long = fInfo.Length
            Dim dLen As Double = Convert.ToDouble(numBytes / 1000000)

            ' // Default limit of 4 MB on web server
            ' // have to change the web.config to if
            '  // you want to allow larger uploads
            If (dLen < 4) Then

                ''  // set up a file stream and binary reader for the
                ''   // selected file
                '    FileStream fStream = new FileStream(filename,
                '    FileMode.Open, FileAccess.Read);
                '    BinaryReader br = new BinaryReader(fStream);

                ''  // convert the file to a byte array
                '    byte[] data = br.ReadBytes((int)numBytes);
                '    br.Close();
                Dim br As New System.IO.BinaryReader(fileStream)
                Dim data As Byte() = br.ReadBytes(CInt(numBytes))
                ' Show the number of bytes in the array.
                'Label1.Text = Convert.ToString(data.Length)
                br.Close()
                '  // pass the byte array (file) and file name to the web
                Sitesproxy.UploadDisplayFileBinAsync(System.Convert.ToBase64String(data), txtMediaFile.Text)

                fileStream.Dispose()
                '   // this will always say OK unless an error occurs,
                '   // if an error occurs, the service returns the error
                'message()
                '    MessageBox.Show("File Upload Status: " + sTmp, "File
                'Upload(");")

            Else
                '     // Display message if the file was too large to upload
                MessageBox.Show("The file selected exceeds the size limit for uploads.", "File Size", MessageBoxButton.OK)

            End If

            'uploadit


            'Using reader As New System.IO.BinaryReader(fileStream)
            '    ' Read the first line from the file and write it to the text box.
            '    tbResults.Text = reader.ReadLine
            'End Using
            'fileStream.Close()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Sitesproxy_DeleteDisplayFilesCompleted(sender As Object, e As LiveMonitoring.DeleteDisplayFilesCompletedEventArgs) Handles Sitesproxy.DeleteDisplayFilesCompleted
        If IsNothing(e.Result) = False Then
            lstMediaFiles.Items.Clear()

            For Each MyString In e.Result
                lstMediaFiles.Items.Add(MyString)
            Next
        End If
    End Sub

    Private Sub Sitesproxy_ReturnDisplayFilesCompleted(sender As Object, e As LiveMonitoring.ReturnDisplayFilesCompletedEventArgs) Handles Sitesproxy.ReturnDisplayFilesCompleted
        If IsNothing(e.Result) = False Then
            lstMediaFiles.Items.Clear()
            For Each MyString In e.Result
                lstMediaFiles.Items.Add(MyString)
            Next
        End If
    End Sub

    Private Sub btnDeleteMedia_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnDeleteMedia.Click
       
        If lstMediaFiles.SelectedIndex > -1 Then
            If MessageBox.Show("Delete File:" + CStr(lstMediaFiles.SelectedItem) + " ?", "File Selection", MessageBoxButton.OKCancel) = MessageBoxResult.OK Then
                Sitesproxy.DeleteDisplayFilesAsync(CStr(lstMediaFiles.SelectedItem))
            End If
        Else
            MessageBox.Show("No  File Selected.", "File Selection", MessageBoxButton.OK)
        End If
    End Sub
End Class
