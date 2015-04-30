<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FinalistByProductionName.aspx.vb" Inherits="Adjudication.FinalistByProductionName" %>

<!DOCTYPE html>
<html>
<head>
    <title>NH Theatre Awards Finalists</title>
    
    
    
    
    <link href="../Styles.css" type="text/css" rel="STYLESHEET" />
</head>
<body>
    <form class="container" style="font-size: small" id="MainMenu" method="post" runat="server">
    <table class="TableSpacing">
        <tr>
            <td style="font-size: small" align="center">
                <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="font-size: large" align="center" height="30">
                NH Theatre Awards Finalists
            </td>
        </tr>
        <tr>
            <td align="center" style="font-size: x-small">
                <asp:Label ID="lblSortOrder" Font-Italic="True" runat="server">Finalists are listed XXX by XXX</asp:Label>
                <hr>
            </td>
        </tr>
        <tr>
            <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Production" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="None" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td height="20">
                <hr>
            </td>
        </tr>
        <tr>
            <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Director" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td height="20">
                <hr>
            </td>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_MusicalDirector" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Choreographer" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_LightingDesigner" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SoundDesigner" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_CostumeDesigner" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_ScenicDesigner" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_OriginalPlaywright" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Actor" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Actress" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SupportingActor" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                    <hr>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SupportingActress" runat="server" AutoGenerateColumns="False" AllowSorting="False" AllowPaging="False" ShowFooter="False" BorderStyle="none" GridLines="None" BorderWidth="1px" DataKeyField="PK_NominationsID" CellPadding="1" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemStyle HorizontalAlign="left" Width="100%"></ItemStyle>
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td style="font-size: small" align="right" colspan="4">
                    <asp:Label ID="lblLoginID" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                </td>
            </tr>
    </table>
    </TD></TR></TABLE></form>
</body>
</html>
