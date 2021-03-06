﻿CREATE TABLE [dbo].[NewComment] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [NewID]     INT NOT NULL,
    [CommentID] INT NOT NULL,
    CONSTRAINT [PK_NewComment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NewComment_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[Comment] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_NewComment_NewComment] FOREIGN KEY ([NewID]) REFERENCES [dbo].[New] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

