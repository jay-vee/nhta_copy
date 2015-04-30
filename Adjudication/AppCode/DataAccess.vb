Imports System
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data
Imports Adjudication.CustomMail
Imports System.Collections.Generic

Public Class DataAccess

    Public Shared sSQLConnectString As String = ConfigurationManager.ConnectionStrings("NHTA").ToString

    Public Shared Function User_has_Access_to_Ballot(ByVal iPK_UserID As Integer, ByVal iScoringID As Integer) As Boolean
        '==================================================================================================
        Dim sSQL As String = "", dt As New DataTable
        '==================================================================================================
        Try
            User_has_Access_to_Ballot = False

            sSQL = "SELECT * FROM Scoring WHERE FK_UserID_Adjudicator=" & iPK_UserID.ToString & " AND PK_ScoringID=" & iScoringID.ToString
            dt = DataAccess.Run_SQL_Query(sSQL)
            Return dt.Rows.Count <> 0

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function Get_Ballot_as_HTML(ByVal iScoringID As Integer, ByVal iNominationID As Integer, Optional ByVal UseTempScoringComments As Boolean = False, Optional ByVal TempCalcScore As String = "") As String
        '==================================================================================================
        Dim sHTML As String = "", sSQL As String = "", FullCategoryName As String = ""
        Dim dtProd As DataTable, dtCategories As DataTable, dtNom As DataTable, dtScore As DataTable, dtComments As New DataTable
        Dim iProductionID As Integer = 0
        '==================================================================================================
        Try
            '===============================================================================================================================================================================
            '=== Get User and Scoring Information
            '===============================================================================================================================================================================
            sSQL = "SELECT " & _
                    "       Users.LastName + ', ' + Users.FirstName as FullName, " & _
                    "       Company.CompanyName, " & _
                    "       Users.EmailPrimary, Users.EmailSecondary, " & _
                    "       Scoring.*, " & _
                    "       Nominations.FK_ProductionID, " & _
                    "       Company_1.CompanyName AS CompanyAdjudication " & _
                    " FROM " & _
                    "       Users " & _
                    "       INNER JOIN Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator " & _
                    "       INNER JOIN Company ON Users.FK_CompanyID = Company.PK_CompanyID " & _
                    "       INNER JOIN Company Company_1 ON Scoring.FK_CompanyID_Adjudicator = Company_1.PK_CompanyID " & _
                    "       INNER JOIN Nominations on Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
                    " WHERE " & _
                    "       Scoring.PK_ScoringID = " & iScoringID.ToString

            dtScore = DataAccess.Run_SQL_Query(sSQL)

            If dtScore.Rows.Count = 0 Then
                Return "ERROR: No matching Production found for Production ID " & iProductionID.ToString
            End If

            '===============================================================================================================================================================================
            '=== Get adjudicated Production details
            '===============================================================================================================================================================================
            dtProd = Get_Production(dtScore.Rows(0)("FK_ProductionID").ToString)

            If dtProd.Rows.Count = 0 Then
                Return "ERROR: No matching Production found for Production ID " & iProductionID.ToString
            End If

            sHTML = "<font size=3><B>NH Theatre Awards Ballot</b></span><br />"
            sHTML = sHTML & "<br /><b>Production:</b> " & "<B>" & dtProd.Rows(0)("Title").ToString & "</b><br />" & dtProd.Rows(0)("ProductionType").ToString
            sHTML = sHTML & "<br /><b>Theatre Company:</b> " & dtProd.Rows(0)("CompanyName").ToString
            sHTML = sHTML & "<br /><b>Venue:</b> " & dtProd.Rows(0)("VenueName").ToString
            sHTML = sHTML & "<br /><b>Production Dates:</b> " & CDate(dtProd.Rows(0)("FirstPerformanceDateTime").ToString).ToShortDateString & " thru " & CDate(dtProd.Rows(0)("LastPerformanceDateTime").ToString).ToShortDateString
            sHTML = sHTML & "<hr noshade>"
            sHTML = sHTML & "<br /><b>Adjudicator:</b> " & dtScore.Rows(0)("FullName").ToString
            If dtScore.Rows(0)("EmailPrimary").ToString.Length > 3 Then sHTML = sHTML & "<br /><b>&nbsp;&nbsp;&nbsp;&nbsp;Primary Email:</b> " & dtScore.Rows(0)("EmailPrimary").ToString
            If dtScore.Rows(0)("EmailSecondary").ToString.Length > 3 Then sHTML = sHTML & "<br /><b>&nbsp;&nbsp;&nbsp;&nbsp;Secondary Email:</b> " & dtScore.Rows(0)("EmailSecondary").ToString
            sHTML = sHTML & "<br /><b>Represented Company:</b> " & dtScore.Rows(0)("CompanyAdjudication").ToString


            '===============================================================================================================================================================================
            '=== Get Score Comment info
            '===============================================================================================================================================================================
            If UseTempScoringComments = True Then
                dtComments = Run_SQL_Query("SELECT Score as ProductionDateAdjudicated_Planned, Comment as AdjudicatorAttendanceComment FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & iScoringID.ToString & " AND CommentFieldName='AdjudicatorAttendanceComment'")
            ElseIf UseTempScoringComments = False Then
                dtComments = dtScore
            End If

            sHTML = sHTML & "<br /><b>Date Adjudicated:</b> " & CDate(dtComments.Rows(0)("ProductionDateAdjudicated_Planned").ToString).ToShortDateString
            sHTML = sHTML & "<br /><b>Adjudicator Comment for Producing Theatre Company:</b> " & dtComments.Rows(0)("AdjudicatorAttendanceComment").ToString

            If UseTempScoringComments = True Then
                dtComments.Clear()
                dtComments = Run_SQL_Query("SELECT Score as FoundAdvertisement FROM Scoring_Temp_Entry WHERE FK_ScoringID=" & iScoringID.ToString & " AND CommentFieldName='FoundAdvertisement'")
            End If
            Select Case dtComments.Rows(0)("FoundAdvertisement").ToString
                Case "1"
                    sHTML = sHTML & "<br /><b>Found NHTA Advertisment:</b> Yes"
                Case "2"
                    sHTML = sHTML & "<br /><b>Found NHTA Advertisment:</b> No"
                Case "3"
                    sHTML = sHTML & "<br /><b>Found NHTA Advertisment:</b> Forgot to Look For it"
                Case Else
                    sHTML = sHTML & "<br /><b>Found NHTA Advertisment:</b> ERROR Processing Response"
            End Select

            sHTML = sHTML & "<br><hr noshade>"

            '===============================================================================================================================================================================
            '=== Get list of Active Categories for Display
            '===============================================================================================================================================================================
            sSQL = "SELECT PK_CategoryID, CategoryName, ScoringCriteria, ScoreFieldName, CommentFieldName, NominationFieldName, RoleFieldName, " & _
                    "       GenderFieldName, DisplayOrder, ActiveCategory, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate " & _
                    "   FROM Category " & _
                    "   WHERE (ActiveCategory = 1) " & _
                    "   ORDER BY DisplayOrder "
            dtCategories = DataAccess.Run_SQL_Query(sSQL)

            For Each dtRow As DataRow In dtCategories.Rows
                '===============================================================================================================================================================================
                '=== For Each Category that is active Get Nominee Name and Role/Gender (where needed)
                '===============================================================================================================================================================================
                sSQL = ""       'reset string
                If dtRow.Item("NominationFieldName").ToString.Length > 1 Then sSQL = sSQL & dtRow.Item("NominationFieldName").ToString & ", "
                If dtRow.Item("RoleFieldName").ToString.Length > 1 Then sSQL = sSQL & dtRow.Item("RoleFieldName").ToString & ", "
                If dtRow.Item("GenderFieldName").ToString.Length > 1 Then sSQL = sSQL & dtRow.Item("GenderFieldName").ToString & ", "

                sSQL = "SELECT " & sSQL & " PK_NominationsID  " & _
                        " FROM Nominations INNER JOIN Production ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                        "   WHERE PK_NominationsID = " & iNominationID.ToString
                dtNom = DataAccess.Run_SQL_Query(sSQL)

                '===============================================================================================================================================================================
                '=== Check if this Category was Nominated ===
                '===============================================================================================================================================================================
                If dtNom.Rows(0)(dtRow.Item("NominationFieldName").ToString).ToString.Length > 1 Then
                    '=== Display Category name with Correct Gender when needed === 
                    If dtRow.Item("GenderFieldName").ToString.Length > 1 Then
                        FullCategoryName = "<B>Best " & dtNom.Rows(0)(dtRow.Item("GenderFieldName").ToString).ToString & "</B>"
                    Else
                        FullCategoryName = "<B>" & dtRow.Item("CategoryName").ToString & "</B>"
                    End If

                    sHTML = sHTML & "<br />" & FullCategoryName & ":"

                    '=== Display acting Role information when needed === 
                    If dtRow.Item("RoleFieldName").ToString.Length > 1 Then
                        sHTML = sHTML & " " & dtNom.Rows(0)(dtRow.Item("NominationFieldName").ToString).ToString & " as '<i>" & dtNom.Rows(0)(dtRow.Item("RoleFieldName").ToString).ToString & "</i>'"
                    Else
                        sHTML = sHTML & " " & dtNom.Rows(0)(dtRow.Item("NominationFieldName").ToString).ToString
                    End If

                    '===============================================================================================================================================================================
                    '=== Get Scores and Comments: If Value for Score (not Zero) is found, display score and comment 
                    '===============================================================================================================================================================================
                    If UseTempScoringComments = True Then
                        sSQL = "SELECT * FROM Scoring_Temp_Entry " & _
                                " WHERE FK_ScoringID=" & iScoringID.ToString & _
                                " AND ScoreFieldName='" & dtRow.Item("ScoreFieldName").ToString & "'" ' dtRow.Item("CommentFieldName").ToString 
                        dtComments = DataAccess.Run_SQL_Query(sSQL)

                        '=== Use data from table Scoring_Temp_Entry if found, if not found check Scoring table for any entry.
                        If dtComments.Rows.Count > 0 Then
                            sHTML = sHTML & "<br /><b>Score: </b><font color=""blue"">" & dtComments.Rows(0)("Score").ToString & "</span> "
                            sHTML = sHTML & "<br /><b>Comment:</b> " & dtComments.Rows(0)("Comment").ToString
                        End If
                    Else
                        If Not dtScore.Rows(0)(dtRow.Item("ScoreFieldName").ToString).ToString = "0" Then
                            sHTML = sHTML & "<br /><b>Score: </b><font color=""blue"">" & dtScore.Rows(0)(dtRow.Item("ScoreFieldName").ToString).ToString & "</span> "
                            sHTML = sHTML & "<br /><b>Comment:</b> " & dtScore.Rows(0)(dtRow.Item("CommentFieldName").ToString).ToString
                        End If
                    End If

                    sHTML = sHTML & "<hr noshade>"

                End If
            Next
            If UseTempScoringComments = True Then
                sHTML = sHTML & "<br /><b>Calculated 'Best Production' Score: <font color=""green"">" & TempCalcScore & "</b></span>  (this calcuation is used for informational purposes only)"
            Else
                sHTML = sHTML & "<br /><b>Calculated 'Best Production' Score: <font color=""green"">" & Get_BestProduction_CalculatedScore(iScoringID).ToString("#0.0") & "</b></span>  (this calcuation is used for informational purposes only)"
            End If
            sHTML = sHTML & "<br /><hr noshade>"
            sHTML = sHTML & "<br />Last Updated by <B>" & dtComments.Rows(0)("LastUpdateByName").ToString & "</b> on " & CDate(dtComments.Rows(0)("LastUpdateByDate").ToString).ToLongDateString & " at " & CDate(dtScore.Rows(0)("LastUpdateByDate").ToString).ToShortTimeString
            sHTML = sHTML & "<br /><br />Created by <B>" & dtComments.Rows(0)("CreateByName").ToString & "</b> on " & CDate(dtComments.Rows(0)("CreateByDate").ToString).ToShortDateString & " at " & CDate(dtScore.Rows(0)("CreateByDate").ToString).ToShortTimeString

        Catch ex As Exception
            '            Return ex.Message
            Throw ex
        End Try

        Return sHTML

    End Function

    Public Shared Function Get_Upcoming_Productions(Optional ByVal DaysUntilOpening As Integer = 90) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT Production.PK_ProductionID, Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Company.CompanyName,  " & _
                "       Company.Website, Venue.VenueName, Venue.Address, Venue.City, Venue.State, AgeAppropriate.AgeAppropriateName, ProductionType.ProductionType,  " & _
                "       Production.TicketPurchaseDetails, Production.Comments, Production.AllPerformanceDatesTimes " & _
                " FROM Production INNER JOIN " & _
                "       Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                "       Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID INNER JOIN " & _
                "       AgeAppropriate ON Production.FK_AgeApproriateID = AgeAppropriate.PK_AgeAppropriateID INNER JOIN " & _
                "       ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
                " WHERE (Production.LastPerformanceDateTime <= GETDATE() + " & DaysUntilOpening.ToString & ")  " & _
                "       AND (Production.LastPerformanceDateTime >= GETDATE() - 1)  " & _
                " ORDER BY Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime "

        Return DataAccess.Run_SQL_Query(sSQL)

    End Function

    Public Shared Function Get_BestProduction_CalculatedScore(ByVal iScoringID As Integer) As Decimal
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        'This should be done dynamically using the 'Category' table where only ActiveCategory=1 are used to calculate
        sSQL = " SELECT Scoring.PK_ScoringID, Scoring.FK_NominationsID, TotalScore, NumberOfNominations, " & _
                 " 		CASE WHEN TotalScore > 0 THEN TotalScore / NumberOfNominations ELSE 0 END as AverageScoreForProduction " & _
                 "FROM Scoring  " & _
                 " INNER JOIN (SELECT SUM(DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score + BestSupportingActress1Score + BestSupportingActress2Score) + 0.00  as TotalScore, FK_NominationsID, PK_ScoringID " & _
                 " 		FROM Scoring  GROUP BY PK_ScoringID, FK_NominationsID) WithReserve_ScoreTotal ON WithReserve_ScoreTotal.PK_ScoringID = Scoring.PK_ScoringID " & _
                 " INNER JOIN (SELECT PK_NominationsID ,  " & _
                 " 		CASE WHEN Director IS NOT NULL AND LEN(Director) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN MusicalDirector IS NOT NULL AND LEN(MusicalDirector) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN Choreographer IS NOT NULL AND LEN(Choreographer) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN ScenicDesigner IS NOT NULL AND LEN(ScenicDesigner) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN LightingDesigner IS NOT NULL AND LEN(LightingDesigner) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN SoundDesigner IS NOT NULL AND LEN(SoundDesigner) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN CostumeDesigner IS NOT NULL AND LEN(CostumeDesigner) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN OriginalPlaywright IS NOT NULL AND LEN(OriginalPlaywright) > 0 THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN (BestPerformer1Name IS NOT NULL AND LEN(BestPerformer1Name) > 0 ) AND (BestPerformer1Role IS NOT NULL AND LEN(BestPerformer1Role) > 0) THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN (BestPerformer2Name IS NOT NULL AND LEN(BestPerformer2Name) > 0 ) AND (BestPerformer2Role IS NOT NULL AND LEN(BestPerformer2Role) > 0) THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN (BestSupportingActor1Name IS NOT NULL AND LEN(BestSupportingActor1Name) > 0) AND (BestSupportingActor1Role IS NOT NULL AND LEN(BestSupportingActor1Role) > 0) THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN (BestSupportingActor2Name IS NOT NULL AND LEN(BestSupportingActor2Name) > 0) AND (BestSupportingActor2Role IS NOT NULL AND LEN(BestSupportingActor2Role) > 0) THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN (BestSupportingActress1Name IS NOT NULL AND LEN(BestSupportingActress1Name) > 0) AND (BestSupportingActress1Role IS NOT NULL AND LEN(BestSupportingActress1Role) > 0) THEN 1 ELSE 0 END +  " & _
                 " 		CASE WHEN (BestSupportingActress2Name IS NOT NULL AND LEN(BestSupportingActress2Name) > 0) AND (BestSupportingActress2Role IS NOT NULL AND LEN(BestSupportingActress2Role) > 0) THEN 1 ELSE 0 END AS NumberOfNominations  " & _
                 " 	    FROM Nominations) TotalNominations ON TotalNominations.PK_NominationsID = Scoring.FK_NominationsID  " & _
                 " WHERE Scoring.PK_ScoringID = " & iScoringID.ToString

        dt = DataAccess.Run_SQL_Query(sSQL)

        '=== Use data from table Scoring_Temp_Entry if found, if not found check Scoring table for any entry.
        If dt.Rows.Count > 0 Then
            Return CDec(dt.Rows(0).Item("AverageScoreForProduction").ToString)
        Else
            Return 0
        End If

    End Function

    Public Shared Function Record_BrowserDetect(ByVal ht As Hashtable) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_BrowserDetect", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@FK_UserID", SqlDbType.Int).Value = CInt(ht(0))
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = ht(1)
                myAdapter.SelectCommand.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = ht(2)
                myAdapter.SelectCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = ht(3)
                myAdapter.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar, 50).Value = ht(4)
                myAdapter.SelectCommand.Parameters.Add("@MajorVersion", SqlDbType.VarChar, 50).Value = ht(5)
                myAdapter.SelectCommand.Parameters.Add("@MinorVersion", SqlDbType.VarChar, 50).Value = ht(6)
                myAdapter.SelectCommand.Parameters.Add("@Platform", SqlDbType.VarChar, 50).Value = ht(7)
                myAdapter.SelectCommand.Parameters.Add("@IsBeta", SqlDbType.VarChar, 50).Value = ht(8)
                myAdapter.SelectCommand.Parameters.Add("@IsCrawler", SqlDbType.VarChar, 50).Value = ht(9)
                myAdapter.SelectCommand.Parameters.Add("@IsAOL", SqlDbType.VarChar, 50).Value = ht(10)
                myAdapter.SelectCommand.Parameters.Add("@IsWin16", SqlDbType.VarChar, 50).Value = ht(11)
                myAdapter.SelectCommand.Parameters.Add("@IsWin32", SqlDbType.VarChar, 50).Value = ht(12)
                myAdapter.SelectCommand.Parameters.Add("@SupportsFrames", SqlDbType.VarChar, 50).Value = ht(13)
                myAdapter.SelectCommand.Parameters.Add("@SupportsTables", SqlDbType.VarChar, 50).Value = ht(14)
                myAdapter.SelectCommand.Parameters.Add("@SupportsVBScript", SqlDbType.VarChar, 50).Value = ht(15)
                myAdapter.SelectCommand.Parameters.Add("@SupportsJavaScript", SqlDbType.VarChar, 50).Value = ht(16)
                myAdapter.SelectCommand.Parameters.Add("@SupportsJavaApplets", SqlDbType.VarChar, 50).Value = ht(17)
                myAdapter.SelectCommand.Parameters.Add("@CDF", SqlDbType.VarChar, 50).Value = ht(18)

                Try
                    SQLConn.Open()
                    myAdapter.Fill(myDataTable)
                Catch ex As Exception
                    Throw ex
                End Try

                Return True

            End Using

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function Save_Scoring_Temp_Entry(ByVal htScore As Hashtable) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_Scoring_Temp_Entry", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@Scoring_Temp_EntryID", SqlDbType.Int).Value = htScore(1)
                myAdapter.SelectCommand.Parameters.Add("@ScoringID", SqlDbType.Int).Value = htScore(2)
                myAdapter.SelectCommand.Parameters.Add("@ScoreFieldName", SqlDbType.VarChar, 50).Value = htScore(3)
                myAdapter.SelectCommand.Parameters.Add("@CommentFieldName", SqlDbType.VarChar, 50).Value = htScore(4)
                If htScore(5) = Nothing Then
                    myAdapter.SelectCommand.Parameters.Add("@Score", SqlDbType.VarChar, 1000).Value = System.DBNull.Value
                Else
                    myAdapter.SelectCommand.Parameters.Add("@Score", SqlDbType.VarChar, 1000).Value = htScore(5)
                End If
                If htScore(6) = Nothing Then
                    myAdapter.SelectCommand.Parameters.Add("@Comment", SqlDbType.VarChar, 8000).Value = System.DBNull.Value
                Else
                    myAdapter.SelectCommand.Parameters.Add("@Comment", SqlDbType.VarChar, 8000).Value = htScore(6)
                End If


                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = htScore(99)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            ' ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_ScoringRange(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_ScoringRange", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_ScoringRangeID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@ScoringRangeMax", SqlDbType.SmallInt).Value = CInt(DataValues(2))
                myAdapter.SelectCommand.Parameters.Add("@ScoringRangeMin", SqlDbType.SmallInt).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(4)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Category(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_Category", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_CategoryID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@CategoryName", SqlDbType.VarChar, 1000).Value = DataValues(2)
                myAdapter.SelectCommand.Parameters.Add("@ActiveCategory", SqlDbType.TinyInt).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@ScoreFieldName", SqlDbType.VarChar, 50).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@CommentFieldName", SqlDbType.VarChar, 50).Value = DataValues(5)
                myAdapter.SelectCommand.Parameters.Add("@NominationFieldName", SqlDbType.VarChar, 50).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@RoleFieldName", SqlDbType.VarChar, 50).Value = DataValues(7)
                myAdapter.SelectCommand.Parameters.Add("@GenderFieldName", SqlDbType.VarChar, 50).Value = DataValues(8)
                myAdapter.SelectCommand.Parameters.Add("@DisplayOrder", SqlDbType.Int).Value = CInt(DataValues(9))
                myAdapter.SelectCommand.Parameters.Add("@ScoringCriteria", SqlDbType.VarChar, 4000).Value = DataValues(10)
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(11)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Category_ScoringRange(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_Category_ScoringRange", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@FK_CategoryID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@FK_ScoringRangeID", SqlDbType.Int).Value = CInt(DataValues(2))
                myAdapter.SelectCommand.Parameters.Add("@MatrixAdjectives", SqlDbType.VarChar, 1000).Value = DataValues(3)
                myAdapter.SelectCommand.Parameters.Add("@MatrixDetail", SqlDbType.VarChar, 4000).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(5)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Find_UserEmail(ByVal EmailAddress As String) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Find_UserEmail", SQLConn)
                myAdapter.SelectCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 200).Value = EmailAddress

                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function ReportScoring_OriginalPlaywright(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_OriginalPlaywright", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_Choreographer(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)

                myAdapter.SelectCommand = New SqlCommand("ReportScoring_Choreographer", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_MusicalDirector(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_MusicalDirector", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_Production(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)

                myAdapter.SelectCommand = New SqlCommand("ReportScoring_Production", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID
                If ProductionTypeID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionTypeID", SqlDbType.Int).Value = ProductionTypeID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_Director(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_Director", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID
                If ProductionTypeID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionTypeID", SqlDbType.Int).Value = ProductionTypeID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_LightingDesigner(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_LightingDesigner", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_SoundDesigner(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_SoundDesigner", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_CostumeDesigner(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_CostumeDesigner", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_ScenicDesigner(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_ScenicDesigner", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    'These values are used by the ReportScoring_BestActorActress sproc
    Public Enum ActorActress As Int16
        Actor = 1
        Actress = 2
    End Enum

    Public Shared Function ReportScoring_BestActress(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        Return ReportScoring_BestActorActress(ActorActress.Actress, WithAdjudicatorScores, ProductionCategoryID, ProductionTypeID)
    End Function

    Public Shared Function ReportScoring_BestActor(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        Return ReportScoring_BestActorActress(ActorActress.Actor, WithAdjudicatorScores, ProductionCategoryID, ProductionTypeID)
    End Function

    Public Shared Function ReportScoring_BestActorActress(ByVal ActorActress As Int16, Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_BestActorActress", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@ActorActress", SqlDbType.Int).Value = ActorActress
                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID
                If ProductionTypeID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionTypeID", SqlDbType.Int).Value = ProductionTypeID


                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_BestSupportingActor(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_BestSupportingActor", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If Not WithAdjudicatorScores = Nothing Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If Not ProductionCategoryID = Nothing Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID
                If Not ProductionTypeID = Nothing Then myAdapter.SelectCommand.Parameters.Add("@ProductionTypeID", SqlDbType.Int).Value = ProductionTypeID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ReportScoring_BestSupportingActress(Optional ByVal WithAdjudicatorScores As Int16 = 0, Optional ByVal ProductionCategoryID As Int16 = 0, Optional ByVal ProductionTypeID As Int16 = 0) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("ReportScoring_BestSupportingActress", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                If WithAdjudicatorScores <> 0 Then myAdapter.SelectCommand.Parameters.Add("@WithAdjudicatorScores", SqlDbType.Int).Value = WithAdjudicatorScores
                If ProductionCategoryID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionCategoryID", SqlDbType.Int).Value = ProductionCategoryID
                If ProductionTypeID <> 0 Then myAdapter.SelectCommand.Parameters.Add("@ProductionTypeID", SqlDbType.Int).Value = ProductionTypeID

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Scoring(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        Dim DateTester As Date
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)

                myAdapter.SelectCommand = New SqlCommand("Save_Scoring", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_ScoringID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@FK_NominationsID", SqlDbType.Int).Value = CInt(DataValues(2))
                myAdapter.SelectCommand.Parameters.Add("@FK_CompanyID_Adjudicator", SqlDbType.Int).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@FK_UserID_Adjudicator", SqlDbType.Int).Value = CInt(DataValues(4))
                myAdapter.SelectCommand.Parameters.Add("@AdjudicatorRequestsReassignment", SqlDbType.TinyInt).Value = CInt(DataValues(5))
                myAdapter.SelectCommand.Parameters.Add("@AdjudicatorRequestsReassignmentNote", SqlDbType.VarChar, 4000).Value = DataValues(41)
                myAdapter.SelectCommand.Parameters.Add("@AdjudicatorScoringLocked", SqlDbType.TinyInt).Value = CInt(DataValues(6))
                Try
                    DateTester = CDate(DataValues(7))
                    myAdapter.SelectCommand.Parameters.Add("@ProductionDateAdjudicated_Planned", SqlDbType.SmallDateTime).Value = CDate(DataValues(7))
                Catch ex As Exception
                    myAdapter.SelectCommand.Parameters.Add("@ProductionDateAdjudicated_Planned", SqlDbType.SmallDateTime).Value = System.DBNull.Value
                End Try

                Try
                    DateTester = CDate(DataValues(8))
                    myAdapter.SelectCommand.Parameters.Add("@ProductionDateAdjudicated_Actual", SqlDbType.SmallDateTime).Value = CDate(DataValues(8))
                Catch ex As Exception
                    myAdapter.SelectCommand.Parameters.Add("@ProductionDateAdjudicated_Actual", SqlDbType.SmallDateTime).Value = System.DBNull.Value
                End Try

                Try
                    DateTester = CDate(DataValues(40))
                    myAdapter.SelectCommand.Parameters.Add("@BallotSubmitDate", SqlDbType.SmallDateTime).Value = CDate(DataValues(40))
                Catch ex As Exception
                    myAdapter.SelectCommand.Parameters.Add("@BallotSubmitDate", SqlDbType.SmallDateTime).Value = System.DBNull.Value
                End Try

                myAdapter.SelectCommand.Parameters.Add("@DirectorScore", SqlDbType.TinyInt).Value = CInt(DataValues(9))
                myAdapter.SelectCommand.Parameters.Add("@DirectorComment", SqlDbType.VarChar, 8000).Value = DataValues(10)
                myAdapter.SelectCommand.Parameters.Add("@MusicalDirectorScore", SqlDbType.TinyInt).Value = CInt(DataValues(11))
                myAdapter.SelectCommand.Parameters.Add("@MusicalDirectorComment", SqlDbType.VarChar, 8000).Value = DataValues(12)
                myAdapter.SelectCommand.Parameters.Add("@ChoreographerScore", SqlDbType.TinyInt).Value = CInt(DataValues(13))
                myAdapter.SelectCommand.Parameters.Add("@ChoreographerComment", SqlDbType.VarChar, 8000).Value = DataValues(14)
                myAdapter.SelectCommand.Parameters.Add("@ScenicDesignerScore", SqlDbType.TinyInt).Value = CInt(DataValues(15))
                myAdapter.SelectCommand.Parameters.Add("@ScenicDesignerComment", SqlDbType.VarChar, 8000).Value = DataValues(16)
                myAdapter.SelectCommand.Parameters.Add("@LightingDesignerScore", SqlDbType.TinyInt).Value = CInt(DataValues(17))
                myAdapter.SelectCommand.Parameters.Add("@LightingDesignerComment", SqlDbType.VarChar, 8000).Value = DataValues(18)
                myAdapter.SelectCommand.Parameters.Add("@SoundDesignerScore", SqlDbType.TinyInt).Value = CInt(DataValues(19))
                myAdapter.SelectCommand.Parameters.Add("@SoundDesignerComment", SqlDbType.VarChar, 8000).Value = DataValues(20)
                myAdapter.SelectCommand.Parameters.Add("@CostumeDesignerScore", SqlDbType.TinyInt).Value = CInt(DataValues(21))
                myAdapter.SelectCommand.Parameters.Add("@CostumeDesignerComment", SqlDbType.VarChar, 8000).Value = DataValues(22)
                myAdapter.SelectCommand.Parameters.Add("@OriginalPlaywrightScore", SqlDbType.TinyInt).Value = CInt(DataValues(23))
                myAdapter.SelectCommand.Parameters.Add("@OriginalPlaywrightComment", SqlDbType.VarChar, 8000).Value = DataValues(24)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer1Score", SqlDbType.TinyInt).Value = CInt(DataValues(25))
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer1Comment", SqlDbType.VarChar, 8000).Value = DataValues(26)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer2Score", SqlDbType.TinyInt).Value = CInt(DataValues(27))
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer2Comment", SqlDbType.VarChar, 8000).Value = DataValues(28)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor1Score", SqlDbType.TinyInt).Value = CInt(DataValues(29))
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor1Comment", SqlDbType.VarChar, 8000).Value = DataValues(30)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor2Score", SqlDbType.TinyInt).Value = CInt(DataValues(31))
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor2Comment", SqlDbType.VarChar, 8000).Value = DataValues(32)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress1Score", SqlDbType.TinyInt).Value = CInt(DataValues(33))
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress1Comment", SqlDbType.VarChar, 8000).Value = DataValues(34)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress2Score", SqlDbType.TinyInt).Value = CInt(DataValues(35))
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress2Comment", SqlDbType.VarChar, 8000).Value = DataValues(36)
                myAdapter.SelectCommand.Parameters.Add("@BestProductionScore", SqlDbType.TinyInt).Value = CInt(DataValues(37))
                myAdapter.SelectCommand.Parameters.Add("@BestProductionComment", SqlDbType.VarChar, 8000).Value = DataValues(38)
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(39)
                myAdapter.SelectCommand.Parameters.Add("@ReserveAdjudicator", SqlDbType.TinyInt).Value = CInt(DataValues(42))
                myAdapter.SelectCommand.Parameters.Add("@FK_ScoringStatusID", SqlDbType.Int).Value = CInt(DataValues(43))
                myAdapter.SelectCommand.Parameters.Add("@AdjudicatorAttendanceComment", SqlDbType.VarChar, 4000).Value = DataValues(44)
                Try
                    myAdapter.SelectCommand.Parameters.Add("@NonScoringUpdate", SqlDbType.TinyInt).Value = CInt(DataValues(45))
                Catch ex As Exception
                    myAdapter.SelectCommand.Parameters.Add("@NonScoringUpdate", SqlDbType.TinyInt).Value = 0
                End Try
                myAdapter.SelectCommand.Parameters.Add("@FoundAdvertisement", SqlDbType.Int).Value = CInt(DataValues(46))

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Nominations(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_Nominations", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_NominationsID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@FK_ProductionID", SqlDbType.Int).Value = CInt(DataValues(2))
                myAdapter.SelectCommand.Parameters.Add("@Director", SqlDbType.VarChar, 100).Value = DataValues(3)
                myAdapter.SelectCommand.Parameters.Add("@MusicalDirector", SqlDbType.VarChar, 100).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@Choreographer", SqlDbType.VarChar, 100).Value = DataValues(5)
                myAdapter.SelectCommand.Parameters.Add("@ScenicDesigner", SqlDbType.VarChar, 100).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@LightingDesigner", SqlDbType.VarChar, 100).Value = DataValues(7)
                myAdapter.SelectCommand.Parameters.Add("@SoundDesigner", SqlDbType.VarChar, 100).Value = DataValues(8)
                myAdapter.SelectCommand.Parameters.Add("@CostumeDesigner", SqlDbType.VarChar, 100).Value = DataValues(9)
                myAdapter.SelectCommand.Parameters.Add("@OriginalPlaywright", SqlDbType.VarChar, 100).Value = DataValues(10)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer1Name", SqlDbType.VarChar, 100).Value = DataValues(11)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer1Role", SqlDbType.VarChar, 100).Value = DataValues(12)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer1Gender", SqlDbType.VarChar, 10).Value = DataValues(25)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer2Name", SqlDbType.VarChar, 100).Value = DataValues(13)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer2Role", SqlDbType.VarChar, 100).Value = DataValues(14)
                myAdapter.SelectCommand.Parameters.Add("@BestPerformer2Gender", SqlDbType.VarChar, 10).Value = DataValues(26)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor1Name", SqlDbType.VarChar, 100).Value = DataValues(15)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor1Role", SqlDbType.VarChar, 100).Value = DataValues(16)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor2Name", SqlDbType.VarChar, 100).Value = DataValues(17)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActor2Role", SqlDbType.VarChar, 100).Value = DataValues(18)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress1Name", SqlDbType.VarChar, 100).Value = DataValues(19)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress1Role", SqlDbType.VarChar, 100).Value = DataValues(20)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress2Name", SqlDbType.VarChar, 100).Value = DataValues(21)
                myAdapter.SelectCommand.Parameters.Add("@BestSupportingActress2Role", SqlDbType.VarChar, 100).Value = DataValues(22)
                myAdapter.SelectCommand.Parameters.Add("@Comments", SqlDbType.VarChar, 2000).Value = DataValues(23)
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(24)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Venue(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_Venue", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_VenueID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@VenueName", SqlDbType.VarChar, 100).Value = DataValues(2)
                myAdapter.SelectCommand.Parameters.Add("@Address", SqlDbType.VarChar, 100).Value = DataValues(3)
                myAdapter.SelectCommand.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = DataValues(5)
                myAdapter.SelectCommand.Parameters.Add("@ZIP", SqlDbType.VarChar, 50).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@Phone", SqlDbType.VarChar, 50).Value = DataValues(7)
                myAdapter.SelectCommand.Parameters.Add("@Website", SqlDbType.VarChar, 100).Value = DataValues(8)
                myAdapter.SelectCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = DataValues(9)
                myAdapter.SelectCommand.Parameters.Add("@Directions", SqlDbType.VarChar, 2000).Value = DataValues(10)
                myAdapter.SelectCommand.Parameters.Add("@Parking", SqlDbType.VarChar, 2000).Value = DataValues(11)
                myAdapter.SelectCommand.Parameters.Add("@HandicappedAccessible", SqlDbType.TinyInt).Value = CInt(DataValues(12))
                myAdapter.SelectCommand.Parameters.Add("@AirConditioned", SqlDbType.TinyInt).Value = CInt(DataValues(13))
                myAdapter.SelectCommand.Parameters.Add("@Outdoor", SqlDbType.TinyInt).Value = CInt(DataValues(14))
                myAdapter.SelectCommand.Parameters.Add("@SeatingCapacity", SqlDbType.VarChar, 200).Value = DataValues(15)
                myAdapter.SelectCommand.Parameters.Add("@Comments", SqlDbType.VarChar, 2000).Value = DataValues(16)
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(17)


                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Production(ByVal DataValues() As String) As Integer
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        'NOT NEEDED 1/11/2010: Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)

                myAdapter.SelectCommand = New SqlCommand("Save_Production", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_ProductionID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@FK_CompanyID", SqlDbType.Int).Value = CInt(DataValues(2))
                myAdapter.SelectCommand.Parameters.Add("@FK_VenueID", SqlDbType.Int).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@FK_AgeApproriateID", SqlDbType.Int).Value = CInt(DataValues(4))
                myAdapter.SelectCommand.Parameters.Add("@FK_ProductionTypeID", SqlDbType.Int).Value = CInt(DataValues(5))
                myAdapter.SelectCommand.Parameters.Add("@Title", SqlDbType.VarChar, 100).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@Authors", SqlDbType.VarChar, 200).Value = DataValues(7)
                myAdapter.SelectCommand.Parameters.Add("@LicensingCompany", SqlDbType.VarChar, 100).Value = DataValues(8)
                myAdapter.SelectCommand.Parameters.Add("@FirstPerformanceDateTime", SqlDbType.SmallDateTime).Value = CDate(DataValues(9))
                myAdapter.SelectCommand.Parameters.Add("@LastPerformanceDateTime", SqlDbType.SmallDateTime).Value = CDate(DataValues(10))
                myAdapter.SelectCommand.Parameters.Add("@AllPerformanceDatesTimes", SqlDbType.VarChar, 2000).Value = DataValues(11)
                myAdapter.SelectCommand.Parameters.Add("@TicketContactName", SqlDbType.VarChar, 50).Value = DataValues(12)
                myAdapter.SelectCommand.Parameters.Add("@TicketContactPhone", SqlDbType.VarChar, 50).Value = DataValues(13)
                myAdapter.SelectCommand.Parameters.Add("@TicketContactEmail", SqlDbType.VarChar, 100).Value = DataValues(14)
                myAdapter.SelectCommand.Parameters.Add("@TicketPurchaseDetails", SqlDbType.VarChar, 2000).Value = DataValues(15)
                myAdapter.SelectCommand.Parameters.Add("@Comments", SqlDbType.VarChar, 2000).Value = DataValues(16)
                myAdapter.SelectCommand.Parameters.Add("@RequiresAdjudication", SqlDbType.TinyInt).Value = CInt(DataValues(17))
                myAdapter.SelectCommand.Parameters.Add("@OriginalProduction", SqlDbType.TinyInt).Value = CInt(DataValues(18))
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(19)
                myAdapter.SelectCommand.Parameters.Add("@FK_ProductionCategoryID", SqlDbType.VarChar, 50).Value = DataValues(20)

                'Setup Output Parameter
                myAdapter.SelectCommand.Parameters.Add("@ProductionID", SqlDbType.Int).Direction = ParameterDirection.Output


                SQLConn.Open()
                myAdapter.SelectCommand.ExecuteNonQuery()
                Return myAdapter.SelectCommand.Parameters("@ProductionID").Value.ToString
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Save_Company(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_Company", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_CompanyID", SqlDbType.Int).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@NumOfProductions", SqlDbType.TinyInt).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar, 50).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = DataValues(5)
                myAdapter.SelectCommand.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = DataValues(7)
                myAdapter.SelectCommand.Parameters.Add("@ZIP", SqlDbType.VarChar, 50).Value = DataValues(8)
                myAdapter.SelectCommand.Parameters.Add("@Phone", SqlDbType.VarChar, 50).Value = DataValues(9)
                myAdapter.SelectCommand.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = DataValues(10)
                myAdapter.SelectCommand.Parameters.Add("@Website", SqlDbType.VarChar, 50).Value = DataValues(11)
                myAdapter.SelectCommand.Parameters.Add("@ActiveCompany", SqlDbType.TinyInt).Value = CInt(DataValues(12))
                myAdapter.SelectCommand.Parameters.Add("@Comments", SqlDbType.VarChar, 2000).Value = DataValues(13)
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(14)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return True
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Secure_UpdateUserOptions(ByVal DataValues() As String) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("sp_Secure_UpdateUserOptions", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@MinLoginIDLength", SqlDbType.SmallInt).Value = CInt(DataValues(1))
                myAdapter.SelectCommand.Parameters.Add("@MaxLoginIDLength", SqlDbType.SmallInt).Value = CInt(DataValues(2))
                myAdapter.SelectCommand.Parameters.Add("@NumOfLoginAttempts", SqlDbType.SmallInt).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@MinPasswordLength", SqlDbType.SmallInt).Value = CInt(DataValues(4))
                myAdapter.SelectCommand.Parameters.Add("@MaxPasswordLength", SqlDbType.SmallInt).Value = CInt(DataValues(5))
                myAdapter.SelectCommand.Parameters.Add("@AllowPasswordReuse", SqlDbType.SmallInt).Value = CInt(DataValues(6))
                myAdapter.SelectCommand.Parameters.Add("@RequirePasswords", SqlDbType.SmallInt).Value = CInt(DataValues(7))
                myAdapter.SelectCommand.Parameters.Add("@ExpirePasswords", SqlDbType.SmallInt).Value = CInt(DataValues(8))
                myAdapter.SelectCommand.Parameters.Add("@ExpirePasswordsAfterXDays", SqlDbType.SmallInt).Value = CInt(DataValues(9))
                myAdapter.SelectCommand.Parameters.Add("@DisableExpiredAccounts", SqlDbType.SmallInt).Value = CInt(DataValues(10))
                myAdapter.SelectCommand.Parameters.Add("@ExpireAccountsAfterXDays", SqlDbType.SmallInt).Value = CInt(DataValues(11))
                myAdapter.SelectCommand.Parameters.Add("@AdminUserLoginID", SqlDbType.VarChar, 50).Value = DataValues(12)


                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function


    Public Shared Function Secure_ResetAccount(ByVal UserLoginID As String, ByVal AdminUserLoginID As String) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("sp_Secure_ResetAccount", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@UserLoginID", SqlDbType.VarChar, 50).Value = UserLoginID
                myAdapter.SelectCommand.Parameters.Add("@AdminUserLoginID", SqlDbType.VarChar, 50).Value = AdminUserLoginID


                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Secure_PasswordChange(ByVal UserLoginID As String, ByVal UserPassword As String) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("sp_Secure_PasswordChange", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@UserLoginID", SqlDbType.VarChar, 50).Value = UserLoginID
                myAdapter.SelectCommand.Parameters.Add("@NewPassword", SqlDbType.VarChar, 50).Value = UserPassword


                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function Secure_UserAddEdit(ByVal DataValues() As String) As DataTable
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("sp_Secure_UserAddEdit", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@PK_UserID", SqlDbType.Int).Value = CInt(DataValues(22))

                myAdapter.SelectCommand.Parameters.Add("@UserLoginID", SqlDbType.VarChar, 50).Value = DataValues(1)
                myAdapter.SelectCommand.Parameters.Add("@UserPassword", SqlDbType.VarChar, 50).Value = DataValues(2)
                myAdapter.SelectCommand.Parameters.Add("@FK_AccessLevelID", SqlDbType.Int).Value = CInt(DataValues(3))
                myAdapter.SelectCommand.Parameters.Add("@FK_CompanyID", SqlDbType.Int).Value = CInt(DataValues(19))
                myAdapter.SelectCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = DataValues(5)
                myAdapter.SelectCommand.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = DataValues(7)
                myAdapter.SelectCommand.Parameters.Add("@State", SqlDbType.VarChar, 50).Value = DataValues(8)
                myAdapter.SelectCommand.Parameters.Add("@ZIP", SqlDbType.VarChar, 50).Value = DataValues(9)
                myAdapter.SelectCommand.Parameters.Add("@PhonePrimary", SqlDbType.VarChar, 50).Value = DataValues(10)
                myAdapter.SelectCommand.Parameters.Add("@PhoneSecondary", SqlDbType.VarChar, 50).Value = DataValues(11)
                myAdapter.SelectCommand.Parameters.Add("@EmailPrimary", SqlDbType.VarChar, 50).Value = DataValues(12)
                myAdapter.SelectCommand.Parameters.Add("@EmailSecondary", SqlDbType.VarChar, 100).Value = DataValues(13)
                If DataValues(14).Length > 5 Then
                    myAdapter.SelectCommand.Parameters.Add("@LastTrainingDate", SqlDbType.SmallDateTime).Value = CDate(DataValues(14))
                Else
                    myAdapter.SelectCommand.Parameters.Add("@LastTrainingDate", SqlDbType.SmallDateTime).Value = CDate("1/1/99")
                End If
                myAdapter.SelectCommand.Parameters.Add("@Website", SqlDbType.VarChar, 50).Value = DataValues(15)
                myAdapter.SelectCommand.Parameters.Add("@UserInformation", SqlDbType.VarChar, 1000).Value = DataValues(16)
                myAdapter.SelectCommand.Parameters.Add("@Active", SqlDbType.TinyInt).Value = CInt(DataValues(17))
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(18)
                myAdapter.SelectCommand.Parameters.Add("@SecurityQuestion", SqlDbType.VarChar, 250).Value = DataValues(20)
                myAdapter.SelectCommand.Parameters.Add("@SecurityAnswer", SqlDbType.VarChar, 100).Value = DataValues(21)
                myAdapter.SelectCommand.Parameters.Add("@FK_UserStatusID", SqlDbType.Int).Value = DataValues(23)

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function Secure_UserValidate(ByVal UserLoginID As String, ByVal UserPassword As String) As DataTable
        '==================================================================================================
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("sp_Secure_UserValidate", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@UserLoginID", SqlDbType.VarChar, 50).Value = UserLoginID
                myAdapter.SelectCommand.Parameters.Add("@UserPassword", SqlDbType.VarChar, 50).Value = UserPassword

                SQLConn.Open()
                myAdapter.Fill(myDataTable)
                Return myDataTable
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try
    End Function

    Public Shared Function ApplicationDefaults_Update(ByVal DataValues() As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_ApplicationDefaults", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@ApplicationName", SqlDbType.VarChar, 100).Value = DataValues(1)
                myAdapter.SelectCommand.Parameters.Add("@MainpageApplicationDesc", SqlDbType.VarChar, 2000).Value = DataValues(2)
                myAdapter.SelectCommand.Parameters.Add("@MainpageApplicationNotes", SqlDbType.VarChar, 2000).Value = DataValues(3)
                myAdapter.SelectCommand.Parameters.Add("@AdminContactName", SqlDbType.VarChar, 50).Value = DataValues(4)
                myAdapter.SelectCommand.Parameters.Add("@AdminContactPhoneNum", SqlDbType.VarChar, 50).Value = DataValues(5)
                myAdapter.SelectCommand.Parameters.Add("@AdminContactEmail", SqlDbType.VarChar, 200).Value = DataValues(6)
                myAdapter.SelectCommand.Parameters.Add("@NHTAwardsShowDate", SqlDbType.SmallDateTime).Value = CDate(DataValues(18))
                myAdapter.SelectCommand.Parameters.Add("@ValidTrainingStartDate", SqlDbType.SmallDateTime).Value = CDate(DataValues(21))
                myAdapter.SelectCommand.Parameters.Add("@DaysToSubmitProduction", SqlDbType.TinyInt).Value = CInt(DataValues(7))
                myAdapter.SelectCommand.Parameters.Add("@DaysToAllowNominationEdits", SqlDbType.TinyInt).Value = CInt(DataValues(8))
                myAdapter.SelectCommand.Parameters.Add("@DaysToConfirmAttendance", SqlDbType.TinyInt).Value = CInt(DataValues(9))
                myAdapter.SelectCommand.Parameters.Add("@DaysToWaitForScoring", SqlDbType.TinyInt).Value = CInt(DataValues(10))
                myAdapter.SelectCommand.Parameters.Add("@MaxShowsPerAdjudicator", SqlDbType.TinyInt).Value = CInt(DataValues(19))
                myAdapter.SelectCommand.Parameters.Add("@NumAdjudicatorsPerShow", SqlDbType.TinyInt).Value = CInt(DataValues(20))
                myAdapter.SelectCommand.Parameters.Add("@ScoringMinimum", SqlDbType.TinyInt).Value = CInt(DataValues(11))
                myAdapter.SelectCommand.Parameters.Add("@ScoringMaximum", SqlDbType.TinyInt).Value = CInt(DataValues(12))
                myAdapter.SelectCommand.Parameters.Add("@ManualURL_Rules", SqlDbType.VarChar, 200).Value = DataValues(14)
                myAdapter.SelectCommand.Parameters.Add("@ManualURL_Admin", SqlDbType.VarChar, 200).Value = DataValues(15)
                myAdapter.SelectCommand.Parameters.Add("@ManualURL_Liaison", SqlDbType.VarChar, 200).Value = DataValues(16)
                myAdapter.SelectCommand.Parameters.Add("@ManualURL_Adjudicator", SqlDbType.VarChar, 200).Value = DataValues(17)
                myAdapter.SelectCommand.Parameters.Add("@NHTAYearStartDate", SqlDbType.SmallDateTime).Value = CDate(DataValues(22))
                myAdapter.SelectCommand.Parameters.Add("@NHTAYearEndDate", SqlDbType.SmallDateTime).Value = CDate(DataValues(23))
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = DataValues(13)


                SQLConn.Open()
                myAdapter.SelectCommand.ExecuteNonQuery()
                Return True
            End Using

        Catch ex As Exception
            Throw
            Return False
        Finally
            ' SQLConn.Close()
        End Try

    End Function


    Public Shared Function Save_ErrorLog(ByVal UserLoginID As String, ByVal ErrorMessage As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_ErrorLog", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@UserLoginID", SqlDbType.VarChar, 50).Value = UserLoginID
                myAdapter.SelectCommand.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 2000).Value = ErrorMessage

                SQLConn.Open()
                myAdapter.SelectCommand.ExecuteNonQuery()
                Return True
            End Using

        Catch ex As Exception
            'Throw ex
            Return False
        Finally
            ' SQLConn.Close()
        End Try

    End Function


    Public Shared Function Save_EmailLog(ByVal UserLoginID As String, _
                                            ByVal EmailFrom As String, _
                                            ByVal EmailTo As String, _
                                            ByVal EmailSubject As String, _
                                            ByVal EmailBody As String, _
                                            Optional ByVal EmailTechnicalNotes As String = "", _
                                            Optional ByVal FK_EmailMessageTypesID As String = "1") As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_EmailLog", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@EmailFrom", SqlDbType.VarChar, 100).Value = EmailFrom
                myAdapter.SelectCommand.Parameters.Add("@EmailTo", SqlDbType.VarChar, 4000).Value = EmailTo
                myAdapter.SelectCommand.Parameters.Add("@EmailSubject", SqlDbType.VarChar, 1000).Value = EmailSubject
                myAdapter.SelectCommand.Parameters.Add("@EmailBody", SqlDbType.Text).Value = EmailBody
                myAdapter.SelectCommand.Parameters.Add("@EmailTechnicalNotes", SqlDbType.VarChar, 1000).Value = EmailTechnicalNotes
                myAdapter.SelectCommand.Parameters.Add("@LastUpdateByName", SqlDbType.VarChar, 50).Value = UserLoginID
                myAdapter.SelectCommand.Parameters.Add("@FK_EmailMessageTypesID", SqlDbType.Int).Value = CInt(FK_EmailMessageTypesID)

                SQLConn.Open()
                myAdapter.SelectCommand.ExecuteNonQuery()
            End Using
            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

    Public Shared Function Save_AdminMessage(ByVal UserLoginID As String, ByVal Subject As String, ByVal Message As String) As Boolean
        '==================================================================================================
        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                myAdapter.SelectCommand = New SqlCommand("Save_AdminMessage", SQLConn)
                myAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

                myAdapter.SelectCommand.Parameters.Add("@UserLoginID", SqlDbType.VarChar, 50).Value = UserLoginID
                myAdapter.SelectCommand.Parameters.Add("@Subject", SqlDbType.VarChar, 50).Value = Subject
                myAdapter.SelectCommand.Parameters.Add("@Message", SqlDbType.VarChar, 2000).Value = Message

                SQLConn.Open()
                myAdapter.SelectCommand.ExecuteNonQuery()
            End Using
            Return True

        Catch ex As Exception
            Throw ex
            Return False

        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function Get_ProductionAdjudicators(Optional ByVal sProductionID As String = "", Optional ByVal FirstRowEmpty As Boolean = False, Optional ByVal SortColumnName As String = "Users.LastName, Users.FirstName", Optional ByVal SortOrder As String = "") As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable, sSQL As String
        '====================================================================================================
        ' Get Adjudicators for Production 
        Try
            sSQL = "SELECT     Scoring.PK_ScoringID, Users.PK_UserID, Users.FirstName + ' ' + Users.LastName AS FullName, Company.CompanyName, Company.PK_CompanyID,  " & _
                    "              Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual, Scoring.AdjudicatorRequestsReassignment,  " & _
                    "              CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END AS AdjudicatorToSeeProduction,  " & _
                    "              Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore  " & _
                    "                   + Scoring.SoundDesignerScore + Scoring.CostumeDesignerScore + Scoring.OriginalPlaywrightScore " & _
                    "                   + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score " & _
                    "                   + Scoring.BestSupportingActor1Score + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score " & _
                    "                   + Scoring.BestSupportingActress2Score " & _
                    "               AS TotalScore, Scoring.ReserveAdjudicator, Scoring.FK_NominationsID, Scoring.LastUpdateByName, Scoring.LastUpdateByDate,  " & _
                    "              Scoring.CreateByName, Scoring.CreateByDate, Production.PK_ProductionID, Production.Title, Production.FirstPerformanceDateTime,  " & _
                    "              Production.LastPerformanceDateTime, Production.AllPerformanceDatesTimes, Production.TicketContactName, Production.TicketContactPhone,  " & _
                    "              Production.TicketContactEmail, Production.TicketPurchaseDetails, " & _
                    "              Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus, " & _
                    "              Production.OriginalProduction " & _
                    "FROM         Scoring INNER JOIN " & _
                    "              Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID INNER JOIN " & _
                    "              Company ON Scoring.FK_CompanyID_Adjudicator = Company.PK_CompanyID INNER JOIN " & _
                    "              Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID INNER JOIN " & _
                    "              Production ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                    "	            INNER JOIN ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID "

            If sProductionID.Length > 0 Then sSQL = sSQL & " WHERE Production.PK_ProductionID = " & sProductionID

            If SortColumnName.Length > 0 Then
                sSQL = sSQL & " ORDER BY " & SortColumnName & " " & SortOrder
            Else
                sSQL = sSQL & " ORDER BY Users.LastName, Users.FirstName "
            End If

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function


    Public Shared Function Set_ProductionAdjudicatorList(Optional ByVal PK_NominationID As String = "", Optional ByVal FirstRowEmpty As Boolean = False, Optional ByVal SortColumnName As String = "Users.LastName, Users.FirstName", Optional ByVal SortOrder As String = "") As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable, sSQL As String
        '====================================================================================================
        ' Get Adjudicators for Production 
        Try
            sSQL = "SELECT PK_ScoringID, Users.PK_UserID, Users.FirstName + ' ' + Users.LastName as FullName, " & _
                "   Company.CompanyName, Company.PK_CompanyID, " & _
                "   ProductionDateAdjudicated_Planned, ProductionDateAdjudicated_Actual, AdjudicatorRequestsReassignment, " & _
                "	CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END as AdjudicatorToSeeProduction, " & _
                "   DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore " & _
                "	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score " & _
                "	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score " & _
                "	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore,  " & _
                "   Scoring.ReserveAdjudicator, " & _
                "   Scoring.FK_NominationsID, Scoring.LastUpdateByName, Scoring.LastUpdateByDate, Scoring.CreateByName, Scoring.CreateByDate, " & _
                "   Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus " & _
                " FROM Scoring INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                "       INNER JOIN Company ON Scoring.FK_CompanyID_Adjudicator = Company.PK_CompanyID " & _
                "	    INNER JOIN ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID "

            If PK_NominationID.Length > 0 Then sSQL = sSQL & " WHERE Scoring.FK_NominationsID = " & PK_NominationID

            If SortColumnName.Length > 0 Then
                sSQL = sSQL & " ORDER BY " & SortColumnName & " " & SortOrder
            Else
                sSQL = sSQL & " ORDER BY Users.LastName, Users.FirstName "
            End If

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function

    Public Shared Function Get_Users(Optional ByVal PK_UserID As String = "", Optional ByVal FirstRowEmpty As Boolean = False, Optional ByVal SortColumnName As String = "Users.LastName, Users.FirstName", Optional ByVal SortOrder As String = "") As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable, sSQL As String
        '====================================================================================================
        ' Get Production Information
        Try
            sSQL = " SELECT Users.*, Users.LastName + ', ' + Users.FirstName as FullName, " & _
                    "       Users.FirstName + ' ' + Users.LastName AS UserFullName, " & _
                    "       Users.Address + ', ' + Users.City + ' ' + Users.State + ' ' + Users.ZIP AS FullAddress, " & _
                    "       Company.CompanyName, UserAccessLevels.AccessLevelName " & _
                    " FROM Users " & _
                    "       INNER JOIN Company ON Users.FK_CompanyID = Company.PK_CompanyID " & _
                    "		INNER JOIN UserAccessLevels ON Users.FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID "

            If PK_UserID.Length > 0 Then sSQL = sSQL & " WHERE PK_UserID = " & PK_UserID

            sSQL = sSQL & " ORDER BY " & SortColumnName & " " & SortOrder

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function

    Public Shared Function Get_BrowserDetect(ByVal sUserID As String) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT * FROM BrowserDetect WHERE FK_UserID=" & sUserID

        Return DataAccess.Run_SQL_Query(sSQL)

    End Function


    Public Shared Function Get_ScoringRange(ByVal sPK_ScoringRangeID As String) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_ScoringRangeID, ScoringRangeMax, ScoringRangeMin, CAST(ScoringRange.ScoringRangeMax as VARCHAR(3)) + '-' +  CAST(ScoringRange.ScoringRangeMin as VARCHAR(3)) as ScoringRange, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate  FROM ScoringRange "

        sSQL = sSQL + " WHERE PK_ScoringRangeID=" + sPK_ScoringRangeID

        Return DataAccess.Run_SQL_Query(sSQL)

    End Function

    Public Shared Function Get_ScoringRanges(Optional ByVal SortColumnName As String = "ScoringRangeMax", _
                                        Optional ByVal SortOrder As String = "", _
                                        Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_ScoringRangeID, ScoringRangeMax, ScoringRangeMin, CAST(ScoringRange.ScoringRangeMax as VARCHAR(3)) + '-' +  CAST(ScoringRange.ScoringRangeMin as VARCHAR(3)) as ScoringRange, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate FROM ScoringRange "

        sSQL = sSQL + " ORDER BY " + SortColumnName + " " + SortOrder

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function


    Public Shared Function Get_EmailFromAddresses(Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_EmailFromID ,EmailFromAddress ,Active ,LastUpdateByName ,LastUpdateByDate ,CreateByName ,CreateByDate FROM EmailFrom WHERE Active=1 "
        sSQL = sSQL & " ORDER BY EmailFromAddress "

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Get_Category(ByVal sPK_CategoryID As String) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_CategoryID, CategoryName, ScoringCriteria, ActiveCategory, ScoreFieldName, CommentFieldName, NominationFieldName, RoleFieldName, GenderFieldName, DisplayOrder, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate FROM Category "
        sSQL = sSQL + " WHERE PK_CategoryID=" + sPK_CategoryID

        Return DataAccess.Run_SQL_Query(sSQL)

    End Function


    Public Shared Function Get_Categories(Optional ByVal SortColumnName As String = "DisplayOrder", _
                                            Optional ByVal SortOrder As String = "", _
                                            Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_CategoryID, CategoryName, ScoringCriteria, ActiveCategory, ScoreFieldName, CommentFieldName, NominationFieldName, RoleFieldName, GenderFieldName, DisplayOrder, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate FROM Category "

        sSQL = sSQL + " ORDER BY " + SortColumnName + " " + SortOrder

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function


    Public Shared Function Get_Category_ScoringRange(ByVal sFK_CategoryID As String, ByVal sFK_ScoringRangeID As String, _
                                        Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT 	Category_ScoringRange.FK_CategoryID, Category_ScoringRange.FK_ScoringRangeID, " & _
            "       Category.CategoryName, ScoringRange.ScoringRangeMax, ScoringRange.ScoringRangeMin,  " & _
            "       CAST(ScoringRange.ScoringRangeMax as VARCHAR(3)) + '-' +  CAST(ScoringRange.ScoringRangeMin as VARCHAR(3)) as ScoringRange, " & _
            "       Category_ScoringRange.MatrixAdjectives, Category_ScoringRange.MatrixDetail, " & _
            "       Category_ScoringRange.LastUpdateByName, Category_ScoringRange.LastUpdateByDate, Category_ScoringRange.CreateByName, Category_ScoringRange.CreateByDate  " & _
            "   FROM Category " & _
            "       INNER JOIN Category_ScoringRange ON Category.PK_CategoryID = Category_ScoringRange.FK_CategoryID  " & _
            "       INNER JOIN ScoringRange ON Category_ScoringRange.FK_ScoringRangeID = ScoringRange.PK_ScoringRangeID " & _
            "   WHERE Category_ScoringRange.FK_CategoryID = " & sFK_CategoryID & _
            "       AND Category_ScoringRange.FK_ScoringRangeID = " & sFK_ScoringRangeID

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Get_Category_ScoringRanges(Optional ByVal SortColumnName As String = "DisplayOrder", _
                                        Optional ByVal SortOrder As String = "", _
                                        Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT 	Category_ScoringRange.FK_CategoryID, Category_ScoringRange.FK_ScoringRangeID, " & _
            "       Category.CategoryName, ScoringRange.ScoringRangeMax, ScoringRange.ScoringRangeMin,  " & _
            "       CAST(ScoringRange.ScoringRangeMax as VARCHAR(3)) + '-' +  CAST(ScoringRange.ScoringRangeMin as VARCHAR(3)) as ScoringRange, " & _
            "       Category_ScoringRange.MatrixAdjectives, Category_ScoringRange.LastUpdateByName, Category_ScoringRange.LastUpdateByDate, Category_ScoringRange.CreateByName, Category_ScoringRange.CreateByDate  " & _
            "   FROM Category " & _
            "       INNER JOIN Category_ScoringRange ON Category.PK_CategoryID = Category_ScoringRange.FK_CategoryID  " & _
            "       INNER JOIN ScoringRange ON Category_ScoringRange.FK_ScoringRangeID = ScoringRange.PK_ScoringRangeID"

        sSQL = sSQL + " ORDER BY " + SortColumnName + " " + SortOrder

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function
    '
    Public Shared Function Get_Productions(Optional ByVal SortColumnName As String = "", _
                                            Optional ByVal SortOrder As String = "", _
                                            Optional ByVal OnlyFutureProductions As Boolean = False, _
                                            Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT Production.PK_ProductionID, Production.Title, Company.CompanyName, ProductionType.ProductionType " & _
                "		, Venue.VenueName, Venue.City, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.RequiresAdjudication " & _
                "       , CASE COUNT(Nominations.PK_NominationsID) WHEN 1 THEN 'Yes' ELSE 'No' END AS SetNominations " & _
                "       , Production.Title + '  [' + RTRIM(Company.CompanyName) + ']' as ProductionInfo " & _
                "       , Nominations.PK_NominationsID, Production.FK_CompanyID " & _
                "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                "       , OriginalProduction " & _
                " FROM  Production " & _
                "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                "		INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                "		INNER JOIN Venue ON Production.FK_VenueID = Venue.PK_VenueID  " & _
                "		INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID  " & _
                "       LEFT OUTER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID "

        If OnlyFutureProductions = True Then
            sSQL = sSQL & " WHERE Production.LastPerformanceDateTime >= (GetDate() -1) "
        End If

        sSQL = sSQL & " GROUP BY Production.PK_ProductionID, Production.Title, Company.CompanyName, ProductionType.ProductionType, " & _
                      "		Venue.VenueName, Venue.City, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.RequiresAdjudication  " & _
                      "     , Nominations.PK_NominationsID, Production.FK_CompanyID, ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory, OriginalProduction "

        If Not (SortColumnName = "") Then
            If (SortColumnName = "CompanyName") Then
                sSQL = sSQL + " ORDER BY " + SortColumnName + " " + SortOrder
            Else
                sSQL = sSQL + " ORDER BY " + SortColumnName + " " + SortOrder + ", Company.CompanyName "
            End If
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY FirstPerformanceDateTime "
        End If

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Get_AdjudicatorAssignments(Optional ByVal SortColumnName As String = "", _
                                                        Optional ByVal SortOrder As String = "", _
                                                        Optional ByVal OnlyFutureProductions As Boolean = False, _
                                                        Optional ByVal FirstRowEmpty As Boolean = False, _
                                                        Optional ByVal sPK_UserID As String = "") As DataTable
        '====================================================================================================
        Dim sSQL As String, sSQLWhere As String = ""
        '====================================================================================================
        sSQL = " SELECT Scoring.PK_ScoringID, Scoring.FK_CompanyID_Adjudicator, Nominations.PK_NominationsID, Production.PK_ProductionID, " & _
                "	    Company.PK_CompanyID, Users.PK_UserID, " & _
                "	    Users.PK_UserID, Users.UserLoginID, Users.LastName, Users.FirstName, Users.LastName + ', ' + Users.FirstName as FullName, " & _
                "	    Production.Title, ProductionType.ProductionType, " & _
                "	    Company.CompanyName, " & _
                "	    Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
                "       Scoring.AdjudicatorRequestsReassignment, Scoring.ReserveAdjudicator, ProductionDateAdjudicated_Planned, ProductionDateAdjudicated_Actual, " & _
                "	    CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END as AdjudicatorToSeeProduction, " & _
                "       DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore " & _
                "	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score " & _
                "	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score " & _
                "	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore,  " & _
                "	    Scoring.LastUpdateByName, Scoring.LastUpdateByDate, " & _
                "       Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus " & _
                " FROM  Users INNER JOIN" & _
                "	    Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN " & _
                "	    Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID INNER JOIN " & _
                "	    Production INNER JOIN " & _
                "	    Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                "	    ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID ON " & _
                "	    Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                "	    INNER JOIN ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID "

        If OnlyFutureProductions = True Then sSQLWhere = sSQLWhere & " AND Production.LastPerformanceDateTime >= (GetDate() -1) "
        If sPK_UserID.Length > 0 Then sSQLWhere = sSQLWhere & " AND Users.PK_UserID = " & sPK_UserID
        If sSQLWhere.Length > 1 Then sSQLWhere = " WHERE (1=1) " & sSQLWhere

        sSQL = sSQL & sSQLWhere

        If Not (SortColumnName = "") And Not (SortColumnName = "CompanyName") Then
            sSQL = sSQL + " ORDER BY " + SortColumnName + " " + SortOrder + ", Company.CompanyName "
        Else    '=== Create a Default Sort Order ===
            sSQL = sSQL + " ORDER BY Users.LastName, Users.FirstName, Production.Title, Company.CompanyName "
        End If

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Get_EmailMessageTypes(Optional ByVal sMassMailing As String = "", Optional ByVal sAutoGenerated As String = "", Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable
        Dim sSQL As String, SQLWhere As String = ""
        '====================================================================================================
        ' Get EMail Message Information w/Last Sent data
        Try
            sSQL = " SELECT EmailMessageTypes.PK_EmailMessageTypesID, EmailMessageTypes.EmailMessageType, EmailMessageTypes.AutoGenerated, EmailMessageTypes.MassMailing, " & _
                        " 		EMLog.PK_EmailLogID, EMLog.LastUpdateByDate, EMLog.LastUpdateByName " & _
                        " FROM  EmailMessageTypes " & _
                        "    LEFT OUTER JOIN (  SELECT LogLastUpdateDate.PK_EmailLogID, LogLastUpdateDate.FK_EmailMessageTypesID, LogLastUpdateDate.LastUpdateByDate, EmailLog.LastUpdateByName" & _
                        " 							FROM EmailLog " & _
                        " 								INNER JOIN ( SELECT MAX(PK_EmailLogID) AS PK_EmailLogID, FK_EmailMessageTypesID, MAX(LastUpdateByDate) as LastUpdateByDate " & _
                        " 												FROM EmailLog GROUP BY FK_EmailMessageTypesID" & _
                        " 											) LogLastUpdateDate ON EmailLog.PK_EmailLogID = LogLastUpdateDate.PK_EmailLogID " & _
                        " 	 				) EMLog ON EMLog.FK_EmailMessageTypesID = EmailMessageTypes.PK_EmailMessageTypesID " & _
                        " GROUP BY EmailMessageTypes.EmailMessageType, EmailMessageTypes.AutoGenerated, EmailMessageTypes.MassMailing," & _
                        " 			EmailMessageTypes.PK_EmailMessageTypesID, EMLog.PK_EmailLogID, EMLog.LastUpdateByDate,EMLog.LastUpdateByName "

            If sMassMailing.Length > 0 Then
                SQLWhere = SQLWhere & " AND (EmailMessageTypes.MassMailing = " & sMassMailing & ") "
            End If
            If sAutoGenerated.Length > 0 Then
                SQLWhere = SQLWhere & " AND (EmailMessageTypes.AutoGenerated = " & sAutoGenerated & ") "
            End If
            If SQLWhere.Length > 0 Then SQLWhere = " HAVING (1=1) " & SQLWhere

            sSQL = sSQL & SQLWhere & " ORDER BY EmailMessageTypes.EmailMessageType "

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function

    Public Shared Function Get_Production(ByVal sProductionID As String, Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        ' Get Production Information
        Try
            sSQL = " SELECT Production.PK_ProductionID, Production.FK_CompanyID, Production.FK_VenueID, Production.FK_AgeApproriateID, Production.FK_ProductionTypeID,  " & _
                        "	    Company.CompanyName, Company.EmailAddress as CompanyEmailAddress, Company.Website as CompanyWebsite, Company.Phone as CompanyPhone, " & _
                        "       AgeAppropriate.AgeAppropriateName, ProductionType.ProductionType, " & _
                        "       Venue.VenueName, Venue.Address, Venue.State, Venue.City, Venue.Zip, Venue.Website, " & _
                        "	    Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.AllPerformanceDatesTimes,  " & _
                        "	    Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, Production.TicketPurchaseDetails, " & _
                        "       Production.Comments, " & _
                        "       OriginalProduction " & _
                        "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                        "  FROM Production " & _
                        "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                        "	    INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                        "	    INNER JOIN Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID " & _
                        "	    INNER JOIN AgeAppropriate ON Production.FK_AgeApproriateID = AgeAppropriate.PK_AgeAppropriateID " & _
                        "	    INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
                        " WHERE PK_ProductionID = " & sProductionID

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function


    Public Shared Function Get_Ballot(ByVal sScoringID As String, Optional ByVal sUserID_Adjudicator As String = "") As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        ' Get Production Nomination Information
        Try
            sSQL = "SELECT Scoring.* , Nominations.* FROM Scoring INNER JOIN  Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID " & _
                    " WHERE Scoring.PK_ScoringID=" & sScoringID

            ' If not an admin, double check that user is assigned this adjudication
            If sUserID_Adjudicator.Length > 0 Then sSQL = sSQL & " AND FK_UserID_Adjudicator = " & sUserID_Adjudicator

            dt = Run_SQL_Query(sSQL)

            'sSQL = " SELECT PK_ScoringID, PK_NominationsID, FK_CompanyID_Adjudicator, FK_UserID_Adjudicator, " & _
            ' "	    	AdjudicatorRequestsReassignment, AdjudicatorScoringLocked, ProductionDateAdjudicated_Planned, " & _
            ' "	    	ProductionDateAdjudicated_Actual, DirectorScore, DirectorComment, MusicalDirectorScore, " & _
            ' "	    	MusicalDirectorComment, ChoreographerScore, ChoreographerComment, ScenicDesignerScore, " & _
            ' "	    	ScenicDesignerComment, LightingDesignerScore, LightingDesignerComment, SoundDesignerScore, " & _
            ' " 	    	SoundDesignerComment, CostumeDesignerScore, CostumeDesignerComment, OriginalPlaywrightScore, " & _
            ' "	    	OriginalPlaywrightComment, BestPerformer1Score, BestPerformer1Comment, BestPerformer2Score, " & _
            ' "	    	BestPerformer2Comment, BestSupportingActor1Score, BestSupportingActor1Comment, " & _
            ' "	    	BestSupportingActor2Score, BestSupportingActor2Comment, BestSupportingActress1Score, " & _
            ' "	    	BestSupportingActress1Comment, BestSupportingActress2Score, BestSupportingActress2Comment, " & _
            ' "	    	Scoring.LastUpdateByName, Scoring.LastUpdateByDate, BallotSubmitDate, " & _
            ' " 	    	FK_ProductionID, Director, MusicalDirector, " & _
            ' "	    	Choreographer, ScenicDesigner, LightingDesigner, SoundDesigner, " & _
            ' "	    	CostumeDesigner, OriginalPlaywright, BestPerformer1Name, BestPerformer1Role, BestPerformer1Gender, " & _
            ' "	    	BestPerformer2Name, BestPerformer2Role, BestPerformer2Gender, BestSupportingActor1Name, BestSupportingActor1Role, " & _
            ' "	    	BestSupportingActor2Name, BestSupportingActor2Role, BestSupportingActress1Name, " & _
            ' "	    	BestSupportingActress1Role, BestSupportingActress2Name, BestSupportingActress2Role, " & _
            ' "	    	Scoring.CreateByDate " & _
            ' " FROM Scoring INNER JOIN Nominations ON Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
            ' " WHERE PK_ScoringID=" & ViewState("PK_ScoringID")

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function


    Public Shared Function Get_Score(ByVal sProductionID As String, ByVal sPK_UserID As String, Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        ' Get Production Nomination Information
        Try
            sSQL = "SELECT Scoring.* FROM Scoring INNER JOIN  Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID " & _
                    " WHERE Nominations.FK_ProductionID=" & sProductionID & _
                    "       AND Scoring.FK_UserID_Adjudicator = " & sPK_UserID

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function

    Public Shared Function Get_Nomination(ByVal sProductionID As String, Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        ' Get Production Nomination Information
        Try

            sSQL = "SELECT  Production.Title, Production.RequiresAdjudication, ProductionType.ProductionType, Production.OriginalProduction,  " & _
                    "	    Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.AllPerformanceDatesTimes,  " & _
                    "	    Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, Production.TicketPurchaseDetails, " & _
                    "		Company.CompanyName, Company.Website, Company.EmailAddress as CompanyEmailAddress, Company.Website as CompanyWebsite,  " & _
                    "       Venue.VenueName, Venue.Address, Venue.State, Venue.City, Venue.Zip, Venue.Website, " & _
                    "		PK_NominationsID, PK_CompanyID, PK_ProductionTypeID, PK_ProductionID,  " & _
                    "		Director, MusicalDirector, Choreographer, ScenicDesigner,  " & _
                    "		LightingDesigner, SoundDesigner, CostumeDesigner, OriginalPlaywright, " & _
                    "		BestPerformer1Name, BestPerformer1Role, BestPerformer1Gender, " & _
                    "       BestPerformer2Name, BestPerformer2Role, BestPerformer2Gender, " & _
                    "       BestSupportingActor1Name, BestSupportingActor1Role, " & _
                    "		BestSupportingActor2Name, BestSupportingActor2Role, " & _
                    "		BestSupportingActress1Name, BestSupportingActress1Role, " & _
                    "       BestSupportingActress2Name, BestSupportingActress2Role, " & _
                    "		Nominations.Comments, Nominations.LastUpdateByName, Nominations.LastUpdateByDate " & _
                    "       , ProductionCategory.ProductionCategory " & _
                    "       , ProductionCategory.PK_ProductionCategoryID " & _
                    " FROM  ProductionType INNER JOIN " & _
                    "	    Production ON ProductionType.PK_ProductionTypeID = Production.FK_ProductionTypeID INNER JOIN " & _
                    "	    Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID INNER JOIN " & _
                    "	    Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                    "	    AgeAppropriate ON Production.FK_AgeApproriateID = AgeAppropriate.PK_AgeAppropriateID LEFT OUTER JOIN " & _
                    "	    Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID " & _
                    "	    INNER JOIN ProductionCategory ON ProductionCategory.PK_ProductionCategoryID = Production.FK_ProductionCategoryID " & _
                    " WHERE PK_ProductionID=" & sProductionID

            dt = Run_SQL_Query(sSQL, FirstRowEmpty)

        Catch ex As Exception
            Throw
        End Try

        Return dt

    End Function

    Public Shared Function Get_AvailableAdjudicatorsForProduction(ByVal TrainingStartDate As String, _
                                    ByVal SortByAssignmentCounts As Boolean, _
                                    ByVal NominationID As String, _
                                    ByVal IncludeBackupAdjudicators As Boolean, _
                                    ByVal CompanyID_Producing As String, _
                                    Optional ByVal ShowCompanyName As Boolean = False, _
                                    Optional ByVal FirstRowEmpty As Boolean = False, _
                                    Optional ByVal ExcludeAlreadyAssigned As Boolean = True, _
                                    Optional ByVal ExcludeAlreadyAssignedFromSameCompany As Boolean = True, _
                                    Optional ByVal ReAssignment_CompanyID As String = "") _
                                As DataTable
        '====================================================================================================
        Dim sSQL As String = "", sSQLFullName As String = "", sBackupAdjudicators = ""
        '====================================================================================================
        ' Listing of Adjudicators that 
        ' - EXCLUDEs Adjudicators from the Company producing the Production to be Adjudicated
        ' - EXCLUDEs Assigned Adjudicators already assigned for ANY Production from the Producing Company
        ' - EXCLUDEs Adjudicators with Old Training Dates 
        '====================================================================================================
        If SortByAssignmentCounts = True Then
            sSQLFullName = "  '(' + CAST((SUM(ISNULL(AdjudicatorTotals.Count_AdjudicatorAssignment,0))) as VARCHAR(2)) + ')' + " & _
                            "   Users.LastName +  ', ' + Users.FirstName + "
        Else
            sSQLFullName = "  Users.LastName +  ', ' + Users.FirstName + " & _
                            " 	' (' + CAST((SUM(ISNULL(AdjudicatorTotals.Count_AdjudicatorAssignment,0))) as VARCHAR(2))  + ')' + "
        End If

        If ShowCompanyName = True Then
            sSQLFullName = sSQLFullName & _
                            " ' [' + CAST(ISNULL(Count_AdjudicationAssignmentsForCompany,0) AS VARCHAR(3)) + " & _
                            " 	' of ' + CAST(ISNULL(Total_CompanyRequiredAdjudications,0) AS VARCHAR(3)) +  " & _
                            " 	' - ' + RTrim(ISNULL(CompanyTotals.CompanyName,'')) + ']'  AS Fullname   "
        Else
            sSQLFullName = sSQLFullName & _
                            " ' [' + CAST(ISNULL(Count_AdjudicationAssignmentsForCompany,0) AS VARCHAR(3)) + " & _
                            " 	' of ' + CAST(ISNULL(Total_CompanyRequiredAdjudications,0) AS VARCHAR(3)) +  ']'  AS Fullname   "
        End If

        If IncludeBackupAdjudicators = True Then sBackupAdjudicators = " OR Users.FK_AccessLevelID = 5 "

        '=== Set main SQL Statement ===
        sSQL = "SELECT ExcludeCompany.PK_UserID, ExcludeCompany.FullName " & _
                " FROM  " & _
                "   ( SELECT Users.FK_CompanyID, Users.PK_UserID,  " & _
                    sSQLFullName & _
                " 	    FROM Production  " & _
                " 		    INNER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID  " & _
                "    		INNER JOIN Users ON Production.FK_CompanyID <> Users.FK_CompanyID  " & _
                "    		LEFT OUTER JOIN Scoring ON Users.PK_UserID <> Scoring.FK_UserID_Adjudicator AND Users.PK_UserID = Scoring.FK_UserID_Adjudicator  " & _
                "    		LEFT OUTER JOIN   " & _
                "    		( 	SELECT COUNT(Scoring.FK_UserID_Adjudicator) As Count_AdjudicatorAssignment, FK_UserID_Adjudicator " & _
                "   				FROM Scoring  " & _
                "   				GROUP BY FK_UserID_Adjudicator " & _
                "   		) AdjudicatorTotals  ON AdjudicatorTotals.FK_UserID_Adjudicator = Users.PK_UserID " & _
                "   		INNER JOIN  " & _
                "    		(	SELECT Company.PK_CompanyID, Company.CompanyName,  " & _
                "   					Company.NumOfProductions * 5 AS Total_CompanyRequiredAdjudications,  " & _
                "   					ISNULL(COUNT(Scoring.PK_ScoringID),0) AS Count_AdjudicationAssignmentsForCompany " & _
                " 	    		FROM Company  " & _
                "   				LEFT OUTER JOIN Scoring ON Company.PK_CompanyID = Scoring.FK_CompanyID_Adjudicator " & _
                "   			GROUP BY Company.PK_CompanyID, Company.CompanyName, Company.NumOfProductions * 5 " & _
                "   		) CompanyTotals ON CompanyTotals.PK_CompanyID = Users.FK_CompanyID  " & _
                "   	WHERE Active=1  " & _
                "    		AND (LastTrainingDate >= CONVERT(DATETIME, '" & TrainingStartDate & " 00:00:00', 102))   " & _
                "    	GROUP BY Users.FK_CompanyID, Users.PK_UserID, Users.LastName, Users.FirstName, Users.FK_AccessLevelID,  " & _
                "   				Nominations.PK_NominationsID, Production.FK_CompanyID, Count_AdjudicatorAssignment, " & _
                " 				CompanyTotals.CompanyName, CompanyTotals.Count_AdjudicationAssignmentsForCompany, CompanyTotals.Total_CompanyRequiredAdjudications " & _
                "   	HAVING (Nominations.PK_NominationsID = " & NominationID & ")  " & _
                "   		AND  (Users.FK_AccessLevelID = 1 OR Users.FK_AccessLevelID = 2 OR Users.FK_AccessLevelID = 4 " & sBackupAdjudicators & " ) " & _
                "   ) ExcludeCompany " & _
                "	    INNER JOIN  " & _
                "       ( " & _
                "           SELECT Users.PK_UserID,  COUNT(Scoring.FK_UserID_Adjudicator) AS CountOfAssignments " & _
                "			    FROM Users LEFT OUTER JOIN Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator " & _
                "				GROUP BY Users.PK_UserID, Scoring.FK_UserID_Adjudicator " & _
                "				HAVING COUNT(Scoring.FK_UserID_Adjudicator) <= (SELECT MaxShowsPerAdjudicator FROM ApplicationDefaults) " & _
                "       ) NotMaxedOnAssignments ON ExcludeCompany.PK_UserID = NotMaxedOnAssignments.PK_UserID "

        If ExcludeAlreadyAssigned = True Then
            '=== AlreadyAssigned SubQuery finds all Adjudicators already assigned to ANY Production from the Producing Company and
            '==== excludes them from the result set via: WHERE (AlreadyAssigned.PK_UserID Is NULL) 
            '===== Scoring.FK_CompanyID_Adjudicator > 2 is to allow more than 1 *Unafilliated Ajudicator to be assigned to a Production
            sSQL = sSQL & " LEFT OUTER JOIN  " & _
                "            	( " & _
                "            	SELECT Scoring.PK_ScoringID, Users.PK_UserID, Scoring.FK_NominationsID, Scoring.FK_CompanyID_Adjudicator, Company.PK_CompanyID " & _
                "            		FROM Nominations  " & _
                " 		           	    INNER JOIN Scoring  " & _
                "            			INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID  " & _
                "            							ON  Nominations.PK_NominationsID = Scoring.FK_NominationsID  " & _
                "            			INNER JOIN Production  " & _
                "            			INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID  " & _
                "            							ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                "            		WHERE Company.PK_CompanyID = " & CompanyID_Producing & _
                "            ) AlreadyAssigned ON ExcludeCompany.PK_UserID = AlreadyAssigned.PK_UserID "

            If ExcludeAlreadyAssignedFromSameCompany = True Then
                If ReAssignment_CompanyID.Length = 0 Then
                    sSQL = sSQL & " LEFT OUTER JOIN  " & _
                    "            	( " & _
                    "            	SELECT Scoring.FK_CompanyID_Adjudicator " & _
                    "            		FROM Nominations  " & _
                    " 		           	    INNER JOIN Scoring  " & _
                    "            			INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID  " & _
                    "            							ON  Nominations.PK_NominationsID = Scoring.FK_NominationsID  " & _
                    "            			INNER JOIN Production  " & _
                    "            			INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID  " & _
                    "            							ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                    "            		WHERE Scoring.FK_CompanyID_Adjudicator > 2 AND Company.PK_CompanyID = " & CompanyID_Producing & _
                    "            		GROUP BY Scoring.FK_CompanyID_Adjudicator " & _
                    "            ) AlreadyAssignedCompany ON ExcludeCompany.FK_CompanyID = AlreadyAssignedCompany.FK_CompanyID_Adjudicator " & _
                    "	WHERE (AlreadyAssigned.PK_UserID Is NULL) AND (AlreadyAssignedCompany.FK_CompanyID_Adjudicator Is NULL) " & _
                    "	GROUP BY ExcludeCompany.PK_UserID, ExcludeCompany.Fullname, AlreadyAssigned.PK_UserID " & _
                    "	ORDER BY ExcludeCompany.Fullname "
                Else
                    sSQL = sSQL & " LEFT OUTER JOIN  " & _
                    "            	( " & _
                    "            	SELECT Scoring.FK_CompanyID_Adjudicator " & _
                    "            		FROM Nominations  " & _
                    " 		           	    INNER JOIN Scoring  " & _
                    "            			INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID  " & _
                    "            							ON  Nominations.PK_NominationsID = Scoring.FK_NominationsID  " & _
                    "            			INNER JOIN Production  " & _
                    "            			INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID  " & _
                    "            							ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                    "            		WHERE Scoring.FK_CompanyID_Adjudicator > 2 " & _
                    "                           AND Company.PK_CompanyID = " & CompanyID_Producing & _
                    "                           AND NOT Scoring.FK_CompanyID_Adjudicator = " & ReAssignment_CompanyID & _
                    "            		GROUP BY Scoring.FK_CompanyID_Adjudicator " & _
                    "            ) AlreadyAssignedCompany ON ExcludeCompany.FK_CompanyID = AlreadyAssignedCompany.FK_CompanyID_Adjudicator " & _
                    "	WHERE (AlreadyAssigned.PK_UserID Is NULL) AND (AlreadyAssignedCompany.FK_CompanyID_Adjudicator Is NULL) " & _
                    "	GROUP BY ExcludeCompany.PK_UserID, ExcludeCompany.Fullname, AlreadyAssigned.PK_UserID " & _
                    "	ORDER BY ExcludeCompany.Fullname "
                End If

            Else
                sSQL = sSQL & " WHERE (AlreadyAssigned.PK_UserID Is NULL) " & _
                "	GROUP BY ExcludeCompany.PK_UserID, ExcludeCompany.Fullname, AlreadyAssigned.PK_UserID " & _
                "	ORDER BY ExcludeCompany.Fullname "
            End If
        Else
            If ExcludeAlreadyAssignedFromSameCompany = True Then
                sSQL = sSQL & " LEFT OUTER JOIN  " & _
                "            	( " & _
                "            	SELECT Scoring.FK_CompanyID_Adjudicator " & _
                "            		FROM Nominations  " & _
                " 		           	    INNER JOIN Scoring  " & _
                "            			INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID  " & _
                "            							ON  Nominations.PK_NominationsID = Scoring.FK_NominationsID  " & _
                "            			INNER JOIN Production  " & _
                "            			INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID  " & _
                "            							ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                "            		WHERE Scoring.FK_CompanyID_Adjudicator > 2 AND Company.PK_CompanyID = " & CompanyID_Producing & _
                "            		GROUP BY Scoring.FK_CompanyID_Adjudicator " & _
                "            ) AlreadyAssignedCompany ON ExcludeCompany.FK_CompanyID = AlreadyAssignedCompany.FK_CompanyID_Adjudicator " & _
                "	WHERE (AlreadyAssignedCompany.FK_CompanyID_Adjudicator Is NULL) " & _
                "	GROUP BY ExcludeCompany.PK_UserID, ExcludeCompany.Fullname " & _
                "	ORDER BY ExcludeCompany.Fullname "
            Else
                sSQL = sSQL & _
                "	GROUP BY ExcludeCompany.PK_UserID, ExcludeCompany.Fullname " & _
                "	ORDER BY ExcludeCompany.Fullname "
            End If

        End If

        Return Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Get_CompanyMemberEmails(ByVal CompanyID As String, _
                                                        Optional ByVal PK_UserID As String = "0", _
                                                        Optional ByVal iLowestUserAccessLevel As Int16 = 4, _
                                                        Optional ByVal bGetSecondaryAddresses As Boolean = False, _
                                                        Optional ByVal ShowNames As Boolean = False) _
                                                    As String
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        Dim sEmailAddresses As String = ""
        Dim iCount As Int16
        '====================================================================================================
        '1	Administrator	
        '2	Liaison & Adjudicator	
        '3	Liaison	
        '4	Primary Adjudicator
        '5	Backup Adjudicator
        '6	Inactive	
        '====================================================================================================
        'Get general Theatre Company Email address - this Query will remove duplicate Names & Addresses
        Try
            sSQL = "	SELECT RTRIM(CompanyName) + ' Email Address' as UserName, EmailAddress as EmailPrimary, '' as EmailSecondary " & _
                    "		FROM Company " & _
                    "		WHERE PK_CompanyID BETWEEN COALESCE(" & CompanyID & " , 1) AND COALESCE( " & CompanyID & " , POWER( 2., 31 ) - 1) " & _
                    "			AND NOT EmailAddress IS NULL" & _
                    " UNION " & _
                    "	SELECT FirstName + ' ' + LastName as UserName, EmailPrimary, EmailSecondary " & _
                    "		FROM Users " & _
                    "		WHERE (Users.Active = 1) " & _
                    "				AND FK_CompanyID BETWEEN COALESCE(" & CompanyID & " , 1) AND COALESCE( " & CompanyID & " , POWER( 2., 31 ) - 1) " & _
                    "	    		AND FK_AccessLevelID <= " & iLowestUserAccessLevel.ToString & "  " & _
                    "				AND ((LEN(EmailPrimary) > 1 OR LEN(EmailSecondary) > 1)) " & _
                    " UNION " & _
                    "	SELECT FirstName + ' ' + LastName as UserName, EmailPrimary, EmailSecondary FROM Users " & _
                    "		WHERE PK_UserID BETWEEN COALESCE(" & PK_UserID & " , 1) AND COALESCE( " & PK_UserID & " , POWER( 2., 31 ) - 1) " & _
                    "				AND ((LEN(EmailPrimary) > 1 OR LEN(EmailSecondary) > 1)) " & _
                    "				AND " & PK_UserID & "  IS NOT NULL "

            dt = DataAccess.Run_SQL_Query(sSQL)

            For iCount = 0 To dt.Rows.Count - 1
                If dt.Rows(iCount)("EmailPrimary").ToString.Length > 6 Then
                    If ShowNames = True Then
                        sEmailAddresses = sEmailAddresses & dt.Rows(iCount)("UserName").ToString & " [" & dt.Rows(iCount)("EmailPrimary").ToString.Trim & "], "
                    Else
                        sEmailAddresses = sEmailAddresses & dt.Rows(iCount)("EmailPrimary").ToString.Trim & ", "
                    End If
                End If

                If bGetSecondaryAddresses = True And dt.Rows(iCount)("EmailSecondary").ToString.Length > 6 Then
                    If ShowNames = True Then
                        sEmailAddresses = sEmailAddresses & dt.Rows(iCount)("UserName").ToString & " [" & dt.Rows(iCount)("EmailSecondary").ToString.Trim & "], "
                    Else
                        sEmailAddresses = sEmailAddresses & dt.Rows(iCount)("EmailSecondary").ToString.Trim & ", "
                    End If

                End If
            Next
        Catch ex As Exception
            Throw
        End Try

        '====================================================================================================
        Return sEmailAddresses

    End Function

    Public Shared Function Get_ApplicationDefaults() As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT TOP 1 * FROM ApplicationDefaults "

        Return DataAccess.Run_SQL_Query(sSQL)

    End Function

    Public Shared Function Find_Late_AdjudicationsScores(Optional ByVal sSortField As String = "", Optional ByVal sSortOrder As String = "") As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        Try
            sSQL = "SELECT Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator, Nominations.PK_NominationsID, " & _
                    "       Production.PK_ProductionID, Company.PK_CompanyID, Production.FK_VenueID, Scoring.AdjudicatorRequestsReassignment, Production.Title, " & _
                    "       Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Company.CompanyName, " & _
                    "       Users.PK_UserID, Users.UserLoginID, Users.LastName + ', ' + Users.FirstName AS FullName, " & _
                    "       Users.LastName, Users.FirstName, " & _
                    "       Users.EmailPrimary, Users.EmailSecondary, Users.PhonePrimary, Users.PhoneSecondary, Scoring.ProductionDateAdjudicated_Planned, " & _
                    "       Scoring.LastUpdateByName, Scoring.LastUpdateByDate, " & _
                    "       Production.LastPerformanceDateTime + (SELECT TOP 1 DaysToWaitForScoring FROM ApplicationDefaults) AS ScoreDueDate,  " & _
                    "       Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore + " & _
                    "       Scoring.SoundDesignerScore + Scoring.CostumeDesignerScore + Scoring.OriginalPlaywrightScore + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score " & _
                    "           + Scoring.BestSupportingActor1Score + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score + Scoring.BestSupportingActress2Score AS TotalScore, " & _
                    "       (SELECT TOP 1 ApplicationName FROM ApplicationDefaults) AS ApplicationName " & _
                    " FROM  Production INNER JOIN Company ON " & _
                    "       Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN Nominations INNER JOIN " & _
                    "       Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON " & _
                    "       Production.PK_ProductionID = Nominations.FK_ProductionID LEFT OUTER JOIN " & _
                    "       Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                    " WHERE Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore + " & _
                    "           Scoring.SoundDesignerScore + Scoring.CostumeDesignerScore + Scoring.OriginalPlaywrightScore + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score  " & _
                    "           + Scoring.BestSupportingActor1Score + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score + Scoring.BestSupportingActress2Score = 0  " & _
                    "       AND (GETDATE() >= Production.LastPerformanceDateTime + (SELECT TOP 1 DaysToWaitForScoring FROM ApplicationDefaults)) "

            If sSortField <> "" Then
                sSQL = sSQL + " ORDER BY " + sSortField + " " + sSortOrder
            End If

            Return Run_SQL_Query(sSQL)

        Catch ex As Exception
            Throw
        End Try

    End Function



    Public Shared Function Find_Late_Ballots(Optional ByVal sSortField As String = "", Optional ByVal sSortOrder As String = "") As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        Try
            sSQL = "SELECT Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator, Nominations.PK_NominationsID, Production.PK_ProductionID, Company.PK_CompanyID,  " & _
                    "	    Production.FK_VenueID, Scoring.AdjudicatorRequestsReassignment, Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,  " & _
                    "	    Company.CompanyName, Users.PK_UserID, Users.UserLoginID, Users.LastName, Users.FirstName, Users.Active, Users.LastName + ', ' + Users.FirstName AS FullName,  " & _
                    "	    Users.FirstName + ' ' + Users.LastName AS FullNameForward, Users.EmailPrimary, Users.EmailSecondary, Users.PhonePrimary, Users.PhoneSecondary,  " & _
                    "	    Scoring.ProductionDateAdjudicated_Planned, Scoring.LastUpdateByName, Scoring.LastUpdateByDate,  " & _
                    "	    Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore + Scoring.SoundDesignerScore + Scoring.CostumeDesignerScore " & _
                    "	     + Scoring.OriginalPlaywrightScore + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score + Scoring.BestSupportingActor1Score + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score " & _
                    "	     + Scoring.BestSupportingActress2Score AS TotalScore, " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults) AS DaysToWaitForScoring, Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults AS ApplicationDefaults_3) AS ScoreDueDate, Company_User.CompanyName AS CompanyName_User, CASE WHEN (GetDate()  " & _
                    "	    >= Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults)) AND (GetDate() < (Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults) * 2)) THEN 1 ELSE 0 END AS Late30to60Days, CASE WHEN (GetDate() >= (Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults) * 2)) AND (GetDate() < (Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults) * 3)) THEN 1 ELSE 0 END AS Late60to90Days, CASE WHEN (GetDate() >= (Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM     ApplicationDefaults) * 3)) THEN 1 ELSE 0 END AS Late90DaysOrMore, " & _
                    "	        (SELECT TOP (1) ApplicationName " & _
                    "	          FROM     ApplicationDefaults AS ApplicationDefaults_2) AS ApplicationName, Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus, UserAccessLevels.AccessLevelName,  " & _
                    "	    UserStatus.UserStatus, UserStatus.PK_UserStatusID " & _
                    "	FROM Company AS Company_User LEFT OUTER JOIN " & _
                    "	    Users ON Company_User.PK_CompanyID = Users.FK_CompanyID RIGHT OUTER JOIN " & _
                    "	    Production INNER JOIN " & _
                    "	    Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                    "	    Nominations INNER JOIN " & _
                    "	    Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID ON  " & _
                    "	    Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN " & _
                    "	    ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID INNER JOIN " & _
                    "	    UserAccessLevels ON Users.FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID INNER JOIN " & _
                    "	    UserStatus ON Users.FK_UserStatusID = UserStatus.PK_UserStatusID " & _
                    "	WHERE (Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore + Scoring.SoundDesignerScore + Scoring.CostumeDesignerScore " & _
                    "	     + Scoring.OriginalPlaywrightScore + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score + Scoring.BestSupportingActor1Score + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score " & _
                    "	     + Scoring.BestSupportingActress2Score = 0) AND (GETDATE() >= Production.LastPerformanceDateTime + " & _
                    "	        (SELECT TOP (1) DaysToWaitForScoring " & _
                    "	          FROM ApplicationDefaults AS ApplicationDefaults_1))"

            If sSortField <> "" Then
                sSQL = sSQL + " ORDER BY " + sSortField + " " + sSortOrder
            End If

            Return Run_SQL_Query(sSQL)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function Find_Late_Ballots_WHATISTHIS(Optional ByVal sSortField As String = "", Optional ByVal sSortOrder As String = "") As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        Try
            sSQL = " SELECT	Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator, Nominations.PK_NominationsID, Production.PK_ProductionID,  " & _
                    "			 Company.PK_CompanyID, Production.FK_VenueID, Scoring.AdjudicatorRequestsReassignment, Production.Title, Production.FirstPerformanceDateTime,  " & _
                    "			 Production.LastPerformanceDateTime, Company.CompanyName, Users.PK_UserID, Users.UserLoginID, Users.LastName + ' " & _
                    "		, ' + Users.FirstName AS FullName,  " & _
                    "			 Users.LastName, Users.FirstName, Users.Active, Users.EmailPrimary, Users.EmailSecondary, Users.PhonePrimary, Users.PhoneSecondary,  " & _
                    "			 Scoring.ProductionDateAdjudicated_Planned, Scoring.LastUpdateByName, Scoring.LastUpdateByDate,  " & _
                    "			 Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore + Scoring.SoundDesignerScore " & _
                    "			  + Scoring.CostumeDesignerScore + Scoring.OriginalPlaywrightScore + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score + Scoring.BestSupportingActor1Score " & _
                    "			  + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score + Scoring.BestSupportingActress2Score AS TotalScore, " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults) AS DaysToWaitForScoring, Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults AS ApplicationDefaults_3) AS ScoreDueDate, Company_User.CompanyName AS CompanyName_User, CASE WHEN (GetDate()  " & _
                    "			 >= Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults)) AND (GetDate() < (Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults) * 2)) THEN 1 ELSE 0 END AS Late30to60Days, CASE WHEN (GetDate() >= (Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults) * 2)) AND (GetDate() < (Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults) * 3)) THEN 1 ELSE 0 END AS Late60to90Days, CASE WHEN (GetDate() >= (Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults) * 3)) THEN 1 ELSE 0 END AS Late90DaysOrMore, " & _
                    "			     (SELECT	TOP (1) ApplicationName " & _
                    "			       FROM	    ApplicationDefaults AS ApplicationDefaults_2) AS ApplicationName, Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus " & _
                    "FROM	    Company AS Company_User LEFT OUTER JOIN " & _
                    "			 Users ON Company_User.PK_CompanyID = Users.FK_CompanyID RIGHT OUTER JOIN " & _
                    "			 Production INNER JOIN " & _
                    "			 Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN " & _
                    "			 Nominations INNER JOIN " & _
                    "			 Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID ON  " & _
                    "			 Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN " & _
                    "			 ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID " & _
                    "WHERE	(Scoring.DirectorScore + Scoring.MusicalDirectorScore + Scoring.ChoreographerScore + Scoring.ScenicDesignerScore + Scoring.LightingDesignerScore + Scoring.SoundDesignerScore " & _
                    "			  + Scoring.CostumeDesignerScore + Scoring.OriginalPlaywrightScore + Scoring.BestPerformer1Score + Scoring.BestPerformer2Score + Scoring.BestSupportingActor1Score " & _
                    "			  + Scoring.BestSupportingActor2Score + Scoring.BestSupportingActress1Score + Scoring.BestSupportingActress2Score = 0) AND (GETDATE()  " & _
                    "			 >= Production.LastPerformanceDateTime + " & _
                    "			     (SELECT	TOP (1) DaysToWaitForScoring " & _
                    "			       FROM	    ApplicationDefaults AS ApplicationDefaults_1)) "

            If sSortField <> "" Then
                sSQL = sSQL + " ORDER BY " + sSortField + " " + sSortOrder
            End If

            Return Run_SQL_Query(sSQL)

        Catch ex As Exception
            Throw
        End Try

    End Function


    Public Shared Function Find_AdjudicationsForCompanyFromUserLoginID(ByVal sUserLoginID As String) As DataTable
        '====================================================================================================
        Dim dt As DataTable = New DataTable, dt2 As New DataTable
        Dim sSQL As String
        '====================================================================================================

        Try
            sSQL = "SELECT PK_UserID, FK_CompanyID FROM Users WHERE UserLoginID = '" & sUserLoginID & "' "
            dt = Run_SQL_Query(sSQL)

            If dt.Rows.Count > 0 Then
                sSQL = " SELECT Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator,  " & _
                            "		(SELECT TOP 1 DaysToConfirmAttendance FROM ApplicationDefaults) as DaysToConfirmAttendance, " & _
                            "		(SELECT TOP 1 DaysToWaitForScoring FROM ApplicationDefaults) as DaysToWaitForScoring, " & _
                            "	    PK_NominationsID, PK_ProductionID, PK_CompanyID, FK_VenueID, BallotSubmitDate, " & _
                            "       AdjudicatorRequestsReassignment, AdjudicatorRequestsReassignmentNote, " & _
                            "       Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
                            "	    Company.CompanyName, Users.LastName + ', ' + Users.FirstName as FullName, " & _
                            "       ProductionDateAdjudicated_Planned, Scoring.LastUpdateByName, Scoring.LastUpdateByDate, " & _
                            "	    CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END as AdjudicatorToSeeProduction, " & _
                            "       DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore " & _
                            "	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score " & _
                            "	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score " & _
                            "	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore  " & _
                            "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                            "	FROM Production " & _
                            "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                            "	    INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                            "	    LEFT OUTER JOIN Nominations ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
                            "	    INNER JOIN Scoring ON Scoring.FK_NominationsID = Nominations.PK_NominationsID " & _
                            "	    LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                            "   WHERE Users.FK_CompanyID = " & dt.Rows(0)("FK_CompanyID").ToString & _
                            "   ORDER BY Users.LastName, Users.FirstName, FirstPerformanceDateTime"

                dt2 = Run_SQL_Query(sSQL)
            End If

        Catch ex As Exception
            Throw
        End Try

        Return dt2

    End Function

    Public Shared Function Find_AdjudicationsForUserLoginID(ByVal sUserLoginID As String, Optional ByVal sScoringID As String = "") As DataTable
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable, dt2 As New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_UserID FROM Users WHERE UserLoginID = '" & sUserLoginID & "' "

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    sSQL = " SELECT Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator, Scoring.ReserveAdjudicator, " & _
                                "		(SELECT TOP 1 DaysToConfirmAttendance FROM ApplicationDefaults) as DaysToConfirmAttendance, " & _
                                "		(SELECT TOP 1 DaysToWaitForScoring FROM ApplicationDefaults) as DaysToWaitForScoring, " & _
                                "	    PK_NominationsID, PK_ProductionID, Company.PK_CompanyID, FK_VenueID, BallotSubmitDate, " & _
                                "       AdjudicatorRequestsReassignment, AdjudicatorRequestsReassignmentNote, " & _
                                "       Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
                                "	    Company.CompanyName, " & _
                                "       Users.LastName, Users.FirstName, Users.LastName + ', ' + Users.FirstName as FullName, " & _
                                "       Users.EmailPrimary, Users.FirstName, Users.LastName + ', ' + Users.FirstName as FullName, " & _
                                "       ProductionDateAdjudicated_Planned, Scoring.LastUpdateByName, Scoring.LastUpdateByDate, " & _
                                "	    CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END as AdjudicatorToSeeProduction, " & _
                                "       DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore " & _
                                "	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score " & _
                                "	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score " & _
                                "	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore,  " & _
                                "       ScoringCompany.CompanyName AS ScoringCompanyName, " & _
                                "       Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus " & _
                                "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                                "	FROM Production " & _
                                "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                                "	    INNER JOIN Company " & _
                                "	        ON  Production.FK_CompanyID = Company.PK_CompanyID " & _
                                "	    INNER JOIN Nominations " & _
                                "	    INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID  " & _
                                "	        ON Production.PK_ProductionID = Nominations.FK_ProductionID " & _
                                "	    LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
                                "       LEFT OUTER JOIN Company ScoringCompany ON Scoring.FK_CompanyID_Adjudicator = ScoringCompany.PK_CompanyID " & _
                                "	    INNER JOIN ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID " & _
                                "   WHERE FK_UserID_Adjudicator = " & dt.Rows(0)("PK_UserID") & _
                                IIf(sScoringID.Length > 0, " AND PK_ScoringID=" & sScoringID, "") & _
                                "   ORDER BY FirstPerformanceDateTime"

                    da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                    da.Fill(dt2)
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

        Return dt2
    End Function

    Public Shared Function Find_ProductionsForUserLoginID(ByVal sUserLoginID As String) As DataTable
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable, dt2 As New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT FK_CompanyID FROM Users WHERE (FK_AccessLevelID <= 3) AND (FK_CompanyID > 2) AND (UserLoginID = '" & sUserLoginID & "') "

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)

                If dt.Rows.Count > 0 Then

                    sSQL = "SELECT Production.PK_ProductionID, Production.Title, ProductionType.ProductionType, Production.FirstPerformanceDateTime, " & _
                                "       Production.LastPerformanceDateTime, Production.RequiresAdjudication, Production.LastUpdateByName, Production.LastUpdateByDate, " & _
                                "       OriginalProduction " & _
                                " FROM Production INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
                                " WHERE FK_CompanyID = " & dt.Rows(0)("FK_CompanyID") & _
                                " ORDER BY FirstPerformanceDateTime, LastPerformanceDateTime"

                    da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                    da.Fill(dt2)
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

        Return dt2

    End Function

    Public Shared Function Find_PK_UserID(ByVal sUserLoginID As String) As String
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_UserID FROM Users WHERE (UserLoginID = '" & sUserLoginID & "') "

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt.Rows(0)("PK_UserID")
                Else
                    Return 0
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function CheckExists_UserID(ByVal sUserLoginID As String) As String
        'THIS FUNCTION DOES NOT MAKE SENSE... THE RETURN VALUE IS THE SAME AS THE PASSED IN VALUE
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT UserLoginID FROM Users WHERE (UserLoginID = '" & sUserLoginID & "') "

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt.Rows(0)("UserLoginID")
                Else
                    Return 0
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function Get_UserRecord(ByVal sUserID As String) As DataTable
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT Users.*, FirstName + ' ' + LastName AS FullName, " & _
                    "       Company.PK_CompanyID, Company.CompanyName, Users.FK_CompanyID, " & _
                    "       UserAccessLevels.AccessLevelName " & _
                    "   FROM Users " & _
                    "       INNER JOIN Company ON Company.PK_CompanyID = Users.FK_CompanyID " & _
                    "       INNER JOIN UserAccessLevels ON Users.FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID " & _
                    "   WHERE PK_UserID = " & sUserID

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                Return dt
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function Find_CompanyForUserID(ByVal sUserID As String) As Integer
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT FK_CompanyID FROM Users WHERE PK_UserID = " & sUserID

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt.Rows(0)("FK_CompanyID")
                Else
                    Return 0
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function Find_CompanyForLiaison(ByVal sUserLoginID As String) As Integer
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT FK_CompanyID FROM Users WHERE (FK_AccessLevelID <= 3) AND (UserLoginID = '" & sUserLoginID & "') "

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt.Rows(0)("FK_CompanyID")
                Else
                    Return 0
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function Find_CompanyForUserLoginID(ByVal sUserLoginID As String) As Integer
        '====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT FK_CompanyID FROM Users WHERE (FK_AccessLevelID <= 3) AND (FK_CompanyID > 2) AND (UserLoginID = '" & sUserLoginID & "') "

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt.Rows(0)("FK_CompanyID")
                Else
                    Return 0
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function Get_LoginID(ByVal PK_UserID As String) As String
        '==================================================================================================
        Dim sSQL As String, dt As DataTable
        '==================================================================================================

        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                sSQL = "SELECT UserLoginID FROM Users WHERE PK_UserID = " & PK_UserID
                dt = DataAccess.Run_SQL_Query(sSQL)

                If dt.Rows.Count > 0 Then
                    Return dt.Rows(0)("UserLoginID")
                Else
                    Return "ERROR: PK_UserID not Found"
                End If
            End Using

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function Get_UserID(ByVal sUserLoginID As String) As Integer
        '==================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        Dim sSQL As String
        '==================================================================================================
        sSQL = "SELECT PK_UserID FROM Users WHERE UserLoginID = '" & sUserLoginID & "'"
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return CInt(dt.Rows(0)("PK_UserID"))
                Else
                    Return 0
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            ' SQLConn.Close()
        End Try

    End Function

    Public Shared Function SQLSelect(ByVal sSQL As String) As DataTable
        '====================================================================================================
        ' Calls the Database with the provided SQL, returns a DataTable
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        '====================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, SQLConn)
                da.Fill(dt)
                Return dt
            End Using

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Sub SQLDelete(ByVal sSQL As String)
        '==================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.DeleteCommand = New SqlCommand(sSQL, SQLConn)
                SQLConn.Open()
                da.DeleteCommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub SQLUpdate(ByVal sSQL As String)
        '==================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.UpdateCommand = New SqlCommand(sSQL, SQLConn)
                SQLConn.Open()
                da.UpdateCommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Sub

    Public Shared Sub SQLInsert(ByVal sSQL As String)
        '==================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        '==================================================================================================
        Try
            Using SQLConn As New SqlConnection(sSQLConnectString)
                da.InsertCommand = New SqlCommand(sSQL, SQLConn)
                SQLConn.Open()
                da.InsertCommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Sub


    Public Shared Function Email_NewPassword(ByVal UserLoginID As String, Optional ByVal NewUser As Boolean = False, Optional ByVal GenerateNewPassword As Boolean = True) As Boolean
        '============================================================================================
        Dim dt As DataTable, dtEmail As DataTable
        Dim sSubject As String, sBody As String = "", sUserBody As String = ""
        Dim sNewPassword As String
        Dim sEmailTo As String = ""
        '============================================================================================

        Try
            dt = DataAccess.Get_UserRecord(Find_PK_UserID(UserLoginID))

            ' Generate a Random NEW password that is 8 characters long
            sNewPassword = Common.Get_RandomPassword(8)

            ' Create the Text for the Email Message
            If NewUser = True Then
                sSubject = "NHTA Adjudication Login Information"
                sBody = sBody & "<B>" & dt.Rows(0)("FullName").ToString & "</b> welcome to the NHTA Adjudication Website<br /><br />"
            Else
                If GenerateNewPassword = True Then
                    sSubject = "NHTA Adjudication Password Reset Request"
                    sBody = sBody & "<B>" & dt.Rows(0)("FullName").ToString & "</b>,<br /><br />"
                    sBody = sBody & "Per your request, your password has been reset.<br />"
                Else
                    sSubject = "NHTA Adjudication website Information for '" & dt.Rows(0)("FullName").ToString & "'"
                    sBody = sBody & "<B>" & dt.Rows(0)("FullName").ToString & "</b>,<br /><br />"
                    sBody = sBody & "A NHTA Website Administrator has reviewed your User information.  Please take a moment to verify this information is correct.<br /><br />"
                End If
            End If

            If GenerateNewPassword = True Or NewUser = True Then
                sBody = sBody & "<UL><LI>NHTA Login ID:     <B>" & dt.Rows(0)("UserLoginID").ToString & "</B>"
                sBody = sBody & "<LI>Your NEW Password:     <B>" & sNewPassword & "</B></UL>"
                sBody = sBody & "  <I>Reminder: passwords are Case sensitive</I><br /><br />"
                sBody = sBody & "After you login please change your password to something you will remember by using the <I>'Change My Password'</I> link on the left side menu.<br /><br />"
                sBody = sBody & "If you have not already done so, we strongly encourage you to fill out the ""Security Question and Answer"" under the left navigation link  ""My Account Info"" which is used to more securely perform automated password reset requests.<br /><br />"
            End If

            sBody = sBody & "<B>" & dt.Rows(0)("FullName").ToString & "</b> Personal Information: <i>(please update yourself after you login)</i><br />"
            sBody = sBody & "<UL><LI>Website Access Level:  <B>" & dt.Rows(0)("AccessLevelName").ToString & "</B>"
            sBody = sBody & "<LI>NHTA Login ID:             <B>" & dt.Rows(0)("UserLoginID").ToString & "</B>"
            sBody = sBody & "<LI>System Active:             <B>" & IIf(dt.Rows(0)("Active").ToString = "1", "Yes", "No") & "</B>"
            sBody = sBody & "<LI>Affiliated Theatre Company:<B>" & dt.Rows(0)("CompanyName").ToString & "</B>"
            sBody = sBody & "<LI>Last NHTA Training Date:   <B>" & dt.Rows(0)("LastTrainingDate").ToString & "</B>"
            sBody = sBody & "<LI>Primary Phone Number:      <B>" & dt.Rows(0)("PhonePrimary").ToString & "</B>"
            sBody = sBody & "<LI>Secondary Phone Number:    <B>" & dt.Rows(0)("PhoneSecondary").ToString & "</B>"
            sBody = sBody & "<LI>Mailing Address:           <B>" & dt.Rows(0)("Address").ToString & ", " & dt.Rows(0)("City").ToString & " " & dt.Rows(0)("State").ToString & ", " & dt.Rows(0)("ZIP").ToString & "</B>"
            sBody = sBody & "<LI>Account Notes:             <B>" & dt.Rows(0)("UserInformation").ToString & "</B>"
            sBody = sBody & "<LI>Last Login Date:           <B>" & dt.Rows(0)("LastLoginTime").ToString & "</B>"
            sBody = sBody & "</UL>"

            'sBody = sBody & ConfigurationManager.AppSettings("PasswordResetEmailEndMessage").ToString & "<br />"
            sBody = sBody & Common.Get_EmailFooter()

            '====================================================================================================================================================================================
            ' Send the Email & Run the SProc to change the Password (done AFTER email is sent)
            If ValidateEmailAddress(dt.Rows(0)("EmailPrimary").ToString) = True Then sEmailTo = dt.Rows(0)("EmailPrimary").ToString
            If ValidateEmailAddress(dt.Rows(0)("EmailSecondary").ToString) = True Then sEmailTo = sEmailTo & ", " & dt.Rows(0)("EmailSecondary").ToString

            SendCDOEmail(ConfigurationManager.AppSettings("PasswordResetEmailFrom").ToString, sEmailTo, False, sSubject, sBody, True, True, ConfigurationManager.AppSettings("PasswordResetEmailFrom").ToString)

            '====================================================================================================================================================================================
            If GenerateNewPassword = True Or NewUser = True Then
                dtEmail = DataAccess.Secure_PasswordChange(UserLoginID, sNewPassword)
                dtEmail.Clear()
                dtEmail = DataAccess.Secure_ResetAccount(UserLoginID, "JoeVago@NHTheatreAwards.org")   ' HACK: JoeVago here is an Admin Level ID
            End If

            '====================================================================================================================================================================================
            If NewUser = True Then
                ' for New Users Get all Liaison, Primary and Backup Adjudicator email addresses
                sEmailTo = sEmailTo & ", " & Get_CompanyMemberEmails(dt.Rows(0)("PK_CompanyID").ToString, dt.Rows(0)("PK_UserID").ToString, 5, False)

                'Create new Email Subject and Body when notifying Company members
                sSubject = "NHTA Adjudication Website has associated " & dt.Rows(0)("FullName").ToString & " with " & dt.Rows(0)("CompanyName").ToString
                sBody = "You have received this email because you are listed as an NHTA active member of <b>" & dt.Rows(0)("CompanyName").ToString & "</b>.<br /><br />"
                sBody = sBody & "This is to let you know that <b>" & dt.Rows(0)("FullName").ToString & "</b> <br />"
                sBody = sBody & "has been associated with <b>" & dt.Rows(0)("CompanyName").ToString & "</b>.<br /><br />"
                sBody = sBody & "This email has been sent to you for informational purposes only.<br /><br />Best Regards,<br /><br />NHTA Administrators<br />"

                'Create Email body message for the Company members 
                sBody = sBody & Common.Get_EmailFooter

                SendCDOEmail(ConfigurationManager.AppSettings("PasswordResetEmailFrom").ToString, sEmailTo, False, sSubject, sBody, True, True, ConfigurationManager.AppSettings("PasswordResetEmailFrom").ToString)

            End If

            ' Successful
            Return True

        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function

    Public Shared Function Get_ScoringStatus(Optional ByVal sPK_ScoringStatusID As String = "", Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_ScoringStatusID, ScoringStatus, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate " & _
                "   FROM ScoringStatus "

        If sPK_ScoringStatusID.Length > 0 Then sSQL = sSQL + " WHERE PK_ScoringStatusID=" + sPK_ScoringStatusID

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Get_ScoringStatus_Selections(Optional ByVal FirstRowEmpty As Boolean = False) As DataTable
        '====================================================================================================
        Dim sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_ScoringStatusID, ScoringStatus, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate " & _
                "   FROM ScoringStatus "

        sSQL = sSQL + " WHERE PK_ScoringStatusID < 5 "

        Return DataAccess.Run_SQL_Query(sSQL, FirstRowEmpty)

    End Function

    Public Shared Function Set_TestSessionValues(FieldName As String) As String
        Dim dt As New DataTable
        dt = Get_UserRecord(ConfigurationManager.AppSettings.Get("IsTestMode_PK_UserID").Trim())
        Dim returnValue As String = dt.Rows(0)(FieldName).ToString
        Return returnValue
    End Function

    Public Shared Function IsTestMode() As Boolean
        '================================================================================================================================
        '=== Checks the Web.config file for the "IsTestMode" Key.  If key is not found also returns FALSE
        '================================================================================================================================
        Try
            If ConfigurationManager.AppSettings("IsTestMode").ToString.ToUpper = "TRUE" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


    '||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    Public Shared Function SQLStoredProcedure(ByVal sProcedureName As String) As DataTable
        ' ====================================================================================================
        ' Calls the Database with the provided SQL Stored Procedure Name, returns a DataTable
        ' >>> NOTE: Only works with Stored Procedures that return a recordset.
        ' ====================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim dt As DataTable = New DataTable
        '==========================================================================================================================================================================

        Try
            Using mySQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sProcedureName, mySQLConn)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.Fill(dt)
            End Using

        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Shared Function AddEmptyRow(ByVal dTable As DataTable) As DataTable
        '==========================================================================================================================================================================
        Dim dr As DataRow
        '==========================================================================================================================================================================
        dr = dTable.NewRow
        dr(0) = 0       'set initial values for blank row for validation controls
        'dr(1) = ""      'set initial values for blank row for validation controls

        dTable.Rows.InsertAt(dr, 0)

        Return dTable

    End Function

    Public Shared Function AddNullRow(ByVal dTable As DataTable) As DataTable
        '==========================================================================================================================================================================
        Dim dr As DataRow
        '==========================================================================================================================================================================
        dr = dTable.NewRow
        dr(0) = ""       'set initial values for blank row for validation controls
        'dr(1) = ""      'set initial values for blank row for validation controls

        dTable.Rows.InsertAt(dr, 0)

        Return dTable

    End Function

    Public Shared Function AddZeroRow(ByVal dTable As DataTable) As DataTable
        Dim dr As DataRow
        dr = dTable.NewRow
        dr(0) = 0       'set initial values for blank row for validation controls
        'dr(1) = ""      'set initial values for blank row for validation controls

        dTable.Rows.InsertAt(dr, 0)

        Return dTable

    End Function

    Public Shared Function AddUnassignedRow(ByVal dTable As DataTable) As DataTable
        Dim dr As DataRow
        dr = dTable.NewRow
        dr(0) = 1       'set initial values for unassigned row for validation controls
        dr(1) = "*Unassigned"      'set initial values for unassigned row for validation controls

        dTable.Rows.InsertAt(dr, 0)

        Return dTable

    End Function

    Public Shared Function Run_SQL_Query(ByVal sSQL As String, Optional ByVal FirstRowEmpty As Boolean = False, Optional ByVal RowUnassigned As Boolean = False) As DataTable
        '==========================================================================================================================================================================
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable
        '==========================================================================================================================================================================

        Try
            Using mySQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, mySQLConn)
                da.Fill(dt)

                If FirstRowEmpty = True Then
                    dt = AddEmptyRow(dt)
                End If
                If RowUnassigned = True Then
                    dt = AddUnassignedRow(dt)
                End If
            End Using               'connection closed 

        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Shared Function SQLExecuteNonQuery(ByVal sSQL As String) As Boolean
        '==========================================================================================================================================================================
        '=== NOTE: Runs a SQL statement with no returning data via .ExecuteNonQuery()
        '==========================================================================================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        '==========================================================================================================================================================================
        Try
            Using mySQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, mySQLConn)
                da.SelectCommand.CommandType = CommandType.Text                             'Tell command we are using a TEXT query

                mySQLConn.Open()
                da.SelectCommand.ExecuteNonQuery()                                          '>>>>>FOR TESTING: GetSqlQueryFromSQLCommand(da.SelectCommand)

                da.SelectCommand.Dispose()
                Return True
            End Using
        Catch ex As Exception
            Throw ex
        End Try

        Return False

    End Function


End Class

Public Class SecureSQL
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    
    Public Shared sSQLConnectString As String = ConfigurationManager.ConnectionStrings("NHTA").ToString
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Shared Function SQLInsert(ByVal TableName As String, _
                                        ByVal PrimaryKeys As Dictionary(Of String, Integer), _
                                        ByVal UpdateBy As String, ByVal CallingPageName As String, _
                                        Optional ByVal boolSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal stringSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal dateSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal integerSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal AutoSaveLastUpdatedDate As Boolean = True, _
                                        Optional ByVal PrimaryKeyReturnValueOverride As Integer = 0, _
                                        Optional ByVal AccountNum As String = "", _
                                        Optional ByVal ChannelName As String = "", _
                                        Optional ByVal ServiceName As String = "") As Integer
        '==========================================================================================================================================================================
        '=== NOTE: Inserts 1 Record in Specified Table
        '==========================================================================================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim sSQL As String = String.Empty, SQLFieldNames As String = String.Empty, SQLValues As String = String.Empty
        Dim dt As New DataTable, dtPK As New DataTable                                                       'an empty dummy table for the Save_AuditLogging function
        Dim htNew As New Hashtable
        '==========================================================================================================================================================================
        Try
            If AutoSaveLastUpdatedDate = True Then
                If UpdateBy.Length > 0 Then stringSaveParms.Add("LastUpdateByName", UpdateBy) 'could manually string this, but cleaner to make a parameter
                If UpdateBy.Length > 0 Then stringSaveParms.Add("CREATE_BY_NM", UpdateBy) 'could manually string this, but cleaner to make a parameter
            End If

            If Not boolSaveParms Is Nothing Then SQLValues = SQLValues & ParameterizedSQL_InsertString(boolSaveParms, htNew, SQLFieldNames)
            If Not dateSaveParms Is Nothing Then SQLValues = SQLValues & ParameterizedSQL_InsertString(dateSaveParms, htNew, SQLFieldNames)
            If Not stringSaveParms Is Nothing Then SQLValues = SQLValues & ParameterizedSQL_InsertString(stringSaveParms, htNew, SQLFieldNames)
            If Not integerSaveParms Is Nothing Then SQLValues = SQLValues & ParameterizedSQL_InsertString(integerSaveParms, htNew, SQLFieldNames)

            'Create base INSERT statement
            If AutoSaveLastUpdatedDate = True Then
                sSQL = " INSERT INTO " & TableName & _
                        "        (LastUpdateByDate , CREATE_BY_DT " & SQLFieldNames & ")" & _
                        " VALUES (GetDate() , GetDate() " & SQLValues & ")"
            Else
                sSQL = " INSERT INTO " & TableName & _
                        "        (" & SQLFieldNames.TrimStart(",") & ")" & _
                        " VALUES (" & SQLValues.TrimStart(",") & ")"
            End If

            sSQL = sSQL & "  SELECT SCOPE_IDENTITY() as InsertedPrimaryKeyValue "

            Using mySQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, mySQLConn)                          'Create the concatendated SQL statement 
                da.SelectCommand.CommandType = CommandType.Text                             'Tell command we are using a TEXT query

                '=== Add Parameters and values  (Parameters used to prevent SQL Injection Attacks) ===
                If Not boolSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(boolSaveParms, da, SqlDbType.Bit)
                If Not dateSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(dateSaveParms, da, SqlDbType.DateTime)
                If Not stringSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(stringSaveParms, da, SqlDbType.VarChar)
                If Not integerSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(integerSaveParms, da, SqlDbType.Int)

                mySQLConn.Open()                                                            'OLD Version w/o SCOPE_IDENTITY() value returned: da.SelectCommand.ExecuteNonQuery()    
                da.Fill(dtPK)                                                               '>>>>>FOR TESTING: GetSqlQueryFromSQLCommand(da.SelectCommand)

                If dtPK.Rows.Count > 0 And PrimaryKeyReturnValueOverride = 0 Then
                    If dtPK.Rows(0).Item("InsertedPrimaryKeyValue").ToString.Length > 0 Then PrimaryKeys.Add("1", CInt(dtPK.Rows(0).Item("InsertedPrimaryKeyValue")))
                End If

                'If PrimaryKeyReturnValueOverride > 0 Then PrimaryKeys.Add("1", PrimaryKeyReturnValueOverride)
                If PrimaryKeyReturnValueOverride > 0 And Not PrimaryKeys.ContainsKey("1") Then PrimaryKeys.Add("1", PrimaryKeyReturnValueOverride)

                htNew.Add("PrimaryKeyID", "ADD")                                            'this is a new record, so list "ADD" for Save_AuditLogging function

                '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                '--- Call AuditLogging function for Saving/Logging of any changed data elements.  NOTE DataTable passed and HashTable must have Data elements in the exact SAME order ---
                'Call AuditLogFunctions.Save_AuditLog(htNew, dt, PrimaryKeys, UpdateBy, CallingPageName, TableName, AccountNum, ChannelName, ServiceName)
                '------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                da.SelectCommand.Dispose()
                Return PrimaryKeys("1")

            End Using
        Catch ex As Exception
            Throw ex
        End Try

        Return PrimaryKeys("1")

    End Function

    Public Shared Function SQLUpdate(ByVal TableName As String, _
                                        ByVal PrimaryKeys As Dictionary(Of String, Integer), _
                                        ByVal UpdateBy As String, ByVal CallingPageName As String, _
                                        ByVal dtOrigValues As DataTable, _
                                        Optional ByVal boolSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal stringSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal dateSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal integerSaveParms As Dictionary(Of String, String) = Nothing, _
                                        Optional ByVal AutoSaveLastUpdatedDate As Boolean = True, _
                                        Optional ByVal AccountNum As String = "", _
                                        Optional ByVal ChannelName As String = "", _
                                        Optional ByVal ServiceName As String = "") As Boolean
        '==========================================================================================================================================================================
        '=== NOTE: UPDATEs 1 Record in Specified Table
        '==========================================================================================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim sSQL As String = String.Empty, SQLWhere As String = String.Empty
        '==========================================================================================================================================================================
        Dim pair As KeyValuePair(Of String, Integer)                                    'used in the FOR...EACH loops
        Dim htNew As New Hashtable                                                      'stores all new record values for comparison in function Save_AuditLogging
        '==========================================================================================================================================================================
        For Each pair In PrimaryKeys
            SQLWhere = SQLWhere & " AND " & pair.Key & "=" & pair.Value.ToString
        Next
        htNew.Add("PrimaryKeyID", "UPDATE")

        If AutoSaveLastUpdatedDate = True Then
            If UpdateBy.Length > 0 Then stringSaveParms.Add("LastUpdateByName", UpdateBy)
        End If

        If Not boolSaveParms Is Nothing Then sSQL = sSQL & ParameterizedSQL_UpdateString(boolSaveParms, htNew)
        If Not dateSaveParms Is Nothing Then sSQL = sSQL & ParameterizedSQL_UpdateString(dateSaveParms, htNew)
        If Not stringSaveParms Is Nothing Then sSQL = sSQL & ParameterizedSQL_UpdateString(stringSaveParms, htNew)
        If Not integerSaveParms Is Nothing Then sSQL = sSQL & ParameterizedSQL_UpdateString(integerSaveParms, htNew)
        If AutoSaveLastUpdatedDate = True Then
            sSQL = " UPDATE " & TableName & " SET LastUpdateByDate = GetDate() " & sSQL    'Create base UPDATE statement
        Else
            sSQL = " UPDATE " & TableName & " SET " & sSQL.TrimStart(",")                   'Create base UPDATE statement
        End If

        SQLWhere = " WHERE " & SQLWhere.TrimStart.Substring(3)                          'remove 1st AND clause from string

        Using mySQLConn As New SqlConnection(sSQLConnectString)
            da.SelectCommand = New SqlCommand(sSQL & SQLWhere, mySQLConn)               'Create the concatendated SQL statement with WHERE clause
            da.SelectCommand.CommandType = CommandType.Text                             'Tell command we are using a TEXT query

            '=== Add Parameters and values  (Parameters used to prevent SQL Injection Attacks) ===
            If Not boolSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(boolSaveParms, da, SqlDbType.Bit)
            If Not dateSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(dateSaveParms, da, SqlDbType.DateTime)
            If Not stringSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(stringSaveParms, da, SqlDbType.VarChar)
            If Not integerSaveParms Is Nothing Then Call AddSqlDataAdapterParameters(integerSaveParms, da, SqlDbType.Int)

            Try
                mySQLConn.Open()
                da.SelectCommand.ExecuteNonQuery()                                      ' FOR TESTING: GetSqlQueryFromSQLCommand(da.SelectCommand)

                '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                '--- Call Save_AuditLog function for any changed data elements.  NOTE DataTable passed and HashTable must have Data elements in the exact SAME order ---
                'Call AuditLogFunctions.Save_AuditLog(htNew, dtOrigValues, PrimaryKeys, UpdateBy, CallingPageName, TableName, AccountNum, ChannelName, ServiceName)
                '------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                Return True

            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return False

    End Function

    Public Shared Function SQLSelect_Paging(ByVal sSQL As String, _
                                                ByVal CurrentPage As Integer, _
                                                ByVal RecordsPerPage As Integer, _
                                                Optional ByVal SortOrder As String = "", _
                                                Optional ByVal SortField As String = "LastUpdateByDate", _
                                                Optional ByVal stringParms As Dictionary(Of String, String) = Nothing, _
                                                Optional ByVal likeParms As Dictionary(Of String, String) = Nothing, _
                                                Optional ByVal dateParms As Dictionary(Of String, String) = Nothing, _
                                                Optional ByVal integerParms As Dictionary(Of String, String) = Nothing) As DataTable
        '==========================================================================================================================================================================
        '=== NOTE: Must have a StartDate and EndDate or query could timeout
        '==========================================================================================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim SQLWhere As String = String.Empty, SQLSort As String = String.Empty
        Dim dt As New DataTable
        '==========================================================================================================================================================================

        If Not dateParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(dateParms, "BETWEEN")
        If Not stringParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(stringParms, "=")
        If Not likeParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(likeParms, "LIKE")
        If Not integerParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(integerParms, "=")

        '=== Add the function for Paging that requires the T-SQL 2005 ROW_NUMBER()
        ' sSQL = "SELECT ROW_NUMBER() OVER(ORDER BY " & SortField & " " & SortOrder & " ) as RowNum, " & sSQL.TrimStart.Substring(6)
        If Not SortField.Contains("ContactInfo") Then
            sSQL = "SELECT ROW_NUMBER() OVER(ORDER BY " & SortField & " " & SortOrder & " ) as RowNum, " & sSQL.TrimStart.Substring(6)
        Else
            sSQL = "SELECT ROW_NUMBER() OVER(ORDER BY " & "ContactName + ' [' + tblAccounts.PhoneNum + ' ' + tblAccounts.ExtNum + '] '" & " " & SortOrder & " ) as RowNum, " & sSQL.TrimStart.Substring(6)
        End If
        '

        If SQLWhere.Length Then SQLWhere = " WHERE " & SQLWhere.TrimStart.Substring(3) ' Remove the 1st "AND" in this string 

        sSQL = sSQL & SQLWhere

        '=== Next Main Query in select which filters only for Rows requested
        sSQL = "SELECT * FROM (" & sSQL & ") AS MainQuery " & _
                " WHERE (RowNum >= " & ((CurrentPage * RecordsPerPage) - RecordsPerPage + 1).ToString & _
                "    And RowNum <= " & ((CurrentPage) * RecordsPerPage).ToString & ")"
        '
        'If SortField.Length Then SQLSort = " ORDER BY " & SortField & " " & SortOrder '  Dont need Parameters for non-user input fields
        'sSQL = sSQL & SQLSort

        If SortField.Length Then
            If Not (SortField.Contains("LastUpdateByDate")) Then
                SQLSort = " ORDER BY " & SortField & " " & SortOrder '  Dont need Parameters for non-user input fields
            Else
                SortField = "LastUpdateByDate"
                SQLSort = " ORDER BY " & SortField & " " & SortOrder '  Dont need Parameters for non-user input fields
            End If
            sSQL = sSQL & SQLSort
        End If

        Using mySQLConn As New SqlConnection(sSQLConnectString)
            da.SelectCommand = New SqlCommand(sSQL, mySQLConn)                          'Create the concatendated SQL statement with WHERE and ORDER BY clauses
            da.SelectCommand.CommandType = CommandType.Text                             'Tell command we are using a TEXT query

            '=== Add Parameters and values (Parameters used to prevent SQL Injection Attacks ===
            If Not stringParms Is Nothing Then Call AddSqlDataAdapterParameters(stringParms, da, SqlDbType.VarChar)
            If Not likeParms Is Nothing Then Call AddSqlDataAdapterParameters(likeParms, da, SqlDbType.VarChar, "LIKE")
            If Not integerParms Is Nothing Then Call AddSqlDataAdapterParameters(integerParms, da, SqlDbType.Int)
            '=== dont need to do DATE parms as validation of date values was done when creating date SQL string ===

            Try
                mySQLConn.Open()
                da.Fill(dt)                                         ' FOR TESTING: GetSqlQueryFromSQLCommand(da.SelectCommand)

            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return dt

    End Function

    Public Shared Function SQLSelect_Parms(ByVal sSQL As String, _
                                                 Optional ByVal SortOrder As String = "", _
                                                 Optional ByVal SortField As String = "", _
                                                 Optional ByVal stringParms As Dictionary(Of String, String) = Nothing, _
                                                 Optional ByVal likeParms As Dictionary(Of String, String) = Nothing, _
                                                 Optional ByVal dateParms As Dictionary(Of String, String) = Nothing, _
                                                 Optional ByVal integerParms As Dictionary(Of String, String) = Nothing) As DataTable
        '==========================================================================================================================================================================
        '=== NOTE: Must have a StartDate and EndDate or query could timeout
        '==========================================================================================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim SQLWhere As String = String.Empty, SQLSort As String = String.Empty
        Dim dt As New DataTable
        '==========================================================================================================================================================================

        If Not dateParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(dateParms, "BETWEEN")
        If Not stringParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(stringParms, "=")
        If Not likeParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(likeParms, "LIKE")
        If Not integerParms Is Nothing Then SQLWhere = SQLWhere & ParameterizedSQL_WhereString(integerParms, "=")

        If SQLWhere.Length Then SQLWhere = " WHERE " & SQLWhere.TrimStart.Substring(3) ' Remove the 1st "AND" in this string 
        If SortField.Length Then SQLSort = " ORDER BY " & SortField & " " & SortOrder '  Dont need Parameters for non-user input fields

        Using mySQLConn As New SqlConnection(sSQLConnectString)
            da.SelectCommand = New SqlCommand(sSQL & SQLWhere & SQLSort, mySQLConn)     'Create the concatendated SQL statement with WHERE and ORDER BY clauses
            da.SelectCommand.CommandType = CommandType.Text                             'Tell command we are using a TEXT query

            '=== Add Parameters and values (Parameters used to prevent SQL Injection Attacks ===
            If Not stringParms Is Nothing Then Call AddSqlDataAdapterParameters(stringParms, da, SqlDbType.VarChar)
            If Not likeParms Is Nothing Then Call AddSqlDataAdapterParameters(likeParms, da, SqlDbType.VarChar, "LIKE")
            If Not integerParms Is Nothing Then Call AddSqlDataAdapterParameters(integerParms, da, SqlDbType.Int)
            '=== dont need to do DATE parms as validation of date values was done when creating date SQL string ===

            Try
                mySQLConn.Open()
                da.Fill(dt)                                         ' FOR TESTING: GetSqlQueryFromSQLCommand(da.SelectCommand)

            Catch ex As Exception
                Throw ex
            End Try
        End Using

        Return dt

    End Function

    Public Shared Function SQLDelete(ByVal TableName As String, _
                                        ByVal PrimaryKeys As Dictionary(Of String, Integer), _
                                        ByVal UpdateBy As String, ByVal CallingPageName As String, _
                                        ByVal dtOrigValues As DataTable, _
                                        Optional ByVal AccountNum As String = "", _
                                        Optional ByVal ChannelName As String = "", _
                                        Optional ByVal ServiceName As String = "") As Boolean
        '==========================================================================================================================================================================
        Dim da As SqlDataAdapter = New SqlDataAdapter
        Dim sSQL As String = String.Empty, SQLWhere As String = String.Empty
        Dim htNew As New Hashtable
        'Dim integerParms As Dictionary(Of String, String) = Nothing
        '==========================================================================================================================================================================
        If PrimaryKeys.Count = 0 Then                                                   'requires 1 WHERE clause, else will delete the entire table contents
            Return False                                                                'delete fails with no WHERE clause
        End If

        Try
            If htNew.Count = 0 Then                                                         'For deletes add orginal values to HastTable
                htNew.Add("PrimaryKeyID", "DELETE")
                For i As Integer = 0 To dtOrigValues.Columns.Count - 1                      'IMPORTANT: dtOrigValues should be in same order as in table, with ONLY delete table fields
                    htNew.Add(dtOrigValues.Columns(i).ToString, dtOrigValues.Rows.Item(0)(i).ToString)
                Next
            End If

            For Each pair In PrimaryKeys
                SQLWhere = SQLWhere & " AND " & pair.Key & "=" & pair.Value.ToString
            Next
            SQLWhere = " WHERE " & SQLWhere.TrimStart.Substring(3)                          'remove 1st AND clause from string

            'Create base DELETE statement
            sSQL = " DELETE FROM " & TableName & SQLWhere

            Using mySQLConn As New SqlConnection(sSQLConnectString)
                da.SelectCommand = New SqlCommand(sSQL, mySQLConn)                          'Create the concatendated SQL statement 
                da.SelectCommand.CommandType = CommandType.Text                             'Tell command we are using a TEXT query

                'If Not integerParms Is Nothing Then Call AddSqlDataAdapterParameters(integerParms, da, SqlDbType.Int)

                Try
                    mySQLConn.Open()
                    da.SelectCommand.ExecuteNonQuery()                                      ' FOR TESTING: GetSqlQueryFromSQLCommand(da.SelectCommand)

                    '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    '--- Call AuditLogging function for Saving/Logging of any changed data elements.  NOTE DataTable passed and HashTable must have Data elements in the exact SAME order ---
                    'Call AuditLogFunctions.Save_AuditLog(htNew, dtOrigValues, PrimaryKeys, UpdateBy, CallingPageName, TableName, AccountNum, ChannelName, ServiceName)
                    '------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                    Return True

                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function GetSqlQueryFromSQLCommand(ByVal command As SqlCommand) As String
        '==========================================================================================================================================================================
        '==========================================================================================================================================================================
        Dim commandText As String = String.Empty

        Try
            commandText = command.CommandText
            If command.CommandType = System.Data.CommandType.Text Then
                For Each param As SqlParameter In command.Parameters
                    Dim replacement As String

                    Select Case param.DbType
                        Case System.Data.DbType.[Boolean]
                            replacement = If(Convert.ToBoolean(param.Value) = True, "1", "0")
                            Exit Select
                        Case System.Data.DbType.[String]
                            replacement = "'" & param.Value & "'"
                            Exit Select
                        Case Else
                            replacement = param.Value.ToString()
                            Exit Select
                    End Select
                    commandText = commandText.Replace(param.ParameterName, replacement)
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return commandText
    End Function

    Private Shared Function ParameterizedSQL_InsertString(ByVal dicParms As Dictionary(Of String, String), ByRef ht As Hashtable, ByRef InsertFieldNames As String) As String
        '==========================================================================================================================================================================
        '=== NOTE: All WHERE clause conditions are AND'ed together.  OR conditions are not allowed.
        '=== Returns 2 strings (VALUES via return, field names byRef to variable in calling function) & New Value Hashtable by Reference
        '==========================================================================================================================================================================
        If dicParms Is Nothing Then Return ""
        '==========================================================================================================================================================================
        Dim sSQL As String = String.Empty                                   'holds the SQL VALUE statement to be returned (without the keyword VALUE)
        Dim pair As KeyValuePair(Of String, String)                         'used in the FOR...EACH loops
        Dim i As Integer = Nothing, sFieldNames() As String
        '==========================================================================================================================================================================
        Try
            For Each pair In dicParms
                sFieldNames = pair.Key.Split(New Char() {"."c})                 'splits if field names is fully qualified (ex: tableName.fieldName)
                i = sFieldNames.Length - 1                                       'value for field name in array

                InsertFieldNames = InsertFieldNames & ", " & pair.Key           'string for the INSERT INTO field names
                sSQL = sSQL & ", @" & sFieldNames(i)                            'string parameters for the VALUES
                ht.Add(sFieldNames(i), pair.Value)
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return sSQL                                                         'Return the SQL Where clause (without the WHERE keyword)

    End Function

    Private Shared Function ParameterizedSQL_UpdateString(ByVal dicParms As Dictionary(Of String, String), ByRef ht As Hashtable) As String
        '==========================================================================================================================================================================
        '=== NOTE: All WHERE clause conditions are AND'ed together.  OR conditions are not allowed.
        '==========================================================================================================================================================================
        If dicParms Is Nothing Then Return ""
        '==========================================================================================================================================================================
        Dim sSQL As String = String.Empty                                   'holds the SQL SET statements to be returned (without the keyword SET)
        Dim pair As KeyValuePair(Of String, String)                         'used in the FOR...EACH loops
        Dim i As Integer = Nothing, sFieldNames() As String
        '==========================================================================================================================================================================
        Try
            For Each pair In dicParms
                sFieldNames = pair.Key.Split(New Char() {"."c})                'splits if field names is fully qualified (ex: tableName.fieldName)
                i = sFieldNames.Length - 1                                      'value for field name in array

                sSQL = sSQL & ", " & pair.Key & "=@" & sFieldNames(i)
                ht.Add(sFieldNames(i), pair.Value)
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return sSQL                                                         'Return the SQL Where clause (without the WHERE keyword)

    End Function

    Public Shared Sub AddSqlDataAdapterParameters(ByVal dicParms As Dictionary(Of String, String), ByRef daSQL As SqlDataAdapter, Optional ByVal FieldType As SqlDbType = SqlDbType.VarChar, Optional ByVal Type As String = "")
        '==========================================================================================================================================================================
        '=== Adds to Parameters collection of passed "byRef" SqlDataAdapter object.
        '=== NOTE: Do not pass DateTime data types in for parameters
        '==========================================================================================================================================================================
        If dicParms Is Nothing Then Exit Sub
        '==========================================================================================================================================================================
        Dim pair As KeyValuePair(Of String, String)
        Dim i As Integer = Nothing, sFieldNames() As String
        '==========================================================================================================================================================================

        Try
            For Each pair In dicParms                                               '=== for Equals "=" or the LIKE Operands ===
                sFieldNames = pair.Key.Split(New Char() {"."c})                     'splits if field names is fully qualified (ex: tableName.fieldName)
                i = sFieldNames.Length - 1                                           'value for field name in array
                Select Case FieldType
                    Case SqlDbType.SmallDateTime, SqlDbType.DateTime
                        daSQL.SelectCommand.Parameters.AddWithValue("@" & sFieldNames(i), FieldType).Value = CDate(pair.Value)
                    Case SqlDbType.SmallInt, SqlDbType.Int, SqlDbType.TinyInt, SqlDbType.Decimal, SqlDbType.Float, SqlDbType.BigInt
                        daSQL.SelectCommand.Parameters.AddWithValue("@" & sFieldNames(i), FieldType).Value = IIf(pair.Value = Nothing, DBNull.Value, Val(pair.Value))  'use VAL command for superior performance
                    Case SqlDbType.Bit
                        daSQL.SelectCommand.Parameters.AddWithValue("@" & sFieldNames(i), FieldType).Value = IIf(pair.Value = Nothing, DBNull.Value, Val(pair.Value))
                    Case SqlDbType.VarChar, SqlDbType.Text
                        If Type = "LIKE" Then
                            daSQL.SelectCommand.Parameters.AddWithValue("@" & sFieldNames(i), FieldType).Value = "%" & pair.Value & "%" 'Add the percect % to start and end of string
                        Else
                            daSQL.SelectCommand.Parameters.AddWithValue("@" & sFieldNames(i), FieldType).Value = IIf(pair.Value = Nothing, DBNull.Value, pair.Value)
                        End If
                    Case Else
                        daSQL.SelectCommand.Parameters.AddWithValue("@" & sFieldNames(i), FieldType).Value = IIf(pair.Value = Nothing, DBNull.Value, pair.Value)
                End Select
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function ParameterizedSQL_WhereString(ByVal dicParms As Dictionary(Of String, String), Optional ByVal Operand As String = "=") As String
        '==========================================================================================================================================================================
        '=== NOTE: All WHERE clause conditions are AND'ed together.  OR conditions are not allowed.
        '==========================================================================================================================================================================
        If dicParms Is Nothing Then Return ""
        '==========================================================================================================================================================================
        Dim sSQL As String = String.Empty                                   'Stores the SQL WHERE statement to be returned (without the keyword WHERE)
        Dim pair As KeyValuePair(Of String, String)                         'used in the FOR...EACH loops
        Dim dtStart As Date = Nothing, dtEnd As Date = Nothing              'used to verify valid dates passed in 
        Dim sFieldNames() As String, i As Integer = 0
        '==========================================================================================================================================================================
        Try
            Select Case Operand
                Case "BETWEEN"
                    Try
                        For Each pair In dicParms
                            If dtStart = Nothing Then                           'IF dtStart is NOTHING then assume this is the 1st of 2 date values in dictionary
                                dtStart = CDate(pair.Value)                     'test for valid date values, and to have date in appropriate format for SQL query
                                sSQL = sSQL & "  AND (" & pair.Key & " BETWEEN CONVERT(DATETIME, '" & dtStart.ToShortDateString & " 00:00:00', 102) "
                            Else
                                dtEnd = CDate(pair.Value)                       'test for valid date values, and to have date in appropriate format for SQL query
                                dtStart = Nothing                               'Must reset dtStart for next possible dictionary pair of start end dates
                                sSQL = sSQL & " AND CONVERT(DATETIME, '" & dtEnd.ToShortDateString & " 11:59:59', 102)) "
                            End If
                        Next
                    Catch ex As Exception                                           'invalid date value found; 
                        sSQL = ""                                                   'reset sSQL string to remove possible invalid SQL Statement
                    End Try

                Case Else
                    For Each pair In dicParms
                        sFieldNames = pair.Key.Split(New Char() {"."c})             'splits if field names is fully qualified (ex: tableName.fieldName)
                        i = sFieldNames.Length - 1                                   'value for field name in array
                        If pair.Value.Length Then                                   'Check that there is a value to add
                            sSQL = sSQL & " AND " & pair.Key & " " & Operand & " @" & sFieldNames(i)
                        End If
                    Next
            End Select

        Catch ex As Exception
            Throw ex
        End Try

        Return sSQL                                                         'Return the SQL Where clause (without the WHERE keyword)

    End Function

    'Public Class AuditLogFunctions
    '    '||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

    '    Public Shared Sub Save_AuditLog(ByVal htNew As Hashtable, ByVal dtOld As DataTable, ByVal PrimaryKeys As Dictionary(Of String, Integer), _
    '                                        ByVal LastChangedBy As String, ByVal UpdatedFromPageName As String, ByVal TableName As String, _
    '                                        ByVal AccountNum As String, ByVal ChannelName As String, ByVal ServiceName As String)
    '        '============================================================================================================================================================================================================================================
    '        '=== If the Key item "PrimaryKeyID" is the Integer value, then iterate thru all items
    '        '--- htNew must contain the Key Value "PrimaryKeyID" and all of the column names that could be recorded in a change
    '        '--- dtOld must contain at least all of the columns in htNew (for DELETE only)
    '        '============================================================================================================================================================================================================================================
    '        Try
    '            Dim PrimaryKeyID_Value As String = htNew.Item("PrimaryKeyID").ToString.ToUpper
    '            htNew.Remove("PrimaryKeyID")                                'remove as not an key/value to be saved for Audit Tracking

    '            Select Case PrimaryKeyID_Value
    '                Case "ADD"
    '                    'NO LONGER USED: PrimaryKeyID_Value = htNew.Item("PrimaryKeyIDValue").ToString.ToUpper
    '                    'NO LONGER USED: htNew.Remove("PrimaryKeyIDValue")                   'remove as not an key/value to be saved for Audit Tracking
    '                    For Each htEntry As DictionaryEntry In htNew
    '                        WriteAuditChanges("ADD", htEntry.Value, PrimaryKeys, UpdatedFromPageName, htEntry.Key, LastChangedBy, TableName, AccountNum, ChannelName, ServiceName)
    '                    Next
    '                Case "DELETE"
    '                    For Each htEntry As DictionaryEntry In htNew
    '                        WriteAuditChanges(dtOld.Rows(0)(htEntry.Key).ToString, "DELETE", PrimaryKeys, UpdatedFromPageName, htEntry.Key, LastChangedBy, TableName, AccountNum, ChannelName, ServiceName)
    '                    Next
    '                Case Else
    '                    For Each htEntry As DictionaryEntry In htNew
    '                        If dtOld.Rows(0)(htEntry.Key).ToString <> htEntry.Value Then        'If new and old data are DIFFERENT, write the change
    '                            If IsDate(htEntry.Value) Then                                   'Dates are hard to compare, so check if value could be a date to verify TIME Stamp on date value
    '                                If dtOld.Rows(0)(htEntry.Key).ToString <> CDate(htEntry.Value).ToString Then 'Again check if new and old DATE data are DIFFERENT, write the change
    '                                    WriteAuditChanges(dtOld.Rows(0)(htEntry.Key).ToString, CDate(htEntry.Value).ToString, PrimaryKeys, UpdatedFromPageName, TableName & "." & htEntry.Key, LastChangedBy, TableName, AccountNum, ChannelName, ServiceName)
    '                                End If
    '                            Else
    '                                WriteAuditChanges(dtOld.Rows(0)(htEntry.Key).ToString, htEntry.Value, PrimaryKeys, UpdatedFromPageName, htEntry.Key, LastChangedBy, TableName, AccountNum, ChannelName, ServiceName)
    '                            End If
    '                        End If
    '                    Next htEntry
    '            End Select
    '        Catch ex As Exception
    '            Throw ex
    '        End Try

    '    End Sub

    '    Public Shared Sub WriteAuditChanges(ByVal OldValue As String, ByVal NewValue As String, ByVal PrimaryKeys As Dictionary(Of String, Integer), _
    '                                        ByVal PageName As String, ByVal FieldName As String, ByVal ChangedBy As String, _
    '                                        ByVal TableName As String, ByVal AccountNum As String, ByVal ChannelName As String, ByVal ServiceName As String)
    '        '============================================================================================================================================================================
    '        '=== Write Changed record to audit log table
    '        '============================================================================================================================================================================
    '        Dim da As SqlDataAdapter = New SqlDataAdapter
    '        Try
    '            Using mySQLConn As New SqlConnection(sSQLConnectString)
    '                da.SelectCommand = New SqlCommand("SAVE_AUDIT_LOG_SP", mySQLConn)
    '                da.SelectCommand.CommandType = CommandType.StoredProcedure
    '                da.SelectCommand.Parameters.AddWithValue("@SOURCE_PRIMARY_KEY_ID", SqlDbType.Int).Value = PrimaryKeys.Values(0).ToString
    '                da.SelectCommand.Parameters.AddWithValue("@SOURCE_PRIMARY_KEY_2_ID", SqlDbType.Int).Value = IIf(PrimaryKeys.Keys(1) = Nothing, DBNull.Value, PrimaryKeys.Values(1).ToString)
    '                da.SelectCommand.Parameters.AddWithValue("@SOURCE_PRIMARY_KEY_3_ID", SqlDbType.Int).Value = IIf(PrimaryKeys.Keys(2) = Nothing, DBNull.Value, PrimaryKeys.Values(2).ToString)
    '                da.SelectCommand.Parameters.AddWithValue("@CHANGE_TABLE_NM", SqlDbType.VarChar).Value = TableName
    '                da.SelectCommand.Parameters.AddWithValue("@CHANGE_PAGE_NM", SqlDbType.VarChar).Value = PageName
    '                da.SelectCommand.Parameters.AddWithValue("@CHANGE_FIELD_NM", SqlDbType.VarChar).Value = FieldName
    '                da.SelectCommand.Parameters.AddWithValue("@ACCOUNT_NUM", SqlDbType.VarChar).Value = AccountNum
    '                da.SelectCommand.Parameters.AddWithValue("@CHANGE_OLD_VALUE_TXT", SqlDbType.VarChar).Value = IIf(OldValue Is Nothing, "", (IIf(OldValue.Length >= 3000, Mid(OldValue, 1, 3000), OldValue)))
    '                da.SelectCommand.Parameters.AddWithValue("@CHANGE_NEW_VALUE_TXT", SqlDbType.VarChar).Value = IIf(NewValue Is Nothing, "", (IIf(NewValue.Length >= 3000, Mid(NewValue, 1, 3000), NewValue)))
    '                da.SelectCommand.Parameters.AddWithValue("@CHANNEL_NM", SqlDbType.VarChar).Value = ChannelName
    '                da.SelectCommand.Parameters.AddWithValue("@SERVICE_NM", SqlDbType.VarChar).Value = ServiceName

    '                da.SelectCommand.Parameters.AddWithValue("@OLD_VALUE_NM", SqlDbType.VarChar).Value = DBNull.Value
    '                da.SelectCommand.Parameters.AddWithValue("@NEW_VALUE_NM", SqlDbType.VarChar).Value = DBNull.Value

    '                da.SelectCommand.Parameters.AddWithValue("@LastUpdateByName", SqlDbType.VarChar).Value = ChangedBy
    '                Try
    '                    mySQLConn.Open()
    '                    da.SelectCommand.ExecuteNonQuery()
    '                Catch ex As Exception
    '                    Throw ex
    '                End Try
    '            End Using
    '        Catch ex As Exception
    '            Throw ex
    '        End Try

    '    End Sub

End Class
