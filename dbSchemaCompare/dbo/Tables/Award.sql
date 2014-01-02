CREATE TABLE [dbo].[Award] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [TeamID]           INT            NULL,
    [GroupID]          INT            NULL,
    [TournamentID]     INT            NOT NULL,
    [Name]             NVARCHAR (500) NOT NULL,
    [IconPath]         NVARCHAR (150) NOT NULL,
    [IsSpecial]        BIT            NOT NULL,
    [AwardedDate]      DATETIME       NULL,
    [MatchID]          INT            NULL,
    [Point]            INT            NOT NULL,
    [Place]            INT            NULL,
    [MoneyGoldPercent] FLOAT (53)     NULL,
    [MoneyWood]        FLOAT (53)     NULL,
    [MoneyCrystal]     FLOAT (53)     NULL,
    CONSTRAINT [PK_Award] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Award_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_Award_Match] FOREIGN KEY ([MatchID]) REFERENCES [dbo].[Match] ([ID]),
    CONSTRAINT [FK_Award_Team] FOREIGN KEY ([TeamID]) REFERENCES [dbo].[Team] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Award_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

