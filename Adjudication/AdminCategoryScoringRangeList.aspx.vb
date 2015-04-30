Imports Adjudication.DataAccess

Public Class AdminCategoryScoringRangeList
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
            Call Populate_DataGrid()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Category_ScoringRanges(txtSortColumnName.Text, txtSortOrder.Text)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

    End Sub

    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        Static MyBackgroundColor As System.Drawing.Color = System.Drawing.Color.White, CategoryID As String = ""
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then

            If CategoryID = "" Then CategoryID = e.Item.Cells(0).Text

            If CategoryID = e.Item.Cells(0).Text Then
            Else
                CategoryID = e.Item.Cells(0).Text
                If MyBackgroundColor.ToString = System.Drawing.Color.White.ToString Then
                    MyBackgroundColor = System.Drawing.Color.LemonChiffon
                Else
                    MyBackgroundColor = System.Drawing.Color.White
                End If
            End If

            e.Item.Cells(1).BackColor = MyBackgroundColor
            e.Item.Cells(2).BackColor = MyBackgroundColor
            e.Item.Cells(3).BackColor = MyBackgroundColor
            e.Item.Cells(4).BackColor = MyBackgroundColor
            e.Item.Cells(5).BackColor = MyBackgroundColor
            e.Item.Cells(6).BackColor = MyBackgroundColor
            e.Item.Cells(7).BackColor = MyBackgroundColor

        End If

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminCategoryScoringRange.aspx?CategoryID=" & e.Item.Cells(0).Text & "&ScoringRangeID=" & e.Item.Cells(1).Text)

            Case "Delete_Command"
                Me.pnlGrid.Visible = False
                Me.pnlDeleteConfirm.Visible = True
                Me.lblConfirmDelete.Text = "Do you really want to delete the Category <b>" & e.Item.Cells(3).Text & "</b>"
                Me.txtID_to_Delete.Text = e.Item.Cells(0).Text
                Me.txtID_to_Delete_2.Text = e.Item.Cells(1).Text

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
        Response.Redirect("AdminCategoryScoringRange.aspx?CategoryID=ADD")
    End Sub


    Private Sub btnDeleteCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteCancel.Click
        Me.txtID_to_Delete.Text = Nothing
        Me.txtID_to_Delete_2.Text = Nothing

        Me.pnlDeleteConfirm.Visible = False
        Me.pnlGrid.Visible = True
    End Sub

    Private Sub btnDeleteConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteConfirm.Click
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        Try
            sSQL = "DELETE FROM Category_ScoringRange WHERE FK_CategoryID=" & Me.txtID_to_Delete.Text & " AND FK_ScoringRangeID=" & Me.txtID_to_Delete_2.Text
            Call DataAccess.SQLDelete(sSQL)
            Me.txtID_to_Delete.Text = Nothing
            Me.txtID_to_Delete_2.Text = Nothing

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
