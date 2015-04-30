<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScoringSummary.aspx.vb" Inherits="Adjudication.ScoringSummary" %>

<!DOCTYPE html>
<html>
<head>
    <title>NHTA Score Summary</title>
    
    
    
    
    <link href="../Styles.css" type="text/css" rel="STYLESHEET" />
</head>
<body>
    <form class="container Text" id="MainMenu" method="post" runat="server">
    <table class="TableSpacing">
        <tr>
            <td   align="center">
                <asp:Label ID="lblErrors" runat="server"   Visible="False" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="fontTitle" align="center" height="30">
                NHTA Top Finalists Summary
            </td>
        </tr>
        <tr>
            <td align="center" style="font-size: x-small">
                <asp:Label ID="lblSortOrder" Font-Italic="True" runat="server">Finalists are listed XXX by XXX</asp:Label>
                <hr>
            </td>
        </tr>
        <tr>
            <td class="ReportHeader1000">
                Best Production
            </td>
        </tr>
        <tr>
            <td   valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Production" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                    <FooterStyle BackColor="BLACK"></FooterStyle>
                    <ItemStyle ForeColor="#333333"></ItemStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Score">
                            <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                            <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                            <ItemTemplate>
                                <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td height="20">
            </td>
        </tr>
        <tr>
            <td class="ReportHeader1000">
                Director
            </td>
        </tr>
        <tr>
            <td   valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Director" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                    <FooterStyle BackColor="BLACK"></FooterStyle>
                    <ItemStyle ForeColor="#333333"></ItemStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Score">
                            <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                            <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                            <ItemTemplate>
                                <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td height="20">
            </td>
            <tr>
                <td class="ReportHeader1000">
                    Musical Director
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_MusicalDirector" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Choreographer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Choreographer" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Lighting Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_LightingDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Sound Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SoundDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Costume Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_CostumeDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Scenic Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_ScenicDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Original Playwright
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_OriginalPlaywright" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Best Actor
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Actor" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Best Actress
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Actress" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Best Supporting Actor
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SupportingActor" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20">
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">
                    Best Supporting Actress
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SupportingActress" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                        <FooterStyle BackColor="BLACK"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Score">
                                <HeaderStyle HorizontalAlign="right" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="right" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="../Images/NHTAGradientHorizontal1000.jpg" />
                </td>
            </tr>
            <tr>
                <td class="fontFooter" valign="middle" align="center" height="24">
                    <span style="color: red;">NOTE: For Productions with less than 4 Adjudicators, the High and Low Scores will not be dropped.</span>
                </td>
            </tr>
            <tr>
                <td   align="right" colspan="4">
                    Logged in as
                    <asp:Label ID="lblLoginID" runat="server"   Font-Bold="True"></asp:Label>
                </td>
            </tr>
    </table>
    </TD></TR></TABLE></form>
</body>
</html>
