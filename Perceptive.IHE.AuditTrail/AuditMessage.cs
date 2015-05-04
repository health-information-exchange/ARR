using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    [XmlRoot("AuditMessage")]
    [Serializable]
    public class AuditMessage : XmlSectionBase
    {
        # region Properties

        [XmlElement("EventIdentification")]
        public EventIdentification Event { get; set; }

        [XmlElement("ActiveParticipant")]
        public List<ActiveParticipant> ActiveParticipants { get; set; }

        [XmlElement("AuditSourceIdentification")]
        public AuditSourceIdentification AuditSource { get; set; }

        [XmlElement("ParticipantObjectIdentification")]
        public List<ParticipantObjectIdentification> ParticipantObjects { get; set; }

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            switch(Actor)
            {
                case MessageType.ApplicationActivity:                    
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Source });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.HumanRequestor });
                    break;
                    
                case MessageType.PDQConsumerV2:
                case MessageType.PDQConsumerV3:
                case MessageType.PIXConsumerV2:
                case MessageType.PIXConsumerV3:
                case MessageType.DocConsumerRegistryStoredQuery:
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Source });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.HumanRequestor });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Destination });
                    ParticipantObjects.Add(new ParticipantObjectIdentification() { SectionType = IHE.AuditTrail.SectionType.Query });
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                case MessageType.PIXSourceV2:
                case MessageType.PIXSourceV3:
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Source });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.HumanRequestor });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Destination });
                    break;

                case MessageType.PIXConsumerUpdateNotificationV2:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Source });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Destination });
                    break;

                case MessageType.DocSourcePHIExport:
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Source });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.HumanRequestor });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Destination });
                    ParticipantObjects.Add(new ParticipantObjectIdentification() { SectionType = IHE.AuditTrail.SectionType.SubmissionSet });
                    break;

                case MessageType.NodeAuthenticationFailure:
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.Source });
                    ActiveParticipants.Add(new ActiveParticipant() { SectionType = IHE.AuditTrail.SectionType.HumanRequestor });
                    break;

                default:
                    break;
            }

            base.PopulateDefaultData(propertyName);
        }

        public void PopulateDefaultValues(int patientCount = 0, int documentCount = 0)
        {
            Event = new EventIdentification();
            AuditSource = new AuditSourceIdentification();
            ActiveParticipants = new List<ActiveParticipant>();
            ParticipantObjects = new List<ParticipantObjectIdentification>();

            for (int i = 0; i < patientCount; i++)
                ParticipantObjects.Add(new ParticipantObjectIdentification() { SectionType = IHE.AuditTrail.SectionType.Patient });

            for (int i = 0; i < documentCount; i++)
                ParticipantObjects.Add(new ParticipantObjectIdentification() { SectionType = IHE.AuditTrail.SectionType.Document });

            PopulateDefaultData(string.Empty);
        }
    }
}
