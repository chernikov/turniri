CREATE TABLE [dbo].[GlobalUnique] (
    [ID]        UNIQUEIDENTIFIER NOT NULL,
    [AddedDate] DATETIME         NOT NULL,
    [LastDate]  DATETIME         NOT NULL,
    [IP]        NVARCHAR (50)    NULL,
    [UserAgent] NVARCHAR (500)   NULL,
    CONSTRAINT [PK_GlobalUnique] PRIMARY KEY CLUSTERED ([ID] ASC)
);

