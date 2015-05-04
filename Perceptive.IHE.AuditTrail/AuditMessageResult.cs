using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Perceptive.IHE.AuditTrail
{
    [DataContract]
    public class AuditMessageResult
    {
        [DataMember]
        public EventIdentificationResult Event { get; set; }

        [DataMember]
        public List<ActiveParticipantResult> ActiveParticipants { get; set; }

        [DataMember]
        public AuditSourceIdentificationResult AuditSource { get; set; }

        [DataMember]
        public List<ParticipantObjectIdentificationResult> ParticipantObjects { get; set; }

        public AuditMessageResult(AuditMessageDicom message)
        {
            if(message.Event != null)
                Event = new EventIdentificationResult(message.Event);

            if (message.ActiveParticipants != null)
            {
                ActiveParticipants = new List<ActiveParticipantResult>();
                foreach (var active in message.ActiveParticipants)
                    ActiveParticipants.Add(new ActiveParticipantResult(active));
            }

            if (message.AuditSource != null)
                AuditSource = new AuditSourceIdentificationResult(message.AuditSource);

            if (message.ParticipantObjects != null)
            {
                ParticipantObjects = new List<ParticipantObjectIdentificationResult>();
                foreach (var obj in message.ParticipantObjects)
                    ParticipantObjects.Add(new ParticipantObjectIdentificationResult(obj));
            }
        }

        public AuditMessageResult(AuditMessage message)
        {
            if (message.Event != null) 
                Event = new EventIdentificationResult(message.Event);

            if (message.ActiveParticipants != null)
            {
                ActiveParticipants = new List<ActiveParticipantResult>();
                foreach (var active in message.ActiveParticipants)
                    ActiveParticipants.Add(new ActiveParticipantResult(active));
            }

            if (message.AuditSource != null) 
                AuditSource = new AuditSourceIdentificationResult(message.AuditSource);

            if (message.ParticipantObjects != null)
            {
                ParticipantObjects = new List<ParticipantObjectIdentificationResult>();
                foreach (var obj in message.ParticipantObjects)
                    ParticipantObjects.Add(new ParticipantObjectIdentificationResult(obj));
            }
        }
    }

    [DataContract]
    public class EventIdentificationResult
    {
        [DataMember]
        public ActorElementResult EventId { get; set; }

        [DataMember]
        public string EventActionCode { get; set; }

        [DataMember]
        public string EventDateTime { get; set; }

        [DataMember]
        public string EventOutcomeIndicator { get; set; }

        [DataMember]
        public ActorElementResult EventTypeCode { get; set; }

        public EventIdentificationResult(EventIdentificationDicom eventIdentification)
        {
            EventId = new ActorElementResult(eventIdentification.EventId);
            EventActionCode = eventIdentification.EventActionCode;
            EventDateTime = eventIdentification.EventDateTime;
            EventOutcomeIndicator = eventIdentification.EventOutcomeIndicator;
            if(eventIdentification.EventTypeCode != null)
                EventTypeCode = new ActorElementResult(eventIdentification.EventTypeCode);
        }

        public EventIdentificationResult(EventIdentification eventIdentification)
        {
            EventId = new ActorElementResult(eventIdentification.EventId);
            EventActionCode = eventIdentification.EventActionCode;
            EventDateTime = eventIdentification.EventDateTime;
            EventOutcomeIndicator = eventIdentification.EventOutcomeIndicator;
            if (eventIdentification.EventTypeCode != null)
                EventTypeCode = new ActorElementResult(eventIdentification.EventTypeCode);
        }
    }

    [DataContract]
    public class ActorElementResult
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string CodeSystemName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        public ActorElementResult(ActorElementDicom actor)
        {
            Code = actor.Code;
            CodeSystemName = actor.CodeSystemName;
            DisplayName = actor.DisplayName;
        }

        public ActorElementResult(ActorElement actor)
        {
            Code = actor.Code;
            CodeSystemName = actor.CodeSystemName;
            DisplayName = actor.DisplayName;
        }
    }

    [DataContract]
    public class ActiveParticipantResult
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string AlternativeUserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool UserIsRequestor { get; set; }

        [DataMember]
        public ActorElementResult RoleIdCode { get; set; }

        [DataMember]
        public string NetworkAccessPointTypeCode { get; set; }

        [DataMember]
        public string NetworkAccessPointId { get; set; }

        public ActiveParticipantResult(ActiveParticipantDicom active)
        {
            UserId = active.UserId;
            AlternativeUserId = active.AlternativeUserId;
            UserName = active.UserName;
            UserIsRequestor = active.UserIsRequestor;
            if(active.RoleIdCode != null)
                RoleIdCode = new ActorElementResult(active.RoleIdCode);
            NetworkAccessPointTypeCode = active.NetworkAccessPointTypeCode;
            NetworkAccessPointId = active.NetworkAccessPointId;
        }

        public ActiveParticipantResult(ActiveParticipant active)
        {
            UserId = active.UserId;
            AlternativeUserId = active.AlternativeUserId;
            UserName = active.UserName;
            UserIsRequestor = active.UserIsRequestor;
            if (active.RoleIdCode != null)
                RoleIdCode = new ActorElementResult(active.RoleIdCode);
            NetworkAccessPointTypeCode = active.NetworkAccessPointTypeCode;
            NetworkAccessPointId = active.NetworkAccessPointId;
        }
    }

    [DataContract]
    public class AuditSourceIdentificationResult
    {
        [DataMember]
        public string AuditSourceId { get; set; }

        [DataMember]
        public string AuditEnterpriseSiteId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string CodeSystemName { get; set; }

        [DataMember]
        public string OriginalText { get; set; }

        public AuditSourceIdentificationResult(AuditSourceIdentification auditSource)
        {
            AuditSourceId = auditSource.AuditSourceId;
            AuditEnterpriseSiteId = auditSource.AuditEnterpriseSiteId;
            if(auditSource.AuditSourceTypeCode != null)
                Code = auditSource.AuditSourceTypeCode.Code;
        }

        public AuditSourceIdentificationResult(AuditSourceIdentificationDicom auditSource)
        {
            AuditSourceId = auditSource.AuditSourceId;
            AuditEnterpriseSiteId = auditSource.AuditEnterpriseSiteId;
            Code = auditSource.Code;
            CodeSystemName = auditSource.CodeSystemName;
            OriginalText = auditSource.OriginalText;
        }
    }

    [DataContract]
    public class ParticipantObjectIdentificationResult
    {
        [DataMember]
        public int ParticipantObjectTypeCode { get; set; }

        [DataMember]
        public string ParticipantObjectTypeCodeRole { get; set; }

        [DataMember]
        public string ParticipantObjectDataLifeCycle { get; set; }

        [DataMember]
        public ActorElementResult ParticipantObjectIdTypeCode { get; set; }

        [DataMember]
        public string ParticipantObjectSensitivity { get; set; }

        [DataMember]
        public string ParticipantObjectId { get; set; }

        [DataMember]
        public string ParticipantObjectName { get; set; }

        [DataMember]
        public string ParticipantObjectQuery { get; set; }

        [DataMember]
        public List<ObjectDetailElementResult> ParticipantObjectDetail { get; set; }

        # region Dicom Optional Fields

        [DataMember]
        public List<ParticipantObjectDescriptionResult> ParticipantObjectDescription { get; set; }

        # endregion

        public ParticipantObjectIdentificationResult(ParticipantObjectIdentification obj)
        {
            ParticipantObjectTypeCode = obj.ParticipantObjectTypeCode;
            ParticipantObjectTypeCodeRole = obj.ParticipantObjectTypeCodeRole;
            ParticipantObjectDataLifeCycle = obj.ParticipantObjectDataLifeCycle;
            if (obj.ParticipantObjectIdTypeCode != null)
                ParticipantObjectIdTypeCode = new ActorElementResult(obj.ParticipantObjectIdTypeCode);
            ParticipantObjectSensitivity = obj.ParticipantObjectSensitivity;
            ParticipantObjectId = obj.ParticipantObjectId;
            ParticipantObjectName = obj.ParticipantObjectName;
            ParticipantObjectQuery = obj.ParticipantObjectQuery;

            if(obj.ParticipantObjectDetail != null)
            {
                ParticipantObjectDetail = new List<ObjectDetailElementResult>();
                foreach (var detail in obj.ParticipantObjectDetail)
                    ParticipantObjectDetail.Add(new ObjectDetailElementResult(detail));
            }

            if (obj.ParticipantObjectDescription != null)
            {
                ParticipantObjectDescription = new List<ParticipantObjectDescriptionResult>();
                foreach (var desc in obj.ParticipantObjectDescription)
                    ParticipantObjectDescription.Add(new ParticipantObjectDescriptionResult(desc));
            }
        }

        public ParticipantObjectIdentificationResult(ParticipantObjectIdentificationDicom obj)
        {
            ParticipantObjectTypeCode = obj.ParticipantObjectTypeCode;
            ParticipantObjectTypeCodeRole = obj.ParticipantObjectTypeCodeRole;
            ParticipantObjectDataLifeCycle = obj.ParticipantObjectDataLifeCycle;
            if (obj.ParticipantObjectIdTypeCode != null)
                ParticipantObjectIdTypeCode = new ActorElementResult(obj.ParticipantObjectIdTypeCode);
            ParticipantObjectSensitivity = obj.ParticipantObjectSensitivity;
            ParticipantObjectId = obj.ParticipantObjectId;
            ParticipantObjectName = obj.ParticipantObjectName;
            ParticipantObjectQuery = obj.ParticipantObjectQuery;

            if (obj.ParticipantObjectDetail != null)
            {
                ParticipantObjectDetail = new List<ObjectDetailElementResult>();
                foreach (var detail in obj.ParticipantObjectDetail)
                    ParticipantObjectDetail.Add(new ObjectDetailElementResult(detail));
            }

            if (obj.ParticipantObjectDescription != null)
            {
                ParticipantObjectDescription = new List<ParticipantObjectDescriptionResult>();
                foreach (var desc in obj.ParticipantObjectDescription)
                    ParticipantObjectDescription.Add(new ParticipantObjectDescriptionResult(desc));
            }
        }
    }

    [DataContract]
    public class ObjectDetailElementResult
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Value { get; set; }        

        public ObjectDetailElementResult(ObjectDetailElement detail)
        {
            Type = detail.Type;
            Value = detail.Value;
        }
    }

    [DataContract]
    public class ParticipantObjectDescriptionResult
    {
        [DataMember]
        public List<ObjectIdentifierResult> MPPS { get; set; }

        [DataMember]
        public List<AccessionResult> Accession { get; set; }

        [DataMember]
        public List<SOPClassResult> SOPClass { get; set; }

        [DataMember]
        public List<ParticipantObjectContainsStudyResult> ParticipantObjectContainsStudy { get; set; }

        [DataMember]
        public string Encrypted { get; set; }

        [DataMember]
        public string Anonymized { get; set; }

        public ParticipantObjectDescriptionResult(ParticipantObjectDescription desc)
        {
            Encrypted = desc.EncryptedAsText;
            Anonymized = desc.AnonymizedAsText;

            if(desc.MPPS != null)
            {
                MPPS = new List<ObjectIdentifierResult>();
                foreach (var x in desc.MPPS)
                    MPPS.Add(new ObjectIdentifierResult(x));
            }

            if (desc.Accession != null)
            {
                Accession = new List<AccessionResult>();
                foreach (var x in desc.Accession)
                    Accession.Add(new AccessionResult(x));
            }

            if (desc.SOPClass != null)
            {
                SOPClass = new List<SOPClassResult>();
                foreach (var x in desc.SOPClass)
                    SOPClass.Add(new SOPClassResult(x));
            }

            if (desc.MPPS != null)
            {
                ParticipantObjectContainsStudy = new List<ParticipantObjectContainsStudyResult>();
                foreach (var x in desc.ParticipantObjectContainsStudy)
                    ParticipantObjectContainsStudy.Add(new ParticipantObjectContainsStudyResult(x));
            }
        }
    }

    [DataContract]
    public class ObjectIdentifierResult
    {
        [DataMember]
        public string UId { get; set; }

        public ObjectIdentifierResult(ObjectIdentifier id)
        {
            UId = id.UId;
        }
    }

    [DataContract]
    public class AccessionResult
    {
        [DataMember]
        public string Number { get; set; }

        public AccessionResult(Accession acc)
        {
            Number = acc.Number;
        }
    }

    [DataContract]
    public class SOPClassResult
    {
        [DataMember]
        public List<Instance_ExtendedResult> Instance { get; set; }

        [DataMember]
        public string UId { get; set; }

        [DataMember]
        public int NumberOfInstances { get; set; }

        public SOPClassResult(SOPClass sop)
        {
            UId = sop.UId;
            NumberOfInstances = sop.NumberOfInstances;

            if(sop.Instance != null)
            {
                Instance = new List<Instance_ExtendedResult>();
                foreach (var x in sop.Instance)
                    Instance.Add(new Instance_ExtendedResult(x));
            }
        }
    }

    [DataContract]
    public class Instance_ExtendedResult
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string UId { get; set; }

        public Instance_ExtendedResult(Instance_Extended ins)
        {
            FileName = ins.FileName;
            UId = ins.UId;
        }
    }

    [DataContract]
    public class ParticipantObjectContainsStudyResult
    {
        [DataMember]
        public List<ObjectIdentifierResult> StudyIds { get; set; }

        public ParticipantObjectContainsStudyResult(ParticipantObjectContainsStudy study)
        {
            if(study.StudyIds != null)
            {
                StudyIds = new List<ObjectIdentifierResult>();
                foreach (var x in study.StudyIds)
                    StudyIds.Add(new ObjectIdentifierResult(x));
            }
        }
    }
}
