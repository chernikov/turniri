﻿CREATE TABLE [dbo].[BlogLike] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [BlogID] INT NOT NULL,
    [UserID] INT NOT NULL,
    CONSTRAINT [PK_BlogLike] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BlogLike_Blog] FOREIGN KEY ([BlogID]) REFERENCES [dbo].[Blog] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_BlogLike_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);
