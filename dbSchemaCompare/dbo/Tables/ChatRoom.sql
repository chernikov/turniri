CREATE TABLE [dbo].[ChatRoom] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [TournamentID]     INT            NULL,
    [Name]             NVARCHAR (500) NOT NULL,
    [LastUpdate]       DATETIME       NOT NULL,
    [LastIdUpdate]     INT            NULL,
    [TranslateInForum] BIT            NOT NULL,
    CONSTRAINT [PK_ChatRoom] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChatRoom_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

