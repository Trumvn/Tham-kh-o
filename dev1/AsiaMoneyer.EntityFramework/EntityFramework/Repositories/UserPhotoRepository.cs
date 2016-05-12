using AsiaMoneyer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class UserPhotoRepository : AsiaMoneyerRepositoryBase<Entities.UserPhoto>, IUserPhotoRepository
    {
        public UserPhotoRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public string GetUerPhoto(string userId)
        {
            Entities.UserPhoto userPhoto = this.Get(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            if (userPhoto != null)
                return userPhoto.Photo;
            return null;
        }
    }

}
