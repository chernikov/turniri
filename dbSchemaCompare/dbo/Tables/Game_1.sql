﻿CREATE TABLE [dbo].[Game] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [PlatformID]      INT            NOT NULL,
    [Name]            NVARCHAR (500) NOT NULL,
    [Url]             NVARCHAR (500) NOT NULL,
    [Description]     NVARCHAR (MAX) NOT NULL,
    [ImagePath189]    NVARCHAR (100) NOT NULL,
    [ImagePath103]    NVARCHAR (100) NOT NULL,
    [ImagePath144v]   NVARCHAR (100) NOT NULL,
    [ImagePath47]     NVARCHAR (100) NOT NULL,
    [ImagePath22]     NVARCHAR (100) NOT NULL,
    [HowToPlay]       NVARCHAR (MAX) NULL,
    [IsMain]          BIT            NOT NULL,
    [GameType]        INT            NOT NULL,
    [ForumID]         INT            NULL,
    [GameCategory]    INT            NOT NULL,
    [IsCommand]       BIT            NOT NULL,
    [MaxCountPlayer]  INT            NULL,
    [Keywords]        NVARCHAR (MAX) NULL,
    [MetaDescription] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Game_Forum] FOREIGN KEY ([ForumID]) REFERENCES [dbo].[Forum] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_Game_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[Platform] ([ID])
);
