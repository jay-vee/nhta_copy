﻿<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminCategoryList.aspx.vb" Inherits="Adjudication.AdminCategoryList" Title="Category List" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <table id="tblMain" class="TableSpacing">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>
                    <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">DisplayOrder</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" width="100%">
                    <asp:LinkButton ID="lbtnAdd" runat="server" class="btn btn-gold">Add Category</asp:LinkButton>&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
            <tr>
                <td colspan="2">
                    <asp:DataGrid ID="gridMain" runat="server" Width="100%" BorderStyle="Double" BorderColor="#000000" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="PK_CategoryID" AutoGenerateColumns="False" AllowSorting="True" CellPadding="2" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect">
                        <FooterStyle ForeColor="#000000"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                        <AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="PK_CategoryID" HeaderText="PK_CategoryID"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="left" ItemStyle-Width="68">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" ForeColor="blue" runat="server" />
                                    <asp:LinkButton ID="btnDelete" Text="Delete" CommandName="Delete_Command" ForeColor="blue" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="DisplayOrder" SortExpression="DisplayOrder" HeaderText="Display Order">
                                <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="ActiveCategory" HeaderText="Active">
                                <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkGridRequiresAdjudication" Checked='<%# DataBinder.Eval(Container.DataItem, "ActiveCategory") %>' Enabled="false" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="LastUpdateByName" HeaderText="Last Updated By">
                                <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "LastUpdateByName","{0:MM/dd/yy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="LastUpdateByDate" HeaderText="Last Updated Date">
                                <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlDeleteConfirm" runat="server" Visible="False">
        <table class="TableSpacing">
            <tr>
                <td align="center">NOTE: Deletions cannot be undone!
                                <asp:TextBox ID="txtID_to_Delete" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="separatorBlack">
                        &nbsp;
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <p>
                        &nbsp;
                    </p>
                    <p>
                        <asp:Label ID="lblConfirmDelete" runat="server"></asp:Label>
                    </p>
                    <p>
                        &nbsp;
                    </p>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="separatorBlack">
                        &nbsp;
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnDeleteConfirm" runat="server" Width="90px" Text="Delete"></asp:Button>&nbsp;
                                <asp:Button ID="btnDeleteCancel" runat="server" Width="90px" Text="Cancel"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>