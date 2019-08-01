CREATE TABLE [dbo].[VisitorData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EnterpriseID] INT NULL, 
    [Mac] NVARCHAR(50) NULL, 
    [Name] NVARCHAR(50) NULL, 
    [Room] INT NULL
)
