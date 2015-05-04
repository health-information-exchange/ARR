using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptive.IHE.AuditTrail
{
    public static partial class AuditTrail
    {
        //public static void ProcessApplicationActivityEvent(EventOutcomeIndicator eventOutcomeIndicator, bool isApplicationStart)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(MessageType.ApplicationActivity);

        //        //Event
        //        message.Event.EventOutcomeIndicator = ((int)eventOutcomeIndicator).ToString(); ;
        //        message.Event.EventTypeCode.Code = (isApplicationStart) ? "110120" : "110121";
        //        message.Event.EventTypeCode.DisplayName = (isApplicationStart) ? "Application Start" : "Application Stop";

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId =
        //            string.Concat(Environment.MachineName, "@", CompanyName);

        //        SendDataForAuditTrail(message, GetAppName(MessageType.ApplicationActivity));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessPIXPDQConsumerAuditData(PIXPDQConsumerAuditData auditData)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(auditData.Actor, (auditData.PatientIds == null) ? 0 : auditData.PatientIds.Count);

        //       //Event
        //        message.Event.EventOutcomeIndicator = ((int)auditData.EventOutcomeIndicator).ToString();

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId = auditData.SourceUserId;

        //        //Destination
        //        ActiveParticipant destination = message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Destination);

        //        if (destination != null)
        //        {
        //            destination.UserId = auditData.DestinationUserId;
        //            destination.NetworkAccessPointId = auditData.DestinationIP;
        //        }

        //        //Patient
        //        if (auditData.PatientIds != null && auditData.PatientIds.Count > 0)
        //        {
        //            List<ParticipantObjectIdentification> patients = message.ParticipantObjects.Where(p => p.SectionType == SectionType.Patient).ToList();
        //            for (int i = 0; i < patients.Count; i++)
        //            {
        //                patients[i].ParticipantObjectId = auditData.PatientIds[i];
        //            }
        //        }

        //        //Query
        //        ParticipantObjectIdentification query = message.ParticipantObjects.FirstOrDefault(p => p.SectionType == SectionType.Query);

        //        if (query != null)
        //        {
        //            query.ParticipantObjectQuery = auditData.QueryMessage.ToBase64String();
        //            if (auditData.Actor == MessageType.PDQConsumerV2 || auditData.Actor == MessageType.PIXConsumerV2)
        //                query.ParticipantObjectDetail.FirstOrDefault(o => o.DetailType == ObjectDetailType.MSH10).Value = auditData.MessageControlId.ToBase64String();
        //        }

        //        SendDataForAuditTrail(message, GetAppName(auditData.Actor));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessRetrieveDocumentSetImportEvent(List<string> documentValues, string endPoint, EventOutcomeIndicator eventOutcomeIndicator)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(MessageType.DocConsumerRetrieveDocumentSetImport, documentCount: documentValues.Count);

        //        //Event
        //        message.Event.EventOutcomeIndicator = ((int)eventOutcomeIndicator).ToString();

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId = endPoint;

        //        //Destination
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Destination).UserId = HostName;

        //        //Patient not known

        //        //Document
        //        List<ParticipantObjectIdentification> documents = message.ParticipantObjects.Where( p => p.SectionType == SectionType.Document).ToList();
        //        for (int i = 0; i < documentValues.Count; i++)
        //        {
        //            string[] docValues = documentValues[i].Split("|".ToCharArray());

        //            // It is expected that the original string will be of the form 
        //            // documentUniqueId|homeCommunityID|repositoryUniqueId

        //            documents[i].ParticipantObjectId = docValues[0];
        //            documents[i].ParticipantObjectDetail.FirstOrDefault(o => o.DetailType == ObjectDetailType.HomeCommunityId).Value = docValues[1].ToBase64String();
        //            documents[i].ParticipantObjectDetail.FirstOrDefault(o => o.DetailType == ObjectDetailType.RepositoryUniqueId).Value = docValues[2].ToBase64String();
        //        }

        //        SendDataForAuditTrail(message, GetAppName(MessageType.DocConsumerRetrieveDocumentSetImport));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessPIXSourceAuditData(PIXSourceData pixSourceAuditData)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(pixSourceAuditData.Actor, 1);

        //        //Event
        //        message.Event.EventActionCode = pixSourceAuditData.EventActionCode;
        //        message.Event.EventOutcomeIndicator = ((int)pixSourceAuditData.EventOutcomeIndicator).ToString();

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId = pixSourceAuditData.SourceUserId;

        //        //Destination

        //        ActiveParticipant destination = message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Destination);
        //        if (destination != null)
        //        {
        //            destination.UserId = destination.AlternativeUserId = pixSourceAuditData.DestinationUserId;
        //            destination.NetworkAccessPointId = pixSourceAuditData.DestinationIP;
        //        }

        //        //Patient
        //        ParticipantObjectIdentification patient = message.ParticipantObjects.FirstOrDefault(p => p.SectionType == SectionType.Patient);

        //        if (patient != null)
        //        {
        //            ObjectDetailType detailType = (pixSourceAuditData.Actor == MessageType.PIXSourceV2) ? ObjectDetailType.MSH10 : ObjectDetailType.II;
        //            patient.ParticipantObjectId = pixSourceAuditData.PatientId;
        //            patient.ParticipantObjectDetail.FirstOrDefault(o => o.DetailType == detailType).Value = pixSourceAuditData.MessageControlId.ToBase64String();
        //        }

        //        SendDataForAuditTrail(message, GetAppName(pixSourceAuditData.Actor));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessPIXConsumerUpdateNotificationAuditData(PIXConsumerUpdateNotificationAuditData auditData)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(auditData.Actor, (auditData.PatientIds == null) ? 0 : auditData.PatientIds.Count);

        //        //Event
        //        message.Event.EventOutcomeIndicator = ((int)auditData.EventOutcomeIndicator).ToString();

        //        //Source
        //        ActiveParticipant source = message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source);
        //        if (source != null)
        //        {
        //            source.UserId = auditData.SourceUserId;
        //            source.NetworkAccessPointId = auditData.SourceIP;
        //        }

        //        //Destination
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Destination).UserId = auditData.DestinationUserId;

        //        //Patient
        //        if (auditData.PatientIds != null && auditData.PatientIds.Count > 0)
        //        {
        //            List<ParticipantObjectIdentification> patients = message.ParticipantObjects.Where(p => p.SectionType == SectionType.Patient).ToList();
        //            for (int i = 0; i < patients.Count; i++)
        //            {
        //                patients[i].ParticipantObjectId = auditData.PatientIds[i];
        //                patients[i].ParticipantObjectDetail.FirstOrDefault(o => o.DetailType == ObjectDetailType.MSH10).Value = auditData.MessageControlId;
        //            }
        //        }

        //        SendDataForAuditTrail(message, GetAppName(auditData.Actor));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessProvideAndRegisterPHIExportEvent(DocSourceProvideAndRegisterAuditData auditData)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(MessageType.DocSourcePHIExport, patientCount: 1);

        //        //Event
        //        message.Event.EventOutcomeIndicator = ((int)auditData.EventOutcomeIndicator).ToString();

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId = HostName;

        //        //Destination
        //        ActiveParticipant destination = message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Destination);
        //        if (destination != null)
        //        {
        //            destination.UserId = destination.AlternativeUserId = auditData.DestinationUserId;
        //            destination.NetworkAccessPointId = auditData.DestinationIP;
        //        }

        //        //Patient
        //        message.ParticipantObjects.FirstOrDefault(p => p.SectionType == SectionType.Patient).ParticipantObjectId = auditData.PatientId;

        //        //Submission Set
        //        message.ParticipantObjects.FirstOrDefault(p => p.SectionType == SectionType.SubmissionSet).ParticipantObjectId = auditData.SubmissionSetUniqueId;

        //        //dataDictionary.Add("$SubmissionSet.ClassificationNode.UUID$", IHEDataModel.XDSDataModel.XdsUUIDS.XDSSubmissionSet.SubmissionSet);

        //        SendDataForAuditTrail(message, GetAppName(MessageType.DocSourcePHIExport));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessNodeAuthenticationFailure(EventOutcomeIndicator eventOutcomeIndicator, MessageType actor)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(MessageType.NodeAuthenticationFailure);

        //        //Event
        //        message.Event.EventOutcomeIndicator = ((int)eventOutcomeIndicator).ToString();

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId = GetAppName(actor);

        //        //Destination and Participating Objects have been specifically kept out
        //        //to avoid complication

        //        SendDataForAuditTrail(message, GetAppName(MessageType.NodeAuthenticationFailure));
        //    }
        //    catch(Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}

        //public static void ProcessRegistryStoredQueryEvent(DocumentConsumerRegisterStoredQueryAuditData auditData)
        //{
        //    try
        //    {
        //        AuditMessage message = GetAuditMessage(auditData.Actor, patientCount: (string.IsNullOrEmpty(auditData.PatientId) ? 0 : 1));

        //        //Event
        //        message.Event.EventOutcomeIndicator = ((int)auditData.EventOutcomeIndicator).ToString();

        //        //Source
        //        message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Source).UserId = auditData.SourceUserId;

        //        //Destination
        //        ActiveParticipant destination = message.ActiveParticipants.FirstOrDefault(a => a.SectionType == SectionType.Destination);

        //        if (destination != null)
        //        {
        //            destination.UserId = auditData.DestinationUserId;
        //            destination.NetworkAccessPointId = auditData.DestinationIP;
        //        }

        //        //Patient
        //        if (!string.IsNullOrEmpty(auditData.PatientId))
        //            message.ParticipantObjects.FirstOrDefault(p => p.SectionType == SectionType.Patient).ParticipantObjectId = auditData.PatientId;

        //        //Query
        //        ParticipantObjectIdentification query = message.ParticipantObjects.FirstOrDefault(p => p.SectionType == SectionType.Query);

        //        if (query != null)
        //        {
        //            query.ParticipantObjectId = auditData.StoredQueryId;
        //            query.ParticipantObjectQuery = auditData.AdhocQueryRequest.ToBase64String();
        //        }

        //        SendDataForAuditTrail(message, GetAppName(auditData.Actor));                
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.WriteToLog(ex, LogCategory.General);
        //    }
        //}
    }
}
