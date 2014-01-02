CREATE TABLE [dbo].[UserVideoComment] (
    [ID]          INT IDENTITY (1, 1) NOT NULL,
    [UserVideoID] INT NOT NULL,
    [CommentID]   INT NOT NULL,
    CONSTRAINT [PK_UserVideoComment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserVideoComment_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserVideoComment_UserVideo] FOREIGN KEY ([UserVideoID]) REFERENCES [dbo].[UserVideo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

