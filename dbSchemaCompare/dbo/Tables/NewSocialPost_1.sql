CREATE TABLE [dbo].[NewSocialPost] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [NewID]        INT NOT NULL,
    [SocialPostID] INT NOT NULL,
    CONSTRAINT [PK_NewSocialPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NewSocialPost_New] FOREIGN KEY ([NewID]) REFERENCES [dbo].[New] ([ID]),
    CONSTRAINT [FK_NewSocialPost_SocialPost] FOREIGN KEY ([SocialPostID]) REFERENCES [dbo].[SocialPost] ([ID])
);

