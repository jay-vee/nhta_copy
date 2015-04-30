UPDATE    Scoring
SET              DirectorScore = 0, MusicalDirectorScore = 0, ChoreographerScore = 0, ScenicDesignerScore = 0, LightingDesignerScore = 0, SoundDesignerScore = 0, 
                      CostumeDesignerScore = 0, OriginalPlaywrightScore = 0, BestPerformer1Score = 0, BestPerformer2Score = 0, BestSupportingActor1Score = 0, 
                      BestSupportingActor2Score = 0, BestSupportingActress1Score = 0, BestSupportingActress2Score = 0, BestProductionScore = 0
WHERE     (FK_NominationsID = 306)