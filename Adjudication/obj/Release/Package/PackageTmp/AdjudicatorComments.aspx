<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AdjudicatorComments.aspx.vb" Inherits="Adjudication.AdjudicatorComments" Title="Adjudicator Comments" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
    <script type="text/javascript">
			function handleError() {				//To handle the error generated when using PRINT VIEW
				return true;
			}
			window.onerror = handleError;
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table class="TableSpacing">
        <tr>
            <td   valign="top" align="center" width="100%" colspan="2">
                <asp:Label ID="lblErrors" runat="server"   ForeColor="red" Visible="False"></asp:Label><asp:Label ID="lblStatus" runat="server"   ForeColor="Green" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td   valign="top" align="left" width="100%" colspan="2">
                <asp:Panel ID="pnlBallotInfo" runat="server">
                    <table class="TableSpacing">
                        <tr>
                            <td colspan="2">
                                <div class="separatorBlack">
                                    &nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontScoringHeader" align="center" colspan="2">
                                Production Information
                                <asp:TextBox ID="txtPK_CompanyID" runat="server" Visible="False"   BorderStyle="Dotted"   Width="64px">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="separatorBlack">
                                    &nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="430px">
                                <asp:CheckBox ID="chkShowAll" runat="server" CssClass="TextSmall" Visible="False" AutoPostBack="True" Text="&nbsp;List All Productions"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Production Name:
                            </td>
                            <td align="left" width="430px">
                                <span style="color: red;">
                                    <asp:DropDownList ID="ddlProductionID" runat="server"   Font-Bold="True" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblTitle" runat="server"   Font-Bold="True" Visible="False" ForeColor="red"></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="430px">
                                Company:
                            </td>
                            <td align="left" width="430px">
                                <span style="color: red;">
                                    <asp:Label ID="lblCompanyName" runat="server"   ForeColor="Black"></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="430px">
                                Venue:
                            </td>
                            <td align="left" width="430px">
                                <span style="color: red;">
                                    <asp:Label ID="lblVenueName" runat="server"   ForeColor="Black"></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Performance Dates:
                            </td>
                            <td align="left">
                                <asp:Label ID="lblFirstPerformanceDateTime" runat="server"   ForeColor="Black"></asp:Label>&nbsp;thru
                                <asp:Label ID="lblLastPerformanceDateTime" runat="server"   ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="separatorBlack">
                                    &nbsp;</div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlUserData" runat="server">
                    <table class="TableSpacing">
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestProduction" runat="server" Font-Italic="True">Production:</asp:Label>
                                <asp:Label ID="lblProductionName" runat="server"   ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestProductionComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblDirector" runat="server">Director:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblDirectorComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblMusicalDirector" runat="server">Musical Director:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblMusicalDirectorComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblChoreographer" runat="server">Choreographer:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblChoreographerComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblScenicDesigner" runat="server">Scenic Designer:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblScenicDesignerComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblLightingDesigner" runat="server">Lighting Designer:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblLightingDesignerComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblSoundDesigner" runat="server">Sound Designer:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblSoundDesignerComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblCostumeDesigner" runat="server">Costume Designer:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblCostumeDesignerComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblOriginalPlaywright" runat="server">Original Playwright:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblOriginalPlaywrightComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestPerformer1" runat="server">Best Actor/Actress #1:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestPerformer1Comment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestPerformer2" runat="server">Best Actor/Actress #2:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestPerformer2Comment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestSupportingActor1" runat="server">Best Supporting Actor #1:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestSupportingActor1Comment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestSupportingActor2" runat="server">Best Supporting Actor #2:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestSupportingActor2Comment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestSupportingActress1" runat="server">Best Supporting Actress #1:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestSupportingActress1Comment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblBestSupportingActress2" runat="server">Best Supporting Actress #2:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblBestSupportingActress2Comment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fontDataHeader" colspan="3" height="0px">
                                <asp:Label ID="lblAdjudicatorAttendance" runat="server"><i>Optional</i> Comments to Producing Theatre Company:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" width="100%" height="0px">
                                <asp:Label ID="lblAdjudicatorAttendanceComment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="separatorBlack">
                                    &nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <p>
                                    <asp:Button ID="btnPrinterFriendly" runat="server"   Width="200px" Text="Print View"></asp:Button>&nbsp;
                                    <asp:Button ID="btnEmailComments" runat="server"   Width="200px" Text="Email Comments To Me"></asp:Button></p>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
