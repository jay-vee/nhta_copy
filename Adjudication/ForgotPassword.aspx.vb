Imports Adjudication.DataAccess
Imports Adjudication.CustomMail

Public Class ForgotPassword
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMainpageApplicationDesc As System.Web.UI.WebControls.Label
    Protected WithEvents hlnkLogin As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtEmailPrimary As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents pnlRequestNewPassword As System.Web.UI.WebControls.Panel
    Protected WithEvents lblResults As System.Web.UI.WebControls.Label
    Protected WithEvents pnlResults As System.Web.UI.WebControls.Panel
    Protected WithEvents HyperLinkLogin As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents txtSecurityAnswer As System.Web.UI.WebControls.TextBox
    Protected WithEvents pnlSecurityQuestion As System.Web.UI.WebControls.Panel
    Protected WithEvents lblSecurityQuestion As System.Web.UI.WebControls.Label
    Protected WithEvents btnSecurityCheck As System.Web.UI.WebControls.Button
    Protected WithEvents lblErrorSecurity As System.Web.UI.WebControls.Label
    Protected WithEvents txtErrorCount As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtInfobox As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUserLoginID As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If Not IsPostBack Then
            'GetDefaultValues()
        End If

    End Sub




    Private Sub GetDefaultValues()
        '====================================================================================================
        'Dim dt As DataTable, sSQL As String
        ''====================================================================================================
        'sSQL = "SELECT TOP 1 * FROM ApplicationDefaults "

        'dt = DataAccess.Run_SQL_Query(sSQL)

        'If dt.Rows.Count > 0 Then

        'End If
    End Sub





    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        '============================================================================================
        Dim dt As DataTable
        Dim sBody As String = ""
        'Dim sSubject As String, sNewPassword As String
        Dim bEmailAddressError As Boolean = True
        '============================================================================================

        Try
            ' Validate that the email address at least could be a real address.
            If ValidateEmailAddress(Me.txtEmailPrimary.Text) = False Then
                Me.lblErrors.Visible = True
            Else
                ' Verify that the Email Address is on the system AND 
                ' return the Login ID and user name for the supplied Email address 
                dt = DataAccess.Find_UserEmail(Me.txtEmailPrimary.Text)

                If CInt(dt.Rows(0)("ReturnValue").ToString) = 0 Then
                    Me.lblErrors.Visible = False
                    Me.txtUserName.Text = dt.Rows(0)("ExistingUsername").ToString
                    'Me.txtPK_UserID.Text = dt.Rows(0)("LoginID").ToString
                    Me.txtUserLoginID.Text = dt.Rows(0)("LoginID").ToString

                    ' If user has not setup a Question, automatically email the password
                    If dt.Rows(0)("SecurityQuestion").ToString.Length = 0 Then
                        ' SEND EMail
                        Call SendNewPasswordEmail()
                    Else
                        Me.lblSecurityQuestion.Text = dt.Rows(0)("SecurityQuestion").ToString
                        Me.txtInfobox.Text = dt.Rows(0)("SecurityAnswer").ToString  ' put in global variable for reference in part 2
                        Me.pnlSecurityQuestion.Visible = True
                        Me.pnlRequestNewPassword.Visible = False
                    End If

                Else
                    If CInt(dt.Rows(0)("ReturnValue").ToString) = 1 Then
                        Me.pnlRequestNewPassword.Visible = False
                        Me.pnlResults.Visible = True
                        Me.lblResults.Visible = True
                        Me.lblResults.ForeColor = System.Drawing.Color.Red
                        Me.lblResults.Text = "Error: The email address '<B>" & Me.txtEmailPrimary.Text & "</B>' Matches more than 1 Login ID.  Cannot reset due to multiple matches.  Please Email support at <A HREF='mailto:Support@NHTheatreAwards.org'>Support@NHTheatreAwards.org</A> for assistance."
                    Else
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "Error: The email address '<B>" & Me.txtEmailPrimary.Text & "</B>' was not found.  Please review your email address and try again."
                    End If
                End If
            End If

        Catch ex As Exception
            'Throw
            Me.pnlRequestNewPassword.Visible = False
            Me.pnlResults.Visible = True
            Me.lblResults.Visible = True
            Me.lblResults.ForeColor = System.Drawing.Color.Red
            Me.lblResults.Text = Me.lblResults.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Sub

    Private Sub btnSecurityCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSecurityCheck.Click
        '============================================================================================
        ' Validate that the Security Answers Match
        '============================================================================================
        If Me.txtSecurityAnswer.Text.Length = 0 Then
            Me.lblErrorSecurity.Visible = True
            Me.lblErrorSecurity.Text = "Please enter in your Security Question Answer."
        Else
            If CInt(txtErrorCount.Text) >= 3 Then
                Me.pnlRequestNewPassword.Visible = False
                Me.pnlSecurityQuestion.Visible = False
                Me.pnlResults.Visible = True
                Me.lblResults.Visible = True
                Me.lblResults.ForeColor = System.Drawing.Color.Red
                Me.lblResults.Text = "Error - to many failed attempts. <br />Please contact <B>Support@NHTheatreAwards.org</B> for assistance. <br />Thank you."
                Exit Sub
            End If

            If Me.txtInfobox.Text.Trim.ToUpper <> Me.txtSecurityAnswer.Text.Trim.ToUpper Then
                Me.lblErrorSecurity.Visible = True
                Me.lblErrorSecurity.Text = "Please enter in your Security Question Answer."
                Me.txtErrorCount.Text = Str(CInt(txtErrorCount.Text) + 1)   ' track errors
            Else
                ' Security Answers Match!
                Me.lblErrorSecurity.Visible = False
                ' SEND EMail
                Call SendNewPasswordEmail()
            End If
        End If

    End Sub

    Private Sub SendNewPasswordEmail()
        '============================================================================================
        Dim sBody As String = ""
        'Dim sSubject As String, sNewPassword As String
        Dim bEmailAddressError As Boolean = True
        '============================================================================================

        Me.pnlRequestNewPassword.Visible = False
        Me.pnlSecurityQuestion.Visible = False
        Me.pnlResults.Visible = True

        Try
            If DataAccess.Email_NewPassword(Me.txtUserLoginID.Text) = True Then
                ' Let user know that the email was sent.
                Me.lblResults.Text = Me.txtUserName.Text & ", a New Password has been sucessfully generated and sent in an email<br /> from 'Support@NHTAdjuducation.org' to your email address '" & Me.txtEmailPrimary.Text.ToUpper & "'"
                Me.lblResults.Visible = True
            Else
                Me.lblResults.ForeColor = System.Drawing.Color.Red
                Me.lblResults.Text = "<P><B>ERROR</B>: An unknown error prevented the email from being Generated.</P><P> "
                Me.lblResults.Visible = True
            End If

        Catch ex As Exception
            Me.lblResults.Visible = True
            Me.lblResults.ForeColor = System.Drawing.Color.Red
            Me.lblResults.Text = Me.lblResults.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
            'Throw ex
        End Try
    End Sub



End Class
