Imports Adjudication.CustomMail
Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CommonFunctions
Imports System.Data
Imports System.Web

Public Class AdjLateBallot30_60_90
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If Not (Master.AccessLevel = 1) Then Response.Redirect("../UnAuthorized.aspx")
        iAccessLevel = Master.AccessLevel
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If
    End Sub



    Private Sub Populate_DataGrid()
        '============================================================================================
        Dim dt As DataTable
        '============================================================================================

        Try
            dt = DataAccess.Find_Late_Ballots(txtSortColumnName.Text, txtSortOrder.Text)
            Me.gridMain.DataSource = dt
            Me.gridMain.DataBind()
            Me.lblTotalNumberOfRecords.Text = "Number of Late Adjudicator Scores: <B>" & dt.Rows.Count & "</B>"

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
