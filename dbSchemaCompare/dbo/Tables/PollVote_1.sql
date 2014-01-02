CREATE TABLE [dbo].[PollVote] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [PollID]     INT      NOT NULL,
    [PollItemID] INT      NULL,
    [UserID]     INT      NOT NULL,
    [AddedDate]  DATETIME NOT NULL,
    CONSTRAINT [PK_PollVote] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PollVote_Poll] FOREIGN KEY ([PollID]) REFERENCES [dbo].[Poll] ([ID]),
    CONSTRAINT [FK_PollVote_PollItem] FOREIGN KEY ([PollItemID]) REFERENCES [dbo].[PollItem] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PollVote_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

