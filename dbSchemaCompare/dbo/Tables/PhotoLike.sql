﻿CREATE TABLE [dbo].[PhotoLike] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [PhotoID] INT NOT NULL,
    [UserID]  INT NOT NULL,
    CONSTRAINT [PK_PhotoLike] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PhotoLike_Photo] FOREIGN KEY ([PhotoID]) REFERENCES [dbo].[Photo] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PhotoLike_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

