<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdjLateScoring.aspx.vb" Inherits="Adjudication.AdjLateScoring_Web" %>

<!DOCTYPE html>
<html>
<head>
    <title>REPORT: Adjudicator Late Scoring</title>
    
    
    
    
    <link href="../Styles.css" type="text/css" rel="STYLESHEET" />
</head>
<body>
    <form class="container" id="Form1" method="post" runat="server">
    <table   id="tblTitle" cellspacing="0" cellpadding="0" width="1240" border="0">
        <tr>
            <td class="ReportHeader1240" align="center" colspan="3" height="30">
                Late Production Scoring
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:DataGrid ID="gridMain" runat="server" Width="1240px" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" AllowSorting="True" BorderWidth="1px" DataKeyField="PK_ScoringID" AutoGenerateColumns="False" CellPadding="2" HorizontalAlign="Left"  >
                    <AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
                    <ItemStyle BackColor="WhiteSmoke"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="Open Date">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Close Date">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn SortExpression="ScoreDueDate" HeaderText="Scoring Due Date">
                            <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80px" ForeColor="red"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "ScoreDueDate","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Assigned Adjudicator">
                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Email Address">
                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                            <ItemTemplate>
                                <a class="fontDataHyperlink" href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
                                    <%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="PhonePrimary" SortExpression="PhonePrimary" HeaderText="Phone #">
                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="separatorBlack">
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Late Adjudicator Scores: 0</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSortOrder" runat="server" Width="64px" BorderStyle="Dotted"     Visible="False"></asp:TextBox><asp:TextBox ID="txtSortColumnName" runat="server" Width="64px" BorderStyle="Dotted"   Visible="False">LastPerformanceDateTime</asp:TextBox>
            </td>
            <td   align="right">
                Logged in as
                <asp:Label ID="lblLoginID" runat="server"  ></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
