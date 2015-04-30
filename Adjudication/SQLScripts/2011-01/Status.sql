BEGIN TRANSACTION

CREATE TABLE [dbo].[UserStatus](
	[PK_UserStatusID] [int] IDENTITY(1,1) NOT NULL,
	[UserStatus] [varchar](50) NOT NULL,
	Active TINYINT NOT NULL CONSTRAINT [DF_UserStatus_Active]  DEFAULT ((1)),
	[UserStatusDescription] [varchar](500) NULL,
	[LastUpdateByName] [varchar](50) NOT NULL CONSTRAINT [DF_UserStatus_LastUpdateByName]  DEFAULT (suser_sname()),
	[LastUpdateByDate] [smalldatetime] NOT NULL CONSTRAINT [DF_UserStatus_LastUpdateByDate]  DEFAULT (getdate()),
	[CreateByName] [varchar](50) NOT NULL CONSTRAINT [DF_UserStatus_CreateByName]  DEFAULT (suser_sname()),
	[CreateByDate] [smalldatetime] NOT NULL CONSTRAINT [DF_UserStatus_CreateByDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_UserStatus] PRIMARY KEY NONCLUSTERED 
(
	[PK_UserStatusID] ASC
) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
go

INSERT INTO UserStatus ([UserStatus], [LastUpdateByName], [CreateByName], Active)
	VALUES ('n/a', 'JoeVago', 'JoeVago', 0)
INSERT INTO UserStatus ([UserStatus], [LastUpdateByName], [CreateByName], Active)
	VALUES ('Active', 'JoeVago', 'JoeVago', 1)
INSERT INTO UserStatus ([UserStatus], [LastUpdateByName], [CreateByName], Active)
	VALUES ('Inactive', 'JoeVago', 'JoeVago', 0)
INSERT INTO UserStatus ([UserStatus], [LastUpdateByName], [CreateByName], Active)
	VALUES ('SUSPENDED', 'JoeVago', 'JoeVago', 0)
INSERT INTO UserStatus ([UserStatus], [LastUpdateByName], [CreateByName], Active)
	VALUES ('EXPELLED', 'JoeVago', 'JoeVago', 0)
GO

ALTER TABLE dbo.Users ADD
	FK_UserStatusID int NULL
GO
ALTER TABLE dbo.Users ADD CONSTRAINT
	FK_Users_UserStatus FOREIGN KEY
	(
	FK_UserStatusID
	) REFERENCES dbo.UserStatus
	(
	PK_UserStatusID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

UPDATE USERS SET FK_UserStatusID = 1
UPDATE USERS SET FK_UserStatusID = 2
	WHERE USERS.ACTIVE = 1

UPDATE USERS SET FK_UserStatusID = 3
	WHERE USERS.ACTIVE = 0

UPDATE USERS SET FK_UserStatusID = 4
	WHERE USERS.FK_AccessLevelID = 7

UPDATE USERS SET FK_UserStatusID = 5
	WHERE USERS.FK_AccessLevelID = 8

GO

COMMIT
