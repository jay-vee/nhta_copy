Imports Adjudication.DataAccess

Public Class AdminLoginSettings
    Inherits System.Web.UI.Page

    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lblSucessfulUpdate As System.Web.UI.WebControls.Label
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastUpdateByName As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastUpdateByDate As System.Web.UI.WebControls.Label
    Protected WithEvents btnUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents ddlExpirePasswords As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlDisableExpiredAccounts As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlAllowPasswordReuse As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtMinLoginIDLength As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMaxLoginIDLength As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMinPasswordLength As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMaxPasswordLength As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNumOfLoginAttempts As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExpirePasswordsAfterXDays As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExpireAccountsAfterXDays As System.Web.UI.WebControls.TextBox

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Not IsPostBack Then
            GetDefaultValues()
        End If

    End Sub

    Private Sub GetDefaultValues()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT TOP 1 * FROM UserOptions "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.txtMinLoginIDLength.Text = dt.Rows(0)("MinLoginIDLength").ToString
            Me.txtMaxLoginIDLength.Text = dt.Rows(0)("MaxLoginIDLength").ToString
            Me.txtNumOfLoginAttempts.Text = dt.Rows(0)("NumOfLoginAttempts").ToString
            Me.txtMinPasswordLength.Text = dt.Rows(0)("MinPasswordLength").ToString
            Me.txtMaxPasswordLength.Text = dt.Rows(0)("MaxPasswordLength").ToString
            'Me.RequirePasswords.Text = dt.Rows(0)("RequirePasswords").ToString
            Me.ddlAllowPasswordReuse.SelectedValue = dt.Rows(0)("AllowPasswordReuse").ToString
            Me.ddlExpirePasswords.SelectedValue = dt.Rows(0)("ExpirePasswords").ToString
            Me.txtExpirePasswordsAfterXDays.Text = dt.Rows(0)("ExpirePasswordsAfterXDays").ToString
            Me.ddlDisableExpiredAccounts.SelectedValue = dt.Rows(0)("DisableExpiredAccounts").ToString
            Me.txtExpireAccountsAfterXDays.Text = dt.Rows(0)("ExpireAccountsAfterXDays").ToString
            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(20) As String, dt As DataTable
        '====================================================================================================

        sDataValues(1) = Me.txtMinLoginIDLength.Text
        sDataValues(2) = Me.txtMaxLoginIDLength.Text
        sDataValues(3) = Me.txtNumOfLoginAttempts.Text
        sDataValues(4) = Me.txtMinPasswordLength.Text
        sDataValues(5) = Me.txtMaxPasswordLength.Text
        sDataValues(6) = "0" 'Me.RequirePasswords.Text  'THIS SITE MUST USE PASSWORDS
        sDataValues(7) = Me.ddlAllowPasswordReuse.SelectedValue
        sDataValues(8) = Me.ddlExpirePasswords.SelectedValue
        sDataValues(9) = Me.txtExpirePasswordsAfterXDays.Text
        sDataValues(10) = Me.ddlDisableExpiredAccounts.SelectedValue
        sDataValues(11) = Me.txtExpireAccountsAfterXDays.Text
        sDataValues(12) = Master.UserLoginID

        dt = Secure_UpdateUserOptions(sDataValues)
        If dt.Rows(0)("ReturnValue") = "0" Then
            Me.lblErrors.Visible = False
            Me.lblSucessfulUpdate.Visible = True
        Else
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = dt.Rows(0)("ReturnUserInformation")
            Me.lblSucessfulUpdate.Visible = False
        End If

    End Sub

End Class
