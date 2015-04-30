<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminCategoryScoringRangeList.aspx.vb" Inherits="Adjudication.AdminCategoryScoringRangeList" Title="Administrative - Matrix Descriptions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <table id="tblMain" class="TableSpacing">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
                    <asp:TextBox ID="txtSortOrder" runat="server" Visible="False"
                        BorderStyle="Dotted" Width="64px"></asp:TextBox>
                    <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False"
                        BorderStyle="Dotted" Width="64px">FK_CategoryID, FK_ScoringRangeID</asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" width="100%">
                    <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Add Matrix Description</asp:LinkButton>&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DataGrid ID="gridMain" runat="server" BorderColor="#000000" BorderStyle="Double"
                        Width="100%" OnItemCommand="gridMain_ItemSelect" HorizontalAlign="Left" CellPadding="2" AllowSorting="True"
                        AutoGenerateColumns="False" DataKeyField="FK_CategoryID" AllowPaging="False" BorderWidth="1px"
                        GridLines="Horizontal">
                        <FooterStyle ForeColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_CategoryID" HeaderText="FK_CategoryID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="FK_ScoringRangeID" HeaderText="FK_ScoringRangeID"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="center" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" ForeColor="blue"
                                        runat="server" />&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDelete" Text="Delete" CommandName="Delete_Command"
                                        ForeColor="blue" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ScoringRange" SortExpression="ScoringRange" HeaderText="Scoring Range">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="MatrixAdjectives" SortExpression="MatrixAdjectives" HeaderText="Matrix Adjectives">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
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
                    </asp:DataGrid></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlDeleteConfirm" runat="server" Visible="False">
        <table class="TableSpacing">
            <tr>
                <td align="center">NOTE: Deletions cannot be undone!
										<asp:TextBox ID="txtID_to_Delete" runat="server" Visible="False"
                                            BorderStyle="Dotted" Width="40px"></asp:TextBox>
                    <asp:TextBox ID="txtID_to_Delete_2" runat="server" Visible="False"
                        BorderStyle="Dotted" Width="40px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="separatorBlack">&nbsp;</div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <p>&nbsp;</p>
                    <p>
                        <asp:Label ID="lblConfirmDelete" runat="server"></asp:Label>
                    </p>
                    <p>&nbsp;</p>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="separatorBlack">&nbsp;</div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnDeleteConfirm" runat="server" Width="90px" Text="Delete"></asp:Button>&nbsp;
										<asp:Button ID="btnDeleteCancel" runat="server" Width="90px" Text="Cancel"></asp:Button></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
