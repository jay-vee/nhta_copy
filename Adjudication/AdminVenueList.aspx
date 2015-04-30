<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" Title="Venue List" CodeBehind="AdminVenueList.aspx.vb" Inherits="Adjudication.AdminVenueList" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <table id="tblMain" class="TableSpacing">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblSuccessful" runat="server" Visible="False" ForeColor="Green">Update Successful</asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="20%">
                    <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Add Venue</asp:LinkButton>
                </td>
                <td align="center" width="60%">
                    <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Venues: 0</asp:Label>&nbsp;
                                    <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
                    <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>&nbsp;
                </td>
                <td align="left" width="20%">
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:DataGrid ID="gridMain" runat="server" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#000000" BorderStyle="Double" BorderWidth="1px" CellPadding="2" DataKeyField="PK_VenueID" GridLines="Horizontal" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect" Width="100%">
                        <FooterStyle ForeColor="#000000" />
                        <SelectedItemStyle BackColor="#FFFF99" Font-Bold="True" ForeColor="White" />
                        <AlternatingItemStyle BackColor="LemonChiffon" HorizontalAlign="Left" />
                        <ItemStyle ForeColor="#333333" />
                        <HeaderStyle BackColor="#000000" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundColumn DataField="PK_VenueID" HeaderText="PK_VenueID" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-Width="51px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit_Command" ForeColor="blue" Text="Edit" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="VenueName" HeaderText="Venue" SortExpression="VenueName">
                                <HeaderStyle HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="City" HeaderText="City" SortExpression="City">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="State" HeaderText="State" SortExpression="State">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SeatingCapacity" HeaderText="Seating Capacity" SortExpression="SeatingCapacity">
                                <HeaderStyle HorizontalAlign="Center" Width="100" />
                                <ItemStyle CssClass="TextSmall" HorizontalAlign="Center" Width="100" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Handi Capped" SortExpression="HandicappedAccessible">
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkGrid" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "HandicappedAccessible") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Air Cond." SortExpression="AirConditioned">
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="Checkbox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "AirConditioned") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Out door" SortExpression="Outdoor">
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="Checkbox2" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Outdoor") %>' Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Last Updated" SortExpression="LastUpdateByDate">
                                <HeaderStyle HorizontalAlign="Center" Width="50" />
                                <ItemStyle CssClass="TextSmall" HorizontalAlign="Center" Width="40" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Address" HeaderText="Address" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="City" HeaderText="City" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ZIP" HeaderText="ZIP" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Phone" HeaderText="PhonePrimary" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="EmailAddress" HeaderText="EmailPrimary" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Website" HeaderText="Website" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Comments" HeaderText="UserInformation" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="LastUpdateByName" HeaderText="LastUpdateByName" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="LastUpdateByDate" HeaderText="LastUpdateByDate" Visible="False"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle BackColor="#000000" ForeColor="White" HorizontalAlign="Center" Mode="NumericPages" />
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
