CREATE TABLE [dbo].[BlogSocialPost] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [BlogID]       INT NOT NULL,
    [SocialPostID] INT NOT NULL,
    CONSTRAINT [PK_BlogSocialPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BlogSocialPost_Blog] FOREIGN KEY ([BlogID]) REFERENCES [dbo].[Blog] ([ID]),
    CONSTRAINT [FK_BlogSocialPost_SocialPost] FOREIGN KEY ([SocialPostID]) REFERENCES [dbo].[SocialPost] ([ID])
);

