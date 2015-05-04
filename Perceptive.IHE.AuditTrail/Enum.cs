using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptive.IHE.AuditTrail
{
    public enum MessageType
    {
        None,
        ApplicationActivity,
        DocConsumerRegistryStoredQuery,
        DocConsumerRetrieveDocumentSetImport,
        DocSourcePHIExport,
        NodeAuthenticationFailure,
        PIXConsumerV2,
        PIXConsumerV3,
        PIXSourceV2,
        PIXSourceV3,
        PIXConsumerUpdateNotificationV2,
        PIXConsumerUpdateNotificationV3,
        PDQConsumerV2,
        PDQConsumerV3,
        XCPDIGQuery,
        XCPDRGQuery
    }

    public enum ActorElementType
    {        
        EventID,
        EventTypeCode,
        ParticipantObjectIdTypeCode,
        RoleIDCode
    }

    public enum EventOutcomeIndicator
    {
        Success = 0,
        MinorFailure = 4,
        SeriousFailure = 8,
        MajorFailure = 12
    }

    public enum SectionType
    {
        None,
        Source,
        HumanRequestor,
        Destination,
        Patient,
        Query,
        Document,
        SubmissionSet
    }

    public enum ObjectDetailType
    {
        MSH10,
        RepositoryUniqueId,
        HomeCommunityId,
        II
    }

    public enum ObjectIdentifierType
    {
        MPPS,
        StudyIDs
    }
}
