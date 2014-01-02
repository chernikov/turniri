CREATE TABLE [dbo].[MoneyFee] (
    [ID]         INT        IDENTITY (1, 1) NOT NULL,
    [Type]       INT        NOT NULL,
    [PercentFee] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_MoneyFee] PRIMARY KEY CLUSTERED ([ID] ASC)
);

