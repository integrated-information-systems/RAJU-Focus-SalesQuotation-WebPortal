<%@ Page Title="Manage Roles" Language="VB"  MasterPageFile="~/Site.Master" AutoEventWireup="false"  CodeFile="ManageRoles.aspx.vb" Inherits="Account_ManageRoles" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
      <h2> Create Roles</h2>
      <asp:Panel runat="server" ID="SuccessMsg" Visible="False">
    <label id="Label1" runat="server" style="color: #00FF00">Role Added Successfully.</label>
    </asp:Panel>
      <asp:Panel ID="FormFields" runat="server" >
            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Group1" runat="server" HeaderText="Following error occurs:" ShowMessageBox="false" DisplayMode="BulletList" ShowSummary="true"  />
            <table class="style1">
            <tr><td valign="top">
          <table class="style1">
              <tr>
             
                  <td >
                     RoleName</td>
                  <td>
                      <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
                      <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtRoleName"
                          ErrorMessage="Required" SetFocusOnError="true" Text="*" ValidationGroup="Group1"></asp:RequiredFieldValidator>
                          <asp:RegularExpressionValidator
                              ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Invalid RoleName" Text="*" ControlToValidate="txtRoleName" ValidationExpression="^[a-zA-Z'.\s]{1,40}$" ValidationGroup="Group1"></asp:RegularExpressionValidator>
                      <asp:CustomValidator ID="CustomValidator1" runat="server" 
                          ErrorMessage="Aleary Exist" Text="*"   Display="Dynamic" ControlToValidate="txtRoleName" OnServerValidate="CheckExist" ValidationGroup="Group1"  ></asp:CustomValidator>
                      </td>
              </tr>
              <tr>
                  
                  <td>
                      &nbsp;</td>
                  <td>
                    <asp:Button ID="submit" runat="server" Text="Create" ValidationGroup="Group1" /></td>
              </tr>
          </table>
          </td><td>
          <asp:GridView runat="server" CellPadding="4" id="RolesGrid" 
                Gridlines="None" AutoGenerateColumns="False" ForeColor="#333333" >
              <EditRowStyle BackColor="#999999" />
              <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" ForeColor="white" Font-Bold="True" />
              <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
              <Columns>
                  <asp:TemplateField HeaderText="Available Roles">
                      <ItemTemplate>
                          <%# Container.DataItem.ToString() %>
                      </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
              <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
              <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
              <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
              <SortedAscendingCellStyle BackColor="#E9E7E2" />
              <SortedAscendingHeaderStyle BackColor="#506C8C" />
              <SortedDescendingCellStyle BackColor="#FFFDF8" />
              <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
   </asp:GridView></td></tr></table>
    </asp:Panel>
   
</asp:Content>