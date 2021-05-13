CREATE DATABASE WMS

--1. Database design
CREATE TABLE Clients
(
	ClientId INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL
)

CREATE TABLE Mechanics
(
	MechanicId INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Address VARCHAR(255) NOT NULL
)

CREATE TABLE Models
(
	ModelId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Jobs
(
	JobId INT IDENTITY PRIMARY KEY,
	ModelId INT REFERENCES Models(ModelId) NOT NULL,
	Status VARCHAR(11) DEFAULT 'Pending', CHECK(Status IN('Pending', 'In Progress', 'Finished')),
	ClientId INT REFERENCES Clients(ClientId) NOT NULL,
	MechanicId INT REFERENCES Mechanics(MechanicId),
	IssueDate DATE NOT NULL,
	FinishDate DATE
)

CREATE TABLE Orders
(
	OrderId INT IDENTITY PRIMARY KEY,
	JobId INT REFERENCES Jobs(JobId) NOT NULL,
	IssueDate DATE,
	Delivered BIT DEFAULT 0
)

CREATE TABLE Vendors
(
	VendorId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts
(
	PartId INT IDENTITY PRIMARY KEY,
	SerialNumber VARCHAR(50) UNIQUE NOT NULL,
	Description VARCHAR(255),
	Price MONEY NOT NULL, CHECK(Price > 0),
	VendorId INT REFERENCES Vendors(VendorId) NOT NULL,
	StockQty INT DEFAULT 0, CHECK(StockQty >= 0)
)

CREATE TABLE OrderParts
(
	OrderId INT REFERENCES Orders(OrderId),
	PartId INT REFERENCES Parts(PartId),
	Quantity INT DEFAULT 1, CHECK(Quantity >= 0),
	PRIMARY KEY(OrderId, PartId)
)

CREATE TABLE PartsNeeded
(
	JobId INT REFERENCES Jobs(JobId),
	PartId INT REFERENCES Parts(PartId),
	Quantity INT DEFAULT 1, CHECK(Quantity >= 0),
	PRIMARY KEY(JobId, PartId)
)


--2. Insert
INSERT INTO Clients(FirstName, LastName, Phone) VALUES
('Teri', 'Ennaco', '570-889-5187'),
('Merlyn', 'Lawler', '201-588-7810'),
('Georgene', 'Montezuma', '925-615-5185'),
('Jettie', 'Mconnell', '908-802-3564'),
('Lemuel', 'Latzke', '631-748-6479'),
('Melodie',	'Knipp', '805-690-1682'),
('Candida', 'Corbley', '908-275-8357')

INSERT INTO Parts(SerialNumber, Description, Price, VendorId) VALUES
('WP8182119', 'Door Boot Seal',	117.86,	2),
('W10780048', 'Suspension Rod', 42.81, 1),
('W10841140', 'Silicone Adhesive', 6.77, 4),
('WPY055980', 'High Temperature Adhesive', 13.94, 3)


--3. Update
UPDATE Jobs SET MechanicId = 3, Status = 'In Progress'
WHERE Status = 'Pending'


--4. Delete
DELETE FROM OrderParts
WHERE OrderId = 19

DELETE FROM Orders
WHERE OrderId = 19


--5. Mechanic Assignments
SELECT M.FirstName + ' ' + M.LastName AS Mechanic, J.Status, J.IssueDate FROM Mechanics M
JOIN Jobs J ON J.MechanicId = M.MechanicId
ORDER BY M.MechanicId, J.JobId, J.IssueDate


--6. Current Clients
SELECT 
	C.FirstName + ' ' + C.LastName AS Client,
	DATEDIFF(DAY, J.IssueDate, '2017-04-24') AS [Days going],
	J.Status 
FROM Clients C
JOIN Jobs J ON J.ClientId = C.ClientId
WHERE J.Status != 'Finished'


--7. Mechanic Performance
SELECT Mechanic, [Average Days] FROM (SELECT
					M.MechanicId,
					M.FirstName + ' ' + M.LastName AS Mechanic, 
					AVG(DATEDIFF(DAY, J.IssueDate, J.FinishDate)) AS [Average Days]
			   FROM Mechanics M
JOIN Jobs J ON J.MechanicId = M.MechanicId
GROUP BY M.MechanicId, M.FirstName + ' ' + M.LastName) AS T
ORDER BY MechanicId


--8. Available Mechanics		
SELECT M.FirstName + ' ' + M.LastName AS Available FROM Mechanics M 
WHERE NOT EXISTS(SELECT * FROM Jobs J WHERE J.MechanicId = M.MechanicId AND J.FinishDate IS NULL)
ORDER BY M.MechanicId


--9. Past Expenses
SELECT J.JobId, ISNULL(SUM(P.Price*OP.Quantity), 0) AS Total FROM Parts P
FULL JOIN OrderParts OP ON OP.PartId = P.PartId
FULL JOIN Orders O ON O.OrderId = OP.OrderId
FULL JOIN Jobs J ON J.JobId = O.JobId
WHERE J.Status = 'Finished'
GROUP BY J.JobId
ORDER BY Total DESC, J.JobId


--10. Missing Parts
SELECT 
	P.PartId, 
	P.Description, 
	PN.Quantity AS Required, 
	P.StockQty AS [In Stock], 
	IIF(O.Delivered = 0, OP.Quantity, 0) AS Ordered 
FROM Jobs J
FULL JOIN PartsNeeded PN ON PN.JobId = J.JobId
FULL JOIN Parts P ON P.PartId = PN.PartId
FULL JOIN OrderParts OP ON OP.PartId = P.PartId
FULL JOIN Orders O ON O.OrderId = OP.OrderId
WHERE J.Status != 'Finished' AND 
(P.StockQty + IIF(O.Delivered = 0, OP.Quantity, 0)) < PN.Quantity
ORDER BY P.PartId


--11. Place Order
CREATE PROC usp_PlaceOrder(@JobId INT, @SerialNumber VARCHAR(50), @Quantity INT)
AS

DECLARE @JobStatus VARCHAR(20) = (SELECT Status FROM Jobs WHERE JobId = @JobId)
DECLARE @CheckJobId INT = (SELECT JobId FROM Jobs WHERE JobId = @JobId)
DECLARE @CheckSerialNumber INT = (SELECT PartId FROM Parts WHERE SerialNumber = @SerialNumber)

IF(@JobStatus = 'Finished')
	THROW 50011, 'This job is not active!', 1

IF(@Quantity <= 0)
	THROW 50012, 'Part quantity must be more than zero!', 1

IF(@CheckJobId IS NULL)
	THROW 50013, 'Job not found!', 1
	
IF(@CheckSerialNumber IS NULL)
	THROW 50014, 'Part not found!', 1

DECLARE @CheckIfJobHaveOrder INT = (SELECT OrderId FROM Orders
										WHERE JobId = @JobId
										  AND IssueDate IS NULL)

IF(@CheckIfJobHaveOrder IS NULL)
INSERT INTO Orders (JobId, IssueDate, Delivered)
VALUES (@JobId, NULL, 0)


DECLARE @OrderId INT = (SELECT OrderId FROM Orders
						 WHERE JobId = @JobId
						   AND IssueDate IS NULL)

DECLARE @OrderPartsQuantity INT = (SELECT Quantity FROM OrderParts
                               WHERE OrderId = @OrderId
                                 AND PartId = @CheckSerialNumber)

IF (@OrderPartsQuantity IS NULL)
BEGIN
	INSERT INTO OrderParts (OrderId, PartId, Quantity)
    VALUES (@OrderId, @CheckSerialNumber, @Quantity)
END
ELSE
BEGIN
	UPDATE OrderParts
	SET Quantity += @Quantity
	WHERE OrderId = @OrderId
	  AND PartId = @CheckSerialNumber
END


--12. Cost Of Order
CREATE FUNCTION udf_GetCost(@JobId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
RETURN ISNULL((SELECT SUM(P.Price * OP.Quantity) FROM OrderParts OP
JOIN Parts P ON P.PartId = OP.PartId
JOIN Orders O ON O.OrderId = OP.OrderId
JOIN Jobs J ON J.JobId = O.JobId
WHERE J.JobId = @JobId
GROUP BY J.JobId), 0)
END