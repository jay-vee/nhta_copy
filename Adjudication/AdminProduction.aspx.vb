Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CustomMail

Partial Public Class AdminProduction
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iProductionID As Integer
    Dim iCompanyID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        '============================================================================================
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel >= 1 And iAccessLevel <= 3) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        Dim da2 As New Adjudication.DataAccess
        iCompanyID = Find_CompanyForUserLoginID(sLoginID) ' Find the Company For this Liaison

        ' Find if a Production needs to be EDITed
        If Request.QueryString("ProductionID") <> "" Then
            iProductionID = Request.QueryString("ProductionID")
            Me.pnlAddEditData.Visible = True
            Me.pnlEditSelection.Visible = False

        Else
            If Request.QueryString("Add") <> "True" And Request.QueryString("Admin") <> "True" Then
                ' If not Production selected, Find all Productions for Liaisons Company
                Me.pnlAddEditData.Visible = False
                Me.pnlEditSelection.Visible = True

                Dim dt As New DataTable
                dt = DataAccess.Find_ProductionsForUserLoginID(sLoginID) ' Find the Productions For this Liaison
                If dt.Rows.Count > 0 Then
                    gridMain.DataSource = dt
                    gridMain.DataBind()
                Else
                    If iCompanyID = 0 Or iCompanyID = 1 Or iCompanyID = 2 Then
                        Me.pnlEditSelection.Visible = False
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "ERROR: You must be a Liaison for a Company to Add a Production."
                        Exit Sub
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Call Populate_DropDowns()

            If Request.QueryString("Add") = "True" Then
                If Request.QueryString("VenueID") <> "" Then
                    ddlFK_VenueID.SelectedValue = Request.QueryString("VenueID")
                    Me.pnlAddEditData.Visible = True
                    Me.pnlEditSelection.Visible = False
                Else
                    Me.pnlSelectVenue.Visible = True
                    Me.pnlEditSelection.Visible = False
                End If

                Me.txtTitle.Enabled = True
                Me.ddlFK_VenueID.Enabled = True
                Me.ddlFK_ProductionCategoryID.Enabled = True
                Me.ddlFK_ProductionTypeID.Enabled = True
                Me.ddlRequiresAdjudication.Enabled = True
                Me.txtFirstPerformanceDateTime.Enabled = True
                Me.txtLastPerformanceDateTime.Enabled = True
            Else
                Call Populate_Data()
            End If
        End If
        If iAccessLevel = 1 Then
            Me.ddlFK_CompanyID.Enabled = True
        Else
            Me.ddlFK_CompanyID.Enabled = False
        End If

    End Sub


    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        dt = DataAccess.Get_ApplicationDefaults

        lblEditInformation.Text = "<p class=""bg-info text-warning"" style=""padding: 0.5em;"">NOTE: Liaisons can edit Production Name, Date, Venue up to <b>" & dt.Rows(0)("DaysToSubmitProduction").ToString & " days</b> before the Production opens." & _
                                    "<br>If you need information changed within that timeframe, you must email the Administrator at <a href=""mailto:" & dt.Rows(0)("AdminContactEmail").ToString & "?subject=Need Production Info updated"">" & dt.Rows(0)("AdminContactEmail").ToString & "</a></p>"

        '====================================================================================================
        sSQL = "SELECT PK_ProductionTypeID, ProductionType FROM ProductionType"

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            ddlFK_ProductionTypeID.DataSource = dt
            ddlFK_ProductionTypeID.DataValueField = "PK_ProductionTypeID"
            ddlFK_ProductionTypeID.DataTextField = "ProductionType"
            ddlFK_ProductionTypeID.DataBind()
        End If

        '====================================================================================================
        sSQL = "SELECT PK_AgeAppropriateID, AgeAppropriateName FROM AgeAppropriate"

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.ddlFK_AgeApproriateID.DataSource = dt
            ddlFK_AgeApproriateID.DataValueField = "PK_AgeAppropriateID"
            ddlFK_AgeApproriateID.DataTextField = "AgeAppropriateName"
            ddlFK_AgeApproriateID.DataBind()
        End If

        '====================================================================================================
        sSQL = "SELECT PK_VenueID, VenueName + ', ' + City  + ' ' + State as VenueLocation FROM Venue ORDER BY VenueName "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.ddlFK_VenueID.DataSource = dt
            ddlFK_VenueID.DataValueField = "PK_VenueID"
            ddlFK_VenueID.DataTextField = "VenueLocation"
            ddlFK_VenueID.DataBind()
        End If

        sSQL = "SELECT PK_VenueID, VenueName, City, State FROM Venue ORDER BY VenueName "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridVenue.DataSource = dt
            gridVenue.DataBind()
        End If

        '====================================================================================================
        sSQL = "SELECT PK_CompanyID, CompanyName FROM Company WHERE  (NOT (Company.CompanyName LIKE '*%')) AND ActiveCompany=1 ORDER BY CompanyName "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.ddlFK_CompanyID.DataSource = dt
            ddlFK_CompanyID.DataValueField = "PK_CompanyID"
            ddlFK_CompanyID.DataTextField = "CompanyName"
            ddlFK_CompanyID.DataBind()

            If iCompanyID > 0 Then ddlFK_CompanyID.SelectedValue = iCompanyID.ToString
        End If

        '====================================================================================================
        sSQL = "SELECT PK_ProductionCategoryID, ProductionCategory FROM ProductionCategory "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.ddlFK_ProductionCategoryID.DataSource = dt
            ddlFK_ProductionCategoryID.DataValueField = "PK_ProductionCategoryID"
            ddlFK_ProductionCategoryID.DataTextField = "ProductionCategory"
            ddlFK_ProductionCategoryID.DataBind()
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String, dt2 As DataTable
        '====================================================================================================
        sSQL = "SELECT ProductionType.ProductionType, PK_ProductionID, FK_CompanyID, FK_VenueID, FK_AgeApproriateID, " & _
                    " FK_ProductionTypeID, Title, Authors, LicensingCompany, FirstPerformanceDateTime,  " & _
                    " LastPerformanceDateTime, AllPerformanceDatesTimes, TicketContactName, TicketContactPhone,  " & _
                    " TicketContactEmail, TicketPurchaseDetails, RequiresAdjudication, Comments, OriginalProduction, " & _
                    " Production.LastUpdateByName, Production.LastUpdateByDate " & _
                    " , Production.FK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                " FROM ProductionType " & _
                    "   INNER JOIN Production ON ProductionType.PK_ProductionTypeID = Production.FK_ProductionTypeID " & _
                    "   INNER JOIN ProductionCategory ON ProductionCategory.PK_ProductionCategoryID = Production.FK_ProductionCategoryID " & _
                " WHERE PK_ProductionID=" & iProductionID & _
                " ORDER BY FirstPerformanceDateTime "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.txtPK_ProductionID.Text = dt.Rows(0)("PK_ProductionID").ToString
            Me.txtTitle.Text = dt.Rows(0)("Title").ToString
            Me.ddlFK_CompanyID.SelectedValue = dt.Rows(0)("FK_CompanyID").ToString
            Me.ddlFK_ProductionCategoryID.SelectedValue = dt.Rows(0)("FK_ProductionCategoryID").ToString
            If CInt(dt.Rows(0)("FK_VenueID").ToString) > 2 Then Me.ddlFK_VenueID.SelectedValue = dt.Rows(0)("FK_VenueID").ToString
            Me.ddlFK_AgeApproriateID.SelectedValue = dt.Rows(0)("FK_AgeApproriateID").ToString
            Me.ddlFK_ProductionTypeID.SelectedValue = dt.Rows(0)("FK_ProductionTypeID").ToString
            Me.txtAuthors.Text = dt.Rows(0)("Authors").ToString
            Me.txtLicensingCompany.Text = dt.Rows(0)("LicensingCompany").ToString
            Me.txtFirstPerformanceDateTime.Text = CDate(dt.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString
            Me.txtLastPerformanceDateTime.Text = CDate(dt.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            Me.txtAllPerformanceDatesTimes.Text = dt.Rows(0)("AllPerformanceDatesTimes").ToString
            Me.txtTicketContactName.Text = dt.Rows(0)("TicketContactName").ToString
            Me.txtTicketContactPhone.Text = dt.Rows(0)("TicketContactPhone").ToString
            Me.txtTicketContactEmail.Text = dt.Rows(0)("TicketContactEmail").ToString
            Me.txtTicketPurchaseDetails.Text = dt.Rows(0)("TicketPurchaseDetails").ToString
            Me.ddlRequiresAdjudication.SelectedValue = dt.Rows(0)("RequiresAdjudication").ToString
            Me.ddlOriginalProduction.SelectedValue = dt.Rows(0)("OriginalProduction").ToString
            Me.txtComments.Text = dt.Rows(0)("Comments").ToString
            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString

            If iAccessLevel = 1 Then
                Me.txtTitle.Enabled = True
                Me.ddlFK_CompanyID.Enabled = True
                Me.ddlFK_VenueID.Enabled = True
                Me.ddlRequiresAdjudication.Enabled = True
                Me.ddlFK_ProductionCategoryID.Enabled = True
                Me.ddlFK_ProductionTypeID.Enabled = True
                Me.txtFirstPerformanceDateTime.Enabled = True
                Me.txtLastPerformanceDateTime.Enabled = True
            Else
                sSQL = "SELECT TOP 1 DaysToSubmitProduction, DaysToAllowNominationEdits, NumAdjudicatorsPerShow FROM ApplicationDefaults "
                dt2 = DataAccess.Run_SQL_Query(sSQL)

                If Today.Date <= CDate(txtFirstPerformanceDateTime.Text).Subtract(TimeSpan.FromDays(CInt(dt2.Rows(0)("DaysToAllowNominationEdits").ToString))) Then
                    Me.txtTitle.Enabled = True
                    Me.ddlFK_CompanyID.Enabled = True
                    Me.ddlFK_VenueID.Enabled = True
                    Me.ddlFK_ProductionCategoryID.Enabled = True
                    Me.ddlFK_ProductionTypeID.Enabled = True
                    Me.txtFirstPerformanceDateTime.Enabled = True
                    Me.txtLastPerformanceDateTime.Enabled = True
                End If

            End If
        End If
    End Sub

    Public Sub gridVenue_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Select_Command"
                Response.Redirect("AdminProduction.aspx?Add=True&VenueID=" & e.Item.Cells(0).Text)
            Case Else
                ' break 
        End Select
    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminProduction.aspx?ProductionID=" & e.Item.Cells(0).Text)

            Case "Nomination_Command"
                Response.Redirect("AdminNominations.aspx?Liaison=True&ProductionID=" & e.Item.Cells(0).Text)

            Case "Delete_Command"
                'Me.pnlGrid.Visible = False
                'Me.pnlDeleteConfirm.Visible = True
                'lblConfirmDelete.Text = "CONFIRM DELETE: WHEN " & ddlAUTO_VALIDATION_FIELD_ID.SelectedItem.ToString & " " & ddlLOGIC_OPERATOR_TYPE_ID.SelectedItem.ToString & " " & AUTO_VALIDATION_FIELD_VALUE_TXT.Text & "  THEN Set Record Status = " & ddlRECORD_STATUS_ID.SelectedItem.ToString

            Case Else
                ' break 
        End Select
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim sDataValues(25) As String
        Dim DateTester_End As Date, DateTester_Start As Date
        Dim dtSettings As DataTable
        '====================================================================================================
        Me.lblErrors.Visible = False  'Reset error message
        Me.lblSucessfulUpdate.Visible = False
        '====================================================================================================
        '=== Data validation ===
        If Me.txtTitle.Text = "" Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide Production Name."
            Exit Sub
        End If

        If Me.txtFirstPerformanceDateTime.Text = "" Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide the date of the First Production."
            Exit Sub
        Else
            Try
                DateTester_Start = (CDate(txtFirstPerformanceDateTime.Text))
                DateTester_End = (CDate(txtLastPerformanceDateTime.Text))
            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Please provide a VALID date values for the Production Dates."
                Exit Sub
            End Try
        End If

        If Me.txtLastPerformanceDateTime.Text = "" Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide the date of the Last Production."
            Exit Sub
        Else
            Try
                dtSettings = DataAccess.Get_ApplicationDefaults()
                If CDate(dtSettings.Rows(0)("NHTAYearEndDate")) < DateTester_End Then
                    Me.lblErrors.Visible = True
                    Me.lblErrors.Text = "ERROR: the Production 'Last Performance Date' cannot be after the NHTA Year closes (" & CDate(dtSettings.Rows(0)("NHTAYearEndDate")).ToShortDateString & ")."
                    Exit Sub
                End If
                If CDate(dtSettings.Rows(0)("NHTAYearStartDate")) > DateTester_Start Then
                    Me.lblErrors.Visible = True
                    Me.lblErrors.Text = "ERROR: the Production 'First Performance Date' cannot be before the NHTA Year starts (" & CDate(dtSettings.Rows(0)("NHTAYearStartDate")).ToShortDateString & ")."
                    Exit Sub
                End If
                If DateTester_Start.AddDays(-1 * CInt(dtSettings.Rows(0)("DaysToSubmitProduction"))) < Today Then
                    If iAccessLevel = 1 Then
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "WARNING: The Production open date is less than <b>" & dtSettings.Rows(0)("DaysToSubmitProduction").ToString & " days</b> from today.  Administrators be aware that assigning Adjudicators should be managed with care."
                    Else
                        If DateTester_Start < CDate("3/16/" & Date.Now.Year.ToString) Then
                            'Do nothing as we will allow productions to be submitted prior to March 16th without checking "DaysToSubmitProduction"
                        Else
                            Me.lblErrors.Visible = True
                            Me.lblErrors.Text = "ERROR: You must submit a Production <b>" & dtSettings.Rows(0)("DaysToSubmitProduction").ToString & " days</b> before it opens.  Email <a href=""mailto:" & dtSettings.Rows(0)("AdminContactEmail").ToString & "?subject=Need Production Data Entry assistance."">" & dtSettings.Rows(0)("AdminContactEmail").ToString & "</a> with questions."
                            Exit Sub
                        End If
                    End If
                End If
            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Please provide a VALID date value for the Last Production Date."
                Exit Sub
            End Try
        End If

        If (CDate(txtFirstPerformanceDateTime.Text)) > (CDate(txtLastPerformanceDateTime.Text)) Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: The First Date of the Production cannot be after the Last Production date."
            Exit Sub
        End If

        If Me.txtAllPerformanceDatesTimes.Text.Length < 10 Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please list ALL Performance Dates and Times, per Performance."
            Exit Sub
        End If

        Try
            '====================================================================================================
            '=== Set Data for saving in HashTable ===
            sDataValues(1) = Me.txtPK_ProductionID.Text
            sDataValues(2) = Me.ddlFK_CompanyID.SelectedValue
            sDataValues(3) = Me.ddlFK_VenueID.SelectedValue
            sDataValues(4) = Me.ddlFK_AgeApproriateID.SelectedValue
            sDataValues(5) = Me.ddlFK_ProductionTypeID.SelectedValue
            sDataValues(6) = Me.txtTitle.Text
            sDataValues(7) = Me.txtAuthors.Text
            sDataValues(8) = Me.txtLicensingCompany.Text
            sDataValues(9) = Me.txtFirstPerformanceDateTime.Text
            sDataValues(10) = Me.txtLastPerformanceDateTime.Text
            sDataValues(11) = Me.txtAllPerformanceDatesTimes.Text
            sDataValues(12) = Me.txtTicketContactName.Text
            sDataValues(13) = Me.txtTicketContactPhone.Text
            sDataValues(14) = Me.txtTicketContactEmail.Text
            sDataValues(15) = Me.txtTicketPurchaseDetails.Text
            sDataValues(16) = Me.txtComments.Text
            sDataValues(17) = Me.ddlRequiresAdjudication.SelectedValue
            sDataValues(18) = Me.ddlOriginalProduction.SelectedValue
            sDataValues(19) = Master.UserLoginID
            sDataValues(20) = Me.ddlFK_ProductionCategoryID.SelectedValue

            '=== Save Data, return production ID of any new Production ===
            Me.txtPK_ProductionID.Text = Save_Production(sDataValues)
            Me.btnUpdate.Text = "Save"
            Me.lblSucessfulUpdate.Visible = True
            Me.lblSucessfulUpdate.Text = "Production Sucessfully Added/Updated.  Remember to review Nominations."

            If Me.rblEmailInfo.SelectedIndex = 1 Then
                If Not Email_AssignedAdjudicators() = True Then
                    Me.lblErrors.Text = "ERROR: Sending EMail.  Please check the Email Log or contact the System Administrators."
                    Me.lblErrors.Visible = True
                End If
            End If

            If Request.QueryString("Admin") = "True" Then
                Response.Redirect("AdminProductionList.aspx")
            Else
                'Response.Redirect("AdminProduction.aspx")
            End If

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: Saving Production Data"
            Me.lblErrors.Visible = True
        End Try

    End Sub

    Private Sub lbtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        Me.pnlEditSelection.Visible = False
        Me.pnlSelectVenue.Visible = True
    End Sub


    Private Sub lbtnAddVenue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAddVenue.Click
        Response.Redirect("AdminVenue.aspx?Add=True")
    End Sub

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
            sToProductingCompany = Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, , 3, True)
            sToProductingCompanyNames = Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, , 3, True, True)

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
                sSubject = "NHTA | '" & Me.txtTitle.Text & "' Production Details Updated on Adjudication Website"
                sBody = sBody & AdminComments
                sBody = sBody & "This email is intended to inform you that the Production details of '" & Me.txtTitle.Text & "' have been <i>updated</i> on the NH Theatre Awards adjudication website by <b>" & Session.Item("FirstName") & " " & Session.Item("LastName") & "</b>.  " & _
                                "Please direct questions regarding this update to the Producing Company Liaison.<br /><br />Production Specifics:<br />"
                sBody = sBody & FormatEmailHTML_Production(Me.txtPK_ProductionID.Text)

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
            Return True

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