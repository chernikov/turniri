CREATE TABLE [dbo].[Team] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (500) NOT NULL,
    [ImagePath18]    NVARCHAR (150) NULL,
    [ImagePath26]    NVARCHAR (150) NULL,
    [ImagePath30]    NVARCHAR (150) NULL,
    [HotReplacement] INT            NULL,
    [AddedDate]      DATETIME       NOT NULL,
    [IsClosed]       BIT            NOT NULL,
    CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED ([ID] ASC)
);

