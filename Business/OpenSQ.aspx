<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="OpenSQ.aspx.vb" Inherits="Business_OpenSQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
  <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Open Sales Quotations</h2>
    <div style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;" >
    <table>

<tr><td>Customer Name</td><td>
    <asp:TextBox ID="txtBPName" runat="server" Width="297px" Height="22px"></asp:TextBox><asp:CustomValidator ID="txtBPNameValidator" runat="server" ForeColor="Red"  ControlToValidate="txtBPName"
                        ErrorMessage="Invalid Customer Name" OnServerValidate="ValidateBPName" ValidationGroup="1"></asp:CustomValidator></td>
    <td><asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="1" /></td></tr>
</table>
</div>
  <div style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;" 
        align="center">
        <center> <b style="font-size: large">Open Sales Quotation List</b></center>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333"  Width="60%" 
            GridLines="None"  EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True" >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
               <asp:CommandField  HeaderText="Details" SelectText="View" ShowSelectButton="true"  
                SortExpression="Document #" />
                <asp:BoundField DataField="DocNum" HeaderText="Document #" 
                    SortExpression="DocNum" />
                <asp:BoundField DataField="DocStatus" HeaderText="Status" 
                    SortExpression="DocStatus" />
                <asp:BoundField DataField="CardCode" HeaderText="Customer Code" 
                    SortExpression="CardCode" />
                <asp:BoundField DataField="CardName" HeaderText="Customer Name" 
                    SortExpression="CardName" />
                <asp:BoundField DataField="ValidUntil" HeaderText="Valid Until"   DataFormatString="{0:dd/MM/yy}"
                    SortExpression="ValidUntil" />
                <asp:BoundField DataField="DocDate" HeaderText="Doc Date"  DataFormatString="{0:dd/MM/yy}"
                    SortExpression="DocDate" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Focus_CRM_DB_ConnectionString %>" 
            SelectCommand="SELECT CASE WHEN ISNULL(docnum,'')='' THEN id ELSE DocNum END AS  DocNum  , [DocStatus], [CardCode], [CardName], [ValidUntil], [DocDate] FROM [SqHeader] WHERE ([DocStatus] IS NULL) AND [CreatedBy]=@CreatedBy  AND [CardName] LIKE @CardName">
            <SelectParameters>
            <asp:Parameter Name="CreatedBy" DefaultValue="" />
            <asp:Parameter Name="CardName" DefaultValue="%" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
         <div style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;" 
        align="center">
        <center> <b style="font-size: large">Details</b></center>
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataSourceID="SqlDataSource2" EmptyDataText="No records found" 
            ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" Width="70%" HorizontalAlign="Center">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="Document #" HeaderText="Document #" 
                    SortExpression="Document #" />
                <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity"  DataFormatString="{0:0.00}"
                    SortExpression="Quantity" />
                       <asp:BoundField DataField="Currency" HeaderText="Currency"  
                SortExpression="Currency" />  
                <asp:BoundField DataField="Cust Price" HeaderText="Cust Price"  DataFormatString="{0:0.00}"
                    SortExpression="Cust Price" />
                <asp:BoundField DataField="N.I Price" HeaderText="N.I Price"  DataFormatString="{0:0.00}"
                    SortExpression="N.I Price" />
                <asp:BoundField DataField="Discount" HeaderText="Discount"  DataFormatString="{0:0.00}"
                    SortExpression="Discount" />
              <asp:BoundField DataField="Price After Discount"  DataFormatString="{0:0.00}"
                HeaderText="Price After Discount" ReadOnly="True" 
                SortExpression="Price After Discount" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Focus_CRM_DB_ConnectionString %>" 
            SelectCommand="SELECT SqHeader.DocNum AS 'Document #',SqLineItems.ItemDescription as 'Model',SqLineItems.Quantity, SUBSTRING (SqLineItems.UnitPrice, 1, 3) AS 'Currency',SqLineItems.UnitPrice AS 'Cust Price', SqLineItems.U_PRICE1 AS 'N.I Price', SqLineItems.Discount AS 'Discount',(SqLineItems.PriceBefDisc-((SqLineItems.PriceBefDisc*SqLineItems.Discount)/100)) as 'Price After Discount'  from  SqHeader INNER JOIN SqLineItems ON SqLineItems.HeaderId=SqHeader.id WHERE SqHeader.id=@DocNum">
            <SelectParameters>
                <asp:Parameter Name="DocNum" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
       
</asp:Content>

