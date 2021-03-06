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

    [Table("Projects")]
    public partial class Project : Entity<string>, Base.IAuditable
    {    
        public Project()
        {
            this.ProjectMembers = new HashSet<ProjectMember>();
        }

        public string ProjectTitle { get; set; }
        public string ProjectDesc { get; set; }
        public string HighlightColor { get; set; }
        public string WorkingEmail { get; set; }
        public string Currency { get; set; }
        public byte FinanceYearStartMonth { get; set; }
        public byte FinanceYearMonths { get; set; }
        public Nullable<bool> IsPrivate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
