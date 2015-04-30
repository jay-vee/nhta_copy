<%@ Page Title="Ballot Summary" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" ValidateRequest="false" CodeBehind="BallotEntrySummary.aspx.vb" Inherits="Adjudication.BallotEntrySummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
	<script src="Includes/Functions.js" type="text/javascript"></script>
	<script>
		function setPrintControls() {
			//alert("setPrintControls: setting Print Controls");
			var myControl;
			myControl = document.getElementById("btnCloseWindow")
			myControl.style.visibility = "visible";

			var printControl;
			printControl = document.getElementById("btnPrint");
			printControl.style.visibility = "visible";
			//document.getElementById(""btnPrint"").focus();
			printControl.focus();
			window.print();
		}

		function handleError() {
			//To handle the error generated when using PRINT VIEW
			return true;
		}
		window.onerror = handleError;
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
	<table class="TableSpacing">
		<tr>
			<td>
				<table id="tblMain" class="TableSpacing">
					<tr>
						<td valign="top">
							<button id="btnPrint" style="visibility: hidden;" onclick="window.print();" type="button" class="btn btn-gold">
								Print Ballot</button>&nbsp;&nbsp;&nbsp;
							<button id="btnCloseWindow" onclick="window.close();" style="visibility: hidden;" type="button" class="btn btn-gold">
								Close Ballot</button>
						</td>
					</tr>
					<tr>
						<td valign="top" colspan="2">
							<div class="panel panel-dark">
								<div class="panel-heading">Production Information</div>
								<div class="panel-body">
									<asp:Panel ID="pnlBallotInfo" runat="server">
										<table class="TableSpacing">
											<tr>
												<td style="text-align: right; width: 40%">Production Name:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblTitle" runat="server" Font-Bold="True"></asp:Label>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Company:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblCompanyName" runat="server"></asp:Label><br />
													<asp:HyperLink ID="hlnkCompanyEmailAddress" runat="server" Target="_blank"></asp:HyperLink><br />
													<asp:HyperLink ID="hlnkCompanyWebsite" runat="server" Target="_blank"></asp:HyperLink>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Venue:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblVenueName" runat="server"></asp:Label><br />
													<asp:Label ID="lblAddress" runat="server"></asp:Label><br />
													<asp:Label ID="lblCity" runat="server"></asp:Label>&nbsp;
											<asp:Label ID="lblState" runat="server"></asp:Label>&nbsp;
											<asp:Label ID="lblZIP" runat="server"></asp:Label><br />
													<asp:HyperLink ID="hlnkWebsite" runat="server" Target="_blank"></asp:HyperLink>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Performance Dates:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblFirstPerformanceDateTime" runat="server"></asp:Label>&nbsp;thru
											<asp:Label ID="lblLastPerformanceDateTime" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Ticket Contact Information:
												</td>
												<td style="text-align: left; width: 60%">
													<p>
														Name:
												<asp:Label ID="lblTicketContactName" runat="server"></asp:Label><br />
														Phone#:
												<asp:Label ID="lblTicketContactPhone" runat="server"></asp:Label><br />
														Email:
												<asp:HyperLink ID="hlnkTicketContactEmail" runat="server" Target="_blank"></asp:HyperLink><br />
														<br />
														Details:
												<asp:Label ID="lblTicketPurchaseDetails" runat="server"></asp:Label>
													</p>
												</td>
											</tr>
										</table>
									</asp:Panel>
								</div>
							</div>

							<asp:Panel ID="pnlUserData" runat="server">
								<div class="TextCenter">
									<asp:Label ID="lblErrors" runat="server" Visible="False" CssClass="alert alert-danger" role="alert">Error</asp:Label>
									<asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" CssClass="alert alert-success" role="alert">Update Successful!</asp:Label>
								</div>
								<asp:TextBox ID="txtPK_ScoringID" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
								<asp:TextBox ID="txtPK_NominationsID" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
								<asp:TextBox ID="txtFK_CompanyID_Adjudicator" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
								<asp:TextBox ID="txtFK_UserID_Adjudicator" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
								<asp:TextBox ID="txtCountOfNominations" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
								<asp:TextBox ID="txtReserveAdjudicator" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
								<div class="panel panel-dark">
									<div class="panel-heading">Adjudication Information</div>
									<div class="panel-body">
										<table class="TableSpacing">
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Assigned Adjudicator:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblFullname" runat="server" Font-Bold="True"></asp:Label>
													<asp:TextBox ID="txtEmailPrimary" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>
													<asp:TextBox ID="txtEmailSecondary" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Adjudicator Affiliated Company:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblAdjudicatorCompany" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Adjudication Performed For Company:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblCompanyAdjudication" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Last Updated By:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblLastUpdateByName" runat="server"></asp:Label>&nbsp;on&nbsp;
														<asp:Label ID="lblLastUpdateByDate" runat="server"></asp:Label>
													<asp:TextBox ID="txtAdjudicatorRequestsReassignmentNote" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Initial Ballot Submit Date:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblBallotSubmitDate" runat="server" Font-Bold="true"></asp:Label>
												</td>
											</tr>

											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">
													<strong>Date Adjudicator Attended Performance:</strong>
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblProductionDateAdjudicated_Planned" runat="server" CssClass="Text_Blue" Font-Bold="true"></asp:Label>
													<asp:TextBox ID="txtProductionDateAdjudicated_Planned" runat="server" CssClass="Text_Blue" Font-Bold="true" Visible="false"></asp:TextBox>
													<cc1:CalendarExtender ID="CalendarExtender_txtProductionDateAdjudicated_Planned" runat="server" CssClass="AjaxCalendar" PopupButtonID="imgCalendarIcon_txtProductionDateAdjudicated_Planned" PopupPosition="BottomLeft" TargetControlID="txtProductionDateAdjudicated_Planned" />
													<asp:Image ID="imgCalendarIcon_txtProductionDateAdjudicated_Planned" runat="server" ImageUrl="~/Images/Calendar_16px.png" ToolTip="Select date from Calendar" CssClass="ImageButton" ImageAlign="Bottom" Visible="false" />
													<asp:RequiredFieldValidator ID="RequiredFieldValidator_txtProductionDateAdjudicated_Planned" ControlToValidate="txtProductionDateAdjudicated_Planned" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in the Date you attended the Production."></asp:RequiredFieldValidator>
													<cc1:ValidatorCalloutExtender ID="ReqValidatorCalloutExtender_txtProductionDateAdjudicated_Planned" runat="server" TargetControlID="RequiredFieldValidator_txtProductionDateAdjudicated_Planned" HighlightCssClass="validatorCalloutHighlight" />
													<cc1:MaskedEditExtender runat="server" ID="maskedEditExtender_txtProductionDateAdjudicated_Planned" TargetControlID="txtProductionDateAdjudicated_Planned" MaskType="Date" Mask="99/99/9999" Enabled="True">
													</cc1:MaskedEditExtender>
													<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator_txtProductionDateAdjudicated_Planned" ControlToValidate="txtProductionDateAdjudicated_Planned" Display="None" ValidationExpression="^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$" ErrorMessage="<b>Invalid Date</b><br />Please enter a valid Date value in the format:<br />MM/DD/YYYY" />
													<cc1:ValidatorCalloutExtender runat="Server" ID="ValidatorCalloutExtender_txtProductionDateAdjudicated_Planned" TargetControlID="RegularExpressionValidator_txtProductionDateAdjudicated_Planned" HighlightCssClass="validatorCalloutHighlight" />
													<asp:TextBox ID="txtProductionDateAdjudicated_Actual" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
													<asp:TextBox ID="txtAdjudicatorRequestsReassignment" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td style="text-align: right; width: 40%; vertical-align: top;">Found the <i>NH Theatre Awards 1/2 page Ad</i> in the Playbill:
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblFoundAdvertisement" runat="server" CssClass="Text_Blue" Font-Bold="true"></asp:Label>
													<asp:DropDownList ID="ddlFoundAdvertisement" runat="server" CssClass="Text_Blue" Font-Bold="true" Visible="false">
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
												<td style="text-align: right; width: 40%; vertical-align: top;">
													<asp:Label ID="lblAdjudicatorAttendanceCommentOptional" runat="server" Width="100%"><i>Optional</i> Comments to Producing Theatre Company:</asp:Label>
												</td>
												<td style="text-align: left; width: 60%">
													<asp:Label ID="lblAdjudicatorAttendanceComment" runat="server" Width="100%"></asp:Label>
												</td>
											</tr>
										</table>
										<table id="tblBallotNamesCommentScores" class="TableSpacing">
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblDirector" runat="server">Director:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="DirectorScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1px"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblDirectorComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblMusicalDirector" runat="server">Musical Director:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="MusicalDirectorScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblMusicalDirectorComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblChoreographer" runat="server">Choreographer:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="ChoreographerScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblChoreographerComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblScenicDesigner" runat="server">Scenic Designer:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="ScenicDesignerScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblScenicDesignerComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblLightingDesigner" runat="server">Lighting Designer:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="LightingDesignerScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblLightingDesignerComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblSoundDesigner" runat="server">Sound Designer:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="SoundDesignerScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblSoundDesignerComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblCostumeDesigner" runat="server">Costume Designer:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="CostumeDesignerScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblCostumeDesignerComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblOriginalPlaywright" runat="server">Original Playwright:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="OriginalPlaywrightScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblOriginalPlaywrightComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestPerformer1" runat="server">Best Actor/Actress #1:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestPerformer1Score" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestPerformer1Comment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestPerformer2" runat="server">Best Actor/Actress #2:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestPerformer2Score" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestPerformer2Comment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestSupportingActor1" runat="server">Best Supporting Actor #1:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestSupportingActor1Score" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestSupportingActor1Comment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestSupportingActor2" runat="server">Best Supporting Actor #2:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestSupportingActor2Score" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestSupportingActor2Comment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestSupportingActress1" runat="server">Best Supporting Actress #1:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestSupportingActress1Score" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestSupportingActress1Comment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestSupportingActress2" runat="server">Best Supporting Actress #2:</asp:Label>
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestSupportingActress2Score" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Nominee" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestSupportingActress2Comment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="LabelMedium" colspan="3">
													<asp:Label ID="lblBestProduction" runat="server">Best Production:</asp:Label>&nbsp;
												</td>
											</tr>
											<tr>
												<td valign="top" align="center" width="10%">
													<asp:TextBox ID="BestProductionScore" runat="server" CssClass="Text_Blue" Font-Bold="True" ToolTip="Please enter in the Score for this Category" Wrap="False" BorderStyle="None" BorderWidth="0">0</asp:TextBox>
												</td>
												<td valign="top" align="right" width="1"></td>
												<td valign="top" align="left" width="90%">
													<asp:Label ID="lblBestProductionComment" runat="server" CssClass="Text_Blue" Width="100%"></asp:Label>
												</td>
											</tr>
											<tr>
												<td align="center" colspan="4">
													<asp:Label ID="lblError2" runat="server" ForeColor="red"></asp:Label>
												</td>
											</tr>
											<tr>
												<td class="fontScoringHeader" align="center" colspan="4">
													<asp:Label ID="lblDaysToEditBallot" runat="server" ForeColor="White" Visible="False">Adjudicators have X days to Edit this Ballot after submitting</asp:Label>
												</td>
											</tr>
											<tr>
												<td align="center" colspan="4">
													<asp:Label ID="lblScoringInstructions" runat="server" CssClass="TextBold" ForeColor="Firebrick">Scoring Ranged Values are from X to XX</asp:Label>
													<asp:TextBox ID="txtScoringMinimum" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
													<asp:TextBox ID="txtScoringMaximum" runat="server" Visible="False" BorderStyle="Dotted">0</asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="center" colspan="4">
													<asp:Button ID="btnPrinterFriendly" runat="server" CssClass="btn btn-gold" Text="Print View"></asp:Button>&nbsp;
												<asp:Button ID="btnEmailMeThisBallot" runat="server" Text="Email Me Ballot" CssClass="btn btn-gold" />
												</td>
											</tr>
										</table>
									</div>
								</div>
							</asp:Panel>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Panel ID="pnlResults" runat="server" Visible="False">
					<asp:Label ID="lblStatus" runat="server"></asp:Label><br />
					<br />
					<br />
					<asp:Label ID="lblSaveResults" runat="server"></asp:Label>
				</asp:Panel>
			</td>
		</tr>
	</table>
</asp:Content>
