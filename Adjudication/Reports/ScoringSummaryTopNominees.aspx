<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScoringSummaryTopNominees.aspx.vb" Inherits="Adjudication.ScoringSummaryTopNominees" %>

<!DOCTYPE html>
<html>
<head>
    <title>NHTA Top Finalists</title>
    
    
    
    
    <link href="../Styles.css" type="text/css" rel="STYLESHEET" />
</head>
<body>
    <form class="container" class="fontData" id="MainMenu" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="800">
        <tr>
            <td class="fontData" align="center">
                <asp:Label ID="lblErrors" runat="server" CssClass="fontData" Visible="False" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="fontTitle" align="center" height="30">
                NHTA Top Finalists 
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Images/NHTAGradientHorizontal800.jpg" />
            </td>
        </tr>
        <tr>
            <td class="fontData" valign="top" align="left" width="800" colspan="2">
                <asp:DataGrid ID="grid_Production" runat="server" CssClass="fontData" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderWidth="0px" GridLines="None" BorderColor="darkgray" BorderStyle="Solid" ShowFooter="False" ShowHeader="False" AllowPaging="False" Width="800px" AllowSorting="False" AutoGenerateColumns="False">
                    <HeaderStyle Font-Bold="True" ForeColor="#cccc33" BackColor="#000000"></HeaderStyle>
                    <FooterStyle BackColor="WHITE"></FooterStyle>
                    <ItemStyle ForeColor="#333333"></ItemStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200" ItemStyle-Font-Bold="True"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120" ItemStyle-Font-Italic="True"></asp:BoundColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Table runat="server" ID="tblHeader" Width="800px" CellPadding="0" CellSpacing="0" Visible="False">
                                    <asp:TableRow CssClass="fontTitleHeaderBlack" ForeColor="Black" Font-Bold="True">
                                        <asp:TableCell Width="800" HorizontalAlign="Left" Height="30px" VerticalAlign="Bottom">
													<%# DataBinder.Eval(Container.DataItem, "ProductionCategory")%> Theater
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:Table runat="server" ID="tblProdData" Width="800px" CellPadding="0" CellSpacing="0" Visible="False">
                                    <asp:TableRow CssClass="fontData" Width="800">
                                        <asp:TableCell ColumnSpan="11" Height="2">
													<IMG SRC="http://localhost/Adjudication/Images/NHTAGradientHorizontal800.jpg" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow CssClass="fontHeader" Font-Bold="True">
                                        <asp:TableCell Width="100%" HorizontalAlign="Left">
													Best Production - <%# DataBinder.Eval(Container.DataItem, "ProductionType") %>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <table width="800" class="fontData" cellpadding="0" cellspacing="0" align="left">
                                    <tr>
                                        <td width="100%" align="left">
                                            <font color="MediumBlue">
                                                <%# DataBinder.Eval(Container.DataItem, "BestName") %></span> -
                                            <%# DataBinder.Eval(Container.DataItem, "CompanyName") %>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Images/NHTAGradientHorizontal800.jpg" />
            </td>
        </tr>
        <tr>
            <td class="fontData" align="right" colspan="4">
                Logged in as
                <asp:Label ID="lblLoginID" runat="server" CssClass="fontData" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    </TD></TR></TABLE></form>
</body>
</html>
