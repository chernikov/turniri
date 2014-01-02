CREATE TABLE [dbo].[Comment] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ParentID]       INT            NULL,
    [UserID]         INT            NOT NULL,
    [Text]           NVARCHAR (MAX) NOT NULL,
    [VideoUrl]       NVARCHAR (500) NULL,
    [VideoCode]      NVARCHAR (MAX) NULL,
    [ImagePath]      NVARCHAR (150) NULL,
    [AddedDate]      DATETIME       NOT NULL,
    [IsBanned]       BIT            NOT NULL,
    [BanDescription] NVARCHAR (500) NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Comment_Comment] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Comment] ([ID]),
    CONSTRAINT [FK_Comment_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

