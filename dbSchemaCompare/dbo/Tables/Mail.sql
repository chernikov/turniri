CREATE TABLE [dbo].[Mail] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [DistributionID] INT              NOT NULL,
    [UserID]         INT              NOT NULL,
    [Email]          NVARCHAR (500)   NOT NULL,
    [AddedDate]      DATETIME         NOT NULL,
    [Subject]        NVARCHAR (500)   NULL,
    [Body]           NVARCHAR (MAX)   NULL,
    [Delivered]      BIT              NOT NULL,
    CONSTRAINT [PK_Mail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Mail_Distribution] FOREIGN KEY ([DistributionID]) REFERENCES [dbo].[Distribution] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Mail_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

