<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Admin_WebsiteSettings.aspx.vb" Inherits="Adjudication.Admin_WebsiteSettings" Title="Adjudication Date Settings (update annually)" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <style>
        .form-control {
            display: inline-block;
        }

        td {
            vertical-align: middle;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpdatePanelMain">
        <ContentTemplate>
            <div class="TextCenter">
                <asp:Label ID="lblErrors" runat="server" Visible="False" ForeColor="red"></asp:Label>
                <asp:Label ID="lblSucessfulUpdate" runat="server" Visible="False" ForeColor="SeaGreen" Font-Bold="True">Update Successful!</asp:Label>
            </div>
            <div class="panel panel-dark">
                <div class="panel-heading">Website Settings</div>
                <div class="panel-body">
                    <table class="TableSpacing">
                        <tr>
                            <td align="right" style="width: 40%">Application Name:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtApplicationName" runat="server" Width="60%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Admin Contact Name:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtAdminContactName" runat="server" Width="60%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Admin Contact Phone #:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtAdminContactPhoneNum" runat="server" Width="20%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Admin Contact Email Address:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtAdminContactEmail" runat="server" Width="60%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="panel panel-dark">
                <div class="panel-heading">Time Windows for Data&nbsp;Sumbmission/Edits&nbsp;(in Days)</div>
                <div class="panel-body">
                    <table class="TableSpacing">
                        <tr>
                            <td align="right" style="width: 40%">
                                <span style="color: red;">*</span>NHTA Calendar Year:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtNHTAYearStartDate" runat="server" Width="22%" Wrap="False" CssClass="form-control"></asp:TextBox>through
                        <asp:TextBox ID="txtNHTAYearEndDate" runat="server" Width="22%" Wrap="False" CssClass="form-control"></asp:TextBox>&nbsp;<span class="FontSmaller">(Productions)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">
                                <span style="color: red;">*</span>Date of NH Theatre Awards Show:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtNHTAwardsShowDate" runat="server" Width="22%" Wrap="False" CssClass="form-control"></asp:TextBox>&nbsp;(<span class="FontSmaller">Controls when Liaisons can view Comments)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">
                                <span style="color: red;">*</span>Use Adjudicators with Training Date After:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtValidTrainingStartDate" runat="server" Width="22%" Wrap="False" CssClass="form-control"></asp:TextBox>&nbsp;<span class="FontSmaller">(Adjudicators)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">To Submit a Production:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtDaysToSubmitProduction" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Liaisons)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">To Edit Nominations or Production Info:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtDaysToAllowNominationEdits" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Liaisons)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">To Confirm Show Attendance:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtDaysToConfirmAttendance" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Adjudicators)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">To Submit Show Scores:<br />
                                To Edit Ballots after Submitting:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtDaysToWaitForScoring" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Adjudicators)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%"># Adjudicators Per Production:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtNumAdjudicatorsPerShow" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Administrators)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 100%" colspan="2">
                                <span class="FontSmaller"><b>NOTE</b>: The # of ballots used to calculate scores will be 1 minus the # Adjudicators Per Production ((# Adjudicators Per Production) - 1)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Max # of Productions per Adjudicator:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtMaxShowsPerAdjudicator" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Administrators)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Scoring Ranges:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtScoringMinimum" runat="server" Width="50px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;to
                        <asp:TextBox ID="txtScoringMaximum" runat="server" Width="50px" Wrap="False" CssClass="form-control">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Minimum Characters in Comments:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtAdjudicatorCommentMinimumCharacterCount" runat="server" Width="64px" Wrap="False" CssClass="form-control">0</asp:TextBox>&nbsp;<span class="FontSmaller">(Ballot Entry)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 40%">Last Updated By:
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
                        <asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
            <div class="TextCenter">
                <asp:Button ID="btnDEFAULTS_Update" runat="server" CssClass="btn btn-gold" Text="Update "></asp:Button>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
