Imports Adjudication.DataAccess
Imports Adjudication.CustomMail
Imports Adjudication.Common

Partial Public Class AdminUser
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim sNewPassword As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        iAccessLevel = Master.AccessLevel
        If iAccessLevel > 5 Or iAccessLevel = 0 Then Response.Redirect("Login.aspx") 'All Authorized Users (values 1-5) 

        Master.PageTitleLabel = Page.Title

        If iAccessLevel = 1 Then Me.btnViewBrowserHistory.Visible = True
        '===========================================================================================

        If Request.QueryString("LoginID") <> "" And iAccessLevel = 1 Then
            sLoginID = Request.QueryString("LoginID")
        Else
            sLoginID = Master.UserLoginID
        End If

        If Request.QueryString("ChangePassword") = "True" Then
            Me.pnlChangePassword.Visible = True
            Me.pnlUserData.Visible = False
            lblLoginID_ChangePassword.Text = sLoginID
            Me.lblOldPassword.Visible = True
            Me.txtOldPassword.Visible = True
        Else
            If Not IsPostBack Then

                Call Populate_DropDowns()
                Call SetPageControls()

                If Not (Request.QueryString("AddUser") = "True" And iAccessLevel = 1) Then
                    Call Populate_Data()
                End If
            End If
        End If

    End Sub

    Private Sub SetPageControls()

        If iAccessLevel = 1 Then
            If Request.QueryString("AddUser") = "True" Then
                Me.txtUserLoginID.Visible = True
                Me.lblUserLoginID.Visible = False
                Me.lblUserLoginID.Enabled = True
                Me.rblEmailInfo.SelectedIndex = 2
                Me.btnChangePassword.Visible = False
                Me.txtOrig_lblFK_CompanyID.Text = "0"
            End If

            Me.ddlFK_AccessLevelID.Visible = True
            Me.lblFK_AccessLevelID.Visible = False

            Me.ddlFK_CompanyID.Visible = True
            Me.lblFK_CompanyID.Visible = False

            Me.ddlActive.Visible = True
            Me.lblActive.Visible = False

            Me.rblEmailInfo.Visible = True
            Me.rblEmailInfo.Enabled = True

            Me.ddlFK_CompanyID.Visible = True
            Me.lblFK_CompanyID.Visible = False

            'Me.chkAdmin.Visible = True
            'Me.chkLiaison.Visible = True

            Me.ddlPK_UserStatusID.Visible = True
            Me.lblPK_UserStatusID.Visible = False

            Me.txtLastTrainingDate.Visible = True
            Me.lblLastTrainingDate.Visible = False

        End If

    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, dr As DataRow, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_AccessLevelID, AccessLevelName FROM UserAccessLevels"
        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            If Request.QueryString("AddUser") = "True" Then
                dr = dt.NewRow()                                        'If new user, add an empty row to force a selection
                dt.Rows.InsertAt(dr, 0)
            End If
            ddlFK_AccessLevelID.DataSource = dt
            ddlFK_AccessLevelID.DataValueField = "PK_AccessLevelID"
            ddlFK_AccessLevelID.DataTextField = "AccessLevelName"
            ddlFK_AccessLevelID.DataBind()
            'ddlFK_AccessLevelID.SelectedIndex = dt.Rows.Count - 2       ' Make the default value: Backup Adjudicator
        End If

        ' ====================================================================================================
        sSQL = "SELECT PK_UserStatusID, UserStatus FROM UserStatus"
        ddlPK_UserStatusID.DataSource = DataAccess.Run_SQL_Query(sSQL)
        ddlPK_UserStatusID.DataValueField = "PK_UserStatusID"
        ddlPK_UserStatusID.DataTextField = "UserStatus"
        ddlPK_UserStatusID.DataBind()
        If Request.QueryString("AddUser") = "True" Then
            ddlPK_UserStatusID.SelectedIndex = 1       'will default to "Active (if Trained)"
        End If
        ' ====================================================================================================
        sSQL = "SELECT PK_CompanyID, CompanyName FROM Company ORDER BY CompanyName"
        dt.Clear()
        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            If Request.QueryString("AddUser") = "True" Then
                dr = dt.NewRow()                'If new user, add an empty row to force a selection
                dt.Rows.InsertAt(dr, 0)
            End If
            ddlFK_CompanyID.DataSource = dt
            ddlFK_CompanyID.DataValueField = "PK_CompanyID"
            ddlFK_CompanyID.DataTextField = "CompanyName"
            ddlFK_CompanyID.DataBind()
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = " SELECT UserAccessLevels.AccessLevelName, PK_UserID, FK_AccessLevelID, FK_CompanyID, UserLoginID, FirstName, " & _
                "       LastName, Address, City, State, ZIP, PhonePrimary, PhoneSecondary, EmailPrimary,   " & _
                "       EmailSecondary, LastTrainingDate, Website, DateLastPasswordChange, LastLoginTime, DisabledFlag,  " & _
                "       BadLoginCount, UserInformation, Active, Users.LastUpdateByName, Users.LastUpdateByDate , " & _
                "       SecurityQuestion, SecurityAnswer, FK_UserStatusID " & _
                " FROM Users INNER JOIN UserAccessLevels ON FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID " & _
                " WHERE UserLoginID='" & sLoginID & "'"

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.txtPK_UserID.Text = dt.Rows(0)("PK_UserID").ToString
            Me.lblUserLoginID.Text = dt.Rows(0)("UserLoginID").ToString
            Me.txtUserLoginID.Text = dt.Rows(0)("UserLoginID").ToString
            Me.lblLoginID_ChangePassword.Text = dt.Rows(0)("UserLoginID").ToString

            Me.chkDisabledFlag.Checked = IIf(dt.Rows(0)("DisabledFlag") = "0", False, True)
            Me.lblFK_AccessLevelID.Text = dt.Rows(0)("AccessLevelName").ToString
            Me.ddlActive.SelectedValue = dt.Rows(0)("Active").ToString
            Me.lblActive.Text = Me.ddlActive.SelectedItem.Text
            Me.ddlFK_AccessLevelID.SelectedValue = dt.Rows(0)("FK_AccessLevelID").ToString
            Me.ddlPK_UserStatusID.SelectedValue = dt.Rows(0)("FK_UserStatusID").ToString
            Me.lblPK_UserStatusID.Text = Me.ddlPK_UserStatusID.SelectedItem.Text
            Me.ddlFK_CompanyID.SelectedValue = dt.Rows(0)("FK_CompanyID").ToString
            Me.txtOrig_lblFK_CompanyID.Text = dt.Rows(0)("FK_CompanyID").ToString   'for determining who to email when a user changes companies
            Me.lblFK_CompanyID.Text = ddlFK_CompanyID.SelectedItem.Text
            Me.txtFirstName.Text = dt.Rows(0)("FirstName").ToString
            Me.txtLastName.Text = dt.Rows(0)("LastName").ToString
            Me.txtAddress.Text = dt.Rows(0)("Address").ToString
            Me.txtCity.Text = dt.Rows(0)("City").ToString
            Me.txtState.Text = dt.Rows(0)("State").ToString
            Me.txtZIP.Text = dt.Rows(0)("ZIP").ToString
            Me.txtPhonePrimary.Text = dt.Rows(0)("PhonePrimary").ToString
            Me.txtPhoneSecondary.Text = dt.Rows(0)("PhoneSecondary").ToString
            Me.txtEmailPrimary.Text = dt.Rows(0)("EmailPrimary").ToString
            Me.txtEmailSecondary.Text = dt.Rows(0)("EmailSecondary").ToString
            Me.lblLastTrainingDate.Text = CDate(dt.Rows(0)("LastTrainingDate").ToString).ToShortDateString
            Me.txtLastTrainingDate.Text = CDate(dt.Rows(0)("LastTrainingDate").ToString).ToShortDateString
            Me.txtWebsite.Text = dt.Rows(0)("Website").ToString
            Me.lblDateLastPasswordChange.Text = dt.Rows(0)("DateLastPasswordChange").ToString
            Me.lblLastLoginTime.Text = dt.Rows(0)("LastLoginTime").ToString
            Me.lblBadLoginCount.Text = dt.Rows(0)("BadLoginCount").ToString
            Me.txtUserInformation.Text = dt.Rows(0)("UserInformation").ToString
            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            If dt.Rows(0)("SecurityQuestion").ToString.Length > 9 Then Me.ddlSecurityQuestion.SelectedValue = dt.Rows(0)("SecurityQuestion").ToString
            Me.txtSecurityAnswer.Text = dt.Rows(0)("SecurityAnswer").ToString

            If iAccessLevel = 1 And Master.UserLoginID <> Me.lblUserLoginID.Text Then
                Me.lblSecurityNote.Text = ">>> NOTE: Administrators should NOT set the security Question/Answer for other Users. <<<"
                Me.lblSecurityNote.Font.Italic = True
                Me.txtSecurityAnswer.TextMode = TextBoxMode.Password    'Hide answer from Admins
                'ReApply the value to the textbox using technique to bypass MS control for TextBoxMode.Password
                Me.txtSecurityAnswer.Attributes.Add("value", dt.Rows(0)("SecurityAnswer").ToString)
            End If

            Call SetPageControls()

        End If
    End Sub


    Private Sub lblUserLoginID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblUserLoginID.Click
        If iAccessLevel.ToString = "1" Then
            Me.lblUserLoginID.Visible = False
            Me.txtUserLoginID.Visible = True
            Me.txtUserLoginID.Text = Me.lblUserLoginID.Text
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        'Validate and Save user info
        '====================================================================================================
        If Save_User() = True Then
            Select Case rblEmailInfo.SelectedIndex
                Case 0
                    'do nothing
                Case 1
                    Call Email_User_Personal_Info(False, True)           'Email Update to user without a New Password
                Case 2
                    Call Email_User_Personal_Info(True, True)            'Email Update to user WITH a New Password
                Case Else
                    Me.lblErrors.Visible = True
                    Me.lblErrors.Text = "ERROR: UNKNOWN Selection in Radio button list. Please contact the Webmaster."
                    Exit Sub
            End Select

            If Request.QueryString("LoginID") <> "" Then
                Response.Redirect("AdminUserList.aspx?LoginID=" & sLoginID)
            End If

            If Request.QueryString("AddUser") = "True" Then
                Me.btnUpdate.Visible = False
            End If
        Else
            If Me.lblErrors.Visible = False Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: UNKNOWN. There was an Unknown Error when saving this record.  Please contact the Webmaster."
            End If
        End If

    End Sub

    Private Sub btnChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePassword.Click
        Me.pnlChangePassword.Visible = True
        Me.pnlUserData.Visible = False
    End Sub

    Private Sub btnCancel_PasswordChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel_PasswordChange.Click
        If Request.QueryString("ChangePassword") = "True" Then
            Response.Redirect("MainPage.aspx")
        Else
            Me.pnlChangePassword.Visible = False
            Me.pnlUserData.Visible = True
        End If

    End Sub

    Private Sub btnUpdate_PasswordChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate_PasswordChange.Click
        '====================================================================================================
        Dim dt As New DataTable
        '====================================================================================================
        Me.lblErrors_ChangePassword.Visible = False
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False

        If Me.txtOldPassword.Visible = True Then
            dt = DataAccess.Secure_UserValidate(sLoginID, Me.txtOldPassword.Text)

            If Not dt.Rows(0)("ReturnValue") = "0" Then
                Me.lblErrors_ChangePassword.Visible = True
                Me.lblErrors_ChangePassword.Text = dt.Rows(0)("ReturnUserInformation")
                Exit Sub
            End If
        End If

        If Me.txtNewPassword.Text = Me.txtConfirmPassword.Text Then
            dt = DataAccess.Secure_PasswordChange(Me.lblLoginID_ChangePassword.Text, Me.txtNewPassword.Text)

            If dt.Rows(0)("ReturnValue") = "0" Then
                If Request.QueryString("ChangePassword") = "True" Then
                    lblSuccess_ChangePassword.Visible = True
                    Me.pnlUserChangePassword.Visible = False                    ' hide controls to change pwd when successful
                Else
                    Me.pnlChangePassword.Visible = False
                    Me.pnlUserData.Visible = True
                    If Me.lblLoginID_ChangePassword.Text = sLoginID Then
                        lblSucessfulUpdate.Text = "Successfully Changed Password for: " & sLoginID
                    Else
                        lblSucessfulUpdate.Text = "Successfully Changed Password for: " & Me.lblLoginID_ChangePassword.Text
                    End If
                    lblSucessfulUpdate.Visible = True

                End If
            Else
                Me.lblErrors_ChangePassword.Text = dt.Rows(0)("ReturnUserInformation")
                Me.lblErrors_ChangePassword.Visible = True
            End If

        Else
            Me.lblErrors_ChangePassword.Text = "The Passwords do not match - please try again."
            Me.lblErrors_ChangePassword.Visible = True
        End If

    End Sub

    Private Function Validate_UserData() As Boolean
        '====================================================================================================
        ' Return Values:
        '   True = No Errors Found
        '   False = Errors Found (displays error message)
        '====================================================================================================
        Me.lblErrors.Text = ""                                          'Clear any previous Error Messages

        If Me.txtFirstName.Text = "" Or Me.txtLastName.Text = "" Then Me.lblErrors.Text = "ERROR: Please provide both a First and Last name."

        'only check the following if the User is ACTIVE
        If ddlActive.SelectedValue = 1 Then
            ' Admins dont have to have a Phone Number, if not their own account
            If Me.txtPhonePrimary.Text.Length = 0 And Not (iAccessLevel.ToString = "1" And Master.UserLoginID <> Me.lblUserLoginID.Text) Then Me.lblErrors.Text = "ERROR: Please provide a Primary Phone Number."

            If Me.txtEmailSecondary.Text.Length > 0 Then
                If Not ValidateEmailAddress(Me.txtEmailSecondary.Text) Then Me.lblErrors.Text = "ERROR MESSAGE: Invalid email address in Secondary Email Address: [" & Me.txtEmailSecondary.Text & "]"
            End If

            If Me.txtEmailPrimary.Text.Length > 0 Then
                If Not ValidateEmailAddress(Me.txtEmailPrimary.Text) Then Me.lblErrors.Text = "ERROR MESSAGE: Invalid email address in Primary Email Address: [" & Me.txtEmailPrimary.Text & "]"
            Else
                Me.lblErrors.Text = "ERROR: Please provide a Primary Email Address."
            End If

            If Not (iAccessLevel.ToString = "1" And Master.UserLoginID <> Me.lblUserLoginID.Text) Then
                If Me.ddlSecurityQuestion.SelectedValue.Length = 0 Or Me.txtSecurityAnswer.Text.Length = 0 Then Me.lblErrors.Text = "ERROR MESSAGE: Please Select a Security Question and Answer."
            End If
        End If

        If Request.QueryString("AddUser") = "True" Then
            If Me.ddlFK_CompanyID.SelectedIndex = 0 Then Me.lblErrors.Text = "ERROR: Please Select the Affilated Company."
            If Me.ddlFK_AccessLevelID.SelectedIndex = 0 Then Me.lblErrors.Text = "ERROR: Please Select the System Access Level."
        End If

        If Me.lblErrors.Text = "" Then
            Me.lblErrors.Visible = False
            Return True
        Else
            Me.lblErrors.Visible = True
            Return False
        End If

    End Function

    Private Function Save_User() As Boolean
        '====================================================================================================
        Dim sDataValues(30) As String, dt As DataTable
        '====================================================================================================
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False

        If Validate_UserData() = True Then
            Try
                If Request.QueryString("AddUser") = "True" Then
                    sNewPassword = Common.Get_RandomPassword(8)
                    sDataValues(22) = "0"
                Else
                    sDataValues(22) = Me.txtPK_UserID.Text
                End If

                '=== Set Values, and pass to updating SProc ===
                sDataValues(1) = Me.txtUserLoginID.Text
                sDataValues(2) = sNewPassword
                sDataValues(3) = Me.ddlFK_AccessLevelID.SelectedValue
                sDataValues(4) = Me.txtFirstName.Text
                sDataValues(5) = Me.txtLastName.Text
                sDataValues(6) = Me.txtAddress.Text
                sDataValues(7) = Me.txtCity.Text
                sDataValues(8) = Me.txtState.Text
                sDataValues(9) = Me.txtZIP.Text
                sDataValues(10) = Me.txtPhonePrimary.Text
                sDataValues(11) = Me.txtPhoneSecondary.Text
                sDataValues(12) = Me.txtEmailPrimary.Text
                sDataValues(13) = Me.txtEmailSecondary.Text
                sDataValues(14) = Me.txtLastTrainingDate.Text
                sDataValues(15) = Me.txtWebsite.Text
                sDataValues(16) = Me.txtUserInformation.Text
                sDataValues(17) = Me.ddlActive.SelectedValue.ToString
                sDataValues(18) = sLoginID
                sDataValues(19) = Me.ddlFK_CompanyID.SelectedValue
                sDataValues(20) = Me.ddlSecurityQuestion.SelectedValue
                sDataValues(21) = Me.txtSecurityAnswer.Text
                sDataValues(23) = Me.ddlPK_UserStatusID.SelectedValue.ToString

                dt = Secure_UserAddEdit(sDataValues)

                Select Case dt.Rows(0)("ReturnValue")
                    Case "0"
                        Me.txtOrig_lblFK_CompanyID.Text = Me.ddlFK_CompanyID.SelectedValue.ToString
                        Me.lblSucessfulUpdate.Visible = True
                        Me.lblSucessfulUpdate.Text = "Successfully Added User ID: " & Me.txtUserLoginID.Text
                        Save_User = True
                        Return True

                    Case "1"
                        Me.lblSucessfulUpdate.Visible = True
                        Me.lblSucessfulUpdate.Text = "Update Successful for Login ID: " & Me.txtUserLoginID.Text
                        Save_User = True
                        Return True

                    Case Else
                        Me.lblErrors.Text = "ERROR: " & dt.Rows(0)("ReturnUserInformation")
                        Me.lblErrors.Visible = True

                End Select

                Return True

            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: " & ex.Message
                Return False
            End Try
        End If

    End Function

    Private Function Email_User_Personal_Info(ByVal GenerateNewPassword As Boolean, Optional ByVal PopUpMessage As Boolean = False) As Boolean
        '============================================================================================
        Dim sSubject As String, sBody As String = ""
        Dim sHead As String, sInfo As String = ""
        Dim sEmailTo As String
        Dim bEmailAddressError As Boolean = True
        '============================================================================================
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False

        Try
            If GenerateNewPassword = True Or Request.QueryString("AddUser") = "True" Then
                If Request.QueryString("AddUser") = "True" Then
                    bEmailAddressError = DataAccess.Email_NewPassword(Me.txtUserLoginID.Text, True, True)
                Else
                    bEmailAddressError = DataAccess.Email_NewPassword(Me.txtUserLoginID.Text, False, True)
                End If
                If bEmailAddressError = True Then
                    Me.lblSucessfulUpdate.Text = "A NEW Password has been sucessfully generated and sent in an email<br /> from <b>" & ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString & "</b> to '" & Me.txtEmailPrimary.Text.ToUpper & "'"
                    If Me.txtEmailSecondary.Text.Length > 6 Then Me.lblSucessfulUpdate.Text = Me.lblSucessfulUpdate.Text & " and '" & Me.txtEmailSecondary.Text & "'"
                    Me.lblSucessfulUpdate.Visible = True
                Else
                    Me.lblErrors.Visible = True
                    Me.lblErrors.ForeColor = System.Drawing.Color.Red
                    Me.lblErrors.Text = Me.lblErrors.Text & "ERROR MESSAGE: Unknown Error in sending email and generating new pasword"
                    Exit Function
                End If
            Else
                bEmailAddressError = DataAccess.Email_NewPassword(Me.txtUserLoginID.Text, False, False)

                '============================================================================================
                'If user changed theatre companies, notify that companies members.
                If Me.txtOrig_lblFK_CompanyID.Text <> Me.ddlFK_CompanyID.SelectedValue.ToString Then
                    sSubject = "NHTA Adjudication Website has associated " & Me.txtFirstName.Text & " " & Me.txtLastName.Text & " with " & Me.ddlFK_CompanyID.SelectedItem.ToString
                    sHead = "You have received this email because you are listed as an NHTA active member of <b>" & Me.ddlFK_CompanyID.SelectedItem.ToString & "</b>.<br /><br />"
                    sHead = sHead & "This is to let you know that <b>" & Me.txtFirstName.Text & " " & Me.txtLastName.Text & "</b> "
                    sHead = sHead & "has been associated with <b>" & Me.ddlFK_CompanyID.SelectedItem.ToString & "</b>.<br /><br />"

                    'Create Email body message for the Company members w/o LoginID & Pwd info
                    sBody = sHead & sInfo & Common.Get_EmailFooter

                    ' for New Users Get all Liaison, Primary and Backup Adjudicator email addresses
                    sEmailTo = Get_CompanyMemberEmails(Me.ddlFK_CompanyID.SelectedValue.ToString, Get_UserID(Me.txtUserLoginID.Text), 5, False)
                    SendCDOEmail(ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString, sEmailTo, False, sSubject, sBody, True, , Session("LoginID"))
                End If

                '============================================================================================
                Me.lblSucessfulUpdate.Text = "Personal information has been sent in an email to '" & Me.txtEmailPrimary.Text.ToUpper & "' "
                If Me.txtEmailSecondary.Text.Length > 6 Then Me.lblSucessfulUpdate.Text = Me.lblSucessfulUpdate.Text & " and '" & Me.txtEmailSecondary.Text & "'"
                Me.lblSucessfulUpdate.Visible = True

                '=====> for testing <=====
                'Me.lblSucessfulUpdate.Text = Me.lblSucessfulUpdate.Text & "<br /><br />" & sBody
            End If

            If PopUpMessage = True Then
                '    Response.Write("<script language=javascript>")
                '    Response.Write("alert('" & Me.lblSucessfulUpdate.Text & " ');")
                '    Response.Write("</script>")
            End If

        Catch ex As Exception
            'Throw
            Me.lblErrors.Visible = True
            Me.lblErrors.ForeColor = System.Drawing.Color.Red
            Me.lblErrors.Text = "<P><B>ERROR</B>: An internal Mail Server error prevented the email from being Generated.</P>"
            Me.lblErrors.Text = Me.lblErrors.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Function


    Private Sub btnViewBrowserHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewBrowserHistory.Click
        Response.Write("<script language=javascript>")
        Response.Write("window.open('Reports/UserBrowserHistory.aspx?UserID=" & Me.txtPK_UserID.Text & "','Microscript','status=yes ,toolbar=no,directories=no,menubar=no,scrollbars=yes,resizable=yes,location=no,top=50,left=250,height=800,width=1000');")
        Response.Write("</script>")

    End Sub



End Class