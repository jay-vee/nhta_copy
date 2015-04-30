Imports Adjudication.DataAccess

Partial Class ScoringBestSupportingActor
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbtnAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents LastProductionScore As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub




    Dim ProductionID As String = "", LastNominatedName As String = "", LastAverageScore As String = ""
    Dim ProductionType As String = "", ProductionCategory As String = ""
    Dim myLabel As Label
    Dim iCount As Int16

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
        If Not iAccessLevel > 0 Then Response.Redirect("../Login.aspx")
        Me.lblLoginID.Text = Session("LoginID")
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If

    End Sub




    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        Try
            dt = DataAccess.ReportScoring_BestSupportingActor()
            Me.gridMain.DataSource = dt
            Me.gridMain.DataBind()
            Me.lblTotalNumberOfRecords.Text = "Number of Scores: <B>" & dt.Rows.Count & "</B>"

        Catch ex As Exception
            lblTotalNumberOfRecords.Text = "ERROR: " & ex.Message
            lblTotalNumberOfRecords.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub





    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound

        Try

            If Not e.Item.ItemType.ToString = "Header" Then
                If Not e.Item.ItemType.ToString = "Footer" Then

                    ' Check if this is the same ProductionCategory and Production Type (ie: Community Musical)
                    If ProductionType = e.Item.Cells(7).Text And ProductionCategory = e.Item.Cells(6).Text Then
                    Else
                        myLabel = e.Item.FindControl("lblCategory")
                        myLabel.Visible = True
                        ProductionCategory = e.Item.Cells(6).Text
                        ProductionType = e.Item.Cells(7).Text
                    End If

                    ' Check if this is the Same Nominee for the same Production
                    If ProductionID = e.Item.Cells(0).Text And LastNominatedName = e.Item.Cells(2).Text Then
                    Else
                        If Not (ProductionID = "" And LastNominatedName = "") Then
                            myLabel = e.Item.FindControl("AverageScore")
                            myLabel.Visible = True
                            myLabel.Text = "'" & LastNominatedName & "' Average: <B>" & Format(CDbl(LastAverageScore), "##,##0.0") & "</B>"
                        End If

                        ProductionID = e.Item.Cells(0).Text
                        LastNominatedName = e.Item.Cells(2).Text
                        LastAverageScore = e.Item.Cells(5).Text
                    End If
                Else
                    lblLastScore.Visible = True
                    lblLastScore.Text = "'" & LastNominatedName & "' Average: <B>" & Format(CDbl(LastAverageScore), "##,##0.0") & "</B>"
                End If

            End If

        Catch ex As Exception
            lblTotalNumberOfRecords.Text = "ERROR Calculating Totals: " & ex.Message
            lblTotalNumberOfRecords.ForeColor = System.Drawing.Color.Red
        End Try

    End Sub




End Class
