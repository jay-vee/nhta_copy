<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScoringFinalists_CardsForPresenters.aspx.vb" Inherits="Adjudication.ScoringFinalists_CardsForPresenters" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head>
    <title>Top Finalists for Presenters</title>
    
    
    
    
</head>
<body>
    <form class="container" style="color: Black; font-family: Arial; font-size: 11;" id="MainMenu" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" style="width: 500px; border: none">
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" align="center">
                <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="font-size: 14pt; color: Black; font-family: Arial; font-variant: small-caps; white-space: nowrap" align="center" height="30px">
                NH Theatre Awards Top
                <asp:Label ID="lblProductionCategory" runat="server" Text=""></asp:Label>
                Finalists
            </td>
        </tr>
        <tr>
            <td align="center" style="font-size: x-small">
                <div style="font-variant: small-caps">
                </div>
                <asp:Label ID="lblSortOrder" Font-Italic="True" runat="server">Finalists are listed XXX by XXX</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 12;" valign="top" align="left" width="100%" colspan="2">
                <hr noshade>
                <br />
                <b>"Matty" Award for Vision and Tenacity</b><br />&nbsp;&nbsp;&nbsp;is awarded to...<br />
                <br />
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_LightingDesigner" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_SoundDesigner" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_ScenicDesigner" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_CostumeDesigner" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Choreographer" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_MusicalDirector" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_SupportingActress" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_SupportingActor" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Actor" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Actress" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 12;" valign="top" align="left" width="100%" colspan="2">
                <hr noshade>
                <br />
                <b>Children's and Youth Theatre Award</b><br />&nbsp;&nbsp;&nbsp;is awarded to...<br />
                <br />
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_OriginalPlaywright" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 12;" valign="top" align="left" width="100%" colspan="2">
                <hr noshade>
                <br />
                <b>Francis Grover Cleveland Lifetime Achievement Award</b><br />&nbsp;&nbsp;&nbsp;is awarded to...<br />
                <br />
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Director" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 12;" valign="top" align="left" width="100%" colspan="2">
                <hr noshade>
                <br />
                <b>General Excellence Awards</b><br />&nbsp;&nbsp;&nbsp;is awarded to... <i>(1 for each Company Type: Professional/Youth/Community)</i><br />
                <br /><hr noshade>
            </td>
        </tr>
        <tr>
            <td style="color: Black; font-family: Arial; font-size: 11;" valign="top" align="left" width="100%" colspan="2">
                <asp:DataGrid ID="grid_Production" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="1" DataKeyField="PK_NominationsID" GridLines="None" BorderStyle="None" ShowFooter="False" AllowPaging="False" Width="500px" AllowSorting="False" AutoGenerateColumns="False" ShowHeader="false">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="FK_ProductionID" HeaderText="ProductionType"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionType" HeaderText="Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Production" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" HeaderText="Theater Company" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestName" HeaderText="Nominee" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BestRole" HeaderText="Role/Position" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CategoryName" HeaderText="Award Category" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"></asp:BoundColumn>
                        <asp:BoundColumn DataField="AverageScoreFinal" HeaderText="Score" DataFormatString="{0:##,##0.00}"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
