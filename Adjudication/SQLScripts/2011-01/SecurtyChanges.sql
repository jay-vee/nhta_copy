
ALTER TABLE Users ADD
	IsAdmin tinyint NULL,
	IsLiaison tinyint NULL,
	IsReportViewer tinyint NULL
GO

UPDATE Users SET IsAdmin = 1, IsReportViewer = 1 WHERE Users.FK_AccessLevelID=1
UPDATE Users SET IsLiaison = 1 WHERE Users.FK_AccessLevelID=2 or Users.FK_AccessLevelID=3
UPDATE Users SET IsReportViewer = 0 WHERE Users.FK_AccessLevelID <> 1
