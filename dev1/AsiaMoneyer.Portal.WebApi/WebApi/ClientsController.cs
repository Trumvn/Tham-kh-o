using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AsiaMoneyer.Client.Dtos;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.Portal.WebApi.Controllers
{
    public class ClientsController : BaseApiController
    {
        Client.IClientAppService _clientAppService;

        public ClientsController(Client.IClientAppService clientAppService)
        {
            this._clientAppService = clientAppService;
        }
                     
        public async Task<IHttpActionResult> GetUsers()
        {
            var l = await this._clientAppService.GetUsers();
            return Ok(l);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserProfiles() {
            try
            {
                var l = await this._clientAppService.GetClients();
                return Ok(l);
            }catch(Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public IHttpActionResult CreateUserProfile(ClientDto profileDto) {
            this._clientAppService.CreateClient(profileDto);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SaveUserProfile(ClientDto profileDto) {
            try
            {
                this._clientAppService.UserId = User.Identity.GetUserId();
                this._clientAppService.SaveClient(profileDto);
                return Ok();
            }catch(Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveUserProfileDetail(ClientDto profileDto)
        {
            try
            {
                this._clientAppService.UserId = User.Identity.GetUserId();
                this._clientAppService.SaveClientDetail(profileDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserPhoto(string userId)
        {
            try
            {
                var l = this._clientAppService.GetUserPhoto(userId);
                return Ok(l);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult ChangeCurrentUserPhoto(ClientDto profileDto)
        {
            try
            {
                this._clientAppService.UserId = User.Identity.GetUserId();
                this._clientAppService.ChangeCurrentUserPhoto(profileDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserProfile(string profileId)
        {
            try
            {
                ClientDto clientDto = this._clientAppService.GetClientByProfileId(profileId);
                return Ok(clientDto);

            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}