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
    
    public partial class SOPClass
    {
        public SOPClass()
        {
            this.Instances = new HashSet<Instance>();
        }
    
        public System.Guid SOPClassID { get; set; }
        public System.Guid ParticipantObjectDescriptionID { get; set; }
        public string UID { get; set; }
        public int NumberOfInstances { get; set; }
    
        public virtual ICollection<Instance> Instances { get; set; }
        public virtual ParticipantObjectDescription ParticipantObjectDescription { get; set; }
    }
}
