<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="PriceList.aspx.vb" Inherits="Business_PriceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
    <style type="text/css">
        .style1
        {
            width: 117px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Last Prices Report</h2>
 <div style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;" 
        >
<table>

<tr><td>Customer Name</td><td>
    <asp:TextBox ID="txtBPName" runat="server"></asp:TextBox><asp:CustomValidator ID="txtBPNameValidator" runat="server" ForeColor="Red"  ControlToValidate="txtBPName"
                        ErrorMessage="Invalid Customer Name" OnServerValidate="ValidateBPName" ValidationGroup="1"></asp:CustomValidator></td>
    <td class="style1">&nbsp;&nbsp;&nbsp; Item Name</td><td>
        <asp:TextBox ID="txtItemName" runat="server"></asp:TextBox></td><td> &nbsp;&nbsp;Territory</td><td> 
    <asp:DropDownList ID="ddlTerritory" runat="server" >
    </asp:DropDownList>
    
    </td><td><asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="1" /></td></tr>
</table>
</div>
<div style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;" 
        align="center">
        <center> <b style="font-size: large">Last Ten Prices Report</b></center>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1" CellPadding="4" 
        EmptyDataText="No Record Found" ForeColor="#333333" GridLines="None"  HorizontalAlign="Center"
        Width="100%" ShowHeaderWhenEmpty="True" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
         <asp:CommandField  HeaderText="Details" SelectText="View" ShowSelectButton="true"  
                SortExpression="Document #" />
            <asp:BoundField  HeaderText="Document #"  DataField="Document #" 
                SortExpression="Document #" />
            <asp:BoundField  HeaderText="DocType"  DataField="objtype"  Visible="true"
                SortExpression="objtype" />
            <asp:BoundField  HeaderText="Customer Name"  DataField="CardName" 
                SortExpression="CardName" />
            <asp:BoundField DataField="Date" HeaderText="Date"  DataFormatString="{0:dd/MM/yy}"
                SortExpression="Date" />
                <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity"  DataFormatString="{0:0.00}"
                SortExpression="Quantity" />
            <asp:BoundField DataField="Cust Price" HeaderText="Cust Price"  DataFormatString="{0:0.00}"
                SortExpression="Cust Price" />
            <asp:BoundField DataField="N.I Price" HeaderText="N.I Price"  DataFormatString="{0:0.00}"
                SortExpression="N.I Price" />
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
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
        SelectCommand="SELECT Top 5  OQUT.ObjType, OQUT.DocNum AS 'Document #', OQUT.CardName, OQUT.DocDate as 'Date' ,QUT1.Quantity,QUT1.Currency,QUT1.Price as 'Cust Price',QUT1.U_PRICE1 as 'N.I Price',QUT1.DiscPrcnt as 'Discount',(QUT1.Price-((QUT1.Price*QUT1.DiscPrcnt)/100)) as 'Price After Discount',  QUT1.Dscription as 'Description' FROM OQUT INNER JOIN QUT1 ON OQUT.DocEntry = QUT1.DocEntry  INNER JOIN  OCRD ON OCRD.CardCode=OQUT.CardCode  WHERE OCRD.Territory=@Territory AND QUT1.ItemCode=@ItemCode AND  OQUT.CardName  LIKE @CardName  UNION SELECT Top 5  ORDR.OBJTYPE, ORDR.DocNum AS 'Document #', ORDR.CardName, ORDR.DocDate as 'Date' ,RDR1.Quantity,RDR1.Currency,RDR1.Price as 'Cust Price',RDR1.U_PRICE1 as 'N.I Price',RDR1.DiscPrcnt as 'Discount',(RDR1.Price-((RDR1.Price*RDR1.DiscPrcnt)/100)) as 'Price After Discount',  RDR1.Dscription as 'Description' FROM ORDR INNER JOIN RDR1 ON ORDR.DocEntry = RDR1.DocEntry  INNER JOIN  OCRD ON OCRD.CardCode=ORDR.CardCode WHERE OCRD.Territory=@Territory AND RDR1.ItemCode=@ItemCode AND  ORDR.CardName  LIKE @CardName ORDER BY 'Date' DESC ">

    <SelectParameters>
    <asp:Parameter Name="CardName" DefaultValue=""  /> 
    <asp:Parameter Name="ItemCode" DefaultValue=""  /> 
       <asp:Parameter Name="Territory" DefaultValue=""  /> 
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
            ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
            SelectCommand="select OQUT.DocNum AS 'Document #',QUT1.Dscription as 'Model',QUT1.Quantity,QUT1.Price AS 'Cust Price', QUT1.U_PRICE1 AS 'N.I Price', QUT1.DiscPrcnt AS 'Discount',(QUT1.Price-((QUT1.Price*QUT1.DiscPrcnt)/100)) as 'Price After Discount'  from  OQUT INNER JOIN QUT1 ON QUT1.DocEntry=OQUT.DocEntry WHERE OQUT.DocNum=@DocNum">
            <SelectParameters>
                <asp:Parameter Name="DocNum" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>
</asp:Content>

