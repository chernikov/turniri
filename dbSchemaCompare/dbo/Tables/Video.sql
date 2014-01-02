CREATE TABLE [dbo].[Video] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Header]     NVARCHAR (500) NOT NULL,
    [Url]        NVARCHAR (500) NOT NULL,
    [VideoUrl]   NVARCHAR (MAX) NOT NULL,
    [VideoThumb] NVARCHAR (150) NOT NULL,
    [VideoCode]  NVARCHAR (MAX) NOT NULL,
    [Text]       NVARCHAR (MAX) NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [VisitCount] INT            NOT NULL,
    [UserID]     INT            NOT NULL,
    CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Video_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

