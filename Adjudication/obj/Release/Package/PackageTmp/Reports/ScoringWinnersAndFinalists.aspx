<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScoringWinnersAndFinalists.aspx.vb" Inherits="Adjudication.ScoringWinnersAndFinalists" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Top Finalists</title>
</head>
<body>
    <form class="container" id="form1" runat="server">
    <img src="../Images/NHTA_Logo_Black_120x57px.png" style="position: absolute; top: 1px; right: 10px;" alt="NHTA" />
    <table cellspacing="0" cellpadding="0" style="width: 100%; font-size: small; color: Black; font-family: Arial; font-size: small; border: none">
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" align="center">
                <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="font-size: x-large; color: Black; font-family: Arial; font-variant: small-caps; white-space: nowrap" align="center">
                <table cellspacing="0" cellpadding="0" style="width: 100%; height: 60px; border: none">
                    <tr>
                        <td style="font-size: x-large; color: Black; font-family: Arial; font-variant: small-caps; white-space: nowrap" align="center">
                            NH Theatre Awards
                            <asp:Label ID="lblProductionCategory" runat="server"></asp:Label>Winners and Finalists
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="font-size: x-small; font-variant: small-caps; font-style: italic">
                            <asp:Label ID="lblSortOrder" runat="server" Style="color: #808080" Visible="false">Finalists are listed XXX by XXX</asp:Label>
                            <hr style="color: #ffcc00; width: 100%;" size="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
               <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_Production" runat="server" GridLines="None" BorderStyle="None" Font-Names="Arial" Font-Size="Small" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False" ShowHeader="false">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_Director" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_OriginalPlaywright" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_Actor" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_Actress" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_SupportingActor" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_SupportingActress" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_MusicalDirector" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_Choreographer" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_CostumeDesigner" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_ScenicDesigner" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_SoundDesigner" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: small;" valign="top" align="left" width="font-size: small;" colspan="2">
                <asp:DataGrid ID="grid_LightingDesigner" runat="server" GridLines="None" BorderStyle="None" HorizontalAlign="Left" CellSpacing="0" CellPadding="0" DataKeyField="PK_NominationsID" ShowFooter="False" AllowPaging="False" Width="100%" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>    </table>
    </form>
</body>
</html>
