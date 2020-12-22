Imports System.IO






Public Class NetworkStatus
    Dim Dir As IO.StreamWriter
    Dim DirMac As IO.StreamWriter
    Dim username = Environ("username")
    Dim i As Integer
    Dim a As IO.StreamReader
    Dim c As String = "assets\lists\iplist.txt"
    Dim cbis As String = "assets\lists\maclist.txt"
    Dim f As String = "assets\scripts\command.bat"
    Dim menudir As String = "assets\scripts\menu.bat"
    Dim psdir As String = "assets\scripts\WOL.ps1"
    Dim movedir As String = "assets\scripts\movedir.bat"

    Dim batchwoldir As String = "assets\scripts\wol.bat"
    Dim ms As New Stopwatch
    Dim Version As String = "Version 1.1 Portable Edition / Developed by Thomas François"


    Dim b As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim ip = ListBox1.SelectedItem


        If My.Computer.Network.Ping(ip) Then
            Label4.Text = "Online"
            Label4.ForeColor = Color.LimeGreen
            ms.Start()
            My.Computer.Network.Ping(ip)
            ms.Stop()
            Label1.Text = (ms.ElapsedMilliseconds) & "ms"

        Else
            Label4.Text = "Offline"
            Label4.ForeColor = Color.OrangeRed
            Label1.Text = "Not available"
        End If


    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Shell("cmd.exe /c mkdir assets", AppWinStyle.Hide, True)
        Shell("cmd.exe /c mkdir assets\scripts", AppWinStyle.Hide, True)
        Shell("cmd.exe /c mkdir assets\lists", AppWinStyle.Hide, True)
        Label5.Text = Version




        If My.Computer.FileSystem.FileExists("mc-wol.exe") Then


        Else
            Dim wolfile = My.Resources.mc_wol
            File.WriteAllBytes("assets\scripts\mc-wol.exe", wolfile)


        End If







        Try
            ListBox1.Items.Clear()
            a = File.OpenText(c)
            While a.Peek <> -1
                b = a.ReadLine()
                ListBox1.Items.Add(b)
            End While
            a.Close()
        Catch ex As Exception

        End Try
        Try
            ListBox2.Items.Clear()
            a = File.OpenText(cbis)
            While a.Peek <> -1
                b = a.ReadLine()
                ListBox2.Items.Add(b)
            End While
            a.Close()
        Catch ex As Exception

        End Try


    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox1.Items.Add(TextBox1.Text)
        ListBox1.SelectedIndex = ListBox1.SelectedIndex + 1
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dir = New IO.StreamWriter(c)
        For i = 0 To ListBox1.Items.Count - 1
            Dir.WriteLine(ListBox1.Items.Item(i))
        Next
        Dir.Close()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim ip1 = ListBox1.SelectedItem
        REM Shell("tracert " & ip1, vbNormalFocus, True)
        REM Dim test As New ProcessStartInfo("tracert.exe", ip1)
        REM Dim test1 As New Process
        Dim test = New IO.StreamWriter(f)
        test.WriteLine("@echo off")
        test.WriteLine("color 1F")
        test.WriteLine("echo Hello %USERNAME%, I hope this software saved time for you")
        test.WriteLine("tracert " & ip1)
        test.WriteLine("pause > nul")
        test.Close()

        Dim tracert = Process.Start(f)
        tracert.WaitForExit()

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim ip2 = ListBox1.SelectedItem
        Dim Menu = New IO.StreamWriter(menudir)
        Menu.WriteLine("
@echo off

color 1F
echo NetworkStatus Advanced Menu
echo.
set ip=" & ip2 & "

if [%ip%]==[] (
echo No ip/Netbios name/Domain name chosen
) else (
echo You are curently using this ip/Netbios name/Domain name : %ip%
)
:menu
echo.
echo 1 for netstat, 2 for nslookup, 3 for arp, 4 arp using " & ip2 & "
set /p action= Action?

if %action%==1 goto netstat
if %action%==2 goto nslookup
if %action%==3 goto arp
if %action%==4 goto arpmore

pause > nul

:netstat
netstat

pause > nul
goto menu

:nslookup
nslookup " & ip2 & "

pause > nul
goto menu

:arp
arp -a
pause > nul
goto menu
:arpmore
arp -a " & ip2 & "
pause > nul

goto menu



")
        Menu.Close()
        Dim advancedmenu = Process.Start(menudir)
        advancedmenu.WaitForExit()
    End Sub


    Private Declare Function inet_addr Lib "wsock32.dll" (ByVal s As String) As Integer

    Private Declare Function SendARP Lib "iphlpapi.dll" (ByVal DestIP As Integer, ByVal SrcIP As Integer, ByRef pMACAddr As Integer, ByRef PhyAddrLen As Integer) As Integer

    Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef dst As Byte, ByRef src As Integer, ByVal bcount As Integer)


    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        Dim sip As String
        Dim inet As Integer
        Dim b(6) As Byte
        Dim pMACAddr As Integer
        Dim i1 As Short
        Dim sResult As String = ""

        sip = ListBox1.SelectedItem
        inet = inet_addr(sip)
        If SendARP(inet, 0, pMACAddr, 6) = 0 Then
            CopyMemory(b(0), pMACAddr, 6)
            For i1 = 0 To 5
                sResult = sResult & Microsoft.VisualBasic.Right("0" & Hex(b(i1)), 2)
                If i1 < 5 Then sResult &= ":"
            Next
        End If

        TextBox2.Text = sResult


    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim ip4 = ListBox1.SelectedItem
        Dim mac = ListBox2.SelectedItem


        Dim batchwol = New IO.StreamWriter(batchwoldir)
        batchwol.WriteLine("@echo off")
        batchwol.WriteLine("cd assets\scripts\")
        batchwol.WriteLine("mc-wol " & mac & "")


        batchwol.Close()
        Dim wol As New ProcessStartInfo()
        REM wol.Verb = "runas"
        wol.FileName = "assets\scripts\wol.bat"

        Try
            Dim executewol = Process.Start(wol)
            executewol.WaitForExit(10000)

        Catch ex As Exception
            MsgBox("User:" & username & " cancelled the operation", 16, "")
        End Try

    End Sub







    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        DirMac = New IO.StreamWriter(cbis)
        For i = 0 To ListBox2.Items.Count - 1
            DirMac.WriteLine(ListBox2.Items.Item(i))
        Next
        DirMac.Close()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ListBox2.Items.Add(TextBox3.Text)
        ListBox2.SelectedIndex = ListBox2.SelectedIndex + 1
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ListBox2.Items.RemoveAt(ListBox2.SelectedIndex)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Help.ShowDialog()
    End Sub

End Class
