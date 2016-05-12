using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaMoneyer.Entities;

namespace AsiaMoneyer.Messaging
{
    public interface IAutoMessagingTemplateContentRepository : IRepository<AsiaMoneyer.Entities.AutoMessagingTemplateContent, string>
    {
        List<TemplateContentModel> GetTemplateContentTitles(String templateId);
        int CountTemplateContent(String templateId);
    }
}
