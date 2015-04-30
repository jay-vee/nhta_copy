<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminProductionList.aspx.vb" Inherits="Adjudication.AdminProductionList" Title="Production List" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"></asp:Label>
    <div class="TextCenter">
        <asp:Label ID="lblSuccessful" runat="server" ForeColor="Green" Visible="False">Update Successful</asp:Label>
    </div>

    <asp:Panel ID="pnlGrid" runat="server">
        <table id="tblMain" class="TableSpacing">
            <tr>
                <td align="left" width="20%">
                    <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-gold">Add Production</asp:LinkButton>
                </td>
                <td align="center" width="60%">Number of Productions:
                                    <asp:Label ID="lblTotalNumberOfRecords" runat="server" CssClass="FontBold">0</asp:Label>&nbsp;
                                        <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">FirstPerformanceDateTime</asp:TextBox>
                    <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>&nbsp;
                </td>
                <td align="right" width="20%">Only Future Productions:
                                        <asp:CheckBox ID="chkShowOnlyFutureProductions" runat="server" AutoPostBack="True" Checked="False"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:DataGrid ID="gridMain" runat="server" BorderStyle="Double" BorderColor="#000000" Width="100%" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="PK_ProductionID" AutoGenerateColumns="False" AllowSorting="True" CellPadding="2" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect">
                        <FooterStyle ForeColor="#000000"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                        <AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="left" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" Text="Edit" CommandName="Edit_Command" CssClass="FontHyperLink" runat="server" /><br />
                                    <asp:LinkButton ID="btnNomination" Text="Nominate" CommandName="Nomination_Command" CssClass="FontHyperLink" runat="server" /><br />
                                    <asp:LinkButton ID="btnRemoveShow" Text="Remove" CommandName="Remove_Command" CssClass="FontHyperLink" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="SetNominations" SortExpression="SetNominations" HeaderText="Nom. Set" Visible="False">
                                <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Font-Bold="True"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionType" SortExpression="ProductionType" HeaderText="Production Type">
                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="OriginalProduction" HeaderText="Orig">
                                <HeaderStyle HorizontalAlign="Center" Width="30"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="30" CssClass="TextSmall" Font-Bold="True" ForeColor="DarkViolet"></ItemStyle>
                                <ItemTemplate>
                                    <%# iif(DataBinder.Eval(Container.DataItem, "OriginalProduction") = 1, "ORIG","") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ProductionCategory" SortExpression="ProductionCategory" HeaderText="Category">
                                <HeaderStyle HorizontalAlign="Center" Width="120"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="120"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="First Performance">
                                <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Last Performance">
                                <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="PK_ProductionCategoryID" HeaderText="10"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="11"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="12"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="13"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="FK_CompanyID" HeaderText="14"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlRemoveShow" Visible="False" Width="100%" runat="server">
        <table id="tblRemoveShow" class="TableSpacing">
            <tr>
                <td align="center">
                    <strong>Are you sure you want to Remove the Production of:</strong>
                </td>
            </tr>
            <tr>
                <td style="border-right: olive thin solid; padding-right: 2px; border-top: olive thin solid; padding-left: 2px; border-left: olive thin solid; padding-top: 2px; border-bottom: olive thin solid; background-color: aliceblue" align="left" width="100%">
                    <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                        <tr>
                            <td align="right" width="430">
                                <font style="font-variant: small-caps" color="#404000">Production:</span>
                            </td>
                            <td width="430">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="True"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblProductionType" runat="server" Font-Italic="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="430">
                                <font style="font-variant: small-caps" color="#404000">Theatre Group:</span>
                            </td>
                            <td valign="top" align="left" width="430">
                                <asp:Label ID="lblCompanyName" runat="server"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblProductionCategory" runat="server" Font-Italic="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="430">
                                <font style="font-variant: small-caps" color="#404000">Production Dates:</span>
                            </td>
                            <td valign="top" align="left" width="430">
                                <asp:Label ID="lblFirstPerformanceDateTime" runat="server"></asp:Label>&nbsp;thru &nbsp;
                                                <asp:Label ID="lblLastPerformanceDateTime" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr bgcolor="lemonchiffon">
                <td align="center">Administrator Comments to include in Email:<br />
                    <asp:TextBox ID="txtAdminEmailComments_Assign" CssClass="TextSmall" Width="100%" runat="server" Height="60px"></asp:TextBox><br />
                    <asp:RadioButtonList ID="rblEmailInfo" runat="server" CssClass="TextSmall" Width="560px">
                        <asp:ListItem Value="NoAction">&nbsp;Do not Email</asp:ListItem>
                        <asp:ListItem Value="EmailAssignmentToAll" Selected="True">&nbsp;Email Assignment to Representing Company and Producing Company members</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="center" height="40">
                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-gold" Text="Remove Production"></asp:Button>
                    <asp:Button ID="btnRemoveDelete" runat="server" CssClass="btn btn-gold" Text="Production List"></asp:Button>
                </td>
            </tr>
            <tr>
                <td valign="bottom" align="center" height="30">
                    <b>Currently Assigned Adjudicators</b> for
                                    <asp:Label ID="txtProdName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="gridSub" runat="server" Width="100%" BorderStyle="Double" BorderColor="#000000" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="FK_NominationsID" AutoGenerateColumns="False" CellPadding="2" HorizontalAlign="Left">
                        <FooterStyle ForeColor="Black"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                        <AlternatingItemStyle HorizontalAlign="Left"></AlternatingItemStyle>
                        <ItemStyle ForeColor="#333333"></ItemStyle>
                        <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="Black"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="FK_NominationsID" HeaderText="FK_NominationsID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="PK_UserID" HeaderText="PK_UserID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Adjudicator">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Representing Company">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="AdjudicatorToSeeProduction" SortExpression="AdjudicatorToSeeProduction" HeaderText="Confirm Adjud.">
                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TotalScore" SortExpression="TotalScore" HeaderText="Scoring">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="LastUpdateByDate" HeaderText="Last Updated">
                                <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="ProductionDateAdjudicated_Planned"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Actual" HeaderText="ProductionDateAdjudicated_Actual"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="AdjudicatorRequestsReassignment"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CreateByName" HeaderText="CreateByName"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="CreateByDate" HeaderText="CreateByDate"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ReserveAdjudicator" HeaderText="ReserveAdjudicator"></asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="ReserveAdjudicator" HeaderText="Rsrv">
                                <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <%# IIF(DataBinder.Eval(Container.DataItem, "ReserveAdjudicator")=0,"No", "Yes") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="Black" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlCompleteDelete" Visible="False" Width="100%" runat="server">
        <asp:Label ID="lblCompleteDeleteNotice" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>
