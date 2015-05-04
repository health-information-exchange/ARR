using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Perceptive.ARR.DataModel;
using System.Transactions;

namespace Perceptive.ARR.HelperLibrary
{
    public class DBLogger
    {
        public bool RecordAuditTrail(RepositoryRequest request)
        {
            var supportedLogElementTypes = new List<SupportedLogElement>();
            using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
            {
                foreach (var supportedType in configModel.SupportedActorElements)
                {
                    supportedLogElementTypes.Add(new SupportedLogElement()
                    {
                        Id = supportedType.ActorElementId,
                        Code = supportedType.Code,
                        SystemName = supportedType.CodeSystemName,
                        DisplayName = supportedType.CodeDisplayName,
                        ElementType = supportedType.UsedAt,
                        AllowLog = supportedType.AllowLog
                    });
                }
            }

            var comparer = new SupportedLogElementComparer();
            supportedLogElementTypes.Sort(comparer);

            int notAllowedAuditSourceCount = supportedLogElementTypes.Count(s => s.ElementType.Equals("SourceParticipant", StringComparison.OrdinalIgnoreCase) && !s.AllowLog);
            int notAllowedEventIDCount = supportedLogElementTypes.Count(s => s.ElementType.Equals("EventID", StringComparison.OrdinalIgnoreCase) && !s.AllowLog);
            int notAllowedEventTypeCodeCount = supportedLogElementTypes.Count(s => s.ElementType.Equals("EventTypeCode", StringComparison.OrdinalIgnoreCase) && !s.AllowLog);
            
            bool success = false;
            using (var dataModel = new Entities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Active)))
            {
                try
                {
                    Guid logId = Guid.NewGuid();

                    # region LogRecorder

                    var logRecorder = new LogRecorder();
                    logRecorder.LogID = logId;
                    logRecorder.RemoteIP = request.IP;
                    logRecorder.LogType = request.Protocol.ToString();
                    logRecorder.IsValid = request.ValidMessage != null;
                    logRecorder.LogDateTime = request.RequestReceiveDateTime;

                    # endregion

                    if (request.ValidMessage == null)
                    {
                        dataModel.LogRecorders.Add(logRecorder);

                        # region InvalidLog

                        var invalidLog = new InvalidLog();
                        invalidLog.LogID = logId;
                        invalidLog.Data = request.Data;

                        dataModel.InvalidLogs.Add(invalidLog);

                        # endregion
                    }
                    else
                    {
                        # region Supported Log Type Check

                        bool isSupported = true;

                        if (supportedLogElementTypes.Count(s => !s.AllowLog) != 0)
                        {
                            string messageEventTypeCode = string.Empty;
                            string messageEventTypeCodeSystemName = string.Empty;
                            string messageEventTypeDisplayName = string.Empty;

                            string messageEventIdCode = string.Empty;
                            string messageEventIdCodeSystemName = string.Empty;
                            string messageEventIdDisplayName = string.Empty;

                            string messageAuditSourceCode = string.Empty;
                            string messageAuditSourceCodeSystemName = string.Empty;
                            string messageAuditSourceDisplayName = string.Empty;

                            if (request.ValidMessage.IsDicomFormat.HasValue)
                            {
                                if (request.ValidMessage.IsDicomFormat.Value)
                                {
                                    messageEventIdCode = request.ValidMessage.AuditMessageDicom.Event.EventId.Code;
                                    messageEventIdCodeSystemName = request.ValidMessage.AuditMessageDicom.Event.EventId.CodeSystemName;
                                    messageEventIdDisplayName = request.ValidMessage.AuditMessageDicom.Event.EventId.DisplayName;

                                    if (request.ValidMessage.AuditMessageDicom.Event.EventTypeCode != null)
                                    {
                                        messageEventTypeCode = request.ValidMessage.AuditMessageDicom.Event.EventTypeCode.Code;
                                        messageEventTypeCodeSystemName = request.ValidMessage.AuditMessageDicom.Event.EventTypeCode.CodeSystemName;
                                        messageEventTypeDisplayName = request.ValidMessage.AuditMessageDicom.Event.EventTypeCode.DisplayName;
                                    }

                                    var source = 
                                        request.ValidMessage.AuditMessageDicom.ActiveParticipants.SingleOrDefault(a => a.SectionType == IHE.AuditTrail.SectionType.Source);

                                    if(source != null)
                                    {
                                        messageAuditSourceCode = source.RoleIdCode.Code;
                                        messageAuditSourceCodeSystemName = source.RoleIdCode.CodeSystemName;
                                        messageAuditSourceDisplayName = source.RoleIdCode.DisplayName;
                                    }
                                }
                                else if (!request.ValidMessage.IsDicomFormat.Value)
                                {
                                    messageEventIdCode = request.ValidMessage.AuditMessage.Event.EventId.Code;
                                    messageEventIdCodeSystemName = request.ValidMessage.AuditMessage.Event.EventId.CodeSystemName;
                                    messageEventIdDisplayName = request.ValidMessage.AuditMessage.Event.EventId.DisplayName;

                                    if (request.ValidMessage.AuditMessage.Event.EventTypeCode != null)
                                    {
                                        messageEventTypeCode = request.ValidMessage.AuditMessage.Event.EventTypeCode.Code;
                                        messageEventTypeCodeSystemName = request.ValidMessage.AuditMessage.Event.EventTypeCode.CodeSystemName;
                                        messageEventTypeDisplayName = request.ValidMessage.AuditMessage.Event.EventTypeCode.DisplayName;
                                    }

                                    var source =
                                        request.ValidMessage.AuditMessage.ActiveParticipants.SingleOrDefault(a => a.SectionType == IHE.AuditTrail.SectionType.Source);

                                    if (source != null)
                                    {
                                        messageAuditSourceCode = source.RoleIdCode.Code;
                                        messageAuditSourceCodeSystemName = source.RoleIdCode.CodeSystemName;
                                        messageAuditSourceDisplayName = source.RoleIdCode.DisplayName;
                                    }
                                }
                            }

                            //Check for Event Id
                            if (notAllowedEventIDCount != 0)
                                isSupported = supportedLogElementTypes.Exists(s => s.AllowLog &&
                                    s.ElementType.Equals("EventID", StringComparison.OrdinalIgnoreCase) &&
                                    s.Code.Equals(messageEventIdCode, StringComparison.OrdinalIgnoreCase) &&
                                    s.SystemName.Equals(messageEventIdCodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                    s.DisplayName.Equals(messageEventIdDisplayName, StringComparison.OrdinalIgnoreCase));

                            //Check for Event Type
                            if (isSupported && notAllowedEventTypeCodeCount != 0)
                                isSupported = supportedLogElementTypes.Exists(s => s.AllowLog &&
                                    s.ElementType.Equals("EventTypeCode", StringComparison.OrdinalIgnoreCase) &&
                                    s.Code.Equals(messageEventTypeCode, StringComparison.OrdinalIgnoreCase) &&
                                    s.SystemName.Equals(messageEventTypeCodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                    s.DisplayName.Equals(messageEventTypeDisplayName, StringComparison.OrdinalIgnoreCase));

                            //Check for Source
                            if(isSupported && notAllowedAuditSourceCount != 0)
                                isSupported = supportedLogElementTypes.Exists(s => s.AllowLog &&
                                    s.ElementType.Equals("SourceParticipant", StringComparison.OrdinalIgnoreCase) &&
                                    s.Code.Equals(messageAuditSourceCode, StringComparison.OrdinalIgnoreCase) &&
                                    s.SystemName.Equals(messageAuditSourceCodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                    s.DisplayName.Equals(messageAuditSourceDisplayName, StringComparison.OrdinalIgnoreCase));
                        }

                        # endregion

                        if (!isSupported)
                        {
                            Helper.LogMessage(string.Format(CultureInfo.InvariantCulture, "Loggging not supported for Audit Log: {0}", request.ValidMessage.Message), Constants.LogCategoryName_Service);
                        }
                        else
                        {
                            dataModel.LogRecorders.Add(logRecorder);

                            # region ValidLog

                            var validLog = new ValidLog();
                            validLog.LogID = logId;
                            validLog.HostName = request.ValidMessage.Header.HostName;
                            validLog.AppName = request.ValidMessage.Header.AppName;
                            validLog.IsDicomFormat = request.ValidMessage.IsDicomFormat.Value;
                            validLog.MessageID = request.ValidMessage.Header.MsgId;
                            validLog.Pri = request.ValidMessage.Header.Pri;
                            validLog.ProcessID = request.ValidMessage.Header.ProcId;
                            validLog.StructuredData = request.ValidMessage.StructuredData;
                            validLog.Timestamp = request.ValidMessage.Header.Timestamp;
                            validLog.Version = request.ValidMessage.Header.Version;
                            validLog.Data = request.ValidMessage.Message;

                            dataModel.ValidLogs.Add(validLog);

                            # endregion

                            # region EventIdentification

                            var eventIdentification = new EventIdentification();
                            var eventId = new ActorElement() { ActorElementTypeID = (int)Perceptive.IHE.AuditTrail.ActorElementType.EventID };
                            var eventTypeCode = new ActorElement() { ActorElementTypeID = (int)Perceptive.IHE.AuditTrail.ActorElementType.EventTypeCode };
                            eventIdentification.LogID = logId;

                            if (request.ValidMessage.IsDicomFormat.Value)
                            {
                                # region Dicom Identification

                                eventIdentification.EventActionCode = request.ValidMessage.AuditMessageDicom.Event.EventActionCode;
                                eventIdentification.EventDateTime = request.ValidMessage.AuditMessageDicom.Event.EventDateTime;
                                eventIdentification.EventOutcomeIndicator = request.ValidMessage.AuditMessageDicom.Event.EventOutcomeIndicator;

                                eventId.Code = request.ValidMessage.AuditMessageDicom.Event.EventId.Code;
                                eventId.CodeSystemName = request.ValidMessage.AuditMessageDicom.Event.EventId.CodeSystemName;
                                eventId.DisplayName = request.ValidMessage.AuditMessageDicom.Event.EventId.DisplayName;

                                var existingEventId = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == eventId.ActorElementTypeID &&
                                    a.Code == eventId.Code && a.CodeSystemName == eventId.CodeSystemName && a.DisplayName == eventId.DisplayName);
                                if (existingEventId == null)
                                {
                                    eventIdentification.EventID = eventId.ActorElementID = Guid.NewGuid();
                                    dataModel.ActorElements.Add(eventId);
                                }
                                else
                                    eventIdentification.EventID = existingEventId.ActorElementID;

                                if (request.ValidMessage.AuditMessageDicom.Event.EventTypeCode != null)
                                {
                                    eventTypeCode.Code = request.ValidMessage.AuditMessageDicom.Event.EventTypeCode.Code;
                                    eventTypeCode.CodeSystemName = request.ValidMessage.AuditMessageDicom.Event.EventTypeCode.CodeSystemName;
                                    eventTypeCode.DisplayName = request.ValidMessage.AuditMessageDicom.Event.EventTypeCode.DisplayName;

                                    var existingEventTypeCode = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == eventTypeCode.ActorElementTypeID &&
                                    a.Code == eventTypeCode.Code && a.CodeSystemName == eventTypeCode.CodeSystemName && a.DisplayName == eventTypeCode.DisplayName);
                                    if (existingEventTypeCode == null)
                                    {
                                        eventIdentification.EventTypeCodeID = eventTypeCode.ActorElementID = Guid.NewGuid();
                                        dataModel.ActorElements.Add(eventTypeCode);
                                    }
                                    else
                                        eventIdentification.EventTypeCodeID = existingEventTypeCode.ActorElementID;
                                }

                                # endregion
                            }
                            else
                            {
                                # region Non Dicom EventIdentification

                                eventIdentification.EventActionCode = request.ValidMessage.AuditMessage.Event.EventActionCode;
                                eventIdentification.EventDateTime = request.ValidMessage.AuditMessage.Event.EventDateTime;
                                eventIdentification.EventOutcomeIndicator = request.ValidMessage.AuditMessage.Event.EventOutcomeIndicator;

                                eventId.Code = request.ValidMessage.AuditMessage.Event.EventId.Code;
                                eventId.CodeSystemName = request.ValidMessage.AuditMessage.Event.EventId.CodeSystemName;
                                eventId.DisplayName = request.ValidMessage.AuditMessage.Event.EventId.DisplayName;

                                var existingEventId = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == eventId.ActorElementTypeID &&
                                    a.Code == eventId.Code && a.CodeSystemName == eventId.CodeSystemName && a.DisplayName == eventId.DisplayName);
                                if (existingEventId == null)
                                {
                                    eventIdentification.EventID = eventId.ActorElementID = Guid.NewGuid();
                                    dataModel.ActorElements.Add(eventId);
                                }
                                else
                                    eventIdentification.EventID = existingEventId.ActorElementID;

                                if (request.ValidMessage.AuditMessage.Event.EventTypeCode != null)
                                {
                                    eventTypeCode.Code = request.ValidMessage.AuditMessage.Event.EventTypeCode.Code;
                                    eventTypeCode.CodeSystemName = request.ValidMessage.AuditMessage.Event.EventTypeCode.CodeSystemName;
                                    eventTypeCode.DisplayName = request.ValidMessage.AuditMessage.Event.EventTypeCode.DisplayName;

                                    var existingEventTypeCode = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == eventTypeCode.ActorElementTypeID &&
                                    a.Code == eventTypeCode.Code && a.CodeSystemName == eventTypeCode.CodeSystemName && a.DisplayName == eventTypeCode.DisplayName);
                                    if (existingEventTypeCode == null)
                                    {
                                        eventIdentification.EventTypeCodeID = eventTypeCode.ActorElementID = Guid.NewGuid();
                                        dataModel.ActorElements.Add(eventTypeCode);
                                    }
                                    else
                                        eventIdentification.EventTypeCodeID = existingEventTypeCode.ActorElementID;
                                }

                                # endregion
                            }

                            dataModel.EventIdentifications.Add(eventIdentification);

                            # endregion

                            # region Active Participant

                            if (request.ValidMessage.IsDicomFormat.Value)
                            {
                                # region Dicom Active Participant

                                foreach (var participant in request.ValidMessage.AuditMessageDicom.ActiveParticipants)
                                {
                                    var activeParticipant = new ActiveParticipant();
                                    activeParticipant.ActiveParticipantID = Guid.NewGuid();
                                    activeParticipant.LogID = logId;
                                    activeParticipant.UserID = participant.UserId;
                                    activeParticipant.UserIsRequestor = participant.UserIsRequestor;
                                    activeParticipant.UserName = participant.UserName;
                                    activeParticipant.AlternativeUserID = participant.AlternativeUserId;
                                    activeParticipant.NetworkAccesPointID = participant.NetworkAccessPointId;
                                    activeParticipant.NetworkAccessPointTypeCode = participant.NetworkAccessPointTypeCode;

                                    if (participant.RoleIdCode != null)
                                    {
                                        var actorElement = new ActorElement() { ActorElementTypeID = (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode };

                                        var existingRoleIdCode = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == actorElement.ActorElementTypeID &&
                                            a.Code.Equals(participant.RoleIdCode.Code, StringComparison.OrdinalIgnoreCase) &&
                                            a.CodeSystemName.Equals(participant.RoleIdCode.CodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                            a.DisplayName.Equals(participant.RoleIdCode.DisplayName, StringComparison.OrdinalIgnoreCase));

                                        if (existingRoleIdCode == null)
                                        {
                                            activeParticipant.RoleIDCode = actorElement.ActorElementID = Guid.NewGuid();
                                            actorElement.Code = participant.RoleIdCode.Code;
                                            actorElement.CodeSystemName = participant.RoleIdCode.CodeSystemName;
                                            actorElement.DisplayName = participant.RoleIdCode.DisplayName;
                                            dataModel.ActorElements.Add(actorElement);
                                        }
                                        else
                                            activeParticipant.RoleIDCode = existingRoleIdCode.ActorElementID;
                                    }

                                    dataModel.ActiveParticipants.Add(activeParticipant);
                                }

                                # endregion
                            }
                            else
                            {
                                # region Non Dicom Active Participant

                                foreach (var participant in request.ValidMessage.AuditMessage.ActiveParticipants)
                                {
                                    var activeParticipant = new ActiveParticipant();
                                    activeParticipant.ActiveParticipantID = Guid.NewGuid();
                                    activeParticipant.LogID = logId;
                                    activeParticipant.UserID = participant.UserId;
                                    activeParticipant.UserIsRequestor = participant.UserIsRequestor;
                                    activeParticipant.UserName = participant.UserName;
                                    activeParticipant.AlternativeUserID = participant.AlternativeUserId;
                                    activeParticipant.NetworkAccesPointID = participant.NetworkAccessPointId;
                                    activeParticipant.NetworkAccessPointTypeCode = participant.NetworkAccessPointTypeCode;

                                    if (participant.RoleIdCode != null)
                                    {
                                        var actorElement = new ActorElement() { ActorElementTypeID = (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode };

                                        var existingRoleIdCode = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == actorElement.ActorElementTypeID &&
                                            a.Code.Equals(participant.RoleIdCode.Code, StringComparison.OrdinalIgnoreCase) &&
                                            a.CodeSystemName.Equals(participant.RoleIdCode.CodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                            a.DisplayName.Equals(participant.RoleIdCode.DisplayName, StringComparison.OrdinalIgnoreCase));

                                        if (existingRoleIdCode == null)
                                        {
                                            activeParticipant.RoleIDCode = actorElement.ActorElementID = Guid.NewGuid();
                                            actorElement.Code = participant.RoleIdCode.Code;
                                            actorElement.CodeSystemName = participant.RoleIdCode.CodeSystemName;
                                            actorElement.DisplayName = participant.RoleIdCode.DisplayName;
                                            dataModel.ActorElements.Add(actorElement);
                                        }
                                        else
                                            activeParticipant.RoleIDCode = existingRoleIdCode.ActorElementID;
                                    }

                                    dataModel.ActiveParticipants.Add(activeParticipant);
                                }

                                # endregion
                            }

                            # endregion

                            # region Audit Source Identification

                            var auditSourceIdentification = new AuditSourceIdentification();
                            auditSourceIdentification.LogID = logId;

                            if (request.ValidMessage.IsDicomFormat.Value)
                            {
                                # region Dicom

                                auditSourceIdentification.AuditSourceID = request.ValidMessage.AuditMessageDicom.AuditSource.AuditSourceId;
                                auditSourceIdentification.AuditEnterpriseSiteID = request.ValidMessage.AuditMessageDicom.AuditSource.AuditEnterpriseSiteId;
                                auditSourceIdentification.Code = request.ValidMessage.AuditMessageDicom.AuditSource.Code;
                                auditSourceIdentification.CodeSystemName = request.ValidMessage.AuditMessageDicom.AuditSource.CodeSystemName;
                                auditSourceIdentification.DisplayName = request.ValidMessage.AuditMessageDicom.AuditSource.OriginalText;

                                # endregion
                            }
                            else
                            {
                                # region Non Dicom

                                auditSourceIdentification.AuditSourceID = request.ValidMessage.AuditMessage.AuditSource.AuditSourceId;
                                auditSourceIdentification.AuditEnterpriseSiteID = request.ValidMessage.AuditMessage.AuditSource.AuditEnterpriseSiteId;
                                if (request.ValidMessage.AuditMessage.AuditSource.AuditSourceTypeCode != null)
                                    auditSourceIdentification.Code = request.ValidMessage.AuditMessage.AuditSource.AuditSourceTypeCode.Code;

                                # endregion
                            }

                            dataModel.AuditSourceIdentifications.Add(auditSourceIdentification);

                            # endregion

                            # region Participant Object Identification

                            if (request.ValidMessage.IsDicomFormat.Value)
                            {
                                # region Dicom

                                foreach (var participant in request.ValidMessage.AuditMessageDicom.ParticipantObjects)
                                {
                                    var participantObjectIdentification = new ParticipantObjectIdentification();
                                    participantObjectIdentification.LogID = logId;
                                    participantObjectIdentification.ParticipantObjectIdentificationID = Guid.NewGuid();
                                    participantObjectIdentification.ParticipantObjectTypeCode = participant.ParticipantObjectTypeCode.ToString();
                                    participantObjectIdentification.ParticipantObjectTypeCodeRole = participant.ParticipantObjectTypeCodeRole;
                                    participantObjectIdentification.ParticipantObjectDataLifeCycle = participant.ParticipantObjectDataLifeCycle;
                                    participantObjectIdentification.ParticipantObjectSensitivity = participant.ParticipantObjectSensitivity;
                                    participantObjectIdentification.ParticipantObjectID = participant.ParticipantObjectId;
                                    participantObjectIdentification.ParticipantObjectName = participant.ParticipantObjectName;
                                    participantObjectIdentification.ParticipantObjectQuery = participant.ParticipantObjectQuery;

                                    var participantObjectIdTypeCode = new ActorElement() { ActorElementTypeID = (int)Perceptive.IHE.AuditTrail.ActorElementType.ParticipantObjectIdTypeCode };
                                    var existingparticipantObjectIdTypeCode = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == participantObjectIdTypeCode.ActorElementTypeID &&
                                        a.Code.Equals(participant.ParticipantObjectIdTypeCode.Code, StringComparison.OrdinalIgnoreCase) &&
                                        a.CodeSystemName.Equals(participant.ParticipantObjectIdTypeCode.CodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                        a.DisplayName.Equals(participant.ParticipantObjectIdTypeCode.DisplayName, StringComparison.OrdinalIgnoreCase));

                                    if (existingparticipantObjectIdTypeCode == null)
                                    {
                                        participantObjectIdentification.ParticipantObjectIDTypeCode = participantObjectIdTypeCode.ActorElementID = Guid.NewGuid();
                                        participantObjectIdTypeCode.Code = participant.ParticipantObjectIdTypeCode.Code;
                                        participantObjectIdTypeCode.CodeSystemName = participant.ParticipantObjectIdTypeCode.CodeSystemName;
                                        participantObjectIdTypeCode.DisplayName = participant.ParticipantObjectIdTypeCode.DisplayName;

                                        dataModel.ActorElements.Add(participantObjectIdTypeCode);
                                    }
                                    else
                                        participantObjectIdentification.ParticipantObjectIDTypeCode = existingparticipantObjectIdTypeCode.ActorElementID;

                                    foreach (var obj in participant.ParticipantObjectDetail)
                                    {
                                        var objectDetailElement = new ObjectDetailElement();
                                        objectDetailElement.ObjectDetailElementID = Guid.NewGuid();
                                        objectDetailElement.ParticipantObjectIdentificationID = participantObjectIdentification.ParticipantObjectIdentificationID;
                                        objectDetailElement.Type = obj.Type;
                                        objectDetailElement.Value = obj.Value;

                                        dataModel.ObjectDetailElements.Add(objectDetailElement);
                                    }

                                    foreach (var objDesc in participant.ParticipantObjectDescription)
                                    {
                                        var participantObjectDescription = new ParticipantObjectDescription();
                                        participantObjectDescription.ParticipantObjectDescriptionID = Guid.NewGuid();
                                        participantObjectDescription.ParticipantObjectIdentificationID = participantObjectIdentification.ParticipantObjectIdentificationID;
                                        participantObjectDescription.Encrypted = objDesc.Encrypted;

                                        foreach (var mpps in objDesc.MPPS)
                                        {
                                            var objectIdentifier = new ObjectIdentifier() { ObjectIdentifierType = (int)Perceptive.IHE.AuditTrail.ObjectIdentifierType.MPPS };
                                            objectIdentifier.ObjectIdentifierID = Guid.NewGuid();
                                            objectIdentifier.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;
                                            objectIdentifier.UID = mpps.UId;

                                            dataModel.ObjectIdentifiers.Add(objectIdentifier);
                                        }

                                        foreach (var acc in objDesc.Accession)
                                        {
                                            var accession = new Accession();
                                            accession.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;
                                            accession.AccessionID = Guid.NewGuid();
                                            accession.Number = acc.Number;

                                            dataModel.Accessions.Add(accession);
                                        }

                                        foreach (var sop in objDesc.SOPClass)
                                        {
                                            var sopClass = new SOPClass();
                                            sopClass.SOPClassID = Guid.NewGuid();
                                            sopClass.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;
                                            sopClass.UID = sop.UId;
                                            sopClass.NumberOfInstances = sop.NumberOfInstances;

                                            foreach (var ins in sop.Instance)
                                            {
                                                var instance = new Instance();
                                                instance.InstanceID = Guid.NewGuid();
                                                instance.SOPCLassID = sopClass.SOPClassID;
                                                instance.FileName = ins.FileName;
                                                instance.UID = ins.UId;

                                                dataModel.Instances.Add(instance);
                                            }

                                            dataModel.SOPClasses.Add(sopClass);
                                        }

                                        foreach (var study in objDesc.ParticipantObjectContainsStudy)
                                        {
                                            var participantObjectContainsStudy = new ParticipantObjectContainsStudy();
                                            participantObjectContainsStudy.ParticipantObjectContainsStudyID = Guid.NewGuid();
                                            participantObjectContainsStudy.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;

                                            foreach (var sid in study.StudyIds)
                                            {
                                                var objectIdentifier = new ObjectIdentifier() { ObjectIdentifierType = (int)Perceptive.IHE.AuditTrail.ObjectIdentifierType.StudyIDs };
                                                objectIdentifier.ObjectIdentifierID = Guid.NewGuid();
                                                objectIdentifier.ParticipantObjectContainsStudyID = participantObjectContainsStudy.ParticipantObjectContainsStudyID;
                                                objectIdentifier.UID = sid.UId;

                                                dataModel.ObjectIdentifiers.Add(objectIdentifier);
                                            }

                                            dataModel.ParticipantObjectContainsStudies.Add(participantObjectContainsStudy);
                                        }

                                        participantObjectDescription.Anonymized = objDesc.Anonymized;
                                    }

                                    dataModel.ParticipantObjectIdentifications.Add(participantObjectIdentification);
                                }

                                # endregion
                            }
                            else
                            {
                                # region Non Dicom

                                foreach (var participant in request.ValidMessage.AuditMessage.ParticipantObjects)
                                {
                                    var participantObjectIdentification = new ParticipantObjectIdentification();
                                    participantObjectIdentification.LogID = logId;
                                    participantObjectIdentification.ParticipantObjectIdentificationID = Guid.NewGuid();
                                    participantObjectIdentification.ParticipantObjectTypeCode = participant.ParticipantObjectTypeCode.ToString();
                                    participantObjectIdentification.ParticipantObjectTypeCodeRole = participant.ParticipantObjectTypeCodeRole;
                                    participantObjectIdentification.ParticipantObjectDataLifeCycle = participant.ParticipantObjectDataLifeCycle;
                                    participantObjectIdentification.ParticipantObjectSensitivity = participant.ParticipantObjectSensitivity;
                                    participantObjectIdentification.ParticipantObjectID = participant.ParticipantObjectId;
                                    participantObjectIdentification.ParticipantObjectName = participant.ParticipantObjectName;
                                    participantObjectIdentification.ParticipantObjectQuery = participant.ParticipantObjectQuery;

                                    var participantObjectIdTypeCode = new ActorElement() { ActorElementTypeID = (int)Perceptive.IHE.AuditTrail.ActorElementType.ParticipantObjectIdTypeCode };
                                    var existingparticipantObjectIdTypeCode = dataModel.ActorElements.FirstOrDefault(a => a.ActorElementTypeID == participantObjectIdTypeCode.ActorElementTypeID &&
                                        a.Code.Equals(participant.ParticipantObjectIdTypeCode.Code, StringComparison.OrdinalIgnoreCase) &&
                                        a.CodeSystemName.Equals(participant.ParticipantObjectIdTypeCode.CodeSystemName, StringComparison.OrdinalIgnoreCase) &&
                                        a.DisplayName.Equals(participant.ParticipantObjectIdTypeCode.DisplayName, StringComparison.OrdinalIgnoreCase));

                                    if (existingparticipantObjectIdTypeCode == null)
                                    {
                                        participantObjectIdentification.ParticipantObjectIDTypeCode = participantObjectIdTypeCode.ActorElementID = Guid.NewGuid();
                                        participantObjectIdTypeCode.Code = participant.ParticipantObjectIdTypeCode.Code;
                                        participantObjectIdTypeCode.CodeSystemName = participant.ParticipantObjectIdTypeCode.CodeSystemName;
                                        participantObjectIdTypeCode.DisplayName = participant.ParticipantObjectIdTypeCode.DisplayName;

                                        dataModel.ActorElements.Add(participantObjectIdTypeCode);
                                    }
                                    else
                                        participantObjectIdentification.ParticipantObjectIDTypeCode = existingparticipantObjectIdTypeCode.ActorElementID;

                                    foreach (var obj in participant.ParticipantObjectDetail)
                                    {
                                        var objectDetailElement = new ObjectDetailElement();
                                        objectDetailElement.ObjectDetailElementID = Guid.NewGuid();
                                        objectDetailElement.ParticipantObjectIdentificationID = participantObjectIdentification.ParticipantObjectIdentificationID;
                                        objectDetailElement.Type = obj.Type;
                                        objectDetailElement.Value = obj.Value;

                                        dataModel.ObjectDetailElements.Add(objectDetailElement);
                                    }

                                    foreach (var objDesc in participant.ParticipantObjectDescription)
                                    {
                                        var participantObjectDescription = new ParticipantObjectDescription();
                                        participantObjectDescription.ParticipantObjectDescriptionID = Guid.NewGuid();
                                        participantObjectDescription.ParticipantObjectIdentificationID = participantObjectIdentification.ParticipantObjectIdentificationID;
                                        participantObjectDescription.Encrypted = objDesc.Encrypted;

                                        foreach (var mpps in objDesc.MPPS)
                                        {
                                            var objectIdentifier = new ObjectIdentifier() { ObjectIdentifierType = (int)Perceptive.IHE.AuditTrail.ObjectIdentifierType.MPPS };
                                            objectIdentifier.ObjectIdentifierID = Guid.NewGuid();
                                            objectIdentifier.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;
                                            objectIdentifier.UID = mpps.UId;

                                            dataModel.ObjectIdentifiers.Add(objectIdentifier);
                                        }

                                        foreach (var acc in objDesc.Accession)
                                        {
                                            var accession = new Accession();
                                            accession.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;
                                            accession.AccessionID = Guid.NewGuid();
                                            accession.Number = acc.Number;

                                            dataModel.Accessions.Add(accession);
                                        }

                                        foreach (var sop in objDesc.SOPClass)
                                        {
                                            var sopClass = new SOPClass();
                                            sopClass.SOPClassID = Guid.NewGuid();
                                            sopClass.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;
                                            sopClass.UID = sop.UId;
                                            sopClass.NumberOfInstances = sop.NumberOfInstances;

                                            foreach (var ins in sop.Instance)
                                            {
                                                var instance = new Instance();
                                                instance.InstanceID = Guid.NewGuid();
                                                instance.SOPCLassID = sopClass.SOPClassID;
                                                instance.FileName = ins.FileName;
                                                instance.UID = ins.UId;

                                                dataModel.Instances.Add(instance);
                                            }

                                            dataModel.SOPClasses.Add(sopClass);
                                        }

                                        foreach (var study in objDesc.ParticipantObjectContainsStudy)
                                        {
                                            var participantObjectContainsStudy = new ParticipantObjectContainsStudy();
                                            participantObjectContainsStudy.ParticipantObjectContainsStudyID = Guid.NewGuid();
                                            participantObjectContainsStudy.ParticipantObjectDescriptionID = participantObjectDescription.ParticipantObjectDescriptionID;

                                            foreach (var sid in study.StudyIds)
                                            {
                                                var objectIdentifier = new ObjectIdentifier() { ObjectIdentifierType = (int)Perceptive.IHE.AuditTrail.ObjectIdentifierType.StudyIDs };
                                                objectIdentifier.ObjectIdentifierID = Guid.NewGuid();
                                                objectIdentifier.ParticipantObjectContainsStudyID = participantObjectContainsStudy.ParticipantObjectContainsStudyID;
                                                objectIdentifier.UID = sid.UId;

                                                dataModel.ObjectIdentifiers.Add(objectIdentifier);
                                            }

                                            dataModel.ParticipantObjectContainsStudies.Add(participantObjectContainsStudy);
                                        }

                                        participantObjectDescription.Anonymized = objDesc.Anonymized;
                                        dataModel.ParticipantObjectDescriptions.Add(participantObjectDescription);
                                    }

                                    dataModel.ParticipantObjectIdentifications.Add(participantObjectIdentification);
                                }

                                # endregion
                            }

                            # endregion
                        }
                    }

                    dataModel.SaveChanges();
                    success = true;
                }
                catch (Exception ex)
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                }
            }            

            return success;
        }
    }
}
