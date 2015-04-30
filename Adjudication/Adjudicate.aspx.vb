Imports Adjudication.DataAccess
Imports Adjudication.Common

Partial Public Class Adjudicate
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iProductionID As Integer
    Dim iUserLoginID As Integer
    Dim sFormAction As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1 Or iAccessLevel = 2 Or iAccessLevel = 4 Or iAccessLevel = 5) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        'Set the Action for this Form
        If Not Request.QueryString("Action") Is Nothing Then sFormAction = Request.QueryString("Action")

        If Not IsPostBack Then

            Call Populate_DropDownLists()

            Dim dt As New DataTable
            dt = DataAccess.Find_AdjudicationsForUserLoginID(sLoginID) ' Find the Productions For this Adjudicator  

            gridMain.DataSource = dt
            gridMain.DataBind()
            lblTotalNumberOfRecords.Text = "Number of Adjudications: " & dt.Rows.Count.ToString

            If dt.Rows.Count = 0 Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "You have not been Assigned any Productions to Adjudicate"
            End If
        End If
    End Sub

    Private Sub Populate_ProductionDetails(ByVal ProductionID As String)
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Production(ProductionID)

        If dt.Rows.Count > 0 Then
            Me.lblTitle.Text = dt.Rows(0)("Title").ToString & " - " & dt.Rows(0)("ProductionType").ToString
            Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("ProductionCategory").ToString
            Me.CompanyPhone.Text = dt.Rows(0)("CompanyPhone").ToString
            If dt.Rows(0)("CompanyEmailAddress").ToString.Length > 9 Then Me.CompanyEmailAddress.Text = "<a href=""mailto:" & dt.Rows(0)("CompanyEmailAddress").ToString & """>" & dt.Rows(0)("CompanyEmailAddress").ToString & "</a>"
            If dt.Rows(0)("CompanyWebsite").ToString.Length > 5 Then Me.CompanyWebsite.Text = "<a href=""HTTP://" & dt.Rows(0)("CompanyWebsite").ToString & """>" & dt.Rows(0)("CompanyWebsite").ToString & "</a>"
            Me.lblFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.lblLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            Me.lblVenueName.Text = dt.Rows(0)("VenueName").ToString & " in " & dt.Rows(0)("City").ToString & ", " & dt.Rows(0)("State").ToString
            Me.lblAllPerformanceDatesTimes.Text = dt.Rows(0)("AllPerformanceDatesTimes").ToString
            Me.lblAgeAppropriateName.Text = dt.Rows(0)("AgeAppropriateName").ToString
            Me.lblTicketContactName.Text = dt.Rows(0)("TicketContactName").ToString
            Me.lblTicketContactPhone.Text = dt.Rows(0)("TicketContactPhone").ToString
            Me.lblTicketContactEmail.Text = dt.Rows(0)("TicketContactEmail").ToString
            Me.lblTicketPurchaseDetails.Text = dt.Rows(0)("TicketPurchaseDetails").ToString
            Me.txtFK_VenueID.Text = dt.Rows(0)("FK_VenueID").ToString
        End If

    End Sub

    Private Sub Populate_DropDownLists()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = Get_ScoringStatus()
        'If sFormAction = "Reassign" Then
        '    dt = Get_ScoringStatus()
        'Else
        '    dt = Get_ScoringStatus_Selections()
        'End If

        If dt.Rows.Count > 0 Then
            Me.ddlPK_ScoringStatusID.DataSource = dt
            Me.ddlPK_ScoringStatusID.DataValueField = "PK_ScoringStatusID"
            Me.ddlPK_ScoringStatusID.DataTextField = "ScoringStatus"
            Me.ddlPK_ScoringStatusID.DataBind()
        End If
    End Sub


    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        Dim iDaysToConfirmAttendance As Int16, iDaysToWaitForScoring As Int16
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then      ' check if row is a Detail Row
            Dim oImgBtn As Button

            Select Case sFormAction.ToUpper
                Case "CONFIRM"
                    oImgBtn = e.Item.FindControl("ibtnConfirm")
                    oImgBtn.Visible = True

                Case "REASSIGN"
                    oImgBtn = e.Item.FindControl("ibtnReassign")
                    oImgBtn.Visible = True

                Case "SCORE"
                    oImgBtn = e.Item.FindControl("ibtnBallot")
                    oImgBtn.Visible = True

                Case "PRINT"
                    oImgBtn = e.Item.FindControl("ibtnPrint")
                    oImgBtn.Visible = True

                Case Else
                    'if no action matches, make all buttons visible
                    oImgBtn = e.Item.FindControl("ibtnConfirm")
                    oImgBtn.Visible = True
                    oImgBtn = Nothing
                    oImgBtn = e.Item.FindControl("ibtnReassign")
                    oImgBtn.Visible = True
                    oImgBtn = Nothing
                    oImgBtn = e.Item.FindControl("ibtnBallot")
                    oImgBtn.Visible = True
                    oImgBtn = Nothing
                    oImgBtn = e.Item.FindControl("ibtnPrint")
                    oImgBtn.Visible = True
            End Select


            'sFormAction
            If e.Item.Cells(15).Text.Length > 0 Then
                Dim PrintButton As Button = e.Item.FindControl("ibtnPrint")
                Dim OpenWindowScript As String = "printOpen(" & e.Item.Cells(15).Text & ");"
                PrintButton.Attributes.Add("onclick", "javascript:" & OpenWindowScript)
            End If

            'If e.Item.Cells(3).Text.ToUpper = "NO" Then
            '    iDaysToConfirmAttendance = CInt(e.Item.Cells(19).Text)

            '    If Today.Date > CDate(e.Item.Cells(9).Text).AddDays(iDaysToConfirmAttendance) Then
            '        e.Item.Cells(3).ForeColor = System.Drawing.Color.Red
            '        e.Item.Cells(3).Font.Bold = True
            '    End If
            'End If

            If e.Item.Cells(3).Text = "0" Then
                iDaysToWaitForScoring = CInt(e.Item.Cells(20).Text)
                If Today.Date > CDate(e.Item.Cells(9).Text).AddDays(iDaysToWaitForScoring) Then
                    e.Item.Cells(3).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(3).Font.Bold = True
                    e.Item.Cells(3).Text = "LATE"
                Else
                    If Today.Date > CDate(e.Item.Cells(9).Text) Then
                        e.Item.Cells(3).Text = "Required"
                        e.Item.Cells(3).Font.Bold = True
                    Else
                        e.Item.Cells(3).Text = ""
                    End If
                End If
            Else
                e.Item.Cells(3).Text = "Submitted"
            End If
        End If

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        '====================================================================================================
        Dim sNote As String()
        '====================================================================================================
        Me.txtPK_ScoringID.Text = e.Item.Cells(15).Text
        Me.lblTotalScore.Text = e.Item.Cells(24).Text
        Me.divSucessfulUpdate.Visible = False
        Me.lblErrors.Visible = False

        Select Case CType(e.CommandSource, Button).CommandName
            Case "Confirm_Command" '====================================================================================================
                Call Populate_ProductionDetails(e.Item.Cells(26).Text)

                Me.pnlGrid.Visible = False
                Me.pnlSelectStatus.Visible = True
                Me.pnlAdminInfo.Visible = True
                Me.pnlReassignmentRequest.Visible = False
                Me.btnUpdate.Visible = True

                Me.pnlSelectedProductionDetail.Visible = True
                Me.lblLastUpdateByName.Text = e.Item.Cells(13).Text
                Me.lblLastUpdateByDate.Text = e.Item.Cells(14).Text
                'Me.ddlAdjudicatorRequestsReassignment.SelectedValue = e.Item.Cells(17).Text
                If Not e.Item.Cells(21).Text = "&nbsp;" Then
                    sNote = e.Item.Cells(21).Text.Split("<br><br>")
                    Me.txtAdjudicatorRequestsReassignmentNote.Text = sNote(0)
                End If

                Me.txtProductionDateAdjudicated_Planned.Enabled = True
                If e.Item.Cells(22).Text = "&nbsp;" Then    ' BallotSubmitDate
                    Me.txtBallotSubmitDate.Text = ""
                Else    ' IF Ballot has been submitted, dont allow entry of a PLANNED date
                    Me.txtBallotSubmitDate.Text = CDate(e.Item.Cells(22).Text).ToShortDateString
                    Me.txtProductionDateAdjudicated_Planned.Enabled = False
                    Me.btnUpdate.Visible = False
                End If

                If e.Item.Cells(16).Text = "&nbsp;" Then    ' ProductionDateAdjudicated_Planned
                    Me.txtProductionDateAdjudicated_Planned.Text = ""
                Else
                    Me.txtProductionDateAdjudicated_Planned.Text = CDate(e.Item.Cells(16).Text).ToShortDateString
                End If

                Me.ddlPK_ScoringStatusID.SelectedValue = e.Item.Cells(25).Text
                If Me.ddlPK_ScoringStatusID.SelectedValue = 2 Then
                    Me.trConfirmAttendanceDate.Visible = True
                    'set the Date selection control to have a jQueryUI calendar picker
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "DatePickerScript", "$('.date-picker').datepicker(); $('#FormSiteMaster').bootstrapValidator();", True)
                End If

            Case "Reassign_Command" '====================================================================================================
                Call Populate_ProductionDetails(e.Item.Cells(26).Text)

                Me.pnlGrid.Visible = False
                Me.pnlSelectStatus.Visible = False
                Me.pnlAdminInfo.Visible = True
                Me.pnlReassignmentRequest.Visible = True

                Me.pnlSelectedProductionDetail.Visible = True
                Me.lblLastUpdateByName.Text = e.Item.Cells(13).Text
                Me.lblLastUpdateByDate.Text = e.Item.Cells(14).Text
                'Me.ddlAdjudicatorRequestsReassignment.SelectedValue = e.Item.Cells(17).Text

                If Not e.Item.Cells(21).Text = "&nbsp;" Then                                    'AdjudicatorRequestsReassignmentNote is not null
                    Me.ddlHasReplacement.SelectedValue = 0
                    Dim StatusText = e.Item.Cells(21).Text

                    If InStr(StatusText, "Replacement Adjudicator Name") Then                       'just ugly...
                        Me.ddlHasReplacement.SelectedValue = "1"
                        StatusText = StatusText.Replace("<b>", "~~")                                'clear the hardcoded BOLD html tag - replace with String to .Split on
                        StatusText = StatusText.Replace("</b>", "")                                 'clear the hardcoded BOLD html tag
                    End If
                    If InStr(StatusText, "Does NOT have a Replacement Adjudicator") Then Me.ddlHasReplacement.SelectedValue = "2"
                    If InStr(StatusText, "Did not try to find one in Theatre Company") Then Me.ddlHasReplacement.SelectedValue = "3"

                    sNote = Regex.Split(StatusText, "<br><br>")                                     'Split on hard coded (on save) HTML breaks 
                    If sNote.Count > 0 Then
                        Me.txtAdjudicatorRequestsReassignmentNote.Text = sNote(0)                   'The note from Adjudicator (drops the hard coded text)
                        If Me.ddlHasReplacement.SelectedValue = "1" Then
                            Me.txtReplacementName.Text = Regex.Split(sNote(1), "~~")(1)
                            Me.trReplacementAdjudicatorName.Visible = True
                        Else
                            Me.txtReplacementName.Text = String.Empty
                            Me.trReplacementAdjudicatorName.Visible = False
                        End If
                    End If
                End If

                If Not (e.Item.Cells(22).Text = "&nbsp;") Or Today >= CDate(Me.lblLastPerformanceDateTime.Text) Or CInt(Me.lblTotalScore.Text) > 0 Then     ' BallotSubmitDate
                    ' IF Ballot has been submitted, OR After Production Close Date - dont allow Reassignment Request
                    Me.pnlReassignmentRequestControls.Visible = False
                    Me.btnUpdate.Visible = False
                    Me.lblCannotRequestReassignment.Visible = True
                    If CInt(Me.lblTotalScore.Text) > 0 Then
                        Me.lblCannotRequestReassignment.Text = "Error: You cannot make a Reassignment Request after you have submitted a Score."
                    Else
                        Me.lblCannotRequestReassignment.Text = "ERROR: You cannot request a Reassignment after the Production closes."
                    End If

                Else
                    Me.lblCannotRequestReassignment.Visible = False
                    Me.pnlReassignmentRequestControls.Visible = True
                    Me.btnUpdate.Visible = True
                End If

                Me.ddlPK_ScoringStatusID.SelectedValue = "6"           ' 6= Reassignment status


            Case "ScoreBallot_Command" '====================================================================================================
                Response.Redirect("Ballot.aspx?ScoringID=" & e.Item.Cells(15).Text)

            Case "Print_Command" '====================================================================================================
                'Response.Redirect("BallotSummary.aspx?Print=True&ScoringID=" & e.Item.Cells(15).Text)
                'Dim OpenWindowScript As String = "<script language='javascript'>function printOpen() {window.open('BallotSummary.aspx?Print=True&ScoringID=" & e.Item.Cells(15).Text & "', 'NH Theatre Award Ballot', 'height=800,width=680,toolbar=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=no,modal=yes');}</script>"
                'Page.RegisterStartupScript("Startup", OpenWindowScript)

            Case "Title_Command" '====================================================================================================
                Me.pnlGrid.Visible = True
                Me.pnlSelectStatus.Visible = False
                Me.pnlAdminInfo.Visible = False
                Me.pnlReassignmentRequest.Visible = False

                Me.pnlSelectedProductionDetail.Visible = True

                Call Populate_ProductionDetails(e.Item.Cells(26).Text)

            Case Else
                ' break 
        End Select
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sSQL As String
        Dim DateTester As Date
        Dim sNote As String = ""
        '====================================================================================================
        Me.lblErrors.Visible = False     ' reset error msg
        Me.divSucessfulUpdate.Visible = False

        '=== Create Note for reassignment, including the name of replacement (if available)
        Select Case Me.ddlHasReplacement.SelectedValue
            Case "1"
                sNote = Common.RemoveInvalidSQLCharacters(Me.txtAdjudicatorRequestsReassignmentNote.Text & " <br><br>Replacement Adjudicator Name: <b>" & Me.txtReplacementName.Text & "</b>")
            Case "2"
                sNote = Common.RemoveInvalidSQLCharacters(Me.txtAdjudicatorRequestsReassignmentNote.Text & " <br><br>Does NOT have a Replacement Adjudicator")
            Case "3"
                sNote = Common.RemoveInvalidSQLCharacters(Me.txtAdjudicatorRequestsReassignmentNote.Text & " <br><br>Did not try to find one in Theatre Company")
            Case Else
                sNote = Common.RemoveInvalidSQLCharacters(Me.txtAdjudicatorRequestsReassignmentNote.Text)
        End Select

        If Me.pnlReassignmentRequest.Visible = True Then
            ' UPDATE for REASSIGNMENT
            sSQL = "UPDATE Scoring SET AdjudicatorRequestsReassignment=1," & _
              "			AdjudicatorRequestsReassignmentNote='" & sNote & "', " & _
              "			ProductionDateAdjudicated_Planned=NULL, " & _
              "			FK_ScoringStatusID= " & Me.ddlPK_ScoringStatusID.SelectedValue.ToString & ", " & _
              "			LastUpdateByName='" & Me.sLoginID & "', LastUpdateByDate=GetDate() " & _
              "		WHERE PK_ScoringID=" & Me.txtPK_ScoringID.Text

        Else
            Try
                Select Case Me.ddlPK_ScoringStatusID.SelectedValue.ToString
                    Case "2"
                        'For any other Status Update
                        DateTester = (CDate(txtProductionDateAdjudicated_Planned.Text))
                        ' For confirming Attendance Date
                        sSQL = "UPDATE Scoring SET AdjudicatorRequestsReassignment=0, AdjudicatorRequestsReassignmentNote=NULL, " & _
                                "			ProductionDateAdjudicated_Planned=CAST('" & Me.txtProductionDateAdjudicated_Planned.Text & "' as SMALLDATETIME), " & _
                                "			FK_ScoringStatusID= " & Me.ddlPK_ScoringStatusID.SelectedValue.ToString & ", " & _
                                "			LastUpdateByName='" & Me.sLoginID & "', LastUpdateByDate= GetDate() " & _
                                "		WHERE PK_ScoringID=" & Me.txtPK_ScoringID.Text

                    Case "4"
                        If Me.txtAdjudicatorAttendanceComment.Text.Length < 9 Then
                            Me.lblErrors.Visible = True
                            Me.lblErrors.Text = "ERROR: Please provide a reason you did not attend the Production."
                            Exit Sub
                        End If

                        'UPDATE on FK_ScoringStatusID and AdjudicatorAttendanceComment
                        sSQL = "UPDATE Scoring SET FK_ScoringStatusID = " & Me.ddlPK_ScoringStatusID.SelectedValue.ToString & ", " & _
                                "			AdjudicatorAttendanceComment='" & Common.RemoveInvalidSQLCharacters(Me.txtAdjudicatorAttendanceComment.Text) & "', " & _
                                "			LastUpdateByName='" & Me.sLoginID & "', LastUpdateByDate= GetDate() " & _
                                "		WHERE PK_ScoringID=" & Me.txtPK_ScoringID.Text
                    Case "5"
                        If Me.lblTotalScore.Text = "0" Then
                            Me.lblErrors.Visible = True
                            Me.lblErrors.Text = "ERROR: You cannot select a 'Ballot Completed' status if the Ballot score is Zero."
                            Exit Sub
                        Else
                            'UPDATE on FK_ScoringStatusID only
                            sSQL = "UPDATE Scoring SET FK_ScoringStatusID = " & Me.ddlPK_ScoringStatusID.SelectedValue.ToString & ", " & _
                                    "			LastUpdateByName='" & Me.sLoginID & "', LastUpdateByDate= GetDate() " & _
                                    "		WHERE PK_ScoringID=" & Me.txtPK_ScoringID.Text
                        End If

                    Case "6"
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "ERROR: You cannot select a 'Pending Reassignment' status from this screen.  Please go back and select the 'Reassign' button."
                        Exit Sub

                    Case Else
                        'UPDATE for FK_ScoringStatusID only
                        sSQL = "UPDATE Scoring SET FK_ScoringStatusID = " & Me.ddlPK_ScoringStatusID.SelectedValue.ToString & ", " & _
                                "			LastUpdateByName='" & Me.sLoginID & "', LastUpdateByDate= GetDate() " & _
                                "		WHERE PK_ScoringID=" & Me.txtPK_ScoringID.Text
                End Select


            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Please provide a VALID Planned Adjudication Date value."
                Exit Sub
            End Try
        End If

        Call SQLUpdate(sSQL)

        Me.pnlGrid.Visible = True
        Me.pnlSelectStatus.Visible = False
        Me.pnlAdminInfo.Visible = False
        Me.pnlReassignmentRequest.Visible = False
        Me.pnlSelectedProductionDetail.Visible = False

        Dim dt As New DataTable
        dt = DataAccess.Find_AdjudicationsForUserLoginID(sLoginID)    ' Find the Productions For this User
        gridMain.DataSource = dt
        gridMain.DataBind()

        Me.divSucessfulUpdate.Visible = True

    End Sub


    Private Sub ddlPK_ScoringStatusID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPK_ScoringStatusID.SelectedIndexChanged
        Me.lblErrors.Visible = False     ' reset error msg
        Me.divSucessfulUpdate.Visible = False

        Me.trConfirmAttendanceDate.Visible = False
        Me.trDidNotAttend.Visible = False
        Me.pnlReassignmentRequest.Visible = False

        Select Case Me.ddlPK_ScoringStatusID.SelectedValue.ToString
            Case "2"
                Me.trConfirmAttendanceDate.Visible = True
                'set the Date selection control to have a jQueryUI calendar picker
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "DatePickerScript", "$('.date-picker').datepicker(); $('#FormSiteMaster').bootstrapValidator();", True)

            Case "4"
                Me.trDidNotAttend.Visible = True
            Case "5"
                If Me.lblTotalScore.Text = "0" Then
                    Me.lblErrors.Visible = True
                    Me.lblErrors.Text = "ERROR: You cannot select a 'Ballot Completed' status if the Ballot score is Zero."
                End If
            Case "6"
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: You cannot select a 'Pending Reassignment' status from this screen.  Please go back and select the 'Reassign' button."
                'Me.pnlReassignmentRequest.Visible = True
            Case Else
                Me.trConfirmAttendanceDate.Visible = False
                Me.trDidNotAttend.Visible = False
                Me.pnlReassignmentRequest.Visible = False
        End Select

    End Sub

    Private Sub lbtnViewVenue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnViewVenue.Click
        Response.Redirect("AdminVenue.aspx?ViewOnly=True&VenueID=" & Me.txtFK_VenueID.Text)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If sFormAction.Length > 0 Then
            Response.Redirect("Adjudicate.aspx?Action=" & sFormAction)
        Else
            Response.Redirect("Adjudicate.aspx")
        End If
    End Sub

    Protected Sub ddlHasReplacement_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlHasReplacement.SelectedIndexChanged
        If Me.ddlHasReplacement.SelectedValue = 1 Then
            Me.trReplacementAdjudicatorName.Visible = True
        Else
            Me.trReplacementAdjudicatorName.Visible = False
            Me.txtReplacementName.Text = String.Empty
        End If
    End Sub
End Class