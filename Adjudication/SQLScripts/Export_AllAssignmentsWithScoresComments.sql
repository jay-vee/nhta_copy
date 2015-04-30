SELECT  Nominations.PK_NominationsID, Production.PK_ProductionID,
	    Company.PK_CompanyID,ProductionCategory.PK_ProductionCategory,
	    Users.PK_UserID, Users.UserLoginID, Users.LastName, Users.FirstName, 
	    Production.Title, ProductionType.ProductionType,
	    Company.CompanyName, ProductionCategory.ProductionCategory,
	    Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime,        Scoring.AdjudicatorRequestsReassignment, Scoring.ReserveAdjudicator, ProductionDateAdjudicated_Planned, ProductionDateAdjudicated_Actual,
	    ScoringStatus.ScoringStatus, 
	    CASE WHEN ProductionDateAdjudicated_Planned IS NOT NULL THEN 'Yes' ELSE 'No' END as AdjudicatorToSeeProduction,        DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore
	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score
	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score
	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore, 
Scoring.*
  FROM  Users INNER JOIN
	    Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN
	    Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID INNER JOIN
	    Production INNER JOIN
	    Company ON Production.FK_CompanyID = Company.PK_CompanyID INNER JOIN
	    ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID ON
	    Nominations.FK_ProductionID = Production.PK_ProductionID INNER JOIN
	    ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID
	    INNER JOIN ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID
  ORDER BY Production.Title, Company.CompanyName, Users.LastName, Users.FirstName