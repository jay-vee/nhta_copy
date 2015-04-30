Imports Adjudication.DataAccess
Imports Adjudication.CustomMail
Imports Adjudication.Common

Partial Public Class AdminEmailMassMailings
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DropDowns()
            Call Populate_DataGrid()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        gridMain.DataSource = Get_EmailMessageTypes(1, 1)
        gridMain.DataBind()
    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_EmailFromAddresses(False)
        If dt.Rows.Count > 0 Then
            Me.ddlPK_FromEmailID.DataSource = dt
            Me.ddlPK_FromEmailID.DataValueField = "EmailFromAddress"
            Me.ddlPK_FromEmailID.DataTextField = "EmailFromAddress"
            Me.ddlPK_FromEmailID.DataBind()
        End If

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Me.lblSendEmailError.Visible = False

        Select Case (e.Item.Cells(0).Text)
            Case "2"        ' Late Ballots [Adjudicator & Liaison(s)]
                Email_Late_AdjudicationsScores(e.Item.Cells(0).Text)
            Case "4"        ' Late Confirmation [Liaison(s) & Company]
                Me.lblSendEmailError.Visible = True
                Me.lblSendEmailError.Text = "ERROR: This Feature is not yet Available."
            Case "3"        ' Late Confirmation: [Adjudicator & Liaison(s)]
                Me.lblSendEmailError.Visible = True
                Me.lblSendEmailError.Text = "ERROR: This Feature is not yet Available."
            Case "5"        ' Late Nominations [Liaison(s) & Company]
                Me.lblSendEmailError.Visible = True
                Me.lblSendEmailError.Text = "ERROR: This Feature is not yet Available."
            Case "7"        ' Reminder: Assignments [Adjudicator] (All assigment
                Email_Reminder_AdjudicationAssignments(e.Item.Cells(0).Text)
            Case "6"        ' Reminder: Nominations  [Liaison(s) & Company]
                Email_Reminder_Nominations(e.Item.Cells(0).Text)
            Case Else
                Me.lblSendEmailError.Visible = True
                Me.lblSendEmailError.Text = "ERROR: Unknown 'Send Email' value selected"
        End Select

    End Sub

    Private Sub btnSendMoreEmails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendMoreEmails.Click
        Me.pnlMain.Visible = True
        Me.pnlFinished.Visible = False
        Me.lblReciepients.Text = ""
        Me.lblEmailSubject.Text = ""
        Me.lblHTMLEMailMessage.Text = ""

        Call Populate_DataGrid()

    End Sub

    Private Sub gridMain_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles gridMain.ItemDataBound

        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then
            If e.Item.Cells(5).Text.Length > 0 And (Not e.Item.Cells(5).Text = "&nbsp;") Then
                Try
                    Dim DateTester As Date = e.Item.Cells(5).Text
                    If Today.AddDays(-30) > DateTester Then
                        e.Item.Cells(4).ForeColor = System.Drawing.Color.Red
                        e.Item.Cells(4).Font.Bold = True
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        End If
    End Sub


    Private Sub Email_Late_AdjudicationsScores(ByVal EmailMessageTypesID As String)
        '====================================================================================================
        Dim dtLate As DataTable, dr As DataRow, dtProd As DataTable, dtDefaults As DataTable, dtAdj As DataTable
        Dim sToNames As String, sToEmails As String, sFromEmail As String, sDaysLate As String = ""
        Dim sSubject As String = "", sBody As String = ""
        Dim iCountEmailsSent As Int16 = 0
        Dim sErrorList As String = ""
        Dim NHTA_CalendarYearEnds As DateTime
        Dim Late_AfterXDays As Int16 = 0
        Dim Suspend_AfterXDays As Int16 = 60     'NEED TO ADD FIELD TO ApplicationDefaults TABLE FOR THIS
        Dim Expell_AfterXDays As Int16 = 90      'NEED TO ADD FIELD TO ApplicationDefaults TABLE FOR THIS 
        '====================================================================================================
        Try
            sFromEmail = Me.ddlPK_FromEmailID.SelectedValue.ToString

            '=== Get Default Values
            dtDefaults = DataAccess.Get_ApplicationDefaults
            NHTA_CalendarYearEnds = CDate(dtDefaults.Rows(0)("NHTAYearEndDate").ToString)
            Late_AfterXDays = CInt(dtDefaults.Rows(0)("DaysToWaitForScoring").ToString)

            '=== Get the list of Adjudicators with Late Ballots
            dtLate = Find_Late_AdjudicationsScores("LastPerformanceDateTime", "")

            '=== Create message when no ballots are late.
            If dtLate.Rows.Count = 0 Then sBody = "NO LATE BALLOTS FOUND."

            For Each dr In dtLate.Rows
                '=== Get Adjudicator Assignment Info on this late Ballot
                dtAdj = DataAccess.Find_AdjudicationsForUserLoginID(dr.Item("UserLoginID").ToString, dr.Item("PK_ScoringID").ToString)

                If dtAdj.Rows.Count > 0 Then
                    '=== Get Email Addresses of Adjudicator and all of their company Liaisions
                    sToEmails = Get_CompanyMemberEmails(dr.Item("FK_CompanyID_Adjudicator").ToString, dr.Item("PK_UserID").ToString, 3, Me.chkUseSecondaryEmailAddress.Checked, False)
                    sToNames = Get_CompanyMemberEmails(dr.Item("FK_CompanyID_Adjudicator").ToString, dr.Item("PK_UserID").ToString, 3, Me.chkUseSecondaryEmailAddress.Checked, True)
                    If sToNames.Length > 6 Then
                        sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1) 'removes last comma
                        sToNames = sToNames.Replace(",", "<li>")                        'Sets up for Ordered List <LI>
                        sToNames = "<hr noshade><B>NOTE:</b> This email has also been sent to all Liaisons for your Theatre Company: " & _
                                        "<ul><li>" & sToNames & "</ul>"
                    End If

                    '=== Get Production Info for late Ballot to include in the EMail
                    dtProd = Get_Production(dtAdj.Rows(0)("PK_ProductionID").ToString)
                    If dtProd.Rows.Count > 0 Then
                        '=== CREATE EMAIL MESSAGE
                        sSubject = "NHTA: " & dr.Item("FirstName").ToString & " " & dr.Item("LastName").ToString & " is LATE submitting Scoring Ballot for '" & dr.Item("Title").ToString & "'"

                        '=== Create Body message regarding the # of days late, and current Adjudicator Status ===
                        Try
                            Dim ShowCloseDate As DateTime = CDate(dtProd.Rows(0).Item("LastPerformanceDateTime").ToString)
                            Dim CurrentDate As DateTime = Now
                            Dim tsDaysLate As TimeSpan = CurrentDate.Subtract(ShowCloseDate)
                            sDaysLate = tsDaysLate.Days.ToString

                            sDaysLate = "As of " & Now.ToLongDateString & ", our calculations show that you are currently <b><FONT Color=""RED"">" & tsDaysLate.Days.ToString & "</span></b> days late in submitting the Scores and Comments for your assigned adjudication. "

                            Select Case tsDaysLate.Days
                                Case Suspend_AfterXDays To Expell_AfterXDays + 1
                                    sDaysLate = sDaysLate & "<br><br><b><FONT Color=""RED"">WARNING</span></b>: Your NH Theatre Award Adjudicator status is now: <b>SUSPENED</b>.  If you complete your ballot before it is " & Expell_AfterXDays.ToString & " days late you can be reinstated without issue. "
                                Case Is > Expell_AfterXDays
                                    sDaysLate = sDaysLate & "<br><br><b><FONT Color=""RED"">WARNING</span></b>: Your NH Theatre Award Adjudicator status is now: <b>EXPELLED</b>.  If you complete your ballot before the end of the NHTA calendar year on " & NHTA_CalendarYearEnds.ToLongDateString & " you can be reinstated for <i>next</i> year without issue. "
                            End Select

                        Catch ex As Exception
                            sDaysLate = dtDefaults.Rows(0)("DaysToWaitForScoring").ToString
                            sDaysLate = "As of " & Now.ToShortDateString & ", our calculations show that you are more than <b><FONT Color=""RED"">" & sDaysLate & "</span></b> "
                        End Try

                        '=== Create the BODY of the Email ===
                        sBody = "<hr noshade><font color=dark red><b>NOTE:</b> This is an automatically system generated email.  This email is intended to be a couresty reminder.</span><hr noshade>" & _
                                "<b>" & dr.Item("FirstName").ToString & " " & dr.Item("LastName").ToString & "</b>,<br /><br />" & _
                                sDaysLate & _
                                "<br><br>Our records indicate that you were assigned to adjudicate the Production: " & _
                                FormatEmailHTML_Production(dtProd) & _
                                FormatEmailHTML_AssignedAdjudicator(dtAdj) & _
                                "Please login to the NHTA Adjudication website as soon as possible and submit the Scores and Comments.<br /><br />" & _
                                "Thank you.<br /><br />" & _
                                FormatEmailHTML_AutomatedEmailDisclaimer() & _
                                sToNames & _
                                Common.Get_EmailFooter()


                        '=== Send and Log Email === 
                        If sToEmails.Length > 6 And sFromEmail.Length > 6 Then
                            SendCDOEmail(sFromEmail, sToEmails, Me.rblReceipientType.SelectedValue, sSubject, sBody, Me.chkHighPriority.Checked, True, Session("LoginID"), EmailMessageTypesID)
                            iCountEmailsSent = iCountEmailsSent + 1
                        Else
                            sErrorList = sErrorList & "Email Address not found for: " & FormatEmailHTML_AssignedAdjudicator(dtAdj) & "<hr noshade>"
                        End If
                    End If
                End If

                dtAdj.Clear()                                         'Clear the Datatable

            Next

            Me.pnlMain.Visible = False
            Me.pnlFinished.Visible = True
            Me.lblReciepients.Text = iCountEmailsSent.ToString
            Me.lblEmailSubject.Text = sSubject
            Me.lblHTMLEMailMessage.Text = "<Font color=""DarkRed"">" & sErrorList & "</span>TEXT OF LAST EMAIL SENT:<hr noshade><br />" & sBody

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Throw
        End Try

    End Sub

    Private Sub Email_Reminder_AdjudicationAssignments(ByVal EmailMessageTypesID As String)
        '====================================================================================================
        Dim dtAssign As DataTable, dr As DataRow, dtProd As DataTable, dtAdj As DataTable
        Dim sToNames As String, sToEmails As String, sFromEmail As String
        Dim sSubject As String = "", sBody As String = ""
        Dim iCountEmailsSent As Int16 = 0
        Dim sErrorList As String = ""
        '====================================================================================================
        Try
            sFromEmail = Me.ddlPK_FromEmailID.SelectedValue.ToString
            '=== Get Default Values
            'dtDefaults = Get_ApplicationDefaults ()
            '=== Get the list of Adjudicator Assignments that have not yet Closed
            dtAssign = DataAccess.Get_AdjudicatorAssignments(, , True)

            '=== Create message when no ballots are late.
            If dtAssign.Rows.Count = 0 Then sBody = "NO ADJUDICATION ASSIGNMENTS FOUND."

            For Each dr In dtAssign.Rows
                '=== Get Adjudicator Assignment Info on this late Ballot
                dtAdj = DataAccess.Find_AdjudicationsForUserLoginID(dr.Item("UserLoginID").ToString, dr.Item("PK_ScoringID").ToString)

                If dtAdj.Rows.Count > 0 Then
                    '=== Get Email Addresses of Adjudicator and all of their company Liaisions
                    sToEmails = Get_CompanyMemberEmails(dr.Item("FK_CompanyID_Adjudicator").ToString, dr.Item("PK_UserID").ToString, 3, Me.chkUseSecondaryEmailAddress.Checked, False)
                    sToEmails = sToEmails & Get_CompanyMemberEmails(dr.Item("PK_CompanyID").ToString, , 3, Me.chkUseSecondaryEmailAddress.Checked, False)
                    sToNames = Get_CompanyMemberEmails(dr.Item("FK_CompanyID_Adjudicator").ToString, dr.Item("PK_UserID").ToString, 3, Me.chkUseSecondaryEmailAddress.Checked, True)
                    sToNames = sToNames & Get_CompanyMemberEmails(dr.Item("PK_CompanyID").ToString, , 3, Me.chkUseSecondaryEmailAddress.Checked, True)
                    If sToNames.Length > 6 Then
                        sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1) 'removes last comma
                        sToNames = sToNames.Replace(",", "<li>")                        'Sets up for Ordered List <LI>
                        sToNames = "<hr noshade><B>NOTE:</b> This email has also been sent to all Liaisons for your Theatre Company and the Producing Theatre Company: " & _
                                        "<ul><li>" & sToNames & "</ul>"
                    End If

                    '=== Get Production Info for late Ballot to include in the EMail
                    dtProd = Get_Production(dtAdj.Rows(0)("PK_ProductionID").ToString)
                    If dtProd.Rows.Count > 0 Then
                        '=== CREATE EMAIL MESSAGE
                        sSubject = "NHTA Reminder to " & dr.Item("FirstName").ToString & " " & dr.Item("LastName").ToString & " for the upcoming Adjudication of '" & dr.Item("Title").ToString & "'"

                        sBody = "<font color=dark red><b>NOTE:</b> This is an Automatically generated email.  This email is intended to be a couresty reminder.</span><hr noshade>" & _
                                "<b>" & dr.Item("FirstName").ToString & " " & dr.Item("LastName").ToString & "</b>,<br /><br />" & _
                                "This is a courtesy reminder that you have been Assigned to Adjudicate the following Production:<br /> " & _
                                "<font color=dark red><B>IMPORTANT:</B></span> 'Re-Assignment Requests' are *only accepted* when submitted via the Adjudication Website.  DO NOT request reassignments via email as they *will not* be processed.<br />" & _
                                FormatEmailHTML_Production(dtProd) & _
                                FormatEmailHTML_AssignedAdjudicator(dtAdj) & _
                                "  Please confirm <u>your</u> 'Attendance Date' with the Producing Company <b>" & dtProd.Rows(0)("CompanyName").ToString & "</b><br />or <u>Request Re-Assignment via the Website</u> as soon as possible.<br /><br />" & _
                                "Thank you.<br /><br />" & _
                                FormatEmailHTML_AutomatedEmailDisclaimer() & _
                                sToNames & _
                                Common.Get_EmailFooter()

                        '=== Send and Log Email === 
                        If sToEmails.Length > 6 And sFromEmail.Length > 6 Then
                            SendCDOEmail(sFromEmail, sToEmails, Me.rblReceipientType.SelectedValue, sSubject, sBody, Me.chkHighPriority.Checked, True, Session("LoginID"), EmailMessageTypesID)
                            iCountEmailsSent = iCountEmailsSent + 1
                        Else
                            sErrorList = sErrorList & "Email Address not found for: " & FormatEmailHTML_AssignedAdjudicator(dtAdj) & "<hr noshade>"
                        End If
                    End If
                End If

                dtAdj = Nothing         'Clear the Datatable

            Next

            Me.pnlMain.Visible = False
            Me.pnlFinished.Visible = True
            Me.lblReciepients.Text = iCountEmailsSent.ToString
            Me.lblEmailSubject.Text = sSubject
            Me.lblHTMLEMailMessage.Text = "<Font color=""DarkRed"">" & sErrorList & "</span>TEXT OF LAST EMAIL SENT:<hr noshade><br />" & sBody

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Throw
        End Try

    End Sub

    Private Sub Email_Reminder_Nominations(ByVal EmailMessageTypesID As String)
        '====================================================================================================
        Dim dt As DataTable, dr As DataRow, dtProd As DataTable, dtNom As DataTable
        Dim sToNames As String, sToEmails As String, sFromEmail As String
        Dim sSubject As String = "", sBody As String = ""
        Dim iCountEmailsSent As Int16 = 0
        Dim sErrorList As String = ""
        '====================================================================================================
        Try
            sFromEmail = Me.ddlPK_FromEmailID.SelectedValue.ToString
            '=== Get Default Values
            'dtDefaults = DataAccess.Get_ApplicationDefaults 
            '=== Get the list of Adjudicator Assignments that have not yet Closed
            dt = DataAccess.Get_Productions(, , True)

            '=== Create message when no ballots are late.
            If dt.Rows.Count = 0 Then sBody = "NO PRODUCTION NOMINATIONS FOUND."

            For Each dr In dt.Rows
                If Not dr.Item("RequiresAdjudication").ToString = 0 Then

                    '=== Get Adjudicator Assignment Info on this late Ballot
                    dtNom = DataAccess.Get_Nomination(dr.Item("PK_ProductionID").ToString)

                    If dtNom.Rows.Count > 0 Then
                        '=== Get Email Addresses of Adjudicator and all of their company Liaisions
                        sToEmails = Get_CompanyMemberEmails(dtNom.Rows(0)("PK_CompanyID").ToString, , 3, Me.chkUseSecondaryEmailAddress.Checked, False)
                        sToNames = Get_CompanyMemberEmails(dtNom.Rows(0)("PK_CompanyID").ToString, , 3, Me.chkUseSecondaryEmailAddress.Checked, True)
                        If sToNames.Length > 6 Then
                            sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1) 'removes last comma
                            sToNames = sToNames.Replace(",", "<li>")                        'Sets up for Ordered List <LI>
                            sToNames = "<hr noshade><B>NOTE:</b> This email has been sent to all Liaisons for your Theatre Company: " & _
                                            "<ul><li>" & sToNames & "</ul>"

                            '=== Get Production Info for late Ballot to include in the EMail
                            dtProd = Get_Production(dtNom.Rows(0)("PK_ProductionID").ToString)
                            If dtProd.Rows.Count > 0 Then
                                '=== CREATE EMAIL MESSAGE
                                sSubject = "NHTA Nomination Reminder for your upcoming production of '" & dr.Item("Title").ToString & "'"

                                sBody = "<font color=dark red><b>NOTE:</b> This is an Automatically generated email.  This email is intended to be a couresty reminder.</span><hr noshade>" & _
                                        "<b>Dear Liaisons for " & dr.Item("CompanyName").ToString & "</b>,<br /><br />" & _
                                        "This is a courtesy reminder that your Nominations for your Production of <b>" & dr.Item("Title").ToString & "</b> are currently:<br /> " & _
                                        FormatEmailHTML_Nomination(dtNom) & _
                                        "The Production Information below will be used by Adjudicators to contact your theatre group to make reservations for the Adjudications:<br /><br />" & _
                                        FormatEmailHTML_Production(dtProd) & _
                                        "<I>If you have not already done so, please fill out <u>all</u> fields for Ticket information.</I><br /><br />" & _
                                        "Thank you.<br /><br />" & _
                                        FormatEmailHTML_AutomatedEmailDisclaimer() & _
                                        sToNames & _
                                        Common.Get_EmailFooter()

                                '=== Send and Log Email === 
                                If sToEmails.Length > 6 And sFromEmail.Length > 6 Then
                                    SendCDOEmail(sFromEmail, sToEmails, Me.rblReceipientType.SelectedValue, sSubject, sBody, Me.chkHighPriority.Checked, True, Session("LoginID"), EmailMessageTypesID)
                                    iCountEmailsSent = iCountEmailsSent + 1
                                End If
                            End If
                        Else
                            sErrorList = sErrorList & "ERROR: No Email Sent because of no Liaison Email Addresses for the Theatre Company: " & dr.Item("CompanyName").ToString & "<hr noshade>"
                        End If

                    End If

                    dtNom = Nothing         'Clear the Datatable
                End If
            Next

            Me.pnlMain.Visible = False
            Me.pnlFinished.Visible = True
            Me.lblReciepients.Text = iCountEmailsSent.ToString
            Me.lblEmailSubject.Text = sSubject
            Me.lblHTMLEMailMessage.Text = "<Font color=""DarkRed"">" & sErrorList & "</span>TEXT OF LAST EMAIL SENT:<hr noshade><br />" & sBody

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Throw
        End Try

    End Sub


End Class