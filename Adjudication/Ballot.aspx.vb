Imports Adjudication.CustomMail
Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CommonFunctions
Imports System.Data

Partial Public Class Ballot
    Inherits System.Web.UI.Page
    '============================================================================================
    ' PAGE COMMENTS:  This page saves all data entered in the Scoring_Temp_Entry table first, then when
    ' all Nominated categories are  scored & commented the data in Scoring_Temp_Entry is saved to the
    ' Scoring table.
    ' NOTE: values in Scoring_Temp_Entry should never be deleted for reasons of backup and to assist
    '       in troubleshooting.
    '============================================================================================
    Public gScoringID As Integer = 0
    Public gDisplayOrder As Integer = 0
    Public gMenuNominationsJSON As String = ""
    Public gMatrixRangesJSON As String = ""
    Public gAdjudicatorCommentMinimumCharacterCount As Integer = 100
    Public gScoringRangeMin As Integer = 1
    Public gScoringRangeMax As Integer = 99

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TESTING ONLY - 'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -
        ViewState("PK_ScoringID") = 2441 'TESTING ONLY
        'TESTING ONLY - 'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -'TESTING ONLY -
        '============================================================================================
        Master.PageTitleLabel = Me.Page.Title
        Master.PageTitleVisible = False
        '============================================================================================
        ViewState("LoginID") = Master.UserLoginID
        If Not (Master.AccessLevel > 0 And Master.AccessLevel <= 5) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        Me.spanFeedbackMessage.InnerHtml = ""                                                                      'reset Error message on postback

        If Request.QueryString("ScoringID") Is Nothing And ViewState("PK_ScoringID") Is Nothing Then
            Me.spanFeedbackMessage.InnerHtml = "ERROR: Cannot find selected Production/Ballot."
            Me.divCommonQuestions.Visible = False
            Me.divCommonQuestions.Visible = False
        Else
            If Not Request.QueryString("ScoringID") Is Nothing Then
                gScoringID = CInt(Request.QueryString("ScoringID").ToString)
            ElseIf Not ViewState("PK_ScoringID") Is Nothing Then
                gScoringID = CInt(ViewState("PK_ScoringID").ToString)
            End If

            ViewState("PK_ScoringID") = gScoringID  ' OLD Request.QueryString("ScoringID")

            If Master.AccessLevel > 1 Then                                                      '=== Verify the User has access to view this Ballot ===
                If User_has_Access_to_Ballot(Master.PK_UserID, gScoringID) = False Then
                    Me.spanFeedbackMessage.InnerHtml = "ERROR: You do not have access to edit or view this Ballot!"
                    Exit Sub
                End If
            End If

            Master.MenuBallotVisible = True                                                     'setup Menus for Ballots
            Master.MenuMainVisible = False

            If IsPostBack Then
                Call IsEvent_CRUD()
                Call Initialize_Menu_NominationCategories()                                     'create Menu after SAVE has completed (assumes "DisplayOrder" is in Viewstate)
            Else
                Call Initialize_Menu_NominationCategories()                                     'create Menu
                If Not Request.QueryString("DisplayOrder") Is Nothing Then                      'if page request was from a Menu selection, "DisplayOrder" is in the querystring
                    gDisplayOrder = Request.QueryString("DisplayOrder")
                    ViewState("DisplayOrder") = Request.QueryString("DisplayOrder")
                Else
                    If ViewState("DisplayOrder") Is Nothing Then
                        ViewState("DisplayOrder") = 0                                           ' Will set to begin point of ballot 
                    End If
                End If

                Call Begin_Ballot_Entry()
                Call Populate_ApplicationDefaults()
                Call Populate_Ballot()

                If gDisplayOrder > 0 Then Call Set_Category("SELECTED")

            End If

        End If

    End Sub

    Public Class MatrixRange                                 'To be serialized and send Client side as JSON
        Public ScoringRangeMin As Integer = 1
        Public ScoringRangeMax As Integer = 99
        Public HeaderScoringRanges As String = "Scoring Range {undefined}"
        Public MatrixAdjectives As String = ""
        Public MatrixDetail As String = ""
        Public IsRangeSelected As Boolean = False

        Public Sub New(iScoringRangeMin As Integer, iScoringRangeMax As Integer, sHeaderScoringRanges As String, sMatrixAdjectives As String, sMatrixDetail As String, bIsRangeSelected As Boolean)
            ScoringRangeMin = iScoringRangeMin
            ScoringRangeMax = iScoringRangeMax
            HeaderScoringRanges = sHeaderScoringRanges.Replace("""", "''").Replace("  ", " ")
            MatrixAdjectives = sMatrixAdjectives.Replace("""", "''").Replace("  ", " ")
            MatrixDetail = sMatrixDetail.Replace("""", "''").Replace("  ", " ")
            IsRangeSelected = bIsRangeSelected
        End Sub
    End Class

    Private Sub Display_MatrixData(ByVal sSelectedScore As String, ByVal sCategoryID As String)
        '====================================================================================================
        Dim dtMatrix As DataTable, sSQL As String
        Dim MatrixRanges As New List(Of MatrixRange)
        Dim bScoreInRange As Boolean = False
        '====================================================================================================
        Try
            Dim iScore As Integer = CInt(sSelectedScore)
            Dim ScoreMin As Integer = CInt(ViewState("ScoringMinimum"))
            Dim ScoreMax As Integer = CInt(ViewState("ScoringMaximum"))

            If Not ((iScore >= ScoreMin) And (iScore <= ScoreMax)) And Not iScore = 0 Then
                Me.spanFeedbackMessage.InnerHtml = "ERROR: Selected Score is outside of allowed Max/Min scoring range. Please contact the System Administrator."
                Exit Sub
            End If

            sSQL = "SELECT  Category_ScoringRange.FK_CategoryID, ScoringRange.ScoringRangeMax, ScoringRange.ScoringRangeMin,  " & _
                            "       CAST(ScoringRange.ScoringRangeMax AS VARCHAR(5)) + ' - ' + CAST(ScoringRange.ScoringRangeMin AS VARCHAR(5)) AS HeaderScoringRanges, " & _
                            "	    Category_ScoringRange.MatrixAdjectives, Category_ScoringRange.MatrixDetail " & _
                            "	FROM Category_ScoringRange INNER JOIN ScoringRange ON Category_ScoringRange.FK_ScoringRangeID = ScoringRange.PK_ScoringRangeID " & _
                            "	WHERE (Category_ScoringRange.FK_CategoryID = " & sCategoryID & ") " & _
                            "	ORDER BY ScoringRangeMax DESC"

            dtMatrix = DataAccess.Run_SQL_Query(sSQL)

            If dtMatrix.Rows.Count > 0 Then
                For Each dr As DataRow In dtMatrix.Rows
                    If Not iScore = 0 Then
                        If (iScore >= dr.Item("ScoringRangeMin")) And (iScore <= dr.Item("ScoringRangeMax")) Then
                            bScoreInRange = True
                        Else
                            bScoreInRange = False
                        End If
                    End If
                    MatrixRanges.Add(New MatrixRange(dr.Item("ScoringRangeMin"), dr.Item("ScoringRangeMax"), dr.Item("HeaderScoringRanges"), dr.Item("MatrixAdjectives"), RemoveCRFL(RemoveHTMLTags(RemoveInvalidMSWordFormatting(dr.Item("MatrixDetail")))), bScoreInRange))
                Next
            Else
                Me.spanFeedbackMessage.InnerHtml = "WARNING: Matrix Adjectives not found - please contact the System Administrator."
            End If

            'sSQL = "SELECT  Category_ScoringRange.FK_CategoryID, ScoringRange.ScoringRangeMax, ScoringRange.ScoringRangeMin,  " & _
            '        "       CAST(ScoringRange.ScoringRangeMax AS VARCHAR(5)) + ' - ' + CAST(ScoringRange.ScoringRangeMin AS VARCHAR(5)) AS HeaderScoringRanges, " & _
            '        "	    Category_ScoringRange.MatrixAdjectives, Category_ScoringRange.MatrixDetail " & _
            '        "	FROM Category_ScoringRange INNER JOIN ScoringRange ON Category_ScoringRange.FK_ScoringRangeID = ScoringRange.PK_ScoringRangeID " & _
            '        "	WHERE (ScoringRange.ScoringRangeMax >= " & sScoreValue & ")  " & _
            '        "		AND (ScoringRange.ScoringRangeMin <= " & sScoreValue & ") " & _
            '        "		AND (Category_ScoringRange.FK_CategoryID = " & sCategoryID & ") "

            'dtMatrix = DataAccess.Run_SQL_Query(sSQL)

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            gMatrixRangesJSON = serializer.Serialize(MatrixRanges)

        Catch ex As Exception
            Me.spanFeedbackMessage.InnerHtml = "ERROR: " & ex.Message
        End Try

    End Sub

    Public Class MenuNomination                                 'To be serialized and send Client side as JSON
        Public DisplayOrder As Integer = 0
        Public Category As String = ""
        Public EntryComplete As Boolean = False
        Public IsEnabled As Boolean = True
        Public Sub New(iDisplayOrder As Integer, sCategory As String, bEntryComplete As Boolean, bIsEnabled As Boolean)
            DisplayOrder = iDisplayOrder
            Category = sCategory
            EntryComplete = bEntryComplete
            IsEnabled = bIsEnabled
        End Sub
    End Class

    Private Sub Initialize_Menu_NominationCategories()
        '====================================================================================================
        '=== Create JSON string of Nominated Categories to be used client side to create Nominations menu
        '=== jQuery will parse the JSON and add Bootstrap styled menu items, with checkboxes where completed
        '====================================================================================================
        Dim dtCat As New DataTable, dtNom As New DataTable, dtTmpScore As New DataTable, dtNominatedNames As New DataTable
        Dim sSQL As String
        Dim MenuNominations As New List(Of MenuNomination)
        Dim IsEntered As Boolean = False
        Dim IsEnteredProduction As Boolean = False, iDisplayOrderProduction As Integer = 99
        Dim EnableProductionScoreEntry As Boolean = False  'set to FALSE now, if even 1 nomination is not done, will be set TRUE
        Dim NomineeName As String = String.Empty
        '====================================================================================================
        dtNom = Get_Ballot(ViewState("PK_ScoringID").ToString, IIf(Master.AccessLevel > 1, Master.PK_UserID.ToString, ""))
        dtNominatedNames = DataAccess.Run_SQL_Query("SELECT Nominations.*, Production.* FROM Nominations INNER JOIN Production ON Nominations.FK_ProductionID = Production.PK_ProductionID WHERE PK_NominationsID = " & dtNom.Rows(0)("FK_NominationsID").ToString)
        '--------
        dtCat = Get_Categories()
        dtTmpScore = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID"))

        If dtNom.Rows.Count > 0 Then
            'Setup the page for "Adjudication Date & Playbill Ad" Menu item (also FAQ)
            If dtNom.Rows(0)("ProductionDateAdjudicated_Planned").ToString.Length > 3 Then
                IsEntered = True
            Else
                IsEntered = False
            End If

            MenuNominations.Add(New MenuNomination(0, "Attendace Date & Playbill Ad", IsEntered, True))
            MenuNominations.Add(New MenuNomination(-1, "divider", False, True))

            For Each dr As DataRow In dtCat.Rows
                If dr.Item("NominationFieldName").ToString.ToUpper = "TITLE" Then
                    'exception check because I named the 'NominationFieldName' for Best Production field wrong, long, long ago....
                    ' Save these values to add later - so to keep production field non-selectable until all Nominiations are done
                    ' this is needed because checks are done on the production score (elsewhere in code) to determine if the ballot
                    ' has been completed and/or submitted.
                    IsEnteredProduction = False

                    If (dtNom.Rows(0).Item(dr.Item("ScoreFieldName")) > 0) Then
                        IsEnteredProduction = True
                    Else
                        'if no score in the main scoring table, check if a score was entered in the TEMP table "Scoring_Temp_Entry"
                        IsEnteredProduction = IsTempScoreEntered(dtTmpScore, dr.Item("ScoreFieldName").ToString)
                    End If
                    NomineeName = dtNominatedNames.Rows(0)("Title").ToString
                    iDisplayOrderProduction = CInt(dr.Item("DisplayOrder").ToString)

                Else
                    IsEntered = False

                    If dtNom.Rows(0).Item(dr.Item("NominationFieldName")).ToString.Length > 0 Then
                        Dim FullCategoryName As String = dr.Item("CategoryName").ToString()

                        If dr.Item("RoleFieldName").ToString.Length > 1 Then
                            NomineeName = dtNominatedNames.Rows(0)(dr.Item("NominationFieldName")).ToString & " as '<i>" & dr.Item("RoleFieldName").ToString & "</i>'"
                        Else
                            NomineeName = dtNominatedNames.Rows(0)(dr.Item("NominationFieldName")).ToString
                        End If

                        If InStr(FullCategoryName, "Supporting") Then                       'ugly hacks...
                            If InStr(FullCategoryName, "Actress") Then
                                FullCategoryName = "Supporting Actress"
                            Else
                                FullCategoryName = "Supporting Actor"
                            End If
                        End If

                        If InStr(FullCategoryName, "Performer") Then                       'ugly hacks...
                            If dtNom.Rows(0).Item(dr.Item("GenderFieldName")).ToString.Length > 1 Then
                                FullCategoryName = dtNom.Rows(0).Item(dr.Item("GenderFieldName")).ToString
                            End If
                        End If

                        If (dtNom.Rows(0).Item(dr.Item("ScoreFieldName")) > 0) Then
                            IsEntered = True
                        Else
                            'if no score in the main scoring table, check if a score was entered in the TEMP table "Scoring_Temp_Entry"
                            IsEntered = IsTempScoreEntered(dtTmpScore, dr.Item("ScoreFieldName").ToString)
                        End If

                        MenuNominations.Add(New MenuNomination(CInt(dr.Item("DisplayOrder").ToString), FullCategoryName & " - " & NomineeName, IsEntered, True))

                    End If
                End If
            Next

            'After the check, add the the Production to the menu - noting if this is enabled or not
            MenuNominations.Add(New MenuNomination(-1, "divider", False, True))
            MenuNominations.Add(New MenuNomination(iDisplayOrderProduction, "Production - " & NomineeName, IsEnteredProduction, EnableProductionScoreEntry))

            Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            gMenuNominationsJSON = serializer.Serialize(MenuNominations)
        End If

    End Sub

    Private Function IsTempScoreEntered(dtTmpScore As DataTable, ScoreFieldName As String) As Boolean
        'if no score in the main scoring table, check if a score was entered in the TEMP table "Scoring_Temp_Entry"
        Dim IsEntered As Boolean = False

        Dim drTmp() As DataRow = dtTmpScore.Select(" ScoreFieldName = '" & ScoreFieldName & "' ")

        For Each drT As DataRow In drTmp
            If drT.Item("Score").ToString.Length > 0 Then
                Try
                    Dim scoreVal As Integer = CInt(drT.Item("Score").ToString)
                    If scoreVal > 0 Then
                        IsEntered = True
                    End If
                Catch ex As Exception
                    'do nothing, keep going with IsEnteredProduction = False
                End Try
            End If
        Next
        Return IsEntered

    End Function

    Private Sub IsEvent_CRUD()
        Try
            Dim iDisOrder = CInt(Request.Form("__EVENTARGUMENT"))
            ViewState("DisplayOrder") = iDisOrder
            gDisplayOrder = iDisOrder

            Select Case Request.Form("__EVENTTARGET").ToUpper
                Case "SAVE"
                    If iDisOrder = 0 Then
                        Call Save_AttendanceDateandPlaybill()
                        Call Set_Category("NEXT")
                    ElseIf iDisOrder > 0 Then
                        If ValidateSave_ScoresComments() = True Then
                            Call Set_Category("NEXT")
                        End If
                    End If
                    If Not Me.spanFeedbackMessage.InnerText.Contains("ERROR") Then
                        Me.spanFeedbackMessage.InnerHtml = ""
                    End If

                Case "SAVEANDREMAIN"
                    If iDisOrder = 0 Then
                        Call Save_AttendanceDateandPlaybill()
                        Me.divCommonQuestions.Visible = True
                        Me.divBallotDataEntry.Visible = False

                    ElseIf iDisOrder > 0 Then
                        If ValidateSave_ScoresComments() = True Then
                            Call Set_Category("SELECTED")
                            'Call Display_MatrixData(Me.span_Score.Value, ViewState("PK_CategoryID").ToString)
                        End If
                    End If
            End Select

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub Begin_Ballot_Entry()
        '====================================================================================================
        'If existing temp Scoring_Temp_Entry record, use existing, else create copy of Scoring record in
        'table Scoring_Temp_Entry
        '====================================================================================================
        Dim dt As DataTable, dtAd As New DataTable
        '====================================================================================================
        'Me.pnlErrors.Visible = False
        ViewState("PK_Scoring_Temp_EntryID_Ad") = "0"           'Hack to make the new Advertisement field work

        Try
            dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='AdjudicatorAttendanceComment'")

            If dt.Rows.Count = 0 Then
                Dim ht As New Hashtable
                'This is a new ballot = create empty records for 1st optional Comment field: AdjudicatorAttendanceComment
                ht.Add(1, 0)                                    'Scoring_Temp_EntryID, set to 0 if new record
                ht.Add(2, ViewState("PK_ScoringID"))            'ScoringID for this Ballot
                ht.Add(3, "ProductionDateAdjudicated_Planned")  'ScoreFieldName
                ht.Add(4, "AdjudicatorAttendanceComment")       'CommentFieldName
                ht.Add(5, Nothing)                              'Score
                ht.Add(6, Nothing)                              'Comment
                ht.Add(99, Master.UserLoginID)                      'LastUpdateByName

                '=== Save a blank record as the 1st record for this Ballot
                Call Save_Scoring_Temp_Entry(ht)

                '=== Get the PK_Scoring_Temp_EntryID value of new record
                dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='AdjudicatorAttendanceComment'")
                ViewState("PK_Scoring_Temp_EntryID") = dt.Rows(0)("PK_Scoring_Temp_EntryID").ToString

            Else
                '=== Matching records found, populate any previously entered data
                ViewState("PK_Scoring_Temp_EntryID") = dt.Rows(0)("PK_Scoring_Temp_EntryID").ToString
                Me.txtProductionDateAdjudicated_Planned.Text = dt.Rows(0)("Score").ToString
                Me.txtAdjudicatorAttendanceComment.Text = RemoveHTMLTags(dt.Rows(0)("Comment").ToString())
                Me.spanTitle_CategoryName.InnerText = "Frequently Asked Questions"
                Me.spanTitle_NomineeName.InnerText = ""         'No nominee name for screen with Attendance Date

                dtAd = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='FoundAdvertisement'")
                If dtAd.Rows.Count > 0 Then
                    ViewState("PK_Scoring_Temp_EntryID_Ad") = dtAd.Rows(0)("PK_Scoring_Temp_EntryID").ToString
                    Me.ddlFoundAdvertisement.SelectedValue = dtAd.Rows(0)("Score").ToString
                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub Populate_ApplicationDefaults()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_ApplicationDefaults

        If dt.Rows.Count > 0 Then
            ViewState("ScoringMinimum") = dt.Rows(0)("ScoringMinimum").ToString
            ViewState("ScoringMaximum") = dt.Rows(0)("ScoringMaximum").ToString
            Me.lblDaysToWaitForScoring.Text = dt.Rows(0)("DaysToWaitForScoring").ToString
            Me.lblMaxShowsPerAdjudicator.Text = dt.Rows(0)("MaxShowsPerAdjudicator").ToString
            Me.lblDaysToEditBallot.Text = "NOTE: Adjudicators have <b>" & dt.Rows(0)("DaysToWaitForScoring").ToString & "</b> days to edit this Ballot after submission."
            gAdjudicatorCommentMinimumCharacterCount = dt.Rows(0)("AdjudicatorCommentMinimumCharacterCount")

            Me.txtScore.ToolTip = "Valid Scoring Ranges are from " & dt.Rows(0)("ScoringMinimum").ToString & " to " & dt.Rows(0)("ScoringMaximum").ToString
        End If
    End Sub

    Private Sub Populate_ProductionDetails(ByVal ProductionID As String)
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        ' Get Production Information
        dt = Get_Production(ProductionID)

        If dt.Rows.Count > 0 Then
            Me.spnProductionName.InnerHtml = "<b>" & dt.Rows(0)("Title").ToString & "</b> - " & dt.Rows(0)("ProductionType").ToString & ""
            Me.spnProducingCompanyName.InnerText = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("ProductionCategory").ToString
            Me.lblFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.lblLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            Me.spnProductionCategory.InnerText = dt.Rows(0)("ProductionCategory").ToString()
            Me.spnProductionType.InnerText = dt.Rows(0)("ProductionType").ToString()

            Master.PageTitleLabel = "Ballot for " & dt.Rows(0)("Title").ToString
        End If

        '====================================================================================================
        ' Get User and Scoring Information
        sSQL = "SELECT Users.LastName + ', ' + Users.FirstName as FullName, Company.CompanyName, " & _
                "       Users.EmailPrimary, Users.EmailSecondary, " & _
                "       Scoring.ReserveAdjudicator " & _
                " FROM Users INNER JOIN " & _
                "       Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN " & _
                "       Company ON Users.FK_CompanyID = Company.PK_CompanyID " & _
                " WHERE Scoring.PK_ScoringID = " & ViewState("PK_ScoringID")

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblFullname.Text = dt.Rows(0)("FullName").ToString
            'Me.lblAdjudicatorCompany.Text = dt.Rows(0)("CompanyName").ToString
            'Me.lblCompanyAdjudication.Text = dt.Rows(0)("CompanyAdjudication").ToString
            ViewState("EmailPrimary") = dt.Rows(0)("EmailPrimary").ToString
            ViewState("EmailSecondary") = dt.Rows(0)("EmailSecondary").ToString
            ViewState("ReserveAdjudicator") = dt.Rows(0)("ReserveAdjudicator").ToString
        End If

    End Sub

    Private Sub Populate_Ballot()
        '====================================================================================================
        Dim dtBallot As DataTable, dtDefaults As DataTable
        '====================================================================================================
        dtDefaults = Get_ApplicationDefaults()                                      'to get the value for: DaysToWaitForScoring

        ' If not an admin, double check that user is assigned this adjudication by also listing the UserID
        dtBallot = Get_Ballot(ViewState("PK_ScoringID").ToString, IIf(Master.AccessLevel > 1, Master.PK_UserID.ToString, ""))

        ' If Ballot has been submitted
        If dtBallot.Rows.Count > 0 Then                                             ' If not an Administrator, check if Ballot can be edited by Adjudicator
            If Master.AccessLevel > 1 Then                                          ' Check if already submitted scores can be EDITed by Adjudicator
                If dtBallot.Rows(0)("BallotSubmitDate").ToString.Length > 6 Then    ' Check if setting for 'DaysToWaitForScoring' allows for Adjudicator edits or not
                    If Today > CDate(dtBallot.Rows(0)("BallotSubmitDate").ToString).AddDays(CDbl(dtDefaults.Rows(0)("DaysToWaitForScoring").ToString)) Then
                        If Not dtBallot.Rows(0)("BestProductionScore").ToString = "0" Then
                            ViewState("EnableControls") = 0                         ' the Ballot has already been submitted
                        Else
                            ViewState("EnableControls") = 1                         ' allow the Ballot to be updated
                        End If
                    End If
                End If
            End If

            ViewState("PK_ScoringID") = dtBallot.Rows(0)("PK_ScoringID").ToString
            ViewState("FK_NominationsID") = dtBallot.Rows(0)("FK_NominationsID").ToString
            ViewState("FK_CompanyID_Adjudicator") = dtBallot.Rows(0)("FK_CompanyID_Adjudicator").ToString
            ViewState("FK_UserID_Adjudicator") = dtBallot.Rows(0)("FK_UserID_Adjudicator").ToString
            ViewState("FK_ProductionID") = dtBallot.Rows(0)("FK_ProductionID").ToString

            'DEPRECATED ViewState("ReserveAdjudicator") = dtBallot.Rows(0)("ReserveAdjudicator").ToString

            If dtBallot.Rows(0)("BestProductionScore") > 0 Then                     'Check if the Ballot was COMPLETED; if yes then use submitted values
                ViewState("ProductionDateAdjudicated_Actual") = dtBallot.Rows(0)("ProductionDateAdjudicated_Actual").ToString
                ViewState("LastUpdateByName") = dtBallot.Rows(0)("LastUpdateByName").ToString
                ViewState("LastUpdateByDate") = dtBallot.Rows(0)("LastUpdateByDate").ToString

                If dtBallot.Rows(0)("FoundAdvertisement").ToString.Length > 0 Then
                    ViewState("FoundAdvertisement") = dtBallot.Rows(0)("FoundAdvertisement").ToString
                    Me.ddlFoundAdvertisement.SelectedValue = dtBallot.Rows(0)("FoundAdvertisement").ToString
                Else
                    ViewState("FoundAdvertisement") = ""
                End If

                If dtBallot.Rows(0)("ProductionDateAdjudicated_Planned").ToString.Length > 3 Then
                    Me.txtProductionDateAdjudicated_Planned.Text = CDate(dtBallot.Rows(0)("ProductionDateAdjudicated_Planned").ToString).ToShortDateString
                End If
                If dtBallot.Rows(0)("ProductionDateAdjudicated_Actual").ToString.Length > 3 Then
                    ViewState("ProductionDateAdjudicated_Actual") = CDate(dtBallot.Rows(0)("ProductionDateAdjudicated_Actual").ToString).ToShortDateString
                End If
            End If

            '=== Populate_ProductionDetails using the ProductionID ===
            Call Populate_ProductionDetails(dtBallot.Rows(0)("FK_ProductionID").ToString)

        Else
            Me.spanFeedbackMessage.InnerHtml = "ERROR: You are not Assigned this Adjudication."
        End If

    End Sub

    Private Sub Set_Category(Optional ByVal Direction As String = "NEXT")
        '====================================================================================================
        Dim dtCatCriteria As DataTable, sSQL As String, dtScore As DataTable, dtNom As DataTable
        Dim FullCategoryName As String = "", iScore As Integer = 0
        '====================================================================================================
        Try
            Select Case Direction.ToUpper
                Case "NEXT"
                    gDisplayOrder = (CInt(ViewState("DisplayOrder")) + 1).ToString
                    ViewState("DisplayOrder") = gDisplayOrder

                Case "PREVIOUS"
                    If CInt(ViewState("DisplayOrder")) > 1 Then
                        gDisplayOrder = (CInt(ViewState("DisplayOrder")) - 1).ToString
                        ViewState("DisplayOrder") = gDisplayOrder
                    Else
                        ViewState("DisplayOrder") = 0
                        gDisplayOrder = 0
                    End If

                Case Else
                    'Case "SAVE" Or "SELECTED" & do not advance
            End Select

            'Show/Hide the appropriate Category panels 
            If gDisplayOrder > 0 Then
                Me.divCommonQuestions.Visible = False                               'About Ballots/Date Adjudicated/NHTA Ad/Optional Comments
                Me.divBallotDataEntry.Visible = True                                   'Category & Score w/Scoring details
            ElseIf gDisplayOrder = 0 Then
                Me.spanTitle_CategoryName.InnerText = "Frequently Asked Questions"
                Me.divCommonQuestions.Visible = True                                'About Ballots/Date Adjudicated/NHTA Ad/Optional Comments
                Me.divBallotDataEntry.Visible = False                                   'Category & Score w/Scoring details
                Exit Sub
            Else
                gDisplayOrder = 0
                Me.spanTitle_CategoryName.InnerText = "Frequently Asked Questions"
                Me.divCommonQuestions.Visible = True                                'About Ballots/Date Adjudicated/NHTA Ad/Optional Comments
                Me.divBallotDataEntry.Visible = False                                   'Category & Score w/Scoring details
                Exit Sub
            End If

            '===============================================================================================================================================================================
            '=== STEP 1: Get current Category Criteria that is being reviewed based on "DisplayOrder"
            '===============================================================================================================================================================================
            sSQL = "SELECT TOP 1 PK_CategoryID, CategoryName, ScoringCriteria, ScoreFieldName, CommentFieldName, NominationFieldName, RoleFieldName, GenderFieldName, DisplayOrder, ActiveCategory, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate " & _
                    "   FROM Category " & _
                    "   WHERE (DisplayOrder >= " & gDisplayOrder & ") AND (ActiveCategory = 1) " & _
                    "   ORDER BY DisplayOrder "

            dtCatCriteria = DataAccess.Run_SQL_Query(sSQL)

            If dtCatCriteria.Rows.Count > 0 Then
                '===============================================================================================================================================================================
                '=== STEP 2: Get Nominee Name and Role/Gender (where needed)
                '===============================================================================================================================================================================
                sSQL = ""       'reset string
                If dtCatCriteria.Rows(0)("NominationFieldName").ToString.Length > 1 Then sSQL = sSQL & dtCatCriteria.Rows(0)("NominationFieldName").ToString & ", "
                If dtCatCriteria.Rows(0)("RoleFieldName").ToString.Length > 1 Then sSQL = sSQL & dtCatCriteria.Rows(0)("RoleFieldName").ToString & ", "
                If dtCatCriteria.Rows(0)("GenderFieldName").ToString.Length > 1 Then sSQL = sSQL & dtCatCriteria.Rows(0)("GenderFieldName").ToString & ", "

                sSQL = "SELECT " & sSQL & " PK_NominationsID FROM Nominations INNER JOIN Production ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                        "   WHERE PK_NominationsID = " & ViewState("FK_NominationsID")
                dtNom = DataAccess.Run_SQL_Query(sSQL)

                '===============================================================================================================================================================================
                '=== STEP 3: Show/hide elements depending on the Nominated Category ===
                '===============================================================================================================================================================================
                If dtNom.Rows(0)(dtCatCriteria.Rows(0)("NominationFieldName").ToString).ToString.Length > 1 Then
                    If dtCatCriteria.Rows(0)("NominationFieldName").ToString.ToUpper = "TITLE" Then
                        Me.txtScore_Calculated_For_Production.Text = Get_BestProduction_Calculated_Temp_Score.ToString("#0.0")
                        Me.txtScore_Calculated_For_Production.Visible = True
                    Else
                        Me.txtScore_Calculated_For_Production.Visible = False
                    End If

                    '=== Assign value incase next DisplayOrder value is greater than the given txtDisplayOrder value ===
                    ViewState("NominationFieldName") = dtCatCriteria.Rows(0)("NominationFieldName").ToString
                    ViewState("ScoreFieldName") = dtCatCriteria.Rows(0)("ScoreFieldName").ToString
                    ViewState("CommentFieldName") = dtCatCriteria.Rows(0)("CommentFieldName").ToString
                    ViewState("DisplayOrder") = dtCatCriteria.Rows(0)("DisplayOrder").ToString
                    ViewState("PK_CategoryID") = dtCatCriteria.Rows(0)("PK_CategoryID").ToString

                    '=== Display acting Role information when needed ===
                    If dtCatCriteria.Rows(0)("RoleFieldName").ToString.Length > 1 Then
                        Me.spanTitle_NomineeName.InnerHtml = dtNom.Rows(0)(dtCatCriteria.Rows(0)("NominationFieldName").ToString).ToString & " as '<i>" & dtNom.Rows(0)(dtCatCriteria.Rows(0)("RoleFieldName").ToString).ToString & "</i>'"
                        Me.divNominee.InnerHtml = dtNom.Rows(0)(dtCatCriteria.Rows(0)("NominationFieldName").ToString).ToString & " as '<i>" & dtNom.Rows(0)(dtCatCriteria.Rows(0)("RoleFieldName").ToString).ToString & "</i>'"
                    Else
                        Me.spanTitle_NomineeName.InnerHtml = dtNom.Rows(0)(dtCatCriteria.Rows(0)("NominationFieldName").ToString).ToString
                        Me.divNominee.InnerHtml = dtNom.Rows(0)(dtCatCriteria.Rows(0)("NominationFieldName").ToString).ToString
                    End If

                    '=== Display Category name with Correct Gender when needed ===
                    If dtCatCriteria.Rows(0)("GenderFieldName").ToString.Length > 1 Then
                        FullCategoryName = "Best " & dtNom.Rows(0)(dtCatCriteria.Rows(0)("GenderFieldName").ToString).ToString
                    Else
                        FullCategoryName = dtCatCriteria.Rows(0)("CategoryName").ToString
                    End If

                    Me.spanTitle_CategoryName.InnerText = FullCategoryName
                    Me.lblCatetoryName.InnerHtml = FullCategoryName
                    Me.lblScoringCriteria.Text = dtCatCriteria.Rows(0)("ScoringCriteria").ToString

                    '===============================================================================================================================================================================
                    '=== STEP 4: Get previously Entered Scores and Comments: 
                    '===         assume values in table Scoring_Temp_Entry are most accurate/recent (if found)
                    '===============================================================================================================================================================================
                    sSQL = "SELECT * FROM Scoring_Temp_Entry " & _
                            " WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='" & dtCatCriteria.Rows(0)("ScoreFieldName").ToString & "'" ' dtCatCriteria.Rows(0)("CommentFieldName").ToString

                    dtScore = DataAccess.Run_SQL_Query(sSQL)

                    '=== Use data from table Scoring_Temp_Entry if found, if not found check Scoring table for any entry.
                    If dtScore.Rows.Count > 0 Then
                        ViewState("PK_Scoring_Temp_EntryID") = dtScore.Rows(0)("PK_Scoring_Temp_EntryID").ToString
                        iScore = dtScore.Rows(0)("Score")
                        Me.span_Score.Value = iScore.ToString
                        Me.span_Comment.InnerHtml = RemoveHTMLTags(dtScore.Rows(0)("Comment").ToString)
                    Else
                        '++++ This is no longer needed as ALL ballots are now using new Ballot form++++ 
                        '=== Needed for backwards compatibilty to edit Ballots submitted via old Scoring.aspx page  
                        'ViewState("PK_Scoring_Temp_EntryID") = "0"      ' Set to ZERO to indicate this is a new record in 'Scoring_Temp_Entry'
                        'dtScore.Clear()
                        'sSQL = "SELECT PK_ScoringID, FK_CompanyID_Adjudicator, FK_UserID_Adjudicator, " & dtCatCriteria.Rows(0)("ScoreFieldName").ToString & ", " & dtCatCriteria.Rows(0)("CommentFieldName").ToString & _
                        '        "   FROM Scoring WHERE PK_ScoringID=" & ViewState("PK_ScoringID")

                        'dtScore = DataAccess.Run_SQL_Query(sSQL)

                        'If dtScore.Rows.Count > 0 Then
                        '    Me.span_Score.Value = dtScore.Rows(0)(dtCatCriteria.Rows(0)("ScoreFieldName").ToString).ToString
                        '    Me.span_Comment.InnerHTML = RemoveHTMLTags(dtScore.Rows(0)(dtCatCriteria.Rows(0)("CommentFieldName").ToString).ToString)
                        'Else
                        ''End If
                        Me.span_Score.Value = String.Empty
                        Me.span_Comment.InnerHtml = String.Empty
                    End If

                    '===============================================================================================================================================================================
                    '=== STEP 5: If a Score was supplied, display corresponding matrix value - or - hide matrix controls
                    '===============================================================================================================================================================================
                    Call Display_MatrixData(iScore.ToString, ViewState("PK_CategoryID").ToString)

                Else
                    '=== If no Nominations found for current category, goto next category ===
                    Call Set_Category(Direction)
                End If
            Else
                '=== No more categories - Save the Ballot & Email to the user ===
                Me.divBallotDataEntry.Visible = False

                Me.pnlScoringSave.Visible = True
                Me.lblBallotToSubmit.Text = Display_Completed_Ballot()
            End If

        Catch ex As Exception
            Me.spanFeedbackMessage.InnerHtml = "ERROR: " & ex.Message
        End Try

    End Sub

    Private Function ValidateSave_ScoresComments(Optional ByVal ByPassValidation As Boolean = False) As Boolean
        '====================================================================================================
        Dim ReturnValue As Boolean = False
        Dim ScoreMin As Int16, ScoreMax As Int16
        '====================================================================================================
        Me.spanFeedbackMessage.InnerHtml = ""

        If ByPassValidation = False Then
            Try
                ScoreMin = CInt(ViewState("ScoringMinimum"))
                ScoreMax = CInt(ViewState("ScoringMaximum"))

                If CInt(Me.span_Score.Value) < ScoreMin Or CInt(Me.span_Score.Value) > ScoreMax Then
                    Me.spanFeedbackMessage.InnerHtml = "ERROR: For <b>" & Me.lblCategoryName.Text & "</b>, please provide a valid 'Score' between the ranges of <b>" & ViewState("ScoringMinimum") & "</b> and <b>" & ViewState("ScoringMaximum") & "</b>"
                Else
                    If Me.span_Comment.InnerHtml.Length <= gAdjudicatorCommentMinimumCharacterCount Then   ' Check MINIMUM Length of Comments
                        Me.spanFeedbackMessage.InnerHtml = "ERROR: You must submit <b>Comments</b> for the <b>" & Me.lblCategoryName.Text & "</b> . "
                    Else
                        If Me.span_Comment.InnerHtml.Length >= 8000 Then                     ' Check MAXIMUM Length of Comments
                            Me.spanFeedbackMessage.InnerHtml = "ERROR: Your comments for <b>" & Me.lblCategoryName.Text & "</b> exceed 8000 Characters. Please keep your comments a bit shorter."
                        End If
                    End If
                End If

            Catch ex As Exception
                Me.spanFeedbackMessage.InnerHtml = "ERROR: Please provide a VALID <b>" & Me.lblCategoryName.Text & "</b> score value " & " between the ranges of <b>" & ViewState("ScoringMinimum") & "</b> and <b>" & ViewState("ScoringMaximum") & "</b>"
            End Try
        End If

        '=== if no entry errors, Save in holding table: Scoring_Temp_Entry
        'If Me.pnlErrors.Visible = False Then
        ReturnValue = Save_Temp_BallotNomination()

        Return ReturnValue

    End Function

    Private Function Save_Temp_BallotNomination() As Boolean
        '====================================================================================================
        Dim ht As New Hashtable
        '====================================================================================================
        Try
            ht.Add(1, ViewState("PK_Scoring_Temp_EntryID"))         '0 if new record
            ht.Add(2, ViewState("PK_ScoringID"))                    'ScoringID for this Ballot
            ht.Add(3, ViewState("ScoreFieldName"))                  'ScoreFieldName
            ht.Add(4, ViewState("CommentFieldName"))                'CommentFieldName
            ht.Add(5, RemoveInvalidSQLCharacters(Me.span_Score.Value)) 'Score
            ht.Add(6, Me.span_Comment.InnerHtml)
            ht.Add(99, Master.UserLoginID)                     'LastUpdateByName

            Call Save_Scoring_Temp_Entry(ht)

            If ViewState("PK_Scoring_Temp_EntryID").ToString = "0" Then
                '=== Get the PK_Scoring_Temp_EntryID value of new record
                Dim dt As DataTable = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='" & ViewState("ScoreFieldName") & "'")
                ViewState("PK_Scoring_Temp_EntryID") = dt.Rows(0)("PK_Scoring_Temp_EntryID").ToString
            End If

            Me.spanFeedbackMessage.InnerHtml = "<b>SUCCESS</b> You have saved your comments and score for category."

            Return True

        Catch ex As Exception
            Me.spanFeedbackMessage.InnerHtml = "ERROR Saving this Score/Comment: " & ex.Message
            Throw ex
            Return False
        End Try

    End Function

    Private Function Get_BestProduction_Calculated_Temp_Score() As Decimal
        '====================================================================================================
        Dim dt As DataTable, sSQL As String, iScoreCount As Int16 = 0, iCategoryCount As Int16 = 0
        '====================================================================================================
        sSQL = "SELECT * FROM Scoring_Temp_Entry  " & _
                "   WHERE  ScoreFieldName <> 'BestProductionScore' " & _
                "       AND ScoreFieldName <> 'FoundAdvertisement' " & _
                "       AND ScoreFieldName <> 'ProductionDateAdjudicated_Planned' " & _
                "       AND FK_ScoringID=" & ViewState("PK_ScoringID")

        dt = DataAccess.Run_SQL_Query(sSQL)

        '=== Use data from table Scoring_Temp_Entry if found, if not found check Scoring table for any entry.
        If dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                Try
                    iScoreCount = iScoreCount + CInt(row("Score").ToString)
                    iCategoryCount = iCategoryCount + 1
                Catch ex As Exception
                    'do nothing: assume non numeric data
                End Try
            Next

            Return iScoreCount / iCategoryCount

        Else
            Return 0
        End If

    End Function

    Private Function Display_Completed_Ballot() As String
        '====================================================================================================
        'Dim dt As DataTable, sSQL As String, dtScore As DataTable, dtNom As DataTable
        'Dim FullCategoryName As String
        'Dim NominationFieldName As String, ScoreFieldName As String, CommentFieldName As String
        'Dim DisplayOrder As Integer, MaxDisplayOrder As Integer
        'Dim PK_Scoring_Temp_EntryID As String, PK_CategoryID As String
        Dim sBallot As String = ""
        '====================================================================================================
        Try
            sBallot = Get_Ballot_as_HTML(CInt(ViewState("PK_ScoringID").ToString), CInt(ViewState("FK_NominationsID").ToString), True, Me.txtScore_Calculated_For_Production.Text)
            sBallot = sBallot & "<br><br>Ballot Submission performed by <B>" & ViewState("LoginID") & "</b> on " & Now.ToLongDateString & " at " & Now.ToShortTimeString
            Return sBallot

        Catch ex As Exception
            Throw ex
        End Try

        'sBallot = "<font size=3><B>NH Theatre Awards Ballot</b></span><br />"
        'sBallot = sBallot & "<br />Production: <B>" & 'Me.lblTitlePerformance.Text & "</b>"
        'sBallot = sBallot & "<br />Producing Company: <B>" & Me.spnProducingCompanyName.Innertext & "</b>"
        'sBallot = sBallot & "<br />Production Open Date: <B>" & Me.lblFirstPerformanceDateTime.Text & "</b>"
        'sBallot = sBallot & "<br />Production Close Date: <B>" & Me.lblLastPerformanceDateTime.Text & "</b>"
        'sBallot = sBallot & "<br />"
        'sBallot = sBallot & "<br />Assigned Adjudicator: <B>" & Me.lblFullname.Text & "</b>"
        'sBallot = sBallot & "<br />Representing Company: <B>" & Me.lblCompanyAdjudication.Text & "</b>"
        'sBallot = sBallot & "<hr noshade>"
        ''ViewState("EmailPrimary")         'ViewState("EmailSecondary")        'ViewState("ReserveAdjudicator")

        ''=== Get Date Adjudicated and Any comments to producing theater company
        'dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='AdjudicatorAttendanceComment'")
        'sBallot = sBallot & "<br /><b>Production Date Adjudicated:</b> " & dt.Rows(0)("Score").ToString
        'sBallot = sBallot & "<br /><b>Adjudicator Comment for Producing Theatre Company:</b> " & dt.Rows(0)("Comment").ToString
        'sBallot = sBallot & "<hr noshade>"

        'dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='FoundAdvertisement'")
        'Select Case dt.Rows(0)("FoundAdvertisement").ToString
        '    Case "1"
        '        sBallot = sBallot & "<br /><b>Found NHTA Advertisment:</b> Yes"
        '    Case "2"
        '        sBallot = sBallot & "<br /><b>Found NHTA Advertisment:</b> No"
        '    Case "3"
        '        sBallot = sBallot & "<br /><b>Found NHTA Advertisment:</b> Forgot to Look For it"
        '    Case Else
        '        sBallot = sBallot & "<br /><b>Found NHTA Advertisment:</b> ERROR Processing Response"
        'End Select
        'sBallot = sBallot & "<hr noshade>"

        'dt.Clear()
        'sSQL = "SELECT MAX(DisplayOrder) as DisplayOrder FROM Category "
        'dt = DataAccess.Run_SQL_Query(sSQL)
        'MaxDisplayOrder = CInt(dt.Rows(0)("DisplayOrder").ToString)

        'Do While DisplayOrder <= MaxDisplayOrder
        '    '===============================================================================================================================================================================
        '    '=== STEP 1: Get 'current' Category that is being reviewed.
        '    '===============================================================================================================================================================================
        '    sSQL = "SELECT TOP 1 PK_CategoryID, CategoryName, ScoringCriteria, ScoreFieldName, CommentFieldName, NominationFieldName, RoleFieldName, " & _
        '            "       GenderFieldName, DisplayOrder, ActiveCategory, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate " & _
        '            "   FROM Category " & _
        '            "   WHERE (DisplayOrder >= " & DisplayOrder.ToString & ") AND (ActiveCategory = 1) " & _
        '            "   ORDER BY DisplayOrder "
        '    dt.Clear()
        '    dt = DataAccess.Run_SQL_Query(sSQL)

        '    If dt.Rows.Count > 0 Then
        '        '===============================================================================================================================================================================
        '        '=== STEP 2: Get Nominee Name and Role/Gender (where needed)
        '        '===============================================================================================================================================================================
        '        sSQL = ""       'reset string
        '        If dt.Rows(0)("NominationFieldName").ToString.Length > 1 Then sSQL = sSQL & dt.Rows(0)("NominationFieldName").ToString & ", "
        '        If dt.Rows(0)("RoleFieldName").ToString.Length > 1 Then sSQL = sSQL & dt.Rows(0)("RoleFieldName").ToString & ", "
        '        If dt.Rows(0)("GenderFieldName").ToString.Length > 1 Then sSQL = sSQL & dt.Rows(0)("GenderFieldName").ToString & ", "

        '        sSQL = "SELECT " & sSQL & " PK_NominationsID FROM Nominations INNER JOIN Production ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
        '                "   WHERE PK_NominationsID = " & ViewState("FK_NominationsID")

        '        dtNom = DataAccess.Run_SQL_Query(sSQL)

        '        '===============================================================================================================================================================================
        '        '=== STEP 3: Check if this Category was Nominated ===
        '        '===============================================================================================================================================================================
        '        If dtNom.Rows(0)(dt.Rows(0)("NominationFieldName").ToString).ToString.Length > 1 Then
        '            '=== Assign value incase next DisplayOrder value is greater than the given txtDisplayOrder value ===
        '            NominationFieldName = dt.Rows(0)("NominationFieldName").ToString
        '            ScoreFieldName = dt.Rows(0)("ScoreFieldName").ToString
        '            CommentFieldName = dt.Rows(0)("CommentFieldName").ToString
        '            DisplayOrder = CInt(dt.Rows(0)("DisplayOrder").ToString)
        '            PK_CategoryID = dt.Rows(0)("PK_CategoryID").ToString

        '            '=== Display Category name with Correct Gender when needed ===
        '            If dt.Rows(0)("GenderFieldName").ToString.Length > 1 Then
        '                FullCategoryName = "<B>Best " & dtNom.Rows(0)(dt.Rows(0)("GenderFieldName").ToString).ToString & "</B>"
        '            Else
        '                FullCategoryName = "<B>" & dt.Rows(0)("CategoryName").ToString & "</B>"
        '            End If

        '            sBallot = sBallot & "<br />" & FullCategoryName & ":"

        '            '=== Display acting Role information when needed ===
        '            If dt.Rows(0)("RoleFieldName").ToString.Length > 1 Then
        '                sBallot = sBallot & " " & dtNom.Rows(0)(dt.Rows(0)("NominationFieldName").ToString).ToString & " as '<i>" & dtNom.Rows(0)(dt.Rows(0)("RoleFieldName").ToString).ToString & "</i>'"
        '            Else
        '                sBallot = sBallot & " " & dtNom.Rows(0)(dt.Rows(0)("NominationFieldName").ToString).ToString
        '            End If
        '            '===============================================================================================================================================================================
        '            '=== STEP 4: Get previously Entered Scores and Comments: assume values in table Scoring_Temp_Entry are most accurate/recent (if found)
        '            '===============================================================================================================================================================================
        '            sSQL = "SELECT * FROM Scoring_Temp_Entry " & _
        '                    " WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & _
        '                    " AND ScoreFieldName='" & dtrows.item("ScoreFieldName").ToString & "'" ' dt.Rows(0)("CommentFieldName").ToString

        '            dtScore = DataAccess.Run_SQL_Query(sSQL)

        '            '=== Use data from table Scoring_Temp_Entry if found, if not found check Scoring table for any entry.
        '            If dtScore.Rows.Count > 0 Then
        '                PK_Scoring_Temp_EntryID = dtScore.Rows(0)("PK_Scoring_Temp_EntryID").ToString
        '                sBallot = sBallot & "<br /><b>Score: </b><font color=""blue"">" & dtScore.Rows(0)("Score").ToString & "</span> "
        '                sBallot = sBallot & "<br /><b>Comment:</b> " & dtScore.Rows(0)("Comment").ToString
        '                sBallot = sBallot & "<hr noshade>"
        '            Else
        '                sBallot = sBallot & "<br />ERROR - Score/Comment not found for this Category"
        '            End If

        '        End If
        '    End If
        '    DisplayOrder = DisplayOrder + 1
        'Loop

        'sBallot = sBallot & "<br /><b>Calculated 'Best Production' Score: <font color=""green"">" & Get_BestProduction_Calculated_Temp_Score.ToString("#0.0") & "</b></span>  (this calcuation is used for informational purposes only)"
        'sBallot = sBallot & "<hr noshade>"

    End Function

    Private Function Save_Completed_Ballot() As Boolean
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(50) As String
        Dim dt As DataTable
        '====================================================================================================
        sDataValues(1) = ViewState("PK_ScoringID")
        sDataValues(2) = ViewState("FK_NominationsID")
        sDataValues(3) = ViewState("FK_CompanyID_Adjudicator")
        sDataValues(4) = ViewState("FK_UserID_Adjudicator")
        sDataValues(5) = "0"        ' AdjudicatorRequestsReassignment: Upon Sumbission, reset any such requests
        sDataValues(6) = "0"        ' AdjudicatorScoringLocked
        sDataValues(7) = IIf(Me.txtProductionDateAdjudicated_Planned.Text = "", "", Me.txtProductionDateAdjudicated_Planned.Text)
        sDataValues(8) = ViewState("ProductionDateAdjudicated_Actual")

        '=== DirectorScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='DirectorScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(9) = dt.Rows(0)("Score").ToString
            sDataValues(10) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(9) = "0"
            sDataValues(10) = ""
        End If

        dt.Clear()  '=== LightingDesignerScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='MusicalDirectorScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(11) = dt.Rows(0)("Score").ToString
            sDataValues(12) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(11) = "0"
            sDataValues(12) = ""
        End If

        dt.Clear()  '=== LightingDesignerScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='ChoreographerScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(13) = dt.Rows(0)("Score").ToString
            sDataValues(14) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(13) = "0"
            sDataValues(14) = ""
        End If

        dt.Clear()  '=== LightingDesignerScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='ScenicDesignerScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(15) = dt.Rows(0)("Score").ToString
            sDataValues(16) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(15) = "0"
            sDataValues(16) = ""
        End If

        dt.Clear()  '=== LightingDesignerScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='LightingDesignerScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(17) = dt.Rows(0)("Score").ToString
            sDataValues(18) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(17) = "0"
            sDataValues(18) = ""
        End If

        dt.Clear()  '=== SoundDesignerScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='SoundDesignerScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(19) = dt.Rows(0)("Score").ToString
            sDataValues(20) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(19) = "0"
            sDataValues(20) = ""
        End If

        dt.Clear()  '=== CostumeDesignerScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='CostumeDesignerScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(21) = dt.Rows(0)("Score").ToString
            sDataValues(22) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(21) = "0"
            sDataValues(22) = ""
        End If

        dt.Clear()  '=== OriginalPlaywrightScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='OriginalPlaywrightScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(23) = dt.Rows(0)("Score").ToString
            sDataValues(24) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(23) = "0"
            sDataValues(24) = ""
        End If

        dt.Clear()  '=== BestPerformer1Score
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestPerformer1Score'")
        If dt.Rows.Count > 0 Then
            sDataValues(25) = dt.Rows(0)("Score").ToString
            sDataValues(26) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(25) = "0"
            sDataValues(26) = ""
        End If

        dt.Clear()  '=== BestPerformer2Score
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestPerformer2Score'")
        If dt.Rows.Count > 0 Then
            sDataValues(27) = dt.Rows(0)("Score").ToString
            sDataValues(28) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(27) = "0"
            sDataValues(28) = ""
        End If

        dt.Clear()  '=== BestSupportingActor1Score
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestSupportingActor1Score'")
        If dt.Rows.Count > 0 Then
            sDataValues(29) = dt.Rows(0)("Score").ToString
            sDataValues(30) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(29) = "0"
            sDataValues(30) = ""
        End If

        dt.Clear()  '=== BestSupportingActor2Score
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestSupportingActor2Score'")
        If dt.Rows.Count > 0 Then
            sDataValues(31) = dt.Rows(0)("Score").ToString
            sDataValues(32) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(31) = "0"
            sDataValues(32) = ""
        End If

        dt.Clear()  '=== BestSupportingActress1Score
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestSupportingActress1Score'")
        If dt.Rows.Count > 0 Then
            sDataValues(33) = dt.Rows(0)("Score").ToString
            sDataValues(34) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(33) = "0"
            sDataValues(34) = ""
        End If

        dt.Clear()  '=== BestSupportingActress2Score
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestSupportingActress2Score'")
        If dt.Rows.Count > 0 Then
            sDataValues(35) = dt.Rows(0)("Score").ToString
            sDataValues(36) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(35) = "0"
            sDataValues(36) = ""
        End If

        dt.Clear()  '=== BestProductionScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='BestProductionScore'")
        If dt.Rows.Count > 0 Then
            sDataValues(37) = dt.Rows(0)("Score").ToString
            sDataValues(38) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(37) = "0"
            sDataValues(38) = ""
        End If

        sDataValues(39) = ViewState("LoginID")
        sDataValues(40) = Now.ToString
        sDataValues(41) = ""        ' AdjudicatorRequestsReassignmentNote: Upon Sumbission, reset any such requests
        sDataValues(42) = ViewState("ReserveAdjudicator")
        sDataValues(43) = 5         ' Status=Ballot Submitted

        dt.Clear()  '=== Adjudicator Comments to Production Company
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='AdjudicatorAttendanceComment'")
        If dt.Rows.Count > 0 Then
            sDataValues(44) = dt.Rows(0)("Comment").ToString
        Else
            sDataValues(44) = ""
        End If

        sDataValues(45) = 0         'False = 0 = @NonScoringUpdate  (this is not a Non-Scoring update)

        dt.Clear()  '=== BestProductionScore
        dt = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='FoundAdvertisement'")
        If dt.Rows.Count > 0 Then
            sDataValues(46) = dt.Rows(0)("Score").ToString
        Else
            sDataValues(46) = "0"
        End If

        Me.spanFeedbackMessage.InnerHtml = "<b>SUCCESS</b> You have saved the Ballot."

        If Save_Scoring(sDataValues) = True Then
            If Request.QueryString("Admin") = "True" Then
                Call Email_Completed_Ballot()
                Response.Redirect("AdminScoring.aspx")
            Else
                Call Email_Completed_Ballot()
                Me.divBallotDataEntry.Visible = False
                Me.pnlScoringSave.Visible = False
                Me.pnlResults.Visible = True
                'Me.lbtnCancel.Visible = False
            End If
        Else
            Me.spanFeedbackMessage.InnerHtml = "ERROR: Saving Scoring Data"
        End If

        Return True

    End Function

    Private Sub Email_Completed_Ballot()
        '====================================================================================================
        Dim sMsg As String = "", sSubject As String, sTo As String, sFrom As String
        Dim iScore As Integer = 0
        '====================================================================================================
        sTo = ViewState("EmailPrimary") & ", " & ViewState("EmailSecondary")
        sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString
        sMsg = Me.lblBallotToSubmit.Text

        Try
            If chkSendSubmitEmail.Checked = True Then
                sSubject = "Successfully Submitted NHTA Adjudication Ballot for '" & Me.spnProductionName.InnerText & "'"

                ' Send the Email in HTML Format
                SendCDOEmail(sFrom, sTo, False, sSubject, sMsg, False, True, Master.UserLoginID, EMAIL_BALLOT_SUBMITTED)

                Me.lblStatus.Text = "An Email has sent to <B>" & ViewState("EmailPrimary") & ", " & ViewState("EmailSecondary") & "</B> with the following information:"
            End If

            Me.lblSaveResults.Text = sMsg

        Catch ex As Exception
            Me.spanFeedbackMessage.InnerHtml = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Sub

    Protected Sub Save_AttendanceDateandPlaybill()
        '====================================================================================================
        Dim DateTester As Date
        '====================================================================================================
        Try
            DateTester = (CDate(txtProductionDateAdjudicated_Planned.Text))

        Catch ex As Exception
            Me.spanFeedbackMessage.InnerHtml = "ERROR: Please provide a valid <B>Attended Adjudication</B> Date value."
            Exit Sub
        End Try

        Try
            Dim ht As New Hashtable
            'Temp Save the AdjudicatorAttendanceComment & ProductionDateAdjudicated_Planned
            ht.Add(1, ViewState("PK_Scoring_Temp_EntryID")) 'Scoring_Temp_EntryID, set to 0 if new record
            ht.Add(2, ViewState("PK_ScoringID"))            'ScoringID for this Ballot
            ht.Add(3, "ProductionDateAdjudicated_Planned")  'ScoreFieldName
            ht.Add(4, "AdjudicatorAttendanceComment")       'CommentFieldName
            ht.Add(5, DateTester.ToShortDateString)         'Score
            ht.Add(6, Me.txtAdjudicatorAttendanceComment.Text)
            ht.Add(99, Master.UserLoginID)             'LastUpdateByName

            Call Save_Scoring_Temp_Entry(ht)

            ht.Clear()
            Dim dtAd As New DataTable
            dtAd = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND CommentFieldName='FoundAdvertisement'")
            ht.Add(1, ViewState("PK_Scoring_Temp_EntryID_Ad")) 'Scoring_Temp_EntryID, set to 0 if new record
            ht.Add(2, ViewState("PK_ScoringID"))            'ScoringID for this Ballot
            ht.Add(3, "FoundAdvertisement")                 'ScoreFieldName
            ht.Add(4, "FoundAdvertisement")                 'CommentFieldName
            ht.Add(5, Me.ddlFoundAdvertisement.SelectedValue) 'Score
            ht.Add(6, "")                                   'No comment for this field
            ht.Add(99, Master.UserLoginID)             'LastUpdateByName

            Call Save_Scoring_Temp_Entry(ht)

            Me.spanFeedbackMessage.InnerHtml = "<b>SUCCESS</b> You have saved the record"

        Catch ex As Exception
            Throw ex
            Exit Sub
        End Try

    End Sub

End Class