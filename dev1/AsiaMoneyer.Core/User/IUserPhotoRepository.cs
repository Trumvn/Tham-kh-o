using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.User
{
    public interface IUserPhotoRepository : IRepository<AsiaMoneyer.Entities.UserPhoto, string>
    {
        string GetUerPhoto(string userId);
    }
}
