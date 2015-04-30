Imports Adjudication.DataAccess

Public Class ScoringAdjudicatorDetail
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lbtnAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents LastProductionScore As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lblCategoryName As System.Web.UI.WebControls.Label
    Protected WithEvents grid_Scoring As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub



    Dim TopXProfessional As Integer = 1000
    Dim TopXCommunity As Integer = 1000
    Dim TopXYouth As Integer = 1000
    Dim NumOfRows As Int16 = 1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        ViewState.Item("AccessLevel") = CInt(Session.Item("AccessLevel"))
        If Not ViewState.Item("AccessLevel") > 0 Then Response.Redirect("../Login.aspx")
        Me.lblLoginID.Text = Session("LoginID")
        '============================================================================================

        ''Session.Item("Category") = "Production"           '===> FOR TESTING ONLY <===
        If Not IsPostBack Then
            '=== Professional Top X ===
            Try
                If CInt(Session.Item("TopXProfessional")) > 0 Then TopXProfessional = Session.Item("TopXProfessional")
            Catch ex As Exception
                'do nothing
            End Try

            '=== Community Top X ===
            Try
                If CInt(Session.Item("TopXCommunity")) > 0 Then TopXCommunity = Session.Item("TopXCommunity")
            Catch ex As Exception
                'do nothing
            End Try

            '=== YOUTH Top X ===
            Try
                If CInt(Session.Item("TopXYouth")) > 0 Then TopXYouth = Session.Item("TopXYouth")
            Catch ex As Exception
                'do nothing
            End Try

            Call Populate_DataGrid()

        End If

    End Sub

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As New DataTable
        Dim ProductionType As Integer
        Dim ProductionCategory As Integer
        '====================================================================================================
        Me.lblErrors.Visible = False

        Try
            If Not Session.Item("ProductionTypeID") = Nothing Then ProductionType = CInt(Session.Item("ProductionTypeID"))
            If Not Session.Item("ProductionCategoryID") = Nothing Then ProductionCategory = CInt(Session.Item("ProductionCategoryID"))

            If Not Session.Item("Category") = Nothing Then
                Me.lblCategoryName.Text = Session.Item("Category")

                Select Case Session.Item("Category").ToString
                    Case "Production"
                        dt = DataAccess.ReportScoring_Production(1, ProductionCategory, ProductionType)
                    Case "Director"
                        dt = DataAccess.ReportScoring_Director(1, ProductionCategory, ProductionType)
                    Case "Music Director"
                        dt = DataAccess.ReportScoring_MusicalDirector(1, ProductionCategory)
                    Case "Choreographer"
                        dt = DataAccess.ReportScoring_Choreographer(1, ProductionCategory)
                    Case "Lighting Designer"
                        dt = DataAccess.ReportScoring_LightingDesigner(1, ProductionCategory)
                    Case "Sound Designer"
                        dt = DataAccess.ReportScoring_SoundDesigner(1, ProductionCategory)
                    Case "Costume Designer"
                        dt = DataAccess.ReportScoring_CostumeDesigner(1, ProductionCategory)
                    Case "Scenic Designer"
                        dt = DataAccess.ReportScoring_ScenicDesigner(1, ProductionCategory)
                    Case "Original Playwright"
                        dt = DataAccess.ReportScoring_OriginalPlaywright(1, ProductionCategory)
                    Case "Best Actor"
                        dt = DataAccess.ReportScoring_BestActorActress(1, 1, ProductionCategory, ProductionType)
                    Case "Best Actress"
                        dt = DataAccess.ReportScoring_BestActorActress(2, 1, ProductionCategory, ProductionType)
                    Case "Best Supporting Actor"
                        dt = DataAccess.ReportScoring_BestSupportingActor(1, ProductionCategory, ProductionType)
                    Case "Best Supporting Actress"
                        dt = DataAccess.ReportScoring_BestSupportingActress(1, ProductionCategory, ProductionType)
                    Case Else
                        Me.lblErrors.Visible = True
                        Me.lblErrors.Text = "ERROR: Unknown Category selected to run report against."
                        Exit Sub
                End Select
            End If

            Me.grid_Scoring.DataSource = dt
            Me.grid_Scoring.DataBind()
            'dt.Clear()

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Me.lblErrors.Visible = True
        End Try
    End Sub

    Public Sub SetDataGrid(ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs, Optional ByVal SeparateByProductionType As Boolean = True)
        '====================================================================================================
        '=== Create Color changes between Professional and Community
        '=== and Musical and Drama/Comedy
        '====================================================================================================
        Static ProductionType As String = ""     'Musical or Drama/Comedy
        Static ProductionCategory As String = ""        'Community or Professional
        Static Title As String = ""              'Production Name
        Static CompanyName As String = ""        'Producing Company Name
        Static BestName As String = ""        'Producing Company Name
        Static BestRole As String = ""        'Producing Company Name
        'Static ScoreBackColor As System.Drawing.Color = System.Drawing.Color.LemonChiffon

        Try
            Select Case e.Item.ItemType.ToString
                Case "Header"
                    'Reset values for new DataGrid 
                    NumOfRows = 1
                    ProductionType = ""
                    ProductionCategory = ""
                    Title = ""
                    CompanyName = ""
                    'ScoreBackColor = System.Drawing.Color.LemonChiffon
                    'If Not Session.Item("DisplayScores") = "Yes" Then e.Item.Cells(7).Text = ""

                Case "Footer"

                Case Else
                    Dim MyTable As Table = e.Item.FindControl("tblProdData")

                    If ((Not Title = e.Item.Cells(3).Text)) Or (Not CompanyName = e.Item.Cells(4).Text) _
                           Or (Not BestName = e.Item.Cells(5).Text) Or (Not BestRole = e.Item.Cells(6).Text) Then
                        Title = e.Item.Cells(3).Text
                        CompanyName = e.Item.Cells(4).Text
                        BestName = e.Item.Cells(5).Text
                        BestRole = e.Item.Cells(6).Text
                        MyTable.Visible = True
                    Else
                        MyTable.Visible = False
                    End If

                    'Check if this is the same ProductionCategory and Production Type (ie: Community Musical)
                    If ((Not ProductionType = e.Item.Cells(2).Text)) Or (Not ProductionCategory = e.Item.Cells(1).Text) Then
                        Dim HeaderTable As Table = e.Item.FindControl("tblHeader")
                        HeaderTable.Visible = True

                        NumOfRows = 1       'reset on new section
                        ProductionCategory = e.Item.Cells(1).Text
                        ProductionType = e.Item.Cells(2).Text
                        'If ScoreBackColor.ToString = System.Drawing.Color.LemonChiffon.ToString Then
                        '    ScoreBackColor = System.Drawing.Color.White
                        'Else
                        '    ScoreBackColor = System.Drawing.Color.LemonChiffon
                        'End If
                    End If

                    'e.Item.BackColor = ScoreBackColor

                    'If ProductionCategory.ToLower = "professional" Then
                    '    If NumOfRows = TopXProfessional + 1 Then
                    '        e.Item.BackColor = System.Drawing.Color.DarkGoldenrod
                    '        e.Item.ForeColor = System.Drawing.Color.DarkGoldenrod
                    '        For Each myCell As TableCell In e.Item.Cells
                    '            myCell.Text = ""
                    '        Next
                    '        'Exit Sub
                    '    Else
                    '        If NumOfRows > TopXProfessional + 1 Then e.Item.Visible = False
                    '    End If
                    'Else
                    '    If NumOfRows = TopXCommunity + 1 Then
                    '        e.Item.BackColor = System.Drawing.Color.DarkGoldenrod
                    '        e.Item.ForeColor = System.Drawing.Color.DarkGoldenrod
                    '        For Each myCell As TableCell In e.Item.Cells
                    '            myCell.Text = ""
                    '        Next
                    '        'Exit Sub
                    '    Else
                    '        If NumOfRows > TopXCommunity + 1 Then e.Item.Visible = False
                    '    End If
                    'End If

                    NumOfRows = NumOfRows + 1

            End Select

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR Calculating Totals: " & ex.Message
            Me.lblErrors.Visible = True
        End Try

    End Sub

    Public Sub grid_Scoring_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Scoring.ItemDataBound
        SetDataGrid(e, True)
    End Sub

End Class
