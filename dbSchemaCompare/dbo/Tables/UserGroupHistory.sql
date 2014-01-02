CREATE TABLE [dbo].[UserGroupHistory] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [UserID]    INT      NOT NULL,
    [GroupID]   INT      NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    [ExitDate]  DATETIME NULL,
    CONSTRAINT [PK_UserGroupHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserGroupHistory_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]),
    CONSTRAINT [FK_UserGroupHistory_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

