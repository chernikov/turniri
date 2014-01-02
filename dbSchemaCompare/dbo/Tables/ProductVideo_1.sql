CREATE TABLE [dbo].[ProductVideo] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]  INT            NOT NULL,
    [VideoUrl]   NVARCHAR (MAX) NOT NULL,
    [VideoThumb] NVARCHAR (150) NOT NULL,
    [VideoCode]  NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProductVideo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductVideo_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

