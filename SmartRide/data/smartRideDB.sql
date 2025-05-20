IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SmartRide')
BEGIN
    CREATE DATABASE SmartRide;
END

GO

USE SmartRide;

-- Create Role table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Role')
BEGIN
CREATE TABLE Role (
    role_id INT PRIMARY KEY,
    role_name VARCHAR(30)
);
END

-- Create Account table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Account')
BEGIN
CREATE TABLE Account (
    accountId UNIQUEIDENTIFIER PRIMARY KEY,
    username NVARCHAR(50),
    password NVARCHAR(50),
    hide BIT,
    passwordResetToken VARCHAR(max),
    tokenExpiration VARCHAR(max),
    role_id INT
);
END