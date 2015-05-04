using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptive.IHE.AuditTrail
{
    public class PIXPDQConsumerAuditData
    {
        public string SourceUserId { get; set; }
        public string DestinationUserId { get; set; }
        public string QueryMessage { get; set; }
        public string MessageControlId { get; set; }
        public string DestinationIP { get; set; }
        public List<string> PatientIds { get; set; }
        public MessageType Actor { get; set; }
        public EventOutcomeIndicator EventOutcomeIndicator { get; set; }
    }

    public class PIXSourceData
    {
        public string EventActionCode { get; set; }
        public string SourceUserId { get; set; }
        public string DestinationUserId { get; set; }
        public string DestinationIP { get; set; }
        public string PatientId { get; set; }
        public string MessageControlId { get; set; }
        public EventOutcomeIndicator EventOutcomeIndicator { get; set; }
        public MessageType Actor { get; set; }
    }

    public class PIXConsumerUpdateNotificationAuditData
    {
        public MessageType Actor { get; set; }
        public string SourceUserId { get; set; }
        public string SourceIP { get; set; }
        public string DestinationUserId { get; set; }
        public string MessageControlId { get; set; }
        public List<string> PatientIds { get; set; }
        public EventOutcomeIndicator EventOutcomeIndicator { get; set; }
    }

    public class DocSourceProvideAndRegisterAuditData
    {
        public EventOutcomeIndicator EventOutcomeIndicator { get; set; }
        public string DestinationUserId { get; set; }
        public string DestinationIP { get; set; }
        public string PatientId { get; set; }
        public string SubmissionSetUniqueId { get; set; }
    }

    public class DocumentConsumerRegisterStoredQueryAuditData
    {
        public MessageType Actor { get { return MessageType.DocConsumerRegistryStoredQuery; } }
        public EventOutcomeIndicator EventOutcomeIndicator { get; set; }
        public string SourceUserId { get; set; }
        public string DestinationUserId { get; set; }
        public string DestinationIP { get; set; }
        public string PatientId { get; set; }
        public string StoredQueryId { get; set; }
        public string AdhocQueryRequest { get; set; }
    }
}
