using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AsiaMoneyer.Messaging;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.WebApi.Controllers
{
    [Authorize]
    [AppExceptionFilterAttribute]
    public class AutoMessagingController : BaseApiController
    {
        private readonly IAutoMessagingAppService _autoMessagingAppService;
        private readonly IAutoMessagingMessageAppService _autoMessagingMessageAppService;

        public AutoMessagingController(IAutoMessagingAppService autoMessagingAppService, IAutoMessagingMessageAppService autoMessagingMessageAppService)
        {
            this._autoMessagingAppService = autoMessagingAppService;
            this._autoMessagingMessageAppService = autoMessagingMessageAppService;
        }

        public IHttpActionResult SendTestEmail()
        {
            string userId = User.Identity.GetUserId();
            this._autoMessagingAppService.UserId = userId;

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("UserId", userId);
            values.Add("{{binding_text}}", String.Format("<br/><strong>{0}</strong>", userId));


            this._autoMessagingAppService.SendEmail(Commons.Constants.AUTO_EMAIL_TEMPLATE_SYSTEM_ERROR, values);

            //this._autoMessagingAppService.sendEmail("dquanghuy82@gmail.com", "A test email from Aegona.com", String.Format("UserId: {0}", userId));
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetMessagingContent()
        {
            var r = this._autoMessagingMessageAppService.GetMessagingContent();
            return Ok(r);
        }

        [HttpGet]
        public IHttpActionResult GetMessages()
        {
            var r = this._autoMessagingMessageAppService.GetMessageTitles();
            return Ok(r);
        }

        [HttpGet]
        public IHttpActionResult GetTemplateContentTitles(String templateId)
        {
            var r = this._autoMessagingMessageAppService.GetTemplateContentTitles(templateId);
            return Ok(r);
        }

        [HttpGet]
        public IHttpActionResult GetMailMessage(String messageId)
        {
            var r = this._autoMessagingMessageAppService.GetMailMessage(messageId);
            return Ok(r);
        }
        
        [HttpGet]
        public IHttpActionResult GetMailTemplateContent(String contentId)
        {
            var r = this._autoMessagingMessageAppService.GetMailTemplateContent(contentId);
            return Ok(r);
        }
        
        [HttpPost]
        public IHttpActionResult SaveMailTemplateContent(Messaging.Dtos.TemplateContentDto templateContentDto)
        {
            try
            {
                var r = this._autoMessagingMessageAppService.SaveMailTemplateContent(templateContentDto);
                return Ok(r);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}