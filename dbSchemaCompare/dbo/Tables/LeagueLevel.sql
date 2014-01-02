CREATE TABLE [dbo].[LeagueLevel] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [LeagueID] INT            NOT NULL,
    [ForumID]  INT            NOT NULL,
    [Image]    NVARCHAR (150) NULL,
    [Name]     NVARCHAR (500) NOT NULL,
    [Level]    INT            NOT NULL,
    [Quantity] INT            NOT NULL,
    CONSTRAINT [PK_LeagueLevel] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LeagueLevel_Forum] FOREIGN KEY ([ForumID]) REFERENCES [dbo].[Forum] ([ID]),
    CONSTRAINT [FK_LeagueLevel_League] FOREIGN KEY ([LeagueID]) REFERENCES [dbo].[League] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

