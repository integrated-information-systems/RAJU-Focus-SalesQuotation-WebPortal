<%@ Page Title="Register" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeFile="Register.aspx.vb" Inherits="Account_Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2> Add User</h2>
 <div align="left" style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;">
    <asp:CreateUserWizard ID="CreateUserWizard1" Runat="server" 
    LoginCreatedUser="False" Width="336px" 
         ContinueDestinationPageUrl="~/Account/Register.aspx">
  <WizardSteps>
    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
     <ContentTemplate>
        <table border="0" style="font-size: 13px;Width:100%; font-family: Arial">
         
            <tr>
                <td align="left">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">
                        User Name:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                        Password:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">
                        Confirm Password:</asp:Label></td>
                <td class="style1">
                    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                        ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
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
                
                    <asp:ListBox  ID="Territory" runat="server" DataSourceID="SqlDataSource1" 
                        DataTextField="descript" DataValueField="territryID" 
                        SelectionMode="Multiple">
                    </asp:ListBox >
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
                        ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                        ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                        ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="color: red">
                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                </td>
            </tr>
        </table>
     
    </ContentTemplate>
    </asp:CreateUserWizardStep>
    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
    </asp:CompleteWizardStep>
  </WizardSteps>
</asp:CreateUserWizard>
</div>    
</asp:Content>