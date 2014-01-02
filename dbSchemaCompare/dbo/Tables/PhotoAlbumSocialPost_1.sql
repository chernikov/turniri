CREATE TABLE [dbo].[PhotoAlbumSocialPost] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [PhotoAlbumID] INT NOT NULL,
    [SocialPostID] INT NOT NULL,
    CONSTRAINT [PK_PhotoAlbumSocialPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PhotoAlbumSocialPost_PhotoAlbum] FOREIGN KEY ([PhotoAlbumID]) REFERENCES [dbo].[PhotoAlbum] ([ID]),
    CONSTRAINT [FK_PhotoAlbumSocialPost_SocialPost] FOREIGN KEY ([SocialPostID]) REFERENCES [dbo].[SocialPost] ([ID])
);

