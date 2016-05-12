using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.UserComments;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class UserCommentRepository : AsiaMoneyerRepositoryBase<Entities.UserComment>, IUserCommentRepository
    {
        public UserCommentRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        #region Override Methods
        public List<AsiaMoneyer.Entities.UserComment> GetUserComments(int typeId, string objId)
        {
            List<Entities.UserComment> userComments = this.Get(x => x.ObjectId == objId && x.AppObjectTypeId == typeId).OrderByDescending(o=>o.CommentDate).ToList();
            return userComments;
        }

        #endregion
    }
}
