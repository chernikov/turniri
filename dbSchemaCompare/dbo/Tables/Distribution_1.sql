CREATE TABLE [dbo].[Distribution] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [UserID]    INT            NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [Subject]   NVARCHAR (500) NOT NULL,
    [Body]      NVARCHAR (MAX) NOT NULL,
    [Name]      NVARCHAR (500) NOT NULL,
    [IsStart]   BIT            NOT NULL,
    CONSTRAINT [PK_Distribution] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Distribution_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

