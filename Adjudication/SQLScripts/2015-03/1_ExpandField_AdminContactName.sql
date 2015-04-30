/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_DaysToSubmitProduction
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_DaysToAllowNominationEdits
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_DaysToConfirmAttendance
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_DaysToSubmitProduction1
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_NumAdjudPerShow
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_ScoringMinimum
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_ScoringMaximum
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_LastUpdateByName
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_LastUpdateByDate
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_CreateByName
GO
ALTER TABLE dbo.ApplicationDefaults
	DROP CONSTRAINT DF_ApplicationDefaults_CreateByDate
GO
CREATE TABLE dbo.Tmp_ApplicationDefaults
	(
	PK_ApplicationDefaultsID int NOT NULL,
	ApplicationName varchar(100) NULL,
	MainpageApplicationDesc text NULL,
	MainpageApplicationNotes text NULL,
	AdminContactName varchar(MAX) NOT NULL,
	AdminContactPhoneNum varchar(20) NULL,
	AdminContactEmail varchar(200) NOT NULL,
	ValidTrainingStartDate smalldatetime NULL,
	DaysToSubmitProduction tinyint NOT NULL,
	DaysToAllowNominationEdits tinyint NOT NULL,
	DaysToConfirmAttendance tinyint NOT NULL,
	DaysToWaitForScoring tinyint NOT NULL,
	NumAdjudicatorsPerShow tinyint NOT NULL,
	MaxShowsPerAdjudicator tinyint NULL,
	ScoringMinimum tinyint NULL,
	ScoringMaximum tinyint NOT NULL,
	ManualURL_Rules varchar(200) NULL,
	ManualURL_Admin varchar(200) NULL,
	ManualURL_Liaison varchar(200) NULL,
	ManualURL_Adjudicator varchar(200) NULL,
	NHTAwardsShowDate smalldatetime NULL,
	NHTAYearStartDate smalldatetime NULL,
	NHTAYearEndDate smalldatetime NULL,
	LastUpdateByName varchar(50) NOT NULL,
	LastUpdateByDate smalldatetime NOT NULL,
	CreateByName varchar(50) NOT NULL,
	CreateByDate smalldatetime NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_DaysToSubmitProduction DEFAULT ((90)) FOR DaysToSubmitProduction
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_DaysToAllowNominationEdits DEFAULT ((14)) FOR DaysToAllowNominationEdits
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_DaysToConfirmAttendance DEFAULT ((14)) FOR DaysToConfirmAttendance
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_DaysToSubmitProduction1 DEFAULT ((30)) FOR DaysToWaitForScoring
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_NumAdjudPerShow DEFAULT ((5)) FOR NumAdjudicatorsPerShow
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_ScoringMinimum DEFAULT ((1)) FOR ScoringMinimum
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_ScoringMaximum DEFAULT ((100)) FOR ScoringMaximum
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_LastUpdateByName DEFAULT (suser_sname()) FOR LastUpdateByName
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_LastUpdateByDate DEFAULT (getdate()) FOR LastUpdateByDate
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_CreateByName DEFAULT (suser_sname()) FOR CreateByName
GO
ALTER TABLE dbo.Tmp_ApplicationDefaults ADD CONSTRAINT
	DF_ApplicationDefaults_CreateByDate DEFAULT (getdate()) FOR CreateByDate
GO
IF EXISTS(SELECT * FROM dbo.ApplicationDefaults)
	 EXEC('INSERT INTO dbo.Tmp_ApplicationDefaults (PK_ApplicationDefaultsID, ApplicationName, MainpageApplicationDesc, MainpageApplicationNotes, AdminContactName, AdminContactPhoneNum, AdminContactEmail, ValidTrainingStartDate, DaysToSubmitProduction, DaysToAllowNominationEdits, DaysToConfirmAttendance, DaysToWaitForScoring, NumAdjudicatorsPerShow, MaxShowsPerAdjudicator, ScoringMinimum, ScoringMaximum, ManualURL_Rules, ManualURL_Admin, ManualURL_Liaison, ManualURL_Adjudicator, NHTAwardsShowDate, NHTAYearStartDate, NHTAYearEndDate, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate)
		SELECT PK_ApplicationDefaultsID, ApplicationName, MainpageApplicationDesc, MainpageApplicationNotes, CONVERT(varchar(MAX), AdminContactName), AdminContactPhoneNum, AdminContactEmail, ValidTrainingStartDate, DaysToSubmitProduction, DaysToAllowNominationEdits, DaysToConfirmAttendance, DaysToWaitForScoring, NumAdjudicatorsPerShow, MaxShowsPerAdjudicator, ScoringMinimum, ScoringMaximum, ManualURL_Rules, ManualURL_Admin, ManualURL_Liaison, ManualURL_Adjudicator, NHTAwardsShowDate, NHTAYearStartDate, NHTAYearEndDate, LastUpdateByName, LastUpdateByDate, CreateByName, CreateByDate FROM dbo.ApplicationDefaults WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.ApplicationDefaults
GO
EXECUTE sp_rename N'dbo.Tmp_ApplicationDefaults', N'ApplicationDefaults', 'OBJECT' 
GO
ALTER TABLE dbo.ApplicationDefaults ADD CONSTRAINT
	ApplicationDefaults_PK PRIMARY KEY CLUSTERED 
	(
	PK_ApplicationDefaultsID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT