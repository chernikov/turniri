CREATE TABLE [dbo].[SocialPost] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [UserID]        INT            NOT NULL,
    [SocialGroupID] INT            NULL,
    [Provider]      INT            NOT NULL,
    [Identifier]    NVARCHAR (500) NOT NULL,
    [AddedDate]     DATETIME       NOT NULL,
    [Title]         NVARCHAR (500) NULL,
    [Preview]       NVARCHAR (500) NULL,
    [Teaser]        NVARCHAR (MAX) NULL,
    [Link]          NVARCHAR (500) NULL,
    [Responce]      NVARCHAR (MAX) NULL,
    [VkPrepared]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SocialPost] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SocialPost_SocialGroup] FOREIGN KEY ([SocialGroupID]) REFERENCES [dbo].[SocialGroup] ([ID]),
    CONSTRAINT [FK_SocialPost_User] FOREIGN KEY ([ID]) REFERENCES [dbo].[User] ([ID])
);

