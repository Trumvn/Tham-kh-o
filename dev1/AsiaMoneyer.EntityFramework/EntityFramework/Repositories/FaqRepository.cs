using AsiaMoneyer.Faq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class FaqRepository : AsiaMoneyerRepositoryBase<Entities.FrequentlyAskedQuestion>, IFaqRepository
    {

        public FaqRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
    
        }

        public List<Entities.FrequentlyAskedQuestion> GetFaqs()
        {
            List<Entities.FrequentlyAskedQuestion> faqs = this.List.ToList();
            return faqs;
        }
    }
}
