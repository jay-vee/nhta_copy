Imports Adjudication.DataAccess

Public Class AdminScoring
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

    Dim sLoginID As String
    Dim iAccessLevel As Int16
    Dim sCompanyID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        '============================================================================================
        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("UnAuthorized.aspx")
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

        dt = DataAccess.Get_AdjudicatorAssignments(txtSortColumnName.Text, txtSortOrder.Text)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

        lblTotalNumberOfRecords.Text = "Number of Ballots: " & dt.Rows.Count.ToString
        'lblCurrentIndex.Text = "Current Page: " & gridMain.CurrentPageIndex + 1
        'lblPageCount.Text = "Total Pages: " & gridMain.PageCount
    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("BallotSummary.aspx?Admin=True&ScoringID=" & e.Item.Cells(0).Text)
            Case "Edit_Ballot"
                Response.Redirect("Ballot.aspx?ScoringID=" & e.Item.Cells(0).Text)
            Case "Print"
                Response.Redirect("BallotSummary.aspx?Admin=True&Print=True&ScoringID=" & e.Item.Cells(0).Text)

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

End Class
