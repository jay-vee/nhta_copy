SET IDENTITY_INSERT ProductionType ON
INSERT INTO ProductionType
           (PK_ProductionTypeID
			,ProductionType
           ,LastUpdateByName
           ,LastUpdateByDate
           ,CreateByName
           ,CreateByDate)
     VALUES
           (3, 'Youth'
           ,'JoeVago@NHTheatreAwards.org'
           ,GetDate()
           ,'JoeVago@NHTheatreAwards.org'
           ,GetDate())

SET IDENTITY_INSERT ProductionType Off
GO

/*

UPDATE Production SET Production.FK_ProductionTypeID = 2 
	FROM  Production INNER JOIN Company ON Company.PK_CompanyID = Production.FK_CompanyID
	WHERE Company.FK_CompanyTypeID= 3

*/
GO