CREATE TABLE [dbo].[Catalog] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

