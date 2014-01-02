CREATE TABLE [dbo].[ProductCatalog] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [ProductID] INT NOT NULL,
    [CatalogID] INT NOT NULL,
    CONSTRAINT [PK_ProductCatalog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductCatalog_Catalog] FOREIGN KEY ([CatalogID]) REFERENCES [dbo].[Catalog] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ProductCatalog_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

