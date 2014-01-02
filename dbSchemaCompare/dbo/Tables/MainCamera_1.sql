CREATE TABLE [dbo].[MainCamera] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [CameraID] INT            NULL,
    [Code]     NVARCHAR (MAX) NULL,
    [Enabled]  BIT            NOT NULL,
    CONSTRAINT [PK_MainCamera] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MainCamera_Camera] FOREIGN KEY ([CameraID]) REFERENCES [dbo].[Camera] ([ID])
);

