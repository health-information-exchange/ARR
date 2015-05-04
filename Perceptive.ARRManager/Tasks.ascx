<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tasks.ascx.cs" Inherits="Perceptive.ARRViewer.Tasks" %>
<script type="text/javascript">
    function CheckIfEmpty() {
        if (document.getElementById("txtDays").value == '' || isNaN(document.getElementById("txtDays").value) == true) {
            alert("Please enter proper number of days");
            return false;
        }
        else
            return true;
    }
</script>
<div>
    <table>
        <tr>
            <td colspan="3">
                <h3>Task processing</h3>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnManulProcessing" runat="server" Text="Remove" CssClass="Initial" align="left" 
                    onclick="btnManulProcessing_Click" OnClientClick="return CheckIfEmpty();" />
            </td>
            <td>
                <asp:Label ID="lblDays" runat="server" Text="logs which are logged before" />
            </td>
            <td>
                
                <asp:TextBox ID="txtDays" ClientIDMode="Static" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;days
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnArchive" runat="server" Text="Archive" OnClick="btnArchive_Click" CssClass="Initial" />
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="Initial" onclick="btnRefresh_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblMessage" runat="server" />
            </td>
        </tr>
    </table>
</div>
