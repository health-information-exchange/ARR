using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    public class ActorElement : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlAttribute("codeSystemName")]
        public string CodeSystemName { get; set; }

        [XmlAttribute("displayName")]
        public string DisplayName { get; set; }        

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            base.PopulateDefaultData(propertyName);

            switch (Actor)
            {
                case MessageType.ApplicationActivity:
                    CodeSystemName = "DCM";

                    switch(propertyName)
                    {
                        case "EventId":
                            Code = "110100";
                            DisplayName = "Application Activity";
                            break;
                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110150";
                                DisplayName = "Application";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                            {
                                Code = "110151";
                                DisplayName = "Application Launcher";
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case MessageType.PDQConsumerV2:
                case MessageType.PDQConsumerV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110112";
                            CodeSystemName = "DCM";
                            DisplayName = "Query";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PDQConsumerV2) ? "ITI-21" : "ITI-47";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Patient Demographics Query";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;
                            
                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Query)
                            {
                                Code = (Actor == MessageType.PDQConsumerV2) ? "ITI-21" : "ITI-47";
                                CodeSystemName = "IHE Transactions";
                                DisplayName = "Patient Demographics Query";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110107";
                            CodeSystemName = "DCM";
                            DisplayName = "Import";
                            break;

                        case "EventTypeCode":
                            Code = "ITI-43";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Retrieve Document Set";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Document)
                            {
                                Code = "9";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Report Number";
                            }
                            break;

                        default:
                            break;
                    }
                    break;
                    
                case MessageType.PIXSourceV2:
                case MessageType.PIXSourceV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110110";
                            CodeSystemName = "DCM";
                            DisplayName = "Patient Record";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PIXSourceV2) ? "ITI-8" : "ITI-44";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Patient Identity Feed";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.PIXConsumerV2:
                case MessageType.PIXConsumerV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110112";
                            CodeSystemName = "DCM";
                            DisplayName = "Query";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PIXConsumerV2) ? "ITI-9" : "ITI-45";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "PIX Query";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Query)
                            {
                                Code = (Actor == MessageType.PIXConsumerV2) ? "ITI-9" : "ITI-45";
                                CodeSystemName = "IHE Transactions";
                                DisplayName = "PIX Query";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.PIXConsumerUpdateNotificationV2:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110110";
                            CodeSystemName = "DCM";
                            DisplayName = "Patient Record";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PIXConsumerUpdateNotificationV2 ) ? "ITI-10" : "ITI-46";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "PIX Update Notification";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if(SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.DocSourcePHIExport:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110106";
                            CodeSystemName = "DCM";
                            DisplayName = "Export";
                            break;

                        case "EventTypeCode":
                            Code = "ITI-41";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Provide and Register Document Set-b";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.SubmissionSet)
                            {
                                Code = "urn:uuid:a54d6aa5-d40d-43f9-88c5-b4633d873bdd";
                                CodeSystemName = "IHE XDS Metadata";
                                DisplayName = "submission set classificationNode";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.NodeAuthenticationFailure:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110113";
                            CodeSystemName = "DCM";
                            DisplayName = "Security Alert";
                            break;

                        case "EventTypeCode":
                            Code = "110126";
                            CodeSystemName = "DCM";
                            DisplayName = "Node Authentication";
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.DocConsumerRegistryStoredQuery:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110112";
                            CodeSystemName = "DCM";
                            DisplayName = "Query";
                            break;

                        case "EventTypeCode":
                            Code = "ITI-18";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Registry Stored Query";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Query)
                            {
                                Code = "ITI-18";
                                CodeSystemName = "IHE Transactions";
                                DisplayName = "Registry Stored Query";
                            }
                            break;


                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public class ActorElementDicom : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("csd-code")]
        public string Code { get; set; }

        [XmlAttribute("codeSystemName")]
        public string CodeSystemName { get; set; }

        [XmlAttribute("originalText")]
        public string DisplayName { get; set; }

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            base.PopulateDefaultData(propertyName);

            switch (Actor)
            {
                case MessageType.ApplicationActivity:
                    CodeSystemName = "DCM";

                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110100";
                            DisplayName = "Application Activity";
                            break;
                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110150";
                                DisplayName = "Application";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.HumanRequestor)
                            {
                                Code = "110151";
                                DisplayName = "Application Launcher";
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case MessageType.PDQConsumerV2:
                case MessageType.PDQConsumerV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110112";
                            CodeSystemName = "DCM";
                            DisplayName = "Query";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PDQConsumerV2) ? "ITI-21" : "ITI-47";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Patient Demographics Query";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Query)
                            {
                                Code = (Actor == MessageType.PDQConsumerV2) ? "ITI-21" : "ITI-47";
                                CodeSystemName = "IHE Transactions";
                                DisplayName = "Patient Demographics Query";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110107";
                            CodeSystemName = "DCM";
                            DisplayName = "Import";
                            break;

                        case "EventTypeCode":
                            Code = "ITI-43";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Retrieve Document Set";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Document)
                            {
                                Code = "9";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Report Number";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.PIXSourceV2:
                case MessageType.PIXSourceV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110110";
                            CodeSystemName = "DCM";
                            DisplayName = "Patient Record";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PIXSourceV2) ? "ITI-8" : "ITI-44";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Patient Identity Feed";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.PIXConsumerV2:
                case MessageType.PIXConsumerV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110112";
                            CodeSystemName = "DCM";
                            DisplayName = "Query";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PIXConsumerV2) ? "ITI-9" : "ITI-45";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "PIX Query";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Query)
                            {
                                Code = (Actor == MessageType.PIXConsumerV2) ? "ITI-9" : "ITI-45";
                                CodeSystemName = "IHE Transactions";
                                DisplayName = "PIX Query";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.PIXConsumerUpdateNotificationV2:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110110";
                            CodeSystemName = "DCM";
                            DisplayName = "Patient Record";
                            break;

                        case "EventTypeCode":
                            Code = (Actor == MessageType.PIXConsumerUpdateNotificationV2) ? "ITI-10" : "ITI-46";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "PIX Update Notification";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.DocSourcePHIExport:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110106";
                            CodeSystemName = "DCM";
                            DisplayName = "Export";
                            break;

                        case "EventTypeCode":
                            Code = "ITI-41";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Provide and Register Document Set-b";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.SubmissionSet)
                            {
                                Code = "urn:uuid:a54d6aa5-d40d-43f9-88c5-b4633d873bdd";
                                CodeSystemName = "IHE XDS Metadata";
                                DisplayName = "submission set classificationNode";
                            }
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.NodeAuthenticationFailure:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110113";
                            CodeSystemName = "DCM";
                            DisplayName = "Security Alert";
                            break;

                        case "EventTypeCode":
                            Code = "110126";
                            CodeSystemName = "DCM";
                            DisplayName = "Node Authentication";
                            break;

                        default:
                            break;
                    }
                    break;

                case MessageType.DocConsumerRegistryStoredQuery:
                    switch (propertyName)
                    {
                        case "EventId":
                            Code = "110112";
                            CodeSystemName = "DCM";
                            DisplayName = "Query";
                            break;

                        case "EventTypeCode":
                            Code = "ITI-18";
                            CodeSystemName = "IHE Transactions";
                            DisplayName = "Registry Stored Query";
                            break;

                        case "RoleIdCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Source)
                            {
                                Code = "110153";
                                CodeSystemName = "DCM";
                                DisplayName = "Source";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Destination)
                            {
                                Code = "110152";
                                CodeSystemName = "DCM";
                                DisplayName = "Destination";
                            }
                            break;

                        case "ParticipantObjectIdTypeCode":
                            if (SectionType == IHE.AuditTrail.SectionType.Patient)
                            {
                                Code = "2";
                                CodeSystemName = "RFC-3881";
                                DisplayName = "Patient Number";
                            }
                            else if (SectionType == IHE.AuditTrail.SectionType.Query)
                            {
                                Code = "ITI-18";
                                CodeSystemName = "IHE Transactions";
                                DisplayName = "Registry Stored Query";
                            }
                            break;


                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public class ObjectDetailElement : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlIgnore]
        public ObjectDetailType DetailType { get; set; }

        # endregion

        protected override void PopulateDefaultData(string propertyName)
        {
            switch (Actor)
            {
                case MessageType.PDQConsumerV2:
                case MessageType.PIXConsumerV2:
                    if (SectionType == IHE.AuditTrail.SectionType.Query && DetailType == ObjectDetailType.MSH10)
                        Type = "MSH-10";
                    break;

                case MessageType.DocConsumerRetrieveDocumentSetImport:
                    if (SectionType == IHE.AuditTrail.SectionType.Document)
                    {
                        if (DetailType == ObjectDetailType.RepositoryUniqueId)
                            Type = "Repository Unique Id";
                        else if (DetailType == ObjectDetailType.HomeCommunityId)
                            Type = "ihe:homeCommunityID";
                    }
                    break;

                case MessageType.PIXSourceV2:
                case MessageType.PIXConsumerUpdateNotificationV2:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient && DetailType == ObjectDetailType.MSH10)
                        Type = "MSH-10";
                    break;

                case MessageType.PIXSourceV3:
                case MessageType.PIXConsumerUpdateNotificationV3:
                    if (SectionType == IHE.AuditTrail.SectionType.Patient && DetailType == ObjectDetailType.II)
                        Type = "II";
                    break;

                default:
                    break;
            }

            base.PopulateDefaultData(propertyName);
        }
    }
}
