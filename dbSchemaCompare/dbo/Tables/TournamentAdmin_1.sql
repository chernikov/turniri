CREATE TABLE [dbo].[TournamentAdmin] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [TournamentID] INT NOT NULL,
    [UserID]       INT NOT NULL,
    CONSTRAINT [PK_TournamentAdmin] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TournamentAdmin_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TournamentAdmin_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

