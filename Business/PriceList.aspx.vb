Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Text.RegularExpressions
Partial Class Business_PriceList
    Inherits System.Web.UI.Page
    Public Shared FocusDBConn As New SqlConnection
    Public Shared SAPDBConn As New SqlConnection
    Protected Sub ValidateBPName(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtBPNameValidator.ServerValidate


        Dim Prof As ProfileCommon = ProfileBase.Create(User.Identity.Name, True)
        Dim Terittory As String = Prof.Territory
        Terittory = Terittory.Replace("|", ",").TrimEnd(",")

        Dim sLookUP As String = String.Empty
        Dim SAPConn As New SqlConnection()
        sLookUP = args.Value.ToString()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT CardCode FROM OCRD " & _
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
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid Then
            If txtBPName.Text.ToString = String.Empty Then
                SqlDataSource1.SelectParameters.Item("CardName").DefaultValue = "%"
            Else
                SqlDataSource1.SelectParameters.Item("CardName").DefaultValue = txtBPName.Text
            End If
            Dim ReaderCommand As SqlCommand = New SqlCommand()
            ReaderCommand.Connection = Business_PriceList.SAPDBConn
            ReaderCommand.CommandText = "SELECT ItemCode from OITM " & _
                                    "WHERE ItemName=@ItemCode"
            Dim SqlPara As New SqlParameter
            SqlPara.ParameterName = "@ItemCode"
            SqlPara.Value = txtItemName.Text
            ReaderCommand.Parameters.Add(SqlPara)

            Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
            If reader.read() Then          
                SqlDataSource1.SelectParameters.Item("ItemCode").DefaultValue = reader("ItemCode").ToString()
            Else
                SqlDataSource1.SelectParameters.Item("ItemCode").DefaultValue = String.empty
            End If
            reader.close()
            ReaderCommand.dispose()
            SqlDataSource1.SelectParameters.Item("Territory").DefaultValue = ddlTerritory.SelectedItem.Value
            SqlDataSource1.DataBind()
            GridView1.HeaderRow.HorizontalAlign = HorizontalAlign.Left
            GridView1.SelectedIndex = -1
            GridView2.SelectedIndex = -1
        End If
      
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load    
        If Business_PriceList.FocusDBConn.State = 0 Then
            Business_PriceList.FocusDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
            Business_PriceList.FocusDBConn.Open()
        End If
        If Business_PriceList.SAPDBConn.State = 0 Then
            Business_PriceList.SAPDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            Business_PriceList.SAPDBConn.Open()
        End If
        If Not IsPostBack Then
            BindTerritories()
        End If
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
                If i = 2 Then
                    If e.Row.Cells(i).Text = "23" Then
                        e.Row.Cells(i).Text = "SQ"
                    End If
                    If e.Row.Cells(i).Text = "17" Then
                        e.Row.Cells(i).Text = "SO"
                    End If
                End If
            Next
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub
    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged, GridView1.DataBound
        If GridView1.SelectedIndex >= 0 Then

            If GridView1.Rows(GridView1.SelectedIndex).Cells(2).Text = "SQ" Then

                SqlDataSource2.SelectCommand = "select OQUT.DocNum AS 'Document #',QUT1.Dscription as 'Model',QUT1.Quantity,QUT1.Price AS 'Cust Price', QUT1.U_PRICE1 AS 'N.I Price', QUT1.DiscPrcnt AS 'Discount',(QUT1.Price-((QUT1.Price*QUT1.DiscPrcnt)/100)) as 'Price After Discount'  from  OQUT INNER JOIN QUT1 ON QUT1.DocEntry=OQUT.DocEntry WHERE OQUT.DocNum=@DocNum"
            ElseIf GridView1.Rows(GridView1.SelectedIndex).Cells(2).Text = "SO" Then
                SqlDataSource2.SelectCommand = "select ORDR.DocNum AS 'Document #',RDR1.Dscription as 'Model',RDR1.Quantity,RDR1.Price AS 'Cust Price', RDR1.U_PRICE1 AS 'N.I Price', RDR1.DiscPrcnt AS 'Discount',(RDR1.Price-((RDR1.Price*RDR1.DiscPrcnt)/100)) as 'Price After Discount'  from  ORDR INNER JOIN RDR1 ON RDR1.DocEntry=ORDR.DocEntry WHERE ORDR.DocNum=@DocNum"               
            End If
            SqlDataSource2.SelectParameters.Item("DocNum").DefaultValue = GridView1.Rows(GridView1.SelectedIndex).Cells(1).Text
            SqlDataSource2.DataBind()
            GridView2.HeaderRow.HorizontalAlign = HorizontalAlign.Left
        Else
            SqlDataSource2.SelectParameters.Item("DocNum").DefaultValue = ""
            SqlDataSource2.DataBind()

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
    Protected Sub BindTerritories()
        Dim Prof As ProfileCommon = ProfileBase.Create(User.Identity.Name, True)
        Dim Terittory As String = Prof.Territory
        Terittory = Terittory.Replace("|", ",").TrimEnd(",")
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_PriceList.SAPDBConn
        ReaderCommand.CommandText = "SELECT [territryID], [descript] FROM [OTER] " & _
                                "WHERE territryID IN (" & Terittory & ")"
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        While reader.Read
            ddlTerritory.Items.Add(New ListItem(reader("descript").ToString, reader("territryID").ToString))
        End While
        reader.Close()
        ReaderCommand.Dispose()
    End Sub
End Class
