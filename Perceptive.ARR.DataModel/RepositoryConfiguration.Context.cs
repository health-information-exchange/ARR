﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PerceptiveARR_ConfigEntities : DbContext
    {
        public PerceptiveARR_ConfigEntities()
            : base("name=PerceptiveARR_ConfigEntities")
        {
        }

        public PerceptiveARR_ConfigEntities(string connectionString)
            : base(connectionString)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<ClientUser> ClientUsers { get; set; }
        public virtual DbSet<SupportedEventType> SupportedEventTypes { get; set; }
        public virtual DbSet<UserActiveDatabase> UserActiveDatabases { get; set; }
        public virtual DbSet<SupportedActorElement> SupportedActorElements { get; set; }
    }
}