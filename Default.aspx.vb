Imports System.Data.SqlClient
Imports System.Data
Partial Class _Default
    Inherits System.Web.UI.Page
    Public Shared CustomDBConnString As String = String.Empty
    Public Shared SAPDBConnString As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If CustomDBConnString = String.Empty Then
            _Default.CustomDBConnString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
        End If
        If SAPDBConnString = String.Empty Then
            _Default.SAPDBConnString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        End If
        loadMessages()
    End Sub
    Private Sub loadMessages()
        Dim message As String = String.Empty

        Dim SQLSelectItemCon As New SqlConnection
        SQLSelectItemCon.ConnectionString = _Default.CustomDBConnString
        SQLSelectItemCon.Open()

        Dim SQLSelectItemCommand As New SqlCommand
        SQLSelectItemCommand.Connection = SQLSelectItemCon

        Dim ValidateSelectItemQuery As String = String.Empty
        ValidateSelectItemQuery = "Select * from Messages  WHERE Active=@Active"

        SQLSelectItemCommand.CommandText = ValidateSelectItemQuery
        SQLSelectItemCommand.Parameters.AddWithValue("Active", 1)
        Dim Reader As SqlDataReader = SQLSelectItemCommand.ExecuteReader
        While Reader.Read()
            message = message & "<b>" & Reader("Title").ToString & "</b><br/>"
            message = message & Reader("Message").ToString & "<br/>"
        End While
        lblMessages.Text = message
        SQLSelectItemCommand.Dispose()
        SQLSelectItemCon.Close()
    End Sub
End Class
