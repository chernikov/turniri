CREATE TABLE [dbo].[New] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [NewTypeID]         INT             NOT NULL,
    [IsMain]            BIT             NOT NULL,
    [PreviewPath]       NVARCHAR (150)  NULL,
    [AvatarPreviewPath] NVARCHAR (150)  NULL,
    [TitlePath]         NVARCHAR (150)  NULL,
    [AvatarTitlePath]   NVARCHAR (150)  NULL,
    [Header]            NVARCHAR (500)  NOT NULL,
    [Url]               NVARCHAR (500)  NOT NULL,
    [SubHeader]         NVARCHAR (1000) NULL,
    [Text]              NVARCHAR (MAX)  NOT NULL,
    [AddedDate]         DATETIME        NOT NULL,
    [VisitCount]        INT             NOT NULL,
    [UserID]            INT             NOT NULL,
    [Likes]             INT             NOT NULL,
    CONSTRAINT [PK_New] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_New_NewType] FOREIGN KEY ([NewTypeID]) REFERENCES [dbo].[NewType] ([ID]),
    CONSTRAINT [FK_New_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

