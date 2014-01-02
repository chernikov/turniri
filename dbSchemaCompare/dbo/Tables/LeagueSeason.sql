CREATE TABLE [dbo].[LeagueSeason] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [LeagueID]        INT            NOT NULL,
    [Image]           NVARCHAR (150) NULL,
    [StartDate]       DATETIME       NOT NULL,
    [EndMainTourDate] DATETIME       NOT NULL,
    [EndDate]         DATETIME       NOT NULL,
    [Name]            NVARCHAR (500) NOT NULL,
    [Status]          INT            NOT NULL,
    CONSTRAINT [PK_LeagueSeason] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LeagueSeason_League] FOREIGN KEY ([LeagueID]) REFERENCES [dbo].[League] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

