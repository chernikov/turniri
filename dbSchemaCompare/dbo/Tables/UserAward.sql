CREATE TABLE [dbo].[UserAward] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [AwardID]   INT      NOT NULL,
    [UserID]    INT      NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    CONSTRAINT [PK_UserAward] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserAward_Award] FOREIGN KEY ([AwardID]) REFERENCES [dbo].[Award] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserAward_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

