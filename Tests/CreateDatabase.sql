IF db_id(N'SqlCommandDatabase') IS NULL
BEGIN
    CREATE DATABASE 
        SqlCommandDatabase 
    ON PRIMARY
    (
        NAME = SqlCommandDatabase,
        FILENAME = '{folderPath}\SqlCommandDatabaseData.mdf',
        SIZE = 2MB, 
        MAXSIZE = 10MB, 
        FILEGROWTH = 10 %
    )
    LOG ON
    (
        NAME = SqlCommandDatabase_Log,
        FILENAME = '{folderPath}\SqlCommandDatabase_Log.ldf',
        SIZE = 1MB,
        MAXSIZE = 5MB,
        FILEGROWTH = 10 %
    )
END
GO

IF OBJECT_ID(N'dbo.User') IS NULL
BEGIN
    create table [User]
    (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        FirstName varchar(255),
        LastName varchar(255),
        DateOfBirth date,
        LastLoginTime datetimeoffset,
    )

END