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
    
    public partial class ActiveParticipant
    {
        public System.Guid ActiveParticipantID { get; set; }
        public System.Guid LogID { get; set; }
        public string UserID { get; set; }
        public string AlternativeUserID { get; set; }
        public string UserName { get; set; }
        public bool UserIsRequestor { get; set; }
        public Nullable<System.Guid> RoleIDCode { get; set; }
        public string NetworkAccessPointTypeCode { get; set; }
        public string NetworkAccesPointID { get; set; }
    
        public virtual ActorElement ActorElement { get; set; }
        public virtual ValidLog ValidLog { get; set; }
    }
}
