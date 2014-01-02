CREATE TABLE [dbo].[Recharge] (
    [ID]             INT              IDENTITY (1, 1) NOT NULL,
    [UserID]         INT              NULL,
    [GlobalUniqueID] UNIQUEIDENTIFIER NULL,
    [MoneyDetailID]  INT              NULL,
    [CartID]         INT              NULL,
    [AddedDate]      DATETIME         NOT NULL,
    [Sum]            FLOAT (53)       NOT NULL,
    [IsSubmitted]    BIT              NOT NULL,
    [Description]    NVARCHAR (500)   NOT NULL,
    [Provider]       INT              NOT NULL,
    [AdditionalInfo] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Recharge] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Recharge_Cart] FOREIGN KEY ([CartID]) REFERENCES [dbo].[Cart] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Recharge_GlobalUnique] FOREIGN KEY ([GlobalUniqueID]) REFERENCES [dbo].[GlobalUnique] ([ID]),
    CONSTRAINT [FK_Recharge_MoneyDetail] FOREIGN KEY ([MoneyDetailID]) REFERENCES [dbo].[MoneyDetail] ([ID]),
    CONSTRAINT [FK_Recharge_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

