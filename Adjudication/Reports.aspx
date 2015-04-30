<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Reports.aspx.vb" Inherits="Adjudication.Reports" Title="Reports"%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="2" width="100%">
            </td>
        </tr>
        <tr>
            <td id="oldLeftNav_DeleteThis_cell">
                
            </td>
            <td valign="top" align="left" width="100%">
                <table id="tblPageTitle" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td class="LabelMediumBold" align="left">
                            Reporting
                        </td>
                        <td   align="right">
                            Logged in as
                            <asp:Label ID="lblLoginID" runat="server" Font-Bold="True"  ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center"   colspan="2">
                            <br />
                            All Reports appear in <b>PopUp</b> windows.<br />
                            Please disable any PopUp blocking software you may have for this website.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="100%" align="center" valign="top"   colspan="2">
                <asp:Panel ID="pnlReports" runat="server" BorderStyle="Groove" Width="300px">
                    <table   cellspacing="0" cellpadding="0" width="600">
                        <tr>
                            <td>
                                <asp:Button ID="btnReportSelection_1" CssClass="LabelLarge" Width="195px" BorderStyle="None" runat="server" BackColor="WhiteSmoke" Text="Requests" ForeColor="DarkGray"></asp:Button>
                            </td>
                            <td bgcolor="gainsboro">
                                <font color="gainsboro">|</span>
                            </td>
                            <td>
                                <asp:Button ID="btnReportSelection_2" CssClass="LabelLarge" Width="195px" BorderStyle="None" runat="server" BackColor="LemonChiffon" Text="Scoring"></asp:Button>
                            </td>
                            <td bgcolor="gainsboro">
                                <font color="gainsboro">|</span>
                            </td>
                            <td>
                                <asp:Button ID="btnReportSelection_3" CssClass="LabelLarge" Width="195px" BorderStyle="None" runat="server" BackColor="WhiteSmoke" Text="Admin Lists" ForeColor="DarkGray"></asp:Button>
                            </td>
                            <td bgcolor="gainsboro">
                                <font color="gainsboro">|</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Panel ID="pnlReportSelection_1"   BorderStyle="None" runat="server" BackColor="LemonChiffon" Visible="false">
                                    <table   width="600" align="center">
                                        <tr>
                                            <td height="10">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center" height="300">
                                                <asp:RadioButtonList ID="rblReportList_1" runat="server" CssClass="LabelLarge" Width="280px" BackColor="LemonChiffon">
                                                    <asp:ListItem Value="AdjLateConfirm">Adjudicator Late Confirmation</asp:ListItem>
                                                    <asp:ListItem Value="AdjLateScoring">Adjudicator Late Scoring</asp:ListItem>
                                                    <asp:ListItem Value="AdjReassignRequest">Adjudicator Reassignment Requests</asp:ListItem>
                                                    <asp:ListItem Value="AdjLateBallot30_60_90" Selected="True">Late Ballots (30-60-90 day status)</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlReportSelection_2"   runat="server" BackColor="LemonChiffon" Visible="True">
                                    <table   width="600" align="center">
                                        <tr>
                                            <td height="10">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center" height="300" style="width: 100%">
                                                <table class="TableSpacing">
                                                    <tr>
                                                        <td align="right">
                                                            Production Category:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlFK_ProductionCategoryID" runat="server"  >
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            Production Type:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlFK_ProductionTypeID" runat="server"  >
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            Display Scores:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlDisplayScores" runat="server"  >
                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;<span class="FontSmaller"><em>"Scoring Summary" only</em></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            Sort Order:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlSortOrder" runat="server"  >
                                                                <asp:ListItem Value="1" Selected="True">by Production Name</asp:ListItem>
                                                                <asp:ListItem Value="0">by Score</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <em>&nbsp;<span class="FontSmaller">"Scoring Summary and Finalist Listing" only</span></em>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            Category:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlCategories" runat="server"   Enabled="False">
                                                                <asp:ListItem Value="Production">Production</asp:ListItem>
                                                                <asp:ListItem Value="Director">Director</asp:ListItem>
                                                                <asp:ListItem Value="Music Director">Music Director</asp:ListItem>
                                                                <asp:ListItem Value="Choreographer">Choreographer</asp:ListItem>
                                                                <asp:ListItem Value="Costume Designer">Costume Designer</asp:ListItem>
                                                                <asp:ListItem Value="Scenic Designer">Scenic Designer</asp:ListItem>
                                                                <asp:ListItem Value="Lighting Designer">Lighting Designer</asp:ListItem>
                                                                <asp:ListItem Value="Sound Designer">Sound Designer</asp:ListItem>
                                                                <asp:ListItem Value="Original Playwright">Original Playwright</asp:ListItem>
                                                                <asp:ListItem Value="Best Actor">Best Actor</asp:ListItem>
                                                                <asp:ListItem Value="Best Actress">Best Actress</asp:ListItem>
                                                                <asp:ListItem Value="Best Supporting Actor">Best Supporting Actor</asp:ListItem>
                                                                <asp:ListItem Value="Best Supporting Actress">Best Supporting Actress</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;<span class="FontSmaller"><em>"Scoring w/Adjudicator Detail" only</em></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TextSmall" align="right" colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            # Top Professional:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtTopXProfessional" runat="server"   Width="40px">5</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            # Top Community:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtTopXCommunity" runat="server"   Width="40px">10</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            # Top Youth:
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtTopXYouth" runat="server"   Width="40px">5</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <hr noshade />
                                                Select Report:
                                                <br />
                                                <asp:RadioButtonList ID="rblReportList_2" runat="server" CssClass="LabelLarge" Width="600px" BackColor="LemonChiffon" AutoPostBack="True">
                                                    <asp:ListItem Value="ScoringSummary" Selected="True">Scoring Summary</asp:ListItem>
                                                    <asp:ListItem Value="ScoringSummaryFinalists" Selected="True">Scoring for Publishing on Web or Press releases</asp:ListItem>
                                                    <asp:ListItem Value="ScoringSummaryDetail">Scoring Summary w/Detail</asp:ListItem>
                                                    <asp:ListItem Value="ScoringAdjudicatorDetail">Scoring w/Adjudicator Detail by Category</asp:ListItem>
                                                    <asp:ListItem Value="ScoringWinnersAndFinalists">Scoring Winners with Finalists (shows Ties)</asp:ListItem>
                                                    <asp:ListItem Value="ScoringFinalists_CardsForPresenters">Presenters Top X (use for Awards Show Envelopes)</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlReportSelection_3"   runat="server" BackColor="LemonChiffon" Visible="False">
                                    <table   width="600" align="center">
                                        <tr>
                                            <td height="10">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center" height="300">
                                                <asp:RadioButtonList ID="rblReportList_3" runat="server" CssClass="LabelLarge" Width="280px" BackColor="LemonChiffon">
                                                    <asp:ListItem Value="UserContactList">User</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlRunReport"   runat="server" BackColor="LemonChiffon">
                                    <table   width="600" align="center">
                                        <tr>
                                            <td>
                                                <hr noshade>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnRunSelectedReport"   Width="280px" runat="server" Text="Run Selected Report"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlReportType"   runat="server" Visible="False">
                                                    <asp:ListItem Value="PortableDocFormat">Adobe PDF File</asp:ListItem>
                                                    <asp:ListItem Value="WordForWindows">Word Document</asp:ListItem>
                                                    <asp:ListItem Value="Excel">Excel</asp:ListItem>
                                                    <asp:ListItem Value="WebPage" Selected="True">Web Page</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
