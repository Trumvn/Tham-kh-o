using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer.Entities
{
    [Table("FrequentlyAskedQuestions")]
    public class FrequentlyAskedQuestion : Entity<string>, Base.IAuditable
    {
        public string Lang { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Question { get; set; }
        public string AssistantName { get; set; }
        public string Answer { get; set; }
        public string Tags { get; set; }
        public int Voting { get; set; }
        public int DisplayOrder { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<bool> IsPublish { get; set; }
    }
}
