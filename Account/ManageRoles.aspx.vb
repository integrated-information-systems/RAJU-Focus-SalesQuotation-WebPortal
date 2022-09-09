Imports System.Array
Imports System.Data.SqlClient
Partial Class Account_ManageRoles
    Inherits System.Web.UI.Page

    Protected Sub submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit.Click
        If Not Roles.RoleExists(txtRoleName.Text) Then
            Roles.CreateRole(txtRoleName.Text)
            Clear()
            LoadRoles()
        End If
    End Sub
    Protected Sub CheckExist(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Roles.RoleExists(txtRoleName.Text) Then
            e.IsValid = False     
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' For Sync Program Notification 
        Dim ConStringToUser As String = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
        Using SqlCon As New SqlConnection(ConStringToUser)

            SqlCon.Open()
            Using cmd As New SqlCommand("Delete from AdminUsers")
                cmd.Connection = SqlCon
                cmd.ExecuteNonQuery()
            End Using
            For Each CurrentUser In Roles.GetUsersInRole("Admin")
                Dim UserProfile As ProfileCommon = Profile.GetProfile(CurrentUser)
                Dim CurrentUserMembership As MembershipUser = Membership.GetUser(CurrentUser)
                Dim CurrentUserEmail As String = CurrentUserMembership.Email
                Using cmd As New SqlCommand("Insert into AdminUsers Values(@Email)")
                    cmd.Connection = SqlCon
                    cmd.Parameters.AddWithValue("@Email", CurrentUserEmail)
                    cmd.ExecuteNonQuery()
                End Using
            Next


        End Using


        If IsPostBack = True Then

            Validate()
            If Page.IsValid = True Then
                SuccessMsg.Visible = True
            Else
                SuccessMsg.Visible = False
            End If
        Else
         
        End If
        ' Bind roles to GridView.

        LoadRoles()

    End Sub
    Protected Sub LoadRoles()
        Dim rolesArray() As String
        rolesArray = Roles.GetAllRoles()
        RolesGrid.DataSource = rolesArray
        RolesGrid.DataBind()
    End Sub
    Protected Sub Clear()
        txtRoleName.Text = String.Empty
    End Sub
    
End Class
