CREATE TABLE [dbo].[SocialGroup] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Provider] INT            NOT NULL,
    [Name]     NVARCHAR (500) NOT NULL,
    [Number]   NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_SocialGroup] PRIMARY KEY CLUSTERED ([ID] ASC)
);

