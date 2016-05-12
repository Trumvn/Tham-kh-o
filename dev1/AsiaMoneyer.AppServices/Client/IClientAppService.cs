using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client
{
    public interface IClientAppService : IAppService
    {
        Task<List<Client.Dtos.UserDto>> GetUsers();
        Task<Client.Dtos.UserDto> GetUser(string userId);

        Task<Client.Dtos.GetClientsDto> GetClients();
        Client.Dtos.ClientDto GetClient(string userId);
        Client.Dtos.ClientDto GetClientByProfileId(string profileId);
        string GetClientIdByUserId(string userId);
        String GetUserPhoto(string userId);

        bool CreateClient(Dtos.ClientDto profileDto);
        void SaveClient(Dtos.ClientDto profileDto);
        void ChangeCurrentUserPhoto(Dtos.ClientDto clientDto);
        void SaveClientDetail(Dtos.ClientDto clientDto);
        void UpdateUserProfile(Dtos.ClientDto profileDto);        
    }
}
