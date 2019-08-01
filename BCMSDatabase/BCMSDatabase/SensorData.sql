CREATE TABLE [dbo].[SensorData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RPiID] INT NULL, 
    [Temperature] FLOAT NULL, 
    [Humidity] FLOAT NULL, 
    [Date] DATETIME NULL
)
