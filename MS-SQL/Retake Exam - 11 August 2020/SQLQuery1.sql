CREATE DATABASE Bakery

--1. Database design
CREATE TABLE Countries
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Customers
(
	Id INT IDENTITY PRIMARY KEY,
	FirstName NVARCHAR(25) NOT NULL,
	LastName NVARCHAR(25) NOT NULL,
	Gender CHAR(1) NOT NULL, CHECK(Gender IN('M', 'F')),
	Age INT NOT NULL,
	PhoneNumber VARCHAR(10) NOT NULL, CHECK(LEN(PhoneNumber) = 10),
	CountryId INT REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Products
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(25) UNIQUE NOT NULL,
	Description NVARCHAR(250) NOT NULL,
	Recipe NVARCHAR(MAX) NOT NULL,
	Price MONEY NOT NULL, CHECK(Price > 0)
)

CREATE TABLE Feedbacks
(
	Id INT IDENTITY PRIMARY KEY,
	Description NVARCHAR(255),
	Rate DECIMAL(10,2) NOT NULL, CHECK(Rate BETWEEN 0 AND 10),
	ProductId INT REFERENCES Products(Id) NOT NULL,
	CustomerId INT REFERENCES Customers(Id) NOT NULL
)

CREATE TABLE Distributors
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(25) UNIQUE NOT NULL,
	AddressText NVARCHAR(30) NOT NULL,
	Summary NVARCHAR(200) NOT NULL,
	CountryId INT REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Ingredients
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(30) NOT NULL,
	Description NVARCHAR(200) NOT NULL,
	OriginCountryId INT REFERENCES Countries(Id) NOT NULL,
	DistributorId INT REFERENCES Distributors(Id) NOT NULL
)

CREATE TABLE ProductsIngredients
(
	ProductId INT REFERENCES Products(Id) NOT NULL,
	IngredientId INT REFERENCES Ingredients(Id) NOT NULL,
	PRIMARY KEY(ProductId, IngredientId)
)


--2. Insert
INSERT INTO Distributors(Name, CountryId, AddressText, Summary) VALUES
('Deloitte & Touche', 2, '6 Arch St #9757',	'Customizable neutral traveling'),
('Congress Title', 13, '58 Hancock St',	'Customer loyalty'),
('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery'),
('General Color Co Inc', 21, '6185 Bohn St #72', 'Focus group'),
('Beck Corporation', 23,	'21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers(FirstName, LastName, Age, Gender, PhoneNumber, CountryId) VALUES
('Francoise', 'Rautenstrauch', 15, 'M',	'0195698399', 5),
('Kendra', 'Loud',	22, 'F', '0063631526', 11),
('Lourdes',	'Bauswell',	50,	'M', '0139037043', 8),
('Hannah', 'Edmison', 18, 'F', '0043343686', 1),
('Tom',	'Loeza', 31, 'M', '0144876096',	23),
('Queenie', 'Kramarczyk', 30, 'F', '0064215793', 29),
('Hiu',	'Portaro', 25,	'M', '0068277755', 16),
('Josefa', 'Opitz', 43, 'F', '0197887645', 17)


--3. Update
UPDATE Ingredients SET DistributorId = 35
WHERE Name IN('Bay Leaf', 'Paprika', 'Poppy')

UPDATE Ingredients SET OriginCountryId  = 14
WHERE OriginCountryId = 8


--4. Delete
DELETE FROM Feedbacks
WHERE CustomerId = 14 OR ProductId = 5


--5. Products By Price
SELECT Name, Price, Description FROM Products
ORDER BY Price DESC, Name


--6. Negative Feedback
SELECT F.ProductId, F.Rate, F.Description, C.Id, C.Age, C.Gender FROM Feedbacks F
JOIN Customers C ON C.Id = F.CustomerId
WHERE F.Rate < 5
ORDER BY F.ProductId DESC, F.Rate


--7. Customers without Feedback
SELECT CONCAT(C.FirstName,' ', C.LastName) AS CustomerName, C.PhoneNumber, C.Gender FROM Customers C
FULL JOIN Feedbacks F ON F.CustomerId = C.Id
WHERE F.Id IS NULL
ORDER BY C.Id


--8. Customers by Criteria
SELECT FirstName, Age, PhoneNumber FROM Customers
WHERE 
	Age >= 21 AND
	CountryId != 31 AND
	(FirstName LIKE '%an%' OR PhoneNumber LIKE '%38')
ORDER BY FirstName, Age DESC


--9. Middle Range Distributors
SELECT D.Name, I.Name, P.Name, AVG(F.Rate) FROM Distributors D
JOIN Ingredients I ON I.DistributorId = D.Id
JOIN ProductsIngredients PI ON PI.IngredientId = I.Id
JOIN Products P ON P.Id = PI.ProductId
JOIN Feedbacks F ON F.ProductId = P.Id
GROUP BY D.Name, I.Name, P.Name
HAVING  AVG(F.Rate) BETWEEN 5 AND 8
ORDER BY D.Name, I.Name, P.Name


--10. Country Representative
SELECT CountryName, DisributorName FROM
			(SELECT 
				C.Name AS CountryName, 
				D.Name AS DisributorName, 
				DENSE_RANK() OVER (PARTITION BY C.Name ORDER BY COUNT(I.Id) DESC) AS CountOfIngredients 
			FROM Countries C 
			JOIN Distributors D ON D.CountryId = C.Id
			FULL JOIN Ingredients I ON I.DistributorId = D.Id
			GROUP BY C.Name, D.Name) AS T
WHERE CountOfIngredients = 1
ORDER BY CountryName, DisributorName


--11. Customers With Countries
CREATE VIEW v_UserWithCountries 
AS
SELECT 
	CU.FirstName + ' ' + CU.LastName AS CustomerName, 
	CU.Age, 
	CU.Gender, 
	CO.Name AS CountryName 
FROM Countries CO
JOIN Customers CU ON CU.CountryId = CO.Id


--12. Delete Products
CREATE TRIGGER tr_DeleteRelations
ON Products INSTEAD OF DELETE
AS
BEGIN
    DECLARE
        @DeletedProductId INT = (SELECT P.Id FROM Products AS P
                                 JOIN deleted AS D ON D.Id = P.Id)
    DELETE
    FROM ProductsIngredients
    WHERE ProductId = @DeletedProductId

    DELETE
    FROM Feedbacks
    WHERE ProductId = @DeletedProductId

    DELETE
    FROM Products
    WHERE Id = @DeletedProductId
END
