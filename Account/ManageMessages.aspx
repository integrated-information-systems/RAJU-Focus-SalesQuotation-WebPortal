<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ManageMessages.aspx.vb" Inherits="ManageMessages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Manage Messages</h1>
<table>
<tr>
    <td>Title</td>
     <td>
        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
      <asp:RegularExpressionValidator ID="txtTitleRegularExpressionValidator" runat="server" Display="Dynamic" ErrorMessage="Max 50 characters only" Text=""  ForeColor="Red" ControlToValidate="txtTitle" ValidationExpression="^[\s\S]{0,50}$" ValidationGroup="1"></asp:RegularExpressionValidator>
      <asp:RequiredFieldValidator
            ID="txtTitleRequiredValidator" runat="server" Display="Dynamic" ErrorMessage="Required" Text=""  ForeColor="Red" ControlToValidate="txtTitle" ValidationGroup="1"></asp:RequiredFieldValidator>
    </td>
    
</tr>
<tr>
    <td>Message</td>
    <td>
        <asp:TextBox ID="txtMessage" runat="server" Rows="5" TextMode="MultiLine" 
            Width="371px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="txtMessageRegularExpressionValidator" runat="server" Display="Dynamic" ErrorMessage="Max 250 characters only" Text=""  ForeColor="Red" ControlToValidate="txtMessage" ValidationExpression="^[\s\S]{0,250}$" ValidationGroup="1"></asp:RegularExpressionValidator>
      <asp:RequiredFieldValidator
            ID="txtMessageRequiredValidator" runat="server" Display="Dynamic" ErrorMessage="Required" Text=""  ForeColor="Red" ControlToValidate="txtMessage" ValidationGroup="1"></asp:RequiredFieldValidator>
    </td>   
</tr>
<tr>
    <td>Status</td>
    <td>
        <asp:RadioButtonList ID="rdoStatusList" runat="server">
        <asp:ListItem Text="Active" Value="True" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Inactive" Value="False"></asp:ListItem>
        </asp:RadioButtonList>
    </td>   
</tr>
<tr>
    <td>&nbsp;</td>
    <td>
        <asp:Button ID="btnAdd" runat="server" Text="Add" ValidationGroup="1" />
    </td>   
</tr>
<tr>
<td colspan="2"><asp:Label runat="server" id="SuccessMsg"  Text=""></asp:Label></td>

</tr>
</table>
<h2>Messages List</h2>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333"  Width="60%" 
            GridLines="None"  EmptyDataText="No Record Found" ShowHeaderWhenEmpty="True" >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
          
               <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>                               
                                   <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%#Eval("id").ToString  %>'
                                            CommandName="CustomDelete" Text="Delete" />
                                </ItemTemplate>
                 </asp:TemplateField>    
                      <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>                               
                                   <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("id").ToString  %>'
                                            CommandName="CustomEdit" Text="Select" />
                                </ItemTemplate>
                 </asp:TemplateField>    
                <asp:BoundField DataField="Title" HeaderText="Title" 
                    SortExpression="Title" />
                <asp:BoundField DataField="Message" HeaderText="Message"  ItemStyle-Width="20%"

                    SortExpression="Message" />
                <asp:BoundField DataField="Active" HeaderText="Active" 
                    SortExpression="Active" />                
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
            SelectCommand="SELECT * From Messages">
        </asp:SqlDataSource>

</asp:Content>

