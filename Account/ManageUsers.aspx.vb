Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Data
Partial Class Account_ManageUser
    Inherits System.Web.UI.Page
    Public Shared FocusDBConn As New SqlConnection
    Public Shared SAPDBConn As New SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindUsers()
        End If
        If Account_ManageUser.FocusDBConn.State = 0 Then
            Account_ManageUser.FocusDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
            Account_ManageUser.FocusDBConn.Open()
        End If
        If Account_ManageUser.SAPDBConn.State = 0 Then
            Account_ManageUser.SAPDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            Account_ManageUser.SAPDBConn.Open()
        End If
    End Sub
    Public Overloads Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            ' Free other state (managed objects).
            Account_ManageUser.FocusDBConn.Close()
            Account_ManageUser.SAPDBConn.Close()
           
        End If
        ' Free your own state (unmanaged objects).

        ' Set large fields to null.
    End Sub
    Protected Sub BindUsers()
        GridView1.DataSource = Membership.GetAllUsers
        GridView1.DataBind()
    End Sub
    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim UserName As String = GridView1.Rows(e.RowIndex).Cells(1).Text

        If UserName.ToLower = "admin" Then
            LblResult.Text = String.Format("You Cannot Delete {0} User", UserName)
            LblResult.ForeColor = Color.Green
        Else
            Membership.DeleteUser(UserName)
            LblResult.Text = String.Format("User ""{0}"" Details deleted Successfully", UserName)
            LblResult.ForeColor = Color.Green
            BindUsers()
        End If
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        UserName.Text = GridView1.Rows(GridView1.SelectedIndex).Cells(1).Text
        Dim MUser As MembershipUser = Membership.GetUser(UserName.Text)
        Dim UpdateProfile As ProfileCommon = Profile.GetProfile(UserName.Text)
        Dim TerritoryList As String = UpdateProfile.Territory()
        Territory.ClearSelection()
        Dim SplittedTerritory() As String = TerritoryList.Split("|")
        For Each i In SplittedTerritory
            Dim index As Integer = Territory.Items.IndexOf(Territory.Items.FindByValue(i))
            If index <> -1 Then
                'Territory.SelectedIndex = Territory.Items.IndexOf(Territory.Items.FindByValue(i))
                Territory.Items.FindByValue(i).Selected = True
            End If
            'Territory.Items.FindByValue(i).Selected = True
        Next

            SalesPersonName.SelectedIndex = SalesPersonName.Items.IndexOf(SalesPersonName.Items.FindByValue(UpdateProfile.SalesPersonName))
            Email.Text = MUser.Email

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim MUser As MembershipUser = Membership.GetUser(UserName.Text)
        If Password.Text <> String.Empty Then
            MUser.UnlockUser()
            MUser.ChangePassword(MUser.ResetPassword(), Password.Text)
        End If

        MUser.Email = Email.Text
        Membership.UpdateUser(MUser)
        Dim UpdateProfile As ProfileCommon = Profile.GetProfile(UserName.Text)
        Dim TerritoryValues As String = String.Empty
        Dim index() As Integer = Territory.GetSelectedIndices()
        For Each i In index
            TerritoryValues = TerritoryValues & Territory.Items(i).Value & "|"
        Next
        UpdateProfile.Territory = TerritoryValues
        UpdateProfile.SalesPersonName = SalesPersonName.SelectedItem.Value

        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Account_ManageUser.SAPDBConn
        ReaderCommand.CommandText = "select empID from OHEM " & _
                                " WHERE OHEM.salesPrson=@LookUP"
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = SalesPersonName.SelectedItem.Value
        ReaderCommand.Parameters.Add(SqlPara)
        'ReaderCommand.Parameters.AddWithValue("@LookUP", Lookup)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        If reader.Read Then
            If Not IsDBNull(reader("empID")) Then
                UpdateProfile.DocOwner = reader("empID").ToString()
            Else
                UpdateProfile.DocOwner = ""
            End If
        Else
            UpdateProfile.DocOwner = ""
        End If

        reader.Close()
        ReaderCommand.Dispose()

        UpdateProfile.Save()
        SuccessMsg.Text = String.Format("User ""{0}"" Details updated Successfully", UserName.Text)
        SuccessMsg.ForeColor = Color.Green
    End Sub
    
End Class
