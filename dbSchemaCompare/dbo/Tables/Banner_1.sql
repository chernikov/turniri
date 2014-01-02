CREATE TABLE [dbo].[Banner] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Path]        NVARCHAR (MAX) NOT NULL,
    [Type]        INT            NOT NULL,
    [ImagePath]   NVARCHAR (150) NULL,
    [Url]         NVARCHAR (MAX) NULL,
    [IsOn]        BIT            NOT NULL,
    [TotalViews]  INT            NOT NULL,
    [TotalClicks] INT            NOT NULL,
    CONSTRAINT [PK_Banner] PRIMARY KEY CLUSTERED ([ID] ASC)
);

