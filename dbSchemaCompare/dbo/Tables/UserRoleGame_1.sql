CREATE TABLE [dbo].[UserRoleGame] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [UserRoleID] INT NOT NULL,
    [GameID]     INT NOT NULL,
    CONSTRAINT [PK_UserRoleGame] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserRoleGame_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[Game] ([ID]),
    CONSTRAINT [FK_UserRoleGame_UserRole] FOREIGN KEY ([UserRoleID]) REFERENCES [dbo].[UserRole] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

