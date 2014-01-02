CREATE TABLE [dbo].[Poll] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [PollType]  INT            NOT NULL,
    [IsClosed]  BIT            NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Poll] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Poll_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

