Imports Adjudication.DataAccess

Public Class ViewAllProductions
    Inherits System.Web.UI.Page
    Protected WithEvents pnlGrid As System.Web.UI.WebControls.Panel
    Protected WithEvents lbtnAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblTotalNumberOfRecords As System.Web.UI.WebControls.Label
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    Dim iAccessLevel As Int16
    Dim sCompanyID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If Not iAccessLevel > 0 Then Response.Redirect("Login.aspx")
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
        Dim dt As DataTable
        '====================================================================================================
        Dim sSQL As String = String.Empty
        sSQL = "SELECT Production.PK_ProductionID, Production.Title, Company.CompanyName, ProductionType.ProductionType, " & _
                "		Venue.VenueName + ' in ' + Venue.City + ', ' + Venue.State as VenueInfo , Production.FirstPerformanceDateTime,  " & _
                "		Production.LastPerformanceDateTime, Production.RequiresAdjudication, " & _
                "       ProductionCategory.ProductionCategory, " & _
                "       COUNT(Nominations.PK_NominationsID) AS SetNominations " & _
                " FROM  Production " & _
                "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                "		INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                "		INNER JOIN Venue ON Production.FK_VenueID = Venue.PK_VenueID " & _
                "		INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
                "       LEFT OUTER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID " & _
                " GROUP BY Production.PK_ProductionID, Production.Title, Company.CompanyName, ProductionType.ProductionType, ProductionCategory.ProductionCategory, " & _
                "		Venue.VenueName, Venue.City, Venue.State, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.RequiresAdjudication  "

        If Not (txtSortColumnName.Text = "") And Not (txtSortColumnName.Text = "CompanyName") Then
            sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text + ", FirstPerformanceDateTime "
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY FirstPerformanceDateTime "
        End If

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

        lblTotalNumberOfRecords.Text = "Number of Productions: " & dt.Rows.Count.ToString
    End Sub


    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminProduction.aspx?Admin=True&ProductionID=" & e.Item.Cells(0).Text)

            Case "Nomination_Command"
                Response.Redirect("AdminNominations.aspx?Admin=True&ProductionID=" & e.Item.Cells(0).Text)

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
        Response.Redirect("AdminProduction.aspx?Add=True&Admin=True")
    End Sub

End Class
