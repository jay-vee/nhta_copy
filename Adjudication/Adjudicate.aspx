<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Adjudicate.aspx.vb" Inherits="Adjudication.Adjudicate" Title="Adjudication Status" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .GridButtonSpacing {
            padding: 0 4px 0 4px;
        }

        .form-control {
            display: inline-block;
        }

        td {
            vertical-align: middle;
        }

        .listItemSpace td label {
            margin-right: 2em;
            margin-left: 0.5em;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
            <div id="divSucessfulUpdate" runat="server" visible="false" class="alert alert-success alert-dismissable alertFeedbackMessage">
                <button type="button" class="close" aria-hidden="true">&times;</button>
                <div style="text-align:center;">
                    <asp:Label ID="lblSucessfulUpdate" runat="server" CssClass="form-control-static">Update Successful</asp:Label>
                </div>
            </div>

            <asp:Panel ID="pnlGrid" Visible="True" runat="server">
                <div class="TextCenter row">
                    <asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Adjudications: 0</asp:Label>
                </div>
                <asp:DataGrid ID="gridMain" runat="server" Width="100%" BorderStyle="Double" BorderColor="#000000" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="PK_ScoringID" AutoGenerateColumns="False" AllowSorting="false" CellPadding="2" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect">
                    <FooterStyle ForeColor="#000000"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
                    <AlternatingItemStyle BackColor="LightGoldenrodYellow"></AlternatingItemStyle>
                    <ItemStyle ForeColor="#333333" Height="50px"></ItemStyle>
                    <HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="0"></asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="35" ItemStyle-CssClass="GridButtonSpacing">
                            <ItemTemplate>
                                <asp:Button ID="ibtnConfirm" runat="server" Visible="false" CommandName="Confirm_Command" CssClass="btn btn-gold" Text="Attendance" ToolTip="Assignment Attendance" />
                                <asp:Button ID="ibtnReassign" runat="server" Visible="false" CommandName="Reassign_Command" CssClass="btn btn-gold" Text="Reassign" ToolTip="Request Reassignment" />
                                <asp:Button ID="ibtnBallot" runat="server" Visible="false" CommandName="ScoreBallot_Command" CssClass="btn btn-gold" Text="Score" ToolTip="Score & Comment Ballot" />
                                <asp:Button ID="ibtnPrint" runat="server" Visible="false" CommandName="Print_Command" CssClass="btn btn-gold" Text="Print" ToolTip="Print the Ballot" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ScoringStatus" SortExpression="ScoringStatus" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Center" Width="160px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="160px"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TotalScore" SortExpression="TotalScore" HeaderText="Score">
                            <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Font-Bold="True"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductionCategory" SortExpression="ProductionCategory" HeaderText="Category">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Show Dates">
                            <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
                                <%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="8"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="9"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="10"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="11"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FK_VenueID" HeaderText="12"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="13"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="14"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="15"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="16"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="17"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FK_VenueID" HeaderText="18"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DaysToConfirmAttendance" HeaderText="19"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DaysToWaitForScoring" HeaderText="20"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignmentNote" HeaderText="21"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="BallotSubmitDate" HeaderText="22"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="ReserveAdjudicator" HeaderText="23"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="TotalScore" HeaderText="24"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="FK_ScoringStatusID" HeaderText="25"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="26"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </asp:Panel>

            <div class="row">
                <asp:Panel ID="pnlSelectedProductionDetail" Visible="False" runat="server">
                    <div class="col-md-6">
                        <asp:TextBox ID="txtPK_NominationID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
                        <asp:TextBox ID="txtFK_CompanyID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
                        <asp:TextBox ID="txtFK_VenueID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>
                        <div class="panel panel-dark">
                            <div class="panel-heading">Production Information</div>
                            <div class="panel-body">
                                <table class="TableSpacing">
                                    <tr>
                                        <td style="text-align: right; width: 40%">Production Name:
                                        </td>
                                        <td style="text-align: left; width: 60%">
                                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Company:
                                        </td>
                                        <td style="text-align: left; width: 60%">
                                            <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Company Email:
                                        </td>
                                        <td style="text-align: left; width: 60%">

                                            <asp:Label ID="CompanyEmailAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Company Website:
                                        </td>
                                        <td style="text-align: left; width: 60%">

                                            <asp:Label ID="CompanyWebsite" runat="server"></asp:Label></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Company Phone:
                                        </td>
                                        <td style="text-align: left; width: 60%">

                                            <asp:Label ID="CompanyPhone" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">
                                            <asp:LinkButton ID="lbtnViewVenue" runat="server" ToolTip="Click to view details of Venue">Venue:</asp:LinkButton>
                                        </td>
                                        <td style="text-align: left; width: 60%">
                                            <asp:Label ID="lblVenueName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Performance Dates:
                                        </td>
                                        <td style="text-align: left; width: 60%">
                                            <asp:Label ID="lblFirstPerformanceDateTime" runat="server"></asp:Label>&nbsp;thru
													<asp:Label ID="lblLastPerformanceDateTime" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Performance Dates Detail:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAllPerformanceDatesTimes" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Age Appropriate:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAgeAppropriateName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Ticket Contact Name:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTicketContactName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Ticket Contact Phone:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTicketContactPhone" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Ticket Contact Email:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTicketContactEmail" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 40%">Ticket Purchase Details:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTicketPurchaseDetails" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlSelectStatus" Visible="False" runat="server">
                    <div class="col-md-6">
                        <asp:TextBox ID="txtPK_ScoringID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
                        <div class="panel panel-dark">
                            <div class="panel-heading">Adjudication Status</div>
                            <div class="panel-body">
                                <table class="TableSpacing">
                                    <tr>
                                        <td style="text-align: right; width: 30%">Select status:
                                        </td>
                                        <td style="text-align: left; width: 70%">
                                            <asp:DropDownList ID="ddlPK_ScoringStatusID" runat="server" AutoPostBack="True" CssClass="form-control" Style="width: 100%;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDidNotAttend" runat="server" visible="false">
                                        <td colspan="2">Please explain why you did not attend this production.
											<asp:TextBox ID="txtAdjudicatorAttendanceComment" runat="server" Width="100%" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Enter in your reason for missing the assignment"></asp:TextBox><br />
                                            <span class="pull-right text-muted small"><strong>NOTE</strong>: <i>Producing company will see this comment.</i></span>
                                        </td>
                                    </tr>
                                    <tr id="trConfirmAttendanceDate" runat="server" visible="false">
                                        <td colspan="2">
                                            <div class="alert alert-warning" role="alert" style="text-align: center;">
                                                <b>ABOUT CONFIRMING YOUR ATTENDANCE DATE</b>
                                                <br />
                                                You are responsible for Contacting the Producing Theatre company to:
													<li>Confirm Attendance Date</li>
                                                <li>Reserve Ticket(s)</li>
                                                <i>NOTE:</i>This update will <b>NOT</b> notify the Producing Theatre company.<br />
                                                <span class="FontSmaller">This online confirmation is used to help the NH Theatre Awards staff resolve missed assignment issues</span>
                                            </div>
                                            <table class="TableSpacing">
                                                <tr>
                                                    <td style="text-align: right; width: 30%">Date to Adjudicate:
                                                    </td>
                                                    <td style="text-align: left; width: 70%">
                                                        <asp:TextBox ID="txtProductionDateAdjudicated_Planned" name="txtProductionDateAdjudicated_Planned" runat="server" Width="50%"
                                                            CssClass="date-picker form-control" placeholder="Date to Attend"></asp:TextBox>
                                                        <span class="text-muted small">&nbsp;[mm/dd/yy]</span>
                                                        <asp:TextBox ID="txtBallotSubmitDate" runat="server" Visible="False" Width="64px" BorderStyle="Dotted" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtAdjudicatorRequestsReassignmentNote" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in a Reason for this request."></asp:RequiredFieldValidator>
                                                        <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator_txtAdjudicatorRequestsReassignmentNote" HighlightCssClass="validatorCalloutHighlight" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">Total Score Submitted:
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalScore" runat="server" CssClass="FontBold">0</asp:Label>&nbsp;&nbsp;<span class="text-muted small"> [zero value means no scoring yet submitted]</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlReassignmentRequest" Visible="False" runat="server">
                    <div class="col-md-6">
                        <div class="panel panel-dark">
                            <div class="panel-heading">Request Reassignment</div>
                            <div class="panel-body">
                                <table class="TableSpacing">
                                    <tr>
                                        <td style="text-align: center; width: 100%;" colspan="2">
                                            <div class="alert alert-warning" role="alert" style="text-align: center;">
                                                <b>ABOUT REQUESTING REASSIGNMENTS</b><br />
                                                <br />
                                                When requesting a Replacement/Reassignment it is expected that you have made an effort to be replaced by another Adjudicators from your Theatre Company.<br />
                                                <br />
                                                Your Liaison(s) are available to assist you in finding a replacement.<br />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; width: 100%;" colspan="2">
                                            <asp:Label ID="lblCannotRequestReassignment" runat="server" CssClass="alert alert-danger" role="alert">ERROR: You cannot request a reassignment after the Production closes.</asp:Label>
                                        </td>
                                    </tr>
                                    <asp:Panel runat="server" ID="pnlReassignmentRequestControls" Visible="True">
                                        <tr>
                                            <td style="text-align: right; width: 40%">Reason for Reassignment Request:
                                            </td>
                                            <td style="text-align: left; width: 60%; vertical-align: top;">
                                                <asp:TextBox ID="txtAdjudicatorRequestsReassignmentNote" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtAdjudicatorRequestsReassignmentNote" ControlToValidate="txtAdjudicatorRequestsReassignmentNote" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in a Reason for this request."></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender_txtAdjudicatorRequestsReassignmentNote" runat="server" TargetControlID="RequiredFieldValidator_txtAdjudicatorRequestsReassignmentNote" HighlightCssClass="validatorCalloutHighlight" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 40%">Do you have a Replacement?
                                            </td>
                                            <td style="text-align: left; width: 60%">
                                                <asp:DropDownList runat="server" ID="ddlHasReplacement" AutoPostBack="true" CssClass="form-control">
                                                    <asp:ListItem Text="" Selected="True" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Did not try to find one in my Company" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddlHasReplacement" ControlToValidate="ddlHasReplacement" InitialValue="0" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please select a value indicating if you have a replacement or not."></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender_ddlHasReplacement" runat="server" TargetControlID="RequiredFieldValidator_ddlHasReplacement" HighlightCssClass="validatorCalloutHighlight" />
                                            </td>
                                        </tr>
                                        <tr id="trReplacementAdjudicatorName" runat="server" visible="false">
                                            <td style="text-align: right; width: 40%">
                                                <asp:Label runat="server" ID="lblReplacementName" Text="Name of Replacement"></asp:Label>
                                            </td>
                                            <td style="text-align: left; width: 60%">
                                                <asp:TextBox ID="txtReplacementName" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtReplacementName" ControlToValidate="txtReplacementName" Display="None" runat="server" ErrorMessage="<b>Required Field Missing</b><br />Please enter in the name of the person who will replace you."></asp:RequiredFieldValidator>
                                                <cc1:ValidatorCalloutExtender ID="ValidatorCalloutExtender_txtReplacementName" runat="server" TargetControlID="RequiredFieldValidator_txtReplacementName" HighlightCssClass="validatorCalloutHighlight" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </table>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <asp:Panel ID="pnlAdminInfo" Visible="False" runat="server">
                <div class="TextCenter">
                    <asp:Label ID="lblErrors" runat="server" Visible="False" CssClass="alert alert-danger" role="alert">Error</asp:Label>
                </div>
                <div class="panel panel-dark">
                    <div class="panel-heading">Administrative Information</div>
                    <div class="panel-body">
                        <table class="TableSpacing">
                            <tr>
                                <td style="text-align: right; width: 40%">Last Updated By:
                                </td>
                                <td style="text-align: left; width: 60%">
                                    <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
										<asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="TextCenter">
                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Update "></asp:Button>
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        function printOpen(iScoringID) {
            if (iScoringID == null) {
                alert("ERROR: No Ballot ID provided!");
            }
            else {
                var url;
                url = "BallotSummary.aspx?Print=True&ScoringID=" + iScoringID;
                window.open(url, "NHTheatreAwardBallot", 'width=680,height=700, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=yes,toolbar=yes');
            }
        }
        function Initialize_BootstrapValidator() {
            // ============== START validations using BootstrapValidator for Bootstrap 3.x ==============
            //$('#FormSiteMaster').bootstrapValidator({                                                   //Create validations using BootstrapValidator for Bootstrap
            //    message: 'This value is not valid',
            //    feedbackIcons: {
            //        valid: 'glyphicon glyphicon-ok',
            //        invalid: 'glyphicon glyphicon-remove',
            //        validating: 'glyphicon glyphicon-refresh'
            //    },
            //    fields: {
            //        ctl00$cphBody$txtProductionDateAdjudicated_Planned: {
            //            validators: {
            //                notEmpty: {
            //                    message: 'Please enter in the date you attended the production'
            //                },
            //                date: {
            //                    format: 'MM/DD/YYYY',
            //                    message: 'Please enter in a valid date [MM/DD/YYYY]'
            //                }
            //            }
            //        },
            //        submitHandler: function (validator, form, submitButton) {
            //            $("#cphMain_EMAIL_ADDRESS_NM").val($("#validate_EMAIL_ADDRESS_NM").val())           //IMPORTANT: put "validated" client side field values into server side hidden input field
            //            Record_Save();
            //        }
            //    }
            //});
            // ============== END validations using BootstrapValidator for Bootstrap 3.x ==============

        }

        $(document).ready(function () {
            $(".date-picker").datepicker();

            //Initialize_BootstrapValidator();
        });
    </script>
</asp:Content>
