<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminUserList.aspx.vb" Inherits="Adjudication.AdminUserList1" Title="User Administration - List" %>

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
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server">
                <div class="TextCenter">
                    <asp:Label ID="lblSuccessfulReset" runat="server" ForeColor="Green" Visible="False">Successfully Reset Login ID: XXX</asp:Label>
                    <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>
                    <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>
                </div>
                <table id="tblMain" class="TableSpacing">
                    <tr>
                        <td style="text-align: left; vertical-align: bottom; width: 15%;">
                            <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Add User</asp:LinkButton>&nbsp;&nbsp;
                        </td>
                        <td align="center" width="75%">
                            <table  class="TableSpacing">
                                <tr>
                                    <td align="right" width="15%">Name:
                                    </td>
                                    <td width="60%" align="left">
                                        <asp:TextBox ID="txtSearchUser" runat="server" Width="45%" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                        <asp:CheckBox ID="chkInactiveUsers" runat="server" Text="&nbsp;Include Inactive/Suspended/Expelled"></asp:CheckBox>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle; width: 15%;" rowspan="2">
                                        <asp:Button ID="btnSearchName" runat="server" CssClass="btn btn-gold" Text="Search"></asp:Button>
                                        <asp:Button ID="btnClear" runat="server" CssClass="btn btn-gold" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="15%">Theatre Company:
                                    </td>
                                    <td width="60%" align="left">
                                        <asp:DropDownList ID="ddlFK_CompanyID" runat="server" CssClass="form-control" Width="90%"></asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: center; vertical-align: middle; width: 15%;"># of Users<br />
                            <asp:Label ID="lblTotalNumberOfRecords" runat="server" Font-Bold="True" ForeColor="Black">0</asp:Label>&nbsp;&nbsp;&nbsp;
                        </td>
                        <tr>
                            <td colspan="99">
                                <asp:DataGrid ID="gridMain" runat="server" BorderStyle="Double" BorderColor="#000000" Width="100%" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="PK_UserID" AutoGenerateColumns="False" AllowSorting="True" CellPadding="2" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect">
                                    <FooterStyle ForeColor="#000000"></FooterStyle>
                                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
                                    <ItemStyle ForeColor="#333333"></ItemStyle>
                                    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="PK_UserID" HeaderText="PK_UserID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="FK_AccessLevelID" HeaderText="FK_AccessLevelID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="FK_CompanyID" HeaderText="FK_CompanyID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastName" HeaderText="LastName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="FirstName" HeaderText="FirstName"></asp:BoundColumn>
                                        <asp:TemplateColumn ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnReset" Text="Reset" CommandName="Reset_Command" ForeColor="blue" runat="server" />
                                                <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" ForeColor="blue" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="User Name">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="UserLoginID" SortExpression="UserLoginID" HeaderText="Login ID">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="AccessLevelName" SortExpression="FK_AccessLevelID" HeaderText="Access Type">
                                            <HeaderStyle HorizontalAlign="left" Width="120"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="120"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Theater Company">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn SortExpression="DisabledFlag" HeaderText="Dis abled">
                                            <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkGridDisabledFlag" Checked='<%# DataBinder.Eval(Container.DataItem, "DisabledFlag") %>' Enabled="false" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="BadLoginCount" HeaderText="BadLoginCount"></asp:BoundColumn>
                                        <asp:TemplateColumn SortExpression="LastLoginTime" HeaderText="Last Login">
                                            <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="40"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "LastLoginTime","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="Address" HeaderText="Address"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="City" HeaderText="City"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="ZIP" HeaderText="ZIP"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="true" DataField="PhonePrimary" HeaderText="Primary Phone#" DataFormatString="{0:(###)###-####}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PhoneSecondary" HeaderText="PhoneSecondary"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="EmailPrimary"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="EmailSecondary" HeaderText="EmailSecondary"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="Website" HeaderText="Website"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="DateLastPasswordChange" HeaderText="DateLastPasswordChange"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastTrainingDate" HeaderText="LastTrainingDate"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="UserInformation" HeaderText="UserInformation"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
                                        <asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Primary Email Address">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle ForeColor="Blue" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "EmailPrimary")%><br />
                                                <%#DataBinder.Eval(Container.DataItem, "EmailSecondary")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
