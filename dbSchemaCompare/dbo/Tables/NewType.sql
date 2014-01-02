CREATE TABLE [dbo].[NewType] (
    [ID]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_NewType] PRIMARY KEY CLUSTERED ([ID] ASC)
);

