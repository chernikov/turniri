CREATE TABLE [dbo].[BannerStatistic] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [BannerID]  INT      NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    CONSTRAINT [PK_BannerStatistic] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BannerStatistic_Banner] FOREIGN KEY ([BannerID]) REFERENCES [dbo].[Banner] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

