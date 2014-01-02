CREATE TABLE [dbo].[PhotoComment] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [PhotoID]   INT NOT NULL,
    [CommentID] INT NOT NULL,
    CONSTRAINT [PK_PhotoComment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PhotoComment_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PhotoComment_Photo] FOREIGN KEY ([PhotoID]) REFERENCES [dbo].[Photo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

