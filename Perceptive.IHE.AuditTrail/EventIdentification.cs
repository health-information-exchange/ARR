using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    public class EventIdentification : XmlSectionBase
    {
        # region Properties

        [XmlElement("EventID")]
        public ActorElement EventId { get; set; }

        [XmlAttribute("EventActionCode")]
        public string EventActionCode { get; set; }

        [XmlAttribute("EventDateTime")]
        public string EventDateTime { get; set; }

        [XmlAttribute("EventOutcomeIndicator")]
        public string EventOutcomeIndicator { get; set; }

        [XmlElement("EventTypeCode")]
        public ActorElement EventTypeCode { get; set; }

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            EventDateTime = DateTime.Now.ToUniversalTime().ToString(AuditTrail.DATETIME_FORMAT);
            EventId = new ActorElement();
            EventTypeCode = new ActorElement();

            switch (Actor)
            {
                case MessageType.ApplicationActivity:
                case MessageType.NodeAuthenticationFailure:
                case MessageType.DocConsumerRegistryStoredQuery:
                case MessageType.PDQConsumerV2:
                case MessageType.PDQConsumerV3:
                case MessageType.PIXConsumerV2:
                case MessageType.PIXConsumerV3:
                    EventActionCode = "E";
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                    EventActionCode = "C";
                    break;

                case MessageType.PIXConsumerUpdateNotificationV2:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    EventActionCode = "U";
                    break;

                case MessageType.DocSourcePHIExport:
                    EventActionCode = "R";
                    break;

                default:
                    break;
            }

            base.PopulateDefaultData(propertyName);
        }
    }
}
