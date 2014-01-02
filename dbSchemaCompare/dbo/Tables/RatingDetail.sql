CREATE TABLE [dbo].[RatingDetail] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [RatingID]     INT            NOT NULL,
    [MatchID]      INT            NULL,
    [TournamentID] INT            NULL,
    [Score]        INT            NOT NULL,
    [AddedDate]    DATETIME       NOT NULL,
    [Description]  NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_RatingDetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_RatingDetail_Match] FOREIGN KEY ([MatchID]) REFERENCES [dbo].[Match] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_RatingDetail_Rating] FOREIGN KEY ([RatingID]) REFERENCES [dbo].[Rating] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_RatingDetail_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID])
);

