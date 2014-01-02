CREATE TABLE [dbo].[Social] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [UserID]       INT            NOT NULL,
    [Identified]   NVARCHAR (MAX) NOT NULL,
    [IsAdvansed]   BIT            NOT NULL,
    [Provider]     INT            NOT NULL,
    [UserInfo]     NVARCHAR (MAX) NULL,
    [JsonResource] NVARCHAR (MAX) NOT NULL,
    [TranslateWin] BIT            NOT NULL,
    CONSTRAINT [PK_Social] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Social_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

