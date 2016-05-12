using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AsiaMoneyer.Client.Dtos;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.WebApi.Controllers
{
    [Authorize]
    public class ClientsController : BaseApiController
    {
        Client.IClientAppService _clientAppService;
        Client.ISubscriptionAppService _subscriptionAppService;

        public ClientsController(Client.IClientAppService clientAppService, Client.ISubscriptionAppService subscriptionAppService)
        {
            this._clientAppService = clientAppService;
            this._subscriptionAppService = subscriptionAppService;
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

        [HttpPost]
        public IHttpActionResult GetUserCurrentSubscription()
        {
            try
            {
                this._subscriptionAppService.UserId = User.Identity.GetUserId();
                SubscriptionDto subscriptiontDto = this._subscriptionAppService.GetUserCurrentSubscription(this._subscriptionAppService.UserId);
                return Ok(subscriptiontDto);

            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }            
        }

        [HttpPost]
        public IHttpActionResult GetUserUpgradeProducts()
        {
            try
            {
                this._subscriptionAppService.UserId = User.Identity.GetUserId();
                List<ProductDto> products = this._subscriptionAppService.GetUserUpgradeProducts(this._subscriptionAppService.UserId);
                return Ok(products);

            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}