Imports Adjudication.CustomMail
Imports Adjudication.DataAccess

Partial Public Class AdjudicatorComments
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iProductionID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1 Or iAccessLevel = 2 Or iAccessLevel = 3) Then Response.Redirect("UnAuthorized.aspx")
        sLoginID = Session("LoginID")
        '============================================================================================
        Me.lblErrors.Visible = False
        Me.lblStatus.Visible = False

        If Request.QueryString("ProductionID") <> "" Then iProductionID = Request.QueryString("ProductionID")
        If Request.QueryString("ShowAll") = "True" Then Me.chkShowAll.Checked = True

        Dim dt As DataTable
        '>>> Check that awards show date has passed <<<
        Try
            dt = DataAccess.Get_ApplicationDefaults
            If CDate(dt.Rows(0)("NHTAwardsShowDate").ToString).AddDays(1) > Date.Now Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: You cannot view Comments until after the NHTA Awards Show has happened.<br />Please check back after the awards show.  <br /><br />Thank you."
                Me.pnlBallotInfo.Visible = False
                Me.pnlUserData.Visible = False
                Exit Sub
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPage.aspx")
        End Try

        Me.txtPK_CompanyID.Text = DataAccess.Find_CompanyForUserLoginID(sLoginID)     ' Find the Company For this Liaison

        If Me.txtPK_CompanyID.Text = 0 And iAccessLevel > 1 Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: You are not a Liaison for a Company with Productions that were Adjudicated."
            Me.pnlBallotInfo.Visible = False
            Me.pnlUserData.Visible = False
        Else
            If iAccessLevel = 1 Then
                chkShowAll.Visible = True
                chkShowAll.Checked = True
            End If

            If Not IsPostBack Then
                Call Populate_DropDowns()
            End If
            If Request.QueryString("Print") = "True" Then
                Call SetPrinterFriendlyLayout()
            End If
        End If

    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = " SELECT    Production.PK_ProductionID, Production.Title + ' - ' + ProductionType.ProductionType AS ProductionTitle " & _
         "			FROM   Production INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
         "			WHERE  (NOT (Production.Title LIKE 'TBD')) "

        If Me.chkShowAll.Checked = False Then
            sSQL = sSQL & "	   AND (Production.FK_CompanyID =" & Me.txtPK_CompanyID.Text & ")"
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.pnlUserData.Visible = True
            Me.ddlProductionID.DataSource = dt
            Me.ddlProductionID.DataValueField = "PK_ProductionID"
            Me.ddlProductionID.DataTextField = "ProductionTitle"
            Me.ddlProductionID.DataBind()

            If iProductionID > 0 Then
                Me.ddlProductionID.SelectedValue = iProductionID
            End If

            Call Populate_Data()
        Else
            Me.pnlUserData.Visible = False
            Me.lblErrors.Text = "ERROR: No Productions Found for your associated Theatre Company"
            Me.lblErrors.Visible = True
        End If

    End Sub

    Private Sub Populate_ProductionDetails(ByVal ProductionID As String)
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = " SELECT Production.PK_ProductionID, Production.FK_CompanyID, Production.FK_VenueID, Production.FK_AgeApproriateID, Production.FK_ProductionTypeID,  " & _
          "	    Company.CompanyName, ProductionCategory.ProductionCategory, Venue.VenueName, Venue.City, Venue.State, " & _
          "       AgeAppropriate.AgeAppropriateName, ProductionType.ProductionType,  " & _
          "	    Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.AllPerformanceDatesTimes,  " & _
          "	    Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, Production.TicketPurchaseDetails " & _
          "  FROM Production INNER JOIN " & _
          "	    Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
          "	    ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID INNER JOIN " & _
          "	    Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID INNER JOIN " & _
          "	    AgeAppropriate ON Production.FK_AgeApproriateID = AgeAppropriate.PK_AgeAppropriateID INNER JOIN " & _
          "	    ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
          " WHERE PK_ProductionID = " & ProductionID

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblTitle.Text = dt.Rows(0)("Title").ToString & " - " & dt.Rows(0)("ProductionType").ToString
            Me.lblProductionName.Text = dt.Rows(0)("Title").ToString & " - " & dt.Rows(0)("ProductionType").ToString
            Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("ProductionCategory").ToString
            Me.lblFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.lblLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            Me.lblVenueName.Text = dt.Rows(0)("VenueName").ToString & " in " & dt.Rows(0)("City").ToString & ", " & dt.Rows(0)("State").ToString
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable
        Dim sSQL As String
        Dim iCount As Int16, iDirScore As Int16 = 0
        '====================================================================================================
        Call Populate_ProductionDetails(Me.ddlProductionID.SelectedValue.ToString)

        sSQL = " SELECT PK_ScoringID, PK_NominationsID, FK_CompanyID_Adjudicator, FK_UserID_Adjudicator, " & _
          "		AdjudicatorRequestsReassignment, AdjudicatorScoringLocked, ProductionDateAdjudicated_Planned, " & _
          "		ProductionDateAdjudicated_Actual, DirectorScore, DirectorComment, MusicalDirectorScore, " & _
          "		MusicalDirectorComment, ChoreographerScore, ChoreographerComment, ScenicDesignerScore, " & _
          "		ScenicDesignerComment, LightingDesignerScore, LightingDesignerComment, SoundDesignerScore, " & _
          "		SoundDesignerComment, CostumeDesignerScore, CostumeDesignerComment, OriginalPlaywrightScore, " & _
          "		OriginalPlaywrightComment, BestPerformer1Score, BestPerformer1Comment, BestPerformer2Score, " & _
          "		BestPerformer2Comment, BestSupportingActor1Score, BestSupportingActor1Comment, " & _
          "		BestSupportingActor2Score, BestSupportingActor2Comment, BestSupportingActress1Score, " & _
          "		BestSupportingActress1Comment, BestSupportingActress2Score, BestSupportingActress2Comment, " & _
          "		Scoring.LastUpdateByName, Scoring.LastUpdateByDate, BallotSubmitDate, " & _
          " 		FK_ProductionID, Director, MusicalDirector, " & _
          "		Choreographer, ScenicDesigner, LightingDesigner, SoundDesigner, " & _
          "		CostumeDesigner, OriginalPlaywright, BestPerformer1Name, BestPerformer1Role, BestPerformer1Gender, " & _
          "		BestPerformer2Name, BestPerformer2Role, BestPerformer2Gender, BestSupportingActor1Name, BestSupportingActor1Role, " & _
          "		BestSupportingActor2Name, BestSupportingActor2Role, BestSupportingActress1Name, " & _
          "		BestSupportingActress1Role, BestSupportingActress2Name, BestSupportingActress2Role, " & _
          "     AdjudicatorAttendanceComment, BestProductionScore, BestProductionComment, " & _
          "		Scoring.CreateByDate " & _
          " FROM Scoring INNER JOIN Nominations ON Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
          " WHERE FK_ProductionID=" & Me.ddlProductionID.SelectedValue.ToString & _
          " ORDER BY Nominations.FK_ProductionID, (PK_ScoringID + FK_CompanyID_Adjudicator + FK_UserID_Adjudicator) "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            ' if total DirectorScore = 0 then no Scores have been submitted for this production
            For iCount = 0 To dt.Rows.Count - 1
                Try
                    If CInt(dt.Rows(iCount)("DirectorScore").ToString) > 0 Then iDirScore = iDirScore + 1
                    If CInt(dt.Rows(iCount)("BestPerformer1Score").ToString) > 0 Then iDirScore = iDirScore + 1
                    If CInt(dt.Rows(iCount)("BestPerformer2Score").ToString) > 0 Then iDirScore = iDirScore + 1
                    If CInt(dt.Rows(iCount)("BestSupportingActor1Score").ToString) > 0 Then iDirScore = iDirScore + 1
                    If CInt(dt.Rows(iCount)("BestSupportingActress1Score").ToString) > 0 Then iDirScore = iDirScore + 1
                Catch ex As Exception
                    ' no score value found... continue
                End Try
            Next

            If iDirScore = 0 Then
                Me.pnlUserData.Visible = False
                Me.lblErrors.Text = "ERROR: no Scores have been submitted for this Production."
                Me.lblErrors.Visible = True
            Else
                Me.pnlUserData.Visible = True
                Set_OtherComments("BestProductionComment", dt)
                Set_Comments("Director", "Director", dt)
                Set_Comments("MusicalDirector", "Musical Director", dt)
                Set_Comments("Choreographer", "Choreographer", dt)
                Set_Comments("ScenicDesigner", "Scenic Designer", dt)
                Set_Comments("LightingDesigner", "Lighting Designer", dt)
                Set_Comments("SoundDesigner", "Sound Designer", dt)
                Set_Comments("CostumeDesigner", "Costume Designer", dt)
                Set_Comments("OriginalPlaywright", "Original Playwright", dt)
                Set_Comments("BestPerformer1", "Best " & dt.Rows(0)("BestPerformer1Gender").ToString & " #1", dt)
                Set_Comments("BestPerformer2", "Best " & dt.Rows(0)("BestPerformer2Gender").ToString & " #1", dt)
                'Set_Comments("BestPerformer1", "Best " & dt.Rows(0)("BestPerformer1Gender").ToString & " #1", dt)
                Set_Comments("BestSupportingActor1", "Best Supporting Actor #1", dt)
                Set_Comments("BestSupportingActor2", "Best Supporting Actor #2", dt)
                Set_Comments("BestSupportingActress1", "Best Supporting Actress #1", dt)
                Set_Comments("BestSupportingActress2", "Best Supporting Actress #2", dt)
                Set_OtherComments("AdjudicatorAttendanceComment", dt)
            End If

        Else
            Me.pnlUserData.Visible = False
            Me.lblErrors.Text = "ERROR: no data found for this Production."
            Me.lblErrors.Visible = True
        End If

    End Sub

    Private Sub SetPrinterFriendlyLayout()
        'Me.pnlHeader.Visible = False
        'Me.pnlLeftNav.Visible = False
    End Sub

    Private Sub Set_OtherComments(ByVal CommentFieldName As String, ByVal dtbl As DataTable)
        '====================================================================================================
        Dim lbl As New Label, sComments As String = "", iCount As Integer
        '====================================================================================================
        'Place all Comments in single string, ending each comment with HTML <hr noshade>
        For iCount = dtbl.Rows.Count - 1 To 0 Step -1
            Try
                ' Only show comments if ballot was completed (which requires a BestProductionScore score)
                If CInt(dtbl.Rows(iCount)("BestProductionScore").ToString) > 0 Then
                    If dtbl.Rows(iCount)(CommentFieldName).ToString.Length > 0 Then
                        sComments = sComments & dtbl.Rows(iCount)(CommentFieldName).ToString() & "<hr noshade>"
                    Else
                        sComments = sComments & "<EM>Adjudicator did not provide a comment.</EM>" & "<hr noshade>"
                    End If
                End If

            Catch ex As Exception
                ' no comment value found... continue
            End Try
        Next

        If sComments.Length > 0 Then
            lbl = Nothing
            lbl = Me.FindControl("lbl" & CommentFieldName)       ' Label Comments
            lbl.Text = sComments
        End If

    End Sub

    Private Sub Set_Comments(ByVal ControlName As String, ByVal LabelName As String, ByVal dtbl As DataTable)
        '====================================================================================================
        Dim lbl As Label, sComments As String = "", iCount As Integer
        '====================================================================================================
        Try
            lbl = Me.FindControl("lbl" & ControlName)     ' Header Label Name
            'If this is a "Best" control, then reference the field correctly and add the Role nominated for
            If ControlName.Substring(0, 4).ToUpper = "BEST" Then
                lbl.Text = "<i>" & LabelName & ":</i> " & dtbl.Rows(0)(ControlName & "Name").ToString & " as " & dtbl.Rows(0)(ControlName & "Role").ToString
            Else
                lbl.Text = "<i>" & LabelName & ":</i> " & dtbl.Rows(0)(ControlName).ToString
            End If

            'Place all Comments in single string, ending each comment with HTML <hr noshade>
            For iCount = 0 To dtbl.Rows.Count - 1
                Try
                    ' Only show comments if ballot was completed (which requires a score)
                    If CInt(dtbl.Rows(iCount)(ControlName & "Score").ToString) > 0 Then
                        If dtbl.Rows(iCount)(ControlName & "Comment").ToString.Length > 0 Then
                            sComments = sComments & dtbl.Rows(iCount)(ControlName & "Comment").ToString() & "<hr noshade>"
                        Else
                            sComments = sComments & "<EM>Adjudicator did not provide comment.</EM>" & "<hr noshade>"
                        End If
                    End If
                Catch ex As Exception
                    ' no score value found... continue
                End Try
            Next

            If sComments.Length > 0 Then
                lbl = Nothing
                lbl = Me.FindControl("lbl" & ControlName & "Comment")      ' Label Comments
                lbl.Text = sComments
                ShowHide_Scoring(True, ControlName)
            Else
                ShowHide_Scoring(False, ControlName)
            End If

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
        End Try

    End Sub

    Private Sub ShowHide_Scoring(ByVal IsVisible As Boolean, ByVal ControlName As String)
        '====================================================================================================
        Dim lbl As Label
        '====================================================================================================
        Try
            lbl = Me.FindControl("lbl" & ControlName)      ' Header Label Name
            lbl.Visible = IsVisible
            lbl = Me.FindControl("lbl" & ControlName & "Comment")   ' Label Comments
            lbl.Visible = IsVisible

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EmailCompletedBallot()
        '====================================================================================================
        Dim sMsg As String = "", sSubject As String, sTo As String, sFrom As String
        '====================================================================================================
        Me.lblErrors.Visible = False
        Me.lblStatus.Visible = False

        If Session.Item("EmailPrimary").ToString.Length = 0 Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: No Primary Email Address found for ID: " & Session.Item("LoginID")
            Exit Sub
        End If

        sTo = Session.Item("EmailPrimary")
        sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString

        sMsg = sMsg & "<br /><b>Production:</b> " & Me.lblTitle.Text
        sMsg = sMsg & "<br /><b>Theatre Company:</b> " & Me.lblCompanyName.Text
        sMsg = sMsg & "<br /><b>Venue:</b> " & Me.lblVenueName.Text
        sMsg = sMsg & "<br /><b>Production Dates:</b> " & Me.lblFirstPerformanceDateTime.Text & " thru " & Me.lblLastPerformanceDateTime.Text

        sMsg = sMsg & "<br /><hr noshade>"

        sMsg = sMsg & GetText_ScoreComments("Director")
        sMsg = sMsg & GetText_ScoreComments("MusicalDirector")
        sMsg = sMsg & GetText_ScoreComments("Choreographer")
        sMsg = sMsg & GetText_ScoreComments("ScenicDesigner")
        sMsg = sMsg & GetText_ScoreComments("LightingDesigner")
        sMsg = sMsg & GetText_ScoreComments("SoundDesigner")
        sMsg = sMsg & GetText_ScoreComments("CostumeDesigner")
        sMsg = sMsg & GetText_ScoreComments("OriginalPlaywright")
        sMsg = sMsg & GetText_ScoreComments("BestPerformer1")
        sMsg = sMsg & GetText_ScoreComments("BestPerformer2")
        sMsg = sMsg & GetText_ScoreComments("BestSupportingActor1")
        sMsg = sMsg & GetText_ScoreComments("BestSupportingActor2")
        sMsg = sMsg & GetText_ScoreComments("BestSupportingActress1")
        sMsg = sMsg & GetText_ScoreComments("BestSupportingActress2")
        sMsg = sMsg & "<br /><hr noshade>"

        Try
            sSubject = "NHTA Comments for: " & Me.lblTitle.Text

            'Send the Email in HTML Format
            CustomMail.SendCDOEmail(sFrom, sTo, False, sSubject, sMsg, False, True, Session("LoginID"))

            Me.lblStatus.Visible = True
            Me.lblStatus.Text = "<b>Sucessfully Emailed Comments To:</b> " & sTo & "<br /> for the production <b>" & Me.lblTitle.Text & "</b><br /><hr noshade>" ' & sMsg

        Catch ex As Exception
            'Throw
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Sub

    Private Function GetText_ScoreComments(ByVal ControlName As String) As String
        '====================================================================================================
        Dim lbl As Label, lblName As Label
        Dim sInfo As String = Nothing
        '====================================================================================================
        Try
            lblName = Me.FindControl("lbl" & ControlName)               ' Name Label

            If lblName.Visible = True Then
                lbl = Me.FindControl("lbl" & ControlName & "Comment")       ' Label Comments

                sInfo = "<b>Category:</b> " & lblName.Text.Split(":")(0) & "  <b>Nominee:</b> " & lblName.Text.Split(":")(1) & _
                "<br /><b>Comments:</b><br /> " & lbl.Text
            End If

            Return sInfo

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub btnPrinterFriendly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrinterFriendly.Click
        Dim ShowAll As String = IIf(Me.chkShowAll.Checked = True, "&ShowAll=True", "")
        Response.Write("<script language=javascript>")
        Response.Write("window.open('AdjudicatorComments.aspx?Print=True" & ShowAll & "&ProductionID=" & Me.ddlProductionID.SelectedValue.ToString & "','Microscript','status=yes ,toolbar=yes,directories=no,menubar=yes,scrollbars=yes,resizable=yes,location=no,top=50,left=250,height=800,width=800');")
        Response.Write("</script>")
    End Sub

    Private Sub ddlProductionID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProductionID.SelectedIndexChanged
        Me.lblErrors.Visible = False
        Me.lblStatus.Visible = False
        Call Populate_Data()
    End Sub

    Private Sub btnEmailComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmailComments.Click
        Me.lblErrors.Visible = False
        Call EmailCompletedBallot()
    End Sub

    Private Sub chkShowAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAll.CheckedChanged
        Me.lblErrors.Visible = False
        Me.lblStatus.Visible = False
        Call Populate_DropDowns()
    End Sub

End Class