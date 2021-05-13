CREATE DATABASE TripService

--1. Database design
CREATE TABLE Cities
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(20) NOT NULL,
	CountryCode VARCHAR(2) NOT NULL
)

CREATE TABLE Hotels
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(30) NOT NULL,
	CityId INT REFERENCES Cities(Id) NOT NULL,
	EmployeeCount INT NOT NULL,
	BaseRate DECIMAL(10,2)
)

CREATE TABLE Rooms
(
	Id INT IDENTITY PRIMARY KEY,
	Price DECIMAL(10,2) NOT NULL,
	Type NVARCHAR(20) NOT NULL,
	Beds INT NOT NULL,
	HotelId INT REFERENCES Hotels(Id) NOT NULL
)

CREATE TABLE Trips
(
	Id INT IDENTITY PRIMARY KEY,
	RoomId INT REFERENCES Rooms(Id) NOT NULL,
	BookDate DATE NOT NULL, CHECK(BookDate < ArrivalDate),
	ArrivalDate DATE NOT NULL, CHECK(ArrivalDate < ReturnDate),
	ReturnDate DATE NOT NULL,
	CancelDate DATE
)

CREATE TABLE Accounts
(
	Id INT IDENTITY PRIMARY KEY,
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(20),
	LastName NVARCHAR(50) NOT NULL,
	CityId INT REFERENCES Cities(Id) NOT NULL,
	BirthDate DATE NOT NULL,
	Email VARCHAR(100) UNIQUE NOT NULL
)

CREATE TABLE AccountsTrips
(
	AccountId INT REFERENCES Accounts(Id) NOT NULL,
	TripId INT REFERENCES Trips(Id) NOT NULL,
	Luggage INT NOT NULL, CHECK(Luggage >= 0),
	PRIMARY KEY(AccountId, TripId)
)
   

--2. Insert
INSERT INTO Accounts(FirstName, MiddleName, LastName, CityId, BirthDate, Email) VALUES
('John', 'Smith', 'Smith', 34, '1975-07-21', 'j_smith@gmail.com'),
('Gosho', NULL, 'Petrov', 11, '1978-05-16', 'g_petrov@gmail.com'),
('Ivan', 'Petrovich', 'Pavlov', 59, '1849-09-26', 'i_pavlov@softuni.bg'),
('Friedrich', 'Wilhelm', 'Nietzsche', 2, '1844-10-15', 'f_nietzsche@softuni.bg')

INSERT INTO Trips(RoomId, BookDate, ArrivalDate, ReturnDate, CancelDate) VALUES
(101, '2015-04-12',	'2015-04-14', '2015-04-20',	'2015-02-02'),
(102, '2015-07-07',	'2015-07-15', '2015-07-22',	'2015-04-29'),
(103, '2013-07-17',	'2013-07-23', '2013-07-24',	NULL),
(104, '2012-03-17',	'2012-03-31', '2012-04-01',	'2012-01-10'),
(109, '2017-08-07',	'2017-08-28', '2017-08-29',	NULL)


--3. Update
UPDATE Rooms SET Price *= 1.14
WHERE HotelId IN(5, 7, 9)


--4. Delete
DELETE FROM AccountsTrips
WHERE AccountId = 47


--5. EEE-Mails
SELECT A.FirstName, A.LastName, CONVERT(VARCHAR, A.BirthDate, 110) AS BirthDate, C.Name AS Hometown, A.Email FROM Accounts A
JOIN Cities C ON C.Id = A.CityId
WHERE A.Email LIKE 'e%'
ORDER BY C.Name


--6. City Statistics
SELECT C.Name AS City, COUNT(*) AS Hotels FROM Cities C
JOIN Hotels H ON H.CityId = C.Id
GROUP BY C.Name
ORDER BY Hotels DESC, City


--7. Longest and Shortest Trips
SELECT 
	[AT].AccountId,
	A.FirstName + ' ' + A.LastName AS FullName, 
	MAX(DATEDIFF(DAY, T.ArrivalDate, T.ReturnDate)) AS LongestTrip, 
	MIN(DATEDIFF(DAY, T.ArrivalDate, T.ReturnDate)) AS ShortestTrip 
FROM Accounts A
JOIN AccountsTrips [AT] ON [AT].AccountId = A.Id
JOIN Trips T ON T.Id = [AT].TripId
WHERE A.MiddleName IS NULL AND T.CancelDate IS NULL
GROUP BY [AT].AccountId, A.FirstName + ' ' + A.LastName
ORDER BY LongestTrip DESC, ShortestTrip


--8. Metropolis
SELECT TOP(10) C.Id, C.Name AS City, C.CountryCode AS Country, COUNT(*) AS Accounts FROM Cities C
JOIN Accounts A ON A.CityId = C.Id
GROUP BY C.Id, C.Name, C.CountryCode
ORDER BY Accounts DESC


--9. Romantic Getaways
SELECT 
	A.Id, 
	A.Email, 
	C.Name AS City,
	COUNT(T.Id) AS Trips
FROM Accounts A
JOIN AccountsTrips [AT] ON [AT].AccountId = A.Id
JOIN Trips T ON T.Id = [AT].TripId
JOIN Rooms R ON R.Id = T.RoomId
JOIN Hotels H ON H.Id = R.HotelId
JOIN Cities C ON C.Id = H.CityId
WHERE H.CityId = A.CityId
GROUP BY A.Id, A.Email, C.Name
ORDER BY Trips DESC, A.Id


--10. GDPR Violation
SELECT 
	T.Id, 
	A.FirstName + ' ' + ISNULL(A.MiddleName + ' ', '' ) + A.LastName AS [Full Name],
	C.Name AS [From],
	C2.Name AS [To],
	CASE
	WHEN T.CancelDate IS NOT NULL THEN 'Canceled'
	ELSE CONVERT(VARCHAR, DATEDIFF(DAY, T.ArrivalDate, T.ReturnDate)) + ' days'
	END AS Duration
FROM Trips T
JOIN AccountsTrips [AT] ON [AT].TripId = T.Id
JOIN Accounts A ON A.Id = [AT].AccountId
JOIN Cities C ON C.Id = A.CityId
JOIN Rooms R ON R.Id = T.RoomId
JOIN Hotels H ON H.Id = R.HotelId
JOIN Cities C2 ON C2.Id = H.CityId
ORDER BY [Full Name], T.Id


--11. Available Room
CREATE FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE @Result VARCHAR(MAX) = (SELECT TOP(1) CONCAT('Room ',R.Id , ': ',R.Type , ' (', R.Beds , ' beds) - $', (H.BaseRate + R.Price) * @People) FROM Rooms R
							JOIN Trips T ON T.RoomId = R.Id
							JOIN Hotels H ON H.Id = R.HotelId
							WHERE 
								(T.CancelDate IS NULL) AND 
								(@Date NOT BETWEEN T.ArrivalDate AND ReturnDate) AND 
								(H.Id = @HotelId) AND
								(R.Beds >= @People) AND
								(YEAR(T.ArrivalDate) = YEAR(@Date))
							ORDER BY (H.BaseRate + R.Price) * @People DESC)
IF(@Result IS NULL)
	RETURN 'No rooms available'
	RETURN @Result
END


-- 12. Switch Room
CREATE PROC usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
DECLARE @TargetRoomHotel INT = (SELECT DISTINCT H.Id FROM Rooms R
				JOIN Hotels H ON H.Id = R.HotelId
				WHERE R.Id = @TargetRoomId)
DECLARE @TripHotel INT = (SELECT DISTINCT H.Id FROM Rooms R
				JOIN Trips T ON T.RoomId = R.Id
				JOIN Hotels H ON H.Id = R.HotelId
				WHERE T.Id = @TripId)

IF(@TargetRoomHotel != @TripHotel)
THROW 50001, 'Target room is in another hotel!', 1

DECLARE @TargetRoomBeds INT = (SELECT Beds FROM Rooms WHERE Id = @TargetRoomId)
DECLARE @TripAccounts INT = (SELECT COUNT(*) FROM Trips T
				JOIN AccountsTrips AT ON AT.TripId = T.Id
				WHERE T.Id = @TripId)

IF(@TargetRoomBeds < @TripAccounts)
THROW 50002, 'Not enough beds in target room!', 1

UPDATE Trips SET RoomId = @TargetRoomId
WHERE Id = @TripId
