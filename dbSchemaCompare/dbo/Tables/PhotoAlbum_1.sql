CREATE TABLE [dbo].[PhotoAlbum] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [UserID]     INT            NOT NULL,
    [GroupID]    INT            NULL,
    [Name]       NVARCHAR (500) NOT NULL,
    [Url]        NVARCHAR (500) NOT NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [OrderBy]    INT            NOT NULL,
    [ShowOnMain] BIT            NOT NULL,
    CONSTRAINT [PK_PhotoAlbum] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PhotoAlbum_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_PhotoAlbum_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

