using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Faq
{
    public interface IFaqRepository : IRepository<AsiaMoneyer.Entities.FrequentlyAskedQuestion, string>
    {
        List<AsiaMoneyer.Entities.FrequentlyAskedQuestion> GetFaqs();
    }
}
