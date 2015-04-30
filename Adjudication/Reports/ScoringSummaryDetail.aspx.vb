Imports Adjudication.DataAccess

Public Class ScoringSummaryDetail
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
    Protected WithEvents grid_Director As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
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
    Protected WithEvents grid_Production As System.Web.UI.WebControls.DataGrid

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
        Dim dt As DataTable
        Dim ProductionType As Integer
        Dim ProductionCategory As Integer
        '====================================================================================================

        Me.lblErrors.Visible = False

        Try
            If Not Session.Item("ProductionTypeID") = Nothing Then ProductionType = CInt(Session.Item("ProductionTypeID"))
            If Not Session.Item("ProductionCategoryID") = Nothing Then ProductionCategory = CInt(Session.Item("ProductionCategoryID"))

            dt = DataAccess.ReportScoring_Production(, ProductionCategory, ProductionType)
            Me.grid_Production.DataSource = dt
            Me.grid_Production.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_Director(, ProductionCategory, ProductionType)
            Me.grid_Director.DataSource = dt
            Me.grid_Director.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_MusicalDirector(, ProductionCategory)
            Me.grid_MusicalDirector.DataSource = dt
            Me.grid_MusicalDirector.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_Choreographer(, ProductionCategory)
            Me.grid_Choreographer.DataSource = dt
            Me.grid_Choreographer.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_LightingDesigner(, ProductionCategory)
            Me.grid_LightingDesigner.DataSource = dt
            Me.grid_LightingDesigner.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_SoundDesigner(, ProductionCategory)
            Me.grid_SoundDesigner.DataSource = dt
            Me.grid_SoundDesigner.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_CostumeDesigner(, ProductionCategory)
            Me.grid_CostumeDesigner.DataSource = dt
            Me.grid_CostumeDesigner.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_ScenicDesigner(, ProductionCategory)
            Me.grid_ScenicDesigner.DataSource = dt
            Me.grid_ScenicDesigner.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_OriginalPlaywright(, ProductionCategory)
            Me.grid_OriginalPlaywright.DataSource = dt
            Me.grid_OriginalPlaywright.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_BestActorActress(1, 0, ProductionCategory, ProductionType)
            Me.grid_Actor.DataSource = dt
            Me.grid_Actor.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_BestActorActress(2, 0, ProductionCategory, ProductionType)
            Me.grid_Actress.DataSource = dt
            Me.grid_Actress.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_BestSupportingActor(, ProductionCategory, ProductionType)
            Me.grid_SupportingActor.DataSource = dt
            Me.grid_SupportingActor.DataBind()
            dt.Clear()
            dt = DataAccess.ReportScoring_BestSupportingActress(, ProductionCategory, ProductionType)
            Me.grid_SupportingActress.DataSource = dt
            Me.grid_SupportingActress.DataBind()

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
        Static ScoreBackColor As System.Drawing.Color = System.Drawing.Color.LemonChiffon

        Try
            Select Case e.Item.ItemType.ToString
                Case "Header"
                    'Reset values for new DataGrid 
                    NumOfRows = 1
                    ProductionType = ""
                    ProductionCategory = ""
                    ScoreBackColor = System.Drawing.Color.LemonChiffon
                    'If Not Session.Item("DisplayScores") = "Yes" Then e.Item.Cells(7).Text = ""

                Case "Footer"
                    If NumOfRows < TopXProfessional + 1 Then
                        e.Item.Visible = True
                    ElseIf NumOfRows < TopXCommunity + 1 Then
                        e.Item.Visible = True
                    ElseIf NumOfRows < TopXYouth + 1 Then
                        e.Item.Visible = True
                    End If

                Case Else
                    'Check if this is the same ProductionCategory and Production Type (ie: Community Musical)
                    If (SeparateByProductionType = True And (Not ProductionType = e.Item.Cells(2).Text)) Or (Not ProductionCategory = e.Item.Cells(1).Text) Then
                        NumOfRows = 1       'reset on new section
                        ProductionType = e.Item.Cells(2).Text
                        ProductionCategory = e.Item.Cells(1).Text
                        If ScoreBackColor.ToString = System.Drawing.Color.LemonChiffon.ToString Then
                            ScoreBackColor = System.Drawing.Color.White
                        Else
                            ScoreBackColor = System.Drawing.Color.LemonChiffon
                        End If
                    End If

                    'If Not Session.Item("DisplayScores") = "Yes" Then e.Item.Cells(7).Text = ""
                    e.Item.BackColor = ScoreBackColor

                    If ProductionCategory.ToLower = "professional" Then
                        If NumOfRows = TopXProfessional + 1 Then
                            e.Item.BackColor = System.Drawing.Color.DarkGoldenrod
                            e.Item.ForeColor = System.Drawing.Color.DarkGoldenrod
                            For Each myCell As TableCell In e.Item.Cells
                                myCell.Text = ""
                            Next
                            'Exit Sub
                        Else
                            If NumOfRows > TopXProfessional + 1 Then e.Item.Visible = False
                        End If
                    ElseIf ProductionCategory.ToLower = "community" Then
                        If NumOfRows = TopXCommunity + 1 Then
                            e.Item.BackColor = System.Drawing.Color.Black
                            e.Item.ForeColor = System.Drawing.Color.Black
                            For Each myCell As TableCell In e.Item.Cells
                                myCell.Text = ""
                            Next
                            'Exit Sub
                        Else
                            If NumOfRows > TopXCommunity + 1 Then e.Item.Visible = False
                        End If
                    ElseIf ProductionCategory.ToLower = "youth" Then
                        If NumOfRows = TopXYouth + 1 Then
                            e.Item.BackColor = System.Drawing.Color.DarkBlue
                            e.Item.ForeColor = System.Drawing.Color.DarkBlue
                            For Each myCell As TableCell In e.Item.Cells
                                myCell.Text = ""
                            Next
                            'Exit Sub
                        Else
                            If NumOfRows > TopXYouth + 1 Then e.Item.Visible = False
                        End If
                    End If

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
