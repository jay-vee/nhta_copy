Imports Adjudication.DataAccess

Partial Class ScoringSummaryTopNominees
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
    Protected WithEvents grid_Director As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents grid_MusicalDirector As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_Choreographer As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_LightingDesigner As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_SoundDesigner As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_CostumeDesigner As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_ScenicDesigner As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_OriginalPlaywright As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_Actor As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_Actress As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_SupportingActor As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_SupportingActress As System.Web.UI.WebControls.DataGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub



    Dim TopXProfessional As Integer
    Dim TopXCommunity As Integer
    Dim TopXYouth As Integer
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
        '============================================================================================
        '==>>> THIS REPORT IS NOT FINISHED <<<<======
        '============================================================================================
        '============================================================================================
        If Not IsPostBack Then
            '=== Professional Top X ===
            Try
                If CInt(Session.Item("TopXProfessional")) > 0 Then
                    TopXProfessional = Session.Item("TopXProfessional")
                Else
                    TopXProfessional = 1000
                    'TopXProfessional = 3  'FOR TESTING
                End If
            Catch ex As Exception               'do nothing
                TopXProfessional = 1000
                'TopXProfessional = 3  'FOR TESTING
            End Try

            '=== Community Top X ===
            Try
                If CInt(Session.Item("TopXCommunity")) > 0 Then
                    TopXCommunity = Session.Item("TopXCommunity")
                Else
                    TopXCommunity = 1000
                    'TopXCommunity = 10  'FOR TESTING
                End If
            Catch ex As Exception                 'do nothing
                TopXCommunity = 1000
                'TopXCommunity = 10      'FOR TESTING
            End Try

            '=== YOUTH Top X ===
            Try
                If CInt(Session.Item("TopXYouth")) > 0 Then
                    TopXYouth = Session.Item("TopXYouth")
                Else
                    TopXYouth = 1000
                    'TopXYouth = 5  'FOR TESTING
                End If
            Catch ex As Exception                 'do nothing
                TopXYouth = 1000
                'TopXYouth = 5      'FOR TESTING
            End Try

            Call Populate_DataGrid()

        End If

    End Sub

    Private Function SortAndLimitData(ByVal dt As DataTable, Optional ByVal NumOfRows As Int16 = 0) As DataTable
        Dim dtNew As New DataTable, dtTemp As New DataTable

        Try
            Dim CurrentPos As Integer = 0, StartPos As Integer = 0
            Dim ProductionCategory As String = dt.Rows(0)("ProductionCategory")
            Dim ProductionType As String = dt.Rows(0)("ProductionType")

            If dt.Rows.Count > NumOfRows Then
                For CurrentPos = 0 To dt.Rows.Count - 1
                    If (Not ProductionType = dt.Rows(CurrentPos)("ProductionType")) Or (Not ProductionCategory = dt.Rows(CurrentPos)("ProductionCategory")) Then
                        ProductionCategory = dt.Rows(CurrentPos)("ProductionCategory")
                        ProductionType = dt.Rows(CurrentPos)("ProductionType")
                        For j As Int16 = StartPos To CurrentPos
                            dtTemp.ImportRow(dt.Rows(j))
                        Next
                        dtTemp.DefaultView.Sort = "Title"

                        For r As Int16 = 0 To dtTemp.Rows.Count - 1
                            dtNew.ImportRow(dtTemp.Rows(r))
                        Next
                        dtTemp.Clear()
                        StartPos = CurrentPos
                    Else
                        If (CurrentPos - StartPos) <= NumOfRows Then dtTemp.ImportRow(dt.Rows(CurrentPos))
                    End If
                Next
            End If

            If dtNew.Rows.Count = 0 Then
                dt.DefaultView.Sort = "Title"
                dtNew = dt.Copy
            End If

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Me.lblErrors.Visible = True
        End Try

        Return dtNew

    End Function

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dtProf As DataTable, dtComm As DataTable, dtYouth As DataTable
        Dim ProductionType As Integer = Nothing
        Dim ProductionCategory As Integer = Nothing
        Dim TopX As Integer = 3
        '====================================================================================================
        Me.lblErrors.Visible = False

        Try
            If Not Session.Item("ProductionTypeID") = Nothing Then ProductionType = CInt(Session.Item("ProductionTypeID"))
            If Not Session.Item("ProductionCategoryID") = Nothing Then
                ProductionCategory = CInt(Session.Item("ProductionCategoryID"))
                Select Case ProductionCategory
                    Case 1
                        TopX = TopXProfessional
                    Case 2
                        TopX = TopXCommunity
                    Case 3
                        TopX = TopXYouth
                End Select
            End If

            If ProductionCategory = Nothing Then
                dtProf = SortAndLimitData(ReportScoring_Production(, 1, ProductionType), TopXProfessional)
                dtComm = SortAndLimitData(ReportScoring_Production(, 2, ProductionType), TopXCommunity)
                dtYouth = SortAndLimitData(ReportScoring_Production(, 2, ProductionType), TopXYouth)
                For i As Integer = 0 To dtComm.Rows.Count - 1
                    dtProf.ImportRow(dtComm.Rows(i))
                Next
            Else
                dtProf = SortAndLimitData(ReportScoring_Production(, ProductionCategory, ProductionType), TopX)
            End If

            Me.grid_Production.DataSource = dtProf
            Me.grid_Production.DataBind()

            'dt.Clear()
            'dt = DataAccess.ReportScoring_Director(, ProductionCategory, ProductionType)
            'Me.grid_Director.DataSource = dt
            'Me.grid_Director.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_MusicalDirector(, ProductionCategory)
            'Me.grid_MusicalDirector.DataSource = dt
            'Me.grid_MusicalDirector.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_Choreographer(, ProductionCategory)
            'Me.grid_Choreographer.DataSource = dt
            'Me.grid_Choreographer.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_LightingDesigner(, ProductionCategory)
            'Me.grid_LightingDesigner.DataSource = dt
            'Me.grid_LightingDesigner.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_SoundDesigner(, ProductionCategory)
            'Me.grid_SoundDesigner.DataSource = dt
            'Me.grid_SoundDesigner.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_CostumeDesigner(, ProductionCategory)
            'Me.grid_CostumeDesigner.DataSource = dt
            'Me.grid_CostumeDesigner.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_ScenicDesigner(, ProductionCategory)
            'Me.grid_ScenicDesigner.DataSource = dt
            'Me.grid_ScenicDesigner.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_OriginalPlaywright(, ProductionCategory)
            'Me.grid_OriginalPlaywright.DataSource = dt
            'Me.grid_OriginalPlaywright.DataBind()

            'dt.Clear()
            'dt = DataAccess.ReportScoring_BestActorActress(1, 0, ProductionCategory, ProductionType)
            'Me.grid_Actor.DataSource = dt
            'Me.grid_Actor.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_BestActorActress(2, 0, ProductionCategory, ProductionType)
            'Me.grid_Actress.DataSource = dt
            'Me.grid_Actress.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_BestSupportingActor(, ProductionCategory, ProductionType)
            'Me.grid_SupportingActor.DataSource = dt
            'Me.grid_SupportingActor.DataBind()
            'dt.Clear()
            'dt = DataAccess.ReportScoring_BestSupportingActress(, ProductionCategory, ProductionType)
            'Me.grid_SupportingActress.DataSource = dt
            'Me.grid_SupportingActress.DataBind()

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
        Static ScoreBackColor As System.Drawing.Color = System.Drawing.Color.LemonChiffon

        Try
            Select Case e.Item.ItemType.ToString
                Case "Header"
                    'Reset values for new DataGrid 
                    NumOfRows = 1
                    ProductionType = ""
                    ProductionCategory = ""
                    Title = ""
                    CompanyName = ""
                    ScoreBackColor = System.Drawing.Color.LemonChiffon
                    'If Not Session.Item("DisplayScores") = "Yes" Then e.Item.Cells(7).Text = ""

                Case "Footer"

                Case Else
                    Dim HeaderTable As Table = e.Item.FindControl("tblHeader")
                    If (Not ProductionCategory = e.Item.Cells(1).Text) Then
                        NumOfRows = 1       'reset on new section
                        ProductionCategory = e.Item.Cells(1).Text
                        HeaderTable.Visible = True
                    Else
                        HeaderTable.Visible = False
                    End If

                    'If ((Not Title = e.Item.Cells(3).Text)) Or (Not CompanyName = e.Item.Cells(4).Text) _
                    '       Or (Not BestName = e.Item.Cells(5).Text) Or (Not BestRole = e.Item.Cells(6).Text) Then
                    '    Title = e.Item.Cells(3).Text
                    '    CompanyName = e.Item.Cells(4).Text
                    '    BestName = e.Item.Cells(5).Text
                    '    BestRole = e.Item.Cells(6).Text
                    '    HeaderTable.Visible = True
                    'Else
                    '    HeaderTable.Visible = False
                    'End If

                    'Check if this is the same ProductionCategory and Production Type (ie: Community Musical)
                    If ((Not ProductionType = e.Item.Cells(2).Text)) Then
                        Dim ProdTable As Table = e.Item.FindControl("tblProdData")
                        ProdTable.Visible = True

                        NumOfRows = 1       'reset on new section
                        ProductionType = e.Item.Cells(2).Text
                        'If ScoreBackColor.ToString = System.Drawing.Color.LemonChiffon.ToString Then
                        '    ScoreBackColor = System.Drawing.Color.White
                        'Else
                        '    ScoreBackColor = System.Drawing.Color.LemonChiffon
                        'End If
                    End If

                    'e.Item.BackColor = ScoreBackColor

                    NumOfRows = NumOfRows + 1

            End Select

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR Calculating Totals: " & ex.Message
            Me.lblErrors.Visible = True
        End Try

    End Sub

    Public Sub grid_Director_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Director.ItemDataBound
        SetDataGrid(e, True)
    End Sub

    Public Sub grid_MusicalDirector_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_MusicalDirector.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_Choreographer_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Choreographer.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_LightingDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_LightingDesigner.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_SoundDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_SoundDesigner.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_CostumeDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_CostumeDesigner.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_ScenicDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_ScenicDesigner.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_OriginalPlaywright_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_OriginalPlaywright.ItemDataBound
        SetDataGrid(e, False)
    End Sub

    Public Sub grid_Actor_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Actor.ItemDataBound
        SetDataGrid(e, True)
    End Sub

    Public Sub grid_Actress_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Actress.ItemDataBound
        SetDataGrid(e, True)
    End Sub

    Public Sub grid_SupportingActor_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_SupportingActor.ItemDataBound
        SetDataGrid(e, True)
    End Sub

    Public Sub grid_SupportingActress_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_SupportingActress.ItemDataBound
        SetDataGrid(e, True)
    End Sub

    Public Sub grid_Production_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Production.ItemDataBound
        SetDataGrid(e, True)
    End Sub


End Class
