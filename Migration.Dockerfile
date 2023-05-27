FROM mcr.microsoft.com/mssql-tools:latest
COPY sql/InitialMigration.sql ./sql/