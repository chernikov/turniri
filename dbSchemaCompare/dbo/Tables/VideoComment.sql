CREATE TABLE [dbo].[VideoComment] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [VideoID]   INT NOT NULL,
    [CommentID] INT NOT NULL,
    CONSTRAINT [PK_VideoComment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_VideoComment_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]),
    CONSTRAINT [FK_VideoComment_Video] FOREIGN KEY ([VideoID]) REFERENCES [dbo].[Video] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

