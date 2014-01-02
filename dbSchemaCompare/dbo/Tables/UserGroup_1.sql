CREATE TABLE [dbo].[UserGroup] (
    [ID]        INT      IDENTITY (1, 1) NOT NULL,
    [UserID]    INT      NOT NULL,
    [GroupID]   INT      NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    [Status]    INT      NOT NULL,
    CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UserGroup_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserGroup_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

