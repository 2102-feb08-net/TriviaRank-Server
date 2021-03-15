-- Query to make tables for database to be used by TriviaRank-Server

-- Player 
CREATE TABLE Player
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    FirstName NVARCHAR(50) NOT NULL DEFAULT 'First' CHECK (LEN(FirstName) > 0),
    LastName NVARCHAR(50) NOT NULL DEFAULT 'Last' CHECK (LEN(LastName) > 0),
    Password NVARCHAR(50) NOT NULL DEFAULT 'PASSWORD',
    Birthday DATETIMEOFFSET NOT NULL,
    Points INT NOT NULL DEFAULT 0 CHECK (Points >= 0)
)

-- Player selection data is separate from the player
CREATE TABLE PlayerStatistics
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    PlayerId INT NOT NULL,
    Selection NVARCHAR(5) NOT NULL,
    Frequency INT NOT NULL CHECK (Frequency >= 0),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id) ON DELETE CASCADE
)

-- Game 
CREATE TABLE Game
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    GameName NVARCHAR(100) NOT NULL,
    OwnerId INT NOT NULL,
    StartDate DATETIMEOFFSET NOT NULL,
    EndDate DATETIMEOFFSET NOT NULL,
    GameMode BIT NOT NULL,
    TotalQuestions INT NOT NULL,
    CHECK(TotalQuestions >= 0),
    IsPublic BIT NOT NULL,
    FOREIGN KEY (OwnerId) REFERENCES Player(Id) ON DELETE CASCADE,
)


-- players in the game
CREATE TABLE GamePlayers
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    GameId INT NOT NULL,
    PlayerId INT NOT NULL,
    TotalCorrect INT NOT NULL CHECK (TotalCorrect >= 0),
    FOREIGN KEY (GameId) REFERENCES Game(Id),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id)
)


-- Question s in the game
CREATE TABLE Question
(
    Id INT NOT NULL,
    QuestionId INT NOT NULL,
    GameId INT NOT NULL,
    PlayerId INT NOT NULL,
    PlayerAnswer NVARCHAR(5) NOT NULL,
    FOREIGN KEY (GameId) REFERENCES Game(Id),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id),
    PRIMARY KEY (Id,QuestionId,GameId)
)

-- Player messages to each other
CREATE TABLE Message
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    FromId INT NOT NULL,
    ToId INT NOT NULL,
    -- this could be confussing codewise
    Body NVARCHAR(400),
    Date DATETIMEOFFSET NOT NULL,
    FOREIGN KEY (FromId) REFERENCES Player(Id),
    FOREIGN KEY (ToId) REFERENCES Player(Id)
)

-- FRIENDS 
-- Could combine PlayerId and FriendId into composite key
CREATE TABLE Friend
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    PlayerId INT NOT NULL,
    FriendId INT NOT NULL,
    FOREIGN KEY (PlayerId) REFERENCES Player(Id),
    FOREIGN KEY (FriendId) REFERENCES Player(Id)
)

-- INVITE TO GAME
CREATE TABLE GameInviteOutbox
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    InvitedId INT NOT NULL,
    Date DATETIMEOFFSET NOT NULL,
    GameId INT NOT NULL,
    FOREIGN KEY (GameId) REFERENCES Game(Id),
    FOREIGN KEY (InvitedId) REFERENCES Player(Id)

)

-- FRIEND INVITES
CREATE TABLE FriendInviteOutbox
(
    Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    InviterId INT NOT NULL,
    InvitedId INT NOT NULL,
    Date DATETIMEOFFSET NOT NULL,
    FOREIGN KEY (InviterId) REFERENCES Player(Id),
    FOREIGN KEY (InvitedId) REFERENCES Player(Id)
)

