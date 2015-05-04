<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Users.ascx.cs" Inherits="Perceptive.ARRViewer.Users" %>
<div>
    <table>
        <tr>
            <td colspan="2">
                <h3>User Management</h3>
            </td>
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
                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" Visible='<%#Eval("Enabled") %>' CommandName="DELETE" CommandArgument='<%#Eval("UserId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Role">
                            <ItemTemplate>
                                <asp:Label ID="lblUserRole" runat="server" Text='<%#Eval("Role")%>' />
                            </ItemTemplate>
                        </asp:TemplateField> 
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h4>Add new user</h4>
            </td>
        </tr>
        <tr>
            <td>User Name</td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Passord</td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Role</td>
            <td>
                <asp:DropDownList ID="ddlRole" runat="server" />
            </td>
        </tr>
        <tr>
            <td />
            <td>
                <asp:Button ID="btnAddUser" CssClass="Initial" Text="Add User" runat="server" onclick="btnAddUser_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" />
            </td>
        </tr>
    </table>
</div>