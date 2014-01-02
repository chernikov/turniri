CREATE TABLE [dbo].[TeamReplacement] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [TeamID]    INT      NOT NULL,
    [UserOutID] INT      NOT NULL,
    [UserInID]  INT      NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    CONSTRAINT [PK_TeamReplacement] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TeamReplacement_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_TeamReplacement_UserIn] FOREIGN KEY ([UserInID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_TeamReplacement_UserOut] FOREIGN KEY ([UserOutID]) REFERENCES [dbo].[User] ([ID])
);

