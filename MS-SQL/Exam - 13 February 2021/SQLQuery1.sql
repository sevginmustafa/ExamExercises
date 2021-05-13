CREATE DATABASE Bitbucket

--1. Database Design
CREATE TABLE Users
(
	Id INT IDENTITY PRIMARY KEY,
	Username VARCHAR(30) NOT NULL,
	Password VARCHAR(30) NOT NULL,
	Email VARCHAR(50) NOT NULL,
)

CREATE TABLE Repositories
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE RepositoriesContributors
(
	RepositoryId INT REFERENCES Repositories(Id) NOT NULL,
	ContributorId INT REFERENCES Users(Id) NOT NULL,
	PRIMARY KEY(RepositoryId, ContributorId)
)

CREATE TABLE Issues
(
	Id INT IDENTITY PRIMARY KEY,
	Title VARCHAR(255) NOT NULL,
	IssueStatus CHAR(6) NOT NULL,
	RepositoryId INT REFERENCES Repositories(Id) NOT NULL,
	AssigneeId INT REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Commits
(
	Id INT IDENTITY PRIMARY KEY,
	Message VARCHAR(255) NOT NULL,
	IssueId INT REFERENCES Issues(Id),
	RepositoryId INT REFERENCES Repositories(Id) NOT NULL,
	ContributorId INT REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Files
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Size DECIMAL(18,2) NOT NULL,
	ParentId INT REFERENCES Files(Id),
	CommitId INT REFERENCES Commits(Id) NOT NULL
)


--2. Insert
INSERT INTO Files(Name, Size, ParentId, CommitId) VALUES
('Trade.idk', 2598.0, 1, 1),
('menu.net', 9238.31, 2, 2),
('Administrate.soshy', 1246.93,	3, 3),
('Controller.php', 7353.15, 4, 4),
('Find.java', 9957.86, 5, 5),
('Controller.json',	14034.87, 3, 6),
('Operate.xix',	7662.92, 7, 7)

INSERT INTO Issues(Title, IssueStatus, RepositoryId, AssigneeId) VALUES
('Critical Problem with HomeController.cs file', 'open', 1,	4),
('Typo fix in Judge.html', 'open', 4, 3),
('Implement documentation for UsersService.cs',	'closed', 8, 2),
('Unreachable code in Index.cs', 'open', 9,	8)


--3. Update 
UPDATE Issues SET IssueStatus = 'closed'
WHERE AssigneeId = 6


--4. Delete
DELETE FROM Issues
WHERE RepositoryId = 3

DELETE FROM RepositoriesContributors
WHERE RepositoryId = 3


--5. Commits
SELECT Id, Message, RepositoryId, ContributorId FROM Commits
ORDER BY Id, Message, RepositoryId, ContributorId


--6. Front-end
SELECT Id, Name, Size FROM Files
WHERE Size > 1000 AND Name LIKE '%html%'
ORDER BY Size DESC, Id, Name


--7. Issue Assignment 
SELECT I.Id, Username + ' : ' + I.Title AS IssueAssignee FROM Issues I
JOIN Users U ON U.Id = I.AssigneeId
ORDER BY I.Id DESC, IssueAssignee


--8. Single Files
SELECT F.Id, F.Name, CONCAT(Size, 'KB') AS Size FROM Files F
WHERE NOT EXISTS(SELECT F.Id FROM Files P WHERE F.Id = P.ParentId)
ORDER BY F.Id, F.Name, Size


--9. Commits in Repositories
SELECT TOP(5) R.Id, R.Name, COUNT(*) AS Commits FROM RepositoriesContributors RC
JOIN Repositories R ON R.Id = RC.RepositoryId
JOIN Commits C ON C.RepositoryId = R.Id
GROUP BY R.Id, R.Name
ORDER BY Commits DESC, R.Id, R.Name


-- 10. Average Size
SELECT U.Username, AVG(F.Size) AS Size FROM Users U
JOIN Commits C ON C.ContributorId = U.Id
JOIN Files F ON F.CommitId = C.Id
GROUP BY U.Username
ORDER BY Size DESC, U.Username


--11. All User Commits
CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(MAX))
RETURNS INT
AS
BEGIN
RETURN (SELECT COUNT (C.Id)FROM Users U
JOIN Commits C ON C.ContributorId = U.Id
WHERE U.Username = @username)
END


--12. Search for Files
CREATE PROC usp_SearchForFiles(@fileExtension VARCHAR(MAX))
AS
SELECT Id, Name, CONCAT(Size, 'KB') AS Size FROM Files
WHERE Name LIKE '%.' + @fileExtension
ORDER BY Id, Name, Size DESC