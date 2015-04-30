Imports Adjudication.DataAccess
Imports Adjudication.CustomMail
Imports Adjudication.Common

Partial Public Class AdminEmail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DropDowns()
            Me.ftbEmailBody.Text = "<FONT face=Verdana size=2>"

            '=== Check QueryString values ===
            If Not Request.QueryString("ELogID") = Nothing Then
                If Not Request.QueryString("ELogID") = "0" Then
                    Call ResendEmail(CInt(Request.QueryString("ELogID")))
                End If
            End If

        End If

    End Sub

    Private Sub ResendEmail(ByVal EmailLogID As Integer)
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================

        sSQL = "SELECT FK_EmailMessageTypesID, EmailFrom, EmailTo, EmailSubject, EmailBody FROM EmailLog WHERE PK_EmailLogID=" & EmailLogID.ToString
        sSQL = sSQL & " ORDER BY EmailFrom "
        dt = DataAccess.Run_SQL_Query(sSQL)
        If dt.Rows.Count > 0 Then
            Me.ddlPK_FromEmailID.DataValueField = dt.Rows(0).Item("EmailFrom").ToString
            Me.txtEmailSubject.Text = dt.Rows(0).Item("EmailSubject").ToString
            Me.ftbEmailBody.Text = dt.Rows(0).Item("EmailBody").ToString
            Me.txtCustomEmailAddresses.Text = dt.Rows(0).Item("EmailTo").ToString
        Else
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Email Log entry not found.  Please contact your system administrator for assistance."
        End If

    End Sub


    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        dt = DataAccess.Get_Productions("Title", , True, True)
        If dt.Rows.Count > 0 Then
            Me.ddlPK_ProductionID.DataSource = dt
            Me.ddlPK_ProductionID.DataValueField = "PK_ProductionID"
            Me.ddlPK_ProductionID.DataTextField = "ProductionInfo"
            Me.ddlPK_ProductionID.DataBind()
        End If

        dt.Clear()
        dt = DataAccess.Get_EmailFromAddresses(False)
        If dt.Rows.Count > 0 Then
            Me.ddlPK_FromEmailID.DataSource = dt
            Me.ddlPK_FromEmailID.DataValueField = "EmailFromAddress"
            Me.ddlPK_FromEmailID.DataTextField = "EmailFromAddress"
            Me.ddlPK_FromEmailID.DataBind()
        End If

        '====================================================================================================
        sSQL = " SELECT 1 as SortOrder, PK_UserID as dbID, LastName + ', ' + FirstName as Name FROM Users WHERE Active = 1 AND (NOT (EmailPrimary IS NULL)) "
        sSQL = sSQL & " UNION SELECT 2 as SortOrder, PK_CompanyID as dbID, '{c} ' + CompanyName as Name FROM Company WHERE (Company.ActiveCompany = 1) "
        sSQL = sSQL & " ORDER BY SortOrder, Name "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lstPK_UserID.DataSource = dt
            Me.lstPK_UserID.DataValueField = "dbID"
            Me.lstPK_UserID.DataTextField = "Name"
            Me.lstPK_UserID.DataBind()
        End If

    End Sub

    Private Sub lbtnViewEmailAddresses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnViewEmailAddresses.Click
        Try
            Call Get_EmailAddresses()
            Me.txtTOEmailAddresses.Visible = True
            'Me.txtTOEmailAddresses.Height = Unit.Pixel(140)

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Sub

    Public Sub btnSendEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendEmail.Click
        '====================================================================================================
        Dim sSubject As String, sBody As String = ""
        '============================================================================================
        Try
            Me.lblErrors.Visible = False

            If Me.txtEmailSubject.Text.Length = 0 Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "Error: You must provide a Subject to send emails."
                Exit Sub
            End If

            If Me.ftbEmailBody.Text.Length = 0 Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "Error: You must provide a message in the Body of the Email."
                Exit Sub
            End If

            Call Get_EmailAddresses()

            If Me.txtTOEmailAddresses.Text.Length = 0 Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "Error: You must choose Reciepients of the Email."
                Exit Sub
            End If

            '--------------------------------------------------------------------------------------------------------------------------------------------------------
            sSubject = Me.txtEmailSubject.Text
            sBody = Me.ftbEmailBody.Text

            ' Send the Email in HTML Format
            SendCDOEmail(Me.ddlPK_FromEmailID.SelectedValue.ToString, Me.txtTOEmailAddresses.Text, Me.rblReceipientType.SelectedValue, sSubject, sBody, Me.chkHighPriority.Checked, True, Session("LoginID"), 1, CBool(rblEmailsPerRecipient.SelectedValue))

            ' Let user know that the email was sent.
            Me.pnlSendEmail.Visible = False
            Me.pnlFinished.Visible = True
            Me.lblEmailSubject.Text = Me.txtEmailSubject.Text
            Me.lblReciepients.Text = Me.txtTOEmailAddresses.Text & "<br />"

        Catch ex As Exception
            'Throw
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try
    End Sub

    Private Sub btnSendMoreEmails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendMoreEmails.Click
        Response.Redirect("AdminEmail.aspx")
    End Sub

    Private Sub ddlPK_ProductionID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPK_ProductionID.SelectedIndexChanged

        If Me.ftbEmailBody.Text.Length > 1 Then
            Me.ftbEmailBody.Text = Me.ftbEmailBody.Text & "<br />" & FormatEmailHTML_Production(Me.ddlPK_ProductionID.SelectedValue.ToString)
        Else
            Me.ftbEmailBody.Text = "<br />" & FormatEmailHTML_Production(Me.ddlPK_ProductionID.SelectedValue.ToString)
        End If

        If Me.txtEmailSubject.Text.Length > 1 Then
            Me.txtEmailSubject.Text = Me.txtEmailSubject.Text & " " & Me.ddlPK_ProductionID.SelectedItem.ToString
        Else
            Me.txtEmailSubject.Text = "Production " & Me.ddlPK_ProductionID.SelectedItem.ToString
        End If

    End Sub


    Private Sub Get_EmailAddresses()
        '====================================================================================================
        Dim dt As DataTable
        Dim sSQL As String = "", sWhere As String = "", sAccessLevels As String = "", sSQLUnion As String = ""
        Dim sEmails As String = ""
        Dim i As Int16 = 0
        '====================================================================================================
        Me.lblErrors.Visible = False
        Me.txtTOEmailAddresses.Text = ""                        ' Clear any previously set values

        ' ---- Add WHERE Conditions ----------------------------------------------------------------------------------------------------------------------------
        If chklstAddressTo.Items(0).Selected = True Then        ' "Users AND Companies"
            sWhere = sWhere & " AND Active = 1 "                ' Default will get all addresses
        End If

        If chklstAddressTo.Items(1).Selected = True Then        ' "Users"
            sWhere = sWhere & " AND Active = 1 "                ' Default will get all addresses
        End If

        If chklstAddressTo.Items(3).Selected = True Then        ' "Administrators"
            sAccessLevels = sAccessLevels & " (FK_AccessLevelID = 1) "
        End If

        If chklstAddressTo.Items(4).Selected = True Then        ' "Liaisons"
            If sAccessLevels.Length = 0 Then
                sAccessLevels = sAccessLevels & " (FK_AccessLevelID = 2 OR FK_AccessLevelID = 3) "
            Else
                sAccessLevels = sAccessLevels & " OR (FK_AccessLevelID = 2 OR FK_AccessLevelID = 3) "
            End If
        End If

        If chklstAddressTo.Items(5).Selected = True Then        ' "PrimaryAdjudicators"
            If sAccessLevels.Length = 0 Then
                sAccessLevels = sAccessLevels & " (FK_AccessLevelID = 4) "
            Else
                sAccessLevels = sAccessLevels & " OR (FK_AccessLevelID = 4) "
            End If
        End If

        If chklstAddressTo.Items(6).Selected = True Then        ' "BackupAdjudicators"
            If sAccessLevels.Length = 0 Then
                sAccessLevels = sAccessLevels & " (FK_AccessLevelID = 5) "
            Else
                sAccessLevels = sAccessLevels & " OR (FK_AccessLevelID = 5) "
            End If
        End If

        If chklstAddressTo.Items(0).Selected = True Then        ' "Users AND Companies"
            sSQLUnion = sSQLUnion & " UNION ALL SELECT EmailAddress FROM Company WHERE (EmailAddress IS NOT NULL) AND (LEN(EmailAddress) > 0) AND Company.ActiveCompany = 1 "
        End If

        If sAccessLevels.Length > 0 Then sWhere = " AND (" & sAccessLevels & ") " ' Add Access Levels to WHERE clause

        ' ---- Set Main Query for Users ------------------------------------------------------------------------------------------------------------------------
        If sWhere.Length > 6 Then
            sSQL = "SELECT EmailPrimary as EmailAddress FROM Users WHERE (Users.Active = 1) AND (EmailPrimary IS NOT NULL AND LEN(EmailPrimary) > 0) " & sWhere
            ' ---- Include Secondary Email address ------------------------------------------------------------------------------------------------------------------------
            If Me.chkUseSecondaryEmailAddress.Checked = True Then
                sSQLUnion = sSQLUnion & " UNION ALL SELECT EmailSecondary as EmailAddress FROM Users WHERE (Users.Active = 1) AND (EmailSecondary IS NOT NULL AND LEN(EmailSecondary) > 0 ) "
            End If
        End If

        ' ---- Set Main Query for Individual/Company list ------------------------------------------------------------------------------------------------------------------------
        If Me.lstPK_UserID_Selected.Rows > 0 Then                        ' Individual/Company list
            Dim sUserIDList As String = "", sCompanyIDList As String = ""

            For i = Me.lstPK_UserID_Selected.Items.Count - 1 To 0 Step -1
                If Not Me.lstPK_UserID_Selected.Items(i).ToString.Substring(0, 3) = "{c}" Then
                    sUserIDList = sUserIDList & " OR (PK_UserID= " & Me.lstPK_UserID_Selected.Items(i).Value.ToString & ") "
                Else
                    sCompanyIDList = sCompanyIDList & " OR (PK_CompanyID= " & Me.lstPK_UserID_Selected.Items(i).Value.ToString & ") "
                End If
            Next
            If sUserIDList.Length > 0 Then sUserIDList = " AND (" & sUserIDList.Substring(3) & ") "
            If sCompanyIDList.Length > 0 Then sCompanyIDList = " AND (" & sCompanyIDList.Substring(3) & ") "

            If sUserIDList.Length > 0 Then
                If sSQL.Length > 0 Then
                    sSQLUnion = sSQLUnion & " UNION ALL SELECT EmailPrimary as EmailAddress FROM Users WHERE (Users.Active = 1) AND (EmailPrimary IS NOT NULL AND LEN(EmailPrimary) > 0) " & sUserIDList
                Else
                    sSQL = sSQL & " SELECT EmailPrimary as EmailAddress FROM Users WHERE (Users.Active = 1) AND (EmailPrimary IS NOT NULL AND LEN(EmailPrimary) > 0)  " & sUserIDList
                End If

                If Me.chkUseSecondaryEmailAddress.Checked = True And sUserIDList.Length > 0 Then
                    sSQLUnion = sSQLUnion & " UNION ALL SELECT EmailSecondary as EmailAddress FROM Users WHERE (Users.Active = 1) AND (EmailSecondary IS NOT NULL  AND LEN(EmailSecondary) > 0)  " & sUserIDList
                End If
            End If

            If sCompanyIDList.Length > 0 Then
                If sSQL.Length > 0 Then
                    sSQLUnion = sSQLUnion & " UNION ALL SELECT EmailAddress FROM Company WHERE (Company.ActiveCompany = 1) AND (EmailAddress IS NOT NULL) AND (LEN(EmailAddress) > 0) " & sCompanyIDList
                Else
                    sSQL = sSQL & " SELECT EmailAddress FROM Company WHERE (Company.ActiveCompany = 1) AND (EmailAddress IS NOT NULL) AND (LEN(EmailAddress) > 0)  " & sCompanyIDList
                End If
            End If

        End If

        ' ---- Theatre Company ------------------------------------------------------------------------------------------------------------------------
        If chklstAddressTo.Items(2).Selected = True Then        '    "All Companies" 
            If sSQL.Length > 0 Then
                sSQLUnion = sSQLUnion & " UNION ALL SELECT EmailAddress FROM Company WHERE (Company.ActiveCompany = 1) AND (EmailAddress IS NOT NULL) AND (LEN(EmailAddress) > 0) "
            Else
                sSQL = sSQL & " SELECT EmailAddress FROM Company WHERE (Company.ActiveCompany = 1) AND (EmailAddress IS NOT NULL) AND (LEN(EmailAddress) > 0) "
            End If
        End If

        '=== Get the Email Recipients =================================================================
        Try
            If sSQL.Length > 6 Then
                dt = DataAccess.Run_SQL_Query(sSQL & sSQLUnion)
                Dim foundRows() As DataRow = dt.Select("", "EmailAddress")
                'If dt.Rows.Count > 0 Then
                '    Dim i As Int16 = 0
                '    sEmails = sEmails & dt.Rows(0)("EmailAddress")
                '    For i = 1 To dt.Rows.Count - 1
                '        sEmails = sEmails & ", " & dt.Rows(i)("EmailAddress")
                '    Next
                'End If
                If dt.Rows.Count > 0 Then
                    sEmails = sEmails & foundRows(0)("EmailAddress")
                    Dim LastEmailAddress As String = sEmails

                    For i = 0 To foundRows.GetUpperBound(0)
                        If LastEmailAddress <> foundRows(i)("EmailAddress") Then
                            sEmails = sEmails & ", " & foundRows(i)("EmailAddress")
                            LastEmailAddress = foundRows(i)("EmailAddress")
                        End If
                    Next
                End If
            End If

            If Me.txtCustomEmailAddresses.Text.Length > 6 Then
                ' validate custom email addresses entered using custom function: ValidateEmailAddress 
                Dim CustomAddresses() As String = Me.txtCustomEmailAddresses.Text.Split(",")
                For Each Address As String In CustomAddresses
                    If ValidateEmailAddress(Address) = False Then
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "<P>ERROR MESSAGE: Invalid email address in Custom Emails Addresses: [" & Me.txtCustomEmailAddresses.Text & "] </p>"
                        Exit Sub
                    End If
                Next
            End If

            If sEmails.Length = 0 Then
                sEmails = Me.txtCustomEmailAddresses.Text
            Else
                If Me.txtCustomEmailAddresses.Text.Length > 5 Then
                    sEmails = sEmails & ", " & Me.txtCustomEmailAddresses.Text
                End If
            End If

            '=== Check if Valid Email addresses found and Display =================================================================================================
            If sEmails.TrimEnd.Length < 6 Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "<P>ERROR MESSAGE: Invalid email address in Custom Emails Addresses: [" & Me.txtCustomEmailAddresses.Text & "] </p>"
                Exit Sub
            Else
                Me.txtTOEmailAddresses.Text = sEmails.TrimEnd
                If i > 3 Then Me.txtTOEmailAddresses.Height = Unit.Pixel(((i / 5) * 20) + 20)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub btnEmailUsers_Remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmailUsers_Remove.Click
        For i As Integer = Me.lstPK_UserID_Selected.Items.Count - 1 To 0 Step -1
            If Me.lstPK_UserID_Selected.Items(i).Selected = True Then
                Dim li As ListItem = Me.lstPK_UserID_Selected.Items(i)
                Me.lstPK_UserID.Items.Add(li)
                Me.lstPK_UserID_Selected.Items.Remove(li)
            End If
        Next
    End Sub

    Private Sub btnEmailUsers_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmailUsers_Add.Click
        For i As Integer = Me.lstPK_UserID.Items.Count - 1 To 0 Step -1
            If Me.lstPK_UserID.Items(i).Selected = True Then
                Dim li As ListItem = Me.lstPK_UserID.Items(i)
                Me.lstPK_UserID_Selected.Items.Add(li)
                Me.lstPK_UserID.Items.Remove(li)
            End If
        Next
    End Sub


End Class