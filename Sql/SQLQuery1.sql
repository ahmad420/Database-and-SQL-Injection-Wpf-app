
--USERS TABLE
CREATE TABLE Users (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  FullName NVARCHAR(100) NOT NULL,
  UserName NVARCHAR(50) NOT NULL UNIQUE,
  Email NVARCHAR(50) NOT NULL UNIQUE,
  PasswordHash NVARCHAR(200) NOT NULL
);

--ADMINS TABLE
CREATE TABLE Admins (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  UserName NVARCHAR(50) NOT NULL UNIQUE,
  PasswordHash NVARCHAR(200) NOT NULL
);

-- UPADTE USER QUERY EXAMPLE
UPDATE users
SET 
email = 'faf@example.com'
WHERE UserName = 'jdoe';


--GET ALL ADMINS 
select * from admins


--GET ALL USERS 
select * from Users


-- Add User PROCEDURE 
CREATE PROCEDURE AddUser
    @FullName NVARCHAR(50),
    @Username NVARCHAR(20),
    @Email NVARCHAR(50),
    @PasswordHash NVARCHAR(100)
AS
BEGIN
    INSERT INTO Users (FullName, Username, Email, PasswordHash)
    VALUES (@FullName, @Username, @Email, @PasswordHash)
END


--Select USER PasswordHash By Username
CREATE PROCEDURE SelectUserPasswordHashByUsername
    @UserName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT PasswordHash FROM Users
    WHERE UserName = @UserName;
END




--Select Admin PasswordHash By Username
CREATE PROCEDURE SelectAdminPasswordHashByUsername
    @UserName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT PasswordHash FROM Admins
    WHERE UserName = @UserName;
END


--UPDATE USER EMAIL  PROCEDURE
CREATE PROCEDURE UpdateUserEmail
    @userId INT,
    @newEmail NVARCHAR(100)
AS
BEGIN
    UPDATE Users
    SET Email = @newEmail
    WHERE Id = @userId
END