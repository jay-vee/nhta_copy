/* 
		IMPORTANT Set the Dates correctly before running!
*/

DECLARE @NHTAYearStarts DATETIME, @NHTAYearEnds DATETIME
SET @NHTAYearStarts = CONVERT(DATETIME, '2014-12-16 00:00:00', 102)
SET @NHTAYearEnds = CONVERT(DATETIME, '2015-12-15 00:00:00', 102)

-- SET DEFAULTS
UPDATE ApplicationDefaults 
	SET [ValidTrainingStartDate] = @NHTAYearStarts
		,[NHTAYearStartDate] = @NHTAYearStarts
		,[NHTAYearEndDate] = @NHTAYearEnds

-- DELETE ALL BALLOTS --
DELETE Scoring
	FROM         Production INNER JOIN
	                      Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID INNER JOIN
	                      Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID
	WHERE     (Production.FirstPerformanceDateTime < @NHTAYearStarts)


-- DELETE ALL NOMINATIONS --
DELETE Nominations
	FROM         Production INNER JOIN
	                      Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID
	WHERE     (Production.FirstPerformanceDateTime < @NHTAYearStarts)

-- DELETE ALL PRODUCTIONS --
DELETE from Production 
	WHERE     (FirstPerformanceDateTime < @NHTAYearStarts)

-- CLEAR DATA FOR THE YEAR ONLY
DELETE from AdminMessage
DELETE from ErrorLog
DELETE from EmailLog
DELETE from BrowserDetect
-- Delete Inactive Users
--DELETE  FROM [Users] where [FK_AccessLevelID] >=6 OR [Active] = 0

-- MAKE ALL NON-ADMIN USERS BACKUP ADMINISTRATORS
UPDATE USERS SET [FK_AccessLevelID] = 6 where [FK_AccessLevelID] > 1 and [FK_AccessLevelID] < 6

-- Reset every companies NumOfProductions
UPDATE Company SET NumOfProductions = 0, ActiveCompany=0


GO