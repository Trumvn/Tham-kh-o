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

    [Table("RecurringTransactions")]
    public partial class RecurringTransaction : Entity<string>
    {
        public RecurringTransaction()
        {
            
        }

        public string ProjectId { get; set; }
        public int TimeFrequencyId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public Nullable<System.DateTime> GeneratedDate { get; set; }
        public Nullable<System.DateTime> GeneratedToDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        [ForeignKey("TimeFrequencyId")]
        public virtual TimeFrequency TimeFrequency { get; set; }
    }
}