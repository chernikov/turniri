CREATE TABLE [dbo].[ProductImage] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [ProductID] INT            NOT NULL,
    [Image]     NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_ProductImage] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductImage_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

