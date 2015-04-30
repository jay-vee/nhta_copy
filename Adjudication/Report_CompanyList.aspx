<%@ Page Language="vb" Title="List of Theatre Companies" MasterPageFile="~/MasterPage.Master" AutoEventWireup="false" CodeBehind="Report_CompanyList.aspx.vb" Inherits="Adjudication.CompanyList" %>

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
    <table cellspacing="0" cellpadding="0" width="100%" align="left">
        <tr>
            <td valign="top" align="left">
                <div class="panel panel-dark">
                    <div class="panel-heading">Theater Companies</div>
                    <div class="panel-body">
                        <asp:DataGrid ID="gridMain" DataKeyField="PK_CompanyID" runat="server" HorizontalAlign="Left" CellPadding="2" AutoGenerateColumns="False" BorderWidth="1px" AllowSorting="True" GridLines="Horizontal" BorderColor="Gainsboro" BorderStyle="None" Width="100%">
                            <AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
                            <ItemStyle BackColor="WhiteSmoke"></ItemStyle>
                            <HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="CompanyName">
                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="FullAddress" SortExpression="FullAddress" HeaderText="Address">
                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Phone" SortExpression="Phone" HeaderText="Phone #">
                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" CssClass="TextSmall" Width="80"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn SortExpression="EmailAddress" HeaderText="Email">
                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Wrap="True" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <a class="fontDataHyperlinkSmall" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailAddress") %>'>
                                            <%# DataBinder.Eval(Container.DataItem, "EmailAddress") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="Website" HeaderText="Website">
                                    <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" Wrap="True" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <a class="fontDataHyperlinkSmall" target="_top" href='HTTP://<%# DataBinder.Eval(Container.DataItem, "Website") %>'>
                                            <%# DataBinder.Eval(Container.DataItem, "Website") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="ActiveCompany" HeaderText="Active">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# iif(DataBinder.Eval(Container.DataItem, "ActiveCompany")=1,"Yes","No") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td width="50%">
                <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black"> Number of Companies: 0</asp:Label>
                <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="40px">Company.ActiveCompany DESC, Company.CompanyName</asp:TextBox>
                <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted" Width="40px"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
