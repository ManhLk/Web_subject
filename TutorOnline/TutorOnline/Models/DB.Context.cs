﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TutorOnline.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Detail_Subject_Grade> Detail_Subject_Grade { get; set; }
        public virtual DbSet<EndUser> EndUsers { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Tutor> Tutors { get; set; }
        public virtual DbSet<TutorApplication> TutorApplications { get; set; }
    }
}