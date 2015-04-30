BEGIN TRANSACTION
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScoringStatus]') AND type in (N'U'))
DROP TABLE [dbo].[ScoringStatus]
GO

CREATE TABLE [dbo].[ScoringStatus](
	[PK_ScoringStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ScoringStatus] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LastUpdateByName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LastUpdateByDate] [smalldatetime] NOT NULL CONSTRAINT [DF_ScoringStatus_LastUpdateByDate]  DEFAULT (getdate()),
	[CreateByName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreateByDate] [smalldatetime] NOT NULL CONSTRAINT [DF_ScoringStatus_CreateByDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_ScoringStatus] PRIMARY KEY CLUSTERED 
(
	[PK_ScoringStatusID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [ScoringStatus] (ScoringStatus, LastUpdateByName, CreateByName)
	VALUES(	'Awaiting Confirmation of Attendance date', 'joevago', 'joevago')
INSERT INTO [ScoringStatus] (ScoringStatus, LastUpdateByName, CreateByName)
	VALUES(	'Confirmed Attendance with Producing Company', 'joevago', 'joevago')
INSERT INTO [ScoringStatus] (ScoringStatus, LastUpdateByName, CreateByName)
	VALUES(	'Attended Production - ballot pending', 'joevago', 'joevago')
INSERT INTO [ScoringStatus] (ScoringStatus, LastUpdateByName, CreateByName)
	VALUES(	'Did not attend Production.  No Ballot will be submitted', 'joevago', 'joevago')
INSERT INTO [ScoringStatus] (ScoringStatus, LastUpdateByName, CreateByName)
	VALUES(	'Ballot Submitted', 'joevago', 'joevago')
INSERT INTO [ScoringStatus] (ScoringStatus, LastUpdateByName, CreateByName)
	VALUES(	'Pending Reassignment', 'joevago', 'joevago')
GO


ALTER TABLE dbo.Scoring ADD
	FK_ScoringStatusID int NULL
GO
ALTER TABLE dbo.Scoring ADD CONSTRAINT
	DF_Scoring_FK_ScoringStatusID DEFAULT 1 FOR FK_ScoringStatusID
GO

ALTER TABLE dbo.Scoring ADD CONSTRAINT
	FK_Scoring_ScoringStatusID FOREIGN KEY
	(
	FK_ScoringStatusID
	) REFERENCES dbo.ScoringStatus
	(
	PK_ScoringStatusID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

UPDATE Scoring SET FK_ScoringStatusID = 1
GO

COMMIT

