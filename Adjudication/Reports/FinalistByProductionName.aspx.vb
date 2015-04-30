Imports Adjudication.DataAccess

Public Class FinalistByProductionName
    Inherits System.Web.UI.Page

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
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
    Protected WithEvents grid_Production_Musical As System.Web.UI.WebControls.DataGrid
    Protected WithEvents grid_Production As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblSortOrder As System.Web.UI.WebControls.Label

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
        Me.lblLoginID.Text = Session("LoginID")
        '============================================================================================

        If Not IsPostBack Then
            If Session.Item("SortOrder") IsNot Nothing Then SortOrder = CInt(Session.Item("SortOrder"))

            TopXProfessional = CoalesceSessionItem("TopXProfessional", 1000)
            TopXCommunity = CoalesceSessionItem("TopXCommunity", 1000)
            TopXYouth = CoalesceSessionItem("TopXYouth", 1000)

            Call Populate_DataGrid()
        End If

    End Sub

    Private Sub Populate_DataGrid()
        Dim dt As DataTable
        Dim ProductionType As Integer = Nothing
        Dim ProductionCategory As Integer = Nothing

        Me.lblErrors.Visible = False

        Try
            If Session.Item("ProductionTypeID") IsNot Nothing Then ProductionType = CInt(Session.Item("ProductionTypeID"))
            If Session.Item("ProductionCategoryID") IsNot Nothing Then ProductionCategory = CInt(Session.Item("ProductionCategoryID"))

            If SortOrder = 1 Then
                Me.lblSortOrder.Text = "Finalists are listed Alphabetically by Production Name"
            Else
                Me.lblSortOrder.Text = "Finalists are listed In Order of Final Score"
            End If

            dt = DataAccess.ReportScoring_Production(, ProductionCategory, ProductionType)
            Me.grid_Production.DataSource = ReduceTableDataToTopX(dt)
            Me.grid_Production.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_Director(, ProductionCategory, ProductionType)
            Me.grid_Director.DataSource = ReduceTableDataToTopX(dt)
            Me.grid_Director.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_MusicalDirector(, ProductionCategory)
            Me.grid_MusicalDirector.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_MusicalDirector.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_Choreographer(, ProductionCategory)
            Me.grid_Choreographer.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_Choreographer.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_LightingDesigner(, ProductionCategory)
            Me.grid_LightingDesigner.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_LightingDesigner.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_SoundDesigner(, ProductionCategory)
            Me.grid_SoundDesigner.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_SoundDesigner.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_CostumeDesigner(, ProductionCategory)
            Me.grid_CostumeDesigner.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_CostumeDesigner.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_ScenicDesigner(, ProductionCategory)
            Me.grid_ScenicDesigner.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_ScenicDesigner.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_OriginalPlaywright(, ProductionCategory)
            Me.grid_OriginalPlaywright.DataSource = ReduceTableDataToTopX(dt, False)
            Me.grid_OriginalPlaywright.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_BestActorActress(DataAccess.ActorActress.Actor, 0, ProductionCategory, ProductionType)
            Me.grid_Actor.DataSource = ReduceTableDataToTopX(dt)
            Me.grid_Actor.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_BestActorActress(DataAccess.ActorActress.Actress, 0, ProductionCategory, ProductionType)
            Me.grid_Actress.DataSource = ReduceTableDataToTopX(dt)
            Me.grid_Actress.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_BestSupportingActor(, ProductionCategory, ProductionType)
            Me.grid_SupportingActor.DataSource = ReduceTableDataToTopX(dt)
            Me.grid_SupportingActor.DataBind()

            dt.Clear()
            dt = DataAccess.ReportScoring_BestSupportingActress(, ProductionCategory, ProductionType)
            Me.grid_SupportingActress.DataSource = ReduceTableDataToTopX(dt)
            Me.grid_SupportingActress.DataBind()

        Catch ex As Exception
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Me.lblErrors.Visible = True
        End Try
    End Sub

    Private Function CoalesceSessionItem(ByVal sessionItem As String, ByVal defaultValue As Integer) As Integer
        Dim itemValue As Integer

        Try
            itemValue = CInt(Session.Item(sessionItem))
        Catch ex As Exception
            Return defaultValue
        End Try

        If itemValue > 0 Then
            Return itemValue
        Else
            Return defaultValue
        End If
    End Function

    Private Function ReduceTableDataToTopX(ByVal dt As DataTable, Optional ByVal ByProductionType As Boolean = True) As DataTable

        Dim dtX As DataTable = dt.Clone
        Dim ProductionCategory As String = ""   'Professional | Community | Youth
        Dim ProductionType As String = ""       'Musical | Drama/Comedy
        Dim iCountRows As Int16 = 1

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

        If SortOrder = 1 Then dtX.DefaultView.Sort = "ProductionCategory, ProductionType, Title"

        Return dtX

    End Function


    Public Sub grid_Director_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Director.ItemDataBound
        SetDataGridFinalists(e, True, "Best Director", True, False)
    End Sub

    Public Sub grid_MusicalDirector_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_MusicalDirector.ItemDataBound
        SetDataGridFinalists(e, False, "Best Musical Director", True, False)
    End Sub

    Public Sub grid_Choreographer_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Choreographer.ItemDataBound
        SetDataGridFinalists(e, False, "Best Choreographer", True, False)
    End Sub

    Public Sub grid_LightingDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_LightingDesigner.ItemDataBound
        SetDataGridFinalists(e, False, "Best Lighting Designer", True, False)
    End Sub

    Public Sub grid_SoundDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_SoundDesigner.ItemDataBound
        SetDataGridFinalists(e, False, "Best Sound Designer", True, False)
    End Sub

    Public Sub grid_CostumeDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_CostumeDesigner.ItemDataBound
        SetDataGridFinalists(e, False, "Best Costume Designer", True, False)
    End Sub

    Public Sub grid_ScenicDesigner_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_ScenicDesigner.ItemDataBound
        SetDataGridFinalists(e, False, "Best Scenic Designer", True, False)
    End Sub

    Public Sub grid_OriginalPlaywright_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_OriginalPlaywright.ItemDataBound
        SetDataGridFinalists(e, False, "Best Original Playwright", True, False)
    End Sub

    Public Sub grid_Actor_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Actor.ItemDataBound
        SetDataGridFinalists(e, True, "Best Actor", True, True)
    End Sub

    Public Sub grid_Actress_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Actress.ItemDataBound
        SetDataGridFinalists(e, True, "Best Actress", True, True)
    End Sub

    Public Sub grid_SupportingActor_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_SupportingActor.ItemDataBound
        SetDataGridFinalists(e, True, "Best Supporting Actor", True, True)
    End Sub

    Public Sub grid_SupportingActress_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_SupportingActress.ItemDataBound
        SetDataGridFinalists(e, True, "Best Supporting Actress", True, True)
    End Sub

    Public Sub grid_Production_DataItemBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid_Production.ItemDataBound
        SetDataGridFinalists(e, True, "Best Production")
    End Sub


    Public Sub SetDataGridFinalists(ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs, _
                                        Optional ByVal SeparateByProductionType As Boolean = True, _
                                        Optional ByVal CategoryName As String = "", _
                                        Optional ByVal DisplayNomineeName As Boolean = False, _
                                        Optional ByVal DisplayRoleName As Boolean = False)
        '====================================================================================================
        '=== Create Color changes between Professional and Community
        '=== and Musical and Drama/Comedy
        '====================================================================================================
        Static ProductionType As String = ""     'Musical or Drama/Comedy
        Static ProductionCategory As String = ""        'Community or Professional
        Static ProductionCategoryHeader As String = ""        'Community or Professional
        Dim sHeaderHTML As String = ""
        Dim DisplayName As String = Nothing
        Dim DisplayProductionType As String = Nothing

        Try
            Select Case e.Item.ItemType.ToString
                Case "Header"
                    'Reset values for new DataGrid 
                    NumOfRows = 1
                    ProductionType = ""
                    ProductionCategory = ""

                Case "Footer"
                    'If NumOfRows < TopXProfessional + 1 Then
                    '    e.Item.Visible = True
                    'ElseIf NumOfRows < TopXCommunity + 1 Then
                    '    e.Item.Visible = True
                    'End If

                Case Else
                    If DisplayNomineeName = True Then
                        If DisplayRoleName = True Then
                            DisplayName = " (" & e.Item.Cells(5).Text & " <font style=""FONT-SIZE: x-small""><i>as " & e.Item.Cells(6).Text & "</i></span>)"
                        Else
                            DisplayName = " (" & e.Item.Cells(5).Text & ")"
                        End If
                    End If

                    'Check if this is the same ProductionCategory and Production Type (ie: Community Musical)
                    If (SeparateByProductionType = True And (Not ProductionType = e.Item.Cells(2).Text)) Or (Not ProductionCategory = e.Item.Cells(1).Text) Then
                        NumOfRows = 1       'reset on new section
                        ProductionType = e.Item.Cells(2).Text

                        If ProductionCategory = e.Item.Cells(1).Text Then
                            ProductionCategoryHeader = Nothing
                        Else
                            ProductionCategory = e.Item.Cells(1).Text
                            ProductionCategoryHeader = "<br />" & e.Item.Cells(1).Text.ToUpper
                        End If

                        If CategoryName.Length = 0 Then CategoryName = e.Item.Cells(6).Text
                        If SeparateByProductionType = True Then DisplayProductionType = " - <i>" & e.Item.Cells(2).Text & "</i>"

                        sHeaderHTML = sHeaderHTML & "<tr>"
                        sHeaderHTML = sHeaderHTML & "<td style=""FONT-SIZE: medium"" width=""100%"">" & _
                                                      ProductionCategoryHeader & "<br /><U>" & CategoryName & DisplayProductionType & "</U></td></tr>"
                        sHeaderHTML = "<table width=""100%;FONT-SIZE: small"">" & sHeaderHTML & "</table>"

                        e.Item.Cells(7).Text = sHeaderHTML & "&nbsp;&nbsp;" & e.Item.Cells(4).Text & " - <I>" & e.Item.Cells(3).Text & "</I> " & DisplayName
                    Else
                        e.Item.Cells(7).Text = "&nbsp;&nbsp;" & e.Item.Cells(4).Text & " - <I>" & e.Item.Cells(3).Text & "</I> " & DisplayName
                    End If

                    'If ProductionCategory.ToLower = "professional" Then
                    '    If NumOfRows = TopXProfessional + 1 Then
                    '        e.Item.BackColor = System.Drawing.Color.Black
                    '        e.Item.ForeColor = System.Drawing.Color.Black
                    '        For Each myCell As TableCell In e.Item.Cells
                    '            myCell.Text = ""
                    '        Next
                    '        'Exit Sub
                    '    Else
                    '        If NumOfRows > TopXProfessional + 1 Then e.Item.Visible = False
                    '    End If
                    'Else
                    '    If NumOfRows = TopXCommunity + 1 Then
                    '        e.Item.BackColor = System.Drawing.Color.Black
                    '        e.Item.ForeColor = System.Drawing.Color.Black
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

End Class
