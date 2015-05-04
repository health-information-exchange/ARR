using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    public class ActiveParticipantDicom : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("UserID")]
        public string UserId { get; set; }

        [XmlAttribute("AlternativeUserID")]
        public string AlternativeUserId { get; set; }

        [XmlAttribute("UserName")]
        public string UserName { get; set; }

        [XmlAttribute("UserIsRequestor")]
        public bool UserIsRequestor { get; set; }

        [XmlElement("RoleIDCode")]
        public ActorElementDicom RoleIdCode { get; set; }

        [XmlAttribute("NetworkAccessPointTypeCode")]
        public string NetworkAccessPointTypeCode { get; set; }

        [XmlAttribute("NetworkAccessPointID")]
        public string NetworkAccessPointId { get; set; }

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                UserId = Environment.UserName;

            switch (Actor)
            {
                case MessageType.ApplicationActivity:
                    UserIsRequestor = true;
                    NetworkAccessPointTypeCode = "2";
                    NetworkAccessPointId = AuditTrail.LocalIPAddress;
                    RoleIdCode = new ActorElementDicom();
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                    if (SectionType == IHE.AuditTrail.SectionType.Source)
                    {
                        UserIsRequestor = false;
                        NetworkAccessPointTypeCode = "2";
                        // this should be source's(repository) ip but not sure how to find it or even if its important
                        NetworkAccessPointId = AuditTrail.LocalIPAddress;
                        RoleIdCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                    {
                        AlternativeUserId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                        UserIsRequestor = true;
                        NetworkAccessPointTypeCode = "2";
                        NetworkAccessPointId = AuditTrail.LocalIPAddress;
                        RoleIdCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                        UserIsRequestor = true;
                    break;

                case MessageType.PIXSourceV2:
                case MessageType.PIXSourceV3:
                case MessageType.PDQConsumerV2:
                case MessageType.PDQConsumerV3:
                case MessageType.PIXConsumerV2:
                case MessageType.PIXConsumerV3:
                case MessageType.DocConsumerRegistryStoredQuery:
                    if (SectionType == IHE.AuditTrail.SectionType.Source)
                    {
                        AlternativeUserId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                        UserIsRequestor = true;
                        NetworkAccessPointTypeCode = "2";
                        NetworkAccessPointId = AuditTrail.LocalIPAddress;
                        RoleIdCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                        UserIsRequestor = true;
                    else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                    {
                        UserIsRequestor = false;
                        NetworkAccessPointTypeCode = "2";
                        RoleIdCode = new ActorElementDicom();
                    }
                    break;

                case MessageType.PIXConsumerUpdateNotificationV2:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    if (SectionType == IHE.AuditTrail.SectionType.Source)
                    {
                        UserIsRequestor = true;
                        NetworkAccessPointTypeCode = "2";
                        RoleIdCode = new ActorElementDicom();
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                    {
                        AlternativeUserId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                        UserIsRequestor = false;
                        NetworkAccessPointTypeCode = "2";
                        NetworkAccessPointId = AuditTrail.LocalIPAddress;
                        RoleIdCode = new ActorElementDicom();
                    }
                    break;

                case MessageType.DocSourcePHIExport:
                    if (SectionType == IHE.AuditTrail.SectionType.Source)
                    {
                        AlternativeUserId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                        UserIsRequestor = true;
                        RoleIdCode = new ActorElementDicom();
                        NetworkAccessPointTypeCode = "2";
                        NetworkAccessPointId = AuditTrail.LocalIPAddress;
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                    {
                        UserIsRequestor = true;
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                    {
                        UserIsRequestor = false;
                        RoleIdCode = new ActorElementDicom();
                        NetworkAccessPointTypeCode = "2";
                    }
                    break;

                case MessageType.NodeAuthenticationFailure:
                    if (SectionType == IHE.AuditTrail.SectionType.Source)
                    {
                        NetworkAccessPointTypeCode = "2";
                        NetworkAccessPointId = AuditTrail.LocalIPAddress;
                        UserIsRequestor = true;
                    }
                    else if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                        UserIsRequestor = true;
                    break;

                default:
                    break;
            }

            base.PopulateDefaultData(propertyName);
        }
    }
}
