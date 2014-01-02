CREATE TABLE [dbo].[ProductPrice] (
    [ID]         INT        IDENTITY (1, 1) NOT NULL,
    [ProductID]  INT        NOT NULL,
    [PlatformID] INT        NULL,
    [Price]      FLOAT (53) NOT NULL,
    [Preorder]   BIT        NOT NULL,
    [IsDeleted]  BIT        NOT NULL,
    [OldPrice]   FLOAT (53) NULL,
    CONSTRAINT [PK_ProductPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductPrice_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[Platform] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ProductPrice_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

