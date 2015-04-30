﻿Imports Adjudication.DataAccess

Partial Public Class AdminCategoryList
    Inherits System.Web.UI.Page
    Dim iAccessLevel As Int16
    Dim sLoginID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Categories(txtSortColumnName.Text, txtSortOrder.Text)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminCategory.aspx?CategoryID=" & e.Item.Cells(0).Text)

            Case "Delete_Command"
                Me.pnlGrid.Visible = False
                Me.pnlDeleteConfirm.Visible = True
                Me.lblConfirmDelete.Text = "Do you really want to delete the Category <b>" & e.Item.Cells(3).Text & "</b>"
                Me.txtID_to_Delete.Text = e.Item.Cells(0).Text

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
        Response.Redirect("AdminCategory.aspx?CategoryID=ADD")
    End Sub


    Private Sub btnDeleteCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteCancel.Click
        Me.pnlDeleteConfirm.Visible = False
        Me.pnlGrid.Visible = True
    End Sub

    Private Sub btnDeleteConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteConfirm.Click
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        Try
            sSQL = "DELETE FROM Category WHERE PK_CategoryID=" & Me.txtID_to_Delete.Text
            Call DataAccess.SQLDelete(sSQL)
            Me.txtID_to_Delete.Text = ""
            Me.pnlDeleteConfirm.Visible = False
            Me.pnlGrid.Visible = True
            Call Populate_DataGrid()

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Throw ex
        End Try
    End Sub

End Class