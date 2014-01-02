CREATE TABLE [dbo].[SocialPostImage] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [SocialPostID] INT            NOT NULL,
    [PhotoUrl]     NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_SocialPostImage] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SocialPostImage_SocialPost] FOREIGN KEY ([SocialPostID]) REFERENCES [dbo].[SocialPost] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

