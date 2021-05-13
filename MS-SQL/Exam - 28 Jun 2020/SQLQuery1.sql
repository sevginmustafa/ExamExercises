CREATE DATABASE ColonialJourney 

--1. Database Design
CREATE TABLE Planets
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(30) NOT NULL
)

CREATE TABLE Spaceports
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	PlanetId INT REFERENCES Planets(Id) NOT NULL
)

CREATE TABLE Spaceships
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Manufacturer VARCHAR(30) NOT NULL,
	LightSpeedRate INT DEFAULT 0
)

CREATE TABLE Colonists
(
	Id INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Ucn VARCHAR(10) UNIQUE NOT NULL,
	BirthDate DATE NOT NULL
)

CREATE TABLE Journeys
(
	Id INT IDENTITY PRIMARY KEY,
	JourneyStart DATETIME NOT NULL,
	JourneyEnd DATETIME NOT NULL,
	Purpose VARCHAR(11), CHECK (Purpose IN('Medical', 'Technical', 'Educational', 'Military')),
	DestinationSpaceportId INT REFERENCES Spaceports(Id) NOT NULL,
	SpaceshipId INT REFERENCES Spaceships(Id) NOT NULL
)

CREATE TABLE TravelCards
(
	Id INT IDENTITY PRIMARY KEY,
	CardNumber VARCHAR(10) UNIQUE NOT NULL,
	JobDuringJourney VARCHAR(8), CHECK (JobDuringJourney IN('Pilot', 'Engineer', 'Trooper', 'Cleaner', 'Cook')),
	ColonistId INT REFERENCES Colonists(Id) NOT NULL,
	JourneyId INT REFERENCES Journeys(Id) NOT NULL
)


--2. Insert
INSERT INTO Planets(Name) VALUES
('Mars'),
('Earth'),
('Jupiter'),
('Saturn')

INSERT INTO Spaceships(Name, Manufacturer, LightSpeedRate) VALUES
('Golf', 'VW', 3),
('WakaWaka', 'Wakanda', 4),
('Falcon9', 'SpaceX', 1),
('Bed', 'Vidolov', 6)


--3. Update
UPDATE Spaceships SET LightSpeedRate +=1
WHERE Id BETWEEN 8 AND 12


--4. Delete
DELETE FROM TravelCards
WHERE JourneyId BETWEEN 1 AND 3

DELETE FROM Journeys
WHERE Id BETWEEN 1 AND 3


--5. Select all military journeys
SELECT 
		Id, 
		CONVERT(VARCHAR, JourneyStart, 103) AS JourneyStart , 
		CONVERT(VARCHAR, JourneyEnd, 103) AS JourneyEnd 
FROM Journeys
WHERE Purpose = 'Military'
ORDER BY JourneyStart


--6. Select all pilots
SELECT 
		C.Id AS id,
		C.FirstName + ' ' + C.LastName AS [full_name]
FROM Colonists C
JOIN TravelCards TC ON TC.ColonistId = C.Id
WHERE TC.JobDuringJourney = 'Pilot'
ORDER BY C.Id


--7. Count colonists
SELECT COUNT(*) AS count FROM Colonists C
JOIN TravelCards TC ON TC.ColonistId = C.Id
JOIN Journeys J ON J.Id = TC.JourneyId
WHERE J.Purpose = 'Technical'


--8. Select spaceships with pilots younger than 30 years
SELECT S.Name, S.Manufacturer FROM Spaceships S
JOIN Journeys J ON J.SpaceshipId = S.Id
JOIN TravelCards TC ON TC.JourneyId = J.Id
JOIN Colonists C ON C.Id = TC.ColonistId
WHERE TC.JobDuringJourney = 'Pilot' AND DATEDIFF(YEAR, C.BirthDate, '01/01/2019') < 30
ORDER BY S.Name


--9. Select all planets and their journey count
SELECT  P.Name AS PlanetName, COUNT(*) AS JourneysCount FROM Planets P
JOIN Spaceports S ON S.PlanetId = P.Id
JOIN Journeys J ON J.DestinationSpaceportId = S.Id
GROUP BY P.Name
ORDER BY JourneysCount DESC, PlanetName


--10. Select Second Oldest Important Colonist
SELECT JobDuringJourney, FullName, JobRank FROM
(SELECT 
		TC.JobDuringJourney, 
		C.FirstName + ' ' + C.LastName AS FullName, 
		DENSE_RANK() OVER (PARTITION BY TC.JobDuringJourney ORDER BY DATEDIFF(DAY, C.BirthDate, GETDATE()) DESC) AS JobRank
FROM Colonists C
JOIN TravelCards TC ON TC.ColonistId = C.Id
JOIN Journeys J ON J.Id = TC.JourneyId) AS T
WHERE JobRank = 2


--11. Get Colonists Count
CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30)) 
RETURNS INT
BEGIN
	RETURN (SELECT COUNT(*) FROM Colonists C 
	JOIN TravelCards TC ON TC.ColonistId = C.Id
	JOIN Journeys J ON J.Id = TC.JourneyId
	JOIN Spaceports S ON S.Id = J.DestinationSpaceportId
	JOIN Planets P ON P.Id = S.PlanetId
	WHERE P.Name = @PlanetName)
END


--12. Change Journey Purpose
CREATE PROC usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(11))
AS
	DECLARE @checkJourneyId INT = (SELECT Id FROM Journeys WHERE Id = @JourneyId)
	IF(@checkJourneyId IS NULL)
		THROW 50001, 'The journey does not exist!', 1
	
	DECLARE @checkPurpose VARCHAR(11) = (SELECT Purpose FROM Journeys WHERE Id = @JourneyId)
	IF(@checkPurpose = @NewPurpose)
		THROW 50002, 'You cannot change the purpose!', 1

	UPDATE Journeys SET Purpose = @NewPurpose
	WHERE Id = @JourneyId
