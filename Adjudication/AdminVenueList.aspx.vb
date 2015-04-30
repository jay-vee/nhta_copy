Imports Adjudication.DataAccess

Public Class AdminVenueList
    Inherits System.Web.UI.Page
   
    Protected WithEvents pnlGrid As System.Web.UI.WebControls.Panel
    Protected WithEvents lbtnAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblTotalNumberOfRecords As System.Web.UI.WebControls.Label
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSuccessful As System.Web.UI.WebControls.Label

    Dim iAccessLevel As Int16
    Dim sCompanyID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.PageTitleLabel = Page.Title
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        ' TEMPORARY: Open Access to Liaisons to input Venues
        If Not (iAccessLevel = 1 Or iAccessLevel = 2 Or iAccessLevel = 3) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_VenueID, VenueName, Address, City, State, ZIP, Phone, Website, EmailAddress, " & _
                "   Directions, Parking, HandicappedAccessible, AirConditioned, " & _
                "   Outdoor, Directions, SeatingCapacity, Comments, LastUpdateByName, LastUpdateByDate " & _
                " FROM Venue " & _
                " WHERE PK_VenueID > 1 "

        If Not (txtSortColumnName.Text = "") And Not (txtSortOrder.Text = "VenueName") Then
            sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY VenueName "
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

        lblTotalNumberOfRecords.Text = "Number of Venues: " & dt.Rows.Count.ToString

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminVenue.aspx?VenueID=" & e.Item.Cells(0).Text)

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
        Response.Redirect("AdminVenue.aspx?Add=True")
    End Sub

End Class
