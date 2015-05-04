<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Perceptive.ARRViewer.Results" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div height="100%" width="100%">
        <table height="100%" width="100%" border="2">
            <tr height="100%">
                <td width="100%" align="center">
                    <asp:TextBox ID="txtResult" runat="server" ReadOnly="true" TextMode="MultiLine" Width="90%" Height="100%"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
