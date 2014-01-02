CREATE TABLE [dbo].[VideoSocialPost] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [VideoID]      INT NOT NULL,
    [SocialPostID] INT NOT NULL,
    CONSTRAINT [PK_VideoSocialPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_VideoSocialPost_SocialPost] FOREIGN KEY ([SocialPostID]) REFERENCES [dbo].[SocialPost] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_VideoSocialPost_Video] FOREIGN KEY ([VideoID]) REFERENCES [dbo].[Video] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

