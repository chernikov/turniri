CREATE TABLE [dbo].[Background] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Path]      NVARCHAR (MAX) NOT NULL,
    [ImagePath] NVARCHAR (150) NOT NULL,
    [IsOn]      BIT            NOT NULL,
    CONSTRAINT [PK_Background] PRIMARY KEY CLUSTERED ([ID] ASC)
);

