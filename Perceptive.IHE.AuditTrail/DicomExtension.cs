using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    public class SOPClass : XmlSectionBase
    {
        # region Properties

        [XmlElement("Instance")]
        public List<Instance_Extended> Instance { get; set; }

        [XmlAttribute("UID")]
        public string UId { get; set; }

        [XmlAttribute("NumberOfInstances")]
        public int NumberOfInstances { get; set; }

        # endregion
    }

    public class Instance_Extended : ObjectIdentifier
    {
        # region Properties

        [XmlAttribute("FileName")]
        public string FileName { get; set; }

        # endregion
    }

    public class Accession : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("Number")]
        public string Number { get; set; }

        # endregion
    }

    public class ObjectIdentifier : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("UID")]
        public string UId { get; set; }

        # endregion
    }

    public class ParticipantObjectContainsStudy : XmlSectionBase
    {
        # region Properties

        [XmlElement("StudyIDs")]
        public List<ObjectIdentifier> StudyIds { get; set; }

        # endregion
    }

    public class ParticipantObjectDescription : XmlSectionBase
    {
        # region Properties

        [XmlElement("MPPS")]
        public List<ObjectIdentifier> MPPS { get; set; }

        [XmlElement("Accession")]
        public List<Accession> Accession { get; set; }

        [XmlElement("SOPClass")]
        public List<SOPClass> SOPClass { get; set; }        

        [XmlElement("ParticipantObjectContainsStudy")]
        public List<ParticipantObjectContainsStudy> ParticipantObjectContainsStudy { get; set; }

        [XmlIgnore]
        public bool? Encrypted { get; set; }

        [XmlElement("Encrypted")]
        public string EncryptedAsText
        {
            get { return Encrypted.HasValue ? Encrypted.ToString() : null; }
            set { Encrypted = !string.IsNullOrEmpty(value) ? bool.Parse(value) : default(bool?); }
        }

        [XmlIgnore]
        public bool? Anonymized { get; set; }

        [XmlElement("Anonymized")]
        public string AnonymizedAsText
        {
            get { return Anonymized.HasValue ? Anonymized.ToString() : null; }
            set { Anonymized = !string.IsNullOrEmpty(value) ? bool.Parse(value) : default(bool?); }
        }

        # endregion
    }
}
