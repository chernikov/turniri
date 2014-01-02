CREATE TABLE [dbo].[CartProduct] (
    [ID]                 INT        IDENTITY (1, 1) NOT NULL,
    [CartID]             INT        NOT NULL,
    [ProductID]          INT        NOT NULL,
    [PromoCodeID]        INT        NULL,
    [ProductVariationID] INT        NULL,
    [Quantity]           INT        NOT NULL,
    [IsFree]             BIT        NOT NULL,
    [ProductPriceID]     INT        NOT NULL,
    [Price]              FLOAT (53) NOT NULL,
    CONSTRAINT [PK_CartProduct] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CartProduct_Cart] FOREIGN KEY ([CartID]) REFERENCES [dbo].[Cart] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_CartProduct_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_CartProduct_ProductPrice] FOREIGN KEY ([ProductPriceID]) REFERENCES [dbo].[ProductPrice] ([ID]),
    CONSTRAINT [FK_CartProduct_ProductVariation] FOREIGN KEY ([ProductVariationID]) REFERENCES [dbo].[ProductVariation] ([ID]),
    CONSTRAINT [FK_CartProduct_PromoCode] FOREIGN KEY ([PromoCodeID]) REFERENCES [dbo].[PromoCode] ([ID])
);

