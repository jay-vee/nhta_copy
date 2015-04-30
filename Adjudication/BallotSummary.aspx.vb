Imports Adjudication.CustomMail
Imports Adjudication.DataAccess
Imports Adjudication.Common

Partial Public Class BallotSummary
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sLoginID As String = ""
    Dim iScoringID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '============================================================================================
            If IsTestMode() = True Then
                Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
                Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
                '============================================================================================
                iScoringID = "811"                      ' FOR TESTING ONLY
                '============================================================================================
            End If
            '============================================================================================
            'If Request.QueryString("ScoringID") = "" Then Response.Redirect("BallotSummary.aspx?ScoringID=811") ' FOR TESTING ONLY
            '===========================================================================================
            iAccessLevel = CInt(Session.Item("AccessLevel"))
            If Not (iAccessLevel > 0 And iAccessLevel <= 5) Then Response.Redirect("UnAuthorized.aspx")
            sLoginID = Session("LoginID")
            '============================================================================================
            'Redirect the user if the page times out
            Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
            '============================================================================================
            Master.PageTitleLabel = Me.Page.Title

            Me.lblErrors.Visible = False
            Me.lblError2.Visible = False

            If Not Request.QueryString("ScoringID") Is Nothing Or iScoringID > 0 Then

                If iScoringID = 0 Then iScoringID = Request.QueryString("ScoringID")

                If iAccessLevel = 1 Then

                Else
                    '=== Verify the User has access to view this Ballot ===
                    If User_has_Access_to_Ballot(CInt(Session("PK_UserID").ToString), iScoringID) = False Then
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "ERROR: You do not have access to view this Ballot!"
                        Me.lblError2.Visible = True
                        Me.lblError2.Text = "ERROR: You do not have access to view this Ballot!"
                        Exit Sub
                    End If
                End If

                If Not Request.QueryString("Print") Is Nothing Then
                    If Request.QueryString("Print").ToUpper = "TRUE" Then
                        ViewState("Print") = True
                    Else
                        ViewState("Print") = False
                    End If
                End If

                If Not IsPostBack Then
                    Call Get_ScoringRanges()
                    Call Populate_Data()
                End If

                If Request.QueryString("Print") = "True" Then Call SetPrinterFriendlyLayout()

            Else
                Me.pnlUserData.Visible = False
                Me.lblErrors.Text = "ERROR: No Score selected to Edit/Update"
                Me.lblError2.Text = "ERROR: No Score selected to Edit/Update"
                Me.lblErrors.Visible = True
                Me.lblError2.Visible = True
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Private Sub SetPrinterFriendlyLayout()
        Master.MenuVisible = False  'Hide the Menu for Printing

        Me.btnPrinterFriendly.Visible = False
        Me.btnEmailMeThisBallot.Visible = False
        Me.lblScoringInstructions.Visible = False

        Me.lblAdjudicatorAttendanceComment.Visible = True
        Me.lblAdjudicatorAttendanceCommentOptional.Visible = True

        Dim PageTitle As New HtmlGenericControl
        PageTitle.InnerText = "Print Ballot"

        If Not ClientScript.IsStartupScriptRegistered("StartUp") Then
            ClientScript.RegisterStartupScript(Page.GetType, "StartUp", "<script language=JavaScript>setPrintControls();</script>")
        End If

    End Sub

    Private Sub Get_ScoringRanges()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT TOP 1 ScoringMinimum, ScoringMaximum FROM ApplicationDefaults "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.txtScoringMinimum.Text = dt.Rows(0)("ScoringMinimum").ToString
            Me.txtScoringMaximum.Text = dt.Rows(0)("ScoringMaximum").ToString
            Me.lblScoringInstructions.Text = "Valid Scoring Ranges are from <B>" & Me.txtScoringMinimum.Text & "</B> to <B>" & Me.txtScoringMaximum.Text & "</B>"
        End If
    End Sub

    Private Sub Populate_ProductionDetails(ByVal ProductionID As String)
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        ' Get Production Information
        dt = Get_Production(ProductionID)

        If dt.Rows.Count > 0 Then
            Me.lblTitle.Text = dt.Rows(0)("Title").ToString & " - <i>" & dt.Rows(0)("ProductionType").ToString & "</i>"
            Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("ProductionCategory").ToString
            Me.hlnkCompanyEmailAddress.Text = dt.Rows(0)("CompanyEmailAddress").ToString
            Me.hlnkCompanyEmailAddress.NavigateUrl = Common.FormatHyperLinkEmail(dt.Rows(0)("CompanyEmailAddress").ToString)
            Me.hlnkCompanyWebsite.Text = Common.FormatHyperLink(dt.Rows(0)("CompanyWebsite").ToString)
            Me.hlnkCompanyWebsite.NavigateUrl = Me.hlnkCompanyWebsite.Text
            Me.lblFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.lblLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            Me.lblVenueName.Text = dt.Rows(0)("VenueName").ToString
            Me.lblAddress.Text = dt.Rows(0)("Address").ToString
            Me.lblCity.Text = dt.Rows(0)("State").ToString
            Me.lblState.Text = dt.Rows(0)("City").ToString
            Me.lblZIP.Text = dt.Rows(0)("Zip").ToString
            Me.hlnkWebsite.Text = Common.FormatHyperLinkEmail(dt.Rows(0)("Website").ToString)
            Me.hlnkWebsite.NavigateUrl = Me.hlnkWebsite.Text
            Me.lblTicketContactName.Text = dt.Rows(0)("TicketContactName").ToString
            Me.lblTicketContactPhone.Text = dt.Rows(0)("TicketContactPhone").ToString
            Me.hlnkTicketContactEmail.Text = dt.Rows(0)("TicketContactEmail").ToString
            Me.lblTicketPurchaseDetails.Text = dt.Rows(0)("TicketPurchaseDetails").ToString
        End If

        '====================================================================================================
        ' Get User and Scoring Information
        sSQL = "SELECT Users.LastName + ', ' + Users.FirstName as FullName, Company.CompanyName, " & _
                "       Users.EmailPrimary, Users.EmailSecondary, " & _
                "       Scoring.ReserveAdjudicator " & _
                "       , Company_1.CompanyName AS CompanyAdjudication " & _
                " FROM Users " & _
                "       INNER JOIN Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator " & _
                "       INNER JOIN Company ON Users.FK_CompanyID = Company.PK_CompanyID " & _
                "       INNER JOIN Company Company_1 ON Scoring.FK_CompanyID_Adjudicator = Company_1.PK_CompanyID" & _
                " WHERE Scoring.PK_ScoringID = " & iScoringID

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblFullname.Text = dt.Rows(0)("FullName").ToString
            Me.lblAdjudicatorCompany.Text = dt.Rows(0)("CompanyName").ToString
            Me.lblCompanyAdjudication.Text = dt.Rows(0)("CompanyAdjudication").ToString
            Me.txtEmailPrimary.Text = dt.Rows(0)("EmailPrimary").ToString
            Me.txtEmailSecondary.Text = dt.Rows(0)("EmailSecondary").ToString
            Me.txtReserveAdjudicator.Text = dt.Rows(0)("ReserveAdjudicator").ToString
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, dtDefaults As DataTable
        Dim bEnableControls As Boolean = True
        '====================================================================================================
        dtDefaults = DataAccess.Get_ApplicationDefaults

        If iAccessLevel = 1 Then      ' If not an admin, double check that user is assigned this adjudication
            dt = Get_Ballot(iScoringID.ToString)
        Else
            dt = Get_Ballot(iScoringID.ToString, Session("PK_UserID").ToString)
        End If

        If dt.Rows.Count > 0 Then
            '====================================================================================================
            '/// Check if already submitted scores can be EDITed///
            '==> dont disable if not using form to edit!!
            '====================================================================================================
            'If dt.Rows(0)("BallotSubmitDate").ToString.Length > 6 Then
            '    'Allow X number of days for an Adjudicator to edit their scores/comments after initial submission
            '    If Today > CDate(dt.Rows(0)("BallotSubmitDate").ToString).AddDays(CDbl(dtDefaults.Rows(0)("DaysToWaitForScoring").ToString)) Then
            '        If Not dt.Rows(0)("DirectorScore").ToString = "0" Then
            '            bEnableControls = False                                     ' the Ballot has already been submitted
            '        End If
            '    End If
            'End If

            'Allow Administrators to edit score
            If iAccessLevel = 1 Then bEnableControls = True

            'Enable or Disable the Panel containing all controls
            'Me.pnlUserData.Enabled = bEnableControls
            'Me.btnUpdate.Visible = bEnableControls

            '====================================================================================================
            '=== Assign Values from Database ===
            '====================================================================================================
            Me.txtPK_ScoringID.Text = dt.Rows(0)("PK_ScoringID").ToString
            Me.txtPK_NominationsID.Text = dt.Rows(0)("PK_NominationsID").ToString
            Me.txtFK_CompanyID_Adjudicator.Text = dt.Rows(0)("FK_CompanyID_Adjudicator").ToString
            Me.txtFK_UserID_Adjudicator.Text = dt.Rows(0)("FK_UserID_Adjudicator").ToString
            Me.txtAdjudicatorRequestsReassignment.Text = dt.Rows(0)("AdjudicatorRequestsReassignment").ToString
            Me.lblAdjudicatorAttendanceComment.Text = dt.Rows(0)("AdjudicatorAttendanceComment").ToString.Trim

            If dt.Rows(0)("ProductionDateAdjudicated_Planned").ToString.Length > 3 Then
                Me.lblProductionDateAdjudicated_Planned.Text = CDate(dt.Rows(0)("ProductionDateAdjudicated_Planned").ToString).ToShortDateString
                Me.txtProductionDateAdjudicated_Planned.Text = CDate(dt.Rows(0)("ProductionDateAdjudicated_Planned").ToString).ToShortDateString
            End If
            If dt.Rows(0)("ProductionDateAdjudicated_Actual").ToString.Length > 3 Then
                Me.txtProductionDateAdjudicated_Actual.Text = CDate(dt.Rows(0)("ProductionDateAdjudicated_Actual").ToString).ToShortDateString
            End If
            If dt.Rows(0)("FoundAdvertisement").ToString.Length > 0 Then
                Me.ddlFoundAdvertisement.SelectedValue = dt.Rows(0)("FoundAdvertisement").ToString
                Select Case dt.Rows(0)("FoundAdvertisement").ToString
                    Case "1"
                        Me.lblFoundAdvertisement.Text = "Yes"
                    Case "2"
                        Me.lblFoundAdvertisement.Text = "No"
                    Case "3"
                        Me.lblFoundAdvertisement.Text = "Forgot to look for it"
                    Case Else
                        Me.lblFoundAdvertisement.Text = "ERROR: No value selected"
                End Select
            End If

            If dt.Rows(0)("BallotSubmitDate").ToString.Length > 5 Then
                Me.lblBallotSubmitDate.Text = CDate(dt.Rows(0)("BallotSubmitDate").ToString).ToShortDateString()
                Me.lblDaysToEditBallot.Text = "Adjudicators have <b>" & dtDefaults.Rows(0)("DaysToWaitForScoring").ToString & "</b> days to edit this Ballot after submission."
            Else
                Me.lblBallotSubmitDate.Text = ""            'Now.Today.ToShortDateString
                Me.lblDaysToEditBallot.Visible = True
                Me.lblDaysToEditBallot.Text = "Adjudicators will have <b>" & dtDefaults.Rows(0)("DaysToWaitForScoring").ToString & "</b> days to edit this Ballot after submission."
            End If

            If dt.Rows(0)("Director").ToString.Length > 0 Then
                Me.lblDirector.Text = "<i>Director:</i> <b>" & dt.Rows(0)("Director").ToString & "</b>"
                Set_ScoringValues("Director", dt.Rows(0)("DirectorScore").ToString, dt.Rows(0)("DirectorComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "Director")
            End If

            If dt.Rows(0)("MusicalDirector").ToString.Length > 0 Then
                Me.lblMusicalDirector.Text = "<i>Musical Director:</i> <b>" & dt.Rows(0)("MusicalDirector").ToString & "</b>"
                Set_ScoringValues("MusicalDirector", dt.Rows(0)("MusicalDirectorScore").ToString, dt.Rows(0)("MusicalDirectorComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "MusicalDirector")
            End If

            If dt.Rows(0)("Choreographer").ToString.Length > 0 Then
                Me.lblChoreographer.Text = "<i>Choreographer:</i> <b>" & dt.Rows(0)("Choreographer").ToString & "</b>"
                Set_ScoringValues("Choreographer", dt.Rows(0)("ChoreographerScore").ToString, dt.Rows(0)("ChoreographerComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "Choreographer")
            End If

            If dt.Rows(0)("ScenicDesigner").ToString.Length > 0 Then
                Me.lblScenicDesigner.Text = "<i>Scenic Designer:</i> <b>" & dt.Rows(0)("ScenicDesigner").ToString & "</b>"
                Set_ScoringValues("ScenicDesigner", dt.Rows(0)("ScenicDesignerScore").ToString, dt.Rows(0)("ScenicDesignerComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "ScenicDesigner")
            End If

            If dt.Rows(0)("LightingDesigner").ToString.Length > 0 Then
                Me.lblLightingDesigner.Text = "<i>Lighting Designer:</i> <b>" & dt.Rows(0)("LightingDesigner").ToString & "</b>"
                Set_ScoringValues("LightingDesigner", dt.Rows(0)("LightingDesignerScore").ToString, dt.Rows(0)("LightingDesignerComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "LightingDesigner")
            End If

            If dt.Rows(0)("SoundDesigner").ToString.Length > 0 Then
                Me.lblSoundDesigner.Text = "<i>Sound Designer:</i> <b>" & dt.Rows(0)("SoundDesigner").ToString & "</b>"
                Set_ScoringValues("SoundDesigner", dt.Rows(0)("SoundDesignerScore").ToString, dt.Rows(0)("SoundDesignerComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "SoundDesigner")
            End If

            If dt.Rows(0)("CostumeDesigner").ToString.Length > 0 Then
                Me.lblCostumeDesigner.Text = "<i>Costume Designer:</i> <b>" & dt.Rows(0)("CostumeDesigner").ToString & "</b>"
                Set_ScoringValues("CostumeDesigner", dt.Rows(0)("CostumeDesignerScore").ToString, dt.Rows(0)("CostumeDesignerComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "CostumeDesigner")
            End If

            If dt.Rows(0)("OriginalPlaywright").ToString.Length > 0 Then
                Me.lblOriginalPlaywright.Text = "<i>Original Playwright:</i> <b>" & dt.Rows(0)("OriginalPlaywright").ToString & "</b>"
                Set_ScoringValues("OriginalPlaywright", dt.Rows(0)("OriginalPlaywrightScore").ToString, dt.Rows(0)("OriginalPlaywrightComment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "OriginalPlaywright")
            End If

            If dt.Rows(0)("BestPerformer1Name").ToString.Length > 0 Then
                Me.lblBestPerformer1.Text = "<i>Best " & dt.Rows(0)("BestPerformer1Gender").ToString & " #1:</i> <b>" & dt.Rows(0)("BestPerformer1Name").ToString & "</b> as '" & dt.Rows(0)("BestPerformer1Role").ToString & "'"
                Set_ScoringValues("BestPerformer1", dt.Rows(0)("BestPerformer1Score").ToString, dt.Rows(0)("BestPerformer1Comment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "BestPerformer1")
            End If

            If dt.Rows(0)("BestPerformer2Name").ToString.Length > 0 Then
                Me.lblBestPerformer2.Text = "<i>Best " & dt.Rows(0)("BestPerformer2Gender").ToString & " #2:</i> <b>" & dt.Rows(0)("BestPerformer2Name").ToString & "</b> as '" & dt.Rows(0)("BestPerformer2Role").ToString & "'"
                Set_ScoringValues("BestPerformer2", dt.Rows(0)("BestPerformer2Score").ToString, dt.Rows(0)("BestPerformer2Comment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "BestPerformer2")
            End If

            If dt.Rows(0)("BestSupportingActor1Name").ToString.Length > 0 Then
                Me.lblBestSupportingActor1.Text = "<i>Best Supporting Actor #1:</i> <b>" & dt.Rows(0)("BestSupportingActor1Name").ToString & "</b> as '" & dt.Rows(0)("BestSupportingActor1Role").ToString & "'"
                Set_ScoringValues("BestSupportingActor1", dt.Rows(0)("BestSupportingActor1Score").ToString, dt.Rows(0)("BestSupportingActor1Comment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "BestSupportingActor1")
            End If

            If dt.Rows(0)("BestSupportingActor2Name").ToString.Length > 0 Then
                Me.lblBestSupportingActor2.Text = "<i>Best Supporting Actor #2:</i> <b>" & dt.Rows(0)("BestSupportingActor2Name").ToString & "</b> as '" & dt.Rows(0)("BestSupportingActor2Role").ToString & "'"
                Set_ScoringValues("BestSupportingActor2", dt.Rows(0)("BestSupportingActor2Score").ToString, dt.Rows(0)("BestSupportingActor2Comment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "BestSupportingActor2")
            End If

            If dt.Rows(0)("BestSupportingActress1Name").ToString.Length > 0 Then
                Me.lblBestSupportingActress1.Text = "<i>Best Supporting Actress #1:</i> <b>" & dt.Rows(0)("BestSupportingActress1Name").ToString & "</b> as '" & dt.Rows(0)("BestSupportingActress1Role").ToString & "'"
                Set_ScoringValues("BestSupportingActress1", dt.Rows(0)("BestSupportingActress1Score").ToString, dt.Rows(0)("BestSupportingActress1Comment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "BestSupportingActress1")
            End If

            If dt.Rows(0)("BestSupportingActress2Name").ToString.Length > 0 Then
                Me.lblBestSupportingActress2.Text = "<i>Best Supporting Actress #2:</i> <b>" & dt.Rows(0)("BestSupportingActress2Name").ToString & "</b> as '" & dt.Rows(0)("BestSupportingActress2Role").ToString & "'"
                Set_ScoringValues("BestSupportingActress2", dt.Rows(0)("BestSupportingActress2Score").ToString, dt.Rows(0)("BestSupportingActress2Comment").ToString.Trim, bEnableControls)
            Else
                ShowHide_Scoring(False, "BestSupportingActress2")
            End If

            '=== Get Production Info now ===
            Call Populate_ProductionDetails(dt.Rows(0)("FK_ProductionID").ToString)

            Me.lblBestProduction.Text = "<i>Best Production:</i> <b>" & Me.lblTitle.Text & "</b>"
            Me.BestProductionScore.Text = "Score: " & dt.Rows(0)("BestProductionScore").ToString
            Me.lblBestProductionComment.Text = dt.Rows(0)("BestProductionComment").ToString

            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString

            If ViewState("Print") = True Then
                If dt.Rows(0)("BestProductionScore").ToString = "0" Then
                    Me.lblAdjudicatorAttendanceCommentOptional.Visible = False
                    Me.lblAdjudicatorAttendanceComment.Visible = False

                    Me.lblBestProduction.Visible = False
                    Me.BestProductionScore.Visible = False
                    Me.lblLastUpdateByName.Visible = False
                    Me.lblLastUpdateByDate.Visible = False
                End If
            End If

        Else
            Me.pnlUserData.Visible = False
            Me.lblErrors.Text = "<i>ERROR: You are not Assigned this Adjudication."
            Me.lblError2.Text = "<i>ERROR: You are not Assigned this Adjudication."
            Me.lblErrors.Visible = True
            Me.lblError2.Visible = True
        End If

    End Sub

    Private Sub Set_ScoringValues(ByVal ControlName As String, ByVal Score As String, ByVal Comments As String, Optional ByVal bEnableControls As Boolean = True)
        '====================================================================================================
        Dim txt As New TextBox, lbl As New Label ', txtComm As New FreeTextBoxControls.FreeTextBox
        '====================================================================================================
        Try
            txt = Me.pnlUserData.FindControl(ControlName & "Score")                 ' Score
            txt.Text = "Score: " & Score
            txt.ReadOnly = True
            'JLV 3/1/11 - Made This page a read only page
            'txtComm = Me.pnlUserData.FindControl(ControlName & "Comment")           ' Editable Comments
            'txtComm.Text = Comments
            lbl = Me.pnlUserData.FindControl("lbl" & ControlName & "Comment")       ' Label Comments
            lbl.Text = Comments
            'Me.txtCountOfNominations.Text = CInt(Me.txtCountOfNominations.Text) + 1

            'If bEnableControls = False Then
            '    txt.Enabled = False
            'End If

            If ViewState("Print") = True Then
                If Score = "0" Then
                    txt.Visible = False
                    'txtComm.Visible = False
                    lbl.Visible = False
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ShowHide_Scoring(ByVal IsVisible As Boolean, ByVal ControlName As String)
        '====================================================================================================
        Dim txt As New TextBox, lbl As New Label    ', txtComm As New FreeTextBoxControls.FreeTextBox
        '====================================================================================================
        Try
            txt = Me.pnlUserData.FindControl(ControlName & "Score")             ' Score
            txt.Visible = IsVisible
            lbl = Me.pnlUserData.FindControl("lbl" & ControlName)               ' Header Label Name
            lbl.Visible = IsVisible
            lbl = Me.pnlUserData.FindControl("lbl" & ControlName & "Comment")   ' Label Comments
            lbl.Visible = IsVisible

            If ViewState("Print") = True Then
                txt.Visible = False
                'txtComm.Visible = False
                lbl.Visible = False
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function Validate_ScoresComments(ByVal ControlName As String) As Boolean
        '====================================================================================================
        Dim txt As New TextBox, lbl As New Label, lblName As Label, CategoryName As String = ""
        Dim txtComm As New FreeTextBoxControls.FreeTextBox
        Dim ReturnValue As Boolean = False
        Dim ScoreMin As Int16, ScoreMax As Int16
        '====================================================================================================
        Dim iMinCommentCount = CInt(ConfigurationManager.AppSettings("AdjudicatorCommentMinimumCharacterCount"))

        Try
            txt = Me.pnlUserData.FindControl(ControlName & "Score")                         ' Score textbox

            If txt.Visible = True Then
                lblName = Me.pnlUserData.FindControl("lbl" & ControlName)                   ' Name Label
                CategoryName = lblName.Text.Split(":")(0)

                ScoreMin = CInt(Me.txtScoringMinimum.Text)
                ScoreMax = CInt(Me.txtScoringMaximum.Text)
                If CInt(txt.Text) < ScoreMin Or CInt(txt.Text) > ScoreMax Then
                    Me.lblErrors.Text = "ERROR: For <b>" & lblName.Text & "</b>, please provide a valid 'Score' between the ranges of <b>" & Me.txtScoringMinimum.Text & "</b> and <b>" & Me.txtScoringMaximum.Text & "</b>"
                    Me.lblError2.Text = Me.lblErrors.Text
                    Me.lblErrors.Visible = True
                    Me.lblError2.Visible = True
                Else
                    txtComm = Me.pnlUserData.FindControl(ControlName & "Comment")           ' Editable Comments
                    If txtComm.Text.Length <= iMinCommentCount Then                         ' Check Length of Comments
                        Me.lblErrors.Text = "ERROR: You must submit <b>Comments</b> for the <b>" & lblName.Text & "</b> . "
                        Me.lblError2.Text = Me.lblErrors.Text
                        Me.lblErrors.Visible = True
                        Me.lblError2.Visible = True
                    Else
                        If txtComm.Text.Length >= 8000 Then                                 ' Check Length of Comments
                            Me.lblErrors.Text = "ERROR: Your comments for <b>" & lblName.Text & "</b> exceed 8000 Characters. "
                            Me.lblError2.Text = Me.lblErrors.Text
                            Me.lblErrors.Visible = True
                            Me.lblError2.Visible = True
                        Else
                            ReturnValue = True
                        End If
                    End If
                End If
            Else
                ReturnValue = True
            End If

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: Please provide a VALID <b>" & CategoryName & "</b> score value " & " between the ranges of <b>" & Me.txtScoringMinimum.Text & "</b> and <b>" & Me.txtScoringMaximum.Text & "</b>"
            Me.lblError2.Text = Me.lblErrors.Text
            Me.lblErrors.Visible = True
            Me.lblError2.Visible = True
        End Try

        lbl = Me.pnlUserData.FindControl("lbl" & ControlName & "Comment")   ' Label Comments
        lbl.Text = ""       'txtComm.Text

        Validate_ScoresComments = ReturnValue

    End Function

    Private Sub EmailCompletedBallot()
        '====================================================================================================
        Dim sMsg As String = "", sSubject As String, sTo As String, sFrom As String
        Dim iScore As Integer = 0
        '====================================================================================================
        Try
            sMsg = Get_Ballot_as_HTML(CInt(ViewState("PK_ScoringID").ToString), CInt(ViewState("FK_NominationsID").ToString))
            sMsg = sMsg & "Ballot Submission performed by <B>" & ViewState("LoginID") & "</b> on " & Now.ToLongDateString & " at " & Now.ToShortTimeString

            sTo = Session.Item("EmailPrimary").ToString                'Me.txtEmailPrimary.Text & ", " & Me.txtEmailSecondary.Text
            sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString

            sSubject = "Ballot for " & Me.lblTitle.Text

            ' Send the Email in HTML Format
            SendCDOEmail(sFrom, sTo, False, sSubject, sMsg, False, True, Session("LoginID"), EMAIL_BALLOT_SUBMITTED)

            Me.lblStatus.Text = "An Email has sent for Production " & Me.lblTitle.Text & " with the following information:"
            sMsg = "<b>Emailed To:</b> " & sTo & "<br /><hr noshade>" & sMsg

            Me.pnlBallotInfo.Visible = False
            Me.pnlResults.Visible = True
            Me.lblSaveResults.Text = sMsg

        Catch ex As Exception
            'Throw
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
            Me.lblError2.Text = Me.lblErrors.Text
            Me.lblError2.Visible = True
        End Try

        '=== OLD PROCESS ===
        'iScore = iScore + CInt(Me.DirectorScore.Text)
        'iScore = iScore + CInt(Me.MusicalDirectorScore.Text)
        'iScore = iScore + CInt(Me.ChoreographerScore.Text)
        'iScore = iScore + CInt(Me.ScenicDesignerScore.Text)
        'iScore = iScore + CInt(Me.LightingDesignerScore.Text)
        'iScore = iScore + CInt(Me.SoundDesignerScore.Text)
        'iScore = iScore + CInt(Me.CostumeDesignerScore.Text)
        'iScore = iScore + CInt(Me.OriginalPlaywrightScore.Text)
        'iScore = iScore + CInt(Me.BestPerformer1Score.Text)
        'iScore = iScore + CInt(Me.BestPerformer2Score.Text)
        'iScore = iScore + CInt(Me.BestSupportingActor1Score.Text)
        'iScore = iScore + CInt(Me.BestSupportingActor2Score.Text)
        'iScore = iScore + CInt(Me.BestSupportingActress1Score.Text)
        'iScore = iScore + CInt(Me.BestSupportingActress2Score.Text)

        'iScore = iScore / CInt(Me.txtCountOfNominations.Text)

        'sMsg = sMsg & "<br /><b>Production:</b> " & Me.lblTitle.Text
        'sMsg = sMsg & "<br /><b>Theatre Company:</b> " & Me.lblCompanyName.Text
        'sMsg = sMsg & "<br /><b>Venue:</b> " & Me.lblVenueName.Text
        'sMsg = sMsg & "<br /><b>Production Dates:</b> " & Me.lblFirstPerformanceDateTime.Text & " thru " & Me.lblLastPerformanceDateTime.Text

        'sMsg = sMsg & "<hr noshade>"
        'sMsg = sMsg & "<br /><b>Adjudicator:</b> " & Me.lblFullname.Text
        'sMsg = sMsg & "<br /><b>Date Adjudicated:</b> " & Me.txtProductionDateAdjudicated_Planned.Text
        'sMsg = sMsg & "<br /><b>Represented Company:</b> " & Me.lblCompanyAdjudication.Text

        'sMsg = sMsg & "<br /><b>Production Date Adjudicated:</b> " & Me.txtProductionDateAdjudicated_Planned.Text
        'sMsg = sMsg & "<br /><b>Adjudicator Comment for Producing Theatre Company:</b> " & 'me.ftbAdjudicatorAttendanceComment.Text
        'sMsg = sMsg & "<br /><b>Found NHTA Advertisment:</b> " & Me.ddlFoundAdvertisement.SelectedItem.ToString

        'sMsg = sMsg & "<br /><hr noshade>"
        'sMsg = sMsg & GetText_ScoreComments("Director")
        'sMsg = sMsg & GetText_ScoreComments("MusicalDirector")
        'sMsg = sMsg & GetText_ScoreComments("Choreographer")
        'sMsg = sMsg & GetText_ScoreComments("ScenicDesigner")
        'sMsg = sMsg & GetText_ScoreComments("LightingDesigner")
        'sMsg = sMsg & GetText_ScoreComments("SoundDesigner")
        'sMsg = sMsg & GetText_ScoreComments("CostumeDesigner")
        'sMsg = sMsg & GetText_ScoreComments("OriginalPlaywright")
        'sMsg = sMsg & GetText_ScoreComments("BestPerformer1")
        'sMsg = sMsg & GetText_ScoreComments("BestPerformer2")
        'sMsg = sMsg & GetText_ScoreComments("BestSupportingActor1")
        'sMsg = sMsg & GetText_ScoreComments("BestSupportingActor2")
        'sMsg = sMsg & GetText_ScoreComments("BestSupportingActress1")
        'sMsg = sMsg & GetText_ScoreComments("BestSupportingActress2")
        'sMsg = sMsg & GetText_ScoreComments("BestProduction")
        'sMsg = sMsg & "<br /><B>Calculated Production Score:</b> <i>" & CStr(iScore) & "</i> (this calcuation is used to inform only)"

        'sMsg = sMsg & "<br /><br /><b>Last Updated By:</b> " & Master.PK_UserID.ToString & " on " & Now.ToLongDateString & " at " & Now.ToLongTimeString
        'sMsg = sMsg & "<br /><hr noshade>"

    End Sub

    Private Function GetText_ScoreComments(ByVal ControlName As String) As String
        '====================================================================================================
        Dim txt As New TextBox, lbl As New Label, txtComm As New FreeTextBoxControls.FreeTextBox
        Dim lblName As Label
        Dim sInfo As String
        '====================================================================================================
        Try
            txt = Me.pnlUserData.FindControl(ControlName & "Score")                     ' Score textbox

            If txt.Visible = True Then
                txtComm = Me.pnlUserData.FindControl(ControlName & "Comment")           ' Editable Comments

                lblName = Me.pnlUserData.FindControl("lbl" & ControlName)               ' Name Label

                sInfo = "<b>Category:</b> " & lblName.Text.Split(":")(0) & _
                    "<br /><b>Nominee:</b> " & lblName.Text.Split(":")(1) & _
                    "<br /><b>Score:</b> " & txt.Text & _
                    "<br /><hr noshade>"
            Else
                sInfo = String.Empty
            End If

            Return sInfo

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function CheckForScoringErrors() As Boolean
        '====================================================================================================
        Dim DateTester As Date
        '====================================================================================================
        Me.lblSucessfulUpdate.Visible = False
        Me.lblErrors.Visible = True
        Me.lblError2.Visible = True

        Try
            DateTester = (CDate(txtProductionDateAdjudicated_Planned.Text))
        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: Please provide a VALID <B>Attended Adjudication</B> Date value."
            Return False
        End Try

        If Me.ddlFoundAdvertisement.SelectedIndex = 0 Then
            Me.lblErrors.Text = "ERROR: Please let us know if you found the <B>NH Theatre Awards Advertisement</B> in the Playbill."
            Return False
        End If

        If Not Validate_ScoresComments("Director") Then Return False
        If Not Validate_ScoresComments("MusicalDirector") Then Return False
        If Not Validate_ScoresComments("Choreographer") Then Return False
        If Not Validate_ScoresComments("ScenicDesigner") Then Return False
        If Not Validate_ScoresComments("LightingDesigner") Then Return False
        If Not Validate_ScoresComments("SoundDesigner") Then Return False
        If Not Validate_ScoresComments("CostumeDesigner") Then Return False
        If Not Validate_ScoresComments("OriginalPlaywright") Then Return False
        If Not Validate_ScoresComments("BestPerformer1") Then Return False
        If Not Validate_ScoresComments("BestPerformer2") Then Return False
        If Not Validate_ScoresComments("BestSupportingActor1") Then Return False
        If Not Validate_ScoresComments("BestSupportingActor2") Then Return False
        If Not Validate_ScoresComments("BestSupportingActress1") Then Return False
        If Not Validate_ScoresComments("BestSupportingActress2") Then Return False
        If Not Validate_ScoresComments("BestProduction") Then Return False

        Me.lblErrors.Visible = False
        Me.lblError2.Visible = False

        Return True

    End Function

    Private Sub btnPrinterFriendly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrinterFriendly.Click
        OpenNewBrowserWindow("BallotSummary.aspx?Print=True&ScoringID=" & iScoringID.ToString)
    End Sub

    Private Sub OpenNewBrowserWindow(ByVal ReportURL As String)
        Try
            Dim sStartupScript As String
            sStartupScript = "<script type='text/javascript'>var detailedresults=(window.open('"
            sStartupScript += ReportURL
            sStartupScript += "'"
            sStartupScript += "));</script>"

            'Response.Write(sStartupScript)  'Cant use this inside an UpdatePanel
            ScriptManager.RegisterStartupScript(Me, GetType(String), "startUp", sStartupScript, False)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub btnEmailMeThisBallot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmailMeThisBallot.Click
        Call EmailCompletedBallot()
    End Sub
End Class