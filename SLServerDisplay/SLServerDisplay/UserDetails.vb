Public Class UserDetails
        Public ID As Integer
        Public PeopleID As Integer
        Public FirstName As String
        Public SurName As String
        Public UserLevel As Integer
        Public Phone As String
        Public Fax As String
        Public Cell As String
        Public Pager As String
        Public Address As String
        Public Email As String
        Public Password As String
        Public LastLoginDT As DateTime
        Public LastPasswordDT As DateTime
        Public UserName As String
        Public LoginDT As DateTime
        Public LastLogon As DateTime
        Public LastPassword As DateTime
        Public UserSites As New List(Of Integer)
        Public UserSensors As List(Of Integer)
        Public UserIPDevices As List(Of Integer)
        Public UserOtherDevices As List(Of Integer)
        Public UserSNMPDevices As List(Of Integer)
        Public UserCameras As List(Of Integer)
End Class
Class People
    Public ID As Integer
    Public FirstName As String
    Public SurName As String
    Public Phone As String
    Public Fax As String
    Public Cell As String
    Public Address As String
    Public Email As String
End Class
