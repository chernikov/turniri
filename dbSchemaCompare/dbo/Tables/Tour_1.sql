CREATE TABLE [dbo].[Tour] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [TournamentID]  INT            NOT NULL,
    [TourType]      INT            NOT NULL,
    [RecommendDate] DATETIME       NULL,
    [Name]          NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_Tour] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Tour_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

