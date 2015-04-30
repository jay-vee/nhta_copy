/*
CREADED BY:		JOE VAGO
CREATED DATE: 	2013-12-24
DESCRIPTION:
During the year, the NHTA found that not categorizing the YOUTH productions to be the best approach.
Yet, current STored Procedures required an INNER JOIN to the YOUTH production type.

Need to update all reporting SProcs to do a OUTER JOIN on the [ProductionType] table.
*/
SET IDENTITY_INSERT [ProductionType] ON

INSERT INTO [ProductionType]
           (PK_ProductionTypeID
           ,[ProductionType]
           ,[LastUpdateByName]
           ,[LastUpdateByDate]
           ,[CreateByName]
           ,[CreateByDate])
     VALUES
           (3
           , 'Youth'
           ,'JoeVago@NHTheatreAwards.org'
           ,GETDATE()
		   ,'JoeVago@NHTheatreAwards.org'
           ,GETDATE()
           )
SET IDENTITY_INSERT [ProductionType] OFF;           
GO


