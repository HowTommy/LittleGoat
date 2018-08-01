CREATE TABLE [dbo].[Player]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL,
	[SerieId] NVARCHAR(50) NULL
)

CREATE TABLE [dbo].[Serie]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
	[CreationDate] DATETIME NOT NULL,
    [Started] BIT NOT NULL,
	[Ended] BIT NOT NULL,
	[CreatorId] NVARCHAR(50) NOT NULL,
	FOREIGN KEY (CreatorId) REFERENCES [Player](Id)
)

ALTER TABLE [dbo].[Player] ADD FOREIGN KEY (SerieId) REFERENCES Serie(Id)

CREATE TABLE [dbo].[Game]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SerieId] NVARCHAR(50) NOT NULL, 
    [CreationDate] DATETIME NOT NULL, 
	[Ended] BIT NOT NULL,
    FOREIGN KEY (SerieId) REFERENCES Serie(Id),
)

CREATE TABLE [dbo].[Score]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[GameId] INT NOT NULL,
	[PlayerId] NVARCHAR(50) NOT NULL,
	[Points] INT NOT NULL,
	FOREIGN KEY (GameId) REFERENCES Game(Id),
	FOREIGN KEY (PlayerId) REFERENCES Player(Id)
)
