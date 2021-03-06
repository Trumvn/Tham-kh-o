//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AsiaMoneyer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProjectUserProfiles")]
    public partial class ProjectUserProfile:Entity<int>
    {
        public ProjectUserProfile()
        {
        }

        public int ProjectId { get; set; }
        public int UserProfileId { get; set; }

        public Nullable<int> PermissionId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
