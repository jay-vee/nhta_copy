USE [NHTA2010DB]
GO
/****** Object:  StoredProcedure [dbo].[Save_Scoring]    Script Date: 02/28/2011 17:42:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER   PROCEDURE [dbo].[Save_Scoring](
	@PK_ScoringID int ,
	@FK_NominationsID int ,
	@FK_CompanyID_Adjudicator int ,
	@FK_UserID_Adjudicator int ,
	@AdjudicatorRequestsReassignment tinyint ,
	@AdjudicatorRequestsReassignmentNote varchar (4000) ,
	@AdjudicatorScoringLocked tinyint ,
	@ProductionDateAdjudicated_Planned smalldatetime ,
	@ProductionDateAdjudicated_Actual smalldatetime ,
	@BallotSubmitDate smalldatetime ,
	@DirectorScore tinyint ,
	@DirectorComment varchar (8000) ,
	@MusicalDirectorScore tinyint ,
	@MusicalDirectorComment varchar (8000) ,
	@ChoreographerScore tinyint ,
	@ChoreographerComment varchar (8000) ,
	@ScenicDesignerScore tinyint ,
	@ScenicDesignerComment varchar (8000) ,
	@LightingDesignerScore tinyint ,
	@LightingDesignerComment varchar (8000) ,
	@SoundDesignerScore tinyint ,
	@SoundDesignerComment varchar (8000) ,
	@CostumeDesignerScore tinyint ,
	@CostumeDesignerComment varchar (8000) ,
	@OriginalPlaywrightScore tinyint ,
	@OriginalPlaywrightComment varchar (8000) ,
	@BestPerformer1Score tinyint ,
	@BestPerformer1Comment varchar (8000) ,
	@BestPerformer2Score tinyint ,
	@BestPerformer2Comment varchar (8000) ,
	@BestSupportingActor1Score tinyint ,
	@BestSupportingActor1Comment varchar (8000) ,
	@BestSupportingActor2Score tinyint ,
	@BestSupportingActor2Comment varchar (8000) ,
	@BestSupportingActress1Score tinyint ,
	@BestSupportingActress1Comment varchar (8000) ,
	@BestSupportingActress2Score tinyint ,
	@BestSupportingActress2Comment varchar (8000) ,
	@BestProductionScore tinyint ,
	@BestProductionComment varchar (8000) ,
	@LastUpdateByName varchar (50) ,
	@ReserveAdjudicator tinyint,
	@FK_ScoringStatusID int ,
	@AdjudicatorAttendanceComment varchar (4000) ,
	@NonScoringUpdate TINYINT = 0,
	@FoundAdvertisement TINYINT = 0 ) 
AS 
/* 
Object:          Save_Scoring
Description:     Saves (Insert or Update) Scoring data
Returns:         errorvalue
Created By:      Joe Vago
Example:         EXEC Save_Scoring....
Created Date:    6/7/07
Modified By:     Joe Vago 
Modified Date:   10/6/2008
NOTES: 			Added @NonScoringUpdate field to allow Admin EDIT updates without resetting the scores.
				12/2009: added 2 fields: FK_ScoringStatusID; AdjudicatorAttendanceComment
 */ 
SET NOCOUNT ON

IF (@PK_ScoringID > 0)
	BEGIN
		IF @NonScoringUpdate = 0 
			BEGIN
				UPDATE Scoring
				SET FK_NominationsID = @FK_NominationsID,
					FK_CompanyID_Adjudicator = @FK_CompanyID_Adjudicator,
					FK_UserID_Adjudicator = @FK_UserID_Adjudicator,
					FK_ScoringStatusID = @FK_ScoringStatusID ,
					AdjudicatorAttendanceComment = @AdjudicatorAttendanceComment, 
					AdjudicatorRequestsReassignment = @AdjudicatorRequestsReassignment,
					AdjudicatorRequestsReassignmentNote = @AdjudicatorRequestsReassignmentNote,
					AdjudicatorScoringLocked = @AdjudicatorScoringLocked,
					ProductionDateAdjudicated_Planned = @ProductionDateAdjudicated_Planned,
					ProductionDateAdjudicated_Actual = @ProductionDateAdjudicated_Actual,
					BallotSubmitDate = @BallotSubmitDate, 
					DirectorScore = @DirectorScore,
					DirectorComment = @DirectorComment,
					MusicalDirectorScore = @MusicalDirectorScore,
					MusicalDirectorComment = @MusicalDirectorComment,
					ChoreographerScore = @ChoreographerScore,
					ChoreographerComment = @ChoreographerComment,
					ScenicDesignerScore = @ScenicDesignerScore,
					ScenicDesignerComment = @ScenicDesignerComment,
					LightingDesignerScore = @LightingDesignerScore,
					LightingDesignerComment = @LightingDesignerComment,
					SoundDesignerScore = @SoundDesignerScore,
					SoundDesignerComment = @SoundDesignerComment,
					CostumeDesignerScore = @CostumeDesignerScore,
					CostumeDesignerComment = @CostumeDesignerComment,
					OriginalPlaywrightScore = @OriginalPlaywrightScore,
					OriginalPlaywrightComment = @OriginalPlaywrightComment,
					BestPerformer1Score = @BestPerformer1Score,
					BestPerformer1Comment = @BestPerformer1Comment,
					BestPerformer2Score = @BestPerformer2Score,
					BestPerformer2Comment = @BestPerformer2Comment,
					BestSupportingActor1Score = @BestSupportingActor1Score,
					BestSupportingActor1Comment = @BestSupportingActor1Comment,
					BestSupportingActor2Score = @BestSupportingActor2Score,
					BestSupportingActor2Comment = @BestSupportingActor2Comment,
					BestSupportingActress1Score = @BestSupportingActress1Score,
					BestSupportingActress1Comment = @BestSupportingActress1Comment,
					BestSupportingActress2Score = @BestSupportingActress2Score,
					BestSupportingActress2Comment = @BestSupportingActress2Comment,
					BestProductionScore = @BestProductionScore,
					BestProductionComment = @BestProductionComment,
					LastUpdateByName = @LastUpdateByName,
					LastUpdateByDate = GetDate(),
					ReserveAdjudicator = @ReserveAdjudicator,
					FoundAdvertisement = @FoundAdvertisement
				WHERE PK_ScoringID = @PK_ScoringID
			END
		ELSE
BEGIN
				UPDATE Scoring
				SET FK_NominationsID = @FK_NominationsID,
					FK_CompanyID_Adjudicator = @FK_CompanyID_Adjudicator,
					FK_UserID_Adjudicator = @FK_UserID_Adjudicator,
					FK_ScoringStatusID = @FK_ScoringStatusID ,
					AdjudicatorAttendanceComment = @AdjudicatorAttendanceComment, 
					AdjudicatorRequestsReassignment = @AdjudicatorRequestsReassignment,
					AdjudicatorRequestsReassignmentNote = @AdjudicatorRequestsReassignmentNote,
					AdjudicatorScoringLocked = @AdjudicatorScoringLocked,
					ProductionDateAdjudicated_Planned = @ProductionDateAdjudicated_Planned,
					ProductionDateAdjudicated_Actual = @ProductionDateAdjudicated_Actual,
					LastUpdateByName = @LastUpdateByName,
					LastUpdateByDate = GetDate(),
					ReserveAdjudicator = @ReserveAdjudicator
				WHERE PK_ScoringID = @PK_ScoringID
			END		
	END
ELSE
	BEGIN
		INSERT INTO Scoring(
			FK_NominationsID,
			FK_CompanyID_Adjudicator,
			FK_UserID_Adjudicator,
			FK_ScoringStatusID,
			AdjudicatorAttendanceComment,
			AdjudicatorRequestsReassignment,
			AdjudicatorRequestsReassignmentNote, 
			AdjudicatorScoringLocked,
			ProductionDateAdjudicated_Planned,
			ProductionDateAdjudicated_Actual,
			BallotSubmitDate,
			DirectorScore,
			DirectorComment,
			MusicalDirectorScore,
			MusicalDirectorComment,
			ChoreographerScore,
			ChoreographerComment,
			ScenicDesignerScore,
			ScenicDesignerComment,
			LightingDesignerScore,
			LightingDesignerComment,
			SoundDesignerScore,
			SoundDesignerComment,
			CostumeDesignerScore,
			CostumeDesignerComment,
			OriginalPlaywrightScore,
			OriginalPlaywrightComment,
			BestPerformer1Score,
			BestPerformer1Comment,
			BestPerformer2Score,
			BestPerformer2Comment,
			BestSupportingActor1Score,
			BestSupportingActor1Comment,
			BestSupportingActor2Score,
			BestSupportingActor2Comment,
			BestSupportingActress1Score,
			BestSupportingActress1Comment,
			BestSupportingActress2Score,
			BestSupportingActress2Comment,
			BestProductionScore,
			BestProductionComment,
			LastUpdateByName,
			CreateByName,
			ReserveAdjudicator,
			FoundAdvertisement)
		VALUES (
			@FK_NominationsID,
			@FK_CompanyID_Adjudicator,
			@FK_UserID_Adjudicator,
			@FK_ScoringStatusID,
			@AdjudicatorAttendanceComment,
			@AdjudicatorRequestsReassignment,
			@AdjudicatorRequestsReassignmentNote,
			@AdjudicatorScoringLocked,
			@ProductionDateAdjudicated_Planned,
			@ProductionDateAdjudicated_Actual,
			@BallotSubmitDate, 
			@DirectorScore,
			@DirectorComment,
			@MusicalDirectorScore,
			@MusicalDirectorComment,
			@ChoreographerScore,
			@ChoreographerComment,
			@ScenicDesignerScore,
			@ScenicDesignerComment,
			@LightingDesignerScore,
			@LightingDesignerComment,
			@SoundDesignerScore,
			@SoundDesignerComment,
			@CostumeDesignerScore,
			@CostumeDesignerComment,
			@OriginalPlaywrightScore,
			@OriginalPlaywrightComment,
			@BestPerformer1Score,
			@BestPerformer1Comment,
			@BestPerformer2Score,
			@BestPerformer2Comment,
			@BestSupportingActor1Score,
			@BestSupportingActor1Comment,
			@BestSupportingActor2Score,
			@BestSupportingActor2Comment,
			@BestSupportingActress1Score,
			@BestSupportingActress1Comment,
			@BestSupportingActress2Score,
			@BestSupportingActress2Comment,
			@BestProductionScore,
			@BestProductionComment,
			@LastUpdateByName,
			@LastUpdateByName,
			@ReserveAdjudicator,
			@FoundAdvertisement )
	END



