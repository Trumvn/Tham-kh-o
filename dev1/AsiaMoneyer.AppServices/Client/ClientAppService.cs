using AsiaMoneyer.AuditLog;
using AsiaMoneyer.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client
{
    public class ClientAppService: AppServiceBase, IClientAppService 
    {

        private readonly IAuditLogAppService _auditLogAppService;

        private readonly IEventPublisher _eventPublisher;

        public ClientAppService(IAuditLogAppService auditLogAppService, IEventPublisher eventPublisher)
        {
            this._auditLogAppService = auditLogAppService;

            this._eventPublisher = eventPublisher;
        }

        #region User
        public Task<List<Dtos.UserDto>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<Dtos.UserDto> GetUser(string userId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Client
        public string GetClientIdByUserId(string userId)
        {
            var clientId = this.UnitOfWork.ClientRepository.List.Where(x => x.UserId == userId).Select(s=>s.Id).FirstOrDefault();
            if (clientId != null)
                return Convert.ToString(clientId);
            return String.Empty;
        }

        public Dtos.ClientDto GetClient(string userId)
        {
            Entities.Client client = this.UnitOfWork.ClientRepository.List.Where(x=>x.UserId == userId).FirstOrDefault();
            Dtos.ClientDto clientDto = AutoMapper.Mapper.Map<Entities.Client, Client.Dtos.ClientDto>(client);

            Entities.UserPhoto userPhoto = this.UnitOfWork.UserPhotoRepository.List.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (userPhoto != null)
            {
                clientDto.Photo = userPhoto.Photo;
            }

            return clientDto;
        }

        public String GetUserPhoto(string userId)
        {
            String base64Photo = this.UnitOfWork.UserPhotoRepository.List.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedDate).Select(s => s.Photo).FirstOrDefault();
            return base64Photo;
        }

        public bool CreateClient(Dtos.ClientDto clientDto)
        {
            try
            {
                // Check existing created in system by invite member (to another project)
                var client = this.UnitOfWork.ClientRepository.List.Where(x => x.EmailAddress == clientDto.EmailAddress).FirstOrDefault();

                if (client == null)
                {
                    client = new Entities.Client()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = clientDto.UserId,
                        FirstName = clientDto.FirstName,
                        EmailAddress = clientDto.EmailAddress,
                        IsActive = clientDto.IsActive,
                        IsDeleted = clientDto.IsDeleted,
                        CreatedDate = DateTime.Now
                    };
                    this.UnitOfWork.ClientRepository.Add(client);
                }
                else
                {
                    client.UserId = clientDto.UserId;
                    client.FirstName = clientDto.FirstName;
                    client.LastName = clientDto.LastName;

                    this.UnitOfWork.ClientRepository.Update(client, x => x.FirstName, x => x.LastName, x => x.UserId);
                }
            
                this.UnitOfWork.Save(clientDto.UserId);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder stb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {

                    stb.AppendLine(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stb.AppendLine(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(stb.ToString());
            }

            return true;
        }

        public async Task<Dtos.GetClientsDto> GetClients()
        {
            Dtos.GetClientsDto userProfiles = new Dtos.GetClientsDto();
            List<Entities.UserClientModel> profiles = this.UnitOfWork.ClientRepository.GetClients(0, 20);
            
            foreach(Entities.UserClientModel userClient in profiles)
            {
                userClient.Photo = this.UnitOfWork.UserPhotoRepository.GetUerPhoto(userClient.UserId);
            }

            userProfiles.UserProfiles = AutoMapper.Mapper.Map<List<Entities.UserClientModel>, List<Client.Dtos.ClientDto>>(profiles);
                        
            return await Task.FromResult(userProfiles);
           
        }

        public Dtos.ClientDto GetClientByProfileId(string profileId)
        {
            Entities.Client client = this.UnitOfWork.ClientRepository.List.Where(x => x.Id == profileId).FirstOrDefault();
            Dtos.ClientDto clientDto = AutoMapper.Mapper.Map<Entities.Client, Client.Dtos.ClientDto>(client);

            Entities.UserPhoto userPhoto = this.UnitOfWork.UserPhotoRepository.List.Where(x => x.UserId == client.UserId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (userPhoto != null)
            {
                clientDto.Photo = userPhoto.Photo;
            }

            return clientDto;

        }

        public void ChangeCurrentUserPhoto(Dtos.ClientDto clientDto)
        {
            var userPhoto = new Entities.UserPhoto()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = clientDto.UserId,
                Photo = clientDto.Photo,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };

            this.UnitOfWork.UserPhotoRepository.Add(userPhoto);

            this.UnitOfWork.Save(this.UserId);
        }

        public void SaveClient(Dtos.ClientDto clientDto)
        {
            if (clientDto != null)
            {
                var client = new Entities.Client()
                {
                    Id = clientDto.Id,
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName
                };

                var clientUser = new Entities.User()
                {
                    Id = clientDto.UserId,
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName
                };

                this.UnitOfWork.ClientRepository.Update(client, u=>u.FirstName, u=>u.LastName);
                this.UnitOfWork.UserRepository.Update(clientUser, u => u.FirstName, u => u.LastName);

                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public void SaveClientDetail(Dtos.ClientDto clientDto)
        {
            if (clientDto != null)
            {
                var client = new Entities.Client()
                {
                    Id = clientDto.Id,
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    Birthday = clientDto.Birthday,
                    IsActive = clientDto.IsActive,
                    Gender = clientDto.Gender
                };

                this.UnitOfWork.ClientRepository.Update(client, u => u.FirstName, u => u.LastName, u=>u.Birthday, u=>u.Gender, u=>u.IsActive);

                this.UnitOfWork.Save(this.UserId);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public void UpdateUserProfile(Dtos.ClientDto profileDto)
        {
            if(profileDto == null)
            {
                throw new ArgumentNullException("Profile cannot be null");
            }

            var client = new Entities.Client()
            {
                Id = profileDto.Id,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName
            };

            var clientUser = new Entities.User()
            {
                Id = profileDto.UserId,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName
            };

            this.UnitOfWork.ClientRepository.Update(client, u => u.FirstName, u => u.LastName);
            this.UnitOfWork.UserRepository.Update(clientUser, u => u.FirstName, u => u.LastName);

            this.UnitOfWork.Save(this.UserId);

        }
        #endregion        
    }
}
