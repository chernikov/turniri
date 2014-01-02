CREATE TABLE [dbo].[BalanceSiteMoney] (
    [ID]             INT        IDENTITY (1, 1) NOT NULL,
    [MoneyGold]      FLOAT (53) NOT NULL,
    [LastUpdateDate] DATETIME   NOT NULL,
    CONSTRAINT [PK_BalanceSiteMoney] PRIMARY KEY CLUSTERED ([ID] ASC)
);

