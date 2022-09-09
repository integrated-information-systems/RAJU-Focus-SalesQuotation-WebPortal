Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Diagnostics
Partial Class Business_ReportViewer
    Inherits System.Web.UI.Page
    Public Shared FocusDBConn As New SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Business_ReportViewer.FocusDBConn.State = 0 Then
            Business_ReportViewer.FocusDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
            Business_ReportViewer.FocusDBConn.Open()
        End If
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.CrystalReportViewer1.PDFOneClickPrinting = False
        Try
            Dim DocEntry As String = String.Empty
            Dim SQorIndenter As String = String.Empty
            Dim LayoutType As String = String.Empty

            If Not IsNothing(Request.QueryString("DocEntry")) Then
                DocEntry = Request.QueryString("DocEntry").ToString
                Session("DocEntry") = DocEntry
            ElseIf Not IsNothing(Session("DocEntry")) Then
                DocEntry = Session("DocEntry")
            End If

            If Not IsNothing(Request.QueryString("Layout")) Then
                SQorIndenter = Request.QueryString("Layout").ToString
                Session("SQorIndenter") = SQorIndenter
            ElseIf Not IsNothing(Session("SQorIndenter")) Then
                SQorIndenter = Session("SQorIndenter")
            End If

            If Not IsNothing(Request.QueryString("LayoutType")) Then
                LayoutType = Request.QueryString("LayoutType").ToString
                Session("LayoutType") = SQorIndenter
            ElseIf Not IsNothing(Session("LayoutType")) Then
                LayoutType = Session("LayoutType")
            End If

            Dim ReportFileName As String

            If LayoutType = "Disc" Then
                If SQorIndenter = "C" Then
                    ReportFileName = Server.MapPath("~/Report/SQ.report-Disc.rpt")
                Else
                    ReportFileName = Server.MapPath("~/Report/Indenter-Disc.rpt")
                End If
            Else
                If SQorIndenter = "C" Then
                    ReportFileName = Server.MapPath("~/Report/SQ.report1.rpt")
                Else
                    ReportFileName = Server.MapPath("~/Report/Indenter.rpt")
                End If

            End If



            'DocEntry = "503280"

            Dim ReportFilesPath As String

            ReportFilesPath = ReportFileName



            Dim crReportDocument As New CrystalDecisions.Web.CrystalReportSource

            crReportDocument.Report.FileName = ReportFilesPath
            crReportDocument.ReportDocument.FileName = ReportFilesPath



            If DocEntry <> String.Empty Then
                crReportDocument.ReportDocument.SetParameterValue("@id", DocEntry)
            Else
                crReportDocument.ReportDocument.SetParameterValue("@id", Nothing)
            End If



            Me.CrystalReportViewer1.ReportSource = crReportDocument
            Me.CrystalReportViewer1.EnableParameterPrompt = False
            Me.CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None



            Me.CrystalReportViewer1.LogOnInfo(0).ConnectionInfo.ServerName = ConfigurationManager.AppSettings("Company_Server")
            Me.CrystalReportViewer1.LogOnInfo(0).ConnectionInfo.UserID = ConfigurationManager.AppSettings("DB_Username")
            Me.CrystalReportViewer1.LogOnInfo(0).ConnectionInfo.Password = ConfigurationManager.AppSettings("DB_Password")
            Me.CrystalReportViewer1.EnableDatabaseLogonPrompt = False

        Catch ex As Exception

            WriteLog(ex)
        End Try

    End Sub
    Public Sub WriteLog(ByRef ex As Exception)

        Dim St As StackTrace = New StackTrace(ex, True)

        If Business_ReportViewer.FocusDBConn.State = 0 Then
            Business_ReportViewer.FocusDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
            Business_ReportViewer.FocusDBConn.Open()
        End If

        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_ReportViewer.FocusDBConn

        ReaderCommand.CommandText = "INSERT INTO Errlog values(@errMsg,@InnerException,@FileName,@LineNumber,@CreatedOn) "

        ReaderCommand.Parameters.AddWithValue("@errMsg", ex.Message)
        If Not IsNothing(ex.InnerException) Then
            ReaderCommand.Parameters.AddWithValue("@InnerException", ex.InnerException.Message)
        Else
            ReaderCommand.Parameters.AddWithValue("@InnerException", String.Empty)
        End If
        Dim Filename As String = String.Empty
        Dim Linenumber As String = String.Empty
        Dim FrameCount As Integer = St.FrameCount
        For i As Integer = 0 To FrameCount - 1
            If Not IsNothing(St.GetFrame(i).GetFileName) Then
                Filename = St.GetFrame(i).GetFileName.ToString
                Linenumber = St.GetFrame(i).GetFileLineNumber.ToString
            End If
        Next
        ReaderCommand.Parameters.AddWithValue("@FileName", Filename)
        ReaderCommand.Parameters.AddWithValue("@LineNumber", Linenumber)

        ReaderCommand.Parameters.AddWithValue("@CreatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))


        ReaderCommand.ExecuteNonQuery()

        ReaderCommand.Dispose()
    End Sub
End Class
