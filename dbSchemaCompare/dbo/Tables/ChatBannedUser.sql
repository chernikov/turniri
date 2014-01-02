CREATE TABLE [dbo].[ChatBannedUser] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [ChatRoomID] INT            NOT NULL,
    [UserID]     INT            NOT NULL,
    [DateTill]   DATETIME       NULL,
    [Reason]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ChatBannedUser] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChatBannedUser_ChatRoom] FOREIGN KEY ([ChatRoomID]) REFERENCES [dbo].[ChatRoom] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ChatBannedUser_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

