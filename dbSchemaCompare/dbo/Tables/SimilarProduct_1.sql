CREATE TABLE [dbo].[SimilarProduct] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [ProductID]        INT NOT NULL,
    [SimilarProductID] INT NOT NULL,
    CONSTRAINT [PK_SimilarProduct] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SimilarProduct_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_SimilarProduct_SimilarProduct] FOREIGN KEY ([SimilarProductID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

