CREATE TABLE [dbo].[Player]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(500) NULL
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

CREATE TABLE [dbo].[SeriePlayers]
(
	[SerieId] NVARCHAR(50) NOT NULL,
	[PlayerId] NVARCHAR(50) NOT NULL, 
	[Position] INT NOT NULL,
	FOREIGN KEY (SerieId) REFERENCES [Serie](Id),
	FOREIGN KEY (PlayerId) REFERENCES [Player](Id),
	PRIMARY KEY ([SerieId], [PlayerId])
)

CREATE TABLE [dbo].[Game]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SerieId] NVARCHAR(50) NOT NULL, 
    [CreationDate] DATETIME NOT NULL, 
	[Ended] BIT NOT NULL,
	[CardGiverId] NVARCHAR(50) NOT NULL,
	[LittleGoatCallerId] NVARCHAR(50) NULL,
	[NextToPlayId] NVARCHAR(50) NOT NULL,
    FOREIGN KEY (SerieId) REFERENCES Serie(Id),
	FOREIGN KEY (LittleGoatCallerId) REFERENCES Player(Id),
	FOREIGN KEY (CardGiverId) REFERENCES Player(Id),
	FOREIGN KEY (NextToPlayId) REFERENCES Player(Id)
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

CREATE TABLE [dbo].[GameCard]
(
	[Symbol] INT NOT NULL,
	[Value] INT NOT NULL, 
	[GameId] INT NOT NULL,
	[PlayerId] NVARCHAR(50) NULL,
	[Position] INT NULL,
	[IsCover] BIT NOT NULL,
	[FileName] NVARCHAR(50) NOT NULL,
	PRIMARY KEY ([Symbol], [Value], [GameId]),
	FOREIGN KEY (GameId) REFERENCES Game(Id),
	FOREIGN KEY (PlayerId) REFERENCES Player(Id)
)

CREATE TABLE [dbo].[SerieChat]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SerieId] NVARCHAR(50) NOT NULL, 
    [PlayerId] NVARCHAR(50) NULL,
    [Message] NVARCHAR(3000) NOT NULL,
    [Date] DATETIME NOT NULL,
	FOREIGN KEY (SerieId) REFERENCES Serie(Id),
	FOREIGN KEY (PlayerId) REFERENCES Player(Id)
)

CREATE TABLE [dbo].[GameActions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SerieId] NVARCHAR(50) NOT NULL, 
    [PlayerId] NVARCHAR(50) NULL,
    [Action] INT NOT NULL,
	[CardSymbol] INT NOT NULL,
	[CardValue] INT NOT NULL, 
	[HiddenData] NVARCHAR(3000) NOT NULL, 
    [Date] DATETIME NOT NULL,
	FOREIGN KEY (SerieId) REFERENCES Serie(Id),
	FOREIGN KEY (PlayerId) REFERENCES Player(Id)
)