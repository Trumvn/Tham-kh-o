using AsiaMoneyer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class ClientRepository : AsiaMoneyerRepositoryBase<Entities.Client>, IClientRepository
    {

        public ClientRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<Entities.Client> GetClients()
        {
            List<Entities.Client> userProfiles = this.List.ToList();
            return userProfiles;
        }

        public List<Entities.UserClientModel> GetClients(int pageIndex, int pageSize)
        {
            var entryPoint = DbContext.ClientDbSet.Join(DbContext.UserDbSet, up => up.UserId, u => u.Id, (up, u) => new {UserProfile = up, User = u})
                .Select(x=> new UserClientModel {
                    Id = x.UserProfile.Id,
                    FirstName = x.UserProfile.FirstName,
                    LastName = x.UserProfile.LastName,
                    EmailAddress = x.UserProfile.EmailAddress,
                    PhoneNumber = x.UserProfile.PhoneNumber,
                    Gender = x.UserProfile.Gender,
                    UserId = x.User.Id,
                    UserName = x.User.UserName,
                    IsActive = x.UserProfile.IsActive,
                    JoinDate = x.User.JoinDate
                })
                .OrderBy(o=>o.FirstName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
            return entryPoint.ToList();
        }
    }
}
