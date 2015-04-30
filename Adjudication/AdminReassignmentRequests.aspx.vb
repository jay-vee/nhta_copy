Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CustomMail

Partial Public Class AdminReassignmentRequests
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sCompanyID As String, sLoginID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
            Call Populate_DropDowns()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================

        sSQL = "SELECT Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator, Nominations.PK_NominationsID, " & _
         "       Company.PK_CompanyID, Production.FK_VenueID, Scoring.AdjudicatorRequestsReassignment, Production.Title, " & _
         "       Company.CompanyName, ProductionCategory.ProductionCategory, Company.EmailAddress as CompanyEmailAddress, Company.Website as CompanyWebsite, " & _
         "       Users.LastName + ', ' + Users.FirstName AS FullName, Users.EmailPrimary, Users.PhonePrimary, Scoring.ProductionDateAdjudicated_Planned, " & _
         "       AdjudicatorRequestsReassignment, AdjudicatorRequestsReassignmentNote, BallotSubmitDate, " & _
         "		 CompanyScoring.CompanyName AS Scoring_CompanyName,  " & _
         "       Venue.VenueName, Venue.Address, Venue.State, Venue.City, Venue.Zip, Venue.Website, " & _
         "       Production.PK_ProductionID, Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, " & _
         "       Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
         "		 Scoring.ReserveAdjudicator, Scoring.LastUpdateByName, Scoring.LastUpdateByDate, Scoring.CreateByName, Scoring.CreateByDate " & _
         " FROM  Production INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
         "       INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
         "       INNER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID " & _
         "       INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
         "       LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
         "		 INNER JOIN Company CompanyScoring ON Scoring.FK_CompanyID_Adjudicator = CompanyScoring.PK_CompanyID " & _
         "		 INNER JOIN Venue ON Production.FK_VenueID = Venue.PK_VenueID " & _
         " WHERE AdjudicatorRequestsReassignment = 1 "

        If Not (txtSortColumnName.Text = "") And Not (txtSortColumnName.Text = "CompanyName") Then
            sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text + ", Company.CompanyName "
        Else          '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY Users.LastName, Users.FirstName, Production.Title, Company.CompanyName "
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        gridMain.DataSource = dt
        gridMain.DataBind()

        lblTotalNumberOfRecords.Text = "Number of Reassignment Requests: " & dt.Rows.Count.ToString

    End Sub

    Private Sub Populate_AssignAdjudicatorData()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        ' Listing of Adjudicators that 
        ' - EXCLUDEs Adjudicators from the Company producing the Production to be Adjudicated
        ' - EXCLUDEs Assigned Adjudicators already assigned for ANY Production from the Producing Company
        ' - EXCLUDEs Adjudicators with Old Training Dates 
        '====================================================================================================
        dt = DataAccess.Get_AvailableAdjudicatorsForProduction(Me.lblValidTrainingStartDate.Text, _
                                                                Me.chkShowAssignmentCounts.Checked, _
                                                                Me.txtPK_NominationID.Text, _
                                                                Me.chkIncludeBackupAdjudicators.Checked, _
                                                                Me.txtFK_CompanyID.Text, _
                                                                Me.chkShowCompanyName.Checked, True, True, True, Me.txtFK_CompanyID_Adjudicator.Text)

        If dt.Rows.Count > 0 Then
            ddlPK_UserID.DataSource = dt
            ddlPK_UserID.DataValueField = "PK_UserID"
            ddlPK_UserID.DataTextField = "Fullname"
            ddlPK_UserID.DataBind()
        End If

        '====================================================================================================
        ' List representing Companies 
        ' - that does NOT include the Company producing the Production to be Adjudicated
        '====================================================================================================
        sSQL = "SELECT PK_CompanyID, CompanyName FROM Company " & _
          " WHERE PK_CompanyID <> " & Me.txtFK_CompanyID.Text & _
          " ORDER BY CompanyName"

        dt = New DataTable
        dt = DataAccess.Run_SQL_Query(sSQL, True)

        If dt.Rows.Count > 0 Then
            ddlFK_CompanyID.DataSource = dt
            ddlFK_CompanyID.DataValueField = "PK_CompanyID"
            ddlFK_CompanyID.DataTextField = "CompanyName"
            ddlFK_CompanyID.DataBind()
        End If

    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_ApplicationDefaults

        If dt.Rows.Count > 0 Then
            Try
                lblNumAdjudicatorsPerShow.Text = dt.Rows(0)("NumAdjudicatorsPerShow").ToString
                lblMaxShowsPerAdjudicator.Text = dt.Rows(0)("MaxShowsPerAdjudicator").ToString
                lblValidTrainingStartDate.Text = CDate(dt.Rows(0)("ValidTrainingStartDate").ToString).ToShortDateString
            Catch ex As Exception
                Me.lblErrors.Text = "ERROR: " & ex.Message
                Me.lblErrors.Visible = True
                Exit Sub
            End Try
        End If

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"

                Me.txtPK_ScoringID.Text = e.Item.Cells(0).Text
                Me.txtPK_ProductionID.Text = e.Item.Cells(30).Text
                Me.lblCurrentAdjudicator.Text = e.Item.Cells(2).Text
                Me.lblCurrentRepresentedCompany.Text = e.Item.Cells(3).Text
                Me.lblTitle.Text = e.Item.Cells(4).Text
                Me.lblCompanyName.Text = e.Item.Cells(5).Text
                Me.lblCompanyEmailAddress.Text = e.Item.Cells(29).Text
                Me.hlnkCompanyWebsite.Text = Common.FormatHyperLink(e.Item.Cells(28).Text)
                Me.hlnkCompanyWebsite.NavigateUrl = Me.hlnkCompanyWebsite.Text

                Me.txtFK_UserID_Adjudicator.Text = e.Item.Cells(9).Text         'ADJUDICATOR TO REPLACE
                Me.txtFK_CompanyID_Adjudicator.Text = e.Item.Cells(10).Text     'COMPANY OF ADJUDICATOR TO REPLACE
                Me.lblFirstPerformanceDateTime.Text = CDate(e.Item.Cells(7).Text).ToShortDateString
                Me.lblLastPerformanceDateTime.Text = CDate(e.Item.Cells(8).Text).ToShortDateString
                Me.txtFK_CompanyID.Text = e.Item.Cells(12).Text         'Producing Company ID
                Me.lblLastUpdateByName.Text = e.Item.Cells(13).Text
                Me.lblLastUpdateByDate.Text = e.Item.Cells(14).Text
                Me.txtFK_CompanyID.Text = e.Item.Cells(12).Text
                Me.txtPK_NominationID.Text = e.Item.Cells(17).Text
                Me.lblAdjudicatorRequestsReassignmentNote.Text = e.Item.Cells(18).Text

                Me.lblVenueName.Text = e.Item.Cells(11).Text
                Me.lblAddress.Text = e.Item.Cells(20).Text
                Me.lblCity.Text = e.Item.Cells(21).Text
                Me.lblState.Text = e.Item.Cells(22).Text
                Me.lblZIP.Text = e.Item.Cells(23).Text
                Me.hlnkWebsite.Text = Common.FormatHyperLink(e.Item.Cells(24).Text)
                Me.hlnkWebsite.NavigateUrl = Me.hlnkWebsite.Text
                Me.lblTicketContactName.Text = e.Item.Cells(25).Text
                Me.lblTicketContactPhone.Text = e.Item.Cells(26).Text
                Me.lblTicketContactEmail.Text = e.Item.Cells(27).Text


                Call Populate_AssignAdjudicatorData()

                Me.ddlReserveAdjudicator.SelectedValue = e.Item.Cells(31).Text  ' 0=No; 1=Yes
                Me.lblReserveAdjudicator.Text = Me.ddlReserveAdjudicator.SelectedItem.ToString
                Me.ddlFK_CompanyID.SelectedValue = e.Item.Cells(10).Text        ' Default as the Same Company Representing

                Me.pnlGrid.Visible = False
                Me.pnlReassignAdjudicator.Visible = True

            Case Else
                ' break 
        End Select
    End Sub

    Sub gridMain_SortCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gridMain.SortCommand

        If txtSortColumnName.Text = e.SortExpression Then
            If txtSortOrder.Text = " DESC " Then
                txtSortOrder.Text = ""
            Else
                txtSortOrder.Text = " DESC "
            End If
        Else
            txtSortOrder.Text = ""
        End If

        txtSortColumnName.Text = e.SortExpression

        Populate_DataGrid()
    End Sub

    Private Sub chkShowAssignmentCounts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAssignmentCounts.CheckedChanged
        Call Populate_AssignAdjudicatorData()
    End Sub

    Private Sub chkIncludeBackupAdjudicators_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIncludeBackupAdjudicators.CheckedChanged
        Call Populate_AssignAdjudicatorData()
    End Sub

    Private Sub chkShowCompanyName_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowCompanyName.CheckedChanged
        Call Populate_AssignAdjudicatorData()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(50) As String
        '====================================================================================================
        Me.lblErrors.Visible = False     ' reset error msg
        Me.lblSuccessful.Visible = False

        If Me.ddlPK_UserID.SelectedIndex = 0 Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please select an Adjudicator to assign."
            Exit Sub
        End If

        If Me.ddlFK_CompanyID.SelectedIndex = 0 Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please select a Company that will be Adjudicating."
            Exit Sub
        End If

        sDataValues(1) = Me.txtPK_ScoringID.Text
        sDataValues(2) = Me.txtPK_NominationID.Text
        sDataValues(3) = Me.ddlFK_CompanyID.SelectedValue
        sDataValues(4) = Me.ddlPK_UserID.SelectedValue
        sDataValues(5) = "0"
        sDataValues(6) = "0"     ' AdjudicatorScoringLocked
        sDataValues(7) = ""
        sDataValues(8) = ""
        sDataValues(9) = "0"
        sDataValues(10) = ""
        sDataValues(11) = "0"
        sDataValues(12) = ""
        sDataValues(13) = "0"
        sDataValues(14) = ""
        sDataValues(15) = "0"
        sDataValues(16) = ""
        sDataValues(17) = "0"
        sDataValues(18) = ""
        sDataValues(19) = "0"
        sDataValues(20) = ""
        sDataValues(21) = "0"
        sDataValues(22) = ""
        sDataValues(23) = "0"
        sDataValues(24) = ""
        sDataValues(25) = "0"
        sDataValues(26) = ""
        sDataValues(27) = "0"
        sDataValues(28) = ""
        sDataValues(29) = "0"
        sDataValues(30) = ""
        sDataValues(31) = "0"
        sDataValues(32) = ""
        sDataValues(33) = "0"
        sDataValues(34) = ""
        sDataValues(35) = "0"
        sDataValues(36) = ""
        sDataValues(37) = "0"
        sDataValues(38) = ""
        sDataValues(39) = sLoginID
        sDataValues(40) = ""        ' BallotSubmitDate
        sDataValues(41) = ""        ' AdjudicatorRequestsReassignmentNote: Upon Sumbission, reset any such requests
        sDataValues(42) = Me.ddlReserveAdjudicator.SelectedValue
        sDataValues(43) = 1         ' Reset Status @FK_ScoringStatusID
        sDataValues(44) = ""        ' @AdjudicatorAttendanceComment
        sDataValues(45) = 1         ' Needed for Update without Deleting any Submitted scoring


        If Save_Scoring(sDataValues) = True Then
            'Send Email if option selected
            If rblEmailInfo.SelectedIndex = 1 Then Call Email_AdjudicatorAssignment(Me.txtAdminEmailComments_ReAssign.Text)

            Me.pnlGrid.Visible = True
            Me.pnlReassignAdjudicator.Visible = False
            Me.lblSuccessful.Visible = True
            Call Populate_DataGrid()
        Else
            Me.lblErrors.Text = "ERROR: Saving Scoring Data"
            Me.lblErrors.Visible = True
        End If

    End Sub

    Private Sub ddlPK_UserID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPK_UserID.SelectedIndexChanged
        '====================================================================================================
        Dim dt As New DataTable
        '====================================================================================================
        dt = Get_UserRecord(Me.ddlPK_UserID.SelectedValue.ToString)

        'Set Company to the selected users company
        Me.ddlFK_CompanyID.SelectedValue = dt.Rows(0)("FK_CompanyID")

    End Sub

    Private Sub Email_AdjudicatorAssignment(Optional ByVal AdminComments As String = "")
        '============================================================================================
        Dim dt As DataTable
        Dim sSubject As String = "", sBody As String = "", sTo As String = "", sFrom As String = "", sToNames As String = ""
        Dim sUserName As String, sUserNameREPLACED As String, sNewlyAssignedUserCompanyID As String, AssignedAdjudicatorEmailAddress As String
        Dim bEmailAddressError As Boolean = True
        '============================================================================================
        Me.lblErrors.Visible = False
        Me.lblSuccessful.Visible = False

        Try
            sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString

            '=== Get the User info of the NEW Adjudicator Assigned ===
            dt = DataAccess.Get_UserRecord(Me.ddlPK_UserID.SelectedValue.ToString)
            If dt.Rows.Count > 0 Then
                sTo = sTo & ", " & dt.Rows(0)("EmailPrimary").ToString & ", "
                AssignedAdjudicatorEmailAddress = dt.Rows(0)("EmailPrimary").ToString
                If dt.Rows(0)("EmailSecondary").ToString.Length > 6 Then sTo = sTo & dt.Rows(0)("EmailSecondary").ToString() & ", "
                sUserName = dt.Rows(0)("FullName").ToString
                sToNames = sToNames & dt.Rows(0)("FullName").ToString & " [" & dt.Rows(0)("EmailPrimary").ToString & "],"
                sNewlyAssignedUserCompanyID = dt.Rows(0)("FK_CompanyID").ToString
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.ForeColor = System.Drawing.Color.Red
                Me.lblErrors.Text = "<P><B>ERROR</B>: Could not find the UserID for " & Me.ddlPK_UserID.SelectedItem.Text
                Exit Sub
            End If

            '=== Get the User info of the ADJUDICATOR TO REPLACE ===
            dt = New DataTable
            dt = DataAccess.Get_UserRecord(Me.txtFK_UserID_Adjudicator.Text)
            If dt.Rows.Count > 0 Then
                sTo = sTo & ", " & dt.Rows(0)("EmailPrimary").ToString & ", "
                sUserNameREPLACED = dt.Rows(0)("FullName").ToString
                sToNames = sToNames & dt.Rows(0)("FullName").ToString & " [" & dt.Rows(0)("EmailPrimary").ToString & "],"
                If dt.Rows(0)("EmailSecondary").ToString.Length > 6 Then sTo = sTo & dt.Rows(0)("EmailSecondary").ToString() & ", "
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.ForeColor = System.Drawing.Color.Red
                Me.lblErrors.Text = "<P><B>ERROR</B>: Could not find the UserID for " & Me.lblCurrentAdjudicator.Text
                Exit Sub
            End If

            '=== Get All Email Reciepients - Liaisons and Company emails addresses 
            sTo = sTo & Get_CompanyMemberEmails(Me.txtFK_CompanyID.Text, , 3, True)                     'Producing Company
            sTo = sTo & Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, , 3, True)   'Adjudicating Company getting credit for Adjudication
            If sNewlyAssignedUserCompanyID <> Me.ddlFK_CompanyID.SelectedValue.ToString Then
                sTo = sTo & Get_CompanyMemberEmails(sNewlyAssignedUserCompanyID, , 3, True)             'Adjudicator Company if different from Original Company
            End If

            '=== Get All Email Reciepients - Liaisons and Company emails addresses W/NAMES
            sToNames = sToNames & Get_CompanyMemberEmails(Me.txtFK_CompanyID.Text, , 3, True, True)
            sToNames = sToNames & Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, , 3, True, True)
            If sNewlyAssignedUserCompanyID <> Me.ddlFK_CompanyID.SelectedValue.ToString Then
                sToNames = sToNames & Get_CompanyMemberEmails(sNewlyAssignedUserCompanyID, , 3, True)
            End If
            If sToNames.Length > 6 Then
                sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1) 'removes last comma
                sToNames = sToNames.Replace(",", "<li>")                        'Sets up for Ordered List <LI>
            End If

            '=== Create the Text for the Email Message ===
            If AdminComments.Length > 1 Then AdminComments = "<br /><p style=""BACKGROUND-COLOR: lemonchiffon; ""><B><FONT COLOR=#404000>NHTA ADMINISTRATOR COMMENTS:</span></B> " & AdminComments & "</p>"

            sSubject = "NHTA Adjudication *Re-assignment* from " & Me.lblCurrentAdjudicator.Text & " to " & sUserName & " for '" & Me.lblTitle.Text & "'"
            sBody = sBody & "<font color=red><B>IMPORTANT</B>:</span> 'Re-Assignment Requests' are only accepted when submitted via the Adjudication Website.  DO NOT request reassignments via email as they will not be processed.<br />"
            sBody = sBody & AdminComments
            sBody = sBody & "<B>" & sUserName & "</B>,<br /><br />"
            sBody = sBody & "You have been Assigned to Adjudicate the following Production in place of <B>" & Me.lblCurrentAdjudicator.Text & "</B>:<br />"
            sBody = sBody & FormatEmailHTML_Production(Me.txtPK_ProductionID.Text)
            sBody = sBody & FormatEmailHTML_AssignedAdjudicator(Me.ddlPK_UserID.SelectedValue.ToString, Me.txtPK_ScoringID.Text)
            sBody = sBody & "  <I>Please confirm <u>your</u> 'Attendance Date' with the Producing Company <b>" & Me.lblCompanyName.Text & "</b><br />or Request Re-Assignment as soon as possible.</I><br /><br />"
            sBody = sBody & "Thank you.<br /><br />"
            sBody = sBody & FormatEmailHTML_AutomatedEmailDisclaimer()
            sBody = sBody & sToNames
            sBody = sBody & Common.Get_EmailFooter()

            '=== Send the Email & Get Email addresses via Get_CompanyMemberEmails() function ===
            SendCDOEmail(sFrom, sTo, False, sSubject, sBody, True, , Session("LoginID"))

            Me.lblSuccessful.Text = "A Adjudication Assignment confirmation email has been successfully sent <br /> from <b>" & _
                                        ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString & _
                                        "</b> to the following email addresses<br /><ul><li>" & sToNames & "</ul><br />"
            '"Email Body:<br /><hr noshade>" & sBody
            Me.lblSuccessful.Visible = True
            '=====> for testing <=====
            'Me.lblSuccessful.Text = Me.lblSuccessful.Text & "<br /><br />" & sBody

        Catch ex As Exception
            'Throw
            Me.lblErrors.Visible = True
            Me.lblErrors.ForeColor = System.Drawing.Color.Red
            Me.lblErrors.Text = "<P><B>ERROR</B>: An internal Mail Server error prevented the email from being Generated.</P>"
            Me.lblErrors.Text = Me.lblErrors.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try
    End Sub


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim sSQL As String = "", iTest As Integer = 0

        Try
            iTest = CInt(Me.txtPK_ScoringID.Text)
            sSQL = "UPDATE Scoring SET AdjudicatorRequestsReassignment=0 WHERE PK_ScoringID=" & iTest.ToString

            DataAccess.Run_SQL_Query(sSQL)

            Me.pnlGrid.Visible = True
            Me.pnlReassignAdjudicator.Visible = False

            Call Populate_DataGrid()

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.ForeColor = System.Drawing.Color.Red
            Me.lblErrors.Text = "<P><B>ERROR</B>: Error Deleting the record.</P>"
            Me.lblErrors.Text = Me.lblErrors.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"

        End Try

    End Sub

End Class