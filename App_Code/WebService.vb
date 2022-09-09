Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient
Imports System.Web.HttpContext

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetBPCodes(ByVal sLookUP As String) As List(Of String)

        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        If HttpContext.Current.Request.IsAuthenticated Then
            sLookUP = sLookUP.Trim
            SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString

            Dim Memship As MembershipUser = Membership.GetUser()
            Dim Prof As ProfileCommon = ProfileBase.Create(Memship.UserName, True)
            Dim Terittory As String = Prof.Territory
            Terittory = Terittory.Replace("|", ",").TrimEnd(",")

            Dim SAPComm As New SqlCommand("SELECT CardCode FROM OCRD " & _
                                    "WHERE OCRD.CardType='C' AND OCRD.CardCode LIKE '%'+@LookUP+'%'  AND OCRD.Territory in (" & Terittory & ")     ORDER BY CardCode ", SAPConn)
            SAPConn.Open()

            Dim SqlPara As New SqlParameter
            SqlPara.ParameterName = "@LookUp"
            SqlPara.Value = sLookUP
            SAPComm.Parameters.Add(SqlPara)

            'MembershipUser mu = Membership.GetUser(userName, false);
            'ProfileCommon p = (ProfileCommon)ProfileBase.Create(mu.UserName, true);
            
            'Terittory = "'" & Terittory & "'"
            ''AND OCRD.Territory=@Territory 
            'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)
            'SAPComm.Parameters.AddWithValue("@Territory", Terittory)
            Dim reader As SqlDataReader = SAPComm.ExecuteReader()

            While reader.Read()
                lstBPs.Add(reader("Cardcode").ToString())
            End While
            reader.Close()
            SAPConn.Close()
        End If

        Return lstBPs


    End Function
    <WebMethod()> _
    Public Function GetBPCode(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()

        sLookUP = sLookUP.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT Top 1 CardCode FROM OCRD " & _
                                "WHERE OCRD.CardType='C' AND OCRD.CardName=@LookUP ", SAPConn)
        SAPConn.Open()
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)
        'Dim ctext As String = SAPComm.Parameters("@LookUP").Value
        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("Cardcode").ToString())
        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function
    <WebMethod()> _
    Public Function GetBPNames(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim.Replace("'", "''")
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim Memship As MembershipUser = Membership.GetUser()
        Dim Prof As ProfileCommon = ProfileBase.Create(Memship.UserName, True)
        Dim Terittory As String = Prof.Territory
        Terittory = Terittory.Replace("|", ",").TrimEnd(",")
        Dim SAPComm As New SqlCommand("SELECT CardName FROM OCRD " & _
                                "WHERE OCRD.CardType='C' AND  OCRD.CardName LIKE '%'+@LookUP+'%' AND OCRD.Territory  in (" & Terittory & ")  ORDER BY CardName", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        ''AND OCRD.Territory=@Territory 
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)
        'SAPComm.Parameters.AddWithValue("@Territory", Prof.Territory.Replace("|", ",").TrimEnd(","))

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("CardName").ToString())

        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function
    <WebMethod()> _
    Public Function GetBPName(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT Top 1 CardName FROM OCRD " & _
                                "WHERE OCRD.CardType='C' AND OCRD.CardCode=@LookUP  ", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("CardName").ToString())
        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs

    End Function
    <WebMethod()> _
    Public Function GetSlpName(ByVal sLookUP As String) As List(Of CntPrsn)

        Dim lstBPs As New List(Of CntPrsn)
        sLookUP = sLookUP.Trim
        Dim SAPConn As New SqlConnection()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT ocpr.CntctCode,ocpr.Name from OCRD INNER JOIN OCPR ON OCPR.CardCode= OCRD.CardCode " & _
                                "WHERE OCRD.CardType='C' AND OCRD.CardCode=@LookUP OR   OCRD.CardName=@LookUP", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            Dim slpcls As New CntPrsn()
            slpcls.CntPrsnCode = reader("CntctCode").ToString()
            slpcls.CntPrsnName = reader("Name").ToString()
            lstBPs.Add(slpcls)
        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function
    <WebMethod()> _
    Public Function GetItemCodes(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT top 100 ItemCode FROM OITM " & _
                                "WHERE  OITM.FrozenFor !='Y' AND  OITM.ItemCode LIKE '%'+@LookUP+'%'  ORDER BY ItemCode ", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("ItemCode").ToString())
        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function
    <WebMethod()> _
    Public Function GetItemName(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT Top 1 ItemName FROM OITM " & _
                                "WHERE OITM.ItemCode=@LookUP  ", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("ItemName").ToString())
        End While

        reader.Close()
        SAPConn.Close()
        Return lstBPs

    End Function
    <WebMethod()> _
    Public Function GetItemNames(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT top 100 ItemName FROM OITM " & _
                                "WHERE  OITM.FrozenFor !='Y' AND OITM.ItemName LIKE '%'+@LookUP+'%'  ORDER BY ItemName ", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("ItemName").ToString())
        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function

    <WebMethod()> _
    Public Function GetItemCode(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT Top 1 ItemCode FROM OITM " & _
                                "WHERE  OITM.ItemName=@LookUP  ", SAPConn)
        SAPConn.Open()

        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            lstBPs.Add(reader("ItemCode").ToString())
        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs

    End Function
    <WebMethod()> _
    Public Function GetItemPrice(ByVal itemcode As String, ByVal cardcode As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        cardcode = cardcode.Trim
        itemcode = itemcode.Trim
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT T1.Price,T1.Currency from ITM1 T1 INNER JOIN OPLN T2 ON T1.Pricelist=T2.ListNum inner join ocrd T3 on T3.listnum=T2.listnum INNER JOIN OITM T4 ON T1.ItemCode=T4.ItemCode " & _
                                "where (T3.cardcode=@cardcode OR T3.cardname=@cardcode) AND (T1.ItemCode=@itemcode OR T4.itemname=@itemcode)   ", SAPConn)
        SAPConn.Open()
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@cardcode"
        SqlPara.Value = cardcode
        SAPComm.Parameters.Add(SqlPara)
        'SAPComm.Parameters.AddWithValue("@cardcode", cardcode)

        Dim SqlPara1 As New SqlParameter
        SqlPara1.ParameterName = "@itemcode"
        SqlPara1.Value = itemcode
        SAPComm.Parameters.Add(SqlPara1)
        'SAPComm.Parameters.AddWithValue("@itemcode", itemcode)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        If reader.Read() Then
            If IsDBNull(reader("Price").ToString()) Then
                Dim NullReplace As String = "0.00"
                lstBPs.Add(Math.Round(Convert.ToDecimal(NullReplace), 2).ToString)
            Else
                If reader("Price").ToString() = String.empty Then
                    Dim NullReplace As String = "0.00"
                    lstBPs.Add(Math.Round(Convert.ToDecimal(NullReplace), 2).ToString)
                Else
                    lstBPs.Add(Math.Round(Convert.ToDecimal(reader("Price").ToString()), 2).ToString)
                End If

            End If
        Else
            Dim NullReplace As String = "0.00"
            lstBPs.Add(Math.Round(Convert.ToDecimal(NullReplace), 2).ToString)
        End If
        reader.Close()
        SAPConn.Close()
        Return lstBPs

    End Function
    <WebMethod()> _
   Public Function GetInternalDocId(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim.Replace("'", "''")
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT id, CardCode FROM SqHeader " & _
                                "WHERE SqHeader.id LIKE '%'+@LookUP+'%' AND  DocStatus is Null  ", SAPConn)
        SAPConn.Open()
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        If User.IsInRole("SameTerritoryAccess") Then
            Dim CreatedbyList As List(Of String) = New List(Of String)
            Dim Createdby As String = String.empty
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
            If User.IsInRole("SameTerritoryAccess") Then
                If AccessibleDoc(reader("CardCode").ToString()) Then
                    lstBPs.Add(reader("id").ToString())
                End If
            Else
                lstBPs.Add(reader("id").ToString())
            End If

        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function
    Protected Function AccessibleDoc(ByVal BPCode As String) As Boolean
        Dim Accessible As Boolean = False


        Dim ReaderCommand As SqlCommand = New SqlCommand()
        Dim CardCodeTerritory As String = String.Empty
        Dim SAPConn As New SqlConnection()
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        SAPConn.open()
        ReaderCommand.Connection = SAPConn
        ReaderCommand.CommandText = "SELECT  OCRD.Territory from OCRD  " & _
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
        SAPConn.Close()
        ReaderCommand.Dispose()


        Dim SplittedTerritory() As String = {CardCodeTerritory}
        Dim CurrentUserProfile As ProfileCommon = ProfileBase.Create(User.Identity.Name, True)
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
    <WebMethod()> _
    Public Function GetSAPDocId(ByVal sLookUP As String) As List(Of String)
        Dim lstBPs As New List(Of String)()
        Dim SAPConn As New SqlConnection()
        sLookUP = sLookUP.Trim.Replace("'", "''")
        SAPConn.ConnectionString = ConfigurationManager.ConnectionStrings("Focus_CRM_DB_ConnectionString").ConnectionString
        Dim SAPComm As New SqlCommand("SELECT DocNum, CardCode FROM SqHeader " & _
                                "WHERE SqHeader.DocNum LIKE '%'+@LookUP+'%' AND  DocStatus is Null  ", SAPConn)
        SAPConn.Open()
        Dim SqlPara As New SqlParameter
        SqlPara.ParameterName = "@LookUp"
        SqlPara.Value = sLookUP
        SAPComm.Parameters.Add(SqlPara)
        If User.IsInRole("SameTerritoryAccess") Then
            Dim CreatedbyList As List(Of String) = New List(Of String)
            Dim Createdby As String = String.empty
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
            SAPComm.CommandText += "  AND SqHeader.CreatedBy IN(" + Createdby + ")"
        Else
            SAPComm.CommandText += "  AND SqHeader.CreatedBy=@CreatedBy"
            SAPComm.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)
        End If
        SAPComm.CommandText += "  Order by SqHeader.id"
        'SAPComm.Parameters.AddWithValue("@LookUP", sLookUP)

        Dim reader As SqlDataReader = SAPComm.ExecuteReader()

        While reader.Read()
            If User.IsInRole("SameTerritoryAccess") Then
                If AccessibleDoc(reader("CardCode").ToString()) Then
                    lstBPs.Add(reader("DocNum").ToString())
                End If
            Else
                lstBPs.Add(reader("DocNum").ToString())
            End If

        End While
        reader.Close()
        SAPConn.Close()
        Return lstBPs
    End Function
End Class

Public Class CntPrsn
    Public CntPrsnCode As String
    Public CntPrsnName As String
End Class