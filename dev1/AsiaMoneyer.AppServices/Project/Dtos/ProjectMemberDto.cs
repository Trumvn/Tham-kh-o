using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Project.Dtos
{
    public class ProjectMemberDto
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public Client.Dtos.ClientDto Client { get; set; }

        public string ProjectId { get; set; }
        public int PermissionId { get; set; }
        public string HighlightColor { get; set; }
        public bool IsArchived { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsFavorite { get; set; }

        public int? ViewFilterByType { get; set; }
        public string ViewFilterByAccount { get; set; }
        public string ViewFilterByCategory { get; set; }

        public Nullable<System.DateTime> ViewFilterFromDate { get; set; }
        public Nullable<System.DateTime> ViewFilterEndDate { get; set; }


        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
