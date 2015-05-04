USE [master]
GO

/****** Object:  Database [PerceptiveARR]    Script Date: 05/06/2013 08:23:53 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'PerceptiveARR')
DROP DATABASE [PerceptiveARR]
GO

USE [master]
GO

/****** Object:  Database [PerceptiveARR]    Script Date: 05/06/2013 08:23:53 ******/
CREATE DATABASE [PerceptiveARR];
GO

USE [master]
GO

/****** Object:  Database [PerceptiveARR_Config]    Script Date: 02/06/2014 14:51:56 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'PerceptiveARR_Config')
DROP DATABASE [PerceptiveARR_Config]
GO

USE [master]
GO

/****** Object:  Database [PerceptiveARR_Config]    Script Date: 02/06/2014 14:51:56 ******/
CREATE DATABASE [PerceptiveARR_Config];
GO