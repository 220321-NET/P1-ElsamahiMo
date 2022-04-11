CREATE TABLE Customers (
    id INT PRIMARY KEY IDENTITY(1,1),
    customerName VARCHAR(50) NOT NULL UNIQUE,
    customerPass VARCHAR(50) NOT NULL,
)

INSERT INTO Customers(customerName, customerPass) VALUES(
    'Mo',
    'Qwerty'
)

INSERT INTO Customers(customerName, customerPass) VALUES(
    'Kendall',
    'cAtS'
)

SELECT * FROM Customers

DROP TABLE Customers

--------------------------------------------------------------------------------

CREATE TABLE Products(
    id INT PRIMARY KEY IDENTITY (1,1),
    productName VARCHAR(50) NOT NULL,
    price FLOAT NOT NULL

)

INSERT INTO Products(productName, price) VALUES(
    'Lethal League Blaze',
    14.99
)

INSERT INTO Products(productName, price) VALUES(
    'The Binding of Issac: Repentance',
    59.99
)

INSERT INTO Products(productName, price) VALUES(
    'Outer Wilds',
    24.99
)

INSERT INTO Products(productName, price) VALUES(
    'Elden Ring',
    59.99
)


SELECT * FROM Products

DROP TABLE Products

--------------------------------------------------------------------------------

CREATE TABLE Stores(
    id INT PRIMARY KEY IDENTITY (1,1),
    storeLocation VARCHAR (100) NOT NULL
)

INSERT INTO Stores(storeLocation) VALUES(
    'New Jersey'
)

INSERT INTO Stores(storeLocation) VALUES(
    'Florida'
)

INSERT INTO Stores(storeLocation) VALUES(
    'Kentucky'
)

SELECT * FROM Stores

DROP TABLE Stores

--------------------------------------------------------------------------------

CREATE TABLE Inventory(
    id INT PRIMARY KEY IDENTITY (1,1),
    quantity INT NOT NULL,
    productID INT FOREIGN KEY REFERENCES Products(id),
    storeID INT NOT NULL FOREIGN KEY REFERENCES Stores(id) ON DELETE CASCADE
) 

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    1,
    10,
    1
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    2,
    15,
    1
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    3,
    5,
    1
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    6,
    9,
    1
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    4,
    20,
    1
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    3,
    15,
    2
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    5,
    25,
    2
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    6,
    15,
    2
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    1,
    20,
    3
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    2,
    5,
    3
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    4,
    30,
    3
)

INSERT INTO Inventory(productID, quantity, storeID) VALUES(
    6,
    30,
    3
)

SELECT * FROM Inventory

DROP TABLE Inventory 

--------------------------------------------------------------------------------

CREATE TABLE Orders(
    id INT PRIMARY KEY IDENTITY (1,1),
    dateCreated DATETIME NULL,
    total FLOAT,
    storeID INT NOT NULL FOREIGN KEY REFERENCES Stores(id),
    customerID INT NOT NULL FOREIGN KEY REFERENCES Customers(id) ON DELETE CASCADE
)

SELECT * FROM Orders 

DROP TABLE Orders
