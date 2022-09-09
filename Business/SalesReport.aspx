<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="SalesReport.aspx.vb" Inherits="Business_SalesReport" %>
 

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link href="../Styles/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Panel ID="FormFields" runat="server" >
<h2>Sales Report</h2>
<div  style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;">
<table style="width:50%">
<tr><td>From Date</td><td><asp:TextBox runat="server" ID="FromDate"></asp:TextBox></td><td>To Date</td><td><asp:TextBox runat="server" ID="ToDate"></asp:TextBox></td></tr>
<tr><td>Concluded?</td><td><asp:DropDownList runat="server" ID="Concluded"><asp:ListItem Text="Selected" Value="0"></asp:ListItem><asp:ListItem Text="Yes" Value="Y"></asp:ListItem><asp:ListItem Text="No" Value="N"></asp:ListItem></asp:DropDownList> </td></tr>
<tr><td>&nbsp;</td></tr>
<tr><td></td><td><asp:Button runat="server" ID="BtnSearch" Text="Search" />&nbsp;<asp:Button runat="server" ID="btnClear" Text="Clear" />&nbsp;<asp:Button runat="server" ID="btnExportExcel" Text="Export to Excel" /></td></tr>
</table>
</div>
<div  style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;">
<asp:GridView ID="GridView1" runat="server" CellPadding="4" 
        EmptyDataText="No Records Added" ForeColor="#333333" GridLines="None" 
        ShowHeaderWhenEmpty="True" Width="100%"  
        AutoGenerateColumns="false"
        HorizontalAlign="Center"  >
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <EditRowStyle BackColor="#999999" />
    <EmptyDataRowStyle BackColor="#00CCFF" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    <Columns>   
         <asp:BoundField DataField="SNo" HeaderText="SNo" ReadOnly="True" ItemStyle-HorizontalAlign="Left" />  
         <asp:BoundField DataField="DocDate" HeaderText="Doc Date" ReadOnly="True" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Left"  />  
         <asp:BoundField DataField="CardName" HeaderText="Customer Name" ReadOnly="True" ItemStyle-HorizontalAlign="Left" />  
         <asp:BoundField DataField="QT-Number" HeaderText="QT-Number" ReadOnly="True" ItemStyle-HorizontalAlign="Left"  />  
         <asp:BoundField DataField="Amount" HeaderText="Amount(USD)" ReadOnly="True" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />           
         <asp:BoundField DataField="U_CHANCE" HeaderText="Chance" ReadOnly="True" ItemStyle-HorizontalAlign="Left"  />          
         <asp:BoundField DataField="U_QUOTE_REMARKS" HeaderText="Remarks" ReadOnly="True" ItemStyle-HorizontalAlign="Left"  />
         <asp:BoundField DataField="U_CONCLUDED" HeaderText="Concluded?" ReadOnly="True" ItemStyle-HorizontalAlign="Left"  />  
    </Columns>
</asp:GridView>
</div>
</asp:Panel>
</asp:Content>

