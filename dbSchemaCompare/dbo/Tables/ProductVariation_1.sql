CREATE TABLE [dbo].[ProductVariation] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [ProductID] INT            NOT NULL,
    [Image]     NVARCHAR (150) NULL,
    [IsDeleted] BIT            NOT NULL,
    CONSTRAINT [PK_ProductVariation] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductVariation_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

