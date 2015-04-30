Imports Adjudication.DataAccess

Partial Public Class Admin_WebsiteSettings
    Inherits System.Web.UI.Page
    Dim iAccessLevel As Int16
    Dim sLoginID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            GetDefaultValues()
        End If

    End Sub

    Private Sub GetDefaultValues()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_ApplicationDefaults()

        If dt.Rows.Count > 0 Then
            'moved MainpageApplicationDesc & MainpageApplicationNotes to separate webpage - save those values in viewstate 
            'so SProc doesnt need changing too,.
            ViewState("MainpageApplicationDesc") = dt.Rows(0)("MainpageApplicationDesc").ToString
            ViewState("MainpageApplicationNotes") = dt.Rows(0)("MainpageApplicationNotes").ToString
            Me.txtApplicationName.Text = dt.Rows(0)("ApplicationName").ToString
            Me.txtAdminContactName.Text = dt.Rows(0)("AdminContactName").ToString
            Me.txtAdminContactPhoneNum.Text = dt.Rows(0)("AdminContactPhoneNum").ToString
            Me.txtAdminContactEmail.Text = dt.Rows(0)("AdminContactEmail").ToString
            If dt.Rows(0)("NHTAYearStartDate").ToString.Length > 6 Then
                Me.txtNHTAYearStartDate.Text = CDate(dt.Rows(0)("NHTAYearStartDate").ToString).ToShortDateString
            End If
            If dt.Rows(0)("NHTAYearEndDate").ToString.Length > 6 Then
                Me.txtNHTAYearEndDate.Text = CDate(dt.Rows(0)("NHTAYearEndDate").ToString).ToShortDateString
            End If
            If dt.Rows(0)("NHTAwardsShowDate").ToString.Length > 6 Then
                Me.txtNHTAwardsShowDate.Text = CDate(dt.Rows(0)("NHTAwardsShowDate").ToString).ToShortDateString
            End If
            If dt.Rows(0)("ValidTrainingStartDate").ToString.Length > 6 Then
                Me.txtValidTrainingStartDate.Text = CDate(dt.Rows(0)("ValidTrainingStartDate").ToString).ToShortDateString
            End If
            Me.txtDaysToSubmitProduction.Text = dt.Rows(0)("DaysToSubmitProduction").ToString
            Me.txtDaysToAllowNominationEdits.Text = dt.Rows(0)("DaysToAllowNominationEdits").ToString
            Me.txtDaysToConfirmAttendance.Text = dt.Rows(0)("DaysToConfirmAttendance").ToString
            Me.txtDaysToWaitForScoring.Text = dt.Rows(0)("DaysToWaitForScoring").ToString
            Me.txtNumAdjudicatorsPerShow.Text = dt.Rows(0)("NumAdjudicatorsPerShow").ToString
            Me.txtMaxShowsPerAdjudicator.Text = dt.Rows(0)("MaxShowsPerAdjudicator").ToString
            Me.txtAdjudicatorCommentMinimumCharacterCount.Text = dt.Rows(0)("AdjudicatorCommentMinimumCharacterCount").ToString
            Me.txtScoringMinimum.Text = dt.Rows(0)("ScoringMinimum").ToString
            Me.txtScoringMaximum.Text = dt.Rows(0)("ScoringMaximum").ToString

            'Me.txtManualURL_Rules.Text = dt.Rows(0)("ManualURL_Rules").ToString
            'Me.txtManualURL_Admin.Text = dt.Rows(0)("ManualURL_Admin").ToString
            'Me.txtManualURL_Liaison.Text = dt.Rows(0)("ManualURL_Liaison").ToString
            'Me.txtManualURL_Adjudicator.Text = dt.Rows(0)("ManualURL_Adjudicator").ToString

            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString

        End If
    End Sub

    Public Sub btnDEFAULTS_Update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDEFAULTS_Update.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(30) As String, DateTester As DateTime
        '====================================================================================================
        Try
            DateTester = CDate(Me.txtNHTAwardsShowDate.Text)
        Catch ex As Exception
            Me.lblErrors.Text = "Error: You must Enter in a Date for the NH Theatre Awards Show"
            Me.lblErrors.Visible = True
            Exit Sub
        End Try

        Try
            DateTester = CDate(Me.txtValidTrainingStartDate.Text)
        Catch ex As Exception
            Me.lblErrors.Text = "Error: You must Enter in a Start Date for Adjudicators."
            Me.lblErrors.Visible = True
            Exit Sub
        End Try

        sDataValues(1) = Me.txtApplicationName.Text
        sDataValues(2) = ViewState("MainpageApplicationDesc")
        sDataValues(3) = ViewState("MainpageApplicationNotes") 'Me.ftbMainpageApplicationNotes.Text
        sDataValues(4) = Me.txtAdminContactName.Text
        sDataValues(5) = Me.txtAdminContactPhoneNum.Text
        sDataValues(6) = Me.txtAdminContactEmail.Text
        sDataValues(7) = Me.txtDaysToSubmitProduction.Text
        sDataValues(8) = Me.txtDaysToAllowNominationEdits.Text
        sDataValues(9) = Me.txtDaysToConfirmAttendance.Text
        sDataValues(10) = Me.txtDaysToWaitForScoring.Text
        sDataValues(11) = Me.txtScoringMinimum.Text
        sDataValues(12) = Me.txtScoringMaximum.Text
        sDataValues(13) = Master.UserLoginID
        sDataValues(14) = ""    'Me.txtManualURL_Rules.Text
        sDataValues(15) = ""    'Me.txtManualURL_Admin.Text
        sDataValues(16) = ""    'Me.txtManualURL_Liaison.Text
        sDataValues(17) = ""    'Me.txtManualURL_Adjudicator.Text
        sDataValues(18) = Me.txtNHTAwardsShowDate.Text
        sDataValues(19) = Me.txtMaxShowsPerAdjudicator.Text
        sDataValues(20) = Me.txtNumAdjudicatorsPerShow.Text
        sDataValues(21) = Me.txtValidTrainingStartDate.Text
        sDataValues(22) = Me.txtNHTAYearStartDate.Text
        sDataValues(23) = Me.txtNHTAYearEndDate.Text

        If ApplicationDefaults_Update(sDataValues) = True Then
            Me.lblErrors.Visible = False
            Me.lblSucessfulUpdate.Visible = True
        Else
            Me.lblErrors.Visible = True
            Me.lblSucessfulUpdate.Visible = False
        End If

    End Sub

End Class