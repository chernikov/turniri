CREATE TABLE [dbo].[UserVideo] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [UserID]     INT            NOT NULL,
    [GroupID]    INT            NULL,
    [Header]     NVARCHAR (500) NOT NULL,
    [Url]        NVARCHAR (500) NOT NULL,
    [VideoThumb] NVARCHAR (150) NOT NULL,
    [VideoCode]  NVARCHAR (MAX) NOT NULL,
    [VideoUrl]   NVARCHAR (MAX) NOT NULL,
    [Text]       NVARCHAR (MAX) NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [VisitCount] INT            NOT NULL,
    CONSTRAINT [PK_UserVideo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserVideo_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_UserVideo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

