CREATE TABLE [dbo].[LeagueParticipant] (
    [ID]             INT IDENTITY (1, 1) NOT NULL,
    [LeagueSeasonID] INT NOT NULL,
    [LeagueLevelID]  INT NOT NULL,
    [ParticipantID]  INT NOT NULL,
    [Place]          INT NULL,
    CONSTRAINT [PK_LeagueParticipant] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LeagueParticipant_LeagueLevel] FOREIGN KEY ([LeagueLevelID]) REFERENCES [dbo].[LeagueLevel] ([ID]),
    CONSTRAINT [FK_LeagueParticipant_LeagueSeason] FOREIGN KEY ([LeagueSeasonID]) REFERENCES [dbo].[LeagueSeason] ([ID]),
    CONSTRAINT [FK_LeagueParticipant_Participant] FOREIGN KEY ([ParticipantID]) REFERENCES [dbo].[Participant] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

