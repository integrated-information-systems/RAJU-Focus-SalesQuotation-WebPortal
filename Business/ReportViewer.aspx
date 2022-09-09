<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ReportViewer.aspx.vb" Inherits="Business_ReportViewer" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>       
 <script language="javascript" >
 function Print() {
    var dvReport = document.getElementById("dvReport");
    var frame1 = dvReport.getElementsByTagName("iframe")[0];
    if (navigator.appName.indexOf("Internet Explorer") != -1) {
        frame1.name = frame1.id;
        window.frames[frame1.id].focus();
        window.frames[frame1.id].print();
    }
    else {
        var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
        frameDoc.print();
    }
}
 </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <%--<input type="button" id="btnPrint" value="Print" onclick="Print()" />--%>
 <div id="dvReport">
   <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" BestFitPage="True" PrintMode="ActiveX" />
 </div>
</asp:Content>

