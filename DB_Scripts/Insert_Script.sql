USE [PerceptiveARR_Config]
GO

INSERT INTO dbo.[ClientUser] 
([UserId], [UserName], [UserRole], [Password]) VALUES 
(NEWID(), 'AcuoAdmin', 4, 'Admin1234!@#$')

INSERT INTO [PerceptiveARR_Config].[dbo].[SupportedEventType]
	([SupportedEventTypeID], [EventTypeCode], [EventTypeDisplayName])
	VALUES (NEWID(), '*', '*')

INSERT INTO [PerceptiveARR_Config].[dbo].[SupportedActorElement]
	([ActorElementId], [Code], [CodeSystemName], [CodeDisplayName], [UsedAt], [AllowLog])
	VALUES 
	(NEWID(), '110106',	'DCM', 'Export', 'EventID', 1),
	(NEWID(), 'ITI-55',	'IHE Transactions', 'Cross Gateway Patient Discovery', 'EventTypeCode', 1),
	(NEWID(), 'IHE0006', 'IHE',	'Disclosure', 'EventTypeCode', 1),
	(NEWID(), '110107',	'DCM','Import', 'EventID', 1),
	(NEWID(), '110110',	'DCM', 'Patient Record', 'EventID', 1),
	(NEWID(), '110112',	'DCM', 'Query',	'EventID', 1),
	(NEWID(), '110114',	'DCM', 'UserAuthenticated',	'EventID', 1),
	(NEWID(), '110123',	'DCM', 'Login',	'EventTypeCode', 1),
	(NEWID(), '110150',	'DCM', 'Application', 'SourceParticipant', 1),
	(NEWID(), '110152',	'DCM', 'Destination', 'SourceParticipant', 1),
	(NEWID(), '110153',	'DCM', 'Source', 'SourceParticipant', 1),
	(NEWID(), '110122',	'DCM', 'Login',	'EventTypeCode',1),
	(NEWID(), '110123', 'DCM',	'Logout', 'EventTypeCode',1),
	(NEWID(), 'ITI-51',	'IHE Transactions',	'Multi-Patient Query', 'EventTypeCode',1),
	(NEWID(), 'ITI-22',	'IHE Transactions',	'Patient Demographics and Visit Query',	'EventTypeCode',1),
	(NEWID(), 'ITI-21',	'IHE Transactions',	'Patient Demographics Query', 'EventTypeCode', 1),
	(NEWID(), 'ITI-47',	'IHE Transactions',	'Patient Demographics Query', 'EventTypeCode', 1),
	(NEWID(), 'ITI-44',	'IHE Transactions',	'Patient Identity Feed', 'EventTypeCode', 1),
	(NEWID(), 'ITI-8',	'IHE Transactions',	'Patient Identity Feed', 'EventTypeCode', 1),
	(NEWID(), 'ITI-45',	'IHE Transactions',	'PIX Query', 'EventTypeCode', 1),
	(NEWID(), 'ITI-9',	'IHE Transactions',	'PIX Query', 'EventTypeCode', 1),
	(NEWID(), 'ITI-10',	'IHE Transactions',	'PIX Update Notification', 'EventTypeCode', 1),
	(NEWID(), 'ITI-46',	'IHE Transactions',	'PIX Update Notification', 'EventTypeCode', 1),
	(NEWID(), 'ITI-41',	'IHE Transactions',	'Provide and Register Document Set-b', 'EventTypeCode', 1),
	(NEWID(), 'ITI-42',	'IHE Transactions',	'Register Document Set-b', 'EventTypeCode', 1),
	(NEWID(), 'ITI-61',	'IHE Transactions',	'Register On-Demand Document Entry', 'EventTypeCode', 1),
	(NEWID(), 'ITI-18',	'IHE Transactions',	'Registry Stored Query', 'EventTypeCode', 1),
	(NEWID(), 'ITI-43',	'IHE Transactions',	'Retrieve Document Set', 'EventTypeCode', 1),
	(NEWID(), 'ITI-60',	'IHE Transactions',	'Retrieve Multiple Value Sets',	'EventTypeCode', 1),
	(NEWID(), 'ITI-48',	'IHE Transactions',	'Retrieve Value Sets', 'EventTypeCode', 1),
	(NEWID(), 'ITI-52',	'IHE Transactions',	'Document Metadata Subscribe', 'EventTypeCode', 1),
	(NEWID(), 'ITI-53',	'IHE Transactions',	'Document Metadata Notify', 'EventTypeCode', 1),
	(NEWID(), 'ITI-54',	'IHE Transactions',	'Document Metadata Publish', 'EventTypeCode', 1),
	(NEWID(), 'ITI-56',	'IHE Transactions',	'Patient Location Query', 'EventTypeCode', 1),
	(NEWID(), 'ITI-57',	'IHE Transactions',	'Update Document Set', 'EventTypeCode', 1),
	(NEWID(), 'ITI-59',	'IHE Transactions',	'Provider Information Feed', 'EventTypeCode', 1),
	(NEWID(), 'ITI-63',	'IHE Transactions',	'XCF Fetch Intermediate Document Creation', 'EventTypeCode', 1),
	(NEWID(), 'ITI-63',	'IHE Transactions',	'XCF Fetch', 'EventTypeCode', 1);

GO


