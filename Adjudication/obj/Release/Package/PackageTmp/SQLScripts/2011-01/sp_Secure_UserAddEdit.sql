
ALTER   PROCEDURE [dbo].[sp_Secure_UserAddEdit] 
	@PK_UserID int, 
	@UserLoginID varchar(50), 
	@UserPassword varchar(50), 
	@FK_AccessLevelID int, 
	@FK_CompanyID int, 
	@FirstName varchar(50), 
	@LastName varchar(50), 
	@Address varchar(50), 
	@City varchar(50), 
	@State varchar(50), 
	@ZIP varchar(50), 
	@PhonePrimary varchar(50), 
	@PhoneSecondary varchar(50), 
	@EmailPrimary varchar(50), 
	@EmailSecondary varchar(50), 
	@LastTrainingDate varchar(50), 
	@Website varchar(50), 
	@UserInformation varchar(1000), 
	@Active TinyInt,
	@LastUpdateByName varchar(50),
	@SecurityQuestion varchar(250), 
	@SecurityAnswer varchar(100),
	@FK_UserStatusID int
AS
/*
Object:  		sp_Secure_UserAddEdit
Description:  	Adds a username (UserLoginID), UserPassword , first/last names and security-level to the TechSupport database.
Usage:  		SEE EXAMPLE
Returns:  		1 on UserPassword too long  
               	2 too short 
               	3 user already exists
				4 Maximum UserPassword length is less than Minimum UserPassword length in UserOptions
Created By:  	Brian Goumillout - Major Modifications by Joe L. Vago
Example:  		exec sp_Secure_UserAddEdit @UserLoginID ='joevago' , @UserPassword ='joevago1' , @FK_AccessLevelID =1 , @FirstName ='Joe' , @LastName ='Vago' , @Address ='700 S. Porter Street' , @City ='Manchester' , @State ='NH' , @ZIP ='03103' , @PhonePrimary ='603-867-5334' , @PhoneSecondary ='' , @EmailPrimary ='JoeVago@Comcast.net' , @EmailSecondary ='Joe.Vago@Citizensbank.com' , 
				@LastTrainingDate = '2/2/07', @Website ='WWW' , @UserInformation ='Site Programmer', @LastUpdateByName ='joevago'
Created:  		11/13/2000
Modified:		6/19/07 
				11/19/11 - added field FK_UserStatusID
*/
SET NOCOUNT ON

Declare @EncryptedString VARCHAR(50)						/* holds theENCRYPTed UserPassword*/
Declare @MinLoginIDLength smallint							/* value from UserOptions*/
Declare @MaxLoginIDLength smallint							/* value from UserOptions*/
Declare @MinPasswordLength smallint							/* value from UserOptions*/
Declare @MaxPasswordLength smallint							/* value from UserOptions*/
Declare @AddOrEdit smallint									/* 0=AddNew; 1=Edit/Update*/
Declare @ExistingUsername varchar(50)						/* holds the username extracted FROM the USERS table*/
Declare @UserID INT											/* holds the UserID extracted FROM the USERS table*/

/* Validate UserLoginID  */
Set @MinLoginIDLength = (SELECT MinLoginIDLength FROM UserOptions WHERE PK_UserOptionsID = 1)  /* Should only be 1 record in UserOptions table*/
Set @MaxLoginIDLength = (SELECT MaxLoginIDLength FROM UserOptions WHERE PK_UserOptionsID = 1)  /* Should only be 1 record in UserOptions table*/

IF @MaxLoginIDLength < @MinLoginIDLength 
  BEGIN
    SELECT 4 As ReturnValue, 'Maximum Login ID length is less than Minimum Password length in UserOptions.' As ReturnUserInformation
	Goto DONE
  END
	
IF (LEN(@UserLoginID) < @MinLoginIDLength)					/* check User Login ID length, reject IF under X characters*/
  BEGIN
    SELECT 3 As ReturnValue, ('Login IDs must be at least (' + Cast(@MinLoginIDLength as varchar(2)) + ') characters long. ') As ReturnUserInformation
	Goto DONE
  END

IF (LEN(@UserLoginID) >@MaxLoginIDLength)					/* check User Login ID length, reject IF more than X characters*/
  BEGIN
    SELECT 2 As ReturnValue, ('Login IDs must be less than (' + Cast(@MinLoginIDLength as varchar(2)) + ') characters long. ') As ReturnUserInformation
	Goto DONE
  END

SELECT @ExistingUsername = UserLoginID, @UserID=PK_UserID FROM Users 
	WHERE PK_UserID = @PK_UserID							/* check for the User ID in the table - IF it exists, Dont use again*/

IF (@ExistingUsername is not null AND @UserID > 0)
  BEGIN
	SET @AddOrEdit = 1    									/* Edit 'Username already exists.' */
  END
ELSE
  BEGIN	
	SET @AddOrEdit = 0										/* Add new record*/
  END

IF @AddOrEdit = 0 											/* Validate UserPassword if new user only*/
	BEGIN
		/* Validate UserPassword  */
		Set @MinPasswordLength = (SELECT MinPasswordLength FROM UserOptions WHERE PK_UserOptionsID = 1)
		Set @MaxPasswordLength = (SELECT MaxPasswordLength FROM UserOptions WHERE PK_UserOptionsID = 1)
		
		IF @MaxPasswordLength < @MinPasswordLength 
		  BEGIN
		    SELECT 4 As ReturnValue, 'Maximum Password length is less than Minimum Password length in UserOptions.' As ReturnUserInformation
			Goto DONE
		  END
			
		IF ( LEN(@UserPassword) < @MinPasswordLength )				/* check UserPassword length, reject IF under X characters*/
		  BEGIN
		    SELECT 3 As ReturnValue, ('Passwords must be at least (' + Cast(@MinPasswordLength as varchar(2)) + ') characters long. Password will not be changed.') As ReturnUserInformation
			Goto DONE
		  END
		
		IF ( LEN(@UserPassword) >@MaxPasswordLength  )				/* check UserPassword length, reject IF more than X characters*/
		  BEGIN
		    SELECT 2 As ReturnValue, ('Passwords must be less than (' + Cast(@MaxPasswordLength as varchar(2)) + ') characters long. Password will not be changed.') As ReturnUserInformation
			Goto DONE
		  END
	END	

/* Add/Update User to Table  */
DECLARE @TranName VARCHAR(20)
SELECT @TranName = 'UpdateUserTables'
BEGIN TRANSACTION @TranName									/* put the data INTO the table */
	IF @AddOrEdit = 0 
			BEGIN

			/* Encyrpt the input UserPassword here */
			SET @UserID = (SELECT MAX(PK_USERID) FROM USERS) + 1
			IF @UserID IS NULL SET @UserID = 1				/* Snippet needed to add the 1st user */
			EXEC Secure_Encrypt @Password = @UserPassword, @SeedID=@UserID, @EncryptPassword=@EncryptedString OUTPUT

			INSERT INTO Users(	FK_AccessLevelID, 
								FK_CompanyID,
								UserLoginID, 
								UserPassword, 
								FirstName, 
								LastName, 
								Address, 
								City, 
								State, 
								ZIP, 
								PhonePrimary, 
								PhoneSecondary, 
								EmailPrimary, 
								EmailSecondary, 
								LastTrainingDate, 
								Website, 
								UserInformation, 
								Active,
								LastUpdateByName, 
								LastUpdateByDate, 
								CreateByName, 
								CreateByDate,
								SecurityQuestion , 
								SecurityAnswer, 
								FK_UserStatusID 	)
					VALUES (	@FK_AccessLevelID, 
								@FK_CompanyID,
								@UserLoginID, 
								@EncryptedString, 
								@FirstName, 
								@LastName, 
								@Address, 
								@City, 
								@State, 
								@ZIP, 
								@PhonePrimary, 
								@PhoneSecondary, 
								@EmailPrimary, 
								@EmailSecondary, 
								CAST(@LastTrainingDate as SMALLDATETIME), 
								@Website, 
								@UserInformation, 
								@Active, 
								@LastUpdateByName, 
								GetDate(), 
								@LastUpdateByName, 
								GetDate(),
								@SecurityQuestion , 
								@SecurityAnswer,
								@FK_UserStatusID 	)

			/* add the user to the UserOldPasswordCheck */
			INSERT INTO UserOldPasswordCheck (UserLoginID,OldPassword1,OldPassword2,OldPassword3,OldPassword4)
				VALUES (@@identity,@EncryptedString,@EncryptedString,@EncryptedString,@EncryptedString)
		END
	ELSE
		BEGIN		
			UPDATE Users SET 
						FK_AccessLevelID = @FK_AccessLevelID, 
						FK_CompanyID = @FK_CompanyID,
						UserLoginID = @UserLoginID, 
						/*UserPassword = @EncryptedString, */
						FirstName = @FirstName, 
						LastName = @LastName, 
						Address = @Address, 
						City = @City, 
						State = @State, 
						ZIP = @ZIP, 
						PhonePrimary = @PhonePrimary, 
						PhoneSecondary = @PhoneSecondary, 
						EmailPrimary = @EmailPrimary, 
						EmailSecondary = @EmailSecondary, 
						LastTrainingDate = CAST(@LastTrainingDate as SMALLDATETIME), 
						Website = @Website, 
						UserInformation = @UserInformation, 
						Active = @Active, 
						LastUpdateByName = @LastUpdateByName, 
						LastUpdateByDate = GetDate(),
						SecurityQuestion = @SecurityQuestion , 
						SecurityAnswer = @SecurityAnswer,
						FK_UserStatusID = @FK_UserStatusID
				FROM Users 
				WHERE PK_UserID=@PK_UserID
		END
COMMIT TRANSACTION @TranName

IF @AddOrEdit = 0 
	BEGIN
		SELECT 0 as ReturnValue, 'User Successfully Added' As ReturnUserInformation
	END
ELSE
	BEGIN
		SELECT 1 as ReturnValue, 'User Successfully Updated' As ReturnUserInformation
	END

DONE:



