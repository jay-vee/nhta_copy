Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CustomMail

Partial Public Class AdminAssignAdjudicators
    Inherits System.Web.UI.Page

    Dim sLoginID As String = ""
    Dim iAccessLevel As Int16 = 0
    Dim iProductionID As Integer
    Dim iUserLoginID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            iUserLoginID = DataAccess.Get_UserID(sLoginID)
            Call Populate_DropDowns()
            Call Populate_Data()
            Call Populate_DropDowns()   'Need this a 2nd time (why?)
        End If

    End Sub

    Private Sub Set_ProductionAdjudicatorList()
        '====================================================================================================
        ' List of Adjudicators for Selected Production
        '====================================================================================================
        Me.gridSub.DataSource = DataAccess.Set_ProductionAdjudicatorList(Me.txtPK_NominationID.Text)
        Me.gridSub.DataBind()

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

        dt.Clear()
        dt = Get_ScoringStatus()

        If dt.Rows.Count > 0 Then
            Me.ddlPK_ScoringStatusID.DataSource = dt
            Me.ddlPK_ScoringStatusID.DataValueField = "PK_ScoringStatusID"
            Me.ddlPK_ScoringStatusID.DataTextField = "ScoringStatus"
            Me.ddlPK_ScoringStatusID.DataBind()
        End If
    End Sub


    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT  Production.Title, Company.CompanyName, Company.EmailAddress, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
                " 		Company.PK_CompanyID, Nominations.PK_NominationsID, Company.EmailAddress as CompanyEmailAddress, Company.Website as CompanyWebsite, " & _
                "       Venue.VenueName, Venue.Address, Venue.State, Venue.City, Venue.Zip, Venue.Website, " & _
                "       Production.PK_ProductionID, Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, " & _
                "       COUNT(Users.PK_UserID) AS UsersAssigned " & _
                "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                " FROM  Nominations INNER JOIN " & _
                "       Production ON Nominations.FK_ProductionID = Production.PK_ProductionID INNER JOIN " & _
                "       Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                "       Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID LEFT OUTER JOIN " & _
                "       Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID LEFT OUTER JOIN " & _
                "       Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                " GROUP BY Production.Title, Company.CompanyName,  Company.EmailAddress, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
                " 		Company.PK_CompanyID, Nominations.PK_NominationsID, Company.Website, " & _
                "       Venue.VenueName, Venue.Address, Venue.State, Venue.City, Venue.Zip, Venue.Website,  " & _
                "       Production.PK_ProductionID, Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, " & _
                "       ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory "

        If Not (txtSortColumnName.Text = "") And Not (txtSortOrder.Text = "FirstPerformanceDateTime") Then
            sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY FirstPerformanceDateTime "
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        gridMain.DataSource = dt
        gridMain.DataBind()

        If dt.Rows.Count > 0 Then
            lblTotalNumberOfRecords.Text = dt.Rows.Count.ToString
        End If

    End Sub

    Private Sub Populate_AssignAdjudicatorData(Optional ByVal ExcludeAlreadyAssigned As Boolean = True)
        '====================================================================================================
        Dim dt As New DataTable, iProducingCompanyID As Integer
        '====================================================================================================
        ' Listing of Adjudicators that 
        ' - EXCLUDEs Adjudicators from the Company producing the Production to be Adjudicated
        ' - EXCLUDEs Assigned Adjudicators already assigned for ANY Production from the Producing Company
        ' - EXCLUDEs Adjudicators with Old Training Dates 
        '====================================================================================================
        If ViewState("EditExistingAssignment") = True Then
            iProducingCompanyID = 0                                    'Set to 0 to get all company Adjudicators
            dt = Get_AvailableAdjudicatorsForProduction(Me.lblValidTrainingStartDate.Text, _
                                                        Me.chkShowAssignmentCounts.Checked, _
                                                        Me.txtPK_NominationID.Text, _
                                                        True, _
                                                        iProducingCompanyID, True, True, False, False)
        Else
            If chkShowAllAdjudicators.Checked = False Then
                dt = Get_AvailableAdjudicatorsForProduction(Me.lblValidTrainingStartDate.Text, _
                                                            Me.chkShowAssignmentCounts.Checked, _
                                                            Me.txtPK_NominationID.Text, _
                                                            Me.chkIncludeBackupAdjudicators.Checked, _
                                                            Me.txtFK_CompanyID.Text, True, True, ExcludeAlreadyAssigned)
            ElseIf chkShowAllAdjudicators.Checked = True Then
                iProducingCompanyID = 0                                    'Set to 0 to get all company Adjudicators
                dt = Get_AvailableAdjudicatorsForProduction(Me.lblValidTrainingStartDate.Text, _
                                                            Me.chkShowAssignmentCounts.Checked, _
                                                            Me.txtPK_NominationID.Text, _
                                                            True, _
                                                            iProducingCompanyID, True, True, ExcludeAlreadyAssigned)
            End If
        End If

        Try
            If dt.Rows.Count > 0 Then
                ddlPK_UserID.DataSource = dt
                ddlPK_UserID.DataValueField = "PK_UserID"
                ddlPK_UserID.DataTextField = "Fullname"
                ddlPK_UserID.DataBind()
            End If
        Catch ex As Exception

        End Try

        Call Populate_DropDownLists()

    End Sub

    Private Sub Populate_DropDownLists()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
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

    Private Sub chkIncludeBackupAdjudicators_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIncludeBackupAdjudicators.CheckedChanged
        Call Populate_AssignAdjudicatorData()
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

        Populate_Data()
    End Sub

    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        Dim iUsersAssigned As Int16
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" Then         ' Do not check if row is a HEADER Row
            If e.Item.Cells(4).Text <> "&nbsp;" Then
                iUsersAssigned = CInt(e.Item.Cells(4).Text)

                If iUsersAssigned < CInt(Me.lblNumAdjudicatorsPerShow.Text) Then
                    e.Item.Cells(4).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(4).Font.Bold = True
                Else
                    If iUsersAssigned > CInt(Me.lblNumAdjudicatorsPerShow.Text) Then
                        e.Item.Cells(4).Font.Bold = True
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub gridSub_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Me.lblSucessfulUpdate.Visible = False
        Me.lblErrors.Visible = False

        Me.txtFullName.Text = e.Item.Cells(5).Text

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command" '===================================================================================================================
                Me.pnlAdjudictorList.Visible = False
                Me.pnlSelectedProductionDetail.Visible = True
                Me.pnlAddEdit.Visible = True

                Me.chkShowAllAdjudicators.Checked = False        ' To more easisly see an EXISTING assignment
                Me.chkShowAssignmentCounts.Checked = False       ' To more easisly see an EXISTING assignment
                ViewState("EditExistingAssignment") = True

                Call Populate_AssignAdjudicatorData(False)       ' False is needed to see EXISTING assigned User in DDL 

                Me.txtPK_ScoringID.Text = e.Item.Cells(0).Text
                Try
                    Me.ddlAdjudicatorRequestsReassignment.SelectedValue = e.Item.Cells(12).Text
                    Me.ddlPK_UserID.SelectedValue = e.Item.Cells(2).Text
                    Me.ddlFK_CompanyID.SelectedValue = e.Item.Cells(3).Text
                Catch ex As Exception
                    'do nothing
                End Try

                Me.ddlPK_ScoringStatusID.SelectedValue = e.Item.Cells(19).Text
                Me.ddlFK_CompanyID.Enabled = True           ' Make edits able to change the companys
                Me.ddlReserveAdjudicator.SelectedValue = e.Item.Cells(17).Text
                If e.Item.Cells(10).Text = "&nbsp;" Then
                    Me.txtProductionDateAdjudicated_Actual.Text = ""
                Else
                    Me.txtProductionDateAdjudicated_Actual.Text = CDate(e.Item.Cells(10).Text).ToShortDateString
                End If
                If e.Item.Cells(11).Text = "&nbsp;" Then
                    Me.txtProductionDateAdjudicated_Planned.Text = ""
                Else
                    Me.txtProductionDateAdjudicated_Planned.Text = CDate(e.Item.Cells(11).Text).ToShortDateString
                End If
                Me.lblLastUpdateByName.Text = e.Item.Cells(13).Text
                Me.lblLastUpdateByDate.Text = CDate(e.Item.Cells(14).Text).ToShortDateString
                Me.lblCreateByName.Text = e.Item.Cells(15).Text
                Me.lblCreateByDate.Text = CDate(e.Item.Cells(16).Text).ToShortDateString

                'Me.btnUpdate.Text = "Update"

            Case "Delete_Command" '===================================================================================================================
                If e.Item.Cells(8).Text = "0" Then
                    Dim dt As DataTable = DataAccess.Get_Users

                    If dt.Rows.Count > 0 Then
                        ddlPK_UserID.DataSource = dt
                        ddlPK_UserID.DataValueField = "PK_UserID"
                        ddlPK_UserID.DataTextField = "Fullname"
                        ddlPK_UserID.DataBind()
                    End If

                    Call Populate_DropDownLists()

                    Me.txtPK_ScoringID.Text = e.Item.Cells(0).Text
                    Me.ddlPK_UserID.SelectedValue = e.Item.Cells(2).Text
                    Me.ddlFK_CompanyID.SelectedValue = e.Item.Cells(3).Text

                    Me.pnlGrid.Visible = False
                    Me.pnlAdjudictorList.Visible = False
                    Me.pnlAddEdit.Visible = False
                    Me.pnlSelectedProductionDetail.Visible = False
                    Me.pnlDeleteConfirm.Visible = True
                    lblConfirmDelete.Text = "<br />CONFIRM DELETE: <br />Are you sure you want to Delete the Adjudication Assignment: <br /><br /><B>" & e.Item.Cells(5).Text & "</B> <br />for <br /><B>" & Me.lblTitle.Text & "</B> "
                Else
                    Me.lblErrors.Visible = True
                    Me.lblErrors.Text = "ERROR: You cannot Delete Adjudictor Assignment if Adjudicator has performed Scoring.  Please set all Scores to ZERO before deleting Adjudication Assignment."
                End If

            Case Else
                ' break 
        End Select
    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command" '===================================================================================================================
                Me.pnlGrid.Visible = False
                Me.pnlAdjudictorList.Visible = True
                Me.pnlAddEdit.Visible = False
                Me.pnlSelectedProductionDetail.Visible = True

                Me.txtPK_NominationID.Text = e.Item.Cells(0).Text
                Me.txtPK_ProductionID.Text = e.Item.Cells(1).Text
                Me.lblTitle.Text = e.Item.Cells(3).Text
                Me.lblCompanyName.Text = e.Item.Cells(5).Text() & " - " & e.Item.Cells(6).Text
                Me.lblCompanyEmailAddress.Text = e.Item.Cells(23).Text
                Me.hlnkCompanyWebsite.Text = Common.FormatHyperLink(e.Item.Cells(22).Text)
                Me.hlnkCompanyWebsite.NavigateUrl = Me.hlnkCompanyWebsite.Text
                Me.lblTotalScore.Text = e.Item.Cells(7).Text
                Me.lblFirstPerformanceDateTime.Text = CDate(e.Item.Cells(9).Text()).ToShortDateString
                Me.lblLastPerformanceDateTime.Text = CDate(e.Item.Cells(10).Text()).ToShortDateString
                Me.lblVenueName.Text = e.Item.Cells(11).Text
                Me.lblAddress.Text = e.Item.Cells(14).Text
                Me.lblCity.Text = e.Item.Cells(15).Text
                Me.lblState.Text = e.Item.Cells(16).Text
                Me.lblZIP.Text = e.Item.Cells(17).Text
                Me.hlnkWebsite.Text = Common.FormatHyperLink(e.Item.Cells(18).Text)
                Me.hlnkWebsite.NavigateUrl = Me.hlnkWebsite.Text
                Me.lblTicketContactName.Text = e.Item.Cells(19).Text
                Me.lblTicketContactPhone.Text = e.Item.Cells(20).Text
                Me.lblTicketContactEmail.Text = e.Item.Cells(21).Text

                Me.txtFK_CompanyID.Text = e.Item.Cells(12).Text

                Call Set_ProductionAdjudicatorList()


            Case "Delete_Command" '===================================================================================================================

            Case Else             '===================================================================================================================
                ' break 
        End Select
    End Sub

    Private Sub lbtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        ViewState("EditExistingAssignment") = False

        Me.pnlAdjudictorList.Visible = False
        Me.pnlSelectedProductionDetail.Visible = True
        Me.pnlAddEdit.Visible = True
        ddlFK_CompanyID.Enabled = False
        Me.txtPK_ScoringID.Text = "0"

        Call Populate_AssignAdjudicatorData()
    End Sub

    Private Sub ddlPK_UserID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPK_UserID.SelectedIndexChanged
        '====================================================================================================
        Dim iCheck As Integer
        '====================================================================================================
        iCheck = DataAccess.Find_CompanyForUserID(Me.ddlPK_UserID.SelectedValue).ToString
        If iCheck > 2 Then ddlFK_CompanyID.SelectedValue = iCheck
        ddlFK_CompanyID.Enabled = True
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(50) As String
        Dim DateTester As Date
        '====================================================================================================
        Me.lblErrors.Visible = False        ' reset error msg

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

        If Me.txtProductionDateAdjudicated_Planned.Text <> "" Then
            Try
                DateTester = (CDate(txtProductionDateAdjudicated_Planned.Text))
            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Please provide a VALID Planned Adjudication Date value."
                Exit Sub
            End Try
        End If

        If Me.txtProductionDateAdjudicated_Actual.Text <> "" Then
            Try
                DateTester = (CDate(txtProductionDateAdjudicated_Actual.Text))
            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Please provide a VALID Adjudication Date value."
                Exit Sub
            End Try
        End If

        sDataValues(1) = Me.txtPK_ScoringID.Text
        sDataValues(2) = Me.txtPK_NominationID.Text
        sDataValues(3) = Me.ddlFK_CompanyID.SelectedValue
        sDataValues(4) = Me.ddlPK_UserID.SelectedValue
        sDataValues(5) = Me.ddlAdjudicatorRequestsReassignment.SelectedValue
        sDataValues(6) = "0"        ' AdjudicatorScoringLocked
        sDataValues(7) = IIf(Me.txtProductionDateAdjudicated_Planned.Text = "", "", Me.txtProductionDateAdjudicated_Planned.Text)
        sDataValues(8) = IIf(Me.txtProductionDateAdjudicated_Actual.Text = "", "", Me.txtProductionDateAdjudicated_Actual.Text)
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
        sDataValues(43) = Me.ddlPK_ScoringStatusID.SelectedValue
        sDataValues(44) = ""
        sDataValues(45) = 1         ' Needed for Update without Deleting any Submitted scoring

        If Save_Scoring(sDataValues) = True Then
            'Send Email if option selected
            If rblEmailInfo.SelectedIndex = 1 Then Call Email_AdjudicatorAssignment(False, Me.txtAdminEmailComments_Assign.Text)

            Me.pnlGrid.Visible = False
            Me.pnlAddEdit.Visible = False
            Me.pnlSelectedProductionDetail.Visible = True
            Me.pnlAdjudictorList.Visible = True

            Call Set_ProductionAdjudicatorList()

        Else
            Me.lblErrors.Text = "ERROR: Saving Scoring Data"
            Me.lblErrors.Visible = True
        End If

    End Sub

    Private Sub btnDeleteCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteCancel.Click
        Me.pnlGrid.Visible = False
        Me.pnlAdjudictorList.Visible = True
        Me.pnlAddEdit.Visible = False
        Me.pnlSelectedProductionDetail.Visible = True
        Me.pnlDeleteConfirm.Visible = False
    End Sub

    Private Sub btnDeleteConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteConfirm.Click
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        If txtPK_ScoringID.Text <> "" Then

            'Need to send the DELETE Email before actual delete to get the Info in the Email.
            If Me.rblEmailInfo_Delete.SelectedIndex = 1 Then Call Email_AdjudicatorAssignment(True, Me.txtAdminEmailComments_Delete.Text)

            'Delete the Assignment
            sSQL = "DELETE FROM Scoring WHERE PK_ScoringID = " & txtPK_ScoringID.Text
            Call DataAccess.SQLDelete(sSQL)

            Me.pnlGrid.Visible = False
            Me.pnlAdjudictorList.Visible = True
            Me.pnlAddEdit.Visible = False
            Me.pnlSelectedProductionDetail.Visible = True
            Me.pnlDeleteConfirm.Visible = False
            Me.txtPK_ScoringID.Text = "0"   'reset the value if a new Add will happen

            Call Set_ProductionAdjudicatorList()
        Else

        End If

    End Sub

    Private Sub chkShowAssignmentCounts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAssignmentCounts.CheckedChanged
        Call Populate_AssignAdjudicatorData()
    End Sub

    Private Sub btnCancelUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelUpdate.Click
        Me.pnlGrid.Visible = False
        Me.pnlAdjudictorList.Visible = True
        Me.pnlAddEdit.Visible = False
        Me.pnlSelectedProductionDetail.Visible = True
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False
    End Sub

    Private Sub chkShowAllAdjudicators_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAllAdjudicators.CheckedChanged
        Call Populate_AssignAdjudicatorData()

        If Me.chkShowAllAdjudicators.Checked = True Then
            Response.Write("<script language=javascript>")
            Response.Write("alert('WARNING: You have bypassed all assigment restrictions.  Proceed with *Caution* to keep within the NHTA Policies and Procedures*');")
            Response.Write("</script>")
        End If

    End Sub

    Private Sub Email_AdjudicatorAssignment(Optional ByVal DeleteAdjudicator As Boolean = False, Optional ByVal AdminComments As String = "")
        '============================================================================================
        Dim dt As DataTable, dtUser As DataTable
        Dim sSubject As String, sBody As String = "", sTo As String = "", sFrom As String = ""
        Dim sToNames As String = ""
        Dim bEmailAddressError As Boolean = True
        '============================================================================================
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False

        Try
            sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString

            '=== Get All Email Reciepients - Liaisons and Company emails addresses
            sTo = sTo & Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, Me.ddlPK_UserID.SelectedValue.ToString, 3, True)    'Adjudicating Company
            sTo = sTo & Get_CompanyMemberEmails(Me.txtFK_CompanyID.Text, , 3, True)                     'Producing Company

            '=== Get All Email Reciepients - Liaisons and Company emails addresses W/NAMES
            sToNames = sToNames & Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, , 3, True, True) & Get_CompanyMemberEmails(Me.txtFK_CompanyID.Text, , 3, True, True)

            If sToNames.Length > 6 Then
                sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1) 'removes last comma
                sToNames = sToNames.Replace(",", "<li>")
                sToNames = "<hr noshade><B>NOTE:</b> This email has been sent to the Liaisons for the Adjudicating and Producing Theatre Companies: " & _
                                                        "<ul><li>" & sToNames & "</ul>"
            End If

            '=== Get the Scoring Data =============================================================
            dt = Nothing
            dt = DataAccess.Get_Score(Me.txtPK_ProductionID.Text, Me.ddlPK_UserID.SelectedValue.ToString)
            Me.txtPK_ScoringID.Text = dt.Rows(0)("PK_ScoringID")

            If AdminComments.Length > 1 Then AdminComments = "<br /><p style=""BACKGROUND-COLOR: lemonchiffon; ""><B><FONT COLOR=#404000>NHTA ADMINISTRATOR COMMENTS:</span></B> " & AdminComments & "</p>"

            dtUser = Get_UserRecord(Me.ddlPK_UserID.SelectedValue.ToString)
            ' === Create the Text for the Email Message ======================================================================
            If DeleteAdjudicator = True Then
                sSubject = "NHTA *Removed* Adjudication assignment for '" & dtUser.Rows(0)("FullName").ToString & " 'on production '" & Me.lblTitle.Text & "'"
                sBody = sBody & AdminComments
                sBody = sBody & "<b>" & dtUser.Rows(0)("FullName").ToString & "</b> has been <b><U>*Removed*</b></U> as an Adjudicator for the following Production:<br />"
            Else
                sSubject = "NHTA Adjudication assignment for '" & dtUser.Rows(0)("FullName").ToString & "' to Adjudicate '" & Me.lblTitle.Text & "'"
                sBody = sBody & "<font color=red><B>IMPORTANT:</B></span> 'Re-Assignment Requests' are *only accepted* when submitted via the Adjudication Website.  DO NOT request reassignments via email as they *will not* be processed.<br /><br />"
                sBody = sBody & AdminComments
                sBody = sBody & dtUser.Rows(0)("FullName").ToString & " has been Assigned to Adjudicate the following Production:<br />"
            End If

            sBody = sBody & FormatEmailHTML_Production(Me.txtPK_ProductionID.Text)
            If DeleteAdjudicator = False Then sBody = sBody & FormatEmailHTML_AssignedAdjudicator(Me.ddlPK_UserID.SelectedValue.ToString, Me.txtPK_ScoringID.Text)
            If DeleteAdjudicator = False Then sBody = sBody & "  <I>Please confirm <u>your</u> 'Attendance Date' with the Producing Company <b>" & Me.lblCompanyName.Text & "</b><br />or Request Re-Assignment as soon as possible.</I><br /><br />"
            sBody = sBody & "Thank you.<br /><br />"
            sBody = sBody & FormatEmailHTML_AutomatedEmailDisclaimer()
            sBody = sBody & sToNames
            sBody = sBody & Common.Get_EmailFooter()

            ' === Send the Email ========================================================================================
            SendCDOEmail(sFrom, sTo, False, sSubject, sBody, True, True, Session("LoginID"), EMAIL_ADJUDICATOR_ASSIGNED)

            Me.lblSucessfulUpdate.Text = "An Confirmation Email has been successfully sent <br /> from <b>" & _
                                        ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString & _
                                        "</b> to the following email addresses<br /><ul><li>" & sToNames & "</ul><br />"
            Me.lblSucessfulUpdate.Visible = True
            '=====> for testing <=====
            'Me.lblSucessfulUpdate.Text = Me.lblSucessfulUpdate.Text & "<br /><br />" & sBody

        Catch ex As Exception
            'Throw
            Me.lblErrors.Visible = True
            Me.lblErrors.ForeColor = System.Drawing.Color.Red
            'Me.lblErrors.Text = "<P><B>ERROR</B>: An internal Mail Server error prevented the email from being Generated.</P>"
            Me.lblErrors.Text = Me.lblErrors.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try
    End Sub

End Class