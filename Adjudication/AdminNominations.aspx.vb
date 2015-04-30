Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CustomMail

Public Class AdminNominations
    Inherits System.Web.UI.Page

    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lblSucessfulUpdate As System.Web.UI.WebControls.Label
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents btnUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents txtLastTrainingDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents pnlUserData As System.Web.UI.WebControls.Panel
    Protected WithEvents lblLastUpdateByName As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastUpdateByDate As System.Web.UI.WebControls.Label
    Protected WithEvents pnlAddEditData As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlSelectVenue As System.Web.UI.WebControls.Panel
    Protected WithEvents lbtnAddVenue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents gridVenue As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtPK_NominationsID As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkRequiresAdjudication As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblVenueName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCompanyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblFirstPerformanceDateTime As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastPerformanceDateTime As System.Web.UI.WebControls.Label
    Protected WithEvents txtDirector As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMusicalDirector As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtChoreographer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestActor1Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtScenicDesigner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLightingDesigner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSoundDesigner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCostumeDesigner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOriginalPlaywright As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestActor1Role As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestActor2Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestActor2Role As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActor1Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActor1Role As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActor2Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActor2Role As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActress1Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActress1Role As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActress2Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBestSupportingActress2Role As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPK_ProductionID As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblUpdateNotes As System.Web.UI.WebControls.Label
    Protected WithEvents btnDelete As System.Web.UI.WebControls.Button
    Protected WithEvents lblConfirmDelete As System.Web.UI.WebControls.Label
    Protected WithEvents lblScoreAdjudictor As System.Web.UI.WebControls.Label
    Protected WithEvents lblScoreProduction As System.Web.UI.WebControls.Label
    Protected WithEvents lblScoreProducingCompany As System.Web.UI.WebControls.Label
    Protected WithEvents btnDeleteConfirm As System.Web.UI.WebControls.Button
    Protected WithEvents btnDeleteCancel As System.Web.UI.WebControls.Button
    Protected WithEvents pnlDeleteConfirm As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlConfirmDeleteInfo As System.Web.UI.WebControls.Panel
    Protected WithEvents gridSub As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCannotDelete As System.Web.UI.WebControls.Label
    Protected WithEvents ddlBestPerformer1Gender As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlBestPerformer2Gender As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtPK_CompanyID As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMusicDirector As System.Web.UI.WebControls.Label
    Protected WithEvents lblChoreographer As System.Web.UI.WebControls.Label
    Protected WithEvents lblOriginalPlaywright As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblSuccess As System.Web.UI.WebControls.Label
    Protected WithEvents rblEmailInfo As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtAdminEmailComments As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnCancel As System.Web.UI.WebControls.Button

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iProductionID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        '============================================================================================
        If Not (iAccessLevel >= 1 And iAccessLevel <= 3) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        ' Find if a Nomination needs to be EDITed
        If Request.QueryString("ProductionID") <> "" Then
            iProductionID = Request.QueryString("ProductionID")
            txtPK_ProductionID.Text = iProductionID
            If Not IsPostBack Then
                'Call Populate_DropDowns()
                Call Populate_Data()
                If iAccessLevel = 1 Then Me.btnDelete.Visible = True
            End If
        Else
            Me.pnlAddEditData.Visible = False
            Me.lblErrors.Text = "ERROR: There is no Production selected.  Please view Productions first to then see the Nominations."
            Me.lblErrors.Visible = True
        End If
    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Nomination(iProductionID.ToString)

        If dt.Rows.Count > 0 Then
            Me.txtPK_CompanyID.Text = dt.Rows(0)("PK_CompanyID").ToString
            Me.lblTitle.Text = dt.Rows(0)("Title").ToString & " - " & dt.Rows(0)("ProductionType").ToString
            Me.chkRequiresAdjudication.Checked = dt.Rows(0)("RequiresAdjudication").ToString
            Me.lblVenueName.Text = dt.Rows(0)("VenueName").ToString & ", " & dt.Rows(0)("City").ToString()
            Me.lblProductionCategory.Text = dt.Rows(0)("ProductionCategory").ToString
            Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString
            Me.lblFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.lblLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            'Me.txtWebsite.Text = dt.Rows(0)("Website").ToString

            '=== Enable Music Director & Choreographer controls for MUSICALS(1) ===
            If CInt(dt.Rows(0)("PK_ProductionTypeID")) = 1 Then
                Me.lblMusicDirector.Enabled = True
                Me.txtMusicalDirector.Enabled = True
                Me.lblChoreographer.Enabled = True
                Me.txtChoreographer.Enabled = True
            End If

            '=== Enable Original Playwright controls for ORIGINALS
            If CInt(dt.Rows(0)("OriginalProduction")) = 1 Then
                Me.lblOriginalPlaywright.Enabled = True
                Me.txtOriginalPlaywright.Enabled = True
            End If

            If dt.Rows(0)("PK_NominationsID").ToString = "" Then
                'Me.btnUpdate.Text = "Add"
            Else
                Me.txtPK_NominationsID.Text = dt.Rows(0)("PK_NominationsID").ToString
                Me.txtDirector.Text = dt.Rows(0)("Director").ToString
                Me.txtMusicalDirector.Text = dt.Rows(0)("MusicalDirector").ToString
                Me.txtChoreographer.Text = dt.Rows(0)("Choreographer").ToString
                Me.txtScenicDesigner.Text = dt.Rows(0)("ScenicDesigner").ToString
                Me.txtLightingDesigner.Text = dt.Rows(0)("LightingDesigner").ToString
                Me.txtSoundDesigner.Text = dt.Rows(0)("SoundDesigner").ToString
                Me.txtCostumeDesigner.Text = dt.Rows(0)("CostumeDesigner").ToString
                Me.txtOriginalPlaywright.Text = dt.Rows(0)("OriginalPlaywright").ToString
                Me.txtBestActor1Name.Text = dt.Rows(0)("BestPerformer1Name").ToString
                Me.txtBestActor1Role.Text = dt.Rows(0)("BestPerformer1Role").ToString
                Me.ddlBestPerformer1Gender.SelectedValue = dt.Rows(0)("BestPerformer1Gender").ToString
                Me.txtBestActor2Name.Text = dt.Rows(0)("BestPerformer2Name").ToString
                Me.txtBestActor2Role.Text = dt.Rows(0)("BestPerformer2Role").ToString
                Me.ddlBestPerformer2Gender.SelectedValue = dt.Rows(0)("BestPerformer2Gender").ToString
                Me.txtBestSupportingActor1Name.Text = dt.Rows(0)("BestSupportingActor1Name").ToString
                Me.txtBestSupportingActor1Role.Text = dt.Rows(0)("BestSupportingActor1Role").ToString
                Me.txtBestSupportingActor2Name.Text = dt.Rows(0)("BestSupportingActor2Name").ToString
                Me.txtBestSupportingActor2Role.Text = dt.Rows(0)("BestSupportingActor2Role").ToString
                Me.txtBestSupportingActress1Name.Text = dt.Rows(0)("BestSupportingActress1Name").ToString
                Me.txtBestSupportingActress1Role.Text = dt.Rows(0)("BestSupportingActress1Role").ToString
                Me.txtBestSupportingActress2Name.Text = dt.Rows(0)("BestSupportingActress2Name").ToString
                Me.txtBestSupportingActress2Role.Text = dt.Rows(0)("BestSupportingActress2Role").ToString
                'Me.txtComments.Text = dt.Rows(0)("Comments").ToString
                Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
                Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString
            End If

            ' if exceeded, lock the user fields and hide the Update button
            Dim MaxStartDate As Date
            ' sSQL = "SELECT TOP 1 DaysToSubmitProduction, DaysToAllowNominationEdits, NumAdjudicatorsPerShow FROM ApplicationDefaults "
            dt = DataAccess.Get_ApplicationDefaults

            MaxStartDate = CDate(lblFirstPerformanceDateTime.Text).Subtract(TimeSpan.FromDays(CInt(dt.Rows(0)("DaysToAllowNominationEdits").ToString)))

            ' If non-Admin user: check if time period to allow edits is exceeded
            If MaxStartDate <= Today.Date Then
                If iAccessLevel > 1 Then
                    Me.lblUpdateNotes.Text = "The Date to edit these Nominations has passed.  Please contact the Adminstrator if changes are needed."
                    Me.txtDirector.Enabled = False
                    Me.txtMusicalDirector.Enabled = False
                    Me.txtChoreographer.Enabled = False
                    Me.txtScenicDesigner.Enabled = False
                    Me.txtLightingDesigner.Enabled = False
                    Me.txtSoundDesigner.Enabled = False
                    Me.txtCostumeDesigner.Enabled = False
                    Me.txtOriginalPlaywright.Enabled = False
                    Me.txtBestActor1Name.Enabled = False
                    Me.txtBestActor1Role.Enabled = False
                    Me.ddlBestPerformer1Gender.Enabled = False
                    Me.txtBestActor2Name.Enabled = False
                    Me.txtBestActor2Role.Enabled = False
                    Me.ddlBestPerformer2Gender.Enabled = False
                    Me.txtBestSupportingActor1Name.Enabled = False
                    Me.txtBestSupportingActor1Role.Enabled = False
                    Me.txtBestSupportingActor2Name.Enabled = False
                    Me.txtBestSupportingActor2Role.Enabled = False
                    Me.txtBestSupportingActress1Name.Enabled = False
                    Me.txtBestSupportingActress1Role.Enabled = False
                    Me.txtBestSupportingActress2Name.Enabled = False
                    Me.txtBestSupportingActress2Role.Enabled = False
                    Me.btnUpdate.Visible = False
                    Me.btnDelete.Visible = False
                Else
                    Me.lblUpdateNotes.Text = "WARNING: The Date for Non-Administrators to edit these Nominations has passed. "
                End If
            End If

        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.pnlAddEditData.Visible = False
        Me.pnlDeleteConfirm.Visible = True
        Me.lblConfirmDelete.Text = "<br />Are you sure you want to Delete the following:"
        Me.lblScoreProduction.Text = "Nominations For the Production <B>" & Me.lblTitle.Text & "</B>"
        Me.lblScoreProducingCompany.Text = "Produced by <B>" & Me.lblCompanyName.Text & "</B>"
    End Sub

    Private Sub btnDeleteCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteCancel.Click
        Me.pnlAddEditData.Visible = True
        Me.pnlDeleteConfirm.Visible = False
    End Sub

    Private Sub btnDeleteConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteConfirm.Click
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_ScoringID, Users.PK_UserID, Users.LastName + ', ' + Users.FirstName as FullName, " & _
                "   Company.CompanyName, Company.PK_CompanyID, " & _
                "   ProductionDateAdjudicated_Planned, ProductionDateAdjudicated_Actual, AdjudicatorRequestsReassignment, " & _
                "	CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END as AdjudicatorToSeeProduction, " & _
                "   DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore " & _
                "	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score " & _
                "	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score " & _
                "	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore,  " & _
                "   Scoring.FK_NominationsID, Scoring.LastUpdateByName, Scoring.LastUpdateByDate, Scoring.CreateByName, Scoring.CreateByDate " & _
                " FROM Scoring INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                "       INNER JOIN Company ON Scoring.FK_CompanyID_Adjudicator = Company.PK_CompanyID " & _
                " WHERE (Scoring.FK_NominationsID = " & Me.txtPK_NominationsID.Text & ") " & _
                " ORDER BY Users.LastName, Users.FirstName "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblCannotDelete.Text = "ERROR: Cannot Delete this Nomination until you Remove all Assigned Adjudicators!"
            Me.gridSub.Visible = True
            Me.gridSub.DataSource = dt
            Me.gridSub.DataBind()
            Me.pnlConfirmDeleteInfo.Visible = False
            Me.btnDeleteConfirm.Visible = False
            Exit Sub
        End If

        sSQL = "DELETE FROM Nominations WHERE PK_NominationsID=" & Me.txtPK_NominationsID.Text

        DataAccess.SQLDelete(sSQL)

        Me.lblConfirmDelete.Text = "<br />The Nomination for <B>" & Me.lblTitle.Text & "</B> has been Deleted."
        Me.lblScoreProduction.Text = ""
        Me.lblScoreProducingCompany.Text = ""

    End Sub

    Private Function ValidateInput() As Boolean
        Dim sErrorMsg As String = ""

        If (Me.txtBestActor1Name.Text.Length > 0 And Me.txtBestActor1Role.Text.Length = 0) Or (Me.txtBestActor1Name.Text.Length = 0 And Me.txtBestActor1Role.Text.Length > 0) Then
            sErrorMsg = sErrorMsg & " You Must enter in BOTH a Best Performer #1 and the Best Performer #2 Role." & vbCrLf
        Else
            If Me.ddlBestPerformer1Gender.SelectedValue.Length = 0 And Me.txtBestActor1Name.Text.Length > 0 And Me.txtBestActor1Role.Text.Length > 0 Then
                sErrorMsg = sErrorMsg & " You Must select if the Best Performer #1 is an ACTOR or an ACTRESS." & vbCrLf
            End If
        End If

        If (Me.txtBestActor2Name.Text.Length > 0 And Me.txtBestActor2Role.Text.Length = 0) Or (Me.txtBestActor2Name.Text.Length = 0 And Me.txtBestActor2Role.Text.Length > 0) Then
            sErrorMsg = sErrorMsg & " You Must enter in BOTH a Best Performer #2 and the Best Performer #2 Role." & vbCrLf
        Else
            If Me.ddlBestPerformer2Gender.SelectedValue.Length = 0 And Me.txtBestActor2Name.Text.Length > 0 And Me.txtBestActor2Role.Text.Length > 0 Then
                sErrorMsg = sErrorMsg & " You Must select if the Best Performer #2 is an ACTOR or an ACTRESS." & vbCrLf
            End If
        End If

        If (Me.txtBestSupportingActor1Name.Text.Length > 0 And Me.txtBestSupportingActor1Role.Text.Length = 0) Or (Me.txtBestSupportingActor1Name.Text.Length = 0 And Me.txtBestSupportingActor1Role.Text.Length > 0) Then
            sErrorMsg = sErrorMsg & " You Must enter in BOTH a Best Supporting Actor #1 and the Best Best Supporting Actor #1 Role." & vbCrLf
        End If

        If (Me.txtBestSupportingActor2Name.Text.Length > 0 And Me.txtBestSupportingActor2Role.Text.Length = 0) Or (Me.txtBestSupportingActor2Name.Text.Length = 0 And Me.txtBestSupportingActor2Role.Text.Length > 0) Then
            sErrorMsg = sErrorMsg & " You Must enter in BOTH a Best Supporting Actor #2 and the Best Best Supporting Actor #2 Role." & vbCrLf
        End If

        If (Me.txtBestSupportingActress1Name.Text.Length > 0 And Me.txtBestSupportingActress1Role.Text.Length = 0) Or (Me.txtBestSupportingActress1Name.Text.Length = 0 And Me.txtBestSupportingActress1Role.Text.Length > 0) Then
            sErrorMsg = sErrorMsg & " You Must enter in BOTH a Best Supporting Actress #1 and the Best Best Supporting Actress #1 Role." & vbCrLf
        End If

        If (Me.txtBestSupportingActress2Name.Text.Length > 0 And Me.txtBestSupportingActress2Role.Text.Length = 0) Or (Me.txtBestSupportingActress2Name.Text.Length = 0 And Me.txtBestSupportingActress2Role.Text.Length > 0) Then
            sErrorMsg = sErrorMsg & " You Must enter in BOTH a Best Supporting Actress #2 and the Best Best Supporting Actress #2 Role." & vbCrLf
        End If

        If Not sErrorMsg = "" Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & sErrorMsg
            ValidateInput = False
        Else
            Me.lblErrors.Visible = False
            ValidateInput = True
        End If
    End Function

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim sDataValues(30) As String
        '====================================================================================================
        Me.lblErrors.Visible = False
        Me.lblSuccess.Visible = False

        If ValidateInput() = True Then
            sDataValues(1) = Me.txtPK_NominationsID.Text
            sDataValues(2) = Me.txtPK_ProductionID.Text
            sDataValues(3) = Me.txtDirector.Text
            sDataValues(4) = Me.txtMusicalDirector.Text
            sDataValues(5) = Me.txtChoreographer.Text
            sDataValues(6) = Me.txtScenicDesigner.Text
            sDataValues(7) = Me.txtLightingDesigner.Text
            sDataValues(8) = Me.txtSoundDesigner.Text
            sDataValues(9) = Me.txtCostumeDesigner.Text
            sDataValues(10) = Me.txtOriginalPlaywright.Text
            sDataValues(11) = Me.txtBestActor1Name.Text
            sDataValues(12) = Me.txtBestActor1Role.Text
            sDataValues(13) = Me.txtBestActor2Name.Text
            sDataValues(14) = Me.txtBestActor2Role.Text
            sDataValues(15) = Me.txtBestSupportingActor1Name.Text
            sDataValues(16) = Me.txtBestSupportingActor1Role.Text
            sDataValues(17) = Me.txtBestSupportingActor2Name.Text
            sDataValues(18) = Me.txtBestSupportingActor2Role.Text
            sDataValues(19) = Me.txtBestSupportingActress1Name.Text
            sDataValues(20) = Me.txtBestSupportingActress1Role.Text
            sDataValues(21) = Me.txtBestSupportingActress2Name.Text
            sDataValues(22) = Me.txtBestSupportingActress2Role.Text
            sDataValues(23) = ""    'Me.txtComments.Text
            sDataValues(24) = Me.sLoginID

            ' Only Add the Gender if a Name and Role was entered in
            If Me.txtBestActor1Name.Text.Length > 0 And Me.txtBestActor1Role.Text.Length > 0 Then
                sDataValues(25) = Me.ddlBestPerformer1Gender.SelectedValue.ToString
            Else
                sDataValues(25) = ""
            End If
            ' Only Add the Gender if a Name and Role was entered in
            If Me.txtBestActor2Name.Text.Length > 0 And Me.txtBestActor2Role.Text.Length > 0 Then
                sDataValues(26) = Me.ddlBestPerformer2Gender.SelectedValue.ToString
            Else
                sDataValues(26) = ""
            End If

            If Save_Nominations(sDataValues) = True Then
                Call EmailCompanyNominations(True)

                If Request.QueryString("Liaison") = "True" Then
                    'Response.Redirect("AdminProduction.aspx")
                    Me.lblSuccess.Visible = True
                    Me.lblSuccess.Text = "Successfully Saved Nominations for " & Me.lblTitle.Text
                Else
                    Response.Redirect("AdminProductionList.aspx")
                End If
            Else
                Me.lblErrors.Text = "ERROR: Saving Nominations"
                Me.lblErrors.Visible = True
            End If
        End If

    End Sub

    Private Function EmailCompanyNominations(Optional ByVal PopUpMessage As Boolean = False) As Boolean
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False
        '============================================================================================

        If Me.rblEmailInfo.SelectedIndex = 1 Then
            If Email_AssignedAdjudicators() = True Then
                ' Let user know that the email was sent.
                Me.pnlAddEditData.Visible = False
                Me.lblSucessfulUpdate.Visible = True
                Me.lblSucessfulUpdate.Text = Me.lblSucessfulUpdate.Text & "<br /><br />Successfully Updated and Emailed Nomination information to all Liaisons and Adjudicators for " & Me.lblCompanyName.Text
            End If
        End If

    End Function


    Private Function Email_AssignedAdjudicators() As Boolean
        '============================================================================================
        Dim dt As DataTable
        Dim sSubject As String = "", sBody As String = "", sTo As String = "", sFrom As String = ""
        Dim sToProductingCompany As String = "", sToProductingCompanyNames As String = ""
        Dim sToNames As String = "", sUserCompanyID As String = ""
        Dim bEmailAddressError As Boolean = True
        Dim i As Int16 = 0, str As String = String.Empty
        Dim AdminComments As String = ""
        '============================================================================================
        Me.lblErrors.Visible = False

        Try
            '=== get all Assigned Adjudicators
            dt = DataAccess.Get_ProductionAdjudicators(Me.txtPK_ProductionID.Text)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    str += dr.Item("PK_UserID").ToString & ", " & dr.Item("PK_CompanyID").ToString & ", "
                Next
                str = str.Substring(0, str.Length - 2)
            End If

            '=== set default FROM email address
            sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString

            '=== Producing Company Email Reciepients 
            sToProductingCompany = Get_CompanyMemberEmails(Me.txtPK_CompanyID.Text, , 3, True)
            sToProductingCompanyNames = Get_CompanyMemberEmails(Me.txtPK_CompanyID.Text, 3, True, True)

            Dim sAdjToEmail() As String = str.Split(",")                          'Previously Stored IDs as:  Adjudicator UserID, Adjudicator CompanyID,

            Do While i < sAdjToEmail.Length

                If sAdjToEmail.Length > 1 Then                                                                  '=== 1 element means NO Adjudicators assigned - just email the Company
                    sTo = Get_CompanyMemberEmails(sAdjToEmail(i + 1), sAdjToEmail(i), 3, True, False)           '=== Adjudicating Company: Email Reciepients (Liaisons and Company emails addresses)
                    sToNames = Get_CompanyMemberEmails(sAdjToEmail(i + 1), sAdjToEmail(i), 3, True, True)
                End If

                If sToNames.Length > 6 Or sToProductingCompanyNames.Length > 6 Then
                    sToNames = sToNames & sToProductingCompanyNames
                    sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1)                             'removes last comma
                    sToNames = sToNames.Replace(",", "<li>")
                    sToNames = "<hr noshade><B>NOTE:</b> This email has been sent to the Producing Theatre Company and all Assigned Adjudcators: " & _
                                "<ul><li>" & sToNames & "</ul>"
                End If

                If Me.txtAdminEmailComments.Text.Length > 1 Then AdminComments = "<br /><p style=""BACKGROUND-COLOR: lemonchiffon; ""><B><FONT COLOR=#404000>NHTA ADMINISTRATOR COMMENTS:</span></B> " & Me.txtAdminEmailComments.Text & "</p>"

                ' === Create the Text for the Email Message ======================================================================
                sSubject = "NHTA | Nominations Set for '" & Me.lblTitle.Text & "' produced by '" & Me.lblCompanyName.Text & "'"
                sBody = sBody & AdminComments
                sBody = sBody & "NOTE: This message has been emailed to all NHTA Active Liaisons and Adjudicators for " & Me.lblCompanyName.Text
                sBody = sBody & "<hr noshade>"
                sBody = sBody & "Nominations have been updated for: <b>" & Me.lblTitle.Text & "</b> produced by '" & Me.lblCompanyName.Text & "'<br />"
                sBody = sBody & "to be performed from <b>" & Me.lblFirstPerformanceDateTime.Text & "</b> thru <b>" & Me.lblLastPerformanceDateTime.Text & "</b><br />"
                sBody = sBody & "to be performed at <b>" & lblVenueName.Text & "</b><br /><br />"
                sBody = sBody & "<hr noshade>"
                sBody = sBody & "<b>" & Me.sLoginID & "</b> has updated the Nominations which are:<br />"
                sBody = sBody & Common.FormatEmailHTML_Nomination(Me.txtPK_ProductionID.Text)
                sBody = sBody & "<hr noshade>"
                sBody = sBody & "<br />Adjudicators should use the Production Information below to make reservations for the Adjudications:<br /><br />"
                sBody = sBody & Common.FormatEmailHTML_Production(Me.txtPK_ProductionID.Text)
                sBody = sBody & "<hr noshade>"
                sBody = sBody & "<I>Producing Liaison: If you have not already done so, please fill out <u>all</u> fields for Ticket information.</I><br /><br />"
                sBody = sBody & "<br /><br />"

                sBody = sBody & "Thank you.<br /><br />"
                sBody = sBody & FormatEmailHTML_AutomatedEmailDisclaimer()
                sBody = sBody & sToNames
                sBody = sBody & Common.Get_EmailFooter()

                ' === Send the Email ========================================================================================
                sTo = sTo & sToProductingCompany
                SendCDOEmail(sFrom, sTo, False, sSubject, sBody, True, True, Session("LoginID"), EMAIL_ADJUDICATOR_ASSIGNED)
                sBody = String.Empty

                i = i + 2                                                                                   '=== TO Step Properly to next 
            Loop

            Me.lblSucessfulUpdate.Visible = True
            Me.lblSucessfulUpdate.Text = Me.lblSucessfulUpdate.Text + "<br /><br />Production update sucessfully Emailed to Producing Company members and <b>" & dt.Rows.Count.ToString & "</b> Assigned Adjudicators."

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: Sending Emails.  DETAILS: " & ex.Message
            Me.lblErrors.Visible = True
            Return False
        End Try

    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Request.QueryString("Admin") = "True" Then
            Response.Redirect("AdminProductionList.aspx")
        Else
            Response.Redirect("AdminProduction.aspx")
        End If

    End Sub
End Class
