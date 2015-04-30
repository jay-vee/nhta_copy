Imports Adjudication.DataAccess

Public Class ScoringDetail_Web
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents pnlGrid As System.Web.UI.WebControls.Panel
    Protected WithEvents lbtnAdd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblTotalNumberOfRecords As System.Web.UI.WebControls.Label
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblCurrentIndex As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageCount As System.Web.UI.WebControls.Label
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents LastProductionScore As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub




    Dim ShowScores(10) As Double, Show_TotalScore As Double = 0
    Dim NumOfAdjudicators As Int16 = 0
    Dim ProductionID As String = ""
    Dim myLabel As Label
    Dim LastProductionName As String = ""
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
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT Scoring.PK_ScoringID, PK_ProductionID,  ProductionType.PK_ProductionTypeID, ProductionType.ProductionType, " & _
                        "   	Production.Title, Company.CompanyName, ProductionCategory.ProductionCategory, " & _
                        "   	Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,   " & _
                        "   	Users.LastName + ', ' + Users.FirstName AS FullName, CompanyRepresented.CompanyName AS AdjudicatorCompanyRepresented, " & _
                        "   	Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual,  " & _
                        "   	(SELECT TOP 1 ApplicationName FROM ApplicationDefaults) AS ApplicationName,  " & _
                        "   	Director, DirectorScore, " & _
                        "   	MusicalDirector, MusicalDirectorScore, " & _
                        "   	Choreographer, ChoreographerScore, " & _
                        "   	ScenicDesigner, ScenicDesignerScore, " & _
                        "   	LightingDesigner, LightingDesignerScore, " & _
                        "   	SoundDesigner, SoundDesignerScore, " & _
                        "   	CostumeDesigner, CostumeDesignerScore, " & _
                        "   	OriginalPlaywright, OriginalPlaywrightScore, " & _
                        "   	CASE WHEN LEN(BestPerformer1Name) > 0 THEN BestPerformer1Name + ' in the Role of ""' + BestPerformer1Role + '""'  ELSE '' END as BestPerformer1 , BestPerformer1Score, " & _
                        "   	CASE WHEN LEN(BestPerformer2Name ) > 0 THEN BestPerformer2Name + ' in the Role of ""' + BestPerformer2Role + '""'  ELSE '' END  as BestPerformer2, BestPerformer2Score, " & _
                        "   	CASE WHEN LEN(BestSupportingActor1Name ) > 0 THEN BestSupportingActor1Name + ' in the Role of ""' + BestSupportingActor1Role + '""'  ELSE '' END  as BestSupportingActor1, BestSupportingActor1Score, " & _
                        "   	CASE WHEN LEN(BestSupportingActor2Name ) > 0 THEN BestSupportingActor2Name + ' in the Role of ""' + BestSupportingActor2Role + '""' ELSE '' END  as BestSupportingActor2, BestSupportingActor2Score, " & _
                        "   	CASE WHEN LEN(BestSupportingActress1Name ) > 0 THEN BestSupportingActress1Name + ' in the Role of ""' + BestSupportingActress1Role + '""'  ELSE '' END  as BestSupportingActress1, BestSupportingActress1Score, " & _
                        "   	CASE WHEN LEN(BestSupportingActress2Name ) > 0 THEN BestSupportingActress2Name + ' in the Role of ""' + BestSupportingActress2Role + '""'  ELSE '' END  as BestSupportingActress2, BestSupportingActress2Score," & _
                        "   	Scoring.DirectorComment, Scoring.MusicalDirectorComment, Scoring.ChoreographerComment, Scoring.ScenicDesignerComment,  " & _
                        "   	Scoring.LightingDesignerComment, Scoring.SoundDesignerComment, Scoring.CostumeDesignerComment, Scoring.OriginalPlaywrightComment,  " & _
                        "   	Scoring.BestPerformer1Comment, Scoring.BestPerformer2Comment, Scoring.BestSupportingActor1Comment, Scoring.BestSupportingActor2Comment,  " & _
                        "   	Scoring.BestSupportingActress1Comment, Scoring.BestSupportingActress2Comment, " & _
                        "   	DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore  " & _
                        "   		+ LightingDesignerScore + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore " & _
                        "   		+ BestPerformer1Score + BestPerformer2Score + BestSupportingActor1Score  " & _
                        "   		+ BestSupportingActor2Score + BestSupportingActress1Score + BestSupportingActress2Score  " & _
                        "   	AS TotalScore , TotNom.NumberOfNominations" & _
                        " FROM " & _
                        "   Production " & _
                        "   INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                        "   INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCateory.PK_ProductionCategoryID " & _
                        "   INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
                        "   INNER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID " & _
                        "   INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
                        "   INNER JOIN Company CompanyRepresented ON Scoring.FK_CompanyID_Adjudicator = CompanyRepresented.PK_CompanyID " & _
                        "   LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                        "   INNER JOIN " & _
                        "       (   SELECT " & _
                        "               PK_NominationsID, " & _
                        "               CASE WHEN Director IS NOT NULL AND LEN(Director) > 0 THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN MusicalDirector IS NOT NULL AND LEN(MusicalDirector) > 0 THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN Choreographer IS NOT NULL AND LEN(Choreographer) > 0  THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN ScenicDesigner IS NOT NULL AND LEN(ScenicDesigner) > 0  THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN LightingDesigner IS NOT NULL AND LEN(LightingDesigner) > 0  THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN SoundDesigner IS NOT NULL AND LEN(SoundDesigner) > 0  THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN CostumeDesigner IS NOT NULL AND LEN(CostumeDesigner) > 0  THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN OriginalPlaywright IS NOT NULL AND LEN(OriginalPlaywright) > 0  THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN (BestPerformer1Name IS NOT NULL AND LEN(BestPerformer1Name) > 0 ) AND (BestPerformer1Role IS NOT NULL AND LEN(BestPerformer1Role) > 0) THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN (BestPerformer2Name IS NOT NULL AND LEN(BestPerformer2Name) > 0 ) AND (BestPerformer2Role IS NOT NULL AND LEN(BestPerformer2Role) > 0) THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN (BestSupportingActor1Name IS NOT NULL AND LEN(BestSupportingActor1Name) > 0) AND (BestSupportingActor1Role IS NOT NULL AND LEN(BestSupportingActor1Role) > 0) THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN (BestSupportingActor2Name IS NOT NULL AND LEN(BestSupportingActor2Name) > 0) AND (BestSupportingActor2Role IS NOT NULL AND LEN(BestSupportingActor2Role) > 0) THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN (BestSupportingActress1Name IS NOT NULL AND LEN(BestSupportingActress1Name) > 0) AND (BestSupportingActress1Role IS NOT NULL AND LEN(BestSupportingActress1Role) > 0) THEN 1 ELSE 0 END + " & _
                        "				CASE WHEN (BestSupportingActress2Name IS NOT NULL AND LEN(BestSupportingActress2Name) > 0) AND (BestSupportingActress2Role IS NOT NULL AND LEN(BestSupportingActress2Role) > 0) THEN 1 ELSE 0 END " & _
                        "               AS NumberOfNominations " & _
                        "		    FROM " & _
                        "               Nominations " & _
                        "       ) TotNom ON TotNom.PK_NominationsID = Scoring.FK_NominationsID " & _
                        " ORDER BY " & _
                        "   ProductionCategory.ProductionCategory, ProductionType.ProductionType, Title,  " & _
                        "   (DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore  " & _
                        "    + LightingDesignerScore + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore " & _
                        "    + BestPerformer1Score + BestPerformer2Score + BestSupportingActor1Score  " & _
                        "    + BestSupportingActor2Score + BestSupportingActress1Score + BestSupportingActress2Score ) DESC "

        Try
            dt = DataAccess.Run_SQL_Query(sSQL)
            Me.gridMain.DataSource = dt
            Me.gridMain.DataBind()
            Me.lblTotalNumberOfRecords.Text = "Number of Scores: <B>" & dt.Rows.Count & "</B>"

        Catch ex As Exception
            lblTotalNumberOfRecords.Text = "ERROR: " & ex.Message
            lblTotalNumberOfRecords.ForeColor = System.Drawing.Color.Red
            End Try
    End Sub





    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        Dim iCount As Integer = 0

        Try

            If Not e.Item.ItemType.ToString = "Header" Then
                If Not e.Item.ItemType.ToString = "Footer" Then
                    If ProductionID = e.Item.Cells(0).Text Then
                        NumOfAdjudicators = NumOfAdjudicators + 1
                        ShowScores(NumOfAdjudicators) = (CInt(e.Item.Cells(1).Text) / CInt(e.Item.Cells(2).Text))

                    Else
                        If Not ProductionID = "" Then
                            Array.Sort(ShowScores)

                            If NumOfAdjudicators < 3 Then
                                ' Starting at 2, and stopping before the last value, drops the high and low values
                                For iCount = (ShowScores.LongLength - NumOfAdjudicators + 1) To ShowScores.LongLength - 2
                                    Show_TotalScore = Show_TotalScore + ShowScores(iCount)
                                    ShowScores(iCount) = 0                                      ' RESETS values for next use
                                Next
                                ShowScores(ShowScores.LongLength - NumOfAdjudicators + 1) = 0   ' RESETS values for next use
                                ShowScores(ShowScores.LongLength - 1) = 0                       ' RESETS values for next use
                                NumOfAdjudicators = NumOfAdjudicators - 2
                            Else
                                ' If 3 or less Adjudicators, do NOT drop the high and low values
                                For iCount = (ShowScores.LongLength - NumOfAdjudicators) To ShowScores.LongLength - 1
                                    Show_TotalScore = Show_TotalScore + ShowScores(iCount)
                                    ShowScores(iCount) = 0                                      ' RESETS values for next use
                                Next
                            End If

                            Show_TotalScore = Show_TotalScore / NumOfAdjudicators               ' Get AVERAGE of scores

                            myLabel = e.Item.FindControl("ProductionScore")
                            myLabel.Visible = True
                            myLabel.Text = "'" & LastProductionName & "' Average: <B>" & Format(Show_TotalScore, "##,##0.0") & "</B>"
                            Show_TotalScore = 0                                             ' RESETS values for next use

                        End If

                        ProductionID = e.Item.Cells(0).Text
                        NumOfAdjudicators = 1
                        ShowScores(NumOfAdjudicators) = (CInt(e.Item.Cells(1).Text) / CInt(e.Item.Cells(2).Text))
                        LastProductionName = e.Item.Cells(3).Text

                    End If
                Else
                    Array.Sort(ShowScores)

                    If NumOfAdjudicators < 3 Then
                        ' Starting at 2, and stopping before the last value, drops the high and low values
                        For iCount = (ShowScores.LongLength - NumOfAdjudicators + 1) To ShowScores.LongLength - 2
                            Show_TotalScore = Show_TotalScore + ShowScores(iCount)
                            ShowScores(iCount) = 0                                      ' RESETS values for next use
                        Next
                        ShowScores(ShowScores.LongLength - NumOfAdjudicators + 1) = 0   ' RESETS values for next use
                        ShowScores(ShowScores.LongLength - 1) = 0                       ' RESETS values for next use
                        NumOfAdjudicators = NumOfAdjudicators - 2
                    Else
                        ' If 3 or less Adjudicators, do NOT drop the high and low values
                        For iCount = (ShowScores.LongLength - NumOfAdjudicators) To ShowScores.LongLength - 1
                            Show_TotalScore = Show_TotalScore + ShowScores(iCount)
                            ShowScores(iCount) = 0                                      ' RESETS values for next use
                        Next
                    End If

                    Show_TotalScore = Show_TotalScore / NumOfAdjudicators               ' Get AVERAGE of scores

                    LastProductionScore.Visible = True
                    LastProductionScore.Text = "'" & LastProductionName & "' Average: <B>" & Format(Show_TotalScore, "##,##0.0") & "</B>"
                End If

            End If

        Catch ex As Exception
            lblTotalNumberOfRecords.Text = "ERROR Calculating Totals: " & ex.Message
            lblTotalNumberOfRecords.ForeColor = System.Drawing.Color.Red
        End Try

    End Sub




End Class
