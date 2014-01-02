CREATE TABLE [dbo].[PromoCode] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [PromoActionID] INT           NOT NULL,
    [Code]          NVARCHAR (50) NOT NULL,
    [AddedDate]     DATETIME      NOT NULL,
    [UsedDate]      DATETIME      NULL,
    CONSTRAINT [PK_PromoCode] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PromoCode_PromoAction] FOREIGN KEY ([PromoActionID]) REFERENCES [dbo].[PromoAction] ([ID])
);

