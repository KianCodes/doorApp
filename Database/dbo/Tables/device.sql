CREATE TABLE [dbo].[device]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ipAddr] NVARCHAR(50) NULL, 
    [macAddr] NVARCHAR(50) NULL, 
    [nickname] NVARCHAR(50) NULL, 
    [status] NVARCHAR(50) NULL
)
