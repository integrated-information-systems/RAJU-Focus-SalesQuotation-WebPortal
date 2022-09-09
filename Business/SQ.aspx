<%@ Page Title="Sales Quotation" MaintainScrollPositionOnPostback="true" EnableEventValidation="true" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="SQ.aspx.vb" Inherits="Business_SQ" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 124px;
        }
        .style5
        {
            text-align: center;
        }
        .style12
        {
            text-align: center;
            width: 85px;
        }
        .style14
        {
            text-align: center;
            width: 122px;
        }
        .style20
        {
            text-align: center;
            width: 24px;
        }
        .style24
        {
            text-align: center;
            width: 20px;
        }
        .style25
        {
            width: 41px;
        }
        .style48
        {
            width: 85px;
        }
        .style55
        {
            width: 122px;
        }
        .style58
        {
            text-align: center;
            width: 38px;
        }
        .style63
        {
            width: 183px;
        }
        .style64
        {
            text-align: center;
            width: 70px;
        }
        .style65
        {
            width: 70px;
        }
        .style67
        {
            width: 106px;
        }
        .style68
        {
            width: 38px;
        }
        .style74
        {
            text-align: center;
            width: 61px;
        }
        .style75
        {
            width: 61px;
        }
        .style78
        {
            text-align: center;
            width: 35px;
        }
        .style79
        {
            width: 35px;
        }
        .style81
        {
            width: 27px;
        }
        .style85
        {
            width: 261px;
        }
        .style96
        {
            width: 349px;
        }
        .style97
        {
            width: 83px;
        }
        .style98
        {
            width: 97px;
        }
        .style99
        {
            width: 774px;
        }
        .style106
        {
            text-align: center;
            width: 76px;
        }
        .style107
        {
            width: 76px;
        }
        .style108
        {
            width: 20px;
        }
        .style111
        {
            text-align: center;
            width: 65px;
        }
        .style112
        {
            width: 65px;
        }
        .style113
        {
            width: 24px;
        }
        .style114
        {
            text-align: center;
            width: 66px;
        }
        .style115
        {
            width: 66px;
        }
        .style116
        {
            text-align: center;
            width: 71px;
        }
        .style117
        {
            width: 71px;
        }
        .style118
        {
            text-align: center;
            width: 58px;
        }
        .style119
        {
            width: 58px;
        }
    </style>
     <link href="../Styles/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">



    <asp:Panel ID="FormFields" runat="server" >
    <h2>Sales Quotation</h2>
       <center><asp:Label ID="LblHeaderMsg" runat="server" Text="" ></asp:Label></center>
      <div  style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;">

    <table class="style1">
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style96">
          
                      &nbsp;</td>
            <td class="style97">
                Internal DocNo
            </td>
            <td class="style85">
             <asp:TextBox ID="txtDocNo" runat="server" Width="46px" MaxLength="20"></asp:TextBox> <asp:RegularExpressionValidator
                              ID="txtDocNoValidator" runat="server" Display="Dynamic" ErrorMessage="Invalid" Text=""  ForeColor="Red" ControlToValidate="txtDocNo" ValidationExpression="^\d+\d*$" ValidationGroup="2"></asp:RegularExpressionValidator>
              SAP DocNo<asp:TextBox ID="txtSAPDocNo" runat="server" Width="49px" MaxLength="20"></asp:TextBox> <asp:RegularExpressionValidator
                              ID="txtSAPDocNoValidator" runat="server" Display="Dynamic" ErrorMessage="Invalid" Text=""  ForeColor="Red" ControlToValidate="txtSAPDocNo" ValidationExpression="^\d+\d*$" ValidationGroup="2"></asp:RegularExpressionValidator>
                <asp:Button ID="BtnFindDocNo" runat="server" Text="Find" style="height: 26px" ValidationGroup="2" /></td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style96">
                &nbsp;</td>
            <td class="style97">
                Revision No</td>
            <td class="style85">
                <asp:TextBox ID="txtRevisionNo" runat="server" MaxLength="15"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Customer Code</td>
            <td class="style96">
              
                <asp:TextBox ID="txtBPCode" runat="server" style="margin-left: 0px" 
                    Width="311px" MaxLength="100"></asp:TextBox> <asp:CustomValidator ID="txtBPCodeValidator" runat="server" ForeColor="Red" ValidateEmptyText="true" ControlToValidate="txtBPCode"
                        ErrorMessage="Invalid Customer Code" OnServerValidate="ValidateBPCode" ValidationGroup="1"></asp:CustomValidator>
            </td>
            <td class="style97">
                SAP Document No</td>
            <td class="style85">
                <asp:Label ID="lblDocNo" runat="server" Text=""></asp:Label>
               
                &nbsp;Internal DocNo:&nbsp;<asp:Label ID="LblIntDocNo" runat="server" Text=""></asp:Label>
               </td>
        </tr>
        <tr>
            <td class="style2">
                Customer Name</td>
            <td class="style96">
                      <asp:TextBox ID="txtBPName" runat="server" style="margin-left: 0px" 
                    Width="311px" MaxLength="100"></asp:TextBox> <asp:CustomValidator ID="txtBPNameValidator" ForeColor="Red" runat="server" ValidateEmptyText="true" ControlToValidate="txtBPName"
                        ErrorMessage="Invalid Customer Name" OnServerValidate="ValidateBPName" ValidationGroup="1"></asp:CustomValidator>
                    </td>
            <td class="style97">
                Status</td>
            <td class="style85">
                <asp:Label ID="LblStatus" runat="server" Text="Open"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Address ID</td>
            <td class="style96">
                <asp:ListBox ID="LstAddressId" runat="server" Height="21px" Rows="1" 
                    Width="317px" AutoPostBack="True"></asp:ListBox>  <asp:CustomValidator ID="LstAddressIdValidator" ForeColor="Red" runat="server" ValidateEmptyText="true" ControlToValidate="LstAddressId"
                        ErrorMessage="Invalid Address ID" OnServerValidate="ValidateBPAddress" ValidationGroup="1"></asp:CustomValidator>
            </td>
            <td class="style97">
                Contact Person</td>
            <td class="style85">
               <asp:ListBox ID="LstCtPrsn" runat="server" Height="21px" Rows="1" Width="250px" 
                    AutoPostBack="True">            
                </asp:ListBox> <asp:CustomValidator ID="LstCtPrsnValidator" ForeColor="Red" runat="server" ValidateEmptyText="false" ControlToValidate="LstCtPrsn"
                        ErrorMessage="Invalid Contact Person ID" OnServerValidate="ValidateBPContactPerson" ValidationGroup="1"></asp:CustomValidator>         </td>
        </tr>
        <tr>
            <td class="style2">
                Address Details:</td>
            <td class="style96">
                <asp:Label ID="lblAddressDetails" runat="server" Height="100px" Width="317px" 
                    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
            </td>
            <td class="style97">
                &nbsp;</td>
            <td class="style85">
                 <asp:Label ID="lblContactDetails" runat="server" BorderColor="#CCCCCC" 
                     BorderStyle="Solid" BorderWidth="1px" Height="100px" Width="250px"></asp:Label>
                 </td>
        </tr>
        <tr>
            <td class="style2">
                Tracking ID</td>
            <td class="style96">
               
                <asp:TextBox ID="txtTrackingId" runat="server" MaxLength="100" ></asp:TextBox>
               
            </td>
            <td class="style97">
                Posting Date</td>
            <td class="style85">
                <asp:TextBox ID="txtPostingDate" runat="server" AutoPostBack="True" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Customer Ref No</td>
            <td class="style96">
                
                <asp:TextBox ID="txtBPRefNo" runat="server" style="margin-left: 0px" 
                    Width="311px" AutoPostBack="True" MaxLength="100"></asp:TextBox>
            </td>
            <td class="style97">
                Valid Until</td>
            <td class="style85">
                <asp:TextBox ID="txtValidUntil" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:ListBox ID="LstCurrencySelector" runat="server" Height="21px" Rows="1" 
                    Width="125px" AutoPostBack="True"></asp:ListBox>
                  <asp:HiddenField ID="BC" runat="server" />
                <asp:HiddenField ID="LC" runat="server" />
                <asp:HiddenField ID="SC" runat="server" />
            </td>
            <td class="style96">
                <asp:DropDownList ID="ddlDocCurrency" runat="server" AutoPostBack="True">
                </asp:DropDownList> 
                <asp:Label ID="lblCurExRate" runat="server" Text="Ex Rate"></asp:Label>
                <asp:TextBox ID="txtCurExRate" runat="server" Width="55px" AutoPostBack="true" 
                    ReadOnly="False"   MaxLength="10"></asp:TextBox> 
                <asp:RequiredFieldValidator ID="DocCurrencyValidator" runat="server" ForeColor="Red" ErrorMessage="" Text="Contact adminstrator to update the exchange rate" ControlToValidate="txtCurExRate" Display="Dynamic"   ValidationGroup="1"></asp:RequiredFieldValidator>
                
            </td>
            <td class="style97">
                Document Date</td>
            <td class="style85">
                <asp:TextBox ID="txtDocDate" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
     
        <tr>
            <td class="style2">
                Delivery Term</td>
            <td class="style96">
                <asp:ListBox ID="LstDeliveryTerm" runat="server" DataSourceID="SqlDataSource1" 
                    DataTextField="Value" DataValueField="Value" Rows="1"></asp:ListBox>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [Value] FROM [CUVV] WHERE ([IndexID] = @IndexID)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="23" Name="IndexID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td class="style97">
                Valid For(Days)</td>
            <td class="style85">
                <asp:TextBox ID="txtValidForDays" runat="server" MaxLength="2">14</asp:TextBox> 
                <asp:CustomValidator ID="txtValidForDaysValidator" ControlToValidate="txtValidForDays" ForeColor="Red" OnServerValidate="ValidateNumericOnly" ValidateEmptyText="true"  runat="server" ErrorMessage="InValid" Display="Dynamic" ValidationGroup="1" ></asp:CustomValidator>
                </td>
        </tr>
        <tr>
            <td class="style2">
                Ship From</td>
            <td class="style96">
                 <asp:ListBox ID="LstShipFrom" runat="server" DataSourceID="SqlDataSource2" 
                    DataTextField="Value" DataValueField="Value" Rows="1"></asp:ListBox>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [Value] FROM [CUVV] WHERE ([IndexID] = @IndexID)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="26" Name="IndexID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td class="style97">
                Ship to</td>
            <td class="style85">
                 <asp:ListBox ID="LstShipTo" runat="server" DataSourceID="SqlDataSource3" 
                    DataTextField="Value" DataValueField="Value" Rows="1"></asp:ListBox>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [Value] FROM [CUVV] WHERE ([IndexID] = @IndexID)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="27" Name="IndexID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
     
        <tr>
            <td class="style2">
               Indenter</td>
            <td class="style96">
                <asp:TextBox ID="txtEndUser" runat="server" Width="519px" MaxLength="500"></asp:TextBox>
            </td>
            <td class="style97">
                Delivery Time</td>
            <td class="style85">
                <asp:ListBox ID="LstDeliveryTime" runat="server" datasourceid="SqlDataSource4" 
                    DataTextField="Value" DataValueField="Value" Rows="1"></asp:ListBox>
                <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [Value] FROM [CUVV] WHERE ([IndexID] = @IndexID)">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="22" Name="IndexID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
     
        <tr>
            <td class="style2">
                Payment Terms</td>
            <td class="style96">
                <asp:DropDownList ID="ddlPaymentterm" runat="server" 
                    DataSourceID="SqlDataSource5" DataTextField="PymntGroup" 
                    DataValueField="GroupNum">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [GroupNum], [PymntGroup] FROM [OCTG]">
                </asp:SqlDataSource>
            </td>
            <td class="style97">
                H.S.Code</td>
            <td class="style85">
                <asp:TextBox ID="txtHSCode" runat="server" Width="331px" MaxLength="500"></asp:TextBox>
            </td>

        </tr>
        
     
        <tr>
            <td class="style2">
                Minimum Charges</td>
            <td class="style96">
                <asp:ListBox ID="LstMinChrge" runat="server" Height="29px" Rows="1" 
                    Width="222px" DataSourceID="SqlDataSource7" DataTextField="Code" 
                    DataValueField="Name" AppendDataBoundItems="true">
                    <asp:ListItem>Select </asp:ListItem>
                </asp:ListBox>
                <asp:SqlDataSource ID="SqlDataSource7" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [Code], [Name] FROM [@MIN_CHARGES]">
                </asp:SqlDataSource>
            </td>
            <td class="style2">
                Chance</td>
            <td class="style96">
                <asp:DropDownList ID="ddlChance" runat="server" 
                    >
                    <asp:ListItem Text="High" Value="High" Selected="True"></asp:ListItem>                    
                    <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>                    
                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>                    
                </asp:DropDownList>                 
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>Concluded</td>
            <td><asp:DropDownList ID="ddlConcluded" runat="server" 
                    >
                    <asp:ListItem Text="N" Value="N" Selected="True"></asp:ListItem>                    
                    <asp:ListItem Text="Y" Value="Y"></asp:ListItem>                                        
                </asp:DropDownList>   </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="style97">
                Email</td>
            <td class="style85">
                <asp:TextBox ID="txtEmailSQ" runat="server" Width="331px" MaxLength="500" readonly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style96">
		&nbsp;
            </td>
            <td class="style2">
                Address</td>
            <td class="style96">
		<asp:TextBox ID="txtAddressSQ" runat="server" Width="331px" MaxLength="500" readonly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
            	&nbsp;
	    </td>
            <td class="style96">
		&nbsp;
            </td>
            <td class="style97">
                Remarks</td>
            <td class="style85">
                <asp:TextBox ID="txtRemarksSQ" runat="server" Width="331px" MaxLength="500" readonly="True"></asp:TextBox></td>
        </tr>    
    </table>
      
      </div>
     
     
    <div  style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;">
    
    <table class="style1" frame="border" border="1" 
        style="padding: inherit; border-collapse: collapse; color: #FFFFFF; font-weight: bold;">
        <tr>
            <td class="style12" bgcolor="#5D7B9D">
                Item Code</td>                
            <td class="style14" bgcolor="#5D7B9D">
                Model</td>
            <td class="style64" bgcolor="#5D7B9D">
               Description</td>
            <td class="style74" bgcolor="#5D7B9D">
                Line Text</td>
             <td bgcolor="#5D7B9D" class="style116">
                 Country</td>
             <td bgcolor="#5D7B9D" class="style118">
                 Currency</td>
             <td class="style106" bgcolor="#5D7B9D" >
                 Cust.Price</td>
            <td class="style114" bgcolor="#5D7B9D">
                N.I.Price</td>
            <td class="style58" bgcolor="#5D7B9D">
                Qty</td>
            <td class="style20" bgcolor="#5D7B9D">
                Disc %</td>
            <td class="style78" bgcolor="#5D7B9D">
                Tax</td>         
            <td class="style12" bgcolor="#5D7B9D">
                Whouse</td>                     
               <td class="style74" bgcolor="#5D7B9D">
                Stock</td>
            <td class="style111" bgcolor="#5D7B9D">
                Print</td>
            <td class="style24" bgcolor="#5D7B9D">
               Add/Update</td>
            <td class="style5" bgcolor="#5D7B9D">
               Delete</td>

        </tr>
        <tr>
            <td class="style48">
                <asp:TextBox ID="txtItemCode" runat="server" Width="87px" MaxLength="100"></asp:TextBox>
            </td>
            <td class="style55">
                <asp:TextBox ID="txtItemName" runat="server" Width="135px" Height="22px" MaxLength="100"></asp:TextBox>
            </td>
            <td class="style67">
             <asp:TextBox ID="txtRemarks" runat="server" Width="132px" MaxLength="500"></asp:TextBox>
            </td>
            <td class="style75">                
                   <asp:TextBox ID="txtLintText" runat="server" Width="130px" MaxLength="500"></asp:TextBox>
            </td>
            <td class="style117">
                <asp:TextBox ID="txtCountry" runat="server" Width="55px" MaxLength="100"></asp:TextBox></td>
            <td class="style119">
                <asp:DropDownList ID="ddlPriceCurrency" runat="server" 
                    DataSourceID="SqlDataSource6" DataTextField="CurrCode" 
                    DataValueField="CurrCode" Height="26px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource6" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="SELECT [CurrCode] FROM [OCRN]"></asp:SqlDataSource>
            </td>
            <td class="style107">
                <asp:TextBox ID="txtItemPrice" runat="server" Width="71px" MaxLength="15"></asp:TextBox>
            </td>
             <td class="style115">
                <asp:TextBox ID="txtNIPrice" runat="server" Width="71px" MaxLength="100"></asp:TextBox>
            </td>
            <td class="style68">
                <asp:TextBox ID="txtItemQty" runat="server" Width="32px" MaxLength="15">1</asp:TextBox>
            </td>
            <td class="style113">
                <asp:TextBox ID="txtItemDiscount" runat="server" Width="32px" MaxLength="15">0</asp:TextBox>
            </td>
            <td class="style79">
                <asp:DropDownList ID="ddlTaxCode" runat="server" Height="26px" Width="80px" 
                    style="margin-bottom: 0px">
                </asp:DropDownList>
            </td>
            <td class="style48">
                <asp:DropDownList ID="ddlWarehouse" runat="server" Height="26px" Width="166px">
                </asp:DropDownList>
            </td>       
             <td class="style75">
                <asp:DropDownList ID="ddlStock" runat="server" Height="26px" Width="81px">
                </asp:DropDownList>
            </td>
             <td class="style112">
                <asp:DropDownList ID="ddlPrint" runat="server" Height="26px" Width="65px">
                </asp:DropDownList>
            </td>
            <td align="center" valign="middle" class="style108">
                <asp:Button ID="btnAction" runat="server" Text="Add" ValidationGroup="Group2" 
                    style="height: 26px" />
            </td>
            <td align="center" valign="middle">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" ValidationGroup="3" />
            </td>

        </tr>
        <tr>
            <td class="style48" align="center">
               <asp:CustomValidator ID="txtItemCodeValidator" runat="server" ForeColor="Red" ValidateEmptyText="true" ControlToValidate="txtItemCode"
                        ErrorMessage="Invalid item " OnServerValidate="ValidateItemCode" ValidationGroup="Group2"></asp:CustomValidator>  
            </td>
            <td class="style55" align="center">

            </td>
            <td class="style65" align="center">
             
            </td>
            <td class="style75" align="center">
            
            </td>
            <td align="center" class="style117">
                &nbsp;</td>
            <td align="center" class="style119">
                &nbsp;</td>
            <td class="style107" align="center">
            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator7" runat="server"  ControlToValidate="txtItemPrice"
                          ErrorMessage="Required" SetFocusOnError="true" Text="*" ForeColor="Red" ValidationGroup="Group2"></asp:RequiredFieldValidator>  
                          <asp:RegularExpressionValidator
                              ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ErrorMessage="Invalid" Text=""  ForeColor="Red" ControlToValidate="txtItemPrice" ValidationExpression="^\d+\.?\d*$" ValidationGroup="Group2"></asp:RegularExpressionValidator>
              
            </td>
            <td class="style115" align="center">
               <%--<asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtNIPrice"   ErrorMessage="Required" SetFocusOnError="true" Text="*" ForeColor="Red" ValidationGroup="Group2"></asp:RequiredFieldValidator>  --%>
                          <asp:RegularExpressionValidator
                              ID="RegularExpressionValidator4" runat="server" Display="Dynamic" ErrorMessage="Invalid" Text=""  ForeColor="Red" ControlToValidate="txtNIPrice" ValidationExpression="^\d+\.?\d*$" ValidationGroup="Group2"></asp:RegularExpressionValidator>
            </td>
            <td class="style68" align="center">
            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator8" runat="server"  ControlToValidate="txtItemQty"
                          ErrorMessage="Required" SetFocusOnError="true" Text="*" ForeColor="Red" ValidationGroup="Group2"></asp:RequiredFieldValidator>  
                <asp:RegularExpressionValidator
                              ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Invalid" Text=""  ForeColor="Red" ControlToValidate="txtItemQty" ValidationExpression="^\d+\.?\d*$" ValidationGroup="Group2"></asp:RegularExpressionValidator>
              
            </td>
           <td class="style113" align="center">
               <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator9" runat="server"  ControlToValidate="txtItemDiscount"
                          ErrorMessage="Required" SetFocusOnError="true" Text="*" ForeColor="Red" ValidationGroup="Group2"></asp:RequiredFieldValidator>  
                          <asp:RegularExpressionValidator
                              ID="RegularExpressionValidator3" runat="server" Display="Dynamic" ErrorMessage="Invalid" Text=""  ForeColor="Red" ControlToValidate="txtItemDiscount" ValidationExpression="^\d+\.?\d*$" ValidationGroup="Group2"></asp:RegularExpressionValidator>
            </td>
            <td align="center" valign="middle" class="style25">
               
            </td>
            <td align="center" valign="middle" class="style81">
             
            </td>

        </tr>
    </table>
           </div>
      <center><asp:CompareValidator ID="CompareValidator1" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="Item Currency must me same as Doc Currency" ValidationGroup="Group2" ControlToValidate="ddlPriceCurrency" ControlToCompare="ddlDocCurrency"></asp:CompareValidator> <asp:CustomValidator ID="GridView1Validator" OnServerValidate="ValidateGrid" Display="Dynamic" ControlToValidate="txtDocDate" ValidationGroup="1" runat="server" ErrorMessage="No Items Added " ForeColor="Red"></asp:CustomValidator><asp:CustomValidator ID="ItemDeleteValidator" OnServerValidate="ValidateItemDelete" Display="Dynamic" ControlToValidate="txtDocDate" ValidationGroup="3" runat="server" ErrorMessage="Please select an item to delete " ForeColor="Red"></asp:CustomValidator></center>
    <div  style="background-color:#EEEEEE;border-color:#6699CC;border-width:1px;border-style:Solid;padding:20px;">
<asp:GridView ID="GridView1" runat="server" CellPadding="4" 
        EmptyDataText="No Records Added" ForeColor="#333333" GridLines="None" 
        ShowHeaderWhenEmpty="True" Width="100%" AutoGenerateSelectButton="True" 
        HorizontalAlign="Center"  >
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <EditRowStyle BackColor="#999999" />
    <EmptyDataRowStyle BackColor="#00CCFF" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
</div>
  
    <table class="style1">   
        <tr>
            <td class="style98">
                Sales Quotation Report Remarks</td>
            <td class="style99" rowspan="2">
                 <asp:TextBox ID="txtQuoteRemarks" runat="server" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
            </td>
            <td class="style63">
                &nbsp;</td>
            <td>
                &nbsp;
            </td>
        </tr>               
        <tr>
            <td class="style98">
                &nbsp;</td>
            <td class="style63" valign="middle">
                &nbsp;
                </td>
            <td>
                &nbsp;
            </td>
        </tr> 
        <tr>
            <td class="style98">
                Remarks</td>
            <td class="style99" rowspan="2" >
                <asp:TextBox ID="txtDocRemarks" runat="server" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
            </td>
            <td class="style63">
                Total Before Dicount</td>
            <td>
                <asp:TextBox ID="txtTotalBeforeDiscount" runat="server" 
                    style="text-align:right; " Width="84px" ReadOnly="True" >0</asp:TextBox>
            </td>
        </tr>               
        <tr>
            <td class="style98">
                &nbsp;</td>
            <td class="style63" valign="middle">
                Discount &nbsp;<asp:TextBox ID="txtDocDiscountPercent" runat="server" 
                    Width="76px" style="text-align:right; " AutoPostBack="True"  MaxLength="10"
                    CausesValidation="True" >0</asp:TextBox>
                % <asp:CustomValidator ControlToValidate="txtDocDiscountPercent" ID="txtDocDiscountPercentValidator" ForeColor="Red" OnServerValidate="ValidateDecimalOnly" ValidateEmptyText="true"  runat="server" ErrorMessage="InValid" Display="Dynamic" ValidationGroup="1" ></asp:CustomValidator>
                </td>
            <td>
                <asp:TextBox ID="txtDocDiscountAmount" runat="server" style="text-align:right; " 
                    Width="84px" ReadOnly="True">0</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style98">
                Country of origin</td>
            <td class="style99">
                <asp:TextBox ID="txtCountryofOrigin" runat="server" MaxLength="15"></asp:TextBox>
            </td>
            <td class="style63">
                Tax</td>
            <td>
                <asp:TextBox ID="txtTaxTotal" runat="server" Width="84px" 
                    style="text-align:right; " ReadOnly="True" >0</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style98">
                Layout Type</td>
            <td class="style99">
                <asp:DropDownList ID="ddlLayout" runat="server" 
                    DataSourceID="SqlDataSource8" DataTextField="Descr" DataValueField="FldValue" >
                   
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource8" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
                    SelectCommand="select UFD1.FldValue,UFD1.Descr from UFD1 INNER JOIN CUFD ON CUFD.FieldID=UFD1.FieldID AND CUFD.TableID= UFD1.TableID  where CUFD.AliasID='layout' and UFD1.TableID='OQUT'">
                </asp:SqlDataSource>
            </td>
            <td class="style63">
                <asp:Label ID="LblTotalFinal" runat="server" Text="Total"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtDocTotal" runat="server" Width="84px" 
                    style="text-align:right; " ReadOnly="True" >0</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style98">
                Customer/Indenter</td>
            <td class="style99">
                <asp:RadioButtonList ID="RBCustIndenter" runat="server">
                <asp:ListItem Value="C" Text="Customer Layout" Selected="True"></asp:ListItem>
                <asp:ListItem Value="I" Text="Indenter Layout"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td class="style63">
                 <% If Roles.IsUserInRole(Membership.GetUser().UserName, ConfigurationManager.AppSettings("Supervisor_Role")) Then%> Assign To <% End If%></td>
            <td>
                  <% If Roles.IsUserInRole(Membership.GetUser().UserName, ConfigurationManager.AppSettings("Supervisor_Role")) Then%> 
                 <asp:DropDownList ID="ddlAssignTo" runat="server" >
                </asp:DropDownList>
                <% End If%></td>
        </tr>
        <tr >
            <td  colspan="4">
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="62px" />
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="1" 
                    width="62px"/>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit"  ValidationGroup="1" 
                    height="26px" width="62px"             />
                <asp:Button ID="btnPreview" runat="server" Text="Preview" height="26px" 
                    width="62px" />
                <asp:Button ID="btnPreviewDisc" runat="server" Text="Preview-Disc" height="26px" 
                    width="100px" />                      
            </td>
          
        </tr>
    </table>
    
  </asp:Panel>
</asp:Content>
