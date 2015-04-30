Partial Public Class AdminEmailLog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Master.PageTitleLabel = Me.Page.Title

            Call Populate_DropDownLists()
            Me.txtStartDate.Text = Today.AddDays(-7).ToShortDateString
            Me.txtEndDate.Text = Today.ToShortDateString
            Me.txtEmailFrom.Text = Master.UserLoginID
            'Call Populate_Data()
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String, sSQLWhere As String = "", dDateTester As Date
        '====================================================================================================
        sSQL = "SELECT * FROM EmailLog "

        If Me.txtEmailFrom.Text.Length > 0 Then
            Me.txtEmailFrom.Text = Common.RemoveInvalidSQLCharacters(Me.txtEmailFrom.Text)
            sSQLWhere = sSQLWhere & " AND LastUpdateByName LIKE '%" & Me.txtEmailFrom.Text & "%'  "
        End If

        If Me.txtEmailTo.Text.Length > 0 Then
            Me.txtEmailTo.Text = Common.RemoveInvalidSQLCharacters(Me.txtEmailTo.Text)
            sSQLWhere = sSQLWhere & " AND EmailTo LIKE '%" & Me.txtEmailTo.Text & "%'  "
        End If

        If Me.txtStartDate.Text.Length > 0 And Me.txtEndDate.Text.Length > 0 Then
            Try
                dDateTester = CDate(Me.txtStartDate.Text)
                dDateTester = CDate(Me.txtEndDate.Text).AddDays(1)
                sSQLWhere = sSQLWhere & " AND LastUpdateByDate BETWEEN CONVERT(DATETIME, '" & Me.txtStartDate.Text & " 00:00:00', 102) AND CONVERT(DATETIME, '" & dDateTester.ToShortDateString & " 00:00:00', 102) "
            Catch ex As Exception
                'do nothing
            End Try
        End If

        If sSQLWhere.Length > 1 Then sSQLWhere = " WHERE (1=1) " & sSQLWhere

        sSQL = sSQL & sSQLWhere & " ORDER BY LastUpdateByDate DESC "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gvMain.DataSource = dt
            gvMain.DataBind()
        Else
            gvMain.DataSource = Nothing
            gvMain.DataBind()
        End If


    End Sub

    Private Sub Populate_DropDownLists()
        '====================================================================================================
        'Dim dt As DataTable, sSQL As String
        '====================================================================================================
        'sSQL = "SELECT '' as EmptyField, LastUpdateByName FROM EmailLog GROUP BY LastUpdateByName ORDER BY LastUpdateByName"

        'dt = DataAccess.Run_SQL_Query(sSQL, True)

        'If dt.Rows.Count > 0 Then
        '    Me.ddlLastUpdatedByName.DataSource = dt
        '    Me.ddlLastUpdatedByName.DataValueField = "EmptyField"
        '    Me.ddlLastUpdatedByName.DataTextField = "LastUpdateByName"
        '    Me.ddlLastUpdatedByName.DataBind()
        'End If


        'dt.Clear()

        'sSQL = "SELECT '' as EmptyField, LastUpdateByName FROM EmailLog GROUP BY LastUpdateByName ORDER BY LastUpdateByName"

        'dt = DataAccess.Run_SQL_Query(sSQL, True)

        'If dt.Rows.Count > 0 Then
        '    Me.ddlLastUpdatedByName.DataSource = dt
        '    Me.ddlLastUpdatedByName.DataValueField = "EmptyField"
        '    Me.ddlLastUpdatedByName.DataTextField = "LastUpdateByName"
        '    Me.ddlLastUpdatedByName.DataBind()
        'End If
    End Sub

    Protected Sub gvMain_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        'this is strangely needed for _RowCommand...  
    End Sub

    Protected Sub gvMain_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        '==================================================================================================================================================================================================================================================
        '=== Controls ADD and EDIT commands; DELETE is done using different methods ===
        '==================================================================================================================================================================================================================================================
        Select Case e.CommandName
            Case "Edit"
                Response.Redirect("AdminEmail.aspx?ELogID=" & e.CommandArgument.ToString)

            Case Else
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Unknown CommandName argument specified for the RowCommand.  Contact Application Development to resolve."

        End Select

    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Me.txtEmailFrom.Text = String.Empty
        Me.txtEmailTo.Text = String.Empty
        Me.txtStartDate.Text = Today.AddDays(-31).ToShortDateString
        Me.txtEndDate.Text = Today.ToShortDateString
        Call Populate_Data()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Call Populate_Data()
    End Sub

    Protected Sub ibtnClear_EmailTo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClear_EmailTo.Click
        Me.txtEmailTo.Text = String.Empty
    End Sub

    Protected Sub ibtnClear_EmailFrom_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClear_EmailFrom.Click
        Me.txtEmailFrom.Text = String.Empty
    End Sub
End Class