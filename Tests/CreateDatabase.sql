USE [master]
GO

IF db_id(N'SqlCommandManager.Tests') IS NULL
CREATE DATABASE [SqlCommandManager.Tests]
GO

USE [SqlCommandManager.Tests]
GO

IF OBJECT_ID(N'dbo.TestValues') IS NULL
CREATE TABLE [dbo].[TestValues]
(
    GuidValue [uniqueidentifier] null,
    StringValue nvarchar(50) null,
    CharValue char(10),
    DateTimeValue datetime,
    DateTimeOffsetValue datetimeoffset(7),
    DecimalValue decimal(18,0),
    DoubleValue float,
    IntValue int,
    TimeSpanValue time(7),
    StreamValue varbinary(MAX)
)
GO



select newID(),
    'Sql Command Manager Test Record',
    CHAR(56),
    GETDATE(),
    SYSDATETIMEOFFSET(),
    47.8,
    153.4532,
    48,
    