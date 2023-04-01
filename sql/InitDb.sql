SET QUOTED_IDENTIFIER ON;

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DomiviceUsers')
BEGIN
        CREATE DATABASE [DomiviceUsers]
END;
GO

USE [DomiviceUsers]
GO
