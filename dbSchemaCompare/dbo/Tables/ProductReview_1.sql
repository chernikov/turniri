CREATE TABLE [dbo].[ProductReview] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [ProductID] INT            NOT NULL,
    [UserID]    INT            NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProductReview] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ProductReview_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]),
    CONSTRAINT [FK_ProductReview_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

