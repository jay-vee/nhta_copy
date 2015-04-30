BEGIN TRANSACTION
GO

ALTER TABLE dbo.Company
	DROP CONSTRAINT FK_Company_CompanyTypes
GO
ALTER TABLE dbo.CompanyTypes SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE dbo.Company SET (LOCK_ESCALATION = TABLE)
GO

DROP TABLE [dbo].[CompanyTypes]
GO

ALTER TABLE Company
	DROP COLUMN FK_CompanyTypeID
GO
COMMIT


BEGIN TRANSACTION
GO
CREATE TABLE [dbo].[ProductionCategory](
	[PK_ProductionCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ProductionCategory] [varchar](50) NULL,
	[LastUpdateByName] [varchar](50) NOT NULL,
	[LastUpdateByDate] [smalldatetime] NOT NULL CONSTRAINT [DF_ProductionCategory_LastUpdateByDate]  DEFAULT (getdate()),
	[CreateByName] [varchar](50) NOT NULL,
	[CreateByDate] [smalldatetime] NOT NULL CONSTRAINT [DF_ProductionCategory_CreateByDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_ProductionCategory] PRIMARY KEY CLUSTERED 
(
	[PK_ProductionCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[ProductionCategory] ([ProductionCategory],[LastUpdateByName],[LastUpdateByDate],[CreateByName],[CreateByDate])
     VALUES ('Professional','joevago@nhtheatreawards.org',getdate(),'joevago@nhtheatreawards.org',getdate())
GO
INSERT INTO [dbo].[ProductionCategory] ([ProductionCategory],[LastUpdateByName],[LastUpdateByDate],[CreateByName],[CreateByDate])
     VALUES ('Community','joevago@nhtheatreawards.org',getdate(),'joevago@nhtheatreawards.org',getdate())
GO
INSERT INTO [dbo].[ProductionCategory] ([ProductionCategory],[LastUpdateByName],[LastUpdateByDate],[CreateByName],[CreateByDate])
     VALUES ('Youth','joevago@nhtheatreawards.org',getdate(),'joevago@nhtheatreawards.org',getdate())
GO


ALTER TABLE Production
ADD FK_ProductionCategoryID INT
GO

ALTER TABLE dbo.ProductionCategory SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE dbo.Production ADD CONSTRAINT
	FK_Production_ProductionCategory FOREIGN KEY
	(
	FK_ProductionCategoryID
	) REFERENCES dbo.ProductionCategory
	(
	PK_ProductionCategoryID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Production SET (LOCK_ESCALATION = TABLE)
GO
COMMIT


BEGIN TRANSACTION
GO
ALTER TABLE dbo.Production
	DROP CONSTRAINT FK_Production_Venue1
GO
ALTER TABLE dbo.Venue SET (LOCK_ESCALATION = TABLE)
GO
COMMIT