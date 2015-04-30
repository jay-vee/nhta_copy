Imports Adjudication.DataAccess

Public Class UserBrowserHistory
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents ddlActive As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlFK_AccessLevelID As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLastTrainingDate As System.Web.UI.WebControls.Label
    Protected WithEvents lblFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents lblPhonePrimary As System.Web.UI.WebControls.Label
    Protected WithEvents lblPhoneSecondary As System.Web.UI.WebControls.Label
    Protected WithEvents lblAddress As System.Web.UI.WebControls.Label
    Protected WithEvents lblCity As System.Web.UI.WebControls.Label
    Protected WithEvents lblState As System.Web.UI.WebControls.Label
    Protected WithEvents lblZIP As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmailPrimary As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmailSecondary As System.Web.UI.WebControls.Label
    Protected WithEvents lblUserInformation As System.Web.UI.WebControls.Label
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label
    Protected WithEvents lblSortColumnName As System.Web.UI.WebControls.Label
    Protected WithEvents lblCompanyName As System.Web.UI.WebControls.Label
    Protected WithEvents lblActive As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub




    Dim iAccessLevel As Int16, FK_UserID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
            Session.Item("PK_UserID") = "85"        ' FOR TESTING ONLY
        End If
        '============================================================================================
        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("../UnAuthorized.aspx")
        Me.lblLoginID.Text = Session("LoginID")
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Request.QueryString("UserID") <> "" And iAccessLevel = 1 Then
            FK_UserID = CInt(Request.QueryString("UserID"))
        Else
            FK_UserID = CInt(Session("PK_UserID"))
        End If

        If Not IsPostBack Then
            Call Populate_DropDowns()
            Call Populate_DataGrid()
        End If

    End Sub




    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String, sTmp As String
        '====================================================================================================
        Try
            sSQL = " SELECT " & _
                "       UserAccessLevels.AccessLevelName, " & _
                "       Users.PK_UserID, Users.FK_AccessLevelID, Users.FK_CompanyID, Users.UserLoginID, " & _
                "       Users.FirstName, " & _
                "       Users.LastName, " & _
                "       Users.Address, Users.City, Users.State, Users.ZIP, " & _
                "       Users.PhonePrimary, Users.PhoneSecondary, " & _
                "       Users.EmailPrimary, Users.EmailSecondary, " & _
                "       Users.LastTrainingDate, " & _
                "       Users.Website, " & _
                "       Users.DateLastPasswordChange, " & _
                "       Users.LastLoginTime, " & _
                "       Users.DisabledFlag, " & _
                "       Users.BadLoginCount, " & _
                "       Users.UserInformation, " & _
                "       Users.Active, " & _
                "       Users.LastUpdateByName, Users.LastUpdateByDate, " & _
                "       Company.CompanyName " & _
                " FROM " & _
                "       Users " & _
                "       INNER JOIN UserAccessLevels ON FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID  " & _
                "       INNER JOIN Company ON Users.FK_CompanyID = Company.PK_CompanyID " & _
                " WHERE " & _
                "       PK_UserID=" & FK_UserID.ToString

            dt = DataAccess.Run_SQL_Query(sSQL)

            If dt.Rows.Count > 0 Then
                Me.lblUserLoginID.Text = dt.Rows(0)("UserLoginID").ToString
                If dt.Rows(0)("Active").ToString = "1" Then
                    Me.lblActive.Text = "[<font color=green>Active</span>]"
                Else
                    Me.lblActive.Text = "[<font color=red>InActive</span>]"
                End If
                Me.ddlFK_AccessLevelID.SelectedValue = dt.Rows(0)("FK_AccessLevelID").ToString
                Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString
                Me.lblFirstName.Text = dt.Rows(0)("FirstName").ToString & " " & dt.Rows(0)("LastName").ToString
                Me.lblAddress.Text = dt.Rows(0)("Address").ToString
                Me.lblCity.Text = dt.Rows(0)("City").ToString
                Me.lblState.Text = dt.Rows(0)("State").ToString
                Me.lblZIP.Text = dt.Rows(0)("ZIP").ToString
                Me.lblPhonePrimary.Text = dt.Rows(0)("PhonePrimary").ToString
                Me.lblPhoneSecondary.Text = dt.Rows(0)("PhoneSecondary").ToString
                Me.lblEmailPrimary.Text = dt.Rows(0)("EmailPrimary").ToString
                Me.lblEmailSecondary.Text = dt.Rows(0)("EmailSecondary").ToString

                ' Added guard;
                ' was throwing an exception if 'LastTrainingDate' wasn't set to
                ' a value that could be converted to a date.

                sTmp = dt.Rows(0)("LastTrainingDate").ToString
                If (sTmp IsNot Nothing And sTmp.Length <> 0) Then
                    Me.lblLastTrainingDate.Text = CDate(dt.Rows(0)("LastTrainingDate").ToString).ToShortDateString
                End If

                Me.lblUserInformation.Text = dt.Rows(0)("UserInformation").ToString
            End If

            '====================================================================================================
            Me.gridMain.DataSource = DataAccess.Get_BrowserDetect(FK_UserID.ToString)
            Me.gridMain.DataBind()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, dr As DataRow, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_AccessLevelID, AccessLevelName FROM UserAccessLevels"

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            If Request.QueryString("AddUser") = "True" Then
                'If new user, add an empty row to force a selection
                dr = dt.NewRow()
                dt.Rows.InsertAt(dr, 0)
            End If
            ddlFK_AccessLevelID.DataSource = dt
            ddlFK_AccessLevelID.DataValueField = "PK_AccessLevelID"
            ddlFK_AccessLevelID.DataTextField = "AccessLevelName"
            ddlFK_AccessLevelID.DataBind()
            'ddlFK_AccessLevelID.SelectedIndex = dt.Rows.Count - 2       ' Make the default value: Backup Adjudicator

        End If

    End Sub





    Sub gridMain_SortCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gridMain.SortCommand

        If Me.lblSortColumnName.Text = e.SortExpression Then
            If Me.lblSortOrder.Text = " DESC " Then
                Me.lblSortOrder.Text = ""
            Else
                Me.lblSortOrder.Text = " DESC "
            End If
        Else
            Me.lblSortOrder.Text = ""
        End If

        Me.lblSortColumnName.Text = e.SortExpression

        Populate_DataGrid()
    End Sub


End Class
