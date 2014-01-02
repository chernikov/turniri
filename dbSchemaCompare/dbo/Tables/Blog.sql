CREATE TABLE [dbo].[Blog] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserID]             INT            NOT NULL,
    [GroupID]            INT            NULL,
    [Header]             NVARCHAR (500) NOT NULL,
    [Url]                NVARCHAR (500) NOT NULL,
    [Text]               NVARCHAR (MAX) NOT NULL,
    [PreviewUrl]         NVARCHAR (150) NULL,
    [CommentsCount]      INT            NOT NULL,
    [AddedDate]          DATETIME       NOT NULL,
    [LastModificateDate] DATETIME       NOT NULL,
    [VisitCount]         INT            NOT NULL,
    [IsBanned]           BIT            NOT NULL,
    [BanDescription]     NVARCHAR (500) NULL,
    [ShowInMain]         BIT            NOT NULL,
    [Likes]              INT            NOT NULL,
    CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Blog_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_Blog_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

