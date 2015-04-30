<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdminReassignmentRequests.aspx.vb" Inherits="Adjudication.AdminReassignmentRequests" Title="Reassignment Requests" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="TextCenter">
        <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label><asp:Label ID="lblSuccessful" runat="server" Visible="False" ForeColor="Green">Adjudication Reassignment Successful</asp:Label>
    </div>
    <table class="TableSpacing">
        <tr>
            <td valign="top" align="left" width="100%" colspan="2">
                <asp:Panel ID="pnlGrid" runat="server">
                    <table id="tblMain" class="TableSpacing">
                        <tr>
                            <td align="center" width="100%" colspan="2">
                                <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Scores: 0</asp:Label>&nbsp;
                                <asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
                                <asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr>
                        <tr>
                            <td valign="top" colspan="2">
                                <asp:DataGrid ID="gridMain" runat="server" BorderColor="#000000" BorderStyle="Double" Width="100%" OnItemCommand="gridMain_ItemSelect" HorizontalAlign="Left" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" DataKeyField="PK_ScoringID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal">
                                    <FooterStyle ForeColor="#000000"></FooterStyle>
                                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
                                    <ItemStyle ForeColor="#333333"></ItemStyle>
                                    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" Text="Process" CommandName="Edit_Command" ForeColor="blue" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Adjudicator">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Scoring_CompanyName" SortExpression="Scoring_CompanyName" HeaderText="Represented Company">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" CssClass="TextSmall"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="Show Dates">
                                            <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="40"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="FirstPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="LastPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="FK_UserID_Adjudicator" HeaderText="FirstPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="FK_CompanyID_Adjudicator" HeaderText="LastPerformanceDateTime"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="VenueName" HeaderText="VenueName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="CreateByName" HeaderText="CreateByName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="CreateByDate" HeaderText="CreateByDate"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="PK_NominationsID"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignmentNote" HeaderText="AdjudicatorRequestsReassignmentNote"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="BallotSubmitDate" HeaderText="BallotSubmitDate"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="Address"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="City"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="State"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="Zip"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="Website"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="TicketContactName"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="TicketContactPhone"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="TicketContactEmail" HeaderText="27"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="CompanyWebsite" HeaderText="28"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="CompanyEmailAddress" HeaderText="29"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="30"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="ReserveAdjudicator" HeaderText="31"></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left" width="100%" colspan="2">
                <asp:Panel ID="pnlReassignAdjudicator" Visible="False" runat="server">
                    <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                        <tr>
                            <td class="fontScoringHeader" align="center" colspan="2">Assign Adjudicators for Production&nbsp;
                                <asp:TextBox ID="txtPK_NominationID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                <asp:TextBox ID="txtFK_CompanyID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                <asp:TextBox ID="txtPK_ProductionID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="330">Production Name:
                            </td>
                            <td align="left" width="330">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" ForeColor="#404040"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right" width="330">Company:
                            </td>
                            <td align="left" width="330">
                                <asp:Label ID="lblCompanyName" runat="server" ForeColor="#404040"></asp:Label><br />
                                <asp:Label ID="lblCompanyEmailAddress" runat="server" ForeColor="#404040"></asp:Label><br />
                                <asp:HyperLink ID="hlnkCompanyWebsite" runat="server"></asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right" width="330">Venue:
                            </td>
                            <td align="left" width="330">
                                <asp:Label ID="lblVenueName" runat="server" ForeColor="#404040"></asp:Label><br />
                                <asp:Label ID="lblAddress" runat="server" ForeColor="#404040"></asp:Label><br />
                                <asp:Label ID="lblCity" runat="server" ForeColor="#404040"></asp:Label>&nbsp;
                                <asp:Label ID="lblState" runat="server" ForeColor="#404040"></asp:Label>&nbsp;
                                <asp:Label ID="lblZIP" runat="server" ForeColor="#404040"></asp:Label><br />
                                <asp:HyperLink ID="hlnkWebsite" runat="server"></asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right">Performance Dates:
                            </td>
                            <td align="left">
                                <asp:Label ID="lblFirstPerformanceDateTime" runat="server" ForeColor="#404040"></asp:Label>&nbsp;thru
                                <asp:Label ID="lblLastPerformanceDateTime" runat="server" ForeColor="#404040"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="right">Ticket Contact Information:
                            </td>
                            <td align="left">
                                <p>
                                    Name:
                                    <asp:Label ID="lblTicketContactName" runat="server" ForeColor="#404040"></asp:Label><br />
                                    Phone#:
                                    <asp:Label ID="lblTicketContactPhone" runat="server" ForeColor="#404040"></asp:Label><br />
                                    Email:
                                    <asp:Label ID="lblTicketContactEmail" runat="server" ForeColor="#404040"></asp:Label>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align:left;">
                                    <tr>
                                        <td class="fontScoringHeader" align="center" colspan="3">Adjudication Reassignment Request
                                            <asp:TextBox ID="txtPK_ScoringID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                            <asp:TextBox ID="txtFK_UserID_Adjudicator" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                            <asp:TextBox ID="txtFK_CompanyID_Adjudicator" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                            <asp:TextBox ID="txtFK_UserID_Adjudicator_REPLACED" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                            <asp:TextBox ID="txt_FK_CompanyID_Adjudicator_REPLACED" runat="server" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr bgcolor="lemonchiffon">
                                        <td align="right" colspan="2">Number of Adjudicators required for Production:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNumAdjudicatorsPerShow" runat="server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr bgcolor="lemonchiffon">
                                        <td align="right" colspan="2">Maximum Shows Per Adjudicator:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblMaxShowsPerAdjudicator" runat="server" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr bgcolor="lemonchiffon">
                                        <td align="right" colspan="2" class="separatorBlack">Adjudicator must have received Training after:
                                        </td>
                                        <td align="left" class="separatorBlack">
                                            <asp:Label ID="lblValidTrainingStartDate" runat="server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="160"></td>
                                        <td align="center" width="350" bgcolor="lemonchiffon" class="LabelLargeBold" style="color: Firebrick">Requesting Adjudicator
                                        </td>
                                        <td align="center" width="350" class="LabelLargeBold">New Adjudicator
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="160">Adjudicator:
                                        </td>
                                        <td align="center" width="350" bgcolor="lemonchiffon">
                                            <asp:Label ID="lblCurrentAdjudicator" runat="server" ForeColor="Firebrick"></asp:Label>
                                        </td>
                                        <td align="left" width="350">
                                            <asp:DropDownList ID="ddlPK_UserID" runat="server" Width="320px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="160">Reserve Adjudicator:
                                        </td>
                                        <td align="center" width="350" bgcolor="lemonchiffon">
                                            <asp:Label ID="lblReserveAdjudicator" runat="server" ForeColor="Firebrick"></asp:Label>
                                        </td>
                                        <td align="left" width="350">
                                            <asp:DropDownList ID="ddlReserveAdjudicator" runat="server">
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 25px" align="right" width="160">Represented Company:
                                        </td>
                                        <td style="height: 25px" align="center" width="350" bgcolor="lemonchiffon">
                                            <asp:Label ID="lblCurrentRepresentedCompany" runat="server" ForeColor="Firebrick"></asp:Label>
                                        </td>
                                        <td style="height: 25px" align="left" width="350">
                                            <asp:DropDownList ID="ddlFK_CompanyID" runat="server" Width="320px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="right">Request Notes:
                                        </td>
                                        <td valign="top" align="left" width="350" bgcolor="lemonchiffon">
                                            <asp:Label ID="lblAdjudicatorRequestsReassignmentNote" runat="server" ForeColor="Firebrick"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:CheckBox ID="chkShowAssignmentCounts" runat="server" CssClass="TextSmall" AutoPostBack="True" ToolTip="Shows (#) in the drop down list, indicating the # of Adjudications already assigned" Text="&nbsp;Sort by Assignment Counts"></asp:CheckBox><br />
                                            <asp:CheckBox ID="chkIncludeBackupAdjudicators" runat="server" CssClass="TextSmall" AutoPostBack="True" ToolTip='Will list Backup Adjudicators in the "Adjudicator" drop down list.' Text="&nbsp;Include Backup Adjudicators"></asp:CheckBox>
                                            <asp:CheckBox ID="chkShowCompanyName" runat="server" CssClass="TextSmall" Visible="False" AutoPostBack="True" ToolTip=" List" Text="&nbsp;Show Company Name" Checked="True"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontScoringHeader" align="center" colspan="2">Administrative Information
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Last Updated By:
                            </td>
                            <td align="left">
                                <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                                <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TextSmall" align="center" bgcolor="lemonchiffon" colspan="2">
                                <asp:RadioButtonList ID="rblEmailInfo" runat="server" CssClass="TextSmall" Width="560px" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="NoAction">Do not Email</asp:ListItem>
                                    <asp:ListItem Value="EmailAssignmentToAll" Selected="True">Email Assignment to Representing Company and Producing Company members</asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                Administrator Comments to include in Email:<br />
                                <asp:TextBox ID="txtAdminEmailComments_ReAssign" CssClass="TextSmall" Width="600" runat="server" Height="40"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="separatorBlack">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnUpdate" runat="server" Width="120px" Text="Update "></asp:Button>&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnDelete" runat="server" Width="120px" Text="Cancel Request"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
