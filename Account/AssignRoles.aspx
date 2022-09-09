<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="AssignRoles.aspx.vb" Inherits="Account_AssignRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Assign Roles</h1>
 <asp:Panel ID="Panel1" runat="server"   >
   <table>
        <tr>
        <td>Select a User:</td>
        <td><asp:listbox ID="UserList" runat="server" AutoPostBack="True"           
                DataTextField="UserName" DataValueField="UserName" Rows="1">      </asp:listbox> </td>
        </tr>
        <tr>
        <td>Roles</td>
         <td>
         <asp:Repeater ID="UsersRoleList" runat="server"> 
           <ItemTemplate>
           <asp:CheckBox runat="server" ID="RoleCheckBox" AutoPostBack="false"                     Text='<%# Container.DataItem %>' /><br />
           </ItemTemplate>   
           
              </asp:Repeater></td></tr>
              <tr><td colspan="2"><asp:Button runat="server" ID="btnAssignRoles" Text="Assign Roles" /></td></tr>
              </table>

    </asp:Panel>
    <asp:Label  runat="server" ID="LblSuccess"></asp:Label>
</asp:Content>

