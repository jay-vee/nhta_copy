Imports Adjudication.DataAccess

Public Class Reports
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMainpageApplicationDesc As System.Web.UI.WebControls.Label
    Protected WithEvents lblMainpageApplicationNotes As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdminContactName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdminContactPhoneNum As System.Web.UI.WebControls.Label
    Protected WithEvents lnkAdminContactEmail As System.Web.UI.WebControls.HyperLink
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
    Protected WithEvents btnReportSelection_3 As System.Web.UI.WebControls.Button
    Protected WithEvents btnRunSelectedReport As System.Web.UI.WebControls.Button
    Protected WithEvents pnlReportSelection_1 As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlReportSelection_2 As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlReportSelection_3 As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlReports As System.Web.UI.WebControls.Panel
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents ddlReportType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents pnlRunReport As System.Web.UI.WebControls.Panel
    Protected WithEvents btnReportSelection_1 As System.Web.UI.WebControls.Button
    Protected WithEvents btnReportSelection_2 As System.Web.UI.WebControls.Button
    Protected WithEvents rblReportList_1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rblReportList_3 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents ddlDisplayScores As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlFK_ProductionCategoryID As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlFK_ProductionTypeID As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtTopXProfessional As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTopXCommunity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTopXYouth As System.Web.UI.WebControls.TextBox
    Protected WithEvents rblReportList_2 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Dropdownlist1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCategories As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlSortOrder As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub



    Dim iAccessLevel As Int16
    Dim sCompanyID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================

        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("UnAuthorized.aspx")
        Me.lblLoginID.Text = Session("LoginID")
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Page.IsPostBack = False Then
            Populate_DropDowns()
        End If
    End Sub


    Private Sub ShowHideReportSections(ByVal SectionNum As Int16)
        Select Case SectionNum
            Case 1
                Me.btnReportSelection_1.BackColor = System.Drawing.Color.FromName("LemonChiffon")
                Me.btnReportSelection_1.ForeColor = System.Drawing.Color.Black
                Me.pnlReportSelection_1.Visible = True
                Me.btnReportSelection_2.BackColor = System.Drawing.Color.WhiteSmoke
                Me.btnReportSelection_2.ForeColor = System.Drawing.Color.DarkGray
                Me.pnlReportSelection_2.Visible = False
                Me.btnReportSelection_3.BackColor = System.Drawing.Color.WhiteSmoke
                Me.btnReportSelection_3.ForeColor = System.Drawing.Color.DarkGray
                Me.pnlReportSelection_3.Visible = False

            Case 2
                Me.btnReportSelection_1.BackColor = System.Drawing.Color.WhiteSmoke
                Me.btnReportSelection_1.ForeColor = System.Drawing.Color.DarkGray
                Me.pnlReportSelection_1.Visible = False
                Me.btnReportSelection_2.BackColor = System.Drawing.Color.FromName("LemonChiffon")
                Me.btnReportSelection_2.ForeColor = System.Drawing.Color.Black
                Me.pnlReportSelection_2.Visible = True
                Me.btnReportSelection_3.BackColor = System.Drawing.Color.WhiteSmoke
                Me.btnReportSelection_3.ForeColor = System.Drawing.Color.DarkGray
                Me.pnlReportSelection_3.Visible = False

            Case 3
                Me.btnReportSelection_1.BackColor = System.Drawing.Color.WhiteSmoke
                Me.btnReportSelection_1.ForeColor = System.Drawing.Color.DarkGray
                Me.pnlReportSelection_1.Visible = False
                Me.btnReportSelection_2.BackColor = System.Drawing.Color.WhiteSmoke
                Me.btnReportSelection_2.ForeColor = System.Drawing.Color.DarkGray
                Me.pnlReportSelection_2.Visible = False
                Me.btnReportSelection_3.BackColor = System.Drawing.Color.FromName("LemonChiffon")
                Me.btnReportSelection_3.ForeColor = System.Drawing.Color.Black
                Me.pnlReportSelection_3.Visible = True

        End Select
    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        ' ====================================================================================================
        sSQL = "SELECT PK_ProductionCategoryID, ProductionCategory FROM ProductionCategory"

        dt = DataAccess.Run_SQL_Query(sSQL, True)

        If dt.Rows.Count > 0 Then
            ddlFK_ProductionCategoryID.DataSource = dt
            ddlFK_ProductionCategoryID.DataValueField = "PK_ProductionCategoryID"
            ddlFK_ProductionCategoryID.DataTextField = "ProductionCategory"
            ddlFK_ProductionCategoryID.DataBind()
        End If

        dt.Clear()
        sSQL = "SELECT PK_ProductionTypeID, ProductionType FROM ProductionType"
        dt = DataAccess.Run_SQL_Query(sSQL, True)

        If dt.Rows.Count > 0 Then
            ddlFK_ProductionTypeID.DataSource = dt
            ddlFK_ProductionTypeID.DataValueField = "PK_ProductionTypeID"
            ddlFK_ProductionTypeID.DataTextField = "ProductionType"
            ddlFK_ProductionTypeID.DataBind()
        End If

        'dt.Clear()
        'sSQL = "SELECT CategoryName, ScoreFieldName FROM Category"
        'dt = DataAccess.Run_SQL_Query(sSQL, True)

        'If dt.Rows.Count > 0 Then
        '    ddlCategories.DataSource = dt
        '    ddlCategories.DataValueField = "ScoreFieldName"
        '    ddlCategories.DataTextField = "CategoryName"
        '    ddlCategories.DataBind()
        'End If

    End Sub

    Private Sub btnReportSelection_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReportSelection_1.Click
        Call ShowHideReportSections(1)
    End Sub

    Private Sub btnReportSelection_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReportSelection_2.Click
        Call ShowHideReportSections(2)
    End Sub

    Private Sub btnReportSelection_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReportSelection_3.Click
        Call ShowHideReportSections(3)
    End Sub

    Private Sub btnRunSelectedReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunSelectedReport.Click
        'Dim sQString As String
        'Session.Item("ReportList_Detail") = Me.rblReportList.SelectedIndex
        'Session.Item("ReportList_Summary") = Me.rblReportSummaryList.SelectedIndex
        'Session.Item("ReportType_Detail") = Me.ddlReportType.SelectedIndex
        'Session.Item("ReportType_Summary") = Me.ddlReportType_2.SelectedIndex
        'sQString = "RptName=" & Me.rblReportList.SelectedValue & "&RptType=" & ddlReportType.SelectedValue
        Dim sReportName As String = ""

        If Me.btnReportSelection_1.BackColor.ToString = "Color [LemonChiffon]" Then
            sReportName = Me.rblReportList_1.SelectedValue.ToString
        ElseIf Me.btnReportSelection_2.BackColor.ToString = "Color [LemonChiffon]" Then
            sReportName = Set_ScoringReport_Values()

        ElseIf Me.btnReportSelection_3.BackColor.ToString = "Color [LemonChiffon]" Then
            sReportName = Me.rblReportList_3.SelectedValue.ToString
        End If

        Response.Write("<script language=javascript>")
        Response.Write("window.open('Reports/" & sReportName & ".aspx','Microscript','status=yes ,toolbar=no,directories=no,menubar=no,scrollbars=yes,resizable=yes,location=no,top=50,left=250,height=800,width=800');")
        Response.Write("</script>")

    End Sub

    Private Function Set_ScoringReport_Values() As String

        If Me.ddlFK_ProductionTypeID.SelectedValue.ToString = "0" Then
            Session.Item("ProductionTypeID") = Nothing
        Else
            Session.Item("ProductionTypeID") = Me.ddlFK_ProductionTypeID.SelectedValue.ToString
        End If

        If Me.ddlFK_ProductionCategoryID.SelectedValue.ToString = "0" Then
            Session.Item("ProductionCategoryID") = Nothing
        Else
            Session.Item("ProductionCategoryID") = Me.ddlFK_ProductionCategoryID.SelectedValue.ToString
        End If

        If Me.ddlDisplayScores.SelectedValue.ToString = "0" Then
            Session.Item("DisplayScores") = Nothing
        Else
            Session.Item("DisplayScores") = Me.ddlDisplayScores.SelectedValue.ToString
        End If

        Session.Item("Category") = Me.ddlCategories.SelectedValue.ToString
        Session.Item("SortOrder") = Me.ddlSortOrder.SelectedValue.ToString

        Session.Item("TopXProfessional") = Me.txtTopXProfessional.Text
        Session.Item("TopXCommunity") = Me.txtTopXCommunity.Text
        Session.Item("TopXYouth") = Me.txtTopXYouth.Text

        Return Me.rblReportList_2.SelectedValue.ToString         'REPORT NAME

    End Function

    Private Sub rblReportList_2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblReportList_2.SelectedIndexChanged
        Select Case rblReportList_2.SelectedIndex
            Case 0
                Me.ddlDisplayScores.Enabled = True
                Me.ddlCategories.Enabled = False
                Me.txtTopXCommunity.Enabled = True
                Me.txtTopXProfessional.Enabled = True
                Me.txtTopXYouth.Enabled = True
            Case 1
                Me.ddlDisplayScores.Enabled = True
                Me.ddlCategories.Enabled = False
                Me.txtTopXCommunity.Enabled = True
                Me.txtTopXProfessional.Enabled = True
                Me.txtTopXYouth.Enabled = True
            Case 2
                Me.ddlDisplayScores.Enabled = False
                Me.ddlCategories.Enabled = False
                Me.txtTopXCommunity.Enabled = True
                Me.txtTopXProfessional.Enabled = True
                Me.txtTopXYouth.Enabled = True
            Case 3
                Me.ddlDisplayScores.Enabled = False
                Me.ddlCategories.Enabled = True
                Me.txtTopXCommunity.Enabled = False
                Me.txtTopXProfessional.Enabled = False
                Me.txtTopXYouth.Enabled = True
            Case 4
                Me.ddlDisplayScores.Enabled = True
                Me.ddlCategories.Enabled = False
                Me.txtTopXCommunity.Enabled = True
                Me.txtTopXProfessional.Enabled = True
                Me.txtTopXYouth.Enabled = True
            Case 5
                Me.ddlDisplayScores.Enabled = True
                'Me.ddlDisplayScores.SelectedIndex = 0
                Me.ddlCategories.Enabled = False
                Me.txtTopXCommunity.Enabled = True
                Me.txtTopXProfessional.Enabled = True
                Me.txtTopXYouth.Enabled = True
            Case Else
                Me.ddlDisplayScores.Enabled = False
                Me.ddlCategories.Enabled = False
                Me.txtTopXCommunity.Enabled = False
                Me.txtTopXProfessional.Enabled = False
                Me.txtTopXYouth.Enabled = False
        End Select
    End Sub

End Class
