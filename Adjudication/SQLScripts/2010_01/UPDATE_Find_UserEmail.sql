set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER   Procedure [dbo].[Find_UserEmail] @EmailAddress varchar(200)
	AS 
/*
Object:  		Find_UserEmail
Description:  	
Usage:  		Find_UserEmail @EmailAddress="Person@Email.com"
Returns:  		ReturnValue, ReturnUserInformation, ExistingUsername, LoginID
Created By:  	Joe Vago   Email: JoeVago@NHTheatreAwards.org
Created:  		09/26/2007
Modified:		

Declare @EmailAddress varchar(200)
SET @EmailAddress = 'JoeVago@NHTheatreAwards.org'
*/
SET NOCOUNT ON

/* holds the data extracted FROM the USER table*/
Declare @ExistingUsername varchar(200)						
Declare @ExistingUserID INT, @NumUsers INT
Declare @ExistingLoginID varchar(200) 						
Declare @SecurityQuestion varchar(250), @SecurityAnswer varchar(100)

/* check for the Email Address in the table - */
SELECT @ExistingLoginID = UserLoginID, @ExistingUsername = Firstname + ' ' + LastName, @ExistingUserID=PK_UserID,
	   @NumUsers = (Select Count(*) as NumUsers FROM USERS WHERE EmailPrimary = @EmailAddress OR EmailSecondary = @EmailAddress) ,
		@SecurityQuestion = SecurityQuestion, @SecurityAnswer = SecurityAnswer
	FROM USERS 
	WHERE EmailPrimary = @EmailAddress OR EmailSecondary = @EmailAddress
	Group By UserLoginID, Firstname + ' ' + LastName, PK_UserID, SecurityQuestion, SecurityAnswer

IF (@ExistingLoginID is null)
	BEGIN
		SELECT 9 As ReturnValue, 'FAILURE: Email Address was *not* found in the system.' As ReturnUserInformation, Null as Username, Null as LoginID
	END
ELSE
	BEGIN
		IF @NumUsers = 1 
			BEGIN
				SELECT 0 As ReturnValue, 'SUCCESS: Email Address was found in the system!' As ReturnUserInformation, @ExistingUsername as ExistingUsername, @ExistingLoginID as LoginID, @SecurityQuestion as SecurityQuestion, @SecurityAnswer as SecurityAnswer
			END
		ELSE
			BEGIN
				SELECT 1 As ReturnValue, 'WARNING: ' + CAST(@NumUsers as VARCHAR(3)) + ' Logins with this email address in the system!' As ReturnUserInformation, @ExistingUsername as ExistingUsername, @ExistingLoginID as LoginID, @SecurityQuestion as SecurityQuestion, @SecurityAnswer as SecurityAnswer
			END
	END



