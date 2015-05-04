/********************** Creating PerceptiveARR Config Tables ************************************/

USE [PerceptiveARR_Config]
GO

/****** Object:  Table [dbo].[SupportedEventType]    Script Date: 02/06/2014 15:00:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SupportedEventType](
	[SupportedEventTypeID] [uniqueidentifier] NOT NULL,
	[EventTypeCode] [nvarchar](max) NULL,
	[EventTypeDisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_SupportedEventType] PRIMARY KEY CLUSTERED 
(
	[SupportedEventTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [PerceptiveARR_Config]
GO

/****** Object: Table [dbo].[SupportedActorElement]	  Script Date: 04/01/2015 12:17:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SupportedActorElement](
	[ActorElementId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[CodeSystemName] [nvarchar](max) NOT NULL,
	[CodeDisplayName] [nvarchar](max) NOT NULL,
	[UsedAt] [nvarchar](max) NOT NULL,
	[AllowLog] [bit] NOT NULL,
 CONSTRAINT [ActorElementId] PRIMARY KEY CLUSTERED 
(
	[ActorElementId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [PerceptiveARR_Config]
GO

/****** Object:  Table [dbo].[AppSettings]    Script Date: 02/07/2014 14:38:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AppSettings](
	[AppSettingsID] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AppSettings] PRIMARY KEY NONCLUSTERED 
(
	[AppSettingsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [PerceptiveARR_Config]
GO

/****** Object:  Table [dbo].[ClientUser]    Script Date: 02/06/2014 15:00:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientUser](
	[UserID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[UserRole] [int] NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ClientUser] PRIMARY KEY NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PerceptiveARR_Config]
GO

/****** Object:  Table [dbo].[UserActiveDatabase]    Script Date: 05/26/2014 15:57:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserActiveDatabase](
	[UserName] [nvarchar](256) NOT NULL,
	[ActiveDatabase] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserActiveDatabase] PRIMARY KEY NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/********************** Creating PerceptiveARR Tables ************************************/

USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[LogRecorder]    Script Date: 02/06/2014 15:05:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LogRecorder](
	[LogID] [uniqueidentifier] NOT NULL,
	[RemoteIP] [nvarchar](max) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[LogType] [nvarchar](max) NOT NULL,
	[IsValid] [bit] NOT NULL,
 CONSTRAINT [PK_LogRecorder] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[InvalidLogs]    Script Date: 02/06/2014 15:05:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InvalidLogs](
	[LogID] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_InvalidLogs] PRIMARY KEY NONCLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[InvalidLogs]  WITH CHECK ADD  CONSTRAINT [FK_InvalidLogs_LogRecorder1] FOREIGN KEY([LogID])
REFERENCES [dbo].[LogRecorder] ([LogID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[InvalidLogs] CHECK CONSTRAINT [FK_InvalidLogs_LogRecorder1]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ValidLogs]    Script Date: 02/06/2014 15:05:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ValidLogs](
	[LogID] [uniqueidentifier] NOT NULL,
	[Pri] [nvarchar](max) NOT NULL,
	[Version] [int] NULL,
	[Timestamp] [nvarchar](max) NOT NULL,
	[HostName] [nvarchar](max) NOT NULL,
	[AppName] [nvarchar](max) NOT NULL,
	[ProcessID] [nvarchar](max) NOT NULL,
	[MessageID] [nvarchar](max) NOT NULL,
	[StructuredData] [nvarchar](max) NOT NULL,
	[IsDicomFormat] [bit] NOT NULL,
	[Data] [nvarchar](max) NOT NULL
 CONSTRAINT [PK_ValidLogs] PRIMARY KEY NONCLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ValidLogs]  WITH CHECK ADD  CONSTRAINT [FK_ValidLogs_LogRecorder] FOREIGN KEY([LogID])
REFERENCES [dbo].[LogRecorder] ([LogID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ValidLogs] CHECK CONSTRAINT [FK_ValidLogs_LogRecorder]
GO

USE [PerceptiveARR]
GO


/****** Object:  Table [dbo].[SupportedActorElement]    Script Date: 02/06/2014 15:07:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SupportedActorElement](
	[ActorElementID] [uniqueidentifier] NOT NULL,
	[ActorElementTypeID] [int] NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[CodeSystemName] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_ActorElement] PRIMARY KEY NONCLUSTERED 
(
	[ActorElementID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ObjectDetailElement]    Script Date: 02/06/2014 15:07:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ObjectDetailElement](
	[ObjectDetailElementID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectIdentificationID] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_ObjectDetailElement_1] PRIMARY KEY NONCLUSTERED 
(
	[ObjectDetailElementID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO





/****** Object:  Table [dbo].[EventIdentification]    Script Date: 02/06/2014 15:06:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventIdentification](
	[LogID] [uniqueidentifier] NOT NULL,
	[EventID] [uniqueidentifier] NOT NULL,
	[EventActionCode] [nvarchar](max) NULL,
	[EventDateTime] [nvarchar](max) NULL,
	[EventOutcomeIndicator] [nvarchar](max) NULL,
	[EventTypeCodeID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EventIdentification] PRIMARY KEY NONCLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EventIdentification]  WITH CHECK ADD  CONSTRAINT [FK_EventIdentification_ActorElement_EventID] FOREIGN KEY([EventID])
REFERENCES [dbo].[SupportedActorElement] ([ActorElementID])
GO

ALTER TABLE [dbo].[EventIdentification] CHECK CONSTRAINT [FK_EventIdentification_ActorElement_EventID]
GO

ALTER TABLE [dbo].[EventIdentification]  WITH CHECK ADD  CONSTRAINT [FK_EventIdentification_ActorElement_EventTypeCodeID] FOREIGN KEY([EventTypeCodeID])
REFERENCES [dbo].[SupportedActorElement] ([ActorElementID])
GO

ALTER TABLE [dbo].[EventIdentification] CHECK CONSTRAINT [FK_EventIdentification_ActorElement_EventTypeCodeID]
GO

ALTER TABLE [dbo].[EventIdentification]  WITH CHECK ADD  CONSTRAINT [FK_EventIdentification_ValidLogs] FOREIGN KEY([LogID])
REFERENCES [dbo].[ValidLogs] ([LogID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EventIdentification] CHECK CONSTRAINT [FK_EventIdentification_ValidLogs]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[AuditSourceIdentification]    Script Date: 02/06/2014 15:06:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditSourceIdentification](
	[LogID] [uniqueidentifier] NOT NULL,
	[AuditSourceID] [nvarchar](max) NULL,
	[AuditEnterpriseSiteID] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[CodeSystemName] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditSourceIdentification] PRIMARY KEY NONCLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AuditSourceIdentification]  WITH CHECK ADD  CONSTRAINT [FK_AuditSourceIdentification_ValidLogs] FOREIGN KEY([LogID])
REFERENCES [dbo].[ValidLogs] ([LogID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuditSourceIdentification] CHECK CONSTRAINT [FK_AuditSourceIdentification_ValidLogs]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ParticipantObjectIdentification]    Script Date: 02/06/2014 15:06:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ParticipantObjectIdentification](
	[ParticipantObjectIdentificationID] [uniqueidentifier] NOT NULL,
	[LogID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectTypeCode] [nvarchar](max) NULL,
	[ParticipantObjectTypeCodeRole] [nvarchar](max) NULL,
	[ParticipantObjectDataLifeCycle] [nvarchar](max) NULL,
	[ParticipantObjectIDTypeCode] [uniqueidentifier] NOT NULL,
	[ParticipantObjectSensitivity] [nvarchar](max) NULL,
	[ParticipantObjectID] [nvarchar](max) NULL,
	[ParticipantObjectName] [nvarchar](max) NULL,
	[ParticipantObjectQuery] [nvarchar](max) NULL,
 CONSTRAINT [PK_ParticipantObjectIdentification] PRIMARY KEY NONCLUSTERED 
(
	[ParticipantObjectIdentificationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ParticipantObjectIdentification]  WITH CHECK ADD  CONSTRAINT [FK_ParticipantObjectIdentification_ValidLogs] FOREIGN KEY([LogID])
REFERENCES [dbo].[ValidLogs] ([LogID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ParticipantObjectIdentification] CHECK CONSTRAINT [FK_ParticipantObjectIdentification_ValidLogs]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ActiveParticipant]    Script Date: 02/06/2014 15:07:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActiveParticipant](
	[ActiveParticipantID] [uniqueidentifier] NOT NULL,
	[LogID] [uniqueidentifier] NOT NULL,
	[UserID] [nvarchar](max) NOT NULL,
	[AlternativeUserID] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[UserIsRequestor] [bit] NOT NULL,
	[RoleIDCode] [uniqueidentifier] NULL,
	[NetworkAccessPointTypeCode] [nvarchar](max) NULL,
	[NetworkAccesPointID] [nvarchar](max) NULL,
 CONSTRAINT [PK_ActiveParticipant] PRIMARY KEY NONCLUSTERED 
(
	[ActiveParticipantID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ActiveParticipant]  WITH CHECK ADD  CONSTRAINT [FK_ActiveParticipant_ActorElement] FOREIGN KEY([RoleIDCode])
REFERENCES [dbo].[SupportedActorElement] ([ActorElementID])
GO

ALTER TABLE [dbo].[ActiveParticipant] CHECK CONSTRAINT [FK_ActiveParticipant_ActorElement]
GO

ALTER TABLE [dbo].[ActiveParticipant]  WITH CHECK ADD  CONSTRAINT [FK_ActiveParticipant_ValidLogs] FOREIGN KEY([LogID])
REFERENCES [dbo].[ValidLogs] ([LogID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ActiveParticipant] CHECK CONSTRAINT [FK_ActiveParticipant_ValidLogs]
GO


USE [PerceptiveARR]
GO

USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ParticipantObjectDescription]    Script Date: 02/07/2014 12:37:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ParticipantObjectDescription](
	[ParticipantObjectDescriptionID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectIdentificationID] [uniqueidentifier] NOT NULL,
	[Encrypted] [bit] NULL,
	[Anonymized] [bit] NULL,
 CONSTRAINT [PK_ParticipantObjectDescription] PRIMARY KEY NONCLUSTERED 
(
	[ParticipantObjectDescriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ParticipantObjectDescription]  WITH CHECK ADD  CONSTRAINT [FK_ParticipantObjectDescription_ParticipantObjectIdentification] FOREIGN KEY([ParticipantObjectIdentificationID])
REFERENCES [dbo].[ParticipantObjectIdentification] ([ParticipantObjectIdentificationID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ParticipantObjectDescription] CHECK CONSTRAINT [FK_ParticipantObjectDescription_ParticipantObjectIdentification]
GO



USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[Accession]    Script Date: 02/07/2014 12:43:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Accession](
	[AccessionID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectDescriptionID] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](max) NULL,
 CONSTRAINT [PK_Accession] PRIMARY KEY CLUSTERED 
(
	[AccessionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Accession]  WITH CHECK ADD  CONSTRAINT [FK_Accession_ParticipantObjectDescription] FOREIGN KEY([ParticipantObjectDescriptionID])
REFERENCES [dbo].[ParticipantObjectDescription] ([ParticipantObjectDescriptionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Accession] CHECK CONSTRAINT [FK_Accession_ParticipantObjectDescription]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[SOPClass]    Script Date: 02/07/2014 12:45:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SOPClass](
	[SOPClassID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectDescriptionID] [uniqueidentifier] NOT NULL,
	[UID] [nvarchar](max) NULL,
	[NumberOfInstances] [int] NOT NULL,
 CONSTRAINT [PK_SOPClass] PRIMARY KEY CLUSTERED 
(
	[SOPClassID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SOPClass]  WITH CHECK ADD  CONSTRAINT [FK_SOPClass_ParticipantObjectDescription] FOREIGN KEY([ParticipantObjectDescriptionID])
REFERENCES [dbo].[ParticipantObjectDescription] ([ParticipantObjectDescriptionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SOPClass] CHECK CONSTRAINT [FK_SOPClass_ParticipantObjectDescription]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[Instance]    Script Date: 02/07/2014 12:47:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Instance](
	[InstanceID] [uniqueidentifier] NOT NULL,
	[SOPCLassID] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[UID] [nvarchar](max) NULL,
 CONSTRAINT [PK_Instance] PRIMARY KEY CLUSTERED 
(
	[InstanceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Instance]  WITH CHECK ADD  CONSTRAINT [FK_Instance_SOPClass] FOREIGN KEY([SOPCLassID])
REFERENCES [dbo].[SOPClass] ([SOPClassID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Instance] CHECK CONSTRAINT [FK_Instance_SOPClass]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ParticipantObjectContainsStudy]    Script Date: 02/07/2014 12:50:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ParticipantObjectContainsStudy](
	[ParticipantObjectContainsStudyID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectDescriptionID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ParticipantObjectContainsStudy] PRIMARY KEY CLUSTERED 
(
	[ParticipantObjectContainsStudyID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ParticipantObjectContainsStudy]  WITH CHECK ADD  CONSTRAINT [FK_ParticipantObjectContainsStudy_ParticipantObjectDescription] FOREIGN KEY([ParticipantObjectDescriptionID])
REFERENCES [dbo].[ParticipantObjectDescription] ([ParticipantObjectDescriptionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ParticipantObjectContainsStudy] CHECK CONSTRAINT [FK_ParticipantObjectContainsStudy_ParticipantObjectDescription]
GO


USE [PerceptiveARR]
GO

/****** Object:  Table [dbo].[ObjectIdentifier]    Script Date: 02/07/2014 12:51:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ObjectIdentifier](
	[ObjectIdentifierID] [uniqueidentifier] NOT NULL,
	[ParticipantObjectDescriptionID] [uniqueidentifier] NULL,
	[ParticipantObjectContainsStudyID] [uniqueidentifier] NULL,
	[ObjectIdentifierType] [int] NOT NULL,
	[UID] [nvarchar](max) NULL,
 CONSTRAINT [PK_ObjectIdentifier] PRIMARY KEY CLUSTERED 
(
	[ObjectIdentifierID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ObjectIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ObjectIdentifier_ParticipantObjectContainsStudy] FOREIGN KEY([ParticipantObjectContainsStudyID])
REFERENCES [dbo].[ParticipantObjectContainsStudy] ([ParticipantObjectContainsStudyID])
GO

ALTER TABLE [dbo].[ObjectIdentifier] CHECK CONSTRAINT [FK_ObjectIdentifier_ParticipantObjectContainsStudy]
GO

ALTER TABLE [dbo].[ObjectIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_ObjectIdentifier_ParticipantObjectDescription] FOREIGN KEY([ParticipantObjectDescriptionID])
REFERENCES [dbo].[ParticipantObjectDescription] ([ParticipantObjectDescriptionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ObjectIdentifier] CHECK CONSTRAINT [FK_ObjectIdentifier_ParticipantObjectDescription]
GO




ALTER TABLE [dbo].[ObjectDetailElement]  WITH CHECK ADD  CONSTRAINT [FK_ObjectDetailElement_ParticipantObjectIdentification] FOREIGN KEY([ParticipantObjectIdentificationID])
REFERENCES [dbo].[ParticipantObjectIdentification] ([ParticipantObjectIdentificationID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ObjectDetailElement] CHECK CONSTRAINT [FK_ObjectDetailElement_ParticipantObjectIdentification]
GO

ALTER TABLE [dbo].[ParticipantObjectIdentification]  WITH CHECK ADD  CONSTRAINT [FK_ParticipantObjectIdentification_ActorElement] FOREIGN KEY([ParticipantObjectIDTypeCode])
REFERENCES [dbo].[SupportedActorElement] ([ActorElementID])
GO

ALTER TABLE [dbo].[ParticipantObjectIdentification] CHECK CONSTRAINT [FK_ParticipantObjectIdentification_ActorElement]
GO