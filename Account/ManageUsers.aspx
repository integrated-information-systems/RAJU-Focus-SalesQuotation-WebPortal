<%@ Page Title="Manage Users" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ManageUsers.aspx.vb" Inherits="Account_ManageUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Manage Users</h2>
<div style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;">
<table width="100%" border="0"><tr><td width="50%">
<table border="0" style="font-size: 13px;Width:100%; font-family: Arial">
         
            <tr>
                <td align="left">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">
                        User Name:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="UserName" runat="server" ReadOnly="True"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                        Password:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" Enabled="False"></asp:TextBox>
                  <%--  <asp:RequiredFieldValidator ID="PasswordRequired"  runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>--%>
                         <asp:RegularExpressionValidator Display = "static"  ControlToValidate = "Password" ID="RegularExpressionValidator3" ValidationGroup="CreateUserWizard1" ValidationExpression = "^[\s\S]{6,}$" runat="server" ErrorMessage="Min 6 and characters required."></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">
                        Confirm Password:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" Enabled="False"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                        ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="SalesPersonNameLabel" runat="server" AssociatedControlID="SalesPersonName">
                        Sales Person Name:</asp:Label></td>
                <td class="style1">
                    <asp:DropDownList ID="SalesPersonName" runat="server" 
                        DataSourceID="SqlDataSource2" DataTextField="SlpName" DataValueField="SlpCode">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                        SelectCommand="SELECT [SlpCode], [SlpName] FROM [OSLP] WHERE SlpCode!=-1"></asp:SqlDataSource>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SalesPersonName"
                        ErrorMessage="Sales Person Name is required." ToolTip="Sales Person Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
             <tr>
                <td align="left">
                    <asp:Label ID="TerritoryLabel" runat="server" AssociatedControlID="Territory">
                       Territory:</asp:Label></td>
                <td class="style1">
                   
                    <asp:listbox ID="Territory" runat="server" DataSourceID="SqlDataSource1" 
                        DataTextField="descript" DataValueField="territryID" 
                        SelectionMode="Multiple">
                    </asp:listbox>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                        SelectCommand="SELECT [territryID], [descript] FROM [OTER]">
                    </asp:SqlDataSource>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Territory"
                        ErrorMessage="Territory is required." ToolTip="Territory is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">
                        E-mail:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                        ErrorMessage="E-mail is required." ToolTip="E-mail is required." Display="Static" ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                        ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                        ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                </td>
            </tr>          
            <tr><td> 
                <asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="CreateUserWizard1" /></td></tr>
            <tr><td>
                <asp:Label ID="SuccessMsg" runat="server" Text=""></asp:Label></td></tr>
        </table>
      </td><td valign="top">
     
       <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"
        GridLines="None" AutoGenerateDeleteButton="False" 
        AutoGenerateSelectButton="True" onrowdeleting="GridView1_RowDeleting">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
        
         <Columns>
<asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="true"/>
<asp:BoundField DataField="Email" HeaderText="Email" />
</Columns>
    </asp:GridView>
        <asp:Label ID="LblResult" runat="server" Text=""></asp:Label>
     </td></tr></table>
       </div>
    
</asp:Content>

