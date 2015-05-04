<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveDatabaseSelection.ascx.cs" Inherits="Perceptive.ARRViewer.ActiveDatabaseSelection" %>
<div>
    <table>
        <tr>
            <td colspan="3"><h3>Active Database</h3></td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlActiveDatabase" runat="server" />
            </td>
            <td>
                <asp:Button ID="btnSetActiveDB" CssClass="Initial" Text="Set" runat="server" OnClick="btnSetActiveDB_Click" />
            </td>
            <td>
                <asp:Button ID="btnRefresh" CssClass="InitialRight" Text="Refresh" runat="server" OnClick="btnRefresh_Click" />
            </td>
        </tr>
    </table>
</div>
