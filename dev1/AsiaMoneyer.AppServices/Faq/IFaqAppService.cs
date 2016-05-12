using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Faq.Dtos;

namespace AsiaMoneyer.Faq
{
    public interface IFaqAppService : IAppService
    {
        Task<List<Dtos.FaqDto>> GetFaqs();
        Task createQuestion(Dtos.FaqDto faqDto);

        Task DeleteFag(string id);
        Task editQuestion(string id, FaqDto faqDto);
        Task<Dtos.FaqDto> GetFaq(string id);
    }
}
