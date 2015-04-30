<%@ Page Language="vb" Title="Production Listing" MasterPageFile="~/MasterPage.Master" AutoEventWireup="false" CodeBehind="Report_ViewAllProductions.aspx.vb" Inherits="Adjudication.ViewAllProductions" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .form-control {
            display: inline-block;
        }

        td {
            vertical-align: middle;
        }
    </style>
    <asp:Panel ID="pnlGrid" runat="server">
        <table id="tblMain" class="TableSpacing">
            <tr>
                <td colspan="2">
                    <div class="panel panel-dark">
                        <div class="panel-heading">Nominated Productions</div>
                        <div class="panel-body">
                            <asp:DataGrid ID="gridMain" runat="server" Width="100%" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" BorderWidth="1px" DataKeyField="PK_ProductionID" AutoGenerateColumns="False" AllowSorting="True" CellPadding="2" HorizontalAlign="Left">
                                <AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
                                <ItemStyle BackColor="WhiteSmoke"></ItemStyle>
                                <HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
                                    <asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="First Show Date">
                                        <HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="90"></ItemStyle>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ProductionCategory" SortExpression="ProductionCategory" HeaderText="Production Category">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="VenueInfo" SortExpression="VenueInfo" HeaderText="Venue">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" CssClass="TextSmall"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn SortExpression="RequiresAdjudication" HeaderText="To Be Adjud." Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="40" CssClass="TextSmall"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="40"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkGridRequiresAdjudication" Checked='<%# DataBinder.Eval(Container.DataItem, "RequiresAdjudication") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn SortExpression="SetNominations" HeaderText="Submit Nomin" Visible="False">
                                        <HeaderStyle HorizontalAlign="Center" Width="40" CssClass="TextSmall"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="40"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Checkbox1" Checked='<%# DataBinder.Eval(Container.DataItem, "SetNominations") %>' Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black"> Number of Productions: 0</asp:Label>
                </td>
                <td align="right">
                    <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
                    <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
