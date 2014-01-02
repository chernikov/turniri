CREATE TABLE [dbo].[TournamentSocialPost] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [TournamentID] INT NOT NULL,
    [SocialPostID] INT NOT NULL,
    CONSTRAINT [PK_TournamentSocialPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TournamentSocialPost_SocialPost] FOREIGN KEY ([SocialPostID]) REFERENCES [dbo].[SocialPost] ([ID]),
    CONSTRAINT [FK_TournamentSocialPost_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [dbo].[Tournament] ([ID])
);

