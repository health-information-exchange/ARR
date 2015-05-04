<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordViewer.aspx.cs" Inherits="Perceptive.ARRViewer.RecordViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Record Repository Manager</title>
    <base target="_self" />
    <link rel="Shortcut Icon" href="perceptive.png" /> 
    <style type="text/css">
  .Initial
  {
    display: block;
    padding: 4px 18px 4px 18px;
    float: left;
    background: url("../Images/InitialImage.png") no-repeat right top;
    color: Black;
    font-weight: bold;
  }
  .Initial:hover
  {
    color: Orange;
    background: url("../Images/SelectedButton.png") no-repeat right top;
  } 
  .InitialRight
  {
    display: block;
    padding: 4px 18px 4px 18px;
    float: right;
    background: url("../Images/InitialImage.png") no-repeat right top;
    color: Black;
    font-weight: bold;
  }
  .InitialRight:hover
  {
    color: Orange;
    background: url("../Images/SelectedButton.png") no-repeat right top;
  } 
  .Clicked
  {
    float: left;
    display: block;
    background: url("../Images/SelectedButton.png") no-repeat right top;
    padding: 4px 18px 4px 18px;
    color: Black;
    font-weight: bold;
    color: Green;
  }
  </style>   
    <script type="text/javascript">
        function DisplayMessage(LogId) {
            window.showModalDialog("Results.aspx?logId=" + LogId, window, "dialogWidth:800px;resizable:yes");
            return false;
        }

        function Reset() {
            document.getElementById("txtIP").value = ''
            document.getElementById("txtStartDate").value = '';
            document.getElementById("txtEndDate").value = '';
            document.getElementById("txtSendStartDate").value = '';
            document.getElementById("txtSendEndDate").value = '';
            document.getElementById("txtGlobalSearch").value = '';
            document.getElementById("txtHost").value = '';
            document.getElementById("txtApp").value = '';
            document.getElementById("ddlDataType").selectedIndex = 0;
            document.getElementById("ddlLogType").selectedIndex = 0;
            document.getElementById("ddlStartTime").selectedIndex = 0;
            document.getElementById("ddlStartFormat").selectedIndex = 0;
            document.getElementById("ddlEndTime").selectedIndex = 0;
            document.getElementById("ddlEndFormat").selectedIndex = 0;
            document.getElementById("ddlSendStartTime").selectedIndex = 0;
            document.getElementById("ddlSendStartFormat").selectedIndex = 0;
            document.getElementById("ddlSendEndTime").selectedIndex = 0;
            document.getElementById("ddlSendEndFormat").selectedIndex = 0;

            document.getElementById("txtEventIdCode").value = '';
            document.getElementById("txtEventIdCodeSystemName").value = '';
            document.getElementById("txtEventIdDisplayName").value = '';
            document.getElementById("txtEventTypeCodeCode").value = '';
            document.getElementById("txtEventTypeCodeCodeSystemName").value = '';
            document.getElementById("txtEventTypeCodeDisplayName").value = '';
            document.getElementById("txtEventActionCode").value = '';
            document.getElementById("txtEventOutcomeIndicator").value = '';
            document.getElementById("txtEventDateTime").value = '';

            document.getElementById("txtAuditSourceId").value = '';
            document.getElementById("txtAuditEnterpriseSiteId").value = '';
            document.getElementById("txtAuditSourceTypeCode").value = '';
            document.getElementById("txtAuditSourceCodeSystemName").value = '';
            document.getElementById("txtAuditSourceOriginalText").value = '';           

            document.getElementById("txtSUId").value = '';
            document.getElementById("txtSNId").value = '';
            document.getElementById("txtHUId").value = '';
            document.getElementById("txtDUId").value = '';
            document.getElementById("txtDNId").value = '';

            document.getElementById("txtPObjId").value = '';

            window.ValidatorValidate(window.CompareValidator1);
            return false;
        }

        function ChangeDisplay(showGrid) {
            if (showGrid) {                
                document.getElementById('loadingRow').style.display = 'none';
                document.getElementById('gridRow').style.display = '';
                document.getElementById('readRow').style.display = '';
            }
            else {
                document.getElementById('loadingRow').style.display = '';
                document.getElementById('gridRow').style.display = 'none';
                document.getElementById('readRow').style.display = 'none';
            }
            return true;
        }

    </script>

</head>
<body onload="return ChangeDisplay(true);">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td colspan="5">
                    <asp:Image ID="Image1" Height="50px" Width="300px" runat="server" ImageUrl="~/PerceptiveSoftware_fromLexmark.jpg" ImageAlign="Left" />
                </td>
                <td align="right">
                    <asp:Label ID="Welcome" runat="server" />
                    <asp:LinkButton ID="lnkSignOut" Text="Sign Out" runat="server" onclick="SignOut_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="5" />
                <td>
                    <asp:Button ID="lnkManagement" runat="server" Text="Management Console" onclick="lnkManagement_Click" CssClass="InitialRight" />
                </td>
            </tr>
            <asp:Panel ID="pnlSearchButton" runat="server">
            <tr>
                <td>
                    <asp:Button ID="btnShowSearchPanel" runat="server" Text="Show Filters" onclick="btnShowSearchPanel_Click" CssClass="Initial" BorderStyle="None" />                    
                </td>
                <td colspan="5">
                    <asp:ToolkitScriptManager ID="toolkitScriptManager" runat="server" />
                    <asp:CalendarExtender ID="calendarExtenderEnd" runat="server" TargetControlID="txtEndDate" PopupPosition="BottomRight" Format="yyyy-MM-dd" />
                    <asp:CalendarExtender ID="calenderExtenderStart" runat="server" TargetControlID="txtStartDate" PopupPosition="BottomRight" Format="yyyy-MM-dd" />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="End date must be greater than or equal to the start date" ForeColor="Red"
                        ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" Operator="GreaterThanEqual" Type="Date" ValidationGroup="searchValidation"/>
                    <asp:CalendarExtender ID="calendarExtenderSendEnd" runat="server" TargetControlID="txtSendEndDate" PopupPosition="BottomRight" Format="yyyy-MM-dd" />
                    <asp:CalendarExtender ID="CalendarExtenderSendStart" runat="server" TargetControlID="txtSendStartDate" PopupPosition="BottomRight" Format="yyyy-MM-dd" />
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="End date must be greater than or equal to the start date" ForeColor="Red"
                        ControlToCompare="txtSendStartDate" ControlToValidate="txtSendEndDate" Operator="GreaterThanEqual" Type="Date" ValidationGroup="searchValidation"/>
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="pnlSearch" runat="server">
            <tr>
                <td>
                    <asp:Label ID="lblIP" Text="Source IP" runat="server" Font-Size="Small" />
                    <br />
                    <asp:TextBox ID="txtIP" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblStartTime" runat="server" Text="Received after:" Font-Size="Small"  /><br />
                    <asp:TextBox ID="txtStartDate" runat="server" Width="50%"/>
                    <asp:DropDownList ID="ddlStartTime" runat="server">
                        <asp:ListItem Text="00" Value="0" Selected="True" />
                        <asp:ListItem Text="01" Value="1" />
                        <asp:ListItem Text="02" Value="2" />
                        <asp:ListItem Text="03" Value="3" />
                        <asp:ListItem Text="04" Value="4" />
                        <asp:ListItem Text="05" Value="5" />
                        <asp:ListItem Text="06" Value="6" />
                        <asp:ListItem Text="07" Value="7" />
                        <asp:ListItem Text="08" Value="8" />
                        <asp:ListItem Text="09" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlStartFormat" runat="server">
                        <asp:ListItem Text="am" Value="0" Selected="True" />
                        <asp:ListItem Text="pm" Value="12" />
                    </asp:DropDownList>
                    <br />
                </td>
                <td>
                    <asp:Label ID="lblEndTime" runat="server" Text="Received before:" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEndDate" runat="server" Width="50%" />
                    <asp:DropDownList ID="ddlEndTime" runat="server">
                        <asp:ListItem Text="00" Value="0" Selected="True" />
                        <asp:ListItem Text="01" Value="1" />
                        <asp:ListItem Text="02" Value="2" />
                        <asp:ListItem Text="03" Value="3" />
                        <asp:ListItem Text="04" Value="4" />
                        <asp:ListItem Text="05" Value="5" />
                        <asp:ListItem Text="06" Value="6" />
                        <asp:ListItem Text="07" Value="7" />
                        <asp:ListItem Text="08" Value="8" />
                        <asp:ListItem Text="09" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlEndFormat" runat="server">
                        <asp:ListItem Text="am" Value="0" Selected="True" />
                        <asp:ListItem Text="pm" Value="12" />
                    </asp:DropDownList>
                    <br />
                </td>
                <td>
                    <asp:Label ID="lblValid" Text="Data Type" runat="server" Font-Size="Small" /><br />
                    <asp:DropDownList ID="ddlDataType" runat="server">
                        <asp:ListItem Text = "Valid" Value="1" Selected="True"/>
                        <asp:ListItem Text = "Invalid" Value="0" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblLogProtocol" Text="Log Protocol" runat="server" Font-Size="Small" /><br />
                    <asp:DropDownList ID="ddlLogType" runat="server">
                        <asp:ListItem Text="All" Value="All" Selected="True" />
                        <asp:ListItem Text="UDP" Value="UDP" />
                        <asp:ListItem Text="TCP" Value="TCP" />
                        <asp:ListItem Text="TLS" Value="TLS" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblGlobalSearch" Text="Contains" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtGlobalSearch" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblApp" Text="Application Name" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtApp" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblHost" Text="Host Name" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtHost" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblSendStartTime" runat="server" Text="Sent after:" Font-Size="Small"  /><br />
                    <asp:TextBox ID="txtSendStartDate" runat="server" Width="50%"/>
                    <asp:DropDownList ID="ddlSendStartTime" runat="server">
                        <asp:ListItem Text="00" Value="0" Selected="True" />
                        <asp:ListItem Text="01" Value="1" />
                        <asp:ListItem Text="02" Value="2" />
                        <asp:ListItem Text="03" Value="3" />
                        <asp:ListItem Text="04" Value="4" />
                        <asp:ListItem Text="05" Value="5" />
                        <asp:ListItem Text="06" Value="6" />
                        <asp:ListItem Text="07" Value="7" />
                        <asp:ListItem Text="08" Value="8" />
                        <asp:ListItem Text="09" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlSendStartFormat" runat="server">
                        <asp:ListItem Text="am" Value="0" Selected="True" />
                        <asp:ListItem Text="pm" Value="12" />
                    </asp:DropDownList>
                    <br />
                </td>
                <td>
                    <asp:Label ID="lblSendEndTime" runat="server" Text="Received before:" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtSendEndDate" runat="server" Width="50%" />
                    <asp:DropDownList ID="ddlSendEndTime" runat="server">
                        <asp:ListItem Text="00" Value="0" Selected="True" />
                        <asp:ListItem Text="01" Value="1" />
                        <asp:ListItem Text="02" Value="2" />
                        <asp:ListItem Text="03" Value="3" />
                        <asp:ListItem Text="04" Value="4" />
                        <asp:ListItem Text="05" Value="5" />
                        <asp:ListItem Text="06" Value="6" />
                        <asp:ListItem Text="07" Value="7" />
                        <asp:ListItem Text="08" Value="8" />
                        <asp:ListItem Text="09" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlSendEndFormat" runat="server">
                        <asp:ListItem Text="am" Value="0" Selected="True" />
                        <asp:ListItem Text="pm" Value="12" />
                    </asp:DropDownList>
                    <br />
                </td>
                <td colspan="2" />
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEventIdCode" Text="EventId Code" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventIdCode" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventIdCodeSystemName" Text="EventId CodeSystemName" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventIdCodeSystemName" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventIdDisplayName" Text="EventId DisplayName" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventIdDisplayName" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventTypeCodeCode" Text="EventTypeCode Code" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventTypeCodeCode" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventTypeCodeCodeSystemName" Text="EventTypeCode CodeSystemName" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventTypeCodeCodeSystemName" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventTypeCodeDisplayName" Text="EventTypeCode DisplayName" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventTypeCodeDisplayName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEventActionCode" Text="EventActionCode" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventActionCode" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventDateTime" Text="EventDateTime like" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventDateTime" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblEventOutcomeIndicator" Text="EventOutcomeIndicator" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtEventOutcomeIndicator" runat="server" />
                </td>
                <td colspan="3" />
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAuditSourceId" Text="Audit Source Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtAuditSourceId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblAuditEnterpriseSiteId" Text="Audit Enterprise Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtAuditEnterpriseSiteId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblAuditSourceTypeCode" Text="Audit Source Code" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtAuditSourceTypeCode" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblAuditSourceCodeSystemName" Text="Audit Source Code System Name" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtAuditSourceCodeSystemName" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblAuditSourceOriginalText" Text="Audit Source Display Name" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtAuditSourceOriginalText" runat="server" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSUId" Text="Source User Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtSUId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblSNId" Text="Source Network Access Point Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtSNId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblHUId" Text="Human Requestor User Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtHUId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblDUId" Text="Destination User Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtDUId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblDNId" Text="Destination Network Access Point Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtDNId" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblPObjId" Text="Participant Object Id" runat="server" Font-Size="Small" /><br />
                    <asp:TextBox ID="txtPObjId" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="5" />
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="Initial" ValidationGroup="searchValidation"
                        onclick="btnSearch_Click" OnClientClick="javascript:return ChangeDisplay(false);" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClientClick="return Reset();" CssClass="Initial" BorderStyle="None"/>
                </td>
            </tr>
            </asp:Panel>
            <tr id="loadingRow">
                <td colspan="6">
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Loading.gif" AlternateText="Loading..." ImageAlign="Middle" />
                </td>
            </tr>
            <tr id="gridRow">
                <td colspan="6">
                    <asp:GridView ID="resultGrid" runat="server" AutoGenerateColumns="false" 
                        Width="100%" Height="100%" AllowSorting="true" AllowPaging="false" 
                        onpageindexchanging="resultGrid_PageIndexChanging">
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#F7F7DE" />
                        <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#6B696B" />
                        <AlternatingRowStyle BackColor="AntiqueWhite" />                        
                        <RowStyle BackColor="Beige" />
                        <Columns>
                            <asp:TemplateField HeaderText="Log ID">                                
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("LogId")%>' 
                                        OnClientClick="DisplayMessage(this.textContent);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Source IP">
                                <ItemTemplate>
                                    <asp:Label ID="lblGridIP" runat="server" Text='<%#Eval("RemoteIP")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Logged at">
                                <ItemTemplate>
                                    <asp:Label ID="lblGridTime" runat="server" Text='<%#Eval("DateTime")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Protocol">
                                <ItemTemplate>
                                    <asp:Label ID="lblGridProtocol" runat="server" Text='<%#Eval("LogType")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Proper Message">
                                <ItemTemplate>
                                    <asp:Label ID="lblGridStatus" runat="server" Text='<%#Eval("IsValid")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr id="readRow">
                <td colspan="3" align="right" width="50%">
                    <asp:Button ID="btnPrevious" runat="server" Text="Previous" BorderStyle="None" CssClass="InitialRight" onclick="btnPrevious_Click" />
                </td>
                <td colspan="3" align="left" width="50%">
                    <asp:Button ID="btnNext" runat="server" Text="Next" onclick="btnNext_Click" BorderStyle="None" CssClass="Initial" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>