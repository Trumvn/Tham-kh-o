using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class TimeFrequencyDto
    {
        public int Id { get; set; }
        public string TimeFrequencyTitle { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<byte> Weeks { get; set; }
        public int SortOrder { get; set; }
    }
}
