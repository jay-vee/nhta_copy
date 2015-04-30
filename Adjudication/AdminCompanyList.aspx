<%@ Page Language="vb" MasterPageFile="~/MasterPage.Master" Title="Theatre Company List" AutoEventWireup="false" CodeBehind="AdminCompanyList.aspx.vb" Inherits="Adjudication.AdminCompanyList" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlGrid" runat="server">
        <div class="TextCenter">
            <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>
            <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
            <asp:Label ID="lblSuccessful" runat="server" Visible="False" ForeColor="Green">Update Successful</asp:Label>
        </div>
        <table id="tblMain" class="TableSpacing">
            <tr>
                <td align="left" width="25%">
                    <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Add Company</asp:LinkButton>
                </td>
                <td align="center" width="50%">
                    <asp:CheckBox ID="chkInactive" runat="server" Text="&nbsp;Include Inactive" AutoPostBack="True"></asp:CheckBox></td>
                <td align="left" width="25%">
                    <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Companies: 0</asp:Label>
                </td>
            </tr>

        </table>
        <asp:DataGrid ID="gridMain" runat="server" BorderColor="#000000" BorderStyle="Double"
            OnItemCommand="gridMain_ItemSelect" HorizontalAlign="Left" CellPadding="2" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyField="PK_CompanyID" AllowPaging="False" BorderWidth="1px"
            GridLines="Horizontal" Width="100%">
            <FooterStyle ForeColor="#000000"></FooterStyle>
            <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
            <AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
            <ItemStyle ForeColor="#333333"></ItemStyle>
            <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_UserID"></asp:BoundColumn>
                <asp:TemplateColumn ItemStyle-Width="51px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" CssClass="FontHyperLink"
                            runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company">
                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="City" SortExpression="City" HeaderText="City">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn SortExpression="ActiveCompany" HeaderText="Active">
                    <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkGridActiveCompany" Checked='<%# DataBinder.Eval(Container.DataItem, "ActiveCompany") %>' Enabled="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="NumOfProductions" SortExpression="NumOfProductions" HeaderText="# Shows">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn SortExpression="LastUpdateByDate" HeaderText="Last Updated">
                    <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn Visible="False" DataField="Address" HeaderText="Address"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="City" HeaderText="City"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="ZIP" HeaderText="ZIP"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="Phone" HeaderText="PhonePrimary"></asp:BoundColumn>
                <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" ItemStyle-ForeColor="navy"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="Website" HeaderText="Website"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="Comments" HeaderText="UserInformation"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
            </Columns>
            <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </asp:Panel>
</asp:Content>
