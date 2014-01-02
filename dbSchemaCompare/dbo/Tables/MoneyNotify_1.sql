CREATE TABLE [dbo].[MoneyNotify] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Type]      INT            NOT NULL,
    [Data]      NVARCHAR (MAX) NOT NULL,
    [AddedDate] DATETIME       NOT NULL,
    [IsSuccess] BIT            NULL,
    [Exception] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_MoneyNotify] PRIMARY KEY CLUSTERED ([ID] ASC)
);

