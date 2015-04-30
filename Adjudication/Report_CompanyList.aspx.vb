Imports Adjudication.DataAccess

Public Class CompanyList
    Inherits System.Web.UI.Page
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblTotalNumberOfRecords As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    Dim iAccessLevel As Int16

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If Not iAccessLevel > 0 Then Response.Redirect("Login.aspx")
        'Me.lblLoginID.Text = Session("LoginID")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        Dim dt As DataTable, sSQL As String, sReportFieldList As String = ""

        sSQL = "SELECT Company.PK_CompanyID, Company.CompanyName, " & _
                "		Company.Address + ', ' + Company.City + ', ' + Company.State + '  ' + Company.ZIP AS FullAddress,  " & _
                "       CASE WHEN SubString(Website,1,7) = 'HTTP://' THEN SubString(Website,8,LEN(Website)) ELSE Website END as Website, " & _
                "		Company.Phone, Company.EmailAddress, Company.ActiveCompany, Company.Comments  " & _
                " FROM Company "

        sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text

        Try
            dt = DataAccess.Run_SQL_Query(sSQL)
            Me.gridMain.DataSource = dt
            Me.gridMain.DataBind()
            Me.lblTotalNumberOfRecords.Text = "Number of Companies: <B>" & dt.Rows.Count & "</B>"

        Catch ex As Exception
            lblTotalNumberOfRecords.Text = "ERROR: " & ex.Message
            lblTotalNumberOfRecords.ForeColor = System.Drawing.Color.Red
        End Try
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
