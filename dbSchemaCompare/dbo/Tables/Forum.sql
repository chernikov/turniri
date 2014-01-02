CREATE TABLE [dbo].[Forum] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [ParentID]   INT            NULL,
    [UserID]     INT            NULL,
    [Name]       NVARCHAR (500) NOT NULL,
    [Url]        NVARCHAR (500) NOT NULL,
    [ImagePath]  NVARCHAR (150) NULL,
    [SubTitle]   NVARCHAR (MAX) NULL,
    [IsDeleted]  BIT            NOT NULL,
    [IsEnd]      BIT            NOT NULL,
    [VisitCount] INT            NOT NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [LastUpdate] DATETIME       NULL,
    [PollID]     INT            NULL,
    [OrderBy]    INT            NOT NULL,
    CONSTRAINT [PK_Forum] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Forum_Parent] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Forum] ([ID]),
    CONSTRAINT [FK_Forum_Poll] FOREIGN KEY ([PollID]) REFERENCES [dbo].[Poll] ([ID]),
    CONSTRAINT [FK_Forum_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

