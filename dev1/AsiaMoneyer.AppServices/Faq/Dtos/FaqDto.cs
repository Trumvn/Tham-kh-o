using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Faq.Dtos
{
    public class FaqDto
    {
        public string Id { get; set; }
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
        public bool? IsPublish { get; set; }

        public bool IsCollapse { get; set; }

        public FaqDto()
        {
            IsCollapse = true;
        }
    }
}
