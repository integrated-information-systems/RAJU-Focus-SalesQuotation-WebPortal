
Partial Class Business_OpenSQBySalesPerson
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadUserList()          
        End If
    End Sub
    Protected Sub LoadUserList()
        Dim Users As MembershipUserCollection = Membership.GetAllUsers()
        UserList.DataSource = Users
        UserList.DataBind()
        UserList.Items.Insert(0, New ListItem("-- Choose a User --", "0"))
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i = 0 To e.Row.Cells.Count - 1
                If i = 1 Then
                    Dim time As DateTime = DateTime.Parse(e.Row.Cells(i).Text)
                    e.Row.Cells(i).Text = time.ToString("dd/MM/yy")
                End If
            Next
        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim DocNum As String = GridView1.Rows(GridView1.SelectedIndex).Cells(2).Text
        SqlDataSource2.SelectParameters("DocNum").DefaultValue = DocNum
        SqlDataSource2.DataBind()

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        GridView1.SelectedIndex = -1
        GridView2.SelectedIndex = -1
        SqlDataSource2.SelectParameters("DocNum").DefaultValue = ""
        SqlDataSource2.DataBind()
        Dim UserName As String = UserList.Text
        Dim SAPDB As String = ConfigurationManager.AppSettings("Company_DB")
        Dim CustomDb As String = ConfigurationManager.AppSettings("Custom_DB")
        SqlDataSource1.SelectCommand = "SELECT T1.DocDate,T1.DocNum,T1.CardCode,T1.CardName,T1.DocTotal as 'Amount',T3.DESCRIPT AS 'Territory', T4.CreatedBy FROM [" & SAPDB & "].[dbo].[OQUT] T1 INNER JOIN [" & SAPDB & "].[dbo].[OCRD] T2 ON T2.CardCode=T1.CardCode INNER JOIN [" & SAPDB & "].[dbo].[OTER] T3 ON T3.territryID = T2.Territory INNER JOIN [" & CustomDb & "].[dbo].[SqHeader] T4 ON T4.DocEntry= T1.DocEntry and T4.DocStatus is NULL WHERE T4.CreatedBy=@CreatedBy"
        'SqlDataSource1.SelectCommand = "SELECT T1.DocDate,T1.DocNum,T1.CardCode,T1.CardName,T1.DocTotal as 'Amount',T3.DESCRIPT AS 'Territory' FROM [" & SAPDB & "].[dbo].[OQUT] T1 INNER JOIN [" & SAPDB & "].[dbo].[OCRD] T2 ON T2.CardCode=T1.CardCode INNER JOIN [" & SAPDB & "].[dbo].[OTER] T3 ON T3.territryID = T2.Territory"
        SqlDataSource1.SelectParameters("CreatedBy").DefaultValue = UserName
        SqlDataSource1.DataBind()
    End Sub
End Class
