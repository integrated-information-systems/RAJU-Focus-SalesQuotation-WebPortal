Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.IO
Partial Class ManageMessages
    Inherits System.Web.UI.Page
    Public Shared CustomDBConnString As String = String.Empty
    Public Shared SAPDBConnString As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If CustomDBConnString = String.Empty Then
            ManageMessages.CustomDBConnString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
        End If
        If SAPDBConnString = String.Empty Then
            ManageMessages.SAPDBConnString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If btnAdd.Text = "Add" Then
                Dim SQLCon As New SqlConnection
                SQLCon.ConnectionString = ManageMessages.CustomDBConnString
                SQLCon.Open()
                Dim SQLCommand As New SqlCommand
                SQLCommand.Connection = SQLCon
                Dim InsertQuery As String = String.Empty
                InsertQuery = " INSERT INTO [Messages]" & _
           "([Title]" & _
           ",[Message]" & _
           ",[Active])" & _
                "VALUES(" & _
           "@Title" & _
           ",@Message" & _
           ",@Active)"
                SQLCommand.CommandText = InsertQuery

                SQLCommand.Parameters.AddWithValue("@Title", txtTitle.Text)
                SQLCommand.Parameters.AddWithValue("@Message", txtMessage.Text)
                SQLCommand.Parameters.AddWithValue("@Active", CBool(rdoStatusList.SelectedItem.Value))

                SQLCommand.ExecuteNonQuery()
                SQLCommand.Dispose()
                SQLCon.Close()
                SuccessMsg.Text = "Added Successfully"
            ElseIf btnAdd.Text = "Update" Then
                Dim SQLCon As New SqlConnection
                SQLCon.ConnectionString = ManageMessages.CustomDBConnString
                SQLCon.Open()
                Dim SQLCommand As New SqlCommand
                SQLCommand.Connection = SQLCon
                Dim InsertQuery As String = String.Empty
                InsertQuery = " UPDATE  [Messages] SET " & _
           "[Title]=@Title" & _
           ",[Message]=@Message" & _
           ",[Active]=@Active" & _
                "  WHERE " & _
           "id=@id"
           
                SQLCommand.CommandText = InsertQuery
                Dim idbutton As LinkButton = CType(GridView1.Rows(GridView1.SelectedIndex).Cells(0).FindControl("btnDelete"), LinkButton)

                SQLCommand.Parameters.AddWithValue("@Title", txtTitle.Text)
                SQLCommand.Parameters.AddWithValue("@Message", txtMessage.Text)
                SQLCommand.Parameters.AddWithValue("@Active", CBool(rdoStatusList.SelectedItem.Value))
                SQLCommand.Parameters.AddWithValue("@id", idbutton.CommandArgument)
                SQLCommand.ExecuteNonQuery()
                SQLCommand.Dispose()
                SQLCon.Close()

                SuccessMsg.Text = "Updated Successfully"
                btnAdd.Text = "Add"
            End If

            RefreshData()
            ClearForm()
        Catch ex As Exception
            'AlertMsg(ex.Message)
            Response.Write(ex.Message)
        End Try
    End Sub
    Private Sub ClearForm()
        txtTitle.Text = String.Empty
        txtMessage.Text = String.Empty
        rdoStatusList.SelectedIndex = rdoStatusList.Items.IndexOf(rdoStatusList.Items.FindByValue(True))
    End Sub
    Public Sub AlertMsg(ByVal msg As String)
        System.Media.SystemSounds.Exclamation.Play()
        'Dim sMsg As String

        Dim sb As New StringBuilder

        'msg.Replace("\n", "\\n")

        'sMsg = msg.Replace("\", "'")

        sb.Append("<script language='javascript'>")

        sb.Append("alert('" + msg + "');")

        'sb.Append(" return true ; ")

        sb.Append("</script>")

        Response.Write(sb.ToString)

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim CName As String = e.CommandName
        Dim CArgument As String = e.CommandArgument

        
        If CName = "CustomEdit" Then
            Dim SQLSelectItemCon As New SqlConnection
            SQLSelectItemCon.ConnectionString = ManageMessages.CustomDBConnString
            SQLSelectItemCon.Open()

            Dim SQLSelectItemCommand As New SqlCommand
            SQLSelectItemCommand.Connection = SQLSelectItemCon

            Dim ValidateSelectItemQuery As String = String.Empty
            ValidateSelectItemQuery = "Select * from Messages  WHERE id=@id"

            SQLSelectItemCommand.CommandText = ValidateSelectItemQuery
            SQLSelectItemCommand.Parameters.AddWithValue("id", CArgument)
            Dim Reader As SqlDataReader = SQLSelectItemCommand.ExecuteReader
            If Reader.Read() Then
                txtTitle.Text = Reader("Title").ToString
                txtMessage.Text = Reader("Message").ToString
                rdoStatusList.SelectedIndex = rdoStatusList.Items.IndexOf(rdoStatusList.Items.FindByValue(CBool(Reader("Active"))))
                Dim IndexRow As GridViewRow = CType(CType(e.CommandSource, LinkButton).NamingContainer, GridViewRow)

                GridView1.SelectedIndex = IndexRow.RowIndex

                btnAdd.Text = "Update"
            End If
            SQLSelectItemCommand.Dispose()
            SQLSelectItemCon.Close()

        ElseIf CName = "CustomDelete" Then


            Dim SQLDeleteItemCon As New SqlConnection
            SQLDeleteItemCon.ConnectionString = ManageMessages.CustomDBConnString
            SQLDeleteItemCon.Open()

            Dim SQLDeleteItemCommand As New SqlCommand
            SQLDeleteItemCommand.Connection = SQLDeleteItemCon

            Dim ValidateDeleteItemQuery As String = String.Empty
            ValidateDeleteItemQuery = "DELETE from Messages  WHERE id=@id"

            SQLDeleteItemCommand.CommandText = ValidateDeleteItemQuery
            SQLDeleteItemCommand.Parameters.AddWithValue("id", CArgument)
            SQLDeleteItemCommand.ExecuteNonQuery()
            SQLDeleteItemCommand.Dispose()
            SQLDeleteItemCon.Close()
            RefreshData()
        End If
    End Sub
    Private Sub RefreshData()
        GridView1.DataBind()
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub

    

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub
End Class
