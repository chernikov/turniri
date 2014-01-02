CREATE TABLE [dbo].[TournamentGroup] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [TournamentID] INT           NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TournamentGroup] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TournamentGroup_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

