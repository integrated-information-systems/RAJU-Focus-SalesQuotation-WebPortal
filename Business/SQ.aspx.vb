Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Diagnostics

Partial Class Business_SQ
    Inherits System.Web.UI.Page
    Public Shared FocusDBConn As New SqlConnection
    Public Shared SAPDBConn As New SqlConnection

    Protected Sub txtBPCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBPCode.TextChanged
        Dim LookUp As String = Request("__EVENTARGUMENT").ToString().Trim()
        BindContactPerson(LookUp)
        BindBPContactPersonDetails()
        BindBPCurrency(LookUp)
        BindBPContactIDs(LookUp)
        SetTaxCode()
        SetPaymentTerm()
        ddlPriceCurrency.SelectedIndex = ddlPriceCurrency.Items.IndexOf(ddlPriceCurrency.Items.FindByValue(BC.Value))
        BindAssignTo()
    End Sub

    Protected Sub txtBPName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBPName.TextChanged
        'MsgBox(Request("__EVENTARGUMENT"))
        Dim LookUp As String = Request("__EVENTARGUMENT").ToString().Trim().Replace("'", "")
        'BindContactPerson(LookUp)
        'BindBPCurrency(LookUp)
        'BindBPContactIDs(LookUp)
    End Sub
    Protected Sub SetTaxCode()
        'Dim Lookup As String = String.Empty
        'Lookup = txtBPCode.Text
        'Dim ReaderCommand As SqlCommand = New SqlCommand()
        'ReaderCommand.Connection = Business_SQ.SAPDBConn
        'ReaderCommand.CommandText = "SELECT ecvatgroup from OCRD " & _
        '                        "WHERE OCRD.CardCode=@LookUP"
        ''ReaderCommand.Parameters.AddWithValue("@LookUP", Lookup)
        'Dim SqlPara As New SqlParameter
        'SqlPara.ParameterName = "@LookUp"
        'SqlPara.Value = Lookup
        'ReaderCommand.Parameters.Add(SqlPara)

        'Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        'If reader.Read Then
        '    ddlTaxCode.SelectedIndex = ddlTaxCode.Items.IndexOf(ddlTaxCode.Items.FindByText(reader("ecvatgroup").ToString))
        'End If
        'reader.Close()
        'ReaderCommand.Dispose()
        ddlTaxCode.SelectedIndex = ddlTaxCode.Items.IndexOf(ddlTaxCode.Items.FindByText("ZR-ZERO RATED OUTPUT TAX"))
    End Sub
    Protected Sub SetPaymentTerm()
        Dim Lookup As String = String.Empty
        Lookup = txtBPCode.Text
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT GroupNum from OCRD " & _
                                "WHERE OCRD.CardCode=@LookUP"
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = Lookup
        ReaderCommand.Parameters.Add(SqlPara)
        'ReaderCommand.Parameters.AddWithValue("@LookUP", Lookup)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        If reader.Read Then
            ddlPaymentterm.SelectedIndex = ddlPaymentterm.Items.IndexOf(ddlPaymentterm.Items.FindByValue(reader("GroupNum").ToString))
        End If
        reader.Close()
        ReaderCommand.Dispose()
    End Sub
    Protected Sub BindBPContactIDs(ByVal LookUp As String)
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = " SELECT CRD1.Address,CRD1.Street, CRD1.StreetNo,CRD1.Block, CRD1.ZipCode, CRD1.City, CRD1.County FROM CRD1 INNER JOIN OCRD ON OCRD.CardCode=CRD1.CardCode " & _
                                "WHERE CRD1.AdresType='B' AND  ( OCRD.CardCode=@LookUP OR   OCRD.CardName=@LookUP)"
        'ReaderCommand.Parameters.AddWithValue("@LookUP", LookUp)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = LookUp
        ReaderCommand.Parameters.Add(SqlPara)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        LstAddressId.Items.Clear()

        While reader.Read()
            Dim AddressDetails As String = String.Empty
            If reader("Street").ToString <> String.Empty Then
                AddressDetails = AddressDetails & reader("Street").ToString & vbTab
            End If
            If reader("Block").ToString <> String.Empty Then
                AddressDetails = AddressDetails & reader("Block").ToString & vbTab
            End If
            If reader("City").ToString <> String.Empty Then
                AddressDetails = AddressDetails & reader("City").ToString & vbTab
            End If
            If reader("County").ToString <> String.Empty Then
                AddressDetails = AddressDetails & reader("County").ToString & vbTab
            End If
            If reader("ZipCode").ToString <> String.Empty Then
                AddressDetails = AddressDetails & reader("StreetNo").ToString
            End If
            LstAddressId.Items.Add(New ListItem(reader("Address").ToString(), AddressDetails))
        End While
        If LstAddressId.SelectedIndex = -1 And LstAddressId.Items.Count > 0 Then
            LstAddressId.SelectedIndex = 0
        End If

        BindAddressDetails()
        reader.Close()
        ReaderCommand.Dispose()
    End Sub
    Protected Sub BindContactPerson(ByVal LookUp As String)
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT ocpr.CntctCode,ocpr.Name from OCRD INNER JOIN OCPR ON OCPR.CardCode= OCRD.CardCode " & _
                                "WHERE OCRD.CardCode=@LookUP OR   OCRD.CardName=@LookUP"
        'ReaderCommand.Parameters.AddWithValue("@LookUP", LookUp)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = LookUp
        ReaderCommand.Parameters.Add(SqlPara)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        LstCtPrsn.Items.Clear()

        While reader.Read()
            LstCtPrsn.Items.Add(New ListItem(reader("Name").ToString(), reader("CntctCode").ToString()))
        End While
        If LstCtPrsn.SelectedIndex = -1 And LstCtPrsn.Items.Count > 0 Then
            LstCtPrsn.SelectedIndex = 0
        End If
        reader.Close()
        ReaderCommand.Dispose()
    End Sub
    Protected Sub BindBPContactPersonDetails()
        Dim Lookup As String = String.Empty
        If LstCtPrsn.SelectedIndex <> -1 Then
            Lookup = LstCtPrsn.SelectedItem.Value
        End If

        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT ocpr.Tel1,ocpr.Fax, OCPR.Cellolar from OCPR  " & _
                                "WHERE OCPR.CntctCode=@LookUP "
        'ReaderCommand.Parameters.AddWithValue("@LookUP", Lookup)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = Lookup
        ReaderCommand.Parameters.Add(SqlPara)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()

        Dim ContactPersonDetails As String = String.Empty
        If reader.Read() Then
            If reader("Tel1").ToString <> String.Empty Then
                ContactPersonDetails = "Telephone No: " & reader("Tel1").ToString
            End If
            If reader("Fax").ToString <> String.Empty Then
                ContactPersonDetails = ContactPersonDetails & "<br/>" & "Fax: " & reader("Fax").ToString
            End If
            If reader("Cellolar").ToString <> String.Empty Then
                ContactPersonDetails = ContactPersonDetails & "<br/>" & "Mobile : " & reader("Cellolar").ToString
            End If
            lblContactDetails.Text = String.Empty
            lblContactDetails.Text = ContactPersonDetails
        End If

        reader.Close()
        ReaderCommand.Dispose()
    End Sub
    Protected Sub BindAddressDetails()
        Dim AddressDetails As String = String.Empty
        If LstAddressId.SelectedIndex <> -1 Then
            AddressDetails = LstAddressId.SelectedItem.Value.ToString().Replace(vbTab, "<br/>")
        End If
        lblAddressDetails.Text = String.Empty
        lblAddressDetails.Text = AddressDetails
    End Sub
    Protected Sub GetLastDocNo()
        'Dim ReaderCommand As SqlCommand = New SqlCommand()
        'ReaderCommand.Connection = Business_SQ.SAPDBConn
        'ReaderCommand.CommandText = "select MAX(docNum)+1 as DocNo from OQUT"
        'Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()

        'If reader.Read() Then
        '    lblDocNo.Text = reader("DocNo").ToString()
        'End If
        'reader.Close()
        'ReaderCommand.Dispose()

        Dim ReaderCommandInternal As SqlCommand = New SqlCommand()
        ReaderCommandInternal.Connection = Business_SQ.FocusDBConn
        ReaderCommandInternal.CommandText = "select MAX(id)+1 as DocNo from SqHeader"
        Dim Internalreader As SqlDataReader = ReaderCommandInternal.ExecuteReader()
        If Internalreader.Read() Then
            LblIntDocNo.Text = Internalreader("DocNo").ToString()
            'lblDocNo.Text = Internalreader("DocNo").ToString()
        End If

        Internalreader.Close()
        ReaderCommandInternal.Dispose()

    End Sub
    Protected Sub BindBPCurrency(ByVal LookUp As String)

        Dim ReaderCommand As SqlCommand = New SqlCommand()
        Dim TempListItem As ListItem = New ListItem()
        Dim BPCurrency As String = String.Empty
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT Currency from OCRD  " & _
            "WHERE OCRD.CardCode=@LookUP OR   OCRD.CardName=@LookUP"

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = LookUp
        ReaderCommand.Parameters.Add(SqlPara)

        'ReaderCommand.Parameters.AddWithValue("@LookUP", LookUp)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()


        If reader.Read() Then
            BPCurrency = reader("Currency").ToString
            If reader("Currency").ToString() = "##" Then
                reader.Close()
                ReaderCommand.Dispose()
                Dim ReaderCommandInternal As SqlCommand = New SqlCommand()
                ReaderCommandInternal.Connection = Business_SQ.SAPDBConn
                ReaderCommandInternal.CommandText = "SELECT CurrCode from OCRN"

                Dim readernternal As SqlDataReader = ReaderCommandInternal.ExecuteReader()
                ddlDocCurrency.Items.Clear()
                While readernternal.Read()
                    ddlDocCurrency.Items.Add(New ListItem(readernternal("CurrCode").ToString(), readernternal("CurrCode").ToString()))
                End While
                ddlDocCurrency.ClearSelection()
                ddlDocCurrency.SelectedIndex = ddlDocCurrency.Items.IndexOf(ddlDocCurrency.Items.FindByValue(LC.Value))
                BC.Value = LC.Value


                readernternal.Close()
                ReaderCommandInternal.Dispose()

            Else

                ddlDocCurrency.Items.Clear()
                ddlDocCurrency.Items.Add(BPCurrency)
                ddlDocCurrency.SelectedIndex = ddlDocCurrency.Items.IndexOf(ddlDocCurrency.Items.FindByValue(BPCurrency))
                BC.Value = BPCurrency



                reader.Close()
                ReaderCommand.Dispose()
            End If

        End If
        reader.Close()
        ReaderCommand.Dispose()
        LoadExRate(LC.Value, BC.Value)

    End Sub
    Protected Function GetExRate(ByVal BaseCurrency As String, ByVal CompareCurrency As String) As String
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        Dim TempListItem As ListItem = New ListItem()
        Dim BPCurrency As String = String.Empty
        Dim CurrencyLookup As String = String.Empty
        Dim RateDateLookup As String = String.Empty
        Dim RateDate As String = String.Empty
        CurrencyLookup = CompareCurrency
        RateDateLookup = txtPostingDate.Text
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT Rate from ORTT  " & _
            "WHERE ORTT.Currency=@CurrencyLookup AND   ORTT.RateDate=@RateDateLookup"
        ReaderCommand.Parameters.AddWithValue("@CurrencyLookup", CurrencyLookup)
        ReaderCommand.Parameters.AddWithValue("@RateDateLookup", RateDateLookup)

        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()

        If reader.Read() Then
            RateDate = reader("Rate").ToString()
        End If

        reader.Close()
        ReaderCommand.Dispose()
        If RateDate = String.Empty Then
            Return String.Empty
        Else
            Return RateDate
        End If
    End Function
    Protected Sub LoadExRate(ByVal BaseCurrency As String, ByVal CompareCurrency As String)
        'If LC.Value <> BC.Value Then
        If BaseCurrency <> CompareCurrency Then
            Dim ExRate As String = GetExRate(BaseCurrency, CompareCurrency)
            If (ExRate <> String.Empty) Then
                txtCurExRate.Text = ExRate
                'lblCurExRate.Visible = True
                'txtCurExRate.Visible = True
            Else
                txtCurExRate.Text = ""
            End If
        Else
            'lblCurExRate.Visible = False
            'txtCurExRate.Visible = False
            txtCurExRate.Text = 1
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If User.Identity.Name.ToLower <> "iis" Then
        '    btnSubmit.Visible = False
        'End If

        Dim RequestStatus As String = Request.QueryString("status")

        If RequestStatus = "1" Then

            'LblHeaderMsg.Text = String.Format("Sales Quotation, Doc No ""{0}"" has been Created/Updated Successfully", Request.QueryString("id"))
            LblHeaderMsg.Text = String.Format("{0}", Request.QueryString("id"))
            LblHeaderMsg.ForeColor = Color.Green

        ElseIf RequestStatus = "0" Then
            LblHeaderMsg.Text = String.Format("{0}", Request.QueryString("id"))
            LblHeaderMsg.ForeColor = Color.Red
        Else
            LblHeaderMsg.Text = String.Empty
        End If

        'If IsPostBack Then
        '    Validate()
        'End If
        If Business_SQ.FocusDBConn.State = 0 Then
            Business_SQ.FocusDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
            Business_SQ.FocusDBConn.Open()
        End If
        If Business_SQ.SAPDBConn.State = 0 Then
            Business_SQ.SAPDBConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            Business_SQ.SAPDBConn.Open()
        End If
        GetLastDocNo()
        If Not Page.IsPostBack Then
            BindCurrencySelector()
            InitializeFormDate()
            InitializeTempDataTable()
            LoadTaxCode()
            LoadWarhouseCode()
            LoadStockandPrint()
            'BindAssignTo()
        Else

        End If
    End Sub
    Public Overloads Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            ' Free other state (managed objects).
            Business_SQ.FocusDBConn.Close()
            Business_SQ.SAPDBConn.Close()
            ViewState("TempDataset").Dispose()
            ViewState("TempDataTable").Dispose()
        End If
        ' Free your own state (unmanaged objects).

        ' Set large fields to null.
    End Sub
    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub
    Protected Function AccessibleDoc(ByVal BPCode As String) As Boolean
        Dim Accessible As Boolean = False


        Dim ReaderCommand As SqlCommand = New SqlCommand()
        Dim CardCodeTerritory As String = String.Empty
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT  OCRD.Territory from OCRD  " &
                                "WHERE OCRD.CardCode=@LookUP"

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
    Protected Sub BindAssignTo()
        ddlAssignTo.Items.Clear()
        ddlAssignTo.Items.Add(New ListItem("Select", ""))
        Dim DataTable As MembershipUserCollection = Membership.GetAllUsers()
        Dim Row As MembershipUser
        'Dim LoggedInUserProfile As ProfileCommon = Profile.GetProfile(Membership.GetUser().UserName)
        'Dim LoggedInUser As MembershipUser = Membership.GetUser(Membership.GetUser().UserName)
        'Dim TerritoryList As String = LoggedInUserProfile.Territory()
        'Dim SplittedTerritory() As String = TerritoryList.Trim.Trim("|").Split("|")
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        Dim CardCodeTerritory As String = String.Empty
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT  OCRD.Territory from OCRD  " &
                                "WHERE OCRD.CardCode=@LookUP"
        'ReaderCommand.Parameters.AddWithValue("@LookUP", LookUp)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = txtBPCode.Text
        ReaderCommand.Parameters.Add(SqlPara)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        If reader.Read() Then
            CardCodeTerritory = reader("Territory").ToString()
        End If
        reader.Close()
        ReaderCommand.Dispose()


        Dim SplittedTerritory() As String = {CardCodeTerritory}
        'Response.Write("<script>alert('" & SplittedTerritory(0) & "');</script>")
        For Each Row In DataTable

            Dim CurrentUserProfile As ProfileCommon = Profile.GetProfile(Row.UserName)
            Dim CurrentTerritoryList As String = CurrentUserProfile.Territory()
            Dim CurrentSplittedTerritory() As String = CurrentTerritoryList.Trim("|").Split("|")
            For Each i As String In CurrentSplittedTerritory

                If SplittedTerritory.Contains(i) And Membership.GetUser().UserName <> Row.UserName Then
                    ddlAssignTo.Items.Add(New ListItem(Row.UserName))
                    Exit For
                End If

            Next

        Next

    End Sub
    Protected Sub BindCurrencySelector()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "select MainCurncy,SysCurrncy from OADM"
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        LstCurrencySelector.Items.Clear()

        If reader.Read() Then

            LC.Value = reader("MainCurncy").ToString()
            SC.Value = reader("SysCurrncy").ToString()
            LstCurrencySelector.Items.Add(New ListItem(("BP Currency"), ""))
            ddlDocCurrency.Items.Add(LC.Value)

        End If
        reader.Close()
        ReaderCommand.Dispose()
    End Sub
    Protected Sub LstCurrencySelector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstCurrencySelector.SelectedIndexChanged
        Dim LookUp As String = String.Empty
        'txtDocCurrency.Text = LstCurrencySelector.SelectedItem.Value
        If ddlDocCurrency.Items.Count > 0 Then
            ddlDocCurrency.Items.Clear()
        End If
        ddlDocCurrency.Items.Add(LstCurrencySelector.SelectedItem.Value)
        ddlDocCurrency.SelectedIndex = 0
        If LstCurrencySelector.SelectedItem.Text = "BP Currency" Then
            LookUp = txtBPCode.Text
            BindBPCurrency(LookUp)
        End If
    End Sub
    Protected Sub LoadWarhouseCode()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "select WhsCode,WhsName from OWHS"
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        ddlWarehouse.Items.Clear()
        While reader.Read()
            ddlWarehouse.Items.Add(New ListItem(reader("WhsName").ToString(), reader("WhsCode").ToString()))
        End While
        reader.Close()
        ReaderCommand.Dispose()

        Dim ReaderCommandWhse As SqlCommand = New SqlCommand()
        ' Get default warehouse and set it as default
        ReaderCommandWhse.Connection = Business_SQ.SAPDBConn
        ReaderCommandWhse.CommandText = "select DfltWhs from oadm"

        Dim readerWhse As SqlDataReader = ReaderCommandWhse.ExecuteReader()
        If readerWhse.Read() Then
            'ddlWarehouse.SelectedItem.Value = readerWhse("DfltWhs").ToString
            ddlWarehouse.SelectedIndex = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(readerWhse("DfltWhs").ToString))
        End If
        readerWhse.Close()
        ReaderCommandWhse.Dispose()
    End Sub
    Protected Sub LoadTaxCode()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT Code, Name,Rate from OVTG where category='O'"
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        ddlTaxCode.Items.Clear()
        While reader.Read()
            ddlTaxCode.Items.Add(New ListItem(reader("Code").ToString() & "-" & reader("Name").ToString(), reader("Code").ToString() & "-" & reader("Rate").ToString()))
        End While
        reader.Close()
        ReaderCommand.Dispose()
    End Sub
    Protected Sub InitializeFormDate()
        txtPostingDate.Text = Now.Date.ToString("yyyy-MM-dd")
        txtValidUntil.Text = Now.Date.ToString("yyyy-MM-dd")
        txtDocDate.Text = Now.Date.ToString("yyyy-MM-dd")
    End Sub
    Protected Sub InitializeTempDataTable()
        Dim TempDataset As New DataSet
        Dim TempDataTable As New DataTable
        Dim SelectedRow As Integer = -1
        Dim HavingChildItems As Boolean = False
        ViewState("TempDataset") = TempDataset
        ViewState("TempDataTable") = TempDataTable
        ViewState("SelectedRow") = SelectedRow
        If Not IsNothing(ViewState("TempDataTable")) Then
            ViewState("TempDataTable").Rows.Clear()
        End If
        Dim SqlAdapter As _
           New SqlDataAdapter("Select [SNo], [ItemCode] ,[ItemDescription],[U_Item_Remarks] ,[U_LineText],[U_Country]  ,[UnitPrice], [U_Price1]  ,[Quantity]  ,[Discount]  ,[TaxCode], [Tax] ,[LineTotal],[WarehouseCode],[U_Stock],[U_Print],[IsChild],[LineStatus],[PriceBefDisc], [LineTotalLC], [TotalFrgn] From SqLineItems", FocusDBConn)

        SqlAdapter.FillSchema(ViewState("TempDataset"), SchemaType.Source, "SqLineItems")
        ViewState("TempDataTable") = ViewState("TempDataset").Tables("SqLineItems")

        GridView1.DataSource = ViewState("TempDataTable")
        GridView1.DataBind()
        SqlAdapter.Dispose()
        ModifyGridViewHeader()

    End Sub
    Protected Sub ModifyGridViewHeader()
        GridView1.HeaderRow.Cells(2).Text = "Item Code"
        GridView1.HeaderRow.Cells(3).Text = "Model"
        GridView1.HeaderRow.Cells(4).Text = "Description"
        GridView1.HeaderRow.Cells(5).Text = "LineText"
        GridView1.HeaderRow.Cells(6).Text = "Country"
        GridView1.HeaderRow.Cells(7).Text = "Cust.Price"
        GridView1.HeaderRow.Cells(8).Text = "N.I.Price"
        GridView1.HeaderRow.Cells(12).Text = "Tax"
        GridView1.HeaderRow.Cells(13).Text = "Total" & "(" & ddlDocCurrency.SelectedItem.Value & ")"
        GridView1.HeaderRow.Cells(14).Text = "WhseCode"
        GridView1.HeaderRow.Cells(15).Text = "Stock"
        GridView1.HeaderRow.Cells(16).Text = "Print"

        GridView1.HeaderRow.HorizontalAlign = HorizontalAlign.Left

    End Sub
    Protected Sub BtnFindDocNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFindDocNo.Click

        LblHeaderMsg.Text = ""
        If txtDocNo.Text <> String.Empty And IsNumeric(txtDocNo.Text.ToString()) Then
            Dim InternalDocId As String = txtDocNo.Text
            LoadSQ(InternalDocId)
            BindAssignTo()
        ElseIf txtSAPDocNo.Text <> String.Empty And IsNumeric(txtSAPDocNo.Text.ToString()) Then
            Dim InternalDocId As String = getInternalId(txtSAPDocNo.Text)
            If InternalDocId <> "0" Then
                LoadSQ(InternalDocId)
                BindAssignTo()
                txtDocNo.Text = InternalDocId
                LblIntDocNo.Text = InternalDocId
            End If
        End If
    End Sub
    Protected Function getInternalId(ByVal DocNum As Integer) As String
        Dim InternalDocId As String = String.Empty
        Dim ReaderCommandHeader As SqlCommand = New SqlCommand()
        ReaderCommandHeader.Connection = Business_SQ.FocusDBConn
        ReaderCommandHeader.CommandText = "SELECT id from SqHeader " &
                                "WHERE SqHeader.DocNum=@LookUP "
        If User.IsInRole("SameTerritoryAccess") Then
            Dim CreatedbyList As List(Of String) = New List(Of String)
            Dim Createdby As String = String.Empty
            Dim DataTable As MembershipUserCollection = Membership.GetAllUsers()
            Dim Row As MembershipUser
            For Each Row In DataTable
                Dim CurrentUserProfile As ProfileCommon = Profile.GetProfile(Row.UserName)
                Dim CurrentTerritoryList As String = CurrentUserProfile.Territory()
                Dim CurrentUserSplittedTerritory() As String = CurrentTerritoryList.Trim("|").Split("|")
                Dim LoggedInUserProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
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
            ReaderCommandHeader.CommandText += "  AND SqHeader.CreatedBy IN (" + Createdby + ")"
        Else
            ReaderCommandHeader.CommandText += "  AND SqHeader.CreatedBy=@CreatedBy"
            ReaderCommandHeader.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        End If

        'ReaderCommandHeader.Parameters.AddWithValue("@LookUP", DocNum)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = DocNum
        ReaderCommandHeader.Parameters.Add(SqlPara)
        'ReaderCommandHeader.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        Dim readerHeader As SqlDataReader = ReaderCommandHeader.ExecuteReader()

        If readerHeader.Read() Then
            InternalDocId = readerHeader("id").ToString
        Else
            InternalDocId = "0"
        End If
        readerHeader.Close()
        Return InternalDocId
    End Function
    Protected Function getSAPId(ByVal HeaderId As Integer) As String
        Dim SAPDocEntry As String = String.Empty
        Dim ReaderCommandHeader As SqlCommand = New SqlCommand()
        ReaderCommandHeader.Connection = Business_SQ.FocusDBConn
        ReaderCommandHeader.CommandText = "SELECT DocEntry from SqHeader " &
                    "WHERE SqHeader.id=@LookUP  "
        If User.IsInRole("SameTerritoryAccess") Then
            Dim CreatedbyList As List(Of String) = New List(Of String)
            Dim Createdby As String = String.Empty
            Dim DataTable As MembershipUserCollection = Membership.GetAllUsers()
            Dim Row As MembershipUser
            For Each Row In DataTable
                Dim CurrentUserProfile As ProfileCommon = Profile.GetProfile(Row.UserName)
                Dim CurrentTerritoryList As String = CurrentUserProfile.Territory()
                Dim CurrentUserSplittedTerritory() As String = CurrentTerritoryList.Trim("|").Split("|")
                Dim LoggedInUserProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
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
            ReaderCommandHeader.CommandText += "  AND SqHeader.CreatedBy IN (" + Createdby + ")"
        Else
            ReaderCommandHeader.CommandText += "  AND SqHeader.CreatedBy=@CreatedBy"
            ReaderCommandHeader.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        End If

        'ReaderCommandHeader.Parameters.AddWithValue("@LookUP", HeaderId)
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = HeaderId
        ReaderCommandHeader.Parameters.Add(SqlPara)
        ReaderCommandHeader.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        Dim readerHeader As SqlDataReader = ReaderCommandHeader.ExecuteReader()


        If readerHeader.Read() Then
            SAPDocEntry = readerHeader("DocEntry").ToString
        Else
            SAPDocEntry = "0"
        End If
        readerHeader.Close()
        Return SAPDocEntry
    End Function
    Private Sub LoadSQ(ByVal InternalDocId As String)
        Dim ReaderCommandHeader As SqlCommand = New SqlCommand()
        ReaderCommandHeader.Connection = Business_SQ.FocusDBConn
        ReaderCommandHeader.CommandText = "SELECT * from SqHeader " &
                        "WHERE SqHeader.id=@LookUP and SqHeader.DocStatus is Null "
        'ReaderCommandHeader.Parameters.AddWithValue("@LookUP", InternalDocId)

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = InternalDocId
        ReaderCommandHeader.Parameters.Add(SqlPara)
        If User.IsInRole("SameTerritoryAccess") Then
            Dim CreatedbyList As List(Of String) = New List(Of String)
            Dim Createdby As String = String.Empty
            Dim DataTable As MembershipUserCollection = Membership.GetAllUsers()
            Dim Row As MembershipUser
            For Each Row In DataTable
                Dim CurrentUserProfile As ProfileCommon = Profile.GetProfile(Row.UserName)
                Dim CurrentTerritoryList As String = CurrentUserProfile.Territory()
                Dim CurrentUserSplittedTerritory() As String = CurrentTerritoryList.Trim("|").Split("|")
                Dim LoggedInUserProfile As ProfileCommon = Profile.GetProfile(User.Identity.Name)
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
            ReaderCommandHeader.CommandText += "  AND SqHeader.CreatedBy IN (" + Createdby + ")"
        Else
            ReaderCommandHeader.CommandText += "  AND SqHeader.CreatedBy=@CreatedBy"
            ReaderCommandHeader.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        End If

        'Response.Write(ReaderCommandHeader.CommandText)

        Dim readerHeader As SqlDataReader = ReaderCommandHeader.ExecuteReader()

        Dim DocEntry As String = String.Empty
        Dim DocStatus As String = String.Empty
        Dim Lookup As String
        If readerHeader.Read() Then
            LblIntDocNo.Text = txtDocNo.Text
            If Not IsDBNull(readerHeader("DocNum")) Then
                lblDocNo.Text = readerHeader("DocNum")
                DocEntry = readerHeader("DocEntry")
                DocStatus = readerHeader("DocStatus").ToString
            Else
                lblDocNo.Text = String.Empty
            End If

            txtBPCode.Text = readerHeader("CardCode")
            Lookup = txtBPCode.Text

            'Added on 15 May 2017 
            If Not IsDBNull(readerHeader("DocNum")) Then
                If readerHeader("id") <> readerHeader("DocNum").ToString Then
                    btnSave.Enabled = False
                    btnSubmit.Enabled = False
                Else
                    btnSave.Enabled = True
                    btnSubmit.Enabled = True
                End If
            Else
                btnSave.Enabled = True
                btnSubmit.Enabled = True
            End If

            Dim NotAccessibleFlag As Boolean = False


            If User.IsInRole("SameTerritoryAccess") Then
                If AccessibleDoc(Lookup) = False Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "There is no Document found" & "'); });", True)
                    ClearFormDocNotFound()
                    NotAccessibleFlag = True
                    'Response.Redirect("~/Business/SQ.aspx?id=There is no Document found&status=0", False)
                End If
            End If
            If NotAccessibleFlag = False Then
                txtBPName.Text = readerHeader("CardName")
                BindContactPerson(Lookup)
                BindBPContactPersonDetails()
                BindBPCurrency(Lookup)
                BindBPContactIDs(Lookup)
                'LstCtPrsn.SelectedItem.Value = readerHeader("ContactPerson")
                If Not IsDBNull(readerHeader("ContactPerson")) Then
                    LstCtPrsn.SelectedIndex = LstCtPrsn.Items.IndexOf(LstCtPrsn.Items.FindByValue(readerHeader("ContactPerson")))
                End If
                If Not IsDBNull(readerHeader("AddressId")) Then
                    LstAddressId.SelectedItem.Text = readerHeader("AddressId")
                End If

                If Not IsDBNull(readerHeader("CustRefNo")) Then
                    txtBPRefNo.Text = readerHeader("CustRefNo")
                End If

                If Not IsDBNull(readerHeader("PostingDate")) Then
                    txtPostingDate.Text = Format(CDate(readerHeader("PostingDate")), "yyyy-MM-dd ")
                End If
                If Not IsDBNull(readerHeader("ValidUntil")) Then
                    txtValidUntil.Text = Format(CDate(readerHeader("ValidUntil")), "yyyy-MM-dd ")
                End If
                If Not IsDBNull(readerHeader("DocDate")) Then
                    txtDocDate.Text = Format(CDate(readerHeader("DocDate")), "yyyy-MM-dd ")
                End If

                'ddlDocCurrency.SelectedItem.Value = readerHeader("DocCurrency")

                If Not IsDBNull(readerHeader("DocCurrency")) Then
                    ddlDocCurrency.SelectedIndex = ddlDocCurrency.Items.IndexOf(ddlDocCurrency.Items.FindByValue(readerHeader("DocCurrency")))
                End If

                BC.Value = ddlDocCurrency.SelectedItem.Value
                LoadExRate(LC.Value, BC.Value)

                If Not IsDBNull(readerHeader("DocRate")) Then
                    txtCurExRate.Text = readerHeader("DocRate")
                End If
                If Not IsDBNull(readerHeader("DocDiscount")) Then
                    txtDocDiscountPercent.Text = readerHeader("DocDiscount")
                End If
                If Not IsDBNull(readerHeader("U_Validity")) Then
                    txtValidForDays.Text = readerHeader("U_Validity").ToString()
                End If
                If Not IsDBNull(readerHeader("U_Delivery_Term")) Then
                    LstDeliveryTerm.SelectedIndex = LstDeliveryTerm.Items.IndexOf(LstDeliveryTerm.Items.FindByValue(readerHeader("U_Delivery_Term").ToString))
                End If
                If Not IsDBNull(readerHeader("U_Shipped_From")) Then
                    LstShipFrom.SelectedIndex = LstShipFrom.Items.IndexOf(LstShipFrom.Items.FindByValue(readerHeader("U_Shipped_From").ToString))
                End If
                If Not IsDBNull(readerHeader("U_Shipped_To")) Then
                    LstShipTo.SelectedIndex = LstShipTo.Items.IndexOf(LstShipTo.Items.FindByValue(readerHeader("U_Shipped_To").ToString))
                End If
                If Not IsDBNull(readerHeader("U_End_User")) Then
                    txtEndUser.Text = readerHeader("U_End_User").ToString
                End If

                If Not IsDBNull(readerHeader("U_Remarks")) Then
                    txtDocRemarks.Text = readerHeader("U_Remarks")
                End If
                If Not IsDBNull(readerHeader("U_Quote_Remarks")) Then
                    txtQuoteRemarks.Text = readerHeader("U_Quote_Remarks")
                End If
                If Not IsDBNull(readerHeader("U_Country_of_Origin")) Then
                    txtCountryofOrigin.Text = readerHeader("U_Country_of_Origin")
                End If

                If Not IsDBNull(readerHeader("U_Revision_No")) Then
                    txtRevisionNo.Text = readerHeader("U_Revision_No")
                End If

                If Not IsDBNull(readerHeader("U_HSCode")) Then
                    txtHSCode.Text = readerHeader("U_HSCode")
                End If

                If Not IsDBNull(readerHeader("U_Min_Charges")) Then
                    LstMinChrge.SelectedIndex = LstMinChrge.Items.IndexOf(LstMinChrge.Items.FindByValue(readerHeader("U_Min_Charges").ToString))
                End If
                If Not IsDBNull(readerHeader("U_Delivery_Date")) Then
                    LstDeliveryTime.SelectedIndex = LstDeliveryTime.Items.IndexOf(LstDeliveryTime.Items.FindByValue(readerHeader("U_Delivery_Date").ToString))
                End If
                If Not IsDBNull(readerHeader("U_Concluded")) Then
                    ddlConcluded.SelectedIndex = ddlConcluded.Items.IndexOf(ddlConcluded.Items.FindByValue(readerHeader("U_Concluded").ToString))
                End If
                If Not IsDBNull(readerHeader("U_Chance")) Then
                    ddlChance.SelectedIndex = ddlChance.Items.IndexOf(ddlChance.Items.FindByValue(readerHeader("U_Chance").ToString))
                End If
                If Not IsDBNull(readerHeader("U_Layout")) Then
                    ddlLayout.SelectedIndex = ddlLayout.Items.IndexOf(ddlLayout.Items.FindByValue(readerHeader("U_Layout").ToString))
                End If
                If Not IsDBNull(readerHeader("PaymentTerm")) Then
                    ddlPaymentterm.SelectedIndex = ddlPaymentterm.Items.IndexOf(ddlPaymentterm.Items.FindByValue(readerHeader("PaymentTerm").ToString))
                End If
            End If

            readerHeader.Close()

            If DocEntry <> String.Empty And DocStatus <> "Closed" Then

                Dim CloseCommand As SqlCommand = New SqlCommand()
                CloseCommand.Connection = Business_SQ.SAPDBConn
                CloseCommand.CommandText = "SELECT DocStatus FROM OQUT " &
                               " WHERE OQUT.DocEntry=@LookUP "
                'ReaderCommandHeader.Parameters.AddWithValue("@LookUP", InternalDocId)

                Dim CloseSqlPara As New SqlParameter
                CloseSqlPara.ParameterName = "@LookUp"
                CloseSqlPara.Value = DocEntry
                CloseCommand.Parameters.Add(CloseSqlPara)

                Dim Closereader As SqlDataReader = CloseCommand.ExecuteReader()
                If Closereader.Read() Then

                    If Closereader("DocStatus").ToString() = "C" Then

                        Closereader.Close()
                        CloseCommand.Dispose()
                        Dim UpdateCommand As SqlCommand = New SqlCommand()
                        UpdateCommand.Connection = Business_SQ.FocusDBConn
                        UpdateCommand.CommandText = "UPDATE SqHeader SET DocStatus=@DocStatus  " &
                                                 "WHERE SqHeader.DocEntry=@DocEntry"
                        UpdateCommand.Parameters.AddWithValue("@DocStatus", "Closed")
                        UpdateCommand.Parameters.AddWithValue("@DocEntry", DocEntry)
                        UpdateCommand.ExecuteNonQuery()
                        UpdateCommand.Dispose()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "There is no Document found" & "'); });", True)
                        ClearFormDocNotFound()
                        'Response.Redirect("~/Business/SQ.aspx?id=There is no Document found&status=0", False)
                        CloseLineStatus(DocEntry, InternalDocId)
                    Else

                        Closereader.Close()
                        CloseCommand.Dispose()
                        CloseLineStatus(DocEntry, InternalDocId)
                    End If
                End If
                Closereader.Close()
                CloseCommand.Dispose()
                CloseLineStatus(DocEntry, InternalDocId)
            ElseIf DocStatus = "Closed" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "There is no Document found" & "'); });", True)
                ClearFormDocNotFound()
                'Response.Redirect("~/Business/SQ.aspx?id=There is no Document found&status=0", False)
            End If

            If NotAccessibleFlag = False Then
                Dim SqlAdapter As _
                       New SqlDataAdapter()
                Dim Sqlcom As SqlCommand = New SqlCommand("Select [SNo], [ItemCode] ,[ItemDescription],[U_Item_Remarks] ,[U_LineText],[U_Country]  ,[UnitPrice], [U_Price1]  ,[Quantity]  ,[Discount]  ,[TaxCode], [Tax] ,[LineTotal],[WarehouseCode],[U_Stock],[U_Print],[IsChild],[LineStatus],[PriceBefDisc], [LineTotalLC], [TotalFrgn] From SqLineItems where HeaderId=@HeaderId  order by CAST([SNo] as int )", Business_SQ.FocusDBConn)
                Sqlcom.Parameters.AddWithValue("@HeaderId", InternalDocId)

                SqlAdapter.SelectCommand = Sqlcom
                Dim dt As New DataTable
                If Not IsNothing(ViewState("TempDataTable")) Then
                    If ViewState("TempDataTable").Rows.Count > 0 Then
                        ViewState("TempDataTable").Rows.Clear()
                    End If
                Else
                    InitializeTempDataTable()
                End If
                SqlAdapter.Fill(dt)
                For Each row As DataRow In dt.Rows
                    Dim DRow As DataRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()
                    DRow("SNo") = row("SNo")
                    DRow("ItemCode") = row("ItemCode")
                    DRow("ItemDescription") = row("ItemDescription")
                    DRow("U_LineText") = row("U_LineText")
                    DRow("U_Item_Remarks") = row("U_Item_Remarks")
                    DRow("UnitPrice") = row("UnitPrice")
                    DRow("U_Price1") = row("U_Price1")
                    DRow("Quantity") = row("Quantity")
                    DRow("Discount") = row("Discount")
                    DRow("U_Country") = row("U_Country")
                    'DRow("U_Group") = row("U_Group")
                    DRow("TaxCode") = row("TaxCode")
                    DRow("U_Print") = row("U_Print")
                    DRow("U_Stock") = row("U_Stock")
                    DRow("Tax") = row("Tax")
                    DRow("WarehouseCode") = row("WarehouseCode")
                    DRow("LineTotal") = row("LineTotal")
                    DRow("IsChild") = row("IsChild")
                    DRow("LineStatus") = row("LineStatus")
                    ViewState("TempDataTable").Rows.Add(DRow.ItemArray)

                Next
                LoadGridData()
            Else
                InitializeTempDataTable()
            End If

        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "There is no Document found" & "'); });", True)
            ClearFormDocNotFound()
            'Response.Redirect("~/Business/SQ.aspx?id=There is no Document found&status=0", False)
        End If

        readerHeader.Close()
        ReaderCommandHeader.Dispose()



    End Sub
    Protected Sub CloseLineStatus(ByVal DocEntry As Integer, ByVal InternalId As Integer)
        Dim CloseLineCommand As SqlCommand = New SqlCommand()
        CloseLineCommand.Connection = Business_SQ.SAPDBConn
        CloseLineCommand.CommandText = "SELECT LineStatus FROM QUT1 " &
                       " WHERE QUT1.DocEntry=@LookUP "
        'ReaderCommandHeader.Parameters.AddWithValue("@LookUP", InternalDocId)

        Dim CloseLineSqlPara As New SqlParameter
        CloseLineSqlPara.ParameterName = "@LookUp"
        CloseLineSqlPara.Value = DocEntry
        CloseLineCommand.Parameters.Add(CloseLineSqlPara)

        Dim CloseLinereader As SqlDataReader = CloseLineCommand.ExecuteReader()
        Dim LineNum As Integer = 1
        While CloseLinereader.Read()
            If CloseLinereader("LineStatus").ToString() = "C" Then
                btnDelete.Enabled = False
                Dim UpdateCommand As SqlCommand = New SqlCommand()
                UpdateCommand.Connection = Business_SQ.FocusDBConn
                UpdateCommand.CommandText = "UPDATE SqLineItems SET LineStatus=@DocStatus  " &
                                         " WHERE SqLineItems.HeaderId=@HeaderId AND SqLineItems.SNo=@SNo "
                UpdateCommand.Parameters.AddWithValue("@DocStatus", "C")
                UpdateCommand.Parameters.AddWithValue("@HeaderId", InternalId)
                UpdateCommand.Parameters.AddWithValue("@SNo", LineNum)
                UpdateCommand.ExecuteNonQuery()
                UpdateCommand.Dispose()
            End If

            LineNum = LineNum + 1
        End While

        CloseLinereader.Close()
        CloseLineCommand.Dispose()

    End Sub
    'Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click

    '    'daAuthors.Fill(dsPubs, "SqHeader")




    '    'Dim a As DataRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()
    '    'a("ItemCode") = "Raja"
    '    'TempDataTable.Rows.Add(a)
    '    'GridView1.DataSource = TempDataTable
    '    'GridView1.DataBind()

    'End Sub
    Protected Function GetPriceCurrency(ByVal Ccode As String, ByVal Icode As String) As String
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        'Dim cardcode As String = txtBPCode.Text.ToString.Replace("'", "''")
        'Dim itemcode As String = txtItemCode.Text.ToString.Replace("'", "''")
        Dim cardcode As String = Ccode
        Dim itemcode As String = Icode
        Dim PriceCurrency As String = String.Empty
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT T1.Currency from ITM1 T1 INNER JOIN OPLN T2 ON T1.Pricelist=T2.ListNum inner join ocrd T3 on T3.listnum=T2.listnum INNER JOIN OITM T4 ON T1.ItemCode=T4.ItemCode " &
                                "where (T3.cardcode=@cardcode OR T3.cardname=@cardcode) AND (T1.ItemCode=@itemcode OR T4.itemname=@itemcode)   ", SAPConn)
        SAPConn.Open()

        SAPComm.Parameters.AddWithValue("@cardcode", cardcode)
        SAPComm.Parameters.AddWithValue("@itemcode", itemcode)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        If reader.Read() Then
            PriceCurrency = reader("Currency").ToString
        End If

        reader.Close()
        SAPConn.Close()
        Return PriceCurrency
    End Function

    Protected Sub btnAction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAction.Click

        Try


            If Page.IsValid Then


                Dim DRow As DataRow
                Dim Rowcount As Integer
                Select Case btnAction.Text
                    Case "Add"
                        DRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()
                        If GridView1.Rows.Count = 0 Then
                            Rowcount = 1
                        Else
                            Rowcount = GridView1.Rows.Count + 1
                        End If
                    Case "Update"
                        'DRow = ViewState("TempDataset").Tables("SqLineItems").Rows(ViewState("SelectedRow"))

                        Dim DT As DataTable = CType(ViewState("TempDataTable"), DataTable)
                        DRow = DT.Rows(ViewState("SelectedRow"))
                        Rowcount = ViewState("SelectedRow") + 1

                End Select
                'Dim DRow As DataRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()
                Dim LineTotalBeforeDiscount As Double = 0
                Dim LineTotalAfterDiscount As Double = 0
                Dim Qunatity As Integer = 0
                Dim Price As Double = 0
                Dim Discount As Double = 0
                Dim LineDiscount As Double = 0
                Dim TaxPercent As Double = 0
                Dim LineTaxAmount As Double = 0
                Dim PriceCurrency As String = String.Empty
                Dim PriceCurrencyExRate As String = String.Empty
                Dim DocCurrency As String = ddlDocCurrency.SelectedItem.Value
                Dim LocalCurrency As String = LC.Value
                Dim BpCurrency As String = BC.Value
                Dim BPCurrencyExRate As String = String.Empty

                DRow("SNo") = Rowcount

                DRow("ItemCode") = txtItemCode.Text

                DRow("ItemDescription") = txtItemName.Text

                DRow("U_ITEM_Remarks") = txtRemarks.Text

                DRow("U_LineText") = txtLintText.Text

                'PriceCurrency = GetPriceCurrency(txtBPCode.Text, txtItemCode.Text)

                PriceCurrency = ddlPriceCurrency.SelectedItem.Value

                Price = (CType(txtItemPrice.Text, Double))
                DRow("UnitPrice") = PriceCurrency & " " & txtItemPrice.Text
                DRow("U_Price1") = txtNIPrice.Text
                'DRow("U_Group") = txtGrp.Text
                DRow("U_Stock") = ddlStock.SelectedItem.Value


                DRow("U_Country") = txtCountry.Text
                DRow("U_Print") = ddlPrint.SelectedItem.Value
                DRow("Quantity") = txtItemQty.Text
                Qunatity = (CType(txtItemQty.Text, Integer))

                DRow("Discount") = txtItemDiscount.Text
                Discount = (CType(txtItemDiscount.Text, Double))

                Dim TaxCode As String = String.Empty
                Dim TaxCodeSplitted() As String
                TaxCode = ddlTaxCode.SelectedItem.Value
                TaxCodeSplitted = TaxCode.Split("-")

                TaxPercent = (CType(TaxCodeSplitted(1), Double))
                DRow("Tax") = String.Format("{0:0.00}", TaxPercent)
                DRow("TaxCode") = ddlTaxCode.SelectedItem.Text

                DRow("WarehouseCode") = ddlWarehouse.SelectedItem.Value

                DRow("U_LineText") = txtLintText.Text

                Price = CalculateExPriceRate(LocalCurrency, PriceCurrency, BpCurrency, Price)
                'If LocalCurrency <> PriceCurrency Then
                '    PriceCurrencyExRate = GetExRate(LocalCurrency, PriceCurrency)
                '    If PriceCurrencyExRate <> "" Then
                '        Price = (Price * CType(PriceCurrencyExRate, Double))
                '    End If
                'End If
                'If LocalCurrency <> BpCurrency Then
                '    BPCurrencyExRate = txtCurExRate.Text
                '    If BPCurrencyExRate = String.Empty Or Not IsNumeric(BPCurrencyExRate) Then
                '        BPCurrencyExRate = GetExRate(LocalCurrency, BpCurrency)
                '    End If
                '    If BPCurrencyExRate <> "" Then
                '        Price = (Price / CType(BPCurrencyExRate, Double))
                '    End If
                'End If

                'LineTotalBeforeDiscount = Qunatity * Price
                'LineDiscount = (LineTotalBeforeDiscount * Discount) / 100
                'LineTotalAfterDiscount = LineTotalBeforeDiscount - LineDiscount
                'LineTaxAmount = (LineTotalAfterDiscount * TaxPercent) / 100

                DRow("LineTotal") = String.Format("{0:0.00}", LineTotalAfterDiscount)

                Select Case btnAction.Text
                    Case "Add"
                        ViewState("TempDataTable").Rows.Add(DRow.ItemArray)
                        LoadGridData()
                        ViewState("SelectedRow") = -1
                        LoadChildItems(txtItemCode.Text)
                    Case "Update"
                        'TempDataTable.Rows.Add(DRow)
                        LoadGridData()
                        'LoadChildItems(txtItemCode.Text)
                        ViewState("SelectedRow") = -1
                        btnAction.Text = "Add"
                        GridView1.SelectedIndex = -1
                End Select

                ClearLineItemboxes()

            End If
        Catch ex As Exception
            WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoadChildItems(ByVal ParentItem As String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        Dim DRow As DataRow
        Dim Rowcount As Integer = 0
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString

        Dim SAPComm As New SqlCommand("SELECT OITM.ItemCode,OITM.ItemName,ITT1.Quantity FROM OITM  INNER JOIN ITT1 ON ITT1.Code=OITM.ItemCode " &
                                "Where  ITT1.Father=@ItemCode  ", SAPConn)
        SAPConn.Open()

        SAPComm.Parameters.AddWithValue("@ItemCode", ParentItem)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()
        ViewState("SelectedRow") = ViewState("SelectedRow") + 1
        While reader.Read()
            'DRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()
            'Rowcount = TempDataTable.Rows.Count + 1

            Select Case btnAction.Text
                Case "Add"
                    DRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()

                    Rowcount = ViewState("TempDataTable").Rows.Count + 1
                    DRow("SNo") = Rowcount
                Case "Update"
                    DRow = ViewState("TempDataset").Tables("SqLineItems").Rows(ViewState("SelectedRow"))
                    Rowcount = ViewState("SelectedRow") + 1
                    DRow("SNo") = Rowcount
            End Select




            Dim CardCode As String = txtBPCode.Text
            Dim ItemCode As String = reader("ItemCode")
            Dim ItemDescription As String = reader("ItemName")
            Dim Price As Double = 0

            ' Price Calculation
            Dim PriceReaderConn As New SqlConnection()
            PriceReaderConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            PriceReaderConn.Open()
            Dim PriceCommand As New SqlCommand("SELECT T1.Price from ITM1 T1 INNER JOIN OPLN T2 ON T1.Pricelist=T2.ListNum inner join ocrd T3 on T3.listnum=T2.listnum INNER JOIN OITM T4 ON T1.ItemCode=T4.ItemCode " &
                                "where (T3.cardcode=@cardcode OR T3.cardname=@cardcode) AND (T1.ItemCode=@itemcode OR T4.itemname=@itemcode) AND T4.InvntItem='Y'  ", PriceReaderConn)
            PriceCommand.Parameters.AddWithValue("@cardcode", CardCode)
            PriceCommand.Parameters.AddWithValue("@itemcode", ItemCode)
            Dim PriceReader As SqlDataReader = PriceCommand.ExecuteReader()
            If PriceReader.Read Then
                If IsDBNull(PriceReader("Price")) Then
                    Price = 0
                Else
                    Price = PriceReader("Price")
                End If

            End If
            PriceReader.Close()
            PriceCommand.Dispose()
            PriceReaderConn.Dispose()



            Dim LineTotalBeforeDiscount As Double = 0
            Dim LineTotalAfterDiscount As Double = 0
            Dim Qunatity As Integer = 0

            Dim Discount As Double = 0
            Dim LineDiscount As Double = 0

            ' Tax Calculation
            Dim TaxPercent As Double = 0
            Dim TaxCode As String = String.Empty
            Dim TaxCodeSplitted() As String
            TaxCode = ddlTaxCode.SelectedItem.Value
            TaxCodeSplitted = TaxCode.Split("-")
            TaxPercent = (CType(TaxCodeSplitted(1), Double))



            Dim LineTaxAmount As Double = 0
            Dim PriceCurrency As String = String.Empty
            Dim PriceCurrencyExRate As String = String.Empty
            Dim DocCurrency As String = ddlDocCurrency.SelectedItem.Value
            Dim LocalCurrency As String = LC.Value
            Dim BpCurrency As String = BC.Value
            Dim BPCurrencyExRate As String = String.Empty



            'DRow("SNo") = Rowcount

            DRow("ItemCode") = ItemCode

            DRow("ItemDescription") = ItemDescription


            DRow("IsChild") = "Y"
            PriceCurrency = GetPriceCurrency(txtBPCode.Text, ItemCode)
            Price = (CType(Price, Double))
            If Price = 0 Then
                DRow("UnitPrice") = BpCurrency & " " & String.Format("{0:0.00}", Price)
            Else
                DRow("UnitPrice") = PriceCurrency & " " & String.Format("{0:0.00}", Price)
            End If

            DRow("Quantity") = (CType(reader("Quantity"), Integer))
            Qunatity = (CType(reader("Quantity"), Integer))

            DRow("Discount") = 0
            Discount = (CType(0, Double))
            DRow("Tax") = String.Format("{0:0.00}", TaxPercent)
            DRow("TaxCode") = ddlTaxCode.SelectedItem.Text

            DRow("WarehouseCode") = ddlWarehouse.SelectedItem.Value

            DRow("U_LineText") = txtLintText.Text
            'DRow("U_Price1") = txtNIPrice.Text
            DRow("U_Price1") = 0
            'DRow("U_Group") = txtGrp.Text
            DRow("U_Stock") = ddlStock.SelectedItem.Value


            DRow("U_Country") = txtCountry.Text
            DRow("U_Print") = ddlPrint.SelectedItem.Value
            'Price = CalculateExPriceRate(LocalCurrency, PriceCurrency, BpCurrency, Price)
            'If LocalCurrency <> PriceCurrency Then
            '    PriceCurrencyExRate = GetExRate(LocalCurrency, PriceCurrency)
            '    If PriceCurrencyExRate <> "" Then
            '        Price = (Price * CType(PriceCurrencyExRate, Double))
            '    End If
            'End If
            'If LocalCurrency <> BpCurrency Then
            '    BPCurrencyExRate = txtCurExRate.Text
            '    If BPCurrencyExRate = String.Empty Or Not IsNumeric(BPCurrencyExRate) Then
            '        BPCurrencyExRate = GetExRate(LocalCurrency, BpCurrency)
            '    End If
            '    If BPCurrencyExRate <> "" Then
            '        Price = (Price / CType(BPCurrencyExRate, Double))
            '    End If
            'End If

            'LineTotalBeforeDiscount = Qunatity * Price
            'LineDiscount = (LineTotalBeforeDiscount * Discount) / 100
            'LineTotalAfterDiscount = LineTotalBeforeDiscount - LineDiscount
            'LineTaxAmount = (LineTotalAfterDiscount * TaxPercent) / 100

            DRow("LineTotal") = String.Format("{0:0.00}", LineTotalAfterDiscount)

            Select Case btnAction.Text
                Case "Add"
                    ViewState("TempDataTable").Rows.Add(DRow)

                Case "Update"
                    ViewState("SelectedRow") = ViewState("SelectedRow") + 1
            End Select

        End While
        ViewState("SelectedRow") = -1
        LoadGridData()
        reader.Close()
        SAPComm.Dispose()
        SAPConn.Close()
    End Sub
    Protected Function CalculateExPriceRate(ByVal LocalCurrency As String, ByVal PriceCurrency As String, ByVal BpCurrency As String, ByVal Price As Double) As String
        Dim PriceCurrencyExRate As String = String.Empty
        Dim BPCurrencyExRate As String = String.Empty
        If LocalCurrency <> PriceCurrency Then
            PriceCurrencyExRate = GetExRate(LocalCurrency, PriceCurrency)
            If PriceCurrencyExRate <> "" Then
                Price = (Price * CType(PriceCurrencyExRate, Double))
            End If
        End If
        If LocalCurrency <> BpCurrency Then
            BPCurrencyExRate = txtCurExRate.Text
            If BPCurrencyExRate = String.Empty Or Not IsNumeric(BPCurrencyExRate) Then
                BPCurrencyExRate = GetExRate(LocalCurrency, BpCurrency)
            End If
            If BPCurrencyExRate <> "" Then
                Price = (Price / CType(BPCurrencyExRate, Double))
            End If
        End If
        Return Price
    End Function
    Protected Sub ClearLineItemboxes()
        txtItemCode.Text = String.Empty
        txtItemName.Text = String.Empty
        txtItemPrice.Text = String.Empty
        txtItemQty.Text = 1
        txtItemDiscount.Text = 0
        txtLintText.Text = String.Empty
        txtNIPrice.Text = String.Empty
        'txtGrp.Text = String.Empty
        txtCountry.Text = String.Empty
        txtRemarks.Text = String.Empty
        'ddlTaxCode.Items.Clear()
        'LoadTaxCode()
        SetTaxCode()
        SetPaymentTerm()
        'ddlWarehouse.Items.Clear()
        'LoadWarhouseCode()
        ddlPrint.SelectedIndex = ddlPrint.Items.IndexOf(ddlPrint.Items.FindByText("NO"))
        ddlStock.SelectedIndex = ddlStock.Items.IndexOf(ddlStock.Items.FindByText("No Stock"))
    End Sub

    Protected Sub GridView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.DataBound

    End Sub




    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        Dim j As Integer = 0
        For Each ro As GridViewRow In GridView1.Rows
            j = j + 1
            ro.Cells(1).Text = j
        Next
        'If GridView1.Columns.Count > 0 Then
        '    For i = 0 To GridView1.Columns.Count - 1
        '        GridView1.Columns(i).ItemStyle.HorizontalAlign = HorizontalAlign.Left
        '    Next

        'End If


        If e.Row.RowType = DataControlRowType.DataRow Then
            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
            Next
            If e.Row.Cells(e.Row.Cells.Count - 1).Text = "C" Then
                e.Row.Cells(0).Text = ""
            End If
            Dim HideRow As Integer = 17
            While e.Row.Cells.Count > HideRow


                e.Row.Cells(HideRow).Visible = False
                HideRow = HideRow + 1
            End While
            'e.Row.Cells(e.Row.Cells.Count - 2).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            Dim HideRow As Integer = 17

            While e.Row.Cells.Count > HideRow

                e.Row.Cells(HideRow).Visible = False
                HideRow = HideRow + 1
            End While



            'e.Row.Cells(e.Row.Cells.Count - 2).Visible = False
        End If


    End Sub


    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        Dim TempDataRow As DataRow
        TempDataRow = ViewState("TempDataTable").Rows(GridView1.SelectedIndex)
        txtItemCode.Text = TempDataRow("ItemCode")
        txtItemName.Text = TempDataRow("ItemDescription")

        Dim SplittedPrice() As String
        SplittedPrice = TempDataRow("UnitPrice").ToString.Split(" ")
        txtItemPrice.Text = SplittedPrice(1)
        ddlPriceCurrency.SelectedIndex = ddlPriceCurrency.Items.IndexOf(ddlPriceCurrency.Items.FindByText(SplittedPrice(0)))

        txtItemQty.Text = TempDataRow("Quantity").ToString
        txtItemDiscount.Text = TempDataRow("Discount").ToString
        txtLintText.Text = TempDataRow("U_LineText").ToString
        txtRemarks.Text = TempDataRow("U_Item_Remarks").ToString
        txtNIPrice.Text = TempDataRow("U_Price1").ToString
        'txtGrp.Text = TempDataRow("U_Group").ToString
        txtCountry.Text = TempDataRow("U_Country").ToString

        'ddlTaxCode.ClearSelection()      
        ddlTaxCode.SelectedIndex = ddlTaxCode.Items.IndexOf(ddlTaxCode.Items.FindByText(TempDataRow("TaxCode")))
        'ddlWarehouse.ClearSelection()
        'Dim TempListItemWH As ListItem = ddlWarehouse.Items.FindByValue(TempDataRow("WarehouseCode"))
        'TempListItemWH.Selected = True
        ddlWarehouse.SelectedIndex = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(TempDataRow("WarehouseCode")))
        ddlStock.SelectedIndex = ddlStock.Items.IndexOf(ddlStock.Items.FindByValue(TempDataRow("U_Stock")))
        ddlPrint.SelectedIndex = ddlPrint.Items.IndexOf(ddlPrint.Items.FindByValue(TempDataRow("U_Print")))
        btnAction.Text = "Update"
        ViewState("SelectedRow") = GridView1.SelectedIndex
        'For Each dr As GridViewRow In GridView1.Rows
        '    If dr.Cells(12).Text = "Y" Then
        '        dr.Cells(0).Text = ""
        '    End If
        'Next
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        If ViewState("SelectedRow") <> -1 Then
            'DeleteChildItems(ViewState("SelectedRow") )
            'Dim DRow As DataRow = ViewState("TempDataset").Tables("SqLineItems").Rows(ViewState("SelectedRow"))
            Dim DT As DataTable = CType(ViewState("TempDataTable"), DataTable)
            Dim DRow As DataRow = DT.Rows(ViewState("SelectedRow"))

            DRow.Delete()
            ClearLineItemboxes()
            LoadGridData()
            ViewState("SelectedRow") = -1
            btnAction.Text = "Add"
        Else

        End If
        GridView1.SelectedIndex = -1

    End Sub
    Protected Sub DeleteChildItems(ByVal SelectedRow As Integer)
        Dim DeleteIndex As New ArrayList
        Dim DRow As DataRow = ViewState("TempDataset").Tables("SqLineItems").Rows(SelectedRow)
        While Not IsDBNull(DRow("IsChild"))
            DeleteIndex.Add(SelectedRow)
            SelectedRow = SelectedRow + 1
            If ViewState("TempDataset").Tables("SqLineItems").Rows().Count > SelectedRow Then
                DRow = ViewState("TempDataset").Tables("SqLineItems").Rows(SelectedRow)
            Else
                Exit While
            End If
        End While
        DeleteIndex.Sort()
        DeleteIndex.Reverse()
        For Index As Integer = 0 To DeleteIndex.Count - 1

            Dim val As String = DeleteIndex(Index).ToString()
            DRow = ViewState("TempDataset").Tables("SqLineItems").Rows(val)
            DRow.Delete()
        Next
    End Sub
    Protected Sub LoadGridData()
        Try


            Dim TotalBeforeDiscount As Decimal = 0
            Dim DocDiscountPercent As Decimal = 0
            Dim TotalDiscountAmount As Decimal = 0
            Dim TotalAfterDiscount As Decimal = 0
            Dim TotalTax As Decimal = 0
            Dim DocTotal As Decimal = 0
            Dim Sum As Object
            Dim BpCurrency As String = BC.Value
            Dim LocalCurrency As String = LC.Value
            Dim PriceCurrency As String = String.Empty

            If IsNumeric(txtDocDiscountPercent.Text) Then
                DocDiscountPercent = CType(txtDocDiscountPercent.Text, Double)
            Else
                DocDiscountPercent = 0
            End If

            For Each dr As DataRow In ViewState("TempDataTable").Rows
                Dim LineTotalBeforeDiscount As Double = 0
                Dim LineTotalAfterDiscount As Double = 0
                Dim Qunatity As Integer = 0
                Dim TaxPercent As Double = 0
                Dim Discount As Double = 0
                Dim LineDiscount As Double = 0
                'Dim LineTaxAmount As Double = 0
                Dim Price As Double = 0
                Dim LineTotalString As String = String.Empty
                'PriceCurrency = GetPriceCurrency(txtBPCode.Text, dr("ItemCode"))
                Dim SplittedPrice() As String
                SplittedPrice = dr("UnitPrice").ToString.Split(" ")
                PriceCurrency = SplittedPrice(0)
                Price = (CType(SplittedPrice(1), Double))
                dr("UnitPrice") = PriceCurrency & " " & String.Format("{0:0.00}", Price)
                dr("PriceBefDisc") = String.Format("{0:0.00}", Price)

                Price = CalculateExPriceRate(LocalCurrency, PriceCurrency, BpCurrency, Price)


                TaxPercent = dr("Tax")
                Qunatity = dr("Quantity")
                Discount = dr("Discount")
                LineTotalBeforeDiscount = Qunatity * Price
                LineDiscount = (LineTotalBeforeDiscount * Discount) / 100
                LineTotalAfterDiscount = LineTotalBeforeDiscount - LineDiscount
                'LineTaxAmount = (LineTotalAfterDiscount * TaxPercent) / 100
                dr("LineTotal") = BC.Value & " " & String.Format("{0:0.00}", LineTotalAfterDiscount)
                If BpCurrency <> LocalCurrency Then
                    Dim LCPrice As Double = CType(dr("PriceBefDisc"), Double)
                    Dim LCLineTotalBeforeDiscount As Double = 0
                    Dim LCLineDiscount As Double = 0
                    Dim LCLineTotalAfterDiscount As Double = 0
                    Dim LCExRate As String = String.Empty
                    Dim LCPriceCurrencyExRate As String = String.Empty

                    LCPriceCurrencyExRate = GetExRate(LocalCurrency, PriceCurrency)
                    If LCPriceCurrencyExRate = String.Empty Then
                        LCPriceCurrencyExRate = 0
                    End If
                    LCPrice = (LCPrice * CType(LCPriceCurrencyExRate, Double))
                    LCLineTotalBeforeDiscount = Qunatity * LCPrice
                    LCLineDiscount = (LCLineTotalBeforeDiscount * Discount) / 100
                    LCLineTotalAfterDiscount = LCLineTotalBeforeDiscount - LCLineDiscount
                    dr("LineTotalLC") = String.Format("{0:0.00}", LCLineTotalAfterDiscount)
                    dr("TotalFrgn") = String.Format("{0:0.00}", LineTotalAfterDiscount)
                Else
                    'Commented on 08-01-2016 and changed to the following line
                    'dr("LineTotalLC") = BC.Value & " " & String.Format("{0:0.00}", LineTotalAfterDiscount)
                    dr("LineTotalLC") = String.Format("{0:0.00}", LineTotalAfterDiscount)
                    dr("TotalFrgn") = String.Format("{0:0.00}", 0)
                End If

                DocTotal = DocTotal + String.Format("{0:0.00}", LineTotalAfterDiscount)
                Dim LineTotal As Decimal = FormatNumber(LineTotalAfterDiscount, 2)
                Dim LineTaxPercent As Decimal = FormatNumber(dr("Tax"), 2)
                Dim LineTotalDocDiscount As Decimal = FormatNumber(((LineTotal * DocDiscountPercent) / 100), 2)
                TotalDiscountAmount = FormatNumber((TotalDiscountAmount + LineTotalDocDiscount), 2)
                Dim LineTotalAfterDocDiscountAmount As Double = LineTotal - LineTotalDocDiscount
                Dim LineTaxAmount As Decimal = FormatNumber(((LineTotalAfterDocDiscountAmount * LineTaxPercent) / 100), 2)
                TotalTax = FormatNumber((TotalTax + LineTaxAmount), 2)
                'dr("Tax") = String.Format("{0:0.00}", LineTaxAmount.ToString())
                LineTotal = FormatNumber((LineTaxAmount + LineTotalAfterDocDiscountAmount), 2)


            Next

            'Sum = ViewState("TempDataTable").Compute("Sum(LineTotal)", "")
            Sum = DocTotal
            If Not IsDBNull(Sum) Then
                txtTotalBeforeDiscount.Text = FormatNumber(Sum, 2)
            Else
                txtTotalBeforeDiscount.Text = 0
                Sum = 0
            End If

            txtTaxTotal.Text = FormatNumber(TotalTax, 2)
            'txtTaxTotal.Text = String.Format("{0:0.00}", 123.4)
            TotalDiscountAmount = FormatNumber(((Sum * DocDiscountPercent) / 100), 2)
            txtDocDiscountAmount.Text = TotalDiscountAmount
            txtDocTotal.Text = FormatNumber((Sum + TotalTax - TotalDiscountAmount), 2)

            GridView1.DataSource = ViewState("TempDataTable")
            GridView1.DataBind()

            ModifyGridViewHeader()
            'GridView1.HeaderRow.Cells(GridView1.HeaderRow.Cells.Count - 2).Text = "Line Total" & "(" & ddlDocCurrency.SelectedItem.Value & ")"

            LblTotalFinal.Text = "Total" & "(" & ddlDocCurrency.SelectedItem.Value & ")"
        Catch ex As Exception
            WriteLog(ex)
        End Try
    End Sub

    Protected Sub txtDocDiscountPercent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocDiscountPercent.TextChanged
        'Dim TotalBeforeDiscount As Double = 0
        'Dim DocDiscountPercent As Double = 0
        'Dim DocDicountAmount As Double = 0
        'TotalBeforeDiscount = txtTotalBeforeDiscount.Text
        'DocDiscountPercent = txtDocDiscountPercent.Text
        'DocDicountAmount = (TotalBeforeDiscount * DocDiscountPercent) / 100
        'txtDocDiscountAmount.Text = String.Format("{0:0.00}", DocDicountAmount)
        If Page.IsValid Then
            LoadGridData()
        End If

    End Sub

    Protected Sub LstAddressId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstAddressId.SelectedIndexChanged
        BindAddressDetails()
    End Sub

    Protected Sub LstCtPrsn_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstCtPrsn.SelectedIndexChanged
        BindBPContactPersonDetails()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim InternalDocId As String = String.Empty
        If txtDocNo.Text <> String.Empty And IsNumeric(txtDocNo.Text.ToString()) Then
            InternalDocId = txtDocNo.Text
        End If
        Dim UpdateCommand As SqlCommand = New SqlCommand()
        UpdateCommand.Connection = Business_SQ.FocusDBConn
        UpdateCommand.CommandText = "UPDATE SqHeader SET SubmitToSAP=1   " &
                                 "WHERE SqHeader.id=@LookUP"
        UpdateCommand.Parameters.AddWithValue("@LookUP", InternalDocId)
        UpdateCommand.ExecuteNonQuery()
        UpdateCommand.Dispose()
        Dim Msg As String = "Submitting To SAP, Internal Doc No : " & InternalDocId
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & Msg & "'); });", True)
        'Response.Redirect("~/Business/SQ.aspx?id=" & Msg & "&status=1", False)

    End Sub
    Protected Sub ClearForm()
        ClearLineItemboxes()
        InitializeTempDataTable()


        txtBPCode.Text = String.Empty
        txtBPName.Text = String.Empty
        LstCtPrsn.Items.Clear()
        txtBPRefNo.Text = String.Empty
        InitializeFormDate()
        txtCurExRate.Text = String.Empty
        ddlDocCurrency.SelectedIndex = -1
        txtDocDiscountPercent.Text = String.Empty
        LstAddressId.Items.Clear()
        lblAddressDetails.Text = String.Empty
        txtValidForDays.Text = String.Empty
        LstDeliveryTerm.SelectedIndex = -1
        LstShipFrom.SelectedIndex = -1
        LstShipTo.SelectedIndex = -1
        txtTrackingId.Text = String.Empty
        txtEndUser.Text = String.Empty
        txtDocRemarks.Text = String.Empty
        txtQuoteRemarks.Text = String.Empty
        LstDeliveryTime.SelectedIndex = -1
        ddlConcluded.SelectedIndex = 0
        ddlChance.SelectedIndex = 0
        txtCountryofOrigin.Text = String.Empty
        txtRevisionNo.Text = String.Empty
        txtHSCode.Text = String.Empty
        LstMinChrge.SelectedIndex = -1
        ddlLayout.SelectedIndex = -1
        ddlPaymentterm.SelectedIndex = -1
        txtDocTotal.Text = String.Empty
        txtDocTotal.Text = String.Empty
        txtDocDiscountAmount.Text = String.Empty
        txtTaxTotal.Text = String.Empty
        txtDocDiscountPercent.Text = String.Empty
        txtTotalBeforeDiscount.Text = String.Empty
        ddlAssignTo.SelectedIndex = -1

    End Sub
    Protected Sub ClearFormDocNotFound()
        ClearLineItemboxes()



        txtBPCode.Text = String.Empty
        txtBPName.Text = String.Empty
        LstCtPrsn.Items.Clear()
        txtBPRefNo.Text = String.Empty
        InitializeFormDate()
        txtCurExRate.Text = String.Empty
        ddlDocCurrency.SelectedIndex = -1
        txtDocDiscountPercent.Text = String.Empty
        LstAddressId.Items.Clear()
        lblAddressDetails.Text = String.Empty
        txtValidForDays.Text = String.Empty
        LstDeliveryTerm.SelectedIndex = -1
        LstShipFrom.SelectedIndex = -1
        LstShipTo.SelectedIndex = -1
        txtTrackingId.Text = String.Empty
        txtEndUser.Text = String.Empty
        txtDocRemarks.Text = String.Empty
        txtQuoteRemarks.Text = String.Empty
        LstDeliveryTime.SelectedIndex = -1
        ddlConcluded.SelectedIndex = 0
        ddlChance.SelectedIndex = 0
        txtCountryofOrigin.Text = String.Empty
        txtRevisionNo.Text = String.Empty
        txtHSCode.Text = String.Empty
        LstMinChrge.SelectedIndex = -1
        ddlLayout.SelectedIndex = -1
        ddlPaymentterm.SelectedIndex = -1
        txtDocTotal.Text = String.Empty
        txtDocTotal.Text = String.Empty
        txtDocDiscountAmount.Text = String.Empty
        txtTaxTotal.Text = String.Empty
        txtDocDiscountPercent.Text = String.Empty
        txtTotalBeforeDiscount.Text = String.Empty
        ddlAssignTo.SelectedIndex = -1

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try


            If Page.IsValid Then
                If txtDocNo.Text <> String.Empty And IsNumeric(txtDocNo.Text.ToString()) Then
                    Dim InternalDocId As Integer = txtDocNo.Text
                    Dim HeaderId As Integer = UpdateHeaderNew(InternalDocId)
                    If HeaderId = 0 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Problem in Updating this document, Contact Site Admin" & "'); });", True)
                    Else
                        Dim Saved As Boolean = UpdateSQItemsNew(HeaderId)
                        If Saved = True Then
                            ClearForm()
                            If ddlAssignTo.Text = "" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Successfully Updated, Internal DocNum: " & HeaderId & "'); });", True)
                                ' Response.Redirect("~/Business/SQ.aspx?id=Successfully Saved, Internal DocNum: " & InternalDocId & "&status=1", False)
                            Else
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Successfully Updated & Assigned, Internal DocNum: " & HeaderId & "'); });", True)
                                'Response.Redirect("~/Business/SQ.aspx?id=Successfully Saved & assigned, Internal DocNum: " & InternalDocId & "&status=1", False)
                            End If
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Problem in saving this document, Contact Site Admin" & HeaderId & "'); });", True)
                        End If

                    End If



                Else
                    Dim HeaderId As Integer
                    HeaderId = SaveSQHeaderNew()
                    If HeaderId = 0 Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Problem in saving this document, Contact Site Admin" & "'); });", True)
                    Else

                        Dim Saved As Boolean = SaveSQItemsNew(HeaderId)
                        If Saved = True Then
                            ClearForm()
                            If ddlAssignTo.Text = "" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Successfully Saved, Internal DocNum: " & HeaderId & "'); });", True)
                                'Response.Redirect("~/Business/SQ.aspx?id=Successfully Saved, Internal DocNum: " & HeaderId & "&status=1", False)
                                'Response.Redirect("~/Business/SQ.aspx", False)
                            Else
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Successfully Saved & Assigned, Internal DocNum: " & HeaderId & "'); });", True)

                                'Response.Redirect("~/Business/SQ.aspx?id=Successfully Saved & assigned, Internal DocNum: " & HeaderId & "&status=1", False)
                                'Response.Redirect("~/Business/SQ.aspx", False)
                            End If
                        Else
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "Problem in saving this document, Contact Site Admin" & HeaderId & "'); });", True)
                        End If
                    End If
                End If

            End If
        Catch ex As Exception
            WriteLog(ex)
        End Try
    End Sub
    Public Sub WriteLog(ByRef ex As Exception)

        Dim St As StackTrace = New StackTrace(ex, True)

        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.FocusDBConn
        ReaderCommand.CommandText = "INSERT INTO Errlog values(@errMsg,@InnerException,@FileName,@LineNumber,@CreatedOn) "

        ReaderCommand.Parameters.AddWithValue("@errMsg", ex.Message)
        If Not IsNothing(ex.InnerException) Then
            ReaderCommand.Parameters.AddWithValue("@InnerException", ex.InnerException.Message)
        Else
            ReaderCommand.Parameters.AddWithValue("@InnerException", String.Empty)
        End If
        Dim Filename As String = String.Empty
        Dim Linenumber As String = String.Empty
        Dim FrameCount As Integer = St.FrameCount
        For i As Integer = 0 To FrameCount - 1
            If Not IsNothing(St.GetFrame(i).GetFileName) Then
                Filename = St.GetFrame(i).GetFileName.ToString
                Linenumber = St.GetFrame(i).GetFileLineNumber.ToString
            End If
        Next
        ReaderCommand.Parameters.AddWithValue("@FileName", Filename)
        ReaderCommand.Parameters.AddWithValue("@LineNumber", Linenumber)

        ReaderCommand.Parameters.AddWithValue("@CreatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))


        ReaderCommand.ExecuteNonQuery()

        ReaderCommand.Dispose()
    End Sub
    Protected Function SaveSQHeaderNew() As Integer
        Try
            Dim ReturnHeaderId As Integer = 0
            Dim LocalCurrency As String = LC.Value
            Dim BpCurrency As String = BC.Value
            Dim InsertCommand As SqlCommand = New SqlCommand()
            InsertCommand.Connection = Business_SQ.FocusDBConn
            InsertCommand.CommandText = " Insert into  SqHeader " &
            "([CardCode]" &
           ",[CardName]" &
           ",[ContactPerson]" &
           ",[CustRefNo]" &
           ",[PostingDate]" &
           ",[ValidUntil]" &
          " ,[DocDate]" &
           ",[DocRate]" &
           ",[DocCurrency]" &
           ",[DocDiscount]" &
           ",[AddressId]" &
            ",[Address]" &
           ",[U_Validity]" &
           ",[U_Delivery_Term]" &
           ",[U_Shipped_From]" &
           ",[U_Shipped_To]" &
           ",[U_Trcking_ID]" &
           ",[U_End_User]" &
           ",[U_Remarks]" &
           ",[U_Quote_Remarks]" &
           ",[U_Delivery_Date]" &
           ",[U_Chance]" &
           ",[U_Concluded]" &
           ",[U_Country_of_origin]" &
           ",[U_Revision_No]" &
           ",[U_HSCode]" &
           ",[U_Min_Charges]" &
           ",[U_Layout]" &
           ",[PaymentTerm]" &
           ",[DocTotal]" &
           ",[DocTotalFC]" &
           ",[DiscPrcnt]" &
           ",[DiscSum]" &
           ",[DiscSumFc]" &
           ",[VatSum]" &
           ",[VatSumFC]" &
           ",[CreatedOn]" &
           ",[UpdatedOn]" &
           ",[CreatedBy])" &
            "  OUTPUT INSERTED.id  VALUES " &
            "(@CardCode" &
           ",@CardName" &
           ",@ContactPerson" &
           ",@CustRefNo" &
           ",@PostingDate" &
           ",@ValidUntil" &
          " ,@DocDate" &
           ",@DocRate" &
           ",@DocCurrency" &
           ",@DocDiscount" &
           ",@AddressId" &
            ",@Address" &
           ",@U_Validity" &
           ",@U_Delivery_Term" &
           ",@U_Shipped_From" &
           ",@U_Shipped_To" &
           ",@U_Trcking_ID" &
           ",@U_End_User" &
           ",@U_Remarks" &
           ",@U_Quote_Remarks" &
           ",@U_Delivery_Date" &
           ",@U_Chance" &
           ",@U_Concluded" &
           ",@U_Country_of_origin" &
           ",@U_Revision_No" &
           ",@U_HSCode" &
           ",@U_Min_Charges" &
           ",@U_Layout" &
           ",@PaymentTerm" &
           ",@DocTotal" &
           ",@DocTotalFC" &
           ",@DiscPrcnt" &
           ",@DiscSum" &
           ",@DiscSumFc" &
           ",@VatSum" &
           ",@VatSumFC" &
           ",@CreatedOn" &
           ",@UpdatedOn" &
           ",@CreatedBy)"

            'InsertCommand.Parameters.AddWithValue("@DocStatus", LblStatus.Text)
            InsertCommand.Parameters.AddWithValue("@CardCode", txtBPCode.Text)
            InsertCommand.Parameters.AddWithValue("@CardName", txtBPName.Text)
            If LstCtPrsn.Items.Count > 0 Then
                InsertCommand.Parameters.AddWithValue("@ContactPerson", LstCtPrsn.SelectedItem.Value)
            Else
                InsertCommand.Parameters.AddWithValue("@ContactPerson", String.Empty)
            End If
            InsertCommand.Parameters.AddWithValue("@CustRefNo", txtBPRefNo.Text)
            InsertCommand.Parameters.AddWithValue("@PostingDate", Format(CDate(txtPostingDate.Text), "yyyy-MM-dd "))
            InsertCommand.Parameters.AddWithValue("@ValidUntil", Format(CDate(txtValidUntil.Text), "yyyy-MM-dd "))
            InsertCommand.Parameters.AddWithValue("@DocDate", Format(CDate(txtDocDate.Text), "yyyy-MM-dd "))
            InsertCommand.Parameters.AddWithValue("@DocRate", txtCurExRate.Text)

            InsertCommand.Parameters.AddWithValue("@DocCurrency", ddlDocCurrency.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@DocDiscount", txtDocDiscountPercent.Text)
            InsertCommand.Parameters.AddWithValue("@AddressId", LstAddressId.SelectedItem.Text)
            InsertCommand.Parameters.AddWithValue("@Address", lblAddressDetails.Text.Replace("<br/>", ""))


            InsertCommand.Parameters.AddWithValue("@U_Validity", txtValidForDays.Text)
            InsertCommand.Parameters.AddWithValue("@U_Delivery_Term", LstDeliveryTerm.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@U_Shipped_From", LstShipFrom.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@U_Shipped_To", LstShipTo.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@U_Trcking_ID", txtTrackingId.Text)
            InsertCommand.Parameters.AddWithValue("@U_End_User", txtEndUser.Text)
            InsertCommand.Parameters.AddWithValue("@U_Remarks", txtDocRemarks.Text)
            InsertCommand.Parameters.AddWithValue("@U_Quote_Remarks", txtQuoteRemarks.Text)
            InsertCommand.Parameters.AddWithValue("@U_Delivery_Date", LstDeliveryTime.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@U_Concluded", ddlConcluded.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@U_Chance", ddlChance.SelectedItem.Value)
            InsertCommand.Parameters.AddWithValue("@U_Country_of_origin", txtCountryofOrigin.Text)
            InsertCommand.Parameters.AddWithValue("@U_Revision_No", txtRevisionNo.Text)
            InsertCommand.Parameters.AddWithValue("@U_HSCode", txtHSCode.Text)
            InsertCommand.Parameters.AddWithValue("@U_Min_Charges", LstMinChrge.SelectedItem.Value.ToString)
            InsertCommand.Parameters.AddWithValue("@U_Layout", ddlLayout.SelectedItem.Value.ToString)
            InsertCommand.Parameters.AddWithValue("@PaymentTerm", ddlPaymentterm.SelectedItem.Value.ToString)

            Dim DocTotalFC As Decimal = 0
            Dim DocTotal As Decimal = 0
            Dim DiscSumFc As Decimal = 0
            Dim VatSumFC As Decimal = 0
            Dim DiscSum As Decimal = 0
            Dim VatSum As Decimal = 0



            If BpCurrency <> LocalCurrency Then
                Decimal.TryParse(txtDocTotal.Text, DocTotalFC)
                Decimal.TryParse(txtDocDiscountAmount.Text, DiscSumFc)
                Decimal.TryParse(txtTaxTotal.Text, VatSumFC)
                Decimal.TryParse(txtTaxTotal.Text, VatSum)

                InsertCommand.Parameters.AddWithValue("@DocTotalFC", DocTotalFC)
                InsertCommand.Parameters.AddWithValue("@DocTotal", 0)
                InsertCommand.Parameters.AddWithValue("@DiscSumFc", DiscSumFc)
                InsertCommand.Parameters.AddWithValue("@VatSumFC", VatSumFC)
                InsertCommand.Parameters.AddWithValue("@DiscSum", 0)
                InsertCommand.Parameters.AddWithValue("@VatSum", VatSum)

                'InsertCommand.Parameters.AddWithValue("@DocTotalFC", txtDocTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DocTotal", 0)
                'InsertCommand.Parameters.AddWithValue("@DiscSumFc", txtDocDiscountAmount.Text)
                'InsertCommand.Parameters.AddWithValue("@VatSumFC", txtTaxTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DiscSum", 0)
                'InsertCommand.Parameters.AddWithValue("@VatSum", txtTaxTotal.Text)


            Else
                Decimal.TryParse(txtDocTotal.Text, DocTotal)
                Decimal.TryParse(txtDocDiscountAmount.Text, DiscSumFc)
                Decimal.TryParse(txtTaxTotal.Text, VatSumFC)


                InsertCommand.Parameters.AddWithValue("@DocTotalFC", 0)
                InsertCommand.Parameters.AddWithValue("@DocTotal", DocTotal)
                InsertCommand.Parameters.AddWithValue("@DiscSumFc", DiscSumFc)
                InsertCommand.Parameters.AddWithValue("@VatSumFC", VatSumFC)
                InsertCommand.Parameters.AddWithValue("@DiscSum", 0)
                InsertCommand.Parameters.AddWithValue("@VatSum", 0)

                'InsertCommand.Parameters.AddWithValue("@DocTotalFC", 0)
                'InsertCommand.Parameters.AddWithValue("@DocTotal", txtDocTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DiscSumFc", txtDocDiscountAmount.Text)
                'InsertCommand.Parameters.AddWithValue("@VatSumFC", txtTaxTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DiscSum", 0)
                'InsertCommand.Parameters.AddWithValue("@VatSum", 0)
            End If



            InsertCommand.Parameters.AddWithValue("@DiscPrcnt", txtDocDiscountPercent.Text)



            InsertCommand.Parameters.AddWithValue("@CreatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
            InsertCommand.Parameters.AddWithValue("@UpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))

            If ddlAssignTo.Text = "" Then
                InsertCommand.Parameters.AddWithValue("@CreatedBy", Membership.GetUser.UserName)
            Else
                InsertCommand.Parameters.AddWithValue("@CreatedBy", ddlAssignTo.Text)
            End If
            Dim PrimaryKeyValueReader As SqlDataReader = InsertCommand.ExecuteReader()
            Try
                If PrimaryKeyValueReader.Read Then
                    ReturnHeaderId = PrimaryKeyValueReader("Id").ToString
                End If
                PrimaryKeyValueReader.Close()
            Catch ex As Exception
                PrimaryKeyValueReader.Close()
                WriteLog(ex)
                Return 0
            End Try
            Return ReturnHeaderId
        Catch ex As Exception
            WriteLog(ex)
            Return 0
        End Try
    End Function

    Protected Function SaveSQHeader() As Integer
        Dim LocalCurrency As String = LC.Value
        Dim BpCurrency As String = BC.Value
        Dim LocalTempHeaderDataTable As New DataTable
        Dim SqlAdapterHeader As _
           New SqlDataAdapter("Select * From SqHeader", FocusDBConn)
        SqlAdapterHeader.FillSchema(ViewState("TempDataset"), SchemaType.Source, "SqHeader")
        LocalTempHeaderDataTable = ViewState("TempDataset").Tables("SqHeader")

        Dim TempItemDataRow As DataRow = ViewState("TempDataset").Tables("SqHeader").NewRow()
        TempItemDataRow("CardCode") = txtBPCode.Text
        TempItemDataRow("CardName") = txtBPName.Text
        'TempItemDataRow("ContactPerson") = LstCtPrsn.SelectedItem.Value
        If LstCtPrsn.Items.Count > 0 Then
            TempItemDataRow("ContactPerson") = LstCtPrsn.SelectedItem.Value
        Else
            TempItemDataRow("ContactPerson") = ""
        End If
        TempItemDataRow("CustRefNo") = txtBPRefNo.Text
        TempItemDataRow("AddressId") = LstAddressId.SelectedItem.Text
        TempItemDataRow("Address") = lblAddressDetails.Text.Replace("<br/>", "")
        TempItemDataRow("PostingDate") = Format(CDate(txtPostingDate.Text), "yyyy-MM-dd ")
        TempItemDataRow("ValidUntil") = Format(CDate(txtValidUntil.Text), "yyyy-MM-dd ")
        TempItemDataRow("DocDate") = Format(CDate(txtDocDate.Text), "yyyy-MM-dd ")


        TempItemDataRow("DiscPrcnt") = txtDocDiscountPercent.Text

        If BpCurrency <> LocalCurrency Then
            TempItemDataRow("DocTotalFC") = txtDocTotal.Text
            TempItemDataRow("DocTotal") = 0
            TempItemDataRow("DiscSumFC") = txtDocDiscountAmount.Text
            TempItemDataRow("VatSumFC") = txtTaxTotal.Text
            TempItemDataRow("DiscSum") = 0
            TempItemDataRow("VatSum") = txtTaxTotal.Text
        Else
            TempItemDataRow("DocTotalFC") = 0
            TempItemDataRow("DocTotal") = txtDocTotal.Text
            TempItemDataRow("DiscSum") = txtDocDiscountAmount.Text
            TempItemDataRow("VatSum") = txtTaxTotal.Text
            TempItemDataRow("DiscSumFC") = 0
            TempItemDataRow("VatSumFC") = 0
        End If
        TempItemDataRow("DocRate") = txtCurExRate.Text
        TempItemDataRow("DocCurrency") = ddlDocCurrency.SelectedItem.Value
        TempItemDataRow("DocDiscount") = txtDocDiscountPercent.Text
        TempItemDataRow("CreatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
        TempItemDataRow("UpdatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
        If ddlAssignTo.Text = "" Then
            TempItemDataRow("CreatedBy") = Membership.GetUser.UserName
        Else
            TempItemDataRow("CreatedBy") = ddlAssignTo.Text
        End If
        TempItemDataRow("U_Validity") = txtValidForDays.Text
        TempItemDataRow("U_Delivery_Term") = LstDeliveryTerm.SelectedItem.Value
        TempItemDataRow("U_Shipped_From") = LstShipFrom.SelectedItem.Value
        TempItemDataRow("U_Shipped_To") = LstShipTo.SelectedItem.Value
        TempItemDataRow("U_Trcking_ID") = txtTrackingId.Text
        TempItemDataRow("U_End_User") = txtEndUser.Text
        TempItemDataRow("U_Delivery_Date") = LstDeliveryTime.SelectedItem.Value
        TempItemDataRow("U_Concluded") = ddlConcluded.SelectedItem.Value
        TempItemDataRow("U_Chance") = ddlChance.SelectedItem.Value
        TempItemDataRow("U_Remarks") = txtDocRemarks.Text
        TempItemDataRow("U_Quote_Remarks") = txtQuoteRemarks.Text
        TempItemDataRow("U_Country_of_origin") = txtCountryofOrigin.Text
        TempItemDataRow("U_Revision_No") = txtRevisionNo.Text
        TempItemDataRow("U_HSCode") = txtHSCode.Text
        TempItemDataRow("U_Min_Charges") = LstMinChrge.SelectedItem.Value.ToString
        TempItemDataRow("U_Layout") = ddlLayout.SelectedItem.Value.ToString
        TempItemDataRow("PaymentTerm") = ddlPaymentterm.SelectedItem.Value

        Dim SCB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapterHeader)
        LocalTempHeaderDataTable.Rows.Add(TempItemDataRow)
        SqlAdapterHeader.Update(ViewState("TempDataset"), "SqHeader")

        SqlAdapterHeader.Dispose()


        Dim HeaderId As Integer
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.FocusDBConn
        ReaderCommand.CommandText = "SELECT MAX(id) as id from SqHeader " &
            "WHERE SqHeader.CreatedBy=@LookUP"
        ReaderCommand.Parameters.AddWithValue("@LookUP", Membership.GetUser.UserName)
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        If reader.Read() Then
            HeaderId = reader("id").ToString()
        End If
        reader.Close()
        ReaderCommand.Dispose()
        LocalTempHeaderDataTable.Dispose()
        Return HeaderId
    End Function
    Protected Function UpdateSQItemsNew(ByVal HeaderId As Integer) As Boolean
        Dim SqlTrans As SqlTransaction = Business_SQ.FocusDBConn.BeginTransaction()
        Try
            Dim Result As Boolean = True

            If Not IsNothing(ViewState("TempDataTable")) Then
                If ViewState("TempDataTable").Rows.Count > 0 Then
                    Dim DeleteCommand As SqlCommand = New SqlCommand()
                    DeleteCommand.Connection = Business_SQ.FocusDBConn
                    DeleteCommand.Transaction = SqlTrans
                    DeleteCommand.CommandText = " DELETE FROM SqLineItems  Where HeaderId=@HeaderID"
                    DeleteCommand.Parameters.AddWithValue("@HeaderID", HeaderId)
                    DeleteCommand.ExecuteNonQuery()

                    Dim LocalTempDataTable As New DataTable
                    LocalTempDataTable = ViewState("TempDataTable")
                    For Each ro As DataRow In LocalTempDataTable.Rows
                        Dim InsertCommand As SqlCommand = New SqlCommand()
                        InsertCommand.Connection = Business_SQ.FocusDBConn
                        InsertCommand.Transaction = SqlTrans
                        InsertCommand.CommandText = " INSERT INTO SqLineItems" &
                        "([SNo]" &
                       ",[HeaderId]" &
                       ",[ItemCode]" &
                       ",[ItemDescription]" &
                       ",[U_Item_Remarks]" &
                       ",[U_Print]" &
                       ",[U_LineText]" &
                       ",[U_Price1]" &
                       ",[U_Country]" &
                       ",[U_Stock]" &
                       ",[UnitPrice]" &
                       ",[Quantity]" &
                       ",[Discount]" &
                       ",[TaxCode]" &
                       ",[Tax]" &
                       ",[WarehouseCode]" &
                       ",[IsChild]" &
                       ",[LineTotal]" &
                       ",[LineStatus]" &
                       ",[PriceBefDisc]" &
                       ",[LineTotalLC]" &
                       ",[TotalFrgn])" &
                       " VALUES " &
                       "(@SNo" &
                       ",@HeaderId" &
                       ",@ItemCode" &
                       ",@ItemDescription" &
                       ",@U_Item_Remarks" &
                       ",@U_Print" &
                       ",@U_LineText" &
                       ",@U_Price1" &
                       ",@U_Country" &
                       ",@U_Stock" &
                       ",@UnitPrice" &
                       ",@Quantity" &
                       ",@Discount" &
                       ",@TaxCode" &
                       ",@Tax" &
                       ",@WarehouseCode" &
                       ",@IsChild" &
                       ",@LineTotal" &
                       ",@LineStatus" &
                       ",@PriceBefDisc" &
                       ",@LineTotalLC" &
                       ",@TotalFrgn)"


                        InsertCommand.Parameters.AddWithValue("SNo", ro("SNo"))
                        InsertCommand.Parameters.AddWithValue("HeaderId", HeaderId)
                        InsertCommand.Parameters.AddWithValue("ItemCode", ro("ItemCode"))
                        InsertCommand.Parameters.AddWithValue("ItemDescription", ro("ItemDescription"))
                        InsertCommand.Parameters.AddWithValue("U_Item_Remarks", ro("U_ITEM_Remarks"))
                        InsertCommand.Parameters.AddWithValue("U_Print", ro("U_Print"))
                        InsertCommand.Parameters.AddWithValue("U_LineText", ro("U_LineText"))
                        InsertCommand.Parameters.AddWithValue("U_Price1", ro("U_Price1"))
                        InsertCommand.Parameters.AddWithValue("U_Country", ro("U_Country"))
                        InsertCommand.Parameters.AddWithValue("U_Stock", ro("U_Stock"))
                        InsertCommand.Parameters.AddWithValue("UnitPrice", ro("UnitPrice"))
                        InsertCommand.Parameters.AddWithValue("Quantity", ro("Quantity"))
                        InsertCommand.Parameters.AddWithValue("Discount", ro("Discount"))
                        InsertCommand.Parameters.AddWithValue("TaxCode", ro("TaxCode"))
                        InsertCommand.Parameters.AddWithValue("Tax", ro("Tax"))
                        InsertCommand.Parameters.AddWithValue("WarehouseCode", ro("WarehouseCode"))
                        InsertCommand.Parameters.AddWithValue("IsChild", ro("IsChild"))
                        InsertCommand.Parameters.AddWithValue("LineTotal", ro("LineTotal"))
                        InsertCommand.Parameters.AddWithValue("LineStatus", ro("LineStatus"))
                        InsertCommand.Parameters.AddWithValue("PriceBefDisc", ro("PriceBefDisc"))
                        InsertCommand.Parameters.AddWithValue("LineTotalLC", ro("LineTotalLC"))
                        InsertCommand.Parameters.AddWithValue("TotalFrgn", ro("TotalFrgn"))

                        InsertCommand.ExecuteNonQuery()
                    Next
                Else
                    SqlTrans.Rollback()
                    Return False
                End If
            Else
                SqlTrans.Rollback()
                Return False
            End If
            SqlTrans.Commit()
            Return Result
        Catch ex As Exception
            WriteLog(ex)
            SqlTrans.Rollback()
            Return False
        End Try
    End Function
    Protected Function SaveSQItemsNew(ByVal HeaderId As Integer) As Boolean
        Try
            Dim Result As Boolean = True
            If Not IsNothing(ViewState("TempDataTable")) Then
                If ViewState("TempDataTable").Rows.Count > 0 Then
                    Dim LocalTempDataTable As New DataTable
                    LocalTempDataTable = ViewState("TempDataTable")
                    For Each ro As DataRow In LocalTempDataTable.Rows
                        Dim InsertCommand As SqlCommand = New SqlCommand()
                        InsertCommand.Connection = Business_SQ.FocusDBConn
                        InsertCommand.CommandText = " INSERT INTO SqLineItems" &
                        "([SNo]" &
                       ",[HeaderId]" &
                       ",[ItemCode]" &
                       ",[ItemDescription]" &
                       ",[U_Item_Remarks]" &
                       ",[U_Print]" &
                       ",[U_LineText]" &
                       ",[U_Price1]" &
                       ",[U_Country]" &
                       ",[U_Stock]" &
                       ",[UnitPrice]" &
                       ",[Quantity]" &
                       ",[Discount]" &
                       ",[TaxCode]" &
                       ",[Tax]" &
                       ",[WarehouseCode]" &
                       ",[IsChild]" &
                       ",[LineTotal]" &
                       ",[LineStatus]" &
                       ",[PriceBefDisc]" &
                       ",[LineTotalLC]" &
                       ",[TotalFrgn])" &
                       " VALUES " &
                       "(@SNo" &
                       ",@HeaderId" &
                       ",@ItemCode" &
                       ",@ItemDescription" &
                       ",@U_Item_Remarks" &
                       ",@U_Print" &
                       ",@U_LineText" &
                       ",@U_Price1" &
                       ",@U_Country" &
                       ",@U_Stock" &
                       ",@UnitPrice" &
                       ",@Quantity" &
                       ",@Discount" &
                       ",@TaxCode" &
                       ",@Tax" &
                       ",@WarehouseCode" &
                       ",@IsChild" &
                       ",@LineTotal" &
                       ",@LineStatus" &
                       ",@PriceBefDisc" &
                       ",@LineTotalLC" &
                       ",@TotalFrgn)"


                        InsertCommand.Parameters.AddWithValue("SNo", ro("SNo"))
                        InsertCommand.Parameters.AddWithValue("HeaderId", HeaderId)
                        InsertCommand.Parameters.AddWithValue("ItemCode", ro("ItemCode"))
                        InsertCommand.Parameters.AddWithValue("ItemDescription", ro("ItemDescription"))
                        InsertCommand.Parameters.AddWithValue("U_Item_Remarks", ro("U_ITEM_Remarks"))
                        InsertCommand.Parameters.AddWithValue("U_Print", ro("U_Print"))
                        InsertCommand.Parameters.AddWithValue("U_LineText", ro("U_LineText"))
                        InsertCommand.Parameters.AddWithValue("U_Price1", ro("U_Price1"))
                        InsertCommand.Parameters.AddWithValue("U_Country", ro("U_Country"))
                        InsertCommand.Parameters.AddWithValue("U_Stock", ro("U_Stock"))
                        InsertCommand.Parameters.AddWithValue("UnitPrice", ro("UnitPrice"))
                        InsertCommand.Parameters.AddWithValue("Quantity", ro("Quantity"))
                        InsertCommand.Parameters.AddWithValue("Discount", ro("Discount"))
                        InsertCommand.Parameters.AddWithValue("TaxCode", ro("TaxCode"))
                        InsertCommand.Parameters.AddWithValue("Tax", ro("Tax"))
                        InsertCommand.Parameters.AddWithValue("WarehouseCode", ro("WarehouseCode"))
                        InsertCommand.Parameters.AddWithValue("IsChild", ro("IsChild"))
                        InsertCommand.Parameters.AddWithValue("LineTotal", ro("LineTotal"))
                        InsertCommand.Parameters.AddWithValue("LineStatus", ro("LineStatus"))
                        InsertCommand.Parameters.AddWithValue("PriceBefDisc", ro("PriceBefDisc"))
                        InsertCommand.Parameters.AddWithValue("LineTotalLC", ro("LineTotalLC"))
                        InsertCommand.Parameters.AddWithValue("TotalFrgn", ro("TotalFrgn"))

                        InsertCommand.ExecuteNonQuery()
                    Next
                Else
                    Return False
                End If
            Else
                Return False
            End If
            Return Result
        Catch ex As Exception
            WriteLog(ex)
            Return False
        End Try
    End Function
    Protected Sub SaveSQItems(ByVal HeaderId As Integer)
        Try


            If Not IsNothing(ViewState("TempDataTable")) Then
                If ViewState("TempDataTable").Rows.Count > 0 Then

                    Dim LocalTempDataTable As New DataTable
                    Dim SqlAdapter As _
                       New SqlDataAdapter("Select  * From SqLineItems", FocusDBConn)

                    SqlAdapter.FillSchema(ViewState("TempDataset"), SchemaType.Source, "SqLineItems")
                    SqlAdapter.Fill(LocalTempDataTable)
                    'LocalTempDataTable = ViewState("TempDataset").Tables("SqLineItems")
                    Dim i As Integer = 0
                    For Each DataRowItem As DataRow In ViewState("TempDataTable").Rows
                        Dim DR As DataRow = ViewState("TempDataset").Tables("SqLineItems").Rows(i)
                        DR("HeaderId") = HeaderId
                        If DR("U_Price1") = String.Empty Then
                            DR("U_Price1") = 0
                        End If

                        i = i + 1
                    Next
                    For Each DataRowItem As DataRow In ViewState("TempDataTable").Rows
                        Dim TempDataRow As DataRow = ViewState("TempDataset").Tables("SqLineItems").NewRow()
                        TempDataRow = DataRowItem
                        Dim CB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapter)
                        SqlAdapter.Update(ViewState("TempDataset"), "SqLineItems")
                    Next
                    SqlAdapter.Dispose()
                    ''Dim b As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapter)
                    ''SqlAdapter.Update(ViewState("TempDataset"), "SqLineItems")
                    ViewState("TempDataTable").Rows.Clear()
                    LoadGridData()
                    LocalTempDataTable.Dispose()
                Else
                    Response.Redirect("~/Business/SQ.aspx?id=ViewState Expired.Please Try again&status=0", False)
                End If
            Else
                Response.Redirect("~/Business/SQ.aspx?id=ViewState Expired.Please Try again&status=0", False)
            End If
        Catch ex As Exception
            WriteLog(ex)
        End Try
    End Sub
    Protected Function UpdateHeaderNew(ByVal InternalDocId As String) As Integer
        Try
            Dim ReturnHeaderId As Integer = 0
            Dim LocalCurrency As String = LC.Value
            Dim BpCurrency As String = BC.Value
            Dim UpdateCommand As SqlCommand = New SqlCommand()
            UpdateCommand.Connection = Business_SQ.FocusDBConn
            UpdateCommand.CommandText = " UPDATE  SqHeader  SET " &
           "CardCode=@CardCode" &
           ",CardName=@CardName" &
           ",ContactPerson=@ContactPerson" &
           ",CustRefNo=@CustRefNo" &
           ",PostingDate=@PostingDate" &
           ",ValidUntil=@ValidUntil" &
          " ,DocDate=@DocDate" &
           ",DocRate=@DocRate" &
           ",DocCurrency=@DocCurrency" &
           ",DocDiscount=@DocDiscount" &
           ",AddressId=@AddressId" &
            ",Address=@Address" &
           ",U_Validity=@U_Validity" &
           ",U_Delivery_Term=@U_Delivery_Term" &
           ",U_Shipped_From=@U_Shipped_From" &
           ",U_Shipped_To=@U_Shipped_To" &
           ",U_Trcking_ID=@U_Trcking_ID" &
           ",U_End_User=@U_End_User" &
           ",U_Remarks=@U_Remarks" &
           ",U_Quote_Remarks=@U_Quote_Remarks" &
           ",U_Delivery_Date=@U_Delivery_Date" &
           ",U_Chance=@U_Chance" &
           ",U_Concluded=@U_Concluded" &
           ",U_Country_of_origin=@U_Country_of_origin" &
           ",U_Revision_No=@U_Revision_No" &
           ",U_HSCode=@U_HSCode" &
           ",U_Min_Charges=@U_Min_Charges" &
           ",U_Layout=@U_Layout" &
           ",PaymentTerm=@PaymentTerm" &
           ",DocTotal=@DocTotal" &
           ",DocTotalFC=@DocTotalFC" &
           ",DiscPrcnt=@DiscPrcnt" &
           ",DiscSum=@DiscSum" &
           ",DiscSumFc=@DiscSumFc" &
           ",VatSum=@VatSum" &
           ",VatSumFC=@VatSumFC" &
           ",UpdatedOn=@UpdatedOn" &
           ",CreatedBy=@CreatedBy" &
           "  OUTPUT INSERTED.id  WHERE " &
            " id=@Headerid"
            UpdateCommand.Parameters.AddWithValue("@Headerid", InternalDocId)
            UpdateCommand.Parameters.AddWithValue("@CardCode", txtBPCode.Text)
            UpdateCommand.Parameters.AddWithValue("@CardName", txtBPName.Text)
            If LstCtPrsn.Items.Count > 0 Then
                UpdateCommand.Parameters.AddWithValue("@ContactPerson", LstCtPrsn.SelectedItem.Value)
            Else
                UpdateCommand.Parameters.AddWithValue("@ContactPerson", String.Empty)
            End If
            UpdateCommand.Parameters.AddWithValue("@CustRefNo", txtBPRefNo.Text)
            UpdateCommand.Parameters.AddWithValue("@PostingDate", Format(CDate(txtPostingDate.Text), "yyyy-MM-dd "))
            UpdateCommand.Parameters.AddWithValue("@ValidUntil", Format(CDate(txtValidUntil.Text), "yyyy-MM-dd "))
            UpdateCommand.Parameters.AddWithValue("@DocDate", Format(CDate(txtDocDate.Text), "yyyy-MM-dd "))
            UpdateCommand.Parameters.AddWithValue("@DocRate", txtCurExRate.Text)

            UpdateCommand.Parameters.AddWithValue("@DocCurrency", ddlDocCurrency.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@DocDiscount", txtDocDiscountPercent.Text)
            UpdateCommand.Parameters.AddWithValue("@AddressId", LstAddressId.SelectedItem.Text)
            UpdateCommand.Parameters.AddWithValue("@Address", lblAddressDetails.Text.Replace("<br/>", ""))


            UpdateCommand.Parameters.AddWithValue("@U_Validity", txtValidForDays.Text)
            UpdateCommand.Parameters.AddWithValue("@U_Delivery_Term", LstDeliveryTerm.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@U_Shipped_From", LstShipFrom.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@U_Shipped_To", LstShipTo.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@U_Trcking_ID", txtTrackingId.Text)
            UpdateCommand.Parameters.AddWithValue("@U_End_User", txtEndUser.Text)
            UpdateCommand.Parameters.AddWithValue("@U_Remarks", txtDocRemarks.Text)
            UpdateCommand.Parameters.AddWithValue("@U_Quote_Remarks", txtQuoteRemarks.Text)
            UpdateCommand.Parameters.AddWithValue("@U_Delivery_Date", LstDeliveryTime.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@U_Concluded", ddlConcluded.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@U_Chance", ddlChance.SelectedItem.Value)
            UpdateCommand.Parameters.AddWithValue("@U_Country_of_origin", txtCountryofOrigin.Text)
            UpdateCommand.Parameters.AddWithValue("@U_Revision_No", txtRevisionNo.Text)
            UpdateCommand.Parameters.AddWithValue("@U_HSCode", txtHSCode.Text)
            UpdateCommand.Parameters.AddWithValue("@U_Min_Charges", LstMinChrge.SelectedItem.Value.ToString)
            UpdateCommand.Parameters.AddWithValue("@U_Layout", ddlLayout.SelectedItem.Value.ToString)
            UpdateCommand.Parameters.AddWithValue("@PaymentTerm", ddlPaymentterm.SelectedItem.Value.ToString)

            Dim DocTotalFC As Decimal = 0
            Dim DocTotal As Decimal = 0
            Dim DiscSumFc As Decimal = 0
            Dim VatSumFC As Decimal = 0
            Dim DiscSum As Decimal = 0
            Dim VatSum As Decimal = 0



            If BpCurrency <> LocalCurrency Then
                Decimal.TryParse(txtDocTotal.Text, DocTotalFC)
                Decimal.TryParse(txtDocDiscountAmount.Text, DiscSumFc)
                Decimal.TryParse(txtTaxTotal.Text, VatSumFC)
                Decimal.TryParse(txtTaxTotal.Text, VatSum)

                UpdateCommand.Parameters.AddWithValue("@DocTotalFC", DocTotalFC)
                UpdateCommand.Parameters.AddWithValue("@DocTotal", 0)
                UpdateCommand.Parameters.AddWithValue("@DiscSumFc", DiscSumFc)
                UpdateCommand.Parameters.AddWithValue("@VatSumFC", VatSumFC)
                UpdateCommand.Parameters.AddWithValue("@DiscSum", 0)
                UpdateCommand.Parameters.AddWithValue("@VatSum", VatSum)

                'InsertCommand.Parameters.AddWithValue("@DocTotalFC", txtDocTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DocTotal", 0)
                'InsertCommand.Parameters.AddWithValue("@DiscSumFc", txtDocDiscountAmount.Text)
                'InsertCommand.Parameters.AddWithValue("@VatSumFC", txtTaxTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DiscSum", 0)
                'InsertCommand.Parameters.AddWithValue("@VatSum", txtTaxTotal.Text)


            Else
                Decimal.TryParse(txtDocTotal.Text, DocTotal)
                Decimal.TryParse(txtDocDiscountAmount.Text, DiscSumFc)
                Decimal.TryParse(txtTaxTotal.Text, VatSumFC)


                UpdateCommand.Parameters.AddWithValue("@DocTotalFC", 0)
                UpdateCommand.Parameters.AddWithValue("@DocTotal", DocTotal)
                UpdateCommand.Parameters.AddWithValue("@DiscSumFc", DiscSumFc)
                UpdateCommand.Parameters.AddWithValue("@VatSumFC", VatSumFC)
                UpdateCommand.Parameters.AddWithValue("@DiscSum", 0)
                UpdateCommand.Parameters.AddWithValue("@VatSum", 0)

                'InsertCommand.Parameters.AddWithValue("@DocTotalFC", 0)
                'InsertCommand.Parameters.AddWithValue("@DocTotal", txtDocTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DiscSumFc", txtDocDiscountAmount.Text)
                'InsertCommand.Parameters.AddWithValue("@VatSumFC", txtTaxTotal.Text)
                'InsertCommand.Parameters.AddWithValue("@DiscSum", 0)
                'InsertCommand.Parameters.AddWithValue("@VatSum", 0)
            End If


            'If BpCurrency <> LocalCurrency Then
            '    UpdateCommand.Parameters.AddWithValue("@DocTotalFC", txtDocTotal.Text)
            '    UpdateCommand.Parameters.AddWithValue("@DocTotal", 0)
            '    UpdateCommand.Parameters.AddWithValue("@DiscSumFc", txtDocDiscountAmount.Text)
            '    UpdateCommand.Parameters.AddWithValue("@VatSumFC", txtTaxTotal.Text)
            '    UpdateCommand.Parameters.AddWithValue("@DiscSum", 0)
            '    UpdateCommand.Parameters.AddWithValue("@VatSum", txtTaxTotal.Text)
            'Else
            '    UpdateCommand.Parameters.AddWithValue("@DocTotalFC", 0)
            '    UpdateCommand.Parameters.AddWithValue("@DocTotal", txtDocTotal.Text)
            '    UpdateCommand.Parameters.AddWithValue("@DiscSumFc", txtDocDiscountAmount.Text)
            '    UpdateCommand.Parameters.AddWithValue("@VatSumFC", txtTaxTotal.Text)
            '    UpdateCommand.Parameters.AddWithValue("@DiscSum", 0)
            '    UpdateCommand.Parameters.AddWithValue("@VatSum", 0)
            'End If



            UpdateCommand.Parameters.AddWithValue("@DiscPrcnt", txtDocDiscountPercent.Text)




            UpdateCommand.Parameters.AddWithValue("@UpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))

            If ddlAssignTo.Text = "" Then
                UpdateCommand.Parameters.AddWithValue("@CreatedBy", Membership.GetUser.UserName)
            Else
                UpdateCommand.Parameters.AddWithValue("@CreatedBy", ddlAssignTo.Text)
            End If

            Dim PrimaryKeyValueReader As SqlDataReader = UpdateCommand.ExecuteReader()
            Try


                If PrimaryKeyValueReader.Read Then
                    ReturnHeaderId = PrimaryKeyValueReader("Id").ToString
                End If
                PrimaryKeyValueReader.Close()
            Catch ex As Exception
                PrimaryKeyValueReader.Close()
                WriteLog(ex)
                Return 0
            End Try
            Return ReturnHeaderId
        Catch ex As Exception
            WriteLog(ex)
            Return 0
        End Try
    End Function
    Protected Sub UpdateHeader(ByVal InternalDocId As String)
        Dim LocalCurrency As String = LC.Value
        Dim BpCurrency As String = BC.Value

        Dim LocalTempHeaderDataTable As New DataTable

        Dim SqlAdapterHeader As _
           New SqlDataAdapter("Select * From SqHeader", FocusDBConn)
        'SqlAdapterHeader.FillSchema(ViewState("TempDataset"), SchemaType.Source, "SqHeader")
        SqlAdapterHeader.MissingSchemaAction = MissingSchemaAction.AddWithKey
        SqlAdapterHeader.Fill(LocalTempHeaderDataTable)
        Dim dr As DataRow = LocalTempHeaderDataTable.Rows.Find(InternalDocId)
        dr("CardCode") = txtBPCode.Text
        dr("CardName") = txtBPName.Text
        'dr("ContactPerson") = LstCtPrsn.SelectedItem.Value
        If LstCtPrsn.Items.Count > 0 Then
            dr("ContactPerson") = LstCtPrsn.SelectedItem.Value
        Else
            dr("ContactPerson") = ""
        End If
        dr("CustRefNo") = txtBPRefNo.Text
        dr("AddressId") = LstAddressId.SelectedItem.Text
        dr("Address") = lblAddressDetails.Text.Replace("<br/>", "")
        dr("PostingDate") = Format(CDate(txtPostingDate.Text), "yyyy-MM-dd ")
        dr("ValidUntil") = Format(CDate(txtValidUntil.Text), "yyyy-MM-dd ")
        dr("DocDate") = Format(CDate(txtDocDate.Text), "yyyy-MM-dd ")
        dr("DiscPrcnt") = txtDocDiscountPercent.Text
        If BpCurrency <> LocalCurrency Then
            dr("DocTotalFC") = txtDocTotal.Text
            dr("DocTotal") = 0
            dr("DiscSumFC") = txtDocDiscountAmount.Text
            dr("VatSumFC") = txtTaxTotal.Text
            dr("DiscSum") = 0
            dr("VatSum") = txtTaxTotal.Text
        Else
            dr("DocTotalFC") = 0
            dr("DocTotal") = txtDocTotal.Text
            dr("DiscSum") = txtDocDiscountAmount.Text
            dr("VatSum") = txtTaxTotal.Text
            dr("DiscSumFC") = 0
            dr("VatSumFC") = 0
        End If
        dr("DocRate") = txtCurExRate.Text
        dr("DocCurrency") = ddlDocCurrency.SelectedItem.Value
        dr("DocDiscount") = txtDocDiscountPercent.Text
        dr("CreatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
        dr("UpdatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
        If ddlAssignTo.Text = "" Then
            dr("CreatedBy") = Membership.GetUser.UserName
        Else
            dr("CreatedBy") = ddlAssignTo.Text
        End If
        dr("U_Validity") = txtValidForDays.Text
        dr("U_Delivery_Term") = LstDeliveryTerm.SelectedItem.Value
        dr("U_Shipped_From") = LstShipFrom.SelectedItem.Value
        dr("U_Shipped_To") = LstShipTo.SelectedItem.Value
        dr("U_Trcking_ID") = txtTrackingId.Text
        dr("U_End_User") = txtEndUser.Text
        dr("U_Delivery_Date") = LstDeliveryTime.SelectedItem.Value
        dr("U_Chance") = ddlChance.SelectedItem.Value
        dr("U_Concluded") = ddlConcluded.SelectedItem.Value
        dr("U_Remarks") = txtDocRemarks.Text
        dr("U_Quote_Remarks") = txtQuoteRemarks.Text
        dr("U_Country_of_Origin") = txtCountryofOrigin.Text
        dr("U_Revision_No") = txtRevisionNo.Text
        dr("U_HSCode") = txtHSCode.Text
        dr("U_Min_Charges") = LstMinChrge.SelectedItem.Value.ToString
        dr("U_Layout") = ddlLayout.SelectedItem.Value.ToString
        dr("PaymentTerm") = ddlPaymentterm.SelectedItem.Value
        Dim b As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapterHeader)
        SqlAdapterHeader.Update(LocalTempHeaderDataTable)
        SqlAdapterHeader.Dispose()

        Dim LocalTempItemDataTable As New DataTable
        Dim SqlAdapterItems As _
                New SqlDataAdapter("Select [SNo], [ItemCode] ,[ItemDescription],[U_Item_Remarks] ,[U_LineText],[U_Country]  ,[UnitPrice], [U_Price1]  ,[Quantity]  ,[Discount]  ,[TaxCode], [Tax] ,[LineTotal],[WarehouseCode],[U_Stock],[U_Print],[IsChild],[LineStatus],[PriceBefDisc], [LineTotalLC], [TotalFrgn] From SqLineItems", FocusDBConn)
        Dim SqlCommand As New SqlCommand("DELETE FROM SqLineItems  Where HeaderId=@HeaderID", FocusDBConn)
        SqlAdapterItems.DeleteCommand = SqlCommand
        SqlAdapterItems.DeleteCommand.Parameters.AddWithValue("@HeaderID", InternalDocId)
        SqlAdapterItems.Fill(LocalTempItemDataTable)
        SqlAdapterItems.DeleteCommand.ExecuteNonQuery()
        SqlAdapterItems.Dispose()
        SqlCommand.Dispose()
        LocalTempItemDataTable.Dispose()
        SaveSQItems(InternalDocId)
    End Sub
    'Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click

    '    '     Dim LocalTempItemDataTable As New DataTable
    '    '     Dim c As Integer = 1
    '    '     Dim SqlAdapterItems As _
    '    '        New SqlDataAdapter("Select * From SqLineItems", FocusDBConn)
    '    '     Dim SqlCommand As New SqlCommand("UPDATE SqLineItems SET ItemCode=@ItemCode Where HeaderId=@HeaderID", FocusDBConn)
    '    '     SqlAdapterItems.UpdateCommand = SqlCommand
    '    '     SqlAdapterItems.UpdateCommand.Parameters.Add( _
    '    '"@ItemCode", SqlDbType.NVarChar, 15, "ItemCode")

    '    '     'Dim parameter As SqlParameter = _
    '    '     'SqlAdapterItems.UpdateCommand.Parameters.Add( _
    '    '     '"@HeaderID", SqlDbType.Int)
    '    '     'parameter.SourceColumn = "HeaderId"
    '    '     'parameter.SourceVersion = DataRowVersion.Proposed

    '    '     SqlAdapterItems.UpdateCommand.Parameters.AddWithValue("@HeaderID", InternalDocId)

    '    '     SqlAdapterItems.Fill(LocalTempItemDataTable)

    '    '     For Each Temprow As DataRow In LocalTempItemDataTable.Rows
    '    '         Temprow("ItemCode") = "222"
    '    '     Next




    '    '     Dim d As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapterHeader)
    '    '     SqlAdapterItems.Update(LocalTempItemDataTable)
    '    '     ViewState("TempDataTable").Rows.Clear()
    'End Sub


    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("~/Business/SQ.aspx", False)
    End Sub

    Protected Sub ValidateBPCode(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtBPCodeValidator.ServerValidate

        Dim Memship As MembershipUser = Membership.GetUser()
        Dim Prof As ProfileCommon = ProfileBase.Create(Memship.UserName, True)
        Dim Terittory As String = Prof.Territory
        Terittory = Terittory.Replace("|", ",").TrimEnd(",")

        Dim sLookUP As String = String.Empty
        Dim SAPConn As New SqlConnection()
        sLookUP = args.Value.ToString()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT CardCode FROM OCRD " &
                                "WHERE OCRD.CardType='C' AND OCRD.CardCode=@LookUP AND OCRD.Territory in (" & Terittory & ")   ", SAPConn)
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
        reader.Close()
        SAPComm.Dispose()
        SAPConn.Close()
    End Sub
    Protected Sub ValidateBPName(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtBPNameValidator.ServerValidate

        Dim Memship As MembershipUser = Membership.GetUser()
        Dim Prof As ProfileCommon = ProfileBase.Create(Memship.UserName, True)
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
        reader.Close()
        SAPConn.Close()
    End Sub
    Protected Sub ValidateBPAddress(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles LstAddressIdValidator.ServerValidate

        Dim sLookUP As String = String.Empty

        sLookUP = args.Value.ToString()
        If sLookUP = String.Empty Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If
    End Sub
    Protected Sub ValidateBPContactPerson(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles LstCtPrsnValidator.ServerValidate

        Dim sLookUP As String = String.Empty

        sLookUP = args.Value.ToString()
        If sLookUP = String.Empty Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If
    End Sub
    Protected Sub ValidateNumericOnly(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtValidForDaysValidator.ServerValidate

        Dim sLookUP As String = String.Empty
        Dim Expression = "^[1-9]+$"

        sLookUP = args.Value.ToString()
        If Regex.IsMatch(sLookUP, Expression) Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub
    Protected Sub ValidateDecimalOnly(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtDocDiscountPercentValidator.ServerValidate

        Dim sLookUP As String = String.Empty
        Dim Expression = "^\d+\.?\d*$"


        sLookUP = args.Value.ToString()

        If Regex.IsMatch(sLookUP, Expression) Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If

    End Sub
    Protected Sub ValidateGrid(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles GridView1Validator.ServerValidate
        If GridView1.Rows.Count = 0 Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If

    End Sub
    Protected Sub ValidateItemDelete(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles ItemDeleteValidator.ServerValidate
        If ViewState("SelectedRow") = -1 Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If

    End Sub
    Protected Sub ValidateItemCode(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtItemCodeValidator.ServerValidate

        Dim sLookUP As String = String.Empty
        Dim SAPConn As New SqlConnection()
        sLookUP = args.Value.ToString()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT ItemCode FROM OITM " & _
                                "WHERE validFor='Y' AND  OITM.ItemCode=@LookUP ", SAPConn)
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
        reader.Close()
        SAPComm.Dispose()
        SAPConn.Close()
    End Sub
    Protected Sub txtDocNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocNo.TextChanged
        txtSAPDocNo.Text = String.Empty
    End Sub

    Protected Sub txtSAPDocNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSAPDocNo.TextChanged
        txtDocNo.Text = String.Empty
    End Sub

    Protected Sub ddlDocCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDocCurrency.SelectedIndexChanged
        BC.Value = ddlDocCurrency.SelectedItem.Value

        LoadExRate(LC.Value, BC.Value)
        LoadGridData()
    End Sub

    Protected Sub txtPostingDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPostingDate.TextChanged
        LoadExRate(LC.Value, BC.Value)
    End Sub


    Protected Sub txtCurExRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCurExRate.TextChanged
        LoadGridData()
    End Sub
    Protected Sub LoadStockandPrint()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = Business_SQ.SAPDBConn
        ReaderCommand.CommandText = "SELECT CUFD.AliasID, UFD1.FldValue,UFD1.Descr from CUFD INNER JOIN UFD1 ON UFD1.TableID=CUFD.TableID AND UFD1.FieldID=CUFD.FieldID" & _
                                " WHERE CUFD.TableID='ADO1'"
        Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        While reader.Read()


            If reader("AliasID").ToString = "PRINT" Then
                ddlPrint.Items.Add(New ListItem(reader("Descr").ToString, reader("FldValue").ToString))
            ElseIf reader("AliasID").ToString = "STOCK" Then
                ddlStock.Items.Add(New ListItem(reader("Descr").ToString, reader("FldValue").ToString))
            End If
        End While
        reader.Close()
        ReaderCommand.Dispose()
        ddlPrint.SelectedIndex = ddlPrint.Items.IndexOf(ddlPrint.Items.FindByText("NO"))
        ddlStock.SelectedIndex = ddlStock.Items.IndexOf(ddlStock.Items.FindByText("No Stock"))
    End Sub
    Protected Sub txtItemCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemCode.TextChanged

        'Dim ReaderCommand As SqlCommand = New SqlCommand()
        'ReaderCommand.Connection = Business_SQ.SAPDBConn
        'ReaderCommand.CommandText = "SELECT CUFD.AliasID, UFD1.FldValue,UFD1.Descr from CUFD INNER JOIN UFD1 ON UFD1.TableID=CUFD.TableID AND UFD1.FieldID=CUFD.FieldID" & _
        '                        " WHERE CUFD.TableID='ADO1'"
        'Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()
        'While reader.Read()


        '    If reader("AliasID").ToString = "PRINT" Then
        '        ddlPrint.Items.Add(New ListItem(reader("Descr").ToString, reader("FldValue").ToString))
        '    ElseIf reader("AliasID").ToString = "STOCK" Then
        '        ddlStock.Items.Add(New ListItem(reader("Descr").ToString, reader("FldValue").ToString))
        '    End If
        'End While
        'reader.Close()
        'ReaderCommand.Dispose()

        Dim ItemCode As String = txtItemCode.Text.ToString()
        Dim ReaderCommandCountry As SqlCommand = New SqlCommand()
        ReaderCommandCountry.Connection = Business_SQ.SAPDBConn
        ReaderCommandCountry.CommandText = "SELECT SWW,UserText FROM OITM" & _
                                " WHERE OITM.ItemCode=@ItemCode"
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@ItemCode"
        SqlPara.Value = ItemCode
        ReaderCommandCountry.Parameters.Add(SqlPara)
        'ReaderCommandCountry.Parameters.AddWithValue("@ItemCode", txtItemCode.Text.ToString().Replace("'", "''"))

        Dim Countryreader As SqlDataReader = ReaderCommandCountry.ExecuteReader()
        If Countryreader.Read Then
            txtCountry.Text = Countryreader("SWW").ToString
            txtRemarks.Text = Countryreader("UserText").ToString
        End If
        Countryreader.Close()
        ReaderCommandCountry.Dispose()
        Dim PriceCurrency As String = String.Empty
        PriceCurrency = GetPriceCurrency(txtBPCode.Text, txtItemCode.Text.ToString().Replace("'", "''"))
        ddlPriceCurrency.SelectedIndex = ddlPriceCurrency.Items.IndexOf(ddlPriceCurrency.Items.FindByValue(PriceCurrency))
        If PriceCurrency = String.Empty Then
            ddlPriceCurrency.SelectedIndex = ddlPriceCurrency.Items.IndexOf(ddlPriceCurrency.Items.FindByValue(BC.Value))
        End If

    End Sub

    Protected Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            Dim Layout As String = RBCustIndenter.SelectedItem.Value
            If txtDocNo.Text <> String.Empty And IsNumeric(txtDocNo.Text.ToString()) Then

                Dim InternalDocId As String = txtDocNo.Text
                Dim DocEntry As Integer = InternalDocId

                If DocEntry <> "0" Then

                    Me.Context.Items.Add("DocEntry", DocEntry)
                    Context.Response.Write("<script> language='javascript'>window.open('ReportViewer.aspx?Layout=" & Layout & "&DocEntry=" & DocEntry & "','_blank');</script>")

                End If

            End If
        Catch ex As Exception
            WriteLog(ex)
        End Try
        'Dim Layout As String = RBCustIndenter.SelectedItem.Value
        'If txtDocNo.Text <> String.Empty And IsNumeric(txtDocNo.Text.ToString()) Then
        '    Dim InternalDocId As String = txtDocNo.Text
        '    'Dim DocEntry As Integer = getSAPId(InternalDocId)
        '    'Dim DocEntry As Integer = InternalDocId
        '    Dim DocEntry As Integer = InternalDocId
        '    If DocEntry <> "0" Then
        '        Dim doc As New ReportDocument()
        '        Dim ReportFileName As String = String.Empty
        '        If Layout = "C" Then
        '            ReportFileName = Server.MapPath("~/Report/SQ.report.rpt")
        '        Else
        '            ReportFileName = Server.MapPath("~/Report/Indenter.rpt")
        '        End If

        '        'Dim fileName As String = "C:\Users\USER\Desktop\PackingSlip.report.rpt"
        '        'reportdocument.SetDatabaseLogon("sa", "Admin123", "vivek", "PURCHASE");


        '        doc.Load(ReportFileName)
        '        doc.SetDatabaseLogon(ConfigurationManager.AppSettings("DB_Username"), ConfigurationManager.AppSettings("DB_Password"), ConfigurationManager.AppSettings("Company_Server"), ConfigurationManager.AppSettings("Company_DB"))
        '        doc.SetParameterValue("@id", DocEntry)
        '        Dim exportOpts As ExportOptions = doc.ExportOptions

        '        exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat

        '        exportOpts.ExportDestinationType = ExportDestinationType.DiskFile

        '        exportOpts.DestinationOptions = New DiskFileDestinationOptions()

        '        Dim diskOpts As New DiskFileDestinationOptions()

        '        Dim origin As New DateTime(1970, 1, 1, 0, 0, 0, 0)
        '        Dim Diff As New TimeSpan
        '        Diff = Now - origin
        '        Dim FileNameGenerated As String = Math.Floor(Diff.TotalSeconds)
        '        FileNameGenerated = DocEntry & "_" & FileNameGenerated & "_Train.pdf"
        '        'CType(doc.ExportOptions.DestinationOptions, DiskFileDestinationOptions).DiskFileName = Server.MapPath("~/Reports/" & FileNameGenerated)
        '        CType(doc.ExportOptions.DestinationOptions, DiskFileDestinationOptions).DiskFileName = "D:\ReportsSQ\" & FileNameGenerated
        '        'export the report to PDF rather than displaying the report in a viewer

        '        doc.Export()

        '        'force download dialog to download the PDF file at user end.

        '        'Set the appropriate ContentType.

        '        Response.ContentType = "Application/pdf"

        '        'Get the physical path to the file.

        '        'Dim FilePath As String = Server.MapPath("~/DesktopModules/OnlineForm/OnlineForm.pdf")

        '        ''Write the file directly to the HTTP content output stream.

        '        'Response.WriteFile(Server.MapPath("~/Reports/" & FileNameGenerated))
        '        Response.WriteFile("D:\ReportsSQ\" & FileNameGenerated)
        '        Response.End()
        '    Else
        '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GPMS", "$(document).ready(function () { alert('" & "There is no Document found" & "'); });", True)
        '        'Response.Redirect("~/Business/SQ.aspx?id=There is no Document found&status=0", False)
        '    End If
        'End If
        ''Dim fp As StreamWriter

        ''Try
        ''    fp = File.CreateText("D:\ReportsSQ\" & "test.txt")
        ''    fp.WriteLine("test")
        ''    Response.Write("File Succesfully created!")
        ''    fp.Close()
        ''Catch err As Exception
        ''    Response.Write(err.ToString())
        ''Finally

        ''End Try
    End Sub
    Protected Sub btnPreviewDisc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreviewDisc.Click
        Try
            Dim Layout As String = RBCustIndenter.SelectedItem.Value
            Dim LayoutType As String = "Disc"

            If txtDocNo.Text <> String.Empty And IsNumeric(txtDocNo.Text.ToString()) Then

                Dim InternalDocId As String = txtDocNo.Text
                Dim DocEntry As Integer = InternalDocId

                If DocEntry <> "0" Then

                    Me.Context.Items.Add("DocEntry", DocEntry)
                    Context.Response.Write("<script> language='javascript'>window.open('ReportViewer.aspx?Layout=" & Layout & "&DocEntry=" & DocEntry & "&LayoutType=" & LayoutType & "','_blank');</script>")

                End If

            End If
        Catch ex As Exception
            WriteLog(ex)
        End Try
    End Sub
    Protected Sub txtItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.TextChanged
        Dim ItemCode As String = txtItemCode.Text.ToString()
        Dim ReaderCommandCountry As SqlCommand = New SqlCommand()
        ReaderCommandCountry.Connection = Business_SQ.SAPDBConn
        ReaderCommandCountry.CommandText = "SELECT SWW,UserText FROM OITM" & _
                                " WHERE OITM.ItemCode=@ItemCode"
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@ItemCode"
        SqlPara.Value = ItemCode
        ReaderCommandCountry.Parameters.Add(SqlPara)
        'ReaderCommandCountry.Parameters.AddWithValue("@ItemCode", txtItemCode.Text.ToString().Replace("'", "''"))

        Dim Countryreader As SqlDataReader = ReaderCommandCountry.ExecuteReader()
        If Countryreader.Read Then
            txtCountry.Text = Countryreader("SWW").ToString
            txtRemarks.Text = Countryreader("UserText").ToString
        End If
        Countryreader.Close()
        ReaderCommandCountry.Dispose()
        Dim PriceCurrency As String = String.Empty
        PriceCurrency = GetPriceCurrency(txtBPCode.Text, txtItemCode.Text.ToString().Replace("'", "''"))
        ddlPriceCurrency.SelectedIndex = ddlPriceCurrency.Items.IndexOf(ddlPriceCurrency.Items.FindByValue(PriceCurrency))
        If PriceCurrency = String.Empty Then
            ddlPriceCurrency.SelectedIndex = ddlPriceCurrency.Items.IndexOf(ddlPriceCurrency.Items.FindByValue(BC.Value))
        End If

    End Sub
    Protected Sub ddlLayout_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLayout.DataBound
        ddlLayout.SelectedIndex = ddlLayout.Items.IndexOf(ddlLayout.Items.FindByText("Sales Quotation"))
    End Sub
End Class
