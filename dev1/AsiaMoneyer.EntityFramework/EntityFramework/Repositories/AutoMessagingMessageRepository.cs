using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Messaging;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.EntityFramework.Repositories
{
    public class AutoMessagingMessageRepository : AsiaMoneyerRepositoryBase<Entities.AutoMessagingMessage>, IAutoMessagingMessageRepository
    {
        public AutoMessagingMessageRepository(IDbContextProvider<AsiaMoneyerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public int CountMessages()
        {
            int count = 0;
            count = this.List.Count(x => x.Id != null);
            return count;
        }

        public List<AutoMessagingMessageModel> GetMessageTitles()
        {
            var query = (from m in DbContext.AutoMessagingMessageDbSet
                             join tc in DbContext.AutoMessagingTemplateContentDbSet on m.AutoMessagingTemplateContentId equals tc.Id
                             join t in DbContext.AutoMessagingTemplateDbSet on tc.AutoMessagingTemplateId equals t.Id
                             orderby m.CreatedDate descending
                             select new AutoMessagingMessageModel{
                                 Id = m.Id,
                                 TemplateName = t.MessagingTemplateName,
                                 MessagingSubject = m.MessagingSubject,
                                 MessagingFromName = m.MessagingFromName,
                                 MessagingFromEmailAddress = m.MessagingFromEmailAddress,
                                 IsSent = m.IsSent,
                                 IsMarkedAsRead = m.IsMarkedAsRead
                             }).Take(20).ToList();

            return query;
        }
    }
}
