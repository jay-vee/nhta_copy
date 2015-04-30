Imports Adjudication.DataAccess
Imports Adjudication.Common

Partial Public Class AdminUserList1
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sLoginID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        sLoginID = Master.UserLoginID
        Master.PageTitleLabel = Page.Title
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DropDowns()
            If Session("SearchCompanyID") <> "" Then
                Me.ddlFK_CompanyID.SelectedValue = Session("SearchCompanyID")
            End If

            Call Populate_DataGrid()
        End If
    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String, sWhere As String = ""
        '====================================================================================================
        sSQL = "SELECT Users.PK_UserID, Users.FK_AccessLevelID, Users.FK_CompanyID, Users.UserLoginID, Users.FirstName, Users.LastName,  " & _
                "   Users.LastName + ', ' + Users.FirstName as FullName, " & _
                "   Users.Address, Users.City, Users.State, Users.ZIP, Users.PhonePrimary, Users.PhoneSecondary, Users.EmailPrimary, Users.EmailSecondary,  " & _
                "   Users.LastTrainingDate, Users.Website, Users.DateLastPasswordChange, Users.LastLoginTime, Users.DisabledFlag, Users.BadLoginCount,  " & _
                "   Users.UserInformation, Users.LastUpdateByName, Users.LastUpdateByDate, Users.CreateByName, Users.CreateByDate, " & _
                "   Company.CompanyName, UserAccessLevels.AccessLevelName " & _
                " FROM Company " & _
                "   INNER JOIN Users ON Company.PK_CompanyID = Users.FK_CompanyID " & _
                "   INNER JOIN UserAccessLevels ON Users.FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID "

        If Not (Me.txtSearchUser.Text = "") Then
            txtSearchUser.Text = RemoveInvalidSQLCharacters(txtSearchUser.Text)   '=== To prevent SQL Injection, Remove any invalid SQL Characters from the text ===================
            sWhere = sWhere + " (Users.LastName LIKE '%" + txtSearchUser.Text + "%' OR Users.FirstName LIKE '%" + txtSearchUser.Text + "%') AND "
        End If

        If Me.chkInactiveUsers.Checked = False Then
            sWhere = sWhere + " (Users.FK_AccessLevelID < 6) AND (Users.Active=1) AND "
        End If

        If Not (Me.ddlFK_CompanyID.SelectedIndex = 0) Then
            sWhere = sWhere + " (Users.FK_CompanyID = " + ddlFK_CompanyID.SelectedItem.Value + ") AND "
        End If

        If sWhere.Length > 1 Then
            sWhere = " WHERE " & sWhere & " Users.PK_UserID IS NOT NULL "
            sSQL = sSQL & sWhere
        End If

        If Not (txtSortColumnName.Text = "") And Not (txtSortOrder.Text = "FullName") Then
            sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY FullName "
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

        lblTotalNumberOfRecords.Text = dt.Rows.Count.ToString

    End Sub

    Private Sub Populate_DropDowns()
        '====================================================================================================
        Dim dt As DataTable, dr As DataRow, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_CompanyID, CompanyName FROM Company ORDER BY CompanyName"

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            dr = dt.NewRow()
            dt.Rows.InsertAt(dr, 0)         ' Inserts blank value as first value in DropDownList
            Me.ddlFK_CompanyID.DataSource = dt
            ddlFK_CompanyID.DataValueField = "PK_CompanyID"
            ddlFK_CompanyID.DataTextField = "CompanyName"
            ddlFK_CompanyID.DataBind()
        End If

    End Sub

    Private Sub btnSearchName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchName.Click
        If Me.ddlFK_CompanyID.SelectedIndex > 0 Then
            Session("SearchCompanyID") = Me.ddlFK_CompanyID.SelectedValue
        Else
            Session("SearchCompanyID") = Nothing
        End If
        Call Populate_DataGrid()
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.txtSearchUser.Text = ""
        Me.ddlFK_CompanyID.SelectedIndex = 0
        Session("SearchCompanyID") = Nothing

        Call Populate_DataGrid()
    End Sub

    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" Then

            Select Case e.Item.Cells(1).Text.ToUpper
                Case "1"
                    e.Item.Cells(8).Font.Bold = True
                Case "2"    'Liaision & Primary Adj.
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.DarkGreen
                    e.Item.Cells(8).Font.Bold = True
                Case "3"       'Liaison 
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.SeaGreen
                Case "4"
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.Green
                    e.Item.Cells(8).Font.Bold = True
                Case "5"
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.LightGreen
                Case "6"
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.DarkGray
                Case "7"
                    e.Item.Cells(8).Font.Italic = True
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.Red
                Case "8"
                    e.Item.Cells(8).Font.Italic = True
                    e.Item.Cells(8).Font.Bold = True
                    e.Item.Cells(8).ForeColor = System.Drawing.Color.Red
            End Select

        End If

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminUser.aspx?LoginID=" & e.Item.Cells(7).Text)

            Case "Reset_Command"
                Call ResetAccount(e.Item.Cells(7).Text)

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

    Private Sub ResetAccount(ByVal UserLoginID As String)
        '====================================================================================================
        Dim dt As New DataTable
        '====================================================================================================
        Me.lblSuccessfulReset.Visible = False

        dt = DataAccess.Secure_ResetAccount(UserLoginID, sLoginID)

        If dt.Rows(0)("ReturnValue") = "0" Then
            lblSuccessfulReset.Text = "Successfully Reset Login ID: <B>" & UserLoginID.ToUpper & "</B>"
            lblSuccessfulReset.Visible = True
            Me.lblSuccessfulReset.ForeColor = System.Drawing.Color.Green
        Else
            Me.lblSuccessfulReset.Text = "ERROR: " & dt.Rows(0)("ReturnUserInformation")
            Me.lblSuccessfulReset.Visible = True
            Me.lblSuccessfulReset.ForeColor = System.Drawing.Color.Red
        End If

    End Sub

    Private Sub lbtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        Response.Redirect("AdminUser.aspx?AddUser=True")
    End Sub



End Class