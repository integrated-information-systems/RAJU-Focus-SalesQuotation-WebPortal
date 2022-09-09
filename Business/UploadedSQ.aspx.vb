
Imports System.Data.SqlClient

Partial Class Business_UploadedSQ
    Inherits System.Web.UI.Page
    Public Shared FocusDBConn As New SqlConnection
    Public Shared SAPDBConn As New SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Business_UploadedSQ.FocusDBConn.State = 0 Then
            Business_UploadedSQ.FocusDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
            Business_UploadedSQ.FocusDBConn.Open()
        End If
        If Business_UploadedSQ.SAPDBConn.State = 0 Then
            Business_UploadedSQ.SAPDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            Business_UploadedSQ.SAPDBConn.Open()
        End If
        UpdateSQs()
        Dim ListofDocs As String = String.Empty
        ListofDocs = GetListofDocs()
        SqlDataSource1.SelectParameters.Clear()
        If ListofDocs <> String.Empty Then
            SqlDataSource1.SelectCommand = "SELECT CASE WHEN ISNULL(docnum,'')='' THEN id ELSE DocNum END AS  DocNum  , [DocStatus], [CardCode], [CardName], [ValidUntil], [DocDate] FROM [SqHeader] WHERE ([DocStatus] IS NULL) AND [id] IN (" + GetListofDocs() + ")  AND [CardName] LIKE @CardName  AND IsUpload=1 order by docnum desc"

        Else

            SqlDataSource1.SelectCommand = "SELECT CASE WHEN ISNULL(docnum,'')='' THEN id ELSE DocNum END AS  DocNum  , [DocStatus], [CardCode], [CardName], [ValidUntil], [DocDate] FROM [SqHeader] WHERE ([DocStatus] IS NULL) AND createdBy=@createdby  AND [CardName] LIKE @CardName AND IsUpload=1  order by docnum desc"
            SqlDataSource1.SelectParameters.Add("createdby", User.Identity.Name)
        End If

        SqlDataSource1.SelectParameters.Add("CardName", "%")
        SqlDataSource1.DataBind()
    End Sub
    Public Overloads Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Public Function GetListofDocs() As String
        Dim lstDocs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT id, CardCode FROM SqHeader " & "WHERE  DocStatus is Null AND IsUpload=1  ", SAPConn)
        SAPConn.Open()
        If User.IsInRole("SameTerritoryAccess") Then
            Dim CreatedbyList As List(Of String) = New List(Of String)
            Dim Createdby As String = String.Empty
            Dim DataTable As MembershipUserCollection = Membership.GetAllUsers()
            Dim Row As MembershipUser
            For Each Row In DataTable
                Dim CurrentUserProfile As ProfileCommon = ProfileBase.Create(Row.UserName, True)
                Dim CurrentTerritoryList As String = CurrentUserProfile.Territory()
                Dim CurrentUserSplittedTerritory() As String = CurrentTerritoryList.Trim("|").Split("|")
                Dim LoggedInUserProfile As ProfileCommon = ProfileBase.Create(User.Identity.Name, True)
                Dim LoggedInUserTerritoryList As String = LoggedInUserProfile.Territory()
                Dim LoggedInUserSplittedTerritory() As String = LoggedInUserTerritoryList.Trim("|").Split("|")
                For Each i As String In LoggedInUserSplittedTerritory
                    If CurrentUserSplittedTerritory.Any(Function(b) i.ToLower().Equals(b.ToLower())) Then
                        CreatedbyList.Add(Row.UserName)
                    End If
                Next
            Next
            Createdby = String.Join("','", CreatedbyList.ToArray)
            Createdby = "'" + Createdby + "'"
            SAPComm.CommandText += "  AND SqHeader.CreatedBy IN (" + Createdby + ")"
        Else
            SAPComm.CommandText += "  AND SqHeader.CreatedBy=@CreatedBy"
            SAPComm.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        End If
        SAPComm.CommandText += "  Order by SqHeader.id"

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            If AccessibleDoc(reader("CardCode").ToString()) Then
                lstDocs.Add(reader("id").ToString())
            End If
        End While
        reader.Close()
        SAPConn.Close()
        Dim Docs As String = String.Join(",", lstDocs.ToArray)
        Return Docs
    End Function
    Protected Function AccessibleDoc(ByVal BPCode As String) As Boolean
        Dim Accessible As Boolean = False


        Dim ReaderCommand As SqlCommand = New SqlCommand()
        Dim CardCodeTerritory As String = String.Empty
        ReaderCommand.Connection = Business_UploadedSQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT  OCRD.Territory from OCRD  " & "WHERE OCRD.CardCode=@LookUP"

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = BPCode
        ReaderCommand.Parameters.Add(SqlPara)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        If reader.Read() Then
            CardCodeTerritory = reader("Territory").ToString()
        End If
        reader.Close()
        ReaderCommand.Dispose()


        Dim SplittedTerritory() As String = {CardCodeTerritory}
        Dim CurrentUserProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
        Dim CurrentTerritoryList As String = CurrentUserProfile.Territory()
        Dim CurrentSplittedTerritory() As String = CurrentTerritoryList.Trim("|").Split("|")
        For Each i As String In CurrentSplittedTerritory
            Dim Territory As String = i
            If SplittedTerritory.Any(Function(b) Territory.ToLower().Equals(b.ToLower())) Then
                'response.write(i & "-" & CardCodeTerritory)
                Accessible = True
                Exit For
            End If
        Next

        Return Accessible
    End Function
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            ' Free other state (managed objects).
            Business_UploadedSQ.FocusDBConn.Close()
            Business_UploadedSQ.SAPDBConn.Close()

        End If
        ' Free your own state (unmanaged objects).

        ' Set large fields to null.
    End Sub
    Protected Sub UpdateSQs()

        Dim OpenSQs As New List(Of String)

        Dim CRMReaderCommand As SqlCommand = New SqlCommand()
        CRMReaderCommand.Connection = Business_UploadedSQ.FocusDBConn
        CRMReaderCommand.CommandText = "SELECT DocNum FROM [SqHeader] WHERE ([DocStatus] IS NULL) "

        Dim CRMreader As SqlDataReader = CRMReaderCommand.ExecuteReader()
        While CRMreader.Read()
            If Not IsDBNull(CRMreader("DocNum")) Then
                OpenSQs.Add(CRMreader("DocNum").ToString())
            End If

        End While

        CRMreader.Close()
        CRMReaderCommand.Dispose()

        Dim ListofDocNums() As String = OpenSQs.ToArray()

        Dim SplittedDocNums As String = String.Join("','", ListofDocNums)
        SplittedDocNums = "'" & SplittedDocNums & "'"
        'Response.Write(SplittedDocNums)



        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_UploadedSQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT DocNum from OQUT " &
                                "WHERE OQUT.DocNum IN (" & SplittedDocNums & ") AND  DocStatus='C' "

        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        Dim ClosedSQs As New List(Of String)

        While reader.Read()
            If Not IsDBNull(reader("DocNum")) Then
                ClosedSQs.Add(reader("DocNum").ToString())
            End If
        End While

        reader.Close()
        ReaderCommand.Dispose()

        Dim ListofClosedDocNums() As String = ClosedSQs.ToArray()

        Dim ClosedDocNums As String = String.Join("','", ListofClosedDocNums)
        ClosedDocNums = "'" & ClosedDocNums & "'"
        'Response.Write(ClosedDocNums)

        Dim UpdateCRMReaderCommand As SqlCommand = New SqlCommand()
        UpdateCRMReaderCommand.Connection = Business_UploadedSQ.FocusDBConn
        UpdateCRMReaderCommand.CommandText = "UPDATE [SqHeader] SET [DocStatus]='Closed'  WHERE [DocNum] in (" & ClosedDocNums & ") "
        UpdateCRMReaderCommand.ExecuteNonQuery()

        UpdateCRMReaderCommand.Dispose()

    End Sub
    Protected Sub ValidateBPName(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtBPNameValidator.ServerValidate


        Dim Prof As ProfileCommon = ProfileBase.Create(User.Identity.Name, True)
        Dim Terittory As String = Prof.Territory
        Terittory = Terittory.Replace("|", ",").TrimEnd(",")

        Dim sLookUP As String = String.Empty
        Dim SAPConn As New SqlConnection()
        sLookUP = args.Value.ToString()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT CardCode FROM OCRD " &
                                "WHERE OCRD.CardType='C' AND OCRD.CardName=@LookUP AND OCRD.Territory in (" & Terittory & ") ", SAPConn)
        SAPConn.Open()

        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()
        If reader.Read() Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If

        SAPConn.Close()
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
    Protected Sub GridView2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView2.RowDataBound
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
        If GridView1.SelectedIndex >= 0 Then
            SqlDataSource2.SelectParameters.Item("DocNum").DefaultValue = GridView1.Rows(GridView1.SelectedIndex).Cells(1).Text
            SqlDataSource2.DataBind()
            GridView2.HeaderRow.HorizontalAlign = HorizontalAlign.Left
        Else
            SqlDataSource2.SelectParameters.Item("DocNum").DefaultValue = ""
            SqlDataSource2.DataBind()

        End If
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        GridView1.SelectedIndex = -1

        If txtBPName.Text <> String.Empty Then
            SqlDataSource1.SelectParameters("CardName").DefaultValue = txtBPName.Text
            SqlDataSource1.DataBind()
        Else
            SqlDataSource1.SelectParameters("CardName").DefaultValue = "%"
            SqlDataSource1.DataBind()
        End If

        SqlDataSource2.SelectParameters.Item("DocNum").DefaultValue = ""
        SqlDataSource2.DataBind()
    End Sub
End Class

