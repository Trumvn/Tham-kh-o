using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Messaging;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AutoMessagingTemplateContentRepository : AsiaMoneyerRepositoryBase<AutoMessagingTemplateContent>, IAutoMessagingTemplateContentRepository
    {
        public AutoMessagingTemplateContentRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public List<TemplateContentModel> GetTemplateContentTitles(String templateId)
        {
            var query = (from tc in DbContext.AutoMessagingTemplateContentDbSet
                         join t in DbContext.AutoMessagingTemplateDbSet on tc.AutoMessagingTemplateId equals t.Id
                         where tc.AutoMessagingTemplateId == templateId
                         orderby tc.CreatedDate descending
                         select new TemplateContentModel { 
                             Id = tc.Id,
                             MessagingFromName = tc.MessagingFromName,
                             MessagingFromEmailAddress = tc.MessagingFromEmailAddress,
                             MessagingSubject = tc.MessagingSubject,
                             MessagingTo = tc.MessagingTo,
                             Lang = tc.Lang,
                             FromDate = tc.FromDate,
                             EndDate = tc.EndDate,
                             IsPublish = tc.IsPublish,
                             CreatedDate = tc.CreatedDate
                         }).Take(20).ToList();
            return query;
        }

        public int CountTemplateContent(String templateId)
        {
            int count = 0;
            count = this.List.Count(x => x.AutoMessagingTemplateId == templateId);
            return count;
        }

    }
}
