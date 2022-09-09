
Partial Class Site
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack = True Then
            Dim sURl As String = Request.Url.ToString().ToLower()
            'If sURl.EndsWith("default.aspx") Then
            '    NavigationMenu
            'End If

        End If
    End Sub

    Protected Sub HeadLoginStatus_LoggingOut(ByVal sender As Object, ByVal e As LoginCancelEventArgs)
        Try
            If Not IsNothing(Session("AccessPortal")) Then
                Dim AccessPortal As New List(Of String)

                AccessPortal = Session("AccessPortal")
                Dim i As Integer = 0
                For Each Item As String In AccessPortal
                    If AccessPortal.Item(i).Equals("SQPortalLive") Then
                        AccessPortal.RemoveAt(i)
                        Exit For
                    End If
                    i = i + 1
                Next

            End If
        Catch ex As Exception

        End Try
    End Sub
End Class

