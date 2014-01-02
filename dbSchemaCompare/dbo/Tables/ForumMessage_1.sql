CREATE TABLE [dbo].[ForumMessage] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [UserID]        INT            NOT NULL,
    [ParentID]      INT            NULL,
    [Message]       NVARCHAR (MAX) NOT NULL,
    [AddedDate]     DATETIME       NOT NULL,
    [IsDeleted]     BIT            NOT NULL,
    [ForumID]       INT            NULL,
    [ModeratedByID] INT            NULL,
    [ModeratedDate] DATETIME       NULL,
    [ChatRoomID]    INT            NULL,
    CONSTRAINT [PK_ForumMessage] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ForumMessage_ChatRoom] FOREIGN KEY ([ChatRoomID]) REFERENCES [dbo].[ChatRoom] ([ID]),
    CONSTRAINT [FK_ForumMessage_Forum] FOREIGN KEY ([ForumID]) REFERENCES [dbo].[Forum] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ForumMessage_ForumMessage] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[ForumMessage] ([ID]),
    CONSTRAINT [FK_ForumMessage_ModeratedBy] FOREIGN KEY ([ModeratedByID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_ForumMessage_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);

