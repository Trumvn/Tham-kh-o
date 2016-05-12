using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using AsiaMoneyer.Security.Infrastructure;
using AsiaMoneyer.Account.Dtos;
using AsiaMoneyer.Client.Dtos;

namespace AsiaMoneyer.Portal.WebApi.Controllers
{
    //[RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {

        Client.IClientAppService _clientAppService;

        public AccountsController(Client.IClientAppService clientAppService)
        {
            this._clientAppService = clientAppService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [Authorize]
        public IHttpActionResult IsAuthenticated()
        {
            string i = User.Identity.GetUserId();
            return Ok();
        }

        //[Authorize]
        //[Authorize(Roles="Admin")]
        //[Route("users")]
        public IHttpActionResult GetUsers()
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        //[Authorize(Roles = "Admin")]
        //[Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        //[Authorize(Roles = "Admin")]
        //[Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [AllowAnonymous]
        //[Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Username.ToLower(),
                    Email = createUserModel.Email.ToLower(),
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName,
                    JoinDate = DateTime.Now,
                };

                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }

                AppGroupManager.SetUserGroups(user.Id, Commons.Constants.IDENTITY_GROUP_USERS);

                var clientDto = new ClientDto()
                {
                    UserId = user.Id,
                    EmailAddress = user.Email,
                    FirstName = user.FirstName,
                    IsActive = true,
                    IsDeleted = false
                };
                this._clientAppService.CreateClient(clientDto);

                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, String.Format("{0}, {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : String.Empty));
            }
        }

        //[Authorize]
        //[Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        //[Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = await this.AppUserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                //TODO  Do we reveal that the user does not exist or is not confirmed???
                ModelState.AddModelError("", "Email has not been existed");
                return BadRequest(ModelState);
            }

            var code = await this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id);
            code = HttpUtility.UrlEncode(code);

            string callbackUrl = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port + "/#/reset_password";
            callbackUrl += "?userId=" + user.Id;
            callbackUrl += "&code=" + code;

            await AppUserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        //[Route("ResetPasswordConfirmation", Name = "ConfirmResetPasswordRoute")]
        public async Task<IHttpActionResult> ResetPasswordConfirm(ResetPasswordBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some errors occur.");
                return BadRequest(ModelState);
            }
            var user = await AppUserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok();
            }
            var code = HttpUtility.UrlDecode(model.Code).Replace(" ", "+");
            var result = await AppUserManager.ResetPasswordAsync(user.Id, code, model.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Reset failed.");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        //[Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        //[Authorize(Roles="Admin")]
        //[Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();

        }

        //[Authorize(Roles = "Admin")]
        //[Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        //[Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> GetCurrentUserProfile()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var user = await this.AppUserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    ClientDto clientDto = this._clientAppService.GetClient(userId);
                    return Ok(clientDto);
                }

            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCurrentUserProfile(ClientDto clientDto)
        {
            string userId = User.Identity.GetUserId();
            var user = await this.AppUserManager.FindByIdAsync(userId);

            if (user != null && clientDto != null)
            {
                try
                {
                    this._clientAppService.UserId = user.Id;
                    this._clientAppService.SaveClient(clientDto);

                    return Ok(clientDto);
                }
                catch (Exception ex)
                {
                    return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return NotFound();
        }

        [Authorize]
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

    }
}