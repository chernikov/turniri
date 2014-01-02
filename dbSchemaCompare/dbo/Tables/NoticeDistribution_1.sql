CREATE TABLE [dbo].[NoticeDistribution] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [UserID]         INT            NOT NULL,
    [AddedDate]      DATETIME       NOT NULL,
    [Caption]        NVARCHAR (500) NULL,
    [Text]           NVARCHAR (MAX) NOT NULL,
    [IsCloseForRead] BIT            NOT NULL,
    CONSTRAINT [PK_NoticeDistirbution] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NoticeDistirbution_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

