using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.UserComments
{
    public interface IUserCommentAppService : IAppService
    {
        List<Dtos.UserCommentDto> GetUserComments(int typeId, string objId);
        Dtos.UserCommentDto AddUserComment(int typeId, string objectId, string logText);
    }

}
