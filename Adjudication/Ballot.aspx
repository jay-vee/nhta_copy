<%@ Page Language="vb" EnableTheming="false" ValidateRequest="true" MasterPageFile="~/MasterPage.Master" CodeBehind="Ballot.aspx.vb" Inherits="Adjudication.Ballot" Title="Ballot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="contentHead" runat="server" ContentPlaceHolderID="cphhead">
</asp:Content>

<asp:Content ID="contentMain" ContentPlaceHolderID="cphBody" runat="server">

    <div id="OLD_DISPLAY_BALLOT" style="display: none;">
        <table width="100%">
            <tr>
                <td valign="top" align="left" class="LabelLargeBold">Score:
                    <div style="padding-left: 30px; vertical-align: top;">
                        <asp:TextBox ID="txtScore" runat="server" CssClass="TextLargeBold" BackColor="Info" Width="38px" AutoPostBack="True" MaxLength="2">0</asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
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
                </td>
            </tr>
        </table>

        <asp:Panel ID="pnlScoringSave" runat="server" Visible="False">
            <table style="width: 1000px">
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
    </div>

    <asp:UpdatePanel ID="UpdatePanel_Main" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-md-12">

                        <div id="divNominatedProductionInformation" class="panel-group" role="tablist" aria-multiselectable="true">
                            <div id="divNominatedProductionHeading" class="panel panel-gold">
                                <div class="panel-heading" role="tab" id="headingOne">
                                    <div class="panel-title">
                                        <a data-toggle="collapse" data-parent="#divNominatedProductionInformation" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                            <span id="spnProductionName" runat="server"></span><span id="spnProducingCompanyName" class="pull-right" runat="server" style="font-style: italic;"></span>
                                        </a>
                                    </div>
                                </div>
                                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                    <div class="panel-body">
                                        <asp:Label ID="lblFullname" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label>
                                        for
                                <asp:Label ID="lblCompanyAdjudication" runat="server" Font-Italic="True"></asp:Label><br />
                                        <span id="spnProductionType" runat="server" class="FontItalic">musical</span>
                                        &nbsp;<span id="spnProductionCategory" runat="server"></span>
                                        <asp:Label ID="lblFirstPerformanceDateTime" runat="server"></asp:Label>
                                        - 
                                <asp:Label ID="lblLastPerformanceDateTime" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="divBallot" class="panel panel-dark">
                            <div class="panel-heading">
                                <div class="panel-title">
                                    <span id="spanTitle_CategoryName" runat="server">Frequently Asked Questions</span> <span id="spanTitle_NomineeName" runat="server" class="FontBold"></span>
                                </div>
                            </div>

                            <div id="divBallotDataEntry" runat="server" visible="false">
                                <div id="divBallotTop" class="row">
                                    <div id="divCriteria" class="col-md-6">
                                        <div class="panel">
                                            <div class="panel-body">
                                                <span class="FontLargeBold">
                                                    <asp:Label ID="lblCategoryName" runat="server"></asp:Label></span> <span class="FontLarge">Scoring Criteria</span>
                                                <p>
                                                    <asp:Label ID="lblScoringCriteria" runat="server" Width="100%" Text="Scoring Criteria goes here..."></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="divMatrix" class="col-md-6">
                                        <div class="panel">
                                            <div class="panel-body">
                                                <div class="well well-sm TextCenter" style="font-style: italic;">Click an appropriate Scoring 'Matrix' Range</div>
                                                <div class="panel-group" id="MatrixAccordion">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div id="divBallotScoresAndComments" class="col-md-12" role="form">
                                        <div class="col-md-4">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">
                                                        <label id="lblCatetoryName" runat="server"></label>
                                                    </h4>
                                                </div>
                                                <div class="panel-body">
                                                    <div id="divNominee" runat="server" class="form-control-static FontBold"></div>
                                                </div>
                                            </div>

                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">Adjudicated Score
                                                    </h4>
                                                </div>
                                                <div class="panel-body">
                                                    <input id="span_Score" runat="server" type="text" class="form-control" style="width: 40%; min-width: 80px;" placeholder="Range: 1-99" title="Set values for the Category Elements" data-bv-regexp="true" data-bv-regexp-regexp="^(1|[1-9][0-9]*)$" data-bv-stringlength-min="1" data-bv-stringlength-max="2" data-bv-stringlength="true" data-bv-notempty="true" data-bv-message="Please enter in a Score from 1 - 99 (numbers only)" />
                                                    <%--textbox 'txtScore_Calculated_For_Production' is displayed on the Best Production Comment/score--%>
                                                    <asp:TextBox ID="txtScore_Calculated_For_Production" runat="server" class="form-control" Style="width: 40%; min-width: 80px;" ToolTip="Calculated Best Production Score (Sum of Scores / # of Categories Nominated)" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-8">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">Adjudicated Comments</h4>
                                                </div>
                                                <div class="panel-body">
                                                    <div class="small FontItalic">Suggested Adjectives: <span id="spnSuggestedAdjectives" class="FontBold"></span></div>
                                                    <textarea id="span_Comment" rows="7" runat="server" class="form-control span12" placeholder="Enter your Score to view suggested Matrix Adjectives scoring range explanation" data-bv-stringlength-min="100" data-bv-stringlength="true" data-bv-notempty="true" data-bv-message="Please enter in a more meaningful and detailed Comment"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="text-center" style="padding-bottom: 0.5em;">
                                                <button id="btnSaveAndRemain" type="button" onclick="Record_Save(false); return false;" class="btn btn-gold"><span class="glyphicon glyphicon-floppy-disk"></span>&nbsp;Save and Remain</button>
                                                <button id="btnSaveAndContinue" type="button" onclick="Record_Save(true); return false;" class="btn btn-gold"><span class="glyphicon glyphicon-floppy-open"></span>&nbsp;Save and Continue</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divCommonQuestions" class="col-md-12" runat="server">
                                <div id="divFAQ" class="panel-body col-md-7">
                                    <b><span class="Question">Q</span>: When is this ballot due?</b><br />
                                    <span class="Answer">A</span>: Adjudication Assignments (ballots) are expected to be submitted on this website within
                                                        <asp:Label ID="lblDaysToWaitForScoring" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label>&nbsp;days of the assigned Productions closing date.
                                                        <br />
                                    <br />
                                    <b><span class="Question">Q</span>: How do I know I successfully submitted my Ballot on the Website?</b><br />
                                    <span class="Answer">A</span>: A successful submission of this Ballot will automatically send&nbsp;you an&nbsp;Email to your provided Email address. The Email will contain the submitted Scores and Comments.<br />
                                    <i>Note: Only <b>fully completed</b> ballots with all Scores and Comments can be submitted.</i><br />
                                    <br />
                                    <b><span class="Question">Q</span>: Who can see my Scores?</b><br />
                                    <span class="Answer">A</span>: Scores are never revealed. Scores are periodically reviewed by the NHTA Proctor to ensure they somewhat follow the criteria and fit the comments provided. Scores are used to calculate the winner for each Nomination Category.<br />
                                    <br />
                                    <b><span class="Question">Q</span>: Who reads my comments?</b><br />
                                    <span class="Answer">A</span>: Comments are periodically reviewed by the NHTA Proctor for content. Producing theatre company Liaisons will be able to retrieve previous years comments immediately after the NH Theatre Awards Show.<br />
                                    <br />
                                    <b>Adjudicator Assignment Guidelines</b><br />
                                    <ul>
                                        <li>Each Adjudicator can adjudicate up to a maximum of
                                                                <asp:Label ID="lblMaxShowsPerAdjudicator" runat="server" Font-Bold="true" Font-Names="Arial"></asp:Label>&nbsp;Productions, per year</li>
                                        <li>Each Adjudicator can adjudicate only <strong>1</strong> Production per Producing Theater Company, per year</li>
                                    </ul>
                                </div>

                                <div id="divAttendanceDate" class="panel-body col-md-5">
                                    <div class="panel panel-dark">
                                        <div class="panel-heading">Attendance & Playbill</div>
                                        <div class="panel-body">
                                            <table style="width: 100%; border-spacing: 4px; border-collapse: separate; text-align: left;">
                                                <tr>
                                                    <td style="width: 50%; text-align: right">Attendance Date:
                                                    </td>
                                                    <td style="width: 50%; text-align: left; vertical-align: top;">
                                                        <asp:TextBox ID="txtProductionDateAdjudicated_Planned" name="txtProductionDateAdjudicated_Planned" runat="server" CssClass="date-picker form-control" placeholder="Enter Date Attended"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%; text-align: right">NHTA Ad in Playbill:
                                                    </td>
                                                    <td style="width: 50%; text-align: left; vertical-align: top;">
                                                        <asp:DropDownList ID="ddlFoundAdvertisement" runat="server" CssClass="form-control" data-bv-package_validation="true" data-bv-greaterthan="true" data-bv-greaterthan-value="0" data-bv-greaterthan-message="Did you find the NHTA Playbill Advertisement?">
                                                            <asp:ListItem Text="Did you find the Ad?" Selected="True" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Forgot to look for it" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="width: 100%; text-align: center;">
                                                        <i>Optional</i> Comments to Producing Theatre Company:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="width: 100%">
                                                        <asp:TextBox ID="txtAdjudicatorAttendanceComment" runat="Server" CssClass="form-control" Rows="5" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="TextCenter">
                                                <button id="btnAttendanceDateandPlaybillSaveAndRemain" type="button" onclick="Record_Save(false); return false;" class="btn btn-gold"><span class="glyphicon glyphicon-floppy-disk"></span>&nbsp;Save and Remain</button>
                                                <button id="btnAttendanceDateandPlaybillSaveAndContinue" type="button" onclick="Record_Save(true); return false;" class="btn btn-gold"><span class="glyphicon glyphicon-floppy-open"></span>&nbsp;Save and Continue</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divMessages" class="col-md-12 container text-center">
                    <div class="row">
                        <div class="text-center alert alert-success alert-dismissable alertFeedbackMessage" style="display: inline-block; text-wrap: none;">
                            <%-- <button type="button" class="close pull-right" aria-hidden="true"><i class="fa fa-times"></i></button>--%>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <span id="spanFeedbackMessage" runat="server" style="display: inline-block; text-wrap: none;"></span>
                        </div>
                    </div>
                </div>
                <div class="clear">
                    <a name="aBottomAnchor"></a>
                </div>
            </div>

            <script id="scptGlobalsFromServerSide">
                var gScoringID = '<% =gScoringID%>';
                var gMenuNominationsJSON = '<% =gMenuNominationsJSON%>';
                var gMatrixRangesJSON = '<% =gMatrixRangesJSON%>';
                var gDisplayOrder = '<% =gDisplayOrder%>';
                var gAdjudicatorCommentMinimumCharacterCount = '<% =gAdjudicatorCommentMinimumCharacterCount%>';
                var gScoringRangeMin = '<% =gScoringRangeMin%>';
                var gScoringRangeMax = '<% =gScoringRangeMax%>';
            </script>

            <script id="scptFunctions">
                function scrollToAnchor(AnchorID) {
                    var aTag = $("a[name='" + AnchorID + "']");
                    $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
                }

                function ErrorDialog(FieldName, ErrorMessage) {
                    BootstrapDialog.show({
                        title: '<strong>DATA ENTRY PROBLEM</strong>',
                        message: "<p style='font-weight: bold;'>Please review the field '" + FieldName + "'</p><p style='margin: 1em;'>" + ErrorMessage + "</p>",
                        type: BootstrapDialog.TYPE_DANGER,
                        closable: false,
                        buttons: [{
                            label: '&nbsp;OK',
                            cssClass: 'btn-default',
                            action: function (dialogRef) {
                                dialogRef.close();
                            }
                        }]
                    });
                }

                function ValidateData() {
                    if (gDisplayOrder == 0) {
                        if ($("#cphBody_txtProductionDateAdjudicated_Planned").val().length < 4) {
                            ErrorDialog("Attendance Date", "Please enter in the date you attended the assigned Production.")
                            return false;
                        }
                        if ($("#cphBody_ddlFoundAdvertisement").val() == "0") {
                            ErrorDialog("NHTA Ad in Playbill", "Please let us know if you found the NH THeatre Awards Advertisement in this productions Playbill.")
                            return false;
                        }
                    }
                    else if (gDisplayOrder > 0) {
                        if ($("#cphBody_span_Score").val().length == 0) {
                            ErrorDialog("Adjudicated Score", "Please enter in a Score from " + gScoringRangeMin + " - " + gScoringRangeMax + " ")
                            return false;
                        }
                        if ($("#cphBody_span_Comment").val().length <= gAdjudicatorCommentMinimumCharacterCount) {
                            ErrorDialog("Adjudicated Comments", "Please enter in more comments about this Nominee.  Remember to have <ul><li>At least 1 positive comment</li><li>Specifics about a detail you noticed</li><li>A detail that could have been better</li><li>a constructive criticism <i>(optional)</i></li></ul>");
                            return false;
                        }
                        if (parseInt($("#cphBody_span_Score").val()) >= (gScoringRangeMax + 1)) {
                            ErrorDialog("Adjudicated Score", "Please enter in a Score that is less than: " + (gScoringRangeMax + 1) + " ")
                            return false;
                        }
                        if (parseInt($("#cphBody_span_Score").val()) <= (gScoringRangeMin - 1)) {
                            ErrorDialog("Adjudicated Score", "Please enter in a Score that is greater than: " + (gScoringRangeMin - 1) + " ")
                            return false;
                        }
                    }
                    return true;            //All Validations Passed!
                }

                function Record_Save(ContinueToNext) {
                    if (ValidateData() == true) {
                        if (ContinueToNext == true) {
                            __doPostBack('SAVE', gDisplayOrder);
                        } else {
                            __doPostBack('SAVEANDREMAIN', gDisplayOrder);
                        }
                        return false;
                    }
                }

                function Initialize_MenuNominations() {
                    if (gMenuNominationsJSON.length) {
                        var $menuJSON = $.parseJSON(gMenuNominationsJSON);

                        for (i = 0; i < $menuJSON.length; i++) {
                            var oListItem
                            if ($menuJSON[i].DisplayOrder == "-1") {
                                oListItem = "<li class='divider'></li>"
                            }
                            else {
                                if ($menuJSON[i].IsEnabled == false) {
                                    oListItem = "<li class='disabled'><a href='#'>";
                                }
                                else {
                                    oListItem = "<li><a href='Ballot.aspx?ScoringID=" + gScoringID + "&DisplayOrder=" + $menuJSON[i].DisplayOrder + "'>";
                                }
                                if ($menuJSON[i].EntryComplete == false) {
                                    oListItem += "<i class='fa fa-square-o fa-fw'  style='color:red;' aria-hidden='true'>&nbsp;</i>";
                                }
                                else {
                                    oListItem += "<i class='fa fa-check-square-o fa-fw' style='color:green;' aria-hidden='true'>&nbsp;</i>";
                                }
                                oListItem += $menuJSON[i].Category;
                                oListItem += "</a></li>";
                            }

                            $("#ulNominatedCategories").append(oListItem);
                        };
                    }
                }

                function Set_MatrixRangesForCategory() {
                    //=== Dynamically create Bootstrap Accordion panels from server side data in JSON format ===
                    var iScore = parseInt($("#cphBody_span_Score").val());
                    var ShowAccordionSelection = "";
                    var iSelectedMatrixIndex = -1;

                    if (gMatrixRangesJSON.length) {
                        var $MatrixRangeJSON = $.parseJSON(gMatrixRangesJSON);

                        for (i = 0; i < $MatrixRangeJSON.length; i++) {
                            //If existing Score, show Accordion that matches the range in the entered Score by adding 'in' to class for that accordion DIV
                            if (iScore > 0) {
                                if (iScore <= $MatrixRangeJSON[i].ScoringRangeMax && iScore >= $MatrixRangeJSON[i].ScoringRangeMin) {
                                    iSelectedMatrixIndex = i;
                                    ShowAccordionSelection = " in";
                                }
                            }
                            else {
                                ShowAccordionSelection = "";
                            }

                            var oRangeDIV = "";
                            oRangeDIV += "<div class='panel panel-dark'>";
                            oRangeDIV += "  <div class='panel-heading'>";
                            oRangeDIV += "    <h4 class='panel-title' data-toggle='collapse' data-parent='#MatrixAccordion' data-target='#collapsePanel_" + $MatrixRangeJSON[i].ScoringRangeMax + "'>";
                            oRangeDIV += "      Scoring 'Matrix' Range <strong>" + $MatrixRangeJSON[i].ScoringRangeMax + "-" + $MatrixRangeJSON[i].ScoringRangeMin + "</strong>";
                            oRangeDIV += "    </h4>";
                            oRangeDIV += "	</div>";
                            oRangeDIV += "  <div class='panel-collapse collapse " + ShowAccordionSelection + "' id='collapsePanel_" + $MatrixRangeJSON[i].ScoringRangeMax + "'>";
                            oRangeDIV += "    <div class='panel-body'>";
                            oRangeDIV += "      <p>" + $MatrixRangeJSON[i].MatrixDetail + "</p>";
                            oRangeDIV += "      <button type='button' onclick='SelectMatrixRange(" + i + "); return false;' class='btn btn-gold pull-right SelectRange'>Select this Range</button>";
                            oRangeDIV += "    </div>";
                            oRangeDIV += "  </div>";
                            oRangeDIV += "</div>";

                            $("#MatrixAccordion").append(oRangeDIV);
                        };

                        if (iSelectedMatrixIndex >= 0) {
                            SelectMatrixRange(iSelectedMatrixIndex);                    //Score is present, so run Function to select the index
                        }
                    }
                }

                function SelectMatrixRange(index) {
                    $MatrixRangeJSON = $.parseJSON(gMatrixRangesJSON);
                    //Click event to Scoring Matrix Range buttons
                    $("#divBallotScoresAndComments").show("fast");
                    scrollToAnchor("aBottomAnchor");
                    $("#spnSuggestedAdjectives").text($MatrixRangeJSON[index].MatrixAdjectives);
                    $("#span_Score").focus();
                }

                // ============== Start $(document).ready() ==============//
                $(document).ready(function () {
                    $(".date-picker").datepicker();

                    Initialize_MenuNominations();
                    Set_MatrixRangesForCategory();

                    if ($("#cphBody_spanFeedbackMessage").text.length > 1) {
                        $("#divMessages").show();
                    }
                    else {
                        $("#divMessages").hide();
                    }

                    //Score should be less than 100 and only Digits
                    $("#cphBody_span_Score").keypress(function (e) {
                        //if the letter is not digit then display error and don't type anything
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                        if ($("#cphBody_span_Score").val().length > 1) {        //zero length = 1 character; otherwise Nothing is returned
                            return false;
                        }
                    });

                    var iScore = parseInt($("#cphBody_span_Score").val());
                    if (iScore > 0) {
                        $("#spanCalculatedScore").show();
                        $("#divBallotScoresAndComments").show();
                    }
                    else {
                        $("#spanCalculatedScore").hide();
                        $("#divBallotScoresAndComments").hide();
                    }
                });
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
