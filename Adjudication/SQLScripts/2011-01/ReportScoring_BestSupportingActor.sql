ALTER PROCEDURE [dbo].[ReportScoring_BestSupportingActor]
	(
		@WithAdjudicatorScores INT = NULL ,
		@CompanyTypeID INT = NULL,
		@ProductionTypeID INT = NULL
	) 
AS
/* =============================================================================== 
Object:  		ReportScoring_BestSupportingActor
Description:  	
Returns:  		Result set, in Order of Highest Average Score to Lowest
Created By:  	Joe L. Vago
Created Date:  	12/18/08
NOTES:			12/18/08: If no value for @ActorActress, Actor will be default
Modified By:  	Joe L. Vago
Modified Date:	1/2/2011
NOTES:			1/2/2011: Fix for including "Youth" with "Community" Production type
=============================================================================== 
DECLARE @CompanyTypeID as INT, @ProductionTypeID as INT, @WithAdjudicatorScores as INT
--SET @CompanyTypeID = 0
SET @WithAdjudicatorScores = 1
DROP TABLE #tmp_ScoreSummaryCalc_1
DROP TABLE #tmp_ScoreSummaryCalc_2
=============================================================================== */
SET NOCOUNT ON
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Get value for # of Adjudicator per show from ApplicationDefaults table
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DECLARE @NumAdjPerShow as INT
SET @NumAdjPerShow = (SELECT NumAdjudicatorsPerShow FROM ApplicationDefaults) - 1

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Do Calculations for 1st Best Actor 
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT 		Nominations.PK_NominationsID, Nominations.FK_ProductionID, Company.FK_CompanyTypeID, Company.CompanyName, 
			CASE WHEN Company.FK_CompanyTypeID= 3 THEN 3 ELSE Production.FK_ProductionTypeID END AS FK_ProductionTypeID, 
			Nominations.BestSupportingActor1Name AS BestName, Nominations.BestSupportingActor1Role AS BestRole, 'Supporting Actor' as CategoryName, 
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
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN 'Yes'
				ELSE 'No'
			END AS UsingReserveAdjudicatorScore,
			NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
INTO #tmp_ScoreSummaryCalc_1
	FROM Production 
		INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID 
		INNER JOIN Nominations 
		INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestSupportingActor1Score > 0 GROUP BY FK_NominationsID) 
			NumAdj ON NumAdj.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE Scoring.BestSupportingActor1Score > 0 GROUP BY FK_NominationsID) 
			NumAdjWithReserve ON NumAdjWithReserve.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.BestSupportingActor1Score) as MaxScore, MIN(Scoring.BestSupportingActor1Score) as MinScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestSupportingActor1Score > 0 GROUP BY FK_NominationsID) 
			ScoreMaxMin ON ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.BestSupportingActor1Score) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestSupportingActor1Score > 0 GROUP BY FK_NominationsID) 
			ScoreTotal ON ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.BestSupportingActor1Score) as MaxScore, MIN(Scoring.BestSupportingActor1Score) as MinScore, FK_NominationsID FROM Scoring WHERE Scoring.BestSupportingActor1Score > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreMaxMin ON WithReserve_ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.BestSupportingActor1Score) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE Scoring.BestSupportingActor1Score > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreTotal ON WithReserve_ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
	WHERE   Scoring.BestSupportingActor1Score > 0
			AND Company.FK_CompanyTypeID BETWEEN COALESCE(@CompanyTypeID, 1) AND COALESCE(@CompanyTypeID, POWER( 2., 31 ) - 1) 
			AND Production.FK_ProductionTypeID BETWEEN COALESCE(@ProductionTypeID, 1) AND COALESCE(@ProductionTypeID, POWER( 2., 31 ) - 1) 
	GROUP BY	Nominations.PK_NominationsID, Nominations.FK_ProductionID, Production.FK_ProductionTypeID, Company.FK_CompanyTypeID, Company.CompanyName, 
				Nominations.BestSupportingActor1Name, Nominations.BestSupportingActor1Role, NumAdj.NumOfAdjudicatorsWithCompletedBallot, NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				,ScoreTotal.TotalScore,	ScoreMaxMin.MaxScore, ScoreMaxMin.MinScore ,WithReserve_ScoreTotal.TotalScore, WithReserve_ScoreMaxMin.MaxScore, WithReserve_ScoreMaxMin.MinScore
	HAVING      (LEN(Nominations.BestSupportingActor1Name) > 0) --AND (LEN(Nominations.BestSupportingActor1Role) > 0)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Same Calculations for 2nd best Actor in field BestSupportingActor2Name, BestSupportingActor2Role
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT 		Nominations.PK_NominationsID, Nominations.FK_ProductionID, Company.FK_CompanyTypeID, Company.CompanyName, 
			CASE WHEN Company.FK_CompanyTypeID= 3 THEN 3 ELSE Production.FK_ProductionTypeID END AS FK_ProductionTypeID, 
			Nominations.BestSupportingActor2Name AS BestName, Nominations.BestSupportingActor2Role AS BestRole, 'Supporting Actor' as CategoryName, 
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
			CASE WHEN NumAdj.NumOfAdjudicatorsWithCompletedBallot < @NumAdjPerShow THEN 'Yes'
				ELSE 'No'
			END AS UsingReserveAdjudicatorScore,
			NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
INTO #tmp_ScoreSummaryCalc_2
	FROM Production 
		INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID 
		INNER JOIN Nominations 
		INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID ON Production.PK_ProductionID = Nominations.FK_ProductionID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestSupportingActor2Score > 0 GROUP BY FK_NominationsID) 
			NumAdj ON NumAdj.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT Count(PK_ScoringID) as NumOfAdjudicatorsWithCompletedBallot, FK_NominationsID FROM Scoring WHERE Scoring.BestSupportingActor2Score > 0 GROUP BY FK_NominationsID) 
			NumAdjWithReserve ON NumAdjWithReserve.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.BestSupportingActor2Score) as MaxScore, MIN(Scoring.BestSupportingActor2Score) as MinScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestSupportingActor2Score > 0 GROUP BY FK_NominationsID) 
			ScoreMaxMin ON ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.BestSupportingActor2Score) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE ReserveAdjudicator = 0 AND Scoring.BestSupportingActor2Score > 0 GROUP BY FK_NominationsID) 
			ScoreTotal ON ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT MAX(Scoring.BestSupportingActor2Score) as MaxScore, MIN(Scoring.BestSupportingActor2Score) as MinScore, FK_NominationsID FROM Scoring WHERE Scoring.BestSupportingActor2Score > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreMaxMin ON WithReserve_ScoreMaxMin.FK_NominationsID = Scoring.FK_NominationsID
		INNER JOIN (SELECT SUM(Scoring.BestSupportingActor2Score) + 0.00  as TotalScore, FK_NominationsID FROM Scoring WHERE Scoring.BestSupportingActor2Score > 0 GROUP BY FK_NominationsID) 
			WithReserve_ScoreTotal ON WithReserve_ScoreTotal.FK_NominationsID = Scoring.FK_NominationsID
	WHERE   Scoring.BestSupportingActor2Score > 0
			AND Company.FK_CompanyTypeID BETWEEN COALESCE(@CompanyTypeID, 1) AND COALESCE(@CompanyTypeID, POWER( 2., 31 ) - 1) 
			AND Production.FK_ProductionTypeID BETWEEN COALESCE(@ProductionTypeID, 1) AND COALESCE(@ProductionTypeID, POWER( 2., 31 ) - 1) 
	GROUP BY	Nominations.PK_NominationsID, Nominations.FK_ProductionID, Production.FK_ProductionTypeID, Company.FK_CompanyTypeID, Company.CompanyName, 
				Nominations.BestSupportingActor2Name, Nominations.BestSupportingActor2Role, NumAdj.NumOfAdjudicatorsWithCompletedBallot, NumAdjWithReserve.NumOfAdjudicatorsWithCompletedBallot
				,ScoreTotal.TotalScore,	ScoreMaxMin.MaxScore, ScoreMaxMin.MinScore ,WithReserve_ScoreTotal.TotalScore, WithReserve_ScoreMaxMin.MaxScore, WithReserve_ScoreMaxMin.MinScore
	HAVING      (LEN(Nominations.BestSupportingActor2Name) > 0) --AND (LEN(Nominations.BestSupportingActor2Role) > 0)


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Return Results in Detail (per Adjudicator) or Summary (summed for Nominee)
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
IF @WithAdjudicatorScores = 1 
	BEGIN
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Return Results in Detail (per Adjudicator)  --
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		(SELECT		Scoring.PK_ScoringID, Nominations.PK_NominationsID, 
					Production.PK_ProductionID, Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,    
					ProductionType.PK_ProductionTypeID, ProductionType.ProductionType,  
					Company.CompanyName, CompanyTypes.PK_CompanyTypeID, CompanyTypes.CompanyType,  
					Users.LastName + ', ' + Users.FirstName AS AdjudicatorName, CompanyRepresented.CompanyName AS AdjudicatorCompanyRepresented,  
					Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual, Scoring.ReserveAdjudicator,
					Scoring.BestSupportingActor1Score as AdjudicatorScore,
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
		)
		UNION
		(SELECT		Scoring.PK_ScoringID, Nominations.PK_NominationsID, 
					Production.PK_ProductionID, Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,    
					ProductionType.PK_ProductionTypeID, ProductionType.ProductionType,  
					Company.CompanyName, CompanyTypes.PK_CompanyTypeID, CompanyTypes.CompanyType,  
					Users.LastName + ', ' + Users.FirstName AS AdjudicatorName, CompanyRepresented.CompanyName AS AdjudicatorCompanyRepresented,  
					Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual, Scoring.ReserveAdjudicator,
					Scoring.BestSupportingActor2Score as AdjudicatorScore,
					AvgScore2.BestName, AvgScore2.BestRole,
					AvgScore2.NumOfAdjudicatorsForProduction, UsingReserveAdjudicatorScore, AvgScore2.NumOfAdjudicatorsWithCompletedBallot, 
					AvgScore2.MaxScore, AvgScore2.MinScore ,AvgScore2.TotalScore, AvgScore2.AvgScore,
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
						FROM #tmp_ScoreSummaryCalc_2
					) AvgScore2	ON AvgScore2.PK_NominationsID = Scoring.FK_NominationsID  
		)
		ORDER BY PK_CompanyTypeID, PK_ProductionTypeID, AverageScoreFinal DESC, PK_ScoringID
	END
ELSE
	BEGIN
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		-- Return Results as Summary (summed for Nominee) --
		-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		(SELECT Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, 
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
					WHEN (NumOfAdjudicatorsForProduction < 3)
						THEN 0
					WHEN (NumOfAdjudicatorsForProduction = 4)
						THEN ((TotalScore - MaxScore - MinScore) + ((MaxScore + MinScore) /2)) /3
					WHEN (NumOfAdjudicatorsForProduction >= (@NumAdjPerShow - 1))
						THEN (TotalScore - MaxScore - MinScore) / (NumOfAdjudicatorsForProduction - 2)
					ELSE TotalScore / NumOfAdjudicatorsForProduction
				END as AverageScoreFinal
			 FROM #tmp_ScoreSummaryCalc_1
				INNER JOIN Production ON Production.PK_ProductionID = #tmp_ScoreSummaryCalc_1.FK_ProductionID
				INNER JOIN ProductionType ON #tmp_ScoreSummaryCalc_1.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID 
				INNER JOIN CompanyTypes ON #tmp_ScoreSummaryCalc_1.FK_CompanyTypeID = CompanyTypes.PK_CompanyTypeID 
			)
		UNION 
		(SELECT Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, 
				ProductionType.ProductionType, CompanyTypes.CompanyType,
				#tmp_ScoreSummaryCalc_2.*,
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
					ELSE TotalScore / NumOfAdjudicatorsForProduction
				END as AverageScoreFinal
			FROM #tmp_ScoreSummaryCalc_2
				INNER JOIN Production ON Production.PK_ProductionID = #tmp_ScoreSummaryCalc_2.FK_ProductionID
				INNER JOIN ProductionType ON #tmp_ScoreSummaryCalc_2.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID 
				INNER JOIN CompanyTypes ON #tmp_ScoreSummaryCalc_2.FK_CompanyTypeID = CompanyTypes.PK_CompanyTypeID
			) 
		ORDER BY FK_CompanyTypeID, FK_ProductionTypeID, AverageScoreFinal DESC	
	END
