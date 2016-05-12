using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AsiaMoneyer.WebApi.Controllers
{
    public class FaqController : BaseApiController
    {
        Faq.IFaqAppService _faqAppService;

        public FaqController(Faq.IFaqAppService faqAppService)
        {
            this._faqAppService = faqAppService;
        }

        public async Task<IHttpActionResult> GetFaqs()
        {
            bool b = User.IsInRole("Admin");
            var l = await this._faqAppService.GetFaqs();
            return Ok(l);
        }

        public async Task<IHttpActionResult> CreateQuestion(Faq.Dtos.FaqDto faqDto)
        {
            this._faqAppService.UserId = User.Identity.GetUserId();
            await this._faqAppService.createQuestion(faqDto);
            return Ok();
        }

        public async Task<IHttpActionResult> DeletFag(string id)
        {
            this._faqAppService.UserId = User.Identity.GetUserId();
            await this._faqAppService.DeleteFag(id);
            return Ok();
        }
        public async Task<IHttpActionResult> EditQuestion(Faq.Dtos.FaqDto faqDto)
        {
            this._faqAppService.UserId = User.Identity.GetUserId();
            await this._faqAppService.editQuestion(faqDto.Id, faqDto);
            return Ok();
        }
        public async Task<IHttpActionResult> GetFaq(string id)
        {
            var f= await this._faqAppService.GetFaq(id);
            return Ok(f);
        }
    }
}