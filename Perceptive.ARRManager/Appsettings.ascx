<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Appsettings.ascx.cs" Inherits="Perceptive.ARRViewer.Appsettings" %>
<div>
    <table>
        <tr>
            <td>
                <h3>Configuration Management</h3>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="resultGrid" runat="server" AutoGenerateColumns="false" 
                    Width="100%" Height="100%" AllowSorting="true" >
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#F7F7DE" />
                    <HeaderStyle ForeColor="White" Font-Bold="True" BackColor="#6B696B" />
                    <AlternatingRowStyle BackColor="AntiqueWhite" />                        
                    <RowStyle BackColor="Beige" />
                    <Columns>
                        
                        <asp:TemplateField HeaderText="Key">
                            <ItemTemplate>
                                <asp:Label ID="lblKey" runat="server" Text='<%#Eval("Key")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value">
                            <ItemTemplate>
                                <asp:TextBox ID="txtValue" runat="server" Text='<%#Eval("Value")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>                        
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" onclick="btnUpdate_Click" CssClass="Initial" />
            </td>
        </tr>
    </table>
</div>
