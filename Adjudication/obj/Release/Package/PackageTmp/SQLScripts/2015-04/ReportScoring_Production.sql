ALTER PROCEDURE [dbo].[ReportScoring_Production]
	(
		@WithAdjudicatorScores INT = NULL ,
		@ProductionCategoryID INT = NULL,
		@ProductionTypeID INT = NULL
	) 
AS
/* =============================================================================== 
Object:  		ReportScoring_Production
Description:  	
Returns:  		Result set, in Order of Highest Average Score to Lowest
Created By:  	Joe L. Vago
Created Date:  	12/15/09
Modified By:  	Joe L. Vago
Modified Date:	1/2/2011
NOTES:			1/2/2011: Fix for including "Youth" with "Community" Production type
Modified By:    Justin Voshell
Modified Date:  4/21/2015
NOTES:          4/21/2015: Updated to swap ProductionCategory for CompanyTypes
=============================================================================== 

DECLARE @ProductionCategoryID as INT, @ProductionTypeID as INT, @WithAdjudicatorScores as INT
--SET @ProductionCategoryID = 0
SET @WithAdjudicatorScores = 1
DROP TABLE #tmp_ScoreSummaryCalc_1
=============================================================================== */
SET NOCOUNT ON

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Get value for # of Adjudicator per show from ApplicationDefaults table
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DECLARE @NumAdjPerShow as INT
SET @NumAdjPerShow = (SELECT NumAdjudicatorsPerShow FROM ApplicationDefaults) - 1

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Do Calculations for Nominee
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT 		Nominations.PK_NominationsID, Nominations.FK_ProductionID, Production.FK_ProductionCategoryID, Company.CompanyName, 
			CASE WHEN Production.FK_ProductionCategoryID= 3 THEN 3 ELSE Production.FK_ProductionTypeID END AS FK_ProductionTypeID, 
			'Production' AS BestName, 'Production' AS BestRole, 'Production' as CategoryName,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreMaxMin.MaxScore
				ELSE ScoreMaxMin.MaxScore
			END AS MaxScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreMaxMin.MinScore
				ELSE ScoreMaxMin.MinScore
			END AS MinScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreTotal.TotalScore
				ELSE ScoreTotal.TotalScore
			END AS TotalScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < 3 THEN 0
				 WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN WithReserve_ScoreTotal.TotalScore / NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot			 
				ELSE ScoreTotal.TotalScore / NumAdj.NumOfAdjudicatorsWithCompletedBallot
			END AS AvgScore,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				ELSE NumAdj.NumOfAdjudicatorsWithCompletedBallot
			END AS NumOfAdjudicatorsForProduction,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				ELSE NumAdj.NumOfAdjudicatorsWithCompletedBallot
			END AS NumOfUsableBallots,
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN 'Yes'
				ELSE 'No'
			END AS UsingReserveAdjudicatorScore,
			NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
INTO #tmp_ScoreSummaryCalc_1
	FROM Production 
		INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID 
		INNER JOIN Nominations 
		INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestProductionScore > 0 GROUP BY FK_NominationsID) 
			NumAdj ON NumAdj.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE Scoring.BestProductionScore > 0 GROUP BY FK_NominationsID) 
			NumAdjWithReserve ON NumAdjWithReserve.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.BestProductionScore) as MaxScore, MIN(Scoring.BestProductionScore) as MinScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestProductionScore > 0 GROUP BY FK_NominationsID) 
			ScoreMaxMin ON ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.BestProductionScore) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestProductionScore > 0 GROUP BY FK_NominationsID) 
			ScoreTotal ON ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.BestProductionScore) as MaxScore, MIN(Scoring.BestProductionScore) as MinScore, FK_NominationsID FROM Scoring WHERE Scoring.BestProductionScore > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreMaxMin ON WithReserve_ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.BestProductionScore) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE Scoring.BestProductionScore > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreTotal ON WithReserve_ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
	WHERE   Scoring.BestProductionScore > 0
			AND Production.FK_ProductionCategoryID BETWEEN COALESCE(@ProductionCategoryID, 1) AND COALESCE(@ProductionCategoryID, POWER( 2., 31 ) - 1) 
			AND Production.FK_ProductionTypeID BETWEEN COALESCE(@ProductionTypeID, 1) AND COALESCE(@ProductionTypeID, POWER( 2., 31 ) - 1) 
	GROUP BY	Nominations.PK_NominationsID, Nominations.FK_ProductionID, Production.FK_ProductionTypeID, Production.FK_ProductionCategoryID, Company.CompanyName, 
				Nominations.Director, NumAdj.NumOfAdjudicatorsWithCompletedBallot, NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				,ScoreTotal.TotalScore,	ScoreMaxMin.MaxScore, ScoreMaxMin.MinScore ,WithReserve_ScoreTotal.TotalScore, WithReserve_ScoreMaxMin.MaxScore, WithReserve_ScoreMaxMin.MinScore
	HAVING      (LEN(Nominations.Director) > 0) --AND (LEN('Director') > 0)


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Return Results in Detail (per Adjudicator) or Summary (summed for Nominee)
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
IF @WithAdjudicatorScores = 1 
	BEGIN
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Return Results in Detail (per Adjudicator)  --
		-- NOTE: here the field "NumOfAdjudicatorsForProduction" = [# Of Usable Ballots]
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		SELECT		Scoring.PK_ScoringID, Nominations.PK_NominationsID, 
					Production.PK_ProductionID, Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,    
					ProductionType.PK_ProductionTypeID, ProductionType.ProductionType,  
					Company.CompanyName, ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory,  
					Users.LastName + ', ' + Users.FirstName AS AdjudicatorName, CompanyRepresented.CompanyName AS AdjudicatorCompanyRepresented,  
					Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual, Scoring.ReserveAdjudicator,
					Scoring.BestProductionScore as AdjudicatorScore,
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
						WHEN (NumOfAdjudicatorsForProduction < 3)
							THEN 0
						WHEN (NumOfAdjudicatorsForProduction = 4)
							THEN ((TotalScore - MaxScore - MinScore) + ((MaxScore + MinScore) /2)) /3
						WHEN (NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1))
							THEN (TotalScore - MaxScore - MinScore) / (NumOfAdjudicatorsForProduction - 2)
					WHEN (NumOfAdjudicatorsWithCompletedBallot < 3)
						THEN 0.00
						ELSE TotalScore / NumOfAdjudicatorsForProduction
					END as AverageScoreFinal
			FROM Production 
				INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID 
				INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID
				INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID 
				INNER JOIN Nominations 
				INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID 
				INNER JOIN Company CompanyRepresented ON Scoring.FK_CompanyID_Adjudicator = CompanyRepresented.PK_CompanyID 
				LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID 
				INNER JOIN 
					(SELECT PK_NominationsID, FK_ProductionTypeID, FK_ProductionCategoryID, BestName, BestRole,
							NumOfAdjudicatorsForProduction, UsingReserveAdjudicatorScore, NumOfAdjudicatorsWithCompletedBallot, 
							MaxScore, MinScore ,TotalScore, AvgScore
						FROM #tmp_ScoreSummaryCalc_1
					) AvgScore1	ON AvgScore1.PK_NominationsID = Scoring.FK_NominationsID  
		ORDER BY PK_ProductionCategoryID, PK_ProductionTypeID, AverageScoreFinal DESC, PK_ScoringID
	END
ELSE
	BEGIN
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Return Results as Summary (summed for Nominee) --
		-- NOTE: here the field "NumOfAdjudicatorsForProduction" = [# Of Usable Ballots]
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		SELECT Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, ProductionType.ProductionType, ProductionCategory.ProductionCategory,  
				#tmp_ScoreSummaryCalc_1.*,
				CASE 
					WHEN (NumOfAdjudicatorsForProduction = 4)
						THEN ((TotalScore - MaxScore - MinScore) +  ((MaxScore + MinScore) /2) )
					WHEN NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1)
						THEN (TotalScore - MaxScore - MinScore) 
					ELSE TotalScore 
				END as TotalScoreFinal,
				CASE 
					WHEN (NumOfAdjudicatorsForProduction < 3)
						THEN 0
					WHEN (NumOfAdjudicatorsForProduction = 4)
						THEN ((TotalScore - MaxScore - MinScore) + ((MaxScore + MinScore) /2)) /3
					WHEN (NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1))
						THEN (TotalScore - MaxScore - MinScore) / (NumOfAdjudicatorsForProduction - 2)
					WHEN (NumOfAdjudicatorsWithCompletedBallot < 3)
						THEN 0.00
					ELSE TotalScore / NumOfAdjudicatorsForProduction
				END as AverageScoreFinal
			 FROM #tmp_ScoreSummaryCalc_1
				INNER JOIN Production ON Production.PK_ProductionID = #tmp_ScoreSummaryCalc_1.FK_ProductionID
				INNER JOIN ProductionType ON #tmp_ScoreSummaryCalc_1.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID 
				INNER JOIN ProductionCategory ON #tmp_ScoreSummaryCalc_1.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID 
			ORDER BY FK_ProductionCategoryID, FK_ProductionTypeID, AverageScoreFinal DESC	
	END