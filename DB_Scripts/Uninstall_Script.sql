USE master;
GO

DECLARE @db_Name NVARCHAR(MAX) --database name

DECLARE db_cursor CURSOR FOR
SELECT [ActiveDatabase]
FROM [PerceptiveARR_Config].[dbo].[UserActiveDatabase]

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @db_Name

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC('ALTER DATABASE ' + @db_Name + ' SET SINGLE_USER WITH ROLLBACK IMMEDIATE;');	
	EXEC('DROP DATABASE ' + @db_Name);
END

CLOSE db_cursor
DEALLOCATE db_cursor

ALTER DATABASE [PerceptiveARR] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;	
DROP DATABASE [PerceptiveARR];
	
ALTER DATABASE [PerceptiveARR_Config] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;	
DROP DATABASE [PerceptiveARR_Config];

USE [master]; 
/****** Object:  Login [ihe]    Script Date: 05/06/2013 08:26:53 ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ihe')
DROP LOGIN [ihe]
GO

