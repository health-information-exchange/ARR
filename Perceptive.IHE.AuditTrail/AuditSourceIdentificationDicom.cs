using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;

namespace Perceptive.IHE.AuditTrail
{
    public class AuditSourceIdentificationDicom : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("AuditSourceID")]
        public string AuditSourceId { get; set; }

        [XmlAttribute("AuditEnterpriseSiteID")]
        public string AuditEnterpriseSiteId { get; set; }

        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlAttribute("codeSystemName")]
        public string CodeSystemName { get; set; }

        [XmlAttribute("originalText")]
        public string OriginalText { get; set; }

       

        protected override void PopulateDefaultData(string propertyName)
        {
            AuditSourceId = string.Format(CultureInfo.InvariantCulture, "{0}@{1}", Actor.ToString(), Environment.MachineName);
            base.PopulateDefaultData(propertyName);
        }

        # endregion
    }
}
