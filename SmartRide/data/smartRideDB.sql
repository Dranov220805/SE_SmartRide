IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SmartRide')
BEGIN
    CREATE DATABASE SmartRide;
END
GO

USE SmartRide;
GO

-- Manager table
CREATE TABLE Managers (
    managerId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    accountId UNIQUEIDENTIFIER DEFAULT NULL
);

-- Customer table
CREATE TABLE Customers (
    customerId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    accountId UNIQUEIDENTIFIER DEFAULT NULL
);

-- Driver table
CREATE TABLE Drivers (
    driverId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    accountId UNIQUEIDENTIFIER DEFAULT NULL,
    vehicleId UNIQUEIDENTIFIER DEFAULT NULL,
    availability BIT
);

-- Account table
CREATE TABLE Accounts (
    accountId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    email NVARCHAR(100),
    userName NVARCHAR(50),
    phone NVARCHAR(30),
    password NVARCHAR(100),
    role NVARCHAR(50)
);

-- Vehicle table
CREATE TABLE Vehicles (
    vehicleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    model NVARCHAR(100),
    licensePlate NVARCHAR(20),
    type NVARCHAR(50),
    numberOfSeat NVARCHAR(20),
);

-- Location table
CREATE TABLE Locations (
    locationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    latitude FLOAT DEFAULT NULL,
    longitude FLOAT DEFAULT NULL,
    address NVARCHAR(255)
);

-- Ride table
CREATE TABLE Rides (
    rideId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    userEmail NVARCHAR(100) DEFAULT NULL,
    pickupLocationId UNIQUEIDENTIFIER DEFAULT NULL,
    dropoffLocationId UNIQUEIDENTIFIER DEFAULT NULL,
    pickupDate DATETIME DEFAULT NULL,
    status NVARCHAR(50)
);

-- Payment table
CREATE TABLE Payments (
    paymentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    rideId UNIQUEIDENTIFIER,
    amount FLOAT,
    method NVARCHAR(50),
    status NVARCHAR(50)
);

-- Report table
CREATE TABLE Reports (
    reportId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    managerId UNIQUEIDENTIFIER,
    dateGenerated DATE,
    content NVARCHAR(MAX)
);