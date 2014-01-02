CREATE TABLE [dbo].[Platform] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    [Url]  NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Platform] PRIMARY KEY CLUSTERED ([ID] ASC)
);

