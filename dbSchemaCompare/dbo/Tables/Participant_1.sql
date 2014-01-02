CREATE TABLE [dbo].[Participant] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [TournamentID]      INT            NULL,
    [UserID]            INT            NOT NULL,
    [TeamID]            INT            NULL,
    [TournamentGroupID] INT            NULL,
    [AddedDate]         DATETIME       NOT NULL,
    [Name]              NVARCHAR (500) NULL,
    [ImagePath18]       NVARCHAR (150) NULL,
    [ImagePath26]       NVARCHAR (150) NULL,
    [ImagePath30]       NVARCHAR (150) NULL,
    CONSTRAINT [PK_Participant] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Participant_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]),
    CONSTRAINT [FK_Participant_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Participant_TournamentGroup] FOREIGN KEY ([TournamentGroupID]) REFERENCES [dbo].[TournamentGroup] ([ID]),
    CONSTRAINT [FK_Participant_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

