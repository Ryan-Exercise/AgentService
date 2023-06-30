DROP DATABASE Evolution;
CREATE DATABASE evolution;
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

SELECT * FROM Consumer;



# ALTER DATABASE evolution  
# SET CHANGE_TRACKING = ON  
# (CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);

# ALTER DATABASE Evolution  
# SET CHANGE_TRACKING = OFF;

# SELECT *
# FROM sys.change_tracking_databases
# WHERE database_id = DB_ID('evolution');

