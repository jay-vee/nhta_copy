<%@ Page Language="vb" EnableTheming="false" ValidateRequest="true" MasterPageFile="~/MasterPageNoNav.Master" CodeBehind="BallotEntry.aspx.vb" Inherits="Adjudication.BallotEntry" Title="Adjudicator Ballot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPageNoNav.master" %>
<asp:Content ID="contentHead" runat="server" ContentPlaceHolderID="cphhead">
</asp:Content>

<asp:Content ID="contentMain" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel ID="UpdatePanel_Main" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" align="left" style="width: 100%;;" colspan="2">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender_AssignmentInfo" runat="server" Collapsed="False" CollapseControlID="pnlAssignmentInfo_Title" CollapsedImage="~/Images/ArrowCollapse_Green_22px.png" ExpandControlID="pnlAssignmentInfo_Title" ExpandedImage="~/Images/ArrowExpand_Green_22px.png" ImageControlID="imgArrows_AssignmentInfo" SuppressPostBack="true" TargetControlID="pnlAssignmentInfo" TextLabelID="lblAssignmentInfo_title" CollapsedSize="1">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="pnlAssignmentInfo_Title" runat="server" CssClass="AccordionHeader" Height="24px" Width="100%" HorizontalAlign="Left" BorderWidth="1" BorderColor="#002469">
                            <div style="padding-left: 4px; padding-top: 2px">
                                <table id="Table1" border="0" cellpadding="0" cellspacing="0" style="width: 100%" class="LabelLargeBold">
                                    <tr class="">
                                        <td align="left" style="width: 30px" class="LabelLargeBold">
                                            <asp:Image ID="imgArrows_AssignmentInfo" runat="server" ImageAlign="AbsMiddle" AlternateText="Expand/Collapse" ImageUrl="~/Images/ArrowCollapse_Green_22px.png" ToolTip="Click to Expand or Collapse" />
                                        </td>
                                        <td align="left" style="width: 320px; font-weight: bold" class="LabelLargeBold">
                                            <asp:Label ID="lblAssignmentInfo_title" runat="server" CssClass="LabelLargeBold" Font-Bold="True">Production</asp:Label>
                                        </td>
                                        <td align="left" style="width: 320px; font-weight: bold" class="LabelLargeBold">Production Dates
                                        </td>
                                        <td align="left" style="width: 320px; font-weight: bold" class="LabelLargeBold">Assigned Adjudicator
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlAssignmentInfo" runat="server" Width="100%" CssClass="GradientHeader" HorizontalAlign="Left">
                            <table width="100%" class="TableBorder">
                                <tr>
                                    <td align="left" style="width: 30px"></td>
                                    <td valign="top" align="left" style="width: 320px">
                                        <asp:Label ID="lblTitle" runat="server"></asp:Label><br />
                                        &nbsp;&nbsp;by
                                        <asp:Label ID="lblCompanyName" runat="server" Font-Italic="True"></asp:Label>
                                    </td>
                                    <td valign="top" align="left" style="width: 320px">
                                        <asp:Label ID="lblFirstPerformanceDateTime" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label><br />
                                        &nbsp;&nbsp;&nbsp;thru<br />
                                        <asp:Label ID="lblLastPerformanceDateTime" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label>
                                    </td>
                                    <td valign="top" align="left" style="width: 320px">
                                        <asp:Label ID="lblFullname" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label><br />
                                        &nbsp;&nbsp;for<br />
                                        <asp:Label ID="lblCompanyAdjudication" runat="server" Font-Italic="True"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div style="height: 5px" class="separatorBlack">
                                &nbsp;
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 100%;;" colspan="2">
                        <asp:Panel ID="pnlStart" runat="server" Width="100%">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender_CommonQuestions" runat="server" Collapsed="False" CollapseControlID="pnlCommonQuestions_Title" CollapsedImage="~/Images/ArrowCollapse_Green_22px.png" ExpandControlID="pnlCommonQuestions_Title" ExpandedImage="~/Images/ArrowExpand_Green_22px.png" ImageControlID="imgArrows_CommonQuestions" SuppressPostBack="true" TargetControlID="pnlCommonQuestions" TextLabelID="lblCommonQuestions" CollapsedSize="1">
                                        </cc1:CollapsiblePanelExtender>
                                        <asp:Panel ID="pnlCommonQuestions_Title" runat="server" CssClass="AccordionHeader" Height="24px" Width="100%" HorizontalAlign="Left" BorderWidth="1" BorderColor="#002469">
                                            <div style="padding-left: 4px; padding-top: 2px">
                                                <table id="tblCommonQuestions_Title" border="0" cellpadding="0" cellspacing="0" style="width: 100%" class="LabelLargeBold">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Image ID="imgArrows_CommonQuestions" runat="server" ImageAlign="AbsMiddle" AlternateText="Expand/Collapse" ImageUrl="~/Images/ArrowCollapse_Green_22px.png" ToolTip="Click to Expand or Collapse" />
                                                            <asp:Label ID="lblCommonQuestions" runat="server" CssClass="LabelLargeBold" Font-Bold="True">&nbsp;Common Questions about Ballots</asp:Label>
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlCommonQuestions" runat="server" CssClass="GradientHeader" Width="100%">
                                            <table style="width: 100%;" class="TableBorder">
                                                <tr>
                                                    <td>
                                                        <b><span class="Question">Q</span>: When is this ballot due?</b><br />
                                                        <span class="Answer">A</span>: Adjudication Assignments (ballots) are expected to be submitted on this website within
                                                        <asp:Label ID="lblDaysToWaitForScoring" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label>&nbsp;days of the assigned Productions closing date.
                                                        <br />
                                                        <br />
                                                        <b><span class="Question">Q</span>: How do I know I successfully submitted my Ballot on the Website?</b><br />
                                                        <span class="Answer">A</span>: A successful submission of this Ballot will automatically send&nbsp;you an&nbsp;Email to your provided Email address. The Email will contain the submitted Scores and Comments.<br />
                                                        <lb>Note: Only <b>fully completed</b> ballots with all Scores and Comments can be submitted.<br />
                                                            <br />
                                                            <b><span class="Question">Q</span>: Who can see my Scores?</b><br />
                                                            <span class="Answer">A</span>: Scores are never revealed. Scores are periodically reviewed by the NHTA Proctor to ensure they somewhat follow the criteria and fit the comments provided. Scores are used to calculate the winner for each Nomination Category.<br />
                                                            <br />
                                                            <b><span class="Question">Q</span>: Who reads my comments?</b><br />
                                                            <span class="Answer">A</span>: Comments are periodically reviewed by the NHTA Proctor for content. Producing theatre company Liaisons will be able to retrieve previous years comments immediately after the NH Theatre Awards Show.<br />
                                                            <br />
                                                            Adjudicator assignment guidelines:
                                                            <lb>Each Adjudicator can adjudicate up to a maximum of
                                                                <asp:Label ID="lblMaxShowsPerAdjudicator" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label>&nbsp;Productions, per year
                                                                <lb>Each Adjudicator can adjudicate only <strong>1</strong> Production per Producing Theater Company, per year </lb>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" width="100%" colspan="2">
                                        <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align: left;">
                                            <tr>
                                                <td align="right" style="width: 50%" class="Label">Attended
                                                    <asp:Label ID="lblTitlePerformance" runat="server" Font-Bold="true"></asp:Label>
                                                    on:
                                                </td>
                                                <td valign="middle" align="left" style="width: 50%">
                                                    <asp:TextBox ID="txtProductionDateAdjudicated_Planned" runat="server" ForeColor="DarkBlue" Font-Bold="true" CssClass="TextBold" BackColor="Info" Width="96px" BorderStyle="Ridge" BorderColor="ActiveBorder"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender_txtProductionDateAdjudicated_Planned" runat="server" CssClass="AjaxCalendar" PopupButtonID="imgCalendarIcon_txtProductionDateAdjudicated_Planned" PopupPosition="BottomLeft" TargetControlID="txtProductionDateAdjudicated_Planned" />
                                                    <asp:Image ID="imgCalendarIcon_txtProductionDateAdjudicated_Planned" runat="server" ImageUrl="~/Images/Calendar_16px.png" ToolTip="Select date from Calendar" CssClass="ImageButton" ImageAlign="Bottom" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtProductionDateAdjudicated_Planned" ControlToValidate="txtProductionDateAdjudicated_Planned" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in the Date you attended the Production."></asp:RequiredFieldValidator>
                                                    <cc1:ValidatorCalloutExtender ID="ReqValidatorCalloutExtender_txtProductionDateAdjudicated_Planned" runat="server" TargetControlID="RequiredFieldValidator_txtProductionDateAdjudicated_Planned" HighlightCssClass="validatorCalloutHighlight" />
                                                    <cc1:MaskedEditExtender runat="server" ID="maskedEditExtender_txtProductionDateAdjudicated_Planned" TargetControlID="txtProductionDateAdjudicated_Planned" MaskType="Date" Mask="99/99/9999" Enabled="True">
                                                    </cc1:MaskedEditExtender>
                                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator_txtProductionDateAdjudicated_Planned" ControlToValidate="txtProductionDateAdjudicated_Planned" Display="None" ValidationExpression="^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$" ErrorMessage="<b>Invalid Date</b><br />Please enter a valid Date value in the format:<br />MM/DD/YYYY" />
                                                    <cc1:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender_txtProductionDateAdjudicated_Planned" TargetControlID="RegularExpressionValidator_txtProductionDateAdjudicated_Planned" HighlightCssClass="validatorCalloutHighlight" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width: 50%" class="Label">Found the <i>NH Theatre Awards 1/2 page Ad</i> in the Playbill:
                                                </td>
                                                <td valign="middle" align="left" style="width: 50%">
                                                    <asp:DropDownList ID="ddlFoundAdvertisement" runat="server" BackColor="Info" CssClass="TextBold" ForeColor="DarkBlue">
                                                        <asp:ListItem Text="" Selected="True" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Forgot to look for it" Value="3"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddlFoundAdvertisement" ControlToValidate="ddlFoundAdvertisement" InitialValue="0" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please select a value indicating if you found the NH Theatre Advertisement."></asp:RequiredFieldValidator>
                                                    <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender_ddlFoundAdvertisement" runat="server" TargetControlID="RequiredFieldValidator_ddlFoundAdvertisement" HighlightCssClass="validatorCalloutHighlight" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="width: 50%" class="LabelLargeBold">
                                                    <i>Optional</i> Comments to Producing Theatre Company:
                                                </td>
                                                <td align="left" style="width: 50%" class="Label"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left" style="width: 1000px">
                                                    <table cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td align="center" valign="top" style="width: 200px" class="TextSmall">
                                                                <br />
                                                                <br />
                                                                <br />
                                                                All comments will be available to the Producing Theater Liaisons when Adjudication Comments are made available to Liasions the day after the NH Theatre Awards Show.
                                                            </td>
                                                            <td align="left" style="width: 800px">
                                                                <asp:TextBox ID="txtAdjudicatorAttendanceComment" ForeColor="DarkBlue" runat="Server" BackColor="Info" Height="120px" Width="790px" TextMode="MultiLine">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:ImageButton ID="ibtn_NextStart" runat="server" ImageUrl="~/Images/Arrow_Right_Blue_80px.png" ToolTip="Next" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="99" style="height: 6px"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlBallotInfo" runat="server" Visible="False" Width="100%">
                            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender_ScoringCriteria" runat="server" Collapsed="False" CollapseControlID="pnlScoringCriteria_Title" CollapsedImage="~/Images/ArrowCollapse_Green_22px.png" ExpandControlID="pnlScoringCriteria_Title" ExpandedImage="~/Images/ArrowExpand_Green_22px.png" ImageControlID="imgArrows_ScoringCriteria" SuppressPostBack="true" TargetControlID="pnlScoringCriteria" TextLabelID="lblScoringCriteria_title" CollapsedSize="1">
                            </cc1:CollapsiblePanelExtender>
                            <asp:Panel ID="pnlScoringCriteria_Title" runat="server" CssClass="AccordionHeader" Height="24px" Width="100%" HorizontalAlign="Left" BorderWidth="1" BorderColor="#002469">
                                <div style="padding-left: 4px; padding-top: 2px">
                                    <table id="Table2" border="0" cellpadding="0" cellspacing="0" style="width: 100%" class="LabelLargeBold">
                                        <tr class="">
                                            <td align="left" style="width: 30px">
                                                <asp:Image ID="imgArrows_ScoringCriteria" runat="server" ImageAlign="AbsMiddle" AlternateText="Expand/Collapse" ImageUrl="~/Images/ArrowCollapse_Green_22px.png" ToolTip="Click to Expand or Collapse" />
                                            </td>
                                            <td align="left" style="width: 960px; font-weight: bold">"<asp:Label ID="lblScoringCriteria_title" runat="server" CssClass="LabelLargeBold" Font-Bold="True"></asp:Label>
                                            " Scoring Criteria
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlScoringCriteria" runat="server" Width="100%" HorizontalAlign="Left" CssClass="GradientHeader">
                                <table width="100%" class="TableBorder">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblScoringCriteria" runat="server" Width="100%" Text="Scoring Criteria goes here..."></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <tr>
                                <td align="left" width="60%" colspan="3"></td>
                            </tr>
                            <table class="TableSpacing">
                                <tr>
                                    <td align="left" width="97%" colspan="3">
                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="LabelLargeBold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 30px; width: 100%" align="left" colspan="3">
                                        <asp:Label ID="lblNomineeName" runat="server" CssClass="TextLargeBold" BorderStyle="None" ForeColor="DarkBlue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="100%" colspan="3">
                                        <table class="TableSpacing">
                                            <tr>
                                                <td valign="top" align="left" width="190px" class="LabelLargeBold">Score:
                                                    <div style="padding-left: 30px; vertical-align: top;">
                                                        <asp:TextBox ID="txtScore" runat="server" CssClass="TextLargeBold" BackColor="Info" Width="38px" AutoPostBack="True" MaxLength="2" BorderStyle="Ridge" BorderColor="ActiveBorder" ForeColor="DarkBlue">0</asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_Score" ControlToValidate="txtScore" Display="None" InitialValue="0" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in a Score."></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender_Score" runat="server" TargetControlID="RequiredFieldValidator_Score" HighlightCssClass="validatorCalloutHighlight" />
                                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator_ScoreNum" ControlToValidate="txtScore" Display="None" ValidationExpression="\d+" ErrorMessage="<b>Invalid Score</b><br />Please enter a valid Score value." />
                                                        <cc1:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender_ScoreNum" TargetControlID="RegularExpressionValidator_ScoreNum" HighlightCssClass="validatorCalloutHighlight" />
                                                        <br />
                                                        <asp:TextBox ID="txtScore_Calculated_For_Production" runat="server" CssClass="TextLargeBold" BorderStyle="Ridge" BorderColor="ActiveBorder" Font-Bold="true" BackColor="#cc9900" Width="38px" ForeColor="White" ToolTip="Calculated Best Production Score (Sum of Scores / # of Categories Nominated)" ReadOnly="True">0</asp:TextBox>
                                                    </div>
                                                </td>
                                                <td valign="middle" align="left" width="780px">
                                                    <cc1:Accordion ID="MyAccordion" runat="Server" SelectedIndex="0" HeaderCssClass="AccordionHeader" HeaderSelectedCssClass="AccordionHeader_Selected" ContentCssClass="AccordionContent" AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                                                        <Panes>
                                                        </Panes>
                                                        <HeaderTemplate>
                                                            Scoring Range:
                                                            <%#Eval("HeaderScoringRanges")%>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <div style="padding: 2px">
                                                                <%# Eval("MatrixDetail") %>
                                                            </div>
                                                        </ContentTemplate>
                                                    </cc1:Accordion>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td align="left" width="200px" valign="top">
                                        <asp:Label ID="Label1" runat="server" CssClass="LabelLargeBold">Comment:</asp:Label><br />
                                        <br />
                                        <div style="padding-left: 30px">
                                            <asp:Label ID="lblMatrixAdjectives" runat="server" CssClass="TextItalic">[Enter your <b>Score</b> to view recommended adjectives]</asp:Label>
                                        </div>
                                    </td>
                                    <td align="left" width="780px">
                                        <asp:TextBox ID="txtComment" runat="Server" ForeColor="DarkBlue" BackColor="Info" Height="150px" Width="780px" TextMode="MultiLine" BorderStyle="Ridge" BorderColor="ActiveBorder"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtComment" ControlToValidate="txtComment" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in a Comment for this category."></asp:RequiredFieldValidator>
                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender_txtComment" runat="server" TargetControlID="RequiredFieldValidator_txtComment" HighlightCssClass="validatorCalloutHighlight" />
                                    </td>
                                    <td align="right" width="10" valign="top">
                                        <asp:ImageButton ID="ibtnComment" runat="server" ToolTip="Expand/Contract" ImageUrl="Images/PlusIcon.jpg" />&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlNextPreviousButtons" runat="server" Visible="False">
                            <table cellpadding="0" cellspacing="0" style="width: 1000px">
                                <tr>
                                    <td align="center"></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtn_Previous" CausesValidation="false" runat="server" ImageUrl="~/Images/Arrow_Left_Blue_80px.png" ToolTip="Save score & Comment - move to Previous category" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtn_SaveCurrent" runat="server" CausesValidation="false" ImageUrl="~/Images/Icon_Save_50px.png" ToolTip="Save current category Score &amp; Comment" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtn_Next" CausesValidation="true" runat="server" ImageUrl="~/Images/Arrow_Right_Blue_80px.png" ToolTip="Save score & Comment - move to Next category" />
                                        <br />
                                        <asp:Label ID="lblSavedCurrent" runat="server" CssClass="TextBold" ForeColor="Green" Visible="false" Text="Saved current Score & Comment"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlScoringSave" runat="server" Visible="False">
                            <table cellpadding="0" cellspacing="0" style="width: 1000px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblDaysToEditBallot" runat="server" ForeColor="Black">NOTE: Adjudicators have X days to Edit this Ballot after submitting</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtn_Previous_ToSubmit" runat="server" ImageUrl="~/Images/Arrow_Left_Blue_80px.png" ToolTip="Previous" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtn_SubmitBallot" runat="server" ImageUrl="~/Images/Icon_Accept_GreenCheck_80px.png" ToolTip="Submit Ballot" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox ID="chkSendSubmitEmail" runat="server" Text="&nbsp;Send Email to Adjudicator on Submit" Checked="True"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblBallotToSubmit" runat="server" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlResults" runat="server" Visible="False" Width="98%">
                            <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align: left;">
                                <tr>
                                    <td valign="top" align="center" colspan="2" class="LabelLargeBold">
                                        <br />
                                        <strong><font color="green">Ballot Successfully Saved! </span></strong>
                                        <br />
                                        <br />
                                        <asp:Button ID="btnMainMenu" runat="server" Width="140px" Text="Return to Main Menu"></asp:Button><br />
                                        <br />
                                        <br />
                                        <asp:Label ID="lblStatus" runat="server" CssClass="LabelLarge"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="separatorBlack">
                                            &nbsp;
                                        </div>
                                        <br />
                                        <asp:Label ID="lblSaveResults" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align: left;">
                            <tr>
                                <td align="center" width="100%">
                                    <asp:Panel runat="server" ID="pnlErrors" Visible="false" Width="100%">
                                        <img src="Images/Icon_YellowExclamation_22px.png" alt="*ERROR*" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblErrors" runat="server" Width="90%" Height="20px" ForeColor="White" Backstyle="color: red;" CssClass="TextBold"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src="Images/Icon_YellowExclamation_22px.png" alt="*ERROR*" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="False" PostBackUrl="MainPage.aspx">Cancel/Main Menu</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
