<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Focus Garment Tech Pte Ltd!
    </h2>
     <h3>Announcements</h3>
    <div > 
<marquee  style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;" onMouseOver="this.scrollAmount=0" onMouseOut="this.scrollAmount=2" bgcolor="#EEEEEE" scrollamount="2" direction="up" loop="true" width="30%" height="150">
<center>
<asp:Label ID="lblMessages" runat="server" Text=""></asp:Label>
</center></marquee>
</div>

</asp:Content>