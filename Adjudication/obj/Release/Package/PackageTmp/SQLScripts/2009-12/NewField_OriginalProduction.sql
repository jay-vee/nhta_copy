-- ADD MISSING AdjudicatorAttendanceComment COMMENTS!
UPDATE SCORING SET AdjudicatorAttendanceComment = misComm.Comment
FROM Scoring INNER JOIN 
	(select FK_ScoringID, CommentFieldName, Comment
	from Scoring_Temp_Entry 
	WHERE CommentFieldName = 'AdjudicatorAttendanceComment' AND Comment IS NOT NULL)
		misComm ON misComm.FK_ScoringID = SCORING.PK_ScoringID
GO
-----------------------------------------------------------------------------------------------------------------------

BEGIN TRANSACTION
	ALTER TABLE dbo.Production ADD
		OriginalProduction tinyint NULL
	GO
	
	ALTER TABLE dbo.Production ADD CONSTRAINT
		DF_Production_OriginalProduction DEFAULT 0 FOR OriginalProduction
	GO
	
COMMIT TRANSACTION

BEGIN TRANSACTION
	UPDATE Production SET OriginalProduction=1 WHERE FK_ProductionTYpeID=4 OR FK_ProductionTYpeID=5
	GO
	UPDATE Production SET OriginalProduction=0 WHERE OriginalProduction IS NULL
	GO
	
	UPDATE Production SET FK_ProductionTYpeID=2 WHERE FK_ProductionTYpeID=3
	GO

	UPDATE Production SET FK_ProductionTYpeID=2 WHERE FK_ProductionTYpeID=4
	GO

	UPDATE Production SET FK_ProductionTYpeID=1 WHERE FK_ProductionTYpeID=5
	GO

	DELETE FROM ProductionType WHERE PK_ProductionTypeID=3 OR PK_ProductionTypeID=4 OR PK_ProductionTypeID=5
	GO

COMMIT TRANSACTION 

GO

-----------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[Save_Production]
						(	@PK_ProductionID int, 
							@FK_CompanyID int, 
							@FK_VenueID int, 
							@FK_AgeApproriateID int, 
							@FK_ProductionTypeID int, 
							@Title varchar(100), 
							@Authors varchar(200), 
							@LicensingCompany varchar(100), 
							@FirstPerformanceDateTime smalldatetime, 
							@LastPerformanceDateTime smalldatetime, 
							@AllPerformanceDatesTimes varchar(8000), 
							@TicketContactName varchar(50), 
							@TicketContactPhone varchar(50), 
							@TicketContactEmail varchar(100), 
							@TicketPurchaseDetails varchar(8000), 
							@Comments varchar (8000), 
							@RequiresAdjudication tinyint, 
							@OriginalProduction tinyint, 
							@LastUpdateByName varchar (50) )
AS 
/*	Object: Save_Production 
	Description:     Saves (Insert or Update) Production data 
	Returns:         errorvalue
	Created By:      Joe Vago 
	Example:         EXEC Save_Production.... 
	Created Date:    6/7/07 
	Modified By:	Joe Vago 
	Modified Date:	12/20/2009
	NOTES:			Added new field 'OriginalProduction'
*/ 
SET NOCOUNT ON 
DECLARE @NewProductionID as INT  

IF (@PK_ProductionID > 0) 
	BEGIN 
		UPDATE Production 
			SET FK_CompanyID= @FK_CompanyID,
				FK_VenueID= @FK_VenueID,
				FK_AgeApproriateID= @FK_AgeApproriateID,
				FK_ProductionTypeID= @FK_ProductionTypeID,
				Title= @Title,
				Authors= @Authors,
				LicensingCompany= @LicensingCompany,
				FirstPerformanceDateTime= @FirstPerformanceDateTime,
				LastPerformanceDateTime= @LastPerformanceDateTime,
				AllPerformanceDatesTimes= @AllPerformanceDatesTimes,
				TicketContactName= @TicketContactName,
				TicketContactPhone= @TicketContactPhone,
				TicketContactEmail= @TicketContactEmail,
				TicketPurchaseDetails= @TicketPurchaseDetails,
				Comments= @Comments,
				RequiresAdjudication= @RequiresAdjudication,
				OriginalProduction= @OriginalProduction, 
				LastUpdateByName= @LastUpdateByName,
				LastUpdateByDate= GetDate() 
		WHERE PK_ProductionID= @PK_ProductionID 
	END 
ELSE 
	BEGIN 
		INSERT INTO Production( FK_CompanyID,
			FK_VenueID,
			FK_AgeApproriateID,
			FK_ProductionTypeID,
			Title,
			Authors,
			LicensingCompany,
			FirstPerformanceDateTime,
			LastPerformanceDateTime,
			AllPerformanceDatesTimes,
			TicketContactName,
			TicketContactPhone,
			TicketContactEmail,
			TicketPurchaseDetails,
			Comments,
			RequiresAdjudication,
			OriginalProduction,
			LastUpdateByName,
			CreateByName ) 
	VALUES( @FK_CompanyID,
			@FK_VenueID,
			@FK_AgeApproriateID,
			@FK_ProductionTypeID,
			@Title,
			@Authors,
			@LicensingCompany,
			@FirstPerformanceDateTime,
			@LastPerformanceDateTime,
			@AllPerformanceDatesTimes,
			@TicketContactName,
			@TicketContactPhone,
			@TicketContactEmail,
			@TicketPurchaseDetails,
			@Comments,
			@RequiresAdjudication,
			@OriginalProduction,
			@LastUpdateByName,
			@LastUpdateByName )  

	IF @RequiresAdjudication = 1 
		BEGIN 
			SET @NewProductionID = @@IDENTITY 
			/* Create a Dummy Nomination */ 
			INSERT INTO Nominations( 
				FK_ProductionID,
				Director,
				LastUpdateByName,
				CreateByName    ) 
			VALUES( @NewProductionID,
				'To Be Announced',
				@LastUpdateByName,
				@LastUpdateByName  ) 
		END 
	END  

GO


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE [dbo].[ReportScoring_OriginalPlaywright]
	(
		@WithAdjudicatorScores INT = NULL ,
		@CompanyTypeID INT = NULL
	) 
AS
/* =============================================================================== 
Object:  		ReportScoring_OriginalPlaywright
Description:  	
Returns:  		Result set, in Order of Highest Average Score to Lowest
Created By:  	Joe L. Vago
Created Date:  	12/18/08
Modified By:  	
Modified Date:	
NOTES:			If no value for @ActorActress, Actor will be default
=============================================================================== 
DECLARE @CompanyTypeID as INT, @WithAdjudicatorScores as INT
--SET @CompanyTypeID = 0
SET @WithAdjudicatorScores = 1
DROP TABLE #tmp_ScoreSummaryCalc_1
=============================================================================== */
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Get value for # of Adjudicator per show from ApplicationDefaults table
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DECLARE @NumAdjPerShow as INT
SET @NumAdjPerShow = (SELECT NumAdjudicatorsPerShow FROM ApplicationDefaults)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Do Calculations for Nominee
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT 		Nominations.PK_NominationsID, Nominations.FK_ProductionID, Production.FK_ProductionTypeID, Company.FK_CompanyTypeID, Company.CompanyName, 
			Nominations.OriginalPlaywright AS BestName, 'Original Playwright' AS BestRole, 
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreMaxMin.MaxScore
				ELSE ScoreMaxMin.MaxScore
			END AS MaxScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreMaxMin.MinScore
				ELSE ScoreMaxMin.MinScore
			END AS MinScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreTotal.TotalScore
				ELSE ScoreTotal.TotalScore
			END AS TotalScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreTotal.TotalScore / NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				ELSE ScoreTotal.TotalScore / NumAdj.NumOfAdjudicatorsWithCompletedBallot
			END AS AvgScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				ELSE NumAdj.NumOfAdjudicatorsWithCompletedBallot
			END AS NumOfAdjudicatorsForProduction,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN 'Yes'
				ELSE 'No'
			END AS UsingReserveAdjudicatorScore,
			NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
INTO #tmp_ScoreSummaryCalc_1
	FROM Production 
		INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID 
		INNER JOIN Nominations 
		INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.OriginalPlaywrightScore > 0 GROUP BY FK_NominationsID) 
			NumAdj ON NumAdj.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE Scoring.OriginalPlaywrightScore > 0 GROUP BY FK_NominationsID) 
			NumAdjWithReserve ON NumAdjWithReserve.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.OriginalPlaywrightScore) as MaxScore, MIN(Scoring.OriginalPlaywrightScore) as MinScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.OriginalPlaywrightScore > 0 GROUP BY FK_NominationsID) 
			ScoreMaxMin ON ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.OriginalPlaywrightScore) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.OriginalPlaywrightScore > 0 GROUP BY FK_NominationsID) 
			ScoreTotal ON ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.OriginalPlaywrightScore) as MaxScore, MIN(Scoring.OriginalPlaywrightScore) as MinScore, FK_NominationsID FROM Scoring WHERE Scoring.OriginalPlaywrightScore > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreMaxMin ON WithReserve_ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.OriginalPlaywrightScore) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE Scoring.OriginalPlaywrightScore > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreTotal ON WithReserve_ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
	WHERE   Scoring.OriginalPlaywrightScore > 0
			AND Production.OriginalProduction = 1 
			AND Company.FK_CompanyTypeID BETWEEN COALESCE(@CompanyTypeID, 1) AND COALESCE(@CompanyTypeID, POWER( 2., 31 ) - 1) 
	GROUP BY	Nominations.PK_NominationsID, Nominations.FK_ProductionID, Production.FK_ProductionTypeID, Company.FK_CompanyTypeID, Company.CompanyName, 
				Nominations.OriginalPlaywright, NumAdj.NumOfAdjudicatorsWithCompletedBallot, NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				,ScoreTotal.TotalScore,	ScoreMaxMin.MaxScore, ScoreMaxMin.MinScore ,WithReserve_ScoreTotal.TotalScore, WithReserve_ScoreMaxMin.MaxScore, WithReserve_ScoreMaxMin.MinScore
	HAVING      (LEN(Nominations.OriginalPlaywright) > 0) --AND (LEN('Original Playwright') > 0)


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Return Results in Detail (per Adjudicator) or Summary (summed for Nominee)
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
IF @WithAdjudicatorScores = 1 
	BEGIN
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Return Results in Detail (per Adjudicator)  --
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		SELECT		Scoring.PK_ScoringID, Nominations.PK_NominationsID, 
					Production.PK_ProductionID, Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,    
					ProductionType.PK_ProductionTypeID, ProductionType.ProductionType,  
					Company.CompanyName, CompanyTypes.PK_CompanyTypeID, CompanyTypes.CompanyType,  
					Users.LastName + ', ' + Users.FirstName AS AdjudicatorName, CompanyRepresented.CompanyName AS AdjudicatorCompanyRepresented,  
					Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual, Scoring.ReserveAdjudicator,
					Scoring.OriginalPlaywrightScore as AdjudicatorScore,
					AvgScore1.BestName, AvgScore1.BestRole,
					AvgScore1.NumOfAdjudicatorsForProduction, UsingReserveAdjudicatorScore, AvgScore1.NumOfAdjudicatorsWithCompletedBallot, 
					AvgScore1.MaxScore, AvgScore1.MinScore ,AvgScore1.TotalScore, AvgScore1.AvgScore,
					CASE 
						WHEN (NumOfAdjudicatorsForProduction = 4)
							THEN ((TotalScore - MaxScore - MinScore) +  ((MaxScore + MinScore) /2) )
						WHEN NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1)
							THEN (TotalScore - MaxScore - MinScore) 
						ELSE TotalScore 
					END as TotalScoreFinal,
					CASE 
						WHEN (NumOfAdjudicatorsForProduction = 4)
							THEN ((TotalScore - MaxScore - MinScore) + ((MaxScore + MinScore) /2)) /3
						WHEN (NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1))
							THEN (TotalScore - MaxScore - MinScore) / (NumOfAdjudicatorsForProduction - 2)
						ELSE TotalScore / NumOfAdjudicatorsForProduction
					END as AverageScoreFinal
			FROM Production 
				INNER JOIN Company 
				INNER JOIN CompanyTypes ON Company.FK_CompanyTypeID = CompanyTypes.PK_CompanyTypeID ON Production.FK_CompanyID = Company.PK_CompanyID 
				INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID 
				INNER JOIN Nominations 
				INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID 
				INNER JOIN Company CompanyRepresented ON Scoring.FK_CompanyID_Adjudicator = CompanyRepresented.PK_CompanyID 
				LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID 
				INNER JOIN 
					(SELECT PK_NominationsID, FK_ProductionTypeID, FK_CompanyTypeID, BestName, BestRole,
							NumOfAdjudicatorsForProduction, UsingReserveAdjudicatorScore, NumOfAdjudicatorsWithCompletedBallot, 
							MaxScore, MinScore ,TotalScore, AvgScore
						FROM #tmp_ScoreSummaryCalc_1
					) AvgScore1	ON AvgScore1.PK_NominationsID = Scoring.FK_NominationsID  
			WHERE Production.OriginalProduction = 1 
			ORDER BY PK_CompanyTypeID, AverageScoreFinal DESC, PK_ScoringID
	END
ELSE
	BEGIN
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Return Results as Summary (summed for Nominee) --
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		SELECT Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, 
				ProductionType.ProductionType, CompanyTypes.CompanyType,
				#tmp_ScoreSummaryCalc_1.*,
				CASE 
					WHEN (NumOfAdjudicatorsForProduction = 4)
						THEN ((TotalScore - MaxScore - MinScore) +  ((MaxScore + MinScore) /2) )
					WHEN NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1)
						THEN (TotalScore - MaxScore - MinScore) 
					ELSE TotalScore 
				END as TotalScoreFinal,
				CASE 
					WHEN (NumOfAdjudicatorsForProduction = 4)
						THEN ((TotalScore - MaxScore - MinScore) + ((MaxScore + MinScore) /2)) /3
					WHEN (NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1))
						THEN (TotalScore - MaxScore - MinScore) / (NumOfAdjudicatorsForProduction - 2)
					ELSE TotalScore / NumOfAdjudicatorsForProduction
				END as AverageScoreFinal
			 FROM #tmp_ScoreSummaryCalc_1
				INNER JOIN Production ON Production.PK_ProductionID = #tmp_ScoreSummaryCalc_1.FK_ProductionID
				INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID 
				INNER JOIN CompanyTypes ON #tmp_ScoreSummaryCalc_1.FK_CompanyTypeID = CompanyTypes.PK_CompanyTypeID 
			WHERE Production.OriginalProduction = 1 
			ORDER BY FK_CompanyTypeID, AverageScoreFinal DESC	
	END

