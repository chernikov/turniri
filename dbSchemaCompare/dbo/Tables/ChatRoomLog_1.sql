CREATE TABLE [dbo].[ChatRoomLog] (
    [ID]         INT      IDENTITY (1, 1) NOT NULL,
    [ChatRoomID] INT      NOT NULL,
    [UserID]     INT      NOT NULL,
    [ReadDate]   DATETIME NOT NULL,
    CONSTRAINT [PK_ChatRoomLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChatRoomLog_ChatRoom] FOREIGN KEY ([ChatRoomID]) REFERENCES [dbo].[ChatRoom] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ChatRoomLog_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

