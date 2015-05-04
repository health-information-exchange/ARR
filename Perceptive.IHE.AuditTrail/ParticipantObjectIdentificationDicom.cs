using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    public class ParticipantObjectIdentificationDicom : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("ParticipantObjectTypeCode")]
        public int ParticipantObjectTypeCode { get; set; }

        [XmlAttribute("ParticipantObjectTypeCodeRole")]
        public string ParticipantObjectTypeCodeRole { get; set; }

        [XmlAttribute("ParticipantObjectDataLifeCycle")]
        public string ParticipantObjectDataLifeCycle { get; set; }

        [XmlElement("ParticipantObjectIDTypeCode")]
        public ActorElementDicom ParticipantObjectIdTypeCode { get; set; }

        [XmlAttribute("ParticipantObjectSensitivity")]
        public string ParticipantObjectSensitivity { get; set; }

        [XmlAttribute("ParticipantObjectID")]
        public string ParticipantObjectId { get; set; }

        [XmlAttribute("ParticipantObjectName")]
        public string ParticipantObjectName { get; set; }

        [XmlElement("ParticipantObjectQuery")]
        public string ParticipantObjectQuery { get; set; }

        [XmlElement("ParticipantObjectDetail")]
        public List<ObjectDetailElement> ParticipantObjectDetail { get; set; }

        # region Dicom Optional Fields

        [XmlElement("ParticipantObjectDescription")]
        public List<ParticipantObjectDescription> ParticipantObjectDescription { get; set; }

        # endregion

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            ParticipantObjectDetail = new List<ObjectDetailElement>();
            switch (Actor)
            {
                case MessageType.PDQConsumerV2:
                case MessageType.PDQConsumerV3:
                case MessageType.PIXConsumerV2:
                case MessageType.PIXConsumerV3:
                case MessageType.DocConsumerRegistryStoredQuery:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient)
                    {
                        ParticipantObjectTypeCode = 1;
                        ParticipantObjectTypeCodeRole = "1";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.Query)
                    {
                        ParticipantObjectTypeCode = 2;
                        ParticipantObjectTypeCodeRole = "24";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                        if (Actor == MessageType.PDQConsumerV2 || Actor == MessageType.PIXConsumerV2)
                            ParticipantObjectDetail.Add(new ObjectDetailElement() { DetailType = ObjectDetailType.MSH10 });
                    }
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient)
                    {
                        ParticipantObjectTypeCode = 1;
                        ParticipantObjectTypeCodeRole = "1";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.Document)
                    {
                        ParticipantObjectTypeCode = 2;
                        ParticipantObjectTypeCodeRole = "3";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                        ParticipantObjectDetail.Add(new ObjectDetailElement() { DetailType = ObjectDetailType.RepositoryUniqueId });
                        ParticipantObjectDetail.Add(new ObjectDetailElement() { DetailType = ObjectDetailType.HomeCommunityId });
                    }
                    break;

                case MessageType.PIXSourceV2:
                case MessageType.PIXSourceV3:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient)
                    {
                        ParticipantObjectTypeCode = 1;
                        ParticipantObjectTypeCodeRole = "1";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                        ParticipantObjectDetail.Add(new ObjectDetailElement()
                        {
                            DetailType = (Actor == MessageType.PIXSourceV2) ? ObjectDetailType.MSH10 : ObjectDetailType.II
                        });
                    }
                    break;

                case MessageType.PIXConsumerUpdateNotificationV2:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient)
                    {
                        ParticipantObjectTypeCode = 1;
                        ParticipantObjectTypeCodeRole = "1";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                        ParticipantObjectDetail.Add(new ObjectDetailElement()
                        {
                            DetailType = (Actor == MessageType.PIXConsumerUpdateNotificationV2) ? ObjectDetailType.MSH10 : ObjectDetailType.II
                        });
                    }
                    break;

                case MessageType.DocSourcePHIExport:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient)
                    {
                        ParticipantObjectTypeCode = 1;
                        ParticipantObjectTypeCodeRole = "1";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.SubmissionSet)
                    {
                        ParticipantObjectTypeCode = 2;
                        ParticipantObjectTypeCodeRole = "20";
                        ParticipantObjectIdTypeCode = new ActorElementDicom();
                    }
                    break;

                default:
                    break;
            }
            base.PopulateDefaultData(propertyName);
        }
    }
}
