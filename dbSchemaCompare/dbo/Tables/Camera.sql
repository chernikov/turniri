CREATE TABLE [dbo].[Camera] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [TournamentID] INT            NOT NULL,
    [MatchID]      INT            NULL,
    [Name]         NVARCHAR (500) NOT NULL,
    [Code]         NVARCHAR (MAX) NOT NULL,
    [AddedDate]    DATETIME       NOT NULL,
    [Enabled]      BIT            NOT NULL,
    CONSTRAINT [PK_Camera] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Camera_Match] FOREIGN KEY ([MatchID]) REFERENCES [dbo].[Match] ([ID]),
    CONSTRAINT [FK_Camera_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

