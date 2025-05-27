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
    accountId UNIQUEIDENTIFIER DEFAULT NULL,
    managerName VARCHAR(100),
    email VARCHAR(100)
);

-- Customer table
CREATE TABLE Customers (
    customerId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    accountId UNIQUEIDENTIFIER DEFAULT NULL,
    customerName VARCHAR(100),
    phone VARCHAR(15),
    email VARCHAR(100)
);

-- Driver table
CREATE TABLE Drivers (
    driverId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    accountId UNIQUEIDENTIFIER DEFAULT NULL,
    driverName VARCHAR(100),
    phone VARCHAR(15),
    licenseNumber VARCHAR(50),
    availability BIT
);

-- Account table
CREATE TABLE Accounts (
    accountId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    email VARCHAR(100),
    userName VARCHAR(50),
    password VARCHAR(100),
    role VARCHAR(50)
);

-- Vehicle table
CREATE TABLE Vehicles (
    vehicleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    model VARCHAR(100),
    licensePlate VARCHAR(20),
    type VARCHAR(50)
);

-- Location table
CREATE TABLE Locations (
    locationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    latitude FLOAT,
    longitude FLOAT,
    address VARCHAR(255)
);

-- Ride table
CREATE TABLE Rides (
    rideId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    pickupLocation VARCHAR(MAX),
    dropoffLocation VARCHAR(MAX),
    status VARCHAR(50)
);

-- Payment table
CREATE TABLE Payments (
    paymentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    rideId UNIQUEIDENTIFIER REFERENCES Rides(rideId),
    amount FLOAT,
    method VARCHAR(50),
    status VARCHAR(50)
);

-- Report table
CREATE TABLE Reports (
    reportId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    managerId UNIQUEIDENTIFIER REFERENCES Managers(managerId),
    dateGenerated DATE,
    content VARCHAR(MAX)
);

-- Insert Managers
INSERT INTO Managers (managerName, email) VALUES
('Alice Johnson', 'alice@example.com'),
('Bob Smith', 'bob@example.com');

-- Insert Customers
INSERT INTO Customers (customerName, phone, email) VALUES
('Charlie Brown', '1234567890', 'charlie@example.com'),
('Dana White', '0987654321', 'dana@example.com');

-- Insert Drivers
INSERT INTO Drivers (driverName, phone, licenseNumber, availability) VALUES
('Eddie Drive', '1112223333', 'L123456', 1),
('Fiona Wheel', '2223334444', 'L654321', 0);

-- Insert Accounts
INSERT INTO Accounts (userName, password, role) VALUES
('charlie_user', 'hashed_password1', 'customer'),
('dana_user', 'hashed_password2', 'customer'),
('eddie_driver', 'hashed_password3', 'driver'),
('fiona_driver', 'hashed_password4', 'driver'),
('alice_mgr', 'hashed_password5', 'manager');

-- Insert Vehicles
INSERT INTO Vehicles (model, licensePlate, type) VALUES
('Toyota Prius', 'ABC123', 'Sedan'),
('Honda Civic', 'XYZ789', 'Sedan');

-- Insert Locations
INSERT INTO Locations (latitude, longitude, address) VALUES
(37.7749, -122.4194, 'San Francisco, CA'),
(34.0522, -118.2437, 'Los Angeles, CA'),
(40.7128, -74.0060, 'New York, NY'),
(41.8781, -87.6298, 'Chicago, IL');