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
    
    public partial class Instance
    {
        public System.Guid InstanceID { get; set; }
        public System.Guid SOPCLassID { get; set; }
        public string FileName { get; set; }
        public string UID { get; set; }
    
        public virtual SOPClass SOPClass { get; set; }
    }
}