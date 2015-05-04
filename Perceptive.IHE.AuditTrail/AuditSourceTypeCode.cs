using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Perceptive.IHE.AuditTrail
{
    public class AuditSourceTypeCode : XmlSectionBase
    {
        # region Properties

        [XmlAttribute("code")]
        public string Code { get; set; }        

        # endregion
    }
}
