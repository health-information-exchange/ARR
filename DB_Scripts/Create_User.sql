/****** Object:  Login [ihe]    Script Date: 05/06/2013 08:26:53 ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ihe')
DROP LOGIN [ihe]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [ihe]    Script Date: 05/06/2013 08:26:53 ******/
CREATE LOGIN [ihe] WITH PASSWORD=N'Abcd1234!@#$', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO

ALTER LOGIN [ihe] ENABLE
GO




USE [PerceptiveARR]
GO

/****** Object:  User [ihe]    Script Date: 05/06/2013 08:26:26 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'ihe')
DROP USER [ihe]
GO

USE [PerceptiveARR]
GO

/****** Object:  User [ihe]    Script Date: 05/06/2013 08:26:26 ******/
GO

CREATE USER [ihe] FOR LOGIN [ihe] WITH DEFAULT_SCHEMA=[dbo]
GO

GO
EXEC sp_addrolemember N'db_owner', N'ihe'
GO




USE [PerceptiveARR_Config]
GO

/****** Object:  User [ihe]    Script Date: 10/30/2013 19:51:56 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'ihe')
DROP USER [ihe]
GO

USE [PerceptiveARR_Config]
GO

/****** Object:  User [ihe]    Script Date: 10/30/2013 19:51:56 ******/
GO

CREATE USER [ihe] FOR LOGIN [ihe] WITH DEFAULT_SCHEMA=[dbo]
GO

GO
EXEC sp_addrolemember N'db_owner', N'ihe'
GO

EXEC SP_ADDSRVROLEMEMBER 'ihe', 'sysadmin'

GO