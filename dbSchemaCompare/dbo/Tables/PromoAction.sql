CREATE TABLE [dbo].[PromoAction] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ProductID]   INT            NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [OnlyManager] BIT            NOT NULL,
    [Type]        INT            NOT NULL,
    [Amount]      FLOAT (53)     NOT NULL,
    [ValidDate]   DATETIME       NULL,
    [Closed]      BIT            NOT NULL,
    [Reusable]    BIT            NOT NULL,
    CONSTRAINT [PK_PromoAction] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PromoAction_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

