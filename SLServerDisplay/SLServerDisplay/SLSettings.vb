Imports System.Text


Public Class SLSettings
    Private myServiceURL As String
    Private myRepeatSound As Boolean = False
    Private myShowDetail As Boolean = False
    Private myShowPageButton As Boolean = False
    Private myPageTimer As Integer = 18
    Private myServerRefereshTimer As Integer = 123
    Private myDisplayID As Integer = 1
    Public ReadOnly Property ServiceURL() As String
        Get
            Return myServiceURL
        End Get
    End Property
    Public ReadOnly Property RepeatSound() As Boolean
        Get
            Return myRepeatSound
        End Get
    End Property
    Public ReadOnly Property ShowPageButton() As Boolean
        Get
            Return myShowPageButton
        End Get
    End Property
    Public ReadOnly Property ShowDetail() As Boolean
        Get
            Return myShowDetail
        End Get
    End Property
    Public ReadOnly Property PageTimer() As Integer
        Get
            Return myPageTimer
        End Get
    End Property
    Public ReadOnly Property DisplayID() As Integer
        Get
            Return myDisplayID
        End Get
    End Property
    Public ReadOnly Property ServerDisplayTimer() As Integer
        Get
            Return myServerRefereshTimer
        End Get
    End Property
    Public Sub XMLReaderTest()
        'Dim output As StringBuilder = New StringBuilder()

        'Dim xmlString As String = _
        '    "<bookstore>" & _
        '            "<book genre='autobiography' publicationdate='1981-03-22' ISBN='1-861003-11-0'>" & _
        '                "<title>The Autobiography of Benjamin Franklin</title>" & _
        '                "<author>" & _
        '                    "<first-name>Benjamin</first-name>" & _
        '                    "<last-name>Franklin</last-name>" & _
        '                "</author> " & _
        '                "<price>8.99</price>" & _
        '            "</book>" & _
        '        "</bookstore>"

        '' Create an XmlReader
        'Using reader As XmlReader = XmlReader.Create(New StringReader(xmlString))

        '    reader.ReadToFollowing("book")
        '    reader.MoveToFirstAttribute()
        '    Dim genre As String = reader.Value
        '    output.AppendLine("The genre value: " + genre)

        '    reader.ReadToFollowing("title")
        '    output.AppendLine("Content of the title element: " + reader.ReadElementContentAsString())
        'End Using

        'OutputTextBlock.Text = output.ToString()
        Try
            'Dim output As New StringBuilder()

            Dim settings As New XmlReaderSettings()
            settings.XmlResolver = New XmlXapResolver()
            'dim reader as  XmlReader.Create("datafile.xml") '  //How do I use a URI location here?
            Dim reader As XmlReader = XmlReader.Create("SLIPMonSettings.xml")
            Try
                reader.ReadToFollowing("Service")
                reader.MoveToFirstAttribute()
                myServiceURL = reader.ReadElementContentAsString
            Catch ex As Exception

            End Try

            Try
                ' reader.ReadToFollowing("Settings")
                ' reader.MoveToContent()
                reader.ReadToFollowing("RepeatSound")
                reader.MoveToFirstAttribute()
                myRepeatSound = CBool(reader.ReadElementContentAsString)
            Catch ex As Exception

            End Try

            Try
                reader.ReadToFollowing("ShowDetail")
                reader.MoveToFirstAttribute()
                ' reader.ReadToFollowing("ShowDetail")
                myShowDetail = CBool(reader.ReadElementContentAsString)
            Catch ex As Exception

            End Try
            Try
                reader.ReadToFollowing("PageTimer")
                reader.MoveToFirstAttribute()
                ' reader.ReadToFollowing("ShowDetail")
                myPageTimer = reader.ReadElementContentAsInt
            Catch ex As Exception

            End Try
            Try
                reader.ReadToFollowing("ServerRefereshTimer")
                reader.MoveToFirstAttribute()
                ' reader.ReadToFollowing("ShowDetail")
                myServerRefereshTimer = reader.ReadElementContentAsInt
            Catch ex As Exception

            End Try
            Try
                reader.ReadToFollowing("ShowPageButton")
                reader.MoveToFirstAttribute()
                ' reader.ReadToFollowing("ShowDetail")
                myShowPageButton = CBool(reader.ReadElementContentAsString)
            Catch ex As Exception

            End Try

            Try
                reader.ReadToFollowing("DisplayID")
                reader.MoveToFirstAttribute()
                ' reader.ReadToFollowing("ShowDetail")
                myDisplayID = CInt(reader.ReadElementContentAsString)
            Catch ex As Exception

            End Try
            reader.Close()

            '    Dim result As MessageBoxResult = _
            'MessageBox.Show(output.ToString, _
            '"Result Sign", MessageBoxButton.OKCancel)
            ' OutputTextBlock.Text = output.ToString()
        Catch ex As Exception

        End Try


    End Sub

    Public Sub New()
        XMLReaderTest()
    End Sub
End Class
