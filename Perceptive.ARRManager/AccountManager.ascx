<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountManager.ascx.cs" Inherits="Perceptive.ARRViewer.AccountManager" %>

<div>
    <table>
        <tr>
            <td colspan="2">
                <h3>Account Management</h3>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblUserName" runat="server" Text="User Name" />                                
            </td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" ReadOnly="true" />
            </td>            
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPassword" runat="server" Text="Password" />                                
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" ReadOnly="true" TextMode="Password"  />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRole" runat="server" Text="Role" />                                
            </td>
            <td>
                <asp:TextBox ID="txtRole" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td />
            <td>
                <asp:Button ID="btnModify" Text="Edit" runat="server" CssClass="Initial" onclick="btnModify_Click" />
                <label>&nbsp;&nbsp;</label>
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="Initial" OnClick="btnCancel_Click" Visible="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" />
            </td>
        </tr>
    </table>
</div>