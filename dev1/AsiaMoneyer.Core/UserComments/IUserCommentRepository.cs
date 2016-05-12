using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.UserComments
{
    public interface IUserCommentRepository : IRepository<AsiaMoneyer.Entities.UserComment, string>
    {
        List<AsiaMoneyer.Entities.UserComment> GetUserComments(int typeId, string objId);
    }
}
