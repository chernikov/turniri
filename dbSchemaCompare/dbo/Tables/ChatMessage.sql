CREATE TABLE [dbo].[ChatMessage] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [ChatRoomID] INT            NOT NULL,
    [UserID]     INT            NOT NULL,
    [Type]       INT            NOT NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [Message]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChatMessage_ChatRoom] FOREIGN KEY ([ChatRoomID]) REFERENCES [dbo].[ChatRoom] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ChatMessage_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

