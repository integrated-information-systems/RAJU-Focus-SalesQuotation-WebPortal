
Partial Class Account_Register
    Inherits System.Web.UI.Page

   
    Protected Sub CreateUserWizard1_CreatedUser(ByVal sender As Object, ByVal e As System.EventArgs) Handles CreateUserWizard1.CreatedUser
        Dim Username As TextBox = CreateUserWizardStep1.ContentTemplateContainer.FindControl("Username")
        Dim SalesPersonName As DropDownList = CreateUserWizardStep1.ContentTemplateContainer.FindControl("SalesPersonName")
        Dim Territory As ListBox = CreateUserWizardStep1.ContentTemplateContainer.FindControl("Territory")
        Dim TerritoryValues As String = String.Empty
        Dim MembershipUser1 As MembershipUser = Membership.GetUser(Username.Text)
        Dim Userid As Object = MembershipUser1.ProviderUserKey
        Dim anonymousProfile As ProfileCommon = Profile.GetProfile(Username.Text)
        anonymousProfile.SalesPersonName = SalesPersonName.SelectedItem.Value
        Dim index() As Integer = Territory.GetSelectedIndices()
        For Each i In index
            TerritoryValues = TerritoryValues & Territory.Items(i).Value & "|"
        Next
        anonymousProfile.Territory = TerritoryValues
        anonymousProfile.Save()
    End Sub

    
    
End Class
