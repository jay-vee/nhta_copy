Imports Adjudication.DataAccess
Imports Adjudication.CustomMail

Public Class MessageToAdmin
    Inherits System.Web.UI.Page

    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lblSucessfulUpdate As System.Web.UI.WebControls.Label
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents pnlUserData As System.Web.UI.WebControls.Panel
    Protected WithEvents btnUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents lblFullName As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmailPrimary As System.Web.UI.WebControls.Label
    Protected WithEvents lblPhonePrimary As System.Web.UI.WebControls.Label
    Protected WithEvents txtSubject As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents ftbMessage As FreeTextBoxControls.FreeTextBox

    Dim iAccessLevel As Int16
    Dim sLoginID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If (Not iAccessLevel > 0) Or iAccessLevel = -99 Then Response.Redirect("Login.aspx")
        sLoginID = Session("LoginID")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Not IsPostBack Then
            If iAccessLevel = -99 Then
                Call RequestUserAccess()
            Else
                Call Populate_Data()
            End If
        End If

    End Sub


    Private Sub RequestUserAccess()
        Me.lblFullName.Text = "NEW User Request"
        Me.lblPhonePrimary.Visible = False
        Me.lblEmailPrimary.Visible = False
        Me.txtSubject.Text = "NEW User Request"
        Me.txtSubject.Enabled = False

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = " SELECT * FROM Users " & _
                " WHERE UserLoginID='" & sLoginID & "'"

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblFullName.Text = dt.Rows(0)("FirstName").ToString.ToUpper & " " & dt.Rows(0)("LastName").ToString.ToUpper
            Me.lblPhonePrimary.Text = dt.Rows(0)("PhonePrimary").ToString
            Me.lblEmailPrimary.Text = dt.Rows(0)("EmailPrimary").ToString
            Me.txtSubject.Text = Me.txtSubject.Text & dt.Rows(0)("FirstName").ToString.ToUpper & " " & dt.Rows(0)("LastName").ToString.ToUpper
        End If
    End Sub


    Public Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess
        Dim sSubject As String, sBody As String = ""
        '====================================================================================================

        If Me.txtSubject.Text = "" Or Me.txtSubject.Text.Length <= 3 Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide a Subject."
            Exit Sub
        End If
        '============================================================================================
        Try
            Me.lblErrors.Visible = False
            Call Save_AdminMessage(sLoginID, Me.txtSubject.Text, "")       'Me.ftbMessage.Text

            sSubject = Me.txtSubject.Text
            sBody = Me.ftbMessage.Text

            ' Send the Email 
            SendCDOEmail(ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString, Me.lblEmailPrimary.Text & ", " & ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString, False, sSubject, sBody, False, True, Session("LoginID"))

            ' Let user know that the email was sent.
            Me.pnlUserData.Visible = False
            Me.lblSucessfulUpdate.Visible = True

        Catch ex As Exception
            'Throw
            Me.pnlUserData.Visible = False
            Me.lblErrors.Text = "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
        End Try

    End Sub

End Class
