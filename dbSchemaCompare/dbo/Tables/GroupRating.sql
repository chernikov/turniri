CREATE TABLE [dbo].[GroupRating] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]     INT            NOT NULL,
    [MatchID]     INT            NOT NULL,
    [Score]       INT            NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [Description] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_GroupRating] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_GroupRating_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_GroupRating_Match] FOREIGN KEY ([MatchID]) REFERENCES [dbo].[Match] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

