CREATE TABLE [dbo].[UserRoleTournament] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserRoleID]   INT NOT NULL,
    [TournamentID] INT NOT NULL,
    CONSTRAINT [PK_UserRoleTournament] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserRoleTournament_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserRoleTournament_UserRole] FOREIGN KEY ([UserRoleID]) REFERENCES [dbo].[UserRole] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

