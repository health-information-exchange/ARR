<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportedLogs.ascx.cs" Inherits="Perceptive.ARRViewer.SupportedLogs" ClassName="SupportedLogs" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

    <div>
        <table>
            <tr>
                <td colspan="2"><h3>Supported Log Types</h3></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="resultGrid" runat="server" AutoGenerateColumns="false" 
                        Width="100%" Height="100%" AllowSorting="true" OnRowCommand="resultGrid_RowCommand" OnRowDeleting="resultGrid_RowDeleting" >
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#F7F7DE" />
                        <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#6B696B" />
                        <AlternatingRowStyle BackColor="AntiqueWhite" />
                        <RowStyle BackColor="Beige" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="DELETE" CommandArgument='<%#Eval("Code") + "|" + Eval("DisplayName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Event Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%#Eval("Code")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Display Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%#Eval("DisplayName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h4>Add new log type</h4>
                </td>
            </tr>
            <tr>
                <td>Event Code</td>
                <td><asp:TextBox ID="txtCode" runat="server" /></td>
            </tr>
            <tr>
                <td>Event DisplayName</td>
                <td><asp:TextBox ID="txtDisplayName" runat="server" /></td>
            </tr>
            <tr>
                <td />
                <td>
                    <asp:Button ID="btnAddSupportedLogType" Text="Add" runat="server" CssClass="Initial" onclick="btnAddSupportedLogType_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrorMessage" runat="server" />
                </td>
            </tr>
        </table>
    </div>
