set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-----------------------------------------------------------------------------------------------------------------------
ALTER PROCEDURE [dbo].[Save_Production]
						(	@PK_ProductionID int, 
							@FK_CompanyID int, 
							@FK_VenueID int, 
							@FK_AgeApproriateID int, 
							@FK_ProductionTypeID int, 
							@Title varchar(100), 
							@Authors varchar(200), 
							@LicensingCompany varchar(100), 
							@FirstPerformanceDateTime smalldatetime, 
							@LastPerformanceDateTime smalldatetime, 
							@AllPerformanceDatesTimes varchar(8000), 
							@TicketContactName varchar(50), 
							@TicketContactPhone varchar(50), 
							@TicketContactEmail varchar(100), 
							@TicketPurchaseDetails varchar(8000), 
							@Comments varchar (8000), 
							@RequiresAdjudication tinyint, 
							@OriginalProduction tinyint, 
							@LastUpdateByName varchar (50),
							@ProductionID Int OUTPUT )
AS 
/*	Object: Save_Production 
	Description:     Saves (Insert or Update) Production data 
	Returns:         @ProductionID Int
	Created By:      Joe Vago 
	Example:         EXEC Save_Production.... 
	Created Date:    6/7/07 
	Modified By:	Joe Vago 
	Modified Date:	12/20/2009
	NOTES:			Added new field 'OriginalProduction'
*/ 
SET NOCOUNT ON 

IF (@PK_ProductionID > 0) 
	BEGIN 
		UPDATE Production 
			SET FK_CompanyID= @FK_CompanyID,
				FK_VenueID= @FK_VenueID,
				FK_AgeApproriateID= @FK_AgeApproriateID,
				FK_ProductionTypeID= @FK_ProductionTypeID,
				Title= @Title,
				Authors= @Authors,
				LicensingCompany= @LicensingCompany,
				FirstPerformanceDateTime= @FirstPerformanceDateTime,
				LastPerformanceDateTime= @LastPerformanceDateTime,
				AllPerformanceDatesTimes= @AllPerformanceDatesTimes,
				TicketContactName= @TicketContactName,
				TicketContactPhone= @TicketContactPhone,
				TicketContactEmail= @TicketContactEmail,
				TicketPurchaseDetails= @TicketPurchaseDetails,
				Comments= @Comments,
				RequiresAdjudication= @RequiresAdjudication,
				OriginalProduction= @OriginalProduction, 
				LastUpdateByName= @LastUpdateByName,
				LastUpdateByDate= GetDate() 
		WHERE PK_ProductionID= @PK_ProductionID 

		SET @ProductionID = @PK_ProductionID
	END 
ELSE 
	BEGIN 
		INSERT INTO Production( FK_CompanyID,
			FK_VenueID,
			FK_AgeApproriateID,
			FK_ProductionTypeID,
			Title,
			Authors,
			LicensingCompany,
			FirstPerformanceDateTime,
			LastPerformanceDateTime,
			AllPerformanceDatesTimes,
			TicketContactName,
			TicketContactPhone,
			TicketContactEmail,
			TicketPurchaseDetails,
			Comments,
			RequiresAdjudication,
			OriginalProduction,
			LastUpdateByName,
			CreateByName ) 
	VALUES( @FK_CompanyID,
			@FK_VenueID,
			@FK_AgeApproriateID,
			@FK_ProductionTypeID,
			@Title,
			@Authors,
			@LicensingCompany,
			@FirstPerformanceDateTime,
			@LastPerformanceDateTime,
			@AllPerformanceDatesTimes,
			@TicketContactName,
			@TicketContactPhone,
			@TicketContactEmail,
			@TicketPurchaseDetails,
			@Comments,
			@RequiresAdjudication,
			@OriginalProduction,
			@LastUpdateByName,
			@LastUpdateByName )  

	SET @ProductionID = @@IDENTITY

	IF @RequiresAdjudication = 1 
		BEGIN 
3			/* Create a Dummy Nomination */ 
			INSERT INTO Nominations( 
				FK_ProductionID,
				Director,
				LastUpdateByName,
				CreateByName    ) 
			VALUES( @ProductionID,
				'To Be Announced',
				@LastUpdateByName,
				@LastUpdateByName  ) 
		END 
	END  

RETURN @ProductionID

