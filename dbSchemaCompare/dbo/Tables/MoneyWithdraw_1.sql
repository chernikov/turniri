CREATE TABLE [dbo].[MoneyWithdraw] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [UserID]        INT            NOT NULL,
    [MoneyDetailID] INT            NULL,
    [Sum]           FLOAT (53)     NOT NULL,
    [Provider]      INT            NOT NULL,
    [Account]       NVARCHAR (500) NOT NULL,
    [AddedDate]     DATETIME       NOT NULL,
    [Submitted]     BIT            NOT NULL,
    CONSTRAINT [PK_MoneyWithdraw] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MoneyWithdraw_MoneyDetail] FOREIGN KEY ([MoneyDetailID]) REFERENCES [dbo].[MoneyDetail] ([ID]),
    CONSTRAINT [FK_MoneyWithdraw_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

