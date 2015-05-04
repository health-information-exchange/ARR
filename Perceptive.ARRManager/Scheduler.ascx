<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Scheduler.ascx.cs" Inherits="Perceptive.ARRViewer.Scheduler" %>
<div>
    <table>
        <tr>
            <td colspan="2">
                <h3>Current Server Activity</h3>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblServerStatus" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h3>Scheduler Statistics</h3>
            </td>
        </tr>        
        <tr>
            <td valign="top">
                <h4>Archives older logs?</h4>
            </td>
            <td>
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="chkArchive" runat="server" Text="Yes, archive them" />
                        </td>
                    </tr>
                    <tr>
                        <td width="40%">
                            <label>archive logs older than&nbsp;</label>
                        </td>
                        <td  width="60%">
                            <asp:TextBox ID="txtArchiveDays" runat="server" />
                            <label>&nbsp;days</label>
                        </td>
                    </tr>
                </table>                
            </td>
        </tr>
        <tr>
            <td />
            <td>
                <asp:Button ID="btnScheduler" runat="server" Text="Set" CssClass="Initial" onclick="btnScheduler_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="Initial" onclick="btnRefresh_Click"/>             
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"/>
            </td>
        </tr>
    </table>
</div>
