Imports Adjudication.DataAccess

Public Class AdminCompanyList
    Inherits System.Web.UI.Page

    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents pnlGrid As System.Web.UI.WebControls.Panel
    Protected WithEvents lbtnAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblTotalNumberOfRecords As System.Web.UI.WebControls.Label
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSuccessful As System.Web.UI.WebControls.Label
    Protected WithEvents chkInactive As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Dim iAccessLevel As Int16
    Dim sCompanyID As String
    Dim sLoginID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        sLoginID = Master.UserLoginID
        Master.PageTitleLabel = Page.Title
        '============================================================================================

        If Not IsPostBack Then

            Call Populate_DataGrid()
        End If
    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_CompanyID, NumOfProductions, CompanyName, " & _
                "   Address, City, State, ZIP, Phone, EmailAddress, Website, ActiveCompany, Comments,  " & _
                "   Company.LastUpdateByName, Company.LastUpdateByDate " & _
                " FROM Company " & _
                " WHERE  (NOT (Company.CompanyName LIKE '*%'))"

        If Me.chkInactive.Checked = False Then
            sSQL = sSQL + " AND (Company.ActiveCompany=1) "
        End If

        If Not (txtSortColumnName.Text = "") And Not (txtSortOrder.Text = "CompanyName") Then
            sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY CompanyName "
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

        lblTotalNumberOfRecords.Text = "Number of Companies: " & dt.Rows.Count.ToString

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminCompany.aspx?CompanyID=" & e.Item.Cells(0).Text)

            Case "Delete_Command"
                'Me.pnlGrid.Visible = False
                'Me.pnlDeleteConfirm.Visible = True
                'lblConfirmDelete.Text = "CONFIRM DELETE: WHEN " & ddlAUTO_VALIDATION_FIELD_ID.SelectedItem.ToString & " " & ddlLOGIC_OPERATOR_TYPE_ID.SelectedItem.ToString & " " & AUTO_VALIDATION_FIELD_VALUE_TXT.Text & "  THEN Set Record Status = " & ddlRECORD_STATUS_ID.SelectedItem.ToString

            Case Else
                ' break 
        End Select
    End Sub

    Sub gridMain_SortCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gridMain.SortCommand

        If txtSortColumnName.Text = e.SortExpression Then
            If txtSortOrder.Text = " DESC " Then
                txtSortOrder.Text = ""
            Else
                txtSortOrder.Text = " DESC "
            End If
        Else
            txtSortOrder.Text = ""
        End If

        txtSortColumnName.Text = e.SortExpression

        Populate_DataGrid()
    End Sub

    Private Sub lbtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        Response.Redirect("AdminCompany.aspx?Add=True")
    End Sub

    Private Sub chkInactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInactive.CheckedChanged
        Call Populate_DataGrid()
    End Sub
End Class
