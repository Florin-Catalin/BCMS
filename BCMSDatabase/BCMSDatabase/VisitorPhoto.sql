﻿CREATE TABLE [dbo].[VisitorPhoto]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Mac] NVARCHAR(50) NULL, 
    [Photo] VARBINARY(MAX) NULL, 
    [Date] DATETIME NULL
)
