Imports Adjudication.DataAccess

Public Class ErrorPage
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMainpageApplicationDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblMainpageApplicationNotes As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdminContactName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdminContactPhoneNum As System.Web.UI.WebControls.Label
    Protected WithEvents lnkAdminContactEmail As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lbtnViewError As System.Web.UI.WebControls.Button
    Protected WithEvents txtErrorMessage As System.Web.UI.WebControls.TextBox
    Protected WithEvents pnlErrorMessage As System.Web.UI.WebControls.Panel
    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lblSucessfulUpdate As System.Web.UI.WebControls.Label
    Protected WithEvents txtMainpageApplicationDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMainpageApplicationNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtApplicationName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAdminContactName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAdminContactPhoneNum As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAdminContactEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDaysToSubmitProduction As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDaysToAllowNominationEdits As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDaysToConfirmAttendance As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDaysToWaitForScoring As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnDEFAULTS_Update As System.Web.UI.WebControls.Button

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
        'Dim dt As DataTable, sSQL As String
        '============================================================================================
        If Page.IsPostBack = False Then
            txtErrorMessage.Text = Session.Item("ErrMessage")
            If DataAccess.Save_ErrorLog(Session("LoginID"), txtErrorMessage.Text) = True Then

            End If
        End If
    End Sub

    Private Sub lbtnViewError_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnViewError.Click
        Me.pnlErrorMessage.Visible = True
        Me.lbtnViewError.Visible = False
    End Sub
End Class
