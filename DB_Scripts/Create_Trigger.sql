USE [PerceptiveARR]
GO
    
    CREATE TRIGGER [dbo].[ObjectIdentifier_Delete_On_ParticipantObjectContainsStudy]
    ON [dbo].[ParticipantObjectContainsStudy]
    FOR DELETE
AS
    DELETE FROM [dbo].[ObjectIdentifier]
    WHERE [dbo].[ObjectIdentifier].[ParticipantObjectContainsStudyID] IN
    (SELECT deleted.ParticipantObjectContainsStudyID FROM deleted)
GO