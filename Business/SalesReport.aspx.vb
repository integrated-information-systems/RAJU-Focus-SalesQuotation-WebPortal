Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Drawing

Partial Class Business_SalesReport
    Inherits System.Web.UI.Page
    Shared Sno As Integer = 1
    Shared CommonChance As String = String.Empty

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub
    Private Sub LoadData()
        Dim ReturnResult As New DataTable
        Dim ConnectionStringToUse As String = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ToString
        Dim CustomQueryParameters As New Dictionary(Of String, String)
        Dim Conditionlist1 As New List(Of String)

        If Concluded.SelectedIndex > 0 Then
            Conditionlist1.Add(" ( T1.U_CONCLUDED=@Concluded ) ")
            CustomQueryParameters.Add("@Concluded", Concluded.SelectedItem.Value)
        End If

        If FromDate.Text.ToString <> String.Empty And ToDate.Text.ToString <> String.Empty Then
            Conditionlist1.Add(" ( T1.DocDate>=@FromDocDate AND T1.DocDate<=@ToDocDate ) ")
            CustomQueryParameters.Add("@FromDocDate", FromDate.Text.ToString)
            CustomQueryParameters.Add("@ToDocDate", ToDate.Text.ToString)
        Else
            If FromDate.Text.ToString <> String.Empty Then
                Conditionlist1.Add(" T1.DocDate>=@FromDocDate  ")
                CustomQueryParameters.Add("@FromDocDate", FromDate.Text.ToString)
            End If

            If ToDate.Text.ToString <> String.Empty Then
                Conditionlist1.Add(" T1.DocDate<=@ToDocDate  ")
                CustomQueryParameters.Add("@ToDocDate", ToDate.Text.ToString)
            End If
        End If
        Dim SAPSvr As String = ConfigurationManager.AppSettings("Company_Server")
        Dim SAPDB As String = ConfigurationManager.AppSettings("Company_DB")

        Using SqlCon As New SqlConnection(ConnectionStringToUse)
            SqlCon.Open()
            Dim Qry As String = "Select '' as SNo,T1.DocDate, T1.CardName,CASE WHEN (T1.docnum is not null and T1.id <> T1.docnum) then CONVERT(varchar(10), T1.docnum) else CONVERT(varchar(10), T1.Id) end  as 'QT-Number',  CASE WHEN (T1.DocTotal>0) THEN  T1.DocTotal Else T1.DocTotalFC END as 'Amount', T1.U_CHANCE, T1.U_CONCLUDED, T1.U_QUOTE_REMARKS   from SqHeader T1  WHERE T1.U_CHANCE IN ('High','Low','Medium')  "
            If Conditionlist1.Count > 0 Then
                Dim CondiString1 As String = String.Join(" AND ", Conditionlist1)
                Qry = Qry & " AND " & CondiString1
            End If
            Qry = Qry & " Order By   Case When T1.U_CHANCE ='High' Then 1 When T1.U_CHANCE ='Medium' Then 2 When T1.U_CHANCE ='Low' Then 3 End, T1.id "
            Using cmd As New SqlCommand(Qry)
                cmd.Connection = SqlCon
                'If Not IsNothing(CQ._Parameters) Then
                Dim Params = CustomQueryParameters
                For Each iKey As String In CustomQueryParameters.Keys
                    Dim ParamValue As String = Params(iKey)
                    cmd.Parameters.AddWithValue(iKey, ParamValue)
                Next
                'End If

                Dim SqlAdap As New SqlDataAdapter(cmd)
                SqlAdap.Fill(ReturnResult)
                SqlAdap.Dispose()
            End Using

        End Using
        'Dim DNewRow As DataRow = ReturnResult.NewRow
        'DNewRow("SNo") = "High"
        'ReturnResult.Rows.InsertAt(DNewRow, 0)
        'Dim HighTotal = ReturnResult.Compute("Sum(Amount)", " U_CHANCE='Low' ")


        Dim dictionary As New Dictionary(Of String, Integer)
        Dim HighRCount = ReturnResult.Compute("Count(Amount)", " U_CHANCE='High' ")
        Dim MediumRCount = ReturnResult.Compute("Count(Amount)", " U_CHANCE='Medium' ")
        Dim LowRCount = ReturnResult.Compute("Count(Amount)", " U_CHANCE='Low' ")
        Dim CurrentIndex As Integer = 0
        Dim HighSubTotal As Decimal = 0
        If HighRCount > 0 Then
            Dim DNewRow As DataRow = ReturnResult.NewRow
            Dim DNewRowChance As DataRow = ReturnResult.NewRow
            DNewRow("SNo") = "High"
            ReturnResult.Rows.InsertAt(DNewRow, CurrentIndex)
            CurrentIndex = CurrentIndex + 1

            Dim i As Integer = 1
            Dim j As Integer = 0
            For Each ro As DataRow In ReturnResult.Rows
                If j >= CurrentIndex And i <= HighRCount Then
                    ro("SNo") = i
                    i = i + 1
                End If
                j = j + 1
            Next

            HighSubTotal = ReturnResult.Compute("Sum(Amount)", " U_CHANCE='High' ")
            DNewRowChance("Amount") = HighSubTotal
            DNewRowChance("QT-Number") = "Subtotal"
            CurrentIndex = HighRCount + CurrentIndex
            ReturnResult.Rows.InsertAt(DNewRowChance, CurrentIndex)

        End If


        If CurrentIndex > 0 And MediumRCount > 0 Then
            CurrentIndex = CurrentIndex + 1
        End If

        Dim MediumSubTotal As Decimal = 0
        If MediumRCount > 0 Then
            Dim DNewRow As DataRow = ReturnResult.NewRow
            Dim DNewRowChance As DataRow = ReturnResult.NewRow
            DNewRow("SNo") = "Medium"
            ReturnResult.Rows.InsertAt(DNewRow, CurrentIndex)
            CurrentIndex = CurrentIndex + 1

            Dim i As Integer = 1
            Dim j As Integer = 0
            For Each ro As DataRow In ReturnResult.Rows
                If j >= CurrentIndex And i <= MediumRCount Then
                    ro("SNo") = i
                    i = i + 1
                End If
                j = j + 1
            Next

            MediumSubTotal = ReturnResult.Compute("Sum(Amount)", " U_CHANCE='Medium' ")
            DNewRowChance("Amount") = MediumSubTotal

            DNewRowChance("QT-Number") = "Subtotal"
            CurrentIndex = MediumRCount + CurrentIndex
            ReturnResult.Rows.InsertAt(DNewRowChance, CurrentIndex)

        Else

        End If
        If CurrentIndex > 0 And LowRCount > 0 Then
            CurrentIndex = CurrentIndex + 1
        End If



        Dim LowSubTotal As Decimal = 0
        If LowRCount > 0 Then
            Dim DNewRow As DataRow = ReturnResult.NewRow
            Dim DNewRowChance As DataRow = ReturnResult.NewRow
            DNewRow("SNo") = "Low"
            ReturnResult.Rows.InsertAt(DNewRow, CurrentIndex)
            CurrentIndex = CurrentIndex + 1

            Dim i As Integer = 1
            Dim j As Integer = 0
            For Each ro As DataRow In ReturnResult.Rows
                If j >= CurrentIndex And i <= LowRCount Then
                    ro("SNo") = i
                    i = i + 1
                End If
                j = j + 1
            Next

            LowSubTotal = ReturnResult.Compute("Sum(Amount)", " U_CHANCE='Low' ")
            DNewRowChance("Amount") = LowSubTotal
            DNewRowChance("QT-Number") = "Subtotal"
            CurrentIndex = LowRCount + CurrentIndex
            ReturnResult.Rows.InsertAt(DNewRowChance, CurrentIndex)
        End If

        Dim DNewRowGrandTotal As DataRow = ReturnResult.NewRow
        CurrentIndex = CurrentIndex + 1
       
        DNewRowGrandTotal("Amount") = (LowSubTotal + MediumSubTotal + HighSubTotal)
        DNewRowGrandTotal("QT-Number") = "Grand Total"
        ReturnResult.Rows.InsertAt(DNewRowGrandTotal, CurrentIndex)




        GridView1.DataSource = ReturnResult
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                If CommonChance <> e.Row.Cells("4").ToString Then

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    '************************Export To Excel-Starts Here*************************
    Protected Sub ExportToExcel(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
            Response.Charset = ""
            Response.ContentType = "application/vnd.ms-excel"

            Using sw As New StringWriter()

                Dim hw As New HtmlTextWriter(sw)

                ' // Hide the Unwanted Values To avoid exeption when export to excel

                GridView1.HeaderRow.BackColor = Color.White

                For Each cell As TableCell In GridView1.HeaderRow.Cells
                    cell.BackColor = GridView1.HeaderStyle.BackColor
                Next

                For Each row As GridViewRow In GridView1.Rows
                    row.BackColor = Color.White
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = GridView1.AlternatingRowStyle.BackColor
                        Else
                            cell.BackColor = GridView1.RowStyle.BackColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                GridView1.RenderControl(hw)
                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)
                Response.Output.Write(sw.ToString())
                Response.Flush()
                Response.[End]()



                
            End Using
        Catch ex As Exception

        End Try
    End Sub
    'This function is mandartory For Export to Excel to Work properly
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered
    End Sub
    '************************Export To Excel-Ends Here***************************

    Protected Sub btnExportExcel_Click(sender As Object, e As System.EventArgs) Handles btnExportExcel.Click
        Try
            'ItemBudgetLinesGrid.ShowFooter = False
            ExportToExcel(sender, e)
            'ItemBudgetLinesGrid.ShowFooter = True
        Catch ex As Exception

        End Try
    End Sub
End Class
