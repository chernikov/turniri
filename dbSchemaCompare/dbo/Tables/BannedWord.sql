CREATE TABLE [dbo].[BannedWord] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [Word]           NVARCHAR (MAX) NOT NULL,
    [IsCanBeSubWord] BIT            NOT NULL,
    CONSTRAINT [PK_BannedWord] PRIMARY KEY CLUSTERED ([ID] ASC)
);

