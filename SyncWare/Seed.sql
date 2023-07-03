USE tempdb;
DECLARE @SQL NVARCHAR(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = N'evolution')
BEGIN
    SET @SQL = N'USE [evolution];
                 ALTER DATABASE evolution SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE [tempdb];
                 
                 DROP DATABASE evolution;';
    EXEC(@SQL);
END;


CREATE DATABASE evolution;
USE evolution;
CREATE TABLE Consumer(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Organisation UNIQUEIDENTIFIER
);

CREATE TABLE Organisation(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name VARCHAR(50),
    Address VARCHAR(50)
);

DECLARE @OrgId UNIQUEIDENTIFIER;
SET @OrgId = NEWID();

INSERT INTO Organisation VALUES(@OrgId, 'Northcote #1', 'Address Northcote #1')
INSERT INTO Consumer VALUES(NEWID(), 'Ada', 'Ada', @OrgId);