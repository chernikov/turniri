CREATE TABLE [dbo].[Rating] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [UserID]     INT NOT NULL,
    [GameID]     INT NOT NULL,
    [Level]      INT NOT NULL,
    [TotalScore] INT NOT NULL,
    [IsActive]   BIT NOT NULL,
    CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Rating_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[Game] ([ID]),
    CONSTRAINT [FK_Rating_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

