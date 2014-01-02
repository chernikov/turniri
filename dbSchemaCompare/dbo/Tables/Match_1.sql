﻿CREATE TABLE [dbo].[Match] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [MessageID]         INT            NULL,
    [TournamentID]      INT            NULL,
    [Participant1ID]    INT            NULL,
    [Participant2ID]    INT            NULL,
    [LeagueLevelID]     INT            NULL,
    [LeagueSeasonID]    INT            NULL,
    [Number]            INT            NOT NULL,
    [Score1]            INT            NULL,
    [Score2]            INT            NULL,
    [AddedDate]         DATETIME       NOT NULL,
    [WinnerID]          INT            NULL,
    [TournamentGroupID] INT            NULL,
    [TourID]            INT            NULL,
    [WinMatchID]        INT            NULL,
    [LoseMatchID]       INT            NULL,
    [Name]              NVARCHAR (500) NULL,
    [Place]             INT            NOT NULL,
    [Status]            INT            NOT NULL,
    [PublishedDate]     DATETIME       NULL,
    [GameID]            INT            NOT NULL,
    [Technical]         BIT            NOT NULL,
    CONSTRAINT [PK_Match] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Match_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[Game] ([ID]),
    CONSTRAINT [FK_Match_LeagueLevel] FOREIGN KEY ([LeagueLevelID]) REFERENCES [dbo].[LeagueLevel] ([ID]),
    CONSTRAINT [FK_Match_LeagueSeason] FOREIGN KEY ([LeagueSeasonID]) REFERENCES [dbo].[LeagueSeason] ([ID]),
    CONSTRAINT [FK_Match_LoseMatch] FOREIGN KEY ([LoseMatchID]) REFERENCES [dbo].[Match] ([ID]),
    CONSTRAINT [FK_Match_Message] FOREIGN KEY ([MessageID]) REFERENCES [dbo].[Message] ([ID]),
    CONSTRAINT [FK_Match_Participant1] FOREIGN KEY ([Participant1ID]) REFERENCES [dbo].[Participant] ([ID]),
    CONSTRAINT [FK_Match_Participant2] FOREIGN KEY ([Participant2ID]) REFERENCES [dbo].[Participant] ([ID]),
    CONSTRAINT [FK_Match_Tour] FOREIGN KEY ([TourID]) REFERENCES [dbo].[Tour] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Match_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]),
    CONSTRAINT [FK_Match_TournamentGroup] FOREIGN KEY ([TournamentGroupID]) REFERENCES [dbo].[TournamentGroup] ([ID]),
    CONSTRAINT [FK_Match_WinMatch] FOREIGN KEY ([WinMatchID]) REFERENCES [dbo].[Match] ([ID]),
    CONSTRAINT [FK_Match_Winner] FOREIGN KEY ([WinnerID]) REFERENCES [dbo].[Participant] ([ID])
);

