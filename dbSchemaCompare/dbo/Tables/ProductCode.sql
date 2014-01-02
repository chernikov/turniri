CREATE TABLE [dbo].[ProductCode] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]          INT            NOT NULL,
    [CartProductID]      INT            NULL,
    [Code]               NVARCHAR (500) NOT NULL,
    [Image]              NVARCHAR (150) NULL,
    [AddedDate]          DATETIME       NOT NULL,
    [IsSelled]           BIT            NOT NULL,
    [ProductPriceID]     INT            NOT NULL,
    [ProductVariationID] INT            NULL,
    CONSTRAINT [PK_ProductCode] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductCode_CartProduct] FOREIGN KEY ([CartProductID]) REFERENCES [dbo].[CartProduct] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_ProductCode_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductCode_ProductPrice] FOREIGN KEY ([ProductPriceID]) REFERENCES [dbo].[ProductPrice] ([ID]),
    CONSTRAINT [FK_ProductCode_ProductVariation] FOREIGN KEY ([ProductVariationID]) REFERENCES [dbo].[ProductVariation] ([ID])
);

