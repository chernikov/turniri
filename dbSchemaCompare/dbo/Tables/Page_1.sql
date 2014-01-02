CREATE TABLE [dbo].[Page] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500) NOT NULL,
    [Url]         NVARCHAR (500) NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Keywords]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([ID] ASC)
);

