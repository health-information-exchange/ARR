//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Perceptive.ARR.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParticipantObjectDescription
    {
        public ParticipantObjectDescription()
        {
            this.Accessions = new HashSet<Accession>();
            this.ObjectIdentifiers = new HashSet<ObjectIdentifier>();
            this.ParticipantObjectContainsStudies = new HashSet<ParticipantObjectContainsStudy>();
            this.SOPClasses = new HashSet<SOPClass>();
        }
    
        public System.Guid ParticipantObjectDescriptionID { get; set; }
        public System.Guid ParticipantObjectIdentificationID { get; set; }
        public Nullable<bool> Encrypted { get; set; }
        public Nullable<bool> Anonymized { get; set; }
    
        public virtual ICollection<Accession> Accessions { get; set; }
        public virtual ICollection<ObjectIdentifier> ObjectIdentifiers { get; set; }
        public virtual ICollection<ParticipantObjectContainsStudy> ParticipantObjectContainsStudies { get; set; }
        public virtual ParticipantObjectIdentification ParticipantObjectIdentification { get; set; }
        public virtual ICollection<SOPClass> SOPClasses { get; set; }
    }
}