//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TutorApplication
    {
        public int RequirementId { get; set; }
        public int TutorId { get; set; }
        public Nullable<int> ApplicationStatus { get; set; }
    
        public virtual Requirement Requirement { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}