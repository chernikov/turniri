CREATE TABLE [dbo].[League] (
    [ID]                INT            IDENTITY (1, 1) NOT NULL,
    [GameID]            INT            NOT NULL,
    [ForumID]           INT            NOT NULL,
    [Name]              NVARCHAR (500) NOT NULL,
    [Image]             NVARCHAR (150) NOT NULL,
    [IsGroup]           BIT            NOT NULL,
    [CountRound]        INT            NOT NULL,
    [SingleWinPoint]    INT            NOT NULL,
    [SingleDrawPoint]   INT            NOT NULL,
    [DoubleGoalInGuest] BIT            NOT NULL,
    [HostGuest]         BIT            NOT NULL,
    [TeamCount]         INT            NULL,
    [HotReplacement]    INT            NULL,
    [Rules]             NVARCHAR (MAX) NOT NULL,
    [Description]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_League] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_League_Forum] FOREIGN KEY ([ForumID]) REFERENCES [dbo].[Forum] ([ID]),
    CONSTRAINT [FK_League_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[Game] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

