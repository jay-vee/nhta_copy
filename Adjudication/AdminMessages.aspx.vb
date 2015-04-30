Imports Adjudication.DataAccess

Public Class AdminMessages
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
             Master.PageTitleLabel = Page.Title
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================
        If Not IsPostBack Then
            Call Populate_Data()
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT top 50 PK_AdminMessageID, Subject, Message, LastUpdateByName, LastUpdateByDate FROM AdminMessage ORDER BY LastUpdateByDate DESC "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If
    End Sub


    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                'Response.Redirect("AdminCompany.aspx?CompanyID=" & e.Item.Cells(0).Text)

            Case "Delete_Command"
                'Me.pnlGrid.Visible = False
                'Me.pnlDeleteConfirm.Visible = True
                'lblConfirmDelete.Text = "CONFIRM DELETE: WHEN " & ddlAUTO_VALIDATION_FIELD_ID.SelectedItem.ToString & " " & ddlLOGIC_OPERATOR_TYPE_ID.SelectedItem.ToString & " " & AUTO_VALIDATION_FIELD_VALUE_TXT.Text & "  THEN Set Record Status = " & ddlRECORD_STATUS_ID.SelectedItem.ToString

            Case Else
                ' break 
        End Select
    End Sub

End Class
