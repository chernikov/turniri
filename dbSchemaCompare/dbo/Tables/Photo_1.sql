CREATE TABLE [dbo].[Photo] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [UserID]           INT            NULL,
    [PhotoAlbumID]     INT            NULL,
    [FilePath]         NVARCHAR (150) NOT NULL,
    [AlbumPreviewPath] NVARCHAR (150) NOT NULL,
    [AvatarPath]       NVARCHAR (150) NOT NULL,
    [SmallPath]        NVARCHAR (150) NOT NULL,
    [Name]             NVARCHAR (500) NOT NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [IsBanned]         BIT            NOT NULL,
    [BanDescription]   NVARCHAR (500) NULL,
    [CommentsCount]    INT            NOT NULL,
    [IsAlbumPreview]   BIT            NOT NULL,
    [VisitCount]       INT            NOT NULL,
    [AddedDate]        DATETIME       NOT NULL,
    [Width]            INT            NULL,
    [Height]           INT            NULL,
    [Likes]            INT            NOT NULL,
    CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Photo_PhotoAlbum] FOREIGN KEY ([PhotoAlbumID]) REFERENCES [dbo].[PhotoAlbum] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Photo_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

