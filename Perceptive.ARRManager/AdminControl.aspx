<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminControl.aspx.cs" Inherits="Perceptive.ARRViewer.AdminControl" %>
<%@ Register TagPrefix="uc" TagName="SupportedLogs" Src="~/SupportedLogs.ascx" %>
<%@ Register TagPrefix="uc" TagName="AccountManagement" Src="~/AccountManager.ascx" %>
<%@ Register TagPrefix="uc" TagName="Task" Src="~/Tasks.ascx" %>
<%@ Register TagPrefix="uc" TagName="Scheduler" Src="~/Scheduler.ascx" %>
<%@ Register TagPrefix="uc" TagName="User" Src="~/Users.ascx" %>
<%@ Register TagPrefix="uc" TagName="Appsetting" Src="~/Appsettings.ascx" %>
<%@ Register TagPrefix="uc" TagName="ActiveDatabase" Src="~/ActiveDatabaseSelection.ascx" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr align="right">
                <td colspan="6">
                    <asp:Image ID="Image1" Height="50px" Width="300px" runat="server" ImageUrl="~/PerceptiveSoftware_fromLexmark.jpg" ImageAlign="Left" />
                </td>
                <td align="right">
                    <asp:Label ID="Welcome" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="6" />
                <td>
                    <asp:Button Text="Back to Viewer" CssClass="InitialRight" ID="btnHome" runat="server" OnClick="btnHome_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button Text="Account Info" BorderStyle="None" ID="btnTab1" CssClass="Initial" runat="server" OnClick="btnTab1_Click" />
                </td>                 
                <td>
                    <asp:Button Text="Tasks" BorderStyle="None" ID="btnTab2" CssClass="Initial" runat="server" OnClick="btnTab2_Click" />
                </td>
                <td>
                    <asp:Button Text="Supported Logs" BorderStyle="None" ID="btnTab3" CssClass="Initial" runat="server" OnClick="btnTab3_Click" />
                </td>
                <td>
                    <asp:Button Text="User Management" BorderStyle="None" ID="btnTab4" CssClass="Initial" runat="server" OnClick="btnTab4_Click" />
                </td>
                <td>
                    <asp:Button Text="Scheduler Status" BorderStyle="None" ID="btnTab5" CssClass="Initial" runat="server" OnClick="btnTab5_Click" />
                </td>
                <td>
                    <asp:Button Text="Configuration" BorderStyle="None" ID="btnTab6" CssClass="Initial" runat="server" OnClick="btnTab6_Click" />
                </td>
                <td>
                    <asp:Button Text="Active Database" BorderStyle="None" ID="btnTab7" CssClass="Initial" runat="server" OnClick="btnTab7_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:MultiView ID="MainView" runat="server" >
                        <asp:View ID="View1" runat="server">
                            <uc:AccountManagement runat="server" ID="accountManagement" />
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <uc:Task runat="server" ID="task" />
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <uc:SupportedLogs runat="server" ID="supportedLogs" />
                        </asp:View>
                        <asp:View ID="View4" runat="server">
                            <uc:User ID="user" runat="server" />
                        </asp:View>
                        <asp:View ID="View5" runat="server">
                            <uc:Scheduler runat="server" ID="scheduler" />
                        </asp:View>
                        <asp:View ID="View6" runat="server">
                            <uc:Appsetting ID="appsetting" runat="server" />
                        </asp:View>
                        <asp:View ID="View7" runat="server">
                            <uc:ActiveDatabase ID="activeDatabase" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
