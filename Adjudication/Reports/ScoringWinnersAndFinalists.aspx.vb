Imports Adjudication.DataAccess

Partial Public Class ScoringWinnersAndFinalists
    Inherits System.Web.UI.Page

    Dim TopXProfessional As Integer = 1000
    Dim TopXCommunity As Integer = 1000
    Dim TopXYouth As Integer = 1000
    Dim ShowScores As Boolean = False            'FALSE For Production so we dont accidentally show scores

    Dim ProductionType As Integer = Nothing
    Dim ProductionCategory As Integer = Nothing

    Dim LastScore As Double = 0
    Dim TieScoreTracker As Boolean = True
    Dim NumOfRows As Int16 = 1
    Dim SortOrder As Int16 = 0              '0 value sorts by Score

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        ViewState.Item("AccessLevel") = CInt(Session.Item("AccessLevel"))
        If Not ViewState.Item("AccessLevel") > 0 Then Response.Redirect("../Login.aspx")
        '============================================================================================

        If Not IsPostBack Then
            If Not Session.Item("DisplayScores") = Nothing Then
                If Session.Item("DisplayScores") = "Yes" Then ShowScores = True
            End If

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

            '== FOR TESTING ===
            'ShowScores = True
            'Session.Item("ProductionCategoryID") = "3"
            'TopXProfessional = 3
            'TopXCommunity = 3
            'TopXYouth = 3

            Call Populate_DataGrid()

        End If

    End Sub

    Private Function ReduceTableDataToTopX(ByVal dt As DataTable, Optional ByVal ByProductionType As Boolean = True) As DataTable
        '============================================================================================
        Dim dtX As DataTable = dt.Clone
        Dim ProductionCategory As String = ""          'Pro/Comm/Youth
        Dim ProductionType As String = ""       'Musical or Drama/Comedy
        Dim iCountRows As Int16 = 1
        Dim SortWithProd As String = ""
        '============================================================================================
        For Each dRow As DataRow In dt.Rows
            If ByProductionType = True Then
                If (Not ProductionCategory = dRow.Item("ProductionCategory")) Or (Not ProductionType = dRow.Item("ProductionType")) Then
                    ProductionType = dRow.Item("ProductionType")
                    ProductionCategory = dRow.Item("ProductionCategory")
                    iCountRows = 1
                End If
            Else
                If (Not ProductionCategory = dRow.Item("ProductionCategory")) Then
                    ProductionType = dRow.Item("ProductionType")
                    ProductionCategory = dRow.Item("ProductionCategory")
                    iCountRows = 1
                End If
            End If

            Select Case ProductionCategory.ToLower
                Case "professional"
                    If iCountRows <= TopXProfessional Then dtX.ImportRow(dRow)
                Case "community"
                    If iCountRows <= TopXCommunity Then dtX.ImportRow(dRow)
                Case "youth"
                    If iCountRows <= TopXYouth Then dtX.ImportRow(dRow)
            End Select

            iCountRows = iCountRows + 1
        Next

        If ByProductionType = True Then SortWithProd = "ProductionType, "
        'If Not Session.Item("SortOrder") = Nothing Then
        '    If Session.Item("SortOrder") = "0" Then
        dtX.DefaultView.Sort = "ProductionCategory, " & SortWithProd & " AverageScoreFinal DESC, Title"
        '    Else
        '        dtX.DefaultView.Sort = "ProductionCategory, " & SortWithProd & " Title"
        '    End If
        'Else
        '    dtX.DefaultView.Sort = "ProductionCategory, " & SortWithProd & " Title"
        'End If

        Return dtX

    End Function

    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As DataTable, dtTest As New DataTable
        '====================================================================================================
        Me.lblErrors.Visible = False

        Try
            If Not Session.Item("ProductionTypeID") = Nothing Then ProductionType = CInt(Session.Item("ProductionTypeID"))
            If Not Session.Item("ProductionCategoryID") = Nothing Then
                ProductionCategory = CInt(Session.Item("ProductionCategoryID"))
                Select Case ProductionCategory
                    Case "1"
                        Me.lblProductionCategory.Text = "Professional "
                    Case "2"
                        Me.lblProductionCategory.Text = "Community "
                    Case "3"
                        Me.lblProductionCategory.Text = "Youth "
                    Case Else
                        Me.lblProductionCategory.Text = ""
                End Select
            End If

            Me.lblSortOrder.Text = "Finalists are listed Alphabetically by Production Name"

            dt = DataAccess.ReportScoring_Production(, ProductionCategory, ProductionType)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_Production.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_Production.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_Director(, ProductionCategory, ProductionType)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_Director.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_Director.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_MusicalDirector(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_MusicalDirector.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_MusicalDirector.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_Choreographer(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_Choreographer.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_Choreographer.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_LightingDesigner(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                dtTest = ReduceTableDataToTopX(dt)
                Me.grid_LightingDesigner.DataSource = ReduceTableDataToTopX(dt, False)
                Me.grid_LightingDesigner.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_SoundDesigner(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_SoundDesigner.DataSource = ReduceTableDataToTopX(dt, False)
                Me.grid_SoundDesigner.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_CostumeDesigner(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_CostumeDesigner.DataSource = ReduceTableDataToTopX(dt, False)
                Me.grid_CostumeDesigner.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_ScenicDesigner(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_ScenicDesigner.DataSource = ReduceTableDataToTopX(dt, False)
                Me.grid_ScenicDesigner.DataBind()
            End If


            dt.Clear()
            dt = DataAccess.ReportScoring_OriginalPlaywright(, ProductionCategory)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_OriginalPlaywright.DataSource = ReduceTableDataToTopX(dt, False)
                Me.grid_OriginalPlaywright.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_BestSupportingActress(, ProductionCategory, ProductionType)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_SupportingActress.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_SupportingActress.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_BestSupportingActor(, ProductionCategory, ProductionType)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_SupportingActor.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_SupportingActor.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_BestActorActress(2, 0, ProductionCategory, ProductionType)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_Actress.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_Actress.DataBind()
            End If

            dt.Clear()
            dt = DataAccess.ReportScoring_BestActorActress(1, 0, ProductionCategory, ProductionType)
            If dt.Rows.Count > 0 Then
                NumOfRows = 1
                Me.grid_Actor.DataSource = ReduceTableDataToTopX(dt)
                Me.grid_Actor.DataBind()
            End If


        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: " & ex.Message
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

    Public Sub SetDataGrid(ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs, Optional ByVal SeparateByProductionType As Boolean = True)
        '====================================================================================================
        '=== Create Color changes between Professional and Community
        '=== and Musical and Drama/Comedy
        '====================================================================================================
        Static ProductionCategory As String = ""        'Community or Professional or Youth
        Static ProductionType As String = ""     'Musical or Drama/Comedy
        Static CategoryType As String = ""
        '----------------------------------------------------------------------------------------------------
        Dim CompanyName As String = ""
        Dim Title As String = ""
        Dim BestName As String = ""
        Dim BestRole As String = ""
        Dim CategoryName As String = ""           'Director, Lighting Design, Actress, etc
        Dim AvgScore As String = ""
        Dim sTableString As New StringBuilder, sTmp As String = ""
        Dim TieScoreFound As Boolean = False
        '====================================================================================================

        '1) ProductionCategory" HeaderText="Category" 
        '2) ProductionType" HeaderText="Type" 
        '3) Title" HeaderText="Production" 
        '4) CompanyName" HeaderText="Theater Company" 
        '5) BestName" HeaderText="Nominee" 
        '6) BestRole" HeaderText="Role/Position" 
        '7) CategoryName

        Try
            Select Case e.Item.ItemType.ToString
                Case "Header"
                    e.Item.Visible = False
                Case "Footer"
                    e.Item.Visible = False
                Case Else
                    Title = e.Item.Cells(3).Text.Trim
                    CompanyName = e.Item.Cells(4).Text.Trim
                    BestName = e.Item.Cells(5).Text.Trim
                    BestRole = e.Item.Cells(6).Text.Trim
                    CategoryName = e.Item.Cells(7).Text.Trim
                    AvgScore = e.Item.Cells(8).Text.Trim

                    'Check if this is the same ProductionCategory and Production Type (ie: Community Musical)
                    If (SeparateByProductionType = True And (Not ProductionType = e.Item.Cells(2).Text)) Or (Not ProductionCategory = e.Item.Cells(1).Text) Or (Not CategoryType = e.Item.Cells(7).Text) Then
                        NumOfRows = 1       'reset on new section
                        TieScoreTracker = True
                        LastScore = CDbl(e.Item.Cells(8).Text)

                        If Not ProductionCategory = "" Then sTableString.Append("<hr style=""color:#ffcc00;"" size=""1"" />")

                        ProductionCategory = e.Item.Cells(1).Text.Trim
                        ProductionType = e.Item.Cells(2).Text.Trim
                        CategoryType = e.Item.Cells(7).Text.Trim
                        'Only display company type if Showing only 1 type of company
                        If Session.Item("ProductionCategoryID") = Nothing Then
                            e.Item.Cells(1).Text = " - " & e.Item.Cells(1).Text & " "
                        Else
                            e.Item.Cells(1).Text = " "
                        End If

                        'If Production Type is same as Company Type (for Youth company type) dont display Company Type
                        If ProductionType = ProductionCategory Then e.Item.Cells(1).Text = " "

                        'if Only Musical titles and design categories, dont display the "Musical" Production Type 
                        If CategoryName = "Music Director" Or _
                            CategoryName = "Lighting Designer" Or _
                            CategoryName = "Sound Designer" Or _
                            CategoryName = "Costume Designer" Or _
                            CategoryName = "Scenic Designer" Or _
                            CategoryName = "Original Playwright" Or _
                            CategoryName = "Choreographer" Then
                            e.Item.Cells(2).Text = ""
                        Else
                            e.Item.Cells(2).Text = " - " & e.Item.Cells(2).Text
                        End If

                        sTmp = "<div style=""color: Black; font-family: Verdana; font-size:small; font-variant: small-caps; text-decoration: underline;"">" & _
                                    "Best " & CategoryName & "<i>" & e.Item.Cells(2).Text & "</i> " & e.Item.Cells(1).Text & "</div>"
                        sTableString.Append(sTmp)
                    Else
                        If LastScore = CDbl(e.Item.Cells(8).Text) And TieScoreTracker = True Then
                            TieScoreFound = True
                        Else
                            TieScoreFound = False
                            TieScoreTracker = False
                        End If
                    End If

                    If NumOfRows = 1 Or TieScoreFound = True Then
                        sTmp = "<div style=""width:100%; color: Black; font-family: Verdana; font-size: small ;white-space:nowrap"">"
                        sTableString.Append(sTmp)
                    Else
                        sTmp = "<div style=""width:100%; color: GrayText; font-family: Verdana; font-size: x-small ;white-space:nowrap"">&nbsp;&nbsp;<font style=""font-variant: small-caps;;"">Finalist</span>&nbsp;"
                        sTableString.Append(sTmp)
                    End If

                    If TieScoreFound = True Then
                        sTableString.Append("&nbsp;&nbsp;&nbsp;&nbsp;<i><font style=""color: Navy; font-family: Verdana; font-variant: small-caps;"">Tie</span></i><br>")
                    End If

                    If Not BestName = BestRole Then
                        If BestRole = CategoryName Then
                            sTableString.Append("<b>" & BestName & "</b> - " & Title & " - <i>" & CompanyName & "</i>")       'Company Name                                          'Best Name
                        Else
                            sTableString.Append("<b>" & BestName & "</b> as <i>" & BestRole & "</i> in """ & Title & """ - <i>" & CompanyName & "</i>")                      'Best Name and Best Role
                        End If
                    Else
                        sTableString.Append("<b>" & Title & "</b> - <i>" & CompanyName & "</i>")       'Company Name                                          'Best Name
                    End If

                    If ShowScores = True Then
                        sTableString.Append(" [<font color=darkgreen>" & AvgScore & "</span>]")
                    End If

                    sTableString.Append("</div>")

                    e.Item.Cells(1).Text = sTableString.ToString
                    e.Item.Cells(2).Visible = False                                                                     'Hide all other cells in DataGrid
                    e.Item.Cells(3).Visible = False                                                                     'Hide all other cells in DataGrid
                    e.Item.Cells(4).Visible = False                                                                     'Hide all other cells in DataGrid
                    e.Item.Cells(5).Visible = False                                                                     'Hide all other cells in DataGrid
                    e.Item.Cells(6).Visible = False                                                                     'Hide all other cells in DataGrid
                    e.Item.Cells(7).Visible = False                                                                     'Hide all other cells in DataGrid
                    e.Item.Cells(8).Visible = False                                                                     'Hide all other cells in DataGrid

                    If ProductionCategory.ToLower = "professional" Then
                        If (NumOfRows >= TopXProfessional + 1) Then
                            e.Item.Visible = False
                        End If
                    ElseIf ProductionCategory.ToLower = "community" Then
                        If (NumOfRows >= TopXCommunity + 1) Then
                            e.Item.Visible = False
                        End If
                    ElseIf ProductionCategory.ToLower = "youth" Then
                        If (NumOfRows >= TopXYouth + 1) Then
                            e.Item.Visible = False
                        End If
                    End If

                    NumOfRows = NumOfRows + 1

            End Select

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR Calculating Totals: " & ex.Message
            Me.lblErrors.Visible = True
        End Try

    End Sub

End Class