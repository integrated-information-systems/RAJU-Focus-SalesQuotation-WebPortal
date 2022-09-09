Imports System.Drawing

Partial Class Account_AssignRoles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadUserList()
            LoadRolesList()
        End If

    End Sub
    Protected Sub LoadUserList()
        Dim Users As MembershipUserCollection = Membership.GetAllUsers()
        UserList.DataSource = Users
        UserList.DataBind()
        UserList.Items.Insert(0, New ListItem("-- Choose a User --", "0"))
    End Sub
    Protected Sub LoadRolesList()
        Dim rolesArray As List(Of String)
        rolesArray = Roles.GetAllRoles().ToList
        UsersRoleList.DataSource = rolesArray
        UsersRoleList.DataBind()
    End Sub

    Protected Sub btnAssignRoles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssignRoles.Click
        Dim selectedUser As String = String.Empty
        selectedUser = UserList.SelectedValue
        If selectedUser <> "0" Then
            For Each item As RepeaterItem In UsersRoleList.Items
                Dim CurrentItem As CheckBox = item.FindControl("RoleCheckBox")
                If CurrentItem.Checked = True And Not Roles.IsUserInRole(selectedUser, CurrentItem.Text) Then
                    Roles.AddUserToRole(selectedUser, CurrentItem.Text)
                ElseIf CurrentItem.Checked = False And Roles.IsUserInRole(selectedUser, CurrentItem.Text) Then

                    Roles.RemoveUserFromRole(selectedUser, CurrentItem.Text)

                End If

            Next
        End If
        LblSuccess.Text = String.Format(" ""{0}"" user roles applied Successfully", selectedUser)
        LblSuccess.ForeColor = Color.Green
    End Sub

    Protected Sub UserList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserList.SelectedIndexChanged
        Dim selectedUser As String = String.Empty
        selectedUser = UserList.SelectedValue
        If selectedUser <> "0" Then
            For Each item As RepeaterItem In UsersRoleList.Items
                Dim CurrentItem As CheckBox = item.FindControl("RoleCheckBox")
                If Roles.IsUserInRole(selectedUser, CurrentItem.Text) Then
                    CurrentItem.Checked = True
                ElseIf Not Roles.IsUserInRole(selectedUser, CurrentItem.Text) Then

                    CurrentItem.Checked = False

                End If

            Next
        End If
    End Sub
End Class
