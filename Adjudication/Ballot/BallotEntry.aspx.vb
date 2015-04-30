Imports Adjudication.CustomMail
Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CommonFunctions
Imports System.Data

Partial Public Class BallotEntry

    Inherits System.Web.UI.Page
    '============================================================================================
    ' PAGE COMMENTS:  This page saves all data entered in the Scoring_Temp_Entry table first, then when 
    ' all Nominated categories are  scored & commented the data in Scoring_Temp_Entry is saved to the
    ' Scoring table.
    ' NOTE: values in Scoring_Temp_Entry should never be deleted for reasons of backup and to assist
    '       in troubleshooting.
    '============================================================================================
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.PageTitleLabel = Me.Page.Title
        '============================================================================================
        ViewState("AccessLevel") = CInt(Session.Item("AccessLevel"))
        If Not (ViewState("AccessLevel") > 0 And ViewState("AccessLevel") <= 5) Then Response.Redirect("UnAuthorized.aspx")
        ViewState("LoginID") = Session("LoginID")
        '============================================================================================

        If Request.QueryString("ScoringID") Is Nothing And ViewState("PK_ScoringID") Is Nothing Then
            Me.lblErrors.Text = "ERROR: Cannot find selected Production/Ballot."
            Me.pnlErrors.Visible = True
            Me.pnlCommonQuestions.Visible = False
            Me.pnlStart.Visible = False
        Else

            If Not IsPostBack Then
                If ViewState("PK_ScoringID") Is Nothing Then    'to allow test setup to run properly
                    ViewState("PK_ScoringID") = Request.QueryString("ScoringID")
                End If

                Me.pnlErrors.Visible = False
                ViewState("DisplayOrder") = 0       ' Set start of ballot to top level

                Call Begin_Ballot_Entry()
                Call Populate_ApplicationDefaults()
                Call Populate_Ballot()
            End If


            If CInt(ViewState("AccessLevel").ToString) > 1 Then
                '=== Verify the User has access to view this Ballot ===
                If User_has_Access_to_Ballot(CInt(Session("PK_UserID").ToString), CInt(ViewState("PK_ScoringID").ToString)) = False Then
                    Me.lblErrors.Visible = True
                    Me.lblErrors.Text = "ERROR: You do not have access to edit or view this Ballot!"
                    Exit Sub
                End If
            End If

        End If

    End Sub

    Private Sub Begin_Ballot_Entry()
        '====================================================================================================
        'If existing temp Scoring_Temp_Entry record, use existing, else create copy of Scoring record in
        'table Scoring_Temp_Entry
        '====================================================================================================
        Dim dt As DataTable, dtAd As New DataTable
        '====================================================================================================
        Me.pnlErrors.Visible = False
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
                ht.Add(99, Session.Item("LoginID"))             'LastUpdateByName

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
            Me.lblTitle.Text = "<B>" & dt.Rows(0)("Title").ToString & "</b><br />" & dt.Rows(0)("ProductionType").ToString
            Me.lblTitlePerformance.Text = dt.Rows(0)("Title").ToString
            Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("CompanyType").ToString
            Me.lblFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.lblLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
        End If

        '====================================================================================================
        ' Get User and Scoring Information
        sSQL = "SELECT Users.LastName + ', ' + Users.FirstName as FullName, Company.CompanyName, " & _
                "       Users.EmailPrimary, Users.EmailSecondary, " & _
                "       Scoring.ReserveAdjudicator, " & _
                "       CompanyTypes.CompanyType, Company_1.CompanyName AS CompanyAdjudication,  CompanyTypes_1.CompanyType AS CompanyTypeAdjudication " & _
                " FROM Users INNER JOIN " & _
                "       Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN " & _
                "       Company ON Users.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                "       CompanyTypes ON Company.FK_CompanyTypeID = CompanyTypes.PK_CompanyTypeID INNER JOIN " & _
                "       Company Company_1 ON Scoring.FK_CompanyID_Adjudicator = Company_1.PK_CompanyID INNER JOIN " & _
                "       CompanyTypes CompanyTypes_1 ON Company_1.FK_CompanyTypeID = CompanyTypes_1.PK_CompanyTypeID " & _
                " WHERE Scoring.PK_ScoringID = " & ViewState("PK_ScoringID")

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblFullname.Text = dt.Rows(0)("FullName").ToString
            'Me.lblAdjudicatorCompany.Text = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("CompanyType").ToString
            Me.lblCompanyAdjudication.Text = dt.Rows(0)("CompanyAdjudication").ToString & " - " & dt.Rows(0)("CompanyTypeAdjudication").ToString
            ViewState("EmailPrimary") = dt.Rows(0)("EmailPrimary").ToString
            ViewState("EmailSecondary") = dt.Rows(0)("EmailSecondary").ToString
            ViewState("ReserveAdjudicator") = dt.Rows(0)("ReserveAdjudicator").ToString
        End If

    End Sub

    Private Sub Populate_Ballot()
        '====================================================================================================
        Dim dtBallot As DataTable, dtDefaults As DataTable
        '====================================================================================================
        dtDefaults = Get_ApplicationDefaults() 'to get the value for: DaysToWaitForScoring

        ' If not an admin, double check that user is assigned this adjudication by also listing the UserID
        dtBallot = DataAccess.Get_Ballot(ViewState("PK_ScoringID").ToString, IIf(ViewState("AccessLevel") > 1, Find_PK_UserID(ViewState("LoginID")), ""))

        ' If Ballot has been submitted
        If dtBallot.Rows.Count > 0 Then
            'if not an Administrator, check if Ballot can be edited by Adjudicator
            If ViewState("AccessLevel") > 1 Then
                ' Check if already submitted scores can be EDITed by Adjudicator
                If dtBallot.Rows(0)("BallotSubmitDate").ToString.Length > 6 Then
                    'check if setting for 'DaysToWaitForScoring' allows for Adjudicator edits or not
                    If Today > CDate(dtBallot.Rows(0)("BallotSubmitDate").ToString).AddDays(CDbl(dtDefaults.Rows(0)("DaysToWaitForScoring").ToString)) Then
                        If Not dtBallot.Rows(0)("BestProductionScore").ToString = "0" Then
                            ViewState("EnableControls") = 0      ' the Ballot has already been submitted
                        Else
                            ViewState("EnableControls") = 1      ' allow the Ballot to be updated
                        End If
                    End If
                End If
            End If

            ViewState("PK_ScoringID") = dtBallot.Rows(0)("PK_ScoringID").ToString
            ViewState("FK_NominationsID") = dtBallot.Rows(0)("FK_NominationsID").ToString
            ViewState("FK_CompanyID_Adjudicator") = dtBallot.Rows(0)("FK_CompanyID_Adjudicator").ToString
            ViewState("FK_UserID_Adjudicator") = dtBallot.Rows(0)("FK_UserID_Adjudicator").ToString
            ViewState("FK_ProductionID") = dtBallot.Rows(0)("FK_ProductionID").ToString
            ViewState("ReserveAdjudicator") = dtBallot.Rows(0)("ReserveAdjudicator").ToString
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

            '=== Populate_ProductionDetails using the ProductionID ===
            Call Populate_ProductionDetails(dtBallot.Rows(0)("FK_ProductionID").ToString)

        Else
            Me.lblErrors.Text = "<i>ERROR: You are not Assigned this Adjudication.</i>"
            Me.pnlErrors.Visible = True
        End If

    End Sub

    Private Sub Display_MatrixData(ByVal sScoreValue As String, ByVal sCategoryID As String)
        '====================================================================================================
        Dim dtMatrix As DataTable, sSQL As String, sMatrixRanges As String = ""
        '====================================================================================================
        Try
            Dim ScoreMin As Integer = CInt(ViewState("ScoringMinimum"))
            Dim ScoreMax As Integer = CInt(ViewState("ScoringMaximum"))

            If (CInt(sScoreValue) < ScoreMin Or CInt(sScoreValue) > ScoreMax) Then

                If CInt(sScoreValue) = 0 Then
                    sSQL = "SELECT  Category_ScoringRange.FK_CategoryID, ScoringRange.ScoringRangeMax, ScoringRange.ScoringRangeMin,  " & _
                            "       CAST(ScoringRange.ScoringRangeMax AS VARCHAR(5)) + ' - ' + CAST(ScoringRange.ScoringRangeMin AS VARCHAR(5)) AS HeaderScoringRanges, " & _
                            "	    Category_ScoringRange.MatrixAdjectives, Category_ScoringRange.MatrixDetail " & _
                            "	FROM Category_ScoringRange INNER JOIN ScoringRange ON Category_ScoringRange.FK_ScoringRangeID = ScoringRange.PK_ScoringRangeID " & _
                            "	WHERE (Category_ScoringRange.FK_CategoryID = " & sCategoryID & ") " & _
                            "	ORDER BY ScoringRangeMax DESC"

                    dtMatrix = DataAccess.Run_SQL_Query(sSQL)

                    If dtMatrix.Rows.Count > 0 Then
                        Me.MyAccordion.Visible = True
                        Me.MyAccordion.DataSource = dtMatrix.DefaultView
                        Me.MyAccordion.DataBind()

                        'For Each MyRow As DataRow In dtMatrix.Rows
                        'sMatrixRanges = sMatrixRanges & "<font color=""olive""><B>" & MyRow.Item("ScoringRangeMin").ToString & "-" & MyRow.Item("ScoringRangeMax").ToString & " Scoring Range: </B></span>" & _
                        'MyRow.Item("MatrixDetail").ToString & "<hr noshade> "
                        'Next
                        'Me.lblMatrixDetail.Visible = True
                        'Me.lblMatrixDetail.Text = sMatrixRanges

                        'Me.lblMatrixDetailTitle.Visible = True
                        'Me.lblMatrixDetailTitle.Text = "All Scoring Range Descriptions: <font size=1>(use scroll to see all ranges)</span>"
                        Me.lblMatrixAdjectives.Text = "[Enter your Score to view suggested Matrix Adjectives scoring range explanation.]"      'clear values
                    Else
                        Me.MyAccordion.Visible = False

                        Me.lblMatrixAdjectives.Text = "Matrix Adjectives not found - please email the system administrator."
                        'Me.lblMatrixDetail.Visible = True
                        'Me.lblMatrixDetail.Text = "Matrix Scoring Ranges not found for this category - please email the system administrator."
                        'Me.lblMatrixDetailTitle.Visible = False
                    End If

                Else
                    Me.lblMatrixAdjectives.Text = "[Enter your Score to view suggested Matrix Adjectives scoring range explanation.]"        'clear values
                    'Me.lblMatrixDetail.Visible = False
                    'Me.lblMatrixDetailTitle.Visible = False
                End If
            Else
                'Me.lblMatrixDetail.Visible = True

                sSQL = "SELECT  Category_ScoringRange.FK_CategoryID, ScoringRange.ScoringRangeMax, ScoringRange.ScoringRangeMin,  " & _
                        "       CAST(ScoringRange.ScoringRangeMax AS VARCHAR(5)) + ' - ' + CAST(ScoringRange.ScoringRangeMin AS VARCHAR(5)) AS HeaderScoringRanges, " & _
                        "	    Category_ScoringRange.MatrixAdjectives, Category_ScoringRange.MatrixDetail " & _
                        "	FROM Category_ScoringRange INNER JOIN ScoringRange ON Category_ScoringRange.FK_ScoringRangeID = ScoringRange.PK_ScoringRangeID " & _
                        "	WHERE (ScoringRange.ScoringRangeMax >= " & sScoreValue & ")  " & _
                        "		AND (ScoringRange.ScoringRangeMin <= " & sScoreValue & ") " & _
                        "		AND (Category_ScoringRange.FK_CategoryID = " & sCategoryID & ") "

                dtMatrix = DataAccess.Run_SQL_Query(sSQL)

                If dtMatrix.Rows.Count > 0 Then
                    Me.MyAccordion.DataSource = dtMatrix.DefaultView
                    Me.MyAccordion.DataBind()
                    Me.MyAccordion.SelectedIndex = 0

                    Me.lblMatrixAdjectives.Text = "<i>Suggested Adjectives:</i> " & dtMatrix.Rows(0)("MatrixAdjectives").ToString
                    'Me.lblMatrixDetail.Text = dtMatrix.Rows(0)("MatrixDetail").ToString
                    'Me.lblMatrixDetailTitle.Text = dtMatrix.Rows(0)("ScoringRangeMin").ToString & "-" & dtMatrix.Rows(0)("ScoringRangeMax").ToString & " Scoring Range Description:"
                    'Me.lblMatrixDetailTitle.Visible = True
                    'Me.lblMatrixDetail.Visible = True
                Else
                    Me.lblMatrixAdjectives.Text = "Adjectives not supplied - please email the system administrator."
                    'Me.lblMatrixDetail.Visible = False
                    'Me.lblMatrixDetailTitle.Visible = False
                End If
            End If

        Catch ex As Exception
            Me.pnlErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
        End Try

    End Sub

    Private Sub Display_CategoryNomination(Optional ByVal Direction As String = "NEXT")
        '====================================================================================================
        Dim dt As DataTable, sSQL As String, dtScore As DataTable, dtNom As DataTable
        Dim FullCategoryName As String = ""
        '====================================================================================================
        Me.pnlErrors.Visible = False
        Me.lblSavedCurrent.Visible = False
        Try
            Select Case Direction.ToUpper
                Case "NEXT"
                    ViewState("DisplayOrder") = (CInt(ViewState("DisplayOrder")) + 1).ToString
                    Me.pnlStart.Visible = False                                     'About Ballots/Date Adjudicated/NHTA Ad/Optional Comments
                    Me.pnlBallotInfo.Visible = True                                 'Category & Score w/Scoring details
                    Me.pnlNextPreviousButtons.Visible = True                        'In 2nd UpdatePanel containing both the PREVIOUS and NEXT arrow buttons

                Case "PREVIOUS"
                    If CInt(ViewState("DisplayOrder")) > 1 Then
                        ViewState("DisplayOrder") = (CInt(ViewState("DisplayOrder")) - 1).ToString
                        Me.pnlStart.Visible = False                                 'About Ballots/Date Adjudicated/NHTA Ad/Optional Comments
                    Else
                        ViewState("DisplayOrder") = 0

                        Me.pnlStart.Visible = True                                  'About Ballots/Date Adjudicated/NHTA Ad/Optional Comments
                        Me.pnlCommonQuestions_Title.Visible = True
                        Me.pnlCommonQuestions.Visible = True

                        Me.pnlBallotInfo.Visible = False                            'Category & Score w/Scoring details
                        Me.pnlNextPreviousButtons.Visible = False                   'In 2nd UpdatePanel containing both the PREVIOUS and NEXT arrow buttons
                        Exit Sub
                    End If

                Case "SAVE"
                    'do not advance, just save
            End Select

            '===============================================================================================================================================================================
            '=== STEP 1: Get 'current' Category that is being reviewed. 
            '===============================================================================================================================================================================
            sSQL = "SELECT TOP 1 PK_CategoryID, CategoryName, ScoringCriteria, ScoreFieldName, CommentFieldName, NominationFieldName, RoleFieldName, GenderFieldName, DisplayOrder, ActiveCategory, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate " & _
                    "   FROM Category " & _
                    "   WHERE (DisplayOrder >= " & ViewState("DisplayOrder") & ") AND (ActiveCategory = 1) " & _
                    "   ORDER BY DisplayOrder "

            dt = DataAccess.Run_SQL_Query(sSQL)

            If dt.Rows.Count > 0 Then
                '===============================================================================================================================================================================
                '=== STEP 2: Get Nominee Name and Role/Gender (where needed)
                '===============================================================================================================================================================================
                sSQL = ""       'reset string
                If dt.Rows(0)("NominationFieldName").ToString.Length > 1 Then sSQL = sSQL & dt.Rows(0)("NominationFieldName").ToString & ", "
                If dt.Rows(0)("RoleFieldName").ToString.Length > 1 Then sSQL = sSQL & dt.Rows(0)("RoleFieldName").ToString & ", "
                If dt.Rows(0)("GenderFieldName").ToString.Length > 1 Then sSQL = sSQL & dt.Rows(0)("GenderFieldName").ToString & ", "

                sSQL = "SELECT " & sSQL & " PK_NominationsID FROM Nominations INNER JOIN Production ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                        "   WHERE PK_NominationsID = " & ViewState("FK_NominationsID")
                dtNom = DataAccess.Run_SQL_Query(sSQL)

                '===============================================================================================================================================================================
                '=== STEP 3: Check if this Category was Nominated ===
                '===============================================================================================================================================================================
                If dtNom.Rows(0)(dt.Rows(0)("NominationFieldName").ToString).ToString.Length > 1 Then
                    If dt.Rows(0)("NominationFieldName").ToString.ToUpper = "TITLE" Then
                        Me.txtScore_Calculated_For_Production.Text = Get_BestProduction_Calculated_Temp_Score.ToString("#0.0")
                        Me.txtScore_Calculated_For_Production.Visible = True
                    Else
                        Me.txtScore_Calculated_For_Production.Visible = False
                    End If

                    '=== Assign value incase next DisplayOrder value is greater than the given txtDisplayOrder value ===
                    ViewState("NominationFieldName") = dt.Rows(0)("NominationFieldName").ToString
                    ViewState("ScoreFieldName") = dt.Rows(0)("ScoreFieldName").ToString
                    ViewState("CommentFieldName") = dt.Rows(0)("CommentFieldName").ToString
                    ViewState("DisplayOrder") = dt.Rows(0)("DisplayOrder").ToString
                    ViewState("PK_CategoryID") = dt.Rows(0)("PK_CategoryID").ToString

                    '=== Display acting Role information when needed === 
                    If dt.Rows(0)("RoleFieldName").ToString.Length > 1 Then
                        Me.lblNomineeName.Text = dtNom.Rows(0)(dt.Rows(0)("NominationFieldName").ToString).ToString & " as '<i>" & dtNom.Rows(0)(dt.Rows(0)("RoleFieldName").ToString).ToString & "</i>'"
                    Else
                        Me.lblNomineeName.Text = dtNom.Rows(0)(dt.Rows(0)("NominationFieldName").ToString).ToString
                    End If

                    '=== Display Category name with Correct Gender when needed === 
                    If dt.Rows(0)("GenderFieldName").ToString.Length > 1 Then
                        FullCategoryName = "Best " & dtNom.Rows(0)(dt.Rows(0)("GenderFieldName").ToString).ToString
                    Else
                        FullCategoryName = dt.Rows(0)("CategoryName").ToString
                    End If

                    Me.lblCategoryName.Text = FullCategoryName & ":"
                    Me.lblScoringCriteria_title.Text = FullCategoryName
                    Me.lblScoringCriteria.Text = dt.Rows(0)("ScoringCriteria").ToString

                    '===============================================================================================================================================================================
                    '=== STEP 4: Get previously Entered Scores and Comments: assume values in table Scoring_Temp_Entry are most accurate/recent (if found)
                    '===============================================================================================================================================================================
                    sSQL = "SELECT * FROM Scoring_Temp_Entry " & _
                            " WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='" & dt.Rows(0)("ScoreFieldName").ToString & "'" ' dt.Rows(0)("CommentFieldName").ToString 

                    dtScore = DataAccess.Run_SQL_Query(sSQL)

                    '=== Use data from table Scoring_Temp_Entry if found, if not found check Scoring table for any entry.
                    If dtScore.Rows.Count > 0 Then
                        ViewState("PK_Scoring_Temp_EntryID") = dtScore.Rows(0)("PK_Scoring_Temp_EntryID").ToString
                        Me.txtScore.Text = dtScore.Rows(0)("Score").ToString
                        Me.txtComment.Text = RemoveHTMLTags(dtScore.Rows(0)("Comment").ToString)
                    Else
                        '=== Needed for backwards compatibilty to edit Ballots submitted via old Scoring.aspx page
                        ViewState("PK_Scoring_Temp_EntryID") = "0"      ' Set to ZERO to indicate this is a new record in 'Scoring_Temp_Entry' 
                        dtScore.Clear()
                        sSQL = "SELECT PK_ScoringID, FK_CompanyID_Adjudicator, FK_UserID_Adjudicator, " & dt.Rows(0)("ScoreFieldName").ToString & ", " & dt.Rows(0)("CommentFieldName").ToString & _
                                "   FROM Scoring WHERE PK_ScoringID=" & ViewState("PK_ScoringID")

                        dtScore = DataAccess.Run_SQL_Query(sSQL)

                        If dtScore.Rows.Count > 0 Then
                            Me.txtScore.Text = dtScore.Rows(0)(dt.Rows(0)("ScoreFieldName").ToString).ToString
                            Me.txtComment.Text = RemoveHTMLTags(dtScore.Rows(0)(dt.Rows(0)("CommentFieldName").ToString).ToString)
                        Else
                            Me.txtScore.Text = "0"
                            Me.txtComment.Text = Nothing
                        End If
                    End If

                    '===============================================================================================================================================================================
                    '=== STEP 5: If a Score was supplied, display corresponding matrix value - or - hide matrix controls
                    '===============================================================================================================================================================================
                    Call Display_MatrixData(Me.txtScore.Text, ViewState("PK_CategoryID").ToString)

                    Dim iLen As Integer = Me.lblNomineeName.Text.Length * 10        'Make sure length doesnt exceed 700 pixels
                    Me.lblNomineeName.Width = Unit.Pixel(IIf(iLen > 700, 700, iLen))

                Else
                    '=== If no Nominations found for current category, goto next category ===
                    Call Display_CategoryNomination(Direction)
                End If
            Else
                '=== No more categories - Save the Ballot & Email to the user === 
                Me.pnlErrors.Visible = False            'True /'why did I ever show this??

                Me.pnlNextPreviousButtons.Visible = False
                Me.pnlAssignmentInfo_Title.Visible = False
                Me.pnlAssignmentInfo.Visible = False
                Me.pnlBallotInfo.Visible = False

                Me.pnlScoringSave.Visible = True
                Me.lblBallotToSubmit.Text = Display_Completed_Ballot()
            End If

        Catch ex As Exception
            Me.pnlErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
        End Try

    End Sub

    Private Function ValidateSave_ScoresComments(Optional ByVal ByPassValidation As Boolean = False) As Boolean
        '====================================================================================================
        Dim ReturnValue As Boolean = False
        Dim ScoreMin As Int16, ScoreMax As Int16
        Dim iMinCommentCount As Int16
        '====================================================================================================
        Me.pnlErrors.Visible = False
        Me.lblSavedCurrent.Visible = False

        If ByPassValidation = False Then
            Try
                ScoreMin = CInt(ViewState("ScoringMinimum"))
                ScoreMax = CInt(ViewState("ScoringMaximum"))
                iMinCommentCount = CInt(ConfigurationManager.AppSettings("AdjudicatorCommentMinimumCharacterCount"))

                If CInt(Me.txtScore.Text) < ScoreMin Or CInt(Me.txtScore.Text) > ScoreMax Then
                    Me.lblErrors.Text = "ERROR: For <b>" & Me.lblCategoryName.Text & "</b>, please provide a valid 'Score' between the ranges of <b>" & ViewState("ScoringMinimum") & "</b> and <b>" & ViewState("ScoringMaximum") & "</b>"
                    Me.pnlErrors.Visible = True
                Else
                    If Me.txtComment.Text.Length <= iMinCommentCount Then                           ' Check MINIMUM Length of Comments
                        Me.lblErrors.Text = "ERROR: You must submit <b>Comments</b> for the <b>" & Me.lblCategoryName.Text & "</b> . "
                        Me.pnlErrors.Visible = True
                    Else
                        If Me.txtComment.Text.Length >= 8000 Then                     ' Check MAXIMUM Length of Comments
                            Me.lblErrors.Text = "ERROR: Your comments for <b>" & Me.lblCategoryName.Text & "</b> exceed 8000 Characters. Please keep your comments a bit shorter."
                            Me.pnlErrors.Visible = True
                        End If
                    End If
                End If

                Me.lblSavedCurrent.Visible = True

            Catch ex As Exception
                Me.lblErrors.Text = "ERROR: Please provide a VALID <b>" & Me.lblCategoryName.Text & "</b> score value " & " between the ranges of <b>" & ViewState("ScoringMinimum") & "</b> and <b>" & ViewState("ScoringMaximum") & "</b>"
                Me.pnlErrors.Visible = True
            End Try
        End If

        '=== if no entry errors, Save in holding table: Scoring_Temp_Entry
        If Me.pnlErrors.Visible = False Then ReturnValue = Save_Temp_BallotNomination()

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
            ht.Add(5, RemoveInvalidSQLCharacters(Me.txtScore.Text)) 'Score
            ht.Add(6, Me.txtComment.Text)
            ht.Add(99, Session.Item("LoginID"))                     'LastUpdateByName

            Call Save_Scoring_Temp_Entry(ht)

            If ViewState("PK_Scoring_Temp_EntryID").ToString = "0" Then
                '=== Get the PK_Scoring_Temp_EntryID value of new record
                Dim dt As DataTable = Run_SQL_Query("SELECT * FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & ViewState("PK_ScoringID") & " AND ScoreFieldName='" & ViewState("ScoreFieldName") & "'")
                ViewState("PK_Scoring_Temp_EntryID") = dt.Rows(0)("PK_Scoring_Temp_EntryID").ToString
            End If

            Return True

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR Saving this Score/Comment: " & ex.Message
            Me.pnlErrors.Visible = True
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
        'sBallot = sBallot & "<br />Production: <B>" & Me.lblTitlePerformance.Text & "</b>"
        'sBallot = sBallot & "<br />Producing Company: <B>" & Me.lblCompanyName.Text & "</b>"
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

        If Save_Scoring(sDataValues) = True Then
            If Request.QueryString("Admin") = "True" Then
                Call Email_Completed_Ballot()
                Response.Redirect("AdminScoring.aspx")
            Else
                Call Email_Completed_Ballot()
                Me.pnlBallotInfo.Visible = False
                Me.pnlScoringSave.Visible = False
                Me.pnlResults.Visible = True
                Me.lbtnCancel.Visible = False
            End If
        Else
            Me.lblErrors.Text = "ERROR: Saving Scoring Data"
            Me.pnlErrors.Visible = True
        End If
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
                sSubject = "Successfully Submitted NHTA Adjudication Ballot for '" & Me.lblTitlePerformance.Text & "'"

                ' Send the Email in HTML Format
                SendCDOEmail(sFrom, sTo, False, sSubject, sMsg, False, True, Session("LoginID"), EMAIL_BALLOT_SUBMITTED)

                Me.lblStatus.Text = "An Email has sent to <B>" & ViewState("EmailPrimary") & ", " & ViewState("EmailSecondary") & "</B> with the following information:"
            End If

            Me.lblSaveResults.Text = sMsg

        Catch ex As Exception
            Me.pnlErrors.Visible = True
            Me.lblErrors.Text = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Sub

    Private Sub txtScore_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtScore.TextChanged
        Try
            Call Display_MatrixData(Me.txtScore.Text, ViewState("PK_CategoryID"))
            Me.txtComment.Focus()
        Catch ex As Exception
            Me.pnlErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
        End Try

    End Sub

    Private Sub ibtnComment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnComment.Click
        If ibtnComment.ImageUrl.ToString = "Images/PlusIcon.jpg" Then
            Me.txtComment.Height = Unit.Pixel(350)
            ibtnComment.ImageUrl = "Images/MinusIcon.jpg"
        Else
            Me.txtComment.Height = Unit.Pixel(150)
            ibtnComment.ImageUrl = "Images/PlusIcon.jpg"
        End If
    End Sub

    Private Sub btnMainMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMainMenu.Click
        Response.Redirect("MainPage.aspx")
    End Sub

    Protected Sub ibtn_NextStart_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_NextStart.Click
        '====================================================================================================
        Dim DateTester As Date
        '====================================================================================================
        Try
            DateTester = (CDate(txtProductionDateAdjudicated_Planned.Text))

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: Please provide a VALID <B>Attended Adjudication</B> Date value."
            Me.pnlErrors.Visible = True
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
            ht.Add(99, Session.Item("LoginID"))             'LastUpdateByName

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
            ht.Add(99, Session.Item("LoginID"))             'LastUpdateByName

            Call Save_Scoring_Temp_Entry(ht)

        Catch ex As Exception
            Throw ex
            Exit Sub
        End Try

        Me.pnlCommonQuestions_Title.Visible = False
        Me.pnlCommonQuestions.Visible = False

        Me.pnlBallotInfo.Visible = True
        Me.pnlAssignmentInfo.Visible = True

        Call Display_CategoryNomination("NEXT")

    End Sub

    Protected Sub ibtn_Previous_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_Previous.Click
        If ValidateSave_ScoresComments(True) = True Then Call Display_CategoryNomination("PREVIOUS")
    End Sub

    Protected Sub ibtn_Next_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_Next.Click
        If ValidateSave_ScoresComments() = True Then Call Display_CategoryNomination("NEXT")
    End Sub

    Sub NextPrevious_Button_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        Me.pnlErrors.Visible = False

        If e.CommandArgument = "PREVIOUS" Then
            If ValidateSave_ScoresComments(True) = True Then Call Display_CategoryNomination("PREVIOUS")
        ElseIf e.CommandArgument = "NEXT" Then
            If ValidateSave_ScoresComments() = True Then Call Display_CategoryNomination("NEXT")
        End If
    End Sub

    Protected Sub ibtn_Previous_ToSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_Previous_ToSubmit.Click
        Me.pnlScoringSave.Visible = False

        Me.pnlNextPreviousButtons.Visible = True
        Me.pnlAssignmentInfo_Title.Visible = True
        Me.pnlAssignmentInfo.Visible = True
        Me.pnlBallotInfo.Visible = True

        Call Display_CategoryNomination("PREVIOUS")
    End Sub

    Protected Sub ibtn_SubmitBallot_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_SubmitBallot.Click
        Call Save_Completed_Ballot()
    End Sub

    Protected Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        If ValidateSave_ScoresComments() = True Then
            'saves if possible (should put warning message to user asking to complete current to save??)
        End If

        Response.Redirect("MainPage.aspx")

    End Sub

    Protected Sub ibtn_SaveCurrent_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_SaveCurrent.Click
        If ValidateSave_ScoresComments() = True Then
            'saves if possible (should put warning message to user asking to complete current to save??)
        End If
    End Sub
End Class