Imports System.Configuration
Imports System.Data.Common
Imports System.Data.SqlClient
Imports Dapper
Imports Microsoft.Win32

Public Class Form1
    Shared kshibai As Integer = 0
    Shared xshibai As Integer = 0
    Public axCZKEM1 As New zkemkeeper.CZKEM
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If CheckBox1.Checked Then
            KaoQin()
        End If
        If CheckBox2.Checked Then
            XiaoFei()
        End If
    End Sub
    Public Sub ShowInfo(Info As String)
        TextBox1.AppendText(Info + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
        TextBox1.AppendText(Environment.NewLine)
        TextBox1.ScrollToCaret()
    End Sub
    Sub XiaoFei()
        Dim FHandle As Integer = Connect("protocol=HTTP,ipaddress=" + ConfigurationManager.AppSettings("xiaofeiip") + ",port=80,name=123456,passwd=123456")
        If FHandle > 0 Then
            Try
                Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("defCon").ConnectionString)
                    Dim tmplst As String = Space(2 * 1024 * 1024)
                    DataQuery(FHandle, tmplst, 2 * 1024 * 1024, "PosLog", "CardNo,PosTime", "PosTime LIKE " & DateTime.Now.ToString("yyyyMM*") & "", "")
                    For Each hang As String In tmplst.Split(Chr(13) & Chr(10))
                        Dim cardNo As String = ""
                        Dim attDate As String = ""
                        Dim col As Integer = 0
                        For Each ziduan As String In hang.Split(Chr(9))
                            If col = 0 Then
                                cardNo = ziduan
                            ElseIf col = 1 Then
                                attDate = ziduan
                            End If
                            col = col + 1
                        Next
                        Dim dt As DateTime
                        If Not String.IsNullOrEmpty(cardNo) AndAlso cardNo <> "CardNo" AndAlso DateTime.TryParseExact(attDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, dt) Then
                            If conn.Query("select F_Id from PosLog where CardNo=@CardNo and PosTime=@PosTime",
                                      New With {
                                      .CardNo = cardNo,
                                      .PosTime = dt}).Count = 0 Then
                                conn.Execute("insert into PosLog(F_Id,CardNo,PosTime)values(@F_Id,@CardNo,@PosTime);", New With {
                                     .F_Id = Guid.NewGuid(),
                                      .CardNo = cardNo,
                                      .PosTime = dt})
                            End If
                        End If
                    Next
                End Using
                ' MsgBox("Conncet Successfully" + tmplst)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            ShowInfo("消费机链接失败！")
            xshibai = xshibai + 1
            If xshibai > 10 Then
                CheckBox2.Checked = False
            End If
        End If
    End Sub
    Sub KaoQin()
        Dim bIsConnected = axCZKEM1.Connect_Net(ConfigurationManager.AppSettings("kaoqinip"), Integer.Parse(ConfigurationManager.AppSettings("kaoqinport")))
        If bIsConnected = True Then
            Dim sdwEnrollNumber As String = ""
            Dim idwVerifyMode As Integer = 0
            Dim idwInOutMode As Integer = 0
            Dim idwYear As Integer = 0
            Dim idwMonth As Integer = 0
            Dim idwDay As Integer = 0
            Dim idwHour As Integer = 0
            Dim idwMinute As Integer = 0
            Dim idwSecond As Integer = 0
            Dim idwWorkcode As Integer = 0
            axCZKEM1.EnableDevice(1, False)
            If axCZKEM1.ReadGeneralLogData(1) Then
                While axCZKEM1.SSR_GetGeneralLogData(1, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth,
                                    idwDay, idwHour, idwMinute, idwSecond, idwWorkcode)
                    Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("defCon").ConnectionString)
                        If conn.Query("select F_Id from  AttLog where CardNo=@CardNo and AttDate=@AttDate",
                                      New With {
                                      .CardNo = sdwEnrollNumber,
                                      .AttDate = New DateTime(idwYear, idwMonth,
                                    idwDay, idwHour, idwMinute, idwSecond)}).Count = 0 Then
                            conn.Execute("insert into AttLog(F_Id,CardNo,AttDate)values(@F_Id,@CardNo,@AttDate);", New With {
                                     .F_Id = Guid.NewGuid(),
                                      .CardNo = sdwEnrollNumber,
                                      .AttDate = New DateTime(idwYear, idwMonth,
                                    idwDay, idwHour, idwMinute, idwSecond)})
                        End If
                    End Using

                End While
            End If
            axCZKEM1.EnableDevice(1, True)
        Else
            ShowInfo("考勤机链接失败！")
            kshibai = kshibai + 1
            If kshibai > 10 Then
                CheckBox1.Checked = False
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckBox1.Checked Then
            KaoQin()
        End If
        If CheckBox2.Checked Then
            XiaoFei()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = ConfigurationManager.AppSettings("timer") * 60 * 1000
        Dim Local = Registry.LocalMachine
        Dim runKey As RegistryKey = Local.CreateSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run\")
        runKey.SetValue("同步数据", Application.StartupPath + "\\NFine.Tasks.exe")
        Local.Close()
    End Sub
End Class
