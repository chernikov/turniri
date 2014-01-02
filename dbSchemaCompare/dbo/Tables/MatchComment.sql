CREATE TABLE [dbo].[MatchComment] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [MatchID]   INT NOT NULL,
    [CommentID] INT NOT NULL,
    CONSTRAINT [PK_MatchComment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MatchComment_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_MatchComment_Match] FOREIGN KEY ([MatchID]) REFERENCES [dbo].[Match] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

