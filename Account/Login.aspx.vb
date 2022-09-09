Imports System.Data.SqlClient
Imports System.Data
Imports System.Diagnostics
Partial Class Account_Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        Try

            If Not IsPostBack Then
                Dim ConString As String = ConfigurationManager.ConnectionStrings("IntegPortal_ConnectionString").ToString
                Dim DB_Name As String = ConfigurationManager.AppSettings("DB_Name").ToString
                Dim ReturnResult As New DataTable
                Using SqlCon As New SqlConnection(ConString)
                    Dim Qry As String = "  Select T1.*, T2.UserName as 'ExistingUName' from Users T1 LEFT JOIN " & DB_Name & ".dbo.aspnet_Users T2 ON T1.UserName=T2.UserName "

                    SqlCon.Open()
                    Using cmd As New SqlCommand(Qry)
                        cmd.Connection = SqlCon
                        Dim SqlAdap As New SqlDataAdapter(cmd)
                        SqlAdap.Fill(ReturnResult)
                        SqlAdap.Dispose()
                    End Using

                End Using

                If Not IsNothing(ReturnResult) Then

                    For Each DRow As DataRow In ReturnResult.Rows
                        If IsDBNull(DRow.Item("ExistingUName")) Then
                            Membership.CreateUser(DRow.Item("UserName").ToString(), DRow.Item("Password").ToString(), DRow.Item("Email").ToString())
                            If Roles.RoleExists("NormalUser") Then
                                Roles.AddUserToRole(DRow.Item("UserName").ToString(), "NormalUser")
                            End If
                        Else
                            Dim MUser As MembershipUser = Membership.GetUser(DRow.Item("UserName").ToString())

                            If Not IsNothing(MUser) Then

                                MUser.UnlockUser()
                                MUser.ChangePassword(MUser.ResetPassword, DRow.Item("Password").ToString())

                                MUser.Email = DRow.Item("Email").ToString()
                                ''MUser.IsApproved = True
                                Membership.UpdateUser(MUser)

                            End If
                        End If
                    Next
                End If
               


            End If

            If User.Identity.Name <> Nothing Then

                Response.Redirect("~")

            ElseIf Not IsNothing(Session("AccessPortal")) Then
                Dim AccessPortal As New List(Of String)

                AccessPortal = Session("AccessPortal")
                If AccessPortal.Contains("SQPortalLive") Then

                    If Not IsNothing(Session("Username")) Then

                        Dim UserCollect As New MembershipUserCollection
                        UserCollect = Membership.FindUsersByName(Session("Username"))
                        If UserCollect.Count <= 0 Then
                            Membership.CreateUser(Session("Username").ToString, Session("Email").ToString, Session("Password").ToString)
                            If Roles.RoleExists("SalesPerson") Then
                                Roles.AddUserToRole(Session("Username").ToString, "SalesPerson")
                            End If
                        End If
                        Response.Cookies.Remove(FormsAuthentication.FormsCookieName)
                        FormsAuthentication.SetAuthCookie(Session("Username").ToString, False)

                        If Not IsNothing(Request.QueryString("ReturnUrl")) Then

                            Response.Redirect(Request.QueryString("ReturnUrl"))
                        Else
                            Response.Redirect("~")
                        End If

                    End If
                End If

            End If

        Catch ex As Exception

            WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoginUser_LoggingIn(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.LoginCancelEventArgs) Handles LoginUser.LoggingIn

        'Dim MUser As MembershipUser = Membership.GetUser(LoginUser.UserName)
        'If MUser.IsOnline = True Then
        '    'e.Cancel = True
        'End If
    End Sub

    Public Sub WriteLog(ByRef ex As Exception)

        Dim St As StackTrace = New StackTrace(ex, True)

        Using SqlCon As New SqlConnection(ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString)

            SqlCon.Open()

            Dim ReaderCommand As Data.SqlClient.SqlCommand = New SqlCommand()
            ReaderCommand.Connection = SqlCon
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
        End Using

    End Sub
End Class