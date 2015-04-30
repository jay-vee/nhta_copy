<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScoringSummaryDetail.aspx.vb" Inherits="Adjudication.ScoringSummaryDetail" %>

<!DOCTYPE html>
<html>
<head>
    <title>NHTA Score Summary Counts</title>
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
                <td class="fontTitle" align="center" height="30">NHTA Top Finalists Summary Counts
                </td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Best Production
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Production" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Director
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Director" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Original Playwright
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_OriginalPlaywright" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Best Actor
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Actor" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Best Actress
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Actress" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Best Supporting Actor
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SupportingActor" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Best Supporting Actress
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SupportingActress" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
                <tr>
                    <td class="ReportHeader1000">Musical Director
                    </td>
                </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_MusicalDirector" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Choreographer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_Choreographer" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Costume Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_CostumeDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Scenic Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_ScenicDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Sound Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_SoundDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"##,##0.00") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td height="20"></td>
            </tr>
            <tr>
                <td class="ReportHeader1000">Lighting Designer
                </td>
            </tr>
            <tr>
                <td   valign="top" align="left" width="100%" colspan="2">
                    <asp:DataGrid ID="grid_LightingDesigner" runat="server"   HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="1px" GridLines="Horizontal" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" AllowPaging="False" width="100%" AllowSorting="False" AutoGenerateColumns="False">
                        <HeaderStyle Font-Bold="True" ForeColor="#ffff99" BackColor="#000000" Wrap="True"></HeaderStyle>
                        <FooterStyle BackColor="#000000"></FooterStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MaxScore" HeaderText="Max Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="MinScore" HeaderText="Min Score" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsWithCompletedBallot" HeaderText="# Adj with Ballot" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NumOfAdjudicatorsForProduction" HeaderText="# Adj Scores Used" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:BoundColumn DataField="UsingReserveAdjudicatorScore" HeaderText="Use Rsrv Adj?" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="50"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Total Score Final">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50"></ItemStyle>
                                <ItemTemplate>
                                    <%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####0") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Avg Score">
                                <HeaderStyle HorizontalAlign="center" Width="50"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="50" Font-Bold="True"></ItemStyle>
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
                <td class="fontFooter" align="left">
                    <font color="#666666">
                        <li>NOTE: For Productions with 3 submitted ballots, the High and Low Scores will not be dropped.</li><li>For Productions with less than 3 <font color="#666666">submitted </span>ballots, the Average Scores will be set to <b>Zero</b>.</li></span>
                </td>
            </tr>
            <tr>
                <td   align="right" colspan="4">Logged in as
                    <asp:Label ID="lblLoginID" runat="server"   Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
