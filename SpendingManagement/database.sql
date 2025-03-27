-- Check if database exists, if yes, drop it
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SpendingManagement')
BEGIN
    ALTER DATABASE SpendingManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SpendingManagement;
END
GO

-- Create a new database
CREATE DATABASE SpendingManagement;
GO
USE SpendingManagement;
GO

-- Table: Users
CREATE TABLE Users (
    id         INT IDENTITY(1,1) PRIMARY KEY,
    username   NVARCHAR(50) UNIQUE NOT NULL,
    email      NVARCHAR(100) UNIQUE NOT NULL,
    password   NVARCHAR(255) NOT NULL,
    fullname   NVARCHAR(100),
    createdAt  DATETIME2 DEFAULT SYSDATETIME()
);
GO

-- Table: Wallets
CREATE TABLE Wallets (
    id       INT IDENTITY(1,1) PRIMARY KEY,
    userID   INT NOT NULL,
    name     NVARCHAR(100) NOT NULL,
    balance  DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (userID) REFERENCES Users(id) ON DELETE NO ACTION
);
GO

-- Table: Categories
CREATE TABLE Categories (
    id       INT IDENTITY(1,1) PRIMARY KEY,
    name     NVARCHAR(50) NOT NULL,
    type     NVARCHAR(10) NOT NULL,
    icon     NVARCHAR(100), 
    parentId INT NULL,
    FOREIGN KEY (parentId) REFERENCES Categories(id)
);
GO

-- Table: Transactions
CREATE TABLE Transactions (
    id         INT IDENTITY(1,1) PRIMARY KEY,
    userID     INT NOT NULL,
    walletID   INT NOT NULL,
    categoryID INT NOT NULL,
    amount     DECIMAL(18,2) NOT NULL,
    type       NVARCHAR(10) CHECK (type IN ('income', 'expense', 'transfer')) NOT NULL,
    note       NVARCHAR(MAX),
    date       DATE NOT NULL,
    FOREIGN KEY (userID) REFERENCES Users(id) ON DELETE CASCADE,
    FOREIGN KEY (walletID) REFERENCES Wallets(id) ON DELETE CASCADE,
    FOREIGN KEY (categoryID) REFERENCES Categories(id) ON DELETE NO ACTION
);
GO

-- Table: Budgets
CREATE TABLE Budgets (
    id         INT IDENTITY(1,1) PRIMARY KEY,
    userID     INT NOT NULL,
    categoryID INT NOT NULL,
    amount     DECIMAL(18,2) NOT NULL,
    startDate  DATE NOT NULL,
    endDate    DATE NOT NULL,
    createAt   DATETIME2 DEFAULT SYSDATETIME(),
    FOREIGN KEY (userID) REFERENCES Users(id) ON DELETE CASCADE,
    FOREIGN KEY (categoryID) REFERENCES Categories(id) ON DELETE NO ACTION
);
GO

-- Insert Expense Categories
INSERT INTO Categories (name, type) VALUES 
('Food and Beverage', 'expense'),
('Rentals', 'expense'),
('Water Bill', 'expense'),
('Phone Bill', 'expense'),
('Electricity Bill', 'expense'),
('Gas Bill', 'expense'),
('Television Bill', 'expense'),
('Internet Bill', 'expense'),
('Other Utility Bills', 'expense'),
('Personal Items', 'expense'),
('Shopping', 'expense'),
('Houseware', 'expense'),
('Makeup', 'expense'),
('Family', 'expense'),
('Home Maintenance', 'expense'),
('Home Services', 'expense'),
('Pets', 'expense'),
('Transportation', 'expense'),
('Vehicle Maintenance', 'expense'),
('Medical Check-up', 'expense'),
('Fitness', 'expense'),
('Education', 'expense'),
('Entertainment', 'expense'),
('Gift and Donations', 'expense'),
('Insurances', 'expense'),
('Investment', 'expense'),
('Other Expense', 'expense'),
('Outgoing Transfer', 'expense'),
('Pay Interest', 'expense'),
('Uncategorized', 'expense');

-- Insert Income Categories
INSERT INTO Categories (name, type) VALUES 
('Salary', 'income'),
('Other Income', 'income'),
('Incoming Transfer', 'income'),
('Collect Interest', 'income'),
('Uncategorized', 'income');

select * from Categories
select * from Wallets
select * from Users
