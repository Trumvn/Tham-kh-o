using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.UserComments.Dtos
{
    public class UserCommentDto
    {
        public string Id { get; set; }
        public Nullable<int> AppObjectTypeId { get; set; }
        public string UserId { get; set; }
        public string ObjectId { get; set; }
        public System.DateTime CommentDate { get; set; }
        public string CommentText { get; set; }

        public Client.Dtos.UserDto User { get; set; }
    }
}
