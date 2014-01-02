CREATE TABLE [dbo].[Vendor] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    [Text] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED ([ID] ASC)
);

