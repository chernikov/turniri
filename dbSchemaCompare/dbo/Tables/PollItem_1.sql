CREATE TABLE [dbo].[PollItem] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [PollID]      INT            NOT NULL,
    [Description] NVARCHAR (500) NOT NULL,
    [CountVotes]  INT            NOT NULL,
    CONSTRAINT [PK_PollItem] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PollItem_Poll] FOREIGN KEY ([PollID]) REFERENCES [dbo].[Poll] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

