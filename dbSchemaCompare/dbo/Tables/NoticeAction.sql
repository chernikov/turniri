CREATE TABLE [dbo].[NoticeAction] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [NoticeID]        INT            NOT NULL,
    [ActionUrl]       NVARCHAR (500) NOT NULL,
    [Name]            NVARCHAR (500) NOT NULL,
    [Direct]          BIT            NOT NULL,
    [IsResolveNotice] BIT            NOT NULL,
    [IsRunNotice]     BIT            NOT NULL,
    CONSTRAINT [PK_NoticeAction] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NoticeAction_Notice] FOREIGN KEY ([NoticeID]) REFERENCES [dbo].[Notice] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

