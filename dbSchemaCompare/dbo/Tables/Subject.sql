CREATE TABLE [dbo].[Subject] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED ([ID] ASC)
);

